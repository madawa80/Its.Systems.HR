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

        public int AddLocation(string location)
        {
            var result = new Location()
            {
                Name = location
            };

            _db.Add<Location>(result);
            _db.SaveChanges();

            return result.Id;
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

        public IQueryable<Tag> GetAllTags()
        {
            return _db.Get<Tag>();
        }

        public void AddTags(IEnumerable<Tag> tags)
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
            // tagsToAdd is the incoming stuff, with all the tags to add to Tags in DB
            // but the list needs to be filtered for any existing tags in db.Tags!!
            var tagsToAddToDb = new List<Tag>(tagsToAdd);
            var currentTags = GetAllTags().ToList();


            var result = new List<Tag>();
            foreach (var tag in tagsToAddToDb.Where(n => currentTags.All(n2 => n2.Name != n.Name)))
            {
                result.Add(tag);
            }

            AddTags(result);


            // NOTICE! Have to savechanges later!
        }
    }
}