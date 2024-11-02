// <copyright file="MSExtDI.cs" company="Visual Software Systems Ltd.">Copyright (c) 2017, 2019 All rights reserved</copyright>

namespace MSExtFacade
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Vssl.Samples.FrameworkInterfaces;

    /// <summary>
    /// A Microsoft Extensions Dependency Injections implementation of the dependency injection resolution interface
    /// </summary>
    public class MSExtDI : IDependencyResolver
    {
        /// <summary>
        /// The service provider
        /// </summary>
        private IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSExtDI" /> class.
        /// </summary>
        public MSExtDI()
        {
        }

        /// <summary>
        /// Configure the service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        public void Configure(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the type mapping from the unity container
        /// </summary>
        /// <typeparam name="InterfaceType">The registered interface type</typeparam>
        /// <returns>The mapped type</returns>
        public InterfaceType Resolve<InterfaceType>()
            where InterfaceType : class
        {
            return this.serviceProvider.GetRequiredService<InterfaceType>();
        }

        /// <summary>
        /// Gets the type mapping from the unity container
        /// </summary>
        /// <param name="interfaceType">The registered interface type</param>
        /// <returns>The mapped type</returns>
        public object Resolve(Type interfaceType)
        {
            return this.serviceProvider.GetRequiredService(interfaceType);
        }

        /// <summary>
        /// Registers a class and its interface
        /// </summary>
        /// <typeparam name="TF">The type of the interface</typeparam>
        /// <typeparam name="TI">The type of the class</typeparam>
        public void Register<TF, TI>()
            where TF : class
            where TI : class, TF
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers a class as a singleton
        /// </summary>
        /// <typeparam name="TF">The type of the interface</typeparam>
        /// <typeparam name="TI">The type of the class</typeparam>
        public void RegisterSingleton<TF, TI>()
            where TF : class
            where TI : class, TF
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the mapped type from the container given the registered type
        /// </summary>
        /// <param name="interfaceType">The registered interface type</param>
        /// <returns>The mapped type</returns>
        public Type GetMappedType(Type interfaceType)
        {
            if (this.serviceProvider != null)
            {
                return this.serviceProvider.GetService(interfaceType)?.GetType();
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
            if (this.serviceProvider != null)
            {
                var serviceProviderIsService = this.serviceProvider.GetService<IServiceProviderIsService>();

                return serviceProviderIsService.IsService(interfaceType);
            }

            return false;
        }
    }
}
