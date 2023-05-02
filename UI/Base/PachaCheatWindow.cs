namespace RootsOfPachaCheatMod.UI;

public abstract class PachaCheatWindow
{
    protected readonly PachaManager Manager;

    protected PachaCheatWindow(PachaManager manager)
    {
        Manager = manager;
    }
    public abstract void DrawInternal(int windowId);
    
    public abstract void Draw();
}