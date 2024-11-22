// <copyright file="TinyIoCContainer.cs" company="Visual Software Systems Ltd.">Copyright (c) 2024 All rights reserved</copyright>
namespace GeometrySample.Initialisation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.TinyDIFacade;
using Vssl.Samples.Framework;
using Vssl.Samples.FrameworkInterfaces;
using Vssl.Samples.ViewModelInterfaces;
using Vssl.Samples.ViewModels;

/// <summary>
/// Dependepcy injection manager
/// </summary>
public class TinyIoCContainer
{
    /// <summary>
    /// returns the container
    /// </summary>
    /// <returns>This class as a container</returns>
    public IDependencyResolver PopulateContainer()
    {
        var container = new TinyIoCDependencyService();
        // DI Facade
        container.RegisterSingleton<IDependencyResolver>(container);

        // Framework
        container.RegisterSingleton<IDispatchOnUIThread, UIDispatcher>();

        // View models
        container.Register<IMainViewModel, MainViewModel>();

        var diFacade = container.Resolve<IDependencyResolver>();
        return diFacade;
    }
}
