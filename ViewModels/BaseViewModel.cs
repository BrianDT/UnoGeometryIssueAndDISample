// <copyright file="BaseViewModel.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019 All rights reserved</copyright>

namespace Vssl.Samples.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Text;
    using FrameworkInterfaces;
    using ViewModelInterfaces;
    using Vssl.Samples.Framework;

    /// <summary>
    /// A base class for all view models
    /// </summary>
    public abstract class BaseViewModel : IBaseViewModel
    {
        /// <summary>
        /// The dispatcher used to marshal onto the UI thread
        /// </summary>
        private IDispatchOnUIThread dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel" /> class.
        /// </summary>
        public BaseViewModel()
        {
            this.dispatcher = DispatchHelper.Dispatcher;
        }

        #region [ INotifyPropertyChanged Events ]

        /// <summary>
        ///  Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Called when a property value has changed
        /// </summary>
        /// <param name="propertyName">The name of the property that changed</param>
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (this.dispatcher != null)
            {
                this.dispatcher.Invoke(() =>
                {
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                });
            }
            else
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
