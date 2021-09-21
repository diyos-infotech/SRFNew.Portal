using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using SRF.P.DAL;
namespace SRF.P.Module_Reports
{
    public partial class ReportforPBAttendance : System.Web.UI.Page
    {
        DataTable dt;
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

                ddlcname.Items.Add("--Select--");
                LoadClientNames();
                FillClientList();
            }
        }


        protected void GetWebConfigdata()
        {
            EmpIDPrefix = GlobalData.Instance.GetEmployeeIDPrefix();
            Elength = (EmpIDPrefix.Trim().Length + 1).ToString();
            CmpIDPrefix = GlobalData.Instance.GetClientIDPrefix();
            Clength = (CmpIDPrefix.Trim().Length + 1).ToString();
        }

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlcname.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                FillClientid();
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        protected void ddlclientid_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();

            if (ddlclientid.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                Fillcname();

            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void Fillcname()
        {

            if (ddlclientid.SelectedIndex == 1)
            {
                ddlcname.SelectedIndex = 1;
                return;
            }
            string SqlQryForCname = "Select Clientname from Clients where clientid='" + ddlclientid.SelectedValue + "'";
            DataTable dtCname = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForCname).Result;
            if (dtCname.Rows.Count > 0)
                ddlcname.SelectedValue = dtCname.Rows[0]["Clientname"].ToString();
        }

        protected void FillClientid()
        {

            if (ddlcname.SelectedIndex == 1)
            {
                ddlclientid.SelectedIndex = 1;
                return;
            }


            string SqlQryForCid = "Select Clientid from Clients where clientname='" + ddlcname.SelectedValue + "'";
            DataTable dtCname = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForCid).Result;
            if (dtCname.Rows.Count > 0)
                ddlclientid.SelectedValue = dtCname.Rows[0]["Clientid"].ToString();
        }

        protected void LoadClientNames()
        {
            string selectquery = "select Clientname from Clients    where clientstatus=1 order by Clientname";
            DataTable dtable = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;
            for (int i = 0; i < dtable.Rows.Count; i++)
            {
                ddlcname.Items.Add(dtable.Rows[i]["Clientname"].ToString());
            }

            ddlcname.Items.Insert(1, "All");
        }

        protected void FillClientList()
        {
            string sqlQry = "Select ClientId from Clients Order By  CAST(right(Clientid,4) as int) ";
            DataTable data = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;
            ddlclientid.Items.Clear();
            ddlclientid.Items.Add("--Select--");
            for (int i = 0; i < data.Rows.Count; i++)
            {
                ddlclientid.Items.Add(data.Rows[i]["ClientId"].ToString());
            }

            ddlclientid.Items.Insert(1, "All");


        }

        protected void ClearData()
        {
            gvpaysheet.DataSource = null;
            gvpaysheet.DataBind();

            // lbtn_Export.Visible = false;
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {

            if (ddlclientid.SelectedIndex == 0 && ddlcname.SelectedIndex == 0)
            {
                if (txtmonth.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Select Client ID/Name  Or Month')", true);
                    return;
                }
            }

            if (ddlclientid.SelectedIndex > 0 && ddlcname.SelectedIndex > 0)
            {
                if (txtmonth.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Select Client ID/Name  Or Month')", true);
                    return;
                }
                else
                {
                    string month = DateTime.Parse(txtmonth.Text.Trim()).Month.ToString();
                    string Year = DateTime.Parse(txtmonth.Text.Trim()).Year.ToString();

                    if (ddlclientid.SelectedIndex == 1)
                    {
                        string SqlQryPSDuties = "Select EA.Clientid as  Clientid ,C.Clientname  as Clientname , sum(isnull(NoOfDuties,0)) as  PaysheetDuties ," +
                            "sum(isnull(OT,0)) as  PaysheetOts,   sum(isnull(NoOfDuties,0))+ sum(isnull(OT,0))  as TotalPaysheetduties From Empattendance as EA, Clients C " +
                            "   Where   EA.Clientid=C.Clientid  and Month='" + month + Year.Substring(2, 2) + "'  group by EA.Clientid,C.Clientname   Order By EA.Clientid   ";

                        //string SqlQryBDuties = "Select sum(isnull(Duties,0)) as  BillingDuties ,sum(isnull(Ot,0)) as  BillingOts  From Clientattendance " +
                        //   "   Where Month='" + month + Year.Substring(2, 2) + "'  Order By Clientid ";
                        BindDAta(SqlQryPSDuties);
                    }
                    else
                    {
                        string SqlQryPSDuties = "Select EA.Clientid as  Clientid  ,C.Clientname as Clientname, sum(isnull(NoOfDuties,0)) as  PaysheetDuties ,sum(isnull(OT,0)) as  PaysheetOts  From Empattendance as EA, Clients C " +
                              "   Where   EA.Clientid=C.Clientid  and Month='" + month + Year.Substring(2, 2) + "'  and  Clientid='" + ddlclientid.SelectedValue + "' ";


                        //string SqlQryBDuties = "Select sum(isnull(Duties,0)) as  BillingDuties ,sum(isnull(Ot,0)) as  BillingOts  From Clientattendance " +
                        //"   Where Month='" + month + Year.Substring(2, 2) + "'  and  Clientid='" + ddlclientid.SelectedValue + "'";
                        BindDAta(SqlQryPSDuties);
                    }
                }
            }
        }

        protected void BindDAta(string SqlqQryForPSD)
        {


            DataTable DtPSDuties = config.ExecuteAdaptorAsyncWithQueryParams(SqlqQryForPSD).Result;

            if (DtPSDuties.Rows.Count > 0)
            {
                gvpaysheet.DataSource = DtPSDuties;
                gvpaysheet.DataBind();
            }
            else
            {
                gvpaysheet.DataSource = null;
                gvpaysheet.DataBind();
            }

        }

        protected void gvpaysheet_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string month = DateTime.Parse(txtmonth.Text.Trim()).Month.ToString();
                string Year = DateTime.Parse(txtmonth.Text.Trim()).Year.ToString();

                string Empid = e.Row.Cells[0].Text;
                float PSDutiesandOts = float.Parse(e.Row.Cells[4].Text);
                Label BDuties = (Label)e.Row.FindControl("lblBillingduties");
                Label Bots = (Label)e.Row.FindControl("lblBillingots");
                Label BDifference = (Label)e.Row.FindControl("lblDifferenceduties");

                float BDutiesandOts = 0;
                float Difference = 0;


                string SqlQryBDuties = "Select sum(isnull(Duties,0)) as  BillingDuties ,sum(isnull(Ot,0)) as  BillingOts," +
                    " ( sum(isnull(Duties,0))+ sum(isnull(Ot,0))) as TotalDandOts    From Clientattendance " +
                                       "   Where Month='" + month + Year.Substring(2, 2) + "'   and  clientid='" + Empid + "' Group by  Clientid Order By Clientid ";
                DataTable dtBDuties = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryBDuties).Result;

                if (dtBDuties.Rows.Count > 0)
                {
                    BDutiesandOts = float.Parse(dtBDuties.Rows[0]["TotalDandOts"].ToString());
                    BDuties.Text = dtBDuties.Rows[0]["BillingDuties"].ToString();
                    Bots.Text = dtBDuties.Rows[0]["BillingOts"].ToString();

                }
                else
                {
                    BDuties.Text = "0";
                    Bots.Text = "0";
                }
                Difference = BDutiesandOts - PSDutiesandOts;
                BDifference.Text = Difference.ToString();

            }
        }
    }
}