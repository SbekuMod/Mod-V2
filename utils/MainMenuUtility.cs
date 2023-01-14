using UnityEngine;
using UnityEngine.UI;

namespace SbekuMod.utils
{
    public class MainMenuUtility
    {

        public static void ReplaceDLCLogo()
        {
            var objects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            GameObject logoObject = null;

            foreach(GameObject obj in objects)
            {
                if (obj.name.Equals("Logo_EchoesOfTheEye")) logoObject = obj;
            }

            if (logoObject == null) return;

            var sprite = AssetLibrary.GetAsset<Sprite>("Assets/MainMenu/MENU_EchoesOfTheEyeDesert.png");
            if (sprite == null) return;  

            var image = logoObject.GetComponent<Image>();
            image.sprite = sprite;
        }

    }
}
