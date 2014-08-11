CREATE TABLE [q].[QuoteDetail] (
    [QuoteID] BIGINT     NOT NULL,
    [Type]    TINYINT    NOT NULL,
    [Time]    TIME (0)   NOT NULL,
    [Open]    FLOAT (53) NOT NULL,
    [High]    FLOAT (53) NOT NULL,
    [Low]     FLOAT (53) NOT NULL,
    [Close]   FLOAT (53) NOT NULL,
    [Volume]  BIGINT     NOT NULL,
    CONSTRAINT [PK_QuoteDetail] PRIMARY KEY CLUSTERED ([QuoteID] ASC, [Type] ASC, [Time] ASC) WITH (DATA_COMPRESSION = PAGE) ON [QuoteData],
    CONSTRAINT [FK_QuoteDetail_Quote] FOREIGN KEY ([QuoteID]) REFERENCES [q].[Quote] ([QuoteID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1,5,10,15,30,60', @level0type = N'SCHEMA', @level0name = N'q', @level1type = N'TABLE', @level1name = N'QuoteDetail', @level2type = N'COLUMN', @level2name = N'Type';

