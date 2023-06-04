
DROP PROCEDURE IF EXISTS Usefullifereport;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `Usefullifereport`(IN companyid int,IN usefullifetype text)
BEGIN
declare v_Date datetime;
declare v_currentdate datetime;

    declare finished int default 0;
    
	declare v_assetid int; 
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
    
    
    
    DEClARE curAsset CURSOR FOR SELECT ID FROM tblassets;
    DECLARE CONTINUE HANDLER 
			FOR NOT FOUND SET finished = 1;
        
    
   /*
   set current accordingto usefullifetype
   
   
   */
   set v_currentdate=Cast(CURRENT_TIMESTAMP as Date);
   if(usefullifetype="0")
    then
    
   set v_Date=Cast(CURRENT_TIMESTAMP as Date);
   elseif(usefullifetype="3")
   then
    set v_Date=Cast(CURRENT_TIMESTAMP as Date);
    set v_Date=DATE_ADD(v_Date, INTERVAL +3 MONTH);
    elseif(usefullifetype="6")
    then
     set v_Date=Cast(CURRENT_TIMESTAMP as Date);
      set v_Date=DATE_ADD(v_Date, INTERVAL +6 MONTH);
        End if;
          DROP TEMPORARY TABLE IF EXISTS temp_report ;
          
		CREATE TEMPORARY TABLE temp_report (
			Assetid INT PRIMARY KEY NOT NULL,
            AssetNo varchar(200) ,
            AssetName varchar(2000),
            Dateputtouse datetime,
            ExpiryDate datetime,
            AmountCapitalised decimal(18,2)  null,
			
            UpToDep decimal(18,2) null
           /* OpGross decimal(18,2)  null,
            Addition decimal(18,2) null,
            Disposal decimal(18,2) null,
            ClGross decimal(18,2) null,
            OpDep decimal(18,2) null,
            DispoDep decimal(18,2) null,
            TotDep decimal(18,2) null,
            NetBalance decimal(18,2) null*/
		);
   
   


    if(usefullifetype='0')
    then
    
   
			insert into temp_report (Assetid,AssetNo,AssetName,Dateputtouse,ExpiryDate,AmountCapitalised,UpToDep )
			select id,AssetNo,AssetName,DtPutToUse,ExpiryDate,AmountCapitalised,0 UpToDep from
			tblassets where companyid=companyid and ExpiryDate <= v_Date;
    
    else
		  insert into temp_report (Assetid,AssetNo,AssetName,Dateputtouse,ExpiryDate,AmountCapitalised,
								UpToDep )
				select id,AssetNo,AssetName,DtPutToUse,ExpiryDate,AmountCapitalised,0 UpToDep
				from  tblassets where 
				companyid=companyid and ExpiryDate>=v_Date;
			 

    end if;
   
					/*
					update temp_report set UpToDep =(select IFNULL(sum(Amount),0) from tbldepreciation
							inner join temp_report on tbldepreciation.AssetId = temp_report.id
							where tbldepreciation.Companyid = companyid 
                            and tbldepreciation.ToDate<= v_currentdate);
            */
            
                UPDATE temp_report rpt
			INNER JOIN (
				SELECT assetid, SUM(Amount) as DepAmount
				FROM tbldepreciation
				WHERE ToDate <= v_currentdate
                and Companyid = companyid
				GROUP BY assetid
				) dep ON rpt.assetid = dep.assetid
		SET rpt.UpToDep = dep.DepAmount
        where rpt.assetid > 0;


           
		select * from temp_report;
        
END$$
DELIMITER ;
