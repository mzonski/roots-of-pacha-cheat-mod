namespace CheatMod.Core.UI;

public abstract class PachaCheatWindow
{
    protected readonly PachaManager Manager;

    protected PachaCheatWindow(PachaManager manager)
    {
        Manager = manager;
    }

    protected abstract void DrawInternal(int windowId);

    public abstract void Draw();
}