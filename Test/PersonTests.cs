using System;
using System.Linq;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Its.Systems.HR.Test
{
    [TestClass]
    public class PersonTests : BaseTest
    {
        private IPersonManager impl;

        public PersonTests() : base()
        {
            
        }


        [TestMethod]
        public void TestMethod1()
        {
            impl = Container().Resolve<IPersonManager>();
            // Add person
            var p = impl.GetAllHrPersons().ToList();
            //Assert.AreEqual();
            var x = "";
        }

    }
}
