using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Its.Systems.HR.Interface.Web.Models.Mapping
{
    public class ParticipantMap : EntityTypeConfiguration<Participant>
    {
        public ParticipantMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Participant");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.Wishes).HasColumnName("Wishes");
        }
    }
}
