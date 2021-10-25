using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseHoldApp_Service.Model
{
    public class Relationship_Mapping
    {
        [Key]
        public int relationshipId { get; set; }
        public int parentUserId { get; set; }
        public int relationUserId { get; set; }
        public string relationship { get; set; }

    }
}