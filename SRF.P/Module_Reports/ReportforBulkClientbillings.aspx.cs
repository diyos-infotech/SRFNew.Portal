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

namespace SRF.P.Module_Reports
{
    public partial class ReportforBulkClientbillings : System.Web.UI.Page
    {
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string FontStyle = "Tahoma";

        AppConfiguration config = new AppConfiguration();

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


            }
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

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();

            if (txtmonth.Text.Trim().Length == 0)
            {
                LblResult.Text = "Please Select Month";
                return;
            }

            var testDate = 0;
            if (txtmonth.Text.Trim().Length > 0)
            {
                testDate = GlobalData.Instance.CheckEnteredDate(txtmonth.Text);
                if (testDate > 0)
                {
                    LblResult.Text = "You Are Entered Invalid Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                    return;
                }

            }
            string date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();


            #region Begin Variable Declaration as on [04-07-2014]
            var Month = "";
            var SPName = "";
            var clientids = "";
            int status = 0;
            DataTable DtClientListBasedonSelectedMonth = null;
            Hashtable HtClientListBasedonSelectedMonth = new Hashtable();
            #endregion End  Variable Declaration as on [04-07-2014]

            #region Begin Assign Values To Vriables as on [04-07-2014]
            Month = month + Year.Substring(2, 2);
            status = ddlBillType.SelectedIndex;

            SPName = "GetClientsbulkbillprint";
            #endregion  End Assing Values to The Variables as on [04-07-2014]

            #region Begin  Pass Values To the Hash table as on [04-07-2014]
            HtClientListBasedonSelectedMonth.Add("@month", Month);
            HtClientListBasedonSelectedMonth.Add("@status", status);
            //HtClientListBasedonSelectedMonth.Add("@clientids", clientids);
            #endregion end Pass Values To the Hash table as on [04-07-2014]

            #region  Begin Call Sp on [04-07-2014]
            DtClientListBasedonSelectedMonth = config.ExecuteAdaptorAsyncWithParams(SPName, HtClientListBasedonSelectedMonth).Result;
            GVListEmployees.DataSource = DtClientListBasedonSelectedMonth;
            GVListEmployees.DataBind();
            #endregion  end Call Sp on [04-07-2014]
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            int fontsize = 10;
            var Month = "";

            if (txtmonth.Text.Trim().Length == 0)
            {
                LblResult.Text = "Please Select Month";
                return;
            }

            var testDate = 0;
            if (txtmonth.Text.Trim().Length > 0)
            {
                testDate = GlobalData.Instance.CheckEnteredDate(txtmonth.Text);
                if (testDate > 0)
                {
                    LblResult.Text = "You have entered Invalid Date.Date Format Should be [DD/MM/YYYY].Ex.26/01/1990";
                    return;
                }

            }
            if (ddlOptions.SelectedIndex == 0)
            {


                string date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                string month = DateTime.Parse(date).Month.ToString();
                string Year = DateTime.Parse(date).Year.ToString();
                Month = month + Year.Substring(2, 2);
                var list = new List<string>();

                MemoryStream ms = new MemoryStream();

                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                document.AddTitle("Webwonders");
                document.AddAuthor("DIYOS");
                document.AddSubject("Invoice");
                document.AddKeywords("Keyword1, keyword2, …");
                string imagepath = Server.MapPath("~/assets/billlogo.png");
                try
                {
                    if (GVListEmployees.Rows.Count > 0)
                    {
                        for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                        {

                            CheckBox chkclientid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                            Label lblclientid = GVListEmployees.Rows[i].FindControl("lblclientid") as Label;
                            Label lblbillno = GVListEmployees.Rows[i].FindControl("lblbillno") as Label;
                            if (chkclientid.Checked == true)
                            {


                                //list.Add(lblclientid.Text);

                                //if (list.Count != 0)
                                {
                                    //foreach (string clientid in list)
                                    {
                                        #region
                                        string companyName = "Your Company Name";
                                        string note = "";
                                        string companyAddress = "Your Company Address";
                                        string companyaddressline = " ";

                                        string Servicetax = string.Empty;
                                        string PTno = string.Empty;

                                        string ServicetaxNo = string.Empty;
                                        string PANNO = string.Empty;
                                        string PFNo = string.Empty;
                                        string Esino = string.Empty;
                                        string SACCode = string.Empty;

                                        string strQry = "Select * from CompanyInfo   where   ClientidPrefix='" + CmpIDPrefix + "'";
                                        DataTable compInfo = config.ExecuteReaderWithQueryAsync(strQry).Result;
                                        if (compInfo.Rows.Count > 0)
                                        {
                                            Servicetax = compInfo.Rows[0]["BillNotes"].ToString();
                                            PTno = compInfo.Rows[0]["bankname"].ToString();
                                            SACCode = compInfo.Rows[0]["SACCode"].ToString();

                                        }


                                        #region For GST CODE
                                        DateTime dtn = DateTime.ParseExact(txtmonth.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        // for both "1/1/2000" or "25/1/2000" formats
                                        string billdt = dtn.ToString("MM/dd/yyyy");

                                        string BQry = "select * from TblOptions  where '" + billdt + "' between fromdate and todate";
                                        DataTable Bdt = config.ExecuteReaderWithQueryAsync(BQry).Result;

                                        string CGSTAlias = "";
                                        string SGSTAlias = "";
                                        string IGSTAlias = "";
                                        string Cess1Alias = "";
                                        string Cess2Alias = "";
                                        string OurGSTINAlias = "";
                                        string GSTINAlias = "";
                                        var SqlQryForTaxes = @"select ServiceTaxSeparate,Cess,SHECess,SBCess,KKCess,CGST,SGST,IGST,cess1,cess2,CGSTAlias,SGSTAlias,IGSTAlias,cess1Alias,cess2Alias,GSTINAlias,OurGSTINAlias,* from TblOptions where '" + billdt + "' between fromdate and todate ";
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
                                            LblResult.Text = "There Is No Tax Values For Generating Bills ";
                                            return;
                                        }

                                        #endregion

                                        var ContractID = "";
                                        DateTime DtLastDay = DateTime.Now;
                                        DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));

                                        #region  Begin Get Contract Id Based on The Last Day

                                        Hashtable HtGetContractID = new Hashtable();
                                        var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                                        HtGetContractID.Add("@clientid", lblclientid.Text);
                                        HtGetContractID.Add("@LastDay", DtLastDay);
                                        DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                                        if (DTContractID.Rows.Count > 0)
                                        {
                                            ContractID = DTContractID.Rows[0]["contractid"].ToString();
                                        }
                                        //else
                                        //{
                                        //    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not  Avaialable For This Client.');", true);
                                        //    return;
                                        //}

                                        #endregion  End Get Contract Id Based on The Last Day

                                        string ServiceCharge = "0";
                                        string strSCType = "";
                                        float srvcharge = 0;
                                        string strDescription = "We are presenting our bill for the Security Services Provided at your establishment";
                                        bool bSCType = false;
                                        string strIncludeST = "";
                                        string strST75 = "";
                                        bool bIncludeST = false;
                                        bool bST75 = false;


                                        string strStaxonservicecharge = "";
                                        bool bStaxonservicecharge = false;
                                        string emailid = "";
                                        string faxno = "";
                                        string phoneno = "";
                                        string BillNo = "";
                                        DateTime BillDate;
                                        string FromDate = "";
                                        string ToDate = "";
                                        int status = 0;
                                        string billdates = "0";

                                        #region Variables for data Fields as on 11/03/2014 by venkat


                                        float servicecharge = 0;
                                        float servicetax = 0;
                                        float servicetaxprc = 0;
                                        float sbcess = 0;
                                        float kkcess = 0;
                                        float kkcessprc = 0;
                                        float Sbcessprc = 0;
                                        float cess = 0;
                                        float shecess = 0;
                                        float totalamount = 0;
                                        float Grandtotal = 0;
                                        float RelChrgAmt = 0;
                                        float ServiceTax75 = 0;
                                        float ServiceTax25 = 0;

                                        #region for GST on 20-6-2017 by sharada

                                        float CGST = 0;
                                        float SGST = 0;
                                        float IGST = 0;
                                        float Cess1 = 0;
                                        float Cess2 = 0;
                                        float CGSTPrc = 0;
                                        float SGSTPrc = 0;
                                        float IGSTPrc = 0;
                                        float Cess1Prc = 0;
                                        float Cess2Prc = 0;

                                        #endregion for GST on 20-6-2017 by sharada

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

                                        string strExtradatacheck = "";

                                        string strExtrastcheck = "";
                                        string strMachinary = "";
                                        string strMaterial = "";
                                        string strMaintenance = "";
                                        string strExtraone = "";
                                        string strExtratwo = "";

                                        bool SCMachinary = false;
                                        bool SCMaterial = false;
                                        bool SCMaintenance = false;
                                        bool SCExtraone = false;
                                        bool SCExtratwo = false;

                                        string strSCMachinary = "";
                                        string strSCMaterial = "";
                                        string strSCMaintenance = "";
                                        string strSCExtraone = "";
                                        string strSCExtratwo = "";

                                        float SCamtonMachinary = 0;
                                        float SCamtonMaintenance = 0;
                                        float SCamtonMaterial = 0;
                                        float SCamtonExtraone = 0;
                                        float SCamtonExtratwo = 0;

                                        string strStaxonExtradataservicecharges = "";
                                        bool bStaxonExtradataservicecharges = false;

                                        string strSTDiscountone = "";
                                        string strSTDiscounttwo = "";

                                        bool STDiscountone = false;
                                        bool STDiscounttwo = false;


                                        #endregion

                                        string SubunitName = "";
                                        status = ddlBillType.SelectedIndex;


                                        string SPName = "";
                                        Hashtable htbilling = new Hashtable();
                                        SPName = "GetpdfsformonthlywiseInvoces";
                                        htbilling.Add("@clientid", lblclientid.Text);
                                        htbilling.Add("@month", Month);
                                        htbilling.Add("@status", status);
                                        htbilling.Add("@billnum", lblbillno.Text);
                                        htbilling.Add("@contractid", ContractID);


                                        //  htbilling.Add("@Contractid", ContractID);

                                        DataTable DtBilling = config.ExecuteAdaptorAsyncWithParams(SPName, htbilling).Result;


                                        string selectclientaddress = "select sg.segname,c.*,s.state as Statename,s.GSTStateCode,gst.gstno from clients c inner join Segments sg on c.ClientSegment = sg.SegId  left join states s on s.stateid=c.state left join GSTMaster gst on gst.id=c.ourgstin where clientid='" + lblclientid.Text + "'";
                                        DataTable dtclientaddress = config.ExecuteReaderWithQueryAsync(selectclientaddress).Result;

                                        string OurGSTIN = "";
                                        string GSTIN = "";
                                        string StateCode = "0";
                                        string State = "";
                                        var BankAccountNo = "";
                                        var IFSCCode = "";
                                        var BankName = "";
                                        if (dtclientaddress.Rows.Count > 0)
                                        {
                                            OurGSTIN = dtclientaddress.Rows[0]["gstno"].ToString();
                                            StateCode = dtclientaddress.Rows[0]["GSTStateCode"].ToString();
                                            GSTIN = dtclientaddress.Rows[0]["GSTIN"].ToString();
                                            State = dtclientaddress.Rows[0]["Statename"].ToString();
                                        }
                                        if (DtBilling.Rows.Count > 0)
                                        {
                                            #region
                                            companyName = DtBilling.Rows[0]["CompanyName"].ToString();
                                            companyAddress = DtBilling.Rows[0]["Address"].ToString();
                                            note = DtBilling.Rows[0]["Notes"].ToString();
                                            phoneno = DtBilling.Rows[0]["Phoneno"].ToString();
                                            faxno = DtBilling.Rows[0]["Faxno"].ToString();
                                            emailid = DtBilling.Rows[0]["Emailid"].ToString();


                                            ServicetaxNo = DtBilling.Rows[0]["BillNotes"].ToString();
                                            PANNO = DtBilling.Rows[0]["Labourrule"].ToString();
                                            PFNo = DtBilling.Rows[0]["PFNo"].ToString();
                                            Esino = DtBilling.Rows[0]["ESINo"].ToString();

                                            ContractID = DtBilling.Rows[0]["contractid"].ToString();

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["servicecharge"].ToString()) == false)
                                            {
                                                ServiceCharge = DtBilling.Rows[0]["servicecharge"].ToString();
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceChargeType"].ToString()) == false)
                                            {
                                                strSCType = DtBilling.Rows[0]["ServiceChargeType"].ToString();
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Scharge"].ToString()) == false)
                                            {
                                                srvcharge = float.Parse(DtBilling.Rows[0]["Scharge"].ToString());

                                            }
                                            string tempDescription = DtBilling.Rows[0]["Description"].ToString();
                                            if (tempDescription.Trim().Length > 0)
                                            {
                                                strDescription = tempDescription;
                                            }
                                            if (strSCType.Length > 0)
                                            {
                                                bSCType = Convert.ToBoolean(strSCType);
                                            }
                                            strIncludeST = DtBilling.Rows[0]["IncludeST"].ToString();
                                            strST75 = DtBilling.Rows[0]["ServiceTax75"].ToString();
                                            if (strIncludeST == "True")
                                            {
                                                bIncludeST = true;
                                            }
                                            if (strST75 == "True")
                                            {
                                                bST75 = true;
                                            }

                                            //strStaxonservicecharge = DtBilling.Rows[0]["Staxonservicecharge"].ToString();
                                            //if (strStaxonservicecharge == "True")
                                            //{
                                            //    bStaxonservicecharge = true;
                                            //}

                                            BankAccountNo = DtBilling.Rows[0]["BankAccountNo"].ToString();
                                            IFSCCode = DtBilling.Rows[0]["IFSCCode"].ToString();
                                            BankName = DtBilling.Rows[0]["BankName"].ToString();
                                            BillNo = DtBilling.Rows[0]["billno"].ToString();
                                            BillDate = Convert.ToDateTime(DtBilling.Rows[0]["billdt"].ToString());
                                            FromDate = (DtBilling.Rows[0]["FromDt"].ToString());
                                            ToDate = (DtBilling.Rows[0]["ToDt"].ToString());
                                            billdates = DtBilling.Rows[0]["billdates"].ToString();

                                            #region Begin New code for values taken from database as on 11/03/2014 by venkat

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["dutiestotalamount"].ToString()) == false)
                                            {
                                                totalamount = float.Parse(DtBilling.Rows[0]["dutiestotalamount"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["RelChrgAmt"].ToString()) == false)
                                            {
                                                RelChrgAmt = float.Parse(DtBilling.Rows[0]["RelChrgAmt"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["servicecharge"].ToString()) == false)
                                            {
                                                servicecharge = float.Parse(DtBilling.Rows[0]["servicecharge"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax"].ToString()) == false)
                                            {
                                                servicetax = float.Parse(DtBilling.Rows[0]["ServiceTax"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTaxPrc"].ToString()) == false)
                                            {
                                                servicetaxprc = float.Parse(DtBilling.Rows[0]["ServiceTaxPrc"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SBCessTaxPrc"].ToString()) == false)
                                            {
                                                Sbcessprc = float.Parse(DtBilling.Rows[0]["SBCessTaxPrc"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["KKCessTaxPrc"].ToString()) == false)
                                            {
                                                kkcessprc = float.Parse(DtBilling.Rows[0]["KKCessTaxPrc"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SBCessAmt"].ToString()) == false)
                                            {
                                                sbcess = float.Parse(DtBilling.Rows[0]["SBCessAmt"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["KKCessAmt"].ToString()) == false)
                                            {
                                                kkcess = float.Parse(DtBilling.Rows[0]["KKCessAmt"].ToString());
                                            }


                                            #region for GST as on 20-6-2017 by sharada

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CGSTAmt"].ToString()) == false)
                                            {
                                                CGST = float.Parse(DtBilling.Rows[0]["CGSTAmt"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SGSTAmt"].ToString()) == false)
                                            {
                                                SGST = float.Parse(DtBilling.Rows[0]["SGSTAmt"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["IGSTAmt"].ToString()) == false)
                                            {
                                                IGST = float.Parse(DtBilling.Rows[0]["IGSTAmt"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess1Amt"].ToString()) == false)
                                            {
                                                Cess1 = float.Parse(DtBilling.Rows[0]["Cess1Amt"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess2Amt"].ToString()) == false)
                                            {
                                                Cess2 = float.Parse(DtBilling.Rows[0]["Cess2Amt"].ToString());
                                            }


                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CGSTPrc"].ToString()) == false)
                                            {
                                                CGSTPrc = float.Parse(DtBilling.Rows[0]["CGSTPrc"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SGSTPrc"].ToString()) == false)
                                            {
                                                SGSTPrc = float.Parse(DtBilling.Rows[0]["SGSTPrc"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["IGSTPrc"].ToString()) == false)
                                            {
                                                IGSTPrc = float.Parse(DtBilling.Rows[0]["IGSTPrc"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess1Prc"].ToString()) == false)
                                            {
                                                Cess1Prc = float.Parse(DtBilling.Rows[0]["Cess1Prc"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess2Prc"].ToString()) == false)
                                            {
                                                Cess2Prc = float.Parse(DtBilling.Rows[0]["Cess2Prc"].ToString());
                                            }

                                            #endregion for GST as on 17-6-2017 by swathi
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



                                            #endregion

                                            SubunitName = DtBilling.Rows[0]["Subunitname"].ToString();
                                            #endregion
                                        }

                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Generate The Bill Once Again');", true);
                                            return;
                                        }

                                        document.AddTitle(companyName);
                                        document.AddAuthor("DIYOS");
                                        document.AddSubject("Invoice");
                                        document.AddKeywords("Keyword1, keyword2, …");
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

                                        PdfPCell CCompAddress = new PdfPCell(new Paragraph(companyAddress, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
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

                                        PdfPCell CCompPhone = new PdfPCell(new Paragraph("Tel: " + phoneno, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
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

                                        PdfPCell CCompFax = new PdfPCell(new Paragraph(" Fax: " + faxno, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
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


                                        PdfPCell Celemail = new PdfPCell(new Paragraph("Email :" + emailid, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
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

                                        //tablelogo.AddCell(celll);






                                        //PdfPTable tempTable1 = new PdfPTable(4);
                                        //tempTable1.TotalWidth = 520f;
                                        //tempTable1.LockedWidth = true;
                                        ////tempTable1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //float[] tempWidth1 = new float[] { 3f, 2f, 1f, 3f };
                                        //tempTable1.SetWidths(tempWidth1);

                                        int sno = 0;

                                        float totaldts = 0;
                                        float totalmanpower = 0;



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




                                        string Year1 = DateTime.Now.Year.ToString();

                                        PdfPCell mress = new PdfPCell(new Paragraph("Customer's Details:", FontFactory.GetFont(FontStyle, 10, Font.UNDERLINE | Font.BOLD, BaseColor.BLACK)));
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
                                            PdfPCell clientaddrhno = new PdfPCell(new Paragraph("M/s. " + addressData, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
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
                                            PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
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
                                            PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
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
                                            PdfPCell clientcolony = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
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
                                            PdfPCell clientcity = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
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
                                            PdfPCell clientstate = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
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
                                            PdfPCell clietnpin = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
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
                                                PdfPCell clietnpin = new PdfPCell(new Paragraph(GSTINAlias + ":  " + GSTIN, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
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
                                                PdfPCell clietnpin = new PdfPCell(new Paragraph("State: " + State, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
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
                                                PdfPCell clietnpin = new PdfPCell(new Paragraph("State Code:  " + StateCode, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
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
                                        phrase.Add(new Chunk("Bill No", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
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
                                        phrase10.Add(new Chunk(": " + BillNo, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
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
                                        phrase11.Add(new Chunk("Date", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
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
                                            BillDate.Year, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
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
                                        phrase12.Add(new Chunk("PAN No.", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
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
                                        phrase12v.Add(new Chunk(": " + PANNO, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
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
                                                var phrase2 = new Phrase();
                                                phrase2.Add(new Chunk("S Tax No", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
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
                                                phrase2v.Add(new Chunk(": " + Servicetax, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
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
                                        }
                                        if (Bdt.Rows.Count > 0)
                                        {

                                            if (OurGSTIN.Length > 0)
                                            {

                                                var phrase21v = new Phrase();
                                                phrase21v.Add(new Chunk(OurGSTINAlias, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                PdfPCell cell16v = new PdfPCell();
                                                cell16v.AddElement(phrase21v);
                                                cell16v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                cell16v.BorderWidthBottom = 0;
                                                cell16v.BorderWidthTop = 0f;
                                                cell16v.Colspan = 1;
                                                cell16v.BorderWidthLeft = 0.5f;
                                                cell16v.BorderWidthRight = 0f;
                                                cell16v.PaddingTop = -5;
                                                tempTable2.AddCell(cell16v);

                                                var phrase21vv = new Phrase();
                                                phrase21vv.Add(new Chunk(": " + OurGSTIN, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                PdfPCell cell16vv = new PdfPCell();
                                                cell16vv.AddElement(phrase21vv);
                                                cell16vv.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                cell16vv.BorderWidthBottom = 0;
                                                cell16vv.BorderWidthTop = 0f;
                                                cell16vv.PaddingLeft = -50f;
                                                cell16vv.Colspan = 1;
                                                cell16vv.BorderWidthLeft = 0;
                                                cell16vv.BorderWidthRight = 1.5f;
                                                cell16vv.PaddingTop = -5;
                                                tempTable2.AddCell(cell16vv);
                                            }
                                        }

                                        if (SACCode.Trim().Length > 0)
                                        {
                                            var phrase222 = new Phrase();
                                            phrase222.Add(new Chunk("SAC Code", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
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
                                            phrase222v.Add(new Chunk(": " + SACCode, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
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

                                        //FromDate = (DtBilling.Rows[0]["FromDt"].ToString());
                                        //ToDate = (DtBilling.Rows[0]["ToDt"].ToString());
                                        //string Fromdate = txtfromdate.Text;
                                        //string Todate = txttodate.Text;

                                        if (billdates != "0")
                                        {
                                            var phrase2 = new Phrase();
                                            phrase2.Add(new Chunk("Bill Period  ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
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
                                            phrase2v.Add(new Chunk(": " + FromDate + "  to  " +
                                                ToDate + " ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
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




                                        PdfPTable bodytablelogo = new PdfPTable(2);
                                        bodytablelogo.TotalWidth = 560f;//600f
                                        bodytablelogo.LockedWidth = true;
                                        float[] widthlogo = new float[] { 2f, 2f };
                                        bodytablelogo.SetWidths(widthlogo);

                                        PdfPCell cellser1 = new PdfPCell(new Phrase("Sub: -We are presenting our bill for the Security Services provided at your establishment for the month of " + GetMonthName() + " " + GetMonthOfYear() + ".Kindly release the payment at the earliest " /*+ GetMonthOfYear()*/, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        cellser1.HorizontalAlignment = 0;
                                        cellser1.Colspan = 2;
                                        cellser1.BorderWidthBottom = 0.5f;
                                        cellser1.BorderWidthLeft = 1.5f;
                                        cellser1.BorderWidthTop = 0.5f;
                                        cellser1.BorderWidthRight = 1.5f;
                                        cellser1.MinimumHeight = 25;
                                        bodytablelogo.AddCell(cellser1);

                                        document.Add(bodytablelogo);

                                        int colCount = 6;// gvClientBilling.Columns.Count;
                                                         //Create a table

                                        #region Code For Data table


                                        PdfPTable table = new PdfPTable(colCount);
                                        table.TotalWidth = 560f;
                                        table.LockedWidth = true;
                                        table.HorizontalAlignment = 1;
                                        float[] colWidths = new float[] { 1.2f, 6.2f, 2f, 2.2f, 2f, 2.7f };
                                        table.SetWidths(colWidths);
                                        #region


                                        PdfPCell cell;
                                        string cellText;
                                        //create the header row
                                        for (int colIndex = 0; colIndex < 6; colIndex++)
                                        {
                                            if (colIndex == 0)
                                            {
                                                PdfPCell CSNo = new PdfPCell(new Phrase("S.No", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                CSNo.HorizontalAlignment = 1;
                                                CSNo.BorderWidthBottom = .5f;
                                                CSNo.BorderWidthLeft = 1.5f;
                                                CSNo.BorderWidthTop = 0f;
                                                CSNo.BorderWidthRight = .5f;
                                                table.AddCell(CSNo);
                                            }

                                            if (colIndex == 1)
                                            {
                                                cell = new PdfPCell(new Phrase("Description", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                //set the background color for the header cell
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = .5f;
                                                cell.BorderWidthLeft = 0.5f;
                                                cell.BorderWidthTop = 0f;
                                                cell.BorderWidthRight = .5f;
                                                table.AddCell(cell);
                                            }
                                            if (colIndex == 2)
                                            {

                                                cell = new PdfPCell(new Phrase("No Of Days Per Month", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                //set the background color for the header cell
                                                cell.HorizontalAlignment = 1;
                                                //cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = .5f;
                                                cell.BorderWidthLeft = 0.5f;
                                                cell.BorderWidthTop = 0f;
                                                cell.BorderWidthRight = .5f;
                                                table.AddCell(cell);
                                            }
                                            if (colIndex == 3)
                                            {
                                                cell = new PdfPCell(new Phrase("No of shifts", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                //set the background color for the header cell
                                                cell.HorizontalAlignment = 1;
                                                //cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = .5f;
                                                cell.BorderWidthLeft = 0.5f;
                                                cell.BorderWidthTop = 0f;
                                                cell.BorderWidthRight = .5f;
                                                table.AddCell(cell);
                                            }

                                            if (colIndex == 4)
                                            {

                                                cell = new PdfPCell(new Phrase("Rate(Rs)", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                //set the background color for the header cell
                                                cell.HorizontalAlignment = 1;
                                                //cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = .5f;
                                                cell.BorderWidthLeft = 0.5f;
                                                cell.BorderWidthTop = 0f;
                                                cell.BorderWidthRight = .5f;
                                                table.AddCell(cell);
                                            }
                                            if (colIndex == 5)
                                            {
                                                cell = new PdfPCell(new Phrase("Amount (Rs)", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                //set the background color for the header cell
                                                cell.HorizontalAlignment = 1;
                                                //cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = .5f;
                                                cell.BorderWidthLeft = 0.5f;
                                                cell.BorderWidthTop = 0f;
                                                cell.BorderWidthRight = 1.5f;
                                                table.AddCell(cell);
                                            }

                                        }
                                        #endregion
                                        float totaldtss = 0;
                                        ////export rows from GridView to table
                                        #region For GV values
                                        string spUnitbillbreakup = "GetpdfforClientbillingfromunitbillbreakup";
                                        Hashtable htunitbillbreakup = new Hashtable();
                                        htunitbillbreakup.Add("@Clientid", lblclientid.Text);
                                        htunitbillbreakup.Add("@month", Month);
                                        htunitbillbreakup.Add("@status", status);
                                        htunitbillbreakup.Add("@munitibillno", lblbillno.Text);

                                        DataTable dtunitbillbreakup = config.ExecuteAdaptorAsyncWithParams(spUnitbillbreakup, htunitbillbreakup).Result;

                                        for (int rowIndex = 0; rowIndex < dtunitbillbreakup.Rows.Count; rowIndex++)
                                        {




                                            string lblamount = dtunitbillbreakup.Rows[rowIndex]["Dutiesamount"].ToString();
                                            if (lblamount != string.Empty)
                                            {
                                                float amount = 0;
                                                if (lblamount.Length > 0)
                                                    amount = Convert.ToSingle(lblamount);
                                                if (amount >= 0)
                                                {
                                                    for (int j = 0; j < 6; j++)
                                                    {
                                                        if (j == 0)
                                                        {
                                                            PdfPCell CSNo = new PdfPCell(new Phrase((++sno).ToString() + "\n", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                            CSNo.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                                            CSNo.HorizontalAlignment = 1;
                                                            CSNo.BorderWidthBottom = 0f;
                                                            CSNo.BorderWidthLeft = 1.5f;
                                                            CSNo.BorderWidthTop = 0;
                                                            CSNo.BorderWidthRight = 0.5f;
                                                            table.AddCell(CSNo);
                                                        }

                                                        //fetch the column value of the current row
                                                        if (j == 1)
                                                        {
                                                            string design = dtunitbillbreakup.Rows[rowIndex]["Designation"].ToString();

                                                            cellText = design;

                                                            //create a new cell with column value
                                                            cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                            cell.BorderWidthBottom = 0f;
                                                            cell.BorderWidthLeft = .5f;
                                                            cell.BorderWidthTop = 0;
                                                            cell.BorderWidthRight = 0.5f;
                                                            table.AddCell(cell);
                                                        }
                                                        if (j == 2)
                                                        {
                                                            string payratetype = dtunitbillbreakup.Rows[rowIndex]["Payratetype"].ToString();
                                                            string monthlydays = dtunitbillbreakup.Rows[rowIndex]["monthlydays"].ToString();
                                                            cellText = monthlydays;
                                                            if (payratetype == "5")
                                                            {
                                                                cellText = "";
                                                            }
                                                            else
                                                            {
                                                                cellText = monthlydays;
                                                            }
                                                            if (cellText.Length > 0)
                                                            {
                                                                totalmanpower += float.Parse(cellText);
                                                            }
                                                            //create a new cell with column value
                                                            cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                            cell.HorizontalAlignment = 1;
                                                            cell.BorderWidthBottom = 0f;
                                                            cell.BorderWidthLeft = 0.5f;
                                                            cell.BorderWidthTop = 0;
                                                            cell.BorderWidthRight = .5f;
                                                            table.AddCell(cell);

                                                        }
                                                        if (j == 3)
                                                        {

                                                            string Noofduties = dtunitbillbreakup.Rows[rowIndex]["Duties"].ToString();
                                                            if (Noofduties == "0" || Noofduties == "")
                                                            {
                                                                cellText = "";
                                                            }
                                                            else
                                                            {
                                                                cellText = Noofduties;

                                                            }
                                                            if (cellText.Length > 0)
                                                            {
                                                                totaldts += float.Parse(cellText);
                                                            }
                                                            //create a new cell with column value
                                                            cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                            cell.HorizontalAlignment = 1;
                                                            cell.BorderWidthBottom = 0f;
                                                            cell.BorderWidthLeft = .5f;
                                                            cell.BorderWidthTop = 0;
                                                            cell.BorderWidthRight = .5f;
                                                            table.AddCell(cell);
                                                            if (cellText == "")
                                                            {
                                                                cellText = "0";
                                                            }
                                                            totaldtss += Convert.ToSingle(cellText);
                                                        }
                                                        if (j == 4)
                                                        {
                                                            string payrate = dtunitbillbreakup.Rows[rowIndex]["payrate"].ToString();
                                                            string payratetype = dtunitbillbreakup.Rows[rowIndex]["Payratetype"].ToString();
                                                            cellText = payrate;

                                                            if (cellText == "0.0000")
                                                            {
                                                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                                cell.HorizontalAlignment = 2;
                                                                cell.BorderWidthBottom = 0f;
                                                                cell.BorderWidthLeft = .5f;
                                                                cell.BorderWidthTop = 0;
                                                                cell.BorderWidthRight = .5f;
                                                                table.AddCell(cell);
                                                            }
                                                            else
                                                            {
                                                                float payr = float.Parse(cellText);
                                                                //create a new cell with column value
                                                                cell = new PdfPCell(new Phrase(payr.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                                cell.HorizontalAlignment = 2;
                                                                cell.BorderWidthBottom = 0f;
                                                                cell.BorderWidthLeft = .5f;
                                                                cell.BorderWidthTop = 0;
                                                                cell.BorderWidthRight = .5f;
                                                                table.AddCell(cell);
                                                            }
                                                        }
                                                        if (j == 5)
                                                        {
                                                            string dutiesamount = dtunitbillbreakup.Rows[rowIndex]["Dutiesamount"].ToString();
                                                            cellText = dutiesamount;
                                                            //create a new cell with column value

                                                            if (cellText == "0.0000")
                                                            {
                                                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                                cell.HorizontalAlignment = 2;
                                                                //cell.Border = 0;
                                                                cell.BorderWidthBottom = 0f;
                                                                cell.BorderWidthLeft = 0.5f;
                                                                cell.BorderWidthTop = 0;
                                                                cell.BorderWidthRight = 1.5f;
                                                                cell.MinimumHeight = 14;
                                                                table.AddCell(cell);
                                                            }
                                                            else
                                                            {
                                                                float Payments = float.Parse(cellText);
                                                                cell = new PdfPCell(new Phrase(Payments.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                                cell.HorizontalAlignment = 2;
                                                                //cell.Border = 0;
                                                                cell.BorderWidthBottom = 0f;
                                                                cell.BorderWidthLeft = 0.5f;
                                                                cell.BorderWidthTop = 0;
                                                                cell.BorderWidthRight = 1.5f;
                                                                cell.MinimumHeight = 14;
                                                                table.AddCell(cell);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        #region for space
                                        PdfPCell Cellempty = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        Cellempty.HorizontalAlignment = 2;
                                        Cellempty.Colspan = 1;
                                        Cellempty.BorderWidthTop = 0;
                                        Cellempty.BorderWidthRight = 0.5f;
                                        Cellempty.BorderWidthLeft = 1.5f;
                                        Cellempty.BorderWidthBottom = 0;
                                        Cellempty.MinimumHeight = 12;

                                        PdfPCell Cellempty1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        Cellempty1.HorizontalAlignment = 2;
                                        Cellempty1.Colspan = 1;
                                        Cellempty1.BorderWidthTop = 0;
                                        Cellempty1.BorderWidthRight = 0.5f;
                                        Cellempty1.BorderWidthLeft = 0.5f;
                                        Cellempty1.BorderWidthBottom = 0;
                                        Cellempty1.MinimumHeight = 12;

                                        PdfPCell Cellempty2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        Cellempty2.HorizontalAlignment = 2;
                                        Cellempty2.Colspan = 1;
                                        Cellempty2.BorderWidthTop = 0;
                                        Cellempty2.BorderWidthRight = 0.5f;
                                        Cellempty2.BorderWidthLeft = 0.5f;
                                        Cellempty2.BorderWidthBottom = 0;
                                        Cellempty2.MinimumHeight = 12;

                                        PdfPCell Cellempty3 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        Cellempty3.HorizontalAlignment = 2;
                                        Cellempty3.Colspan = 1;
                                        Cellempty3.BorderWidthTop = 0;
                                        Cellempty3.BorderWidthRight = 0.5f;
                                        Cellempty3.BorderWidthLeft = 0.5f;
                                        Cellempty3.BorderWidthBottom = 0;
                                        Cellempty3.MinimumHeight = 12;

                                        PdfPCell Cellempty4 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        Cellempty4.HorizontalAlignment = 2;
                                        Cellempty4.Colspan = 1;
                                        Cellempty4.BorderWidthTop = 0;
                                        Cellempty4.BorderWidthRight = 0.5f;
                                        Cellempty4.BorderWidthLeft = 0.5f;
                                        Cellempty4.BorderWidthBottom = 0;
                                        Cellempty4.MinimumHeight = 12;

                                        PdfPCell Cellempty5 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        Cellempty5.HorizontalAlignment = 2;
                                        Cellempty5.Colspan = 1;
                                        Cellempty5.BorderWidthTop = 0;
                                        Cellempty5.BorderWidthRight = 1.5f;
                                        Cellempty5.BorderWidthLeft = 0.5f;
                                        Cellempty5.BorderWidthBottom = 0;
                                        Cellempty5.MinimumHeight = 12;

                                        if (dtunitbillbreakup.Rows.Count == 1)
                                        {
                                            #region For cell count

                                            if (servicetax == 0)
                                            {

                                                for (int k = 0; k < 18; k++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    table.AddCell(Cellempty1);
                                                    table.AddCell(Cellempty2);
                                                    table.AddCell(Cellempty3);
                                                    table.AddCell(Cellempty4);
                                                    table.AddCell(Cellempty5);
                                                }
                                            }

                                            else
                                            {
                                                for (int k = 0; k < 17; k++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    table.AddCell(Cellempty1);
                                                    table.AddCell(Cellempty2);
                                                    table.AddCell(Cellempty3);
                                                    table.AddCell(Cellempty4);
                                                    table.AddCell(Cellempty5);
                                                }
                                            }


                                            #endregion
                                        }
                                        if (dtunitbillbreakup.Rows.Count == 2)
                                        {
                                            #region For cell count

                                            if (servicetax == 0)
                                            {

                                                for (int k = 0; k < 18; k++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    table.AddCell(Cellempty1);
                                                    table.AddCell(Cellempty2);
                                                    table.AddCell(Cellempty3);
                                                    table.AddCell(Cellempty4);
                                                    table.AddCell(Cellempty5);
                                                }
                                            }

                                            else
                                            {
                                                for (int k = 0; k < 17; k++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    table.AddCell(Cellempty1);
                                                    table.AddCell(Cellempty2);
                                                    table.AddCell(Cellempty3);
                                                    table.AddCell(Cellempty4);
                                                    table.AddCell(Cellempty5);
                                                }
                                            }



                                            #endregion
                                        }
                                        if (dtunitbillbreakup.Rows.Count == 3)
                                        {
                                            #region For cell count

                                            if (servicetax == 0)
                                            {

                                                for (int k = 0; k < 16; k++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    table.AddCell(Cellempty1);
                                                    table.AddCell(Cellempty2);
                                                    table.AddCell(Cellempty3);
                                                    table.AddCell(Cellempty4);
                                                    table.AddCell(Cellempty5);
                                                }
                                            }

                                            else
                                            {
                                                for (int k = 0; k < 15; k++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    table.AddCell(Cellempty1);
                                                    table.AddCell(Cellempty2);
                                                    table.AddCell(Cellempty3);
                                                    table.AddCell(Cellempty4);
                                                    table.AddCell(Cellempty5);
                                                }
                                            }

                                            #endregion
                                        }
                                        if (dtunitbillbreakup.Rows.Count == 4)
                                        {
                                            if (servicetax == 0)
                                            {

                                                for (int k = 0; k < 15; k++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    table.AddCell(Cellempty1);
                                                    table.AddCell(Cellempty2);
                                                    table.AddCell(Cellempty3);
                                                    table.AddCell(Cellempty4);
                                                    table.AddCell(Cellempty5);
                                                }
                                            }

                                            else
                                            {
                                                for (int k = 0; k < 14; k++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    table.AddCell(Cellempty1);
                                                    table.AddCell(Cellempty2);
                                                    table.AddCell(Cellempty3);
                                                    table.AddCell(Cellempty4);
                                                    table.AddCell(Cellempty5);
                                                }
                                            }
                                        }
                                        if (dtunitbillbreakup.Rows.Count == 5)
                                        {
                                            if (servicetax == 0)
                                            {

                                                for (int k = 0; k < 14; k++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    table.AddCell(Cellempty1);
                                                    table.AddCell(Cellempty2);
                                                    table.AddCell(Cellempty3);
                                                    table.AddCell(Cellempty4);
                                                    table.AddCell(Cellempty5);
                                                }
                                            }

                                            else
                                            {
                                                for (int k = 0; k < 13; k++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    table.AddCell(Cellempty1);
                                                    table.AddCell(Cellempty2);
                                                    table.AddCell(Cellempty3);
                                                    table.AddCell(Cellempty4);
                                                    table.AddCell(Cellempty5);
                                                }
                                            }
                                        }
                                        if (dtunitbillbreakup.Rows.Count == 6)
                                        {
                                            if (servicetax == 0)
                                            {

                                                for (int k = 0; k < 13; k++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    table.AddCell(Cellempty1);
                                                    table.AddCell(Cellempty2);
                                                    table.AddCell(Cellempty3);
                                                    table.AddCell(Cellempty4);
                                                    table.AddCell(Cellempty5);
                                                }
                                            }

                                            else
                                            {
                                                for (int k = 0; k < 12; k++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    table.AddCell(Cellempty1);
                                                    table.AddCell(Cellempty2);
                                                    table.AddCell(Cellempty3);
                                                    table.AddCell(Cellempty4);
                                                    table.AddCell(Cellempty5);
                                                }
                                            }
                                        }
                                        if (dtunitbillbreakup.Rows.Count == 7)
                                        {
                                            if (servicetax == 0)
                                            {

                                                for (int k = 0; k < 12; k++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    table.AddCell(Cellempty1);
                                                    table.AddCell(Cellempty2);
                                                    table.AddCell(Cellempty3);
                                                    table.AddCell(Cellempty4);
                                                    table.AddCell(Cellempty5);
                                                }
                                            }

                                            else
                                            {
                                                for (int k = 0; k < 11; k++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    table.AddCell(Cellempty1);
                                                    table.AddCell(Cellempty2);
                                                    table.AddCell(Cellempty3);
                                                    table.AddCell(Cellempty4);
                                                    table.AddCell(Cellempty5);
                                                }
                                            }
                                        }


                                        #endregion
                                        document.Add(table);

                                        //tablelogo.AddCell(celll);

                                        PdfPTable tabled = new PdfPTable(colCount);
                                        tabled.TotalWidth = 560;//432f;
                                        tabled.LockedWidth = true;
                                        float[] widthd = new float[] { 1.2f, 6.2f, 2f, 2.2f, 2f, 2.7f };
                                        tabled.SetWidths(widthd);


                                        PdfPCell celldz11 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        celldz11.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldz11.Colspan = 1;
                                        celldz11.BorderWidthBottom = .5f;
                                        celldz11.BorderWidthLeft = 1.5f;
                                        celldz11.BorderWidthTop = 0.5f;
                                        celldz11.BorderWidthRight = 0.5f;
                                        //celldz1.BorderColor = BaseColor.LIGHT_GRAY;
                                        tabled.AddCell(celldz11);

                                        celldz11 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        celldz11.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldz11.Colspan = 1;
                                        celldz11.BorderWidthBottom = .5f;
                                        celldz11.BorderWidthLeft = 0.5f;
                                        celldz11.BorderWidthTop = 0.5f;
                                        celldz11.BorderWidthRight = 0.5f;
                                        //celldz1.BorderColor = BaseColor.LIGHT_GRAY;
                                        tabled.AddCell(celldz11);

                                        PdfPCell celldz1 = new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        celldz1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                        celldz1.Colspan = 1;
                                        celldz1.BorderWidthBottom = .5f;
                                        celldz1.BorderWidthLeft = 0.5f;
                                        celldz1.BorderWidthTop = 0.5f;
                                        celldz1.BorderWidthRight = .5f;
                                        //celldz1.BorderColor = BaseColor.LIGHT_GRAY;
                                        tabled.AddCell(celldz1);

                                        PdfPCell celldz12 = new PdfPCell(new Phrase(totaldtss.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        celldz12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                        celldz12.Colspan = 1;
                                        celldz12.BorderWidthBottom = .5f;
                                        celldz12.BorderWidthLeft = 0.5f;
                                        celldz12.BorderWidthTop = 0.5f;
                                        celldz12.BorderWidthRight = .5f;
                                        //celldz1.BorderColor = BaseColor.LIGHT_GRAY;
                                        tabled.AddCell(celldz12);

                                        PdfPCell celldz13 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        celldz13.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldz13.Colspan = 1;
                                        celldz13.BorderWidthBottom = .5f;
                                        celldz13.BorderWidthLeft = 0.5f;
                                        celldz13.BorderWidthTop = 0.5f;
                                        celldz13.BorderWidthRight = .5f;
                                        //celldz1.BorderColor = BaseColor.LIGHT_GRAY;
                                        tabled.AddCell(celldz13);

                                        PdfPCell celldz141 = new PdfPCell(new Phrase(" " + totalamount.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        celldz141.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldz141.Colspan = 1;
                                        celldz141.BorderWidthBottom = .5f;
                                        celldz141.BorderWidthLeft = 0.5f;
                                        celldz141.BorderWidthTop = 0.5f;
                                        celldz141.BorderWidthRight = 1.5f;
                                        celldz141.MinimumHeight = 20;
                                        //celldz1.BorderColor = BaseColor.LIGHT_GRAY;
                                        tabled.AddCell(celldz141);

                                        PdfPTable tempTable11 = new PdfPTable(3);
                                        tempTable11.TotalWidth = 323f; ;
                                        tempTable11.LockedWidth = true;
                                        float[] tempWidth21 = new float[] { 1f, 6.1f, 1.2f };//1.2f, 6.2f, 2f, 2.3f
                                        tempTable11.SetWidths(tempWidth21);
                                        #region 
                                        #region
                                        cell = new PdfPCell(new Phrase(" PLEASE NOTE : Payment Shall be made through RTGS only.", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 0;
                                        cell.BorderWidthBottom = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0;
                                        cell.BorderWidthLeft = 1.5f;
                                        cell.Colspan = 4;
                                        tempTable11.AddCell(cell);
                                        cell = new PdfPCell(new Phrase("  A/c.No " + BankAccountNo, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 0;
                                        cell.BorderWidthBottom = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0;
                                        cell.BorderWidthLeft = 1.5f;
                                        // cell.PaddingRight = 40;
                                        cell.Colspan = 4;
                                        tempTable11.AddCell(cell);
                                        cell = new PdfPCell(new Phrase("  IFSC Code : " + IFSCCode, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 0;
                                        cell.BorderWidthBottom = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0;
                                        cell.BorderWidthLeft = 1.5f;
                                        cell.Colspan = 4;
                                        tempTable11.AddCell(cell);
                                        cell = new PdfPCell(new Phrase("  " + BankName, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 0;
                                        cell.BorderWidthBottom = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0;
                                        cell.BorderWidthLeft = 1.5f;
                                        cell.Colspan = 4;
                                        tempTable11.AddCell(cell);
                                        #endregion

                                        #endregion

                                        PdfPCell Chid = new PdfPCell(tempTable11);
                                        Chid.Border = 0;
                                        Chid.Colspan = 3;
                                        Chid.HorizontalAlignment = 0;
                                        tabled.AddCell(Chid);

                                        PdfPTable tempTable22 = new PdfPTable(3);
                                        tempTable22.TotalWidth = 237f;
                                        tempTable22.LockedWidth = true;
                                        float[] tempWidth22 = new float[] { 1.8f, 1f, 1.8f }; ;//2.9f, 1.83f
                                        tempTable22.SetWidths(tempWidth22);


                                        if (RelChrgAmt > 0)
                                        {
                                            PdfPCell celldz5 = new PdfPCell(new Phrase("1/6 Reliever Charges", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                            celldz5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                            celldz5.Colspan = 2;
                                            celldz5.BorderWidthBottom = 0;
                                            celldz5.BorderWidthLeft = 0.5f;
                                            celldz5.BorderWidthTop = 0;
                                            celldz5.BorderWidthRight = 0f;
                                            //celldz1.BorderColor = BaseColor.LIGHT_GRAY;
                                            tempTable22.AddCell(celldz5);


                                            PdfPCell celldz6 = new PdfPCell(new Phrase(" " + RelChrgAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                            celldz6.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            celldz6.BorderWidthBottom = .5f;
                                            celldz6.Colspan = 1;
                                            celldz6.BorderWidthLeft = 0f;
                                            celldz6.BorderWidthTop = 0;
                                            celldz6.BorderWidthRight = .5f;
                                            //celldz4.BorderColor = BaseColor.LIGHT_GRAY;
                                            tempTable22.AddCell(celldz6);
                                        }



                                        #endregion









                                        int pfstatus = 0;
                                        int esistatus = 0;
                                        int panstatus = 0;
                                        int servicetaxstatus = 0;

                                        #region Code for Service Charge


                                        if (ServiceCharge.Length > 0)//bSCType == true)
                                        {
                                            float scharge1 = Convert.ToSingle(ServiceCharge);
                                            if (scharge1 > 0)
                                            {
                                                //if (servicecharge > 0 && ExtraDataSTcheck == false)
                                                //{



                                                string SCharge = "";
                                                if (bSCType == false)
                                                {
                                                    SCharge = ServiceCharge + " %";
                                                }
                                                else
                                                {
                                                    SCharge = ServiceCharge;
                                                }



                                                PdfPCell celldc3 = new PdfPCell(new Phrase("Service Charges @ " + srvcharge.ToString("#") + " %", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldc3.Colspan = 2;
                                                celldc3.HorizontalAlignment = 2;
                                                celldc3.BorderWidthBottom = 0;
                                                celldc3.BorderWidthLeft = 0f;
                                                celldc3.BorderWidthTop = 0;
                                                celldc3.BorderWidthRight = .5f;
                                                //celldc2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldc3);

                                                PdfPCell celldc4 = new PdfPCell(new Phrase(scharge1.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldc4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldc4.BorderWidthBottom = 0;
                                                celldc4.Colspan = 1;
                                                celldc4.BorderWidthLeft = 0.5f;
                                                celldc4.BorderWidthTop = 0f;
                                                celldc4.BorderWidthRight = 1.5f;
                                                //celldc4.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldc4);
                                            }
                                        }

                                        #endregion

                                        #region New code for Service charge amount on Extradata if Service Tax on Service Charge as on 04/04/2014 by venkat

                                        if (Extradatacheck == true && bStaxonExtradataservicecharges == true)
                                        {
                                            #region Extradata SCmachinary

                                            if (SCMachinary == true && SCamtonMachinary > 0)
                                            {


                                                PdfPCell celldcst1 = new PdfPCell(new Phrase("Service Charge on " + machinarycosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = 2;
                                                celldcst1.BorderWidthBottom = 0;
                                                celldcst1.BorderWidthLeft = 0f;
                                                celldcst1.BorderWidthTop = 0f;
                                                celldcst1.BorderWidthRight = .5f;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(SCamtonMachinary.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0f;
                                                celldcst2.Colspan = 1;
                                                celldcst2.BorderWidthLeft = 0.5f;
                                                celldcst2.BorderWidthTop = 0f;
                                                celldcst2.BorderWidthRight = 1.5f;
                                                tempTable22.AddCell(celldcst2);

                                            }
                                            #endregion

                                            #region Extradata SCmaterial

                                            if (SCMaterial == true && SCamtonMaterial > 0)
                                            {


                                                PdfPCell celldcst1 = new PdfPCell(new Phrase("Service Charge on " + materialcosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.BorderWidthBottom = 0f;
                                                celldcst1.BorderWidthLeft = 0f;
                                                celldcst1.BorderWidthTop = 0f;
                                                celldcst1.BorderWidthRight = .5f;
                                                //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(SCamtonMaterial.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = .5f;
                                                celldcst2.Colspan = 1;
                                                celldcst2.BorderWidthLeft = 0.5f;
                                                celldcst2.BorderWidthTop = 0f;
                                                celldcst2.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst2);

                                            }
                                            #endregion

                                            #region Extradata SCmaintenance

                                            if (SCMaintenance == true && SCamtonMaintenance > 0)
                                            {


                                                PdfPCell celldcst1 = new PdfPCell(new Phrase("Service Charge on " + maintenancecosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = 2;
                                                celldcst1.BorderWidthBottom = 0f;
                                                celldcst1.BorderWidthLeft = 0f;
                                                celldcst1.BorderWidthTop = 0f;
                                                celldcst1.BorderWidthRight = .5f;
                                                //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(SCamtonMaintenance.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0f;
                                                celldcst2.Colspan = 1;
                                                celldcst2.BorderWidthLeft = 0.5f;
                                                celldcst2.BorderWidthTop = 0f;
                                                celldcst2.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst2);

                                            }
                                            #endregion

                                            #region Extradata SCmaintenance

                                            if (SCExtraone == true && SCamtonExtraone > 0)
                                            {


                                                PdfPCell celldcst1 = new PdfPCell(new Phrase("Service Charge on " + extraonecosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = 2;
                                                celldcst1.BorderWidthBottom = 0f;
                                                celldcst1.BorderWidthLeft = 0f;
                                                celldcst1.BorderWidthTop = 0f;
                                                celldcst1.BorderWidthRight = .5f;
                                                //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(SCamtonExtraone.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0f;
                                                celldcst2.Colspan = 1;
                                                celldcst2.BorderWidthLeft = 0;
                                                celldcst2.BorderWidthTop = 0f;
                                                celldcst2.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst2);

                                            }
                                            #endregion

                                            #region Extradata SCmaintenance

                                            if (SCExtratwo == true && SCamtonExtratwo > 0)
                                            {


                                                PdfPCell celldcst1 = new PdfPCell(new Phrase("Service Charge on " + extratwocosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = 2;
                                                celldcst1.BorderWidthBottom = 0f;
                                                celldcst1.BorderWidthLeft = 0f;
                                                celldcst1.BorderWidthTop = 0f;
                                                celldcst1.BorderWidthRight = .5f;
                                                //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(SCamtonExtratwo.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0f;
                                                celldcst2.Colspan = 1;
                                                celldcst2.BorderWidthLeft = 0;
                                                celldcst2.BorderWidthTop = 0f;
                                                celldcst2.BorderWidthRight = .5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst2);

                                            }
                                            #endregion

                                        }

                                        #endregion

                                        #region When Extra data is checked and STcheck is true


                                        if (Extradatacheck == true)
                                        {
                                            #region Extradata Stmachinary


                                            if (machinarycost > 0 && STMachinary == true)
                                            {


                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = 2;
                                                celldcst1.BorderWidthBottom = 0f;
                                                celldcst1.BorderWidthLeft = 0f;
                                                celldcst1.BorderWidthTop = 0;
                                                celldcst1.BorderWidthRight = .5f;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0f;
                                                celldcst2.Colspan = 1;
                                                celldcst2.BorderWidthLeft = 0;
                                                celldcst2.BorderWidthTop = 0f;
                                                celldcst2.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst2);
                                            }

                                            #endregion

                                            #region Extradata StMaterial

                                            if (materialcost > 0 && STMaterial == true)
                                            {


                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = 2;
                                                celldcst1.BorderWidthBottom = 0f;
                                                celldcst1.BorderWidthLeft = 0f;
                                                celldcst1.BorderWidthTop = 0f;
                                                celldcst1.BorderWidthRight = .5f;
                                                //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0f;
                                                celldcst2.Colspan = 1;
                                                celldcst2.BorderWidthLeft = 0;
                                                celldcst2.BorderWidthTop = 0f;
                                                celldcst2.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst2);

                                            }

                                            #endregion

                                            #region Extradata Stmaintenance



                                            if (maintenancecost > 0 && STMaintenance == true)
                                            {


                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = 2;
                                                celldcst1.BorderWidthBottom = 0f;
                                                celldcst1.BorderWidthLeft = 0f;
                                                celldcst1.BorderWidthTop = 0f;
                                                celldcst1.BorderWidthRight = .5f;
                                                //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = .5f;
                                                celldcst2.Colspan = 1;
                                                celldcst2.BorderWidthLeft = 0.5f;
                                                celldcst2.BorderWidthTop = 0f;
                                                celldcst2.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst2);

                                            }

                                            #endregion

                                            #region Extradata Stextraone



                                            if (extraonecost > 0 && STExtraone == true)
                                            {


                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = 2;
                                                celldcst1.BorderWidthBottom = 0f;
                                                celldcst1.BorderWidthLeft = 0f;
                                                celldcst1.BorderWidthTop = 0f;
                                                celldcst1.BorderWidthRight = .5f;
                                                //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = .5f;
                                                celldcst2.Colspan = 1;
                                                celldcst2.BorderWidthLeft = 0.5f;
                                                celldcst2.BorderWidthTop = 0f;
                                                celldcst2.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst2);

                                            }


                                            #endregion

                                            #region Extradata Stextratwo



                                            if (extratwocost > 0 && STExtratwo == true)
                                            {


                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = 2;
                                                celldcst1.BorderWidthBottom = 0f;
                                                celldcst1.BorderWidthLeft = 0f;
                                                celldcst1.BorderWidthTop = 0f;
                                                celldcst1.BorderWidthRight = .5f;
                                                //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = .5f;
                                                celldcst2.Colspan = 1;
                                                celldcst2.BorderWidthLeft = 0.5f;
                                                celldcst2.BorderWidthTop = 0f;
                                                celldcst2.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst2);

                                            }
                                            #endregion

                                            #region Discountone

                                            if (discountone > 0 && STDiscountone == true)
                                            {


                                                PdfPCell celldMt1 = new PdfPCell(new Phrase(discountonetitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMt1.Colspan = 2;
                                                celldMt1.BorderWidthBottom = 0f;
                                                celldMt1.BorderWidthLeft = 0f;
                                                celldMt1.BorderWidthTop = 0f;
                                                celldMt1.BorderWidthRight = .5f;
                                                //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldMt1);

                                                PdfPCell celldMt3 = new PdfPCell(new Phrase(discountone.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMt3.BorderWidthBottom = 0f;
                                                celldMt3.Colspan = 1;
                                                celldMt3.BorderWidthLeft = 0;
                                                celldMt3.BorderWidthTop = 0f;
                                                celldMt3.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldMt3);
                                            }

                                            #endregion

                                            #region Discounttwo


                                            if (discounttwo > 0 && STDiscounttwo == true)
                                            {


                                                PdfPCell celldMt1 = new PdfPCell(new Phrase(discounttwotitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMt1.Colspan = 2;
                                                celldMt1.BorderWidthBottom = 0f;
                                                celldMt1.BorderWidthLeft = 0f;
                                                celldMt1.BorderWidthTop = 0f;
                                                celldMt1.BorderWidthRight = .5f;
                                                //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldMt1);

                                                PdfPCell celldMt3 = new PdfPCell(new Phrase(discounttwo.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMt3.BorderWidthBottom = 0f;
                                                celldMt3.Colspan = 1;
                                                celldMt3.BorderWidthLeft = 0.5f;
                                                celldMt3.BorderWidthTop = 0f;
                                                celldMt3.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldMt3);
                                            }
                                            #endregion

                                        }

                                        #endregion

                                        #region Code for Service Tax

                                        if (!bIncludeST)
                                        {

                                            decimal scpercent = 0;
                                            if (bST75)
                                            {
                                                scpercent = 3;
                                            }
                                            else
                                            {
                                                scpercent = Convert.ToDecimal(SCPersent);
                                            }


                                            if (scpercent > 0 && servicetax > 0)
                                            {


                                                PdfPCell celldd3 = new PdfPCell(new Phrase("Service Tax @ " + servicetaxprc.ToString("#") + "%", FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldd3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldd3.Colspan = 2;
                                                celldd3.BorderWidthBottom = 0;
                                                celldd3.BorderWidthLeft = 0f;
                                                celldd3.BorderWidthTop = 0;
                                                celldd3.BorderWidthRight = .5f;
                                                tempTable22.AddCell(celldd3);

                                                PdfPCell celldd4 = new PdfPCell(new Phrase(servicetax.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                                                 // celldd4.PaddingRight = -30;
                                                celldd4.BorderWidthBottom = 0f;
                                                celldd4.Colspan = 1;
                                                celldd4.BorderWidthLeft = 0.5f;
                                                celldd4.BorderWidthTop = 0;
                                                celldd4.BorderWidthRight = 1.5f;
                                                tempTable22.AddCell(celldd4);

                                            }

                                            if (sbcess > 0)
                                            {


                                                PdfPCell celldd2 = new PdfPCell(new Phrase("Swachh Bharat Cess @ " + Sbcessprc + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldd2.Colspan = 2;
                                                celldd2.BorderWidthBottom = 0f;
                                                celldd2.BorderWidthLeft = 0f;
                                                celldd2.BorderWidthTop = 0;
                                                celldd2.BorderWidthRight = .5f;
                                                tempTable22.AddCell(celldd2);


                                                PdfPCell celldd4 = new PdfPCell(new Phrase(sbcess.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldd4.BorderWidthBottom = 0f;
                                                celldd4.Colspan = 1;
                                                celldd4.BorderWidthLeft = 0.5f;
                                                celldd4.BorderWidthTop = 0;
                                                celldd4.BorderWidthRight = 1.5f;
                                                tempTable22.AddCell(celldd4);

                                            }

                                            if (kkcess > 0)
                                            {


                                                PdfPCell celldd2 = new PdfPCell(new Phrase("Krishi Kalyan Cess @ " + kkcessprc.ToString() + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldd2.Colspan = 2;
                                                celldd2.BorderWidthBottom = 0f;
                                                celldd2.BorderWidthLeft = 0f;
                                                celldd2.BorderWidthTop = 0;
                                                celldd2.BorderWidthRight = .5f;
                                                tempTable22.AddCell(celldd2);


                                                PdfPCell celldd4 = new PdfPCell(new Phrase(kkcess.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldd4.BorderWidthBottom = 0;
                                                celldd4.Colspan = 1;
                                                celldd4.BorderWidthLeft = 0.5f;
                                                celldd4.BorderWidthTop = 0;
                                                celldd4.BorderWidthRight = 1.5f;
                                                tempTable22.AddCell(celldd4);

                                            }

                                            #region for GST as on 17-6-2017

                                            if (CGST > 0)
                                            {


                                                PdfPCell CellCGST = new PdfPCell(new Phrase(CGSTAlias + " @ " + CGSTPrc + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellCGST.Colspan = 2;
                                                CellCGST.BorderWidthBottom = 0;
                                                CellCGST.BorderWidthLeft = 0;
                                                CellCGST.BorderWidthTop = 0;
                                                CellCGST.BorderWidthRight = .5f;
                                                // CellCGST.BorderColor = BaseColor.GRAY;
                                                tempTable22.AddCell(CellCGST);

                                                PdfPCell CellCGSTAmt = new PdfPCell(new Phrase(CGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                CellCGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellCGSTAmt.BorderWidthBottom = 0;
                                                CellCGST.Colspan = 1;

                                                CellCGSTAmt.BorderWidthLeft = 0.5f;
                                                CellCGSTAmt.BorderWidthTop = 0;
                                                CellCGSTAmt.BorderWidthRight = 1.5f;
                                                // CellCGSTAmt.BorderColor = BaseColor.GRAY;
                                                tempTable22.AddCell(CellCGSTAmt);

                                            }


                                            if (SGST > 0)
                                            {



                                                PdfPCell CellSGST = new PdfPCell(new Phrase(SGSTAlias + " @ " + SGSTPrc + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                CellSGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellSGST.Colspan = 2;
                                                CellSGST.BorderWidthBottom = 0;
                                                CellSGST.BorderWidthLeft = 0f;
                                                CellSGST.BorderWidthTop = 0;
                                                CellSGST.BorderWidthRight = 0.5f;
                                                CellSGST.BorderColor = BaseColor.GRAY;
                                                tempTable22.AddCell(CellSGST);

                                                PdfPCell CellSGSTAmt = new PdfPCell(new Phrase(SGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                CellSGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellSGSTAmt.BorderWidthBottom = 0;
                                                CellSGSTAmt.Colspan = 1;
                                                CellSGSTAmt.BorderWidthLeft = 0.5f;
                                                CellSGSTAmt.BorderWidthTop = 0;
                                                CellSGSTAmt.BorderWidthRight = 1.5f;
                                                CellSGSTAmt.BorderColor = BaseColor.GRAY;
                                                tempTable22.AddCell(CellSGSTAmt);

                                            }

                                            if (IGST > 0)
                                            {

                                                PdfPCell CellIGST = new PdfPCell(new Phrase(IGSTAlias + " @ " + IGSTPrc + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST.Colspan = 2;
                                                CellIGST.BorderWidthBottom = 0;
                                                CellIGST.BorderWidthLeft = 0;
                                                CellIGST.BorderWidthTop = 0;
                                                CellIGST.BorderWidthRight = 0.5f;
                                                CellIGST.BorderColor = BaseColor.GRAY;
                                                tempTable22.AddCell(CellIGST);

                                                PdfPCell CellIGSTAmt = new PdfPCell(new Phrase(IGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGSTAmt.BorderWidthBottom = 0;
                                                CellIGSTAmt.Colspan = 1;
                                                CellIGSTAmt.BorderWidthLeft = 0.5f;
                                                CellIGSTAmt.BorderWidthTop = 0;
                                                CellIGSTAmt.BorderWidthRight = 1.5f;
                                                CellIGSTAmt.BorderColor = BaseColor.GRAY;
                                                tempTable22.AddCell(CellIGSTAmt);

                                            }

                                            decimal RoundOffvalue = 0;
                                            RoundOffvalue = Convert.ToDecimal(Grandtotal) - Convert.ToDecimal(totalamount + SGST + CGST + IGST);


                                            if (RoundOffvalue != 0)
                                            {
                                                PdfPCell CeelRoundOffvalue = new PdfPCell(new Phrase("Round Off", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                CeelRoundOffvalue.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CeelRoundOffvalue.Colspan = 2;
                                                CeelRoundOffvalue.BorderWidthBottom = 0;
                                                CeelRoundOffvalue.BorderWidthLeft = 0;
                                                CeelRoundOffvalue.BorderWidthTop = 0;
                                                CeelRoundOffvalue.BorderWidthRight = 0.5f;
                                                CeelRoundOffvalue.PaddingTop = 5;
                                                tempTable22.AddCell(CeelRoundOffvalue);

                                                CeelRoundOffvalue = new PdfPCell(new Phrase(RoundOffvalue.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
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

                                                PdfPCell CellCess1 = new PdfPCell(new Phrase(Cess1Alias + " @ " + Cess1Prc + "%", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                CellCess1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellCess1.Colspan = 2;
                                                CellCess1.BorderWidthBottom = 0;
                                                CellCess1.BorderWidthLeft = 0f;
                                                CellCess1.BorderWidthTop = 0;
                                                CellCess1.BorderWidthRight = 0.5f;
                                                CellCess1.BorderColor = BaseColor.GRAY;
                                                tempTable22.AddCell(CellCess1);

                                                PdfPCell CellCess1Amt = new PdfPCell(new Phrase(Cess1.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                CellCess1Amt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellCess1Amt.BorderWidthBottom = 0;
                                                CellCess1Amt.Colspan = 1;
                                                CellCess1Amt.BorderWidthLeft = 0.5f;
                                                CellCess1Amt.BorderWidthTop = 0;
                                                CellCess1Amt.BorderWidthRight = 1.5f;
                                                CellCess1Amt.BorderColor = BaseColor.GRAY;
                                                tempTable22.AddCell(CellCess1Amt);

                                            }


                                            if (Cess2 > 0)
                                            {

                                                PdfPCell CellCess2 = new PdfPCell(new Phrase(Cess2Alias + " @ " + Cess2Prc + "%", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                CellCess2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellCess2.Colspan = 2;
                                                CellCess2.BorderWidthBottom = 0;
                                                CellCess2.BorderWidthLeft = 0f;
                                                CellCess2.BorderWidthTop = 0;
                                                CellCess2.BorderWidthRight = 0.5f;
                                                CellCess2.BorderColor = BaseColor.GRAY;
                                                tempTable22.AddCell(CellCess2);

                                                PdfPCell CellCess2Amt = new PdfPCell(new Phrase(Cess2.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                CellCess2Amt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellCess2Amt.BorderWidthBottom = 0;
                                                CellCess2Amt.Colspan = 1;
                                                CellCess2Amt.BorderWidthLeft = 0.5f;
                                                CellCess2Amt.BorderWidthTop = 0;
                                                CellCess2Amt.BorderWidthRight = 1.5f;
                                                CellCess2Amt.BorderColor = BaseColor.GRAY;
                                                tempTable22.AddCell(CellCess2Amt);

                                            }

                                            #endregion for GST 

                                            string CESSPresent = DtTaxes.Rows[0]["Cess"].ToString();

                                            if (CESSPresent.Trim().Length > 0 && cess > 0)
                                            {


                                                PdfPCell cellde3 = new PdfPCell(new Phrase("Education CESS @  " + CESSPresent + "%", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                cellde3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                cellde3.Colspan = 2;
                                                cellde3.BorderWidthBottom = 0;
                                                cellde3.BorderWidthLeft = 0f;
                                                cellde3.BorderWidthTop = 0;
                                                cellde3.BorderWidthRight = .5f;
                                                tempTable22.AddCell(cellde3);

                                                PdfPCell cellde4 = new PdfPCell(new Phrase(cess.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                cellde4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                                                 //cellde4.PaddingRight = -30;
                                                cellde4.BorderWidthBottom = 0;
                                                cellde4.Colspan = 1;
                                                cellde4.BorderWidthLeft = 0.5f;
                                                cellde4.BorderWidthTop = 0;
                                                cellde4.BorderWidthRight = 1.5f;
                                                tempTable22.AddCell(cellde4);

                                            }
                                            string SHECESSPresent = DtTaxes.Rows[0]["shecess"].ToString();
                                            double higheredu = 0;
                                            higheredu = servicetax * (double.Parse(SHECESSPresent) / 100);

                                            if (SHECESSPresent.Trim().Length > 0 && shecess > 0)
                                            {


                                                PdfPCell celldf3 = new PdfPCell(new Phrase("Higher Education CESS @  " + SHECESSPresent + "%", FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldf3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldf3.Colspan = 2;
                                                celldf3.BorderWidthBottom = 0;
                                                celldf3.BorderWidthLeft = 0f;
                                                celldf3.BorderWidthTop = 0;
                                                celldf3.BorderWidthRight = 0.5f;
                                                tempTable22.AddCell(celldf3);

                                                PdfPCell celldf4 = new PdfPCell(new Phrase(shecess.ToString(), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldf4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldf4.BorderWidthBottom = 0;
                                                celldf4.Colspan = 1;
                                                celldf4.BorderWidthLeft = .5f;
                                                celldf4.BorderWidthTop = 0;
                                                celldf4.BorderWidthRight = .5f;
                                                //celldf4.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldf4);
                                            }
                                        }


                                        #endregion


                                        #region New code for Service charge amount on Extradata if Service Tax on Service Charge as on 04/04/2014 by venkat

                                        if (Extradatacheck == true && bStaxonExtradataservicecharges == false)
                                        {
                                            #region Extradata SCmachinary

                                            if (SCMachinary == true && SCamtonMachinary > 0)
                                            {


                                                PdfPCell celldcst1 = new PdfPCell(new Phrase("Service Charge on " + machinarycosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = 2;
                                                celldcst1.BorderWidthBottom = 0f;
                                                celldcst1.BorderWidthLeft = 0f;
                                                celldcst1.BorderWidthTop = 0f;
                                                celldcst1.BorderWidthRight = .5f;
                                                //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(SCamtonMachinary.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0f;
                                                celldcst2.Colspan = 1;
                                                celldcst2.BorderWidthLeft = 0;
                                                celldcst2.BorderWidthTop = 0f;
                                                celldcst2.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;

                                                tempTable22.AddCell(celldcst2);

                                            }
                                            #endregion

                                            #region Extradata SCmaterial

                                            if (SCMaterial == true && SCamtonMaterial > 0)
                                            {



                                                PdfPCell celldcst1 = new PdfPCell(new Phrase("Service Charge on " + materialcosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = 2;
                                                celldcst1.BorderWidthBottom = 0f;
                                                celldcst1.BorderWidthLeft = 0f;
                                                celldcst1.BorderWidthTop = 0f;
                                                celldcst1.BorderWidthRight = .5f;
                                                //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(SCamtonMaterial.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0f;
                                                celldcst2.BorderWidthLeft = 0;
                                                celldcst2.Colspan = 1;
                                                celldcst2.BorderWidthTop = 0f;
                                                celldcst2.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst2);

                                            }
                                            #endregion

                                            #region Extradata SCmaintenance

                                            if (SCMaintenance == true && SCamtonMaintenance > 0)
                                            {


                                                PdfPCell celldcst1 = new PdfPCell(new Phrase("Service Charge on " + maintenancecosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = 2;
                                                celldcst1.BorderWidthBottom = .5f;
                                                celldcst1.BorderWidthLeft = .5f;
                                                celldcst1.BorderWidthTop = .5f;
                                                celldcst1.BorderWidthRight = .5f;
                                                //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(SCamtonMaintenance.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0f;
                                                celldcst2.Colspan = 1;
                                                celldcst2.BorderWidthLeft = .5f;
                                                celldcst2.BorderWidthTop = 0f;
                                                celldcst2.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst2);

                                            }
                                            #endregion

                                            #region Extradata SCmaintenance

                                            if (SCExtraone == true && SCamtonExtraone > 0)
                                            {


                                                PdfPCell celldcst1 = new PdfPCell(new Phrase("Service Charge on " + extraonecosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = 2;
                                                celldcst1.BorderWidthBottom = 0f;
                                                celldcst1.BorderWidthLeft = 0f;
                                                celldcst1.BorderWidthTop = 0f;
                                                celldcst1.BorderWidthRight = .5f;
                                                // celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(SCamtonExtraone.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.BorderWidthBottom = .5f;
                                                celldcst2.HorizontalAlignment = 2;
                                                celldcst2.Colspan = 1;
                                                celldcst2.BorderWidthLeft = 0;
                                                celldcst2.BorderWidthTop = 0f;
                                                celldcst2.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst2);

                                            }
                                            #endregion

                                            #region Extradata SCmaintenance

                                            if (SCExtratwo == true && SCamtonExtratwo > 0)
                                            {



                                                PdfPCell celldcst1 = new PdfPCell(new Phrase("Service Charge on " + extratwocosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = 2;
                                                celldcst1.BorderWidthBottom = 0f;
                                                celldcst1.BorderWidthLeft = 0f;
                                                celldcst1.BorderWidthTop = 0f;
                                                celldcst1.BorderWidthRight = .5f;
                                                // celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(SCamtonExtratwo.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0f;
                                                celldcst2.BorderWidthLeft = 0;
                                                celldcst2.BorderWidthTop = 0f;
                                                celldcst2.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldcst2);

                                            }
                                            #endregion

                                        }

                                        #endregion

                                        #region When Extradata check is True and STcheck is false


                                        if (Extradatacheck == true)
                                        {
                                            #region Machinary Cost

                                            if (machinarycost > 0 && STMachinary == false)
                                            {


                                                PdfPCell celldMeci1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldMeci1.Colspan = 2;
                                                celldMeci1.HorizontalAlignment = 2;
                                                celldMeci1.BorderWidthBottom = 0f;
                                                celldMeci1.BorderWidthLeft = 0f;
                                                celldMeci1.BorderWidthTop = .5f;
                                                celldMeci1.BorderWidthRight = .5f;
                                                // celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldMeci1);

                                                PdfPCell celldMeci3 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldMeci3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMeci3.BorderWidthBottom = 0f;
                                                celldMeci3.BorderWidthLeft = 0.5f;
                                                celldMeci3.BorderWidthTop = 0f;
                                                celldMeci3.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldMeci3);
                                            }

                                            #endregion

                                            #region Material Cost

                                            if (materialcost > 0 && STMaterial == false)
                                            {


                                                PdfPCell celldMt1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMt1.Colspan = 2;
                                                celldMt1.BorderWidthBottom = .5f;
                                                celldMt1.BorderWidthLeft = 0f;
                                                celldMt1.BorderWidthTop = .5f;
                                                celldMt1.BorderWidthRight = 0.5f;
                                                // celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldMt1);

                                                PdfPCell celldMt3 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMt3.BorderWidthBottom = .5f;
                                                celldMt3.BorderWidthLeft = 0.5f;
                                                celldMt3.BorderWidthTop = .5f;
                                                celldMt3.BorderWidthRight = 1.5f;
                                                //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldMt3);
                                            }

                                            #endregion

                                            #region Maintenance Cost

                                            if (maintenancecost > 0 && STMaintenance == false)
                                            {


                                                PdfPCell celldMt1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMt1.Colspan = 2;
                                                celldMt1.BorderWidthBottom = .5f;
                                                celldMt1.BorderWidthLeft = 0f;
                                                celldMt1.BorderWidthTop = .5f;
                                                celldMt1.BorderWidthRight = 0.5f;
                                                // celldMt1.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldMt1);

                                                PdfPCell celldMt3 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMt3.BorderWidthBottom = .5f;
                                                celldMt3.BorderWidthLeft = 0.5f;
                                                celldMt3.Colspan = 1;
                                                celldMt3.BorderWidthTop = .5f;
                                                celldMt3.BorderWidthRight = 1.5f;
                                                // celldMt3.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldMt3);
                                            }

                                            #endregion

                                            #region Extra amount two

                                            if (extraonecost > 0 && STExtraone == false)
                                            {



                                                PdfPCell celldMt1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMt1.BorderWidthBottom = 0f;
                                                celldMt1.BorderWidthLeft = 0;
                                                celldMt1.BorderWidthTop = 0;
                                                celldMt1.BorderWidthRight = .5f;
                                                //celldMt1.BorderColor = BaseColor.LIGHT_GRAY;
                                                celldMt1.Colspan = 2;
                                                tempTable22.AddCell(celldMt1);

                                                PdfPCell celldMt3 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMt3.BorderWidthBottom = 0f;
                                                celldMt3.BorderWidthLeft = 0.5f;
                                                celldMt3.Colspan = 1;
                                                celldMt3.BorderWidthTop = 0;
                                                celldMt3.BorderWidthRight = 1.5f;
                                                // celldMt3.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldMt3);
                                            }

                                            #endregion

                                            #region Extraamount two

                                            if (extratwocost > 0 && STExtratwo == false)
                                            {


                                                PdfPCell celldMt1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMt1.BorderWidthBottom = 0f;
                                                celldMt1.BorderWidthLeft = 0f;
                                                celldMt1.BorderWidthTop = 0;
                                                celldMt1.BorderWidthRight = .5f;
                                                //celldMt1.BorderColor = BaseColor.LIGHT_GRAY;
                                                celldMt1.Colspan = 2;
                                                tempTable22.AddCell(celldMt1);

                                                PdfPCell celldMt3 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMt3.BorderWidthBottom = 0f;
                                                celldMt3.BorderWidthLeft = 0.5f;
                                                celldMt3.Colspan = 1;
                                                celldMt3.BorderWidthTop = 0;
                                                celldMt3.BorderWidthRight = 1.5f;
                                                // celldMt3.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldMt3);
                                            }

                                            #endregion

                                            #region Discountone

                                            if (discountone > 0 && STDiscountone == false)
                                            {



                                                PdfPCell celldMt1 = new PdfPCell(new Phrase(discountonetitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMt1.BorderWidthBottom = 0f;
                                                celldMt1.BorderWidthLeft = 0f;
                                                celldMt1.BorderWidthTop = 0;
                                                celldMt1.BorderWidthRight = .5f;
                                                //celldMt1.BorderColor = BaseColor.LIGHT_GRAY;
                                                celldMt1.Colspan = 2;
                                                tempTable22.AddCell(celldMt1);

                                                PdfPCell celldMt3 = new PdfPCell(new Phrase(discountone.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMt3.BorderWidthBottom = 0f;
                                                celldMt3.BorderWidthLeft = 0.5f;
                                                celldMt3.Colspan = 1;
                                                celldMt3.BorderWidthTop = 0;
                                                celldMt3.BorderWidthRight = 1.5f;
                                                // celldMt3.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldMt3);
                                            }

                                            #endregion

                                            #region Discounttwo


                                            if (discounttwo > 0 && STDiscounttwo == false)
                                            {



                                                PdfPCell celldMt1 = new PdfPCell(new Phrase(discounttwotitle, FontFactory.GetFont(FontStyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                                                celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMt1.BorderWidthBottom = 0f;
                                                celldMt1.BorderWidthLeft = .5f;
                                                celldMt1.BorderWidthTop = 0;
                                                celldMt1.BorderWidthRight = .5f;
                                                //celldMt1.BorderColor = BaseColor.LIGHT_GRAY;
                                                celldMt1.Colspan = 2;
                                                tempTable22.AddCell(celldMt1);

                                                PdfPCell celldMt3 = new PdfPCell(new Phrase(discounttwo.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                                                celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldMt3.BorderWidthBottom = 0f;
                                                celldMt3.BorderWidthLeft = 0.5f;
                                                celldMt3.Colspan = 1;
                                                celldMt3.BorderWidthTop = 0;
                                                celldMt3.BorderWidthRight = .5f;
                                                // celldMt3.BorderColor = BaseColor.LIGHT_GRAY;
                                                tempTable22.AddCell(celldMt3);
                                            }
                                            #endregion

                                        }
                                        PdfPCell Chids = new PdfPCell(tempTable22);
                                        Chids.Border = 0;
                                        Chids.Colspan = 3;
                                        Chids.HorizontalAlignment = 0;
                                        tabled.AddCell(Chids);

                                        document.Add(tabled);

                                        #endregion

                                        #region footer
                                        PdfPTable addrssf = new PdfPTable(colCount);
                                        addrssf.TotalWidth = 560f;
                                        addrssf.LockedWidth = true;
                                        float[] addr = new float[] { 1.2f, 6.2f, 2f, 2.2f, 2.3f, 2.4f };
                                        addrssf.SetWidths(addr);


                                        PdfPTable cellt = new PdfPTable(3);
                                        cellt.TotalWidth = 323f;
                                        cellt.LockedWidth = true;
                                        float[] widthcell = new float[] { 1f, 6.1f, 1.2f };//1.2f, 6.2f, 2f, 2.3f
                                        cellt.SetWidths(widthcell);
                                        #region

                                        cell = new PdfPCell(new Phrase(" Amount In Words: ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 0;
                                        cell.BorderWidthBottom = 0;
                                        cell.BorderWidthTop = 0.5f;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.BorderWidthLeft = 1.5f;
                                        cell.Colspan = 3;
                                        cellt.AddCell(cell);

                                        string Amountinwords = NumberToEnglish.Instance.changeNumericToWords(Grandtotal.ToString());

                                        cell = new PdfPCell(new Phrase("  " + Amountinwords.Trim() + " Only ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
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
                                        float[] Dfv = new float[] { 1.8f, 1f, 1.8f }; ;//2.9f, 1.83f
                                        celltf.SetWidths(Dfv);

                                        #region
                                        cell = new PdfPCell(new Phrase("Grand  Total", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 2;
                                        cell.BorderWidthBottom = 0;
                                        cell.BorderWidthTop = 0.5f;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.BorderWidthLeft = 0.5f;
                                        cell.Colspan = 2;
                                        celltf.AddCell(cell);
                                        cell = new PdfPCell(new Phrase(Grandtotal.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 2;
                                        cell.BorderWidthBottom = 0;
                                        cell.BorderWidthTop = 0.5f;
                                        cell.BorderWidthRight = 1.5f;
                                        cell.BorderWidthLeft = 0.5f;
                                        cell.Colspan = 1;
                                        celltf.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 2;
                                        cell.BorderWidthBottom = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.BorderWidthLeft = 0.5f;
                                        cell.Colspan = 2;
                                        // cell.MinimumHeight = 30;
                                        celltf.AddCell(cell);
                                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
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

                                        #endregion

                                        document.NewPage();
                                    }
                                }
                            }
                        }
                        document.Close();

                    }

                }
                catch (Exception ex)
                {

                }

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Invoice.pdf");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }
            else
            {

                return;
            }
        }

        public string GetMonthOfYear()
        {
            string MonthYear = "";

            var Month = "";

            string date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();
            Month = month + Year.Substring(2, 2);

            if (Month.ToString().Length == 4)
            {

                MonthYear = "20" + Month.ToString().Substring(2, 2);

            }
            if (Month.ToString().Length == 3)
            {

                MonthYear = "20" + Month.ToString().Substring(1, 2);

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
            DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            monthname = mfi.GetMonthName(date.Month).ToString();
            //payMonth = GetMonth(monthname);
            return monthname;
        }
    }
}