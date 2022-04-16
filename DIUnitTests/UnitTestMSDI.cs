
namespace DIUnitTests
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MSExtFacade;
    using Vssl.Samples.FrameworkInterfaces;

    [TestClass]
    public class UnitTestMSDI
    {
        private Guid testKey;

        private IDependencyResolver diFacade;

        [TestInitialize]
        public void TestInitialize()
        {
            this.testKey = Guid.NewGuid();
            SampleViewModel.InitCount(this.testKey, typeof(SampleViewModel));
            SampleService.InitCount(this.testKey, typeof(SampleService));
            var services = new ServiceCollection();
            services.AddSingleton<IDependencyResolver, MSExtDI>()
                    .AddSingleton<ISampleService>(_ => new SampleService(this.testKey))
                    .AddTransient<ISampleViewModel>(_ => new SampleViewModel(this.testKey));

            var serviceProvider = services.BuildServiceProvider();
            this.diFacade = serviceProvider.GetRequiredService<IDependencyResolver>();
            ((MSExtDI)this.diFacade).Configure(serviceProvider);
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