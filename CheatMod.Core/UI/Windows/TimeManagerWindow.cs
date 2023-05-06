using System;
using CheatMod.Core.Extensions;
using SodaDen.Pacha;
using UnityEngine;

namespace CheatMod.Core.UI.Windows;

public class TimeManagerWindow : PachaCheatWindow
{
    private Rect _window = new(16, 430, 300, 90);

    private int _timeOffset = 420;
    public TimeSpan SelectedTime { get; set; } = TimeSpan.FromMinutes(420);

    private void HandleSetTime()
    {
        var nc = GameObject.FindObjectOfType<NotificationController>();
        nc.ShowNotification(new Notification
        {
            ID = Guid.NewGuid().ToBase64String(),
            Description = $"Time set to: {SelectedTime.Hours:00}:{SelectedTime.Minutes:00}"
        });
        
        PachaCheats.SetTime(SelectedTime);
    }

    public int TimeOffset
    {
        get => _timeOffset;
        set
        {
            if (value != _timeOffset)
            {
                _timeOffset = value;
                // In Game the clock is starting from 6 o'clock
                SelectedTime = TimeSpan.FromMinutes(value + 6 * 60);
            }
        }
    }

    public TimeManagerWindow(PachaManager manager) : base(manager)
    {
    }

    public bool FreezeTimeEnabled
    {
        get => Manager.Config.IsFreezeTimeEnabled;
        set
        {
            if (Manager.Config.IsFreezeTimeEnabled && !value) // on disable
            {
                PachaCheats.SetTime(TimeSpan.FromHours(6));
            }

            if (value != Manager.Config.IsFreezeTimeEnabled)
            {
                Manager.Config.IsFreezeTimeEnabled = value;
            }
        }
    }

    protected override void DrawInternal(int windowId)
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Pick time");
        TimeOffset =
            (int)GUI.HorizontalSlider(new Rect(126, 26, 180, 20), TimeOffset, 1, 1199);
        GUILayout.EndHorizontal();

        var timeString = $"{SelectedTime.Hours:00}:{SelectedTime.Minutes:00}";

        GUILayout.Label($"Selected time: {timeString}");

        if (FreezeTimeEnabled && GUILayout.Button("Set time"))
        {
            HandleSetTime();
        }

        FreezeTimeEnabled = GUILayout.Toggle(FreezeTimeEnabled, "Freeze time", CheatUIStyles.Toggle);

        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    public override void Draw()
    {
        if (!Manager.Config.DrawTimeManagerWindow) return;
        _window = GUILayout.Window((int)CheatWindowType.TimeManager, _window, DrawInternal, "Pacha Time Manager");
    }
}