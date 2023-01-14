using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SbekuMod.utils
{
    public class AssetLibrary
    {
        private static AssetBundle ReelAssetBundle;

        public static T GetAsset<T>(string path) where T : UnityEngine.Object {
            if(ReelAssetBundle == null) 
                ReelAssetBundle = SbekuMod.Instance.ModHelper.Assets.LoadBundle("assets/AssetBundles/assets");

            return ReelAssetBundle.LoadAsset<T>(path);
        }

    }
}
