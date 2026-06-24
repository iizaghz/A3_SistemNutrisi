using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SistemNutrisi
{
    public partial class FormMakanan : Form
    {
        // CONNECTION STRING
        private readonly string connectionString = DAL.GetConnectionString();

        // SQL CONNECTION
        private SqlConnection conn;

        // BINDING SOURCE
        private BindingSource bs = new BindingSource();

        // IMPORT PREVIEW DATA
        private DataTable importedDataPreview = null;

        public FormMakanan()
        {
            InitializeComponent();

            conn = new SqlConnection(connectionString);

           
            btnImportDb.ForeColor = System.Drawing.Color.White;
        }


        private void FormMakanan_Load(object sender, EventArgs e)
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

            // HUBUNGKAN NAVIGATOR
            bindingNavigator1.BindingSource = bs;

            // HUBUNGKAN DATAGRIDVIEW
            dataGridView1.DataSource = bs;

            // LOAD COMBOBOX
            LoadKategoriComboBox();

            // LOAD DATA
            LoadData();

            // SET INITIAL IMPORTING STATE
            SetImportingState(false);

            // BINDING TEXTBOX
            txtNamaMakanan.DataBindings.Clear();

            txtNamaMakanan.DataBindings.Add(
                "Text",
                bs,
                "nama_makanan",
                true,
                DataSourceUpdateMode.OnPropertyChanged
            );

            // BINDING COMBOBOX
            cmbKategori.DataBindings.Clear();

            cmbKategori.DataBindings.Add(
                "SelectedValue",
                bs,
                "id_kategori",
                true,
                DataSourceUpdateMode.OnPropertyChanged
            );
        }

        // =====================================================
        // LOAD KATEGORI COMBOBOX
        // =====================================================
        private void LoadKategoriComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM v_KategoriMakanan";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        cmbKategori.DataSource = dt;
                        cmbKategori.DisplayMember = "nama_kategori";
                        cmbKategori.ValueMember = "id_kategori";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan load kategori : " + ex.Message);
            }
        }

        // =====================================================
        // LOAD DATA
        // =====================================================
        private void LoadData(string search = "")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM v_MakananLengkap";
                    if (!string.IsNullOrEmpty(search))
                    {
                        query += " WHERE nama_makanan LIKE @search";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(search))
                        {
                            cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            bs.DataSource = dt;
                            dataGridView1.DataSource = bs;

                            if (dataGridView1.Columns.Count > 0)
                            {
                                dataGridView1.Columns["id_makanan"].HeaderText = "ID Makanan";
                                dataGridView1.Columns["nama_makanan"].HeaderText = "Nama Makanan";
                                dataGridView1.Columns["nama_kategori"].HeaderText = "Kategori";
                            }
                        }
                    }

                    using (SqlCommand cmdCount = new SqlCommand("sp_GetMakananCount", conn))
                    {
                        cmdCount.CommandType = CommandType.StoredProcedure;
                        lblJumlah.Text = "Total Makanan : " + cmdCount.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data : " + ex.Message);
            }
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
        // SEARCH
        // =====================================================
        private void btnSearch_Click(
            object sender,
            EventArgs e
        )
        {
            LoadData(txtSearch.Text);
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
                // VALIDASI
                if (string.IsNullOrWhiteSpace(
                    txtNamaMakanan.Text))
                {
                    MessageBox.Show(
                        "Nama makanan wajib diisi"
                    );

                    txtNamaMakanan.Focus();

                    return;
                }

                if (!IsValidText(
                    txtNamaMakanan.Text))
                {
                    MessageBox.Show(
                        "Nama makanan hanya boleh huruf"
                    );

                    txtNamaMakanan.Focus();

                    return;
                }

                if (cmbKategori.SelectedIndex < 0)
                {
                    MessageBox.Show(
                        "Kategori wajib dipilih"
                    );

                    cmbKategori.Focus();

                    return;
                }

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd =
                    new SqlCommand(
                        "sp_InsertMakanan",
                        conn
                    );

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@id_kategori",
                    cmbKategori.SelectedValue
                );

                cmd.Parameters.AddWithValue(
                    "@nama_makanan",
                    txtNamaMakanan.Text.Trim()
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
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Terjadi kesalahan : "
                    + ex.Message
                );
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
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

                // AMBIL ID
                string id =
                    ((DataRowView)bs.Current)["id_makanan"]
                    .ToString();

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd =
                    new SqlCommand(
                        "sp_UpdateMakanan",
                        conn
                    );

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@id_makanan",
                    int.Parse(id)
                );

                cmd.Parameters.AddWithValue(
                    "@id_kategori",
                    cmbKategori.SelectedValue
                );

                cmd.Parameters.AddWithValue(
                    "@nama_makanan",
                    txtNamaMakanan.Text.Trim()
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
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Terjadi kesalahan : "
                    + ex.Message
                );
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
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
                    ((DataRowView)bs.Current)["id_makanan"]
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

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd =
                    new SqlCommand(
                        "sp_DeleteMakanan",
                        conn
                    );

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@id_makanan",
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
            catch (SqlException ex)
            when (ex.Number == 50000)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Terjadi kesalahan : "
                    + ex.Message
                );
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        // =====================================================
        // CLEAR FORM
        // =====================================================
        private void ClearForm()
        {
            txtNamaMakanan.Clear();

            cmbKategori.SelectedIndex = -1;

            txtSearch.Clear();

            txtNamaMakanan.Focus();
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
        // EVENT KOSONG
        // =====================================================
        private void dataGridView1_CellContentClick(
            object sender,
            DataGridViewCellEventArgs e
        )
        {

        }

        private void cmbKategori_SelectedIndexChanged(
            object sender,
            EventArgs e
        )
        {

        }

        private void label4_Click(
            object sender,
            EventArgs e
        )
        {

        }

        private void pnlHeader_Paint(
            object sender,
            PaintEventArgs e
        )
        {

        }

        private void bindingNavigator1_RefreshItems(
            object sender,
            EventArgs e
        )
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        private void lblJumlah_Click(object sender, EventArgs e)
        {

        }

        private void SetImportingState(bool isImporting)
        {
            dataGridView1.Enabled = !isImporting;

            // Gunakan perubahan warna (bukan Enabled=false) agar ForeColor putih tetap tampil.
            // Click handler sudah memvalidasi apakah data preview ada atau tidak.
            btnImportDb.BackColor = System.Drawing.Color.ForestGreen;
            btnImportDb.ForeColor = System.Drawing.Color.White;

            btnInsert.Enabled = !isImporting;
            btnUpdate.Enabled = !isImporting;
            btnDelete.Enabled = !isImporting;
            btnLoad.Enabled = !isImporting;
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            openFileDialog.Title = "Pilih File Excel Data Makanan";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable dt = ExcelReader.ReadExcel(openFileDialog.FileName);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        MessageBox.Show("File Excel kosong atau tidak valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Identifikasi kolom
                    int colMakananIdx = -1;
                    int colKategoriIdx = -1;

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string colName = dt.Columns[i].ColumnName.ToLower();
                        if (colName.Contains("makanan") || colName.Contains("nama"))
                        {
                            colMakananIdx = i;
                        }
                        else if (colName.Contains("kategori") || colName.Contains("category"))
                        {
                            colKategoriIdx = i;
                        }
                    }

                    // Default mapping jika kolom tidak terdeteksi
                    if (colMakananIdx == -1 && dt.Columns.Count > 0) colMakananIdx = 0;
                    if (colKategoriIdx == -1 && dt.Columns.Count > 1) colKategoriIdx = 1;

                    if (colMakananIdx == -1 || colKategoriIdx == -1)
                    {
                        MessageBox.Show("Format kolom Excel tidak sesuai. Pastikan ada kolom Makanan dan Kategori.", "Format Salah", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Buat preview DataTable dengan struktur yang sesuai dengan grid
                    DataTable previewTable = new DataTable();
                    previewTable.Columns.Add("id_makanan", typeof(string));
                    previewTable.Columns.Add("nama_makanan", typeof(string));
                    previewTable.Columns.Add("id_kategori", typeof(string));
                    previewTable.Columns.Add("nama_kategori", typeof(string));

                    foreach (DataRow row in dt.Rows)
                    {
                        string namaMakanan = row[colMakananIdx]?.ToString()?.Trim();
                        string namaKategori = row[colKategoriIdx]?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(namaMakanan) || !string.IsNullOrEmpty(namaKategori))
                        {
                            previewTable.Rows.Add("PREVIEW", namaMakanan, "", namaKategori);
                        }
                    }

                    if (previewTable.Rows.Count == 0)
                    {
                        MessageBox.Show("Tidak ada data makanan valid untuk di-import.", "Data Kosong", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Simpan ke private member
                    importedDataPreview = previewTable;

                    // Bind ke grid agar tampil ke layar
                    dataGridView1.DataSource = importedDataPreview;

                    // Set control state to importing
                    SetImportingState(true);

                    // Update label jumlah
                    lblJumlah.Text = "Total Makanan (Preview): " + importedDataPreview.Rows.Count;

                    MessageBox.Show("Berhasil memuat data dari Excel. Silakan periksa data pada layar, lalu klik 'Import to Database' untuk menyimpannya.", "Preview Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan saat membaca file Excel: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnImportDb_Click(object sender, EventArgs e)
        {
            if (importedDataPreview == null || importedDataPreview.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data preview untuk diimport ke database.\nSilakan klik 'Import from Excel' terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show($"Yakin ingin mengimpor {importedDataPreview.Rows.Count} data dari preview ke database?", "Konfirmasi Import", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            try
            {
                int successCount = 0;
                int skipCount = 0;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    foreach (DataRow row in importedDataPreview.Rows)
                    {
                        string namaMakanan = row["nama_makanan"]?.ToString()?.Trim();
                        string namaKategori = row["nama_kategori"]?.ToString()?.Trim();

                        if (string.IsNullOrEmpty(namaMakanan) || string.IsNullOrEmpty(namaKategori))
                        {
                            skipCount++;
                            continue;
                        }

                        // 1. Dapatkan Kategori (gunakan fallback 'Umum' jika tidak terdaftar)
                        int idKategori = -1;
                        string qKategori = "SELECT id_kategori FROM KategoriMakanan WHERE LOWER(nama_kategori) = LOWER(@nama_kategori)";
                        using (SqlCommand cmdKategori = new SqlCommand(qKategori, connection))
                        {
                            cmdKategori.Parameters.AddWithValue("@nama_kategori", namaKategori);
                            object res = cmdKategori.ExecuteScalar();
                            if (res != null)
                            {
                                idKategori = Convert.ToInt32(res);
                            }
                            else
                            {
                                // Fallback ke Kategori "Umum" jika tidak terdaftar
                                string qUmum = "SELECT id_kategori FROM KategoriMakanan WHERE LOWER(nama_kategori) = 'umum'";
                                using (SqlCommand cmdUmum = new SqlCommand(qUmum, connection))
                                {
                                    object resUmum = cmdUmum.ExecuteScalar();
                                    if (resUmum != null)
                                    {
                                        idKategori = Convert.ToInt32(resUmum);
                                    }
                                    else
                                    {
                                        string insKategori = "INSERT INTO KategoriMakanan (nama_kategori) OUTPUT INSERTED.id_kategori VALUES ('Umum')";
                                        using (SqlCommand cmdInsKategori = new SqlCommand(insKategori, connection))
                                        {
                                            idKategori = Convert.ToInt32(cmdInsKategori.ExecuteScalar());
                                        }
                                    }
                                }
                            }
                        }

                        // 2. Cek apakah Makanan sudah ada (tanpa memandang kategori)
                        int existingIdMakanan = -1;
                        string qMakanan = "SELECT id_makanan FROM Makanan WHERE LOWER(nama_makanan) = LOWER(@nama_makanan)";
                        using (SqlCommand cmdMakanan = new SqlCommand(qMakanan, connection))
                        {
                            cmdMakanan.Parameters.AddWithValue("@nama_makanan", namaMakanan);
                            object res = cmdMakanan.ExecuteScalar();
                            if (res != null)
                            {
                                existingIdMakanan = Convert.ToInt32(res);
                            }
                        }

                        if (existingIdMakanan != -1)
                        {
                            // Makanan sudah ada, update kategorinya agar sesuai dengan Excel
                            string qUpdMakanan = "UPDATE Makanan SET id_kategori = @id_kategori WHERE id_makanan = @id_makanan";
                            using (SqlCommand cmdUpdM = new SqlCommand(qUpdMakanan, connection))
                            {
                                cmdUpdM.Parameters.AddWithValue("@id_kategori", idKategori);
                                cmdUpdM.Parameters.AddWithValue("@id_makanan", existingIdMakanan);
                                cmdUpdM.ExecuteNonQuery();
                            }
                            successCount++;
                        }
                        else
                        {
                            // 3. Masukkan Makanan baru jika belum ada sama sekali
                            using (SqlCommand cmdInsMakanan = new SqlCommand("sp_InsertMakanan", connection))
                            {
                                cmdInsMakanan.CommandType = CommandType.StoredProcedure;
                                cmdInsMakanan.Parameters.AddWithValue("@id_kategori", idKategori);
                                cmdInsMakanan.Parameters.AddWithValue("@nama_makanan", namaMakanan);
                                cmdInsMakanan.ExecuteNonQuery();
                                successCount++;
                            }
                        }
                    }
                }

                // Reset preview data
                importedDataPreview = null;

                // Reset control state
                SetImportingState(false);

                // Reload database data & update bindings
                LoadData();

                MessageBox.Show($"Import ke database selesai!\nBerhasil: {successCount} data\nLewati (sudah ada/kosong): {skipCount} data", "Import Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat mengimpor data ke database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}