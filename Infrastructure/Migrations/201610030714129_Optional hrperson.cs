namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Optionalhrperson : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TagSessions", newName: "SessionTags");
            DropForeignKey("dbo.Session", "HrPersonId", "dbo.HrPerson");
            DropForeignKey("dbo.Session", "LocationId", "dbo.Location");
            DropIndex("dbo.Session", new[] { "LocationId" });
            DropIndex("dbo.Session", new[] { "HrPersonId" });
            RenameColumn(table: "dbo.SessionTags", name: "Tag_Id", newName: "TagId");
            RenameColumn(table: "dbo.SessionTags", name: "Session_Id", newName: "SessionId");
            RenameIndex(table: "dbo.SessionTags", name: "IX_Session_Id", newName: "IX_SessionId");
            RenameIndex(table: "dbo.SessionTags", name: "IX_Tag_Id", newName: "IX_TagId");
            DropPrimaryKey("dbo.SessionTags");
            AlterColumn("dbo.Session", "LocationId", c => c.Int());
            AlterColumn("dbo.Session", "HrPersonId", c => c.Int());
            AddPrimaryKey("dbo.SessionTags", new[] { "SessionId", "TagId" });
            CreateIndex("dbo.Session", "LocationId");
            CreateIndex("dbo.Session", "HrPersonId");
            AddForeignKey("dbo.Session", "HrPersonId", "dbo.HrPerson", "Id");
            AddForeignKey("dbo.Session", "LocationId", "dbo.Location", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Session", "LocationId", "dbo.Location");
            DropForeignKey("dbo.Session", "HrPersonId", "dbo.HrPerson");
            DropIndex("dbo.Session", new[] { "HrPersonId" });
            DropIndex("dbo.Session", new[] { "LocationId" });
            DropPrimaryKey("dbo.SessionTags");
            AlterColumn("dbo.Session", "HrPersonId", c => c.Int(nullable: false));
            AlterColumn("dbo.Session", "LocationId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.SessionTags", new[] { "Tag_Id", "Session_Id" });
            RenameIndex(table: "dbo.SessionTags", name: "IX_TagId", newName: "IX_Tag_Id");
            RenameIndex(table: "dbo.SessionTags", name: "IX_SessionId", newName: "IX_Session_Id");
            RenameColumn(table: "dbo.SessionTags", name: "SessionId", newName: "Session_Id");
            RenameColumn(table: "dbo.SessionTags", name: "TagId", newName: "Tag_Id");
            CreateIndex("dbo.Session", "HrPersonId");
            CreateIndex("dbo.Session", "LocationId");
            AddForeignKey("dbo.Session", "LocationId", "dbo.Location", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Session", "HrPersonId", "dbo.HrPerson", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.SessionTags", newName: "TagSessions");
        }
    }
}
