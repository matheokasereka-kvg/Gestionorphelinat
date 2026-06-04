using GestionDecanat.DAL;

namespace GestionDecanat.Forms
{
    public class UtilisateursForm : BaseDataForm
    {
        private readonly UtilisateurRepository repo = new UtilisateurRepository();
        private readonly System.Windows.Forms.TextBox username;
        private readonly System.Windows.Forms.TextBox password;
        private readonly System.Windows.Forms.ComboBox role;

        public UtilisateursForm() : base("Gestion des utilisateurs")
        {
            Label("Utilisateur", 10, 18); username = TextBox(110, 15, 180);
            Label("Mot de passe", 310, 18); password = TextBox(420, 15, 180); password.PasswordChar = '*';
            Label("Rôle", 620, 18); role = Combo(680, 15, 180);
            role.Items.AddRange(new object[] { "Administrateur", "Agent Décanat", "Enseignant" });
        }

        protected override void RefreshGrid() { grid.DataSource = repo.GetAll(); }
        protected override void Search() { grid.DataSource = repo.Search(txtSearch.Text, "username", "role"); }
        protected override void AddRecord() { repo.Insert(repo.Parameters(username.Text.Trim(), password.Text, role.Text)); }
        protected override void UpdateRecord() { repo.Update(SelectedId("idUser"), repo.Parameters(username.Text.Trim(), password.Text, role.Text)); }
        protected override void DeleteRecord() { ConfirmDelete(() => repo.Delete(SelectedId("idUser"))); }
        protected override void LoadSelected()
        {
            if (grid.CurrentRow == null) return;
            username.Text = grid.CurrentRow.Cells["username"].Value.ToString();
            password.Text = grid.CurrentRow.Cells["password"].Value.ToString();
            role.Text = grid.CurrentRow.Cells["role"].Value.ToString();
        }
    }
}
