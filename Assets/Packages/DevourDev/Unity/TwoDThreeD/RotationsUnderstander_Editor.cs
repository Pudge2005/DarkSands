using UnityEngine;

namespace DevourDev.Unity.TwoDThreeD
{
    [ExecuteInEditMode]
    internal sealed class RotationsUnderstander_Editor : MonoBehaviour
    {
#if UNITY_EDITOR
        [System.Serializable]
        public sealed class TransformToInspect
        {
            [SerializeField] private Transform _transform;
            [SerializeField] private Quaternion _rotQuat;
            [SerializeField] private Vector3 _rotEul;
            [SerializeField] private Vector3 _rotVec;


            internal void Update()
            {
                if (_transform == null)
                    return;

                _rotQuat = _transform.rotation;
                _rotEul = _rotQuat.eulerAngles;
                _rotVec = _transform.forward;
            }


            public float AngleWith(TransformToInspect other)
            {
                if (_transform == null || other._transform == null)
                    return float.NaN;

                return Quaternion.Angle(_rotQuat, other._rotQuat);
            }
        }


        [SerializeField] private TransformToInspect[] _transformsToInspect;

        [SerializeField] private TransformToInspect _a;
        [SerializeField] private TransformToInspect _b;
        [SerializeField] private float _abAngle;
        [SerializeField] private bool _swapAB;


        private void Update()
        {
            if (_transformsToInspect == null)
                return;

            foreach (var tti in _transformsToInspect)
            {
                tti.Update();
            }

            if (_a == null || _b == null)
                _abAngle = float.NaN;

            _a.Update();
            _b.Update();

            if (_swapAB)
            {
                _swapAB = false;
                (_a, _b) = (_b, _a);
            }

            _abAngle = _a.AngleWith(_b);
        }
#endif
    }
}
