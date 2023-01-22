using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SbekuMod.patches
{
    [HarmonyPatch]
    public class TimelineObliterationControllerPatch
    {
        public static bool EasterEggObliteration = false;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TimelineObliterationController), nameof(TimelineObliterationController.CompleteTimelineObliteration))]
        public static bool TimelineObliterationController_CompleteTimelineObliteration_Prefix(TimelineObliterationController __instance)
        {
            if (!EasterEggObliteration) return true;

            __instance._cameraEffect.OnRealityShatterEffectComplete -= __instance.CompleteTimelineObliteration;
            TimelineObliterationController.s_hasRealityEnded = true;
            PlayerData.SetPersistentCondition("DESTROYED_TIMELINE_LAST_SAVE", true);
            PlayerData.RevertParadoxLoopCountStates();
            GlobalMessenger.FireEvent("TriggerDeathByHornfels");

            return false;
        }

    }
}
