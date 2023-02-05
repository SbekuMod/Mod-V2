using SbekuMod.utils;
using System;
using UnityEngine;
using static NomaiWarpPlatform;
using static SbekuMod.patches.AudioSignalPatch;

namespace SbekuMod.components
{
    public class ExternalSlideReel: MonoBehaviour
    {
        private SlideReelItem _slideReelItem;
        private bool _isFirst;

        public void Setup(SlideData[] slides, HideableFromEntryway hideableFromEntryway = null, bool isFirst = false)
        {
            try
            {
                var slideCollection = new SlideCollection(slides.Length)
                {
                    slides = SlideData.DataToSlideList(slides)
                };

                var collectionContainer = gameObject.AddComponent<SlideCollectionContainer>();
                collectionContainer._slideCollection = slideCollection;
                collectionContainer._playWithShipLogFacts = new string[0];

                var animator = gameObject.GetComponentInChildren<TransformAnimator>();
                var slideReelItem = gameObject.AddComponent<SlideReelItem>();
                slideReelItem._animator = animator;

                //audioSignal = GetComponent<AudioSignal>();
                //slideReelItem.onPickedUp += (item) =>
                //{
                //    if (audioSignal != null)
                //    {
                //        audioSignal._onlyAudibleToScope = true;
                //        audioSignal._owAudioSource.Stop();
                //    }
                //};

                if(hideableFromEntryway != null)
                {
                    slideReelItem.onPickedUp += (item) =>
                    {
                        if(!hideableFromEntryway.IsVisible)
                            hideableFromEntryway.Toggle(true);

                        hideableFromEntryway.enabled = false;
                    };
                }

                _slideReelItem = slideReelItem;
                _isFirst = isFirst;

                if (!_isFirst)
                {
                    ReelManager.instance?.RegisterReel(this);
                    slideReelItem.onPickedUp += (item) =>
                    {
                        GlobalMessenger<string>.FireEvent(ReelManager.ON_SEE_REEL_EVENT_NAME, gameObject.name);
                    };
                }
            }
            catch (Exception e)
            {
                SbekuMod.Instance.ModHelper.Console.WriteLine("REEL SLIDE SETUP ERROR: " + e.Message, OWML.Common.MessageType.Error);
            }
        }

        private void Start()
        {
            if (!SbekuMod.Instance.EventStorage.Get().HasSeenBeginning && _isFirst)
            {
                var container = _slideReelItem._slideCollectionContainer;
                container.onEndOfSlides += OnEndOfSlides;
            }
        }

        private void OnDestroy()
        {
            var container = _slideReelItem._slideCollectionContainer;
            container.onEndOfSlides -= OnEndOfSlides;
        }

        private void OnEndOfSlides()
        {
            SbekuMod.Instance.EventStorage.Get().HasSeenBeginning = true;
            SbekuMod.Instance.EventStorage.Save();
            GlobalMessenger.FireEvent("OnFirstSignalTrigger");
            PlayerData.LearnFrequency((SignalFrequency)CustomSignalFrequency.CUSTOM_REELS);
            string text = "NUOVI <color=#a82debff>SEGNALI ANOMALI</color> INDIVIDUATI";
            NotificationData notificationData = new(NotificationTarget.All, text, 10f, true);
            NotificationManager.SharedInstance.PostNotification(notificationData, false);

            _slideReelItem._slideCollectionContainer.onEndOfSlides -= OnEndOfSlides;
        }

    }
}
