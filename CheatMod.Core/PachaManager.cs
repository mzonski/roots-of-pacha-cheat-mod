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
    private readonly CheatModHarmony Harmony;

    public PachaManager(PachaItemDb itemDb, IModLogger logger, CheatModHarmony harmony)
    {
        Logger = logger;
        ItemDb = itemDb;
        Harmony = harmony;
        
        PachaCheats = new PachaCheats(logger);
        _cheatUI = new PachaCheatUI(this);
    }

    public void CatchKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.F2)) CheatOptions.DrawUI = !CheatOptions.DrawUI;
        
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