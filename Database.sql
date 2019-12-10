-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               10.1.13-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             9.5.0.5196
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for servermanager
CREATE DATABASE IF NOT EXISTS `servermanager` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `servermanager`;

-- Dumping structure for table servermanager.authentication
CREATE TABLE IF NOT EXISTS `authentication` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `serialKey` varchar(50) NOT NULL,
  `activated` tinyint(4) NOT NULL DEFAULT '0',
  `hwid` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

-- Dumping data for table servermanager.authentication: ~1 rows (approximately)
/*!40000 ALTER TABLE `authentication` DISABLE KEYS */;
INSERT INTO `authentication` (`id`, `serialKey`, `activated`, `hwid`) VALUES
	(1, 'abcde', 1, 'AJDY/B2jS9oxqpjLC9HRyuh/HoZy88V/0bkjBIADOlo=');
/*!40000 ALTER TABLE `authentication` ENABLE KEYS */;

-- Dumping structure for table servermanager.servers
CREATE TABLE IF NOT EXISTS `servers` (
  `serverId` int(128) NOT NULL AUTO_INCREMENT,
  `serverToken` varchar(250) DEFAULT NULL,
  `ownerToken` varchar(250) DEFAULT NULL,
  `name` varchar(100) DEFAULT NULL,
  `created` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`serverId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table servermanager.servers: ~0 rows (approximately)
/*!40000 ALTER TABLE `servers` DISABLE KEYS */;
/*!40000 ALTER TABLE `servers` ENABLE KEYS */;

-- Dumping structure for table servermanager.users
CREATE TABLE IF NOT EXISTS `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `username` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `phone_number` varchar(255) NOT NULL,
  `adminlevel` tinyint(4) NOT NULL,
  `banned` tinyint(4) NOT NULL DEFAULT '0',
  `ownerToken` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

-- Dumping data for table servermanager.users: ~1 rows (approximately)
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`id`, `name`, `email`, `username`, `password`, `phone_number`, `adminlevel`, `banned`, `ownerToken`) VALUES
	(2, 'Olle Strand', 'ollestrand02@gmail.com', 'OlleStr', 'FdEXt82Y7AXlCieR0iSPOSCXRPIdYTw3R3T5YsGKEXGOPSX+', '0708572507', 3, 0, 'BNmVQhaJMipVgbB6HdtA26vS1PXXKBte1gTGLjSVJPptKkRY'),
	(3, 'TestUser', 'testmail@gmail.com', 'TestRole', 'ppPx0wJUHLGKWYpN0xlVCjukQeHL3oPj503UG/xnSzNLeA4J', '112', 0, 0, 'qpennxz6GpQXO78=1kbYqciVZMLF5U?bmr06oEgqPSZ?xa=o');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
