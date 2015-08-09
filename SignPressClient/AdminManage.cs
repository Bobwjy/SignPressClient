using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SignPressClient.SignSocket;
using SignPressClient.Model;

namespace SignPressClient
{
    public partial class AdminManage : Form
    {
        SignSocketClient _sc;

        public AdminManage()
        {
            InitializeComponent();
        }

        public AdminManage(int selectedIndex, SignSocketClient sc)
            : this()
        {
            this.Admin.SelectedIndex = selectedIndex;
            _sc = sc;
        }

        private void DepartmentManage_Load(object sender, EventArgs e)            //窗体加载事件
        {
            //窗体加载时位datagridview绑定数据源
            if (UserHelper.DepList == null)                   //绑定部门信息
            {
                BindGridViewDataSourece();
            }
            else
            {
                this.dataGridView1.DataSource = UserHelper.DepList;

                this.SelectedDepartment.ValueMember = "Id";
                this.SelectedDepartment.DisplayMember = "Name";
                this.SelectedDepartment.DataSource = UserHelper.DepList;
            }

            if (UserHelper.sEmpList == null)                   //绑定人员信息
            {
                int departmentID = 0;
                BindEmployee(departmentID);
            }
            else
            {
                this.dataGridView3.AutoGenerateColumns = false;
                this.dataGridView3.DataSource = UserHelper.sEmpList;
            }

            if (UserHelper.TempList == null)                  //绑定模板信息
            {
                BindGroupViewData2Source();
            }
            else
            {
                this.dataGridView2.AutoGenerateColumns = false;
                this.dataGridView2.DataSource = UserHelper.TempList;
            }

            BindDataGridView1Column();            //部门列表添加编辑列
            BindDataGridView3Column();            //人员列表添加编辑列
            BindDataGridView2Column();            //模板列表添加编辑列
        }

        private void BindDataGridView3Column()
        {
            this.dataGridView1.AutoGenerateColumns = false;
            DataGridViewLinkColumn modify = new DataGridViewLinkColumn();
            modify.Text = "修改";
            modify.Name = "LinkModify";
            modify.HeaderText = "修改";
            modify.UseColumnTextForLinkValue = true;
            this.dataGridView3.Columns.Add(modify);

            this.dataGridView1.AutoGenerateColumns = false;
            DataGridViewLinkColumn resetPwd = new DataGridViewLinkColumn();
            resetPwd.Text = "重置密码";
            resetPwd.Name = "LinkResetPwd";
            resetPwd.HeaderText = "重置密码";
            resetPwd.UseColumnTextForLinkValue = true;
            this.dataGridView3.Columns.Add(resetPwd);

            this.dataGridView1.AutoGenerateColumns = false;
            DataGridViewLinkColumn delete = new DataGridViewLinkColumn();
            delete.Text = "删除";
            delete.Name = "LinkDelete";
            delete.HeaderText = "删除";
            delete.UseColumnTextForLinkValue = true;
            this.dataGridView3.Columns.Add(delete);
        }

        private void BindDataGridView1Column()
        {
            this.dataGridView1.AutoGenerateColumns = false;
            DataGridViewLinkColumn edit = new DataGridViewLinkColumn();
            edit.Text = "修改";
            edit.Name = "LinkEdit";
            edit.HeaderText = "修改";
            edit.UseColumnTextForLinkValue = true;
            this.dataGridView1.Columns.Add(edit);

            this.dataGridView1.AutoGenerateColumns = false;
            DataGridViewLinkColumn delete = new DataGridViewLinkColumn();
            delete.Text = "删除";
            delete.Name = "LinkDelete";
            delete.HeaderText = "删除";
            delete.UseColumnTextForLinkValue = true;
            this.dataGridView1.Columns.Add(delete);
        }

        private void BindDataGridView2Column()
        {
            //  添加修改模版信息
            this.dataGridView2.AutoGenerateColumns = false;
            DataGridViewLinkColumn edit = new DataGridViewLinkColumn();
            edit.Text = "修改";
            edit.Name = "LinkEdit";
            edit.HeaderText = "修改";
            edit.UseColumnTextForLinkValue = true;
            this.dataGridView2.Columns.Add(edit);

            //  删除修改模版信息
            this.dataGridView2.AutoGenerateColumns = false;
            DataGridViewLinkColumn delete = new DataGridViewLinkColumn();
            delete.Text = "删除";
            delete.Name = "LinkDelete";
            delete.HeaderText = "删除";
            delete.UseColumnTextForLinkValue = true;
            this.dataGridView2.Columns.Add(delete);

        }

        private void BindEmployee(int departmentID)           //绑定部门员工
        {
            //List<Employee> emp = new List<Employee>();
            //emp = await _sc.QueryEmployeeByDepartmentID(departmentID); 

            List<Employee> emp = _sc.QueryEmployeeByDepartmentID(departmentID);

            if (emp != null)
            {
                UserHelper.EmpList = emp;
            }
            List<sEmployee> semp = new List<sEmployee>();
            if (emp != null)
            {
                foreach (Employee e in emp)
                {
                    sEmployee se = new sEmployee();
                    se.Id = e.Id;
                    se.Name = e.Name;
                    se.Position = e.Position;
                    se.UserName = e.User.Username;
                    se.Department = e.Department.Name;
                    se.IsAdmin = e.IsAdmin == 0 ? "否" : "是";
                    se.CanSubmit = e.CanSubmit == 0 ? "否" : "是";
                    se.CanSign = e.CanSign == 0 ? "否" : "是";
                    semp.Add(se);
                }
                UserHelper.sEmpList = semp;
                this.dataGridView3.AutoGenerateColumns = false;
                this.dataGridView3.DataSource = semp;
            }
        }

        public void BindGridViewDataSourece()              //异步获取部门列表数据并绑定
        {
            //List<Department> list = new List<Department>();
            List<Department> list = _sc.QueryDepartment();

            if (list != null)
            {
                UserHelper.DepList = list;
                this.dataGridView1.DataSource = list;

                this.SelectedDepartment.ValueMember = "Id";
                this.SelectedDepartment.DisplayMember = "Name";
                this.SelectedDepartment.DataSource = list;
            }
        }

        private void AddDepartment_Click(object sender, EventArgs e)                  //添加部门
        {
            //  modify by gatieme at 2015-08-08 16:20
            //  为部门添加部门简称
            string departmentName = this.DepartmentName.Text.Trim();
            string departmentShortCall = this.textBoxDepartmentShortCall.Text.Trim();

            if (departmentName == "")
            {
                MessageBox.Show("请填写部门名称!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (departmentShortCall == "")
            {
                MessageBox.Show("请填写部门简称!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (UserHelper.DepList.Where(o => o.Name == departmentName).ToList().Count > 0)
            {
                MessageBox.Show("该部门的部门名称与其他部门重复!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (UserHelper.DepList.Where(o => o.ShortCall == departmentShortCall).ToList().Count > 0)
            {
                MessageBox.Show("该部门的部门简称与其他部门重复!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Department department = new Department { Id = -1, Name = departmentName, ShortCall = departmentShortCall };
            string result = _sc.InsertDepartment(department);

            if (result == Response.INSERT_DEPARTMENT_SUCCESS.ToString())
            {
                MessageBox.Show("添加" + departmentName + "部门成功!");
                
                ///////
                BindGridViewDataSourece();
                ///////
            }
            else
            {
                MessageBox.Show("添加" + departmentName + "部门失败!");
            }

        }

        /// <summary>
        /// 添加新会签单模版的按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTemp_Click(object sender, EventArgs e)
        {
            AddConTemp add = new AddConTemp(_sc);
            add.ShowDialog();
            BindGroupViewData2Source();
        }

        private void Admin_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void BindGroupViewData2Source()                        //获取模板信息
        {
            List<Templete> list = new List<Templete>();
            list = _sc.QueryContractTemplate();

            if (list != null)
            {
                UserHelper.TempList = list;
                this.dataGridView2.AutoGenerateColumns = false;
                this.dataGridView2.DataSource = list;
            }
        }

        private async void button1_Click(object sender, EventArgs e)                //添加人员
        {
            //  首先进行容错
            if (this.Position.Text.Trim() == ""
            || this.UserName.Text.Trim() == ""
            || this.UserPassword.Text.Trim() == "")
            {
                MessageBox.Show("请将信息填写完整！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (this.CanSign.Checked
            && this.pictureBox1.ImageLocation == null)
            {
                MessageBox.Show("签字人需要提交签字图片！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (UserHelper.EmpList.Where(o => o.User.Username == this.UserName.Text.Trim()).ToList().Count > 0)
            {
                MessageBox.Show("员工信息已经存在，无法再次插入\n请检查用户名是否与他人重复！！！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Employee emp = new Employee();
            emp.Name = this.eName.Text.Trim();

            Department d = new Department();
            d.Id = Convert.ToInt32(this.SelectedDepartment.SelectedValue);
            d.Name = this.SelectedDepartment.SelectedItem.ToString();
            emp.Department = d;

            emp.Position = this.Position.Text.Trim();
            User u = new User();
            u.Username = this.UserName.Text.Trim();
            u.Password = this.UserPassword.Text.Trim();
            emp.User = u;

            if (this.CanSubmit.Checked)
            {
                emp.CanSubmit = 1;
            }
            else
            {
                emp.CanSubmit = 0;
            }

            if (this.CanSign.Checked)
            {
                emp.CanSign = 1;
            }
            else
            {
                emp.CanSign = 0;
            }

            if (this.CanAdmin.Checked)
            {
                emp.IsAdmin = 1;
            }
            else
            {
                emp.IsAdmin = 0;
            }

            int id = _sc.InsertEmployee(emp);
            if (id == -2)
            {
                MessageBox.Show("员工信息已经存在，无法再次插入\n请检查用户名是否与他人重复！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (id == -1)
            {
                MessageBox.Show("添加人员失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (id == 0)
            {
                MessageBox.Show("服务器连接中断！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("添加人员成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (this.CanSign.Checked && this.pictureBox1.ImageLocation != null)
                {
                    await _sc.UploadPicture(id, this.pictureBox1.ImageLocation);
                }
            }
            BindEmployee(0);
        }

        /// <summary>
        /// 模版管理操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)    //模板列表点击事件
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == 1)
            {
                int Id = Convert.ToInt32(this.dataGridView2.Rows[e.RowIndex].Cells[0].Value);
                ViewConTemp vcp = new ViewConTemp(Id, _sc);
                vcp.Show();
            }
            else if (e.ColumnIndex == 3)            //  修改模版
            {
                if (MessageBox.Show("确定要修改此模板信息？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int Id = Convert.ToInt32(this.dataGridView2.Rows[e.RowIndex].Cells[0].Value);
                    EditConTemp ecp = new EditConTemp(Id, _sc);
                    ecp.ShowDialog();

                    if (ecp.DialogResult == DialogResult.OK)
                    {
                        ecp.Close();
                        BindGroupViewData2Source();
                    }
                }
            }
            else if (e.ColumnIndex == 4)
            {
                if (MessageBox.Show("确定要删除此模板信息？\n请谨慎选择\n删除模版会导致以次模版为基础的所有的会签单全部被删除请谨慎选择", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (MessageBox.Show("重复确认？您正在进行一项危险操作，请谨慎选择", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int Id = Convert.ToInt32(this.dataGridView2.Rows[e.RowIndex].Cells[0].Value);
                        string result = _sc.DeleteContractTemplate(Id);
                        if (result == Response.DELETE_CONTRACT_TEMPLATE_SUCCESS.ToString())
                        {
                            MessageBox.Show("删除模板成功!", "提示", MessageBoxButtons.OK);
                        }
                        else if (result == "服务器连接中断")
                        {
                            MessageBox.Show("服务器连接中断,删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (result == Response.DELETE_CONTRACT_TEMPLATE_EXIST_CONTRACT.ToString())
                        {
                            MessageBox.Show("此模板已经有会签单使用，无法删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("删除模板失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        BindGroupViewData2Source();
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex == 3)
            {
                if (MessageBox.Show("确定要修改当前部门信息？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int row = e.RowIndex;
                    Department department = UserHelper.DepList[row];

                    EditDepartment ed = new EditDepartment(department, _sc);
                    ed.ShowDialog();

                    if (ed.DialogResult == DialogResult.OK)
                    {
                        BindGridViewDataSourece();          //  绑定部门源

                        BindEmployee(0);                    //  绑定员工信息
                    }
                }
            }
            else if (e.ColumnIndex == 4)
            {
                if (MessageBox.Show("确定要删除此部门？\n危险操作，请谨慎进行\n由于部门下面可能有员工，因此您的删除操作会将部门下的所有员工全部被删除，由此将引入很多不安全问题，请问您是否继续删除", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int Id = Convert.ToInt32(this.dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                    string result = _sc.DeleteDepartment(Id);

                    if (result == Response.DELETE_DEPARTMENT_SUCCESS.ToString())
                    {
                        MessageBox.Show("删除部门成功!", "提示", MessageBoxButtons.OK);
                        BindGridViewDataSourece();
                        BindEmployee(0);                    //  绑定员工信息

                    }
                    else if (result == "服务器连接中断")
                    {
                        MessageBox.Show("服务器连接中断,删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (result == Response.DELETE_DEPARTMENT_EXIST_EMPLOYEE.ToString())
                    {
                        MessageBox.Show("该部门下有人员存在，无法删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("删除部门失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请上传签名图片";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filepath = ofd.FileName;
                int position = filepath.LastIndexOf("\\");
                string fileName = filepath.Substring(position + 1);
                this.PicturrName.Text = fileName;
                this.pictureBox1.ImageLocation = filepath;
                this.pictureBox1.Visible = true;
            }
        }

        private void CanSign_Click(object sender, EventArgs e)
        {
            if (this.CanSign.Checked)
            {
                this.PicturrName.Visible = true;
                this.UploadButton.Visible = true;
            }
            else
            {
                this.PicturrName.Visible = false;
                this.UploadButton.Visible = false;
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == 8)         // 修改员工
            {
                if (MessageBox.Show("确定要修改此员工信息？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //int Id = Convert.ToInt32(this.dataGridView3.Rows[e.RowIndex].Cells[0].Value);
                    //string result = await _sc.ModifyEmployee(Id);
                    int row = Convert.ToInt32(e.RowIndex);              // 获取当前的信息行
                    Employee employee = UserHelper.EmpList[row];        // 获取数据源中第row个员工的信息
                    EditEmployee editEmployee = new EditEmployee(employee, _sc);
                    editEmployee.ShowDialog();


                    if (editEmployee.DialogResult == DialogResult.OK)
                    {
                        editEmployee.Close();

                        BindEmployee(0);
                    }
                }
            }
            else if (e.ColumnIndex == 9)  // 重置密码
            {
                String Name = this.dataGridView3.Rows[e.RowIndex].Cells[1].Value.ToString();
                if (MessageBox.Show("确定重置员工" + Name + "的密码？员工密码将被重置为111", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int Id = Convert.ToInt32(this.dataGridView3.Rows[e.RowIndex].Cells[0].Value);
                    String Username = this.dataGridView3.Rows[e.RowIndex].Cells[2].Value.ToString();
                    String Password = "111";
                    User user = new User { Username = Username, Password = Password };
                    string result = _sc.ModifyEmployeePassword(user);      //  重置员工密码

                    if (result == Response.MODIFY_EMP_PWD_SUCCESS.ToString())
                    {
                        MessageBox.Show("重置密码成功!", "提示", MessageBoxButtons.OK);
                    }
                    else if (result == "服务器连接中断")
                    {
                        MessageBox.Show("服务器连接中断,重置密码失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("重置密码成功失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    BindEmployee(0);
                }
            }
            else if (e.ColumnIndex == 10)         // 删除员工
            {
                if (MessageBox.Show("确定要删除此人员？\n危险操作，谨慎进行\n由于该员工可能关联了很多会签单模版以及会签单信息，请谨慎操作", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int Id = Convert.ToInt32(this.dataGridView3.Rows[e.RowIndex].Cells[0].Value);
                    string result = _sc.DeleteEmployee(Id);

                    if (result == Response.DELETE_EMPLOYEE_SUCCESS.ToString())
                    {
                        MessageBox.Show("删除人员成功!", "提示", MessageBoxButtons.OK);
                    }
                    else if (result == "服务器连接中断")
                    {
                        MessageBox.Show("服务器连接中断,删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (result == Response.DELETE_EMPLOYEE_EXIST_CONTRACT.ToString())
                    {
                        MessageBox.Show("该人员存在会签单信息中，无法删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (result == Response.DELETE_EMPLOYEE_EXIST_CONTEMP.ToString())
                    {
                        MessageBox.Show("该人员存在会签单模版信息中，无法删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("删除人员失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    BindEmployee(0);
                }
            }
        }

    }
}
