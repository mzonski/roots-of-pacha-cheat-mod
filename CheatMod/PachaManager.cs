namespace RootsOfPachaCheatMod;

public class PachaManager
{
    public readonly CheatConfiguration Config;
    public readonly PachaItemDb ItemDb;

    public PachaManager(CheatConfiguration config, PachaItemDb itemDb)
    {
        Config = config;
        ItemDb = itemDb;
    }
}