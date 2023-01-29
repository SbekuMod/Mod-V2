using OWML.Common;
using OWML.ModHelper;
using HarmonyLib;
using System.Reflection;
using SbekuMod.storage;
using SbekuMod.utils;
using UnityEngine.InputSystem;
using UnityEngine;
using static SbekuMod.patches.AudioSignalPatch;
using SbekuMod.components;
using static NomaiWarpPlatform;
using System;
using SbekuMod.patches;

namespace SbekuMod
{
    public class SbekuMod : ModBehaviour
    {
        public static SbekuMod Instance;
        public EventStorage EventStorage;
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

        private void InitializeMainMenu ()
        {
            VersionText.Setup();
            MainMenuUtility.ReplaceDLCLogo();
            MainMenuUtility.ReplaceMusic();


            var button = ModHelper.Menus.MainMenu.OptionsButton.Duplicate("CREDITI ECHOES OF THE DESERT");
            button.OnClick += () =>
            {
                if (EventStorage.Get().HasSeenEnding)
                    CreditsUtility.StartCredits(CreditsPatch.CustomCreditsType.FINAL);
                else
                    Popups.ShowCreditsToBeUnlocked();
                
            };

        }

        private void InitializeContent ()
        {
            if (!PlayerData._currentGameSave.dictConditions.TryGetValue("LAUNCH_CODES_GIVEN", out var hasLaunchCodes))
                hasLaunchCodes = false;

            if (!hasLaunchCodes) return;

            ShipLogUtility.RevealAllLoadedFacts();
        }

        private void InitializeDlcContent(Signalscope scope)
        {
            if (!PlayerData._currentGameSave.dictConditions.TryGetValue("LAUNCH_CODES_GIVEN", out var hasLaunchCodes))
                hasLaunchCodes = false;

            if (!hasLaunchCodes) return;
            try
            {
                var frequency = (SignalFrequency)CustomSignalFrequency.STORY_REELS;
                int num = AudioSignal.FrequencyToIndex(frequency);
                ModHelper.Console.WriteLine($"TRYING TO LEARN FREQUENCY {PlayerData._currentGameSave.knownFrequencies[num]}");
                if (!PlayerData._currentGameSave.knownFrequencies[num])
                {
                    PlayerData.LearnFrequency(frequency);
                    string text = UITextLibrary.GetString(UITextType.NotificationNewFreq) + " <color=orange>" + AudioSignal.FrequencyToString(frequency, false) + "</color>";
                    NotificationData notificationData = new(NotificationTarget.All, text, 10f, true);
                    NotificationManager.SharedInstance.PostNotification(notificationData, false);
                }
            }
            catch (Exception e)
            {
                ModHelper.Console.WriteLine($"ErrorLearning Frquency {e.Message}");
            }
        }

        public void Start()
        {
            InitializeLanguage();
            EventStorage = new EventStorage();
            StandaloneProfileManager.SharedInstance.OnProfileReadDone += () => EventStorage.Initialize();

            var titleScreenManager = FindObjectOfType<TitleScreenManager>();
            titleScreenManager._cameraController.OnLogoPanComplete += () => Popups.ShowWelcomePopup();

            ModHelper.Menus.MainMenu.OnInit += () => InitializeMainMenu();

            LoadManager.OnCompleteSceneLoad += (scene, loadedScene) =>
            {
                if (loadedScene == OWScene.SolarSystem)
                {
                    if (!PlayerData._currentGameSave.dictConditions.TryGetValue("LAUNCH_CODES_GIVEN", out var hasLaunchCodes))
                        hasLaunchCodes = false;

                    if (!hasLaunchCodes) return;

                    EasterEggUtility.InitializeParadoxEasterEgg();
                    ReelSetup.SetupReels();
                }
            };

            GlobalMessenger.AddListener("PutOnHelmet", InitializeContent);
            GlobalMessenger<Signalscope>.AddListener("EquipSignalscope", new Callback<Signalscope>(InitializeDlcContent));

            ModHelper.Console.WriteLine($"{nameof(SbekuMod)} initialized!", MessageType.Success);
        }
    }
}