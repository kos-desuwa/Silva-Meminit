using System;
using System.Collections.Generic;
using System.Text;
using static Silva_Meminit.GameConfig;
namespace Silva_Meminit
{

    class InvalidTreeException(int age) : Exception($"Invalid tree age: {age}");
    internal class Tree
    {
        static bool IsValidTree(int age) => age > 0 && age <= DeathAge;
        int age;
        public int Age
        {
            get => age;
            set
            {
                age = IsValidTree(value) ?
                    value : throw new InvalidTreeException(value);
            }
        }
        public ConsoleColor Color { get; private init; }
        public Tree? Parent { get; private init; }
        public Tree(int age, ConsoleColor color, Tree? parent)
        {
            Age = age;
            Color = color;
            Parent = parent;
        }
        public Tree(ConsoleColor color, Tree? parent) : this(1, color, parent) { }
        public Tree() : this(1, ConsoleColor.Green, null) { }
        public bool IsMature => Age >= MaturityAge;
        public bool IsDead => Age >= DeathAge;
        public bool CanReproduce => IsMature && !IsDead;
        public bool Reproduces()
        {
            int chance = Random.Shared.Next(1, 101);
            return chance <= ReproductionAgeBonus * (Age - MaturityAge) + BaseReproductionChance;
        }
        public override string ToString() => Age.ToString();
    }
}
