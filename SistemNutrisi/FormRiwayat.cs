using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SistemNutrisi
{
    public partial class FormRiwayat : Form
    {
        // =====================================================
        // CONNECTION STRING
        // =====================================================
        private readonly string connectionString =
            "Data Source=IZAYAAA\\IZA;Initial Catalog=DBSistemNutrisi;Integrated Security=True";



        // =====================================================
        // USER LOGIN
        // =====================================================
        private int idUser;
        private string namaUser; 

        // =====================================================
        // BINDING SOURCE
        // =====================================================
        private BindingSource bs =
            new BindingSource();

        // =====================================================
        // CONSTRUCTOR
        // =====================================================
        public FormRiwayat(
            int idUser,
            string namaUser
        )
        {
            InitializeComponent();

            this.idUser = idUser;
            this.namaUser = namaUser;


        }

        // =====================================================
        // FORM LOAD
        // =====================================================
        private void FormRiwayat_Load(
            object sender,
            EventArgs e
        )
        {
            btnBack.BringToFront();

            // Sembunyikan header jika di-embed sebagai child control agar tidak double header dan menghilangkan tombol kembali
            if (!this.TopLevel)
            {
                pnlHeader.Visible = false;
                btnBack.Visible = false;
            }

            // BINDING CONTEXT INDEPENDENT
            this.BindingContext = new BindingContext();

            // LABEL USER
            lblInfo.Text =
                "Riwayat Konsumsi : "
                + namaUser;

            // SETUP GRID
            SetupDataGridView();

            // HUBUNGKAN DATAGRIDVIEW KE BINDINGSOURCE
            dataGridView1.DataSource = bs;

            // HUBUNGKAN NAVIGATOR KE BINDINGSOURCE
            bindingNavigator1.BindingSource = bs;

            // OPTIONAL
            bindingNavigator1.AddNewItem = null;
            bindingNavigator1.DeleteItem = null;

            // Tambahkan tombol Rekap & Cetak ke BindingNavigator
            ToolStripButton btnCetakNav = new ToolStripButton();
            btnCetakNav.Text = "Rekap & Cetak Laporan";
            btnCetakNav.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCetakNav.ForeColor = Color.FromArgb(46, 204, 113);
            btnCetakNav.Click += (s, ev) => {
                FormRekap rekapForm = new FormRekap(idUser, namaUser);
                rekapForm.ShowDialog();
            };
            bindingNavigator1.Items.Add(new ToolStripSeparator());
            bindingNavigator1.Items.Add(btnCetakNav);

            // EVENT UNTUK SYNCHRONIZE NAVIGASI KE SELEKSI GRID
            bs.PositionChanged += (s, ev) =>
            {
                if (bs.Position >= 0 && bs.Position < dataGridView1.Rows.Count)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[bs.Position].Selected = true;
                    if (dataGridView1.Columns.Count > 0)
                    {
                        try
                        {
                            dataGridView1.CurrentCell = dataGridView1.Rows[bs.Position].Cells[0];
                        }
                        catch (Exception)
                        {
                            // Abaikan jika cell belum siap ditampilkan
                        }
                    }
                }
            };

            // LOAD DATA
            LoadData();

            // REFRESH
            bindingNavigator1.Refresh();
        }

        // =====================================================
        // SETUP DATAGRIDVIEW
        // =====================================================
        private void SetupDataGridView()
        {
            // SELECT FULL ROW
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            // READ ONLY
            dataGridView1.ReadOnly = true;

            // DISABLE ADD ROW
            dataGridView1.AllowUserToAddRows = false;

            // AUTO SIZE
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // SCROLLBAR
            dataGridView1.ScrollBars = ScrollBars.Both;

            // NORMAL BAWAAN WINDOWS TANPA DESAIN
            dataGridView1.EnableHeadersVisualStyles = true;
            dataGridView1.ColumnHeadersDefaultCellStyle = null;
            dataGridView1.DefaultCellStyle = null;
            dataGridView1.AlternatingRowsDefaultCellStyle = null;
            dataGridView1.BackgroundColor = SystemColors.AppWorkspace;
            dataGridView1.BorderStyle = BorderStyle.Fixed3D;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.GridColor = SystemColors.ControlDark;
        }

        // =====================================================
        // LOAD DATA
        // =====================================================
        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_GetRiwayatKonsumsi", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_user", idUser);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // MASUKKAN KE BINDINGSOURCE
                            bs.DataSource = dt;

                            // REFRESH BINDING
                            bs.ResetBindings(false);

                            // REFRESH GRID
                            dataGridView1.Refresh();

                            // REFRESH NAVIGATOR
                            bindingNavigator1.Refresh();

                            // HEADER KOLOM
                            if (dataGridView1.Columns.Count > 0)
                            {
                                dataGridView1.Columns["tanggal"]
                                    .HeaderText = "Tanggal";

                                dataGridView1.Columns["nama_makanan"]
                                    .HeaderText = "Makanan";

                                dataGridView1.Columns["jumlah"]
                                    .HeaderText = "Jumlah";

                                dataGridView1.Columns["total_kalori"]
                                    .HeaderText = "Kalori";

                                dataGridView1.Columns["total_protein"]
                                    .HeaderText = "Protein";

                                dataGridView1.Columns["total_lemak"]
                                    .HeaderText = "Lemak";

                                dataGridView1.Columns["total_karbohidrat"]
                                    .HeaderText = "Karbohidrat";
                            }

                            // JIKA DATA KOSONG
                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show(
                                    "Data riwayat belum tersedia.",
                                    "Informasi",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information
                                );
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
        // BUTTON BACK
        // =====================================================
        private void btnBack_Click(
            object sender,
            EventArgs e
        )
        {
            this.Close();
        }

        private void btnCetakLaporan_Click(object sender, EventArgs e)
        {
            FormRekap rekapForm = new FormRekap(idUser, namaUser);
            rekapForm.ShowDialog();
        }

        // =====================================================
        // EVENT NAVIGATOR
        // =====================================================
        private void bindingNavigator1_RefreshItems(
            object sender,
            EventArgs e
        )
        {

        }
    }
}