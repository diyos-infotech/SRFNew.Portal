using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using KLTS.Data;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class ClientAttenDanceReports : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        string Fontstyle = "";

        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();
        protected void Page_Load(object sender, EventArgs e)
        {
            GetWebConfigdata();
            if (!IsPostBack)
            {
                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {
                    
                }
                else
                {
                    Response.Redirect("login.aspx");
                }
                LoadClientIdAndName();
            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            Elength = (EmpIDPrefix.Trim().Length + 1).ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            Clength = (CmpIDPrefix.Trim().Length + 1).ToString();
        }

        protected void LoadClientIdAndName()
        {
            string selectquery;
            string cmpidprefix = "01/";
            if (CmpIDPrefix == cmpidprefix)
            {
                selectquery = "select clientid from clients order by  clientid";
            }
            else
            {
                selectquery = "select clientid from clients  Where Clientid like '%" + CmpIDPrefix + "%' order by  clientid";
            }
            DataTable dtForClientIdAndName = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

            if (dtForClientIdAndName.Rows.Count > 0)
            {
                ddlClientID.DataTextField = "Clientid";
                ddlClientID.DataValueField = "Clientid";
                ddlClientID.DataSource = dtForClientIdAndName;
                ddlClientID.DataBind();
            }
            ddlClientID.Items.Insert(0, "-Select-");
            ddlClientID.Items.Insert(1, "All");
            dtForClientIdAndName = null;

            if (CmpIDPrefix == cmpidprefix)
            {
                selectquery = "select clientid,Clientname from clients order by  Clientname";
            }
            else
            {
                selectquery = "select clientid,Clientname from clients  Where Clientid like '%" + CmpIDPrefix + "%' order by  Clientname";
            }
            dtForClientIdAndName = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

            if (dtForClientIdAndName.Rows.Count > 0)
            {
                ddlclientname.DataTextField = "Clientname";
                ddlclientname.DataValueField = "Clientid";
                ddlclientname.DataSource = dtForClientIdAndName;
                ddlclientname.DataBind();
            }
            ddlclientname.Items.Insert(0, "-Select-");
            ddlclientname.Items.Insert(1, "All");
        }

        protected void Fillcname()
        {
            string SqlQryForCname = "Select clientid from Clients where clientid='" + ddlClientID.SelectedValue + "'";
            DataTable dtCname = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForCname).Result;
            if (dtCname.Rows.Count > 0)
            {
                ddlclientname.SelectedValue = dtCname.Rows[0]["clientid"].ToString();
            }
        }

        protected void FillClientid()
        {
            string SqlQryForCid = "Select Clientid from Clients where clientid='" + ddlclientname.SelectedValue + "'";
            DataTable dtCname = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForCid).Result;
            if (dtCname.Rows.Count > 0)
            {
                ddlClientID.SelectedValue = dtCname.Rows[0]["clientid"].ToString();
            }
        }

        protected void ddlClientID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlClientID.SelectedIndex == 1)
            {
                ddlclientname.SelectedIndex = 1;
            }
            if (ddlClientID.SelectedIndex > 1)
            {
                Fillcname();

            }
            if (ddlClientID.SelectedIndex == 0)
            {
                Cleardata();
            }
        }

        protected void ddlclientname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlclientname.SelectedIndex == 1)
            {
                ddlClientID.SelectedIndex = 1;
            }
            if (ddlclientname.SelectedIndex > 1)
            {
                FillClientid();
            }
            if (ddlclientname.SelectedIndex == 0)
            {
                Cleardata();
            }
        }


        protected void Cleardata()
        {
            ddlclientname.SelectedIndex = 0;
            ddlClientID.SelectedIndex = 0;
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
            GVListClients.DataSource = null;
            GVListClients.DataBind();
            txtMonth.Text = "";
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LblResult.Text = "";
            LblResult.Visible = true;

            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
            GVListClients.DataSource = null;
            GVListClients.DataBind();

            if (txtMonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Plese Select The Date');", true);
                return;
            }
            string Month = string.Empty;
            string Year = string.Empty;
            string Date = string.Empty;
            string month = string.Empty;
            Hashtable HtClientAttendace = new Hashtable();
            string spname = "ClientAttendaceReort";
            if (txtMonth.Text.Trim().Length > 0)
            {
                Date = DateTime.Parse(txtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            Month = DateTime.Parse(Date).Month.ToString();
            Year = DateTime.Parse(Date).Year.ToString().Substring(2, 2);

            //DataTable DtGetEmployees=null;

            //DateTime date = Convert.ToDateTime(txtMonth.Text);
            //string mon = string.Format("{0}{1:yy}", date.Month.ToString("00"), (date.Year - 2000).ToString());
            month = Month + Year;
            //string SqlQryForGetEmployyes = "0";
            if (ChkAllattendance.Checked == true) { }
            else
            {
                if (ddlClientID.SelectedIndex == 1)
                {
                    HtClientAttendace.Add("@month", month);
                    HtClientAttendace.Add("@Attendancetype", ddlAttendanceType.SelectedIndex);
                    HtClientAttendace.Add("@ClientIdIndex", 1);
                    HtClientAttendace.Add("@CmpIDPrefix", CmpIDPrefix);

                }
                if (ddlClientID.SelectedIndex > 1)
                {
                    HtClientAttendace.Add("@month", month);
                    HtClientAttendace.Add("@Attendancetype", ddlAttendanceType.SelectedIndex);
                    HtClientAttendace.Add("@ClientIdIndex", 0);
                    HtClientAttendace.Add("@CmpIDPrefix", CmpIDPrefix);
                    HtClientAttendace.Add("@ClientId", ddlClientID.SelectedValue);
                }
            }
            //DataTable dt = SqlHelper.Instance.GetTableByQuery(SqlQryForGetEmployyes);
            DataTable DtGetList = config.ExecuteAdaptorAsyncWithParams(spname, HtClientAttendace).Result;

            if (DtGetList.Rows.Count > 0)
            {
                System.Threading.Thread.Sleep(2000);
                if (ddlAttendanceType.SelectedIndex == 0)
                {
                    GVListEmployees.DataSource = DtGetList;
                    GVListEmployees.DataBind();
                }
                else
                {
                    if (ddlAttendanceType.SelectedIndex == 1)
                    {
                        GVListClients.DataSource = DtGetList;
                        GVListClients.DataBind();
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('There is no list available for this client');", true);

            }

        }
        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("EmployeeAttendanceClientWise.xls", this.GVListEmployees);
        }

        protected string GetMonthName(string month)
        {

            int Month = 0;
            string Gmonth = "";
            string Year = "";

            if (month.Length == 3)
            {
                Month = int.Parse(month.Substring(0, 1));
                Year = "20" + month.Substring(1, 2);

            }

            if (month.Length == 4)
            {
                Month = int.Parse(month.Substring(0, 2));
                Year = "20" + month.Substring(2, 2);
            }

            // if(month!=0)
            {
                switch (Month)
                {
                    case 1:
                        Gmonth = "January";
                        break;
                    case 2:
                        Gmonth = "February";
                        break;
                    case 3:
                        Gmonth = "March";
                        break;
                    case 4:
                        Gmonth = "April";
                        break;
                    case 5:
                        Gmonth = "May";
                        break;
                    case 6:
                        Gmonth = "June";
                        break;
                    case 7:
                        Gmonth = "July";
                        break;
                    case 8:
                        Gmonth = "August";
                        break;
                    case 9:
                        Gmonth = "September";
                        break;
                    case 10:
                        Gmonth = "October";
                        break;
                    case 11:
                        Gmonth = "November";
                        break;
                    case 12:
                        Gmonth = "December";
                        break;
                    default:
                        break;

                }
            }
            return Gmonth + "-" + Year;
        }



        protected void lbtn_ExportPdf_Click(object sender, EventArgs e)
        {
            int titleofdocumentindex = 0;
            if (ddlClientID.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert()", "alert('Please select Client ID to generate wage sheet')", true);
                return;
            }


            if (txtMonth.Text.Trim().Length == 0)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Plese Select The Date');", true);
                return;
            }

            string Month = string.Empty;
            string Year = string.Empty;
            string Date = string.Empty;
            string month = string.Empty;
            if (txtMonth.Text.Trim().Length > 0)
            {
                Date = DateTime.Parse(txtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            Month = DateTime.Parse(Date).Month.ToString();
            Year = DateTime.Parse(Date).Year.ToString().Substring(2, 2);


            //DataTable DtGetEmployees = null;
            //GVListEmployees.DataSource = null;
            //GVListEmployees.DataBind();

            //DateTime date = Convert.ToDateTime(txtMonth.Text);
            //string mon = string.Format("{0}{1:yy}", date.Month.ToString("00"), (date.Year - 2000).ToString());
            month = Month + Year;
            string SqlQryForGetEmployyes = "";
            if (ddlClientID.SelectedIndex == 1)
            {
                string CompPrefix = "01/";
                if (CmpIDPrefix == CompPrefix)
                {
                    //selectclientid = "Select   clientid  from clients   Order By clientid";
                    SqlQryForGetEmployyes = " select  E.Empid,(Isnull(E.empfname,'')+ ' '+ isnull(E.empmname,'')+' '+ isnull(E.emplname,'')) as Name, " +
                                             " d.Design,  isnull(EA.NoOfDuties,0) as  NoOfDuties,EA.DutyHrs,isnull(EA.OT,0)  as OT, " +
                                             " isnull(EA.NHS,0) as  NHS ,isnull(EA.Npots,0) as Npots," +
                                             "(isnull(EA.NoOfDuties,0)+isnull(EA.OT,0)+isnull(EA.NHS,0)+isnull(EA.Npots,0)) as TotDts,C.Clientname " +
                                             " from EmpAttendance EA  Inner join EmpDetails E on  E.EmpId=EA.Empid  inner join Emppostingorder EPO on  " +
                                             "  EA.Design=EPO.Desgn  Inner join Clients C on C.Clientid=EPO.Tounitid inner join Designations d on EA.Design=d.DesignId " +
                                             " and EA.Clientid=EPO.Tounitid " +
                                             "  and EA.Month=" + month + "  and EA.EmpId=EPO.EmpId  Order by EA.EmpId   ";


                }
                else
                {
                    //   selectclientid = "Select clientid  from clients where clientid like '" + CmpIDPrefix + "%' Order By clientid";

                    SqlQryForGetEmployyes = " select  E.Empid,(Isnull(E.empfname,'')+ ' '+ isnull(E.empmname,'')+' '+ isnull(E.emplname,'')) as Name, " +
                                             " d.Design,  isnull(EA.NoOfDuties,0) as  NoOfDuties,EA.DutyHrs,isnull(EA.OT,0)  as OT, " +
                                             " isnull(EA.NHS,0) as  NHS ,isnull(EA.Npots,0) as Npots," +
                                             "(isnull(EA.NoOfDuties,0)+isnull(EA.OT,0)+isnull(EA.NHS,0)+isnull(EA.Npots,0)) as TotDts,C.Clientname " +
                                             " from EmpAttendance EA  Inner join EmpDetails E on  E.EmpId=EA.Empid  inner join Emppostingorder EPO on  " +
                                             "  EA.Design=EPO.Desgn  Inner join Clients C on C.Clientid=EPO.Tounitid inner join Designations d on EA.Design=d.DesignId " +
                                             "  and EA.Clientid=EPO.Tounitid  " +
                                             "  and EA.Month=" + month + "  and EA.EmpId=EPO.EmpId and EA.Clientid like '" + CmpIDPrefix + "%' Order by EA.EmpId   ";

                }
            }
            if (ddlClientID.SelectedIndex > 1)
            {

                SqlQryForGetEmployyes = " select   E.Empid,(Isnull(E.empfname,'')+ ' '+ isnull(E.empmname,'')+' '+ isnull(E.emplname,'')) as Name, " +
                                                " d.Design,   isnull(EA.NoOfDuties,0) as  NoOfDuties,EA.DutyHrs,isnull(EA.OT,0)  as OT, " +
                                                " isnull(EA.NHS,0) as  NHS ,isnull(EA.Npots,0) as Npots, " +
                                                " (isnull(EA.NoOfDuties,0)+isnull(EA.OT,0)+isnull(EA.NHS,0)+isnull(EA.Npots,0)) as TotDts,C.Clientname " +
                                         "  from EmpAttendance EA Inner join EmpDetails E on  E.EmpId=EA.Empid  inner join Emppostingorder EPO on " +
                                         " EA.Design=EPO.Desgn  Inner join Clients C on C.Clientid=EPO.Tounitid inner join Designations d on EA.Design=d.DesignId " +
                                         " and EA.Clientid=EPO.Tounitid and EA.Month=" + month + "  and EA.EmpId=EPO.EmpId And EA.ClientId='" +
                                         ddlClientID.SelectedValue + "'  Order by EA.EmpId ";

            }
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForGetEmployyes).Result;

            MemoryStream ms = new MemoryStream();
            if (dt.Rows.Count > 0)
            {
                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                document.AddTitle("FaMS");
                document.AddAuthor("DIYOS");
                document.AddSubject("Wage Sheet");
                document.AddKeywords("Keyword1, keyword2, …");//
                float forConvert;
                string strQry = "Select * from CompanyInfo";
                DataTable compInfo = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
                string companyName1 = "Your Company Name";
                string companyAddress = "Your Company Address";
                if (compInfo.Rows.Count > 0)
                {
                    companyName1 = compInfo.Rows[0]["CompanyName"].ToString();
                    companyAddress = compInfo.Rows[0]["Address"].ToString();
                }

                int tableCells = 10;
                #region variables for total
                float totalDuties = 0;
                float totalOts = 0;
                float totalNhs = 0;
                float totalNpots = 0;
                float totalTot = 0;

                #endregion

                #region variables for  Grand  total
                float GrandtotalDuties = 0;
                float GrandtotalOts = 0;
                float GrandtotalNhs = 0;
                float GrandtotalNpots = 0;
                float GrandtotalTot = 0;

                #endregion

                int nextpagerecordscount = 0;
                int targetpagerecors = 33;
                int secondpagerecords = targetpagerecors + 3;
                bool nextpagehasPages = false;
                int j = 0;
                PdfPTable SecondtableFooter = null;
                PdfPTable SecondtablecheckedbyFooter = null;

                for (int nextpagei = 0; nextpagei < dt.Rows.Count; nextpagei++)
                {
                    nextpagehasPages = true;
                    #region Titles of Document
                    PdfPTable Maintable = new PdfPTable(tableCells);
                    Maintable.TotalWidth = 550f;
                    Maintable.LockedWidth = true;
                    float[] width = new float[] { 1.5f, 1f, 1f, 3f, 3f, 2f, 2f, 2f, 2f, 2f };
                    Maintable.SetWidths(width);
                    uint FONT_SIZE = 8;

                    //Company Name & vage act details

                    PdfPCell cellemp = new PdfPCell(new Phrase("  ", FontFactory.GetFont(Fontstyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cellemp.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellemp.Colspan = tableCells;
                    cellemp.Border = 0;

                    Maintable.AddCell(cellemp);
                    #endregion

                    #region Table Headings
                    PdfPCell companyName = new PdfPCell(new Phrase(companyName1, FontFactory.GetFont("Arial Black", 20, Font.BOLD, BaseColor.BLACK)));
                    companyName.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    companyName.Colspan = tableCells;
                    companyName.Border = 0;// 15;
                    companyName.PaddingTop = -10;
                    Maintable.AddCell(companyName);

                    PdfPCell paySheet = new PdfPCell(new Phrase("ATTENDANCE", FontFactory.GetFont(Fontstyle, 10, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
                    paySheet.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    paySheet.Colspan = tableCells;
                    paySheet.Border = 0;// 15;
                    Maintable.AddCell(paySheet);

                    PdfPCell CClient = new PdfPCell(new Phrase("Client ID : " + ddlClientID.SelectedValue, FontFactory.GetFont(Fontstyle, 10, Font.BOLD, BaseColor.BLACK)));
                    CClient.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    CClient.Colspan = 5;
                    CClient.Border = 0;// 15;
                    Maintable.AddCell(CClient);

                    PdfPCell CClientName = new PdfPCell(new Phrase("Client Name : " + ddlclientname.SelectedItem, FontFactory.GetFont(Fontstyle, 10, Font.BOLD, BaseColor.BLACK)));
                    CClientName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    CClientName.Colspan = 2;
                    CClientName.Border = 0;// 15;
                    Maintable.AddCell(CClientName);


                    PdfPCell CPayMonth = new PdfPCell(new Phrase("      For the month of :   " + GetMonthName(month.ToString()), FontFactory.GetFont(Fontstyle, 10, Font.BOLD, BaseColor.BLACK)));
                    CPayMonth.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    CPayMonth.Colspan = 3;
                    CPayMonth.Border = 0;// 15;
                    Maintable.AddCell(CPayMonth);
                    Maintable.AddCell(cellemp);

                    if (titleofdocumentindex == 0)
                    {
                        document.Add(Maintable);
                        titleofdocumentindex = 1;
                    }
                    PdfPTable SecondtableHeadings = new PdfPTable(tableCells);
                    SecondtableHeadings.TotalWidth = 550f;
                    SecondtableHeadings.LockedWidth = true;
                    float[] SecondHeadingsWidth = new float[] { 1f, 2f, 4.0f, 3f, 6f, 1.5f, 1.3f, 1.3f, 1.3f, 2f };
                    SecondtableHeadings.SetWidths(SecondHeadingsWidth);

                    //Cell Headings
                    //1
                    PdfPCell sNo = new PdfPCell(new Phrase("S.No.", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    sNo.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //sNo.Colspan = 1;
                    sNo.Border = 15;// 15;
                    SecondtableHeadings.AddCell(sNo);
                    //2
                    PdfPCell CEmpId = new PdfPCell(new Phrase("Emp Id", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CEmpId.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    CEmpId.Border = 15;// 15;
                    SecondtableHeadings.AddCell(CEmpId);
                    //3
                    PdfPCell CEmpName = new PdfPCell(new Phrase("Emp Name", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CEmpName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    CEmpName.Border = 15;// 15;
                    SecondtableHeadings.AddCell(CEmpName);
                    //4
                    PdfPCell CDesgn = new PdfPCell(new Phrase("Desgn", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CDesgn.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    CDesgn.Border = 15;
                    SecondtableHeadings.AddCell(CDesgn);
                    //5
                    PdfPCell CClinetname = new PdfPCell(new Phrase("Client Name", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CClinetname.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    CClinetname.Border = 15;
                    SecondtableHeadings.AddCell(CClinetname);


                    //6
                    PdfPCell CDuties = new PdfPCell(new Phrase("No.Of Duties", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CDuties.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    CDuties.Border = 15;
                    SecondtableHeadings.AddCell(CDuties);

                    //7
                    PdfPCell COTs = new PdfPCell(new Phrase("OTs", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    COTs.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    COTs.Border = 15;
                    SecondtableHeadings.AddCell(COTs);
                    //8
                    PdfPCell CBasic = new PdfPCell(new Phrase("NHs", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CBasic.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    CBasic.Border = 15;
                    SecondtableHeadings.AddCell(CBasic);
                    //9
                    PdfPCell CDa = new PdfPCell(new Phrase("NPOTs", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CDa.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    CDa.Border = 15;
                    SecondtableHeadings.AddCell(CDa);

                    //10
                    PdfPCell CTot = new PdfPCell(new Phrase("Total Dts", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CTot.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    CTot.Border = 15;
                    SecondtableHeadings.AddCell(CTot);

                    #endregion

                    PdfPTable Secondtable = new PdfPTable(tableCells);
                    Secondtable.TotalWidth = 550f;
                    Secondtable.LockedWidth = true;
                    float[] SecondWidth = new float[] { 1f, 2f, 4.0f, 3f, 6f, 1.5f, 1.3f, 1.3f, 1.3f, 2f };
                    Secondtable.SetWidths(SecondWidth);

                    #region Table Data
                    int rowCount = 0;
                    //int pageCount = 0;
                    int slipsCount = 0;
                    int i = nextpagei;

                    //if (int i=nextpagei)
                    {

                        int Dts, Ots, Nhs, npots;
                        Dts = Ots = Nhs = npots = 0;

                        forConvert = 0;
                        if (dt.Rows[i]["NoOfDuties"].ToString().Trim().Length > 0)
                        {
                            forConvert = Convert.ToSingle(dt.Rows[i]["NoOfDuties"].ToString());
                            Dts = 1;
                        }
                        //}
                        if (forConvert != float.Parse(dt.Rows[i]["Ot"].ToString()))
                        {
                            forConvert = Convert.ToSingle(dt.Rows[i]["Ot"].ToString());
                            Ots = 1;
                        }
                        if (forConvert != float.Parse(dt.Rows[i]["NHs"].ToString()))
                        {
                            forConvert = Convert.ToSingle(dt.Rows[i]["NHs"].ToString());
                            Nhs = 1;
                        }
                        if (forConvert != float.Parse(dt.Rows[i]["NPots"].ToString()))
                        {
                            forConvert = Convert.ToSingle(dt.Rows[i]["NPots"].ToString());
                            npots = 1;
                        }
                        //else
                        //{

                        //    return;
                        //}
                        if (Dts != 0 || Ots != 0 || Nhs != 0 || npots != 0)
                        {
                            if (nextpagerecordscount == 0)
                            {
                                document.Add(SecondtableHeadings);
                            }

                            nextpagerecordscount++;
                            //1
                            PdfPCell CSNo = new PdfPCell(new Phrase((++j).ToString() + "", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                            CSNo.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            CSNo.VerticalAlignment = Element.ALIGN_MIDDLE;
                            CSNo.Border = 15;
                            Secondtable.AddCell(CSNo);
                            //2
                            PdfPCell CEmpId1 = new PdfPCell(new Phrase(dt.Rows[i]["EmpId"].ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                            CEmpId1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            CEmpId1.Border = 15;
                            Secondtable.AddCell(CEmpId1);
                            //3
                            PdfPCell CEmpName1 = new PdfPCell(new Phrase(dt.Rows[i]["Name"].ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                            CEmpName1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            CEmpName1.Border = 15;
                            Secondtable.AddCell(CEmpName1);
                            //4
                            PdfPCell CEmpDesgn = new PdfPCell(new Phrase(dt.Rows[i]["Desgn"].ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                            CEmpDesgn.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            CEmpDesgn.Border = 15;
                            Secondtable.AddCell(CEmpDesgn);
                            //5

                            PdfPCell CTotcname = new PdfPCell(new Phrase(dt.Rows[i]["clientname"].ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                            CTotcname.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            CTotcname.Border = 15;
                            Secondtable.AddCell(CTotcname);

                            //6
                            forConvert = 0;
                            if (dt.Rows[i]["NoOfDuties"].ToString().Trim().Length > 0)
                                forConvert = Convert.ToSingle(dt.Rows[i]["NoOfDuties"].ToString());
                            totalDuties += forConvert;
                            GrandtotalDuties += forConvert;
                            PdfPCell CNoOfDuties = new PdfPCell(new Phrase(forConvert.ToString("0.0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                            CNoOfDuties.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            CNoOfDuties.Border = 15;
                            Secondtable.AddCell(CNoOfDuties);
                            //7
                            if (dt.Rows[i]["ot"].ToString().Trim().Length > 0)
                                forConvert = Convert.ToSingle(dt.Rows[i]["ot"].ToString());
                            totalOts += forConvert;
                            GrandtotalOts += forConvert;
                            PdfPCell CNoOfots = new PdfPCell(new Phrase(forConvert.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                            CNoOfots.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            CNoOfots.Border = 15;
                            Secondtable.AddCell(CNoOfots);

                            //8
                            forConvert = 0;
                            if (dt.Rows[i]["NHS"].ToString().Trim().Length > 0)
                                forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["NHS"].ToString()));
                            totalNhs += forConvert;
                            GrandtotalNhs += forConvert;
                            PdfPCell CBasic1 = new PdfPCell(new Phrase(forConvert.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                            CBasic1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            CBasic1.Border = 15;
                            Secondtable.AddCell(CBasic1);

                            //9
                            forConvert = 0;

                            if (dt.Rows[i]["Npots"].ToString().Trim().Length > 0)
                                forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Npots"].ToString()));
                            totalNpots += forConvert;
                            GrandtotalNpots += forConvert;
                            PdfPCell CDa1 = new PdfPCell(new Phrase(forConvert.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                            CDa1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            CDa1.Border = 15;
                            Secondtable.AddCell(CDa1);

                            //10
                            forConvert = 0;
                            float TotalDuties = float.Parse(dt.Rows[i]["NHS"].ToString()) + float.Parse(dt.Rows[i]["ot"].ToString()) + float.Parse(dt.Rows[i]["NoOfDuties"].ToString()) + float.Parse(dt.Rows[i]["Npots"].ToString());
                            //if (dt.Rows[i]["TotDts"].ToString().Trim().Length > 0)
                            //    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["TotDts"].ToString()));
                            totalTot += TotalDuties;
                            GrandtotalTot += TotalDuties;
                            PdfPCell CTot1 = new PdfPCell(new Phrase(TotalDuties.ToString("0.0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                            CTot1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            CTot1.Border = 15;
                            Secondtable.AddCell(CTot1);

                        }

                    }

                    #endregion

                    #region Comment the foote code

                    SecondtableFooter = new PdfPTable(tableCells);
                    SecondtableFooter.TotalWidth = 550f;
                    SecondtableFooter.LockedWidth = true;
                    float[] SecondFooterWidth = new float[] { 1f, 2f, 4.0f, 3f, 6f, 1.5f, 1.3f, 1.3f, 1.3f, 2f };
                    SecondtableFooter.SetWidths(SecondFooterWidth);

                    #region Table Footer
                    //1
                    PdfPCell FCSNo = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    FCSNo.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    FCSNo.Border = 15;
                    SecondtableFooter.AddCell(FCSNo);
                    //2
                    PdfPCell FCEmpId1 = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    FCEmpId1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    FCEmpId1.Border = 15;
                    SecondtableFooter.AddCell(FCEmpId1);
                    //3
                    PdfPCell FCEmpName1 = new PdfPCell(new Phrase("Total : ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    FCEmpName1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    FCEmpName1.Border = 15;
                    SecondtableFooter.AddCell(FCEmpName1);
                    //4
                    PdfPCell FCEmpDesgn = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    FCEmpDesgn.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    FCEmpDesgn.Border = 15;
                    SecondtableFooter.AddCell(FCEmpDesgn);
                    //5

                    PdfPCell Ftotname = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    Ftotname.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    Ftotname.Border = 15;
                    SecondtableFooter.AddCell(Ftotname);
                    //6

                    PdfPCell FCNoOfDuties = new PdfPCell(new Phrase(totalDuties.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    FCNoOfDuties.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    FCNoOfDuties.Border = 15;
                    SecondtableFooter.AddCell(FCNoOfDuties);
                    //7
                    PdfPCell FCNoOfots = new PdfPCell(new Phrase(totalOts.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    FCNoOfots.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    FCNoOfots.Border = 15;
                    SecondtableFooter.AddCell(FCNoOfots);

                    //8
                    PdfPCell FCBasic1 = new PdfPCell(new Phrase(Math.Round(totalNhs).ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    FCBasic1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    FCBasic1.Border = 15;
                    SecondtableFooter.AddCell(FCBasic1);


                    //9
                    PdfPCell FDa = new PdfPCell(new Phrase(Math.Round(totalNpots).ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    FDa.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    FDa.Border = 15;
                    SecondtableFooter.AddCell(FDa);

                    //10
                    PdfPCell FTot = new PdfPCell(new Phrase(Math.Round(totalTot).ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    FTot.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    FTot.Border = 15;
                    SecondtableFooter.AddCell(FTot);

                    #endregion

                    SecondtablecheckedbyFooter = new PdfPTable(tableCells);
                    SecondtablecheckedbyFooter.TotalWidth = 550f;
                    SecondtablecheckedbyFooter.LockedWidth = true;
                    float[] SecondcheckedFooterWidth = new float[] { 1f, 2f, 4.0f, 3f, 6f, 1.5f, 1.3f, 1.3f, 1.3f, 2f };
                    SecondtablecheckedbyFooter.SetWidths(SecondcheckedFooterWidth);


                    #region Table  Grand  Total   Footer
                    //1
                    PdfPCell GFCSNo = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    GFCSNo.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    GFCSNo.Border = 15;
                    SecondtablecheckedbyFooter.AddCell(GFCSNo);
                    //2
                    PdfPCell GFCEmpId1 = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    GFCEmpId1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    GFCEmpId1.Border = 15;
                    SecondtablecheckedbyFooter.AddCell(GFCEmpId1);
                    //3
                    PdfPCell GFCEmpName1 = new PdfPCell(new Phrase(" Grand  Total: ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    GFCEmpName1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    GFCEmpName1.Border = 15;
                    SecondtablecheckedbyFooter.AddCell(GFCEmpName1);
                    //4
                    PdfPCell GFCEmpDesgn = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    GFCEmpDesgn.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    GFCEmpDesgn.Border = 15;
                    //FCEmpDesgn.Colspan = 4;
                    SecondtablecheckedbyFooter.AddCell(GFCEmpDesgn);

                    //5
                    PdfPCell GFTname = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    GFTname.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    GFTname.Border = 15;
                    SecondtablecheckedbyFooter.AddCell(GFTname);

                    //6
                    PdfPCell GFCNoOfDuties = new PdfPCell(new Phrase(GrandtotalDuties.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    GFCNoOfDuties.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    GFCNoOfDuties.Border = 15;
                    SecondtablecheckedbyFooter.AddCell(GFCNoOfDuties);
                    //7
                    PdfPCell GFCNoOfots = new PdfPCell(new Phrase(GrandtotalOts.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    GFCNoOfots.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    GFCNoOfots.Border = 15;
                    SecondtablecheckedbyFooter.AddCell(GFCNoOfots);

                    //8
                    PdfPCell GFCBasic1 = new PdfPCell(new Phrase(Math.Round(GrandtotalNhs).ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    GFCBasic1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    GFCBasic1.Border = 15;
                    SecondtablecheckedbyFooter.AddCell(GFCBasic1);
                    //9
                    PdfPCell GFDa = new PdfPCell(new Phrase(Math.Round(GrandtotalNpots).ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    GFDa.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    GFDa.Border = 15;
                    SecondtablecheckedbyFooter.AddCell(GFDa);
                    //10
                    PdfPCell GFTot = new PdfPCell(new Phrase(Math.Round(GrandtotalTot).ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    GFTot.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    GFTot.Border = 15;
                    SecondtablecheckedbyFooter.AddCell(GFTot);


                    #endregion

                    #region   Footer Headings


                    //1
                    PdfPCell FHCheckedbybr1 = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    FHCheckedbybr1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    FHCheckedbybr1.Border = 0;
                    FHCheckedbybr1.Rowspan = 0;
                    FHCheckedbybr1.Colspan = 10;
                    SecondtablecheckedbyFooter.AddCell(FHCheckedbybr1);
                    //2
                    PdfPCell FHApprovedbr2 = new PdfPCell(new Phrase("  ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    FHApprovedbr2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    FHApprovedbr2.Border = 0;
                    FHApprovedbr2.Colspan = 10;
                    SecondtablecheckedbyFooter.AddCell(FHApprovedbr2);


                    SecondtablecheckedbyFooter.AddCell(FHCheckedbybr1);
                    SecondtablecheckedbyFooter.AddCell(FHApprovedbr2);

                    //1
                    PdfPCell FHCheckedby = new PdfPCell(new Phrase("Checked By  ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    FHCheckedby.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    FHCheckedby.Border = 0;
                    FHCheckedby.Rowspan = 0;
                    FHCheckedby.Colspan = 5;
                    SecondtablecheckedbyFooter.AddCell(FHCheckedby);
                    //2
                    PdfPCell FHApprovedBy = new PdfPCell(new Phrase("Attendance  Approved By   ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    FHApprovedBy.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    FHApprovedBy.Border = 0;
                    FHApprovedBy.Colspan = 5;
                    SecondtablecheckedbyFooter.AddCell(FHApprovedBy);

                    #endregion

                    #endregion
                    document.Add(Secondtable);
                    //#region    Pdf New page and  all the totals are zero
                    if (nextpagerecordscount == targetpagerecors)
                    {
                        targetpagerecors = secondpagerecords;
                        // document.Add(SecondtableFooter);
                        document.NewPage();
                        nextpagerecordscount = 0;
                        #region  Zero variables

                        totalDuties = 0;
                        totalOts = 0;
                        totalNhs = 0;
                        totalNpots = 0;

                        #endregion
                    }
                }

                if (nextpagerecordscount >= 0)
                {
                    // document.Add(SecondtableFooter);
                    document.Add(SecondtablecheckedbyFooter);

                }

                //#endregion  
                if (nextpagehasPages)
                {
                    document.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Attendance.pdf");
                    Response.Buffer = true;
                    Response.Clear();
                    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    Response.OutputStream.Flush();
                    Response.End();
                }
            }
        }
        float totalNoOfDuties = 0;
        float totalOTs = 0;
        float totalNHs = 0;
        float totalNPOTs = 0;
        float TotalDuties = 0;
        protected void GVListEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                float NoOfDuties = float.Parse(((Label)e.Row.FindControl("lblnoofduties")).Text);
                totalNoOfDuties += NoOfDuties;
                float OTs = float.Parse(((Label)e.Row.FindControl("lblots")).Text);
                totalOTs += OTs;
                float NHs = float.Parse(((Label)e.Row.FindControl("lblNhs")).Text);
                totalNHs += NHs;
                float NPOTs = float.Parse(((Label)e.Row.FindControl("lblNpots")).Text);
                totalNPOTs += NPOTs;
                TotalDuties += float.Parse(((Label)e.Row.FindControl("lblTotalDuties")).Text);

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Total";
                ((Label)e.Row.FindControl("lblTotalNoOfDutied")).Text = totalNoOfDuties.ToString();
                ((Label)e.Row.FindControl("lblTotalOTs")).Text = totalOTs.ToString();
                ((Label)e.Row.FindControl("lblTotalNHS")).Text = totalNHs.ToString();
                ((Label)e.Row.FindControl("lblTotalNpots")).Text = totalNPOTs.ToString();
                e.Row.Cells[11].Text = TotalDuties.ToString();
            }
        }

        decimal NoOfDuties = 0;
        protected void GVListClients_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                NoOfDuties += decimal.Parse(((Label)e.Row.FindControl("lblnoofduties")).Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Total";
                e.Row.Cells[4].Text = NoOfDuties.ToString();
            }
        }
    }
}