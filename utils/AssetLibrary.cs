using UnityEngine;

namespace SbekuMod.utils
{
    public class AssetLibrary
    {
        private static AssetBundle AssetBundle;
        //private static AssetBundle EndingAssetBundle;

        private static void LoadAssetBundle()
        {
            if (AssetBundle == null)
                AssetBundle = SbekuMod.Instance.ModHelper.Assets.LoadBundle("assets/AssetBundles/assets");

            //if (EndingAssetBundle == null)
            //    EndingAssetBundle = SbekuMod.Instance.ModHelper.Assets.LoadBundle("assets/AssetBundles/ending");
        }

        public static T GetAsset<T>(string path) where T : UnityEngine.Object {
            LoadAssetBundle();

            //if (path.Contains("Assets\\Slides\\Ending")) return EndingAssetBundle.LoadAsset<T>(path);

            return AssetBundle.LoadAsset<T>(path);
        }


    }
}
