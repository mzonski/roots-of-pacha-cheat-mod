using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using CheatMod.Core.Extensions;
using CheatMod.Core.Utils;
using HarmonyLib;
using SodaDen.Pacha;
using SodaDen.Pacha.Audio;
using SodaDen.Pacha.UI;
using UnityEngine;

namespace CheatMod.Core.Patches;

public static class SkipDisclaimer
{
    public static void Patch(Harmony harmony)
    {
        var start = typeof(Disclaimer).GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance);
        var startPostfix =
            typeof(SkipDisclaimer).GetMethod(nameof(StartPostfix), BindingFlags.Static | BindingFlags.NonPublic);
        var startTranspiler = typeof(SkipDisclaimer).GetMethod(nameof(StartMethodTranspiler),
            BindingFlags.Static | BindingFlags.NonPublic);
        harmony.PatchAsyncMethod(start, transpiler: startTranspiler, postfix: startPostfix);

        var sceneTransition =
            typeof(Disclaimer).GetMethod("TransitionToNextScene", BindingFlags.NonPublic | BindingFlags.Instance);
        var sceneTransitionTranspiler =
            typeof(SkipDisclaimer).GetMethod(nameof(TransitionToNextSceneMethodTranspiler),
                BindingFlags.Static | BindingFlags.NonPublic);
        harmony.PatchAsyncMethod(sceneTransition, transpiler: sceneTransitionTranspiler);

        var onAllLogos = typeof(Disclaimer).GetMethod("OnAllLogos", BindingFlags.Public | BindingFlags.Instance);
        var onAllLogosTranspiler = typeof(SkipDisclaimer).GetMethod(nameof(OnAllLogosTranspiler),
            BindingFlags.Static | BindingFlags.NonPublic);
        harmony.PatchAsyncMethod(onAllLogos, transpiler: onAllLogosTranspiler);

        var awake = typeof(Disclaimer).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
        var awakePostfix =
            typeof(SkipDisclaimer).GetMethod(nameof(AwakePostfix), BindingFlags.Static | BindingFlags.NonPublic);
        var awakeTranspiler = typeof(SkipDisclaimer).GetMethod(nameof(AwakeMethodTranspiler),
            BindingFlags.Static | BindingFlags.NonPublic);
        harmony.PatchMethod(awake, transpiler: awakeTranspiler, postfix: awakePostfix);
    }

    private static void AwakePostfix()
    {
        var dsc = GameObject.FindObjectOfType<Disclaimer>();

        dsc.OnAllLogos();
        dsc.OnSplashEnded();
    }

    private static void StartPostfix()
    {
        MasterSceneLoader.LoadAddressableAsync("Assets/Game/Scenes/Scn_MainMenu.unity");
    }

    private static IEnumerable<CodeInstruction> StartMethodTranspiler(IEnumerable<CodeInstruction> codeInstructions)
    {
        var instructionBuilder = new SequenceSkipBuilder<CodeInstruction>(codeInstructions);

        var initializeStaticMethod = typeof(AudioManager)
            .GetMethod("InitializeStatic", BindingFlags.Static | BindingFlags.Public);
        var startBoostLoadingMethod = typeof(GameOptimization)
            .GetMethod("StartBoostLoading", BindingFlags.Static | BindingFlags.Public);

        instructionBuilder.SkipNextEdges(
            start => start.opcode == OpCodes.Call && start.operand as MethodInfo == initializeStaticMethod,
            end => end.opcode == OpCodes.Call && end.operand as MethodInfo == startBoostLoadingMethod,
            RangeBoundaryOptions.IncludeStart | RangeBoundaryOptions.IncludeEnd);

        return instructionBuilder.Build();
    }

    private static IEnumerable<CodeInstruction> OnAllLogosTranspiler(IEnumerable<CodeInstruction> codeInstructions)
    {
        var instructionBuilder = new SequenceSkipBuilder<CodeInstruction>(codeInstructions);
        instructionBuilder.ReplaceNext(code =>
                code.opcode == OpCodes.Call && code.operand.ToString()
                    .Contains(
                        "System.Threading.CancellationToken GetCancellationTokenOnDestroy(UnityEngine.Component)"),
            new CodeInstruction(OpCodes.Call, typeof(CancellationToken).GetMethod("get_None")));
        return instructionBuilder.Build();
    }

    private static IEnumerable<CodeInstruction> AwakeMethodTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        var globalSettingInitMethod = typeof(PlayerGlobalSettings)
            .GetMethod("Initialize", BindingFlags.Static | BindingFlags.Public);

        var instructionBuilder = new SequenceSkipBuilder<CodeInstruction>(instructions);
        instructionBuilder.SkipUntil(code =>
            code.opcode == OpCodes.Call && code.operand as MethodInfo == globalSettingInitMethod);

        return instructionBuilder.Build();
    }

    private static IEnumerable<CodeInstruction> TransitionToNextSceneMethodTranspiler(
        IEnumerable<CodeInstruction> codeInstructions)
    {
        var instructions = codeInstructions.ToList();
        var instructionBuilder = new SequenceSkipBuilder<CodeInstruction>(instructions);

        var switchInstruction = instructions.First(i => i.opcode == OpCodes.Switch);
        if (switchInstruction.operand is Label[] labels)
        {
            var newLabels = labels.ToList();
            newLabels.RemoveAt(0);
            switchInstruction.operand = newLabels.ToArray();
        }

        instructionBuilder
            .ReplaceNext(i => i.opcode == OpCodes.Switch, switchInstruction)
            .SkipNextEdges(
            start => start.opcode == OpCodes.Call && start.operand.ToString().Contains("Boolean get_GoToIntent()"),
            end => end.opcode == OpCodes.Ldsfld && end.operand.ToString()
                .Contains("SodaDen.Pacha.UI.Disclaimer"), RangeBoundaryOptions.IncludeEnd)
            .SkipNextEdges(
            start => start.opcode == OpCodes.Call && start.operand.ToString().Contains("Boolean get_GoToIntent()"),
            end => end.opcode == OpCodes.Call && end.operand.ToString()
                .Contains("Void LoadAddressableAsync(System.String, Boolean)"));

        return instructionBuilder.Build();
    }
}