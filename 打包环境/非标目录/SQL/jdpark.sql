/*
liliangfeng

Date: 2018-06-20 18:34:29
*/


CREATE DATABASE jdpark CHARACTER SET utf8;

use jdpark;

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for jdbill
-- ----------------------------
DROP TABLE IF EXISTS `jdbill`;
CREATE TABLE `jdbill` (
  `LogNo` varchar(100) NOT NULL,
  `ResultCode` varchar(20) DEFAULT NULL,
  `QrCode` varchar(1000) DEFAULT NULL,
  `Cost` varchar(20) DEFAULT NULL,
  `PayResult` tinyint(2) DEFAULT NULL,
  `ReasonCode` varchar(50) DEFAULT NULL,
  `Reason` varchar(255) DEFAULT NULL,
  `CreateTime` datetime DEFAULT NULL,
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`ID`),
  KEY `LogNo_Index` (`LogNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for jdbillarchived
-- ----------------------------
DROP TABLE IF EXISTS `jdbillarchived`;
CREATE TABLE `jdbillarchived` (
  `LogNo` varchar(100) NOT NULL,
  `ResultCode` varchar(20) DEFAULT NULL,
  `QrCode` varchar(1000) DEFAULT NULL,
  `Cost` varchar(20) DEFAULT NULL,
  `PayResult` tinyint(2) DEFAULT NULL,
  `ReasonCode` varchar(50) DEFAULT NULL,
  `Reason` varchar(255) DEFAULT NULL,
  `CreatTime` datetime DEFAULT NULL,
  `ID` bigint(20) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `LogNo_Index` (`LogNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for parkdic
-- ----------------------------
DROP TABLE IF EXISTS `parkdic`;
CREATE TABLE `parkdic` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `KeyStr` varchar(255) DEFAULT NULL,
  `ValueStr` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for parkwhitelist
-- ----------------------------
DROP TABLE IF EXISTS `parkwhitelist`;
CREATE TABLE `parkwhitelist` (
  `VehicleNo` varchar(50) NOT NULL,
  `ParkLotCode` varchar(50) DEFAULT NULL,
  `yn` varchar(10) DEFAULT NULL,
  `PersonId` varchar(50) DEFAULT NULL,
  `ParkServiceId` varchar(50) DEFAULT NULL,
  `BindCar` tinyint(2) DEFAULT NULL,
  `CreateTime` datetime DEFAULT NULL,
  `UpdateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`VehicleNo`),
  KEY `VehicleNo_Index` (`VehicleNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

insert into ParkDic (KeyStr,ValueStr) values ('Version','0');
insert into ParkDic (KeyStr,ValueStr) values ('OverFlowCount','0');
