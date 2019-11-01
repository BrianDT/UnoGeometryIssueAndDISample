// <copyright file="ICommandEx.cs" company="Visual Software Systems Ltd.">Copyright (c) 2013 All rights reserved</copyright>

namespace Vssl.Samples.FrameworkInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// An extended ICommand interface that allows async execution
    /// </summary>
    public interface ICommandEx : ICommand
    {
        /// <summary>
        /// Executes the command action asynchronously
        /// </summary>
        /// <param name="parameter">The optional command parameter</param>
        /// <returns>The task that can be awaited</returns>
        Task ExecuteAsync(object parameter);

        /// <summary>
        /// Forces the raising of the CanExecute Changed event
        /// </summary>
        void RaiseCanExecuteChanged();
    }
}
