using UnityEngine;

namespace SbekuMod.components
{
    [RequireComponent(typeof(Light))]
    public class LightAnimator : MonoBehaviour
    {
        public enum LightBehaviour
        {
            Flickering,
            MaxIntensity,
            MinIntensity
        }

        private Light _light;
        private float _velocity;
        private bool isFlickeringIncreasing = false;

        public LightBehaviour lightBehaviour;
        public float minIntensity = 1f;
        public float maxIntensity = 4f;
        public float transitionLength = .5f;

        public void Start()
        {
            _light = GetComponent<Light>();
        }

        private void FixedUpdate()
        {

            if (lightBehaviour == LightBehaviour.MinIntensity || (lightBehaviour == LightBehaviour.Flickering && !isFlickeringIncreasing))
                _light.intensity = Mathf.SmoothDamp(_light.intensity, minIntensity, ref _velocity, transitionLength, 10f, Time.fixedDeltaTime);


            if (lightBehaviour == LightBehaviour.MaxIntensity || (lightBehaviour == LightBehaviour.Flickering && isFlickeringIncreasing))
                _light.intensity = Mathf.SmoothDamp(_light.intensity, maxIntensity, ref _velocity, transitionLength, 10f, Time.fixedDeltaTime);

            if (lightBehaviour == LightBehaviour.Flickering && ((isFlickeringIncreasing && Mathf.Abs(_light.intensity - maxIntensity) < 0.05) || (!isFlickeringIncreasing && Mathf.Abs(_light.intensity - minIntensity) < 0.05)))
            {
                isFlickeringIncreasing = !isFlickeringIncreasing;
                _velocity = 0;
            }

        }

        public void SetBehaviour(LightBehaviour behaviour, float? transition)
        {
            _velocity = 0;

            lightBehaviour = behaviour;

            if (transition.HasValue)
                transitionLength = transition.Value;
        }
    }

}
