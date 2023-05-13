using System.Reflection;
using HarmonyLib;
using SodaDen.Pacha.UI;
using UnityEngine;

namespace CheatMod.Core.Patches;

public partial class CheatModPatches
{
    [HarmonyPatch(typeof(Disclaimer), "Start")]
    [HarmonyPostfix]
    private static void SkipDisclaimerPatch()
    {
        SkipDisclaimerIntros();
    }

    private static void SkipDisclaimerIntros()
    {
        var dsc = GameObject.FindObjectOfType<Disclaimer>();

        if (dsc == null) return;

        dsc.OnAllLogos();
        dsc.OnSplashEnded();

        var currentIndexProperty =
            typeof(Disclaimer).GetProperty("CurrentIndex", BindingFlags.NonPublic | BindingFlags.Instance);
        currentIndexProperty?.SetValue(dsc, 0);
    }
}