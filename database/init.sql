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
    Password NVARCHAR(255) NOT NULL
);

-- BOOKS
CREATE TABLE Books (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Title NVARCHAR(150) NOT NULL,
    FirstPublishYear DATETIME2 NOT NULL,
    CoverUrl NVARCHAR(255),
    BookExternalId NVARCHAR(100) NOT NULL UNIQUE
);

-- AUTHORS
CREATE TABLE Authors (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL
);

-- ===============================
-- JOIN TABLES
-- ===============================

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
-- SEED DATA
-- ===============================

DECLARE @UserId UNIQUEIDENTIFIER = NEWID();
DECLARE @BookId UNIQUEIDENTIFIER = NEWID();
DECLARE @AuthorId UNIQUEIDENTIFIER = NEWID();

-- USER
INSERT INTO Users (Id, FirstName, LastName, UserName, Password)
VALUES (
    @UserId,
    'Test',
    'User',
    'testuser',
    '$2a$11$XQ4q5mQy6nK2xJYzqP0xCO1ZbZ2W8yW5MZJZP8n4u1xkZPZ5J0OqS' -- bcrypt dummy
);

-- AUTHOR
INSERT INTO Authors (Id, Name)
VALUES (
    @AuthorId,
    'George Orwell'
);

-- BOOK
INSERT INTO Books (Id, Title, FirstPublishYear, CoverUrl, BookExternalId)
VALUES (
    @BookId,
    '1984',
    '1949-01-01',
    NULL,
    'OL123456W'
);

-- RELATIONS
INSERT INTO BookAuthors (BookId, AuthorId)
VALUES (@BookId, @AuthorId);

INSERT INTO BookUsers (BookId, UserId)
VALUES (@BookId, @UserId);
