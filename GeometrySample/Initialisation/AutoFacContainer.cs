// <copyright file="AutoFacContainer.cs" company="Visual Software Systems Ltd.">Copyright (c) 2024 All rights reserved</copyright>
namespace GeometrySample.Initialisation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AutoFacDIFacade;
using Vssl.Samples.Framework;
using Vssl.Samples.FrameworkInterfaces;
using Vssl.Samples.ViewModelInterfaces;
using Vssl.Samples.ViewModels;

/// <summary>
/// Dependepcy injection manager
/// </summary>
public class AutoFacContainer
{
    /// <summary>
    /// returns the container
    /// </summary>
    /// <returns>This class as a container</returns>
    public IDependencyResolver PopulateContainer()
    {
        var builder = new ContainerBuilder();

        // Configure the container (register)
        builder.RegisterInstance(new AutoFacDI()).As<IDependencyResolver>();
        builder.RegisterType<UIDispatcher>().As<IDispatchOnUIThread>().SingleInstance();
        builder.RegisterType<MainViewModel>().As<IMainViewModel>();

        var container = builder.Build();

        // define a global scope
        var scope = container.BeginLifetimeScope();

        var diFacade = scope.Resolve<IDependencyResolver>();
        ((AutoFacDI)diFacade).SetGlobalScope(scope);

        return diFacade;
    }
}
