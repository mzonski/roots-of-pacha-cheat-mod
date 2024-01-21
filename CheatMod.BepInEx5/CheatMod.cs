using BepInEx;
using CheatMod.Core;
using CheatMod.Core.Managers;
using UnityEngine;

namespace CheatMod.BepInEx5;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, BuildInfo.Name, BuildInfo.Version)]
[BepInProcess("Roots of Pacha")]
public class CheatMod : BaseUnityPlugin
{
    private readonly PachaManager _pachaManager = new PachaManager(new ModLogger());

    private void Awake()
    {
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void Update()
    {
        _pachaManager.CatchKeyboardInput();
    }

    public void OnGUI()
    {
        _pachaManager.DrawGui();
    }
    
    private void OnDestroy()
    {
        Destroy(gameObject);
        Debug.Log("Mod has been unloaded!");
    }
}