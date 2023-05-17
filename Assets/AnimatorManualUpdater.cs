using UnityEngine;

namespace Miscs
{
    public sealed class AnimatorManualUpdater : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _updateRate = 1f;

        private float _timeToUpdateLeft;


        private void Awake()
        {
            _animator.enabled = false;    
        }

        private void Update()
        {
            if ((_timeToUpdateLeft -= Time.deltaTime) > 0)
                return;

            _timeToUpdateLeft = 1f / _updateRate;
            _animator.Update(_timeToUpdateLeft);
        }
    }
}
