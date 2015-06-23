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
                                                        VALUES (@Id, @Name, @ConTempId, @SubEmpId, @SubmitDate, @ColumnData_1, @ColumnData_2, @ColumnData_3, @ColumnData_4, @ColumnData_5)";

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
        private const String QUERY_HDJCONTRACT_STR = @"SELECT `id`, `name`, `subempid`, `subempname`, `submitdate` FROM `hdjcontract`";


        private const String GET_HDJCONTRACT_STR = @"SELECT h.id id, h.name name, c.id contempid, c.name contempname, 
c.column1 columnname1, c.column2 columnname2, c.column3 columnname3, c.column4 columnname4, c.column5 columnname5,
h.columndata1 columndata1, h.columndata2 columndata2, h.columndata3 columndata3, h.columndata4 columndata4, h.columndata5 columndata5, 
c.signinfo1 signinfo1, c.signinfo2 signinfo2, c.signinfo3 signinfo3, c.signinfo4 signinfo4, c.signinfo5 signinfo5, c.signinfo5 signinfo5, c.signinfo6 signinfo6, c.signinfo7 signinfo7, c.signinfo8 signinfo8,                                                                  
e1.id signid1, e2.id signid2, e3.id signid3, e4.id signid4, e5.id signid5, e6.id signid6, e7.id signid7, e8.id signid8,
e1.name signname1, e2.name signname2, e3.name signname3, e4.name signname4, e5.name signname5, e6.name signname6, e7.name signname7, e8.name signname8,          
d1.id departmentid1, d2.id departmentid2, d3.id departmentid3, d4.id departmentid4, d5.id departmentid5, d6.id departmentid6, d7.id departmentid7, d8.id departmentid8,
d1.name departmentname1, d2.name departmentname2, d3.name departmentname3, d4.name departmentname4, d5.name departmentname5, d6.name departmentname6, d7.name departmentname7, d8.name departmentname8,
signlevel1 signlevel1, c.signlevel2, c.signlevel2, c.signlevel3, signlevel3, c.signlevel4 signlevel4, c.signlevel5 signlevel5, c.signlevel6 signlevel6, c.signlevel7 signlevel7, c.signlevel8 signlevel8,
s.result1 result1, s.result2 result2, s.result3 result3, s.result4 result4, s.result5 result5, s.result6 result6, s.result7 result7, s.result8 result8
FROM hdjcontract h, contemp c, signaturestatus s,
employee e1, employee e2, employee e3, employee e4, employee e5, employee e6, employee e7, employee e8,
department d1, department d2, department d3, department d4, department d5, department d6, department d7, department d8
WHERE (h.id = @Id and h.contempid = c.id  
and c.signid1 = e1.id  and c.signid2 = e2.id and c.signid3 = e3.id and c.signid4 = e4.id and c.signid5 = e5.id and c.signid6 = e6.id and c.signid7 = e7.id and c.signid8 = e8.id
and d1.id = e1.departmentid and d2.id = e2.departmentid and d3.id = e3.departmentid and d4.id = e4.departmentid and d5.id = e5.departmentid and d6.id = e6.departmentid and d7.id = e7.departmentid and d8.id = e8.departmentid
and h.id = s.conid);";

        
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


                /// 修改编号的设置
                //////////////////////////////////////////////////////////////////////
                contract.Id = System.DateTime.Now.ToString("yyyyMMddHHmmss");/////////
                //////////////////////////////////////////////////////////////////////
                cmd.Parameters.AddWithValue("@Id", contract.Id);
                cmd.Parameters.AddWithValue("@Name", contract.Name);
                cmd.Parameters.AddWithValue("@ConTempId", contract.ConTemp.TempId);
                cmd.Parameters.AddWithValue("@SubEmpId", contract.SubmitEmployee.Id);
                cmd.Parameters.AddWithValue("@SubmitDate", System.DateTime.Now);

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


        #region 查询会签单的信息
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


        #region 查询会签单模版的信息
        public static HDJContract GetHDJContract(String contractId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            HDJContract contract = new HDJContract();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = GET_HDJCONTRACT_STR;
                cmd.Parameters.AddWithValue("@Id", contractId);

                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    /*
                     h.id id, c.id contempid, c.name contempname, 
c.column1 columnname1, c.column2 columnname2, c.column3 columnname3, c.column4 columnname4, c.column5 columnname5,
h.columndata1 columndata1, h.columndata2 columndata2, h.columndata3 columndata3, h.columndata4 columndata4, h.columndata5 columndata5, 
c.signinfo1 signinfo1, c.signinfo2 signinfo2, c.signinfo3 signinfo3, c.signinfo4 signinfo4, c.signinfo5 signinfo5, c.signinfo6 signinfo6, c.signinfo7 signinfo7, c.signinfo8 signinfo8,                                                                  
e1.id signid1, e2.id signid2, e3.id signid3, e4.id signid4, e5.id signid5, e6.id signid6, e7.id signid7, e8.id signid8,
e1.name signname1, e2.name signname2, e3.name signname3, e4.name signname4, e5.name signname5, e6.name signname6, e7.name signname7, e8.name signname8,          
d1.id departmentid1, d2.id departmentid2, d3.id departmentid3, d4.id departmentid4, d5.id departmentid5, d6.id departmentid6, d7.id departmentid7, d8.id departmentid8,
d1.name departmentname1, d2.name departmentname2, d3.name departmentname3, d4.name departmentname4, d5.name departmentname5, d6.name departmentname6, d7.name departmentname7, d8.name departmentname8,
signlevel1 signlevel1, c.signlevel2, c.signlevel2, c.signlevel3, signlevel3, c.signlevel4 signlevel4, c.signlevel5 signlevel5, c.signlevel6 signlevel6, c.signlevel7 signlevel7, c.signlevel8 signlevel8,
s.result1 result1, s.result2 result2, s.result3 result3, s.result4 result4, s.result5 resul5, s.result6 result6, s.result7 resiult7, s.result8 result8
                     */
                    contract.Id = sqlRead["id"].ToString();
                    contract.Name = sqlRead["name"].ToString();
                    ContractTemplate conTemp = new ContractTemplate();
                    conTemp.TempId = int.Parse(sqlRead["contempid"].ToString());
                    conTemp.Name = sqlRead["contempname"].ToString();
                    // 5个栏目信息
                    // conTemp.ColumnCount = 5;
                    List<String> columnnames = new List<String>();
                    List<String> columndatas = new List<String>();
                    for (int cnt = 1; cnt <= 5; cnt++)
                    {
                        String strColumnname = "columnname" + cnt.ToString();
                        String strColumnData = "columndata" + cnt.ToString();
                        columnnames.Add(sqlRead[strColumnname].ToString());
                        columndatas.Add(sqlRead[strColumnData].ToString());
                    }
                    conTemp.ColumnNames = columnnames;
                    contract.ColumnDatas = columndatas;

                    // 8个签字人信息
                    // conTemp.SignCount = 8;
                    List<SignatureTemplate> signatures = new List<SignatureTemplate>();
                    List<int> signResults = new List<int>();
                    for (int cnt = 1; cnt <= 8; cnt++)
                    {
                        String strSignInfo = "signinfo" + cnt.ToString();
                        String strSignId = "signid" + cnt.ToString();
                        String strSignName = "signname" + cnt.ToString();
                        String strDepartmentId = "departmentid" + cnt.ToString();
                        String strDepartmentName = "departmentname" + cnt.ToString();
                        String strSignLevel = "signlevel" + cnt.ToString();
                        String strSignResult = "result" + cnt.ToString();

                        SignatureTemplate signDatas = new SignatureTemplate();
                        signDatas.SignInfo = sqlRead[strSignInfo].ToString();
                        signDatas.SignLevel = int.Parse(sqlRead[strSignLevel].ToString());
                        Employee employee = new Employee();
                        employee.Id = int.Parse(sqlRead[strSignId].ToString());
                        employee.Name = sqlRead[strSignName].ToString();
                        Department department = new Department();
                        department.Id = int.Parse(sqlRead[strDepartmentId].ToString());
                        department.Name = sqlRead[strDepartmentName].ToString();
                        employee.Department = department;
                        signDatas.SignEmployee = employee;
                        
                        // 8个人的签字结果
                        signResults.Add(int.Parse(sqlRead[strSignResult].ToString()));


                        signatures.Add(signDatas);
                    }
                    conTemp.SignDatas = signatures;
                    contract.ConTemp = conTemp;

                    contract.SignResults = signResults;

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
            return contract;
        }
        #endregion




    }
}
