 DROP PROCEDURE IF exists usp_calc_update_dep_table; //

DELIMITER //

CREATE PROCEDURE usp_calc_update_dep_table(IN requestid int)
BEGIN


	

	insert into tbldepreciation
    (AssetId,AssetName,FromDate,ToDate,DepreciationType,Amount,NormalRate,AdditionRate,TotalRate,DepreciationDays,DepreciationMethod,CreatedUserId,Modified_Userid,CreatedDate,companyid,clientid)
    select Assetid,Assetname,Fromdate,Todate,DepType ,dep_for_period,Normalrate,Additionrate,DepRate,NoOfDays,DepMethod, 1,0,CURDATE(),companyid,0
    from tbldepreciationcalculation;
    
    update tbldepreciationrequest set InProcess = 2 ,
    EndDateTime = current_timestamp()
    where id = requestid;
    
    
	truncate table tbldepreciationcalculation;
    truncate table tbldepreciation_log;

    
	/* TODO Update lck flag in period table here */
    select 1;
    
END//

DELIMITER ;