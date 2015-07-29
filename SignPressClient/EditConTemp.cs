using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SignPressClient.Model;
using SignPressClient.SignSocket;
using SignPressClient.SignLogging;

namespace SignPressClient
{
    public partial class EditConTemp : Form
    {
        int Id;
        SignSocketClient _sc;

        public EditConTemp()
        {
            InitializeComponent();
        }

        public EditConTemp(int ConTempId, SignSocketClient sc)
            : this()
        {
            Id = ConTempId;
            _sc = sc;
        }

        private  void EditConTemp_Load(object sender, EventArgs e)
        {
            BindDepartment(UserHelper.DepList);               //绑定部门信息
            try
            {
                Templete temp = new Templete();
                temp =  _sc.GetContractTemplate(Id);

                this.ConTempName.Text = temp.Name.ToString();
                this.Column1.Text = temp.ColumnNames[0].ToString();
                this.Column2.Text = temp.ColumnNames[1].ToString();
                this.Column3.Text = temp.ColumnNames[2].ToString();
                this.Column4.Text = temp.ColumnNames[3].ToString();
                this.Column5.Text = temp.ColumnNames[4].ToString();

                this.Sign1.Text = temp.SignDatas[0].SignInfo.ToString();
                this.Sign2.Text = temp.SignDatas[1].SignInfo.ToString();
                this.Sign3.Text = temp.SignDatas[2].SignInfo.ToString();
                this.Sign4.Text = temp.SignDatas[3].SignInfo.ToString();
                this.Sign5.Text = temp.SignDatas[4].SignInfo.ToString();
                this.Sign6.Text = temp.SignDatas[5].SignInfo.ToString();
                this.Sign7.Text = temp.SignDatas[6].SignInfo.ToString();
                this.Sign8.Text = temp.SignDatas[7].SignInfo.ToString();

                this.SignDep1.SelectedValue = temp.SignDatas[0].SignEmployee.Department.Id;
                this.SignDep2.SelectedValue = temp.SignDatas[1].SignEmployee.Department.Id;
                this.SignDep3.SelectedValue = temp.SignDatas[2].SignEmployee.Department.Id;
                this.SignDep4.SelectedValue = temp.SignDatas[3].SignEmployee.Department.Id;
                this.SignDep5.SelectedValue = temp.SignDatas[4].SignEmployee.Department.Id;
                this.SignDep6.SelectedValue = temp.SignDatas[5].SignEmployee.Department.Id;
                this.SignDep7.SelectedValue = temp.SignDatas[6].SignEmployee.Department.Id;
                this.SignDep8.SelectedValue = temp.SignDatas[7].SignEmployee.Department.Id;

                this.SignPer1.SelectedValue = temp.SignDatas[0].SignEmployee.Id;
                this.SignPer2.SelectedValue = temp.SignDatas[1].SignEmployee.Id;
                this.SignPer3.SelectedValue = temp.SignDatas[2].SignEmployee.Id;
                this.SignPer4.SelectedValue = temp.SignDatas[3].SignEmployee.Id;
                this.SignPer5.SelectedValue = temp.SignDatas[4].SignEmployee.Id;
                this.SignPer6.SelectedValue = temp.SignDatas[5].SignEmployee.Id;
                this.SignPer7.SelectedValue = temp.SignDatas[6].SignEmployee.Id;
                this.SignPer8.SelectedValue = temp.SignDatas[7].SignEmployee.Id;

                this.Sign1Level.SelectedItem = temp.SignDatas[0].SignLevel.ToString();
                this.Sign2Level.SelectedItem = temp.SignDatas[1].SignLevel.ToString();
                this.Sign3Level.SelectedItem = temp.SignDatas[2].SignLevel.ToString();
                this.Sign4Level.SelectedItem = temp.SignDatas[3].SignLevel.ToString();
                this.Sign5Level.SelectedItem = temp.SignDatas[4].SignLevel.ToString();
                this.Sign6Level.SelectedItem = temp.SignDatas[5].SignLevel.ToString();
                this.Sign7Level.SelectedItem = temp.SignDatas[6].SignLevel.ToString();
                this.Sign8Level.SelectedItem = temp.SignDatas[7].SignLevel.ToString();

                this.View1.SelectedIndex = temp.SignDatas[0].CanView;
                this.View2.SelectedIndex = temp.SignDatas[1].CanView;
                this.View3.SelectedIndex = temp.SignDatas[2].CanView;
                this.View4.SelectedIndex = temp.SignDatas[3].CanView;
                this.View5.SelectedIndex = temp.SignDatas[4].CanView;
                this.View6.SelectedIndex = temp.SignDatas[5].CanView;
                this.View7.SelectedIndex = temp.SignDatas[6].CanView;
                this.View8.SelectedIndex = temp.SignDatas[7].CanView;

                this.Download1.SelectedIndex = temp.SignDatas[0].CanDownload;
                this.Download2.SelectedIndex = temp.SignDatas[1].CanDownload;
                this.Download3.SelectedIndex = temp.SignDatas[2].CanDownload;
                this.Download4.SelectedIndex = temp.SignDatas[3].CanDownload;
                this.Download5.SelectedIndex = temp.SignDatas[4].CanDownload;
                this.Download6.SelectedIndex = temp.SignDatas[5].CanDownload;
                this.Download7.SelectedIndex = temp.SignDatas[6].CanDownload;
                this.Download8.SelectedIndex = temp.SignDatas[7].CanDownload;

            }
            catch
            {
                MessageBox.Show("加载数据失败!");
                Logging.AddLog("修改模板时加载模板信息失败!");

                // 重启应用程序
                if (MessageBox.Show("好吧,我们的程序出现点问题需要重新启动\n点击\"确定\"重启\n点击\"取消\"退出程序？",
                                    "提示",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    Application.Restart();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void BindDepartment(List<Department> list)
        {
            if (list != null)
            {
                this.SignDep1.ValueMember = "Id";
                this.SignDep1.DisplayMember = "Name";
                this.SignDep1.DataSource = new List<Department>(list);

                this.SignDep2.ValueMember = "Id";
                this.SignDep2.DisplayMember = "Name";
                this.SignDep2.DataSource = new List<Department>(list);

                this.SignDep3.ValueMember = "Id";
                this.SignDep3.DisplayMember = "Name";
                this.SignDep3.DataSource = new List<Department>(list);

                this.SignDep4.ValueMember = "Id";
                this.SignDep4.DisplayMember = "Name";
                this.SignDep4.DataSource = new List<Department>(list);

                this.SignDep5.ValueMember = "Id";
                this.SignDep5.DisplayMember = "Name";
                this.SignDep5.DataSource = new List<Department>(list);

                this.SignDep6.ValueMember = "Id";
                this.SignDep6.DisplayMember = "Name";
                this.SignDep6.DataSource = new List<Department>(list);

                this.SignDep7.ValueMember = "Id";
                this.SignDep7.DisplayMember = "Name";
                this.SignDep7.DataSource = new List<Department>(list);

                this.SignDep8.ValueMember = "Id";
                this.SignDep8.DisplayMember = "Name";
                this.SignDep8.DataSource = new List<Department>(list);
            }
        }

        private List<Employee> BindEmployee(int departmentID)            //绑定员工信息
        {
            List<Employee> emp = new List<Employee>();
            emp = UserHelper.EmpList.Where(o => o.Department.Id == departmentID).ToList();

            return emp;
        }

        private void BindEmployeeControl(ComboBox comboBox, List<Employee> list)     //绑定控件人员信息
        {
            comboBox.DataSource = list;
            comboBox.ValueMember = "Id";
            comboBox.DisplayMember = "Name";
        }

        #region 各部门控件选择值发生改变事件
        private void SignDep1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Employee> list = new List<Employee>();
            string v = this.SignDep1.SelectedValue.ToString();
            int departmentID = Convert.ToInt32(this.SignDep1.SelectedValue.ToString());
            list = BindEmployee(departmentID);
            BindEmployeeControl(this.SignPer1, list);
        }

        private void SignDep2_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Employee> list = new List<Employee>();
            int departmentID = Convert.ToInt32(this.SignDep2.SelectedValue.ToString());
            list = BindEmployee(departmentID);
            BindEmployeeControl(this.SignPer2, list);
        }

        private void SignDep3_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Employee> list = new List<Employee>();
            int departmentID = Convert.ToInt32(this.SignDep3.SelectedValue.ToString());
            list = BindEmployee(departmentID);
            BindEmployeeControl(this.SignPer3, list);
        }

        private void SignDep4_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Employee> list = new List<Employee>();
            int departmentID = Convert.ToInt32(this.SignDep4.SelectedValue.ToString());
            list = BindEmployee(departmentID);
            BindEmployeeControl(this.SignPer4, list);
        }

        private void SignDep5_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Employee> list = new List<Employee>();
            int departmentID = Convert.ToInt32(this.SignDep5.SelectedValue.ToString());
            list = BindEmployee(departmentID);
            BindEmployeeControl(this.SignPer5, list);
        }

        private void SignDep6_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Employee> list = new List<Employee>();
            int departmentID = Convert.ToInt32(this.SignDep6.SelectedValue.ToString());
            list = BindEmployee(departmentID);
            BindEmployeeControl(this.SignPer6, list);
        }

        private void SignDep7_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Employee> list = new List<Employee>();
            int departmentID = Convert.ToInt32(this.SignDep7.SelectedValue.ToString());
            list = BindEmployee(departmentID);
            BindEmployeeControl(this.SignPer7, list);
        }

        private void SignDep8_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Employee> list = new List<Employee>();
            int departmentID = Convert.ToInt32(this.SignDep8.SelectedValue.ToString());
            list = BindEmployee(departmentID);
            BindEmployeeControl(this.SignPer8, list);
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.CheckIntegrity() == false)
            {
                return;
            }

            Templete temp = new Templete();
            SignatureTemplate signtemp1 = new SignatureTemplate();
            SignatureTemplate signtemp2 = new SignatureTemplate();
            SignatureTemplate signtemp3 = new SignatureTemplate();
            SignatureTemplate signtemp4 = new SignatureTemplate();
            SignatureTemplate signtemp5 = new SignatureTemplate();
            SignatureTemplate signtemp6 = new SignatureTemplate();
            SignatureTemplate signtemp7 = new SignatureTemplate();
            SignatureTemplate signtemp8 = new SignatureTemplate();
            List<string> list = new List<string>();
            List<SignatureTemplate> tlist = new List<SignatureTemplate>();

            temp.Name = this.ConTempName.Text.ToString();
            list.Add(this.Column1.Text.ToString());
            list.Add(this.Column2.Text.ToString());
            list.Add(this.Column3.Text.ToString());
            list.Add(this.Column4.Text.ToString());
            list.Add(this.Column5.Text.ToString());
            temp.ColumnNames = list;

            signtemp1.SignInfo = this.Sign1.Text.ToString();
            Employee emp1 = new Employee();
            emp1.Name = this.SignPer1.SelectedText;
            emp1.Id = Convert.ToInt32(this.SignPer1.SelectedValue.ToString());
            Department dep1 = new Department();
            dep1.Id = Convert.ToInt32(this.SignDep1.SelectedValue.ToString());
            dep1.Name = this.SignDep1.SelectedText;
            emp1.Department = dep1;
            signtemp1.SignEmployee = emp1;
            signtemp1.SignLevel = Convert.ToInt32(this.Sign1Level.SelectedItem.ToString());
            signtemp1.CanView = this.View1.SelectedIndex;
            signtemp1.CanDownload = this.Download1.SelectedIndex;
            tlist.Add(signtemp1);

            signtemp2.SignInfo = this.Sign2.Text.ToString();
            Employee emp2 = new Employee();
            emp2.Name = this.SignPer2.SelectedText;
            emp2.Id = Convert.ToInt32(this.SignPer2.SelectedValue.ToString());
            Department dep2 = new Department();
            dep2.Id = Convert.ToInt32(this.SignDep2.SelectedValue.ToString());
            dep2.Name = this.SignDep2.SelectedText;
            emp2.Department = dep2;
            signtemp2.SignEmployee = emp2;
            signtemp2.SignLevel = Convert.ToInt32(this.Sign2Level.SelectedItem.ToString());
            signtemp2.CanView = this.View2.SelectedIndex;
            signtemp2.CanDownload = this.Download2.SelectedIndex;
            tlist.Add(signtemp2);

            signtemp3.SignInfo = this.Sign3.Text.ToString();
            Employee emp3 = new Employee();
            emp3.Name = this.SignPer3.SelectedText;
            emp3.Id = Convert.ToInt32(this.SignPer3.SelectedValue.ToString());
            Department dep3 = new Department();
            dep3.Id = Convert.ToInt32(this.SignDep3.SelectedValue.ToString());
            dep3.Name = this.SignDep3.SelectedText;
            emp3.Department = dep3;
            signtemp3.SignEmployee = emp3;
            signtemp3.SignLevel = Convert.ToInt32(this.Sign3Level.SelectedItem.ToString());
            signtemp3.CanView = this.View3.SelectedIndex;
            signtemp3.CanDownload = this.Download3.SelectedIndex;
            tlist.Add(signtemp3);

            signtemp4.SignInfo = this.Sign4.Text.ToString();
            Employee emp4 = new Employee();
            emp4.Name = this.SignPer4.SelectedText;
            emp4.Id = Convert.ToInt32(this.SignPer4.SelectedValue.ToString());
            Department dep4 = new Department();
            dep4.Id = Convert.ToInt32(this.SignDep4.SelectedValue.ToString());
            dep4.Name = this.SignDep4.SelectedText;
            emp4.Department = dep4;
            signtemp4.SignEmployee = emp4;
            signtemp4.SignLevel = Convert.ToInt32(this.Sign4Level.SelectedItem.ToString());
            signtemp4.CanView = this.View4.SelectedIndex;
            signtemp4.CanDownload = this.Download4.SelectedIndex;
            tlist.Add(signtemp4);

            signtemp5.SignInfo = this.Sign5.Text.ToString();
            Employee emp5 = new Employee();
            emp5.Name = this.SignPer5.SelectedText;
            emp5.Id = Convert.ToInt32(this.SignPer5.SelectedValue.ToString());
            Department dep5 = new Department();
            dep5.Id = Convert.ToInt32(this.SignDep5.SelectedValue.ToString());
            dep5.Name = this.SignDep5.SelectedText;
            emp5.Department = dep5;
            signtemp5.SignEmployee = emp5;
            signtemp5.SignLevel = Convert.ToInt32(this.Sign5Level.SelectedItem.ToString());
            signtemp5.CanView = this.View5.SelectedIndex;
            signtemp5.CanDownload = this.Download5.SelectedIndex;
            tlist.Add(signtemp5);

            signtemp6.SignInfo = this.Sign6.Text.ToString();
            Employee emp6 = new Employee();
            emp6.Name = this.SignPer6.SelectedText;
            emp6.Id = Convert.ToInt32(this.SignPer6.SelectedValue.ToString());
            Department dep6 = new Department();
            dep6.Id = Convert.ToInt32(this.SignDep6.SelectedValue.ToString());
            dep6.Name = this.SignDep6.SelectedText;
            emp6.Department = dep6;
            signtemp6.SignEmployee = emp6;
            signtemp6.SignLevel = Convert.ToInt32(this.Sign6Level.SelectedItem.ToString());
            signtemp6.CanView = this.View6.SelectedIndex;
            signtemp6.CanDownload = this.Download6.SelectedIndex;
            tlist.Add(signtemp6);

            signtemp7.SignInfo = this.Sign7.Text.ToString();
            Employee emp7 = new Employee();
            emp7.Name = this.SignPer7.SelectedText;
            emp7.Id = Convert.ToInt32(this.SignPer7.SelectedValue.ToString());
            Department dep7 = new Department();
            dep7.Id = Convert.ToInt32(this.SignDep7.SelectedValue.ToString());
            dep7.Name = this.SignDep7.SelectedText;
            emp7.Department = dep7;
            signtemp7.SignEmployee = emp7;
            signtemp7.SignLevel = Convert.ToInt32(this.Sign7Level.SelectedItem.ToString());
            signtemp7.CanView = this.View7.SelectedIndex;
            signtemp7.CanDownload = this.Download7.SelectedIndex;
            tlist.Add(signtemp7);

            signtemp8.SignInfo = this.Sign8.Text.ToString();
            Employee emp8 = new Employee();
            emp8.Name = this.SignPer8.SelectedText;
            emp8.Id = Convert.ToInt32(this.SignPer8.SelectedValue.ToString());
            Department dep8 = new Department();
            dep8.Id = Convert.ToInt32(this.SignDep8.SelectedValue.ToString());
            dep8.Name = this.SignDep8.SelectedText;
            emp8.Department = dep8;
            signtemp8.SignEmployee = emp8;
            signtemp8.SignLevel = Convert.ToInt32(this.Sign8Level.SelectedItem.ToString());
            signtemp8.CanView = this.View8.SelectedIndex;
            signtemp8.CanDownload = this.Download8.SelectedIndex;
            tlist.Add(signtemp8);

            temp.SignDatas = tlist;
            temp.TempId = Id;

            string result = _sc.ModifyContractTemplate(temp);
            if (result == Response.MODIFY_CONTRACT_TEMPLATE_SUCCESS.ToString())
            {
                MessageBox.Show("修改模板成功!", "提示", MessageBoxButtons.OK);
                this.DialogResult = DialogResult.OK;
            }
            else if (result == "服务器连接中断")
            {
                MessageBox.Show("服务器连接中断,修改失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("修改失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 点击取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 保存为新模版
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void buttonSaveNew_Click(object sender, EventArgs e)
        {
            if (this.CheckIntegrity() == false)
            {
                return;
            }
            Templete temp = new Templete();
            SignatureTemplate signtemp1 = new SignatureTemplate();
            SignatureTemplate signtemp2 = new SignatureTemplate();
            SignatureTemplate signtemp3 = new SignatureTemplate();
            SignatureTemplate signtemp4 = new SignatureTemplate();
            SignatureTemplate signtemp5 = new SignatureTemplate();
            SignatureTemplate signtemp6 = new SignatureTemplate();
            SignatureTemplate signtemp7 = new SignatureTemplate();
            SignatureTemplate signtemp8 = new SignatureTemplate();
            List<string> list = new List<string>();
            List<SignatureTemplate> tlist = new List<SignatureTemplate>();

            temp.Name = this.ConTempName.Text.ToString();
            list.Add(this.Column1.Text.ToString());
            list.Add(this.Column2.Text.ToString());
            list.Add(this.Column3.Text.ToString());
            list.Add(this.Column4.Text.ToString());
            list.Add(this.Column5.Text.ToString());
            temp.ColumnNames = list;

            signtemp1.SignInfo = this.Sign1.Text.ToString();
            Employee emp1 = new Employee();
            emp1.Name = this.SignPer1.SelectedText;
            emp1.Id = Convert.ToInt32(this.SignPer1.SelectedValue.ToString());
            Department dep1 = new Department();
            dep1.Id = Convert.ToInt32(this.SignDep1.SelectedValue.ToString());
            dep1.Name = this.SignDep1.SelectedText;
            emp1.Department = dep1;
            signtemp1.SignEmployee = emp1;
            signtemp1.SignLevel = Convert.ToInt32(this.Sign1Level.SelectedItem.ToString());
            tlist.Add(signtemp1);

            signtemp2.SignInfo = this.Sign2.Text.ToString();
            Employee emp2 = new Employee();
            emp2.Name = this.SignPer2.SelectedText;
            emp2.Id = Convert.ToInt32(this.SignPer2.SelectedValue.ToString());
            Department dep2 = new Department();
            dep2.Id = Convert.ToInt32(this.SignDep2.SelectedValue.ToString());
            dep2.Name = this.SignDep2.SelectedText;
            emp2.Department = dep2;
            signtemp2.SignEmployee = emp2;
            signtemp2.SignLevel = Convert.ToInt32(this.Sign2Level.SelectedItem.ToString());
            tlist.Add(signtemp2);

            signtemp3.SignInfo = this.Sign3.Text.ToString();
            Employee emp3 = new Employee();
            emp3.Name = this.SignPer3.SelectedText;
            emp3.Id = Convert.ToInt32(this.SignPer3.SelectedValue.ToString());
            Department dep3 = new Department();
            dep3.Id = Convert.ToInt32(this.SignDep3.SelectedValue.ToString());
            dep3.Name = this.SignDep3.SelectedText;
            emp3.Department = dep3;
            signtemp3.SignEmployee = emp3;
            signtemp3.SignLevel = Convert.ToInt32(this.Sign3Level.SelectedItem.ToString());
            tlist.Add(signtemp3);

            signtemp4.SignInfo = this.Sign4.Text.ToString();
            Employee emp4 = new Employee();
            emp4.Name = this.SignPer4.SelectedText;
            emp4.Id = Convert.ToInt32(this.SignPer4.SelectedValue.ToString());
            Department dep4 = new Department();
            dep4.Id = Convert.ToInt32(this.SignDep4.SelectedValue.ToString());
            dep4.Name = this.SignDep4.SelectedText;
            emp4.Department = dep4;
            signtemp4.SignEmployee = emp4;
            signtemp4.SignLevel = Convert.ToInt32(this.Sign4Level.SelectedItem.ToString());
            tlist.Add(signtemp4);

            signtemp5.SignInfo = this.Sign5.Text.ToString();
            Employee emp5 = new Employee();
            emp5.Name = this.SignPer5.SelectedText;
            emp5.Id = Convert.ToInt32(this.SignPer5.SelectedValue.ToString());
            Department dep5 = new Department();
            dep5.Id = Convert.ToInt32(this.SignDep5.SelectedValue.ToString());
            dep5.Name = this.SignDep5.SelectedText;
            emp5.Department = dep5;
            signtemp5.SignEmployee = emp5;
            signtemp5.SignLevel = Convert.ToInt32(this.Sign5Level.SelectedItem.ToString());
            tlist.Add(signtemp5);

            signtemp6.SignInfo = this.Sign6.Text.ToString();
            Employee emp6 = new Employee();
            emp6.Name = this.SignPer6.SelectedText;
            emp6.Id = Convert.ToInt32(this.SignPer6.SelectedValue.ToString());
            Department dep6 = new Department();
            dep6.Id = Convert.ToInt32(this.SignDep6.SelectedValue.ToString());
            dep6.Name = this.SignDep6.SelectedText;
            emp6.Department = dep6;
            signtemp6.SignEmployee = emp6;
            signtemp6.SignLevel = Convert.ToInt32(this.Sign6Level.SelectedItem.ToString());
            tlist.Add(signtemp6);

            signtemp7.SignInfo = this.Sign7.Text.ToString();
            Employee emp7 = new Employee();
            emp7.Name = this.SignPer7.SelectedText;
            emp7.Id = Convert.ToInt32(this.SignPer7.SelectedValue.ToString());
            Department dep7 = new Department();
            dep7.Id = Convert.ToInt32(this.SignDep4.SelectedValue.ToString());
            dep7.Name = this.SignDep7.SelectedText;
            emp7.Department = dep7;
            signtemp7.SignEmployee = emp7;
            signtemp7.SignLevel = Convert.ToInt32(this.Sign7Level.SelectedItem.ToString());
            tlist.Add(signtemp7);

            signtemp8.SignInfo = this.Sign8.Text.ToString();
            Employee emp8 = new Employee();
            emp8.Name = this.SignPer8.SelectedText;
            emp8.Id = Convert.ToInt32(this.SignPer8.SelectedValue.ToString());
            Department dep8 = new Department();
            dep8.Id = Convert.ToInt32(this.SignDep8.SelectedValue.ToString());
            dep8.Name = this.SignDep8.SelectedText;
            emp8.Department = dep8;
            signtemp8.SignEmployee = emp8;
            signtemp8.SignLevel = Convert.ToInt32(this.Sign8Level.SelectedItem.ToString());
            tlist.Add(signtemp8);

            temp.SignDatas = tlist;

            string result =  _sc.AddConTemplete(temp);

            if (result == Response.INSERT_CONTRACT_TEMPLATE_SUCCESS.ToString())
            {
                MessageBox.Show("保存为新模板" + this.ConTempName.Text + "成功！");
                this.DialogResult = DialogResult.OK;

            }
            else
            {
                MessageBox.Show("保存为新模板！");
            }
        }


        #region 检查输入信息的完整性和合法性
        private bool CheckIntegrity()
        {
            if (this.ConTempName.Text.ToString() == "")
            {
                MessageBox.Show("请输入会前单模版的名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 否则检查前5个栏目的信息
            if (this.Column1.Text.ToString() == ""
             || this.Column2.Text.ToString() == ""
             || this.Column3.Text.ToString() == ""
             || this.Column4.Text.ToString() == ""
             || this.Column5.Text.ToString() == "")
            {
                MessageBox.Show("请将栏目填写完整", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (this.Sign1.Text.ToString() == ""
            || this.SignPer1.SelectedValue.ToString() == ""
            || this.SignDep1.SelectedValue.ToString() == ""
            || this.Sign1Level.SelectedItem.ToString() == "请选择签字顺序")
            {
                MessageBox.Show("请将签字人1的信息填写完整", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (this.Sign2.Text.ToString() == ""
            || this.SignPer2.SelectedValue.ToString() == ""
            || this.SignDep2.SelectedValue.ToString() == ""
            || this.Sign2Level.SelectedItem.ToString() == "请选择签字顺序")
            {
                MessageBox.Show("请将签字人2的信息填写完整", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (this.Sign3.Text.ToString() == ""
            || this.SignPer3.SelectedValue.ToString() == ""
            || this.SignDep3.SelectedValue.ToString() == ""
            || this.Sign3Level.SelectedItem.ToString() == "请选择签字顺序")
            {
                MessageBox.Show("请将签字人3的信息填写完整", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (this.Sign4.Text.ToString() == ""
            || this.SignPer4.SelectedValue.ToString() == ""
            || this.SignDep4.SelectedValue.ToString() == ""
            || this.Sign4Level.SelectedItem.ToString() == "请选择签字顺序")
            {
                MessageBox.Show("请将签字人4的信息填写完整", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (this.Sign5.Text.ToString() == ""
            || this.SignPer5.SelectedValue.ToString() == ""
            || this.SignDep5.SelectedValue.ToString() == ""
            || this.Sign5Level.SelectedItem.ToString() == "请选择签字顺序")
            {
                MessageBox.Show("请将签字人5的信息填写完整", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (this.Sign6.Text.ToString() == ""
            || this.SignPer6.SelectedValue.ToString() == ""
            || this.SignDep6.SelectedValue.ToString() == ""
            || this.Sign6Level.SelectedItem.ToString() == "请选择签字顺序")
            {
                MessageBox.Show("请将签字人2的信息填写完整", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.Sign7.Text.ToString() == ""
            || this.SignPer7.SelectedValue.ToString() == ""
            || this.SignDep7.SelectedValue.ToString() == ""
            || this.Sign7Level.SelectedItem.ToString() == "请选择签字顺序")
            {
                MessageBox.Show("请将签字人7的信息填写完整", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (this.Sign8.Text.ToString() == ""
            || this.SignPer8.SelectedValue.ToString() == ""
            || this.SignDep8.SelectedValue.ToString() == ""
            || this.Sign8Level.SelectedItem.ToString() == "请选择签字顺序")
            {
                MessageBox.Show("请将签字人8的信息填写完整", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;

        }
        #endregion
    }
}
