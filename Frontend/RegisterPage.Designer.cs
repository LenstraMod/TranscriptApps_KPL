namespace Frontend
{
    partial class RegisterPage
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
            btnDaftar = new Button();
            txtUsernameReg = new TextBox();
            txtNama = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            label4 = new Label();
            txtPasswordReg = new TextBox();
            label5 = new Label();
            txtConfirmPassword = new TextBox();
            label6 = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // btnDaftar
            // 
            btnDaftar.Location = new Point(240, 317);
            btnDaftar.Name = "btnDaftar";
            btnDaftar.Size = new Size(75, 23);
            btnDaftar.TabIndex = 9;
            btnDaftar.Text = "Daftar";
            btnDaftar.UseVisualStyleBackColor = true;
            btnDaftar.Click += btnDaftar_Click;
            // 
            // txtUsernameReg
            // 
            txtUsernameReg.Location = new Point(278, 185);
            txtUsernameReg.Name = "txtUsernameReg";
            txtUsernameReg.Size = new Size(186, 23);
            txtUsernameReg.TabIndex = 8;
            // 
            // txtNama
            // 
            txtNama.Location = new Point(278, 147);
            txtNama.Name = "txtNama";
            txtNama.Size = new Size(186, 23);
            txtNama.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(129, 188);
            label3.Name = "label3";
            label3.Size = new Size(117, 15);
            label3.TabIndex = 4;
            label3.Text = "Masukkan Username";
            label3.Click += label3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(129, 150);
            label2.Name = "label2";
            label2.Size = new Size(87, 15);
            label2.TabIndex = 5;
            label2.Text = "Nama Lengkap";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(206, 104);
            label1.Name = "label1";
            label1.Size = new Size(141, 15);
            label1.TabIndex = 6;
            label1.Text = "Register Aplikasi Psikolog";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(129, 228);
            label4.Name = "label4";
            label4.Size = new Size(114, 15);
            label4.TabIndex = 10;
            label4.Text = "Masukkan Password";
            // 
            // txtPasswordReg
            // 
            txtPasswordReg.Location = new Point(278, 225);
            txtPasswordReg.Name = "txtPasswordReg";
            txtPasswordReg.PasswordChar = '*';
            txtPasswordReg.Size = new Size(186, 23);
            txtPasswordReg.TabIndex = 11;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(129, 270);
            label5.Name = "label5";
            label5.Size = new Size(117, 15);
            label5.TabIndex = 12;
            label5.Text = "Konfirmasi Password";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Location = new Point(278, 267);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PasswordChar = '*';
            txtConfirmPassword.Size = new Size(186, 23);
            txtConfirmPassword.TabIndex = 13;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(224, 355);
            label6.Name = "label6";
            label6.Size = new Size(110, 15);
            label6.TabIndex = 14;
            label6.Text = "Sudah punya akun?";
            // 
            // button1
            // 
            button1.Location = new Point(240, 383);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 15;
            button1.Text = "Masuk";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // RegisterPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(581, 446);
            Controls.Add(button1);
            Controls.Add(label6);
            Controls.Add(txtConfirmPassword);
            Controls.Add(label5);
            Controls.Add(txtPasswordReg);
            Controls.Add(label4);
            Controls.Add(btnDaftar);
            Controls.Add(txtUsernameReg);
            Controls.Add(txtNama);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "RegisterPage";
            Text = "Register Page - Aplikasi Psikolog";
            Load += RegisterPage_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnDaftar;
        private TextBox txtUsernameReg;
        private TextBox txtNama;
        private Label label3;
        private Label label2;
        private Label label1;
        private Label label4;
        private TextBox txtPasswordReg;
        private Label label5;
        private TextBox txtConfirmPassword;
        private Label label6;
        private Button button1;
    }
}