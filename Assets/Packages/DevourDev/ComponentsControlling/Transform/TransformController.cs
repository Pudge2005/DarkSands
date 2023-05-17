using UnityEngine;

namespace DevourDev.Unity.Utils
{
    [DisallowMultipleComponent]
    public sealed class TransformController : ComponentController<Transform>
    {
        private AutoLerper<Vector3> _posingModule;


        protected override void Awake()
        {
            base.Awake();
            _posingModule = new(this, Vector3.Lerp, SetPosition);
        }

        private void SetPosition(Vector3 pos)
        {
            Component.position = pos;
        }


        public void TranslateToPositionInSeconds(Vector3 to, float time)
        {
            _posingModule.StartLerping(Component.position, to, time);
        }

        public void TranslateToPositionWithSpeed(Vector3 to, float speed)
        {
            TranslateToPositionInSeconds(to, PosingTimeFromSpeed(to, speed));
        }

        public float PosingTimeFromSpeed(Vector3 to, float speed)
        {
            return (to - transform.position).magnitude / speed;
        }
    }
}
