using CheatMod.Core.Managers;
using SodaDen.Pacha;
using UnityEngine;

namespace CheatMod.Core.CheatCommands.AddItemToInventory;

public class AddItemToInventoryCommandExecutor : CheatCommandExecutor<AddItemToInventoryCommand>
{
    public AddItemToInventoryCommandExecutor(PachaManager manager) : base(manager)
    {
    }
    
    private void AddItemToInventory(short itemId, int qty, ItemQuality quality = ItemQuality.Normal)
    {
        var it = (InventoryItem)SodaDen.Pacha.Database.Instance[itemId];
        var pen = GameObject.FindObjectOfType<PlayerEntity>();

        if (it is SeedItem)
            pen.SeedInventory.AddItem(pen, it, ItemObtainedSource.Picked, qty);
        else
        {
            var item = new InventoryItemWithProperties
            {
                Item = it,
                Quality = quality
            };
            pen.Inventory.AddItem(item, qty);
        }

        Manager.Logger.Log($"Item {it.Name}[{it.ID}] added (x{qty})");
    }

    public override void Execute(AddItemToInventoryCommand command)
    {
        AddItemToInventory(command.ItemId, command.Qty, command.Quality);
    }
}