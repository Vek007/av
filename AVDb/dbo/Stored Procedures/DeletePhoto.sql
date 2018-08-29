
CREATE PROC [dbo].[DeletePhoto]
 @id INT
AS

DELETE FROM Photos
WHERE id = @id