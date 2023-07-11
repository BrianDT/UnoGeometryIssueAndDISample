
namespace DIUnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Castle.Core.Resource;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using CastleWindsorDiFacade;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Vssl.Samples.FrameworkInterfaces;

    [TestClass]
    public class UnitTestCastleWindsor
    {
        private Guid testKey;

        private IDependencyResolver diFacade;

        [TestInitialize]
        public void TestInitialize()
        {
            this.testKey = Guid.NewGuid();
            SampleViewModel.InitCount(this.testKey, typeof(SampleViewModel));
            SampleService.InitCount(this.testKey, typeof(SampleService));

            var container = new WindsorContainer();

            container.Register(Component.For<IDependencyResolver>().Instance(new CastleWindsorDi(container)));
            container.Register(Component.For<ISampleService>().Instance(new SampleService(this.testKey)));
            container.Register(Component.For<ISampleService2>().ImplementedBy<SampleService2>().LifeStyle.Transient);
            container.Register(Component.For<ISampleViewModel>().UsingFactoryMethod(() => new SampleViewModel(this.testKey)).LifeStyle.Transient);

            this.diFacade = container.Resolve<IDependencyResolver>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ((IDisposable)this.diFacade).Dispose();
            this.diFacade = null;
        }

        [TestMethod]
        public void TestResolve()
        {
            int expected = 0;
            this.TestResolveViewModel(ref expected);
            this.TestResolveService();
        }

        [TestMethod]
        public void TestRegisterSingleton()
        {
            bool threwException = false;
            try
            {
                this.diFacade.RegisterSingleton<ISampleSingleton, SampleSingletonService>();
                var dy1 = this.diFacade.Resolve<ISampleSingleton>();
                Assert.IsNotNull(dy1);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                threwException = true;
            }

            Assert.IsFalse(threwException);
        }

        [TestMethod]
        public void TestRegister()
        {
            bool threwException = false;
            try
            {
                this.diFacade.Register<ISampleDynamic, SampleDynamic>();
                var dy1 = this.diFacade.Resolve<ISampleDynamic>();
                Assert.IsNotNull(dy1);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                threwException = true;
            }

            Assert.IsFalse(threwException);
        }

        [TestMethod]
        public void TestMapping()
        {
            var isMapped = ((CastleWindsorDi)this.diFacade).IsMapped(typeof(ISampleDynamic));
            Assert.IsFalse(isMapped);

            isMapped = ((CastleWindsorDi)this.diFacade).IsMapped(typeof(ISampleService2));
            Assert.IsTrue(isMapped);

            isMapped = ((CastleWindsorDi)this.diFacade).IsMapped(typeof(ISampleService));
            Assert.IsTrue(isMapped);

            isMapped = ((CastleWindsorDi)this.diFacade).IsMapped(typeof(ISampleViewModel));
            Assert.IsTrue(isMapped);

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
