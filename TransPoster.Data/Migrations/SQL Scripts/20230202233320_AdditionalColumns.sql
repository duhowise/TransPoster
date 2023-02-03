BEGIN TRANSACTION;
GO

ALTER TABLE [AspNetUserRoles] ADD [ApplicationUserId] nvarchar(450) NULL;
GO

CREATE INDEX [IX_AspNetUserRoles_ApplicationUserId] ON [AspNetUserRoles] ([ApplicationUserId]);
GO

ALTER TABLE [AspNetUserRoles] ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230202233320_AdditionalColumns', N'7.0.2');
GO

COMMIT;
GO

