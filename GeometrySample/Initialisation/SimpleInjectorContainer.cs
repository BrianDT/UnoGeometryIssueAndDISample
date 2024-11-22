// <copyright file="SimpleInjectorContainer.cs" company="Visual Software Systems Ltd.">Copyright (c) 2024 All rights reserved</copyright>
namespace GeometrySample.Initialisation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using SimInjDIFacade;
using Vssl.Samples.Framework;
using Vssl.Samples.FrameworkInterfaces;
using Vssl.Samples.ViewModelInterfaces;
using Vssl.Samples.ViewModels;

/// <summary>
/// Dependepcy injection manager
/// </summary>
public class SimpleInjectorContainer
{
    /// <summary>
    /// returns the container
    /// </summary>
    /// <returns>This class as a container</returns>
    public IDependencyResolver PopulateContainer()
    {
        // Create a new Simple Injector container
        var container = new Container();

        // Configure the container (register)
        container.Register<IDependencyResolver>(() => new SimpInjDI(container), Lifestyle.Singleton);
        container.Register<IDispatchOnUIThread, UIDispatcher>(Lifestyle.Singleton);
        container.Register<IMainViewModel, MainViewModel>();

        // Verify your configuration
        container.Verify();

        var diFacade = container.GetInstance<IDependencyResolver>();
        return diFacade;
    }
}
