using UnityEngine;

namespace SbekuMod.components
{
    public class CustomAudioSignal: AudioSignal
    {

        public new void Awake()
        {

        }

        public void Setup(SignalFrequency frequency, SignalName name, float radius)
        {
            _frequency = frequency;
            _name = name;
            _sourceRadius = radius;
            _preventIdentification = false;

			base.Awake();

			if (this._sector == null)
			{
				Debug.LogWarning("AudioSignal sectors should not be null!!!", this);
			}
			this._active = this._startActive;
			this._activeVolume = (this._startActive ? 1f : 0f);
			this._owAudioSource = this.GetRequiredComponent<OWAudioSource>();
			if (this._owAudioSource.GetTrack() != OWAudioMixer.TrackName.Signal)
			{
				Debug.Log("Audio signal not set to Signal track!", base.gameObject);
				Debug.Break();
			}
			this._showCrazyFarMaskDistance = this._name == SignalName.Traveler_Nomai && this._sourceRadius > 100f;
			Locator.RegisterAudioSignal(this);
			GlobalMessenger<Signalscope>.AddListener("EquipSignalscope", new Callback<Signalscope>(this.OnEquipSignalscope));
			GlobalMessenger.AddListener("UnequipSignalscope", new Callback(this.OnUnequipSignalScope));
		}

    }
}
