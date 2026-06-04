using GestionDecanat.DAL;

namespace GestionDecanat.Forms
{
    public class AnneesAcademiquesForm : BaseDataForm
    {
        private readonly AnneeAcademiqueRepository repo = new AnneeAcademiqueRepository(); private readonly System.Windows.Forms.TextBox txtLibelle; private readonly System.Windows.Forms.CheckBox chkActive;
        public AnneesAcademiquesForm() : base("Gestion des années académiques") { Label("Libellé", 10, 18); txtLibelle = TextBox(140, 15); chkActive = Check("Année active", 330, 15); System.Windows.Forms.Button active = new System.Windows.Forms.Button { Text = "Définir active", Left = 500, Top = 12, Width = 120 }; active.Click += delegate { SafeAction(() => repo.SetActive(SelectedId("idAnnee"))); }; editor.Controls.Add(active); }
        protected override void RefreshGrid() { grid.DataSource = repo.GetAll(); }
        protected override void Search() { grid.DataSource = repo.Search(txtSearch.Text, "libelle"); }
        protected override void AddRecord() { repo.Insert(repo.Parameters(txtLibelle.Text.Trim(), chkActive.Checked)); if (chkActive.Checked) repo.SetActive(SelectedId("idAnnee")); }
        protected override void UpdateRecord() { repo.Update(SelectedId("idAnnee"), repo.Parameters(txtLibelle.Text.Trim(), chkActive.Checked)); if (chkActive.Checked) repo.SetActive(SelectedId("idAnnee")); }
        protected override void DeleteRecord() { ConfirmDelete(() => repo.Delete(SelectedId("idAnnee"))); }
        protected override void LoadSelected() { if (grid.CurrentRow != null) { txtLibelle.Text = grid.CurrentRow.Cells["libelle"].Value.ToString(); chkActive.Checked = System.Convert.ToBoolean(grid.CurrentRow.Cells["estActive"].Value); } }
    }
}
