using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SRF.P.DAL;

namespace SRF.P.Module_Clients
{
    public partial class Clients1 : System.Web.UI.Page
    {
        DataTable dt;
        string CmpIDPrefix = "";

        AppConfiguration config = new AppConfiguration();

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

                    DisplayData();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void DisplayData()
        {
            gvclient.DataSource = null;
            gvclient.DataBind();
            var SPName = "";
            var Condition = 0;

            DataTable DtListOfClients = null;
            Hashtable HtListOfClients = new Hashtable();

            SPName = "ClientDetailsSearchBase";
            HtListOfClients.Add("@clientidprefix", CmpIDPrefix);

            DtListOfClients = config.ExecuteAdaptorAsyncWithParams(SPName, HtListOfClients).Result;
            if (DtListOfClients.Rows.Count > 0)
            {
                gvclient.DataSource = DtListOfClients;
                gvclient.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('The Client Details Are Not Avaialable');", true);
            }

        }

        protected void GetWebConfigdata()
        {
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            #region Begin Code For Check Validation/Become Zero Data  as on  [20-09-2013]
            gvclient.DataSource = null;
            gvclient.DataBind();

            if (txtsearch.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Enter Either Client ID Nor Client Name');", true);
                return;
            }

            #endregion End Code For Check Validation/Become Zero Data  as on  [20-09-2013]

            #region Begin Code For Declaration/Assign Values as on [20-09-2013]
            Hashtable HTSpParameters = new Hashtable();
            var SPName = "SearchIndClientIfo";
            var SearchedValue = txtsearch.Text;
            HTSpParameters.Add("@ClientidorName", SearchedValue);
            #endregion End  Code For Declaration/Assign Values as on [20-09-2013]

            #region Begin code For Calling Stored Procedure And Retrived Data/Bind To The Gridview  What user Searched    as on [20-09-2013]
            DataTable DtIndClientInfo = config.ExecuteAdaptorAsyncWithParams(SPName, HTSpParameters).Result;
            if (DtIndClientInfo.Rows.Count > 0)
            {
                gvclient.DataSource = DtIndClientInfo;
                gvclient.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Search not Found');", true);
            }
            #endregion  End   code For Calling Stored Procedure And Retrived Data/Bind To The Gridview  What user Searched    as on [20-09-2013]
        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblresult.Visible = true;
            Label ClientId = gvclient.Rows[e.RowIndex].FindControl("lblclientid") as Label;
            string deletequery = "delete from clients where ClientId ='" + ClientId.Text.Trim() + "'";
            int status = config.ExecuteNonQueryWithQueryAsync(deletequery).Result;
            if (status != 0)
            {
                lblresult.Text = "Client Deleted Successfully";
            }
            else
            {
                lblresult.Text = "Client Not Deleted ";
            }

            DisplayData();
        }

        protected void lbtn_Select_Click(object sender, EventArgs e)
        {
            try
            {

                ImageButton thisTextBox = (ImageButton)sender;
                GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
                Label lblid = (Label)thisGridViewRow.FindControl("lblclientid");
                Response.Redirect("ViewClient.aspx?Clientid=" + lblid.Text, false);

            }
            catch (Exception ex)
            {
            }

        }

        protected void lbtn_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton thisTextBox = (ImageButton)sender;
                GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
                Label lblid = (Label)thisGridViewRow.FindControl("lblclientid");
                Response.Redirect("ModifyClient.aspx?Clientid=" + lblid.Text, false);
            }
            catch (Exception ex)
            {

            }

        }

        protected void gvclient_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvclient.PageIndex = e.NewPageIndex;
            DisplayData();
        }
    }
}