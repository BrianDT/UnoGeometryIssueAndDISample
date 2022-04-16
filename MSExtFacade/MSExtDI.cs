// <copyright file="MSExtDI.cs" company="Visual Software Systems Ltd.">Copyright (c) 2017, 2019 All rights reserved</copyright>

namespace MSExtFacade
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Vssl.Samples.FrameworkInterfaces;

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

        public void Configure(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the type mapping from the unity container
        /// </summary>
        /// <typeparam name="InterfaceType">The registered interface type</typeparam>
        /// <returns>The mapped type</returns>
        public InterfaceType Resolve<InterfaceType>() where InterfaceType : class
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
        /// <typeparam name="T">The type of the interface</typeparam>
        /// <typeparam name="U">The type of the class</typeparam>
        public void Register<T, U>() where T : class where U : class, T
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers a class as a singleton
        /// </summary>
        /// <typeparam name="T">The type of the interface</typeparam>
        /// <typeparam name="U">The type of the class</typeparam>
        public void RegisterSingleton<T, U>() where T : class where U : class, T
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