using SbekuMod.utils;
using System;
using UnityEngine;

namespace SbekuMod.components
{
    public class ExternalProjectionReel: MonoBehaviour
    {
        private AudioSignal audioSignal;

        public void Setup(SlideData[] slides, string screenPrompt = "Interagisci")
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

                // TODO CACHE PROJECTOR BECAUSE FIND AND GETCOMPONENT ARE SLOW
                var vanillaProjector = GameObject.Find("Prefab_IP_VisionTorchProjector").GetComponent<MindSlideProjector>();

                var mindCollection = gameObject.AddComponent<MindSlideCollection>();
                mindCollection._slideCollectionContainer = collectionContainer;

                var mindProjector = gameObject.AddComponent<MindSlideProjector>();
                mindProjector._mindSlideCollection = mindCollection;
                mindProjector._slideCollectionItem = collectionContainer;

                mindProjector._startSlideFadeCloseTimeOffset = vanillaProjector._startSlideFadeCloseTimeOffset;
                mindProjector._slideFadeDuration = vanillaProjector._slideFadeDuration;
                mindProjector._openingDuration = vanillaProjector._openingDuration;
                mindProjector._closingDuration = vanillaProjector._closingDuration;
                mindProjector._closingCurve = vanillaProjector._closingCurve;
                mindProjector._openingCurve = vanillaProjector._openingCurve;

                audioSignal = GetComponent<AudioSignal>();

                Light light = GetComponentInChildren<Light>(false);
                LightAnimator lightAnimator = null;

                if (light != null)
                    lightAnimator = light.gameObject.AddComponent<LightAnimator>();
                

                var interactReceiver = gameObject.AddComponent<InteractReceiver>();
                interactReceiver._screenPrompt = new ScreenPrompt(InputLibrary.interact, $"<CMD>{screenPrompt}", 0, ScreenPrompt.DisplayState.Normal, false);
                interactReceiver._noCommandIconPrompt = new ScreenPrompt("", 0);
                interactReceiver.OnPressInteract += () =>
                {
                    if (audioSignal != null)
                    {
                        audioSignal._onlyAudibleToScope = true;
                        audioSignal._owAudioSource.Stop();
                    }

                    if (lightAnimator != null) lightAnimator.SetBehaviour(LightAnimator.LightBehaviour.MaxIntensity, 1);
                    mindProjector.Play(true);
                };

                mindProjector.OnProjectionComplete += () =>
                {
                    if (lightAnimator != null) lightAnimator.SetBehaviour(LightAnimator.LightBehaviour.MinIntensity, 1);
                };

                mindProjector.OnProjectionStop += () =>
                {
                    if (lightAnimator != null) lightAnimator.SetBehaviour(LightAnimator.LightBehaviour.MinIntensity, 1);
                };

            }
            catch(Exception e)
            {
                SbekuMod.Instance.ModHelper.Console.WriteLine("REEL PROJECTION SETUP ERROR: " + e.Message, OWML.Common.MessageType.Error);
            }

        }

    }
}
