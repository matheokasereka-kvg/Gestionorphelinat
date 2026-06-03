using GestionDecanat.DAL;
namespace GestionDecanat.Forms
{
    public class InscriptionsForm : BaseDataForm
    {
        private readonly InscriptionRepository repo=new InscriptionRepository(); private readonly EtudiantRepository etuRepo=new EtudiantRepository(); private readonly PromotionRepository promRepo=new PromotionRepository(); private readonly AnneeAcademiqueRepository anneeRepo=new AnneeAcademiqueRepository(); private readonly System.Windows.Forms.ComboBox etudiant,promotion,annee;
        public InscriptionsForm():base("Gestion des inscriptions"){ Label("Étudiant",10,18); etudiant=Combo(100,15,260); Label("Promotion",380,18); promotion=Combo(470,15,220); Label("Année",710,18); annee=Combo(770,15,170); BindCombo(etudiant,etuRepo.GetAll(),"idEtudiant","matricule"); BindCombo(promotion,promRepo.GetAll(),"idPromotion","nomPromotion"); BindCombo(annee,anneeRepo.GetAll(),"idAnnee","libelle"); }
        protected override void RefreshGrid(){grid.DataSource=repo.GetAll();} protected override void Search(){grid.DataSource=repo.Search(txtSearch.Text,"idEtudiant");}
        protected override void AddRecord(){repo.Insert(repo.Parameters(ToInt(etudiant),ToInt(promotion),ToInt(annee)));} protected override void UpdateRecord(){repo.Update(SelectedId("idInscription"),repo.Parameters(ToInt(etudiant),ToInt(promotion),ToInt(annee)));} protected override void DeleteRecord(){ConfirmDelete(()=>repo.Delete(SelectedId("idInscription")));}
        protected override void LoadSelected(){ if(grid.CurrentRow!=null){etudiant.SelectedValue=grid.CurrentRow.Cells["idEtudiant"].Value; promotion.SelectedValue=grid.CurrentRow.Cells["idPromotion"].Value; annee.SelectedValue=grid.CurrentRow.Cells["idAnnee"].Value;}}
    }
}
