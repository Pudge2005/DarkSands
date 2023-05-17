using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DevourDev.Utility
{
    public sealed class MultiDict<T>
    {
        public delegate TKey GetKeyDelegate<TKey>(T item);


        private abstract class RawContainer
        {
            internal abstract bool TryGetItemRaw(object keyRaw, out object itemRawS);

            internal abstract bool TryAddItemRaw(object itemRaw);

            internal abstract bool TryRemoveItemRaw(object itemRaw);

            public abstract void Clear();
        }


        private sealed class Container<TKey> : RawContainer, IDictionary<TKey, T>
        {
            private readonly Dictionary<TKey, T> _dict;
            private readonly GetKeyDelegate<TKey> _getKeyDelegate;


            public Container(IEnumerable<T> items, GetKeyDelegate<TKey> getKeyDelegate)
            {
                _getKeyDelegate = getKeyDelegate ?? throw new ArgumentNullException(nameof(getKeyDelegate));
                Dictionary<TKey, T> dict = new(items.Count());

                foreach (var item in items)
                {
                    dict.Add(_getKeyDelegate(item), item);
                }

                _dict = dict;
            }

            public Container(IEnumerable<T> items, int capacity, GetKeyDelegate<TKey> getKeyDelegate)
            {
                _getKeyDelegate = getKeyDelegate;
                Dictionary<TKey, T> dict = new(capacity);


                foreach (var item in items)
                {
                    dict.Add(_getKeyDelegate(item), item);
                }

                _dict = dict;
            }


            public ICollection<TKey> Keys => ((IDictionary<TKey, T>)_dict).Keys;

            public ICollection<T> Values => ((IDictionary<TKey, T>)_dict).Values;

            public int Count => ((ICollection<KeyValuePair<TKey, T>>)_dict).Count;

            public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, T>>)_dict).IsReadOnly;

            public T this[TKey key] { get => ((IDictionary<TKey, T>)_dict)[key]; set => ((IDictionary<TKey, T>)_dict)[key] = value; }


            public TKey GetKey(T item)
            {
                return _getKeyDelegate(item);
            }

            internal override bool TryGetItemRaw(object keyRaw, out object itemRaw)
            {
                var key = (TKey)keyRaw;

                if (!_dict.TryGetValue(key, out var item))
                {
                    itemRaw = null;
                    return false;
                }

                itemRaw = item;
                return true;
            }

            internal override bool TryAddItemRaw(object itemRaw)
            {
                return itemRaw is T item && AddItem(item);
            }

            internal override bool TryRemoveItemRaw(object itemRaw)
            {
                return itemRaw is T item && RemoveItem(item);
            }


            public bool AddItem(T item)
            {
                return _dict.TryAdd(_getKeyDelegate(item), item);
            }

            public bool RemoveItem(T item)
            {
                return _dict.Remove(_getKeyDelegate(item));
            }


            public void Add(TKey key, T value)
            {
                ((IDictionary<TKey, T>)_dict).Add(key, value);
            }

            public bool ContainsKey(TKey key)
            {
                return ((IDictionary<TKey, T>)_dict).ContainsKey(key);
            }

            public bool Remove(TKey key)
            {
                return ((IDictionary<TKey, T>)_dict).Remove(key);
            }

            public bool TryGetValue(TKey key, out T value)
            {
                return ((IDictionary<TKey, T>)_dict).TryGetValue(key, out value);
            }

            public void Add(KeyValuePair<TKey, T> item)
            {
                ((ICollection<KeyValuePair<TKey, T>>)_dict).Add(item);
            }

            public override void Clear()
            {
                _dict.Clear();
            }

            public bool Contains(KeyValuePair<TKey, T> item)
            {
                return ((ICollection<KeyValuePair<TKey, T>>)_dict).Contains(item);
            }

            public void CopyTo(KeyValuePair<TKey, T>[] array, int arrayIndex)
            {
                ((ICollection<KeyValuePair<TKey, T>>)_dict).CopyTo(array, arrayIndex);
            }

            public bool Remove(KeyValuePair<TKey, T> item)
            {
                return ((ICollection<KeyValuePair<TKey, T>>)_dict).Remove(item);
            }

            public IEnumerator<KeyValuePair<TKey, T>> GetEnumerator()
            {
                return ((IEnumerable<KeyValuePair<TKey, T>>)_dict).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)_dict).GetEnumerator();
            }
        }


        private readonly int _initialCapacity;
        private readonly Dictionary<System.Type, RawContainer> _containers = new();


        public MultiDict(int initialCapacity = 0)
        {
            _initialCapacity = initialCapacity;
        }


        public void AddKey<TKey>(GetKeyDelegate<TKey> getKeyDelegate)
        {
#if UNITY_EDITOR
            if (_containers.Count > 0 && ((ICollection)_containers.First().Value).Count > 0)
            {
                InvalidOperationException exception = new($"Keys cannot be added to {GetType().Name} with containing elements.");
                UnityEngine.Debug.LogError(exception.Message);
                throw exception;
            }
#endif

            _containers.Add(typeof(TKey), new Container<TKey>(Enumerable.Empty<T>(), _initialCapacity, getKeyDelegate));
        }

        public bool Add(T item)
        {
            foreach (var kvp in _containers)
            {
                var containerRaw = kvp.Value;

                if (!containerRaw.TryAddItemRaw(item))
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError($"Unable to add item {item}");
#endif
                    return false;
                }
            }

            return true;
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var kvp in _containers)
            {
                var containerRaw = kvp.Value;

                foreach (var item in items)
                {
                    if (!containerRaw.TryAddItemRaw(item))
                    {
#if UNITY_EDITOR
                        UnityEngine.Debug.LogError($"Unable to add item {item}");
#endif
                        continue;
                    }
                }
            }

        }

        public bool Remove(T item)
        {
            foreach (var kvp in _containers)
            {
                var containerRaw = kvp.Value;

                if (!containerRaw.TryRemoveItemRaw(item))
                    return false;
            }

            return true;
        }

        public bool TryGetItem<TKey>(TKey key, out T item)
        {
            if (!_containers.TryGetValue(typeof(TKey), out var rawContainer))
            {
                item = default;
                return false;
            }

            return ((Container<TKey>)rawContainer).TryGetValue(key, out item);
        }


        public void Clear()
        {
            foreach (var item in _containers.Values)
            {
                item.Clear();
            }
        }
    }
}
