CREATE TABLE [dbo].[Order] (
    [order_id]        INT            NOT NULL IDENTITY,
    [order_time]      SMALLDATETIME     NULL,
    [order_user_pass] NVARCHAR (MAX) NULL,
    [memo]            NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([order_id] ASC)
);

