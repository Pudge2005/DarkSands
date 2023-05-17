using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DevourDev.Utility
{
    public static class GenericCollectionsHelpers<T>
    {
        private static readonly FieldInfo _listArrayFieldInfo;
        private static readonly FieldInfo _stackArrayFieldInfo;
        private static readonly FieldInfo _queueArrayFieldInfo;


        static GenericCollectionsHelpers()
        {
            _listArrayFieldInfo = GetPrivateFieldInfo<List<T>>("_items");
            _stackArrayFieldInfo = GetPrivateFieldInfo<Stack<T>>("_array");
            _queueArrayFieldInfo = GetPrivateFieldInfo<Queue<T>>("_array");
        }


        public static T[] GetArray(List<T> list) => (T[])_listArrayFieldInfo.GetValue(list);
        public static T[] GetArray(Stack<T> stack) => (T[])_stackArrayFieldInfo.GetValue(stack);
        public static T[] GetArray(Queue<T> queue) => (T[])_queueArrayFieldInfo.GetValue(queue);

        public static ReadOnlySpan<T> GetSpan(List<T> list) => GetArray(list).AsSpan(0, list.Count);
        public static ReadOnlySpan<T> GetSpan(Stack<T> stack) => GetArray(stack).AsSpan(0, stack.Count);
        public static ReadOnlySpan<T> GetSpan(Queue<T> queue) => GetArray(queue).AsSpan(0, queue.Count);


        private static FieldInfo GetPrivateFieldInfo<T2>(string name)
        {
            return typeof(T2).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }


    public static class CollectionsHelpers
    {
        private readonly struct Enumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerator<T> _enumerator;


            public Enumerable(IEnumerator<T> enumerator)
            {
                _enumerator = enumerator;
            }


            public IEnumerator<T> GetEnumerator()
            {
                return _enumerator;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _enumerator;
            }
        }





        public static IEnumerable<T> EnumerableFromEnumerator<T>(IEnumerator<T> enumerator)
        {
            return new Enumerable<T>(enumerator);
        }


        public static void IterateEnumerator(IEnumerator enumerator, System.Action<object> action)
        {
            try
            {
                while (enumerator.MoveNext())
                {
                    action(enumerator.Current);
                }
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        public static void IterateEnumerator<T>(IEnumerator<T> enumerator, System.Action<T> action)
        {
            try
            {
                while (enumerator.MoveNext())
                {
                    action(enumerator.Current);
                }
            }
            finally
            {
                enumerator.Dispose();
            }
        }
    }
}
