CREATE TABLE [dbo].[Drinks] (
    [drink_id]    INT          IDENTITY (1, 1) NOT NULL,
    [drink_name]  NVARCHAR(50) DEFAULT ((0)) NOT NULL,
    [drink_price] INT          DEFAULT ((0)) NOT NULL,
    [drink_qt]    INT          DEFAULT ((0)) NOT NULL,
    [drink_class] NVARCHAR(50) NULL,
    PRIMARY KEY CLUSTERED ([drink_id] ASC)
);

