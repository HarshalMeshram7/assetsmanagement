DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `Disposal`(IN companyid int )
BEGIN


 
 


            
   DROP TEMPORARY TABLE IF EXISTS temp_report_final ;
    CREATE TEMPORARY TABLE temp_report_final (
		
			Id INT,
            companyid INT,
			DisposalDate text,
            DisposalType text,
            Remarks text,
            AssetNo text,
			AssetName text,
		    DisposalAmount decimal(18,2), 
             Qty int
			
            );
            
            --   for assets table
            insert into temp_report_final (Id,companyid,DisposalDate,DisposalType,Remarks,AssetNo,
			AssetName , DisposalAmount,Qty )
			select D.ID,D.Companyid,DATE_FORMAT(D.DisposalDate,'%d/%m/%Y'),D.DisposalType,D.Remarks,A.AssetNo,D.AssetName,D.DisposalAmount,D.Qty
         
				 from tblDisposal D inner join  tblassets A on D.AssetId=A.id  and A.companyid=D.companyid where companyid=companyid ;

        
        
		select * from temp_report_final; /* repoort output */
        

END$$
DELIMITER ;
