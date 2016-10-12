namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCasIdIsHrPersonIsActiveIsSuperAdmintoParticipant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Participant", "CasId", c => c.String());
            AddColumn("dbo.Participant", "IsHrPerson", c => c.Boolean(nullable: false));
            AddColumn("dbo.Participant", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Participant", "IsSuperAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Participant", "IsSuperAdmin");
            DropColumn("dbo.Participant", "IsActive");
            DropColumn("dbo.Participant", "IsHrPerson");
            DropColumn("dbo.Participant", "CasId");
        }
    }
}
