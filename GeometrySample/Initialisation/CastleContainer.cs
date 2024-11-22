// <copyright file="CastleContainer.cs" company="Visual Software Systems Ltd.">Copyright (c) 2024 All rights reserved</copyright>
namespace GeometrySample.Initialisation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CastleWindsorDiFacade;
using Vssl.Samples.Framework;
using Vssl.Samples.FrameworkInterfaces;
using Vssl.Samples.ViewModelInterfaces;
using Vssl.Samples.ViewModels;

/// <summary>
/// Dependepcy injection manager
/// </summary>
public class CastleContainer
{
    /// <summary>
    /// returns the container
    /// </summary>
    /// <returns>This class as a container</returns>
    public IDependencyResolver PopulateContainer()
    {
        // Create a new Castle Windsor container
        var container = new WindsorContainer();

        // Configure the container (register)
        container.Register(Component.For<IDependencyResolver>().Instance(new CastleWindsorDi(container)));
        container.Register(Component.For<IDispatchOnUIThread>().ImplementedBy<UIDispatcher>());
        container.Register(Component.For<IMainViewModel>().ImplementedBy<MainViewModel>().LifeStyle.Transient);

        var diFacade = container.Resolve<IDependencyResolver>();
        return diFacade;
    }
}
