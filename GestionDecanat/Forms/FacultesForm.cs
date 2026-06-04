using GestionDecanat.DAL;

namespace GestionDecanat.Forms
{
    public class FacultesForm : BaseDataForm
    {
        private readonly FaculteRepository repo = new FaculteRepository(); private readonly System.Windows.Forms.TextBox txtNom;
        public FacultesForm() : base("Gestion des facultés") { Label("Nom faculté", 10, 18); txtNom = TextBox(140, 15, 260); }
        protected override void RefreshGrid() { grid.DataSource = repo.GetAll(); }
        protected override void Search() { grid.DataSource = repo.Search(txtSearch.Text, "nomFaculte"); }
        protected override void AddRecord() { repo.Insert(repo.Parameters(txtNom.Text.Trim())); }
        protected override void UpdateRecord() { repo.Update(SelectedId("idFaculte"), repo.Parameters(txtNom.Text.Trim())); }
        protected override void DeleteRecord() { ConfirmDelete(() => repo.Delete(SelectedId("idFaculte"))); }
        protected override void LoadSelected() { if (grid.CurrentRow != null) txtNom.Text = grid.CurrentRow.Cells["nomFaculte"].Value.ToString(); }
    }
}
