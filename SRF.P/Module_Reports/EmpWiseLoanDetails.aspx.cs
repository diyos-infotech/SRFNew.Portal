using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using KLTS.Data;
using System.Globalization;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class EmpWiseLoanDetails : System.Web.UI.Page
    {
        DataTable dt;
        string EmpIDPrefix = "";
        string fontsyle = "verdana";
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
                }
            }
            catch (Exception ex)
            {
                GoToLoginPage();
            }


        }


        public void GoToLoginPage()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
            Response.Redirect("~/login.aspx");
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        protected void txtEmpid_TextChanged(object sender, EventArgs e)
        {
            GetEmpName();
        }

        protected void GetEmpName()
        {
            string Sqlqry = "select (empfname+' '+empmname+' '+emplname) as empname,EmpDesgn from empdetails where empid='" + txtEmpid.Text + "' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtName.Text = dt.Rows[0]["empname"].ToString();

                }
                catch (Exception ex)
                {
                    // MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                // MessageLabel.Text = "There Is No Name For The Selected Employee";
            }


        }

        protected void GetEmpid()
        {

            #region  Old Code
            string Sqlqry = "select Empid from empdetails where (empfname+' '+empmname+' '+emplname)  like '" + txtName.Text + "' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtEmpid.Text = dt.Rows[0]["Empid"].ToString();

                }
                catch (Exception ex)
                {
                    // MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                // MessageLabel.Text = "There Is No Name For The Selected Employee";
            }
            #endregion // End Old Code


        }

        protected void txtName_TextChanged(object sender, EventArgs e)
        {
            ClearData();
            GetEmpid();

        }

        protected void btnsudmit_Click(object sender, EventArgs e)
        {
            ClearData();
            string date = "";
            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();

            }
            string month = DateTime.Parse(date).Month.ToString();
            string year = DateTime.Parse(date).Year.ToString();


            #region  Begin Code For Variable Declaration
            var SPName = "";
            var Empid = "";


            DataTable DtListOfEmployees = null;
            Hashtable HtListOfEmployees = new Hashtable();

            #endregion End  Code For Variable Declaration


            #region  Begin Code For Assign Values to Variables


            SPName = "EmpWiseLoanDetails";
            Empid = txtEmpid.Text;
            HtListOfEmployees.Add("@Empid", Empid);
            HtListOfEmployees.Add("@month", month);
            HtListOfEmployees.Add("@Year", year);


            #endregion End  Code For Assign Values to Variables


            DtListOfEmployees = config.ExecuteAdaptorAsyncWithParams(SPName, HtListOfEmployees).Result;
            if (DtListOfEmployees.Rows.Count > 0)
            {
                gvresources.DataSource = DtListOfEmployees;
                gvresources.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('The Employee Details Are Not Avaialable');", true);
            }


        }
        protected void ClearData()
        {
            gvresources.DataSource = null;
            gvresources.DataBind();
        }
    }
}