using System;
using System.Collections.Generic;
using System.Linq;

namespace BloomFilterExample
{
    class Program
    {
        static void Main(string[] args)
        {
            const int amountOfItems = 1_000_000;
            const double desiredErrorRate = 0.001; // 1%
         
            var bloomFilter = new BloomFilter<string>(desiredErrorRate, amountOfItems);
            var hashSet = new HashSet<string>(amountOfItems);
            var rnd = new Random(42);

            var words = GetWords(amountOfItems, rnd, 65);
            foreach (var w in words)
            {
                bloomFilter.Add(w);
                hashSet.Add(w);
            }

            var newWords = GetWords(amountOfItems, rnd, 32);
            int errorCount = newWords
                .Count(w => bloomFilter.Contains(w));


            Console.WriteLine($"Data size in bytes: {GetUsedMemory(words):##,###}");
            Console.WriteLine($"Filter size in bytes: {amountOfItems * -1.44*Math.Log(desiredErrorRate, 2) / 8:##,###}");
            Console.WriteLine($"Rate of errors: {(double)errorCount/ amountOfItems:P}");
            Console.WriteLine($"Fill rate (% of ones): {bloomFilter.FillRate:P}");

            Console.ReadLine();
        }

        private static string[] GetWords(int count, Random rnd, int startCharcode)
        {
            return Enumerable.Range(1, count)
                .Select(_ =>
                    new string(
                        Enumerable.Range(1, 5 + rnd.Next(10))
                            .Select(__ => (char) (startCharcode + rnd.Next(26)))
                            .ToArray())

                )
                .ToArray();
        }

        private static int GetUsedMemory(IEnumerable<string> collection)
        {
            return collection.Sum(s => 26 + s.Length * 2);
        }
    }
}
