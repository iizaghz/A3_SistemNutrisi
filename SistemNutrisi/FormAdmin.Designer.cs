namespace SistemNutrisi
{
    partial class FormAdmin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.btnKategori = new System.Windows.Forms.Button();
            this.btnMakanan = new System.Windows.Forms.Button();
            this.btnNutrisi = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlSidebar.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.pnlSidebar.Controls.Add(this.lblWelcome);
            this.pnlSidebar.Controls.Add(this.btnKategori);
            this.pnlSidebar.Controls.Add(this.btnMakanan);
            this.pnlSidebar.Controls.Add(this.btnNutrisi);
            this.pnlSidebar.Controls.Add(this.btnLogout);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(258, 419);
            this.pnlSidebar.TabIndex = 0;
            // 
            // lblWelcome
            // 
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(16, 15);
            this.lblWelcome.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(235, 62);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Selamat datang, Admin";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnKategori
            // 
            this.btnKategori.FlatAppearance.BorderSize = 0;
            this.btnKategori.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKategori.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnKategori.ForeColor = System.Drawing.Color.White;
            this.btnKategori.Location = new System.Drawing.Point(0, 98);
            this.btnKategori.Margin = new System.Windows.Forms.Padding(4);
            this.btnKategori.Name = "btnKategori";
            this.btnKategori.Size = new System.Drawing.Size(267, 62);
            this.btnKategori.TabIndex = 1;
            this.btnKategori.Text = "Kelola Kategori";
            this.btnKategori.UseVisualStyleBackColor = true;
            this.btnKategori.Click += new System.EventHandler(this.btnKategori_Click);
            // 
            // btnMakanan
            // 
            this.btnMakanan.FlatAppearance.BorderSize = 0;
            this.btnMakanan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMakanan.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnMakanan.ForeColor = System.Drawing.Color.White;
            this.btnMakanan.Location = new System.Drawing.Point(0, 160);
            this.btnMakanan.Margin = new System.Windows.Forms.Padding(4);
            this.btnMakanan.Name = "btnMakanan";
            this.btnMakanan.Size = new System.Drawing.Size(267, 62);
            this.btnMakanan.TabIndex = 2;
            this.btnMakanan.Text = "Kelola Makanan";
            this.btnMakanan.UseVisualStyleBackColor = true;
            this.btnMakanan.Click += new System.EventHandler(this.btnMakanan_Click);
            // 
            // btnNutrisi
            // 
            this.btnNutrisi.FlatAppearance.BorderSize = 0;
            this.btnNutrisi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNutrisi.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnNutrisi.ForeColor = System.Drawing.Color.White;
            this.btnNutrisi.Location = new System.Drawing.Point(0, 222);
            this.btnNutrisi.Margin = new System.Windows.Forms.Padding(4);
            this.btnNutrisi.Name = "btnNutrisi";
            this.btnNutrisi.Size = new System.Drawing.Size(267, 62);
            this.btnNutrisi.TabIndex = 3;
            this.btnNutrisi.Text = "Kelola Nutrisi";
            this.btnNutrisi.UseVisualStyleBackColor = true;
            this.btnNutrisi.Click += new System.EventHandler(this.btnNutrisi_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnLogout.Location = new System.Drawing.Point(0, 357);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(4);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(258, 62);
            this.btnLogout.TabIndex = 4;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(258, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(4);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(692, 69);
            this.pnlHeader.TabIndex = 1;
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.White;
            this.pnlContent.Controls.Add(this.label2);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(258, 69);
            this.pnlContent.Margin = new System.Windows.Forms.Padding(4);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(692, 350);
            this.pnlContent.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.label2.ForeColor = System.Drawing.Color.Silver;
            this.label2.Location = new System.Drawing.Point(129, 134);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(427, 32);
            this.label2.TabIndex = 0;
            this.label2.Text = "Pilih menu di samping untuk memulai.";
            // 
            // FormAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 419);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard Admin - Nutri Life";
            this.Load += new System.EventHandler(this.FormAdmin_Load);
            this.pnlSidebar.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnNutrisi;
        private System.Windows.Forms.Button btnMakanan;
        private System.Windows.Forms.Button btnKategori;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label label2;
    }
}
