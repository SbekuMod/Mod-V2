using UnityEngine;

namespace SbekuMod.utils
{
    public class AssetLibrary
    {
        private static AssetBundle AssetBundle;

        private static void LoadAssetBundle()
        {
            if (AssetBundle == null)
                AssetBundle = SbekuMod.Instance.ModHelper.Assets.LoadBundle("assets/AssetBundles/assets");
        }

        public static T GetAsset<T>(string path) where T : UnityEngine.Object {
            LoadAssetBundle();

            return AssetBundle.LoadAsset<T>(path);
        }


    }
}
