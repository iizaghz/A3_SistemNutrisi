using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SistemNutrisi
{
    public partial class FormCetak : Form
    {
        private DataTable dtReport;
        private string namaUser;
        private DateTime startDate;
        private DateTime endDate;

        private PrintDocument printDoc;

        public FormCetak(DataTable dt, string user, DateTime start, DateTime end)
        {
            InitializeComponent();
            this.dtReport = dt;
            this.namaUser = user;
            this.startDate = start;
            this.endDate = end;

            // Set up PrintDocument
            printDoc = new PrintDocument();
            printDoc.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);
            printDoc.PrintPage += new PrintPageEventHandler(PrintDoc_PrintPage);
        }

        private void FormCetak_Load(object sender, EventArgs e)
        {
            // Bind PrintDocument to PrintPreviewControl
            printPreviewControl1.Document = printDoc;
            printPreviewControl1.Zoom = 1.0;
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int marginL = e.MarginBounds.Left;
            int marginT = e.MarginBounds.Top;
            int width = e.MarginBounds.Width;
            int height = e.MarginBounds.Height;

            int currentY = marginT;

            // 1. Draw Institutional Header (UMY or NutriLife style)
            using (Font appNameFont = new Font("Segoe UI", 16, FontStyle.Bold))
            using (Font subtitleFont = new Font("Segoe UI", 10, FontStyle.Italic))
            using (Font metaFont = new Font("Segoe UI", 10, FontStyle.Regular))
            using (Font metaBold = new Font("Segoe UI", 10, FontStyle.Bold))
            {
                // Title
                g.DrawString("NUTRI LIFE", appNameFont, Brushes.DarkSlateGray, marginL, currentY);
                currentY += 28;

                // Subtitle
                g.DrawString("Sistem Informasi Pemantauan Gizi dan Nutrisi Harian", subtitleFont, Brushes.Gray, marginL, currentY);
                currentY += 25;

                // Line separator
                using (Pen headerPen = new Pen(Color.FromArgb(44, 62, 80), 2))
                {
                    g.DrawLine(headerPen, marginL, currentY, marginL + width, currentY);
                }
                currentY += 15;

                // Document Title
                using (Font docTitleFont = new Font("Segoe UI", 14, FontStyle.Bold))
                {
                    string docTitle = "LAPORAN RIWAYAT KONSUMSI NUTRISI";
                    SizeF docTitleSize = g.MeasureString(docTitle, docTitleFont);
                    g.DrawString(docTitle, docTitleFont, Brushes.Black, marginL + (width - docTitleSize.Width) / 2, currentY);
                    currentY += 35;
                }

                // Metadata Block
                g.DrawString("Nama Pengguna : ", metaFont, Brushes.Black, marginL, currentY);
                g.DrawString(namaUser, metaBold, Brushes.Black, marginL + 120, currentY);
                
                string tglString = string.Format("{0:dd/MM/yyyy} s/d {1:dd/MM/yyyy}", startDate, endDate);
                g.DrawString("Periode Laporan: ", metaFont, Brushes.Black, marginL + 400, currentY);
                g.DrawString(tglString, metaBold, Brushes.Black, marginL + 520, currentY);
                currentY += 20;

                g.DrawString("Dicetak Pada   : ", metaFont, Brushes.Black, marginL, currentY);
                g.DrawString(DateTime.Now.ToString("dd MMMM yyyy HH:mm"), metaBold, Brushes.Black, marginL + 120, currentY);
                currentY += 30;
            }

            // 2. Setup Columns for Table
            // Column widths: Total width = 700 (approx width of A4 portrait printable area)
            int colTanggalW = 90;
            int colMakananW = 210;
            int colJumlahW = 60;
            int colKaloriW = 85;
            int colProteinW = 85;
            int colLemakW = 85;

            int colTanggalX = marginL;
            int colMakananX = colTanggalX + colTanggalW;
            int colJumlahX = colMakananX + colMakananW;
            int colKaloriX = colJumlahX + colJumlahW;
            int colProteinX = colKaloriX + colKaloriW;
            int colLemakX = colProteinX + colProteinW;
            int colKarboX = colLemakX + colLemakW;

            // Draw Table Header Background
            using (SolidBrush headerBg = new SolidBrush(Color.FromArgb(46, 204, 113)))
            {
                g.FillRectangle(headerBg, marginL, currentY, width, 30);
            }

            // Draw Table Header Columns Label
            using (Font tableHeaderFont = new Font("Segoe UI", 9.5F, FontStyle.Bold))
            {
                g.DrawString("Tanggal", tableHeaderFont, Brushes.White, colTanggalX + 5, currentY + 7);
                g.DrawString("Nama Makanan", tableHeaderFont, Brushes.White, colMakananX + 5, currentY + 7);
                g.DrawString("Jumlah", tableHeaderFont, Brushes.White, colJumlahX + 5, currentY + 7);
                g.DrawString("Kalori", tableHeaderFont, Brushes.White, colKaloriX + 5, currentY + 7);
                g.DrawString("Protein", tableHeaderFont, Brushes.White, colProteinX + 5, currentY + 7);
                g.DrawString("Lemak", tableHeaderFont, Brushes.White, colLemakX + 5, currentY + 7);
                g.DrawString("Karbo", tableHeaderFont, Brushes.White, colKarboX + 5, currentY + 7);
            }
            currentY += 30;

            // 3. Draw Rows
            double totalKal = 0;
            double totalProt = 0;
            double totalLem = 0;
            double totalKarb = 0;
            int totalJml = 0;

            using (Font rowFont = new Font("Segoe UI", 9, FontStyle.Regular))
            using (Pen borderPen = new Pen(Color.LightGray, 1))
            {
                int rowIndex = 0;
                foreach (DataRow row in dtReport.Rows)
                {
                    // Alternating background colors
                    if (rowIndex % 2 == 1)
                    {
                        using (SolidBrush altBg = new SolidBrush(Color.FromArgb(248, 249, 249)))
                        {
                            g.FillRectangle(altBg, marginL, currentY, width, 25);
                        }
                    }

                    // Extract data
                    DateTime tanggal = Convert.ToDateTime(row["tanggal"]);
                    string makanan = row["nama_makanan"].ToString();
                    int jumlah = Convert.ToInt32(row["jumlah"]);
                    double kalori = Convert.ToDouble(row["total_kalori"]);
                    double protein = Convert.ToDouble(row["total_protein"]);
                    double lemak = Convert.ToDouble(row["total_lemak"]);
                    double karbo = Convert.ToDouble(row["total_karbohidrat"]);

                    // Sum values
                    totalKal += kalori;
                    totalProt += protein;
                    totalLem += lemak;
                    totalKarb += karbo;
                    totalJml += jumlah;

                    // Draw values
                    g.DrawString(tanggal.ToString("dd/MM/yyyy"), rowFont, Brushes.Black, colTanggalX + 5, currentY + 4);
                    g.DrawString(makanan, rowFont, Brushes.Black, colMakananX + 5, currentY + 4);
                    g.DrawString(jumlah.ToString(), rowFont, Brushes.Black, colJumlahX + 5, currentY + 4);
                    g.DrawString(kalori.ToString("0"), rowFont, Brushes.Black, colKaloriX + 5, currentY + 4);
                    g.DrawString(protein.ToString("0.0"), rowFont, Brushes.Black, colProteinX + 5, currentY + 4);
                    g.DrawString(lemak.ToString("0.0"), rowFont, Brushes.Black, colLemakX + 5, currentY + 4);
                    g.DrawString(karbo.ToString("0.0"), rowFont, Brushes.Black, colKarboX + 5, currentY + 4);

                    // Draw bottom border for row
                    g.DrawLine(borderPen, marginL, currentY + 25, marginL + width, currentY + 25);

                    currentY += 25;
                    rowIndex++;

                    // Page break handling (simplified since single-page is default, but keeps layout safe)
                    if (currentY > marginT + height - 120)
                    {
                        g.DrawString("(bersambung ke halaman berikutnya...)", rowFont, Brushes.Gray, marginL, currentY + 10);
                        e.HasMorePages = true;
                        return;
                    }
                }
            }

            // 4. Draw Totals Row
            using (Font totalFont = new Font("Segoe UI", 9.5F, FontStyle.Bold))
            using (Pen borderPen = new Pen(Color.FromArgb(44, 62, 80), 2))
            {
                // Top border for totals
                g.DrawLine(borderPen, marginL, currentY, marginL + width, currentY);

                g.DrawString("TOTAL", totalFont, Brushes.Black, colTanggalX + 5, currentY + 7);
                g.DrawString(totalJml.ToString(), totalFont, Brushes.Black, colJumlahX + 5, currentY + 7);
                g.DrawString(totalKal.ToString("0"), totalFont, Brushes.Black, colKaloriX + 5, currentY + 7);
                g.DrawString(totalProt.ToString("0.0"), totalFont, Brushes.Black, colProteinX + 5, currentY + 7);
                g.DrawString(totalLem.ToString("0.0"), totalFont, Brushes.Black, colLemakX + 5, currentY + 7);
                g.DrawString(totalKarb.ToString("0.0"), totalFont, Brushes.Black, colKarboX + 5, currentY + 7);

                // Bottom border for totals
                g.DrawLine(borderPen, marginL, currentY + 30, marginL + width, currentY + 30);
                currentY += 45;
            }



            // End of page
            e.HasMorePages = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            using (PrintDialog pd = new PrintDialog())
            {
                pd.Document = printDoc;
                pd.UseEXDialog = true;
                if (pd.ShowDialog() == DialogResult.OK)
                {
                    printDoc.Print();
                }
            }
        }

        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            if (dtReport == null || dtReport.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data untuk di-export.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                sfd.FileName = string.Format("Laporan_Nutrisi_{0}_{1:yyyyMMdd}.csv", namaUser.Replace(" ", "_"), DateTime.Today);
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();

                        // Header
                        string[] columnNames = { "Tanggal", "Nama Makanan", "Jumlah", "Kalori (kcal)", "Protein (g)", "Lemak (g)", "Karbohidrat (g)" };
                        sb.AppendLine(string.Join(",", columnNames));

                        // Data rows
                        foreach (DataRow row in dtReport.Rows)
                        {
                            DateTime tanggal = Convert.ToDateTime(row["tanggal"]);
                            string makanan = row["nama_makanan"].ToString().Replace(",", " "); // Avoid CSV breaking
                            int jumlah = Convert.ToInt32(row["jumlah"]);
                            double kalori = Convert.ToDouble(row["total_kalori"]);
                            double protein = Convert.ToDouble(row["total_protein"]);
                            double lemak = Convert.ToDouble(row["total_lemak"]);
                            double karbo = Convert.ToDouble(row["total_karbohidrat"]);

                            string line = string.Format("{0:yyyy-MM-dd},{1},{2},{3:0},{4:0.0},{5:0.0},{6:0.0}",
                                tanggal, makanan, jumlah, kalori, protein, lemak, karbo);
                            sb.AppendLine(line);
                        }

                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        MessageBox.Show("Berhasil meng-export data ke CSV!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal meng-export data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
