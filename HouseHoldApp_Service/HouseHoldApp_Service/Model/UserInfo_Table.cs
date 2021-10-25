using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseHoldApp_Service.Model
{
    public class UserInfo_Table
    {
        [Key]

        public int userId { get; set; }

        public string username { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public string role { get; set; }
    }
}