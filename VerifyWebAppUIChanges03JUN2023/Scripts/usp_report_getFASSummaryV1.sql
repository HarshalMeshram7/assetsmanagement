DROP PROCEDURE usp_report_getFASSummaryV1;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_report_getFASSummaryV1`(IN companyid int,IN startdate date, IN enddate date)
BEGIN

/* changes by Mandar 10 DEC 2020 */
	
declare v_StartDate datetime;
declare v_EndDate datetime;
    declare finished int default 0;
    
	declare v_agroupid int; 
    declare v_OpGross decimal(18,2);
	declare v_Addition decimal(18,2);
	declare v_Disposal decimal(18,2);
	declare v_ClGross decimal(18,2);
    declare v_OpDepAsset decimal(18,2); 
	declare v_UpToDep decimal(18,2);
	declare v_OpDep decimal(18,2);

    declare v_DepForPeriod decimal(18,2);
    
	declare v_DispoDep decimal(18,2);
	declare v_TotDep decimal(18,2);
	declare v_NetBalance decimal(18,2);
    
    
    declare v_disposalopgross decimal(18,2);
    declare v_disposalopdep decimal(18,2);
	/*declare v_disposaldepforfromdttodt decimal(18,2);*/
      
    declare v_Disposalgross decimal(18,2);
    declare v_Disposalopaccumalted decimal(18,2);
    
    
    declare v_depriciation_openingamt decimal(18,2);
    declare v_depriciation_UptoDep decimal(18,2);
    
    
    
    DEClARE curAGroup CURSOR FOR SELECT id FROM tblagroup;
    DECLARE CONTINUE HANDLER 
			FOR NOT FOUND SET finished = 1;
        
    /*
		crete required temp tables
        
    
    */
   
   DROP TEMPORARY TABLE IF EXISTS temp_report ;
    CREATE TEMPORARY TABLE temp_report (
			id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
            agropid int null,
            AGroupName varchar(2000),
            OpGross decimal(18,2)  null,
            Addition decimal(18,2) null,
            Disposal decimal(18,2) null,
            ClGross decimal(18,2) null,
            OpDepAsset decimal(18,2) null, /* at the time of system starting */
            UpToDep decimal(18,2) null, /* till from date*/
            OpDep decimal(18,2) null, /* at the time of system starting */
            DepForPeriod decimal(18,2) null, /* disposal for report period */
            DispoDep decimal(18,2) null,
            TotDep decimal(18,2) null,
            NetBalance decimal(18,2) null
		);
        
			
		
                set v_StartDate=Cast(startdate as Date);
                set v_EndDate=Cast(enddate as Date);
		insert into temp_report (agropid,AGroupName,
			OpGross , Addition ,Disposal ,ClGross ,OpDepAsset,UpToDep, OpDep ,
				 DepForPeriod,DispoDep ,TotDep ,NetBalance)
			select id, agroupname,0 OpGross,0 Addition,0 Disposal,0 ClGross, 0 OpDepAsset ,0 UpToDep,
				0 OpDep, 0  DepForPeriod ,0 DispoDep ,0 TotDep,0 NetBalance from tblagroup where companyid=companyid;

		

		
		OPEN curAGroup;
        getAGroup: LOOP
			FETCH curAGroup INTO v_agroupid;
				IF finished = 1 THEN 
					LEAVE getAGroup;
				END IF;
			
            /*Logic inside loop Start */
			/*SELECT v_agroupid; */
            
            set v_OpGross = 0;
            set v_OpGross = (select IFNULL(sum(AmountCapitalisedCompany),0) from tblassets
							where Companyid = companyid and agroupid = v_agroupid
                            and voucherdate < v_StartDate  and voucherdate <= v_EndDate );
                            
                            
			set v_Addition = 0;
            set v_Addition = (select IFNULL(sum(AmountCapitalisedCompany),0) from tblassets
							where Companyid = companyid and agroupid = v_agroupid
                            and voucherdate >= v_StartDate and voucherdate <= v_EndDate);
                            



            set v_OpDepAsset = 0;
            set v_OpDepAsset = (select IFNULL(sum(OPAccDepreciation),0) from tblassets
							where Companyid = companyid and agroupid = v_agroupid);
                            
                          


			set v_disposalopgross = 0;
            
			set v_disposalopgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal  
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                            and tbldisposal.voucherdate <= v_EndDate and tbldisposal.voucherdate < v_StartDate );
                            
		

			set v_disposalopdep = 0;
            
			set v_disposalopdep = (select IFNULL(sum(OpAccumulatedDep),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                          and tbldisposal.voucherdate<= v_EndDate  and tbldisposal.voucherdate < v_StartDate ) ;
            
	       /* dep for from date and to date */
            
		  set v_DepForPeriod  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                            and tbldepreciation.FromDate >= v_StartDate and tbldepreciation.ToDate <= v_EndDate);
                            
         
            
           
            
				set v_Disposalgross = (select IFNULL(sum(DisposalAmount),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid
                            and tblassets.agroupid = v_agroupid
                          and tbldisposal.voucherdate >= v_StartDate  and tbldisposal.voucherdate <= v_EndDate);
                          
            
          
		

			
			
        
			set v_depriciation_UptoDep  = 0;
            set v_depriciation_UptoDep  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                            and tbldepreciation.ToDate < v_EndDate);


		set v_DispoDep = 0;
		set v_DispoDep = (select IFNULL(sum(DepForFromDtToDt),0) from tbldisposal
				inner join tblassets 
				on tbldisposal.AssetId = tblassets.id
				where tbldisposal.Companyid = companyid 
				and tblassets.agroupid = v_agroupid
			  and tbldisposal.DisposalDate >= v_StartDate   and tbldisposal.DisposalDate <= v_EndDate ) ;


			set v_OpDep = 0;
             
            set v_OpDep = v_OpDepAsset + v_depriciation_UptoDep;
			set v_TotDep = 0;
            set v_TotDep =  v_OpDep + v_DepForPeriod;
            
			 set v_TotDep =  (v_TotDep - v_DispoDep);

			update temp_report set OpGross = v_OpGross where agropid  = v_agroupid and id > 0;
            update temp_report set OpDep = v_OpDep where agropid  = v_agroupid and id > 0;
            update temp_report set Addition = v_Addition where agropid  = v_agroupid and id > 0;
			update temp_report set Disposal = v_Disposalgross where agropid  = v_agroupid and id > 0;
		    update temp_report set DispoDep = v_DispoDep where agropid  = v_agroupid and id > 0;
		    update temp_report set OpDepAsset = v_OpDepAsset where agropid  = v_agroupid and id > 0;
		    update temp_report set UpToDep = v_depriciation_UptoDep where agropid  = v_agroupid and id > 0;
            update temp_report set DepForPeriod = v_DepForPeriod where agropid  = v_agroupid and id > 0;
            
            update temp_report set  OpDep = v_OpDep where agropid  = v_agroupid and id > 0;
       
            update temp_report set  TotDep  = v_TotDep where agropid  = v_agroupid and id > 0;
            
            update temp_report set ClGross = (OpGross + Addition) - Disposal where agropid  = v_agroupid and id > 0;
		
			update temp_report set  NetBalance= (clgross -TotDep)  where agropid  = v_agroupid and id > 0;
           
        /*Logic inside loop End */
        END LOOP getAGroup;
        CLOSE curAGroup;

            
        
		select * from temp_report; /* repoort output */
        
	
        
END$$
DELIMITER ;

call usp_report_getFASSummaryV1(1,'2019-04-01','2020-03-31')

