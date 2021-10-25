using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseHoldRelationShips.Models
{
    public class HouseHoldData
    {
     
      
        public int userID { get; set; }
        [Required(ErrorMessage = "Please Enter First name")]
        [Display(Name = "First Name")]
        [RegularExpression("^[a-zA-Z-'*]{0,32}$", ErrorMessage = "Username should contains alphabets and *,-,' and maximum of 32 characters")]
        public string firstName { get; set; }
        [Display(Name ="Middle Name")]
        [RegularExpression("^[a-zA-Z-'*]{0,32}$", ErrorMessage = "Username should contains alphabets and *,-,' and maximum of 32 characters")]
        public string middleName { get; set; }
        [Required(ErrorMessage = "Please Enter Last name")]
        [Display(Name = "Last Name")]
        [RegularExpression("^[a-zA-Z-'*]{0,32}$", ErrorMessage = "Username should contains alphabets and *,-,' and maximum of 32 characters")]
        public string lastName { get; set; }
        [Display(Name = "Suffix")]
        public string suffix { get; set; }
        [Required(ErrorMessage = "Please Enter Date of Birth")]
        [Display(Name = "Date of Birth(mm/dd/yyyy)")]
        public String dateofBirth { get; set; }
        [Required(ErrorMessage = "Please Select Gender")]
        [Display(Name ="Gender")]

        public string gender { get; set; }
        public string applicationId { get; set; }
        public string status { get; set; }
       
    }
}