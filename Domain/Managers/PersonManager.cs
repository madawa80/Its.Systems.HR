using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Domain.Managers
{
    public class PersonManager : IPersonManager
    {
        public IDbRepository db;
        public PersonManager(IDbRepository repo)
        {
            db = repo;
        }

        //public IQueryable<Person> GetPersons()
        //{
        //    db.Get<Person>().Include().Where().
        //}

        public IQueryable<Participant> GetAllParticipants()
        {
            return db.Get<Participant>();
        }

        public IQueryable<HrPerson> GetAllHrPersons()
        {
            return db.Get<HrPerson>();
        }

        public bool AddHrPerson(HrPerson hrPerson)
        {
            var allHrPersons = db.Get<HrPerson>().ToList();

            if (allHrPersons.Any(n => n.GetHrFullName() == hrPerson.GetHrFullName()))
                return false;

            db.Add<HrPerson>(hrPerson);

            return true;
        }
    }
}
