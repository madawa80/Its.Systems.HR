namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedindexforTagName : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Tag", "NameIndex");
            AlterColumn("dbo.Tag", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tag", "Name", c => c.String(maxLength: 100));
            CreateIndex("dbo.Tag", "Name", unique: true, name: "NameIndex");
        }
    }
}
