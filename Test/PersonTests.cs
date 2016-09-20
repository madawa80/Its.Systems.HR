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
        private IDbRepository db;

        public PersonTests() : base()
        {
            
        }


        [TestMethod]
        public void TestMethod1()
        {
            db = Container().Resolve<IDbRepository>();
            var p = db.Get<HrPerson>().ToList();
            var x = "";
        }

    }
}
