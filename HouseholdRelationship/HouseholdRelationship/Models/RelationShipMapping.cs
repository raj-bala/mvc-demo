using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseHoldRelationShips.Models
{
    public class RelationShipMapping
    {
        [Key]
        public int relationshipId { get; set; }
        public int parentUserId { get; set; }
        public int relationUserId { get; set; }
        public string relationship { get; set; }

    }
}
