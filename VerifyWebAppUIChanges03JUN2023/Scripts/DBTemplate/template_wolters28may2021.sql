CREATE DATABASE  IF NOT EXISTS `wolters` /*!40100 DEFAULT CHARACTER SET utf8 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `wolters`;
-- MySQL dump 10.13  Distrib 8.0.16, for Win64 (x86_64)
--
-- Host: localhost    Database: wolters
-- ------------------------------------------------------
-- Server version	8.0.16

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `tblaccessrights`
--

DROP TABLE IF EXISTS `tblaccessrights`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblaccessrights` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Userid` int(11) DEFAULT NULL,
  `ControllerName` varchar(100) DEFAULT NULL,
  `Add` varchar(45) DEFAULT NULL,
  `Edit` varchar(45) DEFAULT NULL,
  `Delete` varchar(45) DEFAULT NULL,
  `CreatedUserid` int(11) DEFAULT NULL,
  `ModifiedUserid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `Export` varchar(45) DEFAULT NULL,
  `Import` varchar(45) DEFAULT NULL,
  `Index` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblaccount`
--

DROP TABLE IF EXISTS `tblaccount`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblaccount` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `AccountCode` varchar(100) DEFAULT NULL,
  `AccountName` varchar(150) DEFAULT NULL,
  `GroupName` varchar(150) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=47 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblacostcenter`
--

DROP TABLE IF EXISTS `tblacostcenter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblacostcenter` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `CCCode` varchar(50) DEFAULT NULL,
  `CCDescription` varchar(150) DEFAULT NULL,
  `Operative` varchar(10) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tbladdition`
--

DROP TABLE IF EXISTS `tbladdition`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tbladdition` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `AssetId` int(11) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `AssetName` varchar(200) DEFAULT NULL,
  `AdditionAssetName` varchar(200) DEFAULT NULL,
  `VoucherNo` varchar(200) DEFAULT NULL,
  `VoucherDate` datetime DEFAULT NULL,
  `PODate` datetime DEFAULT NULL,
  `ReceiptDate` datetime DEFAULT NULL,
  `CommissioningDate` datetime DEFAULT NULL,
  `BillDate` datetime DEFAULT NULL,
  `DtPutToUse` datetime DEFAULT NULL,
  `DtPutToUseIT` datetime DEFAULT NULL,
  `PONo` varchar(50) DEFAULT NULL,
  `BillNo` varchar(50) DEFAULT NULL,
  `MRRNo` varchar(50) DEFAULT NULL,
  `Qty` int(11) DEFAULT NULL,
  `SupplierNo` int(11) DEFAULT NULL,
  `BrandName` varchar(100) DEFAULT NULL,
  `SrNo` varchar(200) DEFAULT NULL,
  `Model` varchar(200) DEFAULT NULL,
  `Remarks` varchar(200) DEFAULT NULL,
  `IsImported` varchar(100) DEFAULT NULL,
  `Currency` varchar(200) DEFAULT NULL,
  `Values` decimal(18,2) DEFAULT NULL,
  `GrossVal` decimal(18,2) DEFAULT NULL,
  `ServiceCharges` decimal(18,2) DEFAULT NULL,
  `OtherExp` decimal(18,2) DEFAULT NULL,
  `CustomDuty` decimal(18,2) DEFAULT NULL,
  `ExciseDuty` decimal(18,2) DEFAULT NULL,
  `ServiceTax` decimal(18,2) DEFAULT NULL,
  `AnyOtherDuty` decimal(18,2) DEFAULT NULL,
  `VAT` decimal(18,2) DEFAULT NULL,
  `CST` decimal(18,2) DEFAULT NULL,
  `CGST` decimal(18,2) DEFAULT NULL,
  `SGST` decimal(18,2) DEFAULT NULL,
  `IGST` decimal(18,2) DEFAULT NULL,
  `AnyOtherTax` decimal(18,2) DEFAULT NULL,
  `TotalAddition` decimal(18,2) DEFAULT NULL,
  `Discount` decimal(18,2) DEFAULT NULL,
  `Roundingoff` decimal(18,2) DEFAULT NULL,
  `TotDeduction` decimal(18,2) DEFAULT NULL,
  `InvoiceAmt` decimal(18,2) DEFAULT NULL,
  `DutyDrawback` decimal(18,2) DEFAULT NULL,
  `ExciseCredit` decimal(18,2) DEFAULT NULL,
  `ServiceTaxCredit` decimal(18,2) DEFAULT NULL,
  `AnyOtherDutyCredit` decimal(18,2) DEFAULT NULL,
  `VATCredit` decimal(18,2) DEFAULT NULL,
  `CSTCredit` decimal(18,2) DEFAULT NULL,
  `CGSTCredit` decimal(18,2) DEFAULT NULL,
  `SGSTCredit` decimal(18,2) DEFAULT NULL,
  `IGSTCredit` decimal(18,2) DEFAULT NULL,
  `AnyOtherCredit` decimal(18,2) DEFAULT NULL,
  `TotalCredit` decimal(18,2) DEFAULT NULL,
  `AmountCapitalised` decimal(18,2) DEFAULT NULL,
  `AmountCapitalisedCompany` decimal(18,2) DEFAULT NULL,
  `AmountCApitalisedIT` decimal(18,2) DEFAULT NULL,
  `ResidualVal` decimal(18,2) DEFAULT NULL,
  `uom` int(11) DEFAULT NULL,
  `GST` decimal(18,2) DEFAULT NULL,
  `GSTCredit` decimal(18,2) DEFAULT NULL,
  `AdditionNo` int(11) unsigned DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`),
  KEY `AssetId_idx` (`AssetId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblagroup`
--

DROP TABLE IF EXISTS `tblagroup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblagroup` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `AGroupName` varchar(150) DEFAULT NULL,
  `NormalRate` int(11) DEFAULT NULL,
  `AdditionalRate` int(11) DEFAULT NULL,
  `TotalRate` int(11) DEFAULT NULL,
  `DepMethod` varchar(10) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblalocation`
--

DROP TABLE IF EXISTS `tblalocation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblalocation` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ALocationName` varchar(150) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblamc`
--

DROP TABLE IF EXISTS `tblamc`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblamc` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FromDate` datetime DEFAULT NULL,
  `ToDate` datetime DEFAULT NULL,
  `ReminderEMail` varchar(200) DEFAULT NULL,
  `AMCDetails` varchar(150) DEFAULT NULL,
  `Remarks` varchar(200) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblassetfreeofcost`
--

DROP TABLE IF EXISTS `tblassetfreeofcost`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblassetfreeofcost` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Description` varchar(45) DEFAULT NULL,
  `Date` datetime DEFAULT NULL,
  `Qty` int(11) DEFAULT NULL,
  `Uom` int(11) DEFAULT NULL,
  `Asset_id` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`),
  KEY `Asset_id_idx` (`Asset_id`),
  CONSTRAINT `Asset_id` FOREIGN KEY (`Asset_id`) REFERENCES `tblassets` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblassets`
--

DROP TABLE IF EXISTS `tblassets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblassets` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ClientID` int(11) DEFAULT NULL,
  `AssetName` varchar(200) DEFAULT NULL,
  `AssetIdentificationNo` varchar(200) DEFAULT NULL,
  `VoucherNo` varchar(200) DEFAULT NULL,
  `VoucherDate` datetime DEFAULT NULL,
  `PODate` datetime DEFAULT NULL,
  `ReceiptDate` datetime DEFAULT NULL,
  `CommissioningDate` datetime DEFAULT NULL,
  `BillDate` datetime DEFAULT NULL,
  `DtPutToUse` datetime DEFAULT NULL,
  `DtPutToUseIT` datetime DEFAULT NULL,
  `PONo` varchar(50) DEFAULT NULL,
  `BillNo` varchar(50) DEFAULT NULL,
  `MRRNo` varchar(50) DEFAULT NULL,
  `Qty` int(11) DEFAULT NULL,
  `SupplierNo` int(11) DEFAULT NULL,
  `UOMNo` int(11) DEFAULT NULL,
  `BrandName` varchar(100) DEFAULT NULL,
  `SrNo` varchar(200) DEFAULT NULL,
  `Model` varchar(200) DEFAULT NULL,
  `Remarks` varchar(200) DEFAULT NULL,
  `IsImported` varchar(100) DEFAULT NULL,
  `Currency` varchar(200) DEFAULT NULL,
  `Values` decimal(18,2) DEFAULT NULL,
  `AGroupID` int(11) DEFAULT NULL,
  `BGroupID` int(11) DEFAULT NULL,
  `CGroupID` int(11) DEFAULT NULL,
  `DGroupID` int(11) DEFAULT NULL,
  `LocAID` int(11) DEFAULT NULL,
  `LocBID` int(11) DEFAULT NULL,
  `LocCID` int(11) DEFAULT NULL,
  `CostCenterAID` int(11) DEFAULT NULL,
  `CostCenterBID` int(11) DEFAULT NULL,
  `ITGroupIDID` int(11) DEFAULT NULL,
  `DepreciationMethod` varchar(50) DEFAULT NULL,
  `NormalRatae` decimal(18,2) DEFAULT NULL,
  `AdditionalRate` decimal(18,2) DEFAULT NULL,
  `TotalRate` decimal(18,2) DEFAULT NULL,
  `Usefullife` decimal(18,2) DEFAULT NULL,
  `YrofManufacturing` decimal(18,2) DEFAULT NULL,
  `ExpiryDate` datetime DEFAULT NULL,
  `AccountID` int(11) DEFAULT NULL,
  `DepAccountId` int(11) DEFAULT NULL,
  `AccAccountID` int(11) DEFAULT NULL,
  `OPAccDepreciation` decimal(18,2) DEFAULT NULL,
  `GrossVal` decimal(18,2) DEFAULT NULL,
  `ServiceCharges` decimal(18,2) DEFAULT NULL,
  `OtherExp` decimal(18,2) DEFAULT NULL,
  `CustomDuty` decimal(18,2) DEFAULT NULL,
  `ExciseDuty` decimal(18,2) DEFAULT NULL,
  `ServiceTax` decimal(18,2) DEFAULT NULL,
  `AnyOtherDuty` decimal(18,2) DEFAULT NULL,
  `VAT` decimal(18,2) DEFAULT NULL,
  `CSt` decimal(18,2) DEFAULT NULL,
  `CGST` decimal(18,2) DEFAULT NULL,
  `SGST` decimal(18,2) DEFAULT NULL,
  `IGST` decimal(18,2) DEFAULT NULL,
  `AnyOtherTax` decimal(18,2) DEFAULT NULL,
  `TotalAddition` decimal(18,2) DEFAULT NULL,
  `Discount` decimal(18,2) DEFAULT NULL,
  `Roundingoff` decimal(18,2) DEFAULT NULL,
  `TotDeduction` decimal(18,2) DEFAULT NULL,
  `InvoiceAmt` decimal(18,2) DEFAULT NULL,
  `DutyDrawback` decimal(18,2) DEFAULT NULL,
  `ExciseCredit` decimal(18,2) DEFAULT NULL,
  `ServiceTaxCredit` decimal(18,2) DEFAULT NULL,
  `AnyOtherDutyCredit` decimal(18,2) DEFAULT NULL,
  `VATCredit` decimal(18,2) DEFAULT NULL,
  `CSTCredit` decimal(18,2) DEFAULT NULL,
  `CGSTCredit` decimal(18,2) DEFAULT NULL,
  `SGSTCredit` decimal(18,2) DEFAULT NULL,
  `IGSTCredit` decimal(18,2) DEFAULT NULL,
  `AnyOtherCredit` decimal(18,2) DEFAULT NULL,
  `TotalCredit` decimal(18,2) DEFAULT NULL,
  `AmountCapitalised` decimal(18,2) DEFAULT NULL,
  `AmountCapitalisedCompany` decimal(18,2) DEFAULT NULL,
  `AmountCApitalisedIT` decimal(18,2) DEFAULT NULL,
  `ResidualVal` decimal(18,2) DEFAULT NULL,
  `AssetNo` varchar(100) NOT NULL,
  `DisposalFlag` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `Parent_AssetId` int(11) DEFAULT NULL,
  `iscomponent` varchar(45) DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`),
  UNIQUE KEY `AssetNo_UNIQUE` (`AssetNo`,`Companyid`) /*!80000 INVISIBLE */,
  KEY `IDX_AssetNo` (`AssetNo`) USING BTREE,
  KEY `IDX_AssetName` (`AssetName`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=28677 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblbatch`
--

DROP TABLE IF EXISTS `tblbatch`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblbatch` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `BatchDescription` varchar(200) DEFAULT NULL,
  `FromDate` datetime DEFAULT NULL,
  `ToDate` datetime DEFAULT NULL,
  `IsBatchOpen` varchar(10) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblbatchverification`
--

DROP TABLE IF EXISTS `tblbatchverification`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblbatchverification` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ClientCode` int(11) DEFAULT NULL,
  `BatchID` int(11) DEFAULT NULL,
  `AssetNumber` varchar(200) DEFAULT NULL,
  `AssetIndex` int(11) DEFAULT NULL,
  `UserID` int(11) DEFAULT NULL,
  `LastUpdateTimeStamp` datetime DEFAULT NULL,
  `Longitude` decimal(10,8) DEFAULT NULL,
  `Latitude` decimal(10,8) DEFAULT NULL,
  `Assetaddress` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblbcostcenter`
--

DROP TABLE IF EXISTS `tblbcostcenter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblbcostcenter` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `SCCCode` varchar(50) DEFAULT NULL,
  `SCCDescription` varchar(150) DEFAULT NULL,
  `CCID` int(11) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblbgroup`
--

DROP TABLE IF EXISTS `tblbgroup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblbgroup` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `AGrpID` int(11) DEFAULT NULL,
  `BGroupName` varchar(150) DEFAULT NULL,
  `NormalRate` int(11) DEFAULT NULL,
  `AdditionalRate` int(11) DEFAULT NULL,
  `TotalRate` int(11) DEFAULT NULL,
  `DepMethod` varchar(10) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblblocation`
--

DROP TABLE IF EXISTS `tblblocation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblblocation` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ALocID` int(11) DEFAULT NULL,
  `BLocationName` varchar(150) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=41 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblcgroup`
--

DROP TABLE IF EXISTS `tblcgroup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblcgroup` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `AGrpID` int(11) DEFAULT NULL,
  `BGrpID` int(11) DEFAULT NULL,
  `CGroupName` varchar(150) DEFAULT NULL,
  `NormalRate` int(11) DEFAULT NULL,
  `AdditionalRate` int(11) DEFAULT NULL,
  `TotalRate` int(11) DEFAULT NULL,
  `DepMethod` varchar(10) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=155 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblchild_asset_attachment`
--

DROP TABLE IF EXISTS `tblchild_asset_attachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblchild_asset_attachment` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Ass_Id` int(11) DEFAULT NULL,
  `Filename` varchar(1000) DEFAULT NULL,
  `Ext` varchar(45) DEFAULT NULL,
  `File_Bytes` mediumblob,
  `UploadDate` datetime DEFAULT NULL,
  `FileSize` bigint(20) DEFAULT NULL,
  `SourceEvent` varchar(45) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblchildcostcenter`
--

DROP TABLE IF EXISTS `tblchildcostcenter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblchildcostcenter` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Date` datetime DEFAULT NULL,
  `AcostcenterID` int(11) DEFAULT NULL,
  `BcostcenterID` int(11) DEFAULT NULL,
  `Percentage` varchar(45) DEFAULT NULL,
  `Ass_Id` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`),
  KEY `Ass_ID_idx` (`Ass_Id`),
  CONSTRAINT `Ass_ID` FOREIGN KEY (`Ass_Id`) REFERENCES `tblassets` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblchildlocation`
--

DROP TABLE IF EXISTS `tblchildlocation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblchildlocation` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Date` datetime DEFAULT NULL,
  `ALocID` int(11) DEFAULT NULL,
  `BLocID` int(11) DEFAULT NULL,
  `CLocID` int(11) DEFAULT NULL,
  `AssetID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`),
  KEY `AssetID_idx` (`AssetID`),
  CONSTRAINT `AssetID` FOREIGN KEY (`AssetID`) REFERENCES `tblassets` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=49389 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblclient`
--

DROP TABLE IF EXISTS `tblclient`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblclient` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ClientCode` varchar(100) DEFAULT NULL,
  `ClientName` varchar(150) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblclocation`
--

DROP TABLE IF EXISTS `tblclocation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblclocation` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ALocID` int(11) DEFAULT NULL,
  `BLocID` int(11) DEFAULT NULL,
  `CLocationName` varchar(150) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=1294 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblcompany`
--

DROP TABLE IF EXISTS `tblcompany`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblcompany` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `CompanyName` varchar(150) DEFAULT NULL,
  `Address` varchar(200) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `Userid` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `ModifiedUserid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblcompanypermission`
--

DROP TABLE IF EXISTS `tblcompanypermission`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblcompanypermission` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) DEFAULT NULL,
  `CompanyId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tbldepreciation`
--

DROP TABLE IF EXISTS `tbldepreciation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tbldepreciation` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `AssetId` int(11) DEFAULT NULL,
  `AssetName` varchar(200) DEFAULT NULL,
  `FromDate` datetime DEFAULT NULL,
  `ToDate` datetime DEFAULT NULL,
  `DepreciationType` varchar(10) DEFAULT NULL,
  `Amount` decimal(18,2) DEFAULT NULL,
  `NormalRate` decimal(18,2) DEFAULT NULL,
  `AdditionRate` decimal(18,2) DEFAULT NULL,
  `TotalRate` decimal(18,2) DEFAULT NULL,
  `DepreciationDays` int(11) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `DepreciationMethod` varchar(10) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`),
  KEY `IDX_DATE` (`FromDate`,`ToDate`,`Companyid`) /*!80000 INVISIBLE */
) ENGINE=InnoDB AUTO_INCREMENT=2325142 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tbldepreciation_log`
--

DROP TABLE IF EXISTS `tbldepreciation_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tbldepreciation_log` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `assetid` int(11) DEFAULT NULL,
  `message` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf32;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tbldepreciationcalculation`
--

DROP TABLE IF EXISTS `tbldepreciationcalculation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tbldepreciationcalculation` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `AssetID` int(11) DEFAULT NULL,
  `OpeningGross` decimal(18,2) DEFAULT '0.00',
  `OpeningAccumalatedDep` decimal(18,2) DEFAULT '0.00',
  `DepRate` decimal(18,2) DEFAULT '0.00',
  `DepType` varchar(20) DEFAULT NULL,
  `ResidualValue` decimal(18,2) DEFAULT '0.00',
  `AssetExpiryDate` date DEFAULT NULL,
  `DepTillFromDate` decimal(18,2) DEFAULT '0.00',
  `DisposedTillFromDate` decimal(18,2) DEFAULT '0.00',
  `DisposalType` varchar(20) DEFAULT NULL,
  `DisposalAmt` decimal(18,2) DEFAULT '0.00',
  `DisposalDate` date DEFAULT NULL,
  `AssetAmt` decimal(18,2) DEFAULT '0.00',
  `FromDate` date DEFAULT NULL,
  `ToDate` date DEFAULT NULL,
  `NoOfDays` int(11) DEFAULT NULL,
  `DepreciationAmt` decimal(18,2) DEFAULT '0.00',
  `companyid` int(11) DEFAULT NULL,
  `AssetName` varchar(200) DEFAULT NULL,
  `NormalRate` decimal(18,2) DEFAULT '0.00',
  `AdditionRate` decimal(18,2) DEFAULT '0.00',
  `DepMethod` varchar(45) DEFAULT NULL,
  `Assetdtputuse` datetime DEFAULT NULL,
  `Usefullife` int(11) DEFAULT NULL,
  `dep_till_startdt` decimal(18,2) DEFAULT '0.00',
  `disp_gross_block` decimal(18,2) DEFAULT '0.00',
  `dep_on_disp_st_dt_to_sale_dt` decimal(18,2) DEFAULT '0.00',
  `dep_rev_on_disposal` decimal(18,2) DEFAULT '0.00',
  `amt_for_dep_calc` decimal(18,2) DEFAULT '0.00',
  `net_block_stdt` decimal(18,2) DEFAULT '0.00',
  `dep_for_period` decimal(18,2) DEFAULT '0.00',
  `net_block_endt` decimal(18,2) DEFAULT '0.00',
  `dep_rev_disposal` decimal(18,2) DEFAULT '0.00' COMMENT 'Depreciation Reversed on Disposal',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf32;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tbldepreciationrequest`
--

DROP TABLE IF EXISTS `tbldepreciationrequest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tbldepreciationrequest` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `CompanyID` int(11) DEFAULT NULL,
  `UserID` int(11) DEFAULT NULL,
  `StartDateTime` datetime DEFAULT NULL,
  `EndDateTime` datetime DEFAULT NULL,
  `InProcess` int(11) DEFAULT NULL,
  `StartDate` date DEFAULT NULL,
  `EndDate` date DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=59 DEFAULT CHARSET=utf32;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tbldepworking`
--

DROP TABLE IF EXISTS `tbldepworking`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tbldepworking` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `AssetID` int(11) NOT NULL,
  `AssetNo` varchar(45) DEFAULT NULL,
  `FromDate` datetime DEFAULT NULL,
  `ToDate` datetime DEFAULT NULL,
  `EffFrom` datetime DEFAULT NULL,
  `EffTo` datetime DEFAULT NULL,
  `OpGross` decimal(18,2) DEFAULT NULL,
  `OpDep` decimal(19,2) DEFAULT NULL,
  `Amount` decimal(18,2) DEFAULT NULL,
  `DepForPeriod` decimal(18,2) DEFAULT NULL,
  `DisposalDate` datetime DEFAULT NULL,
  `Days` int(11) DEFAULT NULL,
  `DispoDep` decimal(18,2) DEFAULT NULL COMMENT 'To Be Reversed',
  `Method` varchar(45) DEFAULT NULL,
  `DepRate` decimal(18,2) DEFAULT NULL,
  `RowType` varchar(45) NOT NULL COMMENT 'to identify if this is dep or disposal dep calculation row',
  `DisposalAmtTillDate` decimal(18,2) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=34463 DEFAULT CHARSET=utf8 COMMENT='Working Table for Dep Calculation';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tbldgroup`
--

DROP TABLE IF EXISTS `tbldgroup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tbldgroup` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `AGrpID` int(11) DEFAULT NULL,
  `BGrpID` int(11) DEFAULT NULL,
  `CGrpID` int(11) DEFAULT NULL,
  `DGroupName` varchar(150) DEFAULT NULL,
  `NormalRate` int(11) DEFAULT NULL,
  `AdditionalRate` int(11) DEFAULT NULL,
  `TotalRate` int(11) DEFAULT NULL,
  `DepMethod` varchar(10) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tbldisposal`
--

DROP TABLE IF EXISTS `tbldisposal`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tbldisposal` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `AssetId` int(11) DEFAULT NULL,
  `AssetName` varchar(200) DEFAULT NULL,
  `VoucherNo` varchar(50) DEFAULT NULL,
  `BillNo` varchar(50) DEFAULT NULL,
  `VoucherDate` datetime DEFAULT NULL,
  `DisposalDate` datetime DEFAULT NULL,
  `BillDate` datetime DEFAULT NULL,
  `DisposalType` varchar(10) DEFAULT NULL,
  `Remarks` varchar(200) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `Qty` int(11) DEFAULT NULL,
  `CustomerName` varchar(200) DEFAULT NULL,
  `DisposalAmount` decimal(18,2) DEFAULT NULL,
  `GrossAmount` decimal(18,2) DEFAULT NULL,
  `OpAccumulatedDepTill` decimal(18,2) DEFAULT NULL,
  `DepChargedFrom` decimal(18,2) DEFAULT NULL,
  `OpAccumulatedDep` decimal(18,2) DEFAULT NULL,
  `DepForFromDtToDt` decimal(18,2) DEFAULT NULL,
  `TotalDepreciation` decimal(18,2) DEFAULT NULL,
  `WDvDisposedOff` decimal(18,2) DEFAULT NULL,
  `ProfitLoss` decimal(18,2) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  `DisposalCategory` varchar(1000) DEFAULT NULL,
  `SaleValue` decimal(18,2) DEFAULT '0.00',
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=10703 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblemployee`
--

DROP TABLE IF EXISTS `tblemployee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblemployee` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(100) DEFAULT NULL,
  `LastName` varchar(100) DEFAULT NULL,
  `MiddleName` varchar(100) DEFAULT NULL,
  `Emailid` varchar(100) DEFAULT NULL,
  `Mobileno` varchar(45) DEFAULT NULL,
  `Address1` varchar(200) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedUserid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedUserid` int(11) DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `EmpId` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`),
  UNIQUE KEY `EmpId_UNIQUE` (`EmpId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf32;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblemployeeasset`
--

DROP TABLE IF EXISTS `tblemployeeasset`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblemployeeasset` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `EmpId` int(11) DEFAULT NULL,
  `AssetId` int(11) DEFAULT NULL,
  `IssueDate` datetime DEFAULT NULL,
  `AssetRecievedFlag` varchar(45) DEFAULT NULL,
  `Companyid` varchar(45) DEFAULT NULL,
  `CreatedUserid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedUserid` int(11) DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RecievedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`),
  KEY `EmpId_idx` (`EmpId`),
  KEY `AssetId_idx` (`AssetId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf32;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblinsurance`
--

DROP TABLE IF EXISTS `tblinsurance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblinsurance` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FromDate` datetime DEFAULT NULL,
  `ToDate` datetime DEFAULT NULL,
  `EMailID` varchar(200) DEFAULT NULL,
  `PolicyDetails` varchar(150) DEFAULT NULL,
  `Remarks` varchar(200) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblitdepreciation`
--

DROP TABLE IF EXISTS `tblitdepreciation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblitdepreciation` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ClientID` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `FromDate` datetime DEFAULT NULL,
  `ToDate` datetime DEFAULT NULL,
  `DepreciationRate` decimal(18,2) DEFAULT NULL,
  `Amount` decimal(18,2) DEFAULT NULL,
  `Opwdv` decimal(18,2) DEFAULT NULL,
  `Additionbefore` decimal(18,2) DEFAULT NULL,
  `AdditionAfter` decimal(18,2) DEFAULT NULL,
  `Disposal` decimal(18,2) DEFAULT NULL,
  `Total` decimal(18,2) DEFAULT NULL,
  `DepreciationAmount` decimal(18,2) DEFAULT NULL,
  `WdvAtend` decimal(18,2) DEFAULT NULL,
  `AGrpID` int(11) DEFAULT NULL,
  `BGrpID` int(11) DEFAULT NULL,
  `CGrpID` int(11) DEFAULT NULL,
  `DGrpID` int(11) DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  `AdditionDephalf` decimal(18,2) DEFAULT NULL,
  `AdditionDepfull` decimal(18,2) DEFAULT NULL,
  `Yearhalf` decimal(18,2) DEFAULT NULL,
  `Yearfull` decimal(18,2) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf32;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblitgroup`
--

DROP TABLE IF EXISTS `tblitgroup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblitgroup` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `GroupName` varchar(150) DEFAULT NULL,
  `DepRate` decimal(18,2) DEFAULT NULL,
  `OPWDV` decimal(18,2) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `DepMethod` varchar(45) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblitperiod`
--

DROP TABLE IF EXISTS `tblitperiod`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblitperiod` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FromDate` datetime DEFAULT NULL,
  `ToDate` datetime DEFAULT NULL,
  `Months` int(11) DEFAULT NULL,
  `DepFlag` varchar(10) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `PeriodlockFlag` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tbllicense`
--

DROP TABLE IF EXISTS `tbllicense`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tbllicense` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Company_Creation_Count` int(11) DEFAULT NULL,
  `Valid_From` datetime DEFAULT NULL,
  `Valid_Till` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblloan`
--

DROP TABLE IF EXISTS `tblloan`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblloan` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FromDate` datetime DEFAULT NULL,
  `ToDate` datetime DEFAULT NULL,
  `BankName` varchar(200) DEFAULT NULL,
  `Year` varchar(50) DEFAULT NULL,
  `Percent` decimal(18,2) DEFAULT NULL,
  `Amount` decimal(18,2) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tbllogin`
--

DROP TABLE IF EXISTS `tbllogin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tbllogin` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(100) DEFAULT NULL,
  `Password` varchar(100) DEFAULT NULL,
  `ConfirmPassword` varchar(100) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CompanyId` int(11) DEFAULT NULL,
  `Userlevel` varchar(45) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `ModifiedUserId` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `FirstName` varchar(100) DEFAULT NULL,
  `LastName` varchar(100) DEFAULT NULL,
  `EmailId` varchar(100) DEFAULT NULL,
  `MobileNo` varchar(45) DEFAULT NULL,
  `Address` varchar(200) DEFAULT NULL,
  `AppUserid` varchar(45) DEFAULT NULL,
  `IsAppAccess` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8 COMMENT='		';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblperiod`
--

DROP TABLE IF EXISTS `tblperiod`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblperiod` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FromDate` datetime DEFAULT NULL,
  `ToDate` datetime DEFAULT NULL,
  `Months` int(11) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblsubamc`
--

DROP TABLE IF EXISTS `tblsubamc`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblsubamc` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AssetId` int(11) DEFAULT NULL,
  `AssetDescription` varchar(1000) DEFAULT NULL,
  `CapitalisedAmount` decimal(18,2) DEFAULT NULL,
  `AmcId` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `AmcId_idx` (`AmcId`),
  CONSTRAINT `AmcId` FOREIGN KEY (`AmcId`) REFERENCES `tblamc` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblsubbatch`
--

DROP TABLE IF EXISTS `tblsubbatch`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblsubbatch` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BatchId` int(11) DEFAULT NULL,
  `LocAId` int(11) DEFAULT NULL,
  `LocBId` int(11) DEFAULT NULL,
  `LocCId` int(11) DEFAULT NULL,
  `LocAName` varchar(200) DEFAULT NULL,
  `LocBName` varchar(200) DEFAULT NULL,
  `LocCName` varchar(200) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `ID_UNIQUE` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblsubinsurance`
--

DROP TABLE IF EXISTS `tblsubinsurance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblsubinsurance` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AssetId` int(11) DEFAULT NULL,
  `AssetDescription` varchar(1000) DEFAULT NULL,
  `CapitalisedAmount` decimal(18,2) DEFAULT NULL,
  `InsuranceId` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `InsuranceId_idx` (`InsuranceId`),
  CONSTRAINT `InsuranceId` FOREIGN KEY (`InsuranceId`) REFERENCES `tblinsurance` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblsubloan`
--

DROP TABLE IF EXISTS `tblsubloan`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblsubloan` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `LoanId` int(11) DEFAULT NULL,
  `AssetId` int(11) DEFAULT NULL,
  `AssetDescription` varchar(200) DEFAULT NULL,
  `CapitalisedAmount` decimal(18,2) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`),
  KEY `Loanid_idx` (`LoanId`),
  CONSTRAINT `Loanid` FOREIGN KEY (`LoanId`) REFERENCES `tblloan` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblsubperiod`
--

DROP TABLE IF EXISTS `tblsubperiod`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblsubperiod` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `PeriodID` int(11) DEFAULT NULL,
  `FromDate` datetime DEFAULT NULL,
  `ToDate` datetime DEFAULT NULL,
  `PeriodLockFlag` varchar(10) DEFAULT NULL,
  `DepFlag` varchar(10) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`),
  KEY `IDX_SubPer_CoID` (`DepFlag`,`Companyid`)
) ENGINE=InnoDB AUTO_INCREMENT=150 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblsupplier`
--

DROP TABLE IF EXISTS `tblsupplier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblsupplier` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `SupplierCode` varchar(100) DEFAULT NULL,
  `SupplierName` varchar(150) DEFAULT NULL,
  `ContactPerson` varchar(150) DEFAULT NULL,
  `Address` varchar(200) DEFAULT NULL,
  `City` varchar(100) DEFAULT NULL,
  `PinCode` int(11) DEFAULT NULL,
  `PhoneNo` varchar(25) DEFAULT NULL,
  `MobileNo` varchar(30) DEFAULT NULL,
  `FaxNo` varchar(50) DEFAULT NULL,
  `ExciseRegNo` varchar(50) DEFAULT NULL,
  `ServiceTaxRegNo` varchar(50) DEFAULT NULL,
  `VATRegNo` varchar(50) DEFAULT NULL,
  `CSTRegNo` varchar(50) DEFAULT NULL,
  `AnyOtherregNo` varchar(50) DEFAULT NULL,
  `PANNo` varchar(50) DEFAULT NULL,
  `TANNo` varchar(50) DEFAULT NULL,
  `GSTNo` varchar(50) DEFAULT NULL,
  `EmailID` varchar(50) DEFAULT NULL,
  `ShopActLicence` varchar(50) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `Address2` varchar(100) DEFAULT NULL,
  `Address3` varchar(100) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  `Msemeno` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=1590 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tbluom`
--

DROP TABLE IF EXISTS `tbluom`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tbluom` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Unit` varchar(50) DEFAULT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `Companyid` int(11) DEFAULT NULL,
  `Modified_Userid` int(11) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `RefId` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblusereventlog`
--

DROP TABLE IF EXISTS `tblusereventlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tblusereventlog` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `TranDate` datetime NOT NULL,
  `UserID` int(11) NOT NULL,
  `Event` varchar(45) NOT NULL,
  `EventDescription` varchar(200) DEFAULT NULL,
  `SourcePage` varchar(45) DEFAULT NULL,
  `CompanyID` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=143 DEFAULT CHARSET=utf32;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `zzztbldepdifffeb`
--

DROP TABLE IF EXISTS `zzztbldepdifffeb`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `zzztbldepdifffeb` (
  `assetno` int(11) NOT NULL,
  `assetid` int(11) DEFAULT NULL,
  `amount` decimal(18,2) DEFAULT NULL,
  PRIMARY KEY (`assetno`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Disposal Diff in FEB 2021';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `zzztbldepdiffjan`
--

DROP TABLE IF EXISTS `zzztbldepdiffjan`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `zzztbldepdiffjan` (
  `assetno` int(11) NOT NULL,
  `assetid` int(11) DEFAULT NULL,
  `amount` decimal(18,2) DEFAULT NULL,
  PRIMARY KEY (`assetno`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `zzztblopdepdiff`
--

DROP TABLE IF EXISTS `zzztblopdepdiff`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `zzztblopdepdiff` (
  `AssetNo` int(11) NOT NULL,
  `OpDepDiff` decimal(18,2) DEFAULT NULL,
  `AssetID` int(11) DEFAULT NULL,
  PRIMARY KEY (`AssetNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping routines for database 'galleghar'
--
/*!50003 DROP PROCEDURE IF EXISTS `allocationreport` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `allocationreport`(IN companyid int ,IN startdate date)
BEGIN


/*

"AssetNo", "AssetName", "VoucherDate", "Date Put To Use ",
                    "SupplierName", "Qty", "Location", "SubLocation",
                    "Sub_SubLocation", "IssueDate", "Amount Capitalised"


*/

 
 declare v_assetid int;

 declare v_locationid int;
 declare v_sublocationid int;
 declare v_sub_sublocationid int;
 declare v_issuedate datetime;

            
   DROP TEMPORARY TABLE IF EXISTS temp_report_final ;
    CREATE TEMPORARY TABLE temp_report_final (
		
        
			assetid  INT NOT NULL PRIMARY KEY,
			companyid INT NOT NULL,
            AssetNo text,
          AssetName text,
          AssetIdentification text,
			VoucherDate Datetime,
			Dateputtouse Datetime,
		    SupplierName text,   
             Qty int,
			Location text,
            Sublocation text,
              Sub_Sublocation text,
          --  IssueDate Datetime,
            AmountCapitalised decimal(18,2)
           
            );
            
            insert into temp_report_final (assetid,companyid,
			AssetNo , AssetName,AssetIdentification ,VoucherDate ,Dateputtouse ,Qty ,SupplierName,
				Location ,Sublocation ,Sub_Sublocation ,AmountCapitalised)
			select id, companyid,AssetNo,AssetName,AssetIdentificationNo,VoucherDate,DtPutToUse,Qty,null SupplierName
				,null Location,null Sublocation,null Sub_Sublocation,AmountCapitalisedCompany from tblassets where companyid=companyid and disposalflag=0  and VoucherDate
                <=startdate;

           update temp_report_final set SupplierName =(select (A.SupplierName) from tblsupplier A inner join tblassets D on A.id=D.Supplierno and A.companyid=D.companyid where D.id=assetid) ;
           update temp_report_final set Location = (select (A.ALocationName) from tblalocation A inner join tblassets D on A.id=D.LocAID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
           update temp_report_final set Sublocation =(select (A.BLocationName) from tblblocation A inner join tblassets D on A.id=D.LocBID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0; 
           update temp_report_final set Sub_Sublocation =(select (A.CLocationName) from tblclocation A inner join tblassets D on A.id=D.LocCID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
		
        
        
        
		select * from temp_report_final; /* repoort output */
        

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `assettrackingreport` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
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
			select CA.AssetID,CA.alocid, CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo,A.MRRNo,CA.`Date`,A.SrNo
				,A.Remarks,null ALocName,A.Model
                from tblchildlocation CA inner join tblassets A on CA.AssetID=A.id
                where CA.companyid=companyid and A.disposalflag=0 
               and  cast(A.assetno as unsigned) >= cast(fromassetno as unsigned) and cast(A.assetno as unsigned) <= cast(toassetno as unsigned)
                and CA.ALocID=alocid and CA.`Date` >= startdate and CA.`Date`<= enddate ;
					
                   
          
		
        
        
        
        -- fromassetno=0toassetno=0alocid=0
            elseif (companyid!=0 and startdate!='' and enddate!='' and fromassetno='' and toassetno='' and alocid=0)
            then
         
            insert into temp_report_final (assetid,alocid,companyid,
			AssetNo , AssetName ,AssetIdentificationNo,SystemAssetId ,IssueDate ,SrNo ,Remarks,
				ALocName ,Model) 
			select CA.assetid,CA.alocid, CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo, A.MRRNo,CA.`Date`,A.SrNo
				,A.Remarks,null ALocName,A.Model
                from tblchildlocation CA inner join tblassets A on CA.AssetID=A.id
                where CA.companyid=companyid and A.disposalflag=0 
            
                and CA.`Date`>=startdate and CA.`Date`<=enddate;

        
          
          
		
        
        
        -- startdate=null enddate=null alocid=0
           elseif (companyid!=0 and startdate='' and enddate='' and fromassetno!='' and toassetno!='' and alocid=0)
            then
         
            insert into temp_report_final (assetid,alocid,companyid,
			AssetNo , AssetName ,AssetIdentificationNo,SystemAssetId ,IssueDate ,SrNo ,Remarks,
				ALocName ,Model) 
			select CA.assetid,CA.alocid, CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo,A.MRRNo,CA.`Date`,A.SrNo
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
			select CA.assetid, CA.alocid,CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo,A.MRRNo,CA.`Date`,A.SrNo
				,A.Remarks,null ALocName,A.Model
                from tblchildlocation CA inner join tblassets A on CA.AssetID=A.id
                where CA.companyid=companyid and A.disposalflag=0 
          
                and CA.ALocID=alocid and CA.`Date`>=startdate and CA.`Date`<=enddate;

      
          
		
        
        
        
                -- startdate=null enddate=null
           elseif (companyid!=0 and startdate='' and enddate='' and fromassetno!='' and toassetno!='' and alocid!=0)
            then
            insert into temp_report_final (assetid,alocid,companyid,
			AssetNo , AssetName ,AssetIdentificationNo,SystemAssetId ,IssueDate ,SrNo ,Remarks,
				ALocName ,Model) 
			select CA.assetid, CA.alocid,CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo,A.MRRNo,CA.`Date`,A.SrNo
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
			select CA.assetid,CA.alocid, CA.companyid,A.AssetNo,A.AssetName,A.AssetIdentificationNo,A.MRRNo,CA.`Date`,A.SrNo
				,A.Remarks,null ALocName,A.Model
                from tblchildlocation CA inner join tblassets A on CA.AssetID=A.id
                where CA.companyid=companyid and A.disposalflag=0 and CA.ALocID=alocid ;
                  

        End if;
       
		select * from temp_report_final; /* repoort output */
        

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Disposal` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `Disposal`(IN companyid int )
BEGIN


 
 


            
   DROP TEMPORARY TABLE IF EXISTS temp_report_final ;
    CREATE TEMPORARY TABLE temp_report_final (
		
			Id INT,
            companyid INT,
			DisposalDate text,
            DisposalType text,
            Remarks text,
            AssetNo text,
			AssetName text,
		    DisposalAmount decimal(18,2), 
             Qty int
			
            );
            
            --   for assets table
            insert into temp_report_final (Id,companyid,DisposalDate,DisposalType,Remarks,AssetNo,
			AssetName , DisposalAmount,Qty )
			select D.ID,D.Companyid,DATE_FORMAT(D.DisposalDate,'%d/%m/%Y'),D.DisposalType,D.Remarks,A.AssetNo,D.AssetName,D.DisposalAmount,D.Qty
         
				 from tblDisposal D inner join  tblassets A on D.AssetId=A.id  and A.companyid=D.companyid where companyid=companyid ;

        
        
		select * from temp_report_final; /* repoort output */
        

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `farreport` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `farreport`(IN companyid int ,IN startdate date)
BEGIN


/*

"AssetNo", "AssetName", "VoucherDate", "Date Put To Use ",
                    "SupplierName", "Qty", "Location", "SubLocation",
                    "Sub_SubLocation", "IssueDate", "Amount Capitalised"


*/

 
 declare v_assetid int;


            
   DROP TEMPORARY TABLE IF EXISTS temp_report_final ;
    CREATE TEMPORARY TABLE temp_report_final (
		
        
			assetid  INT ,
			companyid INT NOT NULL,
            AGroupName text,
            BGroupName text,
            CGroupName text,
            DGroupName text,
            AssetNo text,
			AssetName text,
            AssetIdentificationNo text,
            VoucherNo text,
			VoucherDate Datetime,
			DTPutUseCompany Datetime,
		    SupplierName text,   
             Qty int,
			DepRate decimal(18,2),
			DepMethod text,
            AmountCapitalisedCompany decimal(18,2),
          AmountCapitalisedIT decimal(18,2),
		DepreciationAmount decimal(18,2),
        NetBalance decimal(18,2),
		TotalCedit decimal(18,2),
       InvoiceAmount decimal(18,2),
       TransactionType text
       
      
            );
            
            --   for assets table
            insert into temp_report_final (assetid,companyid,AGroupName,BGroupName,CGroupName,DGroupName,
			AssetNo , AssetName,AssetIdentificationNo ,VoucherNo,VoucherDate ,DTPutUseCompany ,SupplierName,Qty,
				DepRate,DepMethod ,AmountCapitalisedCompany ,AmountCapitalisedIT ,DepreciationAmount,NetBalance,TotalCedit,InvoiceAmount,
                TransactionType)
			select id, companyid,null AGroupName,null BGroupName,null CGroupName,null DGroupName,AssetNo,AssetName,AssetIdentificationNo,VoucherNo,
            VoucherDate,DtPutToUse,null SupplierName,Qty,TotalRate,DepreciationMethod,AmountCapitalisedCompany,AmountCapitalisedIT,OPAccDepreciation,
            (AmountCapitalisedCompany-OPAccDepreciation),TotalCredit,InvoiceAmt,"Purchase"
				 from tblassets where companyid=companyid and VoucherDate<=startdate ;

           update temp_report_final set SupplierName =(select (A.SupplierName) from tblsupplier A 
			inner join tblassets D on A.id=D.Supplierno and A.companyid=D.companyid where D.id=assetid ) 
             where assetid > 0;
           update temp_report_final set AGroupName = (select (A.AGroupName) from tblagroup A inner join tblassets D on A.id=D.AGroupID  and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
           update temp_report_final set BGroupName =(select (A.BGroupName) from tblbgroup A inner join tblassets D on A.id=D.BGroupID  and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0; 
           update temp_report_final set CGroupName =(select (A.CGroupName) from tblcgroup A inner join tblassets D on A.id=D.CGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
		 update temp_report_final set DGroupName =(select (A.DGroupName) from tbldgroup A inner join tblassets D on A.id=D.DGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
		
        /*for addition 
        
        logic
        
        */
         -- for disposal 
         
          insert into temp_report_final (assetid,companyid,AGroupName,BGroupName,CGroupName,DGroupName,
			AssetNo ,AssetName,AssetIdentificationNo ,VoucherNo,VoucherDate ,DTPutUseCompany ,SupplierName,Qty,
				DepRate,DepMethod ,AmountCapitalisedCompany ,AmountCapitalisedIT ,DepreciationAmount,NetBalance,TotalCedit,InvoiceAmount,
                TransactionType)
			select assetid, companyid,null AGroupName,null BGroupName,null CGroupName,null DGroupName,null AssetNo,D.AssetName,A.AssetIdentificationNo,D.VoucherNo,
            D.VoucherDate,D.DisposalDate,null SupplierName,(D.Qty-A.Qty),A.TotalRate,A.DepreciationMethod,(D.GrossAmount-A.AmountCapitalisedCompany),(D.GrossAmount-A.AmountCapitalisedIT),0,
            0,0,0,"disposal"
				 from tbldisposal D inner join  tblassets A on D.AssetId=A.id  and A.companyid=D.companyid where D.companyid=companyid and D.VoucherDate<=startdate ;

           update temp_report_final set SupplierName =(select (A.SupplierName) from tblsupplier A inner join tblassets D on A.id=D.Supplierno where D.id=assetid) ;
           update temp_report_final set AGroupName = (select (A.AGroupName) from tblagroup A inner join tblassets D on A.id=D.AGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
           update temp_report_final set BGroupName =(select (A.BGroupName) from tblbgroup A inner join tblassets D on A.id=D.BGroupID  and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0; 
           update temp_report_final set CGroupName =(select (A.CGroupName) from tblcgroup A inner join tblassets D on A.id=D.CGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
		 update temp_report_final set DGroupName =(select (A.DGroupName) from tbldgroup A inner join tblassets D on A.id=D.DGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
        
        
        -- depreciation
         insert into temp_report_final (assetid,companyid,AGroupName,BGroupName,CGroupName,DGroupName,
			AssetNo ,AssetName,AssetIdentificationNo ,VoucherNo ,VoucherDate ,DTPutUseCompany ,SupplierName,Qty,
				DepRate,DepMethod ,AmountCapitalisedCompany ,AmountCapitalisedIT ,DepreciationAmount,NetBalance,TotalCedit,InvoiceAmount,
                TransactionType)
			select assetid, companyid,null AGroupName,null BGroupName,null CGroupName,null DGroupName,A.AssetNo,D.AssetName,A.AssetIdentificationNo,null VoucherNo,
            D.FromDate,D.ToDate,null SupplierName,A.Qty,A.TotalRate,A.DepreciationMethod,A.AmountCapitalisedCompany,A.AmountCapitalisedIT,Amount,
            0,0,0,"depreciation"
				 from tbldepreciation D inner join  tblassets A on D.AssetId=A.id  and A.companyid=D.companyid where D.companyid=companyid and  D.ToDate <= startdate ;

           update temp_report_final set SupplierName =(select (A.SupplierName) from tblsupplier A 
           inner join tblassets D on A.id=D.Supplierno and A.companyid=D.companyid where D.id=assetid) 
           where assetid > 0;
           
           update temp_report_final set AGroupName = (select (A.AGroupName) from tblagroup A inner join tblassets D on A.id=D.AGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
           update temp_report_final set BGroupName =(select (A.BGroupName) from tblbgroup A inner join tblassets D on A.id=D.BGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0; 
           update temp_report_final set CGroupName =(select (A.CGroupName) from tblcgroup A inner join tblassets D on A.id=D.CGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
		 update temp_report_final set DGroupName =(select (A.DGroupName) from tbldgroup A inner join tblassets D on A.id=D.DGroupID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
        
		select * from temp_report_final; /* repoort output */
        

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `removedepreciation` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `removedepreciation`(IN companyid int ,
IN startdate date,IN enddate date )
BEGIN

declare delete_status text;
/*
delete from tbldepreciation where FromDate=startdate and ToDate=enddate and companyid=companyid;
 */
 set delete_status="Yes";

select delete_status;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `removeitdepreciation` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `removeitdepreciation`(IN companyid int ,
IN startdate date,IN enddate date )
BEGIN

declare delete_status text;

delete from tblitdepreciation where FromDate=startdate and ToDate=enddate and companyid=companyid;
 set delete_status="Yes";

select delete_status;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `singlelocationreport` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `singlelocationreport`(IN companyid int ,IN startdate date,IN alocid int ,IN blocid int,IN clocid int)
BEGIN


/*

"AssetNo", "AssetName", "VoucherDate", "Date Put To Use ",
                    "SupplierName", "Qty", "Location", "SubLocation",
                    "Sub_SubLocation", "IssueDate", "Amount Capitalised"


*/

 
 declare v_assetid int;

 declare v_alocid int;
 declare v_blocid int;
 declare v_clocid int;


            
   DROP TEMPORARY TABLE IF EXISTS temp_report_final ;
    CREATE TEMPORARY TABLE temp_report_final (
		
        
			assetid  INT NOT NULL PRIMARY KEY,
			companyid INT NOT NULL,
            AssetNo text,
          AssetName text,
			VoucherDate Datetime,
			Dateputtouse Datetime,
		    SupplierName text,   
             Qty int,
			Location text,
            Sublocation text,
              Sub_Sublocation text,
          --  IssueDate Datetime,
            AmountCapitalised decimal(18,2)
           
            );
            
            set v_alocid=alocid;
            set v_blocid=blocid;
            set v_clocid=clocid;
            
            insert into temp_report_final (assetid,companyid,
			AssetNo , AssetName ,VoucherDate ,Dateputtouse ,Qty ,SupplierName,
				Location ,Sublocation ,Sub_Sublocation ,AmountCapitalised)
			select id, companyid,AssetNo,AssetName,VoucherDate,DtPutToUse,Qty,null SupplierName
				,null Location,null Sublocation,null Sub_Sublocation,AmountCapitalised from tblassets where companyid=companyid and disposalflag=0  and VoucherDate
                <=startdate and LocAID=v_alocid and LocBID=v_blocid and LocCID=v_clocid;


           update temp_report_final set SupplierName =(select (A.SupplierName) from tblsupplier A inner join tblassets D on A.id=D.Supplierno and A.companyid=D.companyid where D.id=assetid) ;
           update temp_report_final set Location = (select (A.ALocationName) from tblalocation A inner join tblassets D on A.id=D.LocAID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
           update temp_report_final set Sublocation =(select (A.BLocationName) from tblblocation A inner join tblassets D on A.id=D.LocBID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0; 
           update temp_report_final set Sub_Sublocation =(select (A.CLocationName) from tblclocation A inner join tblassets D on A.id=D.LocCID and A.companyid=D.companyid where D.id=assetid) 
                where assetid > 0;
		
        
        
        
		select * from temp_report_final; /* repoort output */
        

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Usefullifereport` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
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
        
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `usp_calc_depriciationV2` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_calc_depriciationV2`(IN companyid int,IN startdate date, IN enddate date,IN userid int)
BEGIN
	
    
    declare days_in_year int;
	declare v_noofdays int;
     
    set days_in_year = 365;
    
    truncate table tbldepreciationcalculation;
    truncate table tbldepreciation_log;

    update tbldepreciationrequest set startdatetime = current_timestamp(), InProcess = 1
    where InProcess = -1
	and id > 0;
    
    /* ignore rows which are disposed of fully */
    /* Mandar 02-NOV-2020 use AmountCapitalized instead of OpeningGross*/
    /* changed logic to match asset count with clarion */
    /*
    insert into tbldepreciationcalculation
    (AssetId,AssetName,OpeningGross,OpeningAccumalatedDep,DepRate,DepType,ResidualValue,
    AssetExpiryDate,FromDate,ToDate,companyid,NormalRate,AdditionRate,DepMethod,Assetdtputuse,Usefullife)
    select id,AssetName,AmountCapitalised,OPAccDepreciation,TotalRate,'A',ResidualVal, ExpiryDate,startdate,
    enddate,companyid,Normalratae,AdditionalRate,DepreciationMethod,DtPutToUse,Usefullife
    from tblassets where id  not in (
					select assetid from tbldisposal
					where tbldisposal.VoucherDate < startdate  and tbldisposal.DisposalType ='Full'
			)
	and tblassets.DtPutToUse <= enddate;
    */
    
   insert into tbldepreciationcalculation
    (AssetId,AssetName,OpeningGross,OpeningAccumalatedDep,DepRate,DepType,ResidualValue,
    AssetExpiryDate,FromDate,ToDate,companyid,NormalRate,AdditionRate,DepMethod,Assetdtputuse,Usefullife,dep_rev_disposal)
    select id,AssetName,AmountCapitalised,OPAccDepreciation,TotalRate,'A',ResidualVal, ExpiryDate,startdate,
    enddate,companyid,Normalratae,AdditionalRate,DepreciationMethod,DtPutToUse,Usefullife,0
    from tblassets
	where  tblassets.DtPutToUse <= enddate;
    
    
   	update tbldepreciationcalculation
    set NoOfDays= 0
    where ID > 0;
    
        
      
        
		update tbldepreciationcalculation working
			inner join (
						select assetid,IFNULL(sum(DisposalAmount),0) as disposal_gross
					    from tbldisposal
                        where tbldisposal.companyid = companyid
						and  tbldisposal.disposaldate < startdate
						group by assetid
					) as disposal ON
					working.assetid = disposal.assetid
			set working.disp_gross_block = disposal.disposal_gross
            where working.id > 0;

	/*
			update tbldepreciationcalculation working
			inner join (
						select assetid,IFNULL(sum(TotalDepreciation),0) as disposal_dep
					    from tbldisposal
                        where tbldisposal.companyid = companyid
                        and  tbldisposal.disposaldate < startdate
						group by assetid
					) as disposal ON
					working.assetid = disposal.assetid
			set working.dep_rev_on_disposal = disposal.disposal_dep
            where working.id > 0;

-- Mandar 25 FEB 2021 Galleghar 
*/
    
		/*
			update tbldepreciationcalculation working
			inner join (
						select assetid,IFNULL(sum(Amount),0) as depriciation_openingamt
						from tbldepreciation
						where tbldepreciation.companyid = companyid
						and  tbldepreciation.ToDate < startdate
						group by assetid
					) as disposal ON
					working.assetid = disposal.assetid
			set working.dep_rev_on_disposal = disposal.disposal_dep
            where working.id > 0;
	*/

    
		update tbldepreciationcalculation working
			inner join (
						select assetid,IFNULL(sum(Amount),0) as depriciation_openingamt
						from tbldepreciation
						where tbldepreciation.companyid = companyid
						and  tbldepreciation.ToDate < startdate
						group by assetid
					) as depreciation ON
					working.assetid = depreciation.assetid
			set working.dep_till_startdt = depreciation.depriciation_openingamt
            where working.id > 0;


	/*dep_rev_disposal*/


	update tbldepreciationcalculation working
			inner join (
						select assetid,IFNULL(sum(OpAccumulatedDep),0) as OpAccumulatedDep
						from tbldisposal
						where tbldisposal.companyid = companyid
						and  tbldisposal.DisposalDate < startdate
						group by assetid
					) as tbldisposal ON
					working.assetid = tbldisposal.assetid
			set working.dep_rev_disposal = tbldisposal.OpAccumulatedDep
            where working.id > 0;



    
    select * from tbldepreciationcalculation;
    
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `usp_calc_depriciationV3` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_calc_depriciationV3`(IN companyid int,IN startdate date, IN enddate date,IN userid int)
BEGIN
	
    truncate table tbldepworking;
    
   insert into tbldepworking
    (	AssetID, AssetNo, FromDate, ToDate, EffFrom, EffTo, OpGross, OpDep, Amount, 
    DepForPeriod, DisposalDate, Days, DispoDep, Method, DepRate, RowType,DisposalAmtTillDate)
    select id,assetno,startdate,enddate,startdate,enddate,0,0,0,
    0,null,0 days ,0 DispoDep,DepreciationMethod, TotalRate,'DEP',0 DisposalAmtTillDate from tblassets;
   
   
		insert into tbldepworking
			(	AssetID, AssetNo, FromDate, ToDate, EffFrom, EffTo, OpGross, OpDep, Amount, 
			DepForPeriod, DisposalDate, Days, DispoDep, Method, DepRate, RowType)
            
			select tbldisposal.assetid,tblassets.assetno,startdate,enddate,tbldisposal.DisposalDate,enddate,0,0,tbldisposal.DisposalAmount,
			0,tbldisposal.DisposalDate,0 days ,0 DispoDep,DepreciationMethod, TotalRate,'DISP' from tbldisposal
            inner join tblassets on tbldisposal.assetid = tblassets.id
            where tbldisposal.DisposalDate >= startdate and tbldisposal.DisposalDate <= enddate;
            
            
   			    update tbldepworking
				inner join tblassets on tbldepworking.assetid =  tblassets.id
                set tbldepworking.OpGross = IFNULL(AmountCapitalisedCompany,0)
                where tblassets.companyid = companyid
                and tbldepworking.id > 0;  
    
    
				/* consider  op accumalted dep from tblasets */
			    update tbldepworking
				inner join tblassets on tbldepworking.assetid =  tblassets.id
                set tbldepworking.OpDep = IFNULL(OPAccDepreciation,0)
                where tblassets.companyid = companyid
                and tbldepworking.id > 0;  
                
              
              
          
              	update tbldepworking working
                inner join (
							select assetid,IFNULL(sum(Amount),0) as depriciation_tillFromDate
                            from tbldepreciation
                            where tbldepreciation.companyid = companyid
							and  tbldepreciation.FromDate < startdate 
                            group by assetid
						) as depreciation ON
                        working.assetid = depreciation.assetid
                set working.OpDep = working.OpDep + depreciation.depriciation_tillFromDate;
                
                
                update tbldepworking working
                inner join (
							select assetid,IFNULL(sum(DisposalAmount),0) as DisposalAmount_TillDate
                            from tbldisposal
                            where tbldisposal.companyid = companyid
							and  tbldisposal.DisposalDate < startdate 
                            group by assetid
						) as disposal ON
                        working.assetid = disposal.assetid
                set working.DisposalAmtTillDate =   disposal.DisposalAmount_TillDate;
                
            
            /* insert disposal records */
            
          
            /**/
              
                
		update tbldepworking set DisposalAmtTillDate =0 where DisposalAmtTillDate is null;
   
   select * from tbldepworking    order by AssetID, RowType ASC;
    
   
       
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `usp_calc_update_dep_table` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_calc_update_dep_table`(IN requestid int)
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
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `usp_ITDepreciationCalc` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
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
    
    
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `usp_report_Addition` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_report_Addition`(IN companyid int,IN startdate date, IN enddate date)
BEGIN


    
    /*DEClARE curAsset CURSOR FOR SELECT id FROM tblassets  where companyid=companyid and disposalflag=0 and id=18245;*/
    
        
    /*
		crete required temp tables
        
    
    */
   
  
    
    
    /*DEClARE curAsset CURSOR FOR SELECT id FROM tblassets  where companyid=companyid and disposalflag=0 and id=18245;*/
    
        
    /*
		crete required temp tables
        
    
    */
   
   
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
            OpeningQty integer,
            DisposedQtyTillFromDate integer
            
            
		);
        
	
                
		insert into temp_report_final (companyid,assetid,AGroupName,BGroupName,CGroupName,DGroupName,AssetNo,AssetIdentificationNo,AssetName,
			OpGross , Addition ,Disposal ,ClGross ,OpDep ,
				UpToDep ,DispoDep ,TotDep ,NetBalance,DepRate,DepMethod,voucherDate,VoucherNo,PONo,DTPutUse,
                Remarks,Qty,BillNo,BillDate,SrNo,Model,ALocName,BLocName,CLocName,SupplierName,OpeningQty)
			select companyid,ID,null agroupname,null BGroupName,null CGroupName,null DGroupName,AssetNo,AssetIdentificationNo,AssetName,0 OpGross,0 Addition,0 Disposal,0 ClGross,0 OpDep,
				0 UpToDep,0 DispoDep ,0 TotDep,0 NetBalance,TotalRate,DepreciationMethod,voucherDate,VoucherNo,PONo,DTPuttoUse,Remarks,
                 Qty,BillNo,BillDate,SrNo,Model,null ALocName,null BLocName,null CLocName,null SupplierName,Qty
                 from tblassets where companyid=companyid and  voucherdate  >= startdate and voucherdate <= enddate;
		

        
		        update temp_report_final
				inner join tblassets on temp_report_final.assetid =  tblassets.id
                set temp_report_final.Addition = IFNULL(AmountCapitalisedCompany,0)
                where tblassets.companyid = companyid
                and  tblassets.voucherdate >= startdate  and tblassets.voucherdate <= enddate
                and id > 0;
                
                
				update temp_report_final working
                inner join (
							select assetid,IFNULL(sum(Amount),0) as TotalDep
                            from tbldepreciation
                            where tbldepreciation.companyid = companyid
							and  tbldepreciation.FromDate >= startdate and tbldepreciation.ToDate <= enddate
                            group by assetid
						) as depreciation ON
                        working.assetid = depreciation.assetid
                set working.TotDep = depreciation.TotalDep;


		
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
        

		update temp_report_final set DisposedQtyTillFromDate = 0 where DisposedQtyTillFromDate is null and assetid > 0 ;

        
		select assetid , companyid ,AGroupName ,BGroupName , CGroupName ,DGroupName ,   
            CAST(AssetNo AS UNSIGNED INTEGER) AssetNo ,AssetIdentificationNo ,AssetName ,
			 OpGross ,Addition ,Disposal ,ClGross ,
            OpDep ,UpToDep ,DispoDep ,TotDep ,NetBalance ,            
            DepRate ,DepMethod ,CAST(voucherDate as DATE)  voucherDate,
            VoucherNo ,PONo , CAST(DTPutUse  as DATE)  DTPutUse,Remarks ,
            Qty ,BillNo ,CAST(BillDate as DATE) BillDate,SrNo ,           
            Model ,ALocName ,BLocName ,CLocName ,
            SupplierName ,OpeningQty ,
            DisposedQtyTillFromDate 
		from  temp_report_final 
		 where CAST(AssetNo AS UNSIGNED INTEGER)  > 0
		order by CAST(AssetNo AS UNSIGNED INTEGER)
		
				 
		/* select * from  temp_report_final where assetid = 7881 */ ;
		 
       
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `usp_report_Disposal` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_report_Disposal`(IN companyid int,IN startdate date, IN enddate date)
BEGIN


    
    /*DEClARE curAsset CURSOR FOR SELECT id FROM tblassets  where companyid=companyid and disposalflag=0 and id=18245;*/
    
        
    /*
		crete required temp tables
        
    
    */
   
  
    
    
    /*DEClARE curAsset CURSOR FOR SELECT id FROM tblassets  where companyid=companyid and disposalflag=0 and id=18245;*/
    
        
    /*
		crete required temp tables
        
    
    */
   
   
   DROP TEMPORARY TABLE IF EXISTS temp_report_final ;
    CREATE TEMPORARY TABLE temp_report_final (
			id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
			assetid  INT NOT NULL,
			companyid INT NOT NULL,
            AGroupName text,
			BGroupName text,
			CGroupName text,
		    DGroupName text,   
			AssetNo text,
			AssetIdentificationNo text,
			AssetName text,
            Disposal decimal(18,2),
            DispoDep decimal(18,2),
            Qty INT,
			SrNo text,           
            Model text,
            ALocName text,
            BLocName text,
            CLocName text,
			DisposalDate datetime
		);
        
        insert into  temp_report_final (assetid,companyid,AGroupName,BGroupName,CGroupName,DGroupName,
			AssetNo,AssetIdentificationNo,AssetName,Disposal,DispoDep,Qty,SrNo,Model,ALocName,BLocName,CLocName,DisposalDate
        )
        select assetid,companyid,'' AGroupName, '' BGroupName, '' CGroupName, '' DGroupName,
        0 AssetNo, '' AssetIdentificationNo, '' AssetName, DisposalAmount, OpAccumulatedDep, Qty,'' SrNo, '' Model,
        '' ALocName, '' BLocName, '' CLocName,DisposalDate
        from tbldisposal
		where tbldisposal.companyid = companyid
		and  tbldisposal.DisposalDate >= startdate  and tbldisposal.DisposalDate <= enddate;

         /*       
		insert into temp_report_final (companyid,assetid,AGroupName,BGroupName,CGroupName,DGroupName,AssetNo,AssetIdentificationNo,AssetName,
			OpGross , Addition ,Disposal ,ClGross ,OpDep ,
				UpToDep ,DispoDep ,TotDep ,NetBalance,DepRate,DepMethod,voucherDate,VoucherNo,PONo,DTPutUse,
                Remarks,Qty,BillNo,BillDate,SrNo,Model,ALocName,BLocName,CLocName,SupplierName,OpeningQty)
			select companyid,ID,null agroupname,null BGroupName,null CGroupName,null DGroupName,AssetNo,AssetIdentificationNo,AssetName,0 OpGross,0 Addition,0 Disposal,0 ClGross,0 OpDep,
				0 UpToDep,0 DispoDep ,0 TotDep,0 NetBalance,TotalRate,DepreciationMethod,voucherDate,VoucherNo,PONo,DTPuttoUse,Remarks,
                 Qty,BillNo,BillDate,SrNo,Model,null ALocName,null BLocName,null CLocName,null SupplierName,Qty
                 from tblassets where companyid=companyid ;
           */      
                 /*and  DtPutToUse  >= startdate and DtPutToUse <= enddate;*/
		

			
		        update temp_report_final
				inner join tblassets on temp_report_final.assetid =  tblassets.id
                set temp_report_final.AssetNo = tblassets.AssetNo,
					temp_report_final.AssetIdentificationNo = tblassets.AssetIdentificationNo,
                    temp_report_final.AssetName = tblassets.AssetName,
                    temp_report_final.SrNo = tblassets.Srno,
                    temp_report_final.Model = tblassets.Model
                where tblassets.companyid = companyid
                and temp_report_final.id > 0;

		
        /*
              update temp_report_final
				inner join tbldisposal on temp_report_final.assetid =  tbldisposal.Assetid
                set temp_report_final.Disposal = IFNULL(DisposalAmount,0)
                where tbldisposal.companyid = companyid
                and  tbldisposal.voucherdate >= startdate  and tbldisposal.voucherdate <= enddate
                and id > 0;

		*/

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
                
                /*
                 update temp_report_final 
				set temp_report_final.SupplierName = (select (A.SupplierName) from tblsupplier A inner join tblassets D on A.id=D.SupplierNo where D.id=assetid) 
                where assetid > 0; 
        */

	/*
		update temp_report_final set DisposedQtyTillFromDate = 0 where DisposedQtyTillFromDate is null and assetid > 0 ;
		*/
        
        /*
		select assetid , companyid ,AGroupName ,BGroupName , CGroupName ,DGroupName ,   
            CAST(AssetNo AS UNSIGNED INTEGER) AssetNo ,AssetIdentificationNo ,AssetName ,
			 OpGross ,Addition ,Disposal ,ClGross ,
            OpDep ,UpToDep ,DispoDep ,TotDep ,NetBalance ,            
            DepRate ,DepMethod ,CAST(voucherDate as DATE)  voucherDate,
            VoucherNo ,PONo , CAST(DTPutUse  as DATE)  DTPutUse,Remarks ,
            Qty ,BillNo ,CAST(BillDate as DATE) BillDate,SrNo ,           
            Model ,ALocName ,BLocName ,CLocName ,
            SupplierName ,OpeningQty ,
            DisposedQtyTillFromDate 
		from  temp_report_final 
		 where CAST(AssetNo AS UNSIGNED INTEGER)  > 0
		order by CAST(AssetNo AS UNSIGNED INTEGER)
		*/
        
        select assetid ,
			companyid ,
            AGroupName ,
			BGroupName ,
			CGroupName ,
		    DGroupName ,   
			CAST(AssetNo AS UNSIGNED INTEGER) AssetNo ,
			AssetIdentificationNo ,
			AssetName ,
            Disposal ,
            DispoDep ,
            Qty ,
			SrNo ,           
            Model ,
            ALocName ,
            BLocName ,
            CLocName ,
			DisposalDate 
			from  temp_report_final 		 
		/* select * from  temp_report_final where assetid = 7881 */ ;
		 
       
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `usp_report_FAR` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_report_FAR`(IN companyid int,IN startdate date, IN enddate date)
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
            OpeningQty integer,
            DisposedQtyTillFromDate integer
            
            
		);
        
			
		
                
		insert into temp_report_final (companyid,assetid,AGroupName,BGroupName,CGroupName,DGroupName,AssetNo,AssetIdentificationNo,AssetName,
			OpGross , Addition ,Disposal ,ClGross ,OpDep ,
				UpToDep ,DispoDep ,TotDep ,NetBalance,DepRate,DepMethod,voucherDate,VoucherNo,PONo,DTPutUse,
                Remarks,Qty,BillNo,BillDate,SrNo,Model,ALocName,BLocName,CLocName,SupplierName,OpeningQty)
			select companyid,ID,null agroupname,null BGroupName,null CGroupName,null DGroupName,AssetNo,AssetIdentificationNo,AssetName,0 OpGross,0 Addition,0 Disposal,0 ClGross,0 OpDep,
				0 UpToDep,0 DispoDep ,0 TotDep,0 NetBalance,TotalRate,DepreciationMethod,voucherDate,VoucherNo,PONo,DTPuttoUse,Remarks,
                 Qty,BillNo,BillDate,SrNo,Model,null ALocName,null BLocName,null CLocName,null SupplierName,Qty
                 from tblassets where companyid=companyid and  DtPutToUse < enddate;


		

		insert into temp_report_working (v_assetid,v_OpGross ,v_Addition ,v_OpDep ,
        v_disposalopgross ,v_Disposalgross,v_disposalopdep,v_disposaldepforfromdttodt ,v_Disposalopaccumalted ,v_depriciation_openingamt ,v_depriciation_UptoDep )
        select id,0,0,0,0,0,0,0,0,0,0 from tblassets where companyid=companyid  and DtPutToUse < enddate;
        
        
        
		
	
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


				/*

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

				Commented BY Mandar to Make same column for disposal 04 MAR 2021
			*/
                   
				   
				update temp_report_working working
                inner join (
							select assetid,IFNULL(sum(DisposalAmount),0) DisposalAmount
                            from tbldisposal
                            where tbldisposal.companyid = companyid
							and  tbldisposal.voucherdate < startdate
                            group by assetid
						) as disposal ON
                        working.v_assetid = disposal.assetid
                set working.v_disposalopgross = disposal.DisposalAmount;

					

				/*
				set v_disposalopgross = 0;
            
				set v_disposalopgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal  
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tbldisposal.AssetId= v_assetid
                            and tbldisposal.voucherdate <= enddate and tbldisposal.voucherdate < startdate );
                  */          
		
        
		/* Diposal Depreciation till Start Date */
			
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
                
             
/*				 update temp_report_working set v_OpDep = v_OpDep - v_disposalopdep  where v_assetid > 0; -- Mandar 21 FEB 2021 */ 
                
             
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
					
					
					/* TODO Mandar for FEB 2021 FIX   */
					
					 update temp_report_working set v_OpDep =  v_OpDep  - v_disposalopdep where v_assetid > 0;
					
					
					/* TODO Mandar for FEB 2021 FIX   */
                 
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


	/* Mandar update disposed qty 16 FEB 2021 */
    
            	update temp_report_final final
                inner join (
						select assetid,IFNULL(sum(qty),0) DisposedQty
						from tbldisposal
						where tbldisposal.companyid = companyid
						and  tbldisposal.voucherdate < startdate
						group by assetid
						) as disposal ON
                        final.assetid = disposal.assetid
                set final.DisposedQtyTillFromDate = disposal.DisposedQty;



	
		/* Disposal Amount For the Period */
		update temp_report_final final
                inner join (
						select assetid,IFNULL(sum(DisposalAmount),0) DisposalAmount
						from tbldisposal
						where tbldisposal.companyid = companyid
						and  tbldisposal.DisposalDate >= startdate and tbldisposal.DisposalDate <= enddate
						group by assetid
						) as disposal ON
                        final.assetid = disposal.assetid
                set final.Disposal = disposal.DisposalAmount;



		/* Disposal Dep to be reversed Amount For the Period */
		
	   	  update temp_report_final final
                inner join (
						select assetid,IFNULL(sum(OpAccumulatedDep),0) DispoDep
						from tbldisposal
						where tbldisposal.companyid = companyid
						and  tbldisposal.DisposalDate >= startdate and tbldisposal.DisposalDate <= enddate
						group by assetid
						) as disposal ON
                        final.assetid = disposal.assetid
                set final.DispoDep = disposal.DispoDep;
				




				update temp_report_final 
				inner join temp_report_working on temp_report_final.assetid = temp_report_working.v_assetid
				set temp_report_final.OpGross =  temp_report_working.v_OpGross,
					temp_report_final.OpDep = temp_report_working.v_OpDep,
                    temp_report_final.Addition = temp_report_working.v_Addition,
                    temp_report_final.UpToDep = temp_report_working.v_depriciation_UptoDep;
					
			/* 21 feb 2021*/
			update temp_report_final  set OpDep =0 where  FLOOR(OpGross) = 0 
            and assetid  > 0
            and FLOOR(OpDep) = 0 ; /* added 15 apr 2021*/
            
					

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
        

      /* Op + add - disposal  */

		
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
        
        /*
                 update temp_report_final 
				set temp_report_final.CCDescription = (select (A.CCDescription) from tblacostcenter A inner join tblassets D on A.id=D.CostCenterAID where D.id=assetid) 
                where assetid > 0;
                
                 update temp_report_final 
				set temp_report_final.CCCode = (select (A.CCCode) from tblacostcenter A inner join tblassets D on A.id=D.CostCenterAID where D.id=assetid) 
                where assetid > 0;
			*/	
				
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
     

		/*
		delete from temp_report_final 
        where assetid in (select assetid from tbldisposal where DisposalType ='Full');
        */

		

		update temp_report_final  set DisposedQtyTillFromDate =0 where  DisposedQtyTillFromDate is null and assetid  > 0;
        
        update temp_report_final  set Qty = OpeningQty - DisposedQtyTillFromDate where assetid  > 0;
		
        
		delete from  temp_report_final where (OpGross =0 AND Addition =0 AND Disposal = 0 AND ClGross =0
		AND OpDep = 0 AND UpToDep = 0 AND DispoDep = 0 AND TotDep = 0 AND NetBalance =0)
        AND AssetID IN (Select AssetID FROM tbldisposal where DisposalDate < startdate);
    
        
        
		select assetid , companyid ,AGroupName ,BGroupName , CGroupName ,DGroupName ,   
            CAST(AssetNo AS UNSIGNED INTEGER) AssetNo ,AssetIdentificationNo ,AssetName ,
			 OpGross ,Addition ,Disposal ,ClGross ,
            OpDep ,UpToDep ,DispoDep ,TotDep ,NetBalance ,            
            DepRate ,DepMethod ,CAST(voucherDate as DATE)  voucherDate,
            VoucherNo ,PONo , CAST(DTPutUse  as DATE)  DTPutUse,Remarks ,
            Qty ,BillNo ,CAST(BillDate as DATE) BillDate,SrNo ,           
            Model ,ALocName ,BLocName ,CLocName ,
            SupplierName ,OpeningQty ,
            DisposedQtyTillFromDate 
		from  temp_report_final 
		 where CAST(AssetNo AS UNSIGNED INTEGER)  > 0
		order by CAST(AssetNo AS UNSIGNED INTEGER)
		
				 
		/* select * from  temp_report_final where assetid = 7881 */ ;
		 
       
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `usp_report_getCCSchedule` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_report_getCCSchedule`(IN companyid int,IN startdate date, IN enddate date)
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
        
	
    
    
    
        
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `usp_report_getFASDetail` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_report_getFASDetail`(IN companyid int,IN startdate date, IN enddate date,
IN _agroupid int , IN _bgroupid int, IN _cgroupid int, _dgroupid int)
BEGIN


    declare finished int default 0;
    
	declare v_agroupid int; 
        
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
    
    
    
    /*
		crete required temp tables
        
    
    */
   
      DROP TEMPORARY TABLE IF EXISTS temp_report_detail ;
    CREATE TEMPORARY TABLE temp_report_detail (
			id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
            agroupid int,
            bgroupid int,
            cgroupid int,
            dgroupid int,
            OpGross decimal(18,2)  null,
            Addition decimal(18,2) null,
            Disposal decimal(18,2) null,
            ClGross decimal(18,2) null,
            OpDep decimal(18,2) null,
            UpToDep decimal(18,2) null,
            DispoDep decimal(18,2) null,
            TotDep decimal(18,2) null,
            NetBalance decimal(18,2) null
		);
   
        
	
                
		insert into temp_report_detail (agroupid,bgroupid,cgroupid,dgroupid,
			OpGross , Addition ,Disposal ,ClGross ,OpDep ,
				UpToDep ,DispoDep ,TotDep ,NetBalance)
			values  (_agroupid,_bgroupid,_cgroupid,_dgroupid,0,0,0,0,0,0,0,0,0);
		

	
	
            /*Logic inside loop Start */
			/*SELECT v_agroupid; */
            
            set v_OpGross = 0;
            set v_OpGross = (select IFNULL(sum(AmountCapitalisedCompany),0) from tblassets
							where Companyid = companyid 
                            and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                           and voucherdate <= startdate);
                            
			
		
                            
			set v_Addition = 0;
            set v_Addition = (select IFNULL(sum(AmountCapitalisedCompany),0) from tblassets
							where Companyid = companyid 
							and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                            and voucherdate >= startdate and voucherdate <= enddate);
                            



            set v_OpDep = 0;
            set v_OpDep = (select IFNULL(sum(OPAccDepreciation),0) from tblassets
							where Companyid = companyid 
							and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                            and  voucherdate <= startdate );


			
			set v_disposalopgross = 0;
            
			set v_disposalopgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal  
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
							and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                            and  tbldisposal.voucherdate < startdate  );
                            
		
        

			set v_disposalopdep = 0;
            
			set v_disposalopdep = (select IFNULL(sum(OpAccumulatedDep),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                          and  tbldisposal.voucherdate < startdate ) ;

		
        set v_disposaldepforfromdttodt =(select IFNULL(sum(DepForFromDtToDt),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                           and tbldisposal.voucherdate < startdate ) ;
			
                           set v_disposalopdep=v_disposalopdep+v_disposaldepforfromdttodt;
							set v_OpGross =  v_OpGross - v_disposalopgross;
							set v_OpDep =  v_OpDep -v_disposalopdep ;

			set v_Disposalgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                          and tbldisposal.voucherdate <= enddate  and tbldisposal.voucherdate >= startdate );
                      

		/*	set v_Disposalopaccumalted = (select IFNULL(sum(OpAccumulatedDep),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                           and tbldisposal.voucherdate <= enddate and tbldisposal.voucherdate >= startdate );*/


            
			set v_depriciation_openingamt  = 0;
            set v_depriciation_openingamt  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                            and tbldepreciation.FromDate < startdate and tbldepreciation.ToDate <= enddate);

			 set v_OpDep=v_OpDep+v_depriciation_openingamt;

			set v_depriciation_UptoDep  = 0;
            set v_depriciation_UptoDep  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tblassets.agroupid = _agroupid and tblassets.bgroupid = _bgroupid and tblassets.cgroupid = _cgroupid and tblassets.dgroupid = _dgroupid 
                            and tbldepreciation.FromDate >= startdate and tbldepreciation.ToDate<= enddate);


			


			UPDATE temp_report_detail 
				SET OpGross = v_OpGross
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid AND id > 0;
                


			UPDATE temp_report_detail
			SET 
				OpDep = v_OpDep
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid  AND id > 0;

			UPDATE temp_report_detail 
			SET 
				Addition = v_Addition
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid  AND id > 0;

			UPDATE temp_report_detail 
			SET 
				Disposal = v_Disposalgross
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid  AND id > 0;
			
          /*  UPDATE temp_report_detail 
			SET 
				DispoDep = v_Disposalopaccumalted
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid  AND id > 0;*/


			UPDATE temp_report_detail 
			SET 
				UpToDep = v_depriciation_UptoDep
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid  AND id > 0;



            
			UPDATE temp_report_detail 
			SET 
				ClGross = (OpGross + Addition) - Disposal
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid AND id > 0;


			UPDATE temp_report_detail 
			SET 
				TotDep = (OpDep + UpToDep) 
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid  AND id > 0;

			UPDATE temp_report_detail 
			SET 
				NetBalance = (clgross - TotDep)
			WHERE
				agroupid = _agroupid and bgroupid = _bgroupid and cgroupid = _cgroupid and dgroupid = _dgroupid  AND id > 0;
					   
      
            
        select * from temp_report_detail;	/*final output*/
        
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `usp_report_getFASSummary` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_report_getFASSummary`(IN companyid int,IN startdate date, IN enddate date)
BEGIN

	
declare v_StartDate datetime;
declare v_EndDate datetime;
    declare finished int default 0;
    
	declare v_agroupid int; 
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
    
    
    
    DEClARE curAGroup CURSOR FOR SELECT id FROM tblagroup;
    DECLARE CONTINUE HANDLER 
			FOR NOT FOUND SET finished = 1;
        
    /*
		crete required temp tables
        
    
    */
   
   DROP TEMPORARY TABLE IF EXISTS temp_report ;
    CREATE TEMPORARY TABLE temp_report (
			id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
            agropid int null,
            AGroupName varchar(2000),
            OpGross decimal(18,2)  null,
            Addition decimal(18,2) null,
            Disposal decimal(18,2) null,
            ClGross decimal(18,2) null,
            OpDep decimal(18,2) null,
            UpToDep decimal(18,2) null,
            DispoDep decimal(18,2) null,
            TotDep decimal(18,2) null,
            NetBalance decimal(18,2) null
		);
        
			
		
                set v_StartDate=Cast(startdate as Date);
                set v_EndDate=Cast(enddate as Date);
		insert into temp_report (agropid,AGroupName,
			OpGross , Addition ,Disposal ,ClGross ,OpDep ,
				UpToDep ,DispoDep ,TotDep ,NetBalance)
			select id, agroupname,0 OpGross,0 Addition,0 Disposal,0 ClGross,0 OpDep,
				0 UpToDep,0 DispoDep ,0 TotDep,0 NetBalance from tblagroup where companyid=companyid;

		

		
		OPEN curAGroup;
        getAGroup: LOOP
			FETCH curAGroup INTO v_agroupid;
				IF finished = 1 THEN 
					LEAVE getAGroup;
				END IF;
			
            /*Logic inside loop Start */
			/*SELECT v_agroupid; */
            
            set v_OpGross = 0;
            set v_OpGross = (select IFNULL(sum(AmountCapitalisedCompany),0) from tblassets
							where Companyid = companyid and agroupid = v_agroupid
                            and voucherdate < v_StartDate  and voucherdate <= v_EndDate );
                            
                            
			set v_Addition = 0;
            set v_Addition = (select IFNULL(sum(AmountCapitalisedCompany),0) from tblassets
							where Companyid = companyid and agroupid = v_agroupid
                            and voucherdate >= v_StartDate and voucherdate <= v_EndDate);
                            



            set v_OpDep = 0;
            set v_OpDep = (select IFNULL(sum(OPAccDepreciation),0) from tblassets
							where Companyid = companyid and agroupid = v_agroupid
                            and voucherdate <= v_EndDate and voucherdate <= v_StartDate );



			set v_disposalopgross = 0;
            
			set v_disposalopgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal  
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                            and tbldisposal.voucherdate <= v_EndDate and tbldisposal.voucherdate < v_StartDate );
                            
		

			set v_disposalopdep = 0;
            
			set v_disposalopdep = (select IFNULL(sum(OpAccumulatedDep),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                          and tbldisposal.voucherdate<= v_EndDate  and tbldisposal.voucherdate < v_StartDate ) ;
                          
			set v_disposaldepforfromdttodt =(select IFNULL(sum(DepForFromDtToDt),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid 
                           and tbldisposal.voucherdate < startdate ) ;
			
                           set v_disposalopdep=v_disposalopdep+v_disposaldepforfromdttodt;
		
	
							set v_OpGross =  v_OpGross - v_disposalopgross;
							set v_OpDep =  v_OpDep-v_disposalopdep ;

			set v_Disposalgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                          and tbldisposal.voucherdate <= v_EndDate  and tbldisposal.voucherdate >= v_StartDate );

		/*	set v_Disposalopaccumalted = (select IFNULL(sum(OpAccumulatedDep),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                           and tbldisposal.voucherdate <= v_EndDate and tbldisposal.voucherdate >= v_StartDate );*/

			
			
            
			set v_depriciation_openingamt  = 0;
            set v_depriciation_openingamt  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                            and tbldepreciation.FromDate < v_StartDate and tbldepreciation.ToDate <= v_EndDate);

			 set v_OpDep=v_OpDep+v_depriciation_openingamt;

			set v_depriciation_UptoDep  = 0;
            set v_depriciation_UptoDep  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                            and tbldepreciation.FromDate >= v_StartDate and tbldepreciation.ToDate<= v_EndDate);



			update temp_report set OpGross = v_OpGross where agropid  = v_agroupid and id > 0;
            update temp_report set OpDep = v_OpDep where agropid  = v_agroupid and id > 0;
            update temp_report set Addition = v_Addition where agropid  = v_agroupid and id > 0;
			update temp_report set Disposal = v_Disposalgross where agropid  = v_agroupid and id > 0;
		-- update temp_report set DispoDep = v_Disposalopaccumalted where agropid  = v_agroupid and id > 0;
            update temp_report set UpToDep = v_depriciation_UptoDep where agropid  = v_agroupid and id > 0;
            
            update temp_report set ClGross = (OpGross + Addition) - Disposal where agropid  = v_agroupid and id > 0;
			update temp_report set  TotDep= (OpDep + UpToDep)  where agropid  = v_agroupid and id > 0;
			update temp_report set  NetBalance= (clgross -TotDep)  where agropid  = v_agroupid and id > 0;
           
            
		
        /*Logic inside loop End */
        END LOOP getAGroup;
        CLOSE curAGroup;

            
        
		select * from temp_report; /* repoort output */
        
	
    
    
    
        
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `usp_report_getFASSummaryV1` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_report_getFASSummaryV1`(IN companyid int,IN startdate date, IN enddate date)
BEGIN

/* changes by Mandar 10 DEC 2020 */
	
declare v_StartDate datetime;
declare v_EndDate datetime;
    declare finished int default 0;
    
	declare v_agroupid int; 
    declare v_OpGross decimal(18,2);
	declare v_Addition decimal(18,2);
	declare v_Disposal decimal(18,2);
	declare v_ClGross decimal(18,2);
    declare v_OpDepAsset decimal(18,2); 
	declare v_UpToDep decimal(18,2);
	declare v_OpDep decimal(18,2);

    declare v_DepForPeriod decimal(18,2);
    
	declare v_DispoDep decimal(18,2);
	declare v_TotDep decimal(18,2);
	declare v_NetBalance decimal(18,2);
    
    
    declare v_disposalopgross decimal(18,2);
    declare v_disposalopdep decimal(18,2);
	/*declare v_disposaldepforfromdttodt decimal(18,2);*/
      
    declare v_Disposalgross decimal(18,2);
    declare v_Disposalopaccumalted decimal(18,2);
    
    
    declare v_depriciation_openingamt decimal(18,2);
    declare v_depriciation_UptoDep decimal(18,2);
    
    
    
    DEClARE curAGroup CURSOR FOR SELECT id FROM tblagroup;
    DECLARE CONTINUE HANDLER 
			FOR NOT FOUND SET finished = 1;
        
    /*
		crete required temp tables
        
    
    */
   
   DROP TEMPORARY TABLE IF EXISTS temp_report ;
    CREATE TEMPORARY TABLE temp_report (
			id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
            agropid int null,
            AGroupName varchar(2000),
            OpGross decimal(18,2)  null,
            Addition decimal(18,2) null,
            Disposal decimal(18,2) null,
            ClGross decimal(18,2) null,
            OpDepAsset decimal(18,2) null, /* at the time of system starting */
            UpToDep decimal(18,2) null, /* till from date*/
            OpDep decimal(18,2) null, /* at the time of system starting */
            DepForPeriod decimal(18,2) null, /* disposal for report period */
            DispoDep decimal(18,2) null,
            TotDep decimal(18,2) null,
            NetBalance decimal(18,2) null
		);
        
			
		
                set v_StartDate=Cast(startdate as Date);
                set v_EndDate=Cast(enddate as Date);
		insert into temp_report (agropid,AGroupName,
			OpGross , Addition ,Disposal ,ClGross ,OpDepAsset,UpToDep, OpDep ,
				 DepForPeriod,DispoDep ,TotDep ,NetBalance)
			select id, agroupname,0 OpGross,0 Addition,0 Disposal,0 ClGross, 0 OpDepAsset ,0 UpToDep,
				0 OpDep, 0  DepForPeriod ,0 DispoDep ,0 TotDep,0 NetBalance from tblagroup where companyid=companyid;

		

		
		OPEN curAGroup;
        getAGroup: LOOP
			FETCH curAGroup INTO v_agroupid;
				IF finished = 1 THEN 
					LEAVE getAGroup;
				END IF;
			
            /*Logic inside loop Start */
			/*SELECT v_agroupid; */
            
            set v_OpGross = 0;
            set v_OpGross = (select IFNULL(sum(AmountCapitalisedCompany),0) from tblassets
							where Companyid = companyid and agroupid = v_agroupid
                            and voucherdate < v_StartDate  and voucherdate <= v_EndDate );
                            
                            
			set v_Addition = 0;
            set v_Addition = (select IFNULL(sum(AmountCapitalisedCompany),0) from tblassets
							where Companyid = companyid and agroupid = v_agroupid
                            and voucherdate >= v_StartDate and voucherdate <= v_EndDate);
                            



            set v_OpDepAsset = 0;
            set v_OpDepAsset = (select IFNULL(sum(OPAccDepreciation),0) from tblassets
							where Companyid = companyid and agroupid = v_agroupid);
                            
                          


			set v_disposalopgross = 0;
            
			set v_disposalopgross = (select IFNULL(sum(GrossAmount),0) from tbldisposal  
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                            and tbldisposal.voucherdate <= v_EndDate and tbldisposal.voucherdate < v_StartDate );
                            
		

			set v_disposalopdep = 0;
            
			set v_disposalopdep = (select IFNULL(sum(OpAccumulatedDep),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                          and tbldisposal.voucherdate<= v_EndDate  and tbldisposal.voucherdate < v_StartDate ) ;
            
	       /* dep for from date and to date */
            
		  set v_DepForPeriod  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                            and tbldepreciation.FromDate >= v_StartDate and tbldepreciation.ToDate <= v_EndDate);
                            
         
            
           
            
				set v_Disposalgross = (select IFNULL(sum(DisposalAmount),0) from tbldisposal
							inner join tblassets on tbldisposal.AssetId = tblassets.id
							where tbldisposal.Companyid = companyid
                            and tblassets.agroupid = v_agroupid
                          and tbldisposal.voucherdate >= v_StartDate  and tbldisposal.voucherdate <= v_EndDate);
                          
            
          
		

			
			
        
			set v_depriciation_UptoDep  = 0;
            set v_depriciation_UptoDep  = (select IFNULL(sum(Amount),0) from tbldepreciation
							inner join tblassets on tbldepreciation.AssetId = tblassets.id
							where tbldepreciation.Companyid = companyid 
                            and tblassets.agroupid = v_agroupid
                            and tbldepreciation.ToDate < v_EndDate);


		set v_DispoDep = 0;
		set v_DispoDep = (select IFNULL(sum(DepForFromDtToDt),0) from tbldisposal
				inner join tblassets 
				on tbldisposal.AssetId = tblassets.id
				where tbldisposal.Companyid = companyid 
				and tblassets.agroupid = v_agroupid
			  and tbldisposal.DisposalDate >= v_StartDate   and tbldisposal.DisposalDate <= v_EndDate ) ;


			set v_OpDep = 0;
             
            set v_OpDep = v_OpDepAsset + v_depriciation_UptoDep;
			set v_TotDep = 0;
            set v_TotDep =  v_OpDep + v_DepForPeriod;
            
			 set v_TotDep =  (v_TotDep - v_DispoDep);

			update temp_report set OpGross = v_OpGross where agropid  = v_agroupid and id > 0;
            update temp_report set OpDep = v_OpDep where agropid  = v_agroupid and id > 0;
            update temp_report set Addition = v_Addition where agropid  = v_agroupid and id > 0;
			update temp_report set Disposal = v_Disposalgross where agropid  = v_agroupid and id > 0;
		    update temp_report set DispoDep = v_DispoDep where agropid  = v_agroupid and id > 0;
		    update temp_report set OpDepAsset = v_OpDepAsset where agropid  = v_agroupid and id > 0;
		    update temp_report set UpToDep = v_depriciation_UptoDep where agropid  = v_agroupid and id > 0;
            update temp_report set DepForPeriod = v_DepForPeriod where agropid  = v_agroupid and id > 0;
            
            update temp_report set  OpDep = v_OpDep where agropid  = v_agroupid and id > 0;
       
            update temp_report set  TotDep  = v_TotDep where agropid  = v_agroupid and id > 0;
            
            update temp_report set ClGross = (OpGross + Addition) - Disposal where agropid  = v_agroupid and id > 0;
		
			update temp_report set  NetBalance= (clgross -TotDep)  where agropid  = v_agroupid and id > 0;
           
        /*Logic inside loop End */
        END LOOP getAGroup;
        CLOSE curAGroup;

            
        
		select * from temp_report; /* repoort output */
        
	
        
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `usp_report_getFASSummaryV1_New` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `usp_report_getFASSummaryV1_New`(IN companyid int,IN startdate date, IN enddate date)
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
            OpeningQty integer,
            DisposedQtyTillFromDate integer
            
            
		);
        
			
		
                
		insert into temp_report_final (companyid,assetid,AGroupName,BGroupName,CGroupName,DGroupName,AssetNo,AssetIdentificationNo,AssetName,
			OpGross , Addition ,Disposal ,ClGross ,OpDep ,
				UpToDep ,DispoDep ,TotDep ,NetBalance,DepRate,DepMethod,voucherDate,VoucherNo,PONo,DTPutUse,
                Remarks,Qty,BillNo,BillDate,SrNo,Model,ALocName,BLocName,CLocName,SupplierName,OpeningQty)
			select companyid,ID,null agroupname,null BGroupName,null CGroupName,null DGroupName,AssetNo,AssetIdentificationNo,AssetName,0 OpGross,0 Addition,0 Disposal,0 ClGross,0 OpDep,
				0 UpToDep,0 DispoDep ,0 TotDep,0 NetBalance,TotalRate,DepreciationMethod,voucherDate,VoucherNo,PONo,DTPuttoUse,Remarks,
                 Qty,BillNo,BillDate,SrNo,Model,null ALocName,null BLocName,null CLocName,null SupplierName,Qty
                 from tblassets where companyid=companyid and  DtPutToUse < enddate;


		

		insert into temp_report_working (v_assetid,v_OpGross ,v_Addition ,v_OpDep ,
        v_disposalopgross ,v_Disposalgross,v_disposalopdep,v_disposaldepforfromdttodt ,v_Disposalopaccumalted ,v_depriciation_openingamt ,v_depriciation_UptoDep )
        select id,0,0,0,0,0,0,0,0,0,0 from tblassets where companyid=companyid  and DtPutToUse < enddate;
        
        
        
		
	
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
		
        
		/* Diposal Depreciation till Start Date */
			
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
                
             
/*				 update temp_report_working set v_OpDep = v_OpDep - v_disposalopdep  where v_assetid > 0; -- Mandar 21 FEB 2021 */ 
                
             
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
					
					
					/* TODO Mandar for FEB 2021 FIX   */
					
					 update temp_report_working set v_OpDep =  v_OpDep  - v_disposalopdep where v_assetid > 0;
					
					
					/* TODO Mandar for FEB 2021 FIX   */
                 
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


	/* Mandar update disposed qty 16 FEB 2021 */
    
            	update temp_report_final final
                inner join (
						select assetid,IFNULL(sum(qty),0) DisposedQty
						from tbldisposal
						where tbldisposal.companyid = companyid
						and  tbldisposal.voucherdate < startdate
						group by assetid
						) as disposal ON
                        final.assetid = disposal.assetid
                set final.DisposedQtyTillFromDate = disposal.DisposedQty;



	
		/* Disposal Amount For the Period */
		update temp_report_final final
                inner join (
						select assetid,IFNULL(sum(DisposalAmount),0) DisposalAmount
						from tbldisposal
						where tbldisposal.companyid = companyid
						and  tbldisposal.DisposalDate >= startdate and tbldisposal.DisposalDate <= enddate
						group by assetid
						) as disposal ON
                        final.assetid = disposal.assetid
                set final.Disposal = disposal.DisposalAmount;



		/* Disposal Dep to be reversed Amount For the Period */
		
	   	  update temp_report_final final
                inner join (
						select assetid,IFNULL(sum(OpAccumulatedDep),0) DispoDep
						from tbldisposal
						where tbldisposal.companyid = companyid
						and  tbldisposal.DisposalDate >= startdate and tbldisposal.DisposalDate <= enddate
						group by assetid
						) as disposal ON
                        final.assetid = disposal.assetid
                set final.DispoDep = disposal.DispoDep;
				




				update temp_report_final 
				inner join temp_report_working on temp_report_final.assetid = temp_report_working.v_assetid
				set temp_report_final.OpGross =  temp_report_working.v_OpGross,
					temp_report_final.OpDep = temp_report_working.v_OpDep,
                    temp_report_final.Addition = temp_report_working.v_Addition,
                    temp_report_final.UpToDep = temp_report_working.v_depriciation_UptoDep;
					
			
			update temp_report_final  set OpDep =0 where  FLOOR(OpGross) = 0 and assetid  > 0; /* 21 feb 2021*/
					

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
        

      /* Op + add - disposal  */

		
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
        
        /*
                 update temp_report_final 
				set temp_report_final.CCDescription = (select (A.CCDescription) from tblacostcenter A inner join tblassets D on A.id=D.CostCenterAID where D.id=assetid) 
                where assetid > 0;
                
                 update temp_report_final 
				set temp_report_final.CCCode = (select (A.CCCode) from tblacostcenter A inner join tblassets D on A.id=D.CostCenterAID where D.id=assetid) 
                where assetid > 0;
			*/	
				
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
     

		/*
		delete from temp_report_final 
        where assetid in (select assetid from tbldisposal where DisposalType ='Full');
        */

		

		update temp_report_final  set DisposedQtyTillFromDate =0 where  DisposedQtyTillFromDate is null and assetid  > 0;
        
        update temp_report_final  set Qty = OpeningQty - DisposedQtyTillFromDate where assetid  > 0;
        
        /*
		select assetid , companyid ,AGroupName ,BGroupName , CGroupName ,DGroupName ,   
            CAST(AssetNo AS UNSIGNED INTEGER) AssetNo ,AssetIdentificationNo ,AssetName ,
			 OpGross ,Addition ,Disposal ,ClGross ,
            OpDep ,UpToDep ,DispoDep ,TotDep ,NetBalance ,            
            DepRate ,DepMethod ,voucherDate,
            VoucherNo ,PONo ,DTPutUse ,Remarks ,
            Qty ,BillNo ,BillDate ,SrNo ,           
            Model ,ALocName ,BLocName ,CLocName ,
            SupplierName ,OpeningQty ,
            DisposedQtyTillFromDate 
		from  temp_report_final 
		 where CAST(AssetNo AS UNSIGNED INTEGER)  > 0
		order by CAST(AssetNo AS UNSIGNED INTEGER)
		*/
				 
		/* select * from  temp_report_final where assetid = 7881 */
		
        
      select AGroupName,SUM(OpGross) OpGross, SUM(Addition) Addition, SUM(Disposal) Disposal,SUM(ClGross) ClGross,
      SUM(OpDep) OpDep,SUM(UpToDep) UpToDep,SUM(DispoDep) DispoDep,
      SUM(TotDep) TotDep,
      SUM(NetBalance) NetBalance
      from temp_report_final
      where CAST(AssetNo AS UNSIGNED INTEGER)  > 0
      group by AGroupName;
	
        
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-05-28 11:49:33
