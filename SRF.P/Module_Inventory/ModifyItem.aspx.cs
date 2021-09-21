﻿using System;
using System.Data;
using KLTS.Data;
using SRF.P.DAL;

namespace SRF.P.Module_Inventory
{
    public partial class ModifyItem : System.Web.UI.Page
    {
        DataTable dt;
        string InvIDPrefix = "";
        string Username = "";
        AppConfiguration config = new AppConfiguration();

        protected void Page_Load(object sender, EventArgs e)
        {
            InvIDPrefix = Session["InvPrefix"].ToString();
            Username = Session["UserId"].ToString();

            if (!IsPostBack)
            {


                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {
                    
                }
                else
                {
                    Response.Redirect("login.aspx");
                }
                loadItemIDs();
                loaditemNames();
                Measuredunits();
                VATDisplay();
                ddlmesure.Items.Insert(0, "--Select--");



            }

        }

        protected void Measuredunits()
        {
            string Sqlqry = "Select * from units";
            DataTable dt = config.ExecuteReaderWithQueryAsync(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlmesure.DataSource = dt;
                ddlmesure.DataValueField = "Unitmeasure";
                ddlmesure.DataTextField = "unitmeasure";
                ddlmesure.DataBind();
            }

        }

        public void loadItemIDs()
        {
            string sqlqry = "Select itemId  from InvStockItemList";
            DataTable dt = config.ExecuteReaderWithQueryAsync(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlItemID.DataValueField = "itemId";
                ddlItemID.DataTextField = "itemId";
                ddlItemID.DataSource = dt;
                ddlItemID.DataBind();
            }

            ddlItemID.Items.Insert(0, "-Select-");
        }

        public void loaditemNames()
        {
            string sqlqry = "Select itemId,itemname  from InvStockItemList";
            DataTable dt = config.ExecuteReaderWithQueryAsync(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlitemname.DataValueField = "itemId";
                ddlitemname.DataTextField = "itemname";
                ddlitemname.DataSource = dt;
                ddlitemname.DataBind();
            }

            ddlitemname.Items.Insert(0, "-Select-");
        }

        protected void GetItemName()
        {
            string sqlqry = "Select itemid from InvStockItemList where itemid = '" + ddlItemID.SelectedValue + "'  ";
            DataTable dt = config.ExecuteReaderWithQueryAsync(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    ddlitemname.SelectedValue = dt.Rows[0]["itemid"].ToString();
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                ddlitemname.SelectedIndex = 0;
            }
        }

        protected void GetItemID()
        {

            #region  Old Code
            string sqlqry = "Select itemId,itemname  from InvStockItemList where Itemid =  '" + ddlitemname.SelectedValue + "'  ";

            DataTable dt = config.ExecuteReaderWithQueryAsync(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    ddlItemID.SelectedValue = dt.Rows[0]["itemId"].ToString();

                }
                catch (Exception ex)
                {
                    // MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                ddlItemID.SelectedIndex = 0;
            }

            #endregion
        }

        protected void ddlItemID_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblresult.Visible = false;
            GetItemName();
            GetData();

        }

        protected void ddlitemname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblresult.Visible = false;
            GetItemID();
            GetData();

        }

        public void GetData()
        {
            string qry = "select itemid,itemname,UnitMeasure,Brand, MinimumQty,BuyingPrice,Sellingprice,Category,cast(VATCmp1 as bit) as VATCmp1,cast(VATCmp2 as bit) as VATCmp2,cast(VATCmp3 as bit) as VATCmp3,cast(VATCmp4 as bit) as VATCmp4,cast(VATCmp5 as bit) as VATCmp5 from InvStockItemList where itemid='" + ddlItemID.SelectedValue + "'";
            DataTable dt = config.ExecuteReaderWithQueryAsync(qry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlmesure.SelectedValue = dt.Rows[0]["UnitMeasure"].ToString();
                txtbrand.Text = dt.Rows[0]["Brand"].ToString();
                txtprice.Text = dt.Rows[0]["BuyingPrice"].ToString();
                txtmq.Text = dt.Rows[0]["MinimumQty"].ToString();
                ddlCategory.SelectedValue = dt.Rows[0]["Category"].ToString();
                txtsellingprice.Text = dt.Rows[0]["Sellingprice"].ToString();
                ChkVATCmp1.Checked = bool.Parse(dt.Rows[0]["VATCmp1"].ToString());
                ChkVATCmp2.Checked = bool.Parse(dt.Rows[0]["VATCmp2"].ToString());
                ChkVATCmp3.Checked = bool.Parse(dt.Rows[0]["VATCmp3"].ToString());
                ChkVATCmp4.Checked = bool.Parse(dt.Rows[0]["VATCmp4"].ToString());
                ChkVATCmp5.Checked = bool.Parse(dt.Rows[0]["VATCmp5"].ToString());

            }
        }


        public void VATDisplay()
        {
            string qry = "select TaxCmpID,TaxCmpName,Visibility from taxcomponentsmaster";
            DataTable dt = config.ExecuteReaderWithQueryAsync(qry).Result;

            if (dt.Rows.Count > 0)
            {
                //VATcmp1
                if (dt.Rows[10]["Visibility"].ToString() == "Y")
                {
                    ChkVATCmp1.Visible = true;
                    ChkVATCmp1.Text = " " + dt.Rows[10]["TaxCmpName"].ToString();
                }
                else
                {
                    ChkVATCmp1.Visible = false;
                }

                //VATcmp2
                if (dt.Rows[11]["Visibility"].ToString() == "Y")
                {
                    ChkVATCmp2.Visible = true;
                    ChkVATCmp2.Text = " " + dt.Rows[11]["TaxCmpName"].ToString();
                }
                else
                {
                    ChkVATCmp2.Visible = false;
                }


                //VATcmp3
                if (dt.Rows[12]["Visibility"].ToString() == "Y")
                {
                    ChkVATCmp3.Visible = true;
                    ChkVATCmp3.Text = " " + dt.Rows[12]["TaxCmpName"].ToString();
                }
                else
                {
                    ChkVATCmp3.Visible = false;
                }

                //VATcmp4
                if (dt.Rows[13]["Visibility"].ToString() == "Y")
                {
                    ChkVATCmp4.Visible = true;
                    ChkVATCmp4.Text = " " + dt.Rows[13]["TaxCmpName"].ToString();
                }
                else
                {
                    ChkVATCmp4.Visible = false;
                }

                //VATcmp5
                if (dt.Rows[14]["Visibility"].ToString() == "Y")
                {
                    ChkVATCmp5.Visible = true;
                    ChkVATCmp5.Text = " " + dt.Rows[14]["TaxCmpName"].ToString();
                }
                else
                {
                    ChkVATCmp5.Visible = false;
                }

            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            var VATCmp1 = 0;
            var VATCmp2 = 0;
            var VATCmp3 = 0;
            var VATCmp4 = 0;
            var VATCmp5 = 0;

            if (ChkVATCmp1.Checked == true)
            {
                VATCmp1 = 1;
            }
            if (ChkVATCmp2.Checked == true)
            {
                VATCmp2 = 1;
            }
            if (ChkVATCmp3.Checked == true)
            {
                VATCmp3 = 1;
            }
            if (ChkVATCmp4.Checked == true)
            {
                VATCmp4 = 1;
            }
            if (ChkVATCmp5.Checked == true)
            {
                VATCmp5 = 1;
            }

            string UpdateQuery = " update InvStockItemList set UnitMeasure='" + ddlmesure.SelectedValue + "',Brand='" + txtbrand.Text + "',MinimumQty='" + txtmq.Text + "',BuyingPrice='" + txtprice.Text + "',Sellingprice='" + txtsellingprice.Text + "',Category='" + ddlCategory.SelectedValue + "',VATCmp1='" + VATCmp1 + "',VATCmp2='" + VATCmp2 + "',VATCmp3='" + VATCmp3 + "',VATCmp4='" + VATCmp4 + "',VATCmp5='" + VATCmp5 + "'  where itemid = '" + ddlItemID.SelectedValue + "'";
            int status = config.ExecuteNonQueryWithQueryAsync(UpdateQuery).Result;

            if (status > 0)
            {
                lblresult.Visible = true;
                lblresult.Text = "Record Modified Successfully";
            }
            else
            {
                lblresult.Visible = false;

            }

            txtbrand.Text = txtmq.Text = txtprice.Text = txtsellingprice.Text = string.Empty;
            ddlItemID.SelectedIndex = ddlitemname.SelectedIndex = ddlmesure.SelectedIndex = ddlCategory.SelectedIndex = 0;

            ChkVATCmp1.Checked = ChkVATCmp2.Checked = ChkVATCmp3.Checked = ChkVATCmp4.Checked = false;



        }
    }
}