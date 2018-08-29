CREATE TABLE [dbo].[ph_al] (
    [ph_id] VARCHAR (255) NOT NULL,
    [al_id] VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_ph_al] PRIMARY KEY CLUSTERED ([ph_id] ASC, [al_id] ASC),
    CONSTRAINT [FK_ph_al_ph] FOREIGN KEY ([al_id]) REFERENCES [dbo].[al] ([id]),
    CONSTRAINT [FK_ph_al_ph1] FOREIGN KEY ([ph_id]) REFERENCES [dbo].[ph] ([id])
);

