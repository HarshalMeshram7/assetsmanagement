

/*DRop PROCEDURE usp_calc_depriciationV3*/


DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_calc_depriciationV3`(IN companyid int,IN startdate date, IN enddate date,IN userid int)
BEGIN
	
    truncate table tbldepworking;
    
   insert into tbldepworking
    (	AssetID, AssetNo, FromDate, ToDate, EffFrom, EffTo, OpGross, OpDep, DisposalAmount, 
    DepForPeriod, DisposalDate, Days, DispoDep, Method, DepRate, RowType,DisposalAmtTillDate,DtPutToUse,AssetLife,ResidualValue)
    select id,assetno,startdate,enddate,startdate,enddate,0,0,0,
    0,null,0 days ,0 DispoDep,DepreciationMethod, TotalRate,'DEP',0 DisposalAmtTillDate,DtPutToUse,Usefullife,ResidualVal from tblassets ;
   
   
   /*
		insert into tbldepworking
			(	AssetID, AssetNo, FromDate, ToDate, EffFrom, EffTo, OpGross, OpDep, Amount, 
			DepForPeriod, DisposalDate, Days, DispoDep, Method, DepRate, RowType)
            
			select tbldisposal.assetid,tblassets.assetno,startdate,enddate,tbldisposal.DisposalDate,enddate,0,0,tbldisposal.DisposalAmount,
			0,tbldisposal.DisposalDate,0 days ,0 DispoDep,DepreciationMethod, TotalRate,'DISP' from tbldisposal
            inner join tblassets on tbldisposal.assetid = tblassets.id
            where tbldisposal.DisposalDate >= startdate and tbldisposal.DisposalDate <= enddate;
     */       
            
   			    update tbldepworking
				inner join tblassets on tbldepworking.assetid =  tblassets.id
                set tbldepworking.OpGross = IFNULL(AmountCapitalisedCompany,0)
                where tblassets.companyid = companyid
                and tbldepworking.id > 0;  
    
    
				/* consider  op accumalted dep from tblasets */
			    update tbldepworking
				inner join tblassets on tbldepworking.assetid =  tblassets.id
                set tbldepworking.OpDep = IFNULL(OPAccDepreciation,0)
                where tblassets.companyid = companyid
                and tbldepworking.id > 0;  
                
              
              
          
              	update tbldepworking working
                inner join (
							select assetid,IFNULL(sum(Amount),0) as depriciation_tillFromDate
                            from tbldepreciation
                            where tbldepreciation.companyid = companyid
							and  tbldepreciation.FromDate < startdate 
                            group by assetid
						) as depreciation ON
                        working.assetid = depreciation.assetid
                set working.OpDep = working.OpDep + depreciation.depriciation_tillFromDate;
                
                
                update tbldepworking working
                inner join (
							select assetid,IFNULL(sum(DisposalAmount),0) as DisposalAmount_TillDate
                            from tbldisposal
                            where tbldisposal.companyid = companyid
							and  tbldisposal.DisposalDate < startdate 
                            group by assetid
						) as disposal ON
                        working.assetid = disposal.assetid
                set working.DisposalAmtTillDate =   disposal.DisposalAmount_TillDate;
                
            
            /* insert disposal records */
            
          
            /**/
              
                
		update tbldepworking set DisposalAmtTillDate =0 where DisposalAmtTillDate is null;
   
   select * from tbldepworking    order by AssetID, RowType ASC;
    
   
       
END$$
DELIMITER ;
