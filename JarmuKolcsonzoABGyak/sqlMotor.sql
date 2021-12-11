CREATE TABLE [dbo].[Motor]
(
	[Rendszam] VARCHAR(7) NOT NULL PRIMARY KEY,
	[Hengerurtartalom] FLOAT NOT NULL,
	CONSTRAINT [Motor_Jarmu_FK] FOREIGN KEY ([Rendszam]) REFERENCES [Jarmu]([Rendszam])
)
