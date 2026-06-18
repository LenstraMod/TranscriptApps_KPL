namespace Frontend
{
    partial class Transkrip_UI
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
            button1 = new Button();
            buttonStop = new Button();
            buttonBack = new Button();
            groupBox1 = new GroupBox();
            txtDraft = new RichTextBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            txtFinal = new RichTextBox();
            label2 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ButtonFace;
            button1.Location = new Point(343, 501);
            button1.Name = "button1";
            button1.Size = new Size(180, 60);
            button1.TabIndex = 0;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // buttonStop
            // 
            buttonStop.BackColor = SystemColors.ButtonFace;
            buttonStop.Location = new Point(607, 501);
            buttonStop.Name = "buttonStop";
            buttonStop.Size = new Size(180, 60);
            buttonStop.TabIndex = 3;
            buttonStop.Text = "Stop";
            buttonStop.UseVisualStyleBackColor = false;
            buttonStop.Click += buttonStop_Click;
            // 
            // buttonBack
            // 
            buttonBack.BackColor = SystemColors.ButtonFace;
            buttonBack.Location = new Point(12, 12);
            buttonBack.Name = "buttonBack";
            buttonBack.Size = new Size(100, 35);
            buttonBack.TabIndex = 4;
            buttonBack.Text = "Back";
            buttonBack.UseVisualStyleBackColor = false;
            buttonBack.Click += buttonBack_Click;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = SystemColors.ButtonFace;
            groupBox1.Controls.Add(txtDraft);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 60);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(511, 412);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            // 
            // txtDraft
            // 
            txtDraft.BorderStyle = BorderStyle.None;
            txtDraft.Location = new Point(6, 42);
            txtDraft.Name = "txtDraft";
            txtDraft.ReadOnly = true;
            txtDraft.Size = new Size(499, 350);
            txtDraft.TabIndex = 1;
            txtDraft.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(228, 14);
            label1.Name = "label1";
            label1.Size = new Size(52, 25);
            label1.TabIndex = 0;
            label1.Text = "Draft";
            // 
            // groupBox2
            // 
            groupBox2.BackColor = SystemColors.ButtonFace;
            groupBox2.Controls.Add(txtFinal);
            groupBox2.Controls.Add(label2);
            groupBox2.Location = new Point(607, 60);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(541, 412);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            // 
            // txtFinal
            // 
            txtFinal.BorderStyle = BorderStyle.None;
            txtFinal.Location = new Point(9, 42);
            txtFinal.Name = "txtFinal";
            txtFinal.ReadOnly = true;
            txtFinal.Size = new Size(526, 350);
            txtFinal.TabIndex = 1;
            txtFinal.Text = "";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(251, 14);
            label2.Name = "label2";
            label2.Size = new Size(48, 25);
            label2.TabIndex = 0;
            label2.Text = "Final";
            // 
            // Transkrip_UI
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(1160, 600);
            Controls.Add(buttonBack);
            Controls.Add(buttonStop);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(button1);
            Name = "Transkrip_UI";
            Text = "Transkrip_UI";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button buttonStop;
        private Button buttonBack;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private RichTextBox txtDraft;
        private RichTextBox txtFinal;
    }
}