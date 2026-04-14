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
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                dataGridView1.Columns.Add("nama_makanan", "Makanan");
                dataGridView1.Columns.Add("kalori", "Kalori");
                dataGridView1.Columns.Add("protein", "Protein");
                dataGridView1.Columns.Add("lemak", "Lemak");
                dataGridView1.Columns.Add("karbohidrat", "Karbohidrat");

                string query = @"SELECT m.nama_makanan, n.kalori, n.protein, n.lemak, n.karbohidrat
                                 FROM Makanan m
                                 LEFT JOIN Nutrisi n ON m.id_makanan = n.id_makanan";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader["nama_makanan"].ToString(),
                        reader["kalori"].ToString(),
                        reader["protein"].ToString(),
                        reader["lemak"].ToString(),
                        reader["karbohidrat"].ToString()
                    );
                }
                reader.Close();
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
