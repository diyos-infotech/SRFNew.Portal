using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using System.Globalization;
using System.Collections;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using SRF.P.DAL;

namespace SRF.P.Module_Inventory
{
    public partial class UniformItemIssuedDetails : System.Web.UI.Page
    {
        DataTable dt;
        string EmpIDPrefix = "";
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
            if (Session.Keys.Count > 0)
            {
                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            }
            else
            {
                Response.Redirect("login.aspx");
            }

        }

        protected void ClearData()
        {
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
        }

        public void GetEmpiddatewise()
        {
            ddlEmpID.Items.Clear();
            ddlEmpName.Items.Clear();



            DateTime Fdate = DateTime.Now;
            DateTime Tdate = DateTime.Now;
            string FrmDt = "";
            string ToDt = "";

            if (txtfromdate.Text.Trim().Length > 0)
            {
                Fdate = DateTime.Parse(txtfromdate.Text, CultureInfo.GetCultureInfo("en-gb"));
            }

            if (txttodate.Text.Trim().Length > 0)
            {
                Tdate = DateTime.Parse(txttodate.Text, CultureInfo.GetCultureInfo("en-gb"));
            }
            FrmDt = Fdate.ToString("yyyy/MM/dd");
            ToDt = Tdate.ToString("yyyy/MM/dd");

            string qry = "";
            DataTable dt = null;


            qry = "select distinct ELM.empid from  EmpResourceDetails ERD  inner join EmpLoanMaster ELM on elm.loanno=ERD.loanno inner join EmpDetails ed on ed.EmpId=ELM.EmpID  where LoanIssuedDate between '" + FrmDt + "' and '" + ToDt + "'";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlEmpID.DataTextField = "empid";
                ddlEmpID.DataValueField = "empid";
                ddlEmpID.DataSource = dt;
                ddlEmpID.DataBind();
                ddlEmpID.Items.Insert(0, "All");
            }



            qry = "select distinct ELM.empid, (empfname+' '+empmname+' '+emplname) as empname from  EmpResourceDetails ERD  inner join EmpLoanMaster ELM on elm.loanno=ERD.loanno inner join EmpDetails ed on ed.EmpId=ELM.EmpID  where LoanIssuedDate between '" + FrmDt + "' and '" + ToDt + "'";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlEmpName.DataTextField = "empname";
                ddlEmpName.DataValueField = "empid";
                ddlEmpName.DataSource = dt;
                ddlEmpName.DataBind();
                ddlEmpName.Items.Insert(0, "All");

            }




        }


        protected void txtfromdate_TextChanged(object sender, EventArgs e)
        {
            GetEmpiddatewise();
        }
        protected void txttodate_TextChanged(object sender, EventArgs e)
        {
            GetEmpiddatewise();
        }

        protected void ddlEmpID_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            ddlEmpName.SelectedValue = ddlEmpID.SelectedValue;
        }
        protected void ddlEmpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            ddlEmpID.SelectedValue = ddlEmpName.SelectedValue;
        }


        protected void GetEmpName()
        {
            string Sqlqry = "select (empfname+' '+empmname+' '+emplname) as empname from empdetails where empid='" + ddlEmpID.SelectedValue + "' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlEmpID.SelectedValue = ddlEmpName.SelectedValue;
            }

        }


        protected void GetEmpid()
        {

            #region  Old Code
            string Sqlqry = "select Empid from empdetails where (empfname+' '+empmname+' '+emplname)  like '" + ddlEmpName.SelectedValue + "' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlEmpID.SelectedValue = ddlEmpName.SelectedValue;
            }
            #endregion // End Old Code


        }


        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("UniformIssuedDetails.xls", this.GVListEmployees);
        }


        protected void btnsearch_Click(object sender, EventArgs e)
        {

            ClearData();

            DateTime Fdate = DateTime.Now;
            DateTime Tdate = DateTime.Now;
            string FrmDt = "";
            string ToDt = "";
            var SPName = "";
            Hashtable HtDetails = new Hashtable();
            DataTable Dt = null;



            if (txtfromdate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select The From To Date');", true);
                return;
            }

            if (txtfromdate.Text.Trim().Length > 0)
            {
                Fdate = DateTime.Parse(txtfromdate.Text, CultureInfo.GetCultureInfo("en-gb"));
            }

            if (txttodate.Text.Trim().Length > 0)
            {
                Tdate = DateTime.Parse(txttodate.Text, CultureInfo.GetCultureInfo("en-gb"));
            }
            FrmDt = Fdate.ToString("yyyy/MM/dd");
            ToDt = Tdate.ToString("yyyy/MM/dd");

            string empid = "";
            if (ddlEmpID.SelectedIndex == 0)
            {
                empid = "%";
            }
            else
            {
                empid = ddlEmpID.SelectedValue;
            }

            SPName = "UNIFORMITEMISSUEDDETAILS";


            HtDetails.Add("@fromdate", FrmDt);
            HtDetails.Add("@todate", ToDt);
            HtDetails.Add("@Empid", empid);

            Dt = config.ExecuteAdaptorAsyncWithParams(SPName, HtDetails).Result;
            if (Dt.Rows.Count > 0)
            {

                GVListEmployees.DataSource = Dt;
                GVListEmployees.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('The Details Are Not Avaialable');", true);
            }

        }

    }
}