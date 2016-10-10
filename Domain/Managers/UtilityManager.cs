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
    }
}