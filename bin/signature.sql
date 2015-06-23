/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50529
Source Host           : localhost:3306
Source Database       : signature

Target Server Type    : MYSQL
Target Server Version : 50529
File Encoding         : 65001

Date: 2015-06-21 22:58:25
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `contemp`
-- ----------------------------
DROP TABLE IF EXISTS `contemp`;
CREATE TABLE `contemp` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '会签单模版编号',
  `createdate` datetime NOT NULL,
  `name` varchar(255) NOT NULL COMMENT '会签单模版名称',
  `column1` varchar(255) NOT NULL COMMENT '栏目1',
  `column2` varchar(255) NOT NULL COMMENT '栏目2',
  `column3` varchar(255) NOT NULL COMMENT '栏目3',
  `column4` varchar(255) NOT NULL COMMENT '栏目4',
  `column5` varchar(255) NOT NULL COMMENT '栏目5',
  `signinfo1` varchar(255) NOT NULL COMMENT '签字人1信息',
  `signinfo2` varchar(255) NOT NULL COMMENT '签字人2信息',
  `signinfo3` varchar(255) NOT NULL COMMENT '签字人3信息',
  `signinfo4` varchar(255) NOT NULL COMMENT '签字人4信息',
  `signinfo5` varchar(255) NOT NULL COMMENT '签字人5信息',
  `signinfo6` varchar(255) NOT NULL COMMENT '签字人6信息',
  `signinfo7` varchar(255) NOT NULL COMMENT '签字人7信息',
  `signinfo8` varchar(255) NOT NULL COMMENT '签字人8信息',
  `signid1` int(11) NOT NULL COMMENT '签字人1的员工号',
  `signid2` int(11) NOT NULL COMMENT '签字人2的员工号',
  `signid3` int(11) NOT NULL COMMENT '签字人3的员工号',
  `signid4` int(11) NOT NULL COMMENT '签字人4的员工号',
  `signid5` int(11) NOT NULL COMMENT '签字人5的员工号',
  `signid6` int(11) NOT NULL COMMENT '签字人6的员工号',
  `signid7` int(11) NOT NULL COMMENT '签字人7的员工号',
  `signid8` int(11) NOT NULL COMMENT '签字人8的员工号',
  `signlevel1` int(11) DEFAULT NULL COMMENT '签字人1的签字顺序',
  `signlevel2` int(11) DEFAULT NULL COMMENT '签字人2的签字顺序',
  `signlevel3` int(11) DEFAULT NULL COMMENT '签字人3的签字顺序',
  `signlevel4` int(11) DEFAULT NULL COMMENT '签字人4的签字顺序',
  `signlevel5` int(11) DEFAULT NULL COMMENT '签字人5的签字顺序',
  `signlevel6` int(11) DEFAULT NULL COMMENT '签字人6的签字顺序',
  `signlevel7` int(11) DEFAULT NULL COMMENT '签字人7的签字顺序',
  `signlevel8` int(11) DEFAULT NULL COMMENT '签字人8的签字顺序',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of contemp
-- ----------------------------
INSERT INTO `contemp` VALUES ('1', '2015-06-20 12:30:30', '养护及例会项目拨款会签审批单模版 ', '工程名称', '项目名称', '主要项目及工程量', '本次申请资金额度（元）', '累计申请资金额度（元）', '申请单位项目负责人', '申请单位负责人', '养护主管部门项目负责人', '养护主管部门负责人', '计划科负责人', '财务科负责人', '主 管 局 长', '局      长', '1', '2', '3', '4', '5', '6', '8', '7', '1', '1', '1', '1', '1', '1', '2', '3');

-- ----------------------------
-- Table structure for `department`
-- ----------------------------
DROP TABLE IF EXISTS `department`;
CREATE TABLE `department` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '部门编号',
  `name` varchar(255) NOT NULL COMMENT '部门名称',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of department
-- ----------------------------
INSERT INTO `department` VALUES ('2', '养护科');
INSERT INTO `department` VALUES ('8', '四爷好逗');
INSERT INTO `department` VALUES ('6', '安全部');
INSERT INTO `department` VALUES ('1', '申请科');
INSERT INTO `department` VALUES ('9', '老五是Sb');
INSERT INTO `department` VALUES ('5', '行政科');
INSERT INTO `department` VALUES ('3', '计划科');
INSERT INTO `department` VALUES ('4', '财务科');
INSERT INTO `department` VALUES ('7', '赵强2b科');

-- ----------------------------
-- Table structure for `employee`
-- ----------------------------
DROP TABLE IF EXISTS `employee`;
CREATE TABLE `employee` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `position` varchar(255) NOT NULL,
  `departmentid` int(11) NOT NULL,
  `cansubmit` int(11) NOT NULL,
  `cansign` int(11) NOT NULL,
  `isadmin` int(11) NOT NULL,
  `username` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `username` (`username`),
  KEY `departmentid` (`departmentid`),
  CONSTRAINT `employee_ibfk_1` FOREIGN KEY (`departmentid`) REFERENCES `department` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of employee
-- ----------------------------
INSERT INTO `employee` VALUES ('1', '成坚', '负责人', '1', '1', '1', '0', 'chengjian', 'chengjian');
INSERT INTO `employee` VALUES ('2', '石啸', '主管', '1', '1', '1', '0', 'shixiao', 'shixiao');
INSERT INTO `employee` VALUES ('3', '张立平', '负责人', '2', '0', '1', '0', 'zhanglilping', 'zhangliping');
INSERT INTO `employee` VALUES ('4', '许伟', '科长', '2', '0', '1', '0', 'xuwei', 'xuwei');
INSERT INTO `employee` VALUES ('5', '赵强', '科长', '3', '0', '1', '0', 'zhaoqiang', 'zhaoqiang');
INSERT INTO `employee` VALUES ('6', '李景龙', '科长', '4', '0', '1', '0', 'lijinglong', 'lijinglong');
INSERT INTO `employee` VALUES ('7', '王盼盼', '副局长', '5', '0', '1', '1', 'wangpanpan', 'wangpanpan');
INSERT INTO `employee` VALUES ('8', '吴佳怡', '局长', '5', '0', '1', '1', 'wujiayi', 'wujiayi');

-- ----------------------------
-- Table structure for `hdjcontract`
-- ----------------------------
DROP TABLE IF EXISTS `hdjcontract`;
CREATE TABLE `hdjcontract` (
  `id` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `contempid` int(11) NOT NULL COMMENT '当前会签单所使用的模版类',
  `columndata1` varchar(255) NOT NULL,
  `columndata2` varchar(255) NOT NULL,
  `columndata3` varchar(255) NOT NULL,
  `columndata4` varchar(255) NOT NULL,
  `columndata5` varchar(255) NOT NULL,
  `subempid` int(11) NOT NULL COMMENT '申请人的ID（=0表示申请人与）',
  `submitdate` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `contempid` (`contempid`),
  KEY `subempid` (`subempid`),
  CONSTRAINT `hdjcontract_ibfk_1` FOREIGN KEY (`contempid`) REFERENCES `contemp` (`id`),
  CONSTRAINT `hdjcontract_ibfk_2` FOREIGN KEY (`subempid`) REFERENCES `employee` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of hdjcontract
-- ----------------------------
INSERT INTO `hdjcontract` VALUES ('1', '会签单', '1', '阿里巴巴', '马云', '你没', '你妹', '赵老五', '2', null);
INSERT INTO `hdjcontract` VALUES ('20150621124713', '养护及例会项目拨款会签审批单 ', '1', '航道局', '百度收购案', '收购百度', '1000', '2000', '2', '2015-06-21 00:00:00');
INSERT INTO `hdjcontract` VALUES ('20150621125041', '养护及例会项目拨款会签审批单 ', '1', '航道局', '腾讯收购案', '收购腾讯', '200', '200', '2', '2015-06-21 00:00:00');
INSERT INTO `hdjcontract` VALUES ('20150621131837', '养护及例会项目拨款会签审批单 ', '1', 'dfrfrg', 'sdgf', 'dsg', 'dsgf', 'sdg', '2', '2015-06-21 00:00:00');
INSERT INTO `hdjcontract` VALUES ('20150621132328', '养护及例会项目拨款会签审批单 ', '1', 'g`w', 'ewf', 'ewf', 'fwedf', 'fw', '2', '2015-06-21 00:00:00');
INSERT INTO `hdjcontract` VALUES ('20150621182606', '养护及例会项目拨款会签审批单 ', '1', 'dsds', 'dsdsds', 'dsgff', 'sdasga', 'ga', '2', '2015-06-21 18:26:06');

-- ----------------------------
-- Table structure for `signaturedetail`
-- ----------------------------
DROP TABLE IF EXISTS `signaturedetail`;
CREATE TABLE `signaturedetail` (
  `id` varchar(255) NOT NULL COMMENT '签字明细的编号',
  `date` datetime NOT NULL COMMENT '签字的日期',
  `empid` int(11) NOT NULL COMMENT '签字的人员id',
  `conid` varchar(255) NOT NULL COMMENT '签字的会签单表',
  `result` int(11) NOT NULL COMMENT '签字结果(-1拒绝，0未知,1同意)',
  `remark` varchar(255) NOT NULL COMMENT '签字的备注信息',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of signaturedetail
-- ----------------------------
INSERT INTO `signaturedetail` VALUES ('1', '2015-06-21 00:00:00', '1', '1', '1', '同意了，写的不错');
INSERT INTO `signaturedetail` VALUES ('2', '2015-06-21 00:00:00', '1', '1', '1', '同意');
INSERT INTO `signaturedetail` VALUES ('3', '2015-06-21 00:00:00', '2', '1', '-1', '同意了');
INSERT INTO `signaturedetail` VALUES ('4', '2015-06-21 00:00:00', '3', '1', '-1', '拒绝');
INSERT INTO `signaturedetail` VALUES ('5', '2015-06-21 00:00:00', '4', '2', '1', '同意了');
INSERT INTO `signaturedetail` VALUES ('6', '2015-06-21 00:00:00', '4', '2015-06-21 12:50:41', '-1', '拒绝');
INSERT INTO `signaturedetail` VALUES ('7', '2015-06-21 00:00:00', '4', '20150621125041', '-1', '拒绝');

-- ----------------------------
-- Table structure for `signaturelevel`
-- ----------------------------
DROP TABLE IF EXISTS `signaturelevel`;
CREATE TABLE `signaturelevel` (
  `contempid` int(11) NOT NULL COMMENT '对应的会签单模版',
  `signnum` int(11) NOT NULL COMMENT '会签单中第signnum个签字人',
  `empid` int(11) DEFAULT NULL COMMENT '对应的员工empid',
  `signlevel` int(11) DEFAULT NULL COMMENT '签字的顺序号',
  PRIMARY KEY (`contempid`,`signnum`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of signaturelevel
-- ----------------------------
INSERT INTO `signaturelevel` VALUES ('1', '1', '1', '1');
INSERT INTO `signaturelevel` VALUES ('1', '2', '2', '1');
INSERT INTO `signaturelevel` VALUES ('1', '3', '3', '1');
INSERT INTO `signaturelevel` VALUES ('1', '4', '4', '1');
INSERT INTO `signaturelevel` VALUES ('1', '5', '5', '1');
INSERT INTO `signaturelevel` VALUES ('1', '6', '6', '1');
INSERT INTO `signaturelevel` VALUES ('1', '7', '8', '2');
INSERT INTO `signaturelevel` VALUES ('1', '8', '7', '3');

-- ----------------------------
-- Table structure for `signaturestatus`
-- ----------------------------
DROP TABLE IF EXISTS `signaturestatus`;
CREATE TABLE `signaturestatus` (
  `id` varchar(255) NOT NULL,
  `conid` varchar(255) NOT NULL,
  `result1` int(11) NOT NULL,
  `result2` int(11) NOT NULL,
  `result3` int(11) NOT NULL,
  `result4` int(11) NOT NULL,
  `result5` int(11) NOT NULL,
  `result6` int(11) NOT NULL,
  `result7` int(11) NOT NULL,
  `result8` int(11) NOT NULL,
  `totalresult` int(11) NOT NULL,
  `agreecount` int(11) NOT NULL DEFAULT '0',
  `refusecount` int(11) NOT NULL,
  `currlevel` int(11) NOT NULL COMMENT '当前签字',
  `maxlevel` int(11) NOT NULL COMMENT '完成所需节点',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of signaturestatus
-- ----------------------------
INSERT INTO `signaturestatus` VALUES ('2015-06-21 11:47:25', '2015-6-21', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '3');
INSERT INTO `signaturestatus` VALUES ('2015-06-21 12:47:13', '20150621124713', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '3');
INSERT INTO `signaturestatus` VALUES ('2015-06-21 12:50:41', '20150621125041', '0', '0', '0', '-1', '0', '0', '0', '0', '0', '0', '0', '1', '3');
INSERT INTO `signaturestatus` VALUES ('2015-06-21 13:18:37', '20150621131837', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '3');
INSERT INTO `signaturestatus` VALUES ('2015-06-21 13:23:28', '20150621132328', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '3');
INSERT INTO `signaturestatus` VALUES ('2015-06-21 18:20:05', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '3');
INSERT INTO `signaturestatus` VALUES ('2015-06-21 18:26:06', '20150621182606', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '3');

-- ----------------------------
-- Table structure for `yhjlhxmbkcontract`
-- ----------------------------
DROP TABLE IF EXISTS `yhjlhxmbkcontract`;
CREATE TABLE `yhjlhxmbkcontract` (
  `id` varchar(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL COMMENT '养护及例会项目拨款会签审批单编号',
  `proname` varchar(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL COMMENT '养护及例会项目拨款会签审批单工程名称',
  `termname` varchar(255) NOT NULL,
  `termsize` varchar(255) NOT NULL COMMENT '养护及例会项目拨款会签审批单主要项目以及工作量',
  `reqcapital` int(11) NOT NULL COMMENT '本次申请资金额度（元）',
  `totalcaptial` int(11) NOT NULL COMMENT '累计申请资金额度（元）',
  `reqdepartproid` int(11) NOT NULL COMMENT '申请单位项目负责人',
  `reqdepartid` int(11) NOT NULL COMMENT '申请单位负责人',
  `condepartproid` int(11) NOT NULL COMMENT '养护主管部门项目负责人（需要签字）',
  `condepartid` int(11) NOT NULL COMMENT '养护主管部门负责人（需要签字）',
  `plandepartid` int(11) NOT NULL COMMENT '计划科负责人（需要签字）',
  `finadepartid` int(11) NOT NULL COMMENT '财务科负责人（需要签字）',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of yhjlhxmbkcontract
-- ----------------------------
DROP TRIGGER IF EXISTS `insert_signature_level`;
DELIMITER ;;
CREATE TRIGGER `insert_signature_level` AFTER INSERT ON `contemp` FOR EACH ROW BEGIN

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
END
;;
DELIMITER ;
DROP TRIGGER IF EXISTS `update_signature_level`;
DELIMITER ;;
CREATE TRIGGER `update_signature_level` AFTER UPDATE ON `contemp` FOR EACH ROW BEGIN

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

END
;;
DELIMITER ;
DROP TRIGGER IF EXISTS `insert_signature_status`;
DELIMITER ;;
CREATE TRIGGER `insert_signature_status` AFTER INSERT ON `hdjcontract` FOR EACH ROW BEGIN

INSERT INTO `signaturestatus` (`id`, `conid`, `result1`, `result2`, `result3`, `result4`, `result5`, `result6`, `result7`, `result8`, `totalresult`, `agreecount`, `refusecount`, `currlevel`, `maxlevel`) 
VALUES (NOW(), new.id, '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', (SELECT c.signlevel8 FROM `hdjcontract` h, `contemp` c WHERE (h.contempid = c.id and h.id = new.id)));

END
;;
DELIMITER ;
DROP TRIGGER IF EXISTS `modify_signature_status`;
DELIMITER ;;
CREATE TRIGGER `modify_signature_status` AFTER INSERT ON `signaturedetail` FOR EACH ROW BEGIN
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
        
        END
;;
DELIMITER ;
