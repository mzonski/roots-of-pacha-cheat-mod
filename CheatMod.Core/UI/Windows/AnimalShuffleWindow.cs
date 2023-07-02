using System;
using System.Collections.Generic;
using SodaDen.Pacha;
using UnityEngine;

namespace CheatMod.Core.UI.Windows;

public class AnimalShuffleWindow : PachaCheatWindow
{
    private Rect _shuffleAnimalsWindow = new(16, 660, 300, 90);
    private readonly GUIContent[] _rarityOptions;
    private readonly GUIContent[] _sexOptions;
    
    private int SelectedRarityIndex { get; set; }
    private int SelectedSexIndex { get; set; }
    private bool IsAdult { get; set; } = true;

    public AnimalShuffleWindow(PachaManager manager) : base(manager)
    {
        _rarityOptions = CreateRarityOptions();
        _sexOptions = CreateSexOptions();
    }

    private GUIContent[] CreateRarityOptions()
    {
        var values = Enum.GetValues(typeof(Rarity));
        var list = new List<GUIContent>();

        foreach (int valueObj in values)
        {
            var name = Enum.GetName(typeof(Rarity), valueObj);
            list.Add(new GUIContent(name, valueObj.ToString()));
        }

        return list.ToArray();
    }

    private GUIContent[] CreateSexOptions()
    {
        return new []
        {
            new GUIContent("Male", "1"),
            new GUIContent("Female", "2")
        };
    }

    protected override void DrawInternal(int windowId)
    {
        GUILayout.BeginVertical();

        SelectedRarityIndex = GUILayout.Toolbar(SelectedRarityIndex, _rarityOptions);
        SelectedSexIndex = GUILayout.Toolbar(SelectedSexIndex, _sexOptions);
        IsAdult = GUILayout.Toggle(IsAdult, "Adult", CheatUIStyles.Toggle);;
        
        GUILayout.Space(20);

        if (GUILayout.Button("Shuffle animals in range"))
        {
            var rarity = (Rarity)int.Parse(_rarityOptions[SelectedRarityIndex].tooltip);
            var sex = (Sex)byte.Parse(_sexOptions[SelectedSexIndex].tooltip);
            Manager.Logger.Log($"Trying to spawn {Enum.GetName(typeof(Sex), sex)} {Enum.GetName(typeof(Rarity), rarity)}");
            Manager.PachaCheats.ReplaceAnimalInHerdWithinRange(6f, rarity, sex, IsAdult);
        }

        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    public override void Draw()
    {
        if (!CheatOptions.DrawAnimalShuffleWindow) return;
        _shuffleAnimalsWindow = GUILayout.Window((int)CheatWindowType.ShuffleAnimals, _shuffleAnimalsWindow,
            DrawInternal, "Animal herd shuffler");
    }
}