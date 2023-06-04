DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `employeeassettrackingreport`(IN companyid int ,
IN startdate date,IN enddate date ,
IN fromassetno text,IN toassetno text,IN empid int)
Begin  


 
   DROP TEMPORARY TABLE IF EXISTS temp_report_final ;
    CREATE TEMPORARY TABLE temp_report_final (
		
        
			assetid int ,
            employeeid int,
			companyid INT NOT NULL,
            AssetNo text,
			AssetName text,
			AssetIdentificationNo text,
            SystemAssetId text,
			IssueDate Datetime,
		    SrNo text,   
             Remarks text,
			EmpName text,
            Model text
          );
           
         
           
           if (companyid!=0 and startdate!=''  and enddate!=''  and fromassetno!=''  and toassetno!='' and empid!=0 )
            
            then
            select 'if';
            insert into temp_report_final (assetid,employeeid,companyid,
			AssetNo , AssetName ,AssetIdentificationNo,SystemAssetId ,IssueDate ,SrNo ,Remarks,
				EmpName ,Model) 
			select CA.AssetID,CA.Empid, CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo,A.MRRNo,CA.IssueDate,A.SrNo
				,A.Remarks,null EmpName,A.Model
                from tblemployeeasset CA inner join tblassets A on CA.AssetID=A.id
                where CA.companyid=companyid and A.disposalflag=0 
               and  cast(A.assetno as unsigned) >= cast(fromassetno as unsigned) and cast(A.assetno as unsigned) <= cast(toassetno as unsigned)
                and CA.EmpId=empid and CA.IssueDate >= startdate and CA.IssueDate <= enddate ;

           select * from temp_report_final;
         
          
		
        
        
        
        -- fromassetno=0toassetno=0alocid=0
            elseif (companyid!=0 and startdate!='' and enddate!='' and fromassetno='' and toassetno='' and empid=0)
            then
            insert into temp_report_final (assetid,employeeid,companyid,
			AssetNo , AssetName ,AssetIdentificationNo,SystemAssetId ,IssueDate ,SrNo ,Remarks,
				EmpName ,Model) 
			select CA.assetid,CA.empid, CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo, A.MRRNo,CA.IssueDate,A.SrNo
				,A.Remarks,null EmpName,A.Model
                from tblemployeeasset CA inner join tblassets A on CA.AssetID=A.id
                where CA.companyid=companyid and A.disposalflag=0 
            
                and CA.IssueDate>=startdate and CA.IssueDate<=enddate;

           
           
          
		
        
        
        -- startdate=null enddate=null alocid=0
           elseif (companyid!=0 and startdate='' and enddate='' and fromassetno!='' and toassetno!='' and empid=0)
            then
         
            insert into temp_report_final (assetid,employeeid,companyid,
			AssetNo , AssetName ,AssetIdentificationNo,SystemAssetId ,IssueDate ,SrNo ,Remarks,
				EmpName ,Model) 
			select CA.assetid,CA.empid, CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo,A.MRRNo,CA.IssueDate,A.SrNo
				,A.Remarks,null EmpName,A.Model
                from tblemployeeasset CA inner join tblassets A on CA.AssetID=A.id
                where CA.companyid=companyid and A.disposalflag=0 
               and cast(A.assetno as unsigned) >= cast(fromassetno as unsigned) and cast(A.assetno as unsigned) <= cast(toassetno as unsigned);
               

        
		
        
      
        
        -- fromassetno=null toassetno=null
           elseif (companyid!=0 and startdate!='' and enddate!='' and fromassetno='' and toassetno='' and empid!=0)
            then
            insert into temp_report_final (assetid,employeeid,companyid,
			AssetNo , AssetName ,AssetIdentificationNo,SystemAssetId ,IssueDate ,SrNo ,Remarks,
				EmpName ,Model) 
			select CA.assetid,CA.employeeid, CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo,A.MRRNo,CA.IssueDate,A.SrNo
				,A.Remarks,null EmpName,A.Model
                from tblemployeeasset CA inner join tblassets A on CA.AssetID=A.id
                where CA.companyid=companyid and A.disposalflag=0 
          
                and CA.EmpId=empid and CA.IssueDate>=startdate and CA.IssueDate<=enddate;

           
             
          
		
        
        
        
                -- startdate=null enddate=null
           elseif (companyid!=0 and startdate='' and enddate='' and fromassetno!='' and toassetno!='' and empid!=0)
            then
            insert into temp_report_final (assetid,employeeid,companyid,
			AssetNo , AssetName ,AssetIdentificationNo,SystemAssetId ,IssueDate ,SrNo ,Remarks,
				EmpName ,Model) 
			select CA.assetid,CA.empid, CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo,A.MRRNo,CA.IssueDate,A.SrNo
				,A.Remarks,null EmpName,A.Model
                from tblemployeeasset CA inner join tblassets A on CA.AssetID=A.id
                where CA.companyid=companyid and A.disposalflag=0 
                and cast(A.assetno as unsigned) >= cast(fromassetno as unsigned) and cast(A.assetno as unsigned) <= cast(toassetno as unsigned)
               and CA.EmpId=empid ;

           
          
		
        
        
        
                   -- startdate=null enddate=null fromassetno=null toassetno=null
           elseif (companyid!=0 and startdate='' and enddate='' and fromassetno='' and toassetno='' and empid!=0)
            then
            insert into temp_report_final (assetid,employeeid,companyid,
			AssetNo , AssetName ,AssetIdentificationNo,SystemAssetId ,IssueDate ,SrNo ,Remarks,
				EmpName ,Model) 
			select CA.assetid,CA.empid, CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo,A.MRRNo,CA.IssueDate,A.SrNo
				,A.Remarks,null EmpName,A.Model
                from tblemployeeasset CA inner join tblassets A on CA.AssetID=A.id
                where CA.companyid=companyid and A.disposalflag=0 and CA.empid=empid ;
                  

           
            
		
        
        End if;
       
		select * from temp_report_final; /* repoort output */
        

END$$
DELIMITER ;
