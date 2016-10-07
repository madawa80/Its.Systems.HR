namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedtoSessionTags : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SessionTags", newName: "SessionTag");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.SessionTag", newName: "SessionTags");
        }
    }
}
