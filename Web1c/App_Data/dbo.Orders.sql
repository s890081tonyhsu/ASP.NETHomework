CREATE TABLE [dbo].[Orders] (
    [order_id]        INT            IDENTITY (1, 1) NOT NULL,
    [order_time]      SMALLDATETIME  NULL,
    [order_user_pass] NVARCHAR (MAX) NULL,
    [order_memo]      NVARCHAR (MAX) NULL,
    [order_status] INT NOT NULL DEFAULT 1, 
    PRIMARY KEY CLUSTERED ([order_id] ASC)
);

