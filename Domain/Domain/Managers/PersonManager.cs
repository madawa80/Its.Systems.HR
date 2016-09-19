using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Its.Systems.HR.Domain.Interfaces;

namespace Its.Systems.HR.Domain.Managers
{
    public class PersonManager
    {
        public IDbRepository db;
        public PersonManager(IDbRepository repo)
        {
            db = repo;
        }

        //public IQueryable<Person> GetPersons()
        //{
        //    db.Get<Person>().Include().Where()
        //} 

    }
}
