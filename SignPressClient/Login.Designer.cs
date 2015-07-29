namespace SignPressClient
{
    partial class Login
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Button Submit;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.AllForm = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PassWord = new System.Windows.Forms.TextBox();
            this.UserName = new System.Windows.Forms.TextBox();
            this.errormessage = new System.Windows.Forms.ToolTip(this.components);
            this.cuemessage = new System.Windows.Forms.ToolTip(this.components);
            this.HeaderForm = new System.Windows.Forms.Panel();
            this.Close = new System.Windows.Forms.PictureBox();
            this.Settings = new System.Windows.Forms.PictureBox();
            this.min = new System.Windows.Forms.PictureBox();
            Submit = new System.Windows.Forms.Button();
            this.AllForm.SuspendLayout();
            this.HeaderForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Close)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Settings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.min)).BeginInit();
            this.SuspendLayout();
            // 
            // Submit
            // 
            Submit.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            Submit.Location = new System.Drawing.Point(124, 284);
            Submit.Name = "Submit";
            Submit.Size = new System.Drawing.Size(179, 39);
            Submit.TabIndex = 3;
            Submit.Text = "登      录";
            Submit.UseVisualStyleBackColor = true;
            Submit.Click += new System.EventHandler(this.Submit_Click);
            // 
            // AllForm
            // 
            this.AllForm.BackColor = System.Drawing.SystemColors.Window;
            this.AllForm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AllForm.Controls.Add(this.label3);
            this.AllForm.Controls.Add(this.label2);
            this.AllForm.Controls.Add(Submit);
            this.AllForm.Controls.Add(this.PassWord);
            this.AllForm.Controls.Add(this.UserName);
            this.AllForm.Controls.Add(this.HeaderForm);
            this.AllForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AllForm.Location = new System.Drawing.Point(0, 0);
            this.AllForm.Name = "AllForm";
            this.AllForm.Size = new System.Drawing.Size(430, 336);
            this.AllForm.TabIndex = 0;
            this.AllForm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Login_MouseDown);
            this.AllForm.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Login_MouseMove);
            this.AllForm.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Login_MouseUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Window;
            this.label3.ForeColor = System.Drawing.Color.Silver;
            this.label3.Location = new System.Drawing.Point(132, 249);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "密码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.ForeColor = System.Drawing.Color.Silver;
            this.label2.Location = new System.Drawing.Point(132, 210);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "用户名";
            // 
            // PassWord
            // 
            this.PassWord.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PassWord.Location = new System.Drawing.Point(124, 239);
            this.PassWord.Name = "PassWord";
            this.PassWord.Size = new System.Drawing.Size(179, 31);
            this.PassWord.TabIndex = 2;
            this.PassWord.UseSystemPasswordChar = true;
            this.PassWord.TextChanged += new System.EventHandler(this.PassWord_TextChanged);
            this.PassWord.Validated += new System.EventHandler(this.PassWord_Validated);
            // 
            // UserName
            // 
            this.UserName.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UserName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.UserName.Location = new System.Drawing.Point(124, 200);
            this.UserName.Name = "UserName";
            this.UserName.Size = new System.Drawing.Size(179, 31);
            this.UserName.TabIndex = 1;
            this.UserName.TextChanged += new System.EventHandler(this.UserName_TextChanged);
            this.UserName.Validated += new System.EventHandler(this.UserName_Validated);
            // 
            // HeaderForm
            // 
            this.HeaderForm.BackColor = System.Drawing.Color.Transparent;
            this.HeaderForm.BackgroundImage = global::SignPressClient.Properties.Resources.航道会签单2;
            this.HeaderForm.Controls.Add(this.Close);
            this.HeaderForm.Controls.Add(this.Settings);
            this.HeaderForm.Controls.Add(this.min);
            this.HeaderForm.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderForm.Location = new System.Drawing.Point(0, 0);
            this.HeaderForm.Margin = new System.Windows.Forms.Padding(0);
            this.HeaderForm.Name = "HeaderForm";
            this.HeaderForm.Size = new System.Drawing.Size(428, 167);
            this.HeaderForm.TabIndex = 0;
            this.HeaderForm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Login_MouseDown);
            this.HeaderForm.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Login_MouseMove);
            this.HeaderForm.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Login_MouseUp);
            // 
            // Close
            // 
            this.Close.BackColor = System.Drawing.Color.Transparent;
            this.Close.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Close.BackgroundImage")));
            this.Close.Location = new System.Drawing.Point(398, -1);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(32, 32);
            this.Close.TabIndex = 5;
            this.Close.TabStop = false;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            this.Close.MouseEnter += new System.EventHandler(this.Close_MouseEnter);
            this.Close.MouseLeave += new System.EventHandler(this.Close_MouseLeave);
            this.Close.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Close_MouseMove);
            // 
            // Settings
            // 
            this.Settings.BackColor = System.Drawing.Color.Transparent;
            this.Settings.BackgroundImage = global::SignPressClient.Properties.Resources.setting;
            this.Settings.Location = new System.Drawing.Point(338, -1);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(32, 32);
            this.Settings.TabIndex = 1;
            this.Settings.TabStop = false;
            this.Settings.Click += new System.EventHandler(this.Settings_Click);
            this.Settings.MouseEnter += new System.EventHandler(this.Settings_MouseEnter);
            this.Settings.MouseLeave += new System.EventHandler(this.Settings_MouseLeave);
            this.Settings.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Settings_MouseMove);
            // 
            // min
            // 
            this.min.BackColor = System.Drawing.Color.Transparent;
            this.min.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("min.BackgroundImage")));
            this.min.Location = new System.Drawing.Point(368, 0);
            this.min.Name = "min";
            this.min.Size = new System.Drawing.Size(32, 31);
            this.min.TabIndex = 6;
            this.min.TabStop = false;
            this.min.Click += new System.EventHandler(this.min_Click);
            this.min.MouseEnter += new System.EventHandler(this.min_MouseEnter);
            this.min.MouseLeave += new System.EventHandler(this.min_MouseLeave);
            this.min.MouseMove += new System.Windows.Forms.MouseEventHandler(this.min_MouseMove);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(430, 336);
            this.Controls.Add(this.AllForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "签字系统";
            this.Load += new System.EventHandler(this.Login_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Login_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Login_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Login_MouseUp);
            this.AllForm.ResumeLayout(false);
            this.AllForm.PerformLayout();
            this.HeaderForm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Close)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Settings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.min)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel AllForm;
        private System.Windows.Forms.Panel HeaderForm;
        private System.Windows.Forms.TextBox PassWord;
        private System.Windows.Forms.TextBox UserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox Close;
        private System.Windows.Forms.PictureBox min;
        private System.Windows.Forms.ToolTip errormessage;
        private System.Windows.Forms.PictureBox Settings;
        private System.Windows.Forms.ToolTip cuemessage;
    }
}

