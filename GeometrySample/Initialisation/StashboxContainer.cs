// <copyright file="StashboxContainer.cs" company="Visual Software Systems Ltd.">Copyright (c) 2024 All rights reserved</copyright>
namespace GeometrySample.Initialisation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StashBoxDIFacade;
using Vssl.Samples.Framework;
using Vssl.Samples.FrameworkInterfaces;
using Vssl.Samples.ViewModelInterfaces;
using Vssl.Samples.ViewModels;
using SB=Stashbox;

/// <summary>
/// Dependepcy injection manager
/// </summary>
public class StashboxContainer
{
    /// <summary>
    /// returns the container
    /// </summary>
    /// <returns>This class as a container</returns>
    public IDependencyResolver PopulateContainer()
    {
        // Create a new Stash Box container
        var container = new SB.StashboxContainer();
        IDependencyResolver diFacade = new StashBoxDI(container);

        container.RegisterInstance<IDependencyResolver>(diFacade);
        container.RegisterSingleton<IDispatchOnUIThread, UIDispatcher>();
        container.Register<IMainViewModel, MainViewModel>();

        // Verify your configuration
        container.Validate();
        return diFacade;
    }
}
