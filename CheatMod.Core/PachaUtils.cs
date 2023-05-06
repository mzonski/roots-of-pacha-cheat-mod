using System.Collections.Generic;
using System.Reflection;
//using MelonLoader;
using SodaDen.Pacha;
using SodaDen.Pacha.UI;
using UnityEngine;

namespace CheatMod.Core;

public static class PachaUtils
{
    // Get current standing tile = TilesManager.Instance.TileAt(GetPlayerCurrentCoords())
    public static Vector2 GetPlayerCurrentCoords()
    {
        var player = GameObject.FindObjectOfType<PlayerEntity>();
       // MelonLogger.Msg(
       //     $"[{player.Name}] x: {player.PlayerStateController.PositionWithDirection.x} y: {player.PlayerStateController.PositionWithDirection.y}");
        return player.PlayerStateController.PositionWithDirection;
    }

    public static IEnumerable<InventoryItem> GetInventoryItems()
    {
        var inventoryItems = new List<InventoryItem>();

        for (short i = 0; i < short.MaxValue; i++)
        {
            var we = Database.Instance[i];
            if (we == null) continue;

            if (we is InventoryItem item) inventoryItems.Add(item);
        }

        return inventoryItems;
    }

    public static void DumpEntities()
    {
        var inventoryItems = new List<InventoryItem>();
        var types = new HashSet<string>();
        for (short i = 0; i < short.MaxValue; i++)
        {
            var we = Database.Instance[i];
            if (we == null) continue;

            if (we is InventoryItem item) inventoryItems.Add(item);

            var typ = we.GetType();
            if (!types.Contains(typ.ToString())) types.Add(typ.ToString());

            //MelonLogger.Msg("Entity: " + we.name + " [-] " + we.ID + " [-] " + we.GetType());
        }

        //MelonLogger.Msg("Inventory items");
        //foreach (var inventoryItem in inventoryItems)
        //    MelonLogger.Msg(inventoryItem.ID + "  " + inventoryItem.Name + "  " + inventoryItem.Description);

        //MelonLogger.Msg("All types");
        //foreach (var type in types) MelonLogger.Msg(type);
    }
    
    public static void SkipDisclaimerIntros()
    {
        var dsc = Object.FindObjectOfType<Disclaimer>();

        if (dsc == null)
        {
            return;
        }

        dsc.OnAllLogos();
        dsc.OnSplashEnded();

        var currentIndexProperty = typeof(Disclaimer).GetProperty("CurrentIndex", BindingFlags.NonPublic | BindingFlags.Instance);
        currentIndexProperty?.SetValue(dsc, 0);
    }


    public static int NormalizeQty(int val)
    {
        return val switch
        {
            < 0 => 0,
            > 255 => 255,
            _ => val
        };
    }
}