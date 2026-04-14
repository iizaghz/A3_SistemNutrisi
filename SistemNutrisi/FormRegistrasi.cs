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
    public partial class FormRegistrasi : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString =
            "Data Source=IZAYAAA\\IZA;Initial Catalog=DBSistemNutrisi;Integrated Security=True";

        public FormRegistrasi()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void FormRegistrasi_Load(object sender, EventArgs e)
        {
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

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

                string query = @"INSERT INTO [User] (nama, email, password) VALUES (@nama, @email, @pass)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nama", txtNama.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@pass", txtPassword.Text);

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
