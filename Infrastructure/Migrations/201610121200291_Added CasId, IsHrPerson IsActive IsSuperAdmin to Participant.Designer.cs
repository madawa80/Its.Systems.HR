// <auto-generated />
namespace Its.Systems.HR.Infrastructure.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public sealed partial class AddedCasIdIsHrPersonIsActiveIsSuperAdmintoParticipant : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddedCasIdIsHrPersonIsActiveIsSuperAdmintoParticipant));
        
        string IMigrationMetadata.Id
        {
            get { return "201610121200291_Added CasId, IsHrPerson IsActive IsSuperAdmin to Participant"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}