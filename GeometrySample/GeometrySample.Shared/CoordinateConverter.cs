// <copyright file="CoordinateConverter.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019 All rights reserved</copyright>

namespace GeometrySample.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Vssl.Samples.ViewModelInterfaces;
    using Windows.Foundation;
#if WINDOWS_UWP
    using Windows.UI.Xaml.Data;
#else
    using Microsoft.UI.Xaml.Data;
#endif

    /// <summary>
    /// Converts from portable co-ordinates to Framework points
    /// </summary>
    public class CoordinateConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to style
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="targetType">The target type of the conversion</param>
        /// <param name="parameter">Any optional parameter</param>
        /// <param name="language">The language</param>
        /// <returns>The converted value</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(Point))
            {
                return null;
            }

            if (value == null)
            {
                return new Point(0.0, 0.0);
            }

            ICoordinates inputValue = value as ICoordinates;
            if (inputValue == null)
            {
                return new Point(0.0, 0.0);
            }

            return new Point(inputValue.X, inputValue.Y);
        }

        /// <summary>
        /// Converts style to enumeration
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="targetType">The target type of the conversion</param>
        /// <param name="parameter">Any optional parameter</param>
        /// <param name="language">The language</param>
        /// <returns>The converted value</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
