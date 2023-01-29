using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SbekuMod.components
{
    public class ReelSignalManager: MonoBehaviour
    {
        private bool _signalscopeEquipped = false;
        private float _timeSync = 0;
        private List<CustomAudioSignal> _signalList = new();

        private void Start()
        {
            _signalList = new List<CustomAudioSignal>();
            GlobalMessenger<Signalscope>.AddListener("EquipSignalscope", new Callback<Signalscope>(OnEquipSignalscope));
            GlobalMessenger.AddListener("UnequipSignalscope", new Callback(OnUnequipSignalscope));
        }

        private void OnDestroy()
        {
            GlobalMessenger<Signalscope>.RemoveListener("EquipSignalscope", new Callback<Signalscope>(OnEquipSignalscope)); 
            GlobalMessenger.RemoveListener("UnequipSignalscope", new Callback(OnUnequipSignalscope));
        }

        private void OnEquipSignalscope(Signalscope signalscope)
        {
            _signalscopeEquipped = true;
            _timeSync = Time.fixedTime;
        }

        private void OnUnequipSignalscope()
        {
            _signalscopeEquipped = false;
            _timeSync = Time.fixedTime;
        }

        void FixedUpdate()
        {
            if (!_signalscopeEquipped || Time.fixedTime - _timeSync < 2) return;
            CustomAudioSignal comparedSignal = null;
            foreach (var signal in _signalList)
            {
                if (comparedSignal == null)
                    comparedSignal = signal;
                else
                    signal.GetOWAudioSource().timeSamples = comparedSignal.GetOWAudioSource().timeSamples;
            }
            _timeSync = Time.fixedTime;
        }

        public void AddSignal(CustomAudioSignal signal)
        {
            if(!_signalList.Exists(s => s == signal))
                _signalList.Add(signal);
        }

        public void RemoveSignal(CustomAudioSignal signal)
        {
            if (_signalList.Exists(s => s == signal))
                _signalList.Remove(signal);
        }

    }
}
