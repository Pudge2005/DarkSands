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
        internal  struct DirectionSprite
        {
            public  StandartDirections Direction;
            public  Sprite Sprite;


            public DirectionSprite(StandartDirections direction, Sprite sprite)
            {
                Direction = direction;
                Sprite = sprite;
            }
        }


        [System.Serializable]
        private struct InitorSettings
        {
            [SerializeField] private TwoDThreeDView _viewPrefab;
            [SerializeField] private Transform _viewsRoot;
            [SerializeField] private Transform _directionsRoot;

            [SerializeField] private DirectionSprite[] _views;
            [SerializeField] private bool _leftIsMirroredRight;

            [SerializeField] private Material _initialViewsMaterial;
            [SerializeField] private string _initialViewsSortingLayer;
            [SerializeField] private SerializableNullable<int> _initialViewsSortingLayerId;
            [SerializeField] private SerializableNullable<int> _initialViewsOrderInLayer;
            [SerializeField] private SerializableNullable<uint> _initialViewsRenderingLayerMask;


            public TwoDThreeDView ViewPrefab => _viewPrefab;
            public Transform ViewsRoot => _viewsRoot;
            public Transform DirectionsRoot => _directionsRoot;

            public IReadOnlyList<DirectionSprite> Views => _views;
            public bool LeftIsMirroredRight => _leftIsMirroredRight;

            public Material InitialViewsMaterial => _initialViewsMaterial;
            public string InitialViewsSortingLayerName => _initialViewsSortingLayer;
            public int? InitialViewsSortingLayerId => SerializableNullable<int>.ToNullable(_initialViewsSortingLayerId);
            public int? InitialViewsOrderInLayer => SerializableNullable<int>.ToNullable(_initialViewsOrderInLayer);
            public uint? UnitialViewsRenderingLayerMask => SerializableNullable<uint>.ToNullable(_initialViewsRenderingLayerMask);



            public StandartDirections GetDirectionsFlags()
            {
                var flags = StandartDirections.None;

                foreach (var f in _views)
                {
                    flags |= f.Direction;
                }

                return flags;
            }

            internal void EnsureViewsUniqueness()
            {
                var hs = HashSetPool<StandartDirections>.Get();

                var arr = _views;
                var len = arr.Length;

                for (int i = 0; i < len; i++)
                {
                    if (!hs.Add(arr[i].Direction))
                    {
                        Debug.LogError($"Non-unique direction detected. Index: {i}, direction: {arr[i].Direction}");
                        DeleteDuplicates(hs);
                        break;
                    }
                }

                HashSetPool<StandartDirections>.Release(hs);
            }

            private void DeleteDuplicates(HashSet<StandartDirections> pooledHs)
            {
                var list = ListPool<DirectionSprite>.Get();

                var arr = _views;
                var len = arr.Length;

                for (int i = 0; i < len; i++)
                {
                    var ds = arr[i];
                    if (pooledHs.Add(ds.Direction))
                    {
                        list.Add(ds);
                    }
                }

                DirectionSprite[] views = list.ToArray();
                ListPool<DirectionSprite>.Release(list);
                _views = views;
            }
        }


        [Flags]
        internal enum StandartDirections
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


        internal static string GetStandartDirectionName(StandartDirections direction)
        {
            return direction switch
            {
                StandartDirections.Forward => "Forward",
                StandartDirections.Back => "Back",
                StandartDirections.Left => "Left",
                StandartDirections.Right => "Right",
                _ => throw new System.NotSupportedException($"name for direction {direction} is not " +
                $"implemented or {nameof(direction)} argument has more than 1 direction")
            };
        }

        internal static string GetStandartFromDirectionName(StandartDirections direction)
        {
            return direction switch
            {
                StandartDirections.Forward => "From forward",
                StandartDirections.Back => "From back",
                StandartDirections.Left => "From left",
                StandartDirections.Right => "From right",
                _ => throw new System.NotSupportedException($"name for direction {direction} is not " +
                $"implemented or {nameof(direction)} argument has more than 1 direction")
            };
        }


        internal static Vector3 StandartDirectionToVector(StandartDirections direction)
        {
            return direction switch
            {
                StandartDirections.Forward => Vector3.forward,
                StandartDirections.Back => Vector3.back,
                StandartDirections.Left => Vector3.left,
                StandartDirections.Right => Vector3.right,
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
            var dirSprites = GetUniqueDirectionSprites();

            TwoDThreeDComponent.BillBoardSettings billBoardSettings = new(false, false, true);
            var viewsSettings = new TwoDThreeDView[dirSprites.Count];

            for (int i = 0; i < dirSprites.Count; i++)
            {
                var relView = CreateView(viewsRoot, dirSprites[i]);

                //if (shouldMirror && dir == StandartDirections.Left)
                //{
                //    viewSr.flipX = true;
                //}

                //viewSr.sprite = dirSprite.Sprite;
                //var dirTr = CreateDirectionTransform(dirName, directionsRoot, StandartDirectionToVector(dir));

                viewsSettings[i] = relView;
            }

            tdtd.InitInternal(billBoardSettings, viewsSettings, transform.root);

            // Probably overkill due to SetDirty(transform.root) in OnValidate().
            UnityEditor.EditorUtility.SetDirty(tdtd);
        }

        private IReadOnlyList<DirectionSprite> GetUniqueDirectionSprites()
        {
            _settings.EnsureViewsUniqueness();
            return _settings.Views;
        }


        private TwoDThreeDView CreateView(Transform root, DirectionSprite directionSprite)
        {
            StandartDirections dir = directionSprite.Direction;
            string fromDirName = GetStandartFromDirectionName(dir);
            var viewTr = CreateChildTransform(fromDirName, root);
            var go = viewTr.gameObject;

#if RelativeViewUsesSpriteRenderer
            var sr = go.AddComponent<SpriteRenderer>();
            sr.drawMode = SpriteDrawMode.Simple;
            sr.spriteSortPoint = SpriteSortPoint.Pivot;

            if (TryGetSortingLayer(out int layerId))
            {
                sr.sortingLayerID = layerId;
            }

            if (TryGetRenderingLayerMask(out uint rlmask))
            {
                sr.renderingLayerMask = rlmask;
            }

            // Sprite Renderers by default oriented to face backwards.
            // In 3D Space we need them to face forward. We can achieve
            // it by either flipping it or  rotating 180 degrees or
            // multiplying scale.x by -1. Flipping is cheap enough,
            // but manipulating scale 
            var scale = viewTr.localScale;
            scale.x = -scale.x;
            viewTr.localScale = scale;
            sr.sprite = directionSprite.Sprite;
#else
            var renderer = go.AddComponent<MeshRenderer>();
            //renderer.ma
#endif


            var dirTr = CreateDirectionTransform(fromDirName, root, StandartDirectionToVector(dir));
            var relView = new TwoDThreeDView();
            return relView;
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

        private bool TryGetSortingLayer(out int layerIndex)
        {
            int? layerId = _settings.InitialViewsSortingLayerId;

            if (layerId.HasValue)
            {
                layerIndex = layerId.Value;
                return true;
            }

            if (!string.IsNullOrWhiteSpace(_settings.InitialViewsSortingLayerName))
            {
                layerIndex = SortingLayer.NameToID(_settings.InitialViewsSortingLayerName);
                return layerIndex > 0;
            }

            layerIndex = -1;
            return false;
        }

        private bool TryGetRenderingLayerMask(out uint rlmask)
        {
            uint? rlmaskRaw = _settings.UnitialViewsRenderingLayerMask;

            if (rlmaskRaw.HasValue)
            {
                rlmask = rlmaskRaw.Value;
                return true;
            }

            rlmask = default;
            return false;
        }
#endif

            private void Start()
        {
            Destroy(this);
        }
    }
}
