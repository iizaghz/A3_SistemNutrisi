using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemNutrisi
{
    public partial class FormKonsumsi : Form
    {
        // =====================================================
        // CONNECTION STRING & FIELDS
        // =====================================================
        private readonly SqlConnection conn;
        private readonly string connectionString =
            "Data Source=IZAYAAA\\IZA;Initial Catalog=DBSistemNutrisi;Integrated Security=True";
        private int idUser;

        // =====================================================
        // CONSTRUCTOR
        // =====================================================
        public FormKonsumsi(int idUser)
        {
            InitializeComponent();
            this.idUser = idUser;
            conn = new SqlConnection(connectionString);
        }

        // =====================================================
        // FORM LOAD
        // =====================================================
        private void FormKonsumsi_Load(object sender, EventArgs e)
        {
            // Batasi input tanggal maksimal seminggu yang lalu (7 hari) sampai hari ini
            dtpTanggal.MinDate = DateTime.Today.AddDays(-7);
            dtpTanggal.MaxDate = DateTime.Today;
            dtpTanggal.Value = DateTime.Today;

            LoadMakananComboBox();

            // Sembunyikan header jika di-embed sebagai child control agar tidak double header
            if (!this.TopLevel)
            {
                pnlHeader.Visible = false;
                btnBack.Visible = false;
            }
        }

        // =====================================================
        // LOAD DATA
        // =====================================================
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
            catch (Exception ex) 
            { 
                MessageBox.Show("Error: " + ex.Message); 
            }
        }

        // =====================================================
        // BUTTON ACTIONS
        // =====================================================
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

                // VALIDASI TANGGAL KONSUMSI (MAKSIMAL 7 HARI YANG LALU HINGGA HARI INI)
                DateTime selectedDate = dtpTanggal.Value.Date;
                DateTime minAllowedDate = DateTime.Today.AddDays(-7);
                DateTime maxAllowedDate = DateTime.Today;

                if (selectedDate < minAllowedDate || selectedDate > maxAllowedDate)
                {
                    MessageBox.Show(
                        $"Tanggal konsumsi harus di antara {minAllowedDate:dd-MM-yyyy} sampai hari ini!",
                        "Validasi Tanggal",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_InsertKonsumsi", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_makanan", cmbMakanan.SelectedValue);
                cmd.Parameters.AddWithValue("@id_user", idUser);
                cmd.Parameters.AddWithValue("@tanggal", selectedDate);
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
