
CREATE PROCEDURE [dbo].[InsertPhoto]
	@name AS NVARCHAR(50),
	@desc AS NVARCHAR(200) = null,
	@photo AS VARBINARY(MAX),
	@albumId AS INT
 AS

INSERT INTO Photos ([name], [description], photo, album_id) 
VALUES (@name, @desc, @photo, @albumId) 

RETURN SCOPE_IDENTITY()