using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace SistemNutrisi
{
    public partial class FormCetak : Form
    {
        private DataTable dtReport;
        private string namaUser;
        private DateTime startDate;
        private DateTime endDate;

        public FormCetak(DataTable dt, string user, DateTime start, DateTime end)
        {
            InitializeComponent();
            this.dtReport = dt;
            this.namaUser = user;
            this.startDate = start;
            this.endDate = end;
        }

        private void FormCetak_Load(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void LoadReport()
        {
            try
            {
                ReportDocument rpt = null;
                bool isStronglyTyped = false;

                try
                {
                    // Attempt to load CrystalReport1 class
                    rpt = new CrystalReport1();
                    isStronglyTyped = true;
                }
                catch
                {
                    // Fallback to dynamic loading
                    rpt = new ReportDocument();
                }

                if (rpt != null)
                {
                    if (!isStronglyTyped || !rpt.IsLoaded)
                    {
                        // Find loose .rpt file if strongly-typed load didn't work
                        string rptPath = FindReportFile();
                        if (!string.IsNullOrEmpty(rptPath) && File.Exists(rptPath))
                        {
                            rpt.Load(rptPath);
                        }
                        else
                        {
                            MessageBox.Show(
                                "File design report (*.rpt) tidak ditemukan.\r\n" +
                                "Silakan buat file Crystal Report (.rpt) di Visual Studio terlebih dahulu.",
                                "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    if (rpt.Database.Tables.Count > 0)
                    {
                        rpt.SetDataSource(dtReport);
                    }
                    else
                    {
                        MessageBox.Show(
                            "Crystal Report Viewer berhasil dimuat, tetapi file desain report (*.rpt) Anda masih kosong (belum memiliki tabel/field).\r\n\r\n" +
                            "Langkah Selanjutnya:\r\n" +
                            "1. Buka file 'CrystalReport1.rpt' di Visual Studio.\r\n" +
                            "2. Gunakan 'Database Expert' untuk menambahkan tabel/kolom yang sesuai dengan data rekap.\r\n" +
                            "3. Letakkan field data pada area Details di report designer.\r\n" +
                            "4. Jalankan kembali aplikasi untuk melihat hasil cetak.",
                            "Desain Laporan Belum Dibuat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Safely set parameters if they are defined in the Crystal Report design
                    try { rpt.SetParameterValue("NamaUser", namaUser); } catch { }
                    try { rpt.SetParameterValue("StartDate", startDate.ToString("dd/MM/yyyy")); } catch { }
                    try { rpt.SetParameterValue("EndDate", endDate.ToString("dd/MM/yyyy")); } catch { }

                    crystalReportViewer1.ReportSource = rpt;
                }

                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat laporan Crystal Reports:\n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FindReportFile()
        {
            string rptPath = "";
            string[] rptFiles = Directory.GetFiles(Application.StartupPath, "*.rpt");
            if (rptFiles.Length > 0)
            {
                rptPath = rptFiles[0];
            }
            else
            {
                string projDir = Path.Combine(Application.StartupPath, "..", "..");
                if (Directory.Exists(projDir))
                {
                    rptFiles = Directory.GetFiles(projDir, "*.rpt", SearchOption.TopDirectoryOnly);
                    if (rptFiles.Length > 0)
                    {
                        rptPath = rptFiles[0];
                    }
                }
            }
            return rptPath;
        }
    }
}
