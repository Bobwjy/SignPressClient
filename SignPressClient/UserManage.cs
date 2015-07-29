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
    public partial class UserManage : Form
    {
        private SignSocketClient _sc;


        public UserManage()
        {
            InitializeComponent();
        }

        public UserManage(int selectedIndex)
            : this()
        {
            this.Users.SelectedIndex = selectedIndex;
        }

        public UserManage(int selectedIndex, SignSocketClient sc)
            : this()
        {
            this.Users.SelectedIndex = selectedIndex;
            this._sc = sc;

            //  设置用户名text框显示用户名
            //this.textUsername.BackColor = System.Drawing.SystemColors.Control;
            //this.textUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
        
            //  显示个人信息
            this.BindEmployee();
            
        }

        public async void BindEmployee()
        {
            Employee employee =  UserHelper.UserInfo;
            this.textBoxName.Text = employee.Name;
            this.textBoxUsername.Text = employee.User.Username;
            this.textBoxDepartment.Text = employee.Department.Name;
            this.textBoxPosition.Text = employee.Position;

            if (employee.CanSign == 1)
            {
                this.labelCanSign.Visible = true;
                //this.PictureBox.Visible = true;
                this.buttonRePicture.Visible = true;


                //  加载签字图片信息
                this.PictureBox.Visible = true;

                string picturePath = UserHelper.DEFAULT_SIGNATURE_PATH + UserHelper.UserInfo.Id.ToString() + ".jpg";
                if (File.Exists((String)picturePath))     // 首先检测文件是否存在
                {
                    this.PictureBox.ImageLocation = picturePath;
                    //this.PictureBox.Visible = true;

                }
                else  //  否则签字图片不存在，则提示用户使用加载
                {
                    if (MessageBox.Show("请您您是否希望加载签字图片的信息?\n由于签字显示图片较大，下载数据可能会造成部分网络延迟", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        await this._sc.DownLoadPicture(UserHelper.UserInfo.Id.ToString(), picturePath);
                        this.PictureBox.ImageLocation = picturePath;
                      //  this.PictureBox.Visible = true;
                    }
                }
            }
            else
            {
                this.labelCanSign.Visible = false;
                this.PictureBox.Visible = false;
                this.buttonRePicture.Visible = false;
            }

            if (employee.CanSubmit == 1)
            {
                this.labelCanSubmit.Visible = true;
            }
            else
            {
                this.labelCanSubmit.Visible = false;
            }

            if (employee.IsAdmin == 1)
            {
                this.labelIsAdmin.Visible = true;
            }
            else
            {
                this.labelIsAdmin.Visible = false;
            }


        }

        private  void buttonModifyPWDConfirm_Click(object sender, EventArgs e)
        {
            String username = UserHelper.UserInfo.User.Username;
            String oldPassword = this.textBoxOldPassword.Text.Trim();
            String newPassword = this.textBoxNewPassword.Text.Trim();
            String rePassword = this.textBoxRePassword.Text.Trim();

            if (oldPassword == ""
             || newPassword == ""
             || rePassword == "")
            {
                MessageBox.Show("请填写完整信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (newPassword != rePassword)
            {
                MessageBox.Show("重复输入的新密码不同，无法修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            else if (newPassword == oldPassword)
            {
                MessageBox.Show("新密码与原密码相同，无需修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (oldPassword != UserHelper.UserInfo.User.Password)
            {

                MessageBox.Show("原密码验证失败，无法修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 用户修改密码
            User user = new User
            {
                Username = username,
                Password = newPassword
            };
            String result =  _sc.ModifyEmployeePassword(user);

            if (result == Response.MODIFY_EMP_PWD_SUCCESS.ToString())
            {
                MessageBox.Show("修改密码成功!", "提示", MessageBoxButtons.OK);
                UserHelper.UserInfo.User.Password = newPassword;
            }
            else if (result == "服务器连接中断")
            {
                MessageBox.Show("服务器连接中断,修改密码失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                MessageBox.Show("修改密码失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonModifyPWDCancle_Click(object sender, EventArgs e)
        {
            this.textBoxOldPassword.Text = "";
            this.textBoxNewPassword.Text = "";
            this.textBoxRePassword.Text = "";
        }

        private void buttonModifyEmployee_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要修改个人信息？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //int Id = Convert.ToInt32(this.dataGridView3.Rows[e.RowIndex].Cells[0].Value);
                //string result = await _sc.ModifyEmployee(Id);
                Employee employee = UserHelper.UserInfo;        // 获取数据源中第row个员工的信息
                EditEmployee editEmployee = new EditEmployee(employee, _sc);
                editEmployee.ShowDialog();


                if (editEmployee.DialogResult == DialogResult.OK)
                {
                    editEmployee.Close();

                    BindEmployee( );
                }
            }
        }


        /// <summary>
        /// 重新上传签字图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonRePicture_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请上传签名图片";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filepath = ofd.FileName;
                int position = filepath.LastIndexOf("\\");
                string fileName = filepath.Substring(position + 1);
                //this.PicturrName.Text = fileName;
                this.PictureBox.ImageLocation = filepath;
                this.PictureBox.Visible = true;

                await _sc.UploadPicture(UserHelper.UserInfo.Id, this.PictureBox.ImageLocation);

                MessageBox.Show("重新上传签字信息成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //this.m_isRepicture = true;          //  用户需要重新提交签字图片
            }
        }
    }

}
