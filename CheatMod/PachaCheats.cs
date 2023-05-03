using System;
using System.Linq;
using System.Reflection;
using MelonLoader;
using Photon.Pun;
using SodaDen.Pacha;
using UnityAtoms;
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
        }

        MelonLogger.Msg($"Watered {wateredAmount} tiles");
    }


    public static void SetTime(TimeSpan time)
    {
        try
        {
            MelonLogger.Msg($"Trying to set {time.Hours:00}:{time.Minutes:00}");
            var session = GameObject.FindObjectOfType<Session>();

            var dayTimeField =
                typeof(Session).GetField("DayTime", BindingFlags.NonPublic | BindingFlags.Instance);
            var offsetTimeField =
                typeof(Session).GetField("OffsetTime", BindingFlags.NonPublic | BindingFlags.Instance);
            var dayStartTimeField =
                typeof(Session).GetField("DayStartTime", BindingFlags.NonPublic | BindingFlags.Instance);

            var dayTime = (FloatVariable)dayTimeField.GetValue(session);

            var serverTimestamp = PhotonNetwork.ServerTimestamp;

            dayTime.Value = time.Hours + time.Minutes / 60;
            offsetTimeField!.SetValue(session, 0f);
            dayStartTimeField!.SetValue(session, serverTimestamp);
            
            MelonLogger.Msg($"Time set to: {time.Hours:00}:{time.Minutes:00}");
        }
        catch (Exception ex)
        {
            MelonLogger.Error("Couldn't set time due to " + ex.Message);
        }
    }
}