using BepInEx;
using CheatMod.Core;
using CheatMod.Core.Patches;
using HarmonyLib;

namespace CheatMod.BepInEx5;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, BuildInfo.Name, BuildInfo.Version)]
[BepInProcess("Roots of Pacha")]
public class CheatMod : BaseUnityPlugin
{
    private static readonly PachaManager PachaManager = new(new PachaItemDb(), new ModLogger());

    private void Awake()
    {
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Harmony.CreateAndPatchAll(typeof(CheatModPatches));
    }

    private void Update()
    {
        PachaManager.CatchKeyboardInput();
    }

    public void OnGUI()
    {
        PachaManager.DrawGui();
    }
}