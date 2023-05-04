using System;
using UnityEngine;

namespace DevourDev.Unity.TwoDThreeD
{

    [DefaultExecutionOrder(TwoDThreeDManager.ExecutionOrder + 1)]
    public sealed class TwoDThreeDComponent : MonoBehaviour
    {
        [System.Serializable]
        internal readonly struct BillBoardSettings
        {
            private readonly bool _lockXRot;
            private readonly bool _lockYRot;
            private readonly bool _lockZRot;


            public BillBoardSettings(bool lockXRot, bool lockYRot, bool lockZRot)
            {
                _lockXRot = lockXRot;
                _lockYRot = lockYRot;
                _lockZRot = lockZRot;
            }


            public bool LockXRot => _lockXRot;
            public bool LockYRot => _lockYRot;
            public bool LockZRot => _lockZRot;


            public Quaternion MergeRotations(Quaternion origRot, Quaternion targetRot)
            {
                return Quaternion.Euler(MergeRotations(origRot.eulerAngles, targetRot.eulerAngles));
            }

            public Vector3 MergeRotations(Vector3 origEulers, Vector3 targetEulers)
            {
                if (!_lockXRot)
                    origEulers.x = targetEulers.x;

                if (!_lockYRot)
                    origEulers.y = targetEulers.y;

                if (!_lockZRot)
                    origEulers.z = targetEulers.z;

                return origEulers;
            }
        }


        [System.Serializable]
        internal sealed class RelativeView
        {
            [SerializeField] GameObject _visual;
            [SerializeField] Transform _lookFromDirection;


            public RelativeView(GameObject visual, Transform lookFromDirection)
            {
                _visual = visual;
                _lookFromDirection = lookFromDirection;
            }


            public GameObject Visual => _visual;
            public Transform Direction => _lookFromDirection;
        }


        [SerializeField] private BillBoardSettings _billBoardSettings;
        [SerializeField] private RelativeView[] _viewsSettings;

        private GameObject _activeViewGo;
        private Transform _activeViewTr;

        private Quaternion _camRot;
        private bool _isDirty;

        internal int _index = -1;


        internal void InitInternal(BillBoardSettings billBoardSettings, RelativeView[] viewsSettings)
        {
            _billBoardSettings = billBoardSettings;
            _viewsSettings = viewsSettings;

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
            AdjustBillboard();

            if (_isDirty)
                UpdateRelativeView();

            _isDirty = true;
        }

        private void OnDestroy()
        {
            if (TwoDThreeDManager.Instance != null)
                TwoDThreeDManager.Instance.Unregister(this);
        }

        internal void OnCameraRotChanged(Quaternion rot)
        {
            _camRot = rot;
            UpdateRelativeView();
            _isDirty = false;
        }


        private void PrewarmViews()
        {
            if (_viewsSettings == null || _viewsSettings.Length == 0)
                throw new NullReferenceException($"{name} contains 0 view settings");


            for (int i = 0; i < _viewsSettings.Length; i++)
            {
                _viewsSettings[i].Visual.SetActive(false);
            }

            SetActiveView(_viewsSettings[0].Visual);
        }

        private void SetActiveView(GameObject go)
        {
            if (_activeViewGo != null)
                _activeViewGo.SetActive(false);

            if(go == null)
            {
                _activeViewGo = null;
                _activeViewTr = null;
                return;
            }

            _activeViewGo = go;
            _activeViewTr = go.transform;
            go.SetActive(true);
        }

        private void HandleSingletonInited(TwoDThreeDManager instance)
        {
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
            RelativeView closestView = null;
            var camRot = _camRot;

            for (int i = 0; i < len; i++)
            {
                RelativeView sv = span[i];
                float angle = Quaternion.Angle(camRot, sv.Direction.rotation);

                //if (angle < minAngle)
                //{
                //    minAngle = angle;
                //    closestView = sv;
                //}

                if(angle > maxAngle)
                {
                    maxAngle = angle;
                    closestView = sv;
                }
            }

            if (closestView.Visual.GetHashCode() == _activeViewGo.GetHashCode())
                return;

            SetActiveView(closestView.Visual);
        }


        private void AdjustBillboard()
        {
            var tr = _activeViewTr;
            tr.rotation = _billBoardSettings.MergeRotations(tr.rotation, _camRot);
        }
    }
}
