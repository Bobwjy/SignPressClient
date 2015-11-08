namespace SignPressClient
{
    partial class AdminManage
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
            this.Admin = new System.Windows.Forms.TabControl();
            this.Department = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.DepartmentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DepartName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DepartmentShortCall = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DepartmentCanBoundary = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DepartmentCanInland = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DepartmentCanEmergency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DepartmentCanRegular = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxDepartmentShortCall = new System.Windows.Forms.TextBox();
            this.labelDepartmentShortCall = new System.Windows.Forms.Label();
            this.AddDepartment = new System.Windows.Forms.Button();
            this.DepartmentName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.EmployeeManage = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.EmployeeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.empName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmployName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DepartmentInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PositionInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CanSub = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CanSignPress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsAdm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.UploadButton = new System.Windows.Forms.Button();
            this.PicturrName = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.CanAdmin = new System.Windows.Forms.CheckBox();
            this.CanSign = new System.Windows.Forms.CheckBox();
            this.CanSubmit = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.UserPassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.UserName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Position = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SelectedDepartment = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.eName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Templete = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.AddTemp = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.TemplateID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TemplateName = new System.Windows.Forms.DataGridViewLinkColumn();
            this.CraeteTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.CanBoundary = new System.Windows.Forms.CheckBox();
            this.CanEmergency = new System.Windows.Forms.CheckBox();
            this.CanRegular = new System.Windows.Forms.CheckBox();
            this.CanInLand = new System.Windows.Forms.CheckBox();
            this.Admin.SuspendLayout();
            this.Department.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.EmployeeManage.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.Templete.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "管理员任务界面";
            // 
            // Admin
            // 
            this.Admin.Controls.Add(this.Department);
            this.Admin.Controls.Add(this.EmployeeManage);
            this.Admin.Controls.Add(this.Templete);
            this.Admin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Admin.Location = new System.Drawing.Point(0, 25);
            this.Admin.MinimumSize = new System.Drawing.Size(615, 402);
            this.Admin.Name = "Admin";
            this.Admin.SelectedIndex = 0;
            this.Admin.Size = new System.Drawing.Size(865, 549);
            this.Admin.TabIndex = 1;
            this.Admin.SelectedIndexChanged += new System.EventHandler(this.Admin_SelectedIndexChanged);
            // 
            // Department
            // 
            this.Department.Controls.Add(this.groupBox1);
            this.Department.Controls.Add(this.panel1);
            this.Department.Location = new System.Drawing.Point(4, 22);
            this.Department.Name = "Department";
            this.Department.Padding = new System.Windows.Forms.Padding(3);
            this.Department.Size = new System.Drawing.Size(857, 523);
            this.Department.TabIndex = 0;
            this.Department.Text = "部门管理";
            this.Department.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(3, 113);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(851, 407);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "部门信息";
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DepartmentID,
            this.DepartName,
            this.DepartmentShortCall,
            this.DepartmentCanBoundary,
            this.DepartmentCanInland,
            this.DepartmentCanEmergency,
            this.DepartmentCanRegular});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 25);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(845, 379);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // DepartmentID
            // 
            this.DepartmentID.DataPropertyName = "Id";
            this.DepartmentID.HeaderText = "部门编号";
            this.DepartmentID.Name = "DepartmentID";
            this.DepartmentID.ReadOnly = true;
            // 
            // DepartName
            // 
            this.DepartName.DataPropertyName = "Name";
            this.DepartName.HeaderText = "部门名称";
            this.DepartName.Name = "DepartName";
            this.DepartName.ReadOnly = true;
            // 
            // DepartmentShortCall
            // 
            this.DepartmentShortCall.DataPropertyName = "ShortCall";
            this.DepartmentShortCall.HeaderText = "部门简称";
            this.DepartmentShortCall.Name = "DepartmentShortCall";
            this.DepartmentShortCall.ReadOnly = true;
            // 
            // DepartmentCanBoundary
            // 
            this.DepartmentCanBoundary.DataPropertyName = "CanBoundary";
            this.DepartmentCanBoundary.HeaderText = "可申请界河项目";
            this.DepartmentCanBoundary.Name = "DepartmentCanBoundary";
            this.DepartmentCanBoundary.ReadOnly = true;
            // 
            // DepartmentCanInland
            // 
            this.DepartmentCanInland.DataPropertyName = "CanInland";
            this.DepartmentCanInland.HeaderText = "可申请内河项目";
            this.DepartmentCanInland.Name = "DepartmentCanInland";
            this.DepartmentCanInland.ReadOnly = true;
            // 
            // DepartmentCanEmergency
            // 
            this.DepartmentCanEmergency.DataPropertyName = "CanEmergency";
            this.DepartmentCanEmergency.HeaderText = "可申请应急项目";
            this.DepartmentCanEmergency.Name = "DepartmentCanEmergency";
            this.DepartmentCanEmergency.ReadOnly = true;
            // 
            // DepartmentCanRegular
            // 
            this.DepartmentCanRegular.DataPropertyName = "CanRegular";
            this.DepartmentCanRegular.HeaderText = "可申请例会项目";
            this.DepartmentCanRegular.Name = "DepartmentCanRegular";
            this.DepartmentCanRegular.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Aqua;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.CanInLand);
            this.panel1.Controls.Add(this.CanRegular);
            this.panel1.Controls.Add(this.CanEmergency);
            this.panel1.Controls.Add(this.CanBoundary);
            this.panel1.Controls.Add(this.textBoxDepartmentShortCall);
            this.panel1.Controls.Add(this.labelDepartmentShortCall);
            this.panel1.Controls.Add(this.AddDepartment);
            this.panel1.Controls.Add(this.DepartmentName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(851, 110);
            this.panel1.TabIndex = 0;
            // 
            // textBoxDepartmentShortCall
            // 
            this.textBoxDepartmentShortCall.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxDepartmentShortCall.Location = new System.Drawing.Point(331, 14);
            this.textBoxDepartmentShortCall.Name = "textBoxDepartmentShortCall";
            this.textBoxDepartmentShortCall.Size = new System.Drawing.Size(132, 25);
            this.textBoxDepartmentShortCall.TabIndex = 5;
            // 
            // labelDepartmentShortCall
            // 
            this.labelDepartmentShortCall.AutoSize = true;
            this.labelDepartmentShortCall.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDepartmentShortCall.Location = new System.Drawing.Point(264, 17);
            this.labelDepartmentShortCall.Name = "labelDepartmentShortCall";
            this.labelDepartmentShortCall.Size = new System.Drawing.Size(61, 19);
            this.labelDepartmentShortCall.TabIndex = 4;
            this.labelDepartmentShortCall.Text = "部门简称";
            // 
            // AddDepartment
            // 
            this.AddDepartment.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AddDepartment.Location = new System.Drawing.Point(746, 76);
            this.AddDepartment.Name = "AddDepartment";
            this.AddDepartment.Size = new System.Drawing.Size(87, 27);
            this.AddDepartment.TabIndex = 3;
            this.AddDepartment.Text = "添    加";
            this.AddDepartment.UseVisualStyleBackColor = true;
            this.AddDepartment.Click += new System.EventHandler(this.AddDepartment_Click);
            // 
            // DepartmentName
            // 
            this.DepartmentName.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DepartmentName.Location = new System.Drawing.Point(108, 14);
            this.DepartmentName.Name = "DepartmentName";
            this.DepartmentName.Size = new System.Drawing.Size(132, 25);
            this.DepartmentName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(41, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "部门名称";
            // 
            // EmployeeManage
            // 
            this.EmployeeManage.Controls.Add(this.panel3);
            this.EmployeeManage.Controls.Add(this.panel2);
            this.EmployeeManage.Location = new System.Drawing.Point(4, 22);
            this.EmployeeManage.Name = "EmployeeManage";
            this.EmployeeManage.Padding = new System.Windows.Forms.Padding(3);
            this.EmployeeManage.Size = new System.Drawing.Size(857, 523);
            this.EmployeeManage.TabIndex = 1;
            this.EmployeeManage.Text = "人员管理";
            this.EmployeeManage.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 174);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(851, 346);
            this.panel3.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridView3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(851, 346);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "员工信息";
            // 
            // dataGridView3
            // 
            this.dataGridView3.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EmployeeID,
            this.empName,
            this.EmployName,
            this.DepartmentInfo,
            this.PositionInfo,
            this.CanSub,
            this.CanSignPress,
            this.IsAdm});
            this.dataGridView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView3.Location = new System.Drawing.Point(3, 25);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowTemplate.Height = 23;
            this.dataGridView3.Size = new System.Drawing.Size(845, 318);
            this.dataGridView3.TabIndex = 0;
            this.dataGridView3.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView3_CellContentClick);
            // 
            // EmployeeID
            // 
            this.EmployeeID.DataPropertyName = "Id";
            this.EmployeeID.HeaderText = "员工编号";
            this.EmployeeID.Name = "EmployeeID";
            this.EmployeeID.ReadOnly = true;
            // 
            // empName
            // 
            this.empName.DataPropertyName = "Name";
            this.empName.HeaderText = "姓名";
            this.empName.Name = "empName";
            this.empName.ReadOnly = true;
            // 
            // EmployName
            // 
            this.EmployName.DataPropertyName = "UserName";
            this.EmployName.HeaderText = "用户名";
            this.EmployName.Name = "EmployName";
            this.EmployName.ReadOnly = true;
            // 
            // DepartmentInfo
            // 
            this.DepartmentInfo.DataPropertyName = "Department";
            this.DepartmentInfo.HeaderText = "所属部门";
            this.DepartmentInfo.Name = "DepartmentInfo";
            this.DepartmentInfo.ReadOnly = true;
            // 
            // PositionInfo
            // 
            this.PositionInfo.DataPropertyName = "Position";
            this.PositionInfo.HeaderText = "所属岗位";
            this.PositionInfo.Name = "PositionInfo";
            this.PositionInfo.ReadOnly = true;
            // 
            // CanSub
            // 
            this.CanSub.DataPropertyName = "CanSubmit";
            this.CanSub.HeaderText = "可提交";
            this.CanSub.Name = "CanSub";
            this.CanSub.ReadOnly = true;
            // 
            // CanSignPress
            // 
            this.CanSignPress.DataPropertyName = "CanSign";
            this.CanSignPress.HeaderText = "可签字";
            this.CanSignPress.Name = "CanSignPress";
            this.CanSignPress.ReadOnly = true;
            // 
            // IsAdm
            // 
            this.IsAdm.DataPropertyName = "IsAdmin";
            this.IsAdm.HeaderText = "可管理";
            this.IsAdm.Name = "IsAdm";
            this.IsAdm.ReadOnly = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Aqua;
            this.panel2.Controls.Add(this.UploadButton);
            this.panel2.Controls.Add(this.PicturrName);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.CanAdmin);
            this.panel2.Controls.Add(this.CanSign);
            this.panel2.Controls.Add(this.CanSubmit);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.UserPassword);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.UserName);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.Position);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.SelectedDepartment);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.eName);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(851, 171);
            this.panel2.TabIndex = 0;
            // 
            // UploadButton
            // 
            this.UploadButton.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UploadButton.Location = new System.Drawing.Point(526, 107);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(86, 33);
            this.UploadButton.TabIndex = 17;
            this.UploadButton.Text = "选择图片";
            this.UploadButton.UseVisualStyleBackColor = true;
            this.UploadButton.Visible = false;
            this.UploadButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // PicturrName
            // 
            this.PicturrName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PicturrName.Location = new System.Drawing.Point(366, 109);
            this.PicturrName.Name = "PicturrName";
            this.PicturrName.ReadOnly = true;
            this.PicturrName.Size = new System.Drawing.Size(154, 29);
            this.PicturrName.TabIndex = 16;
            this.PicturrName.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(618, 47);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(191, 86);
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(725, 136);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 32);
            this.button1.TabIndex = 14;
            this.button1.Text = "添   加";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CanAdmin
            // 
            this.CanAdmin.AutoSize = true;
            this.CanAdmin.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CanAdmin.Location = new System.Drawing.Point(250, 112);
            this.CanAdmin.Name = "CanAdmin";
            this.CanAdmin.Size = new System.Drawing.Size(77, 25);
            this.CanAdmin.TabIndex = 13;
            this.CanAdmin.Text = "可管理";
            this.CanAdmin.UseVisualStyleBackColor = true;
            // 
            // CanSign
            // 
            this.CanSign.AutoSize = true;
            this.CanSign.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CanSign.Location = new System.Drawing.Point(166, 112);
            this.CanSign.Name = "CanSign";
            this.CanSign.Size = new System.Drawing.Size(77, 25);
            this.CanSign.TabIndex = 12;
            this.CanSign.Text = "可签字";
            this.CanSign.UseVisualStyleBackColor = true;
            this.CanSign.Click += new System.EventHandler(this.CanSign_Click);
            // 
            // CanSubmit
            // 
            this.CanSubmit.AutoSize = true;
            this.CanSubmit.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CanSubmit.Location = new System.Drawing.Point(83, 111);
            this.CanSubmit.Name = "CanSubmit";
            this.CanSubmit.Size = new System.Drawing.Size(77, 25);
            this.CanSubmit.TabIndex = 11;
            this.CanSubmit.Text = "可提交";
            this.CanSubmit.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(14, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 21);
            this.label8.TabIndex = 10;
            this.label8.Text = "权   限：";
            // 
            // UserPassword
            // 
            this.UserPassword.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UserPassword.Location = new System.Drawing.Point(334, 61);
            this.UserPassword.Name = "UserPassword";
            this.UserPassword.Size = new System.Drawing.Size(118, 29);
            this.UserPassword.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(246, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 21);
            this.label7.TabIndex = 8;
            this.label7.Text = "用户密码：";
            // 
            // UserName
            // 
            this.UserName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UserName.Location = new System.Drawing.Point(83, 61);
            this.UserName.Name = "UserName";
            this.UserName.Size = new System.Drawing.Size(118, 29);
            this.UserName.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(13, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 21);
            this.label6.TabIndex = 6;
            this.label6.Text = "用户名：";
            // 
            // Position
            // 
            this.Position.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Position.Location = new System.Drawing.Point(606, 12);
            this.Position.Name = "Position";
            this.Position.Size = new System.Drawing.Size(118, 29);
            this.Position.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(522, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 21);
            this.label5.TabIndex = 4;
            this.label5.Text = "所属岗位：";
            // 
            // SelectedDepartment
            // 
            this.SelectedDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectedDepartment.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SelectedDepartment.FormattingEnabled = true;
            this.SelectedDepartment.Location = new System.Drawing.Point(331, 12);
            this.SelectedDepartment.Name = "SelectedDepartment";
            this.SelectedDepartment.Size = new System.Drawing.Size(121, 29);
            this.SelectedDepartment.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(246, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 21);
            this.label4.TabIndex = 2;
            this.label4.Text = "所属部门：";
            // 
            // eName
            // 
            this.eName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.eName.Location = new System.Drawing.Point(83, 12);
            this.eName.Name = "eName";
            this.eName.Size = new System.Drawing.Size(118, 29);
            this.eName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(14, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 21);
            this.label3.TabIndex = 0;
            this.label3.Text = "姓   名：";
            // 
            // Templete
            // 
            this.Templete.Controls.Add(this.panel4);
            this.Templete.Controls.Add(this.groupBox2);
            this.Templete.Location = new System.Drawing.Point(4, 22);
            this.Templete.Name = "Templete";
            this.Templete.Size = new System.Drawing.Size(857, 523);
            this.Templete.TabIndex = 2;
            this.Templete.Text = "模板管理";
            this.Templete.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.AddTemp);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 473);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(857, 50);
            this.panel4.TabIndex = 5;
            // 
            // AddTemp
            // 
            this.AddTemp.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.AddTemp.Location = new System.Drawing.Point(381, 9);
            this.AddTemp.Name = "AddTemp";
            this.AddTemp.Size = new System.Drawing.Size(100, 33);
            this.AddTemp.TabIndex = 3;
            this.AddTemp.Text = "添加模板";
            this.AddTemp.UseVisualStyleBackColor = true;
            this.AddTemp.Click += new System.EventHandler(this.AddTemp_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(857, 523);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "模板信息";
            // 
            // dataGridView2
            // 
            this.dataGridView2.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TemplateID,
            this.TemplateName,
            this.CraeteTime});
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 25);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(851, 495);
            this.dataGridView2.TabIndex = 4;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            // 
            // TemplateID
            // 
            this.TemplateID.DataPropertyName = "TempId";
            this.TemplateID.HeaderText = "模板编号";
            this.TemplateID.Name = "TemplateID";
            this.TemplateID.ReadOnly = true;
            // 
            // TemplateName
            // 
            this.TemplateName.DataPropertyName = "Name";
            this.TemplateName.HeaderText = "模板名称";
            this.TemplateName.Name = "TemplateName";
            this.TemplateName.ReadOnly = true;
            this.TemplateName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TemplateName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.TemplateName.Width = 300;
            // 
            // CraeteTime
            // 
            this.CraeteTime.DataPropertyName = "CreateDate";
            this.CraeteTime.HeaderText = "创建时间";
            this.CraeteTime.Name = "CraeteTime";
            this.CraeteTime.ReadOnly = true;
            this.CraeteTime.Width = 180;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // CanBoundary
            // 
            this.CanBoundary.AutoSize = true;
            this.CanBoundary.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CanBoundary.Location = new System.Drawing.Point(45, 55);
            this.CanBoundary.Name = "CanBoundary";
            this.CanBoundary.Size = new System.Drawing.Size(119, 23);
            this.CanBoundary.TabIndex = 3;
            this.CanBoundary.Text = "可申请界河项目";
            this.CanBoundary.UseVisualStyleBackColor = true;
            // 
            // CanEmergency
            // 
            this.CanEmergency.AutoSize = true;
            this.CanEmergency.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CanEmergency.Location = new System.Drawing.Point(375, 55);
            this.CanEmergency.Name = "CanEmergency";
            this.CanEmergency.Size = new System.Drawing.Size(119, 23);
            this.CanEmergency.TabIndex = 6;
            this.CanEmergency.Text = "可申请应急项目";
            this.CanEmergency.UseVisualStyleBackColor = true;
            // 
            // CanRegular
            // 
            this.CanRegular.AutoSize = true;
            this.CanRegular.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CanRegular.Location = new System.Drawing.Point(550, 55);
            this.CanRegular.Name = "CanRegular";
            this.CanRegular.Size = new System.Drawing.Size(119, 23);
            this.CanRegular.TabIndex = 7;
            this.CanRegular.Text = "可申请例会项目";
            this.CanRegular.UseVisualStyleBackColor = true;
            // 
            // CanInLand
            // 
            this.CanInLand.AutoSize = true;
            this.CanInLand.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CanInLand.Location = new System.Drawing.Point(208, 55);
            this.CanInLand.Name = "CanInLand";
            this.CanInLand.Size = new System.Drawing.Size(119, 23);
            this.CanInLand.TabIndex = 8;
            this.CanInLand.Text = "可申请内河项目";
            this.CanInLand.UseVisualStyleBackColor = true;
            // 
            // AdminManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(865, 574);
            this.Controls.Add(this.Admin);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(202, 0);
            this.Name = "AdminManage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.DepartmentManage_Load);
            this.Admin.ResumeLayout(false);
            this.Department.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.EmployeeManage.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.Templete.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl Admin;
        private System.Windows.Forms.TabPage Department;
        private System.Windows.Forms.TabPage EmployeeManage;
        private System.Windows.Forms.TabPage Templete;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button AddDepartment;
        private System.Windows.Forms.TextBox DepartmentName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button AddTemp;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox eName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox SelectedDepartment;
        private System.Windows.Forms.TextBox Position;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox UserName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox UserPassword;
        private System.Windows.Forms.CheckBox CanSign;
        private System.Windows.Forms.CheckBox CanSubmit;
        private System.Windows.Forms.CheckBox CanAdmin;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button UploadButton;
        private System.Windows.Forms.TextBox PicturrName;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmployeeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn empName;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmployName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DepartmentInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn PositionInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn CanSub;
        private System.Windows.Forms.DataGridViewTextBoxColumn CanSignPress;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsAdm;
        private System.Windows.Forms.DataGridViewTextBoxColumn TemplateID;
        private System.Windows.Forms.DataGridViewLinkColumn TemplateName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CraeteTime;
        private System.Windows.Forms.TextBox textBoxDepartmentShortCall;
        private System.Windows.Forms.Label labelDepartmentShortCall;
        private System.Windows.Forms.DataGridViewTextBoxColumn DepartmentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DepartName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DepartmentShortCall;
        private System.Windows.Forms.DataGridViewTextBoxColumn DepartmentCanBoundary;
        private System.Windows.Forms.DataGridViewTextBoxColumn DepartmentCanInland;
        private System.Windows.Forms.DataGridViewTextBoxColumn DepartmentCanEmergency;
        private System.Windows.Forms.DataGridViewTextBoxColumn DepartmentCanRegular;
        private System.Windows.Forms.CheckBox CanInLand;
        private System.Windows.Forms.CheckBox CanRegular;
        private System.Windows.Forms.CheckBox CanEmergency;
        private System.Windows.Forms.CheckBox CanBoundary;
    }
}