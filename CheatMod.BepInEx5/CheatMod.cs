using BepInEx;
using CheatMod.Core;
using CheatMod.Core.UI;
using HarmonyLib;
using SodaDen.Pacha;
using UnityEngine;

namespace CheatMod.BepInEx5;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, CheatModBuildInfo.Name, CheatModBuildInfo.Version)]
[BepInProcess("Roots of Pacha")]
public partial class CheatMod : BaseUnityPlugin
{
    private static readonly PachaManager PachaManager = new(new CheatConfiguration(), new PachaItemDb());
    private static readonly PachaCheatUI CheatUI = new(PachaManager);

    private void Awake()
    {
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Harmony.CreateAndPatchAll(typeof(CheatMod));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12)) PachaCheats.ForceRegenerateHittableResources();

        if (Input.GetKeyDown(KeyCode.F2)) PachaManager.Config.DrawUI = !PachaManager.Config.DrawUI;

        if (Input.GetKeyDown(KeyCode.F3)) PachaCheats.AddDayBuff(PlayerStatBuffType.Charisma, 5);

        if (Input.GetKeyDown(KeyCode.F4)) PachaUtils.GetPlayerCurrentCoords();

        if (Input.GetKeyDown(KeyCode.F5)) PachaCheats.GrowCrops();
    }

    public void OnGUI()
    {
        CheatUI.Draw();
    }
}