namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reset : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Activity",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        Name = c.String(),
            //    })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.Session",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        Name = c.String(),
            //        StartDate = c.DateTime(),
            //        EndDate = c.DateTime(),
            //        Comments = c.String(),
            //        Evaluation = c.String(),
            //        LocationId = c.Int(),
            //        HrPersonId = c.Int(),
            //        ActivityId = c.Int(nullable: false),
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Participant", t => t.HrPersonId)
            //    .ForeignKey("dbo.Location", t => t.LocationId)
            //    .ForeignKey("dbo.Activity", t => t.ActivityId, cascadeDelete: true)
            //    .Index(t => t.LocationId)
            //    .Index(t => t.HrPersonId)
            //    .Index(t => t.ActivityId);

            //CreateTable(
            //    "dbo.Participant",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        CasId = c.String(),
            //        IsHrPerson = c.Boolean(nullable: false),
            //        IsActive = c.Boolean(nullable: false),
            //        IsSuperAdmin = c.Boolean(nullable: false),
            //        FirstName = c.String(),
            //        LastName = c.String(),
            //        Comments = c.String(),
            //        Wishes = c.String(),
            //    })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.SessionParticipant",
            //    c => new
            //        {
            //            SessionId = c.Int(nullable: false),
            //            ParticipantId = c.Int(nullable: false),
            //            Rating = c.Int(nullable: false),
            //            Comments = c.String(),
            //        })
            //    .PrimaryKey(t => new { t.SessionId, t.ParticipantId })
            //    .ForeignKey("dbo.Participant", t => t.ParticipantId, cascadeDelete: true)
            //    .ForeignKey("dbo.Session", t => t.SessionId, cascadeDelete: true)
            //    .Index(t => t.SessionId)
            //    .Index(t => t.ParticipantId);

            //CreateTable(
            //    "dbo.Location",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        Name = c.String(),
            //        Address = c.String(),
            //    })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.SessionTag",
            //    c => new
            //        {
            //            SessionId = c.Int(nullable: false),
            //            TagId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.SessionId, t.TagId })
            //    .ForeignKey("dbo.Session", t => t.SessionId, cascadeDelete: true)
            //    .ForeignKey("dbo.Tag", t => t.TagId, cascadeDelete: true)
            //    .Index(t => t.SessionId)
            //    .Index(t => t.TagId);

            //CreateTable(
            //    "dbo.Tag",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        Name = c.String(),
            //    })
            //    .PrimaryKey(t => t.Id);

        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Session", "ActivityId", "dbo.Activity");
            //DropForeignKey("dbo.SessionTag", "TagId", "dbo.Tag");
            //DropForeignKey("dbo.SessionTag", "SessionId", "dbo.Session");
            //DropForeignKey("dbo.Session", "LocationId", "dbo.Location");
            //DropForeignKey("dbo.Session", "HrPersonId", "dbo.Participant");
            //DropForeignKey("dbo.SessionParticipant", "SessionId", "dbo.Session");
            //DropForeignKey("dbo.SessionParticipant", "ParticipantId", "dbo.Participant");
            //DropIndex("dbo.SessionTag", new[] { "TagId" });
            //DropIndex("dbo.SessionTag", new[] { "SessionId" });
            //DropIndex("dbo.SessionParticipant", new[] { "ParticipantId" });
            //DropIndex("dbo.SessionParticipant", new[] { "SessionId" });
            //DropIndex("dbo.Session", new[] { "ActivityId" });
            //DropIndex("dbo.Session", new[] { "HrPersonId" });
            //DropIndex("dbo.Session", new[] { "LocationId" });
            //DropTable("dbo.Tag");
            //DropTable("dbo.SessionTag");
            //DropTable("dbo.Location");
            //DropTable("dbo.SessionParticipant");
            //DropTable("dbo.Participant");
            //DropTable("dbo.Session");
            //DropTable("dbo.Activity");
        }
    }
}
