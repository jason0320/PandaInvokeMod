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
    internal static IEnumerable<CodeInstruction> OnSetCategoryFlagIl(IEnumerable<CodeInstruction> instructions)
    {
        var cm = new CodeMatcher(instructions);
        return cm.MatchStartForward(
                new CodeMatch(OpCodes.Ldloc_S),
                new CodeMatch(OpCodes.Ldstr),
                new CodeMatch(OpCodes.Call),
                new CodeMatch(OpCodes.Brtrue))
            .Insert(
                cm.InstructionsInRange(cm.Pos, cm.Pos + 3))
            .Advance(1)
            .SetOperandAndAdvance("ability")
            .InstructionEnumeration();
    }
}
