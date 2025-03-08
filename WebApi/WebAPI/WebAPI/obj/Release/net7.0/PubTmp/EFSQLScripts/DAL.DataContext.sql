IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231005051302_addCity')
BEGIN
    CREATE TABLE [Cities] (
        [CityID] int NOT NULL IDENTITY,
        [CityName] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_Cities] PRIMARY KEY ([CityID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231005051302_addCity')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231005051302_addCity', N'7.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    ALTER TABLE [Cities] ADD [AddorUpdateAt] time NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    ALTER TABLE [Cities] ADD [AddorUpdateBy] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    ALTER TABLE [Cities] ADD [Status] float NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [Birthday] datetime2 NULL,
        [Gender] int NULL,
        [Image] nvarchar(max) NULL,
        [RefreshToken] nvarchar(max) NULL,
        [RefreshTokenExpiryTime] datetime2 NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE TABLE [CategoryProducts] (
        [CategoryID] int NOT NULL IDENTITY,
        [CategoryName] nvarchar(100) NOT NULL,
        [AddorUpdateBy] nvarchar(max) NULL,
        [AddorUpdateAt] time NULL,
        [Status] float NULL,
        CONSTRAINT [PK_CategoryProducts] PRIMARY KEY ([CategoryID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE TABLE [Districts] (
        [DistrictID] int NOT NULL IDENTITY,
        [DistrictName] nvarchar(100) NOT NULL,
        [CityID] int NOT NULL,
        [AddorUpdateBy] nvarchar(max) NULL,
        [AddorUpdateAt] time NULL,
        [Status] float NULL,
        CONSTRAINT [PK_Districts] PRIMARY KEY ([DistrictID]),
        CONSTRAINT [FK_Districts_Cities_CityID] FOREIGN KEY ([CityID]) REFERENCES [Cities] ([CityID]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE TABLE [Collections] (
        [Id] int NOT NULL IDENTITY,
        [CollectionName] nvarchar(100) NOT NULL,
        [CollectionImg] nvarchar(max) NULL,
        [CollectionDes] nvarchar(max) NULL,
        [CategoryID] int NOT NULL,
        [CityID] int NOT NULL,
        [AddorUpdateBy] nvarchar(max) NULL,
        [AddorUpdateAt] time NULL,
        [Status] float NULL,
        CONSTRAINT [PK_Collections] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Collections_CategoryProducts_CategoryID] FOREIGN KEY ([CategoryID]) REFERENCES [CategoryProducts] ([CategoryID]) ON DELETE CASCADE,
        CONSTRAINT [FK_Collections_Cities_CityID] FOREIGN KEY ([CityID]) REFERENCES [Cities] ([CityID]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE TABLE [ContentProducts] (
        [ContentID] int NOT NULL IDENTITY,
        [ContentName] nvarchar(100) NOT NULL,
        [CategoryID] int NOT NULL,
        [AddorUpdateBy] nvarchar(max) NULL,
        [AddorUpdateAt] time NULL,
        [Status] float NULL,
        CONSTRAINT [PK_ContentProducts] PRIMARY KEY ([ContentID]),
        CONSTRAINT [FK_ContentProducts_CategoryProducts_CategoryID] FOREIGN KEY ([CategoryID]) REFERENCES [CategoryProducts] ([CategoryID]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE TABLE [Wards] (
        [WardID] int NOT NULL IDENTITY,
        [WardName] nvarchar(max) NOT NULL,
        [DistrictID] int NOT NULL,
        [AddorUpdateBy] nvarchar(max) NULL,
        [AddorUpdateAt] time NULL,
        [Status] float NULL,
        CONSTRAINT [PK_Wards] PRIMARY KEY ([WardID]),
        CONSTRAINT [FK_Wards_Districts_DistrictID] FOREIGN KEY ([DistrictID]) REFERENCES [Districts] ([DistrictID]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE TABLE [Stores] (
        [StoreID] int NOT NULL IDENTITY,
        [StoreName] nvarchar(200) NOT NULL,
        [StoreImage] nvarchar(max) NULL,
        [TimeOpen] time NOT NULL,
        [TimeClose] time NOT NULL,
        [PreferentialStore] nvarchar(max) NULL,
        [StarEvaluateStore] float NOT NULL,
        [StoreAddress] nvarchar(max) NOT NULL,
        [WardID] int NOT NULL,
        [ContentID] int NOT NULL,
        [AddorUpdateBy] nvarchar(max) NULL,
        [AddorUpdateAt] time NULL,
        [Status] float NULL,
        CONSTRAINT [PK_Stores] PRIMARY KEY ([StoreID]),
        CONSTRAINT [FK_Stores_ContentProducts_ContentID] FOREIGN KEY ([ContentID]) REFERENCES [ContentProducts] ([ContentID]) ON DELETE CASCADE,
        CONSTRAINT [FK_Stores_Wards_WardID] FOREIGN KEY ([WardID]) REFERENCES [Wards] ([WardID]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE TABLE [CollectionStores] (
        [CollectionStoreID] int NOT NULL IDENTITY,
        [CollectionID] int NULL,
        [StoreID] int NULL,
        [AddorUpdateBy] nvarchar(max) NULL,
        [AddorUpdateAt] time NULL,
        [Status] float NULL,
        CONSTRAINT [PK_CollectionStores] PRIMARY KEY ([CollectionStoreID]),
        CONSTRAINT [FK_CollectionStores_Collections_CollectionID] FOREIGN KEY ([CollectionID]) REFERENCES [Collections] ([Id]),
        CONSTRAINT [FK_CollectionStores_Stores_StoreID] FOREIGN KEY ([StoreID]) REFERENCES [Stores] ([StoreID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE TABLE [ListMenus] (
        [Id] int NOT NULL IDENTITY,
        [MenuName] nvarchar(100) NOT NULL,
        [StoreID] int NOT NULL,
        [AddorUpdateBy] nvarchar(max) NULL,
        [AddorUpdateAt] time NULL,
        [Status] float NULL,
        CONSTRAINT [PK_ListMenus] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ListMenus_Stores_StoreID] FOREIGN KEY ([StoreID]) REFERENCES [Stores] ([StoreID]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE TABLE [Foods] (
        [Id] int NOT NULL IDENTITY,
        [NameFood] nvarchar(100) NOT NULL,
        [imgFood] nvarchar(max) NULL,
        [PriceFood] float NOT NULL,
        [DiscountFood] int NULL,
        [DescriptionFood] nvarchar(max) NULL,
        [MenuID] int NOT NULL,
        [AddorUpdateBy] nvarchar(max) NULL,
        [AddorUpdateAt] time NULL,
        [Status] float NULL,
        CONSTRAINT [PK_Foods] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Foods_ListMenus_MenuID] FOREIGN KEY ([MenuID]) REFERENCES [ListMenus] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
    VALUES (N''d5360ab2-83f5-4eb1-ab5e-9339e27cf150'', N''1'', N''Admin'', N''Admin''),
    (N''e0c7ead6-79a2-4c09-a0e6-9696712b3092'', N''2'', N''Customer'', N''Customer'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE INDEX [IX_Collections_CategoryID] ON [Collections] ([CategoryID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE INDEX [IX_Collections_CityID] ON [Collections] ([CityID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE INDEX [IX_CollectionStores_CollectionID] ON [CollectionStores] ([CollectionID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE INDEX [IX_CollectionStores_StoreID] ON [CollectionStores] ([StoreID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE INDEX [IX_ContentProducts_CategoryID] ON [ContentProducts] ([CategoryID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE INDEX [IX_Districts_CityID] ON [Districts] ([CityID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE INDEX [IX_Foods_MenuID] ON [Foods] ([MenuID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE INDEX [IX_ListMenus_StoreID] ON [ListMenus] ([StoreID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE INDEX [IX_Stores_ContentID] ON [Stores] ([ContentID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE INDEX [IX_Stores_WardID] ON [Stores] ([WardID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    CREATE INDEX [IX_Wards_DistrictID] ON [Wards] ([DistrictID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231019025625_addDatabase')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231019025625_addDatabase', N'7.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231101025700_addLocationUser')
BEGIN
    EXEC(N'DELETE FROM [AspNetRoles]
    WHERE [Id] = N''d5360ab2-83f5-4eb1-ab5e-9339e27cf150'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231101025700_addLocationUser')
BEGIN
    EXEC(N'DELETE FROM [AspNetRoles]
    WHERE [Id] = N''e0c7ead6-79a2-4c09-a0e6-9696712b3092'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231101025700_addLocationUser')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [Location] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231101025700_addLocationUser')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
    VALUES (N''21c71463-5a8f-4a35-91af-6a35adc00870'', N''1'', N''Admin'', N''Admin''),
    (N''4cfca213-7611-4c01-b92e-7a75c24d4033'', N''2'', N''Customer'', N''Customer'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231101025700_addLocationUser')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231101025700_addLocationUser', N'7.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231106062551_addCodeForgotPasswordUser')
BEGIN
    EXEC(N'DELETE FROM [AspNetRoles]
    WHERE [Id] = N''21c71463-5a8f-4a35-91af-6a35adc00870'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231106062551_addCodeForgotPasswordUser')
BEGIN
    EXEC(N'DELETE FROM [AspNetRoles]
    WHERE [Id] = N''4cfca213-7611-4c01-b92e-7a75c24d4033'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231106062551_addCodeForgotPasswordUser')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [CodeForgotPassword] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231106062551_addCodeForgotPasswordUser')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
    VALUES (N''43b753d7-3248-4692-bb33-8cfac7cb52db'', N''1'', N''Admin'', N''Admin''),
    (N''c415db48-b487-4f90-9252-6c6a9bad1a97'', N''2'', N''Customer'', N''Customer'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231106062551_addCodeForgotPasswordUser')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231106062551_addCodeForgotPasswordUser', N'7.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231127095258_addCart')
BEGIN
    EXEC(N'DELETE FROM [AspNetRoles]
    WHERE [Id] = N''43b753d7-3248-4692-bb33-8cfac7cb52db'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231127095258_addCart')
BEGIN
    EXEC(N'DELETE FROM [AspNetRoles]
    WHERE [Id] = N''c415db48-b487-4f90-9252-6c6a9bad1a97'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231127095258_addCart')
BEGIN
    CREATE TABLE [Carts] (
        [CartID] int NOT NULL IDENTITY,
        [StoreID] int NOT NULL,
        [CustomerId] nvarchar(450) NOT NULL,
        [DeliveryAddress] nvarchar(max) NOT NULL,
        [Phone] int NULL,
        [TimeOrder] datetime2 NULL,
        [Status] float NULL,
        CONSTRAINT [PK_Carts] PRIMARY KEY ([CartID]),
        CONSTRAINT [FK_Carts_AspNetUsers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231127095258_addCart')
BEGIN
    CREATE TABLE [DetailCarts] (
        [DetailCartID] int NOT NULL IDENTITY,
        [FoodId] int NOT NULL,
        [Quantity] int NOT NULL,
        [PriceFood] float NOT NULL,
        [CartID] int NOT NULL,
        CONSTRAINT [PK_DetailCarts] PRIMARY KEY ([DetailCartID]),
        CONSTRAINT [FK_DetailCarts_Carts_CartID] FOREIGN KEY ([CartID]) REFERENCES [Carts] ([CartID]) ON DELETE CASCADE,
        CONSTRAINT [FK_DetailCarts_Foods_FoodId] FOREIGN KEY ([FoodId]) REFERENCES [Foods] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231127095258_addCart')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
    VALUES (N''477beb2c-684e-4f0a-a340-98ff809c8a44'', N''1'', N''Admin'', N''Admin''),
    (N''90c16a8a-8295-4b8e-8b94-53c879818200'', N''2'', N''Customer'', N''Customer'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231127095258_addCart')
BEGIN
    CREATE INDEX [IX_Carts_CustomerId] ON [Carts] ([CustomerId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231127095258_addCart')
BEGIN
    CREATE INDEX [IX_DetailCarts_CartID] ON [DetailCarts] ([CartID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231127095258_addCart')
BEGIN
    CREATE INDEX [IX_DetailCarts_FoodId] ON [DetailCarts] ([FoodId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231127095258_addCart')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231127095258_addCart', N'7.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129021644_addAccountStore')
BEGIN
    EXEC(N'DELETE FROM [AspNetRoles]
    WHERE [Id] = N''477beb2c-684e-4f0a-a340-98ff809c8a44'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129021644_addAccountStore')
BEGIN
    EXEC(N'DELETE FROM [AspNetRoles]
    WHERE [Id] = N''90c16a8a-8295-4b8e-8b94-53c879818200'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129021644_addAccountStore')
BEGIN
    CREATE TABLE [AccountStores] (
        [AccountStoreID] int NOT NULL IDENTITY,
        [LoginName] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [UserName] nvarchar(max) NOT NULL,
        [StoreID] int NOT NULL,
        CONSTRAINT [PK_AccountStores] PRIMARY KEY ([AccountStoreID]),
        CONSTRAINT [FK_AccountStores_Stores_StoreID] FOREIGN KEY ([StoreID]) REFERENCES [Stores] ([StoreID]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129021644_addAccountStore')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
    VALUES (N''9f05b71f-8716-41d4-bd26-4b00dcb76dee'', N''2'', N''Customer'', N''Customer''),
    (N''c2a11630-e3a8-4aaf-a930-332ff5c7fe41'', N''1'', N''Admin'', N''Admin'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129021644_addAccountStore')
BEGIN
    CREATE INDEX [IX_AccountStores_StoreID] ON [AccountStores] ([StoreID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129021644_addAccountStore')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231129021644_addAccountStore', N'7.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129023916_editadmintime')
BEGIN
    EXEC(N'DELETE FROM [AspNetRoles]
    WHERE [Id] = N''9f05b71f-8716-41d4-bd26-4b00dcb76dee'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129023916_editadmintime')
BEGIN
    EXEC(N'DELETE FROM [AspNetRoles]
    WHERE [Id] = N''c2a11630-e3a8-4aaf-a930-332ff5c7fe41'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129023916_editadmintime')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Wards]') AND [c].[name] = N'AddorUpdateAt');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Wards] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Wards] ALTER COLUMN [AddorUpdateAt] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129023916_editadmintime')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Stores]') AND [c].[name] = N'AddorUpdateAt');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Stores] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Stores] ALTER COLUMN [AddorUpdateAt] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129023916_editadmintime')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ListMenus]') AND [c].[name] = N'AddorUpdateAt');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [ListMenus] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [ListMenus] ALTER COLUMN [AddorUpdateAt] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129023916_editadmintime')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Foods]') AND [c].[name] = N'AddorUpdateAt');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Foods] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [Foods] ALTER COLUMN [AddorUpdateAt] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129023916_editadmintime')
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Districts]') AND [c].[name] = N'AddorUpdateAt');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Districts] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [Districts] ALTER COLUMN [AddorUpdateAt] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129023916_editadmintime')
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ContentProducts]') AND [c].[name] = N'AddorUpdateAt');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [ContentProducts] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [ContentProducts] ALTER COLUMN [AddorUpdateAt] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129023916_editadmintime')
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CollectionStores]') AND [c].[name] = N'AddorUpdateAt');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [CollectionStores] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [CollectionStores] ALTER COLUMN [AddorUpdateAt] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129023916_editadmintime')
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Collections]') AND [c].[name] = N'AddorUpdateAt');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Collections] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [Collections] ALTER COLUMN [AddorUpdateAt] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129023916_editadmintime')
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Cities]') AND [c].[name] = N'AddorUpdateAt');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Cities] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [Cities] ALTER COLUMN [AddorUpdateAt] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129023916_editadmintime')
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CategoryProducts]') AND [c].[name] = N'AddorUpdateAt');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [CategoryProducts] DROP CONSTRAINT [' + @var9 + '];');
    ALTER TABLE [CategoryProducts] ALTER COLUMN [AddorUpdateAt] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129023916_editadmintime')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
    VALUES (N''32198edd-553e-4b02-8a44-ac11a9f713f1'', N''1'', N''Admin'', N''Admin''),
    (N''cb30cc9e-c0d5-45da-9f93-fc4845875640'', N''2'', N''Customer'', N''Customer'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129023916_editadmintime')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231129023916_editadmintime', N'7.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129032103_editstatus')
BEGIN
    EXEC(N'DELETE FROM [AspNetRoles]
    WHERE [Id] = N''32198edd-553e-4b02-8a44-ac11a9f713f1'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129032103_editstatus')
BEGIN
    EXEC(N'DELETE FROM [AspNetRoles]
    WHERE [Id] = N''cb30cc9e-c0d5-45da-9f93-fc4845875640'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129032103_editstatus')
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Wards]') AND [c].[name] = N'Status');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Wards] DROP CONSTRAINT [' + @var10 + '];');
    ALTER TABLE [Wards] ALTER COLUMN [Status] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129032103_editstatus')
BEGIN
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Stores]') AND [c].[name] = N'Status');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Stores] DROP CONSTRAINT [' + @var11 + '];');
    ALTER TABLE [Stores] ALTER COLUMN [Status] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129032103_editstatus')
BEGIN
    DECLARE @var12 sysname;
    SELECT @var12 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ListMenus]') AND [c].[name] = N'Status');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [ListMenus] DROP CONSTRAINT [' + @var12 + '];');
    ALTER TABLE [ListMenus] ALTER COLUMN [Status] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129032103_editstatus')
BEGIN
    DECLARE @var13 sysname;
    SELECT @var13 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Foods]') AND [c].[name] = N'Status');
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Foods] DROP CONSTRAINT [' + @var13 + '];');
    ALTER TABLE [Foods] ALTER COLUMN [Status] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129032103_editstatus')
BEGIN
    DECLARE @var14 sysname;
    SELECT @var14 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Districts]') AND [c].[name] = N'Status');
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Districts] DROP CONSTRAINT [' + @var14 + '];');
    ALTER TABLE [Districts] ALTER COLUMN [Status] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129032103_editstatus')
BEGIN
    DECLARE @var15 sysname;
    SELECT @var15 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ContentProducts]') AND [c].[name] = N'Status');
    IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [ContentProducts] DROP CONSTRAINT [' + @var15 + '];');
    ALTER TABLE [ContentProducts] ALTER COLUMN [Status] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129032103_editstatus')
BEGIN
    DECLARE @var16 sysname;
    SELECT @var16 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CollectionStores]') AND [c].[name] = N'Status');
    IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [CollectionStores] DROP CONSTRAINT [' + @var16 + '];');
    ALTER TABLE [CollectionStores] ALTER COLUMN [Status] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129032103_editstatus')
BEGIN
    DECLARE @var17 sysname;
    SELECT @var17 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Collections]') AND [c].[name] = N'Status');
    IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [Collections] DROP CONSTRAINT [' + @var17 + '];');
    ALTER TABLE [Collections] ALTER COLUMN [Status] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129032103_editstatus')
BEGIN
    DECLARE @var18 sysname;
    SELECT @var18 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Cities]') AND [c].[name] = N'Status');
    IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [Cities] DROP CONSTRAINT [' + @var18 + '];');
    ALTER TABLE [Cities] ALTER COLUMN [Status] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129032103_editstatus')
BEGIN
    DECLARE @var19 sysname;
    SELECT @var19 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CategoryProducts]') AND [c].[name] = N'Status');
    IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [CategoryProducts] DROP CONSTRAINT [' + @var19 + '];');
    ALTER TABLE [CategoryProducts] ALTER COLUMN [Status] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129032103_editstatus')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
    VALUES (N''39d1b2d5-39fc-4ae7-8093-5e3466dfaccb'', N''1'', N''Admin'', N''Admin''),
    (N''d5ae2b5b-3930-43c9-a3bb-94b8a995812d'', N''2'', N''Customer'', N''Customer'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231129032103_editstatus')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231129032103_editstatus', N'7.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231228020738_addColumnNameNoDiacritic')
BEGIN
    EXEC(N'DELETE FROM [AspNetRoles]
    WHERE [Id] = N''39d1b2d5-39fc-4ae7-8093-5e3466dfaccb'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231228020738_addColumnNameNoDiacritic')
BEGIN
    EXEC(N'DELETE FROM [AspNetRoles]
    WHERE [Id] = N''d5ae2b5b-3930-43c9-a3bb-94b8a995812d'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231228020738_addColumnNameNoDiacritic')
BEGIN
    ALTER TABLE [Stores] ADD [AddressNoDiacritic] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231228020738_addColumnNameNoDiacritic')
BEGIN
    ALTER TABLE [Stores] ADD [NameNoDiacritic] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231228020738_addColumnNameNoDiacritic')
BEGIN
    ALTER TABLE [Foods] ADD [NameNoDiacritic] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231228020738_addColumnNameNoDiacritic')
BEGIN
    ALTER TABLE [Carts] ADD [DeliveryNoDiacritic] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231228020738_addColumnNameNoDiacritic')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
    VALUES (N''9c06ecca-70f8-415f-865d-aacae076ecb2'', N''2'', N''Customer'', N''Customer''),
    (N''b740904d-6bcf-4522-b6d6-cb42e705ea04'', N''1'', N''Admin'', N''Admin'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231228020738_addColumnNameNoDiacritic')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231228020738_addColumnNameNoDiacritic', N'7.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231229081056_updateColumnEntities')
BEGIN
    EXEC(N'DELETE FROM [AspNetRoles]
    WHERE [Id] = N''9c06ecca-70f8-415f-865d-aacae076ecb2'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231229081056_updateColumnEntities')
BEGIN
    EXEC(N'DELETE FROM [AspNetRoles]
    WHERE [Id] = N''b740904d-6bcf-4522-b6d6-cb42e705ea04'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231229081056_updateColumnEntities')
BEGIN
    DECLARE @var20 sysname;
    SELECT @var20 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Stores]') AND [c].[name] = N'StoreName');
    IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [Stores] DROP CONSTRAINT [' + @var20 + '];');
    ALTER TABLE [Stores] ALTER COLUMN [StoreName] nvarchar(150) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231229081056_updateColumnEntities')
BEGIN
    DECLARE @var21 sysname;
    SELECT @var21 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Stores]') AND [c].[name] = N'StoreAddress');
    IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [Stores] DROP CONSTRAINT [' + @var21 + '];');
    ALTER TABLE [Stores] ALTER COLUMN [StoreAddress] nvarchar(150) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231229081056_updateColumnEntities')
BEGIN
    DECLARE @var22 sysname;
    SELECT @var22 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Stores]') AND [c].[name] = N'PreferentialStore');
    IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [Stores] DROP CONSTRAINT [' + @var22 + '];');
    ALTER TABLE [Stores] ALTER COLUMN [PreferentialStore] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231229081056_updateColumnEntities')
BEGIN
    DECLARE @var23 sysname;
    SELECT @var23 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Stores]') AND [c].[name] = N'NameNoDiacritic');
    IF @var23 IS NOT NULL EXEC(N'ALTER TABLE [Stores] DROP CONSTRAINT [' + @var23 + '];');
    ALTER TABLE [Stores] ALTER COLUMN [NameNoDiacritic] nvarchar(150) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231229081056_updateColumnEntities')
BEGIN
    DECLARE @var24 sysname;
    SELECT @var24 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Stores]') AND [c].[name] = N'AddressNoDiacritic');
    IF @var24 IS NOT NULL EXEC(N'ALTER TABLE [Stores] DROP CONSTRAINT [' + @var24 + '];');
    ALTER TABLE [Stores] ALTER COLUMN [AddressNoDiacritic] nvarchar(150) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231229081056_updateColumnEntities')
BEGIN
    DECLARE @var25 sysname;
    SELECT @var25 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Stores]') AND [c].[name] = N'AddorUpdateBy');
    IF @var25 IS NOT NULL EXEC(N'ALTER TABLE [Stores] DROP CONSTRAINT [' + @var25 + '];');
    ALTER TABLE [Stores] ALTER COLUMN [AddorUpdateBy] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231229081056_updateColumnEntities')
BEGIN
    DECLARE @var26 sysname;
    SELECT @var26 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ListMenus]') AND [c].[name] = N'AddorUpdateBy');
    IF @var26 IS NOT NULL EXEC(N'ALTER TABLE [ListMenus] DROP CONSTRAINT [' + @var26 + '];');
    ALTER TABLE [ListMenus] ALTER COLUMN [AddorUpdateBy] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231229081056_updateColumnEntities')
BEGIN
    DECLARE @var27 sysname;
    SELECT @var27 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Foods]') AND [c].[name] = N'NameNoDiacritic');
    IF @var27 IS NOT NULL EXEC(N'ALTER TABLE [Foods] DROP CONSTRAINT [' + @var27 + '];');
    ALTER TABLE [Foods] ALTER COLUMN [NameNoDiacritic] nvarchar(100) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231229081056_updateColumnEntities')
BEGIN
    DECLARE @var28 sysname;
    SELECT @var28 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Foods]') AND [c].[name] = N'DescriptionFood');
    IF @var28 IS NOT NULL EXEC(N'ALTER TABLE [Foods] DROP CONSTRAINT [' + @var28 + '];');
    ALTER TABLE [Foods] ALTER COLUMN [DescriptionFood] nvarchar(500) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231229081056_updateColumnEntities')
BEGIN
    DECLARE @var29 sysname;
    SELECT @var29 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Foods]') AND [c].[name] = N'AddorUpdateBy');
    IF @var29 IS NOT NULL EXEC(N'ALTER TABLE [Foods] DROP CONSTRAINT [' + @var29 + '];');
    ALTER TABLE [Foods] ALTER COLUMN [AddorUpdateBy] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231229081056_updateColumnEntities')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
    VALUES (N''342cc35b-1707-4e31-b3d6-cc503ac77388'', N''1'', N''Admin'', N''Admin''),
    (N''916792f8-6cb7-4454-88dd-362259d9a396'', N''2'', N''Customer'', N''Customer'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231229081056_updateColumnEntities')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231229081056_updateColumnEntities', N'7.0.11');
END;
GO

COMMIT;
GO

