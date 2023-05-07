using UnityEngine;

namespace DevourDev.Unity.TwoDThreeD
{
    internal sealed class TwoDThreeDView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _lookFromDirection;


        public Animator Animator { get => _animator; internal set => _animator = value; }
        public Transform LookFromDirection { get => _lookFromDirection; internal set => _lookFromDirection = value; }

        public GameObject GameObject { get; private set; }
        public Transform Transform { get; private set; }


        private void Awake()
        {
            GameObject = gameObject;
            Transform = GameObject.transform;
        }


        public override int GetHashCode()
        {
            return GameObject.GetHashCode();
        }
    }
}
