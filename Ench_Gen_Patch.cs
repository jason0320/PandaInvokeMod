using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using HarmonyLib;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

[HarmonyPatch]
internal class Ench_Gen_Patch
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Thing), nameof(Thing.GetEnchant))]
    internal static IEnumerable<CodeInstruction> GetEnchantIl(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchStartForward(
                new CodeMatch(o => o.opcode == OpCodes.Ldstr &&
                                   o.operand.ToString().Contains("resist")))
            .InsertAndAdvance(
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Ldstr, "ability"),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(string), "op_Equality")),
                new CodeInstruction(OpCodes.Or))
            .InstructionEnumeration();
    }
}
