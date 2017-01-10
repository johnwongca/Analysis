alter procedure DIM.Fact_BaseWrite
(
	@SymbolID int,
	@Seq int,
	@PeriodID smallint
)
as
begin
/*
	declare @a TABLE (
					Seq int,							Date datetime,							Opening float,								High float,							Low float,
					Closing float,						Volume float,							Interest float,
					DiffClosing float,					GainClosing float,						LossClosing float,
					DiffVolume float,					GainVolume float,						LossVolume float,
					WeightedClosing float,				DiffWeightedClosing float,				GainWeightedClosing float,					LossWeightedClosing float,
					TypicalClosing float,				DiffTypicalClosing float,				GainTypicalClosing float,					LossTypicalClosing float,
					VolumeAdjustedClosing float,		DiffVolumeAdjustedClosing float,		GainVolumeAdjustedClosing float,			LossVolumeAdjustedClosing float,
					VolumeAdjustedWeightedClosing float,DiffVolumeAdjustedWeightedClosing float,GainVolumeAdjustedWeightedClosing float,	LossVolumeAdjustedWeightedClosing float,
					VolumeAdjustedTypicalClosing float,	DiffVolumeAdjustedTypicalClosing float, GainVolumeAdjustedTypicalClosing float,		LossVolumeAdjustedTypicalClosing float,
					PRIMARY KEY CLUSTERED (Seq)
				) 
	insert into @a (
					Seq,							Date ,								Opening,								High,							Low,
					Closing,						Volume,								Interest,
					DiffClosing,					GainClosing,						LossClosing,
					DiffVolume,						GainVolume,							LossVolume,
					WeightedClosing,				DiffWeightedClosing,				GainWeightedClosing,					LossWeightedClosing,
					TypicalClosing,					DiffTypicalClosing,					GainTypicalClosing,						LossTypicalClosing,
					VolumeAdjustedClosing,			DiffVolumeAdjustedClosing,			GainVolumeAdjustedClosing,				LossVolumeAdjustedClosing,
					VolumeAdjustedWeightedClosing,	DiffVolumeAdjustedWeightedClosing,	GainVolumeAdjustedWeightedClosing,		LossVolumeAdjustedWeightedClosing,
					VolumeAdjustedTypicalClosing,	DiffVolumeAdjustedTypicalClosing,	GainVolumeAdjustedTypicalClosing,		LossVolumeAdjustedTypicalClosing
				)
	select
					Seq,							Date ,								Opening,								High,							Low,
					Closing,						Volume,								Interest,
					DiffClosing,					case when  DiffClosing > 0 then DiffClosing else 0 end,	case when DiffClosing < 0 then -DiffClosing else 0 end,
					DiffVolume,						case when  DiffVolume > 0 then DiffVolume else 0 end,	case when DiffVolume < 0 then -DiffVolume else 0 end,
					WeightedClosing,				DiffWeightedClosing,				case when  DiffWeightedClosing > 0 then DiffWeightedClosing else 0 end,	case when DiffWeightedClosing < 0 then -DiffWeightedClosing else 0 end,
					TypicalClosing,					DiffTypicalClosing,					case when  DiffTypicalClosing > 0 then DiffTypicalClosing else 0 end,	case when DiffTypicalClosing < 0 then -DiffTypicalClosing else 0 end,
					VolumeAdjustedClosing,			DiffVolumeAdjustedClosing,			case when  DiffTypicalClosing > 0 then DiffTypicalClosing else 0 end,	case when  DiffTypicalClosing< 0 then -DiffTypicalClosing else 0 end,
					VolumeAdjustedWeightedClosing,	DiffVolumeAdjustedWeightedClosing,	case when  DiffVolumeAdjustedWeightedClosing > 0 then DiffVolumeAdjustedWeightedClosing else 0 end,	case when DiffVolumeAdjustedWeightedClosing < 0 then -DiffVolumeAdjustedWeightedClosing else 0 end,
					VolumeAdjustedTypicalClosing,	DiffVolumeAdjustedTypicalClosing,	case when  DiffVolumeAdjustedTypicalClosing > 0 then DiffVolumeAdjustedTypicalClosing else 0 end,	case when DiffVolumeAdjustedTypicalClosing < 0 then -DiffVolumeAdjustedTypicalClosing else 0 end
	from A.Daily 
	where SymbolID = @SymbolID
		and Seq between @Seq - @PeriodID and @Seq
*/

	/*Declare Variables*/
	declare @F_Seq float, @F_Period float,  @F_One float, @F_Two float, @F_Hundred float, @F_Thousand float,
			@alpha float, @beta float, @gamma float, @delta float, @eplsilon float, @zeta float
	select	@F_Seq = @Seq, @F_Period = @PeriodID, @F_One = 1, @F_Two = 2,	@F_Hundred = 100, @F_Thousand = 1000
	
	/*Hold Last Record*/
	declare @LastDateFrom float, @LastDateTo float, @LastOpening float, @LastHigh float, @LastLow float, @LastClosing float, @LastVolume float, @LastTotalClosing float, @LastTotalWeightedClosing float, @LastTotalTypicalClosing float, @LastTotalWeightedNormalClosing float, @LastTotalWeightedWeightedClosing float, @LastTotalWeightedTypicalClosing float, @LastTotalVolume float, @LastTotalVolumeAdjustedClosing float, @LastTotalVolumeAdjustedTypicalClosing float, @LastTotalVolumeAdjustedWeightedClosing float, @LastTotalGainClosing float, @LastTotalLossClosing float, @LastTotalGainVolume float, @LastTotalLossVolume float, @LastTotalGainWeightedClosing float, @LastTotalLossWeightedClosing float, @LastTotalGainTypicalClosing float, @LastTotalLossTypicalClosing float, @LastTotalGainVolumeAdjustedClosing float, @LastTotalLossVolumeAdjustedClosing float, @LastTotalGainVolumeAdjustedWeightedClosing float, @LastTotalLossVolumeAdjustedWeightedClosing float, @LastTotalGainVolumeAdjustedTypicalClosing float, @LastTotalLossVolumeAdjustedTypicalClosing float, @LastSMAClosing float, @LastSMAWeightedClosing float, @LastSMATypicalClosing float, @LastSMAVolume float, @LastSMAGainClosing float, @LastSMALossClosing float, @LastSMAGainVolume float, @LastSMALossVolume float, @LastSMAGainWeightedClosing float, @LastSMALossWeightedClosing float, @LastSMAGainTypicalClosing float, @LastSMALossTypicalClosing float, @LastSMAGainVolumeAdjustedClosing float, @LastSMALossVolumeAdjustedClosing float, @LastSMAGainVolumeAdjustedWeightedClosing float, @LastSMALossVolumeAdjustedWeightedClosing float, @LastSMAGainVolumeAdjustedTypicalClosing float, @LastSMALossVolumeAdjustedTypicalClosing float, @LastVAMAClosing float, @LastVAMATypicalClosing float, @LastVAMAWeightedClosing float, @LastWMAClosing float, @LastWMAWeightedClosing float, @LastWMATypicalClosing float, @LastEMAClosing float, @LastEMAWeightedClosing float, @LastEMATypicalClosing float, @LastEMAVolume float, @LastEMAGainClosing float, @LastEMALossClosing float, @LastEMAGainVolume float, @LastEMALossVolume float, @LastEMAGainWeightedClosing float, @LastEMALossWeightedClosing float, @LastEMAGainTypicalClosing float, @LastEMALossTypicalClosing float, @LastEMAGainVolumeAdjustedClosing float, @LastEMALossVolumeAdjustedClosing float, @LastEMAGainVolumeAdjustedWeightedClosing float, @LastEMALossVolumeAdjustedWeightedClosing float, @LastEMAGainVolumeAdjustedTypicalClosing float, @LastEMALossVolumeAdjustedTypicalClosing float, @LastCMOClosing float, @LastCMOVolume float, @LastCMOWeightedClosing float, @LastCMOTypicalClosing float, @LastVMAClosing float, @LastVMAVolume float, @LastVMAWeightedClosing float, @LastVMATypicalClosing float, @LastWSClosing float, @LastWSWeightedClosing float, @LastWSTypicalClosing float, @LastWSWeightedNormalClosing float, @LastWSWeightedWeightedClosing float, @LastWSWeightedTypicalClosing float, @LastWSVolume float, @LastWSVolumeAdjustedClosing float, @LastWSVolumeAdjustedTypicalClosing float, @LastWSVolumeAdjustedWeightedClosing float, @LastWSGainClosing float, @LastWSLossClosing float, @LastWSGainVolume float, @LastWSLossVolume float, @LastWSGainWeightedClosing float, @LastWSLossWeightedClosing float, @LastWSGainTypicalClosing float, @LastWSLossTypicalClosing float, @LastWSGainVolumeAdjustedClosing float, @LastWSLossVolumeAdjustedClosing float, @LastWSGainVolumeAdjustedWeightedClosing float, @LastWSLossVolumeAdjustedWeightedClosing float, @LastWSGainVolumeAdjustedTypicalClosing float, @LastWSLossVolumeAdjustedTypicalClosing float, @LastRSI_SMAClosing float, @LastRSI_SMAVolume float, @LastRSI_SMAWeightedClosing float, @LastRSI_SMATypicalClosing float, @LastRSI_SMAVolumeAdjustedClosing float, @LastRSI_SMAVolumeAdjustedWeightedClosing float, @LastRSI_SMAVolumeAdjustedTypicalClosing float, @LastRSI_EMAClosing float, @LastRSI_EMAVolume float, @LastRSI_EMAWeightedClosing float, @LastRSI_EMATypicalClosing float, @LastRSI_EMAVolumeAdjustedClosing float, @LastRSI_EMAVolumeAdjustedWeightedClosing float, @LastRSI_EMAVolumeAdjustedTypicalClosing float, @LastRSI_WSClosing float, @LastRSI_WSVolume float, @LastRSI_WSWeightedClosing float, @LastRSI_WSTypicalClosing float, @LastRSI_WSVolumeAdjustedClosing float, @LastRSI_WSVolumeAdjustedWeightedClosing float, @LastRSI_WSVolumeAdjustedTypicalClosing float
	declare @LastSTDEVClosing float, @LastSTDEVWeightedClosing float, @LastSTDEVTypicalClosing float, @LastSTDEVVolume float, @LastSTDEVGainClosing float, @LastSTDEVLossClosing float, @LastSTDEVGainVolume float, @LastSTDEVLossVolume float, @LastSTDEVGainWeightedClosing float, @LastSTDEVLossWeightedClosing float, @LastSTDEVGainTypicalClosing float, @LastSTDEVLossTypicalClosing float, @LastSTDEVGainVolumeAdjustedClosing float, @LastSTDEVLossVolumeAdjustedClosing float, @LastSTDEVGainVolumeAdjustedWeightedClosing float, @LastSTDEVLossVolumeAdjustedWeightedClosing float, @LastSTDEVGainVolumeAdjustedTypicalClosing float, @LastSTDEVLossVolumeAdjustedTypicalClosing float
	declare @LastRVIClosing float, @LastRVIVolume float, @LastRVIWeightedClosing float, @LastRVITypicalClosing float, @LastRVIVolumeAdjustedClosing float, @LastRVIVolumeAdjustedWeightedClosing float, @LastRVIVolumeAdjustedTypicalClosing float
	declare @LastHighestOpening float, @LastLowestOpening float, @LastHighestHigh float, @LastLowestHigh float, @LastHighestLow float, @LastLowestLow float, @LastHighestClosing float, @LastLowestClosing float, @LastHighestVolume float, @LastLowestVolume float, @LastHighestSMAClosing float, @LastLowestSMAClosing float, @LastHighestSMAWeightedClosing float, @LastLowestSMAWeightedClosing float, @LastHighestSMATypicalClosing float, @LastLowestSMATypicalClosing float, @LastHighestSMAVolume float, @LastLowestSMAVolume float, @LastHighestVAMAClosing float, @LastLowestVAMAClosing float, @LastHighestVAMATypicalClosing float, @LastLowestVAMATypicalClosing float, @LastHighestVAMAWeightedClosing float, @LastLowestVAMAWeightedClosing float, @LastHighestWMAClosing float, @LastLowestWMAClosing float, @LastHighestWMAWeightedClosing float, @LastLowestWMAWeightedClosing float, @LastHighestWMATypicalClosing float, @LastLowestWMATypicalClosing float, @LastHighestEMAClosing float, @LastLowestEMAClosing float, @LastHighestEMAWeightedClosing float, @LastLowestEMAWeightedClosing float, @LastHighestEMATypicalClosing float, @LastLowestEMATypicalClosing float, @LastHighestEMAVolume float, @LastLowestEMAVolume float, @LastHighestCMOClosing float, @LastLowestCMOClosing float, @LastHighestCMOVolume float, @LastLowestCMOVolume float, @LastHighestCMOWeightedClosing float, @LastLowestCMOWeightedClosing float, @LastHighestCMOTypicalClosing float, @LastLowestCMOTypicalClosing float, @LastHighestVMAClosing float, @LastLowestVMAClosing float, @LastHighestVMAVolume float, @LastLowestVMAVolume float, @LastHighestVMAWeightedClosing float, @LastLowestVMAWeightedClosing float, @LastHighestVMATypicalClosing float, @LastLowestVMATypicalClosing float, @LastHighestWSClosing float, @LastLowestWSClosing float, @LastHighestWSWeightedClosing float, @LastLowestWSWeightedClosing float, @LastHighestWSTypicalClosing float, @LastLowestWSTypicalClosing float, @LastHighestWSWeightedNormalClosing float, @LastLowestWSWeightedNormalClosing float, @LastHighestWSWeightedWeightedClosing float, @LastLowestWSWeightedWeightedClosing float, @LastHighestWSWeightedTypicalClosing float, @LastLowestWSWeightedTypicalClosing float, @LastHighestWSVolume float, @LastLowestWSVolume float, @LastHighestWSVolumeAdjustedClosing float, @LastLowestWSVolumeAdjustedClosing float, @LastHighestWSVolumeAdjustedTypicalClosing float, @LastLowestWSVolumeAdjustedTypicalClosing float, @LastHighestWSVolumeAdjustedWeightedClosing float, @LastLowestWSVolumeAdjustedWeightedClosing float, @LastHighestRSI_SMAClosing float, @LastLowestRSI_SMAClosing float, @LastHighestRSI_SMAVolume float, @LastLowestRSI_SMAVolume float, @LastHighestRSI_SMAWeightedClosing float, @LastLowestRSI_SMAWeightedClosing float, @LastHighestRSI_SMATypicalClosing float, @LastLowestRSI_SMATypicalClosing float, @LastHighestRSI_SMAVolumeAdjustedClosing float, @LastLowestRSI_SMAVolumeAdjustedClosing float, @LastHighestRSI_SMAVolumeAdjustedWeightedClosing float, @LastLowestRSI_SMAVolumeAdjustedWeightedClosing float, @LastHighestRSI_SMAVolumeAdjustedTypicalClosing float, @LastLowestRSI_SMAVolumeAdjustedTypicalClosing float, @LastHighestRSI_EMAClosing float, @LastLowestRSI_EMAClosing float, @LastHighestRSI_EMAVolume float, @LastLowestRSI_EMAVolume float, @LastHighestRSI_EMAWeightedClosing float, @LastLowestRSI_EMAWeightedClosing float, @LastHighestRSI_EMATypicalClosing float, @LastLowestRSI_EMATypicalClosing float, @LastHighestRSI_EMAVolumeAdjustedClosing float, @LastLowestRSI_EMAVolumeAdjustedClosing float, @LastHighestRSI_EMAVolumeAdjustedWeightedClosing float, @LastLowestRSI_EMAVolumeAdjustedWeightedClosing float, @LastHighestRSI_EMAVolumeAdjustedTypicalClosing float, @LastLowestRSI_EMAVolumeAdjustedTypicalClosing float, @LastHighestRSI_WSClosing float, @LastLowestRSI_WSClosing float, @LastHighestRSI_WSVolume float, @LastLowestRSI_WSVolume float, @LastHighestRSI_WSWeightedClosing float, @LastLowestRSI_WSWeightedClosing float, @LastHighestRSI_WSTypicalClosing float, @LastLowestRSI_WSTypicalClosing float, @LastHighestRSI_WSVolumeAdjustedClosing float, @LastLowestRSI_WSVolumeAdjustedClosing float, @LastHighestRSI_WSVolumeAdjustedWeightedClosing float, @LastLowestRSI_WSVolumeAdjustedWeightedClosing float, @LastHighestRSI_WSVolumeAdjustedTypicalClosing float, @LastLowestRSI_WSVolumeAdjustedTypicalClosing float, @LastHighestSTDEVClosing float, @LastLowestSTDEVClosing float, @LastHighestSTDEVWeightedClosing float, @LastLowestSTDEVWeightedClosing float, @LastHighestSTDEVTypicalClosing float, @LastLowestSTDEVTypicalClosing float, @LastHighestSTDEVVolume float, @LastLowestSTDEVVolume float, @LastHighestRVIClosing float, @LastLowestRVIClosing float, @LastHighestRVIVolume float, @LastLowestRVIVolume float, @LastHighestRVIWeightedClosing float, @LastLowestRVIWeightedClosing float, @LastHighestRVITypicalClosing float, @LastLowestRVITypicalClosing float, @LastHighestRVIVolumeAdjustedClosing float, @LastLowestRVIVolumeAdjustedClosing float, @LastHighestRVIVolumeAdjustedWeightedClosing float, @LastLowestRVIVolumeAdjustedWeightedClosing float, @LastHighestRVIVolumeAdjustedTypicalClosing float, @LastLowestRVIVolumeAdjustedTypicalClosing float
	declare @LastSO_Opening float, @LastSO_High float, @LastSO_Low float, @LastSO_Closing float, @LastSO_Volume float, @LastSO_SMAClosing float, @LastSO_SMAWeightedClosing float, @LastSO_SMATypicalClosing float, @LastSO_SMAVolume float, @LastSO_VAMAClosing float, @LastSO_VAMATypicalClosing float, @LastSO_VAMAWeightedClosing float, @LastSO_WMAClosing float, @LastSO_WMAWeightedClosing float, @LastSO_WMATypicalClosing float, @LastSO_EMAClosing float, @LastSO_EMAWeightedClosing float, @LastSO_EMATypicalClosing float, @LastSO_EMAVolume float, @LastSO_CMOClosing float, @LastSO_CMOVolume float, @LastSO_CMOWeightedClosing float, @LastSO_CMOTypicalClosing float, @LastSO_VMAClosing float, @LastSO_VMAVolume float, @LastSO_VMAWeightedClosing float, @LastSO_VMATypicalClosing float, @LastSO_WSClosing float, @LastSO_WSWeightedClosing float, @LastSO_WSTypicalClosing float, @LastSO_WSWeightedNormalClosing float, @LastSO_WSWeightedWeightedClosing float, @LastSO_WSWeightedTypicalClosing float, @LastSO_WSVolume float, @LastSO_WSVolumeAdjustedClosing float, @LastSO_WSVolumeAdjustedTypicalClosing float, @LastSO_WSVolumeAdjustedWeightedClosing float, @LastSO_RSI_SMAClosing float, @LastSO_RSI_SMAVolume float, @LastSO_RSI_SMAWeightedClosing float, @LastSO_RSI_SMATypicalClosing float, @LastSO_RSI_SMAVolumeAdjustedClosing float, @LastSO_RSI_SMAVolumeAdjustedWeightedClosing float, @LastSO_RSI_SMAVolumeAdjustedTypicalClosing float, @LastSO_RSI_EMAClosing float, @LastSO_RSI_EMAVolume float, @LastSO_RSI_EMAWeightedClosing float, @LastSO_RSI_EMATypicalClosing float, @LastSO_RSI_EMAVolumeAdjustedClosing float, @LastSO_RSI_EMAVolumeAdjustedWeightedClosing float, @LastSO_RSI_EMAVolumeAdjustedTypicalClosing float, @LastSO_RSI_WSClosing float, @LastSO_RSI_WSVolume float, @LastSO_RSI_WSWeightedClosing float, @LastSO_RSI_WSTypicalClosing float, @LastSO_RSI_WSVolumeAdjustedClosing float, @LastSO_RSI_WSVolumeAdjustedWeightedClosing float, @LastSO_RSI_WSVolumeAdjustedTypicalClosing float, @LastSO_STDEVClosing float, @LastSO_STDEVWeightedClosing float, @LastSO_STDEVTypicalClosing float, @LastSO_STDEVVolume float, @LastSO_RVIClosing float, @LastSO_RVIVolume float, @LastSO_RVIWeightedClosing float, @LastSO_RVITypicalClosing float, @LastSO_RVIVolumeAdjustedClosing float, @LastSO_RVIVolumeAdjustedWeightedClosing float, @LastSO_RVIVolumeAdjustedTypicalClosing float
	declare @LastSO3_Opening float, @LastSO3_High float, @LastSO3_Low float, @LastSO3_Closing float, @LastSO3_Volume float, @LastSO3_SMAClosing float, @LastSO3_SMAWeightedClosing float, @LastSO3_SMATypicalClosing float, @LastSO3_SMAVolume float, @LastSO3_VAMAClosing float, @LastSO3_VAMATypicalClosing float, @LastSO3_VAMAWeightedClosing float, @LastSO3_WMAClosing float, @LastSO3_WMAWeightedClosing float, @LastSO3_WMATypicalClosing float, @LastSO3_EMAClosing float, @LastSO3_EMAWeightedClosing float, @LastSO3_EMATypicalClosing float, @LastSO3_EMAVolume float, @LastSO3_CMOClosing float, @LastSO3_CMOVolume float, @LastSO3_CMOWeightedClosing float, @LastSO3_CMOTypicalClosing float, @LastSO3_VMAClosing float, @LastSO3_VMAVolume float, @LastSO3_VMAWeightedClosing float, @LastSO3_VMATypicalClosing float, @LastSO3_WSClosing float, @LastSO3_WSWeightedClosing float, @LastSO3_WSTypicalClosing float, @LastSO3_WSWeightedNormalClosing float, @LastSO3_WSWeightedWeightedClosing float, @LastSO3_WSWeightedTypicalClosing float, @LastSO3_WSVolume float, @LastSO3_WSVolumeAdjustedClosing float, @LastSO3_WSVolumeAdjustedTypicalClosing float, @LastSO3_WSVolumeAdjustedWeightedClosing float, @LastSO3_RSI_SMAClosing float, @LastSO3_RSI_SMAVolume float, @LastSO3_RSI_SMAWeightedClosing float, @LastSO3_RSI_SMATypicalClosing float, @LastSO3_RSI_SMAVolumeAdjustedClosing float, @LastSO3_RSI_SMAVolumeAdjustedWeightedClosing float, @LastSO3_RSI_SMAVolumeAdjustedTypicalClosing float, @LastSO3_RSI_EMAClosing float, @LastSO3_RSI_EMAVolume float, @LastSO3_RSI_EMAWeightedClosing float, @LastSO3_RSI_EMATypicalClosing float, @LastSO3_RSI_EMAVolumeAdjustedClosing float, @LastSO3_RSI_EMAVolumeAdjustedWeightedClosing float, @LastSO3_RSI_EMAVolumeAdjustedTypicalClosing float, @LastSO3_RSI_WSClosing float, @LastSO3_RSI_WSVolume float, @LastSO3_RSI_WSWeightedClosing float, @LastSO3_RSI_WSTypicalClosing float, @LastSO3_RSI_WSVolumeAdjustedClosing float, @LastSO3_RSI_WSVolumeAdjustedWeightedClosing float, @LastSO3_RSI_WSVolumeAdjustedTypicalClosing float, @LastSO3_STDEVClosing float, @LastSO3_STDEVWeightedClosing float, @LastSO3_STDEVTypicalClosing float, @LastSO3_STDEVVolume float, @LastSO3_RVIClosing float, @LastSO3_RVIVolume float, @LastSO3_RVIWeightedClosing float, @LastSO3_RVITypicalClosing float, @LastSO3_RVIVolumeAdjustedClosing float, @LastSO3_RVIVolumeAdjustedWeightedClosing float, @LastSO3_RVIVolumeAdjustedTypicalClosing float
	declare @LastSO4_Opening float, @LastSO4_High float, @LastSO4_Low float, @LastSO4_Closing float, @LastSO4_Volume float, @LastSO4_SMAClosing float, @LastSO4_SMAWeightedClosing float, @LastSO4_SMATypicalClosing float, @LastSO4_SMAVolume float, @LastSO4_VAMAClosing float, @LastSO4_VAMATypicalClosing float, @LastSO4_VAMAWeightedClosing float, @LastSO4_WMAClosing float, @LastSO4_WMAWeightedClosing float, @LastSO4_WMATypicalClosing float, @LastSO4_EMAClosing float, @LastSO4_EMAWeightedClosing float, @LastSO4_EMATypicalClosing float, @LastSO4_EMAVolume float, @LastSO4_CMOClosing float, @LastSO4_CMOVolume float, @LastSO4_CMOWeightedClosing float, @LastSO4_CMOTypicalClosing float, @LastSO4_VMAClosing float, @LastSO4_VMAVolume float, @LastSO4_VMAWeightedClosing float, @LastSO4_VMATypicalClosing float, @LastSO4_WSClosing float, @LastSO4_WSWeightedClosing float, @LastSO4_WSTypicalClosing float, @LastSO4_WSWeightedNormalClosing float, @LastSO4_WSWeightedWeightedClosing float, @LastSO4_WSWeightedTypicalClosing float, @LastSO4_WSVolume float, @LastSO4_WSVolumeAdjustedClosing float, @LastSO4_WSVolumeAdjustedTypicalClosing float, @LastSO4_WSVolumeAdjustedWeightedClosing float, @LastSO4_RSI_SMAClosing float, @LastSO4_RSI_SMAVolume float, @LastSO4_RSI_SMAWeightedClosing float, @LastSO4_RSI_SMATypicalClosing float, @LastSO4_RSI_SMAVolumeAdjustedClosing float, @LastSO4_RSI_SMAVolumeAdjustedWeightedClosing float, @LastSO4_RSI_SMAVolumeAdjustedTypicalClosing float, @LastSO4_RSI_EMAClosing float, @LastSO4_RSI_EMAVolume float, @LastSO4_RSI_EMAWeightedClosing float, @LastSO4_RSI_EMATypicalClosing float, @LastSO4_RSI_EMAVolumeAdjustedClosing float, @LastSO4_RSI_EMAVolumeAdjustedWeightedClosing float, @LastSO4_RSI_EMAVolumeAdjustedTypicalClosing float, @LastSO4_RSI_WSClosing float, @LastSO4_RSI_WSVolume float, @LastSO4_RSI_WSWeightedClosing float, @LastSO4_RSI_WSTypicalClosing float, @LastSO4_RSI_WSVolumeAdjustedClosing float, @LastSO4_RSI_WSVolumeAdjustedWeightedClosing float, @LastSO4_RSI_WSVolumeAdjustedTypicalClosing float, @LastSO4_STDEVClosing float, @LastSO4_STDEVWeightedClosing float, @LastSO4_STDEVTypicalClosing float, @LastSO4_STDEVVolume float, @LastSO4_RVIClosing float, @LastSO4_RVIVolume float, @LastSO4_RVIWeightedClosing float, @LastSO4_RVITypicalClosing float, @LastSO4_RVIVolumeAdjustedClosing float, @LastSO4_RVIVolumeAdjustedWeightedClosing float, @LastSO4_RVIVolumeAdjustedTypicalClosing float
	declare @LastSO5_Opening float, @LastSO5_High float, @LastSO5_Low float, @LastSO5_Closing float, @LastSO5_Volume float, @LastSO5_SMAClosing float, @LastSO5_SMAWeightedClosing float, @LastSO5_SMATypicalClosing float, @LastSO5_SMAVolume float, @LastSO5_VAMAClosing float, @LastSO5_VAMATypicalClosing float, @LastSO5_VAMAWeightedClosing float, @LastSO5_WMAClosing float, @LastSO5_WMAWeightedClosing float, @LastSO5_WMATypicalClosing float, @LastSO5_EMAClosing float, @LastSO5_EMAWeightedClosing float, @LastSO5_EMATypicalClosing float, @LastSO5_EMAVolume float, @LastSO5_CMOClosing float, @LastSO5_CMOVolume float, @LastSO5_CMOWeightedClosing float, @LastSO5_CMOTypicalClosing float, @LastSO5_VMAClosing float, @LastSO5_VMAVolume float, @LastSO5_VMAWeightedClosing float, @LastSO5_VMATypicalClosing float, @LastSO5_WSClosing float, @LastSO5_WSWeightedClosing float, @LastSO5_WSTypicalClosing float, @LastSO5_WSWeightedNormalClosing float, @LastSO5_WSWeightedWeightedClosing float, @LastSO5_WSWeightedTypicalClosing float, @LastSO5_WSVolume float, @LastSO5_WSVolumeAdjustedClosing float, @LastSO5_WSVolumeAdjustedTypicalClosing float, @LastSO5_WSVolumeAdjustedWeightedClosing float, @LastSO5_RSI_SMAClosing float, @LastSO5_RSI_SMAVolume float, @LastSO5_RSI_SMAWeightedClosing float, @LastSO5_RSI_SMATypicalClosing float, @LastSO5_RSI_SMAVolumeAdjustedClosing float, @LastSO5_RSI_SMAVolumeAdjustedWeightedClosing float, @LastSO5_RSI_SMAVolumeAdjustedTypicalClosing float, @LastSO5_RSI_EMAClosing float, @LastSO5_RSI_EMAVolume float, @LastSO5_RSI_EMAWeightedClosing float, @LastSO5_RSI_EMATypicalClosing float, @LastSO5_RSI_EMAVolumeAdjustedClosing float, @LastSO5_RSI_EMAVolumeAdjustedWeightedClosing float, @LastSO5_RSI_EMAVolumeAdjustedTypicalClosing float, @LastSO5_RSI_WSClosing float, @LastSO5_RSI_WSVolume float, @LastSO5_RSI_WSWeightedClosing float, @LastSO5_RSI_WSTypicalClosing float, @LastSO5_RSI_WSVolumeAdjustedClosing float, @LastSO5_RSI_WSVolumeAdjustedWeightedClosing float, @LastSO5_RSI_WSVolumeAdjustedTypicalClosing float, @LastSO5_STDEVClosing float, @LastSO5_STDEVWeightedClosing float, @LastSO5_STDEVTypicalClosing float, @LastSO5_STDEVVolume float, @LastSO5_RVIClosing float, @LastSO5_RVIVolume float, @LastSO5_RVIWeightedClosing float, @LastSO5_RVITypicalClosing float, @LastSO5_RVIVolumeAdjustedClosing float, @LastSO5_RVIVolumeAdjustedWeightedClosing float, @LastSO5_RVIVolumeAdjustedTypicalClosing float
	declare @LastSO6_Opening float, @LastSO6_High float, @LastSO6_Low float, @LastSO6_Closing float, @LastSO6_Volume float, @LastSO6_SMAClosing float, @LastSO6_SMAWeightedClosing float, @LastSO6_SMATypicalClosing float, @LastSO6_SMAVolume float, @LastSO6_VAMAClosing float, @LastSO6_VAMATypicalClosing float, @LastSO6_VAMAWeightedClosing float, @LastSO6_WMAClosing float, @LastSO6_WMAWeightedClosing float, @LastSO6_WMATypicalClosing float, @LastSO6_EMAClosing float, @LastSO6_EMAWeightedClosing float, @LastSO6_EMATypicalClosing float, @LastSO6_EMAVolume float, @LastSO6_CMOClosing float, @LastSO6_CMOVolume float, @LastSO6_CMOWeightedClosing float, @LastSO6_CMOTypicalClosing float, @LastSO6_VMAClosing float, @LastSO6_VMAVolume float, @LastSO6_VMAWeightedClosing float, @LastSO6_VMATypicalClosing float, @LastSO6_WSClosing float, @LastSO6_WSWeightedClosing float, @LastSO6_WSTypicalClosing float, @LastSO6_WSWeightedNormalClosing float, @LastSO6_WSWeightedWeightedClosing float, @LastSO6_WSWeightedTypicalClosing float, @LastSO6_WSVolume float, @LastSO6_WSVolumeAdjustedClosing float, @LastSO6_WSVolumeAdjustedTypicalClosing float, @LastSO6_WSVolumeAdjustedWeightedClosing float, @LastSO6_RSI_SMAClosing float, @LastSO6_RSI_SMAVolume float, @LastSO6_RSI_SMAWeightedClosing float, @LastSO6_RSI_SMATypicalClosing float, @LastSO6_RSI_SMAVolumeAdjustedClosing float, @LastSO6_RSI_SMAVolumeAdjustedWeightedClosing float, @LastSO6_RSI_SMAVolumeAdjustedTypicalClosing float, @LastSO6_RSI_EMAClosing float, @LastSO6_RSI_EMAVolume float, @LastSO6_RSI_EMAWeightedClosing float, @LastSO6_RSI_EMATypicalClosing float, @LastSO6_RSI_EMAVolumeAdjustedClosing float, @LastSO6_RSI_EMAVolumeAdjustedWeightedClosing float, @LastSO6_RSI_EMAVolumeAdjustedTypicalClosing float, @LastSO6_RSI_WSClosing float, @LastSO6_RSI_WSVolume float, @LastSO6_RSI_WSWeightedClosing float, @LastSO6_RSI_WSTypicalClosing float, @LastSO6_RSI_WSVolumeAdjustedClosing float, @LastSO6_RSI_WSVolumeAdjustedWeightedClosing float, @LastSO6_RSI_WSVolumeAdjustedTypicalClosing float, @LastSO6_STDEVClosing float, @LastSO6_STDEVWeightedClosing float, @LastSO6_STDEVTypicalClosing float, @LastSO6_STDEVVolume float, @LastSO6_RVIClosing float, @LastSO6_RVIVolume float, @LastSO6_RVIWeightedClosing float, @LastSO6_RVITypicalClosing float, @LastSO6_RVIVolumeAdjustedClosing float, @LastSO6_RVIVolumeAdjustedWeightedClosing float, @LastSO6_RVIVolumeAdjustedTypicalClosing float
	declare @LastSO7_Opening float, @LastSO7_High float, @LastSO7_Low float, @LastSO7_Closing float, @LastSO7_Volume float, @LastSO7_SMAClosing float, @LastSO7_SMAWeightedClosing float, @LastSO7_SMATypicalClosing float, @LastSO7_SMAVolume float, @LastSO7_VAMAClosing float, @LastSO7_VAMATypicalClosing float, @LastSO7_VAMAWeightedClosing float, @LastSO7_WMAClosing float, @LastSO7_WMAWeightedClosing float, @LastSO7_WMATypicalClosing float, @LastSO7_EMAClosing float, @LastSO7_EMAWeightedClosing float, @LastSO7_EMATypicalClosing float, @LastSO7_EMAVolume float, @LastSO7_CMOClosing float, @LastSO7_CMOVolume float, @LastSO7_CMOWeightedClosing float, @LastSO7_CMOTypicalClosing float, @LastSO7_VMAClosing float, @LastSO7_VMAVolume float, @LastSO7_VMAWeightedClosing float, @LastSO7_VMATypicalClosing float, @LastSO7_WSClosing float, @LastSO7_WSWeightedClosing float, @LastSO7_WSTypicalClosing float, @LastSO7_WSWeightedNormalClosing float, @LastSO7_WSWeightedWeightedClosing float, @LastSO7_WSWeightedTypicalClosing float, @LastSO7_WSVolume float, @LastSO7_WSVolumeAdjustedClosing float, @LastSO7_WSVolumeAdjustedTypicalClosing float, @LastSO7_WSVolumeAdjustedWeightedClosing float, @LastSO7_RSI_SMAClosing float, @LastSO7_RSI_SMAVolume float, @LastSO7_RSI_SMAWeightedClosing float, @LastSO7_RSI_SMATypicalClosing float, @LastSO7_RSI_SMAVolumeAdjustedClosing float, @LastSO7_RSI_SMAVolumeAdjustedWeightedClosing float, @LastSO7_RSI_SMAVolumeAdjustedTypicalClosing float, @LastSO7_RSI_EMAClosing float, @LastSO7_RSI_EMAVolume float, @LastSO7_RSI_EMAWeightedClosing float, @LastSO7_RSI_EMATypicalClosing float, @LastSO7_RSI_EMAVolumeAdjustedClosing float, @LastSO7_RSI_EMAVolumeAdjustedWeightedClosing float, @LastSO7_RSI_EMAVolumeAdjustedTypicalClosing float, @LastSO7_RSI_WSClosing float, @LastSO7_RSI_WSVolume float, @LastSO7_RSI_WSWeightedClosing float, @LastSO7_RSI_WSTypicalClosing float, @LastSO7_RSI_WSVolumeAdjustedClosing float, @LastSO7_RSI_WSVolumeAdjustedWeightedClosing float, @LastSO7_RSI_WSVolumeAdjustedTypicalClosing float, @LastSO7_STDEVClosing float, @LastSO7_STDEVWeightedClosing float, @LastSO7_STDEVTypicalClosing float, @LastSO7_STDEVVolume float, @LastSO7_RVIClosing float, @LastSO7_RVIVolume float, @LastSO7_RVIWeightedClosing float, @LastSO7_RVITypicalClosing float, @LastSO7_RVIVolumeAdjustedClosing float, @LastSO7_RVIVolumeAdjustedWeightedClosing float, @LastSO7_RVIVolumeAdjustedTypicalClosing float
	declare @LastSO8_Opening float, @LastSO8_High float, @LastSO8_Low float, @LastSO8_Closing float, @LastSO8_Volume float, @LastSO8_SMAClosing float, @LastSO8_SMAWeightedClosing float, @LastSO8_SMATypicalClosing float, @LastSO8_SMAVolume float, @LastSO8_VAMAClosing float, @LastSO8_VAMATypicalClosing float, @LastSO8_VAMAWeightedClosing float, @LastSO8_WMAClosing float, @LastSO8_WMAWeightedClosing float, @LastSO8_WMATypicalClosing float, @LastSO8_EMAClosing float, @LastSO8_EMAWeightedClosing float, @LastSO8_EMATypicalClosing float, @LastSO8_EMAVolume float, @LastSO8_CMOClosing float, @LastSO8_CMOVolume float, @LastSO8_CMOWeightedClosing float, @LastSO8_CMOTypicalClosing float, @LastSO8_VMAClosing float, @LastSO8_VMAVolume float, @LastSO8_VMAWeightedClosing float, @LastSO8_VMATypicalClosing float, @LastSO8_WSClosing float, @LastSO8_WSWeightedClosing float, @LastSO8_WSTypicalClosing float, @LastSO8_WSWeightedNormalClosing float, @LastSO8_WSWeightedWeightedClosing float, @LastSO8_WSWeightedTypicalClosing float, @LastSO8_WSVolume float, @LastSO8_WSVolumeAdjustedClosing float, @LastSO8_WSVolumeAdjustedTypicalClosing float, @LastSO8_WSVolumeAdjustedWeightedClosing float, @LastSO8_RSI_SMAClosing float, @LastSO8_RSI_SMAVolume float, @LastSO8_RSI_SMAWeightedClosing float, @LastSO8_RSI_SMATypicalClosing float, @LastSO8_RSI_SMAVolumeAdjustedClosing float, @LastSO8_RSI_SMAVolumeAdjustedWeightedClosing float, @LastSO8_RSI_SMAVolumeAdjustedTypicalClosing float, @LastSO8_RSI_EMAClosing float, @LastSO8_RSI_EMAVolume float, @LastSO8_RSI_EMAWeightedClosing float, @LastSO8_RSI_EMATypicalClosing float, @LastSO8_RSI_EMAVolumeAdjustedClosing float, @LastSO8_RSI_EMAVolumeAdjustedWeightedClosing float, @LastSO8_RSI_EMAVolumeAdjustedTypicalClosing float, @LastSO8_RSI_WSClosing float, @LastSO8_RSI_WSVolume float, @LastSO8_RSI_WSWeightedClosing float, @LastSO8_RSI_WSTypicalClosing float, @LastSO8_RSI_WSVolumeAdjustedClosing float, @LastSO8_RSI_WSVolumeAdjustedWeightedClosing float, @LastSO8_RSI_WSVolumeAdjustedTypicalClosing float, @LastSO8_STDEVClosing float, @LastSO8_STDEVWeightedClosing float, @LastSO8_STDEVTypicalClosing float, @LastSO8_STDEVVolume float, @LastSO8_RVIClosing float, @LastSO8_RVIVolume float, @LastSO8_RVIWeightedClosing float, @LastSO8_RVITypicalClosing float, @LastSO8_RVIVolumeAdjustedClosing float, @LastSO8_RVIVolumeAdjustedWeightedClosing float, @LastSO8_RVIVolumeAdjustedTypicalClosing float
	declare @LastSO9_Opening float, @LastSO9_High float, @LastSO9_Low float, @LastSO9_Closing float, @LastSO9_Volume float, @LastSO9_SMAClosing float, @LastSO9_SMAWeightedClosing float, @LastSO9_SMATypicalClosing float, @LastSO9_SMAVolume float, @LastSO9_VAMAClosing float, @LastSO9_VAMATypicalClosing float, @LastSO9_VAMAWeightedClosing float, @LastSO9_WMAClosing float, @LastSO9_WMAWeightedClosing float, @LastSO9_WMATypicalClosing float, @LastSO9_EMAClosing float, @LastSO9_EMAWeightedClosing float, @LastSO9_EMATypicalClosing float, @LastSO9_EMAVolume float, @LastSO9_CMOClosing float, @LastSO9_CMOVolume float, @LastSO9_CMOWeightedClosing float, @LastSO9_CMOTypicalClosing float, @LastSO9_VMAClosing float, @LastSO9_VMAVolume float, @LastSO9_VMAWeightedClosing float, @LastSO9_VMATypicalClosing float, @LastSO9_WSClosing float, @LastSO9_WSWeightedClosing float, @LastSO9_WSTypicalClosing float, @LastSO9_WSWeightedNormalClosing float, @LastSO9_WSWeightedWeightedClosing float, @LastSO9_WSWeightedTypicalClosing float, @LastSO9_WSVolume float, @LastSO9_WSVolumeAdjustedClosing float, @LastSO9_WSVolumeAdjustedTypicalClosing float, @LastSO9_WSVolumeAdjustedWeightedClosing float, @LastSO9_RSI_SMAClosing float, @LastSO9_RSI_SMAVolume float, @LastSO9_RSI_SMAWeightedClosing float, @LastSO9_RSI_SMATypicalClosing float, @LastSO9_RSI_SMAVolumeAdjustedClosing float, @LastSO9_RSI_SMAVolumeAdjustedWeightedClosing float, @LastSO9_RSI_SMAVolumeAdjustedTypicalClosing float, @LastSO9_RSI_EMAClosing float, @LastSO9_RSI_EMAVolume float, @LastSO9_RSI_EMAWeightedClosing float, @LastSO9_RSI_EMATypicalClosing float, @LastSO9_RSI_EMAVolumeAdjustedClosing float, @LastSO9_RSI_EMAVolumeAdjustedWeightedClosing float, @LastSO9_RSI_EMAVolumeAdjustedTypicalClosing float, @LastSO9_RSI_WSClosing float, @LastSO9_RSI_WSVolume float, @LastSO9_RSI_WSWeightedClosing float, @LastSO9_RSI_WSTypicalClosing float, @LastSO9_RSI_WSVolumeAdjustedClosing float, @LastSO9_RSI_WSVolumeAdjustedWeightedClosing float, @LastSO9_RSI_WSVolumeAdjustedTypicalClosing float, @LastSO9_STDEVClosing float, @LastSO9_STDEVWeightedClosing float, @LastSO9_STDEVTypicalClosing float, @LastSO9_STDEVVolume float, @LastSO9_RVIClosing float, @LastSO9_RVIVolume float, @LastSO9_RVIWeightedClosing float, @LastSO9_RVITypicalClosing float, @LastSO9_RVIVolumeAdjustedClosing float, @LastSO9_RVIVolumeAdjustedWeightedClosing float, @LastSO9_RVIVolumeAdjustedTypicalClosing float

	/*Hold Calculated record*/ 
	declare @DateFrom float, @DateTo float, @Opening float, @High float, @Low float, @Closing float, @Volume float, @TotalClosing float, @TotalWeightedClosing float, @TotalTypicalClosing float, @TotalWeightedNormalClosing float, @TotalWeightedWeightedClosing float, @TotalWeightedTypicalClosing float, @TotalVolume float, @TotalVolumeAdjustedClosing float, @TotalVolumeAdjustedTypicalClosing float, @TotalVolumeAdjustedWeightedClosing float, @TotalGainClosing float, @TotalLossClosing float, @TotalGainVolume float, @TotalLossVolume float, @TotalGainWeightedClosing float, @TotalLossWeightedClosing float, @TotalGainTypicalClosing float, @TotalLossTypicalClosing float, @TotalGainVolumeAdjustedClosing float, @TotalLossVolumeAdjustedClosing float, @TotalGainVolumeAdjustedWeightedClosing float, @TotalLossVolumeAdjustedWeightedClosing float, @TotalGainVolumeAdjustedTypicalClosing float, @TotalLossVolumeAdjustedTypicalClosing float, @SMAClosing float, @SMAWeightedClosing float, @SMATypicalClosing float, @SMAVolume float, @SMAGainClosing float, @SMALossClosing float, @SMAGainVolume float, @SMALossVolume float, @SMAGainWeightedClosing float, @SMALossWeightedClosing float, @SMAGainTypicalClosing float, @SMALossTypicalClosing float, @SMAGainVolumeAdjustedClosing float, @SMALossVolumeAdjustedClosing float, @SMAGainVolumeAdjustedWeightedClosing float, @SMALossVolumeAdjustedWeightedClosing float, @SMAGainVolumeAdjustedTypicalClosing float, @SMALossVolumeAdjustedTypicalClosing float, @VAMAClosing float, @VAMATypicalClosing float, @VAMAWeightedClosing float, @WMAClosing float, @WMAWeightedClosing float, @WMATypicalClosing float, @EMAClosing float, @EMAWeightedClosing float, @EMATypicalClosing float, @EMAVolume float, @EMAGainClosing float, @EMALossClosing float, @EMAGainVolume float, @EMALossVolume float, @EMAGainWeightedClosing float, @EMALossWeightedClosing float, @EMAGainTypicalClosing float, @EMALossTypicalClosing float, @EMAGainVolumeAdjustedClosing float, @EMALossVolumeAdjustedClosing float, @EMAGainVolumeAdjustedWeightedClosing float, @EMALossVolumeAdjustedWeightedClosing float, @EMAGainVolumeAdjustedTypicalClosing float, @EMALossVolumeAdjustedTypicalClosing float, @CMOClosing float, @CMOVolume float, @CMOWeightedClosing float, @CMOTypicalClosing float, @VMAClosing float, @VMAVolume float, @VMAWeightedClosing float, @VMATypicalClosing float, @WSClosing float, @WSWeightedClosing float, @WSTypicalClosing float, @WSWeightedNormalClosing float, @WSWeightedWeightedClosing float, @WSWeightedTypicalClosing float, @WSVolume float, @WSVolumeAdjustedClosing float, @WSVolumeAdjustedTypicalClosing float, @WSVolumeAdjustedWeightedClosing float, @WSGainClosing float, @WSLossClosing float, @WSGainVolume float, @WSLossVolume float, @WSGainWeightedClosing float, @WSLossWeightedClosing float, @WSGainTypicalClosing float, @WSLossTypicalClosing float, @WSGainVolumeAdjustedClosing float, @WSLossVolumeAdjustedClosing float, @WSGainVolumeAdjustedWeightedClosing float, @WSLossVolumeAdjustedWeightedClosing float, @WSGainVolumeAdjustedTypicalClosing float, @WSLossVolumeAdjustedTypicalClosing float, @RSI_SMAClosing float, @RSI_SMAVolume float, @RSI_SMAWeightedClosing float, @RSI_SMATypicalClosing float, @RSI_SMAVolumeAdjustedClosing float, @RSI_SMAVolumeAdjustedWeightedClosing float, @RSI_SMAVolumeAdjustedTypicalClosing float, @RSI_EMAClosing float, @RSI_EMAVolume float, @RSI_EMAWeightedClosing float, @RSI_EMATypicalClosing float, @RSI_EMAVolumeAdjustedClosing float, @RSI_EMAVolumeAdjustedWeightedClosing float, @RSI_EMAVolumeAdjustedTypicalClosing float, @RSI_WSClosing float, @RSI_WSVolume float, @RSI_WSWeightedClosing float, @RSI_WSTypicalClosing float, @RSI_WSVolumeAdjustedClosing float, @RSI_WSVolumeAdjustedWeightedClosing float, @RSI_WSVolumeAdjustedTypicalClosing float, @STDEVClosing float, @STDEVWeightedClosing float, @STDEVTypicalClosing float, @STDEVVolume float, @STDEVGainClosing float, @STDEVLossClosing float, @STDEVGainVolume float, @STDEVLossVolume float, @STDEVGainWeightedClosing float, @STDEVLossWeightedClosing float, @STDEVGainTypicalClosing float, @STDEVLossTypicalClosing float, @STDEVGainVolumeAdjustedClosing float, @STDEVLossVolumeAdjustedClosing float, @STDEVGainVolumeAdjustedWeightedClosing float, @STDEVLossVolumeAdjustedWeightedClosing float, @STDEVGainVolumeAdjustedTypicalClosing float, @STDEVLossVolumeAdjustedTypicalClosing float, @RVIClosing float, @RVIVolume float, @RVIWeightedClosing float, @RVITypicalClosing float, @RVIVolumeAdjustedClosing float, @RVIVolumeAdjustedWeightedClosing float, @RVIVolumeAdjustedTypicalClosing float
	declare @HighestOpening float, @LowestOpening float, @HighestHigh float, @LowestHigh float, @HighestLow float, @LowestLow float, @HighestClosing float, @LowestClosing float, @HighestVolume float, @LowestVolume float, @HighestSMAClosing float, @LowestSMAClosing float, @HighestSMAWeightedClosing float, @LowestSMAWeightedClosing float, @HighestSMATypicalClosing float, @LowestSMATypicalClosing float, @HighestSMAVolume float, @LowestSMAVolume float, @HighestVAMAClosing float, @LowestVAMAClosing float, @HighestVAMATypicalClosing float, @LowestVAMATypicalClosing float, @HighestVAMAWeightedClosing float, @LowestVAMAWeightedClosing float, @HighestWMAClosing float, @LowestWMAClosing float, @HighestWMAWeightedClosing float, @LowestWMAWeightedClosing float, @HighestWMATypicalClosing float, @LowestWMATypicalClosing float, @HighestEMAClosing float, @LowestEMAClosing float, @HighestEMAWeightedClosing float, @LowestEMAWeightedClosing float, @HighestEMATypicalClosing float, @LowestEMATypicalClosing float, @HighestEMAVolume float, @LowestEMAVolume float, @HighestCMOClosing float, @LowestCMOClosing float, @HighestCMOVolume float, @LowestCMOVolume float, @HighestCMOWeightedClosing float, @LowestCMOWeightedClosing float, @HighestCMOTypicalClosing float, @LowestCMOTypicalClosing float, @HighestVMAClosing float, @LowestVMAClosing float, @HighestVMAVolume float, @LowestVMAVolume float, @HighestVMAWeightedClosing float, @LowestVMAWeightedClosing float, @HighestVMATypicalClosing float, @LowestVMATypicalClosing float, @HighestWSClosing float, @LowestWSClosing float, @HighestWSWeightedClosing float, @LowestWSWeightedClosing float, @HighestWSTypicalClosing float, @LowestWSTypicalClosing float, @HighestWSWeightedNormalClosing float, @LowestWSWeightedNormalClosing float, @HighestWSWeightedWeightedClosing float, @LowestWSWeightedWeightedClosing float, @HighestWSWeightedTypicalClosing float, @LowestWSWeightedTypicalClosing float, @HighestWSVolume float, @LowestWSVolume float, @HighestWSVolumeAdjustedClosing float, @LowestWSVolumeAdjustedClosing float, @HighestWSVolumeAdjustedTypicalClosing float, @LowestWSVolumeAdjustedTypicalClosing float, @HighestWSVolumeAdjustedWeightedClosing float, @LowestWSVolumeAdjustedWeightedClosing float, @HighestRSI_SMAClosing float, @LowestRSI_SMAClosing float, @HighestRSI_SMAVolume float, @LowestRSI_SMAVolume float, @HighestRSI_SMAWeightedClosing float, @LowestRSI_SMAWeightedClosing float, @HighestRSI_SMATypicalClosing float, @LowestRSI_SMATypicalClosing float, @HighestRSI_SMAVolumeAdjustedClosing float, @LowestRSI_SMAVolumeAdjustedClosing float, @HighestRSI_SMAVolumeAdjustedWeightedClosing float, @LowestRSI_SMAVolumeAdjustedWeightedClosing float, @HighestRSI_SMAVolumeAdjustedTypicalClosing float, @LowestRSI_SMAVolumeAdjustedTypicalClosing float, @HighestRSI_EMAClosing float, @LowestRSI_EMAClosing float, @HighestRSI_EMAVolume float, @LowestRSI_EMAVolume float, @HighestRSI_EMAWeightedClosing float, @LowestRSI_EMAWeightedClosing float, @HighestRSI_EMATypicalClosing float, @LowestRSI_EMATypicalClosing float, @HighestRSI_EMAVolumeAdjustedClosing float, @LowestRSI_EMAVolumeAdjustedClosing float, @HighestRSI_EMAVolumeAdjustedWeightedClosing float, @LowestRSI_EMAVolumeAdjustedWeightedClosing float, @HighestRSI_EMAVolumeAdjustedTypicalClosing float, @LowestRSI_EMAVolumeAdjustedTypicalClosing float, @HighestRSI_WSClosing float, @LowestRSI_WSClosing float, @HighestRSI_WSVolume float, @LowestRSI_WSVolume float, @HighestRSI_WSWeightedClosing float, @LowestRSI_WSWeightedClosing float, @HighestRSI_WSTypicalClosing float, @LowestRSI_WSTypicalClosing float, @HighestRSI_WSVolumeAdjustedClosing float, @LowestRSI_WSVolumeAdjustedClosing float, @HighestRSI_WSVolumeAdjustedWeightedClosing float, @LowestRSI_WSVolumeAdjustedWeightedClosing float, @HighestRSI_WSVolumeAdjustedTypicalClosing float, @LowestRSI_WSVolumeAdjustedTypicalClosing float, @HighestSTDEVClosing float, @LowestSTDEVClosing float, @HighestSTDEVWeightedClosing float, @LowestSTDEVWeightedClosing float, @HighestSTDEVTypicalClosing float, @LowestSTDEVTypicalClosing float, @HighestSTDEVVolume float, @LowestSTDEVVolume float, @HighestRVIClosing float, @LowestRVIClosing float, @HighestRVIVolume float, @LowestRVIVolume float, @HighestRVIWeightedClosing float, @LowestRVIWeightedClosing float, @HighestRVITypicalClosing float, @LowestRVITypicalClosing float, @HighestRVIVolumeAdjustedClosing float, @LowestRVIVolumeAdjustedClosing float, @HighestRVIVolumeAdjustedWeightedClosing float, @LowestRVIVolumeAdjustedWeightedClosing float, @HighestRVIVolumeAdjustedTypicalClosing float, @LowestRVIVolumeAdjustedTypicalClosing float
	declare @SO_Opening float, @SO_High float, @SO_Low float, @SO_Closing float, @SO_Volume float, @SO_SMAClosing float, @SO_SMAWeightedClosing float, @SO_SMATypicalClosing float, @SO_SMAVolume float, @SO_VAMAClosing float, @SO_VAMATypicalClosing float, @SO_VAMAWeightedClosing float, @SO_WMAClosing float, @SO_WMAWeightedClosing float, @SO_WMATypicalClosing float, @SO_EMAClosing float, @SO_EMAWeightedClosing float, @SO_EMATypicalClosing float, @SO_EMAVolume float, @SO_CMOClosing float, @SO_CMOVolume float, @SO_CMOWeightedClosing float, @SO_CMOTypicalClosing float, @SO_VMAClosing float, @SO_VMAVolume float, @SO_VMAWeightedClosing float, @SO_VMATypicalClosing float, @SO_WSClosing float, @SO_WSWeightedClosing float, @SO_WSTypicalClosing float, @SO_WSWeightedNormalClosing float, @SO_WSWeightedWeightedClosing float, @SO_WSWeightedTypicalClosing float, @SO_WSVolume float, @SO_WSVolumeAdjustedClosing float, @SO_WSVolumeAdjustedTypicalClosing float, @SO_WSVolumeAdjustedWeightedClosing float, @SO_RSI_SMAClosing float, @SO_RSI_SMAVolume float, @SO_RSI_SMAWeightedClosing float, @SO_RSI_SMATypicalClosing float, @SO_RSI_SMAVolumeAdjustedClosing float, @SO_RSI_SMAVolumeAdjustedWeightedClosing float, @SO_RSI_SMAVolumeAdjustedTypicalClosing float, @SO_RSI_EMAClosing float, @SO_RSI_EMAVolume float, @SO_RSI_EMAWeightedClosing float, @SO_RSI_EMATypicalClosing float, @SO_RSI_EMAVolumeAdjustedClosing float, @SO_RSI_EMAVolumeAdjustedWeightedClosing float, @SO_RSI_EMAVolumeAdjustedTypicalClosing float, @SO_RSI_WSClosing float, @SO_RSI_WSVolume float, @SO_RSI_WSWeightedClosing float, @SO_RSI_WSTypicalClosing float, @SO_RSI_WSVolumeAdjustedClosing float, @SO_RSI_WSVolumeAdjustedWeightedClosing float, @SO_RSI_WSVolumeAdjustedTypicalClosing float, @SO_STDEVClosing float, @SO_STDEVWeightedClosing float, @SO_STDEVTypicalClosing float, @SO_STDEVVolume float, @SO_RVIClosing float, @SO_RVIVolume float, @SO_RVIWeightedClosing float, @SO_RVITypicalClosing float, @SO_RVIVolumeAdjustedClosing float, @SO_RVIVolumeAdjustedWeightedClosing float, @SO_RVIVolumeAdjustedTypicalClosing float
	declare @SO3_Opening float, @SO3_High float, @SO3_Low float, @SO3_Closing float, @SO3_Volume float, @SO3_SMAClosing float, @SO3_SMAWeightedClosing float, @SO3_SMATypicalClosing float, @SO3_SMAVolume float, @SO3_VAMAClosing float, @SO3_VAMATypicalClosing float, @SO3_VAMAWeightedClosing float, @SO3_WMAClosing float, @SO3_WMAWeightedClosing float, @SO3_WMATypicalClosing float, @SO3_EMAClosing float, @SO3_EMAWeightedClosing float, @SO3_EMATypicalClosing float, @SO3_EMAVolume float, @SO3_CMOClosing float, @SO3_CMOVolume float, @SO3_CMOWeightedClosing float, @SO3_CMOTypicalClosing float, @SO3_VMAClosing float, @SO3_VMAVolume float, @SO3_VMAWeightedClosing float, @SO3_VMATypicalClosing float, @SO3_WSClosing float, @SO3_WSWeightedClosing float, @SO3_WSTypicalClosing float, @SO3_WSWeightedNormalClosing float, @SO3_WSWeightedWeightedClosing float, @SO3_WSWeightedTypicalClosing float, @SO3_WSVolume float, @SO3_WSVolumeAdjustedClosing float, @SO3_WSVolumeAdjustedTypicalClosing float, @SO3_WSVolumeAdjustedWeightedClosing float, @SO3_RSI_SMAClosing float, @SO3_RSI_SMAVolume float, @SO3_RSI_SMAWeightedClosing float, @SO3_RSI_SMATypicalClosing float, @SO3_RSI_SMAVolumeAdjustedClosing float, @SO3_RSI_SMAVolumeAdjustedWeightedClosing float, @SO3_RSI_SMAVolumeAdjustedTypicalClosing float, @SO3_RSI_EMAClosing float, @SO3_RSI_EMAVolume float, @SO3_RSI_EMAWeightedClosing float, @SO3_RSI_EMATypicalClosing float, @SO3_RSI_EMAVolumeAdjustedClosing float, @SO3_RSI_EMAVolumeAdjustedWeightedClosing float, @SO3_RSI_EMAVolumeAdjustedTypicalClosing float, @SO3_RSI_WSClosing float, @SO3_RSI_WSVolume float, @SO3_RSI_WSWeightedClosing float, @SO3_RSI_WSTypicalClosing float, @SO3_RSI_WSVolumeAdjustedClosing float, @SO3_RSI_WSVolumeAdjustedWeightedClosing float, @SO3_RSI_WSVolumeAdjustedTypicalClosing float, @SO3_STDEVClosing float, @SO3_STDEVWeightedClosing float, @SO3_STDEVTypicalClosing float, @SO3_STDEVVolume float, @SO3_RVIClosing float, @SO3_RVIVolume float, @SO3_RVIWeightedClosing float, @SO3_RVITypicalClosing float, @SO3_RVIVolumeAdjustedClosing float, @SO3_RVIVolumeAdjustedWeightedClosing float, @SO3_RVIVolumeAdjustedTypicalClosing float
	declare @SO4_Opening float, @SO4_High float, @SO4_Low float, @SO4_Closing float, @SO4_Volume float, @SO4_SMAClosing float, @SO4_SMAWeightedClosing float, @SO4_SMATypicalClosing float, @SO4_SMAVolume float, @SO4_VAMAClosing float, @SO4_VAMATypicalClosing float, @SO4_VAMAWeightedClosing float, @SO4_WMAClosing float, @SO4_WMAWeightedClosing float, @SO4_WMATypicalClosing float, @SO4_EMAClosing float, @SO4_EMAWeightedClosing float, @SO4_EMATypicalClosing float, @SO4_EMAVolume float, @SO4_CMOClosing float, @SO4_CMOVolume float, @SO4_CMOWeightedClosing float, @SO4_CMOTypicalClosing float, @SO4_VMAClosing float, @SO4_VMAVolume float, @SO4_VMAWeightedClosing float, @SO4_VMATypicalClosing float, @SO4_WSClosing float, @SO4_WSWeightedClosing float, @SO4_WSTypicalClosing float, @SO4_WSWeightedNormalClosing float, @SO4_WSWeightedWeightedClosing float, @SO4_WSWeightedTypicalClosing float, @SO4_WSVolume float, @SO4_WSVolumeAdjustedClosing float, @SO4_WSVolumeAdjustedTypicalClosing float, @SO4_WSVolumeAdjustedWeightedClosing float, @SO4_RSI_SMAClosing float, @SO4_RSI_SMAVolume float, @SO4_RSI_SMAWeightedClosing float, @SO4_RSI_SMATypicalClosing float, @SO4_RSI_SMAVolumeAdjustedClosing float, @SO4_RSI_SMAVolumeAdjustedWeightedClosing float, @SO4_RSI_SMAVolumeAdjustedTypicalClosing float, @SO4_RSI_EMAClosing float, @SO4_RSI_EMAVolume float, @SO4_RSI_EMAWeightedClosing float, @SO4_RSI_EMATypicalClosing float, @SO4_RSI_EMAVolumeAdjustedClosing float, @SO4_RSI_EMAVolumeAdjustedWeightedClosing float, @SO4_RSI_EMAVolumeAdjustedTypicalClosing float, @SO4_RSI_WSClosing float, @SO4_RSI_WSVolume float, @SO4_RSI_WSWeightedClosing float, @SO4_RSI_WSTypicalClosing float, @SO4_RSI_WSVolumeAdjustedClosing float, @SO4_RSI_WSVolumeAdjustedWeightedClosing float, @SO4_RSI_WSVolumeAdjustedTypicalClosing float, @SO4_STDEVClosing float, @SO4_STDEVWeightedClosing float, @SO4_STDEVTypicalClosing float, @SO4_STDEVVolume float, @SO4_RVIClosing float, @SO4_RVIVolume float, @SO4_RVIWeightedClosing float, @SO4_RVITypicalClosing float, @SO4_RVIVolumeAdjustedClosing float, @SO4_RVIVolumeAdjustedWeightedClosing float, @SO4_RVIVolumeAdjustedTypicalClosing float
	declare @SO5_Opening float, @SO5_High float, @SO5_Low float, @SO5_Closing float, @SO5_Volume float, @SO5_SMAClosing float, @SO5_SMAWeightedClosing float, @SO5_SMATypicalClosing float, @SO5_SMAVolume float, @SO5_VAMAClosing float, @SO5_VAMATypicalClosing float, @SO5_VAMAWeightedClosing float, @SO5_WMAClosing float, @SO5_WMAWeightedClosing float, @SO5_WMATypicalClosing float, @SO5_EMAClosing float, @SO5_EMAWeightedClosing float, @SO5_EMATypicalClosing float, @SO5_EMAVolume float, @SO5_CMOClosing float, @SO5_CMOVolume float, @SO5_CMOWeightedClosing float, @SO5_CMOTypicalClosing float, @SO5_VMAClosing float, @SO5_VMAVolume float, @SO5_VMAWeightedClosing float, @SO5_VMATypicalClosing float, @SO5_WSClosing float, @SO5_WSWeightedClosing float, @SO5_WSTypicalClosing float, @SO5_WSWeightedNormalClosing float, @SO5_WSWeightedWeightedClosing float, @SO5_WSWeightedTypicalClosing float, @SO5_WSVolume float, @SO5_WSVolumeAdjustedClosing float, @SO5_WSVolumeAdjustedTypicalClosing float, @SO5_WSVolumeAdjustedWeightedClosing float, @SO5_RSI_SMAClosing float, @SO5_RSI_SMAVolume float, @SO5_RSI_SMAWeightedClosing float, @SO5_RSI_SMATypicalClosing float, @SO5_RSI_SMAVolumeAdjustedClosing float, @SO5_RSI_SMAVolumeAdjustedWeightedClosing float, @SO5_RSI_SMAVolumeAdjustedTypicalClosing float, @SO5_RSI_EMAClosing float, @SO5_RSI_EMAVolume float, @SO5_RSI_EMAWeightedClosing float, @SO5_RSI_EMATypicalClosing float, @SO5_RSI_EMAVolumeAdjustedClosing float, @SO5_RSI_EMAVolumeAdjustedWeightedClosing float, @SO5_RSI_EMAVolumeAdjustedTypicalClosing float, @SO5_RSI_WSClosing float, @SO5_RSI_WSVolume float, @SO5_RSI_WSWeightedClosing float, @SO5_RSI_WSTypicalClosing float, @SO5_RSI_WSVolumeAdjustedClosing float, @SO5_RSI_WSVolumeAdjustedWeightedClosing float, @SO5_RSI_WSVolumeAdjustedTypicalClosing float, @SO5_STDEVClosing float, @SO5_STDEVWeightedClosing float, @SO5_STDEVTypicalClosing float, @SO5_STDEVVolume float, @SO5_RVIClosing float, @SO5_RVIVolume float, @SO5_RVIWeightedClosing float, @SO5_RVITypicalClosing float, @SO5_RVIVolumeAdjustedClosing float, @SO5_RVIVolumeAdjustedWeightedClosing float, @SO5_RVIVolumeAdjustedTypicalClosing float
	declare @SO6_Opening float, @SO6_High float, @SO6_Low float, @SO6_Closing float, @SO6_Volume float, @SO6_SMAClosing float, @SO6_SMAWeightedClosing float, @SO6_SMATypicalClosing float, @SO6_SMAVolume float, @SO6_VAMAClosing float, @SO6_VAMATypicalClosing float, @SO6_VAMAWeightedClosing float, @SO6_WMAClosing float, @SO6_WMAWeightedClosing float, @SO6_WMATypicalClosing float, @SO6_EMAClosing float, @SO6_EMAWeightedClosing float, @SO6_EMATypicalClosing float, @SO6_EMAVolume float, @SO6_CMOClosing float, @SO6_CMOVolume float, @SO6_CMOWeightedClosing float, @SO6_CMOTypicalClosing float, @SO6_VMAClosing float, @SO6_VMAVolume float, @SO6_VMAWeightedClosing float, @SO6_VMATypicalClosing float, @SO6_WSClosing float, @SO6_WSWeightedClosing float, @SO6_WSTypicalClosing float, @SO6_WSWeightedNormalClosing float, @SO6_WSWeightedWeightedClosing float, @SO6_WSWeightedTypicalClosing float, @SO6_WSVolume float, @SO6_WSVolumeAdjustedClosing float, @SO6_WSVolumeAdjustedTypicalClosing float, @SO6_WSVolumeAdjustedWeightedClosing float, @SO6_RSI_SMAClosing float, @SO6_RSI_SMAVolume float, @SO6_RSI_SMAWeightedClosing float, @SO6_RSI_SMATypicalClosing float, @SO6_RSI_SMAVolumeAdjustedClosing float, @SO6_RSI_SMAVolumeAdjustedWeightedClosing float, @SO6_RSI_SMAVolumeAdjustedTypicalClosing float, @SO6_RSI_EMAClosing float, @SO6_RSI_EMAVolume float, @SO6_RSI_EMAWeightedClosing float, @SO6_RSI_EMATypicalClosing float, @SO6_RSI_EMAVolumeAdjustedClosing float, @SO6_RSI_EMAVolumeAdjustedWeightedClosing float, @SO6_RSI_EMAVolumeAdjustedTypicalClosing float, @SO6_RSI_WSClosing float, @SO6_RSI_WSVolume float, @SO6_RSI_WSWeightedClosing float, @SO6_RSI_WSTypicalClosing float, @SO6_RSI_WSVolumeAdjustedClosing float, @SO6_RSI_WSVolumeAdjustedWeightedClosing float, @SO6_RSI_WSVolumeAdjustedTypicalClosing float, @SO6_STDEVClosing float, @SO6_STDEVWeightedClosing float, @SO6_STDEVTypicalClosing float, @SO6_STDEVVolume float, @SO6_RVIClosing float, @SO6_RVIVolume float, @SO6_RVIWeightedClosing float, @SO6_RVITypicalClosing float, @SO6_RVIVolumeAdjustedClosing float, @SO6_RVIVolumeAdjustedWeightedClosing float, @SO6_RVIVolumeAdjustedTypicalClosing float
	declare @SO7_Opening float, @SO7_High float, @SO7_Low float, @SO7_Closing float, @SO7_Volume float, @SO7_SMAClosing float, @SO7_SMAWeightedClosing float, @SO7_SMATypicalClosing float, @SO7_SMAVolume float, @SO7_VAMAClosing float, @SO7_VAMATypicalClosing float, @SO7_VAMAWeightedClosing float, @SO7_WMAClosing float, @SO7_WMAWeightedClosing float, @SO7_WMATypicalClosing float, @SO7_EMAClosing float, @SO7_EMAWeightedClosing float, @SO7_EMATypicalClosing float, @SO7_EMAVolume float, @SO7_CMOClosing float, @SO7_CMOVolume float, @SO7_CMOWeightedClosing float, @SO7_CMOTypicalClosing float, @SO7_VMAClosing float, @SO7_VMAVolume float, @SO7_VMAWeightedClosing float, @SO7_VMATypicalClosing float, @SO7_WSClosing float, @SO7_WSWeightedClosing float, @SO7_WSTypicalClosing float, @SO7_WSWeightedNormalClosing float, @SO7_WSWeightedWeightedClosing float, @SO7_WSWeightedTypicalClosing float, @SO7_WSVolume float, @SO7_WSVolumeAdjustedClosing float, @SO7_WSVolumeAdjustedTypicalClosing float, @SO7_WSVolumeAdjustedWeightedClosing float, @SO7_RSI_SMAClosing float, @SO7_RSI_SMAVolume float, @SO7_RSI_SMAWeightedClosing float, @SO7_RSI_SMATypicalClosing float, @SO7_RSI_SMAVolumeAdjustedClosing float, @SO7_RSI_SMAVolumeAdjustedWeightedClosing float, @SO7_RSI_SMAVolumeAdjustedTypicalClosing float, @SO7_RSI_EMAClosing float, @SO7_RSI_EMAVolume float, @SO7_RSI_EMAWeightedClosing float, @SO7_RSI_EMATypicalClosing float, @SO7_RSI_EMAVolumeAdjustedClosing float, @SO7_RSI_EMAVolumeAdjustedWeightedClosing float, @SO7_RSI_EMAVolumeAdjustedTypicalClosing float, @SO7_RSI_WSClosing float, @SO7_RSI_WSVolume float, @SO7_RSI_WSWeightedClosing float, @SO7_RSI_WSTypicalClosing float, @SO7_RSI_WSVolumeAdjustedClosing float, @SO7_RSI_WSVolumeAdjustedWeightedClosing float, @SO7_RSI_WSVolumeAdjustedTypicalClosing float, @SO7_STDEVClosing float, @SO7_STDEVWeightedClosing float, @SO7_STDEVTypicalClosing float, @SO7_STDEVVolume float, @SO7_RVIClosing float, @SO7_RVIVolume float, @SO7_RVIWeightedClosing float, @SO7_RVITypicalClosing float, @SO7_RVIVolumeAdjustedClosing float, @SO7_RVIVolumeAdjustedWeightedClosing float, @SO7_RVIVolumeAdjustedTypicalClosing float
	declare @SO8_Opening float, @SO8_High float, @SO8_Low float, @SO8_Closing float, @SO8_Volume float, @SO8_SMAClosing float, @SO8_SMAWeightedClosing float, @SO8_SMATypicalClosing float, @SO8_SMAVolume float, @SO8_VAMAClosing float, @SO8_VAMATypicalClosing float, @SO8_VAMAWeightedClosing float, @SO8_WMAClosing float, @SO8_WMAWeightedClosing float, @SO8_WMATypicalClosing float, @SO8_EMAClosing float, @SO8_EMAWeightedClosing float, @SO8_EMATypicalClosing float, @SO8_EMAVolume float, @SO8_CMOClosing float, @SO8_CMOVolume float, @SO8_CMOWeightedClosing float, @SO8_CMOTypicalClosing float, @SO8_VMAClosing float, @SO8_VMAVolume float, @SO8_VMAWeightedClosing float, @SO8_VMATypicalClosing float, @SO8_WSClosing float, @SO8_WSWeightedClosing float, @SO8_WSTypicalClosing float, @SO8_WSWeightedNormalClosing float, @SO8_WSWeightedWeightedClosing float, @SO8_WSWeightedTypicalClosing float, @SO8_WSVolume float, @SO8_WSVolumeAdjustedClosing float, @SO8_WSVolumeAdjustedTypicalClosing float, @SO8_WSVolumeAdjustedWeightedClosing float, @SO8_RSI_SMAClosing float, @SO8_RSI_SMAVolume float, @SO8_RSI_SMAWeightedClosing float, @SO8_RSI_SMATypicalClosing float, @SO8_RSI_SMAVolumeAdjustedClosing float, @SO8_RSI_SMAVolumeAdjustedWeightedClosing float, @SO8_RSI_SMAVolumeAdjustedTypicalClosing float, @SO8_RSI_EMAClosing float, @SO8_RSI_EMAVolume float, @SO8_RSI_EMAWeightedClosing float, @SO8_RSI_EMATypicalClosing float, @SO8_RSI_EMAVolumeAdjustedClosing float, @SO8_RSI_EMAVolumeAdjustedWeightedClosing float, @SO8_RSI_EMAVolumeAdjustedTypicalClosing float, @SO8_RSI_WSClosing float, @SO8_RSI_WSVolume float, @SO8_RSI_WSWeightedClosing float, @SO8_RSI_WSTypicalClosing float, @SO8_RSI_WSVolumeAdjustedClosing float, @SO8_RSI_WSVolumeAdjustedWeightedClosing float, @SO8_RSI_WSVolumeAdjustedTypicalClosing float, @SO8_STDEVClosing float, @SO8_STDEVWeightedClosing float, @SO8_STDEVTypicalClosing float, @SO8_STDEVVolume float, @SO8_RVIClosing float, @SO8_RVIVolume float, @SO8_RVIWeightedClosing float, @SO8_RVITypicalClosing float, @SO8_RVIVolumeAdjustedClosing float, @SO8_RVIVolumeAdjustedWeightedClosing float, @SO8_RVIVolumeAdjustedTypicalClosing float
	declare @SO9_Opening float, @SO9_High float, @SO9_Low float, @SO9_Closing float, @SO9_Volume float, @SO9_SMAClosing float, @SO9_SMAWeightedClosing float, @SO9_SMATypicalClosing float, @SO9_SMAVolume float, @SO9_VAMAClosing float, @SO9_VAMATypicalClosing float, @SO9_VAMAWeightedClosing float, @SO9_WMAClosing float, @SO9_WMAWeightedClosing float, @SO9_WMATypicalClosing float, @SO9_EMAClosing float, @SO9_EMAWeightedClosing float, @SO9_EMATypicalClosing float, @SO9_EMAVolume float, @SO9_CMOClosing float, @SO9_CMOVolume float, @SO9_CMOWeightedClosing float, @SO9_CMOTypicalClosing float, @SO9_VMAClosing float, @SO9_VMAVolume float, @SO9_VMAWeightedClosing float, @SO9_VMATypicalClosing float, @SO9_WSClosing float, @SO9_WSWeightedClosing float, @SO9_WSTypicalClosing float, @SO9_WSWeightedNormalClosing float, @SO9_WSWeightedWeightedClosing float, @SO9_WSWeightedTypicalClosing float, @SO9_WSVolume float, @SO9_WSVolumeAdjustedClosing float, @SO9_WSVolumeAdjustedTypicalClosing float, @SO9_WSVolumeAdjustedWeightedClosing float, @SO9_RSI_SMAClosing float, @SO9_RSI_SMAVolume float, @SO9_RSI_SMAWeightedClosing float, @SO9_RSI_SMATypicalClosing float, @SO9_RSI_SMAVolumeAdjustedClosing float, @SO9_RSI_SMAVolumeAdjustedWeightedClosing float, @SO9_RSI_SMAVolumeAdjustedTypicalClosing float, @SO9_RSI_EMAClosing float, @SO9_RSI_EMAVolume float, @SO9_RSI_EMAWeightedClosing float, @SO9_RSI_EMATypicalClosing float, @SO9_RSI_EMAVolumeAdjustedClosing float, @SO9_RSI_EMAVolumeAdjustedWeightedClosing float, @SO9_RSI_EMAVolumeAdjustedTypicalClosing float, @SO9_RSI_WSClosing float, @SO9_RSI_WSVolume float, @SO9_RSI_WSWeightedClosing float, @SO9_RSI_WSTypicalClosing float, @SO9_RSI_WSVolumeAdjustedClosing float, @SO9_RSI_WSVolumeAdjustedWeightedClosing float, @SO9_RSI_WSVolumeAdjustedTypicalClosing float, @SO9_STDEVClosing float, @SO9_STDEVWeightedClosing float, @SO9_STDEVTypicalClosing float, @SO9_STDEVVolume float, @SO9_RVIClosing float, @SO9_RVIVolume float, @SO9_RVIWeightedClosing float, @SO9_RVITypicalClosing float, @SO9_RVIVolumeAdjustedClosing float, @SO9_RVIVolumeAdjustedWeightedClosing float, @SO9_RVIVolumeAdjustedTypicalClosing float

	/*Hold Period -1 Daily*/
	declare @A0_Date float, @A0_Opening float, @A0_High float, @A0_Low float, @A0_Closing float, @A0_Volume float, @A0_Interest float, @A0_GainClosing float, @A0_LossClosing float, @A0_GainVolume float, @A0_LossVolume float, @A0_WeightedClosing float, @A0_GainWeightedClosing float, @A0_LossWeightedClosing float, @A0_TypicalClosing float, @A0_GainTypicalClosing float, @A0_LossTypicalClosing float, @A0_VolumeAdjustedClosing float, @A0_GainVolumeAdjustedClosing float, @A0_LossVolumeAdjustedClosing float, @A0_VolumeAdjustedWeightedClosing float, @A0_GainVolumeAdjustedWeightedClosing float, @A0_LossVolumeAdjustedWeightedClosing float, @A0_VolumeAdjustedTypicalClosing float, @A0_GainVolumeAdjustedTypicalClosing float, @A0_LossVolumeAdjustedTypicalClosing float
	/*Hold Period Daily*/
	declare @A1_Date float, @A1_Opening float, @A1_High float, @A1_Low float, @A1_Closing float, @A1_Volume float, @A1_Interest float, @A1_GainClosing float, @A1_LossClosing float, @A1_GainVolume float, @A1_LossVolume float, @A1_WeightedClosing float, @A1_GainWeightedClosing float, @A1_LossWeightedClosing float, @A1_TypicalClosing float, @A1_GainTypicalClosing float, @A1_LossTypicalClosing float, @A1_VolumeAdjustedClosing float, @A1_GainVolumeAdjustedClosing float, @A1_LossVolumeAdjustedClosing float, @A1_VolumeAdjustedWeightedClosing float, @A1_GainVolumeAdjustedWeightedClosing float, @A1_LossVolumeAdjustedWeightedClosing float, @A1_VolumeAdjustedTypicalClosing float, @A1_GainVolumeAdjustedTypicalClosing float, @A1_LossVolumeAdjustedTypicalClosing float
	/*Hold Today's Daily*/
	declare @A2_Date float, @A2_Opening float, @A2_High float, @A2_Low float, @A2_Closing float, @A2_Volume float, @A2_Interest float, @A2_GainClosing float, @A2_LossClosing float, @A2_GainVolume float, @A2_LossVolume float, @A2_WeightedClosing float, @A2_GainWeightedClosing float, @A2_LossWeightedClosing float, @A2_TypicalClosing float, @A2_GainTypicalClosing float, @A2_LossTypicalClosing float, @A2_VolumeAdjustedClosing float, @A2_GainVolumeAdjustedClosing float, @A2_LossVolumeAdjustedClosing float, @A2_VolumeAdjustedWeightedClosing float, @A2_GainVolumeAdjustedWeightedClosing float, @A2_LossVolumeAdjustedWeightedClosing float, @A2_VolumeAdjustedTypicalClosing float, @A2_GainVolumeAdjustedTypicalClosing float, @A2_LossVolumeAdjustedTypicalClosing float
	/*Read Last Period*/
	select	@LastDateFrom =  cast(DateFrom as float), @LastDateTo =  cast(DateTo as float), @LastOpening =  isnull(Opening, 0) , @LastHigh =  isnull(High, 0) , @LastLow =  isnull(Low, 0) , @LastClosing =  isnull(Closing, 0) , @LastVolume =  isnull(Volume, 0) , 
			@LastTotalClosing =  isnull(TotalClosing, 0) , @LastTotalWeightedClosing =  isnull(TotalWeightedClosing, 0) , @LastTotalTypicalClosing =  isnull(TotalTypicalClosing, 0) , @LastTotalWeightedNormalClosing =  isnull(TotalWeightedNormalClosing, 0) , @LastTotalWeightedWeightedClosing =  isnull(TotalWeightedWeightedClosing, 0) , @LastTotalWeightedTypicalClosing =  isnull(TotalWeightedTypicalClosing, 0) , @LastTotalVolume =  isnull(TotalVolume, 0) , @LastTotalVolumeAdjustedClosing =  isnull(TotalVolumeAdjustedClosing, 0) , @LastTotalVolumeAdjustedTypicalClosing =  isnull(TotalVolumeAdjustedTypicalClosing, 0) , @LastTotalVolumeAdjustedWeightedClosing =  isnull(TotalVolumeAdjustedWeightedClosing, 0) , @LastTotalGainClosing =  isnull(TotalGainClosing, 0) , @LastTotalLossClosing =  isnull(TotalLossClosing, 0) , @LastTotalGainVolume =  isnull(TotalGainVolume, 0) , @LastTotalLossVolume =  isnull(TotalLossVolume, 0) , @LastTotalGainWeightedClosing =  isnull(TotalGainWeightedClosing, 0) , @LastTotalLossWeightedClosing =  isnull(TotalLossWeightedClosing, 0) , @LastTotalGainTypicalClosing =  isnull(TotalGainTypicalClosing, 0) , @LastTotalLossTypicalClosing =  isnull(TotalLossTypicalClosing, 0) , @LastTotalGainVolumeAdjustedClosing =  isnull(TotalGainVolumeAdjustedClosing, 0) , @LastTotalLossVolumeAdjustedClosing =  isnull(TotalLossVolumeAdjustedClosing, 0) , @LastTotalGainVolumeAdjustedWeightedClosing =  isnull(TotalGainVolumeAdjustedWeightedClosing, 0) , @LastTotalLossVolumeAdjustedWeightedClosing =  isnull(TotalLossVolumeAdjustedWeightedClosing, 0) , @LastTotalGainVolumeAdjustedTypicalClosing =  isnull(TotalGainVolumeAdjustedTypicalClosing, 0) , @LastTotalLossVolumeAdjustedTypicalClosing =  isnull(TotalLossVolumeAdjustedTypicalClosing, 0) , 
			@LastSMAClosing =  isnull(SMAClosing, 0) , @LastSMAWeightedClosing =  isnull(SMAWeightedClosing, 0) , @LastSMATypicalClosing =  isnull(SMATypicalClosing, 0) , @LastSMAVolume =  isnull(SMAVolume, 0) , @LastSMAGainClosing =  isnull(SMAGainClosing, 0) , @LastSMALossClosing =  isnull(SMALossClosing, 0) , @LastSMAGainVolume =  isnull(SMAGainVolume, 0) , @LastSMALossVolume =  isnull(SMALossVolume, 0) , @LastSMAGainWeightedClosing =  isnull(SMAGainWeightedClosing, 0) , @LastSMALossWeightedClosing =  isnull(SMALossWeightedClosing, 0) , @LastSMAGainTypicalClosing =  isnull(SMAGainTypicalClosing, 0) , @LastSMALossTypicalClosing =  isnull(SMALossTypicalClosing, 0) , @LastSMAGainVolumeAdjustedClosing =  isnull(SMAGainVolumeAdjustedClosing, 0) , @LastSMALossVolumeAdjustedClosing =  isnull(SMALossVolumeAdjustedClosing, 0) , @LastSMAGainVolumeAdjustedWeightedClosing =  isnull(SMAGainVolumeAdjustedWeightedClosing, 0) , @LastSMALossVolumeAdjustedWeightedClosing =  isnull(SMALossVolumeAdjustedWeightedClosing, 0) , @LastSMAGainVolumeAdjustedTypicalClosing =  isnull(SMAGainVolumeAdjustedTypicalClosing, 0) , @LastSMALossVolumeAdjustedTypicalClosing =  isnull(SMALossVolumeAdjustedTypicalClosing, 0) , 
			@LastVAMAClosing =  isnull(VAMAClosing, 0) , @LastVAMATypicalClosing =  isnull(VAMATypicalClosing, 0) , @LastVAMAWeightedClosing =  isnull(VAMAWeightedClosing, 0) , 
			@LastWMAClosing =  isnull(WMAClosing, 0) , @LastWMAWeightedClosing =  isnull(WMAWeightedClosing, 0) , @LastWMATypicalClosing =  isnull(WMATypicalClosing, 0) , 
			@LastEMAClosing =  isnull(EMAClosing, 0) , @LastEMAWeightedClosing =  isnull(EMAWeightedClosing, 0) , @LastEMATypicalClosing =  isnull(EMATypicalClosing, 0) , @LastEMAVolume =  isnull(EMAVolume, 0) , @LastEMAGainClosing =  isnull(EMAGainClosing, 0) , @LastEMALossClosing =  isnull(EMALossClosing, 0) , @LastEMAGainVolume =  isnull(EMAGainVolume, 0) , @LastEMALossVolume =  isnull(EMALossVolume, 0) , @LastEMAGainWeightedClosing =  isnull(EMAGainWeightedClosing, 0) , @LastEMALossWeightedClosing =  isnull(EMALossWeightedClosing, 0) , @LastEMAGainTypicalClosing =  isnull(EMAGainTypicalClosing, 0) , @LastEMALossTypicalClosing =  isnull(EMALossTypicalClosing, 0) , @LastEMAGainVolumeAdjustedClosing =  isnull(EMAGainVolumeAdjustedClosing, 0) , @LastEMALossVolumeAdjustedClosing =  isnull(EMALossVolumeAdjustedClosing, 0) , @LastEMAGainVolumeAdjustedWeightedClosing =  isnull(EMAGainVolumeAdjustedWeightedClosing, 0) , @LastEMALossVolumeAdjustedWeightedClosing =  isnull(EMALossVolumeAdjustedWeightedClosing, 0) , @LastEMAGainVolumeAdjustedTypicalClosing =  isnull(EMAGainVolumeAdjustedTypicalClosing, 0) , @LastEMALossVolumeAdjustedTypicalClosing =  isnull(EMALossVolumeAdjustedTypicalClosing, 0) , 
			@LastCMOClosing =  isnull(CMOClosing, 0) , @LastCMOVolume =  isnull(CMOVolume, 0) , @LastCMOWeightedClosing =  isnull(CMOWeightedClosing, 0) , @LastCMOTypicalClosing =  isnull(CMOTypicalClosing, 0) , 
			@LastVMAClosing =  isnull(VMAClosing, 0) , @LastVMAVolume =  isnull(VMAVolume, 0) , @LastVMAWeightedClosing =  isnull(VMAWeightedClosing, 0) , @LastVMATypicalClosing =  isnull(VMATypicalClosing, 0) , 
			@LastWSClosing =  isnull(WSClosing, 0) , @LastWSWeightedClosing =  isnull(WSWeightedClosing, 0) , @LastWSTypicalClosing =  isnull(WSTypicalClosing, 0) , @LastWSWeightedNormalClosing =  isnull(WSWeightedNormalClosing, 0) , @LastWSWeightedWeightedClosing =  isnull(WSWeightedWeightedClosing, 0) , @LastWSWeightedTypicalClosing =  isnull(WSWeightedTypicalClosing, 0) , @LastWSVolume =  isnull(WSVolume, 0) , @LastWSVolumeAdjustedClosing =  isnull(WSVolumeAdjustedClosing, 0) , @LastWSVolumeAdjustedTypicalClosing =  isnull(WSVolumeAdjustedTypicalClosing, 0) , @LastWSVolumeAdjustedWeightedClosing =  isnull(WSVolumeAdjustedWeightedClosing, 0) , @LastWSGainClosing =  isnull(WSGainClosing, 0) , @LastWSLossClosing =  isnull(WSLossClosing, 0) , @LastWSGainVolume =  isnull(WSGainVolume, 0) , @LastWSLossVolume =  isnull(WSLossVolume, 0) , @LastWSGainWeightedClosing =  isnull(WSGainWeightedClosing, 0) , @LastWSLossWeightedClosing =  isnull(WSLossWeightedClosing, 0) , @LastWSGainTypicalClosing =  isnull(WSGainTypicalClosing, 0) , @LastWSLossTypicalClosing =  isnull(WSLossTypicalClosing, 0) , @LastWSGainVolumeAdjustedClosing =  isnull(WSGainVolumeAdjustedClosing, 0) , @LastWSLossVolumeAdjustedClosing =  isnull(WSLossVolumeAdjustedClosing, 0) , @LastWSGainVolumeAdjustedWeightedClosing =  isnull(WSGainVolumeAdjustedWeightedClosing, 0) , @LastWSLossVolumeAdjustedWeightedClosing =  isnull(WSLossVolumeAdjustedWeightedClosing, 0) , @LastWSGainVolumeAdjustedTypicalClosing =  isnull(WSGainVolumeAdjustedTypicalClosing, 0) , @LastWSLossVolumeAdjustedTypicalClosing =  isnull(WSLossVolumeAdjustedTypicalClosing, 0) , 
			@LastRSI_SMAClosing =  isnull(RSI_SMAClosing, 0) , @LastRSI_SMAVolume =  isnull(RSI_SMAVolume, 0) , @LastRSI_SMAWeightedClosing =  isnull(RSI_SMAWeightedClosing, 0) , @LastRSI_SMATypicalClosing =  isnull(RSI_SMATypicalClosing, 0) , @LastRSI_SMAVolumeAdjustedClosing =  isnull(RSI_SMAVolumeAdjustedClosing, 0) , @LastRSI_SMAVolumeAdjustedWeightedClosing =  isnull(RSI_SMAVolumeAdjustedWeightedClosing, 0) , @LastRSI_SMAVolumeAdjustedTypicalClosing =  isnull(RSI_SMAVolumeAdjustedTypicalClosing, 0) , @LastRSI_EMAClosing =  isnull(RSI_EMAClosing, 0) , @LastRSI_EMAVolume =  isnull(RSI_EMAVolume, 0) , @LastRSI_EMAWeightedClosing =  isnull(RSI_EMAWeightedClosing, 0) , @LastRSI_EMATypicalClosing =  isnull(RSI_EMATypicalClosing, 0) , @LastRSI_EMAVolumeAdjustedClosing =  isnull(RSI_EMAVolumeAdjustedClosing, 0) , @LastRSI_EMAVolumeAdjustedWeightedClosing =  isnull(RSI_EMAVolumeAdjustedWeightedClosing, 0) , @LastRSI_EMAVolumeAdjustedTypicalClosing =  isnull(RSI_EMAVolumeAdjustedTypicalClosing, 0) , @LastRSI_WSClosing =  isnull(RSI_WSClosing, 0) , @LastRSI_WSVolume =  isnull(RSI_WSVolume, 0) , @LastRSI_WSWeightedClosing =  isnull(RSI_WSWeightedClosing, 0) , @LastRSI_WSTypicalClosing =  isnull(RSI_WSTypicalClosing, 0) , @LastRSI_WSVolumeAdjustedClosing =  isnull(RSI_WSVolumeAdjustedClosing, 0) , @LastRSI_WSVolumeAdjustedWeightedClosing =  isnull(RSI_WSVolumeAdjustedWeightedClosing, 0) , @LastRSI_WSVolumeAdjustedTypicalClosing =  isnull(RSI_WSVolumeAdjustedTypicalClosing, 0),
			@LastSTDEVClosing = isnull(STDEVClosing, 0), @LastSTDEVWeightedClosing = isnull(STDEVWeightedClosing, 0), @LastSTDEVTypicalClosing = isnull(STDEVTypicalClosing, 0), @LastSTDEVVolume = isnull(STDEVVolume, 0), @LastSTDEVGainClosing = isnull(STDEVGainClosing, 0), @LastSTDEVLossClosing = isnull(STDEVLossClosing, 0), @LastSTDEVGainVolume = isnull(STDEVGainVolume, 0), @LastSTDEVLossVolume = isnull(STDEVLossVolume, 0), @LastSTDEVGainWeightedClosing = isnull(STDEVGainWeightedClosing, 0), @LastSTDEVLossWeightedClosing = isnull(STDEVLossWeightedClosing, 0), @LastSTDEVGainTypicalClosing = isnull(STDEVGainTypicalClosing, 0), @LastSTDEVLossTypicalClosing = isnull(STDEVLossTypicalClosing, 0), @LastSTDEVGainVolumeAdjustedClosing = isnull(STDEVGainVolumeAdjustedClosing, 0), @LastSTDEVLossVolumeAdjustedClosing = isnull(STDEVLossVolumeAdjustedClosing, 0), @LastSTDEVGainVolumeAdjustedWeightedClosing = isnull(STDEVGainVolumeAdjustedWeightedClosing, 0), @LastSTDEVLossVolumeAdjustedWeightedClosing = isnull(STDEVLossVolumeAdjustedWeightedClosing, 0), @LastSTDEVGainVolumeAdjustedTypicalClosing = isnull(STDEVGainVolumeAdjustedTypicalClosing, 0), @LastSTDEVLossVolumeAdjustedTypicalClosing = isnull(STDEVLossVolumeAdjustedTypicalClosing, 0),
			@LastRVIClosing = isnull(RVIClosing, 0), @LastRVIVolume = isnull(RVIVolume, 0), @LastRVIWeightedClosing = isnull(RVIWeightedClosing, 0), @LastRVITypicalClosing = isnull(RVITypicalClosing, 0), @LastRVIVolumeAdjustedClosing = isnull(RVIVolumeAdjustedClosing, 0), @LastRVIVolumeAdjustedWeightedClosing = isnull(RVIVolumeAdjustedWeightedClosing, 0), @LastRVIVolumeAdjustedTypicalClosing = isnull(RVIVolumeAdjustedTypicalClosing, 0),
			@LastHighestOpening =  isnull(HighestOpening, 0) , @LastLowestOpening =  isnull(LowestOpening, 0) , @LastHighestHigh =  isnull(HighestHigh, 0) , @LastLowestHigh =  isnull(LowestHigh, 0) , @LastHighestLow =  isnull(HighestLow, 0) , @LastLowestLow =  isnull(LowestLow, 0) , @LastHighestClosing =  isnull(HighestClosing, 0) , @LastLowestClosing =  isnull(LowestClosing, 0) , @LastHighestVolume =  isnull(HighestVolume, 0) , @LastLowestVolume =  isnull(LowestVolume, 0) , @LastHighestSMAClosing =  isnull(HighestSMAClosing, 0) , @LastLowestSMAClosing =  isnull(LowestSMAClosing, 0) , @LastHighestSMAWeightedClosing =  isnull(HighestSMAWeightedClosing, 0) , @LastLowestSMAWeightedClosing =  isnull(LowestSMAWeightedClosing, 0) , @LastHighestSMATypicalClosing =  isnull(HighestSMATypicalClosing, 0) , @LastLowestSMATypicalClosing =  isnull(LowestSMATypicalClosing, 0) , @LastHighestSMAVolume =  isnull(HighestSMAVolume, 0) , @LastLowestSMAVolume =  isnull(LowestSMAVolume, 0) , @LastHighestVAMAClosing =  isnull(HighestVAMAClosing, 0) , @LastLowestVAMAClosing =  isnull(LowestVAMAClosing, 0) , @LastHighestVAMATypicalClosing =  isnull(HighestVAMATypicalClosing, 0) , @LastLowestVAMATypicalClosing =  isnull(LowestVAMATypicalClosing, 0) , @LastHighestVAMAWeightedClosing =  isnull(HighestVAMAWeightedClosing, 0) , @LastLowestVAMAWeightedClosing =  isnull(LowestVAMAWeightedClosing, 0) , @LastHighestWMAClosing =  isnull(HighestWMAClosing, 0) , @LastLowestWMAClosing =  isnull(LowestWMAClosing, 0) , @LastHighestWMAWeightedClosing =  isnull(HighestWMAWeightedClosing, 0) , @LastLowestWMAWeightedClosing =  isnull(LowestWMAWeightedClosing, 0) , @LastHighestWMATypicalClosing =  isnull(HighestWMATypicalClosing, 0) , @LastLowestWMATypicalClosing =  isnull(LowestWMATypicalClosing, 0) , @LastHighestEMAClosing =  isnull(HighestEMAClosing, 0) , @LastLowestEMAClosing =  isnull(LowestEMAClosing, 0) , @LastHighestEMAWeightedClosing =  isnull(HighestEMAWeightedClosing, 0) , @LastLowestEMAWeightedClosing =  isnull(LowestEMAWeightedClosing, 0) , @LastHighestEMATypicalClosing =  isnull(HighestEMATypicalClosing, 0) , @LastLowestEMATypicalClosing =  isnull(LowestEMATypicalClosing, 0) , @LastHighestEMAVolume =  isnull(HighestEMAVolume, 0) , @LastLowestEMAVolume =  isnull(LowestEMAVolume, 0) , @LastHighestCMOClosing =  isnull(HighestCMOClosing, 0) , @LastLowestCMOClosing =  isnull(LowestCMOClosing, 0) , @LastHighestCMOVolume =  isnull(HighestCMOVolume, 0) , @LastLowestCMOVolume =  isnull(LowestCMOVolume, 0) , @LastHighestCMOWeightedClosing =  isnull(HighestCMOWeightedClosing, 0) , @LastLowestCMOWeightedClosing =  isnull(LowestCMOWeightedClosing, 0) , @LastHighestCMOTypicalClosing =  isnull(HighestCMOTypicalClosing, 0) , @LastLowestCMOTypicalClosing =  isnull(LowestCMOTypicalClosing, 0) , @LastHighestVMAClosing =  isnull(HighestVMAClosing, 0) , @LastLowestVMAClosing =  isnull(LowestVMAClosing, 0) , @LastHighestVMAVolume =  isnull(HighestVMAVolume, 0) , @LastLowestVMAVolume =  isnull(LowestVMAVolume, 0) , @LastHighestVMAWeightedClosing =  isnull(HighestVMAWeightedClosing, 0) , @LastLowestVMAWeightedClosing =  isnull(LowestVMAWeightedClosing, 0) , @LastHighestVMATypicalClosing =  isnull(HighestVMATypicalClosing, 0) , @LastLowestVMATypicalClosing =  isnull(LowestVMATypicalClosing, 0) , @LastHighestWSClosing =  isnull(HighestWSClosing, 0) , @LastLowestWSClosing =  isnull(LowestWSClosing, 0) , @LastHighestWSWeightedClosing =  isnull(HighestWSWeightedClosing, 0) , @LastLowestWSWeightedClosing =  isnull(LowestWSWeightedClosing, 0) , @LastHighestWSTypicalClosing =  isnull(HighestWSTypicalClosing, 0) , @LastLowestWSTypicalClosing =  isnull(LowestWSTypicalClosing, 0) , @LastHighestWSWeightedNormalClosing =  isnull(HighestWSWeightedNormalClosing, 0) , @LastLowestWSWeightedNormalClosing =  isnull(LowestWSWeightedNormalClosing, 0) , @LastHighestWSWeightedWeightedClosing =  isnull(HighestWSWeightedWeightedClosing, 0) , @LastLowestWSWeightedWeightedClosing =  isnull(LowestWSWeightedWeightedClosing, 0) , @LastHighestWSWeightedTypicalClosing =  isnull(HighestWSWeightedTypicalClosing, 0) , @LastLowestWSWeightedTypicalClosing =  isnull(LowestWSWeightedTypicalClosing, 0) , @LastHighestWSVolume =  isnull(HighestWSVolume, 0) , @LastLowestWSVolume =  isnull(LowestWSVolume, 0) , @LastHighestWSVolumeAdjustedClosing =  isnull(HighestWSVolumeAdjustedClosing, 0) , @LastLowestWSVolumeAdjustedClosing =  isnull(LowestWSVolumeAdjustedClosing, 0) , @LastHighestWSVolumeAdjustedTypicalClosing =  isnull(HighestWSVolumeAdjustedTypicalClosing, 0) , @LastLowestWSVolumeAdjustedTypicalClosing =  isnull(LowestWSVolumeAdjustedTypicalClosing, 0) , @LastHighestWSVolumeAdjustedWeightedClosing =  isnull(HighestWSVolumeAdjustedWeightedClosing, 0) , @LastLowestWSVolumeAdjustedWeightedClosing =  isnull(LowestWSVolumeAdjustedWeightedClosing, 0) , @LastHighestRSI_SMAClosing =  isnull(HighestRSI_SMAClosing, 0) , @LastLowestRSI_SMAClosing =  isnull(LowestRSI_SMAClosing, 0) , @LastHighestRSI_SMAVolume =  isnull(HighestRSI_SMAVolume, 0) , @LastLowestRSI_SMAVolume =  isnull(LowestRSI_SMAVolume, 0) , @LastHighestRSI_SMAWeightedClosing =  isnull(HighestRSI_SMAWeightedClosing, 0) , @LastLowestRSI_SMAWeightedClosing =  isnull(LowestRSI_SMAWeightedClosing, 0) , @LastHighestRSI_SMATypicalClosing =  isnull(HighestRSI_SMATypicalClosing, 0) , @LastLowestRSI_SMATypicalClosing =  isnull(LowestRSI_SMATypicalClosing, 0) , @LastHighestRSI_SMAVolumeAdjustedClosing =  isnull(HighestRSI_SMAVolumeAdjustedClosing, 0) , @LastLowestRSI_SMAVolumeAdjustedClosing =  isnull(LowestRSI_SMAVolumeAdjustedClosing, 0) , @LastHighestRSI_SMAVolumeAdjustedWeightedClosing =  isnull(HighestRSI_SMAVolumeAdjustedWeightedClosing, 0) , @LastLowestRSI_SMAVolumeAdjustedWeightedClosing =  isnull(LowestRSI_SMAVolumeAdjustedWeightedClosing, 0) , @LastHighestRSI_SMAVolumeAdjustedTypicalClosing =  isnull(HighestRSI_SMAVolumeAdjustedTypicalClosing, 0) , @LastLowestRSI_SMAVolumeAdjustedTypicalClosing =  isnull(LowestRSI_SMAVolumeAdjustedTypicalClosing, 0) , @LastHighestRSI_EMAClosing =  isnull(HighestRSI_EMAClosing, 0) , @LastLowestRSI_EMAClosing =  isnull(LowestRSI_EMAClosing, 0) , @LastHighestRSI_EMAVolume =  isnull(HighestRSI_EMAVolume, 0) , @LastLowestRSI_EMAVolume =  isnull(LowestRSI_EMAVolume, 0) , @LastHighestRSI_EMAWeightedClosing =  isnull(HighestRSI_EMAWeightedClosing, 0) , @LastLowestRSI_EMAWeightedClosing =  isnull(LowestRSI_EMAWeightedClosing, 0) , @LastHighestRSI_EMATypicalClosing =  isnull(HighestRSI_EMATypicalClosing, 0) , @LastLowestRSI_EMATypicalClosing =  isnull(LowestRSI_EMATypicalClosing, 0) , @LastHighestRSI_EMAVolumeAdjustedClosing =  isnull(HighestRSI_EMAVolumeAdjustedClosing, 0) , @LastLowestRSI_EMAVolumeAdjustedClosing =  isnull(LowestRSI_EMAVolumeAdjustedClosing, 0) , @LastHighestRSI_EMAVolumeAdjustedWeightedClosing =  isnull(HighestRSI_EMAVolumeAdjustedWeightedClosing, 0) , @LastLowestRSI_EMAVolumeAdjustedWeightedClosing =  isnull(LowestRSI_EMAVolumeAdjustedWeightedClosing, 0) , @LastHighestRSI_EMAVolumeAdjustedTypicalClosing =  isnull(HighestRSI_EMAVolumeAdjustedTypicalClosing, 0) , @LastLowestRSI_EMAVolumeAdjustedTypicalClosing =  isnull(LowestRSI_EMAVolumeAdjustedTypicalClosing, 0) , @LastHighestRSI_WSClosing =  isnull(HighestRSI_WSClosing, 0) , @LastLowestRSI_WSClosing =  isnull(LowestRSI_WSClosing, 0) , @LastHighestRSI_WSVolume =  isnull(HighestRSI_WSVolume, 0) , @LastLowestRSI_WSVolume =  isnull(LowestRSI_WSVolume, 0) , @LastHighestRSI_WSWeightedClosing =  isnull(HighestRSI_WSWeightedClosing, 0) , @LastLowestRSI_WSWeightedClosing =  isnull(LowestRSI_WSWeightedClosing, 0) , @LastHighestRSI_WSTypicalClosing =  isnull(HighestRSI_WSTypicalClosing, 0) , @LastLowestRSI_WSTypicalClosing =  isnull(LowestRSI_WSTypicalClosing, 0) , @LastHighestRSI_WSVolumeAdjustedClosing =  isnull(HighestRSI_WSVolumeAdjustedClosing, 0) , @LastLowestRSI_WSVolumeAdjustedClosing =  isnull(LowestRSI_WSVolumeAdjustedClosing, 0) , @LastHighestRSI_WSVolumeAdjustedWeightedClosing =  isnull(HighestRSI_WSVolumeAdjustedWeightedClosing, 0) , @LastLowestRSI_WSVolumeAdjustedWeightedClosing =  isnull(LowestRSI_WSVolumeAdjustedWeightedClosing, 0) , @LastHighestRSI_WSVolumeAdjustedTypicalClosing =  isnull(HighestRSI_WSVolumeAdjustedTypicalClosing, 0) , @LastLowestRSI_WSVolumeAdjustedTypicalClosing =  isnull(LowestRSI_WSVolumeAdjustedTypicalClosing, 0) , @LastHighestSTDEVClosing =  isnull(HighestSTDEVClosing, 0) , @LastLowestSTDEVClosing =  isnull(LowestSTDEVClosing, 0) , @LastHighestSTDEVWeightedClosing =  isnull(HighestSTDEVWeightedClosing, 0) , @LastLowestSTDEVWeightedClosing =  isnull(LowestSTDEVWeightedClosing, 0) , @LastHighestSTDEVTypicalClosing =  isnull(HighestSTDEVTypicalClosing, 0) , @LastLowestSTDEVTypicalClosing =  isnull(LowestSTDEVTypicalClosing, 0) , @LastHighestSTDEVVolume =  isnull(HighestSTDEVVolume, 0) , @LastLowestSTDEVVolume =  isnull(LowestSTDEVVolume, 0) , @LastHighestRVIClosing =  isnull(HighestRVIClosing, 0) , @LastLowestRVIClosing =  isnull(LowestRVIClosing, 0) , @LastHighestRVIVolume =  isnull(HighestRVIVolume, 0) , @LastLowestRVIVolume =  isnull(LowestRVIVolume, 0) , @LastHighestRVIWeightedClosing =  isnull(HighestRVIWeightedClosing, 0) , @LastLowestRVIWeightedClosing =  isnull(LowestRVIWeightedClosing, 0) , @LastHighestRVITypicalClosing =  isnull(HighestRVITypicalClosing, 0) , @LastLowestRVITypicalClosing =  isnull(LowestRVITypicalClosing, 0) , @LastHighestRVIVolumeAdjustedClosing =  isnull(HighestRVIVolumeAdjustedClosing, 0) , @LastLowestRVIVolumeAdjustedClosing =  isnull(LowestRVIVolumeAdjustedClosing, 0) , @LastHighestRVIVolumeAdjustedWeightedClosing =  isnull(HighestRVIVolumeAdjustedWeightedClosing, 0) , @LastLowestRVIVolumeAdjustedWeightedClosing =  isnull(LowestRVIVolumeAdjustedWeightedClosing, 0) , @LastHighestRVIVolumeAdjustedTypicalClosing =  isnull(HighestRVIVolumeAdjustedTypicalClosing, 0) , @LastLowestRVIVolumeAdjustedTypicalClosing =  isnull(LowestRVIVolumeAdjustedTypicalClosing, 0) , 
			@LastSO_Opening =  isnull(SO_Opening, 0) , @LastSO_High =  isnull(SO_High, 0) , @LastSO_Low =  isnull(SO_Low, 0) , @LastSO_Closing =  isnull(SO_Closing, 0) , @LastSO_Volume =  isnull(SO_Volume, 0) , @LastSO_SMAClosing =  isnull(SO_SMAClosing, 0) , @LastSO_SMAWeightedClosing =  isnull(SO_SMAWeightedClosing, 0) , @LastSO_SMATypicalClosing =  isnull(SO_SMATypicalClosing, 0) , @LastSO_SMAVolume =  isnull(SO_SMAVolume, 0) , @LastSO_VAMAClosing =  isnull(SO_VAMAClosing, 0) , @LastSO_VAMATypicalClosing =  isnull(SO_VAMATypicalClosing, 0) , @LastSO_VAMAWeightedClosing =  isnull(SO_VAMAWeightedClosing, 0) , @LastSO_WMAClosing =  isnull(SO_WMAClosing, 0) , @LastSO_WMAWeightedClosing =  isnull(SO_WMAWeightedClosing, 0) , @LastSO_WMATypicalClosing =  isnull(SO_WMATypicalClosing, 0) , @LastSO_EMAClosing =  isnull(SO_EMAClosing, 0) , @LastSO_EMAWeightedClosing =  isnull(SO_EMAWeightedClosing, 0) , @LastSO_EMATypicalClosing =  isnull(SO_EMATypicalClosing, 0) , @LastSO_EMAVolume =  isnull(SO_EMAVolume, 0) , @LastSO_CMOClosing =  isnull(SO_CMOClosing, 0) , @LastSO_CMOVolume =  isnull(SO_CMOVolume, 0) , @LastSO_CMOWeightedClosing =  isnull(SO_CMOWeightedClosing, 0) , @LastSO_CMOTypicalClosing =  isnull(SO_CMOTypicalClosing, 0) , @LastSO_VMAClosing =  isnull(SO_VMAClosing, 0) , @LastSO_VMAVolume =  isnull(SO_VMAVolume, 0) , @LastSO_VMAWeightedClosing =  isnull(SO_VMAWeightedClosing, 0) , @LastSO_VMATypicalClosing =  isnull(SO_VMATypicalClosing, 0) , @LastSO_WSClosing =  isnull(SO_WSClosing, 0) , @LastSO_WSWeightedClosing =  isnull(SO_WSWeightedClosing, 0) , @LastSO_WSTypicalClosing =  isnull(SO_WSTypicalClosing, 0) , @LastSO_WSWeightedNormalClosing =  isnull(SO_WSWeightedNormalClosing, 0) , @LastSO_WSWeightedWeightedClosing =  isnull(SO_WSWeightedWeightedClosing, 0) , @LastSO_WSWeightedTypicalClosing =  isnull(SO_WSWeightedTypicalClosing, 0) , @LastSO_WSVolume =  isnull(SO_WSVolume, 0) , @LastSO_WSVolumeAdjustedClosing =  isnull(SO_WSVolumeAdjustedClosing, 0) , @LastSO_WSVolumeAdjustedTypicalClosing =  isnull(SO_WSVolumeAdjustedTypicalClosing, 0) , @LastSO_WSVolumeAdjustedWeightedClosing =  isnull(SO_WSVolumeAdjustedWeightedClosing, 0) , @LastSO_RSI_SMAClosing =  isnull(SO_RSI_SMAClosing, 0) , @LastSO_RSI_SMAVolume =  isnull(SO_RSI_SMAVolume, 0) , @LastSO_RSI_SMAWeightedClosing =  isnull(SO_RSI_SMAWeightedClosing, 0) , @LastSO_RSI_SMATypicalClosing =  isnull(SO_RSI_SMATypicalClosing, 0) , @LastSO_RSI_SMAVolumeAdjustedClosing =  isnull(SO_RSI_SMAVolumeAdjustedClosing, 0) , @LastSO_RSI_SMAVolumeAdjustedWeightedClosing =  isnull(SO_RSI_SMAVolumeAdjustedWeightedClosing, 0) , @LastSO_RSI_SMAVolumeAdjustedTypicalClosing =  isnull(SO_RSI_SMAVolumeAdjustedTypicalClosing, 0) , @LastSO_RSI_EMAClosing =  isnull(SO_RSI_EMAClosing, 0) , @LastSO_RSI_EMAVolume =  isnull(SO_RSI_EMAVolume, 0) , @LastSO_RSI_EMAWeightedClosing =  isnull(SO_RSI_EMAWeightedClosing, 0) , @LastSO_RSI_EMATypicalClosing =  isnull(SO_RSI_EMATypicalClosing, 0) , @LastSO_RSI_EMAVolumeAdjustedClosing =  isnull(SO_RSI_EMAVolumeAdjustedClosing, 0) , @LastSO_RSI_EMAVolumeAdjustedWeightedClosing =  isnull(SO_RSI_EMAVolumeAdjustedWeightedClosing, 0) , @LastSO_RSI_EMAVolumeAdjustedTypicalClosing =  isnull(SO_RSI_EMAVolumeAdjustedTypicalClosing, 0) , @LastSO_RSI_WSClosing =  isnull(SO_RSI_WSClosing, 0) , @LastSO_RSI_WSVolume =  isnull(SO_RSI_WSVolume, 0) , @LastSO_RSI_WSWeightedClosing =  isnull(SO_RSI_WSWeightedClosing, 0) , @LastSO_RSI_WSTypicalClosing =  isnull(SO_RSI_WSTypicalClosing, 0) , @LastSO_RSI_WSVolumeAdjustedClosing =  isnull(SO_RSI_WSVolumeAdjustedClosing, 0) , @LastSO_RSI_WSVolumeAdjustedWeightedClosing =  isnull(SO_RSI_WSVolumeAdjustedWeightedClosing, 0) , @LastSO_RSI_WSVolumeAdjustedTypicalClosing =  isnull(SO_RSI_WSVolumeAdjustedTypicalClosing, 0) , @LastSO_STDEVClosing =  isnull(SO_STDEVClosing, 0) , @LastSO_STDEVWeightedClosing =  isnull(SO_STDEVWeightedClosing, 0) , @LastSO_STDEVTypicalClosing =  isnull(SO_STDEVTypicalClosing, 0) , @LastSO_STDEVVolume =  isnull(SO_STDEVVolume, 0) , @LastSO_RVIClosing =  isnull(SO_RVIClosing, 0) , @LastSO_RVIVolume =  isnull(SO_RVIVolume, 0) , @LastSO_RVIWeightedClosing =  isnull(SO_RVIWeightedClosing, 0) , @LastSO_RVITypicalClosing =  isnull(SO_RVITypicalClosing, 0) , @LastSO_RVIVolumeAdjustedClosing =  isnull(SO_RVIVolumeAdjustedClosing, 0) , @LastSO_RVIVolumeAdjustedWeightedClosing =  isnull(SO_RVIVolumeAdjustedWeightedClosing, 0) , @LastSO_RVIVolumeAdjustedTypicalClosing =  isnull(SO_RVIVolumeAdjustedTypicalClosing, 0) , 
			@LastSO3_Opening =  isnull(SO3_Opening, 0) , @LastSO3_High =  isnull(SO3_High, 0) , @LastSO3_Low =  isnull(SO3_Low, 0) , @LastSO3_Closing =  isnull(SO3_Closing, 0) , @LastSO3_Volume =  isnull(SO3_Volume, 0) , @LastSO3_SMAClosing =  isnull(SO3_SMAClosing, 0) , @LastSO3_SMAWeightedClosing =  isnull(SO3_SMAWeightedClosing, 0) , @LastSO3_SMATypicalClosing =  isnull(SO3_SMATypicalClosing, 0) , @LastSO3_SMAVolume =  isnull(SO3_SMAVolume, 0) , @LastSO3_VAMAClosing =  isnull(SO3_VAMAClosing, 0) , @LastSO3_VAMATypicalClosing =  isnull(SO3_VAMATypicalClosing, 0) , @LastSO3_VAMAWeightedClosing =  isnull(SO3_VAMAWeightedClosing, 0) , @LastSO3_WMAClosing =  isnull(SO3_WMAClosing, 0) , @LastSO3_WMAWeightedClosing =  isnull(SO3_WMAWeightedClosing, 0) , @LastSO3_WMATypicalClosing =  isnull(SO3_WMATypicalClosing, 0) , @LastSO3_EMAClosing =  isnull(SO3_EMAClosing, 0) , @LastSO3_EMAWeightedClosing =  isnull(SO3_EMAWeightedClosing, 0) , @LastSO3_EMATypicalClosing =  isnull(SO3_EMATypicalClosing, 0) , @LastSO3_EMAVolume =  isnull(SO3_EMAVolume, 0) , @LastSO3_CMOClosing =  isnull(SO3_CMOClosing, 0) , @LastSO3_CMOVolume =  isnull(SO3_CMOVolume, 0) , @LastSO3_CMOWeightedClosing =  isnull(SO3_CMOWeightedClosing, 0) , @LastSO3_CMOTypicalClosing =  isnull(SO3_CMOTypicalClosing, 0) , @LastSO3_VMAClosing =  isnull(SO3_VMAClosing, 0) , @LastSO3_VMAVolume =  isnull(SO3_VMAVolume, 0) , @LastSO3_VMAWeightedClosing =  isnull(SO3_VMAWeightedClosing, 0) , @LastSO3_VMATypicalClosing =  isnull(SO3_VMATypicalClosing, 0) , @LastSO3_WSClosing =  isnull(SO3_WSClosing, 0) , @LastSO3_WSWeightedClosing =  isnull(SO3_WSWeightedClosing, 0) , @LastSO3_WSTypicalClosing =  isnull(SO3_WSTypicalClosing, 0) , @LastSO3_WSWeightedNormalClosing =  isnull(SO3_WSWeightedNormalClosing, 0) , @LastSO3_WSWeightedWeightedClosing =  isnull(SO3_WSWeightedWeightedClosing, 0) , @LastSO3_WSWeightedTypicalClosing =  isnull(SO3_WSWeightedTypicalClosing, 0) , @LastSO3_WSVolume =  isnull(SO3_WSVolume, 0) , @LastSO3_WSVolumeAdjustedClosing =  isnull(SO3_WSVolumeAdjustedClosing, 0) , @LastSO3_WSVolumeAdjustedTypicalClosing =  isnull(SO3_WSVolumeAdjustedTypicalClosing, 0) , @LastSO3_WSVolumeAdjustedWeightedClosing =  isnull(SO3_WSVolumeAdjustedWeightedClosing, 0) , @LastSO3_RSI_SMAClosing =  isnull(SO3_RSI_SMAClosing, 0) , @LastSO3_RSI_SMAVolume =  isnull(SO3_RSI_SMAVolume, 0) , @LastSO3_RSI_SMAWeightedClosing =  isnull(SO3_RSI_SMAWeightedClosing, 0) , @LastSO3_RSI_SMATypicalClosing =  isnull(SO3_RSI_SMATypicalClosing, 0) , @LastSO3_RSI_SMAVolumeAdjustedClosing =  isnull(SO3_RSI_SMAVolumeAdjustedClosing, 0) , @LastSO3_RSI_SMAVolumeAdjustedWeightedClosing =  isnull(SO3_RSI_SMAVolumeAdjustedWeightedClosing, 0) , @LastSO3_RSI_SMAVolumeAdjustedTypicalClosing =  isnull(SO3_RSI_SMAVolumeAdjustedTypicalClosing, 0) , @LastSO3_RSI_EMAClosing =  isnull(SO3_RSI_EMAClosing, 0) , @LastSO3_RSI_EMAVolume =  isnull(SO3_RSI_EMAVolume, 0) , @LastSO3_RSI_EMAWeightedClosing =  isnull(SO3_RSI_EMAWeightedClosing, 0) , @LastSO3_RSI_EMATypicalClosing =  isnull(SO3_RSI_EMATypicalClosing, 0) , @LastSO3_RSI_EMAVolumeAdjustedClosing =  isnull(SO3_RSI_EMAVolumeAdjustedClosing, 0) , @LastSO3_RSI_EMAVolumeAdjustedWeightedClosing =  isnull(SO3_RSI_EMAVolumeAdjustedWeightedClosing, 0) , @LastSO3_RSI_EMAVolumeAdjustedTypicalClosing =  isnull(SO3_RSI_EMAVolumeAdjustedTypicalClosing, 0) , @LastSO3_RSI_WSClosing =  isnull(SO3_RSI_WSClosing, 0) , @LastSO3_RSI_WSVolume =  isnull(SO3_RSI_WSVolume, 0) , @LastSO3_RSI_WSWeightedClosing =  isnull(SO3_RSI_WSWeightedClosing, 0) , @LastSO3_RSI_WSTypicalClosing =  isnull(SO3_RSI_WSTypicalClosing, 0) , @LastSO3_RSI_WSVolumeAdjustedClosing =  isnull(SO3_RSI_WSVolumeAdjustedClosing, 0) , @LastSO3_RSI_WSVolumeAdjustedWeightedClosing =  isnull(SO3_RSI_WSVolumeAdjustedWeightedClosing, 0) , @LastSO3_RSI_WSVolumeAdjustedTypicalClosing =  isnull(SO3_RSI_WSVolumeAdjustedTypicalClosing, 0) , @LastSO3_STDEVClosing =  isnull(SO3_STDEVClosing, 0) , @LastSO3_STDEVWeightedClosing =  isnull(SO3_STDEVWeightedClosing, 0) , @LastSO3_STDEVTypicalClosing =  isnull(SO3_STDEVTypicalClosing, 0) , @LastSO3_STDEVVolume =  isnull(SO3_STDEVVolume, 0) , @LastSO3_RVIClosing =  isnull(SO3_RVIClosing, 0) , @LastSO3_RVIVolume =  isnull(SO3_RVIVolume, 0) , @LastSO3_RVIWeightedClosing =  isnull(SO3_RVIWeightedClosing, 0) , @LastSO3_RVITypicalClosing =  isnull(SO3_RVITypicalClosing, 0) , @LastSO3_RVIVolumeAdjustedClosing =  isnull(SO3_RVIVolumeAdjustedClosing, 0) , @LastSO3_RVIVolumeAdjustedWeightedClosing =  isnull(SO3_RVIVolumeAdjustedWeightedClosing, 0) , @LastSO3_RVIVolumeAdjustedTypicalClosing =  isnull(SO3_RVIVolumeAdjustedTypicalClosing, 0) , 
			@LastSO4_Opening =  isnull(SO4_Opening, 0) , @LastSO4_High =  isnull(SO4_High, 0) , @LastSO4_Low =  isnull(SO4_Low, 0) , @LastSO4_Closing =  isnull(SO4_Closing, 0) , @LastSO4_Volume =  isnull(SO4_Volume, 0) , @LastSO4_SMAClosing =  isnull(SO4_SMAClosing, 0) , @LastSO4_SMAWeightedClosing =  isnull(SO4_SMAWeightedClosing, 0) , @LastSO4_SMATypicalClosing =  isnull(SO4_SMATypicalClosing, 0) , @LastSO4_SMAVolume =  isnull(SO4_SMAVolume, 0) , @LastSO4_VAMAClosing =  isnull(SO4_VAMAClosing, 0) , @LastSO4_VAMATypicalClosing =  isnull(SO4_VAMATypicalClosing, 0) , @LastSO4_VAMAWeightedClosing =  isnull(SO4_VAMAWeightedClosing, 0) , @LastSO4_WMAClosing =  isnull(SO4_WMAClosing, 0) , @LastSO4_WMAWeightedClosing =  isnull(SO4_WMAWeightedClosing, 0) , @LastSO4_WMATypicalClosing =  isnull(SO4_WMATypicalClosing, 0) , @LastSO4_EMAClosing =  isnull(SO4_EMAClosing, 0) , @LastSO4_EMAWeightedClosing =  isnull(SO4_EMAWeightedClosing, 0) , @LastSO4_EMATypicalClosing =  isnull(SO4_EMATypicalClosing, 0) , @LastSO4_EMAVolume =  isnull(SO4_EMAVolume, 0) , @LastSO4_CMOClosing =  isnull(SO4_CMOClosing, 0) , @LastSO4_CMOVolume =  isnull(SO4_CMOVolume, 0) , @LastSO4_CMOWeightedClosing =  isnull(SO4_CMOWeightedClosing, 0) , @LastSO4_CMOTypicalClosing =  isnull(SO4_CMOTypicalClosing, 0) , @LastSO4_VMAClosing =  isnull(SO4_VMAClosing, 0) , @LastSO4_VMAVolume =  isnull(SO4_VMAVolume, 0) , @LastSO4_VMAWeightedClosing =  isnull(SO4_VMAWeightedClosing, 0) , @LastSO4_VMATypicalClosing =  isnull(SO4_VMATypicalClosing, 0) , @LastSO4_WSClosing =  isnull(SO4_WSClosing, 0) , @LastSO4_WSWeightedClosing =  isnull(SO4_WSWeightedClosing, 0) , @LastSO4_WSTypicalClosing =  isnull(SO4_WSTypicalClosing, 0) , @LastSO4_WSWeightedNormalClosing =  isnull(SO4_WSWeightedNormalClosing, 0) , @LastSO4_WSWeightedWeightedClosing =  isnull(SO4_WSWeightedWeightedClosing, 0) , @LastSO4_WSWeightedTypicalClosing =  isnull(SO4_WSWeightedTypicalClosing, 0) , @LastSO4_WSVolume =  isnull(SO4_WSVolume, 0) , @LastSO4_WSVolumeAdjustedClosing =  isnull(SO4_WSVolumeAdjustedClosing, 0) , @LastSO4_WSVolumeAdjustedTypicalClosing =  isnull(SO4_WSVolumeAdjustedTypicalClosing, 0) , @LastSO4_WSVolumeAdjustedWeightedClosing =  isnull(SO4_WSVolumeAdjustedWeightedClosing, 0) , @LastSO4_RSI_SMAClosing =  isnull(SO4_RSI_SMAClosing, 0) , @LastSO4_RSI_SMAVolume =  isnull(SO4_RSI_SMAVolume, 0) , @LastSO4_RSI_SMAWeightedClosing =  isnull(SO4_RSI_SMAWeightedClosing, 0) , @LastSO4_RSI_SMATypicalClosing =  isnull(SO4_RSI_SMATypicalClosing, 0) , @LastSO4_RSI_SMAVolumeAdjustedClosing =  isnull(SO4_RSI_SMAVolumeAdjustedClosing, 0) , @LastSO4_RSI_SMAVolumeAdjustedWeightedClosing =  isnull(SO4_RSI_SMAVolumeAdjustedWeightedClosing, 0) , @LastSO4_RSI_SMAVolumeAdjustedTypicalClosing =  isnull(SO4_RSI_SMAVolumeAdjustedTypicalClosing, 0) , @LastSO4_RSI_EMAClosing =  isnull(SO4_RSI_EMAClosing, 0) , @LastSO4_RSI_EMAVolume =  isnull(SO4_RSI_EMAVolume, 0) , @LastSO4_RSI_EMAWeightedClosing =  isnull(SO4_RSI_EMAWeightedClosing, 0) , @LastSO4_RSI_EMATypicalClosing =  isnull(SO4_RSI_EMATypicalClosing, 0) , @LastSO4_RSI_EMAVolumeAdjustedClosing =  isnull(SO4_RSI_EMAVolumeAdjustedClosing, 0) , @LastSO4_RSI_EMAVolumeAdjustedWeightedClosing =  isnull(SO4_RSI_EMAVolumeAdjustedWeightedClosing, 0) , @LastSO4_RSI_EMAVolumeAdjustedTypicalClosing =  isnull(SO4_RSI_EMAVolumeAdjustedTypicalClosing, 0) , @LastSO4_RSI_WSClosing =  isnull(SO4_RSI_WSClosing, 0) , @LastSO4_RSI_WSVolume =  isnull(SO4_RSI_WSVolume, 0) , @LastSO4_RSI_WSWeightedClosing =  isnull(SO4_RSI_WSWeightedClosing, 0) , @LastSO4_RSI_WSTypicalClosing =  isnull(SO4_RSI_WSTypicalClosing, 0) , @LastSO4_RSI_WSVolumeAdjustedClosing =  isnull(SO4_RSI_WSVolumeAdjustedClosing, 0) , @LastSO4_RSI_WSVolumeAdjustedWeightedClosing =  isnull(SO4_RSI_WSVolumeAdjustedWeightedClosing, 0) , @LastSO4_RSI_WSVolumeAdjustedTypicalClosing =  isnull(SO4_RSI_WSVolumeAdjustedTypicalClosing, 0) , @LastSO4_STDEVClosing =  isnull(SO4_STDEVClosing, 0) , @LastSO4_STDEVWeightedClosing =  isnull(SO4_STDEVWeightedClosing, 0) , @LastSO4_STDEVTypicalClosing =  isnull(SO4_STDEVTypicalClosing, 0) , @LastSO4_STDEVVolume =  isnull(SO4_STDEVVolume, 0) , @LastSO4_RVIClosing =  isnull(SO4_RVIClosing, 0) , @LastSO4_RVIVolume =  isnull(SO4_RVIVolume, 0) , @LastSO4_RVIWeightedClosing =  isnull(SO4_RVIWeightedClosing, 0) , @LastSO4_RVITypicalClosing =  isnull(SO4_RVITypicalClosing, 0) , @LastSO4_RVIVolumeAdjustedClosing =  isnull(SO4_RVIVolumeAdjustedClosing, 0) , @LastSO4_RVIVolumeAdjustedWeightedClosing =  isnull(SO4_RVIVolumeAdjustedWeightedClosing, 0) , @LastSO4_RVIVolumeAdjustedTypicalClosing =  isnull(SO4_RVIVolumeAdjustedTypicalClosing, 0) , 
			@LastSO5_Opening =  isnull(SO5_Opening, 0) , @LastSO5_High =  isnull(SO5_High, 0) , @LastSO5_Low =  isnull(SO5_Low, 0) , @LastSO5_Closing =  isnull(SO5_Closing, 0) , @LastSO5_Volume =  isnull(SO5_Volume, 0) , @LastSO5_SMAClosing =  isnull(SO5_SMAClosing, 0) , @LastSO5_SMAWeightedClosing =  isnull(SO5_SMAWeightedClosing, 0) , @LastSO5_SMATypicalClosing =  isnull(SO5_SMATypicalClosing, 0) , @LastSO5_SMAVolume =  isnull(SO5_SMAVolume, 0) , @LastSO5_VAMAClosing =  isnull(SO5_VAMAClosing, 0) , @LastSO5_VAMATypicalClosing =  isnull(SO5_VAMATypicalClosing, 0) , @LastSO5_VAMAWeightedClosing =  isnull(SO5_VAMAWeightedClosing, 0) , @LastSO5_WMAClosing =  isnull(SO5_WMAClosing, 0) , @LastSO5_WMAWeightedClosing =  isnull(SO5_WMAWeightedClosing, 0) , @LastSO5_WMATypicalClosing =  isnull(SO5_WMATypicalClosing, 0) , @LastSO5_EMAClosing =  isnull(SO5_EMAClosing, 0) , @LastSO5_EMAWeightedClosing =  isnull(SO5_EMAWeightedClosing, 0) , @LastSO5_EMATypicalClosing =  isnull(SO5_EMATypicalClosing, 0) , @LastSO5_EMAVolume =  isnull(SO5_EMAVolume, 0) , @LastSO5_CMOClosing =  isnull(SO5_CMOClosing, 0) , @LastSO5_CMOVolume =  isnull(SO5_CMOVolume, 0) , @LastSO5_CMOWeightedClosing =  isnull(SO5_CMOWeightedClosing, 0) , @LastSO5_CMOTypicalClosing =  isnull(SO5_CMOTypicalClosing, 0) , @LastSO5_VMAClosing =  isnull(SO5_VMAClosing, 0) , @LastSO5_VMAVolume =  isnull(SO5_VMAVolume, 0) , @LastSO5_VMAWeightedClosing =  isnull(SO5_VMAWeightedClosing, 0) , @LastSO5_VMATypicalClosing =  isnull(SO5_VMATypicalClosing, 0) , @LastSO5_WSClosing =  isnull(SO5_WSClosing, 0) , @LastSO5_WSWeightedClosing =  isnull(SO5_WSWeightedClosing, 0) , @LastSO5_WSTypicalClosing =  isnull(SO5_WSTypicalClosing, 0) , @LastSO5_WSWeightedNormalClosing =  isnull(SO5_WSWeightedNormalClosing, 0) , @LastSO5_WSWeightedWeightedClosing =  isnull(SO5_WSWeightedWeightedClosing, 0) , @LastSO5_WSWeightedTypicalClosing =  isnull(SO5_WSWeightedTypicalClosing, 0) , @LastSO5_WSVolume =  isnull(SO5_WSVolume, 0) , @LastSO5_WSVolumeAdjustedClosing =  isnull(SO5_WSVolumeAdjustedClosing, 0) , @LastSO5_WSVolumeAdjustedTypicalClosing =  isnull(SO5_WSVolumeAdjustedTypicalClosing, 0) , @LastSO5_WSVolumeAdjustedWeightedClosing =  isnull(SO5_WSVolumeAdjustedWeightedClosing, 0) , @LastSO5_RSI_SMAClosing =  isnull(SO5_RSI_SMAClosing, 0) , @LastSO5_RSI_SMAVolume =  isnull(SO5_RSI_SMAVolume, 0) , @LastSO5_RSI_SMAWeightedClosing =  isnull(SO5_RSI_SMAWeightedClosing, 0) , @LastSO5_RSI_SMATypicalClosing =  isnull(SO5_RSI_SMATypicalClosing, 0) , @LastSO5_RSI_SMAVolumeAdjustedClosing =  isnull(SO5_RSI_SMAVolumeAdjustedClosing, 0) , @LastSO5_RSI_SMAVolumeAdjustedWeightedClosing =  isnull(SO5_RSI_SMAVolumeAdjustedWeightedClosing, 0) , @LastSO5_RSI_SMAVolumeAdjustedTypicalClosing =  isnull(SO5_RSI_SMAVolumeAdjustedTypicalClosing, 0) , @LastSO5_RSI_EMAClosing =  isnull(SO5_RSI_EMAClosing, 0) , @LastSO5_RSI_EMAVolume =  isnull(SO5_RSI_EMAVolume, 0) , @LastSO5_RSI_EMAWeightedClosing =  isnull(SO5_RSI_EMAWeightedClosing, 0) , @LastSO5_RSI_EMATypicalClosing =  isnull(SO5_RSI_EMATypicalClosing, 0) , @LastSO5_RSI_EMAVolumeAdjustedClosing =  isnull(SO5_RSI_EMAVolumeAdjustedClosing, 0) , @LastSO5_RSI_EMAVolumeAdjustedWeightedClosing =  isnull(SO5_RSI_EMAVolumeAdjustedWeightedClosing, 0) , @LastSO5_RSI_EMAVolumeAdjustedTypicalClosing =  isnull(SO5_RSI_EMAVolumeAdjustedTypicalClosing, 0) , @LastSO5_RSI_WSClosing =  isnull(SO5_RSI_WSClosing, 0) , @LastSO5_RSI_WSVolume =  isnull(SO5_RSI_WSVolume, 0) , @LastSO5_RSI_WSWeightedClosing =  isnull(SO5_RSI_WSWeightedClosing, 0) , @LastSO5_RSI_WSTypicalClosing =  isnull(SO5_RSI_WSTypicalClosing, 0) , @LastSO5_RSI_WSVolumeAdjustedClosing =  isnull(SO5_RSI_WSVolumeAdjustedClosing, 0) , @LastSO5_RSI_WSVolumeAdjustedWeightedClosing =  isnull(SO5_RSI_WSVolumeAdjustedWeightedClosing, 0) , @LastSO5_RSI_WSVolumeAdjustedTypicalClosing =  isnull(SO5_RSI_WSVolumeAdjustedTypicalClosing, 0) , @LastSO5_STDEVClosing =  isnull(SO5_STDEVClosing, 0) , @LastSO5_STDEVWeightedClosing =  isnull(SO5_STDEVWeightedClosing, 0) , @LastSO5_STDEVTypicalClosing =  isnull(SO5_STDEVTypicalClosing, 0) , @LastSO5_STDEVVolume =  isnull(SO5_STDEVVolume, 0) , @LastSO5_RVIClosing =  isnull(SO5_RVIClosing, 0) , @LastSO5_RVIVolume =  isnull(SO5_RVIVolume, 0) , @LastSO5_RVIWeightedClosing =  isnull(SO5_RVIWeightedClosing, 0) , @LastSO5_RVITypicalClosing =  isnull(SO5_RVITypicalClosing, 0) , @LastSO5_RVIVolumeAdjustedClosing =  isnull(SO5_RVIVolumeAdjustedClosing, 0) , @LastSO5_RVIVolumeAdjustedWeightedClosing =  isnull(SO5_RVIVolumeAdjustedWeightedClosing, 0) , @LastSO5_RVIVolumeAdjustedTypicalClosing =  isnull(SO5_RVIVolumeAdjustedTypicalClosing, 0) , 
			@LastSO6_Opening =  isnull(SO6_Opening, 0) , @LastSO6_High =  isnull(SO6_High, 0) , @LastSO6_Low =  isnull(SO6_Low, 0) , @LastSO6_Closing =  isnull(SO6_Closing, 0) , @LastSO6_Volume =  isnull(SO6_Volume, 0) , @LastSO6_SMAClosing =  isnull(SO6_SMAClosing, 0) , @LastSO6_SMAWeightedClosing =  isnull(SO6_SMAWeightedClosing, 0) , @LastSO6_SMATypicalClosing =  isnull(SO6_SMATypicalClosing, 0) , @LastSO6_SMAVolume =  isnull(SO6_SMAVolume, 0) , @LastSO6_VAMAClosing =  isnull(SO6_VAMAClosing, 0) , @LastSO6_VAMATypicalClosing =  isnull(SO6_VAMATypicalClosing, 0) , @LastSO6_VAMAWeightedClosing =  isnull(SO6_VAMAWeightedClosing, 0) , @LastSO6_WMAClosing =  isnull(SO6_WMAClosing, 0) , @LastSO6_WMAWeightedClosing =  isnull(SO6_WMAWeightedClosing, 0) , @LastSO6_WMATypicalClosing =  isnull(SO6_WMATypicalClosing, 0) , @LastSO6_EMAClosing =  isnull(SO6_EMAClosing, 0) , @LastSO6_EMAWeightedClosing =  isnull(SO6_EMAWeightedClosing, 0) , @LastSO6_EMATypicalClosing =  isnull(SO6_EMATypicalClosing, 0) , @LastSO6_EMAVolume =  isnull(SO6_EMAVolume, 0) , @LastSO6_CMOClosing =  isnull(SO6_CMOClosing, 0) , @LastSO6_CMOVolume =  isnull(SO6_CMOVolume, 0) , @LastSO6_CMOWeightedClosing =  isnull(SO6_CMOWeightedClosing, 0) , @LastSO6_CMOTypicalClosing =  isnull(SO6_CMOTypicalClosing, 0) , @LastSO6_VMAClosing =  isnull(SO6_VMAClosing, 0) , @LastSO6_VMAVolume =  isnull(SO6_VMAVolume, 0) , @LastSO6_VMAWeightedClosing =  isnull(SO6_VMAWeightedClosing, 0) , @LastSO6_VMATypicalClosing =  isnull(SO6_VMATypicalClosing, 0) , @LastSO6_WSClosing =  isnull(SO6_WSClosing, 0) , @LastSO6_WSWeightedClosing =  isnull(SO6_WSWeightedClosing, 0) , @LastSO6_WSTypicalClosing =  isnull(SO6_WSTypicalClosing, 0) , @LastSO6_WSWeightedNormalClosing =  isnull(SO6_WSWeightedNormalClosing, 0) , @LastSO6_WSWeightedWeightedClosing =  isnull(SO6_WSWeightedWeightedClosing, 0) , @LastSO6_WSWeightedTypicalClosing =  isnull(SO6_WSWeightedTypicalClosing, 0) , @LastSO6_WSVolume =  isnull(SO6_WSVolume, 0) , @LastSO6_WSVolumeAdjustedClosing =  isnull(SO6_WSVolumeAdjustedClosing, 0) , @LastSO6_WSVolumeAdjustedTypicalClosing =  isnull(SO6_WSVolumeAdjustedTypicalClosing, 0) , @LastSO6_WSVolumeAdjustedWeightedClosing =  isnull(SO6_WSVolumeAdjustedWeightedClosing, 0) , @LastSO6_RSI_SMAClosing =  isnull(SO6_RSI_SMAClosing, 0) , @LastSO6_RSI_SMAVolume =  isnull(SO6_RSI_SMAVolume, 0) , @LastSO6_RSI_SMAWeightedClosing =  isnull(SO6_RSI_SMAWeightedClosing, 0) , @LastSO6_RSI_SMATypicalClosing =  isnull(SO6_RSI_SMATypicalClosing, 0) , @LastSO6_RSI_SMAVolumeAdjustedClosing =  isnull(SO6_RSI_SMAVolumeAdjustedClosing, 0) , @LastSO6_RSI_SMAVolumeAdjustedWeightedClosing =  isnull(SO6_RSI_SMAVolumeAdjustedWeightedClosing, 0) , @LastSO6_RSI_SMAVolumeAdjustedTypicalClosing =  isnull(SO6_RSI_SMAVolumeAdjustedTypicalClosing, 0) , @LastSO6_RSI_EMAClosing =  isnull(SO6_RSI_EMAClosing, 0) , @LastSO6_RSI_EMAVolume =  isnull(SO6_RSI_EMAVolume, 0) , @LastSO6_RSI_EMAWeightedClosing =  isnull(SO6_RSI_EMAWeightedClosing, 0) , @LastSO6_RSI_EMATypicalClosing =  isnull(SO6_RSI_EMATypicalClosing, 0) , @LastSO6_RSI_EMAVolumeAdjustedClosing =  isnull(SO6_RSI_EMAVolumeAdjustedClosing, 0) , @LastSO6_RSI_EMAVolumeAdjustedWeightedClosing =  isnull(SO6_RSI_EMAVolumeAdjustedWeightedClosing, 0) , @LastSO6_RSI_EMAVolumeAdjustedTypicalClosing =  isnull(SO6_RSI_EMAVolumeAdjustedTypicalClosing, 0) , @LastSO6_RSI_WSClosing =  isnull(SO6_RSI_WSClosing, 0) , @LastSO6_RSI_WSVolume =  isnull(SO6_RSI_WSVolume, 0) , @LastSO6_RSI_WSWeightedClosing =  isnull(SO6_RSI_WSWeightedClosing, 0) , @LastSO6_RSI_WSTypicalClosing =  isnull(SO6_RSI_WSTypicalClosing, 0) , @LastSO6_RSI_WSVolumeAdjustedClosing =  isnull(SO6_RSI_WSVolumeAdjustedClosing, 0) , @LastSO6_RSI_WSVolumeAdjustedWeightedClosing =  isnull(SO6_RSI_WSVolumeAdjustedWeightedClosing, 0) , @LastSO6_RSI_WSVolumeAdjustedTypicalClosing =  isnull(SO6_RSI_WSVolumeAdjustedTypicalClosing, 0) , @LastSO6_STDEVClosing =  isnull(SO6_STDEVClosing, 0) , @LastSO6_STDEVWeightedClosing =  isnull(SO6_STDEVWeightedClosing, 0) , @LastSO6_STDEVTypicalClosing =  isnull(SO6_STDEVTypicalClosing, 0) , @LastSO6_STDEVVolume =  isnull(SO6_STDEVVolume, 0) , @LastSO6_RVIClosing =  isnull(SO6_RVIClosing, 0) , @LastSO6_RVIVolume =  isnull(SO6_RVIVolume, 0) , @LastSO6_RVIWeightedClosing =  isnull(SO6_RVIWeightedClosing, 0) , @LastSO6_RVITypicalClosing =  isnull(SO6_RVITypicalClosing, 0) , @LastSO6_RVIVolumeAdjustedClosing =  isnull(SO6_RVIVolumeAdjustedClosing, 0) , @LastSO6_RVIVolumeAdjustedWeightedClosing =  isnull(SO6_RVIVolumeAdjustedWeightedClosing, 0) , @LastSO6_RVIVolumeAdjustedTypicalClosing =  isnull(SO6_RVIVolumeAdjustedTypicalClosing, 0) , 
			@LastSO7_Opening =  isnull(SO7_Opening, 0) , @LastSO7_High =  isnull(SO7_High, 0) , @LastSO7_Low =  isnull(SO7_Low, 0) , @LastSO7_Closing =  isnull(SO7_Closing, 0) , @LastSO7_Volume =  isnull(SO7_Volume, 0) , @LastSO7_SMAClosing =  isnull(SO7_SMAClosing, 0) , @LastSO7_SMAWeightedClosing =  isnull(SO7_SMAWeightedClosing, 0) , @LastSO7_SMATypicalClosing =  isnull(SO7_SMATypicalClosing, 0) , @LastSO7_SMAVolume =  isnull(SO7_SMAVolume, 0) , @LastSO7_VAMAClosing =  isnull(SO7_VAMAClosing, 0) , @LastSO7_VAMATypicalClosing =  isnull(SO7_VAMATypicalClosing, 0) , @LastSO7_VAMAWeightedClosing =  isnull(SO7_VAMAWeightedClosing, 0) , @LastSO7_WMAClosing =  isnull(SO7_WMAClosing, 0) , @LastSO7_WMAWeightedClosing =  isnull(SO7_WMAWeightedClosing, 0) , @LastSO7_WMATypicalClosing =  isnull(SO7_WMATypicalClosing, 0) , @LastSO7_EMAClosing =  isnull(SO7_EMAClosing, 0) , @LastSO7_EMAWeightedClosing =  isnull(SO7_EMAWeightedClosing, 0) , @LastSO7_EMATypicalClosing =  isnull(SO7_EMATypicalClosing, 0) , @LastSO7_EMAVolume =  isnull(SO7_EMAVolume, 0) , @LastSO7_CMOClosing =  isnull(SO7_CMOClosing, 0) , @LastSO7_CMOVolume =  isnull(SO7_CMOVolume, 0) , @LastSO7_CMOWeightedClosing =  isnull(SO7_CMOWeightedClosing, 0) , @LastSO7_CMOTypicalClosing =  isnull(SO7_CMOTypicalClosing, 0) , @LastSO7_VMAClosing =  isnull(SO7_VMAClosing, 0) , @LastSO7_VMAVolume =  isnull(SO7_VMAVolume, 0) , @LastSO7_VMAWeightedClosing =  isnull(SO7_VMAWeightedClosing, 0) , @LastSO7_VMATypicalClosing =  isnull(SO7_VMATypicalClosing, 0) , @LastSO7_WSClosing =  isnull(SO7_WSClosing, 0) , @LastSO7_WSWeightedClosing =  isnull(SO7_WSWeightedClosing, 0) , @LastSO7_WSTypicalClosing =  isnull(SO7_WSTypicalClosing, 0) , @LastSO7_WSWeightedNormalClosing =  isnull(SO7_WSWeightedNormalClosing, 0) , @LastSO7_WSWeightedWeightedClosing =  isnull(SO7_WSWeightedWeightedClosing, 0) , @LastSO7_WSWeightedTypicalClosing =  isnull(SO7_WSWeightedTypicalClosing, 0) , @LastSO7_WSVolume =  isnull(SO7_WSVolume, 0) , @LastSO7_WSVolumeAdjustedClosing =  isnull(SO7_WSVolumeAdjustedClosing, 0) , @LastSO7_WSVolumeAdjustedTypicalClosing =  isnull(SO7_WSVolumeAdjustedTypicalClosing, 0) , @LastSO7_WSVolumeAdjustedWeightedClosing =  isnull(SO7_WSVolumeAdjustedWeightedClosing, 0) , @LastSO7_RSI_SMAClosing =  isnull(SO7_RSI_SMAClosing, 0) , @LastSO7_RSI_SMAVolume =  isnull(SO7_RSI_SMAVolume, 0) , @LastSO7_RSI_SMAWeightedClosing =  isnull(SO7_RSI_SMAWeightedClosing, 0) , @LastSO7_RSI_SMATypicalClosing =  isnull(SO7_RSI_SMATypicalClosing, 0) , @LastSO7_RSI_SMAVolumeAdjustedClosing =  isnull(SO7_RSI_SMAVolumeAdjustedClosing, 0) , @LastSO7_RSI_SMAVolumeAdjustedWeightedClosing =  isnull(SO7_RSI_SMAVolumeAdjustedWeightedClosing, 0) , @LastSO7_RSI_SMAVolumeAdjustedTypicalClosing =  isnull(SO7_RSI_SMAVolumeAdjustedTypicalClosing, 0) , @LastSO7_RSI_EMAClosing =  isnull(SO7_RSI_EMAClosing, 0) , @LastSO7_RSI_EMAVolume =  isnull(SO7_RSI_EMAVolume, 0) , @LastSO7_RSI_EMAWeightedClosing =  isnull(SO7_RSI_EMAWeightedClosing, 0) , @LastSO7_RSI_EMATypicalClosing =  isnull(SO7_RSI_EMATypicalClosing, 0) , @LastSO7_RSI_EMAVolumeAdjustedClosing =  isnull(SO7_RSI_EMAVolumeAdjustedClosing, 0) , @LastSO7_RSI_EMAVolumeAdjustedWeightedClosing =  isnull(SO7_RSI_EMAVolumeAdjustedWeightedClosing, 0) , @LastSO7_RSI_EMAVolumeAdjustedTypicalClosing =  isnull(SO7_RSI_EMAVolumeAdjustedTypicalClosing, 0) , @LastSO7_RSI_WSClosing =  isnull(SO7_RSI_WSClosing, 0) , @LastSO7_RSI_WSVolume =  isnull(SO7_RSI_WSVolume, 0) , @LastSO7_RSI_WSWeightedClosing =  isnull(SO7_RSI_WSWeightedClosing, 0) , @LastSO7_RSI_WSTypicalClosing =  isnull(SO7_RSI_WSTypicalClosing, 0) , @LastSO7_RSI_WSVolumeAdjustedClosing =  isnull(SO7_RSI_WSVolumeAdjustedClosing, 0) , @LastSO7_RSI_WSVolumeAdjustedWeightedClosing =  isnull(SO7_RSI_WSVolumeAdjustedWeightedClosing, 0) , @LastSO7_RSI_WSVolumeAdjustedTypicalClosing =  isnull(SO7_RSI_WSVolumeAdjustedTypicalClosing, 0) , @LastSO7_STDEVClosing =  isnull(SO7_STDEVClosing, 0) , @LastSO7_STDEVWeightedClosing =  isnull(SO7_STDEVWeightedClosing, 0) , @LastSO7_STDEVTypicalClosing =  isnull(SO7_STDEVTypicalClosing, 0) , @LastSO7_STDEVVolume =  isnull(SO7_STDEVVolume, 0) , @LastSO7_RVIClosing =  isnull(SO7_RVIClosing, 0) , @LastSO7_RVIVolume =  isnull(SO7_RVIVolume, 0) , @LastSO7_RVIWeightedClosing =  isnull(SO7_RVIWeightedClosing, 0) , @LastSO7_RVITypicalClosing =  isnull(SO7_RVITypicalClosing, 0) , @LastSO7_RVIVolumeAdjustedClosing =  isnull(SO7_RVIVolumeAdjustedClosing, 0) , @LastSO7_RVIVolumeAdjustedWeightedClosing =  isnull(SO7_RVIVolumeAdjustedWeightedClosing, 0) , @LastSO7_RVIVolumeAdjustedTypicalClosing =  isnull(SO7_RVIVolumeAdjustedTypicalClosing, 0) , 
			@LastSO8_Opening =  isnull(SO8_Opening, 0) , @LastSO8_High =  isnull(SO8_High, 0) , @LastSO8_Low =  isnull(SO8_Low, 0) , @LastSO8_Closing =  isnull(SO8_Closing, 0) , @LastSO8_Volume =  isnull(SO8_Volume, 0) , @LastSO8_SMAClosing =  isnull(SO8_SMAClosing, 0) , @LastSO8_SMAWeightedClosing =  isnull(SO8_SMAWeightedClosing, 0) , @LastSO8_SMATypicalClosing =  isnull(SO8_SMATypicalClosing, 0) , @LastSO8_SMAVolume =  isnull(SO8_SMAVolume, 0) , @LastSO8_VAMAClosing =  isnull(SO8_VAMAClosing, 0) , @LastSO8_VAMATypicalClosing =  isnull(SO8_VAMATypicalClosing, 0) , @LastSO8_VAMAWeightedClosing =  isnull(SO8_VAMAWeightedClosing, 0) , @LastSO8_WMAClosing =  isnull(SO8_WMAClosing, 0) , @LastSO8_WMAWeightedClosing =  isnull(SO8_WMAWeightedClosing, 0) , @LastSO8_WMATypicalClosing =  isnull(SO8_WMATypicalClosing, 0) , @LastSO8_EMAClosing =  isnull(SO8_EMAClosing, 0) , @LastSO8_EMAWeightedClosing =  isnull(SO8_EMAWeightedClosing, 0) , @LastSO8_EMATypicalClosing =  isnull(SO8_EMATypicalClosing, 0) , @LastSO8_EMAVolume =  isnull(SO8_EMAVolume, 0) , @LastSO8_CMOClosing =  isnull(SO8_CMOClosing, 0) , @LastSO8_CMOVolume =  isnull(SO8_CMOVolume, 0) , @LastSO8_CMOWeightedClosing =  isnull(SO8_CMOWeightedClosing, 0) , @LastSO8_CMOTypicalClosing =  isnull(SO8_CMOTypicalClosing, 0) , @LastSO8_VMAClosing =  isnull(SO8_VMAClosing, 0) , @LastSO8_VMAVolume =  isnull(SO8_VMAVolume, 0) , @LastSO8_VMAWeightedClosing =  isnull(SO8_VMAWeightedClosing, 0) , @LastSO8_VMATypicalClosing =  isnull(SO8_VMATypicalClosing, 0) , @LastSO8_WSClosing =  isnull(SO8_WSClosing, 0) , @LastSO8_WSWeightedClosing =  isnull(SO8_WSWeightedClosing, 0) , @LastSO8_WSTypicalClosing =  isnull(SO8_WSTypicalClosing, 0) , @LastSO8_WSWeightedNormalClosing =  isnull(SO8_WSWeightedNormalClosing, 0) , @LastSO8_WSWeightedWeightedClosing =  isnull(SO8_WSWeightedWeightedClosing, 0) , @LastSO8_WSWeightedTypicalClosing =  isnull(SO8_WSWeightedTypicalClosing, 0) , @LastSO8_WSVolume =  isnull(SO8_WSVolume, 0) , @LastSO8_WSVolumeAdjustedClosing =  isnull(SO8_WSVolumeAdjustedClosing, 0) , @LastSO8_WSVolumeAdjustedTypicalClosing =  isnull(SO8_WSVolumeAdjustedTypicalClosing, 0) , @LastSO8_WSVolumeAdjustedWeightedClosing =  isnull(SO8_WSVolumeAdjustedWeightedClosing, 0) , @LastSO8_RSI_SMAClosing =  isnull(SO8_RSI_SMAClosing, 0) , @LastSO8_RSI_SMAVolume =  isnull(SO8_RSI_SMAVolume, 0) , @LastSO8_RSI_SMAWeightedClosing =  isnull(SO8_RSI_SMAWeightedClosing, 0) , @LastSO8_RSI_SMATypicalClosing =  isnull(SO8_RSI_SMATypicalClosing, 0) , @LastSO8_RSI_SMAVolumeAdjustedClosing =  isnull(SO8_RSI_SMAVolumeAdjustedClosing, 0) , @LastSO8_RSI_SMAVolumeAdjustedWeightedClosing =  isnull(SO8_RSI_SMAVolumeAdjustedWeightedClosing, 0) , @LastSO8_RSI_SMAVolumeAdjustedTypicalClosing =  isnull(SO8_RSI_SMAVolumeAdjustedTypicalClosing, 0) , @LastSO8_RSI_EMAClosing =  isnull(SO8_RSI_EMAClosing, 0) , @LastSO8_RSI_EMAVolume =  isnull(SO8_RSI_EMAVolume, 0) , @LastSO8_RSI_EMAWeightedClosing =  isnull(SO8_RSI_EMAWeightedClosing, 0) , @LastSO8_RSI_EMATypicalClosing =  isnull(SO8_RSI_EMATypicalClosing, 0) , @LastSO8_RSI_EMAVolumeAdjustedClosing =  isnull(SO8_RSI_EMAVolumeAdjustedClosing, 0) , @LastSO8_RSI_EMAVolumeAdjustedWeightedClosing =  isnull(SO8_RSI_EMAVolumeAdjustedWeightedClosing, 0) , @LastSO8_RSI_EMAVolumeAdjustedTypicalClosing =  isnull(SO8_RSI_EMAVolumeAdjustedTypicalClosing, 0) , @LastSO8_RSI_WSClosing =  isnull(SO8_RSI_WSClosing, 0) , @LastSO8_RSI_WSVolume =  isnull(SO8_RSI_WSVolume, 0) , @LastSO8_RSI_WSWeightedClosing =  isnull(SO8_RSI_WSWeightedClosing, 0) , @LastSO8_RSI_WSTypicalClosing =  isnull(SO8_RSI_WSTypicalClosing, 0) , @LastSO8_RSI_WSVolumeAdjustedClosing =  isnull(SO8_RSI_WSVolumeAdjustedClosing, 0) , @LastSO8_RSI_WSVolumeAdjustedWeightedClosing =  isnull(SO8_RSI_WSVolumeAdjustedWeightedClosing, 0) , @LastSO8_RSI_WSVolumeAdjustedTypicalClosing =  isnull(SO8_RSI_WSVolumeAdjustedTypicalClosing, 0) , @LastSO8_STDEVClosing =  isnull(SO8_STDEVClosing, 0) , @LastSO8_STDEVWeightedClosing =  isnull(SO8_STDEVWeightedClosing, 0) , @LastSO8_STDEVTypicalClosing =  isnull(SO8_STDEVTypicalClosing, 0) , @LastSO8_STDEVVolume =  isnull(SO8_STDEVVolume, 0) , @LastSO8_RVIClosing =  isnull(SO8_RVIClosing, 0) , @LastSO8_RVIVolume =  isnull(SO8_RVIVolume, 0) , @LastSO8_RVIWeightedClosing =  isnull(SO8_RVIWeightedClosing, 0) , @LastSO8_RVITypicalClosing =  isnull(SO8_RVITypicalClosing, 0) , @LastSO8_RVIVolumeAdjustedClosing =  isnull(SO8_RVIVolumeAdjustedClosing, 0) , @LastSO8_RVIVolumeAdjustedWeightedClosing =  isnull(SO8_RVIVolumeAdjustedWeightedClosing, 0) , @LastSO8_RVIVolumeAdjustedTypicalClosing =  isnull(SO8_RVIVolumeAdjustedTypicalClosing, 0) , 
			@LastSO9_Opening =  isnull(SO9_Opening, 0) , @LastSO9_High =  isnull(SO9_High, 0) , @LastSO9_Low =  isnull(SO9_Low, 0) , @LastSO9_Closing =  isnull(SO9_Closing, 0) , @LastSO9_Volume =  isnull(SO9_Volume, 0) , @LastSO9_SMAClosing =  isnull(SO9_SMAClosing, 0) , @LastSO9_SMAWeightedClosing =  isnull(SO9_SMAWeightedClosing, 0) , @LastSO9_SMATypicalClosing =  isnull(SO9_SMATypicalClosing, 0) , @LastSO9_SMAVolume =  isnull(SO9_SMAVolume, 0) , @LastSO9_VAMAClosing =  isnull(SO9_VAMAClosing, 0) , @LastSO9_VAMATypicalClosing =  isnull(SO9_VAMATypicalClosing, 0) , @LastSO9_VAMAWeightedClosing =  isnull(SO9_VAMAWeightedClosing, 0) , @LastSO9_WMAClosing =  isnull(SO9_WMAClosing, 0) , @LastSO9_WMAWeightedClosing =  isnull(SO9_WMAWeightedClosing, 0) , @LastSO9_WMATypicalClosing =  isnull(SO9_WMATypicalClosing, 0) , @LastSO9_EMAClosing =  isnull(SO9_EMAClosing, 0) , @LastSO9_EMAWeightedClosing =  isnull(SO9_EMAWeightedClosing, 0) , @LastSO9_EMATypicalClosing =  isnull(SO9_EMATypicalClosing, 0) , @LastSO9_EMAVolume =  isnull(SO9_EMAVolume, 0) , @LastSO9_CMOClosing =  isnull(SO9_CMOClosing, 0) , @LastSO9_CMOVolume =  isnull(SO9_CMOVolume, 0) , @LastSO9_CMOWeightedClosing =  isnull(SO9_CMOWeightedClosing, 0) , @LastSO9_CMOTypicalClosing =  isnull(SO9_CMOTypicalClosing, 0) , @LastSO9_VMAClosing =  isnull(SO9_VMAClosing, 0) , @LastSO9_VMAVolume =  isnull(SO9_VMAVolume, 0) , @LastSO9_VMAWeightedClosing =  isnull(SO9_VMAWeightedClosing, 0) , @LastSO9_VMATypicalClosing =  isnull(SO9_VMATypicalClosing, 0) , @LastSO9_WSClosing =  isnull(SO9_WSClosing, 0) , @LastSO9_WSWeightedClosing =  isnull(SO9_WSWeightedClosing, 0) , @LastSO9_WSTypicalClosing =  isnull(SO9_WSTypicalClosing, 0) , @LastSO9_WSWeightedNormalClosing =  isnull(SO9_WSWeightedNormalClosing, 0) , @LastSO9_WSWeightedWeightedClosing =  isnull(SO9_WSWeightedWeightedClosing, 0) , @LastSO9_WSWeightedTypicalClosing =  isnull(SO9_WSWeightedTypicalClosing, 0) , @LastSO9_WSVolume =  isnull(SO9_WSVolume, 0) , @LastSO9_WSVolumeAdjustedClosing =  isnull(SO9_WSVolumeAdjustedClosing, 0) , @LastSO9_WSVolumeAdjustedTypicalClosing =  isnull(SO9_WSVolumeAdjustedTypicalClosing, 0) , @LastSO9_WSVolumeAdjustedWeightedClosing =  isnull(SO9_WSVolumeAdjustedWeightedClosing, 0) , @LastSO9_RSI_SMAClosing =  isnull(SO9_RSI_SMAClosing, 0) , @LastSO9_RSI_SMAVolume =  isnull(SO9_RSI_SMAVolume, 0) , @LastSO9_RSI_SMAWeightedClosing =  isnull(SO9_RSI_SMAWeightedClosing, 0) , @LastSO9_RSI_SMATypicalClosing =  isnull(SO9_RSI_SMATypicalClosing, 0) , @LastSO9_RSI_SMAVolumeAdjustedClosing =  isnull(SO9_RSI_SMAVolumeAdjustedClosing, 0) , @LastSO9_RSI_SMAVolumeAdjustedWeightedClosing =  isnull(SO9_RSI_SMAVolumeAdjustedWeightedClosing, 0) , @LastSO9_RSI_SMAVolumeAdjustedTypicalClosing =  isnull(SO9_RSI_SMAVolumeAdjustedTypicalClosing, 0) , @LastSO9_RSI_EMAClosing =  isnull(SO9_RSI_EMAClosing, 0) , @LastSO9_RSI_EMAVolume =  isnull(SO9_RSI_EMAVolume, 0) , @LastSO9_RSI_EMAWeightedClosing =  isnull(SO9_RSI_EMAWeightedClosing, 0) , @LastSO9_RSI_EMATypicalClosing =  isnull(SO9_RSI_EMATypicalClosing, 0) , @LastSO9_RSI_EMAVolumeAdjustedClosing =  isnull(SO9_RSI_EMAVolumeAdjustedClosing, 0) , @LastSO9_RSI_EMAVolumeAdjustedWeightedClosing =  isnull(SO9_RSI_EMAVolumeAdjustedWeightedClosing, 0) , @LastSO9_RSI_EMAVolumeAdjustedTypicalClosing =  isnull(SO9_RSI_EMAVolumeAdjustedTypicalClosing, 0) , @LastSO9_RSI_WSClosing =  isnull(SO9_RSI_WSClosing, 0) , @LastSO9_RSI_WSVolume =  isnull(SO9_RSI_WSVolume, 0) , @LastSO9_RSI_WSWeightedClosing =  isnull(SO9_RSI_WSWeightedClosing, 0) , @LastSO9_RSI_WSTypicalClosing =  isnull(SO9_RSI_WSTypicalClosing, 0) , @LastSO9_RSI_WSVolumeAdjustedClosing =  isnull(SO9_RSI_WSVolumeAdjustedClosing, 0) , @LastSO9_RSI_WSVolumeAdjustedWeightedClosing =  isnull(SO9_RSI_WSVolumeAdjustedWeightedClosing, 0) , @LastSO9_RSI_WSVolumeAdjustedTypicalClosing =  isnull(SO9_RSI_WSVolumeAdjustedTypicalClosing, 0) , @LastSO9_STDEVClosing =  isnull(SO9_STDEVClosing, 0) , @LastSO9_STDEVWeightedClosing =  isnull(SO9_STDEVWeightedClosing, 0) , @LastSO9_STDEVTypicalClosing =  isnull(SO9_STDEVTypicalClosing, 0) , @LastSO9_STDEVVolume =  isnull(SO9_STDEVVolume, 0) , @LastSO9_RVIClosing =  isnull(SO9_RVIClosing, 0) , @LastSO9_RVIVolume =  isnull(SO9_RVIVolume, 0) , @LastSO9_RVIWeightedClosing =  isnull(SO9_RVIWeightedClosing, 0) , @LastSO9_RVITypicalClosing =  isnull(SO9_RVITypicalClosing, 0) , @LastSO9_RVIVolumeAdjustedClosing =  isnull(SO9_RVIVolumeAdjustedClosing, 0) , @LastSO9_RVIVolumeAdjustedWeightedClosing =  isnull(SO9_RVIVolumeAdjustedWeightedClosing, 0) , @LastSO9_RVIVolumeAdjustedTypicalClosing =  isnull(SO9_RVIVolumeAdjustedTypicalClosing, 0) 
	from (select 1 as x) x
		left outer join DIM.Fact_Base f on	f.SymbolID = @SymbolID 
										and f.Seq = @Seq -1
										and f.PeriodID = @PeriodID
	/*Read Period -1 Daily*/
	select  @A0_Date = isnull(cast(Date as float),0), @A0_Opening = isnull(Opening,0), @A0_High = isnull(High,0), @A0_Low = isnull(Low,0), @A0_Closing = isnull(Closing,0), @A0_Volume = isnull(Volume,0), @A0_Interest = isnull(Interest,0), @A0_GainClosing = isnull((case when DiffClosing >=0 then DiffClosing else 0 end), 0), @A0_LossClosing = isnull((case when DiffClosing <=0 then -DiffClosing else 0 end), 0), @A0_GainVolume = isnull((case when DiffVolume >=0 then DiffVolume else 0 end), 0), @A0_LossVolume = isnull((case when DiffVolume <=0 then -DiffVolume else 0 end), 0), @A0_WeightedClosing = isnull(WeightedClosing,0), @A0_GainWeightedClosing = isnull((case when DiffWeightedClosing >=0 then DiffWeightedClosing else 0 end), 0), @A0_LossWeightedClosing = isnull((case when DiffWeightedClosing <=0 then -DiffWeightedClosing else 0 end), 0), @A0_TypicalClosing = isnull(TypicalClosing,0), @A0_GainTypicalClosing = isnull((case when DiffTypicalClosing >=0 then DiffTypicalClosing else 0 end), 0), @A0_LossTypicalClosing = isnull((case when DiffTypicalClosing <=0 then -DiffTypicalClosing else 0 end), 0), @A0_VolumeAdjustedClosing = isnull(VolumeAdjustedClosing,0), @A0_GainVolumeAdjustedClosing = isnull((case when DiffVolumeAdjustedClosing >=0 then DiffVolumeAdjustedClosing else 0 end), 0), @A0_LossVolumeAdjustedClosing = isnull((case when DiffVolumeAdjustedClosing <=0 then -DiffVolumeAdjustedClosing else 0 end), 0), @A0_VolumeAdjustedWeightedClosing = isnull(VolumeAdjustedWeightedClosing,0), @A0_GainVolumeAdjustedWeightedClosing = isnull((case when DiffVolumeAdjustedWeightedClosing >=0 then DiffVolumeAdjustedWeightedClosing else 0 end), 0), @A0_LossVolumeAdjustedWeightedClosing = isnull((case when DiffVolumeAdjustedWeightedClosing <=0 then -DiffVolumeAdjustedWeightedClosing else 0 end), 0), @A0_VolumeAdjustedTypicalClosing = isnull(VolumeAdjustedTypicalClosing,0), @A0_GainVolumeAdjustedTypicalClosing = isnull((case when DiffVolumeAdjustedTypicalClosing >=0 then DiffVolumeAdjustedTypicalClosing else 0 end), 0), @A0_LossVolumeAdjustedTypicalClosing = isnull((case when DiffVolumeAdjustedTypicalClosing <=0 then -DiffVolumeAdjustedTypicalClosing else 0 end), 0)
	from (select 1 as x) x
		left outer join A.Daily b with (nolock) on b.SymbolID = @SymbolID and b.Seq = @Seq - @PeriodID --+ 1
	/*select  @A0_Date = isnull(cast(Date as float),0), @A0_Opening = isnull(Opening,0), @A0_High = isnull(High,0), @A0_Low = isnull(Low,0), @A0_Closing = isnull(Closing,0), @A0_Volume = isnull(Volume,0), @A0_Interest = isnull(Interest,0), @A0_GainClosing = isnull((case when DiffClosing >=0 then DiffClosing else 0 end), 0), @A0_LossClosing = isnull((case when DiffClosing <=0 then -DiffClosing else 0 end), 0), @A0_GainVolume = isnull((case when DiffVolume >=0 then DiffVolume else 0 end), 0), @A0_LossVolume = isnull((case when DiffVolume <=0 then -DiffVolume else 0 end), 0), @A0_WeightedClosing = isnull(WeightedClosing,0), @A0_GainWeightedClosing = isnull((case when DiffWeightedClosing >=0 then DiffWeightedClosing else 0 end), 0), @A0_LossWeightedClosing = isnull((case when DiffWeightedClosing <=0 then -DiffWeightedClosing else 0 end), 0), @A0_TypicalClosing = isnull(TypicalClosing,0), @A0_GainTypicalClosing = isnull((case when DiffTypicalClosing >=0 then DiffTypicalClosing else 0 end), 0), @A0_LossTypicalClosing = isnull((case when DiffTypicalClosing <=0 then -DiffTypicalClosing else 0 end), 0), @A0_VolumeAdjustedClosing = isnull(VolumeAdjustedClosing,0), @A0_GainVolumeAdjustedClosing = isnull((case when DiffVolumeAdjustedClosing >=0 then DiffVolumeAdjustedClosing else 0 end), 0), @A0_LossVolumeAdjustedClosing = isnull((case when DiffVolumeAdjustedClosing <=0 then -DiffVolumeAdjustedClosing else 0 end), 0), @A0_VolumeAdjustedWeightedClosing = isnull(VolumeAdjustedWeightedClosing,0), @A0_GainVolumeAdjustedWeightedClosing = isnull((case when DiffVolumeAdjustedWeightedClosing >=0 then DiffVolumeAdjustedWeightedClosing else 0 end), 0), @A0_LossVolumeAdjustedWeightedClosing = isnull((case when DiffVolumeAdjustedWeightedClosing <=0 then -DiffVolumeAdjustedWeightedClosing else 0 end), 0), @A0_VolumeAdjustedTypicalClosing = isnull(VolumeAdjustedTypicalClosing,0), @A0_GainVolumeAdjustedTypicalClosing = isnull((case when DiffVolumeAdjustedTypicalClosing >=0 then DiffVolumeAdjustedTypicalClosing else 0 end), 0), @A0_LossVolumeAdjustedTypicalClosing = isnull((case when DiffVolumeAdjustedTypicalClosing <=0 then -DiffVolumeAdjustedTypicalClosing else 0 end), 0)
	from (select 1 as x) x
		left outer join @a b on b.Seq = @Seq - @PeriodID --+ 1*/
	/*Read Period Daily*/
	select top 1  @A1_Date = isnull(cast(Date as float),0), @A1_Opening = isnull(Opening,0), @A1_High = isnull(High,0), @A1_Low = isnull(Low,0), @A1_Closing = isnull(Closing,0), @A1_Volume = isnull(Volume,0), @A1_Interest = isnull(Interest,0), @A1_GainClosing = isnull((case when DiffClosing >=0 then DiffClosing else 0 end), 0), @A1_LossClosing = isnull((case when DiffClosing <=0 then -DiffClosing else 0 end), 0), @A1_GainVolume = isnull((case when DiffVolume >=0 then DiffVolume else 0 end), 0), @A1_LossVolume = isnull((case when DiffVolume <=0 then -DiffVolume else 0 end), 0), @A1_WeightedClosing = isnull(WeightedClosing,0), @A1_GainWeightedClosing = isnull((case when DiffWeightedClosing >=0 then DiffWeightedClosing else 0 end), 0), @A1_LossWeightedClosing = isnull((case when DiffWeightedClosing <=0 then -DiffWeightedClosing else 0 end), 0), @A1_TypicalClosing = isnull(TypicalClosing,0), @A1_GainTypicalClosing = isnull((case when DiffTypicalClosing >=0 then DiffTypicalClosing else 0 end), 0), @A1_LossTypicalClosing = isnull((case when DiffTypicalClosing <=0 then -DiffTypicalClosing else 0 end), 0), @A1_VolumeAdjustedClosing = isnull(VolumeAdjustedClosing,0), @A1_GainVolumeAdjustedClosing = isnull((case when DiffVolumeAdjustedClosing >=0 then DiffVolumeAdjustedClosing else 0 end), 0), @A1_LossVolumeAdjustedClosing = isnull((case when DiffVolumeAdjustedClosing <=0 then -DiffVolumeAdjustedClosing else 0 end), 0), @A1_VolumeAdjustedWeightedClosing = isnull(VolumeAdjustedWeightedClosing,0), @A1_GainVolumeAdjustedWeightedClosing = isnull((case when DiffVolumeAdjustedWeightedClosing >=0 then DiffVolumeAdjustedWeightedClosing else 0 end), 0), @A1_LossVolumeAdjustedWeightedClosing = isnull((case when DiffVolumeAdjustedWeightedClosing <=0 then -DiffVolumeAdjustedWeightedClosing else 0 end), 0), @A1_VolumeAdjustedTypicalClosing = isnull(VolumeAdjustedTypicalClosing,0), @A1_GainVolumeAdjustedTypicalClosing = isnull((case when DiffVolumeAdjustedTypicalClosing >=0 then DiffVolumeAdjustedTypicalClosing else 0 end), 0), @A1_LossVolumeAdjustedTypicalClosing = isnull((case when DiffVolumeAdjustedTypicalClosing <=0 then -DiffVolumeAdjustedTypicalClosing else 0 end), 0)
	from A.Daily with(nolock)
	where SymbolID = @SymbolID
		and Seq >= @Seq - @PeriodID + 1
	order by SymbolID, Seq
	/*select top 1  @A1_Date = isnull(cast(Date as float),0), @A1_Opening = isnull(Opening,0), @A1_High = isnull(High,0), @A1_Low = isnull(Low,0), @A1_Closing = isnull(Closing,0), @A1_Volume = isnull(Volume,0), @A1_Interest = isnull(Interest,0), @A1_GainClosing = isnull((case when DiffClosing >=0 then DiffClosing else 0 end), 0), @A1_LossClosing = isnull((case when DiffClosing <=0 then -DiffClosing else 0 end), 0), @A1_GainVolume = isnull((case when DiffVolume >=0 then DiffVolume else 0 end), 0), @A1_LossVolume = isnull((case when DiffVolume <=0 then -DiffVolume else 0 end), 0), @A1_WeightedClosing = isnull(WeightedClosing,0), @A1_GainWeightedClosing = isnull((case when DiffWeightedClosing >=0 then DiffWeightedClosing else 0 end), 0), @A1_LossWeightedClosing = isnull((case when DiffWeightedClosing <=0 then -DiffWeightedClosing else 0 end), 0), @A1_TypicalClosing = isnull(TypicalClosing,0), @A1_GainTypicalClosing = isnull((case when DiffTypicalClosing >=0 then DiffTypicalClosing else 0 end), 0), @A1_LossTypicalClosing = isnull((case when DiffTypicalClosing <=0 then -DiffTypicalClosing else 0 end), 0), @A1_VolumeAdjustedClosing = isnull(VolumeAdjustedClosing,0), @A1_GainVolumeAdjustedClosing = isnull((case when DiffVolumeAdjustedClosing >=0 then DiffVolumeAdjustedClosing else 0 end), 0), @A1_LossVolumeAdjustedClosing = isnull((case when DiffVolumeAdjustedClosing <=0 then -DiffVolumeAdjustedClosing else 0 end), 0), @A1_VolumeAdjustedWeightedClosing = isnull(VolumeAdjustedWeightedClosing,0), @A1_GainVolumeAdjustedWeightedClosing = isnull((case when DiffVolumeAdjustedWeightedClosing >=0 then DiffVolumeAdjustedWeightedClosing else 0 end), 0), @A1_LossVolumeAdjustedWeightedClosing = isnull((case when DiffVolumeAdjustedWeightedClosing <=0 then -DiffVolumeAdjustedWeightedClosing else 0 end), 0), @A1_VolumeAdjustedTypicalClosing = isnull(VolumeAdjustedTypicalClosing,0), @A1_GainVolumeAdjustedTypicalClosing = isnull((case when DiffVolumeAdjustedTypicalClosing >=0 then DiffVolumeAdjustedTypicalClosing else 0 end), 0), @A1_LossVolumeAdjustedTypicalClosing = isnull((case when DiffVolumeAdjustedTypicalClosing <=0 then -DiffVolumeAdjustedTypicalClosing else 0 end), 0)
	from @a
	where Seq >= @Seq - @PeriodID + 1
	order by Seq*/
	/*Read This Period*/
	select  @A2_Date = isnull(cast(Date as float),0), @A2_Opening = isnull(Opening,0), @A2_High = isnull(High,0), @A2_Low = isnull(Low,0), @A2_Closing = isnull(Closing,0), @A2_Volume = isnull(Volume,0), @A2_Interest = isnull(Interest,0), @A2_GainClosing = isnull((case when DiffClosing >=0 then DiffClosing else 0 end), 0), @A2_LossClosing = isnull((case when DiffClosing <=0 then -DiffClosing else 0 end), 0), @A2_GainVolume = isnull((case when DiffVolume >=0 then DiffVolume else 0 end), 0), @A2_LossVolume = isnull((case when DiffVolume <=0 then -DiffVolume else 0 end), 0), @A2_WeightedClosing = isnull(WeightedClosing,0), @A2_GainWeightedClosing = isnull((case when DiffWeightedClosing >=0 then DiffWeightedClosing else 0 end), 0), @A2_LossWeightedClosing = isnull((case when DiffWeightedClosing <=0 then -DiffWeightedClosing else 0 end), 0), @A2_TypicalClosing = isnull(TypicalClosing,0), @A2_GainTypicalClosing = isnull((case when DiffTypicalClosing >=0 then DiffTypicalClosing else 0 end), 0), @A2_LossTypicalClosing = isnull((case when DiffTypicalClosing <=0 then -DiffTypicalClosing else 0 end), 0), @A2_VolumeAdjustedClosing = isnull(VolumeAdjustedClosing,0), @A2_GainVolumeAdjustedClosing = isnull((case when DiffVolumeAdjustedClosing >=0 then DiffVolumeAdjustedClosing else 0 end), 0), @A2_LossVolumeAdjustedClosing = isnull((case when DiffVolumeAdjustedClosing <=0 then -DiffVolumeAdjustedClosing else 0 end), 0), @A2_VolumeAdjustedWeightedClosing = isnull(VolumeAdjustedWeightedClosing,0), @A2_GainVolumeAdjustedWeightedClosing = isnull((case when DiffVolumeAdjustedWeightedClosing >=0 then DiffVolumeAdjustedWeightedClosing else 0 end), 0), @A2_LossVolumeAdjustedWeightedClosing = isnull((case when DiffVolumeAdjustedWeightedClosing <=0 then -DiffVolumeAdjustedWeightedClosing else 0 end), 0), @A2_VolumeAdjustedTypicalClosing = isnull(VolumeAdjustedTypicalClosing,0), @A2_GainVolumeAdjustedTypicalClosing = isnull((case when DiffVolumeAdjustedTypicalClosing >=0 then DiffVolumeAdjustedTypicalClosing else 0 end), 0), @A2_LossVolumeAdjustedTypicalClosing = isnull((case when DiffVolumeAdjustedTypicalClosing <=0 then -DiffVolumeAdjustedTypicalClosing else 0 end), 0)
	from A.Daily with(nolock)
	where SymbolID = @SymbolID
		and Seq = @Seq

/* calculate */

	--Date From
	select @DateFrom = @A1_Date
	--Date to
	select @DateTo = @A2_Date
	--The opening price of the period
	select @Opening = @A1_Opening
	--The highest price in the period
	select @High = case when @LastHigh < @A2_High then @A2_High else @LastHigh end
	--The lowest price in the period
	select @Low = case when @Seq = 1 then @A2_Low else case when @LastLow > @A2_Low then @A2_Low else @LastLow end end
	--The last price of the period
	select @Closing = @A2_Closing
	--Total volume in the period
	select @Volume = @LastTotalVolume + @A2_Volume - @A0_Volume

	--Total of Closing
	select @TotalClosing = @LastTotalClosing + @A2_Closing - @A0_Closing
	--Total of Weighted Closing
	select @TotalWeightedClosing = @LastTotalWeightedClosing + @A2_WeightedClosing - @A0_WeightedClosing
	--Total of Typical Closing
	select @TotalTypicalClosing = @LastTotalTypicalClosing + @A2_TypicalClosing - @A0_TypicalClosing
	--Total of Weighted Closing -- used for WMA
	select @TotalWeightedNormalClosing = @LastTotalWeightedNormalClosing + @F_Period * @A2_Closing - @LastTotalClosing
	--Total of Weighted Weighted Closing -- used for WMA
	select @TotalWeightedWeightedClosing = @LastTotalWeightedWeightedClosing + @F_Period * @A2_WeightedClosing - @LastTotalWeightedClosing
	--Total of Weighted Typical Closing -- used for WMA
	select @TotalWeightedTypicalClosing = @LastTotalWeightedTypicalClosing + @F_Period * @A2_TypicalClosing - @LastTotalTypicalClosing
	--Total of Volume
	select @TotalVolume = @LastTotalVolume + @A2_Volume - @A0_Volume
	--Total of Volume Adjusted Closing -- use for VAMA
	select @TotalVolumeAdjustedClosing = @LastTotalVolumeAdjustedClosing + @A2_VolumeAdjustedClosing - @A0_VolumeAdjustedClosing
	--Total of Volume Adjusted Typical Closing -- use for VAMA
	select @TotalVolumeAdjustedTypicalClosing = @LastTotalVolumeAdjustedTypicalClosing + @A2_VolumeAdjustedTypicalClosing - @A0_VolumeAdjustedTypicalClosing
	--Total of Volume Adjusted Weighted Closing -- use for VAMA
	select @TotalVolumeAdjustedWeightedClosing = @LastTotalVolumeAdjustedWeightedClosing + @A2_VolumeAdjustedWeightedClosing - @A0_VolumeAdjustedWeightedClosing
	--Total Gain by comparing Closing
	select @TotalGainClosing = @LastTotalGainClosing + @A2_GainClosing - @A0_GainClosing
	--Total Loss by comparing Closing
	select @TotalLossClosing = @LastTotalLossClosing + @A2_LossClosing - @A0_LossClosing
	--Total Gain by comparing Volume
	select @TotalGainVolume = @LastTotalGainVolume + @A2_GainVolume - @A0_GainVolume
	--Total Loss by comparing Closing
	select @TotalLossVolume = @LastTotalLossVolume + @A2_LossVolume - @A0_LossVolume
	--Total Gain by comparing Weighted Closing
	select @TotalGainWeightedClosing = @LastTotalGainWeightedClosing + @A2_GainWeightedClosing - @A0_GainWeightedClosing
	--Total Loss by comparing Weighted Closing
	select @TotalLossWeightedClosing = @LastTotalLossWeightedClosing + @A2_LossWeightedClosing - @A0_LossWeightedClosing
	--Total Gain by comparing Typical Closing
	select @TotalGainTypicalClosing = @LastTotalGainTypicalClosing + @A2_GainTypicalClosing - @A0_GainTypicalClosing
	--Total Loss by comparing Typical Closing
	select @TotalLossTypicalClosing = @LastTotalLossTypicalClosing + @A2_LossTypicalClosing - @A0_LossTypicalClosing
	--Total Gain by comparing Volume Adjusted Closing
	select @TotalGainVolumeAdjustedClosing = @LastTotalGainVolumeAdjustedClosing + @A2_GainVolumeAdjustedClosing - @A0_GainVolumeAdjustedClosing
	--Total Loss by comparing VolumeAdjustedClosin Adjusted Closing
	select @TotalLossVolumeAdjustedClosing = @LastTotalLossVolumeAdjustedClosing + @A2_LossVolumeAdjustedClosing - @A0_LossVolumeAdjustedClosing
	--Total Gain by comparing Volume Adjusted Weighted Closing
	select @TotalGainVolumeAdjustedWeightedClosing = @LastTotalGainVolumeAdjustedWeightedClosing + @A2_GainVolumeAdjustedWeightedClosing - @A0_GainVolumeAdjustedWeightedClosing
	--Total Loss by comparing VolumeAdjustedWeightedClosing Adjusted Weighted Closing
	select @TotalLossVolumeAdjustedWeightedClosing = @LastTotalLossVolumeAdjustedWeightedClosing + @A2_LossVolumeAdjustedWeightedClosing - @A0_LossVolumeAdjustedWeightedClosing
	--Total Gain by comparing Volume Adjusted Typical Closing
	select @TotalGainVolumeAdjustedTypicalClosing = @LastTotalGainVolumeAdjustedTypicalClosing + @A2_GainVolumeAdjustedTypicalClosing - @A0_GainVolumeAdjustedTypicalClosing
	--Total Loss by comparing VolumeAdjustedTypicalClosingAdjustedTypicalClosing
	select @TotalLossVolumeAdjustedTypicalClosing = @LastTotalLossVolumeAdjustedTypicalClosing + @A2_LossVolumeAdjustedTypicalClosing - @A0_LossVolumeAdjustedTypicalClosing

	--Simple Moving Average of Closing
	select @SMAClosing = @TotalClosing / @F_Period
	--Simple Moving Average of Weighted Closing
	select @SMAWeightedClosing = @TotalWeightedClosing / @F_Period
	--Simple Moving Average of Typical Closing
	select @SMATypicalClosing = @TotalTypicalClosing / @F_Period
	--Simple Moving Average of Volume
	select @SMAVolume = @TotalVolume / @F_Period
	--Simple Moving Average of Gain by comparing Closing
	select @SMAGainClosing = @TotalGainClosing / @F_Period
	--Simple Moving Average of Loss by comparing Closing
	select @SMALossClosing = @TotalLossClosing / @F_Period
	--Simple Moving Average of Gain by comparing Volume
	select @SMAGainVolume = @TotalGainVolume / @F_Period
	--Simple Moving Average of Loss by comparing Closing
	select @SMALossVolume = @TotalLossVolume / @F_Period
	--Simple Moving Average of Gain by comparing Weighted Closing
	select @SMAGainWeightedClosing = @TotalGainWeightedClosing / @F_Period
	--Simple Moving Average of Loss by comparing Weighted Closing
	select @SMALossWeightedClosing = @TotalLossWeightedClosing / @F_Period
	--Simple Moving Average of Gain by comparing Typical Closing
	select @SMAGainTypicalClosing = @TotalGainTypicalClosing / @F_Period
	--Simple Moving Average of Loss by comparing Typical Closing
	select @SMALossTypicalClosing = @TotalLossTypicalClosing / @F_Period
	--Simple Moving Average of Gain by comparing Volume Adjusted Closing
	select @SMAGainVolumeAdjustedClosing = @TotalGainVolumeAdjustedClosing / @F_Period
	--Simple Moving Average of Loss by comparing Volume Adjusted Closing
	select @SMALossVolumeAdjustedClosing = @TotalLossVolumeAdjustedClosing / @F_Period
	--Simple Moving Average of Gain by comparing Volume Adjusted Weighted Closing
	select @SMAGainVolumeAdjustedWeightedClosing = @TotalGainVolumeAdjustedWeightedClosing / @F_Period
	--Simple Moving Average of Loss by comparing Volume Adjusted Weighted Closing
	select @SMALossVolumeAdjustedWeightedClosing = @TotalLossVolumeAdjustedWeightedClosing / @F_Period
	--Simple Moving Average of Gain by comparing Volume Adjusted Typical Closing
	select @SMAGainVolumeAdjustedTypicalClosing = @TotalGainVolumeAdjustedTypicalClosing / @F_Period
	--Simple Moving Average of Loss by comparing VolumeAdjustedTypicalClosing
	select @SMALossVolumeAdjustedTypicalClosing = @TotalLossVolumeAdjustedTypicalClosing / @F_Period

	select @alpha = @TotalVolume/@F_Thousand
	--Volume Adjusted Moving Average of Closing
	select @VAMAClosing = case when @alpha = 0 then 0 else @TotalVolumeAdjustedClosing / @alpha end
	--Volume Adjusted Moving Average of Typical Closing
	select @VAMATypicalClosing = case when @alpha = 0 then 0 else @TotalVolumeAdjustedTypicalClosing / @alpha end
	--Volume Adjusted Moving Average of Weighted Closing
	select @VAMAWeightedClosing = case when @alpha = 0 then 0 else @TotalVolumeAdjustedWeightedClosing / @alpha end

	select @alpha = @F_Period * (@F_Period + @F_One) / (@F_Two)
	--Weighted Moving Average of Closing
	select @WMAClosing = @TotalWeightedNormalClosing / @alpha
	--Weighted Moving Average of Weighted Closing
	select @WMAWeightedClosing = @TotalWeightedWeightedClosing / @alpha
	--Weighted Moving Average of Typical Closing
	select @WMATypicalClosing = @TotalWeightedTypicalClosing / @alpha

	select @alpha = @F_Two / (@F_Period + @F_One)
	select @beta = @F_One - @alpha
	--Exponential Moving Average of Closing
	select @EMAClosing = case when @Seq = 1 then @A2_Closing else @A2_Closing * @alpha + @beta * @LastEMAClosing end
	--Exponential Moving Average of Weighted Closing
	select @EMAWeightedClosing = case when @Seq = 1 then @A2_WeightedClosing else @A2_WeightedClosing * @alpha + @beta * @LastEMAWeightedClosing end
	--Exponential Moving Average of Typical Closing
	select @EMATypicalClosing = case when @Seq = 1 then @A2_TypicalClosing else @A2_TypicalClosing * @alpha + @beta * @LastEMATypicalClosing end
	--Exponential Moving Average of Volume
	select @EMAVolume = case when @Seq = 1 then @A2_Volume else @A2_Volume * @alpha + @beta * @LastEMAVolume end
	--Exponential Moving Average of Gain by comparing Closing
	select @EMAGainClosing = case when @Seq = 1 then @A2_GainClosing else @A2_GainClosing * @alpha + @beta * @LastEMAGainClosing end
	--Exponential Moving Average of Loss by comparing Closing
	select @EMALossClosing = case when @Seq = 1 then @A2_LossClosing else @A2_LossClosing * @alpha + @beta * @LastEMALossClosing end
	--Exponential Moving Average of Gain by comparing Volume
	select @EMAGainVolume = case when @Seq = 1 then @A2_GainVolume else @A2_GainVolume * @alpha + @beta * @LastEMAGainVolume end
	--Exponential Moving Average of Loss by comparing Volume
	select @EMALossVolume = case when @Seq = 1 then @A2_LossVolume else @A2_LossVolume * @alpha + @beta * @LastEMALossVolume end
	--Exponential Moving Average of Gain by comparing Weighted Closing
	select @EMAGainWeightedClosing = case when @Seq = 1 then @A2_GainWeightedClosing else @A2_GainWeightedClosing * @alpha + @beta * @LastEMAGainWeightedClosing end
	--Exponential Moving Average of Loss by comparing Weighted Closing
	select @EMALossWeightedClosing = case when @Seq = 1 then @A2_LossWeightedClosing else @A2_LossWeightedClosing * @alpha + @beta * @LastEMALossWeightedClosing end
	--Exponential Moving Average of Gain by comparing Typical Closing
	select @EMAGainTypicalClosing = case when @Seq = 1 then @A2_GainTypicalClosing else @A2_GainTypicalClosing * @alpha + @beta * @LastEMAGainTypicalClosing end
	--Exponential Moving Average of Loss by comparing Typical Closing
	select @EMALossTypicalClosing = case when @Seq = 1 then @A2_LossTypicalClosing else @A2_LossTypicalClosing * @alpha + @beta * @LastEMALossTypicalClosing end
	--Exponential Moving Average of Gain by comparing Volume Adjusted Closing
	select @EMAGainVolumeAdjustedClosing = case when @Seq = 1 then @A2_GainVolumeAdjustedClosing else @A2_GainVolumeAdjustedClosing * @alpha + @beta * @LastEMAGainVolumeAdjustedClosing end
	--Exponential Moving Average of Loss by comparing Volume Adjusted Closing
	select @EMALossVolumeAdjustedClosing = case when @Seq = 1 then @A2_LossVolumeAdjustedClosing else @A2_LossVolumeAdjustedClosing * @alpha + @beta * @LastEMALossVolumeAdjustedClosing end
	--Exponential Moving Average of Gain by comparing Volume Adjusted Weighted Closing
	select @EMAGainVolumeAdjustedWeightedClosing = case when @Seq = 1 then @A2_GainVolumeAdjustedWeightedClosing else @A2_GainVolumeAdjustedWeightedClosing * @alpha + @beta * @LastEMAGainVolumeAdjustedWeightedClosing end
	--Exponential Moving Average of Loss by comparing Volume Adjusted Weighted Closing
	select @EMALossVolumeAdjustedWeightedClosing = case when @Seq = 1 then @A2_LossVolumeAdjustedWeightedClosing else @A2_LossVolumeAdjustedWeightedClosing * @alpha + @beta * @LastEMALossVolumeAdjustedWeightedClosing end
	--Exponential Moving Average of Gain by comparing Volume Adjusted Typical Closing
	select @EMAGainVolumeAdjustedTypicalClosing = case when @Seq = 1 then @A2_GainVolumeAdjustedTypicalClosing else @A2_GainVolumeAdjustedTypicalClosing * @alpha + @beta * @LastEMAGainVolumeAdjustedTypicalClosing end
	--Exponential Moving Average of Loss by comparing VolumeAdjustedTypicalClosing
	select @EMALossVolumeAdjustedTypicalClosing = case when @Seq = 1 then @A2_LossVolumeAdjustedTypicalClosing else @A2_LossVolumeAdjustedTypicalClosing * @alpha + @beta * @LastEMALossVolumeAdjustedTypicalClosing end

	--Chande Momentum Oscillator of Closing
	select @alpha = @TotalGainClosing - @TotalLossClosing, @beta = @TotalGainClosing + @TotalLossClosing
	select @CMOClosing = case when @beta = 0 then 0 else @F_hundred * @alpha / @beta end
	--Chande Momentum Oscillator of Volume
	select @alpha = @TotalGainVolume - @TotalLossVolume, @beta = @TotalGainVolume + @TotalLossVolume
	select @CMOVolume = case when @beta = 0 then 0 else @F_hundred * @alpha / @beta end
	--Chande Momentum Oscillator of Weighted Closing
	select @alpha = @TotalGainWeightedClosing - @TotalLossWeightedClosing, @beta = @TotalGainWeightedClosing + @TotalLossWeightedClosing
	select @CMOWeightedClosing = case when @beta = 0 then 0 else @F_hundred * @alpha / @beta end
	--Chande Momentum Oscillator of Typical Closing
	select @alpha = @TotalGainTypicalClosing - @TotalLossTypicalClosing, @beta = @TotalGainTypicalClosing + @TotalLossTypicalClosing
	select @CMOTypicalClosing = case when @beta = 0 then 0 else @F_hundred * @alpha / @beta end

	--Variable Moving Average of Closing
	select @alpha = @TotalGainClosing - @TotalLossClosing, @beta = @TotalGainClosing + @TotalLossClosing
	select @gamma = case when @beta = 0 or @F_Period - @F_One = 0 then 0 else (@F_Two * @alpha / @beta)/(@F_Period - @F_One) end
	select @VMAClosing = @gamma * @A2_Closing + (@F_One - @gamma) * @LastVMAClosing
	--Variable Moving Average of Volume
	select @alpha = @TotalGainVolume - @TotalLossVolume, @beta = @TotalGainVolume + @TotalLossVolume
	select @gamma = case when @beta = 0 or @F_Period - @F_One = 0 then 0 else (@F_Two * @alpha / @beta)/(@F_Period - @F_One) end
	select @VMAVolume = @gamma * @A2_Volume + (@F_One - @gamma) * @LastVMAVolume
	--Variable Moving Average of Weighted Closing
	select @alpha = @TotalGainWeightedClosing - @TotalLossWeightedClosing, @beta = @TotalGainWeightedClosing + @TotalLossWeightedClosing
	select @gamma = case when @beta = 0 or @F_Period - @F_One = 0 then 0 else (@F_Two * @alpha / @beta)/(@F_Period - @F_One) end
	select @VMAWeightedClosing = @gamma * @A2_WeightedClosing + (@F_One - @gamma) * @LastVMAWeightedClosing
	--Variable Moving Average of Typical Closing
	select @alpha = @TotalGainTypicalClosing - @TotalLossTypicalClosing, @beta = @TotalGainTypicalClosing + @TotalLossTypicalClosing
	select @gamma = case when @beta = 0 or @F_Period - @F_One = 0 then 0 else (@F_Two * @alpha / @beta)/(@F_Period - @F_One) end
	select @VMATypicalClosing = @gamma * @A2_TypicalClosing + (@F_One - @gamma) * @LastVMATypicalClosing

	--Wilder's Smooth  Closing
	select @WSClosing = @LastSMAClosing + (@A2_Closing - @LastSMAClosing ) / @F_Period
	--Wilder's Smooth  Weighted Closing
	select @WSWeightedClosing = @LastSMAWeightedClosing + (@A2_WeightedClosing - @LastSMAWeightedClosing ) / @F_Period
	--Wilder's Smooth  Typical Closing
	select @WSTypicalClosing = @LastSMATypicalClosing + (@A2_TypicalClosing - @LastSMATypicalClosing ) / @F_Period
	--Wilder's Smooth  Weighted Closing -- used for WMA
	select @WSWeightedNormalClosing = @LastWMAClosing + ( @WMAClosing - @LastWMAClosing) / @F_Period
	--Wilder's Smooth  Weighted Weighted Closing -- used for WMA
	select @WSWeightedWeightedClosing = @LastWMAWeightedClosing + ( @WMAWeightedClosing - @LastWMAWeightedClosing) / @F_Period
	--Wilder's Smooth  Weighted Typical Closing -- used for WMA
	select @WSWeightedTypicalClosing = @LastWMATypicalClosing + (@WMATypicalClosing - @LastWMATypicalClosing ) / @F_Period
	--Wilder's Smooth  Volume
	select @WSVolume = @LastSMAVolume + (@A2_Volume - @LastSMAVolume ) / @F_Period
	--Wilder's Smooth  Volume Adjusted Closing -- use for VAMA
	select @WSVolumeAdjustedClosing = @LastVAMAClosing + (@VAMAClosing - @LastVAMAClosing) / @F_Period
	--Wilder's Smooth  Volume Adjusted Typical Closing -- use for VAMA
	select @WSVolumeAdjustedTypicalClosing = @LastVAMATypicalClosing + (@VAMATypicalClosing - @LastVAMATypicalClosing) / @F_Period
	--Wilder's Smooth  Volume Adjusted Weighted Closing -- use for VAMA
	select @WSVolumeAdjustedWeightedClosing = @LastVAMAWeightedClosing + (@VAMAWeightedClosing - @LastVAMAWeightedClosing) / @F_Period
	--Wilder's Smooth of Gain by comparing Closing
	select @WSGainClosing = @LastSMAGainClosing + (@A2_GainClosing - @LastSMAGainClosing ) / @F_Period
	--Wilder's Smooth of Loss by comparing Closing
	select @WSLossClosing = @LastSMALossClosing + (@A2_LossClosing - @LastSMALossClosing ) / @F_Period
	--Wilder's Smooth of Gain by comparing Volume
	select @WSGainVolume = @LastSMAGainVolume + (@A2_GainVolume - @LastSMAGainVolume ) / @F_Period
	--Wilder's Smooth of Loss by comparing Closing
	select @WSLossVolume = @LastSMALossVolume + (@A2_LossVolume - @LastSMALossVolume ) / @F_Period
	--Wilder's Smooth of Gain by comparing Weighted Closing
	select @WSGainWeightedClosing = @LastSMAGainWeightedClosing + (@A2_GainWeightedClosing - @LastSMAGainWeightedClosing ) / @F_Period
	--Wilder's Smooth of Loss by comparing Weighted Closing
	select @WSLossWeightedClosing = @LastSMALossWeightedClosing + (@A2_LossWeightedClosing - @LastSMALossWeightedClosing ) / @F_Period
	--Wilder's Smooth of Gain by comparing Typical Closing
	select @WSGainTypicalClosing = @LastSMAGainTypicalClosing + (@A2_GainTypicalClosing - @LastSMAGainTypicalClosing ) / @F_Period
	--Wilder's Smooth of Loss by comparing Typical Closing
	select @WSLossTypicalClosing = @LastSMALossTypicalClosing + (@A2_LossTypicalClosing - @LastSMALossTypicalClosing ) / @F_Period
	--Wilder's Smooth of Gain by comparing Volume Adjusted Closing
	select @WSGainVolumeAdjustedClosing = @LastSMAGainVolumeAdjustedClosing + (@A2_GainVolumeAdjustedClosing - @LastSMAGainVolumeAdjustedClosing ) / @F_Period
	--Wilder's Smooth of Loss by comparing Volume Adjusted Closing
	select @WSLossVolumeAdjustedClosing = @LastSMALossVolumeAdjustedClosing + (@A2_LossVolumeAdjustedClosing - @LastSMALossVolumeAdjustedClosing ) / @F_Period
	--Wilder's Smooth of Gain by comparing Volume Adjusted Weighted Closing
	select @WSGainVolumeAdjustedWeightedClosing = @LastSMAGainVolumeAdjustedWeightedClosing + (@A2_GainVolumeAdjustedWeightedClosing - @LastSMAGainVolumeAdjustedWeightedClosing ) / @F_Period
	--Wilder's Smooth of Loss by comparing Volume Adjusted Weighted Closing
	select @WSLossVolumeAdjustedWeightedClosing = @LastSMALossVolumeAdjustedWeightedClosing + (@A2_LossVolumeAdjustedWeightedClosing - @LastSMALossVolumeAdjustedWeightedClosing ) / @F_Period
	--Wilder's Smooth of Gain by comparing Volume Adjusted Typical Closing
	select @WSGainVolumeAdjustedTypicalClosing =  @LastSMAGainVolumeAdjustedTypicalClosing + (@A2_GainVolumeAdjustedTypicalClosing - @LastSMAGainVolumeAdjustedTypicalClosing ) / @F_Period
	--Wilder's Smooth of Loss by comparing VolumeAdjustedTypicalClosing
	select @WSLossVolumeAdjustedTypicalClosing =  @LastSMALossVolumeAdjustedTypicalClosing + (@A2_LossVolumeAdjustedTypicalClosing - @LastSMALossVolumeAdjustedTypicalClosing ) / @F_Period

	--RSI - Simple Moving Average of Closing
	select @alpha = case when @SMALossClosing = 0 then 0 else @SMAGainClosing / @SMALossClosing end + @F_One
	select @RSI_SMAClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Simple Moving Average of Volume
	select @alpha = case when @SMALossVolume = 0 then 0 else @SMAGainVolume / @SMALossVolume end + @F_One
	select @RSI_SMAVolume = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Simple Moving Average of Weighted Closing
	select @alpha = case when @SMALossWeightedClosing = 0 then 0 else @SMAGainWeightedClosing / @SMALossWeightedClosing end + @F_One
	select @RSI_SMAWeightedClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Simple Moving Average of Typical Closing
	select @alpha = case when @SMALossTypicalClosing = 0 then 0 else @SMAGainTypicalClosing / @SMALossTypicalClosing end + @F_One
	select @RSI_SMATypicalClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Simple Moving Average of Volume Adjusted Closing
	select @alpha = case when @SMALossVolumeAdjustedClosing = 0 then 0 else @SMAGainVolumeAdjustedClosing / @SMALossVolumeAdjustedClosing end + @F_One
	select @RSI_SMAVolumeAdjustedClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Simple Moving Average of Volume Adjusted Weighted Closing
	select @alpha = case when @SMALossVolumeAdjustedWeightedClosing = 0 then 0 else @SMAGainVolumeAdjustedWeightedClosing / @SMALossVolumeAdjustedWeightedClosing end + @F_One
	select @RSI_SMAVolumeAdjustedWeightedClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Simple Moving Average of Volume Adjusted Typical Closing
	select @alpha = case when @SMALossVolumeAdjustedTypicalClosing = 0 then 0 else @SMAGainVolumeAdjustedTypicalClosing / @SMALossVolumeAdjustedTypicalClosing end + @F_One
	select @RSI_SMAVolumeAdjustedTypicalClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Exponential Moving Average of Closing
	select @alpha = case when @EMALossClosing = 0 then 0 else @EMAGainClosing / @EMALossClosing end + @F_One
	select @RSI_EMAClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Exponential Moving Average of Volume
	select @alpha = case when @EMALossVolume = 0 then 0 else @EMAGainVolume / @EMALossVolume end + @F_One
	select @RSI_EMAVolume = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Exponential Moving Average of Weighted Closing
	select @alpha = case when @EMALossWeightedClosing = 0 then 0 else @EMAGainWeightedClosing / @EMALossWeightedClosing end + @F_One
	select @RSI_EMAWeightedClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Exponential Moving Average of Typical Closing
	select @alpha = case when @EMALossTypicalClosing = 0 then 0 else @EMAGainTypicalClosing / @EMALossTypicalClosing end + @F_One
	select @RSI_EMATypicalClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Exponential Moving Average of Volume Adjusted Closing
	select @alpha = case when @EMALossVolumeAdjustedClosing = 0 then 0 else @EMAGainVolumeAdjustedClosing / @EMALossVolumeAdjustedClosing end + @F_One
	select @RSI_EMAVolumeAdjustedClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Exponential Moving Average of Volume Adjusted Weighted Closing
	select @alpha = case when @EMALossVolumeAdjustedWeightedClosing = 0 then 0 else @EMAGainVolumeAdjustedWeightedClosing / @EMALossVolumeAdjustedWeightedClosing end + @F_One
	select @RSI_EMAVolumeAdjustedWeightedClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Exponential Moving Average of Volume Adjusted Typical Closing
	select @alpha = case when @EMALossVolumeAdjustedTypicalClosing = 0 then 0 else @EMAGainVolumeAdjustedTypicalClosing / @EMALossVolumeAdjustedTypicalClosing end + @F_One
	select @RSI_EMAVolumeAdjustedTypicalClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Wilder's Smooth of Closing
	select @alpha = case when @WSLossClosing = 0 then 0 else @WSGainClosing / @WSLossClosing end + @F_One
	select @RSI_WSClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Wilder's Smooth of Volume
	select @alpha = case when @WSLossVolume = 0 then 0 else @WSGainVolume / @WSLossVolume end + @F_One
	select @RSI_WSVolume = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Wilder's Smooth of Weighted Closing
	select @alpha = case when @WSLossWeightedClosing = 0 then 0 else @WSGainWeightedClosing / @WSLossWeightedClosing end + @F_One
	select @RSI_WSWeightedClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Wilder's Smooth of Typical Closing
	select @alpha = case when @WSLossTypicalClosing = 0 then 0 else @WSGainTypicalClosing / @WSLossTypicalClosing end + @F_One
	select @RSI_WSTypicalClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Wilder's Smooth of Volume Adjusted Closing
	select @alpha = case when @WSLossVolumeAdjustedClosing = 0 then 0 else @WSGainVolumeAdjustedClosing / @WSLossVolumeAdjustedClosing end + @F_One
	select @RSI_WSVolumeAdjustedClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Wilder's Smooth of Volume Adjusted Weighted Closing
	select @alpha = case when @WSLossVolumeAdjustedWeightedClosing = 0 then 0 else @WSGainVolumeAdjustedWeightedClosing / @WSLossVolumeAdjustedWeightedClosing end + @F_One
	select @RSI_WSVolumeAdjustedWeightedClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--RSI - Wilder's Smooth of Volume Adjusted Typical Closing
	select @alpha = case when @WSLossVolumeAdjustedTypicalClosing = 0 then 0 else @WSGainVolumeAdjustedTypicalClosing / @WSLossVolumeAdjustedTypicalClosing end + @F_One
	select @RSI_WSVolumeAdjustedTypicalClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	
	--Calculate values that not related to yesterday's values
	select
		--Standard Deviation of Closing
		@STDEVClosing = isnull(stdev(Closing), 0),
		--Standard Deviation of Weighted Closing
		@STDEVWeightedClosing = isnull(stdev(WeightedClosing), 0),
		--Standard Deviation of Typical Closing
		@STDEVTypicalClosing = isnull(stdev(TypicalClosing), 0),
		--Standard Deviation of Volume
		@STDEVVolume = isnull(stdev(Volume), 0),
		--Standard Deviation of Gain by comparing Closing
		@STDEVGainClosing = isnull(stdev(case when  DiffClosing > 0 then DiffClosing else 0 end), 0),
		--Standard Deviation of Loss by comparing Closing
		@STDEVLossClosing = isnull(stdev(case when DiffClosing < 0 then -DiffClosing else 0 end), 0),
		--Standard Deviation of Gain by comparing Volume
		@STDEVGainVolume = isnull(stdev(case when  DiffVolume > 0 then DiffVolume else 0 end), 0),
		--Standard Deviation of Loss by comparing Closing
		@STDEVLossVolume = isnull(stdev(case when DiffVolume < 0 then -DiffVolume else 0 end), 0),
		--Standard Deviation of Gain by comparing Weighted Closing
		@STDEVGainWeightedClosing = isnull(stdev(case when  DiffWeightedClosing > 0 then DiffWeightedClosing else 0 end), 0),
		--Standard Deviation of Loss by comparing Weighted Closing
		@STDEVLossWeightedClosing = isnull(stdev(case when DiffWeightedClosing < 0 then -DiffWeightedClosing else 0 end), 0),
		--Standard Deviation of Gain by comparing Typical Closing
		@STDEVGainTypicalClosing = isnull(stdev(case when  DiffTypicalClosing > 0 then DiffTypicalClosing else 0 end), 0),
		--Standard Deviation of Loss by comparing Typical Closing
		@STDEVLossTypicalClosing = isnull(stdev(case when DiffTypicalClosing < 0 then -DiffTypicalClosing else 0 end), 0),
		--Standard Deviation of Gain by comparing Volume Adjusted Closing
		@STDEVGainVolumeAdjustedClosing = isnull(stdev(case when  DiffTypicalClosing > 0 then DiffTypicalClosing else 0 end), 0),
		--Standard Deviation of Loss by comparing Volume Adjusted Closing
		@STDEVLossVolumeAdjustedClosing = isnull(stdev(case when  DiffTypicalClosing< 0 then -DiffTypicalClosing else 0 end), 0),
		--Standard Deviation of Gain by comparing Volume Adjusted Weighted Closing
		@STDEVGainVolumeAdjustedWeightedClosing = isnull(stdev(case when  DiffVolumeAdjustedWeightedClosing > 0 then DiffVolumeAdjustedWeightedClosing else 0 end), 0),
		--Standard Deviation of Loss by comparing Volume Adjusted Weighted Closing
		@STDEVLossVolumeAdjustedWeightedClosing = isnull(stdev(case when DiffVolumeAdjustedWeightedClosing < 0 then -DiffVolumeAdjustedWeightedClosing else 0 end), 0),
		--Standard Deviation of Gain by comparing Volume Adjusted Typical Closing
		@STDEVGainVolumeAdjustedTypicalClosing = isnull(stdev(case when  DiffVolumeAdjustedTypicalClosing > 0 then DiffVolumeAdjustedTypicalClosing else 0 end), 0),
		--Standard Deviation of Loss by comparing VolumeAdjustedTypicalClosing
		@STDEVLossVolumeAdjustedTypicalClosing = isnull(stdev(case when DiffVolumeAdjustedTypicalClosing < 0 then -DiffVolumeAdjustedTypicalClosing else 0 end), 0)
	from A.Daily 
	where SymbolID = @SymbolID
		and Seq between @Seq - @PeriodID +1 and @Seq

	--Standard Deviation of Closing
	select @alpha = case when @STDEVLossClosing = 0 then 0 else @STDEVGainClosing / @STDEVLossClosing end + @F_One
	select @RVIClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--Standard Deviation of Volume
	select @alpha = case when @STDEVLossVolume = 0 then 0 else @STDEVGainVolume / @STDEVLossVolume end + @F_One
	select @RVIVolume = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--Standard Deviation of Weighted Closing
	select @alpha = case when @STDEVLossWeightedClosing = 0 then 0 else @STDEVGainWeightedClosing / @STDEVLossWeightedClosing end + @F_One
	select @RVIWeightedClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--Standard Deviation of Typical Closing
	select @alpha = case when @STDEVLossTypicalClosing = 0 then 0 else @STDEVGainTypicalClosing / @STDEVLossTypicalClosing end + @F_One
	select @RVITypicalClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--Standard Deviation of Volume Adjusted Closing
	select @alpha = case when @STDEVLossVolumeAdjustedClosing = 0 then 0 else @STDEVGainVolumeAdjustedClosing / @STDEVLossVolumeAdjustedClosing end + @F_One
	select @RVIVolumeAdjustedClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--Standard Deviation of Volume Adjusted Weighted Closing
	select @alpha = case when @STDEVLossVolumeAdjustedWeightedClosing = 0 then 0 else @STDEVGainVolumeAdjustedWeightedClosing / @STDEVLossVolumeAdjustedWeightedClosing end + @F_One
	select @RVIVolumeAdjustedWeightedClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end
	--Standard Deviation of Volume Adjusted Typical Closing
	select @alpha = case when @STDEVLossVolumeAdjustedTypicalClosing = 0 then 0 else @STDEVGainVolumeAdjustedTypicalClosing / @STDEVLossVolumeAdjustedTypicalClosing end + @F_One
	select @RVIVolumeAdjustedTypicalClosing = case when @alpha = 0 then 0 else @F_Hundred - @F_Hundred / @alpha end


	--The Highest The opening price of the period
	select @HighestOpening = case when @LastHighestOpening < @A2_Opening then @A2_Opening else @LastOpening end
	--The Lowest The opening price of the period
	select @LowestOpening = case when @Seq = 1 then @A2_Opening else case when @LastOpening > @A2_Opening then @A2_Opening else @LastOpening end end
	--The Highest The highest price in the period
	select @HighestHigh = case when @LastHighestHigh < @A2_High then @A2_High else @LastHigh end
	--The Lowest The highest price in the period
	select @LowestHigh = case when @Seq = 1 then @A2_High else case when @LastHigh > @A2_High then @A2_High else @LastHigh end end
	--The Highest The lowest price in the period
	select @HighestLow = case when @LastHighestLow < @A2_Low then @A2_Low else @LastLow end
	--The Lowest The lowest price in the period
	select @LowestLow = case when @Seq = 1 then @A2_Low else case when @LastLow > @A2_Low then @A2_Low else @LastLow end end
	--The Highest The last price of the period
	select @HighestClosing = case when @LastHighestClosing < @A2_Closing then @A2_Closing else @LastClosing end
	--The Lowest The last price of the period
	select @LowestClosing = case when @Seq = 1 then @A2_Closing else case when @LastClosing > @A2_Closing then @A2_Closing else @LastClosing end end
	--The Highest Total volume inthe period
	select @HighestVolume = case when @LastHighestVolume < @A2_Volume then @A2_Volume else @LastVolume end
	--The Lowest Total volume inthe period
	select @LowestVolume = case when @Seq = 1 then @A2_Volume else case when @LastVolume > @A2_Volume then @A2_Volume else @LastVolume end end
	--The Highest Simple Moving Average of Closing
	select @HighestSMAClosing = case when @LastHighestSMAClosing < @SMAClosing then @SMAClosing else @LastSMAClosing end
	--The Lowest Simple Moving Average of Closing
	select @LowestSMAClosing = case when @Seq = 1 then @SMAClosing else case when @LastSMAClosing > @SMAClosing then @SMAClosing else @LastSMAClosing end end
	--The Highest Simple Moving Average of Weighted Closing
	select @HighestSMAWeightedClosing = case when @LastHighestSMAWeightedClosing < @SMAWeightedClosing then @SMAWeightedClosing else @LastSMAWeightedClosing end
	--The Lowest Simple Moving Average of Weighted Closing
	select @LowestSMAWeightedClosing = case when @Seq = 1 then @SMAWeightedClosing else case when @LastSMAWeightedClosing > @SMAWeightedClosing then @SMAWeightedClosing else @LastSMAWeightedClosing end end
	--The Highest Simple Moving Average of Typical Closing
	select @HighestSMATypicalClosing = case when @LastHighestSMATypicalClosing < @SMATypicalClosing then @SMATypicalClosing else @LastSMATypicalClosing end
	--The Lowest Simple Moving Average of Typical Closing
	select @LowestSMATypicalClosing = case when @Seq = 1 then @SMATypicalClosing else case when @LastSMATypicalClosing > @SMATypicalClosing then @SMATypicalClosing else @LastSMATypicalClosing end end
	--The Highest Simple Moving Average of Volume
	select @HighestSMAVolume = case when @LastHighestSMAVolume < @SMAVolume then @SMAVolume else @LastSMAVolume end
	--The Lowest Simple Moving Average of Volume
	select @LowestSMAVolume = case when @Seq = 1 then @SMAVolume else case when @LastSMAVolume > @SMAVolume then @SMAVolume else @LastSMAVolume end end
	--The Highest Volume Adjusted Moving Average of Closing
	select @HighestVAMAClosing = case when @LastHighestVAMAClosing < @VAMAClosing then @VAMAClosing else @LastVAMAClosing end
	--The Lowest Volume Adjusted Moving Average of Closing
	select @LowestVAMAClosing = case when @Seq = 1 then @VAMAClosing else case when @LastVAMAClosing > @VAMAClosing then @VAMAClosing else @LastVAMAClosing end end
	--The Highest Volume Adjusted Moving Average of Typical Closing
	select @HighestVAMATypicalClosing = case when @LastHighestVAMATypicalClosing < @VAMATypicalClosing then @VAMATypicalClosing else @LastVAMATypicalClosing end
	--The Lowest Volume Adjusted Moving Average of Typical Closing
	select @LowestVAMATypicalClosing = case when @Seq = 1 then @VAMATypicalClosing else case when @LastVAMATypicalClosing > @VAMATypicalClosing then @VAMATypicalClosing else @LastVAMATypicalClosing end end
	--The Highest Volume Adjusted Moving Average of Weighted Closing
	select @HighestVAMAWeightedClosing = case when @LastHighestVAMAWeightedClosing < @VAMAWeightedClosing then @VAMAWeightedClosing else @LastVAMAWeightedClosing end
	--The Lowest Volume Adjusted Moving Average of Weighted Closing
	select @LowestVAMAWeightedClosing = case when @Seq = 1 then @VAMAWeightedClosing else case when @LastVAMAWeightedClosing > @VAMAWeightedClosing then @VAMAWeightedClosing else @LastVAMAWeightedClosing end end
	--The Highest Weighted Moving Average of Closing
	select @HighestWMAClosing = case when @LastHighestWMAClosing < @WMAClosing then @WMAClosing else @LastWMAClosing end
	--The Lowest Weighted Moving Average of Closing
	select @LowestWMAClosing = case when @Seq = 1 then @WMAClosing else case when @LastWMAClosing > @WMAClosing then @WMAClosing else @LastWMAClosing end end
	--The Highest Weighted Moving Average of Weighted Closing
	select @HighestWMAWeightedClosing = case when @LastHighestWMAWeightedClosing < @WMAWeightedClosing then @WMAWeightedClosing else @LastWMAWeightedClosing end
	--The Lowest Weighted Moving Average of Weighted Closing
	select @LowestWMAWeightedClosing = case when @Seq = 1 then @WMAWeightedClosing else case when @LastWMAWeightedClosing > @WMAWeightedClosing then @WMAWeightedClosing else @LastWMAWeightedClosing end end
	--The Highest Weighted Moving Average of Typical Closing
	select @HighestWMATypicalClosing = case when @LastHighestWMATypicalClosing < @WMATypicalClosing then @WMATypicalClosing else @LastWMATypicalClosing end
	--The Lowest Weighted Moving Average of Typical Closing
	select @LowestWMATypicalClosing = case when @Seq = 1 then @WMATypicalClosing else case when @LastWMATypicalClosing > @WMATypicalClosing then @WMATypicalClosing else @LastWMATypicalClosing end end
	--The Highest Exponential Moving Average of Closing
	select @HighestEMAClosing = case when @LastHighestEMAClosing < @EMAClosing then @EMAClosing else @LastEMAClosing end
	--The Lowest Exponential Moving Average of Closing
	select @LowestEMAClosing = case when @Seq = 1 then @EMAClosing else case when @LastEMAClosing > @EMAClosing then @EMAClosing else @LastEMAClosing end end
	--The Highest Exponential Moving Average of Weighted Closing
	select @HighestEMAWeightedClosing = case when @LastHighestEMAWeightedClosing < @EMAWeightedClosing then @EMAWeightedClosing else @LastEMAWeightedClosing end
	--The Lowest Exponential Moving Average of Weighted Closing
	select @LowestEMAWeightedClosing = case when @Seq = 1 then @EMAWeightedClosing else case when @LastEMAWeightedClosing > @EMAWeightedClosing then @EMAWeightedClosing else @LastEMAWeightedClosing end end
	--The Highest Exponential Moving Average of Typical Closing
	select @HighestEMATypicalClosing = case when @LastHighestEMATypicalClosing < @EMATypicalClosing then @EMATypicalClosing else @LastEMATypicalClosing end
	--The Lowest Exponential Moving Average of Typical Closing
	select @LowestEMATypicalClosing = case when @Seq = 1 then @EMATypicalClosing else case when @LastEMATypicalClosing > @EMATypicalClosing then @EMATypicalClosing else @LastEMATypicalClosing end end
	--The Highest Exponential Moving Average of Volume
	select @HighestEMAVolume = case when @LastHighestEMAVolume < @EMAVolume then @EMAVolume else @LastEMAVolume end
	--The Lowest Exponential Moving Average of Volume
	select @LowestEMAVolume = case when @Seq = 1 then @EMAVolume else case when @LastEMAVolume > @EMAVolume then @EMAVolume else @LastEMAVolume end end
	--The Highest Chande Momentum Oscillator of Closing
	select @HighestCMOClosing = case when @LastHighestCMOClosing < @CMOClosing then @CMOClosing else @LastCMOClosing end
	--The Lowest Chande Momentum Oscillator of Closing
	select @LowestCMOClosing = case when @Seq = 1 then @CMOClosing else case when @LastCMOClosing > @CMOClosing then @CMOClosing else @LastCMOClosing end end
	--The Highest Chande Momentum Oscillator of Volume
	select @HighestCMOVolume = case when @LastHighestCMOVolume < @CMOVolume then @CMOVolume else @LastCMOVolume end
	--The Lowest Chande Momentum Oscillator of Volume
	select @LowestCMOVolume = case when @Seq = 1 then @CMOVolume else case when @LastCMOVolume > @CMOVolume then @CMOVolume else @LastCMOVolume end end
	--The Highest Chande Momentum Oscillator of Weighted Closing
	select @HighestCMOWeightedClosing = case when @LastHighestCMOWeightedClosing < @CMOWeightedClosing then @CMOWeightedClosing else @LastCMOWeightedClosing end
	--The Lowest Chande Momentum Oscillator of Weighted Closing
	select @LowestCMOWeightedClosing = case when @Seq = 1 then @CMOWeightedClosing else case when @LastCMOWeightedClosing > @CMOWeightedClosing then @CMOWeightedClosing else @LastCMOWeightedClosing end end
	--The Highest Chande Momentum Oscillator of Typical Closing
	select @HighestCMOTypicalClosing = case when @LastHighestCMOTypicalClosing < @CMOTypicalClosing then @CMOTypicalClosing else @LastCMOTypicalClosing end
	--The Lowest Chande Momentum Oscillator of Typical Closing
	select @LowestCMOTypicalClosing = case when @Seq = 1 then @CMOTypicalClosing else case when @LastCMOTypicalClosing > @CMOTypicalClosing then @CMOTypicalClosing else @LastCMOTypicalClosing end end
	--The Highest Variable Moving Average of Closing
	select @HighestVMAClosing = case when @LastHighestVMAClosing < @VMAClosing then @VMAClosing else @LastVMAClosing end
	--The Lowest Variable Moving Average of Closing
	select @LowestVMAClosing = case when @Seq = 1 then @VMAClosing else case when @LastVMAClosing > @VMAClosing then @VMAClosing else @LastVMAClosing end end
	--The Highest Variable Moving Average of Volume
	select @HighestVMAVolume = case when @LastHighestVMAVolume < @VMAVolume then @VMAVolume else @LastVMAVolume end
	--The Lowest Variable Moving Average of Volume
	select @LowestVMAVolume = case when @Seq = 1 then @VMAVolume else case when @LastVMAVolume > @VMAVolume then @VMAVolume else @LastVMAVolume end end
	--The Highest Variable Moving Average of Weighted Closing
	select @HighestVMAWeightedClosing = case when @LastHighestVMAWeightedClosing < @VMAWeightedClosing then @VMAWeightedClosing else @LastVMAWeightedClosing end
	--The Lowest Variable Moving Average of Weighted Closing
	select @LowestVMAWeightedClosing = case when @Seq = 1 then @VMAWeightedClosing else case when @LastVMAWeightedClosing > @VMAWeightedClosing then @VMAWeightedClosing else @LastVMAWeightedClosing end end
	--The Highest Variable Moving Average of Typical Closing
	select @HighestVMATypicalClosing = case when @LastHighestVMATypicalClosing < @VMATypicalClosing then @VMATypicalClosing else @LastVMATypicalClosing end
	--The Lowest Variable Moving Average of Typical Closing
	select @LowestVMATypicalClosing = case when @Seq = 1 then @VMATypicalClosing else case when @LastVMATypicalClosing > @VMATypicalClosing then @VMATypicalClosing else @LastVMATypicalClosing end end
	--The Highest Wilder's Smooth  Closing
	select @HighestWSClosing = case when @LastHighestWSClosing < @WSClosing then @WSClosing else @LastWSClosing end
	--The Lowest Wilder's Smooth  Closing
	select @LowestWSClosing = case when @Seq = 1 then @WSClosing else case when @LastWSClosing > @WSClosing then @WSClosing else @LastWSClosing end end
	--The Highest Wilder's Smooth  Weighted Closing
	select @HighestWSWeightedClosing = case when @LastHighestWSWeightedClosing < @WSWeightedClosing then @WSWeightedClosing else @LastWSWeightedClosing end
	--The Lowest Wilder's Smooth  Weighted Closing
	select @LowestWSWeightedClosing = case when @Seq = 1 then @WSWeightedClosing else case when @LastWSWeightedClosing > @WSWeightedClosing then @WSWeightedClosing else @LastWSWeightedClosing end end
	--The Highest Wilder's Smooth  Typical Closing
	select @HighestWSTypicalClosing = case when @LastHighestWSTypicalClosing < @WSTypicalClosing then @WSTypicalClosing else @LastWSTypicalClosing end
	--The Lowest Wilder's Smooth  Typical Closing
	select @LowestWSTypicalClosing = case when @Seq = 1 then @WSTypicalClosing else case when @LastWSTypicalClosing > @WSTypicalClosing then @WSTypicalClosing else @LastWSTypicalClosing end end
	--The Highest Wilder's Smooth  Weighted Closing -- used for WMA
	select @HighestWSWeightedNormalClosing = case when @LastHighestWSWeightedNormalClosing < @WSWeightedNormalClosing then @WSWeightedNormalClosing else @LastWSWeightedNormalClosing end
	--The Lowest Wilder's Smooth  Weighted Closing -- used for WMA
	select @LowestWSWeightedNormalClosing = case when @Seq = 1 then @WSWeightedNormalClosing else case when @LastWSWeightedNormalClosing > @WSWeightedNormalClosing then @WSWeightedNormalClosing else @LastWSWeightedNormalClosing end end
	--The Highest Wilder's Smooth  Weighted Weighted Closing -- used for WMA
	select @HighestWSWeightedWeightedClosing = case when @LastHighestWSWeightedWeightedClosing < @WSWeightedWeightedClosing then @WSWeightedWeightedClosing else @LastWSWeightedWeightedClosing end
	--The Lowest Wilder's Smooth  Weighted Weighted Closing -- used for WMA
	select @LowestWSWeightedWeightedClosing = case when @Seq = 1 then @WSWeightedWeightedClosing else case when @LastWSWeightedWeightedClosing > @WSWeightedWeightedClosing then @WSWeightedWeightedClosing else @LastWSWeightedWeightedClosing end end
	--The Highest Wilder's Smooth  Weighted Typical Closing -- used for WMA
	select @HighestWSWeightedTypicalClosing = case when @LastHighestWSWeightedTypicalClosing < @WSWeightedTypicalClosing then @WSWeightedTypicalClosing else @LastWSWeightedTypicalClosing end
	--The Lowest Wilder's Smooth  Weighted Typical Closing -- used for WMA
	select @LowestWSWeightedTypicalClosing = case when @Seq = 1 then @WSWeightedTypicalClosing else case when @LastWSWeightedTypicalClosing > @WSWeightedTypicalClosing then @WSWeightedTypicalClosing else @LastWSWeightedTypicalClosing end end
	--The Highest Wilder's Smooth  Volume
	select @HighestWSVolume = case when @LastHighestWSVolume < @WSVolume then @WSVolume else @LastWSVolume end
	--The Lowest Wilder's Smooth  Volume
	select @LowestWSVolume = case when @Seq = 1 then @WSVolume else case when @LastWSVolume > @WSVolume then @WSVolume else @LastWSVolume end end
	--The Highest Wilder's Smooth  Volume Adjusted Closing -- use for VAMA
	select @HighestWSVolumeAdjustedClosing = case when @LastHighestWSVolumeAdjustedClosing < @WSVolumeAdjustedClosing then @WSVolumeAdjustedClosing else @LastWSVolumeAdjustedClosing end
	--The Lowest Wilder's Smooth  Volume Adjusted Closing -- use for VAMA
	select @LowestWSVolumeAdjustedClosing = case when @Seq = 1 then @WSVolumeAdjustedClosing else case when @LastWSVolumeAdjustedClosing > @WSVolumeAdjustedClosing then @WSVolumeAdjustedClosing else @LastWSVolumeAdjustedClosing end end
	--The Highest Wilder's Smooth  Volume Adjusted Typical Closing -- use for VAMA
	select @HighestWSVolumeAdjustedTypicalClosing = case when @LastHighestWSVolumeAdjustedTypicalClosing < @WSVolumeAdjustedTypicalClosing then @WSVolumeAdjustedTypicalClosing else @LastWSVolumeAdjustedTypicalClosing end
	--The Lowest Wilder's Smooth  Volume Adjusted Typical Closing -- use for VAMA
	select @LowestWSVolumeAdjustedTypicalClosing = case when @Seq = 1 then @WSVolumeAdjustedTypicalClosing else case when @LastWSVolumeAdjustedTypicalClosing > @WSVolumeAdjustedTypicalClosing then @WSVolumeAdjustedTypicalClosing else @LastWSVolumeAdjustedTypicalClosing end end
	--The Highest Wilder's Smooth  Volume Adjusted Weighted Closing -- use for VAMA
	select @HighestWSVolumeAdjustedWeightedClosing = case when @LastHighestWSVolumeAdjustedWeightedClosing < @WSVolumeAdjustedWeightedClosing then @WSVolumeAdjustedWeightedClosing else @LastWSVolumeAdjustedWeightedClosing end
	--The Lowest Wilder's Smooth  Volume Adjusted Weighted Closing -- use for VAMA
	select @LowestWSVolumeAdjustedWeightedClosing = case when @Seq = 1 then @WSVolumeAdjustedWeightedClosing else case when @LastWSVolumeAdjustedWeightedClosing > @WSVolumeAdjustedWeightedClosing then @WSVolumeAdjustedWeightedClosing else @LastWSVolumeAdjustedWeightedClosing end end
	--The Highest RSI - Simple Moving Average of Closing
	select @HighestRSI_SMAClosing = case when @LastHighestRSI_SMAClosing < @RSI_SMAClosing then @RSI_SMAClosing else @LastRSI_SMAClosing end
	--The Lowest RSI - Simple Moving Average of Closing
	select @LowestRSI_SMAClosing = case when @Seq = 1 then @RSI_SMAClosing else case when @LastRSI_SMAClosing > @RSI_SMAClosing then @RSI_SMAClosing else @LastRSI_SMAClosing end end
	--The Highest RSI - Simple Moving Average of Volume
	select @HighestRSI_SMAVolume = case when @LastHighestRSI_SMAVolume < @RSI_SMAVolume then @RSI_SMAVolume else @LastRSI_SMAVolume end
	--The Lowest RSI - Simple Moving Average of Volume
	select @LowestRSI_SMAVolume = case when @Seq = 1 then @RSI_SMAVolume else case when @LastRSI_SMAVolume > @RSI_SMAVolume then @RSI_SMAVolume else @LastRSI_SMAVolume end end
	--The Highest RSI - Simple Moving Average of Weighted Closing
	select @HighestRSI_SMAWeightedClosing = case when @LastHighestRSI_SMAWeightedClosing < @RSI_SMAWeightedClosing then @RSI_SMAWeightedClosing else @LastRSI_SMAWeightedClosing end
	--The Lowest RSI - Simple Moving Average of Weighted Closing
	select @LowestRSI_SMAWeightedClosing = case when @Seq = 1 then @RSI_SMAWeightedClosing else case when @LastRSI_SMAWeightedClosing > @RSI_SMAWeightedClosing then @RSI_SMAWeightedClosing else @LastRSI_SMAWeightedClosing end end
	--The Highest RSI - Simple Moving Average of Typical Closing
	select @HighestRSI_SMATypicalClosing = case when @LastHighestRSI_SMATypicalClosing < @RSI_SMATypicalClosing then @RSI_SMATypicalClosing else @LastRSI_SMATypicalClosing end
	--The Lowest RSI - Simple Moving Average of Typical Closing
	select @LowestRSI_SMATypicalClosing = case when @Seq = 1 then @RSI_SMATypicalClosing else case when @LastRSI_SMATypicalClosing > @RSI_SMATypicalClosing then @RSI_SMATypicalClosing else @LastRSI_SMATypicalClosing end end
	--The Highest RSI - Simple Moving Average of Volume Adjusted Closing
	select @HighestRSI_SMAVolumeAdjustedClosing = case when @LastHighestRSI_SMAVolumeAdjustedClosing < @RSI_SMAVolumeAdjustedClosing then @RSI_SMAVolumeAdjustedClosing else @LastRSI_SMAVolumeAdjustedClosing end
	--The Lowest RSI - Simple Moving Average of Volume Adjusted Closing
	select @LowestRSI_SMAVolumeAdjustedClosing = case when @Seq = 1 then @RSI_SMAVolumeAdjustedClosing else case when @LastRSI_SMAVolumeAdjustedClosing > @RSI_SMAVolumeAdjustedClosing then @RSI_SMAVolumeAdjustedClosing else @LastRSI_SMAVolumeAdjustedClosing end end
	--The Highest RSI - Simple Moving Average of Volume Adjusted Weighted Closing
	select @HighestRSI_SMAVolumeAdjustedWeightedClosing = case when @LastHighestRSI_SMAVolumeAdjustedWeightedClosing < @RSI_SMAVolumeAdjustedWeightedClosing then @RSI_SMAVolumeAdjustedWeightedClosing else @LastRSI_SMAVolumeAdjustedWeightedClosing end
	--The Lowest RSI - Simple Moving Average of Volume Adjusted Weighted Closing
	select @LowestRSI_SMAVolumeAdjustedWeightedClosing = case when @Seq = 1 then @RSI_SMAVolumeAdjustedWeightedClosing else case when @LastRSI_SMAVolumeAdjustedWeightedClosing > @RSI_SMAVolumeAdjustedWeightedClosing then @RSI_SMAVolumeAdjustedWeightedClosing else @LastRSI_SMAVolumeAdjustedWeightedClosing end end
	--The Highest RSI - Simple Moving Average of Volume Adjusted Typical Closing
	select @HighestRSI_SMAVolumeAdjustedTypicalClosing = case when @LastHighestRSI_SMAVolumeAdjustedTypicalClosing < @RSI_SMAVolumeAdjustedTypicalClosing then @RSI_SMAVolumeAdjustedTypicalClosing else @LastRSI_SMAVolumeAdjustedTypicalClosing end
	--The Lowest RSI - Simple Moving Average of Volume Adjusted Typical Closing
	select @LowestRSI_SMAVolumeAdjustedTypicalClosing = case when @Seq = 1 then @RSI_SMAVolumeAdjustedTypicalClosing else case when @LastRSI_SMAVolumeAdjustedTypicalClosing > @RSI_SMAVolumeAdjustedTypicalClosing then @RSI_SMAVolumeAdjustedTypicalClosing else @LastRSI_SMAVolumeAdjustedTypicalClosing end end
	--The Highest RSI - Exponential Moving Average of Closing
	select @HighestRSI_EMAClosing = case when @LastHighestRSI_EMAClosing < @RSI_EMAClosing then @RSI_EMAClosing else @LastRSI_EMAClosing end
	--The Lowest RSI - Exponential Moving Average of Closing
	select @LowestRSI_EMAClosing = case when @Seq = 1 then @RSI_EMAClosing else case when @LastRSI_EMAClosing > @RSI_EMAClosing then @RSI_EMAClosing else @LastRSI_EMAClosing end end
	--The Highest RSI - Exponential Moving Average of Volume
	select @HighestRSI_EMAVolume = case when @LastHighestRSI_EMAVolume < @RSI_EMAVolume then @RSI_EMAVolume else @LastRSI_EMAVolume end
	--The Lowest RSI - Exponential Moving Average of Volume
	select @LowestRSI_EMAVolume = case when @Seq = 1 then @RSI_EMAVolume else case when @LastRSI_EMAVolume > @RSI_EMAVolume then @RSI_EMAVolume else @LastRSI_EMAVolume end end
	--The Highest RSI - Exponential Moving Average of Weighted Closing
	select @HighestRSI_EMAWeightedClosing = case when @LastHighestRSI_EMAWeightedClosing < @RSI_EMAWeightedClosing then @RSI_EMAWeightedClosing else @LastRSI_EMAWeightedClosing end
	--The Lowest RSI - Exponential Moving Average of Weighted Closing
	select @LowestRSI_EMAWeightedClosing = case when @Seq = 1 then @RSI_EMAWeightedClosing else case when @LastRSI_EMAWeightedClosing > @RSI_EMAWeightedClosing then @RSI_EMAWeightedClosing else @LastRSI_EMAWeightedClosing end end
	--The Highest RSI - Exponential Moving Average of Typical Closing
	select @HighestRSI_EMATypicalClosing = case when @LastHighestRSI_EMATypicalClosing < @RSI_EMATypicalClosing then @RSI_EMATypicalClosing else @LastRSI_EMATypicalClosing end
	--The Lowest RSI - Exponential Moving Average of Typical Closing
	select @LowestRSI_EMATypicalClosing = case when @Seq = 1 then @RSI_EMATypicalClosing else case when @LastRSI_EMATypicalClosing > @RSI_EMATypicalClosing then @RSI_EMATypicalClosing else @LastRSI_EMATypicalClosing end end
	--The Highest RSI - Exponential Moving Average of Volume Adjusted Closing
	select @HighestRSI_EMAVolumeAdjustedClosing = case when @LastHighestRSI_EMAVolumeAdjustedClosing < @RSI_EMAVolumeAdjustedClosing then @RSI_EMAVolumeAdjustedClosing else @LastRSI_EMAVolumeAdjustedClosing end
	--The Lowest RSI - Exponential Moving Average of Volume Adjusted Closing
	select @LowestRSI_EMAVolumeAdjustedClosing = case when @Seq = 1 then @RSI_EMAVolumeAdjustedClosing else case when @LastRSI_EMAVolumeAdjustedClosing > @RSI_EMAVolumeAdjustedClosing then @RSI_EMAVolumeAdjustedClosing else @LastRSI_EMAVolumeAdjustedClosing end end
	--The Highest RSI - Exponential Moving Average of Volume Adjusted Weighted Closing
	select @HighestRSI_EMAVolumeAdjustedWeightedClosing = case when @LastHighestRSI_EMAVolumeAdjustedWeightedClosing < @RSI_EMAVolumeAdjustedWeightedClosing then @RSI_EMAVolumeAdjustedWeightedClosing else @LastRSI_EMAVolumeAdjustedWeightedClosing end
	--The Lowest RSI - Exponential Moving Average of Volume Adjusted Weighted Closing
	select @LowestRSI_EMAVolumeAdjustedWeightedClosing = case when @Seq = 1 then @RSI_EMAVolumeAdjustedWeightedClosing else case when @LastRSI_EMAVolumeAdjustedWeightedClosing > @RSI_EMAVolumeAdjustedWeightedClosing then @RSI_EMAVolumeAdjustedWeightedClosing else @LastRSI_EMAVolumeAdjustedWeightedClosing end end
	--The Highest RSI - Exponential Moving Average of Volume Adjusted Typical Closing
	select @HighestRSI_EMAVolumeAdjustedTypicalClosing = case when @LastHighestRSI_EMAVolumeAdjustedTypicalClosing < @RSI_EMAVolumeAdjustedTypicalClosing then @RSI_EMAVolumeAdjustedTypicalClosing else @LastRSI_EMAVolumeAdjustedTypicalClosing end
	--The Lowest RSI - Exponential Moving Average of Volume Adjusted Typical Closing
	select @LowestRSI_EMAVolumeAdjustedTypicalClosing = case when @Seq = 1 then @RSI_EMAVolumeAdjustedTypicalClosing else case when @LastRSI_EMAVolumeAdjustedTypicalClosing > @RSI_EMAVolumeAdjustedTypicalClosing then @RSI_EMAVolumeAdjustedTypicalClosing else @LastRSI_EMAVolumeAdjustedTypicalClosing end end
	--The Highest RSI - Wilder's Smooth of Closing
	select @HighestRSI_WSClosing = case when @LastHighestRSI_WSClosing < @RSI_WSClosing then @RSI_WSClosing else @LastRSI_WSClosing end
	--The Lowest RSI - Wilder's Smooth of Closing
	select @LowestRSI_WSClosing = case when @Seq = 1 then @RSI_WSClosing else case when @LastRSI_WSClosing > @RSI_WSClosing then @RSI_WSClosing else @LastRSI_WSClosing end end
	--The Highest RSI - Wilder's Smooth of Volume
	select @HighestRSI_WSVolume = case when @LastHighestRSI_WSVolume < @RSI_WSVolume then @RSI_WSVolume else @LastRSI_WSVolume end
	--The Lowest RSI - Wilder's Smooth of Volume
	select @LowestRSI_WSVolume = case when @Seq = 1 then @RSI_WSVolume else case when @LastRSI_WSVolume > @RSI_WSVolume then @RSI_WSVolume else @LastRSI_WSVolume end end
	--The Highest RSI - Wilder's Smooth of Weighted Closing
	select @HighestRSI_WSWeightedClosing = case when @LastHighestRSI_WSWeightedClosing < @RSI_WSWeightedClosing then @RSI_WSWeightedClosing else @LastRSI_WSWeightedClosing end
	--The Lowest RSI - Wilder's Smooth of Weighted Closing
	select @LowestRSI_WSWeightedClosing = case when @Seq = 1 then @RSI_WSWeightedClosing else case when @LastRSI_WSWeightedClosing > @RSI_WSWeightedClosing then @RSI_WSWeightedClosing else @LastRSI_WSWeightedClosing end end
	--The Highest RSI - Wilder's Smooth of Typical Closing
	select @HighestRSI_WSTypicalClosing = case when @LastHighestRSI_WSTypicalClosing < @RSI_WSTypicalClosing then @RSI_WSTypicalClosing else @LastRSI_WSTypicalClosing end
	--The Lowest RSI - Wilder's Smooth of Typical Closing
	select @LowestRSI_WSTypicalClosing = case when @Seq = 1 then @RSI_WSTypicalClosing else case when @LastRSI_WSTypicalClosing > @RSI_WSTypicalClosing then @RSI_WSTypicalClosing else @LastRSI_WSTypicalClosing end end
	--The Highest RSI - Wilder's Smooth of Volume Adjusted Closing
	select @HighestRSI_WSVolumeAdjustedClosing = case when @LastHighestRSI_WSVolumeAdjustedClosing < @RSI_WSVolumeAdjustedClosing then @RSI_WSVolumeAdjustedClosing else @LastRSI_WSVolumeAdjustedClosing end
	--The Lowest RSI - Wilder's Smooth of Volume Adjusted Closing
	select @LowestRSI_WSVolumeAdjustedClosing = case when @Seq = 1 then @RSI_WSVolumeAdjustedClosing else case when @LastRSI_WSVolumeAdjustedClosing > @RSI_WSVolumeAdjustedClosing then @RSI_WSVolumeAdjustedClosing else @LastRSI_WSVolumeAdjustedClosing end end
	--The Highest RSI - Wilder's Smooth of Volume Adjusted Weighted Closing
	select @HighestRSI_WSVolumeAdjustedWeightedClosing = case when @LastHighestRSI_WSVolumeAdjustedWeightedClosing < @RSI_WSVolumeAdjustedWeightedClosing then @RSI_WSVolumeAdjustedWeightedClosing else @LastRSI_WSVolumeAdjustedWeightedClosing end
	--The Lowest RSI - Wilder's Smooth of Volume Adjusted Weighted Closing
	select @LowestRSI_WSVolumeAdjustedWeightedClosing = case when @Seq = 1 then @RSI_WSVolumeAdjustedWeightedClosing else case when @LastRSI_WSVolumeAdjustedWeightedClosing > @RSI_WSVolumeAdjustedWeightedClosing then @RSI_WSVolumeAdjustedWeightedClosing else @LastRSI_WSVolumeAdjustedWeightedClosing end end
	--The Highest RSI - Wilder's Smooth of Volume Adjusted Typical Closing
	select @HighestRSI_WSVolumeAdjustedTypicalClosing = case when @LastHighestRSI_WSVolumeAdjustedTypicalClosing < @RSI_WSVolumeAdjustedTypicalClosing then @RSI_WSVolumeAdjustedTypicalClosing else @LastRSI_WSVolumeAdjustedTypicalClosing end
	--The Lowest RSI - Wilder's Smooth of Volume Adjusted Typical Closing
	select @LowestRSI_WSVolumeAdjustedTypicalClosing = case when @Seq = 1 then @RSI_WSVolumeAdjustedTypicalClosing else case when @LastRSI_WSVolumeAdjustedTypicalClosing > @RSI_WSVolumeAdjustedTypicalClosing then @RSI_WSVolumeAdjustedTypicalClosing else @LastRSI_WSVolumeAdjustedTypicalClosing end end
	--The Highest Standard Deviation of Closing
	select @HighestSTDEVClosing = case when @LastHighestSTDEVClosing < @STDEVClosing then @STDEVClosing else @LastSTDEVClosing end
	--The Lowest Standard Deviation of Closing
	select @LowestSTDEVClosing = case when @Seq = 1 then @STDEVClosing else case when @LastSTDEVClosing > @STDEVClosing then @STDEVClosing else @LastSTDEVClosing end end
	--The Highest Standard Deviation of Weighted Closing
	select @HighestSTDEVWeightedClosing = case when @LastHighestSTDEVWeightedClosing < @STDEVWeightedClosing then @STDEVWeightedClosing else @LastSTDEVWeightedClosing end
	--The Lowest Standard Deviation of Weighted Closing
	select @LowestSTDEVWeightedClosing = case when @Seq = 1 then @STDEVWeightedClosing else case when @LastSTDEVWeightedClosing > @STDEVWeightedClosing then @STDEVWeightedClosing else @LastSTDEVWeightedClosing end end
	--The Highest Standard Deviation of Typical Closing
	select @HighestSTDEVTypicalClosing = case when @LastHighestSTDEVTypicalClosing < @STDEVTypicalClosing then @STDEVTypicalClosing else @LastSTDEVTypicalClosing end
	--The Lowest Standard Deviation of Typical Closing
	select @LowestSTDEVTypicalClosing = case when @Seq = 1 then @STDEVTypicalClosing else case when @LastSTDEVTypicalClosing > @STDEVTypicalClosing then @STDEVTypicalClosing else @LastSTDEVTypicalClosing end end
	--The Highest Standard Deviation of Volume
	select @HighestSTDEVVolume = case when @LastHighestSTDEVVolume < @STDEVVolume then @STDEVVolume else @LastSTDEVVolume end
	--The Lowest Standard Deviation of Volume
	select @LowestSTDEVVolume = case when @Seq = 1 then @STDEVVolume else case when @LastSTDEVVolume > @STDEVVolume then @STDEVVolume else @LastSTDEVVolume end end
	--The Highest Standard Deviation of Closing
	select @HighestRVIClosing = case when @LastHighestRVIClosing < @RVIClosing then @RVIClosing else @LastRVIClosing end
	--The Lowest Standard Deviation of Closing
	select @LowestRVIClosing = case when @Seq = 1 then @RVIClosing else case when @LastRVIClosing > @RVIClosing then @RVIClosing else @LastRVIClosing end end
	--The Highest Standard Deviation of Volume
	select @HighestRVIVolume = case when @LastHighestRVIVolume < @RVIVolume then @RVIVolume else @LastRVIVolume end
	--The Lowest Standard Deviation of Volume
	select @LowestRVIVolume = case when @Seq = 1 then @RVIVolume else case when @LastRVIVolume > @RVIVolume then @RVIVolume else @LastRVIVolume end end
	--The Highest Standard Deviation of Weighted Closing
	select @HighestRVIWeightedClosing = case when @LastHighestRVIWeightedClosing < @RVIWeightedClosing then @RVIWeightedClosing else @LastRVIWeightedClosing end
	--The Lowest Standard Deviation of Weighted Closing
	select @LowestRVIWeightedClosing = case when @Seq = 1 then @RVIWeightedClosing else case when @LastRVIWeightedClosing > @RVIWeightedClosing then @RVIWeightedClosing else @LastRVIWeightedClosing end end
	--The Highest Standard Deviation of Typical Closing
	select @HighestRVITypicalClosing = case when @LastHighestRVITypicalClosing < @RVITypicalClosing then @RVITypicalClosing else @LastRVITypicalClosing end
	--The Lowest Standard Deviation of Typical Closing
	select @LowestRVITypicalClosing = case when @Seq = 1 then @RVITypicalClosing else case when @LastRVITypicalClosing > @RVITypicalClosing then @RVITypicalClosing else @LastRVITypicalClosing end end
	--The Highest Standard Deviation of Volume Adjusted Closing
	select @HighestRVIVolumeAdjustedClosing = case when @LastHighestRVIVolumeAdjustedClosing < @RVIVolumeAdjustedClosing then @RVIVolumeAdjustedClosing else @LastRVIVolumeAdjustedClosing end
	--The Lowest Standard Deviation of Volume Adjusted Closing
	select @LowestRVIVolumeAdjustedClosing = case when @Seq = 1 then @RVIVolumeAdjustedClosing else case when @LastRVIVolumeAdjustedClosing > @RVIVolumeAdjustedClosing then @RVIVolumeAdjustedClosing else @LastRVIVolumeAdjustedClosing end end
	--The Highest Standard Deviation of Volume Adjusted Weighted Closing
	select @HighestRVIVolumeAdjustedWeightedClosing = case when @LastHighestRVIVolumeAdjustedWeightedClosing < @RVIVolumeAdjustedWeightedClosing then @RVIVolumeAdjustedWeightedClosing else @LastRVIVolumeAdjustedWeightedClosing end
	--The Lowest Standard Deviation of Volume Adjusted Weighted Closing
	select @LowestRVIVolumeAdjustedWeightedClosing = case when @Seq = 1 then @RVIVolumeAdjustedWeightedClosing else case when @LastRVIVolumeAdjustedWeightedClosing > @RVIVolumeAdjustedWeightedClosing then @RVIVolumeAdjustedWeightedClosing else @LastRVIVolumeAdjustedWeightedClosing end end
	--The Highest Standard Deviation of Volume Adjusted Typical Closing
	select @HighestRVIVolumeAdjustedTypicalClosing = case when @LastHighestRVIVolumeAdjustedTypicalClosing < @RVIVolumeAdjustedTypicalClosing then @RVIVolumeAdjustedTypicalClosing else @LastRVIVolumeAdjustedTypicalClosing end
	--The Lowest Standard Deviation of Volume Adjusted Typical Closing
	select @LowestRVIVolumeAdjustedTypicalClosing = case when @Seq = 1 then @RVIVolumeAdjustedTypicalClosing else case when @LastRVIVolumeAdjustedTypicalClosing > @RVIVolumeAdjustedTypicalClosing then @RVIVolumeAdjustedTypicalClosing else @LastRVIVolumeAdjustedTypicalClosing end end

	--Stochastic Oscillator The opening price of the period
	select @SO_Opening = case when (@HighestOpening - @LowestOpening) = 0 then 0 else (@A2_Opening - @LowestOpening) * @F_Hundred / (@HighestOpening - @LowestOpening) end
	--Stochastic Oscillator The highest price in the period
	select @SO_High = case when (@HighestHigh - @LowestHigh) = 0 then 0 else (@A2_High - @LowestHigh) * @F_Hundred / (@HighestHigh - @LowestHigh) end
	--Stochastic Oscillator The lowest price in the period
	select @SO_Low = case when (@HighestLow - @LowestLow) = 0 then 0 else (@A2_Low - @LowestLow) * @F_Hundred / (@HighestLow - @LowestLow) end
	--Stochastic Oscillator The last price of the period
	select @SO_Closing = case when (@HighestHigh - @LowestLow) = 0 then 0 else (@A2_Closing - @LowestLow) * @F_Hundred / (@HighestHigh - @LowestLow) end
	--Stochastic Oscillator Total volume inthe period
	select @SO_Volume = case when (@HighestVolume - @LowestVolume) = 0 then 0 else (@A2_Volume - @LowestVolume) * @F_Hundred / (@HighestVolume - @LowestVolume) end
	--Stochastic Oscillator Simple Moving Average of Closing
	select @SO_SMAClosing = case when (@HighestSMAClosing - @LowestSMAClosing) = 0 then 0 else (@SMAClosing - @LowestSMAClosing) * @F_Hundred / (@HighestSMAClosing - @LowestSMAClosing) end
	--Stochastic Oscillator Simple Moving Average of Weighted Closing
	select @SO_SMAWeightedClosing = case when (@HighestSMAWeightedClosing - @LowestSMAWeightedClosing) = 0 then 0 else (@SMAWeightedClosing - @LowestSMAWeightedClosing) * @F_Hundred / (@HighestSMAWeightedClosing - @LowestSMAWeightedClosing) end
	--Stochastic Oscillator Simple Moving Average of Typical Closing
	select @SO_SMATypicalClosing = case when (@HighestSMATypicalClosing - @LowestSMATypicalClosing) = 0 then 0 else (@SMATypicalClosing - @LowestSMATypicalClosing) * @F_Hundred / (@HighestSMATypicalClosing - @LowestSMATypicalClosing) end
	--Stochastic Oscillator Simple Moving Average of Volume
	select @SO_SMAVolume = case when (@HighestSMAVolume - @LowestSMAVolume) = 0 then 0 else (@SMAVolume - @LowestSMAVolume) * @F_Hundred / (@HighestSMAVolume - @LowestSMAVolume) end
	--Stochastic Oscillator Volume Adjusted Moving Average of Closing
	select @SO_VAMAClosing = case when (@HighestVAMAClosing - @LowestVAMAClosing) = 0 then 0 else (@VAMAClosing - @LowestVAMAClosing) * @F_Hundred / (@HighestVAMAClosing - @LowestVAMAClosing) end
	--Stochastic Oscillator Volume Adjusted Moving Average of Typical Closing
	select @SO_VAMATypicalClosing = case when (@HighestVAMATypicalClosing - @LowestVAMATypicalClosing) = 0 then 0 else (@VAMATypicalClosing - @LowestVAMATypicalClosing) * @F_Hundred / (@HighestVAMATypicalClosing - @LowestVAMATypicalClosing) end
	--Stochastic Oscillator Volume Adjusted Moving Average of Weighted Closing
	select @SO_VAMAWeightedClosing = case when (@HighestVAMAWeightedClosing - @LowestVAMAWeightedClosing) = 0 then 0 else (@VAMAWeightedClosing - @LowestVAMAWeightedClosing) * @F_Hundred / (@HighestVAMAWeightedClosing - @LowestVAMAWeightedClosing) end
	--Stochastic Oscillator Weighted Moving Average of Closing
	select @SO_WMAClosing = case when (@HighestWMAClosing - @LowestWMAClosing) = 0 then 0 else (@WMAClosing - @LowestWMAClosing) * @F_Hundred / (@HighestWMAClosing - @LowestWMAClosing) end
	--Stochastic Oscillator Weighted Moving Average of Weighted Closing
	select @SO_WMAWeightedClosing = case when (@HighestWMAWeightedClosing - @LowestWMAWeightedClosing) = 0 then 0 else (@WMAWeightedClosing - @LowestWMAWeightedClosing) * @F_Hundred / (@HighestWMAWeightedClosing - @LowestWMAWeightedClosing) end
	--Stochastic Oscillator Weighted Moving Average of Typical Closing
	select @SO_WMATypicalClosing = case when (@HighestWMATypicalClosing - @LowestWMATypicalClosing) = 0 then 0 else (@WMATypicalClosing - @LowestWMATypicalClosing) * @F_Hundred / (@HighestWMATypicalClosing - @LowestWMATypicalClosing) end
	--Stochastic Oscillator Exponential Moving Average of Closing
	select @SO_EMAClosing = case when (@HighestEMAClosing - @LowestEMAClosing) = 0 then 0 else (@EMAClosing - @LowestEMAClosing) * @F_Hundred / (@HighestEMAClosing - @LowestEMAClosing) end
	--Stochastic Oscillator Exponential Moving Average of Weighted Closing
	select @SO_EMAWeightedClosing = case when (@HighestEMAWeightedClosing - @LowestEMAWeightedClosing) = 0 then 0 else (@EMAWeightedClosing - @LowestEMAWeightedClosing) * @F_Hundred / (@HighestEMAWeightedClosing - @LowestEMAWeightedClosing) end
	--Stochastic Oscillator Exponential Moving Average of Typical Closing
	select @SO_EMATypicalClosing = case when (@HighestEMATypicalClosing - @LowestEMATypicalClosing) = 0 then 0 else (@EMATypicalClosing - @LowestEMATypicalClosing) * @F_Hundred / (@HighestEMATypicalClosing - @LowestEMATypicalClosing) end
	--Stochastic Oscillator Exponential Moving Average of Volume
	select @SO_EMAVolume = case when (@HighestEMAVolume - @LowestEMAVolume) = 0 then 0 else (@EMAVolume - @LowestEMAVolume) * @F_Hundred / (@HighestEMAVolume - @LowestEMAVolume) end
	--Stochastic Oscillator Chande Momentum Oscillator of Closing
	select @SO_CMOClosing = case when (@HighestCMOClosing - @LowestCMOClosing) = 0 then 0 else (@CMOClosing - @LowestCMOClosing) * @F_Hundred / (@HighestCMOClosing - @LowestCMOClosing) end
	--Stochastic Oscillator Chande Momentum Oscillator of Volume
	select @SO_CMOVolume = case when (@HighestCMOVolume - @LowestCMOVolume) = 0 then 0 else (@CMOVolume - @LowestCMOVolume) * @F_Hundred / (@HighestCMOVolume - @LowestCMOVolume) end
	--Stochastic Oscillator Chande Momentum Oscillator of Weighted Closing
	select @SO_CMOWeightedClosing = case when (@HighestCMOWeightedClosing - @LowestCMOWeightedClosing) = 0 then 0 else (@CMOWeightedClosing - @LowestCMOWeightedClosing) * @F_Hundred / (@HighestCMOWeightedClosing - @LowestCMOWeightedClosing) end
	--Stochastic Oscillator Chande Momentum Oscillator of Typical Closing
	select @SO_CMOTypicalClosing = case when (@HighestCMOTypicalClosing - @LowestCMOTypicalClosing) = 0 then 0 else (@CMOTypicalClosing - @LowestCMOTypicalClosing) * @F_Hundred / (@HighestCMOTypicalClosing - @LowestCMOTypicalClosing) end
	--Stochastic Oscillator Variable Moving Average of Closing
	select @SO_VMAClosing = case when (@HighestVMAClosing - @LowestVMAClosing) = 0 then 0 else (@VMAClosing - @LowestVMAClosing) * @F_Hundred / (@HighestVMAClosing - @LowestVMAClosing) end
	--Stochastic Oscillator Variable Moving Average of Volume
	select @SO_VMAVolume = case when (@HighestVMAVolume - @LowestVMAVolume) = 0 then 0 else (@VMAVolume - @LowestVMAVolume) * @F_Hundred / (@HighestVMAVolume - @LowestVMAVolume) end
	--Stochastic Oscillator Variable Moving Average of Weighted Closing
	select @SO_VMAWeightedClosing = case when (@HighestVMAWeightedClosing - @LowestVMAWeightedClosing) = 0 then 0 else (@VMAWeightedClosing - @LowestVMAWeightedClosing) * @F_Hundred / (@HighestVMAWeightedClosing - @LowestVMAWeightedClosing) end
	--Stochastic Oscillator Variable Moving Average of Typical Closing
	select @SO_VMATypicalClosing = case when (@HighestVMATypicalClosing - @LowestVMATypicalClosing) = 0 then 0 else (@VMATypicalClosing - @LowestVMATypicalClosing) * @F_Hundred / (@HighestVMATypicalClosing - @LowestVMATypicalClosing) end
	--Stochastic Oscillator Wilder's Smooth  Closing
	select @SO_WSClosing = case when (@HighestWSClosing - @LowestWSClosing) = 0 then 0 else (@WSClosing - @LowestWSClosing) * @F_Hundred / (@HighestWSClosing - @LowestWSClosing) end
	--Stochastic Oscillator Wilder's Smooth  Weighted Closing
	select @SO_WSWeightedClosing = case when (@HighestWSWeightedClosing - @LowestWSWeightedClosing) = 0 then 0 else (@WSWeightedClosing - @LowestWSWeightedClosing) * @F_Hundred / (@HighestWSWeightedClosing - @LowestWSWeightedClosing) end
	--Stochastic Oscillator Wilder's Smooth  Typical Closing
	select @SO_WSTypicalClosing = case when (@HighestWSTypicalClosing - @LowestWSTypicalClosing) = 0 then 0 else (@WSTypicalClosing - @LowestWSTypicalClosing) * @F_Hundred / (@HighestWSTypicalClosing - @LowestWSTypicalClosing) end
	--Stochastic Oscillator Wilder's Smooth  Weighted Closing -- used for WMA
	select @SO_WSWeightedNormalClosing = case when (@HighestWSWeightedNormalClosing - @LowestWSWeightedNormalClosing) = 0 then 0 else (@WSWeightedNormalClosing - @LowestWSWeightedNormalClosing) * @F_Hundred / (@HighestWSWeightedNormalClosing - @LowestWSWeightedNormalClosing) end
	--Stochastic Oscillator Wilder's Smooth  Weighted Weighted Closing -- used for WMA
	select @SO_WSWeightedWeightedClosing = case when (@HighestWSWeightedWeightedClosing - @LowestWSWeightedWeightedClosing) = 0 then 0 else (@WSWeightedWeightedClosing - @LowestWSWeightedWeightedClosing) * @F_Hundred / (@HighestWSWeightedWeightedClosing - @LowestWSWeightedWeightedClosing) end
	--Stochastic Oscillator Wilder's Smooth  Weighted Typical Closing -- used for WMA
	select @SO_WSWeightedTypicalClosing = case when (@HighestWSWeightedTypicalClosing - @LowestWSWeightedTypicalClosing) = 0 then 0 else (@WSWeightedTypicalClosing - @LowestWSWeightedTypicalClosing) * @F_Hundred / (@HighestWSWeightedTypicalClosing - @LowestWSWeightedTypicalClosing) end
	--Stochastic Oscillator Wilder's Smooth  Volume
	select @SO_WSVolume = case when (@HighestWSVolume - @LowestWSVolume) = 0 then 0 else (@WSVolume - @LowestWSVolume) * @F_Hundred / (@HighestWSVolume - @LowestWSVolume) end
	--Stochastic Oscillator Wilder's Smooth  Volume Adjusted Closing -- use for VAMA
	select @SO_WSVolumeAdjustedClosing = case when (@HighestWSVolumeAdjustedClosing - @LowestWSVolumeAdjustedClosing) = 0 then 0 else (@WSVolumeAdjustedClosing - @LowestWSVolumeAdjustedClosing) * @F_Hundred / (@HighestWSVolumeAdjustedClosing - @LowestWSVolumeAdjustedClosing) end
	--Stochastic Oscillator Wilder's Smooth  Volume Adjusted Typical Closing -- use for VAMA
	select @SO_WSVolumeAdjustedTypicalClosing = case when (@HighestWSVolumeAdjustedTypicalClosing - @LowestWSVolumeAdjustedTypicalClosing) = 0 then 0 else (@WSVolumeAdjustedTypicalClosing - @LowestWSVolumeAdjustedTypicalClosing) * @F_Hundred / (@HighestWSVolumeAdjustedTypicalClosing - @LowestWSVolumeAdjustedTypicalClosing) end
	--Stochastic Oscillator Wilder's Smooth  Volume Adjusted Weighted Closing -- use for VAMA
	select @SO_WSVolumeAdjustedWeightedClosing = case when (@HighestWSVolumeAdjustedWeightedClosing - @LowestWSVolumeAdjustedWeightedClosing) = 0 then 0 else (@WSVolumeAdjustedWeightedClosing - @LowestWSVolumeAdjustedWeightedClosing) * @F_Hundred / (@HighestWSVolumeAdjustedWeightedClosing - @LowestWSVolumeAdjustedWeightedClosing) end
	--Stochastic Oscillator RSI - Simple Moving Average of Closing
	select @SO_RSI_SMAClosing = case when (@HighestRSI_SMAClosing - @LowestRSI_SMAClosing) = 0 then 0 else (@RSI_SMAClosing - @LowestRSI_SMAClosing) * @F_Hundred / (@HighestRSI_SMAClosing - @LowestRSI_SMAClosing) end
	--Stochastic Oscillator RSI - Simple Moving Average of Volume
	select @SO_RSI_SMAVolume = case when (@HighestRSI_SMAVolume - @LowestRSI_SMAVolume) = 0 then 0 else (@RSI_SMAVolume - @LowestRSI_SMAVolume) * @F_Hundred / (@HighestRSI_SMAVolume - @LowestRSI_SMAVolume) end
	--Stochastic Oscillator RSI - Simple Moving Average of Weighted Closing
	select @SO_RSI_SMAWeightedClosing = case when (@HighestRSI_SMAWeightedClosing - @LowestRSI_SMAWeightedClosing) = 0 then 0 else (@RSI_SMAWeightedClosing - @LowestRSI_SMAWeightedClosing) * @F_Hundred / (@HighestRSI_SMAWeightedClosing - @LowestRSI_SMAWeightedClosing) end
	--Stochastic Oscillator RSI - Simple Moving Average of Typical Closing
	select @SO_RSI_SMATypicalClosing = case when (@HighestRSI_SMATypicalClosing - @LowestRSI_SMATypicalClosing) = 0 then 0 else (@RSI_SMATypicalClosing - @LowestRSI_SMATypicalClosing) * @F_Hundred / (@HighestRSI_SMATypicalClosing - @LowestRSI_SMATypicalClosing) end
	--Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Closing
	select @SO_RSI_SMAVolumeAdjustedClosing = case when (@HighestRSI_SMAVolumeAdjustedClosing - @LowestRSI_SMAVolumeAdjustedClosing) = 0 then 0 else (@RSI_SMAVolumeAdjustedClosing - @LowestRSI_SMAVolumeAdjustedClosing) * @F_Hundred / (@HighestRSI_SMAVolumeAdjustedClosing - @LowestRSI_SMAVolumeAdjustedClosing) end
	--Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Weighted Closing
	select @SO_RSI_SMAVolumeAdjustedWeightedClosing = case when (@HighestRSI_SMAVolumeAdjustedWeightedClosing - @LowestRSI_SMAVolumeAdjustedWeightedClosing) = 0 then 0 else (@RSI_SMAVolumeAdjustedWeightedClosing - @LowestRSI_SMAVolumeAdjustedWeightedClosing) * @F_Hundred / (@HighestRSI_SMAVolumeAdjustedWeightedClosing - @LowestRSI_SMAVolumeAdjustedWeightedClosing) end
	--Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Typical Closing
	select @SO_RSI_SMAVolumeAdjustedTypicalClosing = case when (@HighestRSI_SMAVolumeAdjustedTypicalClosing - @LowestRSI_SMAVolumeAdjustedTypicalClosing) = 0 then 0 else (@RSI_SMAVolumeAdjustedTypicalClosing - @LowestRSI_SMAVolumeAdjustedTypicalClosing) * @F_Hundred / (@HighestRSI_SMAVolumeAdjustedTypicalClosing - @LowestRSI_SMAVolumeAdjustedTypicalClosing) end
	--Stochastic Oscillator RSI - Exponential Moving Average of Closing
	select @SO_RSI_EMAClosing = case when (@HighestRSI_EMAClosing - @LowestRSI_EMAClosing) = 0 then 0 else (@RSI_EMAClosing - @LowestRSI_EMAClosing) * @F_Hundred / (@HighestRSI_EMAClosing - @LowestRSI_EMAClosing) end
	--Stochastic Oscillator RSI - Exponential Moving Average of Volume
	select @SO_RSI_EMAVolume = case when (@HighestRSI_EMAVolume - @LowestRSI_EMAVolume) = 0 then 0 else (@RSI_EMAVolume - @LowestRSI_EMAVolume) * @F_Hundred / (@HighestRSI_EMAVolume - @LowestRSI_EMAVolume) end
	--Stochastic Oscillator RSI - Exponential Moving Average of Weighted Closing
	select @SO_RSI_EMAWeightedClosing = case when (@HighestRSI_EMAWeightedClosing - @LowestRSI_EMAWeightedClosing) = 0 then 0 else (@RSI_EMAWeightedClosing - @LowestRSI_EMAWeightedClosing) * @F_Hundred / (@HighestRSI_EMAWeightedClosing - @LowestRSI_EMAWeightedClosing) end
	--Stochastic Oscillator RSI - Exponential Moving Average of Typical Closing
	select @SO_RSI_EMATypicalClosing = case when (@HighestRSI_EMATypicalClosing - @LowestRSI_EMATypicalClosing) = 0 then 0 else (@RSI_EMATypicalClosing - @LowestRSI_EMATypicalClosing) * @F_Hundred / (@HighestRSI_EMATypicalClosing - @LowestRSI_EMATypicalClosing) end
	--Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Closing
	select @SO_RSI_EMAVolumeAdjustedClosing = case when (@HighestRSI_EMAVolumeAdjustedClosing - @LowestRSI_EMAVolumeAdjustedClosing) = 0 then 0 else (@RSI_EMAVolumeAdjustedClosing - @LowestRSI_EMAVolumeAdjustedClosing) * @F_Hundred / (@HighestRSI_EMAVolumeAdjustedClosing - @LowestRSI_EMAVolumeAdjustedClosing) end
	--Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Weighted Closing
	select @SO_RSI_EMAVolumeAdjustedWeightedClosing = case when (@HighestRSI_EMAVolumeAdjustedWeightedClosing - @LowestRSI_EMAVolumeAdjustedWeightedClosing) = 0 then 0 else (@RSI_EMAVolumeAdjustedWeightedClosing - @LowestRSI_EMAVolumeAdjustedWeightedClosing) * @F_Hundred / (@HighestRSI_EMAVolumeAdjustedWeightedClosing - @LowestRSI_EMAVolumeAdjustedWeightedClosing) end
	--Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Typical Closing
	select @SO_RSI_EMAVolumeAdjustedTypicalClosing = case when (@HighestRSI_EMAVolumeAdjustedTypicalClosing - @LowestRSI_EMAVolumeAdjustedTypicalClosing) = 0 then 0 else (@RSI_EMAVolumeAdjustedTypicalClosing - @LowestRSI_EMAVolumeAdjustedTypicalClosing) * @F_Hundred / (@HighestRSI_EMAVolumeAdjustedTypicalClosing - @LowestRSI_EMAVolumeAdjustedTypicalClosing) end
	--Stochastic Oscillator RSI - Wilder's Smooth of Closing
	select @SO_RSI_WSClosing = case when (@HighestRSI_WSClosing - @LowestRSI_WSClosing) = 0 then 0 else (@RSI_WSClosing - @LowestRSI_WSClosing) * @F_Hundred / (@HighestRSI_WSClosing - @LowestRSI_WSClosing) end
	--Stochastic Oscillator RSI - Wilder's Smooth of Volume
	select @SO_RSI_WSVolume = case when (@HighestRSI_WSVolume - @LowestRSI_WSVolume) = 0 then 0 else (@RSI_WSVolume - @LowestRSI_WSVolume) * @F_Hundred / (@HighestRSI_WSVolume - @LowestRSI_WSVolume) end
	--Stochastic Oscillator RSI - Wilder's Smooth of Weighted Closing
	select @SO_RSI_WSWeightedClosing = case when (@HighestRSI_WSWeightedClosing - @LowestRSI_WSWeightedClosing) = 0 then 0 else (@RSI_WSWeightedClosing - @LowestRSI_WSWeightedClosing) * @F_Hundred / (@HighestRSI_WSWeightedClosing - @LowestRSI_WSWeightedClosing) end
	--Stochastic Oscillator RSI - Wilder's Smooth of Typical Closing
	select @SO_RSI_WSTypicalClosing = case when (@HighestRSI_WSTypicalClosing - @LowestRSI_WSTypicalClosing) = 0 then 0 else (@RSI_WSTypicalClosing - @LowestRSI_WSTypicalClosing) * @F_Hundred / (@HighestRSI_WSTypicalClosing - @LowestRSI_WSTypicalClosing) end
	--Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Closing
	select @SO_RSI_WSVolumeAdjustedClosing = case when (@HighestRSI_WSVolumeAdjustedClosing - @LowestRSI_WSVolumeAdjustedClosing) = 0 then 0 else (@RSI_WSVolumeAdjustedClosing - @LowestRSI_WSVolumeAdjustedClosing) * @F_Hundred / (@HighestRSI_WSVolumeAdjustedClosing - @LowestRSI_WSVolumeAdjustedClosing) end
	--Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Weighted Closing
	select @SO_RSI_WSVolumeAdjustedWeightedClosing = case when (@HighestRSI_WSVolumeAdjustedWeightedClosing - @LowestRSI_WSVolumeAdjustedWeightedClosing) = 0 then 0 else (@RSI_WSVolumeAdjustedWeightedClosing - @LowestRSI_WSVolumeAdjustedWeightedClosing) * @F_Hundred / (@HighestRSI_WSVolumeAdjustedWeightedClosing - @LowestRSI_WSVolumeAdjustedWeightedClosing) end
	--Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Typical Closing
	select @SO_RSI_WSVolumeAdjustedTypicalClosing = case when (@HighestRSI_WSVolumeAdjustedTypicalClosing - @LowestRSI_WSVolumeAdjustedTypicalClosing) = 0 then 0 else (@RSI_WSVolumeAdjustedTypicalClosing - @LowestRSI_WSVolumeAdjustedTypicalClosing) * @F_Hundred / (@HighestRSI_WSVolumeAdjustedTypicalClosing - @LowestRSI_WSVolumeAdjustedTypicalClosing) end
	--Stochastic Oscillator Standard Deviation of Closing
	select @SO_STDEVClosing = case when (@HighestSTDEVClosing - @LowestSTDEVClosing) = 0 then 0 else (@STDEVClosing - @LowestSTDEVClosing) * @F_Hundred / (@HighestSTDEVClosing - @LowestSTDEVClosing) end
	--Stochastic Oscillator Standard Deviation of Weighted Closing
	select @SO_STDEVWeightedClosing = case when (@HighestSTDEVWeightedClosing - @LowestSTDEVWeightedClosing) = 0 then 0 else (@STDEVWeightedClosing - @LowestSTDEVWeightedClosing) * @F_Hundred / (@HighestSTDEVWeightedClosing - @LowestSTDEVWeightedClosing) end
	--Stochastic Oscillator Standard Deviation of Typical Closing
	select @SO_STDEVTypicalClosing = case when (@HighestSTDEVTypicalClosing - @LowestSTDEVTypicalClosing) = 0 then 0 else (@STDEVTypicalClosing - @LowestSTDEVTypicalClosing) * @F_Hundred / (@HighestSTDEVTypicalClosing - @LowestSTDEVTypicalClosing) end
	--Stochastic Oscillator Standard Deviation of Volume
	select @SO_STDEVVolume = case when (@HighestSTDEVVolume - @LowestSTDEVVolume) = 0 then 0 else (@STDEVVolume - @LowestSTDEVVolume) * @F_Hundred / (@HighestSTDEVVolume - @LowestSTDEVVolume) end
	--Stochastic Oscillator Standard Deviation of Closing
	select @SO_RVIClosing = case when (@HighestRVIClosing - @LowestRVIClosing) = 0 then 0 else (@RVIClosing - @LowestRVIClosing) * @F_Hundred / (@HighestRVIClosing - @LowestRVIClosing) end
	--Stochastic Oscillator Standard Deviation of Volume
	select @SO_RVIVolume = case when (@HighestRVIVolume - @LowestRVIVolume) = 0 then 0 else (@RVIVolume - @LowestRVIVolume) * @F_Hundred / (@HighestRVIVolume - @LowestRVIVolume) end
	--Stochastic Oscillator Standard Deviation of Weighted Closing
	select @SO_RVIWeightedClosing = case when (@HighestRVIWeightedClosing - @LowestRVIWeightedClosing) = 0 then 0 else (@RVIWeightedClosing - @LowestRVIWeightedClosing) * @F_Hundred / (@HighestRVIWeightedClosing - @LowestRVIWeightedClosing) end
	--Stochastic Oscillator Standard Deviation of Typical Closing
	select @SO_RVITypicalClosing = case when (@HighestRVITypicalClosing - @LowestRVITypicalClosing) = 0 then 0 else (@RVITypicalClosing - @LowestRVITypicalClosing) * @F_Hundred / (@HighestRVITypicalClosing - @LowestRVITypicalClosing) end
	--Stochastic Oscillator Standard Deviation of Volume Adjusted Closing
	select @SO_RVIVolumeAdjustedClosing = case when (@HighestRVIVolumeAdjustedClosing - @LowestRVIVolumeAdjustedClosing) = 0 then 0 else (@RVIVolumeAdjustedClosing - @LowestRVIVolumeAdjustedClosing) * @F_Hundred / (@HighestRVIVolumeAdjustedClosing - @LowestRVIVolumeAdjustedClosing) end
	--Stochastic Oscillator Standard Deviation of Volume Adjusted Weighted Closing
	select @SO_RVIVolumeAdjustedWeightedClosing = case when (@HighestRVIVolumeAdjustedWeightedClosing - @LowestRVIVolumeAdjustedWeightedClosing) = 0 then 0 else (@RVIVolumeAdjustedWeightedClosing - @LowestRVIVolumeAdjustedWeightedClosing) * @F_Hundred / (@HighestRVIVolumeAdjustedWeightedClosing - @LowestRVIVolumeAdjustedWeightedClosing) end
	--Stochastic Oscillator Standard Deviation of Volume Adjusted Typical Closing
	select @SO_RVIVolumeAdjustedTypicalClosing = case when (@HighestRVIVolumeAdjustedTypicalClosing - @LowestRVIVolumeAdjustedTypicalClosing) = 0 then 0 else (@RVIVolumeAdjustedTypicalClosing - @LowestRVIVolumeAdjustedTypicalClosing) * @F_Hundred / (@HighestRVIVolumeAdjustedTypicalClosing - @LowestRVIVolumeAdjustedTypicalClosing) end


	select @alpha = @F_Two / (cast(3 as float) + @F_One)
	select @beta = @F_One - @alpha
	--3-Unit EA of Stochastic Oscillator The opening price of the period
	select @SO3_Opening = case when @Seq = 1 then @SO_Opening else @SO_Opening * @alpha + @beta * @LastSO_Opening end
	--3-Unit EA of Stochastic Oscillator The highest price in the period
	select @SO3_High = case when @Seq = 1 then @SO_High else @SO_High * @alpha + @beta * @LastSO_High end
	--3-Unit EA of Stochastic Oscillator The lowest price in the period
	select @SO3_Low = case when @Seq = 1 then @SO_Low else @SO_Low * @alpha + @beta * @LastSO_Low end
	--3-Unit EA of Stochastic Oscillator The last price of the period
	select @SO3_Closing = case when @Seq = 1 then @SO_Closing else @SO_Closing * @alpha + @beta * @LastSO_Closing end
	--3-Unit EA of Stochastic Oscillator Total volume inthe period
	select @SO3_Volume = case when @Seq = 1 then @SO_Volume else @SO_Volume * @alpha + @beta * @LastSO_Volume end
	--3-Unit EA of Stochastic Oscillator Simple Moving Average of Closing
	select @SO3_SMAClosing = case when @Seq = 1 then @SO_SMAClosing else @SO_SMAClosing * @alpha + @beta * @LastSO_SMAClosing end
	--3-Unit EA of Stochastic Oscillator Simple Moving Average of Weighted Closing
	select @SO3_SMAWeightedClosing = case when @Seq = 1 then @SO_SMAWeightedClosing else @SO_SMAWeightedClosing * @alpha + @beta * @LastSO_SMAWeightedClosing end
	--3-Unit EA of Stochastic Oscillator Simple Moving Average of Typical Closing
	select @SO3_SMATypicalClosing = case when @Seq = 1 then @SO_SMATypicalClosing else @SO_SMATypicalClosing * @alpha + @beta * @LastSO_SMATypicalClosing end
	--3-Unit EA of Stochastic Oscillator Simple Moving Average of Volume
	select @SO3_SMAVolume = case when @Seq = 1 then @SO_SMAVolume else @SO_SMAVolume * @alpha + @beta * @LastSO_SMAVolume end
	--3-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Closing
	select @SO3_VAMAClosing = case when @Seq = 1 then @SO_VAMAClosing else @SO_VAMAClosing * @alpha + @beta * @LastSO_VAMAClosing end
	--3-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Typical Closing
	select @SO3_VAMATypicalClosing = case when @Seq = 1 then @SO_VAMATypicalClosing else @SO_VAMATypicalClosing * @alpha + @beta * @LastSO_VAMATypicalClosing end
	--3-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Weighted Closing
	select @SO3_VAMAWeightedClosing = case when @Seq = 1 then @SO_VAMAWeightedClosing else @SO_VAMAWeightedClosing * @alpha + @beta * @LastSO_VAMAWeightedClosing end
	--3-Unit EA of Stochastic Oscillator Weighted Moving Average of Closing
	select @SO3_WMAClosing = case when @Seq = 1 then @SO_WMAClosing else @SO_WMAClosing * @alpha + @beta * @LastSO_WMAClosing end
	--3-Unit EA of Stochastic Oscillator Weighted Moving Average of Weighted Closing
	select @SO3_WMAWeightedClosing = case when @Seq = 1 then @SO_WMAWeightedClosing else @SO_WMAWeightedClosing * @alpha + @beta * @LastSO_WMAWeightedClosing end
	--3-Unit EA of Stochastic Oscillator Weighted Moving Average of Typical Closing
	select @SO3_WMATypicalClosing = case when @Seq = 1 then @SO_WMATypicalClosing else @SO_WMATypicalClosing * @alpha + @beta * @LastSO_WMATypicalClosing end
	--3-Unit EA of Stochastic Oscillator Exponential Moving Average of Closing
	select @SO3_EMAClosing = case when @Seq = 1 then @SO_EMAClosing else @SO_EMAClosing * @alpha + @beta * @LastSO_EMAClosing end
	--3-Unit EA of Stochastic Oscillator Exponential Moving Average of Weighted Closing
	select @SO3_EMAWeightedClosing = case when @Seq = 1 then @SO_EMAWeightedClosing else @SO_EMAWeightedClosing * @alpha + @beta * @LastSO_EMAWeightedClosing end
	--3-Unit EA of Stochastic Oscillator Exponential Moving Average of Typical Closing
	select @SO3_EMATypicalClosing = case when @Seq = 1 then @SO_EMATypicalClosing else @SO_EMATypicalClosing * @alpha + @beta * @LastSO_EMATypicalClosing end
	--3-Unit EA of Stochastic Oscillator Exponential Moving Average of Volume
	select @SO3_EMAVolume = case when @Seq = 1 then @SO_EMAVolume else @SO_EMAVolume * @alpha + @beta * @LastSO_EMAVolume end
	--3-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Closing
	select @SO3_CMOClosing = case when @Seq = 1 then @SO_CMOClosing else @SO_CMOClosing * @alpha + @beta * @LastSO_CMOClosing end
	--3-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Volume
	select @SO3_CMOVolume = case when @Seq = 1 then @SO_CMOVolume else @SO_CMOVolume * @alpha + @beta * @LastSO_CMOVolume end
	--3-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Weighted Closing
	select @SO3_CMOWeightedClosing = case when @Seq = 1 then @SO_CMOWeightedClosing else @SO_CMOWeightedClosing * @alpha + @beta * @LastSO_CMOWeightedClosing end
	--3-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Typical Closing
	select @SO3_CMOTypicalClosing = case when @Seq = 1 then @SO_CMOTypicalClosing else @SO_CMOTypicalClosing * @alpha + @beta * @LastSO_CMOTypicalClosing end
	--3-Unit EA of Stochastic Oscillator Variable Moving Average of Closing
	select @SO3_VMAClosing = case when @Seq = 1 then @SO_VMAClosing else @SO_VMAClosing * @alpha + @beta * @LastSO_VMAClosing end
	--3-Unit EA of Stochastic Oscillator Variable Moving Average of Volume
	select @SO3_VMAVolume = case when @Seq = 1 then @SO_VMAVolume else @SO_VMAVolume * @alpha + @beta * @LastSO_VMAVolume end
	--3-Unit EA of Stochastic Oscillator Variable Moving Average of Weighted Closing
	select @SO3_VMAWeightedClosing = case when @Seq = 1 then @SO_VMAWeightedClosing else @SO_VMAWeightedClosing * @alpha + @beta * @LastSO_VMAWeightedClosing end
	--3-Unit EA of Stochastic Oscillator Variable Moving Average of Typical Closing
	select @SO3_VMATypicalClosing = case when @Seq = 1 then @SO_VMATypicalClosing else @SO_VMATypicalClosing * @alpha + @beta * @LastSO_VMATypicalClosing end
	--3-Unit EA of Stochastic Oscillator Wilder's Smooth  Closing
	select @SO3_WSClosing = case when @Seq = 1 then @SO_WSClosing else @SO_WSClosing * @alpha + @beta * @LastSO_WSClosing end
	--3-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Closing
	select @SO3_WSWeightedClosing = case when @Seq = 1 then @SO_WSWeightedClosing else @SO_WSWeightedClosing * @alpha + @beta * @LastSO_WSWeightedClosing end
	--3-Unit EA of Stochastic Oscillator Wilder's Smooth  Typical Closing
	select @SO3_WSTypicalClosing = case when @Seq = 1 then @SO_WSTypicalClosing else @SO_WSTypicalClosing * @alpha + @beta * @LastSO_WSTypicalClosing end
	--3-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Closing -- used for WMA
	select @SO3_WSWeightedNormalClosing = case when @Seq = 1 then @SO_WSWeightedNormalClosing else @SO_WSWeightedNormalClosing * @alpha + @beta * @LastSO_WSWeightedNormalClosing end
	--3-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Weighted Closing -- used for WMA
	select @SO3_WSWeightedWeightedClosing = case when @Seq = 1 then @SO_WSWeightedWeightedClosing else @SO_WSWeightedWeightedClosing * @alpha + @beta * @LastSO_WSWeightedWeightedClosing end
	--3-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Typical Closing -- used for WMA
	select @SO3_WSWeightedTypicalClosing = case when @Seq = 1 then @SO_WSWeightedTypicalClosing else @SO_WSWeightedTypicalClosing * @alpha + @beta * @LastSO_WSWeightedTypicalClosing end
	--3-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume
	select @SO3_WSVolume = case when @Seq = 1 then @SO_WSVolume else @SO_WSVolume * @alpha + @beta * @LastSO_WSVolume end
	--3-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Closing -- use for VAMA
	select @SO3_WSVolumeAdjustedClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedClosing else @SO_WSVolumeAdjustedClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedClosing end
	--3-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Typical Closing -- use for VAMA
	select @SO3_WSVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedTypicalClosing else @SO_WSVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedTypicalClosing end
	--3-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Weighted Closing -- use for VAMA
	select @SO3_WSVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedWeightedClosing else @SO_WSVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedWeightedClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Closing
	select @SO3_RSI_SMAClosing = case when @Seq = 1 then @SO_RSI_SMAClosing else @SO_RSI_SMAClosing * @alpha + @beta * @LastSO_RSI_SMAClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume
	select @SO3_RSI_SMAVolume = case when @Seq = 1 then @SO_RSI_SMAVolume else @SO_RSI_SMAVolume * @alpha + @beta * @LastSO_RSI_SMAVolume end
	--3-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Weighted Closing
	select @SO3_RSI_SMAWeightedClosing = case when @Seq = 1 then @SO_RSI_SMAWeightedClosing else @SO_RSI_SMAWeightedClosing * @alpha + @beta * @LastSO_RSI_SMAWeightedClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Typical Closing
	select @SO3_RSI_SMATypicalClosing = case when @Seq = 1 then @SO_RSI_SMATypicalClosing else @SO_RSI_SMATypicalClosing * @alpha + @beta * @LastSO_RSI_SMATypicalClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Closing
	select @SO3_RSI_SMAVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedClosing else @SO_RSI_SMAVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Weighted Closing
	select @SO3_RSI_SMAVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedWeightedClosing else @SO_RSI_SMAVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedWeightedClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Typical Closing
	select @SO3_RSI_SMAVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedTypicalClosing else @SO_RSI_SMAVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedTypicalClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Closing
	select @SO3_RSI_EMAClosing = case when @Seq = 1 then @SO_RSI_EMAClosing else @SO_RSI_EMAClosing * @alpha + @beta * @LastSO_RSI_EMAClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume
	select @SO3_RSI_EMAVolume = case when @Seq = 1 then @SO_RSI_EMAVolume else @SO_RSI_EMAVolume * @alpha + @beta * @LastSO_RSI_EMAVolume end
	--3-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Weighted Closing
	select @SO3_RSI_EMAWeightedClosing = case when @Seq = 1 then @SO_RSI_EMAWeightedClosing else @SO_RSI_EMAWeightedClosing * @alpha + @beta * @LastSO_RSI_EMAWeightedClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Typical Closing
	select @SO3_RSI_EMATypicalClosing = case when @Seq = 1 then @SO_RSI_EMATypicalClosing else @SO_RSI_EMATypicalClosing * @alpha + @beta * @LastSO_RSI_EMATypicalClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Closing
	select @SO3_RSI_EMAVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedClosing else @SO_RSI_EMAVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Weighted Closing
	select @SO3_RSI_EMAVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedWeightedClosing else @SO_RSI_EMAVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedWeightedClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Typical Closing
	select @SO3_RSI_EMAVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedTypicalClosing else @SO_RSI_EMAVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedTypicalClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Closing
	select @SO3_RSI_WSClosing = case when @Seq = 1 then @SO_RSI_WSClosing else @SO_RSI_WSClosing * @alpha + @beta * @LastSO_RSI_WSClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume
	select @SO3_RSI_WSVolume = case when @Seq = 1 then @SO_RSI_WSVolume else @SO_RSI_WSVolume * @alpha + @beta * @LastSO_RSI_WSVolume end
	--3-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Weighted Closing
	select @SO3_RSI_WSWeightedClosing = case when @Seq = 1 then @SO_RSI_WSWeightedClosing else @SO_RSI_WSWeightedClosing * @alpha + @beta * @LastSO_RSI_WSWeightedClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Typical Closing
	select @SO3_RSI_WSTypicalClosing = case when @Seq = 1 then @SO_RSI_WSTypicalClosing else @SO_RSI_WSTypicalClosing * @alpha + @beta * @LastSO_RSI_WSTypicalClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Closing
	select @SO3_RSI_WSVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedClosing else @SO_RSI_WSVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Weighted Closing
	select @SO3_RSI_WSVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedWeightedClosing else @SO_RSI_WSVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedWeightedClosing end
	--3-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Typical Closing
	select @SO3_RSI_WSVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedTypicalClosing else @SO_RSI_WSVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedTypicalClosing end
	--3-Unit EA of Stochastic Oscillator Standard Deviation of Closing
	select @SO3_STDEVClosing = case when @Seq = 1 then @SO_STDEVClosing else @SO_STDEVClosing * @alpha + @beta * @LastSO_STDEVClosing end
	--3-Unit EA of Stochastic Oscillator Standard Deviation of Weighted Closing
	select @SO3_STDEVWeightedClosing = case when @Seq = 1 then @SO_STDEVWeightedClosing else @SO_STDEVWeightedClosing * @alpha + @beta * @LastSO_STDEVWeightedClosing end
	--3-Unit EA of Stochastic Oscillator Standard Deviation of Typical Closing
	select @SO3_STDEVTypicalClosing = case when @Seq = 1 then @SO_STDEVTypicalClosing else @SO_STDEVTypicalClosing * @alpha + @beta * @LastSO_STDEVTypicalClosing end
	--3-Unit EA of Stochastic Oscillator Standard Deviation of Volume
	select @SO3_STDEVVolume = case when @Seq = 1 then @SO_STDEVVolume else @SO_STDEVVolume * @alpha + @beta * @LastSO_STDEVVolume end
	--3-Unit EA of Stochastic Oscillator Standard Deviation of Closing
	select @SO3_RVIClosing = case when @Seq = 1 then @SO_RVIClosing else @SO_RVIClosing * @alpha + @beta * @LastSO_RVIClosing end
	--3-Unit EA of Stochastic Oscillator Standard Deviation of Volume
	select @SO3_RVIVolume = case when @Seq = 1 then @SO_RVIVolume else @SO_RVIVolume * @alpha + @beta * @LastSO_RVIVolume end
	--3-Unit EA of Stochastic Oscillator Standard Deviation of Weighted Closing
	select @SO3_RVIWeightedClosing = case when @Seq = 1 then @SO_RVIWeightedClosing else @SO_RVIWeightedClosing * @alpha + @beta * @LastSO_RVIWeightedClosing end
	--3-Unit EA of Stochastic Oscillator Standard Deviation of Typical Closing
	select @SO3_RVITypicalClosing = case when @Seq = 1 then @SO_RVITypicalClosing else @SO_RVITypicalClosing * @alpha + @beta * @LastSO_RVITypicalClosing end
	--3-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Closing
	select @SO3_RVIVolumeAdjustedClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedClosing else @SO_RVIVolumeAdjustedClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedClosing end
	--3-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Weighted Closing
	select @SO3_RVIVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedWeightedClosing else @SO_RVIVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedWeightedClosing end
	--3-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Typical Closing
	select @SO3_RVIVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedTypicalClosing else @SO_RVIVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedTypicalClosing end
	select @alpha = @F_Two / (cast(4 as float) + @F_One)
	select @beta = @F_One - @alpha
	--4-Unit EA of Stochastic Oscillator The opening price of the period
	select @SO4_Opening = case when @Seq = 1 then @SO_Opening else @SO_Opening * @alpha + @beta * @LastSO_Opening end
	--4-Unit EA of Stochastic Oscillator The highest price in the period
	select @SO4_High = case when @Seq = 1 then @SO_High else @SO_High * @alpha + @beta * @LastSO_High end
	--4-Unit EA of Stochastic Oscillator The lowest price in the period
	select @SO4_Low = case when @Seq = 1 then @SO_Low else @SO_Low * @alpha + @beta * @LastSO_Low end
	--4-Unit EA of Stochastic Oscillator The last price of the period
	select @SO4_Closing = case when @Seq = 1 then @SO_Closing else @SO_Closing * @alpha + @beta * @LastSO_Closing end
	--4-Unit EA of Stochastic Oscillator Total volume inthe period
	select @SO4_Volume = case when @Seq = 1 then @SO_Volume else @SO_Volume * @alpha + @beta * @LastSO_Volume end
	--4-Unit EA of Stochastic Oscillator Simple Moving Average of Closing
	select @SO4_SMAClosing = case when @Seq = 1 then @SO_SMAClosing else @SO_SMAClosing * @alpha + @beta * @LastSO_SMAClosing end
	--4-Unit EA of Stochastic Oscillator Simple Moving Average of Weighted Closing
	select @SO4_SMAWeightedClosing = case when @Seq = 1 then @SO_SMAWeightedClosing else @SO_SMAWeightedClosing * @alpha + @beta * @LastSO_SMAWeightedClosing end
	--4-Unit EA of Stochastic Oscillator Simple Moving Average of Typical Closing
	select @SO4_SMATypicalClosing = case when @Seq = 1 then @SO_SMATypicalClosing else @SO_SMATypicalClosing * @alpha + @beta * @LastSO_SMATypicalClosing end
	--4-Unit EA of Stochastic Oscillator Simple Moving Average of Volume
	select @SO4_SMAVolume = case when @Seq = 1 then @SO_SMAVolume else @SO_SMAVolume * @alpha + @beta * @LastSO_SMAVolume end
	--4-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Closing
	select @SO4_VAMAClosing = case when @Seq = 1 then @SO_VAMAClosing else @SO_VAMAClosing * @alpha + @beta * @LastSO_VAMAClosing end
	--4-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Typical Closing
	select @SO4_VAMATypicalClosing = case when @Seq = 1 then @SO_VAMATypicalClosing else @SO_VAMATypicalClosing * @alpha + @beta * @LastSO_VAMATypicalClosing end
	--4-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Weighted Closing
	select @SO4_VAMAWeightedClosing = case when @Seq = 1 then @SO_VAMAWeightedClosing else @SO_VAMAWeightedClosing * @alpha + @beta * @LastSO_VAMAWeightedClosing end
	--4-Unit EA of Stochastic Oscillator Weighted Moving Average of Closing
	select @SO4_WMAClosing = case when @Seq = 1 then @SO_WMAClosing else @SO_WMAClosing * @alpha + @beta * @LastSO_WMAClosing end
	--4-Unit EA of Stochastic Oscillator Weighted Moving Average of Weighted Closing
	select @SO4_WMAWeightedClosing = case when @Seq = 1 then @SO_WMAWeightedClosing else @SO_WMAWeightedClosing * @alpha + @beta * @LastSO_WMAWeightedClosing end
	--4-Unit EA of Stochastic Oscillator Weighted Moving Average of Typical Closing
	select @SO4_WMATypicalClosing = case when @Seq = 1 then @SO_WMATypicalClosing else @SO_WMATypicalClosing * @alpha + @beta * @LastSO_WMATypicalClosing end
	--4-Unit EA of Stochastic Oscillator Exponential Moving Average of Closing
	select @SO4_EMAClosing = case when @Seq = 1 then @SO_EMAClosing else @SO_EMAClosing * @alpha + @beta * @LastSO_EMAClosing end
	--4-Unit EA of Stochastic Oscillator Exponential Moving Average of Weighted Closing
	select @SO4_EMAWeightedClosing = case when @Seq = 1 then @SO_EMAWeightedClosing else @SO_EMAWeightedClosing * @alpha + @beta * @LastSO_EMAWeightedClosing end
	--4-Unit EA of Stochastic Oscillator Exponential Moving Average of Typical Closing
	select @SO4_EMATypicalClosing = case when @Seq = 1 then @SO_EMATypicalClosing else @SO_EMATypicalClosing * @alpha + @beta * @LastSO_EMATypicalClosing end
	--4-Unit EA of Stochastic Oscillator Exponential Moving Average of Volume
	select @SO4_EMAVolume = case when @Seq = 1 then @SO_EMAVolume else @SO_EMAVolume * @alpha + @beta * @LastSO_EMAVolume end
	--4-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Closing
	select @SO4_CMOClosing = case when @Seq = 1 then @SO_CMOClosing else @SO_CMOClosing * @alpha + @beta * @LastSO_CMOClosing end
	--4-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Volume
	select @SO4_CMOVolume = case when @Seq = 1 then @SO_CMOVolume else @SO_CMOVolume * @alpha + @beta * @LastSO_CMOVolume end
	--4-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Weighted Closing
	select @SO4_CMOWeightedClosing = case when @Seq = 1 then @SO_CMOWeightedClosing else @SO_CMOWeightedClosing * @alpha + @beta * @LastSO_CMOWeightedClosing end
	--4-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Typical Closing
	select @SO4_CMOTypicalClosing = case when @Seq = 1 then @SO_CMOTypicalClosing else @SO_CMOTypicalClosing * @alpha + @beta * @LastSO_CMOTypicalClosing end
	--4-Unit EA of Stochastic Oscillator Variable Moving Average of Closing
	select @SO4_VMAClosing = case when @Seq = 1 then @SO_VMAClosing else @SO_VMAClosing * @alpha + @beta * @LastSO_VMAClosing end
	--4-Unit EA of Stochastic Oscillator Variable Moving Average of Volume
	select @SO4_VMAVolume = case when @Seq = 1 then @SO_VMAVolume else @SO_VMAVolume * @alpha + @beta * @LastSO_VMAVolume end
	--4-Unit EA of Stochastic Oscillator Variable Moving Average of Weighted Closing
	select @SO4_VMAWeightedClosing = case when @Seq = 1 then @SO_VMAWeightedClosing else @SO_VMAWeightedClosing * @alpha + @beta * @LastSO_VMAWeightedClosing end
	--4-Unit EA of Stochastic Oscillator Variable Moving Average of Typical Closing
	select @SO4_VMATypicalClosing = case when @Seq = 1 then @SO_VMATypicalClosing else @SO_VMATypicalClosing * @alpha + @beta * @LastSO_VMATypicalClosing end
	--4-Unit EA of Stochastic Oscillator Wilder's Smooth  Closing
	select @SO4_WSClosing = case when @Seq = 1 then @SO_WSClosing else @SO_WSClosing * @alpha + @beta * @LastSO_WSClosing end
	--4-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Closing
	select @SO4_WSWeightedClosing = case when @Seq = 1 then @SO_WSWeightedClosing else @SO_WSWeightedClosing * @alpha + @beta * @LastSO_WSWeightedClosing end
	--4-Unit EA of Stochastic Oscillator Wilder's Smooth  Typical Closing
	select @SO4_WSTypicalClosing = case when @Seq = 1 then @SO_WSTypicalClosing else @SO_WSTypicalClosing * @alpha + @beta * @LastSO_WSTypicalClosing end
	--4-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Closing -- used for WMA
	select @SO4_WSWeightedNormalClosing = case when @Seq = 1 then @SO_WSWeightedNormalClosing else @SO_WSWeightedNormalClosing * @alpha + @beta * @LastSO_WSWeightedNormalClosing end
	--4-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Weighted Closing -- used for WMA
	select @SO4_WSWeightedWeightedClosing = case when @Seq = 1 then @SO_WSWeightedWeightedClosing else @SO_WSWeightedWeightedClosing * @alpha + @beta * @LastSO_WSWeightedWeightedClosing end
	--4-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Typical Closing -- used for WMA
	select @SO4_WSWeightedTypicalClosing = case when @Seq = 1 then @SO_WSWeightedTypicalClosing else @SO_WSWeightedTypicalClosing * @alpha + @beta * @LastSO_WSWeightedTypicalClosing end
	--4-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume
	select @SO4_WSVolume = case when @Seq = 1 then @SO_WSVolume else @SO_WSVolume * @alpha + @beta * @LastSO_WSVolume end
	--4-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Closing -- use for VAMA
	select @SO4_WSVolumeAdjustedClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedClosing else @SO_WSVolumeAdjustedClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedClosing end
	--4-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Typical Closing -- use for VAMA
	select @SO4_WSVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedTypicalClosing else @SO_WSVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedTypicalClosing end
	--4-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Weighted Closing -- use for VAMA
	select @SO4_WSVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedWeightedClosing else @SO_WSVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedWeightedClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Closing
	select @SO4_RSI_SMAClosing = case when @Seq = 1 then @SO_RSI_SMAClosing else @SO_RSI_SMAClosing * @alpha + @beta * @LastSO_RSI_SMAClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume
	select @SO4_RSI_SMAVolume = case when @Seq = 1 then @SO_RSI_SMAVolume else @SO_RSI_SMAVolume * @alpha + @beta * @LastSO_RSI_SMAVolume end
	--4-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Weighted Closing
	select @SO4_RSI_SMAWeightedClosing = case when @Seq = 1 then @SO_RSI_SMAWeightedClosing else @SO_RSI_SMAWeightedClosing * @alpha + @beta * @LastSO_RSI_SMAWeightedClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Typical Closing
	select @SO4_RSI_SMATypicalClosing = case when @Seq = 1 then @SO_RSI_SMATypicalClosing else @SO_RSI_SMATypicalClosing * @alpha + @beta * @LastSO_RSI_SMATypicalClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Closing
	select @SO4_RSI_SMAVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedClosing else @SO_RSI_SMAVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Weighted Closing
	select @SO4_RSI_SMAVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedWeightedClosing else @SO_RSI_SMAVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedWeightedClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Typical Closing
	select @SO4_RSI_SMAVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedTypicalClosing else @SO_RSI_SMAVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedTypicalClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Closing
	select @SO4_RSI_EMAClosing = case when @Seq = 1 then @SO_RSI_EMAClosing else @SO_RSI_EMAClosing * @alpha + @beta * @LastSO_RSI_EMAClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume
	select @SO4_RSI_EMAVolume = case when @Seq = 1 then @SO_RSI_EMAVolume else @SO_RSI_EMAVolume * @alpha + @beta * @LastSO_RSI_EMAVolume end
	--4-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Weighted Closing
	select @SO4_RSI_EMAWeightedClosing = case when @Seq = 1 then @SO_RSI_EMAWeightedClosing else @SO_RSI_EMAWeightedClosing * @alpha + @beta * @LastSO_RSI_EMAWeightedClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Typical Closing
	select @SO4_RSI_EMATypicalClosing = case when @Seq = 1 then @SO_RSI_EMATypicalClosing else @SO_RSI_EMATypicalClosing * @alpha + @beta * @LastSO_RSI_EMATypicalClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Closing
	select @SO4_RSI_EMAVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedClosing else @SO_RSI_EMAVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Weighted Closing
	select @SO4_RSI_EMAVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedWeightedClosing else @SO_RSI_EMAVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedWeightedClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Typical Closing
	select @SO4_RSI_EMAVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedTypicalClosing else @SO_RSI_EMAVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedTypicalClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Closing
	select @SO4_RSI_WSClosing = case when @Seq = 1 then @SO_RSI_WSClosing else @SO_RSI_WSClosing * @alpha + @beta * @LastSO_RSI_WSClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume
	select @SO4_RSI_WSVolume = case when @Seq = 1 then @SO_RSI_WSVolume else @SO_RSI_WSVolume * @alpha + @beta * @LastSO_RSI_WSVolume end
	--4-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Weighted Closing
	select @SO4_RSI_WSWeightedClosing = case when @Seq = 1 then @SO_RSI_WSWeightedClosing else @SO_RSI_WSWeightedClosing * @alpha + @beta * @LastSO_RSI_WSWeightedClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Typical Closing
	select @SO4_RSI_WSTypicalClosing = case when @Seq = 1 then @SO_RSI_WSTypicalClosing else @SO_RSI_WSTypicalClosing * @alpha + @beta * @LastSO_RSI_WSTypicalClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Closing
	select @SO4_RSI_WSVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedClosing else @SO_RSI_WSVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Weighted Closing
	select @SO4_RSI_WSVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedWeightedClosing else @SO_RSI_WSVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedWeightedClosing end
	--4-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Typical Closing
	select @SO4_RSI_WSVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedTypicalClosing else @SO_RSI_WSVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedTypicalClosing end
	--4-Unit EA of Stochastic Oscillator Standard Deviation of Closing
	select @SO4_STDEVClosing = case when @Seq = 1 then @SO_STDEVClosing else @SO_STDEVClosing * @alpha + @beta * @LastSO_STDEVClosing end
	--4-Unit EA of Stochastic Oscillator Standard Deviation of Weighted Closing
	select @SO4_STDEVWeightedClosing = case when @Seq = 1 then @SO_STDEVWeightedClosing else @SO_STDEVWeightedClosing * @alpha + @beta * @LastSO_STDEVWeightedClosing end
	--4-Unit EA of Stochastic Oscillator Standard Deviation of Typical Closing
	select @SO4_STDEVTypicalClosing = case when @Seq = 1 then @SO_STDEVTypicalClosing else @SO_STDEVTypicalClosing * @alpha + @beta * @LastSO_STDEVTypicalClosing end
	--4-Unit EA of Stochastic Oscillator Standard Deviation of Volume
	select @SO4_STDEVVolume = case when @Seq = 1 then @SO_STDEVVolume else @SO_STDEVVolume * @alpha + @beta * @LastSO_STDEVVolume end
	--4-Unit EA of Stochastic Oscillator Standard Deviation of Closing
	select @SO4_RVIClosing = case when @Seq = 1 then @SO_RVIClosing else @SO_RVIClosing * @alpha + @beta * @LastSO_RVIClosing end
	--4-Unit EA of Stochastic Oscillator Standard Deviation of Volume
	select @SO4_RVIVolume = case when @Seq = 1 then @SO_RVIVolume else @SO_RVIVolume * @alpha + @beta * @LastSO_RVIVolume end
	--4-Unit EA of Stochastic Oscillator Standard Deviation of Weighted Closing
	select @SO4_RVIWeightedClosing = case when @Seq = 1 then @SO_RVIWeightedClosing else @SO_RVIWeightedClosing * @alpha + @beta * @LastSO_RVIWeightedClosing end
	--4-Unit EA of Stochastic Oscillator Standard Deviation of Typical Closing
	select @SO4_RVITypicalClosing = case when @Seq = 1 then @SO_RVITypicalClosing else @SO_RVITypicalClosing * @alpha + @beta * @LastSO_RVITypicalClosing end
	--4-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Closing
	select @SO4_RVIVolumeAdjustedClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedClosing else @SO_RVIVolumeAdjustedClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedClosing end
	--4-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Weighted Closing
	select @SO4_RVIVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedWeightedClosing else @SO_RVIVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedWeightedClosing end
	--4-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Typical Closing
	select @SO4_RVIVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedTypicalClosing else @SO_RVIVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedTypicalClosing end

	select @alpha = @F_Two / (cast(5 as float) + @F_One)
	select @beta = @F_One - @alpha
	--5-Unit EA of Stochastic Oscillator The opening price of the period
	select @SO5_Opening = case when @Seq = 1 then @SO_Opening else @SO_Opening * @alpha + @beta * @LastSO_Opening end
	--5-Unit EA of Stochastic Oscillator The highest price in the period
	select @SO5_High = case when @Seq = 1 then @SO_High else @SO_High * @alpha + @beta * @LastSO_High end
	--5-Unit EA of Stochastic Oscillator The lowest price in the period
	select @SO5_Low = case when @Seq = 1 then @SO_Low else @SO_Low * @alpha + @beta * @LastSO_Low end
	--5-Unit EA of Stochastic Oscillator The last price of the period
	select @SO5_Closing = case when @Seq = 1 then @SO_Closing else @SO_Closing * @alpha + @beta * @LastSO_Closing end
	--5-Unit EA of Stochastic Oscillator Total volume inthe period
	select @SO5_Volume = case when @Seq = 1 then @SO_Volume else @SO_Volume * @alpha + @beta * @LastSO_Volume end
	--5-Unit EA of Stochastic Oscillator Simple Moving Average of Closing
	select @SO5_SMAClosing = case when @Seq = 1 then @SO_SMAClosing else @SO_SMAClosing * @alpha + @beta * @LastSO_SMAClosing end
	--5-Unit EA of Stochastic Oscillator Simple Moving Average of Weighted Closing
	select @SO5_SMAWeightedClosing = case when @Seq = 1 then @SO_SMAWeightedClosing else @SO_SMAWeightedClosing * @alpha + @beta * @LastSO_SMAWeightedClosing end
	--5-Unit EA of Stochastic Oscillator Simple Moving Average of Typical Closing
	select @SO5_SMATypicalClosing = case when @Seq = 1 then @SO_SMATypicalClosing else @SO_SMATypicalClosing * @alpha + @beta * @LastSO_SMATypicalClosing end
	--5-Unit EA of Stochastic Oscillator Simple Moving Average of Volume
	select @SO5_SMAVolume = case when @Seq = 1 then @SO_SMAVolume else @SO_SMAVolume * @alpha + @beta * @LastSO_SMAVolume end
	--5-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Closing
	select @SO5_VAMAClosing = case when @Seq = 1 then @SO_VAMAClosing else @SO_VAMAClosing * @alpha + @beta * @LastSO_VAMAClosing end
	--5-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Typical Closing
	select @SO5_VAMATypicalClosing = case when @Seq = 1 then @SO_VAMATypicalClosing else @SO_VAMATypicalClosing * @alpha + @beta * @LastSO_VAMATypicalClosing end
	--5-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Weighted Closing
	select @SO5_VAMAWeightedClosing = case when @Seq = 1 then @SO_VAMAWeightedClosing else @SO_VAMAWeightedClosing * @alpha + @beta * @LastSO_VAMAWeightedClosing end
	--5-Unit EA of Stochastic Oscillator Weighted Moving Average of Closing
	select @SO5_WMAClosing = case when @Seq = 1 then @SO_WMAClosing else @SO_WMAClosing * @alpha + @beta * @LastSO_WMAClosing end
	--5-Unit EA of Stochastic Oscillator Weighted Moving Average of Weighted Closing
	select @SO5_WMAWeightedClosing = case when @Seq = 1 then @SO_WMAWeightedClosing else @SO_WMAWeightedClosing * @alpha + @beta * @LastSO_WMAWeightedClosing end
	--5-Unit EA of Stochastic Oscillator Weighted Moving Average of Typical Closing
	select @SO5_WMATypicalClosing = case when @Seq = 1 then @SO_WMATypicalClosing else @SO_WMATypicalClosing * @alpha + @beta * @LastSO_WMATypicalClosing end
	--5-Unit EA of Stochastic Oscillator Exponential Moving Average of Closing
	select @SO5_EMAClosing = case when @Seq = 1 then @SO_EMAClosing else @SO_EMAClosing * @alpha + @beta * @LastSO_EMAClosing end
	--5-Unit EA of Stochastic Oscillator Exponential Moving Average of Weighted Closing
	select @SO5_EMAWeightedClosing = case when @Seq = 1 then @SO_EMAWeightedClosing else @SO_EMAWeightedClosing * @alpha + @beta * @LastSO_EMAWeightedClosing end
	--5-Unit EA of Stochastic Oscillator Exponential Moving Average of Typical Closing
	select @SO5_EMATypicalClosing = case when @Seq = 1 then @SO_EMATypicalClosing else @SO_EMATypicalClosing * @alpha + @beta * @LastSO_EMATypicalClosing end
	--5-Unit EA of Stochastic Oscillator Exponential Moving Average of Volume
	select @SO5_EMAVolume = case when @Seq = 1 then @SO_EMAVolume else @SO_EMAVolume * @alpha + @beta * @LastSO_EMAVolume end
	--5-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Closing
	select @SO5_CMOClosing = case when @Seq = 1 then @SO_CMOClosing else @SO_CMOClosing * @alpha + @beta * @LastSO_CMOClosing end
	--5-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Volume
	select @SO5_CMOVolume = case when @Seq = 1 then @SO_CMOVolume else @SO_CMOVolume * @alpha + @beta * @LastSO_CMOVolume end
	--5-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Weighted Closing
	select @SO5_CMOWeightedClosing = case when @Seq = 1 then @SO_CMOWeightedClosing else @SO_CMOWeightedClosing * @alpha + @beta * @LastSO_CMOWeightedClosing end
	--5-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Typical Closing
	select @SO5_CMOTypicalClosing = case when @Seq = 1 then @SO_CMOTypicalClosing else @SO_CMOTypicalClosing * @alpha + @beta * @LastSO_CMOTypicalClosing end
	--5-Unit EA of Stochastic Oscillator Variable Moving Average of Closing
	select @SO5_VMAClosing = case when @Seq = 1 then @SO_VMAClosing else @SO_VMAClosing * @alpha + @beta * @LastSO_VMAClosing end
	--5-Unit EA of Stochastic Oscillator Variable Moving Average of Volume
	select @SO5_VMAVolume = case when @Seq = 1 then @SO_VMAVolume else @SO_VMAVolume * @alpha + @beta * @LastSO_VMAVolume end
	--5-Unit EA of Stochastic Oscillator Variable Moving Average of Weighted Closing
	select @SO5_VMAWeightedClosing = case when @Seq = 1 then @SO_VMAWeightedClosing else @SO_VMAWeightedClosing * @alpha + @beta * @LastSO_VMAWeightedClosing end
	--5-Unit EA of Stochastic Oscillator Variable Moving Average of Typical Closing
	select @SO5_VMATypicalClosing = case when @Seq = 1 then @SO_VMATypicalClosing else @SO_VMATypicalClosing * @alpha + @beta * @LastSO_VMATypicalClosing end
	--5-Unit EA of Stochastic Oscillator Wilder's Smooth  Closing
	select @SO5_WSClosing = case when @Seq = 1 then @SO_WSClosing else @SO_WSClosing * @alpha + @beta * @LastSO_WSClosing end
	--5-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Closing
	select @SO5_WSWeightedClosing = case when @Seq = 1 then @SO_WSWeightedClosing else @SO_WSWeightedClosing * @alpha + @beta * @LastSO_WSWeightedClosing end
	--5-Unit EA of Stochastic Oscillator Wilder's Smooth  Typical Closing
	select @SO5_WSTypicalClosing = case when @Seq = 1 then @SO_WSTypicalClosing else @SO_WSTypicalClosing * @alpha + @beta * @LastSO_WSTypicalClosing end
	--5-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Closing -- used for WMA
	select @SO5_WSWeightedNormalClosing = case when @Seq = 1 then @SO_WSWeightedNormalClosing else @SO_WSWeightedNormalClosing * @alpha + @beta * @LastSO_WSWeightedNormalClosing end
	--5-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Weighted Closing -- used for WMA
	select @SO5_WSWeightedWeightedClosing = case when @Seq = 1 then @SO_WSWeightedWeightedClosing else @SO_WSWeightedWeightedClosing * @alpha + @beta * @LastSO_WSWeightedWeightedClosing end
	--5-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Typical Closing -- used for WMA
	select @SO5_WSWeightedTypicalClosing = case when @Seq = 1 then @SO_WSWeightedTypicalClosing else @SO_WSWeightedTypicalClosing * @alpha + @beta * @LastSO_WSWeightedTypicalClosing end
	--5-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume
	select @SO5_WSVolume = case when @Seq = 1 then @SO_WSVolume else @SO_WSVolume * @alpha + @beta * @LastSO_WSVolume end
	--5-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Closing -- use for VAMA
	select @SO5_WSVolumeAdjustedClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedClosing else @SO_WSVolumeAdjustedClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedClosing end
	--5-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Typical Closing -- use for VAMA
	select @SO5_WSVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedTypicalClosing else @SO_WSVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedTypicalClosing end
	--5-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Weighted Closing -- use for VAMA
	select @SO5_WSVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedWeightedClosing else @SO_WSVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedWeightedClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Closing
	select @SO5_RSI_SMAClosing = case when @Seq = 1 then @SO_RSI_SMAClosing else @SO_RSI_SMAClosing * @alpha + @beta * @LastSO_RSI_SMAClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume
	select @SO5_RSI_SMAVolume = case when @Seq = 1 then @SO_RSI_SMAVolume else @SO_RSI_SMAVolume * @alpha + @beta * @LastSO_RSI_SMAVolume end
	--5-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Weighted Closing
	select @SO5_RSI_SMAWeightedClosing = case when @Seq = 1 then @SO_RSI_SMAWeightedClosing else @SO_RSI_SMAWeightedClosing * @alpha + @beta * @LastSO_RSI_SMAWeightedClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Typical Closing
	select @SO5_RSI_SMATypicalClosing = case when @Seq = 1 then @SO_RSI_SMATypicalClosing else @SO_RSI_SMATypicalClosing * @alpha + @beta * @LastSO_RSI_SMATypicalClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Closing
	select @SO5_RSI_SMAVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedClosing else @SO_RSI_SMAVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Weighted Closing
	select @SO5_RSI_SMAVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedWeightedClosing else @SO_RSI_SMAVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedWeightedClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Typical Closing
	select @SO5_RSI_SMAVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedTypicalClosing else @SO_RSI_SMAVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedTypicalClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Closing
	select @SO5_RSI_EMAClosing = case when @Seq = 1 then @SO_RSI_EMAClosing else @SO_RSI_EMAClosing * @alpha + @beta * @LastSO_RSI_EMAClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume
	select @SO5_RSI_EMAVolume = case when @Seq = 1 then @SO_RSI_EMAVolume else @SO_RSI_EMAVolume * @alpha + @beta * @LastSO_RSI_EMAVolume end
	--5-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Weighted Closing
	select @SO5_RSI_EMAWeightedClosing = case when @Seq = 1 then @SO_RSI_EMAWeightedClosing else @SO_RSI_EMAWeightedClosing * @alpha + @beta * @LastSO_RSI_EMAWeightedClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Typical Closing
	select @SO5_RSI_EMATypicalClosing = case when @Seq = 1 then @SO_RSI_EMATypicalClosing else @SO_RSI_EMATypicalClosing * @alpha + @beta * @LastSO_RSI_EMATypicalClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Closing
	select @SO5_RSI_EMAVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedClosing else @SO_RSI_EMAVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Weighted Closing
	select @SO5_RSI_EMAVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedWeightedClosing else @SO_RSI_EMAVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedWeightedClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Typical Closing
	select @SO5_RSI_EMAVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedTypicalClosing else @SO_RSI_EMAVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedTypicalClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Closing
	select @SO5_RSI_WSClosing = case when @Seq = 1 then @SO_RSI_WSClosing else @SO_RSI_WSClosing * @alpha + @beta * @LastSO_RSI_WSClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume
	select @SO5_RSI_WSVolume = case when @Seq = 1 then @SO_RSI_WSVolume else @SO_RSI_WSVolume * @alpha + @beta * @LastSO_RSI_WSVolume end
	--5-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Weighted Closing
	select @SO5_RSI_WSWeightedClosing = case when @Seq = 1 then @SO_RSI_WSWeightedClosing else @SO_RSI_WSWeightedClosing * @alpha + @beta * @LastSO_RSI_WSWeightedClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Typical Closing
	select @SO5_RSI_WSTypicalClosing = case when @Seq = 1 then @SO_RSI_WSTypicalClosing else @SO_RSI_WSTypicalClosing * @alpha + @beta * @LastSO_RSI_WSTypicalClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Closing
	select @SO5_RSI_WSVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedClosing else @SO_RSI_WSVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Weighted Closing
	select @SO5_RSI_WSVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedWeightedClosing else @SO_RSI_WSVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedWeightedClosing end
	--5-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Typical Closing
	select @SO5_RSI_WSVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedTypicalClosing else @SO_RSI_WSVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedTypicalClosing end
	--5-Unit EA of Stochastic Oscillator Standard Deviation of Closing
	select @SO5_STDEVClosing = case when @Seq = 1 then @SO_STDEVClosing else @SO_STDEVClosing * @alpha + @beta * @LastSO_STDEVClosing end
	--5-Unit EA of Stochastic Oscillator Standard Deviation of Weighted Closing
	select @SO5_STDEVWeightedClosing = case when @Seq = 1 then @SO_STDEVWeightedClosing else @SO_STDEVWeightedClosing * @alpha + @beta * @LastSO_STDEVWeightedClosing end
	--5-Unit EA of Stochastic Oscillator Standard Deviation of Typical Closing
	select @SO5_STDEVTypicalClosing = case when @Seq = 1 then @SO_STDEVTypicalClosing else @SO_STDEVTypicalClosing * @alpha + @beta * @LastSO_STDEVTypicalClosing end
	--5-Unit EA of Stochastic Oscillator Standard Deviation of Volume
	select @SO5_STDEVVolume = case when @Seq = 1 then @SO_STDEVVolume else @SO_STDEVVolume * @alpha + @beta * @LastSO_STDEVVolume end
	--5-Unit EA of Stochastic Oscillator Standard Deviation of Closing
	select @SO5_RVIClosing = case when @Seq = 1 then @SO_RVIClosing else @SO_RVIClosing * @alpha + @beta * @LastSO_RVIClosing end
	--5-Unit EA of Stochastic Oscillator Standard Deviation of Volume
	select @SO5_RVIVolume = case when @Seq = 1 then @SO_RVIVolume else @SO_RVIVolume * @alpha + @beta * @LastSO_RVIVolume end
	--5-Unit EA of Stochastic Oscillator Standard Deviation of Weighted Closing
	select @SO5_RVIWeightedClosing = case when @Seq = 1 then @SO_RVIWeightedClosing else @SO_RVIWeightedClosing * @alpha + @beta * @LastSO_RVIWeightedClosing end
	--5-Unit EA of Stochastic Oscillator Standard Deviation of Typical Closing
	select @SO5_RVITypicalClosing = case when @Seq = 1 then @SO_RVITypicalClosing else @SO_RVITypicalClosing * @alpha + @beta * @LastSO_RVITypicalClosing end
	--5-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Closing
	select @SO5_RVIVolumeAdjustedClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedClosing else @SO_RVIVolumeAdjustedClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedClosing end
	--5-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Weighted Closing
	select @SO5_RVIVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedWeightedClosing else @SO_RVIVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedWeightedClosing end
	--5-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Typical Closing
	select @SO5_RVIVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedTypicalClosing else @SO_RVIVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedTypicalClosing end

	select @alpha = @F_Two / (cast(6 as float) + @F_One)
	select @beta = @F_One - @alpha
	--6-Unit EA of Stochastic Oscillator The opening price of the period
	select @SO6_Opening = case when @Seq = 1 then @SO_Opening else @SO_Opening * @alpha + @beta * @LastSO_Opening end
	--6-Unit EA of Stochastic Oscillator The highest price in the period
	select @SO6_High = case when @Seq = 1 then @SO_High else @SO_High * @alpha + @beta * @LastSO_High end
	--6-Unit EA of Stochastic Oscillator The lowest price in the period
	select @SO6_Low = case when @Seq = 1 then @SO_Low else @SO_Low * @alpha + @beta * @LastSO_Low end
	--6-Unit EA of Stochastic Oscillator The last price of the period
	select @SO6_Closing = case when @Seq = 1 then @SO_Closing else @SO_Closing * @alpha + @beta * @LastSO_Closing end
	--6-Unit EA of Stochastic Oscillator Total volume inthe period
	select @SO6_Volume = case when @Seq = 1 then @SO_Volume else @SO_Volume * @alpha + @beta * @LastSO_Volume end
	--6-Unit EA of Stochastic Oscillator Simple Moving Average of Closing
	select @SO6_SMAClosing = case when @Seq = 1 then @SO_SMAClosing else @SO_SMAClosing * @alpha + @beta * @LastSO_SMAClosing end
	--6-Unit EA of Stochastic Oscillator Simple Moving Average of Weighted Closing
	select @SO6_SMAWeightedClosing = case when @Seq = 1 then @SO_SMAWeightedClosing else @SO_SMAWeightedClosing * @alpha + @beta * @LastSO_SMAWeightedClosing end
	--6-Unit EA of Stochastic Oscillator Simple Moving Average of Typical Closing
	select @SO6_SMATypicalClosing = case when @Seq = 1 then @SO_SMATypicalClosing else @SO_SMATypicalClosing * @alpha + @beta * @LastSO_SMATypicalClosing end
	--6-Unit EA of Stochastic Oscillator Simple Moving Average of Volume
	select @SO6_SMAVolume = case when @Seq = 1 then @SO_SMAVolume else @SO_SMAVolume * @alpha + @beta * @LastSO_SMAVolume end
	--6-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Closing
	select @SO6_VAMAClosing = case when @Seq = 1 then @SO_VAMAClosing else @SO_VAMAClosing * @alpha + @beta * @LastSO_VAMAClosing end
	--6-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Typical Closing
	select @SO6_VAMATypicalClosing = case when @Seq = 1 then @SO_VAMATypicalClosing else @SO_VAMATypicalClosing * @alpha + @beta * @LastSO_VAMATypicalClosing end
	--6-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Weighted Closing
	select @SO6_VAMAWeightedClosing = case when @Seq = 1 then @SO_VAMAWeightedClosing else @SO_VAMAWeightedClosing * @alpha + @beta * @LastSO_VAMAWeightedClosing end
	--6-Unit EA of Stochastic Oscillator Weighted Moving Average of Closing
	select @SO6_WMAClosing = case when @Seq = 1 then @SO_WMAClosing else @SO_WMAClosing * @alpha + @beta * @LastSO_WMAClosing end
	--6-Unit EA of Stochastic Oscillator Weighted Moving Average of Weighted Closing
	select @SO6_WMAWeightedClosing = case when @Seq = 1 then @SO_WMAWeightedClosing else @SO_WMAWeightedClosing * @alpha + @beta * @LastSO_WMAWeightedClosing end
	--6-Unit EA of Stochastic Oscillator Weighted Moving Average of Typical Closing
	select @SO6_WMATypicalClosing = case when @Seq = 1 then @SO_WMATypicalClosing else @SO_WMATypicalClosing * @alpha + @beta * @LastSO_WMATypicalClosing end
	--6-Unit EA of Stochastic Oscillator Exponential Moving Average of Closing
	select @SO6_EMAClosing = case when @Seq = 1 then @SO_EMAClosing else @SO_EMAClosing * @alpha + @beta * @LastSO_EMAClosing end
	--6-Unit EA of Stochastic Oscillator Exponential Moving Average of Weighted Closing
	select @SO6_EMAWeightedClosing = case when @Seq = 1 then @SO_EMAWeightedClosing else @SO_EMAWeightedClosing * @alpha + @beta * @LastSO_EMAWeightedClosing end
	--6-Unit EA of Stochastic Oscillator Exponential Moving Average of Typical Closing
	select @SO6_EMATypicalClosing = case when @Seq = 1 then @SO_EMATypicalClosing else @SO_EMATypicalClosing * @alpha + @beta * @LastSO_EMATypicalClosing end
	--6-Unit EA of Stochastic Oscillator Exponential Moving Average of Volume
	select @SO6_EMAVolume = case when @Seq = 1 then @SO_EMAVolume else @SO_EMAVolume * @alpha + @beta * @LastSO_EMAVolume end
	--6-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Closing
	select @SO6_CMOClosing = case when @Seq = 1 then @SO_CMOClosing else @SO_CMOClosing * @alpha + @beta * @LastSO_CMOClosing end
	--6-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Volume
	select @SO6_CMOVolume = case when @Seq = 1 then @SO_CMOVolume else @SO_CMOVolume * @alpha + @beta * @LastSO_CMOVolume end
	--6-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Weighted Closing
	select @SO6_CMOWeightedClosing = case when @Seq = 1 then @SO_CMOWeightedClosing else @SO_CMOWeightedClosing * @alpha + @beta * @LastSO_CMOWeightedClosing end
	--6-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Typical Closing
	select @SO6_CMOTypicalClosing = case when @Seq = 1 then @SO_CMOTypicalClosing else @SO_CMOTypicalClosing * @alpha + @beta * @LastSO_CMOTypicalClosing end
	--6-Unit EA of Stochastic Oscillator Variable Moving Average of Closing
	select @SO6_VMAClosing = case when @Seq = 1 then @SO_VMAClosing else @SO_VMAClosing * @alpha + @beta * @LastSO_VMAClosing end
	--6-Unit EA of Stochastic Oscillator Variable Moving Average of Volume
	select @SO6_VMAVolume = case when @Seq = 1 then @SO_VMAVolume else @SO_VMAVolume * @alpha + @beta * @LastSO_VMAVolume end
	--6-Unit EA of Stochastic Oscillator Variable Moving Average of Weighted Closing
	select @SO6_VMAWeightedClosing = case when @Seq = 1 then @SO_VMAWeightedClosing else @SO_VMAWeightedClosing * @alpha + @beta * @LastSO_VMAWeightedClosing end
	--6-Unit EA of Stochastic Oscillator Variable Moving Average of Typical Closing
	select @SO6_VMATypicalClosing = case when @Seq = 1 then @SO_VMATypicalClosing else @SO_VMATypicalClosing * @alpha + @beta * @LastSO_VMATypicalClosing end
	--6-Unit EA of Stochastic Oscillator Wilder's Smooth  Closing
	select @SO6_WSClosing = case when @Seq = 1 then @SO_WSClosing else @SO_WSClosing * @alpha + @beta * @LastSO_WSClosing end
	--6-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Closing
	select @SO6_WSWeightedClosing = case when @Seq = 1 then @SO_WSWeightedClosing else @SO_WSWeightedClosing * @alpha + @beta * @LastSO_WSWeightedClosing end
	--6-Unit EA of Stochastic Oscillator Wilder's Smooth  Typical Closing
	select @SO6_WSTypicalClosing = case when @Seq = 1 then @SO_WSTypicalClosing else @SO_WSTypicalClosing * @alpha + @beta * @LastSO_WSTypicalClosing end
	--6-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Closing -- used for WMA
	select @SO6_WSWeightedNormalClosing = case when @Seq = 1 then @SO_WSWeightedNormalClosing else @SO_WSWeightedNormalClosing * @alpha + @beta * @LastSO_WSWeightedNormalClosing end
	--6-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Weighted Closing -- used for WMA
	select @SO6_WSWeightedWeightedClosing = case when @Seq = 1 then @SO_WSWeightedWeightedClosing else @SO_WSWeightedWeightedClosing * @alpha + @beta * @LastSO_WSWeightedWeightedClosing end
	--6-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Typical Closing -- used for WMA
	select @SO6_WSWeightedTypicalClosing = case when @Seq = 1 then @SO_WSWeightedTypicalClosing else @SO_WSWeightedTypicalClosing * @alpha + @beta * @LastSO_WSWeightedTypicalClosing end
	--6-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume
	select @SO6_WSVolume = case when @Seq = 1 then @SO_WSVolume else @SO_WSVolume * @alpha + @beta * @LastSO_WSVolume end
	--6-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Closing -- use for VAMA
	select @SO6_WSVolumeAdjustedClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedClosing else @SO_WSVolumeAdjustedClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedClosing end
	--6-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Typical Closing -- use for VAMA
	select @SO6_WSVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedTypicalClosing else @SO_WSVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedTypicalClosing end
	--6-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Weighted Closing -- use for VAMA
	select @SO6_WSVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedWeightedClosing else @SO_WSVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedWeightedClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Closing
	select @SO6_RSI_SMAClosing = case when @Seq = 1 then @SO_RSI_SMAClosing else @SO_RSI_SMAClosing * @alpha + @beta * @LastSO_RSI_SMAClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume
	select @SO6_RSI_SMAVolume = case when @Seq = 1 then @SO_RSI_SMAVolume else @SO_RSI_SMAVolume * @alpha + @beta * @LastSO_RSI_SMAVolume end
	--6-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Weighted Closing
	select @SO6_RSI_SMAWeightedClosing = case when @Seq = 1 then @SO_RSI_SMAWeightedClosing else @SO_RSI_SMAWeightedClosing * @alpha + @beta * @LastSO_RSI_SMAWeightedClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Typical Closing
	select @SO6_RSI_SMATypicalClosing = case when @Seq = 1 then @SO_RSI_SMATypicalClosing else @SO_RSI_SMATypicalClosing * @alpha + @beta * @LastSO_RSI_SMATypicalClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Closing
	select @SO6_RSI_SMAVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedClosing else @SO_RSI_SMAVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Weighted Closing
	select @SO6_RSI_SMAVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedWeightedClosing else @SO_RSI_SMAVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedWeightedClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Typical Closing
	select @SO6_RSI_SMAVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedTypicalClosing else @SO_RSI_SMAVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedTypicalClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Closing
	select @SO6_RSI_EMAClosing = case when @Seq = 1 then @SO_RSI_EMAClosing else @SO_RSI_EMAClosing * @alpha + @beta * @LastSO_RSI_EMAClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume
	select @SO6_RSI_EMAVolume = case when @Seq = 1 then @SO_RSI_EMAVolume else @SO_RSI_EMAVolume * @alpha + @beta * @LastSO_RSI_EMAVolume end
	--6-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Weighted Closing
	select @SO6_RSI_EMAWeightedClosing = case when @Seq = 1 then @SO_RSI_EMAWeightedClosing else @SO_RSI_EMAWeightedClosing * @alpha + @beta * @LastSO_RSI_EMAWeightedClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Typical Closing
	select @SO6_RSI_EMATypicalClosing = case when @Seq = 1 then @SO_RSI_EMATypicalClosing else @SO_RSI_EMATypicalClosing * @alpha + @beta * @LastSO_RSI_EMATypicalClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Closing
	select @SO6_RSI_EMAVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedClosing else @SO_RSI_EMAVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Weighted Closing
	select @SO6_RSI_EMAVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedWeightedClosing else @SO_RSI_EMAVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedWeightedClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Typical Closing
	select @SO6_RSI_EMAVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedTypicalClosing else @SO_RSI_EMAVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedTypicalClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Closing
	select @SO6_RSI_WSClosing = case when @Seq = 1 then @SO_RSI_WSClosing else @SO_RSI_WSClosing * @alpha + @beta * @LastSO_RSI_WSClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume
	select @SO6_RSI_WSVolume = case when @Seq = 1 then @SO_RSI_WSVolume else @SO_RSI_WSVolume * @alpha + @beta * @LastSO_RSI_WSVolume end
	--6-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Weighted Closing
	select @SO6_RSI_WSWeightedClosing = case when @Seq = 1 then @SO_RSI_WSWeightedClosing else @SO_RSI_WSWeightedClosing * @alpha + @beta * @LastSO_RSI_WSWeightedClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Typical Closing
	select @SO6_RSI_WSTypicalClosing = case when @Seq = 1 then @SO_RSI_WSTypicalClosing else @SO_RSI_WSTypicalClosing * @alpha + @beta * @LastSO_RSI_WSTypicalClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Closing
	select @SO6_RSI_WSVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedClosing else @SO_RSI_WSVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Weighted Closing
	select @SO6_RSI_WSVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedWeightedClosing else @SO_RSI_WSVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedWeightedClosing end
	--6-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Typical Closing
	select @SO6_RSI_WSVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedTypicalClosing else @SO_RSI_WSVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedTypicalClosing end
	--6-Unit EA of Stochastic Oscillator Standard Deviation of Closing
	select @SO6_STDEVClosing = case when @Seq = 1 then @SO_STDEVClosing else @SO_STDEVClosing * @alpha + @beta * @LastSO_STDEVClosing end
	--6-Unit EA of Stochastic Oscillator Standard Deviation of Weighted Closing
	select @SO6_STDEVWeightedClosing = case when @Seq = 1 then @SO_STDEVWeightedClosing else @SO_STDEVWeightedClosing * @alpha + @beta * @LastSO_STDEVWeightedClosing end
	--6-Unit EA of Stochastic Oscillator Standard Deviation of Typical Closing
	select @SO6_STDEVTypicalClosing = case when @Seq = 1 then @SO_STDEVTypicalClosing else @SO_STDEVTypicalClosing * @alpha + @beta * @LastSO_STDEVTypicalClosing end
	--6-Unit EA of Stochastic Oscillator Standard Deviation of Volume
	select @SO6_STDEVVolume = case when @Seq = 1 then @SO_STDEVVolume else @SO_STDEVVolume * @alpha + @beta * @LastSO_STDEVVolume end
	--6-Unit EA of Stochastic Oscillator Standard Deviation of Closing
	select @SO6_RVIClosing = case when @Seq = 1 then @SO_RVIClosing else @SO_RVIClosing * @alpha + @beta * @LastSO_RVIClosing end
	--6-Unit EA of Stochastic Oscillator Standard Deviation of Volume
	select @SO6_RVIVolume = case when @Seq = 1 then @SO_RVIVolume else @SO_RVIVolume * @alpha + @beta * @LastSO_RVIVolume end
	--6-Unit EA of Stochastic Oscillator Standard Deviation of Weighted Closing
	select @SO6_RVIWeightedClosing = case when @Seq = 1 then @SO_RVIWeightedClosing else @SO_RVIWeightedClosing * @alpha + @beta * @LastSO_RVIWeightedClosing end
	--6-Unit EA of Stochastic Oscillator Standard Deviation of Typical Closing
	select @SO6_RVITypicalClosing = case when @Seq = 1 then @SO_RVITypicalClosing else @SO_RVITypicalClosing * @alpha + @beta * @LastSO_RVITypicalClosing end
	--6-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Closing
	select @SO6_RVIVolumeAdjustedClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedClosing else @SO_RVIVolumeAdjustedClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedClosing end
	--6-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Weighted Closing
	select @SO6_RVIVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedWeightedClosing else @SO_RVIVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedWeightedClosing end
	--6-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Typical Closing
	select @SO6_RVIVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedTypicalClosing else @SO_RVIVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedTypicalClosing end

	select @alpha = @F_Two / (cast(7 as float) + @F_One)
	select @beta = @F_One - @alpha
	--7-Unit EA of Stochastic Oscillator The opening price of the period
	select @SO7_Opening = case when @Seq = 1 then @SO_Opening else @SO_Opening * @alpha + @beta * @LastSO_Opening end
	--7-Unit EA of Stochastic Oscillator The highest price in the period
	select @SO7_High = case when @Seq = 1 then @SO_High else @SO_High * @alpha + @beta * @LastSO_High end
	--7-Unit EA of Stochastic Oscillator The lowest price in the period
	select @SO7_Low = case when @Seq = 1 then @SO_Low else @SO_Low * @alpha + @beta * @LastSO_Low end
	--7-Unit EA of Stochastic Oscillator The last price of the period
	select @SO7_Closing = case when @Seq = 1 then @SO_Closing else @SO_Closing * @alpha + @beta * @LastSO_Closing end
	--7-Unit EA of Stochastic Oscillator Total volume inthe period
	select @SO7_Volume = case when @Seq = 1 then @SO_Volume else @SO_Volume * @alpha + @beta * @LastSO_Volume end
	--7-Unit EA of Stochastic Oscillator Simple Moving Average of Closing
	select @SO7_SMAClosing = case when @Seq = 1 then @SO_SMAClosing else @SO_SMAClosing * @alpha + @beta * @LastSO_SMAClosing end
	--7-Unit EA of Stochastic Oscillator Simple Moving Average of Weighted Closing
	select @SO7_SMAWeightedClosing = case when @Seq = 1 then @SO_SMAWeightedClosing else @SO_SMAWeightedClosing * @alpha + @beta * @LastSO_SMAWeightedClosing end
	--7-Unit EA of Stochastic Oscillator Simple Moving Average of Typical Closing
	select @SO7_SMATypicalClosing = case when @Seq = 1 then @SO_SMATypicalClosing else @SO_SMATypicalClosing * @alpha + @beta * @LastSO_SMATypicalClosing end
	--7-Unit EA of Stochastic Oscillator Simple Moving Average of Volume
	select @SO7_SMAVolume = case when @Seq = 1 then @SO_SMAVolume else @SO_SMAVolume * @alpha + @beta * @LastSO_SMAVolume end
	--7-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Closing
	select @SO7_VAMAClosing = case when @Seq = 1 then @SO_VAMAClosing else @SO_VAMAClosing * @alpha + @beta * @LastSO_VAMAClosing end
	--7-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Typical Closing
	select @SO7_VAMATypicalClosing = case when @Seq = 1 then @SO_VAMATypicalClosing else @SO_VAMATypicalClosing * @alpha + @beta * @LastSO_VAMATypicalClosing end
	--7-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Weighted Closing
	select @SO7_VAMAWeightedClosing = case when @Seq = 1 then @SO_VAMAWeightedClosing else @SO_VAMAWeightedClosing * @alpha + @beta * @LastSO_VAMAWeightedClosing end
	--7-Unit EA of Stochastic Oscillator Weighted Moving Average of Closing
	select @SO7_WMAClosing = case when @Seq = 1 then @SO_WMAClosing else @SO_WMAClosing * @alpha + @beta * @LastSO_WMAClosing end
	--7-Unit EA of Stochastic Oscillator Weighted Moving Average of Weighted Closing
	select @SO7_WMAWeightedClosing = case when @Seq = 1 then @SO_WMAWeightedClosing else @SO_WMAWeightedClosing * @alpha + @beta * @LastSO_WMAWeightedClosing end
	--7-Unit EA of Stochastic Oscillator Weighted Moving Average of Typical Closing
	select @SO7_WMATypicalClosing = case when @Seq = 1 then @SO_WMATypicalClosing else @SO_WMATypicalClosing * @alpha + @beta * @LastSO_WMATypicalClosing end
	--7-Unit EA of Stochastic Oscillator Exponential Moving Average of Closing
	select @SO7_EMAClosing = case when @Seq = 1 then @SO_EMAClosing else @SO_EMAClosing * @alpha + @beta * @LastSO_EMAClosing end
	--7-Unit EA of Stochastic Oscillator Exponential Moving Average of Weighted Closing
	select @SO7_EMAWeightedClosing = case when @Seq = 1 then @SO_EMAWeightedClosing else @SO_EMAWeightedClosing * @alpha + @beta * @LastSO_EMAWeightedClosing end
	--7-Unit EA of Stochastic Oscillator Exponential Moving Average of Typical Closing
	select @SO7_EMATypicalClosing = case when @Seq = 1 then @SO_EMATypicalClosing else @SO_EMATypicalClosing * @alpha + @beta * @LastSO_EMATypicalClosing end
	--7-Unit EA of Stochastic Oscillator Exponential Moving Average of Volume
	select @SO7_EMAVolume = case when @Seq = 1 then @SO_EMAVolume else @SO_EMAVolume * @alpha + @beta * @LastSO_EMAVolume end
	--7-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Closing
	select @SO7_CMOClosing = case when @Seq = 1 then @SO_CMOClosing else @SO_CMOClosing * @alpha + @beta * @LastSO_CMOClosing end
	--7-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Volume
	select @SO7_CMOVolume = case when @Seq = 1 then @SO_CMOVolume else @SO_CMOVolume * @alpha + @beta * @LastSO_CMOVolume end
	--7-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Weighted Closing
	select @SO7_CMOWeightedClosing = case when @Seq = 1 then @SO_CMOWeightedClosing else @SO_CMOWeightedClosing * @alpha + @beta * @LastSO_CMOWeightedClosing end
	--7-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Typical Closing
	select @SO7_CMOTypicalClosing = case when @Seq = 1 then @SO_CMOTypicalClosing else @SO_CMOTypicalClosing * @alpha + @beta * @LastSO_CMOTypicalClosing end
	--7-Unit EA of Stochastic Oscillator Variable Moving Average of Closing
	select @SO7_VMAClosing = case when @Seq = 1 then @SO_VMAClosing else @SO_VMAClosing * @alpha + @beta * @LastSO_VMAClosing end
	--7-Unit EA of Stochastic Oscillator Variable Moving Average of Volume
	select @SO7_VMAVolume = case when @Seq = 1 then @SO_VMAVolume else @SO_VMAVolume * @alpha + @beta * @LastSO_VMAVolume end
	--7-Unit EA of Stochastic Oscillator Variable Moving Average of Weighted Closing
	select @SO7_VMAWeightedClosing = case when @Seq = 1 then @SO_VMAWeightedClosing else @SO_VMAWeightedClosing * @alpha + @beta * @LastSO_VMAWeightedClosing end
	--7-Unit EA of Stochastic Oscillator Variable Moving Average of Typical Closing
	select @SO7_VMATypicalClosing = case when @Seq = 1 then @SO_VMATypicalClosing else @SO_VMATypicalClosing * @alpha + @beta * @LastSO_VMATypicalClosing end
	--7-Unit EA of Stochastic Oscillator Wilder's Smooth  Closing
	select @SO7_WSClosing = case when @Seq = 1 then @SO_WSClosing else @SO_WSClosing * @alpha + @beta * @LastSO_WSClosing end
	--7-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Closing
	select @SO7_WSWeightedClosing = case when @Seq = 1 then @SO_WSWeightedClosing else @SO_WSWeightedClosing * @alpha + @beta * @LastSO_WSWeightedClosing end
	--7-Unit EA of Stochastic Oscillator Wilder's Smooth  Typical Closing
	select @SO7_WSTypicalClosing = case when @Seq = 1 then @SO_WSTypicalClosing else @SO_WSTypicalClosing * @alpha + @beta * @LastSO_WSTypicalClosing end
	--7-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Closing -- used for WMA
	select @SO7_WSWeightedNormalClosing = case when @Seq = 1 then @SO_WSWeightedNormalClosing else @SO_WSWeightedNormalClosing * @alpha + @beta * @LastSO_WSWeightedNormalClosing end
	--7-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Weighted Closing -- used for WMA
	select @SO7_WSWeightedWeightedClosing = case when @Seq = 1 then @SO_WSWeightedWeightedClosing else @SO_WSWeightedWeightedClosing * @alpha + @beta * @LastSO_WSWeightedWeightedClosing end
	--7-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Typical Closing -- used for WMA
	select @SO7_WSWeightedTypicalClosing = case when @Seq = 1 then @SO_WSWeightedTypicalClosing else @SO_WSWeightedTypicalClosing * @alpha + @beta * @LastSO_WSWeightedTypicalClosing end
	--7-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume
	select @SO7_WSVolume = case when @Seq = 1 then @SO_WSVolume else @SO_WSVolume * @alpha + @beta * @LastSO_WSVolume end
	--7-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Closing -- use for VAMA
	select @SO7_WSVolumeAdjustedClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedClosing else @SO_WSVolumeAdjustedClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedClosing end
	--7-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Typical Closing -- use for VAMA
	select @SO7_WSVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedTypicalClosing else @SO_WSVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedTypicalClosing end
	--7-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Weighted Closing -- use for VAMA
	select @SO7_WSVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedWeightedClosing else @SO_WSVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedWeightedClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Closing
	select @SO7_RSI_SMAClosing = case when @Seq = 1 then @SO_RSI_SMAClosing else @SO_RSI_SMAClosing * @alpha + @beta * @LastSO_RSI_SMAClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume
	select @SO7_RSI_SMAVolume = case when @Seq = 1 then @SO_RSI_SMAVolume else @SO_RSI_SMAVolume * @alpha + @beta * @LastSO_RSI_SMAVolume end
	--7-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Weighted Closing
	select @SO7_RSI_SMAWeightedClosing = case when @Seq = 1 then @SO_RSI_SMAWeightedClosing else @SO_RSI_SMAWeightedClosing * @alpha + @beta * @LastSO_RSI_SMAWeightedClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Typical Closing
	select @SO7_RSI_SMATypicalClosing = case when @Seq = 1 then @SO_RSI_SMATypicalClosing else @SO_RSI_SMATypicalClosing * @alpha + @beta * @LastSO_RSI_SMATypicalClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Closing
	select @SO7_RSI_SMAVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedClosing else @SO_RSI_SMAVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Weighted Closing
	select @SO7_RSI_SMAVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedWeightedClosing else @SO_RSI_SMAVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedWeightedClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Typical Closing
	select @SO7_RSI_SMAVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedTypicalClosing else @SO_RSI_SMAVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedTypicalClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Closing
	select @SO7_RSI_EMAClosing = case when @Seq = 1 then @SO_RSI_EMAClosing else @SO_RSI_EMAClosing * @alpha + @beta * @LastSO_RSI_EMAClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume
	select @SO7_RSI_EMAVolume = case when @Seq = 1 then @SO_RSI_EMAVolume else @SO_RSI_EMAVolume * @alpha + @beta * @LastSO_RSI_EMAVolume end
	--7-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Weighted Closing
	select @SO7_RSI_EMAWeightedClosing = case when @Seq = 1 then @SO_RSI_EMAWeightedClosing else @SO_RSI_EMAWeightedClosing * @alpha + @beta * @LastSO_RSI_EMAWeightedClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Typical Closing
	select @SO7_RSI_EMATypicalClosing = case when @Seq = 1 then @SO_RSI_EMATypicalClosing else @SO_RSI_EMATypicalClosing * @alpha + @beta * @LastSO_RSI_EMATypicalClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Closing
	select @SO7_RSI_EMAVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedClosing else @SO_RSI_EMAVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Weighted Closing
	select @SO7_RSI_EMAVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedWeightedClosing else @SO_RSI_EMAVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedWeightedClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Typical Closing
	select @SO7_RSI_EMAVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedTypicalClosing else @SO_RSI_EMAVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedTypicalClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Closing
	select @SO7_RSI_WSClosing = case when @Seq = 1 then @SO_RSI_WSClosing else @SO_RSI_WSClosing * @alpha + @beta * @LastSO_RSI_WSClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume
	select @SO7_RSI_WSVolume = case when @Seq = 1 then @SO_RSI_WSVolume else @SO_RSI_WSVolume * @alpha + @beta * @LastSO_RSI_WSVolume end
	--7-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Weighted Closing
	select @SO7_RSI_WSWeightedClosing = case when @Seq = 1 then @SO_RSI_WSWeightedClosing else @SO_RSI_WSWeightedClosing * @alpha + @beta * @LastSO_RSI_WSWeightedClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Typical Closing
	select @SO7_RSI_WSTypicalClosing = case when @Seq = 1 then @SO_RSI_WSTypicalClosing else @SO_RSI_WSTypicalClosing * @alpha + @beta * @LastSO_RSI_WSTypicalClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Closing
	select @SO7_RSI_WSVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedClosing else @SO_RSI_WSVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Weighted Closing
	select @SO7_RSI_WSVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedWeightedClosing else @SO_RSI_WSVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedWeightedClosing end
	--7-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Typical Closing
	select @SO7_RSI_WSVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedTypicalClosing else @SO_RSI_WSVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedTypicalClosing end
	--7-Unit EA of Stochastic Oscillator Standard Deviation of Closing
	select @SO7_STDEVClosing = case when @Seq = 1 then @SO_STDEVClosing else @SO_STDEVClosing * @alpha + @beta * @LastSO_STDEVClosing end
	--7-Unit EA of Stochastic Oscillator Standard Deviation of Weighted Closing
	select @SO7_STDEVWeightedClosing = case when @Seq = 1 then @SO_STDEVWeightedClosing else @SO_STDEVWeightedClosing * @alpha + @beta * @LastSO_STDEVWeightedClosing end
	--7-Unit EA of Stochastic Oscillator Standard Deviation of Typical Closing
	select @SO7_STDEVTypicalClosing = case when @Seq = 1 then @SO_STDEVTypicalClosing else @SO_STDEVTypicalClosing * @alpha + @beta * @LastSO_STDEVTypicalClosing end
	--7-Unit EA of Stochastic Oscillator Standard Deviation of Volume
	select @SO7_STDEVVolume = case when @Seq = 1 then @SO_STDEVVolume else @SO_STDEVVolume * @alpha + @beta * @LastSO_STDEVVolume end
	--7-Unit EA of Stochastic Oscillator Standard Deviation of Closing
	select @SO7_RVIClosing = case when @Seq = 1 then @SO_RVIClosing else @SO_RVIClosing * @alpha + @beta * @LastSO_RVIClosing end
	--7-Unit EA of Stochastic Oscillator Standard Deviation of Volume
	select @SO7_RVIVolume = case when @Seq = 1 then @SO_RVIVolume else @SO_RVIVolume * @alpha + @beta * @LastSO_RVIVolume end
	--7-Unit EA of Stochastic Oscillator Standard Deviation of Weighted Closing
	select @SO7_RVIWeightedClosing = case when @Seq = 1 then @SO_RVIWeightedClosing else @SO_RVIWeightedClosing * @alpha + @beta * @LastSO_RVIWeightedClosing end
	--7-Unit EA of Stochastic Oscillator Standard Deviation of Typical Closing
	select @SO7_RVITypicalClosing = case when @Seq = 1 then @SO_RVITypicalClosing else @SO_RVITypicalClosing * @alpha + @beta * @LastSO_RVITypicalClosing end
	--7-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Closing
	select @SO7_RVIVolumeAdjustedClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedClosing else @SO_RVIVolumeAdjustedClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedClosing end
	--7-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Weighted Closing
	select @SO7_RVIVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedWeightedClosing else @SO_RVIVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedWeightedClosing end
	--7-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Typical Closing
	select @SO7_RVIVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedTypicalClosing else @SO_RVIVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedTypicalClosing end

	select @alpha = @F_Two / (cast(8 as float) + @F_One)
	select @beta = @F_One - @alpha
	--8-Unit EA of Stochastic Oscillator The opening price of the period
	select @SO8_Opening = case when @Seq = 1 then @SO_Opening else @SO_Opening * @alpha + @beta * @LastSO_Opening end
	--8-Unit EA of Stochastic Oscillator The highest price in the period
	select @SO8_High = case when @Seq = 1 then @SO_High else @SO_High * @alpha + @beta * @LastSO_High end
	--8-Unit EA of Stochastic Oscillator The lowest price in the period
	select @SO8_Low = case when @Seq = 1 then @SO_Low else @SO_Low * @alpha + @beta * @LastSO_Low end
	--8-Unit EA of Stochastic Oscillator The last price of the period
	select @SO8_Closing = case when @Seq = 1 then @SO_Closing else @SO_Closing * @alpha + @beta * @LastSO_Closing end
	--8-Unit EA of Stochastic Oscillator Total volume inthe period
	select @SO8_Volume = case when @Seq = 1 then @SO_Volume else @SO_Volume * @alpha + @beta * @LastSO_Volume end
	--8-Unit EA of Stochastic Oscillator Simple Moving Average of Closing
	select @SO8_SMAClosing = case when @Seq = 1 then @SO_SMAClosing else @SO_SMAClosing * @alpha + @beta * @LastSO_SMAClosing end
	--8-Unit EA of Stochastic Oscillator Simple Moving Average of Weighted Closing
	select @SO8_SMAWeightedClosing = case when @Seq = 1 then @SO_SMAWeightedClosing else @SO_SMAWeightedClosing * @alpha + @beta * @LastSO_SMAWeightedClosing end
	--8-Unit EA of Stochastic Oscillator Simple Moving Average of Typical Closing
	select @SO8_SMATypicalClosing = case when @Seq = 1 then @SO_SMATypicalClosing else @SO_SMATypicalClosing * @alpha + @beta * @LastSO_SMATypicalClosing end
	--8-Unit EA of Stochastic Oscillator Simple Moving Average of Volume
	select @SO8_SMAVolume = case when @Seq = 1 then @SO_SMAVolume else @SO_SMAVolume * @alpha + @beta * @LastSO_SMAVolume end
	--8-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Closing
	select @SO8_VAMAClosing = case when @Seq = 1 then @SO_VAMAClosing else @SO_VAMAClosing * @alpha + @beta * @LastSO_VAMAClosing end
	--8-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Typical Closing
	select @SO8_VAMATypicalClosing = case when @Seq = 1 then @SO_VAMATypicalClosing else @SO_VAMATypicalClosing * @alpha + @beta * @LastSO_VAMATypicalClosing end
	--8-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Weighted Closing
	select @SO8_VAMAWeightedClosing = case when @Seq = 1 then @SO_VAMAWeightedClosing else @SO_VAMAWeightedClosing * @alpha + @beta * @LastSO_VAMAWeightedClosing end
	--8-Unit EA of Stochastic Oscillator Weighted Moving Average of Closing
	select @SO8_WMAClosing = case when @Seq = 1 then @SO_WMAClosing else @SO_WMAClosing * @alpha + @beta * @LastSO_WMAClosing end
	--8-Unit EA of Stochastic Oscillator Weighted Moving Average of Weighted Closing
	select @SO8_WMAWeightedClosing = case when @Seq = 1 then @SO_WMAWeightedClosing else @SO_WMAWeightedClosing * @alpha + @beta * @LastSO_WMAWeightedClosing end
	--8-Unit EA of Stochastic Oscillator Weighted Moving Average of Typical Closing
	select @SO8_WMATypicalClosing = case when @Seq = 1 then @SO_WMATypicalClosing else @SO_WMATypicalClosing * @alpha + @beta * @LastSO_WMATypicalClosing end
	--8-Unit EA of Stochastic Oscillator Exponential Moving Average of Closing
	select @SO8_EMAClosing = case when @Seq = 1 then @SO_EMAClosing else @SO_EMAClosing * @alpha + @beta * @LastSO_EMAClosing end
	--8-Unit EA of Stochastic Oscillator Exponential Moving Average of Weighted Closing
	select @SO8_EMAWeightedClosing = case when @Seq = 1 then @SO_EMAWeightedClosing else @SO_EMAWeightedClosing * @alpha + @beta * @LastSO_EMAWeightedClosing end
	--8-Unit EA of Stochastic Oscillator Exponential Moving Average of Typical Closing
	select @SO8_EMATypicalClosing = case when @Seq = 1 then @SO_EMATypicalClosing else @SO_EMATypicalClosing * @alpha + @beta * @LastSO_EMATypicalClosing end
	--8-Unit EA of Stochastic Oscillator Exponential Moving Average of Volume
	select @SO8_EMAVolume = case when @Seq = 1 then @SO_EMAVolume else @SO_EMAVolume * @alpha + @beta * @LastSO_EMAVolume end
	--8-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Closing
	select @SO8_CMOClosing = case when @Seq = 1 then @SO_CMOClosing else @SO_CMOClosing * @alpha + @beta * @LastSO_CMOClosing end
	--8-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Volume
	select @SO8_CMOVolume = case when @Seq = 1 then @SO_CMOVolume else @SO_CMOVolume * @alpha + @beta * @LastSO_CMOVolume end
	--8-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Weighted Closing
	select @SO8_CMOWeightedClosing = case when @Seq = 1 then @SO_CMOWeightedClosing else @SO_CMOWeightedClosing * @alpha + @beta * @LastSO_CMOWeightedClosing end
	--8-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Typical Closing
	select @SO8_CMOTypicalClosing = case when @Seq = 1 then @SO_CMOTypicalClosing else @SO_CMOTypicalClosing * @alpha + @beta * @LastSO_CMOTypicalClosing end
	--8-Unit EA of Stochastic Oscillator Variable Moving Average of Closing
	select @SO8_VMAClosing = case when @Seq = 1 then @SO_VMAClosing else @SO_VMAClosing * @alpha + @beta * @LastSO_VMAClosing end
	--8-Unit EA of Stochastic Oscillator Variable Moving Average of Volume
	select @SO8_VMAVolume = case when @Seq = 1 then @SO_VMAVolume else @SO_VMAVolume * @alpha + @beta * @LastSO_VMAVolume end
	--8-Unit EA of Stochastic Oscillator Variable Moving Average of Weighted Closing
	select @SO8_VMAWeightedClosing = case when @Seq = 1 then @SO_VMAWeightedClosing else @SO_VMAWeightedClosing * @alpha + @beta * @LastSO_VMAWeightedClosing end
	--8-Unit EA of Stochastic Oscillator Variable Moving Average of Typical Closing
	select @SO8_VMATypicalClosing = case when @Seq = 1 then @SO_VMATypicalClosing else @SO_VMATypicalClosing * @alpha + @beta * @LastSO_VMATypicalClosing end
	--8-Unit EA of Stochastic Oscillator Wilder's Smooth  Closing
	select @SO8_WSClosing = case when @Seq = 1 then @SO_WSClosing else @SO_WSClosing * @alpha + @beta * @LastSO_WSClosing end
	--8-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Closing
	select @SO8_WSWeightedClosing = case when @Seq = 1 then @SO_WSWeightedClosing else @SO_WSWeightedClosing * @alpha + @beta * @LastSO_WSWeightedClosing end
	--8-Unit EA of Stochastic Oscillator Wilder's Smooth  Typical Closing
	select @SO8_WSTypicalClosing = case when @Seq = 1 then @SO_WSTypicalClosing else @SO_WSTypicalClosing * @alpha + @beta * @LastSO_WSTypicalClosing end
	--8-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Closing -- used for WMA
	select @SO8_WSWeightedNormalClosing = case when @Seq = 1 then @SO_WSWeightedNormalClosing else @SO_WSWeightedNormalClosing * @alpha + @beta * @LastSO_WSWeightedNormalClosing end
	--8-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Weighted Closing -- used for WMA
	select @SO8_WSWeightedWeightedClosing = case when @Seq = 1 then @SO_WSWeightedWeightedClosing else @SO_WSWeightedWeightedClosing * @alpha + @beta * @LastSO_WSWeightedWeightedClosing end
	--8-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Typical Closing -- used for WMA
	select @SO8_WSWeightedTypicalClosing = case when @Seq = 1 then @SO_WSWeightedTypicalClosing else @SO_WSWeightedTypicalClosing * @alpha + @beta * @LastSO_WSWeightedTypicalClosing end
	--8-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume
	select @SO8_WSVolume = case when @Seq = 1 then @SO_WSVolume else @SO_WSVolume * @alpha + @beta * @LastSO_WSVolume end
	--8-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Closing -- use for VAMA
	select @SO8_WSVolumeAdjustedClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedClosing else @SO_WSVolumeAdjustedClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedClosing end
	--8-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Typical Closing -- use for VAMA
	select @SO8_WSVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedTypicalClosing else @SO_WSVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedTypicalClosing end
	--8-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Weighted Closing -- use for VAMA
	select @SO8_WSVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedWeightedClosing else @SO_WSVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedWeightedClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Closing
	select @SO8_RSI_SMAClosing = case when @Seq = 1 then @SO_RSI_SMAClosing else @SO_RSI_SMAClosing * @alpha + @beta * @LastSO_RSI_SMAClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume
	select @SO8_RSI_SMAVolume = case when @Seq = 1 then @SO_RSI_SMAVolume else @SO_RSI_SMAVolume * @alpha + @beta * @LastSO_RSI_SMAVolume end
	--8-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Weighted Closing
	select @SO8_RSI_SMAWeightedClosing = case when @Seq = 1 then @SO_RSI_SMAWeightedClosing else @SO_RSI_SMAWeightedClosing * @alpha + @beta * @LastSO_RSI_SMAWeightedClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Typical Closing
	select @SO8_RSI_SMATypicalClosing = case when @Seq = 1 then @SO_RSI_SMATypicalClosing else @SO_RSI_SMATypicalClosing * @alpha + @beta * @LastSO_RSI_SMATypicalClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Closing
	select @SO8_RSI_SMAVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedClosing else @SO_RSI_SMAVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Weighted Closing
	select @SO8_RSI_SMAVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedWeightedClosing else @SO_RSI_SMAVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedWeightedClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Typical Closing
	select @SO8_RSI_SMAVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedTypicalClosing else @SO_RSI_SMAVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedTypicalClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Closing
	select @SO8_RSI_EMAClosing = case when @Seq = 1 then @SO_RSI_EMAClosing else @SO_RSI_EMAClosing * @alpha + @beta * @LastSO_RSI_EMAClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume
	select @SO8_RSI_EMAVolume = case when @Seq = 1 then @SO_RSI_EMAVolume else @SO_RSI_EMAVolume * @alpha + @beta * @LastSO_RSI_EMAVolume end
	--8-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Weighted Closing
	select @SO8_RSI_EMAWeightedClosing = case when @Seq = 1 then @SO_RSI_EMAWeightedClosing else @SO_RSI_EMAWeightedClosing * @alpha + @beta * @LastSO_RSI_EMAWeightedClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Typical Closing
	select @SO8_RSI_EMATypicalClosing = case when @Seq = 1 then @SO_RSI_EMATypicalClosing else @SO_RSI_EMATypicalClosing * @alpha + @beta * @LastSO_RSI_EMATypicalClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Closing
	select @SO8_RSI_EMAVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedClosing else @SO_RSI_EMAVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Weighted Closing
	select @SO8_RSI_EMAVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedWeightedClosing else @SO_RSI_EMAVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedWeightedClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Typical Closing
	select @SO8_RSI_EMAVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedTypicalClosing else @SO_RSI_EMAVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedTypicalClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Closing
	select @SO8_RSI_WSClosing = case when @Seq = 1 then @SO_RSI_WSClosing else @SO_RSI_WSClosing * @alpha + @beta * @LastSO_RSI_WSClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume
	select @SO8_RSI_WSVolume = case when @Seq = 1 then @SO_RSI_WSVolume else @SO_RSI_WSVolume * @alpha + @beta * @LastSO_RSI_WSVolume end
	--8-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Weighted Closing
	select @SO8_RSI_WSWeightedClosing = case when @Seq = 1 then @SO_RSI_WSWeightedClosing else @SO_RSI_WSWeightedClosing * @alpha + @beta * @LastSO_RSI_WSWeightedClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Typical Closing
	select @SO8_RSI_WSTypicalClosing = case when @Seq = 1 then @SO_RSI_WSTypicalClosing else @SO_RSI_WSTypicalClosing * @alpha + @beta * @LastSO_RSI_WSTypicalClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Closing
	select @SO8_RSI_WSVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedClosing else @SO_RSI_WSVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Weighted Closing
	select @SO8_RSI_WSVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedWeightedClosing else @SO_RSI_WSVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedWeightedClosing end
	--8-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Typical Closing
	select @SO8_RSI_WSVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedTypicalClosing else @SO_RSI_WSVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedTypicalClosing end
	--8-Unit EA of Stochastic Oscillator Standard Deviation of Closing
	select @SO8_STDEVClosing = case when @Seq = 1 then @SO_STDEVClosing else @SO_STDEVClosing * @alpha + @beta * @LastSO_STDEVClosing end
	--8-Unit EA of Stochastic Oscillator Standard Deviation of Weighted Closing
	select @SO8_STDEVWeightedClosing = case when @Seq = 1 then @SO_STDEVWeightedClosing else @SO_STDEVWeightedClosing * @alpha + @beta * @LastSO_STDEVWeightedClosing end
	--8-Unit EA of Stochastic Oscillator Standard Deviation of Typical Closing
	select @SO8_STDEVTypicalClosing = case when @Seq = 1 then @SO_STDEVTypicalClosing else @SO_STDEVTypicalClosing * @alpha + @beta * @LastSO_STDEVTypicalClosing end
	--8-Unit EA of Stochastic Oscillator Standard Deviation of Volume
	select @SO8_STDEVVolume = case when @Seq = 1 then @SO_STDEVVolume else @SO_STDEVVolume * @alpha + @beta * @LastSO_STDEVVolume end
	--8-Unit EA of Stochastic Oscillator Standard Deviation of Closing
	select @SO8_RVIClosing = case when @Seq = 1 then @SO_RVIClosing else @SO_RVIClosing * @alpha + @beta * @LastSO_RVIClosing end
	--8-Unit EA of Stochastic Oscillator Standard Deviation of Volume
	select @SO8_RVIVolume = case when @Seq = 1 then @SO_RVIVolume else @SO_RVIVolume * @alpha + @beta * @LastSO_RVIVolume end
	--8-Unit EA of Stochastic Oscillator Standard Deviation of Weighted Closing
	select @SO8_RVIWeightedClosing = case when @Seq = 1 then @SO_RVIWeightedClosing else @SO_RVIWeightedClosing * @alpha + @beta * @LastSO_RVIWeightedClosing end
	--8-Unit EA of Stochastic Oscillator Standard Deviation of Typical Closing
	select @SO8_RVITypicalClosing = case when @Seq = 1 then @SO_RVITypicalClosing else @SO_RVITypicalClosing * @alpha + @beta * @LastSO_RVITypicalClosing end
	--8-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Closing
	select @SO8_RVIVolumeAdjustedClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedClosing else @SO_RVIVolumeAdjustedClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedClosing end
	--8-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Weighted Closing
	select @SO8_RVIVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedWeightedClosing else @SO_RVIVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedWeightedClosing end
	--8-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Typical Closing
	select @SO8_RVIVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedTypicalClosing else @SO_RVIVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedTypicalClosing end

	select @alpha = @F_Two / (cast(9 as float) + @F_One)
	select @beta = @F_One - @alpha
	--9-Unit EA of Stochastic Oscillator The opening price of the period
	select @SO9_Opening = case when @Seq = 1 then @SO_Opening else @SO_Opening * @alpha + @beta * @LastSO_Opening end
	--9-Unit EA of Stochastic Oscillator The highest price in the period
	select @SO9_High = case when @Seq = 1 then @SO_High else @SO_High * @alpha + @beta * @LastSO_High end
	--9-Unit EA of Stochastic Oscillator The lowest price in the period
	select @SO9_Low = case when @Seq = 1 then @SO_Low else @SO_Low * @alpha + @beta * @LastSO_Low end
	--9-Unit EA of Stochastic Oscillator The last price of the period
	select @SO9_Closing = case when @Seq = 1 then @SO_Closing else @SO_Closing * @alpha + @beta * @LastSO_Closing end
	--9-Unit EA of Stochastic Oscillator Total volume inthe period
	select @SO9_Volume = case when @Seq = 1 then @SO_Volume else @SO_Volume * @alpha + @beta * @LastSO_Volume end
	--9-Unit EA of Stochastic Oscillator Simple Moving Average of Closing
	select @SO9_SMAClosing = case when @Seq = 1 then @SO_SMAClosing else @SO_SMAClosing * @alpha + @beta * @LastSO_SMAClosing end
	--9-Unit EA of Stochastic Oscillator Simple Moving Average of Weighted Closing
	select @SO9_SMAWeightedClosing = case when @Seq = 1 then @SO_SMAWeightedClosing else @SO_SMAWeightedClosing * @alpha + @beta * @LastSO_SMAWeightedClosing end
	--9-Unit EA of Stochastic Oscillator Simple Moving Average of Typical Closing
	select @SO9_SMATypicalClosing = case when @Seq = 1 then @SO_SMATypicalClosing else @SO_SMATypicalClosing * @alpha + @beta * @LastSO_SMATypicalClosing end
	--9-Unit EA of Stochastic Oscillator Simple Moving Average of Volume
	select @SO9_SMAVolume = case when @Seq = 1 then @SO_SMAVolume else @SO_SMAVolume * @alpha + @beta * @LastSO_SMAVolume end
	--9-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Closing
	select @SO9_VAMAClosing = case when @Seq = 1 then @SO_VAMAClosing else @SO_VAMAClosing * @alpha + @beta * @LastSO_VAMAClosing end
	--9-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Typical Closing
	select @SO9_VAMATypicalClosing = case when @Seq = 1 then @SO_VAMATypicalClosing else @SO_VAMATypicalClosing * @alpha + @beta * @LastSO_VAMATypicalClosing end
	--9-Unit EA of Stochastic Oscillator Volume Adjusted Moving Average of Weighted Closing
	select @SO9_VAMAWeightedClosing = case when @Seq = 1 then @SO_VAMAWeightedClosing else @SO_VAMAWeightedClosing * @alpha + @beta * @LastSO_VAMAWeightedClosing end
	--9-Unit EA of Stochastic Oscillator Weighted Moving Average of Closing
	select @SO9_WMAClosing = case when @Seq = 1 then @SO_WMAClosing else @SO_WMAClosing * @alpha + @beta * @LastSO_WMAClosing end
	--9-Unit EA of Stochastic Oscillator Weighted Moving Average of Weighted Closing
	select @SO9_WMAWeightedClosing = case when @Seq = 1 then @SO_WMAWeightedClosing else @SO_WMAWeightedClosing * @alpha + @beta * @LastSO_WMAWeightedClosing end
	--9-Unit EA of Stochastic Oscillator Weighted Moving Average of Typical Closing
	select @SO9_WMATypicalClosing = case when @Seq = 1 then @SO_WMATypicalClosing else @SO_WMATypicalClosing * @alpha + @beta * @LastSO_WMATypicalClosing end
	--9-Unit EA of Stochastic Oscillator Exponential Moving Average of Closing
	select @SO9_EMAClosing = case when @Seq = 1 then @SO_EMAClosing else @SO_EMAClosing * @alpha + @beta * @LastSO_EMAClosing end
	--9-Unit EA of Stochastic Oscillator Exponential Moving Average of Weighted Closing
	select @SO9_EMAWeightedClosing = case when @Seq = 1 then @SO_EMAWeightedClosing else @SO_EMAWeightedClosing * @alpha + @beta * @LastSO_EMAWeightedClosing end
	--9-Unit EA of Stochastic Oscillator Exponential Moving Average of Typical Closing
	select @SO9_EMATypicalClosing = case when @Seq = 1 then @SO_EMATypicalClosing else @SO_EMATypicalClosing * @alpha + @beta * @LastSO_EMATypicalClosing end
	--9-Unit EA of Stochastic Oscillator Exponential Moving Average of Volume
	select @SO9_EMAVolume = case when @Seq = 1 then @SO_EMAVolume else @SO_EMAVolume * @alpha + @beta * @LastSO_EMAVolume end
	--9-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Closing
	select @SO9_CMOClosing = case when @Seq = 1 then @SO_CMOClosing else @SO_CMOClosing * @alpha + @beta * @LastSO_CMOClosing end
	--9-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Volume
	select @SO9_CMOVolume = case when @Seq = 1 then @SO_CMOVolume else @SO_CMOVolume * @alpha + @beta * @LastSO_CMOVolume end
	--9-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Weighted Closing
	select @SO9_CMOWeightedClosing = case when @Seq = 1 then @SO_CMOWeightedClosing else @SO_CMOWeightedClosing * @alpha + @beta * @LastSO_CMOWeightedClosing end
	--9-Unit EA of Stochastic Oscillator Chande Momentum Oscillator of Typical Closing
	select @SO9_CMOTypicalClosing = case when @Seq = 1 then @SO_CMOTypicalClosing else @SO_CMOTypicalClosing * @alpha + @beta * @LastSO_CMOTypicalClosing end
	--9-Unit EA of Stochastic Oscillator Variable Moving Average of Closing
	select @SO9_VMAClosing = case when @Seq = 1 then @SO_VMAClosing else @SO_VMAClosing * @alpha + @beta * @LastSO_VMAClosing end
	--9-Unit EA of Stochastic Oscillator Variable Moving Average of Volume
	select @SO9_VMAVolume = case when @Seq = 1 then @SO_VMAVolume else @SO_VMAVolume * @alpha + @beta * @LastSO_VMAVolume end
	--9-Unit EA of Stochastic Oscillator Variable Moving Average of Weighted Closing
	select @SO9_VMAWeightedClosing = case when @Seq = 1 then @SO_VMAWeightedClosing else @SO_VMAWeightedClosing * @alpha + @beta * @LastSO_VMAWeightedClosing end
	--9-Unit EA of Stochastic Oscillator Variable Moving Average of Typical Closing
	select @SO9_VMATypicalClosing = case when @Seq = 1 then @SO_VMATypicalClosing else @SO_VMATypicalClosing * @alpha + @beta * @LastSO_VMATypicalClosing end
	--9-Unit EA of Stochastic Oscillator Wilder's Smooth  Closing
	select @SO9_WSClosing = case when @Seq = 1 then @SO_WSClosing else @SO_WSClosing * @alpha + @beta * @LastSO_WSClosing end
	--9-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Closing
	select @SO9_WSWeightedClosing = case when @Seq = 1 then @SO_WSWeightedClosing else @SO_WSWeightedClosing * @alpha + @beta * @LastSO_WSWeightedClosing end
	--9-Unit EA of Stochastic Oscillator Wilder's Smooth  Typical Closing
	select @SO9_WSTypicalClosing = case when @Seq = 1 then @SO_WSTypicalClosing else @SO_WSTypicalClosing * @alpha + @beta * @LastSO_WSTypicalClosing end
	--9-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Closing -- used for WMA
	select @SO9_WSWeightedNormalClosing = case when @Seq = 1 then @SO_WSWeightedNormalClosing else @SO_WSWeightedNormalClosing * @alpha + @beta * @LastSO_WSWeightedNormalClosing end
	--9-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Weighted Closing -- used for WMA
	select @SO9_WSWeightedWeightedClosing = case when @Seq = 1 then @SO_WSWeightedWeightedClosing else @SO_WSWeightedWeightedClosing * @alpha + @beta * @LastSO_WSWeightedWeightedClosing end
	--9-Unit EA of Stochastic Oscillator Wilder's Smooth  Weighted Typical Closing -- used for WMA
	select @SO9_WSWeightedTypicalClosing = case when @Seq = 1 then @SO_WSWeightedTypicalClosing else @SO_WSWeightedTypicalClosing * @alpha + @beta * @LastSO_WSWeightedTypicalClosing end
	--9-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume
	select @SO9_WSVolume = case when @Seq = 1 then @SO_WSVolume else @SO_WSVolume * @alpha + @beta * @LastSO_WSVolume end
	--9-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Closing -- use for VAMA
	select @SO9_WSVolumeAdjustedClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedClosing else @SO_WSVolumeAdjustedClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedClosing end
	--9-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Typical Closing -- use for VAMA
	select @SO9_WSVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedTypicalClosing else @SO_WSVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedTypicalClosing end
	--9-Unit EA of Stochastic Oscillator Wilder's Smooth  Volume Adjusted Weighted Closing -- use for VAMA
	select @SO9_WSVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_WSVolumeAdjustedWeightedClosing else @SO_WSVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_WSVolumeAdjustedWeightedClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Closing
	select @SO9_RSI_SMAClosing = case when @Seq = 1 then @SO_RSI_SMAClosing else @SO_RSI_SMAClosing * @alpha + @beta * @LastSO_RSI_SMAClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume
	select @SO9_RSI_SMAVolume = case when @Seq = 1 then @SO_RSI_SMAVolume else @SO_RSI_SMAVolume * @alpha + @beta * @LastSO_RSI_SMAVolume end
	--9-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Weighted Closing
	select @SO9_RSI_SMAWeightedClosing = case when @Seq = 1 then @SO_RSI_SMAWeightedClosing else @SO_RSI_SMAWeightedClosing * @alpha + @beta * @LastSO_RSI_SMAWeightedClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Typical Closing
	select @SO9_RSI_SMATypicalClosing = case when @Seq = 1 then @SO_RSI_SMATypicalClosing else @SO_RSI_SMATypicalClosing * @alpha + @beta * @LastSO_RSI_SMATypicalClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Closing
	select @SO9_RSI_SMAVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedClosing else @SO_RSI_SMAVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Weighted Closing
	select @SO9_RSI_SMAVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedWeightedClosing else @SO_RSI_SMAVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedWeightedClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Simple Moving Average of Volume Adjusted Typical Closing
	select @SO9_RSI_SMAVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_SMAVolumeAdjustedTypicalClosing else @SO_RSI_SMAVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_SMAVolumeAdjustedTypicalClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Closing
	select @SO9_RSI_EMAClosing = case when @Seq = 1 then @SO_RSI_EMAClosing else @SO_RSI_EMAClosing * @alpha + @beta * @LastSO_RSI_EMAClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume
	select @SO9_RSI_EMAVolume = case when @Seq = 1 then @SO_RSI_EMAVolume else @SO_RSI_EMAVolume * @alpha + @beta * @LastSO_RSI_EMAVolume end
	--9-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Weighted Closing
	select @SO9_RSI_EMAWeightedClosing = case when @Seq = 1 then @SO_RSI_EMAWeightedClosing else @SO_RSI_EMAWeightedClosing * @alpha + @beta * @LastSO_RSI_EMAWeightedClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Typical Closing
	select @SO9_RSI_EMATypicalClosing = case when @Seq = 1 then @SO_RSI_EMATypicalClosing else @SO_RSI_EMATypicalClosing * @alpha + @beta * @LastSO_RSI_EMATypicalClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Closing
	select @SO9_RSI_EMAVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedClosing else @SO_RSI_EMAVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Weighted Closing
	select @SO9_RSI_EMAVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedWeightedClosing else @SO_RSI_EMAVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedWeightedClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Exponential Moving Average of Volume Adjusted Typical Closing
	select @SO9_RSI_EMAVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_EMAVolumeAdjustedTypicalClosing else @SO_RSI_EMAVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_EMAVolumeAdjustedTypicalClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Closing
	select @SO9_RSI_WSClosing = case when @Seq = 1 then @SO_RSI_WSClosing else @SO_RSI_WSClosing * @alpha + @beta * @LastSO_RSI_WSClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume
	select @SO9_RSI_WSVolume = case when @Seq = 1 then @SO_RSI_WSVolume else @SO_RSI_WSVolume * @alpha + @beta * @LastSO_RSI_WSVolume end
	--9-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Weighted Closing
	select @SO9_RSI_WSWeightedClosing = case when @Seq = 1 then @SO_RSI_WSWeightedClosing else @SO_RSI_WSWeightedClosing * @alpha + @beta * @LastSO_RSI_WSWeightedClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Typical Closing
	select @SO9_RSI_WSTypicalClosing = case when @Seq = 1 then @SO_RSI_WSTypicalClosing else @SO_RSI_WSTypicalClosing * @alpha + @beta * @LastSO_RSI_WSTypicalClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Closing
	select @SO9_RSI_WSVolumeAdjustedClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedClosing else @SO_RSI_WSVolumeAdjustedClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Weighted Closing
	select @SO9_RSI_WSVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedWeightedClosing else @SO_RSI_WSVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedWeightedClosing end
	--9-Unit EA of Stochastic Oscillator RSI - Wilder's Smooth of Volume Adjusted Typical Closing
	select @SO9_RSI_WSVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RSI_WSVolumeAdjustedTypicalClosing else @SO_RSI_WSVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RSI_WSVolumeAdjustedTypicalClosing end
	--9-Unit EA of Stochastic Oscillator Standard Deviation of Closing
	select @SO9_STDEVClosing = case when @Seq = 1 then @SO_STDEVClosing else @SO_STDEVClosing * @alpha + @beta * @LastSO_STDEVClosing end
	--9-Unit EA of Stochastic Oscillator Standard Deviation of Weighted Closing
	select @SO9_STDEVWeightedClosing = case when @Seq = 1 then @SO_STDEVWeightedClosing else @SO_STDEVWeightedClosing * @alpha + @beta * @LastSO_STDEVWeightedClosing end
	--9-Unit EA of Stochastic Oscillator Standard Deviation of Typical Closing
	select @SO9_STDEVTypicalClosing = case when @Seq = 1 then @SO_STDEVTypicalClosing else @SO_STDEVTypicalClosing * @alpha + @beta * @LastSO_STDEVTypicalClosing end
	--9-Unit EA of Stochastic Oscillator Standard Deviation of Volume
	select @SO9_STDEVVolume = case when @Seq = 1 then @SO_STDEVVolume else @SO_STDEVVolume * @alpha + @beta * @LastSO_STDEVVolume end
	--9-Unit EA of Stochastic Oscillator Standard Deviation of Closing
	select @SO9_RVIClosing = case when @Seq = 1 then @SO_RVIClosing else @SO_RVIClosing * @alpha + @beta * @LastSO_RVIClosing end
	--9-Unit EA of Stochastic Oscillator Standard Deviation of Volume
	select @SO9_RVIVolume = case when @Seq = 1 then @SO_RVIVolume else @SO_RVIVolume * @alpha + @beta * @LastSO_RVIVolume end
	--9-Unit EA of Stochastic Oscillator Standard Deviation of Weighted Closing
	select @SO9_RVIWeightedClosing = case when @Seq = 1 then @SO_RVIWeightedClosing else @SO_RVIWeightedClosing * @alpha + @beta * @LastSO_RVIWeightedClosing end
	--9-Unit EA of Stochastic Oscillator Standard Deviation of Typical Closing
	select @SO9_RVITypicalClosing = case when @Seq = 1 then @SO_RVITypicalClosing else @SO_RVITypicalClosing * @alpha + @beta * @LastSO_RVITypicalClosing end
	--9-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Closing
	select @SO9_RVIVolumeAdjustedClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedClosing else @SO_RVIVolumeAdjustedClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedClosing end
	--9-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Weighted Closing
	select @SO9_RVIVolumeAdjustedWeightedClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedWeightedClosing else @SO_RVIVolumeAdjustedWeightedClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedWeightedClosing end
	--9-Unit EA of Stochastic Oscillator Standard Deviation of Volume Adjusted Typical Closing
	select @SO9_RVIVolumeAdjustedTypicalClosing = case when @Seq = 1 then @SO_RVIVolumeAdjustedTypicalClosing else @SO_RVIVolumeAdjustedTypicalClosing * @alpha + @beta * @LastSO_RVIVolumeAdjustedTypicalClosing end


	insert into DIM.Fact_Base(
								SymbolID, Seq, PeriodID,
								DateFrom, DateTo, Opening, High, Low, Closing, Volume, 
								TotalClosing, TotalWeightedClosing, TotalTypicalClosing, TotalWeightedNormalClosing, TotalWeightedWeightedClosing, TotalWeightedTypicalClosing, TotalVolume, TotalVolumeAdjustedClosing, TotalVolumeAdjustedTypicalClosing, TotalVolumeAdjustedWeightedClosing, TotalGainClosing, TotalLossClosing, TotalGainVolume, TotalLossVolume, TotalGainWeightedClosing, TotalLossWeightedClosing, TotalGainTypicalClosing, TotalLossTypicalClosing, TotalGainVolumeAdjustedClosing, TotalLossVolumeAdjustedClosing, TotalGainVolumeAdjustedWeightedClosing, TotalLossVolumeAdjustedWeightedClosing, TotalGainVolumeAdjustedTypicalClosing, TotalLossVolumeAdjustedTypicalClosing, 
								SMAClosing, SMAWeightedClosing, SMATypicalClosing, SMAVolume, SMAGainClosing, SMALossClosing, SMAGainVolume, SMALossVolume, SMAGainWeightedClosing, SMALossWeightedClosing, SMAGainTypicalClosing, SMALossTypicalClosing, SMAGainVolumeAdjustedClosing, SMALossVolumeAdjustedClosing, SMAGainVolumeAdjustedWeightedClosing, SMALossVolumeAdjustedWeightedClosing, SMAGainVolumeAdjustedTypicalClosing, SMALossVolumeAdjustedTypicalClosing, 
								VAMAClosing, VAMATypicalClosing, VAMAWeightedClosing, 
								WMAClosing, WMAWeightedClosing, WMATypicalClosing, 
								EMAClosing, EMAWeightedClosing, EMATypicalClosing, EMAVolume, EMAGainClosing, EMALossClosing, EMAGainVolume, EMALossVolume, EMAGainWeightedClosing, EMALossWeightedClosing, EMAGainTypicalClosing, EMALossTypicalClosing, EMAGainVolumeAdjustedClosing, EMALossVolumeAdjustedClosing, EMAGainVolumeAdjustedWeightedClosing, EMALossVolumeAdjustedWeightedClosing, EMAGainVolumeAdjustedTypicalClosing, EMALossVolumeAdjustedTypicalClosing, 
								CMOClosing, CMOVolume, CMOWeightedClosing, CMOTypicalClosing, 
								VMAClosing, VMAVolume, VMAWeightedClosing, VMATypicalClosing, 
								WSClosing, WSWeightedClosing, WSTypicalClosing, WSWeightedNormalClosing, WSWeightedWeightedClosing, WSWeightedTypicalClosing, WSVolume, WSVolumeAdjustedClosing, WSVolumeAdjustedTypicalClosing, WSVolumeAdjustedWeightedClosing, WSGainClosing, WSLossClosing, WSGainVolume, WSLossVolume, WSGainWeightedClosing, WSLossWeightedClosing, WSGainTypicalClosing, WSLossTypicalClosing, WSGainVolumeAdjustedClosing, WSLossVolumeAdjustedClosing, WSGainVolumeAdjustedWeightedClosing, WSLossVolumeAdjustedWeightedClosing, WSGainVolumeAdjustedTypicalClosing, WSLossVolumeAdjustedTypicalClosing, 
								RSI_SMAClosing, RSI_SMAVolume, RSI_SMAWeightedClosing, RSI_SMATypicalClosing, RSI_SMAVolumeAdjustedClosing, RSI_SMAVolumeAdjustedWeightedClosing, RSI_SMAVolumeAdjustedTypicalClosing, RSI_EMAClosing, RSI_EMAVolume, RSI_EMAWeightedClosing, RSI_EMATypicalClosing, RSI_EMAVolumeAdjustedClosing, RSI_EMAVolumeAdjustedWeightedClosing, RSI_EMAVolumeAdjustedTypicalClosing, RSI_WSClosing, RSI_WSVolume, RSI_WSWeightedClosing, RSI_WSTypicalClosing, RSI_WSVolumeAdjustedClosing, RSI_WSVolumeAdjustedWeightedClosing, RSI_WSVolumeAdjustedTypicalClosing, 
								STDEVClosing, STDEVWeightedClosing, STDEVTypicalClosing, STDEVVolume, STDEVGainClosing, STDEVLossClosing, STDEVGainVolume, STDEVLossVolume, STDEVGainWeightedClosing, STDEVLossWeightedClosing, STDEVGainTypicalClosing, STDEVLossTypicalClosing, STDEVGainVolumeAdjustedClosing, STDEVLossVolumeAdjustedClosing, STDEVGainVolumeAdjustedWeightedClosing, STDEVLossVolumeAdjustedWeightedClosing, STDEVGainVolumeAdjustedTypicalClosing, STDEVLossVolumeAdjustedTypicalClosing, 
								RVIClosing, RVIVolume, RVIWeightedClosing, RVITypicalClosing, RVIVolumeAdjustedClosing, RVIVolumeAdjustedWeightedClosing, RVIVolumeAdjustedTypicalClosing,
								HighestOpening, LowestOpening, HighestHigh, LowestHigh, HighestLow, LowestLow, HighestClosing, LowestClosing, HighestVolume, LowestVolume, HighestSMAClosing, LowestSMAClosing, HighestSMAWeightedClosing, LowestSMAWeightedClosing, HighestSMATypicalClosing, LowestSMATypicalClosing, HighestSMAVolume, LowestSMAVolume, HighestVAMAClosing, LowestVAMAClosing, HighestVAMATypicalClosing, LowestVAMATypicalClosing, HighestVAMAWeightedClosing, LowestVAMAWeightedClosing, HighestWMAClosing, LowestWMAClosing, HighestWMAWeightedClosing, LowestWMAWeightedClosing, HighestWMATypicalClosing, LowestWMATypicalClosing, HighestEMAClosing, LowestEMAClosing, HighestEMAWeightedClosing, LowestEMAWeightedClosing, HighestEMATypicalClosing, LowestEMATypicalClosing, HighestEMAVolume, LowestEMAVolume, HighestCMOClosing, LowestCMOClosing, HighestCMOVolume, LowestCMOVolume, HighestCMOWeightedClosing, LowestCMOWeightedClosing, HighestCMOTypicalClosing, LowestCMOTypicalClosing, HighestVMAClosing, LowestVMAClosing, HighestVMAVolume, LowestVMAVolume, HighestVMAWeightedClosing, LowestVMAWeightedClosing, HighestVMATypicalClosing, LowestVMATypicalClosing, HighestWSClosing, LowestWSClosing, HighestWSWeightedClosing, LowestWSWeightedClosing, HighestWSTypicalClosing, LowestWSTypicalClosing, HighestWSWeightedNormalClosing, LowestWSWeightedNormalClosing, HighestWSWeightedWeightedClosing, LowestWSWeightedWeightedClosing, HighestWSWeightedTypicalClosing, LowestWSWeightedTypicalClosing, HighestWSVolume, LowestWSVolume, HighestWSVolumeAdjustedClosing, LowestWSVolumeAdjustedClosing, HighestWSVolumeAdjustedTypicalClosing, LowestWSVolumeAdjustedTypicalClosing, HighestWSVolumeAdjustedWeightedClosing, LowestWSVolumeAdjustedWeightedClosing, HighestRSI_SMAClosing, LowestRSI_SMAClosing, HighestRSI_SMAVolume, LowestRSI_SMAVolume, HighestRSI_SMAWeightedClosing, LowestRSI_SMAWeightedClosing, HighestRSI_SMATypicalClosing, LowestRSI_SMATypicalClosing, HighestRSI_SMAVolumeAdjustedClosing, LowestRSI_SMAVolumeAdjustedClosing, HighestRSI_SMAVolumeAdjustedWeightedClosing, LowestRSI_SMAVolumeAdjustedWeightedClosing, HighestRSI_SMAVolumeAdjustedTypicalClosing, LowestRSI_SMAVolumeAdjustedTypicalClosing, HighestRSI_EMAClosing, LowestRSI_EMAClosing, HighestRSI_EMAVolume, LowestRSI_EMAVolume, HighestRSI_EMAWeightedClosing, LowestRSI_EMAWeightedClosing, HighestRSI_EMATypicalClosing, LowestRSI_EMATypicalClosing, HighestRSI_EMAVolumeAdjustedClosing, LowestRSI_EMAVolumeAdjustedClosing, HighestRSI_EMAVolumeAdjustedWeightedClosing, LowestRSI_EMAVolumeAdjustedWeightedClosing, HighestRSI_EMAVolumeAdjustedTypicalClosing, LowestRSI_EMAVolumeAdjustedTypicalClosing, HighestRSI_WSClosing, LowestRSI_WSClosing, HighestRSI_WSVolume, LowestRSI_WSVolume, HighestRSI_WSWeightedClosing, LowestRSI_WSWeightedClosing, HighestRSI_WSTypicalClosing, LowestRSI_WSTypicalClosing, HighestRSI_WSVolumeAdjustedClosing, LowestRSI_WSVolumeAdjustedClosing, HighestRSI_WSVolumeAdjustedWeightedClosing, LowestRSI_WSVolumeAdjustedWeightedClosing, HighestRSI_WSVolumeAdjustedTypicalClosing, LowestRSI_WSVolumeAdjustedTypicalClosing, HighestSTDEVClosing, LowestSTDEVClosing, HighestSTDEVWeightedClosing, LowestSTDEVWeightedClosing, HighestSTDEVTypicalClosing, LowestSTDEVTypicalClosing, HighestSTDEVVolume, LowestSTDEVVolume, HighestRVIClosing, LowestRVIClosing, HighestRVIVolume, LowestRVIVolume, HighestRVIWeightedClosing, LowestRVIWeightedClosing, HighestRVITypicalClosing, LowestRVITypicalClosing, HighestRVIVolumeAdjustedClosing, LowestRVIVolumeAdjustedClosing, HighestRVIVolumeAdjustedWeightedClosing, LowestRVIVolumeAdjustedWeightedClosing, HighestRVIVolumeAdjustedTypicalClosing, LowestRVIVolumeAdjustedTypicalClosing, 
								SO_Opening, SO_High, SO_Low, SO_Closing, SO_Volume, SO_SMAClosing, SO_SMAWeightedClosing, SO_SMATypicalClosing, SO_SMAVolume, SO_VAMAClosing, SO_VAMATypicalClosing, SO_VAMAWeightedClosing, SO_WMAClosing, SO_WMAWeightedClosing, SO_WMATypicalClosing, SO_EMAClosing, SO_EMAWeightedClosing, SO_EMATypicalClosing, SO_EMAVolume, SO_CMOClosing, SO_CMOVolume, SO_CMOWeightedClosing, SO_CMOTypicalClosing, SO_VMAClosing, SO_VMAVolume, SO_VMAWeightedClosing, SO_VMATypicalClosing, SO_WSClosing, SO_WSWeightedClosing, SO_WSTypicalClosing, SO_WSWeightedNormalClosing, SO_WSWeightedWeightedClosing, SO_WSWeightedTypicalClosing, SO_WSVolume, SO_WSVolumeAdjustedClosing, SO_WSVolumeAdjustedTypicalClosing, SO_WSVolumeAdjustedWeightedClosing, SO_RSI_SMAClosing, SO_RSI_SMAVolume, SO_RSI_SMAWeightedClosing, SO_RSI_SMATypicalClosing, SO_RSI_SMAVolumeAdjustedClosing, SO_RSI_SMAVolumeAdjustedWeightedClosing, SO_RSI_SMAVolumeAdjustedTypicalClosing, SO_RSI_EMAClosing, SO_RSI_EMAVolume, SO_RSI_EMAWeightedClosing, SO_RSI_EMATypicalClosing, SO_RSI_EMAVolumeAdjustedClosing, SO_RSI_EMAVolumeAdjustedWeightedClosing, SO_RSI_EMAVolumeAdjustedTypicalClosing, SO_RSI_WSClosing, SO_RSI_WSVolume, SO_RSI_WSWeightedClosing, SO_RSI_WSTypicalClosing, SO_RSI_WSVolumeAdjustedClosing, SO_RSI_WSVolumeAdjustedWeightedClosing, SO_RSI_WSVolumeAdjustedTypicalClosing, SO_STDEVClosing, SO_STDEVWeightedClosing, SO_STDEVTypicalClosing, SO_STDEVVolume, SO_RVIClosing, SO_RVIVolume, SO_RVIWeightedClosing, SO_RVITypicalClosing, SO_RVIVolumeAdjustedClosing, SO_RVIVolumeAdjustedWeightedClosing, SO_RVIVolumeAdjustedTypicalClosing, 
								SO3_Opening, SO3_High, SO3_Low, SO3_Closing, SO3_Volume, SO3_SMAClosing, SO3_SMAWeightedClosing, SO3_SMATypicalClosing, SO3_SMAVolume, SO3_VAMAClosing, SO3_VAMATypicalClosing, SO3_VAMAWeightedClosing, SO3_WMAClosing, SO3_WMAWeightedClosing, SO3_WMATypicalClosing, SO3_EMAClosing, SO3_EMAWeightedClosing, SO3_EMATypicalClosing, SO3_EMAVolume, SO3_CMOClosing, SO3_CMOVolume, SO3_CMOWeightedClosing, SO3_CMOTypicalClosing, SO3_VMAClosing, SO3_VMAVolume, SO3_VMAWeightedClosing, SO3_VMATypicalClosing, SO3_WSClosing, SO3_WSWeightedClosing, SO3_WSTypicalClosing, SO3_WSWeightedNormalClosing, SO3_WSWeightedWeightedClosing, SO3_WSWeightedTypicalClosing, SO3_WSVolume, SO3_WSVolumeAdjustedClosing, SO3_WSVolumeAdjustedTypicalClosing, SO3_WSVolumeAdjustedWeightedClosing, SO3_RSI_SMAClosing, SO3_RSI_SMAVolume, SO3_RSI_SMAWeightedClosing, SO3_RSI_SMATypicalClosing, SO3_RSI_SMAVolumeAdjustedClosing, SO3_RSI_SMAVolumeAdjustedWeightedClosing, SO3_RSI_SMAVolumeAdjustedTypicalClosing, SO3_RSI_EMAClosing, SO3_RSI_EMAVolume, SO3_RSI_EMAWeightedClosing, SO3_RSI_EMATypicalClosing, SO3_RSI_EMAVolumeAdjustedClosing, SO3_RSI_EMAVolumeAdjustedWeightedClosing, SO3_RSI_EMAVolumeAdjustedTypicalClosing, SO3_RSI_WSClosing, SO3_RSI_WSVolume, SO3_RSI_WSWeightedClosing, SO3_RSI_WSTypicalClosing, SO3_RSI_WSVolumeAdjustedClosing, SO3_RSI_WSVolumeAdjustedWeightedClosing, SO3_RSI_WSVolumeAdjustedTypicalClosing, SO3_STDEVClosing, SO3_STDEVWeightedClosing, SO3_STDEVTypicalClosing, SO3_STDEVVolume, SO3_RVIClosing, SO3_RVIVolume, SO3_RVIWeightedClosing, SO3_RVITypicalClosing, SO3_RVIVolumeAdjustedClosing, SO3_RVIVolumeAdjustedWeightedClosing, SO3_RVIVolumeAdjustedTypicalClosing, 
								SO4_Opening, SO4_High, SO4_Low, SO4_Closing, SO4_Volume, SO4_SMAClosing, SO4_SMAWeightedClosing, SO4_SMATypicalClosing, SO4_SMAVolume, SO4_VAMAClosing, SO4_VAMATypicalClosing, SO4_VAMAWeightedClosing, SO4_WMAClosing, SO4_WMAWeightedClosing, SO4_WMATypicalClosing, SO4_EMAClosing, SO4_EMAWeightedClosing, SO4_EMATypicalClosing, SO4_EMAVolume, SO4_CMOClosing, SO4_CMOVolume, SO4_CMOWeightedClosing, SO4_CMOTypicalClosing, SO4_VMAClosing, SO4_VMAVolume, SO4_VMAWeightedClosing, SO4_VMATypicalClosing, SO4_WSClosing, SO4_WSWeightedClosing, SO4_WSTypicalClosing, SO4_WSWeightedNormalClosing, SO4_WSWeightedWeightedClosing, SO4_WSWeightedTypicalClosing, SO4_WSVolume, SO4_WSVolumeAdjustedClosing, SO4_WSVolumeAdjustedTypicalClosing, SO4_WSVolumeAdjustedWeightedClosing, SO4_RSI_SMAClosing, SO4_RSI_SMAVolume, SO4_RSI_SMAWeightedClosing, SO4_RSI_SMATypicalClosing, SO4_RSI_SMAVolumeAdjustedClosing, SO4_RSI_SMAVolumeAdjustedWeightedClosing, SO4_RSI_SMAVolumeAdjustedTypicalClosing, SO4_RSI_EMAClosing, SO4_RSI_EMAVolume, SO4_RSI_EMAWeightedClosing, SO4_RSI_EMATypicalClosing, SO4_RSI_EMAVolumeAdjustedClosing, SO4_RSI_EMAVolumeAdjustedWeightedClosing, SO4_RSI_EMAVolumeAdjustedTypicalClosing, SO4_RSI_WSClosing, SO4_RSI_WSVolume, SO4_RSI_WSWeightedClosing, SO4_RSI_WSTypicalClosing, SO4_RSI_WSVolumeAdjustedClosing, SO4_RSI_WSVolumeAdjustedWeightedClosing, SO4_RSI_WSVolumeAdjustedTypicalClosing, SO4_STDEVClosing, SO4_STDEVWeightedClosing, SO4_STDEVTypicalClosing, SO4_STDEVVolume, SO4_RVIClosing, SO4_RVIVolume, SO4_RVIWeightedClosing, SO4_RVITypicalClosing, SO4_RVIVolumeAdjustedClosing, SO4_RVIVolumeAdjustedWeightedClosing, SO4_RVIVolumeAdjustedTypicalClosing, 
								SO5_Opening, SO5_High, SO5_Low, SO5_Closing, SO5_Volume, SO5_SMAClosing, SO5_SMAWeightedClosing, SO5_SMATypicalClosing, SO5_SMAVolume, SO5_VAMAClosing, SO5_VAMATypicalClosing, SO5_VAMAWeightedClosing, SO5_WMAClosing, SO5_WMAWeightedClosing, SO5_WMATypicalClosing, SO5_EMAClosing, SO5_EMAWeightedClosing, SO5_EMATypicalClosing, SO5_EMAVolume, SO5_CMOClosing, SO5_CMOVolume, SO5_CMOWeightedClosing, SO5_CMOTypicalClosing, SO5_VMAClosing, SO5_VMAVolume, SO5_VMAWeightedClosing, SO5_VMATypicalClosing, SO5_WSClosing, SO5_WSWeightedClosing, SO5_WSTypicalClosing, SO5_WSWeightedNormalClosing, SO5_WSWeightedWeightedClosing, SO5_WSWeightedTypicalClosing, SO5_WSVolume, SO5_WSVolumeAdjustedClosing, SO5_WSVolumeAdjustedTypicalClosing, SO5_WSVolumeAdjustedWeightedClosing, SO5_RSI_SMAClosing, SO5_RSI_SMAVolume, SO5_RSI_SMAWeightedClosing, SO5_RSI_SMATypicalClosing, SO5_RSI_SMAVolumeAdjustedClosing, SO5_RSI_SMAVolumeAdjustedWeightedClosing, SO5_RSI_SMAVolumeAdjustedTypicalClosing, SO5_RSI_EMAClosing, SO5_RSI_EMAVolume, SO5_RSI_EMAWeightedClosing, SO5_RSI_EMATypicalClosing, SO5_RSI_EMAVolumeAdjustedClosing, SO5_RSI_EMAVolumeAdjustedWeightedClosing, SO5_RSI_EMAVolumeAdjustedTypicalClosing, SO5_RSI_WSClosing, SO5_RSI_WSVolume, SO5_RSI_WSWeightedClosing, SO5_RSI_WSTypicalClosing, SO5_RSI_WSVolumeAdjustedClosing, SO5_RSI_WSVolumeAdjustedWeightedClosing, SO5_RSI_WSVolumeAdjustedTypicalClosing, SO5_STDEVClosing, SO5_STDEVWeightedClosing, SO5_STDEVTypicalClosing, SO5_STDEVVolume, SO5_RVIClosing, SO5_RVIVolume, SO5_RVIWeightedClosing, SO5_RVITypicalClosing, SO5_RVIVolumeAdjustedClosing, SO5_RVIVolumeAdjustedWeightedClosing, SO5_RVIVolumeAdjustedTypicalClosing, 
								SO6_Opening, SO6_High, SO6_Low, SO6_Closing, SO6_Volume, SO6_SMAClosing, SO6_SMAWeightedClosing, SO6_SMATypicalClosing, SO6_SMAVolume, SO6_VAMAClosing, SO6_VAMATypicalClosing, SO6_VAMAWeightedClosing, SO6_WMAClosing, SO6_WMAWeightedClosing, SO6_WMATypicalClosing, SO6_EMAClosing, SO6_EMAWeightedClosing, SO6_EMATypicalClosing, SO6_EMAVolume, SO6_CMOClosing, SO6_CMOVolume, SO6_CMOWeightedClosing, SO6_CMOTypicalClosing, SO6_VMAClosing, SO6_VMAVolume, SO6_VMAWeightedClosing, SO6_VMATypicalClosing, SO6_WSClosing, SO6_WSWeightedClosing, SO6_WSTypicalClosing, SO6_WSWeightedNormalClosing, SO6_WSWeightedWeightedClosing, SO6_WSWeightedTypicalClosing, SO6_WSVolume, SO6_WSVolumeAdjustedClosing, SO6_WSVolumeAdjustedTypicalClosing, SO6_WSVolumeAdjustedWeightedClosing, SO6_RSI_SMAClosing, SO6_RSI_SMAVolume, SO6_RSI_SMAWeightedClosing, SO6_RSI_SMATypicalClosing, SO6_RSI_SMAVolumeAdjustedClosing, SO6_RSI_SMAVolumeAdjustedWeightedClosing, SO6_RSI_SMAVolumeAdjustedTypicalClosing, SO6_RSI_EMAClosing, SO6_RSI_EMAVolume, SO6_RSI_EMAWeightedClosing, SO6_RSI_EMATypicalClosing, SO6_RSI_EMAVolumeAdjustedClosing, SO6_RSI_EMAVolumeAdjustedWeightedClosing, SO6_RSI_EMAVolumeAdjustedTypicalClosing, SO6_RSI_WSClosing, SO6_RSI_WSVolume, SO6_RSI_WSWeightedClosing, SO6_RSI_WSTypicalClosing, SO6_RSI_WSVolumeAdjustedClosing, SO6_RSI_WSVolumeAdjustedWeightedClosing, SO6_RSI_WSVolumeAdjustedTypicalClosing, SO6_STDEVClosing, SO6_STDEVWeightedClosing, SO6_STDEVTypicalClosing, SO6_STDEVVolume, SO6_RVIClosing, SO6_RVIVolume, SO6_RVIWeightedClosing, SO6_RVITypicalClosing, SO6_RVIVolumeAdjustedClosing, SO6_RVIVolumeAdjustedWeightedClosing, SO6_RVIVolumeAdjustedTypicalClosing, 
								SO7_Opening, SO7_High, SO7_Low, SO7_Closing, SO7_Volume, SO7_SMAClosing, SO7_SMAWeightedClosing, SO7_SMATypicalClosing, SO7_SMAVolume, SO7_VAMAClosing, SO7_VAMATypicalClosing, SO7_VAMAWeightedClosing, SO7_WMAClosing, SO7_WMAWeightedClosing, SO7_WMATypicalClosing, SO7_EMAClosing, SO7_EMAWeightedClosing, SO7_EMATypicalClosing, SO7_EMAVolume, SO7_CMOClosing, SO7_CMOVolume, SO7_CMOWeightedClosing, SO7_CMOTypicalClosing, SO7_VMAClosing, SO7_VMAVolume, SO7_VMAWeightedClosing, SO7_VMATypicalClosing, SO7_WSClosing, SO7_WSWeightedClosing, SO7_WSTypicalClosing, SO7_WSWeightedNormalClosing, SO7_WSWeightedWeightedClosing, SO7_WSWeightedTypicalClosing, SO7_WSVolume, SO7_WSVolumeAdjustedClosing, SO7_WSVolumeAdjustedTypicalClosing, SO7_WSVolumeAdjustedWeightedClosing, SO7_RSI_SMAClosing, SO7_RSI_SMAVolume, SO7_RSI_SMAWeightedClosing, SO7_RSI_SMATypicalClosing, SO7_RSI_SMAVolumeAdjustedClosing, SO7_RSI_SMAVolumeAdjustedWeightedClosing, SO7_RSI_SMAVolumeAdjustedTypicalClosing, SO7_RSI_EMAClosing, SO7_RSI_EMAVolume, SO7_RSI_EMAWeightedClosing, SO7_RSI_EMATypicalClosing, SO7_RSI_EMAVolumeAdjustedClosing, SO7_RSI_EMAVolumeAdjustedWeightedClosing, SO7_RSI_EMAVolumeAdjustedTypicalClosing, SO7_RSI_WSClosing, SO7_RSI_WSVolume, SO7_RSI_WSWeightedClosing, SO7_RSI_WSTypicalClosing, SO7_RSI_WSVolumeAdjustedClosing, SO7_RSI_WSVolumeAdjustedWeightedClosing, SO7_RSI_WSVolumeAdjustedTypicalClosing, SO7_STDEVClosing, SO7_STDEVWeightedClosing, SO7_STDEVTypicalClosing, SO7_STDEVVolume, SO7_RVIClosing, SO7_RVIVolume, SO7_RVIWeightedClosing, SO7_RVITypicalClosing, SO7_RVIVolumeAdjustedClosing, SO7_RVIVolumeAdjustedWeightedClosing, SO7_RVIVolumeAdjustedTypicalClosing, 
								SO8_Opening, SO8_High, SO8_Low, SO8_Closing, SO8_Volume, SO8_SMAClosing, SO8_SMAWeightedClosing, SO8_SMATypicalClosing, SO8_SMAVolume, SO8_VAMAClosing, SO8_VAMATypicalClosing, SO8_VAMAWeightedClosing, SO8_WMAClosing, SO8_WMAWeightedClosing, SO8_WMATypicalClosing, SO8_EMAClosing, SO8_EMAWeightedClosing, SO8_EMATypicalClosing, SO8_EMAVolume, SO8_CMOClosing, SO8_CMOVolume, SO8_CMOWeightedClosing, SO8_CMOTypicalClosing, SO8_VMAClosing, SO8_VMAVolume, SO8_VMAWeightedClosing, SO8_VMATypicalClosing, SO8_WSClosing, SO8_WSWeightedClosing, SO8_WSTypicalClosing, SO8_WSWeightedNormalClosing, SO8_WSWeightedWeightedClosing, SO8_WSWeightedTypicalClosing, SO8_WSVolume, SO8_WSVolumeAdjustedClosing, SO8_WSVolumeAdjustedTypicalClosing, SO8_WSVolumeAdjustedWeightedClosing, SO8_RSI_SMAClosing, SO8_RSI_SMAVolume, SO8_RSI_SMAWeightedClosing, SO8_RSI_SMATypicalClosing, SO8_RSI_SMAVolumeAdjustedClosing, SO8_RSI_SMAVolumeAdjustedWeightedClosing, SO8_RSI_SMAVolumeAdjustedTypicalClosing, SO8_RSI_EMAClosing, SO8_RSI_EMAVolume, SO8_RSI_EMAWeightedClosing, SO8_RSI_EMATypicalClosing, SO8_RSI_EMAVolumeAdjustedClosing, SO8_RSI_EMAVolumeAdjustedWeightedClosing, SO8_RSI_EMAVolumeAdjustedTypicalClosing, SO8_RSI_WSClosing, SO8_RSI_WSVolume, SO8_RSI_WSWeightedClosing, SO8_RSI_WSTypicalClosing, SO8_RSI_WSVolumeAdjustedClosing, SO8_RSI_WSVolumeAdjustedWeightedClosing, SO8_RSI_WSVolumeAdjustedTypicalClosing, SO8_STDEVClosing, SO8_STDEVWeightedClosing, SO8_STDEVTypicalClosing, SO8_STDEVVolume, SO8_RVIClosing, SO8_RVIVolume, SO8_RVIWeightedClosing, SO8_RVITypicalClosing, SO8_RVIVolumeAdjustedClosing, SO8_RVIVolumeAdjustedWeightedClosing, SO8_RVIVolumeAdjustedTypicalClosing, 
								SO9_Opening, SO9_High, SO9_Low, SO9_Closing, SO9_Volume, SO9_SMAClosing, SO9_SMAWeightedClosing, SO9_SMATypicalClosing, SO9_SMAVolume, SO9_VAMAClosing, SO9_VAMATypicalClosing, SO9_VAMAWeightedClosing, SO9_WMAClosing, SO9_WMAWeightedClosing, SO9_WMATypicalClosing, SO9_EMAClosing, SO9_EMAWeightedClosing, SO9_EMATypicalClosing, SO9_EMAVolume, SO9_CMOClosing, SO9_CMOVolume, SO9_CMOWeightedClosing, SO9_CMOTypicalClosing, SO9_VMAClosing, SO9_VMAVolume, SO9_VMAWeightedClosing, SO9_VMATypicalClosing, SO9_WSClosing, SO9_WSWeightedClosing, SO9_WSTypicalClosing, SO9_WSWeightedNormalClosing, SO9_WSWeightedWeightedClosing, SO9_WSWeightedTypicalClosing, SO9_WSVolume, SO9_WSVolumeAdjustedClosing, SO9_WSVolumeAdjustedTypicalClosing, SO9_WSVolumeAdjustedWeightedClosing, SO9_RSI_SMAClosing, SO9_RSI_SMAVolume, SO9_RSI_SMAWeightedClosing, SO9_RSI_SMATypicalClosing, SO9_RSI_SMAVolumeAdjustedClosing, SO9_RSI_SMAVolumeAdjustedWeightedClosing, SO9_RSI_SMAVolumeAdjustedTypicalClosing, SO9_RSI_EMAClosing, SO9_RSI_EMAVolume, SO9_RSI_EMAWeightedClosing, SO9_RSI_EMATypicalClosing, SO9_RSI_EMAVolumeAdjustedClosing, SO9_RSI_EMAVolumeAdjustedWeightedClosing, SO9_RSI_EMAVolumeAdjustedTypicalClosing, SO9_RSI_WSClosing, SO9_RSI_WSVolume, SO9_RSI_WSWeightedClosing, SO9_RSI_WSTypicalClosing, SO9_RSI_WSVolumeAdjustedClosing, SO9_RSI_WSVolumeAdjustedWeightedClosing, SO9_RSI_WSVolumeAdjustedTypicalClosing, SO9_STDEVClosing, SO9_STDEVWeightedClosing, SO9_STDEVTypicalClosing, SO9_STDEVVolume, SO9_RVIClosing, SO9_RVIVolume, SO9_RVIWeightedClosing, SO9_RVITypicalClosing, SO9_RVIVolumeAdjustedClosing, SO9_RVIVolumeAdjustedWeightedClosing, SO9_RVIVolumeAdjustedTypicalClosing
							)
	Values(
								@SymbolID, @Seq, @PeriodID,
								cast(@DateFrom as datetime), cast(@DateTo as datetime), @Opening, @High, @Low, @Closing, @Volume, 
								@TotalClosing, @TotalWeightedClosing, @TotalTypicalClosing, @TotalWeightedNormalClosing, @TotalWeightedWeightedClosing, @TotalWeightedTypicalClosing, @TotalVolume, @TotalVolumeAdjustedClosing, @TotalVolumeAdjustedTypicalClosing, @TotalVolumeAdjustedWeightedClosing, @TotalGainClosing, @TotalLossClosing, @TotalGainVolume, @TotalLossVolume, @TotalGainWeightedClosing, @TotalLossWeightedClosing, @TotalGainTypicalClosing, @TotalLossTypicalClosing, @TotalGainVolumeAdjustedClosing, @TotalLossVolumeAdjustedClosing, @TotalGainVolumeAdjustedWeightedClosing, @TotalLossVolumeAdjustedWeightedClosing, @TotalGainVolumeAdjustedTypicalClosing, @TotalLossVolumeAdjustedTypicalClosing, 
								@SMAClosing, @SMAWeightedClosing, @SMATypicalClosing, @SMAVolume, @SMAGainClosing, @SMALossClosing, @SMAGainVolume, @SMALossVolume, @SMAGainWeightedClosing, @SMALossWeightedClosing, @SMAGainTypicalClosing, @SMALossTypicalClosing, @SMAGainVolumeAdjustedClosing, @SMALossVolumeAdjustedClosing, @SMAGainVolumeAdjustedWeightedClosing, @SMALossVolumeAdjustedWeightedClosing, @SMAGainVolumeAdjustedTypicalClosing, @SMALossVolumeAdjustedTypicalClosing, 
								@VAMAClosing, @VAMATypicalClosing, @VAMAWeightedClosing, 
								@WMAClosing, @WMAWeightedClosing, @WMATypicalClosing, 
								@EMAClosing, @EMAWeightedClosing, @EMATypicalClosing, @EMAVolume, @EMAGainClosing, @EMALossClosing, @EMAGainVolume, @EMALossVolume, @EMAGainWeightedClosing, @EMALossWeightedClosing, @EMAGainTypicalClosing, @EMALossTypicalClosing, @EMAGainVolumeAdjustedClosing, @EMALossVolumeAdjustedClosing, @EMAGainVolumeAdjustedWeightedClosing, @EMALossVolumeAdjustedWeightedClosing, @EMAGainVolumeAdjustedTypicalClosing, @EMALossVolumeAdjustedTypicalClosing, 
								@CMOClosing, @CMOVolume, @CMOWeightedClosing, @CMOTypicalClosing, 
								@VMAClosing, @VMAVolume, @VMAWeightedClosing, @VMATypicalClosing, 
								@WSClosing, @WSWeightedClosing, @WSTypicalClosing, @WSWeightedNormalClosing, @WSWeightedWeightedClosing, @WSWeightedTypicalClosing, @WSVolume, @WSVolumeAdjustedClosing, @WSVolumeAdjustedTypicalClosing, @WSVolumeAdjustedWeightedClosing, @WSGainClosing, @WSLossClosing, @WSGainVolume, @WSLossVolume, @WSGainWeightedClosing, @WSLossWeightedClosing, @WSGainTypicalClosing, @WSLossTypicalClosing, @WSGainVolumeAdjustedClosing, @WSLossVolumeAdjustedClosing, @WSGainVolumeAdjustedWeightedClosing, @WSLossVolumeAdjustedWeightedClosing, @WSGainVolumeAdjustedTypicalClosing, @WSLossVolumeAdjustedTypicalClosing, 
								@RSI_SMAClosing, @RSI_SMAVolume, @RSI_SMAWeightedClosing, @RSI_SMATypicalClosing, @RSI_SMAVolumeAdjustedClosing, @RSI_SMAVolumeAdjustedWeightedClosing, @RSI_SMAVolumeAdjustedTypicalClosing, @RSI_EMAClosing, @RSI_EMAVolume, @RSI_EMAWeightedClosing, @RSI_EMATypicalClosing, @RSI_EMAVolumeAdjustedClosing, @RSI_EMAVolumeAdjustedWeightedClosing, @RSI_EMAVolumeAdjustedTypicalClosing, @RSI_WSClosing, @RSI_WSVolume, @RSI_WSWeightedClosing, @RSI_WSTypicalClosing, @RSI_WSVolumeAdjustedClosing, @RSI_WSVolumeAdjustedWeightedClosing, @RSI_WSVolumeAdjustedTypicalClosing, 
								@STDEVClosing, @STDEVWeightedClosing, @STDEVTypicalClosing, @STDEVVolume, @STDEVGainClosing, @STDEVLossClosing, @STDEVGainVolume, @STDEVLossVolume, @STDEVGainWeightedClosing, @STDEVLossWeightedClosing, @STDEVGainTypicalClosing, @STDEVLossTypicalClosing, @STDEVGainVolumeAdjustedClosing, @STDEVLossVolumeAdjustedClosing, @STDEVGainVolumeAdjustedWeightedClosing, @STDEVLossVolumeAdjustedWeightedClosing, @STDEVGainVolumeAdjustedTypicalClosing, @STDEVLossVolumeAdjustedTypicalClosing, 
								@RVIClosing, @RVIVolume, @RVIWeightedClosing, @RVITypicalClosing, @RVIVolumeAdjustedClosing, @RVIVolumeAdjustedWeightedClosing, @RVIVolumeAdjustedTypicalClosing,
								@HighestOpening, @LowestOpening, @HighestHigh, @LowestHigh, @HighestLow, @LowestLow, @HighestClosing, @LowestClosing, @HighestVolume, @LowestVolume, @HighestSMAClosing, @LowestSMAClosing, @HighestSMAWeightedClosing, @LowestSMAWeightedClosing, @HighestSMATypicalClosing, @LowestSMATypicalClosing, @HighestSMAVolume, @LowestSMAVolume, @HighestVAMAClosing, @LowestVAMAClosing, @HighestVAMATypicalClosing, @LowestVAMATypicalClosing, @HighestVAMAWeightedClosing, @LowestVAMAWeightedClosing, @HighestWMAClosing, @LowestWMAClosing, @HighestWMAWeightedClosing, @LowestWMAWeightedClosing, @HighestWMATypicalClosing, @LowestWMATypicalClosing, @HighestEMAClosing, @LowestEMAClosing, @HighestEMAWeightedClosing, @LowestEMAWeightedClosing, @HighestEMATypicalClosing, @LowestEMATypicalClosing, @HighestEMAVolume, @LowestEMAVolume, @HighestCMOClosing, @LowestCMOClosing, @HighestCMOVolume, @LowestCMOVolume, @HighestCMOWeightedClosing, @LowestCMOWeightedClosing, @HighestCMOTypicalClosing, @LowestCMOTypicalClosing, @HighestVMAClosing, @LowestVMAClosing, @HighestVMAVolume, @LowestVMAVolume, @HighestVMAWeightedClosing, @LowestVMAWeightedClosing, @HighestVMATypicalClosing, @LowestVMATypicalClosing, @HighestWSClosing, @LowestWSClosing, @HighestWSWeightedClosing, @LowestWSWeightedClosing, @HighestWSTypicalClosing, @LowestWSTypicalClosing, @HighestWSWeightedNormalClosing, @LowestWSWeightedNormalClosing, @HighestWSWeightedWeightedClosing, @LowestWSWeightedWeightedClosing, @HighestWSWeightedTypicalClosing, @LowestWSWeightedTypicalClosing, @HighestWSVolume, @LowestWSVolume, @HighestWSVolumeAdjustedClosing, @LowestWSVolumeAdjustedClosing, @HighestWSVolumeAdjustedTypicalClosing, @LowestWSVolumeAdjustedTypicalClosing, @HighestWSVolumeAdjustedWeightedClosing, @LowestWSVolumeAdjustedWeightedClosing, @HighestRSI_SMAClosing, @LowestRSI_SMAClosing, @HighestRSI_SMAVolume, @LowestRSI_SMAVolume, @HighestRSI_SMAWeightedClosing, @LowestRSI_SMAWeightedClosing, @HighestRSI_SMATypicalClosing, @LowestRSI_SMATypicalClosing, @HighestRSI_SMAVolumeAdjustedClosing, @LowestRSI_SMAVolumeAdjustedClosing, @HighestRSI_SMAVolumeAdjustedWeightedClosing, @LowestRSI_SMAVolumeAdjustedWeightedClosing, @HighestRSI_SMAVolumeAdjustedTypicalClosing, @LowestRSI_SMAVolumeAdjustedTypicalClosing, @HighestRSI_EMAClosing, @LowestRSI_EMAClosing, @HighestRSI_EMAVolume, @LowestRSI_EMAVolume, @HighestRSI_EMAWeightedClosing, @LowestRSI_EMAWeightedClosing, @HighestRSI_EMATypicalClosing, @LowestRSI_EMATypicalClosing, @HighestRSI_EMAVolumeAdjustedClosing, @LowestRSI_EMAVolumeAdjustedClosing, @HighestRSI_EMAVolumeAdjustedWeightedClosing, @LowestRSI_EMAVolumeAdjustedWeightedClosing, @HighestRSI_EMAVolumeAdjustedTypicalClosing, @LowestRSI_EMAVolumeAdjustedTypicalClosing, @HighestRSI_WSClosing, @LowestRSI_WSClosing, @HighestRSI_WSVolume, @LowestRSI_WSVolume, @HighestRSI_WSWeightedClosing, @LowestRSI_WSWeightedClosing, @HighestRSI_WSTypicalClosing, @LowestRSI_WSTypicalClosing, @HighestRSI_WSVolumeAdjustedClosing, @LowestRSI_WSVolumeAdjustedClosing, @HighestRSI_WSVolumeAdjustedWeightedClosing, @LowestRSI_WSVolumeAdjustedWeightedClosing, @HighestRSI_WSVolumeAdjustedTypicalClosing, @LowestRSI_WSVolumeAdjustedTypicalClosing, @HighestSTDEVClosing, @LowestSTDEVClosing, @HighestSTDEVWeightedClosing, @LowestSTDEVWeightedClosing, @HighestSTDEVTypicalClosing, @LowestSTDEVTypicalClosing, @HighestSTDEVVolume, @LowestSTDEVVolume, @HighestRVIClosing, @LowestRVIClosing, @HighestRVIVolume, @LowestRVIVolume, @HighestRVIWeightedClosing, @LowestRVIWeightedClosing, @HighestRVITypicalClosing, @LowestRVITypicalClosing, @HighestRVIVolumeAdjustedClosing, @LowestRVIVolumeAdjustedClosing, @HighestRVIVolumeAdjustedWeightedClosing, @LowestRVIVolumeAdjustedWeightedClosing, @HighestRVIVolumeAdjustedTypicalClosing, @LowestRVIVolumeAdjustedTypicalClosing, 
								@SO_Opening, @SO_High, @SO_Low, @SO_Closing, @SO_Volume, @SO_SMAClosing, @SO_SMAWeightedClosing, @SO_SMATypicalClosing, @SO_SMAVolume, @SO_VAMAClosing, @SO_VAMATypicalClosing, @SO_VAMAWeightedClosing, @SO_WMAClosing, @SO_WMAWeightedClosing, @SO_WMATypicalClosing, @SO_EMAClosing, @SO_EMAWeightedClosing, @SO_EMATypicalClosing, @SO_EMAVolume, @SO_CMOClosing, @SO_CMOVolume, @SO_CMOWeightedClosing, @SO_CMOTypicalClosing, @SO_VMAClosing, @SO_VMAVolume, @SO_VMAWeightedClosing, @SO_VMATypicalClosing, @SO_WSClosing, @SO_WSWeightedClosing, @SO_WSTypicalClosing, @SO_WSWeightedNormalClosing, @SO_WSWeightedWeightedClosing, @SO_WSWeightedTypicalClosing, @SO_WSVolume, @SO_WSVolumeAdjustedClosing, @SO_WSVolumeAdjustedTypicalClosing, @SO_WSVolumeAdjustedWeightedClosing, @SO_RSI_SMAClosing, @SO_RSI_SMAVolume, @SO_RSI_SMAWeightedClosing, @SO_RSI_SMATypicalClosing, @SO_RSI_SMAVolumeAdjustedClosing, @SO_RSI_SMAVolumeAdjustedWeightedClosing, @SO_RSI_SMAVolumeAdjustedTypicalClosing, @SO_RSI_EMAClosing, @SO_RSI_EMAVolume, @SO_RSI_EMAWeightedClosing, @SO_RSI_EMATypicalClosing, @SO_RSI_EMAVolumeAdjustedClosing, @SO_RSI_EMAVolumeAdjustedWeightedClosing, @SO_RSI_EMAVolumeAdjustedTypicalClosing, @SO_RSI_WSClosing, @SO_RSI_WSVolume, @SO_RSI_WSWeightedClosing, @SO_RSI_WSTypicalClosing, @SO_RSI_WSVolumeAdjustedClosing, @SO_RSI_WSVolumeAdjustedWeightedClosing, @SO_RSI_WSVolumeAdjustedTypicalClosing, @SO_STDEVClosing, @SO_STDEVWeightedClosing, @SO_STDEVTypicalClosing, @SO_STDEVVolume, @SO_RVIClosing, @SO_RVIVolume, @SO_RVIWeightedClosing, @SO_RVITypicalClosing, @SO_RVIVolumeAdjustedClosing, @SO_RVIVolumeAdjustedWeightedClosing, @SO_RVIVolumeAdjustedTypicalClosing, 
								@SO3_Opening, @SO3_High, @SO3_Low, @SO3_Closing, @SO3_Volume, @SO3_SMAClosing, @SO3_SMAWeightedClosing, @SO3_SMATypicalClosing, @SO3_SMAVolume, @SO3_VAMAClosing, @SO3_VAMATypicalClosing, @SO3_VAMAWeightedClosing, @SO3_WMAClosing, @SO3_WMAWeightedClosing, @SO3_WMATypicalClosing, @SO3_EMAClosing, @SO3_EMAWeightedClosing, @SO3_EMATypicalClosing, @SO3_EMAVolume, @SO3_CMOClosing, @SO3_CMOVolume, @SO3_CMOWeightedClosing, @SO3_CMOTypicalClosing, @SO3_VMAClosing, @SO3_VMAVolume, @SO3_VMAWeightedClosing, @SO3_VMATypicalClosing, @SO3_WSClosing, @SO3_WSWeightedClosing, @SO3_WSTypicalClosing, @SO3_WSWeightedNormalClosing, @SO3_WSWeightedWeightedClosing, @SO3_WSWeightedTypicalClosing, @SO3_WSVolume, @SO3_WSVolumeAdjustedClosing, @SO3_WSVolumeAdjustedTypicalClosing, @SO3_WSVolumeAdjustedWeightedClosing, @SO3_RSI_SMAClosing, @SO3_RSI_SMAVolume, @SO3_RSI_SMAWeightedClosing, @SO3_RSI_SMATypicalClosing, @SO3_RSI_SMAVolumeAdjustedClosing, @SO3_RSI_SMAVolumeAdjustedWeightedClosing, @SO3_RSI_SMAVolumeAdjustedTypicalClosing, @SO3_RSI_EMAClosing, @SO3_RSI_EMAVolume, @SO3_RSI_EMAWeightedClosing, @SO3_RSI_EMATypicalClosing, @SO3_RSI_EMAVolumeAdjustedClosing, @SO3_RSI_EMAVolumeAdjustedWeightedClosing, @SO3_RSI_EMAVolumeAdjustedTypicalClosing, @SO3_RSI_WSClosing, @SO3_RSI_WSVolume, @SO3_RSI_WSWeightedClosing, @SO3_RSI_WSTypicalClosing, @SO3_RSI_WSVolumeAdjustedClosing, @SO3_RSI_WSVolumeAdjustedWeightedClosing, @SO3_RSI_WSVolumeAdjustedTypicalClosing, @SO3_STDEVClosing, @SO3_STDEVWeightedClosing, @SO3_STDEVTypicalClosing, @SO3_STDEVVolume, @SO3_RVIClosing, @SO3_RVIVolume, @SO3_RVIWeightedClosing, @SO3_RVITypicalClosing, @SO3_RVIVolumeAdjustedClosing, @SO3_RVIVolumeAdjustedWeightedClosing, @SO3_RVIVolumeAdjustedTypicalClosing, 
								@SO4_Opening, @SO4_High, @SO4_Low, @SO4_Closing, @SO4_Volume, @SO4_SMAClosing, @SO4_SMAWeightedClosing, @SO4_SMATypicalClosing, @SO4_SMAVolume, @SO4_VAMAClosing, @SO4_VAMATypicalClosing, @SO4_VAMAWeightedClosing, @SO4_WMAClosing, @SO4_WMAWeightedClosing, @SO4_WMATypicalClosing, @SO4_EMAClosing, @SO4_EMAWeightedClosing, @SO4_EMATypicalClosing, @SO4_EMAVolume, @SO4_CMOClosing, @SO4_CMOVolume, @SO4_CMOWeightedClosing, @SO4_CMOTypicalClosing, @SO4_VMAClosing, @SO4_VMAVolume, @SO4_VMAWeightedClosing, @SO4_VMATypicalClosing, @SO4_WSClosing, @SO4_WSWeightedClosing, @SO4_WSTypicalClosing, @SO4_WSWeightedNormalClosing, @SO4_WSWeightedWeightedClosing, @SO4_WSWeightedTypicalClosing, @SO4_WSVolume, @SO4_WSVolumeAdjustedClosing, @SO4_WSVolumeAdjustedTypicalClosing, @SO4_WSVolumeAdjustedWeightedClosing, @SO4_RSI_SMAClosing, @SO4_RSI_SMAVolume, @SO4_RSI_SMAWeightedClosing, @SO4_RSI_SMATypicalClosing, @SO4_RSI_SMAVolumeAdjustedClosing, @SO4_RSI_SMAVolumeAdjustedWeightedClosing, @SO4_RSI_SMAVolumeAdjustedTypicalClosing, @SO4_RSI_EMAClosing, @SO4_RSI_EMAVolume, @SO4_RSI_EMAWeightedClosing, @SO4_RSI_EMATypicalClosing, @SO4_RSI_EMAVolumeAdjustedClosing, @SO4_RSI_EMAVolumeAdjustedWeightedClosing, @SO4_RSI_EMAVolumeAdjustedTypicalClosing, @SO4_RSI_WSClosing, @SO4_RSI_WSVolume, @SO4_RSI_WSWeightedClosing, @SO4_RSI_WSTypicalClosing, @SO4_RSI_WSVolumeAdjustedClosing, @SO4_RSI_WSVolumeAdjustedWeightedClosing, @SO4_RSI_WSVolumeAdjustedTypicalClosing, @SO4_STDEVClosing, @SO4_STDEVWeightedClosing, @SO4_STDEVTypicalClosing, @SO4_STDEVVolume, @SO4_RVIClosing, @SO4_RVIVolume, @SO4_RVIWeightedClosing, @SO4_RVITypicalClosing, @SO4_RVIVolumeAdjustedClosing, @SO4_RVIVolumeAdjustedWeightedClosing, @SO4_RVIVolumeAdjustedTypicalClosing, 
								@SO5_Opening, @SO5_High, @SO5_Low, @SO5_Closing, @SO5_Volume, @SO5_SMAClosing, @SO5_SMAWeightedClosing, @SO5_SMATypicalClosing, @SO5_SMAVolume, @SO5_VAMAClosing, @SO5_VAMATypicalClosing, @SO5_VAMAWeightedClosing, @SO5_WMAClosing, @SO5_WMAWeightedClosing, @SO5_WMATypicalClosing, @SO5_EMAClosing, @SO5_EMAWeightedClosing, @SO5_EMATypicalClosing, @SO5_EMAVolume, @SO5_CMOClosing, @SO5_CMOVolume, @SO5_CMOWeightedClosing, @SO5_CMOTypicalClosing, @SO5_VMAClosing, @SO5_VMAVolume, @SO5_VMAWeightedClosing, @SO5_VMATypicalClosing, @SO5_WSClosing, @SO5_WSWeightedClosing, @SO5_WSTypicalClosing, @SO5_WSWeightedNormalClosing, @SO5_WSWeightedWeightedClosing, @SO5_WSWeightedTypicalClosing, @SO5_WSVolume, @SO5_WSVolumeAdjustedClosing, @SO5_WSVolumeAdjustedTypicalClosing, @SO5_WSVolumeAdjustedWeightedClosing, @SO5_RSI_SMAClosing, @SO5_RSI_SMAVolume, @SO5_RSI_SMAWeightedClosing, @SO5_RSI_SMATypicalClosing, @SO5_RSI_SMAVolumeAdjustedClosing, @SO5_RSI_SMAVolumeAdjustedWeightedClosing, @SO5_RSI_SMAVolumeAdjustedTypicalClosing, @SO5_RSI_EMAClosing, @SO5_RSI_EMAVolume, @SO5_RSI_EMAWeightedClosing, @SO5_RSI_EMATypicalClosing, @SO5_RSI_EMAVolumeAdjustedClosing, @SO5_RSI_EMAVolumeAdjustedWeightedClosing, @SO5_RSI_EMAVolumeAdjustedTypicalClosing, @SO5_RSI_WSClosing, @SO5_RSI_WSVolume, @SO5_RSI_WSWeightedClosing, @SO5_RSI_WSTypicalClosing, @SO5_RSI_WSVolumeAdjustedClosing, @SO5_RSI_WSVolumeAdjustedWeightedClosing, @SO5_RSI_WSVolumeAdjustedTypicalClosing, @SO5_STDEVClosing, @SO5_STDEVWeightedClosing, @SO5_STDEVTypicalClosing, @SO5_STDEVVolume, @SO5_RVIClosing, @SO5_RVIVolume, @SO5_RVIWeightedClosing, @SO5_RVITypicalClosing, @SO5_RVIVolumeAdjustedClosing, @SO5_RVIVolumeAdjustedWeightedClosing, @SO5_RVIVolumeAdjustedTypicalClosing, 
								@SO6_Opening, @SO6_High, @SO6_Low, @SO6_Closing, @SO6_Volume, @SO6_SMAClosing, @SO6_SMAWeightedClosing, @SO6_SMATypicalClosing, @SO6_SMAVolume, @SO6_VAMAClosing, @SO6_VAMATypicalClosing, @SO6_VAMAWeightedClosing, @SO6_WMAClosing, @SO6_WMAWeightedClosing, @SO6_WMATypicalClosing, @SO6_EMAClosing, @SO6_EMAWeightedClosing, @SO6_EMATypicalClosing, @SO6_EMAVolume, @SO6_CMOClosing, @SO6_CMOVolume, @SO6_CMOWeightedClosing, @SO6_CMOTypicalClosing, @SO6_VMAClosing, @SO6_VMAVolume, @SO6_VMAWeightedClosing, @SO6_VMATypicalClosing, @SO6_WSClosing, @SO6_WSWeightedClosing, @SO6_WSTypicalClosing, @SO6_WSWeightedNormalClosing, @SO6_WSWeightedWeightedClosing, @SO6_WSWeightedTypicalClosing, @SO6_WSVolume, @SO6_WSVolumeAdjustedClosing, @SO6_WSVolumeAdjustedTypicalClosing, @SO6_WSVolumeAdjustedWeightedClosing, @SO6_RSI_SMAClosing, @SO6_RSI_SMAVolume, @SO6_RSI_SMAWeightedClosing, @SO6_RSI_SMATypicalClosing, @SO6_RSI_SMAVolumeAdjustedClosing, @SO6_RSI_SMAVolumeAdjustedWeightedClosing, @SO6_RSI_SMAVolumeAdjustedTypicalClosing, @SO6_RSI_EMAClosing, @SO6_RSI_EMAVolume, @SO6_RSI_EMAWeightedClosing, @SO6_RSI_EMATypicalClosing, @SO6_RSI_EMAVolumeAdjustedClosing, @SO6_RSI_EMAVolumeAdjustedWeightedClosing, @SO6_RSI_EMAVolumeAdjustedTypicalClosing, @SO6_RSI_WSClosing, @SO6_RSI_WSVolume, @SO6_RSI_WSWeightedClosing, @SO6_RSI_WSTypicalClosing, @SO6_RSI_WSVolumeAdjustedClosing, @SO6_RSI_WSVolumeAdjustedWeightedClosing, @SO6_RSI_WSVolumeAdjustedTypicalClosing, @SO6_STDEVClosing, @SO6_STDEVWeightedClosing, @SO6_STDEVTypicalClosing, @SO6_STDEVVolume, @SO6_RVIClosing, @SO6_RVIVolume, @SO6_RVIWeightedClosing, @SO6_RVITypicalClosing, @SO6_RVIVolumeAdjustedClosing, @SO6_RVIVolumeAdjustedWeightedClosing, @SO6_RVIVolumeAdjustedTypicalClosing, 
								@SO7_Opening, @SO7_High, @SO7_Low, @SO7_Closing, @SO7_Volume, @SO7_SMAClosing, @SO7_SMAWeightedClosing, @SO7_SMATypicalClosing, @SO7_SMAVolume, @SO7_VAMAClosing, @SO7_VAMATypicalClosing, @SO7_VAMAWeightedClosing, @SO7_WMAClosing, @SO7_WMAWeightedClosing, @SO7_WMATypicalClosing, @SO7_EMAClosing, @SO7_EMAWeightedClosing, @SO7_EMATypicalClosing, @SO7_EMAVolume, @SO7_CMOClosing, @SO7_CMOVolume, @SO7_CMOWeightedClosing, @SO7_CMOTypicalClosing, @SO7_VMAClosing, @SO7_VMAVolume, @SO7_VMAWeightedClosing, @SO7_VMATypicalClosing, @SO7_WSClosing, @SO7_WSWeightedClosing, @SO7_WSTypicalClosing, @SO7_WSWeightedNormalClosing, @SO7_WSWeightedWeightedClosing, @SO7_WSWeightedTypicalClosing, @SO7_WSVolume, @SO7_WSVolumeAdjustedClosing, @SO7_WSVolumeAdjustedTypicalClosing, @SO7_WSVolumeAdjustedWeightedClosing, @SO7_RSI_SMAClosing, @SO7_RSI_SMAVolume, @SO7_RSI_SMAWeightedClosing, @SO7_RSI_SMATypicalClosing, @SO7_RSI_SMAVolumeAdjustedClosing, @SO7_RSI_SMAVolumeAdjustedWeightedClosing, @SO7_RSI_SMAVolumeAdjustedTypicalClosing, @SO7_RSI_EMAClosing, @SO7_RSI_EMAVolume, @SO7_RSI_EMAWeightedClosing, @SO7_RSI_EMATypicalClosing, @SO7_RSI_EMAVolumeAdjustedClosing, @SO7_RSI_EMAVolumeAdjustedWeightedClosing, @SO7_RSI_EMAVolumeAdjustedTypicalClosing, @SO7_RSI_WSClosing, @SO7_RSI_WSVolume, @SO7_RSI_WSWeightedClosing, @SO7_RSI_WSTypicalClosing, @SO7_RSI_WSVolumeAdjustedClosing, @SO7_RSI_WSVolumeAdjustedWeightedClosing, @SO7_RSI_WSVolumeAdjustedTypicalClosing, @SO7_STDEVClosing, @SO7_STDEVWeightedClosing, @SO7_STDEVTypicalClosing, @SO7_STDEVVolume, @SO7_RVIClosing, @SO7_RVIVolume, @SO7_RVIWeightedClosing, @SO7_RVITypicalClosing, @SO7_RVIVolumeAdjustedClosing, @SO7_RVIVolumeAdjustedWeightedClosing, @SO7_RVIVolumeAdjustedTypicalClosing, 
								@SO8_Opening, @SO8_High, @SO8_Low, @SO8_Closing, @SO8_Volume, @SO8_SMAClosing, @SO8_SMAWeightedClosing, @SO8_SMATypicalClosing, @SO8_SMAVolume, @SO8_VAMAClosing, @SO8_VAMATypicalClosing, @SO8_VAMAWeightedClosing, @SO8_WMAClosing, @SO8_WMAWeightedClosing, @SO8_WMATypicalClosing, @SO8_EMAClosing, @SO8_EMAWeightedClosing, @SO8_EMATypicalClosing, @SO8_EMAVolume, @SO8_CMOClosing, @SO8_CMOVolume, @SO8_CMOWeightedClosing, @SO8_CMOTypicalClosing, @SO8_VMAClosing, @SO8_VMAVolume, @SO8_VMAWeightedClosing, @SO8_VMATypicalClosing, @SO8_WSClosing, @SO8_WSWeightedClosing, @SO8_WSTypicalClosing, @SO8_WSWeightedNormalClosing, @SO8_WSWeightedWeightedClosing, @SO8_WSWeightedTypicalClosing, @SO8_WSVolume, @SO8_WSVolumeAdjustedClosing, @SO8_WSVolumeAdjustedTypicalClosing, @SO8_WSVolumeAdjustedWeightedClosing, @SO8_RSI_SMAClosing, @SO8_RSI_SMAVolume, @SO8_RSI_SMAWeightedClosing, @SO8_RSI_SMATypicalClosing, @SO8_RSI_SMAVolumeAdjustedClosing, @SO8_RSI_SMAVolumeAdjustedWeightedClosing, @SO8_RSI_SMAVolumeAdjustedTypicalClosing, @SO8_RSI_EMAClosing, @SO8_RSI_EMAVolume, @SO8_RSI_EMAWeightedClosing, @SO8_RSI_EMATypicalClosing, @SO8_RSI_EMAVolumeAdjustedClosing, @SO8_RSI_EMAVolumeAdjustedWeightedClosing, @SO8_RSI_EMAVolumeAdjustedTypicalClosing, @SO8_RSI_WSClosing, @SO8_RSI_WSVolume, @SO8_RSI_WSWeightedClosing, @SO8_RSI_WSTypicalClosing, @SO8_RSI_WSVolumeAdjustedClosing, @SO8_RSI_WSVolumeAdjustedWeightedClosing, @SO8_RSI_WSVolumeAdjustedTypicalClosing, @SO8_STDEVClosing, @SO8_STDEVWeightedClosing, @SO8_STDEVTypicalClosing, @SO8_STDEVVolume, @SO8_RVIClosing, @SO8_RVIVolume, @SO8_RVIWeightedClosing, @SO8_RVITypicalClosing, @SO8_RVIVolumeAdjustedClosing, @SO8_RVIVolumeAdjustedWeightedClosing, @SO8_RVIVolumeAdjustedTypicalClosing, 
								@SO9_Opening, @SO9_High, @SO9_Low, @SO9_Closing, @SO9_Volume, @SO9_SMAClosing, @SO9_SMAWeightedClosing, @SO9_SMATypicalClosing, @SO9_SMAVolume, @SO9_VAMAClosing, @SO9_VAMATypicalClosing, @SO9_VAMAWeightedClosing, @SO9_WMAClosing, @SO9_WMAWeightedClosing, @SO9_WMATypicalClosing, @SO9_EMAClosing, @SO9_EMAWeightedClosing, @SO9_EMATypicalClosing, @SO9_EMAVolume, @SO9_CMOClosing, @SO9_CMOVolume, @SO9_CMOWeightedClosing, @SO9_CMOTypicalClosing, @SO9_VMAClosing, @SO9_VMAVolume, @SO9_VMAWeightedClosing, @SO9_VMATypicalClosing, @SO9_WSClosing, @SO9_WSWeightedClosing, @SO9_WSTypicalClosing, @SO9_WSWeightedNormalClosing, @SO9_WSWeightedWeightedClosing, @SO9_WSWeightedTypicalClosing, @SO9_WSVolume, @SO9_WSVolumeAdjustedClosing, @SO9_WSVolumeAdjustedTypicalClosing, @SO9_WSVolumeAdjustedWeightedClosing, @SO9_RSI_SMAClosing, @SO9_RSI_SMAVolume, @SO9_RSI_SMAWeightedClosing, @SO9_RSI_SMATypicalClosing, @SO9_RSI_SMAVolumeAdjustedClosing, @SO9_RSI_SMAVolumeAdjustedWeightedClosing, @SO9_RSI_SMAVolumeAdjustedTypicalClosing, @SO9_RSI_EMAClosing, @SO9_RSI_EMAVolume, @SO9_RSI_EMAWeightedClosing, @SO9_RSI_EMATypicalClosing, @SO9_RSI_EMAVolumeAdjustedClosing, @SO9_RSI_EMAVolumeAdjustedWeightedClosing, @SO9_RSI_EMAVolumeAdjustedTypicalClosing, @SO9_RSI_WSClosing, @SO9_RSI_WSVolume, @SO9_RSI_WSWeightedClosing, @SO9_RSI_WSTypicalClosing, @SO9_RSI_WSVolumeAdjustedClosing, @SO9_RSI_WSVolumeAdjustedWeightedClosing, @SO9_RSI_WSVolumeAdjustedTypicalClosing, @SO9_STDEVClosing, @SO9_STDEVWeightedClosing, @SO9_STDEVTypicalClosing, @SO9_STDEVVolume, @SO9_RVIClosing, @SO9_RVIVolume, @SO9_RVIWeightedClosing, @SO9_RVITypicalClosing, @SO9_RVIVolumeAdjustedClosing, @SO9_RVIVolumeAdjustedWeightedClosing, @SO9_RVIVolumeAdjustedTypicalClosing
			)

end
go
