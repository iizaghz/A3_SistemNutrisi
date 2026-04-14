namespace SistemNutrisi
{
    partial class FormUser
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
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnKonsumsi = new System.Windows.Forms.Button();
            this.btnRiwayat = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.pnlSidebar.Controls.Add(this.lblWelcome);
            this.pnlSidebar.Controls.Add(this.btnLoad);
            this.pnlSidebar.Controls.Add(this.btnKonsumsi);
            this.pnlSidebar.Controls.Add(this.btnRiwayat);
            this.pnlSidebar.Controls.Add(this.btnLogout);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(267, 615);
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
            this.lblWelcome.Text = "Selamat datang, User";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnLoad
            // 
            this.btnLoad.FlatAppearance.BorderSize = 0;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnLoad.ForeColor = System.Drawing.Color.White;
            this.btnLoad.Location = new System.Drawing.Point(0, 98);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(267, 62);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Cek Nutrisi";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnKonsumsi
            // 
            this.btnKonsumsi.FlatAppearance.BorderSize = 0;
            this.btnKonsumsi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKonsumsi.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnKonsumsi.ForeColor = System.Drawing.Color.White;
            this.btnKonsumsi.Location = new System.Drawing.Point(0, 160);
            this.btnKonsumsi.Margin = new System.Windows.Forms.Padding(4);
            this.btnKonsumsi.Name = "btnKonsumsi";
            this.btnKonsumsi.Size = new System.Drawing.Size(267, 62);
            this.btnKonsumsi.TabIndex = 2;
            this.btnKonsumsi.Text = "Catat Konsumsi";
            this.btnKonsumsi.UseVisualStyleBackColor = true;
            this.btnKonsumsi.Click += new System.EventHandler(this.btnKonsumsi_Click);
            // 
            // btnRiwayat
            // 
            this.btnRiwayat.FlatAppearance.BorderSize = 0;
            this.btnRiwayat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRiwayat.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnRiwayat.ForeColor = System.Drawing.Color.White;
            this.btnRiwayat.Location = new System.Drawing.Point(0, 222);
            this.btnRiwayat.Margin = new System.Windows.Forms.Padding(4);
            this.btnRiwayat.Name = "btnRiwayat";
            this.btnRiwayat.Size = new System.Drawing.Size(267, 62);
            this.btnRiwayat.TabIndex = 3;
            this.btnRiwayat.Text = "Riwayat Anda";
            this.btnRiwayat.UseVisualStyleBackColor = true;
            this.btnRiwayat.Click += new System.EventHandler(this.btnRiwayat_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnLogout.Location = new System.Drawing.Point(0, 553);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(4);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(267, 62);
            this.btnLogout.TabIndex = 4;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(267, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(4);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(933, 74);
            this.pnlHeader.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(267, 74);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(933, 541);
            this.dataGridView1.TabIndex = 2;
            // 
            // FormUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 615);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard User";
            this.Load += new System.EventHandler(this.FormUser_Load);
            this.pnlSidebar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnRiwayat;
        private System.Windows.Forms.Button btnKonsumsi;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}
