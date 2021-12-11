CREATE TABLE [dbo].[Jarmu]
(
	[Rendszam] VARCHAR(7) NOT NULL PRIMARY KEY,
	[Marka] VARCHAR(64) NOT NULL,
	[Tipus] VARCHAR(64) NOT NULL,
	[FutottKM] INT NOT NULL,
	[Kolcsonozve] BIT NOT NULL
)
