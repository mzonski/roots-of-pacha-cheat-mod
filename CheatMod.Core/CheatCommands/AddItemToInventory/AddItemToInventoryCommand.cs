using SodaDen.Pacha;

namespace CheatMod.Core.CheatCommands.AddItemToInventory;

public class AddItemToInventoryCommand : ICheatCommand
{
    public short ItemId { get; set; }
    public int Qty { get; set; }
    public ItemQuality Quality { get; set; } = ItemQuality.Normal;
}