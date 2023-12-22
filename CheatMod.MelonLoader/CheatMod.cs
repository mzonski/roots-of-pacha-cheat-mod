using CheatMod.Core;
using MelonLoader;

namespace CheatMod.MelonLoader;

public class CheatMod : MelonMod
{
    private static readonly PachaManager PachaManager = new(new PachaItemDb(), new ModLogger());

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