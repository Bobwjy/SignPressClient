using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using System.Threading;



using SignPressServer.SignData;



using System.Data;
using MySql.Data.MySqlClient;
using SignPressServer.SignTools;


namespace SignPressServer.SignDAL
{
    /*
     *  签字明细表
     */
    public class DALSignatureDetail
    {

        #region 数据库处理信息串
        

        /// <summary>
        /// 插入签字明细的信息串
        /// </summary>
        private const String INSERT_SIGNATURE_DETAIL_STR = @"INSERT INTO `signaturedetail` (`date`, `empid`, `conid`, `result`, `remark`) 
                                                             VALUES (@Date, @EmpId, @ConId, @Result, @Remark)";

        /// <summary>
        /// 删除签字明细的信息串
        /// </summary>
        private const String DELETE_SIGNATURE_DETAIL_STR = @"DELETE FROM `signaturedetail` WHERE (`id` = @Id)";

        /// <summary>
        /// 修改签字明细的信息串
        /// </summary>
        private const String MODIFY_SIGNATURE_DETAIL_STR = @"";

        /// <summary>
        /// 查询签字明细的信息串
        /// </summary>
        private const String QUERY_SIGNATURE_DETAIL_STR = @"SELECT * FROM signaturedetail ORDER BY `id` DESC";
        private const String QUERY_SIGNATURE_DETAIL_EMP_STR = @"SELECT * FROM signaturedetail WHERE (`empid` = @EmpId) ORDER BY `id` DESC";
        private const String QUERY_SIGNATURE_DETAIL_CON_STR = @"SELECT * FROM signaturedetail WHERE (`conid` = @ConId) ORDER BY `id` DESC";
        private const String QUERY_SIGNATURE_DETAIL_DATE_STR = @"SELECT * FROM signaturedeail WHERE (`date` > @dateBegin and `date` < @DateEnd)";
        private const String QUERY_SIGNATURE_DETAIL_EMP_CON_STR = @"SELECT * FROM signaturedetail WHERE (`empid` = @EmpId and `conid` = @ConId)";          
        
        /// <summary>
        /// 获取某个签字明细的信息串
        /// </summary>
        private const String GET_SIGNATURE_DETAIL_STR = @"";

        /// <summary>
        /// 设置签字同意的信息串
        /// </summary>
        private const String SET_SIGNATURE_DETAIL_AGREE_STR = @"";

        /// <summary>
        /// 设置签字拒绝的信息串
        /// </summary>
        private const String SET_SIGNATURE_DETAIL_REFUSE_STR = @"";

        #endregion


        #region  插入签字明细信息
        public static void CreateHDJContractTrigger(object conId)
        {
            String pdfPath = MSWordTools.DEFAULT_HDJCONTRACT_PATH + conId + ".pdf";
            if (!(File.Exists((String)pdfPath)))     // 首先检测文件是否存在
            {
                String wordPath = MSWordTools.DEFAULT_HDJCONTRACT_PATH + conId + ".doc";
                HDJContract contract = DALHDJContract.GetHDJContactAgree((string)conId);       // 获取待生成的会签单信息
                MSWordTools.CreateHDJContractWordWithReplace(contract, wordPath);
                MSWordTools.WordConvertToPdf(wordPath, pdfPath);

                File.Delete((String)wordPath);
                MSWordTools.KillWordProcess();
            }
        }
        
        /// <summary>
        /// 插入签字明细信息
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// 
        public static bool InsertSignatureDetail(SignatureDetail detail)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();

            MySqlCommand cmd;
            int count = -1;                      // 受影响行数
            try
            {
                con.Open();

                cmd = con.CreateCommand();
                cmd.CommandText = INSERT_SIGNATURE_DETAIL_STR;
                //cmd.Parameters.AddWithValue("@Id", System.DateTime.Now.ToString("yyyyMMddHHmmss"));
                cmd.Parameters.AddWithValue("@Date", System.DateTime.Now);                  // 签字明细职位
                cmd.Parameters.AddWithValue("@EmpId", detail.EmpId);
                cmd.Parameters.AddWithValue("@ConId", detail.ConId);
                cmd.Parameters.AddWithValue("@Result", detail.Result);
                cmd.Parameters.AddWithValue("@Remark", detail.Remark);

                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();
                if (count == 1)     //  插入成功后的受影响行数为1
                {
                    Console.WriteLine("签字明细插入成功");
                    return true;
                }
                else
                {
                    Console.WriteLine("签字明细插入失败");
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

                // 每次进行签字以后需要判断一下当前阶段是否走完，如果完结则阶段  [计数器 + 1]
                DALSignatureDetail.SetSignatureCurrLevelTrigger(detail.ConId);
                
                ////  会签单信息已经完成，直接生成会签单信息
                //if (dalsignaturedetail.iscontractagree(detail.conid) == true)
                //{

                //    // 创建后台线程生成会签单信息
                //    parameterizedthreadstart pts = new parameterizedthreadstart(createhdjcontracttrigger);
                //    thread thradrecmsg = new thread(pts);
                //    thradrecmsg.isbackground = true;
                //    thradrecmsg.start(detail.conid);

                //}
            }


        }
        #endregion



        #region 删除签字明细信息
        /// <summary>
        /// 删除签字明细的信息DeleteEmployee
        /// </summary>
        /// <param name="id">签字明细的签字明细号</param>
        /// <returns></returns>
        public static bool DeleteSignatureDetail(int detailId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;
            int count = -1;

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = DELETE_SIGNATURE_DETAIL_STR;
                //cmd.Parameters.AddWithValue("@Id", detailId.ToString());                        // 签字明细姓名


                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();

                if (count == 1)
                {
                    Console.WriteLine("删除签字明细" + detailId.ToString() + "成功");
                    return true;
                }
                else
                {
                    Console.WriteLine("删除签字明细" + detailId.ToString() + "失败");
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


        #region 查询所有签字明细的信息
        /// <summary>
        /// 查询所有签字明细的信息
        /// </summary>
        /// <returns></returns>
        public static List<SignatureDetail> QuerySignatureDetail()
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<SignatureDetail> details = new List<SignatureDetail>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_SIGNATURE_DETAIL_STR;


                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    SignatureDetail detail = new SignatureDetail();

                    //detail.Id = sqlRead["id"].ToString();
                    detail.Date = sqlRead["date"].ToString();
                    detail.EmpId = int.Parse(sqlRead["empid"].ToString());
                    detail.ConId = sqlRead["conid"].ToString();
                    detail.Result = int.Parse(sqlRead["result"].ToString());
                    detail.Remark = sqlRead["remark"].ToString();
                    
                    details.Add(detail);
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
            return details;
        }
        #endregion

        #region 查询与empid相关的所有的签字信息
        public static List<SignatureDetail> QuerySignatureDetail(int empid)
        {
            MySqlConnection con = DBTools.GetMySqlConnection( );
            MySqlCommand cmd;

            List<SignatureDetail> details = new List<SignatureDetail>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_SIGNATURE_DETAIL_EMP_STR;
                cmd.Parameters.AddWithValue("@empid", empid);


                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    SignatureDetail detail = new SignatureDetail();

                   // detail.Id = sqlRead["id"].ToString();
                    detail.Date = sqlRead["date"].ToString();
                    detail.EmpId = int.Parse(sqlRead["empid"].ToString());
                    detail.ConId = sqlRead["conid"].ToString();
                    detail.Result = int.Parse(sqlRead["result"].ToString());
                    detail.Remark = sqlRead["remark"].ToString();
                    
                    details.Add(detail);
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
            return details;
        }
        #endregion

        #region 查询与empid相关的所有的签字信息
        public static List<SignatureDetail> QuerySignatureDetail(int empId, String conId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<SignatureDetail> details = new List<SignatureDetail>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_SIGNATURE_DETAIL_EMP_CON_STR;
                cmd.Parameters.AddWithValue("@Empid", empId);
                cmd.Parameters.AddWithValue("@ConId", conId);

                MySqlDataReader sqlRead = cmd.ExecuteReader();


                cmd.Dispose();

                while (sqlRead.Read())
                {
                    SignatureDetail detail = new SignatureDetail();

                    //detail.Id = sqlRead["id"].ToString();
                    detail.Date = sqlRead["date"].ToString();
                    detail.EmpId = int.Parse(sqlRead["empid"].ToString());
                    detail.ConId = sqlRead["conid"].ToString();
                    detail.Result = int.Parse(sqlRead["result"].ToString());
                    detail.Remark = sqlRead["remark"].ToString();

                    details.Add(detail);
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
            return details;
        }
        #endregion


        private const String SET_SIGNATURESTATUS_CURRLEVEL = @"UPDATE signaturestatus 
SET currlevel = currlevel+ 1
WHERE (((SELECT count(sl.empid) 
FROM (SELECT conid,currlevel, totalresult, updatecount  FROM signaturestatus) st2, signaturelevel  sl, hdjcontract hc, signaturedetail sd
WHERE (st2.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st2.currlevel and st2.totalresult = 0
   and sd.conid = hc.id and sd.empid = sl.empid and sd.result = 1 and sd.updatecount = st2.updatecount
   and hc.id = @Id)) 
= 
(SELECT count(sl.empid)
FROM (SELECT conid,currlevel, totalresult  FROM signaturestatus) st2, signaturelevel  sl, hdjcontract hc
WHERE (st2.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st2.currlevel and st2.totalresult = 0
   and hc.id = @Id))) and conid = @Id);";
        
        private static bool SetSignatureCurrLevelTrigger(String contractId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();

            MySqlCommand cmd;
            int count = -1;                      // 受影响行数
            try
            {
                con.Open();

                cmd = con.CreateCommand();
                cmd.CommandText = SET_SIGNATURESTATUS_CURRLEVEL;
                //cmd.Parameters.AddWithValue("@Id", System.DateTime.Now.ToString("yyyyMMddHHmmss"));
                cmd.Parameters.AddWithValue("@Id", contractId);
                count = cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                con.Dispose();
                if (count == 1)     //  插入成功后的受影响行数为1
                {
                    Console.WriteLine("==前一个阶段的签字流程已经走完，进入下一个流程==");
                    return true;
                }
                else
                {
                    Console.WriteLine("==前一个阶段的签字流程还没有走完==");
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

        private const String IS_CONTRACT_AGREE_STR = "SELECT Count(id) count FROM `signaturestatus` WHERE (totalresult = 1 and conid = @ConId)";

        public static bool IsContractAgree(String contractId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();

            MySqlCommand cmd;
 
            int count = 0;                      // 受影响行数
 
            try
            {
                con.Open();

                cmd = con.CreateCommand();
                cmd.CommandText = IS_CONTRACT_AGREE_STR;

                cmd.Parameters.AddWithValue("@ConId", contractId);
                MySqlDataReader sqlReader = cmd.ExecuteReader();
                cmd.Dispose();


                while(sqlReader.Read())
                {
                    count = int.Parse(sqlReader["count"].ToString());
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
            if (count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
