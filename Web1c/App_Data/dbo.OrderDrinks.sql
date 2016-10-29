CREATE TABLE [dbo].[OrderDrinks] (
    [orderdrink_count]    INT IDENTITY (1, 1) NOT NULL,
    [orderdrink_order_id] INT NOT NULL,
    [orderdrink_drink_id] INT NOT NULL,
    [orderdrink_no]       INT DEFAULT ((0)) NOT NULL,
    [orderdrink_sweet]    INT DEFAULT ((0)) NOT NULL,
    [orderdrink_ice]      INT DEFAULT ((-1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([orderdrink_count] ASC)
);

