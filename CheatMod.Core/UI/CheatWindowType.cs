namespace CheatMod.Core.UI;

public struct CheatWindowType
{
    private readonly int _value;

    private CheatWindowType(int value)
    {
        _value = value;
    }

    public static readonly CheatWindowType Main = new(10000);
    public static readonly CheatWindowType ItemSpawner = Main + 1;
    public static readonly CheatWindowType PlayerStats = Main + 2;
    public static readonly CheatWindowType TimeManager = Main + 3;
    public static readonly CheatWindowType Teleports = Main + 4;
    public static readonly CheatWindowType ShuffleAnimals = Main + 5;

    public static CheatWindowType operator +(CheatWindowType type, int add)
    {
        return new CheatWindowType(type._value + add);
    }

    public static implicit operator int(CheatWindowType type)
    {
        return type._value;
    }
}