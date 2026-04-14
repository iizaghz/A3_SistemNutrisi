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
