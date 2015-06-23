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
         * 其实本表可以与HDJcontract表在数据库中进行合并，但是为了清晰，我们还是单独做了张表
         * 其理由在于，签字人的列表是跟会签单所使用的模版有关的吗，会签单本身只有一个从T额，ontempID存储了会签单模版的信息
         * 会签单本身是不知道的，因此我们分开一张表来存储
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

        /// <summary>
        /// 查询员工employeeId所有的正在审核中的签单
        /// </summary>
        private const String QUERY_SIGNATURE_STATUS_PENDDING_EMP_STR = @"SELECT h.id id, h.name name, h.submitdate submitdate, h.columndata1 columndata1, s.currlevel currlevel, s.maxlevel maxlevel
                                                                    FROM `signaturestatus` s, `hdjcontract` h
                                                                    WHERE (s.totalresult = 0 and h.id = s.conid and h.subempid = @EmployeeId)
                                                                    ORDER BY id";
        /// <summary>
        /// 查询员工employeeId所有的已经同意的签单
        /// </summary>
        private const String QUERY_SIGNATURE_STATUS_AGREE_EMP_STR = @"SELECT h.id id, h.name name, h.submitdate submitdate, h.columndata1 columndata1, s.currlevel currlevel, s.maxlevel maxlevel
                                                                      FROM `signaturestatus` s, `hdjcontract` h
                                                                      WHERE (s.totalresult = 1 and h.id = s.conid and h.subempid = @EmployeeId)
                                                                      ORDER BY id";

        /// <summary>
        /// 查询员工employeeId所有的已经被拒绝的签单
        /// </summary>
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
         * 
         * 
         * 本表中有如下触发器
         * ①insert_signature_status AFTER INSERT on `hdjcontract` 
         * 申请人每次提交一张新表的时候，就插入一个数据域全为空的数据
         * 
         * ②trigger modify_signature_status AFTER INSERT on `signaturedetail`
         * 签字人每次处理一个签单请求的时候，就会置signaturestatus中对应位置的数据域为-1(拒绝)或者1(同意)
         * 
         * ③每次签字人拒绝一张签单的时候，就把整个signature表的totalresult数据置为-1，即一人拒绝则整单拒绝
         * ④当且仅当所有的签字人都同意了一张表的时候，才把整个signature表的totalresult数据置为1，
         * 
         * ⑤当一张表的totalresult被置为-1的时候，用户查询出自己的会签单被拒绝后，对会签单进行修改，
         * 因此修改hdjcontract同样会触发更新signaturestatus的所有数据域重新置为初始
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
        CREATE trigger insert_signature_status
        AFTER INSERT on `hdjcontract` 
        FOR EACH ROW 
        BEGIN
            INSERT INTO `signaturestatus` (`id`, `conid`, `result1`, `result2`, `result3`, `result4`, `result5`, `result6`, `result7`, `result8`, `totalresult`, `agreecount`, `refusecount`, `currlevel`, `maxlevel`, `updatecount`) 
            VALUES (NOW(), new.id, '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', (SELECT c.signlevel8 FROM `hdjcontract` h, `contemp` c WHERE (h.contempid = c.id and h.id = new.id)), '1');

        END;";


        private const String UPDATE_SIGNATURE_STATUS_TRIGGER = @"
        CREATE trigger update_signature_status
        AFTER UPDATE on `hdjcontract` 
        FOR EACH ROW 
        BEGIN

            UPDATE `signaturestatus`
            set `result1` = '0', `result2` = '0', `result3` = '0', `result4` = '0', `result5` = '0', `result6` = '0', `result7` = '0', `result8` = '0', `totalresult` = '0', `agreecount` = '0', `refusecount` = '0', `currlevel` = '1', `updatecount` = `updatecount` + 1
            WHERE (conid = new.id);

        END;";

        private const String SET_SIGNATRUE_TOTALRESULT = @"
        CREATE trigger set_signature_status_totalresult
        BEFORE UPDATE on `signaturestatus` 
        FOR EACH ROW 
BEGIN

         /*  所有的人都同意签字，整个会签单才能被同意  */
         if (new.result1 = '1' and new.result2 = '1' and new.result3 = '1' and new.result4 = '1' and new.result5 = '1' and new.result6 = '1' and new.result7 = '1' and new.result8 = '1')

            then set new.totalresult = '1';
        /*  整个会签单只要有一个人拒绝签字，那么整张表都会被拒绝  */
        elseif (new.result1 = '-1' or new.result2 = '-1' or new.result3 = '-1' or new.result4 = '-1' or new.result5 = '-1' or new.result6 = '-1' or new.result7 = '-1' or new.result8 = '-1')
            
            then set new.totalresult = '-1';

        end if;
/* 当前currlevel阶段下需要签字的人数 == 当前currlevel下已经同意签字的人数
   说明当前会签单当前阶段的流程已经走完，需要进入下一个流程  */
if
((SELECT count(sl.empid)
FROM (SELECT conid,currlevel, totalresult, updatecount  FROM signaturestatus) st2, signaturelevel  sl, hdjcontract hc, signaturedetail sd
WHERE (st2.conid = hc.id 
    and sl.contempid = hc.contempid 
   and sl.signlevel = st2.currlevel and st2.totalresult = 0
   and sd.conid = hc.id and sd.empid = sl.empid and sd.result = 1 and sd.updatecount = st2.updatecount
   and hc.id = new.id)) 
= 
(SELECT count(sl.empid)
FROM (SELECT conid,currlevel, totalresult  FROM signaturestatus) st2, signaturelevel  sl, hdjcontract hc
WHERE (st2.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st2.currlevel and st2.totalresult = 0
   and hc.id = new.id)))

    then  set new.currlevel = new.currlevel + 1;
end if;
END";
        /*
         * ERROR 1442 (HY000): Can’t update table ‘dept’ in stored function/trigger because it is already used by statement which invoked this stored function/trigger.
         * 
                CREATE trigger set_signature_status_totalresult


                CREATE trigger set_signature_status_totalresult
                BEFORE UPDATE on `signaturestatus` 
                FOR EACH ROW 
                BEGIN

                    set new.totalresult = '1'
                    WHERE (new.result1 = '1' and new.result2 = '1' and new.result3 = '1' and new.result4 = '1' and new.result5 = '1' and new.result6 = '1' and new.result7 = '1' and new.result8 = '1');


                    set new.totalresult = '-1'
                    WHERE (new.result1 = '-1' or new.result2 = '-1' or new.result3 = '-1' or new.result4 = '-1' or new.result5 = '-1' or new.result6 = '-1' or new.result7 = '-1' or new.result8 = '-1');
                END;
        */

        /*
        /*
        BEGIN
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
        BEFORE INSERT on `signaturedetail`
        FOR EACH ROW
        BEGIN

            [2015-6-22 19：08添加]
            set new.updatecount = (SELECT `updatecount` FROM `signaturestatus` WHERE (conid = new.conid)));
            
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
        public static List<SHDJContract> QuerySignatureStatusPendding(int employeeId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<SHDJContract> contracts = new List<SHDJContract>();

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
                    SHDJContract contract = new SHDJContract();
                    contract.Id = sqlRead["id"].ToString();
                    contract.Name = sqlRead["name"].ToString();
                    contract.SubmitDate = sqlRead["submitdate"].ToString();

                    contract.ProjectName = sqlRead["columndata1"].ToString();

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
        public static List<SHDJContract> QuerySignatureStatusAgree(int employeeId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<SHDJContract> contracts = new List<SHDJContract>();

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
                    SHDJContract contract = new SHDJContract();
                    contract.Id = sqlRead["id"].ToString();
                    contract.Name = sqlRead["name"].ToString();
                    contract.SubmitDate = sqlRead["submitdate"].ToString();

                    //List<String> columnDatas = new List<String>();
                    //String columnData1 = sqlRead["columndata1"].ToString();
                    contract.ProjectName = sqlRead["columndata1"].ToString();

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
        public static List<SHDJContract> QuerySignatureStatusRefuse(int employeeId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<SHDJContract> contracts = new List<SHDJContract>();

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
                    SHDJContract contract = new SHDJContract();
                    contract.Id = sqlRead["id"].ToString();
                    contract.Name = sqlRead["name"].ToString();
                    contract.SubmitDate = sqlRead["submitdate"].ToString();

                    contract.ProjectName = sqlRead["columndata1"].ToString();

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
        private const String QUERT_UNSIGN_CONTRACT_STR = @"SELECT  hc.id id, hc.name name, e.name subempname, hc.submitdate submitdate, hc.columndata1 columndata1
FROM `hdjcontract` hc, `signaturestatus` st, `signaturelevel` sl, `employee` e
WHERE ((hc.id = st.conid and st.totalresult = 0)
and (sl.contempid = hc.contempid and sl.empid = @EmployeeId)
and (st.currlevel >= sl.signlevel)
and (hc.subempid = e.id)
and (hc.id not in (
SELECT  sd.conid
FROM `signaturestatus` st, `signaturedetail` sd
WHERE (sd.conid = st.conid and sd.empid = @EmployeeId and sd.updatecount = st.updatecount))))";


/*
@"SELECT  hc.id id, hc.name name, hc.submitdate submitdate, hc.columndata1 columndata1
                                                               FROM `hdjcontract` hc, `signaturestatus` st, `signaturelevel` sl
                                                               WHERE ((hc.id = st.conid and st.totalresult != 1)
                                                                  and (sl.contempid = hc.contempid and sl.empid = @EmployeeId)
                                                                  and (st.currlevel = sl.signlevel));";
 */
        /// <summary>
        /// 查询编号为employeeId的人是否有未处理的签字单子
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public static List<SHDJContract> QueryUnsignContract(int employeeId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<SHDJContract> contracts = new List<SHDJContract>();

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
                    SHDJContract contract = new SHDJContract();
                    
                    contract.Id = sqlRead["id"].ToString();
                    contract.Name = sqlRead["name"].ToString();
                    contract.SubmitDate = sqlRead["submitdate"].ToString();
                    contract.SubmitEmployeeName = sqlRead["subempname"].ToString();
                    contract.ProjectName = sqlRead["columndata1"].ToString();


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


        #region 查询某个人是否已经处理的签字单子
        /// <summary>
        /// 查询某个人是否有已经处理过的签字单子
        /// 引入会签单模版签字顺序表signaturelevel表后，不再需要关联contemp表
        /// 当当前单子需要某个人签字的时候，需要满足几个条件
        /// 一是，这个会签单已经完成签字，即签字流程已经走完,signaturestatus中，SQL表示为hc.id = st.conid and st.totalresult = 1[当前会签单的在待办会签单状态表中]
        /// 二是，当前员工的ID在会签单模版中，即当前会签单需要此ID的员工签字,SQL语句表示为sl.contempid = hc.contempid and sl.empid = @EmployeeId 
        /// </summary>
        private const String QUERT_SIGNED_CONTRACT_STR = @"SELECT  hc.id id, hc.name name, hc.columndata1 projectname, e.name subempname, hc.submitdate submitdate, sd.date signdate, sd.result signresult, sd.remark signremark 
FROM `hdjcontract` hc, `signaturestatus` st, `signaturelevel` sl, `signaturedetail` sd, employee e
WHERE ((hc.id = st.conid)
and (sl.contempid = hc.contempid and sl.empid = @EmployeeId)
and (st.currlevel >= sl.signlevel)
and (hc.subempid = e.id)
and (sd.conid = hc.id and sd.empid = sl.empid and sd.updatecount = st.updatecount));";

        /// <summary>
        /// 查询编号为employeeId的人是否有未处理的签字单子
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public static List<SHDJContract> QuerySignedContract(int employeeId)
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<SHDJContract> contracts = new List<SHDJContract>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();
                // SELECT  h.id id, h.name name, h.submitdate submitdate, h.columndata1 columndata1
                cmd.CommandText = QUERT_SIGNED_CONTRACT_STR;
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    SHDJContract contract = new SHDJContract();
                    contract.Id = sqlRead["id"].ToString();
                    contract.Name = sqlRead["name"].ToString();
                    contract.ProjectName = sqlRead["projectname"].ToString();
                    contract.SubmitEmployeeName = sqlRead["subempname"].ToString();
                    contract.SubmitDate = sqlRead["submitdate"].ToString();
                    contract.SignDate = sqlRead["signdate"].ToString();
                    contract.SignRemark = sqlRead["signremark"].ToString();

                    if(int.Parse(sqlRead["signresult"].ToString()) == 1)
                    {
                        contract.SignResult = "同意"; 
                    }
                    else
                    {
                        contract.SignResult = "拒绝";
                    }
                    
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