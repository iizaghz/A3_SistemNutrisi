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
    public partial class FormKonsumsi : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString =
            "Data Source=IZAYAAA\\IZA;Initial Catalog=DBSistemNutrisi;Integrated Security=True";

        private int idUser;
        private List<int> idMakananList = new List<int>();

        public FormKonsumsi(int idUser)
        {
            InitializeComponent();
            this.idUser = idUser;
            conn = new SqlConnection(connectionString);
        }

        private void FormKonsumsi_Load(object sender, EventArgs e)
        {
            dtpTanggal.Value = DateTime.Now;
            LoadMakananComboBox();
        }

        private void LoadMakananComboBox()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                cmbMakanan.Items.Clear();
                idMakananList.Clear();

                string query = "SELECT id_makanan, nama_makanan FROM Makanan";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    idMakananList.Add(Convert.ToInt32(reader["id_makanan"]));
                    cmbMakanan.Items.Add(reader["nama_makanan"].ToString());
                }
                reader.Close();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbMakanan.SelectedIndex < 0 || string.IsNullOrEmpty(txtJumlah.Text))
                {
                    MessageBox.Show("Pilih makanan dan isi jumlah!");
                    return;
                }

                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                // Sesuai skema Baru (id_makanan, id_user, tanggal, jumlah)
                string query = @"INSERT INTO KonsumsiMakanan (id_makanan, id_user, tanggal, jumlah) 
                                 VALUES (@idm, @idu, @tgl, @jml)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idm", idMakananList[cmbMakanan.SelectedIndex]);
                cmd.Parameters.AddWithValue("@idu", idUser);
                cmd.Parameters.AddWithValue("@tgl", dtpTanggal.Value.Date);
                cmd.Parameters.AddWithValue("@jml", int.Parse(txtJumlah.Text));

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Catatan konsumsi berhasil disimpan");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
