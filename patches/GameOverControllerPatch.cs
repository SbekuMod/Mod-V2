using HarmonyLib;
using SbekuMod.utils;

namespace SbekuMod.patches
{
    [HarmonyPatch]
    public class GameOverControllerPatch
    {
        private static GameOverController _controllerInstance = null;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameOverController), nameof(GameOverController.Awake))]
        public static void GameOverController_Awake_Postfix(GameOverController __instance)
        {
            _controllerInstance = __instance;
            GlobalMessenger.AddListener("TriggerDeathByHornfels", new Callback(OnTriggerDeathByHornfels));
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameOverController), nameof(GameOverController.OnDestroy))]
        public static void GameOverController_OnDestroy_Postfix()
        {
            GlobalMessenger.RemoveListener("TriggerDeathByHornfels", new Callback(OnTriggerDeathByHornfels));
        }

        private static void OnTriggerDeathByHornfels() {
            _controllerInstance._deathText.text = UITextLibrary.GetString((UITextType)CustomTextType.PARADOX_ENDING_TEXT);
            _controllerInstance.SetupGameOverScreen(6f);
        }

    }
}
