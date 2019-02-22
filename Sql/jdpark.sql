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
  `CreateTime` datetime DEFAULT NULL,
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


CREATE TABLE `VehicleLogSql` (
  `LogNo` varchar(100) NOT NULL,
  `actionDescId` varchar(50) DEFAULT NULL,
  `vehicleNo` varchar(50) DEFAULT NULL,
   `parkLotCode` varchar(100) DEFAULT NULL,
  
  `actionPositionCode` varchar(50) DEFAULT NULL,
  `actionPosition`  varchar(200)  DEFAULT NULL,
  `actionTime` datetime DEFAULT NULL,
  `entryTime` datetime DEFAULT NULL,
   `reasonCode` varchar(255) DEFAULT NULL,
   `reason` varchar(255) DEFAULT NULL,
    `photoStr` longtext DEFAULT NULL,
    `photoName` varchar(255) DEFAULT NULL,
	`resend` varchar(255) DEFAULT NULL,
	`postTime` datetime DEFAULT NULL,
	`result` tinyint(2) DEFAULT NULL,
	`failReason` varchar(5000) DEFAULT NULL,
	`token` varchar(255) DEFAULT NULL,
	
	
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`ID`),
  KEY `LogNo_Index` (`LogNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;



CREATE TABLE `VehicleLogArchive` (
  `LogNo` varchar(100) NOT NULL,
  `actionDescId` varchar(50) DEFAULT NULL,
  `vehicleNo` varchar(50) DEFAULT NULL,
   `parkLotCode` varchar(100) DEFAULT NULL,
  
  `actionPositionCode` varchar(50) DEFAULT NULL,
  `actionPosition`  varchar(200)  DEFAULT NULL,
  `actionTime` datetime DEFAULT NULL,
  `entryTime` datetime DEFAULT NULL,
   `reasonCode` varchar(255) DEFAULT NULL,
   `reason` varchar(255) DEFAULT NULL,
    `photoStr` longtext DEFAULT NULL,
    `photoName` varchar(255) DEFAULT NULL,
	`resend` varchar(255) DEFAULT NULL,
	`postTime` datetime DEFAULT NULL,
	`result` tinyint(2) DEFAULT NULL,
	`failReason` varchar(5000) DEFAULT NULL,
	`token` varchar(255) DEFAULT NULL,
	
	
  `ID` bigint(20) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `LogNo_Index` (`LogNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `syncindex` (
  `ID` int(11) NOT NULL,
  `IndexKey` varchar(100) NOT NULL,
  `IndexNo` int(11) DEFAULT NULL,
  `Remark` tinyint(2) DEFAULT 0,
  `CreateTime` datetime DEFAULT NULL,
  `UpdateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`,`IndexKey`),
  KEY `IndexKey_Index` (`IndexKey`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

insert into syncindex values(1,'PhoneNo',0,'Phone last number',now(),now());


delimiter $
create procedure sp_logArchive()
begin
insert into VehicleLogArchive select * from VehicleLogSql where postTime < NOW() - INTERVAL 2 DAY;
delete from VehicleLogSql where postTime < NOW() - INTERVAL 2 DAY;
end
;
delimiter $
create procedure sp_deletelogArchive()
begin
delete from VehicleLogArchive where postTime < NOW() - INTERVAL 30 DAY;
end
;



create event if not exists event_logarchive
on schedule every 1 day
on completion preserve
do call sp_logArchive();


create event if not exists event_deletelogarchive
on schedule every 1 day
on completion preserve
do call sp_deletelogArchive();
