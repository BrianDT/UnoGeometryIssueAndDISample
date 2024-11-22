// <copyright file="Bootstrapper.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019 All rights reserved</copyright>
#define USEMSIOC
namespace GeometrySample.Initialisation;

using System;
using System.Collections.Generic;
using System.Text;

using Vssl.Samples.Framework;
using Vssl.Samples.FrameworkInterfaces;

/// <summary>
/// Bootstraps the DI
/// </summary>
public class Bootstrapper
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
    /// </summary>
    public Bootstrapper()
    {
    }

    /// <summary>
    /// Create the DI container and register all classes against their interfaces
    /// </summary>
    /// <returns>The interface to the DI facade</returns>
    public IDependencyResolver Startup()
    {
        IDependencyResolver diFacade = null;
#if USELAMAR
        var containerCreator = new LamarContainer();
        diFacade = containerCreator.PopulateContainer();
#endif
#if USEMSIOC
        var containerCreator = new MSServiceContainer();
        diFacade = containerCreator.PopulateContainer();
#endif
#if USEAUTOFAC
        var containerCreator = new AutoFacContainer();
        diFacade = containerCreator.PopulateContainer();
#endif
#if USESTASHBOX
        var containerCreator = new StashboxContainer();
        diFacade = containerCreator.PopulateContainer();
#endif
#if USECASTLE
        var containerCreator = new CastleContainer();
        diFacade = containerCreator.PopulateContainer();
#endif
#if USESIMPLEDI
        var containerCreator = new SimpleInjectorContainer();
        diFacade = containerCreator.PopulateContainer();
#endif
#if USEDRYIOC
        var containerCreator = new DryIocContainer();
        diFacade = containerCreator.PopulateContainer();
#endif
#if USETINY
        var containerCreator = new TinyIoCContainer();
        diFacade = containerCreator.PopulateContainer();
#endif
#if USESTRUCTUREMAP
        var containerCreator = new StructureMapContainer();
        diFacade = containerCreator.PopulateContainer();
#endif
#if USEGRACE
        var containerCreator = new GraceDIContainer();
        diFacade = containerCreator.PopulateContainer();
#endif
#if USEUNITY
        var containerCreator = new UnityContainer();
        diFacade = containerCreator.PopulateContainer();
#endif
        DependencyHelper.Container = diFacade;

        // ensure the singleton dispatcher is created.
        diFacade.Resolve<IDispatchOnUIThread>();

        return diFacade;
    }
}
