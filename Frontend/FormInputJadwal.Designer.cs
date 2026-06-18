namespace Frontend
{
    partial class FormInputJadwal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            label1 = new Label();
            txtNamaPsikolog = new TextBox();
            label2 = new Label();
            dtpTanggal = new DateTimePicker();
            label3 = new Label();
            txtHari = new TextBox();
            label4 = new Label();
            contextMenuStrip1 = new ContextMenuStrip(components);
            txtMulai = new TextBox();
            label5 = new Label();
            txtAkhir = new TextBox();
            label6 = new Label();
            cmbJenisSesi = new ComboBox();
            btnSave = new Button();
            btnPilihGambar = new Button();
            lblPathGambar = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 19);
            label1.Name = "label1";
            label1.Size = new Size(112, 20);
            label1.TabIndex = 0;
            label1.Text = "Nama Psikolog:";
            // 
            // txtNamaPsikolog
            // 
            txtNamaPsikolog.Location = new Point(130, 19);
            txtNamaPsikolog.Name = "txtNamaPsikolog";
            txtNamaPsikolog.Size = new Size(176, 27);
            txtNamaPsikolog.TabIndex = 1;
            txtNamaPsikolog.Text = "Tempat mengetik nama";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 52);
            label2.Name = "label2";
            label2.Size = new Size(94, 20);
            label2.TabIndex = 2;
            label2.Text = "Tanggal Sesi:";
            // 
            // dtpTanggal
            // 
            dtpTanggal.Format = DateTimePickerFormat.Custom;
            dtpTanggal.Location = new Point(130, 52);
            dtpTanggal.Name = "dtpTanggal";
            dtpTanggal.Size = new Size(250, 27);
            dtpTanggal.TabIndex = 3;
            dtpTanggal.ValueChanged += dtpTanggal_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 85);
            label3.Name = "label3";
            label3.Size = new Size(70, 20);
            label3.TabIndex = 4;
            label3.Text = "Hari Sesi:";
            // 
            // txtHari
            // 
            txtHari.Location = new Point(130, 85);
            txtHari.Name = "txtHari";
            txtHari.Size = new Size(176, 27);
            txtHari.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 118);
            label4.Name = "label4";
            label4.Size = new Size(79, 20);
            label4.TabIndex = 6;
            label4.Text = "Jam Mulai:";
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // txtMulai
            // 
            txtMulai.Location = new Point(130, 118);
            txtMulai.Name = "txtMulai";
            txtMulai.Size = new Size(176, 27);
            txtMulai.TabIndex = 8;
            txtMulai.Text = "Tempat input jam mulai";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 154);
            label5.Name = "label5";
            label5.Size = new Size(76, 20);
            label5.TabIndex = 9;
            label5.Text = "Jam Akhir:";
            // 
            // txtAkhir
            // 
            txtAkhir.Location = new Point(130, 154);
            txtAkhir.Name = "txtAkhir";
            txtAkhir.Size = new Size(176, 27);
            txtAkhir.TabIndex = 10;
            txtAkhir.Text = "Tempat input jam selesai";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 187);
            label6.Name = "label6";
            label6.Size = new Size(73, 20);
            label6.TabIndex = 11;
            label6.Text = "Jenis Sesi:";
            // 
            // cmbJenisSesi
            // 
            cmbJenisSesi.FormattingEnabled = true;
            cmbJenisSesi.Items.AddRange(new object[] { "Online", "Offline" });
            cmbJenisSesi.Location = new Point(130, 187);
            cmbJenisSesi.Name = "cmbJenisSesi";
            cmbJenisSesi.Size = new Size(151, 28);
            cmbJenisSesi.TabIndex = 12;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(510, 253);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(94, 29);
            btnSave.TabIndex = 13;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnPilihGambar
            // 
            btnPilihGambar.Location = new Point(130, 221);
            btnPilihGambar.Name = "btnPilihGambar";
            btnPilihGambar.Size = new Size(145, 29);
            btnPilihGambar.TabIndex = 14;
            btnPilihGambar.Text = "Pilih Foto...";
            btnPilihGambar.UseVisualStyleBackColor = true;
            // 
            // lblPathGambar
            // 
            lblPathGambar.AutoSize = true;
            lblPathGambar.Location = new Point(142, 262);
            lblPathGambar.Name = "lblPathGambar";
            lblPathGambar.Size = new Size(120, 20);
            lblPathGambar.TabIndex = 15;
            lblPathGambar.Text = "\"Belum ada file.\"";
            // 
            // FormInputJadwal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(641, 314);
            Controls.Add(lblPathGambar);
            Controls.Add(btnPilihGambar);
            Controls.Add(btnSave);
            Controls.Add(cmbJenisSesi);
            Controls.Add(label6);
            Controls.Add(txtAkhir);
            Controls.Add(label5);
            Controls.Add(txtMulai);
            Controls.Add(label4);
            Controls.Add(txtHari);
            Controls.Add(label3);
            Controls.Add(dtpTanggal);
            Controls.Add(label2);
            Controls.Add(txtNamaPsikolog);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormInputJadwal";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Tambah Sesi Jadwal Baru";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtNamaPsikolog;
        private Label label2;
        private DateTimePicker dtpTanggal;
        private Label label3;
        private TextBox txtHari;
        private Label label4;
        private ContextMenuStrip contextMenuStrip1;
        private TextBox txtMulai;
        private Label label5;
        private TextBox txtAkhir;
        private Label label6;
        private ComboBox cmbJenisSesi;
        private Button btnSave;
        private Button btnPilihGambar;
        private Label lblPathGambar;
    }
}