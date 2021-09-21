using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using SRF.P.DAL;

namespace SRF.P.Module_Employees
{
    public partial class Employees : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
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
                    loadEmpIDs();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void DisplayData()
        {
            int OrderBy = 1;
            DataTable DtEmployees = GlobalData.Instance.LoadActiveEmployeesOrderBy(EmpIDPrefix, OrderBy);
            if (DtEmployees.Rows.Count > 0)
            {
                gvemployee.DataSource = DtEmployees;
                gvemployee.DataBind();
            }
            else
            {
                gvemployee.DataSource = null;
                gvemployee.DataBind();


                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('There is No employees . ');", true);
            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
        }
        public void loadEmpIDs()
        {
            string sqlqry = "Select distinct(EmpID) as EmpID ,(isnull(EmpID,'')+'-'+isnull(EmpFname,'')) as EmpIDDisplay from EmpDetails order by EmpID";
            DataTable dt = config.ExecuteReaderWithQueryAsync(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlEmpID.DataValueField = "EmpID";
                ddlEmpID.DataTextField = "EmpIDDisplay";
                ddlEmpID.DataSource = dt;
                ddlEmpID.DataBind();
            }

            ddlEmpID.Items.Insert(0, "-Select-");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                if (ddlEmpID.SelectedIndex == 0)
                {
                    lblMsg.Text = "Please Select The Employee ID/Name. Whatever You Want To Search";
                    return;
                }
                // Begin Code For Variable Declaration as on [18-10-2013]
                Hashtable HtSearchEmployee = new Hashtable();
                DataTable DtSearchEmployee = null;
                var SPName = "";
                var SearchedEmpIdOrName = "";

                //  Begin Code For Assign Values to The Variables as on [18-10-2013]
                SearchedEmpIdOrName = ddlEmpID.SelectedValue;
                SPName = "IMSearchEmployeeIdorName";

                //  Begin Code For SP Parameters as on [18-10-2013]
                HtSearchEmployee.Add("@EmpidorName", SearchedEmpIdOrName);
                HtSearchEmployee.Add("@empidprefix", EmpIDPrefix);


                //  Begin Code For Calling Stored Procedure as on [18-10-2013]
                DtSearchEmployee = config.ExecuteAdaptorAsyncWithParams(SPName, HtSearchEmployee).Result;


                //  Begin code For Assing Data to Gridview Whatever Retrived As on[18-10-2010]

                if (DtSearchEmployee.Rows.Count > 0)
                {
                    gvemployee.DataSource = DtSearchEmployee;
                    gvemployee.DataBind();
                }
                else
                {
                    gvemployee.DataSource = null;
                    gvemployee.DataBind();
                    lblMsg.Text = "There are no Employees";
                }


            }
            catch (Exception ex)
            {
                lblMsg.Text = "Your Session Expired . Please Login";
            }


        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblresult.Visible = true;
            Label empid = gvemployee.Rows[e.RowIndex].FindControl("lblempid") as Label;
            string deletequery = "Update Empdetails set empstatus=0  where EmpId ='" + empid.Text.Trim() + "'";
            int status = config.ExecuteNonQueryWithQueryAsync(deletequery).Result;
            if (status != 0)
            {
                lblSuc.Text = "Employee Inactivated Successfully";
            }
            else
            {
                lblMsg.Text = "Employee Not Inactivated ";
            }

            DisplayData();
        }

        protected void lbtn_Select_Click(object sender, EventArgs e)
        {
            try
            {

                ImageButton thisTextBox = (ImageButton)sender;
                GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
                Label lblid = (Label)thisGridViewRow.FindControl("lblempid");
                Response.Redirect("ModifyEmployee.aspx?Empid=" + lblid.Text, false);

            }
            catch (Exception ex)
            {
                lblMsg.Text = "Your Session Expired . Please Login";
            }

        }

        protected void lbtn_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton thisTextBox = (ImageButton)sender;
                GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
                Label lblid = (Label)thisGridViewRow.FindControl("lblempid");
                Response.Redirect("ModifyEmployee.aspx?Empid=" + lblid.Text, false);
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Your Session Expired . Please Login";

            }

        }

        protected void gvemployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvemployee.PageIndex = e.NewPageIndex;
            DisplayData();
        }

        protected void gvemployee_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label status = (Label)e.Row.FindControl("lblempGen") as Label;
                if (status.Text == "INACTIVE")
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                    //ImageButton edit = (ImageButton)e.Row.FindControl("lbtn_Edit") as ImageButton;
                    //edit.Enabled = false;
                }

            }
        }
    }
}