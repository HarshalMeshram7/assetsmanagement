DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `getverifiedasset`(In companyid int,IN batchid int)
BEGIN

 DROP TEMPORARY TABLE IF EXISTS temp_report_final ;
    CREATE TEMPORARY TABLE temp_report_final (
		
        
			assetid  INT NOT NULL PRIMARY KEY,
			companyid INT NOT NULL,
            AssetNo text,
			AssetName text,
			AssetIdentificationno text,
            Srno text,
            Model text,
            Remarks text,
		    Systemassetid text,
            Location text,
            Sublocation text,
            Sub_Sublocation text
		);


    insert into temp_report_final (assetid,companyid,
			AssetNo , AssetName ,AssetIdentificationno,Srno,Model,Remarks,Systemassetid,
				Location ,Sublocation ,Sub_Sublocation )
			select A.id, A.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationno,
            A.Srno,A.Model,A.Remarks,A.MRRNo,null Location,null Sublocation,
            null Sub_Sublocation from tblassets A inner join 
            tblbatchverification D on A.id=D.AssetIndex and 
            D.BatchID=BatchId where   A.companyid=companyid and A.disposalflag=0 ;


		update temp_report_final set Location = (select (A.ALocationName) from tblalocation A inner join tblassets D on A.id=D.LocAID where D.id=assetid) 
                where assetid > 0;
           update temp_report_final set Sublocation =(select (A.BLocationName) from tblblocation A inner join tblassets D on A.id=D.LocBID where D.id=assetid) 
                where assetid > 0; 
           update temp_report_final set Sub_Sublocation =(select (A.CLocationName) from tblclocation A inner join tblassets D on A.id=D.LocCID where D.id=assetid) 
                where assetid > 0;
                
                select * from temp_report_final;

END$$
DELIMITER ;
