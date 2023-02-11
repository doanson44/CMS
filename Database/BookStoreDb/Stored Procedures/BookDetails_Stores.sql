CREATE PROCEDURE GetBookDetails @BookId bigint
AS
SELECT b.Id, b.Title, b.CategoryId, c.NAME AS CategoryName, r.Id as ReviewId, r.ReviewText FROM Book b 
			  LEFT JOIN BookCategory c ON b.CategoryId = c.Id
			  LEFT JOIN Review r ON r.BookId = b.Id
			  WHERE b.Id = @BookId
GO