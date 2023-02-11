
IF NOT EXISTS( SELECT * FROM BookCategory)
BEGIN
INSERT INTO BookCategory
VALUES
('Fantasy Fiction'),
('Spirituality'),
('Fiction'),
('Science Fiction')
END

IF NOT EXISTS (SELECT * FROM Publisher)
BEGIN
INSERT INTO Publisher
VALUES
('HarperCollins'),
('New World Library'),
('Oneworld Publications')
END

IF NOT EXISTS (SELECT * FROM Author)
BEGIN
INSERT INTO Author
VALUES
('Paulo Coelho'),
('Eckhart Tolle'),
('Amie Kaufman'),
('Jay Kristoff')
END

IF NOT EXISTS (SELECT * FROM AuthorContact)
BEGIN
INSERT INTO AuthorContact
VALUES
(1, '111-222-3333', '133 salas 601 / 602, Rio de Janeiro 22070-010. BRAZIL'),
(2, '444-555-6666', '933 Seymour St, Vancouver, BC V6B 6L6, Canada'),
(3, '777-888-9999', 'Mentone 3194. Victoria. AUSTRALIA'),
(4, '222-333-4444', '234 Collins Street, Melbourne, VIC, AUSTRALIA')
END

IF NOT EXISTS (SELECT * FROM Book)
BEGIN
INSERT INTO Book
VALUES
('The Alchemist', 1, 1),
('The Power of Now', 2, 2),
('Eleven Minutes', 3, 1),
('Illuminae', 4, 3)
END

IF NOT EXISTS (SELECT * FROM BookAuthors)
BEGIN
INSERT INTO BookAuthors
VALUES
(1,1),
(2,2),
(3,1),
(4,3),
(4,4)
END