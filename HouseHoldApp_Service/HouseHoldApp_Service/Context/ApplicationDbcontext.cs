using HouseHoldApp_Service.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseHoldApp_Service.Context
{
    public class ApplicationDbcontext: DbContext
    {
        public ApplicationDbcontext() : base("RelationShip")
        {

        }
        public DbSet<Household_Table> houseHoldDatas { get; set; }


        public DbSet<Relationship_Mapping> relationShipMappings { get; set; }

        public DbSet<UserInfo_Table> userDetails { get; set; }

    }
}
