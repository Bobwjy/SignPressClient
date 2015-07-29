using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SignPressClient.Model;
using SignPressClient.SignSocket;
using SignPressClient.SignLogging;

namespace SignPressClient
{
    public partial class Login : Form
    {
        private bool isMouseDown = false;
        private Point FormLocation;     //form的location
        private Point mouseOffset;      //鼠标的按下位置
        private OpaqueCommand cmd = new OpaqueCommand();
        public SignSocketClient _sc;

        public Login()
        {
            InitializeComponent();
        }

        private void UserName_TextChanged(object sender, EventArgs e)
        {
            this.label2.Visible = false;
        }

        private void PassWord_TextChanged(object sender, EventArgs e)
        {
            this.label3.Visible = false;
        }

        private void UserName_Validated(object sender, EventArgs e)       //用户名输入框失去焦点的时候发生
        {
            if (this.UserName.Text.Trim() == "")
            {
                this.label2.Visible = true;
            }
            else
            {
                this.label2.Visible = false;
            }
        }

        private void PassWord_Validated(object sender, EventArgs e)       //密码输入框时候失去焦点的时候发生
        {
            if (this.PassWord.Text.Trim() == "")
            {
                this.label3.Visible = true;
            }
            else
            {
                this.label3.Visible = false;
            }
        }

        #region  窗口移动
        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                FormLocation = this.Location;
                mouseOffset = Control.MousePosition;
                this.errormessage.Hide(this.UserName);
            }
        }

        private void Login_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void Login_MouseMove(object sender, MouseEventArgs e)
        {
            int _x = 0;
            int _y = 0;
            if (isMouseDown)
            {
                Point pt = Control.MousePosition;
                _x = mouseOffset.X - pt.X;
                _y = mouseOffset.Y - pt.Y;

                this.Location = new Point(FormLocation.X - _x, FormLocation.Y - _y);
            }
        }
        #endregion

        private void Close_Click(object sender, EventArgs e)       //关闭窗口
        {
            if (_sc == null)
            {
                this.Close();
            }
            else
            {
                _sc.Close();
                this.Close();
            }
        }

        private void min_Click(object sender, EventArgs e)     //最小化窗口
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Settings_Click(object sender, EventArgs e)  //设置窗口
        {
            Setting s = new Setting();
            s.StartPosition = FormStartPosition.CenterParent;
            s.ShowDialog();
        }

        #region 关闭按钮的效果显示
        private void Close_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            pb.BackColor = Color.Red;
        }

        private void Close_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            pb.BackColor = Color.Transparent;

            this.cuemessage.Hide(this.Close);
        }

        private void Close_MouseEnter(object sender, EventArgs e)
        {
            this.cuemessage.Show("关闭",this.Close,this.Close.Width,this.Close.Height);
        }
        #endregion

        #region 最小化按钮的效果显示
        private void min_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            pb.BackColor = Color.Snow;
        }

        private void min_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            pb.BackColor = Color.Transparent;

            this.cuemessage.Hide(this.min);
        }

        private void min_MouseEnter(object sender, EventArgs e)
        {
            this.cuemessage.Show("最小化", this.min, this.min.Width, this.min.Height);
        }
        #endregion

        #region 设置按钮的效果显示
        private void Settings_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            pb.BackColor = Color.Snow;
        }

        private void Settings_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            pb.BackColor = Color.Transparent;

            this.cuemessage.Hide(this.Settings);
        }

        private void Settings_MouseEnter(object sender, EventArgs e)
        {
            this.cuemessage.Show("设置", this.Settings, this.Settings.Width, this.Settings.Height);
        }
        #endregion

        private async void Submit_Click(object sender, EventArgs e)                       //登陆验证
        {
            string username = this.UserName.Text.Trim();
            string password = this.PassWord.Text.Trim();

            if (username == "")
            {
                errormessage.IsBalloon = true;
                errormessage.SetToolTip(this.UserName, "请填写用户名后再登录");
                errormessage.Show("请填写用户名后再登录", this.UserName, 1, this.UserName.Height / 2, 2000);
                errormessage.UseFading = false;
                UserName.Focus();
                return;
            }
            else if (password == "")
            {
                errormessage.IsBalloon = true;
                errormessage.SetToolTip(this.PassWord, "请填写密码后再登录");
                errormessage.Show("请填写密码后再登录", this.PassWord, 1, this.PassWord.Height / 2, 2000);
                errormessage.UseFading = false;
                PassWord.Focus();
                return;
            }
            else
            {
                try
                {
                    _sc = new SignSocketClient();
                    cmd.ShowOpaqueLayer(AllForm, 125, true, true, "正在登录");
                    Employee emp = new Employee();

                    emp = await _sc.Login(username, password);

                    if (emp != null)
                    {
                        UserHelper.UserInfo = emp;
                        this.DialogResult = DialogResult.OK;
                        cmd.HideOpaqueLayer();
                        this.Close();
                        
                        Logging.AddLog("用户:" + emp.Name + "登陆成功!");
                    }
                    else
                    {
                        cmd.HideOpaqueLayer();
                        PassWord.Text = "";
                        errormessage.IsBalloon = true;
                        errormessage.SetToolTip(this.PassWord, "用户名与密码不匹配");
                        errormessage.Show("用户名与密码不匹配", this.PassWord, 1, this.PassWord.Height / 2, 2000);
                        errormessage.UseFading = false;
                        PassWord.Focus();
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("无法连接服务器", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.Blue;
            this.TransparencyKey = Color.Blue;
        }
    }  
}
