DECLARE @StartDate DATE = GETDATE();

INSERT INTO SearchResults (SearchQuery, Url, Positions, SearchDate)
VALUES
    ('land-registry-searches', 'https://www.infotrack.co.uk/', '1,3,42,65', DATEADD(DAY, -1, @StartDate)),
    ('land-registry-searches', 'https://www.infotrack.co.uk/', '2,4,33', DATEADD(DAY, -2, @StartDate)),
    ('land-registry-searches', 'https://www.infotrack.co.uk/', '3,5,40', DATEADD(DAY, -3, @StartDate)),
    ('land-registry-searches', 'https://www.infotrack.co.uk/', '4,6,49', DATEADD(DAY, -4, @StartDate)),
    ('land-registry-searches', 'https://www.infotrack.co.uk/', '5,7,60', DATEADD(DAY, -5, @StartDate)),
    ('land-registry-searches', 'https://www.infotrack.co.uk/', '6,8,72', DATEADD(DAY, -6, @StartDate)),
    ('land-registry-searches', 'https://www.infotrack.co.uk/', '7,9', DATEADD(DAY, -7, @StartDate)),
    ('land-registry-searches', 'https://www.infotrack.co.uk/', '8,10', DATEADD(DAY, -8, @StartDate)),
    ('land-registry-searches', 'https://www.infotrack.co.uk/', '9,11', DATEADD(DAY, -9, @StartDate)),
    ('land-registry-searches', 'https://www.infotrack.co.uk/', '10,12', DATEADD(DAY, -10, @StartDate));