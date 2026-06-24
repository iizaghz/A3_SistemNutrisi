using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemNutrisi
{
    public partial class FormNutrisi : Form
    {
        // CONNECTION STRING
        private readonly string connectionString = DAL.GetConnectionString();

        // BINDING SOURCE
        private BindingSource bs =
            new BindingSource();

        // IMPORT PREVIEW DATA
        private DataTable importedNutrisiPreview = null;

        public FormNutrisi()
        {
            InitializeComponent();

            // Force warna teks putih pada tombol Import to Database
            btnImportDb.ForeColor = System.Drawing.Color.White;
        }

        // =====================================================
        // FORM LOAD
        // =====================================================
        private void FormNutrisi_Load(
            object sender,
            EventArgs e
        )
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

            // LOAD DATA
            LoadMakananComboBox();

            LoadData();

            // SET INITIAL IMPORTING STATE
            SetImportingState(false);

            // HAPUS BINDING LAMA
            cmbMakanan.DataBindings.Clear();

            txtKalori.DataBindings.Clear();

            txtProtein.DataBindings.Clear();

            txtLemak.DataBindings.Clear();

            txtKarbohidrat.DataBindings.Clear();

            // BINDING CONTROL
            cmbMakanan.DataBindings.Add(
                "SelectedValue",
                bs,
                "id_makanan",
                true,
                DataSourceUpdateMode.Never
            );

            txtKalori.DataBindings.Add(
                "Text",
                bs,
                "kalori",
                true,
                DataSourceUpdateMode.Never
            );

            txtProtein.DataBindings.Add(
                "Text",
                bs,
                "protein",
                true,
                DataSourceUpdateMode.Never
            );

            txtLemak.DataBindings.Add(
                "Text",
                bs,
                "lemak",
                true,
                DataSourceUpdateMode.Never
            );

            txtKarbohidrat.DataBindings.Add(
                "Text",
                bs,
                "karbohidrat",
                true,
                DataSourceUpdateMode.Never
            );
        }

        // =====================================================
        // LOAD COMBOBOX
        // =====================================================
        private void LoadMakananComboBox()
        {
            try
            {
                using (SqlConnection conn =
                    new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query =
                        "SELECT id_makanan, nama_makanan FROM v_MakananLengkap";

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(query, conn))
                    {
                        DataTable dt =
                            new DataTable();

                        adapter.Fill(dt);

                        cmbMakanan.DataSource = dt;

                        cmbMakanan.DisplayMember =
                            "nama_makanan";

                        cmbMakanan.ValueMember =
                            "id_makanan";
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
        // LOAD DATA
        // =====================================================
        private void LoadData(string search = "")
        {
            try
            {
                using (SqlConnection conn =
                    new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query =
                        "SELECT * FROM v_NutrisiLengkap";

                    // SEARCH
                    if (!string.IsNullOrEmpty(search))
                    {
                        query +=
                            " WHERE nama_makanan LIKE @search";
                    }

                    using (SqlCommand cmd =
                        new SqlCommand(query, conn))
                    {
                        // PARAMETER SEARCH
                        if (!string.IsNullOrEmpty(search))
                        {
                            cmd.Parameters.AddWithValue(
                                "@search",
                                "%" + search + "%"
                            );
                        }

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(cmd))
                        {
                            DataTable dt =
                                new DataTable();

                            adapter.Fill(dt);

                            // MASUKKAN KE BINDINGSOURCE
                            bs.DataSource = dt;
                            dataGridView1.DataSource = bs;

                            // TOTAL NUTRISI
                            using (SqlCommand cmdCount =
                                new SqlCommand(
                                    "SELECT COUNT(*) FROM v_NutrisiLengkap",
                                    conn))
                            {
                                int totalNutrisi =
                                    (int)cmdCount.ExecuteScalar();

                                label4.Text =
                                    "Total Nutrisi : "
                                    + totalNutrisi.ToString();
                            }

                            // HEADER DATAGRIDVIEW
                            if (dataGridView1.Columns.Count > 0)
                            {
                                dataGridView1.Columns["id_makanan"]
                                    .HeaderText =
                                    "ID Makanan";

                                dataGridView1.Columns["nama_makanan"]
                                    .HeaderText =
                                    "Nama Makanan";

                                dataGridView1.Columns["kalori"]
                                    .HeaderText =
                                    "Kalori";

                                dataGridView1.Columns["protein"]
                                    .HeaderText =
                                    "Protein";

                                dataGridView1.Columns["lemak"]
                                    .HeaderText =
                                    "Lemak";

                                dataGridView1.Columns["karbohidrat"]
                                    .HeaderText =
                                    "Karbohidrat";
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
        private void txtSearch_TextChanged(
            object sender,
            EventArgs e
        )
        {
            LoadData(txtSearch.Text);
        }

        // =====================================================
        // INSERT
        // =====================================================
        private void btnInsert_Click(
            object sender,
            EventArgs e
        )
        {
            try
            {
                // VALIDASI ANGKA
                bool validKalori =
                    double.TryParse(
                        txtKalori.Text,
                        out double kalori
                    );

                bool validProtein =
                    double.TryParse(
                        txtProtein.Text,
                        out double protein
                    );

                bool validLemak =
                    double.TryParse(
                        txtLemak.Text,
                        out double lemak
                    );

                bool validKarbo =
                    double.TryParse(
                        txtKarbohidrat.Text,
                        out double karbohidrat
                    );

                // VALIDASI HURUF
                if (!validKalori ||
                    !validProtein ||
                    !validLemak ||
                    !validKarbo)
                {
                    MessageBox.Show(
                        "Data harus berupa angka"
                    );

                    return;
                }

                // VALIDASI MINUS
                if (kalori < 0 ||
                    protein < 0 ||
                    lemak < 0 ||
                    karbohidrat < 0)
                {
                    MessageBox.Show(
                        "Nilai tidak boleh minus"
                    );

                    return;
                }

                using (SqlConnection conn =
                    new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd =
                        new SqlCommand(
                            "sp_InsertNutrisi",
                            conn))
                    {
                        cmd.CommandType =
                            CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue(
                            "@id_makanan",
                            cmbMakanan.SelectedValue
                        );

                        cmd.Parameters.AddWithValue(
                            "@kalori",
                            kalori
                        );

                        cmd.Parameters.AddWithValue(
                            "@protein",
                            protein
                        );

                        cmd.Parameters.AddWithValue(
                            "@lemak",
                            lemak
                        );

                        cmd.Parameters.AddWithValue(
                            "@karbohidrat",
                            karbohidrat
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
        // UPDATE
        // =====================================================
        private void btnUpdate_Click(
            object sender,
            EventArgs e
        )
        {
            try
            {
                if (bs.Current == null)
                {
                    MessageBox.Show(
                        "Pilih data terlebih dahulu"
                    );

                    return;
                }

                // VALIDASI ANGKA
                bool validKalori =
                    double.TryParse(
                        txtKalori.Text,
                        out double kalori
                    );

                bool validProtein =
                    double.TryParse(
                        txtProtein.Text,
                        out double protein
                    );

                bool validLemak =
                    double.TryParse(
                        txtLemak.Text,
                        out double lemak
                    );

                bool validKarbo =
                    double.TryParse(
                        txtKarbohidrat.Text,
                        out double karbohidrat
                    );

                // VALIDASI HURUF
                if (!validKalori ||
                    !validProtein ||
                    !validLemak ||
                    !validKarbo)
                {
                    MessageBox.Show(
                        "Data harus berupa angka"
                    );

                    return;
                }

                // VALIDASI MINUS
                if (kalori < 0 ||
                    protein < 0 ||
                    lemak < 0 ||
                    karbohidrat < 0)
                {
                    MessageBox.Show(
                        "Nilai tidak boleh minus"
                    );

                    return;
                }

                // AMBIL ID
                string id =
                    ((DataRowView)bs.Current)["id_makanan"]
                    .ToString();

                using (SqlConnection conn =
                    new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd =
                        new SqlCommand(
                            "sp_UpdateNutrisi",
                            conn))
                    {
                        cmd.CommandType =
                            CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue(
                            "@id_makanan",
                            int.Parse(id)
                        );

                        cmd.Parameters.AddWithValue(
                            "@kalori",
                            kalori
                        );

                        cmd.Parameters.AddWithValue(
                            "@protein",
                            protein
                        );

                        cmd.Parameters.AddWithValue(
                            "@lemak",
                            lemak
                        );

                        cmd.Parameters.AddWithValue(
                            "@karbohidrat",
                            karbohidrat
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

        private void btnDelete_Click(
            object sender,
            EventArgs e
        )
        {
            try
            {
                if (bs.Current == null)
                {
                    MessageBox.Show(
                        "Pilih data terlebih dahulu"
                    );

                    return;
                }

                string id =
                    ((DataRowView)bs.Current)["id_makanan"]
                    .ToString();

                string namaMakanan =
                    ((DataRowView)bs.Current)["nama_makanan"]
                    .ToString();

                // CEK APAKAH MAKANAN SUDAH PERNAH DIKONSUMSI
                using (SqlConnection connCek =
                    new SqlConnection(connectionString))
                {
                    connCek.Open();

                    using (SqlCommand cmdCek = new SqlCommand(
                        "SELECT COUNT(*) FROM KonsumsiMakanan WHERE id_makanan = @id",
                        connCek))
                    {
                        cmdCek.Parameters.AddWithValue("@id", int.Parse(id));
                        int jumlahKonsumsi = (int)cmdCek.ExecuteScalar();

                        if (jumlahKonsumsi > 0)
                        {
                            MessageBox.Show(
                                "⚠️ Makanan \"" + namaMakanan + "\" tidak dapat dihapus sepenuhnya\n\n" +
                                "Makanan ini sudah tercatat dalam " + jumlahKonsumsi + " riwayat konsumsi user.\n\n" +
                                "Data nutrisinya akan dihapus, tetapi nama makanan tetap disimpan\n" +
                                "untuk menjaga integritas riwayat konsumsi.",
                                "Tidak Bisa Dihapus Penuh",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                            return;
                        }
                    }
                }

                DialogResult confirm =
                    MessageBox.Show(
                        "Yakin ingin menghapus data \"" + namaMakanan + "\"?\n" +
                        "Data makanan dan nutrisinya akan dihapus permanen.",
                        "Konfirmasi Hapus",
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
                            "sp_DeleteNutrisi",
                            conn))
                    {
                        cmd.CommandType =
                            CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue(
                            "@id_makanan",
                            int.Parse(id)
                        );

                        cmd.ExecuteNonQuery();

                        MessageBox.Show(
                            "Data \"" + namaMakanan + "\" berhasil dihapus.",
                            "Berhasil",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        ClearForm();
                        LoadData();
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
        // CLEAR FORM
        // =====================================================
        private void ClearForm()
        {
            txtKalori.Clear();

            txtProtein.Clear();

            txtLemak.Clear();

            txtKarbohidrat.Clear();
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
        private void bindingNavigator1_RefreshItems(
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

        private void dataGridView1_CellContentClick(
            object sender,
            DataGridViewCellEventArgs e
        )
        {

        }



        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void SetImportingState(bool isImporting)
        {
            dataGridView1.Enabled = !isImporting;

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
            openFileDialog.Title = "Pilih File Excel Data Nutrisi";

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
                    int colKaloriIdx = -1;
                    int colProteinIdx = -1;
                    int colLemakIdx = -1;
                    int colKarboIdx = -1;

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string colName = dt.Columns[i].ColumnName.ToLower();
                        if (colName.Contains("makanan") || colName.Contains("nama"))
                        {
                            colMakananIdx = i;
                        }
                        else if (colName.Contains("kalori") || colName.Contains("cal"))
                        {
                            colKaloriIdx = i;
                        }
                        else if (colName.Contains("protein") || colName.Contains("prot"))
                        {
                            colProteinIdx = i;
                        }
                        else if (colName.Contains("lemak") || colName.Contains("fat"))
                        {
                            colLemakIdx = i;
                        }
                        else if (colName.Contains("karbo") || colName.Contains("carb"))
                        {
                            colKarboIdx = i;
                        }
                    }

                    // Default mapping jika tidak terdeteksi
                    if (colMakananIdx == -1 && dt.Columns.Count > 0) colMakananIdx = 0;
                    if (colKaloriIdx == -1 && dt.Columns.Count > 1) colKaloriIdx = 1;
                    if (colProteinIdx == -1 && dt.Columns.Count > 2) colProteinIdx = 2;
                    if (colLemakIdx == -1 && dt.Columns.Count > 3) colLemakIdx = 3;
                    if (colKarboIdx == -1 && dt.Columns.Count > 4) colKarboIdx = 4;

                    if (colMakananIdx == -1 || colKaloriIdx == -1 || colProteinIdx == -1 || colLemakIdx == -1 || colKarboIdx == -1)
                    {
                        MessageBox.Show("Format kolom Excel tidak lengkap. Pastikan ada kolom Makanan, Kalori, Protein, Lemak, dan Karbohidrat.", "Format Salah", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Buat preview DataTable dengan struktur yang sesuai dengan grid
                    DataTable previewTable = new DataTable();
                    previewTable.Columns.Add("id_makanan", typeof(string));
                    previewTable.Columns.Add("nama_makanan", typeof(string));
                    previewTable.Columns.Add("kalori", typeof(double));
                    previewTable.Columns.Add("protein", typeof(double));
                    previewTable.Columns.Add("lemak", typeof(double));
                    previewTable.Columns.Add("karbohidrat", typeof(double));

                    foreach (DataRow row in dt.Rows)
                    {
                        string namaMakanan = row[colMakananIdx]?.ToString()?.Trim();
                        if (string.IsNullOrEmpty(namaMakanan))
                            continue;

                        double.TryParse(row[colKaloriIdx]?.ToString(), out double kalori);
                        double.TryParse(row[colProteinIdx]?.ToString(), out double protein);
                        double.TryParse(row[colLemakIdx]?.ToString(), out double lemak);
                        double.TryParse(row[colKarboIdx]?.ToString(), out double karbohidrat);

                        previewTable.Rows.Add("PREVIEW", namaMakanan, kalori, protein, lemak, karbohidrat);
                    }

                    if (previewTable.Rows.Count == 0)
                    {
                        MessageBox.Show("Tidak ada data nutrisi valid untuk di-import.", "Data Kosong", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Simpan ke private member
                    importedNutrisiPreview = previewTable;

                    // Bind ke grid agar tampil ke layar
                    dataGridView1.DataSource = importedNutrisiPreview;

                    // Set control state to importing
                    SetImportingState(true);

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
            if (importedNutrisiPreview == null || importedNutrisiPreview.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data preview untuk diimport ke database.\nSilakan klik 'Import from Excel' terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show($"Yakin ingin mengimpor {importedNutrisiPreview.Rows.Count} data dari preview ke database?", "Konfirmasi Import", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            try
            {
                int insertCount = 0;
                int updateCount = 0;
                int skipCount = 0;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Ambil atau buat ID Kategori Default "Umum" jika Makanan baru perlu dibuat
                    int defaultIdKategori = -1;
                    string qDefaultKat = "SELECT id_kategori FROM KategoriMakanan WHERE LOWER(nama_kategori) = 'umum'";
                    using (SqlCommand cmdKat = new SqlCommand(qDefaultKat, connection))
                    {
                        object res = cmdKat.ExecuteScalar();
                        if (res != null)
                        {
                            defaultIdKategori = Convert.ToInt32(res);
                        }
                        else
                        {
                            string insKat = "INSERT INTO KategoriMakanan (nama_kategori) OUTPUT INSERTED.id_kategori VALUES ('Umum')";
                            using (SqlCommand cmdInsKat = new SqlCommand(insKat, connection))
                            {
                                defaultIdKategori = Convert.ToInt32(cmdInsKat.ExecuteScalar());
                            }
                        }
                    }

                    foreach (DataRow row in importedNutrisiPreview.Rows)
                    {
                        string namaMakanan = row["nama_makanan"]?.ToString()?.Trim();
                        if (string.IsNullOrEmpty(namaMakanan))
                        {
                            skipCount++;
                            continue;
                        }

                        double.TryParse(row["kalori"]?.ToString(), out double kalori);
                        double.TryParse(row["protein"]?.ToString(), out double protein);
                        double.TryParse(row["lemak"]?.ToString(), out double lemak);
                        double.TryParse(row["karbohidrat"]?.ToString(), out double karbohidrat);

                        // Cek/buat Makanan
                        int idMakanan = -1;
                        string qMakanan = "SELECT id_makanan FROM Makanan WHERE LOWER(nama_makanan) = LOWER(@nama_makanan)";
                        using (SqlCommand cmdM = new SqlCommand(qMakanan, connection))
                        {
                            cmdM.Parameters.AddWithValue("@nama_makanan", namaMakanan);
                            object res = cmdM.ExecuteScalar();
                            if (res != null)
                            {
                                idMakanan = Convert.ToInt32(res);
                            }
                            else
                            {
                                // Masukkan Makanan baru dengan kategori default "Umum"
                                string insM = "INSERT INTO Makanan (id_kategori, nama_makanan) OUTPUT INSERTED.id_makanan VALUES (@id_kategori, @nama_makanan)";
                                using (SqlCommand cmdInsM = new SqlCommand(insM, connection))
                                {
                                    cmdInsM.Parameters.AddWithValue("@id_kategori", defaultIdKategori);
                                    cmdInsM.Parameters.AddWithValue("@nama_makanan", namaMakanan);
                                    idMakanan = Convert.ToInt32(cmdInsM.ExecuteScalar());
                                }
                            }
                        }

                        // Cek apakah nutrisi sudah terdaftar untuk id_makanan ini (Upsert)
                        string qNutrisi = "SELECT COUNT(*) FROM Nutrisi WHERE id_makanan = @id_makanan";
                        bool exists = false;
                        using (SqlCommand cmdN = new SqlCommand(qNutrisi, connection))
                        {
                            cmdN.Parameters.AddWithValue("@id_makanan", idMakanan);
                            exists = Convert.ToInt32(cmdN.ExecuteScalar()) > 0;
                        }

                        if (exists)
                        {
                            // Update
                            using (SqlCommand cmdUpd = new SqlCommand("sp_UpdateNutrisi", connection))
                            {
                                cmdUpd.CommandType = CommandType.StoredProcedure;
                                cmdUpd.Parameters.AddWithValue("@id_makanan", idMakanan);
                                cmdUpd.Parameters.AddWithValue("@kalori", kalori);
                                cmdUpd.Parameters.AddWithValue("@protein", protein);
                                cmdUpd.Parameters.AddWithValue("@lemak", lemak);
                                cmdUpd.Parameters.AddWithValue("@karbohidrat", karbohidrat);
                                cmdUpd.ExecuteNonQuery();
                                updateCount++;
                            }
                        }
                        else
                        {
                            // Insert
                            using (SqlCommand cmdIns = new SqlCommand("sp_InsertNutrisi", connection))
                            {
                                cmdIns.CommandType = CommandType.StoredProcedure;
                                cmdIns.Parameters.AddWithValue("@id_makanan", idMakanan);
                                cmdIns.Parameters.AddWithValue("@kalori", kalori);
                                cmdIns.Parameters.AddWithValue("@protein", protein);
                                cmdIns.Parameters.AddWithValue("@lemak", lemak);
                                cmdIns.Parameters.AddWithValue("@karbohidrat", karbohidrat);
                                cmdIns.ExecuteNonQuery();
                                insertCount++;
                            }
                        }
                    }
                }

                // Reset preview data
                importedNutrisiPreview = null;

                // Reset control state
                SetImportingState(false);

                // Reload database data & update bindings
                LoadMakananComboBox();
                LoadData();

                MessageBox.Show($"Import ke database selesai!\nBerhasil ditambahkan: {insertCount} data\nBerhasil diupdate: {updateCount} data\nLewati: {skipCount} data", "Import Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat mengimpor data ke database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}