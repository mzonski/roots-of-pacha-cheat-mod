﻿using System;
using System.Linq;
using UnityEngine;

namespace RootsOfPachaCheatMod.UI.Windows;

public class ItemSpawnerWindow : PachaCheatWindow
{
    private Rect _itemSpawnerWindow = new(220, 16, 400, 520);

    private bool _isFirstRender;
    private GUIContent[] _currentListItems = Array.Empty<GUIContent>();
    private int _selectedItemId = -1;
    private int _itemQty = 1;
    private string _itemsFilterBy = string.Empty;
    private Vector2 _scrollPosition = Vector2.zero;

    private string ItemsFilterBy
    {
        get => _itemsFilterBy;
        set
        {
            if (string.IsNullOrEmpty(value) || _itemsFilterBy == value) return;
            _itemsFilterBy = value;
            SetSelectedListItems();
        }
    }

    public ItemSpawnerWindow(PachaManager manager) : base(manager)
    {
    }

    public override void DrawInternal(int windowId)
    {
        if (_isFirstRender == false)
        {
            ItemsFilterBy = "poop";
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
            if (GUILayout.Button("SPAWN"))
                PachaCheats.AddItemToInventory(short.Parse(_currentListItems[_selectedItemId].tooltip), _itemQty);


        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    public override void Draw()
    {
        if (Manager.Config.ItemSpawnerWindowOpen)
            _itemSpawnerWindow = GUILayout.Window((int)CheatWindowType.ItemSpawner, _itemSpawnerWindow, DrawInternal,
                "Pacha Item Spawner");
    }


    private void SetSelectedListItems()
    {
        var filteredList = !string.IsNullOrEmpty(ItemsFilterBy)
            ? Manager.ItemDb.InventoryItems.Where(ii =>
                ii.Name.ToLowerInvariant().Contains(ItemsFilterBy.ToLowerInvariant()))
            : Manager.ItemDb.InventoryItems.Where(ii =>
                ii.Name.ToLowerInvariant().Contains("poop"));

        _currentListItems = filteredList.Select(ii => new GUIContent(ii.Name, ii.ID.ToString())).ToArray();
    }
}