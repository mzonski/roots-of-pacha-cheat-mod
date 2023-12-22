using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using HarmonyLib;

namespace CheatMod.Core.Extensions;

public static class ILExtensions
{
    public static string PrintCodeInstructions(this IEnumerable<CodeInstruction> instructions)
    {
        var output = new StringBuilder();
        var code = instructions.ToList();

        for (var i = 0; i < code.Count; i++)
        {
            var instruction = code[i];
            var operandDetails = GetOperandDetails(instruction.operand);

            output.Append($"Instruction {i}:\n" +
                          $"  Name: {instruction.opcode.Name}\n" +
                          $"  Value: {instruction.opcode.Value}\n" +
                          $"  Type: {instruction.opcode.OpCodeType}\n" +
                          $"  Operand Type: {instruction.opcode.OperandType}\n" +
                          $"  Operand: [{operandDetails}]\n");
        }

        return output.ToString();
    }

    private static string GetOperandDetails(object operand)
    {
        if (operand == null)
        {
            return "null";
        }

        switch (operand)
        {
            case MethodInfo methodInfo:
                return
                    $"MethodInfo: {methodInfo}, DeclaringType: {methodInfo.DeclaringType}, ReturnType: {methodInfo.ReturnType}";
            case FieldInfo fieldInfo:
                return
                    $"FieldInfo: {fieldInfo}, FieldType: {fieldInfo.FieldType}, DeclaringType: {fieldInfo.DeclaringType}";
            case LocalVariableInfo localVariableInfo:
                return $"LocalVariableInfo: {localVariableInfo}, LocalType: {localVariableInfo.LocalType}";
            case Label label:
                return $"Label: {label}";
            case Label[] labels:
                return $"Labels: [{string.Join(", ", labels)}]";
            default:
                return operand.ToString();
        }
    }
}