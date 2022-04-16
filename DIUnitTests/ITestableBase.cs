using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIUnitTests
{
    public interface ITestableBase
    {
        int Count(Guid testKey);
    }
}
