using System;
using System.Collections.Generic;
using System.Text;
using static Silva_Meminit.Algos;
namespace Silva_Meminit
{
    internal class Forest : Grove
    {
        internal Tree?[,] Trees { get; private set; }
        public Forest(int height, int width) : base(height, width)
        {
            Trees = new Tree?[height, width];
        }
        public bool IsEmpty => IsEmptyTable(Trees);
        public bool IsFull => IsFullTable(Trees);
        public HashSet<Tree> Family { get; private init; } = new();
        public Tree? FatherTree { get; private set; }
        public bool IsInFamily(Tree tree) => Family.Contains(tree);
        public override string ToString()
        {
            string resetColor = "\x1b[0m";
            StringBuilder sb = new();
            for (int i = 0; i < Height + 1; i++)
            {
                for (int j = 0; j < Width + 1; j++)
                {
                    if (i == 0)
                        sb.Append($"{ToAnsi(ConsoleColor.White)}―{resetColor}");
                    else if (j == 0)
                        sb.Append($"{ToAnsi(ConsoleColor.White)}|{resetColor}");
                    else
                        sb.Append(Trees[i - 1, j - 1] is Tree tree ? $"{ToAnsi(tree.Color)}{tree.Age}{resetColor}" : " ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
        public bool PickFatherTree()
        {
            int r = ReadInteger("Pick the fatherTree's row (or -1 to cancel) : ");
            if (r == -1) return false;
            int c = ReadInteger("Pick the fatherTree's column (or -1 to cancel) : ");
            if (c == -1) return false;
            while (!IsInBounds(r, c) || !(Trees[r, c] is Tree tree))
            {
                Console.WriteLine("No tree at that position.");
                r = ReadInteger("Pick the fatherTree's row (or -1 to cancel) : ");
                if (r == -1) return false;
                c = ReadInteger("Pick the fatherTree's column (or -1 to cancel) : ");
                if (c == -1) return false;
            }
            Tree picked = Trees[r, c]!;
            FatherTree = new(picked.Age, ConsoleColor.DarkYellow, null);
            Family.Add(FatherTree);
            Trees[r, c] = FatherTree;
            return true;
        }
    }
}
