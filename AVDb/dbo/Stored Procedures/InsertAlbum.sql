
CREATE PROCEDURE [dbo].[InsertAlbum]
	@name AS NVARCHAR(20),
	@desc NVARCHAR(200)
 AS

INSERT INTO Albums ([name], [description]) 
VALUES (@name, @desc) 

RETURN SCOPE_IDENTITY()