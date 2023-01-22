using SbekuMod.components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SbekuMod.utils
{
    public class EasterEggUtility
    {

        public static void InitializeParadoxEasterEgg()
        {
            var hornfels = GameObject.Find("ConversationZone_Hornfels");
            if (hornfels == null) return;

            hornfels.AddComponent<ParadoxEastereggController>();
        }

    }
}
