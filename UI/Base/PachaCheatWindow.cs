namespace RootsOfPachaCheatMod.UI.Windows;

public abstract class PachaCheatWindow
{
    protected readonly PachaManager Manager;

    public PachaCheatWindow(PachaManager manager)
    {
        Manager = manager;
    }
    public abstract void Draw(int windowId);
}