using SodaDen.Pacha;

namespace CheatMod.Core.CheatCommands.ShuffleAnimalHerd;

public class ShuffleAnimalHerdCommand : ICheatCommand
{
    public float Range { get; set; } = 3f;

    public Rarity? Rarity { get; set; } = null;
    public Sex? Sex { get; set; } = null;
    public bool? IsAdult { get; set; } = true;
}