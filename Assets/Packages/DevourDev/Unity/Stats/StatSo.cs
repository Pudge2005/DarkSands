using UnityEngine;

namespace DevourDev.Unity.Stats
{
    [CreateAssetMenu(menuName ="DevourDev/Stats/Stat Object")]
    public sealed class StatSo : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;


        public string Name => _name;
        public Sprite Icon => _icon;
    }
}
