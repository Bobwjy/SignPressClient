namespace SignPressClient
{
    partial class Setting
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
            this.components = new System.ComponentModel.Container();
            this.AllPanel = new System.Windows.Forms.Panel();
            this.Port = new System.Windows.Forms.TextBox();
            this.IPAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.Confirm = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.min = new System.Windows.Forms.PictureBox();
            this.Close = new System.Windows.Forms.PictureBox();
            this.cuemessage = new System.Windows.Forms.ToolTip(this.components);
            this.errormessage = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.AllPanel.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Close)).BeginInit();
            this.SuspendLayout();
            // 
            // AllPanel
            // 
            this.AllPanel.BackColor = System.Drawing.Color.Transparent;
            this.AllPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AllPanel.Controls.Add(this.Port);
            this.AllPanel.Controls.Add(this.IPAddress);
            this.AllPanel.Controls.Add(this.label3);
            this.AllPanel.Controls.Add(this.label2);
            this.AllPanel.Controls.Add(this.label1);
            this.AllPanel.Controls.Add(this.BottomPanel);
            this.AllPanel.Controls.Add(this.HeaderPanel);
            this.AllPanel.Location = new System.Drawing.Point(0, 0);
            this.AllPanel.Name = "AllPanel";
            this.AllPanel.Size = new System.Drawing.Size(430, 334);
            this.AllPanel.TabIndex = 2;
            // 
            // Port
            // 
            this.Port.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Port.Location = new System.Drawing.Point(145, 163);
            this.Port.Name = "Port";
            this.Port.Size = new System.Drawing.Size(176, 29);
            this.Port.TabIndex = 8;
            // 
            // IPAddress
            // 
            this.IPAddress.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IPAddress.Location = new System.Drawing.Point(145, 124);
            this.IPAddress.Name = "IPAddress";
            this.IPAddress.Size = new System.Drawing.Size(176, 29);
            this.IPAddress.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(59, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 21);
            this.label3.TabIndex = 6;
            this.label3.Text = "端 口：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(59, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 21);
            this.label2.TabIndex = 5;
            this.label2.Text = "地 址：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(11, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 28);
            this.label1.TabIndex = 4;
            this.label1.Text = "服务器设置";
            // 
            // BottomPanel
            // 
            this.BottomPanel.BackgroundImage = global::SignPressClient.Properties.Resources.bottom_1_;
            this.BottomPanel.Controls.Add(this.Confirm);
            this.BottomPanel.Controls.Add(this.Cancel);
            this.BottomPanel.Location = new System.Drawing.Point(0, 284);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(430, 50);
            this.BottomPanel.TabIndex = 3;
            // 
            // Confirm
            // 
            this.Confirm.Font = new System.Drawing.Font("黑体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Confirm.Location = new System.Drawing.Point(243, 8);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(78, 34);
            this.Confirm.TabIndex = 4;
            this.Confirm.Text = "确  定";
            this.Confirm.UseVisualStyleBackColor = true;
            this.Confirm.Click += new System.EventHandler(this.Confirm_Click);
            // 
            // Cancel
            // 
            this.Cancel.Font = new System.Drawing.Font("黑体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Cancel.Location = new System.Drawing.Point(339, 8);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(78, 34);
            this.Cancel.TabIndex = 5;
            this.Cancel.Text = "取  消";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackgroundImage = global::SignPressClient.Properties.Resources.bottom;
            this.HeaderPanel.Controls.Add(this.min);
            this.HeaderPanel.Controls.Add(this.Close);
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(430, 46);
            this.HeaderPanel.TabIndex = 2;
            // 
            // min
            // 
            this.min.BackgroundImage = global::SignPressClient.Properties.Resources.icon_minus_06;
            this.min.Location = new System.Drawing.Point(363, -1);
            this.min.Name = "min";
            this.min.Size = new System.Drawing.Size(33, 32);
            this.min.TabIndex = 0;
            this.min.TabStop = false;
            this.min.Click += new System.EventHandler(this.min_Click);
            this.min.MouseEnter += new System.EventHandler(this.min_MouseEnter);
            this.min.MouseLeave += new System.EventHandler(this.min_MouseLeave);
            this.min.MouseMove += new System.Windows.Forms.MouseEventHandler(this.min_MouseMove);
            // 
            // Close
            // 
            this.Close.BackColor = System.Drawing.Color.Transparent;
            this.Close.BackgroundImage = global::SignPressClient.Properties.Resources.icon_close;
            this.Close.Location = new System.Drawing.Point(395, 0);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(33, 32);
            this.Close.TabIndex = 1;
            this.Close.TabStop = false;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            this.Close.MouseEnter += new System.EventHandler(this.Close_MouseEnter);
            this.Close.MouseLeave += new System.EventHandler(this.Close_MouseLeave);
            this.Close.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Close_MouseMove);
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(430, 334);
            this.Controls.Add(this.AllPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Setting";
            this.Text = "设置";
            this.Load += new System.EventHandler(this.Setting_Load);
            this.AllPanel.ResumeLayout(false);
            this.AllPanel.PerformLayout();
            this.BottomPanel.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Close)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox min;
        private System.Windows.Forms.PictureBox Close;
        private System.Windows.Forms.Panel AllPanel;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Button Confirm;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.TextBox IPAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Port;
        private System.Windows.Forms.ToolTip cuemessage;
        private System.Windows.Forms.ToolTip errormessage;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}