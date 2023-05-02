using System.Linq;
using System.Reflection;
using MelonLoader;
using SodaDen.Pacha;
using UnityEngine;

namespace RootsOfPachaCheatMod;

public static class PachaCheats
{
    public static void AddItemToInventory(short itemId, int qty)
    {
        var it = (InventoryItem)Database.Instance[itemId];
        var pen = GameObject.FindObjectOfType<PlayerEntity>();

        if (it is SeedItem)
            pen.SeedInventory.AddItem(pen, it, ItemObtainedSource.Picked, qty);
        else
            pen.Inventory.AddItem(it, qty);

        MelonLogger.Msg($"Item {it.Name}[{it.ID}] added (x{qty})");
    }

    // Use it with caution. Generates a lot of stuff
    public static void ForceRegenerateHittableResources()
    {
        var player = GameObject.FindObjectOfType<PlayerEntity>();

        var method = typeof(TilesManager).GetMethod("RegenerateHittableResources",
            BindingFlags.NonPublic | BindingFlags.Instance);

        method?.Invoke(TilesManager.Instance, new object[] { player.CurrentDay, true });
    }

    public static void AddDayBuff(PlayerStatBuffType type, int value)
    {
        var stats = GameObject.FindObjectOfType<PlayerStats>();
        var buff = PlayerStatBuff.CreateForDays(PlayerStatBuffSource.PassedOut, type, value, 1);
        stats.AddBuffLocal(buff);
    }

    public static void WaterAllTilledTiles()
    {
        var tiles = TilesManager.Instance.AllTiles;
        var wateredAmount = 0;
        foreach (var tile in tiles)
        {
            if (tile is TillableTile { Stage: TillableStage.Tilled } tilledTile)
            {
                tilledTile.SetStage(TillableStage.TilledWet, null, false, null,
                    Network.PlayerList.First(x => x.IsMasterClient), true);
                wateredAmount++;
            }

            MelonLogger.Msg($"Watered {wateredAmount} tiles");
        }
    }
}