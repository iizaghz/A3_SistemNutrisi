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
        private readonly string connectionString =
            "Data Source=IZAYAAA\\IZA;Initial Catalog=DBSistemNutrisi;Integrated Security=True";

        private int idUser;
        private string namaUser;
        private BindingSource bs = new BindingSource();
        private BindingNavigator bn;

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

            btnLoad.PerformClick();
        }

        private void loadForm(object Form)
        {
            if (bn != null) bn.Visible = false;

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
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (bn != null) bn.Visible = true;

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

        // =====================================================
        // EMPTY EVENT HANDLERS
        // =====================================================
        private void lblWelcome_Click(object sender, EventArgs e)
        {
        }
    }
}
