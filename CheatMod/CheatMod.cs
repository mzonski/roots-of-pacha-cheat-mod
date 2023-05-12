using System;
using System.Collections.Generic;
using System.Reflection;
using CheatMod.Core;
using CheatMod.Core.Patches;
using CheatMod.Core.UI;
using MelonLoader;
using SodaDen.Pacha;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CheatMod.MelonLoader;

public class CheatMod : MelonMod
{
    private static PachaCheatUI _cheatUI;

    public override void OnInitializeMelon()
    {
        var pachaManager = new PachaManager(new PachaItemDb());
        _cheatUI = new PachaCheatUI(pachaManager);
        
        HarmonyInstance.PatchAll(typeof(CheatModPatches));
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F12)) PachaCheats.ForceRegenerateHittableResources();

        if (Input.GetKeyDown(KeyCode.F2)) CheatOptions.DrawUI = !CheatOptions.DrawUI;

        if (Input.GetKeyDown(KeyCode.F3)) PachaCheats.AddDayBuff(PlayerStatBuffType.Charisma, 5);
        
        if (Input.GetKeyDown(KeyCode.F4)) PachaUtils.GetPlayerCurrentCoords();
        
        if (Input.GetKeyDown(KeyCode.F5)) PachaCheats.GrowCrops();
    }

    public override void OnGUI()
    {
        _cheatUI.Draw();
    }
}