using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SistemNutrisi
{
    public partial class FormKategori : Form
    {
        // CONNECTION STRING
        private readonly string connectionString = DAL.GetConnectionString();

        // SQL CONNECTION
        private SqlConnection conn;

        // BINDING SOURCE
        private BindingSource bs = new BindingSource();

        public FormKategori()
        {
            InitializeComponent();

            conn = new SqlConnection(connectionString);
        }

        // =====================================================
        // FORM LOAD
        // =====================================================
        private void FormKategori_Load(object sender, EventArgs e)
        {
            btnBack.BringToFront();

            // DATAGRIDVIEW SETTING
            dataGridView1.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dataGridView1.MultiSelect = false;

            dataGridView1.ReadOnly = true;

            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            // HUBUNGKAN BINDINGNAVIGATOR
            bindingNavigator2.BindingSource = bs;

            // HUBUNGKAN DATAGRIDVIEW
            dataGridView1.DataSource = bs;

            // EVENT CLICK
            dataGridView1.CellClick +=
                dataGridView1_CellClick;

            // LOAD DATA
            LoadData();

            // CLEAR BINDING LAMA
            txtNamaKategori.DataBindings.Clear();

            // BINDING TEXTBOX
            txtNamaKategori.DataBindings.Add(
                "Text",
                bs,
                "nama_kategori",
                true,
                DataSourceUpdateMode.OnPropertyChanged
            );
        }

        // =====================================================
        // LOAD DATA
        // =====================================================
        private void LoadData(string searchTerm = "")
        {
            try
            {
                using (SqlConnection conn =
                    new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query =
                        "SELECT * FROM v_KategoriMakanan";

                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        query +=
                            " WHERE nama_kategori LIKE @search";
                    }

                    using (SqlCommand cmd =
                        new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(searchTerm))
                        {
                            cmd.Parameters.AddWithValue(
                                "@search",
                                "%" + searchTerm + "%"
                            );
                        }

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(cmd))
                        {
                            DataTable dt =
                                new DataTable();

                            adapter.Fill(dt);

                            bs.DataSource = dt;

                            // TOTAL KATEGORI
                            using (SqlCommand cmdCount =
                                new SqlCommand(
                                    "SELECT COUNT(*) FROM v_KategoriMakanan",
                                    conn))
                            {
                                int totalKategori =
                                    (int)cmdCount.ExecuteScalar();

                                label2.Text =
                                    "Total Kategori : "
                                    + totalKategori.ToString();
                            }

                            // HEADER DATAGRIDVIEW
                            if (dataGridView1.Columns.Count > 0)
                            {
                                dataGridView1.Columns["id_kategori"]
                                    .HeaderText =
                                    "ID Kategori";

                                dataGridView1.Columns["nama_kategori"]
                                    .HeaderText =
                                    "Nama Kategori";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal menampilkan data : "
                    + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =====================================================
        // SEARCH OTOMATIS
        // =====================================================
        private void txtSearch_TextChanged(
            object sender,
            EventArgs e
        )
        {
            LoadData(txtSearch.Text);
        }

        // =====================================================
        // BUTTON LOAD
        // =====================================================
        private void btnLoad_Click(
            object sender,
            EventArgs e
        )
        {
            LoadData();
        }

        // =====================================================
        // INSERT DATA
        // =====================================================
        private void btnInsert_Click(
            object sender,
            EventArgs e
        )
        {
            try
            {
                // VALIDASI KOSONG
                if (string.IsNullOrWhiteSpace(
                    txtNamaKategori.Text))
                {
                    MessageBox.Show(
                        "Nama kategori wajib diisi"
                    );

                    txtNamaKategori.Focus();

                    return;
                }

                // VALIDASI HURUF
                if (!IsValidText(
                    txtNamaKategori.Text))
                {
                    MessageBox.Show(
                        "Nama kategori hanya boleh huruf"
                    );

                    txtNamaKategori.Focus();

                    return;
                }

                using (SqlConnection conn =
                    new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd =
                        new SqlCommand(
                            "sp_InsertKategori",
                            conn))
                    {
                        cmd.CommandType =
                            CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue(
                            "@nama_kategori",
                            txtNamaKategori.Text.Trim()
                        );

                        int result =
                            cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show(
                                "Data berhasil ditambahkan"
                            );

                            ClearForm();

                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Data gagal ditambahkan"
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Terjadi kesalahan : "
                    + ex.Message
                );
            }
        }

        // =====================================================
        // UPDATE DATA
        // =====================================================
        private void btnUpdate_Click(
            object sender,
            EventArgs e
        )
        {
            try
            {
                // VALIDASI PILIH DATA
                if (bs.Current == null)
                {
                    MessageBox.Show(
                        "Pilih data terlebih dahulu"
                    );

                    return;
                }

                // VALIDASI INPUT
                if (string.IsNullOrWhiteSpace(
                    txtNamaKategori.Text))
                {
                    MessageBox.Show(
                        "Nama kategori wajib diisi"
                    );

                    txtNamaKategori.Focus();

                    return;
                }

                // VALIDASI HURUF
                if (!IsValidText(
                    txtNamaKategori.Text))
                {
                    MessageBox.Show(
                        "Nama kategori hanya boleh huruf"
                    );

                    txtNamaKategori.Focus();

                    return;
                }

                // AMBIL ID
                string id =
                    ((DataRowView)bs.Current)["id_kategori"]
                    .ToString();

                using (SqlConnection conn =
                    new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd =
                        new SqlCommand(
                            "sp_UpdateKategori",
                            conn))
                    {
                        cmd.CommandType =
                            CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue(
                            "@id_kategori",
                            int.Parse(id)
                        );

                        cmd.Parameters.AddWithValue(
                            "@nama_kategori",
                            txtNamaKategori.Text.Trim()
                        );

                        int result =
                            cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show(
                                "Data berhasil diupdate"
                            );

                            ClearForm();

                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Data gagal diupdate"
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Terjadi kesalahan : "
                    + ex.Message
                );
            }
        }

        // =====================================================
        // DELETE DATA
        // =====================================================
        private void btnDelete_Click(
            object sender,
            EventArgs e
        )
        {
            try
            {
                // VALIDASI PILIH DATA
                if (bs.Current == null)
                {
                    MessageBox.Show(
                        "Pilih data terlebih dahulu"
                    );

                    return;
                }

                // AMBIL ID
                string id =
                    ((DataRowView)bs.Current)["id_kategori"]
                    .ToString();

                DialogResult confirm =
                    MessageBox.Show(
                        "Yakin ingin menghapus data?",
                        "Konfirmasi",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                if (confirm == DialogResult.No)
                    return;

                using (SqlConnection conn =
                    new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd =
                        new SqlCommand(
                            "sp_DeleteKategori",
                            conn))
                    {
                        cmd.CommandType =
                            CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue(
                            "@id_kategori",
                            int.Parse(id)
                        );

                        int result =
                            cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show(
                                "Data berhasil dihapus"
                            );

                            ClearForm();

                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Data gagal dihapus"
                            );
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "SQL Error : "
                    + ex.Message
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Terjadi kesalahan : "
                    + ex.Message
                );
            }
        }

        // =====================================================
        // DATAGRIDVIEW CLICK
        // =====================================================
        private void dataGridView1_CellClick(
            object sender,
            DataGridViewCellEventArgs e
        )
        {
            if (e.RowIndex >= 0)
            {
                txtNamaKategori.Text =
                    dataGridView1.Rows[e.RowIndex]
                    .Cells["nama_kategori"]
                    .Value
                    .ToString();
            }
        }

        // =====================================================
        // CLEAR FORM
        // =====================================================
        private void ClearForm()
        {
            txtNamaKategori.Clear();

            txtNamaKategori.Focus();
        }

        // =====================================================
        // VALIDASI TEXT
        // =====================================================
        private bool IsValidText(string input)
        {
            return Regex.IsMatch(
                input,
                @"^[a-zA-Z\s]+$"
            );
        }

        // =====================================================
        // BUTTON BACK
        // =====================================================
        private void btnBack_Click(
            object sender,
            EventArgs e
        )
        {
            this.Close();
        }

        // =====================================================
        // EVENT BINDING NAVIGATOR
        // =====================================================
        private void bindingNavigator2_RefreshItems(
            object sender,
            EventArgs e
        )
        {

        }

        private void label2_Click(
            object sender,
            EventArgs e
        )
        {

        }

    }
}