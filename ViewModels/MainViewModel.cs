// <copyright file="MainViewModel.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019 All rights reserved</copyright>

namespace Vssl.Samples.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ServiceInterfaces;
    using Vssl.Samples.Framework;
    using Vssl.Samples.FrameworkInterfaces;
    using Vssl.Samples.ViewModelInterfaces;

    /// <summary>
    /// The view model for the geometry sample
    /// </summary>
    [Bindable(bindable: true)]
    public class MainViewModel : BaseViewModel, IMainViewModel
    {
        #region [ Private Fields ]

        /// <summary>
        /// True is the view has been loaded
        /// </summary>
        private bool isLoaded;

        /// <summary>
        /// The window width
        /// </summary>
        private double pageWidth;

        /// <summary>
        /// The page height
        /// </summary>
        private double pageHeight;

        /// <summary>
        /// True if currently animating
        /// </summary>
        private bool annimating;

        /// <summary>
        /// True if the rectangle has been centered on its initial display
        /// </summary>
        private bool hasCentered;

        /// <summary>
        /// The animation x increment
        /// </summary>
        private int xInc = 1;

        /// <summary>
        /// The animation y increment
        /// </summary>
        private int yInc = 1;

        /// <summary>
        /// A DI injected singleton service
        /// </summary>
        private ISampleService sampleService;

        #endregion

        #region [ Constructor ]

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        /// <param name="sampleService">A DI injected singleton service</param>
        public MainViewModel(ISampleService sampleService)
            : base()
        {
            this.sampleService = sampleService;
            this.StartCommand = new DelegateCommandAsync(this.StartAnnimating, (p) => true);
            this.StopCommand = new DelegateCommandAsync(this.StopAnnimating, (p) => true);
            this.Height = 150.0D;
            this.Width = 150.0D;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the command thaqt starts the annimation
        /// </summary>
        public ICommandEx StartCommand { get; private set; }

        /// <summary>
        /// Gets the command thaqt stops the annimation
        /// </summary>
        public ICommandEx StopCommand { get; private set; }

        /// <summary>
        /// Gets the height of the moving rectangle
        /// </summary>
        public double Height { get; private set; }

        /// <summary>
        /// Gets the width of the moving rectangle
        /// </summary>
        public double Width { get; private set; }

        /// <summary>
        /// Gets the X co-ordainate of the top left of moving rectangle
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// Gets the Y co-ordainate of the top left of moving rectangle
        /// </summary>
        public double Y { get; private set; }

        /// <summary>
        /// Gets the P1 co-ordainate of the Bezier Segment
        /// </summary>
        public ICoordinates P1 { get; private set; }

        /// <summary>
        /// Gets the CP1 co-ordainate of the Bezier Segment
        /// </summary>
        public ICoordinates CP1 { get; private set; }

        /// <summary>
        /// Gets the P2 co-ordainate of the Bezier Segment
        /// </summary>
        public ICoordinates P2 { get; private set; }

        /// <summary>
        /// Gets the CP2 co-ordainate of the Bezier Segment
        /// </summary>
        public ICoordinates CP2 { get; private set; }

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Called when the view is loaded
        /// </summary>
        public void OnViewLoaded()
        {
            this.isLoaded = true;
            if (this.pageWidth != 0.0d && this.pageHeight != 0.0d)
            {
                this.SizeChanged(this.pageWidth, this.pageHeight);
            }

            this.OnPropertyChanged("Width");
            this.OnPropertyChanged("Height");
        }

        /// <summary>
        /// Called when the window changes size
        /// </summary>
        /// <param name="width">The new window height</param>
        /// <param name="height">The new window width</param>
        public void SizeChanged(double width, double height)
        {
            this.pageWidth = width;
            this.pageHeight = height;

            if (!this.isLoaded)
            {
                return;
            }

            var boxSize = this.pageWidth / 8.0;
            if (this.pageHeight / 8.0D < boxSize)
            {
                boxSize = this.pageHeight / 10.0D;
            }

            if (!this.hasCentered)
            {
                this.X = (this.pageWidth - boxSize) / 2.0D;
                this.Y = (this.pageHeight - boxSize) / 2.0D;
                this.OnPropertyChanged("X");
                this.OnPropertyChanged("Y");
                this.hasCentered = true;
            }

            this.Width = boxSize;
            this.Height = boxSize;
            this.P1 = new Coordinates(boxSize * 0.1, boxSize * 0.1);
            this.CP1 = new Coordinates(boxSize, boxSize * 0.1);
            this.P2 = new Coordinates(boxSize * 0.9, boxSize * 0.9);
            this.CP2 = new Coordinates(0, boxSize * 0.9);
            this.OnPropertyChanged("Width");
            this.OnPropertyChanged("Height");
            this.OnPropertyChanged("P1");
            this.OnPropertyChanged("CP1");
            this.OnPropertyChanged("P2");
            this.OnPropertyChanged("CP2");

            this.CheckInBounds();
        }

        #endregion

        #region [ Private Methods ]

        /// <summary>
        /// Check that the rectangle is still within the bounds of the window
        /// </summary>
        /// <param name="notifyPropertiesChanged">If true then property change notifications will be raised</param>
        private void CheckInBounds(bool notifyPropertiesChanged = false)
        {
            bool xChanged = false;
            bool yChanged = false;
            if (this.X < 0)
            {
                this.X = 0;
                this.xInc = 1;
                xChanged = true;
            }
            else if (this.X + this.Width > this.pageWidth)
            {
                this.X = this.pageWidth - this.Width;
                this.xInc = -1;
                xChanged = true;
            }

            if (this.Y < 0)
            {
                this.Y = 0;
                this.yInc = 1;
                yChanged = true;
            }
            else if (this.Y + this.Height > this.pageHeight)
            {
                this.Y = this.pageHeight - this.Height;
                this.yInc = -1;
                yChanged = true;
            }

            if (notifyPropertiesChanged || xChanged)
            {
                this.OnPropertyChanged("X");
            }

            if (notifyPropertiesChanged || yChanged)
            {
                this.OnPropertyChanged("Y");
            }
        }

        /// <summary>
        /// Start animating the location of the rectangle
        /// </summary>
        /// <param name="parameter">An optional parameter</param>
        /// <returns>An awaitable task</returns>
        private async Task StartAnnimating(object parameter)
        {
            this.annimating = true;
            var task = Task.Run(async () =>
            {
                await this.AnnimateAsync();
            });

            await Task.CompletedTask;
        }

        /// <summary>
        /// Stop animating the location of the rectangle
        /// </summary>
        /// <param name="parameter">An optional parameter</param>
        /// <returns>An awaitable task</returns>
        private async Task StopAnnimating(object parameter)
        {
            this.annimating = false;
            await Task.CompletedTask;
        }

        /// <summary>
        /// Continues animating the shape until flagged to stop
        /// </summary>
        /// <returns>An awaitable task</returns>
        private async Task AnnimateAsync()
        {
            while (this.annimating)
            {
                this.X += this.xInc;
                this.Y += this.yInc;
                this.CheckInBounds(notifyPropertiesChanged: true);

                await Task.Delay(TimeSpan.FromMilliseconds(5));
            }
        }

        #endregion
    }
}
