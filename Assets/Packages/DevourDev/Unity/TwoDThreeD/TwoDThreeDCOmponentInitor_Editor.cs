using System;
using System.Collections.Generic;
using Microsoft.Cci;
using UnityEngine;
using UnityEngine.Pool;

namespace DevourDev.Unity.TwoDThreeD
{
    internal sealed class TwoDThreeDCOmponentInitor_Editor : MonoBehaviour
    {
#if UNITY_EDITOR
        [System.Serializable]
        private struct InitorSettings
        {
            [SerializeField] private TwoDThreeDView _viewPrefab;
            [SerializeField] private Transform _viewsRoot;
            [SerializeField] private Transform _directionsRoot;

            [SerializeField] private StandardDirections[] _directions;
            [SerializeField] private bool _leftIsMirroredRight;


            public TwoDThreeDView ViewPrefab => _viewPrefab;
            public Transform ViewsRoot => _viewsRoot;
            public Transform DirectionsRoot => _directionsRoot;

            public IReadOnlyList<StandardDirections> Directions => _directions;
            public bool LeftIsMirroredRight => _leftIsMirroredRight;



            public StandardDirections GetDirectionsFlags()
            {
                var flags = StandardDirections.None;

                foreach (var dir in _directions)
                {
                    flags |= dir;
                }

                return flags;
            }

            internal void EnsureDirectionsUniqueness()
            {
                var hs = HashSetPool<StandardDirections>.Get();

                var arr = _directions;
                var len = arr.Length;

                for (int i = 0; i < len; i++)
                {
                    if (!hs.Add(arr[i]))
                    {
                        Debug.LogError($"Non-unique direction detected. Index: {i}, direction: {arr[i]}");
                        DeleteDuplicates(hs);
                        break;
                    }
                }

                HashSetPool<StandardDirections>.Release(hs);
            }

            private void DeleteDuplicates(HashSet<StandardDirections> pooledHs)
            {
                var list = ListPool<StandardDirections>.Get();

                var arr = _directions;
                var len = arr.Length;

                for (int i = 0; i < len; i++)
                {
                    var ds = arr[i];
                    if (pooledHs.Add(ds))
                    {
                        list.Add(ds);
                    }
                }

                StandardDirections[] views = list.ToArray();
                ListPool<StandardDirections>.Release(list);
                _directions = views;
            }
        }


        [Flags]
        internal enum StandardDirections
        {
            None = 0b0,
            Forward = 0b1,
            Back = 0b10,
            Left = 0b100,
            Right = 0b1000,
        }


        internal const string TwoDInThreeDName = "2D in 3D";
        internal const string ViewsRootName = "Views";
        internal const string DirectionsRootName = "Directions";

        [SerializeField] private InitorSettings _settings;
        [SerializeField] private bool _init;


        internal static string GetStandardDirectionName(StandardDirections direction)
        {
            return direction switch
            {
                StandardDirections.Forward => "Forward",
                StandardDirections.Back => "Back",
                StandardDirections.Left => "Left",
                StandardDirections.Right => "Right",
                _ => throw new System.NotSupportedException($"name for direction {direction} is not " +
                $"implemented or {nameof(direction)} argument has more than 1 direction")
            };
        }

        internal static string GetStandardFromDirectionViewName(StandardDirections direction)
        {
            var name = direction switch
            {
                StandardDirections.Forward => "From forward",
                StandardDirections.Back => "From back",
                StandardDirections.Left => "From left",
                StandardDirections.Right => "From right",
                _ => throw new System.NotSupportedException($"name for direction {direction} is not " +
                $"implemented or {nameof(direction)} argument has more than 1 direction")
            };

            return name + " View";
        }


        internal static Vector3 StandardDirectionToVector(StandardDirections direction)
        {
            return direction switch
            {
                StandardDirections.Forward => Vector3.forward,
                StandardDirections.Back => Vector3.back,
                StandardDirections.Left => Vector3.left,
                StandardDirections.Right => Vector3.right,
                _ => throw new System.NotSupportedException($"vector for direction {direction} is not " +
                $"implemented or {nameof(direction)} argument has more than 1 direction")
            };
        }


        private void OnValidate()
        {
            if (_init)
            {
                _init = false;
                Init_Editor();
                UnityEditor.EditorUtility.SetDirty(transform.root);
            }
        }

        private void Init_Editor()
        {
            var tdtd = GetTwoDThreeDComponent();
            Transform viewsRoot = GetViewsRoot(tdtd.transform);
            Transform directionsRoot = GetDirectionsRoot(tdtd.transform);

            InitDirectories(tdtd, viewsRoot, directionsRoot);
        }

        private void InitDirectories(TwoDThreeDComponent tdtd, Transform viewsRoot,
            Transform directionsRoot)
        {
            var directions = GetUniqueDirections();

            TwoDThreeDComponent.BillBoardSettings billBoardSettings = new(false, false, true);
            var views = new TwoDThreeDView[directions.Count];

            for (int i = 0; i < directions.Count; i++)
            {
                var relView = CreateView(viewsRoot, directionsRoot, directions[i]);
                views[i] = relView;
            }

            InitMaxAngleToKeepViews(views);

            foreach (var view in views)
            {
                UnityEditor.EditorUtility.SetDirty(view.gameObject);
            }

            tdtd.InitInternal(billBoardSettings, views, transform.root);

            // Probably overkill due to SetDirty(transform.root) in OnValidate().
            UnityEditor.EditorUtility.SetDirty(tdtd);
        }

        private void InitMaxAngleToKeepViews(TwoDThreeDView[] viewsSettings)
        {
            foreach (var vs in viewsSettings)
            {
                float minAngle = 180f;

                foreach (var vs2 in viewsSettings)
                {
                    if (vs == vs2)
                        continue;

                    float angle = Quaternion.Angle(vs.LookFromDirection.rotation, vs2.LookFromDirection.rotation);

                    if (angle < minAngle)
                        minAngle = angle;
                }

                vs.MaxAngleToKeepView = minAngle;
            }
        }

        private IReadOnlyList<StandardDirections> GetUniqueDirections()
        {
            _settings.EnsureDirectionsUniqueness();
            return _settings.Directions;
        }


        private TwoDThreeDView CreateView(Transform viewsRoot, Transform directionsRoot, StandardDirections dir)
        {
            string viewName = GetStandardFromDirectionViewName(dir);
            string dirName = GetStandardDirectionName(dir);

            // Sprite Renderers by default oriented to face backwards.
            // In 3D Space we need them to face forward. We can achieve
            // it by either flipping it or  rotating 180 degrees or
            // multiplying scale.x by -1. Flipping is cheap enough,
            // but manipulating scale 

            //var scale = viewTr.localScale;
            //scale.x = -scale.x;
            //viewTr.localScale = scale;
            //sr.sprite = dir.Sprite;

            var dirTr = CreateDirectionTransform(dirName, directionsRoot, StandardDirectionToVector(dir));
            var relView = GetViewInstance(viewName, viewsRoot);
            relView.LookFromDirection = dirTr;

            return relView;
        }

        private TwoDThreeDView GetViewInstance(string gameObjectName, Transform parent)
        {
            if (_settings.ViewPrefab == null)
                return CreateViewInstanceAuto(gameObjectName, parent);

            var view = Instantiate(_settings.ViewPrefab, parent);
            view.GameObject.name = gameObjectName;
            return view;
        }

        private TwoDThreeDView CreateViewInstanceAuto(string gameObjectName, Transform parent)
        {
            var viewGo = new GameObject(gameObjectName);
            var viewTr = viewGo.transform;
            viewTr.SetParent(parent, false);
            viewTr.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            var view = viewGo.AddComponent<TwoDThreeDView>();
            view.Animator = viewGo.AddComponent<Animator>();
            return view;
        }

        private Transform CreateDirectionTransform(string name, Transform root, Vector3 relatedDirection)
        {
            var tr = CreateChildTransform(name, root);
            tr.localRotation = Quaternion.LookRotation(relatedDirection);
            return tr;
        }

        private TwoDThreeDComponent GetTwoDThreeDComponent()
        {
            TwoDThreeDComponent tdtd = gameObject.GetComponentInChildren<TwoDThreeDComponent>();

            if (tdtd == null)
            {
                tdtd = GetTdtdRootTransform().gameObject.AddComponent<TwoDThreeDComponent>();
            }

            return tdtd;
        }

        private Transform GetViewsRoot(Transform tdtdRoot)
        {
            if (_settings.ViewsRoot != null)
            {
                return _settings.ViewsRoot;
            }


            return CreateChildTransform(ViewsRootName, tdtdRoot);
        }

        private Transform GetTdtdRootTransform()
        {
            Transform tdtdRoot;

            if (transform.root == transform)
                tdtdRoot = CreateChildTransform(TwoDInThreeDName);
            else
                tdtdRoot = transform;

            return tdtdRoot;
        }

        private Transform GetDirectionsRoot(Transform tdtdRoot)
        {
            if (_settings.DirectionsRoot != null)
            {
                return _settings.DirectionsRoot;
            }

            return CreateChildTransform(DirectionsRootName, tdtdRoot);
        }

        private Transform CreateChildTransform(string name)
        {
            return CreateChildTransform(name, transform);
        }

        private Transform CreateChildTransform(string name, Transform parent)
        {
            var go = new GameObject(name);
            var tr = go.transform;
            tr.parent = parent;
            tr.SetLocalPositionAndRotation(default, Quaternion.identity);
            return tr;
        }
#endif

        private void Start()
        {
            Destroy(this);
        }
    }
}
