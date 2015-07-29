using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;



using SignPressClient.Model;
using SignPressClient.SignSocket;

namespace SignPressClient
{
    public enum ModifyEmployeeType
    {
        USER_MODIFY,            //  用户个人修改
        ADMIN_MODIFY,           //  管理员进行修改
    }

    public partial class EditEmployee : Form
    {

        ModifyEmployeeType m_type;                            //  用户类型
        private bool m_isRepicture;         //  是否重新上传签字图片
        private int m_id;                   //  员工编号
        private Employee m_employee;
        public Employee Employee
        {
            get { return this.m_employee; }
            set { this.m_employee = value; }
        }

        private SignSocketClient _sc;


        public EditEmployee()
        {
            InitializeComponent();
        }

        public EditEmployee(Employee employee, SignSocketClient sc)
        :this()
        {
            this.m_employee = employee;
            this.m_id = -1;
            this._sc = sc;
            
            this.m_isRepicture = false;         //  初始化不修改签字图片
        }

        public EditEmployee(int employeeId, SignSocketClient sc)
            : this()
        {
            this.m_id = employeeId;
            this.m_employee = null;
            this._sc = sc;

            this.m_isRepicture = false;
        }
        public EditEmployee(int employeeId, SignSocketClient sc, ModifyEmployeeType type)
            : this()
        {
            this.m_id = employeeId;
            this.m_employee = null;
            this._sc = sc;

            this.m_isRepicture = false;
            this.m_type = type;         //  用户修改自己的信息和管理员修改时看到的不一样
        }


        public  void BindDepartmentDataSourece()              //异步获取部门列表数据并绑定
        {
            //List<Department> list = new List<Department>();
            //list = await _sc.QueryDepartment();

            List<Department> depList = _sc.QueryDepartment();

            if (depList != null)
            {
                UserHelper.DepList = depList;
                this.comboBoxDepartment.DataSource = depList;

                this.comboBoxDepartment.ValueMember = "Id";
                this.comboBoxDepartment.DisplayMember = "Name";
            }
        }

        private  async void EditEmployee_Load(object sender, EventArgs e)
        {
            // 首先获取员工的信息
            if (this.m_employee == null)
            {
                //this.m_employee = _sc.
            }

            // 绑定数据源

            this.textBoxName.Text = this.m_employee.Name;

            //窗体加载时位datagridview绑定数据源
            if (UserHelper.DepList == null)                   //绑定部门信息
            {
                BindDepartmentDataSourece();
            }
            else
            {
                this.comboBoxDepartment.DataSource = UserHelper.DepList;

                this.comboBoxDepartment.ValueMember = "Id";
                this.comboBoxDepartment.DisplayMember = "Name";
            }
            this.textBoxPosition.Text = this.m_employee.Position;
            this.textBoxUserName.Text = this.m_employee.User.Username;

            if (this.m_type == ModifyEmployeeType.USER_MODIFY)  // 用户修改自己的信息
            { 
                
            }
            //this.textBoxPassword.Text = this.m_employee.User.Password;
            if (this.m_employee.CanSign == 1)
            {
                this.CanSign.Checked = true;
                // 下面处理签字图片的信息
                //String picturePath = "./signature/" + this.m_employee.Id + ".jpg";
                String picturePath = UserHelper.DEFAULT_SIGNATURE_PATH + this.m_employee.Id + ".jpg";
                if (File.Exists((String)picturePath))     // 首先检测文件是否存在
                {
                    this.PictureBox.ImageLocation = picturePath;
                    this.PictureBox.Visible = true;
                    this.UploadButton.Visible = true;

                }
                else  //  否则签字图片不存在，则提示用户使用加载
                {
                    if (MessageBox.Show("请您您是否希望加载签字图片的信息?\n由于签字显示图片较大，下载数据可能会造成部分网络延迟", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        await this._sc.DownLoadPicture(m_employee.Id.ToString(), picturePath);
                        this.PictureBox.ImageLocation = picturePath;
                        this.PictureBox.Visible = true;
                    }
                }
            }
            else
            {
                this.CanSign.Checked = false;
                this.PictureBox.Visible = false;
                this.UploadButton.Visible = false;
            }
            if (this.m_employee.CanSubmit == 1)
            {
                this.CanSubmit.Checked = true;
            }
            else
            {
                this.CanSubmit.Checked = false;
            }
            if (this.m_employee.IsAdmin == 1)
            {
                this.CanAdmin.Checked = true;
            }
            else
            {
                this.CanAdmin.Checked = false;
            }

        }

        private async void buttonModify_Click(object sender, EventArgs e)
        {
            //  首先进行容错
            if (this.textBoxName.Text.Trim() == ""
            || this.textBoxPosition.Text.Trim() == "")            {
                MessageBox.Show("请将信息填写完整！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (this.CanSign.Checked
            && this.PictureBox.ImageLocation == null)
            //if (this.m_isRepicture == false)
            {
                MessageBox.Show("签字人需要提交签字图片！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //if (UserHelper.EmpList.Where(o => o. == this.textBoxName.Text).ToList().Count > 0)
            //{
            //    MessageBox.Show("员工信息已经存在，无法再次插入\n请检查用户名是否与他人重复！！！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            try
            {
                this.m_employee.Name = this.textBoxName.Text;

                this.m_employee.Department.Id = Convert.ToInt32(this.comboBoxDepartment.SelectedValue);
                this.m_employee.Department.Name = this.comboBoxDepartment.SelectedItem.ToString();

                this.m_employee.Position = this.textBoxPosition.Text;
                if (this.CanSubmit.Checked)
                {
                    this.m_employee.CanSubmit = 1;
                }
                else
                {
                    this.m_employee.CanSubmit = 0;
                }

                if (this.CanSign.Checked)
                {
                    this.m_employee.CanSign = 1;
                    
                    this.PicturrName.Visible = true;        // 图片路径
                    this.UploadButton.Visible = true;       // 上传按钮
                    this.PictureBox.Visible = true;         // 
                }
                else
                {
                    this.m_employee.CanSign = 0;

                    this.PicturrName.Visible = false;
                    this.UploadButton.Visible = false;
                    this.PictureBox.Visible = false;         // 

                }

                if (this.CanAdmin.Checked)
                {
                    this.m_employee.IsAdmin = 1;
                }
                else
                {
                    this.m_employee.IsAdmin = 0;
                }

                string result =  _sc.ModifyEmployee(this.m_employee);
                if (result == Response.MODIFY_EMPLOYEE_SUCCESS.ToString())
                {
                    MessageBox.Show("修改员工信息成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    

                    ////////////////////////////////////////
                    // 绑定底层数据信息
                    //UserHelper.UserInfo = this.m_employee;
                    ////////////////////////////////////////

                    if (this.CanSign.Checked && this.PictureBox.ImageLocation != null && this.m_isRepicture == true)
                    {
                        await _sc.UploadPicture(this.m_employee.Id, this.PictureBox.ImageLocation);
                        MessageBox.Show("重新上传签字信息成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (result == "服务器连接中断")
                {
                    MessageBox.Show("服务器连接中断,修改员工信息失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("修改员工信息失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void CanSign_Click(object sender, EventArgs e)
        {

            if (this.CanSign.Checked)
            {
                this.PicturrName.Visible = true;        // 图片路径
                this.UploadButton.Visible = true;       // 上传按钮
                this.PictureBox.Visible = true;         // 
            }
            else
            {
                this.PicturrName.Visible = false;
                this.UploadButton.Visible = false;
                this.PictureBox.Visible = false;         // 

            }
        }

        //  重新上传签字图片
        private void UploadButton_Click(object sender, EventArgs e)
        {
            //if (this.CanSign.Checked != true)
            //{
            //    MessageBox.Show("用户没有提交权限，不能", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //}
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请上传签名图片";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filepath = ofd.FileName;
                int position = filepath.LastIndexOf("\\");
                string fileName = filepath.Substring(position + 1);
                this.PicturrName.Text = fileName;
                this.PictureBox.ImageLocation = filepath;
                this.PictureBox.Visible = true;

                this.m_isRepicture = true;          //  用户需要重新提交签字图片
            }
        }

        private void buttonCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }









    }
}
