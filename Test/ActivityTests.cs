using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Its.Systems.HR.Test
{
    public class ActivityTests : BaseTest
    {
        private IDbRepository db;

        public ActivityTests() : base()
        {

        }


        [TestMethod]
        public void AdminCanAddActivity()
        {
            db = Container().Resolve<IDbRepository>();
            var p = db.Get<HrPerson>().SingleOrDefault(n => n.Id == 1);

            p.AddActivity("DataHack");
            

            Assert.AreEqual(1, db.Get<Activity>().SingleOrDefault(n => n.Name == "DataHack").Id);
        }
    }
}
