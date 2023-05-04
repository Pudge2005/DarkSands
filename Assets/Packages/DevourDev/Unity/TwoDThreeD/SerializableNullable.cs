using UnityEngine;

namespace DevourDev.Unity.TwoDThreeD
{
    [System.Serializable]
    public sealed class SerializableNullable<T> where T : struct
    {
        [SerializeField] private T _value;
        [SerializeField] private bool _hasValue;


        public SerializableNullable(T value)
        {
            _value = value;
            _hasValue = true;
        }


        public T Value => _value;


        public static T? ToNullable(SerializableNullable<T> serializableNullable)
        {
            if (HasValue(serializableNullable))
                return default;

            return serializableNullable.Value;
        }


        public static bool HasValue(SerializableNullable<T> serializableNullable)
        {
            return serializableNullable == null || !serializableNullable._hasValue;
        }
    }
}
