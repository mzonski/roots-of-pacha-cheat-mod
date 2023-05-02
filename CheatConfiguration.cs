namespace RootsOfPachaCheatMod;

public class CheatConfiguration
{
    public bool IsEasyFishingEnabled { get; set; } = true;
    public bool IsInfiniteFluteEnabled { get; set; } = true;
    public bool IsInfiniteSeedsEnabled { get; set; } = true;
    public bool IsInfiniteStaminaEnabled { get; set; } = true;
    public bool IsInfiniteWaterToolEnabled { get; set; } = true;
    public bool IsFreezeTimeEnabled { get; set; } = false;
    public bool IsMovementSpeedEnabled { get; set; } = false;
    public float PlayerMovementSpeed { get; set; } = 2f;
    public bool DrawUI { get; set; }
    public bool ItemSpawnerWindowOpen { get; set; }
}