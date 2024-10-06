# MYG_Project_6

# PHP :
 [Uploadi
<?php

if (!empty($_GET)) {
    try {
        $dbh = new PDO('mysql:host=localhost;dbname=myg_project_6', "root", "");
    } catch (PDOException $e) {
        echo 'ERREUR DE CONNEXION';
    }

    switch ($_GET) {

        case !empty($_GET['fullproductinfo']):
            $productname = $_GET['fullproductinfo'];

            $sql = "SELECT *
            FROM `product`;";

            $value = $dbh->prepare($sql);
            $value->execute();
            $_result = $value->fetchAll(PDO::FETCH_ASSOC);

            print ((json_encode($_result, JSON_UNESCAPED_UNICODE)));
            break;

        case !empty($_GET['cart_content']):
            $email = $_POST['email'];

            $sql = "SELECT 'items_id'
            FROM `cart_content` 
            INNER JOIN 'client_data' ON 'client_data.Email' = '$email'
            WHERE 'client_data.Client_ID' = 'cart_content.Client_ID';";

            $value = $dbh->prepare($sql);
            $value->execute();
            $_result = $value->fetchAll(PDO::FETCH_ASSOC);

            print ((json_encode($_result, JSON_UNESCAPED_UNICODE)));
            break;
    }
}

?>ng index.php…]()

[Uploa
<?php
if (!empty($_POST)) {
    try {
        $dbh = new PDO('mysql:host=localhost;dbname=myg_project_6', "root", "");
    } catch (PDOException $e) {
        echo 'ERREUR DE CONNEXION';
    }
    if (isset($_POST['action'])) {

        $action = $_POST['action'];

        if ($action == 'register') {
            $first_name = $_POST['first_name'];
            $last_name = $_POST['last_name'];
            $email = $_POST['email'];
            $password = password_hash($_POST['password'], PASSWORD_BCRYPT);
            $address = $_POST['address'];
            $zip_code = $_POST['zip_code'];
            $city = $_POST['city'];
            $birthdate = $_POST['birthdate'];
            $phone_number = $_POST['phone_number'];


            $sql = "SELECT `Email`
            FROM `client_data`
            WHERE `Email` = :email;";

            $sth = $dbh->prepare($sql);
            $sth->bindParam(':email', $email);
            $sth->execute();

            $result = $sth->fetch();
            $json = array();

            if (empty($result)) {
                $sql = "INSERT INTO `client_data` (`Client_ID`, `Email`, `Password`, `FirstName`, `LastName`, `Birthdate`, `Address`, `ZIP_Code`, `City`, `Phone_Number`) 
                VALUES (NULL, '$email', '$password', '$first_name', '$last_name', '$birthdate', '$address', '$zip_code', '$city', '$phone_number');
                INSERT INTO `cart_content`
                VALUES (NULL, NULL)";


                $sth = $dbh->prepare($sql);
                $sth->execute();
                $json["Success"] = true;
            } else {
                $json["Success"] = false;
                $json["Error Message"] = "User found";
            }
            print ((json_encode($json, JSON_UNESCAPED_UNICODE)));

        } else if ($action == 'login') {
            $email = $_POST['email'];
            $password = $_POST['password'];

            $sql = "SELECT `Email`, `Password` 
            FROM `client_data`
            WHERE `Email` = :email;";

            $sth = $dbh->prepare($sql);
            $sth->bindParam(':email', $email);
            $sth->execute();

            $result = $sth->fetch();
            $json = array();

            if (!empty($result)) {

                if (password_verify($password, $result['Password'])) {
                    $json['Success'] = true;
                    $json['Message'] = "Logged In";

                    $sql = "SELECT `role` FROM `roles`
                    INNER JOIN `client_data` ON roles.ID = client_data.Client_ID
                    WHERE client_data.Email = '$email';";

                    $sth = $dbh->prepare($sql);
                    $sth->execute();
                    $result = $sth->fetch();

                    if (!empty($result['role'])) {
                        if ($result['role'] == "1") {
                            $json['Role'] = "admin";
                        } else {
                            $json['Role'] = "nonadmin";
                        }
                    }
                } else {
                    $json['Success'] = false;
                    $json["Error Message"] = "Password incorect";
                }
            } else {
                $json["Success"] = false;
                $json["Error Message"] = "User not found";
            }
            print ((json_encode($json, JSON_UNESCAPED_UNICODE)));

        } else if ($action == 'editproduct') {
            $product_ID = $_POST['productID'];
            $product_Name = $_POST['productName'];
            $product_Description = $_POST['productDescription'];
            $product_Price = $_POST['productPrice'];

            $sql = "UPDATE `product`
                    SET `Product_Name` = '$product_Name', `Price` = '$product_Price', `Full_Descritption` = '$product_Description'
                    WHERE `Product_ID` = '$product_ID';";

            $sth = $dbh->prepare($sql);
            $sth->execute();

            $result = $sth->fetch();
            $json = array();

            if (!empty($result)) {
                $json['Success'] = true;
                $json["Result"] = $result;
            }
        } else if ($action == 'updateCart') {
            $cart_content = $_POST['cart_content'];
            $email = $_POST['email'];

            $sql = "UPDATE `cart_content`
            INNER JOIN `client_data` ON `client_data`.`Email` = :email
            SET `items_id` = :cart_content
            WHERE `client_data`.`Client_ID` = `cart_content`.`Client_ID`;";

            $value = $dbh->prepare($sql);
            $value->bindParam(':email', $email);
            $value->bindParam(':cart_content', $cart_content);
            $value->execute();

            $result = $value->fetch();
            $json = array();

            if (!empty($result)) {
                $json['Success'] = true;
                $json["Result"] = $result;
            }

            print ((json_encode($json, JSON_UNESCAPED_UNICODE)));
        }
        else if ($action == "getCartContent") {
            $email = $_POST['email'];

            $sql = "SELECT `items_id`
            FROM `cart_content` 
            INNER JOIN `client_data` ON `client_data`.`Email` = :email
            WHERE `client_data`.`Client_ID` = `cart_content`.`Client_ID`;";

            $value = $dbh->prepare($sql);
            $value->bindParam(':email', $email);
            $value->execute();

            $result = $value->fetch();
            $json = array();

            $json["result"] = true;
            $json["CartContent"] = $result["items_id"];
            
            print ((json_encode($json, JSON_UNESCAPED_UNICODE)));
        }
    }
}
?>ding insert.php…]()


# DATABSE :
[Uploading myg_project_6.s-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:3306
-- Generation Time: Oct 06, 2024 at 12:05 PM
-- Server version: 8.3.0
-- PHP Version: 8.2.18

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `myg_project_6`
--

-- --------------------------------------------------------

--
-- Table structure for table `cart_content`
--

DROP TABLE IF EXISTS `cart_content`;
CREATE TABLE IF NOT EXISTS `cart_content` (
  `items_id` varchar(100) NOT NULL,
  `Client_ID` int NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Client_ID`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `cart_content`
--

INSERT INTO `cart_content` (`items_id`, `Client_ID`) VALUES
('3,1,4', 0);

-- --------------------------------------------------------

--
-- Table structure for table `client_data`
--

DROP TABLE IF EXISTS `client_data`;
CREATE TABLE IF NOT EXISTS `client_data` (
  `Client_ID` int NOT NULL AUTO_INCREMENT,
  `Email` varchar(30) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `Password` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `FirstName` varchar(30) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `LastName` varchar(30) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `Birthdate` date NOT NULL,
  `Address` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `ZIP_Code` int NOT NULL,
  `City` varchar(10) NOT NULL,
  `Phone_Number` int NOT NULL,
  PRIMARY KEY (`Client_ID`)
) ENGINE=MyISAM AUTO_INCREMENT=17 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `client_data`
--

INSERT INTO `client_data` (`Client_ID`, `Email`, `Password`, `FirstName`, `LastName`, `Birthdate`, `Address`, `ZIP_Code`, `City`, `Phone_Number`) VALUES
(0, 'admin@admin.com', '$2y$10$Xsmk/QoQfIhJ8JrKU1UeKuuZxW3NLd0zlf2xAZ5teJKgTq7NZq4Wi', 'Thomas', 'RENAUX', '1998-01-25', '32 rue du château', 31850, 'Beaupuy', 658018920),
(14, 'aze', '$2y$10$Xsmk/QoQfIhJ8JrKU1UeKuuZxW3NLd0zlf2xAZ5teJKgTq7NZq4Wi', 'aze', 'aze', '1010-10-10', 'aze', 10, 'aze', 10),
(13, 'wxc', '$2y$10$ju8bolTJa7lwvNhg/LC/g.dhTWXyflwb152JQP8WepxCHwTkx91zi', 'wxc', 'wxc', '1010-10-10', 'WXC', 10, 'wxc', 10),
(15, 'qsd', '$2y$10$3l6cd0MsEPgC5zDmmKAo1OjW5Z7eCuCS2BZ4Qs8aobWAre7M9/fCO', 'qsd', 'qsd', '1010-10-10', 'qsd', 10, 'qsd', 10),
(16, 'wxcs', '$2y$10$uj.dz.yBPHfU3Affn1xbrO4VvfLM7Ob2./LOGJjPi5KbufqolSUmq', 'wxc', 'wxc', '1010-10-10', 'wxc', 10, 'wxc', 10);

-- --------------------------------------------------------

--
-- Table structure for table `product`
--

DROP TABLE IF EXISTS `product`;
CREATE TABLE IF NOT EXISTS `product` (
  `Product_ID` int NOT NULL AUTO_INCREMENT,
  `Product_Name` varchar(20) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `Price` float NOT NULL,
  `Short_Descritption` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `Full_Descritption` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `Tags` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `Prefab` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  PRIMARY KEY (`Product_ID`)
) ENGINE=MyISAM AUTO_INCREMENT=21 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `product`
--

INSERT INTO `product` (`Product_ID`, `Product_Name`, `Price`, `Short_Descritption`, `Full_Descritption`, `Tags`, `Prefab`) VALUES
(0, 'Armchair', 199, 'Armchair', 'You can really loosen up and relax in comfort because the high back on this chair provides extra support for your neck.', 'Chair chair', 'Assets/Addressables/Furnitures/Armchair.prefab'),
(1, 'Blue Flower', 4.99, 'Artificial flower, 52 cm', 'Lifelike, artificial plants that stay fresh year after year. Perfect if you can\'t have a live plant, but still want to enjoy the beauty of nature.', 'flower Plant', 'Assets/Addressables/Furnitures/Blue Flower.prefab'),
(2, 'Couch', 449, '3-seat sofa-bed', 'Seat cushions with polyurethane foam and polyester fibre wadding give firm and supportive seating comfort.  The storage space under the seat has room for bedlinen or other things. You can make the sofa more comfortable and personal by completing with pillows in different colours and patterns.', 'Couch couch', 'Assets/Addressables/Furnitures/Couch.prefab'),
(3, 'Cylinder Lamp', 62.99, 'Floor lamp', 'You can create a soft, cosy atmosphere in your home with a textile shade that spreads a diffused and decorative light.  Easy to take home since the lampshade comes in a flat-pack.', 'Lamp lamp', 'Assets/Addressables/Furnitures/Cylinder Lamp.prefab'),
(4, 'Desk', 209.97, 'Desk, 140x60 cm', 'The table top is made of board-on-frame, a strong and lightweight material.  The cut-out handles not only add character to the drawer units, they also make it easy for you to grip and open the drawers.', 'desk Desk ', 'Assets/Addressables/Furnitures/Desk.prefab'),
(5, 'Double Bed', 429, 'Bed frame, 180x200 cm', 'The sturdy solid pine frame has natural variations in grain, colour and texture, giving every piece a unique look. And it has been stained and lacquered making it more durable and easy to care for.  You can sit up comfortably in bed thanks to the high headboard – just prop some pillows behind your back and you will have a comfortable place to read or watch TV.', 'bed Bed ', 'Assets/Addressables/Furnitures/Double Bed.prefab'),
(6, 'L Shaped Couch', 999, 'Corner sofa, 5-s w chaise longue.', 'This comfortable sofa has seat cushions with high resilience foam and a top layer of wadding. It provides a nice comfort and support for your body while maintaining the shape of the seat cushions.  Both the headrests and armrests are adjustable, so you can easily find the perfect sitting position.', 'couch Couch', 'Assets/Addressables/Furnitures/L Shaped Couch.prefab'),
(7, 'L Shaped Desk', 599, 'Extendable table, 150/200x80 cm', 'Extendable dining table with 1 extra leaf seats 4-6; makes it possible to adjust the table size according to need.  1 extension leaf included.  1 person can quickly and smoothly extend the table before the guests arrive.', 'table Table Table & desks', 'Assets/Addressables/Furnitures/L Shaped Desk.prefab'),
(8, 'Lamp', 54.99, 'Floor lamp, 150 cm', 'The lamp gives a soft light and creates a warm, cosy atmosphere in your room.', 'lamp Lamp', 'Assets/Addressables/Furnitures/Lamp.prefab'),
(9, 'Nightstand', 39.99, 'Bedside table, 40x39 cm', 'A clean expression that fits right in, in the bedroom or wherever you place it. Smooth-running drawers and in a choice of finishes – pick your favourite.', 'bed Bed nightstand Nightstand Bedside tables', 'Assets/Addressables/Furnitures/Nightstand.prefab'),
(10, 'Pink Vase', 9.99, 'Vase, 21 cm', 'Subdued colours, round shapes and beautiful grooves – GRADVIS vase gives your home a slightly softer and warmer feel. Enhance your favourite flowers or just let it stand beautifully on its own.', 'vase Vase Vases & bowls', 'Assets/Addressables/Furnitures/Pink Vase.prefab'),
(11, 'Rectangle Table', 99, 'Extendable table, 120/180x80 cm', 'Pine has a straight grain and the knots give the pine its signature rustic look. In its natural form it is heavy and gives a very solid feel.  Solid pine is a natural material that ages beautifully and acquires its own unique character over time.', 'table Table', 'Assets/Addressables/Furnitures/Rectangle Table.prefab'),
(12, 'Round Table', 199, 'Table, 103 cm', 'The sturdy metal construction with criss-crossed legs gives this table a modern look that goes just as well in the kitchen as in the living room. A practical choice for those who live in a small space.', 'table Table Table & desks', 'Assets/Addressables/Furnitures/Round Table.prefab'),
(13, 'Short Nightstand', 29.99, 'Chest of 2 drawers, 35x49 cm', 'A simple chest of drawers with a clean look that fits right in, in the bedroom or wherever you place it. Use it as an extra storage solution for smaller items – works great as a bedside table too.', 'bed Bed nightstand Nightstand Bedside tables', 'Assets/Addressables/Furnitures/Short Nightstand.prefab'),
(14, 'Single Bed', 149, 'Bed frame with slatted bed base, 90x200 cm', 'TARVA bed frame is a modern example of Scandinavian furniture tradition – a simple design and untreated wood. A timeless expression mixes nicely with a variety of other styles and furniture.', 'bed Bed Mattresses Bedding Beds bed slats Beds', 'Assets/Addressables/Furnitures/Single Bed.prefab'),
(15, 'Bookshelf', 59.99, 'Bookcase, 80x28x202 cm', 'Adjustable shelves, adapt space between shelves according to your needs.  A simple unit can be enough storage for a limited space or the foundation for a larger storage solution if your needs change.', 'bookshelf Bookshelf book Book ', 'Assets/Addressables/Furnitures/Bookshelf.prefab'),
(16, 'Table', 199, 'Extendable table, 80/120x70 cm', 'A durable dining table that makes it easy to have big dinners. A single person can extend the table and there’s plenty of room for chairs since the legs are always located at the corners of the table.', 'table Table Table & desks', 'Assets/Addressables/Furnitures/Table.prefab'),
(17, 'Thin Chair', 49.99, 'Chair', 'A comfy chair that’s sturdy, yet lightweight and stackable too. An easy match with different tables and styles and eager to please, whether it’s in the dining room, in the entrance or by your bed.', 'chair Chair Chairs', 'Assets/Addressables/Furnitures/Thin Chair.prefab'),
(18, 'Triple Flower', 9.99, 'Artificial flower, 27 cm', 'Lifelike artificial bouquet that remains just as fresh-looking and beautiful year after year.  You can bend and adjust the flowers any way you want because of the steel wires in the stems.  The stem can be shortened by means of cutting pliers.', 'flower Flower Plants & flowers', 'Assets/Addressables/Furnitures/Triple Flower.prefab'),
(19, 'Triple Cactus', 4.99, 'Artificial potted plant', 'Lifelike artificial plant that remain just as fresh-looking year after year.  Perfect if you can\'t have a live plant, but still want to enjoy the beauty of nature.', 'flower Flower Plants & flowers', 'Assets/Addressables/Furnitures/Triple Cactus.prefab'),
(20, 'Twin Wardrobe', 449, 'Wardrobe, 175x58x201 cm', 'Adjustable feet make it possible to compensate any irregularities in the floor.  Sliding doors allow more room for furniture because they don’t take any space to open.  The soft-closing mechanism is integrated into the rails and catches the doors as they run on the rails so that they open and close slowly, quietly and softly.', 'wardrobe Wardrobe', 'Assets/Addressables/Furnitures/Twin Wardrobe.prefab');

-- --------------------------------------------------------

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
CREATE TABLE IF NOT EXISTS `roles` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `role` tinyint(1) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `roles`
--

INSERT INTO `roles` (`ID`, `role`) VALUES
(0, 1);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
ql…]()
