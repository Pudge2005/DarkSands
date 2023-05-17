using System.Collections.Generic;
using DevourDev.Unity.Diplomacy;
using UnityEngine;

namespace DarkSands.Diplomacy
{
    internal sealed class PredefinedDiplomacyResolver : MonoBehaviour, IDiplomacyResolver
    {
        [SerializeField] private TeamSo _ally;
        [SerializeField] private TeamSo _enemy;
        [SerializeField] private TeamSo _neutral;

        private HashSet<int> _neutralsHs;


        private void Awake()
        {
            CacheTeams();
            DiplomacyManager.SetDiplomacyResolver(this);
        }

        private void CacheTeams()
        {
            var hs = new HashSet<int>(3)
            {
                _ally.GetDatabaseElementID(),
                _enemy.GetDatabaseElementID(),
                _neutral.GetDatabaseElementID()
            };

            _neutralsHs = hs;
        }

        public AllyFlags GetAllyMode(TeamSo teamA, TeamSo teamB)
        {
            return _neutralsHs.Contains(teamA.GetDatabaseElementID())
                || _neutralsHs.Contains(teamB.GetDatabaseElementID())
                ? AllyFlags.Neutral : AllyFlags.Enemy;
        }
    }
}
