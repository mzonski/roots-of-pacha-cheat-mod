using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;

namespace CheatMod.Core.Extensions;

public static class HarmonyExtensions
{
    public static void PatchAsyncMethod(this Harmony harmony, MethodBase target,
        MethodInfo prefix = null,
        MethodInfo postfix = null,
        MethodInfo transpiler = null,
        MethodInfo finalizer = null,
        MethodInfo ilManipulator = null)
    {
        var stateMachineAttr = target.GetCustomAttribute<AsyncStateMachineAttribute>();

        if (stateMachineAttr is null)
        {
            throw new ArgumentException(
                $"The method '{target.Name}' is not an asynchronous method with a state machine");
        }

        var stateMachineType = stateMachineAttr.StateMachineType;
        var moveNextMethod = stateMachineType.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);

        if (moveNextMethod is null)
        {
            throw new ArgumentException(
                $"The method '{target.Name}' is not an asynchronous method with a state machine");
        }

        harmony.PatchMethod(moveNextMethod, prefix, postfix, transpiler, finalizer, ilManipulator);
    }
    public static void PatchMethod(this Harmony harmony, MethodBase target,
        MethodInfo prefix = null,
        MethodInfo postfix = null,
        MethodInfo transpiler = null,
        MethodInfo finalizer = null,
        MethodInfo ilManipulator = null)
    {
        var patcher = harmony.CreateProcessor(target);
        if (prefix is not null) patcher.AddPrefix(new HarmonyMethod(prefix));
        if (postfix is not null) patcher.AddPostfix(new HarmonyMethod(postfix));
        if (transpiler is not null) patcher.AddTranspiler(new HarmonyMethod(transpiler));
        if (finalizer is not null) patcher.AddFinalizer(new HarmonyMethod(finalizer));
        if (ilManipulator is not null) patcher.AddILManipulator(new HarmonyMethod(ilManipulator));

        patcher.Patch();
    }
}