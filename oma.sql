-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: localhost    Database: oma
-- ------------------------------------------------------
-- Server version	8.0.40

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `barangayproduction`
--

DROP TABLE IF EXISTS `barangayproduction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `barangayproduction` (
  `ProductionID` int NOT NULL AUTO_INCREMENT,
  `Barangay` varchar(100) NOT NULL,
  `Area` decimal(10,2) NOT NULL,
  `Production` decimal(10,2) NOT NULL,
  `AverageYield` decimal(10,2) NOT NULL,
  `RecordDate` datetime NOT NULL,
  `CreatedDate` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ProductionID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `barangayproduction`
--

LOCK TABLES `barangayproduction` WRITE;
/*!40000 ALTER TABLE `barangayproduction` DISABLE KEYS */;
/*!40000 ALTER TABLE `barangayproduction` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `commodities`
--

DROP TABLE IF EXISTS `commodities`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `commodities` (
  `CommodityID` int NOT NULL AUTO_INCREMENT,
  `CommodityName` varchar(255) NOT NULL,
  `CommodityType` varchar(255) DEFAULT NULL,
  `UnitOfMeasurement` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`CommodityID`),
  UNIQUE KEY `CommodityName` (`CommodityName`)
) ENGINE=InnoDB AUTO_INCREMENT=101 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `commodities`
--

LOCK TABLES `commodities` WRITE;
/*!40000 ALTER TABLE `commodities` DISABLE KEYS */;
INSERT INTO `commodities` VALUES (1,'Coconut','Nut','Nut'),(2,'Cacao','Bean','Kilogram'),(3,'Rice','Grain','Kilogram'),(4,'Corn','Grain','Kilogram'),(5,'Live Stock','Animals','Kilogram'),(6,'Fish','Animals','Kilogram'),(7,'High value crops','Vegetable & Fruits','Kilogram'),(8,'Industrial Crop','Non-food crops','Kilogram'),(100,'Unknown','Unknown','Unknown');
/*!40000 ALTER TABLE `commodities` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `farmernotes`
--

DROP TABLE IF EXISTS `farmernotes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `farmernotes` (
  `NoteID` int NOT NULL AUTO_INCREMENT,
  `FarmerID` int DEFAULT NULL,
  `Note` text,
  `DateAdded` datetime DEFAULT NULL,
  PRIMARY KEY (`NoteID`),
  KEY `FarmerID` (`FarmerID`),
  CONSTRAINT `farmernotes_ibfk_1` FOREIGN KEY (`FarmerID`) REFERENCES `farmers` (`FarmerID`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `farmernotes`
--

LOCK TABLES `farmernotes` WRITE;
/*!40000 ALTER TABLE `farmernotes` DISABLE KEYS */;
INSERT INTO `farmernotes` VALUES (1,1,'qwe','2025-01-07 10:29:31'),(2,1,'DEAD','2025-01-07 10:39:24'),(3,2,'dead','2025-01-07 10:43:05'),(4,107,'dead','2025-01-07 10:48:35'),(5,2,'deed','2025-01-07 10:57:08'),(6,4,'ded','2025-01-07 12:25:22'),(7,3,'almost dead\r\n50-50','2025-01-07 14:29:53'),(8,49,'ALMOST DEAD','2025-01-07 15:59:43'),(9,49,'DEAD','2025-01-07 16:01:08'),(10,49,'2025-01-07 17:54:29 - revive','2025-01-07 17:54:30'),(11,51,'2025-01-07 19:13:06 - Dead','2025-01-07 19:13:07'),(12,51,'2025-01-07 19:13:22 - CPR-Revive','2025-01-07 19:13:22'),(13,51,'2025-01-08 11:39:10 - wwee','2025-01-08 11:39:11'),(14,164,'2025-01-08 13:29:07 - qweweqweq','2025-01-08 13:29:08');
/*!40000 ALTER TABLE `farmernotes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `farmers`
--

DROP TABLE IF EXISTS `farmers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `farmers` (
  `FarmerID` int NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(255) DEFAULT NULL,
  `LastName` varchar(255) DEFAULT NULL,
  `MiddleName` varchar(255) DEFAULT NULL,
  `Address` text,
  `Barangay` varchar(45) DEFAULT NULL,
  `ContactInfo` varchar(255) DEFAULT NULL,
  `FarmSize` decimal(10,2) DEFAULT NULL,
  `RegistrationDate` date DEFAULT NULL,
  `Crop` int DEFAULT NULL,
  PRIMARY KEY (`FarmerID`),
  KEY `harvest_ibfk_2_idx` (`Crop`),
  KEY `farmer_ibfk_1_idx` (`Crop`),
  CONSTRAINT `farmer_ibfk_1` FOREIGN KEY (`Crop`) REFERENCES `commodities` (`CommodityID`)
) ENGINE=InnoDB AUTO_INCREMENT=181 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `farmers`
--

LOCK TABLES `farmers` WRITE;
/*!40000 ALTER TABLE `farmers` DISABLE KEYS */;
INSERT INTO `farmers` VALUES (1,'Juans','Dela Cruz','','Brooke\'s Point, Palawan','Amas','09123456789',2.00,'2023-11-20',100),(2,'Maria','Santos','','Brooke\'s Point, Palawan','Aribungos','09987654321',3.20,'2023-12-15',100),(3,'Pedro','Tan','','Brooke\'s Point, Palawan','Barong-barong','09258147036',8.00,'2024-01-10',100),(4,'Ana','Reyes','','Brooke\'s Point, Palawan','Calasaguen','09196325874',2.70,'2024-02-05',100),(5,'Jose','Garcia','','Brooke\'s Point, Palawan','Imulnod','09062485713',6.30,'2024-03-02',100),(6,'Rosa','Castro','','Brooke\'s Point, Palawan','Ipilan','09214703692',4.10,'2024-03-28',100),(7,'Luis','Gomez','','Brooke\'s Point, Palawan','Maasin','09085214736',9.50,'2024-04-22',100),(8,'Carmen','Torres','','Brooke\'s Point, Palawan','Mainit','09270369145',1.90,'2024-05-18',100),(9,'Ricardo','Diaz','','Brooke\'s Point, Palawan','Malis','09051827364',7.20,'2024-06-13',100),(10,'Elena','Flores','','Brooke\'s Point, Palawan','Oring-oring','09283647510',3.80,'2024-07-09',100),(11,'Fernando','Morales','','Brooke\'s Point, Palawan','Pangobilian','09172538469',5.10,'2024-08-04',100),(12,'Isabel','Ramos','','Brooke\'s Point, Palawan','Poblacion','09098765432',2.30,'2024-08-30',100),(13,'Andres','Sanchez','','Brooke\'s Point, Palawan','Samariñana','09258741369',6.90,'2024-09-26',100),(14,'Sofia','Gonzales','','Brooke\'s Point, Palawan','Saraza','09123698745',4.60,'2024-10-22',100),(15,'Miguel','Lopez','','Brooke\'s Point, Palawan','Talandal','09065478912',8.70,'2024-11-18',100),(16,'Teresa','Hernandez','','Brooke\'s Point, Palawan','Samariñana','09236987412',1.50,'2024-12-14',100),(17,'Francisco','Perez','','Brooke\'s Point, Palawan','Tubtub','09058741236',5.80,'2023-11-10',100),(18,'Dolores','Alvarez','','Brooke\'s Point, Palawan','Salogon','09182736450',3.00,'2023-12-05',100),(19,'Balbastro','Bryan','','Brooke\'s Point, Palawan','Malis','09058741236',9.00,'2023-11-10',100),(20,'Gaspar','Mary Ryan','','Brooke\'s Point, Palawan','Mainit','09058741236',5.80,'2023-11-10',100),(21,'Dajay','Nove','','Brooke\'s Point, Palawan','Amas','09058741236',3.80,'2023-11-10',100),(22,'Bonifacio','Once','','Brooke\'s Point','Pangobilian','9050951396',73.00,'2024-05-27',100),(23,'Elisa','Antenero','','','Pangobilian','',73.00,'2024-05-27',100),(24,'Elisa','Antenero','','Brooke\'s Point','Pangobilian','',73.00,'2024-05-27',100),(25,'Florentino JR.','Once','','Brooke\'s Point','Pangobilian','',73.00,'2024-05-27',100),(26,'Dominador','Once','','Brooke\'s Point','Pangobilian','',73.00,'2024-05-27',100),(27,'Elsa','Once','','Brooke\'s Point','Pangobilian','',73.00,'2024-05-27',100),(28,'Amelita','Sabuya','','Brooke\'s Point','Pangobilian','',1.50,'2024-05-27',100),(29,'Lily','Trinidad','','Brooke\'s Point','Pangobilian','',1.50,'2024-05-27',100),(30,'Rosendo','Ocreto Sr.','','Brooke\'s Point','Pangobilian','',1.50,'2024-05-27',100),(31,'Jose','Pega','','Brooke\'s Point','Pangobilian','',2.00,'2024-05-27',100),(32,'Jose','Joe Francis','','Brooke\'s Point','Pangobilian','',2.00,'2024-05-27',100),(33,'Samson','Domondon','','Brooke\'s Point','Pangobilian','9353674628',10.00,'2024-05-27',100),(34,'Ruben','Laab','','Brooke\'s Point','Pangobilian','9359972325',2.00,'2024-05-27',100),(35,'Edmundo','Raquem','','Brooke\'s Point','Pangobilian','9979874111',1.00,'2024-05-27',100),(36,'Hermina','Acaia','','Brooke\'s Point','Pangobilian','',2.30,'2024-05-27',100),(37,'Rey','Rodriguez','','Brooke\'s Point','Pangobilian','9278676814',2.00,'2024-05-27',100),(38,'Vilma','Opinaldo','','Brooke\'s Point','Pangobilian','9056858413',6.00,'2024-05-27',100),(39,'Romero','Mordido','','Brooke\'s Point','Pangobilian','9752793611',2.70,'2024-05-27',100),(40,'Salvacion','Jordan','','Brooke\'s Point','Pangobilian','9678992552',3.00,'2024-05-27',100),(41,'Romualdez','Belviatora','','Brooke\'s Point','Pangobilian','9972103216',1.00,'2024-05-27',100),(42,'Alda','Belviatora','','Brooke\'s Point','Pangobilian','9532369406',1.00,'2024-05-27',100),(43,'Celso','Soplay','','Brooke\'s Point','Pangobilian','9532369406',0.25,'2024-05-27',100),(44,'Naiwa','Beret','','Brooke\'s Point','Pangobilian','9979217252',2.00,'2024-05-27',100),(45,'Dondong','Beret','','Brooke\'s Point','Pangobilian','9978719125',4.50,'2024-05-27',100),(46,'Dondong','Roselyn','','Brooke\'s Point','Pangobilian','9261386597',3.00,'2024-05-27',100),(47,'Emran','Yap','','Brooke\'s Point','Pangobilian','9559306011',2.60,'2024-05-27',100),(48,'Rosalynda','Talpane','','Brooke\'s Point','Pangobilian','9261386597',1.00,'2024-05-27',100),(49,'Aina May','Kentong','','Brooke\'s Point','Pangobilian','9654712464',1.00,'2024-05-27',100),(50,'Vilma','Batenna','','Brooke\'s Point','Pangobilian','9056419399',1.50,'2024-05-27',100),(51,'Adriano','Socrates','','Brooke\'s Point','Pangobilian','9755566896',2.50,'2024-05-27',100),(52,'Susan','Socrates','','Brooke\'s Point','Pangobilian','',2.00,'2024-05-27',100),(53,'Susan','Gerry','','Brooke\'s Point','Pangobilian','',2.00,'2024-05-27',100),(54,'Emilio','Canut','','Brooke\'s Point','Pangobilian','9659241696',5.00,'2024-05-27',100),(55,'Ronald','Canut','','Brooke\'s Point','Pangobilian','',5.00,'2024-05-27',100),(56,'Analie','Canut','','Brooke\'s Point','Pangobilian','',5.00,'2024-05-27',100),(57,'Emily','Canut','','Brooke\'s Point','Pangobilian','',5.00,'2024-05-27',100),(58,'Ruby','Canut','','Brooke\'s Point','Pangobilian','',1.00,'2024-05-27',100),(59,'Jaime','Socrates','','Brooke\'s Point','Pangobilian','',2.00,'2024-05-27',100),(60,'Delia','Bollon','','Brooke\'s Point','Pangobilian','',2.00,'2024-05-27',100),(107,'qwe','qwe','','qweqwe','qweqweq','eqwqwe',0.00,'2025-01-07',2),(108,'Isagani','Sandacan','','Pansur, Malis, Brooke\'s Point','Malis','',1.00,'2020-01-01',2),(109,'Ardelito','Maling','','Capangian, Aribungos, Brooke\'s Point','Aribungos','',1.50,'2020-01-01',2),(110,'Alejandro','Cinco','','Kapangyan, Aribungos, Brooke\'s Point','Aribungos','9355188910',3.50,'2020-01-01',2),(111,'Jaquiline','Aglima','','Kapangyan, Aribungos, Brooke\'s Point','Aribungos','',1.00,'2020-01-01',2),(112,'Nerlita','Suaring','','Kapangyan, Aribungos, Brooke\'s Point','Aribungos','',16.00,'2020-01-01',2),(113,'Lorenzo','Maling','','Capangian, Aribungos, Brooke\'s Point','Aribungos','',1.50,'2020-01-01',2),(114,'Remedios','Contreras','','Malis, Brooke\'s Point, Palawan','Malis','',1.00,'2020-01-01',2),(115,'Solomon','Tarusan','','Salogon, Brooke\'s Point, Palawan','Salogon','',6.00,'2020-01-01',2),(116,'Tereto','Soda','','Tagpinasao, Salogon, Brooke\'s Point','Salogon','985980402',3.00,'2020-01-01',2),(117,'Danilo','Ceralbo','','Propern1, Salogon, Brooke\'s Point','Salogon','9057313433',0.50,'2020-01-01',2),(120,'Conrada','Castillo','Taneo','','','9558568233',2.00,'2020-01-01',1),(121,'Pedro','Castillo','Nacorol','','','9558568233',1.00,'2020-01-01',1),(122,'Emmanuel','Jaloca','Esartero','','','9479754166',2.00,'2020-01-01',1),(123,'Emelio','Luzara','Cabatingan','','','9675137433',0.50,'2020-01-01',1),(124,'Estaly','Luzara','Cabatingan','','','9355134973',0.50,'2020-01-01',1),(125,'Jessie','Luzara','Cabatingan','','','9460221347',0.50,'2020-01-01',1),(126,'Nathaniel','Luzara','Cabatingan','','','9355134973',0.50,'2020-01-01',1),(127,'Victorio','Luzara','Cabatingan','','','9354571814',0.50,'2020-01-01',1),(128,'Judie','Magbanua','Luzara','','','9558568233',0.50,'2020-01-01',1),(129,'Gevalie','Petuerto','Luzara','','','9355134973',0.50,'2020-01-01',1),(130,'Shiela','Toneives','Rabo','','','9555160997',0.50,'2020-01-01',1),(131,'Julieto','Anoto','Enero','','','9854173902',1.00,'2020-01-01',1),(132,'Nova','Sagala','Villa','','','9539816373',2.25,'2020-01-01',1),(133,'Erlinda','Rojales','Tomeres','','','9811825092',0.50,'2020-01-01',1),(134,'Poindimng','Alimoot','Actsquela','','','9309665257',1.00,'2020-01-01',1),(135,'Liticia','Cañeta','Muntalid','','','9366617068',3.00,'2020-01-01',1),(136,'Saraylyn','Sagala','Osayan','','','9633970932',0.50,'2020-01-01',1),(137,'Angelito','Mejollo','Dela Peña','','','9649515493',1.00,'2020-01-01',1),(138,'Peter','Lagan','Feria','','','9127646725',3.00,'2020-01-01',1),(139,'Jeran','Abdurahman','Ahad','','','9079962498',1.00,'2020-01-01',1),(140,'Ic','Tamayo','Reboya','','','95525544859',0.60,'2020-01-01',1),(141,'Marfe Joy','Tamayo','','','','9484390809',0.60,'2020-01-01',1),(142,'Wezotto','Arevalo','Maricari','','','9973641722',1.00,'2020-01-01',1),(143,'Janette','Sagala','Irando','','','9488002349',1.00,'2020-01-01',1),(144,'Ileonor','Sagala','Gentalian','','','9059546690',1.50,'2020-01-01',1),(145,'Marites','Sagala','Dela Peña','','','9059546690',1.50,'2020-01-01',1),(146,'Liberty','Artequela','Arevalo','','','9103248297',1.00,'2020-01-01',1),(147,'Erlina','Tomeres','Molejon','','','9129414232',2.50,'2020-01-01',1),(148,'Hadjie','Retuerto','Catampanga','','','9993994737',0.70,'2020-01-01',1),(149,'Primo','Alejandrino','Duga','','','9659157616',2.00,'2020-01-01',1),(150,'Lonsilo','Radi','Banda','','Salogon','9361277392',0.50,'2020-01-01',1),(151,'Asna','Sario','Vesvo','','Salogon','9675443712',2.50,'2020-01-01',1),(152,'Jay-ar','Sario','Vesvo','','Salogon','9675443712',2.50,'2020-01-01',1),(153,'Maricel','Sario','Uson','','Salogon','9675443712',2.50,'2020-01-01',1),(154,'Emelio','Marudi','Lucas','','Salogon','9261847667',0.50,'2020-01-01',1),(155,'Sarima','Rimbonan','Uson','','Salogon','9051942016',0.50,'2020-01-01',1),(156,'Tedeño','Etos','Tuna','','Salogon','9758476919',0.75,'2020-01-01',1),(157,'Sindo','Awasan','Salim','','Salogon','9811805949',1.50,'2020-01-01',1),(158,'Criselda','Tambalque','Sua','','Salogon','9812160813',1.00,'2020-01-01',1),(159,'Hermiña','Espinocilla','Pontoc','','Salogon','9066398167',1.00,'2020-01-01',1),(160,'Milagros','Espinocilla','Daprosa','','Salogon','9268557287',0.75,'2020-01-01',1),(161,'Ronalyn','Tomahaw','Awasan','','Salogon','9911805949',0.50,'2020-01-01',1),(162,'Levita','Reola','David','','Salogon','9812172929',1.00,'2020-01-01',1),(163,'Nul','Astala','David','','Salogon','9658417436',1.00,'2020-01-01',1),(164,'Alsaide','Dugasan','Astala','','Salogon','9050472064',1.00,'2020-01-01',1),(165,'Eksan','Dugasan','Darin','','Salogon','9976641184',1.00,'2020-01-01',1),(166,'Lita','Tapiculia','Marcos','','Salogon','',3.00,'2020-01-01',1),(167,'Isniya','Kassim','Astala','','Salogon','',1.00,'2020-01-01',1),(168,'Bahanaria','Astala','Utaalah','','Salogon','',1.00,'2020-01-01',1),(169,'Sanra','Usman','Astala','','Salogon','',1.00,'2020-01-01',1),(170,'Wilson','Belinanio','Masangkay','','Salogon','',0.75,'2020-01-01',1),(171,'Mahacutla','Astala','Bandahalah','','Salogon','',1.00,'2020-01-01',1),(172,'Maricnes','Graspela','Suranol','','Salogon','9369484554',1.00,'2020-01-01',1),(173,'Macmillan','Graspela','Suranol','','Salogon','9758086991',1.00,'2020-01-01',1),(174,'Rosalinda','Mercado','Suranol','','Salogon','9066397656',1.00,'2020-01-01',1),(175,'Hetler','Mercado','Suranol','','Salogon','9050459854',1.00,'2020-01-01',1),(176,'Hetlon','Mercado','Suranol','','Salogon','9351881385',1.00,'2020-01-01',1),(177,'Alli','Mercado','Suranol','','Salogon','9484040372',1.00,'2020-01-01',1),(178,'Alberto','Mercado','Sicaf','','Salogon','9266017824',1.00,'2020-01-01',1),(179,'Alonzo','Radi','Maning','','Salogon','9361277392',0.50,'2020-01-01',1),(180,'qweewq','e','','eqeqe','q','eqqe',3.00,'2025-01-08',4);
/*!40000 ALTER TABLE `farmers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `harvest`
--

DROP TABLE IF EXISTS `harvest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `harvest` (
  `HarvestID` int NOT NULL AUTO_INCREMENT,
  `FarmerID` int DEFAULT NULL,
  `CommodityID` int DEFAULT NULL,
  `HarvestDate` date DEFAULT NULL,
  `Quantity` decimal(10,2) DEFAULT NULL,
  `PricePerUnit` decimal(10,2) DEFAULT NULL,
  `TotalRevenue` decimal(10,2) GENERATED ALWAYS AS ((`Quantity` * `PricePerUnit`)) VIRTUAL,
  `TotalArea` decimal(10,2) DEFAULT NULL,
  `OtherDetails` text,
  PRIMARY KEY (`HarvestID`),
  KEY `FarmerID` (`FarmerID`),
  KEY `CommodityID` (`CommodityID`),
  CONSTRAINT `harvest_ibfk_1` FOREIGN KEY (`FarmerID`) REFERENCES `farmers` (`FarmerID`),
  CONSTRAINT `harvest_ibfk_2` FOREIGN KEY (`CommodityID`) REFERENCES `commodities` (`CommodityID`)
) ENGINE=InnoDB AUTO_INCREMENT=250 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `harvest`
--

LOCK TABLES `harvest` WRITE;
/*!40000 ALTER TABLE `harvest` DISABLE KEYS */;
INSERT INTO `harvest` (`HarvestID`, `FarmerID`, `CommodityID`, `HarvestDate`, `Quantity`, `PricePerUnit`, `TotalArea`, `OtherDetails`) VALUES (1,1,1,'2024-04-20',7500.00,11.50,2.80,NULL),(2,2,1,'2024-02-15',6200.00,12.00,2.20,NULL),(3,3,1,'2023-12-25',5800.00,11.80,2.50,NULL),(4,4,1,'2024-01-10',6500.00,12.50,2.70,NULL),(5,5,1,'2023-11-30',4800.00,12.20,2.00,NULL),(6,6,1,'2024-03-05',8200.00,11.60,3.20,NULL),(7,7,1,'2023-10-18',3900.00,13.00,1.80,NULL),(8,8,1,'2024-02-28',7000.00,12.80,2.60,NULL),(9,9,1,'2023-12-08',5200.00,11.90,2.10,NULL),(10,10,1,'2024-03-20',7800.00,12.30,3.00,NULL),(11,1,1,'2024-05-25',100.00,10.00,50.00,NULL),(12,1,1,'2024-01-25',100.00,10.00,50.00,NULL),(13,1,1,'2023-12-01',2500.00,10.00,50.00,NULL),(14,2,1,'2024-01-10',2100.00,10.00,50.00,NULL),(15,22,1,'2024-05-27',570.00,11.00,73.00,NULL),(16,23,1,'2024-05-27',500.00,11.00,73.00,NULL),(17,25,1,'2024-05-27',1000.00,11.00,73.00,NULL),(18,26,1,'2024-05-27',800.00,11.00,73.00,NULL),(19,27,1,'2024-05-27',1000.00,11.00,73.00,NULL),(20,28,1,'2024-05-27',650.00,11.00,73.00,NULL),(21,28,1,'2024-05-27',1800.00,11.00,1.50,NULL),(22,30,1,'2024-05-27',2500.00,11.00,1.00,NULL),(23,31,1,'2024-05-27',3600.00,11.00,2.00,NULL),(24,32,1,'2024-05-27',1800.00,11.00,2.00,NULL),(38,NULL,3,'2017-12-31',481.97,NULL,143.50,'Amas - 2017'),(39,NULL,3,'2017-12-31',547.74,NULL,159.00,'Aribungos - 2017'),(40,NULL,3,'2017-12-31',1917.01,NULL,529.00,'Barong2x - 2017'),(41,NULL,3,'2017-12-31',1567.78,NULL,454.00,'Calasaguen - 2017'),(42,NULL,3,'2017-12-31',2097.35,NULL,581.50,'Ipilan - 2017'),(43,NULL,3,'2017-12-31',122.18,NULL,41.50,'Imulnod - 2017'),(44,NULL,3,'2017-12-31',3209.36,NULL,907.50,'Maasin - 2017'),(45,NULL,3,'2017-12-31',180.00,NULL,58.50,'Mainit - 2017'),(46,NULL,3,'2017-12-31',1625.00,NULL,438.50,'Malis - 2017'),(47,NULL,3,'2017-12-31',2260.87,NULL,661.50,'Mambalot - 2017'),(48,NULL,3,'2017-12-31',234.49,NULL,71.20,'Oring-Oring - 2017'),(49,NULL,3,'2017-12-31',3164.49,NULL,794.00,'Pangobilian - 2017'),(50,NULL,3,'2017-12-31',2150.25,NULL,571.75,'Salogon - 2017'),(51,NULL,3,'2017-12-31',1141.82,NULL,312.00,'Saraza - 2017'),(52,NULL,3,'2017-12-31',2509.93,NULL,698.00,'Samariñana - 2017'),(53,NULL,3,'2017-12-31',1568.76,NULL,437.75,'Tubtub - 2017'),(54,NULL,3,'2018-12-31',496.18,NULL,157.50,'Amas - 2018'),(55,NULL,3,'2018-12-31',481.55,NULL,148.50,'Aribungos - 2018'),(56,NULL,3,'2018-12-31',1370.48,NULL,471.00,'Barong2x - 2018'),(57,NULL,3,'2018-12-31',1313.70,NULL,449.00,'Calasaguen - 2018'),(58,NULL,3,'2018-12-31',1596.85,NULL,494.20,'Ipilan - 2018'),(59,NULL,3,'2018-12-31',84.70,NULL,33.00,'Imulnod - 2018'),(60,NULL,3,'2018-12-31',2675.62,NULL,904.20,'Maasin - 2018'),(61,NULL,3,'2018-12-31',131.35,NULL,52.50,'Mainit - 2018'),(62,NULL,3,'2018-12-31',1209.68,NULL,436.95,'Malis - 2018'),(63,NULL,3,'2018-12-31',1906.98,NULL,656.50,'Mambalot - 2018'),(64,NULL,3,'2018-12-31',201.42,NULL,66.00,'Oring-Oring - 2018'),(65,NULL,3,'2018-12-31',2985.04,NULL,864.50,'Pangobilian - 2018'),(66,NULL,3,'2018-12-31',2286.16,NULL,694.00,'Salogon - 2018'),(67,NULL,3,'2018-12-31',1142.66,NULL,332.50,'Saraza - 2018'),(68,NULL,3,'2018-12-31',2233.65,NULL,690.75,'Samariñana - 2018'),(69,NULL,3,'2018-12-31',1395.93,NULL,422.50,'Tubtub - 2018'),(70,NULL,3,'2019-12-31',502.18,NULL,163.50,'Amas - 2019'),(71,NULL,3,'2019-12-31',486.55,NULL,152.50,'Aribungos - 2019'),(72,NULL,3,'2019-12-31',1400.48,NULL,481.00,'Barong2x - 2019'),(73,NULL,3,'2019-12-31',1330.70,NULL,459.00,'Calasaguen - 2019'),(74,NULL,3,'2019-12-31',1626.85,NULL,504.20,'Ipilan - 2019'),(75,NULL,3,'2019-12-31',88.70,NULL,35.00,'Imulnod - 2019'),(76,NULL,3,'2019-12-31',2700.62,NULL,914.20,'Maasin - 2019'),(77,NULL,3,'2019-12-31',135.35,NULL,54.50,'Mainit - 2019'),(78,NULL,3,'2019-12-31',1250.68,NULL,446.95,'Malis - 2019'),(79,NULL,3,'2019-12-31',1950.98,NULL,666.50,'Mambalot - 2019'),(80,NULL,3,'2019-12-31',205.42,NULL,68.00,'Oring-Oring - 2019'),(81,NULL,3,'2019-12-31',3000.04,NULL,874.50,'Pangobilian - 2019'),(82,NULL,3,'2019-12-31',2300.16,NULL,704.00,'Salogon - 2019'),(83,NULL,3,'2019-12-31',1150.66,NULL,342.50,'Saraza - 2019'),(84,NULL,3,'2019-12-31',2250.65,NULL,700.75,'Samariñana - 2019'),(85,NULL,3,'2019-12-31',1400.93,NULL,432.50,'Tubtub - 2019'),(86,NULL,3,'2020-12-31',510.00,NULL,168.00,'Amas - 2020'),(87,NULL,3,'2020-12-31',490.00,NULL,155.00,'Aribungos - 2020'),(88,NULL,3,'2020-12-31',1425.00,NULL,485.00,'Barong2x - 2020'),(89,NULL,3,'2020-12-31',1345.00,NULL,466.00,'Calasaguen - 2020'),(90,NULL,3,'2020-12-31',1640.00,NULL,510.00,'Ipilan - 2020'),(91,NULL,3,'2020-12-31',90.00,NULL,37.00,'Imulnod - 2020'),(92,NULL,3,'2020-12-31',2750.00,NULL,925.00,'Maasin - 2020'),(93,NULL,3,'2020-12-31',140.00,NULL,55.00,'Mainit - 2020'),(94,NULL,3,'2020-12-31',1280.00,NULL,454.00,'Malis - 2020'),(95,NULL,3,'2020-12-31',2000.00,NULL,680.00,'Mambalot - 2020'),(96,NULL,3,'2020-12-31',210.00,NULL,70.00,'Oring-Oring - 2020'),(97,NULL,3,'2020-12-31',3050.00,NULL,890.00,'Pangobilian - 2020'),(98,NULL,3,'2020-12-31',2350.00,NULL,715.00,'Salogon - 2020'),(99,NULL,3,'2020-12-31',1200.00,NULL,350.00,'Saraza - 2020'),(100,NULL,3,'2020-12-31',2400.00,NULL,730.00,'Samariñana - 2020'),(101,NULL,3,'2020-12-31',1450.00,NULL,440.00,'Tubtub - 2020'),(102,NULL,3,'2021-12-31',520.00,NULL,170.00,'Amas - 2021'),(103,NULL,3,'2021-12-31',495.00,NULL,158.00,'Aribungos - 2021'),(104,NULL,3,'2021-12-31',1440.00,NULL,490.00,'Barong2x - 2021'),(105,NULL,3,'2021-12-31',1360.00,NULL,470.00,'Calasaguen - 2021'),(106,NULL,3,'2021-12-31',1660.00,NULL,520.00,'Ipilan - 2021'),(107,NULL,3,'2021-12-31',92.00,NULL,38.00,'Imulnod - 2021'),(108,NULL,3,'2021-12-31',2800.00,NULL,950.00,'Maasin - 2021'),(109,NULL,3,'2021-12-31',145.00,NULL,57.00,'Mainit - 2021'),(110,NULL,3,'2021-12-31',1300.00,NULL,460.00,'Malis - 2021'),(111,NULL,3,'2021-12-31',2050.00,NULL,690.00,'Mambalot - 2021'),(112,NULL,3,'2021-12-31',215.00,NULL,72.00,'Oring-Oring - 2021'),(113,NULL,3,'2021-12-31',3100.00,NULL,900.00,'Pangobilian - 2021'),(114,NULL,3,'2021-12-31',2400.00,NULL,730.00,'Salogon - 2021'),(115,NULL,3,'2021-12-31',1250.00,NULL,360.00,'Saraza - 2021'),(116,NULL,3,'2021-12-31',2450.00,NULL,740.00,'Samariñana - 2021'),(117,NULL,3,'2021-12-31',1500.00,NULL,450.00,'Tubtub - 2021'),(118,NULL,3,'2022-12-31',530.00,NULL,172.00,'Amas - 2022'),(119,NULL,3,'2022-12-31',500.00,NULL,160.00,'Aribungos - 2022'),(120,NULL,3,'2022-12-31',1460.00,NULL,495.00,'Barong2x - 2022'),(121,NULL,3,'2022-12-31',1375.00,NULL,475.00,'Calasaguen - 2022'),(122,NULL,3,'2022-12-31',1680.00,NULL,525.00,'Ipilan - 2022'),(123,NULL,3,'2022-12-31',95.00,NULL,39.00,'Imulnod - 2022'),(124,NULL,3,'2022-12-31',2850.00,NULL,975.00,'Maasin - 2022'),(125,NULL,3,'2022-12-31',150.00,NULL,60.00,'Mainit - 2022'),(126,NULL,3,'2022-12-31',1320.00,NULL,465.00,'Malis - 2022'),(127,NULL,3,'2022-12-31',2100.00,NULL,700.00,'Mambalot - 2022'),(128,NULL,3,'2022-12-31',220.00,NULL,74.00,'Oring-Oring - 2022'),(129,NULL,3,'2022-12-31',3150.00,NULL,910.00,'Pangobilian - 2022'),(130,NULL,3,'2022-12-31',2500.00,NULL,740.00,'Salogon - 2022'),(131,NULL,3,'2022-12-31',1300.00,NULL,365.00,'Saraza - 2022'),(132,NULL,3,'2022-12-31',2550.00,NULL,750.00,'Samariñana - 2022'),(133,NULL,3,'2022-12-31',1550.00,NULL,460.00,'Tubtub - 2022'),(134,NULL,3,'2023-12-31',540.00,NULL,175.00,'Amas - 2023'),(135,NULL,3,'2023-12-31',510.00,NULL,163.00,'Aribungos - 2023'),(136,NULL,3,'2023-12-31',1480.00,NULL,500.00,'Barong2x - 2023'),(137,NULL,3,'2023-12-31',1390.00,NULL,480.00,'Calasaguen - 2023'),(138,NULL,3,'2023-12-31',1700.00,NULL,530.00,'Ipilan - 2023'),(139,NULL,3,'2023-12-31',98.00,NULL,40.00,'Imulnod - 2023'),(140,NULL,3,'2023-12-31',2900.00,NULL,1000.00,'Maasin - 2023'),(141,NULL,3,'2023-12-31',155.00,NULL,59.00,'Mainit - 2023'),(142,NULL,3,'2023-12-31',1350.00,NULL,475.00,'Malis - 2023'),(143,NULL,3,'2023-12-31',2150.00,NULL,710.00,'Mambalot - 2023'),(144,NULL,3,'2023-12-31',230.00,NULL,77.00,'Oring-Oring - 2023'),(145,NULL,3,'2023-12-31',3200.00,NULL,920.00,'Pangobilian - 2023'),(146,NULL,3,'2023-12-31',2600.00,NULL,760.00,'Salogon - 2023'),(147,NULL,3,'2023-12-31',1350.00,NULL,375.00,'Saraza - 2023'),(148,NULL,3,'2023-12-31',2650.00,NULL,765.00,'Samariñana - 2023'),(149,NULL,3,'2023-12-31',1600.00,NULL,475.00,'Tubtub - 2023'),(151,NULL,7,'2023-01-31',3510.00,NULL,NULL,NULL),(152,NULL,7,'2023-02-28',15045.00,NULL,NULL,NULL),(153,NULL,7,'2023-03-31',15635.00,NULL,NULL,NULL),(154,NULL,7,'2023-04-30',46713.20,NULL,NULL,NULL),(155,NULL,7,'2023-05-31',40090.60,NULL,NULL,NULL),(156,NULL,7,'2023-06-30',49526.65,NULL,NULL,NULL),(157,NULL,7,'2023-07-31',39985.80,NULL,NULL,NULL),(158,NULL,7,'2023-08-31',61687.90,NULL,NULL,NULL),(159,NULL,7,'2023-09-30',36997.50,NULL,NULL,NULL),(160,NULL,7,'2023-10-31',20013.88,NULL,NULL,NULL),(161,NULL,7,'2023-11-30',68362.30,NULL,NULL,NULL),(162,NULL,7,'2023-12-31',71198.09,NULL,NULL,NULL),(171,NULL,7,'2021-01-31',0.00,NULL,NULL,NULL),(172,NULL,7,'2021-02-28',0.00,NULL,NULL,NULL),(173,NULL,7,'2021-03-31',0.00,NULL,NULL,NULL),(174,NULL,7,'2021-04-30',0.00,NULL,NULL,NULL),(175,NULL,7,'2021-05-31',0.00,NULL,NULL,NULL),(176,NULL,7,'2021-06-30',0.00,NULL,NULL,NULL),(177,NULL,7,'2021-07-31',21470.00,NULL,NULL,NULL),(178,NULL,7,'2021-08-31',30750.00,NULL,NULL,NULL),(179,NULL,7,'2021-09-30',29470.00,NULL,NULL,NULL),(180,NULL,7,'2021-10-31',20840.00,NULL,NULL,NULL),(181,NULL,7,'2021-11-30',28700.00,NULL,NULL,NULL),(182,NULL,7,'2021-12-31',0.00,NULL,NULL,NULL),(183,NULL,4,'2021-12-31',919.94,NULL,199.88,NULL),(184,NULL,4,'2022-12-31',652.15,NULL,157.75,NULL),(185,NULL,4,'2023-12-31',1507.15,NULL,358.92,NULL),(186,NULL,4,'2019-12-31',602.84,NULL,149.45,NULL),(187,NULL,4,'2020-12-31',713.37,NULL,177.30,NULL),(190,120,1,'2023-11-20',800.00,NULL,2.00,NULL),(191,121,1,'2023-10-12',1000.00,NULL,1.00,NULL),(192,122,1,'2024-06-01',1512.00,NULL,2.00,NULL),(193,123,1,'2024-01-14',620.00,NULL,0.50,NULL),(194,124,1,'2024-01-14',600.00,NULL,0.50,NULL),(195,125,1,'2024-01-14',610.00,NULL,0.50,NULL),(196,126,1,'2024-01-14',699.00,NULL,0.50,NULL),(197,127,1,'2024-01-14',633.00,NULL,0.50,NULL),(198,128,1,'2024-01-14',638.00,NULL,0.50,NULL),(199,129,1,'2024-01-14',575.00,NULL,0.50,NULL),(200,130,1,'2024-02-15',10.00,NULL,0.50,NULL),(201,131,1,'2024-02-15',100.00,NULL,1.00,NULL),(202,132,1,'2024-02-15',250.00,NULL,2.25,NULL),(203,133,1,'2024-02-15',35.00,NULL,0.50,NULL),(204,134,1,'2024-02-13',70.00,NULL,1.00,NULL),(205,135,1,'2024-02-15',100.00,NULL,3.00,NULL),(206,136,1,'2024-02-20',25.00,NULL,0.50,NULL),(207,137,1,'2024-02-25',100.00,NULL,1.00,NULL),(208,138,1,'2024-02-28',150.00,NULL,3.00,NULL),(209,139,1,'2024-02-28',100.00,NULL,1.00,NULL),(210,140,1,'2024-03-08',80.00,NULL,0.60,NULL),(211,141,1,'2024-03-08',80.00,NULL,0.60,NULL),(212,142,1,'2024-03-10',100.00,NULL,1.00,NULL),(213,143,1,'2024-03-13',30.00,NULL,1.00,NULL),(214,144,1,'2024-03-16',150.00,NULL,1.50,NULL),(215,145,1,'2024-03-16',150.00,NULL,1.50,NULL),(216,146,1,'2024-03-23',120.00,NULL,1.00,NULL),(217,147,1,'2024-03-23',200.00,NULL,2.50,NULL),(218,148,1,'2024-03-23',70.00,NULL,0.70,NULL),(219,149,1,'2024-03-23',120.00,NULL,2.00,NULL),(220,150,1,'2024-01-15',100.00,NULL,0.50,NULL),(221,151,1,'2024-01-15',100.00,NULL,2.50,NULL),(222,152,1,'2024-01-15',100.00,NULL,2.50,NULL),(223,153,1,'2024-01-15',100.00,NULL,2.50,NULL),(224,154,1,'2024-01-13',500.00,NULL,0.50,NULL),(225,155,1,'2024-01-20',300.00,NULL,0.50,NULL),(226,156,1,'2024-01-20',NULL,NULL,0.75,NULL),(227,157,1,'2024-01-25',350.00,NULL,1.50,NULL),(228,158,1,'2024-01-30',500.00,NULL,1.00,NULL),(229,159,1,'2024-01-30',500.00,NULL,1.00,NULL),(230,160,1,'2024-01-30',400.00,NULL,0.75,NULL),(231,161,1,'2024-01-30',NULL,NULL,0.50,NULL),(232,162,1,'2024-02-15',210.00,NULL,1.00,NULL),(233,163,1,'2024-02-15',NULL,NULL,1.00,NULL),(234,164,1,'2024-02-18',1200.00,NULL,1.00,NULL),(235,165,1,'2024-02-20',350.00,NULL,1.00,NULL),(236,166,1,'2024-02-22',1500.00,NULL,3.00,NULL),(237,167,1,'2024-02-22',11200.00,NULL,1.00,NULL),(238,168,1,'2024-02-25',300.00,NULL,1.00,NULL),(239,169,1,'2024-02-25',400.00,NULL,1.00,NULL),(240,170,1,'2024-02-28',600.00,NULL,0.75,NULL),(241,171,1,'2024-03-05',240.00,NULL,1.00,NULL),(242,172,1,'2024-03-10',1000.00,NULL,1.00,NULL),(243,173,1,'2024-03-10',1000.00,NULL,1.00,NULL),(244,174,1,'2024-03-12',700.00,NULL,1.00,NULL),(245,175,1,'2024-03-12',NULL,NULL,1.00,NULL),(246,176,1,'2024-03-12',NULL,NULL,1.00,NULL),(247,177,1,'2024-03-12',NULL,NULL,1.00,NULL),(248,178,1,'2024-03-15',500.00,NULL,1.00,NULL),(249,179,1,'2024-03-15',500.00,NULL,0.50,NULL);
/*!40000 ALTER TABLE `harvest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `importerrors`
--

DROP TABLE IF EXISTS `importerrors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `importerrors` (
  `ErrorID` int NOT NULL AUTO_INCREMENT,
  `ImportID` int NOT NULL,
  `RowNumber` int NOT NULL,
  `ErrorMessage` text NOT NULL,
  `CreatedDate` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ErrorID`),
  KEY `ImportID` (`ImportID`),
  CONSTRAINT `importerrors_ibfk_1` FOREIGN KEY (`ImportID`) REFERENCES `importhistory` (`ImportID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `importerrors`
--

LOCK TABLES `importerrors` WRITE;
/*!40000 ALTER TABLE `importerrors` DISABLE KEYS */;
/*!40000 ALTER TABLE `importerrors` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `importhistory`
--

DROP TABLE IF EXISTS `importhistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `importhistory` (
  `ImportID` int NOT NULL AUTO_INCREMENT,
  `ImportDate` datetime NOT NULL,
  `ImportType` varchar(50) NOT NULL,
  `FileName` varchar(255) NOT NULL,
  `RowsProcessed` int DEFAULT '0',
  `RowsSuccessful` int DEFAULT '0',
  `RowsFailed` int DEFAULT '0',
  `CompletedDate` datetime DEFAULT NULL,
  `ErrorLog` text,
  PRIMARY KEY (`ImportID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `importhistory`
--

LOCK TABLES `importhistory` WRITE;
/*!40000 ALTER TABLE `importhistory` DISABLE KEYS */;
/*!40000 ALTER TABLE `importhistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `importstaging`
--

DROP TABLE IF EXISTS `importstaging`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `importstaging` (
  `StagingID` int NOT NULL AUTO_INCREMENT,
  `ImportID` int NOT NULL,
  `RowNumber` int NOT NULL,
  `RawData` json NOT NULL,
  `ValidationStatus` varchar(20) DEFAULT 'Pending',
  `ErrorMessage` text,
  `CreatedDate` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`StagingID`),
  KEY `ImportID` (`ImportID`),
  CONSTRAINT `importstaging_ibfk_1` FOREIGN KEY (`ImportID`) REFERENCES `importhistory` (`ImportID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `importstaging`
--

LOCK TABLES `importstaging` WRITE;
/*!40000 ALTER TABLE `importstaging` DISABLE KEYS */;
/*!40000 ALTER TABLE `importstaging` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `UserID` int NOT NULL AUTO_INCREMENT,
  `Username` varchar(255) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `FirstName` varchar(255) DEFAULT NULL,
  `LastName` varchar(255) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `Role` varchar(50) NOT NULL,
  `IsActive` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `Username` (`Username`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=112 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'admin','admin','Jan Seth','Daganta','janseth@example.com','Admin',1),(110,'user','123','user','user','user@email.com','User',1);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-01-08 15:06:33
