// <copyright file="UnityContainer.cs" company="Visual Software Systems Ltd.">Copyright (c) 2024 All rights reserved</copyright>
namespace GeometrySample.Initialisation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Lifetime;
using Unity;
using UnityDIFacade;
using Vssl.Samples.FrameworkInterfaces;
using Vssl.Samples.ViewModelInterfaces;
using Vssl.Samples.ViewModels;
using Vssl.Samples.Framework;

/// <summary>
/// Dependepcy injection manager
/// </summary>
public class UnityContainer
{
    /// <summary>
    /// returns the container
    /// </summary>
    /// <returns>This class as a container</returns>
    public IDependencyResolver PopulateContainer()
    {
        IUnityContainer container = new Unity.UnityContainer();
        container.RegisterInstance<IUnityContainer>(container, new ContainerControlledLifetimeManager());
        container.RegisterType<IDependencyResolver, UnityDI>(new ContainerControlledLifetimeManager());

        // Framework
        container.RegisterType<IDispatchOnUIThread, UIDispatcher>(new ContainerControlledLifetimeManager());

        // View models
        container.RegisterType<IMainViewModel, MainViewModel>();

        var diFacade = container.Resolve<IDependencyResolver>();
        return diFacade;
    }
}
