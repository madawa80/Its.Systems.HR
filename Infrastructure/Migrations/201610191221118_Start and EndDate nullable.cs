namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StartandEndDatenullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Session", "StartDate", c => c.DateTime());
            AlterColumn("dbo.Session", "EndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Session", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Session", "StartDate", c => c.DateTime(nullable: false));
        }
    }
}
