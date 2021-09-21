using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Data;
using System.Collections;
using System.Globalization;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class BankUploadFormats : System.Web.UI.Page
    {
        //DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string BranchID = "";
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
                   
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }

        public void GetEmpDetail()
        {

        }

        protected void GetWebConfigdata()
        {
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        public string GetMonth()
        {
            string month = "";
            string year = "";
            string DateVal = "";
            DateTime date;


            if (txtmonth.Text != "")
            {
                date = DateTime.ParseExact(txtmonth.Text, "MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                month = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-GB")).Month.ToString();
                year = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-GB")).Year.ToString();

            }

            DateVal = month + year.Substring(2, 2);
            return DateVal;

        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            string date = string.Empty;
            string ClientID = "";

            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();

            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select The Month');", true);
                return;
            }

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string Month = GetMonth();

            string query = "select distinct(eps.clientid),clientname from emppaysheet eps inner join clients C on C.Clientid=eps.clientid where month='" + Month + "' and eps.clientid like '%" + CmpIDPrefix + "%'";
            DataTable dtClients = SqlHelper.Instance.GetTableByQuery(query);
            if (dtClients.Rows.Count > 0)
            {
                GVListEmployees.DataSource = dtClients;
                GVListEmployees.DataBind();
            }
            else
            {
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();

            }


            lbtn_Export.Visible = true;
        }

        protected void ClearData()
        {
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
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
            //return month;

            return month;

            #endregion
        }

        protected int GetMonth(string NameOfmonth)
        {
            int month = -1;
            var formatInfoinfo = new DateTimeFormatInfo();
            string[] monthName = formatInfoinfo.MonthNames;
            for (int i = 0; i < monthName.Length; i++)
            {
                if (monthName[i].CompareTo(NameOfmonth) == 0)
                {
                    month = i + 1;
                    break;
                }
            }
            return month;
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

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            DataTable DtListOfEmployees = new DataTable();

            string Month = GetMonth();
            var list = new List<string>();

            string clientname = "";


            GVListClients.DataSource = null;
            GVListClients.DataBind();

            if (GVListEmployees.Rows.Count > 0)
            {
                string strQry = "Select * from CompanyInfo";
                DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
                string companyName = "Your Company Name";
                //   string companyAddress = "Your Company Address";

                string heading1 = "";
                string heading = "";

                if (compInfo.Rows.Count > 0)
                {
                    companyName = compInfo.Rows[0]["CompanyName"].ToString();
                    //companyAddress = compInfo.Rows[0]["Address"].ToString();
                }



                for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                {
                    CheckBox chkclientid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                    if (chkclientid.Checked == true)
                    {
                        Label lblclientid = GVListEmployees.Rows[i].FindControl("lblclientid") as Label;
                        Label lblclientname = GVListEmployees.Rows[i].FindControl("lblclientname") as Label;

                        if (chkclientid.Checked == true)
                        {
                            list.Add(lblclientid.Text);
                        }

                    }

                }


            }

            string Clientids = string.Join(",", list.ToArray());

            DataTable dtClientList = new DataTable();
            dtClientList.Columns.Add("Clientid");
            if (list.Count != 0)
            {
                foreach (string s in list)
                {
                    DataRow row = dtClientList.NewRow();
                    row["Clientid"] = s;
                    dtClientList.Rows.Add(row);
                }
            }


            Hashtable HtsearchEmp = new Hashtable();
            string sp = "";
            sp = "BankUpLoadFormat";

            HtsearchEmp.Add("@month", Month);
            HtsearchEmp.Add("@ClientId", dtClientList);

            DtListOfEmployees = SqlHelper.Instance.ExecuteStoredProcedureWithParams(sp, HtsearchEmp);
            if (DtListOfEmployees.Rows.Count > 0)
            {
                GVListClients.DataSource = DtListOfEmployees;
                GVListClients.DataBind();
                GVUtil.Export("BankUploadFormat.xls", this.GVListClients);
            }
        }

        protected void GVListClients_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes.Add("class", "text");
                e.Row.Cells[7].Attributes.Add("class", "text");
            }

        }
    }
}