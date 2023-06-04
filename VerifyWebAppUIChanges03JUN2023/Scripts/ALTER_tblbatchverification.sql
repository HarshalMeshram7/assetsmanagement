

ALTER TABLE `tblbatchverification` 
ADD COLUMN `Longitude` DECIMAL(10,8) NULL AFTER `LastUpdateTimeStamp`,
ADD COLUMN `Latitude` DECIMAL(10,8) NULL AFTER `Longitude`,
ADD COLUMN `Assetaddress` VARCHAR(500) NULL AFTER `Latitude`;
