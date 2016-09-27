using System;
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

        public bool DeletePaticipantById(int id)
        {
            var paticipantFromDb = db.Get<Participant>().SingleOrDefault(n => n.Id == id);
            if (paticipantFromDb == null)
                return false;

            db.Delete(paticipantFromDb);
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }

        public Participant GetParticipantById(int id)
        {
            return db.Get<Participant>().SingleOrDefault(n => n.Id == id);
        }


        public bool SaveCommentsForParticipant(int personId, string comments)
        {
            Participant participant = GetAllParticipants().SingleOrDefault(n => n.Id == personId);
            if (participant == null)
                return false;

            participant.Comments = comments;
            try
            {
                db.SaveChanges();

            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }

        public bool SaveWishesForParticipant(int personId, string wishes)
        {
            Participant participant = GetAllParticipants().SingleOrDefault(n => n.Id == personId);
            if (participant == null)
                return false;

            participant.Wishes = wishes;
            try
            {
                db.SaveChanges();

            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }
    }
}
