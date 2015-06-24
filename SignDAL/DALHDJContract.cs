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
            return GetHDJContactRefuse(contractId);
        }
        #endregion



        #region 获取到会签单更加详细的信息(特使用在查询被拒绝的单子)

        private const String GET_HDJCONTRACT_REFUSE_STR = @"SELECT h.id id, h.name name, c.id contempid, c.name contempname, 
c.column1 columnname1, c.column2 columnname2, c.column3 columnname3, c.column4 columnname4, c.column5 columnname5,
h.columndata1 columndata1, h.columndata2 columndata2, h.columndata3 columndata3, h.columndata4 columndata4, h.columndata5 columndata5, 
c.signinfo1 signinfo1, c.signinfo2 signinfo2, c.signinfo3 signinfo3, c.signinfo4 signinfo4, c.signinfo5 signinfo5, c.signinfo5 signinfo5, c.signinfo6 signinfo6, c.signinfo7 signinfo7, c.signinfo8 signinfo8,                                                                  
e1.id signid1, e2.id signid2, e3.id signid3, e4.id signid4, e5.id signid5, e6.id signid6, e7.id signid7, e8.id signid8,
e1.name signname1, e2.name signname2, e3.name signname3, e4.name signname4, e5.name signname5, e6.name signname6, e7.name signname7, e8.name signname8,          
d1.id departmentid1, d2.id departmentid2, d3.id departmentid3, d4.id departmentid4, d5.id departmentid5, d6.id departmentid6, d7.id departmentid7, d8.id departmentid8,
d1.name departmentname1, d2.name departmentname2, d3.name departmentname3, d4.name departmentname4, d5.name departmentname5, d6.name departmentname6, d7.name departmentname7, d8.name departmentname8,
signlevel1 signlevel1, c.signlevel2, c.signlevel2, c.signlevel3, signlevel3, c.signlevel4 signlevel4, c.signlevel5 signlevel5, c.signlevel6 signlevel6, c.signlevel7 signlevel7, c.signlevel8 signlevel8,
s.result1 result1, s.result2 result2, s.result3 result3, s.result4 result4, s.result5 result5, s.result6 result6, s.result7 result7, s.result8 result8,

(SELECT sd.remark remark1
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = @Id and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '1' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) remark1,

(SELECT sd.remark remark2
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = @Id and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '2' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) remark2,

  (SELECT sd.remark remark3
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = @Id and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '3' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) remark3,
  (SELECT sd.remark remark4
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = @Id and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '4' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) remark4,
  (SELECT sd.remark remark5
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = @Id and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '5' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) remark5,
  (SELECT sd.remark remark6
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = @Id and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '6' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) remark6,

  (SELECT sd.remark remark7
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = @Id and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '7' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) remark7,
    
  (SELECT sd.remark remark8
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = @Id and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '8' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) remark8

FROM 

hdjcontract h, 
contemp c, 
signaturestatus s,
employee e1, employee e2, employee e3, employee e4, employee e5, employee e6, employee e7, employee e8,
department d1, department d2, department d3, department d4, department d5, department d6, department d7, department d8

WHERE (h.id = @Id and h.contempid = c.id
and c.signid1 = e1.id  and c.signid2 = e2.id and c.signid3 = e3.id and c.signid4 = e4.id and c.signid5 = e5.id and c.signid6 = e6.id and c.signid7 = e7.id and c.signid8 = e8.id
and d1.id = e1.departmentid and d2.id = e2.departmentid and d3.id = e3.departmentid and d4.id = e4.departmentid and d5.id = e5.departmentid and d6.id = e6.departmentid and d7.id = e7.departmentid and d8.id = e8.departmentid
and h.id = s.conid);
";
        public static HDJContract GetHDJContactRefuse(String contractId)
        { 
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            HDJContract contract = new HDJContract();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = GET_HDJCONTRACT_REFUSE_STR;
                cmd.Parameters.AddWithValue("@Id", contractId);

                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {

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
                    List<String > signRemarks = new List<String>();
                    for (int cnt = 1; cnt <= 8; cnt++)
                    {
                        String strSignInfo = "signinfo" + cnt.ToString();
                        String strSignId = "signid" + cnt.ToString();
                        String strSignName = "signname" + cnt.ToString();
                        String strDepartmentId = "departmentid" + cnt.ToString();
                        String strDepartmentName = "departmentname" + cnt.ToString();
                        String strSignLevel = "signlevel" + cnt.ToString();
                        String strSignResult = "result" + cnt.ToString();
                        String strSignRemark = "remark" + cnt.ToString();


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
                        signRemarks.Add(sqlRead[strSignRemark].ToString());

                        signatures.Add(signDatas);
                    }
                    conTemp.SignDatas = signatures;
                    contract.ConTemp = conTemp;

                    contract.SignResults = signResults;
                    contract.SignRemarks = signRemarks;

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


        #region 获取已经完成签字的会签单的信息
        private const String GET_HDJCONTRACT_AGREE_STR = @"SELECT h.id id, h.name name, c.id contempid, c.name contempname, 
c.column1 columnname1, c.column2 columnname2, c.column3 columnname3, c.column4 columnname4, c.column5 columnname5,
h.columndata1 columndata1, h.columndata2 columndata2, h.columndata3 columndata3, h.columndata4 columndata4, h.columndata5 columndata5, 
c.signinfo1 signinfo1, c.signinfo2 signinfo2, c.signinfo3 signinfo3, c.signinfo4 signinfo4, c.signinfo5 signinfo5, c.signinfo5 signinfo5, c.signinfo6 signinfo6, c.signinfo7 signinfo7, c.signinfo8 signinfo8,                                                                  
e1.id signid1, e2.id signid2, e3.id signid3, e4.id signid4, e5.id signid5, e6.id signid6, e7.id signid7, e8.id signid8,
e1.name signname1, e2.name signname2, e3.name signname3, e4.name signname4, e5.name signname5, e6.name signname6, e7.name signname7, e8.name signname8,          
d1.id departmentid1, d2.id departmentid2, d3.id departmentid3, d4.id departmentid4, d5.id departmentid5, d6.id departmentid6, d7.id departmentid7, d8.id departmentid8,
d1.name departmentname1, d2.name departmentname2, d3.name departmentname3, d4.name departmentname4, d5.name departmentname5, d6.name departmentname6, d7.name departmentname7, d8.name departmentname8,
signlevel1 signlevel1, c.signlevel2, c.signlevel2, c.signlevel3, signlevel3, c.signlevel4 signlevel4, c.signlevel5 signlevel5, c.signlevel6 signlevel6, c.signlevel7 signlevel7, c.signlevel8 signlevel8,
s.result1 result1, s.result2 result2, s.result3 result3, s.result4 result4, s.result5 result5, s.result6 result6, s.result7 result7, s.result8 result8,
s1.remark remark1, s2.remark remark2, s3.remark remark3, s4.remark remark4, s5.remark remark5, s6.remark remark6, s7.remark remark7, s8.remark remark8
FROM 

hdjcontract h, 
contemp c, 
signaturestatus s,
employee e1, employee e2, employee e3, employee e4, employee e5, employee e6, employee e7, employee e8,
department d1, department d2, department d3, department d4, department d5, department d6, department d7, department d8,
signaturedetail s1, signaturedetail s2, signaturedetail s3, signaturedetail s4,
signaturedetail s5, signaturedetail s6, signaturedetail s7, signaturedetail s8

WHERE (h.id = @Id and h.contempid = c.id and h.id = s.conid
and c.signid1 = e1.id  and c.signid2 = e2.id and c.signid3 = e3.id and c.signid4 = e4.id and c.signid5 = e5.id and c.signid6 = e6.id and c.signid7 = e7.id and c.signid8 = e8.id
and d1.id = e1.departmentid and d2.id = e2.departmentid and d3.id = e3.departmentid and d4.id = e4.departmentid and d5.id = e5.departmentid and d6.id = e6.departmentid and d7.id = e7.departmentid and d8.id = e8.departmentid
 and s1.empid = e1.id and s1.conid = h.id and s1.updatecount = s.updatecount
 and s2.empid = e2.id and s2.conid = h.id and s2.updatecount = s.updatecount
 and s3.empid = e3.id and s3.conid = h.id and s3.updatecount = s.updatecount
 and s4.empid = e4.id and s4.conid = h.id and s4.updatecount = s.updatecount
 and s5.empid = e5.id and s5.conid = h.id and s5.updatecount = s.updatecount
 and s6.empid = e6.id and s6.conid = h.id and s6.updatecount = s.updatecount
 and s7.empid = e7.id and s7.conid = h.id and s7.updatecount = s.updatecount
 and s8.empid = e8.id and s8.conid = h.id and s8.updatecount = s.updatecount)";
        public static HDJContract GetHDJContactAgree(String contractId)
        { 
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            HDJContract contract = new HDJContract();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = GET_HDJCONTRACT_AGREE_STR;
                cmd.Parameters.AddWithValue("@Id", contractId);

                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {

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
                    List<String > signRemarks = new List<String>();
                    for (int cnt = 1; cnt <= 8; cnt++)
                    {
                        String strSignInfo = "signinfo" + cnt.ToString();
                        String strSignId = "signid" + cnt.ToString();
                        String strSignName = "signname" + cnt.ToString();
                        String strDepartmentId = "departmentid" + cnt.ToString();
                        String strDepartmentName = "departmentname" + cnt.ToString();
                        String strSignLevel = "signlevel" + cnt.ToString();
                        String strSignResult = "result" + cnt.ToString();
                        String strSignRemark = "remark" + cnt.ToString();


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
                       // Console.WriteLine(1111);
                        signRemarks.Add(sqlRead[strSignRemark].ToString());

                        signatures.Add(signDatas);
                    }
                    conTemp.SignDatas = signatures;
                    contract.ConTemp = conTemp;

                    contract.SignResults = signResults;
                    contract.SignRemarks = signRemarks;

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
