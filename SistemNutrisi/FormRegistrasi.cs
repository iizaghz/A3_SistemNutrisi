using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemNutrisi
{
    public partial class FormRegistrasi : Form
    {
        // =====================================================
        // CONNECTION STRING & FIELDS
        // =====================================================
        private readonly SqlConnection conn;
        private readonly string connectionString =
            "Data Source=IZAYAAA\\IZA;Initial Catalog=DBSistemNutrisi;Integrated Security=True";

        // =====================================================
        // CONSTRUCTOR
        // =====================================================
        public FormRegistrasi()
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
        private void FormRegistrasi_Load(object sender, EventArgs e)
        {
        }

        // =====================================================
        // BUTTON ACTIONS
        // =====================================================
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNama.Text)) 
                { 
                    MessageBox.Show("Nama harus diisi"); 
                    txtNama.Focus(); 
                    return; 
                }
                if (string.IsNullOrEmpty(txtEmail.Text)) 
                { 
                    MessageBox.Show("Email harus diisi"); 
                    txtEmail.Focus(); 
                    return; 
                }
                if (string.IsNullOrEmpty(txtPassword.Text)) 
                { 
                    MessageBox.Show("Password harus diisi"); 
                    txtPassword.Focus(); 
                    return; 
                }

                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_Registrasi", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nama", txtNama.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                cmd.Parameters.AddWithValue("@role", "user");

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Registrasi berhasil!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Registrasi gagal");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }
    }
}
