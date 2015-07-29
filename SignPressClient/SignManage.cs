using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using SignPressClient.Model;
using SignPressClient.SignSocket;

namespace SignPressClient
{
    public partial class SignManage : Form
    {
        SignSocketClient _sc;
        //private OpaqueCommand cmd = new OpaqueCommand();


        public SignManage()
        {
            InitializeComponent();
        }

        public SignManage(int selectedIndex, SignSocketClient sc)
            : this()
        {
            this.Sign.SelectedIndex = selectedIndex;
            _sc = sc;
        }

        private void SignManage_Load(object sender, EventArgs e)
        {

            this.BindUnsignData(false);

            //  modify by gatieme [2015-07-06 10:57]
            //this.BindSignedData(false);
            /////////////////////////////////////////
        }
        //QUERY_UNSIGN_CONTRACT_REQUEST;1;1QUERY_SIGN_REFUSE_REQUEST;1;1,

        /// <summary>
        /// 代办列表的数据量较小，可以自动刷新
        /// </summary>
        /// <param name="isFlush"></param>
        private  void BindUnsignData(bool isFlush)
        {
            if (UserHelper.ToDoList == null
            || isFlush == true)
            {
                List<SHDJContract> unsignList =  _sc.QueryUnsignContract(UserHelper.UserInfo.Id);

                //List<SHDJContract> unsignList = null;

                while (unsignList == null)
                {
                    unsignList =  _sc.QueryUnsignContract(UserHelper.UserInfo.Id);
                }

                this.ToDoListView.AutoGenerateColumns = false;
                this.ToDoListView.DataSource = unsignList;

                UserHelper.ToDoList = unsignList;
            }
            else
            {
                this.ToDoListView.AutoGenerateColumns = false;
                this.ToDoListView.DataSource = UserHelper.ToDoList;
            }
        }

        /// <summary>
        /// 已办列表的数据量较大，自动刷新不是很划算
        /// </summary>
        /// <param name="isFlush"></param>
        private  void BindSignedData(bool isFlush)
        {
            /// 当且仅当缓存数据UserHelpwer为空，或者用户期望强制刷新时，强制进行数据获取
            if (UserHelper.DoneList == null
            || isFlush == true)
            {
                List<SHDJContract> signedList =  _sc.QuerySignedContract(UserHelper.UserInfo.Id);
                //List<SHDJContract> signedList = null;
                //while (signedList == null)
                //{
                //    signedList = await _sc.QuerySignedContract(UserHelper.UserInfo.Id);
                //}
                this.DoneListView.AutoGenerateColumns = false;
                this.DoneListView.DataSource = signedList;

                UserHelper.DoneList = signedList;
            }
            else
            {
                this.DoneListView.AutoGenerateColumns = false;
                this.DoneListView.DataSource = UserHelper.DoneList;

            }
        }



        // 待签字列表的单机事件
        private void ToDoListView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == 1)
            {
                string Id = this.ToDoListView.Rows[e.RowIndex].Cells[0].Value.ToString();
                SignConTemp sct = new SignConTemp(_sc, Id, 4);
                sct.ShowDialog();


                if (sct.DialogResult == DialogResult.OK)
                {
                    sct.Close();
                    ///[BUG NO.2015-07-05 17:52]

                    BindUnsignData(true);

                    // 用户签字后，待办列表数目减少1，已办列表数目增加1
                    MainWindow mw = (MainWindow)this.MdiParent;
                    foreach (TreeNode t in mw.treeView1.Nodes)
                    {
                        if (t.Text.Contains("签字管理("))
                        {
                            int count = Convert.ToInt32(t.Text.Split('(')[1].Split(')')[0]);
                            if (count - 1 == 0)
                            {
                                t.Text = "签字管理";
                                t.Nodes[0].Text = "待办列表";
                            }
                            else
                            {
                                t.Text = "签字管理(" + (count - 1) + ")";
                                t.Nodes[0].Text = "待办列表(" + (count - 1) + ")";
                            }
                        }
                    }
                    //BindSignedData(true);
                }
            }
        }


        // 已办列表的单击事件
        private void DoneListView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == 1)
            {

                string id = this.DoneListView.Rows[e.RowIndex].Cells[0].Value.ToString();
                SignConTemp sct = new SignConTemp(_sc, id, 5);
                sct.ShowDialog();

            }
        }

        private  void button3_Click(object sender, EventArgs e)
        {
            string ContractId = this.ContractID.Text;
            string projectName = this.ProgramName.Text;
            DateTime start = this.AgreeStartDate.Value;
            DateTime end = this.AgreeEndDate.Value;
            if (start > end)
            {
                MessageBox.Show("开始日期必须小于结束日期！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (this.Downloadable.Checked == true)
                {
                    Search s = new Search();
                    s.EmployeeId = UserHelper.UserInfo.Id;
                    s.ConId = ContractId;
                    s.ProjectName = projectName;
                    s.DateBegin = start;
                    s.DateEnd = end;
                    s.Downloadable = 1;

                    List<SHDJContract> list = new List<SHDJContract>();
                    list = _sc.SearchSignedHDJConstract(s);


                    this.DoneListView.AutoGenerateColumns = false;
                    this.DoneListView.DataSource = list;
                    DataGridViewLinkColumn download = new DataGridViewLinkColumn();
                    download.Text = "下载签单附件";
                    download.Name = "LinkDownLoad";
                    download.HeaderText = "下载签单附件";
                    download.Width = 150;
                    download.UseColumnTextForLinkValue = true;
                    this.DoneListView.Columns.Add(download);
                }
                else
                {
                    Search s = new Search();
                    s.EmployeeId = UserHelper.UserInfo.Id;
                    s.ConId = ContractId;
                    s.ProjectName = projectName;
                    s.DateBegin = start;
                    s.DateEnd = end;
                    s.Downloadable = 0;

                    List<SHDJContract> list = new List<SHDJContract>();
                    list = _sc.SearchSignedHDJConstract(s);


                    this.DoneListView.AutoGenerateColumns = false;
                    this.DoneListView.DataSource = list;
                }
            }
        }

        private void RefreshPendinglist_Click(object sender, EventArgs e)
        {
            List<SHDJContract> list = new List<SHDJContract>();
            list = null;
            while (list == null)
            {
                list = _sc.QueryUnsignContract(UserHelper.UserInfo.Id);
            }
            this.ToDoListView.DataSource = list;
            UserHelper.ToDoList = list;
        }
    }
}
