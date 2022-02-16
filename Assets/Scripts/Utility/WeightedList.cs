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
        [OdinSerialize]
        public List<Entry> entries = new List<Entry>();

        private bool _accumalatedWeightSet;

        private double accumulatedWeight = 0;

        private Random rand = new Random();

        [Serializable]
        public class Entry
        {
            public double accumulatedWeight;
            public T item;
        }

        public void AddEntry(T item, double weight)
        {
            accumulatedWeight += weight;
            entries.Add(new Entry { item = item, accumulatedWeight = accumulatedWeight });
        }

        public bool Equals(T other)
        {
            throw new NotImplementedException();
        }

        public T GetRandom()
        {
            if (!_accumalatedWeightSet)
            {
                accumulatedWeight = entries.Sum(x => x.accumulatedWeight);
            }

            double r = rand.NextDouble() * accumulatedWeight;
            var f = entries.First(i => (r -= i.accumulatedWeight) < 0).item;

            return f;
        }
    }
}