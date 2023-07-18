USE [master]
GO
/****** Object:  Database [AssSales]    Script Date: 27/06/2023 11:28:48 CH ******/
CREATE DATABASE [AssSales]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AssSales', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\AssSales.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AssSales_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\AssSales_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [AssSales] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AssSales].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AssSales] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AssSales] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AssSales] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AssSales] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AssSales] SET ARITHABORT OFF 
GO
ALTER DATABASE [AssSales] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [AssSales] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AssSales] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AssSales] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AssSales] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AssSales] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AssSales] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AssSales] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AssSales] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AssSales] SET  ENABLE_BROKER 
GO
ALTER DATABASE [AssSales] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AssSales] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AssSales] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AssSales] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AssSales] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AssSales] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AssSales] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AssSales] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AssSales] SET  MULTI_USER 
GO
ALTER DATABASE [AssSales] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AssSales] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AssSales] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AssSales] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AssSales] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AssSales] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [AssSales] SET QUERY_STORE = OFF
GO
USE [AssSales]
GO
/****** Object:  Table [dbo].[Member]    Script Date: 27/06/2023 11:28:48 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Member](
	[MemberId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[CompanyName] [varchar](40) NOT NULL,
	[City] [varchar](15) NOT NULL,
	[Country] [varchar](15) NOT NULL,
	[Password] [varchar](30) NOT NULL,
 CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED 
(
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 27/06/2023 11:28:48 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[RequiredDate] [datetime] NULL,
	[ShippedDate] [datetime] NULL,
	[Freight] [money] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 27/06/2023 11:28:48 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[UnitPrice] [money] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Discount] [float] NOT NULL,
 CONSTRAINT [PK_OrderDetail] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 27/06/2023 11:28:48 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [varchar](40) NOT NULL,
	[Weight] [varchar](20) NOT NULL,
	[UnitPrice] [money] NOT NULL,
	[UnitsInStock] [int] NOT NULL,
	[CategoryId] [int] NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([MemberId])
REFERENCES [dbo].[Member] ([MemberId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
USE [master]
GO
ALTER DATABASE [AssSales] SET  READ_WRITE 
GO
