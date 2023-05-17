namespace DevourDev.Unity.Diplomacy
{
    public interface IDiplomacyResolver
    {
        AllyFlags GetAllyMode(TeamSo teamA, TeamSo teamB);
    }
}
