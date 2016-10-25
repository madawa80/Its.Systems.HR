using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Its.Systems.HR.Interface.Web.Models.Mapping
{
    public class SessionMap : EntityTypeConfiguration<Session>
    {
        public SessionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Session");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.LocationId).HasColumnName("LocationId");
            this.Property(t => t.HrPersonId).HasColumnName("HrPersonId");
            this.Property(t => t.ActivityId).HasColumnName("ActivityId");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.Evaluation).HasColumnName("Evaluation");

            // Relationships
            this.HasRequired(t => t.Activity)
                .WithMany(t => t.Sessions)
                .HasForeignKey(d => d.ActivityId);
            this.HasRequired(t => t.HrPerson)
                .WithMany(t => t.Sessions)
                .HasForeignKey(d => d.HrPersonId);
            this.HasRequired(t => t.Location)
                .WithMany(t => t.Sessions)
                .HasForeignKey(d => d.LocationId);

        }
    }
}
