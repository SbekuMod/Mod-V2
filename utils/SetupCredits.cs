using SbekuMod.components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

namespace SbekuMod.utils
{
    public class SetupCredits
    {

        public static CreditsController Setup()
        {
            var camera = GameObject.Find("Camera");
            if (camera == null) return null;

            var creditsClip = AssetLibrary.GetAsset<VideoClip>("Assets/Credits.mp4");
            if (creditsClip == null) return null;

            var controller = camera.AddComponent<CreditsController>();
            controller.Initialize(creditsClip);

            return controller;
        }

    }
}
