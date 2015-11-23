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
    public class DALContractWorkload
    {

         
        #region 获取到当前会签单的工作量集合
        private const String QUERY_CONTRACT_WORKLOAD_STR = @"SELECT w.contractid contractid, i.id itemid, i.item item, i.projectid projectid, w.work work, w.expense expense FROM `workload` w, `item` i WHERE (w.itemid = i.id and `contractid` = @ContractId)";

        public static List<ContractWorkload> QureyContractWorkLoad(string contractId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<ContractWorkload> workloads = new List<ContractWorkload>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_CONTRACT_WORKLOAD_STR;
                cmd.Parameters.AddWithValue("@ContractId", contractId);

                MySqlDataReader sqlRead = cmd.ExecuteReader();

                cmd.Dispose();

                while (sqlRead.Read())
                {
                    ContractWorkload workload = new ContractWorkload();

                    workload.ContractId = contractId;

                    workload.Work = double.Parse(sqlRead["work"].ToString());
                    workload.Expense = double.Parse(sqlRead["expense"].ToString());

                    ContractItem item = new ContractItem();
                    item.Id = int.Parse(sqlRead["itemid"].ToString());
                    item.ProjectId = int.Parse(sqlRead["projectid"].ToString());
                    item.Item = sqlRead["item"].ToString();
                    workload.Item = item;
                    //Console.WriteLine(workload.Work + "  " + workload.Expense);

                    workloads.Add(workload);
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
            return workloads;
        }
        #endregion


        #region  插入部门信息
        /// <summary>
        /// 插入部门信息
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private static String INSERT_WORKLOAD_STR = @"INSERT INTO `workload` (`contractid`, `itemid`, `work`, `expense`) VALUES (@ContractId, @ItemId, @Work, @Expense)";
        public static bool InsertWorkload(ContractWorkload workload)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();

            MySqlCommand cmd;
            int count = -1;                      // 受影响行数
            try
            {
                con.Open();

                cmd = con.CreateCommand();
                cmd.CommandText = INSERT_WORKLOAD_STR;
                cmd.Parameters.AddWithValue("@ContractId", workload.ContractId);             //  当前工作量所属的会签单信息
                cmd.Parameters.AddWithValue("@ItemId", workload.Item.Id);                    //  当前工作量的工作量信息
                cmd.Parameters.AddWithValue("@Work", workload.Work);                         //  当前工作量的工作量大小
                cmd.Parameters.AddWithValue("@Expense", workload.Expense);                   //   当前工作量的报价


                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();
                if (count == 1)     //  插入成功后的受影响行数为1
                {
                    Console.WriteLine("工作量信息插入成功");
                    return true;
                }
                else
                {
                    Console.WriteLine("工作量信息插入失败");
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


        #region 删除工作量信息
        /// <summary>
        /// 删除部门的信息DeleteEmployee
        /// </summary>
        /// <param name="id">部门的部门号</param>
        /// <returns></returns>
        private static String DELETE_WORKLOAD_STR = @"DELETE FROM `workload` where(`contractid` = @ContractId and `itemid` = @ItemId)";

        public static bool DeleteWorkload(ContractWorkload workload)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;
            int count = -1;
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = DELETE_WORKLOAD_STR;
                cmd.Parameters.AddWithValue("@Id", workload.ContractId);                        // 部门姓名
                cmd.Parameters.AddWithValue("@ItemId", workload.Item.Id);

                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();

                if (count == 1)
                {
                    Console.WriteLine("删除工作量[表" + workload.ContractId.ToString() + ", " + workload.Item.Id + "成功");
                    return true;
                }
                else
                {
                    Console.WriteLine("删除工作量[表" + workload.ContractId.ToString() + ", " + workload.Item.Id + "失败");

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


        #region    修改工作量信息
        private const String MODIFY_WORKLOAD_STR = @"UPDATE `workload` SET `work` = @Work, `expense` = @Expense WHERE (`contractid` = @ContractId and`itemid` = @ItemId)";
        /// <summary>
        /// 修改工作量信息
        /// </summary>
        /// <param name="workload"></param>
        /// <returns></returns>
        public static bool ModifyWorkload(ContractWorkload workload)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;
            int count = -1;
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = MODIFY_WORKLOAD_STR;
                cmd.Parameters.AddWithValue("@ContractId", workload.ContractId);
                cmd.Parameters.AddWithValue("@ItemId", workload.Item.Id);
                cmd.Parameters.AddWithValue("@Work", workload.Work);
                cmd.Parameters.AddWithValue("@Expense", workload.Expense);              


                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();

                if (count == 1)
                {
                    Console.WriteLine("删除工作量[表" + workload.ContractId.ToString() + ", " + workload.Item.Id + "成功");
                    return true;
                }
                else
                {
                    Console.WriteLine("删除工作量[表" + workload.ContractId.ToString() + ", " + workload.Item.Id + "失败");

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

        #region 统计工作量的信息[Search数据填写SDepartmentShortCall + ItemId]
        private static String   QUERY_SDEPARTMENT_ITEM_YEAR_WORKLOAD_STR = @"SELECT `contractid`, `itemid`, `work`, `expense` FROM `workload` WHERE `contractid` like @SDepartmentYear AND `itemid` = @ItemId";
        ///  SELECT w.contractid, w.itemid, i.item, w.work, w.expense FROM `workload` w, `item` i WHERE w.itemid = i.id AND `contractid` like "申%" AND `itemid` = 1
        ///  SELECT w.contractid,  p.id projectid, p.project, i.id itemid, i.item,w.work, w.expense FROM `workload` w, `item` i, `project` p WHERE w.itemid = i.id AND i.projectid = p.id AND `contractid` like "申%" AND `itemid` = 1
        /// <summary>
        ///  统计工作量的信息[Search数据填写SDepartmentShortCall + ItemId]
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<ContractWorkload> GetSDepartmentItemWorkload(Search search)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<ContractWorkload> workloads = new List<ContractWorkload>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_SDEPARTMENT_ITEM_YEAR_WORKLOAD_STR;
                cmd.Parameters.AddWithValue("@SDepartmentYear", search.SDepartmentShortlCall + "_" + search.Year.ToString() + "%");
                cmd.Parameters.AddWithValue("@ItemId", search.ItemId);
                MySqlDataReader sqlRead = cmd.ExecuteReader();

                cmd.Dispose();

                while (sqlRead.Read())
                {
                    ContractWorkload workload = new ContractWorkload();

                    workload.ContractId = sqlRead["contractid"].ToString();

                    workload.Work = double.Parse(sqlRead["work"].ToString());
                    workload.Expense = double.Parse(sqlRead["expense"].ToString());

                    ContractItem item = new ContractItem();
                    item.Id = int.Parse(sqlRead["itemid"].ToString());
                    //item.ProjectId = int.Parse(sqlRead["projectid"].ToString());
                    //item.Item = sqlRead["item"].ToString();
                    workload.Item = item;
                    //Console.WriteLine(workload.Work + "  " + workload.Expense);

                    workloads.Add(workload);
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
            return workloads;

        }

        private static String STATIS_SDEPARTMENT_ITEM_YEAR_WORKLOAD_STR = @"SELECT Sum(work) works, Sum(expense) expenses FROM `workload` WHERE `contractid` like @SDepartmentYear AND `itemid` = @ItemId";
        ///  SELECT w.contractid, w.itemid, i.item, w.work, w.expense FROM `workload` w, `item` i WHERE w.itemid = i.id AND `contractid` like "申%" AND `itemid` = 1
        ///  SELECT w.contractid,  p.id projectid, p.project, i.id itemid, i.item,w.work, w.expense FROM `workload` w, `item` i, `project` p WHERE w.itemid = i.id AND i.projectid = p.id AND `contractid` like "申%" AND `itemid` = 1
        /// <summary>
        ///  统计工作量的信息[Search数据填写SDepartmentShortCall + ItemId]
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static ContractWorkload StatisSDepartmentItemYearWorkload(Search search)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            ContractWorkload workload = null;

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = STATIS_SDEPARTMENT_ITEM_YEAR_WORKLOAD_STR;
                cmd.Parameters.AddWithValue("@SDepartmentYear", search.SDepartmentShortlCall + "_" + search.Year.ToString() + "%");
                cmd.Parameters.AddWithValue("@ItemId", search.ItemId);
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


        private static String STATIS_SDEPARTMENT_PROJECT_YEAR_WORKLOAD_STR = @"SELECT Sum(expense) expense FROM `workload` w, `item` i WHERE w.itemid = i.id AND w.contractid like @SDepartmentYear AND i.projectid = @ProjectId";
        /// <summary>
        ///  统计当前部门申请的所有工程Project下的会签单信息[Search数据填写SDepartmentShortCall + ItemId]
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static ContractWorkload StatisSDepartmentProjectYearWorkload(Search search)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            ContractWorkload workload = null;

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = STATIS_SDEPARTMENT_PROJECT_YEAR_WORKLOAD_STR;
                cmd.Parameters.AddWithValue("@SDepartmentYear", search.SDepartmentShortlCall + "_" + search.Year.ToString() + "%");
                cmd.Parameters.AddWithValue("@ProjectId", search.ProjectId);
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

        private static String STATIS_PROJECT_YEAR_WORKLOAD_STR = @"SELECT Sum(expense) expense FROM `workload` w, `item` i WHERE w.itemid = i.id AND i.projectid = @ProjectId";
        /// <summary>
        ///  统计所有工程为Project下的会签单信息[Search数据填写SDepartmentShortCall + ItemId]
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static ContractWorkload StatisProjectYearWorkload(Search search)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            ContractWorkload workload = null;

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = STATIS_PROJECT_YEAR_WORKLOAD_STR;
                cmd.Parameters.AddWithValue("@ProjectId", search.ProjectId);
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

    }
}
