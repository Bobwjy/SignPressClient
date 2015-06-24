using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignPressServer.SignTools
{
    public enum ServerResponse
    {
        LOGIN_SUCCESS,          //  登录成功响应
        LOGIN_FAILED,           //  登录失败响应

        QUIT_SUCCESS,           //  退出请求
        QUIT_FAILED,           //  退出请求


        // 用户文件操作
        DOWNLOAD_HDJCONTRACT_SUCCESS,   //  下载会签单的信息
        DOWNLOAD_HDJCONTRACT_FAILED,   //  下载会签单的信息

        UPLOAD_SIGN_PIC_SUCCESS,        //  上传签字图片
        UPLOAD_SIGN_PIC_FAILED,        //  上传签字图片


        /// <summary>
        /// ==部门操作==
        /// 增加部门  INSERT_DEPARTMENT_REQUEST
        /// 删除部门  DELETE_DEPARTMENT_REQUEST
        /// 修改部门  MODIFY_DEPARTMENT_REQUEST
        /// 查询部门  QUERY_DEPARTMENT_REQUEST
        /// </summary>
        INSERT_DEPARTMENT_SUCCESS,
        INSERT_DEPARTMENT_FAILED,

        DELETE_DEPARTMENT_SUCCESS,
        DELETE_DEPARTMENT_FAILED,
        
        MODIFY_DEPARTMENT_SUCCESS,
        MODIFY_DEPARTMENT_FAILED,
        
        QUERY_DEPARTMENT_SUCCESS,
        QUERY_DEPARTMENT_FAILED,

        /// <summary>
        /// ==员工操作==
        /// 增加员工  INSERT_EMPLOYEE_REQUEST
        /// 删除员工  DELETE_EMPLOYEE_REQUEST
        /// 修改员工  MODIFY_EMPLOYEE_REQUEST
        /// 查询员工  QUERY_ERMPLOYEE_REQUEST
        /// </summary>
        INSERT_EMPLOYEE_SUCCESS,        /// </summary>
        INSERT_EMPLOYEE_FAILED,

        DELETE_EMPLOYEE_SUCCESS,
        DELETE_EMPLOYEE_FAILED,
        
        MODIFY_EMPLOYEE_SUCCESS,
        MODIFY_EMPLOYEE_FAILED,
        
        QUERY_EMPLOYEE_SUCCESS,
        QUERY_EMPLOYEE_FAILED,

        /// <summary>
        /// ==会签单模版操作==
        /// 增加会签单模版  INSERT_CONTRACT_TEMPLATE_REQUEST
        /// 删除会签单模版  DELETE_CONTRACT_TEMPLATE_REQUEST
        /// 修改会签单模版  MODIFY_CONTRACT_TEMPLATE_REQUEST
        /// 查询会签单模版  QUERY_CONTRACT_TEMPLATE_REQUEST
        /// 获取会签单模版  GET_CONTRACT_TEMPLATE_REQUEST
        /// </summary>
        INSERT_CONTRACT_TEMPLATE_SUCCESS,
        INSERT_CONTRACT_TEMPLATE_FAILED,

        DELETE_CONTRACT_TEMPLATE_SUCCESS,
        DELETE_CONTRACT_TEMPLATE_FAILED,
        
        MODIFY_CONTRACT_TEMPLATE_SUCCESS,
        MODIFY_CONTRACT_TEMPLATE_FAILED,
        
        QUERY_CONTRACT_TEMPLATE_SUCCESS,
        QUERY_CONTRACT_TEMPLATE_FAILED,

        GET_CONTRACT_TEMPLATE_SUCCESS,
        GET_CONTRACT_TEMPLATE_FAILED,
                /// <summary>
        /// ==航道局会签单操作==
        /// 增加会签单模版  INSERT_HDJCONTRACT_REQUEST
        /// 删除会签单模版  DELETE_HDJCONTRACT_REQUEST
        /// 修改会签单模版  MODIFY_HDJCONTRACT_REQUEST
        /// 查询会签单模版  QUERY_HDJCONTRACT_REQUEST
        /// </summary>
        INSERT_HDJCONTRACT_SUCCESS,
        INSERT_HDJCONTRACT_FAILED,

        DELETE_HDJCONTRACT_SUCCESS,
        DELETE_HDJCONTRACT_FAILED,
        
        MODIFY_HDJCONTRACT_SUCCESS,
        MODIFY_HDJCONTRACT_FAILED,

        QUERY_HDJCONTRACT_SUCCESS,
        QUERY_HDJCONTRACT_FAILED,

        GET_HDJCONTRACT_SUCCESS,
        GET_HDJCONTRACT_FAILED,

        /// <summary>
        /// 提交人查询会签单状态操作
        /// </summary>
        QUERY_SIGN_PEND_SUCCESS,
        QUERY_SIGN_PEND_FAILED,

        QUERY_SIGN_AGREE_SUCCESS,
        QUERY_SIGN_AGREE_FAILED,

        QUERY_SIGN_REFUSE_SUCCESS,
        QUERY_SIGN_REFUSE_FAILED,

        /// <summary>
        /// 签字人查询会签单状态操作
        /// </summary>
        QUERY_UNSIGN_CONTRACT_SUCCESS,
        QUERY_UNSIGN_CONTRACT_FAILED,

        QUERY_SIGNED_CONTRACT_SUCCESS,
        QUERY_SIGNED_CONTRACT_FAILED,

        INSERT_SIGN_DETAIL_SUCCESS,
        INSERT_SIGN_DETAIL_FAILED,

        QUERY_SIGN_DETAIL_SUCCESS,
        QUERY_SIGN_DETAIL_FAILED,

        QUERY_SIGN_DETAIL_CON_SUCCESS,
        QUERY_SIGN_DETAIL_CON_FAILED,
        
    }
}
