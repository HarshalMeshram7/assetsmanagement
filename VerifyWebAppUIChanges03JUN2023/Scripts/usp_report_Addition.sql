
DROP PROCEDURE IF  EXISTS usp_report_Addition;
DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_report_Addition`(IN companyid int,IN startdate date, IN enddate date)
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
		
        
			assetid  INT NOT NULL PRIMARY KEY,
			companyid INT NOT NULL,
            AGroupName text,
			BGroupName text,
			CGroupName text,
		    DGroupName text,   
             AssetNo text,
			AssetIdentificationNo text,
			AssetName text,
            OpGross decimal(18,2),
            Addition decimal(18,2),
            Disposal decimal(18,2),
            ClGross decimal(18,2),
            OpDep decimal(18,2),
            UpToDep decimal(18,2),
            DispoDep decimal(18,2),
            TotDep decimal(18,2),
            NetBalance decimal(18,2),            
            DepRate decimal(18,2),
            DepMethod text,
            voucherDate  datetime,
           
            VoucherNo text,
            PONo text,
            DTPutUse datetime,
            Remarks text,
            Qty INT,
            BillNo text,
            BillDate datetime,
            SrNo text,           
            Model text,
            ALocName text,
            BLocName text,
            CLocName text,
            SupplierName text,
            OpeningQty integer,
            DisposedQtyTillFromDate integer
            
            
		);
        
	
                
		insert into temp_report_final (companyid,assetid,AGroupName,BGroupName,CGroupName,DGroupName,AssetNo,AssetIdentificationNo,AssetName,
			OpGross , Addition ,Disposal ,ClGross ,OpDep ,
				UpToDep ,DispoDep ,TotDep ,NetBalance,DepRate,DepMethod,voucherDate,VoucherNo,PONo,DTPutUse,
                Remarks,Qty,BillNo,BillDate,SrNo,Model,ALocName,BLocName,CLocName,SupplierName,OpeningQty)
			select companyid,ID,null agroupname,null BGroupName,null CGroupName,null DGroupName,AssetNo,AssetIdentificationNo,AssetName,0 OpGross,0 Addition,0 Disposal,0 ClGross,0 OpDep,
				0 UpToDep,0 DispoDep ,0 TotDep,0 NetBalance,TotalRate,DepreciationMethod,voucherDate,VoucherNo,PONo,DTPuttoUse,Remarks,
                 Qty,BillNo,BillDate,SrNo,Model,null ALocName,null BLocName,null CLocName,null SupplierName,Qty
                 from tblassets where companyid=companyid and  voucherdate  >= startdate and voucherdate <= enddate;
		

        
		        update temp_report_final
				inner join tblassets on temp_report_final.assetid =  tblassets.id
                set temp_report_final.Addition = IFNULL(AmountCapitalisedCompany,0)
                where tblassets.companyid = companyid
                and  tblassets.voucherdate >= startdate  and tblassets.voucherdate <= enddate
                and id > 0;
                
                
				update temp_report_final working
                inner join (
							select assetid,IFNULL(sum(Amount),0) as TotalDep
                            from tbldepreciation
                            where tbldepreciation.companyid = companyid
							and  tbldepreciation.FromDate >= startdate and tbldepreciation.ToDate <= enddate
                            group by assetid
						) as depreciation ON
                        working.assetid = depreciation.assetid
                set working.TotDep = depreciation.TotalDep;


		
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
                
                
                 update temp_report_final 
				set temp_report_final.SupplierName = (select (A.SupplierName) from tblsupplier A inner join tblassets D on A.id=D.SupplierNo where D.id=assetid) 
                where assetid > 0; 
        

		update temp_report_final set DisposedQtyTillFromDate = 0 where DisposedQtyTillFromDate is null and assetid > 0 ;

        
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
		
				 
		/* select * from  temp_report_final where assetid = 7881 */ ;
		 
       
END$$
DELIMITER ;

