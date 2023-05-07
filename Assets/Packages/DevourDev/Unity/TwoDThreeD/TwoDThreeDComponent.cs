using System;
using System.ComponentModel;
using UnityEngine;

namespace DevourDev.Unity.TwoDThreeD
{
    public interface IAnimator
    {
        void SetFloat(int hash, float value);
        void SetInt(int hash, int value);
        void SetBool(int hash, bool value);
        void SetTrigger(int hash);

        float GetFloat(int hash);
        int GetInt(int hash);
        bool GetBool(int hash);
    }

    public static class AnimationHelpers
    {
        public static void SyncAnimators(Animator from, Animator to)
        {
            //Animation animation = null;
            //animation.
            int paramsCount = from.parameterCount;

            for (int i = 0; i < paramsCount; i++)
            {
                var animParam = from.GetParameter(i);
                SyncAnimParam(ref from, ref to, ref animParam);
            }

            var state = from.GetCurrentAnimatorStateInfo(0);
            to.Play(state.fullPathHash, 0, state.normalizedTime);
        }

        private static void SyncAnimParam(ref Animator from, ref Animator to,
                                   ref AnimatorControllerParameter animParam)
        {
            var hash = animParam.nameHash;
            switch (animParam.type)
            {
                case AnimatorControllerParameterType.Float:
                    to.SetFloat(hash, from.GetFloat(hash));
                    break;
                case AnimatorControllerParameterType.Int:
                    to.SetInteger(hash, from.GetInteger(hash));
                    break;
                case AnimatorControllerParameterType.Bool:
                    to.SetBool(hash, from.GetBool(hash));
                    break;
                case AnimatorControllerParameterType.Trigger:
                    //to.SetTrigger(hash);
                    break;
                default:
                    break;
            }
        }
    }

    [DefaultExecutionOrder(TwoDThreeDManager.ExecutionOrder + 1)]
    public sealed class TwoDThreeDComponent : MonoBehaviour
    {
        [System.Serializable]
        internal struct BillBoardSettings
        {
            public bool LockXRot;
            public bool LockYRot;
            public bool LockZRot;


            public BillBoardSettings(bool lockXRot, bool lockYRot, bool lockZRot)
            {
                LockXRot = lockXRot;
                LockYRot = lockYRot;
                LockZRot = lockZRot;
            }


            public Quaternion MergeRotations(Quaternion origRot, Quaternion targetRot)
            {
                return Quaternion.Euler(MergeRotations(origRot.eulerAngles, targetRot.eulerAngles));
            }

            public Vector3 MergeRotations(Vector3 origEulers, Vector3 targetEulers)
            {
                if (!LockXRot)
                    origEulers.x = targetEulers.x;

                if (!LockYRot)
                    origEulers.y = targetEulers.y;

                if (!LockZRot)
                    origEulers.z = targetEulers.z;

                return origEulers;
            }
        }


        [SerializeField] private LinkedAnimator _linkedAnimator;
        [SerializeField] private BillBoardSettings _billBoardSettings;
        [SerializeField] private TwoDThreeDView[] _viewsSettings;
        [SerializeField] private Transform _rootTr;


        private Quaternion _camRot;
        private Quaternion _prevRootRot;
        private bool _isDirty;

        internal int _index = -1;


        private TwoDThreeDView ActiveView => _linkedAnimator.ActiveView;


        internal void InitInternal(BillBoardSettings billBoardSettings, TwoDThreeDView[] viewsSettings, Transform rootTransform)
        {
            _billBoardSettings = billBoardSettings;
            _viewsSettings = viewsSettings;
            _rootTr = rootTransform;

            if (UnityEngine.Application.isPlaying)
            {
                PrewarmViews();
            }

            // SetDirty should be called from Initializer.
        }

        private void Awake()
        {
            PrewarmViews();
        }

        private void Start()
        {
            if (TwoDThreeDManager.Instance == null)
            {
                TwoDThreeDManager.SingletonInited += HandleSingletonInited;
            }
            else
            {
                HandleSingletonInited(TwoDThreeDManager.Instance);
            }
        }


        private void Update()
        {
            if (_isDirty)
            {
                if (ShouldAdjust())
                {
                    AdjustAll();
                }
            }

            _isDirty = true;
        }

        private bool ShouldAdjust()
        {
            var rootRot = _rootTr.localRotation;

            if (rootRot == _prevRootRot)
                return false;

            _prevRootRot = rootRot;
            return true;
        }

        private void OnDestroy()
        {
            if (TwoDThreeDManager.Instance != null)
                TwoDThreeDManager.Instance.Unregister(this);
        }

        internal void OnCameraRotChanged(Quaternion rot)
        {
            _camRot = rot;
            AdjustAll();
        }

        private void AdjustAll()
        {
            UpdateRelativeView();
            AdjustBillboard();
            _isDirty = false;
        }

        private void PrewarmViews()
        {
            if (_viewsSettings == null || _viewsSettings.Length == 0)
                throw new NullReferenceException($"{name} contains 0 view settings");


            for (int i = 0; i < _viewsSettings.Length; i++)
            {
                _viewsSettings[i].gameObject.SetActive(false);
            }

            if (_viewsSettings.Length > 1)
                UpdateRelativeView();
            else
                SetActiveView(_viewsSettings[0]);

            _isDirty = false;
        }

        private void SetActiveView(TwoDThreeDView view)
        {
            _linkedAnimator.SwitchView(view);
            //if (_activeViewGo != null)
            //{
            //    _activeViewGo.SetActive(false);
            //    view.Animator.Play(10, 10, 0.5f); // <- это мы используем
            //}

            //if (view == null)
            //{
            //    _activeViewGo = null;
            //    _activeViewTr = null;
            //    _activeView = null;
            //    return;
            //}

            //_activeViewGo = view.gameObject;
            //_activeViewTr = _activeViewGo.transform;
            //_activeViewGo.SetActive(true);
        }

        private void HandleSingletonInited(TwoDThreeDManager instance)
        {
            TwoDThreeDManager.SingletonInited -= HandleSingletonInited;
            instance.Register(this);
        }

        private void UpdateRelativeView()
        {
            var span = _viewsSettings.AsSpan();
            var len = span.Length;

            // If we have 1 view - there is no reason
            // to calculate better view, cuz it always
            // be this one. In case of 0 views - we
            // simply can do nothing.

            if (len < 2)
                return;

            //float minAngle = float.PositiveInfinity;
            float maxAngle = float.NegativeInfinity;
            TwoDThreeDView closestView = null;
            var camRot = _camRot;

            for (int i = 0; i < len; i++)
            {
                TwoDThreeDView sv = span[i];
                float angle = Quaternion.Angle(camRot, sv.LookFromDirection.rotation);

                //if (angle < minAngle)
                //{
                //    minAngle = angle;
                //    closestView = sv;
                //}

                if (angle > maxAngle)
                {
                    maxAngle = angle;
                    closestView = sv;
                }
            }

            var activeView = ActiveView;

            if (activeView != null && closestView.GetHashCode() == activeView.GetHashCode())
                return;

            SetActiveView(closestView);
        }


        private void AdjustBillboard()
        {
            var tr = ActiveView.Transform;
            tr.rotation = _billBoardSettings.MergeRotations(tr.rotation, _camRot);
        }
    }
}
