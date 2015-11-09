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
    public class DALContractItem
    {

        
        #region 获取到当前部门所申请的会签单的可申请工程的项目列表
        private const String QUERY_PROJECT_ITEM_STR = @"SELECT id, projectid, item FROM `item` WHERE (`projectid` = @ProjectId)";

        public static List<ContractItem> QueryProjectItem(int projectId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<ContractItem> items = new List<ContractItem>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_PROJECT_ITEM_STR;
                cmd.Parameters.AddWithValue("@ProjectId", projectId);

                MySqlDataReader sqlRead = cmd.ExecuteReader();

                cmd.Dispose();

                while (sqlRead.Read())
                {
                    ContractItem item = new ContractItem();

                    item.Id = int.Parse(sqlRead["id"].ToString());
                    item.ProjectId = int.Parse(sqlRead["projectid"].ToString());
                    item.Item = sqlRead["item"].ToString();

                    items.Add(item);
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
            return items;
        }
        #endregion
    }
}
