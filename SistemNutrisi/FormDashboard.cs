using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SistemNutrisi
{
    public partial class FormDashboard : Form
    {
        private readonly string connectionString = DAL.GetConnectionString();

        private int idUser;

        private double totalKalori  = 0;
        private double totalProtein = 0;
        private double totalLemak   = 0;
        private double totalKarbo   = 0;

        private double targetKalori  = 2000;
        private double targetProtein = 60;
        private double targetLemak   = 70;
        private double targetKarbo   = 300;

        public FormDashboard(int idUser)
        {
            InitializeComponent();
            this.idUser = idUser;
        }

        private void FormDashboard_Load(object sender, EventArgs e)
        {
            cmbChartType.SelectedIndex = 0; // Default to Pie Chart
            dtpTanggal.Value = DateTime.Today;
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_GetDashboardSummary", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_user", idUser);
                        cmd.Parameters.AddWithValue("@tanggal", dtpTanggal.Value.Date);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                totalKalori  = Convert.ToDouble(reader["total_kalori"]);
                                totalProtein = Convert.ToDouble(reader["total_protein"]);
                                totalLemak   = Convert.ToDouble(reader["total_lemak"]);
                                totalKarbo   = Convert.ToDouble(reader["total_karbohidrat"]);

                                targetKalori  = Convert.ToDouble(reader["target_kalori"]);
                                targetProtein = Convert.ToDouble(reader["target_protein"]);
                                targetLemak   = Convert.ToDouble(reader["target_lemak"]);
                                targetKarbo   = Convert.ToDouble(reader["target_karbohidrat"]);
                            }
                            else
                            {
                                ResetData();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data dashboard: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetData();
            }

            UpdateChart();
            UpdateUIValues();
        }

        private void ResetData()
        {
            totalKalori  = 0;
            totalProtein = 0;
            totalLemak   = 0;
            totalKarbo   = 0;
            targetKalori  = 2000;
            targetProtein = 60;
            targetLemak   = 70;
            targetKarbo   = 300;
        }

        /// <summary>Pushes live data into ChartPanel properties and redraws.</summary>
        private void UpdateChart()
        {
            pnlChart.Protein   = totalProtein;
            pnlChart.Lemak     = totalLemak;
            pnlChart.Karbo     = totalKarbo;
            pnlChart.ChartType = cmbChartType.SelectedIndex < 0 ? 0 : cmbChartType.SelectedIndex;
            // Invalidate is called automatically by property setters inside ChartPanel
        }

        private void UpdateUIValues()
        {
            double pctKalori  = targetKalori  > 0 ? (totalKalori  / targetKalori)  * 100 : 0;
            double pctProtein = targetProtein > 0 ? (totalProtein / targetProtein) * 100 : 0;
            double pctLemak   = targetLemak   > 0 ? (totalLemak   / targetLemak)   * 100 : 0;
            double pctKarbo   = targetKarbo   > 0 ? (totalKarbo   / targetKarbo)   * 100 : 0;

            lblKaloriVal.Text  = string.Format("{0:0} / {1:0} kcal ({2:0}%)",  totalKalori,  targetKalori,  pctKalori);
            lblProteinVal.Text = string.Format("{0:0.0} / {1:0} g ({2:0}%)",   totalProtein, targetProtein, pctProtein);
            lblLemakVal.Text   = string.Format("{0:0.0} / {1:0} g ({2:0}%)",   totalLemak,   targetLemak,   pctLemak);
            lblKarboVal.Text   = string.Format("{0:0.0} / {1:0} g ({2:0}%)",   totalKarbo,   targetKarbo,   pctKarbo);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        private void dtpTanggal_ValueChanged(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        private void cmbChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Just update chart type; data stays the same
            pnlChart.ChartType = cmbChartType.SelectedIndex < 0 ? 0 : cmbChartType.SelectedIndex;
        }

        // Accent borders on card controls for sleek modern UI design
        private void Card_Paint(object sender, PaintEventArgs e)
        {
            Panel card = sender as Panel;
            if (card == null) return;

            using (Pen pen = new Pen(Color.FromArgb(50, Color.White), 2))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawRectangle(pen, 1, 1, card.Width - 3, card.Height - 3);
            }
        }
    }
}
