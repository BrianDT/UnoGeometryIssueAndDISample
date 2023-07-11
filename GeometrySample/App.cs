// <copyright file="App.cs" company="Visual Software Systems Ltd.">Copyright (c) 2023 All rights reserved</copyright>

namespace GeometrySample
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using Vssl.Samples.FrameworkInterfaces;

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public class App : Application
    {
        private static Window window;

        /// <summary>
        /// The dependency injection container
        /// </summary>
        private IDependencyResolver diFacade;

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
#if NET7_0_OR_GREATER && WINDOWS && !HAS_UNO
            window = new Window();
#else
            window = Microsoft.UI.Xaml.Window.Current;
#endif
            var bootstrapper = new Bootstrapper();

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (window.Content is not Frame rootFrame)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // Place the frame in the current Window
                window.Content = rootFrame;

                rootFrame.NavigationFailed += this.OnNavigationFailed;
            }

            if (rootFrame.Content == null)
            {
                this.diFacade = bootstrapper.Startup();

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), args.Arguments);
            }

            // Ensure the current window is active
            window.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new InvalidOperationException($"Failed to load {e.SourcePageType.FullName}: {e.Exception}");
        }
    }
}