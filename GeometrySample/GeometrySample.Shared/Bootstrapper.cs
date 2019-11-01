// <copyright file="Bootstrapper.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019 All rights reserved</copyright>

namespace GeometrySample.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Text;
#if __DROID__
    using Lamar;
    using LamarDiFacade;
#endif
#if __WPF__
    using Framework.SMDIFacade;
    using StructureMap;
#endif
#if __IOS__
    using Framework.TinyDIFacade;
#endif
#if __WASM__ || NETFX_CORE
    using Unity;
    using Unity.Lifetime;
    using UnityDIFacade;
#endif

    using Vssl.Samples.Framework;
    using Vssl.Samples.FrameworkInterfaces;
    using Vssl.Samples.ViewModelInterfaces;
    using Vssl.Samples.ViewModels;

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
#if __DROID__
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
#elif __WASM__ || NETFX_CORE
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance<IUnityContainer>(container, new ContainerControlledLifetimeManager());
            container.RegisterType<IDependencyResolver, UnityDI>(new ContainerControlledLifetimeManager());

            // Framework
            container.RegisterType<IDispatchOnUIThread, UIDispatcher>(new ContainerControlledLifetimeManager());

            // View models
            container.RegisterType<IMainViewModel, MainViewModel>();

            var diFacade = container.Resolve<IDependencyResolver>();
#elif __IOS__
            var container = new TinyIoCDependencyService();
            // DI Facade
            container.RegisterSingleton<IDependencyResolver>(container);

            // Framework
            container.RegisterSingleton<IDispatchOnUIThread, UIDispatcher>();

            // View models
            container.Register<IMainViewModel, MainViewModel>();

            var diFacade = container.Resolve<IDependencyResolver>();
#elif __WPF__
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

#endif
            DependencyHelper.Container = diFacade;

            // ensure the singleton dispatcher is created.
            diFacade.Resolve<IDispatchOnUIThread>();

            return diFacade;
        }
    }
}
