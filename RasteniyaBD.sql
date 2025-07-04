USE [master]
GO
/****** Object:  Database [OrganayzerRasteniy]    Script Date: 10.06.2025 23:45:38 ******/
CREATE DATABASE [OrganayzerRasteniy]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'OrganayzerRasteniy', FILENAME = N'C:\Users\Lenovo\OrganayzerRasteniy.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'OrganayzerRasteniy_log', FILENAME = N'C:\Users\Lenovo\OrganayzerRasteniy_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [OrganayzerRasteniy] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [OrganayzerRasteniy].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [OrganayzerRasteniy] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET ARITHABORT OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [OrganayzerRasteniy] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [OrganayzerRasteniy] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [OrganayzerRasteniy] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET  ENABLE_BROKER 
GO
ALTER DATABASE [OrganayzerRasteniy] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [OrganayzerRasteniy] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [OrganayzerRasteniy] SET  MULTI_USER 
GO
ALTER DATABASE [OrganayzerRasteniy] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [OrganayzerRasteniy] SET DB_CHAINING OFF 
GO
ALTER DATABASE [OrganayzerRasteniy] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [OrganayzerRasteniy] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [OrganayzerRasteniy] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [OrganayzerRasteniy] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [OrganayzerRasteniy] SET QUERY_STORE = OFF
GO
USE [OrganayzerRasteniy]
GO
/****** Object:  Table [dbo].[Admins]    Script Date: 10.06.2025 23:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admins](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[email] [nvarchar](100) NOT NULL,
	[phone] [nvarchar](20) NULL,
	[password] [nvarchar](255) NOT NULL,
	[admin_id] [int] NULL,
	[login] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CareHistory]    Script Date: 10.06.2025 23:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CareHistory](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[plant_id] [int] NULL,
	[action] [nvarchar](100) NULL,
	[date] [date] NULL,
	[comments] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 10.06.2025 23:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[cart_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[plant_id] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[added_date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[cart_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 10.06.2025 23:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Discounts]    Script Date: 10.06.2025 23:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Discounts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[plant_id] [int] NOT NULL,
	[discount_percent] [decimal](5, 2) NOT NULL,
	[start_date] [date] NOT NULL,
	[end_date] [date] NOT NULL,
	[color] [nvarchar](50) NULL,
	[description] [nvarchar](255) NULL,
	[created_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fertilization]    Script Date: 10.06.2025 23:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fertilization](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[plant_id] [int] NULL,
	[fertilizer_id] [int] NULL,
	[fertilization_date] [date] NULL,
	[dosage_ml] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fertilizers]    Script Date: 10.06.2025 23:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fertilizers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
	[composition] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inventory]    Script Date: 10.06.2025 23:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inventory](
	[InventoryID] [int] IDENTITY(1,1) NOT NULL,
	[PlantID] [int] NOT NULL,
	[WarehouseID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[InventoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lighting]    Script Date: 10.06.2025 23:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lighting](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[plant_id] [int] NULL,
	[level] [nvarchar](50) NULL,
	[uses_lights] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 10.06.2025 23:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[order_id] [int] NOT NULL,
	[plant_id] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[price] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[order_id] ASC,
	[plant_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 10.06.2025 23:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[order_date] [datetime] NOT NULL,
	[total_amount] [decimal](10, 2) NOT NULL,
	[status] [nvarchar](50) NOT NULL,
	[shipping_address] [nvarchar](255) NULL,
	[payment_method] [nvarchar](50) NULL,
	[delivery_method] [nvarchar](50) NULL,
	[comment] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 10.06.2025 23:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[PaymentID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[PaymentDate] [date] NULL,
	[Amount] [decimal](10, 2) NULL,
	[PaymentMethod] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Photos]    Script Date: 10.06.2025 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Photos](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[plant_id] [int] NULL,
	[photo_path] [nvarchar](255) NULL,
	[upload_date] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlantLocations]    Script Date: 10.06.2025 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlantLocations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[plant_id] [int] NULL,
	[room_id] [int] NULL,
	[window_position] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Plants]    Script Date: 10.06.2025 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Plants](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[description] [nvarchar](max) NULL,
	[planting_date] [date] NULL,
	[category_id] [int] NULL,
	[user_id] [int] NULL,
	[price] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reminders]    Script Date: 10.06.2025 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reminders](
	[reminder_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[plant_id] [int] NULL,
	[reminder_type] [nvarchar](100) NOT NULL,
	[reminder_time] [time](7) NOT NULL,
	[frequency] [int] NOT NULL,
	[message] [nvarchar](255) NOT NULL,
	[is_active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[reminder_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rooms]    Script Date: 10.06.2025 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rooms](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
	[has_south_windows] [bit] NULL,
	[user_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suppliers]    Script Date: 10.06.2025 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suppliers](
	[SupplierID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[ContactInfo] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Temperature]    Script Date: 10.06.2025 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Temperature](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[plant_id] [int] NULL,
	[min_temp] [int] NULL,
	[max_temp] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserPlants]    Script Date: 10.06.2025 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPlants](
	[user_plant_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[plant_id] [int] NOT NULL,
	[purchase_date] [datetime] NOT NULL,
	[care_schedule] [nvarchar](255) NULL,
	[last_care_date] [datetime] NULL,
	[notes] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[user_plant_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10.06.2025 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[email] [nvarchar](100) NULL,
	[phone] [nvarchar](20) NULL,
	[password] [nvarchar](255) NOT NULL,
	[login] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Warehouses]    Script Date: 10.06.2025 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Warehouses](
	[WarehouseID] [int] IDENTITY(1,1) NOT NULL,
	[Location] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[WarehouseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Watering]    Script Date: 10.06.2025 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Watering](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[plant_id] [int] NULL,
	[watering_date] [date] NOT NULL,
	[amount_ml] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Admins] ON 

INSERT [dbo].[Admins] ([id], [name], [email], [phone], [password], [admin_id], [login]) VALUES (1, N'Админ Иван', N'ivan@admin.ru', N'+79001234567', N'password1', 1, N'admin1')
INSERT [dbo].[Admins] ([id], [name], [email], [phone], [password], [admin_id], [login]) VALUES (2, N'Админ Мария', N'maria@admin.ru', N'+79001234568', N'password2', 2, N'admin2')
INSERT [dbo].[Admins] ([id], [name], [email], [phone], [password], [admin_id], [login]) VALUES (3, N'Админ Петр', N'petr@admin.ru', N'+79001234569', N'password3', 3, N'admin3')
INSERT [dbo].[Admins] ([id], [name], [email], [phone], [password], [admin_id], [login]) VALUES (4, N'Админ Анна', N'anna@admin.ru', N'+79001234570', N'password4', 4, N'admin4')
INSERT [dbo].[Admins] ([id], [name], [email], [phone], [password], [admin_id], [login]) VALUES (5, N'Админ Сергей', N'sergey@admin.ru', N'+79001234571', N'password5', 5, N'admin5')
INSERT [dbo].[Admins] ([id], [name], [email], [phone], [password], [admin_id], [login]) VALUES (6, N'Админ Елена', N'elena@admin.ru', N'+79001234572', N'password6', 6, N'admin6')
INSERT [dbo].[Admins] ([id], [name], [email], [phone], [password], [admin_id], [login]) VALUES (7, N'Админ Дмитрий', N'dmitry@admin.ru', N'+79001234573', N'password7', 7, N'admin7')
INSERT [dbo].[Admins] ([id], [name], [email], [phone], [password], [admin_id], [login]) VALUES (8, N'Админ Ольга', N'olga@admin.ru', N'+79001234574', N'password8', 8, N'admin8')
INSERT [dbo].[Admins] ([id], [name], [email], [phone], [password], [admin_id], [login]) VALUES (9, N'Админ Алексей', N'alexey@admin.ru', N'+79001234575', N'password9', 9, N'admin9')
INSERT [dbo].[Admins] ([id], [name], [email], [phone], [password], [admin_id], [login]) VALUES (10, N'Админ Татьяна', N'tatyana@admin.ru', N'+79001234576', N'password10', 10, N'admin10')
SET IDENTITY_INSERT [dbo].[Admins] OFF
GO
SET IDENTITY_INSERT [dbo].[CareHistory] ON 

INSERT [dbo].[CareHistory] ([id], [plant_id], [action], [date], [comments]) VALUES (1, 1, N'Полив', CAST(N'2023-10-01' AS Date), N'Добавлено 200 мл воды')
INSERT [dbo].[CareHistory] ([id], [plant_id], [action], [date], [comments]) VALUES (2, 2, N'Удобрение', CAST(N'2023-10-02' AS Date), N'Использовано универсальное удобрение')
INSERT [dbo].[CareHistory] ([id], [plant_id], [action], [date], [comments]) VALUES (3, 3, N'Пересадка', CAST(N'2023-09-25' AS Date), N'Пересажен в новый горшок')
INSERT [dbo].[CareHistory] ([id], [plant_id], [action], [date], [comments]) VALUES (4, 4, N'Полив', CAST(N'2023-10-03' AS Date), N'Добавлено 500 мл воды')
INSERT [dbo].[CareHistory] ([id], [plant_id], [action], [date], [comments]) VALUES (5, 5, N'Опрыскивание', CAST(N'2023-10-04' AS Date), N'Листья опрысканы водой')
INSERT [dbo].[CareHistory] ([id], [plant_id], [action], [date], [comments]) VALUES (6, 6, N'Полив', CAST(N'2023-10-05' AS Date), N'Добавлено 700 мл воды')
INSERT [dbo].[CareHistory] ([id], [plant_id], [action], [date], [comments]) VALUES (7, 7, N'Удобрение', CAST(N'2023-10-06' AS Date), N'Использовано удобрение для кактусов')
INSERT [dbo].[CareHistory] ([id], [plant_id], [action], [date], [comments]) VALUES (8, 8, N'Обрезка', CAST(N'2023-09-30' AS Date), N'Срезаны старые листья')
INSERT [dbo].[CareHistory] ([id], [plant_id], [action], [date], [comments]) VALUES (9, 9, N'Полив', CAST(N'2023-10-07' AS Date), N'Добавлено 1000 мл воды')
INSERT [dbo].[CareHistory] ([id], [plant_id], [action], [date], [comments]) VALUES (10, 10, N'Пересадка', CAST(N'2023-09-20' AS Date), N'Пересажен в более просторный горшок')
SET IDENTITY_INSERT [dbo].[CareHistory] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([id], [name], [description]) VALUES (1, N'Суккуленты', N'Растения с толстыми листьями для хранения воды')
INSERT [dbo].[Categories] ([id], [name], [description]) VALUES (2, N'Цветущие', N'Растения с яркими цветами')
INSERT [dbo].[Categories] ([id], [name], [description]) VALUES (3, N'Листопадные', N'Растения, теряющие листья осенью')
INSERT [dbo].[Categories] ([id], [name], [description]) VALUES (4, N'Вечнозеленые', N'Растения, сохраняющие листву круглый год')
INSERT [dbo].[Categories] ([id], [name], [description]) VALUES (5, N'Плодовые', N'Растения, дающие плоды')
INSERT [dbo].[Categories] ([id], [name], [description]) VALUES (6, N'Травянистые', N'Растения без древесного ствола')
INSERT [dbo].[Categories] ([id], [name], [description]) VALUES (7, N'Лианы', N'Вьющиеся растения')
INSERT [dbo].[Categories] ([id], [name], [description]) VALUES (8, N'Кактусы', N'Колючие растения с толстым стеблем')
INSERT [dbo].[Categories] ([id], [name], [description]) VALUES (9, N'Ароматические', N'Растения с приятным запахом')
INSERT [dbo].[Categories] ([id], [name], [description]) VALUES (10, N'Водные', N'Растения, растущие в воде')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Discounts] ON 

INSERT [dbo].[Discounts] ([id], [plant_id], [discount_percent], [start_date], [end_date], [color], [description], [created_at]) VALUES (1, 1, CAST(15.00 AS Decimal(5, 2)), CAST(N'2025-04-01' AS Date), CAST(N'2025-04-30' AS Date), N'#FFA500', N'Весенняя распродажа: 15% на все цветущие растения', CAST(N'2025-05-26T18:34:23.977' AS DateTime))
INSERT [dbo].[Discounts] ([id], [plant_id], [discount_percent], [start_date], [end_date], [color], [description], [created_at]) VALUES (2, 2, CAST(10.00 AS Decimal(5, 2)), CAST(N'2025-04-01' AS Date), CAST(N'2025-04-30' AS Date), N'#90EE90', N'Акция: 10% на кактусы в апреле', CAST(N'2025-05-26T18:34:23.977' AS DateTime))
INSERT [dbo].[Discounts] ([id], [plant_id], [discount_percent], [start_date], [end_date], [color], [description], [created_at]) VALUES (3, 3, CAST(20.00 AS Decimal(5, 2)), CAST(N'2025-04-01' AS Date), CAST(N'2025-04-30' AS Date), N'#87CEEB', N'Скидка для новых клиентов — 20% на герань', CAST(N'2025-05-26T18:34:23.977' AS DateTime))
INSERT [dbo].[Discounts] ([id], [plant_id], [discount_percent], [start_date], [end_date], [color], [description], [created_at]) VALUES (4, 4, CAST(25.00 AS Decimal(5, 2)), CAST(N'2025-04-01' AS Date), CAST(N'2025-04-15' AS Date), N'#FF6347', N'Распродажа к 8 марта — 25% на фиалки', CAST(N'2025-05-26T18:34:23.977' AS DateTime))
INSERT [dbo].[Discounts] ([id], [plant_id], [discount_percent], [start_date], [end_date], [color], [description], [created_at]) VALUES (5, 5, CAST(30.00 AS Decimal(5, 2)), CAST(N'2025-04-05' AS Date), CAST(N'2025-04-20' AS Date), N'#BA55D3', N'Специальное предложение: 30% на орхидеи', CAST(N'2025-05-26T18:34:23.977' AS DateTime))
INSERT [dbo].[Discounts] ([id], [plant_id], [discount_percent], [start_date], [end_date], [color], [description], [created_at]) VALUES (6, 6, CAST(10.00 AS Decimal(5, 2)), CAST(N'2025-04-10' AS Date), CAST(N'2025-05-10' AS Date), N'#3CB371', N'Акция "Зелёный май" — 10% на уличные растения', CAST(N'2025-05-26T18:34:23.977' AS DateTime))
INSERT [dbo].[Discounts] ([id], [plant_id], [discount_percent], [start_date], [end_date], [color], [description], [created_at]) VALUES (7, 7, CAST(15.00 AS Decimal(5, 2)), CAST(N'2025-04-15' AS Date), CAST(N'2025-05-01' AS Date), N'#FFD700', N'Праздничная скидка — 15% на декоративные травы', CAST(N'2025-05-26T18:34:23.977' AS DateTime))
INSERT [dbo].[Discounts] ([id], [plant_id], [discount_percent], [start_date], [end_date], [color], [description], [created_at]) VALUES (8, 8, CAST(5.00 AS Decimal(5, 2)), CAST(N'2025-04-01' AS Date), CAST(N'2025-06-30' AS Date), N'#8B4513', N'Сезонная скидка — 5% на комнатные пальмы', CAST(N'2025-05-26T18:34:23.977' AS DateTime))
INSERT [dbo].[Discounts] ([id], [plant_id], [discount_percent], [start_date], [end_date], [color], [description], [created_at]) VALUES (9, 9, CAST(12.50 AS Decimal(5, 2)), CAST(N'2025-04-01' AS Date), CAST(N'2025-04-30' AS Date), N'#FF69B4', N'Подарочная акция — 12.5% на суккуленты', CAST(N'2025-05-26T18:34:23.977' AS DateTime))
INSERT [dbo].[Discounts] ([id], [plant_id], [discount_percent], [start_date], [end_date], [color], [description], [created_at]) VALUES (10, 10, CAST(18.00 AS Decimal(5, 2)), CAST(N'2025-04-05' AS Date), CAST(N'2025-04-25' AS Date), N'#4682B4', N'Лучшая цена апреля — 18% на хвойные растения', CAST(N'2025-05-26T18:34:23.977' AS DateTime))
INSERT [dbo].[Discounts] ([id], [plant_id], [discount_percent], [start_date], [end_date], [color], [description], [created_at]) VALUES (11, 14, CAST(20.00 AS Decimal(5, 2)), CAST(N'2025-05-25' AS Date), CAST(N'2025-06-30' AS Date), N'#0000', N'', NULL)
SET IDENTITY_INSERT [dbo].[Discounts] OFF
GO
SET IDENTITY_INSERT [dbo].[Fertilization] ON 

INSERT [dbo].[Fertilization] ([id], [plant_id], [fertilizer_id], [fertilization_date], [dosage_ml]) VALUES (1, 1, 1, CAST(N'2023-10-01' AS Date), 50)
INSERT [dbo].[Fertilization] ([id], [plant_id], [fertilizer_id], [fertilization_date], [dosage_ml]) VALUES (2, 2, 2, CAST(N'2023-10-02' AS Date), 60)
INSERT [dbo].[Fertilization] ([id], [plant_id], [fertilizer_id], [fertilization_date], [dosage_ml]) VALUES (3, 3, 3, CAST(N'2023-10-03' AS Date), 70)
INSERT [dbo].[Fertilization] ([id], [plant_id], [fertilizer_id], [fertilization_date], [dosage_ml]) VALUES (4, 4, 4, CAST(N'2023-10-04' AS Date), 80)
INSERT [dbo].[Fertilization] ([id], [plant_id], [fertilizer_id], [fertilization_date], [dosage_ml]) VALUES (5, 5, 5, CAST(N'2023-10-05' AS Date), 90)
INSERT [dbo].[Fertilization] ([id], [plant_id], [fertilizer_id], [fertilization_date], [dosage_ml]) VALUES (6, 6, 6, CAST(N'2023-10-06' AS Date), 100)
INSERT [dbo].[Fertilization] ([id], [plant_id], [fertilizer_id], [fertilization_date], [dosage_ml]) VALUES (7, 7, 7, CAST(N'2023-10-07' AS Date), 110)
INSERT [dbo].[Fertilization] ([id], [plant_id], [fertilizer_id], [fertilization_date], [dosage_ml]) VALUES (8, 8, 8, CAST(N'2023-10-08' AS Date), 120)
INSERT [dbo].[Fertilization] ([id], [plant_id], [fertilizer_id], [fertilization_date], [dosage_ml]) VALUES (9, 9, 9, CAST(N'2023-10-09' AS Date), 130)
INSERT [dbo].[Fertilization] ([id], [plant_id], [fertilizer_id], [fertilization_date], [dosage_ml]) VALUES (10, 10, 10, CAST(N'2023-10-10' AS Date), 140)
SET IDENTITY_INSERT [dbo].[Fertilization] OFF
GO
SET IDENTITY_INSERT [dbo].[Fertilizers] ON 

INSERT [dbo].[Fertilizers] ([id], [name], [composition]) VALUES (1, N'Универсальное', N'Азот, фосфор, калий')
INSERT [dbo].[Fertilizers] ([id], [name], [composition]) VALUES (2, N'Для цветущих', N'Фосфор, калий')
INSERT [dbo].[Fertilizers] ([id], [name], [composition]) VALUES (3, N'Для суккулентов', N'Калий, магний')
INSERT [dbo].[Fertilizers] ([id], [name], [composition]) VALUES (4, N'Органическое', N'Перегной, торф')
INSERT [dbo].[Fertilizers] ([id], [name], [composition]) VALUES (5, N'Минеральное', N'Нитраты, сульфаты')
INSERT [dbo].[Fertilizers] ([id], [name], [composition]) VALUES (6, N'Для плодовых', N'Фосфор, азот')
INSERT [dbo].[Fertilizers] ([id], [name], [composition]) VALUES (7, N'Жидкое', N'Концентрат микроэлементов')
INSERT [dbo].[Fertilizers] ([id], [name], [composition]) VALUES (8, N'Гранулированное', N'Гранулы минералов')
INSERT [dbo].[Fertilizers] ([id], [name], [composition]) VALUES (9, N'Для кактусов', N'Калий, магний, железо')
INSERT [dbo].[Fertilizers] ([id], [name], [composition]) VALUES (10, N'Биоудобрение', N'Бактерии, гумус')
SET IDENTITY_INSERT [dbo].[Fertilizers] OFF
GO
SET IDENTITY_INSERT [dbo].[Inventory] ON 

INSERT [dbo].[Inventory] ([InventoryID], [PlantID], [WarehouseID], [Quantity], [LastUpdated]) VALUES (1, 1, 1, 55, CAST(N'2025-05-28T23:29:00.877' AS DateTime))
INSERT [dbo].[Inventory] ([InventoryID], [PlantID], [WarehouseID], [Quantity], [LastUpdated]) VALUES (2, 2, 1, 29, CAST(N'2025-05-28T23:29:00.877' AS DateTime))
INSERT [dbo].[Inventory] ([InventoryID], [PlantID], [WarehouseID], [Quantity], [LastUpdated]) VALUES (3, 3, 1, 95, CAST(N'2025-05-28T23:29:00.877' AS DateTime))
INSERT [dbo].[Inventory] ([InventoryID], [PlantID], [WarehouseID], [Quantity], [LastUpdated]) VALUES (4, 4, 1, 64, CAST(N'2025-05-28T23:29:00.877' AS DateTime))
INSERT [dbo].[Inventory] ([InventoryID], [PlantID], [WarehouseID], [Quantity], [LastUpdated]) VALUES (5, 5, 1, 56, CAST(N'2025-05-28T23:29:00.877' AS DateTime))
SET IDENTITY_INSERT [dbo].[Inventory] OFF
GO
SET IDENTITY_INSERT [dbo].[Lighting] ON 

INSERT [dbo].[Lighting] ([id], [plant_id], [level], [uses_lights]) VALUES (1, 1, N'Высокий', 0)
INSERT [dbo].[Lighting] ([id], [plant_id], [level], [uses_lights]) VALUES (2, 2, N'Средний', 1)
INSERT [dbo].[Lighting] ([id], [plant_id], [level], [uses_lights]) VALUES (3, 3, N'Средний', 0)
INSERT [dbo].[Lighting] ([id], [plant_id], [level], [uses_lights]) VALUES (4, 4, N'Высокий', 0)
INSERT [dbo].[Lighting] ([id], [plant_id], [level], [uses_lights]) VALUES (5, 5, N'Низкий', 1)
INSERT [dbo].[Lighting] ([id], [plant_id], [level], [uses_lights]) VALUES (6, 6, N'Высокий', 0)
INSERT [dbo].[Lighting] ([id], [plant_id], [level], [uses_lights]) VALUES (7, 7, N'Низкий', 0)
INSERT [dbo].[Lighting] ([id], [plant_id], [level], [uses_lights]) VALUES (8, 8, N'Средний', 0)
INSERT [dbo].[Lighting] ([id], [plant_id], [level], [uses_lights]) VALUES (9, 9, N'Высокий', 1)
INSERT [dbo].[Lighting] ([id], [plant_id], [level], [uses_lights]) VALUES (10, 10, N'Низкий', 0)
SET IDENTITY_INSERT [dbo].[Lighting] OFF
GO
INSERT [dbo].[OrderDetails] ([order_id], [plant_id], [quantity], [price]) VALUES (1, 1, 2, CAST(300.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([order_id], [plant_id], [quantity], [price]) VALUES (1, 3, 1, CAST(900.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([order_id], [plant_id], [quantity], [price]) VALUES (2, 2, 3, CAST(400.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([order_id], [plant_id], [quantity], [price]) VALUES (2, 4, 1, CAST(1100.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([order_id], [plant_id], [quantity], [price]) VALUES (3, 5, 1, CAST(900.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([order_id], [plant_id], [quantity], [price]) VALUES (4, 6, 2, CAST(600.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([order_id], [plant_id], [quantity], [price]) VALUES (5, 7, 4, CAST(200.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([order_id], [plant_id], [quantity], [price]) VALUES (6, 8, 2, CAST(1000.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([order_id], [plant_id], [quantity], [price]) VALUES (6, 9, 1, CAST(1000.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([order_id], [plant_id], [quantity], [price]) VALUES (7, 10, 1, CAST(450.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([order_id], [plant_id], [quantity], [price]) VALUES (8, 1, 1, CAST(300.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([order_id], [plant_id], [quantity], [price]) VALUES (8, 2, 1, CAST(400.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([order_id], [plant_id], [quantity], [price]) VALUES (8, 5, 1, CAST(900.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([order_id], [plant_id], [quantity], [price]) VALUES (9, 3, 1, CAST(900.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([order_id], [plant_id], [quantity], [price]) VALUES (10, 4, 1, CAST(1100.00 AS Decimal(10, 2)))
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([id], [user_id], [order_date], [total_amount], [status], [shipping_address], [payment_method], [delivery_method], [comment]) VALUES (1, 1, CAST(N'2025-04-01T10:30:00.000' AS DateTime), CAST(1500.00 AS Decimal(10, 2)), N'Ожидает оплаты', N'ул. Ленина, д. 5, кв. 10', N'Картой при получении', N'Курьером', N'Прошу позвонить заранее')
INSERT [dbo].[Orders] ([id], [user_id], [order_date], [total_amount], [status], [shipping_address], [payment_method], [delivery_method], [comment]) VALUES (2, 2, CAST(N'2025-04-02T11:15:00.000' AS DateTime), CAST(2300.00 AS Decimal(10, 2)), N'Оплачен', N'ул. Гагарина, д. 12, кв. 45', N'Онлайн', N'Самовывоз', N'Без комментариев')
INSERT [dbo].[Orders] ([id], [user_id], [order_date], [total_amount], [status], [shipping_address], [payment_method], [delivery_method], [comment]) VALUES (3, 3, CAST(N'2025-04-03T12:45:00.000' AS DateTime), CAST(900.00 AS Decimal(10, 2)), N'Доставляется', N'ул. Пушкина, д. 30', N'Наличными', N'Курьером', N'Оставить у двери')
INSERT [dbo].[Orders] ([id], [user_id], [order_date], [total_amount], [status], [shipping_address], [payment_method], [delivery_method], [comment]) VALUES (4, 4, CAST(N'2025-04-04T14:20:00.000' AS DateTime), CAST(1200.00 AS Decimal(10, 2)), N'Завершён', N'ул. Чехова, д. 8, кв. 101', N'Онлайн', N'Курьером', N'Без комментариев')
INSERT [dbo].[Orders] ([id], [user_id], [order_date], [total_amount], [status], [shipping_address], [payment_method], [delivery_method], [comment]) VALUES (5, 5, CAST(N'2025-04-05T09:00:00.000' AS DateTime), CAST(800.00 AS Decimal(10, 2)), N'Ожидает оплаты', N'ул. Садовая, д. 15, кв. 30', N'Наличными', N'Самовывоз', N'Привезите после 18:00')
INSERT [dbo].[Orders] ([id], [user_id], [order_date], [total_amount], [status], [shipping_address], [payment_method], [delivery_method], [comment]) VALUES (6, 6, CAST(N'2025-04-06T16:00:00.000' AS DateTime), CAST(3000.00 AS Decimal(10, 2)), N'Оплачен', N'ул. Некрасова, д. 22', N'Картой онлайн', N'Курьером', N'Без комментариев')
INSERT [dbo].[Orders] ([id], [user_id], [order_date], [total_amount], [status], [shipping_address], [payment_method], [delivery_method], [comment]) VALUES (7, 7, CAST(N'2025-04-07T10:00:00.000' AS DateTime), CAST(450.00 AS Decimal(10, 2)), N'Завершён', N'ул. Кирова, д. 1', N'Наличными', N'Самовывоз', N'Заберу сам')
INSERT [dbo].[Orders] ([id], [user_id], [order_date], [total_amount], [status], [shipping_address], [payment_method], [delivery_method], [comment]) VALUES (8, 8, CAST(N'2025-04-08T12:30:00.000' AS DateTime), CAST(2000.00 AS Decimal(10, 2)), N'Ожидает оплаты', N'ул. Мира, д. 100', N'Картой при получении', N'Курьером', N'Без комментариев')
INSERT [dbo].[Orders] ([id], [user_id], [order_date], [total_amount], [status], [shipping_address], [payment_method], [delivery_method], [comment]) VALUES (9, 9, CAST(N'2025-04-09T15:45:00.000' AS DateTime), CAST(700.00 AS Decimal(10, 2)), N'Оплачен', N'ул. Лермонтова, д. 5', N'Онлайн', N'Самовывоз', N'Нужен подарочный пакет')
INSERT [dbo].[Orders] ([id], [user_id], [order_date], [total_amount], [status], [shipping_address], [payment_method], [delivery_method], [comment]) VALUES (10, 10, CAST(N'2025-04-10T17:00:00.000' AS DateTime), CAST(1300.00 AS Decimal(10, 2)), N'Доставляется', N'ул. Радищева, д. 3', N'Наличными', N'Курьером', N'Не звонить до 9:00')
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Photos] ON 

INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (3, 3, N'photos/ficus.jpg', CAST(N'2023-10-03' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (4, 4, N'photos/lemon.jpg', CAST(N'2023-10-04' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (5, 5, N'photos/monstera.jpg', CAST(N'2023-10-05' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (6, 6, N'photos/orchid.jpg', CAST(N'2023-10-06' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (7, 7, N'photos/cactus.jpg', CAST(N'2023-10-07' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (8, 8, N'photos/basil.jpg', CAST(N'2023-10-08' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (9, 9, N'photos/lotus.jpg', CAST(N'2023-10-09' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (10, 10, N'photos/fern.jpg', CAST(N'2023-10-10' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (12, NULL, N'D:\Загрузки\касета без фона.png', CAST(N'2025-05-22' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (13, 13, N'D:\Загрузки\касета без фона.png', CAST(N'2025-05-23' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (14, 14, N'Images/Снимок экрана 2025-05-13 173227.png', CAST(N'2025-05-25' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (15, NULL, N'D:\Загрузки\tumblr_472b3f948e63f59a373418987a12d79f_3e6db1cb_2048.jpg', CAST(N'2025-05-25' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (16, NULL, N'D:\Загрузки\Telegram Desktop\photo_2023-10-01_15-06-28.jpg', CAST(N'2025-05-27' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (17, 17, N'D:\Загрузки\Telegram Desktop\photo_2024-12-16_12-31-18.jpg', CAST(N'2025-05-28' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (19, 10, N'/Images/b519f11d-118d-4335-8df7-5f715f273060.jpg', CAST(N'2025-05-28' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (21, 17, N'/Images/photo_2023-10-01_15-06-28.jpg', CAST(N'2025-05-28' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (25, NULL, N'/Images/Обложка.png', CAST(N'2025-05-28' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (28, 2, N'/Images/galina-n-miziNqvJx5M-unsplash.jpg', CAST(N'2025-05-30' AS Date))
INSERT [dbo].[Photos] ([id], [plant_id], [photo_path], [upload_date]) VALUES (29, 1, N'/Images/no-image.jpg', CAST(N'2025-05-30' AS Date))
SET IDENTITY_INSERT [dbo].[Photos] OFF
GO
SET IDENTITY_INSERT [dbo].[PlantLocations] ON 

INSERT [dbo].[PlantLocations] ([id], [plant_id], [room_id], [window_position]) VALUES (1, 1, 1, N'На подоконнике')
INSERT [dbo].[PlantLocations] ([id], [plant_id], [room_id], [window_position]) VALUES (2, 2, 2, N'У окна')
INSERT [dbo].[PlantLocations] ([id], [plant_id], [room_id], [window_position]) VALUES (3, 3, 3, N'На столе')
INSERT [dbo].[PlantLocations] ([id], [plant_id], [room_id], [window_position]) VALUES (4, 4, 4, N'На полке')
INSERT [dbo].[PlantLocations] ([id], [plant_id], [room_id], [window_position]) VALUES (5, 5, 5, N'На полу')
INSERT [dbo].[PlantLocations] ([id], [plant_id], [room_id], [window_position]) VALUES (6, 6, 6, N'На балконе')
INSERT [dbo].[PlantLocations] ([id], [plant_id], [room_id], [window_position]) VALUES (7, 7, 7, N'На тумбе')
INSERT [dbo].[PlantLocations] ([id], [plant_id], [room_id], [window_position]) VALUES (8, 8, 8, N'У входа')
INSERT [dbo].[PlantLocations] ([id], [plant_id], [room_id], [window_position]) VALUES (9, 9, 9, N'На комоде')
INSERT [dbo].[PlantLocations] ([id], [plant_id], [room_id], [window_position]) VALUES (10, 10, 10, N'В углу')
SET IDENTITY_INSERT [dbo].[PlantLocations] OFF
GO
SET IDENTITY_INSERT [dbo].[Plants] ON 

INSERT [dbo].[Plants] ([id], [name], [description], [planting_date], [category_id], [user_id], [price]) VALUES (1, N'Алоэ', N'Лекарственное растение', CAST(N'2023-03-15' AS Date), 1, 1, CAST(500.00 AS Decimal(10, 2)))
INSERT [dbo].[Plants] ([id], [name], [description], [planting_date], [category_id], [user_id], [price]) VALUES (2, N'Роза', N'Красиво цветущее растение', CAST(N'2023-04-20' AS Date), 2, 2, CAST(10.00 AS Decimal(10, 2)))
INSERT [dbo].[Plants] ([id], [name], [description], [planting_date], [category_id], [user_id], [price]) VALUES (3, N'Фикус', N'Вечнозеленое декоративное дерево', CAST(N'2022-06-10' AS Date), 4, 3, CAST(1200.00 AS Decimal(10, 2)))
INSERT [dbo].[Plants] ([id], [name], [description], [planting_date], [category_id], [user_id], [price]) VALUES (4, N'Лимон', N'Плодовое дерево', CAST(N'2023-05-01' AS Date), 5, 4, CAST(1500.00 AS Decimal(10, 2)))
INSERT [dbo].[Plants] ([id], [name], [description], [planting_date], [category_id], [user_id], [price]) VALUES (5, N'Монстера', N'Декоративная лиана', CAST(N'2021-08-12' AS Date), 7, 5, CAST(700.00 AS Decimal(10, 2)))
INSERT [dbo].[Plants] ([id], [name], [description], [planting_date], [category_id], [user_id], [price]) VALUES (6, N'Орхидея', N'Элегантное цветущее растение', CAST(N'2023-01-25' AS Date), 2, 6, CAST(900.00 AS Decimal(10, 2)))
INSERT [dbo].[Plants] ([id], [name], [description], [planting_date], [category_id], [user_id], [price]) VALUES (7, N'Кактус', N'Колючий суккулент', CAST(N'2022-11-05' AS Date), 8, 7, CAST(300.00 AS Decimal(10, 2)))
INSERT [dbo].[Plants] ([id], [name], [description], [planting_date], [category_id], [user_id], [price]) VALUES (8, N'Базилик', N'Ароматическая трава', CAST(N'2023-07-10' AS Date), 9, 8, CAST(200.00 AS Decimal(10, 2)))
INSERT [dbo].[Plants] ([id], [name], [description], [planting_date], [category_id], [user_id], [price]) VALUES (9, N'Лотос', N'Водное растение', CAST(N'2023-02-14' AS Date), 10, 9, CAST(1000.00 AS Decimal(10, 2)))
INSERT [dbo].[Plants] ([id], [name], [description], [planting_date], [category_id], [user_id], [price]) VALUES (10, N'Папоротник', N'Травянистое растение', CAST(N'2023-03-22' AS Date), 6, 10, CAST(600.00 AS Decimal(10, 2)))
INSERT [dbo].[Plants] ([id], [name], [description], [planting_date], [category_id], [user_id], [price]) VALUES (13, N'аа', N'аа', CAST(N'2025-05-20' AS Date), 5, 1, CAST(15000.00 AS Decimal(10, 2)))
INSERT [dbo].[Plants] ([id], [name], [description], [planting_date], [category_id], [user_id], [price]) VALUES (14, N'фикус', N'1111', CAST(N'2025-05-12' AS Date), 3, 1, CAST(13.00 AS Decimal(10, 2)))
INSERT [dbo].[Plants] ([id], [name], [description], [planting_date], [category_id], [user_id], [price]) VALUES (17, N'aa', N'1234', CAST(N'2025-05-07' AS Date), 8, NULL, CAST(444.00 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[Plants] OFF
GO
SET IDENTITY_INSERT [dbo].[Rooms] ON 

INSERT [dbo].[Rooms] ([id], [name], [has_south_windows], [user_id]) VALUES (1, N'Гостиная', 1, 1)
INSERT [dbo].[Rooms] ([id], [name], [has_south_windows], [user_id]) VALUES (2, N'Кухня', 0, 2)
INSERT [dbo].[Rooms] ([id], [name], [has_south_windows], [user_id]) VALUES (3, N'Спальня', 1, 3)
INSERT [dbo].[Rooms] ([id], [name], [has_south_windows], [user_id]) VALUES (4, N'Кабинет', 0, 4)
INSERT [dbo].[Rooms] ([id], [name], [has_south_windows], [user_id]) VALUES (5, N'Коридор', 0, 5)
INSERT [dbo].[Rooms] ([id], [name], [has_south_windows], [user_id]) VALUES (6, N'Балкон', 1, 6)
INSERT [dbo].[Rooms] ([id], [name], [has_south_windows], [user_id]) VALUES (7, N'Детская', 0, 7)
INSERT [dbo].[Rooms] ([id], [name], [has_south_windows], [user_id]) VALUES (8, N'Ванная', 0, 8)
INSERT [dbo].[Rooms] ([id], [name], [has_south_windows], [user_id]) VALUES (9, N'Столовая', 1, 9)
INSERT [dbo].[Rooms] ([id], [name], [has_south_windows], [user_id]) VALUES (10, N'Гардероб', 0, 10)
SET IDENTITY_INSERT [dbo].[Rooms] OFF
GO
SET IDENTITY_INSERT [dbo].[Temperature] ON 

INSERT [dbo].[Temperature] ([id], [plant_id], [min_temp], [max_temp]) VALUES (1, 1, 15, 25)
INSERT [dbo].[Temperature] ([id], [plant_id], [min_temp], [max_temp]) VALUES (2, 2, 18, 28)
INSERT [dbo].[Temperature] ([id], [plant_id], [min_temp], [max_temp]) VALUES (3, 3, 10, 30)
INSERT [dbo].[Temperature] ([id], [plant_id], [min_temp], [max_temp]) VALUES (4, 4, 15, 25)
INSERT [dbo].[Temperature] ([id], [plant_id], [min_temp], [max_temp]) VALUES (5, 5, 20, 30)
INSERT [dbo].[Temperature] ([id], [plant_id], [min_temp], [max_temp]) VALUES (6, 6, 18, 28)
INSERT [dbo].[Temperature] ([id], [plant_id], [min_temp], [max_temp]) VALUES (7, 7, 10, 35)
INSERT [dbo].[Temperature] ([id], [plant_id], [min_temp], [max_temp]) VALUES (8, 8, 15, 25)
INSERT [dbo].[Temperature] ([id], [plant_id], [min_temp], [max_temp]) VALUES (9, 9, 20, 30)
INSERT [dbo].[Temperature] ([id], [plant_id], [min_temp], [max_temp]) VALUES (10, 10, 15, 25)
SET IDENTITY_INSERT [dbo].[Temperature] OFF
GO
SET IDENTITY_INSERT [dbo].[UserPlants] ON 

INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (2, 1, 2, CAST(N'2024-12-24T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-06T22:43:39.943' AS DateTime), N'Листья желтеют')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (3, 1, 3, CAST(N'2024-09-06T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-12T22:43:39.943' AS DateTime), NULL)
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (4, 1, 4, CAST(N'2025-04-06T22:43:39.943' AS DateTime), N'Полив: 1 раз в день', CAST(N'2025-05-09T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (5, 1, 5, CAST(N'2025-05-22T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-07T22:43:39.943' AS DateTime), NULL)
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (6, 1, 6, CAST(N'2024-07-09T22:43:39.943' AS DateTime), N'Освещение: яркое', CAST(N'2025-05-21T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (7, 1, 7, CAST(N'2025-04-22T22:43:39.943' AS DateTime), N'Температура: +20°C', CAST(N'2025-05-23T22:43:39.943' AS DateTime), N'Листья желтеют')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (8, 1, 8, CAST(N'2024-06-07T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-28T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (9, 1, 9, CAST(N'2025-02-03T22:43:39.943' AS DateTime), N'Полив: 1 раз в день', CAST(N'2025-05-12T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (10, 1, 10, CAST(N'2024-08-30T22:43:39.943' AS DateTime), N'Освещение: яркое', CAST(N'2025-05-25T22:43:39.943' AS DateTime), N'Нуждается в поливе')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (11, 2, 1, CAST(N'2024-11-26T22:43:39.943' AS DateTime), N'Температура: +20°C', CAST(N'2025-05-04T22:43:39.943' AS DateTime), NULL)
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (12, 2, 2, CAST(N'2025-05-28T22:43:39.943' AS DateTime), N'Полив: 1 раз в день', CAST(N'2025-05-14T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (13, 2, 3, CAST(N'2024-12-14T22:43:39.943' AS DateTime), N'Подкормка: 1 раз в неделю', CAST(N'2025-05-05T22:43:39.943' AS DateTime), N'Нуждается в поливе')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (14, 2, 4, CAST(N'2024-09-17T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-03T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (15, 2, 5, CAST(N'2025-05-14T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-23T22:43:39.943' AS DateTime), NULL)
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (16, 2, 6, CAST(N'2025-03-17T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-26T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (17, 2, 7, CAST(N'2025-05-02T22:43:39.943' AS DateTime), N'Освещение: яркое', CAST(N'2025-05-16T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (18, 2, 8, CAST(N'2025-01-10T22:43:39.943' AS DateTime), N'Температура: +20°C', CAST(N'2025-05-31T22:43:39.943' AS DateTime), N'Нуждается в поливе')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (19, 2, 9, CAST(N'2024-12-10T22:43:39.943' AS DateTime), N'Полив: 1 раз в день', CAST(N'2025-05-15T22:43:39.943' AS DateTime), N'Нуждается в поливе')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (20, 2, 10, CAST(N'2025-01-21T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-05T22:43:39.943' AS DateTime), N'Листья желтеют')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (21, 3, 1, CAST(N'2025-05-22T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-18T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (22, 3, 2, CAST(N'2025-02-05T22:43:39.943' AS DateTime), N'Полив: 1 раз в день', CAST(N'2025-05-04T22:43:39.943' AS DateTime), NULL)
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (23, 3, 3, CAST(N'2024-12-22T22:43:39.943' AS DateTime), N'Температура: +20°C', CAST(N'2025-05-20T22:43:39.943' AS DateTime), N'Листья желтеют')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (24, 3, 4, CAST(N'2024-09-09T22:43:39.943' AS DateTime), N'Температура: +20°C', CAST(N'2025-05-20T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (25, 3, 5, CAST(N'2025-04-17T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-07T22:43:39.943' AS DateTime), N'Нуждается в поливе')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (26, 3, 6, CAST(N'2025-05-19T22:43:39.943' AS DateTime), N'Температура: +20°C', CAST(N'2025-05-19T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (27, 3, 7, CAST(N'2025-03-20T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-18T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (28, 3, 8, CAST(N'2024-11-05T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-07T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (29, 3, 9, CAST(N'2024-06-01T22:43:39.943' AS DateTime), N'Освещение: яркое', CAST(N'2025-05-12T22:43:39.943' AS DateTime), N'Нуждается в поливе')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (30, 3, 10, CAST(N'2024-08-21T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-16T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (31, 4, 1, CAST(N'2024-10-04T22:43:39.943' AS DateTime), N'Полив: 1 раз в день', CAST(N'2025-05-11T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (32, 4, 2, CAST(N'2024-08-04T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-25T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (33, 4, 3, CAST(N'2024-08-20T22:43:39.943' AS DateTime), N'Подкормка: 1 раз в неделю', CAST(N'2025-05-28T22:43:39.943' AS DateTime), N'Нуждается в поливе')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (34, 4, 4, CAST(N'2025-03-16T22:43:39.943' AS DateTime), N'Полив: 1 раз в день', CAST(N'2025-05-05T22:43:39.943' AS DateTime), N'Нуждается в поливе')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (35, 4, 5, CAST(N'2024-09-27T22:43:39.943' AS DateTime), N'Освещение: яркое', CAST(N'2025-05-11T22:43:39.943' AS DateTime), NULL)
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (36, 4, 6, CAST(N'2024-09-21T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-22T22:43:39.943' AS DateTime), N'Листья желтеют')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (37, 4, 7, CAST(N'2024-08-06T22:43:39.943' AS DateTime), N'Полив: 1 раз в день', CAST(N'2025-05-12T22:43:39.943' AS DateTime), N'Нуждается в поливе')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (38, 4, 8, CAST(N'2024-12-26T22:43:39.943' AS DateTime), N'Полив: 1 раз в день', CAST(N'2025-05-19T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (39, 4, 9, CAST(N'2024-11-18T22:43:39.943' AS DateTime), N'Температура: +20°C', CAST(N'2025-05-25T22:43:39.943' AS DateTime), NULL)
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (40, 4, 10, CAST(N'2024-10-05T22:43:39.943' AS DateTime), N'Подкормка: 1 раз в неделю', CAST(N'2025-05-09T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (41, 5, 1, CAST(N'2025-03-26T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-11T22:43:39.943' AS DateTime), NULL)
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (42, 5, 2, CAST(N'2025-03-18T22:43:39.943' AS DateTime), N'Температура: +20°C', CAST(N'2025-05-25T22:43:39.943' AS DateTime), N'Нуждается в поливе')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (43, 5, 3, CAST(N'2024-11-11T22:43:39.943' AS DateTime), N'Полив: 1 раз в день', CAST(N'2025-05-27T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (44, 5, 4, CAST(N'2024-08-14T22:43:39.943' AS DateTime), N'Полив: 1 раз в день', CAST(N'2025-05-25T22:43:39.943' AS DateTime), N'Листья желтеют')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (45, 5, 5, CAST(N'2024-10-04T22:43:39.943' AS DateTime), N'Подкормка: 1 раз в неделю', CAST(N'2025-05-07T22:43:39.943' AS DateTime), N'Нуждается в поливе')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (46, 5, 6, CAST(N'2024-06-07T22:43:39.943' AS DateTime), N'Подкормка: 1 раз в неделю', CAST(N'2025-05-29T22:43:39.943' AS DateTime), N'Листья желтеют')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (47, 5, 7, CAST(N'2024-11-24T22:43:39.943' AS DateTime), NULL, CAST(N'2025-05-29T22:43:39.943' AS DateTime), N'Нуждается в поливе')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (48, 5, 8, CAST(N'2025-05-22T22:43:39.943' AS DateTime), N'Температура: +20°C', CAST(N'2025-05-30T22:43:39.943' AS DateTime), N'Хорошее состояние')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (49, 5, 9, CAST(N'2025-04-27T22:43:39.943' AS DateTime), N'Температура: +20°C', CAST(N'2025-05-17T22:43:39.943' AS DateTime), N'Нуждается в поливе')
INSERT [dbo].[UserPlants] ([user_plant_id], [user_id], [plant_id], [purchase_date], [care_schedule], [last_care_date], [notes]) VALUES (50, 5, 10, CAST(N'2024-06-14T22:43:39.943' AS DateTime), N'Освещение: яркое', CAST(N'2025-05-31T22:43:39.943' AS DateTime), N'Листья желтеют')
SET IDENTITY_INSERT [dbo].[UserPlants] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([id], [name], [email], [phone], [password], [login]) VALUES (1, N'Иван Иванов', N'ivan@example.com', N'+79123456789', N'qwerty123', N'ivan_iv')
INSERT [dbo].[Users] ([id], [name], [email], [phone], [password], [login]) VALUES (2, N'Мария Петрова', N'maria@example.com', N'+79234567890', N'password1', N'maria01')
INSERT [dbo].[Users] ([id], [name], [email], [phone], [password], [login]) VALUES (3, N'Петр Сидоров', N'petr@example.com', N'+79345678901', N'abc123', N'petyaogon')
INSERT [dbo].[Users] ([id], [name], [email], [phone], [password], [login]) VALUES (4, N'Анна Кузнецова', N'anna@example.com', N'+79456789012', N'anna123', N'anna_kz')
INSERT [dbo].[Users] ([id], [name], [email], [phone], [password], [login]) VALUES (5, N'Сергей Смирнов', N'sergey@example.com', N'+79567890123', N'sergpass', N'sergey_sm')
INSERT [dbo].[Users] ([id], [name], [email], [phone], [password], [login]) VALUES (6, N'Елена Васильева', N'elena@example.com', N'+79678901234', N'elenapass', N'elena_vs')
INSERT [dbo].[Users] ([id], [name], [email], [phone], [password], [login]) VALUES (7, N'Дмитрий Николаев', N'dmitry@example.com', N'+79789012345', N'dmitry123', N'dmitry_nk')
INSERT [dbo].[Users] ([id], [name], [email], [phone], [password], [login]) VALUES (8, N'Ольга Медведева', N'olga@example.com', N'+79890123456', N'olgapass', N'olga_md')
INSERT [dbo].[Users] ([id], [name], [email], [phone], [password], [login]) VALUES (9, N'Андрей Ковалев', N'andrey@example.com', N'+79012345678', N'andrey123', N'andrey_kv')
INSERT [dbo].[Users] ([id], [name], [email], [phone], [password], [login]) VALUES (10, N'Татьяна Лебедева', N'tatiana@example.com', N'+79123456789', N'tanya123', N'tatiana_lb')
INSERT [dbo].[Users] ([id], [name], [email], [phone], [password], [login]) VALUES (11, N'мария', N'maria@gmail.com', NULL, N'1234567a', N'mariya_gl')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[Warehouses] ON 

INSERT [dbo].[Warehouses] ([WarehouseID], [Location]) VALUES (1, N'Склад №1, Москва')
SET IDENTITY_INSERT [dbo].[Warehouses] OFF
GO
SET IDENTITY_INSERT [dbo].[Watering] ON 

INSERT [dbo].[Watering] ([id], [plant_id], [watering_date], [amount_ml]) VALUES (1, 1, CAST(N'2023-10-01' AS Date), 200)
INSERT [dbo].[Watering] ([id], [plant_id], [watering_date], [amount_ml]) VALUES (2, 2, CAST(N'2023-10-02' AS Date), 300)
INSERT [dbo].[Watering] ([id], [plant_id], [watering_date], [amount_ml]) VALUES (3, 3, CAST(N'2023-10-03' AS Date), 400)
INSERT [dbo].[Watering] ([id], [plant_id], [watering_date], [amount_ml]) VALUES (4, 4, CAST(N'2023-10-04' AS Date), 500)
INSERT [dbo].[Watering] ([id], [plant_id], [watering_date], [amount_ml]) VALUES (5, 5, CAST(N'2023-10-05' AS Date), 600)
INSERT [dbo].[Watering] ([id], [plant_id], [watering_date], [amount_ml]) VALUES (6, 6, CAST(N'2023-10-06' AS Date), 700)
INSERT [dbo].[Watering] ([id], [plant_id], [watering_date], [amount_ml]) VALUES (7, 7, CAST(N'2023-10-07' AS Date), 800)
INSERT [dbo].[Watering] ([id], [plant_id], [watering_date], [amount_ml]) VALUES (8, 8, CAST(N'2023-10-08' AS Date), 900)
INSERT [dbo].[Watering] ([id], [plant_id], [watering_date], [amount_ml]) VALUES (9, 9, CAST(N'2023-10-09' AS Date), 1000)
INSERT [dbo].[Watering] ([id], [plant_id], [watering_date], [amount_ml]) VALUES (10, 10, CAST(N'2023-10-10' AS Date), 1100)
SET IDENTITY_INSERT [dbo].[Watering] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Admins__AB6E61643E44D058]    Script Date: 10.06.2025 23:45:39 ******/
ALTER TABLE [dbo].[Admins] ADD UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__7838F272C09ED2F1]    Script Date: 10.06.2025 23:45:39 ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__AB6E616435D70EF8]    Script Date: 10.06.2025 23:45:39 ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cart] ADD  DEFAULT ((1)) FOR [quantity]
GO
ALTER TABLE [dbo].[Cart] ADD  DEFAULT (getdate()) FOR [added_date]
GO
ALTER TABLE [dbo].[Discounts] ADD  DEFAULT ('#FFD700') FOR [color]
GO
ALTER TABLE [dbo].[Discounts] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Inventory] ADD  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Inventory] ADD  DEFAULT (getdate()) FOR [LastUpdated]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [order_date]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (N'Ожидает оплаты') FOR [status]
GO
ALTER TABLE [dbo].[Reminders] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[UserPlants] ADD  DEFAULT (getdate()) FOR [purchase_date]
GO
ALTER TABLE [dbo].[Admins]  WITH CHECK ADD FOREIGN KEY([admin_id])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[CareHistory]  WITH CHECK ADD FOREIGN KEY([plant_id])
REFERENCES [dbo].[Plants] ([id])
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([plant_id])
REFERENCES [dbo].[Plants] ([id])
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Discounts]  WITH CHECK ADD FOREIGN KEY([plant_id])
REFERENCES [dbo].[Plants] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Fertilization]  WITH CHECK ADD FOREIGN KEY([fertilizer_id])
REFERENCES [dbo].[Fertilizers] ([id])
GO
ALTER TABLE [dbo].[Fertilization]  WITH CHECK ADD FOREIGN KEY([plant_id])
REFERENCES [dbo].[Plants] ([id])
GO
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD FOREIGN KEY([PlantID])
REFERENCES [dbo].[Plants] ([id])
GO
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD FOREIGN KEY([WarehouseID])
REFERENCES [dbo].[Warehouses] ([WarehouseID])
GO
ALTER TABLE [dbo].[Lighting]  WITH CHECK ADD FOREIGN KEY([plant_id])
REFERENCES [dbo].[Plants] ([id])
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD FOREIGN KEY([order_id])
REFERENCES [dbo].[Orders] ([id])
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD FOREIGN KEY([plant_id])
REFERENCES [dbo].[Plants] ([id])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Photos]  WITH CHECK ADD FOREIGN KEY([plant_id])
REFERENCES [dbo].[Plants] ([id])
GO
ALTER TABLE [dbo].[PlantLocations]  WITH CHECK ADD FOREIGN KEY([plant_id])
REFERENCES [dbo].[Plants] ([id])
GO
ALTER TABLE [dbo].[PlantLocations]  WITH CHECK ADD FOREIGN KEY([room_id])
REFERENCES [dbo].[Rooms] ([id])
GO
ALTER TABLE [dbo].[Plants]  WITH CHECK ADD FOREIGN KEY([category_id])
REFERENCES [dbo].[Categories] ([id])
GO
ALTER TABLE [dbo].[Plants]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Reminders]  WITH CHECK ADD FOREIGN KEY([plant_id])
REFERENCES [dbo].[Plants] ([id])
GO
ALTER TABLE [dbo].[Reminders]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Rooms]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Temperature]  WITH CHECK ADD FOREIGN KEY([plant_id])
REFERENCES [dbo].[Plants] ([id])
GO
ALTER TABLE [dbo].[UserPlants]  WITH CHECK ADD FOREIGN KEY([plant_id])
REFERENCES [dbo].[Plants] ([id])
GO
ALTER TABLE [dbo].[UserPlants]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Watering]  WITH CHECK ADD FOREIGN KEY([plant_id])
REFERENCES [dbo].[Plants] ([id])
GO
USE [master]
GO
ALTER DATABASE [OrganayzerRasteniy] SET  READ_WRITE 
GO
