using System.Collections.Generic;
using System.Linq;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Domain.Interfaces
{
    public interface IPersonManager
    {
        /// <summary>
        /// Gets all the participants that are not hidden (IsDeleted).
        /// </summary>
        /// <returns>IQueryable of Participant.</returns>
        IQueryable<Participant> GetAllParticipants();

        /// <summary>
        /// Gets all participants INCLUDING those that are hidden (IsDeleted).
        /// </summary>
        /// <returns></returns>
        IQueryable<Participant> GetAllParticipantsIncludingDeleted();

        /// <summary>
        /// Gets all the HrPersons that are not hidden (IsDeleted).
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

        /// <summary>
        /// Gets a participant by CAS-ID.
        /// </summary>
        /// <param name="cas"></param>
        /// <returns>The participant, if found.</returns>
        Participant GetParticipantByCas(string cas);

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

        /// <summary>
        /// Gets a specific Participant that is also a HRPerson.
        /// </summary>
        /// <param name="hrPersonId"></param>
        /// <returns>Participant or null</returns>
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
        /// Gets all participants that are not hidden (IsDeleted) for a session by sessionId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IQueryable<Participant> GetAllParticipantsForSession(int id);

        /// <summary>
        /// Updates a review for a Session Participant, with the rating and optional comments.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="participantId"></param>
        /// <param name="rating"></param>
        /// <param name="comments"></param>
        /// <returns>True if successfull.</returns>
        bool UpdateReviewForSessionParticipant(int sessionId, int participantId, int rating, string comments);

        /// <summary>
        /// Adds one or many Employees from the UmU Web Api for the ITS-section to 
        /// the database Participant table.
        /// </summary>
        /// <returns>The added participants, if any.</returns>
        List<Participant> AddItsPersons();

        /// <summary>
        /// Sets the flag "IsActive" to false in the database Participant table for one or many employers that 
        /// is no longer existing in the UmU Web Apu for the ITS-section.
        /// </summary>
        /// <returns>The inactivated participants, if any.</returns>
        List<Participant> InactivateItsPersons();

        /// <summary>
        /// Gets a Session Participant, by sessionId & participantId.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="participantId"></param>
        /// <returns>The SessionParticipation, or null.</returns>
        SessionParticipant GetASessionParticipant(int sessionId, int participantId);

        /// <summary>
        /// Gets all the Session Participations that are not hidden (IsDeleted) by a sessionId.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns>IQueryable of Session Participant or null if none is found.</returns>
        IQueryable<SessionParticipant> GetAllSessionParticipationsForSessionById(int sessionId);

        /// <summary>
        /// Gets all the Session Participatants that are not hidden (IsDeleted).
        /// </summary>
        /// <returns>IQueryable of SessionParticipant, or null if there are none in the database.</returns>
        IQueryable<SessionParticipant> GetAllSessionParticipants();

        /// <summary>
        /// Adds a participant to a session that is open for
        /// expression of interest AND has not yet happened.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="participantId"></param>
        /// <returns>True if successfull.</returns>
        bool AddExpressionOfInterest(int sessionId, int participantId);

        /// <summary>
        /// Removes a participant from a session that is open for
        /// expression of interest AND has not yet happened.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="participantId"></param>
        /// <returns></returns>
        bool RemoveExpressionOfInterest(int sessionId, int participantId);

        bool ChangeParticipantHrStatus(int id, bool status);

        bool ChangeParticipantDeletedStatus(int id, bool status);

        void UpdateEmail();
    }
}