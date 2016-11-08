namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeddescriptionandOpenForExpressionOfInteresttosessionmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Session", "Description", c => c.String());
            AddColumn("dbo.Session", "OpenForExpressionOfInterest", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Session", "OpenForExpressionOfInterest");
            DropColumn("dbo.Session", "Description");
        }
    }
}
