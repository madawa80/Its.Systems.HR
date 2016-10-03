namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedrelationshipfortagsandremovedrequiredonhrperson : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TagSessions",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Session_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Session_Id })
                .ForeignKey("dbo.Tag", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Session", t => t.Session_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Session_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagSessions", "Session_Id", "dbo.Session");
            DropForeignKey("dbo.TagSessions", "Tag_Id", "dbo.Tag");
            DropIndex("dbo.TagSessions", new[] { "Session_Id" });
            DropIndex("dbo.TagSessions", new[] { "Tag_Id" });
            DropTable("dbo.TagSessions");
        }
    }
}
