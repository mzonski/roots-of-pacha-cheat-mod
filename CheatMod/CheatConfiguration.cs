namespace RootsOfPachaCheatMod;

public class CheatConfiguration
{
    #region Game options
    public bool IsEasyFishingEnabled { get; set; } = true;
    public bool IsInfiniteFluteEnabled { get; set; } = true;
    public bool IsInfiniteSeedsEnabled { get; set; } = true;
    public bool IsInfiniteStaminaEnabled { get; set; } = true;
    public bool IsInfiniteWaterToolEnabled { get; set; } = true;
    public bool IsFreezeTimeEnabled { get; set; } = false;
    public bool IsMovementSpeedEnabled { get; set; } = true;
    public float PlayerMovementSpeed { get; set; } = 4f;

    #endregion

    #region UI Options
    public bool DrawUI { get; set; } = true;
    public bool DrawItemSpawnerWindow { get; set; }
    public bool DrawTimeManagerWindow { get; set; }

    #endregion
}