using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIUnitTests
{
    public class SampleService : TestableBase, ISampleService
    {
        public SampleService(Guid testKey) : base(testKey)
        {
            this.Name = "Fred";
        }

        public string Name { get; private set; }
    }
}
