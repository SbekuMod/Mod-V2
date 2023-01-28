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
        private List<CustomAudioSignal> _signalList = new();

        private void Start()
        {
            _signalList = new List<CustomAudioSignal>();
            GlobalMessenger<Signalscope>.AddListener("EquipSignalscope", new Callback<Signalscope>(OnEquipSignalscope));
        }

        private void OnDestroy()
        {
            GlobalMessenger<Signalscope>.RemoveListener("EquipSignalscope", new Callback<Signalscope>(OnEquipSignalscope));
        }

        private void OnEquipSignalscope(Signalscope signalscope)
        {
            CustomAudioSignal comparedSignal = null;
            foreach(var signal in _signalList)
            {
                if (comparedSignal == null)
                    comparedSignal = signal;
                else
                    signal.GetOWAudioSource().timeSamples = comparedSignal.GetOWAudioSource().timeSamples;
            }
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
