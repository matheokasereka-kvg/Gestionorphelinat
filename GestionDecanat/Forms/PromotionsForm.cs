using GestionDecanat.DAL;

namespace GestionDecanat.Forms
{
    public class PromotionsForm : BaseDataForm
    {
        private readonly PromotionRepository repo = new PromotionRepository(); private readonly FaculteRepository facRepo = new FaculteRepository(); private readonly System.Windows.Forms.TextBox txtNom; private readonly System.Windows.Forms.ComboBox cboFaculte;
        public PromotionsForm() : base("Gestion des promotions") { Label("Promotion", 10, 18); txtNom = TextBox(140, 15); Label("Faculté", 330, 18); cboFaculte = Combo(430, 15, 240); BindCombo(cboFaculte, facRepo.GetAll(), "idFaculte", "nomFaculte"); }
        protected override void RefreshGrid() { grid.DataSource = repo.GetAll(); }
        protected override void Search() { grid.DataSource = repo.Search(txtSearch.Text, "nomPromotion"); }
        protected override void AddRecord() { repo.Insert(repo.Parameters(txtNom.Text.Trim(), ToInt(cboFaculte))); }
        protected override void UpdateRecord() { repo.Update(SelectedId("idPromotion"), repo.Parameters(txtNom.Text.Trim(), ToInt(cboFaculte))); }
        protected override void DeleteRecord() { ConfirmDelete(() => repo.Delete(SelectedId("idPromotion"))); }
        protected override void LoadSelected() { if (grid.CurrentRow != null) { txtNom.Text = grid.CurrentRow.Cells["nomPromotion"].Value.ToString(); cboFaculte.SelectedValue = grid.CurrentRow.Cells["idFaculte"].Value; } }
    }
}
