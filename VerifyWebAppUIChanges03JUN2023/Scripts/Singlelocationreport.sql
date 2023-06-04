


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `singlelocationreport`(IN companyid int ,IN startdate date,IN alocid int ,IN blocid int,IN clocid int)
BEGIN


/*

"AssetNo", "AssetName", "VoucherDate", "Date Put To Use ",
                    "SupplierName", "Qty", "Location", "SubLocation",
                    "Sub_SubLocation", "IssueDate", "Amount Capitalised"


*/

 
 declare v_assetid int;

 declare v_alocid int;
 declare v_blocid int;
 declare v_clocid int;


            
   DROP TEMPORARY TABLE IF EXISTS temp_report_final ;
    CREATE TEMPORARY TABLE temp_report_final (
		
        
			assetid  INT NOT NULL PRIMARY KEY,
			companyid INT NOT NULL,
            AssetNo text,
            AssetIdentification text,
            AssetName text,
			VoucherDate Datetime,
			Dateputtouse Datetime,
		    SupplierName text,   
             Qty int,
			Location text,
            Sublocation text,
              Sub_Sublocation text,
          --  IssueDate Datetime,
            AmountCapitalised decimal(18,2)
           
            );
            
            set v_alocid=alocid;
            set v_blocid=blocid;
            set v_clocid=clocid;
            
            insert into temp_report_final (assetid,companyid,
			AssetNo , AssetIdentification,AssetName ,VoucherDate ,Dateputtouse ,Qty ,SupplierName,
				Location ,Sublocation ,Sub_Sublocation ,AmountCapitalised)
			select id, companyid,AssetNo,AssetIdentificationNo,AssetName,VoucherDate,DtPutToUse,Qty,null SupplierName
				,null Location,null Sublocation,null Sub_Sublocation,AmountCapitalised from tblassets where companyid=companyid and disposalflag=0  and VoucherDate
                <=startdate and LocAID=v_alocid and LocBID=v_blocid and LocCID=v_clocid;


           update temp_report_final set SupplierName =(select (A.SupplierName) from tblsupplier A inner join tblassets D on A.id=D.Supplierno and A.companyid=D.companyid where D.id=assetid) ;
           update temp_report_final set Location = (select (A.ALocationName) from tblalocation A inner join tblassets D on A.id=D.LocAID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
           update temp_report_final set Sublocation =(select (A.BLocationName) from tblblocation A inner join tblassets D on A.id=D.LocBID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0; 
           update temp_report_final set Sub_Sublocation =(select (A.CLocationName) from tblclocation A inner join tblassets D on A.id=D.LocCID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
		
        
        
        
		select * from temp_report_final; /* repoort output */
        

END$$
DELIMITER ;
