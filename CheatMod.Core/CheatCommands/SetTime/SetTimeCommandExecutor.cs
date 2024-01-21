using System;
using System.Reflection;
using CheatMod.Core.Managers;
using Photon.Pun;
using SodaDen.Pacha;
using UnityAtoms;
using UnityEngine;

namespace CheatMod.Core.CheatCommands.SetTime;

public class SetTimeCommandExecutor : CheatCommandExecutor<SetTimeCommand>
{
    public SetTimeCommandExecutor(PachaManager manager) : base(manager)
    {
    }

    private void SetTime(TimeSpan time)
    {
        try
        {
            Manager.Logger.Log($"Trying to set {time.Hours:00}:{time.Minutes:00}");
            var session = GameObject.FindObjectOfType<Session>();

            var dayTimeField =
                typeof(Session).GetField("DayTime", BindingFlags.NonPublic | BindingFlags.Instance);
            var offsetTimeField =
                typeof(Session).GetField("OffsetTime", BindingFlags.NonPublic | BindingFlags.Instance);
            var dayStartTimeField =
                typeof(Session).GetField("DayStartTime", BindingFlags.NonPublic | BindingFlags.Instance);

            var dayTime = (FloatVariable)dayTimeField!.GetValue(session);

            var serverTimestamp = PhotonNetwork.ServerTimestamp;

            dayTime.Value = time.Hours + time.Minutes / 60f;
            offsetTimeField!.SetValue(session, 0f);
            dayStartTimeField!.SetValue(session, serverTimestamp);

            Manager.Logger.Log($"Time set to: {time.Hours:00}:{time.Minutes:00}");
        }
        catch (Exception ex)
        {
            Manager.Logger.Log("Couldn't set time due to " + ex.Message);
        }
    }

    public override void Execute(SetTimeCommand command)
    {
        SetTime(command.Time);
    }
}