CREATE TABLE [dbo].[al] (
    [id]          VARCHAR (255)  NOT NULL,
    [name]        NVARCHAR (50)  NOT NULL,
    [description] NVARCHAR (200) NULL,
    [r]           INT            CONSTRAINT [DF_al_r] DEFAULT ((100)) NULL,
    [g]           INT            CONSTRAINT [DF_al_g] DEFAULT ((100)) NULL,
    [b]           INT            CONSTRAINT [DF_al_b] DEFAULT ((100)) NULL,
    CONSTRAINT [PK_Albums] PRIMARY KEY CLUSTERED ([id] ASC)
);

