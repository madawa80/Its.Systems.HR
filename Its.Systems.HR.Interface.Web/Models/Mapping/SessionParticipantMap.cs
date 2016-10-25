using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Its.Systems.HR.Interface.Web.Models.Mapping
{
    public class SessionParticipantMap : EntityTypeConfiguration<SessionParticipant>
    {
        public SessionParticipantMap()
        {
            // Primary Key
            this.HasKey(t => new { t.SessionId, t.ParticipantId });

            // Properties
            this.Property(t => t.SessionId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ParticipantId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("SessionParticipant");
            this.Property(t => t.SessionId).HasColumnName("SessionId");
            this.Property(t => t.ParticipantId).HasColumnName("ParticipantId");
            this.Property(t => t.Rating).HasColumnName("Rating");

            // Relationships
            this.HasRequired(t => t.Participant)
                .WithMany(t => t.SessionParticipants)
                .HasForeignKey(d => d.ParticipantId);
            this.HasRequired(t => t.Session)
                .WithMany(t => t.SessionParticipants)
                .HasForeignKey(d => d.SessionId);

        }
    }
}
