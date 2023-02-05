using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

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

        private void Update()
        {
            if(SbekuMod.Instance.ModHelper.Config.GetSettingsValue<bool>("Premi I per sbloccare tutti i rulli") && Keyboard.current.iKey.isPressed)
            {
                foreach (var availableReel in _availableReels)
                    OnSeeReel(availableReel.gameObject.name);
            }
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
                string text = $"NUOVO <color=#a82debff>SEGNALE ANOMALO</color> IDENTIFICATO";
                NotificationData notificationData = new(NotificationTarget.All, text, 5f, true);
                NotificationManager.SharedInstance.PostNotification(notificationData, false);
            }
            else
            {
                string text = $"{_seenReels.Count}/{_availableReels.Count} <color=#a82debff>RULLI DI DIAPOSITIVE</color> TROVATI";
                NotificationData notificationData = new(NotificationTarget.All, text, 5f, true);
                NotificationManager.SharedInstance.PostNotification(notificationData, false);
            }
        }

    }
}
