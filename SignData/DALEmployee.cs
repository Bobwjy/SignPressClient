using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using SignPressServer.SignData;

using System.Data;
using MySql.Data.MySqlClient;


namespace SignPressServer.SignDAL
{
    /*
     *  员工类的数据库操作接口 
     *
     */
    class DALEmployee
    {
        /// <summary>
        /// 向数据库中添加人员， 并返回其编号信息
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>

        #region  数据库信息串
        /// <summary>
        /// 连接数据库的信息串
        /// </summary>

        private const String MYSQL_SQLCON_STR = "server=localhost;user id=root;password=root;database=signature"; //根据自己的设置

        /// <summary>
        /// 插入员工信息的信息串
        /// </summary>
        private const String INSERT_EMPLOYEE_STR = @"INSERT INTO `employee` (`name`, `position`, `depart`, `username`, `password`) 
                                                VALUES (@Name, @Position, @Department, @Username, @Password)";

        /// <summary>
        /// 插入员工信息的信息串
        /// </summary>
        private const String DELETE_EMPLOYEE_STR = @"DELETE FROM `employee` WHERE (`id`=@Id)";

        /// <summary>
        /// 判断用户名密码是否正确
        /// </summary>
        private const String LOGIN_EMPLOYEE_STR = @"SELECT * FROM `employee` WHERE(`username` = @Username and `password` = @Password)";
        #endregion

        #region  插入员工信息
        /// <summary>
        /// 插入员工信息
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public static bool InsertEmployee(Employee employee)
        {
            MySqlConnection con = new MySqlConnection(MYSQL_SQLCON_STR);
            MySqlCommand cmd;
            int count = -1;                      // 受影响行数
            try
            {
                con.Open();

                cmd = con.CreateCommand();
                cmd.CommandText = INSERT_EMPLOYEE_STR;
                cmd.Parameters.AddWithValue("@Name", employee.Name);                        // 员工姓名
                cmd.Parameters.AddWithValue("@Position", employee.Position);                  // 员工职位
                cmd.Parameters.AddWithValue("@Department", employee.Department.Id);         // 员工部门编号
                cmd.Parameters.AddWithValue("@Username", employee.User.Username);                // 员工登录用户名
                cmd.Parameters.AddWithValue("@Password", employee.User.Password);                // 员工密码

                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();
                if (count == 1)     //  插入成功后的受影响行数为1
                {
                    return true;
                }
                else
                {
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



        #region 删除员工信息
        /// <summary>
        /// 删除员工的信息DeleteEmployee
        /// </summary>
        /// <param name="id">员工的员工号</param>
        /// <returns></returns>
        public static bool DeleteEmployee(int employeeID)
        {
            MySqlConnection con = new MySqlConnection(MYSQL_SQLCON_STR);
            MySqlCommand cmd;
            int count = -1;
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = DELETE_EMPLOYEE_STR;
                cmd.Parameters.AddWithValue("@Id", employeeID);                        // 员工姓名


                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();

                if (count == 1)
                {
                    return true;
                }
                else
                {
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

        #region 处理用户的登录
        public static bool LoginEmployee(String username, String password)
        {
            MySqlConnection con = new MySqlConnection(MYSQL_SQLCON_STR);
            MySqlCommand cmd;
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = LOGIN_EMPLOYEE_STR;
                cmd.Parameters.AddWithValue("@Username", username);                         // 员工登录用户名
                cmd.Parameters.AddWithValue("@Password", password);                         // 员工登录密码

                MySqlDataReader sqlRead = cmd.ExecuteReader( );
                
                cmd.Dispose();


                /*while (sqlRead.Read( ))
                {
                    Console.WriteLine(sqlRead["id"].ToString() + "  " + sqlRead["name"].ToString());
                }*/
                con.Close();
                con.Dispose();

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
            return true;
        }
        #endregion

        #region 测试增加用户
        public void TestInsertEmployee()
        {
            Employee em = new Employee
            {
                Id = 9,
                Name = "王盼盼",
                Position = "局长",
                Department = new Department { Id = 5, Name = "行政科" },
                User = new User { Username = "wangpanpan", Password = "wangpanpan" }
            };
            DALEmployee.InsertEmployee(em);
        }
        #endregion



    }
}
