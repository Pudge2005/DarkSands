using UnityEngine;

namespace DevourDev.Utility
{
    public abstract class NamedSoDatabaseElement : SoDatabaseElement
    {
        [SerializeField] private string _elementName;

        private string _uniformedName;


        public string ElementName => _elementName;

        public string UniformedName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_uniformedName))
                    _uniformedName = _elementName.ToLower();

                return _uniformedName;
            }
        }
    }
}