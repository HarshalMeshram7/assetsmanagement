
 DROP PROCEDURE IF exists usp_calc_depriciationV1; //



DELIMITER //






CREATE PROCEDURE usp_calc_depriciationV1(IN companyid int,IN startdate date, IN enddate date)
BEGIN

	
    declare days_in_year int;
    
    set days_in_year = 365;
    
    truncate table tbldepreciationcalculation;
    
    
    
    /* ignore rows which are disposed of fully */
    insert into tbldepreciationcalculation
    (AssetId,OpeningGross,OpeningAccumalatedDep,DepRate,DepType,ResidualValue,AssetExpiryDate,FromDate,ToDate)
    select id,grossval,OPAccDepreciation,TotalRate,DepreciationMethod,ResidualVal, ExpiryDate,startdate,enddate
    from tblassets where id  not in (
					select assetid from tbldisposal
					where tbldisposal.VoucherDate < startdate  and tbldisposal.DisposalType ='Full'
			);
    
    
    

	update tbldepreciationcalculation
    set DisposalType= 'None'
    where AssetID not in (select assetid from
			tbldisposal )
	and ID > 0;


	/*
	update tbldepreciationcalculation
    set DisposalType= 'Calculate'
    where AssetID in (select assetid from
			tbldisposal )
	and ID > 0;
	*/
    
    /* delete assets with disposed records */
    delete from tbldepreciationcalculation 
    where DisposalType != 'None'
    and id > 0;
    

	update tbldepreciationcalculation
    set ToDate= AssetExpiryDate
    where ID > 0 and (AssetExpiryDate > startdate and AssetExpiryDate < enddate);


	update tbldepreciationcalculation
    set NoOfDays= datediff(ToDate,FromDate) +1
    where ID > 0;

	
		update tbldepreciationcalculation set AssetAmt =  (OpeningGross - DisposedTillFromDate)
        where id > 0;


		update tbldepreciationcalculation set DepreciationAmt =  ((AssetAmt * DepRate / 100) / days_in_year) * NoOfDays
        where id > 0 and DisposalType = 'None';


		/* TO Do Save to Depreciation table */
		
	
		select * from tbldepreciationcalculation ;
        
        
    
    
END//

DELIMITER ;