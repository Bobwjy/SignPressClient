using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;





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
        /// 插入签字状态的信息串
        /// </summary>
        private const String INSERT_SIGNATURE_DETAIL_STR = @"INSERT INTO `signaturedetail` (`id`, `date`, `empid`, `conid`, `result`, `remark`) 
                                                             VALUES (@Id, @Date, @EmpId, @ConId, @Result, @Remark)";

        /// <summary>
        /// 删除签字状态的信息串
        /// </summary>
        private const String DELETE_SIGNATURE_DETAIL_STR = @"DELETE FROM `signaturedetail` WHERE (`id` = @Id)";

        /// <summary>
        /// 修改签字状态的信息串
        /// </summary>
        private const String MODIFY_SIGNATURE_DETAIL_STR = @"";

        /// <summary>
        /// 查询签字状态的信息串
        /// </summary>
        private const String QUERY_SIGNATURE_DETAIL_STR = @"SELECT * FROM signaturedetail ORDER BY `id`";
        private const String QUERY_SIGNATURE_DETAIL_SIGNEMPID_STR = @"SELECT * FROM signaturedetail WHERE (`signempid` = @SignEmpId) ORDER BY `id`";
        private const String QUERY_SIGNATURE_DETAIL_SIGCONPID_STR = @"SELECT * FROM signaturedetail WHERE (`signconid` = @SignConId) ORDER BY `id`";
        /// <summary>
        /// 获取某个签字状态的信息串
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
        /// <summary>
        /// 插入签字明细信息
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
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
                cmd.Parameters.AddWithValue("@Id", System.DateTime.Now.ToString("yyyyMMddHHmmss"));
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
                cmd.Parameters.AddWithValue("@Id", detailId.ToString());                        // 签字明细姓名


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

                    detail.Id = sqlRead["id"].ToString();
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

        #region 查询与signEmpId相关的所有的签字信息
        public static List<SignatureDetail> QuerySignatureDetail(int signEmpId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection( );
            MySqlCommand cmd;

            List<SignatureDetail> details = new List<SignatureDetail>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_SIGNATURE_DETAIL_SIGNEMPID_STR;


                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Parameters.AddWithValue("@SignEmpId", signEmpId);
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    SignatureDetail detail = new SignatureDetail();

                    detail.Id = sqlRead["id"].ToString();
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
    }
}
