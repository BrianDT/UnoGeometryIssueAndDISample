// <copyright file="DispatchHelper.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019 All rights reserved</copyright>

namespace Vssl.Samples.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Vssl.Samples.FrameworkInterfaces;

    /// <summary>
    /// A helper class for solutions that do not need full dependency injection
    /// </summary>
    public static class DispatchHelper
    {
        /// <summary>
        /// Gets the dispatcher facade
        /// </summary>
        public static IDispatchOnUIThread Dispatcher { get; private set; }

        /// <summary>
        /// Initialise the helper with the interface to the helper facade
        /// </summary>
        /// <param name="dispatcher">The dispatcher facade</param>
        public static void Initialise(IDispatchOnUIThread dispatcher)
        {
            DispatchHelper.Dispatcher = dispatcher;
        }
    }
}
