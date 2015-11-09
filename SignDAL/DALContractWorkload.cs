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
    }
}
