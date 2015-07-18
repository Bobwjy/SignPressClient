using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using SignPressServer.SignData;


using MySql.Data.MySqlClient;
using MySql.Data;
using System.Data;
using SignPressServer.SignTools;

/*

[问题2]
仍然是查询某个人是否有未签字的单子
当当前单子需要某个人签字的时候，需要满足几个条件
一是，这个会签单仍然需要签字，即签字流程还没走完,signaturestatus中，SQL表示为(h.contempid = c.id and s.conid = h.id)
二是，当前员工的ID在会签单模版中，即当前会签单需要此ID的员工签字,SQL语句表示为(c.signid[1~8] = @employeeId)
三是，这个会签单的当前进的节点currLevel正好等于当前员工的签字顺序号,


这里就需要一个问题怎么查询出一个员工ID，对应某个会签单模版中的签字顺序
SELECT employeelevel 
FROM `contemp`
WHERE (c.signid)不可行
我们的解决方案是，再引入一张触发表，

signaturelevel表
存储了这样的信息，在会签单模版contempid中，第signnum个签字人是empid,他的签字顺序是signlevel

在新建/修改模版的时候，就插入一项数据
表中数据存储了,每个员工在每个会签单模版中的签字顺序号
员工empid, 会签单模版contempid，签字顺序signlevel[1~3], 签字人在表中的序号signnum[1~8]
表中的主键是contempid + signnum，不能是contempid + contempid
因为任何一个会签单模版中只有1~8个位置，
那么针对每个会签单模版，每个位置必定对应一个人，而且这个人可以修改
但是如果用contempid + contempid作为主键，那么修改模版中的签字人的时候就会出问题


// 当插入一张新会签单模版的时候
CREATE trigger insert_signature_level
AFTER INSERT on `contemp` 
FOR EACH ROW
BEGIN

	INSERT INTO `signaturelevel` (`contempid`, `signnum`, `empid`, `signlevel`, `canview`, `candownload`)
        VALUES (new.id, '1', new.signid1, new.signlevel1, new.canview1, new.candownload1);


	INSERT INTO `signaturelevel` (`contempid`, `signnum`, `empid`, `signlevel`, `canview`, `candownload`)
        VALUES (new.id, '2', new.signid2, new.signlevel2, new.canview2, new.candownload2);
	
	INSERT INTO `signaturelevel` (`contempid`, `signnum`, `empid`, `signlevel`, `canview`, `candownload`)
        VALUES (new.id, '3', new.signid3, new.signlevel3, new.canview3, new.candownload3);


	INSERT INTO `signaturelevel` (`contempid`, `signnum`, `empid`, `signlevel`, `canview`, `candownload`)
        VALUES (new.id, '4', new.signid4, new.signlevel4, new.canview4, new.candownload4);
  	
    INSERT INTO `signaturelevel` (`contempid`, `signnum`, `empid`, `signlevel`, `canview`, `candownload`)
        VALUES (new.id, '5', new.signid5, new.signlevel5, new.canview5, new.candownload5);

	INSERT INTO `signaturelevel` (`contempid`, `signnum`, `empid`, `signlevel`, `canview`, `candownload`)
        VALUES (new.id, '6', new.signid6, new.signlevel6, new.canview6, new.candownload6);
	
    INSERT INTO `signaturelevel` (`contempid`, `signnum`, `empid`, `signlevel`, `canview`, `candownload`)
        VALUES (new.id, '7', new.signid7, new.signlevel7, new.canview7, new.candownload7);
	
    INSERT INTO `signaturelevel` (`contempid`, `signnum`, `empid`, `signlevel`, `canview`, `candownload`)
        VALUES (new.id, '8', new.signid8, new.signlevel8, new.canview8, new.candownload8);
 
END;  

// 当修改一张新会签单模版的时候
CREATE trigger update_signature_level
AFTER UPDATE on `contemp` 
FOR EACH ROW 
BEGIN

	UPDATE `signaturelevel` 
	SET `empid` = new.signid1, `signlevel` = new.signlevel1, `canview` = new.canview1, `candownload` = new.candownload1
	WHERE(`contempid` = new.id and `signnum` = 1);

	UPDATE `signaturelevel` 
	SET `empid` = new.signid2, `signlevel` = new.signlevel2, `canview` = new.canview2, `candownload` = new.candownload2
	WHERE(`contempid` = new.id and `signnum` = 2);
	
	UPDATE `signaturelevel` 
	SET `empid` = new.signid3, `signlevel` = new.signlevel3, `canview` = new.canview3, `candownload` = new.candownload3
	WHERE(`contempid` = new.id and `signnum` = 3);
	
	UPDATE `signaturelevel` 
	SET `empid` = new.signid4, `signlevel` = new.signlevel4, `canview` = new.canview4, `candownload` = new.candownload4
	WHERE(`contempid` = new.id and `signnum` = 4);
	
	UPDATE `signaturelevel` 
	SET `empid` = new.signid5, `signlevel` = new.signlevel5, `canview` = new.canview5, `candownload` = new.candownload5
	WHERE(`contempid` = new.id and `signnum` = 5);
	
	UPDATE `signaturelevel` 
	SET `empid` = new.signid6, `signlevel` = new.signlevel6, `canview` = new.canview6, `candownload` = new.candownload6
	WHERE(`contempid` = new.id and `signnum` = 6);

	UPDATE `signaturelevel` 
	SET `empid` = new.signid7, `signlevel` = new.signlevel7, `canview` = new.canview7, `candownload` = new.candownload7
	WHERE(`contempid` = new.id and `signnum` = 7);
	
	UPDATE `signaturelevel` 
	SET `empid` = new.signid8, `signlevel` = new.signlevel8, `canview` = new.canview8, `candownload` = new.candownload8
	WHERE(`contempid` = new.id and `signnum` = 8);

END;  

*/
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
        private const String INSERT_CONTRACT_TEMPLATE_STR = @"INSERT INTO `contemp` (`name`,  `createdate`,
                                                                                     `column1`, `column2`, `column3`, `column4`, `column5`, 
                                                                                     `signinfo1`, `signinfo2`, `signinfo3`, `signinfo4`, `signinfo5`, `signinfo6`, `signinfo7`, `signinfo8`, 
                                                                                     `signid1`, `signid2`, `signid3`, `signid4`, `signid5`, `signid6`, `signid7`, `signid8`,
                                                                                     `signlevel1`, `signlevel2`, `signlevel3`, `signlevel4`, `signlevel5`, `signlevel6`, `signlevel7`, `signlevel8`,
                                                                                     `canview1`, `canview2`, `canview3`, `canview4`, `canview5`, `canview6`, `canview7`, `canview8`,
                                                                                     `candownload1`, `candownload2`, `candownload3`, `candownload4`, `candownload5`, `candownload6`, `candownload7`, `candownload8`)
                                                              VALUES (@Name, @CreateDate,
                                                                      @Column_1, @Column_2, @Column_3, @Column_4, @Column_5, 
                                                                      @SignInfo_1, @SignInfo_2, @SignInfo_3, @SignInfo_4, @SignInfo_5, @SignInfo_6, @SignInfo_7, @SignInfo_8, 
                                                                      @SignId_1, @SignId_2, @SignId_3, @SignId_4, @SignId_5, @SignId_6, @SignId_7, @SignId_8,
                                                                      @SignLevel_1, @SignLevel_2, @SignLevel_3, @SignLevel_4, @SignLevel_5, @SignLevel_6, @SignLevel_7, @SignLevel_8,
                                                                      @CanView_1, @CanView_2, @CanView_3, @CanView_4, @CanView_5, @CanView_6, @CanView_7, @CanView_8,
                                                                      @CanDownload_1, @CanDownload_2, @CanDownload_3, @CanDownload_4, @CanDownload_5, @CanDownload_6, @CanDownload_7, @CanDownload_8)"; 

        /// <summary>
        /// 删除会签单模版信息串
        /// </summary>
        private const String DELETE_CONTRACT_TEMPLATE_ID_STR = @"DELETE FROM `contemp` WHERE (`id` = @Id)";

        private const String DELETE_CONTRACT_TEMPLATE_NAME_STR = @"DELETE FROM `contemp` WHERE (`name` = @Name)";


        /// <summary>
        /// 修改会签单模版信息串
        /// </summary>
        private const String MODIFY_CONTRACT_TEMPLATE_STR = @"UPDATE `contemp` 
                                                        SET `name` = @Name, `column1` = @Column_1, `column2` = @Column_2, `column3` = @Column_3, `column4` = @Column_4, `column5` = @Column_5, 
                                                             `signinfo1` = @SignInfo_1, `signinfo2` = @SignInfo_2, `signinfo3` = @SignInfo_3, `signinfo4` = @SignInfo_4, `signinfo5` = @SignInfo_5, `signinfo6` = @SignInfo_6, `signinfo7` = @SignInfo_7, `signinfo8` = @SignInfo_8, 
                                                             `signid1` = @SignId_1, `signid2` = @SignId_2, `signid3` = @SignId_3, `signid4` = @SignId_4, `signid5` = @SignId_5, `signid6` = @SignId_6, `signid7` = @SignId_7, `signid8` = @SignId_8,
                                                             `signlevel1` = @SignLevel_1, `signlevel2` = @SignLevel_2, `signlevel3` = @SignLevel_3, `signlevel4` = @SignLevel_4, `signlevel5` = @SignLevel_5, `signlevel6` = @SignLevel_6, `signlevel7` = @SignLevel_7, `signlevel8` = @SignLevel_8,
                                                             `canview1` = @CanView_1, `canview2` = @CanView_2, `canview3` = @CanView_3, `canview4` = @CanView_4, `canview5` = @CanView_5, `canview6` = @CanView_6, `canview7` = @CanView_7, `canview8` = @CanView_8,
                                                             `candownload1` = @CanDownload_1, `candownload2` = @CanDownload_2, `candownload3` = @CanDownload_3, `candownload4` = @CanDownload_4, `candownload5` = @CanDownload_5, `candownload6` = @CanDownload_6, `candownload7` = @CanDownload_7, `candownload8` = @CanDownload_8
                                                        WHERE (`id` = @Id)";


        /// <summary>
        /// 获取会签单模版信息串
        /// </summary>
        private const String GET_CONTRACT_TEMPLATE_ID_STR = @"SELECT c.id id, c.name name, c.createdate createdate,
                                                                  c.column1 column1, c.column2 column2, c.column3 column3, c.column4 column4, c.column5 column5, 
                                                                  c.signinfo1 signinfo1, c.signinfo2 signinfo2, c.signinfo3 signinfo3, c.signinfo4 signinfo4, c.signinfo5 signinfo5, c.signinfo6 signinfo6, c.signinfo7 signinfo7, c.signinfo8 signinfo8, 
                                                                  e1.id signid1, e2.id signid2, e3.id signid3, e4.id signid4, e5.id signid5, e6.id signid6, e7.id signid7, e8.id signid8,
                                                                  e1.name signname1, e2.name signname2, e3.name signname3, e4.name signname4, e5.name signname5, e6.name signname6, e7.name signname7, e8.name signname8,          
                                                                  d1.id departmentid1, d2.id departmentid2, d3.id departmentid3, d4.id departmentid4, d5.id departmentid5, d6.id departmentid6, d7.id departmentid7, d8.id departmentid8,
                                                                  d1.name departmentname1, d2.name departmentname2, d3.name departmentname3, d4.name departmentname4, d5.name departmentname5, d6.name departmentname6, d7.name departmentname7, d8.name departmentname8,
                                                                  c.signlevel1 signlevel1, c.signlevel2, c.signlevel2, c.signlevel3, signlevel3, c.signlevel4 signlevel4, c.signlevel5 signlevel5, c.signlevel6 signlevel6, c.signlevel7 signlevel7, c.signlevel8 signlevel8,
                                                                  c.canview1 canview1, c.canview2 canview2, c.canview3 canview3, c.canview4 canview4, c.canview5 canview5, c.canview6 canview6, c.canview7 canview7, c.canview8 canview8,
                                                                  c.candownload1 candownload1, c.candownload2 candownload2, c.candownload3 candownload3, c.candownload4 candownload4, c.candownload5 candownload5, c.candownload6 candownload6, c.candownload7 candownload7, c.candownload8 candownload8
   
                                                              FROM contemp c, 
                                                                   employee e1, employee e2, employee e3, employee e4, employee e5, employee e6, employee e7, employee e8,
                                                                   department d1, department d2, department d3, department d4, department d5, department d6, department d7, department d8 
                                                              WHERE (c.id = @Id 
                                                                 and c.signid1 = e1.id  and c.signid2 = e2.id and c.signid3 = e3.id and c.signid4 = e4.id and c.signid5 = e5.id and c.signid6 = e6.id and c.signid7 = e7.id and c.signid8 = e8.id
                                                                 and d1.id = e1.departmentid and d2.id = e2.departmentid and d3.id = e3.departmentid and d4.id = e4.departmentid and d5.id = e5.departmentid and d6.id = e6.departmentid and d7.id = e7.departmentid and d8.id = e8.departmentid)";


        private const String GET_CONTRACT_TEMPLATE_NAME_STR = @"SELECT `name`, 
                                                                  `column1`, `column2`, `column3`, `column4`, `column5`, 
                                                                  `signinfo1`, `signinfo2`, `signinfo3`, `signinfo4`, `signinfo5`, `signinfo6`, `signinfo7`, `signinfo8`, 
                                                                  `signid1`, `signid2`, `signid3`, `signid4`, `signid5`, `signid6`, `signid7`, `signid8`
                                                                  `signlevel1`, `signlevel2`, `signlevel3`, `signlevel4`, `signlevel5`, `signlevel6`, `signlevel7`, `signlevel8`,
                                                                  `canview1`, `canview2`, `canview3`, `canview4`, `canview5`, `canview6`, `canview7`, `canview8`,
                                                                  `candownload1`, `candownload2`, `candownload3`, `candownload4`, `candownload5`, `candownload6`, `candownload7`, `candownload8`)
                                                              FROM `contemp`
                                                              WHERE (`name` = @Name)"; 



        /// <summary>
        /// 查询会签单模版的信息串
        /// </summary>
        private const String QUERY_CONTRACT_TEMPLATE_STR = @"SELECT id, name, createdate FROM `contemp` ORDER BY id"; 
        #endregion


        #region 插入会签单模版信息
        /// <summary>
        /// 插入员工信息
        /// </summary>
        /// <param name="conTemp">待插入的会签单模版</param>
        /// <returns></returns>
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
                cmd.Parameters.AddWithValue("@CreateDate", System.DateTime.Now);
//                cmd.Parameters.AddWithValue("@CreateDate", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                
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
                    String strSignLevel = @"SignLevel_" + (cnt + 1).ToString();
                    String strCanView = "@CanView_" + (cnt + 1).ToString();
                    String strCanDownload = "@CanDownload_" + (cnt + 1).ToString();
    
                    cmd.Parameters.AddWithValue(strSignInfo, conTemp.SignDatas[cnt].SignInfo);
                    cmd.Parameters.AddWithValue(strSignId, conTemp.SignDatas[cnt].SignEmployee.Id);
                    cmd.Parameters.AddWithValue(strSignLevel, conTemp.SignDatas[cnt].SignLevel);
                    cmd.Parameters.AddWithValue(strCanView, conTemp.SignDatas[cnt].CanView);
                    cmd.Parameters.AddWithValue(strCanDownload, conTemp.SignDatas[cnt].CanDownload);
                    
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
                cmd.Parameters.AddWithValue("@name", conTempName);                        // 会签单模版姓名


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
                cmd.CommandText = MODIFY_CONTRACT_TEMPLATE_STR;
                cmd.Parameters.AddWithValue("@Id", conTemp.TempId);
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
                    String strSignLevel = @"SignLevel_" + (cnt + 1).ToString();
                    String strCanView = "@CanView_" + (cnt + 1).ToString();
                    String strCanDownload = "@CanDownload_" + (cnt + 1).ToString();

                    cmd.Parameters.AddWithValue(strSignInfo, conTemp.SignDatas[cnt].SignInfo);
                    cmd.Parameters.AddWithValue(strSignId, conTemp.SignDatas[cnt].SignEmployee.Id);
                    cmd.Parameters.AddWithValue(strSignLevel, conTemp.SignDatas[cnt].SignLevel);
                    cmd.Parameters.AddWithValue(strCanView, conTemp.SignDatas[cnt].CanView);
                    cmd.Parameters.AddWithValue(strCanDownload, conTemp.SignDatas[cnt].CanDownload);
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


        #region 查询会签单模版的信息
        public static List<ContractTemplate> QueryContractTemplate()
        {
            MySqlConnection con = DBTools.GetMySqlConnection();
            MySqlCommand cmd;

            List<ContractTemplate> conTemps = new List<ContractTemplate>();

            try
            {
                con.Open();

                cmd = con.CreateCommand();

                cmd.CommandText = QUERY_CONTRACT_TEMPLATE_STR;


                MySqlDataReader sqlRead = cmd.ExecuteReader();
                cmd.Dispose();

                while (sqlRead.Read())
                {
                    ContractTemplate conTemp = new ContractTemplate();


                    conTemp.TempId = int.Parse(sqlRead["id"].ToString());
                    conTemp.Name = sqlRead["name"].ToString();
                    conTemp.CreateDate = sqlRead["createdate"].ToString();
                   // // 5个栏目信息
                   // // conTemp.ColumnCount = 5;
                   // List<String> columns = new List<String>();
                   // /*
                   // columns.Add(sqlRead["column1"].ToString());
                   // columns.Add(sqlRead["column2"].ToString());
                   // columns.Add(sqlRead["column3"].ToString());
                   // columns.Add(sqlRead["column4"].ToString());
                   // columns.Add(sqlRead["column5"].ToString());
                   //*/
                   // for (int cnt = 1; cnt <= 5; cnt++)
                   // {
                   //     String strColumn = "column" + cnt.ToString();
                   //     columns.Add(sqlRead[strColumn].ToString());
                   // }
                   // conTemp.ColumnNames = columns;

                   // // 8个签字人信息
                   // // conTemp.SignCount = 8;
                   // List<SignatureTemplate> signatures = new List<SignatureTemplate>();
                   // for (int cnt = 1; cnt <= 8; cnt++)
                   // {
                   //     String strSignInfo = "signinfo" + cnt.ToString();
                   //     String strSignId = "signId" + cnt.ToString();

                   //     SignatureTemplate signDatas = new SignatureTemplate();
                   //     signDatas.SignInfo = sqlRead[strSignInfo].ToString();

                   //     signDatas.SignId = int.Parse(sqlRead[strSignId].ToString());

                   //     signatures.Add(signDatas);
                   // }
                   // conTemp.SignDatas = signatures;

                    conTemps.Add(conTemp);
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
            return conTemps;
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
                    conTemp.CreateDate = sqlRead["createdate"].ToString();
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
                        String strSignId = "signid" + cnt.ToString();
                        String strSignName = "signname" + cnt.ToString();
                        String strDepartmentId = "departmentid" + cnt.ToString();
                        String strDepartmentName = "departmentname" + cnt.ToString();
                        String strSignLevel = "signlevel" + cnt.ToString();
                        String strCanView = "canview" + cnt.ToString();
                        String strCanDownload = "candownload" + cnt.ToString();

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

                        signDatas.CanView = int.Parse(sqlRead[strCanView].ToString());
                        signDatas.CanDownload = int.Parse(sqlRead[strCanDownload].ToString());

                        signatures.Add(signDatas);
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
                        String strSignId = "signid" + cnt.ToString();
                        String strSignName = "signname" + cnt.ToString();
                        String strDepartmentId = "department" + cnt.ToString();
                        String strDepartmentName = "department" + cnt.ToString();
                        String strCanView = "canview" + cnt.ToString();
                        String strCanDownload = "candownload" + cnt.ToString();


                        SignatureTemplate signDatas = new SignatureTemplate();
                        signDatas.SignInfo = sqlRead[strSignInfo].ToString();
                        
                        Employee employee = new Employee();
                        employee.Id = int.Parse(sqlRead[strSignId].ToString());
                        employee.Name = sqlRead[strSignName].ToString();
                        Department department = new Department();
                        department.Id = int.Parse(sqlRead[strDepartmentId].ToString());
                        department.Name = sqlRead[strDepartmentName].ToString();
                        employee.Department = department;
                        signDatas.SignEmployee = employee;

                        signDatas.CanView = int.Parse(sqlRead[strCanView].ToString());
                        signDatas.CanDownload = int.Parse(sqlRead[strCanDownload].ToString());

                        signatures.Add(signDatas);
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

        #region 查询当前员工关联的会签单模版的数目
		private  const String GET_EMPLOYEE_CONTRACT_TEMPLATE_COUNT = @"SELECT Count(contempid) count FROM `signaturelevel` WHERE (empid = @EmployeeId)";

        public static int GetEmployeeContractTemplateCount(int employeeId)
        {
            {
                MySqlConnection con = DBTools.GetMySqlConnection();
                MySqlCommand cmd;

                int count = 0;

                try
                {
                    con.Open();

                    cmd = con.CreateCommand();

                    cmd.CommandText = GET_EMPLOYEE_CONTRACT_TEMPLATE_COUNT;
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                    MySqlDataReader sqlRead = cmd.ExecuteReader();
                    cmd.Dispose();

                    while (sqlRead.Read())
                    {

                        count = int.Parse(sqlRead["count"].ToString());
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
                return count;
            }
        }

        #endregion
        
        #region 获取当前会签单模版相关联的会签单信息
        private const String GET_CONTRACT_TEMPLATE_HCONTRACT_COUNT = @"SELECT Count(id) count FROM `hdjcontract` WHERE (contempid = @ConTempId)";

        public static int GetContractTemplateHDJContractCount(int contempId)
        {
            {
                MySqlConnection con = DBTools.GetMySqlConnection();
                MySqlCommand cmd;

                int count = 0;

                try
                {
                    con.Open();

                    cmd = con.CreateCommand();

                    cmd.CommandText = GET_CONTRACT_TEMPLATE_HCONTRACT_COUNT;
                    cmd.Parameters.AddWithValue("@ConTempId", contempId);

                    MySqlDataReader sqlRead = cmd.ExecuteReader();
                    cmd.Dispose();

                    while (sqlRead.Read())
                    {

                        count = int.Parse(sqlRead["count"].ToString());
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
                return count;
            }
        }

        #endregion
        
        

        #region 查询某个员工对于会签单是否有查看状态的权限
        private const string IS_EMPLOYEE_CAN_VIEW_CONTRACT = @"SELECT sl.canview canview FROM `signaturelevel` sl, `hdjcontract` hc WHERE (sl.empid = @EmployeeId and hc.contempid = sl.contempid)";
        public bool  IsEmployeeCanViewContract(int employeeId, string contractId)
        {
            return false;
        }
        #endregion

        #region 查询某个员工对于会签单是否有
        public bool  IsEmployeeCanDownloadContract(int employeeId, string contractId)
        {
            return false;
        }


        #endregion
    }
}
