using System;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GestionDecanat.Services;

namespace GestionDecanat.Forms
{
    public class StatistiquesForm : Form
    {
        private readonly StatisticsService service=new StatisticsService(); private readonly DataGridView grid=new DataGridView(); private readonly Chart chart=new Chart();
        public StatistiquesForm(){ Text="Statistiques académiques"; Width=1000; Height=620; StartPosition=FormStartPosition.CenterScreen; Button refresh=new Button{Text="Actualiser",Dock=DockStyle.Top,Height=35}; grid.Dock=DockStyle.Left; grid.Width=360; grid.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill; chart.Dock=DockStyle.Fill; chart.ChartAreas.Add(new ChartArea("main")); chart.Series.Add(new Series("Répartition"){ChartType=SeriesChartType.Column}); Controls.Add(chart); Controls.Add(grid); Controls.Add(refresh); refresh.Click+=delegate{LoadStats();}; Load+=delegate{LoadStats();}; }
        private void LoadStats(){ try{ DataTable general=service.GetGeneralStatistics(); grid.DataSource=general; DataTable rep=service.GetRepartitionByFaculte(); chart.Series[0].Points.Clear(); foreach(DataRow row in rep.Rows) chart.Series[0].Points.AddXY(row["nomFaculte"].ToString(),Convert.ToInt32(row["nombre"])); }catch(Exception ex){MessageBox.Show(ex.Message);} }
    }
}
