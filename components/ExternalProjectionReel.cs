using SbekuMod.utils;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SbekuMod.components
{
    public class ExternalProjectionReel: MonoBehaviour
    {
        private Signalscope _equippedSignalscope = null;
        private MindSlideProjector _mindProjector;
        private LightAnimator _lightAnimator;
        private InteractReceiver _interactReceiver;
        private string _screenPrompt;
        private InputMode _inputMode;

        public void Setup(SlideData[] slides, string screenPrompt = null)
        {
            if (screenPrompt == null)
                screenPrompt = UITextLibrary.GetString(UITextType.RebindX);

            _screenPrompt = screenPrompt;
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

                _mindProjector = gameObject.AddComponent<MindSlideProjector>();
                _mindProjector._mindSlideCollection = mindCollection;
                _mindProjector._slideCollectionItem = collectionContainer;
                _mindProjector._startSlideFadeCloseTimeOffset = vanillaProjector._startSlideFadeCloseTimeOffset;
                _mindProjector._slideFadeDuration = vanillaProjector._slideFadeDuration;
                _mindProjector._openingDuration = vanillaProjector._openingDuration;
                _mindProjector._closingDuration = vanillaProjector._closingDuration;
                _mindProjector._closingCurve = vanillaProjector._closingCurve;
                _mindProjector._openingCurve = vanillaProjector._openingCurve;

                //audioSignal = GetComponent<AudioSignal>();

                Light light = GetComponentInChildren<Light>(false);

                if (light != null)
                    _lightAnimator = light.gameObject.AddComponent<LightAnimator>();

                if (SbekuMod.Instance.EventStorage.Get().HasUnlockedEnding)
                {
                    InitializeInteraction();
                }
                else
                {
                    GlobalMessenger.AddListener(ReelManager.ON_ENDING_UNLOCKED_EVENT_NAME, OnEndingUnlocked);
                    _lightAnimator?.SetBehaviour(LightAnimator.LightBehaviour.OFF, 1);
                }

                _mindProjector.OnProjectionStart += () =>
                {
                    _inputMode = OWInput.SharedInputManager.GetInputMode();
                    OWInput.SharedInputManager.ChangeInputMode(InputMode.None);
                };


                _mindProjector.OnProjectionComplete += () =>
                {
                    OWInput.SharedInputManager.ChangeInputMode(_inputMode);
                    _lightAnimator?.SetBehaviour(LightAnimator.LightBehaviour.MinIntensity, 1);

                    if(!SbekuMod.Instance.EventStorage.Get().HasSeenEnding)
                    {
                        string text1 = UITextLibrary.GetString((UITextType)CustomTextType.CREDITS_AVAILABLE);
                        NotificationData notificationData1 = new(NotificationTarget.All, text1, 5f, true);
                        NotificationManager.SharedInstance.PostNotification(notificationData1, false);

                        string text = UITextLibrary.GetString((UITextType)CustomTextType.THANKS_FOR_PLAYING);
                        NotificationData notificationData = new(NotificationTarget.All, text, 5f, true);
                        NotificationManager.SharedInstance.PostNotification(notificationData, false);
                    }
                    
                    SbekuMod.Instance.EventStorage.Get().HasSeenEnding = true;
                    SbekuMod.Instance.EventStorage.Save();
                    SbekuMod.Instance.ModHelper.Console.WriteLine($"Ending Seen");
                };

                _mindProjector.OnProjectionStop += () =>
                {
                    OWInput.SharedInputManager.ChangeInputMode(_inputMode);
                    _lightAnimator?.SetBehaviour(LightAnimator.LightBehaviour.MinIntensity, 1);
                };

            }
            catch(Exception e)
            {
                SbekuMod.Instance.ModHelper.Console.WriteLine("REEL PROJECTION SETUP ERROR: " + e.Message, OWML.Common.MessageType.Error);
            }

        }

        private void InitializeInteraction()
        {
            _lightAnimator?.SetBehaviour(LightAnimator.LightBehaviour.Flickering, .5f);

            if (_interactReceiver != null) return;
            _interactReceiver = gameObject.AddComponent<InteractReceiver>();
            _interactReceiver._screenPrompt = new ScreenPrompt(InputLibrary.interact, $"<CMD>{_screenPrompt}", 0, ScreenPrompt.DisplayState.Normal, false);
            _interactReceiver._noCommandIconPrompt = new ScreenPrompt("", 0);
            _interactReceiver.OnPressInteract += () =>
            {
                if (_equippedSignalscope != null) _equippedSignalscope.UnequipTool();
                if (_lightAnimator != null) _lightAnimator.SetBehaviour(LightAnimator.LightBehaviour.MaxIntensity, 1);
                _mindProjector.Play(true);
            };
        }

        private void Start()
        {
            GlobalMessenger<Signalscope>.AddListener("EquipSignalscope", new Callback<Signalscope>(OnEquipSignalscope));
            GlobalMessenger.AddListener("UnequipSignalscope", new Callback(OnUnequipSignalscope));
        }

        private void OnDestroy()
        {
            GlobalMessenger<Signalscope>.RemoveListener("EquipSignalscope", new Callback<Signalscope>(OnEquipSignalscope));
            GlobalMessenger.RemoveListener("UnequipSignalscope", new Callback(OnUnequipSignalscope));
            GlobalMessenger.RemoveListener(ReelManager.ON_ENDING_UNLOCKED_EVENT_NAME, OnEndingUnlocked);
        }

        private void OnEquipSignalscope(Signalscope signalscope)
        {
            _equippedSignalscope = signalscope;
        }

        private void OnUnequipSignalscope()
        {
            _equippedSignalscope = null;
        }

        private void OnEndingUnlocked()
        {
            InitializeInteraction();
        }

    }
}
