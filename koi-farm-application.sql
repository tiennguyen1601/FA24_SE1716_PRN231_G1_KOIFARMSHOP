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