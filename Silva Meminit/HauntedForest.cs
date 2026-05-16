using System;
using System.Collections.Generic;
using System.Text;
using static Silva_Meminit.Algos;
namespace Silva_Meminit
{
    internal class HauntedForest : Grove
    {
        internal GhostTree?[,] GhostTrees { get; private set; }
        public HauntedForest(int height, int width) : base(height, width)
        {
            GhostTrees = new GhostTree?[height, width];
        }
        public bool IsEmpty => IsEmptyTable(GhostTrees);
        public bool IsFull => IsFullTable(GhostTrees);

        public override string ToString()
        {
            const string resetColor = "\x1b[0m";
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
                        sb.Append(!(GhostTrees[i - 1, j - 1] is null) ? GhostTrees[i - 1, j - 1]!.ToString() : ' ');
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
