

CREATE TABLE `tblitdepreciation` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ClientID` int(11) DEFAULT 0,
  `Companyid` int(11) DEFAULT 1,
  `ITGroupID` int(11) DEFAULT NULL,
  `GroupName` varchar(1000) DEFAULT NULL,
  `FromDate` datetime DEFAULT NULL,
  `ToDate` datetime DEFAULT NULL,
  `DepreciationRate` decimal(18,2) DEFAULT NULL,
  `Opwdv` decimal(18,2) DEFAULT NULL,
  `AdditionAfter` decimal(18,2) DEFAULT NULL,
  `Additionbefore` decimal(18,2) DEFAULT NULL,
  `Disposal` decimal(18,2) DEFAULT NULL,
  `Total` decimal(18,2) DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  `DeponOPwdv` decimal(18,2) DEFAULT NULL,
  `AdditionDephalf` decimal(18,2) DEFAULT NULL,
  `AdditionDepfull` decimal(18,2) DEFAULT NULL,
  `TotalDep` decimal(18,2) DEFAULT NULL,
  `ClosingWDV` decimal(18,2) DEFAULT NULL,
  
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  

  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf32;
