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
    public partial class FormNutrisi : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString =
            "Data Source=IZAYAAA\\IZA;Initial Catalog=DBSistemNutrisi;Integrated Security=True";

        private List<int> idMakananList = new List<int>();

        public FormNutrisi()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void FormNutrisi_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LoadMakananComboBox();
        }

        private void LoadMakananComboBox()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                cmbMakanan.Items.Clear();
                idMakananList.Clear();

                SqlCommand cmd = new SqlCommand("SELECT id_makanan, nama_makanan FROM v_MakananLengkap", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    idMakananList.Add(Convert.ToInt32(reader["id_makanan"]));
                    cmbMakanan.Items.Add(reader["nama_makanan"].ToString());
                }
                reader.Close();
            }
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan: " + ex.Message); }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData(string searchTerm = "")
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                dataGridView1.Columns.Add("id_makanan", "ID Makanan");
                dataGridView1.Columns.Add("nama_makanan", "Nama Makanan");
                dataGridView1.Columns.Add("kalori", "Kalori");
                dataGridView1.Columns.Add("protein", "Protein");
                dataGridView1.Columns.Add("lemak", "Lemak");
                dataGridView1.Columns.Add("karbohidrat", "Karbohidrat");

                SqlCommand cmd = new SqlCommand("sp_GetNutrisi", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@search", string.IsNullOrEmpty(searchTerm) ? (object)DBNull.Value : searchTerm);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader["id_makanan"].ToString(),
                        reader["nama_makanan"].ToString(),
                        reader["kalori"].ToString(),
                        reader["protein"].ToString(),
                        reader["lemak"].ToString(),
                        reader["karbohidrat"].ToString()
                    );
                }
                reader.Close();
            }
            catch (Exception ex) { MessageBox.Show("Gagal menampilkan data: " + ex.Message); }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbMakanan.SelectedIndex < 0) { MessageBox.Show("Pilih Makanan!"); cmbMakanan.Focus(); return; }
                if (string.IsNullOrEmpty(txtKalori.Text)) { MessageBox.Show("Kalori harus diisi"); txtKalori.Focus(); return; }
                if (string.IsNullOrEmpty(txtProtein.Text)) { MessageBox.Show("Protein harus diisi"); txtProtein.Focus(); return; }
                if (string.IsNullOrEmpty(txtLemak.Text)) { MessageBox.Show("Lemak harus diisi"); txtLemak.Focus(); return; }
                if (string.IsNullOrEmpty(txtKarbohidrat.Text)) { MessageBox.Show("Karbohidrat harus diisi"); txtKarbohidrat.Focus(); return; }

                if (!IsValidNumber(txtKalori.Text) || !IsValidNumber(txtProtein.Text) ||
                    !IsValidNumber(txtLemak.Text) || !IsValidNumber(txtKarbohidrat.Text))
                {
                    MessageBox.Show("Data angka tidak boleh minus, 0, atau mengandung karakter selain angka.");
                    return;
                }

                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_InsertNutrisi", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_makanan", idMakananList[cmbMakanan.SelectedIndex]);
                cmd.Parameters.AddWithValue("@kalori", double.Parse(txtKalori.Text));
                cmd.Parameters.AddWithValue("@protein", double.Parse(txtProtein.Text));
                cmd.Parameters.AddWithValue("@lemak", double.Parse(txtLemak.Text));
                cmd.Parameters.AddWithValue("@karbohidrat", double.Parse(txtKarbohidrat.Text));

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Data nutrisi berhasil ditambahkan");
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
                if (cmbMakanan.SelectedIndex < 0) { MessageBox.Show("Pilih Makanan!"); cmbMakanan.Focus(); return; }
                if (string.IsNullOrEmpty(txtKalori.Text)) { MessageBox.Show("Kalori harus diisi"); txtKalori.Focus(); return; }
                if (string.IsNullOrEmpty(txtProtein.Text)) { MessageBox.Show("Protein harus diisi"); txtProtein.Focus(); return; }
                if (string.IsNullOrEmpty(txtLemak.Text)) { MessageBox.Show("Lemak harus diisi"); txtLemak.Focus(); return; }
                if (string.IsNullOrEmpty(txtKarbohidrat.Text)) { MessageBox.Show("Karbohidrat harus diisi"); txtKarbohidrat.Focus(); return; }

                if (!IsValidNumber(txtKalori.Text) || !IsValidNumber(txtProtein.Text) ||
                    !IsValidNumber(txtLemak.Text) || !IsValidNumber(txtKarbohidrat.Text))
                {
                    MessageBox.Show("Data angka tidak boleh minus, 0, atau mengandung karakter selain angka.");
                    return;
                }

                DialogResult confirm = MessageBox.Show("Yakin ingin mengubah data nutrisi ini?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;

                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_UpdateNutrisi", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_makanan", idMakananList[cmbMakanan.SelectedIndex]);
                cmd.Parameters.AddWithValue("@kalori", double.Parse(txtKalori.Text));
                cmd.Parameters.AddWithValue("@protein", double.Parse(txtProtein.Text));
                cmd.Parameters.AddWithValue("@lemak", double.Parse(txtLemak.Text));
                cmd.Parameters.AddWithValue("@karbohidrat", double.Parse(txtKarbohidrat.Text));

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
                if (cmbMakanan.SelectedIndex < 0) { MessageBox.Show("Pilih Makanan!"); return; }

                DialogResult confirm = MessageBox.Show("Yakin ingin menghapus data nutrisi?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;

                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_DeleteNutrisi", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_makanan", idMakananList[cmbMakanan.SelectedIndex]);

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
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan: " + ex.Message); }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow r = dataGridView1.Rows[e.RowIndex];
                cmbMakanan.Text = r.Cells["nama_makanan"].Value?.ToString();
                txtKalori.Text = r.Cells["kalori"].Value?.ToString();
                txtProtein.Text = r.Cells["protein"].Value?.ToString();
                txtLemak.Text = r.Cells["lemak"].Value?.ToString();
                txtKarbohidrat.Text = r.Cells["karbohidrat"].Value?.ToString();
            }
        }

        private void ClearForm()
        {
            cmbMakanan.SelectedIndex = -1;
            txtKalori.Clear();
            txtProtein.Clear();
            txtLemak.Clear();
            txtKarbohidrat.Clear();
        }

        private bool IsValidNumber(string input)
        {
            if (decimal.TryParse(input, out decimal value))
                return value > 0;
            return false;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
