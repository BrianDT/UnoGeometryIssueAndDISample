﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIUnitTests
{
    public class SampleViewModel : TestableBase, ISampleViewModel
    {
        public SampleViewModel(Guid testKey) : base(testKey)
        {
        }
    }
}
