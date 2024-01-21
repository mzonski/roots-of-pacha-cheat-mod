using CheatMod.Core.Patches;
using HarmonyLib;

namespace CheatMod.Core.Cheats;

public class CheatModHarmony : Harmony
{
    public CheatModHarmony() : base("CheatModHarmony")
    {
    }

    public void ApplyPatches()
    {
        var processor = new PatchClassProcessor(this, typeof(CheatModPatches), true);
        processor.Patch();

        SkipDisclaimer.Patch(this);
    }
}