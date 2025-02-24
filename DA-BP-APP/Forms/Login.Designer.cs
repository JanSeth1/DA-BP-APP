namespace DA_BP_APP
{
    partial class Login
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
            this.txt_username = new MaterialSkin.Controls.MaterialTextBox();
            this.txt_password = new MaterialSkin.Controls.MaterialTextBox2();
            this.chkbox_show = new MaterialSkin.Controls.MaterialCheckbox();
            this.materialButton1 = new MaterialSkin.Controls.MaterialButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_username
            // 
            this.txt_username.AnimateReadOnly = false;
            this.txt_username.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_username.Depth = 0;
            this.txt_username.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txt_username.Hint = "Username";
            this.txt_username.LeadingIcon = null;
            this.txt_username.Location = new System.Drawing.Point(100, 258);
            this.txt_username.MaxLength = 50;
            this.txt_username.MouseState = MaterialSkin.MouseState.OUT;
            this.txt_username.Multiline = false;
            this.txt_username.Name = "txt_username";
            this.txt_username.Size = new System.Drawing.Size(226, 50);
            this.txt_username.TabIndex = 1;
            this.txt_username.Text = "";
            this.txt_username.TrailingIcon = null;
            // 
            // txt_password
            // 
            this.txt_password.AnimateReadOnly = false;
            this.txt_password.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.txt_password.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txt_password.Depth = 0;
            this.txt_password.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txt_password.HideSelection = true;
            this.txt_password.Hint = "Password";
            this.txt_password.LeadingIcon = null;
            this.txt_password.Location = new System.Drawing.Point(100, 334);
            this.txt_password.MaxLength = 50;
            this.txt_password.MouseState = MaterialSkin.MouseState.OUT;
            this.txt_password.Name = "txt_password";
            this.txt_password.PasswordChar = '●';
            this.txt_password.PrefixSuffixText = null;
            this.txt_password.ReadOnly = false;
            this.txt_password.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txt_password.SelectedText = "";
            this.txt_password.SelectionLength = 0;
            this.txt_password.SelectionStart = 0;
            this.txt_password.ShortcutsEnabled = true;
            this.txt_password.Size = new System.Drawing.Size(226, 48);
            this.txt_password.TabIndex = 2;
            this.txt_password.TabStop = false;
            this.txt_password.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txt_password.TrailingIcon = null;
            this.txt_password.UseSystemPasswordChar = true;
            // 
            // chkbox_show
            // 
            this.chkbox_show.AutoSize = true;
            this.chkbox_show.Depth = 0;
            this.chkbox_show.Location = new System.Drawing.Point(100, 409);
            this.chkbox_show.Margin = new System.Windows.Forms.Padding(0);
            this.chkbox_show.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkbox_show.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkbox_show.Name = "chkbox_show";
            this.chkbox_show.ReadOnly = false;
            this.chkbox_show.Ripple = true;
            this.chkbox_show.Size = new System.Drawing.Size(148, 37);
            this.chkbox_show.TabIndex = 4;
            this.chkbox_show.Text = "Show password";
            this.chkbox_show.UseVisualStyleBackColor = true;
            this.chkbox_show.CheckedChanged += new System.EventHandler(this.chkbox_show_CheckedChanged);
            // 
            // materialButton1
            // 
            this.materialButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.materialButton1.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton1.Depth = 0;
            this.materialButton1.HighEmphasis = true;
            this.materialButton1.Icon = null;
            this.materialButton1.Location = new System.Drawing.Point(100, 457);
            this.materialButton1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton1.Name = "materialButton1";
            this.materialButton1.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton1.Size = new System.Drawing.Size(64, 36);
            this.materialButton1.TabIndex = 5;
            this.materialButton1.Text = "Login";
            this.materialButton1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton1.UseAccentColor = false;
            this.materialButton1.UseVisualStyleBackColor = true;
            this.materialButton1.Click += new System.EventHandler(this.materialButton1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(146, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(281, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "OFFICE OF THE MUNICIPAL ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(180, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 25);
            this.label2.TabIndex = 7;
            this.label2.Text = "AGRICULTURIST";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DA_BP_APP.Properties.Resources.OMA_Logo__Option_4_;
            this.pictureBox1.Location = new System.Drawing.Point(41, 110);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(83, 82);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 522);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.materialButton1);
            this.Controls.Add(this.chkbox_show);
            this.Controls.Add(this.txt_password);
            this.Controls.Add(this.txt_username);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialTextBox txt_username;
        private MaterialSkin.Controls.MaterialTextBox2 txt_password;
        private MaterialSkin.Controls.MaterialCheckbox chkbox_show;
        private MaterialSkin.Controls.MaterialButton materialButton1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}