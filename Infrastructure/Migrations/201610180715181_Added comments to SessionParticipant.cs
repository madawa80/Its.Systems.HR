namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedcommentstoSessionParticipant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SessionParticipant", "Comments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SessionParticipant", "Comments");
        }
    }
}
