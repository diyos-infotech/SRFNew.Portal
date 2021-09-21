using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Text;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class EmpIDCard : System.Web.UI.Page
    {
        DataTable dt;
        string EmpIDPrefix = "";
        string fontsyle = "verdana";
        string CmpIDPrefix = "";

        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                GetWebConfigdata();
                if (!IsPostBack)
                {
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {
                        BindData();
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }


                }
            }
            catch (Exception ex)
            {
                GoToLoginPage();
            }


        }

        protected void BindData()
        {

            string Qry = "select empid,(empid+' - '+empfname+' '+empmname+' '+emplname) as empname from empdetails";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Qry).Result;

            if (dt.Rows.Count > 0)
            {
                lstEmpIdName.DataSource = dt;
                lstEmpIdName.DataTextField = "empname";
                lstEmpIdName.DataValueField = "empid";
                lstEmpIdName.DataBind();
            }
        }

        public void GoToLoginPage()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
            Response.Redirect("~/login.aspx");
        }

        protected void GetWebConfigdata()
        {
            if (Session.Keys.Count > 0)
            {
                EmpIDPrefix = Session["EmpIDPrefix"].ToString();
                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void GvSearchEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvSearchEmp.PageIndex = e.NewPageIndex;
            //  Bindata();
        }

        protected void BtnIDCard_Click(object sender, EventArgs e)
        {
            int Fontsize = 10;
            int fontsize2 = 6;// font for company address

            int fontsize1 = 8;
            string fontstyle = "Calibri";

            List<String> EmpId_list = new List<string>();




            var list = new List<string>();

            for (int i = 0; i < lstEmpIdName.Items.Count; i++)
            {
                //CheckBox chkempid = GvSearchEmp.Rows[i].FindControl("chkindividual") as CheckBox;
                //Label lblempid = GvSearchEmp.Rows[i].FindControl("lblempid") as Label;

                if (lstEmpIdName.Items[i].Selected == true)
                {
                    list.Add("'" + lstEmpIdName.Items[i].Value + "'");
                }
            }


            string empids = string.Join(",", list.ToArray());



            #region for Variable Declaration

            string Empid = "";
            string Name = "";
            string Designation = "";
            string IDcardIssued = "";
            string IDcardvalid = "";
            string BloodGroup = "";
            string Image = "";
            string EmpSign = "";


            #endregion for Variable Declaration


            string QueryCompanyInfo = "select * from companyinfo";
            DataTable DtCompanyInfo = config.ExecuteAdaptorAsyncWithQueryParams(QueryCompanyInfo).Result;

            string CompanyName = "";
            string Address = "";

            if (DtCompanyInfo.Rows.Count > 0)
            {
                CompanyName = DtCompanyInfo.Rows[0]["CompanyName"].ToString();
                Address = DtCompanyInfo.Rows[0]["Address"].ToString();


            }


            string query = "";
            DataTable dt = new DataTable();




            query = "select Empid,(EmpFName+' '+EmpMName+''+EmpLName) as Fullname,D.Design as EmpDesgn,convert(varchar(10),IDCardIssued,103) as IDCardIssued,convert(varchar(10),IDCardValid,103) as IDCardValid,Image,EmpSign,BN.BloodGroupName as EmpBloodGroup from EmpDetails " +
                         " inner join designations D on D.Designid=EmpDetails.EmpDesgn " +
                         " left join BloodGroupNames BN on BN.BloodGroupId=EmpDetails.EmpBloodGroup " +
                         " where empid  in (" + empids + ")  order by empid";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;





            if (dt.Rows.Count > 0)
            {



                MemoryStream ms = new MemoryStream();

                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("assets/EmpPhotos/");
                string imagepath2 = Server.MapPath("assets/Images/");
                string imagepath3 = Server.MapPath("assets/EmpSign/");


                Document document = new Document(PageSize.A4);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                #region for range ID Card Display

                PdfPTable IDCarddetails = new PdfPTable(10);
                IDCarddetails.TotalWidth = 370f;
                IDCarddetails.LockedWidth = true;
                float[] width = new float[] { 5f, 2f, 2f, 2f, 0.2f, 5f, 2f, 2f, 2f, 2.4f };
                IDCarddetails.SetWidths(width);



                for (int k = 0; k < dt.Rows.Count; k++)
                {



                    Empid = dt.Rows[k]["Empid"].ToString();
                    Name = dt.Rows[k]["Fullname"].ToString();
                    Designation = dt.Rows[k]["EmpDesgn"].ToString();
                    IDcardIssued = dt.Rows[k]["IDCardIssued"].ToString();
                    IDcardvalid = dt.Rows[k]["IDCardValid"].ToString();
                    BloodGroup = dt.Rows[k]["EmpBloodGroup"].ToString();
                    Image = dt.Rows[k]["Image"].ToString();
                    EmpSign = dt.Rows[k]["EmpSign"].ToString();




                    PdfPTable IDCardTemptable1 = new PdfPTable(4);
                    IDCardTemptable1.TotalWidth = 185f;
                    // IDCardTemptable1.HorizontalAlignment = 0;
                    IDCardTemptable1.LockedWidth = true;
                    float[] width1 = new float[] { 2.4f, 2.4f, 2.4f, 2.4f };
                    IDCardTemptable1.SetWidths(width1);


                    iTextSharp.text.Image srflogo = iTextSharp.text.Image.GetInstance(imagepath2 + "/srflogo.png");
                    srflogo.ScaleAbsolute(160f, 50f);
                    PdfPCell companylogo = new PdfPCell();
                    Paragraph cmplogo = new Paragraph();
                    cmplogo.Add(new Chunk(srflogo, 38f, 0));
                    companylogo.AddElement(cmplogo);
                    companylogo.HorizontalAlignment = 0;
                    companylogo.Colspan = 4;
                    companylogo.PaddingLeft = -19;
                    companylogo.Border = 0;
                    IDCardTemptable1.AddCell(companylogo);

                    BaseColor color = new BaseColor(255, 0, 0);
                    PdfPCell cellCertification = new PdfPCell(new Phrase("ISO 9001-2015", FontFactory.GetFont(fontstyle, Fontsize, Font.BOLD, color)));
                    cellCertification.HorizontalAlignment = 1;
                    cellCertification.Border = 0;
                    cellCertification.Colspan = 4;
                    cellCertification.PaddingTop = -10;
                    cellCertification.PaddingLeft = 20;
                    IDCardTemptable1.AddCell(cellCertification);

                    if (Image.Length > 0)
                    {
                        iTextSharp.text.Image Empphoto = iTextSharp.text.Image.GetInstance(imagepath1 + Image);
                        //Empphoto.ScalePercent(25f);
                        Empphoto.ScaleAbsolute(80f, 80f);
                        PdfPCell EmpImage = new PdfPCell();
                        Paragraph Emplogo = new Paragraph();
                        Emplogo.Add(new Chunk(Empphoto, 70f, 0));
                        EmpImage.AddElement(Emplogo);
                        EmpImage.HorizontalAlignment = 1;
                        EmpImage.PaddingLeft = -15f;
                        EmpImage.Colspan = 4;
                        EmpImage.Border = 0;
                        IDCardTemptable1.AddCell(EmpImage);
                    }
                    else
                    {
                        PdfPCell EmpImage = new PdfPCell();
                        EmpImage.HorizontalAlignment = 2;
                        EmpImage.Colspan = 4;
                        EmpImage.Border = 0;
                        IDCardTemptable1.AddCell(EmpImage);

                    }

                    PdfPCell cellName = new PdfPCell(new Phrase(Name, FontFactory.GetFont(fontstyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellName.HorizontalAlignment = 1;
                    cellName.Border = 0;
                    cellName.Colspan = 4;
                    IDCardTemptable1.AddCell(cellName);

                    PdfPCell cellIDNo = new PdfPCell(new Phrase("P.No : " + Empid, FontFactory.GetFont(fontstyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellIDNo.HorizontalAlignment = 1;
                    cellIDNo.Border = 0;
                    cellIDNo.Colspan = 4;
                    IDCardTemptable1.AddCell(cellIDNo);

                    PdfPCell cellRank = new PdfPCell(new Phrase("Rank : " + Designation, FontFactory.GetFont(fontstyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellRank.HorizontalAlignment = 1;
                    cellRank.Border = 0;
                    cellRank.Colspan = 4;
                    IDCardTemptable1.AddCell(cellRank);

                    PdfPCell cellDtIssued = new PdfPCell(new Phrase("Date of Issue : " + IDcardIssued, FontFactory.GetFont(fontstyle, Fontsize - 2, Font.NORMAL, BaseColor.BLACK)));
                    cellDtIssued.HorizontalAlignment = 1;
                    cellDtIssued.Border = 0;
                    cellDtIssued.Colspan = 4;
                    IDCardTemptable1.AddCell(cellDtIssued);

                    PdfPCell cellDtValid = new PdfPCell(new Phrase("Valid Upto : " + IDcardvalid, FontFactory.GetFont(fontstyle, Fontsize - 2, Font.NORMAL, BaseColor.BLACK)));
                    cellDtValid.HorizontalAlignment = 1;
                    cellDtValid.Border = 0;
                    cellDtValid.Colspan = 4;
                    IDCardTemptable1.AddCell(cellDtValid);

                    if (EmpSign.Length > 0)
                    {
                        iTextSharp.text.Image Sign = iTextSharp.text.Image.GetInstance(imagepath3 + EmpSign);
                        //Sign.ScalePercent(10f);
                        Sign.ScaleAbsolute(50f, 8f);
                        PdfPCell Signature = new PdfPCell();
                        Paragraph signlogo = new Paragraph();
                        signlogo.Add(new Chunk(Sign, 25f, 0));
                        Signature.AddElement(signlogo);
                        Signature.HorizontalAlignment = 1;
                        Signature.PaddingLeft = 5f;
                        Signature.Colspan = 2;
                        Signature.PaddingTop = -2f;
                        Signature.Border = 0;
                        IDCardTemptable1.AddCell(Signature);
                    }
                    else
                    {

                        PdfPCell Signature = new PdfPCell();
                        Signature.HorizontalAlignment = 1;
                        Signature.Colspan = 2;
                        Signature.PaddingTop = -2f;
                        Signature.Border = 0;
                        Signature.FixedHeight = 15;
                        IDCardTemptable1.AddCell(Signature);

                    }

                    iTextSharp.text.Image IssuingAuth = iTextSharp.text.Image.GetInstance(imagepath2 + "Authority.png");
                    //IssuingAuth.ScalePercent(10f);
                    IssuingAuth.ScaleAbsolute(60f, 8f);
                    PdfPCell Authority = new PdfPCell();
                    Paragraph Authoritylogo = new Paragraph();
                    Authoritylogo.Add(new Chunk(IssuingAuth, 10f, 0));
                    Authority.AddElement(Authoritylogo);
                    Authority.HorizontalAlignment = 0;
                    Authority.Colspan = 2;
                    Authority.PaddingTop = -2f;
                    Authority.Border = 0;
                    IDCardTemptable1.AddCell(Authority);

                    PdfPCell cellEmpSign = new PdfPCell(new Phrase("Signature of \nCard Holder", FontFactory.GetFont(fontstyle, Fontsize - 2, Font.NORMAL, BaseColor.BLACK)));
                    cellEmpSign.HorizontalAlignment = 1;
                    cellEmpSign.Border = 0;
                    cellEmpSign.Colspan = 2;
                    cellEmpSign.PaddingLeft = 20f;
                    IDCardTemptable1.AddCell(cellEmpSign);

                    PdfPCell cellAuthority = new PdfPCell(new Phrase("Issuing Authority", FontFactory.GetFont(fontstyle, Fontsize - 2, Font.NORMAL, BaseColor.BLACK)));
                    cellAuthority.HorizontalAlignment = 1;
                    cellAuthority.Border = 0;
                    cellAuthority.Colspan = 2;
                    cellAuthority.PaddingTop = 5;
                    IDCardTemptable1.AddCell(cellAuthority);

                    PdfPCell childTable1 = new PdfPCell(IDCardTemptable1);
                    childTable1.HorizontalAlignment = 0;
                    childTable1.Colspan = 4;
                    childTable1.Border = 0;
                    childTable1.PaddingLeft = -220f;
                    childTable1.PaddingBottom = 40f;
                    childTable1.PaddingTop = -28f;
                    IDCarddetails.AddCell(childTable1);

                    PdfPTable IDCardTemptable41 = new PdfPTable(1);
                    IDCardTemptable41.TotalWidth = 6f;
                    IDCardTemptable41.LockedWidth = true;
                    float[] width41 = new float[] { 0.8f };
                    IDCardTemptable41.SetWidths(width41);

                    PdfPCell cellempcell1 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellempcell1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellempcell1.Border = 0;
                    cellempcell1.Colspan = 1;
                    IDCardTemptable41.AddCell(cellempcell1);


                    PdfPCell childTable4 = new PdfPCell(IDCardTemptable41);
                    childTable4.HorizontalAlignment = 0;
                    childTable4.Colspan = 1;
                    childTable4.Border = 0;
                    IDCarddetails.AddCell(childTable4);


                    PdfPTable IDCardTemptable2 = new PdfPTable(4);
                    IDCardTemptable2.TotalWidth = 180f;
                    IDCardTemptable2.LockedWidth = true;
                    float[] width2 = new float[] { 2.3f, 2.3f, 2.3f, 2.3f };
                    IDCardTemptable2.SetWidths(width2);


                    PdfPCell cellInstructions = new PdfPCell(new Phrase("Instructions :", FontFactory.GetFont(fontstyle, fontsize1 + 1, Font.BOLD, BaseColor.BLACK)));
                    cellInstructions.HorizontalAlignment = 0;
                    cellInstructions.Border = 0;
                    cellInstructions.Colspan = 4;
                    //cellInstructions.PaddingTop = -20f;
                    IDCardTemptable2.AddCell(cellInstructions);

                    PdfPCell cellInstructions1 = new PdfPCell(new Phrase("1) The Identity Card Be worn by Employee \n    at all times.", FontFactory.GetFont(fontstyle, fontsize1, Font.NORMAL, BaseColor.BLACK)));
                    cellInstructions1.HorizontalAlignment = 0;
                    cellInstructions1.Border = 0;
                    cellInstructions1.Colspan = 4;
                    IDCardTemptable2.AddCell(cellInstructions1);

                    PdfPCell cellInstructions2 = new PdfPCell(new Phrase("2) Loss or Recovery of the Card shall be \n    reported to HR / Admin Dept.", FontFactory.GetFont(fontstyle, fontsize1, Font.NORMAL, BaseColor.BLACK)));
                    cellInstructions2.HorizontalAlignment = 0;
                    cellInstructions2.Border = 0;
                    cellInstructions2.Colspan = 4;
                    IDCardTemptable2.AddCell(cellInstructions2);

                    PdfPCell cellInstructions3 = new PdfPCell(new Phrase("3) The Finder May Please return to:", FontFactory.GetFont(fontstyle, fontsize1, Font.NORMAL, BaseColor.BLACK)));
                    cellInstructions3.HorizontalAlignment = 0;
                    cellInstructions3.Border = 0;
                    cellInstructions3.Colspan = 4;
                    IDCardTemptable2.AddCell(cellInstructions3);


                    PdfPCell cellInstructions4 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize1, Font.NORMAL, BaseColor.BLACK)));
                    cellInstructions4.HorizontalAlignment = 0;
                    cellInstructions4.Border = 0;
                    cellInstructions4.Colspan = 4;
                    cellInstructions4.PaddingBottom = 5f;
                    IDCardTemptable2.AddCell(cellInstructions4);

                    PdfPCell cellComp = new PdfPCell(new Phrase("SRF DETECTIVE & SECURITY \nSERVICES PVT LTD", FontFactory.GetFont(fontstyle, fontsize1, Font.BOLD, BaseColor.BLACK)));
                    cellComp.HorizontalAlignment = 0;
                    cellComp.Border = 0;
                    cellComp.Colspan = 4;
                    //cellComp.PaddingTop = 20;
                    IDCardTemptable2.AddCell(cellComp);

                    PdfPCell cellAddress = new PdfPCell(new Phrase("Keshava Nivas, # 24, Second Floor, Above \nCorporation Bank, Kalidasa Road,\nGandhinagar, Bengaluru - 560009 \nKarnataka", FontFactory.GetFont(fontstyle, fontsize2 + 2, Font.NORMAL, BaseColor.BLACK)));
                    cellAddress.HorizontalAlignment = 0;
                    cellAddress.Border = 0;
                    cellAddress.Colspan = 4;
                    cellAddress.SetLeading(0f, 1.2f);
                    cellAddress.VerticalAlignment = 13;
                    IDCardTemptable2.AddCell(cellAddress);

                    PdfPCell cellmobile = new PdfPCell(new Phrase("M. 9900066100", FontFactory.GetFont(fontstyle, fontsize1, Font.NORMAL, BaseColor.BLACK)));
                    cellmobile.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellmobile.Border = 0;
                    cellmobile.Colspan = 4;
                    IDCardTemptable2.AddCell(cellmobile);

                    PdfPCell cellmobile1 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize1, Font.NORMAL, BaseColor.BLACK)));
                    cellmobile1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellmobile1.Border = 0;
                    cellmobile1.Colspan = 4;
                    cellmobile1.PaddingBottom = 5f;
                    IDCardTemptable2.AddCell(cellmobile1);

                    PdfPCell cellEmergency = new PdfPCell(new Phrase("Emergency : ", FontFactory.GetFont(fontstyle, fontsize1, Font.BOLD, BaseColor.BLACK)));
                    cellEmergency.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellEmergency.Border = 0;
                    cellEmergency.Colspan = 4;
                    //cellEmergency.PaddingTop = 5;
                    IDCardTemptable2.AddCell(cellEmergency);

                    PdfPCell cellpolice = new PdfPCell(new Phrase("Police : 100   Ambulance : 102/108", FontFactory.GetFont(fontstyle, fontsize1, Font.NORMAL, BaseColor.BLACK)));
                    cellpolice.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellpolice.Border = 0;
                    cellpolice.Colspan = 4;
                    IDCardTemptable2.AddCell(cellpolice);



                    PdfPCell cellFire = new PdfPCell(new Phrase("Fire : 101   Traffic : 103", FontFactory.GetFont(fontstyle, fontsize1, Font.NORMAL, BaseColor.BLACK)));
                    cellFire.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellFire.Border = 0;
                    cellFire.Colspan = 4;
                    IDCardTemptable2.AddCell(cellFire);

                    PdfPCell cellATS = new PdfPCell(new Phrase("ATS : 1019", FontFactory.GetFont(fontstyle, fontsize1, Font.NORMAL, BaseColor.BLACK)));
                    cellATS.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellATS.Border = 0;
                    cellATS.Colspan = 4;
                    IDCardTemptable2.AddCell(cellATS);


                    PdfPCell cellBloodGroup = new PdfPCell(new Phrase("Blood Group : " + BloodGroup, FontFactory.GetFont(fontstyle, fontsize1, Font.NORMAL, BaseColor.BLACK)));
                    cellBloodGroup.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellBloodGroup.Border = 0;
                    cellBloodGroup.Colspan = 4;
                    //cellBloodGroup.PaddingTop = 15;
                    IDCardTemptable2.AddCell(cellBloodGroup);

                    PdfPCell childTable2 = new PdfPCell(IDCardTemptable2);
                    childTable2.HorizontalAlignment = 0;
                    childTable2.Colspan = 4;
                    childTable2.PaddingLeft = -174f;
                    childTable2.PaddingBottom = 40f;
                    childTable1.PaddingTop = -48f;
                    childTable2.Border = 0;
                    IDCarddetails.AddCell(childTable2);



                    PdfPTable IDCardTemptable31 = new PdfPTable(1);
                    IDCardTemptable31.TotalWidth = 2f;
                    IDCardTemptable31.LockedWidth = true;
                    float[] width31 = new float[] { 1f };
                    IDCardTemptable31.SetWidths(width31);

                    PdfPCell cellempcell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize2, Font.NORMAL, BaseColor.BLACK)));
                    cellempcell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellempcell.Border = 0;
                    cellempcell.Colspan = 1;
                    IDCardTemptable31.AddCell(cellempcell);

                    PdfPCell childTable3 = new PdfPCell(IDCardTemptable31);
                    childTable3.HorizontalAlignment = 0;
                    childTable3.Colspan = 1;
                    childTable3.Border = 0;
                    IDCarddetails.AddCell(childTable3);


                    ///

                    PdfPCell childTable6 = new PdfPCell();
                    childTable6.HorizontalAlignment = 0;
                    childTable6.Colspan = 10;
                    childTable6.Border = 0;

                    #endregion for range ID Card Display


                }

                PdfPCell empcellnew = new PdfPCell();
                empcellnew.HorizontalAlignment = 0;
                empcellnew.Colspan = 10;
                empcellnew.Border = 0;
                IDCarddetails.AddCell(empcellnew);

                PdfPCell empcellnewtrial = new PdfPCell();
                empcellnewtrial.HorizontalAlignment = 0;
                empcellnewtrial.Colspan = 10;
                empcellnewtrial.Border = 0;
                IDCarddetails.AddCell(empcellnewtrial);



                document.Add(IDCarddetails);

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=IDCard.pdf");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();

            }

        }

        protected void BtnIDCardNew_Click(object sender, EventArgs e)
        {
            int totalfonts = FontFactory.RegisterDirectory("c:\\WINDOWS\\fonts");
            StringBuilder sa = new StringBuilder();
            foreach (string fontname in FontFactory.RegisteredFonts)
            {
                sa.Append(fontname + "\n");
            }

            int Fontsize = 11;
            string fontstyle = "Calibri";

            List<String> EmpId_list = new List<string>();

            var list = new List<string>();

            for (int i = 0; i < lstEmpIdName.Items.Count; i++)
            {
                if (lstEmpIdName.Items[i].Selected == true)
                {
                    list.Add("'" + lstEmpIdName.Items[i].Value + "'");
                }
            }

            string empids = string.Join(",", list.ToArray());

            #region for Variable Declaration

            string Empid = "";
            string Name = "";
            string Designation = "";
            string IDcardIssued = "";
            string IDcardvalid = "";
            string BloodGroup = "";
            string prTown = "";
            string prPostOffice = "";
            string prTaluka = "";
            string statessndcity = "";
            string prPoliceStation = "";
            string prcity = "";
            string prphone = "";
            string prlmark = "";
            string prLmark = "";
            string prPincode = "";
            string prState = "";
            string State = "";
            string address1 = "";
            string Image = "";
            string EmpSign = "";
            string empdob = "";
            string empdoj = "";
            string empphoneno = "";
            string peTaluka = "";
            string peTown = "";
            string peLmark = "";
            string pearea = "";
            string pecity = "";
            string peDistrict = "";
            string pePincode = "";
            string addres1 = "";
            string peState = "";
            string pelmark = "";
            string branch = "";
            string pestreet = "";
            string pePostOffice = "";
            string pephone = "";
            string pePoliceStation = "";
            string ESInumber = "";


            #endregion for Variable Declaration

            #region for companyinfo
            string QueryCompanyInfo = "select * from companyinfo";
            DataTable DtCompanyInfo = SqlHelper.Instance.GetTableByQuery(QueryCompanyInfo);

            string CompanyName = "";
            string Address = "";
            string Emailid = "";
            string Website = "";
            string Phoneno = "";
            string Faxno = "";

            if (DtCompanyInfo.Rows.Count > 0)
            {
                CompanyName = DtCompanyInfo.Rows[0]["CompanyName"].ToString();
                Address = DtCompanyInfo.Rows[0]["Address"].ToString();
                Phoneno = DtCompanyInfo.Rows[0]["Phoneno"].ToString();
                Faxno = DtCompanyInfo.Rows[0]["Faxno"].ToString();
                Emailid = DtCompanyInfo.Rows[0]["Emailid"].ToString();
                Website = DtCompanyInfo.Rows[0]["Website"].ToString();


            }
            #endregion for companyinfo

            string query = "";
            DataTable dt = new DataTable();

            query = "select empdetails.Empid,(EmpFName+' '+EmpMName+''+EmpLName) as Fullname,EmpPhone,D.Design as EmpDesgn,prPostOffice,prPincode,(States.State+Cities.City) as statessndcity,(prTaluka+prPostOffice) as address1,EmpDetails.prLmark,prphone,prState,prcity,EmpDetails.prTaluka,EmpDetails.prTown,States.State,Cities.City,EmpDetails.prPincode,EmpPermanentAddress,(EmpDetails.prcity+EmpDetails.prLmark+EmpDetails.prTaluka+EmpDetails.prTown+States.State+Cities.City+EmpDetails.prPincode+EmpDetails.EmpPresentAddress) as address ," +
              "case convert(varchar(10),EmpDtofBirth,103) when '01/01/1900' then '' else convert(varchar(10),EmpDtofBirth,103) end EmpDtofBirth ," +
              "case convert(varchar(10),EmpDtofJoining,103) when '01/01/1900' then '' else convert(varchar(10),EmpDtofJoining,103) end EmpDtofJoining ," +
              "case convert(varchar(10),EmpDtofLeaving,103) when '01/01/1900' then '' else convert(varchar(10),EmpDtofLeaving,103) end EmpDtofLeaving ," +
              "case convert(varchar(11),IDCardIssued,106) when '01 Jan 1900' then '' else convert(varchar(11),IDCardIssued,106) end IDCardIssued ," +
              "case convert(varchar(11),IDCardValid,106) when '01 Jan 1900' then '' else convert(varchar(11),IDCardValid,106) end IDCardValid ," +
              "Image,EmpSign,BN.BloodGroupName as EmpBloodGroup,EmpESINo from EmpDetails " +
                       " inner join designations D on D.Designid=EmpDetails.EmpDesgn " +
                       " left join BloodGroupNames BN on BN.BloodGroupId=EmpDetails.EmpBloodGroup left join Cities on  Cities.CityID= EmpDetails.prCity LEFT JOIN States on States.StateID=EmpDetails.prState " +
                       "left join branch b on b.branchid=empdetails.branch left join empesicodes esi on esi.Empid=EmpDetails.EmpId" +
                       " where empdetails.empid  in (" + empids + ")  order by empid";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;

            if (dt.Rows.Count > 0)
            {
                MemoryStream ms = new MemoryStream();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("assets/EmpPhotos/");
                string imagepath2 = Server.MapPath("assets/Images/");
                string imagepath3 = Server.MapPath("assets/EmpSign/");
                string imagepath4 = Server.MapPath("assets/");
                Document document = new Document(PageSize.A4);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                #region for range ID Card Display

                PdfPTable MainIDCarddetails = new PdfPTable(12);
                MainIDCarddetails.TotalWidth = 850f;
                MainIDCarddetails.LockedWidth = true;
                MainIDCarddetails.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                MainIDCarddetails.DefaultCell.VerticalAlignment = Element.ALIGN_TOP;
                float[] width4 = new float[] { 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f };
                MainIDCarddetails.SetWidths(width4);

                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    prlmark = "";
                    prTaluka = "";
                    prTown = "";
                    prphone = "";
                    prcity = "";
                    prPincode = "";
                    peState = "";
                    prPostOffice = "";

                    Empid = dt.Rows[k]["Empid"].ToString();
                    Name = dt.Rows[k]["Fullname"].ToString();
                    Designation = dt.Rows[k]["EmpDesgn"].ToString();

                    IDcardIssued = dt.Rows[k]["IDCardIssued"].ToString();
                    IDcardvalid = dt.Rows[k]["IDCardValid"].ToString();

                    // IDcardIssued = DateTime.Parse(dt.Rows[k]["IDCardIssued"].ToString()).ToString("dd/MMM/yyyy");

                    // IDcardvalid = DateTime.Parse(dt.Rows[k]["IDCardValid"].ToString()).ToString("dd/MMM/yyyy");


                    BloodGroup = dt.Rows[k]["EmpBloodGroup"].ToString();
                    Image = dt.Rows[k]["Image"].ToString();
                    EmpSign = dt.Rows[k]["EmpSign"].ToString();
                    empdob = dt.Rows[k]["EmpDtofBirth"].ToString();
                    empdoj = dt.Rows[k]["EmpDtofJoining"].ToString();
                    // address = dt.Rows[k]["address"].ToString();
                    prlmark = dt.Rows[k]["prLmark"].ToString();
                    prTaluka = dt.Rows[k]["prTaluka"].ToString();
                    prTown = dt.Rows[k]["prTown"].ToString();
                    prphone = dt.Rows[k]["prphone"].ToString();
                    prcity = dt.Rows[0]["City"].ToString();
                    prPincode = dt.Rows[0]["prPincode"].ToString();
                    peState = dt.Rows[k]["State"].ToString();
                    prPostOffice = dt.Rows[k]["prPostOffice"].ToString();
                    Emailid = DtCompanyInfo.Rows[0]["Emailid"].ToString();
                    Website = DtCompanyInfo.Rows[0]["Website"].ToString();
                    address1 = dt.Rows[k]["address1"].ToString();
                    State = dt.Rows[k]["State"].ToString();
                    prPincode = dt.Rows[k]["prPincode"].ToString();
                    //EmpDtofLeaving = dt.Rows[k]["EmpDtofLeaving"].ToString();
                    empphoneno = dt.Rows[k]["EmpPhone"].ToString();
                    ESInumber = dt.Rows[k]["EmpESINo"].ToString();

                    PdfPTable IDCardTemptable1 = new PdfPTable(4);
                    IDCardTemptable1.TotalWidth = 460f;
                    // IDCardTemptable1.HorizontalAlignment = 0;
                    IDCardTemptable1.LockedWidth = true;
                    float[] width1 = new float[] { 2.4f, 2.4f, 2.4f, 2.4f };
                    IDCardTemptable1.SetWidths(width1);

                    iTextSharp.text.Image srflogo = iTextSharp.text.Image.GetInstance(imagepath2 + "/srflogo.png");
                    srflogo.ScaleAbsolute(430f, 110f);
                    PdfPCell companylogo = new PdfPCell();
                    Paragraph cmplogo = new Paragraph();
                    cmplogo.Add(new Chunk(srflogo, 60f, 0, true));
                    companylogo.AddElement(cmplogo);
                    companylogo.HorizontalAlignment = 0;
                    companylogo.Colspan = 4;
                    companylogo.PaddingLeft = -62;
                    //companylogo.PaddingTop = -10f;
                    companylogo.Border = 0;
                    IDCardTemptable1.AddCell(companylogo);

                    BaseColor color = new BaseColor(255, 0, 0);


                    PdfPCell compaddr = new PdfPCell(new Phrase("ISO 9001-2015", FontFactory.GetFont(fontstyle, Fontsize + 15, Font.BOLD, color)));
                    compaddr.HorizontalAlignment = 1;
                    compaddr.BorderWidthLeft = 0;
                    compaddr.BorderWidthTop = 0;
                    compaddr.BorderWidthBottom = 0;
                    compaddr.BorderWidthRight = 0;
                    compaddr.Colspan = 4;
                    compaddr.PaddingTop = -5;
                    IDCardTemptable1.AddCell(compaddr);

                    PdfPCell compno = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, Fontsize + 15, Font.BOLD, BaseColor.BLACK)));
                    compno.HorizontalAlignment = 1;
                    compno.BorderWidthLeft = 0;
                    compno.BorderWidthTop = 0;
                    compno.BorderWidthBottom = 0;
                    compno.BorderWidthRight = 0;
                    compno.Colspan = 4;
                    compno.FixedHeight = 5;
                    IDCardTemptable1.AddCell(compno);

                    if (Image.Length > 0)
                    {
                        iTextSharp.text.Image Empphoto = iTextSharp.text.Image.GetInstance(imagepath1 + Image);
                        //Empphoto.ScalePercent(25f);
                        Empphoto.ScaleAbsolute(260f, 280f);
                        PdfPCell EmpImage = new PdfPCell();
                        Paragraph Emplogo = new Paragraph();
                        Emplogo.Add(new Chunk(Empphoto, 85f, 0, true));
                        EmpImage.AddElement(Emplogo);
                        EmpImage.HorizontalAlignment = 1;
                        EmpImage.Colspan = 4;
                        EmpImage.Border = 0;
                        IDCardTemptable1.AddCell(EmpImage);
                    }
                    else
                    {
                        PdfPCell EmpImage = new PdfPCell();
                        EmpImage.HorizontalAlignment = 2;
                        EmpImage.Colspan = 4;
                        EmpImage.Border = 0;
                        EmpImage.FixedHeight = 220f;
                        IDCardTemptable1.AddCell(EmpImage);
                    }

                    compno = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, Fontsize + 15, Font.BOLD, BaseColor.BLACK)));
                    compno.HorizontalAlignment = 1;
                    compno.BorderWidthLeft = 0;
                    compno.BorderWidthTop = 0;
                    compno.BorderWidthBottom = 0;
                    compno.BorderWidthRight = 0;
                    compno.Colspan = 4;
                    compno.FixedHeight = 5;
                    IDCardTemptable1.AddCell(compno);


                    PdfPCell cellNameval = new PdfPCell(new Phrase("" + Name, FontFactory.GetFont(fontstyle, Fontsize + 22, Font.BOLD, BaseColor.BLACK)));
                    cellNameval.HorizontalAlignment = 1;
                    cellNameval.BorderWidthLeft = 0;
                    cellNameval.BorderWidthTop = 0;
                    cellNameval.BorderWidthBottom = 0;
                    cellNameval.BorderWidthRight = 0;
                    cellNameval.Colspan = 4;
                    IDCardTemptable1.AddCell(cellNameval);

                    cellNameval = new PdfPCell(new Phrase("P.No : " + Empid, FontFactory.GetFont(fontstyle, Fontsize + 22, Font.BOLD, BaseColor.BLACK)));
                    cellNameval.HorizontalAlignment = 1;
                    cellNameval.BorderWidthLeft = 0;
                    cellNameval.BorderWidthTop = 0;
                    cellNameval.BorderWidthBottom = 0;
                    cellNameval.BorderWidthRight = 0;
                    cellNameval.Colspan = 4;
                    IDCardTemptable1.AddCell(cellNameval);

                    cellNameval = new PdfPCell(new Phrase("Rank : " + Designation, FontFactory.GetFont(fontstyle, Fontsize + 22, Font.BOLD, BaseColor.BLACK)));
                    cellNameval.HorizontalAlignment = 1;
                    cellNameval.BorderWidthLeft = 0;
                    cellNameval.BorderWidthTop = 0;
                    cellNameval.BorderWidthBottom = 0;
                    cellNameval.BorderWidthRight = 0;
                    cellNameval.Colspan = 4;
                    IDCardTemptable1.AddCell(cellNameval);

                    cellNameval = new PdfPCell(new Phrase("Date of Issue : " + IDcardIssued, FontFactory.GetFont(fontstyle, Fontsize + 19, Font.NORMAL, BaseColor.BLACK)));
                    cellNameval.HorizontalAlignment = 1;
                    cellNameval.BorderWidthLeft = 0;
                    cellNameval.BorderWidthTop = 0;
                    cellNameval.BorderWidthBottom = 0;
                    cellNameval.BorderWidthRight = 0;
                    cellNameval.Colspan = 4;
                    IDCardTemptable1.AddCell(cellNameval);

                    cellNameval = new PdfPCell(new Phrase("Valid Upto : " + IDcardvalid, FontFactory.GetFont(fontstyle, Fontsize + 19, Font.NORMAL, BaseColor.BLACK)));
                    cellNameval.HorizontalAlignment = 1;
                    cellNameval.BorderWidthLeft = 0;
                    cellNameval.BorderWidthTop = 0;
                    cellNameval.BorderWidthBottom = 0;
                    cellNameval.BorderWidthRight = 0;
                    cellNameval.Colspan = 4;
                    IDCardTemptable1.AddCell(cellNameval);

                    cellNameval = new PdfPCell(new Phrase("ESI No : " + ESInumber, FontFactory.GetFont(fontstyle, Fontsize + 19, Font.NORMAL, BaseColor.BLACK)));
                    cellNameval.HorizontalAlignment = 1;
                    cellNameval.BorderWidthLeft = 0;
                    cellNameval.BorderWidthTop = 0;
                    cellNameval.BorderWidthBottom = 0;
                    cellNameval.BorderWidthRight = 0;
                    cellNameval.Colspan = 4;
                    IDCardTemptable1.AddCell(cellNameval);

                    cellNameval = new PdfPCell(new Phrase("Blood Group : " + BloodGroup, FontFactory.GetFont(fontstyle, Fontsize + 19, Font.NORMAL, BaseColor.BLACK)));
                    cellNameval.HorizontalAlignment = 1;
                    cellNameval.BorderWidthLeft = 0;
                    cellNameval.BorderWidthTop = 0;
                    cellNameval.BorderWidthBottom = 0;
                    cellNameval.BorderWidthRight = 0;
                    cellNameval.Colspan = 4;
                    IDCardTemptable1.AddCell(cellNameval);

                    PdfPCell cellemptyid = new PdfPCell(new Phrase(" ", FontFactory.GetFont(fontstyle, Fontsize + 12, Font.BOLD, BaseColor.BLACK)));
                    cellemptyid.HorizontalAlignment = 0;
                    cellemptyid.Border = 0;
                    cellemptyid.Colspan = 4;
                    cellemptyid.FixedHeight = 10;
                    IDCardTemptable1.AddCell(cellemptyid);


                    if (EmpSign.Length > 0)
                    {
                        iTextSharp.text.Image Empphoto = iTextSharp.text.Image.GetInstance(imagepath3 + EmpSign);
                        //Empphoto.ScalePercent(25f);
                        Empphoto.ScaleAbsolute(130f, 40f);
                        PdfPCell EmpImage = new PdfPCell();
                        Paragraph Emplogo = new Paragraph();
                        Emplogo.Add(new Chunk(Empphoto, 40f, 0));
                        EmpImage.AddElement(Emplogo);
                        EmpImage.HorizontalAlignment = 1;
                        EmpImage.Colspan = 2;
                        EmpImage.Border = 0;
                        IDCardTemptable1.AddCell(EmpImage);
                    }
                    else
                    {
                        PdfPCell EmpImage = new PdfPCell();
                        EmpImage.HorizontalAlignment = 2;
                        EmpImage.Colspan = 2;
                        EmpImage.Border = 0;
                        IDCardTemptable1.AddCell(EmpImage);

                    }

                    iTextSharp.text.Image IssuingAuth = iTextSharp.text.Image.GetInstance(imagepath2 + "Authority.png");
                    //IssuingAuth.ScalePercent(10f);
                    IssuingAuth.ScaleAbsolute(130f, 40f);
                    PdfPCell Authority = new PdfPCell();
                    Paragraph Authoritylogo = new Paragraph();
                    Authoritylogo.Add(new Chunk(IssuingAuth, 40f, -5f));
                    Authority.AddElement(Authoritylogo);
                    Authority.HorizontalAlignment = 0;
                    Authority.Colspan = 2;
                    Authority.PaddingLeft = 30f;
                    Authority.Border = 0;
                    Authority.PaddingTop = 10;
                    IDCardTemptable1.AddCell(Authority);

                    cellemptyid = new PdfPCell(new Phrase("Signature of Card Holder ", FontFactory.GetFont(fontstyle, Fontsize + 12, Font.BOLD, BaseColor.BLACK)));
                    cellemptyid.HorizontalAlignment = 0;
                    cellemptyid.Border = 0;
                    cellemptyid.Colspan = 2;
                    IDCardTemptable1.AddCell(cellemptyid);
                    //Tuesday January 17th 4:25pm

                    PdfPCell cellissu = new PdfPCell(new Phrase("Issuing Authority", FontFactory.GetFont(fontstyle, Fontsize + 17, Font.BOLD, BaseColor.BLACK)));
                    cellissu.HorizontalAlignment = 2;
                    cellissu.Border = 0;
                    cellissu.Colspan = 4;
                    IDCardTemptable1.AddCell(cellissu);

                    document.Add(IDCardTemptable1);



                    document.NewPage();

                    #region for subtable3


                    PdfPTable IDCardTemptable2 = new PdfPTable(4);
                    IDCardTemptable2.TotalWidth = 460f;
                    // IDCardTemptable1.HorizontalAlignment = 0;
                    IDCardTemptable2.LockedWidth = true;
                    float[] widthNEW = new float[] { 2.4f, 2.4f, 2.4f, 2.4f };
                    IDCardTemptable2.SetWidths(widthNEW);



                    PdfPCell Instructions = new PdfPCell(new Phrase("INSTRUCTIONS : ", FontFactory.GetFont(fontstyle, Fontsize + 21, Font.BOLD, BaseColor.BLACK)));
                    Instructions.HorizontalAlignment = 0;
                    Instructions.Border = 0;
                    Instructions.Colspan = 4;
                    Instructions.PaddingTop = 10;
                    IDCardTemptable2.AddCell(Instructions);

                    Instructions = new PdfPCell(new Phrase(" ", FontFactory.GetFont(fontstyle, Fontsize + 21, Font.BOLD, BaseColor.BLACK)));
                    Instructions.HorizontalAlignment = 0;
                    Instructions.Border = 0;
                    Instructions.Colspan = 4;
                    Instructions.FixedHeight = 5;
                    IDCardTemptable2.AddCell(Instructions);

                    PdfPCell Instructions1 = new PdfPCell(new Phrase("1) The Identity Card Be worn by\n    Employee at all times.", FontFactory.GetFont(fontstyle, Fontsize + 17, Font.BOLD, BaseColor.BLACK)));
                    Instructions1.HorizontalAlignment = 0;
                    Instructions1.Border = 0;
                    Instructions1.Colspan = 4;
                    //Instructions1.SetLeading(0f, 1.2f);
                    IDCardTemptable2.AddCell(Instructions1);

                    PdfPCell Instructions3 = new PdfPCell(new Phrase("2) Loss or Recovery of the Card shall\n    be reported to HR / Admin Dept.", FontFactory.GetFont(fontstyle, Fontsize + 17, Font.BOLD, BaseColor.BLACK)));
                    Instructions3.HorizontalAlignment = 0;
                    Instructions3.Border = 0;
                    Instructions3.Colspan = 4;
                    //Instructions3.SetLeading(0f, 1.2f);
                    IDCardTemptable2.AddCell(Instructions3);

                    PdfPCell cellbloodgrp = new PdfPCell(new Phrase("3) The Finder May Please return to:", FontFactory.GetFont(fontstyle, Fontsize + 17, Font.BOLD, BaseColor.BLACK)));
                    cellbloodgrp.HorizontalAlignment = 0;
                    cellbloodgrp.Border = 0;
                    cellbloodgrp.Colspan = 4;
                    //cellbloodgrp.PaddingLeft = 20f;
                    IDCardTemptable2.AddCell(cellbloodgrp);



                    PdfPCell Issuedate = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, Fontsize + 21, Font.BOLD, BaseColor.BLACK)));
                    Issuedate.HorizontalAlignment = 0;
                    Issuedate.Border = 0;
                    Issuedate.Colspan = 4;
                    Issuedate.PaddingLeft = 10;
                    Issuedate.FixedHeight = 25;
                    IDCardTemptable2.AddCell(Issuedate);

                    Issuedate = new PdfPCell(new Phrase(CompanyName, FontFactory.GetFont(fontstyle, Fontsize + 18, Font.BOLD, BaseColor.BLACK)));
                    Issuedate.HorizontalAlignment = 0;
                    Issuedate.Border = 0;
                    Issuedate.Colspan = 4;
                    Issuedate.PaddingLeft = 10;
                    IDCardTemptable2.AddCell(Issuedate);

                    Issuedate = new PdfPCell(new Phrase(Address, FontFactory.GetFont(fontstyle, Fontsize + 17, Font.NORMAL, BaseColor.BLACK)));
                    Issuedate.HorizontalAlignment = 0;
                    Issuedate.Border = 0;
                    Issuedate.Colspan = 4;
                    Issuedate.PaddingLeft = 10;
                    IDCardTemptable2.AddCell(Issuedate);

                    PdfPCell cellmobile = new PdfPCell(new Phrase("M. 9900066100", FontFactory.GetFont(fontstyle, Fontsize + 17, Font.NORMAL, BaseColor.BLACK)));
                    cellmobile.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellmobile.Border = 0;
                    cellmobile.Colspan = 4;
                    cellmobile.PaddingLeft = 10;
                    IDCardTemptable2.AddCell(cellmobile);

                    PdfPCell cellmobile1 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, Fontsize + 18, Font.NORMAL, BaseColor.BLACK)));
                    cellmobile1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellmobile1.Border = 0;
                    cellmobile1.Colspan = 4;
                    cellmobile1.FixedHeight = 5;
                    cellmobile1.PaddingBottom = 5f;
                    IDCardTemptable2.AddCell(cellmobile1);

                    Issuedate = new PdfPCell(new Phrase("Training Center", FontFactory.GetFont(fontstyle, Fontsize + 18, Font.BOLD, BaseColor.BLACK)));
                    Issuedate.HorizontalAlignment = 0;
                    Issuedate.Border = 0;
                    Issuedate.Colspan = 4;
                    Issuedate.PaddingLeft = 10;
                    IDCardTemptable2.AddCell(Issuedate);
                    Issuedate = new PdfPCell(new Phrase("SRF TRAINING CENTER\n# K-73, C/O Nadgir Institute of Technology, Madavara, Tumkur road,\nBangalore- 562162,Ph: 09686918100", FontFactory.GetFont(fontstyle, Fontsize + 17, Font.NORMAL, BaseColor.BLACK)));
                    Issuedate.HorizontalAlignment = 0;
                    Issuedate.Border = 0;
                    Issuedate.Colspan = 4;
                    Issuedate.PaddingLeft = 10;
                    IDCardTemptable2.AddCell(Issuedate);

                    cellmobile1 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, Fontsize + 18, Font.NORMAL, BaseColor.BLACK)));
                    cellmobile1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellmobile1.Border = 0;
                    cellmobile1.Colspan = 4;
                    cellmobile1.FixedHeight = 5;
                    cellmobile1.PaddingBottom = 5f;
                    IDCardTemptable2.AddCell(cellmobile1);

                    PdfPCell cellEmergency = new PdfPCell(new Phrase("Emergency : ", FontFactory.GetFont(fontstyle, Fontsize + 18, Font.BOLD, BaseColor.BLACK)));
                    cellEmergency.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellEmergency.Border = 0;
                    cellEmergency.Colspan = 4;
                    //cellEmergency.PaddingTop = 5;
                    IDCardTemptable2.AddCell(cellEmergency);

                    PdfPCell cellpolice = new PdfPCell(new Phrase("Police : 100   Ambulance : 102/108", FontFactory.GetFont(fontstyle, Fontsize + 18, Font.NORMAL, BaseColor.BLACK)));
                    cellpolice.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellpolice.Border = 0;
                    cellpolice.Colspan = 4;
                    IDCardTemptable2.AddCell(cellpolice);



                    PdfPCell cellFire = new PdfPCell(new Phrase("Fire : 101   Traffic : 103  ATS : 1019", FontFactory.GetFont(fontstyle, Fontsize + 18, Font.NORMAL, BaseColor.BLACK)));
                    cellFire.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellFire.Border = 0;
                    cellFire.Colspan = 4;
                    IDCardTemptable2.AddCell(cellFire);







                    document.Add(IDCardTemptable2);



                    //PdfPCell childTable2 = new PdfPCell(IDCardTemptable2);
                    //childTable2.HorizontalAlignment = 0;
                    //childTable2.Colspan = 4;
                    //childTable2.PaddingLeft = 20;
                    //IDCarddetails.AddCell(childTable2);

                    #endregion for sub table

                    document.NewPage();
                    //document.Add(IDCardTemptable1);
                }
                #endregion

                //document.Add(MainIDCarddetails);
                document.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=IDCard.pdf");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }
        }

    }
}