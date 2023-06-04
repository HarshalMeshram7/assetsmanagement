DROP PROCEDURE IF EXISTS usp_ITDepreciationCalc;
DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_ITDepreciationCalc`(IN _companyid int,IN startdate date, IN enddate date,
cutoffdate date)
BEGIN




	DROP TEMPORARY TABLE IF EXISTS temp_report_detail ;
	CREATE TEMPORARY TABLE temp_report_detail (
			id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
            itgroupid int,
            groupname varchar(800) DEFAULT NULL,
			FromDate datetime DEFAULT NULL,
			ToDate datetime DEFAULT NULL,
            DepreciationRate decimal(18,2) DEFAULT 0,
            Opwdv decimal(18,2) DEFAULT 0, /* at the time of starting software */
			Additionbefore decimal(18,2) DEFAULT 0,
			AdditionAfter  decimal(18,2) DEFAULT 0,
            Disposal decimal(18,2) DEFAULT 0,
            DeponOPwdv decimal(18,2) DEFAULT 0,
            DepBefore decimal(18,2)   DEFAULT 0,
            DepAfter decimal(18,2)   DEFAULT 0,
            TotalDep decimal(18,2)   DEFAULT 0 ,       
            ClosingWDV decimal(18,2)   DEFAULT 0        
		);
   
    
    insert into temp_report_detail 
    (itgroupid,groupname,fromdate,todate,DepreciationRate,opwdv	)  
    select id,GroupName,startdate,enddate,deprate,opwdv from tblitgroup
    where companyid = _companyid;
    
	
-- find out sum of assets added in A Group before 180 days

-- select 
          
    update temp_report_detail detail 
    inner join (
		select ITGroupIDID,SUM(AmountCApitalisedIT)  as amount_capitalized from tblassets
		where  (DtPutToUseIT >= startdate and DtPutToUseIT <=  enddate)
		and DtPutToUseIT <= cutoffdate
        and Companyid = _companyid
		group by ITGroupIDID
	) as assetbefore ON
    detail.itgroupid = assetbefore.ITGroupIDID
    set detail.Additionbefore = assetbefore.amount_capitalized
    where id > 0;



-- find out sum of assets added in A Group after 180 days
	           
    update temp_report_detail detail 
    inner join (
		select ITGroupIDID,SUM(AmountCApitalisedIT)  as amount_capitalized from tblassets
		where  (DtPutToUseIT >= startdate and DtPutToUseIT <=  enddate)
		and DtPutToUseIT > cutoffdate
         and Companyid = _companyid
		group by agroupid
	) as assetafter ON
    detail.itgroupid = assetafter.ITGroupIDID
    set detail.AdditionAfter = assetafter.amount_capitalized
    where id > 0;
    
    
    
    -- disposal
    
     update temp_report_detail detail 
     inner join (
		select tblassets.ITGroupIDID,SUM(DisposalAmount) dispamount from tbldisposal
		inner join tblassets  on tbldisposal.AssetID = tblassets.ID
		where  (tbldisposal.DisposalDate >= startdate and tbldisposal.DisposalDate  <= enddate)
         and tblassets.Companyid = _companyid
		group by tblassets.ITGroupIDID
	) as disposal    ON
    detail.itgroupid = disposal.ITGroupIDID
    set detail.Disposal = disposal.dispamount
    where id > 0;
    
    
    
    /* end  Calculate Dep here*/
        
    select * from temp_report_detail; /* commnet this once proc is complete*/
    
    
    
END$$
DELIMITER ;

call usp_ITDepreciationCalc(1,'2019-04-01','2020-03-31','2019-09-30');
