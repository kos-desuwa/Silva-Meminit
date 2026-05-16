using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Media;

namespace Silva_Meminit
{
    public static class Algos
    {
        public static (int r, int c) FindNearest<T>(T[,] t, int r, int c, Func<T, bool> pred)
        {
            double minDistance = double.MaxValue;
            int closestRowIndex = 0;
            int closestColumnIndex = 0;
            for (int i = 0; i < t.GetLength(0); i++)
                for (int j = 0; j < t.GetLength(1); j++)
                {
                    if (!(t[i, j] is null) && (i != r || j != c) && pred(t[i, j]!))
                    {
                        double distance = Math.Sqrt(Math.Pow(i - r, 2) + Math.Pow(j - c, 2));
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestRowIndex = i;
                            closestColumnIndex = j;
                        }
                    }
                }
            return (closestRowIndex, closestColumnIndex);
        }
        public static void DisplayDistribution(Dictionary<int, int> d, ConsoleColor color)
        {
            string ansiValue = ToAnsi(color);
            string reset = "\x1b[0m";
            if (d.Count == 0)
            {
                Console.WriteLine($"{ansiValue}N/A{reset}");
                return;
            }
            foreach ((int key, int value) in d.OrderBy(d => d.Key))
                Console.WriteLine($"{ansiValue}{key} : {value}{reset}");
        }
        public static double CalculateAverage(Dictionary<int, int> d) => d.Count == 0 ? 0 : (double)d.Sum(
        dict => dict.Key * dict.Value) / d.Values.Sum();
        public static void Add<T>(Dictionary<T, int> d, T key) where T : notnull
        {
            if (d.ContainsKey(key))
                d[key]++;
            else
                d.Add(key, 1);
        }
        public static string ToAnsi(ConsoleColor color) => color switch
        {
            ConsoleColor.Black => "\x1b[30m",
            ConsoleColor.DarkRed => "\x1b[31m",
            ConsoleColor.DarkGreen => "\x1b[32m",

            ConsoleColor.DarkYellow => "\x1b[33m",
            ConsoleColor.DarkBlue => "\x1b[34m",
            ConsoleColor.DarkMagenta => "\x1b[35m",
            ConsoleColor.DarkCyan => "\x1b[36m",
            ConsoleColor.Gray => "\x1b[37m",
            ConsoleColor.DarkGray => "\x1b[90m",
            ConsoleColor.Red => "\x1b[91m",
            ConsoleColor.Green => "\x1b[92m",
            ConsoleColor.Yellow => "\x1b[93m",
            ConsoleColor.Blue => "\x1b[94m",
            ConsoleColor.Magenta => "\x1b[95m",
            ConsoleColor.Cyan => "\x1b[96m",
            ConsoleColor.White => "\x1b[97m",
            _ => "\x1b[0m"
        };
        public static bool IsEmptyTable<T>(T[,] table)
        {
            for (int i = 0; i < table.GetLength(0); i++)
                for (int j = 0; j < table.GetLength(1); j++)
                    if (!(table[i, j] is null))
                        return false;
            return true;
        }
        public static bool IsFullTable<T>(T[,] table)
        {
            for (int i = 0; i < table.GetLength(0); i++)
                for (int j = 0; j < table.GetLength(1); j++)
                    if (table[i, j] is null)
                        return false;
            return true;
        }
        public static int ReadInteger(string message)
        {
            int result;
            Console.Write(message);
            while (!int.TryParse(Console.ReadLine(), out result))
                Console.Write("Invalid input, try again : ");
            return result;
        }
        public static int ReadIntegerBetweenInterval(string message, int min, Func<int> max)
        {
            int result;
            Console.Write(message);
            while (!int.TryParse(Console.ReadLine(), out result) || result < min || result > max())
                Console.Write($"Invalid input, enter a number between {min} and {max()}: ");
            return result;
        }
        public static bool All<T>(List<T> src, Func<T, bool> pred) => !Exists(src, e => !pred(e));
        public static bool Exists<T>(List<T> src, Func<T, bool> pred)
        {
            foreach (T e in src)
                if (pred(e))
                    return true;
            return false;
        }
        public static bool None<T>(List<T> src, Func<T, bool> pred) => !Exists(src, pred);
        public static Dictionary<K, int> Fusionner<K>(Dictionary<K, int> d1, Dictionary<K, int> d2) where K : notnull
        {
            Dictionary<K, int> fusionné = new(d1);
            return Cumuler(d2,
                (d, paire) =>
                {
                    if (d.ContainsKey(paire.Key))
                        d[paire.Key] += paire.Value;
                    else
                        d[paire.Key] = paire.Value;
                    return d;
                },
                fusionné
            );
        }
        public static bool TableauxSontEgaux<T>(T[] lst1, T[] lst2) where T : IEquatable<T>
        {
            if (lst1.Length != lst2.Length)
                return false;
            for (int i = 0; i < lst1.Length; i++)
                if (!lst1[i].Equals(lst2[i]))
                    return false;
            return true;
        }
        public static void Permuter<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
        public static void PermuterÉléments<T>(List<T> lst, int i, int j)
        {
            T elem = lst[i];
            lst[i] = lst[j];
            lst[j] = elem;
        }

        //public static T[] RotaterGauche<T>(T[] src)
        //{
        //   T[] dest = new T[src.Length];
        //   for(int i = 0; i != src.Length; ++i)
        //      dest[i] = src[i];
        //   for (int i = 1; i < dest.Length; ++i)
        //      Permuter(ref dest[i], ref dest[i - 1]);
        //   return dest;
        //}
        //public static List<T> RotaterGauche<T>(List<T> src) =>
        //   RotaterGauche(src.ToArray()).ToList();

        public static List<T> RotaterGauche<T>(List<T> src)
        {
            List<T> dest = new(src);
            for (int i = 1; i < src.Count; ++i)
                PermuterÉléments(dest, i, i - 1);
            return dest;
        }

        public static List<T> RotaterDroite<T>(List<T> src)
        {
            List<T> dest = new(src);
            for (int i = src.Count - 1; i > 0; --i)
                PermuterÉléments(dest, i, i - 1);
            return dest;
        }
        //public static List<T> Concaténer<T>(params IEnumerable<T>[] srcs)
        //{
        //    List<T> dest = new();
        //    foreach (var src in srcs)
        //        dest.AddRange(src);
        //    return dest;
        //}
        public static List<T> Concaténer<T>(params IEnumerable<T>[] src) =>
        Cumuler
        (
            src.ToList(),
            (a, b) => { a.AddRange(b); return a; },
            new List<T>()
        );
        public static List<U> Transformer<T, U>(IEnumerable<T> src, Func<T, U> f)
        {
            List<U> dest = new();
            foreach (T e in src)
                dest.Add(f(e));
            return dest;
        }
        public static U Cumuler<T, U>(this IEnumerable<T> src, Func<U, T, U> accum, U init) //methode d'extention 
        {
            foreach (T e in src)
                init = accum(init, e);
            return init;
        }
        public static int Trouver<T>(List<T> src, T val) where T : IEquatable<T>
            => TrouverSi(src, e => e.Equals(val));
        //{
        //    for(int i = 0; i != src.Count; i++)
        //        if (src[i].Equals(val))
        //            return i;
        //    return -1;
        //}
        public static int TrouverSi<T>(List<T> src, Func<T, bool> pred)
        {
            for (int i = 0; i != src.Count; i++)
                if (pred(src[i]))
                    return i;
            return -1;
        }
        public static List<T> Filtrer<T>(IEnumerable<T> src, T val) where T : IEquatable<T> =>
            FiltrerSi(src, e => e.Equals(val));
        //{
        //    List<T> dest = new();
        //    foreach (T e in src)
        //        if (!e.Equals(val))
        //            dest.Add(e);
        //    return dest;
        //}
        public static List<T> FiltrerSi<T>(IEnumerable<T> src, Func<T, bool> pred)
        {
            List<T> dest = new();
            foreach (T e in src)
                if (!pred(e))
                    dest.Add(e);
            return dest;
        }
        public static List<T> Remplacer<T>(List<T> src, T pre, T post) where T : IEquatable<T> =>
            RemplacerSi(src, e => e.Equals(pre), post);
        //{
        //    List<T> dest = new();
        //    foreach (T e in src)
        //        if (e.Equals(pre))
        //            dest.Add(post);
        //        else
        //            dest.Add(e);
        //    return dest;
        //}
        public static List<T> RemplacerSi<T>(List<T> src, Func<T, bool> pred, T post) =>
            Transformer(src, e => pred(e) ? post : e);
        //{
        //    List<T> dest = new();
        //    foreach (T e in src)
        //        dest.Add(pred(e) ? post : e);
        //if (pred(e))
        //    dest.Add(post);
        //else
        //    dest.Add(e);
        //    return dest;
        //}
        public static bool EstLettre(char c) =>
            EstEntreInclusif(char.ToUpper(c), 'A', 'Z');
        public static bool EstEntreInclusif(int val, int min, int max) =>
            min <= val && val <= max;
        public static bool EstDans(char c, char[] tab)
        {
            foreach (char ch in tab)
                if (ch == c)
                    return true;
            return false;
        }
        public static bool EstVoyelle(char c)
        {
            char[] voyelles = { 'A', 'E', 'Y', 'O', 'E', 'I', 'Y' };
            return EstDans(char.ToUpper(c), voyelles);
        }
        public static bool ContientSeulementConsonnes(string s)
        {
            foreach (char c in s)
            {
                if (!EstLettre(c) || EstVoyelle(c))
                    return false;
            }
            return true;
        }
        //source: https://fr.wikipedia.org/wiki/Algorithme_d%27Euclide
        public static int PGCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
        public static void PlaySoundEffect(string fileName)
        {
            if(OperatingSystem.IsWindows())
            {
                SoundPlayer sp = new(fileName);
                sp.Play();
            }
        }
    }
}
