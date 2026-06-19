namespace Frontend
{
    partial class UserControl1
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            panel2 = new Panel();
            tableLayoutButtons = new TableLayoutPanel();
            Details = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            //
            // panel1
            //
            panel1.BackColor = SystemColors.ActiveBorder;
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(766, 186);
            panel1.TabIndex = 1;
            //
            // panel2
            //
            panel2.BackColor = Color.FromArgb(0, 192, 192);
            panel2.Controls.Add(tableLayoutButtons);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(186, 0);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(10);
            panel2.Size = new Size(580, 186);
            panel2.TabIndex = 2;
            //
            // tableLayoutButtons
            //
            tableLayoutButtons.ColumnCount = 3;
            tableLayoutButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tableLayoutButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tableLayoutButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            tableLayoutButtons.Controls.Add(Details, 0, 0);
            tableLayoutButtons.Controls.Add(btnEdit, 1, 0);
            tableLayoutButtons.Controls.Add(btnDelete, 2, 0);
            tableLayoutButtons.Dock = DockStyle.Bottom;
            tableLayoutButtons.Location = new Point(10, 133);
            tableLayoutButtons.Name = "tableLayoutButtons";
            tableLayoutButtons.RowCount = 1;
            tableLayoutButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutButtons.Size = new Size(560, 43);
            tableLayoutButtons.TabIndex = 2;
            //
            // Details
            //
            Details.Dock = DockStyle.Fill;
            Details.Name = "Details";
            Details.TabIndex = 0;
            Details.Text = "Details";
            Details.UseVisualStyleBackColor = true;
            Details.Click += this.Details_Click;
            //
            // btnEdit
            //
            btnEdit.Dock = DockStyle.Fill;
            btnEdit.Name = "btnEdit";
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += this.btnEdit_Click;
            //
            // btnDelete
            //
            btnDelete.Dock = DockStyle.Fill;
            btnDelete.Name = "btnDelete";
            btnDelete.TabIndex = 2;
            btnDelete.Text = "Hapus";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += this.btnDelete_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(23, 110);
            label6.Name = "label6";
            label6.Size = new Size(50, 20);
            label6.TabIndex = 5;
            label6.Text = "online";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(23, 90);
            label5.Name = "label5";
            label5.Size = new Size(44, 20);
            label5.TabIndex = 4;
            label5.Text = "12:00";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(23, 70);
            label4.Name = "label4";
            label4.Size = new Size(50, 20);
            label4.TabIndex = 3;
            label4.Text = "09:00\"";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(23, 50);
            label3.Name = "label3";
            label3.Size = new Size(63, 20);
            label3.TabIndex = 2;
            label3.Text = "Monday";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 30);
            label2.Name = "label2";
            label2.Size = new Size(85, 20);
            label2.TabIndex = 1;
            label2.Text = "2026-06-15";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 10);
            label1.Name = "label1";
            label1.Size = new Size(163, 20);
            label1.TabIndex = 0;
            label1.Text = "Dr. Budi Santoso, M.Psi.";
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Left;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(186, 186);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.Click += this.pictureBox1_Click;
            // 
            // UserControl1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Name = "UserControl1";
            Size = new Size(776, 198);
            tableLayoutButtons.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private TableLayoutPanel tableLayoutButtons;
        private Button Details;
        private Button btnEdit;
        private Button btnDelete;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private PictureBox pictureBox1;
    }
}
