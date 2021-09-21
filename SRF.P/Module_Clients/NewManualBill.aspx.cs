using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using SRF.P.DAL;

namespace SRF.P.Module_Clients
{
    public partial class NewManualBill : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string FontStyle = "Arial";
        string BranchID = "";

        AppConfiguration config = new AppConfiguration();

        protected void Page_Load(object sender, EventArgs e)
        {
            GetWebConfigdata();
            if (!IsPostBack)
            {
                LoadClientIDAndData();
                //month
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

                try
                {
                    LastMonth = monthName[DateTime.Now.Month - 4];
                }
                catch (IndexOutOfRangeException ex)
                {
                    LastMonth = monthName[12 - (4 - DateTime.Now.Month)];
                }
                ddlmonth.Items.Add(LastMonth);
                ClearData();
                ddlMBBillnos.Items.Insert(0, "--Select--");

                DataTable DtDesignation = GlobalData.Instance.LoadDesigns();
                List<string> list = DtDesignation.AsEnumerable()
                                   .Select(r => r.Field<string>("Design"))
                                   .ToList();
                var result = new JavaScriptSerializer().Serialize(list);
                hdDesignations.Value = result;

                FillDefaultGird();
            }

        }

        protected void LoadClientIDAndData()
        {
            string selectclientid = "select clientid from clients where ClientStatus=1 and clientid like '%" + CmpIDPrefix + "%' Order By right(clientid,4) ";
            DataTable dt = config.ExecuteReaderWithQueryAsync(selectclientid).Result;
            ddlmonth.Items.Add("--Select--");
            if (dt.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "clientid";
                ddlclientid.DataTextField = "clientid";
                ddlclientid.DataSource = dt;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "--Select--");
            dt = null;


            dt = config.ExecuteReaderWithQueryAsync("select ClientName,Clientid from clients where ClientStatus=1 and clientid like '%" + CmpIDPrefix + "%'  order by Clientname").Result;
            if (dt.Rows.Count > 0)
            {
                ddlCname.DataValueField = "clientid";
                ddlCname.DataTextField = "ClientName";
                ddlCname.DataSource = dt;
                ddlCname.DataBind();
            }
            ddlCname.Items.Insert(0, "--Select--");
        }

        protected void AddNewRow()
        {
            try
            {


                //DataTable dt = (DataTable)ViewState["DTDefaultManual"];
                //DataRow dr = dt.NewRow();
                //dr["Sid"] = dt.Rows.Count + 1;
                //dr["Designation"] = "";
                //dr["NoofEmps"] = 0;
                //dr["DutyHrs"] = 0;
                //dr["DutyHours"] = 0;
                //dr["payrate"] = 0;
                //dr["paytype"] = 0;
                //dr["BasicDa"] = 0;
                //dr["OTAmount"] = 0;
                //dr["NoOfDays"] = 1;
                //dr["Totalamount"] = 0;
                //dt.Rows.Add(dr);
                //gvClientBilling.DataSource = dt;
                //gvClientBilling.DataBind();
                //ScriptManager.RegisterStartupScript(this, GetType(), "bindautofilldesgs", "bindautofilldesgs();", true);




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
                            TextBox txtgvdesgn = (TextBox)gvClientBilling.Rows[rowIndex].Cells[1].FindControl("txtgvdesgn");
                            TextBox txtnoofemployees = (TextBox)gvClientBilling.Rows[rowIndex].Cells[2].FindControl("txtnoofemployees");
                            TextBox txtNoOfDuties = (TextBox)gvClientBilling.Rows[rowIndex].Cells[3].FindControl("txtNoOfDuties");
                            TextBox txtPayRate = (TextBox)gvClientBilling.Rows[rowIndex].Cells[4].FindControl("txtPayRate");
                            DropDownList ddldutytype = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[5].FindControl("ddldutytype");
                            DropDownList ddlnod = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[6].FindControl("ddlnod");
                            TextBox txtda = (TextBox)gvClientBilling.Rows[rowIndex].Cells[7].FindControl("txtda");
                            TextBox txtAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[8].FindControl("txtAmount");


                            drCurrentRow = dtCurrentTable.NewRow();
                            // drCurrentRow["Sid"] = i + 1;

                            dtCurrentTable.Columns["Designation"].AllowDBNull = true;
                            dtCurrentTable.Columns["NoofEmps"].AllowDBNull = true;
                            dtCurrentTable.Columns["NoofEmps"].ReadOnly = false;
                            dtCurrentTable.Columns["DutyHrs"].AllowDBNull = true;
                            dtCurrentTable.Columns["DutyHrs"].ReadOnly = false;
                            dtCurrentTable.Columns["payrate"].AllowDBNull = true;
                            dtCurrentTable.Columns["payrate"].ReadOnly = false;
                            dtCurrentTable.Columns["paytype"].AllowDBNull = true;
                            dtCurrentTable.Columns["paytype"].ReadOnly = false;
                            dtCurrentTable.Columns["NoOfDays"].AllowDBNull = true;
                            dtCurrentTable.Columns["NoOfDays"].ReadOnly = false;
                            dtCurrentTable.Columns["BasicDa"].AllowDBNull = true;
                            dtCurrentTable.Columns["BasicDa"].ReadOnly = false;
                            dtCurrentTable.Columns["Totalamount"].AllowDBNull = true;
                            dtCurrentTable.Columns["Totalamount"].ReadOnly = false;
                            dtCurrentTable.Columns["ServiceCharge"].AllowDBNull = true;
                            dtCurrentTable.Columns["ServiceCharge"].ReadOnly = false;
                            dtCurrentTable.Columns["BillDate"].AllowDBNull = true;


                            dtCurrentTable.Rows[i - 1]["Designation"] = txtgvdesgn.Text;
                            dtCurrentTable.Rows[i - 1]["NoofEmps"] = txtnoofemployees.Text.Trim() == "" ? 0 : Convert.ToInt32(txtnoofemployees.Text);
                            dtCurrentTable.Rows[i - 1]["DutyHours"] = txtNoOfDuties.Text.Trim() == "" ? 0 : Convert.ToSingle(txtNoOfDuties.Text);
                            dtCurrentTable.Rows[i - 1]["payrate"] = txtPayRate.Text.Trim() == "" ? 0 : Convert.ToSingle(txtPayRate.Text);
                            dtCurrentTable.Rows[i - 1]["paytype"] = ddldutytype.SelectedValue;
                            dtCurrentTable.Rows[i - 1]["NoOfDays"] = ddlnod.SelectedValue;
                            dtCurrentTable.Rows[i - 1]["BasicDa"] = txtda.Text.Trim() == "" ? 0 : Convert.ToSingle(txtda.Text);
                            dtCurrentTable.Rows[i - 1]["Totalamount"] = txtAmount.Text.Trim() == "" ? 0 : Convert.ToSingle(txtAmount.Text);


                            rowIndex++;
                        }
                        dtCurrentTable.Rows.Add(drCurrentRow);
                        ViewState["DTDefaultManual"] = dtCurrentTable;

                        gvClientBilling.DataSource = dtCurrentTable;
                        gvClientBilling.DataBind();
                    }
                }
                else
                {
                    Response.Write("ViewState is null");
                }
                SetPreviousData();

            }
            catch (Exception ex)
            {

            }
        }

        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["DTDefaultManual"] != null)
            {
                DataTable dt = (DataTable)ViewState["DTDefaultManual"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox txtgvdesgn = (TextBox)gvClientBilling.Rows[rowIndex].Cells[1].FindControl("txtgvdesgn");
                        TextBox txtnoofemployees = (TextBox)gvClientBilling.Rows[rowIndex].Cells[2].FindControl("txtnoofemployees");
                        TextBox txtNoOfDuties = (TextBox)gvClientBilling.Rows[rowIndex].Cells[3].FindControl("txtNoOfDuties");
                        TextBox txtPayRate = (TextBox)gvClientBilling.Rows[rowIndex].Cells[4].FindControl("txtPayRate");
                        DropDownList ddldutytype = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[5].FindControl("ddldutytype");
                        DropDownList ddlnod = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[6].FindControl("ddlnod");
                        TextBox txtda = (TextBox)gvClientBilling.Rows[rowIndex].Cells[7].FindControl("txtda");
                        TextBox txtAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[8].FindControl("txtAmount");


                        txtgvdesgn.Text = dt.Rows[i]["Designation"].ToString();
                        txtnoofemployees.Text = dt.Rows[i]["NoofEmps"].ToString();
                        txtNoOfDuties.Text = dt.Rows[i]["DutyHours"].ToString();
                        txtPayRate.Text = dt.Rows[i]["payrate"].ToString();
                        ddldutytype.SelectedValue = dt.Rows[i]["paytype"].ToString();
                        ddlnod.SelectedValue = dt.Rows[i]["NoOfDays"].ToString();
                        txtda.Text = dt.Rows[i]["BasicDa"].ToString();
                        txtAmount.Text = dt.Rows[i]["Totalamount"].ToString();

                        rowIndex++;
                    }
                }
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
            ddlmonth.Items.Insert(0, "-select-");
        }

        protected void GetWebConfigdata()
        {
            if (Session.Keys.Count > 0)
            {
                EmpIDPrefix = Session["EmpIDPrefix"].ToString();
                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

        }

        protected void ClearData()
        {
            lblTotalResources.Text = "0";
            lblServiceTax.Text = "0";
            lblGrandTotal.Text = "0";
            lblCESS.Text = "0";
            lblSBCESS.Text = "0";
            lblKKCESS.Text = "0";
            lblSheCESS.Text = "0";
            lblSubTotal.Text = "0";
            lblServiceCharges.Text = "0";
            Txtservicechrg.Text = "0";
        }

        protected void DisplayDataInGrid()
        {
            #region Variable Declaration
            ClearData();
            int month = 0;



            #endregion

            #region  Select Month

            month = GetMonthBasedOnSelectionDateorMonth();

            string year = "";
            string monval = "";

            if (month.ToString().Length == 3)
            {
                monval = month.ToString().Substring(0, 1);
                year = "20" + month.ToString().Substring(1, 2);
            }
            else
            {
                monval = month.ToString().Substring(0, 2);
                year = "20" + month.ToString().Substring(2, 2);
            }

            int noOfDaysInMonth = DateTime.DaysInMonth(int.Parse(year), int.Parse(monval));

            //if (ddlmonth.SelectedIndex == 1)
            //{
            //    month = GlobalData.Instance.GetIDForNextMonth();
            //}
            //else if (ddlmonth.SelectedIndex == 2)
            //{
            //    month = GlobalData.Instance.GetIDForThisMonth();
            //}
            //else
            //{
            //    month = GlobalData.Instance.GetIDForPrviousMonth();
            //}
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
                             ISNULL(cd.Servicecharge,0) as ServiceCharge,
                             cd.NoOfDays,
                             mub.ServiceChrg as ServiceChrg,
                             mub.BillDt as BillDate,mub.BillType as BillType
                               
                    from MUnitBillBreakup as Ubb 
                    inner join Contracts cd on cd.ClientID = ubb.UnitId inner join MUnitBill mub on Ubb.UnitId=mub.UnitId and Ubb.MunitidBillno=mub.Billno
                    where Ubb.unitid ='" + ddlclientid.SelectedValue + "' and Ubb.month=" + month + " and  Ubb.MunitidBillno='" + ddlMBBillnos.SelectedValue
                        + "' and cd.contractid='" + ContractID + "' order by ubb.sino";

            //Group by  Ubb.UnitId, Ubb.Designation,Ubb.BasicDA, Ubb.NoofEmps,Ubb.DutyHours,Ubb.monthlydays,Ubb.PayRate,Ubb.PayRateType,Ubb.DutyHours,Ubb.otamount,Ubb.Remarks,Ubb.Description,cd.NoOfDays, mub.ServiceChrg,mub.BillDt,cd.ServiceCharge,Ubb.Totalamount";

            DataTable DtForUBB = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;


            var SumTotal = @"select sum(isnull(Totalamount,0)) as Total from  MUnitBillBreakup where unitid ='" + ddlclientid.SelectedValue + "' and month=" + month + " and MunitidBillno='" + ddlMBBillnos.SelectedValue
                        + "'";

            DataTable DtForSumUBB = config.ExecuteAdaptorAsyncWithQueryParams(SumTotal).Result;


            if (DtForUBB.Rows.Count > 0)
            {

                gvClientBilling.DataSource = DtForUBB;
                gvClientBilling.DataBind();
                for (int i = 0; i < DtForUBB.Rows.Count; i++)
                {
                    DropDownList Nods = gvClientBilling.Rows[i].FindControl("ddlnod") as DropDownList;

                    if (Nods != null)
                    {



                        int noofdays = int.Parse(DtForUBB.Rows[i]["monthlydays"].ToString());
                        Nods.SelectedValue = DtForUBB.Rows[i]["monthlydays"].ToString();

                    }


                    DropDownList Dtype = gvClientBilling.Rows[i].FindControl("ddldutytype") as DropDownList;

                    if (Dtype != null)
                    {



                        int amt = int.Parse(DtForUBB.Rows[i]["PayType"].ToString());
                        Dtype.SelectedValue = DtForUBB.Rows[i]["PayType"].ToString();

                    }
                }
                ViewState["DTDefaultManual"] = DtForUBB;

                //  lblServiceCharges.Text=DtForUBB.Rows[0]["ServiceChrg"].ToString();
                if (DtForSumUBB.Rows.Count > 0)
                {
                    lblTotalResources.Text = DtForSumUBB.Rows[0]["Total"].ToString();
                }
                // txtbilldate.Text=
                txtremarks.Text = DtForUBB.Rows[0]["Remarks"].ToString();
                txtdescription.Text = DtForUBB.Rows[0]["Description"].ToString();

                #region    Retrive Data From munitbill  table data based on the bill no

                string SqlQryForunitbill = "Select *,convert(varchar(10),BillDt,103) as Billdate,convert(varchar(10),FromDt,103) as FromDate,convert(varchar(10),ToDt,103) as ToDate,isnull(BillType,0) as BillType from munitbill   Where  unitid='" + ddlclientid.SelectedValue +
                                           "'  and  Month='" + month + "'  and billno='" + ddlMBBillnos.SelectedValue + "'";

                DataTable DtForUnitBill = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForunitbill).Result;
                if (DtForUnitBill.Rows.Count > 0)
                {
                    System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-GB");
                    //DateTime dtb = Convert.ToDateTime(DtForUnitBill.Rows[0]["BillDt"].ToString(), enGB);
                    // string billdate = dtb.ToString("dd/MM/yyyy");

                    if (lblServiceTax.Text != "0")
                    {
                        lblServiceTaxTitle.Visible = true;
                        lblServiceTax.Visible = true;
                        lblSBCESSTitle.Visible = true;
                        lblSBCESS.Visible = true;
                        lblKKCESSTitle.Visible = true;
                        lblKKCESS.Visible = true;
                        lblCESSTitle.Visible = true;
                        lblCESS.Visible = true;
                        lblSheCESSTitle.Visible = true;
                        lblSheCESS.Visible = true;
                    }
                    else
                    {
                        lblServiceTaxTitle.Visible = false;
                        lblServiceTax.Visible = false;
                        lblSBCESSTitle.Visible = false;
                        lblSBCESS.Visible = false;
                        lblKKCESSTitle.Visible = false;
                        lblKKCESS.Visible = false;
                        lblCESSTitle.Visible = false;
                        lblCESS.Visible = false;
                        lblSheCESSTitle.Visible = false;
                        lblSheCESS.Visible = false;

                    }

                    txtBankname.Text = DtForUnitBill.Rows[0]["BankName"].ToString();
                    txtBankAccNo.Text = DtForUnitBill.Rows[0]["BankAccountNo"].ToString();
                    txtifsccode.Text = DtForUnitBill.Rows[0]["IFSCCode"].ToString();
                    string billdate = DtForUnitBill.Rows[0]["Billdate"].ToString();
                    txtbilldate.Text = billdate;
                    string BillType = DtForUnitBill.Rows[0]["BillType"].ToString();
                    if (BillType == "M")
                    {
                        ddltype.SelectedIndex = 0;
                    }
                    else if (BillType == "A")
                    {
                        ddltype.SelectedIndex = 1;
                    }
                    else if (BillType == "B")
                    {
                        ddltype.SelectedIndex = 2;
                    }
                    else if (BillType == "E")
                    {
                        ddltype.SelectedIndex = 3;
                    }

                    txtfromdate.Text = DtForUnitBill.Rows[0]["FromDate"].ToString();
                    txttodate.Text = DtForUnitBill.Rows[0]["ToDate"].ToString();

                    lblbillnolatest.Text = DtForUnitBill.Rows[0]["BillNo"].ToString();
                    Txtservicechrg.Text = DtForUnitBill.Rows[0]["ServiceChrgPer"].ToString();
                    lblSubTotal.Text = DtForUnitBill.Rows[0]["Subtotal"].ToString();
                    lblServiceCharges.Text = DtForUnitBill.Rows[0]["ServiceChrg"].ToString();
                    lblTotalResources.Text = DtForUnitBill.Rows[0]["TotalChrg"].ToString();
                    lblServiceTax.Text = DtForUnitBill.Rows[0]["ServiceTax"].ToString();
                    lblSBCESS.Text = DtForUnitBill.Rows[0]["SBCessAmt"].ToString();
                    lblKKCESS.Text = DtForUnitBill.Rows[0]["KKCessAmt"].ToString();
                    lblCESS.Text = DtForUnitBill.Rows[0]["CESS"].ToString();
                    lblSheCESS.Text = DtForUnitBill.Rows[0]["SHECESS"].ToString();
                    lblGrandTotal.Text = DtForUnitBill.Rows[0]["GrandTotal"].ToString();

                    #region for GST as on 17-6-2017 by sharada


                    float cgstamt = 0;
                    float sgsamt = 0;
                    float igstamt = 0;
                    float cell1amt = 0;
                    float cess2amt = 0;
                    TxtCGSTPrc.Text = DtForUnitBill.Rows[0]["CGSTPrc"].ToString();
                    TxtSGSTPrc.Text = DtForUnitBill.Rows[0]["SGSTPrc"].ToString();
                    TxtIGSTPrc.Text = DtForUnitBill.Rows[0]["IGSTPrc"].ToString();
                    TxtCess1Prc.Text = DtForUnitBill.Rows[0]["Cess1Prc"].ToString();
                    TxtCess2Prc.Text = DtForUnitBill.Rows[0]["Cess2Prc"].ToString();

                    lblCGST.Text = DtForUnitBill.Rows[0]["CGSTAmt"].ToString();
                    cgstamt = float.Parse(DtForUnitBill.Rows[0]["CGSTAmt"].ToString());
                    if (cgstamt > 0)
                    {
                        lblCGSTTitle.Visible = true;
                        TxtCGSTPrc.Visible = true;
                        lblCGST.Visible = true;
                    }

                    lblSGST.Text = DtForUnitBill.Rows[0]["SGSTAmt"].ToString();
                    sgsamt = float.Parse(DtForUnitBill.Rows[0]["SGSTAmt"].ToString());
                    if (sgsamt > 0)
                    {
                        lblSGSTTitle.Visible = true;
                        TxtSGSTPrc.Visible = true;
                        lblSGST.Visible = true;
                    }
                    lblIGST.Text = DtForUnitBill.Rows[0]["IGSTAmt"].ToString();
                    igstamt = float.Parse(DtForUnitBill.Rows[0]["IGSTAmt"].ToString());
                    if (igstamt > 0)
                    {
                        lblIGSTTitle.Visible = true;
                        TxtIGSTPrc.Visible = true;
                        lblIGST.Visible = true;
                    }

                    lblCess1.Text = DtForUnitBill.Rows[0]["Cess1Amt"].ToString();
                    lblCess2.Text = DtForUnitBill.Rows[0]["Cess2Amt"].ToString();


                    #endregion for GST as on 16-6-2017 by sharada


                    //lblServiceTaxTitle.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    //lblServiceTax.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    //lblSBCESSTitle.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    //lblSBCESS.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    //lblKKCESSTitle.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    //lblKKCESS.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    //lblCESSTitle.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    //lblCESS.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    //lblSheCESSTitle.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    //lblSheCESS.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    //lblGrandTotal.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
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

            //if (ddlmonth.SelectedIndex == 1)
            //{
            //    month = GlobalData.Instance.GetIDForNextMonth();
            //}
            //else if (ddlmonth.SelectedIndex == 2)
            //{
            //    month = GlobalData.Instance.GetIDForThisMonth();
            //}
            //else
            //{
            //    month = GlobalData.Instance.GetIDForPrviousMonth();
            //}


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

        protected void btninvoice_Click(object sender, EventArgs e)
        {
            int month = 0;
            month = GetMonthBasedOnSelectionDateorMonth();
            if (gvClientBilling.Rows.Count > 0)
            {
                //try
                //{
                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string strQry = "Select * from CompanyInfo   where   ClientidPrefix='" + CmpIDPrefix + "'";
                DataTable compInfo = config.ExecuteReaderWithQueryAsync(strQry).Result;
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
                DataTable DtServicecharge = config.ExecuteReaderWithQueryAsync(SqlQuryForServiCharge).Result;
                string ServiceCharge = "0";
                string strSCType = "";
                string strDescription = "";
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
                    // string tempDescription = .Rows[0]["Description"].ToString();
                    //if (tempDescription.Trim().Length > 0)
                    //{
                    //    //strDescription = tempDescription;
                    //}
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
                string imagepath = Server.MapPath("~/assets/BillLogo.png");
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

                string FontStyle = "Verdana";

                PdfPCell celll = new PdfPCell(new Paragraph(" ", FontFactory.GetFont(FontStyle, 12, Font.NORMAL, BaseColor.BLACK)));
                celll.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                celll.Border = 0;
                celll.Colspan = 2;


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

                CCompAddress.SetLeading(0f, 1.3f);
                tablelogo.AddCell(CCompAddress);

                PdfPCell cellline = new PdfPCell(new Paragraph(companyaddressline, FontFactory.GetFont(FontStyle, 12, Font.NORMAL, BaseColor.BLACK)));
                cellline.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cellline.Border = 0;
                cellline.Colspan = 2;
                tablelogo.AddCell(cellline);
                //For Space

                tablelogo.AddCell(celll);

                PdfPCell CInvoice = new PdfPCell(new Paragraph("TAX INVOICE", FontFactory.GetFont(FontStyle, 18, Font.UNDERLINE | Font.BOLD, BaseColor.BLACK)));
                CInvoice.HorizontalAlignment = 1;
                CInvoice.Border = 0;
                CInvoice.Colspan = 2;
                tablelogo.AddCell(CInvoice);

                tablelogo.AddCell(celll);

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

                string Invoicedesc = "select Description,Remarks from MUnitBillBreakup where UnitId='" + ddlclientid.SelectedItem.ToString() + "' and MunitIdBillno='" + ddlMBBillnos.SelectedValue + "' and  month='" + month + "'";
                DataTable dtinvoicedesc = config.ExecuteReaderWithQueryAsync(Invoicedesc).Result;
                string tempDescription = dtinvoicedesc.Rows[0]["Description"].ToString();
                string Remarks = dtinvoicedesc.Rows[0]["Remarks"].ToString();

                if (tempDescription.Trim().Length > 0)
                {
                    strDescription = tempDescription;
                }


                string selectclientaddress = "select * from clients where clientid= '" + ddlclientid.SelectedItem.ToString() + "'";
                DataTable dtclientaddress = config.ExecuteReaderWithQueryAsync(selectclientaddress).Result;

                string SelectBillNo = "Select * from MUnitBill where BillNo= '" + ddlMBBillnos.SelectedValue + "' and  month='" + month + "' and unitid='" + ddlclientid.SelectedValue + "' ";
                DataTable DtBilling = config.ExecuteReaderWithQueryAsync(SelectBillNo).Result;
                string BillNo = "";
                DateTime BillDate;
                DateTime DueDate;

                #region Variables for data Fields as on 11/03/2014 by venkat


                float servicecharge = 0;
                float servicechargePer = 0;
                float servicetax = 0;
                float sbcess = 0;
                float kkcess = 0;
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



                #endregion

                if (DtBilling.Rows.Count > 0)
                {
                    BillNo = DtBilling.Rows[0]["billno"].ToString();
                    BillDate = Convert.ToDateTime(DtBilling.Rows[0]["billdt"].ToString());
                    // DueDate = Convert.ToDateTime(DtBilling.Rows[0]["duedt"].ToString());

                    #region Begin New code for values taken from database as on 11/03/2014 by venkat

                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["TotalChrg"].ToString()) == false)
                    {
                        totalamount = float.Parse(DtBilling.Rows[0]["TotalChrg"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceChrg"].ToString()) == false)
                    {
                        servicecharge = float.Parse(DtBilling.Rows[0]["ServiceChrg"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceChrgPer"].ToString()) == false)
                    {
                        servicechargePer = float.Parse(DtBilling.Rows[0]["ServiceChrgPer"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax"].ToString()) == false)
                    {
                        servicetax = float.Parse(DtBilling.Rows[0]["ServiceTax"].ToString());
                    }

                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["SBCessAmt"].ToString()) == false)
                    {
                        sbcess = float.Parse(DtBilling.Rows[0]["SBCessAmt"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["KKCessAmt"].ToString()) == false)
                    {
                        kkcess = float.Parse(DtBilling.Rows[0]["KKCessAmt"].ToString());
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


                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["MachinaryCost"].ToString()) == false)
                    {
                        machinarycost = float.Parse(DtBilling.Rows[0]["MachinaryCost"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["MaterialCost"].ToString()) == false)
                    {
                        materialcost = float.Parse(DtBilling.Rows[0]["MaterialCost"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ElectricalChrg"].ToString()) == false)
                    {
                        maintenancecost = float.Parse(DtBilling.Rows[0]["ElectricalChrg"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraAmtone"].ToString()) == false)
                    {
                        extraonecost = float.Parse(DtBilling.Rows[0]["ExtraAmtone"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraAmtTwo"].ToString()) == false)
                    {
                        extratwocost = float.Parse(DtBilling.Rows[0]["ExtraAmtTwo"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["Discount"].ToString()) == false)
                    {
                        discountone = float.Parse(DtBilling.Rows[0]["Discount"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["Discounttwo"].ToString()) == false)
                    {
                        discounttwo = float.Parse(DtBilling.Rows[0]["Discounttwo"].ToString());
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
                    //    ServiceTax75 = float.Parse(DtBilling.Rows[0]["ServiceTax75"].ToString());
                    //}

                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax25"].ToString()) == false)
                    //{
                    //    ServiceTax25 = float.Parse(DtBilling.Rows[0]["ServiceTax25"].ToString());
                    //}

                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["Staxonservicecharge"].ToString()) == false)
                    //{
                    //    staxamtonservicecharge = float.Parse(DtBilling.Rows[0]["Staxonservicecharge"].ToString());
                    //}

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
                //addressData = dtclientaddress.Rows[0]["clientname"].ToString();
                //if (addressData.Trim().Length > 0)
                //{
                //    PdfPCell clientname = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 12, Font.BOLD, BaseColor.BLACK)));
                //    clientname.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //    clientname.Border = 0;
                //    tempTable1.AddCell(clientname);
                //}
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


                PdfPCell cell12 = new PdfPCell(new Paragraph("Invoice No: " + BillNo, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell12.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell12.Border = 0;

                tempTable2.AddCell(cell12);
                PdfPCell cell13 = new PdfPCell(new Paragraph("Date: " + BillDate.Day.ToString("00") + "/" + BillDate.ToString("MM") + "/" +
                    BillDate.Year, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell13.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell13.Border = 0;
                tempTable2.AddCell(cell13);


                PdfPCell cell14 = new PdfPCell(new Paragraph("For Month: " +
                    GetMonthName() + " - " + GetMonthOfYear() +
                    "      ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell14.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell14.Border = 0;
                tempTable2.AddCell(cell14);

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

                PdfPCell cell9 = new PdfPCell(new Phrase("Unit Name : " + dtclientaddress.Rows[0]["clientname"].ToString(),
                    FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell9.Colspan = 2;
                cell9.Border = 0;
                bodytablelogo.AddCell(cell9);

                string Fromdate = txtfromdate.Text;
                string Todate = txttodate.Text;

                PdfPCell cell10 = new PdfPCell(new Phrase("Bill period : " + Fromdate + "  to  " +
                    Todate + " ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell10.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell10.Colspan = 2;
                cell10.Border = 0;
                bodytablelogo.AddCell(cell10);
                bodytablelogo.AddCell(celll);

                PdfPCell cell19 = new PdfPCell(new Phrase("Dear Sir, ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell19.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell19.Colspan = 2;
                cell19.Border = 0;
                bodytablelogo.AddCell(cell19);
                bodytablelogo.AddCell(celll);

                PdfPCell cell20 = new PdfPCell(new Phrase(strDescription, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell20.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell20.Colspan = 2;
                cell20.Border = 0;
                bodytablelogo.AddCell(cell20);
                bodytablelogo.AddCell(celll);
                PdfPCell cell21 = new PdfPCell(new Phrase("The Details are given below : ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell21.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell21.Colspan = 1;
                cell21.Border = 0;
                bodytablelogo.AddCell(cell21);
                bodytablelogo.AddCell(celll);
                // bodytablelogo.AddCell(celll);
                document.Add(bodytablelogo);
                int colCount = 6;// gvClientBilling.Columns.Count;
                //Create a table

                PdfPTable table = new PdfPTable(colCount);
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                table.HorizontalAlignment = 1;

                //create an array to store column widths
                // int[] colWidths = new int[5];
                float[] colWidths = new float[] { 4.6f, 1.2f, 1.8f, 1.2f, 1.2f, 2.4f };
                table.SetWidths(colWidths);
                PdfPCell cell;
                string cellText;
                //create the header row
                for (int colIndex = 1; colIndex < 7; colIndex++)
                {
                    //set the column width
                    if (colIndex < 3)
                    {
                        // colWidths[colIndex] = (int)gvClientBilling.Columns[colIndex].ItemStyle.Width.Value;
                        //fetch the header text
                        cellText = Server.HtmlDecode(gvClientBilling.HeaderRow.Cells[colIndex].Text);
                        //create a new cell with header text
                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        //set the background color for the header cell
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);
                    }
                    if (colIndex == 3)
                    {
                        // colWidths[colIndex] = (int)gvClientBilling.Columns[colIndex].ItemStyle.Width.Value;
                        //fetch the header text
                        cellText = Server.HtmlDecode(gvClientBilling.HeaderRow.Cells[colIndex].Text);
                        //create a new cell with header text
                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        //set the background color for the header cell
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);
                    }
                    if (colIndex == 4)
                    {
                        // colWidths[colIndex] = (int)gvClientBilling.Columns[colIndex].ItemStyle.Width.Value;
                        //fetch the header text
                        cellText = Server.HtmlDecode(gvClientBilling.HeaderRow.Cells[4].Text);
                        //create a new cell with header text
                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        //set the background color for the header cell
                        cell.HorizontalAlignment = 1;
                        cell.Colspan = 2;
                        table.AddCell(cell);
                    }

                    if (colIndex == 6)
                    {
                        // colWidths[colIndex] = (int)gvClientBilling.Columns[colIndex].ItemStyle.Width.Value;
                        //fetch the header text
                        cellText = Server.HtmlDecode(gvClientBilling.HeaderRow.Cells[7].Text);
                        //create a new cell with header text
                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        //set the background color for the header cell
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);
                    }

                }
                ////export rows from GridView to table
                for (int rowIndex = 0; rowIndex < gvClientBilling.Rows.Count; rowIndex++)
                {
                    if (gvClientBilling.Rows[rowIndex].RowType == DataControlRowType.DataRow)
                    {
                        DropDownList ddldutytype = (DropDownList)(gvClientBilling.Rows[rowIndex].FindControl("ddldutytype"));
                        TextBox lblamount = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtAmount"));
                        if (lblamount != null)
                        {
                            string dtype = ddldutytype.Text;
                            string strAmount = lblamount.Text;
                            float amount = 0;
                            if (strAmount.Length > 0)
                                amount = Convert.ToSingle(strAmount);
                            if (amount >= 0)
                            {
                                for (int j = 1; j < 7; j++)
                                {
                                    //fetch the column value of the current row
                                    if (j == 1)
                                    {
                                        TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtgvdesgn"));

                                        string summaryQry = "select summary from contractdetails " +
                                            "  where clientid='" + ddlclientid.SelectedValue + "' and Designations='" + label1.Text + "'";
                                        DataTable dt = config.ExecuteReaderWithQueryAsync(summaryQry).Result;
                                        cellText = label1.Text;
                                        if (dt.Rows.Count > 0)
                                        {
                                            if (dt.Rows[0]["summary"].ToString().Trim().Length > 0)
                                                cellText += " (" + dt.Rows[0]["summary"].ToString() + ")";
                                        }

                                        //create a new cell with column value
                                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        table.AddCell(cell);
                                    }

                                    if (j == 2)
                                    {
                                        TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtnoofemployees"));
                                        if (label1.Text == "0")
                                        {
                                            cellText = "";
                                        }
                                        else
                                        {
                                            cellText = label1.Text;
                                        }
                                        //create a new cell with column value
                                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        table.AddCell(cell);
                                    }
                                    if (j == 3)
                                    {
                                        TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtNoOfDuties"));
                                        cellText = label1.Text;
                                        //create a new cell with column value
                                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        table.AddCell(cell);
                                    }
                                    if (j == 4)
                                    {
                                        TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtPayRate"));
                                        cellText = label1.Text;

                                        if (cellText == "0")
                                        {
                                            cellText = "";
                                        }

                                        //create a new cell with column value
                                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 2;
                                        cell.Colspan = 2;
                                        table.AddCell(cell);
                                    }
                                    //if (j == 5)
                                    //{
                                    //    DropDownList label1 = (DropDownList)(gvClientBilling.Rows[rowIndex].FindControl("ddldutytype"));
                                    //    cellText = label1.SelectedItem.Text;
                                    //    //create a new cell with column value
                                    //    cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    //    cell.HorizontalAlignment = 2;
                                    //    table.AddCell(cell);
                                    //}
                                    if (j == 6)
                                    {
                                        TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtAmount"));
                                        cellText = label1.Text;

                                        if (cellText == "0")
                                        {
                                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 2;
                                            table.AddCell(cell);
                                        }
                                        else
                                        {
                                            cell = new PdfPCell(new Phrase(float.Parse(cellText).ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 2;
                                            table.AddCell(cell);
                                        }
                                        //create a new cell with column value

                                    }
                                }
                            }
                        }
                    }
                }
                document.Add(table);

                tablelogo.AddCell(celll);

                PdfPTable tabled = new PdfPTable(colCount);
                tabled.TotalWidth = 500;//432f;
                tabled.LockedWidth = true;
                float[] widthd = new float[] { 4.6f, 1.2f, 1.8f, 1.2f, 1.2f, 2.4f };
                tabled.SetWidths(widthd);

                PdfPCell celldz1 = new PdfPCell(new Phrase("Total ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                celldz1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                celldz1.Colspan = 5;
                tabled.AddCell(celldz1);

                PdfPCell celldz4 = new PdfPCell(new Phrase(" " + totalamount.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                celldz4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                tabled.AddCell(celldz4);

                string SqlQryForTaxes = "select * from  Tbloptions ";
                DataTable DtTaxes = config.ExecuteReaderWithQueryAsync(SqlQryForTaxes).Result;
                string SCPersent = "";
                if (DtTaxes.Rows.Count > 0)
                {
                    SCPersent = DtTaxes.Rows[0]["ServiceTaxSeparate"].ToString();
                }
                else
                {
                    lblResult.Text = "There Is No Tax Values For Generating Bills ";
                    return;
                }
                if (servicecharge > 0)//bSCType == true)
                {
                    float scharge = servicecharge;
                    if (scharge > 0)
                    {
                        PdfPCell celldc2 = new PdfPCell(new Phrase("Service Charges @ ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldc2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        celldc2.Colspan = 4;
                        tabled.AddCell(celldc2);

                        string SCharge = "";
                        if (bSCType == false)
                        {
                            SCharge = servicechargePer.ToString() + " %";
                        }
                        else
                        {
                            SCharge = servicechargePer.ToString();
                        }
                        PdfPCell celldc3 = new PdfPCell(new Phrase(SCharge, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldc3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        tabled.AddCell(celldc3);

                        PdfPCell celldc4 = new PdfPCell(new Phrase(servicecharge.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldc4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        tabled.AddCell(celldc4);
                    }
                }

                #region When Extra data is checked and STcheck is true


                if (Extradatacheck == true)
                {
                    //float machineryCostwithst = 0;
                    //if (lblMachinerywithst.Text.Length > 0)
                    //    machineryCostwithst = Convert.ToSingle(lblMachinerywithst.Text);
                    if (ExtraDataSTcheck == true)
                    {
                        if (machinarycost > 0)
                        {
                            if (STMachinary == true)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 6;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }

                        //float materialcostwithst = 0;
                        //if (lblMaterialwithst.Text.Length > 0)
                        //    materialcostwithst = Convert.ToSingle(lblMaterialwithst.Text);
                        if (materialcost > 0)
                        {
                            if (STMaterial == true)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 6;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }


                        //float electricalcostwithst = 0;
                        //if (lblElectricalwithst.Text.Length > 0)
                        //    electricalcostwithst = Convert.ToSingle(lblElectricalwithst.Text);
                        if (maintenancecost > 0)
                        {
                            if (STMaintenance == true)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 6;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }

                        //float extraamtwithst = 0;
                        //if (lblextraonewithst.Text.Length > 0)
                        //    extraamtwithst = Convert.ToSingle(lblextraonewithst.Text);
                        if (extraonecost > 0)
                        {
                            if (STExtraone == true)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 6;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }
                        //float Extraamtwithst1 = 0;
                        //if (lblextratwowithst.Text.Length > 0)
                        //    Extraamtwithst1 = Convert.ToSingle(lblextratwowithst.Text);
                        if (extratwocost > 0)
                        {
                            if (STExtratwo == true)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 6;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }
                    }

                }

                #endregion


                if (servicetax > 0)
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

                    PdfPCell celldd2 = new PdfPCell(new Phrase("Service Tax @ " + scpercent + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    celldd2.Colspan = 5;
                    tabled.AddCell(celldd2);

                    PdfPCell celldd4 = new PdfPCell(new Phrase(servicetax.ToString("#,##"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    tabled.AddCell(celldd4);


                    if (sbcess > 0)
                    {

                        string SBCESSPresent = DtTaxes.Rows[0]["sbcess"].ToString();
                        PdfPCell cellde2 = new PdfPCell(new Phrase("Swachh Bharat Cess @ " + SBCESSPresent + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        cellde2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        cellde2.Colspan = 5;
                        tabled.AddCell(cellde2);



                        PdfPCell cellde4 = new PdfPCell(new Phrase(sbcess.ToString("#,##"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        cellde4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        tabled.AddCell(cellde4);

                    }


                    if (kkcess > 0)
                    {

                        string KKCESSPresent = DtTaxes.Rows[0]["KKcess"].ToString();
                        PdfPCell cellde2 = new PdfPCell(new Phrase("Krishi Kalyan Cess @ " + KKCESSPresent + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        cellde2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        cellde2.Colspan = 5;
                        tabled.AddCell(cellde2);


                        PdfPCell cellde4 = new PdfPCell(new Phrase(kkcess.ToString("#,##"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        cellde4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        tabled.AddCell(cellde4);

                    }

                    if (cess > 0)
                    {

                        string CESSPresent = DtTaxes.Rows[0]["Cess"].ToString();
                        PdfPCell cellde2 = new PdfPCell(new Phrase("CESS @ " + CESSPresent + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        cellde2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        cellde2.Colspan = 5;
                        tabled.AddCell(cellde2);


                        PdfPCell cellde4 = new PdfPCell(new Phrase(cess.ToString("#,##"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        cellde4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        tabled.AddCell(cellde4);

                    }

                    if (shecess > 0)
                    {
                        string SHECESSPresent = DtTaxes.Rows[0]["shecess"].ToString();
                        PdfPCell celldf2 = new PdfPCell(new Phrase("S&H Ed.CESS @ " + SHECESSPresent + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldf2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        celldf2.Colspan = 4;
                        tabled.AddCell(celldf2);




                        PdfPCell celldf4 = new PdfPCell(new Phrase((servicetax * (double.Parse(SHECESSPresent) / 100)).ToString("0.00"),
                            FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldf4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        tabled.AddCell(celldf4);
                    }

                }
                //if (bST75)
                //{
                //    if (ServiceTax75 > 0)
                //    {
                //        PdfPCell celldMeci1 = new PdfPCell(new Phrase("Less 75% Service Tax ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                //        celldMeci1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                //        //celld7.Border = 1;
                //        celldMeci1.Colspan = 4;
                //        tabled.AddCell(celldMeci1);

                //        PdfPCell celldMeci3 = new PdfPCell(new Phrase(ServiceTax75.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                //        celldMeci3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                //        tabled.AddCell(celldMeci3);
                //    }
                //    if (ServiceTax25 > 0)
                //    {

                //        PdfPCell cellST25h = new PdfPCell(new Phrase("Service Tax Chargable @3.09% ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                //        cellST25h.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                //        //celld7.Border = 1;
                //        cellST25h.Colspan = 4;
                //        tabled.AddCell(cellST25h);

                //        PdfPCell cellST25d = new PdfPCell(new Phrase(ServiceTax25.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                //        cellST25d.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                //        tabled.AddCell(cellST25d);
                //    }


                //}

                #region When Extradata check is false and STcheck is false


                if (Extradatacheck == true)
                {
                    if (ExtraDataSTcheck == false)
                    {
                        if (machinarycost > 0)
                        {
                            PdfPCell celldMeci1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMeci1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldMeci1.Colspan = 5;
                            tabled.AddCell(celldMeci1);

                            PdfPCell celldMeci3 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMeci3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            tabled.AddCell(celldMeci3);
                        }

                        if (materialcost > 0)
                        {
                            PdfPCell celldMt1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldMt1.Colspan = 5;
                            tabled.AddCell(celldMt1);

                            PdfPCell celldMt3 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            tabled.AddCell(celldMt3);
                        }
                        if (maintenancecost > 0)
                        {
                            PdfPCell celldMt1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldMt1.Colspan = 5;
                            tabled.AddCell(celldMt1);

                            PdfPCell celldMt3 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            tabled.AddCell(celldMt3);
                        }

                        if (extraonecost > 0)
                        {
                            PdfPCell celldMt1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldMt1.Colspan = 5;
                            tabled.AddCell(celldMt1);

                            PdfPCell celldMt3 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            tabled.AddCell(celldMt3);
                        }
                        if (extratwocost > 0)
                        {
                            PdfPCell celldMt1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldMt1.Colspan = 5;
                            tabled.AddCell(celldMt1);

                            PdfPCell celldMt3 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            tabled.AddCell(celldMt3);
                        }

                    }

                    if (ExtraDataSTcheck == true)
                    {
                        if (machinarycost > 0)
                        {
                            if (STMachinary == false)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 5;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }
                        if (materialcost > 0)
                        {
                            if (STMaterial == false)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 5;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }


                        if (maintenancecost > 0)
                        {
                            if (STMaintenance == false)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 5;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }

                        if (extraonecost > 0)
                        {
                            if (STExtraone == false)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 5;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }
                        if (extratwocost > 0)
                        {
                            if (STExtratwo == false)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 5;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }
                    }

                    if (discountone > 0)
                    {
                        PdfPCell celldMt1 = new PdfPCell(new Phrase(discountonetitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        celldMt1.Colspan = 5;
                        tabled.AddCell(celldMt1);

                        PdfPCell celldMt3 = new PdfPCell(new Phrase(discountone.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        tabled.AddCell(celldMt3);
                    }
                    if (discounttwo > 0)
                    {
                        PdfPCell celldMt1 = new PdfPCell(new Phrase(discounttwotitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        celldMt1.Colspan = 5;
                        tabled.AddCell(celldMt1);

                        PdfPCell celldMt3 = new PdfPCell(new Phrase(discounttwo.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        tabled.AddCell(celldMt3);
                    }
                }
                #endregion

                PdfPCell cellremarks = new PdfPCell(new Phrase("Remarks : " + Remarks, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cellremarks.HorizontalAlignment = 0;
                cellremarks.Colspan = 3;//0=Left, 1=Centre, 2=Right
                tabled.AddCell(cellremarks);

                // PdfPCell cellremarks1 = new PdfPCell(new Phrase(Remarks, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                // cellremarks1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                ////  cellremarks1.Colspan = 1;
                // tabled.AddCell(cellremarks1);

                PdfPCell celldg6 = new PdfPCell(new Phrase("Grand Total(Rs.)", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                celldg6.HorizontalAlignment = 2;
                celldg6.Colspan = 2;//0=Left, 1=Centre, 2=Right
                tabled.AddCell(celldg6);

                PdfPCell celldg8 = new PdfPCell(new Phrase(Grandtotal.ToString("#,##"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                celldg8.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                celldg8.Colspan = 1;
                tabled.AddCell(celldg8);
                document.Add(tabled);

                PdfPTable tablecon = new PdfPTable(2);
                tablecon.TotalWidth = 500f;
                tablecon.LockedWidth = true;
                float[] widthcon = new float[] { 2f, 3.5f };
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

                PdfPCell cellcamt = new PdfPCell(new Phrase(" In Words: Rupees " + Amountinwords.Trim() + "",
                    FontFactory.GetFont(FontStyle, 10, Font.BOLDITALIC, BaseColor.BLACK)));
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
                string Notes = string.Empty;

                if (compInfo.Rows.Count > 0)
                {
                    Servicetax = compInfo.Rows[0]["BillNotes"].ToString();
                    PANNO = compInfo.Rows[0]["Labourrule"].ToString();
                    PFNo = compInfo.Rows[0]["PFNo"].ToString();
                    Esino = compInfo.Rows[0]["ESINo"].ToString();
                    PTno = compInfo.Rows[0]["bankname"].ToString();
                    Notes = compInfo.Rows[0]["Notes"].ToString();

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

                //var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                //var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

                //var phrase = new Phrase();
                //phrase.Add(new Chunk("REASON(S) FOR CANCELLATION:", boldFont));
                //phrase.Add(new Chunk(" See Statutoryreason(s) designated by Code No(s) 1 on the reverse side hereof", normalFont));

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



                if (Notes.Trim().Length > 0)
                {

                    //PdfPCell note = new PdfPCell(new Phrase("Terms & Conditions:", FontFactory.GetFont(FontStyle, 9, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
                    //note.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //note.Colspan = 7;
                    //note.Border = 0;
                    //tablecon.AddCell(note);

                    PdfPCell note1 = new PdfPCell(new Phrase(Notes.ToString(), FontFactory.GetFont(FontStyle, 8, Font.BOLD, BaseColor.BLACK)));
                    note1.HorizontalAlignment = Element.ALIGN_JUSTIFIED; //0=Left, 1=Centre, 2=Right
                    note1.Colspan = 7;
                    note1.Border = 0;
                    note1.SetLeading(0, 1.5f);
                    tablecon.AddCell(note1);

                }


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
                //}
                //catch (Exception ex)
                //{
                //    //LblResult.Text = ex.Message;
                //}
            }
            else
            {
                // LblResult.Text = "There is no bill generated for selected client";
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' There is no bill generated for selected client ');", true);

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



                    string SelectBillNo = "Select * from MUnitBill where BillNo= '" + ddlMBBillnos.SelectedValue + "' and  month='" + month + "' and unitid='" + ddlclientid.SelectedValue + "' ";
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
                    //create the header row
                    for (int colIndex = 0; colIndex < 6; colIndex++)
                    {
                        //set the column width

                        if (colIndex == 0)
                        {
                            // colWidths[colIndex] = (int)gvClientBilling.Columns[colIndex].ItemStyle.Width.Value;
                            //fetch the header text
                            cellText = Server.HtmlDecode(gvClientBilling.HeaderRow.Cells[colIndex].Text);
                            //create a new cell with header text
                            cell = new PdfPCell(new Phrase("S.No", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            //set the background color for the header cell
                            cell.HorizontalAlignment = 1;
                            cell.BorderWidthBottom = 0.5f;
                            cell.BorderWidthLeft = 1.5f;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0.5f;
                            //cell.BorderColor = BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }
                        if (colIndex == 1)
                        {
                            // colWidths[colIndex] = (int)gvClientBilling.Columns[colIndex].ItemStyle.Width.Value;
                            //fetch the header text
                            cellText = Server.HtmlDecode(gvClientBilling.HeaderRow.Cells[colIndex].Text);
                            //create a new cell with header text
                            cell = new PdfPCell(new Phrase("Description", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            //set the background color for the header cell
                            cell.HorizontalAlignment = 1;
                            cell.BorderWidthBottom = 0.5f;
                            cell.BorderWidthLeft = 0.5f;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0.5f;
                            //cell.BorderColor = BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }

                        if (colIndex == 2)
                        {
                            // colWidths[colIndex] = (int)gvClientBilling.Columns[colIndex].ItemStyle.Width.Value;
                            //fetch the header text
                            cellText = Server.HtmlDecode(gvClientBilling.HeaderRow.Cells[colIndex].Text);
                            //create a new cell with header text
                            cell = new PdfPCell(new Phrase("No Of Days Per Month ", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            //set the background color for the header cell
                            cell.HorizontalAlignment = 1;
                            cell.BorderWidthBottom = 0.5f;
                            cell.BorderWidthLeft = 0.5f;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0.5f;
                            //cell.BorderColor = BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }
                        if (colIndex == 3)
                        {

                            cell = new PdfPCell(new Phrase("No of shifts", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            //set the background color for the header cell
                            cell.HorizontalAlignment = 1;
                            cell.BorderWidthBottom = 0.5f;
                            cell.BorderWidthLeft = 0.5f;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0.5f;
                            //cell.BorderColor = BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }
                        if (colIndex == 4)
                        {
                            // colWidths[colIndex] = (int)gvClientBilling.Columns[colIndex].ItemStyle.Width.Value;
                            //fetch the header text
                            cellText = Server.HtmlDecode(gvClientBilling.HeaderRow.Cells[3].Text);
                            //create a new cell with header text
                            //cellText = "UOM";
                            //set the background color for the header cell
                            cell = new PdfPCell(new Phrase("Rate(Rs)", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            //cell.HorizontalAlignment = 1;
                            cell.BorderWidthBottom = 0.5f;
                            cell.BorderWidthLeft = 0.5f;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0.5f;
                            //cell.BorderColor = BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }


                        if (colIndex == 5)
                        {
                            // colWidths[colIndex] = (int)gvClientBilling.Columns[colIndex].ItemStyle.Width.Value;
                            //fetch the header text
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
                        }

                    }
                    decimal noofshifts = 0;
                    ////export rows from GridView to table
                    for (int rowIndex = 0; rowIndex < gvClientBilling.Rows.Count; rowIndex++)
                    {
                        if (gvClientBilling.Rows[rowIndex].RowType == DataControlRowType.DataRow)
                        //gvClientBilling.RowStyle.BorderColor = System.Drawing.Color.Gray;
                        {
                            TextBox lblamount = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtAmount"));
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
                                            TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("lblgvSlno"));
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
                                            TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtgvdesgn"));
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

                                            TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtNoOfDuties"));

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
                                            TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtPayRate"));

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
                                            TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtAmount"));
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
                    Cellempty.MinimumHeight = 14;
                    PdfPCell Cellempty1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    Cellempty1.HorizontalAlignment = 2;
                    Cellempty1.Colspan = 1;
                    Cellempty1.BorderWidthTop = 0;
                    Cellempty1.BorderWidthRight = 0.5f;
                    Cellempty1.BorderWidthLeft = 0.5f;
                    Cellempty1.BorderWidthBottom = 0;
                    Cellempty1.MinimumHeight = 14;

                    PdfPCell Cellempty2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    Cellempty2.HorizontalAlignment = 2;
                    Cellempty2.Colspan = 1;
                    Cellempty2.BorderWidthTop = 0;
                    Cellempty2.BorderWidthRight = 0.5f;
                    Cellempty2.BorderWidthLeft = 0.5f;
                    Cellempty2.BorderWidthBottom = 0;
                    Cellempty2.MinimumHeight = 14;

                    PdfPCell Cellempty3 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    Cellempty3.HorizontalAlignment = 2;
                    Cellempty3.Colspan = 1;
                    Cellempty3.BorderWidthTop = 0;
                    Cellempty3.BorderWidthRight = 0.5f;
                    Cellempty3.BorderWidthLeft = 0.5f;
                    Cellempty3.BorderWidthBottom = 0;
                    Cellempty3.MinimumHeight = 14;

                    PdfPCell Cellempty4 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    Cellempty4.HorizontalAlignment = 2;
                    Cellempty4.Colspan = 1;
                    Cellempty4.BorderWidthTop = 0;
                    Cellempty4.BorderWidthRight = 0.5f;
                    Cellempty4.BorderWidthLeft = 0.5f;
                    Cellempty4.BorderWidthBottom = 0;
                    Cellempty4.MinimumHeight = 14;

                    PdfPCell Cellempty5 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                    Cellempty5.HorizontalAlignment = 2;
                    Cellempty5.Colspan = 1;
                    Cellempty5.BorderWidthTop = 0;
                    Cellempty5.BorderWidthRight = 1.5f;
                    Cellempty5.BorderWidthLeft = 0.5f;
                    Cellempty5.BorderWidthBottom = 0;
                    Cellempty5.MinimumHeight = 14;

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

        protected void ddlmonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblbillnolatest.Text = "";
            txtbilldate.Text = "";
            FillMonthDetails();
            LoadOldBillnos();
            DisplayDataInGrid();
        }

        protected void btnAddNewRow_Click(object sender, EventArgs e)
        {
            AddNewRow();

        }

        protected void btnCalculateTotals_Click(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        private void CalculateTotals()
        {

            decimal totalamt = 0;
            var billdtnew = DateTime.Now.ToString("dd/MM/yyyy");

            if (txtbilldate.Text != "")
            {
                billdtnew = txtbilldate.Text;
            }

            DateTime dt = DateTime.ParseExact(billdtnew, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            // for both "1/1/2000" or "25/1/2000" formats
            string billdt = dt.ToString("MM/dd/yyyy");
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
            DataTable DTContractID = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPNameForGetContractID, HtGetContractID);

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

            bool Roundoff = false;

            var cid = ddlclientid.SelectedValue;
            var query = @"select * from Contracts where ClientID =  '" + cid + "' and contractid='" + ContractID + "'";
            var query1 = @"select ServiceTaxSeparate,Cess,SHECess,SBCess,KKCess,CGST,SGST,IGST,cess1,cess2 from TblOptions where '" + billdt + "' between fromdate and todate";

            var contractdetails = SqlHelper.Instance.GetTableByQuery(query);
            var optiondetails = SqlHelper.Instance.GetTableByQuery(query1);

            decimal ServiceTaxSeparate = Convert.ToDecimal(optiondetails.Rows[0]["ServiceTaxSeparate"].ToString());
            decimal Cess = Convert.ToDecimal(optiondetails.Rows[0]["Cess"].ToString());
            decimal SHEcess = Convert.ToDecimal(optiondetails.Rows[0]["SHECess"].ToString());
            decimal SBcess = Convert.ToDecimal(optiondetails.Rows[0]["SBCess"].ToString());
            decimal KKcess = Convert.ToDecimal(optiondetails.Rows[0]["KKCess"].ToString());


            decimal CGST = Convert.ToDecimal(optiondetails.Rows[0]["CGST"].ToString());
            decimal SGST = Convert.ToDecimal(optiondetails.Rows[0]["SGST"].ToString());
            decimal IGST = Convert.ToDecimal(optiondetails.Rows[0]["IGST"].ToString());
            decimal Cess1 = Convert.ToDecimal(optiondetails.Rows[0]["Cess1"].ToString());
            decimal Cess2 = Convert.ToDecimal(optiondetails.Rows[0]["Cess2"].ToString());
            Roundoff = bool.Parse(contractdetails.Rows[0]["Roundoff"].ToString());

            decimal servicetax = 0;
            decimal cesstax = 0;
            decimal sbcesstax = 0;
            decimal kkcesstax = 0;
            decimal educess = 0;
            decimal gtotal = 0;
            decimal servicecharge = 0;
            decimal subtotal = 0;
            decimal Servicechargeamt = 0;

            decimal CGSTTax = 0;
            decimal SGSTTax = 0;
            decimal IGSTTax = 0;
            decimal Cess1Tax = 0;
            decimal Cess2Tax = 0;

            for (int i = 0; i < gvClientBilling.Rows.Count; i++)
            {
                DropDownList ddldtype = gvClientBilling.Rows[i].FindControl("ddldutytype") as DropDownList;
                DropDownList ddlnod = gvClientBilling.Rows[i].FindControl("ddlnod") as DropDownList;
                TextBox txtDesg = gvClientBilling.Rows[i].FindControl("txtgvdesgn") as TextBox;
                TextBox txtpayrate = gvClientBilling.Rows[i].FindControl("txtPayRate") as TextBox;
                HiddenField hdNOD = gvClientBilling.Rows[i].FindControl("hdNOD") as HiddenField;
                TextBox txtnod = gvClientBilling.Rows[i].FindControl("txtNoOfDuties") as TextBox;
                TextBox txtdutyamt = gvClientBilling.Rows[i].FindControl("txtda") as TextBox;
                TextBox txtTotal = gvClientBilling.Rows[i].FindControl("txtAmount") as TextBox;

                if (!string.IsNullOrEmpty(txtDesg.Text.Trim()))
                {
                    switch (ddldtype.SelectedIndex)
                    {
                        case 4:
                            if (Roundoff == false)
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

                            if (Roundoff == false)
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

                            if (Roundoff == false)
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
                    if (!string.IsNullOrEmpty(txtTotal.Text.Trim()))
                    {
                        if (Roundoff == false)
                        {
                            totalamt += Convert.ToDecimal(txtTotal.Text.ToString());
                        }
                        else
                        {
                            totalamt += Convert.ToDecimal(txtTotal.Text.ToString());
                        }

                        txtTotal.Text = txtdutyamt.Text.ToString();
                    }

                }
            }




            if (contractdetails.Rows.Count > 0)
            {
                //  Txtservicechrg.Text = contractdetails.Rows[0]["ServiceCharge"].ToString();
                servicecharge = Convert.ToDecimal(Txtservicechrg.Text);
                if (Roundoff == false)
                {
                    lblServiceCharges.Text = (totalamt * (servicecharge / 100)).ToString("0");
                }
                else
                {
                    lblServiceCharges.Text = (totalamt * (servicecharge / 100)).ToString("0.00");
                }
                Servicechargeamt = Convert.ToDecimal(lblServiceCharges.Text);
                subtotal = totalamt + Servicechargeamt;
                if (Roundoff == false)
                {
                    lblSubTotal.Text = (subtotal).ToString("0");
                }
                else
                {
                    lblSubTotal.Text = (subtotal).ToString("0.00");
                }

                if (contractdetails.Rows[0]["IncludeST"].ToString() == "True")
                {
                    servicetax = 0;
                    cesstax = 0;
                    educess = 0;
                    sbcesstax = 0;
                    kkcesstax = 0;
                    CGSTTax = 0;
                    SGSTTax = 0;
                    IGSTTax = 0;
                    Cess1Tax = 0;
                    Cess2Tax = 0;
                    CGST = 0;
                    SGST = 0;
                    IGST = 0;
                    Cess1 = 0;
                    Cess2 = 0;
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
                    servicetax = ServiceTaxSeparate * (totalamt + Servicechargeamt) / 100;
                    sbcesstax = SBcess * (totalamt + Servicechargeamt) / 100;
                    kkcesstax = KKcess * (totalamt + Servicechargeamt) / 100;
                    cesstax = Cess * servicetax / 100;
                    educess = SHEcess * servicetax / 100;
                    if (contractdetails.Rows[0]["CGST"].ToString() == "False")
                    {
                        CGSTTax = 0;
                    }
                    else
                    {
                        CGSTTax = (CGST * (totalamt + Servicechargeamt) / 100);
                    }

                    if (contractdetails.Rows[0]["SGST"].ToString() == "False")
                    {
                        SGSTTax = 0;
                    }
                    else
                    {
                        SGSTTax = (SGST * (totalamt + Servicechargeamt) / 100);
                    }


                    if (contractdetails.Rows[0]["IGST"].ToString() == "False")
                    {
                        IGSTTax = 0;
                    }
                    else
                    {
                        IGSTTax = (IGST * (totalamt + Servicechargeamt) / 100);
                    }


                    if (contractdetails.Rows[0]["Cess1"].ToString() == "False")
                    {
                        Cess1Tax = 0;
                    }
                    else
                    {
                        Cess1Tax = (Cess1 * (totalamt + Servicechargeamt) / 100);
                    }

                    if (contractdetails.Rows[0]["Cess2"].ToString() == "False")
                    {
                        Cess2Tax = 0;
                    }
                    else
                    {
                        Cess2Tax = (Cess2 * (totalamt + Servicechargeamt) / 100);
                    }
                }
                gtotal = subtotal + servicetax + cesstax + educess + sbcesstax + kkcesstax + Cess1Tax + Cess2Tax + CGSTTax + SGSTTax + IGSTTax;
            }


            if (Roundoff == false)
            {
                if (totalamt > 0)
                {
                    lblTotalResources.Text = totalamt.ToString("0");
                    lblTotalResources.Visible = true;
                }


                if (servicetax > 0)
                {
                    lblServiceTax.Text = servicetax.ToString("0");
                    lblServiceTaxTitle.Visible = true;
                    lblServiceTax.Visible = true;
                }
                else
                {
                    lblServiceTax.Text = servicetax.ToString("0");
                    lblServiceTaxTitle.Visible = false;
                    lblServiceTax.Visible = false;
                }
                if (sbcesstax > 0)
                {
                    lblSBCESS.Text = sbcesstax.ToString("0");
                    lblSBCESSTitle.Visible = true;
                    lblSBCESS.Visible = true;
                }
                else
                {
                    lblSBCESS.Text = sbcesstax.ToString("0");
                    lblSBCESSTitle.Visible = false;
                    lblSBCESS.Visible = false;
                }
                if (kkcesstax > 0)
                {
                    lblKKCESS.Text = kkcesstax.ToString("0");
                    lblKKCESSTitle.Visible = true;
                    lblKKCESS.Visible = true;
                }
                else
                {
                    lblKKCESS.Text = kkcesstax.ToString("0");
                    lblKKCESSTitle.Visible = false;
                    lblKKCESS.Visible = false;
                }
                #region for GST on 17-6-2017 by swathi

                if (CGSTTax > 0)
                {
                    lblCGST.Text = CGSTTax.ToString("0");
                    TxtCGSTPrc.Text = CGST.ToString();
                    lblCGSTTitle.Visible = true;
                    lblCGST.Visible = true;
                    TxtCGSTPrc.Visible = true;
                }
                else
                {
                    lblCGST.Text = CGSTTax.ToString("0");
                    TxtCGSTPrc.Text = CGST.ToString();
                    lblCGSTTitle.Visible = false;
                    lblCGST.Visible = false;
                    TxtCGSTPrc.Visible = false;

                }



                if (SGSTTax > 0)
                {
                    lblSGST.Text = SGSTTax.ToString("0");
                    TxtSGSTPrc.Text = SGST.ToString();
                    lblSGSTTitle.Visible = true;
                    lblSGST.Visible = true;
                    TxtSGSTPrc.Visible = true;
                }
                else
                {
                    lblSGST.Text = SGSTTax.ToString("0");
                    TxtSGSTPrc.Text = SGST.ToString();
                    lblSGSTTitle.Visible = false;
                    lblSGST.Visible = false;
                    TxtSGSTPrc.Visible = false;

                }



                if (IGSTTax > 0)
                {
                    lblIGST.Text = IGSTTax.ToString("0");
                    TxtIGSTPrc.Text = IGST.ToString();
                    lblIGSTTitle.Visible = true;
                    lblIGST.Visible = true;
                    TxtIGSTPrc.Visible = true;
                }
                else
                {
                    lblIGST.Text = IGSTTax.ToString("0");
                    TxtIGSTPrc.Text = IGST.ToString();
                    lblIGSTTitle.Visible = false;
                    lblIGST.Visible = false;
                    TxtIGSTPrc.Visible = false;

                }



                if (Cess1Tax > 0)
                {
                    lblCess1.Text = Cess1Tax.ToString("0");
                    TxtCess1Prc.Text = Cess1.ToString();
                    lblCess1Title.Visible = true;
                    lblCess1.Visible = true;
                    TxtCess1Prc.Visible = true;
                }
                else
                {
                    lblCess1.Text = Cess1Tax.ToString("0");
                    TxtCess1Prc.Text = Cess1.ToString();
                    lblCess1Title.Visible = false;
                    lblCess1.Visible = false;
                    TxtCess1Prc.Visible = false;

                }



                if (Cess2Tax > 0)
                {
                    lblCess2.Text = Cess2Tax.ToString("0");
                    TxtCess2Prc.Text = Cess2.ToString();
                    lblCess2Title.Visible = true;
                    lblCess2.Visible = true;
                    TxtCess2Prc.Visible = true;
                }
                else
                {
                    lblCess2.Text = Cess2Tax.ToString("0");
                    TxtCess2Prc.Text = Cess2.ToString();
                    lblCess2Title.Visible = false;
                    lblCess2.Visible = false;
                    TxtCess2Prc.Visible = false;

                }

                #endregion for GST on 17-6-2017
                if (cesstax > 0)
                {
                    lblCESS.Text = cesstax.ToString("0");
                    lblCESSTitle.Visible = true;
                    lblCESS.Visible = true;
                }
                else
                {
                    lblCESS.Text = cesstax.ToString("0");
                    lblCESSTitle.Visible = false;
                    lblCESS.Visible = false;
                }
                if (educess > 0)
                {
                    lblSheCESS.Text = educess.ToString("0");
                    lblSheCESSTitle.Visible = true;
                    lblSheCESS.Visible = true;
                }
                else
                {
                    lblSheCESS.Text = educess.ToString("0");
                    lblSheCESSTitle.Visible = false;
                    lblSheCESS.Visible = false;
                }
                if (gtotal > 0)
                {
                    lblGrandTotal.Text = gtotal.ToString("0");
                    lblGrandTotal.Visible = true;
                }
            }
            else
            {
                if (totalamt > 0)
                {
                    lblTotalResources.Text = totalamt.ToString("0.00");
                    lblTotalResources.Visible = true;
                }


                if (servicetax > 0)
                {
                    lblServiceTax.Text = servicetax.ToString("0.00");
                    lblServiceTaxTitle.Visible = true;
                    lblServiceTax.Visible = true;
                }
                else
                {
                    lblServiceTax.Text = servicetax.ToString("0.00");
                    lblServiceTaxTitle.Visible = false;
                    lblServiceTax.Visible = false;
                }
                if (sbcesstax > 0)
                {
                    lblSBCESS.Text = sbcesstax.ToString("0.00");
                    lblSBCESSTitle.Visible = true;
                    lblSBCESS.Visible = true;
                }
                else
                {
                    lblSBCESS.Text = sbcesstax.ToString("0.00");
                    lblSBCESSTitle.Visible = false;
                    lblSBCESS.Visible = false;
                }
                if (kkcesstax > 0)
                {
                    lblKKCESS.Text = kkcesstax.ToString("0.00");
                    lblKKCESSTitle.Visible = true;
                    lblKKCESS.Visible = true;
                }
                else
                {
                    lblKKCESS.Text = kkcesstax.ToString("0.00");
                    lblKKCESSTitle.Visible = false;
                    lblKKCESS.Visible = false;
                }
                #region for GST on 17-6-2017 by swathi

                if (CGSTTax > 0)
                {
                    lblCGST.Text = CGSTTax.ToString("0.00");
                    TxtCGSTPrc.Text = CGST.ToString();
                    lblCGSTTitle.Visible = true;
                    lblCGST.Visible = true;
                    TxtCGSTPrc.Visible = true;
                }
                else
                {
                    lblCGST.Text = CGSTTax.ToString("0.00");
                    TxtCGSTPrc.Text = CGST.ToString();
                    lblCGSTTitle.Visible = false;
                    lblCGST.Visible = false;
                    TxtCGSTPrc.Visible = false;

                }



                if (SGSTTax > 0)
                {
                    lblSGST.Text = SGSTTax.ToString("0.00");
                    TxtSGSTPrc.Text = SGST.ToString();
                    lblSGSTTitle.Visible = true;
                    lblSGST.Visible = true;
                    TxtSGSTPrc.Visible = true;
                }
                else
                {
                    lblSGST.Text = SGSTTax.ToString("0.00");
                    TxtSGSTPrc.Text = SGST.ToString();
                    lblSGSTTitle.Visible = false;
                    lblSGST.Visible = false;
                    TxtSGSTPrc.Visible = false;

                }



                if (IGSTTax > 0)
                {
                    lblIGST.Text = IGSTTax.ToString("0.00");
                    TxtIGSTPrc.Text = IGST.ToString();
                    lblIGSTTitle.Visible = true;
                    lblIGST.Visible = true;
                    TxtIGSTPrc.Visible = true;
                }
                else
                {
                    lblIGST.Text = IGSTTax.ToString("0.00");
                    TxtIGSTPrc.Text = IGST.ToString();
                    lblIGSTTitle.Visible = false;
                    lblIGST.Visible = false;
                    TxtIGSTPrc.Visible = false;

                }



                if (Cess1Tax > 0)
                {
                    lblCess1.Text = Cess1Tax.ToString("0.00");
                    TxtCess1Prc.Text = Cess1.ToString();
                    lblCess1Title.Visible = true;
                    lblCess1.Visible = true;
                    TxtCess1Prc.Visible = true;
                }
                else
                {
                    lblCess1.Text = Cess1Tax.ToString("0.00");
                    TxtCess1Prc.Text = Cess1.ToString();
                    lblCess1Title.Visible = false;
                    lblCess1.Visible = false;
                    TxtCess1Prc.Visible = false;

                }



                if (Cess2Tax > 0)
                {
                    lblCess2.Text = Cess2Tax.ToString("0.00");
                    TxtCess2Prc.Text = Cess2.ToString();
                    lblCess2Title.Visible = true;
                    lblCess2.Visible = true;
                    TxtCess2Prc.Visible = true;
                }
                else
                {
                    lblCess2.Text = Cess2Tax.ToString("0.00");
                    TxtCess2Prc.Text = Cess2.ToString();
                    lblCess2Title.Visible = false;
                    lblCess2.Visible = false;
                    TxtCess2Prc.Visible = false;

                }

                #endregion for GST on 17-6-2017
                if (cesstax > 0)
                {
                    lblCESS.Text = cesstax.ToString("0.00");
                    lblCESSTitle.Visible = true;
                    lblCESS.Visible = true;
                }
                else
                {
                    lblCESS.Text = cesstax.ToString("0.00");
                    lblCESSTitle.Visible = false;
                    lblCESS.Visible = false;
                }
                if (educess > 0)
                {
                    lblSheCESS.Text = educess.ToString("0.00");
                    lblSheCESSTitle.Visible = true;
                    lblSheCESS.Visible = true;
                }
                else
                {
                    lblSheCESS.Text = educess.ToString("0.00");
                    lblSheCESSTitle.Visible = false;
                    lblSheCESS.Visible = false;
                }
                if (gtotal > 0)
                {
                    lblGrandTotal.Text = gtotal.ToString("0");
                    lblGrandTotal.Visible = true;
                }
            }
        }

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

        public void FillDefaultGird()
        {
            DataTable DefaultTable = new DataTable();
            DefaultTable.Columns.Add("Sid", typeof(int));
            DefaultTable.Columns.Add("Designation", typeof(string));
            // DefaultTable.Columns.Add("DutyHrs", typeof(string));
            DefaultTable.Columns.Add("NoofEmps", typeof(string));
            DefaultTable.Columns.Add("DutyHours", typeof(string));
            DefaultTable.Columns.Add("payrate", typeof(string));
            DefaultTable.Columns.Add("paytype", typeof(string));
            DefaultTable.Columns.Add("BasicDa", typeof(string));
            // DefaultTable.Columns.Add("OTAmount", typeof(string));
            DefaultTable.Columns.Add("NoOfDays", typeof(string));
            DefaultTable.Columns.Add("Totalamount", typeof(string));

            var cid = ddlclientid.SelectedValue;

            int noOfDaysInMonth = 0;

            if (!cid.Equals("--Select--"))
            {
                #region Month Selection
                int month = 0;
                month = GetMonthBasedOnSelectionDateorMonth();




                //if (ddlmonth.SelectedIndex == 1)
                //{
                //    month = GlobalData.Instance.GetIDForNextMonth();
                //}
                //else if (ddlmonth.SelectedIndex == 2)
                //{
                //    month = GlobalData.Instance.GetIDForThisMonth();
                //}
                //else
                //{
                //    month = GlobalData.Instance.GetIDForPrviousMonth();
                //}
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


                string sqlqry = "select max(isnull(munitidbillno,'')) as billno from MUnitBillBreakup where  UnitId='" + ddlclientid.SelectedValue + "' and month='" + prevmonth + "'";
                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
                string MaxBillno = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    MaxBillno = dt.Rows[0]["billno"].ToString();

                    if (MaxBillno != "")
                    {
                        var query = @"select Designation,NoofEmps,DutyHours,PayRate,PayRateType as paytype,monthlydays as NoOfDays,BasicDa,Totalamount from MUnitBillBreakup  where UnitId='" + ddlclientid.SelectedValue + "' and month='" + prevmonth + "' and munitidbillno='" + MaxBillno + "' order by sino ";
                        var griddata = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
                        DefaultTable = griddata;
                    }
                    else
                    {
                        var query = @"select d.Design as Designation,
	                               ISNULL(cd.Quantity,0) as NoofEmps,
                                   ISNULL(cd.DutyHrs,0) as DutyHrs,
	                               ISNULL(cad.Duties,0) as DutyHours,
	                               ISNULL(Amount,0) as payrate,
	                               cd.PayType as paytype,0 as BasicDa,0 as OTAmount,cd.NoOfDays,0 as Totalamount
	                        from Designations d 
                            inner join ContractDetails cd on d.DesignId = cd.Designations
                            left outer join ClientAttenDance cad on cd.ClientID = cad.ClientId and cd.Designations = cad.Desingnation 
                            and cad.[month]= " + month.ToString() + " where cd.ClientID = '" + cid + "' and cd.contractid='" + ContractID + "'  ";

                        var griddata = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
                        DefaultTable = griddata;
                    }
                }
                else
                {
                    var query = @"select d.Design as Designation,
	                               ISNULL(cd.Quantity,0) as NoofEmps,
                                   ISNULL(cd.DutyHrs,0) as DutyHrs,
	                               ISNULL(cad.Duties,0) as DutyHours,
	                               ISNULL(Amount,0) as payrate,
	                               cd.PayType as paytype,0 as BasicDa,0 as OTAmount,cd.NoOfDays,0 as Totalamount
	                        from Designations d 
                            inner join ContractDetails cd on d.DesignId = cd.Designations
                            left outer join ClientAttenDance cad on cd.ClientID = cad.ClientId and cd.Designations = cad.Desingnation 
                            and cad.[month]= " + month.ToString() + " where cd.ClientID = '" + cid + "' and cd.contractid='" + ContractID + "'  ";

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
                dr["NoofEmps"] = 0;
                //dr["DutyHrs"] = 0;
                dr["DutyHours"] = 0;
                dr["payrate"] = 0;
                dr["paytype"] = 0;
                dr["BasicDa"] = 0;
                // dr["OTAmount"] = 0;
                dr["NoOfDays"] = 1;
                dr["Totalamount"] = 0;
                DefaultTable.Rows.Add(dr);
            }

            ViewState["DTDefaultManual"] = DefaultTable;
            gvClientBilling.DataSource = DefaultTable;
            gvClientBilling.DataBind();

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


            }

            btnAddNewRow.Visible = (gvClientBilling.Rows.Count > 0);
            btnCalculateTotals.Visible = (gvClientBilling.Rows.Count > 0);
        }

        protected void ddlclientid_SelectedIndexChanged(object sender, EventArgs e)
        {

            lblResult.Text = "";
            lbltotalamount.Text = "";
            lblbillnolatest.Text = "";
            txtbilldate.Text = "";
            txtfromdate.Text = "";
            txttodate.Text = "";
            ddlmonth.SelectedIndex = 0;
            txtmonth.Text = string.Empty;
            rdbcreatebill.Checked = true;
            rdbmodifybill.Checked = false;
            ddlMBBillnos.SelectedIndex = 0;
            lblbillnolatest.Text = "";

            ClearExtraDataForBilling();
            if (ddlclientid.SelectedIndex > 0)
            {
                string SqlQryGetCname = "select clientid from clients where clientid='" + ddlclientid.SelectedValue + "'";
                DataTable dt;
                dt = config.ExecuteReaderWithQueryAsync(SqlQryGetCname).Result;
                ddlCname.SelectedValue = dt.Rows[0]["clientid"].ToString();
                ddlmonth.SelectedIndex = 0;
                if (ddlmonth.SelectedIndex > 0)
                {
                    FillMonthDetails();
                }
            }
            else
            {
                ddlCname.SelectedIndex = 0;
            }
        }

        protected void ddlCname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblResult.Text = "";
            ddlmonth.SelectedIndex = 0;
            lblbillnolatest.Text = "";
            txtbilldate.Text = "";
            txtfromdate.Text = "";
            txttodate.Text = "";
            txtmonth.Text = string.Empty;
            rdbcreatebill.Checked = true;
            rdbmodifybill.Checked = false;
            ddlMBBillnos.SelectedIndex = 0;
            lblbillnolatest.Text = "";


            ClearExtraDataForBilling();
            if (ddlCname.SelectedIndex > 0)
            {
                string SqlQryGetCname = "select clientid from clients where clientid='" + ddlCname.SelectedValue + "'";
                DataTable dt;
                dt = config.ExecuteReaderWithQueryAsync(SqlQryGetCname).Result;
                if (dt.Rows.Count > 0)
                {
                    ddlclientid.SelectedValue = dt.Rows[0]["clientid"].ToString();
                }

                if (ddlmonth.SelectedIndex > 0)
                {
                    FillMonthDetails();
                }
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        protected void ClearExtraDataForBilling()
        {
            lblResult.Text = "";

            gvClientBilling.DataSource = null;
            gvClientBilling.DataBind();
            lblTotalResources.Text = "";
            lblServiceTax.Text = "";
            lblCESS.Text = "";
            lblSBCESS.Text = "";
            lblKKCESS.Text = "";
            lblSheCESS.Text = "";
            lblGrandTotal.Text = "";
            lblSubTotal.Text = "";
            lblServiceCharges.Text = "";
            Txtservicechrg.Text = "";
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

        protected void ddlMBBillnos_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbmodifybill.Checked == false)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Bill Type Modify');", true);
                ddlMBBillnos.SelectedIndex = 0;
            }
            else
            {
                DisplayDataInGrid();
            }
        }

        protected void btngenratepayment_Click(object sender, EventArgs e)
        {
            // btninvoice.Visible = true;
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
            var monthnew = "0";
            monthnew = GetMonthNew();
            string SelectedClient = ddlclientid.SelectedValue;
            #region Month Selection

            month = GetMonthBasedOnSelectionDateorMonth();

            //if (ddlmonth.SelectedIndex == 1)
            //{
            //    month = GlobalData.Instance.GetIDForNextMonth();
            //}
            //else if (ddlmonth.SelectedIndex == 2)
            //{
            //    month = GlobalData.Instance.GetIDForThisMonth();
            //}
            //else
            //{
            //    month = GlobalData.Instance.GetIDForPrviousMonth();
            //}
            #endregion

            #region  Query For Delete Unitbill Break Up Data

            /** Delete previously generated UnitBillBreakup data */

            if (rdbmodifybill.Checked)
            {
                string DeleteQueryForSelectedMonth = "Delete from Munitbillbreakup where unitid ='" + SelectedClient + "' and month =" +
                                                                     month + " and MunitidBillno='" + ddlMBBillnos.SelectedValue + "'";
                int status = config.ExecuteNonQueryWithQueryAsync(DeleteQueryForSelectedMonth).Result;
            }
            //Unitbill details are not deleted now due to Billno(avoid regeneration)
            /** Delete **/

            #endregion

            var billdtnew = DateTime.Now.ToString("dd/MM/yyyy");

            if (txtbilldate.Text != "")
            {
                billdtnew = txtbilldate.Text;
            }

            DateTime dt = DateTime.ParseExact(billdtnew, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            // for both "1/1/2000" or "25/1/2000" formats
            string billdt = dt.ToString("MM/dd/yyyy");
            string Billtype = string.Empty;
            if (ddltype.SelectedIndex == 0)
            {
                Billtype = "M";
            }
            else if (ddltype.SelectedIndex == 1)
            {
                Billtype = "A";
            }
            else if (ddltype.SelectedIndex == 2)
            {
                Billtype = "B";
            }
            else if (ddltype.SelectedIndex == 3)
            {
                Billtype = "E";
            }
            var query1 = @"select ServiceTaxSeparate,Cess,SHECess,SBCess,KKCess,CGST,SGST,IGST,cess1,cess2 from TblOptions where '" + billdt + "' between fromdate and todate ";
            var optiondetails = config.ExecuteReaderWithQueryAsync(query1).Result;

            decimal CGST = Convert.ToDecimal(optiondetails.Rows[0]["CGST"].ToString());
            decimal SGST = Convert.ToDecimal(optiondetails.Rows[0]["SGST"].ToString());
            decimal IGST = Convert.ToDecimal(optiondetails.Rows[0]["IGST"].ToString());
            decimal Cess1 = Convert.ToDecimal(optiondetails.Rows[0]["Cess1"].ToString());
            decimal Cess2 = Convert.ToDecimal(optiondetails.Rows[0]["Cess2"].ToString());


            decimal ServiceTaxSeparate = 0;
            decimal Cessprc = 0;
            decimal SHEcessprc = 0;
            decimal SBcessprc = 0;
            decimal KKcessprc = 0;
            decimal Cess1prc = 0;
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


            #region   Query for  Get  Contracts  Details

            string sqlQry = "Select ContractId,ContractStartDate,ContractEndDate,PaymentType,MaterialCostPerMonth, " +
                " MachinaryCostPerMonth,NoOfDays,servicecharge,OTPersent,PayLumpsum,ServiceChargeType,ServiceTax75,IncludeST, " +
                "  ServiceTaxType,BillDates from Contracts where ClientId='" + ddlclientid.SelectedValue + "'";
            DataTable dtContracts = config.ExecuteReaderWithQueryAsync(sqlQry).Result;

            if (dtContracts.Rows.Count > 0)
            {
                //CalculateTotals();

                string strSTType = dtContracts.Rows[0]["ServiceTaxType"].ToString();
                string NoOfDaysFromContract = dtContracts.Rows[0]["NoOfDays"].ToString();
                string strServiceChargetType = dtContracts.Rows[0]["ServiceChargeType"].ToString();
                string ServiceCharge = dtContracts.Rows[0]["ServiceCharge"].ToString();

                bool bSTType = (strSTType == "True");
                string billno = (rdbmodifybill.Checked)
                                ? ddlMBBillnos.SelectedValue
                                : BillnoAutoGenrate(bSTType, ddlclientid.SelectedValue, month);



                #region   Get Data From GridView and Saving In the Munitbillbreakup Table

                if (gvClientBilling.Rows.Count > 0)
                {
                    string invoicedesc = txtdescription.Text;
                    string remarks = txtremarks.Text;
                    string Unitid = ddlclientid.SelectedValue;
                    int totalstatus = 0;
                    int i = 0;

                    foreach (GridViewRow GvRow in gvClientBilling.Rows)
                    {
                        string sno = ((TextBox)GvRow.FindControl("lblgvSlno")).Text;
                        string Desgn = ((TextBox)GvRow.FindControl("txtgvdesgn")).Text;
                        string NoOfEmps = ((TextBox)GvRow.FindControl("txtnoofemployees")).Text;
                        string NoOfDuties = ((TextBox)GvRow.FindControl("txtNoOfDuties")).Text;
                        string Payrate = ((TextBox)GvRow.FindControl("txtPayRate")).Text; //lblda
                                                                                          // string Payratetype = ((TextBox)GvRow.FindControl("txtPayRatetype")).Text;
                        string DutiesAmount = ((TextBox)GvRow.FindControl("txtda")).Text;
                        //string OtAmount = ((TextBox)GvRow.FindControl("lblOtAmount")).Text;
                        string Total = ((TextBox)GvRow.FindControl("txtAmount")).Text;
                        float ToatlAmount = 0;
                        float basicda = 0;
                        ToatlAmount = (Total.Trim().Length != 0) ? float.Parse(Total) : 0;
                        basicda = (DutiesAmount.Trim().Length != 0) ? float.Parse(DutiesAmount) : 0;
                        DropDownList ddlnodays = gvClientBilling.Rows[i].FindControl("ddlnod") as DropDownList;
                        int ddlnod = int.Parse(ddlnodays.SelectedItem.Text);
                        DropDownList ddldttype = gvClientBilling.Rows[i].FindControl("ddldutytype") as DropDownList;
                        int ddldutytype = int.Parse(ddldttype.SelectedValue);
                        i = i + 1;

                        if (Desgn.Length > 0)
                        {
                            string Sqlqry = string.Format("insert into Munitbillbreakup(unitid,designation,DutyHours,NoofEmps,BasicDa, " +
                                "PayRate,PayRateType,Month,OTamount,Totalamount,MunitidBillno,monthlydays,Description,Remarks,sino,monthnew,BillType) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')",
                                Unitid, Desgn, NoOfDuties, NoOfEmps, DutiesAmount, Payrate, ddldutytype, month, 0, Total, billno, ddlnod, invoicedesc, remarks, sno, monthnew, Billtype);
                            int Status = config.ExecuteNonQueryWithQueryAsync(Sqlqry).Result;
                            if (Status != 0)
                            {
                                totalstatus++;
                                if (totalstatus == 1)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Manual Billing Details Added Sucessfully');", true);
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

                //float serviceCharge = 0;
                //if (ServiceCharge.Length > 0)
                //{
                //    serviceCharge = Convert.ToSingle(ServiceCharge);
                //}

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
                #region for GST as on 17-6-2017 by swathi

                string CGSTTax = lblCGST.Text;

                if (CGSTTax.Trim().Length == 0)
                {
                    CGSTTax = "0";
                }


                string SGSTTax = lblSGST.Text;

                if (SGSTTax.Trim().Length == 0)
                {
                    SGSTTax = "0";
                }


                string IGSTTax = lblIGST.Text;

                if (IGSTTax.Trim().Length == 0)
                {
                    IGSTTax = "0";
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
                string cesstax = lblCESS.Text;

                if (cesstax.Trim().Length == 0)
                {
                    cesstax = "0";
                }


                string Shesstax = lblSheCESS.Text;

                if (Shesstax.Trim().Length == 0)
                {
                    Shesstax = "0";
                }

                string ServiceChargePer = Txtservicechrg.Text;
                if (ServiceChargePer.Trim().Length == 0)
                {
                    ServiceChargePer = "0";
                }

                string SubTotal = lblSubTotal.Text;
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

                query1 = @"select ServiceTaxSeparate,Cess,SHECess,SBCess,KKCess from TblOptions where '" + billdate + "' between fromdate and todate";
                optiondetails = config.ExecuteReaderWithQueryAsync(query1).Result;
                ServiceTaxSeparate = 0;
                decimal Cessper = 0;
                decimal SHEcessper = 0;
                decimal SBcessper = 0;
                decimal KKcessper = 0;

                if (optiondetails.Rows.Count > 0)
                {
                    ServiceTaxSeparate = Convert.ToDecimal(optiondetails.Rows[0]["ServiceTaxSeparate"].ToString());
                    Cessper = Convert.ToDecimal(optiondetails.Rows[0]["Cess"].ToString());
                    SHEcessper = Convert.ToDecimal(optiondetails.Rows[0]["SHECess"].ToString());
                    SBcessper = Convert.ToDecimal(optiondetails.Rows[0]["SBCess"].ToString());
                    KKcessper = Convert.ToDecimal(optiondetails.Rows[0]["KKCess"].ToString());
                }

                string TotalServiceTax = (float.Parse(ServiceTax) + float.Parse(sbcesstax) + float.Parse(kkcesstax) + float.Parse(CGSTTax) + float.Parse(SGSTTax) + float.Parse(IGSTTax) + float.Parse(Cess1Tax) + float.Parse(Cess2Tax) + float.Parse(cesstax) + float.Parse(Shesstax)).ToString();


                if (rdbcreatebill.Checked)
                {
                    string InsertQueryForUnitBill = "insert into Munitbill(billno,billdt,unitid,fromdt,todt,TotalChrg," +
                                                    " monthlydays,grandtotal,ServiceChrg,servicetax,cess,shecess,ServiceTax75percentage,ServiceChrgPer,Subtotal," +
                                                    " month,MachinaryCost,MaterialCost,Discount,ElectricalChrg,Remarks,SBCessAmt,kkcessamt,ServiceTaxPrc,CESSPer,SHECessPer,SBCessTaxPrc,KKCessTaxPrc,Created_On,CGSTAmt,CGSTPrc,SGSTAmt,SGSTPrc,IGSTAmt,IGSTPrc,Cess1Amt,Cess1Prc,Cess2Amt,Cess2Prc,TotalServiceTax,monthnew,BillType,BankName,BankAccountNo,IFSCCode,OURGSTNo,BillToGSTNo,BillToState,BillToStateCode,GSTAddress,Phoneno,Faxno) values('"
                                                    + billno + "','"
                                                    + billdate + "','"
                                                    + ddlclientid.SelectedValue + "','"
                                                    + tfrom + "','"
                                                    + tto + "','"
                                                    + totalCharges + "','0','"
                                                    + grandtotal + "',"
                                                    + lblServiceCharges.Text + ","
                                                    + ServiceTax + ","
                                                    + cesstax + ","
                                                    + Shesstax + ","
                                                    + "null,"
                                                    + Txtservicechrg.Text + ","
                                                    + lblSubTotal.Text + ","
                                                    + month + ","
                                                    + " null,null,null,null,null,'" + lblSBCESS.Text + "','" + lblKKCESS.Text + "' ,'" + ServiceTaxSeparate + "','" + Cessper + "','" + SHEcessper + "','" + SBcessper + "','" + KKcessper + "','" + DateTime.Now + "','" + CGSTTax + "','" + CGSTprc + "','" + SGSTTax + "','" + SGSTprc + "','" + IGSTTax + "','" + IGSTprc + "','" + Cess1Tax + "','" + Cess1prc + "','" + Cess2Tax + "','" + Cess2prc + "','" + TotalServiceTax + "','" + monthnew + "','" + Billtype + "','" + BankName + "','" + BankAccountNo + "','" + IFSCCode + "','" + OURGSTNo + "','" + BillToGSTNo + "','" + BillToState + "','" + BillToStateCode + "','" + GSTAddress + "','" + Phoneno + "','" + Faxno + "')";
                    int status = config.ExecuteNonQueryWithQueryAsync(InsertQueryForUnitBill).Result;
                }

                if (rdbmodifybill.Checked)
                {
                    string ServiceCharge1 = lblServiceCharges.Text;
                    string ServiceChargePer1 = Txtservicechrg.Text;
                    string totalCharges1 = lblTotalResources.Text;
                    string ServiceTax1 = lblServiceTax.Text;
                    string cesstax1 = lblCESS.Text;
                    string sbcesstax1 = lblSBCESS.Text;
                    string kkcesstax1 = lblKKCESS.Text;
                    string Shesstax1 = lblSheCESS.Text;
                    string SubTotal1 = lblSubTotal.Text;
                    string grandtotal1 = lblGrandTotal.Text;
                    string desc = txtdescription.Text;
                    string remark = txtremarks.Text;



                    string SqlQryForUnitBill = "Select * from MUnitbill  where unitid ='" + SelectedClient + "' and month=" + month + "   and  billno='" + ddlMBBillnos.SelectedValue + "'";
                    DataTable dtUnitBill = config.ExecuteReaderWithQueryAsync(SqlQryForUnitBill).Result;

                    string SqlQryForudateUnitBillbreakup = "Select Description,Remarks from MUnitbillBreakUp  where unitid ='" + SelectedClient + "' and month=" + month + " and MunitidBillno='" + ddlMBBillnos.SelectedValue + "'  ";
                    DataTable dtMUnitBill = config.ExecuteReaderWithQueryAsync(SqlQryForudateUnitBillbreakup).Result;


                    // txtremarks.Text = dtUnitBill.Rows[0]["Remarks"].ToString();
                    //&& txtremarks.Text != "" && txtdescription.Text!=""

                    if (dtUnitBill.Rows.Count > 0)
                    {
                        string InsertQueryForUnitBill = string.Format("update Munitbill set billdt='{1}',unitid='{2}',fromdt='{3}',todt='{4}', " +
                           " monthlydays='{5}',grandtotal='{6}',servicetax='{7}',cess='{8}',shecess='{9}',month='{10}',TotalChrg='{11}',ServiceChrg='{12}',ServiceChrgPer='{13}',Subtotal='{14}',sbcessamt='{15}',kkcessamt='{16}',ServiceTaxPrc='{17}',SBCessTaxPrc='{18}',KKCessTaxPrc='{19}',modified_on='{20}',CGSTAmt='{21}',CGSTPrc='{22}',SGSTAmt='{23}',SGSTPrc='{24}',IGSTAmt='{25}',IGSTPrc='{26}',Cess1Amt='{27}',Cess1Prc='{28}',Cess2Amt='{29}',Cess2Prc='{30}',monthnew='{31}',BillType='{32}',BankName='{33}',BankAccountNo='{34}',IFSCCode='{35}',OURGSTNo='{36}',BillToGSTNo='{37}',BillToState='{38}',BillToStateCode='{39}',GSTAddress='{40}',Phoneno='{41}',Faxno='{42}'" +
                           " where  billno='{0}'  ",
                           billno, billdate, ddlclientid.SelectedValue, tfrom, tto,
                           0, grandtotal1, ServiceTax1, cesstax1, Shesstax1, month, totalCharges1, ServiceCharge1, ServiceChargePer1, SubTotal1, sbcesstax1, kkcesstax1, ServiceTaxSeparate, SBcessper, KKcessper, DateTime.Now, CGSTTax, CGSTprc, SGSTTax, SGSTprc, IGSTTax, IGSTprc, Cess1Tax, Cess1prc, Cess2Tax, Cess2prc, monthnew, Billtype, BankName, BankAccountNo, IFSCCode, OURGSTNo, BillToGSTNo, BillToState, BillToStateCode, GSTAddress, Phoneno, Faxno);

                        int status = config.ExecuteNonQueryWithQueryAsync(InsertQueryForUnitBill).Result;

                        string InsertQueryForMUnitBill = string.Format("update MunitbillBreakUp set Description='" + desc + "',Remarks='" + remark + "' where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "' and MunitidBillno='" + billno + "'");
                        int status1 = config.ExecuteNonQueryWithQueryAsync(InsertQueryForMUnitBill).Result;

                    }


                }
                #endregion
                LoadDefaultData();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('ContractId not available for this client.');", true);
            }
            #endregion

        }

        protected void LoadDefaultData()
        {
            //ddlclientid.SelectedIndex = 0;
            //ddlCname.SelectedIndex = 0;
            txtfromdate.Text = "";
            txttodate.Text = "";
            ddlmonth.SelectedIndex = 0;
            txtbilldate.Text = "";
            ddlMBBillnos.SelectedIndex = 0;
            FillDefaultGird();
            lblTotalResources.Text = "0";
            lblServiceTax.Text = "0";
            lblGrandTotal.Text = "0";
            lblCESS.Text = "0";
            lblSBCESS.Text = "0";
            lblKKCESS.Text = "0";
            #region for GST on 17-6-2017 by swathi
            lblCGST.Text = "0";
            lblSGST.Text = "0";
            lblIGST.Text = "0";
            lblCess1.Text = "0";
            lblCess2.Text = "0";
            #endregion for GST on 17-6-2017 by swathi
            lblSheCESS.Text = "0";
            lblServiceCharges.Text = "0";
            lblSubTotal.Text = "0";
            Txtservicechrg.Text = "";
            txtremarks.Text = "";
            txtmonth.Text = "";
            lblbillnolatest.Text = "";
        }

        private string BillnoAutoGenrate(bool StType, string unitId, int month)
        {
            string billno = "00001";
            string strBillprefix = "select isnull(gst.BillPrefix,'') as BillPrefix from Clients c inner join GSTMaster gst on gst.Id=c.OurGSTIN where c.ClientId  = '" + unitId + "' ";
            DataTable dtBillPrefix = config.ExecuteReaderWithQueryAsync(strBillprefix).Result;
            string billPrefix = "";
            if (dtBillPrefix.Rows.Count > 0)
            {
                billPrefix = dtBillPrefix.Rows[0]["BillPrefix"].ToString();
            }
            //string strQry = "Select BillNo from UnitBill where UnitId='" + unitId + "' And Month=" + month;
            //  DataTable noTable = SqlHelper.Instance.GetTableByQuery(strQry);
            //  if (noTable.Rows.Count > 0)
            //  {
            // if (noTable.Rows[0]["billno"].ToString().Length > 0)
            //   {
            //         billno = noTable.Rows[0]["billno"].ToString();
            //    }
            // }
            // else
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
                    btninvoice.Enabled = true;
                }
                else
                {
                    btngenratepayment.Enabled = false;
                    btninvoice.Enabled = false;
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

        public string GetMonthNew()
        {
            string DateVal = "";
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
                        return "0";
                    }
                    EnteredDate = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return "0";
                }
            }
            #endregion


            #region  Month Get Based on the Control Selection
            int month = 0;
            if (Chk_Month.Checked == false)
            {
                month = Timings.Instance.GetIdForSelectedMonth(ddlmonth.SelectedIndex);
                DateVal = monthval(month);

            }
            if (Chk_Month.Checked == true)
            {
                DateTime date = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                month = Timings.Instance.GetIdForEnteredMOnth(date);
                DateVal = monthval(month);
            }
            return DateVal;

            #endregion


        }


        public string monthval(int month)
        {
            string monthnew = "";

            if (month.ToString().Length == 3)
            {
                monthnew = month.ToString().Substring(1, 2) + 0 + month.ToString().Substring(0, 1);
            }
            else
            {
                monthnew = month.ToString().Substring(2, 2) + month.ToString().Substring(0, 2);
            }

            return monthnew;

        }

        protected void txtmonthOnTextChanged(object sender, EventArgs e)
        {
            lblbillnolatest.Text = "";
            txtbilldate.Text = "";
            FillMonthDetails();
            LoadOldBillnos();
            DisplayDataInGrid();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {


            var password = string.Empty;
            var SPName = string.Empty;
            password = txtPassword.Text.Trim();
            string sqlPassword = "select password from IouserDetails where password='" + txtPassword.Text + "'";
            DataTable dtpassword = config.ExecuteReaderWithQueryAsync(sqlPassword).Result;
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
            ddlMBBillnos.SelectedIndex = 0;
            //txtduedate.Text = string.Empty;
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
            ddlMBBillnos.SelectedIndex = 0;
            //txtduedate.Text = string.Empty;
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

        protected void ddldutytype_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddldutytype = sender as DropDownList;
            GridViewRow row = null;
            if (ddldutytype == null)
                return;

            row = (GridViewRow)ddldutytype.NamingContainer;
            if (row == null)
                return;


            TextBox txtnoofemployees = row.FindControl("txtnoofemployees") as TextBox;
            TextBox txtNoOfDuties = row.FindControl("txtNoOfDuties") as TextBox;
            TextBox txtPayRate = row.FindControl("txtPayRate") as TextBox;
            TextBox txtda = row.FindControl("txtda") as TextBox;
            TextBox txtAmount = row.FindControl("txtAmount") as TextBox;

            if (ddldutytype.SelectedIndex == 5)
            {
                txtnoofemployees.Text = "";
                txtNoOfDuties.Text = "";
                txtPayRate.Text = "";
                txtda.Text = "";
                txtAmount.Text = "";
            }
        }
    }
}