namespace HouseHoldApp_Service.Migrations
{
    using HouseHoldApp_Service.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HouseHoldApp_Service.Context.ApplicationDbcontext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HouseHoldApp_Service.Context.ApplicationDbcontext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            UserInfo_Table admin = new UserInfo_Table();
            admin.password = "123";
            admin.role = "Admin";
            admin.username = "Admin";
            context.userDetails.Add(admin);
            context.SaveChanges();
        }
    }
}
