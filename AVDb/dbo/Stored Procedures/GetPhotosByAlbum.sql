
CREATE proc [dbo].[GetPhotosByAlbum]
	@albumId INT
AS

SELECT id, [name], ISNULL([description], '') AS [description], [photo]
FROM Photos
WHERE [album_id] = @albumId