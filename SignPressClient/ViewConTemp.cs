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
    public partial class ViewConTemp : Form
    {
        int Id;
        SignSocketClient _sc;

        public ViewConTemp()
        {
            InitializeComponent();
        }

        public ViewConTemp(int ConTempId,SignSocketClient sc)
            : this()
        {
            Id = ConTempId;
            _sc = sc;
        }

        private  void ViewConTemp_Load(object sender, EventArgs e)
        {
            try
            {
                Templete temp = new Templete();
                temp =  _sc.GetContractTemplate(Id);

                this.ConTempName.Text = temp.Name.ToString();
                this.Column1.Text = temp.ColumnNames[0].ToString();
                this.Column2.Text = temp.ColumnNames[1].ToString();
                this.Column3.Text = temp.ColumnNames[2].ToString();
                this.Column4.Text = temp.ColumnNames[3].ToString();
                this.Column5.Text = temp.ColumnNames[4].ToString();

                this.Sign1.Text = temp.SignDatas[0].SignInfo.ToString();
                this.Sign2.Text = temp.SignDatas[1].SignInfo.ToString();
                this.Sign3.Text = temp.SignDatas[2].SignInfo.ToString();
                this.Sign4.Text = temp.SignDatas[3].SignInfo.ToString();
                this.Sign5.Text = temp.SignDatas[4].SignInfo.ToString();
                this.Sign6.Text = temp.SignDatas[5].SignInfo.ToString();
                this.Sign7.Text = temp.SignDatas[6].SignInfo.ToString();
                this.Sign8.Text = temp.SignDatas[7].SignInfo.ToString();

                this.SignPer1.Text = temp.SignDatas[0].SignEmployee.Name.ToString();
                this.SignPer2.Text = temp.SignDatas[1].SignEmployee.Name.ToString();
                this.SignPer3.Text = temp.SignDatas[2].SignEmployee.Name.ToString();
                this.SignPer4.Text = temp.SignDatas[3].SignEmployee.Name.ToString();
                this.SignPer5.Text = temp.SignDatas[4].SignEmployee.Name.ToString();
                this.SignPer6.Text = temp.SignDatas[5].SignEmployee.Name.ToString();
                this.SignPer7.Text = temp.SignDatas[6].SignEmployee.Name.ToString();
                this.SignPer8.Text = temp.SignDatas[7].SignEmployee.Name.ToString();
            }
            catch
            {
                MessageBox.Show("加载数据失败!");
                Logging.AddLog("查看模板详细信息失败!");

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


    }
}
