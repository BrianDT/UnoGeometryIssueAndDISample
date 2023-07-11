// <copyright file="Bootstrapper.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019 All rights reserved</copyright>

namespace GeometrySample
{
    using System;
    using System.Collections.Generic;
    ////using System.ComponentModel;
    using System.Text;
#if ANDROID
    using Lamar;
    using LamarDiFacade;
#elif __WPF__
    using Framework.SMDIFacade;
    using StructureMap;
#elif __IOS__
    using Framework.TinyDIFacade;
#elif __WASM__
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using MSExtFacade;
    ////using Autofac;
    ////using AutoFacDIFacade;
    ////using SB=Stashbox;
    ////using StashBoxDIFacade;
    ////using Castle.MicroKernel.Registration;
    ////using Castle.Windsor;
    ////using CastleWindsorDiFacade;
    ////using SimpleInjector;
    ////    using SimpleInjector;
    ////    using SimInjDIFacade;
#elif WINDOWS_UWP
    using DryIoc;
    using DryIocDIFacade;
#elif WINDOWS && NET7_0
    using Grace.DependencyInjection;
    using Grace.DependencyInjection.Lifestyle;
    using GraceDIFacade;
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
#if ANDROID
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
            var services = new ServiceCollection();
            services.AddSingleton<IDependencyResolver, MSExtDI>()
                    .AddSingleton<IDispatchOnUIThread, UIDispatcher>()
                    .AddTransient<IMainViewModel, MainViewModel>();

            var serviceProvider = services.BuildServiceProvider();
            var diFacade = serviceProvider.GetRequiredService<IDependencyResolver>();
            ((MSExtDI)diFacade).Configure(serviceProvider);

            /*
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
            */

            /*
            // Create a new Stash Box container
            var container = new SB.StashboxContainer();
            IDependencyResolver diFacade = new StashBoxDI(container);

            container.RegisterInstance<IDependencyResolver>(diFacade);
            container.RegisterSingleton<IDispatchOnUIThread, UIDispatcher>();
            container.Register<IMainViewModel, MainViewModel>();

            // Verify your configuration
            container.Validate();
            */
            /*
            // Create a new Castle Windsor container
            var container = new WindsorContainer();

            // Configure the container (register)
            container.Register(Component.For<IDependencyResolver>().Instance(new CastleWindsorDi(container)));
            container.Register(Component.For<IDispatchOnUIThread>().ImplementedBy<UIDispatcher>());
            container.Register(Component.For<IMainViewModel>().ImplementedBy<MainViewModel>().LifeStyle.Transient);

            var diFacade = container.Resolve<IDependencyResolver>();
            */
            /*
            // Create a new Simple Injector container
            var container = new Container();

            // Configure the container (register)
            container.Register<IDependencyResolver>(() => new SimpInjDI(container), Lifestyle.Singleton);
            container.Register<IDispatchOnUIThread, UIDispatcher>(Lifestyle.Singleton);
            container.Register<IMainViewModel, MainViewModel>();

            // Verify your configuration
            container.Verify();

            var diFacade = container.GetInstance<IDependencyResolver>();
            */
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
#elif WINDOWS && NET7_0
            var container = new DependencyInjectionContainer();
            container.Configure(c =>
            {
                c.ExportInstance(new GraceDI(container)).As<IDependencyResolver>();
                c.Export<UIDispatcher>().As<IDispatchOnUIThread>().Lifestyle.Singleton();
                c.Export<MainViewModel>().As<IMainViewModel>();
            });

            var diFacade = container.Locate<IDependencyResolver>();
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
