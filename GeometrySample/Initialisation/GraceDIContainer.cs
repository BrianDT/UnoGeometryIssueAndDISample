// <copyright file="GraceDIContainer.cs" company="Visual Software Systems Ltd.">Copyright (c) 2024 All rights reserved</copyright>
namespace GeometrySample.Initialisation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grace.DependencyInjection;
////using Grace.DependencyInjection.Lifestyle;
using GraceDIFacade;
using Vssl.Samples.Framework;
using Vssl.Samples.FrameworkInterfaces;
using Vssl.Samples.ViewModelInterfaces;
using Vssl.Samples.ViewModels;

/// <summary>
/// Dependepcy injection manager
/// </summary>
public class GraceDIContainer
{
    /// <summary>
    /// returns the container
    /// </summary>
    /// <returns>This class as a container</returns>
    public IDependencyResolver PopulateContainer()
    {
        var container = new DependencyInjectionContainer();
        container.Configure(c =>
        {
            c.ExportInstance(new GraceDI(container)).As<IDependencyResolver>();
            c.Export<UIDispatcher>().As<IDispatchOnUIThread>().Lifestyle.Singleton();
            c.Export<MainViewModel>().As<IMainViewModel>();
        });

        var diFacade = container.Locate<IDependencyResolver>();
        return diFacade;
    }
}
