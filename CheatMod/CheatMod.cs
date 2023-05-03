using MelonLoader;
using RootsOfPachaCheatMod.UI;
using SodaDen.Pacha;
using UnityEngine;

namespace RootsOfPachaCheatMod;

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

        if (Input.GetKeyDown(KeyCode.F1)) _pachaManager.Config.DrawUI = !_pachaManager.Config.DrawUI;

        if (Input.GetKeyDown(KeyCode.F2)) PachaCheats.AddDayBuff(PlayerStatBuffType.Charisma, 5);
        
        if (Input.GetKeyDown(KeyCode.F3)) PachaUtils.GetPlayerCurrentCoords();
        
        if (Input.GetKeyDown(KeyCode.F4)) PachaCheats.GrowCrops();
    }

    public override void OnGUI()
    {
        _cheatUI.Draw();
    }
}