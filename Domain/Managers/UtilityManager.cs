using System.Collections.Generic;
using System.Linq;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Domain.Managers
{
    public class UtilityManager : IUtilityManager
    {
        private readonly IDbRepository _db;

        public UtilityManager(IDbRepository repo)
        {
            _db = repo;
        }

        public IQueryable<Location> GetAllLocations()
        {
            return _db.Get<Location>();
        }

        public int? GetIdForLocationOrCreateIfNotExists(string location)
        {
            if (string.IsNullOrEmpty(location))
                return null;


            int resultId;

            Location locationExisting =
                GetAllLocations().SingleOrDefault(n => n.Name.ToLower() == location.ToLower());

            if (locationExisting == null)
            {
                resultId = AddLocation(location);
            }
            else
            {
                resultId = locationExisting.Id;
            }

            return resultId;
        }

        private int AddLocation(string location)
        {
            var result = new Location()
            {
                Name = location
            };

            _db.Add<Location>(result);
            _db.SaveChanges();

            return result.Id;
        }

        public IQueryable<Tag> GetAllTags()
        {
            return _db.Get<Tag>();
        }

        private void AddTags(IEnumerable<Tag> tags)
        {
            // TODO: this will result in one roundtrip for every new tag...
            foreach (var tag in tags)
            {
                _db.Add<Tag>(tag);
            }
            _db.SaveChanges();
        }

        public void AddNewTagsToDb(List<Tag> tagsToAdd)
        {
            // Make sure only new tags are inserted
            for (int index = 0; index < tagsToAdd.Count; index++)
                tagsToAdd[index].Name = tagsToAdd[index].Name.ToLower();

            List<Tag> tagsToAddToDb = new List<Tag>();
            foreach (var tagName in tagsToAdd.Select(n => n.Name).Distinct())
                tagsToAddToDb.Add(new Tag() {Name = tagName });

            var currentTags = GetAllTags().ToList();
            
            var result = new List<Tag>();
            foreach (var tag in tagsToAddToDb.Where(n => currentTags.All(n2 => n2.Name != n.Name)))
                result.Add(tag);

            // NOTICE! SaveChanges in the method below.
            AddTags(result);
        }

        public double GetRatingForSessionById(int id)
        {
            var sessionParticipations = _db.Get<SessionParticipant>().Where(n => n.SessionId == id).ToList();

            if (sessionParticipations.Count < 1)
                return 0;

            if (sessionParticipations.Count(n => n.Rating != 0) < 1)
                return 0;

            return sessionParticipations.Where(n => n.Rating != 0).Average(a => a.Rating);
        }

        public IQueryable<Tag> GetAllTagsForSessionById(int sessionId)
        {
            return _db.Get<SessionTag>().Where(n => n.SessionId == sessionId).Select(a => a.Tag);
        }

        public Tag GetTag(int tagId)
        {
            return _db.Get<Tag>().SingleOrDefault(n => n.Id == tagId);
        }
    }
}