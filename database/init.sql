-- ===============================
-- DATABASE
-- ===============================
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FavoriteLibraryDb')
BEGIN
    CREATE DATABASE FavoriteLibraryDb;
END
GO

USE FavoriteLibraryDb;
GO

-- ===============================
-- TABLES
-- ===============================

-- USERS
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    FirstName NVARCHAR(150) NOT NULL,
    LastName NVARCHAR(150) NOT NULL,
    UserName NVARCHAR(15) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    -- Campos de auditoría
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- BOOKS
CREATE TABLE Books (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Title NVARCHAR(150) NOT NULL,
    FirstPublishYear DATETIME2 NOT NULL,
    CoverUrl NVARCHAR(255),
    BookExternalId NVARCHAR(100) NOT NULL UNIQUE,
    -- Campos de auditoría
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- AUTHORS
CREATE TABLE Authors (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    -- Campos de auditoría
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- ===============================
-- JOIN TABLES
-- ===============================
-- Nota: Las tablas intermedias (N:N) generalmente no llevan CreatedAt/UpdatedAt 
-- a menos que necesites saber cuándo se creó la relación específicamente.

-- BOOK ↔ AUTHOR (N:N)
CREATE TABLE BookAuthors (
    BookId UNIQUEIDENTIFIER NOT NULL,
    AuthorId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT PK_BookAuthors PRIMARY KEY (BookId, AuthorId),
    CONSTRAINT FK_BookAuthors_Books 
        FOREIGN KEY (BookId) REFERENCES Books(Id) ON DELETE CASCADE,
    CONSTRAINT FK_BookAuthors_Authors 
        FOREIGN KEY (AuthorId) REFERENCES Authors(Id) ON DELETE CASCADE
);

-- BOOK ↔ USER (Favorites)
CREATE TABLE BookUsers (
    BookId UNIQUEIDENTIFIER NOT NULL,
    UserId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT PK_BookUsers PRIMARY KEY (BookId, UserId),
    CONSTRAINT FK_BookUsers_Books 
        FOREIGN KEY (BookId) REFERENCES Books(Id) ON DELETE CASCADE,
    CONSTRAINT FK_BookUsers_Users 
        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- ===============================
-- SEED DATA (Actualizado con fechas)
-- ===============================

DECLARE @UserId UNIQUEIDENTIFIER = NEWID();
DECLARE @BookId UNIQUEIDENTIFIER = NEWID();
DECLARE @AuthorId UNIQUEIDENTIFIER = NEWID();
DECLARE @Now DATETIME2 = SYSUTCDATETIME();

-- USER
INSERT INTO Users (Id, FirstName, LastName, UserName, Password, CreatedAt, UpdatedAt)
VALUES (
    @UserId, 'Test', 'User', 'testuser', 
    '$2a$11$XQ4q5mQy6nK2xJYzqP0xCO1ZbZ2W8yW5MZJZP8n4u1xkZPZ5J0OqS', 
    @Now, @Now
);

-- AUTHOR
INSERT INTO Authors (Id, Name, CreatedAt, UpdatedAt)
VALUES (@AuthorId, 'George Orwell', @Now, @Now);

-- BOOK
INSERT INTO Books (Id, Title, FirstPublishYear, CoverUrl, BookExternalId, CreatedAt, UpdatedAt)
VALUES (@BookId, '1984', '1949-01-01', NULL, 'OL123456W', @Now, @Now);

-- RELATIONS
INSERT INTO BookAuthors (BookId, AuthorId) VALUES (@BookId, @AuthorId);
INSERT INTO BookUsers (BookId, UserId) VALUES (@BookId, @UserId);