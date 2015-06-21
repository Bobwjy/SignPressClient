using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using SignPressServer.SignData;

using SignPressServer.SignTools;
using System.Data;
using MySql.Data.MySqlClient;
 

namespace SignPressServer.SignDAL
{
    /*
     *  处理签字状态表信息
     *  本表全是触发器
     */
    public class DALSignatureStatus
    {

        /*
         * 基本功能有
         * 
         * 查看编号为contractId的单子的审核状态
         * 
         * 查看用户id为employeeId所提交的单子的审核状态
         * 
         */

        #region 数据库操作数据串
        /// <summary>
        /// 删除签字状态的信息串
        /// </summary>
        private const String DELETE_SIGNATURE_STATUS_STR = @"";

        /// <summary>
        /// 查询签字状态的信息串
        /// </summary>
        private const String QUERY_SIGNATURE_STATUS_PENDDING_STR = @"SELECT h.id id, h.name name, h.submitdate submitdate, h.columndata1 columndata1, s.currlevel currlevel, s.maxlevel maxlevel
                                                                    FROM `signaturestatus` s, `hdjcontract` h
                                                                    WHERE (s.totalresult = 0 and h.id = s.conid)
                                                                    ORDER BY id";
        private const String QUERY_SIGNATURE_STATUS_AGREE_STR = @"SELECT h.id id, h.name name, h.submitdate submitdate, h.columndata1 columndata1, s.currlevel currlevel, s.maxlevel maxlevel
                                                                  FROM `signaturestatus` s, `hdjcontract` h
                                                                  WHERE (s.totalresult = 1 and h.id = s.conid)
                                                                  ORDER BY id";

        private const String QUERY_SIGNATURE_STATUS_REFUSE_STR = @"SELECT h.id id, h.name name, h.submitdate submitdate, h.columndata1 columndata1, s.currlevel currlevel, s.maxlevel maxlevel
                                                                   FROM `signaturestatus` s, `hdjcontract` h
                                                                   WHERE (s.totalresult = -1 and h.id = s.conid)
                                                                   ORDER BY id";

        private const String QUERY_SIGNATURE_STATUS_PENDDING_EMP_STR = @"SELECT h.id id, h.name name, h.submitdate submitdate, h.columndata1 columndata1, s.currlevel currlevel, s.maxlevel maxlevel
                                                                    FROM `signaturestatus` s, `hdjcontract` h
                                                                    WHERE (s.totalresult = 0 and h.id = s.conid and h.subempid = @EmployeeId)";
        private const String QUERY_SIGNATURE_STATUS_AGREE_EMP_STR = @"SELECT h.id id, h.name name, h.submitdate submitdate, h.columndata1 columndata1, s.currlevel currlevel, s.maxlevel maxlevel
                                                                      FROM `signaturestatus` s, `hdjcontract` h
                                                                      WHERE (s.totalresult = 1 and h.id = s.conid and h.subempid = @EmployeeId)
                                                                      ORDER BY id";

        private const String QUERY_SIGNATURE_STATUS_REFUSE_EMP_STR = @"SELECT h.id id, h.name name, h.submitdate submitdate, h.columndata1 columndata1, s.currlevel currlevel, s.maxlevel maxlevel
                                                                   FROM `signaturestatus` s, `hdjcontract` h
                                                                   WHERE (s.totalresult = -1 and h.id = s.conid and h.subempid = @EmployeeId)
                                                                   ORDER BY id";
        /// <summary>
        /// 获取某个签字状态的信息串
        /// </summary>
        private const String GET_SIGNATURE_STATUS_STR = @"";


        #endregion


        #region 触发器信息串
        /*
         * 关于触发器        
         * 对于INSERT语句,只有NEW是合法的；
         * 对于DELETE语句，只有OLD才合法；
         * 而UPDATE语句可以在和NEW以及OLD同时使用。
        */


        /*首先，
         * 签字状态表是对外是一个只读表，其数据的修改，
         * 由数据库触发器进行维护
         * 用户提交签单时（即用户在数据库插入或者修改签单之后），
         * 通过触发器在数据库signaturestatus表中插入一项数据，数据项全为未处理
         */

        /// <summary>
        /// 用户提交签单时（即用户在数据库插入或者修改签单之后），
        ///  通过触发器在数据库signaturestatus表中插入一项数据，数据项全为未处理
        ///  2015-06-21 11:47:25触发器测试完整
        /// </summary>
        private const String INSERT_SIGNATURE_STATUS_TRIGGER = @"
        CREATE trigger insert_signature_status =
        AFTER INSERT on `hdjcontract` 
        FOR EACH ROW 
        BEGIN

            INSERT INTO `signaturestatus` (`id`, `conid`, `result1`, `result2`, `result3`, `result4`, `result5`, `result6`, `result7`, `result8`, `totalresult`, `agreecount`, `refusecount`, `currlevel`, `maxlevel`) 
            VALUES (NOW(), new.id, '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', (SELECT c.signlevel8 FROM `hdjcontract` h, `contemp` c WHERE (h.contempid = c.id and h.id = new.id)));

        END;";  
  /*      BEGIN
INSERT INTO `signaturestatus` (`id`, `conid`, `result1`, `result2`, `result3`, `result4`, `result5`, `result6`, `result7`, `result8`, `signtotalresult`) 
VALUES (NOW(), new.id, '0', '0', '0', '0', '0', '0', '0', '0', '0');

*/
         /*
          * 用户每次签字，将会在签字明细表中插入一行数据
          * 通过触发器在在签字明细表signaturestatus中，
          * 置对应数据项为[同意或者拒绝]
          * 
          * 置数要求两个选项
          * 第一个是signaturestatus的对应会签单conid同signaturestatus的会签单编号相同，也就是signaturestatus.conid = new.conid
          * 第二个是signaturestatus的对应的签字人result[X]同是其对应模版的签字人
          * 第一个比较复杂，我们通过hdjcontract.id = new.conid查找出对应的会签单表的模版h.contempid = c.id
          * 然后通过模版表c.signid[X] = e.id查找出第X个人的信息
         */
        private const String MODIFY_SIGNATURE_STATUS_SIGNID_TRIGGER = @"
        CREATE trigger modify_signature_status
        AFTER INSERT on `signaturedetail`
        FOR EACH ROW
        BEGIN
            UPDATE `signaturestatus`
            SET result1 = new.result 
            WHERE (signaturestatus.conid = new.conid 
               and new.empid = (SELECT e.id FROM `hdjcontract` h, contemp c, employee e WHERE (h.id = signaturestatus.conid and h.contempid = c.id and c.signid1 = e.id)));
        
            UPDATE `signaturestatus`
            SET result2 = new.result 
            WHERE (signaturestatus.conid = new.conid 
               and new.empid = (SELECT e.id FROM `hdjcontract` h, contemp c, employee e WHERE (h.id = signaturestatus.conid and h.contempid = c.id and c.signid2 = e.id)));
            
            UPDATE `signaturestatus`
            SET result3 = new.result 
            WHERE (signaturestatus.conid = new.conid 
               and new.empid = (SELECT e.id FROM `hdjcontract` h, contemp c, employee e WHERE (h.id = signaturestatus.conid and h.contempid = c.id and c.signid3 = e.id)));
            
            UPDATE `signaturestatus`
            SET result4 = new.result 
            WHERE (signaturestatus.conid = new.conid 
               and new.empid = (SELECT e.id FROM `hdjcontract` h, contemp c, employee e WHERE (h.id = signaturestatus.conid and h.contempid = c.id and c.signid4 = e.id)));
            
            UPDATE `signaturestatus`
            SET result5 = new.result 
            WHERE (signaturestatus.conid = new.conid 
               and new.empid = (SELECT e.id FROM `hdjcontract` h, contemp c, employee e WHERE (h.id = signaturestatus.conid and h.contempid = c.id and c.signid5 = e.id)));
            
            UPDATE `signaturestatus`
            SET result6 = new.result 
            WHERE (signaturestatus.conid = new.conid 
               and new.empid = (SELECT e.id FROM `hdjcontract` h, contemp c, employee e WHERE (h.id = signaturestatus.conid and h.contempid = c.id and c.signid6 = e.id)));
            
            UPDATE `signaturestatus`
            SET result7 = new.result 
            WHERE (signaturestatus.conid = new.conid 
               and new.empid = (SELECT e.id FROM `hdjcontract` h, contemp c, employee e WHERE (h.id = signaturestatus.conid and h.contempid = c.id and c.signid7 = e.id)));
        
            UPDATE `signaturestatus`
            SET result8 = new.result 
            WHERE (signaturestatus.conid = new.conid 
               and new.empid = (SELECT e.id FROM `hdjcontract` h, contemp c, employee e WHERE (h.id = signaturestatus.conid and h.contempid = c.id and c.signid8 = e.id)));
        
        END;";



        #endregion



        #region 查询所有正在审核中的所有的会签单
        /// <summary>
        /// 查询出员工编号为employeeId的所有正在审核的签单的信息
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public static List<HDJContract> QuerySignatureStatusPendding(int employeeId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<HDJContract> contracts = new List<HDJContract>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_SIGNATURE_STATUS_PENDDING_EMP_STR;
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    HDJContract contract = new HDJContract();
                    contract.Id = sqlRead["id"].ToString();
                    contract.Name = sqlRead["name"].ToString();
                    contract.SubmitDate = sqlRead["submitdate"].ToString();

                    List<String> columnDatas = new List<String>();
                    String columnData1 = sqlRead["columndata1"].ToString();
                    columnDatas.Add(columnData1);

                    contract.ColumnDatas = columnDatas;

                    contract.CurrLevel = int.Parse(sqlRead["currlevel"].ToString());
                    contract.MaxLevel = int.Parse(sqlRead["maxlevel"].ToString());

                    contracts.Add(contract);
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

        #region 查询所有已经通过的所有的签字单子
        public static List<HDJContract> QuerySignatureStatusAgree(int employeeId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<HDJContract> contracts = new List<HDJContract>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_SIGNATURE_STATUS_AGREE_EMP_STR;
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    HDJContract contract = new HDJContract();
                    contract.Id = sqlRead["id"].ToString();
                    contract.Name = sqlRead["name"].ToString();
                    contract.SubmitDate = sqlRead["submitdate"].ToString();

                    List<String> columnDatas = new List<String>();
                    String columnData1 = sqlRead["columndata1"].ToString();
                    columnDatas.Add(columnData1);

                    contract.ColumnDatas = columnDatas;

                    contract.CurrLevel = int.Parse(sqlRead["currlevel"].ToString());
                    contract.MaxLevel = int.Parse(sqlRead["maxlevel"].ToString());

                    contracts.Add(contract);
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


        #region 查询所有的被拒绝的所有的签字单子
        public static List<HDJContract> QuerySignatureStatusRefuse(int employeeId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<HDJContract> contracts = new List<HDJContract>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_SIGNATURE_STATUS_REFUSE_EMP_STR;
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    HDJContract contract = new HDJContract();
                    contract.Id = sqlRead["id"].ToString();
                    contract.Name = sqlRead["name"].ToString();
                    contract.SubmitDate = sqlRead["submitdate"].ToString();

                    List<String> columnDatas = new List<String>();
                    String columnData1 = sqlRead["columndata1"].ToString();
                    columnDatas.Add(columnData1);

                    contract.ColumnDatas = columnDatas;
                    contract.CurrLevel = int.Parse(sqlRead["currlevel"].ToString());
                    contract.MaxLevel = int.Parse(sqlRead["maxlevel"].ToString());

                    contracts.Add(contract);

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




        #region 查询某个人是否有未处理的签字单子
        /// <summary>
        /// 查询某个人是否有未签字的单子
        /// 当当前单子需要某个人签字的时候，需要满足几个条件
        /// 一是，这个会签单仍然需要签字，即签字流程还没走完,signaturestatus中，SQL表示为(h.contempid = c.id and s.conid = h.id)
        /// 二是，当前员工的ID在会签单模版中，即当前会签单需要此ID的员工签字,SQL语句表示为(c.signid[1~8] = @employeeId)
        /// 三是，这个会签单的当前进的节点currLevel正好等于当前员工的签字顺序号,
        /// </summary>
        /// 因此以下的串我们解决不了第三个问题
//        private const String QUERT_UNSIGN_CONTRACT_COM_STR = @"SELECT  h.id id, h.name name, h.submitdate submitdate, h.columndata1 columndata1
//                                                           FROM `hdjcontract` h, `contemp` c, `signaturestatus` s
//                                                           WHERE (h.contempid = c.id and s.conid = h.id
//                                                              and (c.signid1 = @employeeId or c.signid2 = @employeeId or c.signid3 = @EmployeeId or c.signid4 = @EmployeeId 
//                                                                or c.signid5 = @employeeId or c.signid6 = @employeeId or c.signid7 = @EmployeeId or c.signid8 = @EmployeeId))";
        /// <summary>
        /// 引入会签单模版签字顺序表signaturelevel表后，不再需要关联contemp表
        /// 当当前单子需要某个人签字的时候，需要满足几个条件
        /// 一是，这个会签单仍然需要签字，即签字流程还没走完,signaturestatus中，SQL表示为hc.id = st.conid[当前会签单的在待办会签单状态表中]
        /// 二是，当前员工的ID在会签单模版中，即当前会签单需要此ID的员工签字,SQL语句表示为sl.contempid = hc.contempid and sl.empid = @EmployeeId 
        /// 三是，这个会签单的当前进的节点currLevel正好等于当前员工的签字顺序号, st.currlevel = sl.signlevel
        /// </summary>
        private const String QUERT_UNSIGN_CONTRACT_STR = @"SELECT  hc.id id, hc.name name, hc.submitdate submitdate, hc.columndata1 columndata1
                                                               FROM `hdjcontract` hc, `signaturestatus` st, `signaturelevel` sl
                                                               WHERE ((hc.id = st.conid and st.totalresult != 1)
                                                                  and (sl.contempid = hc.contempid and sl.empid = @EmployeeId)
                                                                  and (st.currlevel = sl.signlevel));";
 
        /// <summary>
        /// 查询编号为employeeId的人是否有未处理的签字单子
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public static List<HDJContract> QueryUnsignContract(int employeeId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<HDJContract> contracts = new List<HDJContract>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();
                // SELECT  h.id id, h.name name, h.submitdate submitdate, h.columndata1 columndata1
                cmd.CommandText = QUERT_UNSIGN_CONTRACT_STR;
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    HDJContract contract = new HDJContract();
                    contract.Id = sqlRead["id"].ToString();
                    contract.Name = sqlRead["name"].ToString();
                    contract.SubmitDate = sqlRead["submitdate"].ToString();

                    List<String> columnDatas = new List<String>();
                    String columnData1 = sqlRead["columndata1"].ToString();
                    columnDatas.Add(columnData1);

                    contract.ColumnDatas = columnDatas;
                    //contract.CurrLevel = int.Parse(sqlRead["currlevel"].ToString());
                    //contract.MaxLevel = int.Parse(sqlRead["maxlevel"].ToString());

                    contracts.Add(contract);

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

/*
        SELECT c.signlevel8 maxlevel
        FROM `hdjcontract` h, `contemp` c 
        WHERE (h.contempid = c.id h.id = new.id);

INSERT INTO `signaturestatus` (`id`, `conid`, `result1`, `result2`, `result3`, `result4`, `result5`, `result6`, `result7`, `result8`, `totalresult`, `agreecount`, `refusecount`, `currlevel`, `maxlevel`) 
VALUES (NOW(), 1, '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', (SELECT c.signlevel8 FROM `hdjcontract` h, `contemp` c WHERE (h.contempid = c.id and h.id = new.id)));
*/