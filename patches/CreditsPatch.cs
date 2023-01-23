using Delaunay.LR;
using HarmonyLib;
using SbekuMod.utils;
using UnityEngine;
using UnityEngine.Video;

namespace SbekuMod.patches
{
    [HarmonyPatch]
    public class CreditsPatch
    {
        public static CustomCreditsType CustomCredits = CustomCreditsType.DEFAULT;
        private static Credits Credits = null;
        public static VideoPlayer VideoPlayer = null;
        public enum CustomCreditsType
        {
            DEFAULT,
            PARADOX,
            FINAL
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Credits), nameof(Credits.Start))]
        public static void Credits_Start_Prefix(Credits __instance)
        {
            if (TimelineObliterationControllerPatch.EasterEggObliteration)
            {
                TimelineObliterationControllerPatch.EasterEggObliteration = false;
                CustomCredits = CustomCreditsType.PARADOX;
            }


            if (CustomCredits != CustomCreditsType.DEFAULT)
            {
                Credits = __instance;
                __instance._currentPlayingSection = 0;
                __instance.EndPreview();
                StartVideo(CustomCredits);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Credits), nameof(Credits.LoadNextScene))]
        public static void Credits_LoadNextScene_Prefix()
        {
            if (VideoPlayer != null)
            {
                VideoPlayer = null;
                Credits = null;
                CustomCredits = CustomCreditsType.DEFAULT;
            }
        }

        private static VideoClip GetVideoFromType(CustomCreditsType type)
        {
            switch (type)
            {
                case CustomCreditsType.PARADOX:
                    return AssetLibrary.GetAsset<VideoClip>("Assets/Credits_Paradox.mp4");
                case CustomCreditsType.FINAL:
                default:
                    return AssetLibrary.GetAsset<VideoClip>("Assets/Credits.mp4");
            }
        }

        private static void StartVideo(CustomCreditsType type)
        {
            GameObject.Find("AudioSource").SetActive(false);
            GameObject.Find("AudioSource_Kazoo").SetActive(false);
            GameObject.Find("Background").SetActive(false);
            var camera = Locator.GetActiveCamera();
            var videoPlayer = camera.gameObject.AddComponent<VideoPlayer>();
            videoPlayer.clip = GetVideoFromType(type);
            videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            videoPlayer.playOnAwake = false;
            videoPlayer.Play();
            videoPlayer.loopPointReached += OnVideoEnd;
            var currentProfile = StandaloneProfileManager.SharedInstance.currentProfile;
            if(currentProfile != null)
            {
                var volume = currentProfile.settingsSave.masterVolume * currentProfile.settingsSave.musicVolume;
                videoPlayer.SetDirectAudioVolume(0, volume);
            }
            VideoPlayer = videoPlayer;
        }

        private static void OnVideoEnd(VideoPlayer vp) => Credits?.LoadNextScene();
        

    }
}
