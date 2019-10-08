SELECT c.CountryCode,
	   COUNT(m.MountainRange) AS [MountainRanges]
FROM Countries c
INNER JOIN MountainsCountries mc
	ON c.CountryCode = mc.CountryCode
INNER JOIN Mountains m
	ON mc.MountainId = m.Id
WHERE c.CountryCode IN ('US', 'RU', 'BG')
GROUP BY c.CountryCode