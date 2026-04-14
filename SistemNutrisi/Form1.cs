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

                // 1. Cek di tabel Admin
                string queryAdmin = @"SELECT id_admin, nama FROM Admin WHERE email = @email AND password = @pass";
                SqlCommand cmdAdmin = new SqlCommand(queryAdmin, conn);
                cmdAdmin.Parameters.AddWithValue("@email", txtEmail.Text);
                cmdAdmin.Parameters.AddWithValue("@pass", txtPassword.Text);

                SqlDataReader readerAdmin = cmdAdmin.ExecuteReader();
                if (readerAdmin.Read())
                {
                    int id = Convert.ToInt32(readerAdmin["id_admin"]);
                    string nama = readerAdmin["nama"].ToString();
                    readerAdmin.Close();

                    this.Hide();
                    FormAdmin formAdmin = new FormAdmin(id, nama);
                    formAdmin.Show();
                    return;
                }
                readerAdmin.Close();

                // 2. Cek di tabel User jika tidak ada di Admin
                string queryUser = @"SELECT id_user, nama FROM [User] WHERE email = @email AND password = @pass";
                SqlCommand cmdUser = new SqlCommand(queryUser, conn);
                cmdUser.Parameters.AddWithValue("@email", txtEmail.Text);
                cmdUser.Parameters.AddWithValue("@pass", txtPassword.Text);

                SqlDataReader readerUser = cmdUser.ExecuteReader();
                if (readerUser.Read())
                {
                    int id = Convert.ToInt32(readerUser["id_user"]);
                    string nama = readerUser["nama"].ToString();
                    readerUser.Close();

                    this.Hide();
                    FormUser formUser = new FormUser(id, nama);
                    formUser.Show();
                    return;
                }
                readerUser.Close();

                // 3. Jika tidak ditemukan di keduanya
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
