using System.Reflection;
using SodaDen.Pacha.UI;
using UnityEngine;

namespace RootsOfPachaCheatMod;

public partial class CheatMod
{
    private static void SkipDisclaimerIntros()
    {
        var dsc = GameObject.FindObjectOfType<Disclaimer>();

        dsc.OnAllLogos();
        dsc.OnSplashEnded();

        var currentIndexProperty = typeof(Disclaimer).GetProperty("CurrentIndex", BindingFlags.NonPublic | BindingFlags.Instance);
        currentIndexProperty?.SetValue(dsc, 0);
    }

    
    public override void OnSceneWasInitialized(int buildindex, string sceneName)
    {
        if (sceneName == "Scn_Disclaimer")
        {
            SkipDisclaimerIntros();
        }
    }
}