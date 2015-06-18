﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignPressServer.SignTools
{
    public enum ServerResponse
    {
        LOGIN_SUCCESS,          //  登录成功响应
        LOGIN_FAILED,           //  登录失败响应

        

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
        /// </summary>
        INSERT_CONTRACT_TEMPLATE_SUCCESS,
        INSERT_CONTRACT_TEMPLATE_FAILED,

        DELETE_CONTRACT_TEMPLATE_SUCCESS,
        DELETE_CONTRACT_TEMPLATE_FAILED,
        
        MODIFY_CONTRACT_TEMPLATE_SUCCESS,
        MODIFY_CONTRACT_TEMPLATE_FAILED,
        
        QUERY_CONTRACT_TEMPLATE_SUCCESS,
        QUERY_CONTRACT_TEMPLATE_FAILED,
    }
}
