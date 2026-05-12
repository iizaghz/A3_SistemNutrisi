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
    public partial class FormMakanan : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString =
            "Data Source=IZAYAAA\\IZA;Initial Catalog=DBSistemNutrisi;Integrated Security=True";

        private List<int> idKategoriList = new List<int>();
        private string selectedId = "";

        public FormMakanan()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void FormMakanan_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LoadKategoriComboBox();
        }

        private void LoadKategoriComboBox()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                cmbKategori.Items.Clear();
                idKategoriList.Clear();

                SqlCommand cmd = new SqlCommand("SELECT * FROM v_SemuaKategori", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    idKategoriList.Add(Convert.ToInt32(reader["id_kategori"]));
                    cmbKategori.Items.Add(reader["nama_kategori"].ToString());
                }
                reader.Close();
            }
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan load kategori: " + ex.Message); }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetMakanan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@search", DBNull.Value);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;

                // Mempercantik Header
                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["id_makanan"].HeaderText = "ID Makanan";
                    dataGridView1.Columns["nama_makanan"].HeaderText = "Nama Makanan";
                    dataGridView1.Columns["nama_kategori"].HeaderText = "Kategori";
                }

                SqlCommand cmdCount = new SqlCommand("sp_GetMakananCount", conn);
                cmdCount.CommandType = CommandType.StoredProcedure;
                lblJumlah.Text = "Total Makanan: " + cmdCount.ExecuteScalar().ToString();
            }
            catch (Exception ex) { MessageBox.Show("Gagal menampilkan data: " + ex.Message); }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNamaMakanan.Text))
                {
                    MessageBox.Show("Nama makanan harus diisi"); txtNamaMakanan.Focus(); return;
                }
                if (!IsValidText(txtNamaMakanan.Text))
                {
                    MessageBox.Show("Nama Makanan tidak boleh mengandung angka atau simbol."); txtNamaMakanan.Focus(); return;
                }
                if (cmbKategori.SelectedIndex < 0)
                {
                    MessageBox.Show("Kategori harus dipilih"); cmbKategori.Focus(); return;
                }

                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_InsertMakanan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_kategori", idKategoriList[cmbKategori.SelectedIndex]);
                cmd.Parameters.AddWithValue("@nama_makanan", txtNamaMakanan.Text);

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Data makanan berhasil ditambahkan");
                    ClearForm();
                    btnLoad.PerformClick();
                }
                else
                {
                    MessageBox.Show("Data gagal ditambahkan");
                }
            }
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan: " + ex.Message); }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(selectedId)) { MessageBox.Show("Pilih data dulu!"); return; }
                if (string.IsNullOrEmpty(txtNamaMakanan.Text))
                {
                    MessageBox.Show("Nama makanan harus diisi"); txtNamaMakanan.Focus(); return;
                }
                if (!IsValidText(txtNamaMakanan.Text))
                {
                    MessageBox.Show("Nama Makanan tidak boleh mengandung angka atau simbol."); txtNamaMakanan.Focus(); return;
                }
                if (cmbKategori.SelectedIndex < 0)
                {
                    MessageBox.Show("Kategori harus dipilih"); cmbKategori.Focus(); return;
                }

                DialogResult confirm = MessageBox.Show("Yakin ingin mengubah data ini?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;

                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_UpdateMakanan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_makanan", int.Parse(selectedId));
                cmd.Parameters.AddWithValue("@id_kategori", idKategoriList[cmbKategori.SelectedIndex]);
                cmd.Parameters.AddWithValue("@nama_makanan", txtNamaMakanan.Text);

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Data berhasil diupdate");
                    ClearForm();
                    btnLoad.PerformClick();
                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan");
                }
            }
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan: " + ex.Message); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(selectedId)) { MessageBox.Show("Pilih data dulu!"); return; }

                DialogResult confirm = MessageBox.Show("Yakin ingin menghapus data?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;

                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_DeleteMakanan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_makanan", int.Parse(selectedId));

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Data berhasil dihapus");
                    ClearForm();
                    btnLoad.PerformClick();
                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan");
                }
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan: " + ex.Message); }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetMakanan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@search", txtSearch.Text);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["id_makanan"].HeaderText = "ID Makanan";
                    dataGridView1.Columns["nama_makanan"].HeaderText = "Nama Makanan";
                    dataGridView1.Columns["nama_kategori"].HeaderText = "Kategori";
                }
            }
            catch (Exception ex) { MessageBox.Show("Gagal menampilkan data: " + ex.Message); }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow r = dataGridView1.Rows[e.RowIndex];
                selectedId = r.Cells["id_makanan"].Value?.ToString();
                txtNamaMakanan.Text = r.Cells["nama_makanan"].Value?.ToString();
                cmbKategori.Text = r.Cells["nama_kategori"].Value?.ToString();
            }
        }

        private void ClearForm()
        {
            selectedId = "";
            txtNamaMakanan.Clear();
            cmbKategori.SelectedIndex = -1;
            txtSearch.Clear();
            txtNamaMakanan.Focus();
        }

        private bool IsValidText(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z\s]+$");
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbKategori_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
