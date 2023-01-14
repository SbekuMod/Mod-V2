using SbekuMod.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SbekuMod.components
{
    public class CustomOWAudioSource: OWAudioSource
    {

        public new void Awake() {}

        public void Setup(CustomAudioType audioType, ClipSelectionOnPlay clipSelectionOnPlay, OWAudioMixer.TrackName track)
        {
            _maxSourceVolume = 0.45f;
            _audioLibraryClip = (AudioType)audioType;
            _clipSelectionOnPlay = clipSelectionOnPlay;
            _track = track;

            this._audioSource = this.GetRequiredComponent<AudioSource>();
            this._audioSource.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
            if (this._track == OWAudioMixer.TrackName.Undefined || this._track == OWAudioMixer.TrackName.Undefined)
            {
                Debug.LogError("Audio source track is Undefined, moving to Menu track", this);
                this._track = OWAudioMixer.TrackName.Menu;
            }
            if (this._audioSource.volume > 0f)
            {
                this._maxSourceVolume = this._audioSource.volume;
            }
        }

    }
}
