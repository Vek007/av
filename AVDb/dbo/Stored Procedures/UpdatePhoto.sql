create proc [dbo].[UpdatePhoto]
	@id INT,
	@name AS NVARCHAR(20),
	@desc NVARCHAR(200)

AS

UPDATE Photos
SET name = @name, [description] = @desc
WHERE id = @id