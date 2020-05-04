CREATE TABLE [dbo].[Table]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [nombre] VARCHAR(100) NOT NULL, 
    [distribuidor] VARCHAR(100) NOT NULL, 
    [precio] DECIMAL(10, 2) NOT NULL
)
