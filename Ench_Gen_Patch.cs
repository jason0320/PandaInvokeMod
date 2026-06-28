using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using HarmonyLib;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;


[HarmonyPatch(typeof(Thing), nameof(Thing.GetEnchant))]
internal static class Ench_Gen_Patch
{
    static readonly MethodInfo ContainsMethod =
        AccessTools.Method(
            typeof(ClassExtension),
            nameof(ClassExtension.Contains),
            new[] { typeof(string[]), typeof(string) });

    static readonly FieldInfo TagField =
        AccessTools.Field(typeof(SourceElement.Row), nameof(SourceElement.Row.tag));

    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions,
        ILGenerator generator)
    {
        var list = new List<CodeInstruction>(instructions);

        for (int i = 0; i < list.Count - 4; i++)
        {
            if (list[i].opcode == OpCodes.Ldstr &&
                list[i].operand is string s &&
                s == "flag" &&
                list[i + 1].Calls(ContainsMethod) &&
                list[i + 2].opcode == OpCodes.Brtrue)
            {
                // Original:
                //
                // ldstr "flag"
                // call Contains
                // brtrue skipRow

                Label skipRow = (Label)list[i + 2].operand;

                Label continueLabel = generator.DefineLabel();

                // Change existing branch:
                //
                // brtrue skipRow
                //
                // into
                //
                // brfalse continueLabel

                list[i + 2].opcode = OpCodes.Brfalse;
                list[i + 2].operand = continueLabel;

                var injected = new List<CodeInstruction>
                {
                    // row.tag
                    new CodeInstruction(list[i - 2]),      // ldloc.s row
                    new CodeInstruction(list[i - 1]),      // ldfld tag

                    new CodeInstruction(OpCodes.Ldstr, "ability"),
                    new CodeInstruction(OpCodes.Call, ContainsMethod),

                    new CodeInstruction(OpCodes.Brtrue, skipRow)
                };

                // First original instruction after injected block
                // receives our new label.
                list.InsertRange(i + 3, injected);
                list[i + 3 + injected.Count].labels.Add(continueLabel);

                return list;
            }
        }

        throw new System.Exception("Couldn't find flag Contains()");
    }
}