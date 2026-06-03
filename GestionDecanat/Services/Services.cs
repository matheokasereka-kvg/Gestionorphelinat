using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using GestionDecanat.DAL;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GestionDecanat.Services
{
    public class CurrentUser { public int IdUser { get; set; } public string Username { get; set; } public string Role { get; set; } }

    public class AuthService
    {
        private readonly UtilisateurRepository repository = new UtilisateurRepository();
        public CurrentUser Login(string username, string password)
        {
            DataTable table = repository.Login(username, password);
            if (table.Rows.Count == 0) return null;
            DataRow row = table.Rows[0];
            return new CurrentUser { IdUser = Convert.ToInt32(row["idUser"]), Username = row["username"].ToString(), Role = row["role"].ToString() };
        }
    }

    public class AcademicResult
    {
        public decimal TotalPoints { get; set; }
        public decimal TotalCoefficients { get; set; }
        public decimal Moyenne { get; set; }
        public string Decision { get { return Moyenne >= 10 ? "Admis" : "Ajourné"; } }
    }

    public class AcademicService
    {
        public AcademicResult CalculateResult(int idEtudiant)
        {
            const string sql = "SELECT SUM(n.note*c.coefficient) AS totalPoints, SUM(c.coefficient) AS totalCoefficients FROM Notes n INNER JOIN Cours c ON c.idCours=n.idCours WHERE n.idEtudiant=@idEtudiant";
            using (SqlConnection cn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@idEtudiant", idEtudiant);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable table = new DataTable();
                    da.Fill(table);
                    decimal total = table.Rows.Count > 0 && table.Rows[0]["totalPoints"] != DBNull.Value ? Convert.ToDecimal(table.Rows[0]["totalPoints"]) : 0;
                    decimal coeff = table.Rows.Count > 0 && table.Rows[0]["totalCoefficients"] != DBNull.Value ? Convert.ToDecimal(table.Rows[0]["totalCoefficients"]) : 0;
                    return new AcademicResult { TotalPoints = total, TotalCoefficients = coeff, Moyenne = coeff == 0 ? 0 : Math.Round(total / coeff, 2) };
                }
            }
        }

        public DataTable GetClassement()
        {
            const string sql = @"SELECT e.idEtudiant,e.matricule,e.nom,e.postnom,e.prenom,
                CAST(CASE WHEN SUM(c.coefficient)=0 THEN 0 ELSE SUM(n.note*c.coefficient)/SUM(c.coefficient) END AS DECIMAL(10,2)) AS moyenne,
                CASE WHEN CASE WHEN SUM(c.coefficient)=0 THEN 0 ELSE SUM(n.note*c.coefficient)/SUM(c.coefficient) END >= 10 THEN 'Admis' ELSE 'Ajourné' END AS decision
                FROM Etudiants e LEFT JOIN Notes n ON n.idEtudiant=e.idEtudiant LEFT JOIN Cours c ON c.idCours=n.idCours
                GROUP BY e.idEtudiant,e.matricule,e.nom,e.postnom,e.prenom ORDER BY moyenne DESC";
            using (SqlConnection cn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable table = new DataTable();
                da.Fill(table);
                table.Columns.Add("rang", typeof(int));
                for (int i = 0; i < table.Rows.Count; i++) table.Rows[i]["rang"] = i + 1;
                table.Columns["rang"].SetOrdinal(0);
                return table;
            }
        }
    }

    public class StatisticsService
    {
        public DataTable GetGeneralStatistics()
        {
            const string sql = @"SELECT 'Etudiants' AS indicateur, COUNT(*) AS valeur FROM Etudiants
                UNION ALL SELECT 'Admis', COUNT(*) FROM (SELECT e.idEtudiant, CASE WHEN SUM(n.note*c.coefficient)/NULLIF(SUM(c.coefficient),0) >= 10 THEN 1 ELSE 0 END ok FROM Etudiants e LEFT JOIN Notes n ON n.idEtudiant=e.idEtudiant LEFT JOIN Cours c ON c.idCours=n.idCours GROUP BY e.idEtudiant) r WHERE ok=1
                UNION ALL SELECT 'Ajournés', COUNT(*) FROM (SELECT e.idEtudiant, CASE WHEN ISNULL(SUM(n.note*c.coefficient)/NULLIF(SUM(c.coefficient),0),0) < 10 THEN 1 ELSE 0 END ko FROM Etudiants e LEFT JOIN Notes n ON n.idEtudiant=e.idEtudiant LEFT JOIN Cours c ON c.idCours=n.idCours GROUP BY e.idEtudiant) r WHERE ko=1";
            using (SqlConnection cn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable table = new DataTable();
                da.Fill(table);
                return table;
            }
        }

        public DataTable GetRepartitionByFaculte()
        {
            const string sql = @"SELECT f.nomFaculte, COUNT(DISTINCT i.idEtudiant) AS nombre FROM Facultes f LEFT JOIN Promotions p ON p.idFaculte=f.idFaculte LEFT JOIN Inscriptions i ON i.idPromotion=p.idPromotion GROUP BY f.nomFaculte";
            using (SqlConnection cn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable table = new DataTable();
                da.Fill(table);
                return table;
            }
        }
    }

    public class ReportService
    {
        private readonly NoteRepository notes = new NoteRepository();
        private readonly AcademicService academic = new AcademicService();

        public void GenerateBulletinPdf(int idEtudiant, string filePath)
        {
            DataTable noteTable = notes.GetByEtudiant(idEtudiant);
            AcademicResult result = academic.CalculateResult(idEtudiant);
            string etudiant = noteTable.Rows.Count == 0 ? "Etudiant" : noteTable.Rows[0]["etudiant"].ToString();
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("Bulletin individuel - Gestion Académique de Décanat").FontSize(18).Bold().FontColor(Colors.Blue.Medium);
                    page.Content().Column(col =>
                    {
                        col.Item().Text("Etudiant : " + etudiant).FontSize(13).Bold();
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c => { c.RelativeColumn(3); c.RelativeColumn(); c.RelativeColumn(); });
                            table.Header(h => { h.Cell().Text("Cours").Bold(); h.Cell().Text("Coefficient").Bold(); h.Cell().Text("Note").Bold(); });
                            foreach (DataRow row in noteTable.Rows)
                            {
                                table.Cell().Text(row["nomCours"].ToString());
                                table.Cell().Text(row["coefficient"].ToString());
                                table.Cell().Text(row["note"].ToString());
                            }
                        });
                        col.Item().Text("Total des points : " + result.TotalPoints);
                        col.Item().Text("Moyenne pondérée : " + result.Moyenne);
                        col.Item().Text("Décision finale : " + result.Decision).Bold();
                    });
                    page.Footer().AlignCenter().Text(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                });
            }).GeneratePdf(filePath);
        }

        public void GenerateDataTablePdf(string title, DataTable table, string filePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(25);
                    page.Header().Text(title).FontSize(17).Bold().FontColor(Colors.Blue.Medium);
                    page.Content().Table(t =>
                    {
                        t.ColumnsDefinition(c => { foreach (DataColumn ignored in table.Columns) c.RelativeColumn(); });
                        t.Header(h => { foreach (DataColumn column in table.Columns) h.Cell().Text(column.ColumnName).Bold(); });
                        foreach (DataRow row in table.Rows) foreach (DataColumn column in table.Columns) t.Cell().Text(row[column].ToString());
                    });
                    page.Footer().AlignCenter().Text("Généré par GestionDecanat");
                });
            }).GeneratePdf(filePath);
        }
    }
}
