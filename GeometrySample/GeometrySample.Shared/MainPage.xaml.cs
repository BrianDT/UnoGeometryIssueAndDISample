// <copyright file="MainPage.xaml.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019 All rights reserved</copyright>

namespace GeometrySample
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using Vssl.Samples.Framework;
    using Vssl.Samples.FrameworkInterfaces;
    using Vssl.Samples.ViewModelInterfaces;
    using Windows.Foundation;
    using Windows.Foundation.Collections;
#if WINDOWS_UWP
    using Windows.UI.Xaml.Controls;
#else
    using Microsoft.UI.Xaml.Controls;
#endif

    // The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// The dispatcher facade
        /// </summary>
        private IDispatchOnUIThread dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            this.dispatcher = DependencyHelper.Resolve<IDispatchOnUIThread>();
            this.dispatcher.Initialize();
            DispatchHelper.Initialise(this.dispatcher);

            this.DataContext = DependencyHelper.Resolve<IMainViewModel>();

            this.drawarea.SizeChanged += (s, e) =>
            {
                this.VM.SizeChanged(e.NewSize.Width, e.NewSize.Height);
            };
            this.Loaded += (s, e) =>
            {
                this.VM.OnViewLoaded();
            };
        }

        /// <summary>
        /// Gets the data context cast as the view model interface
        /// </summary>
        public IMainViewModel VM => this.DataContext as IMainViewModel;
    }
}
