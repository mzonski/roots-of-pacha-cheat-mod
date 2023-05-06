using CheatMod.Core;
using CheatMod.Core.UI;
using MelonLoader;
using SodaDen.Pacha;
using UnityEngine;

namespace CheatMod.MelonLoader;

public partial class CheatMod : MelonMod
{
    private static PachaManager _pachaManager;
    private static PachaCheatUI _cheatUI;

    public CheatMod()
    {
        _pachaManager = new PachaManager(new CheatConfiguration(), new PachaItemDb());
        _cheatUI = new PachaCheatUI(_pachaManager);
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F12)) PachaCheats.ForceRegenerateHittableResources();

        if (Input.GetKeyDown(KeyCode.F2)) _pachaManager.Config.DrawUI = !_pachaManager.Config.DrawUI;

        if (Input.GetKeyDown(KeyCode.F3)) PachaCheats.AddDayBuff(PlayerStatBuffType.Charisma, 5);
        
        if (Input.GetKeyDown(KeyCode.F4)) PachaUtils.GetPlayerCurrentCoords();
        
        if (Input.GetKeyDown(KeyCode.F5)) PachaCheats.GrowCrops();
    }

    public override void OnGUI()
    {
        _cheatUI.Draw();
    }
}