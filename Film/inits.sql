CREATE DATABASE MovieCatalogDB;
GO
USE MovieCatalogDB;
GO

CREATE TABLE Movies (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(100) NOT NULL,
    ReleaseYear INT,
    Genre NVARCHAR(50),
    Duration INT,
    Rating DECIMAL(3,1)
);
GO

INSERT INTO Movies (Title, ReleaseYear, Genre, Duration, Rating) VALUES
(N'Побег из Шоушенка', 1994, N'Драма', 142, 9.3),
(N'Крёстный отец', 1972, N'Криминал', 175, 9.2),
(N'Тёмный рыцарь', 2008, N'Боевик', 152, 9.0),
(N'Криминальное чтиво', 1994, N'Криминал', 154, 8.9),
(N'Форрест Гамп', 1994, N'Драма', 142, 8.8);
GO