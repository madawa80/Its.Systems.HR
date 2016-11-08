namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class email : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Participant", "Email", c => c.String());
            DropColumn("dbo.Session", "Description");
            DropColumn("dbo.Session", "IsOpenForExpressionOfInterest");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Session", "IsOpenForExpressionOfInterest", c => c.Boolean(nullable: false));
            AddColumn("dbo.Session", "Description", c => c.String());
            DropColumn("dbo.Participant", "Email");
        }
    }
}
