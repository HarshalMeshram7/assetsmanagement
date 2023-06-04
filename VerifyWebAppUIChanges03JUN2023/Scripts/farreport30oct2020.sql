DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `farreport`(IN companyid int ,IN startdate date)
BEGIN


/*

"AssetNo", "AssetName", "VoucherDate", "Date Put To Use ",
                    "SupplierName", "Qty", "Location", "SubLocation",
                    "Sub_SubLocation", "IssueDate", "Amount Capitalised"


*/

 
 declare v_assetid int;


            
   DROP TEMPORARY TABLE IF EXISTS temp_report_final ;
    CREATE TEMPORARY TABLE temp_report_final (
		
        
			assetid  INT ,
			companyid INT NOT NULL,
            AGroupName text,
            BGroupName text,
            CGroupName text,
            DGroupName text,
            AssetNo text,
			AssetName text,
            AssetIdentificationNo text,
            VoucherNo text,
			VoucherDate Datetime,
			DTPutUseCompany Datetime,
		    SupplierName text,   
             Qty int,
			DepRate decimal(18,2),
			DepMethod text,
            AmountCapitalisedCompany decimal(18,2),
          AmountCapitalisedIT decimal(18,2),
		DepreciationAmount decimal(18,2),
        NetBalance decimal(18,2),
		TotalCedit decimal(18,2),
       InvoiceAmount decimal(18,2),
       TransactionType text
       
      
            );
            
            --   for assets table
            insert into temp_report_final (assetid,companyid,AGroupName,BGroupName,CGroupName,DGroupName,
			AssetNo , AssetName,AssetIdentificationNo ,VoucherNo,VoucherDate ,DTPutUseCompany ,SupplierName,Qty,
				DepRate,DepMethod ,AmountCapitalisedCompany ,AmountCapitalisedIT ,DepreciationAmount,NetBalance,TotalCedit,InvoiceAmount,
                TransactionType)
			select id, companyid,null AGroupName,null BGroupName,null CGroupName,null DGroupName,AssetNo,AssetName,AssetIdentificationNo,VoucherNo,
            VoucherDate,DtPutToUse,null SupplierName,Qty,TotalRate,DepreciationMethod,AmountCapitalisedCompany,AmountCapitalisedIT,OPAccDepreciation,
            (AmountCapitalisedCompany-OPAccDepreciation),TotalCredit,InvoiceAmt,"Purchase"
				 from tblassets where companyid=companyid and VoucherDate<=startdate ;

           update temp_report_final set SupplierName =(select (A.SupplierName) from tblsupplier A inner join tblassets D on A.id=D.Supplierno and A.companyid=D.companyid where D.id=assetid ) ;
           update temp_report_final set AGroupName = (select (A.AGroupName) from tblagroup A inner join tblassets D on A.id=D.AGroupID  and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
           update temp_report_final set BGroupName =(select (A.BGroupName) from tblbgroup A inner join tblassets D on A.id=D.BGroupID  and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0; 
           update temp_report_final set CGroupName =(select (A.CGroupName) from tblcgroup A inner join tblassets D on A.id=D.CGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
		 update temp_report_final set DGroupName =(select (A.DGroupName) from tbldgroup A inner join tblassets D on A.id=D.DGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
		
        /*for addition 
        
        logic
        
        */
         -- for disposal 
         
          insert into temp_report_final (assetid,companyid,AGroupName,BGroupName,CGroupName,DGroupName,
			AssetNo ,AssetName,AssetIdentificationNo ,VoucherNo,VoucherDate ,DTPutUseCompany ,SupplierName,Qty,
				DepRate,DepMethod ,AmountCapitalisedCompany ,AmountCapitalisedIT ,DepreciationAmount,NetBalance,TotalCedit,InvoiceAmount,
                TransactionType)
			select assetid, companyid,null AGroupName,null BGroupName,null CGroupName,null DGroupName,null AssetNo,D.AssetName,A.AssetIdentificationNo,D.VoucherNo,
            D.VoucherDate,D.DisposalDate,null SupplierName,(D.Qty-A.Qty),A.TotalRate,A.DepreciationMethod,(D.GrossAmount-A.AmountCapitalisedCompany),(D.GrossAmount-A.AmountCapitalisedIT),0,
            0,0,0,"disposal"
				 from tbldisposal D inner join  tblassets A on D.AssetId=A.id  and A.companyid=D.companyid where D.companyid=companyid and D.VoucherDate<=startdate ;

           update temp_report_final set SupplierName =(select (A.SupplierName) from tblsupplier A inner join tblassets D on A.id=D.Supplierno where D.id=assetid) ;
           update temp_report_final set AGroupName = (select (A.AGroupName) from tblagroup A inner join tblassets D on A.id=D.AGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
           update temp_report_final set BGroupName =(select (A.BGroupName) from tblbgroup A inner join tblassets D on A.id=D.BGroupID  and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0; 
           update temp_report_final set CGroupName =(select (A.CGroupName) from tblcgroup A inner join tblassets D on A.id=D.CGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
		 update temp_report_final set DGroupName =(select (A.DGroupName) from tbldgroup A inner join tblassets D on A.id=D.DGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
        
        
        -- depreciation
         insert into temp_report_final (assetid,companyid,AGroupName,BGroupName,CGroupName,DGroupName,
			AssetNo ,AssetName,AssetIdentificationNo ,VoucherNo ,VoucherDate ,DTPutUseCompany ,SupplierName,Qty,
				DepRate,DepMethod ,AmountCapitalisedCompany ,AmountCapitalisedIT ,DepreciationAmount,NetBalance,TotalCedit,InvoiceAmount,
                TransactionType)
			select assetid, companyid,null AGroupName,null BGroupName,null CGroupName,null DGroupName,A.AssetNo,D.AssetName,A.AssetIdentificationNo,null VoucherNo,
            D.FromDate,D.ToDate,null SupplierName,A.Qty,A.TotalRate,A.DepreciationMethod,A.AmountCapitalisedCompany,A.AmountCapitalisedIT,Amount,
            0,0,0,"depreciation"
				 from tbldepreciation D inner join  tblassets A on D.AssetId=A.id  and A.companyid=D.companyid where D.companyid=companyid and  D.ToDate <= startdate ;

           update temp_report_final set SupplierName =(select (A.SupplierName) from tblsupplier A inner join tblassets D on A.id=D.Supplierno and A.companyid=D.companyid where D.id=assetid) ;
           update temp_report_final set AGroupName = (select (A.AGroupName) from tblagroup A inner join tblassets D on A.id=D.AGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
           update temp_report_final set BGroupName =(select (A.BGroupName) from tblbgroup A inner join tblassets D on A.id=D.BGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0; 
           update temp_report_final set CGroupName =(select (A.CGroupName) from tblcgroup A inner join tblassets D on A.id=D.CGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
		 update temp_report_final set DGroupName =(select (A.DGroupName) from tbldgroup A inner join tblassets D on A.id=D.DGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
        
		select * from temp_report_final; /* repoort output */
        

END$$
DELIMITER ;
