using System.Collections.Generic;
using SodaDen.Pacha;

namespace RootsOfPachaCheatMod;

public class PachaItemDb
{
    public readonly List<InventoryItem> InventoryItems = new();

    public void Refresh()
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        InventoryItems.Clear();
        InventoryItems.AddRange(PachaUtils.GetInventoryItems());
    }
}