using System;
using System.Drawing;
using System.Windows.Forms;
using GestionDecanat.Services;

namespace GestionDecanat.Forms
{
    public class DashboardForm : Form
    {
        private readonly CurrentUser currentUser;
        public DashboardForm(CurrentUser user)
        {
            currentUser = user;
            Text = "Tableau de bord - " + currentUser.Role;
            WindowState = FormWindowState.Maximized;
            BuildMenu();
            Label welcome = new Label { Dock = DockStyle.Fill, Text = "Bienvenue " + currentUser.Username + "\nSystème de Gestion Académique de Décanat", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 24F, FontStyle.Bold) };
            Controls.Add(welcome);
        }
        private void BuildMenu()
        {
            MenuStrip menu = new MenuStrip();
            Add(menu, "Facultés", () => new FacultesForm().ShowDialog());
            Add(menu, "Promotions", () => new PromotionsForm().ShowDialog());
            Add(menu, "Années académiques", () => new AnneesAcademiquesForm().ShowDialog());
            Add(menu, "Étudiants", () => new EtudiantsForm().ShowDialog());
            Add(menu, "Inscriptions", () => new InscriptionsForm().ShowDialog());
            Add(menu, "Enseignants", () => new EnseignantsForm().ShowDialog());
            Add(menu, "Cours", () => new CoursForm().ShowDialog());
            Add(menu, "Attributions", () => new AttributionsCoursForm().ShowDialog());
            Add(menu, "Notes", () => new NotesForm().ShowDialog());
            Add(menu, "Rapports", () => new RapportsForm().ShowDialog());
            Add(menu, "Statistiques", () => new StatistiquesForm().ShowDialog());
            Add(menu, "Utilisateurs", () => new UtilisateursForm().ShowDialog());
            if (currentUser.Role == "Enseignant")
            {
                foreach (ToolStripMenuItem item in menu.Items) item.Enabled = item.Text == "Notes" || item.Text == "Rapports";
            }
            if (currentUser.Role != "Administrateur") menu.Items[menu.Items.Count - 1].Enabled = false;
            MainMenuStrip = menu; Controls.Add(menu);
        }
        private void Add(MenuStrip menu, string text, Action action) { ToolStripMenuItem item = new ToolStripMenuItem(text); item.Click += delegate { action(); }; menu.Items.Add(item); }
    }
}
