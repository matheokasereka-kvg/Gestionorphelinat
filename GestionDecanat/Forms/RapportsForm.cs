using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using GestionDecanat.DAL;
using GestionDecanat.Services;

namespace GestionDecanat.Forms
{
    public class RapportsForm : Form
    {
        private readonly ComboBox cboEtudiant=new ComboBox(); private readonly DataGridView grid=new DataGridView(); private readonly ReportService reports=new ReportService(); private readonly AcademicService academic=new AcademicService(); private DataTable current;
        public RapportsForm(){ Text="Rapports académiques"; Width=1000; Height=620; StartPosition=FormStartPosition.CenterScreen; FlowLayoutPanel top=new FlowLayoutPanel{Dock=DockStyle.Top,Height=45}; Button bulletin=Btn("Bulletin individuel"); Button classement=Btn("Classement"); Button etudiants=Btn("Liste étudiants"); Button admis=Btn("Liste admis"); Button ajournes=Btn("Liste ajournés"); Button pdf=Btn("Exporter PDF"); cboEtudiant.Width=180; cboEtudiant.DropDownStyle=ComboBoxStyle.DropDownList; cboEtudiant.DataSource=new EtudiantRepository().GetAll(); cboEtudiant.ValueMember="idEtudiant"; cboEtudiant.DisplayMember="matricule"; top.Controls.Add(cboEtudiant); top.Controls.Add(bulletin); top.Controls.Add(classement); top.Controls.Add(etudiants); top.Controls.Add(admis); top.Controls.Add(ajournes); top.Controls.Add(pdf); grid.Dock=DockStyle.Fill; grid.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill; Controls.Add(grid); Controls.Add(top); bulletin.Click+=delegate{SaveBulletin();}; classement.Click+=delegate{current=academic.GetClassement(); grid.DataSource=current;}; etudiants.Click+=delegate{current=new EtudiantRepository().GetAll(); grid.DataSource=current;}; admis.Click+=delegate{FilterDecision("Admis");}; ajournes.Click+=delegate{FilterDecision("Ajourné");}; pdf.Click+=delegate{ExportPdf();}; }
        private Button Btn(string text){return new Button{Text=text,Width=115,Height=30};}
        private void FilterDecision(string decision){ DataTable c=academic.GetClassement(); DataView v=c.DefaultView; v.RowFilter="decision='"+decision.Replace("'","''")+"'"; current=v.ToTable(); grid.DataSource=current; }
        private void SaveBulletin(){ try{ string path=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"Bulletin_"+cboEtudiant.Text+".pdf"); reports.GenerateBulletinPdf(Convert.ToInt32(cboEtudiant.SelectedValue),path); MessageBox.Show("Bulletin généré : "+path);}catch(Exception ex){MessageBox.Show(ex.Message);} }
        private void ExportPdf(){ try{ if(current==null){MessageBox.Show("Sélectionnez d'abord un rapport.");return;} string path=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"Rapport_GestionDecanat.pdf"); reports.GenerateDataTablePdf("Rapport académique",current,path); MessageBox.Show("PDF généré : "+path);}catch(Exception ex){MessageBox.Show(ex.Message);} }
    }
}
