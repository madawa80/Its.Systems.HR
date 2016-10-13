﻿using System;
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
            impl = Container().Resolve<IPersonManager>();
        }
         
        [TestMethod]
        public void GetAllParticipants_ShouldReturnCountOf6()
        {
            impl = Container().Resolve<IPersonManager>();

            var p = impl.GetAllParticipants().ToList();

            Assert.AreEqual(6, p.Count);
        }

        [TestMethod]
        public void GetAllHrPersons_ShouldReturnCountOf5()
        {
            var p = impl.GetAllHrPersons().ToList();

            Assert.AreEqual(5, p.Count);
        }

        [TestMethod]
        public void AddHrPersonWithUniqueFullName_ShouldReturnTrue()
        {
            var newHrPerson = new HrPerson()
            {
                FirstName = "UniqueFirstName123",
                LastName = "UniqueLastName123",
            };
            
            var result = impl.AddHrPerson(newHrPerson);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void AddHrPersonWithFullNameAlreadyExisting_ShouldReturnFalse()
        {
            var newHrPerson = new HrPerson()
            {
                FirstName = "Samme",
                LastName = "Petersson",
            };
            
            var result = impl.AddHrPerson(newHrPerson);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void GetAllParticipantsForSessionJavaOne2015_ShouldReturnCountOf3()
        {
            var result = impl.GetAllParticipantsForSession(1).Count();

            Assert.AreEqual(3, result);
        }

    }
}
