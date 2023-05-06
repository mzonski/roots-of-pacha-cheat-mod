using System.Reflection;
using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.BepInEx5;

public partial class CheatMod
{
    [HarmonyPatch(typeof(Session), "AdvanceTime")]
    [HarmonyPrefix]
    private static bool FreezeTimePatch()
    {
        return !PachaManager.Config.IsFreezeTimeEnabled;
    }
}