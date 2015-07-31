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
using System.Timers;
using SignPressClient.SignLogging;
using SignPressClient.Tools;

///  日期2015-07-29 21:40
///  版本更新自1.0.1
///  添加托盘程序
/// 
///



///  日期2015-07-30 21:48
///  版本更新自1.0.2
///  登录窗体，增加记密码的功能

namespace SignPressClient
{
    public partial class MainWindow : Form
    {
        private CartoonForm m_cartoonForm;

        private System.Timers.Timer UnSign_timer = new System.Timers.Timer();                  //定时器
        private System.Timers.Timer Submit_timer = new System.Timers.Timer();

        SignSocketClient _sc;
        Employee emp = UserHelper.UserInfo;

        public MainWindow()
        {
            InitializeComponent();
            m_cartoonForm = new CartoonForm(this);
            
            this.UnSign_timer.Elapsed += new ElapsedEventHandler(this.UnSign_timer_Elapsed);
            this.Submit_timer.Elapsed += new ElapsedEventHandler(this.Submit_timer_Elapsed);

            if (emp.CanSign == 1)
            {
                this.UnSign_timer.Enabled = true;
            }
            else
            {
                this.UnSign_timer.Enabled = false;
            }

            if (emp.CanSubmit == 1)
            {
                this.Submit_timer.Enabled = true;
            }
            else
            {
                this.Submit_timer.Enabled = false;
            }
            // 2015-07-01 14:17 modify by gatieme
            // 设置主窗口的标题
            this.Text = "黑龙江省航道局会签单 : " + UserHelper.UserInfo.Name;
            this.mainNotifyIcon.Text = "黑龙江省航道局会签单 : " + UserHelper.UserInfo.Name;
        }

        private void Submit_timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Submit_timer.Interval = 1800*1000;
            this.Submit_timer.Start();
            BindSubmitData();
        }


        //public delegate void 
        private  void BindSubmitData()                                    //窗体加载时绑定提交管理中各任务的数量
        {

            ///  modify by gatieme 2015-07-02 16:56
            ///  BUG[NO.20150702-000001]
            ///  定时器会定时更新数据，
            ///  但是会出现一个问题，
            ///  如果定时器刷新的时候, 某一项数据在TCP传输的时候，获取失败的话，
            ///  那么这项数据就成为null, 由于之间未进行null异常的处理，程序throw出异常信息
            ///  
            ///  2015-07-02 16:56发现此BUG 
            ///  2015-07-02 16:56决定修复此BUG
            ///  我们目前有两种处理方案
            ///  
            ///  第一种处理方案
            ///  如果有一个为空，就全部不重新加载，仍然使用久的数据源
            ///  
            ///  第二种处理方案
            ///  哪一项数据获取不正常，就不加载，其他数据照样加载
            try
            {
                List<SHDJContract> penddingList = null;
                while (penddingList == null)
                {
                    penddingList =  _sc.QuerySignPend(UserHelper.UserInfo.Id);
                }

                List<SHDJContract> refuseList = null;
                while (refuseList == null)
                {
                    refuseList = _sc.QuerySignRefuse(UserHelper.UserInfo.Id);
                }

                List<SHDJContract> agreeList = null;
                while (agreeList == null)
                {
                    agreeList = _sc.QuerySignAgreeUndownload(UserHelper.UserInfo.Id);
                }

//                List<SHDJContract> penddingList =  _sc.QuerySignPend(UserHelper.UserInfo.Id);
//                List<SHDJContract> refuseList =  _sc.QuerySignRefuse(UserHelper.UserInfo.Id);
////                List<SHDJContract> agreeList = _sc.QuerySignAgree(UserHelper.UserInfo.Id);
//                List<SHDJContract> agreeList = _sc.QuerySignAgreeUndownload(UserHelper.UserInfo.Id);

                //  只有当数据发生变化的时候，才需要跟新数据
                //  首先如果全局数据为空，说明定时器第一次启动，则需要进行一次数据绑定
                //  其次当数据存储更新时才更新数据
                if (UserHelper.PenddingList != null
                 && UserHelper.RefuseList != null
                 && UserHelper.AgreeList != null
                 && penddingList.Count == UserHelper.PenddingList.Count
                 && refuseList.Count == UserHelper.RefuseList.Count
                 && agreeList.Count == UserHelper.AgreeList.Count)
                {
                    return;
                }
                //    if (UserHelper.PenddingList == null
                //|| UserHelper.RefuseList == null
                //|| UserHelper.AgreeList == null
                //|| penddingList.Count != UserHelper.PenddingList.Count
                //|| refuseList.Count != UserHelper.RefuseList.Count
                //|| agreeList.Count != UserHelper.AgreeList.Count)
                //{

                if (penddingList != null)
                {
                    UserHelper.PenddingList = penddingList;
                }

                if (refuseList != null)
                {
                    UserHelper.RefuseList = refuseList;
                }

                if (agreeList != null)
                {
                    UserHelper.AgreeList = agreeList;
                }

                for (int i = 0; i < Application.OpenForms.Count; i++)
                {
                    if (Application.OpenForms[i].Name == "SubmitManage")
                    {
                        ((SubmitManage)Application.OpenForms[i]).SignPendding.DataSource = UserHelper.PenddingList;
                        ((SubmitManage)Application.OpenForms[i]).SignRefuse.DataSource = UserHelper.RefuseList;
                        ((SubmitManage)Application.OpenForms[i]).SignAgree.DataSource = UserHelper.AgreeList;
                    }
                }
                //}

                int sum = UserHelper.PenddingList.Count + UserHelper.RefuseList.Count + UserHelper.AgreeList.Count;
                if (sum > 0)
                {
                    foreach (TreeNode n in this.treeView1.Nodes)
                    {
                        if (n.Text == "提交管理")
                        {
                            n.Text = "提交管理(" + sum + ")";
                            if (UserHelper.PenddingList.Count > 0)
                            {
                                n.Nodes[1].Text = "审核中(" + UserHelper.PenddingList.Count + ")";
                            }
                            if (UserHelper.RefuseList.Count > 0)
                            {
                                n.Nodes[2].Text = "已拒绝(" + UserHelper.RefuseList.Count + ")";
                                MessageBox.Show("您有新的提交方案被拒绝，请及时查看!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            if (UserHelper.AgreeList.Count > 0)
                            {
                                n.Nodes[3].Text = "已通过(" + UserHelper.AgreeList.Count + ")";
                            }
                        }
                        else if (n.Text.Contains("提交管理("))
                        {
                            n.Text = "提交管理(" + sum + ")";
                            if (UserHelper.PenddingList.Count > 0)
                            {
                                n.Nodes[1].Text = "审核中(" + UserHelper.PenddingList.Count + ")";
                            }
                            else
                            {
                                n.Nodes[1].Text = "审核中";
                            }
                            if (UserHelper.RefuseList.Count > 0)
                            {
                                n.Nodes[2].Text = "已拒绝(" + UserHelper.RefuseList.Count + ")";
                            }
                            else
                            {
                                n.Nodes[2].Text = "已拒绝";
                            }
                            if (UserHelper.AgreeList.Count > 0)
                            {
                                n.Nodes[3].Text = "已通过(" + UserHelper.AgreeList.Count + ")";
                            }
                            else
                            {
                                n.Nodes[3].Text = "已通过";
                            }
                        }
                    }
                }
                else
                {
                    foreach (TreeNode n in this.treeView1.Nodes)
                    {
                        if (n.Name == "SubmitManage")
                        {
                            n.Text = "提交管理";
                            n.Nodes[1].Text = "审核中";
                            n.Nodes[2].Text = "已拒绝";
                            n.Nodes[3].Text = "已通过";
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //throw;
                Logging.AddLog(e.Message + "HelloWord" + e.TargetSite);
            }
        }

        private void UnSign_timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.UnSign_timer.Interval = 1200*1000;         //  设置1min = 60s = 60000ms刷新一次
            this.UnSign_timer.Start();
            BindSignData();  
        }

        private  void BindSignData()                              //绑定待办列表的个数
        {
            try
            {
                //List<SHDJContract> list = new List<SHDJContract>();
                //list = await _sc.QueryUnsignContract(UserHelper.UserInfo.Id);

                List<SHDJContract> list = null;

                while (list == null)
                {
                    list =  _sc.QueryUnsignContract(UserHelper.UserInfo.Id);
                }
                if (UserHelper.ToDoList != null
                 && UserHelper.ToDoList.Count == list.Count)
                {
                    return;
                }

                //if (UserHelper.ToDoList == null
                // || UserHelper.ToDoList.Count != list.Count)
                //{


                if (list != null)
                {
                    UserHelper.ToDoList = list;
                }
                for (int i = 0; i < Application.OpenForms.Count; i++)
                {

                    if (Application.OpenForms[i].Name == "SignManage")
                    {
                        ((SignManage)Application.OpenForms[i]).ToDoListView.DataSource = list;
                    }
                }

                if (UserHelper.ToDoList.Count > 0)
                {
                    foreach (TreeNode n in this.treeView1.Nodes)
                    {
                        if (n.Text == "签字管理")
                        {
                            n.Text = "签字管理(" + UserHelper.ToDoList.Count + ")";
                            n.Nodes[0].Text = "待办列表(" + UserHelper.ToDoList.Count + ")";
                            MessageBox.Show("您有新的提交方案需签字!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else if (n.Text.Contains("签字管理("))
                        {
                            n.Text = "签字管理(" + UserHelper.ToDoList.Count + ")";
                            n.Nodes[0].Text = "待办列表(" + UserHelper.ToDoList.Count + ")";
                        }
                    }
                }
                else
                {
                    foreach (TreeNode n in this.treeView1.Nodes)
                    {
                        if (n.Name == "SignPressManage")
                        {
                            n.Text = "签字管理";
                            n.Nodes[0].Text = "待办列表";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logging.AddLog(e.Message + "Helloword" + e.TargetSite);
            }
        }

        public MainWindow(SignSocketClient sc)
            : this()
        {
            _sc = sc;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.label1.Text = "欢迎您，" + UserHelper.UserInfo.Name;
            try
            {
                if (emp.IsAdmin == 0)
                {
                    foreach (TreeNode n in this.treeView1.Nodes)
                    {
                        if (n.Name == "Admin")
                        {
                            this.treeView1.Nodes.Remove(n);
                            break;
                        }
                    }
                }

                if (emp.CanSubmit == 0)
                {
                    foreach (TreeNode n in this.treeView1.Nodes)
                    {
                        if (n.Name == "SubmitManage")
                        {
                            this.treeView1.Nodes.Remove(n);
                            break;
                        }
                    }
                }

                if (emp.CanSign == 0)
                {
                    foreach (TreeNode n in this.treeView1.Nodes)
                    {
                        if (n.Name == "SignPressManage")
                        {
                            this.treeView1.Nodes.Remove(n);
                            break;
                        }
                    }
                }
                this.treeView1.ExpandAll();
            }
            catch(Exception ex)
            {
                Logging.AddLog(ex.Message);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            switch (e.Node.Text.Split('(')[0]) 
            {
                case "部门管理":
                    CreatAdminChildForm(0,_sc);
                    break;
                case "人员管理":
                    CreatAdminChildForm(1,_sc);
                    break;
                case "模板管理":
                    CreatAdminChildForm(2,_sc);
                    break;
                case "提交方案":
                    CreatSubmitChildForm(0, _sc);
                    break;
                case "审核中":
                    CreatSubmitChildForm(1, _sc);
                    break;
                case "已拒绝":
                    CreatSubmitChildForm(2, _sc);
                    break;
                case "已通过":
                    CreatSubmitChildForm(3, _sc);
                    break;
                case "待办列表":
                    CreatSignChildForm(0, _sc);
                    break;
                case "已办列表":
                    CreatSignChildForm(1, _sc);
                    break;
                case "查看个人信息":
                    CreatUserChildForm(0);
                    break;
                case "修改密码":
                    CreatUserChildForm(1);
                    break;
            }
        }

        /// <summary>
        /// 加载用户管理窗体
        /// </summary>
        /// <param name="selectedIndex"></param>
        private void CreatUserChildForm(int selectedIndex)
        {
            if (this.MdiChildren != null)
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Name == "UserManage")
                    {
                        form.Close();
                    }
                }
            }
            UserManage user = new UserManage(selectedIndex, _sc);
            user.MdiParent = this;
            user.Dock = DockStyle.Fill;
            user.Show();
        }

        /// <summary>
        /// 加载签字管理窗体
        /// </summary>
        /// <param name="selectedIndex"></param>
        /// <param name="sc"></param>
        private void CreatSignChildForm(int selectedIndex,SignSocketClient sc)
        {
            if (this.MdiChildren != null)
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Name == "SignManage")
                    {
                        form.Close();
                    }
                }
            }
            SignManage sign = new SignManage(selectedIndex, sc);
            sign.MdiParent = this;
            sign.Dock = DockStyle.Fill;
            sign.Show();
        }

        /// <summary>
        /// 加载提交管理窗体
        /// </summary>
        /// <param name="selectedIndex"></param>
        /// <param name="sc"></param>
        private void CreatSubmitChildForm(int selectedIndex,SignSocketClient sc)
        {
            if (this.MdiChildren != null)
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Name == "SubmitManage")
                    {
                        form.Close();
                    }
                }
            }
            SubmitManage sub = new SubmitManage(selectedIndex, sc);
            sub.MdiParent = this;
            sub.Dock = DockStyle.Fill;
            sub.Show();
        }

        /// <summary>
        /// 加载管理员信息管理窗体
        /// </summary>
        /// <param name="selectedIndex"></param>
        /// <param name="sc"></param>
        private void CreatAdminChildForm(int selectedIndex,SignSocketClient sc)
        {
            if (this.MdiChildren != null)
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Name == "AdminManage")
                    {
                        form.Close();
                    }
                }
            }
            AdminManage admin = new AdminManage(selectedIndex, sc);
            admin.MdiParent = this;
            admin.Dock = DockStyle.Fill;
            admin.Show();
        }

        public void RestartWhenError()
        {
            if (MessageBox.Show("程序出现鼓掌需重新启动？",
                "提示",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Restart();
            }
        }


        //  BUG
        //  只有Form_Closing事件中 e.Cancel可以用。
        //  你的是Form_Closed事件。 Form_Closed事件时窗口已关了 ，Cancel没用了；
        //  Form_Closing是窗口即将关闭时询问你是不是真的关闭才有Cancel事件
        //private  void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    //await this._sc.Quit();
        //    //this._sc.Close();
        //    // 注意判断关闭事件reason来源于窗体按钮，否则用菜单退出时无法退出!
        //    if (e.CloseReason == CloseReason.UserClosing)
        //    {
        //        //取消"关闭窗口"事件
        //        e.Cancel = true;
        //        e.Cancel = true; // 取消关闭窗体 
        //        //使关闭时窗口向右下角缩小的效果
        //        this.WindowState = FormWindowState.Minimized;
        //        this.mainNotifyIcon.Visible = true;
        //        this.Hide();
        //        return;
        //    }
        //}
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            //await this._sc.Quit();
            //this._sc.Close();
            // 注意判断关闭事件reason来源于窗体按钮，否则用菜单退出时无法退出!
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //取消"关闭窗口"事件
                e.Cancel = true; // 取消关闭窗体 
               
                //使关闭时窗口向右下角缩小的效果
                this.WindowState = FormWindowState.Minimized;
                this.mainNotifyIcon.Visible = true;
                //this.m_cartoonForm.CartoonClose();
                this.Hide();
                return;
            }
        }

        //  添加托盘程序
        //  版本更新自1.0.1
        private void mainNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
            {
                this.WindowState = FormWindowState.Minimized;
                this.mainNotifyIcon.Visible = true;
                this.Hide();
            }
            else
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }


        /// http://www.cnblogs.com/yuejin/p/3445713.html
        //  添加托盘程序右键菜单项
        //  版本更新自1.0.1
        //  最小化
        //  添加日期 --  2015-07-29 21:40
        private void toolStripMenuItemMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.mainNotifyIcon.Visible = true;
            this.Hide();
        }

        //  添加托盘程序右键菜单项
        //  版本更新自1.0.1
        //  最大化
        //  添加日期 --  2015-07-29 21:41
        private void toolStripMenuItemMaximize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.mainNotifyIcon.Visible = true;
            this.Show();
        }

        //  添加托盘程序右键菜单项
        //  版本更新自1.0.1
        //  还原
        //  添加日期 --  2015-07-29 21:43
        private void toolStripMenuItemNormal_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.mainNotifyIcon.Visible = true;
            //this.m_cartoonForm.CartoonShowNormal();
            this.Show();
        }

        //  添加托盘程序右键菜单项
        //  版本更新自1.0.1
        //  退出
        //  添加日期 --  2015-07-29 21:44
        private async void toolStripMenuItemQuit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要退出？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {

                await this._sc.Quit();
                this._sc.Close();

                this.mainNotifyIcon.Visible = false;
                this.Close();
                this.Dispose();
                System.Environment.Exit(System.Environment.ExitCode);   

            }
        }
    }
}
