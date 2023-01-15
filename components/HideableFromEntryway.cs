using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SbekuMod.components
{
    public class HideableFromEntryway: MonoBehaviour
    {
        private MeshRenderer[] _meshes;
        private EntrywayTrigger _trigger;

        public bool IsVisible
        {
            get {
                if(_meshes == null || _meshes.Length == 0) return false;

                return _meshes[0].enabled;
            }
        }

        public void Toggle(bool show = true)
        {
            SbekuMod.Instance.ModHelper.Console.WriteLine($"REEL ENTRYWAY TOGGLE: {show}");
            if (!enabled || _meshes == null || _trigger == null) return;

            foreach (var mesh in _meshes) { 
                mesh.enabled = show;
            }
        }

        public void OnEntry(GameObject hitObj)
        {
            Toggle(false);
        }
        public void OnExit(GameObject hitObj)
        {
            Toggle(true);
        }

        public void Setup(string EntrywayName)
        {
            _meshes = GetComponentsInChildren<MeshRenderer>();

            var entryway = GameObject.Find(EntrywayName);
            if (entryway == null) return;

            _trigger = entryway.GetComponent<EntrywayTrigger>();
            if (_trigger == null) return;

            _trigger.OnEntry += OnEntry;
            _trigger.OnExit += OnExit;
            Toggle(false);

        }

    }
}
