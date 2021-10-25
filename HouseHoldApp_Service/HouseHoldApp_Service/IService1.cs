using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using HouseHoldApp_Service.Model;
namespace HouseHoldApp_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
        [OperationContract]
        List<Household_Table> AddingUsersDetail(List<Household_Table> obj);
        [OperationContract]
        void AddingRelations(List<Relationship_Mapping> obj);

        [OperationContract]
        List<UserSearchInfo_table> SearchingUsersDetail(UserSearchInfo_table obj);
        [OperationContract]
        List<Household_Table> EditingRelationShipDetail(string appId);

        [OperationContract]
        void DeleteUpdatedUserDetails(int id);
        [OperationContract]
        bool AddingUser(UserInfo_Table tbl_UserDetails);


        [OperationContract]
        List<Relationship_Mapping> GettingRelationData(string appId);
        [OperationContract]
        Relationship_Mapping FindExistingRelations(Relationship_Mapping tbl_RelationShip);
        [OperationContract]
        void UpdateExistingUserRelations(Relationship_Mapping tbl_RelationShip);
        [OperationContract]
        List<Data_pdf> ToGetPDFData(string appId);
        [OperationContract]
        string Username(int id);
        [OperationContract]
        List<Household_Table> HouseholdData(string appId);
        [OperationContract]
        List<UserInfo_Table> UserDetail();
        // TODO: Add your service operations here
    }
    [MessageContract]
    public class PDFdata
    {
        [MessageBodyMember]
        public List<Data_pdf> data { get; set; }
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
