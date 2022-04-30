using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Data;
using System.Collections;
using System.Globalization;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class CreditNoteReport : System.Web.UI.Page
    {
        AppConfiguration Config = new AppConfiguration();
        GridViewExportUtil GVUtill = new GridViewExportUtil();
        DataTable dt;
        string CmpIDPrefix = "";
        string BranchID = "";

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
                FillClientList();
                FillClientNameList();
                LoadOurGSTNos();
            }
        }

        protected void GetWebConfigdata()
        {
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }



        private void LoadOurGSTNos()
        {
            DataTable DtGSTNos = GlobalData.Instance.LoadGSTNumbers();
            if (DtGSTNos.Rows.Count > 0)
            {
                ddlOurGSTIN.DataValueField = "Id";
                ddlOurGSTIN.DataTextField = "GSTNo";
                ddlOurGSTIN.DataSource = DtGSTNos;
                ddlOurGSTIN.DataBind();
            }

            ddlOurGSTIN.Items.Insert(0, new ListItem("All", "-1"));

        }


        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlcname.SelectedIndex > 0)
            {
                ddlClientId.SelectedValue = ddlcname.SelectedValue;
            }
            else
            {

                ddlClientId.SelectedIndex = 0;
            }

        }

        protected void ddlClientId_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlClientId.SelectedIndex > 0)
            {
                ddlcname.SelectedValue = ddlClientId.SelectedValue;
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void FillClientList()
        {
            string qry = "select Clientid from clients order by clientid ";
            DataTable dt = Config.ExecuteReaderWithQueryAsync(qry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlClientId.DataValueField = "clientid";
                ddlClientId.DataTextField = "clientid";
                ddlClientId.DataSource = dt;
                ddlClientId.DataBind();
            }
            ddlClientId.Items.Insert(0, "--Select--");
            ddlClientId.Items.Insert(1, "ALL");


        }

        protected void FillClientNameList()
        {
            string qry = "select Clientid,Clientname from clients order by clientname";
            DataTable dt = Config.ExecuteReaderWithQueryAsync(qry).Result;
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

        protected void ClearData()
        {
            GVInvoiceBills.DataSource = null;
            GVInvoiceBills.DataBind();
        }

        protected void Btn_Search_Invoice_Btn_Dates_Click(object sender, EventArgs e)
        {
            //LblResult.Text = "";
            DataTable DtNull = null;
            GVInvoiceBills.DataSource = DtNull;
            GVInvoiceBills.DataBind();
            Hashtable HtGetInvoiceAlldata = new Hashtable();

            string SPName = "CreditNoteReport";

            if (ddlClientId.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Please Select ClientId');", true);
                return;
            }
            else
            {
                var FromDate = DateTime.Now;
                var ToDate = DateTime.Now;
                string month = "";
                string Year = "";

                if (ddlPeriod.SelectedIndex == 0)
                {
                    if (Txt_From_Date.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Fill the From Date');", true);
                        return;
                    }
                    if (Txt_To_Date.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Fill the To Date');", true);
                        return;
                    }

                    FromDate = DateTime.Parse(Txt_From_Date.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));
                    ToDate = DateTime.Parse(Txt_To_Date.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));

                }
                else
                {
                    if (txtEndDate.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Fill the Month');", true);
                        return;
                    }


                    month = DateTime.Parse(txtEndDate.Text.Trim(), CultureInfo.GetCultureInfo("en-GB")).Month.ToString();
                    Year = DateTime.Parse(txtEndDate.Text.Trim(), CultureInfo.GetCultureInfo("en-GB")).Year.ToString().Substring(2, 2);

                }


                string SelectQueryByDate = string.Empty;
                string clientid = ddlClientId.SelectedValue;

                if (ddlClientId.SelectedIndex == 1)
                {
                    clientid = "%";

                }

                int billtype = ddlbilltype.SelectedIndex;

                int period = 0;

                if (ddlPeriod.SelectedIndex == 1)
                {
                    period = 1;
                }

                string GSTINUIN = ddlOurGSTIN.SelectedValue;

                if (ddlOurGSTIN.SelectedIndex == 0)
                {
                    GSTINUIN = "%";
                }


                HtGetInvoiceAlldata.Add("@FromDate", FromDate);
                HtGetInvoiceAlldata.Add("@ToDate", ToDate);
                HtGetInvoiceAlldata.Add("@ClientId", clientid);
                HtGetInvoiceAlldata.Add("@BillType", billtype);
                HtGetInvoiceAlldata.Add("@Period", period);
                HtGetInvoiceAlldata.Add("@month", month + Year);
                HtGetInvoiceAlldata.Add("@ClientIDPrefix", CmpIDPrefix);
                HtGetInvoiceAlldata.Add("@GSTINUIN", GSTINUIN);

                DataTable DtSqlData = Config.ExecuteAdaptorAsyncWithParams(SPName, HtGetInvoiceAlldata).Result;


                if (DtSqlData.Rows.Count > 0)
                {
                    GVInvoiceBills.DataSource = DtSqlData;
                    GVInvoiceBills.DataBind();

                    lbtn_Export.Visible = true;

                }
                else
                {
                    GVInvoiceBills.DataSource = null;
                    GVInvoiceBills.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('There is no data for selected client');", true);
                    return;
                }
            }

        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {

            GVUtill.Export("CreditNoteReport.xls", GVInvoiceBills);
        }

        protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            GVInvoiceBills.DataSource = null;
            GVInvoiceBills.DataBind();

            if (ddlPeriod.SelectedIndex == 1)
            {
                lblmonth.Visible = true;
                txtEndDate.Visible = true;
                Txt_From_Date.Visible = false;
                Txt_To_Date.Visible = false;
                lblfromdate.Visible = false;
                lbltodate.Visible = false;
            }
            else
            {
                lblmonth.Visible = false;
                txtEndDate.Visible = false;
                Txt_From_Date.Visible = true;
                Txt_To_Date.Visible = true;
                lblfromdate.Visible = true;
                lbltodate.Visible = true;
            }
        }
    }
}