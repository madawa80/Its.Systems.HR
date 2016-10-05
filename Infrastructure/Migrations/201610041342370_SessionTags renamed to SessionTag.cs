namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SessionTagsrenamedtoSessionTag : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SessionTags", newName: "SessionTag");
            AlterColumn("dbo.Tag", "Name", c => c.String(maxLength: 100));
            CreateIndex("dbo.Tag", "Name", unique: true, name: "NameIndex");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tag", "NameIndex");
            AlterColumn("dbo.Tag", "Name", c => c.String());
            RenameTable(name: "dbo.SessionTag", newName: "SessionTags");
        }
    }
}
