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
using SignPressClient.SignLogging;

namespace SignPressClient
{
    public partial class SignConTemp : Form
    {
        SignSocketClient _sc;
        string Id;
        int _type;  //1为审核中;2为已拒绝;3为已通过;4为待办列表
        //private OpaqueCommand cmd = new OpaqueCommand();

        public SignConTemp()
        {
            InitializeComponent();
        }

        public SignConTemp(SignSocketClient sc, string ID, int type)
            : this()
        {
            _sc = sc;
            Id = ID;
            _type = type;
        }

        private async void SignConTemp_Load(object sender, EventArgs e)
        {
            //cmd.ShowOpaqueLayer(this, 125, true);
            bool canview = false;
            try
            {
                //this.MinimizeBox = false;
                //this.MaximizeBox = false;

                if (_type == 1)
                {
                    this.RemarkList.Visible = true;
                    this.RemarkList.Height = 140;

                    canview = true;
                }
                if (_type == 2)
                {
                    this.RemarkList.Visible = true;
                    this.RemarkList.Height = 140;

                    canview = true;
                }
                if (_type == 3)
                {
                    this.RemarkList.Visible = true;
                    this.RemarkList.Height = 140;

                    canview = true;
                }
                if (_type == 4)
                {
                    this.Reason.Visible = true;
                    this.AgreeConTemp.Visible = true;
                    this.RefuseConTemp.Visible = true;
                }
                if (_type == 5)
                {
 
                }

                HDJContract hdj = new HDJContract();
                hdj = await  _sc.GetHDJContract(Id);

                this.ConTempName.Text = hdj.Name;
                this.ConTempId.Text = hdj.Id;
                List<string> columnlist = new List<string>();
                columnlist = hdj.ConTemp.ColumnNames;
                this.Column1.Text = columnlist[0].ToString();
                this.Column2.Text = columnlist[1].ToString();
                this.Column3.Text = columnlist[2].ToString();
                this.Column4.Text = columnlist[3].ToString();
                this.Column5.Text = columnlist[4].ToString();

                this.Sign1.Text = hdj.ConTemp.SignDatas[0].SignInfo.ToString();
                this.Sign2.Text = hdj.ConTemp.SignDatas[1].SignInfo.ToString();
                this.Sign3.Text = hdj.ConTemp.SignDatas[2].SignInfo.ToString();
                this.Sign4.Text = hdj.ConTemp.SignDatas[3].SignInfo.ToString();
                this.Sign5.Text = hdj.ConTemp.SignDatas[4].SignInfo.ToString();
                this.Sign6.Text = hdj.ConTemp.SignDatas[5].SignInfo.ToString();
                this.Sign7.Text = hdj.ConTemp.SignDatas[6].SignInfo.ToString();
                this.Sign8.Text = hdj.ConTemp.SignDatas[7].SignInfo.ToString();

                this.Column1Info.Text = hdj.ColumnDatas[0].ToString();
                this.Column2Info.Text = hdj.ColumnDatas[1].ToString(); 
                this.Column3Info.Text = hdj.ColumnDatas[2].ToString();
                this.Column4Info.Text = hdj.ColumnDatas[3].ToString();
                this.Column5Info.Text = hdj.ColumnDatas[4].ToString();

                for (int s = 0; s < hdj.ConTemp.SignDatas.Count; s++)
                {
                    if (hdj.ConTemp.SignDatas[s].SignEmployee.Name == UserHelper.UserInfo.Name)
                    {
                        if (hdj.ConTemp.SignDatas[s].CanView == 1)
                        {
                            canview = true;
                        }
                    }
                }

                if (canview)
                {
                    string result1 = hdj.SignResults[0] == 1 ? "(同意)" : (hdj.SignResults[0] == 0 ? "(未处理)" : "(拒绝)");
                    string result2 = hdj.SignResults[1] == 1 ? "(同意)" : (hdj.SignResults[1] == 0 ? "(未处理)" : "(拒绝)");
                    string result3 = hdj.SignResults[2] == 1 ? "(同意)" : (hdj.SignResults[2] == 0 ? "(未处理)" : "(拒绝)");
                    string result4 = hdj.SignResults[3] == 1 ? "(同意)" : (hdj.SignResults[3] == 0 ? "(未处理)" : "(拒绝)");
                    string result5 = hdj.SignResults[4] == 1 ? "(同意)" : (hdj.SignResults[4] == 0 ? "(未处理)" : "(拒绝)");
                    string result6 = hdj.SignResults[5] == 1 ? "(同意)" : (hdj.SignResults[5] == 0 ? "(未处理)" : "(拒绝)");
                    string result7 = hdj.SignResults[6] == 1 ? "(同意)" : (hdj.SignResults[6] == 0 ? "(未处理)" : "(拒绝)");
                    string result8 = hdj.SignResults[7] == 1 ? "(同意)" : (hdj.SignResults[7] == 0 ? "(未处理)" : "(拒绝)");

                    this.SignPer1.Text = hdj.ConTemp.SignDatas[0].SignEmployee.Name.ToString() + result1;
                    this.SignPer2.Text = hdj.ConTemp.SignDatas[1].SignEmployee.Name.ToString() + result2;
                    this.SignPer3.Text = hdj.ConTemp.SignDatas[2].SignEmployee.Name.ToString() + result3;
                    this.SignPer4.Text = hdj.ConTemp.SignDatas[3].SignEmployee.Name.ToString() + result4;
                    this.SignPer5.Text = hdj.ConTemp.SignDatas[4].SignEmployee.Name.ToString() + result5;
                    this.SignPer6.Text = hdj.ConTemp.SignDatas[5].SignEmployee.Name.ToString() + result6;
                    this.SignPer7.Text = hdj.ConTemp.SignDatas[6].SignEmployee.Name.ToString() + result7;
                    this.SignPer8.Text = hdj.ConTemp.SignDatas[7].SignEmployee.Name.ToString() + result8;

                    this.RemarkList.Visible = true;
                }
                else
                {
                    this.SignPer1.Text = hdj.ConTemp.SignDatas[0].SignEmployee.Name.ToString();
                    this.SignPer2.Text = hdj.ConTemp.SignDatas[1].SignEmployee.Name.ToString();
                    this.SignPer3.Text = hdj.ConTemp.SignDatas[2].SignEmployee.Name.ToString(); 
                    this.SignPer4.Text = hdj.ConTemp.SignDatas[3].SignEmployee.Name.ToString();
                    this.SignPer5.Text = hdj.ConTemp.SignDatas[4].SignEmployee.Name.ToString();
                    this.SignPer6.Text = hdj.ConTemp.SignDatas[5].SignEmployee.Name.ToString();
                    this.SignPer7.Text = hdj.ConTemp.SignDatas[6].SignEmployee.Name.ToString();
                    this.SignPer8.Text = hdj.ConTemp.SignDatas[7].SignEmployee.Name.ToString();

                    this.RemarkList.Visible = false;
                    this.Reason.Location = new Point(35, 524);
                    this.AgreeConTemp.Location = new Point(314, 604);
                    this.RefuseConTemp.Location = new Point(567, 604);
                    this.Height = 710;
                }

                this.RemarkList.Items.Add("备注信息：");
                if (hdj.SignRemarks != null)
                {
                    for (int i = 0; i < hdj.SignRemarks.Count; i++)
                    {
                        if ((hdj.SignRemarks[i]) != "")
                        {
                            this.RemarkList.Items.Add("   " + hdj.ConTemp.SignDatas[i].SignEmployee.Name.ToString() + "：" + hdj.SignRemarks[i].ToString());
                        }
                    }
                }
                if (this.RemarkList.Items.Count == 1)
                {
                    this.RemarkList.Visible = false;
                    this.Reason.Location = new Point(35,524);
                    this.AgreeConTemp.Location = new Point(314, 604);
                    this.RefuseConTemp.Location = new Point(567, 604);
                    this.Height = 710;
                }
                //cmd.HideOpaqueLayer();
            }
            catch
            {
                MessageBox.Show("加载数据失败!");
                Logging.AddLog("查看签单详细信息失败!");

                if (MessageBox.Show("好吧,我们的程序出现点问题需要重新启动\n点击\"确定\"重启\n点击\"取消\"退出程序？",
                    "提示",
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

        private void Reason_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Reason.Text))
            {
                this.Reason.ForeColor = Color.Silver;
                this.Reason.Text = "备注：同意时此栏可不填，拒绝时需填写理由";
            }
        }

        private void Reason_MouseDown(object sender, MouseEventArgs e)
        {
            this.Reason.Text = "";
            this.Reason.ForeColor = Color.Black;
        }

        private  void AgreeConTemp_Click(object sender, EventArgs e)
        {
            SignatureDetail sd = new SignatureDetail();
            sd.ConId = this.ConTempId.Text;
            sd.EmpId = UserHelper.UserInfo.Id;

            if (this.Reason.Text == "备注：同意时此栏可不填，拒绝时需填写理由")
            {
                sd.Remark = "未填";
            }
            else
            {
                sd.Remark = this.Reason.Text.Trim();
            }
            sd.Result = 1;

            string result =  _sc.SignDetail(sd);
            if (result == Response.INSERT_SIGN_DETAIL_SUCCESS.ToString())
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("处理失败，请重新处理！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private  void RefuseConTemp_Click(object sender, EventArgs e)
        {
            if (this.Reason.Text == "备注：同意时此栏可不填，拒绝时需填写理由"
            || string.IsNullOrEmpty(this.Reason.Text))
            {
                MessageBox.Show("需填写拒绝理由！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //return;
            }
            else
            {
                SignatureDetail sd = new SignatureDetail();
                sd.ConId = this.ConTempId.Text;
                sd.EmpId = UserHelper.UserInfo.Id;
                sd.Remark = this.Reason.Text;
                sd.Result = -1;

                string result =  _sc.SignDetail(sd);
                if (result == Response.INSERT_SIGN_DETAIL_SUCCESS.ToString())
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("处理失败，请重新处理！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }


    }
}
