namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedonenameofsessioncolumntoIsOpenForExpressionOfInterest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Session", "IsOpenForExpressionOfInterest", c => c.Boolean(nullable: false));
            DropColumn("dbo.Session", "OpenForExpressionOfInterest");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Session", "OpenForExpressionOfInterest", c => c.Boolean(nullable: false));
            DropColumn("dbo.Session", "IsOpenForExpressionOfInterest");
        }
    }
}
