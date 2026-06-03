using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace GestionDecanat.Forms
{
    public abstract class BaseDataForm : Form
    {
        protected DataGridView grid = new DataGridView();
        protected TextBox txtSearch = new TextBox();
        protected Button btnAdd = new Button();
        protected Button btnUpdate = new Button();
        protected Button btnDelete = new Button();
        protected Button btnRefresh = new Button();
        protected Panel editor = new Panel();

        protected BaseDataForm(string title)
        {
            Text = title;
            Width = 1100;
            Height = 650;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 9F);
            BuildLayout();
        }

        private void BuildLayout()
        {
            Label title = new Label { Text = Text, Dock = DockStyle.Top, Height = 45, Font = new Font("Segoe UI", 16F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(12, 0, 0, 0), BackColor = Color.FromArgb(33, 91, 150), ForeColor = Color.White };
            Panel top = new Panel { Dock = DockStyle.Top, Height = 48, Padding = new Padding(8) };
            txtSearch.Width = 260; txtSearch.PlaceholderTextSafe("Rechercher...");
            Button btnSearch = new Button { Text = "Rechercher", Left = 270, Width = 100 };
            btnRefresh.Text = "Actualiser"; btnRefresh.Left = 380; btnRefresh.Width = 100;
            top.Controls.Add(txtSearch); top.Controls.Add(btnSearch); top.Controls.Add(btnRefresh);
            editor.Dock = DockStyle.Top; editor.Height = 155; editor.Padding = new Padding(8);
            Panel buttons = new Panel { Dock = DockStyle.Top, Height = 45, Padding = new Padding(8) };
            btnAdd.Text = "Ajouter"; btnAdd.Width = 100;
            btnUpdate.Text = "Modifier"; btnUpdate.Left = 110; btnUpdate.Width = 100;
            btnDelete.Text = "Supprimer"; btnDelete.Left = 220; btnDelete.Width = 100;
            buttons.Controls.Add(btnAdd); buttons.Controls.Add(btnUpdate); buttons.Controls.Add(btnDelete);
            grid.Dock = DockStyle.Fill; grid.ReadOnly = true; grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; grid.MultiSelect = false; grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Controls.Add(grid); Controls.Add(buttons); Controls.Add(editor); Controls.Add(top); Controls.Add(title);
            Load += delegate { RefreshGrid(); };
            btnRefresh.Click += delegate { RefreshGrid(); };
            btnSearch.Click += delegate { Search(); };
            btnAdd.Click += delegate { SafeAction(AddRecord); };
            btnUpdate.Click += delegate { SafeAction(UpdateRecord); };
            btnDelete.Click += delegate { SafeAction(DeleteRecord); };
            grid.SelectionChanged += delegate { LoadSelected(); };
        }

        protected Label Label(string text, int x, int y) { Label l = new Label { Text = text, Left = x, Top = y, Width = 120 }; editor.Controls.Add(l); return l; }
        protected TextBox TextBox(int x, int y, int w = 170) { TextBox t = new TextBox { Left = x, Top = y, Width = w }; editor.Controls.Add(t); return t; }
        protected ComboBox Combo(int x, int y, int w = 170) { ComboBox c = new ComboBox { Left = x, Top = y, Width = w, DropDownStyle = ComboBoxStyle.DropDownList }; editor.Controls.Add(c); return c; }
        protected DateTimePicker DateBox(int x, int y) { DateTimePicker d = new DateTimePicker { Left = x, Top = y, Width = 170, Format = DateTimePickerFormat.Short }; editor.Controls.Add(d); return d; }
        protected CheckBox Check(string text, int x, int y) { CheckBox c = new CheckBox { Text = text, Left = x, Top = y, Width = 170 }; editor.Controls.Add(c); return c; }
        protected int SelectedId(string idColumn) { if (grid.CurrentRow == null) return 0; return Convert.ToInt32(grid.CurrentRow.Cells[idColumn].Value); }
        protected int ToInt(ComboBox combo) { return combo.SelectedValue == null ? 0 : Convert.ToInt32(combo.SelectedValue); }
        protected decimal ToDecimal(TextBox box) { decimal v; return decimal.TryParse(box.Text, out v) ? v : 0; }
        protected void BindCombo(ComboBox combo, DataTable table, string valueMember, string displayMember) { combo.DataSource = table; combo.ValueMember = valueMember; combo.DisplayMember = displayMember; }
        protected void SafeAction(Action action) { try { action(); RefreshGrid(); } catch (Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
        protected void ConfirmDelete(Action action) { if (MessageBox.Show("Confirmer la suppression ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) action(); }
        protected abstract void RefreshGrid();
        protected abstract void Search();
        protected abstract void AddRecord();
        protected abstract void UpdateRecord();
        protected abstract void DeleteRecord();
        protected virtual void LoadSelected() { }
    }

    internal static class TextBoxExtensions
    {
        public static void PlaceholderTextSafe(this TextBox box, string text)
        {
            try { box.GetType().GetProperty("PlaceholderText").SetValue(box, text, null); } catch { box.Text = string.Empty; }
        }
    }
}
