using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemNutrisi
{
    public partial class Form1 : Form
    {
        // =====================================================
        // CONNECTION STRING & FIELDS
        // =====================================================
        private readonly SqlConnection conn;
        private readonly string connectionString = DAL.GetConnectionString();

        // =====================================================
        // CONSTRUCTOR
        // =====================================================
        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);

            // Atur agar form otomatis maximized / full screen
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.WindowState = FormWindowState.Maximized;
        }

        // =====================================================
        // FORM LOAD
        // =====================================================
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal terhubung ke Database: " + ex.Message, "Error Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================
        // ACTION BUTTONS
        // =====================================================
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                MessageBox.Show("Koneksi Berhasil!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_Login", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = Convert.ToInt32(reader["id_user"]);
                    string nama = reader["nama"].ToString();
                    string role = reader["role"].ToString();
                    reader.Close();

                    this.Hide();
                    if (role == "admin")
                    {
                        FormAdmin formAdmin = new FormAdmin(id, nama);
                        formAdmin.Show();
                    }
                    else
                    {
                        FormUser formUser = new FormUser(id, nama);
                        formUser.Show();
                    }
                    return;
                }
                reader.Close();
                MessageBox.Show("Email atau Password salah!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            FormRegistrasi reg = new FormRegistrasi();
            reg.Show();
        }

        // =====================================================
        // TEXT CHANGED & EVENT HANDLERS
        // =====================================================
        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
