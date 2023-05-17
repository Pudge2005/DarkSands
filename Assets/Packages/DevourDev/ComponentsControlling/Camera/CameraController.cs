using UnityEngine;

namespace DevourDev.Unity.Utils
{
    [DisallowMultipleComponent]
    public sealed class CameraController : ComponentController<Camera>
    {
        private AutoLerper<float> _sizingModule;


        protected override void Awake()
        {
            base.Awake();
            _sizingModule = new(this, Mathf.Lerp, SetSize);
        }

        private void SetSize(float size)
        {
            Component.orthographicSize = size;
        }


        public void TranslateToSizeInSeconds(float to, float time)
        {
            _sizingModule.StartLerping(Component.orthographicSize, to, time);
        }

        public void TranslateToSizeWithSpeed(float to, float speed)
        {
            TranslateToSizeInSeconds(to, SizingTimeFromSpeed(to, speed));
        }

        public float SizingTimeFromSpeed(float to, float speed)
        {
            return System.Math.Abs(to - Component.orthographicSize) / speed;
        }
    }
}
