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
        private int idAdmin;
        private string namaAdmin;

        public FormAdmin(int idAdmin, string namaAdmin)
        {
            InitializeComponent();
            this.idAdmin = idAdmin;
            this.namaAdmin = namaAdmin;
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "Selamat datang, " + namaAdmin + " (Admin)";
        }

        private void btnKategori_Click(object sender, EventArgs e)
        {
            FormKategori formKategori = new FormKategori();
            formKategori.Show();
        }

        private void btnMakanan_Click(object sender, EventArgs e)
        {
            FormMakanan formMakanan = new FormMakanan();
            formMakanan.Show();
        }

        private void btnNutrisi_Click(object sender, EventArgs e)
        {
            FormNutrisi formNutrisi = new FormNutrisi();
            formNutrisi.Show();
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
