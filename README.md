# SignPressServer
一个远程签单工具的服务器程序


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


[问题1]
怎么查询每个人是否有未签字的单子信息
一种实现方案，就是上面的[签字状态表id + 签字明细表]
首先，签字状态表是对外是一个只读表，其数据的修改，由数据库触发器进行维护
用户提交签单时（即用户在数据库插入或者修改签单之后），通过触发器在数据库signaturestatus表中插入一项数据，数据项全为未处理
用户每次签字，通过触发器在在签字明细表signaturestatus中，置对应数据项为[同意或者拒绝]

每次查询自己是否有签字信息的时候的时候，查询一下签字状态表中，是否未处理的会签单信息
[通过会签单信息，查询出会签单的模版信息，通过模版信息查询出每个签字人的信息]
使用一个方法处理，直接通过会签单模版获取出每个签字人的信息id即可
继而通过



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


