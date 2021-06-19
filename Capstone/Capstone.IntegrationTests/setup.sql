-- Put steps here to set up your database in a default good state for testing
DELETE FROM category_venue;
DELETE FROM category;
DELETE FROM state;
DELETE FROM city;
DELETE FROM venue;
DELETE FROM space;
DELETE FROM reservation;

SET IDENTITY_INSERT reservation ON;
INSERT INTO reservation (reservation_id, space_id, number_of_attendees, start_date, end_date, reserved_for)
VALUES (1, 1, 50, 2021-09-08, 2021-10-14, 'Charles Family')
SET IDENTITY_INSERT reservation OFF;

SET IDENTITY_INSERT space ON;
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy)
VALUES (1, 1, 'Earth', 0, 4, 10, 1000, 100)
SET IDENTITY_INSERT space OFF;

SET IDENTITY_INSERT venue ON;
INSERT INTO venue (id, name, city_id, description)
VALUES (1, 'Milky Way', 1, 'Pale Blue Dot')
SET IDENTITY_INSERT venue OFF;

SET IDENTITY_INSERT city ON;
INSERT INTO city (id, name, state_abbreviation)
VALUES (1, 'Columbus', 'OH')
SET IDENTITY_INSERT city OFF;

SET IDENTITY_INSERT state ON;
INSERT INTO state (abbreviation, name)
VALUES ('OH', 'Ohio')
SET IDENTITY_INSERT state OFF;

SET IDENTITY_INSERT category ON;
INSERT INTO category (id, name)
VALUES (1, 'Party Planet')
SET IDENTITY_INSERT category OFF;

INSERT INTO category_venue (venue_id, category_id)
VALUES (1, 1)