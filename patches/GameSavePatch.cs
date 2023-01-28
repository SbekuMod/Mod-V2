using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SbekuMod.patches
{
    [HarmonyPatch]
    public class GameSavePatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameSave), nameof(GameSave.SetDefaultValuesOnDeserialized))]
        public static bool GameSave_SetDefaultValuesOnDeserialized_Prefix(GameSave __instance)
        {
            if (__instance.knownFrequencies.Length < 9)
            {
                Array.Resize<bool>(ref __instance.knownFrequencies, 9);
            }

            return false;
        }

    }
}
