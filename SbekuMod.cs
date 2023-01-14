using OWML.Common;
using OWML.ModHelper;
using HarmonyLib;
using System.Reflection;
using SbekuMod.storage;
using SbekuMod.utils;
using UnityEngine.InputSystem;
using UnityEngine;
using static SbekuMod.patches.AudioSignalPatch;

namespace SbekuMod
{
    public class SbekuMod : ModBehaviour
    {
        public static SbekuMod Instance;
        public static EventStorage EventStorage;
        public static string CurrentLanguage = null;

        private static readonly string RELOAD_DIALOGS_SETTING_KEY = "Premi K per ricaricare i dialoghi";
        private static readonly string UNLOCK_EVERYTHING_SETTING_KEY = "Premi U per sbloccare tutti gli eventi";

        private void Awake()
        {
            Instance = this;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }

        private void Update()
        {
            if (ModHelper.Config.GetSettingsValue<bool>(UNLOCK_EVERYTHING_SETTING_KEY) && Keyboard.current.uKey.wasPressedThisFrame)
                ShipLogUtility.RevealAllFacts();

            if (ModHelper.Config.GetSettingsValue<bool>(RELOAD_DIALOGS_SETTING_KEY) && Keyboard.current.kKey.wasPressedThisFrame) { 
                ModReloader.ReloadDialogs(); 
                ModReloader.ReloadTranslation();
            }
        }


        private void InitializeLanguage()
        {
            TextTranslation translation = FindObjectOfType<TextTranslation>();
            CurrentLanguage = TextTranslation.s_langFolder[(int)translation.m_language];
            ModHelper.Console.WriteLine("CURRENT LANGUAGE: " + CurrentLanguage);
        }

        private void Start()
        {
            EventStorage = new EventStorage();
            InitializeLanguage();

            var titleScreenManager = FindObjectOfType<TitleScreenManager>();
            titleScreenManager._cameraController.OnLogoPanComplete += () =>
            {
                Popups.ShowWelcomePopup();
            };

            ModHelper.Menus.MainMenu.OnInit += () =>
            {
                MainMenuUtility.ReplaceDLCLogo();

                if (Popups.CanShowCredits())
                {
                    var button = ModHelper.Menus.MainMenu.OptionsButton.Duplicate("CREDITI MOD SNM");
                    button.OnClick += () => Popups.ShowCreditsPopup();
                }
            };

            LoadManager.OnCompleteSceneLoad += (scene, loadedScene) =>
            {
                if (loadedScene != OWScene.SolarSystem) return;

                ReelSetup.SetupReels();
            };

            GlobalMessenger.AddListener("PutOnHelmet", () => {
                PlayerData.LearnFrequency((SignalFrequency)CustomSignalFrequency.CUSTOM_REELS);

                ShipLogUtility.RevealAllLoadedFacts();
            });

            ModHelper.Console.WriteLine($"{nameof(SbekuMod)} initialized!", MessageType.Success);
        }
    }
}