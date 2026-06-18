namespace Frontend
{
    partial class loginpage
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            btnMasuk = new Button();
            label4 = new Label();
            btnDftr = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(206, 104);
            label1.Name = "label1";
            label1.Size = new Size(129, 15);
            label1.TabIndex = 0;
            label1.Text = "Login Aplikasi Psikolog";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(129, 150);
            label2.Name = "label2";
            label2.Size = new Size(117, 15);
            label2.TabIndex = 0;
            label2.Text = "Masukkan Username";
            label2.Click += label1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(129, 197);
            label3.Name = "label3";
            label3.Size = new Size(114, 15);
            label3.TabIndex = 0;
            label3.Text = "Masukkan Password";
            label3.Click += label1_Click;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(278, 147);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(186, 23);
            txtUsername.TabIndex = 1;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(278, 197);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(186, 23);
            txtPassword.TabIndex = 2;
            txtPassword.TextChanged += textBox2_TextChanged;
            // 
            // btnMasuk
            // 
            btnMasuk.Location = new Point(229, 246);
            btnMasuk.Name = "btnMasuk";
            btnMasuk.Size = new Size(75, 23);
            btnMasuk.TabIndex = 3;
            btnMasuk.Text = "Masuk";
            btnMasuk.UseVisualStyleBackColor = true;
            btnMasuk.Click += btnMasuk_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(215, 299);
            label4.Name = "label4";
            label4.Size = new Size(111, 15);
            label4.TabIndex = 4;
            label4.Text = "Belum punya akun?";
            // 
            // btnDftr
            // 
            btnDftr.Location = new Point(229, 327);
            btnDftr.Name = "btnDftr";
            btnDftr.Size = new Size(75, 23);
            btnDftr.TabIndex = 5;
            btnDftr.Text = "Daftar";
            btnDftr.UseVisualStyleBackColor = true;
            btnDftr.Click += btnDftr_Click;
            // 
            // loginpage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(581, 446);
            Controls.Add(btnDftr);
            Controls.Add(label4);
            Controls.Add(btnMasuk);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "loginpage";
            Text = "Login - Aplikasi Psikolog";
            Load += loginpage_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnMasuk;
        private Label label4;
        private Button btnDftr;
    }
}
