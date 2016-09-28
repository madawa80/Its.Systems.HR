using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Domain.Interfaces
{
    public interface IPersonManager
    {
        IQueryable<Participant> GetAllParticipants();

        IQueryable<HrPerson> GetAllHrPersons();

        bool AddHrPerson(HrPerson hrPerson);

        Participant GetParticipantById(int id);

        bool DeletePaticipantById(int id);

        bool SaveCommentsForParticipant(int personId, string comments);

        bool SaveWishesForParticipant(int personId, string wishes);

        HrPerson GetHRPersonById(int sessionId);
    }
}