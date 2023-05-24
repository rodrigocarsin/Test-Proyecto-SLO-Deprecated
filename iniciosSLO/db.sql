CREATE DATABASE SLO;

USE SLO;

CREATE TABLE products(
	id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    descripcion VARCHAR(255) NOT NULL,
    product_code VARCHAR(255) NOT NULL,
    product_type_id int NOT NULL,
	seller_id int NOT NULL
    );

