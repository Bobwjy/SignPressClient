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
using SignPressClient.Tools;

namespace SignPressClient
{
    public partial class SubmitManage : Form
    {
        SignSocketClient _sc;


        private OpaqueCommand cmd = new OpaqueCommand();


        public SubmitManage()
        {
            InitializeComponent();
        }

        public SubmitManage(int SelectedIndex,SignSocketClient sc)
            : this()
        {
            this.Submit.SelectedIndex = SelectedIndex;
            _sc = sc;
        }

        private void SubmitManage_Load(object sender, EventArgs e)                 //窗体加载事件
        {
            /*
             * combox的SelectedIndexChanged事件,在datasouce指定的时候就被触发了,这时候数据还没有绑定好,自然会报错.
             * 
             * 我认为这是不合理的.SelectedIndexChanged不应该在绑定数据的中间被触发.
             * 
             * 我最后解决办法是设置了一个标志符isLoaded,bool类型,在填充方法完毕后,设为true.允许SelectedIndexChanged被触发.
            */

            //if (UserHelper.TempList == null)                     
            //{
            //    List<Templete> list = await _sc.QueryContractTemplate();

            //    this.SelecteConTemplate.ValueMember = "TempId";
            //    this.SelecteConTemplate.DisplayMember = "Name";

            //    this.SelecteConTemplate.DataSource = list;
            //}
            //else
            //{
            //    this.SelecteConTemplate.ValueMember = "TempId";
            //    this.SelecteConTemplate.DisplayMember = "Name";

            //    this.SelecteConTemplate.DataSource = UserHelper.TempList;
               
            //}
            /*
             * combox的SelectedIndexChanged事件,在datasouce指定的时候就被触发了,这时候数据还没有绑定好,自然会报错.
             * 
             * 我认为这是不合理的.SelectedIndexChanged不应该在绑定数据的中间被触发.
             * 
             * 我最后解决办法是设置了一个标志符isLoaded,bool类型,在填充方法完毕后,设为true.允许SelectedIndexChanged被触发.
            */
            this.BindContractTemplate();
            this.ConTempInfo.Visible = false;

            //this.StartDate.Format = DateTimePickerFormat.Custom;
            //this.StartDate.CustomFormat = "yyyy-MM-dd";

            //this.EndDate.Format = DateTimePickerFormat.Custom;
            //this.EndDate.CustomFormat = "yyyy-MM-dd";

            //this.RefuseStartDate.Format = DateTimePickerFormat.Custom;
            //this.RefuseStartDate.CustomFormat = "yyyy-MM-dd";

            //this.RefuseEndDate.Format = DateTimePickerFormat.Custom;
            //this.RefuseEndDate.CustomFormat = "yyyy-MM-dd";

            this.AgreeStartDate.Format = DateTimePickerFormat.Custom;
            this.AgreeStartDate.CustomFormat = "yyyy-MM-dd";

            this.AgreeEndDate.Format = DateTimePickerFormat.Custom;
            this.AgreeEndDate.CustomFormat = "yyyy-MM-dd";

            //if (UserHelper.PenddingList == null)
            //{
            //    List<SHDJContract> hdj1 = new List<SHDJContract>();                   //加载正在审批的方案
            //    hdj1 = await _sc.QuerySignPend(UserHelper.UserInfo.Id);
            //    this.SignPendding.AutoGenerateColumns = false;
            //    this.SignPendding.DataSource = hdj1;
            //}
            //else
            //{
            //    this.SignPendding.AutoGenerateColumns = false;
            //    this.SignPendding.DataSource = UserHelper.PenddingList;
            //}

            //if (UserHelper.AgreeList == null)
            //{
            //    List<SHDJContract> hdj2 = new List<SHDJContract>();                   //加载已通过的方案
            //    hdj2 = await _sc.QuerySignAgree(UserHelper.UserInfo.Id);
            //    this.SignAgree.AutoGenerateColumns = false;
            //    this.SignAgree.DataSource = hdj2;
            //}
            //else
            //{
            //    this.SignAgree.AutoGenerateColumns = false;
            //    this.SignAgree.DataSource = UserHelper.AgreeList;
            //}

            this.BindPenddingList(false);
            //// 同意列表可能很多，数据量太大，取消自动刷新
            ///  bug 2015-07-06 10:59
            ///  修改为查询已通过的但是未曾下载过的会签单信息
            this.BindAgreeUndownloadList(false);
            //this.BindAgreeList(false);
            this.BindRefuseList(false);                                //加载已拒绝的方案


            BindSignRefuseAndAgreeOpera();                     //绑定已拒绝以及已通过方案操作列
            BindRefuseOper();
        }

        private  void BindContractTemplate()
        {
            if (UserHelper.TempList == null)
            {
                List<Templete> list = null;
                while (list == null)
                {
                    list =  _sc.QueryContractTemplate();
                }
                //List<Templete> list = await _sc.QueryContractTemplate();

                this.SelecteConTemplate.ValueMember = "TempId";
                this.SelecteConTemplate.DisplayMember = "Name";

                this.SelecteConTemplate.DataSource = list;

                UserHelper.TempList = list;
            }
            else
            {
                this.SelecteConTemplate.ValueMember = "TempId";
                this.SelecteConTemplate.DisplayMember = "Name";

                this.SelecteConTemplate.DataSource = UserHelper.TempList;
            }
        }

        /// <summary>
        /// 绑定审核中的数据信息
        /// </summary>
        /// <param name="isFlush">强制刷新标识
        /// 默认情况下，是直接读取UserHelper中的信息结构，不会强制刷新，但是某些情况下，比如用户刚提交了一个信息的时候，是期望进行强制刷新的
        /// 希望强制刷新的时候，将isFlush置为true</param>
        private  void BindPenddingList(bool isFlush)
        {
            /// 当且仅当缓存数据UserHelpwer为空，或者用户期望强制刷新时，强制进行数据获取
            if (UserHelper.PenddingList == null
            || isFlush == true)
            {

                //List<SHDJContract> penddingList = new List<SHDJContract>();
                List<SHDJContract> penddingList = null;
                while (penddingList == null)
                {
                    penddingList =  _sc.QuerySignPend(UserHelper.UserInfo.Id);
                }
                this.SignPendding.AutoGenerateColumns = false;
                this.SignPendding.DataSource = penddingList;

                UserHelper.PenddingList = penddingList;
            }
            else
            {
                this.SignPendding.AutoGenerateColumns = false;
                this.SignPendding.DataSource = UserHelper.PenddingList;
            }
        }

        private  void BindRefuseList(bool isFlush)
        {
            if (UserHelper.RefuseList == null
             || isFlush == true)
            {
                //List<SHDJContract> hdj3 = new List<SHDJContract>();                   

                List<SHDJContract> refuseList = null;
                while (refuseList == null)
                {
                    refuseList =  _sc.QuerySignRefuse(UserHelper.UserInfo.Id);
                }
                this.SignRefuse.AutoGenerateColumns = false;
                this.SignRefuse.DataSource = refuseList;

                UserHelper.RefuseList = refuseList;
            }
            else
            {
                this.SignRefuse.AutoGenerateColumns = false;
                this.SignRefuse.DataSource = UserHelper.RefuseList;
            }
        }


        private void BindAgreeUndownloadList(bool isFlush)
        {
            if (UserHelper.AgreeUndownList == null
            || isFlush == true)
            {
                List<SHDJContract> hdj2 = new List<SHDJContract>();                   //加载已通过的方案
                hdj2 = _sc.QuerySignAgreeUndownload(UserHelper.UserInfo.Id);
                this.SignAgree.AutoGenerateColumns = false;
                this.SignAgree.DataSource = hdj2;

                UserHelper.AgreeUndownList = hdj2;
            }
            else
            {
                this.SignAgree.AutoGenerateColumns = false;
                this.SignAgree.DataSource = UserHelper.AgreeUndownList;
            }
        }

        private  void BindAgreeList(bool isFlush)
        {
            if (UserHelper.AgreeList == null
             || isFlush == true)
            {
                List<SHDJContract> agreeList = null;                  //加载已通过的方案
                while (agreeList == null)
                {
                    agreeList =  _sc.QuerySignAgree(UserHelper.UserInfo.Id);                    
                }
                

                this.SignAgree.AutoGenerateColumns = false;
                this.SignAgree.DataSource = agreeList;
            }
            else
            {
                this.SignAgree.AutoGenerateColumns = false;
                this.SignAgree.DataSource = UserHelper.AgreeList;
            }       
        }

        /// <summary>
        /// 绑定后台操作项，重新提交
        /// </summary>
        private void BindRefuseOper()
        {
            this.SignRefuse.AutoGenerateColumns = false;
            DataGridViewLinkColumn rs = new DataGridViewLinkColumn();
            rs.Text = "重新提交";
            rs.Name = "LinkReSubmit";
            rs.HeaderText = "重新提交";
            rs.UseColumnTextForLinkValue = true;
            this.SignRefuse.Columns.Add(rs);
        }

        private void BindSignRefuseAndAgreeOpera()
        {
            this.SignAgree.AutoGenerateColumns = false;
            DataGridViewLinkColumn download = new DataGridViewLinkColumn();
            download.Text = "下载签单附件";
            download.Name = "LinkDownLoad";
            download.HeaderText = "下载签单附件";
            download.Width = 150;
            download.UseColumnTextForLinkValue = true;
            this.SignAgree.Columns.Add(download);
        }

        private  void SelecteConTemplate_SelectedIndexChanged(object sender, EventArgs e)         //选择会签单模板
        {
            ///  BUG[2015-07-05 13:50]
            ///  每次点击提交管理的时候，都会调用次函数进行一次会签单模版查询，
            ///  这是个BUG，并且在当一个员工从普通员工提升为管理员后，出现加载数据失败
            /// 
            int TemplateId = -1;
           
            try
            {
                this.ConTempInfo.Visible = true;

                TemplateId = Convert.ToInt32(this.SelecteConTemplate.SelectedValue.ToString());
                
                if (UserHelper.SelectedTemp == null 
                ||  UserHelper.SelectedTemp.TempId != TemplateId)
                {
                    Templete temp =  _sc.GetContractTemplate(TemplateId);
                    if(temp != null)
                    {
                        UserHelper.SelectedTemp = temp;
                    }
                }
                this.ConName.Text = UserHelper.SelectedTemp.Name.ToString().Replace("模版", null);
                this.Column1.Text = UserHelper.SelectedTemp.ColumnNames[0].ToString();
                this.Column2.Text = UserHelper.SelectedTemp.ColumnNames[1].ToString();
                this.Column3.Text = UserHelper.SelectedTemp.ColumnNames[2].ToString();
                this.Column4.Text = UserHelper.SelectedTemp.ColumnNames[3].ToString();
                this.Column5.Text = UserHelper.SelectedTemp.ColumnNames[4].ToString();

                this.Sign1.Text = UserHelper.SelectedTemp.SignDatas[0].SignInfo.ToString();
                this.Sign2.Text = UserHelper.SelectedTemp.SignDatas[1].SignInfo.ToString();
                this.Sign3.Text = UserHelper.SelectedTemp.SignDatas[2].SignInfo.ToString();
                this.Sign4.Text = UserHelper.SelectedTemp.SignDatas[3].SignInfo.ToString();
                this.Sign5.Text = UserHelper.SelectedTemp.SignDatas[4].SignInfo.ToString();
                this.Sign6.Text = UserHelper.SelectedTemp.SignDatas[5].SignInfo.ToString();
                this.Sign7.Text = UserHelper.SelectedTemp.SignDatas[6].SignInfo.ToString();
                this.Sign8.Text = UserHelper.SelectedTemp.SignDatas[7].SignInfo.ToString();

                this.SignPer1.Text = UserHelper.SelectedTemp.SignDatas[0].SignEmployee.Name.ToString();
                this.SignPer2.Text = UserHelper.SelectedTemp.SignDatas[1].SignEmployee.Name.ToString();
                this.SignPer3.Text = UserHelper.SelectedTemp.SignDatas[2].SignEmployee.Name.ToString();
                this.SignPer4.Text = UserHelper.SelectedTemp.SignDatas[3].SignEmployee.Name.ToString();
                this.SignPer5.Text = UserHelper.SelectedTemp.SignDatas[4].SignEmployee.Name.ToString();
                this.SignPer6.Text = UserHelper.SelectedTemp.SignDatas[5].SignEmployee.Name.ToString();
                this.SignPer7.Text = UserHelper.SelectedTemp.SignDatas[6].SignEmployee.Name.ToString();
                this.SignPer8.Text = UserHelper.SelectedTemp.SignDatas[7].SignEmployee.Name.ToString();
            }
            catch
            {
                MessageBox.Show("加载会签单模版" + TemplateId + "数据失败!");

            }
        }

        private void button1_Click(object sender, EventArgs e)                   //提交会签单信息
        {
            if (this.Column1Info.Text.Trim() != "" && this.Column2Info.Text.Trim() != "" &&
                this.Column3Info.Text.Trim() != "" && this.Column4Info.Text.Trim() != "" && this.Column5Info.Text.Trim() != ""
                && this.label3.Text.Trim() != "")
            {
                if (MessageBox.Show("您确定要提交所填方案吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    HDJContract hdj = new HDJContract();
                    hdj.Name = this.ConName.Text;
                    hdj.SubmitEmployee = UserHelper.UserInfo;
                    Templete temp = new Templete();
                    temp.TempId = UserHelper.SelectedTemp.TempId;
                    hdj.ConTemp = temp;
                    hdj.Id = this.ID.Text.Trim();

                    List<String> list = new List<string>();
                    list.Add(this.Column1Info.Text.ToString());
                    list.Add(this.Column2Info.Text.ToString());
                    list.Add(this.Column3Info.Text.ToString());
                    list.Add(this.Column4Info.Text.ToString());
                    list.Add(this.Column5Info.Text.ToString());
                    hdj.ColumnDatas = list;

                    string result = _sc.InsertHDJContract(hdj);
                    if (result == Response.INSERT_HDJCONTRACT_SUCCESS.ToString())
                    {
                        this.ConTempInfo.Visible = false;
                        this.Column1Info.Text = "";
                        this.Column2Info.Text = "";
                        this.Column3Info.Text = "";
                        this.Column4Info.Text = "";
                        this.Column5Info.Text = "";
                        this.ID.Text = "";

                        MessageBox.Show("提交成功!", "提示", MessageBoxButtons.OK);

                        //if(hdj.ConTemp.SignDatas.Where())
                        // 2015-07-03 11:25  提交成功后应该刷新一下待签字结构体
                        this.BindPenddingList(true);        //  强制刷新待审核的数据

                        /////////////////////////////////////////
                        /// 每次提交一个新的单子之后，待审核数目增加 1///
                        /////////////////////////////////////////
                        MainWindow mw = (MainWindow)this.MdiParent;
                        foreach (TreeNode t in mw.treeView1.Nodes)
                        {
                            if (t.Text.Contains("提交管理("))
                            {
                                int count = Convert.ToInt32(t.Text.Split('(')[1].Split(')')[0]);    // 提交管理

                                t.Text = "提交管理(" + (count + 1) + ")";

                                //t.Nodes[0]  -=>  提交方案
                                //t.Nodes[1]  -=>  审核中
                                //t.Nodes[2]  -=>  已拒绝
                                //t.Nodes[3]  -=>  已通过
                                int childcount = Convert.ToInt32(t.Nodes[1].Text.Split('(')[1].Split(')')[0]);
                                t.Nodes[1].Text = "审核中(" + (childcount + 1) + ")";

                            }
                        }
                    }
                    else if (result == "服务器连接中断")
                    {
                        MessageBox.Show("服务器连接中断,提交失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (result == Response.INSERT_HDJCONTRACT_EXIST.ToString())
                    {
                        MessageBox.Show("该会签单编号已经存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("提交失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("请将所有空白处填完!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SignPendding_CellContentClick(object sender, DataGridViewCellEventArgs e)           //查看正在审批方案的详细信息
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == 1)
            {

                string Id = this.SignPendding.Rows[e.RowIndex].Cells[0].Value.ToString();
                SignConTemp sct = new SignConTemp(_sc, Id, 1);
                sct.ShowDialog();
            }
        }

        private void SignRefuse_CellContentClick(object sender, DataGridViewCellEventArgs e)              //已拒绝列表操作功能
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == 1)
            {
                string Id = this.SignRefuse.Rows[e.RowIndex].Cells[0].Value.ToString();
                SignConTemp sct = new SignConTemp(_sc, Id, 2);
                sct.ShowDialog();
            }
            else if (e.ColumnIndex == 4)
            {
                string Id = this.SignRefuse.Rows[e.RowIndex].Cells[0].Value.ToString();
                ReSubmitConTemp rsct = new ReSubmitConTemp(_sc, Id);
                rsct.ShowDialog();
                if (rsct.DialogResult == DialogResult.OK)
                {
                    rsct.Close();
                    ////
                    BindRefuseList(true);      // 重新提交后，强制是刷新拒绝列表
                    BindPenddingList(true);

                    MainWindow mw = (MainWindow)this.MdiParent;
                    foreach (TreeNode t in mw.treeView1.Nodes)
                    {
                        if (t.Text.Contains("提交管理("))
                        {
                            //t.Nodes[0]  -=>  提交方案
                            //t.Nodes[1]  -=>  审核中
                            //t.Nodes[2]  -=>  已拒绝
                            //t.Nodes[3]  -=>  已通过
                            int count = Convert.ToInt32(t.Text.Split('(')[1].Split(')')[0]);
                            if (count - 1 == 0)
                            {
                                t.Text = "提交管理";
                                t.Nodes[2].Text = "已拒绝";
                            }
                            else
                            {
                                t.Text = "提交管理(" + (count - 1) + ")";
                                int childcount = Convert.ToInt32(t.Nodes[2].Text.Split('(')[1].Split(')')[0]);
                                if (childcount - 1 == 0)
                                {
                                    t.Nodes[2].Text = "已拒绝";
                                }
                                else
                                {
                                    t.Nodes[2].Text = "已拒绝(" + (childcount - 1) + ")";
                                }
                            }
                        }
                    }
                }
            }
        }

        private async void SignAgree_CellContentClick(object sender, DataGridViewCellEventArgs e)        //已通过列表操作列表
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == 1)
            {
                string Id = this.SignAgree.Rows[e.RowIndex].Cells[0].Value.ToString();
                SignConTemp sct = new SignConTemp(_sc, Id, 3);
                sct.ShowDialog();
            }
            if (e.ColumnIndex == 4)
            {
                MessageBox.Show(@"温馨提示
如果您是首次下载附件，系统将为您生成会签单文件，这个过程比较费时间，希望您能耐心等待!
生成过程中我们会在磁盘上为您生成缓存文件，打开缓存可能会导致文件损毁！
为防止用户误操作造成的损毁，我们已经为您做了备份。
如果下载完成后提示您文件损坏无法打开，您只需要点击重新下载即可，系统会立即调用缓存为您重新生成（这个速度是很快的）
由此给您带来的不便，我们表示真诚的歉意", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                string Id = this.SignAgree.Rows[e.RowIndex].Cells[0].Value.ToString();
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = this.SignAgree.Rows[e.RowIndex].Cells[0].Value.ToString();
                sfd.Filter = "*.pdf | *.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    MainWindow mw = (MainWindow)this.MdiParent;
                    cmd.ShowOpaqueLayer(this.groupBox2, 125, true,true,"正在下载中，请稍候");
                    mw.treeView1.Enabled = false;

                    string filepath = sfd.FileName.ToString()+".pdf";
                    await _sc.DownloadHDJContract(Id, filepath);
                    //if(cmd.)
                    //{
                        cmd.HideOpaqueLayer();
                    //}
                    MessageBox.Show("下载完成！");
                    mw.treeView1.Enabled = true;
                
                    foreach (TreeNode t in mw.treeView1.Nodes)
                    {
                        if (t.Text.Contains("提交管理("))
                        {
                            //t.Nodes[0]  -=>  提交方案
                            //t.Nodes[1]  -=>  审核中
                            //t.Nodes[2]  -=>  已拒绝
                            //t.Nodes[3]  -=>  已通过
                            int count = Convert.ToInt32(t.Text.Split('(')[1].Split(')')[0]);
                            if (count - 1 == 0)
                            {
                                t.Text = "提交管理";
                                t.Nodes[3].Text = "已通过";
                            }
                            else
                            {
                                t.Text = "提交管理(" + (count - 1) + ")";
                                int childcount = Convert.ToInt32(t.Nodes[3].Text.Split('(')[1].Split(')')[0]);
                                if (childcount - 1 == 0)
                                {
                                    t.Nodes[3].Text = "已通过";
                                }
                                else
                                {
                                    t.Nodes[3].Text = "已通过(" + (childcount - 1) + ")";
                                }
                            }
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
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
                Search s = new Search();
                s.EmployeeId = UserHelper.UserInfo.Id;
                s.ConId = ContractId;
                s.ProjectName = projectName;
                s.DateBegin = start;
                s.DateEnd = end;

                List<SHDJContract> list = new List<SHDJContract>();
                list =  _sc.SearchAgreeHDJConstract(s);

                this.SignAgree.DataSource = list;
            }
        }

        private void SignPendding_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            List<SHDJContract> list = new List<SHDJContract>();
            list = _sc.QuerySignPend(UserHelper.UserInfo.Id);
            this.SignPendding.DataSource = list;
            UserHelper.PenddingList = list;
        }

        private void SignRefuse_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            List<SHDJContract> list = new List<SHDJContract>();
            list = _sc.QuerySignRefuse(UserHelper.UserInfo.Id);
            this.SignRefuse.DataSource = list;
            UserHelper.RefuseList = list;
        }

        private void SignAgree_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            List<SHDJContract> list = new List<SHDJContract>();
            list = _sc.QuerySignAgree(UserHelper.UserInfo.Id);
            this.SignAgree.DataSource = list;
            UserHelper.AgreeList = list;
        }

        private void RefreshRefuselist_Click(object sender, EventArgs e)
        {
            List<SHDJContract> list = new List<SHDJContract>();
            list = null;
            while (list == null)
            {
                list = _sc.QuerySignRefuse(UserHelper.UserInfo.Id);
            }
            this.SignRefuse.DataSource = list;
            UserHelper.RefuseList = list;
        }

        private void RefreshPendinglist_Click(object sender, EventArgs e)
        {
            List<SHDJContract> list = new List<SHDJContract>();
            list = null;
            while (list == null)
            {
                list = _sc.QuerySignPend(UserHelper.UserInfo.Id);
            }
            this.SignPendding.DataSource = list;
            UserHelper.PenddingList = list;
        }



    }
}
