using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Its.Systems.HR.Infrastructure
{
    public class HRContext : DbContext
    {
        public HRContext() : base("name=HRContext")
        {
            Database.SetInitializer<HRContext>(null);
            this.Configuration.LazyLoadingEnabled = false;
        }

        public HRContext(DbConnection connection) : base(connection, true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
