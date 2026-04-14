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
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                cmbKategori.Items.Clear();
                idKategoriList.Clear();

                string query = "SELECT id_kategori, nama_kategori FROM KategoriMakanan";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    idKategoriList.Add(Convert.ToInt32(row["id_kategori"]));
                    cmbKategori.Items.Add(row["nama_kategori"].ToString());
                }
            }
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan load kategori: " + ex.Message); }
        }


        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                dataGridView1.Columns.Add("id_makanan", "ID Makanan");
                dataGridView1.Columns.Add("nama_makanan", "Nama Makanan");
                dataGridView1.Columns.Add("nama_kategori", "Kategori");


                string query = @"SELECT m.id_makanan, m.nama_makanan, k.nama_kategori 
                                 FROM Makanan m
                                 JOIN KategoriMakanan k ON m.id_kategori = k.id_kategori";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader["id_makanan"].ToString(), 
                        reader["nama_makanan"].ToString(), 
                        reader["nama_kategori"].ToString()
                    );
                }
                reader.Close();

                SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM Makanan", conn);
                lblJumlah.Text = "Total Makanan: " + cmdCount.ExecuteScalar().ToString();
            }
            catch (Exception ex) { MessageBox.Show("Gagal menampilkan data: " + ex.Message); }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                if (string.IsNullOrEmpty(txtNamaMakanan.Text))
                {
                    MessageBox.Show("Nama makanan harus diisi");
                    txtNamaMakanan.Focus();
                    return;
                }
                if (cmbKategori.SelectedIndex < 0)
                {
                    MessageBox.Show("Kategori harus dipilih");
                    cmbKategori.Focus();
                    return;
                }

                string query = @"INSERT INTO Makanan (id_kategori, nama_makanan) VALUES (@idk, @nama)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idk", idKategoriList[cmbKategori.SelectedIndex]);
                cmd.Parameters.AddWithValue("@nama", txtNamaMakanan.Text);
                
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
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                if (string.IsNullOrEmpty(selectedId)) { MessageBox.Show("Pilih data dulu!"); return; }

                if (string.IsNullOrEmpty(txtNamaMakanan.Text))
                {
                    MessageBox.Show("Nama makanan harus diisi");
                    txtNamaMakanan.Focus();
                    return;
                }
                if (cmbKategori.SelectedIndex < 0)
                {
                    MessageBox.Show("Kategori harus dipilih");
                    cmbKategori.Focus();
                    return;
                }

                DialogResult resultConfirm = MessageBox.Show(
                    "Yakin ingin mengubah data ini?",
                    "Konfirmasi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultConfirm == DialogResult.No) return;

                string query = @"UPDATE Makanan SET id_kategori=@idk, nama_makanan=@nama WHERE id_makanan=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", selectedId);
                cmd.Parameters.AddWithValue("@idk", idKategoriList[cmbKategori.SelectedIndex]);
                cmd.Parameters.AddWithValue("@nama", txtNamaMakanan.Text);


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
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                if (string.IsNullOrEmpty(selectedId)) { MessageBox.Show("Pilih data dulu!"); return; }

                DialogResult resultConfirm = MessageBox.Show(
                    "Yakin ingin menghapus data?",
                    "Konfirmasi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultConfirm == DialogResult.Yes)
                {
                    string query = "DELETE FROM Makanan WHERE id_makanan=@id"; 
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", selectedId);

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
            }
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan: " + ex.Message); }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                dataGridView1.Rows.Clear();

                string query = @"SELECT m.id_makanan, m.nama_makanan, k.nama_kategori 
                                 FROM Makanan m
                                 JOIN KategoriMakanan k ON m.id_kategori = k.id_kategori
                                 WHERE m.nama_makanan LIKE @key";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@key", "%" + txtSearch.Text + "%");
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) 
                { 
                    dataGridView1.Rows.Add(
                        reader["id_makanan"].ToString(), 
                        reader["nama_makanan"].ToString(), 
                        reader["nama_kategori"].ToString()
                    ); 
                }
                reader.Close();
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
