using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SignPressClient.SignSocket;
using System.Net;
using System.Net.Sockets;
using System.Configuration;



///  日期2015-07-30 21:48
///  版本更新自1.0.2
///  登录窗体，增加记密码的功能
///  最后采用
///  http://www.cnblogs.com/ou444/archive/2011/09/13/2174911.html
///  http://www.cnblogs.com/yhyjy/p/3688466.html
///  
///  http://bbs.csdn.net/topics/360171350
///  http://blog.163.com/daishuguang4461@126/blog/static/93370424201172373935540/
///  http://blog.csdn.net/yanguan55/article/details/8206808
///  
///  方法一：写入数据库
///  方法二：写入文件——txt、xml、ini
///  方法三：注册表



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
