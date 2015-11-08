namespace SignPressClient
{
    partial class EditDepartment
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
            this.ModifyDepartment = new System.Windows.Forms.Button();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxId = new System.Windows.Forms.TextBox();
            this.labelId = new System.Windows.Forms.Label();
            this.textBoxShortCall = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CanBoundary = new System.Windows.Forms.CheckBox();
            this.CanInLand = new System.Windows.Forms.CheckBox();
            this.CanEmergency = new System.Windows.Forms.CheckBox();
            this.CanRegular = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ModifyDepartment
            // 
            this.ModifyDepartment.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ModifyDepartment.Location = new System.Drawing.Point(244, 216);
            this.ModifyDepartment.Name = "ModifyDepartment";
            this.ModifyDepartment.Size = new System.Drawing.Size(87, 34);
            this.ModifyDepartment.TabIndex = 6;
            this.ModifyDepartment.Text = "修改";
            this.ModifyDepartment.UseVisualStyleBackColor = true;
            this.ModifyDepartment.Click += new System.EventHandler(this.ModifyDepartment_Click);
            // 
            // textBoxName
            // 
            this.textBoxName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxName.Location = new System.Drawing.Point(116, 74);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(132, 29);
            this.textBoxName.TabIndex = 5;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelName.Location = new System.Drawing.Point(36, 77);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(74, 21);
            this.labelName.TabIndex = 4;
            this.labelName.Text = "部门名称";
            // 
            // textBoxId
            // 
            this.textBoxId.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxId.Location = new System.Drawing.Point(116, 21);
            this.textBoxId.Name = "textBoxId";
            this.textBoxId.ReadOnly = true;
            this.textBoxId.Size = new System.Drawing.Size(132, 29);
            this.textBoxId.TabIndex = 8;
            // 
            // labelId
            // 
            this.labelId.AutoSize = true;
            this.labelId.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelId.Location = new System.Drawing.Point(36, 24);
            this.labelId.Name = "labelId";
            this.labelId.Size = new System.Drawing.Size(74, 21);
            this.labelId.TabIndex = 7;
            this.labelId.Text = "部门编号";
            // 
            // textBoxShortCall
            // 
            this.textBoxShortCall.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxShortCall.Location = new System.Drawing.Point(376, 74);
            this.textBoxShortCall.Name = "textBoxShortCall";
            this.textBoxShortCall.Size = new System.Drawing.Size(132, 29);
            this.textBoxShortCall.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(296, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 9;
            this.label1.Text = "部门简称";
            // 
            // CanBoundary
            // 
            this.CanBoundary.AutoSize = true;
            this.CanBoundary.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CanBoundary.Location = new System.Drawing.Point(40, 128);
            this.CanBoundary.Name = "CanBoundary";
            this.CanBoundary.Size = new System.Drawing.Size(126, 24);
            this.CanBoundary.TabIndex = 12;
            this.CanBoundary.Text = "可申请界河项目";
            this.CanBoundary.UseVisualStyleBackColor = true;
            // 
            // CanInLand
            // 
            this.CanInLand.AutoSize = true;
            this.CanInLand.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CanInLand.Location = new System.Drawing.Point(300, 128);
            this.CanInLand.Name = "CanInLand";
            this.CanInLand.Size = new System.Drawing.Size(126, 24);
            this.CanInLand.TabIndex = 13;
            this.CanInLand.Text = "可申请内河项目";
            this.CanInLand.UseVisualStyleBackColor = true;
            // 
            // CanEmergency
            // 
            this.CanEmergency.AutoSize = true;
            this.CanEmergency.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CanEmergency.Location = new System.Drawing.Point(40, 174);
            this.CanEmergency.Name = "CanEmergency";
            this.CanEmergency.Size = new System.Drawing.Size(126, 24);
            this.CanEmergency.TabIndex = 14;
            this.CanEmergency.Text = "可申请应急项目";
            this.CanEmergency.UseVisualStyleBackColor = true;
            // 
            // CanRegular
            // 
            this.CanRegular.AutoSize = true;
            this.CanRegular.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CanRegular.Location = new System.Drawing.Point(300, 174);
            this.CanRegular.Name = "CanRegular";
            this.CanRegular.Size = new System.Drawing.Size(126, 24);
            this.CanRegular.TabIndex = 15;
            this.CanRegular.Text = "可申请例会项目";
            this.CanRegular.UseVisualStyleBackColor = true;
            // 
            // EditDepartment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 262);
            this.Controls.Add(this.CanRegular);
            this.Controls.Add(this.CanEmergency);
            this.Controls.Add(this.CanInLand);
            this.Controls.Add(this.CanBoundary);
            this.Controls.Add(this.textBoxShortCall);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxId);
            this.Controls.Add(this.labelId);
            this.Controls.Add(this.ModifyDepartment);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.Name = "EditDepartment";
            this.Text = "部门信息";
            this.Load += new System.EventHandler(this.EditDepartment_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ModifyDepartment;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxId;
        private System.Windows.Forms.Label labelId;
        private System.Windows.Forms.TextBox textBoxShortCall;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox CanBoundary;
        private System.Windows.Forms.CheckBox CanInLand;
        private System.Windows.Forms.CheckBox CanEmergency;
        private System.Windows.Forms.CheckBox CanRegular;
    }
}