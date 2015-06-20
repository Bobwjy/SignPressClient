using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SignPressServer.SignTools;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Data;
using SignPressServer.SignData;

namespace SignPressServer.SignDAL
{
    /*
     * 航道局基本会签单的模版信息
     */
    class DALHDJContract
    {
        /// <summary>
        /// 添加航道局会签单的信息串
        /// </summary>
        private const String INSERT_HDJCONTRACT_STR = @"INSERT INTO `hdjcontract` (`id`, `name`, `contempid`, `subempid`, `submitdate`, `columndata1`, `columndata2`, `columndata3`, `columndata4`, `columndata5`) 
                                                        VALUES (@Id, @Name, @ConTempId, @SubmitDate, @ColumnData_1, @ColumnData_2, @ColumnData_3, @ColumnData_4, @ColumnData_5, @SubEmpId)";

        /// <summary>
        /// 删除航道局会签单的信息串
        /// </summary>
        private const String DELETE_HDJCONTRACT_ID_STR = @"DELETE FROM `hdjcontract` WHERE (`id` = @Id)";

        /// <summary>
        /// 修改航道局会签单的信息串
        /// </summary>
        private const String MODIFY_HDJCONTRACT_STR = @"UPDATE `hdjcontract`
                                                        SET `name` = @Name, `subempid` = @SubEmpId, `submitdate` = @SubmitDate
                                                            `columndata1` = @ColumnData_1, `columndata2` = @ColumnData_2, `columndata3` = @ColumnData_3, `columndata4` = @ColumnData_4, `columndata5` = @ColumnData_5        
                                                        WHERE (`id` = @Id)";

        /// <summary>
        /// 查询航道局会签单的信息串
        /// </summary>
        private const String QUERY_HDJCONTRACT_STR = @"SELECT `id`, `name`, `subempid`, `subempname`, `submitdate` FROM `hdjcontract` WHERE";


        #region 插入会签单模版信息

        public static bool InsertHDJContract(HDJContract contract)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            int count = -1;                      // 受影响行数
            try
            {
                con.Open();

                cmd = con.CreateCommand();
                cmd.CommandText = INSERT_HDJCONTRACT_STR;

                cmd.Parameters.AddWithValue("@Id", contract.Id);
                cmd.Parameters.AddWithValue("@Name", contract.Name);
                cmd.Parameters.AddWithValue("@TempId", contract.ConTemp.TempId);
                cmd.Parameters.AddWithValue("@SubEmpId", contract.SubmitEmployee.Id);
                cmd.Parameters.AddWithValue("@SubmitDate", contract.SubmitDate);
                ///  5个栏目信息
                for (int cnt = 0; cnt < 5; cnt++)
                {
                    String strColumn = "@ColumnData_" + (cnt + 1).ToString();
                    cmd.Parameters.AddWithValue(strColumn, contract.ColumnDatas[cnt]);
                }



                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();
                if (count == 1)     //  插入成功后的受影响行数为1
                {
                    Console.WriteLine("插入会签单成功");
                    return true;
                }
                else
                {
                    Console.WriteLine("插入会签单失败");
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


        #region 删除会签单模版信息
        /// <summary>
        /// 删除编号为conTempId的会签单模版信息
        /// </summary>
        /// <param name="conTempId"></param>
        /// <returns></returns>
        public static bool DeleteHDJContact(int conTempId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            int count = -1;
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = DELETE_HDJCONTRACT_ID_STR;
                cmd.Parameters.AddWithValue("@Id", conTempId);                        // 会签单模版姓名


                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();

                if (count == 1)
                {
                    Console.WriteLine("删除会签单" + conTempId.ToString() + "成功");

                    return true;
                }
                else
                {
                    Console.WriteLine("删除会签单" + conTempId.ToString() + "失败");

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


        #region 修改会签单模版的信息
        public static bool ModifyHDJContract(HDJContract contract)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            int count = -1;
            try
            {


                con.Open();

                cmd = con.CreateCommand();
                cmd.CommandText = MODIFY_HDJCONTRACT_STR;

                cmd.Parameters.AddWithValue("@Id", contract.Id);
                cmd.Parameters.AddWithValue("@Name", contract.Name);
                cmd.Parameters.AddWithValue("@TempId", contract.ConTemp.TempId);
                cmd.Parameters.AddWithValue("@SubEmpId", contract.SubmitEmployee.Id);
                cmd.Parameters.AddWithValue("@SubmitDate", contract.SubmitDate);
                ///  5个栏目信息
                for (int cnt = 0; cnt < 5; cnt++)
                {
                    String strColumn = "@ColumnData_" + (cnt + 1).ToString();
                    cmd.Parameters.AddWithValue(strColumn, contract.ColumnDatas[cnt]);
                }

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
                Console.WriteLine("修改会签单信息" + contract.Id.ToString() + "成功");

                return true;
            }
            else
            {
                Console.WriteLine("修改会签单信息" + contract.Id.ToString() + "失败");

                return false;
            }
        }
        #endregion


        #region 查询会签单模版的信息
        public static List<HDJContract> QueryHDJContract()
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<HDJContract> contracts = new List<HDJContract>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_HDJCONTRACT_STR;


                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    HDJContract contract = new HDJContract();


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
            return contracts;
        }
        #endregion

    }
}
