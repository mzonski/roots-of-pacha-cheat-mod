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
    
    public override void Draw()
    {
        if (!CheatOptions.DrawAnimalShuffleWindow) return;
        _shuffleAnimalsWindow = GUILayout.Window(CheatWindowType.ShuffleAnimals, _shuffleAnimalsWindow,
            DrawWindow, "Animal herd shuffler");
    }

    protected override void DrawWindow(int windowId)
    {
        GUILayout.BeginVertical();

        SelectedRarityIndex = GUILayout.Toolbar(SelectedRarityIndex, _rarityOptions);
        SelectedSexIndex = GUILayout.Toolbar(SelectedSexIndex, _sexOptions);
        IsAdult = GUILayout.Toggle(IsAdult, "Adult", CheatUIStyles.Toggle);;
        
        GUILayout.Space(20);

        if (GUILayout.Button("Shuffle animals in range"))
            ShuffleAnimals();

        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    private void ShuffleAnimals()
    {
        var rarity = (Rarity)int.Parse(_rarityOptions[SelectedRarityIndex].tooltip);
        var sex = (Sex)byte.Parse(_sexOptions[SelectedSexIndex].tooltip);
        Manager.Logger.Log($"Trying to spawn {Enum.GetName(typeof(Sex), sex)} {Enum.GetName(typeof(Rarity), rarity)}");
        Manager.PachaCheats.ReplaceAnimalInHerdWithinRange(6f, rarity, sex, IsAdult);
    }
    
    private static GUIContent[] CreateRarityOptions()
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

    private static GUIContent[] CreateSexOptions()
    {
        return new []
        {
            new GUIContent("Male", "1"),
            new GUIContent("Female", "2")
        };
    }
}