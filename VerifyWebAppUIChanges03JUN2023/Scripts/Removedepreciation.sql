DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `removedepreciation`(IN companyid int ,
IN startdate date,IN enddate date )
BEGIN

declare delete_status text;

delete from tbldepreciation where FromDate=startdate and ToDate=enddate and companyid=companyid;
 set delete_status="Yes";

select delete_status;

END$$
DELIMITER ;
