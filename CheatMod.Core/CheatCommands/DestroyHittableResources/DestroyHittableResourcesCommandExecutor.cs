using System;
using System.Linq;
using System.Reflection;
using CheatMod.Core.CheatCommands.GrowTrees;
using CheatMod.Core.Managers;
using JetBrains.Annotations;
using SodaDen.Pacha;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CheatMod.Core.CheatCommands.DestroyHittableResources;

public class DestroyHittableResourcesCommandExecutor : CheatCommandExecutor<DestroyHittableResourcesCommand>
{
    public DestroyHittableResourcesCommandExecutor(PachaManager manager) : base(manager)
    {
    }

    private void DestroyHittableResourcesInCaves(float range, PlayerStageController psc)
    {
        var cavesManager = GameObject.FindObjectOfType<CavesManager>();

        foreach (var caveController in cavesManager.Caves)
        {
            foreach (var caveRoom in caveController.Rooms)
            {
                if (!caveRoom.Stage.GetComponent<Collider2D>().OverlapPoint(psc.transform.position))
                    continue;

                ProcessHittableEntitiesInCaveRoom(range, psc, caveRoom);
                ProcessCaveOresShims(range, psc, caveRoom);
            }
        }
    }


    private void ProcessHittableEntitiesInCaveRoom(float range, PlayerStageController psc, CaveRoomEntity caveRoom)
    {
        foreach (var hittableEntity in caveRoom.CurrentHittables.Where(ch => ch != null))
        {
            if (Vector2.Distance(hittableEntity.transform.position, psc.transform.position) >= range)
                continue;

            DestroyHittableResourceEntity(hittableEntity);
            caveRoom.CurrentHittables.Remove(hittableEntity);
        }
    }

    private void ProcessCaveOresShims(float range, PlayerStageController psc, CaveRoomEntity caveRoom)
    {
        foreach (var shim in caveRoom.CurrentCaveOresShims.Where(sh => sh != null))
        {
            if (Vector2.Distance(shim.transform.position, psc.transform.position) >= range || shim.CurrentHealth <= 0f)
                continue;

            var healthComponent = GetHealthComponentFromShim(shim);
            if (healthComponent != null && healthComponent.CurrentHealth > 0f)
            {
                healthComponent.DecreaseHealth(healthComponent.CurrentHealth);
            }

            if (Application.isPlaying)
                caveRoom.Pool.Release(shim);
            else
                Object.DestroyImmediate(shim.gameObject);

            caveRoom.CurrentCaveOresShims.Remove(shim);
        }
    }

    private HealthComponent GetHealthComponentFromShim(CaveOreShim shim)
    {
        var caveOreShinType = shim.GetType().GetProperty("Health", BindingFlags.NonPublic | BindingFlags.Instance);
        return caveOreShinType != null ? (HealthComponent)caveOreShinType.GetValue(shim) : null;
    }


    
    private static void DestroyHittableResourceEntity([CanBeNull] HittableResourceEntity hittableEntity)
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

    private void DestroyHittableResourcesOnLand(float range)
    {
        var hittableEntities = CommandHelper.GetEntityDataInRange(range)
            .Where(e => e.Type == EntityType.Resource)
            .Select(e => GuidManager.ResolveGuid(e.ID)?.GetComponent<HittableResourceEntity>())
            .Where(entity => entity is not null);

        foreach (var hittableEntity in hittableEntities)
        {
            DestroyHittableResourceEntity(hittableEntity);
        }
    }

    public override void Execute(DestroyHittableResourcesCommand command)
    {
        Manager.Logger.Log("Destroy hittable resources");

        var playerManager = GameObject.FindObjectOfType<PlayerManager>();
        var psc = playerManager.PlayerEntity.PlayerStageController;

        switch (psc.Stage.Region)
        {
            case Region.Caves:
                DestroyHittableResourcesInCaves(command.Range, psc);
                break;
            case Region.TheLand:
                DestroyHittableResourcesOnLand(command.Range);
                break;
        }

        throw new NotImplementedException();
    }
}