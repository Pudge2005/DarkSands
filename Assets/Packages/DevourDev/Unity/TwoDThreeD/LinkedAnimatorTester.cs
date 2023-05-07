using UnityEngine;

namespace DevourDev.Unity.TwoDThreeD
{
    public sealed class LinkedAnimatorTester : MonoBehaviour
    {
        [System.Serializable]
        private sealed class ViewLauncher
        {
            [SerializeField] private TwoDThreeDView _view;
            [SerializeField] private bool _switch;


            public void SetActiveState(bool activeState)
            {
                _view.gameObject.SetActive(activeState);
            }

            public void Switch(LinkedAnimator linkedAnimator)
            {
                _switch = false;
                linkedAnimator.SwitchView(_view);
            }

            public bool TrySwitch(LinkedAnimator linkedAnimator)
            {
                if (!_switch)
                    return false;

                Switch(linkedAnimator);
                return true;
            }
        }

        [System.Serializable]
        private sealed class AnimLauncher
        {
            [SerializeField] private string _paramName;
            [SerializeField] private AnimatorControllerParameterType _paramType;
            [SerializeField] private int _intVal;
            [SerializeField] private float _floatVal;
            [SerializeField] private bool _boolVal;
            [SerializeField] private bool _launch;

            private int _hash;


            public void Init()
            {
                _hash = Animator.StringToHash(_paramName);
            }

            public bool TryLaunch(IAnimator animator)
            {
                if (!_launch)
                    return false;

                _launch = false;

                switch (_paramType)
                {
                    case AnimatorControllerParameterType.Float:
                        break;
                    case AnimatorControllerParameterType.Int:
                        break;
                    case AnimatorControllerParameterType.Bool:
                        animator.SetBool(_hash, _boolVal);
                        break;
                    case AnimatorControllerParameterType.Trigger:
                        animator.SetTrigger(_hash);
                        break;
                    default:
                        break;
                }

                return true;
            }
        }


        [SerializeField] private LinkedAnimator _linkedAnimator;
        [SerializeField] ViewLauncher[] _viewLaunchers;
        [SerializeField] private AnimLauncher[] _animLaunchers;
        [SerializeField] private bool _prewarm = false;


        private void Awake()
        {
            foreach (var al in _animLaunchers)
            {
                al.Init();
            }

            if (_prewarm)
            {
                foreach (var vl in _viewLaunchers)
                {
                    vl.SetActiveState(false);
                }
            }
        }

        private void Start()
        {
            if (_prewarm)
            {
                _viewLaunchers[0].Switch(_linkedAnimator);
            }
        }


        private void Update()
        {
            foreach (var vl in _viewLaunchers)
            {
                if (vl.TrySwitch(_linkedAnimator))
                    break;
            }

            foreach (var al in _animLaunchers)
            {
                al.TryLaunch(_linkedAnimator);
            }
        }
    }
}
