

ALTER TABLE `tblchild_asset_attachment` 
ADD COLUMN `AssetID` INT NULL AFTER `ModifiedDate`,
CHANGE COLUMN `Ass_Id` `AssetNumber` INT(11) NULL DEFAULT NULL ;
