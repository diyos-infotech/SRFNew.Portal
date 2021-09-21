using System;
using System.Data;
using System.Web.UI.WebControls;
using SRF.P.DAL;

namespace SRF.P.Module_Settings
{
    public partial class SalaryBreakup : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {
                       
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }

                    displaydata();
                }
            }
            catch (Exception ex)
            {
                displaydata();
                lblresult.Visible = true;
                lblresult.Text = "Incorrect data";
            }
        }


        private void displaydata()
        {
            string selectquery = "select DesignId,Design from designations";
            DataTable dtable = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;
            DataRow row = dtable.NewRow();
            row["DesignId"] = "-1";
            row["Design"] = "--Select--";
            dtable.Rows.InsertAt(row, 0);
            ddlDesignations.DataSource = dtable;
            ddlDesignations.DataTextField = "Design";
            ddlDesignations.DataValueField = "DesignId";
            ddlDesignations.DataBind();

            DataBind();
        }

        protected void DataBind()
        {
            string strQry = "Select * from Designations";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
            gvSalaryBreakup.DataSource = dt;
            gvSalaryBreakup.DataBind();
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            lblresult.Text = "";
            if (ddlDesignations.SelectedIndex > 0)
            {
                lblresult.Visible = true;
                try
                {
                    float basic = 0;
                    float da = 0;
                    float hra = 0;
                    float cca = 0;
                    float washAllo = 0;
                    float otherAllo = 0;
                    float conceyance = 0;
                    float bonus = 0;
                    float ctc = 0;

                    if (txtBasic.Text.Trim().Length > 0)
                        basic = Convert.ToSingle(txtBasic.Text.Trim());
                    if (txtDA.Text.Trim().Length > 0)
                        da = Convert.ToSingle(txtDA.Text.Trim());
                    if (txtHRA.Text.Trim().Length > 0)
                        hra = Convert.ToSingle(txtHRA.Text.Trim());
                    if (txtCCA.Text.Trim().Length > 0)
                        cca = Convert.ToSingle(txtCCA.Text.Trim());
                    if (txtWashAllo.Text.Trim().Length > 0)
                        washAllo = Convert.ToSingle(txtWashAllo.Text.Trim());
                    if (txtOtherAllo.Text.Trim().Length > 0)
                        otherAllo = Convert.ToSingle(txtOtherAllo.Text.Trim());
                    if (txtConceyance.Text.Trim().Length > 0)
                        conceyance = Convert.ToSingle(txtConceyance.Text.Trim());
                    if (txtBonus.Text.Trim().Length > 0)
                        bonus = Convert.ToSingle(txtBonus.Text.Trim());
                    if (txtCTC.Text.Trim().Length > 0)
                        ctc = Convert.ToSingle(txtCTC.Text.Trim());

                    string strQty = "Update Designations set Basic = " + basic + ",DA=" + da +
                        ",HRA=" + hra + ",CCA=" + cca + ",WashAllowance=" + washAllo + ",OtherAllowance=" + otherAllo +
                        ",Conveyance=" + conceyance + ",Bonus=" + bonus + ",CTC=" + ctc + " where DesignId = '" + ddlDesignations.SelectedValue + "'";
                    int status = config.ExecuteNonQueryWithQueryAsync(strQty).Result;
                    if (status == 1)
                    {
                        lblresult.Visible = true;
                        lblresult.Text = "Salary Breakup Updated Successfully";
                    }
                }
                catch (Exception ex)
                {
                    lblresult.Visible = true;
                    lblresult.Text = "Invalid Data";
                }
            }
            displaydata();
            FillZeros();

        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            //txtcpwd.Text = txtpwd.Text = txtusername.Text = string.Empty;
        }

        protected void gvcreatelogin_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void FillZeros()
        {
            txtBasic.Text = "0";
            txtDA.Text = "0";
            txtHRA.Text = "0";
            txtCCA.Text = "0";
            txtWashAllo.Text = "0";
            txtOtherAllo.Text = "0";
            txtConceyance.Text = "0";
            txtBonus.Text = "0";
            txtCTC.Text = "0";
        }

        protected void ddlDesignations_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblresult.Text = "";
            if (ddlDesignations.SelectedIndex > 0)
            {
                string sqlQry = "Select Basic,DA,HRA,CCA,WashAllowance,OtherAllowance,Conveyance,Bonus,CTC from Designations where DesignId='" + ddlDesignations.SelectedValue + "'";
                DataTable dtable = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;
                if (dtable.Rows.Count > 0)
                {
                    string basic = dtable.Rows[0]["Basic"].ToString();
                    float iBasic = 0;
                    if (basic.Length > 0)
                        iBasic = Convert.ToSingle(basic);
                    if (iBasic <= 0)
                    {
                        FillZeros();
                    }
                    else
                    {
                        try
                        {
                            txtBasic.Text = iBasic.ToString();
                            if (dtable.Rows[0]["DA"].ToString().Length > 0)
                                txtDA.Text = Convert.ToSingle(dtable.Rows[0]["DA"].ToString()).ToString();
                            if (dtable.Rows[0]["HRA"].ToString().Length > 0)
                                txtHRA.Text = Convert.ToSingle(dtable.Rows[0]["HRA"].ToString()).ToString();
                            if (dtable.Rows[0]["CCA"].ToString().Length > 0)
                                txtCCA.Text = Convert.ToSingle(dtable.Rows[0]["CCA"].ToString()).ToString();
                            if (dtable.Rows[0]["WashAllowance"].ToString().Length > 0)
                                txtWashAllo.Text = Convert.ToSingle(dtable.Rows[0]["WashAllowance"].ToString()).ToString();
                            if (dtable.Rows[0]["OtherAllowance"].ToString().Length > 0)
                                txtOtherAllo.Text = Convert.ToSingle(dtable.Rows[0]["OtherAllowance"].ToString()).ToString();
                            if (dtable.Rows[0]["Conveyance"].ToString().Length > 0)
                                txtConceyance.Text = Convert.ToSingle(dtable.Rows[0]["Conveyance"].ToString()).ToString();
                            if (dtable.Rows[0]["Bonus"].ToString().Length > 0)
                                txtBonus.Text = Convert.ToSingle(dtable.Rows[0]["Bonus"].ToString()).ToString();
                            if (dtable.Rows[0]["CTC"].ToString().Length > 0)
                                txtCTC.Text = Convert.ToSingle(dtable.Rows[0]["CTC"].ToString()).ToString();
                        }
                        catch (Exception ex)
                        {
                            lblresult.Visible = true;
                            lblresult.Text = ex.Message.ToString();
                        }
                    }
                }
                else
                {
                    FillZeros();
                }
            }
        }
        protected void gvSalaryBreakup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSalaryBreakup.PageIndex = e.NewPageIndex;
            DataBind();
        }
    }
}