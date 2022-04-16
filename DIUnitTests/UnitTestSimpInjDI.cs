
namespace DIUnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SimpleInjector;
    using SimInjDIFacade;
    using Vssl.Samples.FrameworkInterfaces;

    [TestClass]
    public class UnitTestSimpInjDI
    {
        private Guid testKey;

        private IDependencyResolver diFacade;

        [TestInitialize]
        public void TestInitialize()
        {
            this.testKey = Guid.NewGuid();
            SampleViewModel.InitCount(this.testKey, typeof(SampleViewModel));
            SampleService.InitCount(this.testKey, typeof(SampleService));

            var container = new Container();
            // Configure the container (register)
            container.Register<IDependencyResolver>(() => new SimpInjDI(container), Lifestyle.Singleton);
            container.Register<ISampleService>(() => new SampleService(this.testKey), Lifestyle.Singleton);
            container.Register<ISampleService2, SampleService2>(Lifestyle.Singleton);
            container.Register<ISampleViewModel>(() => new SampleViewModel(this.testKey));

            // Verify your configuration
            container.Verify();

            this.diFacade = container.GetInstance<IDependencyResolver>();
        }

        [TestMethod]
        public void TestResolve()
        {
            int expected = 1; // There is a resolve during verify
            this.TestResolveViewModel(ref expected);
            this.TestResolveService();
        }

        [TestMethod]
        public void TestRegister()
        {
            bool threwException = false;
            try
            {
                this.diFacade.RegisterSingleton<ISampleDynamic, SampleDynamic>();
                var dy1 = this.diFacade.Resolve<ISampleDynamic>();
                Assert.IsNotNull(dy1);
            }
            catch (Exception)
            {
                threwException = true;
            }

            Assert.IsTrue(threwException);
        }

        [TestMethod]
        public void TestMapping()
        {
            var type = this.diFacade.GetMappedType(typeof(ISampleDynamic));
            Assert.IsNull(type);

            type = this.diFacade.GetMappedType(typeof(ISampleService2));
            Assert.AreEqual(typeof(SampleService2), type);

            type = this.diFacade.GetMappedType(typeof(ISampleService));
            Assert.AreNotEqual(typeof(SampleService), type); // Because of constructor args
        }

        private void TestResolveService()
        {
            var s1 = this.diFacade.Resolve(typeof(ISampleService)) as ISampleService;
            Assert.IsNotNull(s1);
            Assert.AreEqual(1, s1.Count(this.testKey));
            var s2 = this.diFacade.Resolve(typeof(ISampleService)) as ISampleService;
            Assert.IsNotNull(s2);
            Assert.AreEqual(1, s2.Count(this.testKey));
        }

        private void TestResolveViewModel(ref int expected)
        {
            var vm = this.diFacade.Resolve<ISampleViewModel>();
            Assert.AreEqual(++expected, vm.Count(this.testKey));
            var vm2 = this.diFacade.Resolve<ISampleViewModel>();
            Assert.AreEqual(++expected, vm2.Count(this.testKey));
        }
    }
}
