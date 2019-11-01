// <copyright file="IMainViewModel.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019 All rights reserved</copyright>

namespace Vssl.Samples.ViewModelInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Vssl.Samples.FrameworkInterfaces;

    /// <summary>
    /// The view model for the geometry sample
    /// </summary>
    public interface IMainViewModel : IBaseViewModel
    {
        /// <summary>
        /// Gets the command thaqt starts the annimation
        /// </summary>
        ICommandEx StartCommand { get; }

        /// <summary>
        /// Gets the command thaqt stops the annimation
        /// </summary>
        ICommandEx StopCommand { get; }

        /// <summary>
        /// Gets the height of the moving rectangle
        /// </summary>
        double Height { get; }

        /// <summary>
        /// Gets the width of the moving rectangle
        /// </summary>
        double Width { get; }

        /// <summary>
        /// Gets the X co-ordainate of the top left of moving rectangle
        /// </summary>
        double X { get; }

        /// <summary>
        /// Gets the Y co-ordainate of the top left of moving rectangle
        /// </summary>
        double Y { get; }

        /// <summary>
        /// Gets the P1 co-ordainate of the Bezier Segment
        /// </summary>
        ICoordinates P1 { get; }

        /// <summary>
        /// Gets the CP1 co-ordainate of the Bezier Segment
        /// </summary>
        ICoordinates CP1 { get; }

        /// <summary>
        /// Gets the P2 co-ordainate of the Bezier Segment
        /// </summary>
        ICoordinates P2 { get; }

        /// <summary>
        /// Gets the CP2 co-ordainate of the Bezier Segment
        /// </summary>
        ICoordinates CP2 { get; }

        /// <summary>
        /// Called when the view is loaded
        /// </summary>
        void OnViewLoaded();

        /// <summary>
        /// Called when the window changes size
        /// </summary>
        /// <param name="width">The new window height</param>
        /// <param name="height">The new window width</param>
        void SizeChanged(double width, double height);
    }
}
