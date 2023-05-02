using SodaDen.Pacha.UI;
using UnityEngine;

namespace RootsOfPachaCheatMod;

public partial class CheatMod
{
    public override void OnSceneWasInitialized(int buildindex, string sceneName)
    {
        if (sceneName == "Scn_Disclaimer")
        {
            var dsc = GameObject.FindObjectOfType<Disclaimer>();

            dsc.OnAllLogos();
            dsc.OnSplashEnded();
        }
    }
}