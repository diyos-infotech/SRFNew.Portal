using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using KLTS.Data;
using System.Globalization;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class IDCardReport : System.Web.UI.Page
    {
        DataTable dt;
        string GRVPrefix = "";
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";

        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();

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

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("IDCardReport.xls", this.GVIDCard);
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {

            GVIDCard.DataSource = null;
            GVIDCard.DataBind();
            lbtn_Export.Visible = false;



            var FromDate = DateTime.Now;
            var ToDate = DateTime.Now;


            if (Txt_From_Date.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Fill the From Date');", true);
                return;
            }
            if (Txt_To_Date.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Fill the To Date');", true);
                return;
            }

            FromDate = DateTime.Parse(Txt_From_Date.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));
            ToDate = DateTime.Parse(Txt_To_Date.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));


            string spname = "";
            DataTable dtIOM = null;
            Hashtable HashtableIOM = new Hashtable();

            spname = "IDCardReport";

            HashtableIOM.Add("@Fromdate", FromDate);
            HashtableIOM.Add("@TOdate", ToDate);


            dtIOM = config.ExecuteAdaptorAsyncWithParams(spname, HashtableIOM).Result;
            if (dtIOM.Rows.Count > 0)
            {
                lbtn_Export.Visible = true;
                GVIDCard.DataSource = dtIOM;
                GVIDCard.DataBind();
            }
            else
            {
                lbtn_Export.Visible = false;
                GVIDCard.DataSource = null;
                GVIDCard.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('There Is No Details For This Client');", true);

            }

        }

        protected void ClearData()
        {

            GVIDCard.DataSource = null;
            GVIDCard.DataBind();
        }

        public string GetMonthName()
        {
            string monthname = string.Empty;
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();
            return monthname;
        }
    }
}