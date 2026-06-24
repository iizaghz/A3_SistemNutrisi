using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemNutrisi
{
    public partial class FormRekap : Form
    {
        private readonly string connectionString =
            "Data Source=IZAYAAA\\IZA;Initial Catalog=DBSistemNutrisi;Integrated Security=True";

        private int idUser;
        private string namaUser;
        private DataTable dtRekap;

        public FormRekap(int idUser, string namaUser)
        {
            InitializeComponent();
            this.idUser = idUser;
            this.namaUser = namaUser;
        }

        private void FormRekap_Load(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Today.AddDays(-7);
            dtpEnd.Value = DateTime.Today;

            SetupDataGridView();
            btnCetak.Enabled = false;

            // Load default data
            LoadData();
        }

        private void SetupDataGridView()
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = System.Drawing.Color.White;
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT k.tanggal, m.nama_makanan, k.jumlah,
                               (n.kalori * k.jumlah) AS total_kalori,
                               (n.protein * k.jumlah) AS total_protein,
                               (n.lemak * k.jumlah) AS total_lemak,
                               (n.karbohidrat * k.jumlah) AS total_karbohidrat
                        FROM KonsumsiMakanan k
                        JOIN Makanan m ON k.id_makanan = m.id_makanan
                        LEFT JOIN Nutrisi n ON m.id_makanan = n.id_makanan
                        WHERE k.id_user = @id_user AND k.tanggal >= @start_date AND k.tanggal <= @end_date
                        ORDER BY k.tanggal DESC;";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_user", idUser);
                        cmd.Parameters.AddWithValue("@start_date", dtpStart.Value.Date);
                        cmd.Parameters.AddWithValue("@end_date", dtpEnd.Value.Date);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            dtRekap = new DataTable();
                            adapter.Fill(dtRekap);

                            dataGridView1.DataSource = dtRekap;

                            // Formating columns
                            if (dataGridView1.Columns.Count > 0)
                            {
                                dataGridView1.Columns["tanggal"].HeaderText = "Tanggal";
                                dataGridView1.Columns["nama_makanan"].HeaderText = "Makanan";
                                dataGridView1.Columns["jumlah"].HeaderText = "Jumlah";
                                dataGridView1.Columns["total_kalori"].HeaderText = "Kalori (kcal)";
                                dataGridView1.Columns["total_protein"].HeaderText = "Protein (g)";
                                dataGridView1.Columns["total_lemak"].HeaderText = "Lemak (g)";
                                dataGridView1.Columns["total_karbohidrat"].HeaderText = "Karbohidrat (g)";
                            }

                            if (dtRekap.Rows.Count > 0)
                            {
                                btnCetak.Enabled = true;
                            }
                            else
                            {
                                btnCetak.Enabled = false;
                                MessageBox.Show("Tidak ada data konsumsi untuk rentang tanggal tersebut.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data rekap: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnCetak.Enabled = false;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (dtpStart.Value.Date > dtpEnd.Value.Date)
            {
                MessageBox.Show("Tanggal mulai tidak boleh melebihi tanggal selesai.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            LoadData();
        }

        private void btnCetak_Click(object sender, EventArgs e)
        {
            if (dtRekap == null || dtRekap.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data untuk dicetak.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Open the print preview / report form
            FormCetak printForm = new FormCetak(dtRekap, namaUser, dtpStart.Value.Date, dtpEnd.Value.Date);
            printForm.ShowDialog();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
