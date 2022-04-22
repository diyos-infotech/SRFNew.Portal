using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SRF.P.DAL;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using RestSharp;
using Newtonsoft.Json;
using QRCoder;

namespace SRF.P.Module_Clients
{
    public partial class ClientBilling : System.Web.UI.Page
    {

        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string EmpIDPrefix = ""; string Branch = "";
        string CmpIDPrefix = "";
        string Username = "";
        string FontStyle = "TimesNewroman";
        string BranchID = "";
        string Emp_id = "";
        DropDownList bind_dropdownlistHSN;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                GetWebConfigdata();
                if (!IsPostBack)
                {
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {
                        lblcname.Text = SqlHelper.Instance.GetCompanyname();

                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }

                    LoadClientList();
                    LoadClientNames();
                    LoadMonths();
                    getfont();
                    DefaultData();
                    ClearData();
                    displaydata();

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }

        }

        public void getfont()
        {
            int totalfonts = FontFactory.RegisterDirectory("c:\\WINDOWS\\fonts");
            StringBuilder sa = new StringBuilder();
            foreach (string fontname in FontFactory.RegisteredFonts)
            {
                sa.Append(fontname + "\n");
            }

        }

        protected void LoadMonths()
        {
            var formatInfoinfo = new DateTimeFormatInfo();
            string[] monthName = formatInfoinfo.MonthNames;
            string currentMonth = monthName[DateTime.Now.Month - 1];
            string month = "";
            string LastMonth = "";
            try
            {
                month = monthName[DateTime.Now.Month - 2];
            }
            catch (IndexOutOfRangeException ex)
            {
                month = monthName[12 - (2 - DateTime.Now.Month)];
            }
            try
            {
                LastMonth = monthName[DateTime.Now.Month - 3];
            }
            catch (IndexOutOfRangeException ex)
            {
                LastMonth = monthName[12 - (3 - DateTime.Now.Month)];
            }
            ddlmonth.Items.Add(currentMonth);
            ddlmonth.Items.Add(month);
            ddlmonth.Items.Add(LastMonth);

            //try
            //{
            //    LastMonth = monthName[DateTime.Now.Month - 4];
            //}
            //catch (IndexOutOfRangeException ex)
            //{
            //    LastMonth = monthName[12 - (4 - DateTime.Now.Month)];
            //}
            //ddlmonth.Items.Add(LastMonth);




            ddlmonth.Items.Insert(0, "-select-");
        }

        protected void LoadClientNames()
        {
            string selectclientid = "select clientid,clientname from clients where ClientStatus=1 and clientid like '%" + CmpIDPrefix + "%' Order By right(clientid,4) ";
            DataTable DtClientids = config.ExecuteReaderWithQueryAsync(selectclientid).Result;
            if (DtClientids.Rows.Count > 0)
            {
                ddlCname.DataValueField = "Clientid";
                ddlCname.DataTextField = "clientname";
                ddlCname.DataSource = DtClientids;
                ddlCname.DataBind();
            }
            ddlCname.Items.Insert(0, "-Select-");


        }

        protected void LoadClientList()
        {
            string selectclientid = "select clientid,clientname from clients where ClientStatus=1 and clientid like '%" + CmpIDPrefix + "%' Order By right(clientid,4) ";
            DataTable DtClientNames = config.ExecuteReaderWithQueryAsync(selectclientid).Result;
            if (DtClientNames.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "Clientid";
                ddlclientid.DataTextField = "Clientid";
                ddlclientid.DataSource = DtClientNames;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "-Select-");

        }

        protected void GetWebConfigdata()
        {
            if (Session.Keys.Count > 0)
            {

                EmpIDPrefix = Session["EmpIDPrefix"].ToString();
                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
                Username = Session["UserId"].ToString();
                Emp_id = Session["Emp_Id"].ToString();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        public void btnvisible()
        {

            string chkpdfsquery = "select pdfs from Contracts where clientid='" + ddlclientid.SelectedValue + "'";
            DataTable dtchkpdfs = config.ExecuteAdaptorAsyncWithQueryParams(chkpdfsquery).Result;
            string chkpdf = "False";
            if (dtchkpdfs.Rows.Count > 0)
                chkpdf = dtchkpdfs.Rows[0]["pdfs"].ToString();

            if (chkpdf == "True")
            {


            }
        }

        public void btnvisible_cname()
        {

            string chkpdfsquery = "select pdfs,c.ClientName from Contracts  ct inner join Clients c on ct.ClientId=c.ClientId where c.clientname='" + ddlCname.SelectedItem.Text + "'";
            DataTable dtchkpdfs = config.ExecuteAdaptorAsyncWithQueryParams(chkpdfsquery).Result;
            string chkpdf = "False";
            if (dtchkpdfs.Rows.Count > 0)
                chkpdf = dtchkpdfs.Rows[0]["pdfs"].ToString();

            if (chkpdf == "True")
            {

            }
        }

        protected void ClearData()
        {
            lblTotalResources.Text = "0";
            lblMachinery.Text = "0";
            lblMaterial.Text = "0";
            lblServiceCharges.Text = "0";
            lblServiceTax.Text = "0";
            lblGrandTotal.Text = "0";
            lblCESS.Text = "0";
            lblSheCESS.Text = "0";
            lblSBCESS.Text = "0";
            lblKKCESS.Text = "0";
            lblbillnolatest.Text = "";
            lblCGST.Text = "0";
            lblSGST.Text = "0";
            lblIGST.Text = "0";
            lblCess1.Text = "0";
            lblCess2.Text = "0";
            txtRoundoffamt.Text = "0";
        }

        protected void DisplayDataInGrid()
        {
            try
            {
                lblResult.Text = string.Empty;
                DataTable DtBilling = null;
                decimal TotalResourceCost = 0;
                string dutiestotalamount = "0";
                decimal MachineryCost = 0;
                decimal MaterialCost = 0;
                decimal ExtraOneAmt = 0;
                decimal ExtraTwoAmt = 0;
                decimal DisCountTwoAmt = 0;
                decimal ServiceCharge = 0;
                decimal ServiceTax = 0;
                decimal GrandTotal = 0;
                decimal sbCess = 0;
                decimal kkCess = 0;
                decimal CGST = 0;
                decimal SGST = 0;
                decimal IGST = 0;
                decimal Cess1 = 0;
                decimal Cess2 = 0;

                decimal Cess = 0;
                decimal Shecess = 0;
                decimal lessST75 = 0;
                decimal lessST25 = 0;
                decimal GRANDTOTAL = 0;
                bool ExtraDataSTCheck = false;
                decimal machineryCost = 0;
                decimal materialCost = 0;
                decimal maintenancecost = 0;
                decimal extraamountonecost = 0;
                decimal extraamoounttwocost = 0;
                decimal discountone = 0;
                decimal discounttwo = 0;
                decimal Staxonservicecharge = 0;
                decimal SCamtonMachinary = 0;
                decimal SCamtonMaintenance = 0;
                decimal SCamtonMaterial = 0;
                decimal SCamtonExtraone = 0;
                decimal SCamtonExtratwo = 0;
                decimal RelChrgAmt = 0;

                ClearExtraDataForBilling();

                ClearData();
                // int month = Timings.Instance.GetIdForSelectedMonth(ddlmonth.SelectedIndex);
                int month = GetMonthBasedOnSelectionDateorMonth();




                lbltotalamount.Text = "";
                DataTable Dtunit = null;
                gvClientBilling.DataSource = Dtunit;
                gvClientBilling.DataBind();

                DateTime LastDate = DateTime.Now;
                if (Chk_Month.Checked == false)
                {
                    LastDate = Timings.Instance.GetLastDayForSelectedMonth(ddlmonth.SelectedIndex);
                }
                if (Chk_Month.Checked == true)
                {
                    LastDate = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                }
                #region  Begin Code For Display Invoice Data Based On The ClientIdAndMonth as on [04-03-2014]
                var SPNameD = "GetInvoiceDataForDisplay";
                Hashtable HTDisplayForInvoice = new Hashtable();
                HTDisplayForInvoice.Add("@Clientid", ddlclientid.SelectedValue);
                HTDisplayForInvoice.Add("@Month", month);
                HTDisplayForInvoice.Add("@LastDay", LastDate);
                DataTable dtContracts = config.ExecuteAdaptorAsyncWithParams(SPNameD, HTDisplayForInvoice).Result;
                if (dtContracts.Rows.Count <= 0)
                {
                    btnFreeze.Visible = false;
                    btnUnFreeze.Visible = false;
                    lblbillnolatest.Text = "";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' Data Not Avaialable For This Month ');", true);
                    lblResult.Text = "";

                    return;
                }
                else
                {
                    string monthreports = "";


                    string paymentType = dtContracts.Rows[0]["Paymenttype"].ToString();
                    double DIncludeST = double.Parse(dtContracts.Rows[0]["servicetax"].ToString());
                    double DstrST75 = double.Parse(dtContracts.Rows[0]["ServiceTax75"].ToString());
                    bool bIncludeST = false;
                    if (DIncludeST == 0)
                    {
                        bIncludeST = true;
                    }
                    bool bST75 = false;
                    if (DstrST75 > 0)
                    {
                        bST75 = true;
                    }

                    string CCGST = dtContracts.Rows[0]["CCGST"].ToString();
                    string CSGST = dtContracts.Rows[0]["CSGST"].ToString();
                    string CIGST = dtContracts.Rows[0]["CIGST"].ToString();
                    string CCess1 = dtContracts.Rows[0]["CCess1"].ToString();
                    string CCess2 = dtContracts.Rows[0]["CCess2"].ToString();

                    lblbillnolatest.Text = dtContracts.Rows[0]["billno"].ToString();
                    txtfromdate.Text = DateTime.Parse(dtContracts.Rows[0]["fromdt"].ToString()).ToString("dd/MM/yyyy");
                    txttodate.Text = DateTime.Parse(dtContracts.Rows[0]["todt"].ToString()).ToString("dd/MM/yyyy");


                    txtbilldate.Text = DateTime.Parse(dtContracts.Rows[0]["billdt"].ToString()).ToString("dd/MM/yyyy");
                    txtduedate.Text = DateTime.Parse(dtContracts.Rows[0]["Duedt"].ToString()).ToString("dd/MM/yyyy");



                    bool Extradatacheck = false;

                    #region New code for extradata for billing titles as on 10/03/2014 by venkat

                    string Machinarycosttitle = "";
                    string Materialcosttitle = "";
                    string Maintanancecosttitle = "";
                    string Extraonetitle = "";
                    string Extratwotitle = "";
                    string Discountonetitle = "";
                    string Discounttwotitle = "";

                    #endregion

                    bool STMachinary = false;
                    bool STMaterial = false;
                    bool STMaintenance = false;
                    bool STExtraone = false;
                    bool STExtratwo = false;

                    bool SCMachinary = false;
                    bool SCMaterial = false;
                    bool SCMaintenance = false;
                    bool SCExtraone = false;
                    bool SCExtratwo = false;

                    bool STDiscountone = false;
                    bool STDiscounttwo = false;


                    if (paymentType == "False")
                    {

                        #region  Begin Man Power Part

                        #region Begin Code For Retrive Data From UnitBillBreakup As on [02-03-2014]

                        #region  Begin  Variable Declaration
                        var UBBSPName = "";
                        var UBBClientId = "";
                        var UBBMonth = 0;
                        Hashtable HtUBB = new Hashtable();
                        #endregion End Variable Declaration

                        #region  Begin Assign Values To the Variable
                        UBBSPName = "GetUnitbillbreakupdataBasedonClientdAndMonth";
                        UBBClientId = ddlclientid.SelectedValue;
                        UBBMonth = month;
                        #endregion End Assign Values To the Variable


                        #region Begin Calling Stored Procedure
                        HtUBB.Add("@clientid", UBBClientId);
                        HtUBB.Add("@month", month);
                        Dtunit = config.ExecuteAdaptorAsyncWithParams(UBBSPName, HtUBB).Result;
                        #endregion End Calling Stored Procedure

                        #endregion End Code For Retrive Data From UnitBillBreakup As on [02-03-2014]


                        if (Dtunit.Rows.Count > 0)
                        {
                            gvClientBilling.DataSource = Dtunit;
                            gvClientBilling.DataBind();

                            displaydata();

                            for (int i = 0; i < gvClientBilling.Rows.Count; i++)
                            {

                                Label lblextra = gvClientBilling.Rows[i].FindControl("lblextra") as Label;
                                CheckBox chkExtra = gvClientBilling.Rows[i].FindControl("chkExtra") as CheckBox;
                                Label lblSno = gvClientBilling.Rows[i].FindControl("lblSno") as Label;
                                TextBox lbldesgn = gvClientBilling.Rows[i].FindControl("lbldesgn") as TextBox;
                                DropDownList ddlHSNNumber = gvClientBilling.Rows[i].FindControl("ddlHSNNumber") as DropDownList;
                                TextBox lblnoofemployees = gvClientBilling.Rows[i].FindControl("lblnoofemployees") as TextBox;
                                TextBox lblNoOfDuties = gvClientBilling.Rows[i].FindControl("lblNoOfDuties") as TextBox;
                                TextBox lblpayrate = gvClientBilling.Rows[i].FindControl("lblpayrate") as TextBox;
                                TextBox lblSchrgPrc = gvClientBilling.Rows[i].FindControl("lblSchrgPrc") as TextBox;
                                TextBox lblSchrgAmt = gvClientBilling.Rows[i].FindControl("lblSchrgAmt") as TextBox;
                                TextBox lblda = gvClientBilling.Rows[i].FindControl("lblda") as TextBox;
                                TextBox lblAmount = gvClientBilling.Rows[i].FindControl("lblAmount") as TextBox;
                                DropDownList ddlnoofdays = gvClientBilling.Rows[i].FindControl("ddlnod") as DropDownList;
                                TextBox lblGSTper = gvClientBilling.Rows[i].FindControl("lblGSTper") as TextBox;
                                TextBox lblCGSTAmount = gvClientBilling.Rows[i].FindControl("lblCGSTAmount") as TextBox;
                                TextBox lblSGSTAmount = gvClientBilling.Rows[i].FindControl("lblSGSTAmount") as TextBox;
                                TextBox lblIGSTAmount = gvClientBilling.Rows[i].FindControl("lblIGSTAmount") as TextBox;
                                TextBox lblTotalTaxmount = gvClientBilling.Rows[i].FindControl("lblTotalTaxmount") as TextBox;
                                DropDownList ddlCalnType = gvClientBilling.Rows[i].FindControl("ddlCalnType") as DropDownList;


                                if (lblextra.Text.Contains("E"))
                                {
                                    chkExtra.Checked = true;
                                    lblSno.Enabled = true;
                                    lbldesgn.Enabled = true;
                                    lblnoofemployees.Enabled = true;
                                    lblNoOfDuties.Enabled = true;
                                    lblpayrate.Enabled = true;
                                    lblSchrgPrc.Enabled = true;
                                    lblSchrgAmt.Enabled = true;
                                    lblda.Enabled = true;
                                    lblAmount.Enabled = true;
                                    ddlnoofdays.Enabled = true;
                                    ddlHSNNumber.Enabled = true;
                                    lblGSTper.Enabled = true;
                                    lblCGSTAmount.Enabled = true;
                                    lblSGSTAmount.Enabled = true;
                                    lblIGSTAmount.Enabled = true;
                                    lblTotalTaxmount.Enabled = true;
                                    ddlCalnType.Enabled = true;
                                }
                                else
                                {
                                    chkExtra.Checked = false;
                                    lblSno.Enabled = false;
                                    lbldesgn.Enabled = false;
                                    lblnoofemployees.Enabled = false;
                                    lblNoOfDuties.Enabled = false;
                                    lblpayrate.Enabled = false;
                                    lblSchrgPrc.Enabled = false;
                                    lblSchrgAmt.Enabled = false;
                                    lblda.Enabled = false;
                                    lblAmount.Enabled = false;
                                    ddlnoofdays.Enabled = false;
                                    ddlHSNNumber.Enabled = false;
                                    lblGSTper.Enabled = false;
                                    lblCGSTAmount.Enabled = false;
                                    lblSGSTAmount.Enabled = false;
                                    lblIGSTAmount.Enabled = false;
                                    lblTotalTaxmount.Enabled = false;
                                    ddlCalnType.Enabled = false;

                                }
                            }

                        }
                        else
                        {
                            //LblResult.Text = "There Is No Bills  For The Selected Client";
                            return;
                        }
                        double totalamount = 0;
                        for (int index = 0; index < Dtunit.Rows.Count; index++)
                        {
                            totalamount = totalamount + double.Parse(Dtunit.Rows[index]["PayRate"].ToString());

                            decimal DutyHrs = decimal.Parse(Dtunit.Rows[index]["DutyHours"].ToString());
                            decimal noofems = decimal.Parse(Dtunit.Rows[index]["noofemps"].ToString());
                            decimal payrate = decimal.Parse(Dtunit.Rows[index]["payrate"].ToString());
                            decimal basic = decimal.Parse(Dtunit.Rows[index]["BasicDA"].ToString());
                            decimal OTAmount = 0;
                            if (String.IsNullOrEmpty(Dtunit.Rows[index]["otamount"].ToString()) == false)
                            {
                                OTAmount = decimal.Parse(Dtunit.Rows[index]["otamount"].ToString());
                            }

                            DropDownList ddlHSNNumber = gvClientBilling.Rows[index].FindControl("ddlHSNNumber") as DropDownList;
                            if (ddlHSNNumber != null)
                            {
                                ddlHSNNumber.SelectedValue = Dtunit.Rows[index]["HSNNumber"].ToString();

                            }

                            decimal amount = basic /*+ hra + Conveyance + WashAllowance + OtherAllowance + pf + esi*/ + OTAmount;
                            Label lblOt = gvClientBilling.Rows[index].FindControl("lblOtAmount") as Label;
                            TextBox totAmount = gvClientBilling.Rows[index].FindControl("lblamount") as TextBox;
                            TextBox PayRateWithType = gvClientBilling.Rows[index].FindControl("lblpayrate") as TextBox;
                            PayRateWithType.Text = payrate.ToString("0.00");
                            lblOt.Text = OTAmount.ToString("0.00");
                            totAmount.Text = (amount).ToString("0.00");

                            DropDownList ddlCalnType = gvClientBilling.Rows[index].FindControl("ddlCalnType") as DropDownList;
                            if (ddlCalnType != null)
                            {
                                ddlCalnType.SelectedValue = Dtunit.Rows[index]["CalnType"].ToString();

                            }


                        }

                        #endregion End Man Power Part

                    }
                    else
                    {

                        #region Begin  Lampsum Part

                        if (dtContracts.Rows.Count > 0)
                        {
                            DataTable tempTable = new DataTable();
                            DataColumn col1 = new DataColumn();
                            col1.DataType = System.Type.GetType("System.String");
                            col1.AllowDBNull = true;
                            col1.Caption = "UnitId";
                            col1.ColumnName = "UnitId";
                            tempTable.Columns.Add(col1);
                            DataColumn col2 = new DataColumn();
                            col2.DataType = System.Type.GetType("System.String");
                            col2.AllowDBNull = true;
                            col2.Caption = "Designation";
                            col2.ColumnName = "Designation";
                            tempTable.Columns.Add(col2);

                            DataColumn col12 = new DataColumn();
                            col12.DataType = System.Type.GetType("System.String");
                            col12.AllowDBNull = true;
                            col12.Caption = "Designid";
                            col12.ColumnName = "Designid";
                            tempTable.Columns.Add(col12);

                            DataColumn col112 = new DataColumn();
                            col112.DataType = System.Type.GetType("System.String");
                            col112.AllowDBNull = true;
                            col112.Caption = "type";
                            col112.ColumnName = "type";
                            tempTable.Columns.Add(col112);



                            DataColumn col1121ss = new DataColumn();
                            col1121ss.DataType = System.Type.GetType("System.String");
                            col1121ss.AllowDBNull = true;
                            col1121ss.Caption = "Branch";
                            col1121ss.ColumnName = "Branch";
                            tempTable.Columns.Add(col1121ss);


                            DataColumn col1121 = new DataColumn();
                            col1121.DataType = System.Type.GetType("System.String");
                            col1121.AllowDBNull = true;
                            col1121.Caption = "noofdays";
                            col1121.ColumnName = "noofdays";
                            tempTable.Columns.Add(col1121);

                            DataColumn col3 = new DataColumn();
                            col3.DataType = System.Type.GetType("System.String");
                            col3.AllowDBNull = true;
                            col3.Caption = "BasicDA";
                            col3.ColumnName = "BasicDA";
                            tempTable.Columns.Add(col3);
                            DataColumn col4 = new DataColumn();
                            col4.DataType = System.Type.GetType("System.String");
                            col4.AllowDBNull = true;
                            col4.Caption = "NoofEmps";
                            col4.ColumnName = "NoofEmps";
                            tempTable.Columns.Add(col4);
                            DataColumn col5 = new DataColumn();
                            col5.DataType = System.Type.GetType("System.String");
                            col5.AllowDBNull = true;
                            col5.Caption = "PayRate";
                            col5.ColumnName = "PayRate";
                            tempTable.Columns.Add(col5);

                            DataColumn col51 = new DataColumn();
                            col51.DataType = System.Type.GetType("System.String");
                            col51.AllowDBNull = true;
                            col51.Caption = "newPayRate";
                            col51.ColumnName = "newPayRate";
                            tempTable.Columns.Add(col51);
                            DataColumn col6 = new DataColumn();
                            col6.DataType = System.Type.GetType("System.String");
                            col6.AllowDBNull = true;
                            col6.Caption = "DutyHours";
                            col6.ColumnName = "DutyHours";
                            tempTable.Columns.Add(col6);
                            DataColumn col7 = new DataColumn();
                            col7.DataType = System.Type.GetType("System.String");
                            col7.AllowDBNull = true;
                            col7.Caption = "OT Amount";
                            col7.ColumnName = "otamount";
                            tempTable.Columns.Add(col7);


                            DataColumn col8 = new DataColumn();
                            col8.DataType = System.Type.GetType("System.String");
                            col8.AllowDBNull = true;
                            col8.Caption = "DutyHrs";
                            col8.ColumnName = "DutyHrs";
                            tempTable.Columns.Add(col8);

                            DataColumn col824 = new DataColumn();
                            col824.DataType = System.Type.GetType("System.String");
                            col824.AllowDBNull = true;
                            col824.Caption = "CGSTAmt";
                            col824.ColumnName = "CGSTAmt";
                            tempTable.Columns.Add(col824);


                            DataColumn col825 = new DataColumn();
                            col825.DataType = System.Type.GetType("System.String");
                            col825.AllowDBNull = true;
                            col825.Caption = "CGSTPrc";
                            col825.ColumnName = "CGSTPrc";
                            tempTable.Columns.Add(col825);


                            DataColumn col826 = new DataColumn();
                            col826.DataType = System.Type.GetType("System.String");
                            col826.AllowDBNull = true;
                            col826.Caption = "SGSTAmt";
                            col826.ColumnName = "SGSTAmt";
                            tempTable.Columns.Add(col826);


                            DataColumn col827 = new DataColumn();
                            col827.DataType = System.Type.GetType("System.String");
                            col827.AllowDBNull = true;
                            col827.Caption = "SGSTPrc";
                            col827.ColumnName = "SGSTPrc";
                            tempTable.Columns.Add(col827);


                            DataColumn col828 = new DataColumn();
                            col828.DataType = System.Type.GetType("System.String");
                            col828.AllowDBNull = true;
                            col828.Caption = "IGSTAmt";
                            col828.ColumnName = "IGSTAmt";
                            tempTable.Columns.Add(col828);


                            DataColumn col829 = new DataColumn();
                            col829.DataType = System.Type.GetType("System.String");
                            col829.AllowDBNull = true;
                            col829.Caption = "IGSTPrc";
                            col829.ColumnName = "IGSTPrc";
                            tempTable.Columns.Add(col829);

                            DataColumn col830 = new DataColumn();
                            col830.DataType = System.Type.GetType("System.String");
                            col830.AllowDBNull = true;
                            col830.Caption = "TotalTaxAmount";
                            col830.ColumnName = "TotalTaxAmount";
                            tempTable.Columns.Add(col830);

                            DataColumn col831 = new DataColumn();
                            col831.DataType = System.Type.GetType("System.String");
                            col831.AllowDBNull = true;
                            col831.Caption = "HSNNumber";
                            col831.ColumnName = "HSNNumber";
                            tempTable.Columns.Add(col831);



                            DataColumn col832 = new DataColumn();
                            col832.DataType = System.Type.GetType("System.String");
                            col832.AllowDBNull = true;
                            col832.Caption = "GSTper";
                            col832.ColumnName = "GSTper";
                            tempTable.Columns.Add(col832);


                            DataColumn col833 = new DataColumn();
                            col833.DataType = System.Type.GetType("System.String");
                            col833.AllowDBNull = true;
                            col833.Caption = "UOM";
                            col833.ColumnName = "UOM";
                            tempTable.Columns.Add(col833);

                            DataColumn col834 = new DataColumn();
                            col834.DataType = System.Type.GetType("System.String");
                            col834.AllowDBNull = true;
                            col834.Caption = "ServiceChargesPrc";
                            col834.ColumnName = "ServiceChargesPrc";
                            tempTable.Columns.Add(col834);

                            DataColumn col835 = new DataColumn();
                            col835.DataType = System.Type.GetType("System.String");
                            col835.AllowDBNull = true;
                            col835.Caption = "ServiceCharges";
                            col835.ColumnName = "ServiceCharges";
                            tempTable.Columns.Add(col835);


                            string strQry = "Select Designations from ContractDetails where ClientID='" + ddlclientid.SelectedValue + "'";
                            string lumquery = "Select lumpsumtext from Contracts  where ClientID='" + ddlclientid.SelectedValue + "'";
                            DataTable desigTable = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
                            DataTable designTable = config.ExecuteAdaptorAsyncWithQueryParams(lumquery).Result;
                            string designation = "Lumpsum";
                            if (desigTable.Rows.Count > 0)
                            {
                                designation = desigTable.Rows[0]["Designations"].ToString();
                            }
                            DataRow row = tempTable.NewRow();
                            row["UnitId"] = ddlclientid.SelectedValue;
                            row["Designation"] = designTable.Rows[0]["lumpsumtext"].ToString();
                            row["Branch"] = "0";
                            row["BasicDA"] = "0";
                            row["NoofEmps"] = "0";
                            row["DutyHrs"] = "0";
                            row["PayRate"] = "0";
                            row["DutyHours"] = "";
                            row["otamount"] = "0";
                            row["newpayrate"] = "0";
                            row["ServiceChargesPrc"] = "0";
                            row["ServiceCharges"] = "0";

                            tempTable.Rows.Add(row);

                            gvClientBilling.DataSource = tempTable;
                            gvClientBilling.DataBind();

                            displaydata();

                            if (gvClientBilling.Rows.Count > 0)
                            {
                                TextBox totAmount = gvClientBilling.Rows[0].FindControl("lblAmount") as TextBox;
                                TextBox lblpayrate = gvClientBilling.Rows[0].FindControl("lblpayrate") as TextBox;

                                decimal lumpsumAmount = 0;
                                if (dtContracts.Rows[0]["TotalChrg"].ToString().Trim().Length > 0)
                                    lumpsumAmount = Convert.ToDecimal(dtContracts.Rows[0]["TotalChrg"].ToString().Trim());
                                totAmount.Text = dtContracts.Rows[0]["TotalChrg"].ToString();
                                TotalResourceCost = lumpsumAmount;
                                lblpayrate.Text = dtContracts.Rows[0]["TotalChrg"].ToString();
                            }
                        }
                        else
                        {
                            gvClientBilling.DataSource = null;
                            gvClientBilling.DataBind();
                        }

                        #endregion End Lumsum Part
                    }

                    #region Begin Extra Data For Billing

                    if (dtContracts.Rows.Count > 0)
                    {
                        string strServCharge = dtContracts.Rows[0]["ServiceChrg"].ToString();
                        string strServTax = dtContracts.Rows[0]["ServiceTax"].ToString();
                        string strsbCess = dtContracts.Rows[0]["SBCESSAmt"].ToString();
                        string strkkCess = dtContracts.Rows[0]["KKCESSAmt"].ToString();
                        string strCess = dtContracts.Rows[0]["CESS"].ToString();
                        string strSheCess = dtContracts.Rows[0]["SHECess"].ToString();
                        string strRelChrgAmt = dtContracts.Rows[0]["RelChrgAmt"].ToString();


                        string strCGST = dtContracts.Rows[0]["CGSTAmt"].ToString();
                        string strSGST = dtContracts.Rows[0]["SGSTAmt"].ToString();
                        string strIGST = dtContracts.Rows[0]["IGSTAmt"].ToString();
                        string strCess1 = dtContracts.Rows[0]["Cess1Amt"].ToString();
                        string strCess2 = dtContracts.Rows[0]["Cess2Amt"].ToString();
                        string strCGSTPrc = dtContracts.Rows[0]["CGSTPrc"].ToString();
                        string strSGSTPrc = dtContracts.Rows[0]["SGSTPrc"].ToString();
                        string strIGSTPrc = dtContracts.Rows[0]["IGSTPrc"].ToString();


                        dutiestotalamount = dtContracts.Rows[0]["dutiestotalamount"].ToString();
                        machineryCost = decimal.Parse(dtContracts.Rows[0]["MachinaryCost"].ToString());
                        materialCost = decimal.Parse(dtContracts.Rows[0]["MaterialCost"].ToString());
                        maintenancecost = decimal.Parse(dtContracts.Rows[0]["ElectricalChrg"].ToString());
                        extraamountonecost = decimal.Parse(dtContracts.Rows[0]["ExtraAmtone"].ToString());
                        extraamoounttwocost = decimal.Parse(dtContracts.Rows[0]["ExtraAmtTwo"].ToString());
                        discountone = decimal.Parse(dtContracts.Rows[0]["Discount"].ToString());
                        discounttwo = decimal.Parse(dtContracts.Rows[0]["Discounttwo"].ToString());

                        txtRemarks.Text = dtContracts.Rows[0]["Remarks"].ToString();
                        txtmachinarycost.Text = dtContracts.Rows[0]["Machinarycosttitle"].ToString();
                        txtMaterialcost.Text = dtContracts.Rows[0]["Materialcosttitle"].ToString();
                        txtMaintanancecost.Text = dtContracts.Rows[0]["Maintanancecosttitle"].ToString();
                        txtextraonetitle.Text = dtContracts.Rows[0]["Extraonetitle"].ToString();
                        txtextratwotitle.Text = dtContracts.Rows[0]["Extratwotitle"].ToString();
                        txtdiscount.Text = dtContracts.Rows[0]["Discountonetitle"].ToString();
                        txtdiscounttwotitle.Text = dtContracts.Rows[0]["Discounttwotitle"].ToString();


                        #region New code for extradata for billing titles as on 20/01/2014 by venkat

                        Machinarycosttitle = dtContracts.Rows[0]["Machinarycosttitle"].ToString();
                        Materialcosttitle = dtContracts.Rows[0]["Materialcosttitle"].ToString();
                        Maintanancecosttitle = dtContracts.Rows[0]["Maintanancecosttitle"].ToString();
                        Extraonetitle = dtContracts.Rows[0]["Extraonetitle"].ToString();
                        Extratwotitle = dtContracts.Rows[0]["Extratwotitle"].ToString();
                        Discountonetitle = dtContracts.Rows[0]["Discountonetitle"].ToString();
                        Discounttwotitle = dtContracts.Rows[0]["Discounttwotitle"].ToString();

                        #endregion

                        #region New code for extradata  as on 27/01/2014 by venkat

                        if (String.IsNullOrEmpty(dtContracts.Rows[0]["Extradatacheck"].ToString()) == false)
                        {
                            Extradatacheck = Boolean.Parse(dtContracts.Rows[0]["Extradatacheck"].ToString());
                            if (Extradatacheck == true)
                            {
                                checkExtraData.Checked = true;
                            }
                            else
                            {
                                checkExtraData.Checked = false;
                            }
                        }


                        if (checkExtraData.Checked == true)
                        {
                            if (Chk_Month.Checked == false)
                            {
                                if (ddlclientid.SelectedIndex > 0 && ddlmonth.SelectedIndex > 0)
                                {
                                    panelRemarks.Visible = true;
                                }
                            }
                            if (Chk_Month.Checked == true)
                            {
                                if (ddlclientid.SelectedIndex > 0 && txtmonth.Text.Trim().Length > 0)
                                {
                                    panelRemarks.Visible = true;
                                }
                            }
                        }

                        #endregion

                        #region Begin New Code for service tax extradata  as 01/04/2014 by venkat

                        if (String.IsNullOrEmpty(dtContracts.Rows[0]["STMachinary"].ToString()) == false)
                        {
                            STMachinary = Boolean.Parse(dtContracts.Rows[0]["STMachinary"].ToString());
                            if (STMachinary == true)
                            {
                                chkSTYesMachinary.Checked = true;
                            }
                            else
                            {
                                chkSTYesMachinary.Checked = false;
                            }
                        }

                        if (String.IsNullOrEmpty(dtContracts.Rows[0]["STMaterial"].ToString()) == false)
                        {
                            STMaterial = Boolean.Parse(dtContracts.Rows[0]["STMaterial"].ToString());
                            if (STMaterial == true)
                            {
                                chkSTYesMaterial.Checked = true;
                            }
                            else
                            {
                                chkSTYesMaterial.Checked = false;
                            }
                        }

                        if (String.IsNullOrEmpty(dtContracts.Rows[0]["STMaintenance"].ToString()) == false)
                        {
                            STMaintenance = Boolean.Parse(dtContracts.Rows[0]["STMaintenance"].ToString());
                            if (STMaintenance == true)
                            {
                                chkSTYesElectrical.Checked = true;
                            }
                            else
                            {
                                chkSTYesElectrical.Checked = false;
                            }
                        }

                        if (String.IsNullOrEmpty(dtContracts.Rows[0]["STExtraone"].ToString()) == false)
                        {
                            STExtraone = Boolean.Parse(dtContracts.Rows[0]["STExtraone"].ToString());
                            if (STExtraone == true)
                            {
                                chkSTYesExtraone.Checked = true;
                            }
                            else
                            {
                                chkSTYesExtraone.Checked = false;
                            }
                        }

                        if (String.IsNullOrEmpty(dtContracts.Rows[0]["STExtratwo"].ToString()) == false)
                        {
                            STExtratwo = Boolean.Parse(dtContracts.Rows[0]["STExtratwo"].ToString());
                            if (STExtratwo == true)
                            {
                                chkSTYesExtratwo.Checked = true;
                            }
                            else
                            {
                                chkSTYesExtratwo.Checked = false;
                            }
                        }


                        #endregion

                        #region Begin New Code for service Charge on extradata  as 01/04/2014 by venkat

                        if (String.IsNullOrEmpty(dtContracts.Rows[0]["SCMachinary"].ToString()) == false)
                        {
                            SCMachinary = Boolean.Parse(dtContracts.Rows[0]["SCMachinary"].ToString());
                            if (SCMachinary == true)
                            {
                                chkSCYesMachinary.Checked = true;
                            }
                            else
                            {
                                chkSCYesMachinary.Checked = false;
                            }
                        }

                        if (String.IsNullOrEmpty(dtContracts.Rows[0]["SCMaterial"].ToString()) == false)
                        {
                            SCMaterial = Boolean.Parse(dtContracts.Rows[0]["SCMaterial"].ToString());
                            if (SCMaterial == true)
                            {
                                chkSCYesMaterial.Checked = true;
                            }
                            else
                            {
                                chkSCYesMaterial.Checked = false;
                            }
                        }

                        if (String.IsNullOrEmpty(dtContracts.Rows[0]["SCMaintenance"].ToString()) == false)
                        {
                            SCMaintenance = Boolean.Parse(dtContracts.Rows[0]["SCMaintenance"].ToString());
                            if (SCMaintenance == true)
                            {
                                chkSCYesElectrical.Checked = true;
                            }
                            else
                            {
                                chkSCYesElectrical.Checked = false;
                            }
                        }

                        if (String.IsNullOrEmpty(dtContracts.Rows[0]["SCExtraone"].ToString()) == false)
                        {
                            SCExtraone = Boolean.Parse(dtContracts.Rows[0]["SCExtraone"].ToString());
                            if (SCExtraone == true)
                            {
                                chkSCYesExtraone.Checked = true;
                            }
                            else
                            {
                                chkSCYesExtraone.Checked = false;
                            }
                        }

                        if (String.IsNullOrEmpty(dtContracts.Rows[0]["SCExtratwo"].ToString()) == false)
                        {
                            SCExtratwo = Boolean.Parse(dtContracts.Rows[0]["SCExtratwo"].ToString());
                            if (SCExtratwo == true)
                            {
                                chkSCYesExtratwo.Checked = true;
                            }
                            else
                            {
                                chkSCYesExtratwo.Checked = false;
                            }
                        }

                        if (String.IsNullOrEmpty(dtContracts.Rows[0]["STDiscountone"].ToString()) == false)
                        {
                            STDiscountone = Boolean.Parse(dtContracts.Rows[0]["STDiscountone"].ToString());
                            if (STDiscountone == true)
                            {
                                chkSTDiscountone.Checked = true;
                            }
                            else
                            {
                                chkSTDiscountone.Checked = false;
                            }
                        }

                        if (String.IsNullOrEmpty(dtContracts.Rows[0]["STDiscounttwo"].ToString()) == false)
                        {
                            STDiscounttwo = Boolean.Parse(dtContracts.Rows[0]["STDiscounttwo"].ToString());
                            if (STDiscounttwo == true)
                            {
                                chkSTDiscounttwo.Checked = true;
                            }
                            else
                            {
                                chkSTDiscounttwo.Checked = false;
                            }
                        }


                        #endregion

                        #region Begin New Code for Service tax amount on Extradata as on 01/04/2014 by venkat

                        Staxonservicecharge = decimal.Parse(dtContracts.Rows[0]["Staxonservicecharge"].ToString());
                        SCamtonMachinary = decimal.Parse(dtContracts.Rows[0]["SCamtonMachinary"].ToString());
                        SCamtonMaintenance = decimal.Parse(dtContracts.Rows[0]["SCamtonMaintenance"].ToString());
                        SCamtonMaterial = decimal.Parse(dtContracts.Rows[0]["SCamtonMaterial"].ToString());
                        SCamtonExtraone = decimal.Parse(dtContracts.Rows[0]["SCamtonExtraone"].ToString());
                        SCamtonExtratwo = decimal.Parse(dtContracts.Rows[0]["SCamtonExtratwo"].ToString());

                        #endregion


                        GRANDTOTAL = decimal.Parse(dtContracts.Rows[0]["GrandTotal"].ToString());
                        /* End New code As on [01-07-2013]   */


                        txtMachinery.Text = machineryCost.ToString();
                        txtMaterial.Text = materialCost.ToString();
                        txtElectical.Text = maintenancecost.ToString();
                        txtextraonevalue.Text = extraamountonecost.ToString();
                        txtextratwovalue.Text = extraamoounttwocost.ToString();
                        txtDiscounts.Text = discountone.ToString();
                        txtdiscounttwovalue.Text = discounttwo.ToString();

                        lblRemarks.Text = txtRemarks.Text;

                        if (strRelChrgAmt.Trim().Length > 0)
                        {
                            RelChrgAmt = Convert.ToDecimal(strRelChrgAmt);
                            lblRelChrgAmt.Text = RelChrgAmt.ToString("0.00");
                        }


                        if (strServCharge.Trim().Length > 0)
                        {
                            ServiceCharge = Convert.ToDecimal(strServCharge);
                            lblServiceCharges.Text = ServiceCharge.ToString("0.00");
                        }
                        if (strServTax.Trim().Length > 0)
                        {
                            ServiceTax = Convert.ToDecimal(strServTax);
                            lblServiceTax.Text = ServiceTax.ToString("0.00");
                        }
                        if (strsbCess.Trim().Length > 0)
                        {
                            sbCess = Convert.ToDecimal(strsbCess);
                            lblSBCESS.Text = sbCess.ToString("0.00");
                        }

                        if (strkkCess.Trim().Length > 0)
                        {
                            kkCess = Convert.ToDecimal(strkkCess);
                            lblKKCESS.Text = kkCess.ToString("0.00");
                        }


                        #region for GST on 17-6-2017 by swathi

                        if (strCGST.Trim().Length > 0)
                        {
                            CGST = Convert.ToDecimal(strCGST);
                            lblCGST.Text = CGST.ToString("0.00");
                            TxtCGSTPrc.Text = strCGSTPrc.ToString();
                        }


                        if (strSGST.Trim().Length > 0)
                        {
                            SGST = Convert.ToDecimal(strSGST);
                            lblSGST.Text = SGST.ToString("0.00");
                            TxtSGSTPrc.Text = strSGSTPrc.ToString();
                        }


                        if (strIGST.Trim().Length > 0)
                        {
                            IGST = Convert.ToDecimal(strIGST);
                            lblIGST.Text = IGST.ToString("0.00");
                            TxtIGSTPrc.Text = strIGSTPrc.ToString();
                        }

                        if (strCess1.Trim().Length > 0)
                        {
                            Cess1 = Convert.ToDecimal(strCess1);
                            lblCess1.Text = Cess1.ToString("0.00");
                        }

                        if (strCess2.Trim().Length > 0)
                        {
                            Cess2 = Convert.ToDecimal(strCess2);
                            lblCess2.Text = Cess2.ToString("0.00");
                        }

                        #endregion for GST on 17-6-2017

                        if (strCess.Trim().Length > 0)
                        {
                            Cess = Convert.ToDecimal(strCess);
                            lblCESS.Text = Cess.ToString("0.00");
                        }
                        if (strSheCess.Trim().Length > 0)
                        {
                            Shecess = Convert.ToDecimal(strSheCess);
                            lblSheCESS.Text = Shecess.ToString("0.00");
                        }
                        if (bIncludeST)
                        {
                            lblServiceTaxTitle.Visible = false;
                            lblServiceTax.Visible = false;
                            lblCESS.Visible = false;
                            lblCESSTitle.Visible = false;
                            lblSheCESS.Visible = false;
                            lblSheCESSTitle.Visible = false;
                            lblSBCESS.Visible = false;
                            lblSBCESSTitle.Visible = false;
                            lblKKCESS.Visible = false;
                            lblKKCESSTitle.Visible = false;
                        }
                        else
                        {
                            lblServiceTaxTitle.Visible = true;
                            lblServiceTax.Visible = true;
                            lblCESS.Visible = true;
                            lblCESSTitle.Visible = true;
                            lblSheCESS.Visible = true;
                            lblSheCESSTitle.Visible = true;
                            lblSBCESS.Visible = true;
                            lblSBCESSTitle.Visible = true;
                            lblKKCESS.Visible = true;
                            lblKKCESSTitle.Visible = true;
                        }
                        #region for GST  on 17-6-2017 by swathi

                        if (CCGST == "True")
                        {
                            lblCGST.Visible = true;
                            TxtCGSTPrc.Visible = true;
                            lblCGSTTitle.Visible = true;
                        }
                        else
                        {
                            lblCGST.Visible = false;
                            TxtCGSTPrc.Visible = false;
                            lblCGSTTitle.Visible = false;
                        }

                        if (CSGST == "True")
                        {
                            lblSGST.Visible = true;
                            TxtSGSTPrc.Visible = true;
                            lblSGSTTitle.Visible = true;
                        }
                        else
                        {
                            lblSGST.Visible = false;
                            TxtSGSTPrc.Visible = false;
                            lblSGSTTitle.Visible = false;
                        }

                        if (CIGST == "True")
                        {
                            lblIGST.Visible = true;
                            TxtIGSTPrc.Visible = true;
                            lblIGSTTitle.Visible = true;
                        }
                        else
                        {
                            lblIGST.Visible = false;
                            TxtIGSTPrc.Visible = false;
                            lblIGSTTitle.Visible = false;
                        }


                        if (CCess1 == "True")
                        {
                            lblCess1.Visible = true;
                            TxtCess1Prc.Visible = false;
                            lblCess1Title.Visible = true;
                        }
                        else
                        {
                            lblCess1.Visible = false;
                            TxtCess1Prc.Visible = false;
                            lblCess1Title.Visible = false;
                        }

                        if (CCess2 == "True")
                        {
                            lblCess2.Visible = true;
                            TxtCess2Prc.Visible = false;
                            lblCess2Title.Visible = true;
                        }
                        else
                        {
                            lblCess2.Visible = false;
                            TxtCess2Prc.Visible = false;
                            lblCess2Title.Visible = false;
                        }


                        #endregion for GST  on 17-6-2017 by swathi
                    }
                    #endregion End Extra Data For Billing

                    DateTime today = DateTime.Now.Date;
                    lblServiceChargeTitle.Visible = false;
                    lblServiceCharges.Visible = false;


                    #region Begin Extra Data For Billing Part - 2


                    decimal electricalCost = 0;
                    decimal discountAmount = 0;

                    if (dtContracts.Rows.Count > 0)
                    {
                        string seviceChargetype = dtContracts.Rows[0]["ServiceChargeType"].ToString();

                        #region Machinary Cost checking

                        if (machineryCost > 0)
                        {
                            MachineryCost = Convert.ToDecimal(machineryCost);
                            if (MachineryCost > 0)
                            {

                                if (STMachinary == true)
                                {
                                    lblMachinerywithst.Text = MachineryCost.ToString("0.00");
                                    lblMachinerywithst.Visible = true;
                                    lblMachineryTitlewithst.Visible = true;
                                    // lblMachineryTitlewithst.Text = txtmachinarycost.Text;
                                    lblMachineryTitlewithst.Text = Machinarycosttitle;
                                }
                                else
                                {
                                    lblMachinery.Text = MachineryCost.ToString("0.00");
                                    lblMachinery.Visible = true;
                                    lblMachineryTitle.Visible = true;
                                    //lblMachineryTitle.Text = txtmachinarycost.Text;
                                    lblMachineryTitle.Text = Machinarycosttitle;
                                }
                            }
                            else
                            {

                                #region New code as on 21/01/2014


                                lblMachinerywithst.Text = string.Empty; ;
                                lblMachineryTitlewithst.Text = string.Empty;
                                lblMachinerywithst.Text = string.Empty;
                                lblMachineryTitlewithst.Text = string.Empty;

                                #endregion
                            }
                        }

                        #endregion

                        #region Material Cost checking

                        if (materialCost > 0)
                        {
                            MaterialCost = Convert.ToDecimal(materialCost);
                            if (MaterialCost > 0)
                            {
                                if (STMaterial == true)
                                {
                                    lblMaterialwithst.Text = MaterialCost.ToString("0.00");
                                    lblMaterialwithst.Visible = true;
                                    lblMaterialTitlewithst.Visible = true;
                                    //lblMaterialTitlewithst.Text = txtMaterialcost.Text;
                                    lblMaterialTitlewithst.Text = Materialcosttitle;
                                }
                                else
                                {
                                    lblMaterial.Text = MaterialCost.ToString("0.00");
                                    lblMaterial.Visible = true;
                                    lblMaterialTitle.Visible = true;
                                    //lblMaterialTitle.Text = txtMaterialcost.Text;
                                    lblMaterialTitle.Text = Materialcosttitle;
                                }

                            }
                            else
                            {

                                lblMaterial.Text = string.Empty;
                                lblMaterialTitle.Text = string.Empty;
                                lblMaterialwithst.Text = string.Empty;
                                lblMaterialTitlewithst.Text = string.Empty;

                            }
                        }
                        #endregion


                        if (RelChrgAmt > 0)
                        {
                            lblRelChrTitle.Visible = true;
                            lblRelChrgAmt.Visible = true;
                        }

                        else
                        {
                            lblRelChrTitle.Visible = false;
                            lblRelChrgAmt.Visible = false;
                        }

                        if (seviceChargetype.Length > 0)
                        {
                            if (ServiceCharge > 0)
                            {
                                lblServiceChargeTitle.Visible = true;
                                lblServiceCharges.Visible = true;
                            }
                        }
                        else
                        {
                            lblServiceChargeTitle.Visible = false;
                            lblServiceCharges.Visible = false;
                        }

                        #region Maintenance Cost Checking

                        if (maintenancecost > 0)
                        {
                            electricalCost = Convert.ToDecimal(maintenancecost);
                            if (electricalCost > 0)
                            {
                                if (STMaintenance == true)
                                {
                                    lblElectricalTitlewithst.Visible = true;
                                    lblElectricalwithst.Visible = true;
                                    lblElectricalwithst.Text = electricalCost.ToString("0.00");
                                    lblElectricalTitlewithst.Text = Maintanancecosttitle;
                                }
                                else
                                {
                                    lblElectricalTitle.Visible = true;
                                    lblElectrical.Visible = true;
                                    lblElectrical.Text = electricalCost.ToString("0.00");
                                    lblElectricalTitle.Text = Maintanancecosttitle;
                                }
                            }
                            else
                            {

                                lblElectrical.Text = string.Empty;
                                lblElectricalTitle.Text = string.Empty;
                                lblElectricalwithst.Text = string.Empty;
                                lblElectricalTitlewithst.Text = string.Empty;

                            }
                        }

                        #endregion

                        #region Discount one


                        if (discountone > 0)
                        {
                            discountAmount = Convert.ToDecimal(discountone);
                            if (discountAmount > 0)
                            {
                                if (STDiscountone == true)
                                {

                                    lblDiscountwithst.Visible = true;
                                    lblDiscountTitlewithst.Visible = true;
                                    lblDiscountwithst.Text = discountAmount.ToString("0.00");
                                    lblDiscountTitlewithst.Text = Discountonetitle;


                                }
                                else
                                {

                                    lblDiscount.Visible = true;
                                    lblDiscountTitle.Visible = true;
                                    lblDiscount.Text = discountAmount.ToString("0.00");
                                    lblDiscountTitle.Text = Discountonetitle;
                                }

                            }
                            else
                            {

                                #region New code as on 21/01/2014

                                lblDiscountwithst.Visible = false;
                                lblDiscountTitlewithst.Visible = false;
                                lblDiscount.Visible = false;
                                lblDiscountTitle.Visible = false;

                                lblDiscount.Text = string.Empty;
                                lblDiscountTitle.Text = string.Empty;
                                lblDiscountwithst.Text = string.Empty;
                                lblDiscountTitlewithst.Text = string.Empty;

                                #endregion
                            }
                        }

                        #endregion

                        /* Begin  New code as on [01-07-2013]*/

                        #region Extraamount One



                        if (extraamountonecost > 0)
                        {
                            ExtraOneAmt = Convert.ToDecimal(extraamountonecost);
                            if (ExtraOneAmt > 0)
                            {
                                if (STExtraone == true)
                                {

                                    lblextraonetitlewithst.Visible = true;
                                    lblextraonewithst.Visible = true;
                                    lblextraonewithst.Text = ExtraOneAmt.ToString("0.00");
                                    //lblextraonetitlewithst.Text = txtextraonetitle.Text;
                                    lblextraonetitlewithst.Text = Extraonetitle;
                                }
                                else
                                {
                                    lblextraoneamttitle.Visible = true;
                                    lblextraamt.Visible = true;
                                    lblextraamt.Text = ExtraOneAmt.ToString("0.00");
                                    //lblextraoneamttitle.Text = txtextraonetitle.Text;
                                    lblextraoneamttitle.Text = Extraonetitle;
                                }
                            }

                            else
                            {

                                #region New code as on 21/01/2014

                                lblextraamt.Text = string.Empty;
                                lblextraoneamttitle.Text = string.Empty;
                                lblextraonewithst.Text = string.Empty;
                                lblextraonetitlewithst.Text = string.Empty;

                                #endregion
                            }
                        }
                        #endregion

                        #region Extraamount Two


                        if (extraamoounttwocost > 0)
                        {
                            ExtraTwoAmt = Convert.ToDecimal(extraamoounttwocost);
                            if (ExtraTwoAmt > 0)
                            {
                                if (STExtratwo == true)
                                {
                                    lblextratwotitlewithst.Visible = true;
                                    lblextratwowithst.Visible = true;
                                    lblextratwowithst.Text = ExtraTwoAmt.ToString("0.00");
                                    //lblextratwotitlewithst.Text = txtextratwotitle.Text;
                                    lblextratwotitlewithst.Text = Extratwotitle;
                                }
                                else
                                {
                                    lblextratwoamttitle.Visible = true;
                                    lblextratwoamt.Visible = true;
                                    lblextratwoamt.Text = ExtraTwoAmt.ToString("0.00");
                                    lblextratwoamttitle.Text = txtextratwotitle.Text;
                                }
                            }

                            else
                            {

                                #region New code as on 21/01/2014

                                lblextratwoamt.Text = string.Empty;
                                lblextratwoamttitle.Text = string.Empty;
                                lblextratwowithst.Text = string.Empty;
                                lblextratwotitlewithst.Text = string.Empty;

                                #endregion
                            }
                        }
                        #endregion


                        #region Discount two


                        if (discounttwo > 0)
                        {
                            DisCountTwoAmt = Convert.ToDecimal(discounttwo);
                            if (DisCountTwoAmt > 0)
                            {
                                if (STDiscounttwo == true)
                                {
                                    lblDiscounttwowithst.Visible = true;
                                    lblDiscounttwotitlewithst.Visible = true;
                                    lblDiscounttwowithst.Text = DisCountTwoAmt.ToString("0.00");
                                    lblDiscounttwotitlewithst.Text = Discounttwotitle;




                                }
                                else
                                {
                                    lblDiscounttwo.Visible = true;
                                    lblDiscounttwoTitle.Visible = true;
                                    lblDiscounttwo.Text = DisCountTwoAmt.ToString("0.00");
                                    lblDiscounttwoTitle.Text = Discounttwotitle;


                                }
                            }
                            else
                            {

                                #region New code as on 21/01/2014
                                lblDiscounttwowithst.Visible = false;
                                lblDiscounttwotitlewithst.Visible = false;
                                lblDiscounttwo.Visible = false;
                                lblDiscounttwoTitle.Visible = false;

                                lblDiscounttwo.Text = string.Empty;
                                lblDiscounttwoTitle.Text = string.Empty;
                                lblDiscounttwowithst.Text = string.Empty;
                                lblDiscounttwotitlewithst.Text = string.Empty;

                                #endregion
                            }

                        }

                        #endregion
                        /*End  New code As on [01-07-2013]*/

                        #region Begin New Code for Service tax amount on Individual extra detail as on 01/04/2014 by Venkat

                        if (Staxonservicecharge > 0)
                        {
                            lblStaxamtonServicechargetitle.Visible = true;
                            lblStaxamtonServicecharge.Visible = true;
                            lblStaxamtonServicecharge.Text = Staxonservicecharge.ToString();
                        }
                        else
                        {

                            lblStaxamtonServicechargetitle.Visible = false;
                            lblStaxamtonServicecharge.Visible = false;
                            lblStaxamtonServicecharge.Text = string.Empty;
                        }

                        if (SCamtonMachinary > 0 && SCMachinary == true)
                        {

                            lblSChargeamtonMachinarytitle.Visible = true;
                            lblSChargeamtonMachinary.Visible = true;
                            lblSChargeamtonMachinary.Text = SCamtonMachinary.ToString();
                        }
                        else
                        {
                            lblSChargeamtonMachinarytitle.Visible = false;
                            lblSChargeamtonMachinary.Visible = false;
                            lblSChargeamtonMachinary.Text = string.Empty;
                        }


                        if (SCamtonMaintenance > 0 && SCMaintenance == true)
                        {

                            lblSchargeamtonMaintenancetitle.Visible = true;
                            lblSchargeamtonMaintenance.Visible = true;
                            lblSchargeamtonMaintenance.Text = SCamtonMaintenance.ToString();
                        }
                        else
                        {
                            lblSchargeamtonMaintenancetitle.Visible = false;
                            lblSchargeamtonMaintenance.Visible = false;
                            lblSchargeamtonMaintenance.Text = string.Empty;
                        }


                        if (SCamtonMaterial > 0 && SCMaterial == true)
                        {

                            lblSchargeamtonMaterialtitle.Visible = true;
                            lblSchargeamtonMaterial.Visible = true;
                            lblSchargeamtonMaterial.Text = SCamtonMaterial.ToString();

                        }
                        else
                        {
                            lblSchargeamtonMaterialtitle.Visible = false;
                            lblSchargeamtonMaterial.Visible = false;
                            lblSchargeamtonMaterial.Text = string.Empty;
                        }


                        if (SCamtonExtraone > 0 && SCExtraone == true)
                        {

                            lblSchargeamtonExtraonetitle.Visible = true;
                            lblSchargeamtonExtraone.Visible = true;
                            lblSchargeamtonExtraone.Text = SCamtonExtraone.ToString();

                        }
                        else
                        {
                            lblSchargeamtonExtraonetitle.Visible = false;
                            lblSchargeamtonExtraone.Visible = false;
                            lblSchargeamtonExtraone.Text = string.Empty;
                        }

                        if (SCamtonExtratwo > 0 && SCExtratwo == true)
                        {

                            lblSchargeamtonExtratwotitle.Visible = true;
                            lblSchargeamtonExtratwo.Visible = true;
                            lblSchargeamtonExtratwo.Text = SCamtonExtratwo.ToString();
                        }
                        else
                        {
                            lblSchargeamtonExtratwotitle.Visible = false;
                            lblSchargeamtonExtratwo.Visible = false;
                            lblSchargeamtonExtratwo.Text = string.Empty;
                        }

                        #endregion


                    }

                    #endregion End Extra Data For Billing Part - 2

                    #region Begin Code For Grand Total Part
                    TotalResourceCost = Convert.ToDecimal(dutiestotalamount);
                    lblTotalResources.Text = (TotalResourceCost).ToString("0.00");
                    GrandTotal = 0;
                    decimal RoundOff = 0;
                    if (dtContracts.Rows.Count > 0)
                    {
                        RoundOff = Convert.ToDecimal(dtContracts.Rows[0]["RoundOffamt"].ToString());
                        GrandTotal = Convert.ToDecimal(dtContracts.Rows[0]["Grandtotal"].ToString());
                    }
                    txtRoundoffamt.Text = RoundOff.ToString("0.00");
                    lblroundoffs.Visible = true;
                    txtRoundoffamt.Visible = true;
                    lblGrandTotal.Text = GrandTotal.ToString("0.00");
                    string GTotal = GrandTotal.ToString("0.00");
                    string[] arr = GTotal.ToString().Split("."[0]);
                    string inwords = "";
                    string rupee = (arr[0]);
                    string paise = "";
                    if (arr.Length == 2)
                    {
                        if (arr[1].Length > 0 && arr[1] != "00")
                        {
                            paise = (arr[1]);
                        }
                    }

                    if (paise != "0.00" && paise != "0" && paise != "")
                    {
                        int I = Int16.Parse(paise);
                        String p = NumberToEnglish.Instance.NumbersToWords(I, true);
                        paise = p;
                        rupee = NumberToEnglish.Instance.NumbersToWords(Convert.ToInt64(arr[0]), false);
                        inwords = " Rupees " + rupee + "" + paise + " Paise Only";

                    }
                    else
                    {
                        rupee = NumberToEnglish.Instance.NumbersToWords(Convert.ToInt64(arr[0]), true);
                        inwords = " Rupees " + rupee + " Only";
                    }



                    lblamtinwords.Text = inwords;

                    #endregion End Code For Grand Total Part

                }
                #endregion  Begin Code For Display Invoice Data Based On The ClientIdAndMonth as on [04-03-2014]
                VisibleFreeze();
                visiblebutton();
            }
            catch (Exception ex)
            {

            }
        }

        protected void btncleardata_Click(object sender, EventArgs e)
        {

            if (gvClientBilling.Rows.Count > 0)
            {
                for (int i = 0; i < gvClientBilling.Rows.Count; i++)
                {
                    TextBox txtgvdesgn = (TextBox)gvClientBilling.Rows[i].Cells[1].FindControl("lbldesgn");
                    TextBox txtHSNNumber = (TextBox)gvClientBilling.Rows[i].Cells[2].FindControl("txtHSNNumber");
                    TextBox txtnoofemployees = (TextBox)gvClientBilling.Rows[i].Cells[3].FindControl("lblnoofemployees");
                    TextBox txtNoOfDuties = (TextBox)gvClientBilling.Rows[i].Cells[4].FindControl("lblNoOfDuties");
                    TextBox txtPayRate = (TextBox)gvClientBilling.Rows[i].Cells[5].FindControl("lblpayrate");
                    TextBox txtNewPayRate = (TextBox)gvClientBilling.Rows[i].Cells[6].FindControl("txtNewPayRate");
                    DropDownList ddldutytype = (DropDownList)gvClientBilling.Rows[i].Cells[7].FindControl("ddldutytype");
                    DropDownList ddlnod = (DropDownList)gvClientBilling.Rows[i].Cells[8].FindControl("ddlnod");
                    TextBox txtda = (TextBox)gvClientBilling.Rows[i].Cells[9].FindControl("lblda");
                    TextBox txtAmount = (TextBox)gvClientBilling.Rows[i].Cells[10].FindControl("lblAmount");

                    txtgvdesgn.Text = "";
                    txtHSNNumber.Text = "";
                    txtnoofemployees.Text = "0";
                    txtNoOfDuties.Text = "0";
                    txtPayRate.Text = "0";
                    txtda.Text = "0";
                    txtAmount.Text = "0";
                    ddldutytype.SelectedIndex = 0;
                    ddlnod.SelectedIndex = 0;
                }
            }

        }

        public class PageEventHelper : PdfPageEventHelper
        {
            PdfContentByte cb;
            PdfTemplate template;

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                cb = writer.DirectContent;
                template = cb.CreateTemplate(50, 50);
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);






                iTextSharp.text.Image imgfoot = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/assets/Footer.png"));
                //iTextSharp.text.Image imghead = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/assets/LetterheadHeader.png"));

                imgfoot.SetAbsolutePosition(0, 0);
                //imghead.SetAbsolutePosition(0, 0);

                //PdfContentByte cbhead = writer.DirectContent;
                //PdfTemplate tp = cbhead.CreateTemplate(500, 130);
                //tp.AddImage(imghead);

                PdfContentByte cbfoot = writer.DirectContent;
                PdfTemplate tpl = cbfoot.CreateTemplate(557, 100);
                tpl.AddImage(imgfoot);

                //cbhead.AddTemplate(tp, 25, 720);

                cbfoot.AddTemplate(tpl, 19, 27);

                //Phrase headPhraseImg = new Phrase(cbhead + "", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 7, iTextSharp.text.Font.NORMAL));
                Phrase footPhraseImg = new Phrase(cbfoot + "", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 7, iTextSharp.text.Font.NORMAL));
            }
        }


        protected void ddlclientid_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnvisible();
            lblResult.Text = "";
            lbltotalamount.Text = "";
            lblbillnolatest.Text = "";
            txtbilldate.Text = "";
            txtduedate.Text = "";
            txtfromdate.Text = "";
            txttodate.Text = "";
            rdbcreatebill.Checked = true;
            rdbmodifybill.Checked = false;

            ddlmonth.SelectedIndex = 0;
            txtmonth.Text = string.Empty;
            ClearExtraDataForBilling();
            if (ddlclientid.SelectedIndex > 0)
            {
                string SqlQryGetCname = "select clientid from clients where clientid='" + ddlclientid.SelectedValue + "'";
                DataTable dt;
                dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryGetCname).Result;
                ddlCname.SelectedValue = dt.Rows[0]["clientid"].ToString();
                ddlmonth.SelectedIndex = 0;
                dt = null;
                gvClientBilling.DataSource = dt;
                gvClientBilling.DataBind();

                if (ddlmonth.SelectedIndex > 0)
                {
                    DisplayDataInGrid();
                }
            }
            else
            {
                ddlCname.SelectedIndex = 0;
            }

            VisibleFreezeCredit();
            visiblebutton();
            displayExtraData();
        }

        protected void ddlCname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            btnvisible_cname();
            lblResult.Text = "";
            ddlmonth.SelectedIndex = 0;
            lblbillnolatest.Text = "";
            txtbilldate.Text = "";
            txtduedate.Text = "";
            txtfromdate.Text = "";
            txttodate.Text = "";
            txtmonth.Text = string.Empty;

            ClearExtraDataForBilling();
            if (ddlCname.SelectedIndex > 0)
            {
                string SqlQryGetCname = "select clientid from clients where clientid='" + ddlCname.SelectedValue + "'";
                DataTable dt;
                dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryGetCname).Result;
                if (dt.Rows.Count > 0)
                {
                    ddlclientid.SelectedValue = dt.Rows[0]["clientid"].ToString();
                }
                if (ddlmonth.SelectedIndex > 0)
                {
                    DisplayDataInGrid();
                }
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }

            VisibleFreezeCredit();
            visiblebutton();
            displayExtraData();
        }

        protected void checkExtraData_CheckedChanged(object sender, EventArgs e)
        {
            if (checkExtraData.Checked)
            {
                if (ddlclientid.SelectedIndex > 0)
                {
                    panelRemarks.Visible = true;

                    txtmachinarycost.Text = "Machinery Cost :";
                    txtMaterialcost.Text = "Material Cost :";
                    txtMaintanancecost.Text = "Maintenance Work :";
                    txtextraonetitle.Text = "Extra Amount one :";
                    txtextratwotitle.Text = "Extra Amount Two :";
                    txtdiscount.Text = "Discounts :";
                    txtdiscounttwotitle.Text = "Discount Two :";

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' Select Client details and month ');", true);
                    checkExtraData.Checked = false;
                }
            }
            else
            {
                panelRemarks.Visible = false;
            }
        }

        protected void ClearExtraDataForBilling()
        {
            lblResult.Text = "";
            txtDiscounts.Text = "";
            txtElectical.Text = "";
            txtMachinery.Text = "";
            txtMaterial.Text = "";
            txtRemarks.Text = "";
            lblDiscount.Text = "";
            lblElectrical.Text = "";
            lblRemarks.Text = "";
            lblMachinery.Text = "";
            lblMaterial.Text = "";
            lblServiceCharges.Text = "";
            gvClientBilling.DataSource = null;
            gvClientBilling.DataBind();
            //txtfromdate.Text = "";
            //txttodate.Text = "";
            lblTotalResources.Text = "";
            lblServiceCharges.Text = "";
            lblServiceTax.Text = "";
            lblCESS.Text = "";
            lblSBCESS.Text = "";
            lblSheCESS.Text = "";
            lblST75.Text = "";
            lblMachinery.Text = "";
            lblMaterial.Text = "";
            lblElectrical.Text = "";
            lblDiscount.Text = "";
            lblGrandTotal.Text = "";
            lblST25.Text = "";



            checkExtraData.Checked = false;
            panelRemarks.Visible = false;


            lblMachineryTitle.Text = string.Empty;
            lblMachineryTitlewithst.Text = string.Empty;
            lblMachinery.Text = string.Empty;
            lblMachinerywithst.Text = string.Empty;

            lblMaterialTitle.Text = string.Empty;
            lblMaterialTitlewithst.Text = string.Empty;
            lblMaterial.Text = string.Empty;
            lblMaterialwithst.Text = string.Empty;

            lblElectricalTitle.Text = string.Empty;
            lblElectricalTitlewithst.Text = string.Empty;
            lblElectrical.Text = string.Empty;
            lblElectricalwithst.Text = string.Empty;

            lblextraoneamttitle.Text = string.Empty;
            lblextraonetitlewithst.Text = string.Empty;
            lblextraamt.Text = string.Empty;
            lblextraonewithst.Text = string.Empty;

            lblextratwoamttitle.Text = string.Empty;
            lblextratwotitlewithst.Text = string.Empty;
            lblextratwoamt.Text = string.Empty;
            lblextratwowithst.Text = string.Empty;

            lblDiscountTitle.Text = string.Empty;
            lblDiscountTitlewithst.Text = string.Empty;
            lblDiscount.Text = string.Empty;
            lblDiscountwithst.Text = string.Empty;

            lblDiscounttwoTitle.Text = string.Empty;
            lblDiscounttwotitlewithst.Text = string.Empty;
            lblDiscounttwo.Text = string.Empty;
            lblDiscounttwowithst.Text = string.Empty;

            txtmachinarycost.Text = "Machinery Cost :";
            txtMaterialcost.Text = "Material Cost :";
            txtMaintanancecost.Text = "Maintenance Work :";
            txtextraonetitle.Text = "Extra Amount one :";
            txtextratwotitle.Text = "Extra Amount Two :";
            txtdiscount.Text = "Discounts :";
            txtdiscounttwotitle.Text = "Discount Two :";


            chkSTYesMachinary.Checked = chkSTYesElectrical.Checked = chkSTYesMachinary.Checked = chkSTYesExtraone.Checked =
                chkSTYesExtratwo.Checked = chkSTDiscountone.Checked = chkSTDiscounttwo.Checked = false;

            chkSCYesMachinary.Checked = chkSCYesElectrical.Checked = chkSCYesMaterial.Checked = chkSCYesExtraone.Checked =
                chkSCYesExtratwo.Checked = false;

        }

        protected void txtbillno_OnTextChanged(object sender, EventArgs e)
        {
            string sqlqry = "Select Clients.Clientname,unitbill.unitid from unitbill  " +
                "  inner join  clients  on Clients.clientid=unitbill.unitid Where billno='" + txtbillno.Text.Trim() + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                txtclientid.Text = dt.Rows[0][1].ToString();
                txtclientname.Text = dt.Rows[0][0].ToString();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('invalid Bill  no');", true);
            }


        }

        protected void btndelelte_Click(object sender, EventArgs e)
        {

            string sqlqry = "  delete from unitbill  Where billno='" + txtbillno.Text.Trim() + "'";

            int status = config.ExecuteNonQueryWithQueryAsync(sqlqry).Result;
            if (status > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Bill Deleted Successfully');", true);


                txtbillno.Text = "";
                txtclientid.Text = "";
                txtclientname.Text = "";

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Invalid Bill No');", true);

            }

        }

        protected int GetMonthBasedonSelection()
        {

            int Month = 0;
            if (ddlmonth.SelectedIndex == 1)
            {
                Month = GlobalData.Instance.GetIDForNextMonth();
            }
            else if (ddlmonth.SelectedIndex == 2)
            {
                Month = GlobalData.Instance.GetIDForThisMonth();
            }
            else if (ddlmonth.SelectedIndex == 3)
            {
                Month = GlobalData.Instance.GetIDForPrviousMonth();
            }
            else if (ddlmonth.SelectedIndex == 4)
            {
                Month = GlobalData.Instance.GetIDForPrviousoneMonth();
            }

            return Month;
        }

        protected void Btn_Genrate_Invoice_Click(object sender, EventArgs e)
        {
            try
            {
                #region  Begin  Validations as on [26-10-2013]
                if (ddlclientid.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Select Client Id ');", true);
                    return;
                }

                #region  Begin New code As on [10-03-2014]

                if (Chk_Month.Checked == true)
                {
                    if (txtmonth.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Enter Month for Bill ');", true);
                        return;
                    }
                    if (Timings.Instance.CheckEnteredDate(txtmonth.Text) == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid TO DATE .Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }
                }
                else
                {
                    if (ddlmonth.SelectedIndex == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Select Month for Bill ');", true);

                        return;
                    }
                }
                #endregion  End Old Code As on [14-02-2014]

                if (txtfromdate.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Fill The From Date  ');", true);
                    return;
                }
                else
                {
                    if (Timings.Instance.CheckEnteredDate(txtfromdate.Text) == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid FROM DATE .Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }
                }

                if (txttodate.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Fill The To Date  ');", true);
                    return;
                }
                else
                {
                    if (Timings.Instance.CheckEnteredDate(txttodate.Text) == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid TO DATE .Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }
                }

                if (txtbilldate.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' Please Fill The Billdate  ');", true);
                    return;
                }
                else
                {
                    if (Timings.Instance.CheckEnteredDate(txtbilldate.Text) == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid BILL DATE .Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }
                }
                if (txtduedate.Text.Trim().Length > 0)
                {
                    if (Timings.Instance.CheckEnteredDate(txtbilldate.Text) == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid DUE DATE .Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }
                }


                #endregion End   Validations as on [26-10-2013]

                if (ddlType.SelectedIndex == 0)
                {

                    DateTime DtLastDay = DateTime.Now;
                    if (Chk_Month.Checked == false)
                    {
                        DtLastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlmonth.SelectedIndex);
                    }
                    if (Chk_Month.Checked == true)
                    {
                        DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                    }
                    var ContractID = "";
                    var bBillDates = 0;
                    var ServiceTaxType = false;
                    string SPName = "IMinvoiceformonth";

                    #region  Begin Get Contract Id Based on The Last Day


                    Hashtable HtGetContractID = new Hashtable();
                    var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                    HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
                    HtGetContractID.Add("@LastDay", DtLastDay);
                    DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                    if (DTContractID.Rows.Count > 0)
                    {
                        ContractID = DTContractID.Rows[0]["contractid"].ToString();
                        bBillDates = int.Parse(DTContractID.Rows[0]["BillDates"].ToString());
                        ServiceTaxType = bool.Parse(DTContractID.Rows[0]["ServiceTaxType"].ToString());
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not  Avaialable For This Client.');", true);
                        return;
                    }

                    string Qry = "Select Cn.CGST,cn.SGST,Cn.IGST,S.GSTStateCode BillToStatecode,GM.GSTNo,GM.StateCode GMStatecode,GM.State GMStatename,S.State BillToStateName from clients C inner join GSTMaster GM on GM.ID=C.OurGSTIN left join States S on S.StateID=C.State left join Contracts Cn on Cn.ClientId=C.ClientId where C.ClientId='" + ddlclientid.SelectedValue + "' and ContractId='" + ContractID + "'";
                    DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Qry).Result;
                    string BillToStatecode = "";
                    string GMStatecode = "";
                    string GMStatename = "";
                    string BillToStateName = "";
                    string GSTNo = "";
                    bool CGST = false;
                    bool SGST = false;
                    bool IGST = false;
                    if (dt.Rows.Count > 0)
                    {
                        BillToStatecode = dt.Rows[0]["BillToStatecode"].ToString();
                        GMStatecode = dt.Rows[0]["GMStatecode"].ToString();
                        GMStatename = dt.Rows[0]["GMStatename"].ToString();
                        BillToStateName = dt.Rows[0]["BillToStateName"].ToString();
                        GSTNo = dt.Rows[0]["GSTNo"].ToString();

                        CGST = bool.Parse(dt.Rows[0]["CGST"].ToString());
                        SGST = bool.Parse(dt.Rows[0]["SGST"].ToString());
                        IGST = bool.Parse(dt.Rows[0]["IGST"].ToString());

                        if (BillToStatecode != GMStatecode)
                        {
                            if (CGST == true)
                            {
                                lblResult.Text = "Please check GST option in Contracts.GST number is " + GSTNo + " GST State is " + GMStatename + " To state is " + BillToStateName;
                                return;
                            }

                        }
                        else if (BillToStatecode == GMStatecode)
                        {
                            if (IGST == true)
                            {
                                lblResult.Text = "Please check GST option in Contracts.GST number is " + GSTNo + " GST State is " + GMStatename + " To state is " + BillToStateName;
                                return;
                            }
                        }
                        else
                        {
                            lblResult.Text = "";
                        }
                    }


                    #endregion  End Get Contract Id Based on The Last Day

                    #region Begin Variable Declarations as on [08-03-2014]


                    #region Begin Part One
                    var ClientId = "";
                    var month = 0;
                    var ContractId = "";
                    var LastDay = "";
                    var Fromdate = "";


                    #endregion End Part One

                    #region Begin  Part Two
                    var Todate = "";
                    var Duedate = "";
                    var BillDate = "";
                    var Gendays = 0;
                    var G_Sdays = 0;
                    // var  Staticdays=0;
                    #endregion End Part Two

                    #region Begin Part Three
                    var Extradatacheck = "0";
                    var Extradatastcheck = "0";
                    var MachinaryCost = "0";
                    var MaterialCost = "0";
                    var MaintenanceCost = "0";

                    var Extraamountone = "";
                    var Extraamounttwo = "";
                    var Discount = "";
                    var DiscountTwo = "";

                    #endregion End Part Three

                    #region Begin Part Four
                    var MaterialCostTitle = "";

                    var MachinaryCostTitle = "";
                    var MaintenanceCostTitle = "";
                    var ExtraamountoneTitle = "";
                    var ExtraamounttwoTitle = "";
                    var DiscountTitle = "";
                    var DiscountTwoTitle = "";
                    var BillNum = "";

                    //New Code as on 10/03/2014 by venkat

                    var STMachinary = 0;
                    var STMaterial = 0;
                    var STMaintenance = 0;
                    var STExtraone = 0;
                    var STExtratwo = 0;

                    var SCMachinary = 0;
                    var SCMaterial = 0;
                    var SCMaintenance = 0;
                    var SCExtraone = 0;
                    var SCExtratwo = 0;

                    var STDiscountone = 0;
                    var STDiscounttwo = 0;

                    #endregion End Part Four



                    #endregion End Variable Declarations as on [08-03-2014]

                    #region Begin Assign Values To The Variables as on [08-03-2014]
                    #region Begin Part One
                    ClientId = ddlclientid.SelectedValue;
                    // month = Timings.Instance.GetIdForSelectedMonth(ddlmonth.SelectedIndex);
                    month = GetMonthBasedOnSelectionDateorMonth();
                    LastDay = DtLastDay.ToString();
                    ContractId = ContractID;
                    Fromdate = Timings.Instance.CheckDateFormat(txtfromdate.Text); ;
                    #endregion End Part One

                    #region Begin  Part Two

                    Todate = Timings.Instance.CheckDateFormat(txttodate.Text); ;
                    Duedate = Timings.Instance.CheckDateFormat(txtduedate.Text); ;
                    BillDate = Timings.Instance.CheckDateFormat(txtbilldate.Text); ;
                    if (Chk_Month.Checked == false)
                    {
                        Gendays = Timings.Instance.GetNoofDaysForSelectedMonth(ddlmonth.SelectedIndex, bBillDates);
                    }

                    //New Code when select month in Textbox
                    if (Chk_Month.Checked == true)
                    {
                        DateTime mGendays = DateTime.Now;
                        DateTime date = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                        mGendays = DateTime.Parse(date.ToString());
                        Gendays = Timings.Instance.GetNoofDaysForEnteredMonth(mGendays, bBillDates);
                    }
                    G_Sdays = Timings.Instance.Get_GS_Days(month, Gendays);


                    string qry = "delete from unitbillbreakup where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "' and Extra like 'E%'";
                    int deletestatus = config.ExecuteNonQueryWithQueryAsync(qry).Result;
                    int k = 0;
                    string Extra = "";
                    Hashtable htnew = new Hashtable();
                    int statusn = 0;
                    int m = 0;
                    foreach (GridViewRow GvRow in gvClientBilling.Rows)
                    {

                        string spname = "";
                        htnew.Clear();


                        string SerialNo = "select max(isnull(sino,0)) as sno from unitbillbreakup where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "'";
                        DataTable dtSerial = config.ExecuteAdaptorAsyncWithQueryParams(SerialNo).Result;

                        int SerialNumber = 1;

                        if (dtSerial.Rows.Count > 0)
                        {
                            SerialNumber = Int32.Parse(dtSerial.Rows[0]["sno"].ToString()) + 1;
                        }

                        //string sno = ((Label)GvRow.FindControl("lblSno")).Text;
                        string Desgn = ((TextBox)GvRow.FindControl("lbldesgn")).Text;
                        string NoOfEmps = ((TextBox)GvRow.FindControl("lblnoofemployees")).Text;
                        string NoOfDuties = ((TextBox)GvRow.FindControl("lblNoOfDuties")).Text;
                        string Payrate = ((TextBox)GvRow.FindControl("lblpayrate")).Text; //lblda
                        string SchrgPrc = ((TextBox)GvRow.FindControl("lblSchrgPrc")).Text;
                        string DutiesAmount = ((TextBox)GvRow.FindControl("lblda")).Text;
                        string Total = ((TextBox)GvRow.FindControl("lblAmount")).Text;
                        string HSNNumber = ((DropDownList)GvRow.FindControl("ddlHSNNumber")).SelectedValue;
                        string noofdays = ((DropDownList)GvRow.FindControl("ddlnod")).SelectedValue;
                        string CalnType = ((DropDownList)GvRow.FindControl("ddlCalnType")).SelectedValue;

                        if (((CheckBox)GvRow.FindControl("chkExtra")).Checked)
                        {
                            Extra = "E";
                        }
                        string designid = ((Label)GvRow.FindControl("lbldesignid")).Text;
                        string GSTper = ((TextBox)GvRow.FindControl("lblGSTper")).Text;

                        if (GSTper == "")
                        {
                            GSTper = "0";
                        }

                        decimal CGSTper = 0;
                        decimal SGSTper = 0;
                        decimal IGSTper = 0;

                        if (CGST == true)
                        {
                            CGSTper = decimal.Parse(GSTper) / 2;
                            SGSTper = decimal.Parse(GSTper) / 2;

                        }

                        if (IGST == true)
                        {
                            IGSTper = decimal.Parse(GSTper);

                        }



                        float ToatlAmount = 0;
                        float basicda = 0;
                        ToatlAmount = (Total.Trim().Length != 0) ? float.Parse(Total) : 0;
                        basicda = (DutiesAmount.Trim().Length != 0) ? float.Parse(DutiesAmount) : 0;

                        float schargeamt = float.Parse(DutiesAmount) * float.Parse(SchrgPrc) / 100;
                        string Ex = "E" + m;

                        if (Desgn.Length > 0 && Extra != "")
                        {
                            spname = "ExtraDataBilling";
                            htnew.Add("@clientid", ddlclientid.SelectedValue);
                            htnew.Add("@month", month);
                            htnew.Add("@contractid", ContractId);
                            htnew.Add("@remarks", Desgn);
                            htnew.Add("@Duties", NoOfDuties);
                            htnew.Add("@Noofemps", NoOfEmps);
                            htnew.Add("@SchrgPrc", SchrgPrc);
                            htnew.Add("@SchrgAmt", schargeamt);
                            htnew.Add("@Payrate", Payrate);
                            htnew.Add("@DutiesAmount", DutiesAmount);
                            htnew.Add("@Gendays", Gendays);
                            htnew.Add("@Extra", Ex);
                            htnew.Add("@sno", SerialNumber);
                            htnew.Add("@HSNNumber", HSNNumber);
                            htnew.Add("@NoofDays", noofdays);
                            htnew.Add("@CGSTPrc", CGSTper);
                            htnew.Add("@SGSTPrc", SGSTper);
                            htnew.Add("@IGSTPrc", IGSTper);
                            htnew.Add("@Type", CalnType);
                            statusn = config.ExecuteNonQueryParamsAsync(spname, htnew).Result;

                        }

                        m++;
                    }


                    #endregion End Part Two

                    #region Begin Part Three

                    if (checkExtraData.Checked)
                    {
                        Extradatacheck = "1";
                    }
                    if (Extradatacheck == "1")
                    {
                        if (txtMachinery.Text.Trim().Length > 0)
                        {
                            MachinaryCost = txtMachinery.Text;
                        }
                        if (txtMaterial.Text.Trim().Length > 0)
                        {
                            MaterialCost = txtMaterial.Text;
                        }
                        if (txtElectical.Text.Trim().Length > 0)
                        {
                            MaintenanceCost = txtElectical.Text;
                        }
                        if (txtextraonevalue.Text.Trim().Length > 0)
                        {
                            Extraamountone = txtextraonevalue.Text;
                        }
                        if (txtextratwovalue.Text.Trim().Length > 0)
                        {
                            Extraamounttwo = txtextratwovalue.Text;
                        }
                        if (txtDiscounts.Text.Trim().Length > 0)
                        {
                            Discount = txtDiscounts.Text;
                        }
                        if (txtdiscounttwovalue.Text.Trim().Length > 0)
                        {
                            DiscountTwo = txtdiscounttwovalue.Text;
                        }
                    }
                    #endregion End Part Three

                    #region Begin Part Four
                    if (Extradatacheck == "1")
                    {
                        MaterialCostTitle = txtMaterialcost.Text;
                        MachinaryCostTitle = txtmachinarycost.Text;
                        MaintenanceCostTitle = txtMaintanancecost.Text;
                        ExtraamountoneTitle = txtextraonetitle.Text;
                        ExtraamounttwoTitle = txtextratwotitle.Text;
                        DiscountTitle = txtdiscount.Text;
                        DiscountTwoTitle = txtdiscounttwotitle.Text;
                    }
                    BillNum = BillnoAutoGenrate(ServiceTaxType, ClientId, month);
                    #endregion End Part Three

                    #region New Code for Service tax for extra data check value on 01/04/2014

                    if (Extradatacheck == "1")
                    {

                        if (chkSTYesMachinary.Checked == true)
                        {
                            STMachinary = 1;
                        }
                        if (chkSTYesMaterial.Checked == true)
                        {
                            STMaterial = 1;
                        }
                        if (chkSTYesElectrical.Checked == true)
                        {
                            STMaintenance = 1;
                        }
                        if (chkSTYesExtraone.Checked == true)
                        {
                            STExtraone = 1;
                        }
                        if (chkSTYesExtratwo.Checked == true)
                        {
                            STExtratwo = 1;
                        }


                        if (chkSCYesMachinary.Checked == true)
                        {
                            SCMachinary = 1;
                        }
                        if (chkSCYesMaterial.Checked == true)
                        {
                            SCMaterial = 1;
                        }
                        if (chkSCYesElectrical.Checked == true)
                        {
                            SCMaintenance = 1;
                        }
                        if (chkSCYesExtraone.Checked == true)
                        {
                            SCExtraone = 1;
                        }
                        if (chkSCYesExtratwo.Checked == true)
                        {
                            SCExtratwo = 1;
                        }


                        if (chkSTDiscountone.Checked == true)
                        {
                            STDiscountone = 1;
                        }
                        if (chkSTDiscounttwo.Checked == true)
                        {
                            STDiscounttwo = 1;
                        }
                    }
                    #endregion


                    #endregion End Assign Values To The Variables as on [08-03-2014]

                    #region for insertion into empbillsheet

                    string chkpdfsquery = "select pdfs,WageType  from Contracts where clientid='" + ddlclientid.SelectedValue + "' and contractid='" + ContractID + "'";
                    DataTable dtchkpdfs = config.ExecuteAdaptorAsyncWithQueryParams(chkpdfsquery).Result;
                    string chkpdf = "False";
                    int Wagetype = 0;
                    if (dtchkpdfs.Rows.Count > 0)
                        chkpdf = dtchkpdfs.Rows[0]["pdfs"].ToString();
                    Wagetype = int.Parse(dtchkpdfs.Rows[0]["WageType"].ToString());

                    if (chkpdf == "True")////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    {


                        #region  Begin Code For Variable Declarations as on [18-03-2014]
                        var ClientID = "";
                        var SPName1 = string.Empty;
                        DataTable DtEmpBillpaysheet = null;
                        Hashtable HTEmpBillpaysheet = new Hashtable();
                        if (Wagetype == 3)
                        {
                            SPName1 = "IMGenrateSalary";
                        }
                        else
                        {
                            SPName1 = "IMGenrateBillPaysheet";

                        }
                        #endregion End Code For Variable Declarations    as on [18-03-2014]



                        #region  Begin Code For Assign Values To the  Variable  as on [18-03-2014]
                        ClientID = ddlclientid.SelectedValue;
                        month = GetMonthBasedOnSelectionDateorMonth();
                        if (Chk_Month.Checked == false)
                        {
                            Gendays = Timings.Instance.GetNoofDaysForSelectedMonth(ddlmonth.SelectedIndex, bBillDates);
                        }

                        //New Code when select month in Textbox
                        if (Chk_Month.Checked == true)
                        {
                            DateTime mGendays = DateTime.Now;
                            DateTime date = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                            mGendays = DateTime.Parse(date.ToString());
                            Gendays = Timings.Instance.GetNoofDaysForEnteredMonth(mGendays, bBillDates);
                        }
                        G_Sdays = Timings.Instance.Get_GS_Days(month, Gendays);
                        HTEmpBillpaysheet.Add("@Gendays", Gendays);
                        HTEmpBillpaysheet.Add("@G_Sdays", G_Sdays);
                        HTEmpBillpaysheet.Add("@clientid", ddlclientid.SelectedValue);
                        HTEmpBillpaysheet.Add("@month", month);
                        HTEmpBillpaysheet.Add("@lastday", DtLastDay);
                        if (Wagetype == 3)
                        {
                            HTEmpBillpaysheet.Add("@Type", 1);
                        }
                        #endregion End Code For  Assign Values To the Variable  as on [18-03-2014]

                        #region Begin Code For Calling Stored procedure as on [18-03-2014]
                        DtEmpBillpaysheet = config.ExecuteAdaptorAsyncWithParams(SPName1, HTEmpBillpaysheet).Result;
                        #endregion  end Code For Calling Stored procedure as on [18-03-2014]


                    }

                    #endregion for insertion into empbillsheet



                    #region Begin Code For Define & Assign Values To The Hash Table as on [08-03-2014]
                    Hashtable HTIMInvoice = new Hashtable();

                    #region Begin Part One

                    HTIMInvoice.Add("@ClientId", ClientId);
                    HTIMInvoice.Add("@month", month);
                    HTIMInvoice.Add("@ContractId", ContractId);
                    HTIMInvoice.Add("@LastDay", DtLastDay);
                    HTIMInvoice.Add("@Fromdate", Fromdate);

                    #endregion End Part One

                    #region Begin  Part Two
                    HTIMInvoice.Add("@Todate", Todate);
                    HTIMInvoice.Add("@Duedate", Duedate);
                    HTIMInvoice.Add("@BillDate", BillDate);
                    HTIMInvoice.Add("@Gendays", Gendays);
                    HTIMInvoice.Add("@G_Sdays", G_Sdays);
                    #endregion End Part Two

                    #region Begin Part Three

                    HTIMInvoice.Add("@Extradatacheck", Extradatacheck);
                    HTIMInvoice.Add("@Extradatastcheck", Extradatastcheck);
                    HTIMInvoice.Add("@MaterialCost", MaterialCost);
                    HTIMInvoice.Add("@MaintenanceCost", MaintenanceCost);
                    HTIMInvoice.Add("@MachinaryCost", MachinaryCost);

                    HTIMInvoice.Add("@Extraamountone", Extraamountone);
                    HTIMInvoice.Add("@Extraamounttwo", Extraamounttwo);
                    HTIMInvoice.Add("@Discount", Discount);
                    HTIMInvoice.Add("@DiscountTwo", DiscountTwo);


                    #endregion End Part Three

                    #region Begin Part Four

                    HTIMInvoice.Add("@MaterialCostTitle", MaterialCostTitle);
                    HTIMInvoice.Add("@MachinaryCostTitle", MachinaryCostTitle);
                    HTIMInvoice.Add("@MaintenanceCostTitle", MaintenanceCostTitle);
                    HTIMInvoice.Add("@ExtraamountoneTitle", ExtraamountoneTitle);
                    HTIMInvoice.Add("@ExtraamounttwoTitle", ExtraamounttwoTitle);
                    HTIMInvoice.Add("@DiscountTitle", DiscountTitle);

                    HTIMInvoice.Add("@DiscountTwoTitle", DiscountTwoTitle);
                    HTIMInvoice.Add("@BillNum", BillNum);
                    #endregion End Part Four

                    #region Begin Part Five on 10/03/2014

                    HTIMInvoice.Add("@STMachinary", STMachinary);
                    HTIMInvoice.Add("@STMaterial", STMaterial);
                    HTIMInvoice.Add("@STMaintenance", STMaintenance);
                    HTIMInvoice.Add("@STExtraone", STExtraone);
                    HTIMInvoice.Add("@STExtratwo", STExtratwo);



                    HTIMInvoice.Add("@SCMachinary", SCMachinary);
                    HTIMInvoice.Add("@SCMaterial", SCMaterial);
                    HTIMInvoice.Add("@SCMaintenance", SCMaintenance);
                    HTIMInvoice.Add("@SCExtraone", SCExtraone);
                    HTIMInvoice.Add("@SCExtratwo", SCExtratwo);

                    HTIMInvoice.Add("@STDiscountone", STDiscountone);
                    HTIMInvoice.Add("@STDiscounttwo", STDiscounttwo);

                    var remarkText = txtRemarks.Text;

                    HTIMInvoice.Add("@Remarks", remarkText);

                    #endregion

                    #endregion Begin Code For Define & Assign Values To The Hash Table as on [08-03-2014]

                    #region Begin Code For Calling Stored Procedure As on [08-05-2014]
                    DataTable DtIMInvoice = config.ExecuteAdaptorAsyncWithParams(SPName, HTIMInvoice).Result;
                    DisplayDataInGrid();
                    #endregion End Code For Calling Stored Procedure As on [08-05-2014]
                }
                else
                {
                    ManualBillGenerateMethod();

                }

                EnabledFields();
                visiblebutton();

            }
            catch (Exception ex)
            {

            }
        }


        private string BillnoAutoGenrate(bool StType, string unitId, int month)
        {



            string billno = "00001";
            string ContractID = "";


            string strBillprefix = "select isnull(gst.BillPrefix,'') as BillPrefix from Clients c inner join GSTMaster gst on gst.Id=c.OurGSTIN where c.ClientId  = '" + unitId + "' ";
            DataTable dtBillPrefix = config.ExecuteReaderWithQueryAsync(strBillprefix).Result;
            string billPrefix = "";
            if (dtBillPrefix.Rows.Count > 0)
            {
                billPrefix = dtBillPrefix.Rows[0]["BillPrefix"].ToString();
            }


            if (ddlType.SelectedIndex == 0)
            {
                #region for Normal Billing
                string strQry = "Select BillNo from UnitBill where UnitId='" + unitId + "' And Month=" + month;
                DataTable noTable = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
                if (noTable.Rows.Count > 0)
                {
                    if (noTable.Rows[0]["billno"].ToString().Length > 0)
                    {
                        billno = noTable.Rows[0]["billno"].ToString();
                    }
                }
                else
                {
                    if (StType)
                    {
                        //string billPrefix = GlobalData.Instance.GetBillPrefix(false);
                        string billStartNo = GlobalData.Instance.GetBillStartingNo(false);
                        string billSeq = GlobalData.Instance.GetBillSequence();
                        billno = billPrefix + billSeq + "/" + billStartNo;

                        int startingNumberPart = billno.Length - 5 + 1;

                        string selectqueryclientid = "select MAX( RIGHT(billno,5)) as billno from unitbill where billno like '"
                             + billPrefix + "%" + billSeq + "%'";
                        DataTable dt = config.ExecuteReaderWithQueryAsync(selectqueryclientid).Result;
                        int BILLNO = 0;
                        int BILLNOMB = 0;
                        if (dt.Rows.Count > 0)
                        {
                            if (String.IsNullOrEmpty(dt.Rows[0]["billno"].ToString()) == false)
                                BILLNO = int.Parse(dt.Rows[0]["billno"].ToString());
                        }

                        string selectqueryclientidMB = "select MAX( RIGHT(billno,5)) as billno from Munitbill where billno like '"
                             + billPrefix + "%" + billSeq + "%'";
                        DataTable dtMB = config.ExecuteReaderWithQueryAsync(selectqueryclientidMB).Result;

                        if (dtMB.Rows.Count > 0)
                        {

                            if (String.IsNullOrEmpty(dtMB.Rows[0]["billno"].ToString()) == false)
                                BILLNOMB = int.Parse(dtMB.Rows[0]["billno"].ToString());
                        }

                        if (BILLNO > BILLNOMB)
                        {
                            billno = billPrefix + billSeq + "/" + (Convert.ToInt32(BILLNO) + 1).ToString("00000");
                        }
                        else
                        {
                            billno = billPrefix + billSeq + "/" + (Convert.ToInt32(BILLNOMB) + 1).ToString("00000");
                        }

                    }
                    else
                    {
                        //string billPrefix = GlobalData.Instance.GetBillPrefix(true);
                        string billStartNo = GlobalData.Instance.GetBillStartingNo(true);
                        string billSeq = GlobalData.Instance.GetBillSequence();
                        billno = billSeq + "/" + billStartNo;
                        int startingNumberPart = billno.Length - 5 + 1;
                        int BILLNO = 0;
                        int BILLNOMB = 0;
                        string selectqueryclientid = "select MAX( RIGHT(billno,5)) as billno from unitbill where billno like '"
                          + billPrefix + "%" + billSeq + "%'";
                        DataTable dt = config.ExecuteReaderWithQueryAsync(selectqueryclientid).Result;

                        if (dt.Rows.Count > 0)
                        {
                            if (String.IsNullOrEmpty(dt.Rows[0]["billno"].ToString()) == false)
                                BILLNO = int.Parse(dt.Rows[0]["billno"].ToString());
                        }

                        string selectqueryclientidMB = "select MAX( RIGHT(billno,5)) as billno from Munitbill where billno like '"
                          + billPrefix + "%" + billSeq + "%'";
                        DataTable dtMB = config.ExecuteReaderWithQueryAsync(selectqueryclientidMB).Result;

                        if (dtMB.Rows.Count > 0)
                        {

                            if (String.IsNullOrEmpty(dtMB.Rows[0]["billno"].ToString()) == false)
                                BILLNOMB = int.Parse(dtMB.Rows[0]["billno"].ToString());
                        }

                        if (BILLNO > BILLNOMB)
                        {
                            billno = billPrefix + billSeq + "/" + (Convert.ToInt32(BILLNO) + 1).ToString("00000");
                        }
                        else
                        {
                            billno = billPrefix + billSeq + "/" + (Convert.ToInt32(BILLNOMB) + 1).ToString("00000");
                        }

                    }
                }
                return billno;
            }
            #endregion

            else
            {
                #region for Manual / Arrear Billing

                if (StType)
                {
                    //string billPrefix = GlobalData.Instance.GetBillPrefix(false);
                    string billStartNo = GlobalData.Instance.GetBillStartingNo(false);
                    string billSeq = GlobalData.Instance.GetBillSequence();
                    billno = billPrefix + billSeq + "/" + billStartNo;

                    int startingNumberPart = billno.Length - 5 + 1;

                    string selectqueryclientid = "select MAX( RIGHT(billno,5)) as billno from unitbill where billno like '"
                         + billPrefix + "%" + billSeq + "%'";
                    DataTable dt = config.ExecuteReaderWithQueryAsync(selectqueryclientid).Result;
                    int BILLNO = 0;
                    int BILLNOMB = 0;
                    if (dt.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(dt.Rows[0]["billno"].ToString()) == false)
                            BILLNO = int.Parse(dt.Rows[0]["billno"].ToString());
                    }

                    string selectqueryclientidMB = "select MAX( RIGHT(billno,5)) as billno from Munitbill where billno like '"
                         + billPrefix + "%" + billSeq + "%'";
                    DataTable dtMB = config.ExecuteReaderWithQueryAsync(selectqueryclientidMB).Result;

                    if (dtMB.Rows.Count > 0)
                    {

                        if (String.IsNullOrEmpty(dtMB.Rows[0]["billno"].ToString()) == false)
                            BILLNOMB = int.Parse(dtMB.Rows[0]["billno"].ToString());
                    }

                    if (BILLNO > BILLNOMB)
                    {
                        billno = billPrefix + billSeq + "/" + (Convert.ToInt32(BILLNO) + 1).ToString("00000");
                    }
                    else
                    {
                        billno = billPrefix + billSeq + "/" + (Convert.ToInt32(BILLNOMB) + 1).ToString("00000");
                    }

                }
                else
                {
                    //string billPrefix = GlobalData.Instance.GetBillPrefix(true);
                    string billStartNo = GlobalData.Instance.GetBillStartingNo(true);
                    string billSeq = GlobalData.Instance.GetBillSequence();
                    billno = billSeq + "/" + billStartNo;
                    int startingNumberPart = billno.Length - 5 + 1;
                    int BILLNO = 0;
                    int BILLNOMB = 0;
                    string selectqueryclientid = "select MAX( RIGHT(billno,5)) as billno from unitbill where billno like '"
                      + billPrefix + "%" + billSeq + "%'";
                    DataTable dt = config.ExecuteReaderWithQueryAsync(selectqueryclientid).Result;

                    if (dt.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(dt.Rows[0]["billno"].ToString()) == false)
                            BILLNO = int.Parse(dt.Rows[0]["billno"].ToString());
                    }

                    string selectqueryclientidMB = "select MAX( RIGHT(billno,5)) as billno from Munitbill where billno like '"
                      + billPrefix + "%" + billSeq + "%'";
                    DataTable dtMB = config.ExecuteReaderWithQueryAsync(selectqueryclientidMB).Result;

                    if (dtMB.Rows.Count > 0)
                    {

                        if (String.IsNullOrEmpty(dtMB.Rows[0]["billno"].ToString()) == false)
                            BILLNOMB = int.Parse(dtMB.Rows[0]["billno"].ToString());
                    }

                    if (BILLNO > BILLNOMB)
                    {
                        billno = billPrefix + billSeq + "/" + (Convert.ToInt32(BILLNO) + 1).ToString("00000");
                    }
                    else
                    {
                        billno = billPrefix + billSeq + "/" + (Convert.ToInt32(BILLNOMB) + 1).ToString("00000");
                    }

                }
                return billno;

                #endregion for Manual / Arrear Billing
            }
        }

        protected void FillMonthDetails()
        {

            if (Chk_Month.Checked == true)
            {
                if (txtmonth.Text.Trim().Length == 0)
                {
                    return;
                }
                if (Timings.Instance.CheckEnteredDate(txtmonth.Text) == 1)
                {
                    return;
                }
            }
            else
            {
                if (ddlmonth.SelectedIndex == 0)
                {
                    return;
                }
            }
            DateTime DtLastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlmonth.SelectedIndex);
            var ContractID = "";
            var bBillDates = 0;
            var ServiceTaxType = false;
            #region  Begin Get Contract Id Based on The Last Day


            Hashtable HtGetContractID = new Hashtable();
            var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
            HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
            HtGetContractID.Add("@LastDay", DtLastDay);
            DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

            if (DTContractID.Rows.Count > 0)
            {
                ContractID = DTContractID.Rows[0]["contractid"].ToString();
                bBillDates = int.Parse(DTContractID.Rows[0]["BillDates"].ToString());
                ServiceTaxType = bool.Parse(DTContractID.Rows[0]["ServiceTaxType"].ToString());

                string ContractStartDate = DTContractID.Rows[0]["ContractStartDate"].ToString();
                string ContractEndDate = DTContractID.Rows[0]["ContractEndDate"].ToString();
                string strBillDates = DTContractID.Rows[0]["BillDates"].ToString();

                DateTime CSdate = DateTime.Parse(ContractStartDate);
                DateTime CurrentDate = DateTime.Now.Date;
                DateTime lastDay = DateTime.Now.Date;
                int monval = GetMonthBasedOnSelectionDateorMonth();
                string mntchk = "0";
                if (monval.ToString().Length == 3)
                {
                    mntchk = monval.ToString().Substring(0, 1);

                }
                else if (monval.ToString().Length == 4)
                {
                    mntchk = monval.ToString().Substring(0, 2);

                }
                if (Chk_Month.Checked == false)
                {
                    if (ddlmonth.SelectedIndex == 1)
                    {
                        CurrentDate = CurrentDate.AddMonths(0);
                        lastDay = Timings.Instance.GetLastDayOfThisMonth();
                        txtyear.Text = GetMonthOfYear();
                    }
                    else if (ddlmonth.SelectedIndex == 2)
                    {
                        txtyear.Text = GetMonthOfYear();

                        if (CurrentDate.Month == 1)
                        {
                            CurrentDate = CurrentDate.AddMonths(11);
                            CurrentDate = CurrentDate.AddYears(-1);

                        }
                        else
                        {
                            CurrentDate = CurrentDate.AddMonths(-1);
                        }

                        lastDay = Timings.Instance.GetLastDayOfPreviousMonth();
                    }
                    else if (ddlmonth.SelectedIndex == 3)
                    {
                        txtyear.Text = GetMonthOfYear();
                        if (CurrentDate.Month == 2)
                        {
                            CurrentDate = CurrentDate.AddMonths(10);
                            CurrentDate = CurrentDate.AddYears(-1);
                        }
                        else
                        {
                            CurrentDate = CurrentDate.AddMonths(-2);
                        }

                        lastDay = Timings.Instance.GetLastDayOfPreviousOneMonth();
                    }
                    else if (ddlmonth.SelectedIndex == 4)
                    {
                        txtyear.Text = GetMonthOfYear();
                        if (CurrentDate.Month == 3)
                        {
                            CurrentDate = CurrentDate.AddMonths(9);
                            CurrentDate = CurrentDate.AddYears(-1);
                        }
                        else
                        {
                            CurrentDate = CurrentDate.AddMonths(-3);
                        }
                        lastDay = Timings.Instance.GetLastDayOfPreviousTwoMonth();
                    }
                }

                #region  Begin Old Code As on [02-03-2014]


                if (Chk_Month.Checked == true)
                {
                    DateTime sdate = DateTime.Now.Date;
                    int month = 0;
                    int year = 0;


                    if (txtmonth.Text.Trim().Length > 0)
                    {
                        sdate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb"));
                    }

                    month = sdate.Month;
                    year = sdate.Year;
                    DateTime finalday = new DateTime(sdate.Year, sdate.Month, DateTime.DaysInMonth(sdate.Year, sdate.Month));

                    CurrentDate = sdate;
                    lastDay = finalday;
                    txtyear.Text = year.ToString();
                }



                DateTime CEdate = DateTime.Parse(ContractEndDate);
                if (CSdate <= lastDay)
                {
                    if (bBillDates == 1)
                    {

                        if (CurrentDate.Month == 2 && (CSdate.Day > 28 || CSdate.Day > 29))
                        {
                            txtfromdate.Text = (new DateTime(CurrentDate.Year, CurrentDate.Month, 28).ToString("dd/MM/yyyy"));
                        }
                        if (CurrentDate.Month == 1)
                        {
                            txtfromdate.Text = (new DateTime(CurrentDate.Year, 12, CSdate.Day).ToString("dd/MM/yyyy"));
                        }
                        else
                        {
                            txtfromdate.Text = (new DateTime(CurrentDate.Year, CurrentDate.Month - 1, CSdate.Day).ToString("dd/MM/yyyy"));

                        }
                        DateTime tempDate = CurrentDate.AddMonths(1);

                        if (CSdate.Day == 1)
                        {
                            txttodate.Text = (new DateTime(tempDate.Year, tempDate.Month, CSdate.Day).ToString("dd/MM/yyyy"));
                        }
                        else
                            if (CurrentDate.Month == 1)
                        {
                            txttodate.Text = (new DateTime(tempDate.Year, 1, CSdate.Day - 1).ToString("dd/MM/yyyy"));
                        }
                        else
                                if (tempDate.Month == 1)
                        {
                            txttodate.Text = (new DateTime(tempDate.Year, 12, CSdate.Day - 1).ToString("dd/MM/yyyy"));
                        }
                        else
                        {
                            txttodate.Text = (new DateTime(tempDate.Year, tempDate.Month - 1, CSdate.Day - 1).ToString("dd/MM/yyyy"));
                        }

                    }
                    if (bBillDates == 0)
                    {
                        txtfromdate.Text = (new DateTime(CurrentDate.Year, CurrentDate.Month/* - 1*/, 1).ToString("dd/MM/yyyy"));

                        if (CSdate.Day == 1)
                        {
                            txttodate.Text = (new DateTime(CurrentDate.Year, CurrentDate.Month/* - 1*/, DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month)).ToString("dd/MM/yyyy"));
                        }
                        else
                        {
                            txttodate.Text = (new DateTime(CurrentDate.Year, CurrentDate.Month/* - 1*/, DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month)).ToString("dd/MM/yyyy"));

                        }
                    }



                    if (bBillDates == 2)
                    {
                        if (CurrentDate.Month == 1)
                        {
                            txtfromdate.Text = (new DateTime(CurrentDate.Year - 1, 12, 26).ToString("dd/MM/yyyy"));
                        }
                        else

                            txtfromdate.Text = (new DateTime(CurrentDate.Year, CurrentDate.Month - 1, 26).ToString("dd/MM/yyyy"));

                        DateTime tempDate = CurrentDate.AddMonths(1);

                        if (mntchk == "12")
                        {
                            tempDate = CurrentDate;
                            txttodate.Text = (new DateTime(tempDate.Year, tempDate.Month, 25).ToString("dd/MM/yyyy"));

                        }
                        else
                        {
                            if (tempDate.Month == 1)
                            {
                                txttodate.Text = (new DateTime(tempDate.Year, 12, 25).ToString("dd/MM/yyyy"));
                            }
                            else
                            {
                                txttodate.Text = (new DateTime(tempDate.Year, tempDate.Month - 1, 25).ToString("dd/MM/yyyy"));
                            }
                        }

                    }


                    if (bBillDates == 3)
                    {
                        if (CurrentDate.Month == 1)
                        {
                            txtfromdate.Text = (new DateTime(CurrentDate.Year - 1, 12, 21).ToString("dd/MM/yyyy"));
                        }
                        else

                            txtfromdate.Text = (new DateTime(CurrentDate.Year, CurrentDate.Month - 1, 21).ToString("dd/MM/yyyy"));
                        DateTime tempDate = CurrentDate.AddMonths(1);

                        if (mntchk == "12")
                        {
                            tempDate = CurrentDate;
                            txttodate.Text = (new DateTime(tempDate.Year, tempDate.Month, 20).ToString("dd/MM/yyyy"));

                        }
                        else
                        {
                            if (tempDate.Month == 1)
                            {
                                txttodate.Text = (new DateTime(tempDate.Year, 12, 20).ToString("dd/MM/yyyy"));
                            }
                            else
                            {
                                txttodate.Text = (new DateTime(tempDate.Year, tempDate.Month - 1, 20).ToString("dd/MM/yyyy"));
                            }
                        }

                    }



                    //if (CurrentDate.Month/* - 1*/ == CSdate.Month && CurrentDate.Year == CSdate.Year)
                    //{
                    //    DateTime date = new DateTime(CurrentDate.Year, CurrentDate.Month/* - 1*/, CSdate.Day);
                    //    txtfromdate.Text = date.ToString("dd/MM/yyyy");
                    //}
                    if (CurrentDate.Month/* - 1*/ == CEdate.Month && CurrentDate.Year == CEdate.Year)
                    {
                        DateTime date = DateTime.Now.Date;
                        if (CSdate.Day == 1)
                        {
                            date = new DateTime(CurrentDate.Year, CurrentDate.Month/* - 1*/, CEdate.Day);
                        }
                        else
                        {
                            date = new DateTime(CurrentDate.Year, CurrentDate.Month/* - 1*/, CEdate.Day - 1);

                        }

                        txttodate.Text = date.ToString("dd/MM/yyyy");
                    }
                    btngenratepayment.Enabled = true;
                }
                else
                {
                    btngenratepayment.Enabled = false;
                    //LblResult.Text = "There Is No Valid Contract for this month";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' There Is No Valid Contract for this month ');", true);

                }


                #endregion End Old Code As on [02-03-2014]


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not  Avaialable For This Client.');", true);
                return;
            }

            #endregion  End Get Contract Id Based on The Last Day
        }

        protected void btndownloadpdffile_Click(object sender, EventArgs e)
        {
            //if (ddlpaymenttype.SelectedIndex == 0)
            //{
            btnmhb_Click(sender, e);
            return;
            //}

            //if (ddlpaymenttype.SelectedIndex == 1)
            //{
            //    btnall_Click(sender, e);
            //    return;
            //}





        }

        protected void btnmhb_Click(object sender, EventArgs e)
        {
            int month = 0;
            month = GetMonthBasedOnSelectionDateorMonth();
            if (gvClientBilling.Rows.Count > 0)
            {
                try
                {
                    MemoryStream ms = new MemoryStream();
                    Document document = new Document(PageSize.A4);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    PageEventHelper pageEventHelper = new PageEventHelper();
                    writer.PageEvent = pageEventHelper;
                    document.Open();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    string strQry = "Select * from CompanyInfo   where  branchid='" + BranchID + "'";
                    DataTable compInfo = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
                    string companyName = "Your Company Name";
                    string companyAddress = "Your Company Address";
                    string companyaddressline = " ";
                    if (compInfo.Rows.Count > 0)
                    {
                        companyName = compInfo.Rows[0]["CompanyName"].ToString();
                        companyAddress = compInfo.Rows[0]["Address"].ToString();
                        companyaddressline = compInfo.Rows[0]["Addresslineone"].ToString();
                    }


                    DateTime DtLastDay = DateTime.Now;
                    if (Chk_Month.Checked == false)
                    {
                        DtLastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlmonth.SelectedIndex);
                    }
                    if (Chk_Month.Checked == true)
                    {
                        DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                    }
                    var ContractID = "";


                    #region  Begin Get Contract Id Based on The Last Day

                    Hashtable HtGetContractID = new Hashtable();
                    var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                    HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
                    HtGetContractID.Add("@LastDay", DtLastDay);
                    DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                    if (DTContractID.Rows.Count > 0)
                    {
                        ContractID = DTContractID.Rows[0]["contractid"].ToString();

                    }
                    #endregion

                    string SqlQuryForServiCharge = "select ContractId,servicecharge,ServiceChargeType,Description,IncludeST,ServiceTax75 from contracts where " +
                        " clientid ='" + ddlclientid.SelectedValue + "' and ContractId='" + ContractID + "'";
                    DataTable DtServicecharge = config.ExecuteAdaptorAsyncWithQueryParams(SqlQuryForServiCharge).Result;
                    string ServiceCharge = "0";
                    string strSCType = "";
                    string strDescription = "We are presenting our bill for the House Keeping Services Provided at your establishment. Kindly release the payment at the earliest";
                    bool bSCType = false;
                    string strIncludeST = "";
                    string strST75 = "";
                    bool bIncludeST = false;
                    bool bST75 = false;
                    if (DtServicecharge.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(DtServicecharge.Rows[0]["ServiceCharge"].ToString()) == false)
                        {
                            ServiceCharge = DtServicecharge.Rows[0]["ServiceCharge"].ToString();
                        }
                        if (String.IsNullOrEmpty(DtServicecharge.Rows[0]["ServiceChargeType"].ToString()) == false)
                        {
                            strSCType = DtServicecharge.Rows[0]["ServiceChargeType"].ToString();
                        }
                        string tempDescription = DtServicecharge.Rows[0]["Description"].ToString();
                        if (tempDescription.Trim().Length > 0)
                        {
                            strDescription = tempDescription;
                        }
                        if (strSCType.Length > 0)
                        {
                            bSCType = Convert.ToBoolean(strSCType);
                        }
                        strIncludeST = DtServicecharge.Rows[0]["IncludeST"].ToString();
                        strST75 = DtServicecharge.Rows[0]["ServiceTax75"].ToString();
                        if (strIncludeST == "True")
                        {
                            bIncludeST = true;
                        }
                        if (strST75 == "True")
                        {
                            bST75 = true;
                        }
                    }
                    document.AddTitle(companyName);
                    document.AddAuthor("DIYOS");
                    document.AddSubject("Invoice");
                    document.AddKeywords("Keyword1, keyword2, …");
                    string imagepath = Server.MapPath("~/assets/Billinglogo.jpg");
                    if (File.Exists(imagepath))
                    {
                        iTextSharp.text.Image gif2 = iTextSharp.text.Image.GetInstance(imagepath);

                        gif2.Alignment = (iTextSharp.text.Image.ALIGN_LEFT | iTextSharp.text.Image.UNDERLYING);
                        // gif2.SpacingBefore = 50;
                        gif2.ScalePercent(70f);
                        gif2.SetAbsolutePosition(34f, 755f);
                        //document.Add(new Paragraph(" "));
                        document.Add(gif2);
                    }

                    PdfPTable tablelogo = new PdfPTable(2);
                    tablelogo.TotalWidth = 350f;
                    tablelogo.LockedWidth = true;
                    float[] widtlogo = new float[] { 2f, 2f };
                    tablelogo.SetWidths(widtlogo);


                    //tablelogo.AddCell(celll);
                    PdfPCell CCompName = new PdfPCell(new Paragraph(companyName, FontFactory.GetFont(FontStyle, 18, Font.BOLD, BaseColor.BLACK)));
                    CCompName.HorizontalAlignment = 1;
                    CCompName.Border = 0;
                    CCompName.Colspan = 2;
                    CCompName.PaddingTop = -20;
                    tablelogo.AddCell(CCompName);

                    PdfPCell CCompAddress = new PdfPCell(new Paragraph(companyAddress, FontFactory.GetFont(FontStyle, 11, Font.BOLD, BaseColor.BLACK)));
                    CCompAddress.HorizontalAlignment = 1;
                    CCompAddress.Border = 0;
                    CCompAddress.Colspan = 2;
                    tablelogo.AddCell(CCompAddress);

                    PdfPCell cellline = new PdfPCell(new Paragraph(companyaddressline, FontFactory.GetFont(FontStyle, 12, Font.NORMAL, BaseColor.BLACK)));
                    cellline.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cellline.Border = 0;
                    cellline.Colspan = 2;
                    tablelogo.AddCell(cellline);
                    //For Space

                    PdfPCell celll = new PdfPCell(new Paragraph(" ", FontFactory.GetFont(FontStyle, 12, Font.NORMAL, BaseColor.BLACK)));
                    celll.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    celll.Border = 0;
                    celll.Colspan = 2;
                    tablelogo.AddCell(celll);
                    //tablelogo.AddCell(celll);

                    // tablelogo.AddCell(celll);

                    PdfPCell CInvoice = new PdfPCell(new Paragraph("INVOICE", FontFactory.GetFont(FontStyle, 18, Font.UNDERLINE | Font.BOLD, BaseColor.BLACK)));
                    CInvoice.HorizontalAlignment = 1;
                    CInvoice.PaddingTop = 30;
                    CInvoice.Border = 0;
                    CInvoice.Colspan = 2;
                    tablelogo.AddCell(CInvoice);

                    //tablelogo.AddCell(celll);

                    document.Add(tablelogo);

                    PdfPTable address = new PdfPTable(2);
                    address.TotalWidth = 500f;
                    address.LockedWidth = true;
                    float[] addreslogo = new float[] { 2f, 2f };
                    address.SetWidths(addreslogo);

                    PdfPTable tempTable1 = new PdfPTable(1);
                    tempTable1.TotalWidth = 250f;
                    tempTable1.LockedWidth = true;
                    float[] tempWidth1 = new float[] { 1f };
                    tempTable1.SetWidths(tempWidth1);

                    string selectclientaddress = "select * from clients where clientid= '" + ddlclientid.SelectedItem.ToString() + "'";
                    DataTable dtclientaddress = config.ExecuteAdaptorAsyncWithQueryParams(selectclientaddress).Result;

                    string Unitbillbreakup = "select sum(isnull(BasicDA,0)) as BasicDA ,sum(isnull(Basic,0)) as Basic,sum(isnull(Da,0)) as DA,sum(isnull(HRA,0)) as HRA,sum(isnull(Conveyance,0)) as Conveyance,sum(isnull(WashAllowance,0)) as WashAllowance, " +
                                                "sum(isnull(OtherAllowance,0)) as OtherAllowance,sum(isnull(ServiceCharges,0)) as ServiceCharges,sum(isnull(bonus,0)) as bonus,sum(isnull(AttendanceAllowance,0)) as AttendanceAllowance,sum(isnull(Arrears,0)) as Arrears, " +
                                                "sum(isnull(ESITotal,0)) as ESITotal,sum(isnull(Encashamt,0)) as Encashamt,sum(PF) as PF,sum(ESI) as ESI from UnitBillBreakup where month='" + month + "' and unitid='" + ddlclientid.SelectedValue + "'";
                    DataTable DtUnitbillbreakup = config.ExecuteAdaptorAsyncWithQueryParams(Unitbillbreakup).Result;

                    float totalbasic = 0;
                    float totalhra = 0;
                    float totalconveyance = 0;
                    float totalotherallowance = 0;
                    float AttendanceAllowance = 0;
                    float TotalWashingAllowance = 0;
                    float Totalbonus = 0;
                    float TotalEnchashement = 0;
                    float BillTotal = 0;
                    float esitotal = 0;
                    float esiempr = 0;
                    float pfempr = 0;
                    float managementfee = 0;
                    float Arrears = 0;

                    if (DtUnitbillbreakup.Rows.Count > 0)
                    {
                        totalbasic = Convert.ToSingle(DtUnitbillbreakup.Rows[0]["Basic"].ToString());
                        totalhra = Convert.ToSingle(DtUnitbillbreakup.Rows[0]["HRA"].ToString());
                        totalconveyance = Convert.ToSingle(DtUnitbillbreakup.Rows[0]["Conveyance"].ToString());
                        totalotherallowance = Convert.ToSingle(DtUnitbillbreakup.Rows[0]["OtherAllowance"].ToString());
                        AttendanceAllowance = Convert.ToSingle(DtUnitbillbreakup.Rows[0]["AttendanceAllowance"].ToString());
                        TotalWashingAllowance = Convert.ToSingle(DtUnitbillbreakup.Rows[0]["WashAllowance"].ToString());
                        Totalbonus = Convert.ToSingle(DtUnitbillbreakup.Rows[0]["bonus"].ToString());
                        TotalEnchashement = Convert.ToSingle(DtUnitbillbreakup.Rows[0]["Encashamt"].ToString());
                        BillTotal = Convert.ToSingle(DtUnitbillbreakup.Rows[0]["BasicDA"].ToString());
                        esitotal = Convert.ToSingle(DtUnitbillbreakup.Rows[0]["ESITotal"].ToString());
                        esiempr = Convert.ToSingle(DtUnitbillbreakup.Rows[0]["PF"].ToString());
                        pfempr = Convert.ToSingle(DtUnitbillbreakup.Rows[0]["ESI"].ToString());
                        managementfee = Convert.ToSingle(DtUnitbillbreakup.Rows[0]["ServiceCharges"].ToString());
                        Arrears = Convert.ToSingle(DtUnitbillbreakup.Rows[0]["Arrears"].ToString());

                    }


                    string SelectBillNo = "Select * from Unitbill where month='" + month + "' and unitid='" + ddlclientid.SelectedValue + "'";
                    DataTable DtBilling = config.ExecuteAdaptorAsyncWithQueryParams(SelectBillNo).Result;
                    string BillNo = "";
                    DateTime BillDate;
                    DateTime DueDate;

                    #region Variables for data Fields as on 11/03/2014 by venkat


                    float servicecharge = 0;
                    float servicetax = 0;
                    float cess = 0;
                    float shecess = 0;
                    float totalamount = 0;
                    float Grandtotal = 0;

                    float ServiceTax75 = 0;
                    float ServiceTax25 = 0;

                    float machinarycost = 0;
                    float materialcost = 0;
                    float maintenancecost = 0;
                    float extraonecost = 0;
                    float extratwocost = 0;
                    float discountone = 0;
                    float discounttwo = 0;

                    string machinarycosttitle = "";
                    string materialcosttitle = "";
                    string maintenancecosttitle = "";
                    string extraonecosttitle = "";
                    string extratwocosttitle = "";
                    string discountonetitle = "";
                    string discounttwotitle = "";

                    bool Extradatacheck = false;
                    bool ExtraDataSTcheck = false;

                    bool STMachinary = false;
                    bool STMaterial = false;
                    bool STMaintenance = false;
                    bool STExtraone = false;
                    bool STExtratwo = false;

                    bool SCMachinary = false;
                    bool SCMaterial = false;
                    bool SCMaintenance = false;
                    bool SCExtraone = false;
                    bool SCExtratwo = false;

                    bool STDiscountone = false;
                    bool STDiscounttwo = false;

                    string strExtradatacheck = "";
                    string strExtrastcheck = "";

                    string strSTMachinary = "";
                    string strSTMaterial = "";
                    string strSTMaintenance = "";
                    string strSTExtraone = "";
                    string strSTExtratwo = "";

                    string strSCMachinary = "";
                    string strSCMaterial = "";
                    string strSCMaintenance = "";
                    string strSCExtraone = "";
                    string strSCExtratwo = "";

                    string strSTDiscountone = "";
                    string strSTDiscounttwo = "";

                    float staxamtonservicecharge = 0;
                    float RelChrgAmt = 0;


                    #endregion

                    if (DtBilling.Rows.Count > 0)
                    {
                        BillNo = DtBilling.Rows[0]["billno"].ToString();
                        BillDate = Convert.ToDateTime(DtBilling.Rows[0]["billdt"].ToString());
                        DueDate = Convert.ToDateTime(DtBilling.Rows[0]["duedt"].ToString());

                        #region Begin New code for values taken from database as on 11/03/2014 by venkat

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["dutiestotalamount"].ToString()) == false)
                        {
                            totalamount = float.Parse(DtBilling.Rows[0]["dutiestotalamount"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["RelChrgAmt"].ToString()) == false)
                        {
                            RelChrgAmt = float.Parse(DtBilling.Rows[0]["RelChrgAmt"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["TotalServiceChargeAmt"].ToString()) == false)
                        {
                            servicecharge = float.Parse(DtBilling.Rows[0]["TotalServiceChargeAmt"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax"].ToString()) == false)
                        {
                            servicetax = float.Parse(DtBilling.Rows[0]["ServiceTax"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["CESS"].ToString()) == false)
                        {
                            cess = float.Parse(DtBilling.Rows[0]["CESS"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["SHECess"].ToString()) == false)
                        {
                            shecess = float.Parse(DtBilling.Rows[0]["SHECess"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["GrandTotal"].ToString()) == false)
                        {
                            Grandtotal = float.Parse(DtBilling.Rows[0]["GrandTotal"].ToString());
                        }



                        #endregion
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Generate The Bill Once Again');", true);
                        return;
                    }
                    string Year = DateTime.Now.Year.ToString();

                    PdfPCell cell11 = new PdfPCell(new Paragraph("To,", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cell11.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell11.Border = 0;
                    tempTable1.AddCell(cell11);
                    string addressData = "";

                    addressData = dtclientaddress.Rows[0]["ClientAddrHno"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientaddrhno = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        clientaddrhno.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        //clientaddrhno.Colspan = 0;
                        clientaddrhno.Border = 0;
                        tempTable1.AddCell(clientaddrhno);
                    }
                    addressData = dtclientaddress.Rows[0]["ClientAddrStreet"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientstreet.Border = 0;
                        tempTable1.AddCell(clientstreet);
                    }


                    addressData = dtclientaddress.Rows[0]["ClientAddrArea"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientstreet.Border = 0;
                        tempTable1.AddCell(clientstreet);
                    }


                    addressData = dtclientaddress.Rows[0]["ClientAddrColony"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientcolony = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        clientcolony.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientcolony.Colspan = 2;
                        clientcolony.Border = 0;
                        tempTable1.AddCell(clientcolony);
                    }
                    addressData = dtclientaddress.Rows[0]["ClientAddrcity"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientcity = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        clientcity.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientcity.Colspan = 2;
                        clientcity.Border = 0;
                        tempTable1.AddCell(clientcity);
                    }
                    addressData = dtclientaddress.Rows[0]["ClientAddrstate"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientstate = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        clientstate.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientstate.Colspan = 2;
                        clientstate.Border = 0;
                        tempTable1.AddCell(clientstate);
                    }
                    addressData = dtclientaddress.Rows[0]["ClientAddrpin"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clietnpin = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clietnpin.Colspan = 2;
                        clietnpin.Border = 0;
                        tempTable1.AddCell(clietnpin);
                    }
                    PdfPCell childTable1 = new PdfPCell(tempTable1);
                    childTable1.Border = 0;
                    childTable1.HorizontalAlignment = 0;
                    address.AddCell(childTable1);

                    PdfPTable tempTable2 = new PdfPTable(1);
                    tempTable2.TotalWidth = 250f;
                    tempTable2.LockedWidth = true;
                    float[] tempWidth2 = new float[] { 1f };
                    tempTable2.SetWidths(tempWidth2);

                    //Blank Field for Space
                    PdfPCell cell12 = new PdfPCell(new Paragraph(" ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cell12.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell12.Border = 0;
                    tempTable2.AddCell(cell12);

                    PdfPCell cell13 = new PdfPCell(new Paragraph("Invoice No: " + BillNo, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cell13.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell13.Border = 0;
                    tempTable2.AddCell(cell13);

                    PdfPCell cell14 = new PdfPCell(new Paragraph("Date: " + BillDate.Day.ToString("00") + "/" + BillDate.Month.ToString("00") + "/" +
                        BillDate.Year, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cell14.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell14.Border = 0;
                    tempTable2.AddCell(cell14);

                    //PdfPCell cell15 = new PdfPCell(new Paragraph("Due Date: " + DueDate.Day.ToString("00") + "/" + DueDate.Month.ToString("00") + "/" +
                    //DueDate.Year, FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
                    //cell15.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //cell15.Border = 0;
                    // tempTable2.AddCell(cell15);


                    //PdfPCell cell15 = new PdfPCell(new Paragraph("For Month: " + GetMonthName() + " - " + GetMonthOfYear() +
                    //    "      ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    //cell15.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //cell15.Border = 0;
                    //tempTable2.AddCell(cell15);

                    PdfPCell childTable2 = new PdfPCell(tempTable2);
                    childTable2.Border = 0;
                    childTable2.HorizontalAlignment = 0;
                    address.AddCell(childTable2);
                    address.AddCell(celll);


                    document.Add(address);

                    PdfPTable bodytablelogo = new PdfPTable(2);
                    bodytablelogo.TotalWidth = 500f;//600f
                    bodytablelogo.LockedWidth = true;
                    float[] widthlogo = new float[] { 2f, 2f };
                    bodytablelogo.SetWidths(widthlogo);


                    //PdfPCell cell9 = new PdfPCell(new Phrase("Unit Name : " + dtclientaddress.Rows[0]["clientname"].ToString(),
                    //    FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    //cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //cell9.Colspan = 2;
                    //cell9.Border = 0;
                    //bodytablelogo.AddCell(cell9);

                    //string Fromdate = txtfromdate.Text;
                    //string Todate = txttodate.Text;

                    //PdfPCell cell10 = new PdfPCell(new Phrase("Bill From : " + Fromdate + "  to  " +
                    //    Todate + " ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    //cell10.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //cell10.Colspan = 2;
                    //cell10.Border = 0;
                    //bodytablelogo.AddCell(cell10);
                    //bodytablelogo.AddCell(celll);

                    //PdfPCell cell19 = new PdfPCell(new Phrase("Dear Sir, ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    //cell19.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //cell19.Colspan = 2;
                    //cell19.Border = 0;
                    //bodytablelogo.AddCell(cell19);
                    //bodytablelogo.AddCell(celll);

                    //PdfPCell cell20 = new PdfPCell(new Phrase(strDescription, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    //cell20.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //cell20.Colspan = 2;
                    //cell20.Border = 0;
                    //bodytablelogo.AddCell(cell20);
                    //bodytablelogo.AddCell(celll);
                    //PdfPCell cell21 = new PdfPCell(new Phrase("The Details are given below: ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    //cell21.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //cell21.Colspan = 1;
                    //cell21.Border = 0;
                    //bodytablelogo.AddCell(cell21);
                    //bodytablelogo.AddCell(celll);
                    ////bodytablelogo.AddCell(celll);
                    //document.Add(bodytablelogo);


                    PdfPTable tabled = new PdfPTable(3);
                    tabled.TotalWidth = 500f;//600f
                    tabled.LockedWidth = true;
                    float[] tabledwidths = new float[] { 0.5f, 6f, 2f };
                    tabled.SetWidths(tabledwidths);

                    PdfPCell cell9 = new PdfPCell(new Phrase("S.No", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell9.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell9.Colspan = 0;
                    cell9.PaddingTop = 3;
                    cell9.PaddingBottom = 3;
                    //cell9.Border = 0;
                    tabled.AddCell(cell9);

                    PdfPCell cell10 = new PdfPCell(new Phrase("Particulars", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell10.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell10.Colspan = 0;
                    cell10.PaddingTop = 3;
                    cell10.PaddingBottom = 3;
                    //cell10.Border = 0;
                    tabled.AddCell(cell10);

                    PdfPCell cellAmount = new PdfPCell(new Phrase("Amount", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cellAmount.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellAmount.Colspan = 0;
                    cellAmount.PaddingTop = 3;
                    cellAmount.PaddingBottom = 3;
                    //cellAmount.Border = 0;
                    tabled.AddCell(cellAmount);


                    PdfPCell cellSno1 = new PdfPCell(new Phrase("1", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellSno1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno1.Border = 0;
                    cellSno1.PaddingTop = 3;
                    cellSno1.PaddingBottom = 3;
                    tabled.AddCell(cellSno1);


                    PdfPCell celldesc = new PdfPCell(new Phrase("Amount Claimed on Kst/Service/Production Staff Rendered to " + ddlCname.SelectedItem.Text + "  for the month of " + GetMonthName() + " - " + GetMonthOfYear(), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldesc.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    celldesc.PaddingTop = 3;
                    celldesc.PaddingBottom = 3;
                    celldesc.SetLeading(0.0f, 1.3f);
                    //celldesc.Border = 0;
                    tabled.AddCell(celldesc);

                    PdfPCell cellAmount1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellAmount1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno1.Border = 0;
                    cellAmount1.PaddingTop = 3;
                    cellAmount1.PaddingBottom = 3;
                    tabled.AddCell(cellAmount1);

                    PdfPCell cellSno2 = new PdfPCell(new Phrase("A", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cellSno2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno2.Border = 0;
                    cellSno2.PaddingTop = 3;
                    cellSno2.PaddingBottom = 3;
                    tabled.AddCell(cellSno2);


                    PdfPCell celldesc2 = new PdfPCell(new Phrase("Monthly Payments ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    celldesc2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    celldesc2.Colspan = 2;
                    celldesc2.PaddingTop = 3;
                    celldesc2.PaddingBottom = 3;
                    //celldesc2.Border = 0;
                    tabled.AddCell(celldesc2);


                    PdfPCell cellSno3 = new PdfPCell(new Phrase("1", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellSno3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno3.Border = 0;
                    cellSno3.PaddingTop = 3;
                    cellSno3.PaddingBottom = 3;
                    tabled.AddCell(cellSno3);


                    PdfPCell celldesc3 = new PdfPCell(new Phrase("Total Basic", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldesc3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    celldesc3.Colspan = 0;
                    celldesc3.PaddingTop = 3;
                    celldesc3.PaddingBottom = 3;
                    // celldesc3.Border = 0;
                    tabled.AddCell(celldesc3);

                    PdfPCell cellAmt1 = new PdfPCell(new Phrase(totalbasic.ToString(), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellAmt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    // cellAmt1.Border = 0;
                    cellAmt1.PaddingTop = 3;
                    cellAmt1.PaddingBottom = 3;
                    tabled.AddCell(cellAmt1);

                    PdfPCell cellSno4 = new PdfPCell(new Phrase("2", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellSno4.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno4.Border = 0;
                    cellSno4.PaddingTop = 3;
                    cellSno4.PaddingBottom = 3;
                    tabled.AddCell(cellSno4);


                    PdfPCell celldesc4 = new PdfPCell(new Phrase("HRA", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldesc4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    celldesc4.Colspan = 0;
                    //celldesc4.Border = 0;
                    celldesc4.PaddingTop = 3;
                    celldesc4.PaddingBottom = 3;
                    tabled.AddCell(celldesc4);

                    PdfPCell cellAmt2 = new PdfPCell(new Phrase(totalhra.ToString(), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellAmt2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //cellAmt2.Border = 0;
                    cellAmt2.PaddingTop = 3;
                    cellAmt2.PaddingBottom = 3;
                    tabled.AddCell(cellAmt2);

                    PdfPCell cellSno5 = new PdfPCell(new Phrase("3", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellSno5.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno5.Border = 0;
                    cellSno5.PaddingTop = 3;
                    cellSno5.PaddingBottom = 3;
                    tabled.AddCell(cellSno5);


                    PdfPCell celldesc5 = new PdfPCell(new Phrase("Total Conveyance", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldesc5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //celldesc5.Border = 0;
                    celldesc5.PaddingTop = 3;
                    celldesc5.PaddingBottom = 3;
                    tabled.AddCell(celldesc5);

                    PdfPCell cellAmt3 = new PdfPCell(new Phrase(totalconveyance.ToString(), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellAmt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //cellAmt3.Border = 0;
                    cellAmt3.PaddingTop = 3;
                    cellAmt3.PaddingBottom = 3;
                    tabled.AddCell(cellAmt3);


                    PdfPCell cellSno6 = new PdfPCell(new Phrase("4", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellSno6.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno6.Border = 0;
                    cellSno6.PaddingTop = 3;
                    cellSno6.PaddingBottom = 3;
                    tabled.AddCell(cellSno6);


                    PdfPCell celldesc6 = new PdfPCell(new Phrase("Total Attendance Allowance", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldesc6.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //celldesc6.Border = 0;
                    celldesc6.PaddingTop = 3;
                    celldesc6.PaddingBottom = 3;
                    tabled.AddCell(celldesc6);

                    PdfPCell cellAmt4 = new PdfPCell(new Phrase(AttendanceAllowance.ToString(), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellAmt4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //cellAmt4.Border = 0;
                    cellAmt4.PaddingTop = 3;
                    cellAmt4.PaddingBottom = 3;
                    tabled.AddCell(cellAmt4);


                    PdfPCell cellSno7 = new PdfPCell(new Phrase("5", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellSno7.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno7.Border = 0;
                    cellSno7.PaddingTop = 3;
                    cellSno7.PaddingBottom = 3;
                    tabled.AddCell(cellSno7);


                    PdfPCell celldesc7 = new PdfPCell(new Phrase("Other Allowance", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldesc7.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //celldesc7.Border = 0;
                    celldesc7.PaddingTop = 3;
                    celldesc7.PaddingBottom = 3;
                    tabled.AddCell(celldesc7);

                    PdfPCell cellAmt5 = new PdfPCell(new Phrase(totalotherallowance.ToString(), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellAmt5.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //cellAmt5.Border = 0;
                    cellAmt5.PaddingTop = 3;
                    cellAmt5.PaddingBottom = 3;
                    tabled.AddCell(cellAmt5);

                    PdfPCell cellSno8 = new PdfPCell(new Phrase("6", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellSno8.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno8.Border = 0;
                    cellSno8.PaddingTop = 3;
                    cellSno8.PaddingBottom = 3;
                    tabled.AddCell(cellSno8);


                    PdfPCell celldesc8 = new PdfPCell(new Phrase("Arreas", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldesc8.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //celldesc8.Border = 0;
                    celldesc8.PaddingTop = 3;
                    celldesc8.PaddingBottom = 3;
                    tabled.AddCell(celldesc8);

                    PdfPCell cellAmt6 = new PdfPCell(new Phrase(Arrears.ToString(), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellAmt6.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    // cellAmt6.Border = 0;
                    cellAmt6.PaddingTop = 3;
                    cellAmt6.PaddingBottom = 3;
                    tabled.AddCell(cellAmt6);

                    PdfPCell cellSno9 = new PdfPCell(new Phrase("7", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellSno9.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    // cellSno9.Border = 0;
                    cellSno9.PaddingTop = 3;
                    cellSno9.PaddingBottom = 3;
                    tabled.AddCell(cellSno9);


                    PdfPCell celldesc9 = new PdfPCell(new Phrase("Washing Allowance", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldesc9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //celldesc9.Border = 0;
                    celldesc9.PaddingTop = 3;
                    celldesc9.PaddingBottom = 3;
                    tabled.AddCell(celldesc9);

                    PdfPCell cellAmt7 = new PdfPCell(new Phrase(TotalWashingAllowance.ToString(), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellAmt7.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //cellAmt7.Border = 0;
                    cellAmt7.PaddingTop = 3;
                    cellAmt7.PaddingBottom = 3;
                    tabled.AddCell(cellAmt7);


                    PdfPCell cellSno10 = new PdfPCell(new Phrase("8", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellSno10.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno10.Border = 0;
                    cellSno10.PaddingTop = 3;
                    cellSno10.PaddingBottom = 3;
                    tabled.AddCell(cellSno10);


                    PdfPCell celldesc10 = new PdfPCell(new Phrase("Total Statutory Bonus", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldesc10.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //celldesc10.Border = 0;
                    celldesc10.PaddingTop = 3;
                    celldesc10.PaddingBottom = 3;
                    tabled.AddCell(celldesc10);

                    PdfPCell cellAmt8 = new PdfPCell(new Phrase(Totalbonus.ToString(), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellAmt8.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //cellAmt8.Border = 0;
                    cellAmt8.PaddingTop = 3;
                    cellAmt8.PaddingBottom = 3;
                    tabled.AddCell(cellAmt8);

                    PdfPCell cellSno11 = new PdfPCell(new Phrase("9", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellSno11.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno11.Border = 0;
                    cellSno11.PaddingTop = 3;
                    cellSno11.PaddingBottom = 3;
                    tabled.AddCell(cellSno11);


                    PdfPCell celldesc11 = new PdfPCell(new Phrase("Total EL Encashment", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldesc11.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //celldesc11.Border = 0;
                    celldesc11.PaddingTop = 3;
                    celldesc11.PaddingBottom = 3;
                    tabled.AddCell(celldesc11);

                    PdfPCell cellAmt9 = new PdfPCell(new Phrase(TotalEnchashement.ToString(), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellAmt9.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //cellAmt9.Border = 0;
                    cellAmt9.PaddingTop = 3;
                    cellAmt9.PaddingBottom = 3;
                    tabled.AddCell(cellAmt9);

                    PdfPCell cellSno12 = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellSno12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno12.Border = 0;
                    cellSno12.PaddingTop = 3;
                    cellSno12.PaddingBottom = 3;
                    tabled.AddCell(cellSno12);


                    PdfPCell celldesc12 = new PdfPCell(new Phrase("TOTAL", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    celldesc12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //celldesc12.Border = 0;
                    celldesc12.PaddingTop = 3;
                    celldesc12.PaddingBottom = 3;
                    tabled.AddCell(celldesc12);

                    PdfPCell cellAmt10 = new PdfPCell(new Phrase(BillTotal.ToString(), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cellAmt10.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //cellAmt10.Border = 0;
                    cellAmt10.PaddingTop = 3;
                    cellAmt10.PaddingBottom = 3;
                    tabled.AddCell(cellAmt10);

                    PdfPCell cellSno13 = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellSno13.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno13.Border = 0;
                    cellSno13.PaddingTop = 3;
                    cellSno13.PaddingBottom = 3;
                    tabled.AddCell(cellSno13);


                    PdfPCell celldesc13 = new PdfPCell(new Phrase("ESI TOTAL", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    celldesc13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //celldesc13.Border = 0;
                    celldesc13.PaddingTop = 3;
                    celldesc13.PaddingBottom = 3;
                    tabled.AddCell(celldesc13);

                    PdfPCell cellAmt11 = new PdfPCell(new Phrase(esitotal.ToString(), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cellAmt11.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //cellAmt11.Border = 0;
                    cellAmt11.PaddingTop = 3;
                    cellAmt11.PaddingBottom = 3;
                    tabled.AddCell(cellAmt11);


                    PdfPCell cellSno14 = new PdfPCell(new Phrase("B", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cellSno14.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno14.Border = 0;
                    cellSno14.PaddingTop = 3;
                    cellSno14.PaddingBottom = 3;
                    tabled.AddCell(cellSno14);


                    PdfPCell celldesc14 = new PdfPCell(new Phrase("Statutory & Reimbursement", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    celldesc14.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    celldesc14.Colspan = 2;
                    // celldesc14.Border = 0;
                    celldesc14.PaddingTop = 3;
                    celldesc14.PaddingBottom = 3;
                    tabled.AddCell(celldesc14);


                    PdfPCell cellSno15 = new PdfPCell(new Phrase("1", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellSno15.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno15.Border = 0;
                    cellSno15.PaddingTop = 3;
                    cellSno15.PaddingBottom = 3;
                    tabled.AddCell(cellSno15);


                    PdfPCell celldesc15 = new PdfPCell(new Phrase("Esic @ 4.75%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldesc15.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    celldesc15.Colspan = 0;
                    // celldesc15.Border = 0;
                    celldesc15.PaddingTop = 3;
                    celldesc15.PaddingBottom = 3;
                    tabled.AddCell(celldesc15);

                    PdfPCell cellAmt12 = new PdfPCell(new Phrase(esiempr.ToString(), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellAmt12.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //cellAmt12.Border = 0;
                    cellAmt12.PaddingTop = 3;
                    cellAmt12.PaddingBottom = 3;
                    tabled.AddCell(cellAmt12);

                    PdfPCell cellSno16 = new PdfPCell(new Phrase("2", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellSno16.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno16.Border = 0;
                    cellSno16.PaddingTop = 3;
                    cellSno16.PaddingBottom = 3;
                    tabled.AddCell(cellSno16);


                    PdfPCell celldesc16 = new PdfPCell(new Phrase("EPF @ 13.36%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldesc16.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    celldesc16.Colspan = 0;
                    //celldesc16.Border = 0;
                    celldesc16.PaddingTop = 3;
                    celldesc16.PaddingBottom = 3;
                    tabled.AddCell(celldesc16);

                    PdfPCell cellAmt13 = new PdfPCell(new Phrase(pfempr.ToString(), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellAmt13.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //cellAmt13.Border = 0;
                    cellAmt13.PaddingTop = 3;
                    cellAmt13.PaddingBottom = 3;
                    tabled.AddCell(cellAmt13);

                    PdfPCell cellSno17 = new PdfPCell(new Phrase("3", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellSno17.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno17.Border = 0;
                    cellSno17.PaddingTop = 3;
                    cellSno17.PaddingBottom = 3;
                    tabled.AddCell(cellSno17);


                    PdfPCell celldesc17 = new PdfPCell(new Phrase("Management Fees", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldesc17.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //celldesc17.Border = 0;
                    celldesc17.PaddingTop = 3;
                    celldesc17.PaddingBottom = 3;
                    tabled.AddCell(celldesc17);

                    PdfPCell cellAmt14 = new PdfPCell(new Phrase(managementfee.ToString(), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellAmt14.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //cellAmt14.Border = 0;
                    cellAmt14.PaddingTop = 3;
                    cellAmt14.PaddingBottom = 3;
                    tabled.AddCell(cellAmt14);

                    PdfPCell cellSno18 = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellSno18.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno18.Border = 0;
                    cellSno18.PaddingTop = 3;
                    cellSno18.PaddingBottom = 3;
                    tabled.AddCell(cellSno18);

                    float TotalB = managementfee + pfempr + esiempr;

                    PdfPCell celldesc18 = new PdfPCell(new Phrase("TOTAL (B)", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    celldesc18.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //celldesc18.Border = 0;
                    celldesc18.PaddingTop = 3;
                    celldesc18.PaddingBottom = 3;
                    tabled.AddCell(celldesc18);

                    PdfPCell cellAmt15 = new PdfPCell(new Phrase(TotalB.ToString(), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cellAmt15.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //cellAmt15.Border = 0;
                    cellAmt15.PaddingTop = 3;
                    cellAmt15.PaddingBottom = 3;
                    tabled.AddCell(cellAmt15);

                    PdfPCell cellSno19 = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cellSno19.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //cellSno19.Border = 0;
                    cellSno19.PaddingTop = 3;
                    cellSno19.PaddingBottom = 3;
                    tabled.AddCell(cellSno19);

                    PdfPCell celldesc19 = new PdfPCell(new Phrase("TOTAL (A + B)", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    celldesc19.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //celldesc19.Border = 0;
                    celldesc19.PaddingTop = 3;
                    celldesc19.PaddingBottom = 3;
                    tabled.AddCell(celldesc19);

                    float TotalAB = BillTotal + TotalB;

                    PdfPCell cellAmt16 = new PdfPCell(new Phrase(TotalAB.ToString(), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cellAmt16.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //cellAmt16.Border = 0;
                    cellAmt16.PaddingTop = 3;
                    cellAmt16.PaddingBottom = 3;
                    tabled.AddCell(cellAmt16);

                    document.Add(tabled);




                    PdfPTable tablecon = new PdfPTable(2);
                    tablecon.TotalWidth = 500f;
                    tablecon.LockedWidth = true;
                    float[] widthcon = new float[] { 2f, 2f };
                    tablecon.SetWidths(widthcon);

                    PdfPCell cellBreak = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 15, Font.NORMAL, BaseColor.BLACK)));
                    cellBreak.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellBreak.Colspan = 2;
                    cellBreak.BorderWidthBottom = 0;
                    cellBreak.BorderWidthLeft = .5f;
                    cellBreak.BorderWidthTop = 0;
                    cellBreak.BorderWidthRight = .5f;
                    //cellBreak.Border = 0;
                    tablecon.AddCell(cellBreak);

                    string gtotal = NumberToEnglish.Instance.changeNumericToWords(TotalAB.ToString("#"));

                    PdfPCell cellcamt = new PdfPCell(new Phrase(" Grand Total is Rs. " + TotalAB.ToString("#") + " (Rupees " + gtotal.ToString() + "Only)", FontFactory.GetFont(FontStyle, 10, Font.BOLDITALIC, BaseColor.BLACK)));
                    cellcamt.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cellcamt.Colspan = 2;
                    cellcamt.BorderWidthBottom = 0;
                    cellcamt.BorderWidthLeft = .5f;
                    cellcamt.BorderWidthTop = 0;
                    cellcamt.BorderWidthRight = .5f;
                    //cellcamt.Border = 1;
                    tablecon.AddCell(cellcamt);
                    tablecon.AddCell(cellBreak);

                    string Servicetax = string.Empty;
                    string PANNO = string.Empty;
                    string PFNo = string.Empty;
                    string Esino = string.Empty;
                    string PTno = string.Empty;

                    if (compInfo.Rows.Count > 0)
                    {
                        Servicetax = compInfo.Rows[0]["BillNotes"].ToString();
                        PANNO = compInfo.Rows[0]["Labourrule"].ToString();
                        PFNo = compInfo.Rows[0]["PFNo"].ToString();
                        Esino = compInfo.Rows[0]["ESINo"].ToString();
                        PTno = compInfo.Rows[0]["bankname"].ToString();
                    }

                    if (Servicetax.Trim().Length > 0)
                    {
                        PdfPCell cellc6 = new PdfPCell(new Phrase("SERVICE TAX NO: " + Servicetax, FontFactory.GetFont(FontStyle, 9, Font.BOLD, BaseColor.BLACK)));
                        cellc6.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cellc6.Colspan = 7;
                        cellc6.BorderWidthBottom = 0;
                        cellc6.BorderWidthLeft = .5f;
                        cellc6.BorderWidthTop = .5f;
                        cellc6.BorderWidthRight = .5f;
                        //cellc6.Border = 0;
                        tablecon.AddCell(cellc6);
                    }



                    if (PANNO.Trim().Length > 0)
                    {
                        PdfPCell cellc7 = new PdfPCell(new Phrase("PAN NO: " + PANNO, FontFactory.GetFont(FontStyle, 9, Font.BOLD, BaseColor.BLACK)));
                        cellc7.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cellc7.Colspan = 7;
                        cellc7.BorderWidthBottom = .5f;
                        cellc7.BorderWidthLeft = .5f;
                        cellc7.BorderWidthTop = 0;
                        cellc7.BorderWidthRight = .5f;
                        //cellc7.Border = 0;
                        tablecon.AddCell(cellc7);
                    }
                    if (PFNo.Trim().Length > 0)
                    {
                        PdfPCell Pfno = new PdfPCell(new Phrase("EPF NO: " + PFNo, FontFactory.GetFont(FontStyle, 9, Font.BOLD, BaseColor.BLACK)));
                        Pfno.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        Pfno.Colspan = 7;
                        Pfno.BorderWidthBottom = .5f;
                        Pfno.BorderWidthLeft = .5f;
                        Pfno.BorderWidthTop = 0;
                        Pfno.BorderWidthRight = .5f;
                        //Pfno.Border = 0;
                        tablecon.AddCell(Pfno);
                    }

                    if (Esino.Trim().Length > 0)
                    {
                        PdfPCell ESino = new PdfPCell(new Phrase("ESIC NO: " + Esino, FontFactory.GetFont(FontStyle, 9, Font.BOLD, BaseColor.BLACK)));
                        ESino.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        ESino.Colspan = 7;
                        ESino.BorderWidthBottom = .5f;
                        ESino.BorderWidthLeft = .5f;
                        ESino.BorderWidthTop = 0;
                        ESino.BorderWidthRight = .5f;
                        //ESino.Border = 0;
                        tablecon.AddCell(ESino);
                    }

                    if (PTno.Trim().Length > 0)
                    {
                        PdfPCell Ptno = new PdfPCell(new Phrase("P Tax No: " + PTno, FontFactory.GetFont(FontStyle, 9, Font.BOLD, BaseColor.BLACK)));
                        Ptno.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        Ptno.Colspan = 7;
                        Ptno.BorderWidthBottom = .5f;
                        Ptno.BorderWidthLeft = .5f;
                        Ptno.BorderWidthTop = 0;
                        Ptno.BorderWidthRight = .5f;
                        //Ptno.Border = 0;
                        tablecon.AddCell(Ptno);
                    }

                    PdfPCell cellspace = new PdfPCell(new Paragraph(" ", FontFactory.GetFont(FontStyle, 12, Font.NORMAL, BaseColor.BLACK)));
                    cellspace.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cellspace.Border = 0;
                    cellspace.Colspan = 2;
                    tablecon.AddCell(cellspace);
                    //tablecon.AddCell(cellspace);

                    PdfPCell cellnote = new PdfPCell(new Paragraph("NOTE", FontFactory.GetFont(FontStyle, 10, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
                    cellnote.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cellnote.Border = 0;
                    cellnote.Colspan = 2;
                    tablecon.AddCell(cellnote);


                    PdfPCell cellnote1 = new PdfPCell(new Paragraph("Service Tax 14%(100%)  payable  by service Receiver ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cellnote1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cellnote1.Border = 0;
                    cellnote1.Colspan = 2;
                    cellnote1.PaddingTop = 5;
                    tablecon.AddCell(cellnote1);

                    PdfPCell cellc41 = new PdfPCell(new Phrase("For " + companyName, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cellc41.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cellc41.Colspan = 7;
                    cellc41.Border = 0;
                    cellc41.PaddingTop = 10;
                    tablecon.AddCell(cellc41);

                    PdfPCell cellc4 = new PdfPCell(new Phrase("Authorized Signatory", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cellc4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cellc4.Colspan = 7;
                    cellc4.Border = 0;
                    cellc4.PaddingTop = 30;
                    tablecon.AddCell(cellc4);





                    document.Add(tablecon);
                    document.NewPage();
                    document.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Invoice.pdf");
                    Response.Buffer = true;
                    Response.Clear();
                    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    Response.OutputStream.Flush();
                    Response.End();
                }
                catch (Exception ex)
                {
                    //LblResult.Text = ex.Message;
                }
            }
            else
            {
                // LblResult.Text = "There is no bill generated for selected client";
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' There is no bill generated for selected client ');", true);

            }
        }


        #region New code for old bill Generation on 10/03/2014 by venkat



        public int GetMonthBasedOnSelectionDateorMonth()
        {

            var testDate = 0;
            string EnteredDate = "";

            #region Validation

            if (txtmonth.Text.Trim().Length > 0)
            {

                try
                {

                    testDate = GlobalData.Instance.CheckEnteredDate(txtmonth.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return 0;
                    }
                    EnteredDate = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return 0;
                }
            }
            #endregion


            #region  Month Get Based on the Control Selection
            int month = 0;
            if (Chk_Month.Checked == false)
            {
                month = Timings.Instance.GetIdForSelectedMonth(ddlmonth.SelectedIndex);
                //return month;
            }
            if (Chk_Month.Checked == true)
            {
                DateTime date = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                month = Timings.Instance.GetIdForEnteredMOnth(date);
                //return month;
            }
            return month;

            #endregion
        }

        public string GetMonthOfYear()
        {
            string MonthYear = "";

            int month = GetMonthBasedOnSelectionDateorMonth();
            if (month.ToString().Length == 4)
            {

                MonthYear = "20" + month.ToString().Substring(2, 2);

            }
            if (month.ToString().Length == 3)
            {

                MonthYear = "20" + month.ToString().Substring(1, 2);

            }
            return MonthYear;
        }

        protected int GetMonth(string NameOfmonth)
        {
            int month = -1;
            var formatInfoinfo = new DateTimeFormatInfo();
            string[] monthName = formatInfoinfo.MonthNames;
            for (int i = 0; i < monthName.Length; i++)
            {
                if (monthName[i].CompareTo(NameOfmonth) == 0)
                {
                    month = i + 1;
                    break;
                }
            }
            return month;
        }

        public string GetMonthName()
        {
            string monthname = string.Empty;
            int payMonth = 0;
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();

            if (Chk_Month.Checked == false)
            {
                payMonth = GetMonth(ddlmonth.SelectedValue);
                monthname = mfi.GetMonthName(payMonth).ToString();
            }
            if (Chk_Month.Checked == true)
            {

                DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                monthname = mfi.GetMonthName(date.Month).ToString();
                //payMonth = GetMonth(monthname);
            }
            return monthname;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {


            var password = string.Empty;
            var SPName = string.Empty;
            password = txtPassword.Text.Trim();
            string sqlPassword = "select password from IouserDetails where password='" + txtPassword.Text + "'";
            DataTable dtpassword = config.ExecuteAdaptorAsyncWithQueryParams(sqlPassword).Result;
            if (dtpassword.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Invalid Password');", true);
                return;
            }

            #region Validation

            gvClientBilling.DataSource = null;
            gvClientBilling.DataBind();
            txtbilldate.Text = string.Empty;
            txtfromdate.Text = string.Empty;
            txttodate.Text = string.Empty;
            lblbillnolatest.Text = string.Empty;
            txtmonth.Text = string.Empty;
            ddlmonth.SelectedIndex = 0;
            txtduedate.Text = string.Empty;
            ClearExtraDataForBilling();
            ClearData();

            if (ddlclientid.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select The client Id');", true);
                Chk_Month.Checked = false;
                return;
            }

            #endregion

            Chk_Month.Checked = true;

            if (Chk_Month.Checked)
            {
                txtmonth.Visible = true;
                ddlmonth.SelectedIndex = 0;
                ddlmonth.Visible = false;

            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            modelLogindetails.Hide();
            Chk_Month.Checked = false;
            gvClientBilling.DataSource = null;
            gvClientBilling.DataBind();
            txtbilldate.Text = string.Empty;
            txtfromdate.Text = string.Empty;
            txttodate.Text = string.Empty;
            lblbillnolatest.Text = string.Empty;
            txtmonth.Text = string.Empty;
            ddlmonth.SelectedIndex = 0;
            txtduedate.Text = string.Empty;
            ClearExtraDataForBilling();
            ClearData();
            if (Chk_Month.Checked == false)
            {
                txtmonth.Visible = false;
                txtmonth.Text = "";
                ddlmonth.SelectedIndex = 0;
                ddlmonth.Visible = true;
            }
        }

        #endregion

        protected void ddlType_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            gvClientBilling.DataSource = null;
            gvClientBilling.DataBind();
            EnabledFields();
            ClearData();

            ClearExtraDataForBilling();

            checkExtraData.Checked = false;
            txtbillno.Text = "";
            txtMachinery.Text = "";
            txtMaterialcost.Text = "";
            txtMaterial.Text = "";
            txtMaterial.Text = "";
            txtElectical.Text = "";
            txtextraonevalue.Text = "";
            txtextratwotitle.Text = "";
            txtextratwovalue.Text = "";
            txtdiscount.Text = "";
            txtDiscounts.Text = "";
            txtdiscounttwotitle.Text = "";
            txtdiscounttwovalue.Text = "";
            ddlmonth.SelectedIndex = 0;

            rdbcreatebill.Checked = true;
            rdbmodifybill.Checked = false;

            if (ddlType.SelectedIndex == 0)
            {
                TxtservicechrgPrc.Visible = false;
                TxtServiceTaxPrc.Visible = false;
                TxtSBCESSPrc.Visible = false;
                TxtKKCESSPrc.Visible = false;
                TxtCESSPrc.Visible = false;
                TxtSheCESSPrc.Visible = false;
                lblServiceCharges.Enabled = false;
            }
            loadDesignations();

            visiblebutton();
            displayExtraData();


        }

        public void EnabledFields()
        {
            try
            {
                for (int i = 0; i < gvClientBilling.Rows.Count; i++)
                {
                    TextBox lbldesgn = gvClientBilling.Rows[i].FindControl("lbldesgn") as TextBox;
                    DropDownList ddlHSNNumber = gvClientBilling.Rows[i].FindControl("ddlHSNNumber") as DropDownList;
                    TextBox lblnoofemployees = gvClientBilling.Rows[i].FindControl("lblnoofemployees") as TextBox;
                    TextBox lblNoOfDuties = gvClientBilling.Rows[i].FindControl("lblNoOfDuties") as TextBox;
                    TextBox lblpayrate = gvClientBilling.Rows[i].FindControl("lblpayrate") as TextBox;
                    TextBox lblSchrgPrc = gvClientBilling.Rows[i].FindControl("lblSchrgPrc") as TextBox;
                    TextBox lblSchrgAmt = gvClientBilling.Rows[i].FindControl("lblSchrgAmt") as TextBox;
                    TextBox txtNewPayRate = gvClientBilling.Rows[i].FindControl("txtNewPayRate") as TextBox;
                    TextBox lblda = gvClientBilling.Rows[i].FindControl("lblda") as TextBox;
                    TextBox lblAmount = gvClientBilling.Rows[i].FindControl("lblAmount") as TextBox;
                    TextBox lblCGSTAmount = gvClientBilling.Rows[i].FindControl("lblCGSTAmount") as TextBox;
                    TextBox lblSGSTAmount = gvClientBilling.Rows[i].FindControl("lblSGSTAmount") as TextBox;
                    TextBox lblIGSTAmount = gvClientBilling.Rows[i].FindControl("lblIGSTAmount") as TextBox;
                    TextBox lblCGSTPrc = gvClientBilling.Rows[i].FindControl("lblCGSTPrc") as TextBox;
                    TextBox lblSGSTPrc = gvClientBilling.Rows[i].FindControl("lblSGSTPrc") as TextBox;
                    TextBox lblIGSTPrc = gvClientBilling.Rows[i].FindControl("lblIGSTPrc") as TextBox;
                    TextBox lblTotalTaxmount = gvClientBilling.Rows[i].FindControl("lblTotalTaxmount") as TextBox;
                    TextBox txtUOM = gvClientBilling.Rows[i].FindControl("txtUOM") as TextBox;
                    DropDownList ddlCalnType = gvClientBilling.Rows[i].FindControl("ddlCalnType") as DropDownList;
                    Label lblextra = gvClientBilling.Rows[i].FindControl("lblextra") as Label;

                    switch (ddlType.SelectedIndex)
                    {
                        case 1:
                            lbldesgn.Enabled = true;
                            lblnoofemployees.Enabled = true;
                            ddlHSNNumber.Enabled = true;
                            lblNoOfDuties.Enabled = true;
                            lblpayrate.Enabled = true;
                            txtNewPayRate.Enabled = false;
                            lblda.Enabled = true;
                            lblAmount.Enabled = true;
                            lblSchrgAmt.Enabled = true;
                            lblSchrgPrc.Enabled = true;
                            lblGrandTotal.Enabled = true;
                            lblSheCESS.Enabled = true;
                            lblCESS.Enabled = true;
                            lblKKCESS.Enabled = true;
                            lblSBCESS.Enabled = true;
                            lblServiceTax.Enabled = true;
                            lblTotalResources.Enabled = true;
                            TxtservicechrgPrc.Enabled = true;
                            TxtservicechrgPrc.Visible = true;
                            lblServiceCharges.Visible = true;
                            lblServiceCharges.Enabled = true;
                            TxtServiceTaxPrc.Enabled = true;
                            TxtSBCESSPrc.Enabled = true;
                            TxtKKCESSPrc.Enabled = true;
                            TxtCESSPrc.Enabled = true;
                            TxtSheCESSPrc.Enabled = true;
                            lblServiceChargeTitle.Visible = true;
                            btnFreeze.Visible = false;
                            lblCGSTAmount.Enabled = false;
                            lblSGSTAmount.Enabled = false;
                            lblIGSTAmount.Enabled = false;
                            lblCGSTPrc.Enabled = false;
                            lblSGSTPrc.Enabled = false;
                            lblIGSTPrc.Enabled = false;
                            lblTotalTaxmount.Enabled = false;
                            txtUOM.Enabled = false;
                            ddlCalnType.Enabled = true;

                            #region for GST as on 16-6-2017 by swathi

                            lblCGST.Enabled = true;
                            lblSGST.Enabled = true;
                            lblIGST.Enabled = true;
                            TxtIGSTPrc.Enabled = true;
                            TxtSGSTPrc.Enabled = true;
                            TxtCGSTPrc.Enabled = true;

                            #endregion for GST as on 16-6-2017

                            break;
                        case 2:
                            lbldesgn.Enabled = true;
                            lblnoofemployees.Enabled = true;
                            lblNoOfDuties.Enabled = true;
                            ddlHSNNumber.Enabled = true;
                            lblpayrate.Enabled = true;
                            lblda.Enabled = true;
                            lblSchrgAmt.Enabled = true;
                            lblSchrgPrc.Enabled = true;
                            lblAmount.Enabled = true;
                            txtNewPayRate.Enabled = true;
                            lblGrandTotal.Enabled = true;
                            lblSheCESS.Enabled = true;
                            lblCESS.Enabled = true;
                            lblKKCESS.Enabled = true;
                            lblSBCESS.Enabled = true;
                            lblServiceTax.Enabled = true;
                            lblTotalResources.Enabled = true;
                            TxtservicechrgPrc.Enabled = true;
                            TxtServiceTaxPrc.Enabled = true;
                            lblServiceCharges.Visible = true;
                            lblServiceCharges.Enabled = true;
                            TxtSBCESSPrc.Enabled = true;
                            TxtKKCESSPrc.Enabled = true;
                            TxtCESSPrc.Enabled = true;
                            TxtSheCESSPrc.Enabled = true;
                            lblServiceChargeTitle.Visible = true;

                            btnFreeze.Visible = false;
                            lblCGSTAmount.Enabled = false;
                            lblSGSTAmount.Enabled = false;
                            lblIGSTAmount.Enabled = false;
                            lblCGSTPrc.Enabled = false;
                            lblSGSTPrc.Enabled = false;
                            lblIGSTPrc.Enabled = false;
                            lblTotalTaxmount.Enabled = false;
                            ddlHSNNumber.Enabled = false;
                            txtUOM.Enabled = false;
                            ddlCalnType.Enabled = true;

                            #region for GST as on 16-6-2017 by swathi

                            lblCGST.Enabled = true;
                            lblSGST.Enabled = true;
                            lblIGST.Enabled = true;
                            TxtIGSTPrc.Enabled = true;
                            TxtSGSTPrc.Enabled = true;
                            TxtCGSTPrc.Enabled = true;
                            lblCess1.Enabled = true;
                            TxtCess1Prc.Enabled = true;
                            lblCess2.Enabled = true;
                            TxtCess2Prc.Enabled = true;

                            #endregion for GST as on 16-6-2017

                            break;
                        case 3:
                            lbldesgn.Enabled = true;
                            lblnoofemployees.Enabled = true;
                            lblNoOfDuties.Enabled = true;
                            lblpayrate.Enabled = true;
                            txtNewPayRate.Enabled = false;
                            lblSchrgAmt.Enabled = true;
                            lblSchrgPrc.Enabled = true;
                            lblda.Enabled = true;
                            lblAmount.Enabled = true;
                            lblGrandTotal.Enabled = true;
                            lblSheCESS.Enabled = true;
                            lblCESS.Enabled = true;
                            lblKKCESS.Enabled = true;
                            lblSBCESS.Enabled = true;
                            lblServiceTax.Enabled = true;
                            lblTotalResources.Enabled = true;
                            TxtservicechrgPrc.Enabled = true;
                            TxtservicechrgPrc.Visible = true;
                            lblServiceCharges.Enabled = true;
                            lblServiceCharges.Visible = true;
                            TxtServiceTaxPrc.Enabled = true;
                            TxtSBCESSPrc.Enabled = true;
                            TxtKKCESSPrc.Enabled = true;
                            TxtCESSPrc.Enabled = true;
                            TxtSheCESSPrc.Enabled = true;
                            lblServiceChargeTitle.Visible = true;
                            btnFreeze.Visible = false;
                            lblCGSTAmount.Enabled = true;
                            lblSGSTAmount.Enabled = true;
                            lblIGSTAmount.Enabled = true;
                            lblCGSTPrc.Enabled = true;
                            lblSGSTPrc.Enabled = true;
                            lblIGSTPrc.Enabled = true;
                            lblTotalTaxmount.Enabled = true;
                            ddlHSNNumber.Enabled = true;
                            txtUOM.Enabled = true;
                            ddlCalnType.Enabled = true;

                            #region for GST as on 16-6-2017 by swathi

                            lblCGST.Enabled = true;
                            lblSGST.Enabled = true;
                            lblIGST.Enabled = true;
                            TxtIGSTPrc.Enabled = true;
                            TxtSGSTPrc.Enabled = true;
                            TxtCGSTPrc.Enabled = true;

                            #endregion for GST as on 16-6-2017
                            break;
                        case 4:
                            lbldesgn.Enabled = true;
                            lblnoofemployees.Enabled = true;
                            ddlHSNNumber.Enabled = true;
                            lblNoOfDuties.Enabled = true;
                            lblpayrate.Enabled = true;
                            txtNewPayRate.Enabled = false;
                            lblda.Enabled = true;
                            lblAmount.Enabled = true;
                            lblSchrgAmt.Enabled = true;
                            lblSchrgPrc.Enabled = true;
                            lblGrandTotal.Enabled = true;
                            lblSheCESS.Enabled = true;
                            lblCESS.Enabled = true;
                            lblKKCESS.Enabled = true;
                            lblSBCESS.Enabled = true;
                            lblServiceTax.Enabled = true;
                            lblTotalResources.Enabled = true;
                            TxtservicechrgPrc.Enabled = true;
                            TxtservicechrgPrc.Visible = true;
                            lblServiceCharges.Visible = true;
                            lblServiceCharges.Enabled = true;
                            TxtServiceTaxPrc.Enabled = true;
                            TxtSBCESSPrc.Enabled = true;
                            TxtKKCESSPrc.Enabled = true;
                            TxtCESSPrc.Enabled = true;
                            TxtSheCESSPrc.Enabled = true;
                            lblServiceChargeTitle.Visible = true;
                            btnFreeze.Visible = false;
                            lblCGSTAmount.Enabled = false;
                            lblSGSTAmount.Enabled = false;
                            lblIGSTAmount.Enabled = false;
                            lblCGSTPrc.Enabled = false;
                            lblSGSTPrc.Enabled = false;
                            lblIGSTPrc.Enabled = false;
                            lblTotalTaxmount.Enabled = false;
                            txtUOM.Enabled = false;
                            ddlCalnType.Enabled = true;

                            #region for GST as on 16-6-2017 by swathi

                            lblCGST.Enabled = true;
                            lblSGST.Enabled = true;
                            lblIGST.Enabled = true;
                            TxtIGSTPrc.Enabled = true;
                            TxtSGSTPrc.Enabled = true;
                            TxtCGSTPrc.Enabled = true;

                            #endregion for GST as on 16-6-2017

                            break;
                        default:

                            if (lblextra.Text.Contains("E"))
                            {

                                lbldesgn.Enabled = true;
                                lblnoofemployees.Enabled = true;
                                lblNoOfDuties.Enabled = true;
                                lblpayrate.Enabled = true;
                                lblda.Enabled = true;
                                lblAmount.Enabled = true;
                                ddlHSNNumber.Enabled = true;
                                lblSchrgAmt.Enabled = true;
                                lblSchrgPrc.Enabled = true;
                            }
                            else
                            {

                                lbldesgn.Enabled = false;
                                lblnoofemployees.Enabled = false;
                                lblNoOfDuties.Enabled = false;
                                lblpayrate.Enabled = false;
                                lblda.Enabled = false;
                                lblAmount.Enabled = false;
                                ddlHSNNumber.Enabled = false;
                                lblSchrgAmt.Enabled = false;
                                lblSchrgPrc.Enabled = false;
                            }



                            txtNewPayRate.Enabled = false;
                            lblGrandTotal.Enabled = false;
                            lblSheCESS.Enabled = false;
                            lblCESS.Enabled = false;
                            lblKKCESS.Enabled = false;
                            lblSBCESS.Enabled = false;
                            lblServiceTax.Enabled = false;
                            lblTotalResources.Enabled = false;
                            TxtservicechrgPrc.Enabled = false;
                            TxtServiceTaxPrc.Enabled = false;
                            TxtSBCESSPrc.Enabled = false;
                            TxtKKCESSPrc.Enabled = false;
                            TxtCESSPrc.Enabled = false;
                            TxtSheCESSPrc.Enabled = false;
                            lblServiceChargeTitle.Visible = true;
                            lblCGSTAmount.Enabled = false;
                            lblSGSTAmount.Enabled = false;
                            lblIGSTAmount.Enabled = false;
                            lblCGSTPrc.Enabled = false;
                            lblSGSTPrc.Enabled = false;
                            lblIGSTPrc.Enabled = false;
                            lblTotalTaxmount.Enabled = false;
                            ddlHSNNumber.Enabled = false;
                            txtUOM.Enabled = false;
                            ddlCalnType.Enabled = true;

                            #region for GST as on 16-6-2017 by swathi

                            lblCGST.Enabled = false;
                            lblSGST.Enabled = false;
                            lblIGST.Enabled = false;
                            TxtIGSTPrc.Enabled = false;
                            TxtSGSTPrc.Enabled = false;
                            TxtCGSTPrc.Enabled = false;

                            #endregion for GST as on 16-6-2017

                            break;
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        protected void ddlMBBillnos_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbmodifybill.Checked == false)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Bill Type Modify');", true);
                ddlMBBillnos.SelectedIndex = 0;
            }
            else
            {
                VisibleFreezeCredit();
                DisplayDataInGridManual();
                EnabledFields();
                visiblebutton();
                displayExtraData();
            }
        }

        protected void DisplayDataInGridManual()
        {

            lblamtinwords.Text = "";
            try
            {
                #region Variable Declaration
                ClearData();
                int month = 0;

                #endregion

                #region  Select Month

                month = GetMonthBasedOnSelectionDateorMonth();

                string monthval = "";
                string yearval = "";
                string monthreports = "";

                DateTime firstday = DateTime.Now;


                if (month.ToString().Length == 3)
                {
                    monthval = month.ToString().Substring(0, 1);
                    yearval = "20" + month.ToString().Substring(1, 2);

                }
                else //if (monthreports.Length == 4)
                {
                    monthval = month.ToString().Substring(0, 2);
                    yearval = "20" + month.ToString().Substring(2, 2);
                }

                firstday = GlobalData.Instance.GetFirstDayMonth(int.Parse(yearval), int.Parse(monthval));




                #endregion

                #region Empty And Assign Data To Gridview
                lbltotalamount.Text = "";
                DataTable Dtunit = null;
                gvClientBilling.DataSource = Dtunit;
                gvClientBilling.DataBind();

                #endregion


                #region  Begin Get Contract Id Based on The Last Day

                DateTime DtLastDay = DateTime.Now;
                if (Chk_Month.Checked == false)
                {
                    DtLastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlmonth.SelectedIndex);
                }
                if (Chk_Month.Checked == true)
                {
                    DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                }
                var ContractID = "";
                Hashtable HtGetContractID = new Hashtable();
                var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
                HtGetContractID.Add("@LastDay", DtLastDay);
                DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                if (DTContractID.Rows.Count > 0)
                {
                    ContractID = DTContractID.Rows[0]["contractid"].ToString();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not  Avaialable For This Client.');", true);
                    return;
                }

                #endregion  End Get Contract Id Based on The Last Day


                #region New Coding For Manual Billing

                var query = @"select 
                             Ubb.Designation,
                             Ubb.BasicDA as BasicDa,
                             ISNULL(Ubb.NoofEmps,0) as NoofEmps,
                             ISNULL(Ubb.DutyHours,0) as DutyHrs,
                             Round(Ubb.PayRate,2) as payrate,  
                             Ubb.PayRateType as paytype,
                             Ubb.monthlydays,
                             Ubb.DutyHours,
                             Ubb.OTAmount,
                             Ubb.Totalamount,
                             Ubb.Remarks, 
                             Ubb.Description,
                             0 as NewPayRate,
                             0 as ServiceCharge,
                             0 as NoOfDayscd ,
                             mub.ServiceChrg as ServiceChrg,
                             isnull(ubb.otamount,0) as otamount,
                             mub.BillDt as BillDate,'' as designid,mub.remarks as BillRemarks,'' as type,ubb.monthlydays as noofdays, isnull(ubb.HSNNumber,'0') HSNNumber ,'' as Extra,
                             (isnull(ubb.CGSTprc,0)+isnull(ubb.SGSTprc,0)+isnull(ubb.IGSTprc,0)) as GSTper,
                             UOM,isnull(ubb.ServiceChargesPrc,0) as ServiceChargesPrc,isnull(ubb.ServiceCharges,0) as ServiceCharges,
                            ubb.CGSTPrc,ubb.CGSTAmt,ubb.SGSTPrc,ubb.SGSTAmt,ubb.IGSTPrc,ubb.IGSTAmt,TotalTaxAmount ,isnull(CalnType,'Add') as CalnType
                              
                    from MUnitBillBreakup as Ubb 
                    inner join MUnitBill mub on Ubb.UnitId=mub.UnitId and Ubb.MunitidBillno=mub.Billno
                    where Ubb.unitid ='" + ddlclientid.SelectedValue + "' and Ubb.month=" + month + " and Ubb.MunitidBillno='" + ddlMBBillnos.SelectedValue
                            + "' order by ubb.sino";

                //Group by  Ubb.UnitId, Ubb.Designation,Ubb.BasicDA, Ubb.NoofEmps,Ubb.DutyHours,Ubb.monthlydays,Ubb.PayRate,Ubb.PayRateType,Ubb.DutyHours,Ubb.otamount,Ubb.Remarks,Ubb.Description,cd.NoOfDays, mub.ServiceChrg,mub.BillDt,cd.ServiceCharge,Ubb.Totalamount";

                DataTable DtForUBB = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;


                var SumTotal = @"select sum(isnull(Totalamount,0)) as Total from  MUnitBillBreakup where unitid ='" + ddlclientid.SelectedValue + "' and month=" + month + " and MunitidBillno='" + ddlMBBillnos.SelectedValue
                            + "'";

                DataTable DtForSumUBB = config.ExecuteAdaptorAsyncWithQueryParams(SumTotal).Result;


                if (DtForUBB.Rows.Count > 0)
                {

                    gvClientBilling.DataSource = DtForUBB;
                    gvClientBilling.DataBind();

                    displaydata();

                    btnAddNewRow.Visible = true;
                    btnCalculateTotals.Visible = true;


                    for (int i = 0; i < DtForUBB.Rows.Count; i++)
                    {






                        DropDownList Nods = gvClientBilling.Rows[i].FindControl("ddlnod") as DropDownList;

                        if (Nods != null)
                        {

                            float noofdays = float.Parse(DtForUBB.Rows[i]["noofdays"].ToString());
                            Nods.SelectedValue = DtForUBB.Rows[i]["noofdays"].ToString();

                        }

                        DropDownList ddlHSNNumber = gvClientBilling.Rows[i].FindControl("ddlHSNNumber") as DropDownList;

                        if (ddlHSNNumber != null)
                        {

                            ddlHSNNumber.SelectedValue = DtForUBB.Rows[i]["HSNNumber"].ToString();

                        }


                        DropDownList Dtype = gvClientBilling.Rows[i].FindControl("ddldutytype") as DropDownList;

                        if (Dtype != null)
                        {

                            int amt = int.Parse(DtForUBB.Rows[i]["PayType"].ToString());
                            Dtype.SelectedValue = DtForUBB.Rows[i]["PayType"].ToString();

                        }

                        DropDownList ddlCalnType = gvClientBilling.Rows[i].FindControl("ddlCalnType") as DropDownList;

                        if (ddlCalnType != null)
                        {

                            ddlCalnType.SelectedValue = DtForUBB.Rows[i]["CalnType"].ToString();

                        }

                        TextBox totalAmt = gvClientBilling.Rows[i].FindControl("lblAmount") as TextBox;
                        totalAmt.Text = DtForUBB.Rows[i]["Totalamount"].ToString();


                    }
                    ViewState["DTDefaultManual"] = DtForUBB;

                    //  lblServiceCharges.Text=DtForUBB.Rows[0]["ServiceChrg"].ToString();
                    if (DtForSumUBB.Rows.Count > 0)
                    {
                        lblTotalResources.Text = DtForSumUBB.Rows[0]["Total"].ToString();
                    }

                    bool Extradatacheck = false;


                    #region    Retrive Data From munitbill  table data based on the bill no

                    string SqlQryForunitbill = "Select *,convert(varchar(10),BillDt,103) as Billdate,convert(varchar(10),DueDt,103) as dtDue,convert(varchar(10),FromDt,103) as FromDate,convert(varchar(10),ToDt,103) as ToDate,CGSTAmt,SGSTAmt,BillType from munitbill   Where  unitid='" + ddlclientid.SelectedValue +
                                               "'  and  Month='" + month + "'  and billno='" + ddlMBBillnos.SelectedValue + "'";

                    DataTable DtForUnitBill = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForunitbill).Result;
                    if (DtForUnitBill.Rows.Count > 0)
                    {



                        System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-GB");

                        string billdate = DtForUnitBill.Rows[0]["Billdate"].ToString();
                        txtbilldate.Text = billdate;

                        string duedate = DtForUnitBill.Rows[0]["dtDue"].ToString();
                        txtduedate.Text = duedate;
                        lblCGST.Text = DtForUnitBill.Rows[0]["CGSTAmt"].ToString();
                        lblSGST.Text = DtForUnitBill.Rows[0]["SGSTAmt"].ToString();
                        lblIGST.Text = DtForUnitBill.Rows[0]["IGSTAmt"].ToString();
                        TxtCGSTPrc.Text = DtForUnitBill.Rows[0]["CGSTPrc"].ToString();
                        TxtSGSTPrc.Text = DtForUnitBill.Rows[0]["SGSTPrc"].ToString();
                        TxtIGSTPrc.Text = DtForUnitBill.Rows[0]["IGSTPrc"].ToString();

                        // txtMBillNo.Text = DtForUnitBill.Rows[0]["MBillno"].ToString();

                        txtfromdate.Text = DtForUnitBill.Rows[0]["FromDate"].ToString();
                        txttodate.Text = DtForUnitBill.Rows[0]["ToDate"].ToString();

                        txtBankname.Text = DtForUnitBill.Rows[0]["BankName"].ToString();
                        txtBankAccNo.Text = DtForUnitBill.Rows[0]["BankAccountNo"].ToString();
                        txtifsccode.Text = DtForUnitBill.Rows[0]["IFSCCode"].ToString();

                        string BillType = DtForUnitBill.Rows[0]["BillType"].ToString();
                        if (BillType == "M")
                        {
                            ddlType.SelectedIndex = 1;
                        }
                        else if (BillType == "A")
                        {
                            ddlType.SelectedIndex = 2;
                        }
                        else if (BillType == "B")
                        {
                            ddlType.SelectedIndex = 3;
                        }
                        else if (BillType == "E")
                        {
                            ddlType.SelectedIndex = 4;
                        }


                        lblbillnolatest.Text = DtForUnitBill.Rows[0]["BillNo"].ToString();
                        TxtservicechrgPrc.Text = DtForUnitBill.Rows[0]["ServiceChrgPer"].ToString();
                        //lblSubTotal.Text = DtForUnitBill.Rows[0]["Subtotal"].ToString();
                        lblServiceCharges.Text = DtForUnitBill.Rows[0]["ServiceChrg"].ToString();
                        lblTotalResources.Text = DtForUnitBill.Rows[0]["dutiestotalamount"].ToString();
                        txtdescription.Text = DtForUBB.Rows[0]["Description"].ToString();


                        lblServiceTax.Text = DtForUnitBill.Rows[0]["ServiceTax"].ToString();
                        lblSBCESS.Text = DtForUnitBill.Rows[0]["SBCessAmt"].ToString();
                        lblKKCESS.Text = DtForUnitBill.Rows[0]["KKCessAmt"].ToString();
                        lblCESS.Text = DtForUnitBill.Rows[0]["CESS"].ToString();
                        lblSheCESS.Text = DtForUnitBill.Rows[0]["SHECESS"].ToString();
                        lblGrandTotal.Text = DtForUnitBill.Rows[0]["GrandTotal"].ToString();
                        TxtServiceTaxPrc.Text = DtForUnitBill.Rows[0]["ServiceTaxPrc"].ToString();
                        TxtSBCESSPrc.Text = DtForUnitBill.Rows[0]["SBCessTaxPrc"].ToString();
                        TxtKKCESSPrc.Text = DtForUnitBill.Rows[0]["KKCessTaxPrc"].ToString();
                        TxtCESSPrc.Text = DtForUnitBill.Rows[0]["Cessper"].ToString();
                        TxtSheCESSPrc.Text = DtForUnitBill.Rows[0]["SheCessper"].ToString();
                        txtRemarks.Text = DtForUnitBill.Rows[0]["Remarks"].ToString();

                        if (lblServiceCharges.Text != "0")
                        {
                            lblServiceCharges.Visible = true;
                            lblServiceChargeTitle.Visible = true;
                            TxtservicechrgPrc.Visible = true;
                        }
                        else
                        {
                            lblServiceCharges.Visible = false;
                            lblServiceChargeTitle.Visible = false;
                            TxtservicechrgPrc.Visible = false;
                        }

                        if (lblServiceTax.Text != "0")
                        {
                            lblServiceTaxTitle.Visible = true;
                            lblServiceTax.Visible = true;
                            TxtServiceTaxPrc.Visible = true;
                        }
                        else
                        {
                            lblServiceTaxTitle.Visible = false;
                            lblServiceTax.Visible = false;
                            TxtServiceTaxPrc.Visible = false;
                        }


                        if (lblSBCESS.Text != "0")
                        {
                            lblSBCESS.Visible = true;
                            lblSBCESSTitle.Visible = true;
                            TxtSBCESSPrc.Visible = true;
                        }
                        else
                        {
                            lblSBCESS.Visible = false;
                            lblSBCESSTitle.Visible = false;
                            TxtSBCESSPrc.Visible = false;
                        }

                        if (lblKKCESS.Text != "0")
                        {
                            lblKKCESS.Visible = true;
                            lblKKCESSTitle.Visible = true;
                            TxtKKCESSPrc.Visible = true;
                        }
                        else
                        {
                            lblKKCESS.Visible = false;
                            lblKKCESSTitle.Visible = false;
                            TxtKKCESSPrc.Visible = false;
                        }

                        if (lblCESS.Text != "0")
                        {
                            lblCESSTitle.Visible = true;
                            lblCESS.Visible = true;
                            TxtCESSPrc.Visible = true;
                        }
                        else
                        {
                            lblCESSTitle.Visible = false;
                            lblCESS.Visible = false;
                            TxtCESSPrc.Visible = false;
                        }

                        if (lblSheCESS.Text != "0")
                        {
                            lblSheCESSTitle.Visible = true;
                            lblSheCESS.Visible = true;
                            TxtSheCESSPrc.Visible = true;
                        }
                        else
                        {
                            lblSheCESSTitle.Visible = false; ;
                            lblSheCESS.Visible = false;
                            TxtSheCESSPrc.Visible = false;
                        }


                        if (lblCGST.Text != "0")
                        {
                            TxtCGSTPrc.Visible = true;
                            lblCGST.Visible = true;
                            lblCGSTTitle.Visible = true;
                        }
                        else
                        {
                            TxtCGSTPrc.Visible = false;
                            lblCGST.Visible = false;
                            lblCGSTTitle.Visible = false;
                        }



                        if (lblSGST.Text != "0")
                        {
                            TxtSGSTPrc.Visible = true;
                            lblSGST.Visible = true;
                            lblSGSTTitle.Visible = true;
                        }
                        else
                        {
                            TxtSGSTPrc.Visible = false;
                            lblSGST.Visible = false;
                            lblSGSTTitle.Visible = false;
                        }


                        if (lblIGST.Text != "0")
                        {
                            TxtIGSTPrc.Visible = true;
                            lblIGST.Visible = true;
                            lblIGSTTitle.Visible = true;
                        }
                        else
                        {
                            TxtIGSTPrc.Visible = false;
                            lblIGST.Visible = false;
                            lblIGSTTitle.Visible = false;
                        }

                        txtmachinarycost.Text = DtForUnitBill.Rows[0]["Machinarycosttitle"].ToString();
                        txtMaterialcost.Text = DtForUnitBill.Rows[0]["Materialcosttitle"].ToString();
                        txtextratwotitle.Text = DtForUnitBill.Rows[0]["Extratwotitle"].ToString();
                        txtextraonetitle.Text = DtForUnitBill.Rows[0]["Extraonetitle"].ToString();
                        txtdiscounttwotitle.Text = DtForUnitBill.Rows[0]["Discounttwotitle"].ToString();
                        txtdiscount.Text = DtForUnitBill.Rows[0]["Discountonetitle"].ToString();
                        txtMaintanancecost.Text = DtForUnitBill.Rows[0]["Maintanancecosttitle"].ToString();
                        //values
                        txtMachinery.Text = DtForUnitBill.Rows[0]["MachinaryCost"].ToString();
                        txtMaterial.Text = DtForUnitBill.Rows[0]["MaterialCost"].ToString();
                        txtextratwovalue.Text = DtForUnitBill.Rows[0]["ExtraAmtTwo"].ToString();
                        txtextraonevalue.Text = DtForUnitBill.Rows[0]["ExtraAmtone"].ToString();
                        txtdiscounttwovalue.Text = DtForUnitBill.Rows[0]["DiscountTwo"].ToString();
                        txtDiscounts.Text = DtForUnitBill.Rows[0]["Discount"].ToString();
                        txtElectical.Text = DtForUnitBill.Rows[0]["ElectricalChrg"].ToString();



                        chkSTYesMachinary.Checked = bool.Parse(DtForUnitBill.Rows[0]["stmachinary"].ToString());
                        chkSTYesMaterial.Checked = bool.Parse(DtForUnitBill.Rows[0]["STMaterial"].ToString());
                        chkSTYesElectrical.Checked = bool.Parse(DtForUnitBill.Rows[0]["STMaintenance"].ToString());
                        chkSTYesExtraone.Checked = bool.Parse(DtForUnitBill.Rows[0]["STExtraone"].ToString());
                        chkSTYesExtratwo.Checked = bool.Parse(DtForUnitBill.Rows[0]["STExtratwo"].ToString());
                        chkSTDiscountone.Checked = bool.Parse(DtForUnitBill.Rows[0]["STDiscountone"].ToString());
                        chkSTDiscounttwo.Checked = bool.Parse(DtForUnitBill.Rows[0]["STDiscounttwo"].ToString());


                        chkSCYesMachinary.Checked = bool.Parse(DtForUnitBill.Rows[0]["SCMachinary"].ToString());
                        chkSCYesMaterial.Checked = bool.Parse(DtForUnitBill.Rows[0]["SCMaterial"].ToString());
                        chkSCYesElectrical.Checked = bool.Parse(DtForUnitBill.Rows[0]["SCMaintenance"].ToString());
                        chkSCYesExtraone.Checked = bool.Parse(DtForUnitBill.Rows[0]["SCExtraone"].ToString());
                        chkSCYesExtratwo.Checked = bool.Parse(DtForUnitBill.Rows[0]["SCExtratwo"].ToString());

                        checkExtraData.Checked = bool.Parse(DtForUnitBill.Rows[0]["Extradatacheck"].ToString());

                        if (String.IsNullOrEmpty(DtForUnitBill.Rows[0]["Extradatacheck"].ToString()) == false)
                        {
                            Extradatacheck = Boolean.Parse(DtForUnitBill.Rows[0]["Extradatacheck"].ToString());
                            if (Extradatacheck == true)
                            {
                                checkExtraData.Checked = true;
                            }
                            else
                            {
                                checkExtraData.Checked = false;
                            }
                        }


                        if (checkExtraData.Checked == true)
                        {
                            if (Chk_Month.Checked == false)
                            {
                                if (ddlclientid.SelectedIndex > 0 && ddlmonth.SelectedIndex > 0)
                                {
                                    panelRemarks.Visible = true;
                                }
                            }
                            if (Chk_Month.Checked == true)
                            {
                                if (ddlclientid.SelectedIndex > 0 && txtmonth.Text.Trim().Length > 0)
                                {
                                    panelRemarks.Visible = true;
                                }
                            }
                        }

                        lblroundoffs.Visible = true;
                        txtRoundoffamt.Visible = true;
                        txtRoundoffamt.Text = DtForUnitBill.Rows[0]["RoundOffAmt"].ToString();


                        string GTotal = Convert.ToDecimal(lblGrandTotal.Text).ToString("0.00");
                        string[] arr = GTotal.ToString().Split("."[0]);
                        string inwords = "";
                        string rupee = (arr[0]);
                        string paise = "";
                        if (arr.Length == 2)
                        {
                            if (arr[1].Length > 0 && arr[1] != "00")
                            {
                                paise = (arr[1]);
                            }
                        }

                        if (paise != "0.00" && paise != "0" && paise != "")
                        {
                            int I = Int16.Parse(paise);
                            String p = NumberToEnglish.Instance.NumbersToWords(I, true);
                            paise = p;
                            rupee = NumberToEnglish.Instance.NumbersToWords(Convert.ToInt64(arr[0]), false);
                            inwords = " Rupees " + rupee + "" + paise + " Paise Only";

                        }
                        else
                        {
                            rupee = NumberToEnglish.Instance.NumbersToWords(Convert.ToInt64(arr[0]), true);
                            inwords = " Rupees " + rupee + " Only";
                        }

                        lblamtinwords.Text = inwords;
                    }

                    #endregion
                }
                else
                {

                    gvClientBilling.DataSource = null;
                    gvClientBilling.DataBind();
                    FillDefaultGird();
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "bindautofilldesgs", "bindautofilldesgs();", true);
                #endregion

                VisibleFreeze();

            }
            catch (Exception ex)
            {

            }
        }

        public void FillDefaultGird()
        {
            DataTable DefaultTable = new DataTable();
            DefaultTable.Columns.Add("Sid", typeof(int));
            DefaultTable.Columns.Add("Designid", typeof(string));
            DefaultTable.Columns.Add("Type", typeof(string));
            DefaultTable.Columns.Add("Designation", typeof(string));
            DefaultTable.Columns.Add("HSNNumber", typeof(string));
            DefaultTable.Columns.Add("UOM", typeof(string));
            DefaultTable.Columns.Add("NoofEmps", typeof(string));
            DefaultTable.Columns.Add("DutyHours", typeof(string));
            DefaultTable.Columns.Add("payrate", typeof(string));
            DefaultTable.Columns.Add("newpayrate", typeof(string));
            DefaultTable.Columns.Add("paytype", typeof(string));
            DefaultTable.Columns.Add("ServiceChargesPrc", typeof(string));
            DefaultTable.Columns.Add("ServiceCharges", typeof(string));
            DefaultTable.Columns.Add("BasicDa", typeof(string));
            DefaultTable.Columns.Add("OTAmount", typeof(string));
            DefaultTable.Columns.Add("NoOfDays", typeof(string));
            DefaultTable.Columns.Add("Totalamount", typeof(string));
            DefaultTable.Columns.Add("Remarks", typeof(string));
            DefaultTable.Columns.Add("GSTPer", typeof(string));
            DefaultTable.Columns.Add("CGSTPrc", typeof(string));
            DefaultTable.Columns.Add("CGSTAmt", typeof(string));
            DefaultTable.Columns.Add("SGSTPrc", typeof(string));
            DefaultTable.Columns.Add("SGSTAmt", typeof(string));
            DefaultTable.Columns.Add("IGSTPrc", typeof(string));
            DefaultTable.Columns.Add("IGSTAmt", typeof(string));
            DefaultTable.Columns.Add("TotalTaxAmount", typeof(string));
            DefaultTable.Columns.Add("CalnType", typeof(string));

            var cid = ddlclientid.SelectedValue;

            int noOfDaysInMonth = 0;

            if (!cid.Equals("--Select--"))
            {
                #region Month Selection
                int month = 0;
                month = GetMonthBasedOnSelectionDateorMonth();


                #endregion


                #region  Begin Get Contract Id Based on The Last Day

                DateTime DtLastDay = DateTime.Now;
                if (Chk_Month.Checked == false)
                {
                    DtLastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlmonth.SelectedIndex);
                }
                if (Chk_Month.Checked == true)
                {
                    DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                }
                var ContractID = "";
                Hashtable HtGetContractID = new Hashtable();
                var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
                HtGetContractID.Add("@LastDay", DtLastDay);
                DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                if (DTContractID.Rows.Count > 0)
                {
                    ContractID = DTContractID.Rows[0]["contractid"].ToString();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not  Avaialable For This Client.');", true);
                    return;
                }

                #endregion  End Get Contract Id Based on The Last Day

                int prevmonth = 0;

                if (ddlmonth.SelectedIndex == 1)
                {
                    prevmonth = Timings.Instance.GetIdForPreviousMonth();
                }

                if (ddlmonth.SelectedIndex == 2)
                {
                    prevmonth = Timings.Instance.GetIdForPreviousOneMonth();
                }

                if (ddlmonth.SelectedIndex == 3)
                {
                    prevmonth = Timings.Instance.GetIdForPreviousTwoMonth();
                }

                string year = "";
                string monval = "";

                if (month != 0)
                {

                    if (month.ToString().Length == 3)
                    {
                        monval = month.ToString().Substring(0, 1);
                        year = "20" + month.ToString().Substring(1, 2);
                    }
                    else if (month.ToString().Length == 4)
                    {
                        monval = month.ToString().Substring(0, 2);
                        year = "20" + month.ToString().Substring(2, 2);
                    }

                    noOfDaysInMonth = DateTime.DaysInMonth(int.Parse(year), int.Parse(monval));
                }

                var queryn = @"select * from Contracts where ClientID =  '" + ddlclientid.SelectedValue + "' AND CONTRACTID='" + ContractID + "'";
                var contractdetails = config.ExecuteReaderWithQueryAsync(queryn).Result;

                var CCGST = false;
                var CSGST = false;
                var CIGST = false;
                var STInclude = false;
                var GSTLineItem = false;

                if (contractdetails.Rows.Count > 0)
                {
                    CCGST = bool.Parse(contractdetails.Rows[0]["CGST"].ToString());
                    CSGST = bool.Parse(contractdetails.Rows[0]["SGST"].ToString());
                    CIGST = bool.Parse(contractdetails.Rows[0]["IGST"].ToString());
                    STInclude = bool.Parse(contractdetails.Rows[0]["IncludeST"].ToString());
                    GSTLineItem = bool.Parse(contractdetails.Rows[0]["GSTLineItem"].ToString());

                }

                var query1 = @"select CGST,SGST,IGST from TblOptions where '" + DtLastDay + "' between fromdate and todate ";
                var optiondetails = config.ExecuteReaderWithQueryAsync(query1).Result;

                decimal CGSTprc = 0;
                decimal SGSTprc = 0;
                decimal IGSTprc = 0;
                decimal GSTprc = 0;

                if (optiondetails.Rows.Count > 0)
                {
                    CGSTprc = Convert.ToDecimal(optiondetails.Rows[0]["CGST"].ToString());
                    SGSTprc = Convert.ToDecimal(optiondetails.Rows[0]["SGST"].ToString());
                    IGSTprc = Convert.ToDecimal(optiondetails.Rows[0]["IGST"].ToString());
                }
                else
                {
                    lblResult.Text = "There Is No Tax Values For Generating Bills ";
                    return;
                }

                if (GSTLineItem == true)
                {
                    if (CCGST)
                    {
                        GSTprc = CGSTprc + SGSTprc;
                    }

                    if (CIGST)
                    {
                        GSTprc = IGSTprc;
                    }
                }

                string sqlqry = "select max(isnull(munitidbillno,'')) as billno from MUnitBillBreakup where  UnitId='" + ddlclientid.SelectedValue + "' and month='" + prevmonth + "'";
                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
                string MaxBillno = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    MaxBillno = dt.Rows[0]["billno"].ToString();

                    if (MaxBillno != "")
                    {
                        var query = @"select Designation,NoofEmps,DutyHours,PayRate,PayRateType as paytype,monthlydays as NoOfDays,BasicDa,Totalamount,0  as NewPayRate,isnull(otamount,0) as otamount,'' as designid,remarks,'' as type,0 as DutyHrs,HSNNumber,UOM,'' as Extra,'" + GSTprc + "' as GSTper,isnull(CGSTAmt,0) as CGSTAmt,isnull(CGSTprc,0) as CGSTprc,isnull(SGSTAmt,0) as SGSTAmt,isnull(SGSTprc,0) as SGSTprc,isnull(IGSTAmt,0) as IGSTAmt,isnull(IGSTprc,0) as IGSTprc,(Totalamount+isnull(CGSTAmt,0)+isnull(SGSTAmt,0)+isnull(IGSTAmt,0) ) as TotalTaxAmount,'Add' as CalnType,isnull(ServiceChargesPrc,0) as ServiceChargesPrc,isnull(ServiceCharges,0) as ServiceCharges   from MUnitBillBreakup  where UnitId='" + ddlclientid.SelectedValue + "' and month='" + prevmonth + "' and munitidbillno='" + MaxBillno + "' order by sino ";
                        var griddata = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
                        DefaultTable = griddata;

                    }
                    else
                    {


                        var query = @"select d.Design as Designation,'' as designid,
	                               ISNULL(cd.Quantity,0) as NoofEmps,
                                   0 as DutyHrs,
	                               ISNULL(cad.Duties,0) as DutyHours,
	                               ISNULL(Amount,0) as payrate,
                                    0 as newpayrate,'' as UOM,
	                               cd.PayType as paytype,0 as BasicDa,0 as OTAmount,cd.NoOfDays,0 as Totalamount,'' as Remarks,'' as type,'' as HSNNumber,'' as Extra,0 as ServiceChargesPrc,0 as ServiceCharges,
								  '" + GSTprc + "' as GSTper, 0 as CGSTprc,0 as CGSTAmt,0 as SGSTAmt,0 as SGSTprc,0 as IGSTAmt,0 as IGSTprc,0 as TotalTaxAmount,'Add' as CalnType from Designations d inner join ContractDetails cd on d.DesignId = cd.Designations left outer join ClientAttenDance cad on cd.ClientID = cad.ClientId and cd.Designations = cad.Desingnation and cad.[month]= " + month.ToString() + " where cd.ClientID = '" + cid + "' and cd.contractid='" + ContractID + "'";


                        var griddata = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
                        DefaultTable = griddata;

                        displaydata();
                    }
                }
                else
                {
                    var query = @"select d.Design as Designation,'' as designid,
	                               ISNULL(cd.Quantity,0) as NoofEmps,
                                   0 as DutyHrs,
	                               ISNULL(cad.Duties,0) as DutyHours,
	                               ISNULL(Amount,0) as payrate,
                                    0 as newpayrate,'' as UOM,
	                               cd.PayType as paytype,0 as BasicDa,0 as OTAmount,cd.NoOfDays,0 as Totalamount,'' as Remarks,'' as type,'' as HSNNumber,'' as Extra,0 as ServiceChargesPrc,0 as ServiceCharges,
								  '" + GSTprc + "' as GSTper, 0 as CGSTprc,0 as CGSTAmt,0 as SGSTAmt,0 as SGSTprc,0 as IGSTAmt,0 as IGSTprc,0 as TotalTaxAmount,'Add' as CalnType from Designations d inner join ContractDetails cd on d.DesignId = cd.Designations left outer join ClientAttenDance cad on cd.ClientID = cad.ClientId and cd.Designations = cad.Desingnation and cad.[month]= " + month.ToString() + " where cd.ClientID = '" + cid + "' and cd.contractid='" + ContractID + "'";


                    var griddata = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;

                    DefaultTable = griddata;
                }
            }

            var sl = DefaultTable.Rows.Count;
            var count = DefaultTable.Rows.Count > 0 ? DefaultTable.Rows.Count + 5 : 1;

            for (int i = sl + 1; i < count; i++)
            {
                DataRow dr = DefaultTable.NewRow();
                //dr["Sid"] = i;
                dr["Designation"] = "";
                dr["designid"] = "";
                dr["NoofEmps"] = 0;
                dr["Type"] = "";
                dr["DutyHours"] = 0;
                dr["payrate"] = 0;
                dr["newpayrate"] = 0;
                dr["paytype"] = 0;
                dr["BasicDa"] = 0;
                dr["OTAmount"] = 0;
                dr["NoOfDays"] = 1;
                dr["ServiceChargesPrc"] = 0;
                dr["ServiceCharges"] = 0;
                dr["Totalamount"] = 0;
                dr["Remarks"] = "";
                dr["GSTPer"] = 0;
                dr["CGSTPrc"] = 0;
                dr["CGSTAmt"] = 0;
                dr["SGSTPrc"] = 0;
                dr["SGSTAmt"] = 0;
                dr["IGSTPrc"] = 0;
                dr["IGSTAmt"] = 0;
                dr["TotalTaxAmount"] = 0;
                dr["HSNNumber"] = "0";
                dr["UOM"] = "";

                DefaultTable.Rows.Add(dr);
            }

            ViewState["DTDefaultManual"] = DefaultTable;
            gvClientBilling.DataSource = DefaultTable;
            gvClientBilling.DataBind();

            displaydata();

            for (int i = 0; i < DefaultTable.Rows.Count; i++)
            {
                DropDownList CDutytype = gvClientBilling.Rows[i].FindControl("ddldutytype") as DropDownList;

                if (String.IsNullOrEmpty(DefaultTable.Rows[i]["paytype"].ToString()) != false)
                {
                    CDutytype.SelectedIndex = 0;
                }
                else
                {
                    if (DefaultTable.Rows[i]["paytype"].ToString().Trim().Length > 0)
                    {
                        CDutytype.SelectedIndex = Convert.ToInt32(DefaultTable.Rows[i]["paytype"].ToString().Trim());
                    }
                }

                DropDownList ddlHSNNumber = gvClientBilling.Rows[i].FindControl("ddlHSNNumber") as DropDownList;

                if (String.IsNullOrEmpty(DefaultTable.Rows[i]["HSNNumber"].ToString()) != false)
                {
                    ddlHSNNumber.SelectedIndex = 0;
                }
                else
                {
                    if (DefaultTable.Rows[i]["HSNNumber"].ToString().Trim().Length > 0)
                    {
                        ddlHSNNumber.SelectedValue = DefaultTable.Rows[i]["HSNNumber"].ToString();
                    }
                }

                DropDownList ddlnod = gvClientBilling.Rows[i].FindControl("ddlnod") as DropDownList;

                if (String.IsNullOrEmpty(DefaultTable.Rows[i]["NoOfDays"].ToString()) != false)
                {
                    ddlnod.SelectedIndex = 0;
                }
                else
                {
                    if (DefaultTable.Rows[i]["NoOfDays"].ToString().Trim().Length > 0)
                    {
                        ddlnod.SelectedValue = noOfDaysInMonth.ToString();
                    }
                }

                DropDownList ddlCalnType = gvClientBilling.Rows[i].FindControl("ddlCalnType") as DropDownList;

                if (String.IsNullOrEmpty(DefaultTable.Rows[i]["CalnType"].ToString()) != false)
                {
                    ddlCalnType.SelectedIndex = 0;
                }
                else
                {
                    if (DefaultTable.Rows[i]["CalnType"].ToString().Trim().Length > 0)
                    {
                        ddlCalnType.SelectedValue = DefaultTable.Rows[i]["CalnType"].ToString();
                    }
                }

            }

            btnAddNewRow.Visible = (gvClientBilling.Rows.Count > 0);
            btnCalculateTotals.Visible = (gvClientBilling.Rows.Count > 0);

            ScriptManager.RegisterStartupScript(this, GetType(), "bindautofilldesgs", "bindautofilldesgs();", true);

        }

        protected void btnAddNewRow_Click(object sender, EventArgs e)
        {
            int month = GetMonthBasedOnSelectionDateorMonth();
            string SP = "EinvGetBillDetails";
            string Type = "GetIRN";
            Hashtable ht = new Hashtable();
            ht.Add("@Type", Type);
            ht.Add("@month", month);
            ht.Add("@Clientid", ddlclientid.SelectedValue);
            ht.Add("@Billno", lblbillnolatest.Text);
            ht.Add("@BillType", ddlType.SelectedValue);
            DataTable dtn = config.ExecuteAdaptorAsyncWithParams(SP, ht).Result;
            if (dtn.Rows.Count > 0 && dtn.Rows[0]["IRN"].ToString() != "" && dtn.Rows[0]["status"].ToString() == "ACT")
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Bill cannot be regenerated as IRN is already generated. ');", true);
                return;
            }

            else if (dtn.Rows.Count > 0 && dtn.Rows[0]["status"].ToString() == "CNL")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Bill cannot be regenerated as IRN is cancelled. ');", true);
                return;
            }

            else
            {
                if (ddlType.SelectedIndex == 0)
                {

                    DataTable dt = new DataTable();
                    dt.Columns.Add("sno");
                    dt.Columns.Add("Designid");
                    dt.Columns.Add("type");
                    dt.Columns.Add("UOM");
                    dt.Columns.Add("Noofdays");
                    dt.Columns.Add("Designation");
                    dt.Columns.Add("NoofEmps");
                    dt.Columns.Add("HSNNumber");
                    dt.Columns.Add("DutyHours");
                    dt.Columns.Add("payrate");
                    dt.Columns.Add("newpayrate");
                    dt.Columns.Add("BasicDa");
                    dt.Columns.Add("OTAmount");
                    dt.Columns.Add("Totalamount");
                    dt.Columns.Add("ServiceChargesPrc");
                    dt.Columns.Add("ServiceCharges");
                    dt.Columns.Add("GSTPer");
                    dt.Columns.Add("CGSTPrc");
                    dt.Columns.Add("CGSTAmt");
                    dt.Columns.Add("SGSTPrc");
                    dt.Columns.Add("SGSTAmt");
                    dt.Columns.Add("IGSTPrc");
                    dt.Columns.Add("IGSTAmt");
                    dt.Columns.Add("TotalTaxAmount");
                    dt.Columns.Add("Extra");
                    dt.Columns.Add("CalnType");

                    foreach (GridViewRow gvRow in gvClientBilling.Rows)
                    {

                        DataRow dr = dt.NewRow();
                        dr["sno"] = ((Label)gvRow.FindControl("lblSno")).Text;
                        dr["designid"] = ((Label)gvRow.FindControl("lbldesignid")).Text;
                        dr["type"] = ((DropDownList)gvRow.FindControl("ddldutytype")).SelectedValue;
                        dr["Noofdays"] = ((DropDownList)gvRow.FindControl("ddlnod")).SelectedValue;
                        dr["Designation"] = ((TextBox)gvRow.FindControl("lbldesgn")).Text;
                        dr["NoofEmps"] = ((TextBox)gvRow.FindControl("lblnoofemployees")).Text;
                        dr["DutyHours"] = ((TextBox)gvRow.FindControl("lblNoOfDuties")).Text; ;
                        dr["payrate"] = ((TextBox)gvRow.FindControl("lblpayrate")).Text;
                        dr["newpayrate"] = ((TextBox)gvRow.FindControl("txtNewPayRate")).Text;
                        dr["BasicDa"] = ((TextBox)gvRow.FindControl("lblda")).Text;
                        dr["OTAmount"] = ((Label)gvRow.FindControl("lblOtAmount")).Text;
                        dr["Totalamount"] = ((TextBox)gvRow.FindControl("lblAmount")).Text;
                        dr["ServiceChargesPrc"] = ((TextBox)gvRow.FindControl("lblSchrgPrc")).Text;
                        dr["ServiceCharges"] = ((TextBox)gvRow.FindControl("lblSchrgAmt")).Text;
                        dr["GSTPer"] = ((TextBox)gvRow.FindControl("lblGSTper")).Text;
                        dr["CGSTPrc"] = ((TextBox)gvRow.FindControl("lblCGSTPrc")).Text;
                        dr["CGSTAmt"] = ((TextBox)gvRow.FindControl("lblCGSTAmount")).Text;
                        dr["SGSTPrc"] = ((TextBox)gvRow.FindControl("lblSGSTPrc")).Text;
                        dr["SGSTAmt"] = ((TextBox)gvRow.FindControl("lblSGSTAmount")).Text;
                        dr["IGSTPrc"] = ((TextBox)gvRow.FindControl("lblIGSTPrc")).Text;
                        dr["IGSTAmt"] = ((TextBox)gvRow.FindControl("lblIGSTAmount")).Text;
                        dr["TotalTaxAmount"] = ((TextBox)gvRow.FindControl("lblTotalTaxmount")).Text;
                        dr["HSNNumber"] = ((DropDownList)gvRow.FindControl("ddlHSNNumber")).SelectedValue;
                        dr["Extra"] = ((Label)gvRow.FindControl("lblextra")).Text;
                        dr["CalnType"] = ((DropDownList)gvRow.FindControl("ddlCalnType")).SelectedValue;
                        ((Label)gvRow.FindControl("lblnoofdays")).Text = ((DropDownList)gvRow.FindControl("ddlnod")).SelectedValue;
                        ((Label)gvRow.FindControl("txthsnno")).Text = ((DropDownList)gvRow.FindControl("ddlHSNNumber")).SelectedValue;
                        ((Label)gvRow.FindControl("lblcaln")).Text = ((DropDownList)gvRow.FindControl("ddlCalnType")).SelectedValue;
                        dt.Rows.Add(dr);
                    }

                    DataRow dr1 = dt.NewRow();
                    dr1["sno"] = "";
                    dr1["Designation"] = "";
                    dr1["designid"] = "";
                    dr1["NoofEmps"] = "0";
                    dr1["type"] = "0";
                    dr1["Noofdays"] = "0";
                    dr1["DutyHours"] = "0";
                    dr1["payrate"] = "0";
                    dr1["newpayrate"] = "0";
                    dr1["BasicDa"] = "0";
                    dr1["OTAmount"] = "9999999999";
                    dr1["Totalamount"] = "0";
                    dr1["ServiceChargesPrc"] = "0";
                    dr1["ServiceCharges"] = "0";
                    dr1["GSTPer"] = "0";
                    dr1["CGSTPrc"] = "0";
                    dr1["CGSTAmt"] = "0";
                    dr1["SGSTPrc"] = "0";
                    dr1["SGSTAmt"] = "0";
                    dr1["IGSTPrc"] = "0";
                    dr1["IGSTAmt"] = "0";
                    dr1["TotalTaxAmount"] = "0";
                    dr1["HSNNumber"] = "0";
                    dr1["Extra"] = "E";
                    dr1["CalnType"] = "Add";

                    dt.Rows.Add(dr1);

                    gvClientBilling.DataSource = dt;
                    gvClientBilling.DataBind();

                    displaydata();

                    foreach (GridViewRow gvRow in gvClientBilling.Rows)
                    {
                        if (gvRow.RowType == DataControlRowType.DataRow)
                        {

                            ((DropDownList)gvRow.FindControl("ddlnod")).SelectedValue = ((Label)gvRow.FindControl("lblnoofdays")).Text;
                            ((DropDownList)gvRow.FindControl("ddlHSNNumber")).SelectedValue = ((Label)gvRow.FindControl("txthsnno")).Text;
                            ((DropDownList)gvRow.FindControl("ddlCalnType")).SelectedValue = ((Label)gvRow.FindControl("lblcaln")).Text;

                            if (((Label)gvRow.FindControl("lblextra")).Text.Contains("E"))
                            {
                                (gvRow.FindControl("lbldesgn") as TextBox).Enabled = true;
                                (gvRow.FindControl("ddldutytype") as DropDownList).Enabled = true;
                                (gvRow.FindControl("lblnoofemployees") as TextBox).Enabled = true;
                                (gvRow.FindControl("lblNoOfDuties") as TextBox).Enabled = true;
                                (gvRow.FindControl("lblpayrate") as TextBox).Enabled = true;
                                (gvRow.FindControl("lblda") as TextBox).Enabled = true;
                                (gvRow.FindControl("ddlHSNNumber") as DropDownList).Enabled = true;
                                (gvRow.FindControl("lblAmount") as TextBox).Enabled = true;
                                (gvRow.FindControl("lblTotalTaxmount") as TextBox).Enabled = true;
                                (gvRow.FindControl("lblSchrgPrc") as TextBox).Enabled = true;
                                (gvRow.FindControl("lblSchrgAmt") as TextBox).Enabled = true;
                                (gvRow.FindControl("chkExtra") as CheckBox).Checked = true;
                                (gvRow.FindControl("ddlnod") as DropDownList).Enabled = true;
                                (gvRow.FindControl("lblGSTper") as TextBox).Enabled = true;
                                (gvRow.FindControl("lblCGSTAmount") as TextBox).Enabled = true;
                                (gvRow.FindControl("lblSGSTAmount") as TextBox).Enabled = true;
                                (gvRow.FindControl("lblIGSTAmount") as TextBox).Enabled = true;
                                (gvRow.FindControl("ddlCalnType") as DropDownList).Enabled = true;



                            }
                            else
                            {
                                (gvRow.FindControl("lbldesgn") as TextBox).Enabled = false;
                                (gvRow.FindControl("ddldutytype") as DropDownList).Enabled = false;
                                (gvRow.FindControl("lblnoofemployees") as TextBox).Enabled = false;
                                (gvRow.FindControl("lblNoOfDuties") as TextBox).Enabled = false;
                                (gvRow.FindControl("lblpayrate") as TextBox).Enabled = false;
                                (gvRow.FindControl("lblda") as TextBox).Enabled = false;
                                (gvRow.FindControl("ddlHSNNumber") as DropDownList).Enabled = false;
                                (gvRow.FindControl("lblAmount") as TextBox).Enabled = false;
                                (gvRow.FindControl("lblTotalTaxmount") as TextBox).Enabled = false;
                                (gvRow.FindControl("lblSchrgPrc") as TextBox).Enabled = false;
                                (gvRow.FindControl("lblSchrgAmt") as TextBox).Enabled = false;
                                (gvRow.FindControl("chkExtra") as CheckBox).Checked = false;
                                (gvRow.FindControl("ddlnod") as DropDownList).Enabled = false;
                                (gvRow.FindControl("lblGSTper") as TextBox).Enabled = false;
                                (gvRow.FindControl("lblCGSTAmount") as TextBox).Enabled = false;
                                (gvRow.FindControl("lblSGSTAmount") as TextBox).Enabled = false;
                                (gvRow.FindControl("lblIGSTAmount") as TextBox).Enabled = false;
                                (gvRow.FindControl("ddlCalnType") as DropDownList).Enabled = false;

                            }


                        }
                    }


                }

                else
                {
                    AddNewRowArrear();
                    EnabledFields();
                }
            }

        }

        protected void btnCalculateTotals_Click(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        protected void AddNewRowArrear()
        {
            try
            {



                int rowIndex = 0;

                if (ViewState["DTDefaultManual"] != null)
                {
                    DataTable dtCurrentTable = (DataTable)ViewState["DTDefaultManual"];
                    DataRow drCurrentRow = null;
                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                        {
                            //extract the TextBox values
                            TextBox txtgvdesgn = (TextBox)gvClientBilling.Rows[rowIndex].Cells[1].FindControl("lbldesgn");
                            DropDownList ddlHSNNumber = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[2].FindControl("ddlHSNNumber");
                            TextBox txtnoofemployees = (TextBox)gvClientBilling.Rows[rowIndex].Cells[3].FindControl("lblnoofemployees");
                            TextBox txtNoOfDuties = (TextBox)gvClientBilling.Rows[rowIndex].Cells[4].FindControl("lblNoOfDuties");
                            TextBox txtPayRate = (TextBox)gvClientBilling.Rows[rowIndex].Cells[5].FindControl("lblpayrate");
                            TextBox txtNewPayRate = (TextBox)gvClientBilling.Rows[rowIndex].Cells[6].FindControl("txtNewPayRate");
                            DropDownList ddldutytype = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[7].FindControl("ddldutytype");
                            DropDownList ddlnod = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[8].FindControl("ddlnod");
                            TextBox txtda = (TextBox)gvClientBilling.Rows[rowIndex].Cells[9].FindControl("lblda");
                            TextBox txtAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[10].FindControl("lblAmount");
                            TextBox lblSchrgPrc = (TextBox)gvClientBilling.Rows[rowIndex].Cells[11].FindControl("lblSchrgPrc");
                            TextBox lblSchrgAmt = (TextBox)gvClientBilling.Rows[rowIndex].Cells[12].FindControl("lblSchrgAmt");
                            TextBox lblGSTper = (TextBox)gvClientBilling.Rows[rowIndex].Cells[13].FindControl("lblGSTper");
                            TextBox lblCGSTAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[14].FindControl("lblCGSTAmount");
                            TextBox lblSGSTAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[15].FindControl("lblSGSTAmount");
                            TextBox lblIGSTAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[16].FindControl("lblIGSTAmount");
                            TextBox lblTotalTaxmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[17].FindControl("lblTotalTaxmount");


                            drCurrentRow = dtCurrentTable.NewRow();
                            // drCurrentRow["Sid"] = i + 1;

                            dtCurrentTable.Rows[i - 1]["Designation"] = txtgvdesgn.Text;
                            dtCurrentTable.Rows[i - 1]["HSNNumber"] = ddlHSNNumber.SelectedValue;
                            dtCurrentTable.Rows[i - 1]["NoofEmps"] = txtnoofemployees.Text.Trim() == "" ? 0 : Convert.ToInt32(txtnoofemployees.Text);
                            dtCurrentTable.Rows[i - 1]["DutyHours"] = txtNoOfDuties.Text.Trim() == "" ? 0 : Convert.ToSingle(txtNoOfDuties.Text);
                            dtCurrentTable.Rows[i - 1]["payrate"] = txtPayRate.Text.Trim() == "" ? 0 : Convert.ToSingle(txtPayRate.Text);
                            dtCurrentTable.Rows[i - 1]["newpayrate"] = txtNewPayRate.Text.Trim() == "" ? 0 : Convert.ToSingle(txtNewPayRate.Text);
                            dtCurrentTable.Rows[i - 1]["paytype"] = ddldutytype.SelectedValue;
                            dtCurrentTable.Rows[i - 1]["NoOfDays"] = ddlnod.SelectedValue;
                            dtCurrentTable.Rows[i - 1]["BasicDa"] = txtda.Text.Trim() == "" ? 0 : Convert.ToSingle(txtda.Text);
                            dtCurrentTable.Rows[i - 1]["Totalamount"] = txtAmount.Text.Trim() == "" ? 0 : Convert.ToSingle(txtAmount.Text);
                            dtCurrentTable.Rows[i - 1]["ServiceChargesPrc"] = lblSchrgPrc.Text.Trim() == "" ? 0 : Convert.ToSingle(lblSchrgPrc.Text);
                            dtCurrentTable.Rows[i - 1]["ServiceCharges"] = lblSchrgAmt.Text.Trim() == "" ? 0 : Convert.ToSingle(lblSchrgAmt.Text);
                            dtCurrentTable.Rows[i - 1]["GSTper"] = lblGSTper.Text.Trim() == "" ? 0 : Convert.ToSingle(lblGSTper.Text);
                            dtCurrentTable.Rows[i - 1]["CGSTAmt"] = lblCGSTAmount.Text.Trim() == "" ? 0 : Convert.ToSingle(lblCGSTAmount.Text);
                            dtCurrentTable.Rows[i - 1]["SGSTAmt"] = lblSGSTAmount.Text.Trim() == "" ? 0 : Convert.ToSingle(lblSGSTAmount.Text);
                            dtCurrentTable.Rows[i - 1]["IGSTAmt"] = lblIGSTAmount.Text.Trim() == "" ? 0 : Convert.ToSingle(lblIGSTAmount.Text);
                            dtCurrentTable.Rows[i - 1]["TotalTaxAmount"] = lblTotalTaxmount.Text.Trim() == "" ? 0 : Convert.ToSingle(lblTotalTaxmount.Text);


                            rowIndex++;
                        }
                        dtCurrentTable.Rows.Add(drCurrentRow);
                        ViewState["DTDefaultManual"] = dtCurrentTable;

                        gvClientBilling.DataSource = dtCurrentTable;
                        gvClientBilling.DataBind();

                        displaydata();
                    }
                }
                else
                {
                    Response.Write("ViewState is null");
                }
                SetPreviousDataArrear();

            }
            catch (Exception ex)
            {

            }
        }

        private void SetPreviousDataArrear()
        {
            int rowIndex = 0;
            if (ViewState["DTDefaultManual"] != null)
            {
                DataTable dt = (DataTable)ViewState["DTDefaultManual"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {




                        TextBox txtgvdesgn = (TextBox)gvClientBilling.Rows[rowIndex].Cells[1].FindControl("lbldesgn");
                        DropDownList ddlHSNNumber = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[2].FindControl("ddlHSNNumber");
                        TextBox txtnoofemployees = (TextBox)gvClientBilling.Rows[rowIndex].Cells[3].FindControl("lblnoofemployees");
                        TextBox txtNoOfDuties = (TextBox)gvClientBilling.Rows[rowIndex].Cells[4].FindControl("lblNoOfDuties");
                        TextBox txtPayRate = (TextBox)gvClientBilling.Rows[rowIndex].Cells[5].FindControl("lblpayrate");
                        TextBox txtNewPayRate = (TextBox)gvClientBilling.Rows[rowIndex].Cells[6].FindControl("txtNewPayRate");
                        DropDownList ddldutytype = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[7].FindControl("ddldutytype");
                        DropDownList ddlnod = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[8].FindControl("ddlnod");
                        TextBox txtda = (TextBox)gvClientBilling.Rows[rowIndex].Cells[9].FindControl("lblda");
                        TextBox txtAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[10].FindControl("lblAmount");
                        TextBox lblSchrgPrc = (TextBox)gvClientBilling.Rows[rowIndex].Cells[11].FindControl("lblSchrgPrc");
                        TextBox lblSchrgAmt = (TextBox)gvClientBilling.Rows[rowIndex].Cells[12].FindControl("lblSchrgAmt");
                        TextBox lblGSTper = (TextBox)gvClientBilling.Rows[rowIndex].Cells[13].FindControl("lblGSTper");
                        TextBox lblCGSTAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[14].FindControl("lblCGSTAmount");
                        TextBox lblSGSTAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[15].FindControl("lblSGSTAmount");
                        TextBox lblIGSTAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[16].FindControl("lblIGSTAmount");
                        TextBox lblTotalTaxmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[17].FindControl("lblTotalTaxmount");


                        txtgvdesgn.Text = dt.Rows[i]["Designation"].ToString();
                        ddlHSNNumber.SelectedValue = dt.Rows[i]["HSNNumber"].ToString();
                        txtnoofemployees.Text = dt.Rows[i]["NoofEmps"].ToString();
                        txtNoOfDuties.Text = dt.Rows[i]["DutyHours"].ToString();
                        txtPayRate.Text = dt.Rows[i]["payrate"].ToString();
                        txtNewPayRate.Text = dt.Rows[i]["newpayrate"].ToString();
                        ddldutytype.SelectedValue = dt.Rows[i]["paytype"].ToString();
                        ddlnod.SelectedValue = dt.Rows[i]["NoOfDays"].ToString();
                        txtda.Text = dt.Rows[i]["BasicDa"].ToString();
                        txtAmount.Text = dt.Rows[i]["Totalamount"].ToString();
                        lblSchrgPrc.Text = dt.Rows[i]["ServiceChargesPrc"].ToString();
                        lblSchrgAmt.Text = dt.Rows[i]["ServiceCharges"].ToString();
                        lblGSTper.Text = dt.Rows[i]["GSTper"].ToString();
                        lblCGSTAmount.Text = dt.Rows[i]["CGSTAmt"].ToString();
                        lblSGSTAmount.Text = dt.Rows[i]["SGSTAmt"].ToString();
                        lblIGSTAmount.Text = dt.Rows[i]["IGSTAmt"].ToString();
                        lblTotalTaxmount.Text = dt.Rows[i]["TotalTaxAmount"].ToString();

                        rowIndex++;
                    }
                }
            }
        }

        private void CalculateTotals()
        {
            try
            {

                DateTime DtLastDay = DateTime.Now;
                if (Chk_Month.Checked == false)
                {
                    DtLastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlmonth.SelectedIndex);
                }
                if (Chk_Month.Checked == true)
                {
                    DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                }

                var ContractID = "";


                Hashtable HtGetContractID = new Hashtable();
                var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
                HtGetContractID.Add("@LastDay", DtLastDay);
                DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                if (DTContractID.Rows.Count > 0)
                {
                    ContractID = DTContractID.Rows[0]["contractid"].ToString();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not Avaialable For This Client.');", true);

                }


                var cid = ddlclientid.SelectedValue;
                var query = @"select * from Contracts where ClientID =  '" + cid + "' AND CONTRACTID='" + ContractID + "'";
                var contractdetails = config.ExecuteReaderWithQueryAsync(query).Result;

                var CCGST = false;
                var CSGST = false;
                var CIGST = false;
                var STInclude = false;
                var GSTLineItem = false;
                var roundoff = false;

                if (contractdetails.Rows.Count > 0)
                {
                    CCGST = bool.Parse(contractdetails.Rows[0]["CGST"].ToString());
                    CSGST = bool.Parse(contractdetails.Rows[0]["SGST"].ToString());
                    CIGST = bool.Parse(contractdetails.Rows[0]["IGST"].ToString());
                    STInclude = bool.Parse(contractdetails.Rows[0]["IncludeST"].ToString());
                    GSTLineItem = bool.Parse(contractdetails.Rows[0]["GSTLineItem"].ToString());
                    roundoff = bool.Parse(contractdetails.Rows[0]["Roundoff"].ToString());
                }

                var billdtnew = DateTime.Now.ToString("dd/MM/yyyy");

                if (txtbilldate.Text != "")
                {
                    billdtnew = txtbilldate.Text;
                }
                DateTime dt = DateTime.ParseExact(billdtnew, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                // for both "1/1/2000" or "25/1/2000" formats
                string billdt = dt.ToString("MM/dd/yyyy");

                string CGSTAlias = "";
                string SGSTAlias = "";
                string IGSTAlias = "";
                string Cess1Alias = "";
                string Cess2Alias = "";
                string GSTINAlias = "";
                string OurGSTAlias = "";
                string SCPersent = "";
                decimal ServiceTaxSeparate = 0;
                decimal Cess = 0;
                decimal SHEcess = 0;
                decimal SBcess = 0;
                decimal KKcess = 0;
                decimal CGST = 0;
                decimal SGST = 0;
                decimal IGST = 0;
                decimal Cess1 = 0;
                decimal Cess2 = 0;

                decimal LineCGST = 0;
                decimal LineSGST = 0;
                decimal LineIGST = 0;
                decimal LineLessCGST = 0;
                decimal LineLessSGST = 0;
                decimal LineLessIGST = 0;


                var query1 = @"select ServiceTaxSeparate,Cess,SHECess,SBCess,KKCess,CGST,SGST,IGST,cess1,cess2,CGSTAlias,SGSTAlias,IGSTAlias,cess1Alias,cess2Alias,OurGSTINAlias OurGSTAlias,GSTINAlias from TblOptions where '" + billdt + "' between fromdate and todate ";
                var optiondetails = config.ExecuteReaderWithQueryAsync(query1).Result;

                if (optiondetails.Rows.Count > 0)
                {
                    CGSTAlias = optiondetails.Rows[0]["CGSTAlias"].ToString();
                    SGSTAlias = optiondetails.Rows[0]["SGSTAlias"].ToString();
                    IGSTAlias = optiondetails.Rows[0]["IGSTAlias"].ToString();
                    Cess1Alias = optiondetails.Rows[0]["Cess1Alias"].ToString();
                    Cess2Alias = optiondetails.Rows[0]["Cess2Alias"].ToString();
                    GSTINAlias = optiondetails.Rows[0]["GSTINAlias"].ToString();
                    OurGSTAlias = optiondetails.Rows[0]["OurGSTAlias"].ToString();
                    ServiceTaxSeparate = Convert.ToDecimal(optiondetails.Rows[0]["ServiceTaxSeparate"].ToString());
                    Cess = Convert.ToDecimal(optiondetails.Rows[0]["Cess"].ToString());
                    SHEcess = Convert.ToDecimal(optiondetails.Rows[0]["SHECess"].ToString());
                    SBcess = Convert.ToDecimal(optiondetails.Rows[0]["SBCess"].ToString());
                    KKcess = Convert.ToDecimal(optiondetails.Rows[0]["KKCess"].ToString());
                    CGST = Convert.ToDecimal(optiondetails.Rows[0]["CGST"].ToString());
                    SGST = Convert.ToDecimal(optiondetails.Rows[0]["SGST"].ToString());
                    IGST = Convert.ToDecimal(optiondetails.Rows[0]["IGST"].ToString());
                    Cess1 = Convert.ToDecimal(optiondetails.Rows[0]["Cess1"].ToString());
                    Cess2 = Convert.ToDecimal(optiondetails.Rows[0]["Cess2"].ToString());

                }
                else
                {
                    lblResult.Text = "There Is No Tax Values For Generating Bills ";
                    return;
                }


                decimal totalamt = 0;
                decimal totallessamt = 0;

                for (int i = 0; i < gvClientBilling.Rows.Count; i++)
                {
                    DropDownList ddldtype = gvClientBilling.Rows[i].FindControl("ddldutytype") as DropDownList;
                    DropDownList ddlnod = gvClientBilling.Rows[i].FindControl("ddlnod") as DropDownList;
                    TextBox txtDesg = gvClientBilling.Rows[i].FindControl("lbldesgn") as TextBox;
                    TextBox txtpayrate = gvClientBilling.Rows[i].FindControl("lblpayrate") as TextBox;
                    TextBox txtNewPayRate = gvClientBilling.Rows[i].FindControl("txtNewPayRate") as TextBox;
                    //HiddenField hdNOD = gvClientBilling.Rows[i].FindControl("hdNOD") as HiddenField;
                    TextBox txtnod = gvClientBilling.Rows[i].FindControl("lblNoOfDuties") as TextBox;
                    TextBox txtdutyamt = gvClientBilling.Rows[i].FindControl("lblda") as TextBox;
                    TextBox txtTotal = gvClientBilling.Rows[i].FindControl("lblAmount") as TextBox;
                    TextBox txtnoofemplyes = gvClientBilling.Rows[i].FindControl("lblnoofemployees") as TextBox;
                    //TextBox txtHSNNumber = gvClientBilling.Rows[i].FindControl("txtHSNNumber") as TextBox;
                    TextBox lblCGSTAmount = gvClientBilling.Rows[i].FindControl("lblCGSTAmount") as TextBox;
                    TextBox lblCGSTPrc = gvClientBilling.Rows[i].FindControl("lblCGSTPrc") as TextBox;
                    TextBox lblSGSTAmount = gvClientBilling.Rows[i].FindControl("lblSGSTAmount") as TextBox;
                    TextBox lblSGSTPrc = gvClientBilling.Rows[i].FindControl("lblSGSTPrc") as TextBox;
                    TextBox lblIGSTAmount = gvClientBilling.Rows[i].FindControl("lblIGSTAmount") as TextBox;
                    TextBox lblIGSTPrc = gvClientBilling.Rows[i].FindControl("lblIGSTPrc") as TextBox;
                    TextBox lblTotalTaxmount = gvClientBilling.Rows[i].FindControl("lblTotalTaxmount") as TextBox;
                    TextBox lblGSTper = gvClientBilling.Rows[i].FindControl("lblGSTper") as TextBox;
                    TextBox lblSchrgPrc = gvClientBilling.Rows[i].FindControl("lblSchrgPrc") as TextBox;
                    TextBox lblSchrgAmt = gvClientBilling.Rows[i].FindControl("lblSchrgAmt") as TextBox;

                    Label lblsrchrgs = gvClientBilling.Rows[i].FindControl("lblsrchrgs") as Label;
                    DropDownList ddlCalnType = gvClientBilling.Rows[i].FindControl("ddlCalnType") as DropDownList;

                    if (!string.IsNullOrEmpty(txtDesg.Text.Trim()))
                    {
                        switch (ddldtype.SelectedIndex)
                        {
                            case 4:
                                if (roundoff == false)
                                {
                                    txtdutyamt.Text = Convert.ToDecimal(txtTotal.Text = txtpayrate.Text).ToString("0");
                                }
                                else
                                {
                                    txtdutyamt.Text = Convert.ToDecimal(txtTotal.Text = txtpayrate.Text).ToString("0.00");

                                }
                                break;
                            case 1:
                            case 2:
                            case 3:

                                if (txtpayrate.Text == "")
                                {
                                    txtpayrate.Text = "0";
                                }

                                if (txtnod.Text == "")
                                {
                                    txtnod.Text = "0";
                                }

                                if (roundoff == false)
                                {
                                    txtdutyamt.Text = txtTotal.Text = (Convert.ToDecimal(txtpayrate.Text) * Convert.ToDecimal(txtnod.Text)).ToString("0");
                                }
                                else
                                {
                                    txtdutyamt.Text = txtTotal.Text = (Convert.ToDecimal(txtpayrate.Text) * Convert.ToDecimal(txtnod.Text)).ToString("0.00");
                                }

                                break;
                            case 0:
                                if (txtpayrate.Text == "")
                                {
                                    txtpayrate.Text = "0";
                                }

                                if (txtnod.Text == "")
                                {
                                    txtnod.Text = "0";
                                }

                                if (roundoff == false)
                                {
                                    txtdutyamt.Text = txtTotal.Text = (Convert.ToDecimal(txtpayrate.Text) / Convert.ToDecimal(ddlnod.SelectedValue) * Convert.ToDecimal(txtnod.Text)).ToString("0");
                                }
                                else
                                {
                                    txtdutyamt.Text = txtTotal.Text = Math.Round((Convert.ToDecimal(txtpayrate.Text) / Convert.ToDecimal(ddlnod.SelectedValue) * Convert.ToDecimal(txtnod.Text)), 2).ToString();
                                }
                                break;
                            default:
                                txtdutyamt.Text = txtTotal.Text = txtpayrate.Text;
                                break;
                        }


                        if (txtTotal.Text == "")
                        {
                            txtTotal.Text = "0";
                        }



                        if (txtpayrate.Text == "")
                        {
                            txtpayrate.Text = "0";
                        }

                        if (lblCGSTAmount.Text == "")
                        {
                            lblCGSTAmount.Text = "0";
                        }

                        if (lblSGSTAmount.Text == "")
                        {
                            lblSGSTAmount.Text = "0";
                        }

                        if (lblIGSTAmount.Text == "")
                        {
                            lblIGSTAmount.Text = "0";
                        }


                        if (lblCGSTPrc.Text == "")
                        {
                            lblCGSTPrc.Text = "0";
                        }

                        if (lblSGSTPrc.Text == "")
                        {
                            lblSGSTPrc.Text = "0";
                        }

                        if (lblIGSTPrc.Text == "")
                        {
                            lblIGSTPrc.Text = "0";
                        }

                        if (lblGSTper.Text == "")
                        {
                            lblGSTper.Text = "0";
                        }

                        if (TxtservicechrgPrc.Text == "")
                        {
                            TxtservicechrgPrc.Text = "0";
                        }


                        if (lblsrchrgs.Text == "")
                        {
                            lblsrchrgs.Text = "0";
                        }



                        if (!STInclude && GSTLineItem == true)
                        {
                            if (lblSchrgPrc.Text.Trim() == "")
                            {
                                lblSchrgPrc.Text = "0";
                            }
                            lblSchrgAmt.Text = ((decimal.Parse(txtTotal.Text)) / 100 * decimal.Parse(lblSchrgPrc.Text)).ToString("0.00");

                            if (lblGSTper.Text.Trim() == "")
                            {
                                lblGSTper.Text = "0";
                            }

                            if (CCGST)
                            {
                                lblCGSTPrc.Text = (decimal.Parse(lblGSTper.Text) / 2).ToString();
                                lblCGSTAmount.Text = ((decimal.Parse(txtTotal.Text) + decimal.Parse(lblsrchrgs.Text)) / 100 * decimal.Parse(lblCGSTPrc.Text)).ToString("0.00");

                                if (ddlCalnType.SelectedValue == "Subract")
                                {
                                    LineLessCGST += decimal.Parse(lblCGSTAmount.Text);
                                }
                                else
                                {
                                    LineCGST += decimal.Parse(lblCGSTAmount.Text);

                                }

                            }
                            else
                            {
                                lblCGSTAmount.Text = "0";
                                lblCGSTPrc.Text = "0";
                            }

                            if (CSGST)
                            {
                                lblSGSTPrc.Text = (decimal.Parse(lblGSTper.Text) / 2).ToString();
                                lblSGSTAmount.Text = ((decimal.Parse(txtTotal.Text) + decimal.Parse(lblsrchrgs.Text)) / 100 * decimal.Parse(lblSGSTPrc.Text)).ToString("0.00");

                                if (ddlCalnType.SelectedValue == "Subract")
                                {
                                    LineLessSGST += decimal.Parse(lblSGSTAmount.Text);
                                }
                                else
                                {
                                    LineSGST += decimal.Parse(lblSGSTAmount.Text);

                                }

                            }
                            else
                            {
                                lblSGSTAmount.Text = "0";
                                lblSGSTPrc.Text = "0";
                            }


                            if (CIGST)
                            {
                                lblIGSTPrc.Text = (decimal.Parse(lblGSTper.Text)).ToString();
                                lblIGSTAmount.Text = ((decimal.Parse(txtTotal.Text) + decimal.Parse(lblsrchrgs.Text)) / 100 * decimal.Parse(lblIGSTPrc.Text)).ToString("0.00");


                                if (ddlCalnType.SelectedValue == "Subract")
                                {
                                    LineLessIGST += decimal.Parse(lblIGSTAmount.Text);
                                }
                                else
                                {
                                    LineIGST += decimal.Parse(lblIGSTAmount.Text);


                                }
                            }
                            else
                            {
                                lblIGSTAmount.Text = "0";
                                lblIGSTPrc.Text = "0";
                            }



                            lblTotalTaxmount.Text = (decimal.Parse(txtTotal.Text) + decimal.Parse(lblsrchrgs.Text) + decimal.Parse(lblCGSTAmount.Text) + decimal.Parse(lblSGSTAmount.Text) + decimal.Parse(lblIGSTAmount.Text)).ToString("0.00");

                        }
                        else
                        {
                            if (!STInclude)
                            {
                                lblCGSTAmount.Text = (((decimal.Parse(txtTotal.Text)) * decimal.Parse(lblCGSTPrc.Text) / 100).ToString());
                                lblSGSTAmount.Text = (((decimal.Parse(txtTotal.Text)) * decimal.Parse(lblSGSTPrc.Text) / 100).ToString());
                                lblIGSTAmount.Text = (((decimal.Parse(txtTotal.Text)) * decimal.Parse(lblIGSTPrc.Text) / 100).ToString());
                                lblTotalTaxmount.Text = (decimal.Parse(txtTotal.Text) + decimal.Parse(lblCGSTAmount.Text) + decimal.Parse(lblSGSTAmount.Text) + decimal.Parse(lblIGSTAmount.Text)).ToString();

                            }
                        }

                        if (lblTotalTaxmount.Text == "")
                        {
                            lblTotalTaxmount.Text = "0";
                        }



                        if (ddlCalnType.SelectedValue == "Subract")
                        {
                            if (!string.IsNullOrEmpty(txtTotal.Text.Trim()))

                                if (roundoff == true)
                                {
                                    totallessamt += Math.Round((Convert.ToDecimal(txtTotal.Text.ToString())), 2);
                                }
                                else
                                {
                                    totallessamt += Math.Round((Convert.ToDecimal(txtTotal.Text.ToString())), 0);

                                }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(txtTotal.Text.Trim()))

                                if (roundoff == true)
                                {
                                    totalamt += Math.Round((Convert.ToDecimal(txtTotal.Text.ToString())), 2);
                                }
                                else
                                {
                                    totalamt += Math.Round((Convert.ToDecimal(txtTotal.Text.ToString())), 0);

                                }
                        }

                    }
                }






                decimal servicetax = 0;
                decimal cesstax = 0;
                decimal sbcesstax = 0;
                decimal kkcesstax = 0;
                decimal educess = 0;
                decimal gtotal = 0;
                decimal servicecharge = 0;
                decimal subtotal = 0;
                decimal Servicechargeamt = 0;
                decimal Machineryamt = 0;
                decimal Materialamt = 0;
                decimal extraonevalueamt = 0;
                decimal extratwovalueamt = 0;
                decimal Electicalamt = 0;
                decimal Discountsamt = 0;
                decimal discounttwovalueamt = 0;

                decimal CGSTTax = 0;   //gst
                decimal SGSTTax = 0;
                decimal IGSTTax = 0;
                decimal Cess1Tax = 0;
                decimal Cess2Tax = 0;

                decimal STCMachineryamt = 0;//Gst
                decimal STCMaterialamt = 0;
                decimal STCextraonevalueamt = 0;
                decimal STCextratwovalueamt = 0;
                decimal STCElecticalamt = 0;


                if (txtMachinery.Text.Length > 0)
                {
                    Machineryamt = Convert.ToDecimal(txtMachinery.Text);
                }
                if (txtMaterial.Text.Length > 0)
                {
                    Materialamt = Convert.ToDecimal(txtMaterial.Text);
                }
                if (txtextraonevalue.Text.Length > 0)
                {
                    extraonevalueamt = Convert.ToDecimal(txtextraonevalue.Text);
                }
                if (txtextratwovalue.Text.Length > 0)
                {
                    extratwovalueamt = Convert.ToDecimal(txtextratwovalue.Text);
                }
                if (txtElectical.Text.Length > 0)
                {
                    Electicalamt = Convert.ToDecimal(txtElectical.Text);
                }
                if (txtDiscounts.Text.Length > 0)
                {
                    Discountsamt = Convert.ToDecimal(txtDiscounts.Text);
                }
                if (txtdiscounttwovalue.Text.Length > 0)
                {
                    discounttwovalueamt = Convert.ToDecimal(txtdiscounttwovalue.Text);
                }


                if (chkSTYesMachinary.Checked == true)  //Gst
                {
                    STCMachineryamt = Machineryamt;
                }
                if (chkSTYesMaterial.Checked == true)
                {
                    STCMaterialamt = Materialamt;
                }
                if (chkSTYesExtraone.Checked == true)
                {
                    STCextraonevalueamt = extraonevalueamt;
                }
                if (chkSTYesExtratwo.Checked == true)
                {
                    STCextratwovalueamt = extratwovalueamt;
                }
                if (chkSTYesElectrical.Checked == true)
                {
                    STCElecticalamt = Electicalamt;
                }


                if (contractdetails.Rows.Count > 0)
                {
                    if (TxtservicechrgPrc.Text == "")
                    {
                        TxtservicechrgPrc.Text = "0";
                    }




                    //  Txtservicechrg.Text = contractdetails.Rows[0]["ServiceCharge"].ToString();
                    servicecharge = Convert.ToDecimal(TxtservicechrgPrc.Text);

                    totalamt = (totalamt - totallessamt);

                    if (roundoff == true)
                    {
                        lblServiceCharges.Text = (totalamt * (servicecharge / 100)).ToString("0.##");
                    }
                    else
                    {
                        lblServiceCharges.Text = (totalamt * (servicecharge / 100)).ToString("0");

                    }


                    if (lblServiceCharges.Text == "")
                    {
                        lblServiceCharges.Text = "0";
                    }
                    Servicechargeamt = Convert.ToDecimal(lblServiceCharges.Text);
                    subtotal = totalamt + Servicechargeamt;

                    if (roundoff == true)
                    {
                        lblTotalResources.Text = subtotal.ToString("0.##");
                    }
                    else
                    {
                        lblTotalResources.Text = subtotal.ToString("0");
                    }

                    if (contractdetails.Rows[0]["IncludeST"].ToString() == "True")
                    {
                        servicetax = 0;
                        cesstax = 0;
                        educess = 0;
                        sbcesstax = 0;
                        kkcesstax = 0;
                        CGSTTax = 0;   //gst
                        SGSTTax = 0;
                        IGSTTax = 0;
                        Cess1Tax = 0;
                        Cess2Tax = 0;

                    }
                    else if (contractdetails.Rows[0]["ServiceTax75"].ToString() == "True")
                    {
                        servicetax = 3 * totalamt / 100;
                        sbcesstax = 3 * totalamt / 100;
                        kkcesstax = 3 * totalamt / 100;
                        cesstax = 2 * servicetax / 100;
                        educess = 1 * servicetax / 100;
                    }
                    else
                    {
                        if (roundoff == true)
                        {
                            servicetax = Math.Round(ServiceTaxSeparate * (totalamt + Servicechargeamt + STCMachineryamt + STCMaterialamt + STCextraonevalueamt + STCextratwovalueamt + STCElecticalamt) / 100, 2);//GST
                        }
                        else
                        {
                            servicetax = Math.Round(ServiceTaxSeparate * (totalamt + Servicechargeamt + STCMachineryamt + STCMaterialamt + STCextraonevalueamt + STCextratwovalueamt + STCElecticalamt) / 100, 0);//GST
                        }

                        if (roundoff == true)
                        {
                            sbcesstax = Math.Round(SBcess * (totalamt + Servicechargeamt + STCMachineryamt + STCMaterialamt + STCextraonevalueamt + STCextratwovalueamt + STCElecticalamt) / 100, 2);
                        }
                        else
                        {
                            sbcesstax = Math.Round(SBcess * (totalamt + Servicechargeamt + STCMachineryamt + STCMaterialamt + STCextraonevalueamt + STCextratwovalueamt + STCElecticalamt) / 100, 0);
                        }

                        if (roundoff == true)
                        {
                            kkcesstax = Math.Round(KKcess * (totalamt + Servicechargeamt + STCMachineryamt + STCMaterialamt + STCextraonevalueamt + STCextratwovalueamt + STCElecticalamt) / 100, 2);
                        }
                        else
                        {
                            kkcesstax = Math.Round(KKcess * (totalamt + Servicechargeamt + STCMachineryamt + STCMaterialamt + STCextraonevalueamt + STCextratwovalueamt + STCElecticalamt) / 100, 0);

                        }


                        cesstax = Math.Round(Cess * servicetax / 100, 0);
                        educess = Math.Round(SHEcess * servicetax / 100, 0);


                        if (!GSTLineItem)
                        {
                            if (contractdetails.Rows[0]["CGST"].ToString() == "False")
                            {
                                CGSTTax = 0;
                            }
                            else
                            {
                                if (roundoff == true)
                                {
                                    CGSTTax = Math.Round(CGST * (totalamt + Servicechargeamt + STCMachineryamt + STCMaterialamt + STCextraonevalueamt + STCextratwovalueamt + STCElecticalamt) / 100, 2);
                                }
                                else
                                {
                                    CGSTTax = Math.Round(CGST * (totalamt + Servicechargeamt + STCMachineryamt + STCMaterialamt + STCextraonevalueamt + STCextratwovalueamt + STCElecticalamt) / 100, 0);
                                }
                            }

                            if (contractdetails.Rows[0]["SGST"].ToString() == "False")
                            {
                                SGSTTax = 0;
                            }
                            else
                            {
                                if (roundoff == true)
                                {
                                    SGSTTax = Math.Round(SGST * (totalamt + Servicechargeamt + STCMachineryamt + STCMaterialamt + STCextraonevalueamt + STCextratwovalueamt + STCElecticalamt) / 100, 2);
                                }
                                else
                                {
                                    CGSTTax = Math.Round(CGST * (totalamt + Servicechargeamt + STCMachineryamt + STCMaterialamt + STCextraonevalueamt + STCextratwovalueamt + STCElecticalamt) / 100, 0);
                                }
                            }


                            if (contractdetails.Rows[0]["IGST"].ToString() == "False")
                            {
                                IGSTTax = 0;
                            }
                            else
                            {
                                if (roundoff == true)
                                {
                                    IGSTTax = Math.Round(IGST * (totalamt + Servicechargeamt + STCMachineryamt + STCMaterialamt + STCextraonevalueamt + STCextratwovalueamt + STCElecticalamt) / 100, 2);
                                }
                                else
                                {
                                    IGSTTax = Math.Round(IGST * (totalamt + Servicechargeamt + STCMachineryamt + STCMaterialamt + STCextraonevalueamt + STCextratwovalueamt + STCElecticalamt) / 100, 0);
                                }
                            }


                            if (contractdetails.Rows[0]["Cess1"].ToString() == "False")
                            {
                                Cess1Tax = 0;
                            }
                            else
                            {
                                Cess1Tax = (Cess1 * (totalamt + Servicechargeamt + STCMachineryamt + STCMaterialamt + STCextraonevalueamt + STCextratwovalueamt + STCElecticalamt) / 100);
                            }

                            if (contractdetails.Rows[0]["Cess2"].ToString() == "False")
                            {
                                Cess2Tax = 0;
                            }
                            else
                            {
                                Cess2Tax = (Cess2 * (totalamt + Servicechargeamt + STCMachineryamt + STCMaterialamt + STCextraonevalueamt + STCextratwovalueamt + STCElecticalamt) / 100);
                            }

                        }
                        else
                        {
                            CGSTTax = LineCGST - LineLessCGST;
                            SGSTTax = LineSGST - LineLessSGST;
                            IGSTTax = LineIGST - LineLessIGST;
                        }


                    }


                    if (roundoff == true)
                    {
                        gtotal = Math.Round(subtotal + servicetax + cesstax + educess + sbcesstax + kkcesstax + Cess1Tax + Cess2Tax + CGSTTax + SGSTTax + IGSTTax + Machineryamt + Materialamt + extraonevalueamt + extratwovalueamt + Electicalamt - (Discountsamt + discounttwovalueamt), 2);
                    }
                    else
                    {
                        gtotal = Math.Round(subtotal + servicetax + cesstax + educess + sbcesstax + kkcesstax + Cess1Tax + Cess2Tax + CGSTTax + SGSTTax + IGSTTax + Machineryamt + Materialamt + extraonevalueamt + extratwovalueamt + Electicalamt - (Discountsamt + discounttwovalueamt), 0);
                    }
                }





                if (totalamt > 0)
                {

                    lblTotalResources.Text = totalamt.ToString();
                    lblTotalResources.Visible = true;
                    lbltotal.Visible = true;
                }


                if (servicetax > 0)
                {
                    lblServiceTax.Text = servicetax.ToString();
                    TxtServiceTaxPrc.Text = ServiceTaxSeparate.ToString();
                    lblServiceTaxTitle.Visible = true;
                    lblServiceTax.Visible = true;
                    TxtServiceTaxPrc.Visible = true;
                }
                else
                {
                    lblServiceTax.Text = servicetax.ToString();
                    lblServiceTaxTitle.Visible = false;
                    lblServiceTax.Visible = false;
                    TxtServiceTaxPrc.Visible = false;
                }


                if (sbcesstax > 0)
                {

                    lblSBCESS.Text = sbcesstax.ToString();
                    TxtSBCESSPrc.Text = SBcess.ToString();
                    lblSBCESSTitle.Visible = true;
                    lblSBCESS.Visible = true;
                    TxtSBCESSPrc.Visible = true;

                }
                else
                {

                    lblSBCESS.Text = sbcesstax.ToString();
                    lblSBCESSTitle.Visible = false;
                    lblSBCESS.Visible = false;
                    TxtSBCESSPrc.Visible = false;
                }


                if (kkcesstax > 0)
                {

                    lblKKCESS.Text = kkcesstax.ToString();
                    TxtKKCESSPrc.Text = KKcess.ToString();
                    lblKKCESSTitle.Visible = true;
                    lblKKCESS.Visible = true;
                    TxtKKCESSPrc.Visible = true;
                }
                else
                {

                    lblKKCESS.Text = kkcesstax.ToString();
                    lblKKCESSTitle.Visible = false;
                    lblKKCESS.Visible = false;
                    TxtKKCESSPrc.Visible = false;

                }

                #region for GST on 17-6-2017 by swathi

                if (CGSTTax > 0)
                {
                    lblCGST.Text = CGSTTax.ToString();
                    TxtCGSTPrc.Text = CGST.ToString();
                    lblCGSTTitle.Visible = true;
                    lblCGST.Visible = true;
                    TxtCGSTPrc.Visible = true;
                }
                else
                {
                    lblCGST.Text = CGSTTax.ToString();
                    lblCGSTTitle.Visible = false;
                    lblCGST.Visible = false;
                    TxtCGSTPrc.Visible = false;

                }



                if (SGSTTax > 0)
                {
                    lblSGST.Text = SGSTTax.ToString();
                    TxtSGSTPrc.Text = SGST.ToString();
                    lblSGSTTitle.Visible = true;
                    lblSGST.Visible = true;
                    TxtSGSTPrc.Visible = true;
                }
                else
                {
                    lblSGST.Text = SGSTTax.ToString();
                    lblSGSTTitle.Visible = false;
                    lblSGST.Visible = false;
                    TxtSGSTPrc.Visible = false;

                }



                if (IGSTTax > 0)
                {
                    lblIGST.Text = IGSTTax.ToString();
                    TxtIGSTPrc.Text = IGST.ToString();
                    lblIGSTTitle.Visible = true;
                    lblIGST.Visible = true;
                    TxtIGSTPrc.Visible = true;
                }
                else
                {
                    lblIGST.Text = IGSTTax.ToString();
                    lblIGSTTitle.Visible = false;
                    lblIGST.Visible = false;
                    TxtIGSTPrc.Visible = false;

                }



                if (Cess1Tax > 0)
                {
                    lblCess1.Text = Cess1Tax.ToString();
                    TxtCess1Prc.Text = Cess1.ToString();
                    lblCess1Title.Visible = true;
                    lblCess1.Visible = true;
                    TxtCess1Prc.Visible = true;
                }
                else
                {
                    lblCess1.Text = Cess1Tax.ToString();
                    lblCess1Title.Visible = false;
                    lblCess1.Visible = false;
                    TxtCess1Prc.Visible = false;

                }



                if (Cess2Tax > 0)
                {
                    lblCess2.Text = Cess2Tax.ToString();
                    TxtCess2Prc.Text = Cess2.ToString();
                    lblCess2Title.Visible = true;
                    lblCess2.Visible = true;
                    TxtCess2Prc.Visible = true;
                }
                else
                {
                    lblCess2.Text = Cess2Tax.ToString();
                    lblCess2Title.Visible = false;
                    lblCess2.Visible = false;
                    TxtCess2Prc.Visible = false;

                }

                #endregion for GST on 17-6-2017

                if (cesstax > 0)
                {

                    lblCESS.Text = cesstax.ToString();
                    TxtCESSPrc.Text = Cess.ToString();
                    lblCESSTitle.Visible = true;
                    lblCESS.Visible = true;
                    TxtCESSPrc.Visible = true;
                }
                else
                {
                    lblCESS.Text = cesstax.ToString();
                    lblCESSTitle.Visible = false;
                    lblCESS.Visible = false;
                    TxtCESSPrc.Visible = false;
                }

                if (educess > 0)
                {

                    lblSheCESS.Text = educess.ToString();
                    TxtSheCESSPrc.Text = SHEcess.ToString();
                    lblSheCESSTitle.Visible = true;
                    lblSheCESS.Visible = true;
                    TxtSheCESSPrc.Visible = true;
                }
                else
                {
                    lblSheCESS.Text = educess.ToString();
                    lblSheCESSTitle.Visible = false;
                    lblSheCESS.Visible = false;
                    TxtSheCESSPrc.Visible = false;
                }



                if (gtotal > 0)
                {
                    lblGrandTotal.Text = gtotal.ToString();
                    lblGrandTotal.Visible = true;
                    lblgrandtotalss.Visible = true;
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlmonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {



                cleartext();
                //ClearExtraDataForBilling();

                if (ddlclientid.SelectedIndex <= 0)
                {
                    //LblResult.Text = "Please select ClientId";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' Please select ClientId ');", true);
                    return;
                }
                if (ddlmonth.SelectedIndex > 0)
                {
                    FillMonthDetails();

                    if (ddlType.SelectedIndex == 0)
                    {
                        DisplayDataInGrid();

                    }
                    else
                    {

                        LoadOldBillnos();
                        DisplayDataInGridManual();


                    }

                }

                EnabledFields();
                VisibleFreezeCredit();
                visiblebutton();
                displayExtraData();
            }
            catch (Exception ex)
            {

            }

        }

        public void loadDesignations()
        {
            DataTable DtDesignation = null;
            List<string> list = new List<string>();

            DtDesignation = GlobalData.Instance.LoadDesigns();
            list = DtDesignation.AsEnumerable()
                              .Select(r => r.Field<string>("Design"))
                              .ToList();
            var result = new JavaScriptSerializer().Serialize(list);
            hdDesignations.Value = result;

        }

        protected void txtmonthOnTextChanged(object sender, EventArgs e)
        {
            cleartext();

            if (ddlclientid.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' Please select ClientId ');", true);
                return;
            }
            if (txtmonth.Text.Length > 0)
            {
                FillMonthDetails();

                if (ddlType.SelectedIndex == 0)
                {
                    DisplayDataInGrid();

                }
                else
                {
                    LoadOldBillnos();
                    DisplayDataInGridManual();
                    VisibleFreeze();

                }

            }

            EnabledFields();
            VisibleFreezeCredit();
            visiblebutton();
            displayExtraData();

        }

        protected void LoadOldBillnos()
        {

            if (ddlclientid.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert()", "alert('please select Client ID/Name')", true);
                return;
            }

            int month = 0;

            month = GetMonthBasedOnSelectionDateorMonth();



            string SqlQryFBill = "SElect BillNo  From unitbill Where  unitid ='" + ddlclientid.SelectedValue + "'  and month ='" + month + "'";
            DataTable DtFBill = config.ExecuteReaderWithQueryAsync(SqlQryFBill).Result;
            if (DtFBill.Rows.Count > 0)
            {
                lblbillnolatest.Text = DtFBill.Rows[0]["BillNo"].ToString();
            }
            else
            {
                lblbillnolatest.Text = "";
            }


            string SqlQry = "SElect BillNo  From Munitbill Where  unitid ='" + ddlclientid.SelectedValue + "'  and month ='" + month + "'";
            DataTable Dt = config.ExecuteReaderWithQueryAsync(SqlQry).Result;
            ddlMBBillnos.Items.Clear();
            if (Dt.Rows.Count > 0)
            {
                ddlMBBillnos.DataTextField = "BillNo";
                ddlMBBillnos.DataValueField = "BillNo";
                ddlMBBillnos.DataSource = Dt;
                ddlMBBillnos.DataBind();
            }
            ddlMBBillnos.Items.Insert(0, "--Select--");


        }

        public void cleartext()
        {
            if (ddlType.SelectedIndex == 0)
            {
                // btnAddNewRow.Visible = false;
                btnCalculateTotals.Visible = false;
                lblbilltype.Visible = false;
                lblManualBillNo.Visible = false;
                ddlMBBillnos.Visible = false;
                rdbcreatebill.Visible = false;
                rdbmodifybill.Visible = false;
                lbltotal.Visible = true;
                lblTotalResources.Visible = true;
                lblgrandtotalss.Visible = true;
                lblGrandTotal.Visible = true;
                checkExtraData.Visible = true;
                btncleardata.Visible = false;
            }
            else
            {

                FillDefaultGird();
                btncleardata.Visible = true;

                //btnAddNewRow.Visible = true;
                btnCalculateTotals.Visible = true;
                lblbilltype.Visible = true;
                lblManualBillNo.Visible = true;
                ddlMBBillnos.Visible = true;
                rdbcreatebill.Visible = true;
                rdbmodifybill.Visible = true;
                lblServiceCharges.Enabled = true;
                // Chk_Month.Visible = false;
                lbltotal.Visible = true;
                lblTotalResources.Visible = true;
                lblgrandtotalss.Visible = true;
                lblGrandTotal.Visible = true;
                checkExtraData.Visible = true;
                ddlMBBillnos.Items.Clear();
                ddlMBBillnos.Items.Insert(0, "--Select--");



            }

            lblResult.Text = "";
            lblbillnolatest.Text = "";
            txtbilldate.Text = "";
            txtduedate.Text = "";
            txtfromdate.Text = "";
            txttodate.Text = "";
            //txtMBillNo.Text = "";
            TxtservicechrgPrc.Text = "";
            checkExtraData.Visible = true;
            lblTotalResources.Visible = true;
            lbltotal.Visible = true;
            lblgrandtotalss.Visible = true;
            lblGrandTotal.Visible = true;
            rdbcreatebill.Checked = true;
            rdbmodifybill.Checked = false;


            for (int i = 0; i < gvClientBilling.Rows.Count; i++)
            {
                TextBox lbldesgn = gvClientBilling.Rows[i].FindControl("lbldesgn") as TextBox;
                TextBox lblnoofemployees = gvClientBilling.Rows[i].FindControl("lblnoofemployees") as TextBox;
                DropDownList ddlHSNNumber = gvClientBilling.Rows[i].FindControl("ddlHSNNumber") as DropDownList;
                TextBox lblNoOfDuties = gvClientBilling.Rows[i].FindControl("lblNoOfDuties") as TextBox;
                TextBox lblpayrate = gvClientBilling.Rows[i].FindControl("lblpayrate") as TextBox;
                TextBox txtNewPayRate = gvClientBilling.Rows[i].FindControl("txtNewPayRate") as TextBox;
                TextBox lblda = gvClientBilling.Rows[i].FindControl("lblda") as TextBox;
                TextBox lblAmount = gvClientBilling.Rows[i].FindControl("lblAmount") as TextBox;
                TextBox lblCGSTAmount = gvClientBilling.Rows[i].FindControl("lblCGSTAmount") as TextBox;
                TextBox lblSGSTAmount = gvClientBilling.Rows[i].FindControl("lblSGSTAmount") as TextBox;
                TextBox lblIGSTAmount = gvClientBilling.Rows[i].FindControl("lblIGSTAmount") as TextBox;
                TextBox lblCGSTPrc = gvClientBilling.Rows[i].FindControl("lblCGSTPrc") as TextBox;
                TextBox lblSGSTPrc = gvClientBilling.Rows[i].FindControl("lblSGSTPrc") as TextBox;
                TextBox lblIGSTPrc = gvClientBilling.Rows[i].FindControl("lblIGSTPrc") as TextBox;
                TextBox lblTotalTaxmount = gvClientBilling.Rows[i].FindControl("lblTotalTaxmount") as TextBox;


                if (ddlType.SelectedIndex == 0)
                {
                    lbldesgn.Enabled = false;
                    lblnoofemployees.Enabled = false;
                    ddlHSNNumber.Enabled = false;
                    lblNoOfDuties.Enabled = false;
                    lblpayrate.Enabled = false;
                    txtNewPayRate.Enabled = false;
                    lblda.Enabled = false;
                    lblAmount.Enabled = false;
                    lblCGSTAmount.Enabled = false;
                    lblSGSTAmount.Enabled = false;
                    lblIGSTAmount.Enabled = false;
                    lblCGSTPrc.Enabled = false;
                    lblSGSTPrc.Enabled = false;
                    lblIGSTPrc.Enabled = false;
                    lblTotalTaxmount.Enabled = false;
                }
                else
                {
                    lbldesgn.Enabled = true;
                    lblnoofemployees.Enabled = true;
                    ddlHSNNumber.Enabled = true;
                    lblNoOfDuties.Enabled = true;
                    lblpayrate.Enabled = true;
                    txtNewPayRate.Enabled = true;
                    lblda.Enabled = true;
                    lblAmount.Enabled = true;
                    lblCGSTAmount.Enabled = true;
                    lblSGSTAmount.Enabled = true;
                    lblIGSTAmount.Enabled = true;
                    lblCGSTPrc.Enabled = true;
                    lblSGSTPrc.Enabled = true;
                    lblIGSTPrc.Enabled = true;
                    lblTotalTaxmount.Enabled = true;
                }

            }



        }

        public void ManualBillGenerateMethod()
        {
            try
            {

                lbltotalamount.Visible = true;

                #region Validations

                if (ddlclientid.SelectedIndex <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' Select Client Id ');", true);

                    return;
                }
                #region  Begin New code As on [10-03-2014]

                if (Chk_Month.Checked == true)
                {
                    if (txtmonth.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Enter Month for Bill ');", true);
                        return;
                    }
                    if (Timings.Instance.CheckEnteredDate(txtmonth.Text) == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid TO DATE .Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }
                }
                else
                {
                    if (ddlmonth.SelectedIndex == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Select Month for Bill ');", true);

                        return;
                    }
                }
                #endregion  End Old Code As on [14-02-2014]
                if (txtbilldate.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' Please Fill The Billdate  ');", true);
                    return;
                }

                if (ddlMBBillnos.SelectedIndex > 0)
                {
                    if (rdbmodifybill.Checked == false)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select The Bill No.');", true);
                        return;
                    }
                }
                #endregion

                int month = 0;
                var monthReports = 0;
                string SelectedClient = ddlclientid.SelectedValue;

                string Billtype = "";

                if (ddlType.SelectedIndex == 1)
                {
                    Billtype = "M";
                }
                else if (ddlType.SelectedIndex == 2)
                {
                    Billtype = "A";
                }
                else if (ddlType.SelectedIndex == 3)
                {
                    Billtype = "B";
                }
                else if (ddlType.SelectedIndex == 4)
                {
                    Billtype = "E";
                }

                #region Month Selection

                month = GetMonthBasedOnSelectionDateorMonth();

                #endregion

                DateTime dt = DateTime.ParseExact(txtbilldate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string billdt = dt.ToString("MM/dd/yyyy");

                var query1 = @"select ServiceTaxSeparate,Cess,SHECess,SBCess,KKCess,CGST,SGST,IGST,cess1,cess2 from TblOptions where '" + billdt + "' between fromdate and todate ";//Gst
                var optiondetails = config.ExecuteAdaptorAsyncWithQueryParams(query1).Result;

                decimal ServiceTaxSeparate = 0;
                decimal Cessprc = 0;
                decimal SHEcessprc = 0;
                decimal SBcessprc = 0;
                decimal KKcessprc = 0;
                decimal Cess1prc = 0;//Gst
                decimal Cess2prc = 0;
                decimal CGSTprc = 0;
                decimal IGSTprc = 0;
                decimal SGSTprc = 0;


                if (optiondetails.Rows.Count > 0)
                {

                    ServiceTaxSeparate = Convert.ToDecimal(optiondetails.Rows[0]["ServiceTaxSeparate"].ToString());
                    Cessprc = Convert.ToDecimal(optiondetails.Rows[0]["Cess"].ToString());
                    SHEcessprc = Convert.ToDecimal(optiondetails.Rows[0]["SHECess"].ToString());
                    SBcessprc = Convert.ToDecimal(optiondetails.Rows[0]["SBCess"].ToString());
                    KKcessprc = Convert.ToDecimal(optiondetails.Rows[0]["KKCess"].ToString());
                    CGSTprc = Convert.ToDecimal(optiondetails.Rows[0]["CGST"].ToString());
                    IGSTprc = Convert.ToDecimal(optiondetails.Rows[0]["IGST"].ToString());
                    SGSTprc = Convert.ToDecimal(optiondetails.Rows[0]["SGST"].ToString());
                    Cess1prc = Convert.ToDecimal(optiondetails.Rows[0]["Cess1"].ToString());
                    Cess2prc = Convert.ToDecimal(optiondetails.Rows[0]["Cess2"].ToString());

                }

                DateTime DtLastDay = DateTime.Now;
                if (Chk_Month.Checked == false)
                {
                    DtLastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlmonth.SelectedIndex);
                }
                if (Chk_Month.Checked == true)
                {
                    DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                }
                var ContractID = "";

                #region  Begin Get Contract Id Based on The Last Day


                Hashtable HtGetContractID = new Hashtable();
                var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
                HtGetContractID.Add("@LastDay", DtLastDay);
                DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                if (DTContractID.Rows.Count > 0)
                {
                    ContractID = DTContractID.Rows[0]["contractid"].ToString();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract details are not avaialable for this client.');", true);
                    return;
                }


                #endregion 

                #region  Query For Delete Unitbill Break Up Data

                /** Delete previously generated UnitBillBreakup data */

                if (rdbmodifybill.Checked)
                {
                    string DeleteQueryForSelectedMonth = "Delete from Munitbillbreakup where unitid ='" + SelectedClient + "' and month =" +
                                                                         month + " and MunitidBillno='" + ddlMBBillnos.SelectedValue + "'";
                    int deleteop = config.ExecuteNonQueryWithQueryAsync(DeleteQueryForSelectedMonth).Result;
                }
                //Unitbill details are not deleted now due to Billno(avoid regeneration)
                /** Delete **/

                #endregion

                #region   Query for  Get  Contracts  Details

                string sqlQry = "Select ContractId,ContractStartDate,ContractEndDate,PaymentType,MaterialCostPerMonth, " +
                    " MachinaryCostPerMonth,NoOfDays,servicecharge,OTPersent,PayLumpsum,ServiceChargeType,ServiceTax75,IncludeST, " +
                    "  ServiceTaxType,BillDates,IGST,isnull(GrandTotalRoff,0) as GrandTotalRoff from Contracts where ClientId='" + ddlclientid.SelectedValue + "' and contractid='" + ContractID + "'";
                DataTable dtContracts = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;

                if (dtContracts.Rows.Count > 0)
                {
                    //CalculateTotals();

                    string strSTType = dtContracts.Rows[0]["ServiceTaxType"].ToString();
                    string NoOfDaysFromContract = dtContracts.Rows[0]["NoOfDays"].ToString();
                    string strServiceChargetType = dtContracts.Rows[0]["ServiceChargeType"].ToString();
                    string ServiceCharge = dtContracts.Rows[0]["ServiceCharge"].ToString();
                    string IGST = dtContracts.Rows[0]["IGST"].ToString();
                    bool GrandTotalRoff = bool.Parse(dtContracts.Rows[0]["GrandTotalRoff"].ToString());

                    bool bSTType = (strSTType == "True");
                    string billno = (rdbmodifybill.Checked)
                                    ? ddlMBBillnos.SelectedValue
                                    : BillnoAutoGenrate(bSTType, ddlclientid.SelectedValue, month);



                    #region   Get Data From GridView and Saving In the Munitbillbreakup Table

                    if (gvClientBilling.Rows.Count > 0)
                    {
                        string invoicedesc = "";//txtdescription.Text;
                        string remarks = "";// txtremarks.Text;
                        string Unitid = ddlclientid.SelectedValue;
                        int totalstatus = 0;
                        int i = 0;



                        foreach (GridViewRow GvRow in gvClientBilling.Rows)
                        {
                            string sno = ((Label)GvRow.FindControl("lblSno")).Text;
                            string Desgn = ((TextBox)GvRow.FindControl("lbldesgn")).Text;
                            string HSNNumber = ((DropDownList)GvRow.FindControl("ddlHSNNumber")).SelectedValue;
                            string NoOfEmps = ((TextBox)GvRow.FindControl("lblnoofemployees")).Text;
                            string NoOfDuties = ((TextBox)GvRow.FindControl("lblNoOfDuties")).Text;
                            string Payrate = ((TextBox)GvRow.FindControl("lblpayrate")).Text; //lblda
                            string NewPayRate = ((TextBox)GvRow.FindControl("txtNewPayRate")).Text; //lblda
                            string DutiesAmount = ((TextBox)GvRow.FindControl("lblda")).Text;
                            string Total = ((TextBox)GvRow.FindControl("lblda")).Text;
                            string LSChargesPrc = ((TextBox)GvRow.FindControl("lblSchrgPrc")).Text;
                            string LSCharges = ((TextBox)GvRow.FindControl("lblSchrgAmt")).Text;
                            string CGSTAmt = ((TextBox)GvRow.FindControl("lblCGSTAmount")).Text;
                            string lblCGSTPrc = ((TextBox)GvRow.FindControl("lblCGSTPrc")).Text;
                            string SGSTAmt = ((TextBox)GvRow.FindControl("lblSGSTAmount")).Text;
                            string lblSGSTPrc = ((TextBox)GvRow.FindControl("lblSGSTPrc")).Text;
                            string IGSTAmt = ((TextBox)GvRow.FindControl("lblIGSTAmount")).Text;
                            string lblIGSTPrc = ((TextBox)GvRow.FindControl("lblIGSTPrc")).Text;
                            string TotalTaxmount = ((TextBox)GvRow.FindControl("lblTotalTaxmount")).Text;
                            string GSTper = ((TextBox)GvRow.FindControl("lblGSTper")).Text;
                            string UOM = ((TextBox)GvRow.FindControl("txtUOM")).Text;
                            string ubsrchrgs = ((Label)GvRow.FindControl("lblsrchrgs")).Text;

                            if (ubsrchrgs == "")
                            {
                                ubsrchrgs = "0";
                            }


                            float ToatlAmount = 0;
                            float basicda = 0;
                            ToatlAmount = (Total.Trim().Length != 0) ? float.Parse(Total) : 0;
                            basicda = (DutiesAmount.Trim().Length != 0) ? float.Parse(DutiesAmount) : 0;
                            DropDownList ddlnodays = gvClientBilling.Rows[i].FindControl("ddlnod") as DropDownList;
                            float ddlnod = float.Parse(ddlnodays.SelectedItem.Text);
                            DropDownList ddldttype = gvClientBilling.Rows[i].FindControl("ddldutytype") as DropDownList;
                            int ddldutytype = int.Parse(ddldttype.SelectedValue);
                            DropDownList ddlCalnType = gvClientBilling.Rows[i].FindControl("ddlCalnType") as DropDownList;
                            string CalnType = ddlCalnType.SelectedValue;


                            i = i + 1;

                            if (Desgn.Length > 0)
                            {
                                string Sqlqry = string.Format("insert into Munitbillbreakup(unitid,designation,DutyHours,NoofEmps,BasicDa,ServiceChargesPrc,ServiceCharges, " +
                                    "PayRate,PayRateType,Month,OTamount,Totalamount,MunitidBillno,monthlydays,Description,Remarks,BillType,SiNo,HSNNumber,UOM,GSTPer,CGSTAmt,SGSTAmt,IGSTAmt,CGSTPrc,SGSTPrc,IGSTPrc,TotalTaxAmount,Sno,CalnType) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}')",
                                    Unitid, Desgn, NoOfDuties, NoOfEmps, DutiesAmount, LSChargesPrc, LSCharges, Payrate, ddldutytype, month, 0, Total, billno, ddlnod, invoicedesc, remarks, Billtype, sno, HSNNumber, UOM, GSTper, CGSTAmt, SGSTAmt, IGSTAmt, lblCGSTPrc, lblSGSTPrc, lblIGSTPrc, TotalTaxmount, sno, CalnType);

                                int Status = config.ExecuteNonQueryWithQueryAsync(Sqlqry).Result;

                                if (Status != 0)
                                {
                                    totalstatus++;
                                    if (totalstatus == 1)
                                    {
                                        if (ddlType.SelectedIndex == 1)
                                        {
                                            ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Manual Billing Details Added Sucessfully');", true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region   Storing Details about the  Unit Bill Table


                    bool bServiceChargeType = false;
                    if (strServiceChargetType == "True")
                    {
                        bServiceChargeType = true;
                    }


                    if (lblTotalResources.Text.Trim().Length == 0)
                    {
                        lblTotalResources.Text = "0";
                    }

                    double totalCharges = double.Parse(lblTotalResources.Text);

                    string grandtotal = lblGrandTotal.Text;
                    if (grandtotal.Trim().Length == 0)
                    {
                        grandtotal = "0";
                    }

                    string ServiceTax = lblServiceTax.Text;

                    if (ServiceTax.Trim().Length == 0)
                    {
                        ServiceTax = "0";

                    }

                    string sbcesstax = lblSBCESS.Text;

                    if (sbcesstax.Trim().Length == 0)
                    {
                        sbcesstax = "0";
                    }

                    string kkcesstax = lblKKCESS.Text;


                    if (kkcesstax.Trim().Length == 0)
                    {
                        kkcesstax = "0";
                    }


                    string cesstax = lblCESS.Text;

                    if (cesstax.Trim().Length == 0)
                    {
                        cesstax = "0";
                    }

                    #region for GST as on 17-6-2017 by swathi

                    string CGSTTax = lblCGST.Text;

                    if (CGSTTax.Trim().Length == 0 || CGSTTax == "0" || CGSTTax == "0.00")
                    {
                        CGSTTax = "0";
                        CGSTprc = 0;
                    }


                    string SGSTTax = lblSGST.Text;

                    if (SGSTTax.Trim().Length == 0 || SGSTTax == "0" || SGSTTax == "0.00")
                    {
                        SGSTTax = "0";
                        SGSTprc = 0;
                    }


                    string IGSTTax = lblIGST.Text;

                    if (IGSTTax.Trim().Length == 0 || IGSTTax == "0" || IGSTTax == "0.00")
                    {
                        IGSTTax = "0";
                        IGSTprc = 0;
                    }


                    string Cess1Tax = lblCess1.Text;

                    if (Cess1Tax.Trim().Length == 0)
                    {
                        Cess1Tax = "0";
                    }

                    string Cess2Tax = lblCess2.Text;

                    if (Cess2Tax.Trim().Length == 0)
                    {
                        Cess2Tax = "0";
                    }

                    #endregion for GST as on 17-6-2017 by swathi

                    string Shesstax = lblSheCESS.Text;

                    if (Shesstax.Trim().Length == 0)
                    {
                        Shesstax = "0";
                    }

                    string ServiceChargePer = TxtservicechrgPrc.Text;
                    if (ServiceChargePer.Trim().Length == 0)
                    {
                        ServiceChargePer = "0";
                    }

                    string SubTotal = lblTotalResources.Text;
                    if (SubTotal.Trim().Length == 0)
                    {
                        SubTotal = "0";
                    }

                    string Servicechrg = lblServiceCharges.Text;

                    if (Servicechrg.Trim().Length == 0)
                    {
                        Servicechrg = "0";
                    }

                    var BankName = "";
                    var BankAccountNo = "";
                    var IFSCCode = "";
                    string SqlQrycompanyinfo = " select BankAccountNo,IFSCCode,BankName from CompanyInfo  where  ClientidPrefix  ='" + CmpIDPrefix + "'";
                    DataTable dtcompanyinfo = config.ExecuteReaderWithQueryAsync(SqlQrycompanyinfo).Result;
                    if (dtcompanyinfo.Rows.Count > 0)
                    {
                        BankName = dtcompanyinfo.Rows[0]["BankName"].ToString();
                        BankAccountNo = dtcompanyinfo.Rows[0]["BankAccountNo"].ToString();
                        IFSCCode = dtcompanyinfo.Rows[0]["IFSCCode"].ToString();
                    }


                    #region Client detils saving in manualbill

                    var OURGSTNo = "";
                    var BillToGSTNo = "";
                    var BillToState = "";
                    var BillToStateCode = "";
                    var GSTAddress = "";
                    var Phoneno = "";
                    var Faxno = "";

                    string Query = "select isnull(GST.GSTAddress,'') GSTAddress,isnull(GST.GSTNo,'') GSTNo, isnull(GSTIN,'') GSTIN,isnull(s.state,'') state,isnull(s.GSTStateCode,'0') GSTStateCode,GST.Phoneno,GST.Faxno from Clients C  left join GSTMaster GST on GST.Id=C.OurGSTIN left join states s on s.stateid=c.state   where ClientId='" + ddlclientid.SelectedValue + "'";
                    DataTable DTCLientData = config.ExecuteAdaptorAsyncWithQueryParams(Query).Result;
                    if (DTCLientData.Rows.Count > 0)
                    {
                        OURGSTNo = DTCLientData.Rows[0]["GSTNo"].ToString();
                        BillToGSTNo = DTCLientData.Rows[0]["GSTIN"].ToString();
                        BillToState = DTCLientData.Rows[0]["state"].ToString();
                        BillToStateCode = DTCLientData.Rows[0]["GSTStateCode"].ToString();
                        GSTAddress = DTCLientData.Rows[0]["GSTAddress"].ToString();
                        Phoneno = DTCLientData.Rows[0]["Phoneno"].ToString();
                        Faxno = DTCLientData.Rows[0]["Faxno"].ToString();

                    }
                    #endregion


                    System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-GB");
                    DateTime dtb = Convert.ToDateTime(txtbilldate.Text, enGB);
                    string billdate = dtb.ToString("yyyy-MM-dd hh:mm:ss");
                    DateTime dtf = Convert.ToDateTime(txtfromdate.Text, enGB);
                    string tfrom = dtf.ToString("yyyy-MM-dd hh:mm:ss");
                    DateTime dtt = Convert.ToDateTime(txttodate.Text, enGB);
                    string tto = dtt.ToString("yyyy-MM-dd hh:mm:ss");

                    //titels
                    string MachinaryCosttitle = txtmachinarycost.Text;
                    string MaterialCosttitle = txtMaterialcost.Text;
                    string ExtraAmtTwotitle = txtextratwotitle.Text;
                    string ExtraAmtonetitle = txtextraonetitle.Text;
                    string DiscountTwotitle = txtdiscounttwotitle.Text;
                    string Discounttitle = txtdiscount.Text;
                    string ElectricalChrgtitle = txtMaintanancecost.Text;
                    //values
                    string MachinaryCost = txtMachinery.Text;
                    string MaterialCost = txtMaterial.Text;
                    string ExtraAmtTwo = txtextratwovalue.Text;
                    string ExtraAmtone = txtextraonevalue.Text;
                    string DiscountTwo = txtdiscounttwovalue.Text;
                    string Discount = txtDiscounts.Text;
                    string ElectricalChrg = txtElectical.Text;
                    //checkbox
                    var chkSTYesMachinarys = 0;
                    if (chkSTYesMachinary.Checked == true)
                    {
                        chkSTYesMachinarys = 1;
                    }
                    var chkSCYesMachinarys = 0;
                    if (chkSCYesMachinary.Checked == true)
                    {
                        chkSCYesMachinarys = 1;
                    }
                    var chkSTYesMaterials = 0;
                    if (chkSTYesMaterial.Checked == true)
                    {
                        chkSTYesMaterials = 1;
                    }
                    var chkSCYesMaterials = 0;
                    if (chkSCYesMaterial.Checked == true)
                    {
                        chkSCYesMaterials = 1;
                    }

                    var chkSTYesElectricals = 0;
                    if (chkSTYesElectrical.Checked == true)
                    {
                        chkSTYesElectricals = 1;
                    }

                    var chkSCYesElectricals = 0;
                    if (chkSCYesElectrical.Checked == true)
                    {
                        chkSCYesElectricals = 1;
                    }



                    var chkSTYesExtraones = 0;
                    if (chkSTYesExtraone.Checked == true)
                    {
                        chkSTYesExtraones = 1;
                    }



                    var chkSCYesExtraones = 0;
                    if (chkSCYesExtraone.Checked == true)
                    {
                        chkSCYesExtraones = 1;
                    }

                    var chkSTYesExtratwos = 0;
                    if (chkSTYesExtratwo.Checked == true)
                    {
                        chkSTYesExtratwos = 1;
                    }


                    var chkSCYesExtratwos = 0;
                    if (chkSCYesExtratwo.Checked == true)
                    {
                        chkSCYesExtratwos = 1;
                    }

                    var chkSTDiscountones = 0;
                    if (chkSTDiscountone.Checked == true)
                    {
                        chkSTDiscountones = 1;
                    }

                    var chkSTDiscounttwos = 0;
                    if (chkSTDiscounttwo.Checked == true)
                    {
                        chkSTDiscounttwos = 1;
                    }

                    var checkExtraDatas = 0;
                    if (checkExtraData.Checked == true)
                    {
                        checkExtraDatas = 1;
                    }




                    string TotalServiceTax = (float.Parse(ServiceTax) + float.Parse(sbcesstax) + float.Parse(kkcesstax) + float.Parse(CGSTTax) + float.Parse(SGSTTax) + float.Parse(IGSTTax) + float.Parse(Cess1Tax) + float.Parse(Cess2Tax) + float.Parse(cesstax) + float.Parse(Shesstax)).ToString();



                    DateTime Created_On = DateTime.Parse(DateTime.Now.ToString());
                    string Created_By = Username;

                    if (rdbcreatebill.Checked)
                    {
                        string InsertQueryForUnitBill = "insert into Munitbill(billno,billdt,unitid,fromdt,todt,TotalChrg,dutiestotalamount," +
                                                       " monthlydays,grandtotal,Servicechrg,servicetax,cess,shecess,ServiceTax75percentage,ServiceChrgPer,Subtotal," +
                                                       " month,SBCessAmt,kkcessamt,ServiceTaxPrc,SBCessTaxPrc,KKCessTaxPrc,CESSPer,SHECessPer, " +
                                                       " MachinaryCost,MaterialCost,ExtraAmtTwo,ExtraAmtone,DiscountTwo,Discount,ElectricalChrg, " +
                                                       " Machinarycosttitle,Materialcosttitle,Extraonetitle,Extratwotitle,Discountonetitle,Discounttwotitle,Maintanancecosttitle, " +
                                                       " stmachinary,STMaterial,STMaintenance,STExtraone,STExtratwo,STDiscountone,STDiscounttwo, " +
                                                       " SCMachinary,SCMaterial,SCMaintenance,SCExtraone,SCExtratwo,Extradatacheck,BillType,Created_By,Created_On, CGSTAmt ,SGSTAmt , IGSTAmt , Cess1Amt,Cess2Amt , CGSTPrc , SGSTPrc ,IGSTPrc , Cess1Prc , Cess2Prc,TotalServiceTax,BillCompletedStatus,Timings,Remarks,BankName,BankAccountNo,IFSCCode,OURGSTNo,BillToGSTNo,BillToState,BillToStateCode,GSTAddress,Phoneno,Faxno ) values('"
                                                       + billno + "','"
                                                       + billdate + "','"
                                                       + ddlclientid.SelectedValue + "','"
                                                       + tfrom + "','"
                                                       + tto + "','"
                                                       + totalCharges + "','"
                                                       + totalCharges + "','0','"
                                                       + grandtotal + "','"
                                                       + lblServiceCharges.Text + "','"
                                                       + ServiceTax + "',"
                                                       + cesstax + ","
                                                       + Shesstax + ","
                                                       + "null,'"
                                                       + ServiceChargePer + "','"
                                                       + lblTotalResources.Text + "',"
                                                       + month + ",'"
                                                       + lblSBCESS.Text + "','" + lblKKCESS.Text + "','" + TxtServiceTaxPrc.Text + "','" + TxtSBCESSPrc.Text + "','" + TxtKKCESSPrc.Text + "','" + TxtCESSPrc.Text + "','" + TxtSheCESSPrc.Text + "','"
                                                       + MachinaryCost + "','" + MaterialCost + "','" + ExtraAmtTwo + "','" + ExtraAmtone + "','" + DiscountTwo + "','" + Discount + "','" + ElectricalChrg + "','"
                                                      + MachinaryCosttitle + "','" + MaterialCosttitle + "','" + ExtraAmtTwotitle + "','" + ExtraAmtonetitle + "','" + DiscountTwotitle + "','" + Discounttitle + "','" + ElectricalChrgtitle + "','"
                                                      + chkSTYesMachinarys + "','" + chkSTYesMaterials + "','" + chkSTYesElectricals + "','" + chkSTYesExtraones + "','" + chkSTYesExtratwos + "','" + chkSTDiscountones + "','" + chkSTDiscounttwos + "','"
                                                      + chkSCYesMachinarys + "','" + chkSCYesMaterials + "','" + chkSCYesElectricals + "','" + chkSCYesExtraones + "','" + chkSCYesExtratwos + "','" + checkExtraDatas + "','" + Billtype + "','" + Created_By + "','" + Created_On + "','"
                                                      + CGSTTax + "','" + SGSTTax + "','" + IGSTTax + "','" + Cess1Tax + "','" + Cess2Tax + "','" + CGSTprc + "','" + SGSTprc + "','" + IGSTprc + "','" + Cess1Tax + "','" + Cess2prc + "','" + TotalServiceTax + "','0','" + DateTime.Now + "','" + txtRemarks.Text + "','" + BankName + "','" + BankAccountNo + "','" + IFSCCode + "','" + OURGSTNo + "','" + BillToGSTNo + "','" + BillToState + "','" + BillToStateCode + "','" + GSTAddress + "','" + Phoneno + "','" + Faxno + "')";
                        int insrt = config.ExecuteNonQueryWithQueryAsync(InsertQueryForUnitBill).Result;

                        string SP = "updateRoundoff";
                        Hashtable ht = new Hashtable();
                        ht.Add("@clientid", ddlclientid.SelectedValue);
                        ht.Add("@month", month);
                        ht.Add("@BillNo", billno);
                        ht.Add("@GrandTotalRoff", GrandTotalRoff);
                        int dtnew = config.ExecuteNonQueryParamsAsync(SP, ht).Result;

                    }


                    if (rdbmodifybill.Checked)
                    {
                        string ServiceCharge1 = lblServiceCharges.Text;
                        string ServiceChargePer1 = TxtservicechrgPrc.Text;
                        string totalCharges1 = lblTotalResources.Text;
                        string ServiceTax1 = lblServiceTax.Text;
                        string cesstax1 = lblCESS.Text;
                        string sbcesstax1 = lblSBCESS.Text;
                        string Shesstax1 = lblSheCESS.Text;
                        string SubTotal1 = lblResult.Text;
                        string grandtotal1 = lblGrandTotal.Text;
                        string kkcesstax1 = lblKKCESS.Text;

                        string desc = ""; //txtdescription.Text;
                        string remark = "";// txtremarks.Text;
                        DateTime Modify_On = DateTime.Parse(DateTime.Now.ToString());
                        string Modify_By = Username;


                        string SqlQryForUnitBill = "Select * from MUnitbill  where unitid ='" + SelectedClient + "' and month=" + month + "   and  billno='" + ddlMBBillnos.SelectedValue + "'";
                        DataTable dtUnitBill = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForUnitBill).Result;

                        string SqlQryForudateUnitBillbreakup = "Select Description,Remarks from MUnitbillBreakUp  where unitid ='" + SelectedClient + "' and month=" + month + " and MunitidBillno='" + ddlMBBillnos.SelectedValue + "'  ";
                        DataTable dtMUnitBill = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForudateUnitBillbreakup).Result;




                        if (dtUnitBill.Rows.Count > 0)
                        {
                            string InsertQueryForUnitBill = string.Format("update Munitbill set billdt='{1}',unitid='{2}',fromdt='{3}',todt='{4}', " +
                              " monthlydays='{5}',grandtotal='{6}',servicetax='{7}',cess='{8}',shecess='{9}',month='{10}',dutiestotalamount='{11}',ServiceChrg='{12}',ServiceChrgPer='{13}',Subtotal='{14}',sbcessamt='{15}',kkcessamt='{16}',ServiceTaxPrc='{17}',SBCessTaxPrc='{18}',KKCessTaxPrc='{19}',CESSPer='{20}',SHECessPer='{21}'," +
                             " MachinaryCost='{22}', MaterialCost='{23}', ExtraAmtTwo='{24}', ExtraAmtone='{25}', DiscountTwo='{26}', Discount='{27}', ElectricalChrg='{28}', " +
                                          "Machinarycosttitle='{29}',Materialcosttitle='{30}',Extraonetitle='{31}',Extratwotitle='{32}',Discountonetitle='{33}',Discounttwotitle='{34}',Maintanancecosttitle='{35}', " +
                                                      "stmachinary='{36}',STMaterial='{37}',STMaintenance='{38}',STExtraone='{39}',STExtratwo='{40}',STDiscountone='{41}',STDiscounttwo='{42}', " +
                                                  " SCMachinary='{43}',SCMaterial='{44}',SCMaintenance='{45}',SCExtraone='{46}',SCExtratwo='{47}',Extradatacheck={48},Billtype='{49}',Modify_By='{50}',Modify_On='{51}',TotalChrg ='{52}', CGSTAmt = '{53}', SGSTAmt = '{54}', IGSTAmt = '{55}', Cess1Amt = '{56}', Cess2Amt = '{57}', CGSTPrc = '{58}', SGSTPrc = '{59}', IGSTPrc = '{60}', Cess1Prc = '{61}', Cess2Prc ='{62}',TotalServiceTax='{63}',Remarks='{64}' ,BankName='{65}',BankAccountNo='{66}',IFSCCode='{67}',OURGSTNo='{68}',BillToGSTNo='{69}',BillToState='{70}',BillToStateCode='{71}',GSTAddress='{72}',Phoneno='{73}',Faxno='{74}'" +
                              " where  billno='{0}'  ",
                              billno, billdate, ddlclientid.SelectedValue, tfrom, tto,
                              0, grandtotal1, ServiceTax1, cesstax1, Shesstax1, month, totalCharges1, ServiceCharge1, ServiceChargePer1, SubTotal1, sbcesstax1, kkcesstax1, ServiceTaxSeparate, SBcessprc, KKcessprc, Cessprc, SHEcessprc,
                              MachinaryCost, MaterialCost, ExtraAmtTwo, ExtraAmtone, DiscountTwo, Discount, ElectricalChrg, MachinaryCosttitle, MaterialCosttitle, ExtraAmtonetitle, ExtraAmtTwotitle, Discounttitle, DiscountTwotitle,
                              ElectricalChrgtitle, chkSTYesMachinarys, chkSTYesMaterials, chkSTYesElectricals, chkSTYesExtraones, chkSTYesExtratwos, chkSTDiscountones, chkSTDiscounttwos,
                                chkSCYesMachinarys, chkSCYesMaterials, chkSCYesElectricals, chkSCYesExtraones, chkSCYesExtratwos, checkExtraDatas, Billtype, Modify_By, Modify_On, totalCharges, CGSTTax, SGSTTax, IGSTTax, Cess1Tax, Cess2Tax, CGSTprc, SGSTprc, IGSTprc, Cess1Tax, Cess2prc, TotalServiceTax, txtRemarks.Text, BankName, BankAccountNo, IFSCCode, OURGSTNo, BillToGSTNo, BillToState, BillToStateCode, GSTAddress, Phoneno, Faxno);

                            int insret2 = config.ExecuteNonQueryWithQueryAsync(InsertQueryForUnitBill).Result;

                            string InsertQueryForMUnitBill = string.Format("update MunitbillBreakUp set Description='" + desc + "',Remarks='" + remark + "' where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "' and MunitidBillno='" + billno + "'");
                            int insert3 = config.ExecuteNonQueryWithQueryAsync(InsertQueryForMUnitBill).Result;

                            string SP = "updateRoundoff";
                            Hashtable ht = new Hashtable();
                            ht.Add("@clientid", ddlclientid.SelectedValue);
                            ht.Add("@month", month);
                            ht.Add("@BillNo", billno);
                            ht.Add("@GrandTotalRoff", GrandTotalRoff);
                            int dtnew = config.ExecuteNonQueryParamsAsync(SP, ht).Result;

                        }


                    }
                    #endregion

                    LoadDefaultData();
                    EnabledFields();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('ContractId not available for this client.');", true);
                }
                #endregion
            }
            catch (Exception ex)
            {

            }
        }

        protected void LoadDefaultData()
        {
            txtfromdate.Text = "";
            txttodate.Text = "";
            ddlmonth.SelectedIndex = 0;
            txtbilldate.Text = "";
            rdbcreatebill.Checked = true;
            rdbmodifybill.Checked = false;
            ddlMBBillnos.SelectedIndex = 0;
            FillDefaultGird();
            lblTotalResources.Text = "0";
            lblServiceTax.Text = "0";
            lblGrandTotal.Text = "0";
            lblCESS.Text = "0";
            lblSBCESS.Text = "0";
            lblKKCESS.Text = "0";
            lblSheCESS.Text = "0";
            #region for GST on 17-6-2017 by swathi
            lblCGST.Text = "0";
            lblSGST.Text = "0";
            lblIGST.Text = "0";
            lblCess1.Text = "0";
            lblCess2.Text = "0";
            #endregion for GST on 17-6-2017 by swathi
            lblServiceCharges.Text = "0";
            lblTotalResources.Text = "0";
            TxtservicechrgPrc.Text = "";
            // txtremarks.Text = "";
            txtmonth.Text = "";
            lblbillnolatest.Text = "";

        }

        protected void lbldesgn_TextChanged(object sender, EventArgs e)
        {

            if (ddlType.SelectedIndex == 3)
            {
                TextBox lbldesgn = sender as TextBox;
                GridViewRow row = null;
                if (lbldesgn == null)
                    return;

                row = (GridViewRow)lbldesgn.NamingContainer;
                if (row == null)
                    return;

                DropDownList ddlHSNNumber = row.FindControl("ddlHSNNumber") as DropDownList;
                TextBox lblGSTper = row.FindControl("lblGSTper") as TextBox;
                TextBox txtUOM = row.FindControl("txtUOM") as TextBox;
                DropDownList ddldutytype = row.FindControl("ddldutytype") as DropDownList;

                string qry = "select hsnnumber,Gstper,unitmeasure from invstockitemlist where itemname='" + lbldesgn.Text.Trim() + "'";
                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

                if (dt.Rows.Count > 0)
                {
                    ddlHSNNumber.SelectedValue = dt.Rows[0]["hsnnumber"].ToString();
                    lblGSTper.Text = dt.Rows[0]["Gstper"].ToString();
                    txtUOM.Text = dt.Rows[0]["unitmeasure"].ToString();
                    ddldutytype.SelectedValue = "7";

                }
            }
        }

        public void VisibleFreeze()
        {
            int month = GetMonthBasedOnSelectionDateorMonth();
            string qry = string.Empty;

            if (ddlType.SelectedIndex == 0)
            {
                qry = "select isnull(freezestatus,0) Freezestatus from unitbill where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "' ";
            }
            else
            {
                qry = "select isnull(freezestatus,0) Freezestatus from Munitbill where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "'  and BillNo='" + ddlMBBillnos.SelectedValue + "'";

            }

            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            string Freezestatus = "";

            if (dt.Rows.Count > 0)
            {
                Freezestatus = dt.Rows[0]["Freezestatus"].ToString();

                if (Freezestatus == "False")
                {
                    btnFreeze.Visible = true;
                    btnUnFreeze.Visible = false;
                    btngenratepayment.Enabled = true;
                }
                else
                {
                    btnFreeze.Visible = false;
                    btnUnFreeze.Visible = true;
                    btngenratepayment.Enabled = false;
                }
            }
            else
            {
                btnFreeze.Visible = false;
                btnUnFreeze.Visible = false;
                btngenratepayment.Enabled = true;

            }
        }

        protected void btnFreeze_Click(object sender, EventArgs e)
        {
            int month = GetMonthBasedOnSelectionDateorMonth();

            string qry = "";
            int status = 0;
            if (ddlType.SelectedIndex == 0)
            {
                qry = "update unitbill set freezestatus=1 where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "' ";
            }
            else
            {
                qry = "update Munitbill set freezestatus=1 where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "' and Billno='" + ddlMBBillnos.SelectedValue + "'";
            }
            status = config.ExecuteNonQueryWithQueryAsync(qry).Result;


            if (status > 0)
            {
                btngenratepayment.Enabled = false;


                btnUnFreeze.Visible = true;
                btnFreeze.Visible = false;


            }
            else
            {
                btngenratepayment.Enabled = true;


                btnUnFreeze.Visible = false;
                btnFreeze.Visible = true;
            }
        }

        protected void btnFreezeSubmit_Click(object sender, EventArgs e)
        {


            var password = string.Empty;
            var SPName = string.Empty;
            password = txtPassword.Text.Trim();
            string sqlPassword = "select password from IouserDetails where password='" + TxtFreeze.Text + "' and id='2'";
            DataTable dtpassword = config.ExecuteAdaptorAsyncWithQueryParams(sqlPassword).Result;
            if (dtpassword.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Invalid Password');", true);
                return;
            }
            else
            {
                btnFreeze.Visible = true;
                btngenratepayment.Enabled = true;
                btnUnFreeze.Visible = false;

                int month = GetMonthBasedOnSelectionDateorMonth();

                string Qry = "";


                if (ddlType.SelectedIndex == 0)
                {
                    Qry = "select * from unitbill where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "'";
                }
                else
                {
                    Qry = "select * from Munitbill where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "' and BillNo='" + ddlMBBillnos.SelectedValue + "'";
                }
                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Qry).Result;
                if (dt.Rows.Count > 0)
                {

                    if (ddlType.SelectedIndex == 0)
                    {
                        Qry = "update unitbill set Freezestatus=0 where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "'";
                    }
                    else
                    {
                        Qry = "update Munitbill set Freezestatus=0 where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "' and BillNo='" + ddlMBBillnos.SelectedValue + "'";

                    }
                    int status = config.ExecuteNonQueryWithQueryAsync(Qry).Result;
                }

            }

            #region Validation

            if (ddlclientid.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select The client Id');", true);
                return;
            }

            #endregion



        }

        protected void btnFreezeClose_Click(object sender, EventArgs e)
        {
            ModalFreezeDetails.Hide();

        }

        protected void btnUnFreeze_Click(object sender, EventArgs e)
        {
            int month = GetMonthBasedOnSelectionDateorMonth();
            string qry = "";
            int status = 0;
            if (ddlType.SelectedIndex == 0)
            {
                qry = "update unitbill set freezestatus=0 where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "'";
            }
            else
            {
                qry = "update unitbill set freezestatus=0 where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "'  and BillNo='" + ddlMBBillnos.SelectedValue + "'";
            }

            status = config.ExecuteNonQueryWithQueryAsync(qry).Result;


            if (status > 0)
            {
                btngenratepayment.Enabled = true;
                btnFreeze.Visible = true;
                btnUnFreeze.Visible = false;

            }
            else
            {
                btngenratepayment.Enabled = false;
                btnFreeze.Visible = false;
                btnUnFreeze.Visible = true;

            }
        }

        protected void btninvoicenew_Click(object sender, EventArgs e)
        {
            int month = 0;
            int MonthSerch = 0;
            var MonthText = "";
            int fontsize = 0;
            fontsize = int.Parse(ddlfontsize.SelectedValue);
            month = GetMonthBasedOnSelectionDateorMonth();
            MonthText = month.ToString();
            if (MonthText.Length == 3)
            {
                MonthSerch = Convert.ToInt32(MonthText.Substring(1, 2));
            }
            if (MonthText.Length == 4)
            {
                MonthSerch = Convert.ToInt32(MonthText.Substring(2, 2));
            }
            if (gvClientBilling.Rows.Count > 0)
            {
                try
                {
                    MemoryStream ms = new MemoryStream();
                    Document document = new Document(PageSize.A4);
                    Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);

                    document.Open();

                    #region
                    BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    string strQry = "Select * from CompanyInfo   where   ClientidPrefix='" + CmpIDPrefix + "'";
                    DataTable compInfo = config.ExecuteReaderWithQueryAsync(strQry).Result;
                    string companyName = "Your Company Name";
                    string companyAddress = "Your Company Address";
                    string companyaddressline = " ";
                    string emailid = "";
                    string website = "";
                    string phoneno = "";
                    string faxno = "";
                    string PANNO = "";
                    string notes = "";
                    string PFNo = "";
                    string Esino = "";
                    string Servicetax = "";
                    string ServiceText = "";
                    decimal dutiestotal = 0;
                    //string Category = "";

                    string SACCode = "";
                    if (compInfo.Rows.Count > 0)
                    {
                        companyName = compInfo.Rows[0]["CompanyName"].ToString();
                        companyAddress = compInfo.Rows[0]["Address"].ToString();
                        //companyAddress = companyAddress.Replace("\r\n", string.Empty);
                        companyaddressline = compInfo.Rows[0]["Addresslineone"].ToString();
                        //CINNO = compInfo.Rows[0]["CINNO"].ToString();
                        PANNO = compInfo.Rows[0]["Labourrule"].ToString();
                        PFNo = compInfo.Rows[0]["PFNo"].ToString();
                        Esino = compInfo.Rows[0]["ESINo"].ToString();
                        Servicetax = compInfo.Rows[0]["BillNotes"].ToString();
                        emailid = compInfo.Rows[0]["Emailid"].ToString();
                        website = compInfo.Rows[0]["Website"].ToString();
                        phoneno = compInfo.Rows[0]["Phoneno"].ToString();
                        notes = compInfo.Rows[0]["Notes"].ToString();
                        faxno = compInfo.Rows[0]["Faxno"].ToString();
                        //Category = compInfo.Rows[0]["Category"].ToString();
                        SACCode = compInfo.Rows[0]["SACCode"].ToString();

                    }


                    DateTime DtLastDay = DateTime.Now;
                    if (Chk_Month.Checked == false)
                    {
                        DtLastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlmonth.SelectedIndex);
                    }
                    if (Chk_Month.Checked == true)
                    {
                        DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                    }

                    DateTime dtn = DateTime.ParseExact(txtbilldate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    // for both "1/1/2000" or "25/1/2000" formats
                    string billdt = dtn.ToString("MM/dd/yyyy");

                    string CGSTAlias = "";
                    string SGSTAlias = "";
                    string IGSTAlias = "";
                    string Cess1Alias = "";
                    string Cess2Alias = "";
                    string OurGSTINAlias = "";
                    string GSTINAlias = "";
                    var SqlQryForTaxes = @"select ServiceTaxSeparate,Cess,SHECess,SBCess,KKCess,CGST,SGST,IGST,cess1,cess2,CGSTAlias,SGSTAlias,IGSTAlias,cess1Alias,cess2Alias,GSTINAlias,OurGSTINAlias from TblOptions where '" + billdt + "' between fromdate and todate ";
                    DataTable DtTaxes = config.ExecuteReaderWithQueryAsync(SqlQryForTaxes).Result;


                    string SCPersent = "";
                    if (DtTaxes.Rows.Count > 0)
                    {
                        SCPersent = DtTaxes.Rows[0]["ServiceTaxSeparate"].ToString();
                        CGSTAlias = DtTaxes.Rows[0]["CGSTAlias"].ToString();
                        SGSTAlias = DtTaxes.Rows[0]["SGSTAlias"].ToString();
                        IGSTAlias = DtTaxes.Rows[0]["IGSTAlias"].ToString();
                        Cess1Alias = DtTaxes.Rows[0]["Cess1Alias"].ToString();
                        Cess2Alias = DtTaxes.Rows[0]["Cess2Alias"].ToString();

                        OurGSTINAlias = DtTaxes.Rows[0]["OurGSTINAlias"].ToString();
                        GSTINAlias = DtTaxes.Rows[0]["GSTINAlias"].ToString();
                    }
                    else
                    {
                        lblResult.Text = "There Is No Tax Values For Generating Bills ";
                        return;
                    }

                    var ContractID = "";

                    var bBillDates = 0;
                    var Gendays = 0;
                    var G_Sdays = 0;
                    decimal WorkingDays = 0;
                    var Wkdays = "";

                    #region  Begin Get Contract Id Based on The Last Day

                    Hashtable HtGetContractID = new Hashtable();
                    var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                    HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
                    HtGetContractID.Add("@LastDay", DtLastDay);
                    DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                    if (DTContractID.Rows.Count > 0)
                    {
                        ContractID = DTContractID.Rows[0]["contractid"].ToString();
                        bBillDates = int.Parse(DTContractID.Rows[0]["BillDates"].ToString());
                    }
                    #endregion

                    //

                    string SqlQuryForServiCharge = "select ContractId,servicecharge, convert(nvarchar(20), ContractStartDate, 103) as ContractStartDate,ServiceChargeType,Description,IncludeST,ServiceTax75,Pono,typeofwork,billdates from contracts where " +
                        " clientid ='" + ddlclientid.SelectedValue + "' and ContractId='" + ContractID + "'";
                    DataTable DtServicecharge = config.ExecuteReaderWithQueryAsync(SqlQuryForServiCharge).Result;
                    string Typeofwork = "";
                    string ServiceCharge = "0";
                    string strSCType = "";
                    string strDescription = "We are presenting our bill for the House Keeping Services Provided at your establishment. Kindly release the payment at the earliest";
                    bool bSCType = false;
                    string strIncludeST = "";
                    string ContractStartDate = "";
                    string strST75 = "";
                    bool bIncludeST = false;
                    bool bST75 = false;
                    string POContent = "";
                    string billdates = "0";
                    // string ServiceTaxCategory = "";
                    if (DtServicecharge.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(DtServicecharge.Rows[0]["ServiceCharge"].ToString()) == false)
                        {
                            ServiceCharge = DtServicecharge.Rows[0]["ServiceCharge"].ToString();
                        }
                        if (String.IsNullOrEmpty(DtServicecharge.Rows[0]["ServiceChargeType"].ToString()) == false)
                        {
                            strSCType = DtServicecharge.Rows[0]["ServiceChargeType"].ToString();
                        }
                        string tempDescription = DtServicecharge.Rows[0]["Description"].ToString();
                        if (tempDescription.Trim().Length > 0)
                        {
                            strDescription = tempDescription;
                        }
                        if (strSCType.Length > 0)
                        {
                            bSCType = Convert.ToBoolean(strSCType);
                        }
                        strIncludeST = DtServicecharge.Rows[0]["IncludeST"].ToString();
                        strST75 = DtServicecharge.Rows[0]["ServiceTax75"].ToString();
                        ContractStartDate = DtServicecharge.Rows[0]["ContractStartDate"].ToString();
                        if (strIncludeST == "True")
                        {
                            bIncludeST = true;
                        }
                        if (strST75 == "True")
                        {
                            bST75 = true;
                        }
                        POContent = DtServicecharge.Rows[0]["pono"].ToString();
                        Typeofwork = DtServicecharge.Rows[0]["typeofwork"].ToString();
                        billdates = DtServicecharge.Rows[0]["billdates"].ToString();
                        // ServiceTaxCategory = DtServicecharge.Rows[0]["ServiceTaxCategory"].ToString();
                    }

                    #endregion

                    document.AddTitle(companyName);
                    document.AddAuthor("DIYOS");
                    document.AddSubject("Invoice");
                    document.AddKeywords("Keyword1, keyword2, …");
                    string imagepath = Server.MapPath("~/assets/billlogo.png");


                    #region For header

                    if (File.Exists(imagepath))
                    {
                        iTextSharp.text.Image gif2 = iTextSharp.text.Image.GetInstance(imagepath);

                        gif2.Alignment = (iTextSharp.text.Image.ALIGN_LEFT | iTextSharp.text.Image.UNDERLYING);
                        // gif2.SpacingBefore = 50;
                        gif2.ScalePercent(70f);
                        gif2.SetAbsolutePosition(34f, 735f);
                        //document.Add(new Paragraph(" "));
                        document.Add(gif2);
                    }



                    #region

                    string selectclientaddress = "select sg.segname,c.*,s.state as Statename,s.GSTStateCode,gst.gstno,isnull(gst.BillPrefix,'') as BillPrefix,isnull(gst.GSTAddress, '') as GSTAddress from clients c inner join Segments sg on c.ClientSegment = sg.SegId  left join states s on s.stateid=c.state left join GSTMaster gst on gst.id=c.ourgstin where clientid= '" + ddlclientid.SelectedItem.ToString() + "'";
                    DataTable dtclientaddress = config.ExecuteReaderWithQueryAsync(selectclientaddress).Result;
                    string OurGSTIN = "";
                    string GSTIN = "";
                    string StateCode = "0";
                    string State = "";
                    string BillPrefix = "";
                    string GSTAddress = "";
                    if (dtclientaddress.Rows.Count > 0)
                    {
                        OurGSTIN = dtclientaddress.Rows[0]["gstno"].ToString();
                        StateCode = dtclientaddress.Rows[0]["GSTStateCode"].ToString();
                        GSTIN = dtclientaddress.Rows[0]["GSTIN"].ToString();
                        State = dtclientaddress.Rows[0]["Statename"].ToString();
                        BillPrefix = dtclientaddress.Rows[0]["BillPrefix"].ToString();
                        GSTAddress = dtclientaddress.Rows[0]["GSTAddress"].ToString();
                    }
                    string BQry = "select * from TblOptions  where '" + billdt + "' between fromdate and todate ";
                    DataTable Bdt = config.ExecuteReaderWithQueryAsync(BQry).Result;

                    string SelectBillNo = "";
                    if (ddlType.SelectedIndex == 0)
                    {
                        SelectBillNo = "Select * from UnitBill where   month='" + month + "' and unitid='" + ddlclientid.SelectedValue + "' ";
                    }
                    else
                    {
                        SelectBillNo = "Select * from MUnitBill where BillNo= '" + ddlMBBillnos.SelectedValue + "' and  month='" + month + "' and unitid='" + ddlclientid.SelectedValue + "' ";
                    }
                    DataTable DtBilling = config.ExecuteReaderWithQueryAsync(SelectBillNo).Result;

                    string BillNo = "";
                    string DisplayBillNo = "";
                    string area = "";
                    string location = "";
                    var BankAccountNo = "";
                    var IFSCCode = "";
                    var BankName = "";
                    if (dtclientaddress.Rows.Count > 0)
                    {
                        area = dtclientaddress.Rows[0]["segname"].ToString();
                        location = dtclientaddress.Rows[0]["location"].ToString();

                    }

                    DateTime BillDate;
                    DateTime DueDate;

                    string SignedQRCode = "";
                    string IRN = "";
                    string Status = "";
                    decimal Roundoffamt = 0;

                    #region Variables for data Fields as on 11/03/2014 by venkat


                    decimal servicecharge = 0;
                    decimal servicechargeper = 0;
                    decimal servicetax = 0;
                    decimal cess = 0;
                    decimal sbcess = 0;
                    decimal kkcess = 0;
                    decimal shecess = 0;
                    decimal servicetaxprc = 0;
                    decimal sbcessprc = 0;
                    decimal kkcessprc = 0;
                    decimal cessprc = 0;
                    decimal shecessprc = 0;
                    decimal totalamount = 0;
                    decimal Grandtotal = 0;
                    #region for GST on 20-6-2017 by sharada

                    decimal CGST = 0;
                    decimal SGST = 0;
                    decimal IGST = 0;
                    decimal Cess1 = 0;
                    decimal Cess2 = 0;
                    decimal CGSTPrc = 0;
                    decimal SGSTPrc = 0;
                    decimal IGSTPrc = 0;
                    decimal Cess1Prc = 0;
                    decimal Cess2Prc = 0;

                    #endregion for GST on 20-6-2017 by sharada
                    decimal ServiceTax75 = 0;
                    decimal ServiceTax25 = 0;

                    decimal machinarycost = 0;
                    decimal materialcost = 0;
                    decimal maintenancecost = 0;
                    decimal extraonecost = 0;
                    decimal extratwocost = 0;
                    decimal discountone = 0;
                    decimal discounttwo = 0;

                    string machinarycosttitle = "";
                    string materialcosttitle = "";
                    string maintenancecosttitle = "";
                    string extraonecosttitle = "";
                    string extratwocosttitle = "";
                    string discountonetitle = "";
                    string discounttwotitle = "";

                    bool Extradatacheck = false;
                    bool ExtraDataSTcheck = false;

                    bool STMachinary = false;
                    bool STMaterial = false;
                    bool STMaintenance = false;
                    bool STExtraone = false;
                    bool STExtratwo = false;

                    bool SCMachinary = false;
                    bool SCMaterial = false;
                    bool SCMaintenance = false;
                    bool SCExtraone = false;
                    bool SCExtratwo = false;

                    bool STDiscountone = false;
                    bool STDiscounttwo = false;

                    string strExtradatacheck = "";
                    string strExtrastcheck = "";

                    string strSTMachinary = "";
                    string strSTMaterial = "";
                    string strSTMaintenance = "";
                    string strSTExtraone = "";
                    string strSTExtratwo = "";

                    string strSCMachinary = "";
                    string strSCMaterial = "";
                    string strSCMaintenance = "";
                    string strSCExtraone = "";
                    string strSCExtratwo = "";

                    string strSTDiscountone = "";
                    string strSTDiscounttwo = "";

                    decimal staxamtonservicecharge = 0;
                    decimal RelChrgAmt = 0;


                    #endregion

                    if (DtBilling.Rows.Count > 0)
                    {

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["IRN"].ToString()) == false)
                        {
                            IRN = DtBilling.Rows[0]["IRN"].ToString();
                        }

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["QRCode"].ToString()) == false)
                        {
                            SignedQRCode = DtBilling.Rows[0]["QRCode"].ToString();
                        }

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["Status"].ToString()) == false)
                        {
                            Status = DtBilling.Rows[0]["Status"].ToString();
                        }

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["RoundOffAmt"].ToString()) == false)
                        {
                            Roundoffamt = decimal.Parse(DtBilling.Rows[0]["RoundOffAmt"].ToString());
                        }

                        BankAccountNo = DtBilling.Rows[0]["BankAccountNo"].ToString();
                        IFSCCode = DtBilling.Rows[0]["IFSCCode"].ToString();
                        BankName = DtBilling.Rows[0]["BankName"].ToString();
                        BillNo = DtBilling.Rows[0]["billno"].ToString();
                        BillDate = Convert.ToDateTime(DtBilling.Rows[0]["billdt"].ToString());
                        GSTAddress = DtBilling.Rows[0]["GSTAddress"].ToString();
                        phoneno = DtBilling.Rows[0]["phoneno"].ToString();
                        faxno = DtBilling.Rows[0]["faxno"].ToString();

                        // DueDate = Convert.ToDateTime(DtBilling.Rows[0]["duedt"].ToString());

                        #region Begin New code for values taken from database as on 11/03/2014 by venkat

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["TotalChrg"].ToString()) == false)
                        {
                            totalamount = decimal.Parse(DtBilling.Rows[0]["TotalChrg"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceChrg"].ToString()) == false)
                        {
                            servicecharge = decimal.Parse(DtBilling.Rows[0]["ServiceChrg"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceChrgPer"].ToString()) == false)
                        {
                            servicechargeper = decimal.Parse(DtBilling.Rows[0]["ServiceChrgPer"].ToString());
                        }

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax"].ToString()) == false)
                        {
                            servicetax = decimal.Parse(DtBilling.Rows[0]["ServiceTax"].ToString());
                        }

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["SBCessAmt"].ToString()) == false)
                        {
                            sbcess = decimal.Parse(DtBilling.Rows[0]["SBCessAmt"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["KKCessAmt"].ToString()) == false)
                        {
                            kkcess = decimal.Parse(DtBilling.Rows[0]["KKCessAmt"].ToString());
                        }
                        #region for GST as on 20-6-2017 by sharada

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["CGSTAmt"].ToString()) == false)
                        {
                            CGST = decimal.Parse(DtBilling.Rows[0]["CGSTAmt"].ToString());
                        }

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["SGSTAmt"].ToString()) == false)
                        {
                            SGST = decimal.Parse(DtBilling.Rows[0]["SGSTAmt"].ToString());
                        }

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["IGSTAmt"].ToString()) == false)
                        {
                            IGST = decimal.Parse(DtBilling.Rows[0]["IGSTAmt"].ToString());
                        }

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess1Amt"].ToString()) == false)
                        {
                            Cess1 = decimal.Parse(DtBilling.Rows[0]["Cess1Amt"].ToString());
                        }

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess2Amt"].ToString()) == false)
                        {
                            Cess2 = decimal.Parse(DtBilling.Rows[0]["Cess2Amt"].ToString());
                        }


                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["CGSTPrc"].ToString()) == false)
                        {
                            CGSTPrc = decimal.Parse(DtBilling.Rows[0]["CGSTPrc"].ToString());
                        }

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["SGSTPrc"].ToString()) == false)
                        {
                            SGSTPrc = decimal.Parse(DtBilling.Rows[0]["SGSTPrc"].ToString());
                        }

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["IGSTPrc"].ToString()) == false)
                        {
                            IGSTPrc = decimal.Parse(DtBilling.Rows[0]["IGSTPrc"].ToString());
                        }

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess1Prc"].ToString()) == false)
                        {
                            Cess1Prc = decimal.Parse(DtBilling.Rows[0]["Cess1Prc"].ToString());
                        }

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess2Prc"].ToString()) == false)
                        {
                            Cess2Prc = decimal.Parse(DtBilling.Rows[0]["Cess2Prc"].ToString());
                        }

                        #endregion for GST as on 17-6-2017 by swathi
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["CESS"].ToString()) == false)
                        {
                            cess = decimal.Parse(DtBilling.Rows[0]["CESS"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["SHECess"].ToString()) == false)
                        {
                            shecess = decimal.Parse(DtBilling.Rows[0]["SHECess"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTaxPrc"].ToString()) == false)
                        {
                            servicetaxprc = decimal.Parse(DtBilling.Rows[0]["ServiceTaxPrc"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["SBCessTaxPrc"].ToString()) == false)
                        {
                            sbcessprc = decimal.Parse(DtBilling.Rows[0]["SBCessTaxPrc"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["KKCessTaxPrc"].ToString()) == false)
                        {
                            kkcessprc = decimal.Parse(DtBilling.Rows[0]["KKCessTaxPrc"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["GrandTotal"].ToString()) == false)
                        {
                            Grandtotal = decimal.Parse(DtBilling.Rows[0]["GrandTotal"].ToString());
                        }


                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["MachinaryCost"].ToString()) == false)
                        {
                            machinarycost = decimal.Parse(DtBilling.Rows[0]["MachinaryCost"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["MaterialCost"].ToString()) == false)
                        {
                            materialcost = decimal.Parse(DtBilling.Rows[0]["MaterialCost"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ElectricalChrg"].ToString()) == false)
                        {
                            maintenancecost = decimal.Parse(DtBilling.Rows[0]["ElectricalChrg"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraAmtone"].ToString()) == false)
                        {
                            extraonecost = decimal.Parse(DtBilling.Rows[0]["ExtraAmtone"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraAmtTwo"].ToString()) == false)
                        {
                            extratwocost = decimal.Parse(DtBilling.Rows[0]["ExtraAmtTwo"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["Discount"].ToString()) == false)
                        {
                            discountone = decimal.Parse(DtBilling.Rows[0]["Discount"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["Discounttwo"].ToString()) == false)
                        {
                            discounttwo = decimal.Parse(DtBilling.Rows[0]["Discounttwo"].ToString());
                        }

                        //machinarycosttitle = DtBilling.Rows[0]["Machinarycosttitle"].ToString();
                        //materialcosttitle = DtBilling.Rows[0]["Materialcosttitle"].ToString();
                        //maintenancecosttitle = DtBilling.Rows[0]["Maintanancecosttitle"].ToString();
                        //extraonecosttitle = DtBilling.Rows[0]["Extraonetitle"].ToString();
                        //extratwocosttitle = DtBilling.Rows[0]["Extratwotitle"].ToString();
                        //discountonetitle = DtBilling.Rows[0]["Discountonetitle"].ToString();
                        //discounttwotitle = DtBilling.Rows[0]["Discounttwotitle"].ToString();

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["Extradatacheck"].ToString()) == false)
                        //{
                        //    strExtradatacheck = DtBilling.Rows[0]["Extradatacheck"].ToString();
                        //    if (strExtradatacheck == "True")
                        //    {
                        //        Extradatacheck = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraDataSTcheck"].ToString()) == false)
                        //{
                        //    strExtrastcheck = DtBilling.Rows[0]["ExtraDataSTcheck"].ToString();
                        //    if (strExtrastcheck == "True")
                        //    {
                        //        ExtraDataSTcheck = true;
                        //    }
                        //}



                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMachinary"].ToString()) == false)
                        //{
                        //    strSTMachinary = DtBilling.Rows[0]["STMachinary"].ToString();
                        //    if (strSTMachinary == "True")
                        //    {
                        //        STMachinary = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMaterial"].ToString()) == false)
                        //{
                        //    strSTMaterial = DtBilling.Rows[0]["STMaterial"].ToString();
                        //    if (strSTMaterial == "True")
                        //    {
                        //        STMaterial = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMaintenance"].ToString()) == false)
                        //{
                        //    strSTMaintenance = DtBilling.Rows[0]["STMaintenance"].ToString();
                        //    if (strSTMaintenance == "True")
                        //    {
                        //        STMaintenance = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STExtraone"].ToString()) == false)
                        //{
                        //    strSTExtraone = DtBilling.Rows[0]["STExtraone"].ToString();
                        //    if (strSTExtraone == "True")
                        //    {
                        //        STExtraone = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STExtratwo"].ToString()) == false)
                        //{
                        //    strSTExtratwo = DtBilling.Rows[0]["STExtratwo"].ToString();
                        //    if (strSTExtratwo == "True")
                        //    {
                        //        STExtratwo = true;
                        //    }
                        //}


                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMachinary"].ToString()) == false)
                        //{
                        //    strSCMachinary = DtBilling.Rows[0]["SCMachinary"].ToString();
                        //    if (strSCMachinary == "True")
                        //    {
                        //        SCMachinary = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMaterial"].ToString()) == false)
                        //{
                        //    strSCMaterial = DtBilling.Rows[0]["SCMaterial"].ToString();
                        //    if (strSCMaterial == "True")
                        //    {
                        //        SCMaterial = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMaintenance"].ToString()) == false)
                        //{
                        //    strSCMaintenance = DtBilling.Rows[0]["SCMaintenance"].ToString();
                        //    if (strSCMaintenance == "True")
                        //    {
                        //        SCMaintenance = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCExtraone"].ToString()) == false)
                        //{
                        //    strSCExtraone = DtBilling.Rows[0]["SCExtraone"].ToString();
                        //    if (strSCExtraone == "True")
                        //    {
                        //        SCExtraone = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCExtratwo"].ToString()) == false)
                        //{
                        //    strSCExtratwo = DtBilling.Rows[0]["SCExtratwo"].ToString();
                        //    if (strSCExtratwo == "True")
                        //    {
                        //        SCExtratwo = true;
                        //    }
                        //}


                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscountone"].ToString()) == false)
                        //{
                        //    strSTDiscountone = DtBilling.Rows[0]["STDiscountone"].ToString();
                        //    if (strSTDiscountone == "True")
                        //    {
                        //        STDiscountone = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscounttwo"].ToString()) == false)
                        //{
                        //    strSTDiscounttwo = DtBilling.Rows[0]["STDiscounttwo"].ToString();
                        //    if (strSTDiscounttwo == "True")
                        //    {
                        //        STDiscounttwo = true;
                        //    }
                        //}


                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax75"].ToString()) == false)
                        //{
                        //    ServiceTax75 = decimal.Parse(DtBilling.Rows[0]["ServiceTax75"].ToString());
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax25"].ToString()) == false)
                        //{
                        //    ServiceTax25 = decimal.Parse(DtBilling.Rows[0]["ServiceTax25"].ToString());
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["Staxonservicecharge"].ToString()) == false)
                        //{
                        //    staxamtonservicecharge = decimal.Parse(DtBilling.Rows[0]["Staxonservicecharge"].ToString());
                        //}

                        #endregion
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Generate The Bill Once Again');", true);
                        return;
                    }
                    string Year = DateTime.Now.Year.ToString();
                    #endregion

                    #region for companyinfo

                    PdfPTable tablelogo = new PdfPTable(2);
                    tablelogo.TotalWidth = 560f;
                    tablelogo.LockedWidth = true;
                    float[] widtlogo = new float[] { 2f, 2f };
                    tablelogo.SetWidths(widtlogo);

                    PdfPCell CCompName = new PdfPCell(new Paragraph(companyName, FontFactory.GetFont(FontStyle, 14, Font.BOLD, BaseColor.BLACK)));
                    CCompName.HorizontalAlignment = 1;
                    CCompName.BorderWidthBottom = 0;
                    CCompName.BorderWidthTop = 1.5f;
                    CCompName.BorderWidthRight = 1.5f;
                    CCompName.BorderWidthLeft = 1.5f;
                    CCompName.Colspan = 2;
                    CCompName.SetLeading(0f, 1.3f);
                    tablelogo.AddCell(CCompName);

                    PdfPCell CCompAddress = new PdfPCell(new Paragraph(GSTAddress, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    CCompAddress.HorizontalAlignment = 1;
                    CCompAddress.BorderWidthBottom = 0;
                    CCompAddress.BorderWidthTop = 0f;
                    CCompAddress.BorderWidthRight = 1.5f;
                    CCompAddress.BorderWidthLeft = 1.5f;
                    CCompAddress.Colspan = 2;
                    //CCompAddress.PaddingLeft = 50;
                    //CCompAddress.FixedHeight = 70;
                    CCompAddress.SetLeading(0f, 1.3f);
                    tablelogo.AddCell(CCompAddress);

                    PdfPCell CCompPhone = new PdfPCell(new Paragraph("Tel: " + phoneno, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    CCompPhone.HorizontalAlignment = 1;
                    CCompPhone.BorderWidthBottom = 0;
                    CCompPhone.BorderWidthTop = 0f;
                    CCompPhone.PaddingLeft = 180f;
                    CCompPhone.BorderWidthRight = 0f;
                    CCompPhone.BorderWidthLeft = 1.5f;
                    CCompPhone.Colspan = 1;
                    //CCompPhone.PaddingLeft = 50;
                    //CCompAddress.FixedHeight = 70;
                    CCompPhone.SetLeading(0f, 1.3f);
                    tablelogo.AddCell(CCompPhone);

                    PdfPCell CCompFax = new PdfPCell(new Paragraph(" Fax: " + faxno, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    CCompFax.HorizontalAlignment = 1;
                    CCompFax.BorderWidthBottom = 0;
                    CCompFax.BorderWidthTop = 0f;
                    CCompFax.BorderWidthRight = 1.5f;
                    CCompFax.BorderWidthLeft = 0f;
                    CCompFax.PaddingLeft = -200f;
                    //CCompFax.PaddingTop = 2f;
                    CCompFax.Colspan = 1;
                    //CCompFax.PaddingLeft = 50;
                    //CCompAddress.FixedHeight = 70;
                    CCompFax.SetLeading(0f, 1.3f);
                    tablelogo.AddCell(CCompFax);


                    PdfPCell Celemail = new PdfPCell(new Paragraph("Email :" + emailid, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    Celemail.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Celemail.BorderWidthBottom = 1.5f;
                    Celemail.BorderWidthTop = 0f;
                    Celemail.PaddingBottom = 5f;
                    //Celemail.PaddingTop = 5f;
                    Celemail.BorderWidthRight = 1.5f;
                    Celemail.BorderWidthLeft = 1.5f;
                    Celemail.Colspan = 2;
                    //Celemail.FixedHeight = 20;
                    tablelogo.AddCell(Celemail);

                    //For Space

                    PdfPCell celll = new PdfPCell(new Paragraph("\n", FontFactory.GetFont(FontStyle, 12, Font.NORMAL, BaseColor.BLACK)));
                    celll.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                    celll.Colspan = 2;
                    //tablelogo.AddCell(celll);

                    // tablelogo.AddCell(celll);

                    PdfPCell CInvoice = new PdfPCell(new Paragraph("TAX INVOICE", FontFactory.GetFont(FontStyle, 16, Font.BOLD, BaseColor.BLACK)));
                    CInvoice.HorizontalAlignment = 1;
                    CInvoice.BorderWidthBottom = 1.5f;
                    CInvoice.BorderWidthTop = 0;
                    CInvoice.FixedHeight = 25;
                    CInvoice.BorderWidthRight = 1.5f;
                    CInvoice.BorderWidthLeft = 1.5f;
                    CInvoice.Colspan = 2;
                    tablelogo.AddCell(CInvoice);

                    //tablelogo.AddCell(celll);

                    document.Add(tablelogo);
                    #endregion

                    PdfPTable address = new PdfPTable(5);
                    address.TotalWidth = 560f;
                    address.LockedWidth = true;
                    float[] addreslogo = new float[] { 2f, 2f, 2f, 2f, 2f };
                    address.SetWidths(addreslogo);

                    PdfPTable tempTable1 = new PdfPTable(3);
                    tempTable1.TotalWidth = 336f;
                    tempTable1.LockedWidth = true;
                    float[] tempWidth1 = new float[] { 2f, 2f, 2f };
                    tempTable1.SetWidths(tempWidth1);

                    PdfPCell mress = new PdfPCell(new Paragraph("Customer's Details:", FontFactory.GetFont(FontStyle, fontsize, Font.UNDERLINE | Font.BOLD, BaseColor.BLACK)));
                    mress.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    mress.BorderWidthBottom = 0;
                    mress.BorderWidthTop = 0;
                    mress.Colspan = 3;
                    mress.BorderWidthLeft = 1.5f;
                    mress.BorderWidthRight = 0.5f;
                    tempTable1.AddCell(mress);

                    string addressData = "";

                    addressData = dtclientaddress.Rows[0]["ClientAddrHno"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientaddrhno = new PdfPCell(new Paragraph("M/s. " + addressData, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                        clientaddrhno.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientaddrhno.Colspan = 3;                                 //clientaddrhno.Colspan = 0;
                        clientaddrhno.BorderWidthBottom = 0;
                        clientaddrhno.BorderWidthTop = 0;
                        clientaddrhno.BorderWidthLeft = 1.5f;
                        clientaddrhno.BorderWidthRight = 0.5f;
                        //clientaddrhno.clientaddrhno = 20;
                        tempTable1.AddCell(clientaddrhno);
                    }
                    addressData = dtclientaddress.Rows[0]["ClientAddrStreet"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                        clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientstreet.BorderWidthBottom = 0;
                        clientstreet.BorderWidthTop = 0;
                        clientstreet.Colspan = 3;
                        clientstreet.BorderWidthLeft = 1.5f;
                        clientstreet.BorderWidthRight = 0.5f;
                        //clientstreet.PaddingLeft = 20;
                        tempTable1.AddCell(clientstreet);
                    }


                    addressData = dtclientaddress.Rows[0]["ClientAddrArea"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                        clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientstreet.BorderWidthBottom = 0;
                        clientstreet.BorderWidthTop = 0;
                        clientstreet.Colspan = 3;

                        clientstreet.BorderWidthLeft = 1.5f;
                        clientstreet.BorderWidthRight = 0.5f;
                        // clientstreet.PaddingLeft = 20;
                        tempTable1.AddCell(clientstreet);
                    }


                    addressData = dtclientaddress.Rows[0]["ClientAddrColony"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientcolony = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                        clientcolony.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientcolony.Colspan = 3;
                        clientcolony.BorderWidthBottom = 0;
                        clientcolony.BorderWidthTop = 0;
                        clientcolony.BorderWidthLeft = 1.5f;
                        clientcolony.BorderWidthRight = 0.5f;
                        //clientcolony.PaddingLeft = 20;
                        tempTable1.AddCell(clientcolony);
                    }
                    addressData = dtclientaddress.Rows[0]["ClientAddrcity"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientcity = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                        clientcity.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientcity.Colspan = 3;
                        clientcity.BorderWidthBottom = 0;
                        clientcity.BorderWidthTop = 0;
                        clientcity.BorderWidthLeft = 1.5f;
                        clientcity.BorderWidthRight = 0.5f;
                        //  clientcity.PaddingLeft = 20;
                        tempTable1.AddCell(clientcity);
                    }
                    addressData = dtclientaddress.Rows[0]["ClientAddrstate"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientstate = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                        clientstate.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientstate.Colspan = 3;
                        clientstate.BorderWidthBottom = 0;
                        clientstate.BorderWidthTop = 0;
                        clientstate.BorderWidthLeft = 1.5f;
                        clientstate.BorderWidthRight = 0.5f;
                        // clientstate.PaddingLeft = 20;
                        tempTable1.AddCell(clientstate);
                    }
                    addressData = dtclientaddress.Rows[0]["ClientAddrpin"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clietnpin = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                        clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clietnpin.Colspan = 3;
                        clietnpin.BorderWidthBottom = 0;
                        clietnpin.BorderWidthTop = 0;
                        clietnpin.BorderWidthLeft = 1.5f;
                        clietnpin.BorderWidthRight = 0.5f;
                        // clientstate.PaddingLeft = 20;
                        tempTable1.AddCell(clietnpin);
                    }



                    if (Bdt.Rows.Count > 0)
                    {

                        if (GSTIN.Length > 0)
                        {
                            PdfPCell clietnpin = new PdfPCell(new Paragraph(GSTINAlias + ":  " + GSTIN, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                            clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clietnpin.Colspan = 3;
                            clietnpin.BorderWidthBottom = 0;
                            clietnpin.BorderWidthTop = 0;
                            clietnpin.BorderWidthLeft = 1.5f;
                            clietnpin.BorderWidthRight = 0.5f;
                            //  clietnpin.PaddingLeft = 20;
                            tempTable1.AddCell(clietnpin);
                        }

                        if (State.Length > 0)
                        {
                            PdfPCell clietnpin = new PdfPCell(new Paragraph("State: " + State, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                            clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clietnpin.Colspan = 3;
                            clietnpin.BorderWidthBottom = 0;
                            clietnpin.BorderWidthTop = 0;
                            clietnpin.BorderWidthLeft = 1.5f;
                            clietnpin.BorderWidthRight = 0.5f;
                            //  clietnpin.PaddingLeft = 20;
                            tempTable1.AddCell(clietnpin);
                        }
                        if (StateCode.Length > 0)
                        {
                            PdfPCell clietnpin = new PdfPCell(new Paragraph("State Code:  " + StateCode, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                            clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clietnpin.Colspan = 3;
                            clietnpin.BorderWidthBottom = 0;
                            clietnpin.BorderWidthTop = 0;
                            clietnpin.BorderWidthLeft = 1.5f;
                            clietnpin.BorderWidthRight = 0.5f;
                            //  clietnpin.PaddingLeft = 20;
                            tempTable1.AddCell(clietnpin);
                        }

                    }




                    PdfPCell childTable1 = new PdfPCell(tempTable1);
                    childTable1.Border = 0;
                    childTable1.Colspan = 3;
                    // childTable1.FixedHeight = 100;
                    childTable1.HorizontalAlignment = 0;
                    address.AddCell(childTable1);

                    PdfPTable tempTable2 = new PdfPTable(2);
                    tempTable2.TotalWidth = 224f;
                    tempTable2.LockedWidth = true;
                    float[] tempWidth2 = new float[] { 1f, 1f };
                    tempTable2.SetWidths(tempWidth2);



                    var phrase = new Phrase();
                    phrase.Add(new Chunk("Bill No", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    PdfPCell cell13 = new PdfPCell();
                    cell13.AddElement(phrase);
                    cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell13.BorderWidthBottom = 0;
                    cell13.BorderWidthTop = 0;
                    //cell13.FixedHeight = 35;
                    cell13.Colspan = 1;
                    cell13.BorderWidthLeft = 0.5f;
                    cell13.BorderWidthRight = 0f;
                    cell13.PaddingTop = -5;
                    tempTable2.AddCell(cell13);

                    var phrase10 = new Phrase();
                    phrase10.Add(new Chunk(": " + BillNo, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    PdfPCell cell13v = new PdfPCell();
                    cell13v.AddElement(phrase10);
                    cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell13v.BorderWidthBottom = 0;
                    cell13v.PaddingLeft = -50;
                    cell13v.BorderWidthTop = 0;
                    //cell13.FixedHeight = 35;
                    cell13v.Colspan = 1;
                    cell13v.BorderWidthLeft = 0;
                    cell13v.BorderWidthRight = 1.5f;
                    cell13v.PaddingTop = -5;
                    tempTable2.AddCell(cell13v);

                    var phrase11 = new Phrase();
                    phrase11.Add(new Chunk("Date", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    PdfPCell cell131 = new PdfPCell();
                    cell131.AddElement(phrase11);
                    cell131.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell131.BorderWidthBottom = 0;
                    cell131.BorderWidthTop = 0;
                    // cell131.FixedHeight = 35;
                    cell131.Colspan = 1;
                    cell131.BorderWidthLeft = 0.5f;
                    cell131.BorderWidthRight = 0f;
                    cell131.PaddingTop = -5;
                    tempTable2.AddCell(cell131);

                    var phrase11v = new Phrase();
                    phrase11v.Add(new Chunk(": " + BillDate.Day.ToString("00") + "/" + BillDate.ToString("MM") + "/" +
                        BillDate.Year, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    PdfPCell cell131v = new PdfPCell();
                    cell131v.AddElement(phrase11v);
                    cell131v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell131v.BorderWidthBottom = 0;
                    cell131v.BorderWidthTop = 0;
                    // cell131.FixedHeight = 35;
                    cell131v.PaddingLeft = -50;
                    cell131v.Colspan = 1;
                    cell131v.BorderWidthLeft = 0;
                    cell131v.BorderWidthRight = 1.5f;
                    cell131v.PaddingTop = -5;
                    tempTable2.AddCell(cell131v);

                    var phrase12 = new Phrase();
                    phrase12.Add(new Chunk("PAN No.", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    PdfPCell cell1311 = new PdfPCell();
                    cell1311.AddElement(phrase12);
                    cell1311.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell1311.BorderWidthBottom = 0;
                    cell1311.BorderWidthTop = 0;
                    //cell1311.FixedHeight = 35;
                    cell1311.Colspan = 1;
                    cell1311.BorderWidthLeft = 0.5f;
                    cell1311.BorderWidthRight = 0f;
                    cell1311.PaddingTop = -5;
                    tempTable2.AddCell(cell1311);

                    var phrase12v = new Phrase();
                    phrase12v.Add(new Chunk(": " + PANNO, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    PdfPCell cell1311v = new PdfPCell();
                    cell1311v.AddElement(phrase12v);
                    cell1311v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell1311v.BorderWidthBottom = 0;
                    cell1311v.BorderWidthTop = 0;
                    cell1311v.PaddingLeft = -50f;
                    //cell1311v.FixedHeight = 35;
                    cell1311v.Colspan = 1;
                    cell1311v.BorderWidthLeft = 0;
                    cell1311v.BorderWidthRight = 1.5f;
                    cell1311v.PaddingTop = -5;
                    tempTable2.AddCell(cell1311v);

                    if (dtn < DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    {
                        if (Servicetax.Trim().Length > 0)
                        {
                            var phrase2gg = new Phrase();
                            phrase2gg.Add(new Chunk("S Tax No", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            PdfPCell cell115 = new PdfPCell();
                            cell115.AddElement(phrase2gg);
                            cell115.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cell115.BorderWidthBottom = 0;
                            cell115.BorderWidthTop = 0f;
                            cell115.Colspan = 1;
                            cell115.BorderWidthLeft = 0.5f;
                            cell115.BorderWidthRight = 0f;
                            cell115.PaddingTop = -5;
                            tempTable2.AddCell(cell115);

                            var phrase2ggv = new Phrase();
                            phrase2ggv.Add(new Chunk(": " + Servicetax, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            PdfPCell cell15v = new PdfPCell();
                            cell15v.AddElement(phrase2ggv);
                            cell15v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cell15v.BorderWidthBottom = 0;
                            cell15v.BorderWidthTop = 0f;
                            cell15v.PaddingLeft = -50f;
                            cell15v.Colspan = 1;
                            cell15v.BorderWidthLeft = 0;
                            cell15v.BorderWidthRight = 1.5f;
                            cell15v.PaddingTop = -5;
                            tempTable2.AddCell(cell15v);

                        }
                    }

                    if (Bdt.Rows.Count > 0)
                    {

                        if (OurGSTIN.Length > 0)
                        {

                            var phrase21v = new Phrase();
                            phrase21v.Add(new Chunk(OurGSTINAlias, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            PdfPCell cell16v = new PdfPCell();
                            cell16v.AddElement(phrase21v);
                            cell16v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cell16v.BorderWidthBottom = 0;
                            cell16v.BorderWidthTop = 0f;
                            cell16v.Colspan = 1;
                            cell16v.BorderWidthLeft = 0.5f;
                            cell16v.BorderWidthRight = 0;
                            cell16v.PaddingTop = -5;
                            tempTable2.AddCell(cell16v);

                            var phrase21vv = new Phrase();
                            phrase21vv.Add(new Chunk(": " + OurGSTIN, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            PdfPCell cell16vv = new PdfPCell();
                            cell16vv.AddElement(phrase21vv);
                            cell16vv.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cell16vv.BorderWidthBottom = 0;
                            cell16vv.BorderWidthTop = 0f;
                            cell16vv.Colspan = 1;
                            cell16vv.BorderWidthLeft = 0;
                            cell16vv.BorderWidthRight = 1.5f;
                            cell16vv.PaddingLeft = -50f;
                            cell16vv.PaddingTop = -5;
                            tempTable2.AddCell(cell16vv);
                        }
                    }

                    if (SACCode.Trim().Length > 0)
                    {
                        var phrase222 = new Phrase();
                        phrase222.Add(new Chunk("SAC Code", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                        PdfPCell cell117 = new PdfPCell();
                        cell117.AddElement(phrase222);
                        cell117.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell117.BorderWidthBottom = 0;
                        cell117.BorderWidthTop = 0f;
                        cell117.Colspan = 1;
                        cell117.BorderWidthLeft = 0.5f;
                        cell117.BorderWidthRight = 0f;
                        cell117.PaddingTop = -5;
                        tempTable2.AddCell(cell117);

                        var phrase222v = new Phrase();
                        phrase222v.Add(new Chunk(": " + SACCode, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                        PdfPCell cell17v = new PdfPCell();
                        cell17v.AddElement(phrase222v);
                        cell17v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell17v.BorderWidthBottom = 0;
                        cell17v.BorderWidthTop = 0f;
                        cell17v.PaddingLeft = -50f;
                        cell17v.Colspan = 1;
                        cell17v.BorderWidthLeft = 0;
                        cell17v.BorderWidthRight = 1.5f;
                        cell17v.PaddingTop = -5;
                        tempTable2.AddCell(cell17v);

                    }
                    string Fromdate = txtfromdate.Text;
                    string Todate = txttodate.Text;

                    if (billdates != "0")
                    {
                        var phrase2 = new Phrase();
                        phrase2.Add(new Chunk("Bill Period  ", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                        PdfPCell cell14 = new PdfPCell();
                        cell14.AddElement(phrase2);
                        cell14.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell14.BorderWidthBottom = 0;
                        cell14.BorderWidthTop = 0f;
                        cell14.Colspan = 1;
                        cell14.BorderWidthLeft = 0.5f;
                        cell14.BorderWidthRight = 0f;
                        cell14.PaddingTop = -5;
                        tempTable2.AddCell(cell14);

                        var phrase2v = new Phrase();
                        phrase2v.Add(new Chunk(": " + Fromdate + "  to  " +
                            Todate + " ", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                        PdfPCell cell14v = new PdfPCell();
                        cell14v.AddElement(phrase2v);
                        cell14v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell14v.BorderWidthBottom = 0;
                        cell14v.BorderWidthTop = 0f;
                        cell14v.PaddingLeft = -50f;
                        cell14v.Colspan = 1;
                        cell14v.BorderWidthLeft = 0;
                        cell14v.BorderWidthRight = 1.5f;
                        cell14v.PaddingTop = -5;
                        tempTable2.AddCell(cell14v);
                    }

                    if (SignedQRCode.Length > 0)
                    {
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(SignedQRCode, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(20);
                        iTextSharp.text.Image qrcodeimg = iTextSharp.text.Image.GetInstance(qrCodeImage, System.Drawing.Imaging.ImageFormat.Bmp);
                        qrcodeimg.ScalePercent(4.7f);
                        if (billdates != "0")
                        {
                            qrcodeimg.SetAbsolutePosition(440f, 495f);
                        }
                        else
                        {
                            qrcodeimg.SetAbsolutePosition(440f, 506f);
                        }
                        document.Add(qrcodeimg);
                    }

                    if (IRN.Length > 0)
                    {
                        if (Status == "ACT")
                        {
                            PdfPCell cell14 = new PdfPCell(new Paragraph("IRN : " + IRN, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                            cell14.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cell14.BorderWidthBottom = 0;
                            cell14.BorderWidthTop = 0f;
                            cell14.SetLeading(0, 1.2f);
                            cell14.Colspan = 2;
                            cell14.BorderWidthLeft = 0.5f;
                            cell14.BorderWidthRight = 1.5f;
                            cell14.PaddingTop = 120;
                            tempTable2.AddCell(cell14);
                        }
                        if (Status == "CNL")
                        {
                            PdfPCell cell14 = new PdfPCell(new Paragraph("IRN is cancelled", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                            cell14.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cell14.BorderWidthBottom = 0;
                            cell14.BorderWidthTop = 0f;
                            cell14.SetLeading(0, 1.2f);
                            cell14.Colspan = 2;
                            cell14.BorderWidthLeft = 0.5f;
                            cell14.BorderWidthRight = 1.5f;
                            cell14.PaddingTop = 120;
                            tempTable2.AddCell(cell14);
                        }
                    }


                    PdfPCell childTable2 = new PdfPCell(tempTable2);
                    childTable2.Border = 0;
                    childTable2.Colspan = 2;
                    //childTable2.FixedHeight = 100;
                    childTable2.HorizontalAlignment = 0;
                    address.AddCell(childTable2);
                    // address.AddCell(celll);


                    document.Add(address);




                    PdfPTable address1 = new PdfPTable(1);
                    address1.TotalWidth = 560f;
                    address1.LockedWidth = true;
                    float[] addreslogo1 = new float[] { 2f };
                    address1.SetWidths(addreslogo1);


                    PdfPCell cellser = new PdfPCell(new Phrase("Sub: -We are presenting our bill for the Security Services provided at your establishment for the month of " + GetMonthName() + " " + GetMonthOfYear() + ".Kindly release the payment at the earliest ", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellser.HorizontalAlignment = 0;
                    cellser.BorderWidthBottom = 0.5f;
                    cellser.BorderWidthLeft = 1.5f;
                    cellser.BorderWidthTop = 0.5f;
                    cellser.BorderWidthRight = 1.5f;
                    cellser.FixedHeight = 25;
                    address1.AddCell(cellser);

                    document.Add(address1);
                    #endregion

                    #region

                    int colCount = 6;

                    PdfPTable table = new PdfPTable(colCount);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.HorizontalAlignment = 1;
                    float[] colWidths = new float[] { 1.2f, 6.2f, 2f, 2.2f, 2f, 2.7f };
                    table.SetWidths(colWidths);
                    PdfPCell cell;
                    string cellText;

                    #region for gridview

                    cell = new PdfPCell(new Phrase("S.No", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    //set the background color for the header cell
                    cell.HorizontalAlignment = 1;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthLeft = 1.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthRight = 0.5f;
                    //cell.BorderColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Description", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    //set the background color for the header cell
                    cell.HorizontalAlignment = 1;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthRight = 0.5f;
                    //cell.BorderColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("No Of Days Per Month ", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    //set the background color for the header cell
                    cell.HorizontalAlignment = 1;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthRight = 0.5f;
                    //cell.BorderColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("No of shifts", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    //set the background color for the header cell
                    cell.HorizontalAlignment = 1;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthRight = 0.5f;
                    //cell.BorderColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Rate(Rs)", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    //cell.HorizontalAlignment = 1;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthRight = 0.5f;
                    //cell.BorderColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cellText = "Amount(Rs)";
                    //create a new cell with header text
                    cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    //set the background color for the header cell
                    cell.HorizontalAlignment = 1;
                    //cell.HorizontalAlignment = 1;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthRight = 1.5f;
                    //cell.BorderColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    decimal noofshifts = 0;
                    ////export rows from GridView to table
                    for (int rowIndex = 0; rowIndex < gvClientBilling.Rows.Count; rowIndex++)
                    {
                        if (gvClientBilling.Rows[rowIndex].RowType == DataControlRowType.DataRow)
                        //gvClientBilling.RowStyle.BorderColor = System.Drawing.Color.Gray;
                        {
                            TextBox lblamount = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("lblda"));
                            if (lblamount != null)
                            {
                                string strAmount = lblamount.Text;
                                decimal amount = 0;
                                if (strAmount.Length > 0)
                                    amount = Convert.ToDecimal(strAmount);
                                //if (amount >= 0)
                                {
                                    for (int j = 0; j < 6; j++)
                                    {
                                        //fetch the column value of the current row
                                        if (j == 0)
                                        {
                                            Label label1 = (Label)(gvClientBilling.Rows[rowIndex].FindControl("lblSno"));
                                            cellText = label1.Text;
                                            //create a new cell with column value
                                            cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                            cell.Colspan = 1;
                                            cell.BorderWidthRight = 0.5f;
                                            cell.BorderWidthLeft = 1.5f;
                                            cell.BorderWidthBottom = 0;
                                            cell.BorderWidthTop = 0;
                                            cell.MinimumHeight = 14;
                                            cell.HorizontalAlignment = 1;
                                            table.AddCell(cell);
                                        }

                                        if (j == 1)
                                        {
                                            //TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtnoofemployees"));
                                            TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("lbldesgn"));
                                            if (label1.Text == "0")
                                            {
                                                cellText = "";
                                            }
                                            else
                                            {
                                                cellText = label1.Text;
                                            }
                                            //create a new cell with column value
                                            cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 0;
                                            cell.Colspan = 1;
                                            cell.BorderWidthRight = 0.5f;
                                            cell.BorderWidthLeft = 0.5f;
                                            cell.BorderWidthBottom = 0;
                                            cell.BorderWidthTop = 0;
                                            table.AddCell(cell);
                                        }

                                        if (j == 2)
                                        {


                                            if (Chk_Month.Checked == false)
                                            {
                                                Gendays = Timings.Instance.GetNoofDaysForSelectedMonth(ddlmonth.SelectedIndex, bBillDates);
                                            }

                                            //New Code when select month in Textbox
                                            if (Chk_Month.Checked == true)
                                            {
                                                DateTime mGendays = DateTime.Now;
                                                DateTime date = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                                                mGendays = DateTime.Parse(date.ToString());
                                                Gendays = Timings.Instance.GetNoofDaysForEnteredMonth(mGendays, bBillDates);
                                            }
                                            G_Sdays = Timings.Instance.Get_GS_Days(month, Gendays);

                                            // TextBox label2 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("lbldesgn"));

                                            string NoofDaysQry = "select c.noofdays,c.PayType,c.quantity from contractdetails c inner join designations d on c.designations = d.designid " +
                                               "  where c.clientid='" + ddlclientid.SelectedValue + "'";

                                            DataTable dt = config.ExecuteReaderWithQueryAsync(NoofDaysQry).Result;

                                            decimal Quantity = 0;

                                            if (dt.Rows.Count > 0)
                                            {
                                                Quantity = decimal.Parse(dt.Rows[0]["Quantity"].ToString());
                                                decimal noofdays = decimal.Parse(dt.Rows[0]["noofdays"].ToString());

                                                if (noofdays == 0)
                                                {
                                                    WorkingDays = Gendays;

                                                }
                                                else if (noofdays == 1)
                                                {
                                                    WorkingDays = G_Sdays;
                                                }
                                                else if (noofdays == 2)
                                                {
                                                    WorkingDays = (Gendays - 4);
                                                }
                                                else
                                                {
                                                    WorkingDays = noofdays;
                                                }

                                            }

                                            if (WorkingDays > 0)
                                            {
                                                Wkdays = (WorkingDays).ToString();
                                            }
                                            else
                                            {
                                                Wkdays = "";
                                            }
                                            // TextBox label3 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("lbldesgn"));

                                            DropDownList label1 = (DropDownList)(gvClientBilling.Rows[rowIndex].FindControl("ddlnod"));
                                            DropDownList dutytype = (DropDownList)(gvClientBilling.Rows[rowIndex].FindControl("ddldutytype"));


                                            if (dutytype.SelectedIndex == 5)
                                            {
                                                cellText = "";
                                            }
                                            else
                                            {
                                                cellText = label1.Text;
                                            }
                                            //create a new cell with column value
                                            cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 1;
                                            cell.Colspan = 1;
                                            cell.BorderWidthRight = 0.5f;
                                            cell.BorderWidthLeft = 0.5f;
                                            cell.BorderWidthBottom = 0;
                                            cell.BorderWidthTop = 0;
                                            table.AddCell(cell);


                                        }
                                        if (j == 3)
                                        {

                                            TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("lblNoOfDuties"));

                                            if (label1.Text == "")
                                            {
                                                label1.Text = "0";
                                            }

                                            cellText = label1.Text;

                                            if (cellText == "0")
                                            {
                                                cellText = "";
                                            }
                                            dutiestotal += (decimal.Parse(label1.Text));

                                            cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 1;
                                            cell.BorderWidthLeft = 0.5f;
                                            cell.BorderWidthRight = 0.5f;
                                            cell.BorderWidthTop = 0;
                                            cell.BorderWidthBottom = 0;
                                            cell.Colspan = 1;
                                            table.AddCell(cell);
                                            if (cellText == "")
                                            {
                                                cellText = "0";
                                            }
                                            noofshifts += Convert.ToDecimal(cellText);
                                        }
                                        if (j == 4)
                                        {
                                            // DropDownList label1 = (DropDownList)(gvClientBilling.Rows[rowIndex].FindControl("ddldutytype"));
                                            TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("lblpayrate"));

                                            //  cellText = label1.SelectedItem.Text;
                                            //create a new cell with column value
                                            if (label1.Text == "0")
                                            {
                                                cellText = "";
                                            }
                                            else
                                            {
                                                cellText = label1.Text;
                                            }

                                            //create a new cell with column value
                                            cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 1;
                                            //cell.Colspan = 2;
                                            cell.BorderWidthLeft = 0.5f;
                                            cell.BorderWidthTop = 0;
                                            cell.BorderWidthRight = 0.5f;
                                            cell.BorderWidthBottom = 0;
                                            cell.Colspan = 1;
                                            table.AddCell(cell);
                                        }



                                        if (j == 5)
                                        {
                                            TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("lblda"));
                                            cellText = label1.Text;

                                            if (cellText == "0")
                                            {
                                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.Colspan = 1;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 1.5f;
                                                cell.BorderWidthLeft = 0.5f;
                                                cell.BorderWidthBottom = 0;
                                                table.AddCell(cell);
                                            }
                                            else
                                            {
                                                cell = new PdfPCell(new Phrase(Convert.ToDecimal(cellText).ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.Colspan = 1;
                                                cell.BorderWidthRight = 1.5f;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthLeft = 0.5f;
                                                cell.BorderWidthBottom = 0;
                                                table.AddCell(cell);
                                            }
                                            //create a new cell with column value

                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region for space
                    PdfPCell Cellempty = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    Cellempty.HorizontalAlignment = 2;
                    Cellempty.Colspan = 1;
                    Cellempty.BorderWidthTop = 0;
                    Cellempty.BorderWidthRight = 0.5f;
                    Cellempty.BorderWidthLeft = 1.5f;
                    Cellempty.BorderWidthBottom = 0;
                    if (Status == "ACT")
                    {
                        Cellempty.MinimumHeight = 8;
                    }
                    else
                    {
                        Cellempty.MinimumHeight = 14;
                    }
                    PdfPCell Cellempty1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    Cellempty1.HorizontalAlignment = 2;
                    Cellempty1.Colspan = 1;
                    Cellempty1.BorderWidthTop = 0;
                    Cellempty1.BorderWidthRight = 0.5f;
                    Cellempty1.BorderWidthLeft = 0.5f;
                    Cellempty1.BorderWidthBottom = 0;

                    PdfPCell Cellempty2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    Cellempty2.HorizontalAlignment = 2;
                    Cellempty2.Colspan = 1;
                    Cellempty2.BorderWidthTop = 0;
                    Cellempty2.BorderWidthRight = 0.5f;
                    Cellempty2.BorderWidthLeft = 0.5f;
                    Cellempty2.BorderWidthBottom = 0;

                    PdfPCell Cellempty3 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    Cellempty3.HorizontalAlignment = 2;
                    Cellempty3.Colspan = 1;
                    Cellempty3.BorderWidthTop = 0;
                    Cellempty3.BorderWidthRight = 0.5f;
                    Cellempty3.BorderWidthLeft = 0.5f;
                    Cellempty3.BorderWidthBottom = 0;

                    PdfPCell Cellempty4 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    Cellempty4.HorizontalAlignment = 2;
                    Cellempty4.Colspan = 1;
                    Cellempty4.BorderWidthTop = 0;
                    Cellempty4.BorderWidthRight = 0.5f;
                    Cellempty4.BorderWidthLeft = 0.5f;
                    Cellempty4.BorderWidthBottom = 0;

                    PdfPCell Cellempty5 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    Cellempty5.HorizontalAlignment = 2;
                    Cellempty5.Colspan = 1;
                    Cellempty5.BorderWidthTop = 0;
                    Cellempty5.BorderWidthRight = 1.5f;
                    Cellempty5.BorderWidthLeft = 0.5f;
                    Cellempty5.BorderWidthBottom = 0;

                    if (gvClientBilling.Rows.Count == 1)
                    {
                        #region For cell count

                        //1


                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //4
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //5
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //6
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //7
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //8
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //9
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //10
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //11
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //12
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //13
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //14
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //15
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //16
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //17
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //18
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //19
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);


                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 2)
                    {
                        #region For cell count

                        //1
                        //table.AddCell(Cellempty);
                        //table.AddCell(Cellempty1);
                        //table.AddCell(Cellempty2);
                        //table.AddCell(Cellempty3);
                        //table.AddCell(Cellempty4);
                        //table.AddCell(Cellempty5);
                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //4
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //5
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //6
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //7
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //8
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //9
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //10
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //11
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //12
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //13
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //14
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //15
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //16
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //17
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //18
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);



                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 3)
                    {
                        #region For cell count
                        //1
                        //table.AddCell(Cellempty);
                        //table.AddCell(Cellempty1);
                        //table.AddCell(Cellempty2);
                        //table.AddCell(Cellempty3);
                        //table.AddCell(Cellempty4);
                        //table.AddCell(Cellempty5);
                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //4
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //5
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //6
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //7
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //8
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //9
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //10
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //11
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //12
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //13
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //14
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //15
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //16
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //17
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 4)
                    {
                        #region For cell count
                        //1
                        //table.AddCell(Cellempty);
                        //table.AddCell(Cellempty1);
                        //table.AddCell(Cellempty2);
                        //table.AddCell(Cellempty3);
                        //table.AddCell(Cellempty4);
                        //table.AddCell(Cellempty5);
                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //4
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //5
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //6
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //7
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //8
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //9
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //10
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //11
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //12
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //13
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //14
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //15
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //16
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 5)
                    {
                        #region For cell count
                        //1
                        //table.AddCell(Cellempty);
                        //table.AddCell(Cellempty1);
                        //table.AddCell(Cellempty2);
                        //table.AddCell(Cellempty3);
                        //table.AddCell(Cellempty4);
                        //table.AddCell(Cellempty5);
                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //4
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //5
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //6
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //7
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //8
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //9
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //10
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //11
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //12
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //13
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //14
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //15
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);

                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 6)
                    {
                        #region For cell count
                        //1
                        //table.AddCell(Cellempty);
                        //table.AddCell(Cellempty1);
                        //table.AddCell(Cellempty2);
                        //table.AddCell(Cellempty3);
                        //table.AddCell(Cellempty4);
                        //table.AddCell(Cellempty5);
                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //4
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //5
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //6
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //7
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //8
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //9
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //10
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //11
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //12
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //13
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //14
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);

                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 7)
                    {
                        #region For cell count
                        //1
                        //table.AddCell(Cellempty);
                        //table.AddCell(Cellempty1);
                        //table.AddCell(Cellempty2);
                        //table.AddCell(Cellempty3);
                        //table.AddCell(Cellempty4);
                        //table.AddCell(Cellempty5);
                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //4
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //5
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //6
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //7
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //8
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //9
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //10
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //11
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //12
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //13
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);

                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 8)
                    {
                        #region For cell count
                        //1
                        //table.AddCell(Cellempty);
                        //table.AddCell(Cellempty1);
                        //table.AddCell(Cellempty2);
                        //table.AddCell(Cellempty3);
                        //table.AddCell(Cellempty4);
                        //table.AddCell(Cellempty5);
                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //4
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //5
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //6
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //7
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //8
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //9
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //10
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //11
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //12
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);

                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 9)
                    {
                        #region For cell count
                        //1
                        //table.AddCell(Cellempty);
                        //table.AddCell(Cellempty1);
                        //table.AddCell(Cellempty2);
                        //table.AddCell(Cellempty3);
                        //table.AddCell(Cellempty4);
                        //table.AddCell(Cellempty5);
                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //4
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //5
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //6
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //7
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //8
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //9
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //10
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //11
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);

                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 10)
                    {
                        #region For cell count
                        //1
                        //table.AddCell(Cellempty);
                        //table.AddCell(Cellempty1);
                        //table.AddCell(Cellempty2);
                        //table.AddCell(Cellempty3);
                        //table.AddCell(Cellempty4);
                        //table.AddCell(Cellempty5);
                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //4
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //5
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //6
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //7
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //8
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //9
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //10
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);

                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 11)
                    {
                        #region For cell count
                        //1

                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //4
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //5
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //6
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //7
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //8
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //9
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);

                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 12)
                    {
                        #region For cell count
                        //1
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //4
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //5
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //6
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //7
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //8
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);

                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 13)
                    {
                        #region For cell count
                        //1
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //4
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //5
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //6
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //7
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);

                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 14)
                    {
                        #region For cell count
                        //1
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //4
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //5
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //6
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);

                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 15)
                    {
                        #region For cell count
                        //1
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //4
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //5
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);


                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 16)
                    {
                        #region For cell count
                        //1
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //4
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);


                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 17)
                    {
                        #region For cell count
                        //1
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //3
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);


                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 18)
                    {
                        #region For cell count
                        //1
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);
                        //2
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);


                        #endregion
                    }
                    if (gvClientBilling.Rows.Count == 19)
                    {
                        #region For cell count
                        //1
                        table.AddCell(Cellempty);
                        table.AddCell(Cellempty1);
                        table.AddCell(Cellempty2);
                        table.AddCell(Cellempty3);
                        table.AddCell(Cellempty4);
                        table.AddCell(Cellempty5);

                        #endregion
                    }


                    #endregion


                    document.Add(table);

                    PdfContentByte content = writer.DirectContent;
                    //  string Fromdate = txtfromdate.Text;
                    //string Todate = txttodate.Text;

                    PdfPTable address11 = new PdfPTable(colCount);
                    address11.TotalWidth = 560f;
                    address11.LockedWidth = true;
                    float[] addreslogo11 = new float[] { 1.2f, 6.2f, 2f, 2.2f, 2f, 2.7f };
                    address11.SetWidths(addreslogo11);

                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthLeft = 1.5f;
                    cell.Colspan = 1;
                    cell.MinimumHeight = 20;
                    address11.AddCell(cell);

                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.Colspan = 1;
                    address11.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.Colspan = 1;
                    address11.AddCell(cell);
                    if (noofshifts == 0)
                    {
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0.5f;
                        cell.BorderWidthTop = 0.5f;
                        cell.BorderWidthRight = 0.5f;
                        cell.BorderWidthLeft = 0.5f;
                        cell.Colspan = 1;
                        address11.AddCell(cell);
                    }
                    else
                    {
                        cell = new PdfPCell(new Phrase(noofshifts.ToString(), FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0.5f;
                        cell.BorderWidthTop = 0.5f;
                        cell.BorderWidthRight = 0.5f;
                        cell.BorderWidthLeft = 0.5f;
                        cell.Colspan = 1;
                        address11.AddCell(cell);
                    }

                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.Colspan = 1;
                    address11.AddCell(cell);
                    cell = new PdfPCell(new Phrase(totalamount.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderWidthRight = 1.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.Colspan = 1;
                    address11.AddCell(cell);

                    PdfPTable tempTable11 = new PdfPTable(3);
                    tempTable11.TotalWidth = 323f;
                    tempTable11.LockedWidth = true;
                    float[] tempWidth21 = new float[] { 1.2f, 6.2f, 2f };//1.2f, 6.2f, 2f, 2.3f
                    tempTable11.SetWidths(tempWidth21);
                    // 1.2f, 6.2f, 2f, 2.2f, 2f, 2.7f };
                    //if (MonthSerch <= 18)
                    //{
                    //    #region
                    //    cell = new PdfPCell(new Phrase(" PLEASE NOTE : Payment Shall be made through RTGS only.", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    //    cell.HorizontalAlignment = 0;
                    //    cell.BorderWidthBottom = 0;
                    //    cell.BorderWidthTop = 0;
                    //    cell.BorderWidthRight = 0;
                    //    cell.BorderWidthLeft = 1.5f;
                    //    cell.Colspan = 4;
                    //    tempTable11.AddCell(cell);
                    //    cell = new PdfPCell(new Phrase("  A/c.No 565101000063165", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    //    cell.HorizontalAlignment = 0;
                    //    cell.BorderWidthBottom = 0;
                    //    cell.BorderWidthTop = 0;
                    //    cell.BorderWidthRight = 0;
                    //    cell.BorderWidthLeft = 1.5f;
                    //    // cell.PaddingRight = 40;
                    //    cell.Colspan = 4;
                    //    tempTable11.AddCell(cell);
                    //    cell = new PdfPCell(new Phrase("  IFSC Code : CORP0000022", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    //    cell.HorizontalAlignment = 0;
                    //    cell.BorderWidthBottom = 0;
                    //    cell.BorderWidthTop = 0;
                    //    cell.BorderWidthRight = 0;
                    //    cell.BorderWidthLeft = 1.5f;
                    //    cell.Colspan = 4;
                    //    tempTable11.AddCell(cell);
                    //    cell = new PdfPCell(new Phrase("  Bank : Corporation Bank,Bangalore City Branch,Gandhinagar", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    //    cell.HorizontalAlignment = 0;
                    //    cell.BorderWidthBottom = 0;
                    //    cell.BorderWidthTop = 0;
                    //    cell.BorderWidthRight = 0;
                    //    cell.BorderWidthLeft = 1.5f;
                    //    cell.Colspan = 4;
                    //    tempTable11.AddCell(cell);


                    //    #endregion
                    //}
                    // if (MonthSerch >= 19)
                    {
                        #region
                        cell = new PdfPCell(new Phrase(" PLEASE NOTE : Payment Shall be made through RTGS only.", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.BorderWidthBottom = 0;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = 0;
                        cell.BorderWidthLeft = 1.5f;
                        cell.Colspan = 4;
                        tempTable11.AddCell(cell);

                        cell = new PdfPCell(new Phrase("  A/c.No  : " + BankAccountNo, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.BorderWidthBottom = 0;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = 0;
                        cell.BorderWidthLeft = 1.5f;
                        // cell.PaddingRight = 40;
                        cell.Colspan = 4;
                        tempTable11.AddCell(cell);


                        cell = new PdfPCell(new Phrase("  IFSC Code : " + IFSCCode, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.BorderWidthBottom = 0;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = 0;
                        cell.BorderWidthLeft = 1.5f;
                        cell.Colspan = 4;
                        tempTable11.AddCell(cell);
                        cell = new PdfPCell(new Phrase("  " + BankName, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.BorderWidthBottom = 0;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = 0;
                        cell.BorderWidthLeft = 1.5f;
                        cell.Colspan = 4;
                        tempTable11.AddCell(cell);


                        #endregion
                    }

                    PdfPCell Chid = new PdfPCell(tempTable11);
                    Chid.Border = 0;
                    Chid.Colspan = 3;
                    Chid.HorizontalAlignment = 0;
                    address11.AddCell(Chid);

                    PdfPTable tempTable22 = new PdfPTable(3);
                    tempTable22.TotalWidth = 237f;
                    tempTable22.LockedWidth = true;
                    float[] tempWidth22 = new float[] { 2.2f, 2f, 2.7f }; ;//2.9f, 1.83f
                    tempTable22.SetWidths(tempWidth22);

                    #region
                    #region








                    if (servicecharge > 0)//bSCType == true)
                    {
                        decimal scharge = servicecharge;
                        if (scharge > 0)
                        {
                            string SCharge = "";
                            if (bSCType == false)
                            {
                                SCharge = ServiceCharge + " %";
                            }
                            else
                            {
                                SCharge = ServiceCharge;
                            }


                            PdfPCell servicechr = new PdfPCell(new Phrase("Service Charges @ " + servicechargeper + "%", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            servicechr.HorizontalAlignment = Element.ALIGN_JUSTIFIED; //0=Left, 1=Centre, 2=Right
                            servicechr.Colspan = 1;
                            servicechr.BorderWidthBottom = 0;
                            servicechr.BorderWidthLeft = 0;
                            servicechr.BorderWidthTop = 0;
                            servicechr.BorderWidthRight = 0.5f;
                            servicechr.SetLeading(0, 1.3f);
                            tempTable22.AddCell(servicechr);

                            PdfPCell srvvalue = new PdfPCell(new Phrase(servicecharge.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            srvvalue.HorizontalAlignment = Element.ALIGN_JUSTIFIED; //0=Left, 1=Centre, 2=Right
                            srvvalue.Colspan = 1;
                            srvvalue.BorderWidthBottom = 0;
                            srvvalue.BorderWidthLeft = 0;
                            srvvalue.BorderWidthTop = 0;
                            srvvalue.BorderWidthRight = 0;
                            srvvalue.SetLeading(0, 1.3f);
                            tempTable22.AddCell(srvvalue);

                        }
                    }

                    #endregion

                    //if (!bIncludeST)
                    {

                        string scpercent = "";
                        if (bST75 == true)
                        {
                            scpercent = "3";
                        }
                        else
                        {
                            scpercent = SCPersent;
                        }

                        if (servicetax > 0)
                        {

                            PdfPCell srvtax = new PdfPCell(new Phrase("Service Tax @ " + servicetaxprc + "%", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            srvtax.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            srvtax.Colspan = 2;
                            srvtax.BorderWidthBottom = 0;
                            srvtax.BorderWidthLeft = 0;
                            srvtax.BorderWidthTop = 0;
                            srvtax.BorderWidthRight = 0.5f;
                            // srvtax.SetLeading(0, 1.3f);
                            tempTable22.AddCell(srvtax);

                            PdfPCell celldd4 = new PdfPCell(new Phrase(servicetax.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldd4.BorderWidthBottom = 0;
                            celldd4.Colspan = 1;
                            celldd4.BorderWidthLeft = 0.5f;
                            celldd4.BorderWidthTop = 0;
                            celldd4.BorderWidthRight = 1.5f;
                            tempTable22.AddCell(celldd4);

                        }

                        if (sbcess > 0)
                        {

                            string SBCESSPresent = DtTaxes.Rows[0]["SBCess"].ToString();
                            PdfPCell celldd2 = new PdfPCell(new Phrase("Swachh Bharat Cess @ " + sbcessprc + "%", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldd2.Colspan = 2;
                            celldd2.BorderWidthBottom = 0;
                            celldd2.BorderWidthLeft = 0;
                            celldd2.BorderWidthTop = 0;
                            celldd2.BorderWidthRight = 0.5f;
                            tempTable22.AddCell(celldd2);


                            PdfPCell celldd4 = new PdfPCell(new Phrase(sbcess.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldd4.BorderWidthBottom = 0;
                            celldd4.BorderWidthLeft = 0.5f;
                            celldd2.Colspan = 1;
                            celldd4.BorderWidthTop = 0;
                            celldd4.BorderWidthRight = 1.5f;
                            tempTable22.AddCell(celldd4);

                        }



                        if (kkcess > 0)
                        {


                            string KKCESSPresent = DtTaxes.Rows[0]["KKCess"].ToString();
                            PdfPCell Cellmtcesskk1 = new PdfPCell(new Phrase("Krishi Kalyan Cess @ " + kkcessprc + "%", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            Cellmtcesskk1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            Cellmtcesskk1.Colspan = 2;
                            Cellmtcesskk1.BorderWidthBottom = 0;
                            Cellmtcesskk1.BorderWidthLeft = 0;
                            Cellmtcesskk1.BorderWidthTop = 0;
                            Cellmtcesskk1.BorderWidthRight = 0.5f;
                            tempTable22.AddCell(Cellmtcesskk1);

                            PdfPCell Cellmtcesskk2 = new PdfPCell(new Phrase(kkcess.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            Cellmtcesskk2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            Cellmtcesskk2.BorderWidthBottom = 0;
                            Cellmtcesskk2.Colspan = 1;
                            Cellmtcesskk2.BorderWidthLeft = 0.5f;
                            Cellmtcesskk2.BorderWidthTop = 0;
                            Cellmtcesskk2.BorderWidthRight = 1.5f;
                            tempTable22.AddCell(Cellmtcesskk2);

                        }
                        #region for GST as on 17-6-2017

                        if (CGST > 0)
                        {


                            PdfPCell CellCGST = new PdfPCell(new Phrase(CGSTAlias + " @ " + CGSTPrc + "%", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                            CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellCGST.Colspan = 2;
                            CellCGST.BorderWidthBottom = 0;
                            CellCGST.BorderWidthLeft = 0;
                            CellCGST.BorderWidthTop = 0;
                            CellCGST.BorderWidthRight = .5f;
                            // CellCGST.BorderColor = BaseColor.GRAY;
                            tempTable22.AddCell(CellCGST);

                            PdfPCell CellCGSTAmt = new PdfPCell(new Phrase(CGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                            CellCGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellCGSTAmt.BorderWidthBottom = 0;
                            CellCGSTAmt.BorderWidthLeft = 0.5f;
                            CellCGSTAmt.BorderWidthTop = 0;
                            CellCGSTAmt.BorderWidthRight = 1.5f;
                            // CellCGSTAmt.BorderColor = BaseColor.GRAY;
                            tempTable22.AddCell(CellCGSTAmt);

                        }


                        if (SGST > 0)
                        {



                            PdfPCell CellSGST = new PdfPCell(new Phrase(SGSTAlias + " @ " + SGSTPrc + "%", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                            CellSGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellSGST.Colspan = 2;
                            CellSGST.BorderWidthBottom = 0;
                            CellSGST.BorderWidthLeft = 0f;
                            CellSGST.BorderWidthTop = 0;
                            CellSGST.BorderWidthRight = 0.5f;
                            tempTable22.AddCell(CellSGST);

                            PdfPCell CellSGSTAmt = new PdfPCell(new Phrase(SGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                            CellSGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellSGSTAmt.BorderWidthBottom = 0;
                            CellSGSTAmt.BorderWidthLeft = 0.5f;
                            CellSGSTAmt.BorderWidthTop = 0;
                            CellSGSTAmt.BorderWidthRight = 1.5f;
                            tempTable22.AddCell(CellSGSTAmt);

                        }

                        if (IGST > 0)
                        {

                            PdfPCell CellIGST = new PdfPCell(new Phrase(IGSTAlias + " @ " + IGSTPrc + "%", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                            CellIGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellIGST.Colspan = 2;
                            CellIGST.BorderWidthBottom = 0;
                            CellIGST.BorderWidthLeft = 0;
                            CellIGST.BorderWidthTop = 0;
                            CellIGST.BorderWidthRight = 0.5f;
                            tempTable22.AddCell(CellIGST);

                            PdfPCell CellIGSTAmt = new PdfPCell(new Phrase(IGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                            CellIGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellIGSTAmt.BorderWidthBottom = 0;
                            CellIGSTAmt.BorderWidthLeft = 0.5f;
                            CellIGSTAmt.BorderWidthTop = 0;
                            CellIGSTAmt.BorderWidthRight = 1.5f;
                            tempTable22.AddCell(CellIGSTAmt);

                        }
                        decimal RoundOffvalue = 0;
                        RoundOffvalue = Convert.ToDecimal(Grandtotal) - Convert.ToDecimal(totalamount + SGST + CGST + IGST);


                        if (RoundOffvalue != 0)
                        {
                            PdfPCell CeelRoundOffvalue = new PdfPCell(new Phrase("Round Off", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            CeelRoundOffvalue.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CeelRoundOffvalue.Colspan = 2;
                            CeelRoundOffvalue.BorderWidthBottom = 0;
                            CeelRoundOffvalue.BorderWidthLeft = 0;
                            CeelRoundOffvalue.BorderWidthTop = 0;
                            CeelRoundOffvalue.BorderWidthRight = 0.5f;
                            CeelRoundOffvalue.PaddingTop = 5;
                            tempTable22.AddCell(CeelRoundOffvalue);

                            CeelRoundOffvalue = new PdfPCell(new Phrase(RoundOffvalue.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            CeelRoundOffvalue.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CeelRoundOffvalue.BorderWidthBottom = 0;
                            CeelRoundOffvalue.BorderWidthLeft = 0.5f;
                            CeelRoundOffvalue.BorderWidthTop = 0;
                            CeelRoundOffvalue.BorderWidthRight = 1.5f;
                            CeelRoundOffvalue.PaddingTop = 5;
                            tempTable22.AddCell(CeelRoundOffvalue);

                        }

                        if (Cess1 > 0)
                        {

                            PdfPCell CellCess1 = new PdfPCell(new Phrase(Cess1Alias + " @ " + Cess1Prc + "%", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                            CellCess1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellCess1.Colspan = 2;
                            CellCess1.BorderWidthBottom = 0;
                            CellCess1.BorderWidthLeft = 0f;
                            CellCess1.BorderWidthTop = 0;
                            CellCess1.BorderWidthRight = 0.5f;
                            tempTable22.AddCell(CellCess1);

                            PdfPCell CellCess1Amt = new PdfPCell(new Phrase(Cess1.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                            CellCess1Amt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellCess1Amt.BorderWidthBottom = 0;
                            CellCess1Amt.BorderWidthLeft = 0.5f;
                            CellCess1Amt.BorderWidthTop = 0;
                            CellCess1Amt.BorderWidthRight = 1.5f;
                            tempTable22.AddCell(CellCess1Amt);

                        }


                        if (Cess2 > 0)
                        {

                            PdfPCell CellCess2 = new PdfPCell(new Phrase(Cess2Alias + " @ " + Cess2Prc + "%", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                            CellCess2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellCess2.Colspan = 2;
                            CellCess2.BorderWidthBottom = 0;
                            CellCess2.BorderWidthLeft = 0f;
                            CellCess2.BorderWidthTop = 0;
                            CellCess2.BorderWidthRight = 0.5f;
                            tempTable22.AddCell(CellCess2);

                            PdfPCell CellCess2Amt = new PdfPCell(new Phrase(Cess2.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                            CellCess2Amt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellCess2Amt.BorderWidthBottom = 0;
                            CellCess2Amt.BorderWidthLeft = 0.5f;
                            CellCess2Amt.BorderWidthTop = 0;
                            CellCess2Amt.BorderWidthRight = 1.5f;
                            tempTable22.AddCell(CellCess2Amt);

                        }

                        #endregion for GST 

                        if (cess > 0)
                        {

                            PdfPCell cessblnk = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            cessblnk.HorizontalAlignment = Element.ALIGN_JUSTIFIED; //0=Left, 1=Centre, 2=Right
                            cessblnk.Colspan = 2;
                            cessblnk.BorderWidthBottom = 0.5f;
                            cessblnk.BorderWidthLeft = 0.5f;
                            cessblnk.BorderWidthTop = 0;
                            cessblnk.BorderWidthRight = 0f;
                            cessblnk.SetLeading(0, 1.3f);
                            tempTable22.AddCell(cessblnk);

                            string CESSPresent = DtTaxes.Rows[0]["Cess"].ToString();
                            PdfPCell celldd2 = new PdfPCell(new Phrase("CESS @ " + CESSPresent + "%", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldd2.Colspan = 2;
                            celldd2.BorderWidthBottom = 0.5f;
                            celldd2.BorderWidthLeft = 0.5f;
                            celldd2.BorderWidthTop = 0;
                            celldd2.BorderWidthRight = 0f;
                            //celldd2.PaddingBottom = 5;
                            //celldd2.PaddingTop = 5;
                            tempTable22.AddCell(celldd2);


                            PdfPCell celldd4 = new PdfPCell(new Phrase(cess.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                            celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldd4.BorderWidthBottom = 0.5f;
                            celldd4.Colspan = 1;
                            celldd4.BorderWidthLeft = 0.5f;
                            celldd4.BorderWidthTop = 0;
                            celldd4.BorderWidthRight = 0.5f;
                            //celldd4.PaddingBottom = 5;
                            //celldd4.PaddingTop = 5;
                            tempTable22.AddCell(celldd4);

                        }

                        if (shecess > 0)
                        {

                            PdfPCell shecessblnk = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            shecessblnk.HorizontalAlignment = Element.ALIGN_JUSTIFIED; //0=Left, 1=Centre, 2=Right
                            shecessblnk.Colspan = 2;
                            shecessblnk.BorderWidthBottom = 0.5f;
                            shecessblnk.BorderWidthLeft = 0.5f;
                            shecessblnk.BorderWidthTop = 0;
                            shecessblnk.BorderWidthRight = 0f;
                            shecessblnk.SetLeading(0, 1.3f);
                            tempTable22.AddCell(shecessblnk);


                            string SHECESSPresent = DtTaxes.Rows[0]["shecess"].ToString();
                            PdfPCell celldf2 = new PdfPCell(new Phrase("S&H Ed.CESS @ " + SHECESSPresent + "%", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            celldf2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldf2.Colspan = 2;
                            celldf2.BorderWidthBottom = 0.5f;
                            celldf2.BorderWidthLeft = 0.5f;
                            celldf2.BorderWidthTop = 0;
                            celldf2.BorderWidthRight = 0f;
                            //celldf2.PaddingBottom = 5;
                            //celldf2.PaddingTop = 5;
                            tempTable22.AddCell(celldf2);


                            PdfPCell celldf4 = new PdfPCell(new Phrase(shecess.ToString("0.00"),
                                FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                            celldf4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldf4.BorderWidthBottom = 0.5f;
                            celldf4.Colspan = 1;
                            celldf4.BorderWidthLeft = 0.5f;
                            celldf4.BorderWidthTop = 0;
                            celldf4.BorderWidthRight = 0.5f;
                            ////celldf4.PaddingBottom = 5;
                            //celldf4.PaddingTop = 5;
                            //celldf4.BorderColor = BaseColor.LIGHT_GRAY;
                            tempTable22.AddCell(celldf4);
                        }
                    }
                    #endregion

                    PdfPCell Chids = new PdfPCell(tempTable22);
                    Chids.Border = 0;
                    Chids.Colspan = 3;
                    Chids.HorizontalAlignment = 0;
                    address11.AddCell(Chids);

                    document.Add(address11);
                    #region footer
                    PdfPTable addrssf = new PdfPTable(colCount);
                    addrssf.TotalWidth = 560f;
                    addrssf.LockedWidth = true;
                    float[] addr = new float[] { 1.2f, 6.2f, 2f, 2.2f, 2f, 2.7f };
                    addrssf.SetWidths(addr);


                    PdfPTable cellt = new PdfPTable(3);
                    cellt.TotalWidth = 323f;
                    cellt.LockedWidth = true;
                    float[] widthcell = new float[] { 1.2f, 6.2f, 2f };//1.2f, 6.2f, 2f, 2.3f
                    cellt.SetWidths(widthcell);
                    #region

                    cell = new PdfPCell(new Phrase(" Amount In Words: ", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthLeft = 1.5f;
                    cell.Colspan = 3;
                    cellt.AddCell(cell);

                    //string Amountinwords = NumberToEnglish.Instance.changeNumericToWords(Grandtotal.ToString());
                    #region Amount in words
                    string GTotal = Convert.ToDecimal(Grandtotal.ToString()).ToString("0.00");
                    string[] arr = GTotal.ToString().Split("."[0]);
                    string inwords = "";
                    string rupee = (arr[0]);
                    string paise = "";
                    string Amountinwords = "";
                    if (arr.Length == 2)
                    {
                        if (arr[1].Length > 0 && arr[1] != "00")
                        {
                            paise = (arr[1]);
                        }
                    }

                    if (paise != "0.00" && paise != "0" && paise != "")
                    {
                        int I = Int16.Parse(paise);
                        String p = NumberToEnglish.Instance.NumbersToWords(I, true);
                        paise = p;
                        rupee = NumberToEnglish.Instance.NumbersToWords(Convert.ToInt64(arr[0]), false);
                        inwords = " Rupees " + rupee + "" + paise + " Paise Only";

                    }
                    else
                    {
                        rupee = NumberToEnglish.Instance.NumbersToWords(Convert.ToInt64(arr[0]), true);
                        inwords = " Rupees " + rupee + " Only";
                    }



                    Amountinwords = inwords;
                    #endregion

                    cell = new PdfPCell(new Phrase("  " + Amountinwords.Trim() + "", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthLeft = 1.5f;
                    cell.Colspan = 3;
                    cellt.AddCell(cell);
                    #endregion
                    PdfPCell Chid1 = new PdfPCell(cellt);
                    Chid1.Border = 0;
                    Chid1.Colspan = 3;
                    Chid1.HorizontalAlignment = 0;
                    addrssf.AddCell(Chid1);

                    PdfPTable celltf = new PdfPTable(3);
                    celltf.TotalWidth = 237f;
                    celltf.LockedWidth = true;
                    float[] Dfv = new float[] { 2.2f, 2f, 2.7f }; ;//2.9f, 1.83f
                    celltf.SetWidths(Dfv);

                    #region
                    cell = new PdfPCell(new Phrase("Grand  Total", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.Colspan = 2;
                    celltf.AddCell(cell);
                    cell = new PdfPCell(new Phrase(Grandtotal.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderWidthRight = 1.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.Colspan = 1;
                    celltf.AddCell(cell);

                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.Colspan = 2;
                    // cell.MinimumHeight = 30;
                    celltf.AddCell(cell);
                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthRight = 1.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.Colspan = 1;
                    celltf.AddCell(cell);

                    #endregion

                    PdfPCell Chid2 = new PdfPCell(celltf);
                    Chid2.Border = 0;
                    Chid2.Colspan = 3;
                    Chid2.HorizontalAlignment = 0;
                    addrssf.AddCell(Chid2);

                    document.Add(addrssf);

                    PdfPTable Addterms = new PdfPTable(colCount);
                    Addterms.TotalWidth = 560f;
                    Addterms.LockedWidth = true;
                    float[] widthrerms = new float[] { 1.2f, 6.2f, 2f, 2.2f, 2f, 2.7f };
                    Addterms.SetWidths(widthrerms);


                    PdfPTable Childterms = new PdfPTable(3);
                    Childterms.TotalWidth = 323f;
                    Childterms.LockedWidth = true;
                    float[] Celters = new float[] { 1.2f, 6.2f, 2f };
                    Childterms.SetWidths(Celters);




                    cell = new PdfPCell(new Phrase(" Payment Terms \n\n", FontFactory.GetFont(FontStyle, 9, Font.UNDERLINE | Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthLeft = 1.5f;
                    cell.Colspan = 3;
                    Childterms.AddCell(cell);
                    cell = new PdfPCell(new Phrase("  1.  Payment to be made as per Agreement Terms.\n\n", FontFactory.GetFont(FontStyle, 9, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthLeft = 1.5f;
                    cell.Colspan = 3;
                    Childterms.AddCell(cell);
                    cell = new PdfPCell(new Phrase("  2.  An interest of 24% p.a.shall be levied from the due date onwards.\n\n", FontFactory.GetFont(FontStyle, 9, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthLeft = 1.5f;
                    cell.Colspan = 3;
                    Childterms.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("  3.  Please Pay this bill by Cheque / DD only favour of", FontFactory.GetFont(FontStyle, 9, Font.BOLD, BaseColor.BLACK)));
                    //cell.HorizontalAlignment = 0;
                    //cell.BorderWidthBottom = 0;
                    //cell.BorderWidthTop = 0;
                    //cell.BorderWidthRight = 0.5f;
                    //cell.BorderWidthLeft = 1.5f;
                    //cell.Colspan = 3;
                    //Childterms.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("       " + companyName+"\n\n", FontFactory.GetFont(FontStyle, 9, Font.BOLD, BaseColor.BLACK)));
                    //cell.HorizontalAlignment = 0;
                    //cell.BorderWidthBottom = 0;
                    //cell.BorderWidthTop = 0;
                    //cell.BorderWidthRight = 0.5f;
                    //cell.BorderWidthLeft = 1.5f;
                    //cell.Colspan = 3;
                    //Childterms.AddCell(cell);
                    cell = new PdfPCell(new Phrase("  3.  All disputes Subjects to Bangalore Jurisdiction.\n\n", FontFactory.GetFont(FontStyle, 9, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.BorderWidthBottom = 1.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthLeft = 1.5f;
                    cell.Colspan = 3;
                    Childterms.AddCell(cell);


                    PdfPCell Chid3 = new PdfPCell(Childterms);
                    Chid3.Border = 0;
                    Chid3.Colspan = 3;
                    Chid3.HorizontalAlignment = 0;
                    Addterms.AddCell(Chid3);



                    PdfPTable chilk = new PdfPTable(3);
                    chilk.TotalWidth = 237f;
                    chilk.LockedWidth = true;
                    float[] Celterss = new float[] { 2.2f, 2f, 2.7f };
                    chilk.SetWidths(Celterss);

                    cell = new PdfPCell(new Phrase("\nFor " + companyName, FontFactory.GetFont(FontStyle, 8, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderWidthRight = 1.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.Colspan = 3;
                    chilk.AddCell(cell);

                    cell = new PdfPCell(new Phrase("\n\n\n\nAuthorised Signatory", FontFactory.GetFont(FontStyle, 9, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    cell.BorderWidthBottom = 1.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthRight = 1.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.Colspan = 3;
                    chilk.AddCell(cell);


                    PdfPCell Chid4 = new PdfPCell(chilk);
                    Chid4.Border = 0;
                    Chid4.Colspan = 3;
                    Chid4.HorizontalAlignment = 0;
                    Addterms.AddCell(Chid4);

                    document.Add(Addterms);
                    #endregion

                    //Childterms.WriteSelectedRows(0, -1, document.LeftMargin - 19, document.BottomMargin + 122, content);
                    //chilk.WriteSelectedRows(0, -1, document.LeftMargin + 261, document.BottomMargin + 122, content);
                    //Addterms.WriteSelectedRows(0, -1, document.RightMargin - 19, document.BottomMargin + 122, content);

                    //cellt.WriteSelectedRows(0, -1, document.LeftMargin - 19, document.BottomMargin + 166, content);
                    //celltf.WriteSelectedRows(0, -1, document.LeftMargin + 301, document.BottomMargin + 166, content);
                    //addrssf.WriteSelectedRows(0, -1, document.RightMargin - 19, document.BottomMargin + 166, content);
                    //tempTable11.WriteSelectedRows(0, -1, document.LeftMargin - 19, document.BottomMargin + 224, content);
                    //tempTable22.WriteSelectedRows(0, -1, document.LeftMargin + 301, document.BottomMargin + 224, content);
                    //address11.WriteSelectedRows(0, -1, document.RightMargin - 19, document.BottomMargin + 244, content);

                    //Rectangle rectangle = new Rectangle(document.PageSize);
                    //rectangle.Left += document.LeftMargin - 7;
                    //rectangle.Right -= document.RightMargin + -25;
                    //rectangle.Top -= document.TopMargin -14;
                    //rectangle.Bottom += document.BottomMargin + 60;
                    //content.SetColorStroke(BaseColor.BLACK);

                    //content.Rectangle(rectangle.Left - 12, rectangle.Bottom - 25, rectangle.Width + 5, rectangle.Height + 5);
                    //content.Stroke();






                    #endregion


                    string filename = DisplayBillNo + "/" + ddlclientid.SelectedValue + "/" + GetMonthName().Substring(0, 3) + "/" + GetMonthOfYear() + "/Invoice.pdf";


                    document.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");
                    Response.Buffer = true;
                    Response.Clear();
                    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    Response.OutputStream.Flush();
                    Response.End();
                }

                catch (Exception ex)
                {
                    //LblResult.Text = ex.Message;
                }
            }
            else
            {
                // LblResult.Text = "There is no bill generated for selected client";
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' There is no bill generated for selected client ');", true);

            }
        }

        protected void btnSendMail_Click(object sender, EventArgs e)
        {
            int month = GetMonthBasedOnSelectionDateorMonth();
            string qry = "";
            int status = 0;

            bool TrueOrFalse = SendMail();
            if ((TrueOrFalse == true))
            {
                if (ddlType.SelectedIndex == 0)
                {
                    qry = "update unitbill set MailStatus=1 where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "'";
                    status = config.ExecuteNonQueryWithQueryAsync(qry).Result;
                }
                else
                {
                    qry = "update munitbill set MailStatus=1 where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "' and BillNo='" + ddlMBBillnos.SelectedValue + "'";
                    status = config.ExecuteNonQueryWithQueryAsync(qry).Result;
                }

                ViewSendMail();

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Mail was not sent');", true);
                return;
            }
        }

        public void ViewSendMail()
        {
            int month = GetMonthBasedOnSelectionDateorMonth();
            DataTable dt = null;
            string qry = "";


            if (ddlType.SelectedIndex == 0)
            {
                qry = "select Grandtotal,billno,isnull(mailstatus,0) as mailstatus from unitbill where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "'";
                dt = config.ExecuteReaderWithQueryAsync(qry).Result;
            }
            else
            {
                qry = "select Grandtotal,billno,isnull(mailstatus,0) as mailstatus from munitbill where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "' and billno='" + ddlMBBillnos.SelectedValue + "'";
                dt = config.ExecuteReaderWithQueryAsync(qry).Result;
            }

            bool MailStatus = false;

            if (dt.Rows.Count > 0)
            {
                btnSendMail.Visible = false;

                MailStatus = bool.Parse(dt.Rows[0]["mailstatus"].ToString());
                if (MailStatus)
                {
                    btnSendMail.Enabled = false;
                }
                else
                {
                    btnSendMail.Enabled = true;
                }
            }
            else
            {
                btnSendMail.Visible = false;
            }
        }

        private bool SendMail()
        {
            int month = 0;
            month = GetMonthBasedOnSelectionDateorMonth();

            try
            {
                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.A4);


                string SelectBillNo = "Select RIGHT(billno,5) as DisplayBillNo from MUnitbill where month='" + month + "' and unitid='" + ddlclientid.SelectedValue + "' and BillNo='" + ddlMBBillnos.SelectedValue + "'";
                DataTable DtBilling = SqlHelper.Instance.GetTableByQuery(SelectBillNo);

                string DisplayBillNo = "";
                if (DtBilling.Rows.Count > 0)
                {

                    DisplayBillNo = DtBilling.Rows[0]["DisplayBillNo"].ToString();
                }


                string ClientsQry = "select ClientEmail,EmailCC,Clientshortname from clients where clientid='" + ddlclientid.SelectedValue + "' ";
                DataTable dtClient = config.ExecuteReaderWithQueryAsync(ClientsQry).Result;

                string ClientEmail = "";
                string EmailCC = "";
                string ShortName = "";

                if (dtClient.Rows.Count > 0)
                {
                    ClientEmail = dtClient.Rows[0]["ClientEmail"].ToString();
                    EmailCC = dtClient.Rows[0]["EmailCC"].ToString();
                    ShortName = dtClient.Rows[0]["Clientshortname"].ToString();

                }



                #region Begin Email sending as on [03-10-2015]


                byte[] bytes = ms.ToArray();
                ms.Close();



                if (ClientEmail.Length > 0)
                {

                    MailMessage mm = new MailMessage();
                    mm.From = new MailAddress("yeddlaanilreddy@gmail.com");
                    string[] multi = ClientEmail.Split(',');

                    foreach (string mailid in multi)
                    {
                        mm.To.Add(new MailAddress(mailid));
                    }
                    string[] multiCC = EmailCC.Split(',');

                    if (EmailCC.Length > 0)
                    {

                        foreach (string mailid in multiCC)
                        {
                            mm.CC.Add(new MailAddress(mailid));
                        }
                    }

                    mm.Subject = "" + ShortName + ": Invoice for the month of " + GetMonthName() + " - " + GetMonthOfYear() + ".";
                    mm.Body = "Dear Sir/Madam,  <br><br> Please find the attached soft-copy of the Invoice for our services undertaken in the month of <B>" + GetMonthName() + " - " + GetMonthOfYear() + "</B> " +
                         "<br><br><I>Please note that the payment has to be made in favor of </I> <B>''DARKS SECURITY CONSULTANT PRIVATE LIMITED''</B> <I>  or by NEFT under the following bank details: </I> <br>" +
                         "<br><B>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;1)A/c Name&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: DARKS SECURITY CONSULTANT PRIVATE LIMITED</B> " +
                         "<br><B>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Bank Name&nbsp;&nbsp;&nbsp;: ICICI BANK</B>" +
                         "<br><B>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;A/c No.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: 031205004941</B>" +
                         "<br><B>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Branch&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: Gorky Terrace Branch,Kolkata</B> " +
                         "<br><B>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;IFSC&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: ICIC0000312</B> " +
                         "<br><br>" +
                         "<br><I><font color='red'>Note: Please remember to mention your Company Name/Invoice No. in the remarks field  for payments made through NEFT.</font></I>" +
                         "<br><br>Thanking you very much for entrusting us with our services, we wish for a long and continued relation between us.<br>" +
                         "<br>. For Admin related please email darks_hm@darks.in" +

                     " <br><br><I><font size=3><B>Accounts Team, </B></font></I><br><br><font size=3><B> DARKS SECURITY CONSULTANT PRIVATE LIMITED</B></font> <br><B> Ph : +91 - 033 - 2289 - 3554/55</B><br><B> Website : www.darksmanpower.com</B><br><B>Address : MANAGEMENT CONSULTANTS, DETECTIVE SERVICES,HOUSE KEEPING & MAINTENANCE,</B><br><B>CIN No :- U749200R1999PTC005980</B><br><B>Reg.Office: 8C, JAY DURGA NAGAR, BHUBANESWAR - 751 006</B><br><B><br>Corporate Office: 224A, A.J.C.BOSE ROAD, KOLKATA - 700 017</B>";

                    string filename = ddlCname.SelectedItem + "-" + DisplayBillNo + "/" + ddlclientid.SelectedValue + "/" + GetMonthName().Substring(0, 3) + "/" + GetMonthOfYear() + "/Invoice.pdf";

                    mm.Attachments.Add(new Attachment(new MemoryStream(bytes), filename));
                    mm.IsBodyHtml = true;
                    //mm.AlternateViews.Add(Mail_Body());
                    SmtpClient smtp = new SmtpClient();
                    smtp.Port = 587;
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = true;
                    NetworkCredential NetworkCred = new NetworkCredential();
                    NetworkCred.UserName = "yeddlaanilreddy@gmail.com";
                    NetworkCred.Password = "pspkalyan";
                    smtp.Credentials = NetworkCred;
                    smtp.Send(mm);


                    return true;


                }
                else
                {
                    return false;
                }

                #endregion End Email sending as on [03-10-2015]



            }

            catch (Exception ex)
            {
                return false;
            }

        }

        public string GetDate()
        {
            var month = 0;
            month = GetMonthBasedOnSelectionDateorMonth();

            int monthval = 0;
            int yearval = 0;

            if (month.ToString().Length == 3)
            {
                monthval = int.Parse(month.ToString().Substring(0, 1));
                yearval = int.Parse("20" + month.ToString().Substring(1, 2));

            }


            if (month.ToString().Length == 4)
            {
                monthval = int.Parse(month.ToString().Substring(0, 2));
                yearval = int.Parse("20" + month.ToString().Substring(2, 2));

            }

            int Selectdays = System.DateTime.DaysInMonth(yearval, monthval);

            string BilldateCheck = Selectdays.ToString() + "/" + monthval.ToString() + "/" + yearval.ToString();

            return BilldateCheck;
        }

        #region for IRN and credit note IRN

        public void displayExtraData()
        {
            int month = GetMonthBasedOnSelectionDateorMonth();

            string monthnew = "";

            if (month != 0)
            {
                if (month.ToString().Length == 3)
                {
                    monthnew = month.ToString().Substring(1, 2) + 0 + month.ToString().Substring(0, 1);
                }
                else
                {
                    monthnew = month.ToString().Substring(2, 2) + month.ToString().Substring(0, 2);
                }


                if (int.Parse(monthnew) < 2204)
                {
                    checkExtraData.Visible = true;


                }
                else
                {
                    checkExtraData.Visible = false;

                }
            }
            else
            {
                checkExtraData.Visible = true;

            }
        }
        protected void btnGenerateIRN_Click(object sender, EventArgs e)
        {

            int month = GetMonthBasedOnSelectionDateorMonth();

            string SPName = "EinvGetMasterGSTDetails";
            Hashtable ht = new Hashtable();
            ht.Add("@Type", "MasterGSTCredentials");
            ht.Add("@clientid", ddlclientid.SelectedValue);
            DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            string MailID = "";
            string Username = "";
            string Password = "";
            string IP = "";
            string ClientID = "";
            string ClientSecretID = "";
            string GSTIN = "";
            string AuthToken = "";
            string TokenExpiryDate = "";


            if (dt.Rows.Count > 0)
            {
                MailID = dt.Rows[0]["EmailID"].ToString();
                Username = dt.Rows[0]["UserName"].ToString();
                Password = dt.Rows[0]["Password"].ToString();
                IP = dt.Rows[0]["IP"].ToString();
                ClientID = dt.Rows[0]["ClientID"].ToString();
                ClientSecretID = dt.Rows[0]["ClientSecretID"].ToString();
                GSTIN = dt.Rows[0]["GSTIN"].ToString();
                AuthToken = dt.Rows[0]["AuthToken"].ToString();
                TokenExpiryDate = dt.Rows[0]["TokenExpiryDate"].ToString();

            }

            IrnReqBody reqBody = new IrnReqBody();
            reqBody.ExpDtls = null;
            reqBody.EwbDtls = null;
            reqBody.DispDtls = null;
            reqBody.ShipDtls = null;


            SPName = "EinvGetBillDetails";
            ht = new Hashtable();
            ht.Add("@Type", "Unitbill");
            ht.Add("@BillType", ddlType.SelectedItem.Text);
            ht.Add("@Month", month);
            ht.Add("@Clientid", ddlclientid.SelectedValue);
            ht.Add("@BillNo", lblbillnolatest.Text.Trim());
            dt = new DataTable();
            dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            string Type = "";
            string BillNo = "";
            string BillDt = "";
            string SellerGSTIN = "";
            string SellerLglName = "";
            string SellerAddr1 = "";
            string SellerAddr2 = "";
            string SellerLocation = "";
            int SellerPIN = 0;
            string SellerStateCode = "";
            string BuyerGSTIN = "";
            string BuyerLglName = "";
            string BuyerPOS = "";
            string BuyerAddr1 = "";
            string BuyerAddr2 = "";
            string BuyerLocation = "";
            string BuyerStateCode = "";
            int BuyerPIN = 0;
            string ShipToGSTIN = "";
            string ShipToLglName = "";
            string ShipToAddr1 = "";
            string ShipToAddr2 = "";
            string ShipToLocation = "";
            string ShipToStateCode = "";
            int ShipToPIN = 0;
            double AssVal = 0;
            double CgstVal = 0;
            double SgstVal = 0;
            double IgstVal = 0;
            double OthChrg = 0;
            double TotInvVal = 0;
            double TotInvValFC = 0;
            double RoundOff = 0;
            double Discount = 0;
            string SupplyType = "B2B";

            if (dt.Rows.Count > 0)
            {
                Type = dt.Rows[0]["Type"].ToString();
                BillNo = dt.Rows[0]["BillNo"].ToString();
                BillDt = dt.Rows[0]["BillDt"].ToString();
                SellerGSTIN = dt.Rows[0]["SellerGSTIN"].ToString();
                SellerLglName = dt.Rows[0]["SellerLglName"].ToString();
                SellerAddr1 = dt.Rows[0]["SellerAddr1"].ToString();
                SellerAddr2 = dt.Rows[0]["SellerAddr2"].ToString();
                SellerLocation = dt.Rows[0]["SellerLocation"].ToString();
                SellerPIN = int.Parse(dt.Rows[0]["SellerPIN"].ToString());
                SellerStateCode = dt.Rows[0]["SellerStateCode"].ToString();
                BuyerGSTIN = dt.Rows[0]["BuyerGSTIN"].ToString();
                BuyerLglName = dt.Rows[0]["BuyerLglName"].ToString();
                BuyerPOS = dt.Rows[0]["BuyerPOS"].ToString();
                BuyerAddr1 = dt.Rows[0]["BuyerAddr1"].ToString();
                BuyerAddr2 = dt.Rows[0]["BuyerAddr2"].ToString();
                if (BuyerAddr2 == "")
                {
                    BuyerAddr2 = null;
                }
                BuyerLocation = dt.Rows[0]["BuyerLocation"].ToString();
                BuyerStateCode = dt.Rows[0]["BuyerStateCode"].ToString();
                BuyerPIN = int.Parse(dt.Rows[0]["BuyerPIN"].ToString());
                ShipToGSTIN = dt.Rows[0]["ShipToGSTIN"].ToString();
                if (ShipToGSTIN == "")
                {
                    ShipToGSTIN = null;
                }
                ShipToLglName = dt.Rows[0]["ShipToLglName"].ToString();
                if (ShipToLglName == "")
                {
                    ShipToLglName = null;
                }
                ShipToAddr1 = dt.Rows[0]["ShipToAddr1"].ToString();
                if (ShipToAddr1 == "")
                {
                    ShipToAddr1 = null;
                }
                ShipToAddr2 = dt.Rows[0]["ShipToAddr2"].ToString();
                if (ShipToAddr2 == "")
                {
                    ShipToAddr2 = null;
                }
                ShipToLocation = dt.Rows[0]["ShipToLocation"].ToString();
                if (ShipToLocation == "")
                {
                    ShipToLocation = null;
                }
                ShipToStateCode = dt.Rows[0]["ShipToStatecode"].ToString();
                if (ShipToStateCode == "0")
                {
                    ShipToStateCode = null;
                }
                ShipToPIN = int.Parse(dt.Rows[0]["ShipToPIN"].ToString());
                AssVal = double.Parse(dt.Rows[0]["AssVal"].ToString());
                CgstVal = double.Parse(dt.Rows[0]["CgstVal"].ToString());
                SgstVal = double.Parse(dt.Rows[0]["SgstVal"].ToString());
                IgstVal = double.Parse(dt.Rows[0]["IgstVal"].ToString());
                OthChrg = double.Parse(dt.Rows[0]["OthChrg"].ToString());
                Discount = double.Parse(dt.Rows[0]["Discount"].ToString());
                TotInvVal = double.Parse(dt.Rows[0]["TotInvVal"].ToString());
                TotInvValFC = double.Parse(dt.Rows[0]["TotInvValFC"].ToString());
                RoundOff = double.Parse(dt.Rows[0]["Roundoff"].ToString());
                SupplyType = dt.Rows[0]["SupplyType"].ToString();

            }

            reqBody.Version = "1.1";

            TranDtls transd = new TranDtls();
            transd.TaxSch = "GST";
            transd.SupTyp = SupplyType;
            transd.RegRev = "N";
            transd.EcmGstin = null;
            transd.IgstOnIntra = "N";
            reqBody.TranDtls = transd;

            DocDtls docDtld = new DocDtls();
            docDtld.Typ = "INV";
            docDtld.Dt = BillDt;
            docDtld.No = BillNo;
            reqBody.DocDtls = docDtld;

            SellerDtls Selldtls = new SellerDtls();
            Selldtls.Gstin = SellerGSTIN;
            Selldtls.LglNm = SellerLglName;
            Selldtls.Addr1 = SellerAddr1;
            Selldtls.Addr2 = SellerAddr2;
            Selldtls.Loc = SellerLocation;
            Selldtls.Pin = SellerPIN;
            Selldtls.Stcd = SellerStateCode;
            reqBody.SellerDtls = Selldtls;

            BuyerDtls buydtls = new BuyerDtls();
            buydtls.Gstin = BuyerGSTIN;
            buydtls.LglNm = BuyerLglName;
            buydtls.Pos = BuyerPOS;
            buydtls.Addr1 = BuyerAddr1;
            buydtls.Addr2 = BuyerAddr2;
            buydtls.Loc = BuyerLocation;
            buydtls.Stcd = BuyerStateCode;
            buydtls.Pin = BuyerPIN;
            reqBody.BuyerDtls = buydtls;

            if (ShipToLglName == null && ShipToGSTIN == null && ShipToAddr1 == null && ShipToAddr2 == null && ShipToLocation == null && ShipToStateCode == null && ShipToPIN == 0)
            {
                reqBody.ShipDtls = null;
            }
            else
            {
                ShipDtls shipDtls = new ShipDtls();
                shipDtls.LglNm = ShipToLglName;
                shipDtls.Gstin = ShipToGSTIN;
                shipDtls.Addr1 = ShipToAddr1;
                shipDtls.Addr2 = ShipToAddr2;
                shipDtls.Loc = ShipToLocation;
                shipDtls.Stcd = ShipToStateCode;
                shipDtls.Pin = ShipToPIN;
                reqBody.ShipDtls = shipDtls;
            }

            SPName = "EinvGetBillDetails";
            ht = new Hashtable();
            ht.Add("@Type", "Unitbillbreakup");
            ht.Add("@BillType", ddlType.SelectedItem.Text);
            ht.Add("@month", month);
            ht.Add("@Clientid", ddlclientid.SelectedValue);
            ht.Add("@BillNo", lblbillnolatest.Text);
            dt = new DataTable();
            dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;
            System.Collections.Generic.List<ItemList> BreakupList = new System.Collections.Generic.List<ItemList>();
            if (dt.Rows.Count > 0)
            {

                int s = 1;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ItemList breakupdata = new ItemList();
                    breakupdata.SlNo = s.ToString();
                    breakupdata.IsServc = "Y";
                    breakupdata.PrdDesc = dt.Rows[i]["Designation"].ToString();
                    breakupdata.HsnCd = dt.Rows[i]["HsnCd"].ToString();
                    breakupdata.Qty = double.Parse(dt.Rows[i]["Qty"].ToString());
                    breakupdata.UnitPrice = double.Parse(dt.Rows[i]["UnitPrice"].ToString());
                    breakupdata.TotAmt = double.Parse(dt.Rows[i]["TotAmt"].ToString());
                    breakupdata.Discount = double.Parse(dt.Rows[i]["Discount"].ToString());
                    breakupdata.AssAmt = double.Parse(dt.Rows[i]["AssAmt"].ToString());
                    breakupdata.OthChrg = Convert.ToDouble(dt.Rows[i]["OthChrg"].ToString());
                    breakupdata.GstRt = int.Parse(dt.Rows[i]["GstRt"].ToString());
                    breakupdata.CgstAmt = Convert.ToDouble(dt.Rows[i]["CgstAmt"].ToString());
                    breakupdata.SgstAmt = Convert.ToDouble(dt.Rows[i]["SgstAmt"].ToString());
                    breakupdata.IgstAmt = Convert.ToDouble(dt.Rows[i]["IgstAmt"].ToString());
                    breakupdata.TotItemVal = Convert.ToDouble(dt.Rows[i]["TotItemVal"].ToString());
                    BreakupList.Add(breakupdata);

                    s++;
                }


            }

            reqBody.ItemList = BreakupList;

            ValDtls valDtls = new ValDtls();
            valDtls.AssVal = AssVal;
            valDtls.CgstVal = CgstVal;
            valDtls.SgstVal = SgstVal;
            valDtls.IgstVal = IgstVal;
            valDtls.Discount = Discount;
            valDtls.OthChrg = OthChrg;
            valDtls.RndOffAmt = RoundOff;
            valDtls.TotInvVal = TotInvVal;
            valDtls.TotInvValFc = TotInvValFC;
            reqBody.ValDtls = valDtls;

            DateTime ExpDate = DateTime.ParseExact(TokenExpiryDate, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);

            reqHeader header = new reqHeader();
            header.clientId = ClientID;
            header.pass = Password;
            header.ipAddress = IP;
            header.emailId = MailID;
            header.gstin = GSTIN;
            header.clientSecret = ClientSecretID;
            header.userName = Username;

            if (DateTime.Compare(ExpDate, DateTime.Now) > 0)
            {
                header.authToken = AuthToken;
                irnGenCall(header, reqBody, ddlType.SelectedItem.Text, month, ddlclientid.SelectedValue, lblbillnolatest.Text.Trim(), "INV", "");

            }
            else
            {
                var client = new RestClient("https://api.mastergst.com/einvoice/authenticate?email=" + MailID);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("username", Username);
                request.AddHeader("password", Password);
                request.AddHeader("ip_address", IP);
                request.AddHeader("client_id", ClientID);
                request.AddHeader("client_secret", ClientSecretID);
                request.AddHeader("gstin", GSTIN);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                AuthRespRoot resp = JsonConvert.DeserializeObject<AuthRespRoot>(response.Content);
                if (resp.status_cd == "0")
                {
                    var ErrorMessage = resp.status_desc;
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert( '" + ErrorMessage + "');", true);
                    return;
                }
                else
                {
                    AuthToken = resp.data.AuthToken;
                    TokenExpiryDate = resp.data.TokenExpiry;

                    SPName = "EinvGetMasterGSTDetails";
                    ht = new Hashtable();
                    ht.Add("@Type", "UpdateAuthToken");
                    ht.Add("@AuthToken", AuthToken);
                    ht.Add("@TokenExpiryDate", TokenExpiryDate);
                    ht.Add("@Clientid", ddlclientid.SelectedValue);

                    int status = config.ExecuteNonQueryParamsAsync(SPName, ht).Result;

                    if (status > 0)
                    {
                        header.authToken = AuthToken;
                        irnGenCall(header, reqBody, ddlType.SelectedItem.Text, month, ddlclientid.SelectedValue, lblbillnolatest.Text.Trim(), "INV", "");
                    }
                }

            }

        }

        public void irnGenCall(reqHeader header, IrnReqBody reqBody, String Billtype, int month, string Clientid, string BillNo, string DocType, string CreditNoteNo)
        {
            string IRN = "";
            string SignedQRCode = "";
            string SignedInvoice = "";
            string Status = "";
            string AckNo = "";
            DateTime AckDt = DateTime.Now;
            string ErrorMessage = "";
            string ErrorCode = "";


            var client = new RestClient("https://api.mastergst.com/einvoice/type/GENERATE/version/V1_03?email=" + header.emailId);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("username", header.userName);
            request.AddHeader("password", header.pass);
            request.AddHeader("ip_address", header.ipAddress);
            request.AddHeader("client_id", header.clientId);
            request.AddHeader("client_secret", header.clientSecret);
            request.AddHeader("auth-token", header.authToken);
            request.AddHeader("gstin", header.gstin);
            request.AddParameter("application/json", JsonConvert.SerializeObject(reqBody, Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            IrnResp resp = JsonConvert.DeserializeObject<IrnResp>(response.Content);

            if (resp.status_cd == "0")
            {

                System.Collections.Generic.List<ErrorDesc> errordesclist = JsonConvert.DeserializeObject<System.Collections.Generic.List<ErrorDesc>>(resp.status_desc);

                for (int i = 0; i < errordesclist.Count; i++)
                {
                    ErrorDesc errorDesc = errordesclist[i];
                    ErrorCode = errorDesc.ErrorCode;
                    ErrorMessage += "Error Code : " + ErrorCode + "\\n" + "Error Desc : " + errorDesc.ErrorMessage.Replace("'", "");

                }
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert( '" + ErrorMessage + "');", true);
                return;


            }
            else
            {
                IRN = resp.data.Irn;
                SignedQRCode = resp.data.SignedQRCode;
                AckNo = resp.data.AckNo;
                AckDt = resp.data.AckDt;
                SignedInvoice = resp.data.SignedInvoice;
                Status = resp.data.Status;


                string SPName = "EinvGetBillDetails";
                Hashtable ht = new Hashtable();
                ht.Add("@Type", "UpdateIRNQRcode");
                ht.Add("@IRN", IRN);
                ht.Add("@SignedQRCode", SignedQRCode);
                ht.Add("@SignedInvoice", SignedInvoice);
                ht.Add("@AckNo", AckNo);
                ht.Add("@AckDt", AckDt);
                ht.Add("@Status", Status);
                ht.Add("@BillType", Billtype);
                ht.Add("@month", month);
                ht.Add("@Clientid", Clientid);
                ht.Add("@BillNo", BillNo);
                ht.Add("@DocType", DocType);
                ht.Add("@CreditNoteNo", CreditNoteNo);
                int status = config.ExecuteNonQueryParamsAsync(SPName, ht).Result;

                if (status > 0)
                {
                    visiblebutton();
                    if (DocType == "INV")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('IRN generated successfully');", true);
                        return;
                    }

                    if (DocType == "CRN")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Credit Note generated successfully');", true);
                        return;
                    }
                }


            }


        }

        protected void btnCancelIRN_Click(object sender, EventArgs e)
        {
            int month = GetMonthBasedOnSelectionDateorMonth();

            string SPName = "EinvGetMasterGSTDetails";
            Hashtable ht = new Hashtable();
            ht.Add("@Type", "MasterGSTCredentials");
            ht.Add("@clientid", ddlclientid.SelectedValue);
            DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            string MailID = "";
            string Username = "";
            string Password = "";
            string IP = "";
            string ClientID = "";
            string ClientSecretID = "";
            string GSTIN = "";
            string AuthToken = "";
            string TokenExpiryDate = "";

            if (dt.Rows.Count > 0)
            {
                MailID = dt.Rows[0]["EmailID"].ToString();
                Username = dt.Rows[0]["UserName"].ToString();
                Password = dt.Rows[0]["Password"].ToString();
                IP = dt.Rows[0]["Password"].ToString();
                ClientID = dt.Rows[0]["ClientID"].ToString();
                ClientSecretID = dt.Rows[0]["ClientSecretID"].ToString();
                GSTIN = dt.Rows[0]["GSTIN"].ToString();
                AuthToken = dt.Rows[0]["AuthToken"].ToString();
                TokenExpiryDate = dt.Rows[0]["TokenExpiryDate"].ToString();

            }

            SPName = "EinvGetBillDetails";
            ht = new Hashtable();
            ht.Add("@Type", "GetIRN");
            ht.Add("@BillType", ddlType.SelectedItem.Text);
            ht.Add("@month", month);
            ht.Add("@BillNo", lblbillnolatest.Text);
            ht.Add("@Clientid", ddlclientid.SelectedValue);
            dt = new DataTable();
            dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            string IRN = "";

            if (dt.Rows.Count > 0)
            {
                IRN = dt.Rows[0]["IRN"].ToString();
            }

            CancelReqBody cancelReq = new CancelReqBody();
            cancelReq.Irn = IRN;
            cancelReq.CnlRsn = hfnCnclRsn.Value;
            cancelReq.CnlRem = hfCnclRemarks.Value;

            DateTime ExpDate = DateTime.ParseExact(TokenExpiryDate, "yyyy-MM-dd HH:mm:ss",
                                      System.Globalization.CultureInfo.InvariantCulture);

            reqHeader header = new reqHeader();
            header.clientId = ClientID;
            header.pass = Password;
            header.ipAddress = IP;
            header.emailId = MailID;
            header.gstin = GSTIN;
            header.clientSecret = ClientSecretID;
            header.userName = Username;

            if (DateTime.Compare(ExpDate, DateTime.Now) > 0)
            {
                header.authToken = AuthToken;
                CancelIRN(header, cancelReq, ddlType.SelectedItem.Text, month, ddlclientid.SelectedValue, lblbillnolatest.Text, "");

            }
            else
            {
                var client = new RestClient("https://api.mastergst.com/einvoice/authenticate?email=" + MailID);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("username", Username);
                request.AddHeader("password", Password);
                request.AddHeader("ip_address", IP);
                request.AddHeader("client_id", ClientID);
                request.AddHeader("client_secret", ClientSecretID);
                request.AddHeader("gstin", GSTIN);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                AuthRespRoot resp = JsonConvert.DeserializeObject<AuthRespRoot>(response.Content);
                if (resp.status_cd == "0")
                {
                    var ErrorMessage = resp.status_desc;
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert( '" + ErrorMessage + "');", true);
                    return;
                }
                else
                {
                    AuthToken = resp.data.AuthToken;
                    TokenExpiryDate = resp.data.TokenExpiry;

                    SPName = "EinvGetMasterGSTDetails";
                    ht = new Hashtable();
                    ht.Add("@Type", "UpdateAuthToken");
                    ht.Add("@AuthToken", AuthToken);
                    ht.Add("@TokenExpiryDate", TokenExpiryDate);
                    ht.Add("@clientid", ddlclientid.SelectedValue);

                    int status = config.ExecuteNonQueryParamsAsync(SPName, ht).Result;

                    if (status > 0)
                    {
                        header.authToken = AuthToken;
                        CancelIRN(header, cancelReq, ddlType.SelectedItem.Text, month, ddlclientid.SelectedValue, lblbillnolatest.Text, "");
                    }
                }

            }
        }

        public void CancelIRN(reqHeader header, CancelReqBody cancelReq, String Billtype, int month, string Clientid, string BillNo, string CreditNoteNo)
        {
            var client = new RestClient("https://api.mastergst.com/einvoice/type/CANCEL/version/V1_03?email=" + header.emailId);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("username", header.userName);
            request.AddHeader("password", header.pass);
            request.AddHeader("ip_address", header.ipAddress);
            request.AddHeader("client_id", header.clientId);
            request.AddHeader("client_secret", header.clientSecret);
            request.AddHeader("auth-token", header.authToken);
            request.AddHeader("gstin", header.gstin);
            request.AddParameter("application/json", JsonConvert.SerializeObject(cancelReq, Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            CancelResp resp = JsonConvert.DeserializeObject<CancelResp>(response.Content);

            string ErrorCode = "";
            string ErrorMessage = "";
            DateTime CancelDt = DateTime.Now;

            if (resp.status_cd == "0")
            {

                System.Collections.Generic.List<ErrorDesc> errordesclist = JsonConvert.DeserializeObject<System.Collections.Generic.List<ErrorDesc>>(resp.status_desc);


                for (int i = 0; i < errordesclist.Count; i++)
                {
                    ErrorDesc errorDesc = errordesclist[i];
                    ErrorCode = errorDesc.ErrorCode;
                    ErrorMessage += "Error Code : " + ErrorCode + "\\n" + "Error Desc : " + errorDesc.ErrorMessage.Replace("'", "");

                }
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert( '" + ErrorMessage + "');", true);
                return;

            }
            else
            {


                CancelDt = resp.data.CancelDate;

                string SPName = "EinvGetBillDetails";
                Hashtable ht = new Hashtable();
                ht.Add("@Type", "UpdateCancelIRN");
                ht.Add("@CancelDt", CancelDt);
                ht.Add("@Status", "CNL");
                ht.Add("@BillType", Billtype);
                ht.Add("@month", month);
                ht.Add("@Clientid", Clientid);
                ht.Add("@BillNo", BillNo);
                ht.Add("@CreditNoteNo", CreditNoteNo);
                int status = config.ExecuteNonQueryParamsAsync(SPName, ht).Result;

                if (status > 0)
                {
                    visiblebutton();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('IRN cancelled successfully');", true);
                    return;
                }


            }

        }

        protected void tempCnDetails_Click(object sender, EventArgs e)
        {
            int month = GetMonthBasedOnSelectionDateorMonth();

            string spname = "UpdateCreditNoteDetails";
            string type = "GetFullpaymentdetails";
            Hashtable ht = new Hashtable();
            ht.Add("@Billno", lblbillnolatest.Text.Trim());
            ht.Add("@type", type);
            ht.Add("@month", month);
            ht.Add("@clientid", ddlclientid.SelectedValue);
            ht.Add("@Billtype", ddlType.SelectedItem.Text);
            ht.Add("@CreditNoteRemarks", hfCreditNoteRemarks.Value.Trim());

            DataTable dt = config.ExecuteAdaptorWithParams(spname, ht).Result;

            if (dt.Rows.Count > 0)
            {
                gvCreditNoteSummary.DataSource = dt;
                gvCreditNoteSummary.DataBind();
                rdbfull.Checked = true;
                txtCreditnotedt.Text = hfCreditNoteDt.Value;

                lblcreditnoteamtn.Style.Add("display", "none");
                txtCreditNoteAmt.Style.Add("display", "none");
                lblCreditNoteCGSTPrcn.Style.Add("display", "none");
                txtCreditNoteCGSTPrc.Style.Add("display", "none");
                lblCreditNoteCGSTn.Style.Add("display", "none");
                txtCreditNoteCGST.Style.Add("display", "none");
                lblCreditNoteSGSTPrcn.Style.Add("display", "none");
                txtCreditNoteSGSTPrc.Style.Add("display", "none");
                txtCreditNoteCGSTPrc.Style.Add("display", "none");
                lblCreditNoteSGSTn.Style.Add("display", "none");
                txtCreditNoteSGST.Style.Add("display", "none");
                lblCreditNoteIGSTPrcn.Style.Add("display", "none");
                txtCreditNoteIGSTPrc.Style.Add("display", "none");
                lblCreditNoteIGSTn.Style.Add("display", "none");
                txtCreditNoteIGST.Style.Add("display", "none");
                lbltotaltaxamtn.Style.Add("display", "none");
                txtCreditNoteTaxamt.Style.Add("display", "none");
                lblcnhsn.Style.Add("display", "none");
                txtCreditNoteHSN.Style.Add("display", "none");


                Page.ClientScript.RegisterStartupScript(this.GetType(), "openDialog", "CreditNotePopup();", true);
            }
            else
            {
                //Bindgrid();
            }
        }

        protected void tempBindGrid_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string qry = "select '' as CreditNoteRemarks,'' as CreditNoteHSN,'0' as CreditNoteAmt,'0' as CreditNoteCGSTPrc,'0' as CreditNoteCGST,'0' as CreditNoteSGSTPrc,0 as CreditNoteSGST,'0' as CreditNoteIGSTPrc,0 as CreditNoteIGST,0 as CreditNoteTotalTaxAmt ";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            gvCreditNoteSummary.DataSource = dt;
            gvCreditNoteSummary.DataBind();

            if (hfRadio.Value == "rdbfull")
            {
                rdbfull.Checked = true;


                lblcreditnoteamtn.Style.Add("display", "none");
                txtCreditNoteAmt.Style.Add("display", "none");
                lblCreditNoteCGSTPrcn.Style.Add("display", "none");
                txtCreditNoteCGSTPrc.Style.Add("display", "none");
                lblCreditNoteCGSTn.Style.Add("display", "none");
                txtCreditNoteCGST.Style.Add("display", "none");
                lblCreditNoteSGSTPrcn.Style.Add("display", "none");
                txtCreditNoteSGSTPrc.Style.Add("display", "none");
                txtCreditNoteCGSTPrc.Style.Add("display", "none");
                lblCreditNoteSGSTn.Style.Add("display", "none");
                txtCreditNoteSGST.Style.Add("display", "none");
                lblCreditNoteIGSTPrcn.Style.Add("display", "none");
                txtCreditNoteIGSTPrc.Style.Add("display", "none");
                lblCreditNoteIGSTn.Style.Add("display", "none");
                txtCreditNoteIGST.Style.Add("display", "none");
                lbltotaltaxamtn.Style.Add("display", "none");
                txtCreditNoteTaxamt.Style.Add("display", "none");
                lblcnhsn.Style.Add("display", "none");
                txtCreditNoteHSN.Style.Add("display", "none");
            }

            if (hfRadio.Value == "rdbPart")
            {
                rdbPart.Checked = true;


                lblcreditnoteamtn.Style.Add("display", "inline");
                txtCreditNoteAmt.Style.Add("display", "inline");
                lblCreditNoteCGSTPrcn.Style.Add("display", "inline");
                txtCreditNoteCGSTPrc.Style.Add("display", "inline");
                lblCreditNoteCGSTn.Style.Add("display", "inline");
                txtCreditNoteCGST.Style.Add("display", "inline");
                lblCreditNoteSGSTPrcn.Style.Add("display", "inline");
                txtCreditNoteSGSTPrc.Style.Add("display", "inline");
                txtCreditNoteCGSTPrc.Style.Add("display", "inline");
                lblCreditNoteSGSTn.Style.Add("display", "inline");
                txtCreditNoteSGST.Style.Add("display", "inline");
                lblCreditNoteIGSTPrcn.Style.Add("display", "inline");
                txtCreditNoteIGSTPrc.Style.Add("display", "inline");
                lblCreditNoteIGSTn.Style.Add("display", "inline");
                txtCreditNoteIGST.Style.Add("display", "inline");
                lbltotaltaxamtn.Style.Add("display", "inline");
                txtCreditNoteTaxamt.Style.Add("display", "inline");
                lblcnhsn.Style.Add("display", "inline");
                txtCreditNoteHSN.Style.Add("display", "inline");
            }





            Page.ClientScript.RegisterStartupScript(this.GetType(), "openDialog", "CreditNotePopup();", true);

        }

        public void DefaultData()
        {

            string qry = "select '' as CreditNoteRemarks,'' as CreditNoteHSN,'0' as CreditNoteAmt,'0' as CreditNoteCGSTPrc,'0' as CreditNoteCGST,'0' as CreditNoteSGSTPrc,0 as CreditNoteSGST,'0' as CreditNoteIGSTPrc,0 as CreditNoteIGST,0 as CreditNoteTotalTaxAmt ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add();
            }


            gvCreditNoteSummary.DataSource = dt;
            gvCreditNoteSummary.DataBind();


        }



        private void displaydata()
        {

            DataTable DtHSNNumber = GlobalData.Instance.LoadHSNNumbers();

            foreach (GridViewRow grdRow in gvClientBilling.Rows)
            {
                bind_dropdownlistHSN = (DropDownList)(gvClientBilling.Rows[grdRow.RowIndex].Cells[2].FindControl("ddlHSNNumber"));
                bind_dropdownlistHSN.Items.Clear();

                if (DtHSNNumber.Rows.Count > 0)
                {
                    bind_dropdownlistHSN.DataValueField = "Id";
                    bind_dropdownlistHSN.DataTextField = "HSNNo";
                    bind_dropdownlistHSN.DataSource = DtHSNNumber;
                    bind_dropdownlistHSN.DataBind();

                }

            }

        }

        public void visiblebutton()
        {
            var monthnew = "";
            lblIRN.Visible = false;

            int month = GetMonthBasedOnSelectionDateorMonth();

            string Einvqry = "select isnull(Einvoice,0) as Einvoice  from logindetails where Emp_id='" + Emp_id + "'";
            DataTable dtEinv = config.ExecuteAdaptorAsyncWithQueryParams(Einvqry).Result;

            bool Einv = false;

            if (dtEinv.Rows.Count > 0)
            {
                Einv = bool.Parse(dtEinv.Rows[0]["Einvoice"].ToString());
            }

            string SP = "EinvGetBillDetails";
            string Type = "GetIRN";
            Hashtable ht = new Hashtable();
            ht.Add("@Type", Type);
            ht.Add("@month", month);
            ht.Add("@Clientid", ddlclientid.SelectedValue);
            ht.Add("@Billno", lblbillnolatest.Text);
            ht.Add("@BillType", ddlType.SelectedItem.Text);
            DataTable dt = config.ExecuteAdaptorAsyncWithParams(SP, ht).Result;
            if (dt.Rows.Count > 0)
            {
                VisibleFreezeCredit();

                if (dt.Rows[0]["IRN"].ToString() != "")
                {

                    if (dt.Rows[0]["Status"].ToString() == "ACT")
                    {


                        if (Einv == true)
                        {
                            btnCancelIRN.Visible = true;
                            btnGenerateIRN.Visible = false;


                        }
                        else
                        {
                            btnCancelIRN.Visible = false;
                            btnGenerateIRN.Visible = false;

                        }

                        btngenratepayment.Visible = false;
                        btnUnFreeze.Visible = false;
                        btnFreeze.Visible = false;
                        lblIRN.Visible = true;
                        if (dt.Rows[0]["DocType"].ToString() == "INV")
                        {
                            lblIRN.Text = "IRN : " + dt.Rows[0]["IRN"].ToString();
                        }

                        if (dt.Rows[0]["DocType"].ToString() == "CRN")
                        {
                            lblIRN.Text = "Credit Note IRN : " + dt.Rows[0]["CreditNoteIRN"].ToString();
                            btncreditbill.Visible = true;
                            btnDeleteCreditNote.Visible = false;
                        }


                    }


                    if (dt.Rows[0]["Status"].ToString() == "CNL")
                    {
                        btnCancelIRN.Visible = false;
                        btnGenerateIRN.Visible = false;
                        btngenratepayment.Visible = false;
                        btnUnFreeze.Visible = false;
                        btnFreeze.Visible = false;
                        lblIRN.Visible = true;
                        lblIRN.Text = "IRN is Cancelled";
                    }
                }
                else
                {
                    if (Einv == true)
                    {
                        btnCancelIRN.Visible = false;


                        if (month != 0)
                        {
                            if (month.ToString().Length == 3)
                            {
                                monthnew = month.ToString().Substring(1, 2) + 0 + month.ToString().Substring(0, 1);
                            }
                            else
                            {
                                monthnew = month.ToString().Substring(2, 2) + month.ToString().Substring(0, 2);
                            }


                            if (int.Parse(monthnew) < 2202)
                            {
                                btnGenerateIRN.Visible = false;

                                if (dt.Rows[0]["DocType"].ToString() == "CRN")
                                {
                                    btngenratepayment.Visible = false;
                                    btnUnFreeze.Visible = false;
                                    btnFreeze.Visible = false;
                                    lblIRN.Visible = true;
                                    btnGenerateIRN.Visible = false;

                                    lblIRN.Text = "Credit Note IRN : " + dt.Rows[0]["CreditNoteIRN"].ToString();
                                    btncreditbill.Visible = true;
                                    btnDeleteCreditNote.Visible = false;

                                }

                            }
                            else
                            {
                                btnGenerateIRN.Visible = true;

                                if (dt.Rows[0]["DocType"].ToString() == "CRN")
                                {
                                    btngenratepayment.Visible = false;
                                    btnUnFreeze.Visible = false;
                                    btnFreeze.Visible = false;
                                    lblIRN.Visible = true;
                                    btnGenerateIRN.Visible = false;

                                    lblIRN.Text = "Credit Note IRN : " + dt.Rows[0]["CreditNoteIRN"].ToString();
                                    btncreditbill.Visible = true;
                                    btnDeleteCreditNote.Visible = false;

                                }

                            }
                        }
                    }
                    else
                    {
                        btnCancelIRN.Visible = false;
                        btnGenerateIRN.Visible = false;
                    }

                }
            }
            else
            {
                btngenratepayment.Visible = true;
                btnCancelIRN.Visible = false;
                btnGenerateIRN.Visible = false;
                lblIRN.Visible = false;
            }

        }

        public void GetCreditnotedetails()
        {
            gvCreditNote.DataSource = null;
            gvCreditNote.DataBind();

            int month = 0;
            month = GetMonthBasedOnSelectionDateorMonth();

            string spname = "UpdateCreditNoteDetails";
            string type = "GetCreditNote";
            Hashtable ht = new Hashtable();
            ht.Add("@Billno", lblbillnolatest.Text.Trim());
            ht.Add("@type", type);
            ht.Add("@month", month);
            ht.Add("@clientid", ddlclientid.SelectedValue);
            DataTable dt = config.ExecuteAdaptorWithParams(spname, ht).Result;


            if (dt.Rows.Count > 0)
            {
                pnlcreditdetails.Visible = true;
                gvCreditNote.DataSource = dt;
                gvCreditNote.DataBind();

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows[i]["CreditNoteIRN"].ToString() == "")
                    {
                        ImageButton lbtn_Delete = gvCreditNote.Rows[i].FindControl("lbtn_Delete") as ImageButton;
                        lbtn_Delete.Visible = false;
                    }
                }

                if (dt.Rows[0]["CreditNotePayment"].ToString() == "Full" && dt.Rows[0]["CreditNotePayment"].ToString() == "ACT")
                {
                    btncredited.Enabled = false;
                }
                else
                {
                    btncredited.Enabled = true;
                }
            }
            else
            {
                pnlcreditdetails.Visible = false;

            }
        }

        protected void btnCreditremarks_Click(object sender, EventArgs e)
        {
            try
            {

                int month = 0;
                month = GetMonthBasedOnSelectionDateorMonth();
                string CreditNo = "";
                bool ServiceTaxType = false;//no use this type 

                CreditNo = CreditBillnoAutoGenrate(ServiceTaxType, ddlclientid.SelectedValue, month);

                string ContractID = "";
                DateTime DtLastDay = DateTime.Now;
                if (Chk_Month.Checked == false)
                {
                    DtLastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlmonth.SelectedIndex);
                }
                if (Chk_Month.Checked == true)
                {
                    DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                }

                Hashtable HtGetContractID = new Hashtable();
                var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
                HtGetContractID.Add("@LastDay", DtLastDay);
                DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;
                if (DTContractID.Rows.Count > 0)
                {
                    ContractID = DTContractID.Rows[0]["contractid"].ToString();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not Avaialable For This Client.');", true);
                    return;
                }

                string qry = "select isnull(IncludeST,0) IncludeST,isnull(CGST,0) as CGST,isnull(SGST,0) as SGST,isnull(IGST,0) as IGST from contracts where clientid='" + ddlclientid.SelectedValue + "' and contractid='" + ContractID + "'";
                DataTable dtqry = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

                bool CGST = false;
                bool SGST = false;
                bool IGST = false;
                bool IncludeST = false;

                if (dtqry.Rows.Count > 0)
                {
                    CGST = bool.Parse(dtqry.Rows[0]["CGST"].ToString());
                    SGST = bool.Parse(dtqry.Rows[0]["SGST"].ToString());
                    IGST = bool.Parse(dtqry.Rows[0]["IGST"].ToString());
                    IncludeST = bool.Parse(dtqry.Rows[0]["IncludeST"].ToString());
                }

                qry = "select isnull(CGST,0) as CGST,isnull(SGST,0) as SGST,isnull(IGST,0) as IGST from tbloptions where '" + DtLastDay + "' between fromdate and todate ";
                DataTable dttbloptions = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

                decimal CGSTPer = 0;
                decimal SGSTPer = 0;
                decimal IGSTPer = 0;

                if (dttbloptions.Rows.Count > 0)
                {
                    CGSTPer = decimal.Parse(dttbloptions.Rows[0]["CGST"].ToString());
                    SGSTPer = decimal.Parse(dttbloptions.Rows[0]["SGST"].ToString());
                    IGSTPer = decimal.Parse(dttbloptions.Rows[0]["IGST"].ToString());
                }


                decimal CGSTAmt = 0;
                decimal SGSTAmt = 0;
                decimal IGSTAmt = 0;

                if (hfCreditNoteAmt.Value == "")
                {
                    hfCreditNoteAmt.Value = "0";
                }

                if (IncludeST == false)
                {
                    if (CGST == true)
                    {
                        if (CGSTPer > 0)
                        {
                            CGSTAmt = Math.Round(decimal.Parse(hfCreditNoteAmt.Value) / 100 * (CGSTPer), 2);
                        }
                    }
                    else
                    {
                        CGSTPer = 0;
                    }

                    if (SGST == true)
                    {
                        if (SGSTPer > 0)
                        {
                            SGSTAmt = Math.Round(decimal.Parse(hfCreditNoteAmt.Value) / 100 * (SGSTPer), 2);
                        }
                    }
                    else
                    {
                        SGSTPer = 0;
                    }

                    if (IGST == true)
                    {
                        if (IGSTPer > 0)
                        {
                            IGSTAmt = Math.Round(decimal.Parse(hfCreditNoteAmt.Value) / 100 * (IGSTPer), 2);
                        }
                    }
                    else
                    {
                        IGSTPer = 0;
                    }
                }
                else
                {
                    CGSTPer = 0;
                    SGSTPer = 0;
                    IGSTPer = 0;
                }


                var CreditNoteDt = "01/01/1900";

                if (hfCreditNoteDt.Value.Length > 0)
                {
                    CreditNoteDt = Timings.Instance.CheckDateFormat(hfCreditNoteDt.Value);
                }

                var CreditNotePayment = "";
                CreditNotePayment = hfCreditPayment.Value;




                //Fetch the Hidden Field values from the Request.Form collection.
                string[] CrediteNoteRemarksv = hfnCreditNoteRemarks.Value.ToString().Split(',');
                string[] CreditNoteHSNv = hfnCreditNoteHSN.Value.ToString().Split(',');
                string[] CreditNoteAmtv = hfnCreditNoteAmt.Value.ToString().Split(',');
                string[] CreditNoteCGSTPrcv = hfnCreditNoteCGSTPrc.Value.ToString().Split(',');
                string[] CreditNoteCGSTv = hfnCreditNoteCGST.Value.ToString().Split(',');
                string[] CreditNoteSGSTPrcv = hfnCreditNoteSGSTPrc.Value.ToString().Split(',');
                string[] CreditNoteSGSTv = hfnCreditNoteSGST.Value.ToString().Split(',');
                string[] CreditNoteIGSTPrcv = hfnCreditNoteIGSTPrc.Value.ToString().Split(',');
                string[] CreditNoteIGSTv = hfnCreditNoteIGST.Value.ToString().Split(',');
                string[] CreditNoteTotalTaxAmountv = hfnCreditNoteTotalTaxAmount.Value.ToString().Split(',');

                decimal TotalCreditNoteAmt = 0, TotalCreditNoteCGSTPrc = 0, TotalCreditNoteCGST = 0, TotalCreditNoteSGSTPrc = 0, TotalCreditNoteSGST = 0, TotalCreditNoteIGSTPrc = 0, TotalCreditNoteIGST = 0, TotalCreditNoteTotalTaxAmount = 0;


                for (int i = 0; i < CrediteNoteRemarksv.Length; i++)
                {
                    string SPName1 = "UpdateCreditNoteDetails";
                    string Type1 = "InsertCreditNoteBreakup";
                    Hashtable ht1 = new Hashtable();
                    ht1.Add("@CreditNoteRemarks", CrediteNoteRemarksv[i]);
                    ht1.Add("@CreditNoteHSN", CreditNoteHSNv[i]);
                    ht1.Add("@CreditNoteAmount", CreditNoteAmtv[i]);
                    ht1.Add("@CreditNoteCGSTPer", CreditNoteCGSTPrcv[i]);
                    ht1.Add("@CreditNoteCGST", CreditNoteCGSTv[i]);
                    ht1.Add("@CreditNoteSGST", CreditNoteSGSTv[i]);
                    ht1.Add("@CreditNoteSGSTPer", CreditNoteSGSTPrcv[i]);
                    ht1.Add("@CreditNoteIGST", CreditNoteIGSTv[i]);
                    ht1.Add("@CreditNoteIGSTPer", CreditNoteIGSTPrcv[i]);
                    ht1.Add("@CreditNoteTotalTaxAmount", CreditNoteTotalTaxAmountv[i]);
                    ht1.Add("@CreditNoteNo", CreditNo);
                    ht1.Add("@CreditNoteSiNo", i + 1);
                    ht1.Add("@BillType", ddlType.SelectedItem.Text);
                    ht1.Add("@month", month);
                    ht1.Add("@Clientid", ddlclientid.SelectedValue);
                    ht1.Add("@BillNo", lblbillnolatest.Text);
                    ht1.Add("@Type", Type1);
                    ht1.Add("@CreditNotePayment", hfCreditPayment.Value);


                    TotalCreditNoteAmt += decimal.Parse(CreditNoteAmtv[i]);
                    TotalCreditNoteCGSTPrc = decimal.Parse(CreditNoteCGSTPrcv[i]);
                    TotalCreditNoteCGST += decimal.Parse(CreditNoteCGSTv[i]);
                    TotalCreditNoteSGSTPrc = decimal.Parse(CreditNoteSGSTPrcv[i]);
                    TotalCreditNoteSGST += decimal.Parse(CreditNoteSGSTv[i]);
                    TotalCreditNoteIGSTPrc = decimal.Parse(CreditNoteIGSTPrcv[i]);
                    TotalCreditNoteIGST += decimal.Parse(CreditNoteIGSTv[i]);
                    TotalCreditNoteTotalTaxAmount += decimal.Parse(CreditNoteTotalTaxAmountv[i]);

                    int statuc = config.ExecuteNonQueryParamsAsync(SPName1, ht1).Result;


                }



                string SPName = "UpdateCreditNoteDetails";
                Hashtable ht = new Hashtable();
                ht.Add("@type", "InsertCreditNote");
                ht.Add("@BillType", ddlType.SelectedItem.Text);
                ht.Add("@month", month);
                ht.Add("@Clientid", ddlclientid.SelectedValue);
                ht.Add("@BillNo", lblbillnolatest.Text);
                ht.Add("@CreditNoteNo", CreditNo);
                ht.Add("@CreditNoteDate", CreditNoteDt);
                ht.Add("@CreditNoteAmount", TotalCreditNoteAmt);
                ht.Add("@CreditNoteCGST", TotalCreditNoteCGST);
                ht.Add("@CreditNoteSGST", TotalCreditNoteSGST);
                ht.Add("@CreditNoteIGST", TotalCreditNoteIGST);
                ht.Add("@CreditNoteCGSTPer", TotalCreditNoteCGSTPrc);
                ht.Add("@CreditNoteSGSTPer", TotalCreditNoteSGSTPrc);
                ht.Add("@CreditNoteIGSTPer", TotalCreditNoteIGSTPrc);
                ht.Add("@CreditNoteTotalTaxAmount", TotalCreditNoteTotalTaxAmount);
                ht.Add("@CreditNotePayment", hfCreditPayment.Value);
                ht.Add("@CreatedBy", Username);
                DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;


                VisibleFreezeCredit();
                visiblebutton();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }

        protected void btnDeleteCreditNote_Click(object sender, EventArgs e)
        {
            ImageButton thisTextBox = (ImageButton)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
            Label lblCreditNoteNo = (Label)thisGridViewRow.FindControl("lblCreditNoteNo");
            Label lblBillNo = (Label)thisGridViewRow.FindControl("lblBillNo");

            int month = GetMonthBasedOnSelectionDateorMonth();
            int status = 0;

            string spname = "UpdateCreditNoteDetails";
            string type = "DeleteCreditNote";
            Hashtable ht = new Hashtable();
            ht.Add("@Creditnoteno", lblCreditNoteNo.Text.Trim());
            ht.Add("@Billno", lblBillNo.Text.Trim());
            ht.Add("@type", type);
            ht.Add("@month", month);
            ht.Add("@clientid", ddlclientid.SelectedValue);
            status = config.ExecuteNonQueryParamsAsync(spname, ht).Result;

            VisibleFreezeCredit();
            visiblebutton();

        }

        public void VisibleFreezeCredit()
        {
            gvCreditNote.DataSource = null;
            gvCreditNote.DataBind();

            int month = 0;
            month = GetMonthBasedOnSelectionDateorMonth();

            string Einvqry = "select isnull(Einvoice,0) as Einvoice  from logindetails where Emp_id='" + Emp_id + "'";
            DataTable dtEinv = config.ExecuteAdaptorAsyncWithQueryParams(Einvqry).Result;

            bool Einv = false;

            if (dtEinv.Rows.Count > 0)
            {
                Einv = bool.Parse(dtEinv.Rows[0]["Einvoice"].ToString());
            }

            string spname = "UpdateCreditNoteDetails";
            string type = "GetCreditNote";
            Hashtable ht = new Hashtable();
            ht.Add("@Billno", lblbillnolatest.Text.Trim());
            ht.Add("@type", type);
            ht.Add("@month", month);
            ht.Add("@clientid", ddlclientid.SelectedValue);
            DataTable dt = config.ExecuteAdaptorWithParams(spname, ht).Result;


            if (dt.Rows.Count > 0)
            {
                pnlcreditdetails.Visible = true;
                gvCreditNote.DataSource = dt;
                gvCreditNote.DataBind();

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows[i]["CreditNoteIRN"].ToString() == "")
                    {
                        ImageButton lbtn_Delete = gvCreditNote.Rows[i].FindControl("lbtn_Delete") as ImageButton;
                        lbtn_Delete.Visible = true;

                        Button btnIssueCreditNote = gvCreditNote.Rows[i].FindControl("btnIssueCreditNote") as Button;
                        Button btnCancelCreditNote = gvCreditNote.Rows[i].FindControl("btnCancelCreditNote") as Button;


                        if (Einv == true)
                        {
                            btnIssueCreditNote.Visible = true;
                            btnCancelCreditNote.Enabled = false;
                        }

                    }
                    else
                    {
                        Button btnIssueCreditNote = gvCreditNote.Rows[i].FindControl("btnIssueCreditNote") as Button;
                        btnIssueCreditNote.Enabled = false;

                        ImageButton lbtn_Delete = gvCreditNote.Rows[i].FindControl("lbtn_Delete") as ImageButton;
                        lbtn_Delete.Enabled = false;

                        Button btnCancelCreditNote = gvCreditNote.Rows[i].FindControl("btnCancelCreditNote") as Button;
                        btnCancelCreditNote.Enabled = true;

                    }
                }

                if (dt.Rows[0]["CreditNotePayment"].ToString() == "Full" && dt.Rows[0]["Status"].ToString() == "ACT")
                {
                    btncredited.Enabled = false;
                }
                else
                {
                    btncredited.Enabled = true;
                }

                btncredited.Visible = true;

            }
            else
            {
                pnlcreditdetails.Visible = false;

                if (Einv == true)
                {
                    btncredited.Visible = true;
                    btncredited.Enabled = true;
                }
            }
        }

        private string CreditBillnoAutoGenrate(bool StType, string unitId, int month)
        {
            string billno = "00001";

            string selquery = "select GST.CreditBillPrefix from Clients C inner join GSTMaster GST on GST.Id=c.OurGSTIN where clientid='" + ddlclientid.SelectedValue + "'";
            DataTable seldt = config.ExecuteAdaptorAsyncWithQueryParams(selquery).Result;

            string CreditNoteBillPrefix = "CN";

            if (seldt.Rows.Count > 0)
            {
                CreditNoteBillPrefix = seldt.Rows[0]["CreditBillPrefix"].ToString();
            }

            string selectqueryclientid = "";

            string billStartNo = GlobalData.Instance.GetBillStartingNo(false);
            string billSeq = GlobalData.Instance.GetBillSequence();
            billno = CreditNoteBillPrefix + "/" + billSeq + "/" + billStartNo;



            selectqueryclientid = "select isnull(MAX( RIGHT(CreditNoteNo,3)),0) as billno from creditnoteunitbill where CreditNoteNo like '" + CreditNoteBillPrefix + "/" +
             billSeq + "/" + "%'";

            DataTable dt = config.ExecuteReaderWithQueryAsync(selectqueryclientid).Result;


            int BILLNO = 0;
            if (dt.Rows.Count > 0)
            {
                if (String.IsNullOrEmpty(dt.Rows[0]["billno"].ToString()) == false)
                    BILLNO = int.Parse(dt.Rows[0]["billno"].ToString());
            }

            billno = CreditNoteBillPrefix + "/" + billSeq + "/" + (Convert.ToInt32(BILLNO) + 1).ToString("000");


            return billno;




        }


        protected void btncreditnoteNew_Click(object sender, EventArgs e)
        {
            int month = GetMonthBasedOnSelectionDateorMonth();

            MemoryStream ms = new MemoryStream();
            Document document = new Document();
            document = new Document(PageSize.A4, 0, 0, 15, 50);


            Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            string filename = "";
            document.Open();

            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            string SelectBillNo = "Select RIGHT(billno,5) as DisplayBillNo from Unitbill where month='" + month + "' and unitid='" + ddlclientid.SelectedValue + "'";
            DataTable DtBilling = config.ExecuteReaderWithQueryAsync(SelectBillNo).Result;

            string DisplayBillNo = "";
            if (DtBilling.Rows.Count > 0)
            {

                DisplayBillNo = DtBilling.Rows[0]["DisplayBillNo"].ToString();
            }


            DownloadCreditBill(document, ms, sender);

            filename = DisplayBillNo + "/" + ddlclientid.SelectedValue + "/" + GetMonthName().Substring(0, 3) + "/" + GetMonthOfYear() + "/Invoice.pdf";



            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
        }

        public void DownloadCreditBill(Document document, MemoryStream ms, object sender)
        {

            ImageButton thisTextBox = (ImageButton)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
            Label lblCreditNoteNo = (Label)thisGridViewRow.FindControl("lblCreditNoteNo");
            Label lblBillNo = (Label)thisGridViewRow.FindControl("lblBillNo");


            int month = 0;

            int FontSize = 9;
            month = GetMonthBasedOnSelectionDateorMonth();
            if (gvClientBilling.Rows.Count > 0)
            {
                try
                {

                    for (int m = 0; m < 2; m++)
                    {
                        document.NewPage();
                        string CopyName = "";
                        if (m == 0)
                        {
                            CopyName = "Original for Recipient";
                        }
                        if (m == 1)
                        {
                            CopyName = "Duplicate for Supplier";
                        }

                        #region for PDF


                        Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
                        PdfWriter writer = PdfWriter.GetInstance(document, ms);
                        document.Open();
                        BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        DateTime dtn = DateTime.ParseExact(txtbilldate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        // for both "1/1/2000" or "25/1/2000" formats
                        string billdt = dtn.ToString("MM/dd/yyyy");

                        string BQry = "select * from TblOptions  where '" + billdt + "' between fromdate and todate ";
                        DataTable Bdt = config.ExecuteReaderWithQueryAsync(BQry).Result;

                        string CGSTAlias = "";
                        string SGSTAlias = "";
                        string IGSTAlias = "";
                        string Cess1Alias = "";
                        string Cess2Alias = "";
                        string GSTINAlias = "";
                        string OurGSTAlias = "";

                        var SqlQryForTaxes = @"select ServiceTaxSeparate,Cess,SHECess,SBCess,KKCess,CGST,SGST,IGST,cess1,cess2,CGSTAlias,SGSTAlias,IGSTAlias,cess1Alias,cess2Alias,GSTINAlias,OurGSTAlias from TblOptions where '" + billdt + "' between fromdate and todate ";
                        DataTable DtTaxes = config.ExecuteReaderWithQueryAsync(SqlQryForTaxes).Result;


                        string SCPersent = "";
                        if (DtTaxes.Rows.Count > 0)
                        {
                            SCPersent = DtTaxes.Rows[0]["ServiceTaxSeparate"].ToString();
                            CGSTAlias = DtTaxes.Rows[0]["CGSTAlias"].ToString();
                            SGSTAlias = DtTaxes.Rows[0]["SGSTAlias"].ToString();
                            IGSTAlias = DtTaxes.Rows[0]["IGSTAlias"].ToString();
                            Cess1Alias = DtTaxes.Rows[0]["Cess1Alias"].ToString();
                            Cess2Alias = DtTaxes.Rows[0]["Cess2Alias"].ToString();
                            GSTINAlias = DtTaxes.Rows[0]["GSTINAlias"].ToString();
                            OurGSTAlias = DtTaxes.Rows[0]["OurGSTAlias"].ToString();

                        }
                        else
                        {
                            lblResult.Text = "There Is No Tax Values For Generating Bills ";
                            return;
                        }



                        #region for CompanyInfo
                        string strQry = "Select * from CompanyInfo   where  branchid='" + BranchID + "'";
                        DataTable compInfo = config.ExecuteReaderWithQueryAsync(strQry).Result;


                        string companyName = "Your Company Name";
                        string companyAddress = "Your Company Address";
                        string companyaddressline = " ";
                        string emailid = "";
                        string website = "";
                        string phoneno = "";
                        string PANNO = "";
                        string PFNo = "";
                        string Esino = "";
                        string Servicetax = "";
                        string notes = "";
                        string ServiceText = "";
                        string HSNNumber = "";
                        string SACCode = "";

                        string BankACName = "";
                        string BankACNo = "";
                        string BankIFSCCode = "";
                        string BankbranchName = "";
                        string CorporateIDNo = "";
                        if (compInfo.Rows.Count > 0)
                        {
                            companyName = compInfo.Rows[0]["CompanyName"].ToString();
                            companyAddress = compInfo.Rows[0]["Address"].ToString();
                            //companyAddress = companyAddress.Replace("\r\n", string.Empty);
                            companyaddressline = compInfo.Rows[0]["Addresslineone"].ToString();
                            //CINNO = compInfo.Rows[0]["CINNO"].ToString();
                            PANNO = compInfo.Rows[0]["labourrule"].ToString();
                            PFNo = compInfo.Rows[0]["PFNo"].ToString();
                            Esino = compInfo.Rows[0]["ESINo"].ToString();
                            Servicetax = compInfo.Rows[0]["BillNotes"].ToString();
                            emailid = compInfo.Rows[0]["Emailid"].ToString();
                            website = compInfo.Rows[0]["Website"].ToString();
                            phoneno = compInfo.Rows[0]["Phoneno"].ToString();
                            notes = compInfo.Rows[0]["notes"].ToString();
                            HSNNumber = compInfo.Rows[0]["HSNNumber"].ToString();
                            SACCode = compInfo.Rows[0]["SACCode"].ToString();

                            BankACName = compInfo.Rows[0]["Bankname"].ToString();
                            BankACNo = compInfo.Rows[0]["bankaccountno"].ToString();
                            BankIFSCCode = compInfo.Rows[0]["IfscCode"].ToString();
                            //BankbranchName = compInfo.Rows[0]["BankbranchName"].ToString();
                            CorporateIDNo = compInfo.Rows[0]["CorporateIDNo"].ToString();
                        }

                        #endregion

                        DateTime DtLastDay = DateTime.Now;
                        if (Chk_Month.Checked == false)
                        {
                            DtLastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlmonth.SelectedIndex);
                        }
                        if (Chk_Month.Checked == true)
                        {
                            DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                        }
                        var ContractID = "";


                        #region  Begin Get Contract Id Based on The Last Day

                        Hashtable HtGetContractID = new Hashtable();
                        var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                        HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
                        HtGetContractID.Add("@LastDay", DtLastDay);
                        DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                        if (DTContractID.Rows.Count > 0)
                        {
                            ContractID = DTContractID.Rows[0]["contractid"].ToString();

                        }
                        #endregion

                        #region
                        string SqlQuryForServiCharge = "select ContractId,servicecharge, convert(nvarchar(20), ContractStartDate, 103) as ContractStartDate,ServiceChargeType,Description,IncludeST,ServiceTax75,Pono,typeofwork from contracts where " +
                            " clientid ='" + ddlclientid.SelectedValue + "' and ContractId='" + ContractID + "'";
                        DataTable DtServicecharge = config.ExecuteReaderWithQueryAsync(SqlQuryForServiCharge).Result;


                        string Typeofwork = "";
                        string ServiceCharge = "0";
                        string strSCType = "";
                        string strDescription = "We are presenting our bill for the House Keeping Services Provided at your establishment. Kindly release the payment at the earliest";
                        bool bSCType = false;
                        string strIncludeST = "";
                        string ContractStartDate = "";
                        string strST75 = "";
                        bool bIncludeST = false;
                        bool bST75 = false;
                        string POContent = "";
                        // string ServiceTaxCategory = "";
                        if (DtServicecharge.Rows.Count > 0)
                        {
                            if (String.IsNullOrEmpty(DtServicecharge.Rows[0]["ServiceCharge"].ToString()) == false)
                            {
                                ServiceCharge = DtServicecharge.Rows[0]["ServiceCharge"].ToString();
                            }
                            if (String.IsNullOrEmpty(DtServicecharge.Rows[0]["ServiceChargeType"].ToString()) == false)
                            {
                                strSCType = DtServicecharge.Rows[0]["ServiceChargeType"].ToString();
                            }
                            string tempDescription = DtServicecharge.Rows[0]["Description"].ToString();
                            if (tempDescription.Trim().Length > 0)
                            {
                                strDescription = tempDescription;
                            }
                            if (strSCType.Length > 0)
                            {
                                bSCType = Convert.ToBoolean(strSCType);
                            }
                            strIncludeST = DtServicecharge.Rows[0]["IncludeST"].ToString();
                            strST75 = DtServicecharge.Rows[0]["ServiceTax75"].ToString();
                            ContractStartDate = DtServicecharge.Rows[0]["ContractStartDate"].ToString();
                            if (strIncludeST == "True")
                            {
                                bIncludeST = true;
                            }
                            if (strST75 == "True")
                            {
                                bST75 = true;
                            }
                            POContent = DtServicecharge.Rows[0]["pono"].ToString();
                            Typeofwork = DtServicecharge.Rows[0]["typeofwork"].ToString();
                            // ServiceTaxCategory = DtServicecharge.Rows[0]["ServiceTaxCategory"].ToString();
                        }

                        #endregion

                        #region


                        string selectclientaddress = "select c.*,ssh.state as ShipToState1,ssh.gststatecode as shiptostatecode1, s.state as Statename,s.GSTStateCode,gst.gstno,gst.Address GstAddress from clients c  left join states ssh on ssh.stateid=c.ShipToState left join states s on s.stateid=c.state left join GSTMaster gst on gst.id=c.ourgstin where clientid= '" + ddlclientid.SelectedItem.ToString() + "'";
                        DataTable dtclientaddress = config.ExecuteAdaptorAsyncWithQueryParams(selectclientaddress).Result;
                        string OurGSTIN = "";
                        string GSTIN = "";
                        string StateCode = "0";
                        string State = "";
                        var Clocation = "";
                        var BuyersOrderNo = "";


                        string ShipToGSTIN = "";
                        string ShipToStateCode = "0";
                        string ShipToState = "";
                        string GstAddress = "";
                        string location = "";
                        if (dtclientaddress.Rows.Count > 0)
                        {
                            OurGSTIN = dtclientaddress.Rows[0]["gstno"].ToString();
                            StateCode = dtclientaddress.Rows[0]["GSTStateCode"].ToString();
                            GSTIN = dtclientaddress.Rows[0]["GSTIN"].ToString();
                            State = dtclientaddress.Rows[0]["Statename"].ToString();

                            ShipToStateCode = dtclientaddress.Rows[0]["shiptostatecode1"].ToString();
                            ShipToGSTIN = dtclientaddress.Rows[0]["ShipToGSTIN"].ToString();
                            ShipToState = dtclientaddress.Rows[0]["ShipToState1"].ToString();
                            Clocation = dtclientaddress.Rows[0]["location"].ToString();
                            BuyersOrderNo = dtclientaddress.Rows[0]["BuyersOrderNo"].ToString();
                            GstAddress = dtclientaddress.Rows[0]["GstAddress"].ToString();
                            location = dtclientaddress.Rows[0]["location"].ToString();

                        }





                        string SelectBillNo = "";



                        if (ddlType.SelectedIndex == 0)
                        {
                            SelectBillNo = "Select convert(nvarchar(10),cub.creditnotedate,103) as crnotedt, RIGHT(cub.billno,5) as DisplayBillNo,cub.CreditNoteNo as 'CreditNoteNumber', * from creditnoteunitbill cub inner join Unitbill ub on ub.billno=cub.billno where cub.month='" + month + "' and unitid='" + ddlclientid.SelectedValue + "' and cub.creditnoteno='" + lblCreditNoteNo.Text + "' and cub.billno='" + lblBillNo.Text.Trim() + "' ";
                        }
                        else
                        {
                            SelectBillNo = "Select convert(nvarchar(10),cub.creditnotedate,103) as crnotedt,RIGHT(cub.billno,5) as DisplayBillNo,cub.CreditNoteNo as 'CreditNoteNumber',* from creditnoteunitbill cub inner join mUnitbill ub on ub.billno=cub.billno where cub.month='" + month + "' and unitid='" + ddlclientid.SelectedValue + "' and cub.billno = '" + lblBillNo.Text.Trim() + "' and cub.creditnoteno='" + lblCreditNoteNo.Text + "'";
                        }
                        DataTable DtBilling = config.ExecuteReaderWithQueryAsync(SelectBillNo).Result;


                        string BillNo = "";
                        string DisplayBillNo = "";
                        string ExtraRemarks = "";
                        string ManualRemarks = "";


                        DateTime BillDate;
                        DateTime DueDate;


                        #region Variables for data Fields as on 11/03/2014 by venkat


                        decimal servicecharge = 0;
                        decimal servicetax = 0;
                        decimal cess = 0;
                        decimal sbcess = 0;
                        decimal kkcess = 0;

                        #region for GST on 17-6-2017 by swathi

                        decimal CGST = 0;
                        decimal SGST = 0;
                        decimal IGST = 0;
                        decimal Cess1 = 0;
                        decimal Cess2 = 0;
                        decimal CGSTPrc = 0;
                        decimal SGSTPrc = 0;
                        decimal IGSTPrc = 0;
                        decimal Cess1Prc = 0;
                        decimal Cess2Prc = 0;

                        #endregion for GST on 17-6-2017 by swathi

                        decimal shecess = 0;
                        decimal totalamount = 0;
                        decimal Grandtotal = 0;
                        decimal ServiceTax75 = 0;
                        decimal ServiceTax25 = 0;
                        decimal machinarycost = 0;
                        decimal materialcost = 0;
                        decimal maintenancecost = 0;
                        decimal extraonecost = 0;
                        decimal extratwocost = 0;
                        decimal discountone = 0;
                        decimal discounttwo = 0;

                        string machinarycosttitle = "";
                        string materialcosttitle = "";
                        string maintenancecosttitle = "";
                        string extraonecosttitle = "";
                        string extratwocosttitle = "";
                        string discountonetitle = "";
                        string discounttwotitle = "";

                        bool Extradatacheck = false;
                        bool ExtraDataSTcheck = false;

                        bool STMachinary = false;
                        bool STMaterial = false;
                        bool STMaintenance = false;
                        bool STExtraone = false;
                        bool STExtratwo = false;

                        bool SCMachinary = false;
                        bool SCMaterial = false;
                        bool SCMaintenance = false;
                        bool SCExtraone = false;
                        bool SCExtratwo = false;

                        bool STDiscountone = false;
                        bool STDiscounttwo = false;

                        string strExtradatacheck = "";
                        string strExtrastcheck = "";

                        string strSTMachinary = "";
                        string strSTMaterial = "";
                        string strSTMaintenance = "";
                        string strSTExtraone = "";
                        string strSTExtratwo = "";

                        string strSCMachinary = "";
                        string strSCMaterial = "";
                        string strSCMaintenance = "";
                        string strSCExtraone = "";
                        string strSCExtratwo = "";

                        string strSTDiscountone = "";
                        string strSTDiscounttwo = "";
                        string strSTCreditNote = "";
                        decimal staxamtonservicecharge = 0;

                        decimal RelChrgAmt = 0;


                        var CreditNoteNo = string.Empty;
                        DateTime CreditNoteDate;
                        var CreditNoteRemarks = "";
                        var CredinoteHSN = "";
                        decimal CreditNoteAmount = 0;
                        decimal CreditNoteCGST = 0;
                        decimal CreditNoteSGST = 0;
                        decimal CreditNoteIGST = 0;
                        decimal CreditNoteCGSTper = 0;
                        decimal CreditNoteSGSTper = 0;
                        decimal CreditNoteIGSTper = 0;
                        string SignedQRCode = "";
                        string IRN = "";
                        string Status = "";
                        decimal Roundoffamt = 0;

                        #endregion


                        if (DtBilling.Rows.Count > 0)
                        {
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CreditNoteIRN"].ToString()) == false)
                            {
                                IRN = DtBilling.Rows[0]["CreditNoteIRN"].ToString();
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CreditNoteQRCode"].ToString()) == false)
                            {
                                SignedQRCode = DtBilling.Rows[0]["CreditNoteQRCode"].ToString();
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Status"].ToString()) == false)
                            {
                                Status = DtBilling.Rows[0]["Status"].ToString();
                            }


                            ExtraRemarks = DtBilling.Rows[0]["Remarks"].ToString();
                            BillNo = DtBilling.Rows[0]["billno"].ToString();

                            DisplayBillNo = DtBilling.Rows[0]["DisplayBillNo"].ToString();
                            BillDate = Convert.ToDateTime(DtBilling.Rows[0]["billdt"].ToString());
                            if (ddlType.SelectedIndex == 0)
                            {
                                DueDate = Convert.ToDateTime(DtBilling.Rows[0]["duedt"].ToString());
                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax75"].ToString()) == false)
                                {
                                    ServiceTax75 = decimal.Parse(DtBilling.Rows[0]["ServiceTax75"].ToString());
                                }

                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax25"].ToString()) == false)
                                {
                                    ServiceTax25 = decimal.Parse(DtBilling.Rows[0]["ServiceTax25"].ToString());
                                }

                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["TotalServiceChargeAmt"].ToString()) == false)
                                {
                                    servicecharge = decimal.Parse(DtBilling.Rows[0]["TotalServiceChargeAmt"].ToString());
                                }

                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["RelChrgAmt"].ToString()) == false)
                                {
                                    RelChrgAmt = decimal.Parse(DtBilling.Rows[0]["RelChrgAmt"].ToString());
                                }
                            }

                            else
                            {
                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceChrg"].ToString()) == false)
                                {
                                    servicecharge = decimal.Parse(DtBilling.Rows[0]["ServiceChrg"].ToString());
                                }

                                // ManualRemarks = (DtBilling.Rows[0]["ManualRemarks"].ToString());
                            }

                            #region Begin New code for values taken from database as on 11/03/2014 by venkat

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["dutiestotalamount"].ToString()) == false)
                            {
                                totalamount = decimal.Parse(DtBilling.Rows[0]["dutiestotalamount"].ToString());
                            }


                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax"].ToString()) == false)
                            {
                                servicetax = decimal.Parse(DtBilling.Rows[0]["ServiceTax"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SBCessAmt"].ToString()) == false)
                            {
                                sbcess = decimal.Parse(DtBilling.Rows[0]["SBCessAmt"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["KKCessAmt"].ToString()) == false)
                            {
                                kkcess = decimal.Parse(DtBilling.Rows[0]["KKCessAmt"].ToString());
                            }

                            #region for GST as on 17-6-2017 by swathi


                            CreditNoteNo = DtBilling.Rows[0]["CreditNoteNumber"].ToString();
                            CreditNoteRemarks = DtBilling.Rows[0]["CreditNoteRemarks"].ToString();
                            CreditNoteDate = Convert.ToDateTime(DtBilling.Rows[0]["CreditNoteDate"].ToString());
                            CredinoteHSN = DtBilling.Rows[0]["CredinoteHSN"].ToString();
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CreditNoteCGST"].ToString()) == false)
                            {
                                CreditNoteCGST = decimal.Parse(DtBilling.Rows[0]["CreditNoteCGST"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CreditNoteSGST"].ToString()) == false)
                            {
                                CreditNoteSGST = decimal.Parse(DtBilling.Rows[0]["CreditNoteSGST"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CreditNoteIGST"].ToString()) == false)
                            {
                                CreditNoteIGST = decimal.Parse(DtBilling.Rows[0]["CreditNoteIGST"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CreditNoteCGSTper"].ToString()) == false)
                            {
                                CreditNoteCGSTper = decimal.Parse(DtBilling.Rows[0]["CreditNoteCGSTper"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CreditNoteSGSTper"].ToString()) == false)
                            {
                                CreditNoteSGSTper = decimal.Parse(DtBilling.Rows[0]["CreditNoteSGSTper"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CreditNoteIGSTper"].ToString()) == false)
                            {
                                CreditNoteIGSTper = decimal.Parse(DtBilling.Rows[0]["CreditNoteIGSTper"].ToString());
                            }



                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CGSTAmt"].ToString()) == false)
                            {
                                CGST = decimal.Parse(DtBilling.Rows[0]["CGSTAmt"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SGSTAmt"].ToString()) == false)
                            {
                                SGST = decimal.Parse(DtBilling.Rows[0]["SGSTAmt"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["IGSTAmt"].ToString()) == false)
                            {
                                IGST = decimal.Parse(DtBilling.Rows[0]["IGSTAmt"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess1Amt"].ToString()) == false)
                            {
                                Cess1 = decimal.Parse(DtBilling.Rows[0]["Cess1Amt"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess2Amt"].ToString()) == false)
                            {
                                Cess2 = decimal.Parse(DtBilling.Rows[0]["Cess2Amt"].ToString());
                            }


                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CGSTPrc"].ToString()) == false)
                            {
                                CGSTPrc = decimal.Parse(DtBilling.Rows[0]["CGSTPrc"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SGSTPrc"].ToString()) == false)
                            {
                                SGSTPrc = decimal.Parse(DtBilling.Rows[0]["SGSTPrc"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["IGSTPrc"].ToString()) == false)
                            {
                                IGSTPrc = decimal.Parse(DtBilling.Rows[0]["IGSTPrc"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess1Prc"].ToString()) == false)
                            {
                                Cess1Prc = decimal.Parse(DtBilling.Rows[0]["Cess1Prc"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess2Prc"].ToString()) == false)
                            {
                                Cess2Prc = decimal.Parse(DtBilling.Rows[0]["Cess2Prc"].ToString());
                            }

                            #endregion for GST as on 17-6-2017 by swathi


                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CESS"].ToString()) == false)
                            {
                                cess = decimal.Parse(DtBilling.Rows[0]["CESS"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SHECess"].ToString()) == false)
                            {
                                shecess = decimal.Parse(DtBilling.Rows[0]["SHECess"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["GrandTotal"].ToString()) == false)
                            {
                                Grandtotal = decimal.Parse(DtBilling.Rows[0]["GrandTotal"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["MachinaryCost"].ToString()) == false)
                            {
                                machinarycost = decimal.Parse(DtBilling.Rows[0]["MachinaryCost"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["MaterialCost"].ToString()) == false)
                            {
                                materialcost = decimal.Parse(DtBilling.Rows[0]["MaterialCost"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ElectricalChrg"].ToString()) == false)
                            {
                                maintenancecost = decimal.Parse(DtBilling.Rows[0]["ElectricalChrg"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraAmtone"].ToString()) == false)
                            {
                                extraonecost = decimal.Parse(DtBilling.Rows[0]["ExtraAmtone"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraAmtTwo"].ToString()) == false)
                            {
                                extratwocost = decimal.Parse(DtBilling.Rows[0]["ExtraAmtTwo"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Discount"].ToString()) == false)
                            {
                                discountone = decimal.Parse(DtBilling.Rows[0]["Discount"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Discounttwo"].ToString()) == false)
                            {
                                discounttwo = decimal.Parse(DtBilling.Rows[0]["Discounttwo"].ToString());
                            }

                            machinarycosttitle = DtBilling.Rows[0]["Machinarycosttitle"].ToString();
                            materialcosttitle = DtBilling.Rows[0]["Materialcosttitle"].ToString();
                            maintenancecosttitle = DtBilling.Rows[0]["Maintanancecosttitle"].ToString();
                            extraonecosttitle = DtBilling.Rows[0]["Extraonetitle"].ToString();
                            extratwocosttitle = DtBilling.Rows[0]["Extratwotitle"].ToString();
                            discountonetitle = DtBilling.Rows[0]["Discountonetitle"].ToString();
                            discounttwotitle = DtBilling.Rows[0]["Discounttwotitle"].ToString();

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Extradatacheck"].ToString()) == false)
                            {
                                strExtradatacheck = DtBilling.Rows[0]["Extradatacheck"].ToString();
                                if (strExtradatacheck == "True")
                                {
                                    Extradatacheck = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraDataSTcheck"].ToString()) == false)
                            {
                                strExtrastcheck = DtBilling.Rows[0]["ExtraDataSTcheck"].ToString();
                                if (strExtrastcheck == "True")
                                {
                                    ExtraDataSTcheck = true;
                                }
                            }



                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMachinary"].ToString()) == false)
                            {
                                strSTMachinary = DtBilling.Rows[0]["STMachinary"].ToString();
                                if (strSTMachinary == "True")
                                {
                                    STMachinary = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMaterial"].ToString()) == false)
                            {
                                strSTMaterial = DtBilling.Rows[0]["STMaterial"].ToString();
                                if (strSTMaterial == "True")
                                {
                                    STMaterial = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMaintenance"].ToString()) == false)
                            {
                                strSTMaintenance = DtBilling.Rows[0]["STMaintenance"].ToString();
                                if (strSTMaintenance == "True")
                                {
                                    STMaintenance = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STExtraone"].ToString()) == false)
                            {
                                strSTExtraone = DtBilling.Rows[0]["STExtraone"].ToString();
                                if (strSTExtraone == "True")
                                {
                                    STExtraone = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STExtratwo"].ToString()) == false)
                            {
                                strSTExtratwo = DtBilling.Rows[0]["STExtratwo"].ToString();
                                if (strSTExtratwo == "True")
                                {
                                    STExtratwo = true;
                                }
                            }


                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMachinary"].ToString()) == false)
                            {
                                strSCMachinary = DtBilling.Rows[0]["SCMachinary"].ToString();
                                if (strSCMachinary == "True")
                                {
                                    SCMachinary = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMaterial"].ToString()) == false)
                            {
                                strSCMaterial = DtBilling.Rows[0]["SCMaterial"].ToString();
                                if (strSCMaterial == "True")
                                {
                                    SCMaterial = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMaintenance"].ToString()) == false)
                            {
                                strSCMaintenance = DtBilling.Rows[0]["SCMaintenance"].ToString();
                                if (strSCMaintenance == "True")
                                {
                                    SCMaintenance = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCExtraone"].ToString()) == false)
                            {
                                strSCExtraone = DtBilling.Rows[0]["SCExtraone"].ToString();
                                if (strSCExtraone == "True")
                                {
                                    SCExtraone = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCExtratwo"].ToString()) == false)
                            {
                                strSCExtratwo = DtBilling.Rows[0]["SCExtratwo"].ToString();
                                if (strSCExtratwo == "True")
                                {
                                    SCExtratwo = true;
                                }
                            }


                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscountone"].ToString()) == false)
                            {
                                strSTDiscountone = DtBilling.Rows[0]["STDiscountone"].ToString();
                                if (strSTDiscountone == "True")
                                {
                                    STDiscountone = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscounttwo"].ToString()) == false)
                            {
                                strSTDiscounttwo = DtBilling.Rows[0]["STDiscounttwo"].ToString();
                                if (strSTDiscounttwo == "True")
                                {
                                    STDiscounttwo = true;
                                }
                            }


                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CreditNoteAmount"].ToString()) == false)
                            {
                                CreditNoteAmount = decimal.Parse(DtBilling.Rows[0]["CreditNoteAmount"].ToString());
                            }



                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Staxonservicecharge"].ToString()) == false)
                            {
                                staxamtonservicecharge = decimal.Parse(DtBilling.Rows[0]["Staxonservicecharge"].ToString());
                            }

                            #endregion
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Generate The Bill Once Again');", true);
                            return;
                        }
                        string Year = DateTime.Now.Year.ToString();
                        #endregion

                        document.AddTitle(companyName);
                        document.AddAuthor("DIYOS");
                        document.AddSubject("Invoice");
                        document.AddKeywords("Keyword1, keyword2, …");
                        string imagepath = Server.MapPath("~/assets/BillLogo.png");

                        if (SignedQRCode.Length > 0)
                        {
                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(SignedQRCode, QRCodeGenerator.ECCLevel.Q);
                            QRCode qrCode = new QRCode(qrCodeData);
                            System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(20);

                            iTextSharp.text.Image qrcodeimg = iTextSharp.text.Image.GetInstance(qrCodeImage, System.Drawing.Imaging.ImageFormat.Bmp);
                            qrcodeimg.ScalePercent(2.5f);
                            qrcodeimg.SetAbsolutePosition(495f, 740f);
                            document.Add(qrcodeimg);
                        }

                        PdfPTable tablelogon = new PdfPTable(1);
                        tablelogon.TotalWidth = 540f;
                        tablelogon.LockedWidth = true;
                        float[] widtlogon = new float[] { 5f };
                        tablelogon.SetWidths(widtlogon);


                        PdfPCell CTaxInvoice = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, FontSize + 2, Font.BOLD, BaseColor.BLACK)));
                        CTaxInvoice.HorizontalAlignment = 1;
                        CTaxInvoice.Border = 0;
                        CTaxInvoice.PaddingTop = 70;
                        tablelogon.AddCell(CTaxInvoice);

                        CTaxInvoice = new PdfPCell(new Paragraph("CREDIT NOTE ( " + CopyName + ")", FontFactory.GetFont(FontStyle, FontSize + 2, Font.BOLD, BaseColor.BLACK)));
                        CTaxInvoice.HorizontalAlignment = 1;
                        CTaxInvoice.Border = 0;
                        tablelogon.AddCell(CTaxInvoice);

                        CTaxInvoice = new PdfPCell(new Paragraph("CREDIT NOTE NO :  " + CreditNoteNo + "   CREDIT NOTE DATE : " + CreditNoteDate, FontFactory.GetFont(FontStyle, FontSize + 1, Font.BOLD, BaseColor.BLACK)));
                        CTaxInvoice.HorizontalAlignment = 0;
                        CTaxInvoice.BorderWidth = 0.2F;
                        tablelogon.AddCell(CTaxInvoice);

                        string imagepath5 = Server.MapPath("~/assets/" + CmpIDPrefix + "Watermarklogo.png");
                        string imagepathNewBillLogo = Server.MapPath("~/assets/" + CmpIDPrefix + "NewBilllogo.png");
                        if (File.Exists(imagepathNewBillLogo))
                        {
                            iTextSharp.text.Image gif2 = iTextSharp.text.Image.GetInstance(imagepathNewBillLogo);
                            gif2.Alignment = (iTextSharp.text.Image.ALIGN_LEFT | iTextSharp.text.Image.UNDERLYING);
                            gif2.ScalePercent(30f);
                            gif2.SetAbsolutePosition(6f, 750f);
                            //document.Add(gif2);
                        }
                        string imagepath4 = Server.MapPath("~/assets/" + CmpIDPrefix + "Imageall.png");
                        if (File.Exists(imagepath4))
                        {
                            iTextSharp.text.Image gif2 = iTextSharp.text.Image.GetInstance(imagepath4);
                            gif2.Alignment = (iTextSharp.text.Image.ALIGN_LEFT | iTextSharp.text.Image.UNDERLYING);
                            gif2.ScalePercent(52f);
                            gif2.SetAbsolutePosition(465f, 759f);
                            //document.Add(gif2);
                        }

                        string imagepathfooter = Server.MapPath("~/assets/" + CmpIDPrefix + "Footerlogo.png");
                        if (File.Exists(imagepathfooter))
                        {
                            iTextSharp.text.Image gif2 = iTextSharp.text.Image.GetInstance(imagepathfooter);
                            gif2.Alignment = (iTextSharp.text.Image.ALIGN_LEFT | iTextSharp.text.Image.UNDERLYING);
                            gif2.ScalePercent(75f);
                            gif2.SetAbsolutePosition(1f, 10f);
                            //document.Add(gif2);
                        }
                        document.Add(tablelogon);

                        PdfContentByte content = writer.DirectContent;

                        PdfPTable tablelogo = new PdfPTable(2);
                        tablelogo.TotalWidth = 540f;
                        tablelogo.LockedWidth = true;
                        float[] widtlogo = new float[] { 0.4f, 2f };
                        tablelogo.SetWidths(widtlogo);


                        PdfPCell CCompName4 = new PdfPCell(new Paragraph(companyName, FontFactory.GetFont(FontStyle, FontSize + 3, Font.BOLD, BaseColor.BLACK)));
                        CCompName4.HorizontalAlignment = 1;
                        CCompName4.Colspan = 2;
                        CCompName4.BorderWidthTop = 0;
                        CCompName4.BorderWidthBottom = 0;
                        CCompName4.BorderWidthLeft = 0;
                        CCompName4.BorderWidthRight = 0;
                        //tablelogo.AddCell(CCompName4);

                        PdfPCell Celemail = new PdfPCell(new Paragraph(GstAddress, FontFactory.GetFont(FontStyle, FontSize + 1, Font.NORMAL, BaseColor.BLACK)));
                        Celemail.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        Celemail.Colspan = 2;
                        Celemail.BorderWidthTop = 0;
                        Celemail.BorderWidthBottom = 0;
                        Celemail.BorderWidthLeft = 0;
                        Celemail.BorderWidthRight = 0;
                        //tablelogo.AddCell(Celemail);


                        Celemail = new PdfPCell(new Paragraph("Email: " + emailid, FontFactory.GetFont(FontStyle, FontSize + 1, Font.NORMAL, BaseColor.BLACK)));
                        Celemail.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        Celemail.Colspan = 2;
                        Celemail.BorderWidthTop = 0;
                        Celemail.BorderWidthBottom = 0;
                        Celemail.BorderWidthLeft = 0;
                        Celemail.BorderWidthRight = 0;
                        // tablelogo.AddCell(Celemail);

                        Celemail = new PdfPCell(new Paragraph("Phone: " + phoneno, FontFactory.GetFont(FontStyle, FontSize + 1, Font.NORMAL, BaseColor.BLACK)));
                        Celemail.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        Celemail.Colspan = 2;
                        Celemail.BorderWidthTop = 0;
                        Celemail.BorderWidthBottom = 0;
                        Celemail.BorderWidthLeft = 0;
                        Celemail.BorderWidthRight = 0;
                        //tablelogo.AddCell(Celemail);





                        var CelGSTaddr = new Paragraph();
                        CelGSTaddr.Add(new Chunk(CopyName, FontFactory.GetFont(FontStyle, FontSize - 1, Font.BOLD, BaseColor.BLACK)));
                        CelGSTaddr.SetLeading(0, 1f);
                        PdfPCell CellGstaddress = new PdfPCell();
                        CellGstaddress.AddElement(CelGSTaddr);
                        CellGstaddress.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        CellGstaddress.Colspan = 2;
                        CellGstaddress.BorderWidthTop = 0;
                        CellGstaddress.BorderWidthBottom = 0.2f;
                        CellGstaddress.BorderWidthLeft = 0;
                        CellGstaddress.BorderWidthRight = 0;
                        CellGstaddress.PaddingLeft = 470;
                        //tablelogo.AddCell(CellGstaddress);


                        //document.Add(tablelogo);

                        PdfPTable address = new PdfPTable(6);
                        address.TotalWidth = 540f;
                        address.LockedWidth = true;
                        float[] addreslogo = new float[] { 2f, 2f, 2f, 2f, 2f, 2f };
                        address.SetWidths(addreslogo);

                        PdfPTable tempTable1 = new PdfPTable(3);
                        tempTable1.TotalWidth = 270f;
                        tempTable1.LockedWidth = true;
                        float[] tempWidth1 = new float[] { 2f, 2f, 2f };
                        tempTable1.SetWidths(tempWidth1);

                        string addressData = "";

                        #region

                        PdfPCell Cellg = new PdfPCell(new Paragraph(companyName, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 3;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = .2f;
                        Cellg.BorderWidthRight = .2f;
                        tempTable1.AddCell(Cellg);

                        //Cellg = new PdfPCell(new Paragraph(": " + OurGSTIN, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        //Cellg.HorizontalAlignment = 0;
                        //Cellg.Colspan = 2;
                        //Cellg.BorderWidthBottom = 0;
                        //Cellg.BorderWidthTop = 0;
                        //Cellg.BorderWidthLeft = 0;
                        //Cellg.BorderWidthRight = 0.2f;
                        //tempTable1.AddCell(Cellg);


                        Cellg = new PdfPCell(new Paragraph(companyAddress, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 3;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = .2f;
                        Cellg.BorderWidthRight = .2f;
                        tempTable1.AddCell(Cellg);

                        Cellg = new PdfPCell(new Paragraph("PAN", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 1;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = .2f;
                        Cellg.BorderWidthRight = 0;
                        tempTable1.AddCell(Cellg);

                        Cellg = new PdfPCell(new Paragraph(": " + PANNO, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 2;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = 0;
                        Cellg.BorderWidthRight = 0.2f;
                        tempTable1.AddCell(Cellg);


                        Cellg = new PdfPCell(new Paragraph("CIN", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 1;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = .2f;
                        Cellg.BorderWidthRight = 0;
                        tempTable1.AddCell(Cellg);

                        Cellg = new PdfPCell(new Paragraph(": " + CorporateIDNo, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 2;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = 0;
                        Cellg.BorderWidthRight = 0.2f;
                        tempTable1.AddCell(Cellg);

                        if (POContent.Length > 0)
                        {
                            Cellg = new PdfPCell(new Paragraph("PO No", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            Cellg.HorizontalAlignment = 0;
                            Cellg.Colspan = 1;
                            Cellg.BorderWidthBottom = 0;
                            Cellg.BorderWidthTop = 0;
                            Cellg.BorderWidthLeft = .2f;
                            Cellg.BorderWidthRight = 0;
                            tempTable1.AddCell(Cellg);

                            Cellg = new PdfPCell(new Paragraph(": " + POContent, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            Cellg.HorizontalAlignment = 0;
                            Cellg.Colspan = 2;
                            Cellg.BorderWidthBottom = 0;
                            Cellg.BorderWidthTop = 0;
                            Cellg.BorderWidthLeft = 0;
                            Cellg.BorderWidthRight = 0.2f;
                            tempTable1.AddCell(Cellg);
                        }
                        else
                        {


                            Cellg = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.WHITE)));
                            Cellg.HorizontalAlignment = 0;
                            Cellg.Colspan = 1;
                            Cellg.BorderWidthBottom = 0;
                            Cellg.BorderWidthTop = 0;
                            Cellg.BorderWidthLeft = .2f;
                            Cellg.BorderWidthRight = 0;
                            Cellg.MinimumHeight = 14;
                            tempTable1.AddCell(Cellg);

                            Cellg = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.WHITE)));
                            Cellg.HorizontalAlignment = 0;
                            Cellg.Colspan = 2;
                            Cellg.BorderWidthBottom = 0;
                            Cellg.BorderWidthTop = 0;
                            Cellg.BorderWidthLeft = 0;
                            Cellg.BorderWidthRight = 0.2f;
                            Cellg.MinimumHeight = 14;
                            tempTable1.AddCell(Cellg);
                        }

                        Cellg = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.WHITE)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 1;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = .2f;
                        Cellg.BorderWidthRight = 0;
                        Cellg.MinimumHeight = 14;
                        tempTable1.AddCell(Cellg);

                        Cellg = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.WHITE)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 2;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = 0;
                        Cellg.BorderWidthRight = 0.2f;
                        Cellg.MinimumHeight = 14;
                        tempTable1.AddCell(Cellg);

                        //Celemail = new PdfPCell(new Paragraph("Receiver: (Billed To)", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        //Celemail.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        //Celemail.Colspan = 3;
                        //tempTable1.AddCell(Celemail);

                        PdfPCell clientaddrhno = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        clientaddrhno.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientaddrhno.Colspan = 3;                                 //clientaddrhno.Colspan = 0;
                        clientaddrhno.BorderWidthBottom = 0;
                        clientaddrhno.BorderWidthTop = 0.2f;
                        clientaddrhno.BorderWidthLeft = .2f;
                        clientaddrhno.BorderWidthRight = 0.2f;
                        tempTable1.AddCell(clientaddrhno);

                        addressData = dtclientaddress.Rows[0]["ClientAddrHno"].ToString();
                        if (addressData.Trim().Length > 0)
                        {
                            clientaddrhno = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                            clientaddrhno.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clientaddrhno.Colspan = 3;                                 //clientaddrhno.Colspan = 0;
                            clientaddrhno.BorderWidthBottom = 0;
                            clientaddrhno.BorderWidthTop = 0;
                            clientaddrhno.BorderWidthLeft = .2f;
                            clientaddrhno.BorderWidthRight = 0.2f;
                            tempTable1.AddCell(clientaddrhno);
                        }
                        addressData = dtclientaddress.Rows[0]["ClientAddrStreet"].ToString();
                        if (addressData.Trim().Length > 0)
                        {
                            PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clientstreet.BorderWidthBottom = 0;
                            clientstreet.BorderWidthTop = 0;
                            clientstreet.Colspan = 3;
                            clientstreet.BorderWidthLeft = .2f;
                            clientstreet.BorderWidthRight = 0.2f;
                            tempTable1.AddCell(clientstreet);
                        }


                        addressData = dtclientaddress.Rows[0]["ClientAddrArea"].ToString();
                        if (addressData.Trim().Length > 0)
                        {
                            PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clientstreet.BorderWidthBottom = 0;
                            clientstreet.BorderWidthTop = 0;
                            clientstreet.Colspan = 3;
                            clientstreet.BorderWidthLeft = .2f;
                            clientstreet.BorderWidthRight = 0.2f;
                            // clientstreet.PaddingLeft = 20;
                            tempTable1.AddCell(clientstreet);
                        }


                        addressData = dtclientaddress.Rows[0]["ClientAddrColony"].ToString();
                        if (addressData.Trim().Length > 0)
                        {
                            PdfPCell clientcolony = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            clientcolony.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clientcolony.Colspan = 3;
                            clientcolony.BorderWidthBottom = 0;
                            clientcolony.BorderWidthTop = 0;
                            clientcolony.BorderWidthLeft = .2f;
                            clientcolony.BorderWidthRight = 0.2f;
                            tempTable1.AddCell(clientcolony);
                        }

                        addressData = dtclientaddress.Rows[0]["ClientAddrcity"].ToString();
                        if (addressData.Trim().Length > 0)
                        {
                            PdfPCell clientcolony = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            clientcolony.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clientcolony.Colspan = 3;
                            clientcolony.BorderWidthBottom = 0;
                            clientcolony.BorderWidthTop = 0;
                            clientcolony.BorderWidthLeft = .2f;
                            clientcolony.BorderWidthRight = 0.2f;
                            tempTable1.AddCell(clientcolony);
                        }

                        addressData = dtclientaddress.Rows[0]["ClientAddrstate"].ToString();
                        if (addressData.Trim().Length > 0)
                        {
                            PdfPCell clientstate = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            clientstate.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clientstate.Colspan = 3;
                            clientstate.BorderWidthBottom = 0;
                            clientstate.BorderWidthTop = 0;
                            clientstate.BorderWidthLeft = .2f;
                            clientstate.BorderWidthRight = 0.2f;
                            tempTable1.AddCell(clientstate);
                        }
                        addressData = dtclientaddress.Rows[0]["ClientAddrpin"].ToString();
                        if (addressData.Trim().Length > 0)
                        {
                            PdfPCell clietnpin = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clietnpin.Colspan = 3;
                            clietnpin.BorderWidthBottom = 0;
                            clietnpin.BorderWidthTop = 0;
                            clietnpin.BorderWidthLeft = .2f;
                            clietnpin.BorderWidthRight = 0.2f;
                            tempTable1.AddCell(clietnpin);
                        }

                        addressData = dtclientaddress.Rows[0]["Line7"].ToString();
                        if (addressData.Trim().Length > 0)
                        {
                            PdfPCell clietnpin = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clietnpin.Colspan = 3;
                            clietnpin.BorderWidthBottom = 0;
                            clietnpin.BorderWidthTop = 0;
                            clietnpin.BorderWidthLeft = .2f;
                            clietnpin.BorderWidthRight = 0.2f;
                            tempTable1.AddCell(clietnpin);
                        }


                        addressData = dtclientaddress.Rows[0]["Line8"].ToString();
                        if (addressData.Trim().Length > 0)
                        {
                            PdfPCell clietnpin = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clietnpin.Colspan = 3;
                            clietnpin.BorderWidthBottom = 0;
                            clietnpin.BorderWidthTop = 0;
                            clietnpin.BorderWidthLeft = .2f;
                            clietnpin.BorderWidthRight = 0.2f;
                            //  clietnpin.PaddingLeft = 20;
                            tempTable1.AddCell(clietnpin);
                        }

                        if (Bdt.Rows.Count > 0)
                        {


                            if (GSTIN.Length > 0)
                            {
                                PdfPCell clietnpin = new PdfPCell(new Paragraph(GSTINAlias, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                clietnpin.Colspan = 1;
                                clietnpin.BorderWidthBottom = 0;
                                clietnpin.BorderWidthTop = 0;
                                clietnpin.BorderWidthLeft = .2f;
                                clietnpin.BorderWidthRight = 0;
                                tempTable1.AddCell(clietnpin);

                                clietnpin = new PdfPCell(new Paragraph(" : " + GSTIN, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                clietnpin.Colspan = 2;
                                clietnpin.BorderWidthBottom = 0;
                                clietnpin.BorderWidthTop = 0;
                                clietnpin.BorderWidthLeft = 0;
                                clietnpin.BorderWidthRight = .2f;
                                tempTable1.AddCell(clietnpin);

                            }
                        }


                        PdfPCell cellemp1 = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        cellemp1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cellemp1.Colspan = 3;
                        cellemp1.BorderWidthTop = 0;
                        cellemp1.BorderWidthBottom = 0;
                        cellemp1.BorderWidthLeft = .2f;
                        cellemp1.BorderWidthRight = 0.2f;
                        cellemp1.PaddingBottom = 5;
                        tempTable1.AddCell(cellemp1);

                        #endregion

                        PdfPCell childTable1 = new PdfPCell(tempTable1);
                        childTable1.Border = 0;
                        childTable1.Colspan = 3;
                        childTable1.HorizontalAlignment = 0;
                        address.AddCell(childTable1);

                        #region copy


                        PdfPTable tempTable2 = new PdfPTable(3);
                        tempTable2.TotalWidth = 270f;
                        tempTable2.LockedWidth = true;
                        float[] tempWidth2 = new float[] { 2.3f, 2f, 1.7f };
                        tempTable2.SetWidths(tempWidth2);

                        Cellg = new PdfPCell(new Paragraph("Credit Note No", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 1;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = .2f;
                        Cellg.BorderWidthRight = 0;
                        tempTable2.AddCell(Cellg);

                        Cellg = new PdfPCell(new Paragraph(": " + CreditNoteNo, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 2;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = 0;
                        Cellg.BorderWidthRight = 0.2f;
                        tempTable2.AddCell(Cellg);
                        Cellg = new PdfPCell(new Paragraph("Credit Note Date", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 1;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = .2f;
                        Cellg.BorderWidthRight = 0;
                        tempTable2.AddCell(Cellg);

                        Cellg = new PdfPCell(new Paragraph(": " + CreditNoteDate.Day.ToString("00") + "/" + CreditNoteDate.ToString("MM") + "/" + CreditNoteDate.Year, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 2;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = 0;
                        Cellg.BorderWidthRight = 0.2f;
                        tempTable2.AddCell(Cellg);

                        Cellg = new PdfPCell(new Paragraph("Invoice No", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 1;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = .2f;
                        Cellg.BorderWidthRight = 0;
                        tempTable2.AddCell(Cellg);

                        Cellg = new PdfPCell(new Paragraph(": " + BillNo, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 2;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = 0;
                        Cellg.BorderWidthRight = 0.2f;
                        tempTable2.AddCell(Cellg);

                        Cellg = new PdfPCell(new Paragraph("Invoice Date", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 1;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = .2f;
                        Cellg.BorderWidthRight = 0;
                        tempTable2.AddCell(Cellg);

                        Cellg = new PdfPCell(new Paragraph(": " + BillDate.Day.ToString("00") + "/" + BillDate.ToString("MM") + "/" + BillDate.Year, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 2;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = 0;
                        Cellg.BorderWidthRight = 0.2f;
                        tempTable2.AddCell(Cellg);

                        Cellg = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        Cellg.HorizontalAlignment = 0;
                        Cellg.Colspan = 3;
                        Cellg.PaddingTop = 45;
                        Cellg.BorderWidthBottom = 0.2f;
                        Cellg.BorderWidthTop = 0;
                        Cellg.BorderWidthLeft = .2f;
                        Cellg.BorderWidthRight = .2f;
                        tempTable2.AddCell(Cellg);

                        //Cellg = new PdfPCell(new Paragraph("Place of Supply", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        //Cellg.HorizontalAlignment = 0;
                        //Cellg.Colspan = 1;
                        //Cellg.BorderWidthBottom = 0;
                        //Cellg.BorderWidthTop = 0;
                        //Cellg.BorderWidthLeft = .2f;
                        //Cellg.BorderWidthRight = 0;
                        //tempTable2.AddCell(Cellg);

                        //Cellg = new PdfPCell(new Paragraph(": " + StateCode + " - " + State, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        //Cellg.HorizontalAlignment = 0;
                        //Cellg.Colspan = 2;
                        //Cellg.BorderWidthBottom = 0;
                        //Cellg.BorderWidthTop = 0;
                        //Cellg.BorderWidthLeft = 0;
                        //Cellg.BorderWidthRight = 0.2f;
                        //tempTable2.AddCell(Cellg);

                        PdfPCell Buyer = new PdfPCell(new Paragraph("SHIP To : ", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        Buyer.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        Buyer.Colspan = 3;
                        Cellg.BorderWidthBottom = 0;
                        Cellg.BorderWidthTop = 0.2f;
                        Cellg.BorderWidthLeft = 0.2f;
                        Cellg.BorderWidthRight = 0.2f;
                        tempTable2.AddCell(Buyer);


                        addressData = dtclientaddress.Rows[0]["ShipToLine1"].ToString();

                        PdfPCell clientaddrhnonew = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        clientaddrhnonew.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientaddrhnonew.Colspan = 3;                                 //clientaddrhno.Colspan = 0;
                        clientaddrhnonew.BorderWidthBottom = 0;
                        clientaddrhnonew.BorderWidthTop = 0.2f;
                        clientaddrhnonew.BorderWidthLeft = 0;
                        clientaddrhnonew.BorderWidthRight = 0.2f;
                        tempTable2.AddCell(clientaddrhnonew);

                        addressData = dtclientaddress.Rows[0]["ShipToLine2"].ToString();
                        if (addressData.Trim().Length > 0)
                        {
                            PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clientstreet.Colspan = 3;
                            clientstreet.BorderWidthBottom = 0;
                            clientstreet.BorderWidthTop = 0;
                            clientstreet.BorderWidthLeft = 0;
                            clientstreet.BorderWidthRight = 0.2f;
                            tempTable2.AddCell(clientstreet);
                        }


                        addressData = dtclientaddress.Rows[0]["ShipToLine3"].ToString();
                        if (addressData.Trim().Length > 0)
                        {
                            PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clientstreet.Colspan = 3;
                            clientstreet.BorderWidthBottom = 0;
                            clientstreet.BorderWidthTop = 0;
                            clientstreet.BorderWidthLeft = 0;
                            clientstreet.BorderWidthRight = 0.2f;
                            tempTable2.AddCell(clientstreet);
                        }


                        addressData = dtclientaddress.Rows[0]["ShipToLine4"].ToString();
                        if (addressData.Trim().Length > 0)
                        {
                            PdfPCell clientcolony = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            clientcolony.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clientcolony.Colspan = 3;
                            clientcolony.BorderWidthBottom = 0;
                            clientcolony.BorderWidthTop = 0;
                            clientcolony.BorderWidthLeft = 0;
                            clientcolony.BorderWidthRight = 0.2f;
                            tempTable2.AddCell(clientcolony);
                        }
                        addressData = dtclientaddress.Rows[0]["ShipToLine5"].ToString();
                        if (addressData.Trim().Length > 0)
                        {
                            PdfPCell clientcity = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            clientcity.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clientcity.Colspan = 3;
                            clientcity.BorderWidthBottom = 0;
                            clientcity.BorderWidthTop = 0;
                            clientcity.BorderWidthLeft = 0;
                            clientcity.BorderWidthRight = 0.2f;
                            tempTable2.AddCell(clientcity);
                        }
                        addressData = dtclientaddress.Rows[0]["ShipToLine6"].ToString();
                        if (addressData.Trim().Length > 0)
                        {
                            PdfPCell clientstate = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            clientstate.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clientstate.Colspan = 3;
                            clientstate.BorderWidthBottom = 0;
                            clientstate.BorderWidthTop = 0;
                            clientstate.BorderWidthLeft = 0;
                            clientstate.BorderWidthRight = 0.2f;
                            tempTable2.AddCell(clientstate);
                        }

                        if (Bdt.Rows.Count > 0)
                        {

                            if (ShipToGSTIN.Length > 0)
                            {
                                PdfPCell clietnpin = new PdfPCell(new Paragraph(GSTINAlias, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                clietnpin.Colspan = 1;
                                clietnpin.BorderWidthBottom = 0;
                                clietnpin.BorderWidthTop = 0;
                                clietnpin.BorderWidthLeft = 0;
                                clietnpin.BorderWidthRight = 0;
                                tempTable2.AddCell(clietnpin);

                                clietnpin = new PdfPCell(new Paragraph(" : " + ShipToGSTIN, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                clietnpin.Colspan = 2;
                                clietnpin.BorderWidthBottom = 0;
                                clietnpin.BorderWidthTop = 0;
                                clietnpin.BorderWidthLeft = 0;
                                clietnpin.BorderWidthRight = 0.5f;
                                tempTable2.AddCell(clietnpin);

                            }


                            if (ShipToStateCode.Length > 0)
                            {
                                PdfPCell clietnpin = new PdfPCell(new Paragraph("State Code", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                clietnpin.Colspan = 1;
                                clietnpin.BorderWidthBottom = 0;
                                clietnpin.BorderWidthTop = 0;
                                clietnpin.BorderWidthLeft = 0;
                                clietnpin.BorderWidthRight = 0;
                                clietnpin.BorderColor = BaseColor.GRAY;

                                // tempTable2.AddCell(clietnpin);

                                clietnpin = new PdfPCell(new Paragraph(" : " + ShipToStateCode, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                clietnpin.Colspan = 2;
                                clietnpin.BorderWidthBottom = 0;
                                clietnpin.BorderWidthTop = 0;
                                clietnpin.BorderWidthLeft = 0;
                                clietnpin.BorderWidthRight = 0.5f;
                                clietnpin.BorderColor = BaseColor.GRAY;

                                // tempTable2.AddCell(clietnpin);
                            }

                            if (ShipToState.Length > 0)
                            {
                                PdfPCell clietnpin = new PdfPCell(new Paragraph("State", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                clietnpin.Colspan = 1;
                                clietnpin.BorderWidthBottom = 0;
                                clietnpin.BorderWidthTop = 0;
                                clietnpin.BorderWidthLeft = 0;
                                clietnpin.BorderWidthRight = 0;
                                clietnpin.BorderColor = BaseColor.GRAY;

                                //tempTable2.AddCell(clietnpin);

                                clietnpin = new PdfPCell(new Paragraph(" : " + ShipToState, FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                clietnpin.Colspan = 2;
                                clietnpin.BorderWidthBottom = 0;
                                clietnpin.BorderWidthTop = 0;
                                clietnpin.BorderWidthLeft = 0;
                                clietnpin.BorderWidthRight = 0.5f;
                                clietnpin.BorderColor = BaseColor.GRAY;
                                //tempTable2.AddCell(clietnpin);
                            }

                        }

                        cellemp1 = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        cellemp1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cellemp1.Colspan = 2;
                        cellemp1.BorderWidthLeft = 0;
                        cellemp1.BorderWidthRight = 0.2f;
                        cellemp1.BorderWidthTop = 0;
                        cellemp1.BorderWidthBottom = 0;
                        tempTable2.AddCell(cellemp1);


                        PdfPCell childTable2 = new PdfPCell(tempTable2);
                        childTable2.Border = 0;
                        childTable2.Colspan = 3;
                        childTable2.HorizontalAlignment = 0;
                        address.AddCell(childTable2);

                        document.Add(address);


                        DateTime FromYear = Convert.ToDateTime(txtfromdate.Text, CultureInfo.GetCultureInfo("en-gb"));
                        DateTime ToYear = Convert.ToDateTime(txttodate.Text, CultureInfo.GetCultureInfo("en-gb")).AddDays(1);
                        TimeSpan objTimeSpan = ToYear - FromYear;
                        double Days = Convert.ToDouble(objTimeSpan.TotalDays);



                        PdfPTable address1 = new PdfPTable(2);
                        address1.TotalWidth = 540f;
                        address1.LockedWidth = true;
                        float[] addreslogo1 = new float[] { 0.6f, 2f };
                        address1.SetWidths(addreslogo1);

                        if (IRN.Length > 0)
                        {
                            if (Status == "ACT")
                            {
                                PdfPCell cellserIRN = new PdfPCell(new Phrase("IRN : " + IRN, FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                                cellserIRN.HorizontalAlignment = 1;
                                cellserIRN.BorderWidthBottom = 0;
                                cellserIRN.BorderWidthLeft = .2f;
                                cellserIRN.BorderWidthTop = 0.2f;
                                cellserIRN.BorderWidthRight = 0.2f;
                                cellserIRN.Colspan = 2;
                                address1.AddCell(cellserIRN);
                            }
                            if (Status == "CNL")
                            {
                                PdfPCell cellserIRN = new PdfPCell(new Phrase("IRN is cancelled ", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                                cellserIRN.HorizontalAlignment = 1;
                                cellserIRN.BorderWidthBottom = 0;
                                cellserIRN.BorderWidthLeft = .2f;
                                cellserIRN.BorderWidthTop = 0.2f;
                                cellserIRN.BorderWidthRight = 0.2f;
                                cellserIRN.Colspan = 2;
                                address1.AddCell(cellserIRN);

                            }
                        }


                        document.Add(address1);


                        #endregion

                        PdfPTable table;
                        PdfPCell cell;
                        string cellText;
                        decimal Taxes = 0;

                        decimal TotalQuantity = 0;
                        decimal TotalPayrate = 0;

                        for (int rowIndex = 0; rowIndex < gvClientBilling.Rows.Count; rowIndex++)
                        {

                            TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("lblnoofemployees"));
                            if (label1.Text == "0")
                            {
                                TotalQuantity = 0;
                            }
                            else
                            {
                                TotalQuantity += Convert.ToDecimal(label1.Text);
                            }

                            TextBox label11 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("lblpayrate"));
                            TextBox label2 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("lbldesgn"));
                            Label labldesig = (Label)(gvClientBilling.Rows[rowIndex].FindControl("lbldesignid"));
                            string dutyhrsQry = "select c.dutyhrs from contractdetails c inner join designations d on c.designations = d.designid " +
                               "  where c.clientid='" + ddlclientid.SelectedValue + "' and d.designid='" + labldesig.Text + "'";

                            //Duty Hrs removed for KL on 27/05/2015
                            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(dutyhrsQry).Result;
                            TotalPayrate = Convert.ToDecimal(label11.Text);


                        }





                        #region

                        table = new PdfPTable(7);
                        table.TotalWidth = 540f;
                        table.LockedWidth = true;
                        table.HorizontalAlignment = 1;
                        float[] colWidths = new float[] { 1f, 1.1f, 5.4f, 1.4f, 1.2f, 2f, 2.2f };
                        table.SetWidths(colWidths);


                        cell = new PdfPCell(new Phrase("SL", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0.2f;
                        cell.BorderWidthLeft = .2f;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = 0f;
                        cell.Colspan = 1;
                        table.AddCell(cell);



                        cell = new PdfPCell(new Phrase("PARTICULARS", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0.2f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = 0f;
                        cell.Colspan = 4;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("HSN CODE", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0.2f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = 0f;
                        cell.Colspan = 1;
                        table.AddCell(cell);

                        //cell = new PdfPCell(new Phrase("QTY", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        //cell.HorizontalAlignment = 1;
                        //cell.BorderWidthBottom = 0.2f;
                        //cell.BorderWidthLeft = 0.2f;
                        //cell.BorderWidthTop = 0.2f;
                        //cell.BorderWidthRight = 0f;
                        //cell.Colspan = 1;
                        //table.AddCell(cell);

                        //cell = new PdfPCell(new Phrase("RATE", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        //cell.HorizontalAlignment = 1;
                        //cell.BorderWidthBottom = 0.2f;
                        //cell.BorderWidthLeft = 0.2f;
                        //cell.BorderWidthTop = 0.2f;
                        //cell.BorderWidthRight = 0f;
                        //cell.Colspan = 1;
                        //table.AddCell(cell);



                        cell = new PdfPCell(new Phrase("Amount", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0.2f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = .2f;
                        cell.Colspan = 1;
                        table.AddCell(cell);

                        string qryn = "select * from creditnotebillbreakup where clientid='" + ddlclientid.SelectedValue + "' and month='" + month + "' and creditnoteno='" + lblCreditNoteNo.Text + "' and billno='" + lblBillNo.Text + "' ";
                        DataTable datatn = config.ExecuteAdaptorAsyncWithQueryParams(qryn).Result;

                        if (datatn.Rows.Count > 0)
                        {

                            for (int k = 0; k < datatn.Rows.Count; k++)
                            {
                                cell = new PdfPCell(new Phrase(datatn.Rows[k]["CreditNoteSiNo"].ToString(), FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                cell.Colspan = 1;
                                cell.BorderWidthRight = 0f;
                                cell.BorderWidthLeft = .2f;
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthTop = 0;
                                if (datatn.Rows.Count == 1)
                                {
                                    cell.FixedHeight = 60;
                                }
                                else
                                {
                                    cell.FixedHeight = 20;

                                }
                                cell.HorizontalAlignment = 1;
                                table.AddCell(cell);


                                cell = new PdfPCell(new Phrase(datatn.Rows[k]["CreditNoteDescription"].ToString(), FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                cell.BorderWidthBottom = .2f;
                                cell.BorderWidthLeft = .2f;
                                cell.BorderWidthTop = 0;
                                if (datatn.Rows.Count == 1)
                                {
                                    cell.FixedHeight = 60;
                                }
                                else
                                {
                                    cell.FixedHeight = 20;

                                }
                                cell.BorderWidthRight = 0f;
                                cell.Colspan = 4;
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase(datatn.Rows[k]["CreditNoteHSNNumber"].ToString(), FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = .2f;
                                cell.BorderWidthLeft = .2f;
                                cell.BorderWidthTop = 0;
                                if (datatn.Rows.Count == 1)
                                {
                                    cell.FixedHeight = 60;
                                }
                                else
                                {
                                    cell.FixedHeight = 20;

                                }
                                cell.BorderWidthRight = 0f;
                                cell.Colspan = 1;
                                table.AddCell(cell);

                                //cell = new PdfPCell(new Phrase(TotalQuantity.ToString(), FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                //cell.HorizontalAlignment = 1;
                                //cell.BorderWidthBottom = .2f;
                                //cell.BorderWidthLeft = .2f;
                                //cell.BorderWidthTop = 0;
                                //cell.FixedHeight = 60;
                                //cell.BorderWidthRight = 0f;
                                //cell.Colspan = 1;
                                //table.AddCell(cell);

                                //cell = new PdfPCell(new Phrase(TotalPayrate.ToString(), FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                //cell.HorizontalAlignment = 1;
                                //cell.BorderWidthBottom = .2f;
                                //cell.BorderWidthLeft = .2f;
                                //cell.BorderWidthTop = 0;
                                //cell.FixedHeight = 60;
                                //cell.BorderWidthRight = 0f;
                                //cell.Colspan = 1;
                                //table.AddCell(cell);

                                cell = new PdfPCell(new Phrase(decimal.Parse(datatn.Rows[k]["CreditNoteBasicDA"].ToString()).ToString("#,##0.00"), FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 2;
                                cell.BorderWidthBottom = .2f;
                                cell.BorderWidthLeft = .2f;
                                cell.BorderWidthTop = 0;
                                if (datatn.Rows.Count == 1)
                                {
                                    cell.FixedHeight = 60;
                                }
                                else
                                {
                                    cell.FixedHeight = 20;

                                }
                                cell.BorderWidthRight = 0.2f;
                                cell.Colspan = 1;
                                table.AddCell(cell);
                            }
                        }


                        document.Add(table);

                        PdfPTable tempTable22 = new PdfPTable(9);
                        tempTable22.TotalWidth = 540f;
                        tempTable22.LockedWidth = true;
                        float[] tempWidth22 = new float[] { 1f, 1.1f, 5.4f, 1.4f, 1.2f, 2f, 1.6f, 1.5f, 2.2f };
                        tempTable22.SetWidths(tempWidth22);

                        #region


                        if (Taxes > 0)
                        {
                            PdfPCell celldz1 = new PdfPCell(new Phrase("Taxable Value", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            celldz1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldz1.Colspan = 8;
                            celldz1.BorderWidthBottom = 0;
                            celldz1.BorderWidthLeft = .2f;
                            celldz1.BorderWidthTop = .2f;
                            celldz1.BorderWidthRight = 0f;
                            tempTable22.AddCell(celldz1);
                        }
                        else
                        {
                            PdfPCell celldz1 = new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                            celldz1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldz1.Colspan = 8;
                            celldz1.BorderWidthBottom = 0;
                            celldz1.BorderWidthLeft = .2f;
                            celldz1.BorderWidthTop = .2f;
                            celldz1.BorderWidthRight = 0f;
                            tempTable22.AddCell(celldz1);
                        }

                        PdfPCell celldz4 = new PdfPCell(new Phrase(" " + CreditNoteAmount.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        celldz4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        celldz4.BorderWidthBottom = 0;
                        celldz4.BorderWidthLeft = 0.2f;
                        celldz4.BorderWidthTop = .2f;
                        celldz4.BorderWidthRight = .2f;
                        tempTable22.AddCell(celldz4);




                        #endregion
                        #endregion

                        string Fromdate = txtfromdate.Text;
                        string Todate = txttodate.Text;




                        #region for taxes

                        if (!bIncludeST)
                        {

                            if (CreditNoteCGST > 0)
                            {
                                PdfPCell CellCGST = new PdfPCell(new Phrase(CGSTAlias + " @ " + CreditNoteCGSTper + "%", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellCGST.Colspan = 8;
                                CellCGST.BorderWidthBottom = 0;
                                CellCGST.BorderWidthLeft = .2f;
                                CellCGST.BorderWidthTop = 0;
                                CellCGST.BorderWidthRight = 0f;
                                tempTable22.AddCell(CellCGST);

                                PdfPCell CellCGSTAmt = new PdfPCell(new Phrase(CreditNoteCGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                CellCGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellCGSTAmt.BorderWidthBottom = 0;
                                CellCGSTAmt.BorderWidthLeft = 0.2f;
                                CellCGSTAmt.BorderWidthTop = 0;
                                CellCGSTAmt.BorderWidthRight = .2f;
                                tempTable22.AddCell(CellCGSTAmt);

                            }


                            if (CreditNoteSGST > 0)
                            {
                                PdfPCell CellSGST = new PdfPCell(new Phrase(SGSTAlias + " @ " + CreditNoteSGSTper + "%", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                CellSGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellSGST.Colspan = 8;
                                CellSGST.BorderWidthBottom = 0;
                                CellSGST.BorderWidthLeft = .2f;
                                CellSGST.BorderWidthTop = 0;
                                CellSGST.BorderWidthRight = 0f;
                                tempTable22.AddCell(CellSGST);

                                PdfPCell CellSGSTAmt = new PdfPCell(new Phrase(CreditNoteSGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                CellSGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellSGSTAmt.BorderWidthBottom = 0;
                                CellSGSTAmt.BorderWidthLeft = 0.2f;
                                CellSGSTAmt.BorderWidthTop = 0;
                                CellSGSTAmt.BorderWidthRight = .2f;
                                tempTable22.AddCell(CellSGSTAmt);

                            }

                            if (CreditNoteIGST > 0)
                            {
                                PdfPCell CellIGST = new PdfPCell(new Phrase(IGSTAlias + " @ " + CreditNoteIGSTper + "%", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                CellIGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellIGST.Colspan = 8;
                                CellIGST.BorderWidthBottom = 0;
                                CellIGST.BorderWidthLeft = .2f;
                                CellIGST.BorderWidthTop = 0;
                                CellIGST.BorderWidthRight = 0f;
                                tempTable22.AddCell(CellIGST);

                                PdfPCell CellIGSTAmt = new PdfPCell(new Phrase(CreditNoteIGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                                CellIGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellIGSTAmt.BorderWidthBottom = 0;
                                CellIGSTAmt.BorderWidthLeft = 0.2f;
                                CellIGSTAmt.BorderWidthTop = 0;
                                CellIGSTAmt.BorderWidthRight = .2f;
                                tempTable22.AddCell(CellIGSTAmt);

                            }

                        }


                        #endregion for taxes



                        cell = new PdfPCell(new Phrase("  ", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.BorderWidthBottom = 0;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = 0f;
                        cell.BorderWidthLeft = .2f;
                        cell.Colspan = 6;
                        tempTable22.AddCell(cell);

                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 2;
                        cell.BorderWidthBottom = 0;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = 0f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.Colspan = 2;
                        tempTable22.AddCell(cell);

                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 2;
                        cell.BorderWidthBottom = 0;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = .2f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.Colspan = 1;
                        tempTable22.AddCell(cell);

                        Grandtotal = CreditNoteAmount + CreditNoteCGST + CreditNoteSGST + CreditNoteIGST;

                        string GTotal = Grandtotal.ToString("0.00");
                        string[] arr = GTotal.ToString().Split("."[0]);
                        string inwords = "";
                        string rupee = (arr[0]);
                        string paise = "";
                        if (arr.Length == 2)
                        {
                            if (arr[1].Length > 0 && arr[1] != "00")
                            {
                                paise = (arr[1]);
                            }
                        }

                        if (paise != "0.00" && paise != "0" && paise != "")
                        {
                            int I = Int16.Parse(paise);
                            String p = NumberToEnglish.Instance.NumbersToWords(I, true);
                            paise = p;
                            rupee = NumberToEnglish.Instance.NumbersToWords(Convert.ToInt64(arr[0]), false);
                            inwords = " Rupees " + rupee + "" + paise + " Paise Only";

                        }
                        else
                        {
                            rupee = NumberToEnglish.Instance.NumbersToWords(Convert.ToInt64(arr[0]), true);
                            inwords = " Rupees " + rupee + " Only";
                        }


                        cell = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.BorderWidthBottom = 0;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = 0f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.Colspan = 6;
                        tempTable22.AddCell(cell);


                        cell = new PdfPCell(new Phrase("Grand  Total", FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 2;
                        cell.BorderWidthBottom = 0;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = 0f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.Colspan = 2;
                        tempTable22.AddCell(cell);


                        cell = new PdfPCell(new Phrase("" + Grandtotal.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 2;
                        cell.BorderWidthBottom = 0;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = .2f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.Colspan = 1;
                        tempTable22.AddCell(cell);


                        //

                        cell = new PdfPCell(new Phrase("( Amount In Words: " + inwords + " )", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0.2f;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = 0.2f;
                        cell.BorderWidthLeft = .2f;
                        cell.Colspan = 9;
                        tempTable22.AddCell(cell);

                        Grandtotal = CreditNoteAmount + CreditNoteCGST + CreditNoteSGST + CreditNoteIGST;



                        document.Add(tempTable22);

                        #region footer


                        PdfPTable Addterms = new PdfPTable(4);
                        Addterms.TotalWidth = 540f;
                        Addterms.LockedWidth = true;
                        float[] widthrerms = new float[] { 2.5f, 2f, 1.7f, 2f };
                        Addterms.SetWidths(widthrerms);





                        cell = new PdfPCell(new Phrase("For " + companyName, FontFactory.GetFont(FontStyle, FontSize, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.Border = 0;
                        cell.PaddingTop = 15;
                        cell.Colspan = 4;
                        Addterms.AddCell(cell);


                        cell = new PdfPCell(new Phrase("Authorised Signatory ", FontFactory.GetFont(FontStyle, FontSize, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.Border = 0;
                        cell.PaddingTop = 40;
                        cell.Colspan = 4;
                        Addterms.AddCell(cell);




                        document.Add(Addterms);




                        //PdfPTable tablelogo1 = new PdfPTable(3);
                        //tablelogo1.TotalWidth = 580f;
                        //tablelogo1.LockedWidth = true;
                        //float[] widtlogo1 = new float[] { 1.4f, 0.1f, 1.2f };
                        //tablelogo1.SetWidths(widtlogo1);

                        //cell = new PdfPCell(new Paragraph("Head Office: #232, 6TH A MAIN, 2nd BLOCK, HRBR LAYOUT, KALYAN NAGAR, BANGALORE- 560043\nBRANCHES AT: KARNATAKA, TAMILNADU, ANDHRA PRADESH, TELANGANA& GOA", FontFactory.GetFont(FontStyle, 9, Font.NORMAL, BaseColor.BLACK)));
                        //cell.HorizontalAlignment = 1;
                        //cell.BorderWidthBottom = 0;
                        //cell.BorderWidthTop = 0.5f;
                        //cell.BorderWidthLeft = 0;
                        //cell.BorderWidthRight = 0;
                        //cell.SetLeading(1, 1.3f);
                        //cell.Colspan = 3;
                        //tablelogo1.AddCell(cell);

                        ////tablelogo1.WriteSelectedRows(0, -1, 8, document.BottomMargin + 0, content);
                        //tablelogo1.WriteSelectedRows(0, -1, document.RightMargin - 10, document.BottomMargin + 96, content);

                        Rectangle rectangle = new Rectangle(document.PageSize);

                        #endregion
                        #endregion

                    }

                    document.Close();

                }
                catch (Exception ex)
                {
                    //LblResult.Text = ex.Message;
                }
            }
            else
            {
                // LblResult.Text = "There is no bill generated for selected client";
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' There is no bill generated for selected client ');", true);

            }
        }

        protected void btnIssueCreditNote_Click(object sender, EventArgs e)
        {
            Button thisTextBox = (Button)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
            Label lblCreditNoteNo = (Label)thisGridViewRow.FindControl("lblCreditNoteNo");
            Label lblBillNo = (Label)thisGridViewRow.FindControl("lblBillNo");


            int month = GetMonthBasedOnSelectionDateorMonth();

            string SPName = "EinvGetMasterGSTDetails";
            Hashtable ht = new Hashtable();
            ht.Add("@Type", "MasterGSTCredentials");
            ht.Add("@clientid", ddlclientid.SelectedValue);
            DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            string MailID = "";
            string Username = "";
            string Password = "";
            string IP = "";
            string ClientID = "";
            string ClientSecretID = "";
            string GSTIN = "";
            string AuthToken = "";
            string TokenExpiryDate = "";


            if (dt.Rows.Count > 0)
            {
                MailID = dt.Rows[0]["EmailID"].ToString();
                Username = dt.Rows[0]["UserName"].ToString();
                Password = dt.Rows[0]["Password"].ToString();
                IP = dt.Rows[0]["IP"].ToString();
                ClientID = dt.Rows[0]["ClientID"].ToString();
                ClientSecretID = dt.Rows[0]["ClientSecretID"].ToString();
                GSTIN = dt.Rows[0]["GSTIN"].ToString();
                AuthToken = dt.Rows[0]["AuthToken"].ToString();
                TokenExpiryDate = dt.Rows[0]["TokenExpiryDate"].ToString();

            }

            IrnReqBody reqBody = new IrnReqBody();
            reqBody.ExpDtls = null;
            reqBody.EwbDtls = null;
            reqBody.DispDtls = null;
            reqBody.ShipDtls = null;


            SPName = "EinvGetBillDetails";
            ht = new Hashtable();
            ht.Add("@Type", "CreditNoteUnitbill");
            ht.Add("@BillType", ddlType.SelectedItem.Text);
            ht.Add("@Month", month);
            ht.Add("@Clientid", ddlclientid.SelectedValue);
            ht.Add("@BillNo", lblBillNo.Text.Trim());
            ht.Add("@CreditNoteNo", lblCreditNoteNo.Text.Trim());
            dt = new DataTable();
            dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            string Type = "";
            string BillNo = "";
            string BillDt = "";
            string SellerGSTIN = "";
            string SellerLglName = "";
            string SellerAddr1 = "";
            string SellerAddr2 = "";
            string SellerLocation = "";
            int SellerPIN = 0;
            string SellerStateCode = "";
            string BuyerGSTIN = "";
            string BuyerLglName = "";
            string BuyerPOS = "";
            string BuyerAddr1 = "";
            string BuyerAddr2 = "";
            string BuyerLocation = "";
            string BuyerStateCode = "";
            int BuyerPIN = 0;
            string ShipToGSTIN = "";
            string ShipToLglName = "";
            string ShipToAddr1 = "";
            string ShipToAddr2 = "";
            string ShipToLocation = "";
            string ShipToStateCode = "";
            int ShipToPIN = 0;
            double AssVal = 0;
            double CgstVal = 0;
            double SgstVal = 0;
            double IgstVal = 0;
            double OthChrg = 0;
            double TotInvVal = 0;
            double TotInvValFC = 0;
            double RoundOff = 0;
            double Discount = 0;
            string SupplyType = "B2B";

            if (dt.Rows.Count > 0)
            {
                Type = dt.Rows[0]["Type"].ToString();
                BillNo = dt.Rows[0]["BillNo"].ToString();
                BillDt = dt.Rows[0]["BillDt"].ToString();
                SellerGSTIN = dt.Rows[0]["SellerGSTIN"].ToString();
                SellerLglName = dt.Rows[0]["SellerLglName"].ToString();
                SellerAddr1 = dt.Rows[0]["SellerAddr1"].ToString();
                SellerAddr2 = dt.Rows[0]["SellerAddr2"].ToString();
                SellerLocation = dt.Rows[0]["SellerLocation"].ToString();
                SellerPIN = int.Parse(dt.Rows[0]["SellerPIN"].ToString());
                SellerStateCode = dt.Rows[0]["SellerStateCode"].ToString();
                BuyerGSTIN = dt.Rows[0]["BuyerGSTIN"].ToString();
                BuyerLglName = dt.Rows[0]["BuyerLglName"].ToString();
                BuyerPOS = dt.Rows[0]["BuyerPOS"].ToString();
                BuyerAddr1 = dt.Rows[0]["BuyerAddr1"].ToString();
                BuyerAddr2 = dt.Rows[0]["BuyerAddr2"].ToString();
                if (BuyerAddr2 == "")
                {
                    BuyerAddr2 = null;
                }
                BuyerLocation = dt.Rows[0]["BuyerLocation"].ToString();
                BuyerStateCode = dt.Rows[0]["BuyerStateCode"].ToString();
                BuyerPIN = int.Parse(dt.Rows[0]["BuyerPIN"].ToString());
                ShipToGSTIN = dt.Rows[0]["ShipToGSTIN"].ToString();
                if (ShipToGSTIN == "")
                {
                    ShipToGSTIN = null;
                }
                ShipToLglName = dt.Rows[0]["ShipToLglName"].ToString();
                if (ShipToLglName == "")
                {
                    ShipToLglName = null;
                }
                ShipToAddr1 = dt.Rows[0]["ShipToAddr1"].ToString();
                if (ShipToAddr1 == "")
                {
                    ShipToAddr1 = null;
                }
                ShipToAddr2 = dt.Rows[0]["ShipToAddr2"].ToString();
                if (ShipToAddr2 == "")
                {
                    ShipToAddr2 = null;
                }
                ShipToLocation = dt.Rows[0]["ShipToLocation"].ToString();
                if (ShipToLocation == "")
                {
                    ShipToLocation = null;
                }
                ShipToStateCode = dt.Rows[0]["ShipToStatecode"].ToString();
                if (ShipToStateCode == "0")
                {
                    ShipToStateCode = null;
                }
                ShipToPIN = int.Parse(dt.Rows[0]["ShipToPIN"].ToString());
                AssVal = double.Parse(dt.Rows[0]["AssVal"].ToString());
                CgstVal = double.Parse(dt.Rows[0]["CgstVal"].ToString());
                SgstVal = double.Parse(dt.Rows[0]["SgstVal"].ToString());
                IgstVal = double.Parse(dt.Rows[0]["IgstVal"].ToString());
                OthChrg = double.Parse(dt.Rows[0]["OthChrg"].ToString());
                Discount = double.Parse(dt.Rows[0]["Discount"].ToString());
                TotInvVal = double.Parse(dt.Rows[0]["TotInvVal"].ToString());
                TotInvValFC = double.Parse(dt.Rows[0]["TotInvValFC"].ToString());
                RoundOff = double.Parse(dt.Rows[0]["Roundoff"].ToString());
                SupplyType = dt.Rows[0]["SupplyType"].ToString();

            }

            reqBody.Version = "1.1";

            TranDtls transd = new TranDtls();
            transd.TaxSch = "GST";
            transd.SupTyp = SupplyType;
            transd.RegRev = "N";
            transd.EcmGstin = null;
            transd.IgstOnIntra = "N";
            reqBody.TranDtls = transd;

            DocDtls docDtld = new DocDtls();
            docDtld.Typ = "CRN";
            docDtld.Dt = BillDt;
            docDtld.No = BillNo;
            reqBody.DocDtls = docDtld;

            SellerDtls Selldtls = new SellerDtls();
            Selldtls.Gstin = SellerGSTIN;
            Selldtls.LglNm = SellerLglName;
            Selldtls.Addr1 = SellerAddr1;
            Selldtls.Addr2 = SellerAddr2;
            Selldtls.Loc = SellerLocation;
            Selldtls.Pin = SellerPIN;
            Selldtls.Stcd = SellerStateCode;
            reqBody.SellerDtls = Selldtls;

            BuyerDtls buydtls = new BuyerDtls();
            buydtls.Gstin = BuyerGSTIN;
            buydtls.LglNm = BuyerLglName;
            buydtls.Pos = BuyerPOS;
            buydtls.Addr1 = BuyerAddr1;
            buydtls.Addr2 = BuyerAddr2;
            buydtls.Loc = BuyerLocation;
            buydtls.Stcd = BuyerStateCode;
            buydtls.Pin = BuyerPIN;
            reqBody.BuyerDtls = buydtls;

            if (ShipToLglName == null && ShipToGSTIN == null && ShipToAddr1 == null && ShipToAddr2 == null && ShipToLocation == null && ShipToStateCode == null && ShipToPIN == 0)
            {
                reqBody.ShipDtls = null;
            }
            else
            {
                ShipDtls shipDtls = new ShipDtls();
                shipDtls.LglNm = ShipToLglName;
                shipDtls.Gstin = ShipToGSTIN;
                shipDtls.Addr1 = ShipToAddr1;
                shipDtls.Addr2 = ShipToAddr2;
                shipDtls.Loc = ShipToLocation;
                shipDtls.Stcd = ShipToStateCode;
                shipDtls.Pin = ShipToPIN;
                reqBody.ShipDtls = shipDtls;
            }

            SPName = "EinvGetBillDetails";
            ht = new Hashtable();
            ht.Add("@Type", "CreditNoteUnitbillbreakup");
            ht.Add("@BillType", ddlType.SelectedItem.Text);
            ht.Add("@month", month);
            ht.Add("@Clientid", ddlclientid.SelectedValue);
            ht.Add("@BillNo", lblBillNo.Text);
            ht.Add("@CreditNoteNo", lblCreditNoteNo.Text.Trim());
            dt = new DataTable();
            dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;
            System.Collections.Generic.List<ItemList> BreakupList = new System.Collections.Generic.List<ItemList>();
            if (dt.Rows.Count > 0)
            {

                int s = 1;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ItemList breakupdata = new ItemList();
                    breakupdata.SlNo = s.ToString();
                    breakupdata.IsServc = "Y";
                    breakupdata.PrdDesc = dt.Rows[i]["Designation"].ToString();
                    breakupdata.HsnCd = dt.Rows[i]["HsnCd"].ToString();
                    breakupdata.Qty = double.Parse(dt.Rows[i]["Qty"].ToString());
                    breakupdata.UnitPrice = double.Parse(dt.Rows[i]["UnitPrice"].ToString());
                    breakupdata.TotAmt = double.Parse(dt.Rows[i]["TotAmt"].ToString());
                    breakupdata.Discount = double.Parse(dt.Rows[i]["Discount"].ToString());
                    breakupdata.AssAmt = double.Parse(dt.Rows[i]["AssAmt"].ToString());
                    breakupdata.OthChrg = Convert.ToDouble(dt.Rows[i]["OthChrg"].ToString());
                    breakupdata.GstRt = int.Parse(dt.Rows[i]["GstRt"].ToString());
                    breakupdata.CgstAmt = Convert.ToDouble(dt.Rows[i]["CgstAmt"].ToString());
                    breakupdata.SgstAmt = Convert.ToDouble(dt.Rows[i]["SgstAmt"].ToString());
                    breakupdata.IgstAmt = Convert.ToDouble(dt.Rows[i]["IgstAmt"].ToString());
                    breakupdata.TotItemVal = Convert.ToDouble(dt.Rows[i]["TotItemVal"].ToString());
                    BreakupList.Add(breakupdata);

                    s++;
                }


            }

            reqBody.ItemList = BreakupList;

            ValDtls valDtls = new ValDtls();
            valDtls.AssVal = AssVal;
            valDtls.CgstVal = CgstVal;
            valDtls.SgstVal = SgstVal;
            valDtls.IgstVal = IgstVal;
            valDtls.Discount = Discount;
            valDtls.OthChrg = OthChrg;
            valDtls.RndOffAmt = RoundOff;
            valDtls.TotInvVal = TotInvVal;
            valDtls.TotInvValFc = TotInvValFC;
            reqBody.ValDtls = valDtls;

            DateTime ExpDate = DateTime.ParseExact(TokenExpiryDate, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);

            reqHeader header = new reqHeader();
            header.clientId = ClientID;
            header.pass = Password;
            header.ipAddress = IP;
            header.emailId = MailID;
            header.gstin = GSTIN;
            header.clientSecret = ClientSecretID;
            header.userName = Username;

            if (DateTime.Compare(ExpDate, DateTime.Now) > 0)
            {
                header.authToken = AuthToken;
                irnGenCall(header, reqBody, ddlType.SelectedItem.Text, month, ddlclientid.SelectedValue, lblbillnolatest.Text.Trim(), "CRN", lblCreditNoteNo.Text.Trim());

            }
            else
            {
                var client = new RestClient("https://api.mastergst.com/einvoice/authenticate?email=" + MailID);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("username", Username);
                request.AddHeader("password", Password);
                request.AddHeader("ip_address", IP);
                request.AddHeader("client_id", ClientID);
                request.AddHeader("client_secret", ClientSecretID);
                request.AddHeader("gstin", GSTIN);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                AuthRespRoot resp = JsonConvert.DeserializeObject<AuthRespRoot>(response.Content);
                if (resp.status_cd == "0")
                {
                    var ErrorMessage = resp.status_desc;
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert( '" + ErrorMessage + "');", true);
                    return;
                }
                else
                {
                    AuthToken = resp.data.AuthToken;
                    TokenExpiryDate = resp.data.TokenExpiry;

                    SPName = "EinvGetMasterGSTDetails";
                    ht = new Hashtable();
                    ht.Add("@Type", "UpdateAuthToken");
                    ht.Add("@AuthToken", AuthToken);
                    ht.Add("@TokenExpiryDate", TokenExpiryDate);
                    ht.Add("@Clientid", ddlclientid.SelectedValue);

                    int status = config.ExecuteNonQueryParamsAsync(SPName, ht).Result;

                    if (status > 0)
                    {
                        header.authToken = AuthToken;
                        irnGenCall(header, reqBody, ddlType.SelectedItem.Text, month, ddlclientid.SelectedValue, lblbillnolatest.Text.Trim(), "CRN", lblCreditNoteNo.Text.Trim());
                    }
                }

            }


        }

        protected void btnCreditNoteCancelIRN_Click(object sender, EventArgs e)
        {

            Button thisTextBox = (Button)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
            Label lblCreditNoteNo = (Label)thisGridViewRow.FindControl("lblCreditNoteNo");
            Label lblBillNo = (Label)thisGridViewRow.FindControl("lblBillNo");


            int month = GetMonthBasedOnSelectionDateorMonth();

            string SPName = "EinvGetMasterGSTDetails";
            Hashtable ht = new Hashtable();
            ht.Add("@Type", "MasterGSTCredentials");
            ht.Add("@clientid", ddlclientid.SelectedValue);
            DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            string MailID = "";
            string Username = "";
            string Password = "";
            string IP = "";
            string ClientID = "";
            string ClientSecretID = "";
            string GSTIN = "";
            string AuthToken = "";
            string TokenExpiryDate = "";

            if (dt.Rows.Count > 0)
            {
                MailID = dt.Rows[0]["EmailID"].ToString();
                Username = dt.Rows[0]["UserName"].ToString();
                Password = dt.Rows[0]["Password"].ToString();
                IP = dt.Rows[0]["Password"].ToString();
                ClientID = dt.Rows[0]["ClientID"].ToString();
                ClientSecretID = dt.Rows[0]["ClientSecretID"].ToString();
                GSTIN = dt.Rows[0]["GSTIN"].ToString();
                AuthToken = dt.Rows[0]["AuthToken"].ToString();
                TokenExpiryDate = dt.Rows[0]["TokenExpiryDate"].ToString();

            }

            SPName = "EinvGetBillDetails";
            ht = new Hashtable();
            ht.Add("@Type", "GetIRN");
            ht.Add("@BillType", ddlType.SelectedItem.Text);
            ht.Add("@month", month);
            ht.Add("@BillNo", lblBillNo.Text);
            ht.Add("@CreditNoteNo", lblCreditNoteNo.Text.Trim());
            ht.Add("@Clientid", ddlclientid.SelectedValue);
            dt = new DataTable();
            dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            string IRN = "";

            if (dt.Rows.Count > 0)
            {
                IRN = dt.Rows[0]["IRN"].ToString();
            }

            CancelReqBody cancelReq = new CancelReqBody();
            cancelReq.Irn = IRN;
            cancelReq.CnlRsn = hfnCnclRsn.Value;
            cancelReq.CnlRem = hfCnclRemarks.Value;

            DateTime ExpDate = DateTime.ParseExact(TokenExpiryDate, "yyyy-MM-dd HH:mm:ss",
                                      System.Globalization.CultureInfo.InvariantCulture);

            reqHeader header = new reqHeader();
            header.clientId = ClientID;
            header.pass = Password;
            header.ipAddress = IP;
            header.emailId = MailID;
            header.gstin = GSTIN;
            header.clientSecret = ClientSecretID;
            header.userName = Username;

            if (DateTime.Compare(ExpDate, DateTime.Now) > 0)
            {
                header.authToken = AuthToken;
                CancelIRN(header, cancelReq, ddlType.SelectedItem.Text, month, ddlclientid.SelectedValue, lblBillNo.Text, lblCreditNoteNo.Text.Trim());

            }
            else
            {
                var client = new RestClient("https://api.mastergst.com/einvoice/authenticate?email=" + MailID);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("username", Username);
                request.AddHeader("password", Password);
                request.AddHeader("ip_address", IP);
                request.AddHeader("client_id", ClientID);
                request.AddHeader("client_secret", ClientSecretID);
                request.AddHeader("gstin", GSTIN);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                AuthRespRoot resp = JsonConvert.DeserializeObject<AuthRespRoot>(response.Content);
                if (resp.status_cd == "0")
                {
                    var ErrorMessage = resp.status_desc;
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert( '" + ErrorMessage + "');", true);
                    return;
                }
                else
                {
                    AuthToken = resp.data.AuthToken;
                    TokenExpiryDate = resp.data.TokenExpiry;

                    SPName = "EinvGetMasterGSTDetails";
                    ht = new Hashtable();
                    ht.Add("@Type", "UpdateAuthToken");
                    ht.Add("@AuthToken", AuthToken);
                    ht.Add("@TokenExpiryDate", TokenExpiryDate);
                    ht.Add("@clientid", ddlclientid.SelectedValue);

                    int status = config.ExecuteNonQueryParamsAsync(SPName, ht).Result;

                    if (status > 0)
                    {
                        header.authToken = AuthToken;
                        CancelIRN(header, cancelReq, ddlType.SelectedItem.Text, month, ddlclientid.SelectedValue, lblBillNo.Text, lblCreditNoteNo.Text);
                    }
                }

            }
        }


        #endregion for IRN and credit note IRN

    }
}
public class reqHeader
{
    public String userName
    {
        get;
        set;
    }

    public String pass
    {
        get;
        set;
    }

    public String ipAddress
    {
        get;
        set;
    }

    public String clientId
    {
        get;
        set;
    }

    public String clientSecret
    {
        get;
        set;
    }

    public String gstin
    {
        get;
        set;
    }

    public String emailId
    {
        get;
        set;
    }
    public String authToken
    {
        get;
        set;
    }
}
public class AuthRespData
{
    public string UserName { get; set; }
    public string TokenExpiry { get; set; }
    public string Sek { get; set; }
    public string ClientId { get; set; }
    public string AuthToken { get; set; }
}
public class AuthRespHeader
{
    public string username { get; set; }
    public string password { get; set; }
    public string ip_address { get; set; }
    public string client_id { get; set; }
    public string client_secret { get; set; }
    public string gstin { get; set; }
    public string PostmanToken { get; set; }
    public string gst_username { get; set; }
    public string txn { get; set; }
}
public class AuthRespRoot
{
    public AuthRespData data { get; set; }
    public string status_cd { get; set; }
    public string status_desc { get; set; }
    public AuthRespHeader header { get; set; }
}
public class TranDtls
{
    public string TaxSch { get; set; } = null;
    public string SupTyp { get; set; } = null;
    public string RegRev { get; set; } = null;
    public object EcmGstin { get; set; }
    public string IgstOnIntra { get; set; } = null;
}
public class DocDtls
{
    public string Typ { get; set; } = null;
    public string No { get; set; } = null;
    public string Dt { get; set; } = null;
}
public class SellerDtls
{
    public string Gstin { get; set; } = null;
    public string LglNm { get; set; } = null;
    public string TrdNm { get; set; } = null;
    public string Addr1 { get; set; } = null;
    public string Addr2 { get; set; } = null;
    public string Loc { get; set; } = null;
    public int Pin { get; set; } = 0;
    public string Stcd { get; set; } = null;
    public string Ph { get; set; } = null;
    public string Em { get; set; } = null;
}
public class BuyerDtls
{
    public string Gstin { get; set; } = null;
    public string LglNm { get; set; } = null;
    public string TrdNm { get; set; } = null;
    public string Pos { get; set; } = null;
    public string Addr1 { get; set; } = null;
    public string Addr2 { get; set; } = null;
    public string Loc { get; set; } = null;
    public int Pin { get; set; } = 0;
    public string Stcd { get; set; } = null;
    public string Ph { get; set; } = null;
    public string Em { get; set; } = null;
}
public class DispDtls
{
    public string Nm { get; set; } = null;
    public string Addr1 { get; set; } = null;
    public string Addr2 { get; set; } = null;
    public string Loc { get; set; } = null;
    public int Pin { get; set; } = 0;
    public string Stcd { get; set; } = null;
}
public class ShipDtls
{
    public string Gstin { get; set; } = null;
    public string LglNm { get; set; } = null;
    public string TrdNm { get; set; } = null;
    public string Addr1 { get; set; } = null;
    public string Addr2 { get; set; } = null;
    public string Loc { get; set; } = null;
    public int Pin { get; set; } = 0;
    public string Stcd { get; set; } = null;
}
public class BchDtls
{
    public string Nm { get; set; } = null;
    public string Expdt { get; set; } = null;
    public string wrDt { get; set; } = null;
}
public class AttribDtl
{
    public string Nm { get; set; }
    public string Val { get; set; }
}
public class ItemList
{
    public string SlNo { get; set; } = null;
    public string IsServc { get; set; } = null;
    public string PrdDesc { get; set; } = null;
    public string HsnCd { get; set; } = null;
    public string Barcde { get; set; } = null;
    public BchDtls BchDtls { get; set; }
    public double Qty { get; set; } = 0;
    public int FreeQty { get; set; } = 0;
    public string Unit { get; set; } = null;
    public double UnitPrice { get; set; } = 0;
    public double TotAmt { get; set; } = 0;
    public double Discount { get; set; } = 0;
    public int PreTaxVal { get; set; } = 0;
    public double AssAmt { get; set; } = 0;
    public int GstRt { get; set; } = 0;
    public double SgstAmt { get; set; } = 0;
    public double IgstAmt { get; set; } = 0;
    public double CgstAmt { get; set; } = 0;
    public int CesRt { get; set; } = 0;
    public double CesAmt { get; set; } = 0;
    public int CesNonAdvlAmt { get; set; } = 0;
    public int StateCesRt { get; set; } = 0;
    public double StateCesAmt { get; set; } = 0;
    public int StateCesNonAdvlAmt { get; set; } = 0;
    public double OthChrg { get; set; } = 0;
    public double TotItemVal { get; set; } = 0;
    public string OrdLineRef { get; set; } = null;
    public string OrgCntry { get; set; } = null;
    public string PrdSlNo { get; set; } = null;
    public System.Collections.Generic.List<AttribDtl> AttribDtls { get; set; }
}
public class ValDtls
{
    public double AssVal { get; set; } = 0;
    public double CgstVal { get; set; } = 0;
    public double SgstVal { get; set; } = 0;
    public double IgstVal { get; set; } = 0;
    public double CesVal { get; set; } = 0;
    public double StCesVal { get; set; } = 0;
    public double Discount { get; set; } = 0;
    public double OthChrg { get; set; } = 0;
    public double RndOffAmt { get; set; } = 0;
    public double TotInvVal { get; set; } = 0;
    public double TotInvValFc { get; set; } = 0;
}
public class PayDtls
{
    public string Nm { get; set; } = null;
    public string Accdet { get; set; } = null;
    public string Mode { get; set; } = null;
    public string Fininsbr { get; set; } = null;
    public string Payterm { get; set; } = null;
    public string Payinstr { get; set; } = null;
    public string Crtrn { get; set; } = null;
    public string Dirdr { get; set; } = null;
    public int Crday { get; set; } = 0;
    public int Paidamt { get; set; } = 0;
    public int Paymtdue { get; set; } = 0;
}
public class DocPerdDtls
{
    public string InvStDt { get; set; } = null;
    public string InvEndDt { get; set; } = null;
}
public class PrecDocDtl
{
    public string InvNo { get; set; } = null;
    public string InvDt { get; set; } = null;
    public string OthRefNo { get; set; } = null;
}
public class ContrDtl
{
    public string RecAdvRefr { get; set; } = null;
    public string RecAdvDt { get; set; } = null;
    public string Tendrefr { get; set; } = null;
    public string Contrrefr { get; set; } = null;
    public string Extrefr { get; set; } = null;
    public string Projrefr { get; set; } = null;
    public string Porefr { get; set; } = null;
    public string PoRefDt { get; set; } = null;
}
public class RefDtls
{
    public string InvRm { get; set; } = null;
    public DocPerdDtls DocPerdDtls { get; set; }
    public System.Collections.Generic.List<PrecDocDtl> PrecDocDtls { get; set; }
    public System.Collections.Generic.List<ContrDtl> ContrDtls { get; set; }
}
public class AddlDocDtl
{
    public string Url { get; set; } = null;
    public string Docs { get; set; } = null;
    public string Info { get; set; } = null;
}
public class ExpDtls
{
    public string ShipBNo { get; set; } = null;
    public string ShipBDt { get; set; } = null;
    public string Port { get; set; } = null;
    public string RefClm { get; set; } = null;
    public string ForCur { get; set; } = null;
    public string CntCode { get; set; } = null;
}
public class EwbDtls
{
    public string Transid { get; set; } = null;
    public string Transname { get; set; } = null;
    public int Distance { get; set; } = 0;
    public string Transdocno { get; set; } = null;
    public string TransdocDt { get; set; } = null;
    public string Vehno { get; set; } = null;
    public string Vehtype { get; set; } = null;
    public string TransMode { get; set; } = null;
}
public class CancelReqBody
{
    public string Irn { get; set; } = "";

    public string CnlRsn { get; set; } = null;

    //"Cancel Reason 1- Duplicate, 2 - Data entry mistake, 3- Order Cancelled, 4 - Others"
    public string CnlRem { get; set; } = "";
}
public class IrnReqBody
{

    public string Version { get; set; } = "1.1";
    public TranDtls TranDtls { get; set; } = new TranDtls();
    public DocDtls DocDtls { get; set; } = new DocDtls();
    public SellerDtls SellerDtls { get; set; } = new SellerDtls();
    public BuyerDtls BuyerDtls { get; set; } = new BuyerDtls();
    public DispDtls DispDtls { get; set; } = new DispDtls();
    public ShipDtls ShipDtls { get; set; } = new ShipDtls();
    public System.Collections.Generic.List<ItemList> ItemList { get; set; } = new System.Collections.Generic.List<ItemList>();
    public ValDtls ValDtls { get; set; } = new ValDtls();
    public PayDtls PayDtls { get; set; } = new PayDtls();
    public RefDtls RefDtls { get; set; } = new RefDtls();
    public System.Collections.Generic.List<AddlDocDtl> AddlDocDtls { get; set; } = new System.Collections.Generic.List<AddlDocDtl>();
    public ExpDtls ExpDtls { get; set; } = new ExpDtls();
    public EwbDtls EwbDtls { get; set; } = new EwbDtls();

}
public class IrnRespData
{
    public string AckNo { get; set; }
    public DateTime AckDt { get; set; }
    public string Irn { get; set; }
    public string SignedInvoice { get; set; }
    public string SignedQRCode { get; set; }
    public string Status { get; set; }
    public object EwbNo { get; set; }
    public object EwbDt { get; set; }
    public object EwbValidTill { get; set; }
    public object Remarks { get; set; }
}
public class IrnResp
{
    public IrnRespData data { get; set; }
    public string status_cd { get; set; }
    public string status_desc { get; set; }
}
public class CancelRespData
{
    public DateTime CancelDate { get; set; }
}
public class CancelResp
{
    public CancelRespData data { get; set; }
    public string status_cd { get; set; }
    public string status_desc { get; set; }
    public string InfoDtls { get; set; }
}
public class ErrorDesc
{
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}