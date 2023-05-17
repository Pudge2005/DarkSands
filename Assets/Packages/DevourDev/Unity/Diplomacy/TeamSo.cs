using DevourDev.Utility;
using UnityEngine;
using UnityEngine.Localization;

namespace DevourDev.Unity.Diplomacy
{
    [CreateAssetMenu(menuName = "DevourDev/Unity/Ai/Team")]
    public sealed class TeamSo : SoDatabaseElement
    {
        [SerializeField] private LocalizedString _teamName;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Color _color;



        public LocalizedString TeamName => _teamName;
        public Sprite Icon => _icon;
        public Color Color => _color;


        public static TeamSo Create(LocalizedString teamName, Sprite icon, Color color)
        {
            var team = ScriptableObject.CreateInstance<TeamSo>();
            team._teamName = teamName;
            team._icon = icon;
            team._color = color;

            return team;
        }
    }
}
