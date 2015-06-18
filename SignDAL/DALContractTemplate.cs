using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using SignPressServer.SignData;


using MySql.Data.MySqlClient;
using MySql.Data;
using System.Data;
using SignPressServer.SignTools;

namespace SignPressServer.SignDAL
{
    /*
     *  处理底层模版的数据库操作接口
     */
    class DALContractTemplate
    {
        #region  数据库信息串
        


        /// <summary>
        /// 插入会签单模版信息串
        /// </summary>
        private const String INSERT_CONTRACT_TEMPLATE_STR = @"INSERT INTO `contemp` (`name`, 
                                                                                     `column1`, `column2`, `column3`, `column4`, `column5`, 
                                                                                     `signinfo1`, `signinfo2`, `signinfo3`, `signinfo4`, `signinfo5`, `signinfo6`, `signinfo7`, `signinfo8`, 
                                                                                     `signid1`, `signid2`, `signid3`, `signid4`, `signid5`, `signid6`, `signid7`, `signid8`) 
                                                              VALUES (@Name, 
                                                                      @Column_1, @Column_2, @Column_3, @Column_4, @Column_5, 
                                                                      @SignInfo_1, @SignInfo_2, @SignInfo_3, @SignInfo4, @SignInfo5, @SignInfo6, @SignInfo7, @SignInfo8, 
                                                                      @SignId_1, @SignId_2, @SignId_3, @SignId_4, @SignId_5, @SignId_6, @SignId_7, @SignId_8)";

        /// <summary>
        /// 删除会签单模版信息串
        /// </summary>
        private const String DELETE_CONTRACT_TEMPLATE_ID_STR = @"DELETE FROM `contemp` WHERE (`id` = @Id)";

        private const String DELETE_CONTRACT_TEMPLATE_NAME_STR = @"DELETE FROM `contemp` WHERE (`name` = @Name)";


        /// <summary>
        /// 修改会签单模版信息串
        /// </summary>
        private const String MODIFY_CONTRACT_TEMPLATE_STR = @"UPDATE `contemp` 
                                                        SET (`name` = @Name, 
                                                             `column1` = @Column_1, `column2` = @Column_2, `column3` = @Column_3, `column4` = @Column_4, `column5` = @Column_5, 
                                                             `signinfo1` = @SignInfo_1, `signinfo2` = @SignInfo_2, `signinfo3` = @SignInfo_3, `signinfo4` = @SignInfo_4, `signinfo5` = @SignInfo_5, `signinfo6` = @SignInfo_6, `signinfo7` = @SignInfo_7, `signinfo8` = @SignInfo_8, 
                                                             `signid1` = @SignId_1, `signid2` = @SignId_2, `signid3` = @SignId_3, `signid4` = @SignId_4, `signid5` = @SignId_5, `signid6` = @SignId_6, `signid7` = @SignId_7, `signid8` = @SignId_8) 
                                                        WHERE (`id` = @Id)";


        /// <summary>
        /// 获取会签单模版信息串
        /// </summary>
        private const String GET_CONTRACT_TEMPLATE_ID_STR = @"SELECT `name`, 
                                                                  `column1`, `column2`, `column3`, `column4`, `column5`, 
                                                                  `signinfo1`, `signinfo2`, `signinfo3`, `signinfo4`, `signinfo5`, `signinfo6`, `signinfo7`, `signinfo8`, 
                                                                  `signid1`, `signid2`, `signid3`, `signid4`, `signid5`, `signid6`, `signid7`, `signid8`
                                                              FROM `contemp`
                                                              WHERE (`id` = @Id)";

        private const String GET_CONTRACT_TEMPLATE_NAME_STR = @"SELECT `name`, 
                                                                  `column1`, `column2`, `column3`, `column4`, `column5`, 
                                                                  `signinfo1`, `signinfo2`, `signinfo3`, `signinfo4`, `signinfo5`, `signinfo6`, `signinfo7`, `signinfo8`, 
                                                                  `signid1`, `signid2`, `signid3`, `signid4`, `signid5`, `signid6`, `signid7`, `signid8`
                                                              FROM `contemp`
                                                              WHERE (`name` = @Name)"; 


        #endregion


        #region 插入会签单模版信息
        /// <summary>
        /// 插入员工信息
        /// </summary>
        /// <param name="conTemp">待插入的会签单模版</param>
        /// <returns></returns>
        /// 
        public static bool InsertContractTemplate(ContractTemplate conTemp)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            int count = -1;                      // 受影响行数
            try
            {
                con.Open();

                cmd = con.CreateCommand();
                cmd.CommandText = INSERT_CONTRACT_TEMPLATE_STR;

                cmd.Parameters.AddWithValue("@Name", conTemp.Name);

                ///  5个栏目信息
                /*
                cmd.Parameters.AddWithValue("@Column_1", conTemp.ColumnDatas[0]);                       
                cmd.Parameters.AddWithValue("@Column_2", conTemp.ColumnDatas[1]);                  
                cmd.Parameters.AddWithValue("@Column_3", conTemp.ColumnDatas[2]);         
                cmd.Parameters.AddWithValue("@Column_4", conTemp.ColumnDatas[3]);              
                cmd.Parameters.AddWithValue("@Column_5", conTemp.ColumnDatas[4]);
                */
                for (int cnt = 0; cnt < 5; cnt++)
                { 
                    String strColumn = "@Column_" + (cnt + 1).ToString( );
                    cmd.Parameters.AddWithValue(strColumn, conTemp.ColumnNames[cnt]);
                }
                

                ///  8项签字信息
                /*
                cmd.Parameters.AddWithValue("@SignInfo_1", conTemp.SignDatas[0].SignInfo);
                cmd.Parameters.AddWithValue("@SignId_1", conTemp.SignDatas[0].SignId);

                cmd.Parameters.AddWithValue("@SignInfo_2", conTemp.SignDatas[1].SignInfo);
                cmd.Parameters.AddWithValue("@SignId_2", conTemp.SignDatas[1].SignId);

                cmd.Parameters.AddWithValue("@SignInfo_3", conTemp.SignDatas[2].SignInfo);
                cmd.Parameters.AddWithValue("@SignId_3", conTemp.SignDatas[2].SignId);

                cmd.Parameters.AddWithValue("@SignInfo_4", conTemp.SignDatas[3].SignInfo);
                cmd.Parameters.AddWithValue("@SignId_4", conTemp.SignDatas[3].SignId);

                cmd.Parameters.AddWithValue("@SignInfo_5", conTemp.SignDatas[4].SignInfo);
                cmd.Parameters.AddWithValue("@SignId_5", conTemp.SignDatas[4].SignId);

                cmd.Parameters.AddWithValue("@SignInfo_6", conTemp.SignDatas[5].SignInfo);
                cmd.Parameters.AddWithValue("@SignId_6", conTemp.SignDatas[5].SignId);
             
                cmd.Parameters.AddWithValue("@SignInfo_7", conTemp.SignData[6].SignInfo);
                cmd.Parameters.AddWithValue("@SignId_7", conTemp.SignData[6].SignId);

                cmd.Parameters.AddWithValue("@SignInfo_8", conTemp.SignData[7].SignInfo);
                cmd.Parameters.AddWithValue("@SignId_8", conTemp.SignData[7].SignId);
                */
                for (int cnt = 0; cnt < 8; cnt++)
                {
                    String strSignInfo = "@SignInfo_" + (cnt + 1).ToString();
                    String strSignId = "@SignId_" + (cnt + 1).ToString();

                    cmd.Parameters.AddWithValue(strSignInfo, conTemp.SignDatas[cnt].SignInfo);
                    cmd.Parameters.AddWithValue(strSignId, conTemp.SignDatas[cnt].SignId);

                }

                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();
                if (count == 1)     //  插入成功后的受影响行数为1
                {
                    Console.WriteLine("插入会签单模版成功");
                    return true;
                }
                else
                {
                    Console.WriteLine("插入会签单模版失败");
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
        public static bool DeleteContactTemplate(int conTempId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            int count = -1;
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = DELETE_CONTRACT_TEMPLATE_ID_STR;
                cmd.Parameters.AddWithValue("@Id", conTempId);                        // 部门姓名


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

        /// <summary>
        /// 删除会签单名称是conTempName的会签单信息
        /// </summary>
        /// <param name="conTempName">带删除的会签单名称</param>
        /// <returns></returns>
        public static bool DeleteContactTemplate(String conTempName)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            int count = -1;
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = DELETE_CONTRACT_TEMPLATE_ID_STR;
                cmd.Parameters.AddWithValue("@name", conTempName);                        // 部门姓名


                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();

                if (count == 1)
                {
                    Console.WriteLine("删除会签单" + conTempName + "成功");

                    return true;
                }
                else
                {
                    Console.WriteLine("删除会签单" + conTempName+ "失败");

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
        public static bool ModifyContractTemplate(ContractTemplate conTemp)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            int count = -1;
            try
            {


                con.Open();

                cmd = con.CreateCommand();
                cmd.CommandText = INSERT_CONTRACT_TEMPLATE_STR;

                cmd.Parameters.AddWithValue("@Name", conTemp.Name);

                ///  5个栏目信息
                /*
                cmd.Parameters.AddWithValue("@Column_1", conTemp.ColumnDatas[0]);                       
                cmd.Parameters.AddWithValue("@Column_2", conTemp.ColumnDatas[1]);                  
                cmd.Parameters.AddWithValue("@Column_3", conTemp.ColumnDatas[2]);         
                cmd.Parameters.AddWithValue("@Column_4", conTemp.ColumnDatas[3]);              
                cmd.Parameters.AddWithValue("@Column_5", conTemp.ColumnDatas[4]);
                */
                for (int cnt = 0; cnt < 5; cnt++)
                { 
                    String strColumn = "@Column_" + (cnt + 1).ToString( );
                    cmd.Parameters.AddWithValue(strColumn, conTemp.ColumnNames[cnt]);
                }
                

                ///  8项签字信息
                /*
                cmd.Parameters.AddWithValue("@SignInfo_1", conTemp.SignDatas[0].SignInfo);
                cmd.Parameters.AddWithValue("@SignId_1", conTemp.SignDatas[0].SignId);

                cmd.Parameters.AddWithValue("@SignInfo_2", conTemp.SignDatas[1].SignInfo);
                cmd.Parameters.AddWithValue("@SignId_2", conTemp.SignDatas[1].SignId);

                cmd.Parameters.AddWithValue("@SignInfo_3", conTemp.SignDatas[2].SignInfo);
                cmd.Parameters.AddWithValue("@SignId_3", conTemp.SignDatas[2].SignId);

                cmd.Parameters.AddWithValue("@SignInfo_4", conTemp.SignDatas[3].SignInfo);
                cmd.Parameters.AddWithValue("@SignId_4", conTemp.SignDatas[3].SignId);

                cmd.Parameters.AddWithValue("@SignInfo_5", conTemp.SignDatas[4].SignInfo);
                cmd.Parameters.AddWithValue("@SignId_5", conTemp.SignDatas[4].SignId);

                cmd.Parameters.AddWithValue("@SignInfo_6", conTemp.SignDatas[5].SignInfo);
                cmd.Parameters.AddWithValue("@SignId_6", conTemp.SignDatas[5].SignId);
             
                cmd.Parameters.AddWithValue("@SignInfo_7", conTemp.SignData[6].SignInfo);
                cmd.Parameters.AddWithValue("@SignId_7", conTemp.SignData[6].SignId);

                cmd.Parameters.AddWithValue("@SignInfo_8", conTemp.SignData[7].SignInfo);
                cmd.Parameters.AddWithValue("@SignId_8", conTemp.SignData[7].SignId);
                */
                for (int cnt = 0; cnt < 8; cnt++)
                {
                    String strSignInfo = "@SignInfo_" + (cnt + 1).ToString();
                    String strSignId = "@SignId_" + (cnt + 1).ToString();

                    cmd.Parameters.AddWithValue(strSignInfo, conTemp.SignDatas[cnt].SignInfo);
                    cmd.Parameters.AddWithValue(strSignId, conTemp.SignDatas[cnt].SignId);

                }

                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();
                //if (count == 1)     //  插入成功后的受影响行数为1
                //{
                //    Console.WriteLine("插入会签单模版成功");
                //    return true;
                //}
                //else
                //{
                //    Console.WriteLine("插入会签单模版失败");
                //    return false;
                //}
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
                Console.WriteLine("修改会签单信息" + conTemp.TempId.ToString() + "成功");

                return true;
            }
            else
            {
                Console.WriteLine("修改会签单信息" + conTemp.TempId.ToString() + "失败");

                return false;
            }
        }
        #endregion

        #region 获取会签单模版的信息
        /// <summary>
        /// 获取到编号为conTempId的模版的信息
        /// </summary>
        /// <param name="conTempId"></param>
        /// <returns></returns>
        public static ContractTemplate GetContractTemplate(int conTempId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            ContractTemplate conTemp = null;
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = GET_CONTRACT_TEMPLATE_ID_STR;
                cmd.Parameters.AddWithValue("@Id", conTempId);                         // 员工编号


                MySqlDataReader sqlRead = cmd.ExecuteReader();

                cmd.Dispose();

                if (sqlRead.Read())
                {
                    conTemp = new ContractTemplate();


                    conTemp.TempId = int.Parse(sqlRead["id"].ToString());
                    conTemp.Name = sqlRead["name"].ToString();                   
                    
                    // 5个栏目信息
                    // conTemp.ColumnCount = 5;
                    List<String> columns = new List<String>();
                    /*
                    columns.Add(sqlRead["column1"].ToString());
                    columns.Add(sqlRead["column2"].ToString());
                    columns.Add(sqlRead["column3"].ToString());
                    columns.Add(sqlRead["column4"].ToString());
                    columns.Add(sqlRead["column5"].ToString());
                   */
                    for (int cnt = 1; cnt <= 5; cnt++)
                    {
                        String strColumn = "column" + cnt.ToString();
                        columns.Add(sqlRead[strColumn].ToString());
                    }
                    conTemp.ColumnNames = columns;

                    // 8个签字人信息
                    // conTemp.SignCount = 8;
                    List<SignatureTemplate> signatures = new List<SignatureTemplate>();
                    for (int cnt = 1; cnt <= 8; cnt++)
                    {
                        String strSignInfo = "signinfo" + cnt.ToString();
                        String strSignId = "signId" + cnt.ToString();
 
                        SignatureTemplate signDatas = new SignatureTemplate();
                        signDatas.SignInfo = sqlRead[strSignInfo].ToString();
                        signDatas.SignId = int.Parse(sqlRead[strSignId].ToString());

                    }
                    conTemp.SignDatas = signatures;

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
            return conTemp;
        }

        /// <summary>
        /// 获取到名称为conTempName的模版的信息
        /// </summary>
        /// <param name="conTempName"></param>
        /// <returns></returns>
        public static ContractTemplate GetContractTemplate(String conTempName)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            ContractTemplate conTemp = null;
            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = GET_CONTRACT_TEMPLATE_ID_STR;
                cmd.Parameters.AddWithValue("@Name", conTempName);                         // 员工编号

                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                if (sqlRead.Read())
                {
                    conTemp = new ContractTemplate();


                    conTemp.TempId = int.Parse(sqlRead["id"].ToString());
                    conTemp.Name = sqlRead["name"].ToString();
                    // 5个栏目信息
                    // conTemp.ColumnCount = 5;
                    List<String> columns = new List<String>();
                    /*
                    columns.Add(sqlRead["column1"].ToString());
                    columns.Add(sqlRead["column2"].ToString());
                    columns.Add(sqlRead["column3"].ToString());
                    columns.Add(sqlRead["column4"].ToString());
                    columns.Add(sqlRead["column5"].ToString());
                   */
                    for (int cnt = 1; cnt <= 5; cnt++)
                    {
                        String strColumn = "column" + cnt.ToString();
                        columns.Add(sqlRead[strColumn].ToString());
                    }
                    conTemp.ColumnNames = columns;

                    // 8个签字人信息
                    // conTemp.SignCount = 8;
                    List<SignatureTemplate> signatures = new List<SignatureTemplate>();
                    for (int cnt = 1; cnt <= 8; cnt++)
                    {
                        String strSignInfo = "signinfo" + cnt.ToString();
                        String strSignId = "signId" + cnt.ToString();

                        SignatureTemplate signDatas = new SignatureTemplate();
                        signDatas.SignInfo = sqlRead[strSignInfo].ToString();
                        signDatas.SignId = int.Parse(sqlRead[strSignId].ToString());

                    }
                    conTemp.SignDatas = signatures;

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
            return conTemp;
        }
        #endregion
    }
}
