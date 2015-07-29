namespace SignPressClient
{
    partial class SignManage
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
            this.Sign = new System.Windows.Forms.TabControl();
            this.TodoList = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ToDoListView = new System.Windows.Forms.DataGridView();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectName = new System.Windows.Forms.DataGridViewLinkColumn();
            this.ProjectName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubmitPerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubmitDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.RefreshPendinglist = new System.Windows.Forms.Button();
            this.DoneList = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DoneListView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewLinkColumn1 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.ProjectName2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.submitemployeename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SignData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SignResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SignRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel17 = new System.Windows.Forms.Panel();
            this.Downloadable = new System.Windows.Forms.CheckBox();
            this.ProgramName = new System.Windows.Forms.TextBox();
            this.ContractID = new System.Windows.Forms.TextBox();
            this.Program = new System.Windows.Forms.Label();
            this.Contrac = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.AgreeEndDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.AgreeStartDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.Sign.SuspendLayout();
            this.TodoList.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ToDoListView)).BeginInit();
            this.panel2.SuspendLayout();
            this.DoneList.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DoneListView)).BeginInit();
            this.panel17.SuspendLayout();
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
            this.label1.Text = "签字管理";
            // 
            // Sign
            // 
            this.Sign.Controls.Add(this.TodoList);
            this.Sign.Controls.Add(this.DoneList);
            this.Sign.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Sign.Location = new System.Drawing.Point(0, 25);
            this.Sign.Name = "Sign";
            this.Sign.SelectedIndex = 0;
            this.Sign.Size = new System.Drawing.Size(779, 521);
            this.Sign.TabIndex = 1;
            // 
            // TodoList
            // 
            this.TodoList.Controls.Add(this.panel3);
            this.TodoList.Controls.Add(this.panel2);
            this.TodoList.Location = new System.Drawing.Point(4, 22);
            this.TodoList.Name = "TodoList";
            this.TodoList.Padding = new System.Windows.Forms.Padding(3);
            this.TodoList.Size = new System.Drawing.Size(771, 495);
            this.TodoList.TabIndex = 0;
            this.TodoList.Text = "待办列表";
            this.TodoList.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(765, 444);
            this.panel3.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ToDoListView);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(765, 444);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "等待办理的方案";
            // 
            // ToDoListView
            // 
            this.ToDoListView.BackgroundColor = System.Drawing.Color.White;
            this.ToDoListView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ToDoListView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ToDoListView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Number,
            this.ProjectName,
            this.ProjectName1,
            this.SubmitPerson,
            this.SubmitDate});
            this.ToDoListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToDoListView.Location = new System.Drawing.Point(3, 25);
            this.ToDoListView.Name = "ToDoListView";
            this.ToDoListView.RowTemplate.Height = 23;
            this.ToDoListView.Size = new System.Drawing.Size(759, 416);
            this.ToDoListView.TabIndex = 0;
            this.ToDoListView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ToDoListView_CellContentClick);
            // 
            // Number
            // 
            this.Number.DataPropertyName = "Id";
            this.Number.HeaderText = "方案编号";
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            this.Number.Width = 150;
            // 
            // ProjectName
            // 
            this.ProjectName.DataPropertyName = "Name";
            this.ProjectName.HeaderText = "方案名称";
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.ReadOnly = true;
            this.ProjectName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ProjectName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ProjectName.Width = 300;
            // 
            // ProjectName1
            // 
            this.ProjectName1.DataPropertyName = "ProjectName";
            this.ProjectName1.HeaderText = "工程名称";
            this.ProjectName1.Name = "ProjectName1";
            this.ProjectName1.ReadOnly = true;
            // 
            // SubmitPerson
            // 
            this.SubmitPerson.DataPropertyName = "SubmitEmployeeName";
            this.SubmitPerson.HeaderText = "提交人";
            this.SubmitPerson.Name = "SubmitPerson";
            this.SubmitPerson.ReadOnly = true;
            // 
            // SubmitDate
            // 
            this.SubmitDate.DataPropertyName = "SubmitDate";
            this.SubmitDate.HeaderText = "提交时间";
            this.SubmitDate.Name = "SubmitDate";
            this.SubmitDate.ReadOnly = true;
            this.SubmitDate.Width = 180;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.RefreshPendinglist);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 447);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(765, 45);
            this.panel2.TabIndex = 3;
            // 
            // RefreshPendinglist
            // 
            this.RefreshPendinglist.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.RefreshPendinglist.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RefreshPendinglist.Location = new System.Drawing.Point(336, 4);
            this.RefreshPendinglist.Name = "RefreshPendinglist";
            this.RefreshPendinglist.Size = new System.Drawing.Size(93, 36);
            this.RefreshPendinglist.TabIndex = 2;
            this.RefreshPendinglist.Text = "刷  新";
            this.RefreshPendinglist.UseVisualStyleBackColor = true;
            this.RefreshPendinglist.Click += new System.EventHandler(this.RefreshPendinglist_Click);
            // 
            // DoneList
            // 
            this.DoneList.Controls.Add(this.panel1);
            this.DoneList.Controls.Add(this.panel17);
            this.DoneList.Location = new System.Drawing.Point(4, 22);
            this.DoneList.Name = "DoneList";
            this.DoneList.Padding = new System.Windows.Forms.Padding(3);
            this.DoneList.Size = new System.Drawing.Size(771, 495);
            this.DoneList.TabIndex = 1;
            this.DoneList.Text = "已办列表";
            this.DoneList.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 114);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(765, 378);
            this.panel1.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DoneListView);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(765, 378);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "已经办理的方案";
            // 
            // DoneListView
            // 
            this.DoneListView.BackgroundColor = System.Drawing.Color.White;
            this.DoneListView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DoneListView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewLinkColumn1,
            this.ProjectName2,
            this.submitemployeename,
            this.dataGridViewTextBoxColumn2,
            this.SignData,
            this.SignResult,
            this.SignRemark});
            this.DoneListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DoneListView.Location = new System.Drawing.Point(3, 25);
            this.DoneListView.Name = "DoneListView";
            this.DoneListView.RowTemplate.Height = 23;
            this.DoneListView.Size = new System.Drawing.Size(759, 350);
            this.DoneListView.TabIndex = 0;
            this.DoneListView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DoneListView_CellContentClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Id";
            this.dataGridViewTextBoxColumn1.HeaderText = "方案编号";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // dataGridViewLinkColumn1
            // 
            this.dataGridViewLinkColumn1.DataPropertyName = "Name";
            this.dataGridViewLinkColumn1.HeaderText = "方案名称";
            this.dataGridViewLinkColumn1.Name = "dataGridViewLinkColumn1";
            this.dataGridViewLinkColumn1.ReadOnly = true;
            this.dataGridViewLinkColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewLinkColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewLinkColumn1.Width = 280;
            // 
            // ProjectName2
            // 
            this.ProjectName2.DataPropertyName = "ProjectName";
            this.ProjectName2.HeaderText = "工程名称";
            this.ProjectName2.Name = "ProjectName2";
            this.ProjectName2.ReadOnly = true;
            // 
            // submitemployeename
            // 
            this.submitemployeename.DataPropertyName = "SubmitEmployeeName";
            this.submitemployeename.HeaderText = "提交人";
            this.submitemployeename.Name = "submitemployeename";
            this.submitemployeename.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "SubmitDate";
            this.dataGridViewTextBoxColumn2.HeaderText = "提交时间";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 180;
            // 
            // SignData
            // 
            this.SignData.DataPropertyName = "SignDate";
            this.SignData.HeaderText = "签字日期";
            this.SignData.Name = "SignData";
            this.SignData.ReadOnly = true;
            this.SignData.Width = 180;
            // 
            // SignResult
            // 
            this.SignResult.DataPropertyName = "SignResult";
            this.SignResult.HeaderText = "签字结果";
            this.SignResult.Name = "SignResult";
            this.SignResult.ReadOnly = true;
            // 
            // SignRemark
            // 
            this.SignRemark.DataPropertyName = "SignRemark";
            this.SignRemark.HeaderText = "签字备注";
            this.SignRemark.Name = "SignRemark";
            this.SignRemark.ReadOnly = true;
            this.SignRemark.Width = 220;
            // 
            // panel17
            // 
            this.panel17.Controls.Add(this.Downloadable);
            this.panel17.Controls.Add(this.ProgramName);
            this.panel17.Controls.Add(this.ContractID);
            this.panel17.Controls.Add(this.Program);
            this.panel17.Controls.Add(this.Contrac);
            this.panel17.Controls.Add(this.button3);
            this.panel17.Controls.Add(this.AgreeEndDate);
            this.panel17.Controls.Add(this.label5);
            this.panel17.Controls.Add(this.AgreeStartDate);
            this.panel17.Controls.Add(this.label6);
            this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel17.Location = new System.Drawing.Point(3, 3);
            this.panel17.Margin = new System.Windows.Forms.Padding(0);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(765, 111);
            this.panel17.TabIndex = 4;
            // 
            // Downloadable
            // 
            this.Downloadable.AutoSize = true;
            this.Downloadable.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Downloadable.Location = new System.Drawing.Point(496, 49);
            this.Downloadable.Name = "Downloadable";
            this.Downloadable.Size = new System.Drawing.Size(125, 25);
            this.Downloadable.TabIndex = 9;
            this.Downloadable.Text = "可下载的签单";
            this.Downloadable.UseVisualStyleBackColor = true;
            // 
            // ProgramName
            // 
            this.ProgramName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ProgramName.Location = new System.Drawing.Point(377, 9);
            this.ProgramName.Name = "ProgramName";
            this.ProgramName.Size = new System.Drawing.Size(158, 29);
            this.ProgramName.TabIndex = 8;
            // 
            // ContractID
            // 
            this.ContractID.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ContractID.Location = new System.Drawing.Point(103, 9);
            this.ContractID.Name = "ContractID";
            this.ContractID.Size = new System.Drawing.Size(158, 29);
            this.ContractID.TabIndex = 7;
            // 
            // Program
            // 
            this.Program.AutoSize = true;
            this.Program.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Program.Location = new System.Drawing.Point(291, 15);
            this.Program.Name = "Program";
            this.Program.Size = new System.Drawing.Size(90, 21);
            this.Program.TabIndex = 6;
            this.Program.Text = "工程名称：";
            // 
            // Contrac
            // 
            this.Contrac.AutoSize = true;
            this.Contrac.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Contrac.Location = new System.Drawing.Point(16, 15);
            this.Contrac.Name = "Contrac";
            this.Contrac.Size = new System.Drawing.Size(90, 21);
            this.Contrac.TabIndex = 5;
            this.Contrac.Text = "方案编号：";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.Location = new System.Drawing.Point(654, 73);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 32);
            this.button3.TabIndex = 4;
            this.button3.Text = "搜   索";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // AgreeEndDate
            // 
            this.AgreeEndDate.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AgreeEndDate.Location = new System.Drawing.Point(295, 49);
            this.AgreeEndDate.Name = "AgreeEndDate";
            this.AgreeEndDate.Size = new System.Drawing.Size(158, 25);
            this.AgreeEndDate.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(267, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 19);
            this.label5.TabIndex = 2;
            this.label5.Text = "至";
            // 
            // AgreeStartDate
            // 
            this.AgreeStartDate.CalendarFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AgreeStartDate.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AgreeStartDate.Location = new System.Drawing.Point(103, 50);
            this.AgreeStartDate.Name = "AgreeStartDate";
            this.AgreeStartDate.Size = new System.Drawing.Size(158, 25);
            this.AgreeStartDate.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(16, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 21);
            this.label6.TabIndex = 0;
            this.label6.Text = "查询范围：";
            // 
            // SignManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(779, 546);
            this.Controls.Add(this.Sign);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(202, 0);
            this.Name = "SignManage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "                ";
            this.Load += new System.EventHandler(this.SignManage_Load);
            this.Sign.ResumeLayout(false);
            this.TodoList.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ToDoListView)).EndInit();
            this.panel2.ResumeLayout(false);
            this.DoneList.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DoneListView)).EndInit();
            this.panel17.ResumeLayout(false);
            this.panel17.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl Sign;
        private System.Windows.Forms.TabPage TodoList;
        private System.Windows.Forms.TabPage DoneList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView DoneListView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.TextBox ProgramName;
        private System.Windows.Forms.TextBox ContractID;
        private System.Windows.Forms.Label Program;
        private System.Windows.Forms.Label Contrac;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DateTimePicker AgreeEndDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker AgreeStartDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewLinkColumn dataGridViewLinkColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectName2;
        private System.Windows.Forms.DataGridViewTextBoxColumn submitemployeename;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn SignData;
        private System.Windows.Forms.DataGridViewTextBoxColumn SignResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn SignRemark;
        private System.Windows.Forms.CheckBox Downloadable;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button RefreshPendinglist;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.DataGridView ToDoListView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewLinkColumn ProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectName1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubmitPerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubmitDate;
    }
}