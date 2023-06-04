

DELIMITER $$
CREATE  PROCEDURE `removeitdepreciation`(IN companyid int ,
IN startdate date,IN enddate date )
BEGIN

declare delete_status text;

delete from tblitdepreciation where FromDate=startdate and ToDate=enddate and companyid=companyid and id > 0;

 set delete_status="Yes";

select delete_status;

END$$
DELIMITER ;
