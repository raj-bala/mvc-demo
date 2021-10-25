using HouseholdRelationship.ServiceReference1;
using HouseHoldRelationShips.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI.WebControls;
using Font = iTextSharp.text.Font;

namespace HouseHoldRelationShips.Controllers
{

    public class RelationShipController : Controller
    {
        private List<HouseHoldData> householdList = new List<HouseHoldData>();
        private static List<Household_Table> houseList = new List<Household_Table>();
        private static List<Relationship_Mapping> relationShipMappingsList = new List<Relationship_Mapping>();
        public Service1Client service = new Service1Client();
       
        public ActionResult Index()
        {
            Session["list"]=householdList;
            return View();
        }

        [HttpGet]
        public ActionResult CreateMembers()
        {
            return View();
        }

        [HttpPost]
       
        public ActionResult CreateMembers(HouseHoldData obj)
        {
           bool status = false;
            householdList=(List<HouseHoldData>)Session["list"];
            try
            {
                if (householdList.Count>=5)
                {
                    Session["message"] ="Sorry! More than 5 members cannot be added";
                }
                else
                {
                    foreach (HouseHoldData item in householdList)
                    {
                        if (item.firstName==obj.firstName&&item.lastName==obj.lastName)
                        {
                            status=true;
                        }
                    }
                    if (!status)
                    {

                        householdList.Add(obj);
                        Session["list"]=householdList;

                    }

                }

                Session["list"]=householdList;
            }
            catch
            {
                Session["message"] ="Sorry.Not able to process your Request. Please Login again";
                return View("CreateMembers");
            }
            return View();
        }
        public ActionResult AddToDataBase()
        {
            householdList = new List<HouseHoldData>();
            string applicationId = Guid.NewGuid().ToString();
            try
            {
                householdList=(List<HouseHoldData>)Session["list"];
                foreach (var c in householdList)
                {
                    Household_Table tbl_HouseHoldData = new Household_Table();
                    tbl_HouseHoldData.dateofBirth=c.dateofBirth;
                    tbl_HouseHoldData.firstName=c.firstName;
                    tbl_HouseHoldData.gender=c.gender;
                    tbl_HouseHoldData.lastName=c.lastName;
                    tbl_HouseHoldData.suffix=c.suffix;
                    tbl_HouseHoldData.middleName=c.middleName;
                    tbl_HouseHoldData.status="APPROVED";
                    if (c.userID==0&&Session["ApplicationId"]==null)
                    {
                        Session["appId"]=applicationId;
                        tbl_HouseHoldData.applicationId=applicationId;
                    }
                    if (c.userID==0&&Session["ApplicationId"]!=null)
                    {
                        tbl_HouseHoldData.applicationId=(string)Session["ApplicationId"];
                    }
                    if (c.userID==0)
                    {
                        houseList.Add(tbl_HouseHoldData);
                    }


                }

                if (Session["ApplicationId"] == null)
                {
                    Session["HouseholdData"] = service.AddingUsersDetail(houseList).ToList<Household_Table>();
                }
                else
                {
                    List<Household_Table> editList = new List<Household_Table>();
                    editList = service.EditingRelationShipDetail((string)(Session["ApplicationId"]));
                    foreach (var c in service.AddingUsersDetail(houseList).ToList<Household_Table>())
                    {
                        editList.Add(c);
                    }
                    Session["HouseholdData"] = editList;
                }
                    householdList.Clear();
                    houseList.Clear();
            }
            catch (Exception ex)
            {
                if (householdList==null||houseList==null)
                {
                    return RedirectToAction("CreateMembers");
                }
                else
                {
                    Session["message"] ="Sorry. Server Issue. Please try after sometime";
                    return RedirectToAction("CreateMembers");
                }
            }
            return RedirectToAction("RelationshipMapping", "RelationShip");

        }

        public ActionResult RelationshipMapping()
        {
            List<Household_Table> _tblList = new List<Household_Table>();
            try
            {
                _tblList=(List<Household_Table>)Session["HouseholdData"];
            }
            catch (Exception)
            {
                if (_tblList==null)
                {
                    _tblList=new List<Household_Table>();
                    return RedirectToAction("SearchUserDetails", "RelationShip");
                }
                else
                {
                    Session["message"] ="Sorry. Server Issue. Please try after sometime";

                }
            }

            return View(_tblList);
        }

        [HttpPost]
        public ActionResult AddRelationShips()
        {
            List<Household_Table> dblist = new List<Household_Table>();
            dblist = (List<Household_Table>)Session["HouseholdData"];
            try
            {
                string param = string.Empty;

                foreach (var relation in dblist)
                {

                    foreach (var relations in dblist)
                    {
                        if (relation.userID != relations.userID)
                        {
                            Relationship_Mapping tbl_RelationsChild = new Relationship_Mapping();

                            tbl_RelationsChild.parentUserId = relation.userID;
                            tbl_RelationsChild.relationUserId = relations.userID;
                            param = relation.userID.ToString() + relations.userID;
                            tbl_RelationsChild.relationship = Request.Form.Get(param);
                            var previousRelationShipdata = service.FindExistingRelations(tbl_RelationsChild);
                            if (previousRelationShipdata == null)
                            {
                                relationShipMappingsList.Add(tbl_RelationsChild);
                            }
                            else
                            {
                                previousRelationShipdata.relationship = Request.Form.Get(param);
                                service.UpdateExistingUserRelations(previousRelationShipdata);
                            }

                        }
                    }

                }
                service.AddingRelations(relationShipMappingsList);
                Session.Remove("HouseholdData");
                relationShipMappingsList.Clear();
                GetPdFData((string)Session["appId"]);
            }
            catch
            {
                Session["message"] = "Sorry. Server Issue. Please try after sometime";
                return View("RelationshipMapping");
            }
            return View();
        }

        public ActionResult DeleteData(HouseHoldData data)
        {
            try
            {
                householdList=(List<HouseHoldData>)Session["list"];
                if (data.userID!=0)
                {
                    service.DeleteUpdatedUserDetails(data.userID);
                }
                householdList.Remove(householdList.Find(x => x.firstName==data.firstName&&x.dateofBirth==data.dateofBirth&&x.gender==data.gender&&x.lastName==data.lastName&&x.suffix==data.suffix&&x.middleName==data.middleName&&x.userID==data.userID));

                Session["list"]=householdList;

            }
            catch
            {
                if (householdList==null)
                {
                    if (data.userID!=0)
                    {
                        Session["message"] ="Sorry. Server Issue. Please try after sometime";

                    }
                    else
                    {
                        return RedirectToAction("CreateMembers");
                    }
                }
                else
                {
                    Session["message"] ="Sorry. Server Issue. Please try after sometime";
                }
            }
            return View("CreateMembers");
        }

        
        public ActionResult SearchUserDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchUserDetails(UserSearch user)
        {
            List<UserSearchInfo_table> searchResults = new List<UserSearchInfo_table>();
            try
            {
                if ((user.dateofBirth!=null||user.firstName!=null||user.lastName!=null||user.applicationId!=null))
                {
                    UserSearchInfo_table search = new UserSearchInfo_table();
                    search.applicationId=user.applicationId;
                    search.dateofBirth=user.dateofBirth;
                    search.firstName=user.firstName;
                    search.lastName=user.lastName;
                    searchResults=service.SearchingUsersDetail(search);
                    Session["UserSearch"]=searchResults;
                    if (searchResults.Count==0)
                    {
                        Session["message"]="No Details Found";
                        Session.Clear();
                    }
                    if (searchResults.Count>=100)
                    {
                        Session["UserSearch"]=new List<UserSearchInfo_table>();
                        Session["message"]="Please refine your search criteria, more than 100 results are found";
                    }

                }
                else
                {
                    Session["UserSearch"]=new List<UserSearchInfo_table>();
                    Session["message"]="Please select atleast one field";
                }
            }
          
            catch (Exception ex)
            {
                    Session["message"] ="Sorry. Server Issue. Please try after sometime";
                    return View();
              
            }
            return View();
        }
        public ActionResult GetRelations()
        {

            var usersList = service.GettingRelationData((string)Session["ApplicationId"]);
            return Json(usersList, JsonRequestBehavior.AllowGet);

        }
        public ActionResult EditRelationShipDetails(UserSearchInfo_table data)
        {
            try
            {
                List<HouseHoldData> listHouseHoldData = new List<HouseHoldData>();
                string applicationId = data.applicationId;
                var tbl_HouseHoldDatasList = service.EditingRelationShipDetail(applicationId);
                foreach (var obj in tbl_HouseHoldDatasList)
                {
                    HouseHoldData houseHoldData = new HouseHoldData();
                    houseHoldData.firstName=obj.firstName;
                    houseHoldData.gender=obj.gender;
                    houseHoldData.userID=obj.userID;
                    houseHoldData.dateofBirth=obj.dateofBirth;
                    houseHoldData.suffix=obj.suffix;
                    houseHoldData.lastName=obj.lastName;
                    houseHoldData.applicationId=obj.applicationId;
                    listHouseHoldData.Add(houseHoldData);
                }
                Session["list"]=listHouseHoldData;
                Session["ApplicationId"]=data.applicationId;
            }
            catch
            {
                Session["message"] ="Sorry. Server Issue. Please try after sometime";
                return View("SearchUserDetails");
            }
            return View("CreateMembers");
        }
      
        public ActionResult SaveAndExit()
        {
            string resultantView = string.Empty;
            try
            {
                string appId = Guid.NewGuid().ToString();
                householdList=(List<HouseHoldData>)Session["list"];
                foreach (var c in householdList)
                {
                    Household_Table tbl_HouseHoldData = new Household_Table();
                    tbl_HouseHoldData.dateofBirth=c.dateofBirth;
                    tbl_HouseHoldData.firstName=c.firstName;
                    tbl_HouseHoldData.gender=c.gender;
                    tbl_HouseHoldData.lastName=c.lastName;
                    tbl_HouseHoldData.suffix=c.suffix;
                    tbl_HouseHoldData.middleName=c.middleName;
                    tbl_HouseHoldData.status="Approved";
                    if (c.userID==0&&Session["ApplicationId"]==null)
                    {
                        tbl_HouseHoldData.applicationId=appId;
                    }
                    if (c.userID==0&&Session["ApplicationId"]!=null)
                    {
                        tbl_HouseHoldData.applicationId=(string)Session["ApplicationId"];
                    }
                    if (c.userID==0)
                    {
                        houseList.Add(tbl_HouseHoldData);
                    }
                    
                }

                if (Session["ApplicationId"]==null)
                {
                    Session["HouseholdData"]=service.AddingUsersDetail(houseList).ToList();
                }
                else
                {
                    List<Household_Table> editList = new List<Household_Table>();
                    editList=service.EditingRelationShipDetail((string)(Session["ApplicationId"]));
                    foreach (var c in service.AddingUsersDetail(houseList).ToList<Household_Table>())
                    {
                        editList.Add(c);
                    }
                    Session["HouseholdData"]=editList;
                }
                houseList.Clear();

                relationShipMappingsList.Clear();


                if ((string)Session["role"]=="Admin")
                {
                    resultantView= "SearchUserDetails";
                }
                else
                {
                    resultantView="CreateMembers";
                }
            }
            catch
            {
                Session["message"] ="Sorry. Server Issue. Please try after sometime";
                return View("CreateMembers");
            }
            return RedirectToAction(resultantView);
        }
        public ActionResult SaveAndExitMappings()
        {
            string view = string.Empty;
            try
            {
                List<Household_Table> dblist = (List<Household_Table>)Session["HouseholdData"];

                foreach (var relation in dblist)
                {

                    foreach (var relations in dblist)
                    {
                        if (relation.userID!=relations.userID)
                        {
                            Relationship_Mapping tbl_RelationsChild = new Relationship_Mapping();

                            tbl_RelationsChild.parentUserId=relation.userID;
                            tbl_RelationsChild.relationUserId=relations.userID;
                            string param = relation.userID.ToString()+relations.userID;
                            tbl_RelationsChild.relationship=Request.Form.Get(param);
                            var olddata = service.FindExistingRelations(tbl_RelationsChild);
                            if (olddata==null)
                            {
                                relationShipMappingsList.Add(tbl_RelationsChild);
                            }
                            else
                            {
                                olddata.relationship=Request.Form.Get(param);
                                service.UpdateExistingUserRelations(olddata);
                            }

                        }
                    }

                }
                service.AddingRelations(relationShipMappingsList);

                relationShipMappingsList.Clear();

                if ((string)Session["role"]=="Admin")
                {
                    view= "SearchUserDetails";
                }
                else
                {
                    view= "CreateMembers";
                }
            }
            catch
            {
                Session["message"] ="Sorry. Server Issue. Please try after sometime";
                return View("RelationshipMapping");
            }
            return RedirectToAction(view);
        }
        public void GetPdFData(string param)
        {
           
            if ((string)(Session["ApplicationId"])==null)
            {
                Session["pdfdata"]=service.ToGetPDFData(param);
                Session["houseHoldList"]=service.HouseholdData(param);
            }
            else
            {
                Session["pdfdata"]=service.ToGetPDFData((string)(Session["ApplicationId"]));
                Session["houseHoldList"]=service.HouseholdData((string)(Session["ApplicationId"]));
            }

        }
        public void GeneratePDF()
        {
            if ((Session["pdfdata"]==null||Session["houseHoldList"]==null))
            {
                Session["sessionTimeOut"]="Please try again to get receipt";
            }
            else
            {
                try
                {
                    List<Data_pdf> pdflist = (List<Data_pdf>)Session["pdfdata"];
                    List<Household_Table> houseHoldDatasList = (List<Household_Table>)Session["houseHoldList"];
                    Document pdfDocument = new Document(PageSize.A4, 25, 25, 25, 15);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDocument, Response.OutputStream);
                    pdfDocument.Open();

                    Paragraph para = new Paragraph();
                    para.Add("\t\t Your ApplicationId is "+pdflist[0].applicationId);
                    pdfDocument.Add(para);


                    Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    pdfDocument.Add(line);
                    PdfPTable table = new PdfPTable(2);
                    table.WidthPercentage=100;
                    table.HorizontalAlignment=1;
                    table.SpacingBefore=20f;
                    table.SpacingAfter=30f;
                    PdfPCell cell = new PdfPCell();


                    table=new PdfPTable(6);
                    table.WidthPercentage=100;
                    table.HorizontalAlignment=0;
                    table.SpacingBefore=20f;
                    table.SpacingAfter=30f;
                    cell=new PdfPCell(new Phrase("First Name", new Font(Font.NORMAL, 12f, Font.BOLD, BaseColor.WHITE)));
                    cell.HorizontalAlignment=Element.ALIGN_CENTER;
                    cell.BackgroundColor=new BaseColor(0, 0, 150);
                    cell.FixedHeight=10;
                    cell.BorderColor=new BaseColor(255, 242, 0);
                    table.AddCell(cell);
                    cell=new PdfPCell(new Phrase("Middle Name", new Font(Font.NORMAL, 12f, Font.BOLD, BaseColor.WHITE)));
                    cell.HorizontalAlignment=Element.ALIGN_CENTER;
                    cell.BackgroundColor=new BaseColor(0, 0, 150);
                    cell.FixedHeight=10;
                    cell.BorderColor=new BaseColor(255, 242, 0);
                    table.AddCell(cell);
                    cell=new PdfPCell(new Phrase("Last Name", new Font(Font.NORMAL, 12f, Font.BOLD, BaseColor.WHITE)));

                    cell.BackgroundColor=new BaseColor(0, 0, 150);
                    cell.HorizontalAlignment=Element.ALIGN_CENTER;
                    cell.BorderColor=new BaseColor(255, 242, 0);
                    table.AddCell(cell);
                    cell=new PdfPCell(new Phrase("Suffix", new Font(Font.NORMAL, 12f, Font.BOLD, BaseColor.WHITE)));

                    cell.BackgroundColor=new BaseColor(0, 0,150);
                    cell.HorizontalAlignment=Element.ALIGN_CENTER;
                    cell.BorderColor=new BaseColor(255, 242, 0);
                    table.AddCell(cell);
                    cell=new PdfPCell(new Phrase("Gender", new Font(Font.NORMAL, 12f, Font.BOLD, BaseColor.WHITE)));

                    cell.BackgroundColor=new BaseColor(0, 0, 150);
                    cell.HorizontalAlignment=Element.ALIGN_CENTER;
                    cell.BorderColor=new BaseColor(255, 242, 0);
                    table.AddCell(cell);
                    cell=new PdfPCell(new Phrase("Date of Birth", new Font(Font.NORMAL, 12f, Font.BOLD, BaseColor.WHITE)));

                    cell.BackgroundColor=new BaseColor(0, 0, 150);
                    cell.HorizontalAlignment=Element.ALIGN_CENTER;
                    cell.BorderColor=new BaseColor(255, 242, 0);
                    table.AddCell(cell);


                    foreach (var item in houseHoldDatasList)
                    {
                        cell=new PdfPCell(new Phrase(item.firstName, new Font(Font.NORMAL, 10f, Font.BOLD, BaseColor.BLACK)));


                        cell.HorizontalAlignment=Element.ALIGN_CENTER;

                        table.AddCell(cell);
                        cell=new PdfPCell(new Phrase(item.middleName, new Font(Font.NORMAL, 10f, Font.BOLD, BaseColor.BLACK)));


                        cell.HorizontalAlignment=Element.ALIGN_CENTER;

                        table.AddCell(cell);
                        cell=new PdfPCell(new Phrase(item.lastName, new Font(Font.NORMAL, 10f, Font.BOLD, BaseColor.BLACK)));


                        cell.HorizontalAlignment=Element.ALIGN_CENTER;

                        table.AddCell(cell);
                        cell=new PdfPCell(new Phrase(item.suffix, new Font(Font.NORMAL, 10f, Font.BOLD, BaseColor.BLACK)));


                        cell.HorizontalAlignment=Element.ALIGN_CENTER;

                        table.AddCell(cell);
                        cell=new PdfPCell(new Phrase(item.gender, new Font(Font.NORMAL, 10f, Font.BOLD, BaseColor.BLACK)));


                        cell.HorizontalAlignment=Element.ALIGN_CENTER;

                        table.AddCell(cell);
                        cell=new PdfPCell(new Phrase(item.dateofBirth, new Font(Font.NORMAL, 10f, Font.BOLD, BaseColor.BLACK)));


                        cell.HorizontalAlignment=Element.ALIGN_CENTER;

                        table.AddCell(cell);


                    }
                    pdfDocument.Add(table);
                    Chunk chunk1 = new Chunk("RelationShip Details", FontFactory.GetFont("Arial", 20, Font.NORMAL, BaseColor.BLACK));
                    pdfDocument.Add(chunk1);
                    pdfDocument.Add(line);
                    table=new PdfPTable(3);
                    table.WidthPercentage=100;
                    table.HorizontalAlignment=0;
                    table.SpacingBefore=20f;
                    table.SpacingAfter=30f;
                    cell=new PdfPCell(new Phrase("Name", new Font(Font.NORMAL, 12f, Font.BOLD, BaseColor.WHITE)));

                    cell.BackgroundColor=new BaseColor(0, 0, 150);
                    cell.HorizontalAlignment=Element.ALIGN_CENTER;
                    cell.BorderColor=new BaseColor(255, 242, 0);
                    table.AddCell(cell);
                    cell=new PdfPCell(new Phrase("Relation Type", new Font(Font.NORMAL, 12f, Font.BOLD, BaseColor.WHITE)));

                    cell.BackgroundColor=new BaseColor(0, 0, 150);
                    cell.HorizontalAlignment=Element.ALIGN_CENTER;
                    cell.BorderColor=new BaseColor(255, 242, 0);
                    table.AddCell(cell);

                    cell=new PdfPCell(new Phrase("Relative Name", new Font(Font.NORMAL, 12f, Font.BOLD, BaseColor.WHITE)));

                    cell.BackgroundColor=new BaseColor(0, 0, 150);
                    cell.HorizontalAlignment=Element.ALIGN_CENTER;
                    cell.BorderColor=new BaseColor(255, 242, 0);
                    table.AddCell(cell);


                    foreach (var item in pdflist)
                    {
                        cell=new PdfPCell(new Phrase(item.firstName+" "+item.lastName, new Font(Font.NORMAL, 10f, Font.BOLD, BaseColor.BLACK)));


                        cell.HorizontalAlignment=Element.ALIGN_CENTER;

                        table.AddCell(cell);

                        cell=new PdfPCell(new Phrase(item.relationship, new Font(Font.NORMAL, 10f, Font.BOLD, BaseColor.BLACK)));


                        cell.HorizontalAlignment=Element.ALIGN_CENTER;

                        table.AddCell(cell);

                        cell=new PdfPCell(new Phrase(service.Username(item.relationUserId), new Font(Font.NORMAL, 10f, Font.BOLD, BaseColor.BLACK)));


                        cell.HorizontalAlignment=Element.ALIGN_CENTER;

                        table.AddCell(cell);


                    }
                    pdfDocument.Add(table);


                    line=new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    pdfDocument.Add(line);

                    pdfWriter.CloseStream=false;
                    pdfDocument.Close();
                    Response.Buffer=true;
                    Response.ContentType="application/pdf";
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", pdflist[0].applicationId.ToUpper()+" "+"Details"+"-"+".pdf"));
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDocument);
                    Response.End();
                }
                catch
                { 
                    Session["message"] ="Sorry. Server Issue. Please try after sometime";
                }
            }
        }
    }
}
