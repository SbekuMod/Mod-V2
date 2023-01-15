using HarmonyLib;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace SbekuMod.patches
{
    [HarmonyPatch]
    public class AudioSignalPatch
    {
        // FREQUENCIES ARE POWERS OF 2
        public enum CustomSignalFrequency
        {
            CUSTOM_REELS = 256
        }

        public enum CustomSignalName
        {
            Test = 102,
            [EnumMember(Value = "SLEEP_WAKE_REPEAT")]
            SleepWakeRepeat = 103,
            [EnumMember(Value = "THE_GRATE_FILTER")]
            TheGrateFilter = 104,
            [EnumMember(Value = "ACHIEVEMENT_1_900")]
            ACHIEVEMENT_1_900 = 105,
            [EnumMember(Value = "NEVER_GET_ME_ALIVE")]
            NEVER_GET_ME_ALIVE = 106,
            [EnumMember(Value = "GHOST_IN_THE_MACHINE")]
            GHOST_IN_THE_MACHINE = 107,
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AudioSignal), nameof(AudioSignal.SignalNameToString))]
        public static bool AudioSignal_SignalNameToString_Prefix(SignalName name, ref string __result)
        {
            SbekuMod.Instance.ModHelper.Console.WriteLine($"LOADING SIGNAL {name}");
            switch((CustomSignalName)name)
            {
                case CustomSignalName.Test: 
                    __result = "POGGERS";
                    return false;
                case CustomSignalName.SleepWakeRepeat:
                    __result = "DORMI SVEGLIA RIPETI";
                    return false;
                case CustomSignalName.TheGrateFilter:
                    __result = "IL FILTRO DELLA GRATA";
                    return false;
                case CustomSignalName.ACHIEVEMENT_1_900:
                    __result = "1/900";
                    return false;
                case CustomSignalName.NEVER_GET_ME_ALIVE:
                    __result = "NON MI PRENDERETE MAI VIVO";
                    return false;
                case CustomSignalName.GHOST_IN_THE_MACHINE:
                    __result = "FANTASMI NELLA MACCHINA";
                    return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AudioSignal), nameof(AudioSignal.IndexToFrequency))]
        public static bool AudioSignal_IndexToFrequency_Prefix(int index, ref SignalFrequency __result)
        {
            if(index == 7)
            {
                __result = (SignalFrequency)CustomSignalFrequency.CUSTOM_REELS;
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AudioSignal), nameof(AudioSignal.FrequencyToIndex))]
        public static bool AudioSignal_FrequencyToIndex_Prefix(SignalFrequency frequency, ref int __result)
        {
            if ((CustomSignalFrequency)frequency == CustomSignalFrequency.CUSTOM_REELS)
            {
                __result = 7;
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AudioSignal), nameof(AudioSignal.FrequencyToString))]
        public static bool AudioSignal_FrequencyToString_Prefix(SignalFrequency frequency, ref string __result)
        {
            if ((CustomSignalFrequency)frequency == CustomSignalFrequency.CUSTOM_REELS)
            {
                __result = "ECHI DEL DESERTO";
                return false;
            }
            return true;
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(AudioSignal), nameof(AudioSignal.UpdateSignalStrength))]
        public static bool AudioSignal_UpdateSignalStrength_Prefix(Signalscope scope, float distToClosestScopeObstruction, AudioSignal __instance)
        {
            __instance._canBePickedUpByScope = false;
            if (__instance._sunController != null && __instance._sunController.IsPointInsideSupernova(__instance.transform.position))
            {
                __instance._signalStrength = 0f;
                __instance._degreesFromScope = 180f;
                return false;
            }
            if (Locator.GetQuantumMoon() != null && Locator.GetQuantumMoon().IsPlayerInside() && __instance._name != SignalName.Quantum_QM)
            {
                __instance._signalStrength = 0f;
                __instance._degreesFromScope = 180f;
                return false;
            }
            if (!__instance._active || !__instance.gameObject.activeInHierarchy || __instance._outerFogWarpVolume != PlayerState.GetOuterFogWarpVolume() || (scope.GetFrequencyFilter() & __instance._frequency) != __instance._frequency)
            {
                __instance._signalStrength = 0f;
                __instance._degreesFromScope = 180f;
                return false;
            }
            __instance._scopeToSignal = __instance.transform.position - scope.transform.position;
            __instance._distToScope = __instance._scopeToSignal.magnitude;
            if (__instance._outerFogWarpVolume == null && distToClosestScopeObstruction < 1000f && __instance._distToScope > 1000f)
            {
                __instance._signalStrength = 0f;
                __instance._degreesFromScope = 180f;
                return false;
            }
            __instance._canBePickedUpByScope = true;
            if (__instance._distToScope < __instance._sourceRadius)
            {
                __instance._signalStrength = 1f;
            }
            else
            {
                __instance._degreesFromScope = Vector3.Angle(scope.GetScopeDirection(), __instance._scopeToSignal);
                float num = Mathf.InverseLerp(2000f, 1000f, __instance._distToScope);
                float num2 = Mathf.Lerp(45f, 90f, num);
                float num3 = 57.29578f * Mathf.Atan2(__instance._sourceRadius, __instance._distToScope);
                float num4 = Mathf.Lerp(Mathf.Max(num3, 5f), Mathf.Max(num3, 1f), scope.GetZoomFraction());
                __instance._signalStrength = Mathf.Clamp01(Mathf.InverseLerp(num2, num4, __instance._degreesFromScope));
            }
            // WHEN INSIDE THE STRANGER IGNORE ALL SIGNALS WITH THE EXCEPTION OF OUR CUSTOM REELS
            if (Locator.GetCloakFieldController() != null && __instance._frequency != (SignalFrequency)CustomSignalFrequency.CUSTOM_REELS)
            {
                float num5 = 1f - Locator.GetCloakFieldController().playerCloakFactor;
                __instance._signalStrength *= num5;
                if (OWMath.ApproxEquals(num5, 0f, 0.001f))
                {
                    __instance._signalStrength = 0f;
                    __instance._degreesFromScope = 180f;
                    return false;
                }
            }
            if (__instance._distToScope < __instance._identificationDistance + __instance._sourceRadius && __instance._signalStrength > 0.9f)
            {
                if (!PlayerData.KnowsFrequency(__instance._frequency) && !__instance._preventIdentification)
                {
                    __instance.IdentifyFrequency();
                }
                if (!PlayerData.KnowsSignal(__instance._name) && !__instance._preventIdentification)
                {
                    __instance.IdentifySignal();
                }
                if (__instance._revealFactID.Length > 0)
                {
                    Locator.GetShipLogManager().RevealFact(__instance._revealFactID, true, true);
                }
            }

            return false;
        }

    }
}
