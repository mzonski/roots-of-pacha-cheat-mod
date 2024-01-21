using System.Linq;
using CheatMod.Core.Managers;
using SodaDen.Pacha;

namespace CheatMod.Core.CheatCommands.WaterAllTiles;

public class WaterAllTilesCommandExecutor : CheatCommandExecutor<WaterAllTilesCommand>
{
    public WaterAllTilesCommandExecutor(PachaManager manager) : base(manager)
    {
    }

    private void WaterAllTilledTiles()
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

        Manager.Logger.Log($"Watered {wateredAmount} tiles");
    }

    public override void Execute(WaterAllTilesCommand command)
    {
        WaterAllTilledTiles();
    }
}