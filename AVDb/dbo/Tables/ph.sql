CREATE TABLE [dbo].[ph] (
    [id]          VARCHAR (255)      NOT NULL,
    [name]        NVARCHAR (255)     NOT NULL,
    [description] NVARCHAR (200)     NULL,
    [photo]       VARBINARY (MAX)    NULL,
    [path]        NVARCHAR (MAX)     NULL,
    [time_stamp]  DATETIMEOFFSET (7) NULL,
    [infoTags]    NVARCHAR (MAX)     NULL,
    CONSTRAINT [PK_Photos] PRIMARY KEY CLUSTERED ([id] ASC)
);



