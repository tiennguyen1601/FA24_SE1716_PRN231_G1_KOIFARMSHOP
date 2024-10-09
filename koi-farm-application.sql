-- Create Staff table
CREATE DATABASE FA24_SE1716_PRN231_G1_KOIFARMSHOP

USE FA24_SE1716_PRN231_G1_KOIFARMSHOP

CREATE TABLE Staff (
    StaffID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20) NOT NULL,
    Address NVARCHAR(255),
    Role NVARCHAR(50) NOT NULL, -- Example: Manager, Salesperson, Technician
    Status NVARCHAR(50), -- Example: Active, Inactive
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME,
);

-- Create Customer table
CREATE TABLE Customer (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20) NOT NULL,
    Address NVARCHAR(255),
    Points INT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME,
    ProfileURL NVARCHAR(255),
    LoyaltyLevel NVARCHAR(50),
	Status VARCHAR (50)
);

-- Create Category table
CREATE TABLE Category (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
	Status VARCHAR (50)
);

-- Create Products table
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Price DECIMAL(18, 2) NOT NULL,
    StockQuantity INT NOT NULL,
    Brand NVARCHAR(100),
    Weight DECIMAL(18, 2),
    Discount DECIMAL(5, 2),
    ExpiryDate DATE,
    ManufacturingDate DATE,
    CategoryID INT FOREIGN KEY REFERENCES Category(CategoryID),
    Status NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME,
	CreatedBy INT FOREIGN KEY REFERENCES Staff(StaffID),
	ModifiedBy INT FOREIGN KEY REFERENCES Staff(StaffID)
);

-- Create ProductImages table
CREATE TABLE ProductImages (
    PImageID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
    ImageURL NVARCHAR(255) NOT NULL
);

-- Create Promotion table
CREATE TABLE Promotion (
    PromotionID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    StartDate DATE,
    EndDate DATE,
    DiscountPercent DECIMAL(5, 2),
    Status NVARCHAR(50)
);

-- Create Order table
CREATE TABLE [Order] (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT FOREIGN KEY REFERENCES Customer(CustomerID),
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(18, 2) NOT NULL,
    PromotionID INT FOREIGN KEY REFERENCES Promotion(PromotionID),
    ShippingAddress NVARCHAR(255),
    DeliveryMethod NVARCHAR(50),
    PaymentStatus NVARCHAR(50),
    VAT DECIMAL(5, 2),
    TotalAmountVAT DECIMAL(18, 2),
	Status NVARCHAR(50)
);

-- Create OrderPromotion table
CREATE TABLE OrderPromotion (
    OrderPromotionID INT PRIMARY KEY IDENTITY(1,1),
    PromotionID INT FOREIGN KEY REFERENCES Promotion(PromotionID),
    OrderID INT FOREIGN KEY REFERENCES [Order](OrderID)
);

-- Create Payment table
CREATE TABLE Payment (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES [Order](OrderID),
    CustomerID INT FOREIGN KEY REFERENCES Customer(CustomerID),
    Method NVARCHAR(50),
    Status NVARCHAR(50),
    TransactionID NVARCHAR(100),
    PaymentDate DATETIME DEFAULT GETDATE(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME
);

-- Create Animals table
CREATE TABLE Animals (
    AnimalID INT PRIMARY KEY IDENTITY(1,1),
    Origin NVARCHAR(100),
    Species NVARCHAR(100),
    Type NVARCHAR(50),
    Gender NVARCHAR(10),
    Size NVARCHAR(50),
    Certificate NVARCHAR(255),
    Price DECIMAL(18, 2),
    Status NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME,
    MaintenanceCost DECIMAL(18, 2),
    Color NVARCHAR(50),
    AmountFeed DECIMAL(18, 2),
    HealthStatus NVARCHAR(255),
    FarmOrigin NVARCHAR(100),
	BirthYear INT,
    Description NVARCHAR(255),
	CreatedBy INT FOREIGN KEY REFERENCES Staff(StaffID),
	ModifiedBy INT FOREIGN KEY REFERENCES Staff(StaffID)
);

-- Create AnimalImages table
CREATE TABLE AnimalImages (
    AImageID INT PRIMARY KEY IDENTITY(1,1),
    AnimalID INT FOREIGN KEY REFERENCES Animals(AnimalID),
    ImageURL NVARCHAR(255) NOT NULL
);

-- Create Consignment table
CREATE TABLE Consignment (
    ConsignmentID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT FOREIGN KEY REFERENCES Customer(CustomerID),
    AnimalID INT FOREIGN KEY REFERENCES Animals(AnimalID),
    ConsignmentType NVARCHAR(50), -- Enum: Online, Offline
    StartDate DATE,
    EndDate DATE,
    Price DECIMAL(18, 2),
    Status NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME,
    Notes NVARCHAR(255),
    CommissionRate DECIMAL(5, 2),
    OrderID INT FOREIGN KEY REFERENCES [Order](OrderID)
);


-- Create OrderDetails table
CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES [Order](OrderID),
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
	AnimalID INT FOREIGN KEY REFERENCES Animals(AnimalID),
    Quantity INT NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    Amount DECIMAL(18, 2),
    Subtotal DECIMAL(18, 2),
    Discount DECIMAL(5, 2)
);



-- Insert sample staff
INSERT INTO Staff (Username, Password, FullName, Email, Phone, Address, Role, Status, CreatedAt)
VALUES 
('john_doe', 'password123', 'John Doe', 'john@example.com', '1234567890', '123 Street, City', 'Manager', 'Active', GETDATE()),
('jane_smith', 'password123', 'Jane Smith', 'jane@example.com', '0987654321', '456 Avenue, City', 'Technician', 'Active', GETDATE());

-- Insert sample customers
INSERT INTO Customer (Username, Password, Name, Phone, Address, Points, CreatedAt, LoyaltyLevel, Status)
VALUES
('alice', 'passalice', 'Alice Johnson', '5555555555', '789 Road, City', 100, GETDATE(), 'Silver', 'Active'),
('bob', 'passbob', 'Bob Brown', '4444444444', '321 Boulevard, City', 50, GETDATE(), 'Bronze', 'Active');

-- Insert sample categories
INSERT INTO Category (Name, Description, Status)
VALUES
('Fruits', 'Fresh farm fruits', 'Active'),
('Vegetables', 'Organic vegetables', 'Active');

-- Insert sample products
INSERT INTO Products (Name, Description, Price, StockQuantity, Brand, Weight, Discount, ExpiryDate, ManufacturingDate, CategoryID, Status, CreatedAt, CreatedBy)
VALUES
('Apple', 'Fresh red apples', 2.50, 100, 'FarmBrand', 0.25, 0.10, '2025-12-31', '2023-10-01', 1, 'Available', GETDATE(), 1),
('Carrot', 'Organic carrots', 1.20, 200, 'GreenFarm', 0.10, 0.05, '2024-06-01', '2023-08-15', 2, 'Available', GETDATE(), 2);

-- Insert sample product images
INSERT INTO ProductImages (ProductID, ImageURL)
VALUES
(1, 'https://example.com/apple.jpg'),
(2, 'https://example.com/carrot.jpg');

-- Insert sample promotion
INSERT INTO Promotion (Title, Description, StartDate, EndDate, DiscountPercent, Status)
VALUES
('Summer Sale', '10% off on selected items', '2024-06-01', '2024-08-31', 10.00, 'Active');

-- Insert sample order
INSERT INTO [Order] (CustomerID, OrderDate, TotalAmount, PromotionID, ShippingAddress, DeliveryMethod, PaymentStatus, VAT, TotalAmountVAT, Status)
VALUES
(1, GETDATE(), 50.00, 1, '777 Road, City', 'Express', 'Paid', 5.00, 52.50, 'Delivered');

-- Insert sample order promotion
INSERT INTO OrderPromotion (PromotionID, OrderID)
VALUES
(1, 1);

-- Insert sample payment
INSERT INTO Payment (OrderID, CustomerID, Method, Status, TransactionID, PaymentDate, CreatedAt)
VALUES
(1, 1, 'Credit Card', 'Completed', 'TXN123456', GETDATE(), GETDATE());

-- Insert sample animals
INSERT INTO Animals (Origin, Species, Type, Gender, Size, Certificate, Price, Status, CreatedAt, MaintenanceCost, Color, AmountFeed, HealthStatus, FarmOrigin, BirthYear, Description, CreatedBy)
VALUES
('Vietnam', 'Pig', 'Farm Animal', 'Male', 'Large', 'CERT123', 300.00, 'Healthy', GETDATE(), 50.00, 'Pink', 2.5, 'Good', 'FarmA', 2021, 'Healthy male pig', 1);

-- Insert sample animal images
INSERT INTO AnimalImages (AnimalID, ImageURL)
VALUES
(1, 'https://example.com/pig.jpg');

-- Insert sample consignment
INSERT INTO Consignment (CustomerID, AnimalID, ConsignmentType, StartDate, EndDate, Price, Status, CreatedAt, Notes, CommissionRate, OrderID)
VALUES
(1, 1, 'Online', '2024-01-01', '2024-12-31', 500.00, 'Active', GETDATE(), 'Online consignment', 5.00, 1);

-- Insert sample order details
INSERT INTO OrderDetails (OrderID, ProductID, AnimalID, Quantity, Price, Amount, Subtotal, Discount)
VALUES
(1, 1, NULL, 5, 2.50, 12.50, 11.25, 10.00),
(1, NULL, 1, 1, 300.00, 300.00, 285.00, 5.00);
