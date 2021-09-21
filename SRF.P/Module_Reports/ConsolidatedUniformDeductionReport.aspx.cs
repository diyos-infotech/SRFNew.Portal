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
    public partial class ConsolidatedUniformDeductionReport : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";

        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();

        protected void Page_Load(object sender, EventArgs e)
        {
            int i = 0;
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

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            var testDate = 0;

            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();
            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select the From Date');", true);
                return;
            }
            else
            {
                testDate = GlobalData.Instance.CheckEnteredDate(txtmonth.Text);
                if (testDate > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid ORDER DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return;
                }
            }


            var SPName = "";
            Hashtable HTPaysheet = new Hashtable();
            SPName = "ConsolidatedUniformDeductionReport";

            HTPaysheet.Add("@month", month);
            HTPaysheet.Add("@year", Year);

            DataTable dt = config.ExecuteAdaptorAsyncWithParamsNew(SPName, HTPaysheet).Result;
            if (dt.Rows.Count > 0)
            {
                GVListOfEmployees.DataSource = dt;
                GVListOfEmployees.DataBind();
            }
            else
            {
                GVListOfEmployees.DataSource = null;
                GVListOfEmployees.DataBind();
            }




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

        protected void Lnkbtnexcel_Click(object sender, EventArgs e)
        {
            string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";

            DataTable compInfo = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;

            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";

            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
            }

            if (ddltype.SelectedIndex == 0)
            {

                GVUtil.UniformExporttoExcel(this.GVListOfEmployees, "Consolidated Uniform Statement <br>" + GetMonthName() + "/" + GetMonthOfYear(), companyName + "<br>" + companyAddress);

            }
            else
            {
                GVUtil.UniformExporttoExcel(this.GVListOfEmployees, "Consolidated Uniform Statement <br>" + GetMonthName() + "/" + GetMonthOfYear(), companyName + "<br>" + companyAddress);

            }
        }

        decimal TotalUniformIss = 0;
        decimal TotalUniformDed = 0;
        decimal TotalUniformPen = 0;

        protected void GVListOfEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string TotalUniformIssued = e.Row.Cells[2].Text;
                if (TotalUniformIssued == null)
                {
                    TotalUniformIss = 0;
                }
                else
                {
                    TotalUniformIss += decimal.Parse(TotalUniformIssued);
                }

                string TotalUniformDeduction = e.Row.Cells[3].Text;
                if (TotalUniformDeduction == null)
                {
                    TotalUniformDed = 0;
                }
                else
                {
                    TotalUniformDed += decimal.Parse(TotalUniformDeduction);
                }

                string TotalUniformPending = e.Row.Cells[4].Text;
                if (TotalUniformPending == null)
                {
                    TotalUniformPen = 0;
                }
                else
                {
                    TotalUniformPen += decimal.Parse(TotalUniformPending);
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Total";
                e.Row.Cells[2].Text = TotalUniformIss.ToString();
                e.Row.Cells[3].Text = TotalUniformDed.ToString();
                e.Row.Cells[4].Text = TotalUniformPen.ToString();
            }
        }

    }
}