// <copyright file="ICoordinates.cs" company="Visual Software Systems Ltd.">Copyright (c) 2014 All rights reserved</copyright>

namespace Vssl.Samples.ViewModelInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A portable co-ordinate interface
    /// </summary>
    public interface ICoordinates
    {
        /// <summary>
        /// Gets or sets the X co-ordinate
        /// </summary>
        double X
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Y co-ordinate
        /// </summary>
        double Y
        {
            get;
            set;
        }
    }
}
