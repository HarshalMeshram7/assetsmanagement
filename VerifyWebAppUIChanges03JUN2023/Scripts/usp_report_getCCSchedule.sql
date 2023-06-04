
DELIMITER //


DROP PROCEDURE IF exists usp_report_getCCSchedule //

CREATE PROCEDURE `usp_report_getCCSchedule`(IN companyid int,IN startdate date, IN enddate date)
BEGIN


 declare finished int default 0;
    
	declare v_assetid int; 
    declare v_OpGross decimal(18,2);
	declare v_Addition decimal(18,2);
	declare v_Disposal decimal(18,2);
	declare v_ClGross decimal(18,2);
	declare v_OpDep decimal(18,2);
	declare v_UpToDep decimal(18,2);
	declare v_DispoDep decimal(18,2);
	declare v_TotDep decimal(18,2);
	declare v_NetBalance decimal(18,2);
 
    declare v_disposalopgross decimal(18,2);
    declare v_disposalopdep decimal(18,2);
    declare v_Disposalgross decimal(18,2);
    declare v_Disposalopaccumalted decimal(18,2);
    
    
    declare v_depriciation_openingamt decimal(18,2);
    declare v_depriciation_UptoDep decimal(18,2);
    
    
    
    /*DEClARE curAsset CURSOR FOR SELECT id FROM tblassets  where companyid=companyid and disposalflag=0 and id=18245;*/
    
    DEClARE curAsset CURSOR FOR SELECT id FROM tblassets  where companyid=companyid and disposalflag=0 ;
    DECLARE CONTINUE HANDLER 
			FOR NOT FOUND SET finished = 1;
        
    /*
		crete required temp tables
        
    
    */
   
   DROP TEMPORARY TABLE IF EXISTS temp_report ;
    CREATE TEMPORARY TABLE temp_report (
		
        
			assetid  INT NOT NULL PRIMARY KEY,
			companyid INT NOT NULL,
            AGroupName text,
			BGroupName text,
			CGroupName text,
		    DGroupName text,            
			AssetIdentification text,
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
            CCCode  text,
            CCDescription text
            
    
            
		);
        
			
		
                
		insert into temp_report (companyid,assetid,AGroupName,BGroupName,CGroupName,DGroupName,AssetIdentification,AssetName,
			OpGross , Addition ,Disposal ,ClGross ,OpDep ,
				UpToDep ,DispoDep ,TotDep ,NetBalance,DepRate,DepMethod,voucherDate,VoucherNo,PONo,DTPutUse,
                Remarks,Qty,BillNo,BillDate,SrNo,Model,ALocName,BLocName,CLocName,SupplierName,CCCode,CCDescription)
			select companyid,ID,null agroupname,null BGroupName,null CGroupName,null DGroupName,AssetIdentificationno,AssetName,0 OpGross,0 Addition,0 Disposal,0 ClGross,0 OpDep,
				0 UpToDep,0 DispoDep ,0 TotDep,0 NetBalance,0 DepRate,DepreciationMethod,voucherDate,VoucherNo,PONo,DTPuttoUse,Remarks,
                 Qty,BillNo,BillDate,SrNo,Model,null ALocName,null BLocName,null CLocName,null SupplierName,
                CostCenterAID,null CCDescription from tblassets;

		

		
		OPEN curAsset;
        getAsset: LOOP
			FETCH curAsset INTO v_assetid;
				IF finished = 1 THEN 
					LEAVE getAsset;
				END IF;
			
            /*Logic inside loop Start */
			/*SELECT v_agroupid; */
            
            
            
            set v_OpGross = 0;
            set v_OpGross = (select IFNULL(AmountCapitalisedCompany,0) from tblassets
							where Companyid = companyid and id = v_assetid
                            and voucherdate < startdate  and voucherdate <= enddate);
                            
                            
			set v_Addition = 0;
            set v_Addition = (select IFNULL(AmountCapitalisedCompany,0) from tblassets
							where Companyid = companyid and id = v_assetid
                            and voucherdate >= startdate and voucherdate <= enddate);
                            

			
            set v_OpDep = 0;
            set v_OpDep = (select IFNULL(OPAccDepreciation,0) from tblassets
							where Companyid = companyid and id = v_assetid
                            and voucherdate <= enddate and voucherdate <= startdate );



			set v_disposalopgross = 0;
            
			set v_disposalopgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal  
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tbldisposal.AssetId= v_assetid
                            and tbldisposal.voucherdate <= enddate and tbldisposal.voucherdate < startdate );
                            
		

			set v_disposalopdep = 0;
            
			set v_disposalopdep = (select IFNULL(sum(OpAccumulatedDep),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tbldisposal.AssetId = v_assetid
                          and tbldisposal.voucherdate<= enddate  and tbldisposal.voucherdate < startdate ) ;

		
	
							set v_OpGross =  v_OpGross - v_disposalopgross;
							set v_OpDep =  v_OpDep - v_disposalopdep;

			set v_Disposalgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.id = v_assetid
                          and tbldisposal.voucherdate <= enddate  and tbldisposal.voucherdate >= startdate );

			set v_Disposalopaccumalted = (select IFNULL(sum(OpAccumulatedDep),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tbldisposal.AssetId  = v_assetid
                           and tbldisposal.voucherdate <= enddate and tbldisposal.voucherdate >= startdate );

			
			
            
			set v_depriciation_openingamt  = 0;
            set v_depriciation_openingamt  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tbldepreciation.AssetId = v_assetid
                            and tbldepreciation.FromDate < startdate and tbldepreciation.ToDate <= enddate);

			 set v_OpDep=v_OpDep+v_depriciation_openingamt;

			set v_depriciation_UptoDep  = 0;
            set v_depriciation_UptoDep  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tbldepreciation.AssetId  = v_assetid
                            and tbldepreciation.FromDate >= startdate and tbldepreciation.ToDate<= enddate);



			update temp_report set OpGross = v_OpGross where assetid  = v_assetid ;
            update temp_report set OpDep = v_OpDep where assetid  = v_assetid ; 
            update temp_report set Addition = v_Addition where assetid  = v_assetid;  
			update temp_report set Disposal = v_Disposalgross where assetid  = v_assetid; 
			update temp_report set DispoDep = v_Disposalopaccumalted where assetid  = v_assetid; 
            update temp_report set UpToDep = v_depriciation_UptoDep where assetid  = v_assetid;
            update temp_report set clgross = (OpGross + Addition) - Disposal where assetid  = v_assetid;
			update temp_report set  TotDep= (OpDep + UpToDep) - DispoDep where assetid  = v_assetid ;
			update temp_report set  NetBalance= (clgross -TotDep)  where assetid  = v_assetid ;
           
            
		
        /*Logic inside loop End */
        END LOOP getAsset;
        CLOSE curAsset;

            
        
		select * from temp_report; /* repoort output */
        
	
    
    
    
        
END //

DELIMITER ;