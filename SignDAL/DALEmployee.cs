using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




using SignPressServer.SignData;



using System.Data;
using MySql.Data.MySqlClient;
using SignPressServer.SignTools;

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
        /// 插入员工信息的信息串
        /// </summary>
        private const String INSERT_EMPLOYEE_STR = @"INSERT INTO `employee` (`name`, `position`, `departmentid`, `username`, `password`) 
                                                VALUES (@Name, @Position, @Department, @Username, @Password)";

        /// <summary>
        /// 插入员工信息的信息串
        /// </summary>
        private const String DELETE_EMPLOYEE_STR = @"DELETE FROM `employee` WHERE (`id`=@Id)";

        /// <summary>
        /// 判断用户名密码是否正确
        /// </summary>
        private const String LOGIN_EMPLOYEE_SIM_STR = @"SELECT id FROM `employee` WHERE(`username` = @Username and `password` = @Password) ORDER BY id";
        private const String LOGIN_EMPLOYEE_COM_STR = @"SELECT e.id id, e.name name, e.position position, e.cansubmit cansubmit, e.cansign cansign, e.isadmin isadmin,
                                                               d.id departid, d.name departname, 
                                                               e.username username, e.password password
                                                        FROM `employee`  e, `department` d 
                                                        WHERE (`username` = @Username and `password` = @Password and e.departmentid = d.id)
                                                        ORDER BY id";

        /// <summary>
        /// 修改密码的信息串
        /// </summary>
        private const String MODIFY_EMPLOYEE_PASSWORD_STR = @"UPDATE `employee` SET `password` = @Password WHERE (`id` = @Id)";

        private const String MODIFY_EMPLOYEE_ID_STR = @"UPDATE `employee` 
                                                        SET `name` = @Name, `position` = @Position, `departmentid` = @DepartmentId, 
                                                            `cansubmit` = CanSubmit, `cansign` = @CanSign, `isadmin` = IsAdmin, 
                                                            `username` = @Username, `password` = @Password 
                                                        WHERE (`id` = @Id)";
        /// <summary>
        /// 获取员工信息的信息串
        /// </summary>

        private const String GET_EMPLOYEE_ID_STR = @"SELECT e.id id, e.name name, e.position position, e.cansubmit cansubmit, e.cansign cansign, e.isadmin isadmin,
                                                         d.id departid, d.name departname, 
                                                         e.username username, e.password password
                                                  FROM `employee`  e, `department` d 
                                                  WHERE (e.id = @Id and e.departmentid = d.id)";

        private const String GET_EMPLOYEE_USERNAME_STR = @"SELECT e.id id, e.name name, e.position position, e.cansubmit cansubmit, e.cansign cansign, e.isadmin isadmin,
                                                         d.id departid, d.name departname, 
                                                         e.username username, e.password password
                                                  FROM `employee`  e, `department` d 
                                                  WHERE (e.username = @Username and e.departmentid = d.id)";


        /// <summary>
        /// 查询所有员工的信息串
        /// </summary>
        private const String QUERY_ALL_EMPLOYEE_STR = @"SELECT e.id id, e.name name, e.position position, e.cansubmit cansubmit, e.cansign cansign, e.isadmin isadmin,
                                                         d.id departid, d.name departname, 
                                                         e.username username, e.password password
                                                  FROM `employee`  e, `department` d 
                                                  WHERE (e.departmentid = d.id)
                                                  ORDER BY e.id";

        private const String QUERY_DEPARTMENT_EMPLOYEE_ID_STR = @"SELECT e.id id, e.name name, e.position position, e.cansubmit cansubmit, e.cansign cansign, e.isadmin isadmin,
                                                                      d.id departid, d.name departname, 
                                                                      e.username username, e.password password
                                                               FROM `employee`  e, `department` d 
                                                               WHERE (e.departmentid = d.id and d.id = @DepartmentId)
                                                               ORDER BY e.id";

        private const String QUERY_DEPARTMENT_EMPLOYEE_NAME_STR = @"SELECT e.id id, e.name name, e.position position, e.cansubmit cansubmit, e.cansign cansign, e.isadmin isadmin,
                                                                      d.id departid, d.name departname, 
                                                                      e.username username, e.password password
                                                               FROM `employee`  e, `department` d 
                                                               WHERE (e.departmentid = d.id and d.name = @DepartmentName)
                                                               ORDER BY e.id";

        /// <summary>
        /// 查询所有员工的签名图片存放地址的信息串
        /// </summary>

        #endregion

        #region  插入员工信息
        /// <summary>
        /// 插入员工信息
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public static bool InsertEmployee(Employee employee)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
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
                    Console.WriteLine("用户插入成功");
                    return true;
                }
                else
                {
                    Console.WriteLine("用户插入失败");
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
        public static bool DeleteEmployee(int employeeId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;
            int count = -1;
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = DELETE_EMPLOYEE_STR;
                cmd.Parameters.AddWithValue("@Id", employeeId);                        // 员工姓名


                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();

                if (count == 1)
                {
                    Console.WriteLine("删除用户" + employeeId.ToString() + "成功");
                    return true;
                }
                else
                {
                    Console.WriteLine("删除用户" + employeeId.ToString() + "失败");
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
        public static int LoginEmployee(String username, String password)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;
            int result = -1;
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = LOGIN_EMPLOYEE_SIM_STR;
                cmd.Parameters.AddWithValue("@Username", username);                         // 员工登录用户名
                cmd.Parameters.AddWithValue("@Password", password);                         // 员工登录密码

                MySqlDataReader sqlRead = cmd.ExecuteReader();

                cmd.Dispose();


                /*while (sqlRead.Read( ))
                {}*/
                if (sqlRead.Read())
                {
                    Console.WriteLine(sqlRead["id"].ToString() + "  " + sqlRead["name"].ToString());
                    result = int.Parse(sqlRead["id"].ToString());
                }
                else
                {
                    result = -1;
                }

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
            return result;
        }

        public static Employee LoginEmployee(User user)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;
            Employee employee = new Employee();
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = LOGIN_EMPLOYEE_COM_STR;
                cmd.Parameters.AddWithValue("@Username", user.Username);                         // 员工登录用户名
                cmd.Parameters.AddWithValue("@Password", user.Password);                         // 员工登录密码
                MySqlDataReader sqlRead = cmd.ExecuteReader();

                cmd.Dispose();


                /*while (sqlRead.Read( ))
                {}*/
                if (sqlRead.Read())
                {
                    // 基本信息
                    employee.Id = int.Parse(sqlRead["id"].ToString());
                    employee.Name = sqlRead["name"].ToString();
                    employee.Position = sqlRead["position"].ToString();
                    employee.CanSubmit = int.Parse(sqlRead["cansubmit"].ToString());
                    employee.CanSign = int.Parse(sqlRead["cansign"].ToString());
                    employee.IsAdmin = int.Parse(sqlRead["isadmin"].ToString());

                    // 职位信息
                    Department depart = new Department();
                    depart.Id = int.Parse(sqlRead["departid"].ToString());
                    depart.Name = sqlRead["departname"].ToString();
                    employee.Department = depart;

                    // 用户登录信息
                    employee.User = user;

                }
                else
                {
                    employee.Id = -1;
                }
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
            return employee;
        }
        #endregion



        #region 修改密码信息
        public static bool ModifyEmployeePassword(int employeeId, String password)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;
            int count = -1;
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = MODIFY_EMPLOYEE_PASSWORD_STR;
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Id", employeeId);                        // 员工姓名


                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

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
            if (count == 1)
            {
                Console.WriteLine("修改密码" + employeeId.ToString() + "成功");

                return true;
            }
            else
            {
                Console.WriteLine("修改密码" + employeeId.ToString() + "失败");

                return false;
            }
        }
        #endregion


        #region 获取用户的具体信息
        public static Employee GetEmployee(int employeeId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;
            Employee employee = new Employee(); ;          //  待返回的员工信息

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = GET_EMPLOYEE_ID_STR;
                cmd.Parameters.AddWithValue("@Id", employeeId);                         // 员工编号

                MySqlDataReader sqlRead = cmd.ExecuteReader();

                cmd.Dispose();


                if (sqlRead.Read())
                {

                    employee.Id = int.Parse(sqlRead["id"].ToString());
                    employee.Name = sqlRead["name"].ToString();
                    employee.Position = sqlRead["position"].ToString();

                    employee.CanSubmit = int.Parse(sqlRead["cansubmit"].ToString());
                    employee.CanSign = int.Parse(sqlRead["cansign"].ToString());
                    employee.IsAdmin = int.Parse(sqlRead["isadmin"].ToString());

                    Department depart = new Department();
                    depart.Id = int.Parse(sqlRead["departid"].ToString());
                    depart.Name = sqlRead["departname"].ToString();
                    employee.Department = depart;

                    User user = new User();
                    user.Username = sqlRead["username"].ToString();
                    user.Password = sqlRead["password"].ToString();
                    employee.User = user;
                }

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
            return employee;
        }

        public static Employee GetEmployee(String employeeUsername)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;
            Employee employee = null;          //  待返回的员工信息

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = GET_EMPLOYEE_USERNAME_STR;
                cmd.Parameters.AddWithValue("@Username", employeeUsername);                         // 员工编号

                MySqlDataReader sqlRead = cmd.ExecuteReader();

                cmd.Dispose();

                if (sqlRead.Read())
                {
                    employee = new Employee();
                    employee.Id = int.Parse(sqlRead["id"].ToString());
                    employee.Name = sqlRead["name"].ToString();
                    employee.Position = sqlRead["position"].ToString();

                    employee.CanSubmit = int.Parse(sqlRead["cansubmit"].ToString());
                    employee.CanSign = int.Parse(sqlRead["cansign"].ToString());
                    employee.IsAdmin = int.Parse(sqlRead["isadmin"].ToString());

                    Department depart = new Department();
                    depart.Id = int.Parse(sqlRead["departid"].ToString());
                    depart.Name = sqlRead["departname"].ToString();
                    employee.Department = depart;

                    User user = new User();
                    user.Username = sqlRead["username"].ToString();
                    user.Password = sqlRead["password"].ToString();
                    employee.User = user;
                    Console.WriteLine(employee);
                }

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
            return employee;
        }
        #endregion



        #region 修改用户的基本信息
        public static bool ModifyEmployee(Employee employee)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;
            int count = -1;
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = MODIFY_EMPLOYEE_ID_STR;

                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Position", employee.Position);                        // 员工姓名
                cmd.Parameters.AddWithValue("@DepartmentId", employee.Department.Id);

                cmd.Parameters.AddWithValue("@CanSubmit", employee.CanSubmit);
                cmd.Parameters.AddWithValue("@CanSign", employee.CanSign);
                cmd.Parameters.AddWithValue("@IsAdmin", employee.IsAdmin);

                cmd.Parameters.AddWithValue("@Username", employee.User.Username);
                cmd.Parameters.AddWithValue("@Password", employee.User.Password);

                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

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
            if (count == 1)
            {
                Console.WriteLine("修改员工信息" + employee.Id.ToString() + "成功");

                return true;
            }
            else
            {
                Console.WriteLine("修改员工信息" + employee.Id.ToString() + "失败");

                return false;
            }
        }
        #endregion


        #region 查询员工的信息
        /// <summary>
        /// 查询所有的员工信息
        /// </summary>
        /// <returns></returns>
        public static List<Employee> QueryEmployee()
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<Employee> employees = new List<Employee>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_ALL_EMPLOYEE_STR;


                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    Employee employee = new Employee();

                    employee.Id = int.Parse(sqlRead["id"].ToString());
                    employee.Name = sqlRead["name"].ToString();
                    employee.Position = sqlRead["position"].ToString();

                    employee.CanSubmit = int.Parse(sqlRead["cansubmit"].ToString());
                    employee.CanSign = int.Parse(sqlRead["cansign"].ToString());
                    employee.IsAdmin = int.Parse(sqlRead["isadmin"].ToString());

                    Department depart = new Department();
                    depart.Id = int.Parse(sqlRead["departid"].ToString());
                    depart.Name = sqlRead["departname"].ToString();
                    employee.Department = depart;

                    User user = new User();
                    user.Username = sqlRead["username"].ToString();
                    user.Password = sqlRead["password"].ToString();
                    employee.User = user;

                    employees.Add(employee);        // 将查询出来的员工信息插入链表中
                }


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
            return employees;
        }

        /// <summary>
        /// 查询部门编号为departmentId的所有员工信息
        /// </summary>
        /// <param name="departmendId">部门的编号，当编号为0时返回所有员工的信息， > 0时返回所在部门员工的信息</param>
        /// <returns></returns>
        public static List<Employee> QueryEmployee(int departmendId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<Employee> employees = new List<Employee>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();
                if (departmendId == 0)
                {
                    cmd.CommandText = QUERY_ALL_EMPLOYEE_STR;

                }
                else
                {
                    cmd.CommandText = QUERY_DEPARTMENT_EMPLOYEE_ID_STR;
                    cmd.Parameters.AddWithValue("@DepartmentId", departmendId);

                }

                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    Employee employee = new Employee();

                    employee.Id = int.Parse(sqlRead["id"].ToString());
                    employee.Name = sqlRead["name"].ToString();
                    employee.Position = sqlRead["position"].ToString();

                    employee.CanSubmit = int.Parse(sqlRead["cansubmit"].ToString());
                    employee.CanSign = int.Parse(sqlRead["cansign"].ToString());
                    employee.IsAdmin = int.Parse(sqlRead["isadmin"].ToString());

                    Department depart = new Department();
                    depart.Id = int.Parse(sqlRead["departid"].ToString());
                    depart.Name = sqlRead["departname"].ToString();
                    employee.Department = depart;

                    User user = new User();
                    user.Username = sqlRead["username"].ToString();
                    user.Password = sqlRead["password"].ToString();
                    employee.User = user;

                    employees.Add(employee);        // 将查询出来的员工信息插入链表中
                }


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
            return employees;
        }

        /// <summary>
        /// 查询部门名称为departmentName的所有员工信息
        /// </summary>
        /// <param name="departmendId"></param>
        /// <returns></returns>
        public static List<Employee> QueryEmployee(String departmentName)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<Employee> employees = new List<Employee>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_DEPARTMENT_EMPLOYEE_NAME_STR;
                cmd.Parameters.AddWithValue("@DepartmentName", departmentName);

                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    Employee employee = new Employee();

                    employee.Id = int.Parse(sqlRead["id"].ToString());
                    employee.Name = sqlRead["name"].ToString();
                    employee.Position = sqlRead["position"].ToString();

                    employee.CanSubmit = int.Parse(sqlRead["cansubmit"].ToString());
                    employee.CanSign = int.Parse(sqlRead["cansign"].ToString());
                    employee.IsAdmin = int.Parse(sqlRead["isadmin"].ToString());

                    Department depart = new Department();
                    depart.Id = int.Parse(sqlRead["departid"].ToString());
                    depart.Name = sqlRead["departname"].ToString();
                    employee.Department = depart;

                    User user = new User();
                    user.Username = sqlRead["username"].ToString();
                    user.Password = sqlRead["password"].ToString();
                    employee.User = user;

                    employees.Add(employee);        // 将查询出来的员工信息插入链表中
                }


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
            return employees;
        }
        #endregion


        #region 获取员工签字图片的地址
        /// <summary>
        /// 获取编号为employeeId的员工签字的图片的地址
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public static String GetSignatureUrl(int employeeId)
        {
            return null;
        }
        
        /// <summary>
        /// 获取编号为employeeId的员工签字的图片的地址
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public static String GetSignatureUrl(String employeeName)
        {
            return null;
        }
        #endregion
        
        
        #region 测试增加用户
        public static void TestInsertEmployee()
        {
            Employee em = new Employee
            {
                Id = 9,
                Name = "王盼盼",
                Position = "局长",
                Department = new Department { Id = 5, Name = "行政科" },
                User = new User { Username = "wangpanpan", Password = "wangpanpan" }
            };
            InsertEmployee(em);
        }
        #endregion
    }
}
