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

    //  对底层数据结构CobtractProject操作的数据库接口类
    //  操作数据库中的project表结构
    public class DALContractProject
    {

        #region  插入部门信息
        private const String INSERT_CONTRACT_PROJECT_STR = @"INSERT INTO `project` (`id`, `categoryid`, `project`) VALUES (@Id, @CategoryId, @Project)";
        /// <summary>
        /// 插入部门信息
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public static bool InsertContractProject(ContractProject project)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();

            MySqlCommand cmd;
            int count = -1;                      // 受影响行数
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = INSERT_CONTRACT_PROJECT_STR;

                cmd.Parameters.AddWithValue("@Id", project.Id);                             //  部门职位
                cmd.Parameters.AddWithValue("@CategoryId", project.CategoryId);                     //  项目类型     
                cmd.Parameters.AddWithValue("@Project", project.Project);   //  项目类型简写


                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();

                if (count == 1)     //  插入成功后的受影响行数为1
                {
                    Console.WriteLine("插入项目类型成功");
                    return true;
                }
                else
                {
                    Console.WriteLine("插入项目类型失败");
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


        #region 获取到当前部门所申请的会签单的可申请工程的项目列表
        private const String QUERY_CATEGORY_PROJECT = @"SELECT id, categoryid, project FROM `project` WHERE (`categoryid` = @CategoryId)";
        public static List<ContractProject> QueryCategoryProject(int categoryId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<ContractProject> projects = new List<ContractProject>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_CATEGORY_PROJECT;

                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    ContractProject project = new ContractProject();

                    project.Id = int.Parse(sqlRead["id"].ToString());
                    project.CategoryId = int.Parse(sqlRead["categoryid"].ToString());
                    project.Project = sqlRead["project"].ToString();

                    projects.Add(project);
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
            return projects;
        }
        #endregion
    }
}
