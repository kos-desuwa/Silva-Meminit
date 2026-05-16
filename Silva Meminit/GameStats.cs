using Silva_Meminit;
using static System.Console;
using static System.Math;
using static Silva_Meminit.Algos;
internal class GameStats
{
    public GameLoop GameLoop { get; }
    public GameStats(GameLoop gameLoop)
    {
        GameLoop = gameLoop;
    }
    public int FindLineageDepth(Tree tree)
    {
        int depth = 0;
        for (Tree? t = tree; t != null; t = t.Parent)
            depth++;
        return depth;
    }
    public int HighestTreeCount { get; private set; }
    public int HighestGhostTreeCount { get; private set; }
    public int HighestFamilyCount { get; private set; }
    public int LongestLineage { get; private set; }
    public Dictionary<int, int> TreesAgeDistribution { get; private set; } = new();
    public Dictionary<int, int> FamilyAgeDistribution { get; private set; } = new();
    public double AverageTreeAge => CalculateAverage(TreesAgeDistribution);
    public double AverageFamilyAge => CalculateAverage(FamilyAgeDistribution);
    public double HighestAverageTreeAge { get; private set; }
    public double HighestAverageFamilyAge { get; private set; }
    public int MostPopulatedTreeAgeGroup { get; private set; }
    public int LeastPopulatedTreeAgeGroup { get; private set; }
    public int MostPopulatedFamilyAgeGroup { get; private set; }
    public int LeastPopulatedFamilyAgeGroup { get; private set; }
    public void UpdateExtremums()
    {
        HighestTreeCount = Max(GameLoop.TreeCount, HighestTreeCount);
        HighestGhostTreeCount = Max(GameLoop.GhostTreeCount, HighestGhostTreeCount);
        HighestFamilyCount = Max(GameLoop.F.Family.Count, HighestFamilyCount);
        if (GameLoop.F.Family.Count > 0)
            LongestLineage = Max(LongestLineage, GameLoop.F.Family.Max(FindLineageDepth));
        HighestAverageTreeAge = Max(AverageTreeAge, HighestAverageTreeAge);
        HighestAverageFamilyAge = Max(AverageFamilyAge, HighestAverageFamilyAge);
    }
    public void UpdateAgeDistribution()
    {
        TreesAgeDistribution.Clear();
        FamilyAgeDistribution.Clear();
        foreach (Tree? tree in GameLoop.F.Trees)
            if (!(tree is null))
                Add(TreesAgeDistribution, tree.Age);
        foreach (Tree? tree in GameLoop.F.Family)
            Add(FamilyAgeDistribution, tree.Age);
        if (TreesAgeDistribution.Count > 0)
        {
            MostPopulatedTreeAgeGroup = TreesAgeDistribution.MaxBy(x => x.Value).Key;
            LeastPopulatedTreeAgeGroup = TreesAgeDistribution.MinBy(x => x.Value).Key;
        }
        if (FamilyAgeDistribution.Count > 0)
        {
            MostPopulatedFamilyAgeGroup = FamilyAgeDistribution.MaxBy(x => x.Value).Key;
            LeastPopulatedFamilyAgeGroup = FamilyAgeDistribution.MinBy(x => x.Value).Key;
        }
    }
    public void Update()
    {
        UpdateAgeDistribution();
        UpdateExtremums();
    }
    public void Display()
    {
        string cyan = ToAnsi(ConsoleColor.Cyan);
        string reset = "\x1b[0m";
        Clear();
        WriteLine($@"{cyan}Turn : {GameLoop.Turns}
Tree Count : {GameLoop.TreeCount}
GhostTree Count : {GameLoop.GhostTreeCount}
Family Count : {GameLoop.F.Family.Count}
Highest Tree Count : {HighestTreeCount}
Highest GhostTree Count : {HighestGhostTreeCount}
Highest Family Count : {HighestFamilyCount}
Tree Spawn Count : {GameLoop.TreeSpawnCount}
Tree Death Count : {GameLoop.TreeDeathCount}
Ghost Tree Spawn Count : {GameLoop.GhostTreeSpawnCount}
Family Spawn Count : {GameLoop.FamilySpawnCount}
Family Death Count : {GameLoop.FamilyDeathCount}
Revive Count : {GameLoop.ReviveCount}
Longest Lineage : {(GameLoop.F.Family.Count != 0 ? LongestLineage : "N/A")}{reset}");
        WriteLine($"{cyan}Trees Age Groups Distribution :{reset}");
        DisplayDistribution(TreesAgeDistribution, ConsoleColor.Cyan);
        WriteLine($"{cyan}Family Age Groups Distribution :{reset}");
        DisplayDistribution(FamilyAgeDistribution, ConsoleColor.Cyan);
        WriteLine($@"{cyan}Average Tree Age : {AverageTreeAge:F4}
Average Family Age : {(FamilyAgeDistribution.Count != 0 ? AverageFamilyAge.ToString("F4") : "N/A")}
Highest Average Tree Age : {HighestAverageTreeAge:F4}
Highest Average Family Age : {(FamilyAgeDistribution.Count != 0 ? HighestAverageFamilyAge.ToString("F4") : "N/A")}
Most Populated Tree Age Group :  {MostPopulatedTreeAgeGroup}
Least Populated Tree Age Group : {LeastPopulatedTreeAgeGroup}
Most Populated Family Age Group : {(FamilyAgeDistribution.Count != 0 ? MostPopulatedFamilyAgeGroup : "N/A")}
Least Populated Family Age Group : {(FamilyAgeDistribution.Count != 0 ? LeastPopulatedFamilyAgeGroup : "N/A")}{reset}");
    }
}