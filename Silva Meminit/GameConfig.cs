using System;
using System.Collections.Generic;
using System.Text;

namespace Silva_Meminit
{
    public static class GameConfig
    {
        public const int MaturityAge = 3; //0 < MaturityAge <= DeathAge
        public const int DeathAge = 10; // 0 < DeathAge <= 10
        // BaseReproductionChance + ReproductionAgeBonus * (DeathAge - MaturityAge) <= 100
        public const int BaseReproductionChance = 25;
        public const int ReproductionAgeBonus = 5;
        public const int TurnsPerRevival = 10;
    }

}
