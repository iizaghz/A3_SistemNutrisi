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
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                dataGridView1.Columns.Add("tanggal", "Tanggal");
                dataGridView1.Columns.Add("nama_makanan", "Makanan");
                dataGridView1.Columns.Add("jumlah", "Jumlah");
                dataGridView1.Columns.Add("total_kalori", "Kalori");
                dataGridView1.Columns.Add("total_protein", "Protein");
                dataGridView1.Columns.Add("total_lemak", "Lemak");
                dataGridView1.Columns.Add("total_karbohidrat", "Karbohidrat");

                string query = @"SELECT k.tanggal, m.nama_makanan, k.jumlah, 
                                        (n.kalori * k.jumlah) as total_kalori, 
                                        (n.protein * k.jumlah) as total_protein,
                                        (n.lemak * k.jumlah) as total_lemak, 
                                        (n.karbohidrat * k.jumlah) as total_karbohidrat
                                 FROM KonsumsiMakanan k
                                 JOIN Makanan m ON k.id_makanan = m.id_makanan
                                 LEFT JOIN Nutrisi n ON m.id_makanan = n.id_makanan
                                 WHERE k.id_user = @idu
                                 ORDER BY k.tanggal DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idu", idUser);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        Convert.ToDateTime(reader["tanggal"]).ToShortDateString(),
                        reader["nama_makanan"].ToString(),
                        reader["jumlah"].ToString(),
                        reader["total_kalori"].ToString(),
                        reader["total_protein"].ToString(),
                        reader["total_lemak"].ToString(),
                        reader["total_karbohidrat"].ToString()
                    );
                }
                reader.Close();
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
