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
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT id_makanan, nama_makanan FROM v_MakananLengkap", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cmbMakanan.DataSource = dt;
                cmbMakanan.DisplayMember = "nama_makanan";
                cmbMakanan.ValueMember = "id_makanan";
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

                if (!int.TryParse(txtJumlah.Text, out int jumlah) || jumlah <= 0)
                {
                    MessageBox.Show("Jumlah harus berupa angka lebih dari 0!");
                    txtJumlah.Focus();
                    return;
                }

                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_InsertKonsumsi", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_makanan", cmbMakanan.SelectedValue);
                cmd.Parameters.AddWithValue("@id_user", idUser);
                cmd.Parameters.AddWithValue("@tanggal", dtpTanggal.Value.Date);
                cmd.Parameters.AddWithValue("@jumlah", jumlah);

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
