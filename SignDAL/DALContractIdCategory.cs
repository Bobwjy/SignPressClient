using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SignPressServer.SignData;


using MySql.Data.MySqlClient;
using MySql.Data;
using System.Data;
using SignPressServer.SignTools;


namespace SignPressServer.SignDAL
{
    class DALContractIdCategory
    {
        //  采用此种侧率时的触发器信息为
        // 如果将conidcategory作为读写表
// 那么触发器变成
//CREATE trigger set_department_category
//BEFORE INSERT on `conidcategory`
//FOR EACH ROW 
//BEGIN

//    if(new.categoryid == 1)
//    then    UPDATE `department` SET canboundary = 1 WHERE (id = new.id);

//    if(new.categoryid == 2)
//    then    UPDATE `department` SET caninland = 1 WHERE (id = new.id);
   
//    if(new.categoryid == 3)
//    then    UPDATE `department` SET canemergency = 1 WHERE (id = new.id);
    
//    if(new.categoryid == 4)
//    then    UPDATE `department` SET canregular = 1 WHERE (id = new.id);

//END;


//CREATE trigger set_department_category
//BEFORE DELETE on `conidcategory`
//FOR EACH ROW 
//BEGIN

//    if(old.categoryid == 1)
//    then    UPDATE `department` SET canboundary = 0 WHERE (id = new.id);

//    if(old.categoryid == 2)
//    then    UPDATE `department` SET caninland = 0 WHERE (id = new.id);
   
//    if(old.categoryid == 3)
//    then    UPDATE `department` SET canemergency = 0 WHERE (id = new.id);
    
//    if(old.categoryid == 4)
//    then    UPDATE `department` SET canregular = 0 WHERE (id = new.id);

//END;
        #region  插入部门信息
        private const String INSERT_CONTRACT_ID_CATEGORY_STR = @"INSERT INTO `conidcategory` (`departmentid`, `category`, `categoryshortcall`) VALUES (@DepartmentId, @IdCategory, @CategoryShortCall)";
        /// <summary>
        /// 插入部门信息
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public static bool InsertContractIdCategory(ContractIdCategory contractIdCategory)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();

            MySqlCommand cmd;
            int count = -1;                      // 受影响行数
            try
            {
                con.Open();

                cmd = con.CreateCommand();
                
                cmd.CommandText = INSERT_CONTRACT_ID_CATEGORY_STR;
                
                cmd.Parameters.AddWithValue("@DepartmentId", contractIdCategory.Department.Id);                             //  部门职位
                cmd.Parameters.AddWithValue("@Category", contractIdCategory.ContractCategory.Category);                     //  项目类型     
                cmd.Parameters.AddWithValue("@CategoryShortCall", contractIdCategory.ContractCategory.CategoryShortCall);   //  项目类型简写


                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();
                
                if (count == 1)     //  插入成功后的受影响行数为1
                {
                    Console.WriteLine("针对部门插入项目类型前缀成功");
                    return true;
                }
                else
                {
                    Console.WriteLine("部门插入项目类型前缀失败");
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


        /// <summary>
        /// 查询当前部门的会签单申请权限
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        private const String QUERY_SDEPARTMENT_CONTRACTCATEGORY_STR = "SELECT c.id id, c.category category, c.shortcall categoryshortcall FROM `category` c, `conidcategory` cc WHERE (c.id = cc.categoryid and cc.departmentid = @DepartmentId);";
        public static List<ContractCategory> QuerySDepartmentContractCategory(int departmentId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<ContractCategory> categorys = new List<ContractCategory>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_SDEPARTMENT_CONTRACTCATEGORY_STR;
                cmd.Parameters.AddWithValue("@DepartmentId", departmentId);
                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    ContractCategory category = new ContractCategory();

                    category.Id = int.Parse(sqlRead["id"].ToString());
                    category.Category = sqlRead["category"].ToString();
                    category.CategoryShortCall = sqlRead["categoryshortcall"].ToString();

                    categorys.Add(category);
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
            return categorys;
        }

        /// <summary>
        /// 查询可申请当前会签单类别的所有部门
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private static String QUERY_CATEGORY_SDEPARTMENT_STR = @"SELECT departmentid FROM conidcategory WHERE categoryid = @CategoryId ORDER BY departmentid";
        /// SELECT * FROM conidcategory WHERE categoryid = @CategoryId
        /// SELECT d.id depid, d.name depname, d.shortcall depshortcall FROM conidcategory c, department d WHERE c.departmentid = d.id AND c.categoryid = 1
        /// SELECT d.id depid, d.name depname, d.shortcall depshortcall, c.category, c.shortcall FROM conidcategory cc, department d, category c WHERE c.id = cc.categoryid AND cc.departmentid = d.id AND cc.categoryid = 
        public static List<Department> QueryCategorySDepartment(int categoryId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<Department> departments = new List<Department>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_CATEGORY_SDEPARTMENT_STR;
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    Department department = new Department();

                    department.Id = int.Parse(sqlRead["id"].ToString());
                    department.Name = sqlRead["name"].ToString();
                    department.ShortCall = sqlRead["shortcall"].ToString();

                    departments.Add(department);
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
            return departments;
        }
    
    }
}
