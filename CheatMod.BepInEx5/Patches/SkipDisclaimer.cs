using CheatMod.Core;
using UnityEngine.SceneManagement;

namespace CheatMod.BepInEx5;

public partial class CheatMod
{
    // Sometimes work, sometimes don't. Don't have time to solve it now
    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     if (scene.name == "Scn_Disclaimer")
    //     {
    //         PachaUtils.SkipDisclaimerIntros();
    //     }
    // }
    //
    // private void OnEnable()
    // {
    //     SceneManager.sceneLoaded += OnSceneLoaded;
    // }
    //
    // void OnDisable()
    // {
    //     SceneManager.sceneLoaded -= OnSceneLoaded;
    // }
}