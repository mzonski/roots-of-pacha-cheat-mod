using CheatMod.Core;
using CheatMod.Core.Managers;
using MelonLoader;

namespace CheatMod.MelonLoader;

public class CheatMod : MelonMod
{
    private static readonly PachaManager PachaManager = new(new ModLogger());

    public override void OnInitializeMelon()
    {
    }

    public override void OnUpdate()
    {
        PachaManager.CatchKeyboardInput();
    }

    public override void OnGUI()
    {
        PachaManager.DrawGui();
    }
}