create database PackageDelivery
go
USE PackageDelivery
GO
/****** Object:  Table [dbo].[ADDR_TBL]    Script Date: 1/16/2018 11:34:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ADDR_TBL](
	[ID_CLM] [int] IDENTITY(1,1) NOT NULL,
	[STR] [char](300) NULL,
	[CT_ST] [char](200) NULL,
	[ZP] [char](5) NULL,
	[DLVR] [int] NULL,
 CONSTRAINT [PK_ADDR_TBL] PRIMARY KEY CLUSTERED 
(
	[ID_CLM] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DLVR_TBL]    Script Date: 1/16/2018 11:34:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DLVR_TBL](
	[NMB_CLM] [int] IDENTITY(1,1) NOT NULL,
	[CSTM] [int] NULL,
	[STS] [char](1) NULL,
	[ESTM_CLM] [float] NULL,
	[PRD_LN_1] [int] NULL,
	[PRD_LN_1_AMN] [char](2) NULL,
	[PRD_LN_2] [int] NULL,
	[PRD_LN_2_AMN] [char](2) NULL,
 CONSTRAINT [PK_DLVR_TBL] PRIMARY KEY CLUSTERED 
(
	[NMB_CLM] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DLVR_TBL2]    Script Date: 1/16/2018 11:34:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DLVR_TBL2](
	[NMB_CLM] [int] NOT NULL,
	[PRD_LN_3] [int] NULL,
	[PRD_LN_3_AMN] [char](2) NULL,
	[PRD_LN_4] [int] NULL,
	[PRD_LN_4_AMN] [char](2) NULL,
 CONSTRAINT [PK_DLVR_TBL2] PRIMARY KEY CLUSTERED 
(
	[NMB_CLM] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[P_TBL]    Script Date: 1/16/2018 11:34:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[P_TBL](
	[NMB_CLM] [int] IDENTITY(1,1) NOT NULL,
	[NM_CLM] [char](100) NULL,
	[ADDR1] [int] NULL,
	[ADDR2] [int] NULL,
 CONSTRAINT [PK_P_TBL] PRIMARY KEY CLUSTERED 
(
	[NMB_CLM] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PRD_TBL]    Script Date: 1/16/2018 11:34:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PRD_TBL](
	[NMB_CM] [int] IDENTITY(1,1) NOT NULL,
	[NM_CLM] [char](100) NULL,
	[DSC_CLM] [char](500) NULL,
	[WT] [float] NULL,
	[WT_KG] [float] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ADDR_TBL] ON 

GO
INSERT [dbo].[ADDR_TBL] ([ID_CLM], [STR], [CT_ST], [ZP], [DLVR]) VALUES (1, N'1234 Main St                                                                                                                                                                                                                                                                                                ', N'Washington DC                                                                                                                                                                                           ', N'22200', NULL)
GO
INSERT [dbo].[ADDR_TBL] ([ID_CLM], [STR], [CT_ST], [ZP], [DLVR]) VALUES (2, N'2345 2nd St                                                                                                                                                                                                                                                                                                 ', N'Washington DC                                                                                                                                                                                           ', N'22201', NULL)
GO
INSERT [dbo].[ADDR_TBL] ([ID_CLM], [STR], [CT_ST], [ZP], [DLVR]) VALUES (3, N'8338 3rd St                                                                                                                                                                                                                                                                                                 ', N'Arlington VA                                                                                                                                                                                            ', N'22202', NULL)
GO
INSERT [dbo].[ADDR_TBL] ([ID_CLM], [STR], [CT_ST], [ZP], [DLVR]) VALUES (11, N'1234 Main St                                                                                                                                                                                                                                                                                                ', N'Washington DC                                                                                                                                                                                           ', N'22200', 8)
GO
INSERT [dbo].[ADDR_TBL] ([ID_CLM], [STR], [CT_ST], [ZP], [DLVR]) VALUES (12, N'1321 S Eads St                                                                                                                                                                                                                                                                                              ', N'Arlingron VA                                                                                                                                                                                            ', N'22202', 9)
GO
SET IDENTITY_INSERT [dbo].[ADDR_TBL] OFF
GO
SET IDENTITY_INSERT [dbo].[DLVR_TBL] ON 

GO
INSERT [dbo].[DLVR_TBL] ([NMB_CLM], [CSTM], [STS], [ESTM_CLM], [PRD_LN_1], [PRD_LN_1_AMN], [PRD_LN_2], [PRD_LN_2_AMN]) VALUES (8, 2, N'R', 320, 1, N'2 ', 2, N'1 ')
GO
INSERT [dbo].[DLVR_TBL] ([NMB_CLM], [CSTM], [STS], [ESTM_CLM], [PRD_LN_1], [PRD_LN_1_AMN], [PRD_LN_2], [PRD_LN_2_AMN]) VALUES (9, 1, N'R', 40, 1, N'1 ', NULL, N'0 ')
GO
SET IDENTITY_INSERT [dbo].[DLVR_TBL] OFF
GO
INSERT [dbo].[DLVR_TBL2] ([NMB_CLM], [PRD_LN_3], [PRD_LN_3_AMN], [PRD_LN_4], [PRD_LN_4_AMN]) VALUES (8, 3, N'1 ', 5, N'4 ')
GO
INSERT [dbo].[DLVR_TBL2] ([NMB_CLM], [PRD_LN_3], [PRD_LN_3_AMN], [PRD_LN_4], [PRD_LN_4_AMN]) VALUES (9, NULL, N'0 ', NULL, N'0 ')
GO
SET IDENTITY_INSERT [dbo].[P_TBL] ON 

GO
INSERT [dbo].[P_TBL] ([NMB_CLM], [NM_CLM], [ADDR1], [ADDR2]) VALUES (1, N'Macro Soft                                                                                          ', 1, NULL)
GO
INSERT [dbo].[P_TBL] ([NMB_CLM], [NM_CLM], [ADDR1], [ADDR2]) VALUES (2, N'Devices for Everyone                                                                                ', 2, NULL)
GO
INSERT [dbo].[P_TBL] ([NMB_CLM], [NM_CLM], [ADDR1], [ADDR2]) VALUES (3, N'Kwik-E-Mart                                                                                         ', 3, NULL)
GO
SET IDENTITY_INSERT [dbo].[P_TBL] OFF
GO
SET IDENTITY_INSERT [dbo].[PRD_TBL] ON 

GO
INSERT [dbo].[PRD_TBL] ([NMB_CM], [NM_CLM], [DSC_CLM], [WT], [WT_KG]) VALUES (1, N'Best Pizza Ever                                                                                     ', N'Your favorite cheese pizza made with classic marinara sauce topped with mozzarella cheese.                                                                                                                                                                                                                                                                                                                                                                                                                          ', 2, NULL)
GO
INSERT [dbo].[PRD_TBL] ([NMB_CM], [NM_CLM], [DSC_CLM], [WT], [WT_KG]) VALUES (2, N'myPhone                                                                                             ', NULL, NULL, 0.5)
GO
INSERT [dbo].[PRD_TBL] ([NMB_CM], [NM_CLM], [DSC_CLM], [WT], [WT_KG]) VALUES (3, N'Couch                                                                                               ', N'Made with a sturdy wood frame and upholstered in touchable and classic linen, this fold-down futon provides a stylish seating solution along with an extra space for overnight guests.                                                                                                                                                                                                                                                                                                                              ', 83.5, NULL)
GO
INSERT [dbo].[PRD_TBL] ([NMB_CM], [NM_CLM], [DSC_CLM], [WT], [WT_KG]) VALUES (4, N'TV Set                                                                                              ', NULL, NULL, 7)
GO
INSERT [dbo].[PRD_TBL] ([NMB_CM], [NM_CLM], [DSC_CLM], [WT], [WT_KG]) VALUES (5, N'Fridge                                                                                              ', NULL, NULL, 34)
GO
SET IDENTITY_INSERT [dbo].[PRD_TBL] OFF
GO
