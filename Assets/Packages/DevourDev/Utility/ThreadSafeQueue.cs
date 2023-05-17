using System.Collections.Generic;

namespace DevourDev.Utility
{
    public sealed class ThreadSafeQueue<T>
    {
        private readonly Queue<T> _internalQ;
        private readonly object _locker = new();


        public ThreadSafeQueue()
        {
            _internalQ = new();
        }

        public ThreadSafeQueue(int initialCapacity)
        {
            _internalQ = new Queue<T>(initialCapacity);
        }


        public void Enqueue(T item)
        {
            lock (_locker)
            {
                _internalQ.Enqueue(item);
            }
        }

        public bool TryDequeue(out T item)
        {
            lock (_locker)
            {
                return _internalQ.TryDequeue(out item);
            }
        } 
    }
}
