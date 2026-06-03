using GestionDecanat.DAL;
namespace GestionDecanat.Forms
{
    public class AttributionsCoursForm : BaseDataForm
    {
        private readonly AttributionCoursRepository repo=new AttributionCoursRepository(); private readonly CoursRepository coursRepo=new CoursRepository(); private readonly EnseignantRepository ensRepo=new EnseignantRepository(); private readonly System.Windows.Forms.ComboBox cours,enseignant;
        public AttributionsCoursForm():base("Gestion des attributions de cours"){ Label("Cours",10,18); cours=Combo(100,15,270); Label("Enseignant",390,18); enseignant=Combo(500,15,270); BindCombo(cours,coursRepo.GetAll(),"idCours","nomCours"); BindCombo(enseignant,ensRepo.GetAll(),"idEnseignant","nom"); }
        protected override void RefreshGrid(){grid.DataSource=repo.GetAll();} protected override void Search(){grid.DataSource=repo.GetAll();}
        protected override void AddRecord(){repo.Insert(repo.Parameters(ToInt(cours),ToInt(enseignant)));} protected override void UpdateRecord(){repo.Update(SelectedId("idAttribution"),repo.Parameters(ToInt(cours),ToInt(enseignant)));} protected override void DeleteRecord(){ConfirmDelete(()=>repo.Delete(SelectedId("idAttribution")));}
        protected override void LoadSelected(){ if(grid.CurrentRow!=null){cours.SelectedValue=grid.CurrentRow.Cells["idCours"].Value; enseignant.SelectedValue=grid.CurrentRow.Cells["idEnseignant"].Value;}}
    }
}
