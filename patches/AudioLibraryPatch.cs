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
