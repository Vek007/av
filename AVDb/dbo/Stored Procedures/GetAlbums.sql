
CREATE PROC [dbo].[GetAlbums] AS
SELECT id, [name], [description]
FROM Albums
ORDER BY [name]