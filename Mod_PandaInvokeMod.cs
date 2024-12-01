using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace Mod_PandaInvokeMod
{

    [BepInPlugin("panda.invoke.mod", "Panda's Invoke Mod", "1.0.0.0")]
    public class Mod_PandaInvokeMod : BaseUnityPlugin
    {
        private void Start()
        {
            var harmony = new Harmony("Panda's Invoke Mod");
            harmony.PatchAll();
        }
        public void OnStartCore()
        {
            InvokePatch.OnStartCore();
        }
    }
}