
namespace DIUnitTests
{
    using System;
    using Autofac;
    using AutoFacDIFacade;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Vssl.Samples.FrameworkInterfaces;

    [TestClass]
    public class UnitTestAutoFacDI
    {
        private Guid testKey;

        private IDependencyResolver diFacade;

        [TestInitialize]
        public void TestInitialize()
        {
            this.testKey = Guid.NewGuid();
            SampleViewModel.InitCount(this.testKey, typeof(SampleViewModel));
            SampleService.InitCount(this.testKey, typeof(SampleService));

            var builder = new ContainerBuilder();
            // Configure the container (register)
            builder.RegisterInstance(new AutoFacDI()).As<IDependencyResolver>();
            builder.RegisterInstance(new SampleService(this.testKey)).As<ISampleService>();
            builder.RegisterType<SampleService2>().As<ISampleService2>();
            builder.Register(_ => new SampleViewModel(this.testKey)).As<ISampleViewModel>();

            var container = builder.Build();

            // define a global scope
            var scope = container.BeginLifetimeScope();

            this.diFacade = scope.Resolve<IDependencyResolver>();
            ((AutoFacDI)this.diFacade).SetGlobalScope(scope);
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
