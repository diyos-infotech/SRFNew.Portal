using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using System.Globalization;
using System.Collections;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class ReportForBulkpaysheetforclients : System.Web.UI.Page
    {
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        string Fontstyle = "";
        string CFontstyle = "";
        DataRow r1 = null;

        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil util = new GridViewExportUtil();

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


            }
        }


        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();

        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
            if (ddltype.SelectedIndex == 0)
            {
                if (txtmonth.Text.Trim().Length == 0)
                {
                    LblResult.Text = "Please Select Month";
                    return;
                }
            }
            else if (ddltype.SelectedIndex == 1)
            {
                if (txtfrommonth.Text.Trim().Length == 0 || txttomonth.Text.Trim().Length == 0)
                {
                    LblResult.Text = "Please Select From Month and To Month";
                    return;
                }
            }

            var testDate = 0;
            var Month = "";
            var FromMonth = "";
            var ToMonth = "";
            if (txtmonth.Text.Trim().Length > 0)
            {
                testDate = GlobalData.Instance.CheckEnteredDate(txtmonth.Text);
                if (testDate > 0)
                {
                    LblResult.Text = "You Are Entered Invalid Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                    return;
                }

            }
            if (ddltype.SelectedIndex == 0)
            {
                string date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                string month = DateTime.Parse(date).Month.ToString();
                string Year = DateTime.Parse(date).Year.ToString();
                Month = month + Year.Substring(2, 2);
            }

            else if (ddltype.SelectedIndex == 1)
            {
                string FromDate = DateTime.Parse(txtfrommonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                string ToDate = DateTime.Parse(txttomonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                string From = DateTime.Parse(FromDate).Month.ToString("00");
                string FromYear = DateTime.Parse(FromDate).Year.ToString();
                string To = DateTime.Parse(ToDate).Month.ToString("00");
                string ToYear = DateTime.Parse(ToDate).Year.ToString();
                FromMonth = FromYear.Substring(2, 2) + From;
                ToMonth = ToYear.Substring(2, 2) + To;
            }


            #region Begin Variable Declaration as on [04-07-2014]
            //var Month = "";
            var SPName = "";
            var clientids = "";
            DataTable DtClientListBasedonSelectedMonth = null;
            Hashtable HtClientListBasedonSelectedMonth = new Hashtable();
            #endregion End  Variable Declaration as on [04-07-2014]

            #region Begin Assign Values To Vriables as on [04-07-2014]


            SPName = "GEtAllclientsListForPaysheetBulkprints";
            #endregion  End Assing Values to The Variables as on [04-07-2014]

            #region Begin  Pass Values To the Hash table as on [04-07-2014]
            if (ddltype.SelectedIndex == 0)
            {
                HtClientListBasedonSelectedMonth.Add("@month", Month);
                HtClientListBasedonSelectedMonth.Add("@Option", ddltype.SelectedIndex);
            }
            else if (ddltype.SelectedIndex == 1)
            {
                HtClientListBasedonSelectedMonth.Add("@FromMonth", FromMonth);
                HtClientListBasedonSelectedMonth.Add("@ToMonth", ToMonth);
                HtClientListBasedonSelectedMonth.Add("@Option", ddltype.SelectedIndex);
            }
            //HtClientListBasedonSelectedMonth.Add("@clientids", clientids);
            #endregion end Pass Values To the Hash table as on [04-07-2014]

            #region  Begin Call Sp on [04-07-2014]
            DtClientListBasedonSelectedMonth = config.ExecuteAdaptorAsyncWithParams(SPName, HtClientListBasedonSelectedMonth).Result;
            GVListEmployees.DataSource = DtClientListBasedonSelectedMonth;
            GVListEmployees.DataBind();
            #endregion  end Call Sp on [04-07-2014]
        }

        protected void Bindata(string Sqlqry)
        {
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                GVListEmployees.DataSource = dt;
                GVListEmployees.DataBind();
                // Fillpfandesidetails();
            }
            else
            {
                LblResult.Text = "There Is No Salary Details For The Selected client";
            }
        }

        public string GetMonthName()
        {
            string monthname = string.Empty;
            int payMonth = 0;
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();



            DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            monthname = mfi.GetMonthName(date.Month).ToString();
            //payMonth = GetMonth(monthname);

            return monthname;
        }

        public string GetMonthOfYear()
        {
            string MonthYear = "";

            int month = GetMonthBasedOnSelectionDateorMonth();
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

        public int GetMonthBasedOnSelectionDateorMonth()
        {

            var testDate = 0;
            string EnteredDate = "";

            #region Validation

            if (txtmonth.Text.Trim().Length > 0)
            {

                try
                {

                    testDate = GlobalData.Instance.CheckEnteredDate(txtmonth.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return 0;
                    }
                    EnteredDate = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return 0;
                }
            }
            #endregion


            #region  Month Get Based on the Control Selection
            int month = 0;

            DateTime date = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            month = Timings.Instance.GetIdForEnteredMOnth(date);
            return month;


            #endregion
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            //var list = new List<string>();
            var list = new List<Tuple<int, string>>();

            if (GVListEmployees.Rows.Count > 0)
            {
                for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                {
                    CheckBox chkclientid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                    Label lblclientid = GVListEmployees.Rows[i].FindControl("lblclientid") as Label;
                    Label lblmonth = GVListEmployees.Rows[i].FindControl("lblmonth") as Label;
                    if (chkclientid.Checked == true)
                    {
                        // list.Add("'" + lblclientid.Text + "'");
                        list.Add(Tuple.Create(Convert.ToInt32(lblmonth.Text), lblclientid.Text));
                    }
                }
            }

            DataTable dtClientList = new DataTable();
            DataTable dtmonth = new DataTable();
            dtClientList.Columns.Add("Clientid");
            dtmonth.Columns.Add("month");
            if (list.Count != 0)
            {
                foreach (Tuple<int, string> tuple in list)
                {
                    DataRow row = dtClientList.NewRow();
                    DataRow row1 = dtmonth.NewRow();
                    row["Clientid"] = tuple.Item2;
                    row1["month"] = tuple.Item1;
                    dtClientList.Rows.Add(row["Clientid"]);
                    dtmonth.Rows.Add(row1["month"]);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please select Client IDs');", true);
            }

            string Spname = "";
            Hashtable HtClients = new Hashtable();

            if (GVListEmployees.Rows.Count > 0)
            {
                var SPName = "BulkPaysheetPrint";
                Hashtable ht = new Hashtable();
                ht.Add("@ClientIDList", dtClientList);
                ht.Add("@MonthList", dtmonth);
                ht.Add("@Option", ddlOptions.SelectedIndex);

                DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;
                if (ddlOptions.SelectedIndex == 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                        if (ddlOptions.SelectedIndex == 0)
                        {
                            util.Export("Allpayments.xls", this.GridView1);
                        }
                    }
                }

                if (ddlOptions.SelectedIndex == 2)
                {

                    ExcelData();
                }
                else
                {
                    btndutieswithots_Click(sender, e);
                    return;
                }

            }

        }

        public void ExcelData()
        {
            try
            {


              
                var Gendays = 0;
                var formatInfoinfo = new DateTimeFormatInfo();
                string[] monthName = formatInfoinfo.MonthNames;
                string monthname = string.Empty;
                DateTimeFormatInfo mfi = new DateTimeFormatInfo();



                DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                monthname = mfi.GetMonthName(date.Month).ToString();
                //int month = GetMonthAndYear();
                int month = GetMonthBasedOnSelectionDateorMonth();

                // Gendays = Timings.Instance.GetNoofDaysForEnteredMonth(mGendays, bPaySheetDates);
                DateTime LastDay = DateTime.Now;
                
                    LastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                DataTable DtEmppaysheet = null;
                Hashtable HTEmppaysheet = new Hashtable();

                var G_Sdays = 0;

                var ContractID = "";
                var bBillDates = 0;
                var bPaySheetDates = 0;






                DataTable dt = null;
                string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
                DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
                string companyName = "Your Company Name";
                string companyAddress = "Your Company Address";

                if (compInfo.Rows.Count > 0)
                {
                    companyName = compInfo.Rows[0]["CompanyName"].ToString();
                    companyAddress = compInfo.Rows[0]["Address"].ToString();
                }





               
                string typeofwork = "";
                

                decimal totalActualamount = 0;
                decimal totalctcamount = 0;
                decimal totalDuties = 0;
                decimal totalOts = 0;
                decimal totalwo = 0;
                decimal totalnhs = 0;
                decimal totalnpots = 0;
                decimal totaltempgross = 0;
                decimal totalBasic = 0;
                decimal totalDA = 0;
                decimal totalHRA = 0;
                decimal totalTotalPay1 = 0;
                decimal totalTotalPay2 = 0;
                decimal totalTotalPay3 = 0;
                decimal totalElamount = 0;
                decimal totalLunchAmount = 0;
                decimal totalPerformanceAllw = 0;
                decimal totalleavamount = 0;
                decimal totalCCA = 0;
                decimal totalConveyance = 0;
                decimal totalWA = 0;
                decimal totalOA = 0;
                decimal totalGrass = 0;
                decimal totalGendays = 0;
                decimal totalpresentduties = 0;
                decimal totalOTAmount = 0;
                decimal totalPF = 0;
                decimal totalESI = 0;
                decimal totalProfTax = 0;
                decimal totalSalAdv = 0;
                decimal totalUniformDed = 0;
                decimal totalAdvDed = 0;
                decimal totalWCDed = 0;
                decimal totalCanteenAdv = 0;
                decimal totalLeaveEncashAmt = 0;
                decimal totalGratuity = 0;
                decimal totalBonus = 0;
                decimal totalnfhs = 0;
                decimal totalDed = 0;
                decimal totalOtherDed = 0;
                decimal totalIncentivs = 0;
                decimal totalWoAmt = 0;
                decimal totalNhsAmt = 0;
                decimal totalNpotsAmt = 0;
                decimal totalPenalty = 0;
                decimal totalRC = 0;
                decimal totalCS = 0;
                decimal totalOWF = 0;
                decimal totalSecDepDed = 0;
                decimal totalloanded = 0;
                decimal totalGenDed = 0;
                decimal totalctc = 0;

                decimal totalAttBonus = 0;
                decimal totalTravelAllw = 0;
                decimal totalNightShiftAllw = 0;
                decimal totalFoodAllowance = 0;
                decimal totalmedicalallowance = 0;
                decimal totalUniformAllw = 0;

                decimal totalAdv4Ded = 0;
                decimal totalNightRoundDed = 0;
                decimal totalManpowerMobDed = 0;
                decimal totalMobileusageDed = 0;
                decimal totalMediClaimDed = 0;
                decimal totalCrisisDed = 0;
                decimal totalMobInstDed = 0;
                decimal totalTDSDed = 0;

                decimal totalRoomRentDed = 0;
                decimal totalSpecialAllowance = 0;
                decimal totalMobileAllowance = 0;
                decimal totalNPCl25Per = 0;
                decimal totalTransport6Per = 0;
                decimal totalTransport = 0;

                decimal totalRentDed = 0;
                decimal totalMedicalDed = 0;
                decimal totalMLWFDed = 0;
                decimal totalFoodDed = 0;
                decimal totalAddlAmount = 0;


                decimal totalElectricityDed = 0;
                decimal totalTransportDed = 0;
                decimal totalDccDed = 0;
                decimal totalLeaveDed = 0;
                decimal totalLicenseDed = 0;
                decimal totalpfempr = 0;
                decimal totalesiempr = 0;
                decimal totalDiv = 0;
                decimal totalArea = 0;
                decimal totalTelephoneBillDed = 0;
                decimal totalPetrolAllowance = 0;

                string SelectmonthWithbankacno = string.Empty;
                string SelectmonthWithoutbankacno = string.Empty;
                //DataTable dt = null;



                var list = new List<Tuple<int, string>>();

                if (GVListEmployees.Rows.Count > 0)
                {
                    for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                    {
                        CheckBox chkclientid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                        Label lblclientid = GVListEmployees.Rows[i].FindControl("lblclientid") as Label;
                        Label lblmonth = GVListEmployees.Rows[i].FindControl("lblmonth") as Label;
                        if (chkclientid.Checked == true)
                        {
                            // list.Add("'" + lblclientid.Text + "'");
                            list.Add(Tuple.Create(Convert.ToInt32(lblmonth.Text), lblclientid.Text));
                        }
                    }
                }

                DataTable dtClientList = new DataTable();
                DataTable dtmonth = new DataTable();
                dtClientList.Columns.Add("Clientid");
                dtmonth.Columns.Add("month");
                if (list.Count != 0)
                {
                    foreach (Tuple<int, string> tuple in list)
                    {
                        DataRow row = dtClientList.NewRow();
                        DataRow row1 = dtmonth.NewRow();
                        row["Clientid"] = tuple.Item2;
                        row1["month"] = tuple.Item1;
                        dtClientList.Rows.Add(row["Clientid"]);
                        dtmonth.Rows.Add(row1["month"]);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please select Client IDs');", true);
                }
                var SPName = "BulkPaysheetPrint";
                Hashtable ht = new Hashtable();
                ht.Add("@ClientIDList", dtClientList);
                ht.Add("@MonthList", dtmonth);
                ht.Add("@Option", ddlOptions.SelectedIndex);

                dt = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPName, ht);

                if (dt.Rows.Count > 0)
                {
                    lbtn_Export.Visible = true;
                    GVListEmployeesNew.DataSource = dt;
                    GVListEmployeesNew.DataBind();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        decimal actAmount = 0;
                        string actualAmount = dt.Rows[i]["ActualAmount"].ToString();
                        if (actualAmount.Trim().Length > 0)
                        {
                            actAmount = Convert.ToDecimal(actualAmount);
                        }
                        //if (actAmount >= 0)
                        {
                            totalActualamount += actAmount;
                            string duties = dt.Rows[i]["NoOfDuties"].ToString();
                            if (duties.Trim().Length > 0)
                            {
                                totalDuties += Convert.ToDecimal(duties);
                            }
                            string ots = dt.Rows[i]["OTs"].ToString();
                            if (ots.Trim().Length > 0)
                            {
                                totalOts += Convert.ToDecimal(ots);
                            }

                            string wos = dt.Rows[i]["wo"].ToString();
                            if (wos.Trim().Length > 0)
                            {
                                totalwo += Convert.ToDecimal(wos);
                            }
                            string nhs = dt.Rows[i]["nhs"].ToString();
                            if (nhs.Trim().Length > 0)
                            {
                                totalnhs += Convert.ToDecimal(nhs);
                            }
                            string npots = dt.Rows[i]["npots"].ToString();
                            if (npots.Trim().Length > 0)
                            {
                                totalnpots += Convert.ToDecimal(npots);
                            }
                            string ntempgross = dt.Rows[i]["tempgross"].ToString();
                            if (ntempgross.Trim().Length > 0)
                            {
                                totaltempgross += Convert.ToDecimal(ntempgross);
                            }

                            string strBasic = dt.Rows[i]["Basic"].ToString();
                            if (strBasic.Trim().Length > 0)
                            {
                                totalBasic += Convert.ToDecimal(strBasic);
                            }
                            string strDA = dt.Rows[i]["DA"].ToString();
                            if (strDA.Trim().Length > 0)
                            {
                                totalDA += Convert.ToDecimal(strDA);
                            }
                            string strhHRA = dt.Rows[i]["HRA"].ToString();
                            if (strhHRA.Trim().Length > 0)
                            {
                                totalHRA += Convert.ToDecimal(strhHRA);
                            }
                            string strCCA = dt.Rows[i]["CCA"].ToString();
                            if (strCCA.Trim().Length > 0)
                            {
                                totalCCA += Convert.ToDecimal(strCCA);
                            }
                            string strConveyance = dt.Rows[i]["Conveyance"].ToString();
                            if (strConveyance.Trim().Length > 0)
                            {
                                totalConveyance += Convert.ToDecimal(strConveyance);
                            }
                            string strWA = dt.Rows[i]["WashAllowance"].ToString();
                            if (strWA.Trim().Length > 0)
                            {
                                totalWA += Convert.ToDecimal(strWA);
                            }
                            string strOA = dt.Rows[i]["OtherAllowance"].ToString();
                            if (strOA.Trim().Length > 0)
                            {
                                totalOA += Convert.ToDecimal(strOA);
                            }
                            string strLeaveEncashAmt = dt.Rows[i]["LeaveEncashAmt"].ToString();
                            if (strCCA.Trim().Length > 0)
                            {
                                totalLeaveEncashAmt += Convert.ToDecimal(strLeaveEncashAmt);
                            }
                            string strGratuity = dt.Rows[i]["Gratuity"].ToString();
                            if (strGratuity.Trim().Length > 0)
                            {
                                totalGratuity += Convert.ToDecimal(strGratuity);
                            }
                            string strBonus = dt.Rows[i]["Bonus"].ToString();
                            if (strBonus.Trim().Length > 0)
                            {
                                totalBonus += Convert.ToDecimal(strBonus);
                            }
                            string strNfhs = dt.Rows[i]["Nfhs"].ToString();
                            if (strNfhs.Trim().Length > 0)
                            {
                                totalnfhs += Convert.ToDecimal(strNfhs);
                            }

                            string strGross = dt.Rows[i]["Gross"].ToString();
                            if (strGross.Trim().Length > 0)
                            {
                                totalGrass += Convert.ToDecimal(strGross);
                            }


                            string strIncentivs = dt.Rows[i]["Incentivs"].ToString();
                            if (strIncentivs.Trim().Length > 0)
                            {
                                totalIncentivs += Convert.ToDecimal(strIncentivs);
                            }

                            string strOTAmount = dt.Rows[i]["OTAmt"].ToString();
                            if (strOTAmount.Trim().Length > 0)
                            {
                                totalOTAmount += Convert.ToDecimal(strOTAmount);
                            }
                            string strPF = dt.Rows[i]["PF"].ToString();
                            if (strPF.Trim().Length > 0)
                            {
                                totalPF += Convert.ToDecimal(strPF);
                            }
                            string strESI = dt.Rows[i]["ESI"].ToString();
                            if (strESI.Trim().Length > 0)
                            {
                                totalESI += Convert.ToDecimal(strESI);
                            }
                            string strProfTax = dt.Rows[i]["ProfTax"].ToString();
                            if (strProfTax.Trim().Length > 0)
                            {
                                totalProfTax += Convert.ToDecimal(strProfTax);
                            }

                            string strSalAdv = dt.Rows[i]["SalAdvDed"].ToString();
                            if (strSalAdv.Trim().Length > 0)
                            {
                                totalSalAdv += Convert.ToDecimal(strSalAdv);
                            }

                            string strUniformDed = dt.Rows[i]["UniformDed"].ToString();
                            if (strUniformDed.Trim().Length > 0)
                            {
                                totalUniformDed += Convert.ToDecimal(strUniformDed);
                            }

                            string strOtherDed = dt.Rows[i]["OtherDed"].ToString();
                            if (strOtherDed.Trim().Length > 0)
                            {
                                totalOtherDed += Convert.ToDecimal(strOtherDed);
                            }
                            string strCanteenAdv = dt.Rows[i]["CanteenAdv"].ToString();
                            if (strCanteenAdv.Trim().Length > 0)
                            {
                                totalCanteenAdv += Convert.ToDecimal(strCanteenAdv);
                            }

                            string strDed = dt.Rows[i]["TotalDeductions"].ToString();
                            if (strDed.Trim().Length > 0)
                            {
                                totalDed += Convert.ToDecimal(strDed);
                            }


                            //New code add as on 24/12/2013 by venkat

                            string strWoAmt = dt.Rows[i]["WOAmt"].ToString();
                            if (strWoAmt.Trim().Length > 0)
                            {
                                totalWoAmt += Convert.ToDecimal(strWoAmt);
                            }

                            string strNhsAmt = dt.Rows[i]["Nhsamt"].ToString();
                            if (strNhsAmt.Trim().Length > 0)
                            {
                                totalNhsAmt += Convert.ToDecimal(strNhsAmt);
                            }

                            string strNpotsAmt = dt.Rows[i]["Npotsamt"].ToString();
                            if (strNpotsAmt.Trim().Length > 0)
                            {
                                totalNpotsAmt += Convert.ToDecimal(strNpotsAmt);
                            }

                            string strPenalty = dt.Rows[i]["Penalty"].ToString();
                            if (strPenalty.Trim().Length > 0)
                            {
                                totalPenalty += Convert.ToDecimal(strPenalty);
                            }

                            string strRC = dt.Rows[i]["RC"].ToString();
                            if (strRC.Trim().Length > 0)
                            {
                                totalRC += Convert.ToDecimal(strRC);
                            }

                            string strCS = dt.Rows[i]["CS"].ToString();
                            if (strCS.Trim().Length > 0)
                            {
                                totalCS += Convert.ToDecimal(strCS);
                            }

                            string strOWF = dt.Rows[i]["OWF"].ToString();
                            if (strOWF.Trim().Length > 0)
                            {
                                totalOWF += Convert.ToDecimal(strOWF);
                            }

                            string strSecDep = dt.Rows[i]["SecurityDepDed"].ToString();
                            if (strSecDep.Trim().Length > 0)
                            {
                                totalSecDepDed += Convert.ToDecimal(strSecDep);
                            }

                            string strRoomRent = dt.Rows[i]["RoomRentDed"].ToString();
                            if (strRoomRent.Trim().Length > 0)
                            {
                                totalRoomRentDed += Convert.ToDecimal(strRoomRent);
                            }

                            string strGeneralDed = dt.Rows[i]["GeneralDed"].ToString();
                            if (strGeneralDed.Trim().Length > 0)
                            {
                                totalGenDed += Convert.ToDecimal(strGeneralDed);
                            }

                        }
                    }


                    #region for old code


                    #endregion for old code


                    Label lblTotalNetAmount = GVListEmployeesNew.FooterRow.FindControl("lblTotalNetAmount") as Label;
                    lblTotalNetAmount.Text = Math.Round(totalActualamount).ToString();

                    Label lblTotalDuties = GVListEmployeesNew.FooterRow.FindControl("lblTotalDuties") as Label;
                    lblTotalDuties.Text = Math.Round(totalDuties).ToString();

                    Label lblTotaltempgross = GVListEmployeesNew.FooterRow.FindControl("lblTotaltempgross") as Label;
                    lblTotaltempgross.Text = Math.Round(totaltempgross).ToString();

                    Label lblTotalBasic = GVListEmployeesNew.FooterRow.FindControl("lblTotalBasic") as Label;
                    lblTotalBasic.Text = Math.Round(totalBasic).ToString();

                    Label lblTotalGross = GVListEmployeesNew.FooterRow.FindControl("lblTotalGross") as Label;
                    lblTotalGross.Text = Math.Round(totalGrass).ToString();


                    Label lblTotalOTs = GVListEmployeesNew.FooterRow.FindControl("lblTotalOTs") as Label;
                    lblTotalOTs.Text = Math.Round(totalOts).ToString();



                    Label lblTotalPF = GVListEmployeesNew.FooterRow.FindControl("lblTotalPF") as Label;
                    lblTotalPF.Text = Math.Round(totalPF).ToString();



                    Label lblTotalESI = GVListEmployeesNew.FooterRow.FindControl("lblTotalESI") as Label;
                    lblTotalESI.Text = Math.Round(totalESI).ToString();



                    Label lblTotalDA = GVListEmployeesNew.FooterRow.FindControl("lblTotalDA") as Label;
                    lblTotalDA.Text = Math.Round(totalDA).ToString();


                    Label lblTotalHRA = GVListEmployeesNew.FooterRow.FindControl("lblTotalHRA") as Label;
                    lblTotalHRA.Text = Math.Round(totalHRA).ToString();



                    Label lblTotalCCA = GVListEmployeesNew.FooterRow.FindControl("lblTotalCCA") as Label;
                    lblTotalCCA.Text = Math.Round(totalCCA).ToString();



                    Label lblTotalConveyance = GVListEmployeesNew.FooterRow.FindControl("lblTotalConveyance") as Label;
                    lblTotalConveyance.Text = Math.Round(totalConveyance).ToString();



                    Label lblTotalWA = GVListEmployeesNew.FooterRow.FindControl("lblTotalWA") as Label;
                    lblTotalWA.Text = Math.Round(totalWA).ToString();



                    Label lblTotalOA = GVListEmployeesNew.FooterRow.FindControl("lblTotalOA") as Label;
                    lblTotalOA.Text = Math.Round(totalOA).ToString();




                    Label lblTotalLeaveEncashAmt = GVListEmployeesNew.FooterRow.FindControl("lblTotalLeaveEncashAmt") as Label;
                    lblTotalLeaveEncashAmt.Text = Math.Round(totalLeaveEncashAmt).ToString();


                    Label lblTotalGratuity = GVListEmployeesNew.FooterRow.FindControl("lblTotalGratuity") as Label;
                    lblTotalGratuity.Text = Math.Round(totalGratuity).ToString();



                    Label lblTotalBonus = GVListEmployeesNew.FooterRow.FindControl("lblTotalBonus") as Label;
                    lblTotalBonus.Text = Math.Round(totalBonus).ToString();




                    Label lblTotalNfhs = GVListEmployeesNew.FooterRow.FindControl("lblTotalNfhs") as Label;
                    lblTotalNfhs.Text = Math.Round(totalnfhs).ToString();



                    Label lblTotalrc = GVListEmployeesNew.FooterRow.FindControl("lblTotalrc") as Label;
                    lblTotalrc.Text = Math.Round(totalRC).ToString();



                    Label lblTotalcs = GVListEmployeesNew.FooterRow.FindControl("lblTotalcs") as Label;
                    lblTotalcs.Text = Math.Round(totalCS).ToString();





                    Label lblTotalIncentivs = GVListEmployeesNew.FooterRow.FindControl("lblTotalIncentivs") as Label;
                    lblTotalIncentivs.Text = Math.Round(totalIncentivs).ToString();






                    Label lblTotalWOAmount = GVListEmployeesNew.FooterRow.FindControl("lblTotalWOAmount") as Label;
                    lblTotalWOAmount.Text = Math.Round(totalWoAmt).ToString();



                    Label lblTotalNhsAmount = GVListEmployeesNew.FooterRow.FindControl("lblTotalNhsAmount") as Label;
                    lblTotalNhsAmount.Text = Math.Round(totalNhsAmt).ToString();




                    Label lblTotalNpotsAmount = GVListEmployeesNew.FooterRow.FindControl("lblTotalNpotsAmount") as Label;
                    lblTotalNpotsAmount.Text = Math.Round(totalNpotsAmt).ToString();




                    Label lblTotalOTAmount = GVListEmployeesNew.FooterRow.FindControl("lblTotalOTAmount") as Label;
                    lblTotalOTAmount.Text = Math.Round(totalOTAmount).ToString();




                    Label lblTotalProfTax = GVListEmployeesNew.FooterRow.FindControl("lblTotalProfTax") as Label;
                    lblTotalProfTax.Text = Math.Round(totalProfTax).ToString();



                    Label lblTotalsaladv = GVListEmployeesNew.FooterRow.FindControl("lblTotalsaladv") as Label;
                    lblTotalsaladv.Text = Math.Round(totalSalAdv).ToString();



                    Label lblTotalUniformDed = GVListEmployeesNew.FooterRow.FindControl("lblTotalUniformDed") as Label;
                    lblTotalUniformDed.Text = Math.Round(totalUniformDed).ToString();




                    Label lblTotalOtherDed = GVListEmployeesNew.FooterRow.FindControl("lblTotalOtherDed") as Label;
                    lblTotalOtherDed.Text = Math.Round(totalOtherDed).ToString();



                    Label lblTotalRoomRentDed = GVListEmployeesNew.FooterRow.FindControl("lblTotalRoomRentDed") as Label;
                    lblTotalRoomRentDed.Text = Math.Round(totalRoomRentDed).ToString();



                    Label lblTotalCanteenAdv = GVListEmployeesNew.FooterRow.FindControl("lblTotalcantadv") as Label;
                    lblTotalCanteenAdv.Text = Math.Round(totalCanteenAdv).ToString();





                    Label lblTotalSecDepDed = GVListEmployeesNew.FooterRow.FindControl("lblTotalSecDepDed") as Label;
                    lblTotalSecDepDed.Text = Math.Round(totalSecDepDed).ToString();





                    Label lblTotalGeneralDed = GVListEmployeesNew.FooterRow.FindControl("lblTotalGeneralDed") as Label;
                    lblTotalGeneralDed.Text = Math.Round(totalGenDed).ToString();




                    Label lblTotalowf = GVListEmployeesNew.FooterRow.FindControl("lblTotalowf") as Label;
                    lblTotalowf.Text = Math.Round(totalOWF).ToString();


                    Label lblTotalPenalty = GVListEmployeesNew.FooterRow.FindControl("lblTotalPenalty") as Label;
                    lblTotalPenalty.Text = Math.Round(totalPenalty).ToString();





                    Label lblTotalDeductions = GVListEmployeesNew.FooterRow.FindControl("lblTotalDeductions") as Label;
                    lblTotalDeductions.Text = Math.Round(totalDed).ToString();


                    //New code add as on 2021/12/07 by Mahesh Goud




                }



            }
            catch (Exception ex)
            {

            }
        }

        protected void GVListEmployeesNew_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes.Add("class", "text");
                e.Row.Cells[40].Attributes.Add("class", "text");
            }

        }
        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            ExcelData();
            try
            {
                var formatInfoinfo = new DateTimeFormatInfo();
                string[] monthName = formatInfoinfo.MonthNames;
                int month = GetMonthBasedOnSelectionDateorMonth();
                string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
                DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
                string companyName = "Your Company Name";
                string companyAddress = "Your Company Address";

                if (compInfo.Rows.Count > 0)
                {
                    companyName = compInfo.Rows[0]["CompanyName"].ToString();
                    companyAddress = compInfo.Rows[0]["Address"].ToString();
                }

                string AddrHno = "";



             
                string typeofwork = "";
              

                int totalcount = 42;



                string line = "[ See Rule 24(9-B) of the Karnataka Shops & Commercial Establishments Rules, 1963] in lieu of";

                string line1 = "Form XVII<br>Register Of Wages<br>Wages Period Monthly<br>For The Month Of " + GetMonthName().ToUpper() + "" + GetMonthOfYear();

                string line2 = "Name & Address of the Establishment Under Which Contract in Carried on  : < br>" + companyName + "<br>" + companyAddress + "";

                string line3 = "1. Form I, II of Rule 22(4): Form IV of Rule 28(2); Form V & VII of Rule 29(1) & (5) of Karnataka Minimum wages Rules 1958;<br>2. Form I of Rules 3 (1) of Karnataka Payment of Wages Rules, 1963;<br> 3. Form XIII of Rules 75; Form XV, XVII, XX, XXI, XXII, XXIII, of Rule 78 (1) a(i), (ii) &(iii) of the Karnataka Contract Labour(Regulation & Abolition) Rules, 1974;<br> 4. Form XIII of Rule 43, Form XVII, XVIII, XIX, XX, XXI, XXII, of Rule 46(2) (a), (c) & (d) of Karnataka inter state Migrant Workmen Rules, 1981";

                string line4 = "Clientid :";

                string line5 = "Name & Address of Principal Employer : ";

                string line6 = "Client Name :";

                string line7 = "Place of Payment";
                string Filename = "Bulk Paysheet" + "" + GetMonthName().ToUpper() + "" + GetMonthOfYear() + ".xls";
                util.ExportGridNew(Filename, totalcount, line, line1, line2, line3, line4, line5, line6, line7, hidGridView);

            }
            catch (Exception ex)
            {

            }
        }
        protected void btndutieswithots_Click(object sender, EventArgs e)
        {

            DataTable dt = null;
            var listforpdf = new List<string>();

            if (GVListEmployees.Rows.Count > 0)
            {

                for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                {
                    CheckBox chkclientid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                    if (chkclientid.Checked == true)
                    {
                        Label lblclientid = GVListEmployees.Rows[i].FindControl("lblclientid") as Label;
                        Label lblclientname = GVListEmployees.Rows[i].FindControl("lblclientname") as Label;

                        if (chkclientid.Checked == true)
                        {
                            listforpdf.Add("'" + lblclientid.Text + "'");
                        }

                    }

                }


            }

            string Clientids = string.Join(",", listforpdf.ToArray());
            var list = new List<Tuple<int, string>>();
            DataTable dtClientList = new DataTable();
            DataTable dtmonth = new DataTable();
            if (GVListEmployees.Rows.Count > 0)
            {
                for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                {
                    CheckBox chkclientid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                    Label lblclientid = GVListEmployees.Rows[i].FindControl("lblclientid") as Label;
                    Label lblmonth = GVListEmployees.Rows[i].FindControl("lblmonth") as Label;
                    if (chkclientid.Checked == true)
                    {
                        list.Add(Tuple.Create(Convert.ToInt32(lblmonth.Text), lblclientid.Text));
                    }
                }
            }


            dtClientList.Columns.Add("Clientid");
            dtmonth.Columns.Add("month");
            if (list.Count != 0)
            {
                foreach (Tuple<int, string> tuple in list)
                {
                    DataRow row = dtClientList.NewRow();
                    DataRow row1 = dtmonth.NewRow();
                    row["Clientid"] = tuple.Item2;
                    row1["month"] = tuple.Item1;
                    dtClientList.Rows.Add(row["Clientid"]);
                    dtmonth.Rows.Add(row1["month"]);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please select Client IDs');", true);
            }

            var SPName = "BulkPaysheetPrint";
            Hashtable ht = new Hashtable();
            ht.Add("@ClientIDList", dtClientList);
            ht.Add("@MonthList", dtmonth);
            ht.Add("@Option", ddlOptions.SelectedIndex);

            dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;


            string clientid = "";
            string clientname = "";
            string Month = "";
            MemoryStream ms = new MemoryStream();
            if (dt.Rows.Count > 0)
            {

                #region Variables for table cells counting

                int owf = 0;
                int dts = 0;
                int srate = 0;
                int basic = 0;
                int da = 0;
                int hra = 0;
                int cca = 0;
                int conveyance = 0;
                int washallowance = 0;
                int otherallowance = 0;
                int leavewages = 0;
                int gratuity = 0;
                int bonus = 0;
                int nfhs = 0;
                int rc = 0;
                int cs = 0;
                int gross = 0;
                int incentivs = 0;
                int pfonduties = 0;
                int esionduties = 0;
                int proftax = 0;
                int salAdvDed = 0;
                int uniformDed = 0;
                int otherDed = 0;
                int canteenAdv = 0;
                int roomrent = 0;
                int totalDeductions = 0;
                int netpay = 0;

                int ots = 0;
                int wo = 0;
                int npots = 0;
                int nhs = 0;
                int woamt = 0;
                int npotsamt = 0;
                int nhsamt = 0;
                int otamt = 0;
                int pfonot = 0;
                int esionot = 0;
                int Pf = 0;
                int Esi = 0;
                int GenDedn = 0;
                int SecDepDedn = 0;
                int subtotal = 0;

                float owf1 = 0;
                float dts1 = 0;
                float srate1 = 0;
                float basic1 = 0;
                float da1 = 0;
                float hra1 = 0;
                float cca1 = 0;
                float conveyance1 = 0;
                float washallowance1 = 0;
                float otherallowance1 = 0;
                float leavewages1 = 0;
                float gratuity1 = 0;
                float bonus1 = 0;
                float nfhs1 = 0;
                float rc1 = 0;
                float cs1 = 0;
                float gross1 = 0;
                float incentivs1 = 0;
                float pfonduties1 = 0;
                float esionduties1 = 0;
                float proftax1 = 0;
                float salAdvDed1 = 0;
                float uniformDed1 = 0;
                float otherDed1 = 0;
                float canteenAdv1 = 0;
                float roomrent1 = 0;
                float totalDeductions1 = 0;
                float netpay1 = 0;
                float subtotal1 = 0;

                float ots1 = 0;
                float wo1 = 0;
                float npots1 = 0;
                float nhs1 = 0;
                float woamt1 = 0;
                float npotsamt1 = 0;
                float nhsamt1 = 0;
                float otamt1 = 0;
                float pfonot1 = 0;
                float esionot1 = 0;
                float Pf1 = 0;
                float Esi1 = 0;
                float GenDedn1 = 0;
                float SecDepDedn1 = 0;


                #endregion



                DataTable dtclientid = dt.DefaultView.ToTable(true, "clientid", "clientname", "Monthname");


                Document document = new Document(PageSize.LEGAL.Rotate());
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                PageEventHelper pageEvent = new PageEventHelper();
                pageEvent.list = Clientids;
                pageEvent.CmpIDPrefix = CmpIDPrefix;
                pageEvent.month = dtclientid.Rows[0]["Monthname"].ToString();
                pageEvent.document = document;
                pageEvent.dt = dt;
                writer.PageEvent = pageEvent;


                float forConvert;
                string strQry = "Select * from CompanyInfo   where   ClientidPrefix='" + CmpIDPrefix + "'";
                DataTable compInfo = config.ExecuteReaderWithQueryAsync(strQry).Result;
                string companyName1 = "Your Company Name";
                string companyAddress = "Your Company Address";
                if (compInfo.Rows.Count > 0)
                {
                    companyName1 = compInfo.Rows[0]["CompanyName"].ToString();
                    companyAddress = compInfo.Rows[0]["Address"].ToString();
                }

                companyAddress = dt.Rows[0]["GSTAddress"].ToString();




                #region variables for total

                float totalwoamt = 0;
                float totalowfamt = 0;
                float totalnpotsamt = 0;
                float totalnhsamt = 0;
                float totalDuties = 0;
                float totalsrate = 0;
                float totalBasic = 0;
                float totalDA = 0;
                float totalHRA = 0;
                float totalCCA = 0;
                float totalConveyance = 0;
                float totalWA = 0;
                float totalOA = 0;
                float totalLw = 0;
                float totalGratuity = 0;
                float totalbonus = 0;
                float totalNfhs = 0;
                float totalRc = 0;
                float totalCs = 0;
                float totalGross = 0;
                float totalIncentivs = 0;
                float totalPFondts = 0;
                float totalESIondts = 0;
                float totalProfTax = 0;
                float totalSalAdv = 0;
                float totalUniforDed = 0;
                float totalOtherdedn = 0;
                float totalCanteenAdv = 0;
                float totalPenalty = 0;
                float totalDed = 0;
                float totalNetpay = 0;

                float totalots = 0;
                float totalwo = 0;
                float totalnpots = 0;
                float totalnhs = 0;
                float totalotamt = 0;
                float totalpfonots = 0;
                float totalesionots = 0;
                float totalpf = 0;
                float totalesi = 0;
                float totalGenDedn = 0;
                float totalSecDepDedn = 0;

                #endregion

                #region variables for  Grand  total

                float Grandtotalwoamt = 0;
                float Grandtotalowfamt = 0;
                float Grandtotalnpotsamt = 0;
                float Grandtotalnhsamt = 0;
                float GrandtotalDuties = 0;
                float GrandtotalSrate = 0;
                float GrandtotalBasic = 0;
                float GrandtotalDA = 0;
                float GrandtotalHRA = 0;
                float GrandtotalCCA = 0;
                float GrandtotalConveyance = 0;
                float GrandtotalWA = 0;
                float GrandtotalOA = 0;
                float GrandtotalLw = 0;
                float GrandtotalGratuity = 0;
                float Grandtotalbonus = 0;
                float GrandtotalNfhs = 0;
                float GrandtotalRc = 0;
                float GrandtotalCs = 0;
                float GrandtotalGross = 0;
                float GrandtotalIncentivs = 0;
                float GrandtotalPFondts = 0;
                float GrandtotalESIondts = 0;
                float GrandtotalProfTax = 0;
                float GrandtotalSalAdv = 0;
                float GrandtotalUniforDed = 0;
                float GrandtotalOtherdedn = 0;
                float GrandtotalCanteenAdv = 0;
                float GrandtotalPenalty = 0;
                float GrandtotalDed = 0;
                float GrandtotalNetpay = 0;

                float Grandtotalots = 0;
                float Grandtotalwo = 0;
                float Grandtotalnpots = 0;
                float Grandtotalnhs = 0;
                float Grandtotalotamt = 0;
                float Grandtotalpfonots = 0;
                float Grandtotalesionots = 0;
                float Grandtotalpf = 0;
                float Grandtotalesi = 0;
                float GrandtotalGenDedn = 0;
                float GrandtotalSecDepDedn = 0;

                #endregion

                string selectclientaddress = "select * from clients where clientid  in (" + Clientids + ")";
                DataTable dtclientaddress = config.ExecuteReaderWithQueryAsync(selectclientaddress).Result;

                bool nextpagehasPages = false;

                nextpagehasPages = true;
                int targetpagerecors = 9;
                int secondpagerecords = targetpagerecors;
                uint FONT_SIZE = 8;

                pageEvent.clientid = dtclientid.Rows[0]["clientid"].ToString(); ;
                pageEvent.clientname = dtclientid.Rows[0]["clientname"].ToString(); ;
                pageEvent.month = dtclientid.Rows[0]["Monthname"].ToString(); ;
                document.Open();
                document.AddTitle("FaMS");
                document.AddAuthor("WebWonders");
                document.AddSubject("Wage Sheet");
                document.AddKeywords("Keyword1, keyword2, …");//



                int tableCells = 0;

                for (int k = 0; k < dtclientid.Rows.Count; k++)
                {

                    tableCells = 0;

                    #region Variables for table cells counting

                    owf = 0;
                    dts = 0;
                    srate = 0;
                    basic = 0;
                    da = 0;
                    hra = 0;
                    cca = 0;
                    conveyance = 0;
                    washallowance = 0;
                    otherallowance = 0;
                    leavewages = 0;
                    gratuity = 0;
                    bonus = 0;
                    nfhs = 0;
                    rc = 0;
                    cs = 0;
                    gross = 0;
                    incentivs = 0;
                    pfonduties = 0;
                    esionduties = 0;
                    proftax = 0;
                    salAdvDed = 0;
                    uniformDed = 0;
                    otherDed = 0;
                    canteenAdv = 0;
                    roomrent = 0;
                    totalDeductions = 0;
                    netpay = 0;

                    ots = 0;
                    wo = 0;
                    npots = 0;
                    nhs = 0;
                    woamt = 0;
                    npotsamt = 0;
                    nhsamt = 0;
                    otamt = 0;
                    pfonot = 0;
                    esionot = 0;
                    Pf = 0;
                    Esi = 0;
                    GenDedn = 0;
                    SecDepDedn = 0;
                    subtotal = 0;

                    owf1 = 0;
                    dts1 = 0;
                    srate1 = 0;
                    basic1 = 0;
                    da1 = 0;
                    hra1 = 0;
                    cca1 = 0;
                    conveyance1 = 0;
                    washallowance1 = 0;
                    otherallowance1 = 0;
                    leavewages1 = 0;
                    gratuity1 = 0;
                    bonus1 = 0;
                    nfhs1 = 0;
                    rc1 = 0;
                    cs1 = 0;
                    gross1 = 0;
                    incentivs1 = 0;
                    pfonduties1 = 0;
                    esionduties1 = 0;
                    proftax1 = 0;
                    salAdvDed1 = 0;
                    uniformDed1 = 0;
                    otherDed1 = 0;
                    canteenAdv1 = 0;
                    roomrent1 = 0;
                    totalDeductions1 = 0;
                    netpay1 = 0;
                    subtotal1 = 0;

                    ots1 = 0;
                    wo1 = 0;
                    npots1 = 0;
                    nhs1 = 0;
                    woamt1 = 0;
                    npotsamt1 = 0;
                    nhsamt1 = 0;
                    otamt1 = 0;
                    pfonot1 = 0;
                    esionot1 = 0;
                    Pf1 = 0;
                    Esi1 = 0;
                    GenDedn1 = 0;
                    SecDepDedn1 = 0;


                    #endregion

                    #region Data for counting tablecells

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {



                        dts1 = float.Parse(dt.Rows[i]["NoOfDuties"].ToString());
                        if (dts1 != 0)
                        {
                            dts1 += 1;
                            if (dts1 > 0)
                            {
                                dts = 1;
                            }
                        }

                        owf1 = float.Parse(dt.Rows[i]["owf"].ToString());
                        if (owf1 != 0)
                        {
                            owf1 += 1;
                            if (owf1 > 0)
                            {
                                owf = 1;
                            }
                        }

                        srate1 = float.Parse(dt.Rows[i]["SalRate"].ToString());
                        if (srate1 != 0)
                        {
                            srate1 += 1;
                            if (srate1 > 0)
                            {
                                srate = 1;
                            }
                        }

                        basic1 = float.Parse(dt.Rows[i]["basic"].ToString());
                        if (basic1 != 0)
                        {
                            basic1 += 1;
                            if (basic1 > 0)
                            {
                                basic = 1;
                            }
                        }
                        da1 = float.Parse(dt.Rows[i]["da"].ToString());
                        if (da1 != 0)
                        {
                            da1 += 1;
                            if (da1 > 0)
                            {
                                da = 1;
                            }
                        }
                        hra1 = float.Parse(dt.Rows[i]["hra"].ToString());
                        if (hra1 != 0)
                        {
                            hra1 += 1;
                            if (hra1 > 0)
                            {
                                hra = 1;
                            }
                        }
                        cca1 = float.Parse(dt.Rows[i]["cca"].ToString());
                        if (cca1 != 0)
                        {
                            cca1 += 1;
                            if (cca1 > 0)
                            {
                                cca = 1;
                            }
                        }
                        conveyance1 = float.Parse(dt.Rows[i]["Conveyance"].ToString());
                        if (conveyance1 != 0)
                        {
                            conveyance1 += 1;
                            if (conveyance1 > 0)
                            {
                                conveyance = 1;
                            }
                        }

                        washallowance1 = float.Parse(dt.Rows[i]["washallowance"].ToString());
                        if (washallowance1 != 0)
                        {
                            washallowance1 += 1;
                            if (washallowance1 > 0)
                            {
                                washallowance = 1;
                            }
                        }
                        otherallowance1 = float.Parse(dt.Rows[i]["otherallowance"].ToString());
                        if (otherallowance1 != 0)
                        {
                            otherallowance1 += 1;
                            if (otherallowance1 > 0)
                            {
                                otherallowance = 1;
                            }
                        }
                        leavewages1 = float.Parse(dt.Rows[i]["Leavewages"].ToString());
                        if (leavewages1 != 0)
                        {
                            leavewages1 += 1;
                            if (leavewages1 > 0)
                            {
                                leavewages = 1;
                            }
                        }
                        gratuity1 = float.Parse(dt.Rows[i]["gratuity"].ToString());
                        if (gratuity1 != 0)
                        {
                            gratuity1 += 1;
                            if (gratuity1 > 0)
                            {
                                gratuity = 1;
                            }
                        }
                        bonus1 = float.Parse(dt.Rows[i]["bonus"].ToString());
                        if (bonus1 != 0)
                        {
                            bonus1 += 1;
                            if (bonus1 > 0)
                            {
                                bonus = 1;
                            }
                        }

                        nfhs1 = float.Parse(dt.Rows[i]["nfhs"].ToString());
                        if (nfhs1 != 0)
                        {
                            nfhs1 += 1;
                            if (nfhs1 > 0)
                            {
                                nfhs = 1;
                            }
                        }
                        rc1 = float.Parse(dt.Rows[i]["rc"].ToString());
                        if (rc1 != 0)
                        {
                            rc1 += 1;
                            if (rc1 > 0)
                            {
                                rc = 1;
                            }
                        }

                        cs1 = float.Parse(dt.Rows[i]["cs"].ToString());
                        if (cs1 != 0)
                        {
                            cs1 += 1;
                            if (cs1 > 0)
                            {
                                cs = 1;
                            }
                        }
                        gross1 = float.Parse(dt.Rows[i]["gross"].ToString());
                        if (gross1 != 0)
                        {
                            gross1 += 1;
                            if (gross1 > 0)
                            {
                                gross = 1;
                            }
                        }

                        incentivs1 = float.Parse(dt.Rows[i]["incentivs"].ToString());
                        if (incentivs1 != 0)
                        {
                            incentivs1 += 1;
                            if (incentivs1 > 0)
                            {
                                incentivs = 1;
                            }
                        }


                        woamt1 = float.Parse(dt.Rows[i]["WOAmt"].ToString());
                        if (woamt1 != 0)
                        {
                            woamt1 += 1;
                            if (woamt1 > 0)
                            {
                                woamt = 1;
                            }
                        }

                        npotsamt1 = float.Parse(dt.Rows[i]["Npotsamt"].ToString());
                        if (npotsamt1 != 0)
                        {
                            npotsamt1 += 1;
                            if (npotsamt1 > 0)
                            {
                                npotsamt = 1;
                            }
                        }
                        nhsamt1 = float.Parse(dt.Rows[i]["Nhsamt"].ToString());
                        if (nhsamt1 != 0)
                        {
                            nhsamt1 += 1;
                            if (nhsamt1 > 0)
                            {
                                nhsamt = 1;
                            }
                        }



                        //CHanged to zero so as to get PF and ESI in PFTotal and ESITotal as given below //Check PFTotal and ESITotal
                        pfonduties1 = float.Parse(dt.Rows[i]["PFonDuties"].ToString());
                        if (pfonduties1 != 0)
                        {
                            pfonduties1 += 1;
                            if (pfonduties1 > 0)
                            {
                                pfonduties = 0;
                            }
                        }
                        esionduties1 = float.Parse(dt.Rows[i]["ESIonduties"].ToString());
                        if (esionduties1 != 0)
                        {
                            esionduties1 += 1;
                            if (esionduties1 > 0)
                            {
                                esionduties = 0;
                            }
                        }
                        proftax1 = float.Parse(dt.Rows[i]["proftax"].ToString());
                        if (proftax1 != 0)
                        {
                            proftax1 += 1;
                            if (proftax1 > 0)
                            {
                                proftax = 1;
                            }
                        }
                        salAdvDed1 = float.Parse(dt.Rows[i]["salAdvDed"].ToString());
                        if (salAdvDed1 != 0)
                        {
                            salAdvDed1 += 1;
                            if (salAdvDed1 > 0)
                            {
                                salAdvDed = 1;
                            }
                        }
                        uniformDed1 = float.Parse(dt.Rows[i]["uniformDed"].ToString());
                        if (uniformDed1 != 0)
                        {
                            uniformDed1 += 1;
                            if (uniformDed1 > 0)
                            {
                                uniformDed = 1;
                            }
                        }

                        GenDedn1 = float.Parse(dt.Rows[i]["GenDedn"].ToString());
                        if (GenDedn1 != 0)
                        {
                            GenDedn1 += 1;
                            if (GenDedn1 > 0)
                            {
                                GenDedn = 1;
                            }
                        }

                        SecDepDedn1 = float.Parse(dt.Rows[i]["SecDepDedn"].ToString());
                        if (SecDepDedn1 != 0)
                        {
                            SecDepDedn1 += 1;
                            if (SecDepDedn1 > 0)
                            {
                                SecDepDedn = 1;
                            }
                        }

                        otherDed1 = float.Parse(dt.Rows[i]["otherDed"].ToString());
                        if (otherDed1 != 0)
                        {
                            otherDed1 += 1;
                            if (otherDed1 > 0)
                            {
                                otherDed = 1;
                            }
                        }
                        canteenAdv1 = float.Parse(dt.Rows[i]["canteenAdv"].ToString());
                        if (canteenAdv1 != 0)
                        {
                            canteenAdv1 += 1;
                            if (canteenAdv1 > 0)
                            {
                                canteenAdv = 1;
                            }
                        }

                        roomrent1 = float.Parse(dt.Rows[i]["penalty"].ToString());
                        if (roomrent1 != 0)
                        {
                            roomrent1 += 1;
                            if (roomrent1 > 0)
                            {
                                roomrent = 1;
                            }
                        }
                        totalDeductions1 = float.Parse(dt.Rows[i]["Totaldeduct"].ToString());
                        if (totalDeductions1 != 0)
                        {
                            totalDeductions1 += 1;
                            if (totalDeductions1 > 0)
                            {
                                totalDeductions = 1;
                            }
                        }
                        netpay1 = float.Parse(dt.Rows[i]["NetAmount"].ToString());
                        if (netpay1 != 0)
                        {
                            netpay1 += 1;
                            if (netpay1 > 0)
                            {
                                netpay = 1;
                            }
                        }

                        ots1 = float.Parse(dt.Rows[i]["Duties"].ToString());
                        if (ots1 != 0)
                        {
                            ots1 += 1;
                            if (ots1 > 0)
                            {
                                ots = 1;
                            }
                        }

                        wo1 = float.Parse(dt.Rows[i]["WO"].ToString());
                        if (wo1 != 0)
                        {
                            wo1 += 1;
                            if (wo1 > 0)
                            {
                                wo = 1;
                            }
                        }

                        npots1 = float.Parse(dt.Rows[i]["Npots"].ToString());
                        if (npots1 != 0)
                        {
                            npots1 += 1;
                            if (npots1 > 0)
                            {
                                npots = 1;
                            }
                        }

                        nhs1 = float.Parse(dt.Rows[i]["NHS"].ToString());
                        if (nhs1 != 0)
                        {
                            nhs1 += 1;
                            if (nhs1 > 0)
                            {
                                nhs = 1;
                            }
                        }

                        otamt1 = float.Parse(dt.Rows[i]["Amount"].ToString());
                        if (otamt1 != 0)
                        {
                            otamt1 += 1;
                            if (otamt1 > 0)
                            {
                                otamt = 1;
                            }
                        }

                        pfonot1 = float.Parse(dt.Rows[i]["PFONOT"].ToString());
                        if (pfonot1 != 0)
                        {
                            pfonot1 += 1;
                            if (pfonot1 > 0)
                            {
                                pfonot = 0;
                            }
                        }

                        esionot1 = float.Parse(dt.Rows[i]["ESIONOT"].ToString());
                        if (esionot1 != 0)
                        {
                            esionot1 += 1;
                            if (esionot1 > 0)
                            {
                                esionot = 0;
                            }
                        }

                        Pf1 = float.Parse(dt.Rows[i]["PFTotal"].ToString());
                        if (Pf1 != 0)
                        {
                            Pf1 += 1;
                            if (Pf1 > 0)
                            {
                                Pf = 1;
                            }
                        }
                        Esi1 = float.Parse(dt.Rows[i]["ESITotal"].ToString());
                        if (Esi1 != 0)
                        {
                            Esi1 += 1;
                            if (Esi1 > 0)
                            {
                                Esi = 1;
                            }
                        }

                        subtotal1 = float.Parse(dt.Rows[i]["subtotal"].ToString());
                        if (subtotal1 != 0)
                        {
                            subtotal1 += 1;
                            if (subtotal1 > 0)
                            {
                                subtotal = 1;
                            }
                        }

                    }


                    #endregion

                    int sno = 1;
                    int empid = 1;
                    int empname = 1;
                    int design = 1;
                    int bankacno = 1;

                    tableCells = dts + srate + basic + da + hra + cca + conveyance + washallowance + otherallowance + leavewages +
                                   gratuity + bonus + nfhs + rc + cs + gross + incentivs + pfonduties + esionduties + proftax + owf +
                                   salAdvDed + uniformDed + otherDed + canteenAdv + roomrent + totalDeductions + netpay + sno + subtotal +
                                   empid + empname + design + bankacno + ots + wo + npots + nhs + otamt + pfonot + esionot + Pf + Esi + GenDedn + SecDepDedn + woamt + npotsamt + nhsamt;


                    pageEvent.MyRow = dtclientid.Rows[k];

                    clientid = dtclientid.Rows[k]["clientid"].ToString();
                    clientname = dtclientid.Rows[k]["clientname"].ToString();
                    Month = dtclientid.Rows[k]["Monthname"].ToString();
                    if (k != 0)
                    {
                        pageEvent.clientid = clientid;
                        pageEvent.clientname = clientname;
                        pageEvent.month = Month;
                        document.NewPage();


                    }


                    #region Table Data

                    PdfPTable Secondtable = new PdfPTable(tableCells);
                    if (tableCells > 20)
                    {
                        Secondtable.TotalWidth = 1000f;
                    }
                    else
                    {
                        Secondtable.TotalWidth = 850f;
                    }
                    Secondtable.LockedWidth = true;
                    float[] SecondWidth = new float[] { };

                    #region Table Cells
                    if (tableCells == 37)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 5f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 36)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 5f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }

                    if (tableCells == 35)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 5f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }

                    if (tableCells == 34)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 5f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 33)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 5f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 32)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 5f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 31)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 30)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 29)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 28)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 27)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 26)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 25)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 24)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 23)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 22)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 21)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 20)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 19)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 18)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 17)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 16)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 15)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 14)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 13)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 12)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 11)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 10)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                    }
                    if (tableCells == 9)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 5f };
                    }
                    if (tableCells == 8)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 5f };
                    }

                    if (tableCells == 7)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 5f };
                    }

                    if (tableCells == 6)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 5f };
                    }
                    #endregion



                    Secondtable.SetWidths(SecondWidth);


                    for (int nextpagei = 0; nextpagei < dt.Rows.Count; ++nextpagei)
                    {

                        int i = nextpagei;
                        #region data from db
                        if (dt.Rows[nextpagei]["clientid"].ToString() == dtclientid.Rows[k]["clientid"].ToString() && dt.Rows[nextpagei]["monthname"].ToString() == dtclientid.Rows[k]["monthname"].ToString())
                        {

                            forConvert = 0;
                            if (dt.Rows[i]["NoOfDuties"].ToString().Trim().Length > 0)
                                forConvert = Convert.ToSingle(dt.Rows[i]["NoOfDuties"].ToString()) + Convert.ToSingle(dt.Rows[i]["Duties"].ToString()) + Convert.ToSingle(dt.Rows[i]["WO"].ToString());



                            //if (forConvert > 0)

                            strQry = "Select p.EmpEpfNo,e.EmpESINo from EMPESICodes AS e INNER JOIN EMPEPFCodes as p ON e.Empid = p.Empid AND e.Empid='" + dt.Rows[i]["EmpId"].ToString() + "'";
                            string pfNo = "";
                            string esiNo = "";
                            DataTable PfTable = config.ExecuteReaderWithQueryAsync(strQry).Result;
                            if (PfTable.Rows.Count > 0)
                            {
                                pfNo = PfTable.Rows[0]["EmpEpfNo"].ToString();
                                esiNo = PfTable.Rows[0]["EmpESINo"].ToString();
                            }

                            //1
                            PdfPCell CSNo = new PdfPCell(new Phrase(dt.Rows[i]["sno"].ToString() + "\n \n \n \n", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                            CSNo.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            CSNo.Border = 15;
                            Secondtable.AddCell(CSNo);


                            //2
                            PdfPCell CEmpId1 = new PdfPCell(new Phrase(dt.Rows[i]["EmpId"].ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                            CEmpId1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            CEmpId1.Border = 15;
                            Secondtable.AddCell(CEmpId1);

                            //3
                            PdfPCell CEmpName1 = new PdfPCell(new Phrase("Name :" + dt.Rows[i]["EmpName"].ToString() + "\nEPF No: " + pfNo + "\nESI No: " + esiNo + "\nUAN No: " + dt.Rows[i]["empuannumber"].ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                            CEmpName1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            CEmpName1.Border = 15;
                            Secondtable.AddCell(CEmpName1);

                            //4
                            PdfPCell CEmpDesgn = new PdfPCell(new Phrase(dt.Rows[i]["Design"].ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                            CEmpDesgn.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            CEmpDesgn.Border = 15;
                            Secondtable.AddCell(CEmpDesgn);

                            //5
                            if (dts != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["NoOfDuties"].ToString().Trim().Length > 0)
                                    forConvert = Convert.ToSingle(dt.Rows[i]["NoOfDuties"].ToString());
                                totalDuties += forConvert;
                                GrandtotalDuties += forConvert;

                                PdfPCell CNoOfDuties = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CNoOfDuties.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CNoOfDuties.Border = 15;
                                Secondtable.AddCell(CNoOfDuties);
                            }


                            if (wo != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["WO"].ToString().Trim().Length > 0)
                                    forConvert = Convert.ToSingle(dt.Rows[i]["WO"].ToString());
                                totalwo += forConvert;
                                Grandtotalwo += forConvert;

                                PdfPCell Cwo = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Cwo.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cwo.Border = 15;
                                Secondtable.AddCell(Cwo);
                            }

                            //6
                            if (ots != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Duties"].ToString().Trim().Length > 0)
                                    forConvert = Convert.ToSingle(dt.Rows[i]["Duties"].ToString());
                                totalots += forConvert;
                                Grandtotalots += forConvert;

                                PdfPCell COts = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                COts.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                COts.Border = 15;
                                Secondtable.AddCell(COts);
                            }


                            if (nhs != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["NHS"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["NHS"].ToString()));
                                totalnhs += forConvert;
                                Grandtotalnhs += forConvert;

                                PdfPCell Cnhs1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Cnhs1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cnhs1.Border = 15;
                                Secondtable.AddCell(Cnhs1);
                            }


                            if (npots != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Npots"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Npots"].ToString()));
                                totalnpots += forConvert;
                                Grandtotalnpots += forConvert;

                                PdfPCell Cnpots1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Cnpots1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cnpots1.Border = 15;
                                Secondtable.AddCell(Cnpots1);
                            }



                            //6A
                            if (srate != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Salrate"].ToString().Trim().Length > 0)
                                    forConvert = Convert.ToSingle(dt.Rows[i]["Salrate"].ToString());
                                totalsrate += forConvert;
                                GrandtotalSrate += forConvert;

                                PdfPCell CSalrate = new PdfPCell(new Phrase(forConvert.ToString("#"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CSalrate.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CSalrate.Border = 15;
                                Secondtable.AddCell(CSalrate);
                            }


                            //7
                            if (basic != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Basic"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Basic"].ToString()));
                                totalBasic += forConvert;
                                GrandtotalBasic += forConvert;
                                PdfPCell CBasic1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CBasic1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CBasic1.Border = 15;
                                Secondtable.AddCell(CBasic1);
                            }

                            //8
                            if (da != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["DA"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["DA"].ToString()));
                                totalDA += forConvert;
                                GrandtotalDA += forConvert;

                                PdfPCell CDa1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CDa1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CDa1.Border = 15;
                                Secondtable.AddCell(CDa1);
                            }

                            //9
                            if (hra != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["HRA"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["HRA"].ToString()));
                                totalHRA += forConvert;
                                GrandtotalHRA += forConvert;

                                PdfPCell CHRA1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CHRA1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CHRA1.Border = 15;
                                Secondtable.AddCell(CHRA1);
                            }

                            //10
                            if (cca != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["cca"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["cca"].ToString()));
                                totalCCA += forConvert;

                                GrandtotalCCA += forConvert;

                                PdfPCell Ccca = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Ccca.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Ccca.Border = 15;
                                Secondtable.AddCell(Ccca);
                            }

                            //11
                            if (conveyance != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Conveyance"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Conveyance"].ToString()));
                                totalConveyance += forConvert;
                                GrandtotalConveyance += forConvert;

                                PdfPCell CConveyance = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CConveyance.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CConveyance.Border = 15;
                                Secondtable.AddCell(CConveyance);
                            }


                            //12
                            if (washallowance != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["washallowance"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["washallowance"].ToString()));
                                totalWA += forConvert;
                                GrandtotalWA += forConvert;

                                PdfPCell CWa = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CWa.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CWa.Border = 15;
                                Secondtable.AddCell(CWa);
                            }

                            //13
                            if (otherallowance != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["OtherAllowance"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["OtherAllowance"].ToString()));
                                totalOA += forConvert;
                                GrandtotalOA += forConvert;
                                PdfPCell COA = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                COA.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                COA.Border = 15;
                                Secondtable.AddCell(COA);
                            }

                            //14
                            if (leavewages != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Leavewages"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Leavewages"].ToString()));
                                totalLw += forConvert;
                                GrandtotalLw += forConvert;
                                PdfPCell CLa1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CLa1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CLa1.Border = 15;
                                Secondtable.AddCell(CLa1);
                            }
                            //15
                            if (gratuity != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Gratuity"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Gratuity"].ToString()));
                                totalGratuity += forConvert;
                                GrandtotalGratuity += forConvert;
                                PdfPCell CGratuity1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CGratuity1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CGratuity1.Border = 15;
                                Secondtable.AddCell(CGratuity1);
                            }

                            //16
                            if (bonus != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["bonus"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["bonus"].ToString()));
                                totalbonus += forConvert;

                                Grandtotalbonus += forConvert;
                                PdfPCell Cbonus = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Cbonus.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cbonus.Border = 15;
                                Secondtable.AddCell(Cbonus);
                            }



                            //17
                            if (nfhs != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Nfhs"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Nfhs"].ToString()));
                                totalNfhs += forConvert;
                                GrandtotalNfhs += forConvert;
                                PdfPCell CNfhs1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CNfhs1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CNfhs1.Border = 15;
                                Secondtable.AddCell(CNfhs1);
                            }

                            //18
                            if (rc != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["RC"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["RC"].ToString()));
                                totalRc += forConvert;
                                GrandtotalRc += forConvert;
                                PdfPCell CRc1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CRc1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CRc1.Border = 15;
                                Secondtable.AddCell(CRc1);
                            }

                            //19
                            if (cs != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["cs"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["cs"].ToString()));
                                totalCs += forConvert;
                                GrandtotalCs += forConvert;
                                PdfPCell CCs1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CCs1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CCs1.Border = 15;
                                Secondtable.AddCell(CCs1);
                            }


                            //22
                            if (incentivs != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Incentivs"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Incentivs"].ToString()));
                                totalIncentivs += forConvert;
                                GrandtotalIncentivs += forConvert;
                                PdfPCell CIncentivs1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CIncentivs1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CIncentivs1.Border = 15;
                                Secondtable.AddCell(CIncentivs1);
                            }

                            if (woamt != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["WOAmt"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["WOAmt"].ToString()));
                                totalwoamt += forConvert;
                                Grandtotalwoamt += forConvert;

                                PdfPCell Cwoamt1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Cwoamt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cwoamt1.Border = 15;
                                Secondtable.AddCell(Cwoamt1);
                            }

                            if (nhsamt != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Nhsamt"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Nhsamt"].ToString()));
                                totalnhsamt += forConvert;
                                Grandtotalnhsamt += forConvert;

                                PdfPCell Cnhsamt1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Cnhsamt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cnhsamt1.Border = 15;
                                Secondtable.AddCell(Cnhsamt1);
                            }


                            if (npotsamt != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Npotsamt"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Npotsamt"].ToString()));
                                totalnpotsamt += forConvert;
                                Grandtotalnpotsamt += forConvert;

                                PdfPCell Cnpotsamt1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Cnpotsamt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cnpotsamt1.Border = 15;
                                Secondtable.AddCell(Cnpotsamt1);
                            }


                            //20
                            if (gross != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Gross"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Gross"].ToString()));
                                totalGross += forConvert;
                                GrandtotalGross += forConvert;

                                PdfPCell CGross1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CGross1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CGross1.Border = 15;
                                Secondtable.AddCell(CGross1);
                            }



                            //21
                            if (otamt != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Amount"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Amount"].ToString()));
                                totalotamt += forConvert;
                                Grandtotalotamt += forConvert;

                                PdfPCell CGross1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CGross1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CGross1.Border = 15;
                                Secondtable.AddCell(CGross1);
                            }

                            if (subtotal != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["subtotal"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["subtotal"].ToString()));


                                PdfPCell CGross1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CGross1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CGross1.Border = 15;
                                Secondtable.AddCell(CGross1);
                            }


                            //23
                            if (pfonduties != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Pfonduties"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Pfonduties"].ToString()));
                                totalPFondts += forConvert;
                                GrandtotalPFondts += forConvert;

                                PdfPCell CPF1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CPF1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CPF1.Border = 15;
                                Secondtable.AddCell(CPF1);
                            }

                            //24
                            if (esionduties != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Esionduties"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Esionduties"].ToString()));
                                totalESIondts += forConvert;

                                GrandtotalESIondts += forConvert;

                                PdfPCell CESI1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CESI1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CESI1.Border = 15;
                                Secondtable.AddCell(CESI1);
                            }

                            //25
                            if (pfonot != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["PFONOT"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["PFONOT"].ToString()));
                                totalpfonots += forConvert;
                                Grandtotalpfonots += forConvert;

                                PdfPCell CPFonots = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CPFonots.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CPFonots.Border = 15;
                                Secondtable.AddCell(CPFonots);
                            }

                            //26
                            if (esionot != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["ESIONOT"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["ESIONOT"].ToString()));
                                totalesionots += forConvert;

                                Grandtotalesionots += forConvert;

                                PdfPCell CESIonots = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CESIonots.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CESIonots.Border = 15;
                                Secondtable.AddCell(CESIonots);
                            }

                            //27
                            if (Pf != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["PFTotal"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["PFTotal"].ToString()));
                                totalpf += forConvert;
                                Grandtotalpf += forConvert;

                                PdfPCell CPFonots = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CPFonots.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CPFonots.Border = 15;
                                Secondtable.AddCell(CPFonots);
                            }

                            //28
                            if (Esi != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["ESITotal"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["ESITotal"].ToString()));
                                totalesi += forConvert;

                                Grandtotalesi += forConvert;

                                PdfPCell CESIonots = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CESIonots.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CESIonots.Border = 15;
                                Secondtable.AddCell(CESIonots);
                            }

                            //29
                            if (proftax != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["ProfTax"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["ProfTax"].ToString()));
                                totalProfTax += forConvert;
                                GrandtotalProfTax += forConvert;

                                PdfPCell CProTax1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CProTax1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CProTax1.Border = 15;
                                Secondtable.AddCell(CProTax1);
                            }


                            //29
                            if (owf != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["owf"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["owf"].ToString()));
                                totalowfamt += forConvert;
                                Grandtotalowfamt += forConvert;

                                PdfPCell Cowfamt1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Cowfamt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cowfamt1.Border = 15;
                                Secondtable.AddCell(Cowfamt1);
                            }

                            //30
                            if (salAdvDed != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["SalAdvDed"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["SalAdvDed"].ToString()));
                                totalSalAdv += forConvert;
                                GrandtotalSalAdv += forConvert;

                                PdfPCell CSalAdv1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CSalAdv1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CSalAdv1.Border = 15;
                                Secondtable.AddCell(CSalAdv1);
                            }

                            //31
                            if (uniformDed != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["UniformDed"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["UniformDed"].ToString()));
                                totalUniforDed += forConvert;
                                GrandtotalUniforDed += forConvert;

                                PdfPCell CUnifDed1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CUnifDed1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CUnifDed1.Border = 15;
                                Secondtable.AddCell(CUnifDed1);
                            }

                            //32
                            if (otherDed != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["OtherDed"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["OtherDed"].ToString()));
                                totalOtherdedn += forConvert;
                                GrandtotalOtherdedn += forConvert;

                                PdfPCell COtherDed1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                COtherDed1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                COtherDed1.Border = 15;
                                Secondtable.AddCell(COtherDed1);
                            }

                            //33
                            if (canteenAdv != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["CanteenAdv"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["CanteenAdv"].ToString()));
                                totalCanteenAdv += forConvert;
                                GrandtotalCanteenAdv += forConvert;

                                PdfPCell CCanteended = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CCanteended.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CCanteended.Border = 15;
                                Secondtable.AddCell(CCanteended);
                            }

                            //34
                            if (roomrent != 0)
                            {
                                if (dt.Rows[i]["penalty"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["penalty"].ToString()));


                                PdfPCell CPenalty1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CPenalty1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CPenalty1.Border = 15;
                                Secondtable.AddCell(CPenalty1);
                            }



                            if (SecDepDedn != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["SecDepDedn"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["SecDepDedn"].ToString()));
                                totalSecDepDedn += forConvert;
                                GrandtotalSecDepDedn += forConvert;

                                PdfPCell CSecDepDedn1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CSecDepDedn1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CSecDepDedn1.Border = 15;
                                Secondtable.AddCell(CSecDepDedn1);
                            }


                            if (GenDedn != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["GenDedn"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["GenDedn"].ToString()));
                                totalGenDedn += forConvert;
                                GrandtotalGenDedn += forConvert;

                                PdfPCell CGenDedn1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CGenDedn1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CGenDedn1.Border = 15;
                                Secondtable.AddCell(CGenDedn1);
                            }




                            //35
                            if (totalDeductions != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Totaldeduct"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Totaldeduct"].ToString()));
                                totalDed += forConvert;
                                GrandtotalDed += forConvert;

                                PdfPCell CTotDed1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CTotDed1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CTotDed1.Border = 15;
                                Secondtable.AddCell(CTotDed1);//OtherDed,Eps.Gross,Eps.Deductions,Eps.ActualAmount
                            }

                            //36
                            if (netpay != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["NetAmount"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["NetAmount"].ToString()));

                                PdfPCell CNetPay1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CNetPay1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CNetPay1.Border = 15;
                                Secondtable.AddCell(CNetPay1);
                            }

                            //37
                            string EmpBankAcNo = dt.Rows[i]["EmpBankAcNo"].ToString();
                            PdfPCell CSignature1 = new PdfPCell(new Phrase(EmpBankAcNo, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                            CSignature1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            CSignature1.Border = 15;
                            CSignature1.MinimumHeight = 25;
                            Secondtable.AddCell(CSignature1);

                        }
                        #endregion

                    }
                    document.Add(Secondtable);
                    #endregion



                }




                document.Close();
                if (nextpagehasPages)
                {

                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Dutieswithots.pdf");
                    Response.Buffer = true;
                    Response.Clear();
                    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    Response.OutputStream.Flush();
                    Response.End();
                }
            }


        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Cells[34].Attributes.Add("class", "text");
            }
        }



        public class PageEventHelper : PdfPageEventHelper
        {
            public DataRow MyRow { get; set; }
            public DataTable dt { get; set; }
            public string list { get; set; }
            public string CmpIDPrefix { get; set; }
            public Document document { get; set; }
            public string month { get; set; }
            public string clientid { get; set; }
            public string clientname { get; set; }





            string Fontstyle = "Verdana";
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                AppConfiguration config = new AppConfiguration();

                document.SetMargins(document.LeftMargin, document.LeftMargin, document.TopMargin, document.BottomMargin); //Mirror the horizontal margins

                string selectclientaddress = "select * from clients where clientid  in (" + list + ")";
                DataTable dtclientaddress = config.ExecuteReaderWithQueryAsync(selectclientaddress).Result;

                string strQry = "Select * from CompanyInfo   where   ClientidPrefix='" + CmpIDPrefix + "'";
                DataTable compInfo = config.ExecuteReaderWithQueryAsync(strQry).Result;
                string companyName1 = "Your Company Name";
                string companyAddress = "Your Company Address";
                if (compInfo.Rows.Count > 0)
                {
                    companyName1 = compInfo.Rows[0]["CompanyName"].ToString();
                    companyAddress = compInfo.Rows[0]["Address"].ToString();
                }




                #region Variables for table cells counting

                int dts = 0;
                int srate = 0;
                int basic = 0;
                int da = 0;
                int hra = 0;
                int cca = 0;
                int conveyance = 0;
                int washallowance = 0;
                int otherallowance = 0;
                int leavewages = 0;
                int gratuity = 0;
                int bonus = 0;
                int nfhs = 0;
                int rc = 0;
                int cs = 0;
                int gross = 0;
                int incentivs = 0;
                int pfonduties = 0;
                int esionduties = 0;
                int proftax = 0;
                int owf = 0;
                int salAdvDed = 0;
                int uniformDed = 0;
                int otherDed = 0;
                int canteenAdv = 0;
                int roomrent = 0;
                int totalDeductions = 0;
                int netpay = 0;
                int subtotal = 0;
                int ots = 0;
                int wo = 0;
                int npots = 0;
                int nhs = 0;
                int woamt = 0;
                int npotsamt = 0;
                int nhsamt = 0;
                int otamt = 0;
                int pfonot = 0;
                int esionot = 0;
                int Pf = 0;
                int Esi = 0;
                int GenDedn = 0;
                int SecDepDedn = 0;
                float owf1 = 0;

                float dts1 = 0;
                float srate1 = 0;
                float basic1 = 0;
                float da1 = 0;
                float hra1 = 0;
                float cca1 = 0;
                float conveyance1 = 0;
                float washallowance1 = 0;
                float otherallowance1 = 0;
                float leavewages1 = 0;
                float gratuity1 = 0;
                float bonus1 = 0;
                float nfhs1 = 0;
                float rc1 = 0;
                float cs1 = 0;
                float gross1 = 0;
                float incentivs1 = 0;
                float pfonduties1 = 0;
                float esionduties1 = 0;
                float proftax1 = 0;
                float salAdvDed1 = 0;
                float uniformDed1 = 0;
                float otherDed1 = 0;
                float canteenAdv1 = 0;
                float roomrent1 = 0;
                float totalDeductions1 = 0;
                float netpay1 = 0;
                float subtotal1 = 0;
                float ots1 = 0;
                float wo1 = 0;
                float npots1 = 0;
                float nhs1 = 0;
                float woamt1 = 0;
                float npotsamt1 = 0;
                float nhsamt1 = 0;
                float otamt1 = 0;
                float pfonot1 = 0;
                float esionot1 = 0;
                float Pf1 = 0;
                float Esi1 = 0;
                float GenDedn1 = 0;
                float SecDepDedn1 = 0;


                #endregion


                #region Data for counting tablecells

                for (int i = 0; i < dt.Rows.Count; i++)
                {


                    dts1 = float.Parse(dt.Rows[i]["NoOfDuties"].ToString());
                    if (dts1 != 0)
                    {
                        dts1 += 1;
                        if (dts1 > 0)
                        {
                            dts = 1;
                        }
                    }


                    owf1 = float.Parse(dt.Rows[i]["owf"].ToString());
                    if (owf1 != 0)
                    {
                        owf1 += 1;
                        if (owf1 > 0)
                        {
                            owf = 1;
                        }
                    }

                    srate1 = float.Parse(dt.Rows[i]["SalRate"].ToString());
                    if (srate1 != 0)
                    {
                        srate1 += 1;
                        if (srate1 > 0)
                        {
                            srate = 1;
                        }
                    }

                    basic1 = float.Parse(dt.Rows[i]["basic"].ToString());
                    if (basic1 != 0)
                    {
                        basic1 += 1;
                        if (basic1 > 0)
                        {
                            basic = 1;
                        }
                    }
                    da1 = float.Parse(dt.Rows[i]["da"].ToString());
                    if (da1 != 0)
                    {
                        da1 += 1;
                        if (da1 > 0)
                        {
                            da = 1;
                        }
                    }
                    hra1 = float.Parse(dt.Rows[i]["hra"].ToString());
                    if (hra1 != 0)
                    {
                        hra1 += 1;
                        if (hra1 > 0)
                        {
                            hra = 1;
                        }
                    }
                    cca1 = float.Parse(dt.Rows[i]["cca"].ToString());
                    if (cca1 != 0)
                    {
                        cca1 += 1;
                        if (cca1 > 0)
                        {
                            cca = 1;
                        }
                    }
                    conveyance1 = float.Parse(dt.Rows[i]["Conveyance"].ToString());
                    if (conveyance1 != 0)
                    {
                        conveyance1 += 1;
                        if (conveyance1 > 0)
                        {
                            conveyance = 1;
                        }
                    }

                    washallowance1 = float.Parse(dt.Rows[i]["washallowance"].ToString());
                    if (washallowance1 != 0)
                    {
                        washallowance1 += 1;
                        if (washallowance1 > 0)
                        {
                            washallowance = 1;
                        }
                    }
                    otherallowance1 = float.Parse(dt.Rows[i]["otherallowance"].ToString());
                    if (otherallowance1 != 0)
                    {
                        otherallowance1 += 1;
                        if (otherallowance1 > 0)
                        {
                            otherallowance = 1;
                        }
                    }
                    leavewages1 = float.Parse(dt.Rows[i]["Leavewages"].ToString());
                    if (leavewages1 != 0)
                    {
                        leavewages1 += 1;
                        if (leavewages1 > 0)
                        {
                            leavewages = 1;
                        }
                    }
                    gratuity1 = float.Parse(dt.Rows[i]["gratuity"].ToString());
                    if (gratuity1 != 0)
                    {
                        gratuity1 += 1;
                        if (gratuity1 > 0)
                        {
                            gratuity = 1;
                        }
                    }
                    bonus1 = float.Parse(dt.Rows[i]["bonus"].ToString());
                    if (bonus1 != 0)
                    {
                        bonus1 += 1;
                        if (bonus1 > 0)
                        {
                            bonus = 1;
                        }
                    }

                    nfhs1 = float.Parse(dt.Rows[i]["nfhs"].ToString());
                    if (nfhs1 != 0)
                    {
                        nfhs1 += 1;
                        if (nfhs1 > 0)
                        {
                            nfhs = 1;
                        }
                    }
                    rc1 = float.Parse(dt.Rows[i]["rc"].ToString());
                    if (rc1 != 0)
                    {
                        rc1 += 1;
                        if (rc1 > 0)
                        {
                            rc = 1;
                        }
                    }

                    cs1 = float.Parse(dt.Rows[i]["cs"].ToString());
                    if (cs1 != 0)
                    {
                        cs1 += 1;
                        if (cs1 > 0)
                        {
                            cs = 1;
                        }
                    }
                    gross1 = float.Parse(dt.Rows[i]["gross"].ToString());
                    if (gross1 != 0)
                    {
                        gross1 += 1;
                        if (gross1 > 0)
                        {
                            gross = 1;
                        }
                    }

                    incentivs1 = float.Parse(dt.Rows[i]["incentivs"].ToString());
                    if (incentivs1 != 0)
                    {
                        incentivs1 += 1;
                        if (incentivs1 > 0)
                        {
                            incentivs = 1;
                        }
                    }


                    woamt1 = float.Parse(dt.Rows[i]["WOAmt"].ToString());
                    if (woamt1 != 0)
                    {
                        woamt1 += 1;
                        if (woamt1 > 0)
                        {
                            woamt = 1;
                        }
                    }

                    npotsamt1 = float.Parse(dt.Rows[i]["Npotsamt"].ToString());
                    if (npotsamt1 != 0)
                    {
                        npotsamt1 += 1;
                        if (npotsamt1 > 0)
                        {
                            npotsamt = 1;
                        }
                    }
                    nhsamt1 = float.Parse(dt.Rows[i]["Nhsamt"].ToString());
                    if (nhsamt1 != 0)
                    {
                        nhsamt1 += 1;
                        if (nhsamt1 > 0)
                        {
                            nhsamt = 1;
                        }
                    }



                    //CHanged to zero so as to get PF and ESI in PFTotal and ESITotal as given below //Check PFTotal and ESITotal
                    pfonduties1 = float.Parse(dt.Rows[i]["PFonDuties"].ToString());
                    if (pfonduties1 != 0)
                    {
                        pfonduties1 += 1;
                        if (pfonduties1 > 0)
                        {
                            pfonduties = 0;
                        }
                    }
                    esionduties1 = float.Parse(dt.Rows[i]["ESIonduties"].ToString());
                    if (esionduties1 != 0)
                    {
                        esionduties1 += 1;
                        if (esionduties1 > 0)
                        {
                            esionduties = 0;
                        }
                    }
                    proftax1 = float.Parse(dt.Rows[i]["proftax"].ToString());
                    if (proftax1 != 0)
                    {
                        proftax1 += 1;
                        if (proftax1 > 0)
                        {
                            proftax = 1;
                        }
                    }
                    salAdvDed1 = float.Parse(dt.Rows[i]["salAdvDed"].ToString());
                    if (salAdvDed1 != 0)
                    {
                        salAdvDed1 += 1;
                        if (salAdvDed1 > 0)
                        {
                            salAdvDed = 1;
                        }
                    }
                    uniformDed1 = float.Parse(dt.Rows[i]["uniformDed"].ToString());
                    if (uniformDed1 != 0)
                    {
                        uniformDed1 += 1;
                        if (uniformDed1 > 0)
                        {
                            uniformDed = 1;
                        }
                    }

                    GenDedn1 = float.Parse(dt.Rows[i]["GenDedn"].ToString());
                    if (GenDedn1 != 0)
                    {
                        GenDedn1 += 1;
                        if (GenDedn1 > 0)
                        {
                            GenDedn = 1;
                        }
                    }

                    SecDepDedn1 = float.Parse(dt.Rows[i]["SecDepDedn"].ToString());
                    if (SecDepDedn1 != 0)
                    {
                        SecDepDedn1 += 1;
                        if (SecDepDedn1 > 0)
                        {
                            SecDepDedn = 1;
                        }
                    }

                    otherDed1 = float.Parse(dt.Rows[i]["otherDed"].ToString());
                    if (otherDed1 != 0)
                    {
                        otherDed1 += 1;
                        if (otherDed1 > 0)
                        {
                            otherDed = 1;
                        }
                    }
                    canteenAdv1 = float.Parse(dt.Rows[i]["canteenAdv"].ToString());
                    if (canteenAdv1 != 0)
                    {
                        canteenAdv1 += 1;
                        if (canteenAdv1 > 0)
                        {
                            canteenAdv = 1;
                        }
                    }

                    roomrent1 = float.Parse(dt.Rows[i]["penalty"].ToString());
                    if (roomrent1 != 0)
                    {
                        roomrent1 += 1;
                        if (roomrent1 > 0)
                        {
                            roomrent = 1;
                        }
                    }
                    totalDeductions1 = float.Parse(dt.Rows[i]["Totaldeduct"].ToString());
                    if (totalDeductions1 != 0)
                    {
                        totalDeductions1 += 1;
                        if (totalDeductions1 > 0)
                        {
                            totalDeductions = 1;
                        }
                    }
                    netpay1 = float.Parse(dt.Rows[i]["NetAmount"].ToString());
                    if (netpay1 != 0)
                    {
                        netpay1 += 1;
                        if (netpay1 > 0)
                        {
                            netpay = 1;
                        }
                    }

                    subtotal1 = float.Parse(dt.Rows[i]["subtotal"].ToString());
                    if (subtotal1 != 0)
                    {
                        subtotal1 += 1;
                        if (subtotal1 > 0)
                        {
                            subtotal = 1;
                        }
                    }

                    ots1 = float.Parse(dt.Rows[i]["Duties"].ToString());
                    if (ots1 != 0)
                    {
                        ots1 += 1;
                        if (ots1 > 0)
                        {
                            ots = 1;
                        }
                    }

                    wo1 = float.Parse(dt.Rows[i]["WO"].ToString());
                    if (wo1 != 0)
                    {
                        wo1 += 1;
                        if (wo1 > 0)
                        {
                            wo = 1;
                        }
                    }

                    npots1 = float.Parse(dt.Rows[i]["Npots"].ToString());
                    if (npots1 != 0)
                    {
                        npots1 += 1;
                        if (npots1 > 0)
                        {
                            npots = 1;
                        }
                    }

                    nhs1 = float.Parse(dt.Rows[i]["NHS"].ToString());
                    if (nhs1 != 0)
                    {
                        nhs1 += 1;
                        if (nhs1 > 0)
                        {
                            nhs = 1;
                        }
                    }

                    otamt1 = float.Parse(dt.Rows[i]["Amount"].ToString());
                    if (otamt1 != 0)
                    {
                        otamt1 += 1;
                        if (otamt1 > 0)
                        {
                            otamt = 1;
                        }
                    }

                    pfonot1 = float.Parse(dt.Rows[i]["PFONOT"].ToString());
                    if (pfonot1 != 0)
                    {
                        pfonot1 += 1;
                        if (pfonot1 > 0)
                        {
                            pfonot = 0;
                        }
                    }

                    esionot1 = float.Parse(dt.Rows[i]["ESIONOT"].ToString());
                    if (esionot1 != 0)
                    {
                        esionot1 += 1;
                        if (esionot1 > 0)
                        {
                            esionot = 0;
                        }
                    }

                    Pf1 = float.Parse(dt.Rows[i]["PFTotal"].ToString());
                    if (Pf1 != 0)
                    {
                        Pf1 += 1;
                        if (Pf1 > 0)
                        {
                            Pf = 1;
                        }
                    }
                    Esi1 = float.Parse(dt.Rows[i]["ESITotal"].ToString());
                    if (Esi1 != 0)
                    {
                        Esi1 += 1;
                        if (Esi1 > 0)
                        {
                            Esi = 1;
                        }
                    }


                }

                #endregion

                int sno = 1;
                int empid = 1;
                int empname = 1;
                int design = 1;
                int bankacno = 1;



                int tableCells = dts + srate + basic + da + hra + cca + conveyance + washallowance + otherallowance + leavewages +
                             gratuity + bonus + nfhs + rc + cs + gross + incentivs + pfonduties + esionduties + proftax + owf +
                             salAdvDed + uniformDed + otherDed + canteenAdv + roomrent + totalDeductions + netpay + sno + subtotal +
                             empid + empname + design + bankacno + ots + wo + npots + nhs + otamt + pfonot + esionot + Pf + Esi + GenDedn + SecDepDedn + woamt + npotsamt + nhsamt;


                #region Titles of Document
                PdfPTable Maintable = new PdfPTable(30);

                if (tableCells > 20)
                {
                    Maintable.TotalWidth = 1000f;
                }
                else
                {
                    Maintable.TotalWidth = 850f;
                }
                Maintable.LockedWidth = true;
                float[] width = new float[] { 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f };


                Maintable.SetWidths(width);
                uint FONT_SIZE = 8;
                string FontStyle = "verdana";

                #region Company Name & wage act details

                PdfPCell cellemp = new PdfPCell(new Phrase("  ", FontFactory.GetFont(Fontstyle, 10, Font.BOLD, BaseColor.BLACK)));
                cellemp.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellemp.Colspan = 30;
                cellemp.Border = 0;


                PdfPCell Heading = new PdfPCell(new Phrase("[ See Rule 24(9-B) of the Karnataka Shops & Commercial Establishments Rules, 1963] in lieu of \n 1. Form I, II of Rule 22(4): Form IV of Rule 28(2); Form V & VII of Rule 29(1) & (5) of Karnataka Minimum wages Rules 1958; \n 2. Form I of Rules 3 (1) of Karnataka Payment of Wages Rules, 1963; \n 3. Form XIII of Rules 75; Form XV, XVII, XX, XXI, XXII, XXIII, of Rule 78 (1) a(i), (ii) &(iii) of the Karnataka Contract Labour (Regulation & Abolition) Rules, 1974; \n 4. Form XIII of Rule 43, Form XVII, XVIII, XIX, XX, XXI, XXII, of Rule 46(2) (a), (c) & (d) of Karnataka inter state Migrant Workmen Rules, 1981  ", FontFactory.GetFont(Fontstyle, 7, Font.NORMAL, BaseColor.BLACK)));
                Heading.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                Heading.Colspan = 10;
                Heading.Border = 0;
                Heading.SetLeading(0.0f, 1.5f);
                Maintable.AddCell(Heading);

                PdfPCell cellemp1 = new PdfPCell(new Phrase("  ", FontFactory.GetFont(Fontstyle, 10, Font.BOLD, BaseColor.BLACK)));
                cellemp1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellemp1.Colspan = 1;
                cellemp1.Border = 0;
                //Maintable.AddCell(cellemp1);




                PdfPCell cellemp2 = new PdfPCell(new Phrase("Form  XVII \n Register Of Wages \n Wages Period Monthly \n  For the Month : " + month, FontFactory.GetFont(Fontstyle, 7, Font.NORMAL, BaseColor.BLACK)));
                cellemp2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellemp2.Colspan = 8;
                cellemp2.Border = 0;
                Maintable.AddCell(cellemp2);

                PdfPCell Heading1 = new PdfPCell(new Phrase("Name & Address of the Establishment \n Under Which Contract in Carried on : ", FontFactory.GetFont(Fontstyle, 7, Font.BOLD, BaseColor.BLACK)));
                Heading1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                Heading1.Colspan = 6;
                Heading1.Border = 0;
                Heading1.SetLeading(0.0f, 1.5f);
                Maintable.AddCell(Heading1);

                PdfPCell Heading21 = new PdfPCell(new Phrase(companyName1 + "\n" + companyAddress, FontFactory.GetFont(Fontstyle, 7, Font.NORMAL, BaseColor.BLACK)));
                Heading21.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                Heading21.Colspan = 6;
                Heading21.Border = 0;
                Heading21.PaddingLeft = -30;
                Heading21.SetLeading(0.0f, 1.5f);
                Maintable.AddCell(Heading21);

                Maintable.AddCell(cellemp1);


                Maintable.AddCell(cellemp);

                PdfPCell CClient = new PdfPCell(new Phrase("Client ID :   " + clientid, FontFactory.GetFont(Fontstyle, 10, Font.NORMAL, BaseColor.BLACK)));
                CClient.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                CClient.Colspan = 15;
                CClient.Border = 0;// 15;
                CClient.PaddingTop = -10;
                Maintable.AddCell(CClient);

                PdfPCell CClientName = new PdfPCell();
                var caclientname = new Phrase();
                caclientname.Add(new Chunk("Name and Address of Principal Employer  ", FontFactory.GetFont(Fontstyle, 8, Font.BOLD, BaseColor.BLACK)));
                caclientname.Add(new Chunk(clientname, FontFactory.GetFont(Fontstyle, 10, Font.NORMAL, BaseColor.BLACK)));
                CClientName.AddElement(caclientname);
                CClientName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                CClientName.Colspan = 15;
                CClientName.PaddingTop = -18;
                // CClientName.PaddingLeft = 250;
                CClientName.Border = 0;
                Maintable.AddCell(CClientName);


                PdfPCell Cplaceofpay = new PdfPCell(new Phrase("Place of Payment", FontFactory.GetFont(Fontstyle, 8, Font.BOLD, BaseColor.BLACK)));
                Cplaceofpay.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                Cplaceofpay.Colspan = 15;
                Cplaceofpay.Border = 0;// 15;
                Maintable.AddCell(Cplaceofpay);





                document.Add(Maintable);

                #endregion

                #region Table Headings

                PdfPTable SecondtableHeadings = new PdfPTable(tableCells);

                if (tableCells > 20)
                {
                    SecondtableHeadings.TotalWidth = 1000f;
                }
                else
                {
                    SecondtableHeadings.TotalWidth = 850f;
                }
                SecondtableHeadings.LockedWidth = true;
                float[] SecondHeadingsWidth = new float[] { };

                #region Table Cells
                if (tableCells == 37)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 5f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 36)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 5f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }

                if (tableCells == 35)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 5f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }

                if (tableCells == 34)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 5f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 33)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 5f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 32)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 5f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 31)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 30)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 29)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 28)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 27)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 26)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 25)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 24)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 23)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 22)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 21)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 20)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 19)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 18)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 17)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 16)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 15)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 14)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 13)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 12)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 11)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 10)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 5f };
                }
                if (tableCells == 9)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 1.5f, 5f };
                }
                if (tableCells == 8)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 2f, 5f };
                }

                if (tableCells == 7)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 1.5f, 5f };
                }

                if (tableCells == 6)
                {
                    SecondHeadingsWidth = new float[] { 1.5f, 2f, 6f, 3f, 1.5f, 5f };
                }
                #endregion

                SecondtableHeadings.SetWidths(SecondHeadingsWidth);

                //Cell Headings
                //1
                PdfPCell sNo = new PdfPCell(new Phrase("S.No", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                sNo.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                //sNo.Colspan = 1;
                sNo.Border = 15;// 15;
                SecondtableHeadings.AddCell(sNo);

                //2
                PdfPCell CEmpId = new PdfPCell(new Phrase("Emp Id", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                CEmpId.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CEmpId.Border = 15;// 15;
                SecondtableHeadings.AddCell(CEmpId);

                //3
                PdfPCell CEmpName = new PdfPCell(new Phrase("Emp Name", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                CEmpName.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CEmpName.Border = 15;// 15;
                SecondtableHeadings.AddCell(CEmpName);

                //4
                PdfPCell CDesgn = new PdfPCell(new Phrase("Desgn", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                CDesgn.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CDesgn.Border = 15;
                SecondtableHeadings.AddCell(CDesgn);

                //5
                if (dts != 0)
                {
                    PdfPCell CDuties = new PdfPCell(new Phrase("Dts", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CDuties.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CDuties.Border = 15;
                    SecondtableHeadings.AddCell(CDuties);
                }
                //6
                if (wo != 0)
                {
                    PdfPCell Cwo = new PdfPCell(new Phrase("WO", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    Cwo.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Cwo.Border = 15;
                    SecondtableHeadings.AddCell(Cwo);
                }


                //7
                if (ots != 0)
                {
                    PdfPCell Cots = new PdfPCell(new Phrase("ED", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    Cots.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Cots.Border = 15;
                    SecondtableHeadings.AddCell(Cots);
                }


                //8
                if (nhs != 0)
                {
                    PdfPCell Cnpots = new PdfPCell(new Phrase("NHS", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    Cnpots.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Cnpots.Border = 15;
                    SecondtableHeadings.AddCell(Cnpots);
                }
                //9
                if (npots != 0)
                {
                    PdfPCell Cnhs = new PdfPCell(new Phrase("Npots", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    Cnhs.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Cnhs.Border = 15;
                    SecondtableHeadings.AddCell(Cnhs);
                }

                //10
                if (srate != 0)
                {
                    PdfPCell Cots = new PdfPCell(new Phrase("S. Rate", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    Cots.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Cots.Border = 15;
                    SecondtableHeadings.AddCell(Cots);
                }


                //11
                if (basic != 0)
                {
                    PdfPCell CBasic = new PdfPCell(new Phrase("Basic", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CBasic.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CBasic.Border = 15;
                    SecondtableHeadings.AddCell(CBasic);
                }

                //12
                if (da != 0)
                {
                    PdfPCell CDa = new PdfPCell(new Phrase("DA", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CDa.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CDa.Border = 15;
                    SecondtableHeadings.AddCell(CDa);
                }

                //13
                if (hra != 0)
                {
                    PdfPCell CHRa = new PdfPCell(new Phrase("HRA", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CHRa.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CHRa.Border = 15;
                    SecondtableHeadings.AddCell(CHRa);
                }

                //14
                if (cca != 0)
                {
                    PdfPCell CCca = new PdfPCell(new Phrase("CCA", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CCca.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CCca.Border = 15;
                    SecondtableHeadings.AddCell(CCca);
                }

                //15
                if (conveyance != 0)
                {
                    PdfPCell Cconveyance = new PdfPCell(new Phrase("Conv", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    Cconveyance.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Cconveyance.Border = 15;
                    SecondtableHeadings.AddCell(Cconveyance);
                }

                //16
                if (washallowance != 0)
                {
                    PdfPCell Cwa = new PdfPCell(new Phrase("WA", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    Cwa.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    Cwa.Border = 15;
                    SecondtableHeadings.AddCell(Cwa);
                }

                //17
                if (otherallowance != 0)
                {
                    PdfPCell COa = new PdfPCell(new Phrase("OA", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    COa.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    COa.Border = 15;
                    SecondtableHeadings.AddCell(COa);
                }

                //18
                if (leavewages != 0)
                {
                    PdfPCell CLa = new PdfPCell(new Phrase("EL", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CLa.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CLa.Border = 15;
                    SecondtableHeadings.AddCell(CLa);
                }

                //19
                if (gratuity != 0)
                {
                    PdfPCell CGratuity = new PdfPCell(new Phrase("Gratuity", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CGratuity.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CGratuity.Border = 15;
                    SecondtableHeadings.AddCell(CGratuity);
                }

                //20
                if (bonus != 0)
                {
                    PdfPCell CBonus = new PdfPCell(new Phrase("Bonus", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CBonus.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CBonus.Border = 15;
                    SecondtableHeadings.AddCell(CBonus);
                }




                //21
                if (nfhs != 0)
                {
                    PdfPCell CNfhs = new PdfPCell(new Phrase("NFHs", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CNfhs.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CNfhs.Border = 15;
                    SecondtableHeadings.AddCell(CNfhs);
                }

                //22
                if (rc != 0)
                {
                    PdfPCell CRc = new PdfPCell(new Phrase("R.C", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CRc.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CRc.Border = 15;
                    SecondtableHeadings.AddCell(CRc);
                }

                //23
                if (cs != 0)
                {
                    PdfPCell CCs = new PdfPCell(new Phrase("C.S", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CCs.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CCs.Border = 15;
                    SecondtableHeadings.AddCell(CCs);
                }

                //24
                if (incentivs != 0)
                {
                    PdfPCell CIncentivs = new PdfPCell(new Phrase("Incentivs", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CIncentivs.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CIncentivs.Border = 15;
                    SecondtableHeadings.AddCell(CIncentivs);
                }

                //25
                if (woamt != 0)
                {
                    PdfPCell Cwoamt = new PdfPCell(new Phrase("WO Amt", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    Cwoamt.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Cwoamt.Border = 15;
                    SecondtableHeadings.AddCell(Cwoamt);
                }
                //26
                if (nhsamt != 0)
                {
                    PdfPCell Cnpots = new PdfPCell(new Phrase("NHS Amt", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    Cnpots.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Cnpots.Border = 15;
                    SecondtableHeadings.AddCell(Cnpots);
                }
                //27
                if (npotsamt != 0)
                {
                    PdfPCell Cnhs = new PdfPCell(new Phrase("Npots Amt", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    Cnhs.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Cnhs.Border = 15;
                    SecondtableHeadings.AddCell(Cnhs);
                }


                //28
                if (gross != 0)
                {
                    PdfPCell CGross = new PdfPCell(new Phrase("Gross", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CGross.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CGross.Border = 15;
                    SecondtableHeadings.AddCell(CGross);
                }




                //29
                if (otamt != 0)
                {
                    PdfPCell COtamt = new PdfPCell(new Phrase("ED Amt", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    COtamt.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    COtamt.Border = 15;
                    SecondtableHeadings.AddCell(COtamt);
                }


                //30
                if (subtotal != 0)
                {
                    PdfPCell Csubtotal = new PdfPCell(new Phrase("Total Ernd", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    Csubtotal.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Csubtotal.Border = 15;
                    SecondtableHeadings.AddCell(Csubtotal);
                }



                //31
                if (pfonduties != 0)
                {
                    PdfPCell CPFondts = new PdfPCell(new Phrase("PF on Dts", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CPFondts.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CPFondts.Border = 15;
                    SecondtableHeadings.AddCell(CPFondts);
                }

                //32
                if (esionduties != 0)
                {
                    PdfPCell CESIondts = new PdfPCell(new Phrase("ESI on Dts", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CESIondts.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CESIondts.Border = 15;
                    SecondtableHeadings.AddCell(CESIondts);
                }

                //33
                if (pfonot != 0)
                {
                    PdfPCell CPFonot = new PdfPCell(new Phrase("PF on OTs", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CPFonot.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CPFonot.Border = 15;
                    SecondtableHeadings.AddCell(CPFonot);
                }

                //34
                if (esionot != 0)
                {
                    PdfPCell CESIonot = new PdfPCell(new Phrase("ESI on OTs", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CESIonot.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CESIonot.Border = 15;
                    SecondtableHeadings.AddCell(CESIonot);
                }

                //35
                if (Pf != 0)
                {
                    PdfPCell CPF = new PdfPCell(new Phrase("PF", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CPF.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CPF.Border = 15;
                    SecondtableHeadings.AddCell(CPF);
                }

                //36
                if (Esi != 0)
                {
                    PdfPCell CESI = new PdfPCell(new Phrase("ESI", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CESI.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CESI.Border = 15;
                    SecondtableHeadings.AddCell(CESI);
                }


                //37
                if (proftax != 0)
                {
                    PdfPCell CPT = new PdfPCell(new Phrase("PT", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CPT.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CPT.Border = 15;
                    SecondtableHeadings.AddCell(CPT);
                }

                if (owf != 0)
                {
                    PdfPCell CPT = new PdfPCell(new Phrase("LWF", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CPT.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CPT.Border = 15;
                    SecondtableHeadings.AddCell(CPT);
                }

                //30
                if (salAdvDed != 0)
                {
                    PdfPCell CSalAdv = new PdfPCell(new Phrase("Sal Adv", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CSalAdv.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CSalAdv.Border = 15;
                    SecondtableHeadings.AddCell(CSalAdv);
                }

                //31
                if (uniformDed != 0)
                {
                    PdfPCell CUnifDed = new PdfPCell(new Phrase("Unif. Ded", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CUnifDed.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CUnifDed.Border = 15;
                    SecondtableHeadings.AddCell(CUnifDed);
                }

                //32
                if (otherDed != 0)
                {
                    PdfPCell COtherDed = new PdfPCell(new Phrase("Other Ded", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    COtherDed.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    COtherDed.Border = 15;
                    SecondtableHeadings.AddCell(COtherDed);
                }

                //33
                if (canteenAdv != 0)
                {
                    PdfPCell Ccanteended = new PdfPCell(new Phrase("Mess. Ded", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    Ccanteended.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Ccanteended.Border = 15;
                    SecondtableHeadings.AddCell(Ccanteended);
                }

                //34
                if (roomrent != 0)
                {
                    PdfPCell CPenalty = new PdfPCell(new Phrase("Room Rent", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CPenalty.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CPenalty.Border = 15;
                    SecondtableHeadings.AddCell(CPenalty);
                }




                if (SecDepDedn != 0)
                {
                    PdfPCell CSecDepDedn = new PdfPCell(new Phrase("Sec Dep Ded", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CSecDepDedn.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CSecDepDedn.Border = 15;
                    SecondtableHeadings.AddCell(CSecDepDedn);
                }

                if (GenDedn != 0)
                {
                    PdfPCell CGenDedn = new PdfPCell(new Phrase("Gen Ded", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CGenDedn.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CGenDedn.Border = 15;
                    SecondtableHeadings.AddCell(CGenDedn);
                }




                //35
                if (totalDeductions != 0)
                {
                    PdfPCell CTotDed = new PdfPCell(new Phrase("Tot Ded", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CTotDed.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CTotDed.Border = 15;
                    SecondtableHeadings.AddCell(CTotDed);
                }

                //36
                if (netpay != 0)
                {
                    PdfPCell CNetPay = new PdfPCell(new Phrase("Net Pay", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CNetPay.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CNetPay.Border = 15;
                    SecondtableHeadings.AddCell(CNetPay);
                }

                //37
                PdfPCell CSignature = new PdfPCell(new Phrase("Bank A/c No./ Signature", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                CSignature.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CSignature.Border = 15;
                SecondtableHeadings.AddCell(CSignature);

                document.Add(SecondtableHeadings);

                #endregion





                #endregion
            }
        }

        protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddltype.SelectedIndex == 0)
            {
                lblfrommonth.Visible = false;
                txtfrommonth.Visible = false;
                lbltomonth.Visible = false;
                txttomonth.Visible = false;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();
            }
            else if (ddltype.SelectedIndex == 1)
            {
                lblmonth.Visible = false;
                txtmonth.Visible = false;
                lblfrommonth.Visible = true;
                txtfrommonth.Visible = true;
                lbltomonth.Visible = true;
                txttomonth.Visible = true;
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();
            }
        }





        //protected void btnPdf_Click(object sender, EventArgs e)
        //{

        //    DataTable dt = null;
        //    var list = new List<string>();

        //    if (GVListEmployees.Rows.Count > 0)
        //    {

        //        for (int i = 0; i < GVListEmployees.Rows.Count; i++)
        //        {
        //            CheckBox chkclientid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
        //            if (chkclientid.Checked == true)
        //            {
        //                Label lblclientid = GVListEmployees.Rows[i].FindControl("lblclientid") as Label;
        //                Label lblclientname = GVListEmployees.Rows[i].FindControl("lblclientname") as Label;

        //                if (chkclientid.Checked == true)
        //                {
        //                    list.Add("'" + lblclientid.Text + "'");
        //                }

        //            }

        //        }


        //    }

        //    string Clientids = string.Join(",", list.ToArray());



        //    string date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
        //    string month = DateTime.Parse(date).Month.ToString();
        //    string Year = DateTime.Parse(date).Year.ToString();

        //    var Month = month + Year.Substring(2, 2);


        //    string query = "select ROW_NUMBER() over( partition by eps.clientid order by eps.clientid,EPS.EmpId) as Sno,ED.EmpId,ISNULL(ED.EmpFName,' ')+' '+ISNULL(ED.EmpMName,' ')+' '+  " +
        //    " ISNULL(EmpLName,' ') EmpName,clientname,eps.clientid,D.Design,EPS.NoOfDuties,EPS.Basic,EPS.DA,EPS.HRA,EPS.CCA,EPS.Conveyance,EPS.WashAllowance," +
        //    " EPS.OtherAllowance,EPS.LeaveEncashAmt as Leavewages,EPS.Gratuity,EPS.Bonus,EPS.Nfhs,eps.WO,eps.WOAmt,eps.Npotsamt,eps.Nhsamt,eps.Npots,eps.nhs," +
        //    " EPS.RC,EPS.cs,EPS.Gross,eps.WO,eps.Npots,eps.NHS,eps.WOAmt,eps.Npotsamt,eps.Nhsamt,(eps.gross+eps.otamt) as subtotal,   " +
        //     "EPS.Incentivs,EPS.Pfonduties,EPS.Esionduties,EPS.PFONOT,EPS.ESIONOT,(EPS.PFONOT+EPS.Pfonduties) PFTotal, " +
        //    " (EPS.Esionduties+EPS.ESIONOT) ESITotal,EPS.ProfTax,EPS.SalAdvDed,EPS.UniformDed,EPS.OtherDed,EPS.CanteenAdv, " +
        //    " EPS.Penalty,EPS.TempGross,EPS.Gross,eps.SecurityDepDed as SecDepDedn,eps.roomrentded,eps.GeneralDed as GenDedn,  " +
        //       "  (EPS.Pf+eps.esi+EPS.ProfTax+EPS.SalAdvDed+EPS.UniformDed+EPS.OtherDed+EPS.CanteenAdv+eps.roomrentded+EPS.Penalty+ISNULL(eps.SecurityDepDed,0)+ISNULL(GeneralDed,0)) Totaldeduct, " +
        //       " ((EPS.Gross+EPS.OTAmt)-(EPS.Pf+EPS.Esi+EPS.ProfTax+EPS.SalAdvDed+EPS.UniformDed+ eps.roomrentded +ISNULL(eps.SecurityDepDed,0)+ISNULL(GeneralDed,0)+  " +
        //        "EPS.OtherDed+EPS.CanteenAdv+EPS.Penalty)) NetAmount,EPS.ots as Duties,(EPS.ots*8) Dutyhrs,  " +
        //        "EPS.TempGross Salrate,EPS.OTAmt as Amount,ED.EmpBankAcNo " +
        //    " from EmpDetails ED join EmpPaySheet EPS on EPS.EmpId=ED.EmpId " +
        //    " join Designations D on D.DesignId=EpS.Desgn inner join clients c on c.clientid =eps.clientid where EPS.Month='" + month + Year.Substring(2, 2) + "'  and EPS.ClientId in (" + Clientids + ") and " +
        //     "(EPS.NoOfDuties+EPS.ots)!=0 and (EPS.NoOfDuties+EPS.ots)>0 order by eps.clientid, EPS.EmpId ";

        //    dt = SqlHelper.Instance.GetTableByQuery(query);


        //    string clientid = "";
        //    string clientname = "";

        //    MemoryStream ms = new MemoryStream();

        //    if (dt.Rows.Count > 0)
        //    {
        //        Document document = new Document(PageSize.LEGAL.Rotate());
        //        PdfWriter writer = PdfWriter.GetInstance(document, ms);
        //        document.Open();
        //        document.AddTitle("FaMS");
        //        document.AddAuthor("WebWonders");
        //        document.AddSubject("Wage Sheet");
        //        document.AddKeywords("Keyword1, keyword2, …");//


        //        float forConvert;
        //        string strQry = "Select * from CompanyInfo   where   ClientidPrefix='" + CmpIDPrefix + "'";
        //        DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
        //        string companyName1 = "Your Company Name";
        //        string companyAddress = "Your Company Address";
        //        if (compInfo.Rows.Count > 0)
        //        {
        //            companyName1 = compInfo.Rows[0]["CompanyName"].ToString();
        //            companyAddress = compInfo.Rows[0]["Address"].ToString();
        //        }



        //        string selectclientaddress = "select * from clients where clientid  in (" + Clientids + ")";
        //        DataTable dtclientaddress = SqlHelper.Instance.GetTableByQuery(selectclientaddress);

        //        int FONT_SIZE = 8;


        //        PdfPTable Maintable = new PdfPTable(19);
        //        Maintable.TotalWidth = 970f;
        //       // SecondtableHeadings.HeaderRows = 6;
        //        Maintable.LockedWidth = true;
        //        float[] width = new float[] { 1.5f, 10f, 2f, 2f, 2f, 2f, 2.5f, 2f, 2f, 2f, 2.2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 6f };
        //        Maintable.SetWidths(width);

        //        #region

        //        PdfPCell Heading = new PdfPCell(new Phrase("[ See Rule 24(9-B) of the Karnataka Shops & Commercial Establishments Rules, 1963] in lieu of \n 1. Form I, II of Rule 22(4): Form IV of Rule 28(2); Form V & VII of Rule 29(1) & (5) of Karnataka Minimum wages Rules 1958; \n 2. Form I of Rules 3 (1) of Karnataka Payment of Wages Rules, 1963; \n 3. Form XIII of Rules 75; Form XV, XVII, XX, XXI, XXII, XXIII, of Rule 78 (1) a(i), (ii) &(iii) of the Karnataka Contract Labour (Regulation & Abolition) Rules, 1974; \n 4. Form XIII of Rule 43, Form XVII, XVIII, XIX, XX, XXI, XXII, of Rule 46(2) (a), (c) & (d) of Karnataka inter state Migrant Workmen Rules, 1981  ", FontFactory.GetFont(Fontstyle, 7, Font.NORMAL, BaseColor.BLACK)));
        //        Heading.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        Heading.Colspan = 5;
        //        Heading.Border = 0;
        //        Heading.SetLeading(0.0f, 1.5f);
        //        Maintable.AddCell(Heading);

        //        PdfPCell cellemp1 = new PdfPCell(new Phrase("  ", FontFactory.GetFont(Fontstyle, 10, Font.BOLD, BaseColor.BLACK)));
        //        cellemp1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //        cellemp1.Colspan = 1;
        //        cellemp1.Border = 0;
        //        //Maintable.AddCell(cellemp1);




        //        PdfPCell cellemp2 = new PdfPCell(new Phrase("Form  XVII \n Register Of Wages \n Wages Period Monthly \n  For the Month : " + GetMonthName() + " - " + GetMonthOfYear(), FontFactory.GetFont(Fontstyle, 7, Font.NORMAL, BaseColor.BLACK)));
        //        cellemp2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //        cellemp2.Colspan = 5;
        //        cellemp2.Border = 0;
        //        Maintable.AddCell(cellemp2);

        //        PdfPCell Heading1 = new PdfPCell(new Phrase("Name & Address of the Establishment \n Under Which Contract in Carried on : ", FontFactory.GetFont(Fontstyle, 7, Font.BOLD, BaseColor.BLACK)));
        //        Heading1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        Heading1.Colspan = 4;
        //        Heading1.Border = 0;
        //        Heading1.SetLeading(0.0f, 1.5f);
        //        Maintable.AddCell(Heading1);

        //        PdfPCell Heading21 = new PdfPCell(new Phrase(companyName1 + "\n" + companyAddress, FontFactory.GetFont(Fontstyle, 7, Font.NORMAL, BaseColor.BLACK)));
        //        Heading21.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        Heading21.Colspan = 5;
        //        Heading21.Border = 0;
        //        //Heading21.PaddingLeft = -30;
        //        Heading21.SetLeading(0.0f, 1.5f);
        //        Maintable.AddCell(Heading21);

        //        PdfPCell Cempty = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, 10, Font.NORMAL, BaseColor.BLACK)));
        //        Cempty.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        Cempty.Colspan = 19;
        //        Cempty.Border = 0;// 15;
        //        Maintable.AddCell(Cempty);


        //        PdfPCell CClient = new PdfPCell(new Phrase("Client ID :   " + clientid, FontFactory.GetFont(Fontstyle, 10, Font.NORMAL, BaseColor.BLACK)));
        //        CClient.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        CClient.Colspan = 9;
        //        CClient.Border = 0;// 15;

        //        Maintable.AddCell(CClient);

        //        PdfPCell CClientName = new PdfPCell();
        //        var caclientname = new Phrase();
        //        caclientname.Add(new Chunk("Name and Address of Principal Employer  ", FontFactory.GetFont(Fontstyle, 10, Font.NORMAL, BaseColor.BLACK)));
        //        caclientname.Add(new Chunk(clientname, FontFactory.GetFont(Fontstyle, 10, Font.NORMAL, BaseColor.BLACK)));
        //        CClientName.AddElement(caclientname);
        //        CClientName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        CClientName.Colspan = 10;
        //        CClientName.PaddingTop = -6;
        //        // CClientName.PaddingLeft = 250;
        //        CClientName.Border = 0;
        //        Maintable.AddCell(CClientName);
        //        Maintable.AddCell(Cempty);

        //        document.Add(Maintable);


        //        #endregion


        //        string empid="";
        //        string name="";
        //        string design="";
        //        float salrate=0;
        //        float duties=0;
        //        float Wo=0;
        //        float ED=0;
        //        float Nhs=0;
        //        float Basic=0;
        //        float DA=0;
        //        float HRA=0;
        //        float CCA=0;
        //        float Conv=0;
        //        float WA=0;
        //        float OA=0;
        //        float Bonus=0;
        //        float EL=0;
        //        float Gratuity=0;
        //        float Nfhs=0;
        //        float Incentives=0;
        //        float WoAmt=0;
        //        float NhsAmt=0;
        //        float Gross=0;
        //        float EDAmt=0;
        //        float TotalEarned=0;
        //        float PF=0;
        //        float ESI=0;
        //        float PT=0;
        //        float MessDed=0;
        //        float saladv=0;
        //        float Uniform=0;
        //        float SecDep=0;
        //        float RoomRent=0;
        //        float OtherDed=0;
        //        float GenDed=0;
        //        float TotalDed=0;
        //        float NetPay=0;



        //        PdfPTable SecondtableHeadings = new PdfPTable(16);
        //        SecondtableHeadings.TotalWidth = 970f;
        //        // SecondtableHeadings.HeaderRows = 6;
        //        SecondtableHeadings.LockedWidth = true;
        //        float[] widths = new float[] { 1.5f, 3f, 6f, 5f, 3f, 3f, 3f, 3f,3f, 3f, 3f, 2f, 3f, 3f, 3f, 3f, 6f };
        //        SecondtableHeadings.SetWidths(widths);


        //        #region for headings


        //        PdfPCell sNo = new PdfPCell(new Phrase("S.No", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
        //        sNo.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //        sNo.Border = 15;// 15;
        //        SecondtableHeadings.AddCell(sNo);


        //        PdfPCell CEmpid = new PdfPCell(new Phrase("Emp ID", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
        //        CEmpid.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //        CEmpid.Border = 15;// 15;
        //        SecondtableHeadings.AddCell(CEmpid);


        //        PdfPCell CEmpName = new PdfPCell(new Phrase("Emp Name", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
        //        CEmpName.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //        CEmpName.Border = 15;// 15;
        //        SecondtableHeadings.AddCell(CEmpName);


        //        PdfPCell CDesgn = new PdfPCell(new Phrase("Design", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
        //        CDesgn.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //        CDesgn.Border = 15;// 15;
        //        SecondtableHeadings.AddCell(CDesgn);


        //        PdfPCell CSalRate = new PdfPCell(new Phrase("Sal. Rate", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
        //        CSalRate.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //        CSalRate.Border = 15;// 15;
        //        SecondtableHeadings.AddCell(CSalRate);


        //        PdfPCell cell;


        //        cell = new PdfPCell(new Phrase("Dts\nWOs\nNHs\nED", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
        //        cell.HorizontalAlignment = 1;
        //        cell.Colspan = 1;
        //        cell.SetLeading(0f, 1.2f);
        //        SecondtableHeadings.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Basic\nDA\nHRA\nConv", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
        //        cell.HorizontalAlignment = 1;
        //        cell.Colspan = 1;
        //        cell.SetLeading(0f, 1.2f);
        //        SecondtableHeadings.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("WA\nOA\nEL\nCCA ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
        //        cell.HorizontalAlignment = 1;
        //        cell.Colspan = 1;
        //        cell.SetLeading(0f, 1.2f);
        //        SecondtableHeadings.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Bonus\nNFHs\nGratuity", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
        //        cell.HorizontalAlignment = 1;
        //        cell.Colspan = 1;
        //        cell.SetLeading(0f, 1.2f);
        //        SecondtableHeadings.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Gross\nED Amt\nTotal Earned", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
        //        cell.HorizontalAlignment = 1;
        //        cell.Colspan = 1;
        //        cell.SetLeading(0f, 1.2f);
        //        SecondtableHeadings.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("PF\nESI\nPT", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
        //        cell.HorizontalAlignment = 1;
        //        cell.Colspan = 1;
        //        cell.SetLeading(0f, 1.2f);
        //        SecondtableHeadings.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Sal Adv\nUniform\nOther Ded\nMess Ded", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
        //        cell.HorizontalAlignment = 1;
        //        cell.Colspan = 1;
        //        cell.SetLeading(0f, 1.2f);
        //        SecondtableHeadings.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Room Rent\nSec. Dep\nGen Ded", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
        //        cell.HorizontalAlignment = 1;
        //        cell.Colspan = 1;
        //        cell.SetLeading(0f, 1.2f);
        //        SecondtableHeadings.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Total Ded", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
        //        cell.HorizontalAlignment = 1;
        //        cell.Colspan = 1;
        //        cell.SetLeading(0f, 1.2f);
        //        SecondtableHeadings.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Net Pay", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
        //        cell.HorizontalAlignment = 1;
        //        cell.Colspan = 1;
        //        cell.SetLeading(0f, 1.2f);
        //        SecondtableHeadings.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Bank A/c No./ Signature", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
        //        cell.HorizontalAlignment = 1;
        //        cell.Colspan = 1;
        //        cell.SetLeading(0f, 1.2f);
        //        SecondtableHeadings.AddCell(cell);


        //        #endregion for headings

        //        int s = 1;

        //        for (int i = 0; i < dt.Rows.Count;i++ )
        //        {
        //            empid = dt.Rows[i]["EmpId"].ToString();
        //            name = dt.Rows[i]["EmpName"].ToString();
        //            design = dt.Rows[i]["Design"].ToString();
        //            salrate = Convert.ToSingle(dt.Rows[i]["Salrate"].ToString());
        //            duties = Convert.ToSingle(dt.Rows[i]["NoOfDuties"].ToString());
        //            Wo= Convert.ToSingle(dt.Rows[i]["WO"].ToString());
        //            ED= Convert.ToSingle(dt.Rows[i]["Duties"].ToString());
        //            Nhs= Convert.ToSingle(dt.Rows[i]["NHS"].ToString());
        //            Basic= Convert.ToSingle(dt.Rows[i]["Basic"].ToString());
        //            DA= Convert.ToSingle(dt.Rows[i]["DA"].ToString());
        //            HRA= Convert.ToSingle(dt.Rows[i]["HRA"].ToString());
        //            CCA= Convert.ToSingle(dt.Rows[i]["CCA"].ToString());
        //            Conv= Convert.ToSingle(dt.Rows[i]["Conveyance"].ToString());
        //            WA= Convert.ToSingle(dt.Rows[i]["washallowance"].ToString());
        //            OA= Convert.ToSingle(dt.Rows[i]["OtherAllowance"].ToString());
        //            Bonus= Convert.ToSingle(dt.Rows[i]["bonus"].ToString());
        //            EL=Convert.ToSingle(dt.Rows[i]["Leavewages"].ToString());
        //            Gratuity= Convert.ToSingle(dt.Rows[i]["Gratuity"].ToString());
        //            Incentives=Convert.ToSingle(dt.Rows[i]["Incentivs"].ToString());
        //            WoAmt=Convert.ToSingle(dt.Rows[i]["WoAmt"].ToString());
        //            NhsAmt=Convert.ToSingle(dt.Rows[i]["nhsAmt"].ToString());
        //            Nfhs=Convert.ToSingle(dt.Rows[i]["nfhs"].ToString());
        //            Gross=Convert.ToSingle(dt.Rows[i]["gross"].ToString());
        //            EDAmt=Convert.ToSingle(dt.Rows[i]["Amount"].ToString());
        //            TotalEarned=Convert.ToSingle(dt.Rows[i]["subtotal"].ToString());
        //            PF=Convert.ToSingle(dt.Rows[i]["PFTotal"].ToString());
        //            ESI=Convert.ToSingle(dt.Rows[i]["ESITOtal"].ToString());
        //            PT=Convert.ToSingle(dt.Rows[i]["ProfTax"].ToString());
        //            saladv= Convert.ToSingle(dt.Rows[i]["SalAdvDed"].ToString());
        //            Uniform= Convert.ToSingle(dt.Rows[i]["UniformDed"].ToString());
        //            MessDed=Convert.ToSingle(dt.Rows[i]["CanteenAdv"].ToString());
        //            SecDep= Convert.ToSingle(dt.Rows[i]["SecDepDedn"].ToString());
        //            RoomRent= Convert.ToSingle(dt.Rows[i]["penalty"].ToString());
        //            OtherDed=Convert.ToSingle(dt.Rows[i]["OtherDed"].ToString());
        //            GenDed= Convert.ToSingle(dt.Rows[i]["GenDedn"].ToString());
        //            TotalDed= Convert.ToSingle(dt.Rows[i]["Totaldeduct"].ToString());
        //            NetPay= Convert.ToSingle(dt.Rows[i]["NetAmount"].ToString());





        //            PdfPCell sNo1 = new PdfPCell(new Phrase(s.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            sNo1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //            sNo1.Border = 15;// 15;
        //            SecondtableHeadings.AddCell(sNo1);


        //            PdfPCell CEmpid1 = new PdfPCell(new Phrase(empid, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            CEmpid1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //            CEmpid1.Border = 15;// 15;
        //            SecondtableHeadings.AddCell(CEmpid1);


        //            PdfPCell CEmpName1 = new PdfPCell(new Phrase(name, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            CEmpName1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            CEmpName1.Border = 15;// 15;
        //            SecondtableHeadings.AddCell(CEmpName1);


        //            PdfPCell CDesgn1 = new PdfPCell(new Phrase(design.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            CDesgn1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            CDesgn1.Border = 15;// 15;
        //            SecondtableHeadings.AddCell(CDesgn1);


        //            PdfPCell CSalRate1 = new PdfPCell(new Phrase(salrate.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            CSalRate1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
        //            CSalRate1.Border = 15;// 15;
        //            SecondtableHeadings.AddCell(CSalRate1);


        //            cell = new PdfPCell(new Phrase(duties.ToString()+"\n"+Wo.ToString()+"\n"+Nhs.ToString()+"\n"+ED.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            cell.HorizontalAlignment = 1;
        //            cell.Colspan = 1;
        //            cell.SetLeading(0f, 1.2f);
        //            SecondtableHeadings.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(Basic.ToString()+"\n"+DA.ToString()+"\n"+HRA.ToString()+"\n"+Conv.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            cell.HorizontalAlignment = 2;
        //            cell.Colspan = 1;
        //            cell.SetLeading(0f, 1.2f);
        //            SecondtableHeadings.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(WA.ToString()+"\n"+OA.ToString()+"\n"+EL.ToString()+"\n"+CCA.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            cell.HorizontalAlignment = 2;
        //            cell.Colspan = 1;
        //            cell.SetLeading(0f, 1.2f);
        //            SecondtableHeadings.AddCell(cell);

        //             cell = new PdfPCell(new Phrase(Bonus.ToString()+"\n"+Nfhs.ToString()+"\n"+Gratuity.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            cell.HorizontalAlignment = 2;
        //            cell.Colspan = 1;
        //            cell.SetLeading(0f, 1.2f);
        //            SecondtableHeadings.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(Incentives.ToString() + "\n" + NhsAmt.ToString() + "\n" + WoAmt.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            cell.HorizontalAlignment = 2;
        //            cell.Colspan = 1;
        //            cell.SetLeading(0f, 1.2f);
        //            SecondtableHeadings.AddCell(cell);

        //           cell = new PdfPCell(new Phrase(Gross.ToString()+"\n"+EDAmt.ToString()+"\n"+TotalEarned.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            cell.HorizontalAlignment = 2;
        //            cell.Colspan = 1;
        //            cell.SetLeading(0f, 1.2f);
        //            SecondtableHeadings.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(PF.ToString()+"\n"+ESI.ToString()+"\n"+PT.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            cell.HorizontalAlignment = 2;
        //            cell.Colspan = 1;
        //            cell.SetLeading(0f, 1.2f);
        //            SecondtableHeadings.AddCell(cell);

        //           cell = new PdfPCell(new Phrase(saladv.ToString()+"\n"+Uniform.ToString()+"\n"+OtherDed.ToString()+"\n"+TotalEarned.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            cell.HorizontalAlignment = 2;
        //            cell.Colspan = 1;
        //            cell.SetLeading(0f, 1.2f);
        //            SecondtableHeadings.AddCell(cell);

        //           cell = new PdfPCell(new Phrase(RoomRent.ToString()+"\n"+SecDep.ToString()+"\n"+GenDed.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            cell.HorizontalAlignment = 2;
        //            cell.Colspan = 1;
        //            cell.SetLeading(0f, 1.2f);
        //            SecondtableHeadings.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(TotalDed.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            cell.HorizontalAlignment = 2;
        //            cell.Colspan = 1;
        //            cell.SetLeading(0f, 1.2f);
        //            SecondtableHeadings.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(NetPay.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            cell.HorizontalAlignment = 2;
        //            cell.Colspan = 1;
        //            cell.SetLeading(0f, 1.2f);
        //            SecondtableHeadings.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(dt.Rows[i]["EmpBankAcNo"].ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
        //            cell.HorizontalAlignment = 0;
        //            cell.Colspan = 1;
        //            cell.SetLeading(0f, 1.2f);
        //            SecondtableHeadings.AddCell(cell);

        //            s++;
        //        }


        //        document.Add(SecondtableHeadings);



        //        string filename = "Segmentwisepaysheet.pdf";

        //        document.Close();
        //        Response.ContentType = "application/pdf";
        //        Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        //        Response.Buffer = true;
        //        Response.Clear();
        //        Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
        //        Response.OutputStream.Flush();
        //        Response.End(); 








        //    }




        //}
    }
}