using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class WageSheetReports : System.Web.UI.Page
    {
        DataTable dt;
        string EmpIDPrefix = "";
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
                     
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }

                    LoadClientList();
                    LoadClientNames();

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }


        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        protected void LoadClientNames()
        {
            DataTable DtClientids = GlobalData.Instance.LoadCNames(CmpIDPrefix);
            if (DtClientids.Rows.Count > 0)
            {
                ddlcname.DataValueField = "Clientid";
                ddlcname.DataTextField = "clientname";
                ddlcname.DataSource = DtClientids;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "-Select-");
            ddlcname.Items.Insert(1, "ALL");

        }

        protected void LoadClientList()
        {
            DataTable DtClientNames = GlobalData.Instance.LoadCIds(CmpIDPrefix);
            if (DtClientNames.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "Clientid";
                ddlclientid.DataTextField = "Clientid";
                ddlclientid.DataSource = DtClientNames;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "-Select-");
            ddlclientid.Items.Insert(1, "ALL");
        }

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlcname.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlclientid.SelectedValue = ddlcname.SelectedValue;

            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        protected void ddlclientid_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();

            if (ddlclientid.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlcname.SelectedValue = ddlclientid.SelectedValue;
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();

            if (ddlclientid.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Client Id/Name');", true);

                return;
            }
            if (ddltype.SelectedIndex == 0)
            {
                if (txtmonth.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);

                    return;
                }
            }
            else
            {
                if (txtfromdate.Text.Trim().Length == 0 || txttodate.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select From Date and To Date');", true);

                    return;
                }
            }
            DisplayData();
        }

        protected void ClearData()
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
            //lbtn_Export.Visible = false;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            if (ddltype.SelectedIndex == 0)
            {
                GVUtil.Export("WageSheetReport.xls", this.GVListEmployees);
            }
            else
            {
                string fromdate = string.Empty;
                string ToDate = string.Empty;
                var SPName = "";
                Hashtable HTPaysheet = new Hashtable();
                if (txtfromdate.Text.Trim().Length > 0)
                {
                    fromdate = DateTime.Parse(txtfromdate.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                }

                if (txttodate.Text.Trim().Length > 0)
                {
                    ToDate = DateTime.Parse(txttodate.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                }

                string frommonth = DateTime.Parse(fromdate).Month.ToString("00");
                string fromYear = DateTime.Parse(fromdate).Year.ToString();

                string Tomonth = DateTime.Parse(ToDate).Month.ToString("00");
                string ToYear = DateTime.Parse(ToDate).Year.ToString();
                string ClientiD = string.Empty;

                ClientiD = ddlclientid.SelectedValue;

                if (ddlclientid.SelectedIndex == 1)
                {
                    ClientiD = "%";
                }

                string fromvalue = fromYear.Substring(2, 2) + frommonth;

                string Tovalue = ToYear.Substring(2, 2) + Tomonth;

                SPName = "WagesheetReport";


                HTPaysheet.Add("@Clientid", ClientiD);
                HTPaysheet.Add("@fromDate", fromvalue);
                HTPaysheet.Add("@ToDate", Tovalue);
                HTPaysheet.Add("@Option", ddltype.SelectedIndex);

                DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, HTPaysheet).Result;
                if (dt.Rows.Count > 0)
                {
                    GVListEmployeesFromTo.DataSource = dt;
                    GVListEmployeesFromTo.DataBind();
                    GVUtil.Export("WagesheetReport.xls", this.GVListEmployeesFromTo);
                }
            }
        }

        protected void GVListEmployees_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVListEmployees.PageIndex = e.NewPageIndex;

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

        decimal totalActualamount = 0;
        decimal totalDuties = 0;
        decimal totalOts = 0;
        decimal totalwo = 0;
        decimal totalnhs = 0;
        decimal totalnpots = 0;
        decimal totaltempgross = 0;
        decimal totalBasic = 0;
        decimal totalDA = 0;
        decimal totalHRA = 0;
        decimal totalCCA = 0;
        decimal totalConveyance = 0;
        decimal totalWA = 0;
        decimal totalOA = 0;
        decimal totalGrass = 0;
        decimal totalOTAmount = 0;
        decimal totalPF = 0;
        decimal totalESI = 0;
        decimal totalProfTax = 0;
        decimal totalSalAdv = 0;
        decimal totalUniformDed = 0;
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
        decimal totalRoomRentDed = 0;
        decimal totalGenDed = 0;

        protected void DisplayData()
        {
            if (ddlclientid.SelectedIndex > 0)
            {
                try
                {
                    var SPName = "";
                    Hashtable HTPaysheet = new Hashtable();
                    string date = string.Empty;
                    string fromdate = string.Empty;
                    string ToDate = string.Empty;
                    if (ddltype.SelectedIndex == 0)
                    {

                        if (txtmonth.Text.Trim().Length > 0)
                        {
                            date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                        }

                        string month = DateTime.Parse(date).Month.ToString();
                        string Year = DateTime.Parse(date).Year.ToString();
                        string ClientiD = string.Empty;

                        ClientiD = ddlclientid.SelectedValue;

                        if (ddlclientid.SelectedIndex == 1)
                        {
                            ClientiD = "%";
                        }

                        SPName = "WagesheetReport";

                        HTPaysheet.Add("@month", month + Year.Substring(2, 2));
                        HTPaysheet.Add("@Clientid", ClientiD);
                        HTPaysheet.Add("@Option", ddltype.SelectedIndex);
                    }

                    //else if(ddltype.SelectedIndex==1)
                    //{
                    //    btn_Submit.Visible = false;
                    //    lbtn_Export.Visible = true;
                    //}
                    DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, HTPaysheet).Result;
                    if (dt.Rows.Count > 0)

                        if (dt.Rows.Count > 0)
                        {

                            GVListEmployees.DataSource = dt;
                            GVListEmployees.DataBind();
                            lbtn_Export.Visible = true;

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

                            //#region commented by swathi
                            //Label tot = GVListEmployees.FooterRow.FindControl("lblTotalNetAmount") as Label;
                            //tot.Text = Math.Round(totalActualamount).ToString();

                            //Label lblTotalDuties = GVListEmployees.FooterRow.FindControl("lblTotalDuties") as Label;
                            //lblTotalDuties.Text = Math.Round(totalDuties).ToString();



                            //Label lblTotalOTs = GVListEmployees.FooterRow.FindControl("lblTotalOTs") as Label;
                            //lblTotalOTs.Text = Math.Round(totalOts).ToString();

                            ////if (totalOts > 0)
                            ////{
                            ////    GVListEmployees.Columns[5].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[5].Visible = false;

                            ////}

                            //Label lblTotalNhs = GVListEmployees.FooterRow.FindControl("lblTotalNhs") as Label;
                            //lblTotalNhs.Text = Math.Round(totalnhs).ToString();

                            ////if (totalnhs > 0)
                            ////{
                            ////    GVListEmployees.Columns[7].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[7].Visible = false;

                            ////}

                            //Label lblTotalwos = GVListEmployees.FooterRow.FindControl("lblTotalwos") as Label;
                            //lblTotalwos.Text = Math.Round(totalwo).ToString();

                            ////if (totalwo > 0)
                            ////{
                            ////    GVListEmployees.Columns[6].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[6].Visible = false;

                            ////}

                            //Label lblTotalNpots = GVListEmployees.FooterRow.FindControl("lblTotalNpots") as Label;
                            //lblTotalNpots.Text = Math.Round(totalnpots).ToString();

                            ////if (totalnpots > 0)
                            ////{
                            ////    GVListEmployees.Columns[8].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[8].Visible = false;

                            ////}

                            //Label lblTotaltempgross = GVListEmployees.FooterRow.FindControl("lblTotaltempgross") as Label;
                            //lblTotaltempgross.Text = Math.Round(totaltempgross).ToString();

                            //Label lblTotalBasic = GVListEmployees.FooterRow.FindControl("lblTotalBasic") as Label;
                            //lblTotalBasic.Text = Math.Round(totalBasic).ToString();

                            //Label lblTotalDA = GVListEmployees.FooterRow.FindControl("lblTotalDA") as Label;
                            //lblTotalDA.Text = Math.Round(totalDA).ToString();

                            ////if (totalDA > 0)
                            ////{
                            ////    GVListEmployees.Columns[11].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[11].Visible = false;

                            ////}

                            //Label lblTotalHRA = GVListEmployees.FooterRow.FindControl("lblTotalHRA") as Label;
                            //lblTotalHRA.Text = Math.Round(totalHRA).ToString();

                            ////if (totalHRA > 0)
                            ////{
                            ////    GVListEmployees.Columns[12].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[12].Visible = false;

                            ////}

                            //Label lblTotalCCA = GVListEmployees.FooterRow.FindControl("lblTotalCCA") as Label;
                            //lblTotalCCA.Text = Math.Round(totalCCA).ToString();

                            ////if (totalCCA > 0)
                            ////{
                            ////    GVListEmployees.Columns[13].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[13].Visible = false;

                            ////}

                            //Label lblTotalConveyance = GVListEmployees.FooterRow.FindControl("lblTotalConveyance") as Label;
                            //lblTotalConveyance.Text = Math.Round(totalConveyance).ToString();

                            ////if (totalConveyance > 0)
                            ////{
                            ////    GVListEmployees.Columns[14].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[14].Visible = false;

                            ////}

                            //Label lblTotalWA = GVListEmployees.FooterRow.FindControl("lblTotalWA") as Label;
                            //lblTotalWA.Text = Math.Round(totalWA).ToString();

                            ////if (totalWA > 0)
                            ////{
                            ////    GVListEmployees.Columns[15].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[15].Visible = false;

                            ////}

                            //Label lblTotalOA = GVListEmployees.FooterRow.FindControl("lblTotalOA") as Label;
                            //lblTotalOA.Text = Math.Round(totalOA).ToString();

                            ////if (totalOA > 0)
                            ////{
                            ////    GVListEmployees.Columns[16].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[16].Visible = false;

                            ////}


                            //Label lblTotalLeaveEncashAmt = GVListEmployees.FooterRow.FindControl("lblTotalLeaveEncashAmt") as Label;
                            //lblTotalLeaveEncashAmt.Text = Math.Round(totalLeaveEncashAmt).ToString();

                            ////if (totalLeaveEncashAmt > 0)
                            ////{
                            ////    GVListEmployees.Columns[17].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[17].Visible = false;

                            ////}
                            //Label lblTotalGratuity = GVListEmployees.FooterRow.FindControl("lblTotalGratuity") as Label;
                            //lblTotalGratuity.Text = Math.Round(totalGratuity).ToString();

                            ////if (totalGratuity > 0)
                            ////{
                            ////    GVListEmployees.Columns[18].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[18].Visible = false;

                            ////}

                            //Label lblTotalBonus = GVListEmployees.FooterRow.FindControl("lblTotalBonus") as Label;
                            //lblTotalBonus.Text = Math.Round(totalBonus).ToString();


                            ////if (totalBonus > 0)
                            ////{
                            ////    GVListEmployees.Columns[19].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[19].Visible = false;

                            ////}

                            //Label lblTotalNfhs = GVListEmployees.FooterRow.FindControl("lblTotalNfhs") as Label;
                            //lblTotalNfhs.Text = Math.Round(totalnfhs).ToString();

                            ////if (totalnfhs > 0)
                            ////{
                            ////    GVListEmployees.Columns[20].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[20].Visible = false;

                            ////}

                            //Label lblTotalGross = GVListEmployees.FooterRow.FindControl("lblTotalGross") as Label;
                            //lblTotalGross.Text = Math.Round(totalGrass).ToString();

                            //Label lblTotalIncentivs = GVListEmployees.FooterRow.FindControl("lblTotalIncentivs") as Label;
                            //lblTotalIncentivs.Text = Math.Round(totalIncentivs).ToString();

                            ////if (totalIncentivs > 0)
                            ////{
                            ////    GVListEmployees.Columns[28].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[28].Visible = false;

                            ////}


                            //Label lblTotalOTAmount = GVListEmployees.FooterRow.FindControl("lblTotalOTAmount") as Label;
                            //lblTotalOTAmount.Text = Math.Round(totalOTAmount).ToString();

                            ////if (totalOTAmount > 0)
                            ////{
                            ////    GVListEmployees.Columns[24].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[24].Visible = false;

                            ////}


                            //Label lblTotalPF = GVListEmployees.FooterRow.FindControl("lblTotalPF") as Label;
                            //lblTotalPF.Text = Math.Round(totalPF).ToString();



                            //Label lblTotalESI = GVListEmployees.FooterRow.FindControl("lblTotalESI") as Label;
                            //lblTotalESI.Text = Math.Round(totalESI).ToString();

                            //Label lblTotalProfTax = GVListEmployees.FooterRow.FindControl("lblTotalProfTax") as Label;
                            //lblTotalProfTax.Text = Math.Round(totalProfTax).ToString();

                            ////if (totalProfTax > 0)
                            ////{
                            ////    GVListEmployees.Columns[31].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[31].Visible = false;

                            ////}

                            //Label lblTotalsaladv = GVListEmployees.FooterRow.FindControl("lblTotalsaladv") as Label;
                            //lblTotalsaladv.Text = Math.Round(totalSalAdv).ToString();

                            ////if (totalSalAdv > 0)
                            ////{
                            ////    GVListEmployees.Columns[32].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[32].Visible = false;

                            ////}

                            //Label lblTotalUniformDed = GVListEmployees.FooterRow.FindControl("lblTotalUniformDed") as Label;
                            //lblTotalUniformDed.Text = Math.Round(totalUniformDed).ToString();

                            ////if (totalUniformDed > 0)
                            ////{
                            ////    GVListEmployees.Columns[33].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[33].Visible = false;

                            ////}


                            //Label lblTotalOtherDed = GVListEmployees.FooterRow.FindControl("lblTotalOtherDed") as Label;
                            //lblTotalOtherDed.Text = Math.Round(totalOtherDed).ToString();

                            ////if (totalOtherDed > 0)
                            ////{
                            ////    GVListEmployees.Columns[35].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[35].Visible = false;

                            ////}


                            //Label lblTotalSecDepDed = GVListEmployees.FooterRow.FindControl("lblTotalSecDepDed") as Label;
                            //lblTotalSecDepDed.Text = Math.Round(totalSecDepDed).ToString();


                            ////if (totalSecDepDed > 0)
                            ////{
                            ////    GVListEmployees.Columns[34].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[34].Visible = false;

                            ////}

                            //Label lblTotalRoomRentDed = GVListEmployees.FooterRow.FindControl("lblTotalRoomRentDed") as Label;
                            //lblTotalRoomRentDed.Text = Math.Round(totalRoomRentDed).ToString();

                            ////if (totalRoomRentDed > 0)
                            ////{
                            ////    GVListEmployees.Columns[36].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[36].Visible = false;

                            ////}

                            //Label lblTotalGeneralDed = GVListEmployees.FooterRow.FindControl("lblTotalGeneralDed") as Label;
                            //lblTotalGeneralDed.Text = Math.Round(totalGenDed).ToString();


                            ////if (totalGenDed > 0)
                            ////{
                            ////    GVListEmployees.Columns[37].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[37].Visible = false;

                            ////}

                            //Label lblTotalCanteenAdv = GVListEmployees.FooterRow.FindControl("lblTotalcantadv") as Label;
                            //lblTotalCanteenAdv.Text = Math.Round(totalCanteenAdv).ToString();

                            ////if (totalCanteenAdv > 0)
                            ////{
                            ////    GVListEmployees.Columns[38].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[38].Visible = false;

                            ////}

                            //Label lblTotalDeductions = GVListEmployees.FooterRow.FindControl("lblTotalDeductions") as Label;
                            //lblTotalDeductions.Text = Math.Round(totalDed).ToString();
                            ////if (totalDed > 0)
                            ////{
                            ////    GVListEmployees.Columns[41].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[41].Visible = false;

                            ////}

                            ////New code add as on 24/12/2013 by venkat

                            //Label lblTotalWOAmount = GVListEmployees.FooterRow.FindControl("lblTotalWOAmount") as Label;
                            //lblTotalWOAmount.Text = Math.Round(totalWoAmt).ToString();

                            ////if (totalWoAmt > 0)
                            ////{
                            ////    GVListEmployees.Columns[25].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[25].Visible = false;

                            ////}

                            //Label lblTotalNhsAmount = GVListEmployees.FooterRow.FindControl("lblTotalNhsAmount") as Label;
                            //lblTotalNhsAmount.Text = Math.Round(totalNhsAmt).ToString();

                            ////if (totalNhsAmt > 0)
                            ////{
                            ////    GVListEmployees.Columns[26].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[26].Visible = false;

                            ////}


                            //Label lblTotalNpotsAmount = GVListEmployees.FooterRow.FindControl("lblTotalNpotsAmount") as Label;
                            //lblTotalNpotsAmount.Text = Math.Round(totalNpotsAmt).ToString();

                            ////if (totalNpotsAmt > 0)
                            ////{
                            ////    GVListEmployees.Columns[27].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[27].Visible = false;

                            ////}


                            //Label lblTotalPenalty = GVListEmployees.FooterRow.FindControl("lblTotalPenalty") as Label;
                            //lblTotalPenalty.Text = Math.Round(totalPenalty).ToString();

                            ////if (totalPenalty > 0)
                            ////{
                            ////    GVListEmployees.Columns[40].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[40].Visible = false;

                            ////}


                            //Label lblTotalowf = GVListEmployees.FooterRow.FindControl("lblTotalowf") as Label;
                            //lblTotalowf.Text = Math.Round(totalOWF).ToString();

                            ////if (totalOWF > 0)
                            ////{
                            ////    GVListEmployees.Columns[39].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[39].Visible = false;

                            ////}

                            //Label lblTotalrc = GVListEmployees.FooterRow.FindControl("lblTotalrc") as Label;
                            //lblTotalrc.Text = Math.Round(totalRC).ToString();

                            ////if (totalRC > 0)
                            ////{
                            ////    GVListEmployees.Columns[21].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[21].Visible = false;

                            ////}

                            //Label lblTotalcs = GVListEmployees.FooterRow.FindControl("lblTotalcs") as Label;
                            //lblTotalcs.Text = Math.Round(totalCS).ToString();

                            ////if (totalCS > 0)
                            ////{
                            ////    GVListEmployees.Columns[22].Visible = true;
                            ////}
                            ////else
                            ////{
                            ////    GVListEmployees.Columns[22].Visible = false;

                            ////}
                            //#endregion commented by swathi

                            #endregion for old code


                            Label lblTotalNetAmount = GVListEmployees.FooterRow.FindControl("lblTotalNetAmount") as Label;
                            lblTotalNetAmount.Text = Math.Round(totalActualamount).ToString();

                            Label lblTotalDuties = GVListEmployees.FooterRow.FindControl("lblTotalDuties") as Label;
                            lblTotalDuties.Text = Math.Round(totalDuties).ToString();

                            Label lblTotaltempgross = GVListEmployees.FooterRow.FindControl("lblTotaltempgross") as Label;
                            lblTotaltempgross.Text = Math.Round(totaltempgross).ToString();

                            Label lblTotalBasic = GVListEmployees.FooterRow.FindControl("lblTotalBasic") as Label;
                            lblTotalBasic.Text = Math.Round(totalBasic).ToString();

                            Label lblTotalGross = GVListEmployees.FooterRow.FindControl("lblTotalGross") as Label;
                            lblTotalGross.Text = Math.Round(totalGrass).ToString();


                            Label lblTotalOTs = GVListEmployees.FooterRow.FindControl("lblTotalOTs") as Label;
                            lblTotalOTs.Text = Math.Round(totalOts).ToString();



                            Label lblTotalPF = GVListEmployees.FooterRow.FindControl("lblTotalPF") as Label;
                            lblTotalPF.Text = Math.Round(totalPF).ToString();



                            Label lblTotalESI = GVListEmployees.FooterRow.FindControl("lblTotalESI") as Label;
                            lblTotalESI.Text = Math.Round(totalESI).ToString();

                            if (totalOts > 0)
                            {
                                GVListEmployees.Columns[8].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[8].Visible = false;

                            }

                            Label lblTotalwos = GVListEmployees.FooterRow.FindControl("lblTotalwos") as Label;
                            lblTotalwos.Text = Math.Round(totalwo).ToString();

                            if (totalwo > 0)
                            {
                                GVListEmployees.Columns[9].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[9].Visible = false;

                            }

                            Label lblTotalNhs = GVListEmployees.FooterRow.FindControl("lblTotalNhs") as Label;
                            lblTotalNhs.Text = Math.Round(totalnhs).ToString();

                            if (totalnhs > 0)
                            {
                                GVListEmployees.Columns[10].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[10].Visible = false;

                            }



                            Label lblTotalNpots = GVListEmployees.FooterRow.FindControl("lblTotalNpots") as Label;
                            lblTotalNpots.Text = Math.Round(totalnpots).ToString();

                            if (totalnpots > 0)
                            {
                                GVListEmployees.Columns[11].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[11].Visible = false;

                            }



                            Label lblTotalDA = GVListEmployees.FooterRow.FindControl("lblTotalDA") as Label;
                            lblTotalDA.Text = Math.Round(totalDA).ToString();

                            if (totalDA > 0)
                            {
                                GVListEmployees.Columns[14].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[14].Visible = false;

                            }

                            Label lblTotalHRA = GVListEmployees.FooterRow.FindControl("lblTotalHRA") as Label;
                            lblTotalHRA.Text = Math.Round(totalHRA).ToString();

                            if (totalHRA > 0)
                            {
                                GVListEmployees.Columns[15].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[15].Visible = false;

                            }

                            Label lblTotalCCA = GVListEmployees.FooterRow.FindControl("lblTotalCCA") as Label;
                            lblTotalCCA.Text = Math.Round(totalCCA).ToString();

                            if (totalCCA > 0)
                            {
                                GVListEmployees.Columns[16].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[16].Visible = false;

                            }

                            Label lblTotalConveyance = GVListEmployees.FooterRow.FindControl("lblTotalConveyance") as Label;
                            lblTotalConveyance.Text = Math.Round(totalConveyance).ToString();

                            if (totalConveyance > 0)
                            {
                                GVListEmployees.Columns[17].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[17].Visible = false;

                            }

                            Label lblTotalWA = GVListEmployees.FooterRow.FindControl("lblTotalWA") as Label;
                            lblTotalWA.Text = Math.Round(totalWA).ToString();

                            if (totalWA > 0)
                            {
                                GVListEmployees.Columns[18].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[18].Visible = false;

                            }

                            Label lblTotalOA = GVListEmployees.FooterRow.FindControl("lblTotalOA") as Label;
                            lblTotalOA.Text = Math.Round(totalOA).ToString();

                            if (totalOA > 0)
                            {
                                GVListEmployees.Columns[19].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[19].Visible = true;

                            }


                            Label lblTotalLeaveEncashAmt = GVListEmployees.FooterRow.FindControl("lblTotalLeaveEncashAmt") as Label;
                            lblTotalLeaveEncashAmt.Text = Math.Round(totalLeaveEncashAmt).ToString();

                            if (totalLeaveEncashAmt > 0)
                            {
                                GVListEmployees.Columns[20].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[20].Visible = true;

                            }
                            Label lblTotalGratuity = GVListEmployees.FooterRow.FindControl("lblTotalGratuity") as Label;
                            lblTotalGratuity.Text = Math.Round(totalGratuity).ToString();

                            if (totalGratuity > 0)
                            {
                                GVListEmployees.Columns[21].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[21].Visible = true;

                            }

                            Label lblTotalBonus = GVListEmployees.FooterRow.FindControl("lblTotalBonus") as Label;
                            lblTotalBonus.Text = Math.Round(totalBonus).ToString();


                            if (totalBonus > 0)
                            {
                                GVListEmployees.Columns[22].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[22].Visible = true;

                            }

                            Label lblTotalNfhs = GVListEmployees.FooterRow.FindControl("lblTotalNfhs") as Label;
                            lblTotalNfhs.Text = Math.Round(totalnfhs).ToString();

                            if (totalnfhs > 0)
                            {
                                GVListEmployees.Columns[23].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[23].Visible = true;

                            }

                            Label lblTotalrc = GVListEmployees.FooterRow.FindControl("lblTotalrc") as Label;
                            lblTotalrc.Text = Math.Round(totalRC).ToString();

                            if (totalRC > 0)
                            {
                                GVListEmployees.Columns[24].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[24].Visible = true;

                            }

                            Label lblTotalcs = GVListEmployees.FooterRow.FindControl("lblTotalcs") as Label;
                            lblTotalcs.Text = Math.Round(totalCS).ToString();

                            if (totalCS > 0)
                            {
                                GVListEmployees.Columns[25].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[25].Visible = true;

                            }



                            Label lblTotalIncentivs = GVListEmployees.FooterRow.FindControl("lblTotalIncentivs") as Label;
                            lblTotalIncentivs.Text = Math.Round(totalIncentivs).ToString();

                            if (totalIncentivs > 0)
                            {
                                GVListEmployees.Columns[26].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[26].Visible = true;

                            }





                            Label lblTotalWOAmount = GVListEmployees.FooterRow.FindControl("lblTotalWOAmount") as Label;
                            lblTotalWOAmount.Text = Math.Round(totalWoAmt).ToString();

                            if (totalWoAmt > 0)
                            {
                                GVListEmployees.Columns[27].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[27].Visible = true;

                            }

                            Label lblTotalNhsAmount = GVListEmployees.FooterRow.FindControl("lblTotalNhsAmount") as Label;
                            lblTotalNhsAmount.Text = Math.Round(totalNhsAmt).ToString();

                            if (totalNhsAmt > 0)
                            {
                                GVListEmployees.Columns[28].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[28].Visible = true;

                            }


                            Label lblTotalNpotsAmount = GVListEmployees.FooterRow.FindControl("lblTotalNpotsAmount") as Label;
                            lblTotalNpotsAmount.Text = Math.Round(totalNpotsAmt).ToString();

                            if (totalNpotsAmt > 0)
                            {
                                GVListEmployees.Columns[29].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[29].Visible = true;

                            }


                            Label lblTotalOTAmount = GVListEmployees.FooterRow.FindControl("lblTotalOTAmount") as Label;
                            lblTotalOTAmount.Text = Math.Round(totalOTAmount).ToString();

                            if (totalOTAmount > 0)
                            {
                                GVListEmployees.Columns[31].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[31].Visible = true;

                            }



                            Label lblTotalProfTax = GVListEmployees.FooterRow.FindControl("lblTotalProfTax") as Label;
                            lblTotalProfTax.Text = Math.Round(totalProfTax).ToString();

                            if (totalProfTax > 0)
                            {
                                GVListEmployees.Columns[34].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[34].Visible = true;

                            }

                            Label lblTotalsaladv = GVListEmployees.FooterRow.FindControl("lblTotalsaladv") as Label;
                            lblTotalsaladv.Text = Math.Round(totalSalAdv).ToString();

                            if (totalSalAdv > 0)
                            {
                                GVListEmployees.Columns[35].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[35].Visible = true;

                            }

                            Label lblTotalUniformDed = GVListEmployees.FooterRow.FindControl("lblTotalUniformDed") as Label;
                            lblTotalUniformDed.Text = Math.Round(totalUniformDed).ToString();

                            if (totalUniformDed > 0)
                            {
                                GVListEmployees.Columns[36].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[36].Visible = true;

                            }


                            Label lblTotalOtherDed = GVListEmployees.FooterRow.FindControl("lblTotalOtherDed") as Label;
                            lblTotalOtherDed.Text = Math.Round(totalOtherDed).ToString();

                            if (totalOtherDed > 0)
                            {
                                GVListEmployees.Columns[37].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[37].Visible = true;

                            }

                            Label lblTotalRoomRentDed = GVListEmployees.FooterRow.FindControl("lblTotalRoomRentDed") as Label;
                            lblTotalRoomRentDed.Text = Math.Round(totalRoomRentDed).ToString();

                            if (totalRoomRentDed > 0)
                            {
                                GVListEmployees.Columns[38].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[38].Visible = true;

                            }

                            Label lblTotalCanteenAdv = GVListEmployees.FooterRow.FindControl("lblTotalcantadv") as Label;
                            lblTotalCanteenAdv.Text = Math.Round(totalCanteenAdv).ToString();

                            if (totalCanteenAdv > 0)
                            {
                                GVListEmployees.Columns[39].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[39].Visible = true;

                            }



                            Label lblTotalSecDepDed = GVListEmployees.FooterRow.FindControl("lblTotalSecDepDed") as Label;
                            lblTotalSecDepDed.Text = Math.Round(totalSecDepDed).ToString();


                            if (totalSecDepDed > 0)
                            {
                                GVListEmployees.Columns[40].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[40].Visible = true;

                            }


                            Label lblTotalGeneralDed = GVListEmployees.FooterRow.FindControl("lblTotalGeneralDed") as Label;
                            lblTotalGeneralDed.Text = Math.Round(totalGenDed).ToString();


                            if (totalGenDed > 0)
                            {
                                GVListEmployees.Columns[41].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[41].Visible = true;

                            }


                            Label lblTotalowf = GVListEmployees.FooterRow.FindControl("lblTotalowf") as Label;
                            lblTotalowf.Text = Math.Round(totalOWF).ToString();

                            if (totalOWF > 0)
                            {
                                GVListEmployees.Columns[42].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[42].Visible = true;

                            }

                            Label lblTotalPenalty = GVListEmployees.FooterRow.FindControl("lblTotalPenalty") as Label;
                            lblTotalPenalty.Text = Math.Round(totalPenalty).ToString();

                            if (totalPenalty > 0)
                            {
                                GVListEmployees.Columns[43].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[43].Visible = true;

                            }




                            Label lblTotalDeductions = GVListEmployees.FooterRow.FindControl("lblTotalDeductions") as Label;
                            lblTotalDeductions.Text = Math.Round(totalDed).ToString();
                            if (totalDed > 0)
                            {
                                GVListEmployees.Columns[44].Visible = true;
                            }
                            else
                            {
                                GVListEmployees.Columns[44].Visible = true;

                            }

                            //New code add as on 24/12/2013 by venkat




                        }

                    lbtn_Export.Visible = true;

                }
                catch (Exception ex)
                {
                }
            }


        }

        protected void GVListEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[46].Attributes.Add("class", "text");
            }

        }

        protected void GVListEmployeesFromTo_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Attributes.Add("class", "text");
                e.Row.Cells[4].Attributes.Add("class", "text");
            }

        }

        protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddltype.SelectedIndex == 0)
            {
                lblfrom.Visible = false;
                txtfromdate.Visible = false;
                lbltodate.Visible = false;
                txttodate.Visible = false;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                btn_Submit.Visible = true;
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();
            }

            if (ddltype.SelectedIndex == 1)
            {
                lblfrom.Visible = true;
                txtfromdate.Visible = true;
                lbltodate.Visible = true;
                txttodate.Visible = true;
                lblmonth.Visible = false;
                txtmonth.Visible = false;
                btn_Submit.Visible = false;
                lbtn_Export.Visible = true;
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();

            }
        }
    }
}