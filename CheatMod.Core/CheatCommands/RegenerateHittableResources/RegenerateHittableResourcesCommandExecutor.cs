using System.Reflection;
using CheatMod.Core.Managers;
using SodaDen.Pacha;
using UnityEngine;

namespace CheatMod.Core.CheatCommands.RegenerateHittableResources;

public class RegenerateHittableResourcesCommandExecutor : CheatCommandExecutor<RegenerateHittableResourcesCommand>
{
    public RegenerateHittableResourcesCommandExecutor(PachaManager manager) : base(manager)
    {
    }

    private void ForceRegenerateHittableResources()
    {
        var player = GameObject.FindObjectOfType<PlayerEntity>();

        var method = typeof(TilesManager).GetMethod("RegenerateHittableResources",
            BindingFlags.NonPublic | BindingFlags.Instance);

        method?.Invoke(TilesManager.Instance, new object[] { player.CurrentDay, true });
    }

    public override void Execute(RegenerateHittableResourcesCommand command)
    {
        ForceRegenerateHittableResources();
    }
}