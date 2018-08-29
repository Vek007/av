create proc [dbo].[UpdateAlbum]
	@id INT,
	@name AS NVARCHAR(20),
	@desc NVARCHAR(200)

AS

UPDATE Albums
SET name = @name, [description] = @desc
WHERE id = @id