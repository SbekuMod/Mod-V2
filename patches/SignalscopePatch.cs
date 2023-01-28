using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SbekuMod.patches
{
    [HarmonyPatch]
    public class SignalscopePatch
    {

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Signalscope), nameof(Signalscope.Awake))]
        public static void Signalscope_Awake_Prefix(Signalscope __instance)
        {
            __instance._strongestSignals = new AudioSignal[9];
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Signalscope), nameof(Signalscope.SwitchFrequencyFilter))]
        public static bool Signalscope_SwitchFrequencyFilter_Prefix(Signalscope __instance, int increment = 1)
        {
            __instance._frequencyFilterIndex += increment;
            __instance._frequencyFilterIndex = ((__instance._frequencyFilterIndex > 8) ? 1 : __instance._frequencyFilterIndex);
            __instance._frequencyFilterIndex = ((__instance._frequencyFilterIndex < 1) ? 8 : __instance._frequencyFilterIndex);
            SignalFrequency signalFrequency = AudioSignal.IndexToFrequency(__instance._frequencyFilterIndex);
            if (__instance._frequencyFilterIndex != 0 && !PlayerData.KnowsFrequency(signalFrequency) && (!__instance._isUnknownFreqNearby || __instance._unknownFrequency != signalFrequency))
            {
                __instance.SwitchFrequencyFilter(increment);
            }

            return false;
        }

    }
}
