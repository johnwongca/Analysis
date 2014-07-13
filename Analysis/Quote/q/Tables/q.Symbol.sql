CREATE TABLE [q].[Symbol] (
    [SymbolID]   INT           IDENTITY (1, 1) NOT NULL,
    [Exchange]   VARCHAR (10)  NOT NULL,
    [Symbol]     VARCHAR (32)  NOT NULL,
    [SymbolName] VARCHAR (128) NOT NULL,
    [LongName]   VARCHAR (128) NULL,
	[DayPriceFirstUpdate] DATETIME2(0) NULL, 
    [DayPriceLastUpdate] DATETIME2(0) NULL, 
	[MinutePriceFirstUpdate] DATETIME2(0) NULL, 
    [MinutePriceLastUpdate] DATETIME2(0) NULL, 
    CONSTRAINT [PK_q_Symbol] PRIMARY KEY CLUSTERED ([SymbolID] ASC),
    CONSTRAINT [UK_q_Exchange_Symbol] UNIQUE NONCLUSTERED ([Exchange] ASC, [Symbol] ASC)
);

