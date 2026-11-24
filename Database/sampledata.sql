/* =====================================================================
   COMP1551 Application Development - Coursework 2026/27
   Music Festival Operations System
   ---------------------------------------------------------------------
   sampledata.sql
   Inserts sample data. Run AFTER databasescheme.sql.

   Minimum required by the brief (this script inserts more):
     * 2 performers (each with 2 genres)   -> 4 performers inserted
     * 2 crew                              -> 4 crew inserted
     * 2 vendors (each with 3 categories)  -> 3 vendors inserted

   Explicit PersonId values are used (IDENTITY_INSERT) so that the IDs in
   the demo video and the report are predictable.
   All names, emails and phone numbers are fictitious.
   ===================================================================== */

USE MusicFestivalDB;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;   -- any error rolls back the whole transaction
BEGIN TRANSACTION;

/* ---------------------------------------------------------------------
   1. Lookup data: music genres and product categories
   --------------------------------------------------------------------- */
SET IDENTITY_INSERT Genres ON;
INSERT INTO Genres (GenreId, GenreName) VALUES
    (1, N'Rock'), (2, N'Pop'), (3, N'Jazz'), (4, N'Electronic'),
    (5, N'Hip Hop'), (6, N'Indie'), (7, N'Folk'), (8, N'House');
SET IDENTITY_INSERT Genres OFF;

SET IDENTITY_INSERT ProductCategories ON;
INSERT INTO ProductCategories (CategoryId, CategoryName) VALUES
    (1, N'Street Food'), (2, N'Drinks'), (3, N'Desserts'),
    (4, N'T-Shirts'),    (5, N'Posters'), (6, N'Accessories'),
    (7, N'Vegan Food'),  (8, N'Vinyl Records');
SET IDENTITY_INSERT ProductCategories OFF;

/* ---------------------------------------------------------------------
   2. People - common fields (RoleId: 1 Performer, 2 Crew, 3 Vendor)
   --------------------------------------------------------------------- */
SET IDENTITY_INSERT People ON;
INSERT INTO People (PersonId, FullName, Telephone, Email, RoleId) VALUES
    -- Performers
    (1,  N'Luna Rivers',       '07700900101', 'luna.rivers@festival.test',      1),
    (2,  N'DJ Kai Nguyen',     '07700900102', 'kai.nguyen@festival.test',       1),
    (3,  N'The Midnight Owls', '07700900103', 'midnight.owls@festival.test',    1),
    (4,  N'Mai Anh Tran',      '07700900104', 'maianh.tran@festival.test',      1),
    -- Crew
    (5,  N'Oliver Bennett',    '07700900201', 'oliver.bennett@festival.test',   2),
    (6,  N'Sophie Clarke',     '07700900202', 'sophie.clarke@festival.test',    2),
    (7,  N'Minh Duc Pham',     '07700900203', 'minhduc.pham@festival.test',     2),
    (8,  N'Grace Owusu',       '07700900204', 'grace.owusu@festival.test',      2),
    -- Vendors
    (9,  N'Hannah Lee',        '07700900301', 'hannah.lee@festival.test',       3),
    (10, N'Tom Walker',        '07700900302', 'tom.walker@festival.test',       3),
    (11, N'Linh Vo',           '07700900303', 'linh.vo@festival.test',          3);
SET IDENTITY_INSERT People OFF;

/* ---------------------------------------------------------------------
   3. Performers + exactly 2 genres each (business rule)
   --------------------------------------------------------------------- */
INSERT INTO Performers (PersonId, Fee) VALUES
    (1, 12000.00),
    (2,  8500.00),
    (3, 15000.00),
    (4,  6000.00);

INSERT INTO PerformerGenres (PersonId, GenreId) VALUES
    (1, 2), (1, 6),     -- Luna Rivers       : Pop, Indie
    (2, 4), (2, 8),     -- DJ Kai Nguyen     : Electronic, House
    (3, 1), (3, 6),     -- The Midnight Owls : Rock, Indie
    (4, 3), (4, 7);     -- Mai Anh Tran      : Jazz, Folk

/* ---------------------------------------------------------------------
   4. Crew - FullTime weekly hours 25-40, PartTime 1-24
   --------------------------------------------------------------------- */
INSERT INTO CrewMembers (PersonId, HourlyRate, EmploymentType, WeeklyHours) VALUES
    (5, 18.50, 'FullTime', 40),   -- stage manager
    (6, 16.00, 'FullTime', 30),   -- sound engineer
    (7, 12.75, 'PartTime', 20),   -- lighting technician
    (8, 11.50, 'PartTime', 12);   -- security volunteer

/* ---------------------------------------------------------------------
   5. Vendors + exactly 3 product categories each (business rule)
   --------------------------------------------------------------------- */
INSERT INTO Vendors (PersonId, StallName) VALUES
    (9,  N'Seoul Street Kitchen'),
    (10, N'Festival Merch Co.'),
    (11, N'Green Bowl Vegan Bar');

INSERT INTO VendorCategories (PersonId, CategoryId) VALUES
    (9,  1), (9,  2), (9,  3),    -- Street Food, Drinks, Desserts
    (10, 4), (10, 5), (10, 6),    -- T-Shirts, Posters, Accessories
    (11, 7), (11, 2), (11, 3);    -- Vegan Food, Drinks, Desserts

COMMIT TRANSACTION;
GO

/* ---------------------------------------------------------------------
   6. Quick verification queries
   --------------------------------------------------------------------- */
SELECT r.RoleName, COUNT(*) AS People
FROM People p
JOIN Roles r ON r.RoleId = p.RoleId
GROUP BY r.RoleName;

SELECT p.PersonId, p.FullName, pf.Fee,
       STRING_AGG(g.GenreName, ', ') AS Genres
FROM People p
JOIN Performers pf      ON pf.PersonId = p.PersonId
JOIN PerformerGenres pg ON pg.PersonId = p.PersonId
JOIN Genres g           ON g.GenreId   = pg.GenreId
GROUP BY p.PersonId, p.FullName, pf.Fee;

SELECT p.PersonId, p.FullName, v.StallName,
       STRING_AGG(c.CategoryName, ', ') AS Categories
FROM People p
JOIN Vendors v           ON v.PersonId   = p.PersonId
JOIN VendorCategories vc ON vc.PersonId  = p.PersonId
JOIN ProductCategories c ON c.CategoryId = vc.CategoryId
GROUP BY p.PersonId, p.FullName, v.StallName;
GO
