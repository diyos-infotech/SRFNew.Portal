﻿using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Globalization;
using KLTS.Data;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class PaySheetReport : System.Web.UI.Page
    {
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
                    FillClientList();
                    FillClientNameList();
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



        protected void FillClientList()
        {
            DataTable dt = GlobalData.Instance.LoadCIds(CmpIDPrefix);
            if (dt.Rows.Count > 0)
            {
                ddlClientId.DataValueField = "clientid";
                ddlClientId.DataTextField = "clientid";
                ddlClientId.DataSource = dt;
                ddlClientId.DataBind();
            }
            ddlClientId.Items.Insert(0, "--Select--");
            ddlClientId.Items.Insert(1, "All");

        }

        protected void FillClientNameList()
        {

            DataTable dt = GlobalData.Instance.LoadCNames(CmpIDPrefix);
            if (dt.Rows.Count > 0)
            {
                ddlcname.DataValueField = "clientid";
                ddlcname.DataTextField = "Clientname";
                ddlcname.DataSource = dt;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "--Select--");
            ddlcname.Items.Insert(1, "All");

        }

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlcname.SelectedIndex == 1)
            {
                ddlClientId.SelectedIndex = 1;
            }
            if (ddlcname.SelectedIndex > 1)
            {
                ddlClientId.SelectedValue = ddlcname.SelectedValue;
            }
            if (ddlcname.SelectedIndex == 0)
            {
                ddlClientId.SelectedIndex = 0;
            }

        }

        protected void ddlClientId_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlClientId.SelectedIndex == 1)
            {
                ddlcname.SelectedIndex = 1;
            }
            if (ddlClientId.SelectedIndex > 1)
            {
                ddlcname.SelectedValue = ddlClientId.SelectedValue;
            }
            if (ddlClientId.SelectedIndex == 0)
            {
                ddlcname.SelectedIndex = 0;
            }

        }

        protected void Binddata()
        {
            DataTable dt = null;
            var ProcedureName = "";
            var Clientid = "";
            Hashtable Htpaysheet = new Hashtable();

            var day = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
            var sqlqry = string.Empty;
            DateTime date = Convert.ToDateTime(day);

            var mon = string.Format("{0}{1:yy}", date.Month.ToString("00"), (date.Year - 2000).ToString());
            var month = Convert.ToInt32(mon);

            if (ddlClientId.SelectedIndex > 1)
            {
                Clientid = ddlClientId.SelectedValue;
            }

            ProcedureName = "Reportforpaysheet";

            Htpaysheet.Add("@Month", month);
            Htpaysheet.Add("@Clientid", Clientid);

            dt = config.ExecuteAdaptorAsyncWithParams(ProcedureName, Htpaysheet).Result;

            if (dt.Rows.Count > 0)
            {
                GVListEmployees.DataSource = dt;
                GVListEmployees.DataBind();
            }
            else
            {
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();
            }
        }

        protected void ClearData()
        {
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            if (ddlClientId.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Select Clientid');", true);
                return;
            }
            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Select Date');", true);
                return;
            }
            Binddata();
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("PaySheetReport.xls", this.GVListEmployees);
        }
    }
}