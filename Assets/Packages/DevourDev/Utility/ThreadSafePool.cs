using System;

namespace DevourDev.Utility
{
    public abstract class ThreadSafePool<T> where T : class
    {
        public const int DefaultBufferSize = 64;

        private readonly object _lockObject;

        private readonly int _bufferSize;
        private readonly T[] _pool;
        private int _count;


        protected ThreadSafePool() : this(DefaultBufferSize)
        {

        }

        protected ThreadSafePool(int bufferSize)
        {
            _bufferSize = bufferSize;

        }


        public T Rent()
        {
            lock (_lockObject)
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
        }

        public void Return(T item)
        {
            lock (_lockObject)
            {
                if (_count == _bufferSize)
                {
                    OnItemReturned(item, true);
                    DestroyItem(item);
                    return;
                }

                OnItemReturned(item, false);
                _pool[_count++] = item;
            }
        }

        public void Clear()
        {
            lock (_lockObject)
            {
                int count = _count;
                var span = _pool.AsSpan(0, count);

                for (int i = 0; i < count; i++)
                {
                    DestroyItem(span[i]);
                    span[i] = null;
                }

                _count = 0;
            }
        }


        protected abstract T CreateItem();


        protected virtual void OnItemReturned(T returnedItem, bool willBeDestroyed)
        {

        }

        protected virtual void OnBeforeItemRented(T item)
        {

        }

        protected virtual void DestroyItem(T item)
        {

        }

    }
}
