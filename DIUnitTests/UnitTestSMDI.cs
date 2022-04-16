
namespace DIUnitTests
{
    using System;
    using Framework.SMDIFacade;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using StructureMap;
    using Vssl.Samples.FrameworkInterfaces;

    [TestClass]
    public class UnitTestSMDI
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
            container.Configure(_ =>
            {
                // DI Facade
                _.For<IDependencyResolver>().Singleton().Use(() => new StructureMapDI(container));

                // Services
                _.For<ISampleService>().Singleton().Use(() => new SampleService(this.testKey));

                // View models
                _.For<ISampleViewModel>().Use(() => new SampleViewModel(this.testKey));
            });

            this.diFacade = container.GetInstance<IDependencyResolver>();
        }

        [TestMethod]
        public void TestResolve()
        {
            int expected = 0;
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

            Assert.IsFalse(threwException);
        }

        [TestMethod]
        public void TestMapping()
        {
            var type = this.diFacade.GetMappedType(typeof(ISampleDynamic));
            Assert.IsNull(type);

            type = this.diFacade.GetMappedType(typeof(ISampleService));
            Assert.AreEqual(typeof(SampleService), type);
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
