using SbekuMod.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SbekuMod.components
{
    public class ReelManager: MonoBehaviour
    {
        public static ReelManager instance;

        public static string ON_SEE_REEL_EVENT_NAME = "OnSeeReel";
        public static string ON_ENDING_UNLOCKED_EVENT_NAME = "OnEndingUnlocked";
        private List<string> _seenReels;
        private List<ExternalSlideReel> _availableReels = new List<ExternalSlideReel>();

        private bool HasSeenAllReels
        {
            get
            {
                foreach(var availableReel in _availableReels)
                    if (!_seenReels.Contains(availableReel.gameObject.name)) return false;

                return true;
            }
        }

        private void Awake()
        {
            instance = this;
            _availableReels = new List<ExternalSlideReel>();
        }

        private void Start()
        {            
            _seenReels = new List<string>(SbekuMod.Instance.EventStorage.Get().SeenReels);
            GlobalMessenger<string>.AddListener(ON_SEE_REEL_EVENT_NAME, new Callback<string>(OnSeeReel));
        }

        private void OnDestroy()
        {
            instance = null;
            GlobalMessenger<string>.RemoveListener(ON_SEE_REEL_EVENT_NAME, new Callback<string>(OnSeeReel));
        }

        public void RegisterReel(ExternalSlideReel reel)
        {
            if (_availableReels.Contains(reel)) return;

            _availableReels.Add(reel);
            SbekuMod.Instance.ModHelper.Console.WriteLine($"{reel.gameObject.name} reel registered");
        }

        private void OnSeeReel(string name)
        {

            if (_seenReels.Contains(name)) return;

            _seenReels.Add(name);
            SbekuMod.Instance.EventStorage.Get().SeenReels = _seenReels.ToArray();
            SbekuMod.Instance.EventStorage.Save();


            SbekuMod.Instance.ModHelper.Console.WriteLine($"{name} reel added to the list");

            if(HasSeenAllReels)
            {
                SbekuMod.Instance.EventStorage.Get().HasUnlockedEnding = true;
                SbekuMod.Instance.EventStorage.Save();
                GlobalMessenger.FireEvent(ON_ENDING_UNLOCKED_EVENT_NAME);
                GlobalMessenger.FireEvent("OnLastSignalTrigger");
                SbekuMod.Instance.ModHelper.Console.WriteLine($"Ending Unlocked");
                string text = UITextLibrary.GetString((UITextType)CustomTextType.NEW_SIGNAL_FOUND);
                NotificationData notificationData = new(NotificationTarget.All, text, 5f, true);
                NotificationManager.SharedInstance.PostNotification(notificationData, false);
            }
            else
            {
                string text = $"{_seenReels.Count}/{_availableReels.Count} {UITextLibrary.GetString((UITextType)CustomTextType.NEW_SLIDE_REEL_FOUND)}";
                NotificationData notificationData = new(NotificationTarget.All, text, 5f, true);
                NotificationManager.SharedInstance.PostNotification(notificationData, false);
            }
        }

    }
}
