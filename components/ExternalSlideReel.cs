using SbekuMod.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SbekuMod.components
{
    public class ExternalSlideReel: MonoBehaviour
    {

        private AudioSignal audioSignal;

        public void Setup(SlideData[] slides)
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

                audioSignal = GetComponent<AudioSignal>();
                slideReelItem.onPickedUp += (item) =>
                {
                    if (audioSignal != null)
                    {
                        audioSignal._onlyAudibleToScope = true;
                        audioSignal._owAudioSource.Stop();
                    }
                };

            }
            catch (Exception e)
            {
                SbekuMod.Instance.ModHelper.Console.WriteLine("REEL SLIDE SETUP ERROR: " + e.Message, OWML.Common.MessageType.Error);
            }
        }

    }
}
