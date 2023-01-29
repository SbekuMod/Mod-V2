using Newtonsoft.Json;
using SbekuMod.components;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static SbekuMod.patches.AudioSignalPatch;


namespace SbekuMod.utils
{
    public class ReelSetup
    {
        private static readonly string BASE_PATH = "assets/reels";
        private static readonly string[] REEL_LIST = new string[]
        {
            "SleepWakeRepeat",
            "TheGrateFilter",
            "1_900",
            "NeverGetMeAlive",
            "GhostInTheMachine",
            "TestEndVision",
            "FireArrows",
            "Beginning",
            "AroundTheWorld"
        };

        //private static List<CustomAudioSignal> activeAudioSignals = new List<CustomAudioSignal>();
        private static readonly Dictionary<string, GameObject> PrefabCache = new();
        public static void SetupReels() {

            //CLEANUP PREVIOUS SIGNALS
            //foreach (var audioSignal in activeAudioSignals)
            //    audioSignal.OnDestroy();
            //activeAudioSignals = new List<CustomAudioSignal>();
            //var reelManager = new GameObject();
            //reelManager.name = "ReelSignalManager";
            //reelManager.AddComponent<ReelSignalManager>();

            var reelBasePath = Path.Combine(UtilityHelper.GetProjectBasePath(), BASE_PATH);

            if (!Directory.Exists(reelBasePath))
            {
                SbekuMod.Instance.ModHelper.Console.WriteLine("REEL DIRECTORY MISSING: Aborting import", OWML.Common.MessageType.Warning);
                return;
            }

            var reelDataList = new List<ReelDataObject>();

            foreach(var reelName in REEL_LIST)
            {
                try
                {
                    var filePath = Path.Combine(UtilityHelper.GetProjectBasePath(), BASE_PATH, $"{reelName}.json");

                    SbekuMod.Instance.ModHelper.Console.WriteLine($"PARSING {filePath}", OWML.Common.MessageType.Info);

                    if (!File.Exists(filePath))
                    {
                        SbekuMod.Instance.ModHelper.Console.WriteLine($"REEL {reelName} NOT FOUND", OWML.Common.MessageType.Warning);

                        continue;
                    }

                    var json = File.ReadAllText(filePath);

                    var reelData = JsonConvert.DeserializeObject<ReelDataObject>(json);

                    SbekuMod.Instance.ModHelper.Console.WriteLine($"PARSED {reelData.Name}", OWML.Common.MessageType.Info);

                    reelDataList.Add(reelData);

                }catch(Exception e)
                {
                    SbekuMod.Instance.ModHelper.Console.WriteLine($"ERROR PARSING REEL {reelName}: {e.Message}", OWML.Common.MessageType.Error);
                }
            }

            SbekuMod.Instance.ModHelper.Console.WriteLine($"LOADING {reelDataList.Count} REELS", OWML.Common.MessageType.Info);

            foreach (var reelData in reelDataList) CreateReel(reelData);

        }

        private static void CreateReel(ReelDataObject reelData)
        {
            try
            {
                SbekuMod.Instance.ModHelper.Console.WriteLine("LOADING REEL " + reelData.Name);

                if (!PrefabCache.TryGetValue(reelData.Prefab, out GameObject reelPrefab))
                    reelPrefab = AssetLibrary.GetAsset<GameObject>(reelData.Prefab);

                var reel = GameObject.Instantiate(reelPrefab, Vector3.zero, Quaternion.Euler(0, 0, 0));

                if (reelData.Name != null) reel.name = reelData.Name;

                var slides = new List<SlideData>();
                foreach (var slideDataItem in reelData.Slides)
                {
                    var slideData = SlideData.CreateSlideData(slideDataItem.Path, duration: slideDataItem.Duration, backdropAudio: slideDataItem.BackdropAudio, beatAudio: slideDataItem.BeatAudio);

                    slides.Add(slideData);
                }

                if (reelData.SignalAudio != null && reelData.Signal != null)
                {
                    var signal = reel.AddComponent<SnmSignal>();
                    signal.Initialize(reelData.SignalAudio.Value, reelData.Signal.Value, reelData.Sector, reelData.IsFirst, frequency: reelData.SignalFrequency.Value);
                    //activeAudioSignals.Add(audioSignal);
                }

                if (reelData.Parent != null)
                {
                    var sectorVillage = GameObject.Find(reelData.Parent);
                    reel.transform.SetParent(sectorVillage.transform);
                }

                HideableFromEntryway hideableFromEntryway = null;
                if (!string.IsNullOrEmpty(reelData.Entryway))
                {
                    hideableFromEntryway = reel.AddComponent<HideableFromEntryway>();
                    hideableFromEntryway.Setup(reelData.Entryway);
                }

                if (reelData.Type == ReelType.SLIDE)
                    reel.AddComponent<ExternalSlideReel>().Setup(slides.ToArray(), isFirst: reelData.IsFirst, hideableFromEntryway: hideableFromEntryway);

                if (reelData.Type == ReelType.PROJECTION)
                    reel.AddComponent<ExternalProjectionReel>().Setup(slides.ToArray());

                var position = reelData.InitialPosition;

                reel.transform.localPosition = new Vector3(position.X, position.Y, position.Z);
                var rotationData = reelData.InitialRotation;
                var rotation = reel.transform.localRotation;
                rotation.eulerAngles = new Vector3(rotationData.X, rotationData.Y, rotationData.Z);
                reel.transform.localRotation = rotation;

                SbekuMod.Instance.ModHelper.Console.WriteLine($"REEL {reel.name} LOADED");
            }catch(Exception e)
            {
                SbekuMod.Instance.ModHelper.Console.WriteLine($"ERROR CREATING REEL {reelData.Name}: {e.Message}");
            }

        }

    }
}
