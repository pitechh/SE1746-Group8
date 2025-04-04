USE [master]
GO
/****** Object:  Database [enroll_db]    Script Date: 3/29/2025 6:39:02 AM ******/
CREATE DATABASE [enroll_db]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'enroll_db', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.TRUNGND\MSSQL\DATA\enroll_db.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'enroll_db_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.TRUNGND\MSSQL\DATA\enroll_db_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [enroll_db] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [enroll_db].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [enroll_db] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [enroll_db] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [enroll_db] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [enroll_db] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [enroll_db] SET ARITHABORT OFF 
GO
ALTER DATABASE [enroll_db] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [enroll_db] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [enroll_db] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [enroll_db] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [enroll_db] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [enroll_db] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [enroll_db] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [enroll_db] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [enroll_db] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [enroll_db] SET  ENABLE_BROKER 
GO
ALTER DATABASE [enroll_db] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [enroll_db] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [enroll_db] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [enroll_db] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [enroll_db] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [enroll_db] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [enroll_db] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [enroll_db] SET RECOVERY FULL 
GO
ALTER DATABASE [enroll_db] SET  MULTI_USER 
GO
ALTER DATABASE [enroll_db] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [enroll_db] SET DB_CHAINING OFF 
GO
ALTER DATABASE [enroll_db] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [enroll_db] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [enroll_db] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [enroll_db] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'enroll_db', N'ON'
GO
ALTER DATABASE [enroll_db] SET QUERY_STORE = OFF
GO
USE [enroll_db]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 3/29/2025 6:39:02 AM ******/
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
/****** Object:  Table [dbo].[course_progress]    Script Date: 3/29/2025 6:39:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[course_progress](
	[id] [uniqueidentifier] NOT NULL,
	[student_id] [uniqueidentifier] NOT NULL,
	[course_id] [uniqueidentifier] NOT NULL,
	[progress_percentage] [decimal](5, 2) NULL,
	[last_accessed] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[enrollments]    Script Date: 3/29/2025 6:39:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[enrollments](
	[id] [uniqueidentifier] NOT NULL,
	[student_id] [uniqueidentifier] NOT NULL,
	[enrolled_date] [datetime] NULL,
	[status] [varchar](20) NULL,
	[course_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[students]    Script Date: 3/29/2025 6:39:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[students](
	[id] [uniqueidentifier] NOT NULL,
	[user_id] [uniqueidentifier] NOT NULL,
	[enrolled_at] [datetime] NULL,
	[total_courses_enrolled] [int] NULL,
	[completed_courses] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[enrollments] ([id], [student_id], [enrolled_date], [status], [course_id]) VALUES (N'3e3245fa-1daf-4645-a229-069068fa0099', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(N'2025-03-28T19:13:23.417' AS DateTime), N'pending', 12)
INSERT [dbo].[enrollments] ([id], [student_id], [enrolled_date], [status], [course_id]) VALUES (N'1f7c9b87-7cca-4063-8e48-0fc642ca7829', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(N'2025-03-28T21:13:45.977' AS DateTime), N'pending', 17)
INSERT [dbo].[enrollments] ([id], [student_id], [enrolled_date], [status], [course_id]) VALUES (N'302f9947-7432-460f-8694-10454ae0a101', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(N'2025-03-28T22:22:59.933' AS DateTime), N'pending', 24)
INSERT [dbo].[enrollments] ([id], [student_id], [enrolled_date], [status], [course_id]) VALUES (N'16c82b01-9717-4f81-bb59-31ac32ca5aff', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(N'2025-03-28T21:22:01.637' AS DateTime), N'pending', 18)
INSERT [dbo].[enrollments] ([id], [student_id], [enrolled_date], [status], [course_id]) VALUES (N'7dd7fe97-6d97-4846-98aa-397245566878', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(N'2025-03-28T21:59:08.470' AS DateTime), N'completed', 22)
INSERT [dbo].[enrollments] ([id], [student_id], [enrolled_date], [status], [course_id]) VALUES (N'fc619407-ef98-43a0-89cc-3b02d698a129', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(N'2025-03-28T23:35:13.313' AS DateTime), N'completed', 30)
INSERT [dbo].[enrollments] ([id], [student_id], [enrolled_date], [status], [course_id]) VALUES (N'bd9e71e7-0597-423b-97c3-535538fcb73c', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(N'2025-03-28T21:47:05.823' AS DateTime), N'pending', 21)
INSERT [dbo].[enrollments] ([id], [student_id], [enrolled_date], [status], [course_id]) VALUES (N'8ead0828-d595-4289-a98c-60742281aa72', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(N'2025-03-28T22:27:05.480' AS DateTime), N'completed', 26)
INSERT [dbo].[enrollments] ([id], [student_id], [enrolled_date], [status], [course_id]) VALUES (N'96915e77-7e1e-4379-8dbd-65cdde1d35da', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(N'2025-03-28T22:20:19.353' AS DateTime), N'pending', 23)
INSERT [dbo].[enrollments] ([id], [student_id], [enrolled_date], [status], [course_id]) VALUES (N'eaf37995-5bb6-4ab4-9362-89bf36d4ac3a', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(N'2025-03-28T19:50:43.877' AS DateTime), N'completed', 16)
INSERT [dbo].[enrollments] ([id], [student_id], [enrolled_date], [status], [course_id]) VALUES (N'5427c7bd-1bab-417a-a785-b1b06fb8becf', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(N'2025-03-28T19:31:39.170' AS DateTime), N'pending', 15)
INSERT [dbo].[enrollments] ([id], [student_id], [enrolled_date], [status], [course_id]) VALUES (N'5d3e88fd-fdfd-460e-acf4-c4836ccf722f', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(N'2025-03-28T23:35:05.337' AS DateTime), N'pending', 2147483647)
INSERT [dbo].[enrollments] ([id], [student_id], [enrolled_date], [status], [course_id]) VALUES (N'1c634baf-5bed-4d53-8f01-dd0809ad99c4', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(N'2025-03-28T22:24:54.567' AS DateTime), N'pending', 25)
INSERT [dbo].[enrollments] ([id], [student_id], [enrolled_date], [status], [course_id]) VALUES (N'96c40ae6-3546-4f21-a46c-e702cfb27e74', N'ec5ed104-9864-42a8-90f2-fd481fca5812', CAST(N'2025-03-28T21:41:23.640' AS DateTime), N'pending', 20)
GO
INSERT [dbo].[students] ([id], [user_id], [enrolled_at], [total_courses_enrolled], [completed_courses]) VALUES (N'35fb3c81-659c-489e-a113-07b69166d441', N'3b4ba19c-52fc-4f79-b82a-0ce7cbbcf2c7', CAST(N'2025-03-07T20:26:10.053' AS DateTime), 1, 0)
INSERT [dbo].[students] ([id], [user_id], [enrolled_at], [total_courses_enrolled], [completed_courses]) VALUES (N'0a37eb6c-d25f-4297-a493-1bd81e711a30', N'df9a584a-5925-4550-984a-cf325289d9d9', CAST(N'2025-03-07T20:26:10.053' AS DateTime), 4, 2)
INSERT [dbo].[students] ([id], [user_id], [enrolled_at], [total_courses_enrolled], [completed_courses]) VALUES (N'c206c30e-efe4-4a5e-84dc-28d2bfe79eb9', N'526612cf-44dc-4ac6-bf54-041278f48c40', CAST(N'2025-03-07T20:26:10.053' AS DateTime), 6, 4)
INSERT [dbo].[students] ([id], [user_id], [enrolled_at], [total_courses_enrolled], [completed_courses]) VALUES (N'a35693df-6364-4cee-9ead-3b5ad67584e1', N'42d1e623-3fd6-441c-9805-12d60092aea9', CAST(N'2025-03-07T20:26:10.053' AS DateTime), 5, 3)
INSERT [dbo].[students] ([id], [user_id], [enrolled_at], [total_courses_enrolled], [completed_courses]) VALUES (N'09b67a12-2fc2-4d5a-b106-6a90bce071da', N'f377c3a7-b7b1-4771-9581-2b34c0051fda', CAST(N'2025-03-07T20:26:10.053' AS DateTime), 2, 0)
INSERT [dbo].[students] ([id], [user_id], [enrolled_at], [total_courses_enrolled], [completed_courses]) VALUES (N'06fdb9e5-1ddf-482f-820f-77905f938110', N'afb14d0d-be41-4552-bbd9-5c6cbe11aaac', CAST(N'2025-03-07T20:26:10.053' AS DateTime), 3, 1)
INSERT [dbo].[students] ([id], [user_id], [enrolled_at], [total_courses_enrolled], [completed_courses]) VALUES (N'971fb6fa-2685-4546-b113-b9d8a53e337f', N'd4a452f8-91a0-43f1-9102-4087f2d17cd8', CAST(N'2025-03-07T20:26:10.053' AS DateTime), 5, 3)
INSERT [dbo].[students] ([id], [user_id], [enrolled_at], [total_courses_enrolled], [completed_courses]) VALUES (N'a67c77ba-837e-44da-a53c-cd7c2e91739b', N'06c4c927-2fee-4453-8733-9ec78bf7b8d6', CAST(N'2025-03-07T20:26:10.053' AS DateTime), 4, 2)
INSERT [dbo].[students] ([id], [user_id], [enrolled_at], [total_courses_enrolled], [completed_courses]) VALUES (N'102bc755-c891-487c-9caf-e8f7f6f98e56', N'ca777ac8-e992-4a36-a77f-96d7eb8e5816', CAST(N'2025-03-07T20:26:10.053' AS DateTime), 2, 1)
INSERT [dbo].[students] ([id], [user_id], [enrolled_at], [total_courses_enrolled], [completed_courses]) VALUES (N'ec5ed104-9864-42a8-90f2-fd481fca5812', N'5d73a8ed-187b-4337-be35-32b1d9317a96', CAST(N'2025-03-28T20:38:22.190' AS DateTime), 0, 0)
INSERT [dbo].[students] ([id], [user_id], [enrolled_at], [total_courses_enrolled], [completed_courses]) VALUES (N'ec5ed104-9864-42a8-90f2-fd481fca5853', N'3dd4e552-f51d-4460-ab30-85ecae0d2d23', CAST(N'2025-03-07T20:26:10.053' AS DateTime), 3, 1)
GO
/****** Object:  Index [UQ__students__B9BE370E2550203D]    Script Date: 3/29/2025 6:39:02 AM ******/
ALTER TABLE [dbo].[students] ADD UNIQUE NONCLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[course_progress] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[course_progress] ADD  DEFAULT (getdate()) FOR [last_accessed]
GO
ALTER TABLE [dbo].[enrollments] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[students] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[students] ADD  DEFAULT (getdate()) FOR [enrolled_at]
GO
ALTER TABLE [dbo].[students] ADD  DEFAULT ((0)) FOR [total_courses_enrolled]
GO
ALTER TABLE [dbo].[students] ADD  DEFAULT ((0)) FOR [completed_courses]
GO
ALTER TABLE [dbo].[course_progress]  WITH CHECK ADD FOREIGN KEY([student_id])
REFERENCES [dbo].[students] ([id])
GO
ALTER TABLE [dbo].[enrollments]  WITH CHECK ADD FOREIGN KEY([student_id])
REFERENCES [dbo].[students] ([id])
GO
ALTER TABLE [dbo].[course_progress]  WITH CHECK ADD CHECK  (([progress_percentage]>=(0) AND [progress_percentage]<=(100)))
GO
ALTER TABLE [dbo].[enrollments]  WITH CHECK ADD  CONSTRAINT [chk_enrollments_status] CHECK  (([status]='cancelled' OR [status]='completed' OR [status]='active' OR [status]='pending'))
GO
ALTER TABLE [dbo].[enrollments] CHECK CONSTRAINT [chk_enrollments_status]
GO
USE [master]
GO
ALTER DATABASE [enroll_db] SET  READ_WRITE 
GO
