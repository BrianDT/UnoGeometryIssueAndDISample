// <copyright file="Coordinates.cs" company="Visual Software Systems Ltd.">Copyright (c) 2014 All rights reserved</copyright>

namespace Vssl.Samples.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Vssl.Samples.ViewModelInterfaces;

    /// <summary>
    /// A portable co-ordinate class
    /// </summary>
    public struct Coordinates : ICoordinates
    {
        /// <summary>
        /// The x co-ordinate
        /// </summary>
        private double x;

        /// <summary>
        /// The y co-ordinate
        /// </summary>
        private double y;

        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinates"/> struct
        /// </summary>
        /// <param name="x">The x co-ordinate</param>
        /// <param name="y">The y co-ordinate</param>
        public Coordinates(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Gets or sets the X co-ordinate
        /// </summary>
        public double X
        {
            get
            {
                return this.x;
            }

            set
            {
                if (value != this.x)
                {
                    this.x = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Y co-ordinate
        /// </summary>
        public double Y
        {
            get
            {
                return this.y;
            }

            set
            {
                if (value != this.y)
                {
                    this.y = value;
                }
            }
        }

        /// <summary>
        /// Checks two co-ordinates to see if they are equal
        /// </summary>
        /// <param name="coord1">The first co-ordinate</param>
        /// <param name="coord2">The second co-ordinate</param>
        /// <returns>True if equal</returns>
        public static bool AreEqual(ICoordinates coord1, ICoordinates coord2)
        {
            return (coord1 == null && coord2 == null) || (coord1 != null && coord2 != null && coord1.Equals(coord2));
        }

        /// <summary>
        /// Returns a representation of a co-ordinate that can be used in a path
        /// </summary>
        /// <returns>The co-ordinates as a string</returns>
        public override string ToString()
        {
            return this.x.ToString() + "," + this.y.ToString();
        }

        /// <summary>
        /// Checks for equality with another instance
        /// </summary>
        /// <param name="obj">An object to compare</param>
        /// <returns>True if equal</returns>
        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            ICoordinates other = obj as ICoordinates;
            if (other == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.X == other.X && this.Y == other.Y)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks for equality with another instance
        /// </summary>
        /// <param name="other">The attribute set to compare</param>
        /// <returns>True if equal</returns>
        public bool Equals(ICoordinates other)
        {
            // If parameter is null return false.
            if (other == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.X == other.X && this.Y == other.Y)
            {
                return true;
            }

            return false;
        }
    }
}
