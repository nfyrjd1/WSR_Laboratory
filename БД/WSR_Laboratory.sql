USE [WSR_Laboratory]
GO
/****** Object:  Table [dbo].[analyzer]    Script Date: 19.03.2021 22:20:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[analyzer](
	[id_analyzer] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_analyzer] PRIMARY KEY CLUSTERED 
(
	[id_analyzer] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[analyzer_blood_service]    Script Date: 19.03.2021 22:20:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[analyzer_blood_service](
	[id_analyzer_blood_service] [int] IDENTITY(1,1) NOT NULL,
	[id_blood_service] [int] NOT NULL,
	[id_analyzer] [int] NOT NULL,
	[date_reception] [datetime] NOT NULL,
	[date_finished] [datetime] NULL,
	[analyze_time_sec] [numeric](6, 0) NULL,
 CONSTRAINT [PK_analyzer_service] PRIMARY KEY CLUSTERED 
(
	[id_analyzer_blood_service] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[blood]    Script Date: 19.03.2021 22:20:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[blood](
	[id_blood] [int] IDENTITY(1,1) NOT NULL,
	[id_patient] [int] NOT NULL,
	[barcode] [numeric](7, 0) NOT NULL,
	[date_create] [date] NOT NULL,
	[analyze_time_days] [numeric](3, 0) NOT NULL,
	[id_status] [int] NOT NULL,
 CONSTRAINT [PK_blood] PRIMARY KEY CLUSTERED 
(
	[id_blood] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[blood_service]    Script Date: 19.03.2021 22:20:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[blood_service](
	[id_blood_service] [int] IDENTITY(1,1) NOT NULL,
	[id_blood] [int] NOT NULL,
	[id_service] [int] NOT NULL,
	[id_status] [int] NOT NULL,
	[date_finished] [date] NULL,
	[result] [numeric](6, 5) NULL,
	[accepted] [bit] NULL,
	[id_employee] [int] NOT NULL,
 CONSTRAINT [PK_blood_service] PRIMARY KEY CLUSTERED 
(
	[id_blood_service] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[employee]    Script Date: 19.03.2021 22:20:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[employee](
	[id_employee] [int] IDENTITY(1,1) NOT NULL,
	[full_name] [nvarchar](80) NOT NULL,
	[id_user] [int] NOT NULL,
 CONSTRAINT [PK_employee] PRIMARY KEY CLUSTERED 
(
	[id_employee] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[employee_service]    Script Date: 19.03.2021 22:20:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[employee_service](
	[id_employee_service] [int] IDENTITY(1,1) NOT NULL,
	[id_employee] [int] NOT NULL,
	[id_service] [int] NOT NULL,
 CONSTRAINT [PK_employee_service] PRIMARY KEY CLUSTERED 
(
	[id_employee_service] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[history]    Script Date: 19.03.2021 22:20:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[history](
	[id_history] [int] IDENTITY(1,1) NOT NULL,
	[id_user] [int] NULL,
	[login] [nvarchar](50) NOT NULL,
	[date_time] [datetime] NOT NULL,
	[ip] [nvarchar](17) NULL,
	[has_entered] [bit] NOT NULL,
 CONSTRAINT [PK_history] PRIMARY KEY CLUSTERED 
(
	[id_history] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[insurance]    Script Date: 19.03.2021 22:20:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[insurance](
	[id_insurance] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[inn] [numeric](7, 0) NOT NULL,
	[address] [nvarchar](100) NOT NULL,
	[bik] [numeric](9, 0) NOT NULL,
	[payment_account] [numeric](9, 0) NOT NULL,
 CONSTRAINT [PK_insurance] PRIMARY KEY CLUSTERED 
(
	[id_insurance] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[patient]    Script Date: 19.03.2021 22:20:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[patient](
	[id_patient] [int] IDENTITY(1,1) NOT NULL,
	[full_name] [nvarchar](80) NOT NULL,
	[email] [nvarchar](50) NOT NULL,
	[ein] [nvarchar](10) NOT NULL,
	[phone] [nvarchar](20) NOT NULL,
	[passport_series] [numeric](4, 0) NOT NULL,
	[passport_number] [numeric](6, 0) NOT NULL,
	[birthday] [date] NOT NULL,
	[country] [nvarchar](50) NOT NULL,
	[social_number] [numeric](8, 0) NOT NULL,
	[social_type] [bit] NOT NULL,
	[id_user] [int] NOT NULL,
	[id_insurance] [int] NOT NULL,
 CONSTRAINT [PK_patient] PRIMARY KEY CLUSTERED 
(
	[id_patient] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[service]    Script Date: 19.03.2021 22:20:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[service](
	[id_service] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[code] [numeric](3, 0) NOT NULL,
	[price] [numeric](6, 2) NOT NULL,
 CONSTRAINT [PK_service] PRIMARY KEY CLUSTERED 
(
	[id_service] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[status]    Script Date: 19.03.2021 22:20:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[status](
	[id_status] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_status] PRIMARY KEY CLUSTERED 
(
	[id_status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user]    Script Date: 19.03.2021 22:20:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user](
	[id_user] [int] IDENTITY(1,1) NOT NULL,
	[login] [nvarchar](50) NOT NULL,
	[password] [nvarchar](50) NOT NULL,
	[id_user_type] [int] NOT NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[id_user] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user_type]    Script Date: 19.03.2021 22:20:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user_type](
	[id_user_type] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_user_type] PRIMARY KEY CLUSTERED 
(
	[id_user_type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[analyzer_blood_service]  WITH CHECK ADD  CONSTRAINT [FK_analyzer_service_analyzer] FOREIGN KEY([id_analyzer])
REFERENCES [dbo].[analyzer] ([id_analyzer])
GO
ALTER TABLE [dbo].[analyzer_blood_service] CHECK CONSTRAINT [FK_analyzer_service_analyzer]
GO
ALTER TABLE [dbo].[analyzer_blood_service]  WITH CHECK ADD  CONSTRAINT [FK_analyzer_service_blood_service] FOREIGN KEY([id_blood_service])
REFERENCES [dbo].[blood_service] ([id_blood_service])
GO
ALTER TABLE [dbo].[analyzer_blood_service] CHECK CONSTRAINT [FK_analyzer_service_blood_service]
GO
ALTER TABLE [dbo].[blood]  WITH CHECK ADD  CONSTRAINT [FK_blood_patient] FOREIGN KEY([id_patient])
REFERENCES [dbo].[patient] ([id_patient])
GO
ALTER TABLE [dbo].[blood] CHECK CONSTRAINT [FK_blood_patient]
GO
ALTER TABLE [dbo].[blood]  WITH CHECK ADD  CONSTRAINT [FK_blood_status] FOREIGN KEY([id_status])
REFERENCES [dbo].[status] ([id_status])
GO
ALTER TABLE [dbo].[blood] CHECK CONSTRAINT [FK_blood_status]
GO
ALTER TABLE [dbo].[blood_service]  WITH CHECK ADD  CONSTRAINT [FK_blood_service_blood] FOREIGN KEY([id_blood])
REFERENCES [dbo].[blood] ([id_blood])
GO
ALTER TABLE [dbo].[blood_service] CHECK CONSTRAINT [FK_blood_service_blood]
GO
ALTER TABLE [dbo].[blood_service]  WITH CHECK ADD  CONSTRAINT [FK_blood_service_employee] FOREIGN KEY([id_employee])
REFERENCES [dbo].[employee] ([id_employee])
GO
ALTER TABLE [dbo].[blood_service] CHECK CONSTRAINT [FK_blood_service_employee]
GO
ALTER TABLE [dbo].[blood_service]  WITH CHECK ADD  CONSTRAINT [FK_blood_service_service] FOREIGN KEY([id_service])
REFERENCES [dbo].[service] ([id_service])
GO
ALTER TABLE [dbo].[blood_service] CHECK CONSTRAINT [FK_blood_service_service]
GO
ALTER TABLE [dbo].[blood_service]  WITH CHECK ADD  CONSTRAINT [FK_blood_service_status] FOREIGN KEY([id_service])
REFERENCES [dbo].[status] ([id_status])
GO
ALTER TABLE [dbo].[blood_service] CHECK CONSTRAINT [FK_blood_service_status]
GO
ALTER TABLE [dbo].[employee]  WITH CHECK ADD  CONSTRAINT [FK_employee_user] FOREIGN KEY([id_user])
REFERENCES [dbo].[user] ([id_user])
GO
ALTER TABLE [dbo].[employee] CHECK CONSTRAINT [FK_employee_user]
GO
ALTER TABLE [dbo].[employee_service]  WITH CHECK ADD  CONSTRAINT [FK_employee_service_employee] FOREIGN KEY([id_employee])
REFERENCES [dbo].[employee] ([id_employee])
GO
ALTER TABLE [dbo].[employee_service] CHECK CONSTRAINT [FK_employee_service_employee]
GO
ALTER TABLE [dbo].[employee_service]  WITH CHECK ADD  CONSTRAINT [FK_employee_service_service] FOREIGN KEY([id_service])
REFERENCES [dbo].[service] ([id_service])
GO
ALTER TABLE [dbo].[employee_service] CHECK CONSTRAINT [FK_employee_service_service]
GO
ALTER TABLE [dbo].[history]  WITH CHECK ADD  CONSTRAINT [FK_history_user] FOREIGN KEY([id_user])
REFERENCES [dbo].[user] ([id_user])
GO
ALTER TABLE [dbo].[history] CHECK CONSTRAINT [FK_history_user]
GO
ALTER TABLE [dbo].[patient]  WITH CHECK ADD  CONSTRAINT [FK_patient_insurance] FOREIGN KEY([id_insurance])
REFERENCES [dbo].[insurance] ([id_insurance])
GO
ALTER TABLE [dbo].[patient] CHECK CONSTRAINT [FK_patient_insurance]
GO
ALTER TABLE [dbo].[patient]  WITH CHECK ADD  CONSTRAINT [FK_patient_user] FOREIGN KEY([id_user])
REFERENCES [dbo].[user] ([id_user])
GO
ALTER TABLE [dbo].[patient] CHECK CONSTRAINT [FK_patient_user]
GO
ALTER TABLE [dbo].[user]  WITH CHECK ADD  CONSTRAINT [FK_user_user_type] FOREIGN KEY([id_user_type])
REFERENCES [dbo].[user_type] ([id_user_type])
GO
ALTER TABLE [dbo].[user] CHECK CONSTRAINT [FK_user_user_type]
GO
