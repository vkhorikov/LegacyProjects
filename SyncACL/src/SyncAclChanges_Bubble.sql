CREATE TABLE [dbo].[Synchronization](
	[IsSyncRequired] [bit] NOT NULL
) ON [PRIMARY]
GO
INSERT [dbo].[Synchronization] VALUES (0)
GO

ALTER TABLE [dbo].[Delivery] ADD
	IsSyncNeeded bit NOT NULL DEFAULT 0
GO

CREATE TRIGGER dbo.DeliverySync ON [dbo].[Delivery]
AFTER UPDATE
AS 
BEGIN
    SET NOCOUNT ON;

	UPDATE [dbo].[Delivery]
	SET IsSyncNeeded = 1
	FROM INSERTED i
	LEFT JOIN DELETED d ON i.DeliveryID = d.DeliveryID
	WHERE [dbo].[Delivery].DeliveryID = i.DeliveryID AND i.CostEstimate <> d.CostEstimate

	UPDATE [dbo].[Synchronization]
	SET [IsSyncRequired] = 1
	FROM INSERTED i
	LEFT JOIN DELETED d ON i.DeliveryID = d.DeliveryID
	WHERE i.CostEstimate <> d.CostEstimate
END
GO

CREATE TRIGGER dbo.ProductLineSync ON [dbo].[ProductLine]
AFTER INSERT, DELETE
AS 
BEGIN
    SET NOCOUNT ON;

	UPDATE [dbo].[Delivery]
	SET IsSyncNeeded = 1
	FROM [dbo].[Delivery]
	LEFT JOIN INSERTED i ON [dbo].[Delivery].DeliveryID = i.DeliveryID
	LEFT JOIN DELETED d ON [dbo].[Delivery].DeliveryID = d.DeliveryID
	WHERE [dbo].[Delivery].DeliveryID = i.DeliveryID OR [dbo].[Delivery].DeliveryID = d.DeliveryID

	UPDATE [dbo].[Synchronization]
	SET [IsSyncRequired] = 1
END
