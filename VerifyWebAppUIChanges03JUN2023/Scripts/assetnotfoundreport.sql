DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `get_assetnotfound`(In companyid int,IN batchid int)
BEGIN

 DROP TEMPORARY TABLE IF EXISTS temp_report_verified ;
    CREATE TEMPORARY TABLE temp_report_verified (
		
        
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
 DROP TEMPORARY TABLE IF EXISTS temp_report_batchlocationwise ;
         CREATE TEMPORARY TABLE temp_report_batchlocationwise (
		
        
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

 DROP TEMPORARY TABLE IF EXISTS temp_report_Assetfound ;
      CREATE TEMPORARY TABLE temp_report_Assetfound (
		
        
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
        
        DROP TEMPORARY TABLE IF EXISTS temp_report_Assetnotfound ;
      CREATE TEMPORARY TABLE temp_report_Assetnotfound (
		
        
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
        
        
    insert into temp_report_verified (assetid,companyid,
			AssetNo , AssetName ,AssetIdentificationno,Srno,Model,Remarks,Systemassetid,
				Location ,Sublocation ,Sub_Sublocation )
			select A.id, A.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationno,A.Srno,A.Model,A.Remarks,
            A.MRRNo,null Location,null Sublocation,null Sub_Sublocation from tblassets
            A inner join  tblbatchverification D on A.id=D.AssetIndex and  D.BatchID=BatchId where  
            A.companyid=companyid and A.disposalflag=0 ;

	insert into temp_report_batchlocationwise (assetid,companyid,
			AssetNo , AssetName ,AssetIdentificationno,Srno,Model,Remarks,Systemassetid,
				Location ,Sublocation ,Sub_Sublocation )
                
		select asset.id, asset.companyid,asset.AssetNo,asset.AssetName,asset.AssetIdentificationno,asset.Srno,asset.Model,asset.Remarks,asset.MRRNo,
        null Location,null Sublocation,null Sub_Sublocation from tblassets asset inner join  tblsubbatch D on 
        asset.LocAId=D.LocAId  and asset.LocBId=D.LocBId and asset.LocCId=D.LocCId and  D.BatchId=BatchId
        where   asset.companyid=companyid and A.disposalflag=0 ;





 insert into temp_report_Assetfound (assetid,companyid,
			AssetNo , AssetName ,AssetIdentificationno,Srno,Model,Remarks,Systemassetid,
				Location ,Sublocation ,Sub_Sublocation )
		select A.assetid, A.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationno,A.Srno,
        A.Model,A.Remarks,A.Systemassetid,null Location,null Sublocation,null Sub_Sublocation
        from temp_report_batchlocationwise A inner join  temp_report_verified D on A.assetid=D.assetid;


 insert into temp_report_Assetnotfound (assetid,companyid,
			AssetNo , AssetName ,AssetIdentificationno,Srno,Model,Remarks,Systemassetid,
				Location ,Sublocation ,Sub_Sublocation )
		select A.assetid, A.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationno,A.Srno,
        A.Model,A.Remarks,A.Systemassetid,null Location,null Sublocation,null Sub_Sublocation
        from temp_report_batchlocationwise A inner join  temp_report_verified D on A.assetid!=D.assetid;





		update temp_report_Assetnotfound set Location = (select (A.ALocationName) from tblalocation A inner join tblassets D on A.id=D.LocAID where D.id=assetid) 
                where assetid > 0;
           update temp_report_Assetnotfound set Sublocation =(select (A.BLocationName) from tblblocation A inner join tblassets D on A.id=D.LocBID where D.id=assetid) 
                where assetid > 0; 
           update temp_report_Assetnotfound set Sub_Sublocation =(select (A.CLocationName) from tblclocation A inner join tblassets D on A.id=D.LocCID where D.id=assetid) 
                where assetid > 0;
                
                select * from temp_report_Assetnotfound;

END$$
DELIMITER ;
