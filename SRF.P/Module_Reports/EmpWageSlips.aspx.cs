using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Data;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class EmpWageSlips : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        string Fontstyle = "";
        string CFontstyle = "";

        AppConfiguration config = new AppConfiguration();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
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


                    var formatInfoinfo = new DateTimeFormatInfo();
                    string[] monthName = formatInfoinfo.MonthNames;
                    string month = monthName[DateTime.Now.Month - 1];

                    loadEmpIDs();
                    loadEmpNames();
                }
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }

        public string GetFromMonth()
        {
            string month = "";
            string year = "";
            string DateVal = "";
            DateTime date;

            if (txtfrom.Text != "")
            {
                date = DateTime.ParseExact(txtfrom.Text, "MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                month = date.ToString("MM");
                year = date.ToString("yy");
            }

            DateVal = year + month;
            return DateVal;

        }

        public string GetToMonth()
        {
            string Tomonth = "";
            string Toyear = "";
            string DateValTo = "";
            DateTime Todate;


            if (txtfrom.Text != "")
            {
                Todate = DateTime.ParseExact(txtfrom.Text, "MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Tomonth = Todate.ToString("MM");
                Toyear = Todate.ToString("yy");
            }


            DateValTo = Toyear + Tomonth;
            return DateValTo;

        }

        public void loadEmpIDs()
        {
            string sqlqry = "Select distinct(EmpID) as EmpID from EmpDetails  order by EmpID";
            DataTable dt = config.ExecuteReaderWithQueryAsync(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlEmpID.DataValueField = "EmpID";
                ddlEmpID.DataTextField = "EmpID";
                ddlEmpID.DataSource = dt;
                ddlEmpID.DataBind();
            }

            ddlEmpID.Items.Insert(0, "-Select-");
        }

        public void loadEmpNames()
        {
            string sqlqry = "Select distinct(EmpID) as EmpID ,(isnull(EmpFname,'')+' '+isnull(EmpMName,'')+' '+isnull(EmpLName,'')) as FullName from EmpDetails order by EmpID";
            DataTable dt = config.ExecuteReaderWithQueryAsync(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlEmpName.DataValueField = "EmpID";
                ddlEmpName.DataTextField = "FullName";
                ddlEmpName.DataSource = dt;
                ddlEmpName.DataBind();
            }

            ddlEmpName.Items.Insert(0, "-Select-");
        }

        protected void ddlEmpName_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmpName.SelectedIndex > 0)
            {
                lblresult.Visible = false;
                lblresult.Text = "";
                ddlEmpID.SelectedValue = ddlEmpName.SelectedValue;

            }
            else
            {
                ddlEmpID.SelectedIndex = 0;
            }
        }

        protected void ddlEmpID_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlEmpID.SelectedIndex > 0)
            {
                lblresult.Visible = false;
                lblresult.Text = "";
                ddlEmpName.SelectedValue = ddlEmpID.SelectedValue;
            }
            else
            {
                ddlEmpName.SelectedIndex = 0;
            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        public void PaysheetDetails()
        {
            if (ddlEmpID.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert()", "alert('Please select employee ID/Name')", true);
                return;
            }
            DateTime frmdate;
            string FromDate = "";
            string Frmonth = "";
            string FrYear = "";

            if (txtfrom.Text.Trim().Length > 0)
            {
                frmdate = DateTime.ParseExact(txtfrom.Text.Trim(), "MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Frmonth = frmdate.ToString("MM");
                FrYear = frmdate.ToString("yy");
            }

            FromDate = FrYear + Frmonth;

            DateTime tdate;
            string ToDate = "";
            string Tomonth = "";
            string ToYear = "";

            if (txtto.Text.Trim().Length > 0)
            {
                tdate = DateTime.ParseExact(txtto.Text.Trim(), "MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Tomonth = tdate.ToString("MM");
                ToYear = tdate.ToString("yy");

            }

            ToDate = ToYear + Tomonth;


            string selectmonth = string.Empty;
            selectmonth = "select eps.clientid,c.clientname,Eps.month,Eps.EmpId,Designations.design as Desgn,TempGross as salaryrate,  Eps.NoOfDuties,Eps.WO,Eps.NHS,Eps.Basic,Eps.DA,Eps.Bonus," +
                    "  Eps.HRA,Eps.CCA,Eps.Conveyance,Eps.WashAllowance,Eps.OtherAllowance,Eps.LeaveEncashAmt,Eps.Woamt,Eps.NhsAmt,Eps.Nfhs,Eps.Gratuity,Eps.PF,Eps.ESI,Eps.OTAmt,Eps.ProfTax," +
                  "Eps.SalAdvDed,Eps.CanteenAdv,Eps.UniformDed,Eps.OWF,Eps.Gross,Eps.TotalDeductions as Deductions,Eps.ActualAmount, " +
                  "  Eps.OWF, (EmpDetails.Empfname +' ' + EmpDetails.EmpMname + ' ' + EmpDetails.emplname) as EmpmName ," +
                  "EmpDetails.UnitId, EmpDetails.EmpBankAcNo, EmpDetails.EmpBankCardRef,Eps.OTs from EmpPaySheet as Eps " +
                  " INNER JOIN EmpDetails ON Eps.EmpId = EmpDetails.EmpId INNER JOIN Designations ON Designations.designid=Eps.desgn INNER JOIN clients c ON c.clientid=Eps.clientid " +
                  "   AND EmpDetails.EmpId='" + ddlEmpID.SelectedValue + "'   and eps.monthnew between '" + FromDate.ToString() + "' and '" + ToDate.ToString() + "' order by eps.monthnew desc ";
            DataTable dt = SqlHelper.Instance.GetTableByQuery(selectmonth);
            if (dt.Rows.Count > 0)
            {
                lblresult.Visible = false;
                lblresult.Text = "";
                gvattendancezero.DataSource = dt;
                gvattendancezero.DataBind();

            }
            else
            {
                lblresult.Visible = true;
                lblresult.Text = "No Records Found";
                return;
            }

        }

        protected void btndownload_Click(object sender, EventArgs e)
        {
            PaysheetDetails();
        }

        public string GetMonthName(int month)
        {
            string monthname = string.Empty;
            int payMonth = 0;
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();

            monthname = mfi.GetMonthName(month).ToString();

            return monthname;
        }

        public string GetMonthOfYear(int month)
        {
            string MonthYear = "";

            if (month.ToString().Length == 4)
            {
                MonthYear = "20" + month.ToString().Substring(2, 2);
            }
            if (month.ToString().Length == 3)
            {
                MonthYear = "20" + month.ToString().Substring(1, 2);

            }
            return MonthYear;
        }

        protected void btnPaySlip_Click(object sender, EventArgs e)
        {
            if (ddlEmpID.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert()", "alert('Please select employee ID/Name')", true);
                return;
            }


            DateTime frmdate;
            string FromDate = "";
            string Frmonth = "";
            string FrYear = "";

            if (txtfrom.Text.Trim().Length > 0)
            {
                frmdate = DateTime.ParseExact(txtfrom.Text.Trim(), "MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Frmonth = frmdate.ToString("MM");
                FrYear = frmdate.ToString("yy");
            }

            FromDate = FrYear + Frmonth;

            DateTime tdate;
            string ToDate = "";
            string Tomonth = "";
            string ToYear = "";

            if (txtto.Text.Trim().Length > 0)
            {
                tdate = DateTime.ParseExact(txtto.Text.Trim(), "MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Tomonth = tdate.ToString("MM");
                ToYear = tdate.ToString("yy");

            }

            ToDate = ToYear + Tomonth;

            string selectmonth = string.Empty;

            int Fontsize = 10;
            string fontsyle = "verdana";

            string monthvalue = "";
            string yearvalue = "";
            string mn = "";
            string yr = "";


            var SPName = "";
            Hashtable HTPaysheet = new Hashtable();
            SPName = "IndividualWageslips";
            HTPaysheet.Add("@Empid", ddlEmpID.SelectedValue);
            HTPaysheet.Add("@fromdate", FromDate.ToString());
            HTPaysheet.Add("@todate", ToDate.ToString());

            DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, HTPaysheet).Result;

            MemoryStream ms = new MemoryStream();

            int slipsCount = 0;
            string UANNumber = "";

            if (dt.Rows.Count > 0)
            {
                Document document = new Document(PageSize.LEGAL);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");

                string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
                DataTable compInfo = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
                string companyName = "Your Company Name";
                string companyAddress = "Your Company Address";
                string PFNOForms = "";
                string TotalPFNOForms = "";
                if (compInfo.Rows.Count > 0)
                {
                    companyName = compInfo.Rows[0]["CompanyName"].ToString();
                    companyAddress = compInfo.Rows[0]["Address"].ToString();
                    //PFNOForms = compInfo.Rows[0]["PFNoForms"].ToString();
                }


                float forConvert = 0;
                float forConvert1 = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["ActualAmount"].ToString().Trim().Length > 0)
                        forConvert = Convert.ToSingle(dt.Rows[i]["ActualAmount"].ToString());
                    if (forConvert > 0)
                    {
                        strQry = "Select p.EmpEpfNo,e.EmpESINo from EMPESICodes AS e INNER JOIN EMPEPFCodes as p ON e.Empid = p.Empid AND e.Empid='" + dt.Rows[i]["EmpId"].ToString() + "'";
                        string pfNo = "";
                        string esiNo = "";
                        DataTable PfTable = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
                        if (PfTable.Rows.Count > 0)
                        {
                            pfNo = PfTable.Rows[0]["EmpEpfNo"].ToString();
                            esiNo = PfTable.Rows[0]["EmpESINo"].ToString();
                        }

                        companyAddress = dt.Rows[i]["GSTAddress"].ToString();

                        // var output = new FileStream(fileheader2, FileMode., FileAccess.Write, FileShare.None);

                        float totalcdBasic = 0;
                        float totalcdDA = 0;
                        float totalcdHRA = 0;
                        float totalcdNFhs = 0;
                        float totalcdBonus = 0;
                        float totalcdCCA = 0;
                        float totalcdGratuity = 0;
                        float totalcdotherAllowance = 0;
                        float totalcdLeaveAmount = 0;
                        float totalcdConveyance = 0;
                        float totalcdWashAllowance = 0;
                        float totalstandradamt = 0;
                        float totalcdRC = 0;
                        float totalcdNHS = 0;




                        #region

                        PdfPTable tablewageslip = new PdfPTable(5);
                        tablewageslip.TotalWidth = 550f;
                        tablewageslip.LockedWidth = true;
                        float[] width = new float[] { 2f, 2f, 2f, 2f, 2f };
                        tablewageslip.SetWidths(width);

                        PdfPCell cellspace = new PdfPCell(new Phrase("  ", FontFactory.GetFont(fontsyle, Fontsize - 2, Font.NORMAL, BaseColor.BLACK)));
                        cellspace.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cellspace.Colspan = 5;
                        cellspace.Border = 0;
                        cellspace.PaddingTop = 6;
                        tablewageslip.AddCell(cellspace);

                        PdfPCell cellHead1 = new PdfPCell(new Phrase("FORM-XIX - Wage Slip  ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellHead1.HorizontalAlignment = 1;
                        cellHead1.Colspan = 5;
                        cellHead1.Border = 0;
                        tablewageslip.AddCell(cellHead1);

                        PdfPCell cellHead2 = new PdfPCell(new Phrase("M/s " + companyName, FontFactory.GetFont(fontsyle, 13, Font.NORMAL, BaseColor.BLACK)));
                        cellHead2.HorizontalAlignment = 1;
                        cellHead2.Colspan = 5;
                        cellHead2.Border = 0;
                        tablewageslip.AddCell(cellHead2);

                        PdfPCell cellHead31 = new PdfPCell(new Phrase(companyAddress, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellHead31.HorizontalAlignment = 1;
                        cellHead31.Colspan = 5;
                        cellHead31.Border = 0;
                        tablewageslip.AddCell(cellHead31);



                        PdfPCell cellHead4 = new PdfPCell(new Phrase("Pay Slip for month of " + dt.Rows[i]["month"].ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellHead4.HorizontalAlignment = 1;
                        cellHead4.Colspan = 5;
                        cellHead4.Border = 0;
                        tablewageslip.AddCell(cellHead4);



                        PdfPCell cellHead5 = new PdfPCell(new Phrase("NAME : " + dt.Rows[i]["EmpmName"].ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellHead5.HorizontalAlignment = 0;
                        cellHead5.Colspan = 3;
                        cellHead5.MinimumHeight = 20;
                        tablewageslip.AddCell(cellHead5);



                        PdfPCell cellHead7 = new PdfPCell(new Phrase("Unit : " + dt.Rows[i]["ClientName"].ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellHead7.HorizontalAlignment = 0;
                        cellHead7.Colspan = 2;
                        tablewageslip.AddCell(cellHead7);


                        PdfPCell cellHead51 = new PdfPCell(new Phrase("Employee ID -  " + dt.Rows[i]["Empid"].ToString() + "\nDesignation - " + dt.Rows[i]["Desgn"].ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellHead51.HorizontalAlignment = 0;
                        cellHead51.Colspan = 3;
                        cellHead51.SetLeading(0, 1.2f);
                        tablewageslip.AddCell(cellHead51);

                        PdfPCell cellHead711 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellHead711.HorizontalAlignment = 0;
                        cellHead711.Colspan = 2;
                        //cellHead711.MinimumHeight = 20;
                        tablewageslip.AddCell(cellHead711);

                        //PdfPCell cellHead711 = new PdfPCell(new Phrase("C/o - " + Address1.TrimStart(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        //cellHead711.HorizontalAlignment = 0;
                        //cellHead711.Colspan = 2;
                        ////cellHead711.MinimumHeight = 20;
                        //tablewageslip.AddCell(cellHead711);

                        PdfPCell cellHead71 = new PdfPCell(new Phrase("Bank Account No - " + dt.Rows[i]["EmpBankAcNo"].ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellHead71.HorizontalAlignment = 0;
                        cellHead71.Colspan = 2;
                        //cellHead71.MinimumHeight = 20;
                        tablewageslip.AddCell(cellHead71);

                        forConvert = Convert.ToSingle(dt.Rows[i]["wrkngdays"].ToString());

                        if (forConvert > 0)
                        {
                            cellHead71 = new PdfPCell(new Phrase("Working Days - " + forConvert.ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellHead71.HorizontalAlignment = 0;
                            cellHead71.Colspan = 1;
                            //cellHead71.MinimumHeight = 20;
                            tablewageslip.AddCell(cellHead71);
                        }
                        else
                        {
                            cellHead71 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellHead71.HorizontalAlignment = 0;
                            cellHead71.Colspan = 1;
                            //cellHead71.MinimumHeight = 20;
                            tablewageslip.AddCell(cellHead71);
                        }

                        PdfPCell cellHead101 = new PdfPCell(new Phrase("E.P.F Number - " + pfNo, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellHead101.HorizontalAlignment = 0;
                        cellHead101.Colspan = 2;
                        tablewageslip.AddCell(cellHead101);


                        forConvert = Convert.ToSingle(dt.Rows[i]["NoOfDuties"].ToString());
                        if (forConvert > 0)
                        {
                            {
                                PdfPCell cellHead111 = new PdfPCell(new Phrase("Duties - " + forConvert.ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                cellHead111.HorizontalAlignment = 0;
                                cellHead111.Colspan = 1;
                                //cellHead111.MinimumHeight = 20;
                                tablewageslip.AddCell(cellHead111);
                            }
                        }
                        else
                        {
                            PdfPCell cellHead111 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellHead111.HorizontalAlignment = 0;
                            cellHead111.Colspan = 1;
                            //cellHead111.MinimumHeight = 20;
                            tablewageslip.AddCell(cellHead111);

                        }

                        forConvert = Convert.ToSingle(dt.Rows[i]["Wo"].ToString());
                        if (forConvert > 0)
                        {

                            PdfPCell cellHead111 = new PdfPCell(new Phrase("WO - " + forConvert.ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellHead111.HorizontalAlignment = 0;
                            cellHead111.Colspan = 1;
                            //cellHead111.MinimumHeight = 20;
                            tablewageslip.AddCell(cellHead111);
                        }
                        else
                        {
                            PdfPCell cellHead111 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellHead111.HorizontalAlignment = 0;
                            cellHead111.Colspan = 1;
                            //cellHead111.MinimumHeight = 20;
                            tablewageslip.AddCell(cellHead111);

                        }

                        forConvert = Convert.ToSingle(dt.Rows[i]["ots"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellHead1112 = new PdfPCell(new Phrase("ED - " + forConvert.ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellHead1112.HorizontalAlignment = 0;
                            cellHead1112.Colspan = 1;
                            //cellHead1112.MinimumHeight = 20;
                            tablewageslip.AddCell(cellHead1112);
                        }

                        else
                        {
                            PdfPCell cellHead11124 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellHead11124.HorizontalAlignment = 0;
                            cellHead11124.Colspan = 1;
                            //cellHead11124.MinimumHeight = 20;
                            tablewageslip.AddCell(cellHead11124);

                        }



                        UANNumber = dt.Rows[i]["EmpUANNumber"].ToString();


                        if (UANNumber.Trim().Length > 0)
                        {

                            PdfPCell cellHeadUANNo = new PdfPCell(new Phrase("UAN No - " + UANNumber, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellHeadUANNo.HorizontalAlignment = 0;
                            cellHeadUANNo.Colspan = 2;
                            //cellHead71.MinimumHeight = 20;
                            tablewageslip.AddCell(cellHeadUANNo);
                        }

                        else
                        {
                            PdfPCell cellHeadUANNo = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellHeadUANNo.HorizontalAlignment = 0;
                            cellHeadUANNo.Colspan = 2;
                            //cellHead71.MinimumHeight = 20;
                            tablewageslip.AddCell(cellHeadUANNo);
                        }




                        {
                            forConvert = Convert.ToSingle(dt.Rows[i]["nhs"].ToString());
                            if (forConvert > 0)
                            {
                                PdfPCell cellHead111 = new PdfPCell(new Phrase("FH/NFH Days - " + forConvert.ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                cellHead111.HorizontalAlignment = 0;
                                cellHead111.Colspan = 1;
                                //cellHead111.MinimumHeight = 20;
                                tablewageslip.AddCell(cellHead111);
                            }
                            else
                            {
                                PdfPCell cellHead111 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                cellHead111.HorizontalAlignment = 0;
                                cellHead111.Colspan = 1;
                                //cellHead111.MinimumHeight = 20;
                                tablewageslip.AddCell(cellHead111);

                            }
                        }


                        forConvert = Convert.ToSingle(dt.Rows[i]["npots"].ToString());
                        if (forConvert > 0)
                        {

                            PdfPCell cellHead111 = new PdfPCell(new Phrase("Npots - " + forConvert.ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellHead111.HorizontalAlignment = 0;
                            cellHead111.Colspan = 1;
                            //cellHead111.MinimumHeight = 20;
                            tablewageslip.AddCell(cellHead111);
                        }
                        else
                        {
                            PdfPCell cellHead111 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellHead111.HorizontalAlignment = 0;
                            cellHead111.Colspan = 1;
                            //cellHead111.MinimumHeight = 20;
                            tablewageslip.AddCell(cellHead111);

                        }


                        {
                            forConvert = Convert.ToSingle(dt.Rows[i]["Leave"].ToString());
                            if (forConvert > 0)
                            {
                                PdfPCell cellHead111 = new PdfPCell(new Phrase("Leave/Absent - " + forConvert.ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                cellHead111.HorizontalAlignment = 0;
                                cellHead111.Colspan = 1;
                                //cellHead111.MinimumHeight = 20;
                                tablewageslip.AddCell(cellHead111);
                            }
                            else
                            {
                                PdfPCell cellHead111 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                cellHead111.HorizontalAlignment = 0;
                                cellHead111.Colspan = 1;
                                //cellHead111.MinimumHeight = 20;
                                tablewageslip.AddCell(cellHead111);

                            }
                        }




                        PdfPCell cellHead121 = new PdfPCell(new Phrase("ESI Number - " + esiNo, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellHead121.HorizontalAlignment = 0;
                        cellHead121.Colspan = 2;
                        tablewageslip.AddCell(cellHead121);



                        BaseColor color = new BaseColor(221, 226, 222);



                        PdfPTable tableEarnings = new PdfPTable(3);
                        tableEarnings.TotalWidth = 330;
                        tableEarnings.LockedWidth = true;
                        float[] width1 = new float[] { 2f, 2f, 2f };

                        tableEarnings.SetWidths(width1);

                        PdfPCell cellHead9 = new PdfPCell(new Phrase("Description", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellHead9.HorizontalAlignment = 1;
                        cellHead9.Colspan = 1;
                        cellHead9.MinimumHeight = 20;
                        cellHead9.BackgroundColor = color;
                        tableEarnings.AddCell(cellHead9);




                        PdfPCell cellHead1011 = new PdfPCell(new Phrase("Standard Amount", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellHead1011.HorizontalAlignment = 1;
                        cellHead1011.Colspan = 1;
                        cellHead1011.BackgroundColor = color;
                        tableEarnings.AddCell(cellHead1011);


                        PdfPCell cellHead10 = new PdfPCell(new Phrase("Earnings Amount", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellHead10.HorizontalAlignment = 1;
                        cellHead10.Colspan = 1;
                        cellHead10.BackgroundColor = color;
                        tableEarnings.AddCell(cellHead10);

                        forConvert = Convert.ToSingle(dt.Rows[i]["Basic"].ToString());
                        forConvert1 = Convert.ToSingle(dt.Rows[i]["cdBasic"].ToString());



                        if (forConvert > 0)
                        {
                            PdfPCell cellbascic = new PdfPCell(new Phrase("Basic", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellbascic.HorizontalAlignment = 0;
                            cellbascic.Colspan = 1;
                            //cellbascic.MinimumHeight = 20;
                            tableEarnings.AddCell(cellbascic);

                            PdfPCell cellbascic11 = new PdfPCell(new Phrase(forConvert1.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellbascic11.HorizontalAlignment = 2;
                            cellbascic11.Colspan = 1;
                            tableEarnings.AddCell(cellbascic11);
                            totalcdBasic += forConvert1;

                            PdfPCell cellbascic1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellbascic1.HorizontalAlignment = 2;
                            cellbascic1.Colspan = 1;
                            tableEarnings.AddCell(cellbascic1);
                        }

                        forConvert = Convert.ToSingle(dt.Rows[i]["DA"].ToString());
                        forConvert1 = Convert.ToSingle(dt.Rows[i]["cdDA"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellDearness = new PdfPCell(new Phrase("Dearness Allowance", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellDearness.HorizontalAlignment = 0;
                            cellDearness.Colspan = 1;
                            //cellDearness.MinimumHeight = 20;
                            tableEarnings.AddCell(cellDearness);


                            PdfPCell cellbascic111 = new PdfPCell(new Phrase(forConvert1.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellbascic111.HorizontalAlignment = 2;
                            cellbascic111.Colspan = 1;
                            tableEarnings.AddCell(cellbascic111);
                            totalcdDA += forConvert1;


                            PdfPCell cellDearness1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellDearness1.HorizontalAlignment = 2;
                            cellDearness1.Colspan = 1;
                            tableEarnings.AddCell(cellDearness1);
                        }



                        forConvert = Convert.ToSingle(dt.Rows[i]["HRA"].ToString());
                        forConvert1 = Convert.ToSingle(dt.Rows[i]["cdHRA"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellHRA = new PdfPCell(new Phrase("HRA", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellHRA.HorizontalAlignment = 0;
                            cellHRA.Colspan = 1;
                            //cellHRA.MinimumHeight = 20;
                            tableEarnings.AddCell(cellHRA);


                            PdfPCell ccellHRA11 = new PdfPCell(new Phrase(forConvert1.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            ccellHRA11.HorizontalAlignment = 2;
                            ccellHRA11.Colspan = 1;
                            tableEarnings.AddCell(ccellHRA11);
                            totalcdHRA += forConvert1;


                            PdfPCell ccellHRA1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            ccellHRA1.HorizontalAlignment = 2;
                            ccellHRA1.Colspan = 1;
                            tableEarnings.AddCell(ccellHRA1);
                        }

                        forConvert = Convert.ToSingle(dt.Rows[i]["WashAllowance"].ToString());
                        forConvert1 = Convert.ToSingle(dt.Rows[i]["cdWashAllowance"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellWAAmt = new PdfPCell(new Phrase("Wash Allowance", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellWAAmt.HorizontalAlignment = 0;
                            cellWAAmt.Colspan = 1;
                            //cellWAAmt.MinimumHeight = 20;
                            tableEarnings.AddCell(cellWAAmt);


                            PdfPCell cellWAAmt11 = new PdfPCell(new Phrase(forConvert1.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellWAAmt11.HorizontalAlignment = 2;
                            cellWAAmt11.Colspan = 1;
                            //////cellWAAmt11.MinimumHeight = 20;
                            tableEarnings.AddCell(cellWAAmt11);
                            totalcdWashAllowance += forConvert1;

                            PdfPCell cellWAAmt112 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellWAAmt112.HorizontalAlignment = 2;
                            cellWAAmt112.Colspan = 1;
                            //cellWAAmt112.MinimumHeight = 20;
                            tableEarnings.AddCell(cellWAAmt112);
                        }

                        forConvert = Convert.ToSingle(dt.Rows[i]["Conveyance"].ToString());
                        forConvert1 = Convert.ToSingle(dt.Rows[i]["cdConveyance"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellConveyance = new PdfPCell(new Phrase("Conveyance", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellConveyance.HorizontalAlignment = 0;
                            cellConveyance.Colspan = 1;
                            //cellConveyance.MinimumHeight = 20;
                            tableEarnings.AddCell(cellConveyance);


                            PdfPCell cellConveyance11 = new PdfPCell(new Phrase(forConvert1.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellConveyance11.HorizontalAlignment = 2;
                            cellConveyance11.Colspan = 1;
                            tableEarnings.AddCell(cellConveyance11);
                            totalcdConveyance += forConvert1;

                            PdfPCell cellConveyance1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellConveyance1.HorizontalAlignment = 2;
                            cellConveyance1.Colspan = 1;
                            tableEarnings.AddCell(cellConveyance1);
                        }



                        forConvert = Convert.ToSingle(dt.Rows[i]["Bonus"].ToString());
                        forConvert1 = Convert.ToSingle(dt.Rows[i]["cdBonus"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellbonus = new PdfPCell(new Phrase("Bonus", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellbonus.HorizontalAlignment = 0;
                            cellbonus.Colspan = 1;
                            //cellbonus.MinimumHeight = 20;
                            tableEarnings.AddCell(cellbonus);

                            PdfPCell cellbonus11 = new PdfPCell(new Phrase(forConvert1.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellbonus11.HorizontalAlignment = 2;
                            cellbonus11.Colspan = 1;
                            tableEarnings.AddCell(cellbonus11);
                            totalcdBonus += forConvert1;

                            PdfPCell cellbonus1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellbonus1.HorizontalAlignment = 2;
                            cellbonus1.Colspan = 1;
                            tableEarnings.AddCell(cellbonus1);
                        }



                        forConvert = Convert.ToSingle(dt.Rows[i]["LeaveEncashAmt"].ToString());
                        forConvert1 = Convert.ToSingle(dt.Rows[i]["cdLeaveAmount"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellLeave = new PdfPCell(new Phrase("Leave Wage", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellLeave.HorizontalAlignment = 0;
                            cellLeave.Colspan = 1;
                            //cellLeave.MinimumHeight = 20;
                            tableEarnings.AddCell(cellLeave);


                            PdfPCell cellLeave11 = new PdfPCell(new Phrase(forConvert1.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellLeave11.HorizontalAlignment = 2;
                            cellLeave11.Colspan = 1;
                            tableEarnings.AddCell(cellLeave11);
                            totalcdLeaveAmount += forConvert1;

                            PdfPCell cellLeave1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellLeave1.HorizontalAlignment = 2;
                            cellLeave1.Colspan = 1;
                            tableEarnings.AddCell(cellLeave1);
                        }

                        forConvert = Convert.ToSingle(dt.Rows[i]["OtherAllowance"].ToString());
                        forConvert1 = Convert.ToSingle(dt.Rows[i]["cdotherAllowance"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellOTher = new PdfPCell(new Phrase("Other Allowance", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellOTher.HorizontalAlignment = 0;
                            cellOTher.Colspan = 1;
                            //cellOTher.MinimumHeight = 20;
                            tableEarnings.AddCell(cellOTher);

                            PdfPCell cellOTher11 = new PdfPCell(new Phrase(forConvert1.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellOTher11.HorizontalAlignment = 2;
                            cellOTher11.Colspan = 1;
                            tableEarnings.AddCell(cellOTher11);
                            totalcdotherAllowance += forConvert1;

                            PdfPCell cellOTher1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellOTher1.HorizontalAlignment = 2;
                            cellOTher1.Colspan = 1;
                            tableEarnings.AddCell(cellOTher1);
                        }





                        forConvert = Convert.ToSingle(dt.Rows[i]["Gratuity"].ToString());
                        forConvert1 = Convert.ToSingle(dt.Rows[i]["cdGratuity"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellGratuity = new PdfPCell(new Phrase("Gratuity", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellGratuity.HorizontalAlignment = 0;
                            cellGratuity.Colspan = 1;
                            //cellGratuity.MinimumHeight = 20;
                            tableEarnings.AddCell(cellGratuity);


                            PdfPCell cellGratuity11 = new PdfPCell(new Phrase(forConvert1.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellGratuity11.HorizontalAlignment = 2;
                            cellGratuity11.Colspan = 1;
                            tableEarnings.AddCell(cellGratuity11);
                            totalcdGratuity += forConvert1;

                            PdfPCell cellGratuity1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellGratuity1.HorizontalAlignment = 2;
                            cellGratuity1.Colspan = 1;
                            tableEarnings.AddCell(cellGratuity1);
                        }


                        forConvert = Convert.ToSingle(dt.Rows[i]["CCA"].ToString());
                        forConvert1 = Convert.ToSingle(dt.Rows[i]["cdCCA"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellCCA = new PdfPCell(new Phrase("CCA", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellCCA.HorizontalAlignment = 0;
                            cellCCA.Colspan = 1;
                            //cellCCA.MinimumHeight = 20;
                            tableEarnings.AddCell(cellCCA);


                            PdfPCell cellCCA11 = new PdfPCell(new Phrase(forConvert1.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellCCA11.HorizontalAlignment = 2;
                            cellCCA11.Colspan = 1;
                            //cellCCA11.MinimumHeight = 20;
                            tableEarnings.AddCell(cellCCA11);
                            totalcdCCA += forConvert1;

                            PdfPCell cellCCA1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellCCA1.HorizontalAlignment = 2;
                            cellCCA1.Colspan = 1;
                            //cellCCA1.MinimumHeight = 20;
                            tableEarnings.AddCell(cellCCA1);
                        }

                        forConvert = Convert.ToSingle(dt.Rows[i]["RC"].ToString());
                        forConvert1 = Convert.ToSingle(dt.Rows[i]["cdRC"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellRC = new PdfPCell(new Phrase("RC", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellRC.HorizontalAlignment = 0;
                            cellRC.Colspan = 1;
                            //cellRC.MinimumHeight = 20;
                            tableEarnings.AddCell(cellRC);


                            PdfPCell cellRC11 = new PdfPCell(new Phrase(forConvert1.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellRC11.HorizontalAlignment = 2;
                            cellRC11.Colspan = 1;
                            tableEarnings.AddCell(cellRC11);
                            totalcdRC += forConvert1;

                            PdfPCell cellRC1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellRC1.HorizontalAlignment = 2;
                            cellRC1.Colspan = 1;
                            tableEarnings.AddCell(cellRC1);
                        }

                        forConvert = Convert.ToSingle(dt.Rows[i]["Incentivs"].ToString());
                        //forConvert1 = Convert.ToSingle(dt.Rows[i]["cdNFhs"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellNFHSAmt = new PdfPCell(new Phrase("Incentives", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellNFHSAmt.HorizontalAlignment = 0;
                            cellNFHSAmt.Colspan = 1;
                            //cellNFHSAmt.MinimumHeight = 20;
                            tableEarnings.AddCell(cellNFHSAmt);

                            PdfPCell cellNFHSAmt11 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellNFHSAmt11.HorizontalAlignment = 2;
                            cellNFHSAmt11.Colspan = 1;
                            //cellNFHSAmt11.MinimumHeight = 20;
                            tableEarnings.AddCell(cellNFHSAmt11);
                            totalcdNFhs += forConvert1;

                            PdfPCell cellNFHSAmt1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellNFHSAmt1.HorizontalAlignment = 2;
                            cellNFHSAmt1.Colspan = 1;
                            //cellNFHSAmt1.MinimumHeight = 20;
                            tableEarnings.AddCell(cellNFHSAmt1);
                        }

                        forConvert = Convert.ToSingle(dt.Rows[i]["Nfhs"].ToString());
                        forConvert1 = Convert.ToSingle(dt.Rows[i]["cdNFhs"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellNFHSAmt = new PdfPCell(new Phrase("NFHS Amt", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellNFHSAmt.HorizontalAlignment = 0;
                            cellNFHSAmt.Colspan = 1;
                            //cellNFHSAmt.MinimumHeight = 20;
                            tableEarnings.AddCell(cellNFHSAmt);

                            PdfPCell cellNFHSAmt11 = new PdfPCell(new Phrase(forConvert1.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellNFHSAmt11.HorizontalAlignment = 2;
                            cellNFHSAmt11.Colspan = 1;
                            //cellNFHSAmt11.MinimumHeight = 20;
                            tableEarnings.AddCell(cellNFHSAmt11);
                            totalcdNFhs += forConvert1;

                            PdfPCell cellNFHSAmt1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellNFHSAmt1.HorizontalAlignment = 2;
                            cellNFHSAmt1.Colspan = 1;
                            //cellNFHSAmt1.MinimumHeight = 20;
                            tableEarnings.AddCell(cellNFHSAmt1);
                        }



                        ///nhs,Wo 've same components in contractdetailssw
                        forConvert = Convert.ToSingle(dt.Rows[i]["WOAmt"].ToString());
                        forConvert1 = Convert.ToSingle(dt.Rows[i]["salrate"].ToString());
                        if (forConvert > 0)
                        {
                            PdfPCell cellWOAmt = new PdfPCell(new Phrase("WO Amt", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellWOAmt.HorizontalAlignment = 0;
                            cellWOAmt.Colspan = 1;
                            //cellWOAmt.MinimumHeight = 20;
                            tableEarnings.AddCell(cellWOAmt);

                            PdfPCell cellWOAmt11 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellWOAmt11.HorizontalAlignment = 2;
                            cellWOAmt11.Colspan = 1;
                            //cellWOAmt11.MinimumHeight = 20;
                            tableEarnings.AddCell(cellWOAmt11);


                            PdfPCell cellWOAmt1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellWOAmt1.HorizontalAlignment = 2;
                            cellWOAmt1.Colspan = 1;
                            //cellWOAmt1.MinimumHeight = 20;
                            tableEarnings.AddCell(cellWOAmt1);
                        }


                        forConvert = Convert.ToSingle(dt.Rows[i]["Nhsamt"].ToString());
                        forConvert1 = Convert.ToSingle(dt.Rows[i]["CDNhsrate"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellNHSAmt = new PdfPCell(new Phrase("NHS Amount", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellNHSAmt.HorizontalAlignment = 0;
                            cellNHSAmt.Colspan = 1;
                            //cellNHSAmt.MinimumHeight = 20;
                            tableEarnings.AddCell(cellNHSAmt);


                            PdfPCell cellNHSAmt11 = new PdfPCell(new Phrase(forConvert1.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellNHSAmt11.HorizontalAlignment = 2;
                            cellNHSAmt11.Colspan = 1;
                            //cellNHSAmt11.MinimumHeight = 20;
                            tableEarnings.AddCell(cellNHSAmt11);
                            totalcdNHS += forConvert1;


                            PdfPCell cellNHSAmt1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellNHSAmt1.HorizontalAlignment = 2;
                            cellNHSAmt1.Colspan = 1;
                            //cellNHSAmt1.MinimumHeight = 20;
                            tableEarnings.AddCell(cellNHSAmt1);
                        }






                        forConvert = Convert.ToSingle(dt.Rows[i]["npotsamt"].ToString());
                        forConvert1 = Convert.ToSingle(dt.Rows[i]["salrate"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellSplDutiesAmt = new PdfPCell(new Phrase("Npots Amt", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellSplDutiesAmt.HorizontalAlignment = 0;
                            cellSplDutiesAmt.Colspan = 1;
                            //cellSplDutiesAmt.MinimumHeight = 20;
                            tableEarnings.AddCell(cellSplDutiesAmt);

                            PdfPCell cellSplDutiesAmt11 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellSplDutiesAmt11.HorizontalAlignment = 2;
                            cellSplDutiesAmt11.Colspan = 1;
                            //cellSplDutiesAmt11.MinimumHeight = 20;
                            tableEarnings.AddCell(cellSplDutiesAmt11);


                            PdfPCell cellSplDutiesAmt1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellSplDutiesAmt1.HorizontalAlignment = 2;
                            cellSplDutiesAmt1.Colspan = 1;
                            //cellSplDutiesAmt1.MinimumHeight = 20;
                            tableEarnings.AddCell(cellSplDutiesAmt1);
                        }

                        forConvert = Convert.ToSingle(dt.Rows[i]["Otamt"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellIncentives = new PdfPCell(new Phrase("ED Amt", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellIncentives.HorizontalAlignment = 0;
                            cellIncentives.Colspan = 1;
                            //cellIncentives.MinimumHeight = 20;
                            tableEarnings.AddCell(cellIncentives);

                            forConvert1 = Convert.ToSingle(dt.Rows[i]["otrate"].ToString());
                            PdfPCell cellotrate11 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellotrate11.HorizontalAlignment = 2;
                            cellotrate11.Colspan = 1;
                            //cellIncentives11.MinimumHeight = 20;
                            tableEarnings.AddCell(cellotrate11);

                            PdfPCell cellotamt1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellotamt1.HorizontalAlignment = 2;
                            cellotamt1.Colspan = 1;
                            tableEarnings.AddCell(cellotamt1);
                        }




                        PdfPCell ChildTable1 = new PdfPCell(tableEarnings);
                        ChildTable1.Colspan = 3;
                        ChildTable1.BorderWidthLeft = 0;
                        ChildTable1.BorderWidthRight = 0;
                        ChildTable1.BorderWidthLeft = 0;
                        ChildTable1.BorderWidthLeft = 0;
                        tablewageslip.AddCell(ChildTable1);



                        PdfPTable tableDeductions = new PdfPTable(2);
                        tableDeductions.TotalWidth = 220;
                        tableDeductions.LockedWidth = true;
                        float[] width2 = new float[] { 2f, 2f };
                        tableDeductions.SetWidths(width2);


                        PdfPCell cellHead11 = new PdfPCell(new Phrase("Deductions", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellHead11.HorizontalAlignment = 1;
                        cellHead11.Colspan = 1;
                        cellHead11.MinimumHeight = 20;
                        cellHead11.BackgroundColor = color;
                        tableDeductions.AddCell(cellHead11);


                        PdfPCell cellHead12 = new PdfPCell(new Phrase("Amount", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellHead12.HorizontalAlignment = 1;
                        cellHead12.Colspan = 1;
                        cellHead12.BackgroundColor = color;
                        tableDeductions.AddCell(cellHead12);

                        forConvert = Convert.ToSingle(dt.Rows[i]["PF"].ToString());
                        if (forConvert > 0)
                        {

                            PdfPCell cellPF2 = new PdfPCell(new Phrase("Provident Fund", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellPF2.HorizontalAlignment = 0;
                            cellPF2.Colspan = 1;
                            //cellPF2.MinimumHeight = 20;
                            tableDeductions.AddCell(cellPF2);


                            PdfPCell cellPF = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellPF.HorizontalAlignment = 2;
                            cellPF.Colspan = 1;
                            tableDeductions.AddCell(cellPF);
                        }


                        forConvert = Convert.ToSingle(dt.Rows[i]["ESI"].ToString());
                        if (forConvert > 0)
                        {

                            PdfPCell cellESI2 = new PdfPCell(new Phrase("ESI Contribution", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellESI2.HorizontalAlignment = 0;
                            cellESI2.Colspan = 1;
                            //cellESI2.MinimumHeight = 20;
                            tableDeductions.AddCell(cellESI2);


                            PdfPCell cellESI3 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellESI3.HorizontalAlignment = 2;
                            cellESI3.Colspan = 1;
                            tableDeductions.AddCell(cellESI3);
                        }


                        forConvert = Convert.ToSingle(dt.Rows[i]["ProfTax"].ToString());
                        if (forConvert > 0)
                        {
                            PdfPCell ccellHRA2 = new PdfPCell(new Phrase("Professional Tax", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            ccellHRA2.HorizontalAlignment = 0;
                            ccellHRA2.Colspan = 1;
                            //ccellHRA2.MinimumHeight = 20;
                            tableDeductions.AddCell(ccellHRA2);


                            PdfPCell ccellHRA3 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            ccellHRA3.HorizontalAlignment = 2;
                            ccellHRA3.Colspan = 1;
                            tableDeductions.AddCell(ccellHRA3);
                        }



                        forConvert = Convert.ToSingle(dt.Rows[i]["TDS"].ToString());
                        if (forConvert > 0)
                        {
                            PdfPCell cellTDS2 = new PdfPCell(new Phrase("TDS", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellTDS2.HorizontalAlignment = 0;
                            cellTDS2.Colspan = 1;
                            //cellTDS2.MinimumHeight = 20;
                            tableDeductions.AddCell(cellTDS2);


                            PdfPCell cellTDS3 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellTDS3.HorizontalAlignment = 2;
                            cellTDS3.Colspan = 1;
                            tableDeductions.AddCell(cellTDS3);
                        }


                        forConvert = 0;

                        if (forConvert > 0)
                        {
                            PdfPCell cellAdvances2 = new PdfPCell(new Phrase("Advances", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellAdvances2.HorizontalAlignment = 0;
                            cellAdvances2.Colspan = 1;
                            //cellAdvances2.MinimumHeight = 20;
                            tableDeductions.AddCell(cellAdvances2);


                            PdfPCell cellAdvances3 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellAdvances3.HorizontalAlignment = 2;
                            cellAdvances3.Colspan = 1;
                            tableDeductions.AddCell(cellAdvances3);

                        }


                        forConvert = Convert.ToSingle(dt.Rows[i]["Fines"].ToString());
                        if (forConvert > 0)
                        {
                            PdfPCell cellFines2 = new PdfPCell(new Phrase("Fines/Damage", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellFines2.HorizontalAlignment = 0;
                            cellFines2.Colspan = 1;
                            cellFines2.MinimumHeight = 20;
                            tableDeductions.AddCell(cellFines2);


                            PdfPCell cellFines3 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellFines3.HorizontalAlignment = 2;
                            cellFines3.Colspan = 1;
                            tableDeductions.AddCell(cellFines3);

                        }



                        forConvert = Convert.ToSingle(dt.Rows[i]["SalAdvDed"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellsaladvded = new PdfPCell(new Phrase("Salary Advance", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellsaladvded.HorizontalAlignment = 0;
                            cellsaladvded.Colspan = 1;
                            //cellsaladvded.MinimumHeight = 20;
                            tableDeductions.AddCell(cellsaladvded);


                            PdfPCell cellsaladvded1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellsaladvded1.HorizontalAlignment = 2;
                            cellsaladvded1.Colspan = 1;
                            tableDeductions.AddCell(cellsaladvded1);

                        }

                        forConvert = Convert.ToSingle(dt.Rows[i]["UniformDed"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell celluniformded = new PdfPCell(new Phrase("Uniform", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            celluniformded.HorizontalAlignment = 0;
                            celluniformded.Colspan = 1;
                            // celluniformded.MinimumHeight = 20;
                            tableDeductions.AddCell(celluniformded);


                            PdfPCell celluniformded1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            celluniformded1.HorizontalAlignment = 2;
                            celluniformded1.Colspan = 1;
                            tableDeductions.AddCell(celluniformded1);

                        }


                        forConvert = Convert.ToSingle(dt.Rows[i]["RoomRentDed"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellbnkfeeded = new PdfPCell(new Phrase("Room Rent", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellbnkfeeded.HorizontalAlignment = 0;
                            cellbnkfeeded.Colspan = 1;
                            //cellbnkfeeded.MinimumHeight = 20;
                            tableDeductions.AddCell(cellbnkfeeded);


                            PdfPCell cellbnkfeeded1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellbnkfeeded1.HorizontalAlignment = 2;
                            cellbnkfeeded1.Colspan = 1;
                            tableDeductions.AddCell(cellbnkfeeded1);

                        }

                        forConvert = Convert.ToSingle(dt.Rows[i]["OtherDedn"].ToString());
                        if (forConvert > 0)
                        {
                            PdfPCell cellOtherDed = new PdfPCell(new Phrase("Other Deductions", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellOtherDed.HorizontalAlignment = 0;
                            cellOtherDed.Colspan = 1;
                            //cellOtherDed.MinimumHeight = 20;
                            tableDeductions.AddCell(cellOtherDed);


                            PdfPCell cellOtherDed1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellOtherDed1.HorizontalAlignment = 2;
                            cellOtherDed1.Colspan = 1;
                            tableDeductions.AddCell(cellOtherDed1);

                        }

                        forConvert = Convert.ToSingle(dt.Rows[i]["CanteenAdv"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellMessDed2 = new PdfPCell(new Phrase("Mess Deduction", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellMessDed2.HorizontalAlignment = 0;
                            cellMessDed2.Colspan = 1;
                            //cellMessDed2.MinimumHeight = 20;
                            tableDeductions.AddCell(cellMessDed2);


                            PdfPCell cellMessDed3 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellMessDed3.HorizontalAlignment = 2;
                            cellMessDed3.Colspan = 1;
                            tableDeductions.AddCell(cellMessDed3);

                        }


                        forConvert = Convert.ToSingle(dt.Rows[i]["SecurityDepDed"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellsecdepded = new PdfPCell(new Phrase("Security Deposit", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellsecdepded.HorizontalAlignment = 0;
                            cellsecdepded.Colspan = 1;
                            //cellsecdepded.MinimumHeight = 20;
                            tableDeductions.AddCell(cellsecdepded);


                            PdfPCell cellsecdepded1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellsecdepded1.HorizontalAlignment = 2;
                            cellsecdepded1.Colspan = 1;
                            tableDeductions.AddCell(cellsecdepded1);

                        }

                        forConvert = Convert.ToSingle(dt.Rows[i]["GeneralDed"].ToString());

                        if (forConvert > 0)
                        {
                            PdfPCell cellGeneralded = new PdfPCell(new Phrase("General Ded", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellGeneralded.HorizontalAlignment = 0;
                            cellGeneralded.Colspan = 1;
                            //cellGeneralded.MinimumHeight = 20;
                            tableDeductions.AddCell(cellGeneralded);


                            PdfPCell cellGeneralded1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellGeneralded1.HorizontalAlignment = 2;
                            cellGeneralded1.Colspan = 1;
                            tableDeductions.AddCell(cellGeneralded1);

                        }



                        //OWF or SBS
                        forConvert = Convert.ToSingle(dt.Rows[i]["owf"].ToString());
                        if (forConvert > 0)
                        {
                            PdfPCell cellSBS = new PdfPCell(new Phrase("Lab. Welfare Fund", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellSBS.HorizontalAlignment = 0;
                            cellSBS.Colspan = 1;
                            //cellSBS.MinimumHeight = 20;
                            tableDeductions.AddCell(cellSBS);


                            PdfPCell cellSBS1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellSBS1.HorizontalAlignment = 2;
                            cellSBS1.Colspan = 1;
                            tableDeductions.AddCell(cellSBS1);

                        }

                        forConvert = Convert.ToSingle(dt.Rows[i]["TDS"].ToString());
                        if (forConvert > 0)
                        {
                            PdfPCell cellempty2 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellempty2.HorizontalAlignment = 0;
                            cellempty2.Colspan = 1;
                            //cellempty2.MinimumHeight = 20;
                            tableDeductions.AddCell(cellempty2);


                            PdfPCell cellempty3 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                            cellempty3.HorizontalAlignment = 2;
                            cellempty3.Colspan = 1;
                            tableDeductions.AddCell(cellempty3);

                        }



                        PdfPCell ChildTable2 = new PdfPCell(tableDeductions);
                        ChildTable2.Colspan = 2;
                        ChildTable2.BorderWidthLeft = 0;
                        ChildTable2.BorderWidthRight = 0;
                        ChildTable2.BorderWidthLeft = 0;
                        ChildTable2.BorderWidthLeft = 0;
                        tablewageslip.AddCell(ChildTable2);


                        totalstandradamt = totalcdBasic + totalcdNHS + totalcdDA + totalcdHRA + totalcdCCA + totalcdWashAllowance + totalcdConveyance + totalcdBonus + totalcdLeaveAmount + totalcdNFhs + totalcdGratuity + totalcdotherAllowance + totalcdRC;


                        PdfPCell cellTotal12 = new PdfPCell(new Phrase("Total", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellTotal12.HorizontalAlignment = 0;
                        cellTotal12.Colspan = 1;
                        //cellTotal.MinimumHeight = 20;
                        tablewageslip.AddCell(cellTotal12);


                        PdfPCell cellTotalAmt11 = new PdfPCell(new Phrase(totalstandradamt.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellTotalAmt11.HorizontalAlignment = 2;
                        cellTotalAmt11.Colspan = 1;
                        //cellTotal11.MinimumHeight = 20;
                        tablewageslip.AddCell(cellTotalAmt11);

                        forConvert = Convert.ToSingle(dt.Rows[i]["Total"].ToString());

                        PdfPCell cellTotalamt1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellTotalamt1.HorizontalAlignment = 2;
                        cellTotalamt1.Colspan = 1;
                        tablewageslip.AddCell(cellTotalamt1);



                        PdfPCell cellTotalDed = new PdfPCell(new Phrase("Total Deductions", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellTotalDed.HorizontalAlignment = 0;
                        cellTotalDed.Colspan = 1;
                        tablewageslip.AddCell(cellTotalDed);

                        forConvert = Convert.ToSingle(dt.Rows[i]["Deductions"].ToString());
                        PdfPCell cellTotalDed1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellTotalDed1.HorizontalAlignment = 2;
                        cellTotalDed1.Colspan = 1;
                        tablewageslip.AddCell(cellTotalDed1);






                        PdfPCell cellTotal = new PdfPCell(new Phrase("Net Pay", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellTotal.HorizontalAlignment = 0;
                        cellTotal.Colspan = 1;
                        // cellTotal.MinimumHeight = 20;
                        tablewageslip.AddCell(cellTotal);

                        PdfPCell cellTotal11 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellTotal11.HorizontalAlignment = 2;
                        cellTotal11.Colspan = 1;
                        //cellTotal11.MinimumHeight = 20;
                        tablewageslip.AddCell(cellTotal11);

                        forConvert = Convert.ToSingle(dt.Rows[i]["Actualamount"].ToString());
                        string gtotal = NumberToEnglish.Instance.changeNumericToWords(forConvert.ToString("#"));

                        PdfPCell cellTotal1 = new PdfPCell(new Phrase("Rs. " + forConvert.ToString("0.00"), FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        cellTotal1.HorizontalAlignment = 2;
                        cellTotal1.Colspan = 1;
                        tablewageslip.AddCell(cellTotal1);

                        PdfPCell cellEmptycell = new PdfPCell(new Phrase("  ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellEmptycell.HorizontalAlignment = 0;
                        cellEmptycell.Colspan = 2;
                        //cellIncentives.MinimumHeight = 20;
                        tablewageslip.AddCell(cellEmptycell);




                        PdfPCell cellInWords = new PdfPCell(new Phrase("Rupees in Words " + gtotal.Trim(), FontFactory.GetFont(fontsyle, Fontsize, Font.ITALIC, BaseColor.BLACK)));
                        cellInWords.HorizontalAlignment = 0;
                        cellInWords.Colspan = 5;
                        cellInWords.Border = 0;
                        tablewageslip.AddCell(cellInWords);


                        PdfPCell cellemptycell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellemptycell.HorizontalAlignment = 0;
                        cellemptycell.Colspan = 5;
                        cellemptycell.BorderWidthLeft = 0;
                        cellemptycell.BorderWidthRight = 0;
                        cellemptycell.BorderWidthTop = 0;
                        cellemptycell.BorderWidthBottom = .5f;
                        tablewageslip.AddCell(cellemptycell);

                        PdfPCell companyname1comp = new PdfPCell(new Phrase(" This is computer generated wage slip which requires no signature", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLDITALIC, BaseColor.BLACK)));
                        companyname1comp.HorizontalAlignment = 2;
                        companyname1comp.Colspan = 5;
                        companyname1comp.Border = 0;
                        companyname1comp.PaddingBottom = 10;
                        tablewageslip.AddCell(companyname1comp);

                        PdfPCell cellcmnyadd1 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 2, Font.NORMAL, BaseColor.BLACK)));
                        cellcmnyadd1.HorizontalAlignment = 2;
                        cellcmnyadd1.Colspan = 5;
                        cellcmnyadd1.MinimumHeight = 30;
                        cellcmnyadd1.Border = 0;
                        cellcmnyadd1.PaddingBottom = 10;
                        tablewageslip.AddCell(cellcmnyadd1);




                        PdfPCell companyname1 = new PdfPCell(new Phrase("Employee Signature  ", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.WHITE)));
                        companyname1.HorizontalAlignment = 0;
                        companyname1.Colspan = 2;
                        companyname1.Border = 0;
                        companyname1.PaddingBottom = 30;
                        tablewageslip.AddCell(companyname1);



                        PdfPCell cellsignature = new PdfPCell(new Phrase("Employer Seal & Sign", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.WHITE)));
                        cellsignature.HorizontalAlignment = 2;
                        cellsignature.Colspan = 3;
                        cellsignature.Border = 0;
                        tablewageslip.AddCell(cellsignature);


                        document.Add(tablewageslip);

                        slipsCount++;
                        if (slipsCount == 2)
                        {
                            slipsCount = 0;
                            document.NewPage();
                        }



                        #endregion Basic Information of the Employee

                    }
                }

                string filename = ddlEmpName.SelectedValue + "/WageSlips.pdf";

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();

            }

        }

        string mvalue = "";
        string monthval = "";
        string Yr = "";
        string yearvalue = "";

        protected void gvattendancezero_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label month = (((Label)e.Row.FindControl("lblmonth")));
                mvalue = month.Text;


                if (mvalue.Length == 3)
                {
                    monthval = mvalue.Substring(0, 1);

                    if (monthval == "1")
                    {
                        monthval = "Jan -";
                    }
                    if (monthval == "2")
                    {
                        monthval = "Feb -";
                    }
                    if (monthval == "3")
                    {
                        monthval = "March -";
                    }
                    if (monthval == "4")
                    {
                        monthval = "April -";
                    }
                    if (monthval == "5")
                    {
                        monthval = "May -";
                    }
                    if (monthval == "6")
                    {
                        monthval = "Jun -";
                    }
                    if (monthval == "7")
                    {
                        monthval = "July -";
                    }
                    if (monthval == "8")
                    {
                        monthval = "Aug -";
                    }
                    if (monthval == "9")
                    {
                        monthval = "Sep -";
                    }
                }
                else
                {
                    monthval = mvalue.Substring(0, 2);

                    if (monthval == "10")
                    {
                        monthval = "Oct -";
                    }

                    if (monthval == "11")
                    {
                        monthval = "Nov -";
                    }
                    if (monthval == "12")
                    {
                        monthval = "Dec -";
                    }
                }


                if (mvalue.Length == 3)
                {
                    yearvalue = mvalue.Substring(1, 2);
                }
                else
                {

                    yearvalue = mvalue.Substring(2, 2);
                }




                ((Label)e.Row.FindControl("lblmonth")).Text = monthval + yearvalue;
            }

        }
    }
}