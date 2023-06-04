DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_report_getFASDetail`(IN companyid int,IN startdate date, IN enddate date,
IN _agroupid int , IN _bgroupid int, IN _cgroupid int, _dgroupid int)
BEGIN


    declare finished int default 0;
    
	declare v_agroupid int; 
        
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
    declare v_disposaldepforfromdttodt decimal(18,2);
    declare v_Disposalgross decimal(18,2);
    declare v_Disposalopaccumalted decimal(18,2);
    
    
    declare v_depriciation_openingamt decimal(18,2);
    declare v_depriciation_UptoDep decimal(18,2);
    
    
    
    /*
		crete required temp tables
        
    
    */
   
      DROP TEMPORARY TABLE IF EXISTS temp_report_detail ;
    CREATE TEMPORARY TABLE temp_report_detail (
			id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
            agroupid int,
            bgroupid int,
            cgroupid int,
            dgroupid int,
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
   
        
	
                
		insert into temp_report_detail (agroupid,bgroupid,cgroupid,dgroupid,
			OpGross , Addition ,Disposal ,ClGross ,OpDep ,
				UpToDep ,DispoDep ,TotDep ,NetBalance)
			values  (_agroupid,_bgroupid,_cgroupid,_dgroupid,0,0,0,0,0,0,0,0,0);
		

	
	
            /*Logic inside loop Start */
			/*SELECT v_agroupid; */
            
            set v_OpGross = 0;
            set v_OpGross = (select IFNULL(sum(AmountCapitalisedCompany),0) from tblassets
							where Companyid = companyid 
                            and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                           and voucherdate <= startdate);
                            
			
		
                            
			set v_Addition = 0;
            set v_Addition = (select IFNULL(sum(AmountCapitalisedCompany),0) from tblassets
							where Companyid = companyid 
							and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                            and voucherdate >= startdate and voucherdate <= enddate);
                            



            set v_OpDep = 0;
            set v_OpDep = (select IFNULL(sum(OPAccDepreciation),0) from tblassets
							where Companyid = companyid 
							and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                            and  voucherdate <= startdate );


			
			set v_disposalopgross = 0;
            
			set v_disposalopgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal  
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
							and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                            and  tbldisposal.voucherdate < startdate  );
                            
		
        

			set v_disposalopdep = 0;
            
			set v_disposalopdep = (select IFNULL(sum(OpAccumulatedDep),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                          and  tbldisposal.voucherdate < startdate ) ;

		
        set v_disposaldepforfromdttodt =(select IFNULL(sum(DepForFromDtToDt),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                           and tbldisposal.voucherdate < startdate ) ;
			
                           set v_disposalopdep=v_disposalopdep+v_disposaldepforfromdttodt;
							set v_OpGross =  v_OpGross - v_disposalopgross;
							set v_OpDep =  v_OpDep -v_disposalopdep ;

			set v_Disposalgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                          and tbldisposal.voucherdate <= enddate  and tbldisposal.voucherdate >= startdate );
                      

		/*	set v_Disposalopaccumalted = (select IFNULL(sum(OpAccumulatedDep),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                           and tbldisposal.voucherdate <= enddate and tbldisposal.voucherdate >= startdate );*/


            
			set v_depriciation_openingamt  = 0;
            set v_depriciation_openingamt  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                            and tbldepreciation.FromDate < startdate and tbldepreciation.ToDate <= enddate);

			 set v_OpDep=v_OpDep+v_depriciation_openingamt;

			set v_depriciation_UptoDep  = 0;
            set v_depriciation_UptoDep  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                            and tbldepreciation.FromDate >= startdate and tbldepreciation.ToDate<= enddate);


			


			UPDATE temp_report_detail 
				SET OpGross = v_OpGross
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid AND id > 0;
                


			UPDATE temp_report_detail
			SET 
				OpDep = v_OpDep
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid  AND id > 0;

			UPDATE temp_report_detail 
			SET 
				Addition = v_Addition
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid  AND id > 0;

			UPDATE temp_report_detail 
			SET 
				Disposal = v_Disposalgross
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid  AND id > 0;
			
          /*  UPDATE temp_report_detail 
			SET 
				DispoDep = v_Disposalopaccumalted
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid  AND id > 0;*/


			UPDATE temp_report_detail 
			SET 
				UpToDep = v_depriciation_UptoDep
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid  AND id > 0;



            
			UPDATE temp_report_detail 
			SET 
				ClGross = (OpGross + Addition) - Disposal
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid AND id > 0;


			UPDATE temp_report_detail 
			SET 
				TotDep = (OpDep + UpToDep) 
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid  AND id > 0;

			UPDATE temp_report_detail 
			SET 
				NetBalance = (clgross - TotDep)
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid  AND id > 0;
					   
      
            
        select * from temp_report_detail;	/*final output*/
        
END$$
DELIMITER ;
