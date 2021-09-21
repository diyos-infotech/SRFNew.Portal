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
    public partial class POReport : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();

        DataTable dt;
        string GRVPrefix = "";
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
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

                GetPONos();
                GRVIDAuto();


            }

        }


        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            GRVPrefix = Session["GRVPrefix"].ToString();

        }

        protected void GetPONos()
        {
            string sqlqry = "Select distinct pono from InvpoMaster  ";
            DataTable dt = config.ExecuteReaderWithQueryAsync(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlPONo.DataTextField = "pono";
                ddlPONo.DataValueField = "pono";
                ddlPONo.DataSource = dt;
                ddlPONo.DataBind();

            }

            ddlPONo.Items.Insert(0, "-Select-");

        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("POReport.xls", this.GVPODetails);
        }

        public void GRVIDAuto()
        {


            int GRVID = 1;
            string selectqueryclientid = "select (max(right(InflowID,4))) as GRVID from InvInflowMaster  where InflowID like '" + GRVPrefix + "%'";
            dt = config.ExecuteReaderWithQueryAsync(selectqueryclientid).Result;
            string invPrefix = string.Empty;

            if (dt.Rows.Count > 0)
            {

                if (String.IsNullOrEmpty(dt.Rows[0]["GRVID"].ToString()) == false)
                {
                    GRVID = Convert.ToInt32(dt.Rows[0]["GRVID"].ToString()) + 1;
                }
                else
                {
                    GRVID = int.Parse("1");
                }
            }






        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {

            GVPODetails.DataSource = null;
            GVPODetails.DataBind();
            lbtn_Export.Visible = true;


            string spname = "";
            DataTable dtIOM = null;
            Hashtable HashtableIOM = new Hashtable();
            string PONo = "";
            string date = "";

            spname = "POPrint";
            PONo = ddlPONo.SelectedValue;


            //string month = DateTime.Parse(date).Month.ToString("00");
            //string Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);

            HashtableIOM.Add("@PONo", PONo);


            dtIOM = config.ExecuteAdaptorAsyncWithParams(spname, HashtableIOM).Result;
            if (dtIOM.Rows.Count > 0)
            {
                GVPODetails.DataSource = dtIOM;
                GVPODetails.DataBind();
            }
            else
            {
                GVPODetails.DataSource = null;
                GVPODetails.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('There Is No Details For This Client');", true);

            }

        }

        protected void ClearData()
        {

            GVPODetails.DataSource = null;
            GVPODetails.DataBind();
        }

        public string GetMonthName()
        {
            string monthname = string.Empty;
            int payMonth = 0;
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();


            //DateTime date = Convert.ToDateTime(ddlPONo.SelectedValue, CultureInfo.GetCultureInfo("en-gb"));
            //monthname = mfi.GetMonthName(date.Month).ToString();
            //payMonth = GetMonth(monthname);

            return monthname;
        }

        float totalQty = 0;
        float totalBuyingpriceUnit = 0;
        float totalBuyingprice = 0;
        float totalVat5Per = 0;
        float totalVat14Per = 0;
        float totalGrandTotal = 0;
        float totalPOVATCmp1 = 0;
        float totalPOVATCmp2 = 0;
        float totalPOVATCmp3 = 0;
        float totalPOVATCmp4 = 0;
        float totalPOVATCmp5 = 0;

        protected void GVPODetails_RowDataBound(object sender, GridViewRowEventArgs e)
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

                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[6].Text = dt.Rows[10]["TaxCmpName"].ToString();

                        }
                    }
                    else
                    {
                        e.Row.Cells[6].Visible = false;

                    }

                    //VATcmp2
                    if (dt.Rows[11]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[7].Visible = true;

                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[7].Text = dt.Rows[11]["TaxCmpName"].ToString();
                        }
                    }
                    else
                    {
                        e.Row.Cells[7].Visible = false;

                    }


                    //VATcmp3
                    if (dt.Rows[12]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[8].Visible = true;

                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[8].Text = dt.Rows[12]["TaxCmpName"].ToString();

                        }
                    }
                    else
                    {
                        e.Row.Cells[8].Visible = false;

                    }

                    //VATcmp4
                    if (dt.Rows[13]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[9].Visible = true;

                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[9].Text = dt.Rows[13]["TaxCmpName"].ToString();

                        }
                    }
                    else
                    {
                        e.Row.Cells[9].Visible = false;

                    }


                    //VATcmp5
                    if (dt.Rows[14]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[10].Visible = true;

                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[10].Text = dt.Rows[14]["TaxCmpName"].ToString();

                        }
                    }
                    else
                    {
                        e.Row.Cells[10].Visible = false;

                    }

                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                float lblQty = float.Parse(((Label)e.Row.FindControl("lblQty")).Text);
                totalQty += lblQty;

                float lblbuyingpriceunit = float.Parse(((Label)e.Row.FindControl("lblbuyingpriceunit")).Text);
                totalBuyingpriceUnit += lblbuyingpriceunit;

                if (((Label)e.Row.FindControl("lbltotalprice")).Text == "")
                {
                    ((Label)e.Row.FindControl("lbltotalprice")).Text = "0";
                }

                float lbltotalprice = float.Parse(((Label)e.Row.FindControl("lbltotalprice")).Text);
                totalBuyingprice += lbltotalprice;

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

                float lbltotal = float.Parse(((Label)e.Row.FindControl("lbltotal")).Text);
                totalGrandTotal += lbltotal;
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((Label)e.Row.FindControl("lblTotalQty")).Text = totalQty.ToString();
                ((Label)e.Row.FindControl("lblTotalBuyingPriceunit")).Text = totalBuyingpriceUnit.ToString();
                ((Label)e.Row.FindControl("lblTotalBuyingPrice")).Text = totalBuyingprice.ToString();
                ((Label)e.Row.FindControl("lblPOTotalVATCmp1")).Text = totalPOVATCmp1.ToString();
                ((Label)e.Row.FindControl("lblPOTotalVATCmp2")).Text = totalPOVATCmp2.ToString();
                ((Label)e.Row.FindControl("lblPOTotalVATCmp3")).Text = totalPOVATCmp3.ToString();
                ((Label)e.Row.FindControl("lblPOTotalVATCmp4")).Text = totalPOVATCmp4.ToString();
                ((Label)e.Row.FindControl("lblPOTotalVATCmp5")).Text = totalPOVATCmp5.ToString();
                ((Label)e.Row.FindControl("lblTotaamount")).Text = totalGrandTotal.ToString();

            }
        }

    }
}