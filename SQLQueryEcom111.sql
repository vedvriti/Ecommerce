CREATE
TABLE
Customers (     CustomerId
INT
PRIMARY
KEY
IDENTITY
(1,1),     Name NVARCHAR(255)
NOT
NULL
,     Email NVARCHAR(255)
NOT
NULL
,     Address NVARCHAR(500)
NOT
NULL
,     IsDeleted BIT
NOT
NULL
DEFAULT
0,
CONSTRAINT
UQ_Customers_Email
UNIQUE
(Email) );


CREATE TABLE Products (     ProductId INT PRIMARY KEY IDENTITY(1,1), ProductImage VARCHAR(1000),    Name NVARCHAR(255) NOT NULL,     Price DECIMAL(10, 2) NOT NULL,     Quantity INT NOT NULL,     Description NVARCHAR(MAX),    IsDeleted BIT NOT NULL DEFAULT 0, CONSTRAINT UQ_Products_Name UNIQUE (Name) );

CREATE
TABLE
Orders (     OrderId
INT
IDENTITY
(1,1)
PRIMARY
KEY,     CustomerId
INT
NOT
NULL
,     TotalAmount
DECIMAL
(10, 2)
NOT
NULL
,     Status NVARCHAR(50)
NOT
NULL
DEFAULT
'Pending'
,     OrderDate DATETIME
NOT
NULL
,
FOREIGN
KEY (CustomerId)
REFERENCES
Customers(CustomerId) );
CREATE
INDEX IDX_CustomerId
ON
Orders (CustomerId);

CREATE
TABLE
Payments (     PaymentId
INT
PRIMARY
KEY
IDENTITY
(1,1),     OrderId
INT
NOT
NULL
,     Amount
DECIMAL
(10, 2)
NOT
NULL
,     Status NVARCHAR(50)
NOT
NULL
DEFAULT
'Pending'
,     PaymentType NVARCHAR(50)
NOT
NULL
, PaymentDate DATETIME
NOT
NULL
,
FOREIGN
KEY (OrderId)
REFERENCES
Orders(OrderId) );

CREATE
TABLE
OrderItems (     OrderItemId
INT
IDENTITY
(1,1)
PRIMARY
KEY,     OrderId
INT
NOT
NULL
,     ProductId
INT
NOT
NULL
,     Quantity
INT
NOT
NULL
,     PriceAtOrder
DECIMAL
(10, 2)
NOT
NULL
,    
FOREIGN
KEY (OrderId)
REFERENCES
Orders(OrderId),
FOREIGN
KEY (ProductId)
REFERENCES
Products(ProductId) );
CREATE
INDEX IDX_OrderId
ON
OrderItems (OrderId);

select * from products;

INSERT INTO Customers (Name, Email, Address)VALUES ('Vedica', 'vedica@gmail.com', 'Bangalore'), ('Diksha Jaiswal', 'diksha@gmail.com', 'Uttar Pradesh'), ('Vyshnavi', 'vyshnavi@gmail.com', 'Bangalore');

select * from customers;

select * from orders;

INSERT INTO Products (ProductImage,Name,Price,Quantity,Description) VALUES ( '
1534cb86-60b2-43e3-a4b7-2f3f6dc329a1.f693b98e9f6c87b295e70e7af8a7cc5e.jpeg (2000×2000) (walmartimages.com)
' , 'OutDoor Chair' , 15999 , 10 , '
Gymax Outdoor Wooden Adirondack Chair Patio Lounge Chair w/ Armrest Natural
' ),('
OIP.Rm94187ErZ3rLBrbsdbeMwHaD4 (296×180) (bing.net)
','Temple' , 5000 , 5 , 'A beautiful Temple for your house '),('
OIP.7D8v2M2QebZLc6ERt4eiuwDREq (138×197) (bing.net)
','BookShelf' , 4000 , 16 ,'A book shelf for the book lovers'),('
OIP.BzXrujwM-gTd-h11PIaTNgHaHa (207×207) (bing.net)
' ,'Shoe Rack',2000,20,'A Shoe Rack');

INSERT
INTO
Orders (CustomerId, TotalAmount, OrderDate)
VALUES
(1, 31999.00, GETDATE()),  
-- Order Id = 1
(2, 5000.00, GETDATE()),   
-- Order Id = 2
(1, 8000.00, GETDATE());
select * from orders;

INSERT
INTO
OrderItems (OrderId, ProductId, Quantity, PriceAtOrder)
VALUES
(1, 1, 2, 15999.00),   
-- 2 laptops ordered by Customer 1
(2, 2, 1, 5000.00),    
-- 1 smartphone ordered by Customer 2
(3, 3, 2, 4000.00);
-- 2 headphones ordered by Customer 1

select * from OrderItems;

INSERT
INTO
Payments (OrderId, Amount, PaymentType, PaymentDate)
VALUES
(1, 31999.00,
'CC'
, GETDATE()),  
-- CC => Credit Card
(2, 5000.00,
'DC'
, GETDATE()),   
-- DC => Debit Card
(3, 8000.00,
'COD'
, GETDATE());
-- COD => Cash On Delivery

CREATE TABLE CartItem (
    CartItemId INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    CONSTRAINT FK_CartItem_CustomerId FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
    CONSTRAINT FK_CartItem_ProductId FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- Inserting sample data into CartItem
INSERT INTO CartItem (CustomerId, ProductId, Quantity)
VALUES
(1, 1, 1),   -- Customer 1 adds 1 Outdoor Chair to cart
(1, 4, 2),   -- Customer 1 adds 2 Shoe Racks to cart
(2, 3, 1);   -- Customer 2 adds 1 BookShelf to cart




select * from customers;
select * from orderitems;
select * from orders;
select * from Products;
select * from cartItem;


UPDATE Products
SET ProductImage='https://phonebox.com.mt/wp-content/uploads/2022/11/0021413_iphone-14-pro-max.jpeg'
WHERE ProductId=7;

INSERT INTO Products (ProductImage,Name,Price,Quantity,Description) VALUES ('https://th.bing.com/th/id/OIP.XbDObQupWfCsefC_L6ElPgHaHS?w=158&h=180&c=7&r=0&o=5&pid=1.7','Apple iPhone 12 Mini',40000,10,'Apple iPhone 12 mini (Purple, 64 GB)')

INSERT INTO Products (ProductImage,Name,Price,Quantity,Description) VALUES ('https://th.bing.com/th/id/OIP.Y8fpew5HyBf36EwuwJqWtwHaHa?w=167&h=180&c=7&r=0&o=5&pid=1.7','Apple iPhone 14 Pro',70000,10,'Apple iphone 14 pro(Black,256GB)')

INSERT INTO Products (ProductImage,Name,Price,Quantity,Description) VALUES ('https://9to5mac.com/wp-content/uploads/sites/6/2022/09/Apple-iPhone-14-Pro-iPhone-14-Pro-Max-gold-220907_inline.jpg.medium_2x.jpg?resize=93','Apple iPhone 15',80000,10,'Apple iPhone 15 (White,256GB)')

INSERT INTO Products (ProductImage,Name,Price,Quantity,Description) VALUES ('https://media.glamour.com/photos/62598e75158940e137724e6b/3:4/w_320%2Cc_limit/Apple%2520AirPods%2520Max%2520%2520(2).png','Apple Airpods',100000,10,'Apple Airpods Max Green')

INSERT INTO Products (ProductImage,Name,Price,Quantity,Description) VALUES ('https://th.bing.com/th/id/OIP.pg-z73mPCT5oynlCvAYjTgAAAA?rs=1&pid=ImgDetMain','JBL Tune 500BT',15000,10,'JBL Tune 500BT(WIRELESS , BLUE)');

INSERT INTO Products (ProductImage,Name,Price,Quantity,Description) VALUES ('https://th.bing.com/th/id/OIP.XX19j1zQ-seO5MhpCuwPswHaIM?pid=ImgDet&w=185&h=204&c=7','BOSE headphone',30000,10,'BOSE Noice Cancelling 700');

INSERT INTO Products (ProductImage,Name,Price,Quantity,Description) VALUES ('https://i.pinimg.com/736x/4f/da/41/4fda41f361fce45c7cddfc9f3256be94.jpg','JBL Airdopes',10000,10,'JBL Airdopes Pro 13');

INSERT INTO Products (ProductImage,Name,Price,Quantity,Description) VALUES ('https://img.makeupalley.com/1/1/8/1/3574012.JPG','MAC Cosmetics',2000,10,'MAC Cosmetics Matte Lipstick');

INSERT INTO Products (ProductImage,Name,Price,Quantity,Description) VALUES ('https://s3.nirnita.com/2023/12/M.A.C-Powder-Kiss-Liquid-Lipcolour-A-Little-Tamed-400x400.png','MAC Cosmetics Lipstick',3000,10,'MAC Cosmetics Liquid Matte');


INSERT INTO Products (ProductImage,Name,Price,Quantity,Description) VALUES ('https://rukminim1.flixcart.com/image/1408/1408/j30gvbk0-1/lipstick/y/2/9/4-7-enrich-matte-lipstick-lakme-original-imaeu8w3cp9uy7hr.jpeg?q=90','LAKME',800,10,'LAKME ENRICH MATTE LIPSTICK')


INSERT INTO Products (ProductImage,Name,Price,Quantity,Description) VALUES ('https://i5.walmartimages.com/asr/f518e3af-5ce4-4cea-8c07-833fac13fde4_1.2f53737bfd98da2de4f3c6b5cc20e726.jpeg','MAC FACE POWDER',4000,10,'MAC FACE POWDER(LIGHT SKIN)');

create table users(
userId INT PRIMARY KEY IDENTITY(1,1),
userName VARCHAR(255) NOT NULL,
password VARCHAR(255) NOT NULL,
email VARCHAR(255) NOT NULL,
role VARCHAR(255) NOT NULL);

INSERT INTO users(userName,password,email,role)VALUES('Vedica','ved@123','vedica@sonata.com','admin')
INSERT INTO users(userName,password,email,role)VALUES('Diksha','dik@731','dilsha@sonata.com','user')

ALTER TABLE users
ADD  email VARCHAR(255);

ALTER TABLE users DROP COLUMN email;
Select * from users;

UPDATE users SET 
email = 'vedica@gmail.com' 
WHERE userName='Vedica';

DROP TABLE users;

