using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemNutrisi
{
    public partial class FormAdmin : Form
    {
        private int idUser;
        private string namaUser;

        public FormAdmin(int idUser, string namaUser)
        {
            InitializeComponent();
            this.idUser = idUser;
            this.namaUser = namaUser;
        }

        private void loadForm(object Form)
        {
            if (this.pnlContent.Controls.Count > 0)
                this.pnlContent.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            f.FormBorderStyle = FormBorderStyle.None;
            this.pnlContent.Controls.Add(f);
            this.pnlContent.Tag = f;
            f.Show();
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "Selamat datang, " + namaUser + " (Admin)";
        }

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
