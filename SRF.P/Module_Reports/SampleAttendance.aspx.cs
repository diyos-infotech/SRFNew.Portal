using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using KLTS.Data;
using System.Globalization;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class SampleAttendance : System.Web.UI.Page
    {

        GridViewExportUtil GVUtil = new GridViewExportUtil();
        AppConfiguration config = new AppConfiguration();

        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        string Branch = "";

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
            string qry = "select Clientid,clientname from clients order by clientid ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlCName.DataValueField = "Clientid";
                ddlCName.DataTextField = "clientname";
                ddlCName.DataSource = dt;
                ddlCName.DataBind();
            }
            ddlCName.Items.Insert(0, "-Select-");
            ddlCName.Items.Insert(1, "ALL");

        }

        protected void LoadClientList()
        {


            string qry = "select Clientid from clients order by clientid ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlClientID.DataValueField = "Clientid";
                ddlClientID.DataTextField = "Clientid";
                ddlClientID.DataSource = dt;
                ddlClientID.DataBind();
            }
            ddlClientID.Items.Insert(0, "-Select-");
            ddlClientID.Items.Insert(1, "ALL");


        }

        protected void ddlCName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlCName.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlClientID.SelectedValue = ddlCName.SelectedValue;

            }
            else
            {
                ddlClientID.SelectedIndex = 0;
            }
        }

        protected void ddlClientID_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();

            if (ddlClientID.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlCName.SelectedValue = ddlClientID.SelectedValue;
            }
            else
            {
                ddlCName.SelectedIndex = 0;
            }
        }


        protected void btnsearch_Click(object sender, EventArgs e)
        {
            GVSampleAttendance.DataSource = null;
            GVSampleAttendance.DataBind();
            lbtn_Export.Visible = true;

            if (ddlClientID.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Client Id/Name');", true);

                return;
            }

            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);

                return;
            }
            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string Month = month + Year.Substring(2, 2);
            string clientid = "";
            if (ddlClientID.SelectedIndex == 1)
            {
                clientid = "%";
            }
            else
            {
                clientid = ddlClientID.SelectedValue;
            }
            string spname = "";
            DataTable dtBP = null;
            Hashtable HashtableBP = new Hashtable();

            int type = ddltype.SelectedIndex;

            spname = "SampleAttendance";
            HashtableBP.Add("@month", Month);
            HashtableBP.Add("@clientid", clientid);
            HashtableBP.Add("@type", type);

            dtBP = config.ExecuteAdaptorAsyncWithParams(spname, HashtableBP).Result;
            if (dtBP.Rows.Count > 0)
            {
                GVSampleAttendance.DataSource = dtBP;
                GVSampleAttendance.DataBind();
            }

        }

        protected void ClearData()
        {
            GVSampleAttendance.DataSource = null;
            GVSampleAttendance.DataBind();
            lbtn_Export.Visible = false;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("SampleAttendanceReports.xls", this.GVSampleAttendance);

        }

    }
}