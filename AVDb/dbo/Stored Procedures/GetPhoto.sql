CREATE PROCEDURE [dbo].[GetPhoto] 
	@id INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [name], ISNULL([description], '') AS description, photo 
	FROM Photos 
	WHERE id = @id

END