using System;
using System.Collections.Generic;
using System.Text;
using static Silva_Meminit.Algos;
namespace Silva_Meminit
{
    class InvalidGhostTreeException(char representation) : Exception($"Invalid ghostTree symbol : '{representation}'");
    internal class GhostTree
    {
        static bool IsValidGhostTree(char symbol)
        {
            int asciiValue = (int)symbol;
            if (asciiValue <= 32 || asciiValue >= 48 && asciiValue <= 57) return false;
            if (asciiValue is 127 or 129 or 141 or 143 or 144 or 157) return false;
            return true;
        }
        char symbol;
        public char Symbol
        {
            get => symbol;
            init
            {
                symbol = IsValidGhostTree(value) ?
                    value : throw new InvalidGhostTreeException(value);
            }
        }
        public Tree? Parent { get; private init; }
        public GhostTree(Tree? parent)
        {
            Parent = parent;
            int ascii = Random.Shared.Next(33, 256);
            while(!IsValidGhostTree((char)ascii))
                ascii = Random.Shared.Next(33, 256);
            Symbol = (char)ascii;
        }
        public override string ToString() => $"{ToAnsi(ConsoleColor.DarkGray)}{Symbol}\x1b[0m";
    }
}
