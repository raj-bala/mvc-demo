using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseHoldRelationShips.Models
{
    public class UserSearch
    {
        [Display(Name = "First Name")]
        
        public string firstName { get; set; }
        [Display(Name = "Last Name")]
     
        public string lastName { get; set; }
        [Display(Name = "Date of Birth")]
       
        public String dateofBirth { get; set; }
        [Display(Name = "ApplicationId")]
        public string applicationId { get; set; }

    
        public int userID { get; set; }
      
        [Display(Name = "Application Status")]
        public string applicationStatus { get; set; }
        public string gender { get; set; }
    }
}