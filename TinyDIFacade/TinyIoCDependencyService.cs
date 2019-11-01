// <copyright file="StructureMapDI.cs" company="Twisted Fish Ltd. and Visual Software Systems Ltd.">Copyright (c) 2017, 2018, 2019 All rights reserved</copyright>

using System;
using System.Collections.Generic;
using TinyIoC;
using Vssl.Samples.FrameworkInterfaces;

namespace Framework.TinyDIFacade
{
	public class TinyIoCDependencyService : IDependencyResolver
    {
		private readonly TinyIoCContainer _container;

		public TinyIoCDependencyService() {
			_container = TinyIoCContainer.Current;
		}

		public TinyIoCDependencyService(TinyIoCContainer container) {
			_container = container;

		}

		public IDependencyResolver GetChildService() {
			return new TinyIoCDependencyService(_container.GetChildContainer());
		}

		public void Register<RegisterType>(Func<RegisterType> factory) where RegisterType : class {
			_container.Register<RegisterType>((_, __) => {
				return factory();
			});
		}

        public void Register<RegisterType>() where RegisterType : class => _container.Register<RegisterType>().AsSingleton();
        public void Register<RegisterType, RegisterImplementation>()
            where RegisterType : class
            where RegisterImplementation : class,RegisterType
            => this._container.Register<RegisterType, RegisterImplementation>().AsMultiInstance();

		public void RegisterMultiple<RegisterType>(IEnumerable<Type> implementationTypes) {
			_container.RegisterMultiple<RegisterType>(implementationTypes);
		}

		public void RegisterSingleton<RegisterType>() where RegisterType : class {
            _container.Register<RegisterType>().AsSingleton();
		}

		public void RegisterSingleton<RegisterType>(RegisterType instance) where RegisterType : class {
			_container.Register<RegisterType>(instance);
		}

		public void RegisterSingleton<RegisterType, RegisterImplementation>()
			where RegisterType : class
			where RegisterImplementation : class, RegisterType {
			_container.Register<RegisterType, RegisterImplementation>().AsSingleton();
		}

		public void RegisterSingleton<RegisterType, RegisterImplementation>(RegisterImplementation instance)
			where RegisterType : class
			where RegisterImplementation : class, RegisterType {
			_container.Register<RegisterType, RegisterImplementation>(instance);
		}

		public ResolveType Resolve<ResolveType>() where ResolveType : class {
			return _container.Resolve<ResolveType>();
		}

		public object Resolve(Type resolveType) {
			return _container.Resolve(resolveType);
		}

		public IEnumerable<ResolveType> ResolveAll<ResolveType>() where ResolveType : class {
			return _container.ResolveAll<ResolveType>();
		}

		public Lazy<ResolveType> LazyResolve<ResolveType>() where ResolveType : class {
			return new Lazy<ResolveType>(() => {
				if (CanResolve<ResolveType>()) {
					return Resolve<ResolveType>();
				}

				return null;
			});
		}

		public bool CanResolve<ResolveType>() where ResolveType : class {
			return _container.CanResolve<ResolveType>();
		}

		public bool TryResolve<ResolveType>(out ResolveType resolvedType) where ResolveType : class {
			return _container.TryResolve(out resolvedType);
		}

		public void Dispose() {
			_container.Dispose();
		}

        /// <summary>
        /// Gets the mapped type from the container given the registered type
        /// </summary>
        /// <param name="interfaceType">The registered interface type</param>
        /// <returns>The mapped type</returns>
        public Type GetMappedType(Type interfaceType)
        {
            throw new NotImplementedException();
        }
    }
}
