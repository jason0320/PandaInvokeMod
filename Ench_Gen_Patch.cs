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
        float num3 = (float)(3 + Mathf.Min(lv / 10, 15)) + Mathf.Sqrt((float)lv);
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
        int num4 = EClass.rnd(num);
        int num5 = 0;
        foreach (SourceElement.Row row2 in list)
        {
            num5 += row2.chance;
            if (num4 < num5)
            {
                string category = EClass.sources.elements.map[row2.id].category;
                bool flag = category == "skill" || category == "attribute" || category == "resist" || category == "ability";
                int item = (row2.mtp + EClass.rnd(row2.mtp + (int)num3)) / row2.mtp * ((flag && neg) ? -1 : 1);
                return new Tuple<SourceElement.Row, int>(row2, item);
            }
        }
        return null;
    }
}
