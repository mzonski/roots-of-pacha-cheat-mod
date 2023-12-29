using CheatMod.Core.Services;
using CheatMod.Core.UI;
using UnityEngine;

namespace CheatMod.Core;

public class PachaManager
{
    private readonly PachaCheatUI _cheatUI;
    public readonly PachaItemDb ItemDb;
    public readonly PachaCheats PachaCheats;
    public readonly IModLogger Logger;

    public PachaManager(PachaItemDb itemDb, IModLogger logger)
    {
        Logger = logger;
        ItemDb = itemDb;

        PachaCheats = new PachaCheats(logger);
        _cheatUI = new PachaCheatUI(this);

        CheatOptions.Load();

        var harmony = new CheatModHarmony();
        harmony.ApplyPatches();
    }

    public void CatchKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.F2)) CheatOptions.Instance.DrawUI.Value = !CheatOptions.Instance.DrawUI.Value;
        
        if (Input.GetKeyDown(KeyCode.F5)) PachaCheats.GrowCrops();
        
        if (Input.GetKeyDown(KeyCode.F6)) PachaCheats.GrowTrees();
        
        if (Input.GetKeyDown(KeyCode.F8)) PachaCheats.DestroyHittableResources();
        
        if (Input.GetKeyDown(KeyCode.F9)) PachaCheats.GetPlayerCurrentCoords();
        
        if (Input.GetKeyDown(KeyCode.F10)) PachaCheats.DumpEntitiesInRange();
        
        if (Input.GetKeyDown(KeyCode.F11)) PachaCheats.ReplaceAnimalInHerdWithinRange();
    }

    public void DrawGui()
    {
        _cheatUI.Draw();
    }
}