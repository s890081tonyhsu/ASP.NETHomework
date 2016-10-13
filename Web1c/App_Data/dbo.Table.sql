CREATE TABLE [dbo].[Table] (
    [Id]             INT          NOT NULL,
    [Web_Account]  VARCHAR (50) NOT NULL,
    [Web_Password] VARCHAR (50) NOT NULL,
    [Web_Points] DECIMAL NULL, 
    [Web_Email] VARCHAR(50) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

