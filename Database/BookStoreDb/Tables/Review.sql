CREATE TABLE [dbo].[Review]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [BookId] INT NULL, 
    [ReviewText] NVARCHAR(MAX) NULL
)
