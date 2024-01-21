using CheatMod.Core.Managers;
using SodaDen.Pacha;
using UnityEngine;

namespace CheatMod.Core.CheatCommands.TeleportPlayer;

public class TeleportPlayerCommandExecutor : CheatCommandExecutor<TeleportPlayerCommand>
{
    public TeleportPlayerCommandExecutor(PachaManager manager) : base(manager)
    {
    }

    private void TeleportPlayer(float x, float y)
    {
        var playerManager = GameObject.FindObjectOfType<PlayerManager>();

        var playerStageCtl = playerManager.PlayerEntity.PlayerStateController;

        playerStageCtl.AboutToTeleport();
        playerStageCtl.GetComponent<BufferedInterpolationPhotonView>().TeleportTo(new Vector2(x, y));
    }

    public override void Execute(TeleportPlayerCommand command)
    {
        TeleportPlayer(command.X, command.Y);
    }
}