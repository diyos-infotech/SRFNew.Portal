using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class ReportforProfitLoss : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {
                  
                    Loadclientids();
                    Loadclientnames();
                }
                else
                {
                    Response.Redirect("login.aspx");
                }

            }
        }


        protected void Loadclientids()
        {
            string sqlclientids = "select clientid from clients  Order by Clientid";
            DataTable dtids = config.ExecuteAdaptorAsyncWithQueryParams(sqlclientids).Result;
            if (dtids.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "clientid";
                ddlclientid.DataTextField = "clientid";
                ddlclientid.DataSource = dtids;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "--Select--");
            ddlclientid.Items.Insert(1, "All");
        }

        protected void Loadclientnames()
        {
            string sqlclientnames = "select clientid,clientname from clients    order by clientname";
            DataTable dtnames = config.ExecuteAdaptorAsyncWithQueryParams(sqlclientnames).Result;
            if (dtnames.Rows.Count > 0)
            {
                ddlcname.DataValueField = "clientid";
                ddlcname.DataTextField = "clientname";
                ddlcname.DataSource = dtnames;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "--Select--");
            ddlcname.Items.Insert(1, "All");
        }

        protected void Fillclientname()
        {
            if (ddlclientid.SelectedIndex > 1)
            {
                ddlcname.SelectedValue = ddlclientid.SelectedValue;
            }
            if (ddlclientid.SelectedIndex == 1)
            {
                ddlcname.SelectedIndex = 1;
            }

            if (ddlclientid.SelectedIndex == 0)
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void Fillclientid()
        {
            if (ddlcname.SelectedIndex > 1)
            {
                ddlclientid.SelectedValue = ddlcname.SelectedValue;
            }
            if (ddlcname.SelectedIndex == 1)
            {
                ddlclientid.SelectedIndex = 1;
            }

            if (ddlcname.SelectedIndex == 0)
            {
                ddlclientid.SelectedIndex = 0;
            }

        }

        protected void ddlclientid_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvreport.DataSource = null;
            gvreport.DataBind();
            if (ddlclientid.SelectedIndex > 0)
            {
                Fillclientname();
                //Displaydata();
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }

        }

        protected void ddlclientname_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvreport.DataSource = null;
            gvreport.DataBind();
            if (ddlcname.SelectedIndex > 0)
            {
                Fillclientid();
                // Displaydata();
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        protected void gvreport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvreport.PageIndex = e.NewPageIndex;
            gvreport.DataBind();
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Select The Month')", true);
                return;
            }
            string sqlclientdata = string.Empty;
            string SqlqryForBilling = string.Empty;
            string SqlqryForPaysheet = string.Empty;
            string sqlclientdataManual = string.Empty;

            string month = DateTime.Parse(txtmonth.Text.Trim()).Month.ToString();
            string Year = DateTime.Parse(txtmonth.Text.Trim()).Year.ToString();
            if (ddlclientid.SelectedIndex == 1)
            {
                try
                {
                    sqlclientdata = "select Clientid,Clientname from Clients  Where Mainunitstatus=1 order by Clientid ";

                }

                catch
                {

                }
            }

            if (ddlclientid.SelectedIndex > 1)
            {
                try
                {
                    sqlclientdata = "select Clientid,Clientname from Clients  Where clientid='" + ddlclientid.SelectedValue + "'";
                }
                catch
                {

                }
            }
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlclientdata).Result;
            if (dt.Rows.Count > 0)
            {
                gvreport.DataSource = dt;
                gvreport.DataBind();

                foreach (GridViewRow gvrow in gvreport.Rows)
                {
                    Label lblclientid = (Label)gvrow.FindControl("lblclientid");

                    Label lblbillno = (Label)gvrow.FindControl("lblbillno");

                    Label lblinvoice = (Label)gvrow.FindControl("lblinvoice");
                    lblinvoice.Text = "0.00";

                    Label lblservicetaxamt = (Label)gvrow.FindControl("lblservicetaxamt");
                    lblservicetaxamt.Text = "0.00";

                    Label lblNet = (Label)gvrow.FindControl("lblNet");
                    lblNet.Text = "0.00";

                    Label lblgross = (Label)gvrow.FindControl("lblgross");
                    lblgross.Text = "0.00";

                    Label lblpfemployer = (Label)gvrow.FindControl("lblpfemployer");
                    lblpfemployer.Text = "0.00";

                    Label lblesiemployer = (Label)gvrow.FindControl("lblesiemployer");
                    lblesiemployer.Text = "0.00";

                    Label lblTotalexpensives = (Label)gvrow.FindControl("lblTotalexpensives");
                    lblTotalexpensives.Text = "0.00";

                    Label lblTotalprofitorloss = (Label)gvrow.FindControl("lblTotalprofitorloss");
                    lblTotalprofitorloss.Text = "0.00";

                    Label lblTotalexpesivesperc = (Label)gvrow.FindControl("lblTotalexpesivesperc");
                    lblTotalexpesivesperc.Text = "0.00";

                    Label lblTotalprofit = (Label)gvrow.FindControl("lblTotalprofit");
                    lblTotalprofit.Text = "0.00";


                    //// sqlclientdata = "select u.BillNo," +
                    //// " isnull(u.GrandTotal,0) as GrandTotal,(u.ServiceTax+u.CESS+u.SHECess)" +
                    //// " as servicetax,(u.GrandTotal-(u.ServiceTax+u.CESS+u.SHECess)) as Nettotal,c.ClientId,c.ClientName" +
                    ////"  from UnitBill  as  u inner join  Clients as c  on c.ClientId=u.UnitId where u.Month='" + month + Year.Substring(2, 2) + "'" +
                    ////" and unitid='" + lblclientid.Text + "' order by right(BillNo,4)";

                    sqlclientdata = "select SUM( isnull(u.GrandTotal,0)) as GrandTotal,sum(u.ServiceTax+u.CESS+u.SHECess+u.sbcessamt+u.kkcessamt) " +
                                     " as servicetax,sum(u.GrandTotal-(u.ServiceTax+u.CESS+u.SHECess+u.sbcessamt+u.kkcessamt)) as Nettotal, " +
                                     " STUFF(( SELECT ' / '+BillNo+ ' ' from UnitBill inner join Clients  on UnitBill.UnitId=Clients.ClientId  where (unitid='" + lblclientid.Text + "' or MainUnitId='" + lblclientid.Text + "') and month='" + month + Year.Substring(2, 2) + "' for xml path(''),Type).value('(./text())[1]','VARCHAR(MAX)'),1,2,'') as Billno " +
                                     " from UnitBill  as  u inner join  Clients as c  on c.ClientId=u.UnitId where u.Month='" + month + Year.Substring(2, 2) + "' " +
                                     " and ( unitid='" + lblclientid.Text + "' or c.MainUnitId='" + lblclientid.Text + "')";

                    sqlclientdataManual = "select SUM( isnull(u.GrandTotal,0)) as GrandTotal,sum(u.ServiceTax+u.sbcessamt+u.CESS+u.SHECess+u.kkcessamt) " +
                                        " as servicetax,sum(u.GrandTotal-(u.ServiceTax+u.sbcessamt+u.CESS+u.SHECess+u.kkcessamt)) as Nettotal, " +
                                        " STUFF(( SELECT ' / '+BillNo+ ' ' from munitbill inner join Clients  on munitbill.UnitId=Clients.ClientId  where (unitid='" + lblclientid.Text + "' or MainUnitId='" + lblclientid.Text + "' ) and month='" + month + Year.Substring(2, 2) + "'  for xml path(''),Type).value('(./text())[1]','VARCHAR(MAX)'),1,2,'') as Billno " +
                                        " from munitbill  as  u inner join  Clients as c  on c.ClientId=u.UnitId where u.Month='" + month + Year.Substring(2, 2) + "' " +
                                        " and ( unitid='" + lblclientid.Text + "' or c.MainUnitId='" + lblclientid.Text + "' )";

                    DataTable Dtbilling = config.ExecuteAdaptorAsyncWithQueryParams(sqlclientdata).Result;
                    DataTable DtbillingManual = config.ExecuteAdaptorAsyncWithQueryParams(sqlclientdataManual).Result;
                    string CBBillno = ""; float CBGrandTotal = 0; float CBServiceTaxAmt = 0; float CBNet = 0; float CBProfitorLoss = 0;
                    string MBBillno = ""; float MBGrandTotal = 0; float MBServiceTaxAmt = 0; float MBNet = 0; float MBProfitorLoss = 0;


                    if (Dtbilling.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(Dtbilling.Rows[0]["BillNo"].ToString()) == false)
                        {
                            CBBillno = Dtbilling.Rows[0]["BillNo"].ToString();
                            CBGrandTotal = float.Parse(Dtbilling.Rows[0]["GrandTotal"].ToString());
                            CBServiceTaxAmt = float.Parse(Dtbilling.Rows[0]["servicetax"].ToString());
                            CBNet = float.Parse(Dtbilling.Rows[0]["Nettotal"].ToString());
                            CBProfitorLoss = float.Parse(CBNet.ToString());

                        }
                    }
                    if (DtbillingManual.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(DtbillingManual.Rows[0]["BillNo"].ToString()) == false)
                        {
                            MBBillno = DtbillingManual.Rows[0]["BillNo"].ToString() + "(M)";
                            MBGrandTotal = float.Parse(DtbillingManual.Rows[0]["GrandTotal"].ToString());
                            MBServiceTaxAmt = float.Parse(DtbillingManual.Rows[0]["servicetax"].ToString());
                            MBNet = float.Parse(DtbillingManual.Rows[0]["Nettotal"].ToString());
                            MBProfitorLoss = float.Parse(MBNet.ToString());

                        }
                    }

                    if (CBBillno.Length > 0 && MBBillno.Length > 0)
                    {
                        lblbillno.Text = CBBillno + "/" + MBBillno;
                    }
                    else if (CBBillno.Length > 0 && MBBillno.Length == 0)
                    {
                        lblbillno.Text = CBBillno;

                    }
                    else if (CBBillno.Length == 0 && MBBillno.Length > 0)
                    {
                        lblbillno.Text = MBBillno;
                    }
                    lblinvoice.Text = (CBGrandTotal + MBGrandTotal).ToString("0.00");
                    lblservicetaxamt.Text = (MBServiceTaxAmt + CBServiceTaxAmt).ToString("0.00");
                    lblNet.Text = (CBNet + MBNet).ToString("0.00");
                    lblTotalprofitorloss.Text = (MBProfitorLoss + CBProfitorLoss).ToString();

                    #region Begin   Variable Declarations

                    float GrandTotalGross = 0;
                    float GrandTotalPfEmployeer = 0;
                    float GrandTotalEsiEmployeer = 0;
                    float GrandTotalTotalExpensive = 0;

                    #endregion  End  Variable Declarations

                    #region  Begin Query For Sub client Belongs The Specific Client   As on [10-09-2013]

                    string SqlQryForSubClients = "select Clientid   From  Clients Where MainUnitID='" + lblclientid.Text + "' or clientid='" + lblclientid.Text + "'";
                    DataTable DtForSubClients = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForSubClients).Result;

                    if (DtForSubClients.Rows.Count > 0)
                    {
                        for (int IClient = 0; IClient < DtForSubClients.Rows.Count; IClient++)
                        {

                            float TotalGross = 0;
                            float TotalPfEmployeer = 0;
                            float TotalEsiEmployeer = 0;
                            float TotalTotalExpensive = 0;
                            string Clientid = DtForSubClients.Rows[IClient]["Clientid"].ToString();
                            string sqlclientdataSubtotal = "  select   SUM(isnull(ep.pf,0)) as emppf,SUM(isnull(ep.PFEmpr,0)) as pfempr,  (SUM(isnull(ep.pf,0))+SUM(isnull(ep.PFEmpr,0))) as totalpf," +
                                  " SUM(isnull(ep.esi,0)) as eesi,SUM(isnull(ep.esiempr,0)) as empresi, (SUM(isnull(ep.esi,0))+SUM(isnull(ep.esiempr,0))) as totalesi,  " +
                                  " ( sum(isnull(ep.Gross,0)) +  sum(isnull(ep.Otamt,0)) +   sum(isnull(ep.NHSAMT,0))+ sum(isnull(ep.Npotsamt,0)) ) as empgross,   " +
                                  " sum(isnull(ep.ProfTax,0)) emproftax ,(SUM(isnull(ep.PFEmpr,0))+sum(isnull(ep.Gross,0)) +  sum(isnull(ep.Otamt,0))+ sum(isnull(ep.NHSAMT,0))+sum(isnull(ep.Npotsamt,0))+SUM(isnull(ep.esiempr,0))) as totalexpenses   " +
                                  "   from EmpPaySheet as ep  where ep.ClientId='" + Clientid + "'   and MONTH='" + month + Year.Substring(2, 2) + "'";

                            DataTable DtPaysheetSubTotal = config.ExecuteAdaptorAsyncWithQueryParams(sqlclientdataSubtotal).Result;
                            if (DtPaysheetSubTotal.Rows.Count > 0)
                            {
                                if (String.IsNullOrEmpty(DtPaysheetSubTotal.Rows[0]["empgross"].ToString()) == false)
                                {
                                    TotalGross = float.Parse(DtPaysheetSubTotal.Rows[0]["empgross"].ToString());
                                    GrandTotalGross += TotalGross;

                                    TotalPfEmployeer = float.Parse(DtPaysheetSubTotal.Rows[0]["pfempr"].ToString());
                                    GrandTotalPfEmployeer += TotalPfEmployeer;

                                    TotalEsiEmployeer = float.Parse(DtPaysheetSubTotal.Rows[0]["empresi"].ToString());
                                    GrandTotalEsiEmployeer += TotalEsiEmployeer;

                                    TotalTotalExpensive = float.Parse(DtPaysheetSubTotal.Rows[0]["totalexpenses"].ToString());
                                    GrandTotalTotalExpensive += TotalTotalExpensive;

                                }
                            }

                        }


                        lblgross.Text = GrandTotalGross.ToString("0.00");

                        lblpfemployer.Text = GrandTotalPfEmployeer.ToString("0.00");

                        lblesiemployer.Text = GrandTotalEsiEmployeer.ToString("0.00");

                        lblTotalexpensives.Text = GrandTotalTotalExpensive.ToString("0.00");

                        /* Begin   total profit/loss*/
                        lblTotalprofitorloss.Text = (float.Parse(lblNet.Text) - float.Parse(lblTotalexpensives.Text)).ToString("0.00");
                        if (float.Parse(lblNet.Text) > 0)
                        {
                            lblTotalexpesivesperc.Text = (((float.Parse(lblTotalexpensives.Text) / float.Parse(lblNet.Text)) * 100).ToString("0.00"));
                        }
                        else
                        {
                            lblTotalexpesivesperc.Text = "100.00";
                        }


                        if (float.Parse(lblNet.Text) > 0)
                        {
                            lblTotalprofit.Text = (100 - float.Parse(lblTotalexpesivesperc.Text)).ToString("0.00");
                        }
                        else
                        {
                            lblTotalprofit.Text = "0.00";

                        }

                        /* End   total profit/loss*/

                    }


                    #endregion  End  Query For Sub client Belongs The Specific Client   As on [10-09-2013]

                    else
                    {
                        #region Begin    Old Code Befor  on  [10-09-2013]
                        sqlclientdata = "  select   SUM(isnull(ep.pf,0)) as emppf,SUM(isnull(ep.PFEmpr,0)) as pfempr,  (SUM(isnull(ep.pf,0))+SUM(isnull(ep.PFEmpr,0))) as totalpf," +
                                        " SUM(isnull(ep.esi,0)) as eesi,SUM(isnull(ep.esiempr,0)) as empresi, (SUM(isnull(ep.esi,0))+SUM(isnull(ep.esiempr,0))) as totalesi,  " +
                                        "  ( sum(isnull(ep.Gross,0)) +  sum(isnull(ep.Otamt,0)) +   sum(isnull(ep.NHSAMT,0))+ sum(isnull(ep.Npotsamt,0)) ) as empgross," +
                                        " sum(isnull(ep.ProfTax,0)) emproftax ,(SUM(isnull(ep.PFEmpr,0))+sum(isnull(ep.Gross,0)) +  sum(isnull(ep.Otamt,0))+ sum(isnull(ep.NHSAMT,0))+sum(isnull(ep.Npotsamt,0))+SUM(isnull(ep.esiempr,0))) as totalexpenses   " +
                                        "   from EmpPaySheet as ep  where ep.ClientId='" + lblclientid.Text + "'   and MONTH='" + month + Year.Substring(2, 2) + "'";

                        DataTable DtPaysheet = config.ExecuteAdaptorAsyncWithQueryParams(sqlclientdata).Result;

                        if (DtPaysheet.Rows.Count > 0)
                        {
                            if (String.IsNullOrEmpty(DtPaysheet.Rows[0]["empgross"].ToString()) == false)
                            {
                                lblgross.Text = float.Parse(DtPaysheet.Rows[0]["empgross"].ToString()).ToString("0.00");
                                lblpfemployer.Text = float.Parse(DtPaysheet.Rows[0]["pfempr"].ToString()).ToString("0.00");
                                lblesiemployer.Text = float.Parse(DtPaysheet.Rows[0]["empresi"].ToString()).ToString("0.00");
                                lblTotalexpensives.Text = float.Parse(DtPaysheet.Rows[0]["totalexpenses"].ToString()).ToString("0.00");
                                /* Begin   total profit/loss*/
                                lblTotalprofitorloss.Text = (float.Parse(lblNet.Text) - float.Parse(lblTotalexpensives.Text)).ToString("0.00");
                                if (float.Parse(lblNet.Text) > 0)
                                {
                                    lblTotalexpesivesperc.Text = (((float.Parse(lblTotalexpensives.Text) / float.Parse(lblNet.Text)) * 100).ToString("0.00"));
                                }
                                else
                                {
                                    lblTotalexpesivesperc.Text = "100.00";
                                }

                                lblTotalprofitorloss.Text = (float.Parse(lblNet.Text) - float.Parse(lblTotalexpensives.Text)).ToString("0.00");
                                if (float.Parse(lblNet.Text) > 0)
                                {

                                    lblTotalprofit.Text = (100 - float.Parse(lblTotalexpesivesperc.Text)).ToString("0.00");
                                }
                                else
                                {
                                    lblTotalprofit.Text = "0.00";
                                }

                                /* End   total profit/loss*/

                            }
                            // lblTotal.Text = float.Parse(DtPaysheet.Rows[0]["Total"].ToString()).ToString("0.00");
                            float NetTotal = 0; //float.Parse(DtPaysheet.Rows[0]["NetTotal"].ToString());
                            float Perc = 0;
                            // lblPerc.Text = "0.00";
                            if (NetTotal > 0)
                            {

                                Perc = (float.Parse(DtPaysheet.Rows[0]["Total"].ToString()) / NetTotal) * 100;
                                // lblPerc.Text = Perc.ToString("0.00");
                            }
                        }

                        #endregion     End    Old Code Befor  on  [10-09-2013]
                    }
                }
            }
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("ProfitLoss.xls", this.gvreport);
        }

    }
}