using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SbekuMod.utils
{
    public class VersionText
    {
        public class Manifest
        {
            public string name;
            public string version;
        }
        
        public static void Setup()
        {
            var manifest = SbekuMod.Instance.ModHelper.Storage.Load<Manifest>("manifest.json");
            if (manifest == null) return;

            var scene = GameObject.Find("Scene");
            if (scene == null) return;

            var titleScreenAnimation = scene.GetComponent<TitleScreenAnimation>();
            titleScreenAnimation.OnLogoPanComplete += () =>
            {
                var versionText = GameObject.Find("VersionText");
                if (versionText == null) return;

                var text = versionText.GetComponent<Text>();
                text.text = $"{text.text} | {manifest.name} - v{manifest.version}-PreAlpha";

            };

        }

    }
}
