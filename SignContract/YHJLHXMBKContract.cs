using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// 引入会签单模版类
using SignPressServer.SignData;

namespace SignPressServer.SignContract
{
    /****
     * 
     *  养护及例会项目拨款会签审批单
     *  
     *  此类继承自签字模版类ContractTemplate
     *  签字模版中只包含了该有的签字的基本数据项，
     *  现实中就对应了一张签字的空白表格
     *  
     *  而YHLLHXMBKContract（养护及例会项目拨款会签审批单）
     *  则对应了客户申请时，填写的数据项
     *  
     *  但是应该注意
     *  签字单子在确认为一个模版后，那么他的签字顺序就已经确定了
     *  提交申请的人就只能按照提交签字的顺序走，
     *  因此签字人的的顺序是在模版中的
     * 
     *  [问题2015/6/14 10:19]
     *  我们将签字也设计进模版了
     *  一个单子的申请人应该也需要签字，很有可能是第一个人或者前两个人
     *  那么如果这两个人也设计进入模版中的话，那么每个申请人都会需要一个模版
     *  这样子后面的几个人在模版中就显得非常冗余
     *  比如张三和李四分别提交的会签单中，可能只是前两个人不一样，后面的人都是一个样式
     *  这样级要考虑是否需要将前面两个人设计进入签字模版
     *  
     *  如果真的前两个人即是申请人又是签字人，有一下设计方案
     *  [1] 把这类人员设计进签字模版，那么同样一张表可能不同的申请人就得设计不同的模版
     *  这样同样一份一个单子模版就有好几类，对应每一个提交人就有一个模版，模版很冗余
     *  而且只能在提交人提交单子的时候选择模版。
     *  这样一来的话， 让一个提交人员可以看到或者提交别的提交人的模版是很不好的
     *  一种解决方案是提交单子的时候，判断用户，只提交自己的单子，
     *  
     *  [2] 把后面的6个人设计进签字模版，前面两个人由提交人员指定，
     *  这里面有个优化方案，就是提交的时候用户后面的签字顺序是定死的，
     *  前面两个应该有一个是自己，有一个是自己的直接领导，
     *  自己已经无需指定，直接同意即可
     * 
     *  目前暂时采用第一种方案，因为麻烦点，麻烦点，但是可以排除提交人员不是签字人的情况出现的大BUG

     ****/
    class YHJLHXMBKContract :  ContractTemplate
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        public YHJLHXMBKContract()
            :base()
        { 
            ///this.m_tempId = ///
            ///this.TempId = 
            
            ///  会签单名称
            this.Name = "养护及例会项目拨款会签审批单";
            
            /// 5个基本项
            this.ColumnData.Add("工程名称");
            this.ColumnData.Add("项目名称");
            this.ColumnData.Add("主要项目及工程量");
            this.ColumnData.Add("本次申请资金额度（元）");
            this.ColumnData.Add("累计申请资金额度（元）");
            
            /// 8个签字信息项
        }
        
        private String m_id;            //  审批会签单编号
        public String Id
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        private String m_proName;      //  工程名称
        public String ProName
        {
            get { return this.m_proName; }
            set { this.m_proName = value; }
        }

        private String m_termName;      //  项目名称
        public String TermName
        {
            get { return this.m_termName; }
            set { this.m_termName = value; }
        }

        private String m_termSize;      //  主要项目以及工作量(养护及例会项目拨款会签审批单)
        public String TermSize
        {
            get { return this.m_termName; }
            set { this.m_termSize = value; }
        }

        private int m_reqCaptial;      //  本次申请资金额度（元）(养护及例会项目拨款会签审批单--)
        public int ReqCapial
        {
            get { return this.m_reqCaptial; }
            set { this.m_reqCaptial = value; }
        }

        private int m_totalCaptial;       //  累计申请资金额度（元）
        public int TotalCaptial
        {
            get { return this.m_totalCaptial; }
            set { this.m_totalCaptial = value; }
        }


        private int m_submitId;                 //  提交会签单的人名单
        public int SubmitId
        {
            get { return this.m_submitId; }
            set { this.m_submitId = value; }
        }
        /*  以下字段考虑是用员工数类表示还是用员工表的主键来表示
        private int m_reqDepartProId;           // 申请单位项目负责人的员工编号
        public int ReqDepartProId
        {
            get { return this.m_reqDepartProId; }
            set { this.m_reqDepartProId = value; }
        }

        private int m_reqDepartId;              //  申请单位负责人的员工编号
        public int ReqDepartId
        {
            get { return this.m_reqDepartId; }
            set { this.m_reqDepartId = value; }
        }
        
        
        private int m_conDepartProId;           //  养护主管部门项目负责人（需要签字）
        public int ConDepartProId
        {
            get { return this.m_conDepartProId; }
            set { this.m_conDepartProId = value; }
        }
        
        private int m_conDepartId;              //  养护主管部门负责人（需要签字）
        public int ConDepartId
        {
            get { return this.m_conDepartProId; }
            set { this.m_conDepartProId = value; }
        }
        
        private int m_planDepartId;             //  计划科负责人（需要签字）
        public int PlanDepartI
        {
            get { return this.m_planDepartId; }
            set { this.m_planDepartId = value; }
        }
        
        private int m_finaDepartId;             //  财务科负责人（需要签字）
        public int FinalDepartId
        {
            get { return this.m_finaDepartId; }
            set { this.m_finaDepartId = value; }
        }

        private int m_                      ·//  副局长
        private int m_director               //  局长
        */
    }
}
