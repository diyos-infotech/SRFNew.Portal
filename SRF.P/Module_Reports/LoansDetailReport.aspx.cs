using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class LoansDetailReport : System.Web.UI.Page
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
                query = "Select ISNULL(ELM.EmpId,'') as EmpId, (ISNULL(ED.EmpFName,'')+' '+ISNULL(ED.EmpMName,'')+' '+ISNULL(ED.EmpLName,'')) as EmpName, ISNULL(ELM.LoanNo,'') as LoanNo, ISNULL(convert(varchar, ELM.LoanDt, 103),'01/01/1990') as LoanDt, ISNULL(SUM(ELM.LoanAmount),0) as LoanAmount, ISNULL(SUM(ELM.PaidAmnt),0) as PaidAmnt from EmpResourceDetails ERD inner join EmpLoanMaster ELM on ELM.LoanNo=ERD.loanno inner join EmpLoanDetails ELD on ELD.LoanNo=ERD.loanno inner join EmpDetails ED on ED.EmpId=ELM.EmpId  Where MONTH(LoanIssuedDate)='" + month + "' and YEAR(LoanIssuedDate)='" + Year + "' and LoanCuttingMonth='" + month + Year.Substring(2, 2) + "' and ELM.TypeOfLoan=1  Group by ELM.EmpId, ED.EmpFName, ED.EmpMName, ED.EmpLName, ELM.LoanNo, ELM.LoanDt";

            }
            else
            {
                query = "Select ISNULL(ELM.EmpId,'') as EmpId, (ISNULL(ED.EmpFName,'')+' '+ISNULL(ED.EmpMName,'')+' '+ISNULL(ED.EmpLName,'')) as EmpName, ISNULL(ELM.LoanNo,'') as LoanNo, ISNULL(convert(varchar, ELM.LoanDt, 103),'01/01/1990') as LoanDt, ISNULL(SUM(ELM.LoanAmount),0) as LoanAmount, ISNULL(SUM(ELM.PaidAmnt),0) as PaidAmnt from EmpResourceDetails ERD inner join EmpLoanMaster ELM on ELM.LoanNo=ERD.loanno inner join EmpLoanDetails ELD on ELD.LoanNo=ERD.loanno inner join EmpDetails ED on ED.EmpId=ELM.EmpId  Where MONTH(LoanIssuedDate)='" + month + "' and YEAR(LoanIssuedDate)='" + Year + "' and LoanCuttingMonth='" + month + Year.Substring(2, 2) + "' and ELM.TypeOfLoan=1  Group by ELM.EmpId, ED.EmpFName, ED.EmpMName, ED.EmpLName, ELM.LoanNo, ELM.LoanDt";
            }

            DataTable envReports = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            GVListOfEmployees.DataSource = envReports;
            GVListOfEmployees.DataBind();

            lbtn_Export.Visible = true;

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

            if (ddltype.SelectedIndex == 0)
            {
                GVUtil.LoanDetailsExporttoExcel(this.GVListOfEmployees);
            }
            else
            {
                GVUtil.LoanDetailsExporttoExcel(this.GVListOfEmployees);
            }
        }

        decimal GTotalLoanAmount = 0;
        decimal GTotalPaidAmnt = 0;

        protected void GVListOfEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string TotalLoanAmount = e.Row.Cells[4].Text;
                if (TotalLoanAmount == null)
                {
                    GTotalLoanAmount = 0;
                }
                else
                {
                    GTotalLoanAmount += decimal.Parse(TotalLoanAmount);
                }

                string TotalPaidAmnt = e.Row.Cells[5].Text;
                if (TotalPaidAmnt == null)
                {
                    GTotalPaidAmnt = 0;
                }
                else
                {
                    GTotalPaidAmnt += decimal.Parse(TotalPaidAmnt);
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Total";
                e.Row.Cells[4].Text = GTotalLoanAmount.ToString();
                e.Row.Cells[5].Text = GTotalPaidAmnt.ToString();
            }
        }
    }
}