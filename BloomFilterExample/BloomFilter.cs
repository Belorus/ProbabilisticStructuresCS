using System;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;

namespace BloomFilterExample
{
    class BloomFilter<T>
    {
        private readonly int _numOfFunctions;
        private readonly BitArray _bitArray;

        public BloomFilter(double desiredErrorRate, int expectedNumberOfItems)
        {
            int bitsPerItem = (int)(-1.44 * Math.Log(desiredErrorRate, 2));
            int bitArraySize = bitsPerItem * expectedNumberOfItems;

            _numOfFunctions = (int)-Math.Log(desiredErrorRate, 2);
            _bitArray = new BitArray(bitArraySize, false);
        }

        public double FillRate => (double)_bitArray.Cast<bool>().Count(f => f) / _bitArray.Count;

        public void Add(T item)
        {
            var rnd = new Random(item.GetHashCode());
            for (int i = 0; i < _numOfFunctions; i++)
            {
                int bitIndex = rnd.Next(_bitArray.Length);
                _bitArray.Set(bitIndex, true);
            }
        }

        public bool Contains(T item)
        {
            var rnd = new Random(item.GetHashCode());
            for (int i = 0; i < _numOfFunctions; i++)
            {
                int bitIndex = rnd.Next(_bitArray.Length);
                if (!_bitArray.Get(bitIndex))
                    return false;
            }

            return true;
        }
    }
}