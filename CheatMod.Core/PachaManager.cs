using CheatMod.Core.Services;
using CheatMod.Core.UI;
using SodaDen.Pacha;
using UnityEngine;

namespace CheatMod.Core;

public class PachaManager
{
    private readonly PachaCheatUI _cheatUI;
    public readonly PachaItemDb ItemDb;
    public readonly PachaCheats PachaCheats;

    public PachaManager(PachaItemDb itemDb, IModLogger logger)
    {
        ItemDb = itemDb;
        PachaCheats = new PachaCheats(logger);
        _cheatUI = new PachaCheatUI(this);
    }

    public void CatchKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.F12)) PachaCheats.ForceRegenerateHittableResources();

        if (Input.GetKeyDown(KeyCode.F2)) CheatOptions.DrawUI = !CheatOptions.DrawUI;

        if (Input.GetKeyDown(KeyCode.F3)) PachaCheats.AddDayBuff(PlayerStatBuffType.Charisma, 5);

        if (Input.GetKeyDown(KeyCode.F4)) PachaCheats.GetPlayerCurrentCoords();

        if (Input.GetKeyDown(KeyCode.F5)) PachaCheats.GrowCrops();
    }

    public void DrawGui()
    {
        _cheatUI.Draw();
    }
}