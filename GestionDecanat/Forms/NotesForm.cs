using System;
using GestionDecanat.DAL;
using GestionDecanat.Services;
namespace GestionDecanat.Forms
{
    public class NotesForm : BaseDataForm
    {
        private readonly NoteRepository repo=new NoteRepository(); private readonly EtudiantRepository etuRepo=new EtudiantRepository(); private readonly CoursRepository coursRepo=new CoursRepository(); private readonly AcademicService academic=new AcademicService(); private readonly System.Windows.Forms.ComboBox etudiant,cours; private readonly System.Windows.Forms.TextBox note; private readonly System.Windows.Forms.Label result;
        public NotesForm():base("Gestion des notes"){ Label("Étudiant",10,18); etudiant=Combo(100,15,250); Label("Cours",370,18); cours=Combo(430,15,250); Label("Note /20",700,18); note=TextBox(780,15,80); result=new System.Windows.Forms.Label{Left=10,Top=60,Width=800,Height=40}; editor.Controls.Add(result); BindCombo(etudiant,etuRepo.GetAll(),"idEtudiant","matricule"); BindCombo(cours,coursRepo.GetAll(),"idCours","nomCours"); etudiant.SelectedIndexChanged+=delegate{ShowResult();}; }
        protected override void RefreshGrid(){grid.DataSource=repo.GetAll(); ShowResult();} protected override void Search(){ if(ToInt(etudiant)>0) grid.DataSource=repo.GetByEtudiant(ToInt(etudiant)); }
        protected override void AddRecord(){repo.Insert(repo.Parameters(ToInt(etudiant),ToInt(cours),ToDecimal(note),DateTime.Now));} protected override void UpdateRecord(){repo.Update(SelectedId("idNote"),repo.Parameters(ToInt(etudiant),ToInt(cours),ToDecimal(note),DateTime.Now));} protected override void DeleteRecord(){ConfirmDelete(()=>repo.Delete(SelectedId("idNote")));}
        protected override void LoadSelected(){ if(grid.CurrentRow!=null){etudiant.SelectedValue=grid.CurrentRow.Cells["idEtudiant"].Value; cours.SelectedValue=grid.CurrentRow.Cells["idCours"].Value; note.Text=grid.CurrentRow.Cells["note"].Value.ToString();}}
        private void ShowResult(){ try{ if(ToInt(etudiant)>0){AcademicResult r=academic.CalculateResult(ToInt(etudiant)); result.Text="Total points: "+r.TotalPoints+" | Coefficients: "+r.TotalCoefficients+" | Moyenne: "+r.Moyenne+" | Décision: "+r.Decision;}}catch{} }
    }
}
