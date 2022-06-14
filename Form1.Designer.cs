
namespace RothsFarmIt
{
    partial class Form1
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
            this.startBtn = new System.Windows.Forms.Button();
            this.stopBtn = new System.Windows.Forms.Button();
            this.frstLoginBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.userNameBox = new System.Windows.Forms.TextBox();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.dbgAttach = new System.Windows.Forms.Button();
            this.mapBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(12, 140);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(75, 23);
            this.startBtn.TabIndex = 0;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // stopBtn
            // 
            this.stopBtn.Location = new System.Drawing.Point(95, 140);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(75, 23);
            this.stopBtn.TabIndex = 1;
            this.stopBtn.Text = "Stop";
            this.stopBtn.UseVisualStyleBackColor = true;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // frstLoginBtn
            // 
            this.frstLoginBtn.Location = new System.Drawing.Point(12, 169);
            this.frstLoginBtn.Name = "frstLoginBtn";
            this.frstLoginBtn.Size = new System.Drawing.Size(75, 23);
            this.frstLoginBtn.TabIndex = 2;
            this.frstLoginBtn.Text = "First Login";
            this.frstLoginBtn.UseVisualStyleBackColor = true;
            this.frstLoginBtn.Click += new System.EventHandler(this.frstLoginBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Path :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Username :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Password :";
            // 
            // pathBox
            // 
            this.pathBox.Location = new System.Drawing.Point(62, 15);
            this.pathBox.Name = "pathBox";
            this.pathBox.Size = new System.Drawing.Size(231, 23);
            this.pathBox.TabIndex = 6;
            // 
            // userNameBox
            // 
            this.userNameBox.Location = new System.Drawing.Point(90, 49);
            this.userNameBox.Name = "userNameBox";
            this.userNameBox.Size = new System.Drawing.Size(157, 23);
            this.userNameBox.TabIndex = 7;
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(90, 86);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.PasswordChar = '*';
            this.passwordBox.Size = new System.Drawing.Size(157, 23);
            this.passwordBox.TabIndex = 8;
            // 
            // dbgAttach
            // 
            this.dbgAttach.Location = new System.Drawing.Point(193, 140);
            this.dbgAttach.Name = "dbgAttach";
            this.dbgAttach.Size = new System.Drawing.Size(100, 23);
            this.dbgAttach.TabIndex = 9;
            this.dbgAttach.Text = "Debug Attach";
            this.dbgAttach.UseVisualStyleBackColor = true;
            this.dbgAttach.Click += new System.EventHandler(this.dbgAttach_Click);
            // 
            // mapBtn
            // 
            this.mapBtn.Location = new System.Drawing.Point(193, 169);
            this.mapBtn.Name = "mapBtn";
            this.mapBtn.Size = new System.Drawing.Size(100, 23);
            this.mapBtn.TabIndex = 10;
            this.mapBtn.Text = "Get Map";
            this.mapBtn.UseVisualStyleBackColor = true;
            this.mapBtn.Click += new System.EventHandler(this.mapBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 228);
            this.Controls.Add(this.mapBtn);
            this.Controls.Add(this.dbgAttach);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.userNameBox);
            this.Controls.Add(this.pathBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.frstLoginBtn);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.startBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.Button frstLoginBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox pathBox;
        private System.Windows.Forms.TextBox userNameBox;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Button dbgAttach;
        private System.Windows.Forms.Button mapBtn;
    }
}

