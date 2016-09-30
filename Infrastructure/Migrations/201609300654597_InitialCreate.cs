namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Session",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        LocationId = c.Int(nullable: false),
                        HrPersonId = c.Int(nullable: false),
                        ActivityId = c.Int(nullable: false),
                        Comments = c.String(),
                        Evaluation = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HrPerson", t => t.HrPersonId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.Activity", t => t.ActivityId, cascadeDelete: true)
                .Index(t => t.LocationId)
                .Index(t => t.HrPersonId)
                .Index(t => t.ActivityId);
            
            CreateTable(
                "dbo.HrPerson",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SessionParticipant",
                c => new
                    {
                        SessionId = c.Int(nullable: false),
                        ParticipantId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SessionId, t.ParticipantId })
                .ForeignKey("dbo.Participant", t => t.ParticipantId, cascadeDelete: true)
                .ForeignKey("dbo.Session", t => t.SessionId, cascadeDelete: true)
                .Index(t => t.SessionId)
                .Index(t => t.ParticipantId);
            
            CreateTable(
                "dbo.Participant",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Comments = c.String(),
                        Wishes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Session", "ActivityId", "dbo.Activity");
            DropForeignKey("dbo.SessionParticipant", "SessionId", "dbo.Session");
            DropForeignKey("dbo.SessionParticipant", "ParticipantId", "dbo.Participant");
            DropForeignKey("dbo.Session", "LocationId", "dbo.Location");
            DropForeignKey("dbo.Session", "HrPersonId", "dbo.HrPerson");
            DropIndex("dbo.SessionParticipant", new[] { "ParticipantId" });
            DropIndex("dbo.SessionParticipant", new[] { "SessionId" });
            DropIndex("dbo.Session", new[] { "ActivityId" });
            DropIndex("dbo.Session", new[] { "HrPersonId" });
            DropIndex("dbo.Session", new[] { "LocationId" });
            DropTable("dbo.Tag");
            DropTable("dbo.Participant");
            DropTable("dbo.SessionParticipant");
            DropTable("dbo.Location");
            DropTable("dbo.HrPerson");
            DropTable("dbo.Session");
            DropTable("dbo.Activity");
        }
    }
}
