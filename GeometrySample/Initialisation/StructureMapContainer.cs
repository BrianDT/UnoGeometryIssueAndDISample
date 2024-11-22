// <copyright file="StructureMapContainer.cs" company="Visual Software Systems Ltd.">Copyright (c) 2024 All rights reserved</copyright>
namespace GeometrySample.Initialisation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.SMDIFacade;
using StructureMap;
using Vssl.Samples.Framework;
using Vssl.Samples.FrameworkInterfaces;
using Vssl.Samples.ViewModelInterfaces;
using Vssl.Samples.ViewModels;

/// <summary>
/// Dependepcy injection manager
/// </summary>
public class StructureMapContainer
{
    /// <summary>
    /// returns the container
    /// </summary>
    /// <returns>This class as a container</returns>
    public IDependencyResolver PopulateContainer()
    {
        var container = new Container();
        container.Configure(_ =>
        {
            // DI Facade
            _.For<IDependencyResolver>().Singleton().Use(() => new StructureMapDI(container));

            // Framework
            _.For<IDispatchOnUIThread>().Singleton().Use<UIDispatcher>();

            // View models
            _.For<IMainViewModel>().Use<MainViewModel>();
        });

        var diFacade = container.GetInstance<IDependencyResolver>();
        return diFacade;
    }
}
