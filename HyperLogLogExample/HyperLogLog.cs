using System;
using System.Linq;

namespace HyperLogLogExample
{
    class HyperLogLog<T>
    {
        private readonly int _bucketSizeInBits;
        private readonly byte[] _buckets;
        private readonly int _bucketCount;

        public HyperLogLog(int bucketSizeInBits)
        {
            _bucketSizeInBits = bucketSizeInBits;
            _bucketCount = (int)Math.Pow(2, bucketSizeInBits);
            _buckets = new byte[_bucketCount];
        }

        public int ApproxCount
        {
            get
            {
                var sum = _buckets.Sum(n => Math.Pow(2, -n));
                var z = Math.Pow(sum, -1);
                var a = 0.7213 / (1 + 1.079 / _bucketCount); 
                var e = a * _bucketCount * _bucketCount * z;

                if (e < 2.5 * _bucketCount)
                {
                    var v = _buckets.Count(r => r == 0);
                    if (v == 0)
                        return (int)e;
                    else
                        return (int)(_bucketCount * Math.Log((double)_bucketCount / v));
                }

                return (int)e;
            }
        }

        public void Add(T item)
        {
            var hashCode = (uint)item.GetHashCode();
            var bucket = hashCode & (_bucketCount - 1);

            var zeroes = PositionOfLeastSignificantBit(hashCode);

            _buckets[bucket] = Math.Max(zeroes, _buckets[bucket]);
        }

        private byte PositionOfLeastSignificantBit(uint hash)
        {
            var value = hash >> _bucketSizeInBits;

            var shifted = 0;
            byte count = 1;
            while ((value & 1) == 0 && shifted < 32)
            {
                value >>= 1;
                count++;
                shifted++;
            }
            return count;
        }
    }
}