using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseHoldRelationShips.Models
{
    public class UserDetails
    {
        [Key]

        public int userId { get; set; }
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please Enter Username")]
        [RegularExpression("^[a-zA-Z-'*]{0,32}$", ErrorMessage ="Username should contains alphabets and *,-,' and a maximum of 32 characters" )]
        
        public string username { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please Enter Email")]
        [DataType(DataType.EmailAddress)]
        public string  email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please Enter Password")]
        [RegularExpression("^[0-9a-zA-Z]{6,}$", ErrorMessage = "Password Should be of minimum 6 characters")]
        [DataType(DataType.Password)]
        public string  password { get; set; }

        public string role { get; set; }
    }
}