USE [master]
GO
/****** Object:  Database [FA24_SE1716_PRN231_G1_KOIFARMSHOP]    Script Date: 10/22/2024 12:09:19 PM ******/
CREATE DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FA24_SE1716_PRN231_G1_KOIFARMSHOP', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\FA24_SE1716_PRN231_G1_KOIFARMSHOP.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FA24_SE1716_PRN231_G1_KOIFARMSHOP_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\FA24_SE1716_PRN231_G1_KOIFARMSHOP_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FA24_SE1716_PRN231_G1_KOIFARMSHOP].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET ARITHABORT OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET  ENABLE_BROKER 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET  MULTI_USER 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET QUERY_STORE = ON
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [FA24_SE1716_PRN231_G1_KOIFARMSHOP]
GO
/****** Object:  Table [dbo].[AnimalImages]    Script Date: 10/22/2024 12:09:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnimalImages](
	[AImageID] [int] IDENTITY(1,1) NOT NULL,
	[AnimalID] [int] NULL,
	[ImageURL] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AImageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Animals]    Script Date: 10/22/2024 12:09:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Animals](
	[AnimalID] [int] IDENTITY(1,1) NOT NULL,
	[Origin] [nvarchar](100) NULL,
	[Name] [nvarchar](100) NULL,
	[Species] [nvarchar](100) NULL,
	[Type] [nvarchar](50) NULL,
	[Gender] [nvarchar](10) NULL,
	[Size] [nvarchar](50) NULL,
	[Certificate] [nvarchar](255) NULL,
	[Price] [decimal](18, 2) NULL,
	[Status] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[MaintenanceCost] [decimal](18, 2) NULL,
	[Color] [nvarchar](50) NULL,
	[AmountFeed] [decimal](18, 2) NULL,
	[HealthStatus] [nvarchar](255) NULL,
	[FarmOrigin] [nvarchar](100) NULL,
	[BirthYear] [int] NULL,
	[Description] [nvarchar](255) NULL,
	[CreatedBy] [int] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[AnimalID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 10/22/2024 12:09:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Status] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Consignment]    Script Date: 10/22/2024 12:09:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consignment](
	[ConsignmentID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NULL,
	[AnimalID] [int] NULL,
	[ConsignmentType] [nvarchar](50) NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[Price] [decimal](18, 2) NULL,
	[Status] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[Notes] [nvarchar](255) NULL,
	[CommissionRate] [decimal](5, 2) NULL,
	[OrderID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ConsignmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 10/22/2024 12:09:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[Address] [nvarchar](255) NULL,
	[Points] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[ProfileURL] [nvarchar](255) NULL,
	[LoyaltyLevel] [nvarchar](50) NULL,
	[Status] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 10/22/2024 12:09:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NULL,
	[OrderDate] [datetime] NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[PromotionID] [int] NULL,
	[ShippingAddress] [nvarchar](255) NULL,
	[DeliveryMethod] [nvarchar](50) NULL,
	[PaymentStatus] [nvarchar](50) NULL,
	[VAT] [decimal](5, 2) NULL,
	[TotalAmountVAT] [decimal](18, 2) NULL,
	[Status] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 10/22/2024 12:09:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[ProductID] [int] NULL,
	[AnimalID] [int] NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Amount] [decimal](18, 2) NULL,
	[Subtotal] [decimal](18, 2) NULL,
	[Discount] [decimal](5, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderPromotion]    Script Date: 10/22/2024 12:09:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderPromotion](
	[OrderPromotionID] [int] IDENTITY(1,1) NOT NULL,
	[PromotionID] [int] NULL,
	[OrderID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderPromotionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 10/22/2024 12:09:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[PaymentID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[CustomerID] [int] NULL,
	[Method] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[TransactionID] [nvarchar](100) NULL,
	[PaymentDate] [datetime] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductImages]    Script Date: 10/22/2024 12:09:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductImages](
	[PImageID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[ImageURL] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PImageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 10/22/2024 12:09:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[StockQuantity] [int] NOT NULL,
	[Brand] [nvarchar](100) NULL,
	[Weight] [decimal](18, 2) NULL,
	[Discount] [decimal](5, 2) NULL,
	[ExpiryDate] [date] NULL,
	[ManufacturingDate] [date] NULL,
	[CategoryID] [int] NULL,
	[Status] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Promotion]    Script Date: 10/22/2024 12:09:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Promotion](
	[PromotionID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[DiscountPercent] [decimal](5, 2) NULL,
	[Status] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[PromotionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 10/22/2024 12:09:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[StaffID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[Address] [nvarchar](255) NULL,
	[Role] [nvarchar](50) NOT NULL,
	[Status] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[StaffID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AnimalImages] ON 

INSERT [dbo].[AnimalImages] ([AImageID], [AnimalID], [ImageURL]) VALUES (1, 1, N'https://example.com/pig.jpg')
INSERT [dbo].[AnimalImages] ([AImageID], [AnimalID], [ImageURL]) VALUES (2, 2, N'https://example.com/cow.jpg')
INSERT [dbo].[AnimalImages] ([AImageID], [AnimalID], [ImageURL]) VALUES (3, 3, N'https://example.com/sheep.jpg')
INSERT [dbo].[AnimalImages] ([AImageID], [AnimalID], [ImageURL]) VALUES (4, 4, N'https://example.com/chicken.jpg')
INSERT [dbo].[AnimalImages] ([AImageID], [AnimalID], [ImageURL]) VALUES (5, 5, N'https://example.com/goat.jpg')
SET IDENTITY_INSERT [dbo].[AnimalImages] OFF
GO
SET IDENTITY_INSERT [dbo].[Animals] ON 

INSERT [dbo].[Animals] ([AnimalID], [Origin], [Name], [Species], [Type], [Gender], [Size], [Certificate], [Price], [Status], [CreatedAt], [UpdatedAt], [MaintenanceCost], [Color], [AmountFeed], [HealthStatus], [FarmOrigin], [BirthYear], [Description], [CreatedBy], [ModifiedBy]) VALUES (1, N'Japan', N'Sakura', N'Koi', N'Fish', N'Female', N'Medium', N'CERT123', CAST(1500.00 AS Decimal(18, 2)), N'Healthy', CAST(N'2024-10-19T07:06:56.273' AS DateTime), NULL, CAST(100.00 AS Decimal(18, 2)), N'Red and White', CAST(0.50 AS Decimal(18, 2)), N'Excellent', N'FarmA', 2020, N'Beautiful red and white female koi', 1, NULL)
INSERT [dbo].[Animals] ([AnimalID], [Origin], [Name], [Species], [Type], [Gender], [Size], [Certificate], [Price], [Status], [CreatedAt], [UpdatedAt], [MaintenanceCost], [Color], [AmountFeed], [HealthStatus], [FarmOrigin], [BirthYear], [Description], [CreatedBy], [ModifiedBy]) VALUES (2, N'Vietnam', N'Ryujin', N'Koi', N'Fish', N'Male', N'Large', N'CERT456', CAST(2000.00 AS Decimal(18, 2)), N'Healthy', CAST(N'2024-10-19T07:06:56.273' AS DateTime), NULL, CAST(120.00 AS Decimal(18, 2)), N'Black and White', CAST(0.75 AS Decimal(18, 2)), N'Good', N'FarmB', 2021, N'Majestic black and white male koi', 2, NULL)
INSERT [dbo].[Animals] ([AnimalID], [Origin], [Name], [Species], [Type], [Gender], [Size], [Certificate], [Price], [Status], [CreatedAt], [UpdatedAt], [MaintenanceCost], [Color], [AmountFeed], [HealthStatus], [FarmOrigin], [BirthYear], [Description], [CreatedBy], [ModifiedBy]) VALUES (3, N'Thailand', N'Akira', N'Koi', N'Fish', N'Male', N'Small', N'CERT789', CAST(1000.00 AS Decimal(18, 2)), N'Healthy', CAST(N'2024-10-19T07:06:56.273' AS DateTime), NULL, CAST(80.00 AS Decimal(18, 2)), N'Orange', CAST(0.40 AS Decimal(18, 2)), N'Good', N'FarmC', 2019, N'Bright orange koi with smooth scales', 1, NULL)
INSERT [dbo].[Animals] ([AnimalID], [Origin], [Name], [Species], [Type], [Gender], [Size], [Certificate], [Price], [Status], [CreatedAt], [UpdatedAt], [MaintenanceCost], [Color], [AmountFeed], [HealthStatus], [FarmOrigin], [BirthYear], [Description], [CreatedBy], [ModifiedBy]) VALUES (4, N'Japan', N'Hoshi', N'Koi', N'Fish', N'Female', N'Large', N'CERT321', CAST(2500.00 AS Decimal(18, 2)), N'Healthy', CAST(N'2024-10-19T07:06:56.273' AS DateTime), NULL, CAST(150.00 AS Decimal(18, 2)), N'Blue and White', CAST(0.65 AS Decimal(18, 2)), N'Excellent', N'FarmD', 2020, N'Rare blue and white female koi', 2, NULL)
INSERT [dbo].[Animals] ([AnimalID], [Origin], [Name], [Species], [Type], [Gender], [Size], [Certificate], [Price], [Status], [CreatedAt], [UpdatedAt], [MaintenanceCost], [Color], [AmountFeed], [HealthStatus], [FarmOrigin], [BirthYear], [Description], [CreatedBy], [ModifiedBy]) VALUES (5, N'China', N'Kumo', N'Koi', N'Fish', N'Male', N'Medium', N'CERT654', CAST(1800.00 AS Decimal(18, 2)), N'Healthy', CAST(N'2024-10-19T07:06:56.273' AS DateTime), NULL, CAST(110.00 AS Decimal(18, 2)), N'Gold', CAST(0.55 AS Decimal(18, 2)), N'Good', N'FarmE', 2021, N'Golden male koi with shiny scales', 1, NULL)
SET IDENTITY_INSERT [dbo].[Animals] OFF
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([CategoryID], [Name], [Description], [Status]) VALUES (1, N'Fruits', N'Fresh farm fruits', N'Active')
INSERT [dbo].[Category] ([CategoryID], [Name], [Description], [Status]) VALUES (2, N'Vegetables', N'Organic vegetables', N'Active')
INSERT [dbo].[Category] ([CategoryID], [Name], [Description], [Status]) VALUES (3, N'Dairy', N'Fresh dairy products', N'Active')
INSERT [dbo].[Category] ([CategoryID], [Name], [Description], [Status]) VALUES (4, N'Meat', N'Quality meats', N'Active')
INSERT [dbo].[Category] ([CategoryID], [Name], [Description], [Status]) VALUES (5, N'Seafood', N'Fresh seafood', N'Active')
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Consignment] ON 

INSERT [dbo].[Consignment] ([ConsignmentID], [CustomerID], [AnimalID], [ConsignmentType], [StartDate], [EndDate], [Price], [Status], [CreatedAt], [UpdatedAt], [Notes], [CommissionRate], [OrderID]) VALUES (1, 1, 1, N'Online', CAST(N'2024-01-01' AS Date), CAST(N'2024-12-31' AS Date), CAST(500.00 AS Decimal(18, 2)), N'Active', CAST(N'2024-10-19T07:06:56.290' AS DateTime), NULL, N'Online consignment for pig', CAST(5.00 AS Decimal(5, 2)), 1)
INSERT [dbo].[Consignment] ([ConsignmentID], [CustomerID], [AnimalID], [ConsignmentType], [StartDate], [EndDate], [Price], [Status], [CreatedAt], [UpdatedAt], [Notes], [CommissionRate], [OrderID]) VALUES (2, 2, 2, N'Offline', CAST(N'2024-02-01' AS Date), CAST(N'2024-12-31' AS Date), CAST(600.00 AS Decimal(18, 2)), N'Active', CAST(N'2024-10-19T07:06:56.290' AS DateTime), NULL, N'Offline consignment for cow', CAST(6.00 AS Decimal(5, 2)), 2)
INSERT [dbo].[Consignment] ([ConsignmentID], [CustomerID], [AnimalID], [ConsignmentType], [StartDate], [EndDate], [Price], [Status], [CreatedAt], [UpdatedAt], [Notes], [CommissionRate], [OrderID]) VALUES (3, 3, 3, N'Online', CAST(N'2024-03-01' AS Date), CAST(N'2024-12-31' AS Date), CAST(300.00 AS Decimal(18, 2)), N'Active', CAST(N'2024-10-19T07:06:56.290' AS DateTime), NULL, N'Online consignment for sheep', CAST(4.00 AS Decimal(5, 2)), 3)
INSERT [dbo].[Consignment] ([ConsignmentID], [CustomerID], [AnimalID], [ConsignmentType], [StartDate], [EndDate], [Price], [Status], [CreatedAt], [UpdatedAt], [Notes], [CommissionRate], [OrderID]) VALUES (4, 4, 4, N'Offline', CAST(N'2024-04-01' AS Date), CAST(N'2024-12-31' AS Date), CAST(100.00 AS Decimal(18, 2)), N'Active', CAST(N'2024-10-19T07:06:56.290' AS DateTime), NULL, N'Offline consignment for chicken', CAST(7.00 AS Decimal(5, 2)), 4)
INSERT [dbo].[Consignment] ([ConsignmentID], [CustomerID], [AnimalID], [ConsignmentType], [StartDate], [EndDate], [Price], [Status], [CreatedAt], [UpdatedAt], [Notes], [CommissionRate], [OrderID]) VALUES (5, 5, 5, N'Online', CAST(N'2024-05-01' AS Date), CAST(N'2024-12-31' AS Date), CAST(400.00 AS Decimal(18, 2)), N'Active', CAST(N'2024-10-19T07:06:56.290' AS DateTime), NULL, N'Online consignment for goat', CAST(5.00 AS Decimal(5, 2)), 5)
SET IDENTITY_INSERT [dbo].[Consignment] OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([CustomerID], [Username], [Password], [Name], [Email], [Phone], [Address], [Points], [CreatedAt], [UpdatedAt], [ProfileURL], [LoyaltyLevel], [Status]) VALUES (1, N'alice_j', N'passwordalice', N'Alice Johnson', NULL, N'5551234567', N'123 Street, City', 100, CAST(N'2024-10-19T07:06:56.203' AS DateTime), NULL, NULL, N'Silver', N'Active')
INSERT [dbo].[Customer] ([CustomerID], [Username], [Password], [Name], [Email], [Phone], [Address], [Points], [CreatedAt], [UpdatedAt], [ProfileURL], [LoyaltyLevel], [Status]) VALUES (2, N'bob_b', N'passwordbob', N'Bob Brown', NULL, N'5552345678', N'456 Avenue, City', 50, CAST(N'2024-10-19T07:06:56.203' AS DateTime), NULL, NULL, N'Bronze', N'Active')
INSERT [dbo].[Customer] ([CustomerID], [Username], [Password], [Name], [Email], [Phone], [Address], [Points], [CreatedAt], [UpdatedAt], [ProfileURL], [LoyaltyLevel], [Status]) VALUES (3, N'charlie_k', N'passwordcharlie', N'Charlie King', NULL, N'5553456789', N'789 Lane, City', 200, CAST(N'2024-10-19T07:06:56.203' AS DateTime), NULL, NULL, N'Gold', N'Active')
INSERT [dbo].[Customer] ([CustomerID], [Username], [Password], [Name], [Email], [Phone], [Address], [Points], [CreatedAt], [UpdatedAt], [ProfileURL], [LoyaltyLevel], [Status]) VALUES (4, N'diana_p', N'passworddiana', N'Diana Prince', NULL, N'5554567890', N'1010 Boulevard, City', 150, CAST(N'2024-10-19T07:06:56.203' AS DateTime), NULL, NULL, N'Silver', N'Active')
INSERT [dbo].[Customer] ([CustomerID], [Username], [Password], [Name], [Email], [Phone], [Address], [Points], [CreatedAt], [UpdatedAt], [ProfileURL], [LoyaltyLevel], [Status]) VALUES (5, N'edward_t', N'passwordedward', N'Edward Turner', NULL, N'5555678901', N'2020 Drive, City', 300, CAST(N'2024-10-19T07:06:56.203' AS DateTime), NULL, NULL, N'Platinum', N'Inactive')
INSERT [dbo].[Customer] ([CustomerID], [Username], [Password], [Name], [Email], [Phone], [Address], [Points], [CreatedAt], [UpdatedAt], [ProfileURL], [LoyaltyLevel], [Status]) VALUES (6, N'alice', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'Alice Johnson', NULL, N'5555555555', N'789 Road, City', 100, CAST(N'2024-10-19T10:05:41.903' AS DateTime), NULL, NULL, N'Silver', N'Active')
INSERT [dbo].[Customer] ([CustomerID], [Username], [Password], [Name], [Email], [Phone], [Address], [Points], [CreatedAt], [UpdatedAt], [ProfileURL], [LoyaltyLevel], [Status]) VALUES (7, N'bob', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'Bob Brown', NULL, N'4444444444', N'321 Boulevard, City', 50, CAST(N'2024-10-19T10:05:41.903' AS DateTime), NULL, NULL, N'Bronze', N'Active')
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PromotionID], [ShippingAddress], [DeliveryMethod], [PaymentStatus], [VAT], [TotalAmountVAT], [Status]) VALUES (1, 1, CAST(N'2024-10-19T07:06:56.250' AS DateTime), CAST(50.00 AS Decimal(18, 2)), 1, N'123 Road, City', N'Express', N'Paid', CAST(5.00 AS Decimal(5, 2)), CAST(52.50 AS Decimal(18, 2)), N'Delivered')
INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PromotionID], [ShippingAddress], [DeliveryMethod], [PaymentStatus], [VAT], [TotalAmountVAT], [Status]) VALUES (2, 2, CAST(N'2024-10-19T07:06:56.250' AS DateTime), CAST(30.00 AS Decimal(18, 2)), 2, N'456 Avenue, City', N'Standard', N'Pending', CAST(3.00 AS Decimal(5, 2)), CAST(31.50 AS Decimal(18, 2)), N'Processing')
INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PromotionID], [ShippingAddress], [DeliveryMethod], [PaymentStatus], [VAT], [TotalAmountVAT], [Status]) VALUES (3, 3, CAST(N'2024-10-19T07:06:56.250' AS DateTime), CAST(100.00 AS Decimal(18, 2)), 3, N'789 Lane, City', N'Express', N'Paid', CAST(10.00 AS Decimal(5, 2)), CAST(105.00 AS Decimal(18, 2)), N'Delivered')
INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PromotionID], [ShippingAddress], [DeliveryMethod], [PaymentStatus], [VAT], [TotalAmountVAT], [Status]) VALUES (4, 4, CAST(N'2024-10-19T07:06:56.250' AS DateTime), CAST(70.00 AS Decimal(18, 2)), 4, N'1010 Street, City', N'Standard', N'Paid', CAST(7.00 AS Decimal(5, 2)), CAST(73.50 AS Decimal(18, 2)), N'Shipped')
INSERT [dbo].[Order] ([OrderID], [CustomerID], [OrderDate], [TotalAmount], [PromotionID], [ShippingAddress], [DeliveryMethod], [PaymentStatus], [VAT], [TotalAmountVAT], [Status]) VALUES (5, 5, CAST(N'2024-10-19T07:06:56.250' AS DateTime), CAST(150.00 AS Decimal(18, 2)), 5, N'2020 Drive, City', N'Express', N'Paid', CAST(15.00 AS Decimal(5, 2)), CAST(157.50 AS Decimal(18, 2)), N'Delivered')
SET IDENTITY_INSERT [dbo].[Order] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetails] ON 

INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [ProductID], [AnimalID], [Quantity], [Price], [Amount], [Subtotal], [Discount]) VALUES (1, 1, 1, NULL, 5, CAST(2.50 AS Decimal(18, 2)), CAST(12.50 AS Decimal(18, 2)), CAST(11.25 AS Decimal(18, 2)), CAST(10.00 AS Decimal(5, 2)))
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [ProductID], [AnimalID], [Quantity], [Price], [Amount], [Subtotal], [Discount]) VALUES (2, 2, 2, NULL, 10, CAST(1.20 AS Decimal(18, 2)), CAST(12.00 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), CAST(8.00 AS Decimal(5, 2)))
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [ProductID], [AnimalID], [Quantity], [Price], [Amount], [Subtotal], [Discount]) VALUES (3, 3, 3, NULL, 2, CAST(3.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)), CAST(5.50 AS Decimal(18, 2)), CAST(5.00 AS Decimal(5, 2)))
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [ProductID], [AnimalID], [Quantity], [Price], [Amount], [Subtotal], [Discount]) VALUES (4, 4, NULL, 1, 1, CAST(300.00 AS Decimal(18, 2)), CAST(300.00 AS Decimal(18, 2)), CAST(285.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(5, 2)))
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [ProductID], [AnimalID], [Quantity], [Price], [Amount], [Subtotal], [Discount]) VALUES (5, 5, NULL, 2, 1, CAST(500.00 AS Decimal(18, 2)), CAST(500.00 AS Decimal(18, 2)), CAST(475.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(5, 2)))
SET IDENTITY_INSERT [dbo].[OrderDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderPromotion] ON 

INSERT [dbo].[OrderPromotion] ([OrderPromotionID], [PromotionID], [OrderID]) VALUES (1, 1, 1)
INSERT [dbo].[OrderPromotion] ([OrderPromotionID], [PromotionID], [OrderID]) VALUES (2, 2, 2)
INSERT [dbo].[OrderPromotion] ([OrderPromotionID], [PromotionID], [OrderID]) VALUES (3, 3, 3)
INSERT [dbo].[OrderPromotion] ([OrderPromotionID], [PromotionID], [OrderID]) VALUES (4, 4, 4)
INSERT [dbo].[OrderPromotion] ([OrderPromotionID], [PromotionID], [OrderID]) VALUES (5, 5, 5)
SET IDENTITY_INSERT [dbo].[OrderPromotion] OFF
GO
SET IDENTITY_INSERT [dbo].[Payment] ON 

INSERT [dbo].[Payment] ([PaymentID], [OrderID], [CustomerID], [Method], [Status], [TransactionID], [PaymentDate], [CreatedAt], [UpdatedAt]) VALUES (1, 1, 1, N'Credit Card', N'Completed', N'TXN123', CAST(N'2024-10-19T07:06:56.267' AS DateTime), CAST(N'2024-10-19T07:06:56.267' AS DateTime), NULL)
INSERT [dbo].[Payment] ([PaymentID], [OrderID], [CustomerID], [Method], [Status], [TransactionID], [PaymentDate], [CreatedAt], [UpdatedAt]) VALUES (2, 2, 2, N'PayPal', N'Pending', N'TXN124', CAST(N'2024-10-19T07:06:56.267' AS DateTime), CAST(N'2024-10-19T07:06:56.267' AS DateTime), NULL)
INSERT [dbo].[Payment] ([PaymentID], [OrderID], [CustomerID], [Method], [Status], [TransactionID], [PaymentDate], [CreatedAt], [UpdatedAt]) VALUES (3, 3, 3, N'Bank Transfer', N'Completed', N'TXN125', CAST(N'2024-10-19T07:06:56.267' AS DateTime), CAST(N'2024-10-19T07:06:56.267' AS DateTime), NULL)
INSERT [dbo].[Payment] ([PaymentID], [OrderID], [CustomerID], [Method], [Status], [TransactionID], [PaymentDate], [CreatedAt], [UpdatedAt]) VALUES (4, 4, 4, N'Credit Card', N'Completed', N'TXN126', CAST(N'2024-10-19T07:06:56.267' AS DateTime), CAST(N'2024-10-19T07:06:56.267' AS DateTime), NULL)
INSERT [dbo].[Payment] ([PaymentID], [OrderID], [CustomerID], [Method], [Status], [TransactionID], [PaymentDate], [CreatedAt], [UpdatedAt]) VALUES (5, 5, 5, N'Cash on Delivery', N'Pending', N'TXN127', CAST(N'2024-10-19T07:06:56.267' AS DateTime), CAST(N'2024-10-19T07:06:56.267' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Payment] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductImages] ON 

INSERT [dbo].[ProductImages] ([PImageID], [ProductID], [ImageURL]) VALUES (1, 1, N'https://example.com/apple.jpg')
INSERT [dbo].[ProductImages] ([PImageID], [ProductID], [ImageURL]) VALUES (2, 2, N'https://example.com/carrot.jpg')
INSERT [dbo].[ProductImages] ([PImageID], [ProductID], [ImageURL]) VALUES (3, 3, N'https://example.com/milk.jpg')
INSERT [dbo].[ProductImages] ([PImageID], [ProductID], [ImageURL]) VALUES (4, 4, N'https://example.com/beef.jpg')
INSERT [dbo].[ProductImages] ([PImageID], [ProductID], [ImageURL]) VALUES (5, 5, N'https://example.com/salmon.jpg')
SET IDENTITY_INSERT [dbo].[ProductImages] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [StockQuantity], [Brand], [Weight], [Discount], [ExpiryDate], [ManufacturingDate], [CategoryID], [Status], [CreatedAt], [UpdatedAt], [CreatedBy], [ModifiedBy]) VALUES (1, N'Apple', N'Fresh red apples', CAST(2.50 AS Decimal(18, 2)), 100, N'FarmBrand', CAST(0.25 AS Decimal(18, 2)), CAST(0.10 AS Decimal(5, 2)), CAST(N'2025-12-31' AS Date), CAST(N'2023-10-01' AS Date), 1, N'Available', CAST(N'2024-10-19T07:06:56.227' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [StockQuantity], [Brand], [Weight], [Discount], [ExpiryDate], [ManufacturingDate], [CategoryID], [Status], [CreatedAt], [UpdatedAt], [CreatedBy], [ModifiedBy]) VALUES (2, N'Carrot', N'Organic carrots', CAST(1.20 AS Decimal(18, 2)), 200, N'GreenFarm', CAST(0.10 AS Decimal(18, 2)), CAST(0.05 AS Decimal(5, 2)), CAST(N'2024-06-01' AS Date), CAST(N'2023-08-15' AS Date), 2, N'Available', CAST(N'2024-10-19T07:06:56.227' AS DateTime), NULL, 2, NULL)
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [StockQuantity], [Brand], [Weight], [Discount], [ExpiryDate], [ManufacturingDate], [CategoryID], [Status], [CreatedAt], [UpdatedAt], [CreatedBy], [ModifiedBy]) VALUES (3, N'Milk', N'Fresh cow milk', CAST(3.00 AS Decimal(18, 2)), 150, N'DairyLand', CAST(1.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(5, 2)), CAST(N'2024-01-01' AS Date), CAST(N'2023-10-05' AS Date), 3, N'Available', CAST(N'2024-10-19T07:06:56.227' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [StockQuantity], [Brand], [Weight], [Discount], [ExpiryDate], [ManufacturingDate], [CategoryID], [Status], [CreatedAt], [UpdatedAt], [CreatedBy], [ModifiedBy]) VALUES (4, N'Beef', N'Grass-fed beef', CAST(10.00 AS Decimal(18, 2)), 50, N'MeatMaster', CAST(1.50 AS Decimal(18, 2)), CAST(0.20 AS Decimal(5, 2)), CAST(N'2024-01-31' AS Date), CAST(N'2023-10-20' AS Date), 4, N'Available', CAST(N'2024-10-19T07:06:56.227' AS DateTime), NULL, 2, NULL)
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [StockQuantity], [Brand], [Weight], [Discount], [ExpiryDate], [ManufacturingDate], [CategoryID], [Status], [CreatedAt], [UpdatedAt], [CreatedBy], [ModifiedBy]) VALUES (5, N'Salmon', N'Fresh wild-caught salmon', CAST(15.00 AS Decimal(18, 2)), 80, N'SeaFoodDelight', CAST(0.50 AS Decimal(18, 2)), CAST(0.15 AS Decimal(5, 2)), CAST(N'2024-03-31' AS Date), CAST(N'2023-11-10' AS Date), 5, N'Available', CAST(N'2024-10-19T07:06:56.227' AS DateTime), NULL, 1, NULL)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[Promotion] ON 

INSERT [dbo].[Promotion] ([PromotionID], [Title], [Description], [StartDate], [EndDate], [DiscountPercent], [Status]) VALUES (1, N'Summer Sale', N'10% off on selected items', CAST(N'2024-06-01' AS Date), CAST(N'2024-08-31' AS Date), CAST(10.00 AS Decimal(5, 2)), N'Active')
INSERT [dbo].[Promotion] ([PromotionID], [Title], [Description], [StartDate], [EndDate], [DiscountPercent], [Status]) VALUES (2, N'Winter Discount', N'15% off dairy products', CAST(N'2024-01-01' AS Date), CAST(N'2024-03-31' AS Date), CAST(15.00 AS Decimal(5, 2)), N'Active')
INSERT [dbo].[Promotion] ([PromotionID], [Title], [Description], [StartDate], [EndDate], [DiscountPercent], [Status]) VALUES (3, N'Meat Feast', N'20% off all meats', CAST(N'2024-02-01' AS Date), CAST(N'2024-02-28' AS Date), CAST(20.00 AS Decimal(5, 2)), N'Upcoming')
INSERT [dbo].[Promotion] ([PromotionID], [Title], [Description], [StartDate], [EndDate], [DiscountPercent], [Status]) VALUES (4, N'Seafood Bonanza', N'25% off seafood', CAST(N'2024-03-01' AS Date), CAST(N'2024-04-30' AS Date), CAST(25.00 AS Decimal(5, 2)), N'Active')
INSERT [dbo].[Promotion] ([PromotionID], [Title], [Description], [StartDate], [EndDate], [DiscountPercent], [Status]) VALUES (5, N'Holiday Sale', N'15% off all items', CAST(N'2024-12-01' AS Date), CAST(N'2024-12-31' AS Date), CAST(15.00 AS Decimal(5, 2)), N'Upcoming')
SET IDENTITY_INSERT [dbo].[Promotion] OFF
GO
SET IDENTITY_INSERT [dbo].[Staff] ON 

INSERT [dbo].[Staff] ([StaffID], [Username], [Password], [FullName], [Email], [Phone], [Address], [Role], [Status], [CreatedAt], [UpdatedAt]) VALUES (1, N'john_doe', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'John Doe', N'john@example.com', N'1234567890', N'123 Street, City', N'Manager', N'Active', CAST(N'2024-10-19T07:06:56.197' AS DateTime), NULL)
INSERT [dbo].[Staff] ([StaffID], [Username], [Password], [FullName], [Email], [Phone], [Address], [Role], [Status], [CreatedAt], [UpdatedAt]) VALUES (2, N'jane_smith', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'Jane Smith', N'jane@example.com', N'0987654321', N'456 Avenue, City', N'Technician', N'Active', CAST(N'2024-10-19T07:06:56.197' AS DateTime), NULL)
INSERT [dbo].[Staff] ([StaffID], [Username], [Password], [FullName], [Email], [Phone], [Address], [Role], [Status], [CreatedAt], [UpdatedAt]) VALUES (3, N'michael_brown', N'b665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae4', N'Michael Brown', N'michael@example.com', N'5555551234', N'789 Lane, City', N'Salesperson', N'Active', CAST(N'2024-10-19T07:06:56.197' AS DateTime), NULL)
INSERT [dbo].[Staff] ([StaffID], [Username], [Password], [FullName], [Email], [Phone], [Address], [Role], [Status], [CreatedAt], [UpdatedAt]) VALUES (4, N'susan_green', N'c665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae5', N'Susan Green', N'susan@example.com', N'6666661234', N'1010 Avenue, City', N'Technician', N'Active', CAST(N'2024-10-19T07:06:56.197' AS DateTime), NULL)
INSERT [dbo].[Staff] ([StaffID], [Username], [Password], [FullName], [Email], [Phone], [Address], [Role], [Status], [CreatedAt], [UpdatedAt]) VALUES (5, N'anna_wong', N'd665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae6', N'Anna Wong', N'anna@example.com', N'7777771234', N'2020 Street, City', N'Manager', N'Inactive', CAST(N'2024-10-19T07:06:56.197' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Staff] OFF
GO
ALTER TABLE [dbo].[Animals] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Consignment] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Customer] ADD  DEFAULT ((0)) FOR [Points]
GO
ALTER TABLE [dbo].[Customer] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[Payment] ADD  DEFAULT (getdate()) FOR [PaymentDate]
GO
ALTER TABLE [dbo].[Payment] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Staff] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[AnimalImages]  WITH CHECK ADD FOREIGN KEY([AnimalID])
REFERENCES [dbo].[Animals] ([AnimalID])
GO
ALTER TABLE [dbo].[Animals]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Staff] ([StaffID])
GO
ALTER TABLE [dbo].[Animals]  WITH CHECK ADD FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Staff] ([StaffID])
GO
ALTER TABLE [dbo].[Consignment]  WITH CHECK ADD FOREIGN KEY([AnimalID])
REFERENCES [dbo].[Animals] ([AnimalID])
GO
ALTER TABLE [dbo].[Consignment]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[Consignment]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([PromotionID])
REFERENCES [dbo].[Promotion] ([PromotionID])
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD FOREIGN KEY([AnimalID])
REFERENCES [dbo].[Animals] ([AnimalID])
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[OrderPromotion]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO
ALTER TABLE [dbo].[OrderPromotion]  WITH CHECK ADD FOREIGN KEY([PromotionID])
REFERENCES [dbo].[Promotion] ([PromotionID])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO
ALTER TABLE [dbo].[ProductImages]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([CategoryID])
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Staff] ([StaffID])
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Staff] ([StaffID])
GO
USE [master]
GO
ALTER DATABASE [FA24_SE1716_PRN231_G1_KOIFARMSHOP] SET  READ_WRITE 
GO
