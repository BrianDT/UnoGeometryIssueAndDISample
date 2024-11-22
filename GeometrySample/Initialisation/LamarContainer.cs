// <copyright file="LamarContainer.cs" company="Visual Software Systems Ltd.">Copyright (c) 2024 All rights reserved</copyright>
namespace GeometrySample.Initialisation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lamar;
using LamarDiFacade;
using Vssl.Samples.Framework;
using Vssl.Samples.FrameworkInterfaces;
using Vssl.Samples.ViewModelInterfaces;
using Vssl.Samples.ViewModels;

/// <summary>
/// Dependepcy injection manager
/// </summary>
public class LamarContainer
{
    /// <summary>
    /// returns the container
    /// </summary>
    /// <returns>This class as a container</returns>
    public IDependencyResolver PopulateContainer()
    {
        // DI Facade
        LamarDI diFacade = new LamarDI();
        var registry = new ServiceRegistry();
        registry.For<IDependencyResolver>().Use(diFacade);

        // Framework
        registry.ForSingletonOf<IDispatchOnUIThread>().Use<UIDispatcher>();

        // View models
        registry.For<IMainViewModel>().Use<MainViewModel>();

        var container = new Container(registry);

        diFacade.Initialise(container);
        return diFacade;
    }
}
