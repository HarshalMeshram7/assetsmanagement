

DELIMITER //


DROP PROCEDURE IF exists usp_calc_depriciationNewLogic //



CREATE PROCEDURE usp_calc_depriciationNewLogic(IN companyid int,IN startdate date, IN enddate date)
BEGIN


	declare _daysdiff int ;
    

	set _daysdiff =  datediff(enddate,startdate) +1;
    


	DROP TEMPORARY TABLE IF EXISTS temp_dep_calc ;
    CREATE TEMPORARY TABLE temp_dep_calc (
			assetid INT NOT NULL PRIMARY KEY,
            op_gross_block decimal(18,2)  null default 0,  /* opening gross block */
            op_accu_dep decimal(18,2)  null default 0,	/* opening accumalted depriciation */
            asset_life_days int  null,	
            dt_put_to_use date null,	
            life_left_till_stdate date null,	
            dep_days int null,	 /* no of days for which dep is to be calculated */
            dep_type varchar(100) null,	 
            dep_rate decimal(18,2)  null default 0,	/* dep rate  */
            disp_gross_block decimal(18,2)  null default 0,	/* dep rate  */
            dep_on_disp_st_dt_to_sale_dt decimal(18,2)  null,	/* dep on disposal from st date to sale date */
            disp_type varchar(25)  null,	/* FULL / PARTIAL */
            dep_rev_on_disposal varchar(25)  null,	/* dep amt reversed on disposal */
            residual_value decimal(18,2)  null default 0,	/* dep amt reversed on disposal */
            dep_till_startdt decimal(18,2)  null default 0,	/* dep amt till start date */
            dep_on_disposal_sale_dt_to_end_dt decimal(18,2)  null default 0,	/* dep amt on disposal */
            amt_for_dep_calc decimal(18,2)  null default 0,	
            net_block_stdt decimal(18,2)  null default 0, /* net block n st date */ 
            dep_for_period decimal(18,2)  null default 0,
            net_block_endt decimal(18,2)  null default 0/* net block n st date */ 
            
		);



	DROP TEMPORARY TABLE IF EXISTS temp_disposal;
    CREATE TEMPORARY TABLE temp_disposal (
			assetid INT NOT NULL PRIMARY KEY,
            disposaldate date NOT NULL,
            disposalid int NOT NULL,
            disp_gross_block decimal(18,2)  null default 0,	/* dep rate  */
            dep_on_disp_st_dt_to_sale_dt decimal(18,2)  null,	/* dep on disposal from st date to sale date */
            disp_type varchar(25)  null,	/* FULL / PARTIAL */
            dep_rev_on_disposal varchar(25)  null,	/* dep amt reversed on disposal */
            dep_on_disposal_sale_dt_to_end_dt decimal(18,2)  null default 0	/* dep amt on disposal */
          
		);
   

		insert into temp_dep_calc ( assetid,op_gross_block,op_accu_dep,dt_put_to_use,dep_type,dep_rate,residual_value)
        select id,grossval,OPAccDepreciation,DtPutToUse,DepreciationMethod,TotalRate,ResidualVal from tblassets;
        
	
		insert into temp_disposal (assetid,disposaldate,disposalid,disp_gross_block,
				dep_on_disp_st_dt_to_sale_dt,disp_type,dep_rev_on_disposal,dep_on_disposal_sale_dt_to_end_dt)
		select Assetid,DisposalDate,id,GrossAmount,0,DisposalType,0,0 from tbldisposal;
    


		update temp_dep_calc working
			inner join (
						select assetid,IFNULL(sum(Amount),0) as depriciation_openingamt
						from tbldepreciation
						where tbldepreciation.companyid = companyid
						and  tbldepreciation.ToDate <= startdate
						group by assetid
					) as depreciation ON
					working.assetid = depreciation.assetid
			set working.dep_till_startdt = depreciation.depriciation_openingamt
            where working.assetid > 0;

		/*select * from temp_dep_calc*/
        
		/* START CALULATE for disposal deprication */
    
		/* START FULL disposal before start date */
		
        update temp_disposal disp 
			inner join temp_dep_calc working
            on disp.assetid = working.assetid
		set disp.disp_gross_block = working.op_gross_block , disp.dep_on_disp_st_dt_to_sale_dt = 0
		where disp.disposaldate < startdate and disp.disp_type = 'Full' 
        and disp.assetid  > 0;
        

        update temp_disposal disp 
			inner join temp_dep_calc working
            on disp.assetid = working.assetid
		set  disp.dep_rev_on_disposal =  working.op_accu_dep + working.dep_till_startdt
		where disp.disposaldate < startdate and disp.disp_type = 'Full'
        and disp.assetid  > 0;
        
        /* END FULL disposal before start date  */



    
		/* START Section PARTIAL disposal before start date */


        update temp_disposal tempdisp 
			inner join tbldisposal disposal
            on tempdisp.assetid = disposal.assetid
		set tempdisp.dep_rev_on_disposal = disposal.TotalDepreciation , tempdisp.dep_on_disp_st_dt_to_sale_dt = 0
		where disposal.disposaldate < startdate and tempdisp.disp_type = 'partial' 
        and tempdisp.assetid  > 0;


        
        /* End Section  PARTIAL disposal before start date */


	

		/* END CALULATE for disposal deprication */           
            
    
    
        
        
        /* disposal logic */
        
        
        update temp_dep_calc set dep_on_disp_st_dt_to_sale_dt = 0 where assetid > 0;
        update temp_dep_calc set dep_on_disposal_sale_dt_to_end_dt = 0 where assetid > 0;
        
        
	  
		UPDATE temp_dep_calc
				set amt_for_dep_calc = (op_gross_block - disp_gross_block - residual_value),
                dep_days = _daysdiff  /* this needs to be changed later*/
                where assetid  > 0;

	


		/* calculate depri */
		update temp_dep_calc set dep_for_period =  (( amt_for_dep_calc * dep_rate /100 ) * dep_days / 365) + ( dep_on_disp_st_dt_to_sale_dt - dep_on_disposal_sale_dt_to_end_dt)  
        where assetid > 0;
        
        
		select * from temp_disposal;
        
        

		
END //

DELIMITER ;