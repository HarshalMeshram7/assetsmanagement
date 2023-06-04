DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `assettrackingreport`(IN companyid int ,
IN startdate date,IN enddate date ,
IN fromassetno text,IN toassetno text,IN alocid int)
Begin  


 
   DROP TEMPORARY TABLE IF EXISTS temp_report_final ;
    CREATE TEMPORARY TABLE temp_report_final (
		
        
			assetid int ,
            alocid int,
			companyid INT NOT NULL,
            AssetNo text,
			AssetName text,
			AssetIdentificationNo text,
            SystemAssetId text,
			IssueDate Datetime,
		    SrNo text,   
             Remarks text,
			ALocName text,
            Model text
          );
           
         
           
           if (companyid!=0 and startdate!=''  and enddate!=''  and fromassetno!=''  and toassetno!='' and alocid!=0 )
            
            then
            
            insert into temp_report_final (assetid,alocid,companyid,
			AssetNo , AssetName ,AssetIdentificationNo,SystemAssetId ,IssueDate ,SrNo ,Remarks,
				ALocName ,Model) 
			select CA.AssetID,CA.alocid, CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo,A.MRRNo,CA.Date,A.SrNo
				,A.Remarks,null ALocName,A.Model
                from tblchildlocation CA inner join tblassets A on CA.AssetID=A.id
                where CA.companyid=companyid and A.disposalflag=0 
               and  cast(A.assetno as unsigned) >= cast(fromassetno as unsigned) and cast(A.assetno as unsigned) <= cast(toassetno as unsigned)
                and CA.ALocID=alocid and CA.Date >= startdate and CA.Date <= enddate ;
					
                   
          
		
        
        
        
        -- fromassetno=0toassetno=0alocid=0
            elseif (companyid!=0 and startdate!='' and enddate!='' and fromassetno='' and toassetno='' and alocid=0)
            then
         
            insert into temp_report_final (assetid,alocid,companyid,
			AssetNo , AssetName ,AssetIdentificationNo,SystemAssetId ,IssueDate ,SrNo ,Remarks,
				ALocName ,Model) 
			select CA.assetid,CA.alocid, CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo, A.MRRNo,CA.Date,A.SrNo
				,A.Remarks,null ALocName,A.Model
                from tblchildlocation CA inner join tblassets A on CA.AssetID=A.id
                where CA.companyid=companyid and A.disposalflag=0 
            
                and CA.Date>=startdate and CA.Date<=enddate;

        
          
          
		
        
        
        -- startdate=null enddate=null alocid=0
           elseif (companyid!=0 and startdate='' and enddate='' and fromassetno!='' and toassetno!='' and alocid=0)
            then
         
            insert into temp_report_final (assetid,alocid,companyid,
			AssetNo , AssetName ,AssetIdentificationNo,SystemAssetId ,IssueDate ,SrNo ,Remarks,
				ALocName ,Model) 
			select CA.assetid,CA.alocid, CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo,A.MRRNo,CA.Date,A.SrNo
				,A.Remarks,null ALocName,A.Model
                from tblchildlocation CA inner join tblassets A on CA.AssetID=A.id
                where CA.companyid=companyid and A.disposalflag=0 
               and cast(A.assetno as unsigned) >= cast(fromassetno as unsigned) and cast(A.assetno as unsigned) <= cast(toassetno as unsigned);
               
        
        -- fromassetno=null toassetno=null
           elseif (companyid!=0 and startdate!='' and enddate!='' and fromassetno='' and toassetno='' and alocid!=0)
            then
            insert into temp_report_final (assetid,alocid,companyid,
			AssetNo , AssetName ,AssetIdentificationNo,SystemAssetId ,IssueDate ,SrNo ,Remarks,
				ALocName ,Model) 
			select CA.assetid, CA.alocid,CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo,A.MRRNo,CA.Date,A.SrNo
				,A.Remarks,null ALocName,A.Model
                from tblchildlocation CA inner join tblassets A on CA.AssetID=A.id
                where CA.companyid=companyid and A.disposalflag=0 
          
                and CA.ALocID=alocid and CA.Date>=startdate and CA.Date<=enddate;

      
          
		
        
        
        
                -- startdate=null enddate=null
           elseif (companyid!=0 and startdate='' and enddate='' and fromassetno!='' and toassetno!='' and alocid!=0)
            then
            insert into temp_report_final (assetid,alocid,companyid,
			AssetNo , AssetName ,AssetIdentificationNo,SystemAssetId ,IssueDate ,SrNo ,Remarks,
				ALocName ,Model) 
			select CA.assetid, CA.alocid,CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo,A.MRRNo,CA.Date,A.SrNo
				,A.Remarks,null ALocName,A.Model
                from tblchildlocation CA inner join tblassets A on CA.AssetID=A.id
                where CA.companyid=companyid and A.disposalflag=0 
                and cast(A.assetno as unsigned) >= cast(fromassetno as unsigned) and cast(A.assetno as unsigned) <= cast(toassetno as unsigned)
               and CA.ALocID=alocid ;

                   -- startdate=null enddate=null fromassetno=null toassetno=null
           elseif (companyid!=0 and startdate='' and enddate='' and fromassetno='' and toassetno='' and alocid!=0)
            then
            insert into temp_report_final (assetid,alocid,companyid,
			AssetNo , AssetName ,AssetIdentificationNo,SystemAssetId ,IssueDate ,SrNo ,Remarks,
				ALocName ,Model) 
			select CA.assetid,CA.alocid, CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo,A.MRRNo,CA.Date,A.SrNo
				,A.Remarks,null ALocName,A.Model
                from tblchildlocation CA inner join tblassets A on CA.AssetID=A.id
                where CA.companyid=companyid and A.disposalflag=0 and CA.ALocID=alocid ;
                  

        End if;
       
		select * from temp_report_final; /* repoort output */
        

END$$
DELIMITER ;
