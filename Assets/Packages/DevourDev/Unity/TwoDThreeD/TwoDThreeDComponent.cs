using System;
using UnityEngine;

namespace DevourDev.Unity.TwoDThreeD
{
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

        internal int _index = -1;

        private Quaternion _camRot;
        private Quaternion _prevRootRot;
        private bool _isDirty;


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
                _viewsSettings[i].GameObject.SetActive(false);
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
            // Check if current view is not actual anymore.

            var camRot = _camRot;
            var activeView = ActiveView;

            if(activeView != null)
            {
                var curViewAngle = Quaternion.Angle(camRot, activeView.LookFromDirection.rotation);

                if (curViewAngle < activeView.MaxAngleToKeepView)
                    return;
            }

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
