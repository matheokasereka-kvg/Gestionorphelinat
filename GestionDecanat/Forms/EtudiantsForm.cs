using GestionDecanat.DAL;

namespace GestionDecanat.Forms
{
    public class EtudiantsForm : BaseDataForm
    {
        private readonly EtudiantRepository repo = new EtudiantRepository(); private readonly System.Windows.Forms.TextBox matricule, nom, postnom, prenom, adresse, telephone; private readonly System.Windows.Forms.ComboBox sexe; private readonly System.Windows.Forms.DateTimePicker naissance;
        public EtudiantsForm() : base("Gestion des étudiants") { Label("Matricule",10,18); matricule=TextBox(100,15); Label("Nom",290,18); nom=TextBox(350,15); Label("Postnom",540,18); postnom=TextBox(620,15); Label("Prénom",10,58); prenom=TextBox(100,55); Label("Sexe",290,58); sexe=Combo(350,55); sexe.Items.AddRange(new object[]{"M","F"}); Label("Naissance",540,58); naissance=DateBox(620,55); Label("Adresse",10,98); adresse=TextBox(100,95,300); Label("Téléphone",430,98); telephone=TextBox(520,95); }
        protected override void RefreshGrid(){ grid.DataSource=repo.GetAll(); }
        protected override void Search(){ grid.DataSource=repo.Search(txtSearch.Text,"matricule","nom","postnom","prenom"); }
        protected override void AddRecord(){ repo.Insert(repo.Parameters(matricule.Text,nom.Text,postnom.Text,prenom.Text,sexe.Text,naissance.Value,adresse.Text,telephone.Text)); }
        protected override void UpdateRecord(){ repo.Update(SelectedId("idEtudiant"),repo.Parameters(matricule.Text,nom.Text,postnom.Text,prenom.Text,sexe.Text,naissance.Value,adresse.Text,telephone.Text)); }
        protected override void DeleteRecord(){ ConfirmDelete(()=>repo.Delete(SelectedId("idEtudiant"))); }
        protected override void LoadSelected(){ if(grid.CurrentRow!=null){ matricule.Text=grid.CurrentRow.Cells["matricule"].Value.ToString(); nom.Text=grid.CurrentRow.Cells["nom"].Value.ToString(); postnom.Text=grid.CurrentRow.Cells["postnom"].Value.ToString(); prenom.Text=grid.CurrentRow.Cells["prenom"].Value.ToString(); sexe.Text=grid.CurrentRow.Cells["sexe"].Value.ToString(); naissance.Value=System.Convert.ToDateTime(grid.CurrentRow.Cells["dateNaissance"].Value); adresse.Text=grid.CurrentRow.Cells["adresse"].Value.ToString(); telephone.Text=grid.CurrentRow.Cells["telephone"].Value.ToString(); } }
    }
}
