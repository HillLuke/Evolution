using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace Assets.Scripts.Utility
{
    [Serializable]
    public class WeightedList<T>
    {
        [Serializable]
        public class Entry
        {
            public double accumulatedWeight;
            public T item;
        }

        [OdinSerialize]
        public List<Entry> entries = new List<Entry>();
        private double accumulatedWeight;
        private Random rand = new Random();

        public void Init()
        {
            accumulatedWeight = entries.Sum(x => x.accumulatedWeight);
        }

        public void AddEntry(T item, double weight)
        {
            accumulatedWeight += weight;
            entries.Add(new Entry { item = item, accumulatedWeight = accumulatedWeight });
        }

        public T GetRandom()
        {
            double r = rand.NextDouble() * accumulatedWeight;
            var f = entries.First(i => (r -= i.accumulatedWeight) < 0).item;

            return f;
        }

        public bool Equals(T other)
        {
            throw new NotImplementedException();
        }
    }
}


