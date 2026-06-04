using System;
using System.Drawing;
using System.Windows.Forms;
using GestionDecanat.Services;

namespace GestionDecanat.Forms
{
    public class LoginForm : Form
    {
        private readonly TextBox txtUsername = new TextBox();
        private readonly TextBox txtPassword = new TextBox();
        private readonly AuthService auth = new AuthService();

        public LoginForm()
        {
            Text = "Connexion - Gestion Académique de Décanat";
            Width = 420; Height = 280; StartPosition = FormStartPosition.CenterScreen; FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false;
            Label title = new Label { Text = "Gestion Académique de Décanat", Dock = DockStyle.Top, Height = 60, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 15F, FontStyle.Bold), BackColor = Color.FromArgb(33, 91, 150), ForeColor = Color.White };
            Label user = new Label { Text = "Nom d'utilisateur", Left = 45, Top = 85, Width = 130 };
            txtUsername.Left = 180; txtUsername.Top = 82; txtUsername.Width = 170; txtUsername.Text = "admin";
            Label pass = new Label { Text = "Mot de passe", Left = 45, Top = 125, Width = 130 };
            txtPassword.Left = 180; txtPassword.Top = 122; txtPassword.Width = 170; txtPassword.PasswordChar = '*'; txtPassword.Text = "admin123";
            Button login = new Button { Text = "Se connecter", Left = 180, Top = 165, Width = 170, Height = 32 };
            login.Click += Login_Click;
            Controls.Add(title); Controls.Add(user); Controls.Add(txtUsername); Controls.Add(pass); Controls.Add(txtPassword); Controls.Add(login);
        }

        private void Login_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentUser user = auth.Login(txtUsername.Text.Trim(), txtPassword.Text);
                if (user == null) { MessageBox.Show("Identifiants incorrects."); return; }
                Hide();
                DashboardForm dashboard = new DashboardForm(user);
                dashboard.FormClosed += delegate { Close(); };
                dashboard.Show();
            }
            catch (Exception ex) { MessageBox.Show("Erreur de connexion : " + ex.Message); }
        }
    }
}
