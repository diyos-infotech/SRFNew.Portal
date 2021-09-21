using System;
using System.Collections;
using System.Web.UI;
using System.Data;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class ReportforBillingAndPaysheet : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";

        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();
        protected void Page_Load(object sender, EventArgs e)
        {
            //GetWebConfigdata();
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
            Elength = (EmpIDPrefix.Trim().Length + 1).ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            Clength = (CmpIDPrefix.Trim().Length + 1).ToString();
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please Select Month');", true);
                return;
            }

            DateTime date = Convert.ToDateTime(txtmonth.Text);
            string mon = string.Format("{0}{1:yy}", date.Month.ToString("00"), (date.Year - 2000).ToString());
            int month = Convert.ToInt32(mon);
            DateTime startDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            Hashtable References2 = new Hashtable();
            string SPReferencesName2 = "GetBillandPaysheet2";
            References2.Add("@month", month);
            DataTable dtbillandPaysheet2 = config.ExecuteAdaptorAsyncWithParams(SPReferencesName2, References2).Result;
            if (dtbillandPaysheet2.Rows.Count > 0)
            {
                GVListEmployees.DataSource = dtbillandPaysheet2;
                GVListEmployees.DataBind();

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('details are not available');", true);
                return;
            }

            Hashtable References = new Hashtable();
            string SPReferencesName = "GetBillandPaysheet";
            References.Add("@month", month);
            DataTable dtbillandPaysheet = config.ExecuteAdaptorAsyncWithParams(SPReferencesName, References).Result;
            txtBYes.Text = dtbillandPaysheet.Rows[0]["invoiceOne"].ToString();
            txtBNo.Text = dtbillandPaysheet.Rows[0]["invoiceZero"].ToString();
            txtPYes.Text = dtbillandPaysheet.Rows[0]["PaysheetOne"].ToString();
            txtPNo.Text = dtbillandPaysheet.Rows[0]["PaysheetZero"].ToString();
            txtBduties.Text = dtbillandPaysheet.Rows[0]["bildts"].ToString();
            txtPduties.Text = dtbillandPaysheet.Rows[0]["paydts"].ToString();

        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("AllUnitsEsiReport.xls", this.GVListEmployees);
        }
    }
}