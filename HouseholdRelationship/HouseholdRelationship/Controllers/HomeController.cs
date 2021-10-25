using HouseholdRelationship.ServiceReference1;
using HouseHoldRelationShips.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace HouseHoldRelationShips.Controllers
{
 
    public class HomeController : Controller
    {
        public Service1Client service = new Service1Client();
        [HttpGet]
        public ActionResult Welcome()
        {
            Session.RemoveAll();
            Session.Clear();
            try
            {
                Session["userList"] = service.UserDetail();
            }
            catch
            {
                Session["message"] = "Sorry. Server Issue. Please try after sometime";
            }

            return RedirectToAction("LogIn", "Home");
        }

        public ActionResult SignUp()
        {
            return View();
        }
  
        [HttpPost]
        public ActionResult SignUp(UserDetails userDetails)
        {
            try
            {
                UserInfo_Table user = new UserInfo_Table();
                string confirmPassword = Request.Form.Get("confirmPassword");
                user.password=userDetails.password;
                bool usernameExists = CheckExistingUserDetails(userDetails.username);
                if (usernameExists)
                {
                    Session["message"]="Sorry! Username is already existing.Please try with another username";
                }
                else
                {
                    if (CheckPasswords(user.password, confirmPassword))
                    {
                        user.email=userDetails.email;
                        user.username=userDetails.username;
                        user.role="User";
                       
                        if (service.AddingUser(user))
                        {
                            Session["message"]="Congratulations. Your Account is successfully Created";
                        }
                        else
                        {
                            Session["message"]="Sorry! Please try after sometime";
                        }

                    }
                    else
                    {
                        Session["message"] ="Password and ConfirmPassword didn't match";
                    }
                }
            }
            catch
            {
                Session["message"]="Sorry. Server Issue. Please try after sometime";
            }
            return View();
        }

        private bool CheckPasswords(string password, string confirmPassword)
        {
            return password.Equals(confirmPassword);
        }
        
        [HttpGet]
      
        public ActionResult LogIn()
        {
            Session.Abandon();
            return View();
        }
        [HttpPost]
     
        public ActionResult LogIn(UserDetails user)
        {
            try
            {
                Session["userList"]=service.UserDetail();
                user.username=Request.Form.Get("inputUsername");
                user.password=Request.Form.Get("inputPassword");
                List<UserInfo_Table> userDetailsList = (List<UserInfo_Table>)Session["userList"];

               UserInfo_Table  loggedUser=   userDetailsList.Find(x => x.password==user.password&&x.username==user.username);
               if(loggedUser==null)
                {
                    Session["message"]="Invalid Credentials";
                }
              else
                {
                    Session["message"] = string.Empty;
                    var status = loggedUser.role;
                    if (status=="Admin")
                    {
                        Session["user"]=user.username;
                        Session["role"]="Admin";
                        return RedirectToAction("Index", "RelationShip");
                    }
                    if (status=="User")
                    {
                        Session["user"]=user.username;
                        Session["role"]="User";
                        return RedirectToAction("Index", "RelationShip");
                    }
                }
            }
            catch
            {
                Session["message"]="Sorry. Please try after sometime";
            }
            return View();
        }

        private bool CheckExistingUserDetails(string username)
        {
            List<UserInfo_Table> userDetailsList = (List<UserInfo_Table>)Session["userList"];
            return userDetailsList.Exists(x => x.username==username);           
                                  
        }
    }
}