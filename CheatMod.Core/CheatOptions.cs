using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using CheatMod.Core.Utils;
using Newtonsoft.Json;

namespace CheatMod.Core;

public delegate void ConfigChangedEvent();

public class CheatOptions
{
    private static readonly string ConfigPath = Directory.GetCurrentDirectory() + "/cheatModConfig.json";
    public event ConfigChangedEvent ConfigChanged;

    private void OnConfigChanged()
    {
        ConfigChanged?.Invoke();
    }

    public readonly SettingEntry<bool> IsEasyFishingEnabled = new(true);
    public readonly SettingEntry<bool> IsInfiniteFluteEnabled = new(true);
    public readonly SettingEntry<bool> IsEasyAnimalsEnabled = new(true);
    public readonly SettingEntry<bool> IsInfiniteSeedsEnabled = new(true);
    public readonly SettingEntry<bool> IsInfiniteStaminaEnabled = new(true);
    public readonly SettingEntry<bool> IsInfiniteWaterToolEnabled = new(true);
    public readonly SettingEntry<bool> IsFreezeTimeEnabled = new(false);
    public readonly SettingEntry<bool> IsMovementSpeedEnabled = new(true);
    public readonly SettingEntry<float> PlayerMovementSpeed = new(4f);
    public readonly SettingEntry<bool> IsInfiniteHarvestEnabled = new(false);
    public readonly SettingEntry<bool> IsFastProductionEnabled = new(true);
    public readonly SettingEntry<bool> DrawUI = new(true);
    public readonly SettingEntry<bool> DrawItemSpawnerWindow = new(false);
    public readonly SettingEntry<bool> DrawTimeManagerWindow = new(false);
    public readonly SettingEntry<bool> DrawTeleportWindow = new(false);
    public readonly SettingEntry<bool> DrawAnimalShuffleWindow = new(false);

    private CheatOptions()
    {
        foreach (var field in typeof(CheatOptions).GetFields())
        {
            if (!field.FieldType.IsGenericType || field.FieldType.GetGenericTypeDefinition() != typeof(SettingEntry<>))
                continue;

            var settingEntry = field.GetValue(this);
            var eventInfo = field.FieldType.GetEvent("OnValueChange");

            if (eventInfo is null || settingEntry is null) continue;

            var delegateType = eventInfo.EventHandlerType;
            var handlerMethod = GetType().GetMethod(nameof(OnConfigChanged)/*"OnConfigChanged"*/, BindingFlags.NonPublic | BindingFlags.Instance);
            var delegateInstance = Delegate.CreateDelegate(delegateType, this, handlerMethod!);
            eventInfo.AddEventHandler(settingEntry, delegateInstance);
        }
    }

    public static void Load()
    {
        if (File.Exists(ConfigPath))
        {
            var contents = File.ReadAllText(ConfigPath);
            var newCheatOptions = JsonConvert.DeserializeObject<CheatOptions>(contents);

            _instance = newCheatOptions;
        }

        _instance.ConfigChanged += () =>
        {
            new Task(() =>
            {
                File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Instance, Formatting.Indented));
            }).Start();
        };
    }

    private static CheatOptions _instance;
    public static CheatOptions Instance => _instance ??= new CheatOptions();
}