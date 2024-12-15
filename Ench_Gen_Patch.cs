using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(Thing))]
[HarmonyPatch("GetEnchant")]
[HarmonyPatch(new Type[]
{
    typeof(Thing)
})]
internal class Ench_Gen_Patch
{
    public static Tuple<SourceElement.Row, int> GetEnchant(int lv, Func<SourceElement.Row, bool> func, bool neg)
    {
        List<SourceElement.Row> list = new List<SourceElement.Row>();
        int num = 0;
        int num2 = lv + 5 + EClass.rndSqrt(10);
        foreach (SourceElement.Row row in EClass.sources.elements.rows)
        {
            if ((!neg || !row.tag.Contains("flag")) && func(row) && row.LV < num2)
            {
                list.Add(row);
                num += row.chance;
            }
        }
        if (num == 0)
        {
            return null;
        }
        int num3 = EClass.rnd(num);
        int num4 = 0;
        foreach (SourceElement.Row item in list)
        {
            num4 += item.chance;
            if (num3 < num4)
            {
                string text = EClass.sources.elements.map[item.id].category;
                bool flag = text == "skill" || text == "attribute" || text == "resist" || text == "ability";
                float num5 = (float)(3 + Mathf.Min(lv / 10, 15)) + Mathf.Sqrt(lv * item.encFactor / 100);
                int num6 = (item.mtp + EClass.rnd(item.mtp + (int)num5)) / item.mtp * ((!(flag && neg)) ? 1 : (-1));
                return new Tuple<SourceElement.Row, int>(item, num6);
            }
        }
        return null;
    }
}
