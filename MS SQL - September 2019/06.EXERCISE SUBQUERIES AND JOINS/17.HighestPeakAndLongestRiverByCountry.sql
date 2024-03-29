SELECT TOP 5
		c.CountryName,
		MAX(p.Elevation) AS [HighestPeakElevation],
		MAX(r.Length) AS [LongestRiverLength]
FROM Countries c
FULL JOIN MountainsCountries mc
	ON c.CountryCode = mc.CountryCode
FULL JOIN Peaks p
	ON p.MountainId = mc.MountainId
FULL JOIN CountriesRivers cr
	ON cr.CountryCode = c.CountryCode
FULL JOIN Rivers r
	ON r.Id = cr.RiverId
GROUP BY c.CountryName
ORDER BY HighestPeakElevation DESC,
	     LongestRiverLength DESC,
		 c.CountryName