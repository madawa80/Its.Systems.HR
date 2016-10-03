namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedNamepropfromHrPerson : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.HrPerson", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HrPerson", "Name", c => c.String());
        }
    }
}
