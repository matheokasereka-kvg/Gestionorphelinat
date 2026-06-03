using System;
using System.Windows.Forms;
using GestionDecanat.Forms;
using QuestPDF.Infrastructure;

namespace GestionDecanat
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}
