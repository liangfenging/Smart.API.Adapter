/*
liliangfeng

Date: 2018-08-16
*/



use jdpark;

SET FOREIGN_KEY_CHECKS=0;

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



