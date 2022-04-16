using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIUnitTests
{
    public interface ISampleService : ITestableBase
    {
        string Name { get; }
    }
}
