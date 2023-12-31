USE [BazkaMvc]
GO
SET IDENTITY_INSERT [dbo].[AspNetUsers] ON 
DELETE FROM [dbo].[AspNetUsers];
INSERT [dbo].[AspNetUsers] ([Id], [IsVerified], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [CreatedAt]) VALUES (2, 0, N'user@example.com', N'USER@EXAMPLE.COM', N'user@example.com', N'USER@EXAMPLE.COM', 0, N'AQAAAAIAAYagAAAAEKVVxdhjxsnnh3JHMpE8EGyLVxOweT3LLtbsQcUGcHe8WkQO8sB0bqkRnfcQR7OGwQ==', N'CAL6XVT4OTRB4FSFXJN3K2EQQQH43K4Z', N'4b224513-d46b-416d-8fe5-fc3f0c89ff04', NULL, 0, 0, NULL, 1, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[AspNetUsers] OFF
GO
SET IDENTITY_INSERT [dbo].[Category] ON 
DELETE FROM [dbo].[Category];
INSERT [dbo].[Category] ([Id], [Name], [ParentCategoryId]) VALUES (1, N'Kryminaly', NULL)
INSERT [dbo].[Category] ([Id], [Name], [ParentCategoryId]) VALUES (2, N'Romansidla', NULL)
INSERT [dbo].[Category] ([Id], [Name], [ParentCategoryId]) VALUES (3, N'Popularno naukowe', NULL)
INSERT [dbo].[Category] ([Id], [Name], [ParentCategoryId]) VALUES (4, N'Religijne', NULL)
INSERT [dbo].[Category] ([Id], [Name], [ParentCategoryId]) VALUES (5, N'Kryminaly antyczne', 1)
INSERT [dbo].[Category] ([Id], [Name], [ParentCategoryId]) VALUES (6, N'Kryminaly wspólczesne', 1)
INSERT [dbo].[Category] ([Id], [Name], [ParentCategoryId]) VALUES (7, N'Kryminaly fajne', 1)
INSERT [dbo].[Category] ([Id], [Name], [ParentCategoryId]) VALUES (8, N'Kryminalne zagadki cesarstwa rzymskiego', 5)
SET IDENTITY_INSERT [dbo].[Category] OFF
GO

