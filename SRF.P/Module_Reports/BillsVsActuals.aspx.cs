using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class BillsVsActuals : System.Web.UI.Page
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
            DataTable dt = GlobalData.Instance.LoadCNames(CmpIDPrefix);
            if (dt.Rows.Count > 0)
            {
                ddlcname.DataValueField = "clientid";
                ddlcname.DataTextField = "Clientname";
                ddlcname.DataSource = dt;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "--Select--");
            ddlcname.Items.Insert(1, "ALL");


        }

        protected void LoadClientList()
        {
            DataTable dt = GlobalData.Instance.LoadCIds(CmpIDPrefix);
            if (dt.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "clientid";
                ddlclientid.DataTextField = "clientid";
                ddlclientid.DataSource = dt;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "--Select--");
            ddlclientid.Items.Insert(1, "ALL");
        }

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlcname.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlclientid.SelectedValue = ddlcname.SelectedValue;

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
                ddlcname.SelectedValue = ddlclientid.SelectedValue;
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            LblResult.Text = "";
            GVBillsVsActuals.DataSource = null;
            GVBillsVsActuals.DataBind();

            if (ddlclientid.SelectedIndex == 0)
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
            string Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);
            string Yearval = DateTime.Parse(date).Year.ToString();

            string Prevonemonth = string.Empty;
            string Prmonth = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).AddMonths(-1).ToString();
            Prevonemonth = DateTime.Parse(Prmonth).Month.ToString();

            string PrYear = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).Year.ToString();


            if (month == "1")
            {
                PrYear = PrYear;

            }
            else
            {
                PrYear = Year;
            }


            DateTime DtLastDay = DateTime.Now;
            DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            string sqlqry = string.Empty;

            DataTable dt = null;


            int currentmonthdays = GlobalData.Instance.GetNoOfDaysForThisMonthNew(int.Parse(Yearval), int.Parse(month));
            int prevmonthdays = GlobalData.Instance.GetNoOfDaysForThisMonthNew(int.Parse(PrYear), int.Parse(Prevonemonth));

            int type = 0;

            if (ddlclientid.SelectedIndex == 1)
            {
                type = 1;
            }

            string ClientID = "";

            if (ddlclientid.SelectedIndex > 1)
            {
                ClientID = ddlclientid.SelectedValue;
            }



            string SPName = "BillsVsActuals";
            Hashtable ht = new Hashtable();
            ht.Add("@month", month + Year);
            ht.Add("@Currentmonthdays", currentmonthdays);
            ht.Add("@PrMonthdays", prevmonthdays);
            ht.Add("@type", type);
            ht.Add("@LastDay", DtLastDay);
            ht.Add("@clientid", ClientID);


            dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            if (dt.Rows.Count > 0)
            {
                GVBillsVsActuals.DataSource = dt;
                GVBillsVsActuals.DataBind();
                lbtn_Export.Visible = true;
            }
            else
            {
                lbtn_Export.Visible = false;

            }





        }

        protected void ClearData()
        {
            LblResult.Text = "";
            GVBillsVsActuals.DataSource = null;
            GVBillsVsActuals.DataBind();
            lbtn_Export.Visible = false;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("BillVsActuals.xls", this.GVBillsVsActuals);
        }

        decimal ActualBillingAmt = 0;
        decimal BillingAmt = 0;
        decimal Difference = 0;

        decimal ActualBillDuties = 0;
        decimal BillingDuties = 0;
        decimal DiffDuties = 0;


        protected void GVBillsVsActuals_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ActualBillDuties += decimal.Parse(e.Row.Cells[3].Text);
                BillingDuties += decimal.Parse(e.Row.Cells[4].Text);
                DiffDuties += decimal.Parse(e.Row.Cells[5].Text);

                ActualBillingAmt += decimal.Parse(e.Row.Cells[6].Text);
                BillingAmt += decimal.Parse(e.Row.Cells[7].Text);
                Difference += decimal.Parse(e.Row.Cells[8].Text);

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Total";

                e.Row.Cells[3].Text = ActualBillDuties.ToString();
                e.Row.Cells[4].Text = BillingDuties.ToString();
                e.Row.Cells[5].Text = DiffDuties.ToString();

                e.Row.Cells[6].Text = ActualBillingAmt.ToString();
                e.Row.Cells[7].Text = BillingAmt.ToString();
                e.Row.Cells[8].Text = Difference.ToString();

            }
        }
    }
}