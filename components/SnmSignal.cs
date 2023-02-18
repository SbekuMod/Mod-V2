using SbekuMod.utils;
using UnityEngine;
using static SbekuMod.patches.AudioSignalPatch;

namespace SbekuMod.components
{
    public class SnmSignal: MonoBehaviour
    {
        private Sector _sector;
        private CustomAudioType _audioType;
        private CustomSignalName _signalName;
        private CustomAudioSignal _signal;
        private CustomSignalFrequency _frequency;
        private bool _isFirst;
        private bool _isLast;
        private ReelSignalManager _reelSignalManager;

        public void Initialize(CustomAudioType signalAudio, CustomSignalName signalName, string sector = null, bool isFirst = false, bool isLast = false, CustomSignalFrequency frequency = CustomSignalFrequency.CUSTOM_REELS)
        {
            _isFirst = isFirst;
            _isLast = isLast;
            _audioType = signalAudio;
            _signalName = signalName;
            _frequency = frequency;
            if (sector != null)
                _sector = GameObject.Find(sector).GetComponent<Sector>();
        }

        private void Start()
        {

            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.dopplerLevel = 0;
            audioSource.loop = true;
            audioSource.maxDistance = 15f;
            audioSource.minDistance = 3f;
            audioSource.volume = 0.50f;
            audioSource.spatialBlend = 1;
            audioSource.rolloffMode = AudioRolloffMode.Custom;
            audioSource.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
            audioSource.playOnAwake = false;

            var owAudioSource = gameObject.AddComponent<CustomOWAudioSource>();
            owAudioSource.Setup(_audioType, OWAudioSource.ClipSelectionOnPlay.RANDOM, OWAudioMixer.TrackName.Signal);
            var audioSignal = gameObject.AddComponent<CustomAudioSignal>();
            audioSignal._onlyAudibleToScope = true;

            if (_sector != null)
                audioSignal._sector = _sector;

            audioSignal.Setup((SignalFrequency) _frequency, (SignalName)_signalName, 0.25f);
            _signal = audioSignal;

            var storedEvents = SbekuMod.Instance.EventStorage.Get();
            SbekuMod.Instance.ModHelper.Console.WriteLine($"SIGNAL CHECKING EVENTS {storedEvents}");

            if (!storedEvents.HasSeenBeginning && !_isFirst && !_isLast)
            {
                owAudioSource.Stop();
                _signal._active = false;
                GlobalMessenger.AddListener("OnFirstSignalTrigger", OnSignalTrigger);
            }

            if (!storedEvents.HasUnlockedEnding && _isLast)
            {
                owAudioSource.Stop();
                _signal._active = false;
                GlobalMessenger.AddListener("OnLastSignalTrigger", OnSignalTrigger);
            }

            if (_frequency == CustomSignalFrequency.CUSTOM_REELS) {
                _reelSignalManager = GameObject.Find(ReelSetup.MANAGER_OBJECT_NAME).GetComponent<ReelSignalManager>();
                _reelSignalManager.AddSignal(audioSignal);
            }
        }

        private void OnSignalTrigger() { 
            _signal._active = true;
            _signal.GetOWAudioSource().Play();
        }

        private void OnDestroy()
        {
            //_reelSignalManager.RemoveSignal(_signal);
            if (_signal != null) _signal.OnDestroy();
            GlobalMessenger.RemoveListener("OnFirstSignalTrigger", OnSignalTrigger);
            GlobalMessenger.RemoveListener("OnLastSignalTrigger", OnSignalTrigger);
        }

    }
}
