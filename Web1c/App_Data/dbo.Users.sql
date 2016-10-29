CREATE TABLE [dbo].[Users] (
    [Id]           INT          NOT NULL IDENTITY,
    [User_Account]  VARCHAR (50) NOT NULL,
    [User_Password] VARCHAR (50) NOT NULL,
    [User_Points]   DECIMAL (18) DEFAULT ((0)) NULL,
    [User_Email]    VARCHAR (50) NULL, 
    CONSTRAINT [PK_Table] PRIMARY KEY ([Id])
);

