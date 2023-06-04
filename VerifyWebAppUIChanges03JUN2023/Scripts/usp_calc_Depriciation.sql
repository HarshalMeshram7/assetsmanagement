DELIMITER //


DROP PROCEDURE IF exists usp_calc_Depriciation //



CREATE PROCEDURE usp_calc_Depriciation(IN startdate date, IN enddate date)
BEGIN

	

	DECLARE v_dateputtouse date;
    DECLARE v_usefullife int;
    DECLARE v_deprate decimal(18,2);
    DECLARE v_assetid int;
    DECLARE v_deptype varchar(100);
    DECLARE v_assetname varchar(200);
    declare done int default 0;

    
    /*
		crete required temp tables
        
    
    */
   
   DROP TEMPORARY TABLE IF EXISTS temp_opening ;
    CREATE TEMPORARY TABLE temp_opening (
			id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
            companyid INT NOT NULL,
            assetid INT NOT NULL,
            openingassetvalue decimal(18,2),
            openingdepriciation decimal(18,2),
            deptilldate decimal(18,2)
		);
        
        /* deptilldate = deprication till from date */
		
        
	
      DROP TEMPORARY TABLE IF EXISTS temp_working_calc ;
      
     CREATE TEMPORARY TABLE  temp_working_calc (
			id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
            companyid INT NOT NULL,
            assetid INT NOT NULL,
            fromdate datetime,
            todate datetime,
            depdays int,
            deprate decimal(18,2),
            amountcapitalized decimal(18,2),
            depamt decimal(18,2),
            disposedflag int
		);
        
        
          DROP TEMPORARY TABLE IF EXISTS temp_working_disposal ;
        	CREATE TEMPORARY TABLE  temp_working_disposal (
			id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
            companyid INT NOT NULL,
            assetid INT NOT NULL,
            VoucherDate datetime 
		);
        
    /* start of system implementation */
    
 
    
    
    
    /* step 0 
		create temp table to hold opening values for the period to be calculated 
    
    */

 
     insert into temp_opening (companyid,assetid,openingassetvalue,openingdepriciation,deptilldate)
	 select 1,id,AmountCapitalisedCompany,OPAccDepreciation,0 from tblassets;
    
    
    
    /*
		take opening + additions - disposal entries
        
    */
    
	insert into temp_working_calc (companyid,assetid,fromdate,todate,depdays,deprate,amountcapitalized,depamt,disposedflag)
    select 1,id,startdate,enddate,0,totalrate,AmountCapitalisedCompany, 0,0 from tblassets
    where VoucherDate < startdate;
    
    update temp_working_calc set depdays =  datediff(todate,fromdate) + 1 where id > 0 ;
    
    update temp_working_calc set depamt =  (((amountcapitalized * deprate ) /100) * depdays / 365)
     where id > 0 ;
    
    
    
    /* reset dep amount calculation if asset id disposed between start and enddate) */
    
    
    insert into temp_working_disposal (companyid,assetid,voucherdate) 
    select 1,AssetId,voucherdate from tbldisposal;
    
    update temp_working_calc set todate=null,depdays = 0, depamt=0,disposedflag =1 where  assetid 
	in (select AssetId from tbldisposal where VoucherDate >=startdate and VoucherDate <=enddate)
	and temp_working_calc.id > 0;
        
	
    /* update disposal date with todate in temp table to calculate dep till disposal date onnly*/
    update temp_working_calc tw
    inner join temp_working_disposal twd
    on tw.assetid = twd.assetid
    set tw.todate =  twd.voucherdate
    where tw.id > 0;
    
	update temp_working_calc set depdays =  datediff(todate,fromdate) + 1 where id > 0 ;
    
    update temp_working_calc set depamt =  (((amountcapitalized * deprate ) /100) * depdays / 365)
	where id > 0 ;
    
    
   
    select * from temp_working_calc; /* temp output */
    
    
        
END //

DELIMITER ;