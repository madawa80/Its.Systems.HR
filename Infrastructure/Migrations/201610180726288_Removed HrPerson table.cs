namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedHrPersontable : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.HrPerson");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.HrPerson",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
