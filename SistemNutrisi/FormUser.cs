using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemNutrisi
{
    public partial class FormUser : Form
    {
        // =====================================================
        // CONNECTION STRING & FIELDS
        // =====================================================
        private readonly SqlConnection conn;
        private readonly string connectionString = DAL.GetConnectionString();

        private int idUser;
        private string namaUser;
        private BindingSource bs = new BindingSource();
        private BindingNavigator bn;
        private Button btnRekapCetak;

        // =====================================================
        // CONSTRUCTOR
        // =====================================================
        public FormUser(int idUser, string namaUser)
        {
            InitializeComponent();
            this.idUser = idUser;
            this.namaUser = namaUser;
            conn = new SqlConnection(connectionString);

            // Atur agar form otomatis maximized / full screen
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new System.Drawing.Size(1200, 700);
        }

        // =====================================================
        // FORM LOAD & HELPER
        // =====================================================
        private void FormUser_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "Selamat datang, " + namaUser;

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
            bn.BringToFront();

            dataGridView1.Dock = DockStyle.Fill;

            // Inisialisasi Button Rekap & Cetak di pnlHeader
            btnRekapCetak = new Button();
            btnRekapCetak.Text = "Rekap Cetak Laporan";
            btnRekapCetak.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnRekapCetak.BackColor = System.Drawing.Color.FromArgb(46, 204, 113); // Green (#2ecc71)
            btnRekapCetak.ForeColor = System.Drawing.Color.White;
            btnRekapCetak.FlatStyle = FlatStyle.Flat;
            btnRekapCetak.FlatAppearance.BorderSize = 0;
            btnRekapCetak.Size = new System.Drawing.Size(200, 40);
            // Center vertically in pnlHeader (height is 74, Y = (74 - 40) / 2 = 17)
            btnRekapCetak.Location = new System.Drawing.Point(pnlHeader.Width - btnRekapCetak.Width - 20, 17);
            btnRekapCetak.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRekapCetak.Cursor = Cursors.Hand;
            btnRekapCetak.Visible = false;
            btnRekapCetak.Click += btnRekapCetak_Click;
            pnlHeader.Controls.Add(btnRekapCetak);

            btnDashboard.PerformClick();
        }

        private void loadForm(object Form)
        {
            if (bn != null) bn.Visible = false;

            if (btnRekapCetak != null)
            {
                btnRekapCetak.Visible = (Form is FormRiwayat);
            }

            if (this.pnlContent.Controls.Count > 0)
                this.pnlContent.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            f.FormBorderStyle = FormBorderStyle.None;
            this.pnlContent.Controls.Add(f);
            this.pnlContent.Tag = f;
            f.Show();
        }

        // =====================================================
        // BUTTON NAVIGATION ACTIONS
        // =====================================================
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            loadForm(new FormDashboard(idUser));
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (bn != null) bn.Visible = true;
            if (btnRekapCetak != null) btnRekapCetak.Visible = false;

            // Kembalikan dataGridView1 ke pnlContent jika sedang menampilkan form lain
            if (!this.pnlContent.Controls.Contains(dataGridView1))
            {
                this.pnlContent.Controls.Clear();
                this.pnlContent.Controls.Add(dataGridView1);
            }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetNutrisiUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                bs.DataSource = dt;
                dataGridView1.DataSource = bs;

                // Mempercantik Header
                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["nama_makanan"].HeaderText = "Makanan";
                    dataGridView1.Columns["kalori"].HeaderText = "Kalori";
                    dataGridView1.Columns["protein"].HeaderText = "Protein";
                    dataGridView1.Columns["lemak"].HeaderText = "Lemak";
                    dataGridView1.Columns["karbohidrat"].HeaderText = "Karbohidrat";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message);
            }
        }

        private void btnKonsumsi_Click(object sender, EventArgs e)
        {
            loadForm(new FormKonsumsi(idUser));
        }

        private void btnRiwayat_Click(object sender, EventArgs e)
        {
            loadForm(new FormRiwayat(idUser, namaUser));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 formLogin = new Form1();
            formLogin.Show();
        }

        private void btnRekapCetak_Click(object sender, EventArgs e)
        {
            FormRekap rekapForm = new FormRekap(idUser, namaUser);
            rekapForm.ShowDialog();
        }

        // =====================================================
        // EMPTY EVENT HANDLERS
        // =====================================================
        private void lblWelcome_Click(object sender, EventArgs e)
        {
        }
    }
}
