
DROP PROCEDURE IF EXISTS usp_report_Disposal;
DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_report_Disposal`(IN companyid int,IN startdate date, IN enddate date)
BEGIN


    
    /*DEClARE curAsset CURSOR FOR SELECT id FROM tblassets  where companyid=companyid and disposalflag=0 and id=18245;*/
    
        
    /*
		crete required temp tables
        
    
    */
   
  
    
    
    /*DEClARE curAsset CURSOR FOR SELECT id FROM tblassets  where companyid=companyid and disposalflag=0 and id=18245;*/
    
        
    /*
		crete required temp tables
        
    
    */
   
   
   DROP TEMPORARY TABLE IF EXISTS temp_report_final ;
    CREATE TEMPORARY TABLE temp_report_final (
			id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
			assetid  INT NOT NULL,
			companyid INT NOT NULL,
            AGroupName text,
			BGroupName text,
			CGroupName text,
		    DGroupName text,   
			AssetNo text,
			AssetIdentificationNo text,
			AssetName text,
            Disposal decimal(18,2),
            DispoDep decimal(18,2),
            Qty INT,
			SrNo text,           
            Model text,
            ALocName text,
            BLocName text,
            CLocName text,
			DisposalDate datetime
		);
        
        insert into  temp_report_final (assetid,companyid,AGroupName,BGroupName,CGroupName,DGroupName,
			AssetNo,AssetIdentificationNo,AssetName,Disposal,DispoDep,Qty,SrNo,Model,ALocName,BLocName,CLocName,DisposalDate
        )
        select assetid,companyid,'' AGroupName, '' BGroupName, '' CGroupName, '' DGroupName,
        0 AssetNo, '' AssetIdentificationNo, '' AssetName, DisposalAmount, OpAccumulatedDep, Qty,'' SrNo, '' Model,
        '' ALocName, '' BLocName, '' CLocName,DisposalDate
        from tbldisposal
		where tbldisposal.companyid = companyid
		and  tbldisposal.DisposalDate >= startdate  and tbldisposal.DisposalDate <= enddate;

         /*       
		insert into temp_report_final (companyid,assetid,AGroupName,BGroupName,CGroupName,DGroupName,AssetNo,AssetIdentificationNo,AssetName,
			OpGross , Addition ,Disposal ,ClGross ,OpDep ,
				UpToDep ,DispoDep ,TotDep ,NetBalance,DepRate,DepMethod,voucherDate,VoucherNo,PONo,DTPutUse,
                Remarks,Qty,BillNo,BillDate,SrNo,Model,ALocName,BLocName,CLocName,SupplierName,OpeningQty)
			select companyid,ID,null agroupname,null BGroupName,null CGroupName,null DGroupName,AssetNo,AssetIdentificationNo,AssetName,0 OpGross,0 Addition,0 Disposal,0 ClGross,0 OpDep,
				0 UpToDep,0 DispoDep ,0 TotDep,0 NetBalance,TotalRate,DepreciationMethod,voucherDate,VoucherNo,PONo,DTPuttoUse,Remarks,
                 Qty,BillNo,BillDate,SrNo,Model,null ALocName,null BLocName,null CLocName,null SupplierName,Qty
                 from tblassets where companyid=companyid ;
           */      
                 /*and  DtPutToUse  >= startdate and DtPutToUse <= enddate;*/
		

			
		        update temp_report_final
				inner join tblassets on temp_report_final.assetid =  tblassets.id
                set temp_report_final.AssetNo = tblassets.AssetNo,
					temp_report_final.AssetIdentificationNo = tblassets.AssetIdentificationNo,
                    temp_report_final.AssetName = tblassets.AssetName,
                    temp_report_final.SrNo = tblassets.Srno,
                    temp_report_final.Model = tblassets.Model
                where tblassets.companyid = companyid
                and temp_report_final.id > 0;

		
        /*
              update temp_report_final
				inner join tbldisposal on temp_report_final.assetid =  tbldisposal.Assetid
                set temp_report_final.Disposal = IFNULL(DisposalAmount,0)
                where tbldisposal.companyid = companyid
                and  tbldisposal.voucherdate >= startdate  and tbldisposal.voucherdate <= enddate
                and id > 0;

		*/

			update temp_report_final 
				set temp_report_final.AGroupName = (select (A.AGroupName) from tblagroup A inner join tblassets D on A.id=D.AGroupID where D.id=assetid) 
                where assetid > 0;
			
            update temp_report_final 
				set temp_report_final.BGroupName = (select (A.BGroupName) from tblbgroup A inner join tblassets D on A.id=D.BGroupID where D.id=assetid) 
                where assetid > 0;
                
              update temp_report_final 
				set temp_report_final.CGroupName = (select (A.CGroupName) from tblcgroup A inner join tblassets D on A.id=D.CGroupID where D.id=assetid) 
                where assetid > 0; 
                
                 update temp_report_final 
				set temp_report_final.DGroupName = (select (A.DGroupName) from tbldgroup A inner join tblassets D on A.id=D.DGroupID where D.id=assetid) 
                where assetid > 0;
                
                -- locname
                
                 update temp_report_final 
				set temp_report_final.ALocName = (select (A.ALocationName) from tblalocation A inner join tblassets D on A.id=D.LocAID where D.id=assetid) 
                where assetid > 0;
                
              update temp_report_final 
				set temp_report_final.BLocName = (select (A.BLocationName) from tblblocation A inner join tblassets D on A.id=D.LocBID where D.id=assetid) 
                where assetid > 0; 
                
                 update temp_report_final 
				set temp_report_final.CLocName = (select (A.CLocationName) from tblclocation A inner join tblassets D on A.id=D.LocCID where D.id=assetid) 
                where assetid > 0;
                
                /*
                 update temp_report_final 
				set temp_report_final.SupplierName = (select (A.SupplierName) from tblsupplier A inner join tblassets D on A.id=D.SupplierNo where D.id=assetid) 
                where assetid > 0; 
        */

	/*
		update temp_report_final set DisposedQtyTillFromDate = 0 where DisposedQtyTillFromDate is null and assetid > 0 ;
		*/
        
        /*
		select assetid , companyid ,AGroupName ,BGroupName , CGroupName ,DGroupName ,   
            CAST(AssetNo AS UNSIGNED INTEGER) AssetNo ,AssetIdentificationNo ,AssetName ,
			 OpGross ,Addition ,Disposal ,ClGross ,
            OpDep ,UpToDep ,DispoDep ,TotDep ,NetBalance ,            
            DepRate ,DepMethod ,CAST(voucherDate as DATE)  voucherDate,
            VoucherNo ,PONo , CAST(DTPutUse  as DATE)  DTPutUse,Remarks ,
            Qty ,BillNo ,CAST(BillDate as DATE) BillDate,SrNo ,           
            Model ,ALocName ,BLocName ,CLocName ,
            SupplierName ,OpeningQty ,
            DisposedQtyTillFromDate 
		from  temp_report_final 
		 where CAST(AssetNo AS UNSIGNED INTEGER)  > 0
		order by CAST(AssetNo AS UNSIGNED INTEGER)
		*/
        
        select assetid ,
			companyid ,
            AGroupName ,
			BGroupName ,
			CGroupName ,
		    DGroupName ,   
			CAST(AssetNo AS UNSIGNED INTEGER) AssetNo ,
			AssetIdentificationNo ,
			AssetName ,
            Disposal ,
            DispoDep ,
            Qty ,
			SrNo ,           
            Model ,
            ALocName ,
            BLocName ,
            CLocName ,
			DisposalDate 
			from  temp_report_final 		 
		/* select * from  temp_report_final where assetid = 7881 */ ;
		 
       
END$$
DELIMITER ;

call usp_report_Disposal(1,'2020-04-01','2021-03-31')

