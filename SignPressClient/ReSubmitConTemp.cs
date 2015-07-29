using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SignPressClient.SignSocket;
using SignPressClient.Model;
using SignPressClient.SignLogging;

namespace SignPressClient
{
    public partial class ReSubmitConTemp : Form
    {
        string              Id;                  // 待处理的会签单编号
        SignSocketClient    _sc;
        HDJContract Contract;

        public ReSubmitConTemp()
        {
            InitializeComponent();
        }

        public ReSubmitConTemp(SignSocketClient sc, string ID)
            : this()
        {
            _sc = sc;
            Id = ID;
        }
        
        public ReSubmitConTemp(SignSocketClient sc, HDJContract contract)
            : this()
        {
            this._sc = sc;
            this.Id = contract.Id;
            this.Contract = contract;

        }

        private async void ReSubmitConTemp_Load(object sender, EventArgs e)
        {
            try
            {
                HDJContract hdj = new HDJContract();
                hdj = await _sc.GetHDJContract(Id);

                this.ConTempName.Text = hdj.Name;
                this.ConTempId.Text = hdj.Id;

                // 显示5个栏目的信息
                List<string> columnlist = new List<string>();
                columnlist = hdj.ConTemp.ColumnNames;
                this.Column1.Text = columnlist[0].ToString();
                this.Column2.Text = columnlist[1].ToString();
                this.Column3.Text = columnlist[2].ToString();
                this.Column4.Text = columnlist[3].ToString();
                this.Column5.Text = columnlist[4].ToString();

                // 2015-07-01 Modify by gatieme
                // 显示项目的信息
                List<String> columnInfo = hdj.ColumnDatas;
                this.Column1Info.Text = columnInfo[0].ToString();
                this.Column2Info.Text = columnInfo[1].ToString();
                this.Column3Info.Text = columnInfo[2].ToString();
                this.Column4Info.Text = columnInfo[3].ToString();
                this.Column5Info.Text = columnInfo[4].ToString();

                // 显示8个签字人的信息
                this.Sign1.Text = hdj.ConTemp.SignDatas[0].SignInfo.ToString();
                this.Sign2.Text = hdj.ConTemp.SignDatas[1].SignInfo.ToString();
                this.Sign3.Text = hdj.ConTemp.SignDatas[2].SignInfo.ToString();
                this.Sign4.Text = hdj.ConTemp.SignDatas[3].SignInfo.ToString();
                this.Sign5.Text = hdj.ConTemp.SignDatas[4].SignInfo.ToString();
                this.Sign6.Text = hdj.ConTemp.SignDatas[5].SignInfo.ToString();
                this.Sign7.Text = hdj.ConTemp.SignDatas[6].SignInfo.ToString();
                this.Sign8.Text = hdj.ConTemp.SignDatas[7].SignInfo.ToString();

                this.SignPer1.Text = hdj.ConTemp.SignDatas[0].SignEmployee.Name.ToString();
                this.SignPer2.Text = hdj.ConTemp.SignDatas[1].SignEmployee.Name.ToString();
                this.SignPer3.Text = hdj.ConTemp.SignDatas[2].SignEmployee.Name.ToString();
                this.SignPer4.Text = hdj.ConTemp.SignDatas[3].SignEmployee.Name.ToString();
                this.SignPer5.Text = hdj.ConTemp.SignDatas[4].SignEmployee.Name.ToString();
                this.SignPer6.Text = hdj.ConTemp.SignDatas[5].SignEmployee.Name.ToString();
                this.SignPer7.Text = hdj.ConTemp.SignDatas[6].SignEmployee.Name.ToString();
                this.SignPer8.Text = hdj.ConTemp.SignDatas[7].SignEmployee.Name.ToString();
            }
            catch
            {
                MessageBox.Show("加载数据失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Logging.AddLog("重新提交时加载数据失败！");

                if (MessageBox.Show("好吧,我们的程序出现点问题需要重新启动\n点击\"确定\"重启\n点击\"取消\"退出程序？",
                    "程序异常",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    Application.Restart();
                }
                else
                {
                    Application.Exit();
                }
            }
        }


        // 重新提交按钮单机以后，应该进行一次刷新
        // modify by gatieme 2015-07-02 9:47 修改提交方案wield异步
        private  void button1_Click(object sender, EventArgs e)
        {
            if (this.Column1Info.Text.Trim() != "" && this.Column2Info.Text.Trim() != "" &&
                this.Column3Info.Text.Trim() != "" && this.Column4Info.Text.Trim() != "" && this.Column5Info.Text.Trim() != "")
            {
                if (MessageBox.Show("您确定要提交所填方案吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    HDJContract hdj = new HDJContract();
                    hdj.Id = this.ConTempId.Text;

                    List<String> list = new List<string>();
                    list.Add(this.Column1Info.Text.ToString());
                    list.Add(this.Column2Info.Text.ToString());
                    list.Add(this.Column3Info.Text.ToString());
                    list.Add(this.Column4Info.Text.ToString());
                    list.Add(this.Column5Info.Text.ToString());
                    hdj.ColumnDatas = list;

                    string result =  _sc.ModifyContractTemplate(hdj);
                    if (result == Response.MODIFY_HDJCONTRACT_SUCCESS.ToString())
                    {
                        MessageBox.Show("提交成功!", "提示", MessageBoxButtons.OK);

                        //  2015-07-01 14:58 modify by gatieme
                        //  重新提交后，应该设置OK
                        //this.DialogResult = DialogResult.OK;
                        //this.Close();         // 不能关闭，否则会异常
                    }
                    else if (result == "服务器连接中断")
                    {
                        MessageBox.Show("服务器连接中断,提交失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("提交失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }


    }
}
