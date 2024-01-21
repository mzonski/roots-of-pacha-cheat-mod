using System;
using System.Collections.Generic;
using System.Linq;
using SodaDen.Pacha;
using UnityEngine;

namespace CheatMod.Core.CheatCommands;

public static class CommandHelper
{
    // Get current standing tile = TilesManager.Instance.TileAt(GetPlayerCurrentCoords())
    public static Vector2 GetPlayerCurrentCoords()
    {
        var player = GameObject.FindObjectOfType<PlayerEntity>();
        return player.PlayerStateController.PositionWithDirection;
    }
    
    public static IEnumerable<IEntityData> GetEntityDataInRange(float range)
    {
        var playerCoords = GetPlayerCurrentCoords();
        var entityList = new List<IEntityData>();

        foreach (var entityData in Game.Current.Entities)
        {
            if (entityData is not IPositionableEntityData posEntityData) continue;
            if (!posEntityData.Position.HasValue) continue;
            var distanceFromPlayer = Vector2.Distance(playerCoords, posEntityData.Position.Value);
            if (distanceFromPlayer > range) continue;

            entityList.Add(entityData);
        }

        return entityList.Where(entity => entity is not null);
    }
    public static IEnumerable<T> GetEntitiesInRange<T>(float range) where T : MonoBehaviour
    {
        return GetEntityDataInRange(range)
            .Select(data => GuidManager.ResolveGuid(data.ID).GetComponent<T>())
            .Where(component => component is not null);
    }
    
    /*
    public void DumpEntitiesInRange(float range = 3f)
    {
        var entities = GetEntitiesInRange(range)
            .Where(entity => entity is not null);
    
        foreach (var entityData in entities)
        {
            _logger.Log($@"[{entityData.Type}]: {entityData.ID}");
        }
    }
    
    public void DumpInventoryItems()
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
    */
}