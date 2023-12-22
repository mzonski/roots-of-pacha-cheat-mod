using BepInEx;
using CheatMod.Core;
using UnityEngine;

namespace CheatMod.BepInEx5;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, BuildInfo.Name, BuildInfo.Version)]
[BepInProcess("Roots of Pacha")]
public class CheatMod : BaseUnityPlugin
{
    private static readonly PachaManager PachaManager = new(new PachaItemDb(), new ModLogger());

    private void Awake()
    {
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void Update()
    {
        PachaManager.CatchKeyboardInput();
    }

    public void OnGUI()
    {
        PachaManager.DrawGui();
    }
    
    private void OnDestroy()
    {
        Destroy(gameObject);
        Debug.Log("Mod has been unloaded!");
    }
}