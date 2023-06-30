using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CheatMod.Core.Services;
using JetBrains.Annotations;
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

    public void AddItemToInventory(short itemId, int qty, ItemQuality quality = ItemQuality.Normal)
    {
        var it = (InventoryItem)Database.Instance[itemId];
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
            if (tile is TillableTile { Stage: TillableStage.Tilled } tilledTile)
            {
                tilledTile.SetStage(TillableStage.TilledWet, null, false, null,
                    Network.PlayerList.First(x => x.IsMasterClient), true);
                wateredAmount++;
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

        if (entityManager is null) _logger.Log("Entity manager is null");

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
            throw new ArgumentNullException(nameof(PlantEntity),
                "Plant entity data fields are initialized incorrectly");

        levelProperty.SetValue(plantEntity, 2);
        plantEntity.Withered = false;
        if (CheatOptions.IsInfiniteHarvestEnabled)
            lastHarvestedProperty.SetValue(plantEntity,
                null);

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

            foreach (var plantEntity in plantsInRange) PatchPlantEntity(plantEntity);
        }
        catch (Exception ex)
        {
            _logger.Log("[GrowCrops] Failed: " + ex.Message);
            _logger.Log(ex.StackTrace);
        }
    }

    public void GrowTrees(float range = 3f)
    {
        try
        {
            _logger.Log("Grow trees");

            var treesInRange = new List<TreeEntity>();
            var playerCoords = GetPlayerCurrentCoords();
            foreach (var entityData in Game.Current.Entities)
            {
                if (entityData.Type != EntityType.Tree) continue;

                var treeData = (TreeData)entityData;

                if (!treeData.Position.HasValue) continue;

                var distanceFromPlayer = Vector2.Distance(playerCoords, treeData.Position.Value);

                if (!(distanceFromPlayer < range)) continue;

                var treeEntity = GuidManager.ResolveGuid(entityData.ID).GetComponent<TreeEntity>();
                treesInRange.Add(treeEntity);
            }

            _logger.Log($"Found {treesInRange.Count} trees in range");

            foreach (var treeEntity in treesInRange)
            {
                var ageField =
                    typeof(TreeEntity).GetField("Age", BindingFlags.NonPublic | BindingFlags.Instance);
                var healthField =
                    typeof(TreeEntity).GetField("Health", BindingFlags.NonPublic | BindingFlags.Instance);
                var treeRendererProperty =
                    typeof(TreeEntity).GetProperty("TreeRenderer", BindingFlags.NonPublic | BindingFlags.Instance);
                var currentDayProperty =
                    typeof(TreeEntity).GetProperty("CurrentDay", BindingFlags.NonPublic | BindingFlags.Instance);

                if (ageField == null || healthField == null || treeRendererProperty == null ||
                    currentDayProperty == null)
                {
                    throw new ArgumentNullException(nameof(treeEntity),
                        "Tree entity data fields are initialized incorrectly");
                }

                var renderer = (TreeRenderer)treeRendererProperty.GetValue(treeEntity);
                var healthComponent = (HealthComponent)healthField.GetValue(treeEntity);
                var currentDay = (int)currentDayProperty.GetValue(treeEntity);

                healthComponent.IncreaseHealth(treeEntity.Tree.StumpAtHealth + 1);
                ageField.SetValue(treeEntity, (short)treeEntity.Tree.MatureAge);
                _logger.Log("Current age: " + ageField.GetValue(treeEntity));

                var currentSeason = new YearSeasonDay(currentDay).Season;
                renderer.UpdateFromData(treeEntity.Tree, treeEntity.Tree.StageIndexAt(treeEntity.Tree.MatureAge),
                    currentSeason, CheatOptions.IsInfiniteHarvestEnabled ? treeEntity.Tree.DaysInFlower + 1 : 0, false,
                    true);

                treeEntity.StartFlowerIn = 0;
                treeEntity.LastFloweredIn = null;
            }
        }
        catch (Exception ex)
        {
            _logger.Log("[GrowTrees] Failed: " + ex.Message);
            _logger.Log(ex.StackTrace);
        }
    }

    private IEnumerable<IEntityData> GetEntityDataInRange(float range)
    {
        var playerCoords = GetPlayerCurrentCoords();
        var entityList = new List<IEntityData>();

        foreach (var entityData in Game.Current.Entities)
        {
            //_logger.Log($"Entity: {entityData.Type.ToString()} ID: {entityData.ID}");
            if (entityData is not IPositionableEntityData posEntityData) continue;
            if (!posEntityData.Position.HasValue) continue;
            var distanceFromPlayer = Vector2.Distance(playerCoords, posEntityData.Position.Value);
            if (distanceFromPlayer > range) continue;

            entityList.Add(entityData);
        }

        return entityList;
    }

    private static void DestroyHittableResource([CanBeNull] HittableResourceEntity hittableEntity)
    {
        if (hittableEntity is null) return;
        var healthPropertyInfo = hittableEntity.GetType()
            .GetProperty("Health", BindingFlags.NonPublic | BindingFlags.Instance);
        if (healthPropertyInfo == null) return;
        var healthComponent = (HealthComponent)healthPropertyInfo.GetValue(hittableEntity);
        if (healthComponent.CurrentHealth > 0f)
        {
            healthComponent.DecreaseHealth(healthComponent.CurrentHealth);
        }
    }

    public void DestroyHittableResources(float range = 3f)
    {
        _logger.Log("Destroy hittable resources");

        var cavesManager = GameObject.FindObjectOfType<CavesManager>();
        var playerManager = GameObject.FindObjectOfType<PlayerManager>();

        var psc = playerManager.PlayerEntity.PlayerStageController;

        switch (psc.Stage.Region)
        {
            case Region.Caves:
            {
                foreach (var caveController in cavesManager.Caves)
                {
                    foreach (var caveRoom in caveController.Rooms)
                    {
                        if (!caveRoom.Stage.GetComponent<Collider2D>().OverlapPoint(psc.transform.position)) continue;
                        
                        foreach (var hittableEntity in caveRoom.CurrentHittables.ToList())
                        {
                            if (Vector2.Distance(hittableEntity.transform.position, psc.transform.position) < range)
                            {
                                DestroyHittableResource(hittableEntity);
                                caveRoom.CurrentHittables.Remove(hittableEntity);
                            }
                        }

                        foreach (var shim in caveRoom.CurrentCaveOresShams.ToList())
                        {
                            if (Vector2.Distance(shim.transform.position, psc.transform.position) < range && 
                                shim.Health.CurrentHealth > 0f)
                            {
                                shim.Health.DecreaseHealth(shim.Health.CurrentHealth);
                                
                                if (Application.isPlaying)
                                    caveRoom.Pool.Release(shim);
                                else
                                    UnityEngine.Object.DestroyImmediate(shim.gameObject);
                                
                                caveRoom.CurrentCaveOresShams.Remove(shim);
                            }
                        }
                    }
                }

                break;
            }
            case Region.TheLand:
            {
                var hittableEntities = GetEntityDataInRange(range)
                    .Where(e => e.Type == EntityType.Resource)
                    .Select(e => GuidManager.ResolveGuid(e.ID)?.GetComponent<HittableResourceEntity>())
                    .Where(entity => entity is not null);

                foreach (var hittableEntity in hittableEntities)
                {
                    DestroyHittableResource(hittableEntity);
                }

                break;
            }
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