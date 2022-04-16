namespace SimInjDIFacade
{
    using SimpleInjector;
    using Vssl.Samples.FrameworkInterfaces;

    public class SimpInjDI : IDependencyResolver
    {
        /// <summary>
        /// The simple injector container
        /// </summary>
        private Container container;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapDI" /> class.
        /// </summary>
        /// <param name="container">The simple injector container</param>
        public SimpInjDI(Container container)
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
            if (this.container != null)
            {
                var instanceProducer = this.container.GetRegistration(interfaceType);
                if (instanceProducer != null)
                {
                    return instanceProducer.ImplementationType;
                }

                return null;
            }

            return null;
        }
    }
}