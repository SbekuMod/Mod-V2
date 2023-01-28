using HarmonyLib;
using SbekuMod.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SbekuMod.patches
{


    [HarmonyPatch]
    public class AudioLibraryPatch
    {
        private static readonly Dictionary<CustomAudioType, string> _Library = new()
        {
            { CustomAudioType.CUSTOM_REEL_SLIDE_BG, "Assets/Models/SbekuMusic.mp3" },
            { CustomAudioType.SBK_SIGNALSCOPE, "Assets/Models/OWSignalscope.mp3" },
            { CustomAudioType.DREAM, "Assets/Sounds/Dream.mp3" },
            { CustomAudioType.BELLS_MUFFLED, "Assets/Sounds/Bells_Muffled.mp3" },
            { CustomAudioType.BELLS, "Assets/Sounds/Bells.mp3" },
            { CustomAudioType.CAMPFIRE, "Assets/Sounds/Campfire.mp3" },
            { CustomAudioType.ELK_KILL, "Assets/Sounds/ElkKill.mp3" },
            { CustomAudioType.SPLASH, "Assets/Sounds/Splash.mp3" },
            { CustomAudioType.WAKE, "Assets/Sounds/Wake.mp3" },
            { CustomAudioType.TEST_ENDING, "Assets/Sounds/StereonautiFinaleOW.wav" },
            { CustomAudioType.CLOCK, "Assets/Sounds/Clock.mp3" },
            { CustomAudioType.CRUNCH, "Assets/Sounds/Crunch.mp3" },
            { CustomAudioType.THE_GRATE_FILTER, "Assets/Sounds/TheGrateFilter.mp3" },
            { CustomAudioType.BLOW, "Assets/Sounds/Soffione.mp3" },
            { CustomAudioType.GHOST_IN_THE_MACHINE, "Assets/Sounds/GhostInTheMachine.mp3" },
            { CustomAudioType.GHOST_IN_THE_MACHINE_SIGNAL, "Assets/Sounds/GhostInTheMachine_Muffled.mp3" },
            { CustomAudioType.SNM_ENDING, "Assets/Sounds/SNMEnding.mp3" },
            { CustomAudioType.SNM_MAIN_MENU, "Assets/Sounds/SNMMainMenu.mp3" },
            { CustomAudioType.SNM_BEGINNING, "Assets/Sounds/SNMBeginning.mp3" },
            //SIGNALSCOPE INDIVIDUAL SIGNALS
            { CustomAudioType.SIGNALSCOPE_BANJO_1, "Assets/Sounds/Signals/banjo_1.mp3" },// 1_900
            { CustomAudioType.SIGNALSCOPE_BANJO_2, "Assets/Sounds/Signals/banjo_2.mp3" },// FireArrows
            { CustomAudioType.SIGNALSCOPE_BOUZOUKI_1, "Assets/Sounds/Signals/bouzouki_1.mp3" },// GhostInTheMachine
            { CustomAudioType.SIGNALSCOPE_BOW_BOUZOUKI, "Assets/Sounds/Signals/bow_bouzouki.mp3" },// NeverGetMeAlive
            { CustomAudioType.SIGNALSCOPE_BOWS, "Assets/Sounds/Signals/bows.mp3" },
            { CustomAudioType.SIGNALSCOPE_CONTRABASS, "Assets/Sounds/Signals/contrabass.mp3" },
            { CustomAudioType.SIGNALSCOPE_GUITAR_1, "Assets/Sounds/Signals/guitar_1.mp3" },// SleepWakeRepeat
            { CustomAudioType.SIGNALSCOPE_GUITAR_2, "Assets/Sounds/Signals/guitar_2.mp3" },// TheGrateFilter
            { CustomAudioType.SIGNALSCOPE_GUITAR_3, "Assets/Sounds/Signals/guitar_3.mp3" },
            { CustomAudioType.SIGNALSCOPE_GUITAR_MELODY, "Assets/Sounds/Signals/guitar_melody.mp3" },
            { CustomAudioType.SIGNALSCOPE_PERCUSSIONS, "Assets/Sounds/Signals/percussions.mp3" },
            { CustomAudioType.SIGNALSCOPE_RHODES, "Assets/Sounds/Signals/rhodes.mp3" },
            { CustomAudioType.SIGNALSCOPE_WOODWINDS, "Assets/Sounds/Signals/woodwinds.mp3" },
            //
        };

        private static bool IsInitialized = false;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AudioLibrary), nameof(AudioLibrary.BuildAudioEntryDictionary))]
        public static void AudioLibrary_BuildAudioEntryDictionary_Prefix(AudioLibrary __instance)
        {
            try
            {
                if (IsInitialized) return;

                List<AudioLibrary.AudioEntry> audioEntries = new(__instance.audioEntries);

                var initialAudioEntryCount = audioEntries.Count;

                foreach (var audioEntryPath in _Library)
                {
                    SbekuMod.Instance.ModHelper.Console.WriteLine($"LOADING CUSTOM AUDIO LIBRARY: {audioEntryPath.Key} - {audioEntryPath.Value}");
                    var audioClip = AssetLibrary.GetAsset<AudioClip>(audioEntryPath.Value);

                    if(audioClip == null)
                    {
                        SbekuMod.Instance.ModHelper.Console.WriteLine($"REQUESTED FILE {audioEntryPath.Value} MISSING", OWML.Common.MessageType.Error);
                        continue;
                    }

                    audioEntries.Add(new AudioLibrary.AudioEntry((AudioType)audioEntryPath.Key, new AudioClip[]{ audioClip }));
                }

                __instance.audioEntries = audioEntries.ToArray();

                SbekuMod.Instance.ModHelper.Console.WriteLine($"CUSTOM AUDIO ENTRIES LOADED: {audioEntries.Count - initialAudioEntryCount} Entries Added", OWML.Common.MessageType.Info);

                IsInitialized = true;

            } catch(Exception e)
            {
                SbekuMod.Instance.ModHelper.Console.WriteLine("REEL AUDIO ERROR: " + e.Message);
            }

        }

    }
}
