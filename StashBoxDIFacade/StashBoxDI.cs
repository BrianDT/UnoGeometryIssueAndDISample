// <copyright file="StashBoxDI.cs" company="Visual Software Systems Ltd.">Copyright (c) 20232 All rights reserved</copyright>

namespace StashBoxDIFacade
{
    using System;
    using System.Linq;
    using Stashbox;
    using VSFI = Vssl.Samples.FrameworkInterfaces;

    /// <summary>
    /// A Stash Box implementation of the dependency injection resolution interface
    /// </summary>
    public class StashBoxDI : VSFI.IDependencyResolver, IDisposable
    {
        /// <summary>
        /// True if Dispose has already been called.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// The DI container
        /// </summary>
        private StashboxContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="StashBoxDI" /> class.
        /// </summary>
        /// <param name="container">The simple injector container</param>
        public StashBoxDI(StashboxContainer container)
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
        /// <typeparam name="TInterfaceType">The registered interface type</typeparam>
        /// <returns>The mapped type</returns>
        public TInterfaceType Resolve<TInterfaceType>()
            where TInterfaceType : class
        {
            return (TInterfaceType)this.container.Resolve<TInterfaceType>();
        }

        /// <summary>
        /// Gets the type mapping from the unity container
        /// </summary>
        /// <param name="interfaceType">The registered interface type</param>
        /// <returns>The mapped type</returns>
        public object Resolve(Type interfaceType)
        {
            return this.container.Resolve(interfaceType);
        }

        /// <summary>
        /// Registers a class and its interface.
        /// </summary>
        /// <typeparam name="TF">The type of the interface.</typeparam>
        /// <typeparam name="TI">The type of the class.</typeparam>
        public void Register<TF, TI>()
            where TF : class
            where TI : class, TF
        {
            this.container.Register<TF, TI>();
        }

        /// <summary>
        /// Registers a class as a singleton
        /// </summary>
        /// <typeparam name="TF">The type of the interface.</typeparam>
        /// <typeparam name="TI">The type of the class.</typeparam>
        public void RegisterSingleton<TF, TI>()
            where TF : class
            where TI : class, TF
        {
            this.container.RegisterSingleton<TF, TI>();
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
                var mappedComponent = this.container.GetRegistrationMappings().Where(c => c.Key == interfaceType).FirstOrDefault();
                if (mappedComponent.Value != null)
                {
                    return mappedComponent.Value.ImplementationType;
                }
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
                return this.container.IsRegistered(interfaceType);
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