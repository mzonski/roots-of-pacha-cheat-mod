namespace CheatMod.Core.Utils;

public delegate void OnValueChangedEvent();

public sealed class SettingEntry<T>
{
    public event OnValueChangedEvent OnValueChange;
    private T _value;

    public SettingEntry(T defaultValue)
    {
        _value = defaultValue;
    }

    public void SetSilent(T value)
    {
        _value = value;
    }

    public T Value
    {
        get => _value;
        set
        {
            if (Equals(_value, value))
                return;

            _value = value;
            OnValueChange?.Invoke();
        }
    }
}