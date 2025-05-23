USE [master]
GO
/****** Object:  Database [payment_db]    Script Date: 3/29/2025 6:40:57 AM ******/
CREATE DATABASE [payment_db]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'payment_db', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.TRUNGND\MSSQL\DATA\payment_db.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'payment_db_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.TRUNGND\MSSQL\DATA\payment_db_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [payment_db] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [payment_db].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [payment_db] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [payment_db] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [payment_db] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [payment_db] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [payment_db] SET ARITHABORT OFF 
GO
ALTER DATABASE [payment_db] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [payment_db] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [payment_db] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [payment_db] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [payment_db] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [payment_db] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [payment_db] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [payment_db] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [payment_db] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [payment_db] SET  ENABLE_BROKER 
GO
ALTER DATABASE [payment_db] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [payment_db] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [payment_db] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [payment_db] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [payment_db] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [payment_db] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [payment_db] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [payment_db] SET RECOVERY FULL 
GO
ALTER DATABASE [payment_db] SET  MULTI_USER 
GO
ALTER DATABASE [payment_db] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [payment_db] SET DB_CHAINING OFF 
GO
ALTER DATABASE [payment_db] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [payment_db] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [payment_db] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [payment_db] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'payment_db', N'ON'
GO
ALTER DATABASE [payment_db] SET QUERY_STORE = OFF
GO
USE [payment_db]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 3/29/2025 6:40:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[invoices]    Script Date: 3/29/2025 6:40:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[invoices](
	[id] [uniqueidentifier] NOT NULL,
	[student_id] [uniqueidentifier] NOT NULL,
	[total_amount] [decimal](10, 2) NOT NULL,
	[invoice_date] [datetime] NULL,
	[due_date] [datetime] NULL,
	[status] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[payments]    Script Date: 3/29/2025 6:40:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[payments](
	[id] [uniqueidentifier] NOT NULL,
	[student_id] [uniqueidentifier] NOT NULL,
	[amount] [decimal](10, 2) NOT NULL,
	[payment_status] [varchar](20) NULL,
	[payment_method] [varchar](50) NULL,
	[transaction_id] [varchar](100) NULL,
	[created_at] [datetime] NULL,
	[course_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[refunds]    Script Date: 3/29/2025 6:40:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[refunds](
	[id] [uniqueidentifier] NOT NULL,
	[payment_id] [uniqueidentifier] NOT NULL,
	[refund_amount] [decimal](10, 2) NOT NULL,
	[refund_reason] [text] NULL,
	[refund_status] [varchar](20) NULL,
	[refunded_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[payments] ([id], [student_id], [amount], [payment_status], [payment_method], [transaction_id], [created_at], [course_id]) VALUES (N'bdfbbb32-6365-4651-8353-138c59416aca', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(10000.00 AS Decimal(10, 2)), N'Completed', N'MoMo', N'638787948841323390', CAST(N'2025-03-29T04:41:25.753' AS DateTime), 20)
INSERT [dbo].[payments] ([id], [student_id], [amount], [payment_status], [payment_method], [transaction_id], [created_at], [course_id]) VALUES (N'f2c6928b-304e-49ce-ad64-14bb79020377', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(10000.00 AS Decimal(10, 2)), N'Pending', N'MoMo', N'638788017062456184', CAST(N'2025-03-29T06:35:08.857' AS DateTime), 2147483647)
INSERT [dbo].[payments] ([id], [student_id], [amount], [payment_status], [payment_method], [transaction_id], [created_at], [course_id]) VALUES (N'54bf6f50-81cb-4b7c-84cc-4785c4eb3cbe', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(10000.00 AS Decimal(10, 2)), N'Completed', N'MoMo', N'638788017133264821', CAST(N'2025-03-29T06:35:13.683' AS DateTime), 30)
INSERT [dbo].[payments] ([id], [student_id], [amount], [payment_status], [payment_method], [transaction_id], [created_at], [course_id]) VALUES (N'af7ef764-7cdf-44c4-bb63-4e95510c8011', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(10000.00 AS Decimal(10, 2)), N'Completed', N'MoMo', N'638787976260531905', CAST(N'2025-03-29T05:27:07.857' AS DateTime), 26)
INSERT [dbo].[payments] ([id], [student_id], [amount], [payment_status], [payment_method], [transaction_id], [created_at], [course_id]) VALUES (N'645eaa7c-fc78-4d9f-8d45-5569a592d200', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(10000.00 AS Decimal(10, 2)), N'Completed', N'MoMo', N'638787959489575410', CAST(N'2025-03-29T04:59:10.493' AS DateTime), 22)
INSERT [dbo].[payments] ([id], [student_id], [amount], [payment_status], [payment_method], [transaction_id], [created_at], [course_id]) VALUES (N'bf092f9e-8a57-4441-86c4-8c6f78b2937b', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(10000.00 AS Decimal(10, 2)), N'Pending', N'MoMo', N'638787882444859588', CAST(N'2025-03-29T02:50:46.347' AS DateTime), 16)
INSERT [dbo].[payments] ([id], [student_id], [amount], [payment_status], [payment_method], [transaction_id], [created_at], [course_id]) VALUES (N'230c4ddf-939e-4023-9539-9e5933bc9ace', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(10000.00 AS Decimal(10, 2)), N'Completed', N'MoMo', N'638787937221221943', CAST(N'2025-03-29T04:22:04.027' AS DateTime), 18)
INSERT [dbo].[payments] ([id], [student_id], [amount], [payment_status], [payment_method], [transaction_id], [created_at], [course_id]) VALUES (N'd4054360-50d2-400d-9169-d5b1922105fc', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(10000.00 AS Decimal(10, 2)), N'Completed', N'MoMo', N'638787870999032418', CAST(N'2025-03-29T02:31:42.340' AS DateTime), 15)
INSERT [dbo].[payments] ([id], [student_id], [amount], [payment_status], [payment_method], [transaction_id], [created_at], [course_id]) VALUES (N'28379d46-b3a7-4570-af3e-e857fdbc7713', N'c7a813c8-1496-42a1-a3d5-26888b6c41ef', CAST(20000.00 AS Decimal(10, 2)), N'Completed', N'MoMo', N'638787338629353254', CAST(N'2025-03-28T11:44:27.823' AS DateTime), 0)
INSERT [dbo].[payments] ([id], [student_id], [amount], [payment_status], [payment_method], [transaction_id], [created_at], [course_id]) VALUES (N'00e362e6-0837-4a96-8c39-eb08ef5ccb45', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(10000.00 AS Decimal(10, 2)), N'Completed', N'MoMo', N'638787932264643143', CAST(N'2025-03-29T04:13:48.290' AS DateTime), 17)
INSERT [dbo].[payments] ([id], [student_id], [amount], [payment_status], [payment_method], [transaction_id], [created_at], [course_id]) VALUES (N'388d1746-e4c8-4de9-85d0-f277bd9a7eba', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(10000.00 AS Decimal(10, 2)), N'Completed', N'MoMo', N'638787952263420541', CAST(N'2025-03-29T04:47:08.010' AS DateTime), 21)
INSERT [dbo].[payments] ([id], [student_id], [amount], [payment_status], [payment_method], [transaction_id], [created_at], [course_id]) VALUES (N'c0a42824-14bc-49b5-837b-ff69de94df5c', N'3fa85f64-5717-4562-b3fc-2c963f66afa6', CAST(1000.00 AS Decimal(10, 2)), N'Completed', N'MoMo', N'638787790031572005', CAST(N'2025-03-29T00:16:49.020' AS DateTime), 0)
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__payments__85C600AE7ACD37FC]    Script Date: 3/29/2025 6:40:57 AM ******/
ALTER TABLE [dbo].[payments] ADD UNIQUE NONCLUSTERED 
(
	[transaction_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[invoices] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[invoices] ADD  DEFAULT (getdate()) FOR [invoice_date]
GO
ALTER TABLE [dbo].[payments] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[payments] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[refunds] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[refunds] ADD  DEFAULT (getdate()) FOR [refunded_at]
GO
ALTER TABLE [dbo].[refunds]  WITH CHECK ADD FOREIGN KEY([payment_id])
REFERENCES [dbo].[payments] ([id])
GO
ALTER TABLE [dbo].[invoices]  WITH CHECK ADD CHECK  (([status]='overdue' OR [status]='paid' OR [status]='unpaid'))
GO
ALTER TABLE [dbo].[payments]  WITH CHECK ADD CHECK  (([payment_status]='failed' OR [payment_status]='completed' OR [payment_status]='pending'))
GO
ALTER TABLE [dbo].[payments]  WITH CHECK ADD CHECK  (([payment_method]='bank_transfer' OR [payment_method]='credit_card' OR [payment_method]='momo'))
GO
ALTER TABLE [dbo].[refunds]  WITH CHECK ADD CHECK  (([refund_status]='rejected' OR [refund_status]='approved' OR [refund_status]='pending'))
GO
USE [master]
GO
ALTER DATABASE [payment_db] SET  READ_WRITE 
GO
