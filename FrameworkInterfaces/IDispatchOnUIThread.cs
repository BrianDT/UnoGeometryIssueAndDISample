// <copyright file="IDispatchOnUIThread.cs" company="Visual Software Systems Ltd.">Copyright (c) 2013, 2019 All rights reserved</copyright>

namespace Vssl.Samples.FrameworkInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The interface to the UI Dispatcher facade.
    /// </summary>
    public interface IDispatchOnUIThread
    {
        /// <summary>
        /// Initialise the dispatcher.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Check the dispatcher is initialised and if not initialise it.
        /// </summary>
        void CheckDispatcher();

        /// <summary>
        /// Execute an action via the dispatcher.
        /// </summary>
        /// <param name="action">The action.</param>
        void Invoke(Action action);

        /// <summary>
        /// Async Execution of an action via the dispatcher.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>An awaitable task.</returns>
        Task InvokeAsync(Action action);
    }
}
