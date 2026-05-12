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
    public partial class FormUser : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString =
            "Data Source=IZAYAAA\\IZA;Initial Catalog=DBSistemNutrisi;Integrated Security=True";

        private int idUser;
        private string namaUser;
        private BindingSource bs = new BindingSource();
        private BindingNavigator bn;

        public FormUser(int idUser, string namaUser)
        {
            InitializeComponent();
            this.idUser = idUser;
            this.namaUser = namaUser;
            conn = new SqlConnection(connectionString);
        }

        private void FormUser_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "Selamat datang, " + namaUser;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Inisialisasi BindingNavigator
            bn = new BindingNavigator(true);
            bn.BindingSource = bs;
            bn.Dock = DockStyle.Bottom;
            this.Controls.Add(bn);

            btnLoad.PerformClick();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetNutrisiUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                bs.DataSource = dt;
                dataGridView1.DataSource = bs;

                // Mempercantik Header
                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["nama_makanan"].HeaderText = "Makanan";
                    dataGridView1.Columns["kalori"].HeaderText = "Kalori";
                    dataGridView1.Columns["protein"].HeaderText = "Protein";
                    dataGridView1.Columns["lemak"].HeaderText = "Lemak";
                    dataGridView1.Columns["karbohidrat"].HeaderText = "Karbohidrat";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message);
            }
        }

        private void btnKonsumsi_Click(object sender, EventArgs e)
        {
            FormKonsumsi formKonsumsi = new FormKonsumsi(idUser);
            formKonsumsi.ShowDialog();
        }

        private void btnRiwayat_Click(object sender, EventArgs e)
        {
            FormRiwayat formRiwayat = new FormRiwayat(idUser, namaUser);
            formRiwayat.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 formLogin = new Form1();
            formLogin.Show();
        }

        private void lblWelcome_Click(object sender, EventArgs e)
        {

        }
    }
}
