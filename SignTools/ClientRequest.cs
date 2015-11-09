using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignPressServer.SignTools
{
    public enum ClientRequest
    {
        //  员工操作
        LOGIN_REQUEST,          //  登录请求
        QUIT_REQUEST,           //  退出请求
        DOWNLOAD_HDJCONTRACT,   //  下载会签单的信息
        UPLOAD_SIGN_PIC,        //  上传签字图片

        /// <summary>
        /// ==部门操作==
        /// 增加部门  INSERT_DEPARTMENT_REQUEST
        /// 删除部门  DELETE_DEPARTMENT_REQUEST
        /// 修改部门  MODIFY_DEPARTMENT_REQUEST
        /// 查询部门  QUERY_DEPARTMENT_REQUEST
        /// </summary>
        INSERT_DEPARTMENT_REQUEST,
        DELETE_DEPARTMENT_REQUEST,
        MODIFY_DEPARTMENT_REQUEST,
        QUERY_DEPARTMENT_REQUEST,
        INSERT_SDEPARTMENT_REQUEST,
        QUERY_SDEPARTMENT_REQUEST,
        MODIFY_SDEPARTMENT_REQUEST,
        /// <summary>
        /// ==员工操作==
        /// 增加员工  INSERT_EMPLOYEE_REQUEST
        /// 删除员工  DELETE_EMPLOYEE_REQUEST
        /// 修改员工  MODIFY_EMPLOYEE_REQUEST
        /// 查询员工  QUERY_ERMPLOYEE_REQUEST
        /// </summary>
        INSERT_EMPLOYEE_REQUEST,
        DELETE_EMPLOYEE_REQUEST,
        MODIFY_EMPLOYEE_REQUEST,
        QUERY_EMPLOYEE_REQUEST,
        MODIFY_EMP_PWD_REQUEST,

        /// <summary>
        /// ==会签单模版操作==
        /// 增加会签单模版  INSERT_CONTRACT_TEMPLATE_REQUEST
        /// 删除会签单模版  DELETE_CONTRACT_TEMPLATE_REQUEST
        /// 修改会签单模版  MODIFY_CONTRACT_TEMPLATE_REQUEST
        /// 查询会签单模版  QUERY_CONTRACT_TEMPLATE_REQUEST
        /// </summary>
        INSERT_CONTRACT_TEMPLATE_REQUEST,
        DELETE_CONTRACT_TEMPLATE_REQUEST,
        MODIFY_CONTRACT_TEMPLATE_REQUEST,
        QUERY_CONTRACT_TEMPLATE_REQUEST,
        GET_CONTRACT_TEMPLATE_REQUEST,

        /// <summary>
        /// ==航道局会签单操作==
        /// 增加会签单模版  INSERT_HDJCONTRACT_REQUEST
        /// 删除会签单模版  DELETE_HDJCONTRACT_REQUEST
        /// 修改会签单模版  MODIFY_HDJCONTRACT_REQUEST
        /// 查询会签单模版  QUERY_HDJCONTRACT_REQUEST
        /// </summary>
        INSERT_HDJCONTRACT_REQUEST,
        DELETE_HDJCONTRACT_REQUEST,
        MODIFY_HDJCONTRACT_REQUEST,
        QUERY_HDJCONTRACT_REQUEST,
        GET_HDJCONTRACT_REQUEST,


        /// <summary>
        /// 查询会签单状态操作
        /// </summary>
        QUERY_SIGN_PEND_REQUEST,
        QUERY_SIGN_AGREE_REQUEST,
        QUERY_SIGN_REFUSE_REQUEST,


        /// <summary>
        /// 查询会签单状态操作
        /// QUERY_UNSIGN_CONTRACT_REQUEST 签字人查询自己需要签字中的会签单
        /// QUERY_SIGNED_CONTRACT_REQUEST 签字人查询自己已经签完字的会签单
        /// 
        /// INSERT_SIGN_DETAIL_REQUEST    签字人进行签字
        /// QUERY_SIGN_DETAIL_REQUEST     签字人查询自己的签字明细  
        /// QUERY_SIGN_DETAIL_CON_REQUEST 签字人查询自己针对与某个单子的签字明细
        /// </summary>
        QUERY_UNSIGN_CONTRACT_REQUEST,
        QUERY_SIGNED_CONTRACT_REQUEST,
    
        INSERT_SIGN_DETAIL_REQUEST,
        QUERY_SIGN_DETAIL_REQUEST,
        QUERY_SIGN_DETAIL_CON_REQUEST,

        QUERY_SIGNED_CONTRACT_TOP_REQUEST,


        //  查询当前部门可以申请的会签单的类别
        QUERY_SDEPARTMENT_CATEGORY_REQUEST,
        MODIFY_SDEPARTMENT_SREQUEST,

        ///
        QUERY_CATEGORY_PROJECT_REQUEST,         /// 查询当前工程类别下的所有项目类别
        QUERY_PROJECT_ITEM_REQUEST,             /// 查询当前项目类别下的所有工作量集合
        QUERY_CONTRACT_WORKLOAD_REQUEST,            /// 查询当前会签单的所有工作量集合  
    }

}
