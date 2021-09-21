using System;
using System.Web.UI;
using KLTS.Data;
using System.Data;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class ESIandPFForms : System.Web.UI.Page
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
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        protected void ddlForms_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (ddlForms.SelectedIndex == 1)
            {
                Response.Redirect("ESIReports.aspx?Value=" + ddlForms.SelectedValue);
            }
            if (ddlForms.SelectedIndex == 2)
            {
                Response.Redirect("ESIUploadReport.aspx?Value=" + ddlForms.SelectedValue);
            }
            if (ddlForms.SelectedIndex == 3)
            {
                Response.Redirect("PFReports.aspx?Value=" + ddlForms.SelectedValue);
            }
            if (ddlForms.SelectedIndex == 4)
            {
                Response.Redirect("UnitwiseESIPFPTReports.aspx?Value=" + ddlForms.SelectedValue);
            }
            if (ddlForms.SelectedIndex == 5)
            {
                Response.Redirect("EmployeewiseESIPFPTReports.aspx?Value=" + ddlForms.SelectedValue);
            }

        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {


        }

    }
}