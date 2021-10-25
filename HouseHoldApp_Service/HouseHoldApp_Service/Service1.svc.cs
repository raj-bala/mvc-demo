using HouseHoldApp_Service;
using HouseHoldApp_Service.Context;
using HouseHoldApp_Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
namespace HouseHoldApp_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private ApplicationDbcontext dbContext = new ApplicationDbcontext();
        static List<Household_Table> list = new List<Household_Table>();
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
        public List<Household_Table> AddingUsersDetail(List<Household_Table> obj)
        {
            if (list.Count > 0)
            {
                list.Clear();
            }
            foreach (var c in obj)
            {
                dbContext.houseHoldDatas.Add(c);
                dbContext.SaveChanges();
            }

            foreach (var j in obj)
            {
                var k = dbContext.houseHoldDatas.FirstOrDefault(x => x.firstName == j.firstName && x.gender == j.gender && x.dateofBirth == j.dateofBirth && x.lastName == j.lastName && x.middleName == j.middleName && x.suffix == j.suffix);
                list.Add(k);
            }
            return list;

        }
        public void AddingRelations(List<Relationship_Mapping> obj)
        {
            
                foreach (var c in obj)
                {
                    dbContext.relationShipMappings.Add(c);
                    dbContext.SaveChanges();
                }
               
        }

   
        public List<UserSearchInfo_table> SearchingUsersDetail(UserSearchInfo_table obj)
        {
            var list = dbContext.houseHoldDatas.ToList().Select(x =>

                new UserSearchInfo_table
                {
                    userID = x.userID,
                    firstName = x.firstName,
                    lastName = x.lastName,
                    dateofBirth = x.dateofBirth,
                    applicationId = x.applicationId,
                    gender = x.gender


                }).ToList();



            if (obj.applicationId != null)
            {
                list = list.Where(x => x.applicationId.Contains(obj.applicationId)).Select(y => y).ToList();
            }
            if (obj.dateofBirth != null)
            {
                list = list.Where(x => x.dateofBirth == obj.dateofBirth).Select(y => y).ToList();
            }
            if (obj.firstName != null)
            {
                list = list.Where(x => x.firstName==obj.firstName).Select(y => y).ToList();
            }
            if (obj.lastName != null)
            {
                list = list.Where(x => x.lastName==obj.lastName).Select(y => y).ToList();
            }


            return list;
        }
        public List<Household_Table> EditingRelationShipDetail(string id)
        {
            var list = dbContext.houseHoldDatas.Where(x => x.applicationId == id).ToList().Select(
                x => new Household_Table
                {

                    userID = x.userID,
                    firstName = x.firstName,
                    lastName = x.lastName,
                    dateofBirth = x.dateofBirth,
                    suffix = x.suffix,
                    applicationId = x.applicationId,
                    gender = x.gender


                }).ToList();





            return list;
        }

        public void DeleteUpdatedUserDetails(int id)
        {

            var tbl_HouseHoldData = dbContext.houseHoldDatas.Find(id);
            dbContext.houseHoldDatas.Remove(tbl_HouseHoldData);
            dbContext.SaveChanges();
            var list = dbContext.relationShipMappings.Where(x => x.parentUserId == id || x.relationUserId == id).ToList();
            foreach (Relationship_Mapping c in list)
            {


                {
                    dbContext.relationShipMappings.Remove(c);
                    dbContext.SaveChanges();
                }

            }


        }

        public bool AddingUser(UserInfo_Table tbl_UserDetails)
        {
            bool status = false;
            try
            {
                dbContext.userDetails.Add(tbl_UserDetails);
                dbContext.SaveChanges();
                status = true;
            }
            catch
            {
                status = false;
            }
            return status;

        }
        
        public List<Relationship_Mapping> GettingRelationData(string id)
        {
            var relations = (from household in dbContext.houseHoldDatas join relation in dbContext.relationShipMappings on household.userID equals relation.parentUserId where household.applicationId == id select relation).ToList().Select(x => new Relationship_Mapping
            {
                relationshipId = x.relationshipId,
                parentUserId = x.parentUserId,
                relationship = x.relationship,
                relationUserId = x.relationUserId
            }).ToList();
            return relations;
        }
        public Relationship_Mapping FindExistingRelations(Relationship_Mapping tbl_RelationShip)
        {
            Relationship_Mapping relationShipMapping = dbContext.relationShipMappings.SingleOrDefault(x => x.parentUserId == tbl_RelationShip.parentUserId && x.relationUserId == tbl_RelationShip.relationUserId);
            return relationShipMapping;
        }
        public void UpdateExistingUserRelations(Relationship_Mapping tbl_RelationShip)
        {
            dbContext.Entry(tbl_RelationShip).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChanges();
        }
        public List<Data_pdf> ToGetPDFData(string id)
        {
            var pdfdata = (from household in dbContext.houseHoldDatas
                           join relations in dbContext.relationShipMappings on household.userID equals relations.parentUserId
                           where household.applicationId == id
                           select new Data_pdf
                           {
                               firstName = household.firstName,
                               userId = household.userID,
                               lastName = household.lastName,
                               gender = household.gender,
                               suffix = household.suffix,
                               applicationId = household.applicationId,
                               dateofBirth = household.dateofBirth,
                               middleName = household.middleName,
                               parentUserId = relations.parentUserId,
                               relationship = relations.relationship,
                               relationUserId = relations.relationUserId,

                           }).ToList();
            return pdfdata;
        }
        public string Username(int id)
        {
            Household_Table user = dbContext.houseHoldDatas.Find(id);
            return user.firstName + " " + user.lastName;
        }
        public List<Household_Table> HouseholdData(string id)
        {
            List<Household_Table> houseHoldDatasList = dbContext.houseHoldDatas.Where(x => x.applicationId == id).Select(x => x).ToList();
            return houseHoldDatasList;
        }
        public List<UserInfo_Table> UserDetail()
        {
            var usersList = dbContext.userDetails.ToList();
            return usersList;
        }
    }
}
