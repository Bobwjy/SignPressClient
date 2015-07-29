using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace SignPressClient
{
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
        }

        #region 关闭按钮效果显示
        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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
            this.cuemessage.Show("关闭", this.Close, this.Close.Width, this.Close.Height);
        }
        #endregion

        #region 最小化按钮效果显示
        private void min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

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

        private void Cancel_Click(object sender, EventArgs e)         //取消
        {
            this.Close();
        }

        private void Confirm_Click(object sender, EventArgs e)        //确定
        {
            string ipAdrress = IPAddress.Text.Trim();
            string port = Port.Text.Trim();

            if (ipAdrress == "")
            {
                errormessage.IsBalloon = true;
                errormessage.SetToolTip(this.IPAddress, "地址栏不得为空");
                errormessage.Show("地址栏不得为空", this.IPAddress, 1, this.IPAddress.Height / 2, 2000);
                errormessage.UseFading = false;
                IPAddress.Focus();
                return;
            }
            else if (port == "")
            {
                errormessage.IsBalloon = true;
                errormessage.SetToolTip(this.Port, "端口栏不得为空");
                errormessage.Show("端口栏不得为空", this.Port, 1, this.Port.Height / 2, 2000);
                errormessage.UseFading = false;
                Port.Focus();
                return;
            }
            else
            {
                //将写入的值更新到配置文件中
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["ipAddress"].Value = ipAdrress;
                config.AppSettings.Settings["port"].Value = port;
                config.Save();
                ConfigurationManager.RefreshSection("appSettings");

                this.Close();
            }
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            string ipAddress = ConfigurationManager.AppSettings["ipAddress"];
            string port = ConfigurationManager.AppSettings["port"];

            this.IPAddress.Text = ipAddress;
            this.Port.Text = port;
        }

    }
}
