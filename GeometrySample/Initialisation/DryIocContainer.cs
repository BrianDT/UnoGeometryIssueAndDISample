// <copyright file="DryIocContainer.cs" company="Visual Software Systems Ltd.">Copyright (c) 2024 All rights reserved</copyright>
namespace GeometrySample.Initialisation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;
using DryIocDIFacade;
using ServiceInterfaces;
using Services;
using Vssl.Samples.Framework;
using Vssl.Samples.FrameworkInterfaces;
using Vssl.Samples.ViewModelInterfaces;
using Vssl.Samples.ViewModels;

/// <summary>
/// Dependepcy injection manager
/// </summary>
public class DryIocContainer
{
    /// <summary>
    /// returns the container
    /// </summary>
    /// <returns>This class as a container</returns>
    public IDependencyResolver PopulateContainer()
    {
#if __IOS__
        var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient().WithUseInterpretation());
#else
        var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());
#endif
        container.RegisterInstance<IDependencyResolver>(new DryIocDI(container));

        // Framework
        container.Register<IDispatchOnUIThread, UIDispatcher>(Reuse.Singleton);

        // Services
        container.Register<ISampleService, SampleService>(Reuse.Singleton);

        // View models
        container.Register<IMainViewModel, MainViewModel>();

        var diFacade = container.Resolve<IDependencyResolver>();
        return diFacade;
    }
}
