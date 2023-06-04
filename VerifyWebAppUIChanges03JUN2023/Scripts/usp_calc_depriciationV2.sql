DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_calc_depriciationV2`(IN companyid int,IN startdate date, IN enddate date,IN userid int)
BEGIN
	
    
    declare days_in_year int;
	declare v_noofdays int;
     
    set days_in_year = 365;
    
    truncate table tbldepreciationcalculation;
    truncate table tbldepreciation_log;

    update tbldepreciationrequest set startdatetime = current_timestamp(), InProcess = 1
    where InProcess = -1
	and id > 0;
    
    /* ignore rows which are disposed of fully */
    /* Mandar 02-NOV-2020 use AmountCapitalized instead of OpeningGross*/
    /* changed logic to match asset count with clarion */
    /*
    insert into tbldepreciationcalculation
    (AssetId,AssetName,OpeningGross,OpeningAccumalatedDep,DepRate,DepType,ResidualValue,
    AssetExpiryDate,FromDate,ToDate,companyid,NormalRate,AdditionRate,DepMethod,Assetdtputuse,Usefullife)
    select id,AssetName,AmountCapitalised,OPAccDepreciation,TotalRate,'A',ResidualVal, ExpiryDate,startdate,
    enddate,companyid,Normalratae,AdditionalRate,DepreciationMethod,DtPutToUse,Usefullife
    from tblassets where id  not in (
					select assetid from tbldisposal
					where tbldisposal.VoucherDate < startdate  and tbldisposal.DisposalType ='Full'
			)
	and tblassets.DtPutToUse <= enddate;
    */
    
   insert into tbldepreciationcalculation
    (AssetId,AssetName,OpeningGross,OpeningAccumalatedDep,DepRate,DepType,ResidualValue,
    AssetExpiryDate,FromDate,ToDate,companyid,NormalRate,AdditionRate,DepMethod,Assetdtputuse,Usefullife,dep_rev_disposal)
    select id,AssetName,AmountCapitalised,OPAccDepreciation,TotalRate,'A',ResidualVal, ExpiryDate,startdate,
    enddate,companyid,Normalratae,AdditionalRate,DepreciationMethod,DtPutToUse,Usefullife,0
    from tblassets
	where  tblassets.DtPutToUse <= enddate;
    
    
   	update tbldepreciationcalculation
    set NoOfDays= 0
    where ID > 0;
    
        
      
        
		update tbldepreciationcalculation working
			inner join (
						select assetid,IFNULL(sum(DisposalAmount),0) as disposal_gross
					    from tbldisposal
                        where tbldisposal.companyid = companyid
						and  tbldisposal.disposaldate < startdate
						group by assetid
					) as disposal ON
					working.assetid = disposal.assetid
			set working.disp_gross_block = disposal.disposal_gross
            where working.id > 0;

	/*
			update tbldepreciationcalculation working
			inner join (
						select assetid,IFNULL(sum(TotalDepreciation),0) as disposal_dep
					    from tbldisposal
                        where tbldisposal.companyid = companyid
                        and  tbldisposal.disposaldate < startdate
						group by assetid
					) as disposal ON
					working.assetid = disposal.assetid
			set working.dep_rev_on_disposal = disposal.disposal_dep
            where working.id > 0;

-- Mandar 25 FEB 2021 Galleghar 
*/
    
		/*
			update tbldepreciationcalculation working
			inner join (
						select assetid,IFNULL(sum(Amount),0) as depriciation_openingamt
						from tbldepreciation
						where tbldepreciation.companyid = companyid
						and  tbldepreciation.ToDate < startdate
						group by assetid
					) as disposal ON
					working.assetid = disposal.assetid
			set working.dep_rev_on_disposal = disposal.disposal_dep
            where working.id > 0;
	*/

    
		update tbldepreciationcalculation working
			inner join (
						select assetid,IFNULL(sum(Amount),0) as depriciation_openingamt
						from tbldepreciation
						where tbldepreciation.companyid = companyid
						and  tbldepreciation.ToDate < startdate
						group by assetid
					) as depreciation ON
					working.assetid = depreciation.assetid
			set working.dep_till_startdt = depreciation.depriciation_openingamt
            where working.id > 0;


	/*dep_rev_disposal*/


	update tbldepreciationcalculation working
			inner join (
						select assetid,IFNULL(sum(OpAccumulatedDep),0) as OpAccumulatedDep
						from tbldisposal
						where tbldisposal.companyid = companyid
						and  tbldisposal.DisposalDate < startdate
						group by assetid
					) as tbldisposal ON
					working.assetid = tbldisposal.assetid
			set working.dep_rev_disposal = tbldisposal.OpAccumulatedDep
            where working.id > 0;



    
    select * from tbldepreciationcalculation;
    
    
END$$
DELIMITER ;
