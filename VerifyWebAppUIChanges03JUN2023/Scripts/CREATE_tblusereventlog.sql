CREATE TABLE `tblusereventlog` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `TranDate` datetime NOT NULL,
  `UserID` int(11) NOT NULL,
  `Event` varchar(45) NOT NULL,
  `EventDescription` varchar(200) DEFAULT NULL,
  `SourcePage` varchar(45) DEFAULT NULL,
  `CompanyID` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf32;
