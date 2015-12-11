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
    /// <summary>
    /// 该类型是统计功能操作数据库的接口 
    /// </summary>
 
    ///  注意我们的数据结构
    ///  Category  标识了会签单的基本类别-[界内应例]，其直接影响着工程名称  
    ///  Project   标识了会签单的项目名称，其依赖于category, 标识了工程下面的每个子项
    ///  Item      标识了会签单的工作量信息，其依赖于Project，标识了项目下面的每个子项
    ///  Workload  标识了会签单的工作量集合，他由Item类别 + 工作量大小 + 花费金额组成
    public class DALContractStatistic
    {
        #region  查询部门department当年year的单项工作量Item的统计信息
        ///  SELECT w.contractid, w.itemid, i.item, w.work, w.expense FROM `workload` w, `item` i WHERE w.itemid = i.id AND `contractid` like "申%" AND `itemid` = 1
        ///  SELECT w.contractid,  p.id projectid, p.project, i.id itemid, i.item,w.work, w.expense FROM `workload` w, `item` i, `project` p WHERE w.itemid = i.id AND i.projectid = p.id AND `contractid` like "申%" AND `itemid` = 1
        
        private static String STATIS_SDEPARTMENT_YEAR_ITEM_STR = @"SELECT Sum(work) works, Sum(expense) expenses FROM `workload` WHERE `contractid` like @SDepartmentYear AND `itemid` = @ItemId";
        /// SELECT Sum(work) works, Sum(expense) expenses FROM `workload` WHERE `contractid` like "申_2015%" AND itemid = 1 GROUP BY itemid
        /// SELECT i.id "工作量编号", i.item "工作量名称", Sum(wl.work) "工作量大小", Sum(wl.expense) "花费" FROM `workload` wl, item i WHERE wl.itemid = i.id AND `contractid` like "申_2015%" GROUP BY i.id

        /// <summary>
        /// 查询部门department当年year的单项工作量的统计信息
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="year"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static ContractWorkload StatisSDepartmentYearItemWorkLoad(string departmentShortCall, int year, int itemId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            ContractWorkload workload = null;

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = STATIS_SDEPARTMENT_YEAR_ITEM_STR;
                cmd.Parameters.AddWithValue("@SDepartmentYear", departmentShortCall + "_" + year.ToString() + "%");
                cmd.Parameters.AddWithValue("@ItemId", itemId);
                MySqlDataReader sqlRead = cmd.ExecuteReader();

                cmd.Dispose();

                while (sqlRead.Read())
                {
                    workload = new ContractWorkload();

                    workload.ContractId = "STATIS";

                    workload.Work = double.Parse(sqlRead["works"].ToString());
                    workload.Expense = double.Parse(sqlRead["expenses"].ToString());

                    ContractItem item = new ContractItem();
                    item.Id = -1;
                    workload.Item = item;

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
            return workload;

        }
        #endregion


        #region  统计当前部门deaprtment当年year申请的所有工程为Project的统计信息

        /// SELECT p.id, p.project, Sum(wl.work) work, Sum(wl.expense) expense FROM `workload` wl, item i, project p WHERE wl.itemid = i.id AND i.projectid = p.id AND `contractid` like "申_2015%" GROUP BY p.id
        private static String STATIS_SDEPARTMENT_YEAR_PROJECT_STR = @"SELECT Sum(wl.work), Sum(expense) expense FROM `workload` w, `item` i WHERE w.itemid = i.id AND w.contractid like @SDepartmentYear AND i.projectid = @ProjectId";
        /// <summary>
        ///  统计当前部门申请的所有工程Project下的会签单信息[Search数据填写SDepartmentShortCall + ItemId]
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static ContractWorkload StatisSDepartmentYearProjectWorkLoad(string departmentShortCall, int year, int projectId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            ContractWorkload workload = null;

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = STATIS_SDEPARTMENT_YEAR_PROJECT_STR;
                cmd.Parameters.AddWithValue("@SDepartmentYear", departmentShortCall + "_" + year.ToString() + "%");
                cmd.Parameters.AddWithValue("@ProjectId", projectId);
                MySqlDataReader sqlRead = cmd.ExecuteReader();

                cmd.Dispose();

                while (sqlRead.Read())
                {
                    workload = new ContractWorkload();

                    workload.ContractId = "STATIS";

                    workload.Work = -1;
                    workload.Expense = double.Parse(sqlRead["expenses"].ToString());

                    ContractItem item = new ContractItem();
                    item.Id = -1;
                    workload.Item = item;

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
            return workload;

        }
        #endregion


        #region  统计当前部门deaprtment当年year申请的类别为Category的统计信息

        /// SELECT p.id, p.project, Sum(wl.work) work, Sum(wl.expense) expense FROM `workload` wl, item i, project p WHERE wl.itemid = i.id AND i.projectid = p.id AND `contractid` like "申_2015%" GROUP BY p.id
        private static String STATIS_SDEPARTMENT_YEAR_CATEGORY_STR = @"SELECT Sum(wl.work), Sum(expense) expense FROM `workload` w, `item` i WHERE w.itemid = i.id AND w.contractid like @SDepartmentYear AND i.projectid = @ProjectId";
        /// <summary>
        ///  统计当前部门申请的所有工程Project下的会签单信息[Search数据填写SDepartmentShortCall + ItemId]
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static ContractWorkload StatisSDepartmentYearCategoryWorkLoad(string departmentShortCall, int year, int categoryId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            ContractWorkload workload = null;

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = STATIS_SDEPARTMENT_YEAR_PROJECT_STR;
                cmd.Parameters.AddWithValue("@SDepartmentYear", departmentShortCall + "_" + year.ToString() + "%");
                cmd.Parameters.AddWithValue("@Category", categoryId);
                MySqlDataReader sqlRead = cmd.ExecuteReader();

                cmd.Dispose();

                while (sqlRead.Read())
                {
                    workload = new ContractWorkload();

                    workload.ContractId = "STATIS";

                    workload.Work = -1;
                    workload.Expense = double.Parse(sqlRead["expenses"].ToString());

                    ContractItem item = new ContractItem();
                    item.Id = -1;
                    workload.Item = item;

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
            return workload;

        }
        #endregion

        /// <summary>
        /// 
        /// 
        ///  我们总是希望获取到当前类别会签单的所有部门的申请信息
        ///  因此我们需要这么做
        ///  首先我们需要获取到哪些部门有当前会签单类别的申请权限
        ///  然后获取到当前工程类别的所有工程信息和工作量信息
        ///  
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool StatisticSDepartmentYearCategory(int year, string categoryShortCall/*int categoryId*/)
        {
            string sheetName = categoryShortCall + year.ToString( );

            //int year = System.DateTime.Now.Year;
            //  首先获取数据库中可以申请本会签单类别的所有部门的列表
            List<Department> departments = DALContractIdCategory.QueryCategorySDepartment(categoryId);            
            
            //  其次查询当前Category类别的所有Project信息和item信息
            List<ContractProject> projects = DALContractProject.QueryCategoryProject(categoryId);

            foreach (Department department in departments)     // 循环每个部门
            {
                
                foreach (ContractProject project in projects)       //  循环每个项目的信息
                {
                    //  统计当前部门Department当年Year项目Project的统计信息
                    ContractWorkload workload = DALContractStatistic.StatisSDepartmentYearProjectWorkLoad(department.ShortCall, year, project.Id);
                    
                    //Console.WriteLine(workload);

                    //  获取当前项目的工作量集合
                    List<ContractItem> items = DALContractItem.QueryProjectItem(project.Id);
                    foreach (ContractItem item in items)
                    {
                        //  统计当前部门Department当年Year工作量为item的统计信息
                        DALContractStatistic.StatisSDepartmentYearItemWorkLoad(department.ShortCall, year, item.Id);
                        
                    }
        
                }
            
            }
            
            return true;
    
        }
    
    
    }
}
