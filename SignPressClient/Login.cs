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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


//  modify by gatieme at version 1.0.2
//  date 2015-07-30 19:01
//  登录界面添加记住密码的功能


namespace SignPressClient
{
    public partial class Login : Form
    {
        private bool isMouseDown = false;
        private Point FormLocation;     //form的location
        private Point mouseOffset;      //鼠标的按下位置
        private OpaqueCommand cmd = new OpaqueCommand();
        public SignSocketClient _sc;


        List<User> item = new List<User>();
        User user = new User();
        //BUser bUser = new BUser();
        Dictionary<string, User> users = new Dictionary<string, User>();


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
                User user = new User();
                //BUser bUser = new BUser();
                FileStream fs = new FileStream("data.bin", FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                user.Username = username;
                if (this.RemeberPassword.Checked)       //  如果单击了记住密码的功能
                {   //  在文件中保存密码
                    user.Password = password;
                }
                else
                {   //  不在文件中保存密码
                    user.Password = "";
                }
                
                //  选在集合中是否存在用户名 
                if (users.ContainsKey(user.Username))
                {
                    users.Remove(user.Username);
                }
                users.Add(user.Username, user);
                //要先将User类先设为可以序列化(即在类的前面加[Serializable])
                bf.Serialize(fs, users);
                //user.Password = this.PassWord.Text;
                fs.Close();
            
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


            //  读取配置文件寻找记住的用户名和密码
            FileStream fs = new FileStream("data.bin", FileMode.OpenOrCreate);

            if (fs.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                users = bf.Deserialize(fs) as Dictionary<string, User>;
                foreach (User user in users.Values)
                {
                    this.UserName.Items.Add(user.Username);
                }

                for (int i = 0; i < users.Count; i++)
                {
                    if (this.UserName.Text != "")
                    {
                        if (users.ContainsKey(this.UserName.Text))
                        {
                            this.PassWord.Text = users[this.UserName.Text].Password;
                            this.RemeberPassword.Checked = true;
                        }
                    }
                }
            }
            fs.Close();
            //  用户名默认选中第一个
            if (this.UserName.Items.Count > 0)
            {
                this.UserName.SelectedIndex = this.UserName.Items.Count - 1;
            }
            //item = (List<User>)bUser.GetAll();
            //item = 
            //this.UserName.DataSource = item;
            //this.UserName.DisplayMember = "Username";
        }

        private void UserName_SelectedValueChanged(object sender, EventArgs e)
        {
            //  首先读取记住密码的配置文件
            FileStream fs = new FileStream("data.bin", FileMode.OpenOrCreate);

            if (fs.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();

                users = bf.Deserialize(fs) as Dictionary<string, User>;
                
                for (int i = 0; i < users.Count; i++)
                {
                    if (this.UserName.Text != "")
                    {
                        if (users.ContainsKey(UserName.Text) && users[UserName.Text].Password != "")
                        {
                            this.PassWord.Text = users[UserName.Text].Password;
                            this.RemeberPassword.Checked = true;
                        }
                        else
                        {
                            this.PassWord.Text = "";
                            this.RemeberPassword.Checked = false;
                        }
                    }
                }
            }

            fs.Close();
        }

        //  modify by gatieme at version 1.0.2
        //  回车实现登录
        //  http://www.bubuko.com/infodetail-828823.html
        //  虽然从字面理解, KeyDown是按下一个键的意思,
        //  但实际上二者的根本区别是, 
        //  系统由KeyDown返回键盘的代码, 然后由TranslateMessage函数翻译成成字符, 由KeyPress返回字符值. 
        //  因此在KeyDown中返回的是键盘的代码, 而KeyPress返回的是ASCII字符. 
        //  所以根据你的目的, 如果只想读取字符, 用KeyPress, 如果想读各键的状态, 用KeyDown. 
        // 
        //  说KeyDown是按下, KeyPress是按下并松开, 是不对的. 
        //  如果你一直按着键呢? 这时不断地产生KeyDown和KeyPress.
        private void PassWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Submit_Click(sender, e);
            }
        }



    }  
}
