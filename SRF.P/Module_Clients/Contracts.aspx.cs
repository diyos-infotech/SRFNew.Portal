using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SRF.P.DAL;

namespace SRF.P.Module_Clients
{
    public partial class Contracts : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";

        AppConfiguration config = new AppConfiguration();

        protected void Page_Load(object sender, EventArgs e)
        {

            int i = 0;

            try
            {
                GetWebConfigdata();
                if (!IsPostBack)
                {
                    //rowindexvisible = 1;
                    Session["ContractsAIndex"] = 0;
                    Session["ContractsAIndexsw"] = 0;
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
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }

        public void DisplayData()
        {

            string sqlQry = "Select Distinct c.Clientid,CL.Clientname,c.ContractId, convert(varchar(20),c.ContractStartDate,101) as ContractStartDate," +
            " convert(varchar(20),c.ContractEndDate,101) as ContractEndDate from Contracts as c inner join Clients CL on CL.Clientid=c.ClientId where C.Clientid like '" + CmpIDPrefix + "%' and c.contractid=(select max(ContractId) from contracts where Contracts.clientid=c.clientid)";
            DataTable data = config.ExecuteReaderWithQueryAsync(sqlQry).Result;

            if (data.Rows.Count > 0)
            {
                gvcontract.DataSource = data;
                gvcontract.DataBind();
            }
            else
            {
                lblMsg.Text = "The Contract Details Are Not Avaialable";
                //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('The Contract Details Are Not Avaialable');", true);
            }


        }

        protected void GetWebConfigdata()
        {

            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            #region Begin Code For Check Validation/Become Zero Data  as on  [20-09-2013]
            gvcontract.DataSource = null;
            gvcontract.DataBind();

            if (txtsearch.Text.Trim().Length == 0)
            {
                lblMsg.Text = "Please Enter Either Client ID Nor Client Name";
                //ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Enter Either Client ID Nor Client Name');", true);
                return;
            }

            #endregion End Code For Check Validation/Become Zero Data  as on  [20-09-2013]

            #region Begin Code For Declaration/Assign Values as on [20-09-2013]
            Hashtable HTSpParameters = new Hashtable();
            var SPName = "ContractDetailsSearchBase";
            var SearchedValue = txtsearch.Text;
            HTSpParameters.Add("@ClientidorName", SearchedValue);
            #endregion End  Code For Declaration/Assign Values as on [20-09-2013]

            #region Begin code For Calling Stored Procedure And Retrived Data/Bind To The Gridview  What user Searched    as on [20-09-2013]
            DataTable DtIndClientInfo = config.ExecuteAdaptorAsyncWithParams(SPName, HTSpParameters).Result;
            if (DtIndClientInfo.Rows.Count > 0)
            {
                gvcontract.DataSource = DtIndClientInfo;
                gvcontract.DataBind();
            }
            else
            {
                lblMsg.Text = "There Is No Clients.What you are Entered";
                //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('There Is No Clients.What you are Entered');", true);
            }
            #endregion  End   code For Calling Stored Procedure And Retrived Data/Bind To The Gridview  What user Searched    as on [20-09-2013]
        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblresult.Visible = true;
            Label ClientId = gvcontract.Rows[e.RowIndex].FindControl("lblClientid") as Label;
            string deletequery = "delete from contracts where ClientId ='" + ClientId.Text.Trim() + "'";
            int status = config.ExecuteNonQueryWithQueryAsync(deletequery).Result;
            if (status != 0)
            {
                lblSuc.Text = "Contract Deleted Successfully";
            }
            else
            {
                lblMsg.Text = "Contract Not Deleted ";
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
                Response.Redirect("ViewContracts.aspx?Clientid=" + lblid.Text, false);

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
                Label lblcontractid = (Label)thisGridViewRow.FindControl("lblcontractid");
                Response.Redirect("AddContract.aspx?Clientid=" + lblid.Text + "&ContractID=" + lblcontractid.Text, false);

            }
            catch (Exception ex)
            {

            }

        }

        protected void gvclient_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvcontract.PageIndex = e.NewPageIndex;
            DisplayData();
        }
    }
}