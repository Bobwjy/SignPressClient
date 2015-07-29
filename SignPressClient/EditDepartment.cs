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

namespace SignPressClient
{
    public partial class EditDepartment : Form
    {
        ////   此处应该注意，这里的dpartment跟底层Uderhelper的数据是同一个数据域
        private Department m_department;

        private SignSocketClient _sc;

        public EditDepartment()
        {
            InitializeComponent();
        }


        public EditDepartment(Department department, SignSocketClient sc)
            :this()
        {
            this.m_department = department;
            this._sc = sc;
        }



        private void EditDepartment_Load(object sender, EventArgs e)
        {
            this.textBoxId.Text = this.m_department.Id.ToString();
            this.textBoxName.Text = this.m_department.Name;
        }

        private  void ModifyDepartment_Click(object sender, EventArgs e)
        {
            //this.m_department.Name = this.textBoxName.Text.Trim();
            string departmentname = this.textBoxName.Text.Trim();

            if (departmentname != "")
            {
                if (departmentname == this.m_department.Name)
                {
                    MessageBox.Show("您未对此部门做任何修改!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (UserHelper.DepList.Where(o => o.Name == departmentname).ToList().Count > 0)
                {
                    MessageBox.Show("该部门已经存在!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    ////   此处应该注意，这里的dpartment跟底层Uderhelper的数据是同一个数据域
                    this.m_department.Name = departmentname;            // 
                    ////
                    
                    string result =  _sc.ModifyDepartment(this.m_department);

                    if (result == Response.MODIFY_DEPARTMENT_SUCCESS.ToString())
                    {
                        MessageBox.Show("修改部门成功!", "提示", MessageBoxButtons.OK);
                        //this.m_department.Name = departmentname;
                        this.DialogResult = DialogResult.OK;
                    }
                    else if (result == "服务器连接中断")
                    {
                        MessageBox.Show("服务器连接中断,删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("删除部门失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("请填写部门名称!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
