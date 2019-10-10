CREATE PROC usp_GetTownsStartingWith (@stringParameter NVARCHAR(20))
AS
SELECT t.Name AS [Town]
FROM Towns t
WHERE LEFT(t.Name, LEN(@stringParameter)) = @stringParameter