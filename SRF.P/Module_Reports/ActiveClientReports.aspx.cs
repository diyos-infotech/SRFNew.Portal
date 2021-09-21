using System;
using System.Web.UI;
using System.Data;
using System.Collections;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class ActiveClientReports : System.Web.UI.Page
    {
        DataTable dt;
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
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void GetWebConfigdata()
        {
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }




        protected void ClearData()
        {
            LblResult.Text = "";
            GVListOfClients.DataSource = null;
            GVListOfClients.DataBind();
        }

        private void BindData(string SqlQuery)
        {
            ClearData();
            LblResult.Visible = true;

            dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQuery).Result;
            if (dt.Rows.Count > 0)
            {
                GVListOfClients.DataSource = dt;
                GVListOfClients.DataBind();
            }
            else
            {
                LblResult.Text = "There Is No Clients ";
            }
        }


        protected void Submit_Click(object sender, EventArgs e)
        {
            GVListOfClients.DataSource = null;
            GVListOfClients.DataBind();

            #region  Begin Code For Validation as on [16-11-2013]
            if (ddlClientsList.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select Client Mode');", true);
                return;
            }
            #endregion End  Code For Validation as on [16-11-2013]

            #region  Begin Code For Variable Declaration   as on [16-11-2013]
            var SPName = "";
            var Condition = 0;

            DataTable DtListOfClients = null;
            Hashtable HtListOfClients = new Hashtable();

            #endregion End  Code For Variable Declaration  as on [16-11-2013]


            #region  Begin Code For Assign Values to Variables   as on [16-11-2013]
            Condition = ddlClientsList.SelectedIndex;
            SPName = "ReportForListOfclientsBasedonConditions";

            HtListOfClients.Add("@Condition", Condition);
            HtListOfClients.Add("@clientidprefix", CmpIDPrefix);


            #endregion End  Code For Assign Values to Variables  as on [16-11-2013]

            #region  Begin code For Calling Stored Procedue  and Data To Gridview  As on [16-11-2013]
            DtListOfClients = config.ExecuteAdaptorAsyncWithParams(SPName, HtListOfClients).Result;
            if (DtListOfClients.Rows.Count > 0)
            {
                GVListOfClients.DataSource = DtListOfClients;
                GVListOfClients.DataBind();
            }
            else
            {
                GVListOfClients.DataSource = null;
                GVListOfClients.DataBind();

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('The Client Details Are Not Avaialable');", true);
            }

            #endregion End Code For Calling Stored Procedue and Data To Gridview  As on [16-11-2013]


        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("ClinetsList.xls", this.GVListOfClients);
        }


    }
}