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
    public partial class Form1 : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString =
            "Data Source=IZAYAAA\\IZA;Initial Catalog=DBSistemNutrisi;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
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
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                // Cek login menggunakan tabel [User] dengan kolom role
                string query = @"SELECT id_user, nama, role FROM [User] WHERE email = @email AND password = @pass";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@pass", txtPassword.Text);

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

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                // Jika koneksi berhasil, aplikasi lanjut. Jika gagal akan ke catch.
                // Bisa menambahkan indikator status di UI jika diperlukan.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal terhubung ke Database: " + ex.Message, "Error Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
