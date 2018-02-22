CREATE TABLE [dbo].[Synchronization](
	[IsSyncRequired] [bit] NOT NULL
) ON [PRIMARY]
GO
INSERT [dbo].[Synchronization] VALUES (0)
GO

ALTER TABLE [dbo].[DLVR_TBL] ADD
	IsSyncNeeded bit NOT NULL DEFAULT 0
GO

CREATE TRIGGER dbo.AddressSync ON [dbo].[ADDR_TBL]
AFTER UPDATE, INSERT
AS 
BEGIN
    SET NOCOUNT ON;

	UPDATE [dbo].[DLVR_TBL]
	SET IsSyncNeeded = 1
	FROM INSERTED i
	LEFT JOIN DELETED d ON i.ID_CLM = d.ID_CLM
	WHERE NMB_CLM = i.DLVR AND (d.[STR] IS NULL OR i.[STR] <> d.[STR] OR i.[CT_ST] <> d.[CT_ST] OR i.[ZP] <> d.[ZP])

	UPDATE [dbo].[Synchronization]
	SET [IsSyncRequired] = 1
	FROM INSERTED i
	LEFT JOIN DELETED d ON i.ID_CLM = d.ID_CLM
	WHERE d.[STR] IS NULL OR i.[STR] <> d.[STR] OR i.[CT_ST] <> d.[CT_ST] OR i.[ZP] <> d.[ZP]
END
