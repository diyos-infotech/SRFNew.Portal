using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class LoanDetailsMonthlywise : System.Web.UI.Page
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

            var query = "";
            if (ddltype.SelectedIndex == 0)
            {
                query = @"select * from (select  elm.EmpId,(empfname+' '+EmpMName+' '+EmpLName) as Empname,(LoanAmount),TypeOfLoan from EmpLoanMaster elm inner join EmpDetails ed on ed.empid=elm.empid where MONTH(LoanIssuedDate)='" + month + "' and YEAR(LoanIssuedDate)='" + Year + "' ) as S " +
                        "pivot" +
                        " (sum(LoanAmount) for typeofloan in ([0],[1],[2],[3],[4]) " +
                        " ) as PIVOTTABLE order by EmpId";

            }
            else
            {
                query = @"select * from (select eld.EmpId,(empfname+' '+EmpMName+' '+EmpLName) as Empname,(RecAmt),LoanType from EmpLoanDetails eld inner join EmpDetails ed on ed.empid=eld.empid where LoanCuttingMonth='" + month + Year.Substring(2, 2) + "' ) as S" +
                        " pivot " +
                        " (sum(recamt) for loantype in ([0],[1],[2],[3],[4]) " +
                        " ) as PIVOTTABLE order by EmpId";
            }

            DataTable envReports = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            GVListOfEmployees.DataSource = envReports;
            GVListOfEmployees.DataBind();


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

                GVUtil.ExporttoExcel(this.GVListOfEmployees, "Loans Issued Statement <br>" + GetMonthName() + "/" + GetMonthOfYear(), companyName + "<br>" + companyAddress);

            }
            else
            {
                GVUtil.ExporttoExcel(this.GVListOfEmployees, "Loans Deducted Statement <br>" + GetMonthName() + "/" + GetMonthOfYear(), companyName + "<br>" + companyAddress);

            }
        }

        decimal TotalSalAdvDed = 0;
        decimal TotalUniformDed = 0;
        decimal TotalSecDepDed = 0;
        decimal TotalOtherDed = 0;
        decimal TotalRoomRentDed = 0;



        protected void GVListOfEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string TotalSalaryAdvance = e.Row.Cells[2].Text;
                if (TotalSalaryAdvance == null)
                {
                    TotalSalAdvDed = 0;
                }
                else
                {
                    TotalSalAdvDed += decimal.Parse(TotalSalaryAdvance);
                }



                string TotalUniform = e.Row.Cells[3].Text;
                if (TotalUniform == null)
                {
                    TotalUniformDed = 0;
                }
                else
                {
                    TotalUniformDed += decimal.Parse(TotalUniform);
                }

                string TotalSecDep = e.Row.Cells[4].Text;
                if (TotalSecDep == null)
                {
                    TotalSecDepDed = 0;
                }
                else
                {
                    TotalSecDepDed += decimal.Parse(TotalSecDep);
                }

                string TotalOtherDeduction = e.Row.Cells[5].Text;
                if (TotalOtherDeduction == null)
                {
                    TotalOtherDed = 0;
                }
                else
                {
                    TotalOtherDed += decimal.Parse(TotalOtherDeduction);
                }

                string TotalRoomRentDeduction = e.Row.Cells[6].Text;
                if (TotalRoomRentDeduction == null)
                {
                    TotalRoomRentDed = 0;
                }
                else
                {
                    TotalRoomRentDed += decimal.Parse(TotalRoomRentDeduction);
                }



                //TotalAdv1 += Convert.ToDecimal(e.Row.Cells[2].Text);
                //TotalAdv2 += Convert.ToDecimal(e.Row.Cells[3].Text);
                //TotalAdv3 += Convert.ToDecimal(e.Row.Cells[4].Text);
                //TotalAdv4 += Convert.ToDecimal(e.Row.Cells[5].Text);
                //TotalLoan += Convert.ToDecimal(e.Row.Cells[6].Text);



            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Total";
                e.Row.Cells[2].Text = TotalSalAdvDed.ToString();
                e.Row.Cells[3].Text = TotalUniformDed.ToString();
                e.Row.Cells[4].Text = TotalSecDepDed.ToString();
                e.Row.Cells[5].Text = TotalOtherDed.ToString();
                e.Row.Cells[6].Text = TotalRoomRentDed.ToString();

            }
        }
    }
}