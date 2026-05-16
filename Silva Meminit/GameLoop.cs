using System;
using System.Collections.Generic;
using System.Text;
using System.Media;
using static Silva_Meminit.GameConfig;
using static Silva_Meminit.Algos;
namespace Silva_Meminit
{
    internal class GameLoop
    {
        public Forest F { get; }
        public HauntedForest HF { get; }
        public GameLoop(int height, int width)
        {
            F = new Forest(height, width);
            HF = new HauntedForest(height, width);
        }
        public int Turns { get; private set; } = 0;
        private int Height => F.Height;
        private int Width => F.Width;
        internal Tree?[,] Trees => F.Trees;
        internal GhostTree?[,] GhostTrees => HF.GhostTrees;
        public bool CanRevive => Turns > 0 && Turns % TurnsPerRevival == 0;
        public bool CanBeRevived(int r, int c) => !(GhostTrees[r, c] is null) && Trees[r, c] is null;
        private List<(int r, int c)> ValidRevivals { get; set; } = new();
        public int TreeCount { get; private set; } = 0;
        public int GhostTreeCount { get; private set; } = 0;
        public int TreeSpawnCount { get; private set; } = 0;
        public int FamilySpawnCount { get; private set; } = 0;
        public int TreeDeathCount { get; private set; } = 0;
        public int GhostTreeSpawnCount { get; private set; } = 0;
        public int FamilyDeathCount { get; private set; } = 0;
        public int ReviveCount { get; private set; } = 0;
        public void PlantATree()
        {
            if (F.IsFull) return;
            int randRow = Random.Shared.Next(Height);
            int ranColumn = Random.Shared.Next(Width);
            while (!(Trees[randRow, ranColumn] is null))
            {
                randRow = Random.Shared.Next(Height);
                ranColumn = Random.Shared.Next(Width);
            }
            Trees[randRow, ranColumn] = new();
            TreeCount++;
            TreeSpawnCount++;
        }
        public void FillForest(int amt)
        {
            for (int i = 0; i < amt; i++)
                PlantATree();
        }
        public void AgeForest()
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    if (!(Trees[i, j] is null))
                    {
                        Trees[i, j]!.Age++;
                        if (Trees[i, j]!.IsDead)
                        {
                            if (F.IsInFamily(Trees[i, j]!))
                            {
                                FamilyDeathCount++;
                                if (GhostTrees[i, j] is null)
                                {
                                    GhostTrees[i, j] = new(Trees[i, j]!.Parent);
                                    GhostTreeCount++;
                                    GhostTreeSpawnCount++;
                                }
                                F.Family.Remove(Trees[i, j]!);
                            }
                            Trees[i, j] = null;
                            TreeCount--;
                            TreeDeathCount++;
                        }
                    }
        }
        public void PlantAroundTree(int r, int c)
        {
            int[] vDirections = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] hDirections = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int direction = Random.Shared.Next(8);
            int vSpawn = r + vDirections[direction];
            int hSpawn = c + hDirections[direction];
            if (F.IsInBounds(vSpawn, hSpawn) && Trees[vSpawn, hSpawn] is null)
            {
                Tree? parent = Trees[r, c]!;
                Tree child = new(F.IsInFamily(parent) ? ConsoleColor.Yellow : ConsoleColor.Green, parent);
                if (F.IsInFamily(parent))
                {
                    F.Family.Add(child);
                    FamilySpawnCount++;
                }
                Trees[vSpawn, hSpawn] = child;
                TreeCount++;
                TreeSpawnCount++;
            }
        }
        void PopulateAroundTree(int r, int c)
        {
            if (Trees[r, c]!.Reproduces())
                PlantAroundTree(r, c);
        }
        public void PopulateForest()
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    if (Trees[i, j] is Tree tree && tree.CanReproduce)
                        PopulateAroundTree(i, j);
        }
        void FindValidRevivalPositions()
        {
            ValidRevivals.Clear();
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    if (CanBeRevived(i, j))
                        ValidRevivals.Add((i, j));
        }
        public void Revive()
        {
            FindValidRevivalPositions();
            if (ValidRevivals.Count == 0) return;
            int index = Random.Shared.Next(ValidRevivals.Count);
            var (r, c) = ValidRevivals[index];
            Tree? parent = GhostTrees[r, c]!.Parent;
            char symbol = GhostTrees[r, c]!.Symbol;
            GhostTrees[r, c] = null;
            GhostTreeCount--;
            Trees[r, c] = new(ConsoleColor.Yellow, parent);
            TreeCount++;
            ReviveCount++;
            WriteJournal(symbol);
        }
        private void WriteJournal(char symbol)
        {
            using (StreamWriter sw = new("../../../memoria.txt", true))
            {
                sw.Write(symbol);
                if ((sw.BaseStream.Length + 1) % 50 == 0)
                    sw.WriteLine();
                if (Random.Shared.Next(180) == 0)
                {
                    DateTime end = DateTime.Now.AddSeconds(1);
                    sw.Write("lime");
                    PlaySoundEffect("lime.wav");
                    while(DateTime.Now < end)
                        Console.Write("lime");
                    Console.Clear();
                }
            }
        }
        public void EvolveGame()
        {
            Turns++;
            AgeForest();
            PopulateForest();
            if (CanRevive)
                Revive();
        }
    }
}
