using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SbekuMod.patches.AudioSignalPatch;

namespace SbekuMod.patches
{
    [HarmonyPatch]
    public class PlayerDataPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.LearnSignal))]
        public static void PlayerData_LearnSignal_Prefix()
        {
            foreach (var signalValue in Enum.GetValues(typeof(CustomSignalName)))
            {
                if (!PlayerData._currentGameSave.knownSignals.ContainsKey((int)signalValue))
                    PlayerData._currentGameSave.knownSignals.Add((int)signalValue, false);
            }
        }

    }
}
