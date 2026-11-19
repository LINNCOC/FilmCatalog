USE MovieCatalogDB;
GO

DELETE FROM Movies WHERE Title LIKE '%(дубль)%';
GO

;WITH Dup AS (
    SELECT Id,
           ROW_NUMBER() OVER (PARTITION BY Title ORDER BY Id) AS rn
    FROM Movies
)
DELETE FROM Dup WHERE rn > 1;
GO

SELECT Title, COUNT(*) AS Cnt
FROM Movies
GROUP BY Title
HAVING COUNT(*) > 1;
GO

CREATE UNIQUE INDEX UX_Movies_Title ON Movies(Title);
GO

EXEC sp_helpindex 'Movies';
GO