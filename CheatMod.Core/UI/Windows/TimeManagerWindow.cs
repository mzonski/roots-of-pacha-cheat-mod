using System;
using CheatMod.Core.Extensions;
using SodaDen.Pacha;
using UnityEngine;

namespace CheatMod.Core.UI.Windows;

public class TimeManagerWindow : PachaCheatWindow
{
    private int _timeOffset = 420;
    private Rect _timeManagerWindow = new(16, 550, 300, 90);
    private TimeSpan SelectedTime { get; set; } = TimeSpan.FromMinutes(420);
    
    private int TimeOffset
    {
        get => _timeOffset;
        set
        {
            if (value == _timeOffset) return;
            _timeOffset = value;
            // In Game the clock is starting from 6 o'clock
            SelectedTime = TimeSpan.FromMinutes(value + 6 * 60);
        }
    }
    
    private bool FreezeTimeEnabled
    {
        get => CheatOptions.Instance.IsFreezeTimeEnabled.Value;
        set
        {
            if (CheatOptions.Instance.IsFreezeTimeEnabled.Value && !value)
                Manager.PachaCheats.SetTime(TimeSpan.FromHours(6));

            if (value != CheatOptions.Instance.IsFreezeTimeEnabled.Value) CheatOptions.Instance.IsFreezeTimeEnabled.Value = value;
        }
    }
    
    public TimeManagerWindow(PachaManager manager) : base(manager)
    {
    }
    
    public override void Draw()
    {
        if (!CheatOptions.Instance.DrawTimeManagerWindow.Value) return;
        _timeManagerWindow = GUILayout.Window(CheatWindowType.TimeManager, _timeManagerWindow, DrawWindow, "Time Manager");
    }
    
    protected override void DrawWindow(int windowId)
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Pick time");
        TimeOffset =
            (int)GUI.HorizontalSlider(new Rect(126, 26, 180, 20), TimeOffset, 1, 1199);
        GUILayout.EndHorizontal();

        var timeString = $"{SelectedTime.Hours:00}:{SelectedTime.Minutes:00}";

        GUILayout.Label($"Selected time: {timeString}");

        if (FreezeTimeEnabled && GUILayout.Button("Set time")) SetInGameTime();

        FreezeTimeEnabled = GUILayout.Toggle(FreezeTimeEnabled, "Freeze time", CheatUIStyles.Toggle);

        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    private void SetInGameTime()
    {
        var nc = GameObject.FindObjectOfType<NotificationController>();
        nc.ShowNotification(new Notification
        {
            ID = Guid.NewGuid().ToBase64String(),
            Description = $"Time set to: {SelectedTime.Hours:00}:{SelectedTime.Minutes:00}"
        });

        Manager.PachaCheats.SetTime(SelectedTime);
    }
}