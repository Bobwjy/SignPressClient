# SignPressServer
一个远程签单工具的服务器程序


#实现工程名称的数据绑定
-------------------------------------------------------------------------------
2015年11月6日 20:41:25 更新
##之前在做的[接口已经实现--客户端已实现，但是BUG]
在之前的Department派生出新类SDepartment
附带了部门的申请权限，包括界, 内,应,例
QUERY_SDEPARTMENT_REQUEST,
MODIFY_SDEPARTMENT_REQUEST,


##工程名称[接口已经实现--客户端未实现]
ContractCategory类存储了工程名称的类别信息
对于数据库中Category表
DALContractIdCategory操作数据库接口
请求
QUERY_SDEPARTMENT_CONTRACTCATEGORY_REQUEST

QUERY_SDEPARTMENT_CONTRACTCATEGORY_SUCCESS,
QUERY_SDEPARTMENT_CONTRACTCATEGORY_FAILED,

查询工程名称信息的时候, 直接向服务器发送departmentid [int]


##项目名称[接口已经实现--客户端未实现]
ContractProject类存储了项目名称的类别信息
对于数据库中Project表
DALContractProject操作数据库接口

查询工程列表请直接发送categoryid [int]
QUERY_CATEGORY_PROJECT_REQUEST,

QUERY_CATEGORY_PROJECT_SUCCESS,
QUERY_CATEGORY_PROJECT_FAILED,

   
##可能需要额外的接口
前面的接口都是单个的，可能需要提供一个直接从部门就获取到包括工程集合和项目集合的接口














#-------------------------------------------------------------------------------
# 签字信息表的数据

一种方案
每个表设计一张签字表结构
签字状态表id
签署的会签单编号
string[8] state 8个签字人的状态[未处理，通过，拒绝]
同意人数计数器
拒绝人数计数器

同时在设置一张签字明细表
签字明细表编号
明细表编号id
签署的会签单编号
签字信息[同意还是拒绝]
签字备注[同意的附加信息以及拒绝的备注信息]



当提交一个明细表后，就向签字表中插入数据，签字状态均为[未处理]
然后依照会签单中的签字顺序，开始一次签字，

会签单中的signId[1~8]存储了签字人的信息
这样使用正则匹配即可计算出某个人有没有会签单需要签字
当用户签字后，就向字状态表中插入签字状态，[通过/拒绝]
同时在签字明细表中，插入签字信息[同意拒绝以及备注信息]

只要有一个人拒绝，那么单子就打回重写，但是编号，不变，同时删除单子状态表中的数据


#[问题1]怎么查询每个人是否有未签字的单子信息
-------
一种实现方案，就是上面的[签字状态表id + 签字明细表]
首先，签字状态表是对外是一个只读表，其数据的修改，由数据库触发器进行维护
用户提交签单时（即用户在数据库插入或者修改签单之后），通过触发器在数据库signaturestatus表中插入一项数据，数据项全为未处理
用户每次签字，通过触发器在在签字明细表signaturestatus中，置对应数据项为[同意或者拒绝]

每次查询自己是否有签字信息的时候的时候，查询一下签字状态表中，是否未处理的会签单信息
[通过会签单信息，查询出会签单的模版信息，通过模版信息查询出每个签字人的信息]
使用一个方法处理，直接通过会签单模版获取出每个签字人的信息id即可
继而通过



[问题2]怎么查询出一个员工ID，对应某个会签单模版中的签字顺序
-------
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

	INSERT INTO `signaturelevel` (`contempid`, `signnum`, `empid`, `signlevel`)
        VALUES (new.id, '1', new.signid1, new.signlevel1);

	INSERT INTO `signaturelevel` (`contempid`, `signnum`, `empid`, `signlevel`)
        VALUES (new.id, '2', new.signid2, new.signlevel2);
	
	INSERT INTO `signaturelevel` (`contempid`, `signnum`, `empid`, `signlevel`)
        VALUES (new.id, '3', new.signid3, new.signlevel3);
	
	INSERT INTO `signaturelevel` (`contempid`, `signnum`, `empid`, `signlevel`)
        VALUES (new.id, '4', new.signid4, new.signlevel4);
	
	INSERT INTO `signaturelevel` (`contempid`, `signnum`, `empid`, `signlevel`)
        VALUES (new.id, '5', new.signid5, new.signlevel5);

	INSERT INTO `signaturelevel` (`contempid`, `signnum`, `empid`, `signlevel`)
        VALUES (new.id, '6', new.signid6, new.signlevel6);

	INSERT INTO `signaturelevel` (`contempid`, `signnum`, `empid`, `signlevel`)
        VALUES (new.id, '7', new.signid7, new.signlevel7);
	
	INSERT INTO `signaturelevel` (`contempid`, `signnum`, `empid`, `signlevel`)
        VALUES (new.id, '8', new.signid8, new.signlevel8);
END;  

// 当修改一张新会签单模版的时候
CREATE trigger update_signature_level
AFTER UPDATE on `contemp` 
FOR EACH ROW 
BEGIN

	UPDATE `signaturelevel` 
	SET `empid` = new.signid1, `signlevel` = new.signlevel1
	WHERE(`contempid` = new.id and `signnum` = 1);

	UPDATE `signaturelevel` 
	SET `empid` = new.signid2, `signlevel` = new.signlevel2
	WHERE(`contempid` = new.id and `signnum` = 2);
	
	UPDATE `signaturelevel` 
	SET `empid` = new.signid3, `signlevel` = new.signlevel3
	WHERE(`contempid` = new.id and `signnum` = 3);
	
	UPDATE `signaturelevel` 
	SET `empid` = new.signid4, `signlevel` = new.signlevel4
	WHERE(`contempid` = new.id and `signnum` = 4);
	
	UPDATE `signaturelevel` 
	SET `empid` = new.signid5, `signlevel` = new.signlevel5
	WHERE(`contempid` = new.id and `signnum` = 5);
	
	UPDATE `signaturelevel` 
	SET `empid` = new.signid6, `signlevel` = new.signlevel6
	WHERE(`contempid` = new.id and `signnum` = 6);

	UPDATE `signaturelevel` 
	SET `empid` = new.signid7, `signlevel` = new.signlevel7
	WHERE(`contempid` = new.id and `signnum` = 7);
	
	UPDATE `signaturelevel` 
	SET `empid` = new.signid8, `signlevel` = new.signlevel8
	WHERE(`contempid` = new.id and `signnum` = 8);

END;  


private const String QUERT_UNSIGN_CONTRACT_STR = @"SELECT  h.id id, h.name name, h.submitdate submitdate, h.columndata1 columndata1
                                                   FROM `hdjcontract` h, `contemp` c, `signaturestatus` s
                                                   WHERE (h.contempid = c.id and s.conid = h.id
                                                      and (c.signid1 = @employeeId or c.signid2 = @employeeId or c.signid3 = @EmployeeId or c.signid4 = @EmployeeId 
                                                        or c.signid5 = @employeeId or c.signid6 = @employeeId or c.signid7 = @EmployeeId or c.signid8 = @EmployeeId))";

SELECT  h.id id, h.name name, h.submitdate submitdate, h.columndata1 columndata1
                                                           FROM `hdjcontract` h, `contemp` c, `signaturestatus` s
                                                           WHERE (h.contempid = c.id and s.conid = h.id
                                                              and (c.signid1 = 3 or c.signid2 = 2 or c.signid3 = 3 or c.signid4 = 3 
                                                                or c.signid5 = 3 or c.signid6 = 3 or c.signid7 = 3 or c.signid8 = 3));



[问题3]签字流程怎么走？
-------
当第一阶段的签字人走完之后，怎么将添加状态标准中的currLevel + 1
其本质就是怎么判断前一个流程的签字人已经走完

// 查询出当前会签单conid，当前签字阶段的所有签字人
SELECT hc.id "会签单编号", sl.empid "签字人编号", st.currlevel "当前签字阶段"
FROM signaturestatus st, signaturelevel  sl, hdjcontract hc
WHERE (st.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st.currlevel and st.totalresult = 0
   and hc.id = 20150621124713);


// 查询出当前会签单conid当前阶段的需要签字人数
SELECT hc.id "会签单编号", st.currlevel "当前签字阶段", count(sl.empid) 
FROM signaturestatus st, signaturelevel  sl, hdjcontract hc
WHERE (st.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st.currlevel and st.totalresult = 0
   and hc.id = 20150621124713);
//GROUB BY sl.empid;

// 查询出当前会签单conid，当前签字阶段的所有签字人的签字信息
SELECT hc.id "会签单编号", sl.empid "签字人编号", st.currlevel "当前签字阶段", sd.result "签字结果"
FROM signaturestatus st, signaturelevel  sl, hdjcontract hc, signaturedetail sd
WHERE (st.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st.currlevel and st.totalresult = 0
   and sd.conid = hc.id and sd.empid = sl.empid
   and hc.id = 20150623145544);

//  查询出当前会签单conid，当前签字阶段的所有已经签字的签字人的人数信息
SELECT count(sl.empid) "已经签字的人数", count(sd.result) "签字的结果条数"
FROM signaturestatus st, signaturelevel  sl, hdjcontract hc, signaturedetail sd
WHERE (st.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st.currlevel and st.totalresult = 0
   and sd.conid = hc.id and sd.empid = sl.empid
   and hc.id = 20150623145544)


//  查询出当前会签单conid，当前签字阶段的所有已经同意的签字人的人数信息
SELECT count(sl.empid) "同意签字的人数", count(sd.result) "签字的结果条数"
FROM signaturestatus st, signaturelevel  sl, hdjcontract hc, signaturedetail sd
WHERE (st.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st.currlevel and st.totalresult = 0
   and sd.conid = hc.id and sd.empid = sl.empid and sd.result = 1
   and hc.id = 20150623145544)


SELECT count(sl.empid) "拒绝签字的人数", count(sd.result) "签字的结果条数"
FROM signaturestatus st, signaturelevel  sl, hdjcontract hc, signaturedetail sd
WHERE (st.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st.currlevel and st.totalresult = 0
   and sd.conid = hc.id and sd.empid = sl.empid and sd.result = -1 
   and hc.id = 20150623145544);



[问题4]第一个阶段走完后，怎么通知下面的流程
-------
但是仍然出现问题，即当一张单子被拒绝之后，重新提交，所有人都会重新开始签字，
这样数据库里面签字明细表signaturedetail签字的信息就会很多很乱、
这样查询出的计数器仍然是有问题的，因此我们引入一个计数器用于计数一张单子的重签(更新)计数器updatecount
这个数据由hdjcontract通过更新触发器维护，每次更新计数器就 + 1，signaturestatus维护，同时引入signaturedetail中
这样signaturedetail的主键就成为，对于会签单conid，签字人empid的第updatecount次签字的信息

这样我们查询前面的计数器的时候，只需要将signaturestatus.updatecount = signaturedetial.updatecount
即可对号入座
但是仍然需要注意，update其实是hdjcontract通过更新触发器维护，标识在signaturestatus表中的
在signaturestatus中只作为引入数据，不能自行修改，因此在插入签字明细的时候
应该通过
SELECT `updatecount` 
FROM `signaturestatus`
WHERE (conid = 20150621124713);
即DALSignatureDetail中INSERT串的信息应该为
private const String INSERT_SIGNATURE_DETAIL_STR = @"INSERT INTO `signaturedetail` (`id`, `date`, `empid`, `conid`, `result`, `remark`, `updatecount`) 
                                                             VALUES (@Id, @Date, @EmpId, @ConId, @Result, @Remark, (SELECT `updatecount` FROM `signaturestatus` WHERE (conid = @ConId)))";

因此我们写下测试串，出错
INSERT INTO `signaturedetail` (`date`, `empid`, `conid`, `result`, `remark`, `updatecount`) 
VALUES (NOW(), 1, 20150621124713, 1, "成坚同意了您的申请", (SELECT `updatecount` FROM `signaturestatus` WHERE (conid = 20150621124713)));

错误信息

1442 - Can't update table 'signaturestatus' in stored function/trigger because it is already used by statement which invoked this stored function/trigger.

其错误极其类似与CREATE trigger set_signature_status_totalresult的出错信息
在一个表table的触发器中insert/update/delete这个表table
因此我们将这个写成一个触发器
CREATE trigger set_signature_detail_updatecount
AFTER INSERT on `signaturedetail` 
FOR EACH ROW 
BEGIN
    set new.updatecount = (SELECT `updatecount` FROM `signaturestatus` WHERE (conid = new.conid)));
END;

因此前面出现的签字信息，
   and sd.conid = hc.id and sd.empid = sl.empid and sd.result = -1 
中再添加一个
   and sd.conid = hc.id and sd.empid = sl.empid and sd.result = -1 and sd.updatecount = st.updatecount



##查询出当前会签单conid，当前签字阶段的所有已经签字的签字人的人数信息
-------
SELECT count(sl.empid) "已经签字的人数", count(sd.result) "签字的结果条数"
FROM signaturestatus st, signaturelevel  sl, hdjcontract hc, signaturedetail sd
WHERE (st.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st.currlevel and st.totalresult = 0
   and sd.conid = hc.id and sd.empid = sl.empid  and sd.updatecount = st.updatecount
   and hc.id = 20150621124713);


##查询出当前会签单conid，当前签字阶段的所有已经同意的签字人的人数信息
-------
// 查询出表20150621124713中第1个签字人的签字信息
// 需要签字明细表signaturedetail  主键 sd.empid + sd.conid + sd.updatecount
// 会签单表 hdjcontract  主键 hc.id
// 签字对应表signaturelevel sl.contempid + sl.signnum
// 会签单状态表 signaturestatus st.conid + st.updatecount 

SELECT count(sl.empid) "同意签字的人数", count(sd.result) "签字的结果条数"
FROM signaturestatus st, signaturelevel  sl, hdjcontract hc, signaturedetail sd
WHERE (st.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st.currlevel and st.totalresult = 0
   and sd.conid = hc.id and sd.empid = sl.empid and sd.result = 1 and sd.updatecount = st.updatecount
   and hc.id = 20150623153358);

##查询出当前会签单conid，当前签字阶段的所有拒绝的签字人的人数信息
-------
SELECT count(sl.empid) "拒绝签字的人数", count(sd.result) "签字的结果条数"
FROM signaturestatus st, signaturelevel  sl, hdjcontract hc, signaturedetail sd
WHERE (st.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st.currlevel and st.totalresult = 0
   and sd.conid = hc.id and sd.empid = sl.empid and sd.result = -1 and sd.updatecount = st.updatecount
   and hc.id = 20150623153358);



那么下面关键的问题来了，怎么判断一个会签单的当前流程已经走完了呢
就是当前currlevel阶段下需要签字的人数 == 当前currlevel下已经签字的人数
SELECT hc.id "会签单编号", st.currlevel "当前签字阶段", count(sl.empid) "已经签字的人数"
FROM signaturestatus st, signaturelevel  sl, hdjcontract hc, signaturedetail sd
WHERE (st.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st.currlevel and st.totalresult = 0
   and sd.conid = hc.id and sd.empid = sl.empid and sd.result = 1 and sd.updatecount = st.updatecount
   and hc.id = 20150623153358);


SELECT hc.id "会签单编号", st.currlevel "当前签字阶段", count(sl.empid) "需要签字的人数"
FROM signaturestatus st, signaturelevel  sl, hdjcontract hc
WHERE (st.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st.currlevel and st.totalresult = 0
   and hc.id = 20150623153358);

 即 每当签字明细表中有人签字的时候
就应该执行如下的触发器，将签字状态表中的，当前currlevel + 1
该触发器如果设置在signaturestatus表中则


但是出现异常，触发器没有调用，因此我们进行如下测试
UPDATE signaturestatus 
SET currlevel = currlevel+ 1
WHERE (((SELECT count(sl.empid)
FROM signaturestatus st, signaturelevel  sl, hdjcontract hc, signaturedetail sd
WHERE (st.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st.currlevel and st.totalresult = 0
   and sd.conid = hc.id and sd.empid = sl.empid and sd.result = 1 and sd.updatecount = st.updatecount
   and hc.id = 20150621124713)) 
= 
(SELECT count(sl.empid)
FROM signaturestatus st, signaturelevel  sl, hdjcontract hc
WHERE (st.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st.currlevel and st.totalresult = 0
   and hc.id = 20150621124713))) and conid = 20150621124713);
then 
    set new.currlevel = new.currlevel + 1;
出现错误[Err] 1093 - You can't specify target table 's' for update in FROM clause
原因在于执行SQL语句时出现这个错误。原因是在更新这个表和数据时又查询了它，而查询的数据又做了更新的条件
解决方法：
1，把要更新的几列数据查询出来做为一个第三方表，然后筛选更新。
2，新建一个临时表保存查询出的数据，然后筛选更新。最后删除临时表。
我们采用第1种方案
```
[[[(SELECT conid,currlevel, totalresult, updatecount  FROM signaturestatus) st2]]]
```
因此下面的sql语句
UPDATE signaturestatus 
SET currlevel = currlevel+ 1
WHERE (
(
(SELECT count(sl.empid) 
FROM (SELECT conid,currlevel, totalresult, updatecount  FROM signaturestatus) st2, signaturelevel  sl, hdjcontract hc, signaturedetail sd
WHERE (st2.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st2.currlevel and st2.totalresult = 0
   and sd.conid = hc.id and sd.empid = sl.empid and sd.result = 1 and sd.updatecount = st2.updatecount
   and hc.id = 20150623153358)) 
= 
(SELECT count(sl.empid)
FROM (SELECT conid,currlevel, totalresult  FROM signaturestatus) st2, signaturelevel  sl, hdjcontract hc
WHERE (st2.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st2.currlevel and st2.totalresult = 0
   and hc.id = 20150623153358))
) 
and conid = 20150623153358);


这样一来触发器变为
CREATE trigger set_signature_status_totalresult
BEFORE UPDATE on `signaturestatus` 
FOR EACH ROW 

BEGIN

         if (new.result1 = '1' and new.result2 = '1' and new.result3 = '1' and new.result4 = '1' and new.result5 = '1' and new.result6 = '1' and new.result7 = '1' and new.result8 = '1')

            then set new.totalresult = '1';

         elseif (new.result1 = '-1' or new.result2 = '-1' or new.result3 = '-1' or new.result4 = '-1' or new.result5 = '-1' or new.result6 = '-1' or new.result7 = '-1' or new.result8 = '-1')
            
            then set new.totalresult = '-1';

        end if;
CREATE trigger set_signature_status_totalresult
AFTER UPDATE on `signaturestatus` 
FOR EACH ROW 
if
((SELECT count(sl.empid)
FROM (SELECT conid, currlevel, totalresult, updatecount  FROM signaturestatus) st2, signaturelevel  sl, hdjcontract hc, signaturedetail sd
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

END




SELECT hc.id "会签单编号", st2.currlevel "当前签字阶段", count(sl.empid) "已经签字的人数"
FROM (SELECT conid, currlevel, totalresult, updatecount  FROM signaturestatus) st2, signaturelevel  sl, hdjcontract hc, signaturedetail sd
WHERE (st2.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st2.currlevel and st2.totalresult = 0
   and sd.conid = hc.id and sd.empid = sl.empid and sd.result = 1 and sd.updatecount = st2.updatecount
   and hc.id = 20150621124713);


SELECT hc.id "会签单编号", st.currlevel "当前签字阶段", count(sl.empid) "需要签字的人数"
FROM signaturestatus st, signaturelevel  sl, hdjcontract hc
WHERE (st.conid = hc.id 
   and sl.contempid = hc.contempid 
   and sl.signlevel = st.currlevel and st.totalresult = 0
   and hc.id = 20150621124713);

当所有人提交会签单之，触发器正常
但是仍然出现一个问题，正常只应该,当currlevel > maxlevel时会触发每次修改全0都会导致currlevel + 1
因为我们将其改为代码实现







SLECT * FROM signaturestatus st, hdjcontarct hc 
WHERE (hc.id = 20150621124713 and hc.id = st.conid);
// 查询出表20150621124713中第1个签字人的签字信息
// 需要签字明细表signaturedetail  主键 sd.empid + sd.conid + sd.updatecount
// 会签单表 hdjcontract  主键 hc.id
// 签字对应表signaturelevel sl.conid + sl.signnum
// 会签单状态表 signaturestatus st.conid + st.updatecount 
SELECT hc.id "会签单编号", sd.result "签字信息", sd.remark "签字备注", sl.signnum "签字位置",sd.updatecount "签字时间", sd.date
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150621124713 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '1' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount;
  GROUP BY sl.signnum;



SELECT result1, result2, result3, result4, result5, result6, result7, result8
FROM signaturestatus st, hdjcontract hc 
WHERE ();



[BUG1]
用户签完字以后，会签单仍然出现在用户的待签字列表中
引入会签单模版签字顺序表signaturelevel表后，不再需要关联contemp表
当当前单子需要某个人签字的时候，需要满足几个条件
一是，这个会签单仍然需要签字，即签字流程还没走完,signaturestatus中，SQL表示为hc.id = st.conid[当前会签单的在待办会签单状态表中]
二是，当前员工的ID在会签单模版中，即当前会签单需要此ID的员工签字,SQL语句表示为sl.contempid = hc.contempid and sl.empid = @EmployeeId 
三是，这个会签单的当前进的节点currLevel正好等于当前员工的签字顺序号, st.currlevel = sl.signlevel
四是，签字明细表中，没有签单用户的签字信息,

第四点比较难判断，因为我们引入，当前用户已经签过字的签单

SELECT  sd.conid "处理过的会签单", sd.date, "签字时间", sd.result "签字结果", st.totalresult, "会签单状态" 
FROM `signaturestatus` st, `signaturedetail` sd
WHERE (sd.conid = st.conid and sd.empid = 2 and sd.updatecount = st.updatecount);


那么用户的需要签字的会签单编号不应该出现在这里

SELECT  hc.id id, hc.name name, hc.submitdate submitdate, hc.columndata1 columndata1
FROM `hdjcontract` hc, `signaturestatus` st, `signaturelevel` sl
WHERE ((hc.id = st.conid and st.totalresult != 1)
and (sl.contempid = hc.contempid and sl.empid = 1)
and (st.currlevel = sl.signlevel)
and (hc.id not in (SELECT  hc.id id 
FROM `hdjcontract` hc, `signaturestatus` st, `signaturelevel` sl, `signaturedetail` sd
WHERE ((hc.id = st.conid)
and (sl.contempid = hc.contempid and sl.empid = 1)
and (st.currlevel = sl.signlevel)
and (sd.conid = hc.id and sd.empid = sl.empid and sd.updatecount = st.updatecount)))));

//精简如下
SELECT  hc.id id, hc.name name, e.name subempname, hc.submitdate submitdate, hc.columndata1 columndata1
FROM `hdjcontract` hc, `signaturestatus` st, `signaturelevel` sl, `employee` e
WHERE ((hc.id = st.conid and st.totalresult = 0)
and (sl.contempid = hc.contempid and sl.empid = 4)
and (st.currlevel >= sl.signlevel)
and (hc.subempid = e.id)
and (hc.id not in (
SELECT  sd.conid
FROM `signaturestatus` st, `signaturedetail` sd
WHERE (sd.conid = st.conid and sd.empid = 4 and sd.updatecount = st.updatecount))));


// 当前会签单需要用户employeeId已经签字，
当且仅当签字明细表有当前用户的签单信息
SELECT  hc.id "会签单编号", hc.name "会签单名称", hc.columndata1 "工程名称", e.name "提交人", hc.submitdate "提交日期", sd.date "签字时间", sd.result "本人签字结果", sd.remark "本人签字备注"
FROM `hdjcontract` hc, `signaturestatus` st, `signaturelevel` sl, `signaturedetail` sd, employee e
WHERE ((hc.id = st.conid)
and (sl.contempid = hc.contempid and sl.empid = 1)
and (st.currlevel >= sl.signlevel)
and (sd.conid = hc.id and sd.empid = sl.empid and sd.updatecount = st.updatecount)
and hc.subempid = e.id);


SELECT  hc.id "会签单编号", hc.name "会签单名称", hc.columndata1 "工程名称", e.name "提交人", hc.submitdate "提交日期", sd.date "签字时间", sd.result "本人签字结果", sd.remark "本人签字备注"
FROM `hdjcontract` hc, `signaturestatus` st, `signaturelevel` sl, `signaturedetail` sd, employee e
WHERE ((hc.id = st.conid)
and (sl.contempid = hc.contempid and sl.empid = 1)
and (sd.conid = hc.id and sd.empid = sl.empid and sd.updatecount = st.updatecount)
and hc.subempid = e.id);


// 查询出会签单的整个详细信息
SELECT h.id id, h.name name, c.id contempid, c.name contempname, 
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
WHERE (h.id = 20150623164733 and h.contempid = c.id  
and c.signid1 = e1.id  and c.signid2 = e2.id and c.signid3 = e3.id and c.signid4 = e4.id and c.signid5 = e5.id and c.signid6 = e6.id and c.signid7 = e7.id and c.signid8 = e8.id
and d1.id = e1.departmentid and d2.id = e2.departmentid and d3.id = e3.departmentid and d4.id = e4.departmentid and d5.id = e5.departmentid and d6.id = e6.departmentid and d7.id = e7.departmentid and d8.id = e8.departmentid
and h.id = s.conid);

// 查询出签字明细
SELECT sd.conid "会签单编号",sd.empid " 签字人", sd.result "签字信息", sd.remark "签字备注" 
FROM signaturestatus st, signaturedetail sd 
WHERE (st.conid = sd.conid and st.updatecount = sd.updatecount); 

// 查询出表conid的所有签字明细
SELECT sd.conid "会签单编号",sd.empid " 签字人", sd.result "签字信息", sd.remark "签字备注" 
FROM signaturestatus st, signaturedetail sd 
WHERE (st.conid = sd.conid and st.updatecount = sd.updatecount);

// 查询表conid的所有拒绝的签字明细

SELECT sd.conid "会签单编号",sd.empid " 签字人", sd.result "签字信息", sd.remark "签字备注" 
FROM signaturestatus st, signaturedetail sd 
WHERE (st.conid = sd.conid and st.updatecount = sd.updatecount
and sd.result = -1);



// 查询每个人的签字信息
(SELECT sd.conid "会签单编号",sd.empid " 签字人", sd.result "签字信息", sd.remark "签字备注" FROM signaturestatus st, signaturedetail sd 
WHERE (st.conid = sd.conid and st.updatecount = sd.updatecount))

SELECT hc.id "会签单编号", sd.result "签字信息", sd.remark "签字备注", sl.signnum "签字位置",sd.updatecount "签字时间", sd.date
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150623164733 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '1' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount;


// 查询出会签单的整个详细信息（附加上每个人的签字备注）
GET_CONTRACT_STR
SELECT h.id id, h.name name, c.id contempid, c.name contempname, 
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
WHERE hc.id = 20150624175351 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '1' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) remark1,

(SELECT sd.remark remark2
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150624175351 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '2' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) remark2,

  (SELECT sd.remark remark3
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150624175351 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '3' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) remark3,
  (SELECT sd.remark remark4
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150624175351 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '4' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) remark4,
  (SELECT sd.remark remark5
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150624175351 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '5' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) remark5,
  (SELECT sd.remark remark6
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150624175351 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '6' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) remark6,

  (SELECT sd.remark remark7
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150624175351 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '7' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) remark7,
    
  (SELECT sd.remark remark8
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150624175351 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '8' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) remark3

FROM 

hdjcontract h, 
contemp c, 
signaturestatus s,
employee e1, employee e2, employee e3, employee e4, employee e5, employee e6, employee e7, employee e8,
department d1, department d2, department d3, department d4, department d5, department d6, department d7, department d8

WHERE (h.id = 20150624175351 and h.contempid = c.id
and c.signid1 = e1.id  and c.signid2 = e2.id and c.signid3 = e3.id and c.signid4 = e4.id and c.signid5 = e5.id and c.signid6 = e6.id and c.signid7 = e7.id and c.signid8 = e8.id
and d1.id = e1.departmentid and d2.id = e2.departmentid and d3.id = e3.departmentid and d4.id = e4.departmentid and d5.id = e5.departmentid and d6.id = e6.departmentid and d7.id = e7.departmentid and d8.id = e8.departmentid
and h.id = s.conid);







// 查询出会签单的整个详细信息（附加上每个人的签字备注）
SELECT h.id id, h.name name, c.id contempid, c.name contempname, 
c.column1 columnname1, c.column2 columnname2, c.column3 columnname3, c.column4 columnname4, c.column5 columnname5,
h.columndata1 columndata1, h.columndata2 columndata2, h.columndata3 columndata3, h.columndata4 columndata4, h.columndata5 columndata5, 
c.signinfo1 signinfo1, c.signinfo2 signinfo2, c.signinfo3 signinfo3, c.signinfo4 signinfo4, c.signinfo5 signinfo5, c.signinfo5 signinfo5, c.signinfo6 signinfo6, c.signinfo7 signinfo7, c.signinfo8 signinfo8,                                                                  
e1.id signid1, e2.id signid2, e3.id signid3, e4.id signid4, e5.id signid5, e6.id signid6, e7.id signid7, e8.id signid8,
e1.name signname1, e2.name signname2, e3.name signname3, e4.name signname4, e5.name signname5, e6.name signname6, e7.name signname7, e8.name signname8,          
d1.id departmentid1, d2.id departmentid2, d3.id departmentid3, d4.id departmentid4, d5.id departmentid5, d6.id departmentid6, d7.id departmentid7, d8.id departmentid8,
d1.name departmentname1, d2.name departmentname2, d3.name departmentname3, d4.name departmentname4, d5.name departmentname5, d6.name departmentname6, d7.name departmentname7, d8.name departmentname8,
signlevel1 signlevel1, c.signlevel2, c.signlevel2, c.signlevel3, signlevel3, c.signlevel4 signlevel4, c.signlevel5 signlevel5, c.signlevel6 signlevel6, c.signlevel7 signlevel7, c.signlevel8 signlevel8,
s.result1 result1, s.result2 result2, s.result3 result3, s.result4 result4, s.result5 result5, s.result6 result6, s.result7 result7, s.result8 result8,
s1.remark remark1,s2.remark remark2,s3.remark remark3, s4.remark remark4, s5.remark remark5, s6.remark remark6, s7.remark remark7, s8.remark remark8

FROM 

hdjcontract h, 
contemp c, 
signaturestatus s,
employee e1, employee e2, employee e3, employee e4, employee e5, employee e6, employee e7, employee e8,
department d1, department d2, department d3, department d4, department d5, department d6, department d7, department d8,

(SELECT hc.id conid, sl.empid empid, sd.result, sd.remark remark
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150623164733 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '1' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) s1,

(SELECT hc.id conid, sl.empid empid, sd.result, sd.remark remark
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150623164733 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '1' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) s2,

  
(SELECT hc.id conid, sl.empid empid, sd.result, sd.remark remark
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150623164733 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '1' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) s3,
  
(SELECT hc.id conid, sl.empid empid, sd.result, sd.remark remark
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150623164733 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '1' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) s4,

  
(SELECT hc.id conid, sl.empid empid, sd.result, sd.remark remark
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150623164733 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '1' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) s5,

  
(SELECT hc.id conid, sl.empid empid, sd.result, sd.remark remark
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150623164733 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '1' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) s6,

  
(SELECT hc.id conid, sl.empid empid, sd.result, sd.remark remark
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150623164733 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '1' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) s7,

(SELECT hc.id conid, sl.empid empid, sd.result, sd.remark remark
FROM signaturedetail sd, hdjcontract hc, signaturelevel sl, signaturestatus st
WHERE hc.id = 20150623164733 and hc.id = sd.conid and st.conid = hc.id
  and sl.contempid = hc.contempid and sl.signnum = '1' and sd.empid = sl.empid
  and sd.updatecount = st.updatecount) s8

WHERE (h.id = 20150623164733 and h.contempid = c.id and h.id = s.conid
and c.signid1 = e1.id  and c.signid2 = e2.id and c.signid3 = e3.id and c.signid4 = e4.id and c.signid5 = e5.id and c.signid6 = e6.id and c.signid7 = e7.id and c.signid8 = e8.id
and d1.id = e1.departmentid and d2.id = e2.departmentid and d3.id = e3.departmentid and d4.id = e4.departmentid and d5.id = e5.departmentid and d6.id = e6.departmentid and d7.id = e7.departmentid and d8.id = e8.departmentid
 and s1.empid = e1.id
 and s2.empid = e2.id
 and s3.empid = e3.id
 and s4.empid = e4.id
and s5.empid = e5.id
and s6.empid = e6.id
 and s7.empid = e7.id
and s8.empid = e8.id
);












// 生成是可以使用如下SQL语句，但是查询GET时候不可以
// 因为如果单子被拒绝了，那么signaturedetail查询的结果可能为空
SELECT h.id id, h.name name, c.id contempid, c.name contempname, 
c.column1 columnname1, c.column2 columnname2, c.column3 columnname3, c.column4 columnname4, c.column5 columnname5,
h.columndata1 columndata1, h.columndata2 columndata2, h.columndata3 columndata3, h.columndata4 columndata4, h.columndata5 columndata5, 
c.signinfo1 signinfo1, c.signinfo2 signinfo2, c.signinfo3 signinfo3, c.signinfo4 signinfo4, c.signinfo5 signinfo5, c.signinfo5 signinfo5, c.signinfo6 signinfo6, c.signinfo7 signinfo7, c.signinfo8 signinfo8,                                                                  
e1.id signid1, e2.id signid2, e3.id signid3, e4.id signid4, e5.id signid5, e6.id signid6, e7.id signid7, e8.id signid8,
e1.name signname1, e2.name signname2, e3.name signname3, e4.name signname4, e5.name signname5, e6.name signname6, e7.name signname7, e8.name signname8,          
d1.id departmentid1, d2.id departmentid2, d3.id departmentid3, d4.id departmentid4, d5.id departmentid5, d6.id departmentid6, d7.id departmentid7, d8.id departmentid8,
d1.name departmentname1, d2.name departmentname2, d3.name departmentname3, d4.name departmentname4, d5.name departmentname5, d6.name departmentname6, d7.name departmentname7, d8.name departmentname8,
signlevel1 signlevel1, c.signlevel2, c.signlevel2, c.signlevel3, signlevel3, c.signlevel4 signlevel4, c.signlevel5 signlevel5, c.signlevel6 signlevel6, c.signlevel7 signlevel7, c.signlevel8 signlevel8,
s.result1 result1, s.result2 result2, s.result3 result3, s.result4 result4, s.result5 result5, s.result6 result6, s.result7 result7, s.result8 result8,
s1.remark remark1,s2.remark remark2,s3.remark remark3, s4.remark remark4, s5.remark remark5, s6.remark remark6, s7.remark remark7, s8.remark remark8
FROM 

hdjcontract h, 
contemp c, 
signaturestatus s,
employee e1, employee e2, employee e3, employee e4, employee e5, employee e6, employee e7, employee e8,
department d1, department d2, department d3, department d4, department d5, department d6, department d7, department d8,
signaturedetail s1, signaturedetail s2, signaturedetail s3, signaturedetail s4,
signaturedetail s5, signaturedetail s6, signaturedetail s7, signaturedetail s8

WHERE (h.id = 20150623164733 and h.contempid = c.id and h.id = s.conid
and c.signid1 = e1.id  and c.signid2 = e2.id and c.signid3 = e3.id and c.signid4 = e4.id and c.signid5 = e5.id and c.signid6 = e6.id and c.signid7 = e7.id and c.signid8 = e8.id
and d1.id = e1.departmentid and d2.id = e2.departmentid and d3.id = e3.departmentid and d4.id = e4.departmentid and d5.id = e5.departmentid and d6.id = e6.departmentid and d7.id = e7.departmentid and d8.id = e8.departmentid
 and s1.empid = e1.id and s1.conid = h.id and s1.updatecount = s.updatecount
 and s2.empid = e2.id and s2.conid = h.id and s2.updatecount = s.updatecount
 and s3.empid = e3.id and s3.conid = h.id and s3.updatecount = s.updatecount
 and s4.empid = e4.id and s4.conid = h.id and s4.updatecount = s.updatecount
 and s5.empid = e5.id and s5.conid = h.id and s5.updatecount = s.updatecount
 and s6.empid = e6.id and s6.conid = h.id and s6.updatecount = s.updatecount
 and s7.empid = e7.id and s7.conid = h.id and s7.updatecount = s.updatecount
 and s8.empid = e8.id and s8.conid = h.id and s8.updatecount = s.updatecount

);





[表头]
目前需要解决的问题
删除数据时，数据库的正常处理
如果删除的数据与某个表相互关联，我们就不能只是简单的删除，否则会破坏数据库的参照完整性

首先是删除用户数据，用户的ID是主键，而username是唯一的标识

 . cascade方式
在父表上update/delete记录时，同步update/delete掉子表的匹配记录 

   . set null方式
在父表上update/delete记录时，将子表上匹配记录的列设为null
要注意子表的外键列不能为not null  

   . No action方式
如果子表中有匹配的记录,则不允许对父表对应候选键进行update/delete操作  

   . Restrict方式
同no action, 都是立即检查外键约束

   . Set default方式
父表有变更时,子表将外键列设置成一个默认的值 但Innodb不能识别

删除部门时（删除部门时）
作为员工信息的一个属性成员
因为我们假定如果需要删除某一个部门的时候，则将其部门员工全部清楚
修改员工时，则级联修改所有员工信息

删除员工时用户的的ID，
作为会签单表[主键id + 唯一标识 = submitdate + submitid]中的提交人subempid，
作为会签单模版[主键为编号]的签字人empid
作为签字明细[主键 conid + empid + updatecount]中的签字Id
因此删除员工时则级联删除对应的信息


// 查询出表20150621124713中第1个签字人的签字信息
// 需要签字明细表signaturedetail  主键 sd.empid + sd.conid + sd.updatecount
// 会签单表 hdjcontract  主键 hc.id
// 签字对应表signaturelevel sl.conid + sl.signnum
// 会签单状态表 signaturestatus st.conid + st.updatecount

查询当前签字人是否有下载和查看签字状态的权限

SELECT hc.id "会签单编号", sl.canview "是否可查看"
FROM `signaturelevel` sl, `hdjcontract` hc
WHERE (sl.empid = 1 and hc.contempid = sl.contempid);



2015-08-11 需求变更因此新增几个表对象


[engineercategory]
存储了工程类别信息，
信息如下 
category对应界河航道养护工程, 内河航道养护工程, 应急抢通工程, 例会项目工程
categoryshortcall对应 界，内。应，例 用来在标识工程类别信息



[更新2015-08-24 11:42]
我们需要实现如下几个功能
在部门管理界面，
A -- 显示出当前部门的信息以及项目申请类别的权限（是/否）
B -- 如果用户点击是/否，实现是否的转换，同时修改数据库中的部门申请选线
C -- 在申请会签单时，填写编号，选择部门后，直接对应后面的申请权限信息

为了判断不同部门的不同项目同脚权限


我们在部门表中新增加了几个数据域
canboundary, caninland, canemergency, canregular
来标识当前部门是否可以提交，界内应例项目的权限
此修改比较容易实现AB功能，实现对部门的管理

同时还有一个表对应此数据项的关联性信息
表conidcategory
表中数据域 departmentid(对应部门) 和 categoryid(对应工程类别)
此表可直接读取出C的信息，
但是C的功能也可以通过扩展的部门功能实现

则当前此功能有多种实现方案
对于AB功能，直接读取和操作
读取部门信息时，直接读取部门表，存储为SDepartment，
部门通过点击是/否按钮，改写部门表中的扩展信息


对于C功能
①可以直接读取并组装department表，
②也可以分析表conidcategory
其中①方法特别不适合扩充
②方法更加灵活，但是需要更多的模块支持，
需要添加一个engineercategory表存储工程类别，同时需要conidcategory关联工程类别和部门
因此我们采用方法二，但是其中canboundary, caninland, canemergency, canregular不对外开发，仅是只读属性
修改时直接通过向数据库conidcategory



我们现在选用的设计方法是，conidcategory是一个只读属性的触发器表
// 查询部门的详细信息
SELECT id, name, shortcall, canboundary, caninland, canemergency, canregular
FROM department




SELECT id FROM `engineercategory` WHERE (`categoryshortcall` =  "界");
// 为部门添加界-内-应-例的权限
        INSERT into `conidcategory` (`departmentid`, `categoryid`)
        VALUES(new.id, (SELECT id FROM `engineercategory` WHERE (`categoryshortcall` =  "界")));


// 如果将department作为读写表，conidcategory作为只读表
CREATE trigger set_conidcategory
BEFORE UPDATE on `department`
FOR EACH ROW 
BEGIN
    if (old.canboundary = 0 and new.canboundary = 1) then 
        INSERT into `conidcategory` (`departmentid`, `categoryid`)
        VALUES(new.id, 1);
    elseif (old.canboundary = 1 and new.canboundary = 0) then 
        DELETE from `conidcategory` 
        WHERE(`departmentid` = new.id and `categoryid` = 1);
    end if;

    if (old.caninland = 0 and new.caninland = 1) then 
        INSERT into `conidcategory` (`departmentid`, `categoryid`)
        VALUES(new.id, 2);
    elseif (old.caninland = 1 and new.caninland = 0) then 
        DELETE from `conidcategory`
        WHERE(`departmentid` = new.id and `categoryid` = 2);
    end if;
        
    if (old.canemergency = 0 and new.canemergency = 1) then 
        INSERT into `conidcategory` (`departmentid`, `categoryid`)
        VALUES(new.id, 3);
    elseif (old.canemergency = 1 and new.canemergency = 0) then 
        DELETE from `conidcategory`
        WHERE(`departmentid` = new.id and `categoryid` = 3);
    end if;

    if (old.canregular = 0 and new.canregular = 1) then 
        INSERT into `conidcategory` (`departmentid`, `categoryid`)
        VALUES(new.id, 3);
    elseif (old.canregular = 1 and new.canregular = 0) then 
        DELETE from `conidcategory`
        WHERE(`departmentid` = new.id and `categoryid` = 4);
    end if;
 END;


 
// 如果将conidcategory作为读写表
// 那么触发器变成
CREATE trigger set_department_category
BEFORE INSERT on `conidcategory`
FOR EACH ROW 
BEGIN

    if(new.categoryid == 1)
    then    UPDATE `department` SET canboundary = 1 WHERE (id = new.id);

    if(new.categoryid == 2)
    then    UPDATE `department` SET caninland = 1 WHERE (id = new.id);
   
    if(new.categoryid == 3)
    then    UPDATE `department` SET canemergency = 1 WHERE (id = new.id);
    
    if(new.categoryid == 4)
    then    UPDATE `department` SET canregular = 1 WHERE (id = new.id);

END;


CREATE trigger set_department_category
BEFORE DELETE on `conidcategory`
FOR EACH ROW 
BEGIN

    if(old.categoryid == 1)
    then    UPDATE `department` SET canboundary = 0 WHERE (id = new.id);

    if(old.categoryid == 2)
    then    UPDATE `department` SET caninland = 0 WHERE (id = new.id);
   
    if(old.categoryid == 3)
    then    UPDATE `department` SET canemergency = 0 WHERE (id = new.id);
    
    if(old.categoryid == 4)
    then    UPDATE `department` SET canregular = 0 WHERE (id = new.id);

END;


SELECT ec.id, ec.category, ec.categoryshortcall 
FROM `conidcategory` cc, `engineercategory` ec 
WHERE (cc.departmentid = 1 and ec.id = cc.categoryid);


