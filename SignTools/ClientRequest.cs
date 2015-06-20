using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignPressServer.SignTools
{
    public enum ClientRequest
    {
        //  员工操作
        LOGIN_REQUEST,  //  登录请求
        QUIT_REQUEST,

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
    }

}
