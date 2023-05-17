namespace DevourDev.Utility
{
    public abstract class NamedSoDatabase<T> : SoDatabase<T>
        where T : NamedSoDatabaseElement
    {
        private MultiDict<T> _multiDict;


        private MultiDict<T> MultiDict
        {
            get
            {
                if (_multiDict == null)
                {
                    _multiDict = new(Count);
                    _multiDict.AddKey((x) => x.UniformedName);
                    _multiDict.AddRange(this);
                }

                return _multiDict;
            }
        }


        public bool TryGetFromName(string name, out T item)
        {
            return MultiDict.TryGetItem(name.ToLower(), out item);
        }
    }
}