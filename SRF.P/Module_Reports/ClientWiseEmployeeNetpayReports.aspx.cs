using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using System.Globalization;
using System.Collections;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using SRF.P.DAL;


namespace SRF.P.Module_Reports
{
    public partial class ClientWiseEmployeeNetpayReports : System.Web.UI.Page
    {
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";

        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();

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
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();

        }

        protected void Bindata(string Sqlqry)
        {
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                GVListEmployees.DataSource = dt;
                GVListEmployees.DataBind();
            }
            else
            {
                LblResult.Text = "There Is No Salary Details For The Selected client";
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();
            }
        }

        protected void ClearData()
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
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
            string month = "";
            string Year = "";
            if (txtmonth.Text.Trim().Length > 0)
            {
                string date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                month = DateTime.Parse(date).Month.ToString();
                Year = DateTime.Parse(date).Year.ToString();

            }



            #region Begin Variable Declaration as on [04-07-2014]
            var Month = "";
            var SPName = "";
            var clientids = "";
            DataTable DtClientListBasedonSelectedMonth = null;
            Hashtable HtClientListBasedonSelectedMonth = new Hashtable();
            #endregion End  Variable Declaration as on [04-07-2014]

            #region Begin Assign Values To Vriables as on [04-07-2014]
            Month = month + Year.Substring(2, 2);

            SPName = "GEtAllclientsListForPaysheetBulkprints";
            #endregion  End Assing Values to The Variables as on [04-07-2014]

            #region Begin  Pass Values To the Hash table as on [04-07-2014]
            HtClientListBasedonSelectedMonth.Add("@month", Month);
            //HtClientListBasedonSelectedMonth.Add("@clientids", clientids);
            #endregion end Pass Values To the Hash table as on [04-07-2014]

            #region  Begin Call Sp on [04-07-2014]
            DtClientListBasedonSelectedMonth = config.ExecuteAdaptorAsyncWithParams(SPName, HtClientListBasedonSelectedMonth).Result;
            GVListEmployees.DataSource = DtClientListBasedonSelectedMonth;
            GVListEmployees.DataBind();
            #endregion  end Call Sp on [04-07-2014]
        }

        string Text = "";

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("NetpayReports.xls", this.GVNetPayDetails);
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            var list = new List<string>();

            if (GVListEmployees.Rows.Count > 0)
            {

                for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                {
                    CheckBox chkclientid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                    if (chkclientid.Checked == true)
                    {
                        Label lblclientid = GVListEmployees.Rows[i].FindControl("lblclientid") as Label;
                        Label lblclientname = GVListEmployees.Rows[i].FindControl("lblclientname") as Label;

                        if (chkclientid.Checked == true)
                        {
                            // list.Add("'" + lblclientid.Text + "'");

                            list.Add(lblclientid.Text);
                        }
                    }
                }
            }

            string Clientids = string.Join(",", list.ToArray());



            DataTable dtClientList = new DataTable();
            dtClientList.Columns.Add("Clientid");
            if (list.Count != 0)
            {
                foreach (string s in list)
                {
                    DataRow row = dtClientList.NewRow();
                    row["Clientid"] = s;
                    dtClientList.Rows.Add(row);
                }
            }


            string date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            var Month = month + Year.Substring(2, 2);

            string spname = "";
            DataTable dtIOM = null;
            Hashtable HashtableIOM = new Hashtable();


            spname = "ClientWiseEmployeeNetpayReports";
            HashtableIOM.Add("@ClientIDList", dtClientList);
            HashtableIOM.Add("@month", Month);

            dtIOM = config.ExecuteAdaptorAsyncWithParams(spname, HashtableIOM).Result;



            if (dtIOM.Rows.Count > 0)
            {
                GVNetPayDetails.DataSource = dtIOM;
                GVNetPayDetails.DataBind();
                GVUtil.Export("NetpayReports.xls", this.GVNetPayDetails);

            }
            else
            {
                GVNetPayDetails.DataSource = null;
                GVNetPayDetails.DataBind();

            }

        }

        protected void lbtn_Export_PDF_Click(object sender, EventArgs e)
        {
            var list = new List<string>();

            if (GVListEmployees.Rows.Count > 0)
            {

                for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                {
                    CheckBox chkclientid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                    if (chkclientid.Checked == true)
                    {
                        Label lblclientid = GVListEmployees.Rows[i].FindControl("lblclientid") as Label;
                        Label lblclientname = GVListEmployees.Rows[i].FindControl("lblclientname") as Label;

                        if (chkclientid.Checked == true)
                        {
                            list.Add("'" + lblclientid.Text + "'");
                        }

                    }

                }


            }

            string Clientids = string.Join(",", list.ToArray());

            string Fontstyle = "verdana";

            string date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            var Month = month + Year.Substring(2, 2);


            string query = "select ROW_NUMBER() over( partition by eps.clientid order by eps.clientid) as Sno,clientname,eps.clientid,sum(EPS.NoOfDuties) as NoOfDuties,sum(EPS.Basic) as Basic,sum(EPS.DA) as DA,sum(EPS.HRA) as HRA,sum(EPS.CCA) as CCA,sum(EPS.Conveyance) as Conveyance,sum(EPS.WashAllowance) as WashAllowance," +
            "sum( EPS.OtherAllowance) as OtherAllowance,sum(EPS.LeaveEncashAmt) as Leavewages,sum(EPS.Gratuity) as Gratuity,sum(EPS.Bonus) as Bonus,sum(EPS.Nfhs) as nfhs,sum(eps.WO) as wo,sum(eps.WOAmt) as woamt,sum(eps.Npotsamt) as Npotsamt,sum(eps.Nhsamt) as Nhsamt,sum(eps.Npots) as Npots,sum(eps.nhs) as nhs," +
            " sum(EPS.RC) as RC,sum(EPS.cs) as CS,sum(EPS.Gross) as gross,sum(eps.Npots) as npots,sum(eps.NHS) as nhs,sum((eps.gross+eps.otamt)) as subtotal,   " +
             "sum(EPS.Incentivs) as Incentivs ,sum(EPS.Pfonduties) as Pfonduties,sum(EPS.Esionduties) as Esionduties,sum(EPS.PFONOT) as PFONOT,sum(EPS.ESIONOT) as ESIONOT,sum(EPS.PFONOT+EPS.Pfonduties) as PFTotal, " +
            " sum((EPS.Esionduties+EPS.ESIONOT)) ESITotal,sum(EPS.ProfTax) as ProfTax,sum(EPS.owf) as owf,sum(EPS.SalAdvDed) as SalAdvDed,sum(EPS.UniformDed) as UniformDed,sum(EPS.OtherDed) as OtherDed,sum(EPS.CanteenAdv) as CanteenAdv , " +
            " sum(EPS.Penalty) as Penalty,sum(EPS.Gross) as gross,sum(eps.SecurityDepDed) as SecDepDedn,sum(eps.penalty) as roomrentded,sum(eps.GeneralDed) as GenDedn,  " +
               " sum((EPS.Pf+EPS.Esi+EPS.ProfTax+EPS.SalAdvDed+EPS.UniformDed+EPS.OtherDed+EPS.CanteenAdv+EPS.Penalty+ISNULL(eps.roomrentded,0)+ISNULL(eps.SecurityDepDed,0)+isnull(eps.roomrentded,0)+ISNULL(GeneralDed,0)+ISNULL(owf,0))) Totaldeduct, " +
               " sum(((EPS.Gross+EPS.OTAmt)-(EPS.Pf+EPS.Esi+EPS.ProfTax+EPS.SalAdvDed+EPS.UniformDed+isnull(eps.roomrentded,0) +ISNULL(eps.SecurityDepDed,0)+ISNULL(GeneralDed,0)+ " +
               " EPS.OtherDed+EPS.CanteenAdv+EPS.Penalty+ISNULL(owf,0)))) NetAmount,sum(EPS.ots) as Duties,  " +
                "sum(EPS.OTAmt) as Amount " +
            " from  EmpPaySheet EPS  " +
            " join Designations D on D.DesignId=EpS.Desgn inner join clients c on c.clientid =eps.clientid where EPS.Month='" + month + Year.Substring(2, 2) + "'  and EPS.ClientId in (" + Clientids + ") and " +
             "(EPS.NoOfDuties+EPS.ots)!=0 and (EPS.NoOfDuties+EPS.ots)>0 group by eps.clientid,clientname order by eps.clientid ";

            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;


            string clientid = "";
            string clientname = "";
            MemoryStream ms = new MemoryStream();
            if (dt.Rows.Count > 0)
            {

                #region Variables for table cells counting

                int dts = 0;
                int owf = 0;
                float owf1 = 0;
                int srate = 0;
                int basic = 0;
                int da = 0;
                int hra = 0;
                int cca = 0;
                int conveyance = 0;
                int washallowance = 0;
                int otherallowance = 0;
                int leavewages = 0;
                int gratuity = 0;
                int bonus = 0;
                int nfhs = 0;
                int rc = 0;
                int cs = 0;
                int gross = 0;
                int incentivs = 0;
                int pfonduties = 0;
                int esionduties = 0;
                int proftax = 0;
                int salAdvDed = 0;
                int uniformDed = 0;
                int otherDed = 0;
                int canteenAdv = 0;
                int roomrent = 0;
                int totalDeductions = 0;
                int netpay = 0;

                int ots = 0;
                int wo = 0;
                int npots = 0;
                int nhs = 0;
                int woamt = 0;
                int npotsamt = 0;
                int nhsamt = 0;
                int otamt = 0;
                int pfonot = 0;
                int esionot = 0;
                int Pf = 0;
                int Esi = 0;
                int GenDedn = 0;
                int SecDepDedn = 0;
                int subtotal = 0;

                float dts1 = 0;
                float srate1 = 0;
                float basic1 = 0;
                float da1 = 0;
                float hra1 = 0;
                float cca1 = 0;
                float conveyance1 = 0;
                float washallowance1 = 0;
                float otherallowance1 = 0;
                float leavewages1 = 0;
                float gratuity1 = 0;
                float bonus1 = 0;
                float nfhs1 = 0;
                float rc1 = 0;
                float cs1 = 0;
                float gross1 = 0;
                float incentivs1 = 0;
                float pfonduties1 = 0;
                float esionduties1 = 0;
                float proftax1 = 0;
                float salAdvDed1 = 0;
                float uniformDed1 = 0;
                float otherDed1 = 0;
                float canteenAdv1 = 0;
                float roomrent1 = 0;
                float totalDeductions1 = 0;
                float netpay1 = 0;
                float subtotal1 = 0;

                float ots1 = 0;
                float wo1 = 0;
                float npots1 = 0;
                float nhs1 = 0;
                float woamt1 = 0;
                float npotsamt1 = 0;
                float nhsamt1 = 0;
                float otamt1 = 0;
                float pfonot1 = 0;
                float esionot1 = 0;
                float Pf1 = 0;
                float Esi1 = 0;
                float GenDedn1 = 0;
                float SecDepDedn1 = 0;


                #endregion


                #region Data for counting tablecells

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dts1 = float.Parse(dt.Rows[i]["NoOfDuties"].ToString());
                    if (dts1 != 0)
                    {
                        dts1 += 1;
                        if (dts1 > 0)
                        {
                            dts = 1;
                        }
                    }

                    owf1 = float.Parse(dt.Rows[i]["owf"].ToString());
                    if (owf1 != 0)
                    {
                        owf1 += 1;
                        if (owf1 > 0)
                        {
                            owf = 1;
                        }
                    }

                    //srate1 = float.Parse(dt.Rows[i]["SalRate"].ToString());
                    //if (srate1 != 0)
                    //{
                    //    srate1 += 1;
                    //    if (srate1 > 0)
                    //    {
                    //        srate = 1;
                    //    }
                    //}

                    basic1 = float.Parse(dt.Rows[i]["basic"].ToString());
                    if (basic1 != 0)
                    {
                        basic1 += 1;
                        if (basic1 > 0)
                        {
                            basic = 1;
                        }
                    }
                    da1 = float.Parse(dt.Rows[i]["da"].ToString());
                    if (da1 != 0)
                    {
                        da1 += 1;
                        if (da1 > 0)
                        {
                            da = 1;
                        }
                    }
                    hra1 = float.Parse(dt.Rows[i]["hra"].ToString());
                    if (hra1 != 0)
                    {
                        hra1 += 1;
                        if (hra1 > 0)
                        {
                            hra = 1;
                        }
                    }
                    cca1 = float.Parse(dt.Rows[i]["cca"].ToString());
                    if (cca1 != 0)
                    {
                        cca1 += 1;
                        if (cca1 > 0)
                        {
                            cca = 1;
                        }
                    }
                    conveyance1 = float.Parse(dt.Rows[i]["Conveyance"].ToString());
                    if (conveyance1 != 0)
                    {
                        conveyance1 += 1;
                        if (conveyance1 > 0)
                        {
                            conveyance = 1;
                        }
                    }

                    washallowance1 = float.Parse(dt.Rows[i]["washallowance"].ToString());
                    if (washallowance1 != 0)
                    {
                        washallowance1 += 1;
                        if (washallowance1 > 0)
                        {
                            washallowance = 1;
                        }
                    }
                    otherallowance1 = float.Parse(dt.Rows[i]["otherallowance"].ToString());
                    if (otherallowance1 != 0)
                    {
                        otherallowance1 += 1;
                        if (otherallowance1 > 0)
                        {
                            otherallowance = 1;
                        }
                    }
                    leavewages1 = float.Parse(dt.Rows[i]["Leavewages"].ToString());
                    if (leavewages1 != 0)
                    {
                        leavewages1 += 1;
                        if (leavewages1 > 0)
                        {
                            leavewages = 1;
                        }
                    }
                    gratuity1 = float.Parse(dt.Rows[i]["gratuity"].ToString());
                    if (gratuity1 != 0)
                    {
                        gratuity1 += 1;
                        if (gratuity1 > 0)
                        {
                            gratuity = 1;
                        }
                    }
                    bonus1 = float.Parse(dt.Rows[i]["bonus"].ToString());
                    if (bonus1 != 0)
                    {
                        bonus1 += 1;
                        if (bonus1 > 0)
                        {
                            bonus = 1;
                        }
                    }

                    nfhs1 = float.Parse(dt.Rows[i]["nfhs"].ToString());
                    if (nfhs1 != 0)
                    {
                        nfhs1 += 1;
                        if (nfhs1 > 0)
                        {
                            nfhs = 1;
                        }
                    }
                    rc1 = float.Parse(dt.Rows[i]["rc"].ToString());
                    if (rc1 != 0)
                    {
                        rc1 += 1;
                        if (rc1 > 0)
                        {
                            rc = 1;
                        }
                    }

                    cs1 = float.Parse(dt.Rows[i]["cs"].ToString());
                    if (cs1 != 0)
                    {
                        cs1 += 1;
                        if (cs1 > 0)
                        {
                            cs = 1;
                        }
                    }
                    gross1 = float.Parse(dt.Rows[i]["gross"].ToString());
                    if (gross1 != 0)
                    {
                        gross1 += 1;
                        if (gross1 > 0)
                        {
                            gross = 1;
                        }
                    }

                    incentivs1 = float.Parse(dt.Rows[i]["incentivs"].ToString());
                    if (incentivs1 != 0)
                    {
                        incentivs1 += 1;
                        if (incentivs1 > 0)
                        {
                            incentivs = 1;
                        }
                    }


                    woamt1 = float.Parse(dt.Rows[i]["WOAmt"].ToString());
                    if (woamt1 != 0)
                    {
                        woamt1 += 1;
                        if (woamt1 > 0)
                        {
                            woamt = 1;
                        }
                    }

                    npotsamt1 = float.Parse(dt.Rows[i]["Npotsamt"].ToString());
                    if (npotsamt1 != 0)
                    {
                        npotsamt1 += 1;
                        if (npotsamt1 > 0)
                        {
                            npotsamt = 1;
                        }
                    }
                    nhsamt1 = float.Parse(dt.Rows[i]["Nhsamt"].ToString());
                    if (nhsamt1 != 0)
                    {
                        nhsamt1 += 1;
                        if (nhsamt1 > 0)
                        {
                            nhsamt = 1;
                        }
                    }



                    //CHanged to zero so as to get PF and ESI in PFTotal and ESITotal as given below //Check PFTotal and ESITotal
                    pfonduties1 = float.Parse(dt.Rows[i]["PFonDuties"].ToString());
                    if (pfonduties1 != 0)
                    {
                        pfonduties1 += 1;
                        if (pfonduties1 > 0)
                        {
                            pfonduties = 0;
                        }
                    }
                    esionduties1 = float.Parse(dt.Rows[i]["ESIonduties"].ToString());
                    if (esionduties1 != 0)
                    {
                        esionduties1 += 1;
                        if (esionduties1 > 0)
                        {
                            esionduties = 0;
                        }
                    }
                    proftax1 = float.Parse(dt.Rows[i]["proftax"].ToString());
                    if (proftax1 != 0)
                    {
                        proftax1 += 1;
                        if (proftax1 > 0)
                        {
                            proftax = 1;
                        }
                    }
                    salAdvDed1 = float.Parse(dt.Rows[i]["salAdvDed"].ToString());
                    if (salAdvDed1 != 0)
                    {
                        salAdvDed1 += 1;
                        if (salAdvDed1 > 0)
                        {
                            salAdvDed = 1;
                        }
                    }
                    uniformDed1 = float.Parse(dt.Rows[i]["uniformDed"].ToString());
                    if (uniformDed1 != 0)
                    {
                        uniformDed1 += 1;
                        if (uniformDed1 > 0)
                        {
                            uniformDed = 1;
                        }
                    }

                    GenDedn1 = float.Parse(dt.Rows[i]["GenDedn"].ToString());
                    if (GenDedn1 != 0)
                    {
                        GenDedn1 += 1;
                        if (GenDedn1 > 0)
                        {
                            GenDedn = 1;
                        }
                    }

                    SecDepDedn1 = float.Parse(dt.Rows[i]["SecDepDedn"].ToString());
                    if (SecDepDedn1 != 0)
                    {
                        SecDepDedn1 += 1;
                        if (SecDepDedn1 > 0)
                        {
                            SecDepDedn = 1;
                        }
                    }

                    otherDed1 = float.Parse(dt.Rows[i]["otherDed"].ToString());
                    if (otherDed1 != 0)
                    {
                        otherDed1 += 1;
                        if (otherDed1 > 0)
                        {
                            otherDed = 1;
                        }
                    }
                    canteenAdv1 = float.Parse(dt.Rows[i]["canteenAdv"].ToString());
                    if (canteenAdv1 != 0)
                    {
                        canteenAdv1 += 1;
                        if (canteenAdv1 > 0)
                        {
                            canteenAdv = 1;
                        }
                    }

                    roomrent1 = float.Parse(dt.Rows[i]["roomrentded"].ToString());
                    if (roomrent1 != 0)
                    {
                        roomrent1 += 1;
                        if (roomrent1 > 0)
                        {
                            roomrent = 1;
                        }
                    }
                    totalDeductions1 = float.Parse(dt.Rows[i]["Totaldeduct"].ToString());
                    if (totalDeductions1 != 0)
                    {
                        totalDeductions1 += 1;
                        if (totalDeductions1 > 0)
                        {
                            totalDeductions = 1;
                        }
                    }
                    netpay1 = float.Parse(dt.Rows[i]["NetAmount"].ToString());
                    if (netpay1 != 0)
                    {
                        netpay1 += 1;
                        if (netpay1 > 0)
                        {
                            netpay = 1;
                        }
                    }

                    ots1 = float.Parse(dt.Rows[i]["Duties"].ToString());
                    if (ots1 != 0)
                    {
                        ots1 += 1;
                        if (ots1 > 0)
                        {
                            ots = 1;
                        }
                    }

                    wo1 = float.Parse(dt.Rows[i]["WO"].ToString());
                    if (wo1 != 0)
                    {
                        wo1 += 1;
                        if (wo1 > 0)
                        {
                            wo = 1;
                        }
                    }

                    npots1 = float.Parse(dt.Rows[i]["Npots"].ToString());
                    if (npots1 != 0)
                    {
                        npots1 += 1;
                        if (npots1 > 0)
                        {
                            npots = 1;
                        }
                    }

                    nhs1 = float.Parse(dt.Rows[i]["NHS"].ToString());
                    if (nhs1 != 0)
                    {
                        nhs1 += 1;
                        if (nhs1 > 0)
                        {
                            nhs = 1;
                        }
                    }

                    otamt1 = float.Parse(dt.Rows[i]["Amount"].ToString());
                    if (otamt1 != 0)
                    {
                        otamt1 += 1;
                        if (otamt1 > 0)
                        {
                            otamt = 1;
                        }
                    }

                    pfonot1 = float.Parse(dt.Rows[i]["PFONOT"].ToString());
                    if (pfonot1 != 0)
                    {
                        pfonot1 += 1;
                        if (pfonot1 > 0)
                        {
                            pfonot = 0;
                        }
                    }

                    esionot1 = float.Parse(dt.Rows[i]["ESIONOT"].ToString());
                    if (esionot1 != 0)
                    {
                        esionot1 += 1;
                        if (esionot1 > 0)
                        {
                            esionot = 0;
                        }
                    }

                    Pf1 = float.Parse(dt.Rows[i]["PFTotal"].ToString());
                    if (Pf1 != 0)
                    {
                        Pf1 += 1;
                        if (Pf1 > 0)
                        {
                            Pf = 1;
                        }
                    }
                    Esi1 = float.Parse(dt.Rows[i]["ESITotal"].ToString());
                    if (Esi1 != 0)
                    {
                        Esi1 += 1;
                        if (Esi1 > 0)
                        {
                            Esi = 1;
                        }
                    }

                    subtotal1 = float.Parse(dt.Rows[i]["subtotal"].ToString());
                    if (subtotal1 != 0)
                    {
                        subtotal1 += 1;
                        if (subtotal1 > 0)
                        {
                            subtotal = 1;
                        }
                    }


                }

                #endregion

                int sno = 1;


                int tableCells = dts + basic + da + hra + cca + conveyance + washallowance + otherallowance + leavewages +
                               gratuity + bonus + nfhs + rc + cs + gross + incentivs + pfonduties + esionduties + proftax + owf +
                               salAdvDed + uniformDed + otherDed + canteenAdv + roomrent + totalDeductions + netpay + sno + subtotal +
                                ots + wo + npots + nhs + otamt + pfonot + esionot + Pf + Esi + GenDedn + SecDepDedn + woamt + npotsamt + nhsamt;

                DataTable dtclientid = dt.DefaultView.ToTable(true, "clientid", "clientname");


                Document document = new Document(PageSize.LEGAL.Rotate());
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                PageEventHelper pageEvent = new PageEventHelper();
                pageEvent.list = Clientids;
                pageEvent.CmpIDPrefix = CmpIDPrefix;
                pageEvent.txtmonth = txtmonth.Text;
                pageEvent.document = document;
                //pageEvent.dt = dt;
                pageEvent.month = month;
                pageEvent.Year = Year;
                pageEvent.Clientids = Clientids;


                writer.PageEvent = pageEvent;


                float forConvert;
                string strQry = "Select * from CompanyInfo   where   ClientidPrefix='" + CmpIDPrefix + "'";
                DataTable compInfo = config.ExecuteReaderWithQueryAsync(strQry).Result;
                string companyName1 = "Your Company Name";
                string companyAddress = "Your Company Address";
                if (compInfo.Rows.Count > 0)
                {
                    companyName1 = compInfo.Rows[0]["CompanyName"].ToString();
                    companyAddress = compInfo.Rows[0]["Address"].ToString();
                }





                #region variables for total

                float totalwoamt = 0;
                float totalnpotsamt = 0;

                float totalowf = 0;
                float Grandtotalowf = 0;

                float totalnhsamt = 0;
                float totalDuties = 0;
                float totalsrate = 0;
                float totalBasic = 0;
                float totalDA = 0;
                float totalHRA = 0;
                float totalCCA = 0;
                float totalConveyance = 0;
                float totalWA = 0;
                float totalOA = 0;
                float totalLw = 0;
                float totalGratuity = 0;
                float totalbonus = 0;
                float totalNfhs = 0;
                float totalRc = 0;
                float totalCs = 0;
                float totalGross = 0;
                float totalIncentivs = 0;
                float totalPFondts = 0;
                float totalESIondts = 0;
                float totalProfTax = 0;
                float totalSalAdv = 0;
                float totalUniforDed = 0;
                float totalOtherdedn = 0;
                float totalCanteenAdv = 0;
                float totalPenalty = 0;
                float totalDed = 0;
                float totalNetpay = 0;

                float totalots = 0;
                float totalwo = 0;
                float totalnpots = 0;
                float totalnhs = 0;
                float totalotamt = 0;
                float totalpfonots = 0;
                float totalesionots = 0;
                float totalpf = 0;
                float totalesi = 0;
                float totalGenDedn = 0;
                float totalSecDepDedn = 0;

                #endregion

                #region variables for  Grand  total

                float Grandtotalwoamt = 0;
                float Grandtotalnpotsamt = 0;
                float Grandtotalnhsamt = 0;
                float GrandtotalDuties = 0;
                float GrandtotalSrate = 0;
                float GrandtotalBasic = 0;
                float GrandtotalDA = 0;
                float GrandtotalHRA = 0;
                float GrandtotalCCA = 0;
                float GrandtotalConveyance = 0;
                float GrandtotalWA = 0;
                float GrandtotalOA = 0;
                float GrandtotalLw = 0;
                float GrandtotalGratuity = 0;
                float Grandtotalbonus = 0;
                float GrandtotalNfhs = 0;
                float GrandtotalRc = 0;
                float GrandtotalCs = 0;
                float GrandtotalGross = 0;
                float GrandtotalIncentivs = 0;
                float GrandtotalPFondts = 0;
                float GrandtotalESIondts = 0;
                float GrandtotalProfTax = 0;
                float GrandtotalSalAdv = 0;
                float GrandtotalUniforDed = 0;
                float GrandtotalOtherdedn = 0;
                float GrandtotalCanteenAdv = 0;
                float GrandtotalPenalty = 0;
                float GrandtotalDed = 0;
                float GrandtotalNetpay = 0;

                float Grandtotalots = 0;
                float Grandtotalwo = 0;
                float Grandtotalnpots = 0;
                float Grandtotalnhs = 0;
                float Grandtotalotamt = 0;
                float Grandtotalpfonots = 0;
                float Grandtotalesionots = 0;
                float Grandtotalpf = 0;
                float Grandtotalesi = 0;
                float GrandtotalGenDedn = 0;
                float GrandtotalSecDepDedn = 0;

                #endregion

                string selectclientaddress = "select * from clients where clientid  in (" + Clientids + ")";
                DataTable dtclientaddress = config.ExecuteReaderWithQueryAsync(selectclientaddress).Result;

                bool nextpagehasPages = false;

                nextpagehasPages = true;
                int targetpagerecors = 10;
                int secondpagerecords = targetpagerecors;
                uint FONT_SIZE = 8;

                pageEvent.clientid = dtclientid.Rows[0]["clientid"].ToString(); ;
                pageEvent.clientname = dtclientid.Rows[0]["clientname"].ToString(); ;
                document.Open();
                document.AddTitle("FaMS");
                document.AddAuthor("WebWonders");
                document.AddSubject("Wage Sheet");
                document.AddKeywords("Keyword1, keyword2, …");//





                for (int k = 0; k < dtclientid.Rows.Count; k++)
                {


                    pageEvent.MyRow = dtclientid.Rows[k];

                    clientid = dtclientid.Rows[k]["clientid"].ToString();
                    clientname = dtclientid.Rows[k]["clientname"].ToString();

                    if (k != 0)
                    {
                        pageEvent.clientid = clientid;
                        pageEvent.clientname = clientname;
                        document.NewPage();


                    }


                    #region Table Data

                    PdfPTable Secondtable = new PdfPTable(tableCells);
                    if (tableCells > 20)
                    {
                        Secondtable.TotalWidth = 1000f;
                    }
                    else
                    {
                        Secondtable.TotalWidth = 850f;
                    }
                    Secondtable.LockedWidth = true;
                    float[] SecondWidth = new float[] { };

                    #region Table Cells
                    if (tableCells == 37)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 36)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }

                    if (tableCells == 35)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }

                    if (tableCells == 34)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 33)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 32)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 31)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 30)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 29)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 28)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 27)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 26)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 25)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 24)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 23)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 22)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 21)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 20)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 19)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 18)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 17)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 16)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 15)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 14)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 13)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 12)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 11)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 10)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 9)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 2.5f };
                    }
                    if (tableCells == 8)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 2f };
                    }

                    if (tableCells == 7)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2.5f };
                    }

                    if (tableCells == 6)
                    {
                        SecondWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 2.5f };
                    }
                    #endregion

                    Secondtable.SetWidths(SecondWidth);


                    for (int nextpagei = 0; nextpagei < dt.Rows.Count; ++nextpagei)
                    {

                        int i = nextpagei;
                        #region data from db
                        if (dt.Rows[nextpagei]["clientid"].ToString() == dtclientid.Rows[k]["clientid"].ToString())
                        {


                            //1
                            PdfPCell CSNo = new PdfPCell(new Phrase(dt.Rows[i]["sno"].ToString() + "\n \n \n \n", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                            CSNo.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            CSNo.Border = 15;
                            Secondtable.AddCell(CSNo);




                            //5
                            if (dts != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["NoOfDuties"].ToString().Trim().Length > 0)
                                    forConvert = Convert.ToSingle(dt.Rows[i]["NoOfDuties"].ToString());
                                totalDuties += forConvert;
                                GrandtotalDuties += forConvert;

                                PdfPCell CNoOfDuties = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CNoOfDuties.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CNoOfDuties.Border = 15;
                                Secondtable.AddCell(CNoOfDuties);
                            }


                            if (wo != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["WO"].ToString().Trim().Length > 0)
                                    forConvert = Convert.ToSingle(dt.Rows[i]["WO"].ToString());
                                totalwo += forConvert;
                                Grandtotalwo += forConvert;

                                PdfPCell Cwo = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Cwo.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cwo.Border = 15;
                                Secondtable.AddCell(Cwo);
                            }

                            //6
                            if (ots != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Duties"].ToString().Trim().Length > 0)
                                    forConvert = Convert.ToSingle(dt.Rows[i]["Duties"].ToString());
                                totalots += forConvert;
                                Grandtotalots += forConvert;

                                PdfPCell COts = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                COts.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                COts.Border = 15;
                                Secondtable.AddCell(COts);
                            }


                            if (nhs != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["NHS"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["NHS"].ToString()));
                                totalnhs += forConvert;
                                Grandtotalnhs += forConvert;

                                PdfPCell Cnhs1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Cnhs1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cnhs1.Border = 15;
                                Secondtable.AddCell(Cnhs1);
                            }


                            if (npots != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Npots"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Npots"].ToString()));
                                totalnpots += forConvert;
                                Grandtotalnpots += forConvert;

                                PdfPCell Cnpots1 = new PdfPCell(new Phrase(forConvert.ToString("0.00"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Cnpots1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cnpots1.Border = 15;
                                Secondtable.AddCell(Cnpots1);
                            }





                            //7
                            if (basic != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Basic"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Basic"].ToString()));
                                totalBasic += forConvert;
                                GrandtotalBasic += forConvert;
                                PdfPCell CBasic1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CBasic1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CBasic1.Border = 15;
                                Secondtable.AddCell(CBasic1);
                            }

                            //8
                            if (da != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["DA"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["DA"].ToString()));
                                totalDA += forConvert;
                                GrandtotalDA += forConvert;

                                PdfPCell CDa1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CDa1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CDa1.Border = 15;
                                Secondtable.AddCell(CDa1);
                            }

                            //9
                            if (hra != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["HRA"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["HRA"].ToString()));
                                totalHRA += forConvert;
                                GrandtotalHRA += forConvert;

                                PdfPCell CHRA1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CHRA1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CHRA1.Border = 15;
                                Secondtable.AddCell(CHRA1);
                            }

                            //10
                            if (cca != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["cca"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["cca"].ToString()));
                                totalCCA += forConvert;

                                GrandtotalCCA += forConvert;

                                PdfPCell Ccca = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Ccca.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Ccca.Border = 15;
                                Secondtable.AddCell(Ccca);
                            }

                            //11
                            if (conveyance != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Conveyance"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Conveyance"].ToString()));
                                totalConveyance += forConvert;
                                GrandtotalConveyance += forConvert;

                                PdfPCell CConveyance = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CConveyance.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CConveyance.Border = 15;
                                Secondtable.AddCell(CConveyance);
                            }


                            //12
                            if (washallowance != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["washallowance"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["washallowance"].ToString()));
                                totalWA += forConvert;
                                GrandtotalWA += forConvert;

                                PdfPCell CWa = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CWa.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CWa.Border = 15;
                                Secondtable.AddCell(CWa);
                            }

                            //13
                            if (otherallowance != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["OtherAllowance"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["OtherAllowance"].ToString()));
                                totalOA += forConvert;
                                GrandtotalOA += forConvert;
                                PdfPCell COA = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                COA.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                COA.Border = 15;
                                Secondtable.AddCell(COA);
                            }

                            //14
                            if (leavewages != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Leavewages"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Leavewages"].ToString()));
                                totalLw += forConvert;
                                GrandtotalLw += forConvert;
                                PdfPCell CLa1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CLa1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CLa1.Border = 15;
                                Secondtable.AddCell(CLa1);
                            }
                            //15
                            if (gratuity != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Gratuity"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Gratuity"].ToString()));
                                totalGratuity += forConvert;
                                GrandtotalGratuity += forConvert;
                                PdfPCell CGratuity1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CGratuity1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CGratuity1.Border = 15;
                                Secondtable.AddCell(CGratuity1);
                            }

                            //16
                            if (bonus != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["bonus"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["bonus"].ToString()));
                                totalbonus += forConvert;

                                Grandtotalbonus += forConvert;
                                PdfPCell Cbonus = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Cbonus.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cbonus.Border = 15;
                                Secondtable.AddCell(Cbonus);
                            }



                            //17
                            if (nfhs != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Nfhs"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Nfhs"].ToString()));
                                totalNfhs += forConvert;
                                GrandtotalNfhs += forConvert;
                                PdfPCell CNfhs1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CNfhs1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CNfhs1.Border = 15;
                                Secondtable.AddCell(CNfhs1);
                            }

                            //18
                            if (rc != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["RC"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["RC"].ToString()));
                                totalRc += forConvert;
                                GrandtotalRc += forConvert;
                                PdfPCell CRc1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CRc1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CRc1.Border = 15;
                                Secondtable.AddCell(CRc1);
                            }

                            //19
                            if (cs != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["cs"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["cs"].ToString()));
                                totalCs += forConvert;
                                GrandtotalCs += forConvert;
                                PdfPCell CCs1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CCs1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CCs1.Border = 15;
                                Secondtable.AddCell(CCs1);
                            }


                            //22
                            if (incentivs != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Incentivs"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Incentivs"].ToString()));
                                totalIncentivs += forConvert;
                                GrandtotalIncentivs += forConvert;
                                PdfPCell CIncentivs1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CIncentivs1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CIncentivs1.Border = 15;
                                Secondtable.AddCell(CIncentivs1);
                            }

                            if (woamt != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["WOAmt"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["WOAmt"].ToString()));
                                totalwoamt += forConvert;
                                Grandtotalwoamt += forConvert;

                                PdfPCell Cwoamt1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Cwoamt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cwoamt1.Border = 15;
                                Secondtable.AddCell(Cwoamt1);
                            }

                            if (nhsamt != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Nhsamt"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Nhsamt"].ToString()));
                                totalnhsamt += forConvert;
                                Grandtotalnhsamt += forConvert;

                                PdfPCell Cnhsamt1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Cnhsamt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cnhsamt1.Border = 15;
                                Secondtable.AddCell(Cnhsamt1);
                            }


                            if (npotsamt != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Npotsamt"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Npotsamt"].ToString()));
                                totalnpotsamt += forConvert;
                                Grandtotalnpotsamt += forConvert;

                                PdfPCell Cnpotsamt1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Cnpotsamt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cnpotsamt1.Border = 15;
                                Secondtable.AddCell(Cnpotsamt1);
                            }


                            //20
                            if (gross != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Gross"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Gross"].ToString()));
                                totalGross += forConvert;
                                GrandtotalGross += forConvert;

                                PdfPCell CGross1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CGross1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CGross1.Border = 15;
                                Secondtable.AddCell(CGross1);
                            }



                            //21
                            if (otamt != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Amount"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Amount"].ToString()));
                                totalotamt += forConvert;
                                Grandtotalotamt += forConvert;

                                PdfPCell CGross1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CGross1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CGross1.Border = 15;
                                Secondtable.AddCell(CGross1);
                            }

                            if (subtotal != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["subtotal"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["subtotal"].ToString()));


                                PdfPCell CGross1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CGross1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CGross1.Border = 15;
                                Secondtable.AddCell(CGross1);
                            }


                            //23
                            if (pfonduties != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Pfonduties"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Pfonduties"].ToString()));
                                totalPFondts += forConvert;
                                GrandtotalPFondts += forConvert;

                                PdfPCell CPF1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CPF1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CPF1.Border = 15;
                                Secondtable.AddCell(CPF1);
                            }

                            //24
                            if (esionduties != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Esionduties"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Esionduties"].ToString()));
                                totalESIondts += forConvert;

                                GrandtotalESIondts += forConvert;

                                PdfPCell CESI1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CESI1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CESI1.Border = 15;
                                Secondtable.AddCell(CESI1);
                            }

                            //25
                            if (pfonot != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["PFONOT"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["PFONOT"].ToString()));
                                totalpfonots += forConvert;
                                Grandtotalpfonots += forConvert;

                                PdfPCell CPFonots = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CPFonots.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CPFonots.Border = 15;
                                Secondtable.AddCell(CPFonots);
                            }

                            //26
                            if (esionot != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["ESIONOT"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["ESIONOT"].ToString()));
                                totalesionots += forConvert;

                                Grandtotalesionots += forConvert;

                                PdfPCell CESIonots = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CESIonots.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CESIonots.Border = 15;
                                Secondtable.AddCell(CESIonots);
                            }

                            //27
                            if (Pf != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["PFTotal"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["PFTotal"].ToString()));
                                totalpf += forConvert;
                                Grandtotalpf += forConvert;

                                PdfPCell CPFonots = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CPFonots.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CPFonots.Border = 15;
                                Secondtable.AddCell(CPFonots);
                            }

                            //28
                            if (Esi != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["ESITotal"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["ESITotal"].ToString()));
                                totalesi += forConvert;

                                Grandtotalesi += forConvert;

                                PdfPCell CESIonots = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CESIonots.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CESIonots.Border = 15;
                                Secondtable.AddCell(CESIonots);
                            }

                            //29
                            if (proftax != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["ProfTax"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["ProfTax"].ToString()));
                                totalProfTax += forConvert;
                                GrandtotalProfTax += forConvert;

                                PdfPCell CProTax1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CProTax1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CProTax1.Border = 15;
                                Secondtable.AddCell(CProTax1);
                            }

                            if (owf != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["owf"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["owf"].ToString()));
                                totalowf += forConvert;
                                Grandtotalowf += forConvert;

                                PdfPCell Cowf1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                Cowf1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cowf1.Border = 15;
                                Secondtable.AddCell(Cowf1);
                            }

                            //30
                            if (salAdvDed != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["SalAdvDed"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["SalAdvDed"].ToString()));
                                totalSalAdv += forConvert;
                                GrandtotalSalAdv += forConvert;

                                PdfPCell CSalAdv1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CSalAdv1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CSalAdv1.Border = 15;
                                Secondtable.AddCell(CSalAdv1);
                            }

                            //31
                            if (uniformDed != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["UniformDed"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["UniformDed"].ToString()));
                                totalUniforDed += forConvert;
                                GrandtotalUniforDed += forConvert;

                                PdfPCell CUnifDed1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CUnifDed1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CUnifDed1.Border = 15;
                                Secondtable.AddCell(CUnifDed1);
                            }

                            //32
                            if (otherDed != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["OtherDed"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["OtherDed"].ToString()));

                                PdfPCell COtherDed1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                COtherDed1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                COtherDed1.Border = 15;
                                Secondtable.AddCell(COtherDed1);
                            }

                            //34
                            if (roomrent != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["penalty"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["penalty"].ToString()));


                                PdfPCell CPenalty1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CPenalty1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CPenalty1.Border = 15;
                                Secondtable.AddCell(CPenalty1);
                            }


                            //33
                            if (canteenAdv != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["CanteenAdv"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["CanteenAdv"].ToString()));
                                totalCanteenAdv += forConvert;
                                GrandtotalCanteenAdv += forConvert;

                                PdfPCell CCanteended = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CCanteended.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CCanteended.Border = 15;
                                Secondtable.AddCell(CCanteended);
                            }

                            if (SecDepDedn != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["SecDepDedn"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["SecDepDedn"].ToString()));
                                totalSecDepDedn += forConvert;
                                GrandtotalSecDepDedn += forConvert;

                                PdfPCell CSecDepDedn1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CSecDepDedn1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CSecDepDedn1.Border = 15;
                                Secondtable.AddCell(CSecDepDedn1);
                            }


                            if (GenDedn != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["GenDedn"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["GenDedn"].ToString()));
                                totalGenDedn += forConvert;
                                GrandtotalGenDedn += forConvert;

                                PdfPCell CGenDedn1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CGenDedn1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CGenDedn1.Border = 15;
                                Secondtable.AddCell(CGenDedn1);
                            }




                            //35
                            if (totalDeductions != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["Totaldeduct"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["Totaldeduct"].ToString()));
                                totalDed += forConvert;
                                GrandtotalDed += forConvert;

                                PdfPCell CTotDed1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CTotDed1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CTotDed1.Border = 15;
                                Secondtable.AddCell(CTotDed1);//OtherDed,Eps.Gross,Eps.Deductions,Eps.ActualAmount
                            }

                            //36
                            if (netpay != 0)
                            {
                                forConvert = 0;
                                if (dt.Rows[i]["NetAmount"].ToString().Trim().Length > 0)
                                    forConvert = (float)Math.Round(Convert.ToSingle(dt.Rows[i]["NetAmount"].ToString()));
                                if (forConvert <= 0)
                                {
                                    forConvert = 0;
                                }
                                totalNetpay += forConvert;
                                GrandtotalNetpay += forConvert;
                                PdfPCell CNetPay1 = new PdfPCell(new Phrase(forConvert.ToString("0"), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                                CNetPay1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CNetPay1.Border = 15;
                                Secondtable.AddCell(CNetPay1);
                            }



                        }
                        #endregion

                    }
                    document.Add(Secondtable);
                    #endregion



                }




                document.Close();
                if (nextpagehasPages)
                {

                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Dutieswithots.pdf");
                    Response.Buffer = true;
                    Response.Clear();
                    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    Response.OutputStream.Flush();
                    Response.End();
                }
            }


        }

        public class PageEventHelper : PdfPageEventHelper
        {
            public DataRow MyRow { get; set; }
            // public DataTable dt { get; set; }
            public string list { get; set; }
            public string CmpIDPrefix { get; set; }
            public Document document { get; set; }
            public string txtmonth { get; set; }
            public string clientid { get; set; }
            public string clientname { get; set; }
            public string month { get; set; }
            public string Year { get; set; }
            public string Clientids { get; set; }


            public string GetMonthName()
            {
                string monthname = string.Empty;
                int payMonth = 0;
                DateTimeFormatInfo mfi = new DateTimeFormatInfo();



                DateTime date = Convert.ToDateTime(txtmonth, CultureInfo.GetCultureInfo("en-gb"));
                monthname = mfi.GetMonthName(date.Month).ToString();
                //payMonth = GetMonth(monthname);

                return monthname;
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
            public int GetMonthBasedOnSelectionDateorMonth()
            {

                var testDate = 0;
                string EnteredDate = "";



                #region  Month Get Based on the Control Selection
                int month = 0;

                DateTime date = DateTime.Parse(txtmonth, CultureInfo.GetCultureInfo("en-gb"));
                month = Timings.Instance.GetIdForEnteredMOnth(date);
                return month;


                #endregion
            }



            string Fontstyle = "Verdana";
            public override void OnStartPage(PdfWriter writer, Document document)
            {

                AppConfiguration config = new AppConfiguration();

                document.SetMargins(document.LeftMargin, document.LeftMargin, document.TopMargin, document.BottomMargin); //Mirror the horizontal margins

                string selectclientaddress = "select * from clients where clientid  in (" + list + ")";
                DataTable dtclientaddress = config.ExecuteAdaptorAsyncWithQueryParams(selectclientaddress).Result;


                string query = "select ROW_NUMBER() over( partition by eps.clientid order by eps.clientid) as Sno,clientname,eps.clientid,sum(EPS.NoOfDuties) as NoOfDuties,sum(EPS.Basic) as Basic,sum(EPS.DA) as DA,sum(EPS.HRA) as HRA,sum(EPS.CCA) as CCA,sum(EPS.Conveyance) as Conveyance,sum(EPS.WashAllowance) as WashAllowance," +
           "sum( EPS.OtherAllowance) as OtherAllowance,sum(EPS.LeaveEncashAmt) as Leavewages,sum(EPS.Gratuity) as Gratuity,sum(EPS.Bonus) as Bonus,sum(EPS.Nfhs) as nfhs,sum(eps.WO) as wo,sum(eps.WOAmt) as woamt,sum(eps.Npotsamt) as Npotsamt,sum(eps.Nhsamt) as Nhsamt,sum(eps.Npots) as Npots,sum(eps.nhs) as nhs," +
           " sum(EPS.RC) as RC,sum(EPS.cs) as CS,sum(EPS.Gross) as gross,sum(eps.Npots) as npots,sum(eps.NHS) as nhs,sum((eps.gross+eps.otamt)) as subtotal,   " +
            "sum(EPS.Incentivs) as Incentivs ,sum(EPS.Pfonduties) as Pfonduties,sum(EPS.Esionduties) as Esionduties,sum(EPS.PFONOT) as PFONOT,sum(EPS.ESIONOT) as ESIONOT,sum(EPS.PFONOT+EPS.Pfonduties) as PFTotal, " +
           " sum((EPS.Esionduties+EPS.ESIONOT)) ESITotal,sum(EPS.ProfTax) as ProfTax,sum(EPS.owf) as owf,sum(EPS.SalAdvDed) as SalAdvDed,sum(EPS.UniformDed) as UniformDed,sum(EPS.OtherDed) as OtherDed,sum(EPS.CanteenAdv) as CanteenAdv , " +
           " sum(EPS.Penalty) as Penalty,sum(EPS.Gross) as gross,sum(eps.SecurityDepDed) as SecDepDedn,sum(eps.penalty) as roomrentded,sum(eps.GeneralDed) as GenDedn,  " +
              " sum((EPS.Pf+EPS.Esi+EPS.ProfTax+EPS.SalAdvDed+EPS.UniformDed+EPS.OtherDed+EPS.CanteenAdv+EPS.Penalty+ISNULL(eps.roomrentded,0)+ISNULL(eps.SecurityDepDed,0)+isnull(eps.roomrentded,0)+ISNULL(GeneralDed,0)+ISNULL(owf,0))) Totaldeduct, " +
              " sum(((EPS.Gross+EPS.OTAmt)-(EPS.Pf+EPS.Esi+EPS.ProfTax+EPS.SalAdvDed+EPS.UniformDed+isnull(eps.roomrentded,0) +ISNULL(eps.SecurityDepDed,0)+ISNULL(GeneralDed,0)+ISNULL(eps.roomrentded,0)+ " +
              " EPS.OtherDed+EPS.CanteenAdv+EPS.Penalty+ISNULL(owf,0)))) NetAmount,sum(EPS.ots) as Duties,  " +
               "sum(EPS.OTAmt) as Amount " +
           " from  EmpPaySheet EPS  " +
           " join Designations D on D.DesignId=EpS.Desgn inner join clients c on c.clientid =eps.clientid where EPS.Month='" + month + Year.Substring(2, 2) + "'  and EPS.ClientId in (" + Clientids + ") and " +
            "(EPS.NoOfDuties+EPS.ots)!=0 and (EPS.NoOfDuties+EPS.ots)>0 group by eps.clientid,clientname order by eps.clientid ";

                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;


                if (dt.Rows.Count > 0)
                {



                    #region Variables for table cells counting

                    int dts = 0;
                    int srate = 0;
                    int basic = 0;
                    int da = 0;
                    int hra = 0;
                    int cca = 0;
                    int conveyance = 0;
                    int washallowance = 0;
                    int otherallowance = 0;
                    int leavewages = 0;
                    int gratuity = 0;
                    int bonus = 0;
                    int nfhs = 0;
                    int rc = 0;
                    int cs = 0;
                    int gross = 0;
                    int incentivs = 0;
                    int pfonduties = 0;
                    int esionduties = 0;
                    int proftax = 0;
                    int owf = 0;
                    int salAdvDed = 0;
                    int uniformDed = 0;
                    int otherDed = 0;
                    int canteenAdv = 0;
                    int roomrent = 0;
                    int totalDeductions = 0;
                    int netpay = 0;

                    int ots = 0;
                    int wo = 0;
                    int npots = 0;
                    int nhs = 0;
                    int woamt = 0;
                    int npotsamt = 0;
                    int nhsamt = 0;
                    int otamt = 0;
                    int pfonot = 0;
                    int esionot = 0;
                    int Pf = 0;
                    int Esi = 0;
                    int GenDedn = 0;
                    int SecDepDedn = 0;
                    int subtotal = 0;

                    float dts1 = 0;
                    float owf1 = 0;
                    float srate1 = 0;
                    float basic1 = 0;
                    float da1 = 0;
                    float hra1 = 0;
                    float cca1 = 0;
                    float conveyance1 = 0;
                    float washallowance1 = 0;
                    float otherallowance1 = 0;
                    float leavewages1 = 0;
                    float gratuity1 = 0;
                    float bonus1 = 0;
                    float nfhs1 = 0;
                    float rc1 = 0;
                    float cs1 = 0;
                    float gross1 = 0;
                    float incentivs1 = 0;
                    float pfonduties1 = 0;
                    float esionduties1 = 0;
                    float proftax1 = 0;
                    float salAdvDed1 = 0;
                    float uniformDed1 = 0;
                    float otherDed1 = 0;
                    float canteenAdv1 = 0;
                    float roomrent1 = 0;
                    float totalDeductions1 = 0;
                    float netpay1 = 0;
                    float subtotal1 = 0;

                    float ots1 = 0;
                    float wo1 = 0;
                    float npots1 = 0;
                    float nhs1 = 0;
                    float woamt1 = 0;
                    float npotsamt1 = 0;
                    float nhsamt1 = 0;
                    float otamt1 = 0;
                    float pfonot1 = 0;
                    float esionot1 = 0;
                    float Pf1 = 0;
                    float Esi1 = 0;
                    float GenDedn1 = 0;
                    float SecDepDedn1 = 0;


                    #endregion


                    #region Data for counting tablecells

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dts1 = float.Parse(dt.Rows[i]["NoOfDuties"].ToString());
                        if (dts1 != 0)
                        {
                            dts1 += 1;
                            if (dts1 > 0)
                            {
                                dts = 1;
                            }
                        }

                        owf1 = float.Parse(dt.Rows[i]["owf"].ToString());
                        if (owf1 != 0)
                        {
                            owf1 += 1;
                            if (owf1 > 0)
                            {
                                owf = 1;
                            }
                        }

                        //srate1 = float.Parse(dt.Rows[i]["SalRate"].ToString());
                        //if (srate1 != 0)
                        //{
                        //    srate1 += 1;
                        //    if (srate1 > 0)
                        //    {
                        //        srate = 1;
                        //    }
                        //}

                        basic1 = float.Parse(dt.Rows[i]["basic"].ToString());
                        if (basic1 != 0)
                        {
                            basic1 += 1;
                            if (basic1 > 0)
                            {
                                basic = 1;
                            }
                        }
                        da1 = float.Parse(dt.Rows[i]["da"].ToString());
                        if (da1 != 0)
                        {
                            da1 += 1;
                            if (da1 > 0)
                            {
                                da = 1;
                            }
                        }
                        hra1 = float.Parse(dt.Rows[i]["hra"].ToString());
                        if (hra1 != 0)
                        {
                            hra1 += 1;
                            if (hra1 > 0)
                            {
                                hra = 1;
                            }
                        }
                        cca1 = float.Parse(dt.Rows[i]["cca"].ToString());
                        if (cca1 != 0)
                        {
                            cca1 += 1;
                            if (cca1 > 0)
                            {
                                cca = 1;
                            }
                        }
                        conveyance1 = float.Parse(dt.Rows[i]["Conveyance"].ToString());
                        if (conveyance1 != 0)
                        {
                            conveyance1 += 1;
                            if (conveyance1 > 0)
                            {
                                conveyance = 1;
                            }
                        }

                        washallowance1 = float.Parse(dt.Rows[i]["washallowance"].ToString());
                        if (washallowance1 != 0)
                        {
                            washallowance1 += 1;
                            if (washallowance1 > 0)
                            {
                                washallowance = 1;
                            }
                        }
                        otherallowance1 = float.Parse(dt.Rows[i]["otherallowance"].ToString());
                        if (otherallowance1 != 0)
                        {
                            otherallowance1 += 1;
                            if (otherallowance1 > 0)
                            {
                                otherallowance = 1;
                            }
                        }
                        leavewages1 = float.Parse(dt.Rows[i]["Leavewages"].ToString());
                        if (leavewages1 != 0)
                        {
                            leavewages1 += 1;
                            if (leavewages1 > 0)
                            {
                                leavewages = 1;
                            }
                        }
                        gratuity1 = float.Parse(dt.Rows[i]["gratuity"].ToString());
                        if (gratuity1 != 0)
                        {
                            gratuity1 += 1;
                            if (gratuity1 > 0)
                            {
                                gratuity = 1;
                            }
                        }
                        bonus1 = float.Parse(dt.Rows[i]["bonus"].ToString());
                        if (bonus1 != 0)
                        {
                            bonus1 += 1;
                            if (bonus1 > 0)
                            {
                                bonus = 1;
                            }
                        }

                        nfhs1 = float.Parse(dt.Rows[i]["nfhs"].ToString());
                        if (nfhs1 != 0)
                        {
                            nfhs1 += 1;
                            if (nfhs1 > 0)
                            {
                                nfhs = 1;
                            }
                        }
                        rc1 = float.Parse(dt.Rows[i]["rc"].ToString());
                        if (rc1 != 0)
                        {
                            rc1 += 1;
                            if (rc1 > 0)
                            {
                                rc = 1;
                            }
                        }

                        cs1 = float.Parse(dt.Rows[i]["cs"].ToString());
                        if (cs1 != 0)
                        {
                            cs1 += 1;
                            if (cs1 > 0)
                            {
                                cs = 1;
                            }
                        }
                        gross1 = float.Parse(dt.Rows[i]["gross"].ToString());
                        if (gross1 != 0)
                        {
                            gross1 += 1;
                            if (gross1 > 0)
                            {
                                gross = 1;
                            }
                        }

                        incentivs1 = float.Parse(dt.Rows[i]["incentivs"].ToString());
                        if (incentivs1 != 0)
                        {
                            incentivs1 += 1;
                            if (incentivs1 > 0)
                            {
                                incentivs = 1;
                            }
                        }


                        woamt1 = float.Parse(dt.Rows[i]["WOAmt"].ToString());
                        if (woamt1 != 0)
                        {
                            woamt1 += 1;
                            if (woamt1 > 0)
                            {
                                woamt = 1;
                            }
                        }

                        npotsamt1 = float.Parse(dt.Rows[i]["Npotsamt"].ToString());
                        if (npotsamt1 != 0)
                        {
                            npotsamt1 += 1;
                            if (npotsamt1 > 0)
                            {
                                npotsamt = 1;
                            }
                        }
                        nhsamt1 = float.Parse(dt.Rows[i]["Nhsamt"].ToString());
                        if (nhsamt1 != 0)
                        {
                            nhsamt1 += 1;
                            if (nhsamt1 > 0)
                            {
                                nhsamt = 1;
                            }
                        }



                        //CHanged to zero so as to get PF and ESI in PFTotal and ESITotal as given below //Check PFTotal and ESITotal
                        pfonduties1 = float.Parse(dt.Rows[i]["PFonDuties"].ToString());
                        if (pfonduties1 != 0)
                        {
                            pfonduties1 += 1;
                            if (pfonduties1 > 0)
                            {
                                pfonduties = 0;
                            }
                        }
                        esionduties1 = float.Parse(dt.Rows[i]["ESIonduties"].ToString());
                        if (esionduties1 != 0)
                        {
                            esionduties1 += 1;
                            if (esionduties1 > 0)
                            {
                                esionduties = 0;
                            }
                        }
                        proftax1 = float.Parse(dt.Rows[i]["proftax"].ToString());
                        if (proftax1 != 0)
                        {
                            proftax1 += 1;
                            if (proftax1 > 0)
                            {
                                proftax = 1;
                            }
                        }
                        salAdvDed1 = float.Parse(dt.Rows[i]["salAdvDed"].ToString());
                        if (salAdvDed1 != 0)
                        {
                            salAdvDed1 += 1;
                            if (salAdvDed1 > 0)
                            {
                                salAdvDed = 1;
                            }
                        }
                        uniformDed1 = float.Parse(dt.Rows[i]["uniformDed"].ToString());
                        if (uniformDed1 != 0)
                        {
                            uniformDed1 += 1;
                            if (uniformDed1 > 0)
                            {
                                uniformDed = 1;
                            }
                        }

                        GenDedn1 = float.Parse(dt.Rows[i]["GenDedn"].ToString());
                        if (GenDedn1 != 0)
                        {
                            GenDedn1 += 1;
                            if (GenDedn1 > 0)
                            {
                                GenDedn = 1;
                            }
                        }

                        SecDepDedn1 = float.Parse(dt.Rows[i]["SecDepDedn"].ToString());
                        if (SecDepDedn1 != 0)
                        {
                            SecDepDedn1 += 1;
                            if (SecDepDedn1 > 0)
                            {
                                SecDepDedn = 1;
                            }
                        }

                        otherDed1 = float.Parse(dt.Rows[i]["otherDed"].ToString());
                        if (otherDed1 != 0)
                        {
                            otherDed1 += 1;
                            if (otherDed1 > 0)
                            {
                                otherDed = 1;
                            }
                        }
                        canteenAdv1 = float.Parse(dt.Rows[i]["canteenAdv"].ToString());
                        if (canteenAdv1 != 0)
                        {
                            canteenAdv1 += 1;
                            if (canteenAdv1 > 0)
                            {
                                canteenAdv = 1;
                            }
                        }

                        roomrent1 = float.Parse(dt.Rows[i]["roomrentded"].ToString());
                        if (roomrent1 != 0)
                        {
                            roomrent1 += 1;
                            if (roomrent1 > 0)
                            {
                                roomrent = 1;
                            }
                        }
                        totalDeductions1 = float.Parse(dt.Rows[i]["Totaldeduct"].ToString());
                        if (totalDeductions1 != 0)
                        {
                            totalDeductions1 += 1;
                            if (totalDeductions1 > 0)
                            {
                                totalDeductions = 1;
                            }
                        }
                        netpay1 = float.Parse(dt.Rows[i]["NetAmount"].ToString());
                        if (netpay1 != 0)
                        {
                            netpay1 += 1;
                            if (netpay1 > 0)
                            {
                                netpay = 1;
                            }
                        }

                        ots1 = float.Parse(dt.Rows[i]["Duties"].ToString());
                        if (ots1 != 0)
                        {
                            ots1 += 1;
                            if (ots1 > 0)
                            {
                                ots = 1;
                            }
                        }

                        wo1 = float.Parse(dt.Rows[i]["WO"].ToString());
                        if (wo1 != 0)
                        {
                            wo1 += 1;
                            if (wo1 > 0)
                            {
                                wo = 1;
                            }
                        }

                        npots1 = float.Parse(dt.Rows[i]["Npots"].ToString());
                        if (npots1 != 0)
                        {
                            npots1 += 1;
                            if (npots1 > 0)
                            {
                                npots = 1;
                            }
                        }

                        nhs1 = float.Parse(dt.Rows[i]["NHS"].ToString());
                        if (nhs1 != 0)
                        {
                            nhs1 += 1;
                            if (nhs1 > 0)
                            {
                                nhs = 1;
                            }
                        }

                        otamt1 = float.Parse(dt.Rows[i]["Amount"].ToString());
                        if (otamt1 != 0)
                        {
                            otamt1 += 1;
                            if (otamt1 > 0)
                            {
                                otamt = 1;
                            }
                        }

                        pfonot1 = float.Parse(dt.Rows[i]["PFONOT"].ToString());
                        if (pfonot1 != 0)
                        {
                            pfonot1 += 1;
                            if (pfonot1 > 0)
                            {
                                pfonot = 0;
                            }
                        }

                        esionot1 = float.Parse(dt.Rows[i]["ESIONOT"].ToString());
                        if (esionot1 != 0)
                        {
                            esionot1 += 1;
                            if (esionot1 > 0)
                            {
                                esionot = 0;
                            }
                        }

                        Pf1 = float.Parse(dt.Rows[i]["PFTotal"].ToString());
                        if (Pf1 != 0)
                        {
                            Pf1 += 1;
                            if (Pf1 > 0)
                            {
                                Pf = 1;
                            }
                        }
                        Esi1 = float.Parse(dt.Rows[i]["ESITotal"].ToString());
                        if (Esi1 != 0)
                        {
                            Esi1 += 1;
                            if (Esi1 > 0)
                            {
                                Esi = 1;
                            }
                        }

                        subtotal1 = float.Parse(dt.Rows[i]["subtotal"].ToString());
                        if (subtotal1 != 0)
                        {
                            subtotal1 += 1;
                            if (subtotal1 > 0)
                            {
                                subtotal = 1;
                            }
                        }


                    }

                    #endregion

                    int sno = 1;


                    int tableCells = dts + basic + da + hra + cca + conveyance + washallowance + otherallowance + leavewages +
                                   gratuity + bonus + nfhs + rc + cs + gross + incentivs + pfonduties + esionduties + proftax +
                                   salAdvDed + uniformDed + otherDed + canteenAdv + roomrent + totalDeductions + netpay + sno + subtotal +
                                    ots + wo + npots + nhs + otamt + pfonot + esionot + Pf + Esi + GenDedn + SecDepDedn + woamt + npotsamt + nhsamt + owf;




                    #region Titles of Document
                    PdfPTable Maintable = new PdfPTable(30);

                    if (tableCells > 20)
                    {
                        Maintable.TotalWidth = 1000f;
                    }
                    else
                    {
                        Maintable.TotalWidth = 850f;
                    }
                    Maintable.LockedWidth = true;
                    float[] width = new float[] { 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f };


                    Maintable.SetWidths(width);
                    uint FONT_SIZE = 8;
                    string FontStyle = "verdana";

                    #region Company Name & wage act details

                    string strQry = "Select * from CompanyInfo   where   ClientidPrefix='" + CmpIDPrefix + "'";
                    DataTable compInfo = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
                    string companyName1 = "Your Company Name";
                    if (compInfo.Rows.Count > 0)
                    {
                        companyName1 = compInfo.Rows[0]["CompanyName"].ToString();
                    }



                    PdfPCell Heading21 = new PdfPCell(new Phrase(companyName1, FontFactory.GetFont(Fontstyle, 10, Font.BOLD, BaseColor.BLACK)));
                    Heading21.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Heading21.Colspan = 30;
                    Heading21.Border = 0;
                    Heading21.SetLeading(0.0f, 1.5f);
                    Maintable.AddCell(Heading21);






                    PdfPCell CClient = new PdfPCell(new Phrase("Client Name :   " + clientname, FontFactory.GetFont(Fontstyle, 10, Font.BOLD, BaseColor.BLACK)));
                    CClient.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    CClient.Colspan = 15;
                    CClient.Border = 0;// 15;
                    Maintable.AddCell(CClient);

                    PdfPCell CMonth = new PdfPCell(new Phrase("For the Month :   " + GetMonthName() + " - " + GetMonthOfYear(), FontFactory.GetFont(Fontstyle, 10, Font.BOLD, BaseColor.BLACK)));
                    CMonth.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    CMonth.Colspan = 15;
                    CMonth.Border = 0;// 15;
                    Maintable.AddCell(CMonth);


                    PdfPCell cempcell = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cempcell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cempcell.Colspan = 30;
                    cempcell.MinimumHeight = 10;
                    cempcell.Border = 0;// 15;
                    Maintable.AddCell(cempcell);




                    document.Add(Maintable);

                    #endregion

                    #region Table Headings

                    PdfPTable SecondtableHeadings = new PdfPTable(tableCells);

                    if (tableCells > 20)
                    {
                        SecondtableHeadings.TotalWidth = 1000f;
                    }
                    else
                    {
                        SecondtableHeadings.TotalWidth = 850f;
                    }
                    SecondtableHeadings.LockedWidth = true;
                    float[] SecondHeadingsWidth = new float[] { };

                    #region Table Cells
                    if (tableCells == 37)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 36)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }

                    if (tableCells == 35)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }

                    if (tableCells == 34)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 33)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 32)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 31)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 30)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 29)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 28)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 27)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 26)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 25)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 24)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 23)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 22)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 21)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 20)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 19)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 18)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 17)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 16)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 15)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 14)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 13)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 12)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 11)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 10)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 1.5f, 2.5f };
                    }
                    if (tableCells == 9)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 1.5f, 2.5f };
                    }
                    if (tableCells == 8)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2f, 2f };
                    }

                    if (tableCells == 7)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 1.5f, 2.5f };
                    }

                    if (tableCells == 6)
                    {
                        SecondHeadingsWidth = new float[] { 1.5f, 2f, 2f, 3f, 1.5f, 2.5f };
                    }
                    #endregion

                    SecondtableHeadings.SetWidths(SecondHeadingsWidth);

                    //Cell Headings
                    //1
                    PdfPCell sNo = new PdfPCell(new Phrase("S.No", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    sNo.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                                 //sNo.Colspan = 1;
                    sNo.Border = 15;// 15;
                    SecondtableHeadings.AddCell(sNo);



                    //5
                    if (dts != 0)
                    {
                        PdfPCell CDuties = new PdfPCell(new Phrase("Dts", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CDuties.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CDuties.Border = 15;
                        SecondtableHeadings.AddCell(CDuties);
                    }

                    if (wo != 0)
                    {
                        PdfPCell Cwo = new PdfPCell(new Phrase("WO", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        Cwo.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        Cwo.Border = 15;
                        SecondtableHeadings.AddCell(Cwo);
                    }


                    //6
                    if (ots != 0)
                    {
                        PdfPCell Cots = new PdfPCell(new Phrase("ED", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        Cots.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        Cots.Border = 15;
                        SecondtableHeadings.AddCell(Cots);
                    }



                    if (nhs != 0)
                    {
                        PdfPCell Cnpots = new PdfPCell(new Phrase("NHS", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        Cnpots.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        Cnpots.Border = 15;
                        SecondtableHeadings.AddCell(Cnpots);
                    }

                    if (npots != 0)
                    {
                        PdfPCell Cnhs = new PdfPCell(new Phrase("Npots", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        Cnhs.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        Cnhs.Border = 15;
                        SecondtableHeadings.AddCell(Cnhs);
                    }




                    //7
                    if (basic != 0)
                    {
                        PdfPCell CBasic = new PdfPCell(new Phrase("Basic", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CBasic.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CBasic.Border = 15;
                        SecondtableHeadings.AddCell(CBasic);
                    }

                    //8
                    if (da != 0)
                    {
                        PdfPCell CDa = new PdfPCell(new Phrase("DA", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CDa.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CDa.Border = 15;
                        SecondtableHeadings.AddCell(CDa);
                    }

                    //9
                    if (hra != 0)
                    {
                        PdfPCell CHRa = new PdfPCell(new Phrase("HRA", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CHRa.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CHRa.Border = 15;
                        SecondtableHeadings.AddCell(CHRa);
                    }

                    //10
                    if (cca != 0)
                    {
                        PdfPCell CCca = new PdfPCell(new Phrase("CCA", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CCca.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CCca.Border = 15;
                        SecondtableHeadings.AddCell(CCca);
                    }

                    //11
                    if (conveyance != 0)
                    {
                        PdfPCell Cconveyance = new PdfPCell(new Phrase("Conv", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        Cconveyance.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        Cconveyance.Border = 15;
                        SecondtableHeadings.AddCell(Cconveyance);
                    }

                    //12
                    if (washallowance != 0)
                    {
                        PdfPCell Cwa = new PdfPCell(new Phrase("WA", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        Cwa.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        Cwa.Border = 15;
                        SecondtableHeadings.AddCell(Cwa);
                    }

                    //13
                    if (otherallowance != 0)
                    {
                        PdfPCell COa = new PdfPCell(new Phrase("OA", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        COa.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        COa.Border = 15;
                        SecondtableHeadings.AddCell(COa);
                    }

                    //14
                    if (leavewages != 0)
                    {
                        PdfPCell CLa = new PdfPCell(new Phrase("EL", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CLa.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CLa.Border = 15;
                        SecondtableHeadings.AddCell(CLa);
                    }

                    //15
                    if (gratuity != 0)
                    {
                        PdfPCell CGratuity = new PdfPCell(new Phrase("Gratuity", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CGratuity.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CGratuity.Border = 15;
                        SecondtableHeadings.AddCell(CGratuity);
                    }

                    //16
                    if (bonus != 0)
                    {
                        PdfPCell CBonus = new PdfPCell(new Phrase("Bonus", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CBonus.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CBonus.Border = 15;
                        SecondtableHeadings.AddCell(CBonus);
                    }




                    //17
                    if (nfhs != 0)
                    {
                        PdfPCell CNfhs = new PdfPCell(new Phrase("NFHs", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CNfhs.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CNfhs.Border = 15;
                        SecondtableHeadings.AddCell(CNfhs);
                    }

                    //18
                    if (rc != 0)
                    {
                        PdfPCell CRc = new PdfPCell(new Phrase("R.C", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CRc.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CRc.Border = 15;
                        SecondtableHeadings.AddCell(CRc);
                    }

                    //19
                    if (cs != 0)
                    {
                        PdfPCell CCs = new PdfPCell(new Phrase("C.S", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CCs.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CCs.Border = 15;
                        SecondtableHeadings.AddCell(CCs);
                    }

                    //22
                    if (incentivs != 0)
                    {
                        PdfPCell CIncentivs = new PdfPCell(new Phrase("Incentivs", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CIncentivs.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CIncentivs.Border = 15;
                        SecondtableHeadings.AddCell(CIncentivs);
                    }


                    if (woamt != 0)
                    {
                        PdfPCell Cwoamt = new PdfPCell(new Phrase("WO Amt", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        Cwoamt.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        Cwoamt.Border = 15;
                        SecondtableHeadings.AddCell(Cwoamt);
                    }

                    if (nhsamt != 0)
                    {
                        PdfPCell Cnpots = new PdfPCell(new Phrase("NHS Amt", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        Cnpots.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        Cnpots.Border = 15;
                        SecondtableHeadings.AddCell(Cnpots);
                    }

                    if (npotsamt != 0)
                    {
                        PdfPCell Cnhs = new PdfPCell(new Phrase("Npots Amt", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        Cnhs.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        Cnhs.Border = 15;
                        SecondtableHeadings.AddCell(Cnhs);
                    }


                    //20
                    if (gross != 0)
                    {
                        PdfPCell CGross = new PdfPCell(new Phrase("Gross", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CGross.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CGross.Border = 15;
                        SecondtableHeadings.AddCell(CGross);
                    }




                    //21
                    if (otamt != 0)
                    {
                        PdfPCell COtamt = new PdfPCell(new Phrase("ED Amt", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        COtamt.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        COtamt.Border = 15;
                        SecondtableHeadings.AddCell(COtamt);
                    }


                    //21
                    if (subtotal != 0)
                    {
                        PdfPCell Csubtotal = new PdfPCell(new Phrase("Total Ernd", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        Csubtotal.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        Csubtotal.Border = 15;
                        SecondtableHeadings.AddCell(Csubtotal);
                    }



                    //23
                    if (pfonduties != 0)
                    {
                        PdfPCell CPFondts = new PdfPCell(new Phrase("PF on Dts", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CPFondts.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CPFondts.Border = 15;
                        SecondtableHeadings.AddCell(CPFondts);
                    }

                    //24
                    if (esionduties != 0)
                    {
                        PdfPCell CESIondts = new PdfPCell(new Phrase("ESI on Dts", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CESIondts.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CESIondts.Border = 15;
                        SecondtableHeadings.AddCell(CESIondts);
                    }

                    //25
                    if (pfonot != 0)
                    {
                        PdfPCell CPFonot = new PdfPCell(new Phrase("PF on OTs", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CPFonot.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CPFonot.Border = 15;
                        SecondtableHeadings.AddCell(CPFonot);
                    }

                    //26
                    if (esionot != 0)
                    {
                        PdfPCell CESIonot = new PdfPCell(new Phrase("ESI on OTs", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CESIonot.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CESIonot.Border = 15;
                        SecondtableHeadings.AddCell(CESIonot);
                    }

                    //27
                    if (Pf != 0)
                    {
                        PdfPCell CPF = new PdfPCell(new Phrase("PF", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CPF.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CPF.Border = 15;
                        SecondtableHeadings.AddCell(CPF);
                    }

                    //28
                    if (Esi != 0)
                    {
                        PdfPCell CESI = new PdfPCell(new Phrase("ESI", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CESI.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CESI.Border = 15;
                        SecondtableHeadings.AddCell(CESI);
                    }


                    //29
                    if (proftax != 0)
                    {
                        PdfPCell CPT = new PdfPCell(new Phrase("PT", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CPT.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CPT.Border = 15;
                        SecondtableHeadings.AddCell(CPT);
                    }

                    if (owf != 0)
                    {
                        PdfPCell Cowf = new PdfPCell(new Phrase("LWF", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        Cowf.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        Cowf.Border = 15;
                        SecondtableHeadings.AddCell(Cowf);
                    }

                    //30
                    if (salAdvDed != 0)
                    {
                        PdfPCell CSalAdv = new PdfPCell(new Phrase("Sal Adv", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CSalAdv.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CSalAdv.Border = 15;
                        SecondtableHeadings.AddCell(CSalAdv);
                    }

                    //31
                    if (uniformDed != 0)
                    {
                        PdfPCell CUnifDed = new PdfPCell(new Phrase("Unif. Ded", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CUnifDed.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CUnifDed.Border = 15;
                        SecondtableHeadings.AddCell(CUnifDed);
                    }

                    //32
                    if (otherDed != 0)
                    {
                        PdfPCell COtherDed = new PdfPCell(new Phrase("Other Ded", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        COtherDed.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        COtherDed.Border = 15;
                        SecondtableHeadings.AddCell(COtherDed);
                    }

                    //34
                    if (roomrent != 0)
                    {
                        PdfPCell CPenalty = new PdfPCell(new Phrase("Room Rent", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CPenalty.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CPenalty.Border = 15;
                        SecondtableHeadings.AddCell(CPenalty);
                    }

                    //33
                    if (canteenAdv != 0)
                    {
                        PdfPCell Ccanteended = new PdfPCell(new Phrase("Mess. Ded", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        Ccanteended.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        Ccanteended.Border = 15;
                        SecondtableHeadings.AddCell(Ccanteended);
                    }


                    if (SecDepDedn != 0)
                    {
                        PdfPCell CSecDepDedn = new PdfPCell(new Phrase("Sec Dep Ded", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CSecDepDedn.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CSecDepDedn.Border = 15;
                        SecondtableHeadings.AddCell(CSecDepDedn);
                    }

                    if (GenDedn != 0)
                    {
                        PdfPCell CGenDedn = new PdfPCell(new Phrase("Gen Ded", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CGenDedn.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CGenDedn.Border = 15;
                        SecondtableHeadings.AddCell(CGenDedn);
                    }




                    //35
                    if (totalDeductions != 0)
                    {
                        PdfPCell CTotDed = new PdfPCell(new Phrase("Tot Ded", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CTotDed.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CTotDed.Border = 15;
                        SecondtableHeadings.AddCell(CTotDed);
                    }

                    //36
                    if (netpay != 0)
                    {
                        PdfPCell CNetPay = new PdfPCell(new Phrase("Net Pay", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                        CNetPay.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        CNetPay.Border = 15;
                        SecondtableHeadings.AddCell(CNetPay);
                    }



                    document.Add(SecondtableHeadings);

                    #endregion





                    #endregion
                }
            }
        }

    }
}