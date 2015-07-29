using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SignPressClient.SignSocket;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

namespace SignPressClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Login());

            //  http://www.cnblogs.com/zyh-nhy/archive/2008/01/28/1056194.html
            Control.CheckForIllegalCrossThreadCalls = false;

            try
            {
                Login login = new Login();
                login.ShowDialog();
                if (login.DialogResult == DialogResult.OK)
                {
                    Application.Run(new MainWindow(login._sc));
                }
                else
                {
                    return;
                }
            }
            catch
            {
                MessageBox.Show("初始化失败!", "系统登录", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}
