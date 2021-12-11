CREATE TABLE [dbo].[Auto]
(
	[Rendszam] VARCHAR(7) NOT NULL PRIMARY KEY,
	[AutoTipusa] INT NOT NULL,
	[SzallithatoSzemSzam] TINYINT NOT NULL,
	CONSTRAINT [Auto_Jarmu_FK] FOREIGN KEY ([Rendszam]) REFERENCES [Jarmu]([Rendszam])
)
