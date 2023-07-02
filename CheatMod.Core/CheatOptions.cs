namespace CheatMod.Core;

public class CheatOptions
{
    #region Game options
    public static bool IsEasyFishingEnabled { get; set; } = true;
    public static bool IsInfiniteFluteEnabled { get; set; } = true;
    public static bool IsEasyAnimalsEnabled { get; set; } = true;
    public static bool IsInfiniteSeedsEnabled { get; set; } = true;
    public static bool IsInfiniteStaminaEnabled { get; set; } = true;
    public static bool IsInfiniteWaterToolEnabled { get; set; } = true;
    public static bool IsFreezeTimeEnabled { get; set; } = false;
    public static bool IsMovementSpeedEnabled { get; set; } = true;
    public static float PlayerMovementSpeed { get; set; } = 4f;
    public static bool IsInfiniteHarvestEnabled { get; set; } = false;

    #endregion

    #region UI Options
    public static bool DrawUI { get; set; } = true;
    public static bool DrawItemSpawnerWindow { get; set; }
    public static bool DrawTimeManagerWindow { get; set; }
    public static bool DrawTeleportWindow { get; set; }

    #endregion
}