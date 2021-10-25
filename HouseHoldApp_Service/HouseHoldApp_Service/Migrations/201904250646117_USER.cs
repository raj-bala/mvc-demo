namespace HouseHoldApp_Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class USER : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Household_Table",
                c => new
                    {
                        userID = c.Int(nullable: false, identity: true),
                        applicationId = c.String(),
                        firstName = c.String(),
                        middleName = c.String(),
                        lastName = c.String(),
                        suffix = c.String(),
                        gender = c.String(),
                        status = c.String(),
                        dateofBirth = c.String(),
                    })
                .PrimaryKey(t => t.userID);
            
            CreateTable(
                "dbo.Relationship_Mapping",
                c => new
                    {
                        relationshipId = c.Int(nullable: false, identity: true),
                        parentUserId = c.Int(nullable: false),
                        relationUserId = c.Int(nullable: false),
                        relationship = c.String(),
                    })
                .PrimaryKey(t => t.relationshipId);
            
            CreateTable(
                "dbo.UserInfo_Table",
                c => new
                    {
                        userId = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        email = c.String(),
                        password = c.String(),
                        role = c.String(),
                    })
                .PrimaryKey(t => t.userId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserInfo_Table");
            DropTable("dbo.Relationship_Mapping");
            DropTable("dbo.Household_Table");
        }
    }
}
