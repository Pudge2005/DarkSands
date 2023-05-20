using System;
using UnityEngine;

namespace DevourDev.Utility
{
    public class UnityAutoPool<T> : MonoBehaviour
         where T : UnityEngine.Object, IAutoPoolableItem
    {
        public const int DefaultBufferSize = 128;

        [SerializeField] private T _prefab;
        [SerializeField, Min(0)] private int _bufferSize = DefaultBufferSize;

        private T[] _pool;
        private int _count;


        protected virtual void Awake()
        {
            InitPool(_bufferSize);
        }


        private void InitPool(int bufferSize)
        {
            _pool = new T[bufferSize];
            _count = 0;
        }


        public T Rent()
        {
            T item;

            if (_count == 0)
            {
                item = CreateItem();
            }
            else
            {
                item = _pool[--_count];
                _pool[_count] = null;
            }

            OnBeforeItemRented(item);
            return item;
        }

        public void Return(T item)
        {
            if (item == null)
                return;

            if (_count == _bufferSize)
            {
                OnItemReturned(item, true);
                DestroyItem(item);
                return;
            }

            OnItemReturned(item, false);
            _pool[_count++] = item;
        }

        public void Clear()
        {
            int count = _count;
            var span = _pool.AsSpan(0, count);

            for (int i = 0; i < count; i++)
            {
                if (span[i] == null)
                    continue;

                DestroyItem(span[i]);
                span[i] = null;
            }

            _count = 0;
        }


        private T CreateItem()
        {
            return Instantiate(_prefab);
        }


        protected virtual void OnItemReturned(T returnedItem, bool willBeDestroyed)
        {
            returnedItem.OnReturn();
        }

        protected virtual void OnBeforeItemRented(T item)
        {
            item.OnBeforeRent();
        }

        protected virtual void DestroyItem(T item)
        {
            item.OnPoolableItemDestroy();
        }

    }
}
