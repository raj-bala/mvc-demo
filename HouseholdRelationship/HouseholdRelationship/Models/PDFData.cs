using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseHoldRelationShips.Models
{
    public class PDFData
    {
        public int userId { get; set; }

        public string firstName { get; set; }

        public string middleName { get; set; }

        public string lastName { get; set; }

        public string suffix { get; set; }

        public string dateofBirth { get; set; }

        public string gender { get; set; }

        public string status { get; set; }
        public int parentUserId { get; set; }
        public int relationUserId { get; set; }
        public string relationship { get; set; }
        public string applicationId { get; set; }
    }
}