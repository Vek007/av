USE [master]
GO
/****** Object:  Database [alv]    Script Date: 2018-08-23 5:37:01 PM ******/
CREATE DATABASE [alv]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AlbumViewer', FILENAME = N'e:\SQLDB\AlbumViewer.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'AlbumViewer_log', FILENAME = N'e:\SQLDB\AlbumViewer_log.ldf' , SIZE = 4096KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [alv] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [alv].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [alv] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [alv] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [alv] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [alv] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [alv] SET ARITHABORT OFF 
GO
ALTER DATABASE [alv] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [alv] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [alv] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [alv] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [alv] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [alv] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [alv] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [alv] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [alv] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [alv] SET  DISABLE_BROKER 
GO
ALTER DATABASE [alv] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [alv] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [alv] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [alv] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [alv] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [alv] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [alv] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [alv] SET RECOVERY FULL 
GO
ALTER DATABASE [alv] SET  MULTI_USER 
GO
ALTER DATABASE [alv] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [alv] SET DB_CHAINING OFF 
GO
ALTER DATABASE [alv] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [alv] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [alv] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'alv', N'ON'
GO
ALTER DATABASE [alv] SET QUERY_STORE = OFF
GO
USE [alv]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [alv]
GO
/****** Object:  Table [dbo].[alv]    Script Date: 2018-08-23 5:37:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[alv](
	[id] [varchar](10) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[description] [nvarchar](200) NULL,
 CONSTRAINT [PK_Albums] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ph]    Script Date: 2018-08-23 5:37:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ph](
	[id] [varchar](255) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
	[description] [nvarchar](200) NULL,
	[photo] [varbinary](max) NULL,
	[path] [nvarchar](max) NULL,
	[time_stamp] [datetimeoffset](7) NULL,
 CONSTRAINT [PK_Photos] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ph_al]    Script Date: 2018-08-23 5:37:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ph_al](
	[ph_id] [varchar](20) NOT NULL,
	[al_id] [varchar](10) NOT NULL,
 CONSTRAINT [PK_ph_al] PRIMARY KEY CLUSTERED 
(
	[ph_id] ASC,
	[al_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ph_al]  WITH CHECK ADD  CONSTRAINT [FK_ph_al_ph] FOREIGN KEY([al_id])
REFERENCES [dbo].[alv] ([id])
GO
ALTER TABLE [dbo].[ph_al] CHECK CONSTRAINT [FK_ph_al_ph]
GO
/****** Object:  StoredProcedure [dbo].[DeleteAlbum]    Script Date: 2018-08-23 5:37:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[DeleteAlbum]
 @id INT
AS

DELETE FROM Albums
WHERE id = @id


GO
/****** Object:  StoredProcedure [dbo].[DeletePhoto]    Script Date: 2018-08-23 5:37:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[DeletePhoto]
 @id INT
AS

DELETE FROM Photos
WHERE id = @id
GO
/****** Object:  StoredProcedure [dbo].[GetAlbums]    Script Date: 2018-08-23 5:37:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[GetAlbums] AS
SELECT id, [name], [description]
FROM Albums
ORDER BY [name]


GO
/****** Object:  StoredProcedure [dbo].[GetPhoto]    Script Date: 2018-08-23 5:37:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPhoto] 
	@id INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [name], ISNULL([description], '') AS description, photo 
	FROM Photos 
	WHERE id = @id

END

GO
/****** Object:  StoredProcedure [dbo].[GetPhotosByAlbum]    Script Date: 2018-08-23 5:37:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[GetPhotosByAlbum]
	@albumId INT
AS

SELECT id, [name], ISNULL([description], '') AS [description], [photo]
FROM Photos
WHERE [album_id] = @albumId

GO
/****** Object:  StoredProcedure [dbo].[InsertAlbum]    Script Date: 2018-08-23 5:37:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertAlbum]
	@name AS NVARCHAR(20),
	@desc NVARCHAR(200)
 AS

INSERT INTO Albums ([name], [description]) 
VALUES (@name, @desc) 

RETURN SCOPE_IDENTITY()


GO
/****** Object:  StoredProcedure [dbo].[InsertPhoto]    Script Date: 2018-08-23 5:37:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertPhoto]
	@name AS NVARCHAR(50),
	@desc AS NVARCHAR(200) = null,
	@photo AS VARBINARY(MAX),
	@albumId AS INT
 AS

INSERT INTO Photos ([name], [description], photo, album_id) 
VALUES (@name, @desc, @photo, @albumId) 

RETURN SCOPE_IDENTITY()



GO
/****** Object:  StoredProcedure [dbo].[UpdateAlbum]    Script Date: 2018-08-23 5:37:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[UpdateAlbum]
	@id INT,
	@name AS NVARCHAR(20),
	@desc NVARCHAR(200)

AS

UPDATE Albums
SET name = @name, [description] = @desc
WHERE id = @id

GO
/****** Object:  StoredProcedure [dbo].[UpdatePhoto]    Script Date: 2018-08-23 5:37:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[UpdatePhoto]
	@id INT,
	@name AS NVARCHAR(20),
	@desc NVARCHAR(200)

AS

UPDATE Photos
SET name = @name, [description] = @desc
WHERE id = @id

GO
USE [master]
GO
ALTER DATABASE [alv] SET  READ_WRITE 
GO
