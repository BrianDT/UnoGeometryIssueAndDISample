// <copyright file="StructureMapDI.cs" company="Twisted Fish Ltd. and Visual Software Systems Ltd.">Copyright (c) 2017, 2018 All rights reserved</copyright>

namespace Framework.SMDIFacade
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using StructureMap;
    using Vssl.Samples.FrameworkInterfaces;

    /// <summary>
    /// A Simple Injector implementation of the dependency injection resolution interface
    /// </summary>
    public class StructureMapDI : IDependencyResolver
    {
        /// <summary>
        /// The simple injector container
        /// </summary>
        private IContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapDI" /> class.
        /// </summary>
        /// <param name="container">The simple injector container</param>
        public StructureMapDI(IContainer container)
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
            return (InterfaceType)this.container.GetInstance<InterfaceType>();
        }

        /// <summary>
        /// Gets the type mapping from the unity container
        /// </summary>
        /// <param name="interfaceType">The registered interface type</param>
        /// <returns>The mapped type</returns>
        public object Resolve(Type interfaceType)
        {
            return this.container.GetInstance(interfaceType);
        }

        /// <summary>
        /// Registers a class and its interface
        /// </summary>
        /// <typeparam name="T">The type of the interface</typeparam>
        /// <typeparam name="U">The type of the class</typeparam>
        public void Register<T, U>() where T : class where U : class, T
        {
            this.container.Configure(c => { c.For<T>().Use<U>(); });
        }

        /// <summary>
        /// Registers a class as a singleton
        /// </summary>
        /// <typeparam name="T">The type of the interface</typeparam>
        /// <typeparam name="U">The type of the class</typeparam>
        public void RegisterSingleton<T, U>() where T : class where U : class, T
        {
            this.container.Configure(c => { c.For<T>().Use<U>().Singleton(); });
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
                var instance = this.container.Model.For(interfaceType).Instances.FirstOrDefault();
                if (instance != null)
                {
                    return instance.ReturnedType;
                }

                return null;
                ////return this.container.Model.For(interfaceType).PluginType;
            }

            return null;
        }
    }
}
