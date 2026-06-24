namespace SistemNutrisi
{
    partial class FormCetak
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
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExportCSV = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.pnlPreview = new System.Windows.Forms.Panel();
            this.printPreviewControl1 = new System.Windows.Forms.PrintPreviewControl();
            this.pnlToolbar.SuspendLayout();
            this.pnlPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.pnlToolbar.Controls.Add(this.btnClose);
            this.pnlToolbar.Controls.Add(this.btnExportCSV);
            this.pnlToolbar.Controls.Add(this.btnPrint);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Location = new System.Drawing.Point(0, 0);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Size = new System.Drawing.Size(850, 50);
            this.pnlToolbar.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(740, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Tutup";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExportCSV
            // 
            this.btnExportCSV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(196)))), ((int)(((byte)(15)))));
            this.btnExportCSV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportCSV.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnExportCSV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnExportCSV.Location = new System.Drawing.Point(140, 10);
            this.btnExportCSV.Name = "btnExportCSV";
            this.btnExportCSV.Size = new System.Drawing.Size(130, 30);
            this.btnExportCSV.TabIndex = 1;
            this.btnExportCSV.Text = "Export ke CSV";
            this.btnExportCSV.UseVisualStyleBackColor = false;
            this.btnExportCSV.Click += new System.EventHandler(this.btnExportCSV_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(15, 10);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(110, 30);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "Cetak / PDF";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // pnlPreview
            // 
            this.pnlPreview.Controls.Add(this.printPreviewControl1);
            this.pnlPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPreview.Location = new System.Drawing.Point(0, 50);
            this.pnlPreview.Name = "pnlPreview";
            this.pnlPreview.Size = new System.Drawing.Size(850, 650);
            this.pnlPreview.TabIndex = 1;
            // 
            // printPreviewControl1
            // 
            this.printPreviewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printPreviewControl1.Location = new System.Drawing.Point(0, 0);
            this.printPreviewControl1.Name = "printPreviewControl1";
            this.printPreviewControl1.Size = new System.Drawing.Size(850, 650);
            this.printPreviewControl1.TabIndex = 0;
            this.printPreviewControl1.Zoom = 1D;
            // 
            // FormCetak
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 700);
            this.Controls.Add(this.pnlPreview);
            this.Controls.Add(this.pnlToolbar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "FormCetak";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preview Laporan Konsumsi";
            this.Load += new System.EventHandler(this.FormCetak_Load);
            this.pnlToolbar.ResumeLayout(false);
            this.pnlPreview.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExportCSV;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel pnlPreview;
        private System.Windows.Forms.PrintPreviewControl printPreviewControl1;
    }
}
