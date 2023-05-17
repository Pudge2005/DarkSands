namespace DevourDev.Utility
{
    public abstract class Pool<T> where T : class
    {
        public const int DefaultBufferSize = 1024;
        private readonly T[] _buffer;
        private int _count;


        public Pool(int bufferSize = DefaultBufferSize)
        {
            _buffer = new T[bufferSize];
        }


        public T Rent()
        {
            T entity;

            if (_count == 0)
                entity = CreateEntity();
            else
                entity = TakeFromBuffer();

            HandleEntityBeforeRent(entity);
            return entity;
        }

        public void Return(T rentedEntity)
        {
            HandleReturnedEntity(rentedEntity);

            if (_count == _buffer.Length)
                DestroyEntity(rentedEntity);
            else
                _buffer[_count++] = rentedEntity;
        }

        public void Clear()
        {
            var arr = _buffer;
            var c = _count;

            for (int i = -1; ++i < c;)
            {
                DestroyEntity(arr[i]);
                arr[i] = null;
            }

            _count = 0;
        }


        private T TakeFromBuffer()
        {
            return _buffer[--_count];
        }


        protected abstract T CreateEntity();
        protected abstract void HandleEntityBeforeRent(T entityForRent);
        protected abstract void HandleReturnedEntity(T returnedEntity);
        protected abstract void DestroyEntity(T entity);
    }
}
