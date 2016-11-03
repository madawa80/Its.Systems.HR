namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParticipantIsDeleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Participant", "IsDeleted", c => c.Boolean(nullable: false));
            DropColumn("dbo.Participant", "IsSuperAdmin");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Participant", "IsSuperAdmin", c => c.Boolean(nullable: false));
            DropColumn("dbo.Participant", "IsDeleted");
        }
    }
}
