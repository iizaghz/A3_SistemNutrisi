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
        private BindingSource bs = new BindingSource();
        private BindingNavigator bn;

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

            // Inisialisasi BindingNavigator
            bn = new BindingNavigator(true);
            bn.BindingSource = bs;
            bn.Dock = DockStyle.Bottom;
            this.Controls.Add(bn);

            LoadMakananComboBox();
            btnLoad.PerformClick();

            // Data Binding (Otomatis sinkron saat navigasi)
            cmbMakanan.DataBindings.Add("Text", bs, "nama_makanan", true, DataSourceUpdateMode.OnPropertyChanged);
            txtKalori.DataBindings.Add("Text", bs, "kalori", true, DataSourceUpdateMode.OnPropertyChanged);
            txtProtein.DataBindings.Add("Text", bs, "protein", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLemak.DataBindings.Add("Text", bs, "lemak", true, DataSourceUpdateMode.OnPropertyChanged);
            txtKarbohidrat.DataBindings.Add("Text", bs, "karbohidrat", true, DataSourceUpdateMode.OnPropertyChanged);
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
                
                SqlCommand cmd = new SqlCommand("SELECT * FROM v_NutrisiLengkap", conn);
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    cmd.CommandText += " WHERE nama_makanan LIKE @search";
                    cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                bs.DataSource = dt;
                dataGridView1.DataSource = bs;

                // Mempercantik Header
                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["id_makanan"].HeaderText = "ID Makanan";
                    dataGridView1.Columns["nama_makanan"].HeaderText = "Nama Makanan";
                    dataGridView1.Columns["kalori"].HeaderText = "Kalori";
                    dataGridView1.Columns["protein"].HeaderText = "Protein";
                    dataGridView1.Columns["lemak"].HeaderText = "Lemak";
                    dataGridView1.Columns["karbohidrat"].HeaderText = "Karbohidrat";
                }
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
                if (bs.Current == null) { MessageBox.Show("Pilih data dulu!"); return; }
                string id = ((DataRowView)bs.Current)["id_makanan"].ToString();

                DialogResult confirm = MessageBox.Show("Yakin ingin mengubah data nutrisi ini?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;

                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_UpdateNutrisi", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_makanan", int.Parse(id));
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
                if (bs.Current == null) { MessageBox.Show("Pilih data dulu!"); return; }
                string id = ((DataRowView)bs.Current)["id_makanan"].ToString();

                DialogResult confirm = MessageBox.Show("Yakin ingin menghapus data nutrisi?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;

                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_DeleteNutrisi", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_makanan", int.Parse(id));

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
