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
CREATE TABLE [Customer] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [Address] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY ([Id])
);

CREATE TABLE [Product] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Price] money NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY ([Id])
);

CREATE TABLE [Store] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Address] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Store] PRIMARY KEY ([Id])
);

CREATE TABLE [Sales] (
    [Id] int NOT NULL IDENTITY,
    [ProductId] int NULL,
    [CustomerId] int NULL,
    [StoreId] int NULL,
    [DateSold] datetime NOT NULL,
    CONSTRAINT [PK_Sales] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Sales_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer] ([Id]),
    CONSTRAINT [FK_Sales_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product] ([Id]),
    CONSTRAINT [FK_Sales_Store] FOREIGN KEY ([StoreId]) REFERENCES [Store] ([Id])
);

CREATE INDEX [IX_Sales_CustomerId] ON [Sales] ([CustomerId]);

CREATE INDEX [IX_Sales_ProductId] ON [Sales] ([ProductId]);

CREATE INDEX [IX_Sales_StoreId] ON [Sales] ([StoreId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251120014155_InitialMigration', N'9.0.10');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251123060713_SchemaUpdateNov2025', N'9.0.10');

COMMIT;
GO

