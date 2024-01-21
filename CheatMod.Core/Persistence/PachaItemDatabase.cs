using System.Collections.Generic;
using SodaDen.Pacha;

namespace CheatMod.Core.Persistence;

public class PachaItemDatabase
{
    public readonly List<InventoryItem> InventoryItems = new();

    public void Refresh()
    {
        RefreshDatabaseInventoryItems();
    }

    private void RefreshDatabaseInventoryItems()
    {
        InventoryItems.Clear();
        InventoryItems.AddRange(GetDatabaseInventoryItems());
    }

    private static IEnumerable<InventoryItem> GetDatabaseInventoryItems()
    {
        var inventoryItems = new List<InventoryItem>();

        for (short i = 0; i < short.MaxValue; i++)
        {
            var we = SodaDen.Pacha.Database.Instance[i];
            if (we == null) continue;

            if (we is InventoryItem item) inventoryItems.Add(item);
        }

        return inventoryItems;
    }
}