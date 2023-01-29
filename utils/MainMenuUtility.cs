using SbekuMod.storage;
using UnityEngine;
using UnityEngine.UI;

namespace SbekuMod.utils
{
    public class MainMenuUtility
    {
        private static GameObject GetGameObject(string name)
        {
            var objects = Resources.FindObjectsOfTypeAll(typeof(GameObject));

            foreach (GameObject obj in objects)
            {
                if (obj.name.Equals(name)) return obj;
            }

            return null;
        }

        public static void ReplaceDLCLogo()
        {
            var logoObject = GetGameObject("Logo_EchoesOfTheEye");
            if (logoObject == null) return;

            var sprite = AssetLibrary.GetAsset<Sprite>("Assets/MainMenu/MENU_EchoesOfTheEyeDesert.png");
            if (sprite == null) return;  

            var image = logoObject.GetComponent<Image>();
            image.sprite = sprite;
        }

        public static void ReplaceMusic()
        {
            //var eventStorage = SbekuMod.Instance.EventStorage;
            //var storedEvents = eventStorage.Get();
            //if (!storedEvents.HasSeenEnding) return;

            var musicObject = GetGameObject("AudioSource_Music");
            if(musicObject == null) return;

            var audioSource = musicObject.GetComponent<OWAudioSource>();
            if(audioSource == null) return;

            audioSource._audioLibraryClip = (AudioType)CustomAudioType.SNM_MAIN_MENU;

        }

    }
}
