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
        /// <summary>
        /// Gets all the participants.
        /// </summary>
        /// <returns>IQueryable of Participant.</returns>
        IQueryable<Participant> GetAllParticipants();

        /// <summary>
        /// Gets all the HrPersons.
        /// </summary>
        /// <returns>IQueryable of HrPerson.</returns>
        IQueryable<Participant> GetAllHrPersons();

        /// <summary>
        /// Makes the participant passed to a HrPerson,
        /// by setting the IsHrPerson flag to true.
        /// </summary>
        /// <param name="participant"></param>
        /// <returns>True if successfull.</returns>
        bool MakeParticipantHrPerson(Participant participant);

        /// <summary>
        /// Gets a participant by participantId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The participant, if found.</returns>
        Participant GetParticipantById(int id);

        Participant GetParticipantByCas(string cas);

        bool DeletePaticipantById(int id);

        /// <summary>
        /// Saves the comments passed to the person by personId.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="comments"></param>
        /// <returns>True if successfull.</returns>
        bool SaveCommentsForParticipant(int personId, string comments);

        /// <summary>
        /// Saves the wishes passed for a person, by personId.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="wishes"></param>
        /// <returns>True if successfull.</returns>
        bool SaveWishesForParticipant(int personId, string wishes);

        Participant GetHrPersonById(int hrPersonId);

        /// <summary>
        /// Adds a participant by participantId to a session, by sessionId.
        /// </summary>
        /// <param name="participantId"></param>
        /// <param name="sessionId"></param>
        /// <returns>True if successfull.</returns>
        bool AddParticipantToSession(int participantId, int sessionId);

        /// <summary>
        /// Removes a participant by participantId from a session, by sessionId.
        /// </summary>
        /// <param name="participantId"></param>
        /// <param name="sessionId"></param>
        /// <returns>True if successfull.</returns>
        bool RemoveParticipantFromSession(int participantId, int sessionId);

        /// <summary>
        /// Gets all participants for a session by sessionId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IQueryable<Participant> GetAllParticipantsForSession(int id);

        bool UpdateReviewForSessionParticipant(int sessionId, int participantId, int rating, string comments);

        bool AddDeleteItsPersonsToDb();
    }
}