namespace SistemNutrisi
{
    partial class FormDashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dtpTanggal = new System.Windows.Forms.DateTimePicker();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlChart = new System.Windows.Forms.Panel();
            this.lblChartTitle = new System.Windows.Forms.Label();
            this.pnlCards = new System.Windows.Forms.FlowLayoutPanel();
            this.cardKalori = new System.Windows.Forms.Panel();
            this.lblKaloriVal = new System.Windows.Forms.Label();
            this.lblKaloriTitle = new System.Windows.Forms.Label();
            this.cardProtein = new System.Windows.Forms.Panel();
            this.lblProteinVal = new System.Windows.Forms.Label();
            this.lblProteinTitle = new System.Windows.Forms.Label();
            this.cardLemak = new System.Windows.Forms.Panel();
            this.lblLemakVal = new System.Windows.Forms.Label();
            this.lblLemakTitle = new System.Windows.Forms.Label();
            this.cardKarbo = new System.Windows.Forms.Panel();
            this.lblKarboVal = new System.Windows.Forms.Label();
            this.lblKarboTitle = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlChart.SuspendLayout();
            this.pnlCards.SuspendLayout();
            this.cardKalori.SuspendLayout();
            this.cardProtein.SuspendLayout();
            this.cardLemak.SuspendLayout();
            this.cardKarbo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.dtpTanggal);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(933, 60);
            this.pnlTop.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(821, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dtpTanggal
            // 
            this.dtpTanggal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpTanggal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpTanggal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTanggal.Location = new System.Drawing.Point(655, 16);
            this.dtpTanggal.Name = "dtpTanggal";
            this.dtpTanggal.Size = new System.Drawing.Size(150, 30);
            this.dtpTanggal.TabIndex = 1;
            this.dtpTanggal.ValueChanged += new System.EventHandler(this.dtpTanggal_ValueChanged);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(306, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Dashboard Nutrisi Harian";
            // 
            // pnlMain
            // 
            this.pnlMain.ColumnCount = 2;
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.pnlMain.Controls.Add(this.pnlChart, 1, 0);
            this.pnlMain.Controls.Add(this.pnlCards, 0, 0);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 60);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.RowCount = 1;
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlMain.Size = new System.Drawing.Size(933, 481);
            this.pnlMain.TabIndex = 1;
            // 
            // pnlChart
            // 
            this.pnlChart.BackColor = System.Drawing.Color.White;
            this.pnlChart.Controls.Add(this.lblChartTitle);
            this.pnlChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChart.Location = new System.Drawing.Point(516, 15);
            this.pnlChart.Margin = new System.Windows.Forms.Padding(3, 15, 15, 15);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Size = new System.Drawing.Size(402, 451);
            this.pnlChart.TabIndex = 1;
            this.pnlChart.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlChart_Paint);
            // 
            // lblChartTitle
            // 
            this.lblChartTitle.AutoSize = true;
            this.lblChartTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblChartTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblChartTitle.Location = new System.Drawing.Point(15, 15);
            this.lblChartTitle.Name = "lblChartTitle";
            this.lblChartTitle.Size = new System.Drawing.Size(262, 28);
            this.lblChartTitle.TabIndex = 0;
            this.lblChartTitle.Text = "Komposisi Makronutrisi (g)";
            // 
            // pnlCards
            // 
            this.pnlCards.AutoScroll = true;
            this.pnlCards.Controls.Add(this.cardKalori);
            this.pnlCards.Controls.Add(this.cardProtein);
            this.pnlCards.Controls.Add(this.cardLemak);
            this.pnlCards.Controls.Add(this.cardKarbo);
            this.pnlCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCards.Location = new System.Drawing.Point(15, 15);
            this.pnlCards.Margin = new System.Windows.Forms.Padding(15, 15, 3, 15);
            this.pnlCards.Name = "pnlCards";
            this.pnlCards.Size = new System.Drawing.Size(495, 451);
            this.pnlCards.TabIndex = 0;
            // 
            // cardKalori
            // 
            this.cardKalori.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.cardKalori.Controls.Add(this.lblKaloriVal);
            this.cardKalori.Controls.Add(this.lblKaloriTitle);
            this.cardKalori.Location = new System.Drawing.Point(3, 3);
            this.cardKalori.Name = "cardKalori";
            this.cardKalori.Size = new System.Drawing.Size(470, 95);
            this.cardKalori.TabIndex = 0;
            this.cardKalori.Paint += new System.Windows.Forms.PaintEventHandler(this.Card_Paint);
            // 
            // lblKaloriVal
            // 
            this.lblKaloriVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKaloriVal.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblKaloriVal.ForeColor = System.Drawing.Color.White;
            this.lblKaloriVal.Location = new System.Drawing.Point(150, 45);
            this.lblKaloriVal.Name = "lblKaloriVal";
            this.lblKaloriVal.Size = new System.Drawing.Size(300, 37);
            this.lblKaloriVal.TabIndex = 1;
            this.lblKaloriVal.Text = "0 / 2000 kcal (0%)";
            this.lblKaloriVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblKaloriTitle
            // 
            this.lblKaloriTitle.AutoSize = true;
            this.lblKaloriTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblKaloriTitle.ForeColor = System.Drawing.Color.White;
            this.lblKaloriTitle.Location = new System.Drawing.Point(15, 15);
            this.lblKaloriTitle.Name = "lblKaloriTitle";
            this.lblKaloriTitle.Size = new System.Drawing.Size(68, 28);
            this.lblKaloriTitle.TabIndex = 0;
            this.lblKaloriTitle.Text = "Kalori";
            // 
            // cardProtein
            // 
            this.cardProtein.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.cardProtein.Controls.Add(this.lblProteinVal);
            this.cardProtein.Controls.Add(this.lblProteinTitle);
            this.cardProtein.Location = new System.Drawing.Point(3, 104);
            this.cardProtein.Name = "cardProtein";
            this.cardProtein.Size = new System.Drawing.Size(470, 95);
            this.cardProtein.TabIndex = 1;
            this.cardProtein.Paint += new System.Windows.Forms.PaintEventHandler(this.Card_Paint);
            // 
            // lblProteinVal
            // 
            this.lblProteinVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProteinVal.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblProteinVal.ForeColor = System.Drawing.Color.White;
            this.lblProteinVal.Location = new System.Drawing.Point(150, 45);
            this.lblProteinVal.Name = "lblProteinVal";
            this.lblProteinVal.Size = new System.Drawing.Size(300, 37);
            this.lblProteinVal.TabIndex = 1;
            this.lblProteinVal.Text = "0 / 60 g (0%)";
            this.lblProteinVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProteinTitle
            // 
            this.lblProteinTitle.AutoSize = true;
            this.lblProteinTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblProteinTitle.ForeColor = System.Drawing.Color.White;
            this.lblProteinTitle.Location = new System.Drawing.Point(15, 15);
            this.lblProteinTitle.Name = "lblProteinTitle";
            this.lblProteinTitle.Size = new System.Drawing.Size(81, 28);
            this.lblProteinTitle.TabIndex = 0;
            this.lblProteinTitle.Text = "Protein";
            // 
            // cardLemak
            // 
            this.cardLemak.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(196)))), ((int)(((byte)(15)))));
            this.cardLemak.Controls.Add(this.lblLemakVal);
            this.cardLemak.Controls.Add(this.lblLemakTitle);
            this.cardLemak.Location = new System.Drawing.Point(3, 205);
            this.cardLemak.Name = "cardLemak";
            this.cardLemak.Size = new System.Drawing.Size(470, 95);
            this.cardLemak.TabIndex = 2;
            this.cardLemak.Paint += new System.Windows.Forms.PaintEventHandler(this.Card_Paint);
            // 
            // lblLemakVal
            // 
            this.lblLemakVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLemakVal.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblLemakVal.ForeColor = System.Drawing.Color.White;
            this.lblLemakVal.Location = new System.Drawing.Point(150, 45);
            this.lblLemakVal.Name = "lblLemakVal";
            this.lblLemakVal.Size = new System.Drawing.Size(300, 37);
            this.lblLemakVal.TabIndex = 1;
            this.lblLemakVal.Text = "0 / 70 g (0%)";
            this.lblLemakVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLemakTitle
            // 
            this.lblLemakTitle.AutoSize = true;
            this.lblLemakTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblLemakTitle.ForeColor = System.Drawing.Color.White;
            this.lblLemakTitle.Location = new System.Drawing.Point(15, 15);
            this.lblLemakTitle.Name = "lblLemakTitle";
            this.lblLemakTitle.Size = new System.Drawing.Size(73, 28);
            this.lblLemakTitle.TabIndex = 0;
            this.lblLemakTitle.Text = "Lemak";
            // 
            // cardKarbo
            // 
            this.cardKarbo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.cardKarbo.Controls.Add(this.lblKarboVal);
            this.cardKarbo.Controls.Add(this.lblKarboTitle);
            this.cardKarbo.Location = new System.Drawing.Point(3, 306);
            this.cardKarbo.Name = "cardKarbo";
            this.cardKarbo.Size = new System.Drawing.Size(470, 95);
            this.cardKarbo.TabIndex = 3;
            this.cardKarbo.Paint += new System.Windows.Forms.PaintEventHandler(this.Card_Paint);
            // 
            // lblKarboVal
            // 
            this.lblKarboVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKarboVal.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblKarboVal.ForeColor = System.Drawing.Color.White;
            this.lblKarboVal.Location = new System.Drawing.Point(150, 45);
            this.lblKarboVal.Name = "lblKarboVal";
            this.lblKarboVal.Size = new System.Drawing.Size(300, 37);
            this.lblKarboVal.TabIndex = 1;
            this.lblKarboVal.Text = "0 / 300 g (0%)";
            this.lblKarboVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblKarboTitle
            // 
            this.lblKarboTitle.AutoSize = true;
            this.lblKarboTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblKarboTitle.ForeColor = System.Drawing.Color.White;
            this.lblKarboTitle.Location = new System.Drawing.Point(15, 15);
            this.lblKarboTitle.Name = "lblKarboTitle";
            this.lblKarboTitle.Size = new System.Drawing.Size(126, 28);
            this.lblKarboTitle.TabIndex = 0;
            this.lblKarboTitle.Text = "Karbohidrat";
            // 
            // FormDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(933, 541);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormDashboard";
            this.Text = "FormDashboard";
            this.Load += new System.EventHandler(this.FormDashboard_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlChart.ResumeLayout(false);
            this.pnlChart.PerformLayout();
            this.pnlCards.ResumeLayout(false);
            this.cardKalori.ResumeLayout(false);
            this.cardKalori.PerformLayout();
            this.cardProtein.ResumeLayout(false);
            this.cardProtein.PerformLayout();
            this.cardLemak.ResumeLayout(false);
            this.cardLemak.PerformLayout();
            this.cardKarbo.ResumeLayout(false);
            this.cardKarbo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DateTimePicker dtpTanggal;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TableLayoutPanel pnlMain;
        private System.Windows.Forms.FlowLayoutPanel pnlCards;
        private System.Windows.Forms.Panel cardKalori;
        private System.Windows.Forms.Label lblKaloriVal;
        private System.Windows.Forms.Label lblKaloriTitle;
        private System.Windows.Forms.Panel cardProtein;
        private System.Windows.Forms.Label lblProteinVal;
        private System.Windows.Forms.Label lblProteinTitle;
        private System.Windows.Forms.Panel cardLemak;
        private System.Windows.Forms.Label lblLemakVal;
        private System.Windows.Forms.Label lblLemakTitle;
        private System.Windows.Forms.Panel cardKarbo;
        private System.Windows.Forms.Label lblKarboVal;
        private System.Windows.Forms.Label lblKarboTitle;
        private System.Windows.Forms.Panel pnlChart;
        private System.Windows.Forms.Label lblChartTitle;
    }
}
