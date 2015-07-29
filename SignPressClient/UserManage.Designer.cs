namespace SignPressClient
{
    partial class UserManage
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
            this.label1 = new System.Windows.Forms.Label();
            this.Users = new System.Windows.Forms.TabControl();
            this.ViewPersonal = new System.Windows.Forms.TabPage();
            this.buttonRePicture = new System.Windows.Forms.Button();
            this.buttonModifyEmployee = new System.Windows.Forms.Button();
            this.labelIsAdmin = new System.Windows.Forms.Label();
            this.labelCanSubmit = new System.Windows.Forms.Label();
            this.labelCanSign = new System.Windows.Forms.Label();
            this.textBoxDepartment = new System.Windows.Forms.TextBox();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.labelPosition = new System.Windows.Forms.Label();
            this.textBoxPosition = new System.Windows.Forms.TextBox();
            this.labelDepartment = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.ChangePassword = new System.Windows.Forms.TabPage();
            this.buttonModifyPWDCancle = new System.Windows.Forms.Button();
            this.buttonModifyPWDConfirm = new System.Windows.Forms.Button();
            this.textBoxNewPassword = new System.Windows.Forms.TextBox();
            this.textBoxRePassword = new System.Windows.Forms.TextBox();
            this.textBoxOldPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Users.SuspendLayout();
            this.ViewPersonal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.ChangePassword.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户管理";
            // 
            // Users
            // 
            this.Users.Controls.Add(this.ViewPersonal);
            this.Users.Controls.Add(this.ChangePassword);
            this.Users.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Users.Location = new System.Drawing.Point(0, 25);
            this.Users.Name = "Users";
            this.Users.SelectedIndex = 0;
            this.Users.Size = new System.Drawing.Size(702, 376);
            this.Users.TabIndex = 1;
            // 
            // ViewPersonal
            // 
            this.ViewPersonal.Controls.Add(this.buttonRePicture);
            this.ViewPersonal.Controls.Add(this.buttonModifyEmployee);
            this.ViewPersonal.Controls.Add(this.labelIsAdmin);
            this.ViewPersonal.Controls.Add(this.labelCanSubmit);
            this.ViewPersonal.Controls.Add(this.labelCanSign);
            this.ViewPersonal.Controls.Add(this.textBoxDepartment);
            this.ViewPersonal.Controls.Add(this.PictureBox);
            this.ViewPersonal.Controls.Add(this.label8);
            this.ViewPersonal.Controls.Add(this.textBoxUsername);
            this.ViewPersonal.Controls.Add(this.label6);
            this.ViewPersonal.Controls.Add(this.labelPosition);
            this.ViewPersonal.Controls.Add(this.textBoxPosition);
            this.ViewPersonal.Controls.Add(this.labelDepartment);
            this.ViewPersonal.Controls.Add(this.textBoxName);
            this.ViewPersonal.Controls.Add(this.labelName);
            this.ViewPersonal.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ViewPersonal.Location = new System.Drawing.Point(4, 22);
            this.ViewPersonal.Name = "ViewPersonal";
            this.ViewPersonal.Padding = new System.Windows.Forms.Padding(3);
            this.ViewPersonal.Size = new System.Drawing.Size(694, 350);
            this.ViewPersonal.TabIndex = 0;
            this.ViewPersonal.Text = "查看个人信息";
            this.ViewPersonal.UseVisualStyleBackColor = true;
            // 
            // buttonRePicture
            // 
            this.buttonRePicture.Location = new System.Drawing.Point(483, 235);
            this.buttonRePicture.Name = "buttonRePicture";
            this.buttonRePicture.Size = new System.Drawing.Size(147, 31);
            this.buttonRePicture.TabIndex = 50;
            this.buttonRePicture.Text = "点击修改签字图片";
            this.buttonRePicture.UseVisualStyleBackColor = true;
            this.buttonRePicture.Visible = false;
            this.buttonRePicture.Click += new System.EventHandler(this.buttonRePicture_Click);
            // 
            // buttonModifyEmployee
            // 
            this.buttonModifyEmployee.Location = new System.Drawing.Point(483, 144);
            this.buttonModifyEmployee.Name = "buttonModifyEmployee";
            this.buttonModifyEmployee.Size = new System.Drawing.Size(147, 31);
            this.buttonModifyEmployee.TabIndex = 49;
            this.buttonModifyEmployee.Text = "点击开始修改";
            this.buttonModifyEmployee.UseVisualStyleBackColor = true;
            this.buttonModifyEmployee.Visible = false;
            this.buttonModifyEmployee.Click += new System.EventHandler(this.buttonModifyEmployee_Click);
            // 
            // labelIsAdmin
            // 
            this.labelIsAdmin.AutoSize = true;
            this.labelIsAdmin.Location = new System.Drawing.Point(280, 154);
            this.labelIsAdmin.Name = "labelIsAdmin";
            this.labelIsAdmin.Size = new System.Drawing.Size(58, 21);
            this.labelIsAdmin.TabIndex = 48;
            this.labelIsAdmin.Text = "管理员";
            // 
            // labelCanSubmit
            // 
            this.labelCanSubmit.AutoSize = true;
            this.labelCanSubmit.Location = new System.Drawing.Point(207, 154);
            this.labelCanSubmit.Name = "labelCanSubmit";
            this.labelCanSubmit.Size = new System.Drawing.Size(58, 21);
            this.labelCanSubmit.TabIndex = 47;
            this.labelCanSubmit.Text = "可提交";
            // 
            // labelCanSign
            // 
            this.labelCanSign.AutoSize = true;
            this.labelCanSign.Location = new System.Drawing.Point(143, 154);
            this.labelCanSign.Name = "labelCanSign";
            this.labelCanSign.Size = new System.Drawing.Size(58, 21);
            this.labelCanSign.TabIndex = 46;
            this.labelCanSign.Text = "可签字";
            // 
            // textBoxDepartment
            // 
            this.textBoxDepartment.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxDepartment.Location = new System.Drawing.Point(147, 78);
            this.textBoxDepartment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxDepartment.Name = "textBoxDepartment";
            this.textBoxDepartment.ReadOnly = true;
            this.textBoxDepartment.Size = new System.Drawing.Size(161, 29);
            this.textBoxDepartment.TabIndex = 45;
            // 
            // PictureBox
            // 
            this.PictureBox.Location = new System.Drawing.Point(196, 195);
            this.PictureBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(223, 122);
            this.PictureBox.TabIndex = 44;
            this.PictureBox.TabStop = false;
            this.PictureBox.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(53, 154);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 21);
            this.label8.TabIndex = 40;
            this.label8.Text = "权   限：";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxUsername.Location = new System.Drawing.Point(469, 20);
            this.textBoxUsername.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.ReadOnly = true;
            this.textBoxUsername.Size = new System.Drawing.Size(171, 29);
            this.textBoxUsername.TabIndex = 39;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(377, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 21);
            this.label6.TabIndex = 38;
            this.label6.Text = "用户名：";
            // 
            // labelPosition
            // 
            this.labelPosition.AutoSize = true;
            this.labelPosition.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelPosition.Location = new System.Drawing.Point(377, 79);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(90, 21);
            this.labelPosition.TabIndex = 36;
            this.labelPosition.Text = "所属岗位：";
            // 
            // textBoxPosition
            // 
            this.textBoxPosition.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxPosition.Location = new System.Drawing.Point(469, 80);
            this.textBoxPosition.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxPosition.Name = "textBoxPosition";
            this.textBoxPosition.ReadOnly = true;
            this.textBoxPosition.Size = new System.Drawing.Size(171, 29);
            this.textBoxPosition.TabIndex = 35;
            // 
            // labelDepartment
            // 
            this.labelDepartment.AutoSize = true;
            this.labelDepartment.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDepartment.Location = new System.Drawing.Point(53, 81);
            this.labelDepartment.Name = "labelDepartment";
            this.labelDepartment.Size = new System.Drawing.Size(90, 21);
            this.labelDepartment.TabIndex = 34;
            this.labelDepartment.Text = "所属部门：";
            // 
            // textBoxName
            // 
            this.textBoxName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxName.Location = new System.Drawing.Point(144, 20);
            this.textBoxName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.ReadOnly = true;
            this.textBoxName.Size = new System.Drawing.Size(161, 29);
            this.textBoxName.TabIndex = 33;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelName.Location = new System.Drawing.Point(53, 24);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(83, 21);
            this.labelName.TabIndex = 32;
            this.labelName.Text = "姓     名：";
            // 
            // ChangePassword
            // 
            this.ChangePassword.Controls.Add(this.buttonModifyPWDCancle);
            this.ChangePassword.Controls.Add(this.buttonModifyPWDConfirm);
            this.ChangePassword.Controls.Add(this.textBoxNewPassword);
            this.ChangePassword.Controls.Add(this.textBoxRePassword);
            this.ChangePassword.Controls.Add(this.textBoxOldPassword);
            this.ChangePassword.Controls.Add(this.label4);
            this.ChangePassword.Controls.Add(this.label3);
            this.ChangePassword.Controls.Add(this.label2);
            this.ChangePassword.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ChangePassword.Location = new System.Drawing.Point(4, 22);
            this.ChangePassword.Name = "ChangePassword";
            this.ChangePassword.Padding = new System.Windows.Forms.Padding(3);
            this.ChangePassword.Size = new System.Drawing.Size(694, 350);
            this.ChangePassword.TabIndex = 1;
            this.ChangePassword.Text = "修改密码";
            this.ChangePassword.UseVisualStyleBackColor = true;
            // 
            // buttonModifyPWDCancle
            // 
            this.buttonModifyPWDCancle.Location = new System.Drawing.Point(171, 157);
            this.buttonModifyPWDCancle.Name = "buttonModifyPWDCancle";
            this.buttonModifyPWDCancle.Size = new System.Drawing.Size(80, 31);
            this.buttonModifyPWDCancle.TabIndex = 4;
            this.buttonModifyPWDCancle.Text = "取消";
            this.buttonModifyPWDCancle.UseVisualStyleBackColor = true;
            this.buttonModifyPWDCancle.Click += new System.EventHandler(this.buttonModifyPWDCancle_Click);
            // 
            // buttonModifyPWDConfirm
            // 
            this.buttonModifyPWDConfirm.Location = new System.Drawing.Point(44, 157);
            this.buttonModifyPWDConfirm.Name = "buttonModifyPWDConfirm";
            this.buttonModifyPWDConfirm.Size = new System.Drawing.Size(80, 31);
            this.buttonModifyPWDConfirm.TabIndex = 3;
            this.buttonModifyPWDConfirm.Text = "修改";
            this.buttonModifyPWDConfirm.UseVisualStyleBackColor = true;
            this.buttonModifyPWDConfirm.Click += new System.EventHandler(this.buttonModifyPWDConfirm_Click);
            // 
            // textBoxNewPassword
            // 
            this.textBoxNewPassword.Location = new System.Drawing.Point(100, 62);
            this.textBoxNewPassword.Name = "textBoxNewPassword";
            this.textBoxNewPassword.Size = new System.Drawing.Size(167, 29);
            this.textBoxNewPassword.TabIndex = 1;
            this.textBoxNewPassword.UseSystemPasswordChar = true;
            // 
            // textBoxRePassword
            // 
            this.textBoxRePassword.Location = new System.Drawing.Point(100, 102);
            this.textBoxRePassword.Name = "textBoxRePassword";
            this.textBoxRePassword.Size = new System.Drawing.Size(167, 29);
            this.textBoxRePassword.TabIndex = 2;
            this.textBoxRePassword.UseSystemPasswordChar = true;
            // 
            // textBoxOldPassword
            // 
            this.textBoxOldPassword.Location = new System.Drawing.Point(100, 23);
            this.textBoxOldPassword.Name = "textBoxOldPassword";
            this.textBoxOldPassword.Size = new System.Drawing.Size(167, 29);
            this.textBoxOldPassword.TabIndex = 0;
            this.textBoxOldPassword.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 21);
            this.label4.TabIndex = 2;
            this.label4.Text = "确认密码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 21);
            this.label3.TabIndex = 1;
            this.label3.Text = "新密码：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 21);
            this.label2.TabIndex = 7;
            this.label2.Text = "原密码：";
            // 
            // UserManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(702, 401);
            this.Controls.Add(this.Users);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(202, 0);
            this.Name = "UserManage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "UserManage";
            this.Users.ResumeLayout(false);
            this.ViewPersonal.ResumeLayout(false);
            this.ViewPersonal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.ChangePassword.ResumeLayout(false);
            this.ChangePassword.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl Users;
        private System.Windows.Forms.TabPage ViewPersonal;
        private System.Windows.Forms.TabPage ChangePassword;
        private System.Windows.Forms.TextBox textBoxNewPassword;
        private System.Windows.Forms.TextBox textBoxRePassword;
        private System.Windows.Forms.TextBox textBoxOldPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonModifyPWDCancle;
        private System.Windows.Forms.Button buttonModifyPWDConfirm;
        private System.Windows.Forms.Label labelIsAdmin;
        private System.Windows.Forms.Label labelCanSubmit;
        private System.Windows.Forms.Label labelCanSign;
        private System.Windows.Forms.TextBox textBoxDepartment;
        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.TextBox textBoxPosition;
        private System.Windows.Forms.Label labelDepartment;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Button buttonModifyEmployee;
        private System.Windows.Forms.Button buttonRePicture;
    }
}