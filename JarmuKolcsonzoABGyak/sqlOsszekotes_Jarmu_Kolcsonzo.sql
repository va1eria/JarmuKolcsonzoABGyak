ALTER TABLE [Jarmu] ADD [KolcsonzoId] INT NOT NULL;

ALTER TABLE [Jarmu] ADD CONSTRAINT [Jarmu_Kolcsonzo_FK] FOREIGN KEY ([KolcsonzoId]) REFERENCES [Kolcsonzo]([Id]);