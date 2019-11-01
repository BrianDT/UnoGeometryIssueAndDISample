// <copyright file="UnityDI.cs" company="Visual Software Systems Ltd.">Copyright (c) 2017, 2019 All rights reserved</copyright>

namespace UnityDIFacade
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Unity;
    using Unity.Lifetime;
    using Vssl.Samples.FrameworkInterfaces;

    /// <summary>
    /// A Unity implementation of the dependency injection resolution interface
    /// </summary>
    public class UnityDI : IDependencyResolver
    {
        #region [ Public static properties ]
#if WINDOWS_PHONE
        /// <summary>
        /// The container service container
        /// </summary>
        private IContainerService container;
#else
        /// <summary>
        /// The unity container
        /// </summary>
        private IUnityContainer container;
#endif
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityDI" /> class.
        /// </summary>
        /// <param name="container">The Unity container</param>
        public UnityDI(IUnityContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Gets the type mapping from the unity container
        /// </summary>
        /// <typeparam name="InterfaceType">The registered interface type</typeparam>
        /// <returns>The mapped type</returns>
        public InterfaceType Resolve<InterfaceType>() where InterfaceType : class
        {
            return this.container.Resolve<InterfaceType>();
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
        /// Registers a class and its interface
        /// </summary>
        /// <typeparam name="T">The type of the interface</typeparam>
        /// <typeparam name="U">The type of the class</typeparam>
        public void Register<T, U>() where T : class where U : class, T
        {
            this.container.RegisterType<T, U>();
        }

        /// <summary>
        /// Registers a class as a singleton
        /// </summary>
        /// <typeparam name="T">The type of the interface</typeparam>
        /// <typeparam name="U">The type of the class</typeparam>
        public void RegisterSingleton<T, U>() where T : class where U : class, T
        {
            this.container.RegisterType<T, U>(new ContainerControlledLifetimeManager());
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
                return this.container.Registrations.Where(r => r.RegisteredType == interfaceType).Select(r => r.MappedToType).FirstOrDefault();
            }

            return null;
        }
    }
}
