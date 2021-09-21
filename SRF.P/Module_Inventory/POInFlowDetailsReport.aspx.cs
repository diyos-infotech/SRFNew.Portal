using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using SRF.P.DAL;

namespace SRF.P.Module_Inventory
{
    public partial class POInFlowDetailsReport : System.Web.UI.Page
    {
        DataTable dt;
        string GRVPrefix = "";
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";

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
            if (Session.Keys.Count > 0)
            {
                EmpIDPrefix = Session["EmpIDPrefix"].ToString();
                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
                GRVPrefix = Session["GRVPrefix"].ToString();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("PODetailsReport.xls", this.GVPOInFlowDetails);
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {

            GVPOInFlowDetails.DataSource = null;
            GVPOInFlowDetails.DataBind();
            lbtn_Export.Visible = false;

            if (txtFromDate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select From Date');", true);
                return;
            }

            if (txtFromDate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select From Date');", true);
                return;
            }

            string date = string.Empty;
            string Month = string.Empty;
            string clientid = string.Empty;
            string FromDate = "";
            string ToDate = "";

            if (txtFromDate.Text.Trim().Length > 0)
            {
                FromDate = DateTime.Parse(txtFromDate.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            if (txtToDate.Text.Trim().Length > 0)
            {
                ToDate = DateTime.Parse(txtToDate.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string spname = "";
            DataTable dtIOM = null;
            Hashtable HashtableIOM = new Hashtable();

            spname = "POInFlowDetailsReport";

            HashtableIOM.Add("@fromdate", FromDate);
            HashtableIOM.Add("@todate", ToDate);


            dtIOM = config.ExecuteAdaptorAsyncWithParams(spname, HashtableIOM).Result;
            if (dtIOM.Rows.Count > 0)
            {
                lbtn_Export.Visible = true;
                GVPOInFlowDetails.DataSource = dtIOM;
                GVPOInFlowDetails.DataBind();
            }
            else
            {
                lbtn_Export.Visible = false;
                GVPOInFlowDetails.DataSource = null;
                GVPOInFlowDetails.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('There Is No Details For This Client');", true);
            }
        }

        protected void ClearData()
        {

            GVPOInFlowDetails.DataSource = null;
            GVPOInFlowDetails.DataBind();
        }

        public string GetMonthName()
        {
            string monthname = string.Empty;
            int payMonth = 0;
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();
            return monthname;
        }

        float totalQty = 0;
        float totalBuyingpriceUnit = 0;
        float totalBuyingprice = 0;
        float totalVat5Per = 0;
        float totalVat14Per = 0;
        float totalGrandTotal = 0;
        float totalinflowamt = 0;
        float totalvat5 = 0;
        float totalvat14 = 0;
        float totalinflowGrandTotal = 0;
        float totalPOVATCmp1 = 0;
        float totalPOVATCmp2 = 0;
        float totalPOVATCmp3 = 0;
        float totalPOVATCmp4 = 0;
        float totalPOVATCmp5 = 0;
        float totalINVATCmp1 = 0;
        float totalINVATCmp2 = 0;
        float totalINVATCmp3 = 0;
        float totalINVATCmp4 = 0;
        float totalINVATCmp5 = 0;

        protected void GVPOInFlowDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.Footer)
            {

                DataTable dt = GlobalData.Instance.LoadTaxComponents();
                if (dt.Rows.Count > 0)
                {
                    //VATcmp1
                    if (dt.Rows[10]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[15].Visible = true;

                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[6].Text = dt.Rows[10]["TaxCmpName"].ToString();
                            e.Row.Cells[15].Text = dt.Rows[10]["TaxCmpName"].ToString();

                        }
                    }
                    else
                    {
                        e.Row.Cells[6].Visible = false;
                        e.Row.Cells[15].Visible = false;

                    }

                    //VATcmp2
                    if (dt.Rows[11]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[16].Visible = true;

                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[7].Text = dt.Rows[11]["TaxCmpName"].ToString();
                            e.Row.Cells[16].Text = dt.Rows[11]["TaxCmpName"].ToString();
                        }
                    }
                    else
                    {
                        e.Row.Cells[7].Visible = false;
                        e.Row.Cells[16].Visible = false;

                    }


                    //VATcmp3
                    if (dt.Rows[12]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[17].Visible = true;

                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[8].Text = dt.Rows[12]["TaxCmpName"].ToString();
                            e.Row.Cells[17].Text = dt.Rows[12]["TaxCmpName"].ToString();

                        }
                    }
                    else
                    {
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[17].Visible = false;

                    }

                    //VATcmp4
                    if (dt.Rows[13]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[9].Visible = true;
                        e.Row.Cells[18].Visible = true;

                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[9].Text = dt.Rows[13]["TaxCmpName"].ToString();
                            e.Row.Cells[18].Text = dt.Rows[13]["TaxCmpName"].ToString();

                        }
                    }
                    else
                    {
                        e.Row.Cells[9].Visible = false;
                        e.Row.Cells[18].Visible = false;

                    }


                    //VATcmp5
                    if (dt.Rows[14]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[10].Visible = true;
                        e.Row.Cells[19].Visible = true;

                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[10].Text = dt.Rows[14]["TaxCmpName"].ToString();
                            e.Row.Cells[19].Text = dt.Rows[14]["TaxCmpName"].ToString();

                        }
                    }
                    else
                    {
                        e.Row.Cells[10].Visible = false;
                        e.Row.Cells[19].Visible = false;

                    }

                }
            }


            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                float lblbuyingpriceunit = float.Parse(((Label)e.Row.FindControl("lbltotalprice")).Text);
                totalBuyingpriceUnit += lblbuyingpriceunit;
                float lblPOVATCmp1 = float.Parse(((Label)e.Row.FindControl("lblPOVATCmp1")).Text);
                totalPOVATCmp1 += lblPOVATCmp1;
                float lblPOVATCmp2 = float.Parse(((Label)e.Row.FindControl("lblPOVATCmp2")).Text);
                totalPOVATCmp2 += lblPOVATCmp1;
                float lblPOVATCmp3 = float.Parse(((Label)e.Row.FindControl("lblPOVATCmp3")).Text);
                totalPOVATCmp3 += lblPOVATCmp1;
                float lblPOVATCmp4 = float.Parse(((Label)e.Row.FindControl("lblPOVATCmp4")).Text);
                totalPOVATCmp4 += lblPOVATCmp1;
                float lblPOVATCmp5 = float.Parse(((Label)e.Row.FindControl("lblPOVATCmp5")).Text);
                totalPOVATCmp5 += lblPOVATCmp1;
                float lblINVATCmp1 = float.Parse(((Label)e.Row.FindControl("lblINVATCmp1")).Text);
                totalINVATCmp1 += lblINVATCmp1;
                float lblINVATCmp2 = float.Parse(((Label)e.Row.FindControl("lblINVATCmp2")).Text);
                totalINVATCmp2 += lblINVATCmp2;
                float lblINVATCmp3 = float.Parse(((Label)e.Row.FindControl("lblINVATCmp3")).Text);
                totalINVATCmp3 += lblINVATCmp3;
                float lblINVATCmp4 = float.Parse(((Label)e.Row.FindControl("lblINVATCmp4")).Text);
                totalINVATCmp4 += lblINVATCmp4;
                float lblINVATCmp5 = float.Parse(((Label)e.Row.FindControl("lblINVATCmp5")).Text);
                totalINVATCmp5 += lblINVATCmp5;
                float lbltotal = float.Parse(((Label)e.Row.FindControl("lbltotal")).Text);
                totalGrandTotal += lbltotal;
                float lbltotalAmt = float.Parse(((Label)e.Row.FindControl("lbltotalAmt")).Text);
                totalinflowamt += lbltotalAmt;
                float lblInflowtotal = float.Parse(((Label)e.Row.FindControl("lblInflowtotal")).Text);
                totalinflowGrandTotal += lblInflowtotal;
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((Label)e.Row.FindControl("lblTotalBuyingPrice")).Text = totalBuyingpriceUnit.ToString();
                ((Label)e.Row.FindControl("lblPOTotalVATCmp1")).Text = totalPOVATCmp1.ToString();
                ((Label)e.Row.FindControl("lblPOTotalVATCmp2")).Text = totalPOVATCmp2.ToString();
                ((Label)e.Row.FindControl("lblPOTotalVATCmp3")).Text = totalPOVATCmp3.ToString();
                ((Label)e.Row.FindControl("lblPOTotalVATCmp4")).Text = totalPOVATCmp4.ToString();
                ((Label)e.Row.FindControl("lblPOTotalVATCmp5")).Text = totalPOVATCmp5.ToString();
                ((Label)e.Row.FindControl("lblINTotalVATCmp1")).Text = totalINVATCmp1.ToString();
                ((Label)e.Row.FindControl("lblINTotalVATCmp2")).Text = totalINVATCmp2.ToString();
                ((Label)e.Row.FindControl("lblINTotalVATCmp3")).Text = totalINVATCmp3.ToString();
                ((Label)e.Row.FindControl("lblINTotalVATCmp4")).Text = totalINVATCmp4.ToString();
                ((Label)e.Row.FindControl("lblINTotalVATCmp5")).Text = totalINVATCmp5.ToString();
                ((Label)e.Row.FindControl("lblTotaamount")).Text = totalGrandTotal.ToString();
                ((Label)e.Row.FindControl("lblInFlowTotalBuyingPrice")).Text = totalinflowamt.ToString();
                ((Label)e.Row.FindControl("lblInflowTotalamount")).Text = totalinflowGrandTotal.ToString();


            }
        }



    }
}