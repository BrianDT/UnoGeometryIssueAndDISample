// <copyright file="Bootstrapper.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019 All rights reserved</copyright>

namespace GeometrySample
{
    using System;
    using System.Collections.Generic;
    using System.Text;
#if __DROID__
    using Lamar;
    using LamarDiFacade;
#elif __WPF__
    using Framework.SMDIFacade;
    using StructureMap;
#elif __IOS__
    using Framework.TinyDIFacade;
#elif __WASM__
    using SimpleInjector;
    using SimInjDIFacade;
#elif WINDOWS_UWP
    using DryIoc;
    using DryIocDIFacade;
#elif NET7_0
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using MSExtFacade;
#else
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
#elif __WASM__
            // Create a new Simple Injector container
            var container = new Container();

            // Configure the container (register)
            container.Register<IDependencyResolver>(() => new SimpInjDI(container), Lifestyle.Singleton);
            container.Register<IDispatchOnUIThread, UIDispatcher>(Lifestyle.Singleton);
            container.Register<IMainViewModel, MainViewModel>();

            // Verify your configuration
            container.Verify();

            var diFacade = container.GetInstance<IDependencyResolver>();
#elif WINDOWS_UWP
            var container = new Container();
            container.RegisterInstance<IDependencyResolver>(new DryIocDI(container));

            // Framework
            container.Register<IDispatchOnUIThread, UIDispatcher>(Reuse.Singleton);

            // View models
            container.Register<IMainViewModel, MainViewModel>();

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
#elif NET7_0
            var services = new ServiceCollection();
            services.AddSingleton<IDependencyResolver, MSExtDI>()
                    .AddSingleton<IDispatchOnUIThread, UIDispatcher>()
                    .AddTransient<IMainViewModel, MainViewModel>();

            var serviceProvider = services.BuildServiceProvider();
            var diFacade = serviceProvider.GetRequiredService<IDependencyResolver>();
            ((MSExtDI)diFacade).Configure(serviceProvider);
#else
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance<IUnityContainer>(container, new ContainerControlledLifetimeManager());
            container.RegisterType<IDependencyResolver, UnityDI>(new ContainerControlledLifetimeManager());

            // Framework
            container.RegisterType<IDispatchOnUIThread, UIDispatcher>(new ContainerControlledLifetimeManager());

            // View models
            container.RegisterType<IMainViewModel, MainViewModel>();

            var diFacade = container.Resolve<IDependencyResolver>();

#endif
            DependencyHelper.Container = diFacade;

            // ensure the singleton dispatcher is created.
            diFacade.Resolve<IDispatchOnUIThread>();

            return diFacade;
        }
    }
}
