using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIUnitTests
{
    public abstract class TestableBase : ITestableBase
    {
        private static Dictionary<Type, Dictionary<Guid, int>> instanceCounts = new Dictionary<Type, Dictionary<Guid, int>>();

        public TestableBase(Guid testKey)
        {
            var type = this.GetType();
            Dictionary<Guid, int> countsForType;
            if (!instanceCounts.TryGetValue(type, out countsForType))
            {
                countsForType = new Dictionary<Guid, int>();
                instanceCounts.Add(type, countsForType);
            }

            countsForType[testKey]++;
        }

        public int Count(Guid testKey)
        {
            var type = this.GetType();
            Dictionary<Guid, int> countsForType;
            if (!instanceCounts.TryGetValue(type, out countsForType))
            {
                return 0;
            }

            return countsForType[testKey];
        }

        public static void InitCount(Guid testKey, Type type)
        {
            Dictionary<Guid, int> countsForType;
            if (!instanceCounts.TryGetValue(type, out countsForType))
            {
                countsForType = new Dictionary<Guid, int>();
                instanceCounts.Add(type, countsForType);
            }

            if (countsForType.ContainsKey(testKey))
            {
                countsForType[testKey] = 0;
            }
            else
            {
                countsForType.Add(testKey, 0);
            }
        }
    }
}
