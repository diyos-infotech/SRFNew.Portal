using SRF.P.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRF.P.Module_Reports
{
    public partial class BillModification : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        string Username = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Username = Session["UserId"].ToString();

            if (!IsPostBack)
            {
                LoadClients();
                rdbBillNo_CheckedChanged(sender, e);
            }
        }

        public void LoadClients()
        {
            string qry = "select clientid,(clientid+' - '+clientname) as Clientname from clients where clientstatus=1";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlClient.DataValueField = "Clientid";
                ddlClient.DataTextField = "clientname";
                ddlClient.DataSource = dt;
                ddlClient.DataBind();
            }
            ddlClient.Items.Insert(0, "-Select-");
        }

        protected void btnTemp_Click(object sender, EventArgs e)
        {
            try
            {

                string SPName = "";
                Hashtable ht = new Hashtable();
                DataTable dt = null;

                string Client = "";
                string BillType = "";
                string monthval = "";
                string BillNo = "";

                SPName = "BillModification";
                ht = new Hashtable();
                ht.Add("@Type", "CheckPassword");
                ht.Add("@Password", hfPassword.Value);
                dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;


                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please check the password');", true);
                    return;
                }
                else
                {

                    if (rdbBillNo.Checked)
                    {

                        if (txtBillNo.Text.Length == 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Bill No');", true);
                            return;
                        }

                        Client = txtClientID.Text.Trim();
                        BillType = ddlBillTypeVal.SelectedValue;
                        monthval = lblmonth.Text.Trim();
                        BillNo = txtBillNo.Text.Trim();
                    }
                    else
                    {
                        if (ddlClient.SelectedIndex == 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Client');", true);
                            return;
                        }

                        if (txtMonth.Text.Length == 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);
                            return;
                        }


                        Client = ddlClient.SelectedValue;
                        BillType = ddlBillType.SelectedValue;

                        string date = string.Empty;
                        string month = "";
                        string Year = "";

                        if (txtMonth.Text.Trim().Length > 0)
                        {
                            date = DateTime.Parse(txtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                            month = DateTime.Parse(date).Month.ToString();
                            Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);
                        }

                        monthval = month + Year;
                        BillNo = ddlBillNo.SelectedValue;
                    }

                    SPName = "BillModification";
                    ht = new Hashtable();
                    ht.Add("@Type", "BillDeletion");
                    ht.Add("@Clientid", Client);
                    ht.Add("@month", monthval);
                    ht.Add("@BillType", BillType);
                    ht.Add("@BillNo", BillNo);
                    ht.Add("@Deleted_By", Username);
                    DataTable dtstatus = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;
                    if (dtstatus.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Bill deleted successfully');", true);
                        ddlClient.SelectedIndex = 0;
                        ddlBillType.SelectedIndex = 0;
                        ddlBillTypeVal.SelectedIndex = 0;
                        ddlBillNo.Items.Clear();
                        txtMonth.Text = txtClientID.Text = txtClientName.Text = txtMonthval.Text = txtBillNo.Text = lblmonth.Text = "";
                        return;
                    }

                }

            }
            catch (Exception ex)
            {

                return;
            }
        }

        protected void txtOldBillNo_TextChanged(object sender, EventArgs e)
        {
            if (txtOldBillNo.Text.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Bill No');", true);
                return;
            }


            string SPName = "BillModification";
            Hashtable ht = new Hashtable();
            ht.Add("@Type", "GetBillByBillNo");
            ht.Add("@BillNo", txtOldBillNo.Text.Trim());
            DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            if (dt.Rows.Count > 0)
            {
                txtClientIDV.Text = dt.Rows[0]["unitid"].ToString();
                txtClientNameV.Text = dt.Rows[0]["clientname"].ToString();
                txtmonthv.Text = dt.Rows[0]["Monthname"].ToString();
                ddlBillTypeV.SelectedValue = dt.Rows[0]["BillType"].ToString();
                lblmontv.Text = dt.Rows[0]["month"].ToString();
                txtNewBillSeq.Text = dt.Rows[0]["BillSeq"].ToString();
                txtNewBillNoV.Focus();
            }
        }


        protected void txtBillNo_TextChanged(object sender, EventArgs e)
        {
            if (txtBillNo.Text.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Bill No');", true);
                return;
            }


            string SPName = "BillModification";
            Hashtable ht = new Hashtable();
            ht.Add("@Type", "GetBillByBillNo");
            ht.Add("@BillNo", txtBillNo.Text.Trim());
            DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            if (dt.Rows.Count > 0)
            {
                txtClientID.Text = dt.Rows[0]["unitid"].ToString();
                txtClientName.Text = dt.Rows[0]["clientname"].ToString();
                txtMonthval.Text = dt.Rows[0]["Monthname"].ToString();
                ddlBillTypeVal.SelectedValue = dt.Rows[0]["BillType"].ToString();
                lblmonth.Text = dt.Rows[0]["month"].ToString();
            }
        }

        protected void rdbBillNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbBillNo.Checked)
            {
                PnlbyBillNo.Visible = true;
                PnlbyClient.Visible = false;
            }
            else
            {
                PnlbyClient.Visible = true;
                PnlbyBillNo.Visible = false;
            }
        }
        
        protected void txtMonth_TextChanged(object sender, EventArgs e)
        {
            ddlBillNo.Items.Clear();

            string Client = ddlClient.SelectedValue;
            string BillType = ddlBillType.SelectedValue;

            string date = string.Empty;
            string month = "";
            string Year = "";

            if (txtMonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                month = DateTime.Parse(date).Month.ToString();
                Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);
            }

            string monthval = month + Year;

            string SPName = "BillModification";
            Hashtable ht = new Hashtable();
            ht.Add("@Type", "GetBillDetails");
            ht.Add("@Clientid", Client);
            ht.Add("@month", monthval);
            ht.Add("@BillType", BillType);
            DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            if (dt.Rows.Count > 0)
            {
                ddlBillNo.DataTextField = "BillNo";
                ddlBillNo.DataValueField = "BillNo";
                ddlBillNo.DataSource = dt;
                ddlBillNo.DataBind();
            }

            ddlBillNo.Items.Insert(0, "-Select-");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string SPName = "";
            Hashtable ht = new Hashtable();
            DataTable dt;

            string Client = "";
            string BillType = "";
            string BillNo = "";
            string NewBillNo = "";
            string date = string.Empty;
            string month = "";
            string Year = "";
            SPName = "BillModification";
            ht = new Hashtable();
            ht.Add("@Type", "CheckPassword");
            ht.Add("@Password", hfPassword.Value);
            dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;


            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please check the password');", true);
                return;
            }

            else
            {

                string CheckBillno = txtNewBillSeq.Text.Trim() + txtNewBillNoV.Text.Trim();


                SPName = "BillModification";
                ht = new Hashtable();
                ht.Add("@Type", "CheckBillNo");
                ht.Add("@Billno", CheckBillno);
                dt = new DataTable();
                dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

                if (dt.Rows.Count > 0)
                {
                    string Clientname = dt.Rows[0]["ClientName"].ToString().Trim();
                    string monthName = dt.Rows[0]["Monthname"].ToString().Trim();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "key", string.Format("alert('Bill No already exists for client {0} , for month {1}');", Clientname, monthName), true);
                    return;

                }

                else
                {
                    if (txtmonthv.Text.Trim().Length > 0)
                    {
                        date = DateTime.Parse(txtmonthv.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                        month = DateTime.Parse(date).Month.ToString();
                        Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);
                    }

                    string monthval = month + Year;
                    BillNo = txtOldBillNo.Text;
                    Client = txtClientIDV.Text;
                    SPName = "BillModification";
                    ht = new Hashtable();
                    ht.Add("@Type", "updateBillno");
                    ht.Add("@Clientid", Client);
                    ht.Add("@month", monthval);
                    ht.Add("@NewBillo", CheckBillno);
                    //ht.Add("@BillType", BillType);
                    ht.Add("@OldBillno", BillNo);
                    ht.Add("@Deleted_By", Username);
                    DataTable dtstatus = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;
                    if (dtstatus.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Bill Updated successfully');", true);
                        ddlBillTypeV.SelectedIndex = 0;

                        txtNewBillNoV.Text = txtOldBillNo.Text = txtClientIDV.Text = txtClientNameV.Text = txtmonthv.Text = lblmontv.Text = txtNewBillSeq.Text = "";
                        return;
                    }
                }

            }
        }
    }
    
}