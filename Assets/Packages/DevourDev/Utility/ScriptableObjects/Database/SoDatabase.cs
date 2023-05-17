using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using DevourDev.Unity.Utils;

namespace DevourDev.Utility
{
    public abstract class SoDatabase<T> : ScriptableObject, IReadOnlyList<T>, IEnumerable<T>
         where T : SoDatabaseElement
    {
        // elements to add (if not contained)
        [SerializeField] private List<T> _newElements;
        [Space]
        [SerializeField] private List<T> _elements;
#if UNITY_EDITOR
        [Header("Find")]
        [SerializeField] private bool _includeInheritedClasses;
        [SerializeField] private bool _find;

        private bool _isDirty;
#endif


        public T this[int id] => _elements[id];
        public int Count => _elements.Count;


#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            RemoveNulls();

            if (_find)
            {
                _find = false;
                FindElements();
            }

            ResolveNewElements();

            if (_isDirty)
            {
                ReassignIds();

                UnityEditor.EditorUtility.SetDirty(this);
                _isDirty = false;
            }
        }

        private void ResolveNewElements()
        {
            var newEls = _newElements;
            var c = newEls.Count;

            if (c == 0)
                return;


            var els = _elements;

            for (int i = 0; i < c; i++)
            {
                var el = newEls[i];

                if (els.Contains(el))
                    continue;

                els.Add(el);
            }

            newEls.Clear();
            _isDirty = true; //because we cleared the collection

        }

        private void ReassignIds()
        {
            var list = _elements;
            var c = list.Count;

            for (int i = 0; i < c; i++)
            {
                var el = list[i];
                el._soDatabaseElementID = i;
                UnityEditor.EditorUtility.SetDirty(el);
            }
        }

        private void RemoveNulls()
        {
            var list = _elements;
            var c = list.Count;

            for (int i = c - 1; i >= 0; i--)
            {
                if (list[i] == null)
                    list.RemoveAt(i);
            }

            if (c != list.Count)
                _isDirty = true;


        }

        private void FindElements()
        {
            if (_includeInheritedClasses)
                _ = EditorHelpers.FindAssetsOfTypeIncludingSubclasses(_newElements);
            else
                _ = EditorHelpers.FindAssetsOfTypeToCollection(_newElements);
        }

#endif

        public T GetElement(int id)
        {
            if (id < 0 || id >= _elements.Count)
                return null;

            return _elements[id];
        }

        public bool TryGetElement(int id, out T element)
        {
            element = GetElement(id);
            return element != null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_elements).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_elements).GetEnumerator();
        }
    }
}