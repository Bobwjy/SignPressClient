using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using MySql.Data.MySqlClient;


using SignPressServer.SignData;
using SignPressServer.SignTools;

namespace SignPressServer.SignDAL
{
    /*
     * 部门信息的数据库操作接口
     */
    class DALDepartment
    {
        #region  数据库信息串

        /// <summary>
        /// 插入数据库的信息串
        /// </summary>
        private const String INSERT_DEPARTMENT_STR = @"INSERT INTO `department` (`name`) VALUES (@Name)";

        /// <summary>
        /// 删除数据库的信息串
        /// </summary>
        private const String DELETE_DEPARTMENT_ID_STR = @"DELETE FROM `department` WHERE (`id`=@Id)";
        private const String DELETE_DEPARTMENT_NAME_STR = @"DELETE FROM `department` WHERE (`name`=@Name)";
        

        private const String MODIFY_DEPARTMENT_NAME_STR = @"UPDATE `department` SET `name`=@Name WHERE (`id`=@Id)";
        #endregion

        #region  插入部门信息
        /// <summary>
        /// 插入部门信息
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public static bool InsertDepartment(String departmengName)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
          
            MySqlCommand cmd;
            int count = -1;                      // 受影响行数
            try
            {
                con.Open();

                cmd = con.CreateCommand();
                cmd.CommandText = INSERT_DEPARTMENT_STR;
                cmd.Parameters.AddWithValue("@Name", departmengName);                  // 部门职位
                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();
                if (count == 1)     //  插入成功后的受影响行数为1
                {
                    Console.WriteLine("部门插入成功");
                    return true;
                }
                else
                {
                    Console.WriteLine("部门插入失败");
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

        }
        #endregion



        #region 删除部门信息
        /// <summary>
        /// 删除部门的信息DeleteEmployee
        /// </summary>
        /// <param name="id">部门的部门号</param>
        /// <returns></returns>
        public static bool DeleteDepartment(int departmentId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;
            int count = -1;
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = DELETE_DEPARTMENT_ID_STR;
                cmd.Parameters.AddWithValue("@Id", departmentId);                        // 部门姓名


                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();

                if (count == 1)
                {
                    Console.WriteLine("删除部门" + departmentId.ToString() + "成功");
                    return true;
                }
                else
                {
                    Console.WriteLine("删除部门" + departmentId.ToString() + "失败");
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public static bool DeleteDepartment(String departmentName)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;
            int count = -1;
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = DELETE_DEPARTMENT_NAME_STR;
                cmd.Parameters.AddWithValue("@Name", departmentName);                        // 部门姓名


                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();

                if (count == 1)
                {
                    Console.WriteLine("删除部门" + departmentName + "成功");
                    return true;
                }
                else
                {
                    Console.WriteLine("删除部门" + departmentName + "失败");
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        #endregion


        #region 修改部门名称信息
        public static bool ModifyDepartmentName(int departmentId, String departmentName)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;
            int count = -1;
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = MODIFY_DEPARTMENT_NAME_STR;
                cmd.Parameters.AddWithValue("@Id", departmentId);
                cmd.Parameters.AddWithValue("@Name", departmentName);                        // 员工姓名


                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();

                if (count == 1)
                {
                    Console.WriteLine("修改部门名称" + departmentId.ToString() + "成功");

                    return true;
                }
                else
                {
                    Console.WriteLine("修改部门名称" + departmentId.ToString() + "失败");

                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        #endregion
    }
}
