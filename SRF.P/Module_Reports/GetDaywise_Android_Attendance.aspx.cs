using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using KLTS.Data;
using System.Globalization;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class GetDaywise_Android_Attendance : System.Web.UI.Page
    {

        GridViewExportUtil GVUtil = new GridViewExportUtil();
        AppConfiguration config = new AppConfiguration();
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string BranchID = "";

        //page changes done by Mahesh Goud on 2022-07-06
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
                    LoadStaffIDs();

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
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        protected void LoadClientNames()
        {
            string query = "select distinct clientid,clientname from clients where clientstatus=1";
            DataTable DtClientids = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (DtClientids.Rows.Count > 0)
            {
                ddlCName.DataValueField = "Clientid";
                ddlCName.DataTextField = "clientname";
                ddlCName.DataSource = DtClientids;
                ddlCName.DataBind();
            }
            ddlCName.Items.Insert(0, "-Select-");
            ddlCName.Items.Insert(1, "ALL");


        }

        protected void LoadStaffIDs()
        {
            DataTable DtopmEmpsIDs = GlobalData.Instance.LoadStaffIDs();
            if (DtopmEmpsIDs.Rows.Count > 0)
            {
                ddlFOID.DataValueField = "EmpId";
                ddlFOID.DataTextField = "FullName";
                ddlFOID.DataSource = DtopmEmpsIDs;
                ddlFOID.DataBind();
            }
            ddlFOID.Items.Insert(0, "-Select-");
            ddlFOID.Items.Insert(1, "ALL");

        }

        protected void LoadClientList()
        {

            string query = "select distinct clientid from clients where clientstatus=1";
            DataTable DtClientNames = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (DtClientNames.Rows.Count > 0)
            {
                ddlClientID.DataValueField = "clientid";
                ddlClientID.DataTextField = "clientid";
                ddlClientID.DataSource = DtClientNames;
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
            GvDayWiseAttendance.DataSource = null;
            GvDayWiseAttendance.DataBind();


            if (ddlOption.SelectedIndex == 0)
            {
                if (ddlClientID.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Client Id/Name');", true);

                    return;
                }
            }

            if (ddlOption.SelectedIndex == 1)
            {
                if (ddlFOID.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select FO ID');", true);

                    return;
                }
            }

            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);

                return;
            }
            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy/MM/dd");
            }
            string Type = "0";
            string clientid = "";
            string FOID = "";

            if (ddlClientID.SelectedIndex == 1)
            {
                clientid = "%";
            }
            else
            {
                clientid = ddlClientID.SelectedValue;
            }

            if (ddlFOID.SelectedIndex == 1)
            {
                FOID = "%";
            }
            else
            {
                FOID = ddlFOID.SelectedValue;
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string AttMonth = month + Year.Substring(2, 2);

            string spname = "";
            DataTable dtBP = null;
            Hashtable HashtableBP = new Hashtable();

            spname = "GetDaywise_Android_Attendance";
            HashtableBP.Add("@Date", date);
            HashtableBP.Add("@clientid", clientid);
            HashtableBP.Add("@Type", ddltypes.SelectedIndex);
            HashtableBP.Add("@Month", AttMonth);
            HashtableBP.Add("@Option", ddlOption.SelectedIndex);
            HashtableBP.Add("@FOID", FOID);


            dtBP = config.ExecuteAdaptorAsyncWithParams(spname, HashtableBP).Result;
            if (dtBP.Rows.Count > 0)
            {

                GvDayWiseAttendance.DataSource = dtBP;
                GvDayWiseAttendance.DataBind();
                lbtn_Export.Visible = true;
            }
            else
            {
                GvDayWiseAttendance.DataSource = null;
                GvDayWiseAttendance.DataBind();
                lbtn_Export.Visible = false;
            }

        }

        protected void ClearData()
        {
            GvDayWiseAttendance.DataSource = null;
            GvDayWiseAttendance.DataBind();
            lbtn_Export.Visible = false;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("Get_Android_Attendance.xls", this.GvDayWiseAttendance);

        }

        protected void ddltypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            GvDayWiseAttendance.DataSource = null;
            GvDayWiseAttendance.DataBind();

            if (ddlOption.SelectedIndex == 0)
            {
                lblclientid.Visible = true;
                ddlClientID.Visible = true;
                lblclientname.Visible = true;
                ddlCName.Visible = true;
                lblFOId.Visible = false;
                ddlFOID.Visible = false;
                ddlClientID.SelectedIndex = 0;
                ddlCName.SelectedIndex = 0;

            }
            else
            {
                lblclientid.Visible = false;
                ddlClientID.Visible = false;
                lblclientname.Visible = false;
                ddlCName.Visible = false;
                lblFOId.Visible = true;
                ddlFOID.Visible = true;
                ddlFOID.SelectedIndex = 0;
            }
        }




    }
}