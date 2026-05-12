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
    public partial class FormRiwayat : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString =
            "Data Source=IZAYAAA\\IZA;Initial Catalog=DBSistemNutrisi;Integrated Security=True";

        private int idUser;
        private string namaUser;
        private BindingSource bs = new BindingSource();
        private BindingNavigator bn;

        public FormRiwayat(int idUser, string namaUser)
        {
            InitializeComponent();
            this.idUser = idUser;
            this.namaUser = namaUser;
            conn = new SqlConnection(connectionString);
        }

        private void FormRiwayat_Load(object sender, EventArgs e)
        {
            lblInfo.Text = "Riwayat Konsumsi: " + namaUser;

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

                SqlCommand cmd = new SqlCommand("sp_GetRiwayatKonsumsi", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_user", idUser);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                bs.DataSource = dt;
                dataGridView1.DataSource = bs;

                // Mempercantik Header
                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["tanggal"].HeaderText = "Tanggal";
                    dataGridView1.Columns["nama_makanan"].HeaderText = "Makanan";
                    dataGridView1.Columns["jumlah"].HeaderText = "Jumlah";
                    dataGridView1.Columns["total_kalori"].HeaderText = "Kalori";
                    dataGridView1.Columns["total_protein"].HeaderText = "Protein";
                    dataGridView1.Columns["total_lemak"].HeaderText = "Lemak";
                    dataGridView1.Columns["total_karbohidrat"].HeaderText = "Karbohidrat";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
