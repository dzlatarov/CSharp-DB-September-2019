SELECT COUNT(*) - COUNT(mc.MountainId)
FROM Countries c
LEFT JOIN MountainsCountries mc
	ON c.CountryCode = mc.CountryCode