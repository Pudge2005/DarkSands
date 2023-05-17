using UnityEngine;

namespace DevourDev.Unity.Diplomacy
{
    public static class DiplomacyManager
    {
        private static IDiplomacyResolver _diplomacyResolver;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void RuntimeInit()
        {
            _diplomacyResolver = null;
        }


        public static void SetDiplomacyResolver(IDiplomacyResolver resolver)
        {
            _diplomacyResolver = resolver;
        }


        public static AllyFlags GetAllyMode(TeamSo teamA, TeamSo teamB)
        {
            if (teamA == teamB)
                return AllyFlags.Ally;

            return _diplomacyResolver.GetAllyMode(teamA, teamB);
        }
    }
}
