using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Globalization;
using SRF.P.DAL;

namespace SRF.P.Module_Inventory
{
    public partial class ReportforStock : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";

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

        protected void GetWebConfigdata()
        {
            if (Session.Keys.Count > 0)
            {
                EmpIDPrefix = Session["EmpIDPrefix"].ToString();
                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            GVInflowOutflowDetails.DataSource = null;
            GVInflowOutflowDetails.DataBind();

            if (txtFromDate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select From Date');", true);
                return;
            }

            if (txtToDate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select To Date');", true);
                return;
            }
            string spname = "";
            DataTable dtIOM = null;
            Hashtable HashtableIOM = new Hashtable();
            string FromDate = "";
            string ToDate = "";
            int options = 0;
            spname = "StockStatement";


            if (txtFromDate.Text.Trim().Length > 0)
            {
                FromDate = DateTime.Parse(txtFromDate.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            if (txtToDate.Text.Trim().Length > 0)
            {
                ToDate = DateTime.Parse(txtToDate.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }



            HashtableIOM.Add("@fromdate", FromDate);
            HashtableIOM.Add("@todate", ToDate);


            dtIOM = config.ExecuteAdaptorAsyncWithParams(spname, HashtableIOM).Result;
            if (dtIOM.Rows.Count > 0)
            {
                GVInflowOutflowDetails.DataSource = dtIOM;
                GVInflowOutflowDetails.DataBind();
            }
            else
            {
                GVInflowOutflowDetails.DataSource = null;
                GVInflowOutflowDetails.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('There Is No Details For This Client');", true);

            }

        }

        protected void ClearData()
        {
            GVInflowOutflowDetails.DataSource = null;
            GVInflowOutflowDetails.DataBind();
        }



        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("StockStatement.xls", this.GVInflowOutflowDetails);
        }
    }
}