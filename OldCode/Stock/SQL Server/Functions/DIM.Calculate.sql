alter function DIM.Calculate
(
	@SymbolID int,
	@Seq int,
	@PeriodID smallint
) returns @ret table (ID smallint primary key, Value float, Description varchar(512))
as
begin
	/*Declare Variables*/
	declare @F_Seq float, @F_Period float,  @F_One float, @F_Two float, @F_Hundred float, @F_Thousand float,
			@alpha float, @beta float, @gamma float, @delta float, @eplsilon float, @zeta float
	select	@F_Seq = @Seq, @F_Period = @PeriodID, @F_One = 1, @F_Two = 2,	@F_Hundred = 100, @F_Thousand = 1000
	
	/*Hold Last Record*/
	declare @LastDateFrom float, @LastDateTo float, @LastOpening float, @LastHigh float, @LastLow float, @LastClosing float, @LastVolume float, @LastTotalClosing float, @LastTotalWeightedClosing float, @LastTotalTypicalClosing float, @LastTotalWeightedNormalClosing float, @LastTotalWeightedWeightedClosing float, @LastTotalWeightedTypicalClosing float, @LastTotalVolume float, @LastTotalVolumeAdjustedClosing float, @LastTotalVolumeAdjustedTypicalClosing float, @LastTotalVolumeAdjustedWeightedClosing float, @LastTotalGainClosing float, @LastTotalLossClosing float, @LastTotalGainVolume float, @LastTotalLossVolume float, @LastTotalGainWeightedClosing float, @LastTotalLossWeightedClosing float, @LastTotalGainTypicalClosing float, @LastTotalLossTypicalClosing float, @LastTotalGainVolumeAdjustedClosing float, @LastTotalLossVolumeAdjustedClosing float, @LastTotalGainVolumeAdjustedWeightedClosing float, @LastTotalLossVolumeAdjustedWeightedClosing float, @LastTotalGainVolumeAdjustedTypicalClosing float, @LastTotalLossVolumeAdjustedTypicalClosing float, @LastSMAClosing float, @LastSMAWeightedClosing float, @LastSMATypicalClosing float, @LastSMAVolume float, @LastSMAGainClosing float, @LastSMALossClosing float, @LastSMAGainVolume float, @LastSMALossVolume float, @LastSMAGainWeightedClosing float, @LastSMALossWeightedClosing float, @LastSMAGainTypicalClosing float, @LastSMALossTypicalClosing float, @LastSMAGainVolumeAdjustedClosing float, @LastSMALossVolumeAdjustedClosing float, @LastSMAGainVolumeAdjustedWeightedClosing float, @LastSMALossVolumeAdjustedWeightedClosing float, @LastSMAGainVolumeAdjustedTypicalClosing float, @LastSMALossVolumeAdjustedTypicalClosing float, @LastVAMAClosing float, @LastVAMATypicalClosing float, @LastVAMAWeightedClosing float, @LastWMAClosing float, @LastWMAWeightedClosing float, @LastWMATypicalClosing float, @LastEMAClosing float, @LastEMAWeightedClosing float, @LastEMATypicalClosing float, @LastEMAVolume float, @LastEMAGainClosing float, @LastEMALossClosing float, @LastEMAGainVolume float, @LastEMALossVolume float, @LastEMAGainWeightedClosing float, @LastEMALossWeightedClosing float, @LastEMAGainTypicalClosing float, @LastEMALossTypicalClosing float, @LastEMAGainVolumeAdjustedClosing float, @LastEMALossVolumeAdjustedClosing float, @LastEMAGainVolumeAdjustedWeightedClosing float, @LastEMALossVolumeAdjustedWeightedClosing float, @LastEMAGainVolumeAdjustedTypicalClosing float, @LastEMALossVolumeAdjustedTypicalClosing float, @LastCMOClosing float, @LastCMOVolume float, @LastCMOWeightedClosing float, @LastCMOTypicalClosing float, @LastVMAClosing float, @LastVMAVolume float, @LastVMAWeightedClosing float, @LastVMATypicalClosing float, @LastWSClosing float, @LastWSWeightedClosing float, @LastWSTypicalClosing float, @LastWSWeightedNormalClosing float, @LastWSWeightedWeightedClosing float, @LastWSWeightedTypicalClosing float, @LastWSVolume float, @LastWSVolumeAdjustedClosing float, @LastWSVolumeAdjustedTypicalClosing float, @LastWSVolumeAdjustedWeightedClosing float, @LastWSGainClosing float, @LastWSLossClosing float, @LastWSGainVolume float, @LastWSLossVolume float, @LastWSGainWeightedClosing float, @LastWSLossWeightedClosing float, @LastWSGainTypicalClosing float, @LastWSLossTypicalClosing float, @LastWSGainVolumeAdjustedClosing float, @LastWSLossVolumeAdjustedClosing float, @LastWSGainVolumeAdjustedWeightedClosing float, @LastWSLossVolumeAdjustedWeightedClosing float, @LastWSGainVolumeAdjustedTypicalClosing float, @LastWSLossVolumeAdjustedTypicalClosing float, @LastRSI_SMAClosing float, @LastRSI_SMAVolume float, @LastRSI_SMAWeightedClosing float, @LastRSI_SMATypicalClosing float, @LastRSI_SMAVolumeAdjustedClosing float, @LastRSI_SMAVolumeAdjustedWeightedClosing float, @LastRSI_SMAVolumeAdjustedTypicalClosing float, @LastRSI_EMAClosing float, @LastRSI_EMAVolume float, @LastRSI_EMAWeightedClosing float, @LastRSI_EMATypicalClosing float, @LastRSI_EMAVolumeAdjustedClosing float, @LastRSI_EMAVolumeAdjustedWeightedClosing float, @LastRSI_EMAVolumeAdjustedTypicalClosing float, @LastRSI_WSClosing float, @LastRSI_WSVolume float, @LastRSI_WSWeightedClosing float, @LastRSI_WSTypicalClosing float, @LastRSI_WSVolumeAdjustedClosing float, @LastRSI_WSVolumeAdjustedWeightedClosing float, @LastRSI_WSVolumeAdjustedTypicalClosing float
	/*Hold Calculated record*/ 
	declare @DateFrom float, @DateTo float, @Opening float, @High float, @Low float, @Closing float, @Volume float, @TotalClosing float, @TotalWeightedClosing float, @TotalTypicalClosing float, @TotalWeightedNormalClosing float, @TotalWeightedWeightedClosing float, @TotalWeightedTypicalClosing float, @TotalVolume float, @TotalVolumeAdjustedClosing float, @TotalVolumeAdjustedTypicalClosing float, @TotalVolumeAdjustedWeightedClosing float, @TotalGainClosing float, @TotalLossClosing float, @TotalGainVolume float, @TotalLossVolume float, @TotalGainWeightedClosing float, @TotalLossWeightedClosing float, @TotalGainTypicalClosing float, @TotalLossTypicalClosing float, @TotalGainVolumeAdjustedClosing float, @TotalLossVolumeAdjustedClosing float, @TotalGainVolumeAdjustedWeightedClosing float, @TotalLossVolumeAdjustedWeightedClosing float, @TotalGainVolumeAdjustedTypicalClosing float, @TotalLossVolumeAdjustedTypicalClosing float, @SMAClosing float, @SMAWeightedClosing float, @SMATypicalClosing float, @SMAVolume float, @SMAGainClosing float, @SMALossClosing float, @SMAGainVolume float, @SMALossVolume float, @SMAGainWeightedClosing float, @SMALossWeightedClosing float, @SMAGainTypicalClosing float, @SMALossTypicalClosing float, @SMAGainVolumeAdjustedClosing float, @SMALossVolumeAdjustedClosing float, @SMAGainVolumeAdjustedWeightedClosing float, @SMALossVolumeAdjustedWeightedClosing float, @SMAGainVolumeAdjustedTypicalClosing float, @SMALossVolumeAdjustedTypicalClosing float, @VAMAClosing float, @VAMATypicalClosing float, @VAMAWeightedClosing float, @WMAClosing float, @WMAWeightedClosing float, @WMATypicalClosing float, @EMAClosing float, @EMAWeightedClosing float, @EMATypicalClosing float, @EMAVolume float, @EMAGainClosing float, @EMALossClosing float, @EMAGainVolume float, @EMALossVolume float, @EMAGainWeightedClosing float, @EMALossWeightedClosing float, @EMAGainTypicalClosing float, @EMALossTypicalClosing float, @EMAGainVolumeAdjustedClosing float, @EMALossVolumeAdjustedClosing float, @EMAGainVolumeAdjustedWeightedClosing float, @EMALossVolumeAdjustedWeightedClosing float, @EMAGainVolumeAdjustedTypicalClosing float, @EMALossVolumeAdjustedTypicalClosing float, @CMOClosing float, @CMOVolume float, @CMOWeightedClosing float, @CMOTypicalClosing float, @VMAClosing float, @VMAVolume float, @VMAWeightedClosing float, @VMATypicalClosing float, @WSClosing float, @WSWeightedClosing float, @WSTypicalClosing float, @WSWeightedNormalClosing float, @WSWeightedWeightedClosing float, @WSWeightedTypicalClosing float, @WSVolume float, @WSVolumeAdjustedClosing float, @WSVolumeAdjustedTypicalClosing float, @WSVolumeAdjustedWeightedClosing float, @WSGainClosing float, @WSLossClosing float, @WSGainVolume float, @WSLossVolume float, @WSGainWeightedClosing float, @WSLossWeightedClosing float, @WSGainTypicalClosing float, @WSLossTypicalClosing float, @WSGainVolumeAdjustedClosing float, @WSLossVolumeAdjustedClosing float, @WSGainVolumeAdjustedWeightedClosing float, @WSLossVolumeAdjustedWeightedClosing float, @WSGainVolumeAdjustedTypicalClosing float, @WSLossVolumeAdjustedTypicalClosing float, @RSI_SMAClosing float, @RSI_SMAVolume float, @RSI_SMAWeightedClosing float, @RSI_SMATypicalClosing float, @RSI_SMAVolumeAdjustedClosing float, @RSI_SMAVolumeAdjustedWeightedClosing float, @RSI_SMAVolumeAdjustedTypicalClosing float, @RSI_EMAClosing float, @RSI_EMAVolume float, @RSI_EMAWeightedClosing float, @RSI_EMATypicalClosing float, @RSI_EMAVolumeAdjustedClosing float, @RSI_EMAVolumeAdjustedWeightedClosing float, @RSI_EMAVolumeAdjustedTypicalClosing float, @RSI_WSClosing float, @RSI_WSVolume float, @RSI_WSWeightedClosing float, @RSI_WSTypicalClosing float, @RSI_WSVolumeAdjustedClosing float, @RSI_WSVolumeAdjustedWeightedClosing float, @RSI_WSVolumeAdjustedTypicalClosing float
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
			@LastRSI_SMAClosing =  isnull(RSI_SMAClosing, 0) , @LastRSI_SMAVolume =  isnull(RSI_SMAVolume, 0) , @LastRSI_SMAWeightedClosing =  isnull(RSI_SMAWeightedClosing, 0) , @LastRSI_SMATypicalClosing =  isnull(RSI_SMATypicalClosing, 0) , @LastRSI_SMAVolumeAdjustedClosing =  isnull(RSI_SMAVolumeAdjustedClosing, 0) , @LastRSI_SMAVolumeAdjustedWeightedClosing =  isnull(RSI_SMAVolumeAdjustedWeightedClosing, 0) , @LastRSI_SMAVolumeAdjustedTypicalClosing =  isnull(RSI_SMAVolumeAdjustedTypicalClosing, 0) , @LastRSI_EMAClosing =  isnull(RSI_EMAClosing, 0) , @LastRSI_EMAVolume =  isnull(RSI_EMAVolume, 0) , @LastRSI_EMAWeightedClosing =  isnull(RSI_EMAWeightedClosing, 0) , @LastRSI_EMATypicalClosing =  isnull(RSI_EMATypicalClosing, 0) , @LastRSI_EMAVolumeAdjustedClosing =  isnull(RSI_EMAVolumeAdjustedClosing, 0) , @LastRSI_EMAVolumeAdjustedWeightedClosing =  isnull(RSI_EMAVolumeAdjustedWeightedClosing, 0) , @LastRSI_EMAVolumeAdjustedTypicalClosing =  isnull(RSI_EMAVolumeAdjustedTypicalClosing, 0) , @LastRSI_WSClosing =  isnull(RSI_WSClosing, 0) , @LastRSI_WSVolume =  isnull(RSI_WSVolume, 0) , @LastRSI_WSWeightedClosing =  isnull(RSI_WSWeightedClosing, 0) , @LastRSI_WSTypicalClosing =  isnull(RSI_WSTypicalClosing, 0) , @LastRSI_WSVolumeAdjustedClosing =  isnull(RSI_WSVolumeAdjustedClosing, 0) , @LastRSI_WSVolumeAdjustedWeightedClosing =  isnull(RSI_WSVolumeAdjustedWeightedClosing, 0) , @LastRSI_WSVolumeAdjustedTypicalClosing =  isnull(RSI_WSVolumeAdjustedTypicalClosing, 0) 
	from (select 1 as x) x
		left outer join DIM.Fact_Base f on	f.SymbolID = @SymbolID 
										and f.Seq = @Seq -1
										and f.PeriodID = @PeriodID
	/*Read Period -1 Daily*/
	select  @A0_Date = isnull(cast(Date as float),0), @A0_Opening = isnull(Opening,0), @A0_High = isnull(High,0), @A0_Low = isnull(Low,0), @A0_Closing = isnull(Closing,0), @A0_Volume = isnull(Volume,0), @A0_Interest = isnull(Interest,0), @A0_GainClosing = isnull((case when DiffClosing >=0 then DiffClosing else 0 end), 0), @A0_LossClosing = isnull((case when DiffClosing <=0 then -DiffClosing else 0 end), 0), @A0_GainVolume = isnull((case when DiffVolume >=0 then DiffVolume else 0 end), 0), @A0_LossVolume = isnull((case when DiffVolume <=0 then -DiffVolume else 0 end), 0), @A0_WeightedClosing = isnull(WeightedClosing,0), @A0_GainWeightedClosing = isnull((case when DiffWeightedClosing >=0 then DiffWeightedClosing else 0 end), 0), @A0_LossWeightedClosing = isnull((case when DiffWeightedClosing <=0 then -DiffWeightedClosing else 0 end), 0), @A0_TypicalClosing = isnull(TypicalClosing,0), @A0_GainTypicalClosing = isnull((case when DiffTypicalClosing >=0 then DiffTypicalClosing else 0 end), 0), @A0_LossTypicalClosing = isnull((case when DiffTypicalClosing <=0 then -DiffTypicalClosing else 0 end), 0), @A0_VolumeAdjustedClosing = isnull(VolumeAdjustedClosing,0), @A0_GainVolumeAdjustedClosing = isnull((case when DiffVolumeAdjustedClosing >=0 then DiffVolumeAdjustedClosing else 0 end), 0), @A0_LossVolumeAdjustedClosing = isnull((case when DiffVolumeAdjustedClosing <=0 then -DiffVolumeAdjustedClosing else 0 end), 0), @A0_VolumeAdjustedWeightedClosing = isnull(VolumeAdjustedWeightedClosing,0), @A0_GainVolumeAdjustedWeightedClosing = isnull((case when DiffVolumeAdjustedWeightedClosing >=0 then DiffVolumeAdjustedWeightedClosing else 0 end), 0), @A0_LossVolumeAdjustedWeightedClosing = isnull((case when DiffVolumeAdjustedWeightedClosing <=0 then -DiffVolumeAdjustedWeightedClosing else 0 end), 0), @A0_VolumeAdjustedTypicalClosing = isnull(VolumeAdjustedTypicalClosing,0), @A0_GainVolumeAdjustedTypicalClosing = isnull((case when DiffVolumeAdjustedTypicalClosing >=0 then DiffVolumeAdjustedTypicalClosing else 0 end), 0), @A0_LossVolumeAdjustedTypicalClosing = isnull((case when DiffVolumeAdjustedTypicalClosing <=0 then -DiffVolumeAdjustedTypicalClosing else 0 end), 0)
	from (select 1 as x) x
		left outer join A.Daily b with (nolock) on b.SymbolID = @SymbolID and b.Seq = @Seq - @PeriodID --+ 1
	/*Read Period Daily*/
	select top 1  @A1_Date = isnull(cast(Date as float),0), @A1_Opening = isnull(Opening,0), @A1_High = isnull(High,0), @A1_Low = isnull(Low,0), @A1_Closing = isnull(Closing,0), @A1_Volume = isnull(Volume,0), @A1_Interest = isnull(Interest,0), @A1_GainClosing = isnull((case when DiffClosing >=0 then DiffClosing else 0 end), 0), @A1_LossClosing = isnull((case when DiffClosing <=0 then -DiffClosing else 0 end), 0), @A1_GainVolume = isnull((case when DiffVolume >=0 then DiffVolume else 0 end), 0), @A1_LossVolume = isnull((case when DiffVolume <=0 then -DiffVolume else 0 end), 0), @A1_WeightedClosing = isnull(WeightedClosing,0), @A1_GainWeightedClosing = isnull((case when DiffWeightedClosing >=0 then DiffWeightedClosing else 0 end), 0), @A1_LossWeightedClosing = isnull((case when DiffWeightedClosing <=0 then -DiffWeightedClosing else 0 end), 0), @A1_TypicalClosing = isnull(TypicalClosing,0), @A1_GainTypicalClosing = isnull((case when DiffTypicalClosing >=0 then DiffTypicalClosing else 0 end), 0), @A1_LossTypicalClosing = isnull((case when DiffTypicalClosing <=0 then -DiffTypicalClosing else 0 end), 0), @A1_VolumeAdjustedClosing = isnull(VolumeAdjustedClosing,0), @A1_GainVolumeAdjustedClosing = isnull((case when DiffVolumeAdjustedClosing >=0 then DiffVolumeAdjustedClosing else 0 end), 0), @A1_LossVolumeAdjustedClosing = isnull((case when DiffVolumeAdjustedClosing <=0 then -DiffVolumeAdjustedClosing else 0 end), 0), @A1_VolumeAdjustedWeightedClosing = isnull(VolumeAdjustedWeightedClosing,0), @A1_GainVolumeAdjustedWeightedClosing = isnull((case when DiffVolumeAdjustedWeightedClosing >=0 then DiffVolumeAdjustedWeightedClosing else 0 end), 0), @A1_LossVolumeAdjustedWeightedClosing = isnull((case when DiffVolumeAdjustedWeightedClosing <=0 then -DiffVolumeAdjustedWeightedClosing else 0 end), 0), @A1_VolumeAdjustedTypicalClosing = isnull(VolumeAdjustedTypicalClosing,0), @A1_GainVolumeAdjustedTypicalClosing = isnull((case when DiffVolumeAdjustedTypicalClosing >=0 then DiffVolumeAdjustedTypicalClosing else 0 end), 0), @A1_LossVolumeAdjustedTypicalClosing = isnull((case when DiffVolumeAdjustedTypicalClosing <=0 then -DiffVolumeAdjustedTypicalClosing else 0 end), 0)
	from A.Daily with(nolock)
		where SymbolID = @SymbolID
			and Seq >= @Seq - @PeriodID + 1
		order by SymbolID, Seq
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
	select @VAMAClosing = @TotalVolumeAdjustedClosing / @alpha
	--Volume Adjusted Moving Average of Typical Closing
	select @VAMATypicalClosing = @TotalVolumeAdjustedTypicalClosing / @alpha
	--Volume Adjusted Moving Average of Weighted Closing
	select @VAMAWeightedClosing = @TotalVolumeAdjustedWeightedClosing / @alpha

	select @alpha = @F_Period * (@F_Period + @F_One) / (@F_Two)
	--Weighted Moving Average of Closing
	select @WMAClosing = @TotalWeightedNormalClosing / @alpha
	--Weighted Moving Average of Weighted Closing
	select @WMAWeightedClosing = @TotalWeightedWeightedClosing / @alpha
	--Weighted Moving Average of Typical Closing
	select @WMATypicalClosing = @TotalWeightedTypicalClosing / @alpha

	select @alpha = @F_Two / (@F_Period + 1)
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
	select @RSI_SMAClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Simple Moving Average of Volume
	select @alpha = case when @SMALossVolume = 0 then 0 else @SMAGainVolume / @SMALossVolume end + @F_One
	select @RSI_SMAVolume = @F_Hundred - @F_Hundred / @alpha
	--RSI - Simple Moving Average of Weighted Closing
	select @alpha = case when @SMALossWeightedClosing = 0 then 0 else @SMAGainWeightedClosing / @SMALossWeightedClosing end + @F_One
	select @RSI_SMAWeightedClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Simple Moving Average of Typical Closing
	select @alpha = case when @SMALossTypicalClosing = 0 then 0 else @SMAGainTypicalClosing / @SMALossTypicalClosing end + @F_One
	select @RSI_SMATypicalClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Simple Moving Average of Volume Adjusted Closing
	select @alpha = case when @SMALossVolumeAdjustedClosing = 0 then 0 else @SMAGainVolumeAdjustedClosing / @SMALossVolumeAdjustedClosing end + @F_One
	select @RSI_SMAVolumeAdjustedClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Simple Moving Average of Volume Adjusted Weighted Closing
	select @alpha = case when @SMALossVolumeAdjustedWeightedClosing = 0 then 0 else @SMAGainVolumeAdjustedWeightedClosing / @SMALossVolumeAdjustedWeightedClosing end + @F_One
	select @RSI_SMAVolumeAdjustedWeightedClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Simple Moving Average of Volume Adjusted Typical Closing
	select @alpha = case when @SMALossVolumeAdjustedTypicalClosing = 0 then 0 else @SMAGainVolumeAdjustedTypicalClosing / @SMALossVolumeAdjustedTypicalClosing end + @F_One
	select @RSI_SMAVolumeAdjustedTypicalClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Exponential Moving Average of Closing
	select @alpha = case when @EMALossClosing = 0 then 0 else @EMAGainClosing / @EMALossClosing end + @F_One
	select @RSI_EMAClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Exponential Moving Average of Volume
	select @alpha = case when @EMALossVolume = 0 then 0 else @EMAGainVolume / @EMALossVolume end + @F_One
	select @RSI_EMAVolume = @F_Hundred - @F_Hundred / @alpha
	--RSI - Exponential Moving Average of Weighted Closing
	select @alpha = case when @EMALossWeightedClosing = 0 then 0 else @EMAGainWeightedClosing / @EMALossWeightedClosing end + @F_One
	select @RSI_EMAWeightedClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Exponential Moving Average of Typical Closing
	select @alpha = case when @EMALossTypicalClosing = 0 then 0 else @EMAGainTypicalClosing / @EMALossTypicalClosing end + @F_One
	select @RSI_EMATypicalClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Exponential Moving Average of Volume Adjusted Closing
	select @alpha = case when @EMALossVolumeAdjustedClosing = 0 then 0 else @EMAGainVolumeAdjustedClosing / @EMALossVolumeAdjustedClosing end + @F_One
	select @RSI_EMAVolumeAdjustedClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Exponential Moving Average of Volume Adjusted Weighted Closing
	select @alpha = case when @EMALossVolumeAdjustedWeightedClosing = 0 then 0 else @EMAGainVolumeAdjustedWeightedClosing / @EMALossVolumeAdjustedWeightedClosing end + @F_One
	select @RSI_EMAVolumeAdjustedWeightedClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Exponential Moving Average of Volume Adjusted Typical Closing
	select @alpha = case when @EMALossVolumeAdjustedTypicalClosing = 0 then 0 else @EMAGainVolumeAdjustedTypicalClosing / @EMALossVolumeAdjustedTypicalClosing end + @F_One
	select @RSI_EMAVolumeAdjustedTypicalClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Wilder's Smooth of Closing
	select @alpha = case when @WSLossClosing = 0 then 0 else @WSGainClosing / @WSLossClosing end + @F_One
	select @RSI_WSClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Wilder's Smooth of Volume
	select @alpha = case when @WSLossVolume = 0 then 0 else @WSGainVolume / @WSLossVolume end + @F_One
	select @RSI_WSVolume = @F_Hundred - @F_Hundred / @alpha
	--RSI - Wilder's Smooth of Weighted Closing
	select @alpha = case when @WSLossWeightedClosing = 0 then 0 else @WSGainWeightedClosing / @WSLossWeightedClosing end + @F_One
	select @RSI_WSWeightedClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Wilder's Smooth of Typical Closing
	select @alpha = case when @WSLossTypicalClosing = 0 then 0 else @WSGainTypicalClosing / @WSLossTypicalClosing end + @F_One
	select @RSI_WSTypicalClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Wilder's Smooth of Volume Adjusted Closing
	select @alpha = case when @WSLossVolumeAdjustedClosing = 0 then 0 else @WSGainVolumeAdjustedClosing / @WSLossVolumeAdjustedClosing end + @F_One
	select @RSI_WSVolumeAdjustedClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Wilder's Smooth of Volume Adjusted Weighted Closing
	select @alpha = case when @WSLossVolumeAdjustedWeightedClosing = 0 then 0 else @WSGainVolumeAdjustedWeightedClosing / @WSLossVolumeAdjustedWeightedClosing end + @F_One
	select @RSI_WSVolumeAdjustedWeightedClosing = @F_Hundred - @F_Hundred / @alpha
	--RSI - Wilder's Smooth of Volume Adjusted Typical Closing
	select @alpha = case when @WSLossVolumeAdjustedTypicalClosing = 0 then 0 else @WSGainVolumeAdjustedTypicalClosing / @WSLossVolumeAdjustedTypicalClosing end + @F_One
	select @RSI_WSVolumeAdjustedTypicalClosing = @F_Hundred - @F_Hundred / @alpha

	insert into @ret(ID, Value, Description) values(10, @DateFrom,'Date From')
	insert into @ret(ID, Value, Description) values(20, @DateTo,'Date to')
	insert into @ret(ID, Value, Description) values(30, @Opening,'The opening price of the period')
	insert into @ret(ID, Value, Description) values(40, @High,'The highest price in the period')
	insert into @ret(ID, Value, Description) values(50, @Low,'The lowest price in the period')
	insert into @ret(ID, Value, Description) values(60, @Closing,'The last price of the period')
	insert into @ret(ID, Value, Description) values(70, @Volume,'Total volume inthe period')
	insert into @ret(ID, Value, Description) values(1010, @TotalClosing,'Total of Closing')
	insert into @ret(ID, Value, Description) values(1020, @TotalWeightedClosing,'Total of Weighted Closing')
	insert into @ret(ID, Value, Description) values(1030, @TotalTypicalClosing,'Total of Typical Closing')
	insert into @ret(ID, Value, Description) values(1040, @TotalWeightedNormalClosing,'Total of Weighted Closing -- used for WMA')
	insert into @ret(ID, Value, Description) values(1050, @TotalWeightedWeightedClosing,'Total of Weighted Weighted Closing -- used for WMA')
	insert into @ret(ID, Value, Description) values(1060, @TotalWeightedTypicalClosing,'Total of Weighted Typical Closing -- used for WMA')
	insert into @ret(ID, Value, Description) values(1070, @TotalVolume,'Total of Volume')
	insert into @ret(ID, Value, Description) values(1080, @TotalVolumeAdjustedClosing,'Total of Volume Adjusted Closing -- use for VAMA')
	insert into @ret(ID, Value, Description) values(1090, @TotalVolumeAdjustedTypicalClosing,'Total of Volume Adjusted Typical Closing -- use for VAMA')
	insert into @ret(ID, Value, Description) values(1100, @TotalVolumeAdjustedWeightedClosing,'Total of Volume Adjusted Weighted Closing -- use for VAMA')
	insert into @ret(ID, Value, Description) values(1110, @TotalGainClosing,'Total Gain by comparing Closing')
	insert into @ret(ID, Value, Description) values(1120, @TotalLossClosing,'Total Loss by comparing Closing')
	insert into @ret(ID, Value, Description) values(1130, @TotalGainVolume,'Total Gain by comparing Volume')
	insert into @ret(ID, Value, Description) values(1140, @TotalLossVolume,'Total Loss by comparing Closing')
	insert into @ret(ID, Value, Description) values(1150, @TotalGainWeightedClosing,'Total Gain by comparing Weighted Closing')
	insert into @ret(ID, Value, Description) values(1160, @TotalLossWeightedClosing,'Total Loss by comparing Weighted Closing')
	insert into @ret(ID, Value, Description) values(1170, @TotalGainTypicalClosing,'Total Gain by comparing Typical Closing')
	insert into @ret(ID, Value, Description) values(1180, @TotalLossTypicalClosing,'Total Loss by comparing Typical Closing')
	insert into @ret(ID, Value, Description) values(1190, @TotalGainVolumeAdjustedClosing,'Total Gain by comparing Volume Adjusted Closing')
	insert into @ret(ID, Value, Description) values(1200, @TotalLossVolumeAdjustedClosing,'Total Loss by comparing Volume Adjusted Closing')
	insert into @ret(ID, Value, Description) values(1210, @TotalGainVolumeAdjustedWeightedClosing,'Total Gain by comparing Volume Adjusted Weighted Closing')
	insert into @ret(ID, Value, Description) values(1220, @TotalLossVolumeAdjustedWeightedClosing,'Total Loss by comparing Volume Adjusted Weighted Closing')
	insert into @ret(ID, Value, Description) values(1230, @TotalGainVolumeAdjustedTypicalClosing,'Total Gain by comparing Volume Adjusted Typical Closing')
	insert into @ret(ID, Value, Description) values(1240, @TotalLossVolumeAdjustedTypicalClosing,'Total Loss by comparing VolumeAdjustedTypicalClosing')
	insert into @ret(ID, Value, Description) values(2010, @SMAClosing,'Simple Moving Average of Closing')
	insert into @ret(ID, Value, Description) values(2020, @SMAWeightedClosing,'Simple Moving Average of Weighted Closing')
	insert into @ret(ID, Value, Description) values(2030, @SMATypicalClosing,'Simple Moving Average of Typical Closing')
	insert into @ret(ID, Value, Description) values(2070, @SMAVolume,'Simple Moving Average of Volume')
	insert into @ret(ID, Value, Description) values(2110, @SMAGainClosing,'Simple Moving Average of Gain by comparing Closing')
	insert into @ret(ID, Value, Description) values(2120, @SMALossClosing,'Simple Moving Average of Loss by comparing Closing')
	insert into @ret(ID, Value, Description) values(2130, @SMAGainVolume,'Simple Moving Average of Gain by comparing Volume')
	insert into @ret(ID, Value, Description) values(2140, @SMALossVolume,'Simple Moving Average of Loss by comparing Closing')
	insert into @ret(ID, Value, Description) values(2150, @SMAGainWeightedClosing,'Simple Moving Average of Gain by comparing Weighted Closing')
	insert into @ret(ID, Value, Description) values(2160, @SMALossWeightedClosing,'Simple Moving Average of Loss by comparing Weighted Closing')
	insert into @ret(ID, Value, Description) values(2170, @SMAGainTypicalClosing,'Simple Moving Average of Gain by comparing Typical Closing')
	insert into @ret(ID, Value, Description) values(2180, @SMALossTypicalClosing,'Simple Moving Average of Loss by comparing Typical Closing')
	insert into @ret(ID, Value, Description) values(2190, @SMAGainVolumeAdjustedClosing,'Simple Moving Average of Gain by comparing Volume Adjusted Closing')
	insert into @ret(ID, Value, Description) values(2200, @SMALossVolumeAdjustedClosing,'Simple Moving Average of Loss by comparing Volume Adjusted Closing')
	insert into @ret(ID, Value, Description) values(2210, @SMAGainVolumeAdjustedWeightedClosing,'Simple Moving Average of Gain by comparing Volume Adjusted Weighted Closing')
	insert into @ret(ID, Value, Description) values(2220, @SMALossVolumeAdjustedWeightedClosing,'Simple Moving Average of Loss by comparing Volume Adjusted Weighted Closing')
	insert into @ret(ID, Value, Description) values(2230, @SMAGainVolumeAdjustedTypicalClosing,'Simple Moving Average of Gain by comparing Volume Adjusted Typical Closing')
	insert into @ret(ID, Value, Description) values(2240, @SMALossVolumeAdjustedTypicalClosing,'Simple Moving Average of Loss by comparing VolumeAdjustedTypicalClosing')
	insert into @ret(ID, Value, Description) values(3080, @VAMAClosing,'Volume Adjusted Moving Average of Closing')
	insert into @ret(ID, Value, Description) values(3090, @VAMATypicalClosing,'Volume Adjusted Moving Average of Typical Closing')
	insert into @ret(ID, Value, Description) values(3100, @VAMAWeightedClosing,'Volume Adjusted Moving Average of Weighted Closing')
	insert into @ret(ID, Value, Description) values(4040, @WMAClosing,'Weighted Moving Average of Closing')
	insert into @ret(ID, Value, Description) values(4050, @WMAWeightedClosing,'Weighted Moving Average of Weighted Closing')
	insert into @ret(ID, Value, Description) values(4060, @WMATypicalClosing,'Weighted Moving Average of Typical Closing')
	insert into @ret(ID, Value, Description) values(5010, @EMAClosing,'Exponential Moving Average of Closing')
	insert into @ret(ID, Value, Description) values(5020, @EMAWeightedClosing,'Exponential Moving Average of Weighted Closing')
	insert into @ret(ID, Value, Description) values(5030, @EMATypicalClosing,'Exponential Moving Average of Typical Closing')
	insert into @ret(ID, Value, Description) values(5070, @EMAVolume,'Exponential Moving Average of Volume')
	insert into @ret(ID, Value, Description) values(5110, @EMAGainClosing,'Exponential Moving Average of Gain by comparing Closing')
	insert into @ret(ID, Value, Description) values(5120, @EMALossClosing,'Exponential Moving Average of Loss by comparing Closing')
	insert into @ret(ID, Value, Description) values(5130, @EMAGainVolume,'Exponential Moving Average of Gain by comparing Volume')
	insert into @ret(ID, Value, Description) values(5140, @EMALossVolume,'Exponential Moving Average of Loss by comparing Closing')
	insert into @ret(ID, Value, Description) values(5150, @EMAGainWeightedClosing,'Exponential Moving Average of Gain by comparing Weighted Closing')
	insert into @ret(ID, Value, Description) values(5160, @EMALossWeightedClosing,'Exponential Moving Average of Loss by comparing Weighted Closing')
	insert into @ret(ID, Value, Description) values(5170, @EMAGainTypicalClosing,'Exponential Moving Average of Gain by comparing Typical Closing')
	insert into @ret(ID, Value, Description) values(5180, @EMALossTypicalClosing,'Exponential Moving Average of Loss by comparing Typical Closing')
	insert into @ret(ID, Value, Description) values(5190, @EMAGainVolumeAdjustedClosing,'Exponential Moving Average of Gain by comparing Volume Adjusted Closing')
	insert into @ret(ID, Value, Description) values(5200, @EMALossVolumeAdjustedClosing,'Exponential Moving Average of Loss by comparing Volume Adjusted Closing')
	insert into @ret(ID, Value, Description) values(5210, @EMAGainVolumeAdjustedWeightedClosing,'Exponential Moving Average of Gain by comparing Volume Adjusted Weighted Closing')
	insert into @ret(ID, Value, Description) values(5220, @EMALossVolumeAdjustedWeightedClosing,'Exponential Moving Average of Loss by comparing Volume Adjusted Weighted Closing')
	insert into @ret(ID, Value, Description) values(5230, @EMAGainVolumeAdjustedTypicalClosing,'Exponential Moving Average of Gain by comparing Volume Adjusted Typical Closing')
	insert into @ret(ID, Value, Description) values(5240, @EMALossVolumeAdjustedTypicalClosing,'Exponential Moving Average of Loss by comparing VolumeAdjustedTypicalClosing')
	insert into @ret(ID, Value, Description) values(6110, @CMOClosing,'Chande Momentum Oscillator of Closing')
	insert into @ret(ID, Value, Description) values(6130, @CMOVolume,'Chande Momentum Oscillator of Volume')
	insert into @ret(ID, Value, Description) values(6150, @CMOWeightedClosing,'Chande Momentum Oscillator of Weighted Closing')
	insert into @ret(ID, Value, Description) values(6170, @CMOTypicalClosing,'Chande Momentum Oscillator of Typical Closing')
	insert into @ret(ID, Value, Description) values(7110, @VMAClosing,'Variable Moving Average of Closing')
	insert into @ret(ID, Value, Description) values(7130, @VMAVolume,'Variable Moving Average of Volume')
	insert into @ret(ID, Value, Description) values(7150, @VMAWeightedClosing,'Variable Moving Average of Weighted Closing')
	insert into @ret(ID, Value, Description) values(7170, @VMATypicalClosing,'Variable Moving Average of Typical Closing')
	insert into @ret(ID, Value, Description) values(8010, @WSClosing,'Wilder''s Smooth  Closing')
	insert into @ret(ID, Value, Description) values(8020, @WSWeightedClosing,'Wilder''s Smooth  Weighted Closing')
	insert into @ret(ID, Value, Description) values(8030, @WSTypicalClosing,'Wilder''s Smooth  Typical Closing')
	insert into @ret(ID, Value, Description) values(8040, @WSWeightedNormalClosing,'Wilder''s Smooth  Weighted Closing -- used for WMA')
	insert into @ret(ID, Value, Description) values(8050, @WSWeightedWeightedClosing,'Wilder''s Smooth  Weighted Weighted Closing -- used for WMA')
	insert into @ret(ID, Value, Description) values(8060, @WSWeightedTypicalClosing,'Wilder''s Smooth  Weighted Typical Closing -- used for WMA')
	insert into @ret(ID, Value, Description) values(8070, @WSVolume,'Wilder''s Smooth  Volume')
	insert into @ret(ID, Value, Description) values(8080, @WSVolumeAdjustedClosing,'Wilder''s Smooth  Volume Adjusted Closing -- use for VAMA')
	insert into @ret(ID, Value, Description) values(8090, @WSVolumeAdjustedTypicalClosing,'Wilder''s Smooth  Volume Adjusted Typical Closing -- use for VAMA')
	insert into @ret(ID, Value, Description) values(8100, @WSVolumeAdjustedWeightedClosing,'Wilder''s Smooth  Volume Adjusted Weighted Closing -- use for VAMA')
	insert into @ret(ID, Value, Description) values(8110, @WSGainClosing,'Wilder''s Smooth of Gain by comparing Closing')
	insert into @ret(ID, Value, Description) values(8120, @WSLossClosing,'Wilder''s Smooth of Loss by comparing Closing')
	insert into @ret(ID, Value, Description) values(8130, @WSGainVolume,'Wilder''s Smooth of Gain by comparing Volume')
	insert into @ret(ID, Value, Description) values(8140, @WSLossVolume,'Wilder''s Smooth of Loss by comparing Closing')
	insert into @ret(ID, Value, Description) values(8150, @WSGainWeightedClosing,'Wilder''s Smooth of Gain by comparing Weighted Closing')
	insert into @ret(ID, Value, Description) values(8160, @WSLossWeightedClosing,'Wilder''s Smooth of Loss by comparing Weighted Closing')
	insert into @ret(ID, Value, Description) values(8170, @WSGainTypicalClosing,'Wilder''s Smooth of Gain by comparing Typical Closing')
	insert into @ret(ID, Value, Description) values(8180, @WSLossTypicalClosing,'Wilder''s Smooth of Loss by comparing Typical Closing')
	insert into @ret(ID, Value, Description) values(8190, @WSGainVolumeAdjustedClosing,'Wilder''s Smooth of Gain by comparing Volume Adjusted Closing')
	insert into @ret(ID, Value, Description) values(8200, @WSLossVolumeAdjustedClosing,'Wilder''s Smooth of Loss by comparing Volume Adjusted Closing')
	insert into @ret(ID, Value, Description) values(8210, @WSGainVolumeAdjustedWeightedClosing,'Wilder''s Smooth of Gain by comparing Volume Adjusted Weighted Closing')
	insert into @ret(ID, Value, Description) values(8220, @WSLossVolumeAdjustedWeightedClosing,'Wilder''s Smooth of Loss by comparing Volume Adjusted Weighted Closing')
	insert into @ret(ID, Value, Description) values(8230, @WSGainVolumeAdjustedTypicalClosing,'Wilder''s Smooth of Gain by comparing Volume Adjusted Typical Closing')
	insert into @ret(ID, Value, Description) values(8240, @WSLossVolumeAdjustedTypicalClosing,'Wilder''s Smooth of Loss by comparing VolumeAdjustedTypicalClosing')
	insert into @ret(ID, Value, Description) values(12110, @RSI_SMAClosing,'RSI - Simple Moving Average of Closing')
	insert into @ret(ID, Value, Description) values(12130, @RSI_SMAVolume,'RSI - Simple Moving Average of Volume')
	insert into @ret(ID, Value, Description) values(12150, @RSI_SMAWeightedClosing,'RSI - Simple Moving Average of Weighted Closing')
	insert into @ret(ID, Value, Description) values(12170, @RSI_SMATypicalClosing,'RSI - Simple Moving Average of Typical Closing')
	insert into @ret(ID, Value, Description) values(12190, @RSI_SMAVolumeAdjustedClosing,'RSI - Simple Moving Average of Volume Adjusted Closing')
	insert into @ret(ID, Value, Description) values(12210, @RSI_SMAVolumeAdjustedWeightedClosing,'RSI - Simple Moving Average of Volume Adjusted Weighted Closing')
	insert into @ret(ID, Value, Description) values(12230, @RSI_SMAVolumeAdjustedTypicalClosing,'RSI - Simple Moving Average of Volume Adjusted Typical Closing')
	insert into @ret(ID, Value, Description) values(15110, @RSI_EMAClosing,'RSI - Exponential Moving Average of Closing')
	insert into @ret(ID, Value, Description) values(15130, @RSI_EMAVolume,'RSI - Exponential Moving Average of Volume')
	insert into @ret(ID, Value, Description) values(15150, @RSI_EMAWeightedClosing,'RSI - Exponential Moving Average of Weighted Closing')
	insert into @ret(ID, Value, Description) values(15170, @RSI_EMATypicalClosing,'RSI - Exponential Moving Average of Typical Closing')
	insert into @ret(ID, Value, Description) values(15190, @RSI_EMAVolumeAdjustedClosing,'RSI - Exponential Moving Average of Volume Adjusted Closing')
	insert into @ret(ID, Value, Description) values(15210, @RSI_EMAVolumeAdjustedWeightedClosing,'RSI - Exponential Moving Average of Volume Adjusted Weighted Closing')
	insert into @ret(ID, Value, Description) values(15230, @RSI_EMAVolumeAdjustedTypicalClosing,'RSI - Exponential Moving Average of Volume Adjusted Typical Closing')
	insert into @ret(ID, Value, Description) values(18110, @RSI_WSClosing,'RSI - Wilder''s Smooth of Closing')
	insert into @ret(ID, Value, Description) values(18130, @RSI_WSVolume,'RSI - Wilder''s Smooth of Volume')
	insert into @ret(ID, Value, Description) values(18150, @RSI_WSWeightedClosing,'RSI - Wilder''s Smooth of Weighted Closing')
	insert into @ret(ID, Value, Description) values(18170, @RSI_WSTypicalClosing,'RSI - Wilder''s Smooth of Typical Closing')
	insert into @ret(ID, Value, Description) values(18190, @RSI_WSVolumeAdjustedClosing,'RSI - Wilder''s Smooth of Volume Adjusted Closing')
	insert into @ret(ID, Value, Description) values(18210, @RSI_WSVolumeAdjustedWeightedClosing,'RSI - Wilder''s Smooth of Volume Adjusted Weighted Closing')
	insert into @ret(ID, Value, Description) values(18230, @RSI_WSVolumeAdjustedTypicalClosing,'RSI - Wilder''s Smooth of Volume Adjusted Typical Closing')
	
	return
end