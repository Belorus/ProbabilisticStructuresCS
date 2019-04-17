using System;
using System.Collections.Generic;
using System.Linq;

namespace HyperLogLogExample
{
    class Program
    {
        static void Main(string[] args)
        {
            const int CountOfItems = 1_000_000;

            var hyperLogLog = new HyperLogLog<string>(14);
            var rnd = new Random(42);
            var set = new HashSet<string>(CountOfItems);

            for (int i = 0; i < CountOfItems; i++)
            {
                var str = (i + rnd.Next(100)).ToString();
                hyperLogLog.Add(str);
                set.Add(str);
            }

            Console.WriteLine($"{hyperLogLog.ApproxCount:##,###} / {set.Count:##,###}"); 
            Console.WriteLine($"Error: {(hyperLogLog.ApproxCount - set.Count) / (double)set.Count:P}");
        }
    }
}
