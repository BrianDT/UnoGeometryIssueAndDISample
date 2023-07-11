// <copyright file="AutoFacDI.cs" company="Visual Software Systems Ltd.">Copyright (c) 2022 All rights reserved</copyright>

namespace AutoFacDIFacade
{
    using Autofac;
    using Vssl.Samples.FrameworkInterfaces;

    /// <summary>
    /// An AutoFac implementation of the dependency injection resolution interface
    /// </summary>
    public class AutoFacDI : IDependencyResolver, IDisposable
    {
        private ILifetimeScope scope;

        /// <summary>
        /// Sets the scopr to be used.
        /// </summary>
        /// <param name="scope">The scope</param>
        public void SetGlobalScope(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose()
        {
            if (this.scope != null)
            {
                this.scope.Dispose();
                this.scope = null;
            }
        }

        /// <summary>
        /// Gets the type mapping from the unity container
        /// </summary>
        /// <typeparam name="InterfaceType">The registered interface type</typeparam>
        /// <returns>The mapped type</returns>
        public InterfaceType Resolve<InterfaceType>() where InterfaceType : class
        {
            return (InterfaceType)this.scope.Resolve<InterfaceType>();
        }

        /// <summary>
        /// Gets the type mapping from the unity container
        /// </summary>
        /// <param name="interfaceType">The registered interface type</param>
        /// <returns>The mapped type</returns>
        public object Resolve(Type interfaceType)
        {
            return this.scope.Resolve(interfaceType);
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
            if (this.scope != null)
            {
                var registration = this.scope.ComponentRegistry.Registrations.FirstOrDefault(r => r.Services.Cast<Autofac.Core.TypedService>().Any(s => s.ServiceType == interfaceType));
                if (registration != null)
                {
                    return registration.Activator.LimitType;
                }

                return null;
                ////return this.container.Model.For(interfaceType).PluginType;
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
            if (this.scope != null)
            {
                return this.scope.IsRegistered(interfaceType);
            }

            return false;
        }
    }
}