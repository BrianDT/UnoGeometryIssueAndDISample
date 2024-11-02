// <copyright file="GraceDI.cs" company="Visual Software Systems Ltd.">Copyright (c) 2023 All rights reserved</copyright>

namespace GraceDIFacade
{
    using System;
    using System.Linq;
    using Grace.DependencyInjection;
    using Vssl.Samples.FrameworkInterfaces;

    /// <summary>
    /// A Grace implementation of the dependency injection resolution interface
    /// </summary>
    public class GraceDI : IDependencyResolver, IDisposable
    {
        /// <summary>
        /// True if Dispose has already been called.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// The simple injector container
        /// </summary>
        private DependencyInjectionContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraceDI" /> class.
        /// </summary>
        /// <param name="container">The simple injector container</param>
        public GraceDI(DependencyInjectionContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the type mapping from the unity container
        /// </summary>
        /// <typeparam name="InterfaceType">The registered interface type</typeparam>
        /// <returns>The mapped type</returns>
        public InterfaceType Resolve<InterfaceType>()
            where InterfaceType : class
        {
            return (InterfaceType)this.container.Locate<InterfaceType>();
        }

        /// <summary>
        /// Gets the type mapping from the unity container
        /// </summary>
        /// <param name="interfaceType">The registered interface type</param>
        /// <returns>The mapped type</returns>
        public object Resolve(Type interfaceType)
        {
            return this.container.Locate(interfaceType);
        }

        /// <summary>
        /// Registers a class and its interface
        /// </summary>
        /// <typeparam name="T">The type of the interface</typeparam>
        /// <typeparam name="U">The type of the class</typeparam>
        public void Register<T, U>()
            where T : class
            where U : class, T
        {
            this.container.Configure(c =>
            {
                c.Export<U>().As<T>();
            });
        }

        /// <summary>
        /// Registers a class as a singleton
        /// </summary>
        /// <typeparam name="T">The type of the interface</typeparam>
        /// <typeparam name="U">The type of the class</typeparam>
        public void RegisterSingleton<T, U>()
            where T : class
            where U : class, T
        {
            this.container.Configure(c =>
            {
                c.Export<U>().As<T>().Lifestyle.Singleton();
            });
        }

        /// <summary>
        /// Gets the mapped type from the container given the registered type
        /// </summary>
        /// <param name="interfaceType">The registered interface type</param>
        /// <returns>The mapped type</returns>
        public Type GetMappedType(Type interfaceType)
        {
            if (this.container != null)
            {
                var collection = this.container.StrategyCollectionContainer.GetActivationStrategyCollection(interfaceType);
                if (collection != null)
                {
                    var strategy = collection.GetStrategies().FirstOrDefault();
                    return strategy.ActivationType;
                }

                return null;
            }

            return null;
        }

        /// <summary>
        /// Determines if the given type is registered
        /// </summary>
        /// <param name="interfaceType">The registered interface type</param>
        /// <returns>True if mapped</returns>
        public bool IsMapped(Type interfaceType)
        {
            if (this.container != null)
            {
                return this.container.CanLocate(interfaceType);
            }

            return false;
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">True if called via Dispose rather than a distructor</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.container != null)
                {
                    this.container.Dispose();
                    this.container = null;
                }
            }

            this.disposed = true;
        }
    }
}
