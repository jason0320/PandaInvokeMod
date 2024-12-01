using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace Mod_PandaInvokeMod
{
    internal class InvokePatch
    {
        public static void OnStartCore()
        {
            SourceManager sources = Core.Instance.sources;
            foreach (SourceElement.Row row in sources.elements.rows)
            {
                string alias = row.alias;
                string text = alias;
                InvokePatch.EnchRewrite(row);
            }
        }
        public static void EnchRewrite(SourceElement.Row ele)
        {
            string[] array = new string[]
            {
                "arrow_",
                "hand_",
                "bolt_",
                "ball_",
                "miasma_",
                "funnel_",
                "weapon_",
                "breathe_",
                "puddle_"
            };

            string[] array2 = new string[]
            {
                "Meteor",
                "Earthquake",
                "Summon",
                "Heal",
                "Holy",
                "Weak",
                "SpSpeed",
                "Darkness",
                "Web",
                "CatsEye",
                "Wisdom",
                "Hero",
                "Silence"
            };

            foreach (string text in array)
            {
                if (ele.alias.Contains(text) && ele.alias!=text)
                {
                    ele.encSlot = "weapon";
                    ele.tag = ele.tag.Where(s => s != "neg").ToArray();
                    ele.tag = ele.tag.AddItem("modRanged").ToArray();
                    ele.tag = ele.tag.AddItem("cane").ToArray();
                }
            }

            foreach (string text in array2)
            {
                if (ele.alias.Contains(text) && ele.alias.StartsWith("Sp"))
                {
                    ele.encSlot = "weapon";
                    ele.tag = ele.tag.Where(s => s != "neg").ToArray();
                    ele.tag = ele.tag.AddItem("modRanged").ToArray();
                    ele.tag = ele.tag.AddItem("cane").ToArray();
                }
            }
        }

    }
}
