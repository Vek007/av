CREATE PROC [dbo].[DeleteAlbum]
 @id INT
AS

DELETE FROM Albums
WHERE id = @id