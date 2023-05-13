using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CheatMod.Core.Services;
using Photon.Pun;
using SodaDen.Pacha;
using UnityAtoms;
using UnityEngine;

namespace CheatMod.Core;

public class PachaCheats
{
    private readonly IModLogger _logger;

    public PachaCheats(IModLogger logger)
    {
        _logger = logger;
    }

    public void AddItemToInventory(short itemId, int qty)
    {
        var it = (InventoryItem)Database.Instance[itemId];
        var pen = GameObject.FindObjectOfType<PlayerEntity>();

        if (it is SeedItem)
            pen.SeedInventory.AddItem(pen, it, ItemObtainedSource.Picked, qty);
        else
            pen.Inventory.AddItem(it, qty);

        _logger.Log($"Item {it.Name}[{it.ID}] added (x{qty})");
    }

    // Use it with caution. Generates a lot of stuff
    public void ForceRegenerateHittableResources()
    {
        var player = GameObject.FindObjectOfType<PlayerEntity>();

        var method = typeof(TilesManager).GetMethod("RegenerateHittableResources",
            BindingFlags.NonPublic | BindingFlags.Instance);

        method?.Invoke(TilesManager.Instance, new object[] { player.CurrentDay, true });
    }

    public void AddDayBuff(PlayerStatBuffType type, int value)
    {
        var stats = GameObject.FindObjectOfType<PlayerStats>();
        var buff = PlayerStatBuff.CreateForDays(PlayerStatBuffSource.PassedOut, type, value, 1);
        stats.AddBuffLocal(buff);
    }

    public void WaterAllTilledTiles()
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

        _logger.Log($"Watered {wateredAmount} tiles");
    }


    public void SetTime(TimeSpan time)
    {
        try
        {
            _logger.Log($"Trying to set {time.Hours:00}:{time.Minutes:00}");
            var session = GameObject.FindObjectOfType<Session>();

            var dayTimeField =
                typeof(Session).GetField("DayTime", BindingFlags.NonPublic | BindingFlags.Instance);
            var offsetTimeField =
                typeof(Session).GetField("OffsetTime", BindingFlags.NonPublic | BindingFlags.Instance);
            var dayStartTimeField =
                typeof(Session).GetField("DayStartTime", BindingFlags.NonPublic | BindingFlags.Instance);

            var dayTime = (FloatVariable)dayTimeField!.GetValue(session);

            var serverTimestamp = PhotonNetwork.ServerTimestamp;

            dayTime.Value = time.Hours + time.Minutes / 60;
            offsetTimeField!.SetValue(session, 0f);
            dayStartTimeField!.SetValue(session, serverTimestamp);

            _logger.Log($"Time set to: {time.Hours:00}:{time.Minutes:00}");
        }
        catch (Exception ex)
        {
            _logger.Log("Couldn't set time due to " + ex.Message);
        }
    }

    private EntityManager RetrieveEntityManager()
    {
        var playerEntity = GameObject.FindObjectOfType<PlayerEntity>();
        var entityManagerField =
            typeof(PlayerEntity).GetField("EntityManager", BindingFlags.NonPublic | BindingFlags.Instance);
        var entityManager = (EntityManager)entityManagerField?.GetValue(playerEntity);

        if (entityManager is null)
        {
            _logger.Log("Entity manager is null");
        }

        return entityManager;
    }

    private void PatchPlantEntity(PlantEntity plantEntity)
    {
        var lastHarvestedProperty =
            typeof(PlantEntity).GetProperty("LastHarvested", BindingFlags.NonPublic | BindingFlags.Instance);
        var plantAgeProperty =
            typeof(PlantEntity).GetProperty("Age", BindingFlags.NonPublic | BindingFlags.Instance);
        var updateRendererMethod =
            typeof(PlantEntity).GetMethod("UpdateRenderer", BindingFlags.NonPublic | BindingFlags.Instance);
        var levelProperty =
            typeof(PlantEntity).GetProperty("LevelWhenSpawnedOrLastHarvested",
                BindingFlags.NonPublic | BindingFlags.Instance);

        if (lastHarvestedProperty == null || plantAgeProperty == null || updateRendererMethod == null ||
            levelProperty == null)
        {
            throw new ArgumentNullException(nameof(PlantEntity),
                "Plant entity data fields are initialized incorrectly");
        }

        levelProperty.SetValue(plantEntity, 2);
        plantEntity.Withered = false;
        if (CheatOptions.IsInfiniteHarvestEnabled)
        {
            lastHarvestedProperty.SetValue(plantEntity,
                null); // it allows to harvest plant in the same day but its glitched, not working
        }

        plantAgeProperty.SetValue(plantEntity, plantEntity.IsRepeating ? plantEntity.Plant.RepeatsEvery : 30f);


        updateRendererMethod.Invoke(plantEntity, null);
    }

    public void GrowCrops(float range = 3f)
    {
        try
        {
            _logger.Log("Grow crops");

            var plantsInRange = new List<PlantEntity>();
            var playerCoords = GetPlayerCurrentCoords();
            foreach (var entityData in Game.Current.Entities)
            {
                if (entityData.Type != EntityType.Plant) continue;

                var plantData = (PlantData)entityData;

                if (!plantData.Position.HasValue) continue;

                var distanceFromPlayer = Vector2.Distance(playerCoords, plantData.Position.Value);

                if (!(distanceFromPlayer < range)) continue;

                var plantEntity = GuidManager.ResolveGuid(entityData.ID).GetComponent<PlantEntity>();
                plantsInRange.Add(plantEntity);
            }

            _logger.Log($"[GrowCrops] Found {plantsInRange.Count} plants in range");

            foreach (var plantEntity in plantsInRange)
            {
                PatchPlantEntity(plantEntity);
            }
        }
        catch (Exception ex)
        {
            _logger.Log("[GrowCrops] Failed: " + ex.Message);
            _logger.Log(ex.StackTrace);
        }
    }

    // Get current standing tile = TilesManager.Instance.TileAt(GetPlayerCurrentCoords())
    public Vector2 GetPlayerCurrentCoords()
    {
        var player = GameObject.FindObjectOfType<PlayerEntity>();
        _logger.Log(
            $"[{player.Name}] x: {player.PlayerStateController.PositionWithDirection.x} y: {player.PlayerStateController.PositionWithDirection.y}");
        return player.PlayerStateController.PositionWithDirection;
    }

    public void DumpEntities()
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

            _logger.Log("Entity: " + we.name + " [-] " + we.ID + " [-] " + we.GetType());
        }

        _logger.Log("Inventory items");
        foreach (var inventoryItem in inventoryItems)
            _logger.Log(inventoryItem.ID + "  " + inventoryItem.Name + "  " + inventoryItem.Description);

        _logger.Log("All types");
        foreach (var type in types) _logger.Log(type);
    }
}