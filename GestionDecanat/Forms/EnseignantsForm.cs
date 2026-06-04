using GestionDecanat.DAL;
namespace GestionDecanat.Forms
{
    public class EnseignantsForm : BaseDataForm
    {
        private readonly EnseignantRepository repo=new EnseignantRepository(); private readonly System.Windows.Forms.TextBox nom,postnom,prenom,telephone,specialite;
        public EnseignantsForm():base("Gestion des enseignants"){ Label("Nom",10,18); nom=TextBox(90,15); Label("Postnom",280,18); postnom=TextBox(360,15); Label("Prénom",550,18); prenom=TextBox(630,15); Label("Téléphone",10,58); telephone=TextBox(90,55); Label("Spécialité",280,58); specialite=TextBox(360,55,260); }
        protected override void RefreshGrid(){grid.DataSource=repo.GetAll();} protected override void Search(){grid.DataSource=repo.Search(txtSearch.Text,"nom","postnom","prenom","specialite");}
        protected override void AddRecord(){repo.Insert(repo.Parameters(nom.Text,postnom.Text,prenom.Text,telephone.Text,specialite.Text));} protected override void UpdateRecord(){repo.Update(SelectedId("idEnseignant"),repo.Parameters(nom.Text,postnom.Text,prenom.Text,telephone.Text,specialite.Text));} protected override void DeleteRecord(){ConfirmDelete(()=>repo.Delete(SelectedId("idEnseignant")));}
        protected override void LoadSelected(){ if(grid.CurrentRow!=null){nom.Text=grid.CurrentRow.Cells["nom"].Value.ToString();postnom.Text=grid.CurrentRow.Cells["postnom"].Value.ToString();prenom.Text=grid.CurrentRow.Cells["prenom"].Value.ToString();telephone.Text=grid.CurrentRow.Cells["telephone"].Value.ToString();specialite.Text=grid.CurrentRow.Cells["specialite"].Value.ToString();}}
    }
}
