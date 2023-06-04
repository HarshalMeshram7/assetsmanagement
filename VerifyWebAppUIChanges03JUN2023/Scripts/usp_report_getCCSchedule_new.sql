
DELIMITER //


DROP PROCEDURE IF exists usp_report_getCCSchedule //

CREATE PROCEDURE `usp_report_getCCSchedule`(IN companyid int,IN startdate date, IN enddate date)
BEGIN


    
    
    /*DEClARE curAsset CURSOR FOR SELECT id FROM tblassets  where companyid=companyid and disposalflag=0 and id=18245;*/
    
        
    /*
		crete required temp tables
        
    
    */
   
  
    
    
    /*DEClARE curAsset CURSOR FOR SELECT id FROM tblassets  where companyid=companyid and disposalflag=0 and id=18245;*/
    
        
    /*
		crete required temp tables
        
    
    */
   
   DROP TEMPORARY TABLE IF EXISTS temp_report_working ;
    CREATE TEMPORARY TABLE temp_report_working (
		v_assetid  INT NOT NULL PRIMARY KEY,
        v_OpGross decimal(18,2),
        v_Addition decimal(18,2),
        v_OpDep decimal(18,2),
        v_disposalopgross decimal(18,2),
        v_Disposalgross decimal(18,2),
        v_disposalopdep decimal(18,2),
		v_disposaldepforfromdttodt decimal(18,2),
        v_Disposalopaccumalted decimal(18,2),
        v_depriciation_openingamt decimal(18,2),
        v_depriciation_UptoDep decimal(18,2)
   
   );
   
   DROP TEMPORARY TABLE IF EXISTS temp_report_final ;
    CREATE TEMPORARY TABLE temp_report_final (
		
        
			assetid  INT NOT NULL PRIMARY KEY,
			companyid INT NOT NULL,
            AGroupName text,
			BGroupName text,
			CGroupName text,
		    DGroupName text,   
             AssetNo text,
			AssetIdentificationNo text,
			AssetName text,
            OpGross decimal(18,2),
            Addition decimal(18,2),
            Disposal decimal(18,2),
            ClGross decimal(18,2),
            OpDep decimal(18,2),
            UpToDep decimal(18,2),
            DispoDep decimal(18,2),
            TotDep decimal(18,2),
            NetBalance decimal(18,2),            
            DepRate decimal(18,2),
            DepMethod text,
            voucherDate  datetime,
           
            VoucherNo text,
            PONo text,
            DTPutUse datetime,
            Remarks text,
            Qty INT,
            BillNo text,
            BillDate datetime,
            SrNo text,           
            Model text,
            ALocName text,
            BLocName text,
            CLocName text,
            SupplierName text,
            CCCode  text,
            CCDescription text
            
    
            
		);
        
			
		
                
		insert into temp_report_final (companyid,assetid,AGroupName,BGroupName,CGroupName,DGroupName,AssetNo,AssetIdentificationNo,AssetName,
			OpGross , Addition ,Disposal ,ClGross ,OpDep ,
				UpToDep ,DispoDep ,TotDep ,NetBalance,DepRate,DepMethod,voucherDate,VoucherNo,PONo,DTPutUse,
                Remarks,Qty,BillNo,BillDate,SrNo,Model,ALocName,BLocName,CLocName,SupplierName,CCCode,CCDescription)
			select companyid,ID,null agroupname,null BGroupName,null CGroupName,null DGroupName,AssetNo,AssetIdentificationNo,AssetName,0 OpGross,0 Addition,0 Disposal,0 ClGross,0 OpDep,
				0 UpToDep,0 DispoDep ,0 TotDep,0 NetBalance,0 DepRate,DepreciationMethod,voucherDate,VoucherNo,PONo,DTPuttoUse,Remarks,
                 Qty,BillNo,BillDate,SrNo,Model,null ALocName,null BLocName,null CLocName,null SupplierName,
                null CCCode,null CCDescription from tblassets where companyid=companyid ;


		

		insert into temp_report_working (v_assetid,v_OpGross ,v_Addition ,v_OpDep ,
        v_disposalopgross ,v_Disposalgross,v_disposalopdep,v_disposaldepforfromdttodt ,v_Disposalopaccumalted ,v_depriciation_openingamt ,v_depriciation_UptoDep )
        select id,0,0,0,0,0,0,0,0,0,0 from tblassets where companyid=companyid ;
        
        
        
		
	
            /*Logic inside loop Start */
			/*SELECT v_agroupid; */
            
            
            /*
            
				UPDATE tableB
				INNER JOIN tableA ON tableB.name = tableA.name
				SET tableB.value = IF(tableA.value > 0, tableA.value, tableB.value)
				WHERE tableA.name = 'Joe'
            */

				update temp_report_working
				inner join tblassets on temp_report_working.v_assetid =  tblassets.id
                set temp_report_working.v_OpGross = IFNULL(AmountCapitalisedCompany,0)
                where tblassets.companyid = companyid
                and  tblassets.voucherdate < startdate  and tblassets.voucherdate <= enddate and assetno>=1 ;
            
				
                /*
				set v_OpGross = 0;
				set v_OpGross = (select IFNULL(AmountCapitalisedCompany,0) from tblassets
							where Companyid = companyid and id = v_assetid
                            and voucherdate < startdate  and voucherdate <= enddate);
                  */          
                  
                  	update temp_report_working
				inner join tblassets on temp_report_working.v_assetid =  tblassets.id
                set temp_report_working.v_Addition = IFNULL(AmountCapitalisedCompany,0)
                where tblassets.companyid = companyid
                and  tblassets.voucherdate >= startdate  and tblassets.voucherdate <= enddate;
            
				/*
				set v_Addition = 0;
				set v_Addition = (select IFNULL(AmountCapitalisedCompany,0) from tblassets
							where Companyid = companyid and id = v_assetid
                            and voucherdate >= startdate and voucherdate <= enddate);
                            
				*/
		
                  
                  	update temp_report_working
				inner join tblassets on temp_report_working.v_assetid =  tblassets.id
                set temp_report_working.v_OpDep = IFNULL(OPAccDepreciation,0)
                where tblassets.companyid = companyid
                  and tblassets.voucherdate <= startdate;
                
         
				/*
				set v_OpDep = 0;
				set v_OpDep = (select IFNULL(OPAccDepreciation,0) from tblassets
							where Companyid = companyid and id = v_assetid
                            and voucherdate <= enddate and voucherdate <= startdate );
				*/



				/*
                
							UPDATE
				Table AS t
				LEFT JOIN (
					SELECT
						Index1,
						Index2,
						COUNT(EventType) AS NumEvents
					FROM
						MEvents
					WHERE
						EventType = 'A' OR EventType = 'B'
					GROUP BY
						Index1,
						Index2
				) AS m ON
					m.Index1 = t.Index1 AND
					m.Index2 = t.Index2
			SET
				t.SpecialEventCount = m.NumEvents
			WHERE
				t.SpecialEventCount IS NULL
                */


				update temp_report_working working
                inner join (
							select assetid,IFNULL(sum(GrossAmount),0) GrossAmount
                            from tbldisposal
                            where tbldisposal.companyid = companyid
							and  tbldisposal.voucherdate < startdate
                            group by assetid
						) as disposal ON
                        working.v_assetid = disposal.assetid
                set working.v_disposalopgross = disposal.GrossAmount;


                   

					

				/*
				set v_disposalopgross = 0;
            
				set v_disposalopgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal  
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tbldisposal.AssetId= v_assetid
                            and tbldisposal.voucherdate <= enddate and tbldisposal.voucherdate < startdate );
                  */          
		
        
        
        	update temp_report_working working
                inner join (
							select assetid,IFNULL(sum(OpAccumulatedDep),0) GrossAmount
                            from tbldisposal
                            where tbldisposal.companyid = companyid
							 and tbldisposal.voucherdate < startdate 
                            group by assetid
						) as disposal ON
                        working.v_assetid = disposal.assetid
                set working.v_disposalopdep = disposal.GrossAmount;

                
                update temp_report_working working
                inner join (
							select assetid,IFNULL(sum(DepForFromDtToDt),0) GrossAmount
                            from tbldisposal
                            where tbldisposal.companyid = companyid
							 and tbldisposal.voucherdate < startdate 
                            group by assetid
						) as disposal ON
                        working.v_assetid = disposal.assetid
                set working.v_disposaldepforfromdttodt = disposal.GrossAmount;
                
               
                 update temp_report_working set v_disposalopdep =  v_disposalopdep + v_disposaldepforfromdttodt where v_assetid > 0;
               
               
				
    
	
	
				/*
					set v_disposalopdep = 0;
            
					set v_disposalopdep = (select IFNULL(sum(OpAccumulatedDep),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tbldisposal.AssetId = v_assetid
                          and tbldisposal.voucherdate<= enddate  and tbldisposal.voucherdate < startdate ) ;

		
				*/
                
                update temp_report_working set v_OpGross = v_OpGross - v_disposalopgross where v_assetid > 0;
					
				/*			set v_OpGross =  v_OpGross - v_disposalopgross;*/
                
                update temp_report_working set v_OpDep = v_OpDep - v_disposalopdep  where v_assetid > 0;
                
             
				 -- set v_OpDep =  v_OpDep - v_disposalopdep;
                            
	
	


        	update temp_report_working working
                inner join (
							select assetid,IFNULL(sum(GrossAmount),0) GrossAmount
                            from tbldisposal
                            where tbldisposal.companyid = companyid
							and  tbldisposal.voucherdate <= enddate and tbldisposal.voucherdate >= startdate
                            group by assetid
						) as disposal ON
                        working.v_assetid = disposal.assetid
                set working.v_Disposalgross = disposal.GrossAmount;


						
                            
                            /*
	
							set v_Disposalgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.id = v_assetid
                          and tbldisposal.voucherdate <= enddate  and tbldisposal.voucherdate >= startdate );

						*/



        	update temp_report_working working
                inner join (
							select assetid,IFNULL(sum(OpAccumulatedDep),0) OpAccumulatedDep
                            from tbldisposal
                            where tbldisposal.companyid = companyid
							and  tbldisposal.voucherdate <= enddate and tbldisposal.voucherdate >= startdate 
                            group by assetid
						) as disposal ON
                        working.v_assetid = disposal.assetid
                set working.v_Disposalopaccumalted = disposal.OpAccumulatedDep;

				
                    
                    
                    /*
					set v_Disposalopaccumalted = (select IFNULL(sum(OpAccumulatedDep),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tbldisposal.AssetId  = v_assetid
                           and tbldisposal.voucherdate <= enddate and tbldisposal.voucherdate >= startdate );

					*/
			
            

        	update temp_report_working working
                inner join (
							select assetid,IFNULL(sum(Amount),0) as depriciation_openingamt
                            from tbldepreciation
                            where tbldepreciation.companyid = companyid
							and  tbldepreciation.FromDate < startdate 
                            group by assetid
						) as depreciation ON
                        working.v_assetid = depreciation.assetid
                set working.v_depriciation_openingamt = depreciation.depriciation_openingamt;

            
                 --    select * from temp_report_working  where v_assetid=5532;
					/*
            
					set v_depriciation_openingamt  = 0;
					set v_depriciation_openingamt  = (select IFNULL(sum(Amount),0) from tbldepreciation
									inner join tblassets on tbldepreciation.AssetId = tblassets.id
									where tbldepreciation.Companyid = companyid 
									and tbldepreciation.AssetId = v_assetid
									and tbldepreciation.FromDate < startdate and tbldepreciation.ToDate <= enddate);

					*/


					update temp_report_working set v_OpDep = v_OpDep + v_depriciation_openingamt where v_assetid > 0;
                 
           --   select * from temp_report_working  where v_assetid=5532;
					/* set v_OpDep=v_OpDep+v_depriciation_openingamt; */


        	update temp_report_working working
                inner join (
							select assetid,IFNULL(sum(Amount),0) as depriciation_UptoDep
                            from tbldepreciation
                            where tbldepreciation.companyid = companyid
							and  tbldepreciation.FromDate >= startdate and tbldepreciation.ToDate <= enddate
                            group by assetid
						) as depreciation ON
                        working.v_assetid = depreciation.assetid
                set working.v_depriciation_UptoDep = depreciation.depriciation_UptoDep;


                    
					/*	
					set v_depriciation_UptoDep  = 0;
					set v_depriciation_UptoDep  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tbldepreciation.AssetId  = v_assetid
                            and tbldepreciation.FromDate >= startdate and tbldepreciation.ToDate<= enddate);

					*/

            /*
            
				UPDATE tableB
				INNER JOIN tableA ON tableB.name = tableA.name
				SET tableB.value = IF(tableA.value > 0, tableA.value, tableB.value)
				WHERE tableA.name = 'Joe'
            */






				update temp_report_final 
				inner join temp_report_working on temp_report_final.assetid = temp_report_working.v_assetid
				set temp_report_final.OpGross =  temp_report_working.v_OpGross,
					temp_report_final.OpDep = temp_report_working.v_OpDep,
                    temp_report_final.Addition = temp_report_working.v_Addition,
                    temp_report_final.Disposal = temp_report_working.v_Disposalgross,
                    temp_report_final.DispoDep = temp_report_working.v_Disposalopaccumalted,
                    temp_report_final.UpToDep = temp_report_working.v_depriciation_UptoDep;

			update temp_report_final 
				set temp_report_final.clgross = (OpGross + Addition) - Disposal
                where assetid > 0;


			update temp_report_final 
				set temp_report_final.OpDep = 0 
                where OpGross=0 and assetid > 0;

                
                
			update temp_report_final 
				set temp_report_final.TotDep = (OpDep + UpToDep) - DispoDep
                where assetid > 0;
                
                
			update temp_report_final 
				set temp_report_final.NetBalance = (clgross - TotDep) 
                where assetid > 0;

			
			update temp_report_final 
				set temp_report_final.NetBalance = (clgross - TotDep) 
                where assetid > 0;
        



		
        update temp_report_final 
				set temp_report_final.AGroupName = (select (A.AGroupName) from tblagroup A inner join tblassets D on A.id=D.AGroupID where D.id=assetid) 
                where assetid > 0;
			
            update temp_report_final 
				set temp_report_final.BGroupName = (select (A.BGroupName) from tblbgroup A inner join tblassets D on A.id=D.BGroupID where D.id=assetid) 
                where assetid > 0;
                
              update temp_report_final 
				set temp_report_final.CGroupName = (select (A.CGroupName) from tblcgroup A inner join tblassets D on A.id=D.CGroupID where D.id=assetid) 
                where assetid > 0; 
                
                 update temp_report_final 
				set temp_report_final.DGroupName = (select (A.DGroupName) from tbldgroup A inner join tblassets D on A.id=D.DGroupID where D.id=assetid) 
                where assetid > 0;
                
                -- locname
                
                 update temp_report_final 
				set temp_report_final.ALocName = (select (A.ALocationName) from tblalocation A inner join tblassets D on A.id=D.LocAID where D.id=assetid) 
                where assetid > 0;
                
              update temp_report_final 
				set temp_report_final.BLocName = (select (A.BLocationName) from tblblocation A inner join tblassets D on A.id=D.LocBID where D.id=assetid) 
                where assetid > 0; 
                
                 update temp_report_final 
				set temp_report_final.CLocName = (select (A.CLocationName) from tblclocation A inner join tblassets D on A.id=D.LocCID where D.id=assetid) 
                where assetid > 0;
                
                
                 update temp_report_final 
				set temp_report_final.SupplierName = (select (A.SupplierName) from tblsupplier A inner join tblassets D on A.id=D.SupplierNo where D.id=assetid) 
                where assetid > 0; 
                
                 update temp_report_final 
				set temp_report_final.CCDescription = (select (A.CCDescription) from tblacostcenter A inner join tblassets D on A.id=D.CostCenterAID where D.id=assetid) 
                where assetid > 0;
                
                 update temp_report_final 
				set temp_report_final.CCCode = (select (A.CCCode) from tblacostcenter A inner join tblassets D on A.id=D.CostCenterAID where D.id=assetid) 
                where assetid > 0;
				
				
/*
 AGroupName text,
			  ALocName text,
            BLocName text,
            CLocName text,
            SupplierName text,
              CCCode  text,
            CCDescription text
*/
			/*
			update temp_report set OpGross = v_OpGross where assetid  = v_assetid ;
            update temp_report set OpDep = v_OpDep where assetid  = v_assetid ; 
            update temp_report set Addition = v_Addition where assetid  = v_assetid;  
			update temp_report set Disposal = v_Disposalgross where assetid  = v_assetid; 
			update temp_report set DispoDep = v_Disposalopaccumalted where assetid  = v_assetid; 
            update temp_report set UpToDep = v_depriciation_UptoDep where assetid  = v_assetid;
            update temp_report set clgross = (OpGross + Addition) - Disposal where assetid  = v_assetid;
			update temp_report set  TotDep= (OpDep + UpToDep) - DispoDep where assetid  = v_assetid ;
			update temp_report set  NetBalance= (clgross -TotDep)  where assetid  = v_assetid ;
           
            
				*/
     

            
        
		select * from temp_report_final ;  /* repoort output */
        
	
    
    
    
        
END //

DELIMITER ;