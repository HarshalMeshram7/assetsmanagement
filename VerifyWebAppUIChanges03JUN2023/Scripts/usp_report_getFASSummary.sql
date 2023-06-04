DELIMITER //


DROP PROCEDURE IF exists usp_report_getFASDetail //



CREATE PROCEDURE usp_report_getFASDetail(IN companyid int,IN startdate date, IN enddate date)
BEGIN


    declare finished int default 0;
    
	declare v_agroupid int; 
    declare v_bgroupid int; 
    
    
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
    
    
    
    DEClARE curAGroup CURSOR FOR SELECT id FROM tblagroup;
    DEClARE curBGroup CURSOR FOR SELECT id FROM tblbgroup;
    
    DECLARE CONTINUE HANDLER 
			FOR NOT FOUND SET finished = 1;
        
    /*
		crete required temp tables
        
    
    */
   
      DROP TEMPORARY TABLE IF EXISTS temp_report_detail ;
    CREATE TEMPORARY TABLE temp_report_detail (
			id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
            grouptype varchar(100),
            gropid int,
            GroupName varchar(2000),
            OpGross decimal(18,2)  null,
            Addition decimal(18,2) null,
            Disposal decimal(18,2) null,
            ClGross decimal(18,2) null,
            OpDep decimal(18,2) null,
            UpToDep decimal(18,2) null,
            DispoDep decimal(18,2) null,
            TotDep decimal(18,2) null,
            NetBalance decimal(18,2) null
		);
   
   DROP TEMPORARY TABLE IF EXISTS temp_report_agroup ;
    CREATE TEMPORARY TABLE temp_report_agroup (
			id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
            agropid int,
            AGroupName varchar(2000),
            OpGross decimal(18,2)  null,
            Addition decimal(18,2) null,
            Disposal decimal(18,2) null,
            ClGross decimal(18,2) null,
            OpDep decimal(18,2) null,
            UpToDep decimal(18,2) null,
            DispoDep decimal(18,2) null,
            TotDep decimal(18,2) null,
            NetBalance decimal(18,2) null
		);
        
	
    /* temp table to hold calculation detail for b group level */
    DROP TEMPORARY TABLE IF EXISTS temp_report_bgroup ;
    CREATE TEMPORARY TABLE temp_report_bgroup (
			id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
            bgropid int,
            bGroupName varchar(2000),
            OpGross decimal(18,2)  null,
            Addition decimal(18,2) null,
            Disposal decimal(18,2) null,
            ClGross decimal(18,2) null,
            OpDep decimal(18,2) null,
            UpToDep decimal(18,2) null,
            DispoDep decimal(18,2) null,
            TotDep decimal(18,2) null,
            NetBalance decimal(18,2) null
		);
   
		
                
		insert into temp_report_agroup (agropid,AGroupName,
			OpGross , Addition ,Disposal ,ClGross ,OpDep ,
				UpToDep ,DispoDep ,TotDep ,NetBalance)
			select id, agroupname,0 OpGross,0 Addition,0 Disposal,0 ClGross,0 OpDep,
				0 UpToDep,0 DispoDep ,0 TotDep,0 NetBalance from tblagroup;

		
		insert into temp_report_bgroup (bgropid,bGroupName,
			OpGross , Addition ,Disposal ,ClGross ,OpDep ,
				UpToDep ,DispoDep ,TotDep ,NetBalance)
			select id, bgroupname,0 OpGross,0 Addition,0 Disposal,0 ClGross,0 OpDep,
				0 UpToDep,0 DispoDep ,0 TotDep,0 NetBalance from tblbgroup;

		


		
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
                            and voucherdate < startdate  and voucherdate <= enddate);
                            
                            
			set v_Addition = 0;
            set v_Addition = (select IFNULL(sum(AmountCapitalisedCompany),0) from tblassets
							where Companyid = companyid and agroupid = v_agroupid
                            and voucherdate >= startdate and voucherdate <= enddate);
                            



            set v_OpDep = 0;
            set v_OpDep = (select IFNULL(sum(OPAccDepreciation),0) from tblassets
							where Companyid = companyid and agroupid = v_agroupid
                            and voucherdate <= enddate and voucherdate <= startdate );



			set v_disposalopgross = 0;
            
			set v_disposalopgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal  
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                            and tbldisposal.voucherdate <= enddate and tbldisposal.voucherdate < startdate );
                            
		

			set v_disposalopdep = 0;
            
			set v_disposalopdep = (select IFNULL(sum(OpAccumulatedDep),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                          and tbldisposal.voucherdate<= enddate  and tbldisposal.voucherdate < startdate ) ;

		
	
							set v_OpGross =  v_OpGross - v_disposalopgross;
							set v_OpDep =  v_OpDep - v_disposalopdep;

			set v_Disposalgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                          and tbldisposal.voucherdate <= enddate  and tbldisposal.voucherdate >= startdate );

			set v_Disposalopaccumalted = (select IFNULL(sum(OpAccumulatedDep),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                           and tbldisposal.voucherdate <= enddate and tbldisposal.voucherdate >= startdate );

			
			
            
			set v_depriciation_openingamt  = 0;
            set v_depriciation_openingamt  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                            and tbldepreciation.FromDate < startdate and tbldepreciation.ToDate <= enddate);

			 set v_OpDep=v_OpDep+v_depriciation_openingamt;

			set v_depriciation_UptoDep  = 0;
            set v_depriciation_UptoDep  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                            and tbldepreciation.FromDate >= startdate and tbldepreciation.ToDate<= enddate);



			UPDATE temp_report_agroup 
				SET OpGross = v_OpGross
			WHERE
				agropid = v_agroupid AND id > 0;

			UPDATE temp_report_agroup 
			SET 
				OpDep = v_OpDep
			WHERE
				agropid = v_agroupid AND id > 0;

			UPDATE temp_report_agroup 
			SET 
				Addition = v_Addition
			WHERE
				agropid = v_agroupid AND id > 0;

			UPDATE temp_report_agroup 
			SET 
				Disposal = v_Disposalgross
			WHERE
			agropid = v_agroupid AND id > 0;
			
            UPDATE temp_report_agroup 
			SET 
				DispoDep = v_Disposalopaccumalted
			WHERE
				agropid = v_agroupid AND id > 0;


			UPDATE temp_report_agroup 
			SET 
				UpToDep = v_depriciation_UptoDep
			WHERE
				agropid = v_agroupid AND id > 0;
            
			UPDATE temp_report_agroup 
			SET 
				ClGross = (OpGross + Addition) - Disposal
			WHERE
				agropid = v_agroupid AND id > 0;


			UPDATE temp_report_agroup 
			SET 
				TotDep = (OpDep + UpToDep) - DispoDep
			WHERE
				agropid = v_agroupid AND id > 0;

			UPDATE temp_report_agroup 
			SET 
				NetBalance = (clgross - TotDep)
			WHERE
				agropid = v_agroupid AND id > 0;
					   
            
		
        /*Logic inside loop End */
        END LOOP getAGroup;
        CLOSE curAGroup;

		/* SELECT   * FROM    temp_report_agroup; */ /* repoort output */
            
        
        

		SET finished = 0; /* reset cursor status */
		SET v_OpGross = 0;
		SET  v_Addition = 0;
		SET v_Disposal =0;
		SET v_ClGross = 0 ;
		SET v_OpDep = 0;
		SET v_UpToDep = 0 ;
		SET v_DispoDep = 0;
		SET v_TotDep = 0;
		SET v_NetBalance =0 ;
    
    
		SET v_disposalopgross = 0 ;
		SET v_disposalopdep = 0 ;
		SET v_Disposalgross =0;
		SET v_Disposalopaccumalted = 0 ;
    
    
		SET v_depriciation_openingamt = 0 ;
		SET v_depriciation_UptoDep = 0 ;
    
    

		OPEN curBGroup;
        getBGroup: LOOP
			FETCH curBGroup INTO v_bgroupid;
				IF finished = 1 THEN 
					LEAVE getBGroup;
				END IF;
			
            
            
        
           set v_OpGross = 0;
            set v_OpGross = (select IFNULL(sum(AmountCapitalisedCompany),0) from tblassets
							where Companyid = companyid and bgroupid = v_bgroupid
                            and voucherdate < startdate  and voucherdate <= enddate);
                            
                            
			set v_Addition = 0;
            set v_Addition = (select IFNULL(sum(AmountCapitalisedCompany),0) from tblassets
							where Companyid = companyid and bgroupid = v_bgroupid
                            and voucherdate >= startdate and voucherdate <= enddate);
                            



            set v_OpDep = 0;
            set v_OpDep = (select IFNULL(sum(OPAccDepreciation),0) from tblassets
							where Companyid = companyid and bgroupid = v_bgroupid
                            and voucherdate <= enddate and voucherdate <= startdate );



			set v_disposalopgross = 0;
            
			set v_disposalopgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal  
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.bgroupid = v_bgroupid
                            and tbldisposal.voucherdate <= enddate and tbldisposal.voucherdate < startdate );
                            
		

			set v_disposalopdep = 0;
            
			set v_disposalopdep = (select IFNULL(sum(OpAccumulatedDep),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.bgroupid = v_bgroupid
                          and tbldisposal.voucherdate<= enddate  and tbldisposal.voucherdate < startdate ) ;

		
	
							set v_OpGross =  v_OpGross - v_disposalopgross;
							set v_OpDep =  v_OpDep - v_disposalopdep;

			set v_Disposalgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.bgroupid = v_bgroupid
                          and tbldisposal.voucherdate <= enddate  and tbldisposal.voucherdate >= startdate );

			set v_Disposalopaccumalted = (select IFNULL(sum(OpAccumulatedDep),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.bgroupid = v_bgroupid
                           and tbldisposal.voucherdate <= enddate and tbldisposal.voucherdate >= startdate );

			
			
            
			set v_depriciation_openingamt  = 0;
            set v_depriciation_openingamt  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tblassets.bgroupid = v_bgroupid
                            and tbldepreciation.FromDate < startdate and tbldepreciation.ToDate <= enddate);

			 set v_OpDep=v_OpDep+v_depriciation_openingamt;

			set v_depriciation_UptoDep  = 0;
            set v_depriciation_UptoDep  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tblassets.bgroupid = v_bgroupid
                            and tbldepreciation.FromDate >= startdate and tbldepreciation.ToDate<= enddate);



        
			UPDATE temp_report_bgroup SET OpGross = v_OpGross WHERE
						bgropid = v_bgroupid AND id > 0;

			UPDATE temp_report_bgroup SET OpDep = v_OpDep
			WHERE bgropid = v_bgroupid AND id > 0;

			UPDATE temp_report_bgroup  SET Addition = v_Addition
			WHERE bgropid = v_bgroupid AND id > 0;

			UPDATE temp_report_bgroup 
			SET Disposal = v_Disposalgross
			WHERE bgropid = v_bgroupid AND id > 0;
			
            UPDATE temp_report_bgroup 
			SET  DispoDep = v_Disposalopaccumalted
			WHERE bgropid = v_bgroupid AND id > 0;


			UPDATE temp_report_bgroup 
			SET  UpToDep = v_depriciation_UptoDep
			WHERE bgropid = v_bgroupid AND id > 0;
            
			UPDATE temp_report_bgroup 
			SET ClGross = (OpGross + Addition) - Disposal
			WHERE
				bgropid = v_bgroupid AND id > 0;


			UPDATE temp_report_bgroup 
			SET TotDep = (OpDep + UpToDep) - DispoDep
			WHERE bgropid = v_bgroupid AND id > 0;

			UPDATE temp_report_bgroup 
			SET NetBalance = (clgross - TotDep)
			WHERE bgropid = v_bgroupid AND id > 0;
					   
        
                
		
        /*Logic inside loop End */
        END LOOP getBGroup;
        CLOSE curBGroup;
        
        
        /*select * from temp_report_bgroup;*/
        
        /*-------------   bgroup   */
        
        
        
        
        
        insert into temp_report_detail	
        (   grouptype ,
            gropid ,
            GroupName ,
            OpGross ,
            Addition ,
            Disposal ,
            ClGross ,
            OpDep ,
            UpToDep ,
            DispoDep ,
            TotDep ,
            NetBalance 
		) select 'AGROUP',
			agropid ,
            AGroupName ,
            OpGross ,
            Addition ,
            Disposal ,
            ClGross ,
            OpDep ,
            UpToDep ,
            DispoDep ,
            TotDep ,
            NetBalance 	
		 FROM temp_report_agroup;
        
        
        insert into temp_report_detail	
        (   grouptype ,
            gropid ,
            GroupName ,
            OpGross ,
            Addition ,
            Disposal ,
            ClGross ,
            OpDep ,
            UpToDep ,
            DispoDep ,
            TotDep ,
            NetBalance 
		) select 'BGROUP',
			bgropid ,
            bGroupName ,
            OpGross ,
            Addition ,
            Disposal ,
            ClGross ,
            OpDep ,
            UpToDep ,
            DispoDep ,
            TotDep ,
            NetBalance 	
		FROM temp_report_bgroup;
        
        
        /*select * from temp_report_agroup;	final output*/
        select * from temp_report_detail;	/*final output*/
        
END //

DELIMITER ;