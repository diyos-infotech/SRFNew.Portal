using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using SRF.P.DAL;
using System.Globalization;
using System.Collections;

namespace SRF.P.Module_Reports
{
    public partial class ReportForUnitWiseProfitMargin : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {
                   
                    Loadclientids();
                    Loadclientnames();
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

        protected void Loadclientids()
        {
            string sqlclientids = "select clientid from clients  Order by Clientid";
            DataTable dtids = config.ExecuteAdaptorAsyncWithQueryParams(sqlclientids).Result;
            if (dtids.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "clientid";
                ddlclientid.DataTextField = "clientid";
                ddlclientid.DataSource = dtids;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "--Select--");
            ddlclientid.Items.Insert(1, "All");
        }

        protected void Loadclientnames()
        {
            string sqlclientnames = "select clientid,clientname from clients    order by clientname";
            DataTable dtnames = config.ExecuteAdaptorAsyncWithQueryParams(sqlclientnames).Result;
            if (dtnames.Rows.Count > 0)
            {
                ddlcname.DataValueField = "clientid";
                ddlcname.DataTextField = "clientname";
                ddlcname.DataSource = dtnames;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "--Select--");
            ddlcname.Items.Insert(1, "All");
        }

        protected void Fillclientname()
        {
            if (ddlclientid.SelectedIndex > 1)
            {
                ddlcname.SelectedValue = ddlclientid.SelectedValue;
            }
            if (ddlclientid.SelectedIndex == 1)
            {
                ddlcname.SelectedIndex = 1;
            }

            if (ddlclientid.SelectedIndex == 0)
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void Fillclientid()
        {
            if (ddlcname.SelectedIndex > 1)
            {
                ddlclientid.SelectedValue = ddlcname.SelectedValue;
            }
            if (ddlcname.SelectedIndex == 1)
            {
                ddlclientid.SelectedIndex = 1;
            }

            if (ddlcname.SelectedIndex == 0)
            {
                ddlclientid.SelectedIndex = 0;
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

        protected void ddlclientid_SelectedIndexChanged(object sender, EventArgs e)
        {
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
            if (ddlclientid.SelectedIndex > 0)
            {
                Fillclientname();
                //Displaydata();
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }

        }

        protected void ddlclientname_SelectedIndexChanged(object sender, EventArgs e)
        {
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
            if (ddlcname.SelectedIndex > 0)
            {
                Fillclientid();
                // Displaydata();
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
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

            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);

                return;
            }
            DisplayData();
        }

        protected void ClearData()
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
            lbtn_Export.Visible = false;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            string strQry = "Select * from CompanyInfo ";
            DataTable compInfo = config.ExecuteReaderWithQueryAsync(strQry).Result;
            string companyName = "";
            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
            }
            string name = "UnitWiseProfitMarginReport.xls";
            string heading1 = companyName;
            string heading2 = "UNITWISE PROFIT MARGIN FOR THE MONTH OF " + GetMonthName() + "-" + GetMonthOfYear();
            int count = GVListEmployees.Columns.Count;

            GVUtil.GridExport(name, GVListEmployees, heading1, heading2, count);
            //GVUtil.ExporttoExcel2(name, this.GVListEmployees, heading1,heading2);
            //GVUtil.Export("UnitWiseProfitMarginReport.xls", this.GVListEmployees);
        }

        protected void GVListEmployees_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVListEmployees.PageIndex = e.NewPageIndex;

        }

        decimal totalActualamount = 0;
        decimal totalBilling = 0;
        decimal totalBasic = 0;
        decimal totalESIGross = 0;
        decimal totalGrass = 0;
        decimal totalWC = 0;
        decimal totalBonus = 0;
        decimal totalnfhs = 0;
        decimal totalPF = 0;
        decimal totalESI = 0;
        decimal totalUniformDed = 0;
        decimal totalDed = 0;
        decimal totalactAmount = 0;
        decimal totalArrearBilling = 0;
        decimal totalBonusBilling = 0;
        decimal totalElnhBilling = 0;

        protected void DisplayData()
        {
            if (ddlclientid.SelectedIndex > 0)
            {
                try
                {

                    string date = string.Empty;

                    if (txtmonth.Text.Trim().Length > 0)
                    {
                        date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                    }

                    string month = DateTime.Parse(date).Month.ToString();
                    string Year = DateTime.Parse(date).Year.ToString();
                    string ClientiD = string.Empty;

                    ClientiD = ddlclientid.SelectedValue;
                    string Type = "0";
                    if (ddlclientid.SelectedIndex == 1)
                    {
                        Type = "1";
                    }

                    var SPName = "";
                    Hashtable HTPaysheet = new Hashtable();
                    //SPName = "UnitWiseProfitMarginReport";
                    SPName = "UnitWiseProfitMarginReport";

                    HTPaysheet.Add("@month", month + Year.Substring(2, 2));
                    HTPaysheet.Add("@Clientid", ClientiD);
                    HTPaysheet.Add("@type", Type);

                    DataTable dt = config.ExecuteAdaptorAsyncWithParamsNew(SPName, HTPaysheet).Result;
                    if (dt.Rows.Count > 0)

                        if (dt.Rows.Count > 0)
                        {

                            GVListEmployees.DataSource = dt;
                            GVListEmployees.DataBind();

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {

                                //string actualAmount = dt.Rows[i]["ActualAmount"].ToString();
                                //if (actualAmount.Trim().Length > 0)
                                //{
                                //    actAmount = Convert.ToDecimal(actualAmount);
                                //}
                                //if (actAmount >= 0)
                                {
                                    string stractualAmount = dt.Rows[i]["ActualAmount"].ToString();
                                    if (stractualAmount.Trim().Length > 0)
                                    {
                                        totalactAmount += Convert.ToDecimal(stractualAmount);
                                    }

                                    string strBilling = dt.Rows[i]["Billing"].ToString();
                                    if (strBilling.Trim().Length > 0)
                                    {
                                        totalBilling += Convert.ToDecimal(strBilling);
                                    }

                                    string strBasic = dt.Rows[i]["Basic"].ToString();
                                    if (strBasic.Trim().Length > 0)
                                    {
                                        totalBasic += Convert.ToDecimal(strBasic);
                                    }

                                    string strESIGross = dt.Rows[i]["ESIWages"].ToString();
                                    if (strESIGross.Trim().Length > 0)
                                    {
                                        totalESIGross += Convert.ToDecimal(strESIGross);
                                    }

                                    string strGross = dt.Rows[i]["Gross"].ToString();
                                    if (strGross.Trim().Length > 0)
                                    {
                                        totalGrass += Convert.ToDecimal(strGross);
                                    }

                                    string strNfhs = dt.Rows[i]["LA"].ToString();
                                    if (strNfhs.Trim().Length > 0)
                                    {
                                        totalnfhs += Convert.ToDecimal(strNfhs);
                                    }

                                    string strUniformDed = dt.Rows[i]["UniformDed"].ToString();
                                    if (strUniformDed.Trim().Length > 0)
                                    {
                                        totalUniformDed += Convert.ToDecimal(strUniformDed);
                                    }

                                    string strWC = dt.Rows[i]["Gratuity"].ToString();
                                    if (strWC.Trim().Length > 0)
                                    {
                                        totalWC += Convert.ToDecimal(strWC);
                                    }


                                    string strBonus = dt.Rows[i]["Bonus"].ToString();
                                    if (strBonus.Trim().Length > 0)
                                    {
                                        totalBonus += Convert.ToDecimal(strBonus);
                                    }

                                    string strPF = dt.Rows[i]["PFEmpr"].ToString();
                                    if (strPF.Trim().Length > 0)
                                    {
                                        totalPF += Convert.ToDecimal(strPF);
                                    }
                                    string strESI = dt.Rows[i]["ESIEmpr"].ToString();
                                    if (strESI.Trim().Length > 0)
                                    {
                                        totalESI += Convert.ToDecimal(strESI);
                                    }

                                    string strDed = dt.Rows[i]["TotalDeductions"].ToString();
                                    if (strDed.Trim().Length > 0)
                                    {
                                        totalDed += Convert.ToDecimal(strDed);
                                    }

                                    string strArrearBilling = dt.Rows[i]["ArrearBilling"].ToString();
                                    if (strArrearBilling.Trim().Length > 0)
                                    {
                                        totalArrearBilling += Convert.ToDecimal(strArrearBilling);
                                    }

                                    string strBonusBilling = dt.Rows[i]["BonusBilling"].ToString();
                                    if (strBonusBilling.Trim().Length > 0)
                                    {
                                        totalBonusBilling += Convert.ToDecimal(strBonusBilling);
                                    }

                                    string strElnhBilling = dt.Rows[i]["ElnhBilling"].ToString();
                                    if (strElnhBilling.Trim().Length > 0)
                                    {
                                        totalElnhBilling += Convert.ToDecimal(strElnhBilling);
                                    }
                                }
                            }


                            Label lblTotalAmount = GVListEmployees.FooterRow.FindControl("lblTotalAmount") as Label;
                            lblTotalAmount.Text = Math.Round(totalactAmount).ToString();

                            Label lblTotalBilling = GVListEmployees.FooterRow.FindControl("lblTotalBilling") as Label;
                            lblTotalBilling.Text = Math.Round(totalBilling).ToString();

                            Label lblTotalArrearBilling = GVListEmployees.FooterRow.FindControl("lblTotalArrearBilling") as Label;
                            lblTotalArrearBilling.Text = Math.Round(totalArrearBilling).ToString();

                            Label lblTotalBonusBilling = GVListEmployees.FooterRow.FindControl("lblTotalBonusBilling") as Label;
                            lblTotalBonusBilling.Text = Math.Round(totalBonusBilling).ToString();

                            Label lblTotalElnhBilling = GVListEmployees.FooterRow.FindControl("lblTotalElnhBilling") as Label;
                            lblTotalElnhBilling.Text = Math.Round(totalElnhBilling).ToString();
                            //if(totalBilling>0)
                            //{
                            //    GVListEmployees.Columns[3].Visible = true;
                            //}
                            //else
                            //{
                            //    GVListEmployees.Columns[3].Visible = false;
                            //}

                            Label lblTotalBasic = GVListEmployees.FooterRow.FindControl("lblTotalBasic") as Label;
                            lblTotalBasic.Text = Math.Round(totalBasic).ToString();
                            //if (totalBasic > 0)
                            //{
                            //    GVListEmployees.Columns[4].Visible = true;
                            //}
                            //else
                            //{
                            //    GVListEmployees.Columns[4].Visible = false;
                            //}

                            Label lblTotalESIGross = GVListEmployees.FooterRow.FindControl("lblTotalESIGross") as Label;
                            lblTotalESIGross.Text = Math.Round(totalESIGross).ToString();
                            //if (totalESIGross > 0)
                            //{
                            //    GVListEmployees.Columns[5].Visible = true;
                            //}
                            //else
                            //{
                            //    GVListEmployees.Columns[5].Visible = false;
                            //}

                            Label lblTotalGross = GVListEmployees.FooterRow.FindControl("lblTotalGross") as Label;
                            lblTotalGross.Text = Math.Round(totalGrass).ToString();
                            //if (totalGrass > 0)
                            //{
                            //    GVListEmployees.Columns[6].Visible = true;
                            //}
                            //else
                            //{
                            //    GVListEmployees.Columns[6].Visible = false;
                            //}

                            Label lblTotalNFH = GVListEmployees.FooterRow.FindControl("lblTotalNFH") as Label;
                            lblTotalNFH.Text = Math.Round(totalnfhs).ToString();
                            //if (totalnfhs > 0)
                            //{
                            //    GVListEmployees.Columns[7].Visible = true;
                            //}
                            //else
                            //{
                            //    GVListEmployees.Columns[7].Visible = false;

                            //}

                            Label lblTotalUniform = GVListEmployees.FooterRow.FindControl("lblTotalUniform") as Label;
                            lblTotalUniform.Text = Math.Round(totalUniformDed).ToString();
                            //if (totalUniformDed > 0)
                            //{
                            //    GVListEmployees.Columns[8].Visible = true;
                            //}
                            //else
                            //{
                            //    GVListEmployees.Columns[8].Visible = false;

                            //}

                            Label lblTotalWC = GVListEmployees.FooterRow.FindControl("lblTotalWC") as Label;
                            lblTotalWC.Text = Math.Round(totalWC).ToString();

                            //if (totalWC > 0)
                            //{
                            //    GVListEmployees.Columns[9].Visible = true;
                            //}
                            //else
                            //{
                            //    GVListEmployees.Columns[9].Visible = false;

                            //}

                            Label lblTotalBonus = GVListEmployees.FooterRow.FindControl("lblTotalBonus") as Label;
                            lblTotalBonus.Text = Math.Round(totalBonus).ToString();
                            //if (totalBonus > 0)
                            //{
                            //    GVListEmployees.Columns[10].Visible = true;
                            //}
                            //else
                            //{
                            //    GVListEmployees.Columns[10].Visible = false;

                            //}

                            Label lblTotalPF = GVListEmployees.FooterRow.FindControl("lblTotalPF") as Label;
                            lblTotalPF.Text = Math.Round(totalPF).ToString();
                            //if (totalPF > 0)
                            //{
                            //    GVListEmployees.Columns[11].Visible = true;
                            //}
                            //else
                            //{
                            //    GVListEmployees.Columns[11].Visible = false;

                            //}


                            Label lblTotalESI = GVListEmployees.FooterRow.FindControl("lblTotalESI") as Label;
                            lblTotalESI.Text = Math.Round(totalESI).ToString();
                            //if (totalESI > 0)
                            //{
                            //    GVListEmployees.Columns[12].Visible = true;
                            //}
                            //else
                            //{
                            //    GVListEmployees.Columns[12].Visible = false;

                            //}



                            Label lblTotalTotalExpences = GVListEmployees.FooterRow.FindControl("lblTotalTotalExpences") as Label;
                            lblTotalTotalExpences.Text = Math.Round(totalDed).ToString();
                            //if (totalDed > 0)
                            //{
                            //    GVListEmployees.Columns[13].Visible = true;
                            //}
                            //else
                            //{
                            //    GVListEmployees.Columns[13].Visible = true;

                            //}
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
            //between FromDate And todate
            DateTime LastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            string strQry = "Select * from TblOptions where '" + LastDay + "' between FromDate And todate";
            DataTable dt = config.ExecuteReaderWithQueryAsync(strQry).Result;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[14].Text = "PF" + "\n@" + dt.Rows[0]["PFEmployer"].ToString();

            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[15].Text = "ESI" + "\n@" + dt.Rows[0]["ESIEmployer"].ToString();

            }

        }

        protected void lbtn_ExportNew_Click(object sender, EventArgs e)
        {
            if (ddlclientid.SelectedIndex > 0)
            {
                try
                {

                    string date = string.Empty;

                    if (txtmonth.Text.Trim().Length > 0)
                    {
                        date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                    }

                    string month = DateTime.Parse(date).Month.ToString();
                    string Year = DateTime.Parse(date).Year.ToString();
                    string ClientiD = string.Empty;

                    ClientiD = ddlclientid.SelectedValue;
                    string Type = "0";
                    if (ddlclientid.SelectedIndex == 1)
                    {
                        Type = "1";
                    }

                    var SPName = "";
                    Hashtable HTPaysheet = new Hashtable();
                    SPName = "UnitWiseProfitMarginReportNew";

                    HTPaysheet.Add("@month", month + Year.Substring(2, 2));
                    HTPaysheet.Add("@Clientid", ClientiD);
                    HTPaysheet.Add("@type", Type);

                    DataTable dt = config.ExecuteAdaptorAsyncWithParamsNew(SPName, HTPaysheet).Result;
                    if (dt.Rows.Count > 0)
                    {
                        GVUtil.ExporttoExcelProftmargin(dt);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}