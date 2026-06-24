using System;
using System.Windows.Forms;

namespace SistemNutrisi
{
    public partial class FormAdmin : Form
    {
        // =====================================================
        // FIELDS
        // =====================================================
        private int idUser;
        private string namaUser;

        // =====================================================
        // CONSTRUCTOR
        // =====================================================
        public FormAdmin(int idUser, string namaUser)
        {
            InitializeComponent();
            this.idUser = idUser;
            this.namaUser = namaUser;

            // Atur agar form otomatis maximized / full screen
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new System.Drawing.Size(1200, 700);
        }

        // =====================================================
        // FORM LOAD & HELPER
        // =====================================================
        private void FormAdmin_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "Selamat datang, " + namaUser + " (Admin)";
        }

        private void loadForm(object Form)
        {
            this.pnlHeader.Visible = false; // Sembunyikan header utama admin agar tidak double header

            if (this.pnlContent.Controls.Count > 0)
                this.pnlContent.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            f.FormBorderStyle = FormBorderStyle.None;

            // Saat form child ditutup, munculkan kembali header utama admin dan label selamat datang
            f.FormClosed += (s, e2) => {
                this.pnlHeader.Visible = true;
                this.pnlContent.Controls.Remove(f);
                if (this.pnlContent.Controls.Count == 0)
                {
                    this.pnlContent.Controls.Add(label2);
                }
            };

            this.pnlContent.Controls.Add(f);
            this.pnlContent.Tag = f;
            f.Show();
        }

        // =====================================================
        // BUTTON NAVIGATION ACTIONS
        // =====================================================
        private void btnKategori_Click(object sender, EventArgs e)
        {
            loadForm(new FormKategori());
        }

        private void btnMakanan_Click(object sender, EventArgs e)
        {
            loadForm(new FormMakanan());
        }

        private void btnNutrisi_Click(object sender, EventArgs e)
        {
            loadForm(new FormNutrisi());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 formLogin = new Form1();
            formLogin.Show();
        }

        // =====================================================
        // EMPTY EVENT HANDLERS
        // =====================================================
        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void pnlContent_Paint(object sender, PaintEventArgs e)
        {
        }

        private void lblWelcome_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}
