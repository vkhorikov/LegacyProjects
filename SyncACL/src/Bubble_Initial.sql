CREATE DATABASE [PackageDeliveryNew]
GO
USE [PackageDeliveryNew]
GO
/****** Object:  Table [dbo].[Delivery]    Script Date: 4/23/2018 3:01:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Delivery](
	[DeliveryID] [int] NOT NULL,
	[CostEstimate] [decimal](18, 2) NULL,
	[DestinationStreet] [nvarchar](300) NOT NULL,
	[DestinationCity] [nvarchar](100) NOT NULL,
	[DestinationState] [nvarchar](100) NOT NULL,
	[DestinationZipCode] [nvarchar](5) NOT NULL,
 CONSTRAINT [PK_Delivery] PRIMARY KEY CLUSTERED 
(
	[DeliveryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product]    Script Date: 4/23/2018 3:01:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [int] NOT NULL,
	[WeightInPounds] [float] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProductLine]    Script Date: 4/23/2018 3:01:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductLine](
	[ProductLineID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[DeliveryID] [int] NOT NULL,
 CONSTRAINT [PK_ProductLine] PRIMARY KEY CLUSTERED 
(
	[ProductLineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Synchronization]    Script Date: 4/23/2018 3:01:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [dbo].[ProductLine]  WITH CHECK ADD  CONSTRAINT [FK_ProductLine_Delivery] FOREIGN KEY([DeliveryID])
REFERENCES [dbo].[Delivery] ([DeliveryID])
GO
ALTER TABLE [dbo].[ProductLine] CHECK CONSTRAINT [FK_ProductLine_Delivery]
GO
ALTER TABLE [dbo].[ProductLine]  WITH CHECK ADD  CONSTRAINT [FK_ProductLine_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[ProductLine] CHECK CONSTRAINT [FK_ProductLine_Product]
GO
