using HarmonyLib;


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
        public static void GameOverController_OnDestroy_Postfix(GameOverController __instance)
        {
            GlobalMessenger.RemoveListener("TriggerDeathByHornfels", new Callback(OnTriggerDeathByHornfels));
        }

        private static void OnTriggerDeathByHornfels() {
            _controllerInstance._deathText.text = "I TUOI DUBBI ERANO FONDATI:\nSCOGLIOCORNO HA DISTRUTTO LA FIBRA DELLO SPAZIO TEMPO\nSOLO PER FARTI MORIRE UN'ALTRA VOLTA";
            _controllerInstance.SetupGameOverScreen(6f);
        }

    }
}
