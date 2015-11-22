using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressClient.Model
{
    public enum Response
    {
        LOGIN_SUCCESS,    //登录成功
        LOGIN_FAILED,     //登录失败
        UnConnection,  //无法连接

        INSERT_DEPARTMENT_SUCCESS,   //添加部门成功

        QUERY_DEPARTMENT_SUCCESS,       //查询部门成功
        DELETE_DEPARTMENT_SUCCESS,      //删除部门成功  
        QUERY_DEPARTMENT_SHORTCALL_SUCCESS,
        QUERY_DEPARTMENT_SHORTCALL_FAILED,

        QUERY_EMPLOYEE_SUCCESS,     //查询部门员工成功
        INSERT_EMPLOYEE_SUCCESS,    //添加部门员工成功
        INSERT_EMPLOYEE_FAILED,
        INSERT_EMPLOYEE_EXIST,
        DELETE_EMPLOYEE_SUCCESS,    //删除部门员工成功

        INSERT_CONTRACT_TEMPLATE_SUCCESS,       //添加模板成功
        QUERY_CONTRACT_TEMPLATE_SUCCESS,         //查询模板成功
        GET_CONTRACT_TEMPLATE_SUCCESS,          //获取特定模板成功
        DELETE_CONTRACT_TEMPLATE_SUCCESS,        //删除特定模板成功
        MODIFY_CONTRACT_TEMPLATE_SUCCESS,         //修改模板信息成功                 
        INSERT_HDJCONTRACT_SUCCESS,                //添加签单成功

        QUERY_SIGN_PEND_SUCCESS,               //查询正在审批的方案
        QUERY_SIGN_AGREE_SUCCESS,              //查询已通过的方案
        QUERY_SIGN_REFUSE_SUCCESS,              //查询已拒绝的方案

        QUERY_UNSIGN_CONTRACT_SUCCESS,           //查询等待签单列表成功
        QUERY_SIGNED_CONTRACT_SUCCESS,            //查询已签字列表成功
        GET_HDJCONTRACT_SUCCESS,                  //获取会签单成功
        INSERT_SIGN_DETAIL_SUCCESS,                //签单签字成功
        UPLOAD_PICTURE_SUCCESS,                     //上传图片成功
        MODIFY_HDJCONTRACT_SUCCESS,                    //重新提交方案成功

        SEARCH_AGREE_HDJCONTRACT_SUCCESS,               //条件查询已通过方案成功
        SEARCH_SIGNED_HDJCONTRACT_SUCCESS,               //条件查询已办理方案成功

        MODIFY_EMP_PWD_SUCCESS,                          //  重置用户密码成功
        MODIFY_EMP_PWD_FAILED,                           //  重置用户密码失败

        MODIFY_EMPLOYEE_SUCCESS,                //  修改员工信息
        MODIFY_EMPLOYEE_FAILED,                //  修改员工信息

        MODIFY_DEPARTMENT_SUCCESS,
        MODIFY_DEPARTMENT_FAILED,

        QUERY_AGREE_UNDOWN_SUCCESS,
        QUERY_AGREE_UNDOWN_FAILED,

        DELETE_DEPARTMENT_EXIST_EMPLOYEE,              //该部门存在员工无法删除
        DELETE_EMPLOYEE_EXIST_CONTRACT,     //  待删除的员工存在会签单信息无法删除
        DELETE_EMPLOYEE_EXIST_CONTEMP,      //  待删除的员工在某个会签模版中，无法删除
        DELETE_CONTRACT_TEMPLATE_EXIST_CONTRACT,
        INSERT_HDJCONTRACT_EXIST,



        // Modify by gatieme at 2015-08-26 13:44
        QUERY_SDEPARTMENT_SUCCESS,
        QUERY_SDEPARTMENT_FAILED,

        // Modify by gatieme at 2015-09-07 10:51
        MODIFY_SDEPARTMENT_SUCCESS,
        MODIFY_SDEPARTMENT_FAILED,
        INSERT_SDEPARTMENT_SUCCESS,             //添加部门权限成功
        QUERY_SDEPARTMENT_CATEGORY_SUCCESS,
        QUERY_CATEGORY_PROJECT_SUCCESS
    }
}
