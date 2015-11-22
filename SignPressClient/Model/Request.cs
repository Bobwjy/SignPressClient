using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressClient.Model
{
    public enum Request
    {
        LOGIN_REQUEST,     //登录请求
        QUIT_REQUEST,           //  退出请求
        INSERT_DEPARTMENT_REQUEST,      //添加部门请求
        QUERY_DEPARTMENT_REQUEST,        //查询所有部门
        QUERY_DEPARTMENT_SHORTCALL_REQUEST,
        QUERY_EMPLOYEE_REQUEST,        //查询所有员工
        INSERT_CONTRACT_TEMPLATE_REQUEST,    //添加模板
        QUERY_CONTRACT_TEMPLATE_REQUEST,      //查询模板
        GET_CONTRACT_TEMPLATE_REQUEST,       //获取特定模板
        DELETE_CONTRACT_TEMPLATE_REQUEST,     //删除特定模板
        MODIFY_CONTRACT_TEMPLATE_REQUEST,      //修改模板信息
        DELETE_DEPARTMENT_REQUEST,              //删除部门信息
        INSERT_HDJCONTRACT_REQUEST,              //添加提交单信息
        QUERY_SIGN_PEND_REQUEST,               //查询正在审批的签单
        QUERY_SIGN_AGREE_REQUEST,              //查询已通过的签单
        QUERY_SIGN_REFUSE_REQUEST,             //查询已拒绝的签单
        QUERY_UNSIGN_CONTRACT_REQUEST,          //查询等待签字的签单
        QUERY_SIGNED_CONTRACT_REQUEST,           //查询已经签字的签单
        GET_HDJCONTRACT_REQUEST,                  //查询会签单信息
        INSERT_SIGN_DETAIL_REQUEST,               //插入签单的状态
        INSERT_EMPLOYEE_REQUEST,                   //添加人员
        UPLOAD_PICTURE_REQUEST,                    //上传图片
        DOWNLOAD_PICTURE_REQUEST,                   //  
        MODIFY_HDJCONTRACT_REQUEST,                    //重新提交方案
        DOWNLOAD_HDJCONTRACT_REQUEST,                   //下载文件
        DELETE_EMPLOYEE_REQUEST,                         //删除员工
        SEARCH_AGREE_HDJCONTRACT_REQUEST,                 //条件查询已通过方案
        SEARCH_SIGNED_HDJCONTRACT_REQUEST,                 //条件查询已办理方案


        MODIFY_EMP_PWD_REQUEST,                  // 重置员工的密码为111
        MODIFY_EMPLOYEE_REQUEST,                //  修改员工信息

        MODIFY_DEPARTMENT_REQUEST,
        QUERY_AGREE_UNDOWN_REQUEST,

        // Modify by gatieme at 2015-08-26 13:44
        QUERY_SDEPARTMENT_REQUEST,
        MODIFY_SDEPARTMENT_REQUEST,
        INSERT_SDEPARTMENT_REQUEST,              //添加部门权限
        QUERY_SDEPARTMENT_CATEGORY_REQUEST,
        QUERY_CATEGORY_PROJECT_REQUEST
    }
}
