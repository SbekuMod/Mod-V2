using Newtonsoft.Json;
using SbekuMod.components;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static SbekuMod.patches.AudioSignalPatch;

//GET AND SET ROTATION FROM CONSOLE OF UNIY EXPLORER
//var reel = GameObject.Find("TEST_REEL");
//var rotation = reel.transform.rotation;

//rotation.eulerAngles = new Vector3((float)57.55733, (float)279.8751, (float)126.4894);

//var angles = rotation.eulerAngles;
//reel.transform.rotation = rotation;
//UnityExplorer.ExplorerCore.Log("X " + angles.x + " Y " + angles.y + " Z " + angles.z);


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
            "FireArrows"
        };

        private static List<CustomAudioSignal> activeAudioSignals = new List<CustomAudioSignal>();
        private static readonly Dictionary<string, GameObject> PrefabCache = new();
        public static void SetupReels() {

            //CLEANUP PREVIOUS SIGNALS
            foreach (var audioSignal in activeAudioSignals)
                audioSignal.OnDestroy();
            activeAudioSignals = new List<CustomAudioSignal>();

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
                    var audioSource = reel.AddComponent<AudioSource>();
                    audioSource.dopplerLevel = 0;
                    audioSource.loop = true;
                    audioSource.maxDistance = 15f;
                    audioSource.minDistance = 3f;
                    audioSource.volume = 0.50f;
                    audioSource.spatialBlend = 1;
                    audioSource.rolloffMode = AudioRolloffMode.Custom;
                    audioSource.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
                    audioSource.playOnAwake = false;

                    var owAudioSource = reel.AddComponent<CustomOWAudioSource>();
                    owAudioSource.Setup(reelData.SignalAudio.Value, OWAudioSource.ClipSelectionOnPlay.RANDOM, OWAudioMixer.TrackName.Signal);
                    var audioSignal = reel.AddComponent<CustomAudioSignal>();
                    audioSignal._onlyAudibleToScope = true;

                    if (reelData.Sector != null)
                        audioSignal._sector = GameObject.Find(reelData.Sector).GetComponent<Sector>();

                    audioSignal.Setup((SignalFrequency)CustomSignalFrequency.CUSTOM_REELS, (SignalName)reelData.Signal.Value, 0.25f);
                    activeAudioSignals.Add(audioSignal);
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
                    reel.AddComponent<ExternalSlideReel>().Setup(slides.ToArray(), hideableFromEntryway: hideableFromEntryway);

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
