--run this create command first then comment it 
CREATE DATABASE Core ;
USE [Core];

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

CREATE TABLE [DecisionTypes] (
    [Id] int NOT NULL IDENTITY,
    [TypeName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_DecisionTypes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Departments] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Roles] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [DeparmentDecision] (
    [Id] int NOT NULL IDENTITY,
    [DepartmentId] int NOT NULL,
    [DecisionTypeId] int NOT NULL,
    [Label] nvarchar(max) NULL,
    [PlaceHolder] nvarchar(max) NULL,
    [Options] nvarchar(max) NULL,
    CONSTRAINT [PK_DeparmentDecision] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DeparmentDecision_DecisionTypes_DecisionTypeId] FOREIGN KEY ([DecisionTypeId]) REFERENCES [DecisionTypes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_DeparmentDecision_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(50) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [FullName] nvarchar(100) NOT NULL,
    [ContactNumber] nvarchar(20) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [DepartmentId] int NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id])
);
GO

CREATE TABLE [RequestRedirect] (
    [Id] int NOT NULL IDENTITY,
    [RequestId] int NOT NULL,
    [DepartmentId] int NOT NULL,
    [CheckoutByUserId] int NULL,
    [CheckoutDate] datetime2 NULL,
    [Status] nvarchar(max) NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] int NOT NULL,
    [UpdatedOn] datetime2 NULL,
    [UpdatedBy] int NULL,
    [CreatedById] int NOT NULL,
    CONSTRAINT [PK_RequestRedirect] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RequestRedirect_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_RequestRedirect_Users_CheckoutByUserId] FOREIGN KEY ([CheckoutByUserId]) REFERENCES [Users] ([Id]),
    CONSTRAINT [FK_RequestRedirect_Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_RequestRedirect_Users_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users] ([Id])
);
GO

CREATE TABLE [Requests] (
    [Id] int NOT NULL IDENTITY,
    [Subject] nvarchar(max) NOT NULL,
    [Originator] nvarchar(max) NOT NULL,
    [ReferenceNumber] nvarchar(450) NOT NULL,
    [Establishment] nvarchar(max) NOT NULL,
    [Applicability] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [TaskType] nvarchar(max) NULL,
    [Effect] nvarchar(max) NULL,
    [Effect2] nvarchar(max) NULL,
    [AttachmentUrl] nvarchar(max) NULL,
    [Status] nvarchar(max) NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] int NOT NULL,
    [UpdateOn] datetime2 NULL,
    [UpdatedBy] int NULL,
    CONSTRAINT [PK_Requests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Requests_Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Requests_Users_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [UserRoles] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [RoleId] int NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [RequestRedirectResult] (
    [Id] int NOT NULL IDENTITY,
    [RequestId] int NOT NULL,
    [DepartmentDecisionId] int NOT NULL,
    [DataValue] nvarchar(max) NULL,
    CONSTRAINT [PK_RequestRedirectResult] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RequestRedirectResult_DeparmentDecision_DepartmentDecisionId] FOREIGN KEY ([DepartmentDecisionId]) REFERENCES [DeparmentDecision] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_RequestRedirectResult_Requests_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [Requests] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'TypeName') AND [object_id] = OBJECT_ID(N'[DecisionTypes]'))
    SET IDENTITY_INSERT [DecisionTypes] ON;
INSERT INTO [DecisionTypes] ([Id], [TypeName])
VALUES (1, N'Checkbox'),
(2, N'Textarea'),
(3, N'Dropdown'),
(4, N'Radio');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'TypeName') AND [object_id] = OBJECT_ID(N'[DecisionTypes]'))
    SET IDENTITY_INSERT [DecisionTypes] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Departments]'))
    SET IDENTITY_INSERT [Departments] ON;
INSERT INTO [Departments] ([Id], [Name])
VALUES (1, N'PG'),
(2, N'FO'),
(3, N'PE'),
(4, N'HE'),
(5, N'HQ');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Departments]'))
    SET IDENTITY_INSERT [Departments] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] ON;
INSERT INTO [Roles] ([Id], [Name])
VALUES (1, N'Admin'),
(2, N'Employee');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ContactNumber', N'DepartmentId', N'Email', N'FullName', N'IsActive', N'Password', N'Username') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] ON;
INSERT INTO [Users] ([Id], [ContactNumber], [DepartmentId], [Email], [FullName], [IsActive], [Password], [Username])
VALUES (1, N'79400399', NULL, N'Abdelrahmansaeed1989@gmail.com', N'Administrator', CAST(1 AS bit), N'pbc', N'Admin');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ContactNumber', N'DepartmentId', N'Email', N'FullName', N'IsActive', N'Password', N'Username') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DecisionTypeId', N'DepartmentId', N'Label', N'Options', N'PlaceHolder') AND [object_id] = OBJECT_ID(N'[DeparmentDecision]'))
    SET IDENTITY_INSERT [DeparmentDecision] ON;
INSERT INTO [DeparmentDecision] ([Id], [DecisionTypeId], [DepartmentId], [Label], [Options], [PlaceHolder])
VALUES (1, 1, 1, N'Accept', NULL, N'Accept'),
(2, 2, 1, N'Comment', NULL, N'Comment'),
(3, 2, 2, N'Comment', NULL, N'Comment'),
(4, 2, 3, N'Comment', NULL, N'Comment'),
(5, 3, 3, N'Select', N'A,M,N', N'Select'),
(6, 4, 4, N'Approval', N'Yes,No', NULL),
(7, 2, 4, N'Comment', NULL, N'Comment'),
(8, 2, 5, N'Comment', NULL, N'Comment'),
(9, 2, 5, N'Comment', NULL, N'Comment'),
(10, 4, 5, N'Approval', N'Yes,No', NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DecisionTypeId', N'DepartmentId', N'Label', N'Options', N'PlaceHolder') AND [object_id] = OBJECT_ID(N'[DeparmentDecision]'))
    SET IDENTITY_INSERT [DeparmentDecision] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[UserRoles]'))
    SET IDENTITY_INSERT [UserRoles] ON;
INSERT INTO [UserRoles] ([Id], [RoleId], [UserId])
VALUES (1, 1, 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[UserRoles]'))
    SET IDENTITY_INSERT [UserRoles] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ContactNumber', N'DepartmentId', N'Email', N'FullName', N'IsActive', N'Password', N'Username') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] ON;
INSERT INTO [Users] ([Id], [ContactNumber], [DepartmentId], [Email], [FullName], [IsActive], [Password], [Username])
VALUES (2, N'79400399', 1, N'Omar@gmail.com', N'Omar', CAST(1 AS bit), N'pbc', N'Omar'),
(3, N'79400399', 2, N'Muhamad@gmail.com', N'Muhamad', CAST(1 AS bit), N'pbc', N'Muhamad'),
(4, N'79400399', 3, N'Hassan@gmail.com', N'Hassan', CAST(1 AS bit), N'pbc', N'Hassan'),
(5, N'79400399', 4, N'Yehia@gmail.com', N'Yehia', CAST(1 AS bit), N'pbc', N'Yehia'),
(6, N'79400399', 5, N'Imran@gmail.com', N'Imran', CAST(1 AS bit), N'pbc', N'Imran');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ContactNumber', N'DepartmentId', N'Email', N'FullName', N'IsActive', N'Password', N'Username') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[UserRoles]'))
    SET IDENTITY_INSERT [UserRoles] ON;
INSERT INTO [UserRoles] ([Id], [RoleId], [UserId])
VALUES (2, 2, 2),
(3, 2, 3),
(4, 2, 4),
(5, 2, 5),
(6, 2, 6);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[UserRoles]'))
    SET IDENTITY_INSERT [UserRoles] OFF;
GO

CREATE INDEX [IX_DeparmentDecision_DecisionTypeId] ON [DeparmentDecision] ([DecisionTypeId]);
GO

CREATE INDEX [IX_DeparmentDecision_DepartmentId] ON [DeparmentDecision] ([DepartmentId]);
GO

CREATE INDEX [IX_RequestRedirect_CheckoutByUserId] ON [RequestRedirect] ([CheckoutByUserId]);
GO

CREATE INDEX [IX_RequestRedirect_CreatedBy] ON [RequestRedirect] ([CreatedBy]);
GO

CREATE INDEX [IX_RequestRedirect_DepartmentId] ON [RequestRedirect] ([DepartmentId]);
GO

CREATE INDEX [IX_RequestRedirect_UpdatedBy] ON [RequestRedirect] ([UpdatedBy]);
GO

CREATE INDEX [IX_RequestRedirectResult_DepartmentDecisionId] ON [RequestRedirectResult] ([DepartmentDecisionId]);
GO

CREATE INDEX [IX_RequestRedirectResult_RequestId] ON [RequestRedirectResult] ([RequestId]);
GO

CREATE INDEX [IX_Requests_CreatedBy] ON [Requests] ([CreatedBy]);
GO

CREATE UNIQUE INDEX [IX_Requests_ReferenceNumber] ON [Requests] ([ReferenceNumber]);
GO

CREATE INDEX [IX_Requests_UpdatedBy] ON [Requests] ([UpdatedBy]);
GO

CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);
GO

CREATE INDEX [IX_UserRoles_UserId] ON [UserRoles] ([UserId]);
GO

CREATE INDEX [IX_Users_DepartmentId] ON [Users] ([DepartmentId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240329213428_InitialCreate', N'8.0.3');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Departments] ADD [ParentId] int NULL;
GO

UPDATE [Departments] SET [ParentId] = NULL
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

UPDATE [Departments] SET [ParentId] = NULL
WHERE [Id] = 2;
SELECT @@ROWCOUNT;

GO

UPDATE [Departments] SET [ParentId] = NULL
WHERE [Id] = 3;
SELECT @@ROWCOUNT;

GO

UPDATE [Departments] SET [ParentId] = NULL
WHERE [Id] = 4;
SELECT @@ROWCOUNT;

GO

UPDATE [Departments] SET [ParentId] = NULL
WHERE [Id] = 5;
SELECT @@ROWCOUNT;

GO

CREATE INDEX [IX_Departments_ParentId] ON [Departments] ([ParentId]);
GO

ALTER TABLE [Departments] ADD CONSTRAINT [FK_Departments_Departments_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Departments] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240329223426_InitialUpdate', N'8.0.3');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Departments] ADD [Shortcut] nvarchar(max) NULL;
GO

UPDATE [Departments] SET [Shortcut] = NULL
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

UPDATE [Departments] SET [Shortcut] = NULL
WHERE [Id] = 2;
SELECT @@ROWCOUNT;

GO

UPDATE [Departments] SET [Shortcut] = NULL
WHERE [Id] = 3;
SELECT @@ROWCOUNT;

GO

UPDATE [Departments] SET [Shortcut] = NULL
WHERE [Id] = 4;
SELECT @@ROWCOUNT;

GO

UPDATE [Departments] SET [Shortcut] = NULL
WHERE [Id] = 5;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240329225512_add-shortcut-department', N'8.0.3');
GO

COMMIT;
GO

