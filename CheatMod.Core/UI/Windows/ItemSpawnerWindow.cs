using System;
using System.Collections.Generic;
using System.Linq;
using CheatMod.Core.CheatCommands.AddItemToInventory;
using CheatMod.Core.Managers;
using SodaDen.Pacha;
using UnityEngine;

namespace CheatMod.Core.UI.Windows;

public class ItemSpawnerWindow : PachaCheatWindow
{
    private Rect _itemSpawnerWindow = new(440, 16, 400, 520);

    private GUIContent[] _currentListItems = Array.Empty<GUIContent>();
    private readonly GUIContent[] _itemQualityOptions;

    private bool _isFirstRender;
    private int _itemQty = 1;
    private Vector2 _scrollPosition = Vector2.zero;
    private int _selectedItemId = -1;
    private string _itemsFilterBy = string.Empty;

    private string ItemsFilterBy
    {
        get => _itemsFilterBy;
        set
        {
            if (value is null || _itemsFilterBy == value) return;
            _itemsFilterBy = value;
            SetSelectedListItems();
        }
    }

    private int SelectedQualityIndex { get; set; }

    public ItemSpawnerWindow(PachaManager manager) : base(manager)
    {
        _itemQualityOptions = CreateItemQualityOptions();
    }

    public override void Draw()
    {
        if (CheatOptions.Instance.DrawItemSpawnerWindow.Value)
            _itemSpawnerWindow = GUILayout.Window(CheatWindowType.ItemSpawner, _itemSpawnerWindow, DrawWindow,
                "Pacha Item Spawner");
    }

    protected override void DrawWindow(int windowId)
    {
        if (_isFirstRender == false)
        {
            ItemsFilterBy = "";
            _isFirstRender = true;
        }

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Filter");
        ItemsFilterBy = GUILayout.TextField(ItemsFilterBy, GUILayout.Width(350));
        GUILayout.EndHorizontal();

        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(400), GUILayout.Height(400));

        _selectedItemId = GUILayout.SelectionGrid(
            _selectedItemId,
            _currentListItems,
            1,
            CheatUIStyles.ListItemButton,
            GUILayout.ExpandWidth(true));

        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Quantity");
        _itemQty = (int)GUI.HorizontalSlider(new Rect(80, 455, 200, 20), _itemQty, 0, 255);
        _itemQty = PachaUtils.NormalizeQty(
            int.Parse(GUILayout.TextField(_itemQty.ToString(), GUILayout.Width(100))));
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        if (_selectedItemId > -1)
        {
            SelectedQualityIndex = GUILayout.Toolbar(SelectedQualityIndex, _itemQualityOptions);
            var quality = (ItemQuality)byte.Parse(_itemQualityOptions[SelectedQualityIndex].tooltip);

            if (GUILayout.Button("Add to inventory"))
                Manager.Mediator.Execute(new AddItemToInventoryCommand
                {
                    ItemId = short.Parse(_currentListItems[_selectedItemId].tooltip), Qty = _itemQty, Quality = quality
                });
        }


        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    private void SetSelectedListItems()
    {
        var filteredList = ItemsFilterBy is not null
            ? Manager.ItemDatabase.InventoryItems.Where(ii =>
                ii.Name.ToLowerInvariant().Contains(ItemsFilterBy.ToLowerInvariant()))
            : Array.Empty<InventoryItem>();

        _currentListItems = filteredList.Select(ii => new GUIContent(ii.Name, ii.ID.ToString())).ToArray();
    }

    private static GUIContent[] CreateItemQualityOptions()
    {
        var values = Enum.GetValues(typeof(ItemQuality));
        var list = new List<GUIContent>();

        foreach (byte valueObj in values)
        {
            var name = Enum.GetName(typeof(ItemQuality), valueObj);
            list.Add(new GUIContent(name, valueObj.ToString()));
        }

        return list.ToArray();
    }
}