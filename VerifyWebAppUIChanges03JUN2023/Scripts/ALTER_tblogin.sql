ALTER TABLE `tbllogin` 
ADD COLUMN `AppUserid` VARCHAR(45) NULL AFTER `Address`,
ADD COLUMN `IsAppAccess` VARCHAR(45) NULL AFTER `AppUserid`;