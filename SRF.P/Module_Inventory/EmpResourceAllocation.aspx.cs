using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using KLTS.Data;
using System.Globalization;
using System.Collections.Generic;
using SRF.P.DAL;

namespace SRF.P.Module_Inventory
{
    public partial class EmpResourceAllocation : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Fontstyle = "";
        string CFontstyle = "";
        string Created_By = "";

        AppConfiguration config = new AppConfiguration();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                GetWebConfigdata();
                if (!IsPostBack)
                {
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {
                       

                        loanidauto();
                        Uniformidauto();
                        txtresourceissue.Text = DateTime.Now.ToString("dd/MM/yyyy");



                    }
                    else
                    {
                        Response.Redirect("Login.aspx");
                    }


                    LoadItemsbyUniformCategory();
                    LoadSupervisorIDs();
                }
            }
            catch (Exception ex)
            {

            }
        }

      

        public void GoToLoginPage()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
            Response.Redirect("~/login.aspx");
        }

        public void LoadItemsbyGeneralCategory()
        {
            gvresources.DataSource = null;
            gvresources.DataBind();

            string qry = "select R.ResourceID,SI.ItemName,R.Price,SI.ActualQuantity,1 as Qty,'' as Balance from Resources R inner join invStockItemList SI on R.ResourceID=SI.ItemId where (category='General') order by category,SI.ItemName";
            DataTable dt = config.ExecuteReaderWithQueryAsync(qry).Result;
            if (dt.Rows.Count > 0)
            {
                gvresources.DataSource = dt;
                gvresources.DataBind();

                for (int i = 0; i < gvresources.Rows.Count; i++)
                {
                    CheckBox chkempid = gvresources.Rows[i].FindControl("CbChecked") as CheckBox;
                    chkempid.Checked = true;

                    if (rdbFromSupervisor.Checked == true)
                    {
                        gvresources.Columns[3].Visible = true;
                    }
                    else
                    {
                        gvresources.Columns[3].Visible = false;

                    }

                }

                GetTotal();
            }
        }

        protected void GetWebConfigdata()
        {

            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            Created_By = Session["UserID"].ToString();
        }

        protected void rdbToSupervisor_CheckedChanged(object sender, EventArgs e)
        {

            ddlSupervisorID.SelectedIndex = 0;
            txtEmpid.Text = "";
            txtName.Text = "";
            gvresources.DataSource = null;
            gvresources.DataBind();
            lblTotalamt.Visible = false;


            if (rdbIndividualWise.Checked == true)
            {
                lblSupervisorID.Visible = false;
                ddlSupervisorID.Visible = false;
                //txtSupervisorID.Text="";
                ddlUniformID.Items.Clear();
                ddlUniformID.Visible = false;
                lblSupUniformID.Visible = false;
                ChkGeneral.Enabled = true;
                ChkGeneral.Checked = true;
                LoadItemsbyUniformCategory();
            }
            else if (rdbToSupervisor.Checked)
            {
                lblSupervisorID.Visible = false;
                //txtSupervisorID.Visible = false;
                ddlSupervisorID.Visible = false;
                ddlUniformID.Items.Clear();
                ddlUniformID.Visible = false;
                lblSupUniformID.Visible = false;
                ChkGeneral.Enabled = true;
                ChkGeneral.Checked = true;
                LoadItemsbyUniformCategory();

            }
            else if (rdbFromSupervisor.Checked)
            {
                lblSupervisorID.Visible = true;
                //txtSupervisorID.Visible = true;
                ddlSupervisorID.Visible = true;
                ddlUniformID.Items.Clear();
                ddlUniformID.Visible = true;
                ChkGeneral.Enabled = true;
                ChkGeneral.Checked = false;
                lblSupUniformID.Visible = true;
                gvresources.DataSource = null;
                gvresources.DataBind();
                GVUniformGrid.DataSource = null;
                GVUniformGrid.DataBind();


            }
            GetTotal();
            uppanel.Update();
            UpTotal.Update();
            UpGv.Update();
        }

        protected void btncalculate_Click(object sender, EventArgs e)
        {
            GetTotal();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (rdbFromSupervisor.Checked)
            {
                if (ddlSupervisorID.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert()", "alert('Please select supervisor Id');", true);
                    return;
                }
            }

            #region check employee id
            if (txtEmpid.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert()", "alert('Select Employee Id');", true);
                return;
            }
            #endregion

            #region check Resource Issue

            if (txtresourceissue.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please  fill the  Resource Issue date');", true);
                return;
            }


            var resourcedate = DateTime.Now.ToString("dd/MM/yyyy");
            var testresuorceDate = 0;

            if (txtresourceissue.Text.Trim().Length > 0)
            {
                testresuorceDate = GlobalData.Instance.CheckEnteredDate(txtresourceissue.Text);
                if (testresuorceDate > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "show alert", "alert('You have Entered Invalid Resouce issue Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return;
                }
                resourcedate = DateTime.Parse(txtresourceissue.Text, CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy-MM-dd hh:mm:ss");
            }


            #endregion

            #region check loandate
            if (txtloandate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please  fill the  Cutting Month');", true);
                return;
            }


            var LoanDate = "01/01/1900";
            var testDate = 0;

            if (txtloandate.Text.Trim().Length > 0)
            {

                LoanDate = DateTime.Parse(txtloandate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy-MM-dd hh:mm:ss");
            }
            #endregion

            loanidauto();
            Uniformidauto();

            string qry = "";
            DataTable dt = null;



            foreach (GridViewRow gvr in GVUniformGrid.Rows)
            {
                CheckBox cbcheck = sender as CheckBox;
                Control ctrlone = gvr.FindControl("CbChecked") as CheckBox;
                CheckBox chkresource = (CheckBox)ctrlone;
                if (chkresource != null)
                {
                    if (chkresource.Checked)
                    {
                        #region Begin Individual Resource Details of the employee

                        float Qty = 0;
                        float StockInHand = 0;

                        TextBox tb = (TextBox)gvr.FindControl("txtresourceprice");
                        Label resourcename = (Label)gvr.FindControl("lblresourcename");
                        Label lblresourceid = (Label)gvr.FindControl("lblresourceid");
                        TextBox txtQty = (TextBox)gvr.FindControl("txtQty");
                        Label lblCategory = (Label)gvr.FindControl("lblCategory");

                        if (lblCategory.Text == "Uniform")
                        {

                            if (rdbIndividualWise.Checked == true || rdbToSupervisor.Checked == true)
                            {
                                qry = "select (isnull(openingstock,0)+ActualQuantity) as ActualQuantity from InvStockItemList where itemid='" + lblresourceid.Text + "'"; //StockItemList
                                dt = config.ExecuteReaderWithQueryAsync(qry).Result;
                            }
                            else
                            {
                                qry = "select (isnull(qty,0) -  (select ISNULL(sum(qty),0) from EmpResourceDetails erd where SupervisorID='" + ddlSupervisorID.SelectedValue + "' and SupervisorUniformID='" + ddlUniformID.SelectedValue + "' and " +
                                       " TYPE='D'   and erd.ResourceId=R.ResourceId)) as actualquantity  from EmpResourcedetails R inner join invStockItemList SI on R.ResourceID=SI.ItemId " +
                                        "where resourceid='" + lblresourceid.Text + "' and empid='" + ddlSupervisorID.SelectedValue + "' and UniformID='" + ddlUniformID.SelectedValue + "' and TYPE='S'";
                                dt = config.ExecuteReaderWithQueryAsync(qry).Result;
                            }

                            if (dt.Rows.Count > 0)
                            {
                                StockInHand = float.Parse(dt.Rows[0]["actualquantity"].ToString());
                            }

                            Qty = float.Parse(txtQty.Text);


                            if (Qty > StockInHand)
                            {
                                gvr.ForeColor = System.Drawing.Color.Red;
                                ScriptManager.RegisterStartupScript(this, GetType(), "show alert()", "alert('Resources are not allocated as quantity is exceeding stock in hand');", true);
                                return;

                            }
                            else
                            {
                                gvr.ForeColor = System.Drawing.Color.Black;
                            }
                        }

                        #endregion
                    }
                }
            }



            #region check issue mode
            int issuemode = 0;

            if (ddlFreepaid.SelectedIndex == 0)
            {
                issuemode = 0;
            }

            if (ddlFreepaid.SelectedIndex == 1)
            {
                issuemode = 1;
            }
            #endregion

            #region check no of installment depend on issuemode

            int NoofInstallments = 1;

            if (txtnoofinstallments.Text.Trim().Length > 0)
            {
                NoofInstallments = int.Parse(txtnoofinstallments.Text);

            }

            if (issuemode == 0)
            {
                if (NoofInstallments == 0 || txtnoofinstallments.Text == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert()", "alert('Please  fill No of Instalments');", true);
                    return;
                }
            }

            if (issuemode == 1)
            {
                NoofInstallments = 1;
            }


            #endregion

            int currentrowindex = 0;
            int CheckAtleastOne = 0;
            int InsertStatus = 0;

            string @TotalTransactionID = "";
            string Empid = txtEmpid.Text;
            string SqlqryForResourceAlloc = string.Empty;
            string ResourceID = string.Empty;
            string loanno = txtloanid.Text;
            string Referredby = TxtReferedBy.Text;

            string LoanIssuedDate = DateTime.Now.Date.ToString("yyyy-MM-dd hh:mm:ss");

            string Type = "";

            if (rdbIndividualWise.Checked)
            {
                Type = "I";
            }
            else if (rdbToSupervisor.Checked)
            {
                Type = "S";
            }
            else if (rdbFromSupervisor.Checked)
            {
                Type = "D";
            }
            //DateTime.Parse(DateTime.Now.Date.ToShortDateString(), CultureInfo.GetCultureInfo("en-gb")).ToString(); 
            string AllResourceNames = string.Empty;

            float amount = 0;

            double sum = 0;

            double loanamnt = 0;

            DataTable DtAddResource = null;

            Hashtable HTInserResource = new Hashtable();
            string SPName = "AddResourcesBranchwise";

            string SupervisorID = ddlSupervisorID.SelectedValue;

            if (ddlSupervisorID.SelectedIndex == 0)
            {
                SupervisorID = "";
            }

            string paidamt = "";
            string Remarks = "";

            if (Type == "S")
            {
                Remarks = "Bulk Uniform Issue";
            }
            else if (Type == "D")
            {
                Remarks = "Issue From Supervisor";
            }






            if (GVUniformGrid.Rows.Count > 0)
            {

                #region For Each for Gridview Indvidual Rows
                foreach (GridViewRow gvr in GVUniformGrid.Rows)
                {
                    CheckBox cbcheck = sender as CheckBox;
                    Control ctrlone = gvr.FindControl("CbChecked") as CheckBox;
                    CheckBox chkresource = (CheckBox)ctrlone;
                    if (chkresource != null)
                    {
                        if (chkresource.Checked)
                        {
                            #region Begin Individual Resource Details of the employee

                            int Qty = 0;
                            float TotalPrice = 0;

                            CheckAtleastOne = 1;
                            TextBox tb = (TextBox)gvr.FindControl("txtresourceprice");
                            Label resourcename = (Label)gvr.FindControl("lblresourcename");
                            Label lblresourceid = (Label)gvr.FindControl("lblresourceid");
                            TextBox txtQty = (TextBox)gvr.FindControl("txtQty");

                            Qty = int.Parse(txtQty.Text);
                            ResourceID = lblresourceid.Text;
                            amount = float.Parse(tb.Text);
                            TotalPrice = Qty * amount;
                            sum += TotalPrice;

                            #region Begin New code for Insert Resource Details as on 19/07/2014

                            HTInserResource.Clear();
                            HTInserResource.Add("@Empid", Empid);
                            HTInserResource.Add("@Resourceid", ResourceID);
                            HTInserResource.Add("@Qty", Qty);
                            //commented by swathi on 30-5-2016
                            // HTInserResource.Add("@Price", TotalPrice);
                            HTInserResource.Add("@Price", amount);
                            HTInserResource.Add("@ClientIDPrefix", CmpIDPrefix);
                            HTInserResource.Add("@TotalTransactionID", @TotalTransactionID);
                            HTInserResource.Add("@currentrowindex", currentrowindex + 1);
                            HTInserResource.Add("@LoanNo", txtloanid.Text);
                            HTInserResource.Add("@LoanType", 'N');
                            HTInserResource.Add("@UniformID", txtuniformid.Text);
                            HTInserResource.Add("@Created_By", Created_By);
                            HTInserResource.Add("@Type", Type);
                            HTInserResource.Add("@SupervisorID", SupervisorID);
                            HTInserResource.Add("@SupervisorUniformID", ddlUniformID.SelectedValue);


                            DtAddResource = config.ExecuteAdaptorAsyncWithParams(SPName, HTInserResource).Result;

                            if (DtAddResource.Rows.Count > 0)
                            {
                                if (currentrowindex == 0)
                                {
                                    @TotalTransactionID = DtAddResource.Rows[0]["transactionid"].ToString();
                                }
                            }

                            #endregion End New code for Insert Resource Details as on 19/07/2014



                            currentrowindex++;

                            #endregion  //End  Individual Resource Details of the employee
                        }
                    }
                }
                #endregion  //End For Each for Gridview Indvidual Rows


                if (issuemode == 0 && !string.IsNullOrEmpty(txtPaidAmnt.Text.Trim()) && float.Parse(txtPaidAmnt.Text.Trim()) > sum)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", "alert('Paid amount should be equal or lower than the total loan amount. Please check and enter correct amount.');", true);
                }
                else// is if paid amount is less than or equal to loan amount
                {
                    #region  //Begin  Else block for the Check Atleast One Resource

                    if (sum == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", "alert('Resources are not allocated for the selected employee please select atleast one resource');", true);
                        return;
                    }
                    else if (sum > 0)
                    {


                        if (txtPaidAmnt.Text.Length > 0)
                        {
                            paidamt = txtPaidAmnt.Text;
                        }
                        else
                        {
                            paidamt = "0";

                        }

                        if (Type == "I" || Type == "D")
                        {
                            loanamnt = issuemode == 0 ? sum - float.Parse(paidamt) : 0;
                        }
                        //var paidamt = issuemode == 0 ? float.Parse(txtPaidAmnt.Text) : 0;
                        if (issuemode == 1)
                        {
                            sum = 0;
                        }





                        SqlqryForResourceAlloc = string.Format("insert into Emploanmaster(LoanDt,EmpId,LoanAmount,NoInstalments, " +
                            "LoanStatus,TypeOfLoan,IssueMode,LoanIssuedDate,PaidAmnt,LoanType,ReferredBy) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                             LoanDate, Empid, loanamnt, NoofInstallments, "0", "1", issuemode, resourcedate, paidamt, Remarks, Referredby);
                        InsertStatus = config.ExecuteNonQueryWithQueryAsync(SqlqryForResourceAlloc).Result;

                        if (InsertStatus != 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", "alert('Resources are allocated for the selected employee.');", true);
                            lblTotalamt.Text = "Total Loan Amount Rs. : " + loanamnt;
                            ClearData();
                            //LoadResourcedetails();
                            return;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", "alert('Resources are not allocated for the selected employee.');", true);
                            return;
                        }


                        //swathi 28/05/2015 add ReferredBy,{10} after noofinstalments 


                    }

                    #endregion   //End  Else block for the Check Atleast One Resource
                }


            }//end Gridview No Of






            gvresources.PageIndex = 0;
            ClearData();

        }

        protected void txtEmpid_TextChanged(object sender, EventArgs e)
        {

            ClearData();
            lblTotalamt.Text = "";
            txtPaidAmnt.Text = "";
            ddlFreepaid.SelectedIndex = 0;

            if (txtEmpid.Text.Trim().Length > 0)
            {
                GetEmpName();
                gvresources.PageIndex = 0;

                if (rdbFromSupervisor.Checked == true)
                {
                    if (ddlUniformID.SelectedIndex > 0)
                    {
                        LoadItemsBySupervisorID();
                    }

                    //UpTotal.Update();
                    //UpGv.Update();
                    ////uppanel1.Update();
                }
            }
            else
            {
                ClearData();
            }

            //GetTotal();

        }

        protected void GetEmpName()
        {
            string Sqlqry = "select (empfname+' '+empmname+' '+emplname) as empname,EmpDesgn from empdetails where empid='" + txtEmpid.Text + "' ";
            DataTable dt = config.ExecuteReaderWithQueryAsync(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtName.Text = dt.Rows[0]["empname"].ToString();

                }
                catch (Exception ex)
                {
                    // MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                // MessageLabel.Text = "There Is No Name For The Selected Employee";
            }
        }

        protected void GetEmpid()
        {
            #region  Old Code
            string Sqlqry = "select Empid from empdetails where (empfname+' '+empmname+' '+emplname)  like '" + txtName.Text + "' ";
            DataTable dt = config.ExecuteReaderWithQueryAsync(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtEmpid.Text = dt.Rows[0]["Empid"].ToString();

                }
                catch (Exception ex)
                {
                    // MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                // MessageLabel.Text = "There Is No Name For The Selected Employee";
            }
            #endregion // End Old Code
        }

        protected void txtName_TextChanged(object sender, EventArgs e)
        {
            ClearData();
            lblTotalamt.Text = "";
            txtPaidAmnt.Text = "";
            ddlFreepaid.SelectedIndex = 0;
            if (txtName.Text.Trim().Length > 0)
            {
                GetEmpid();
                gvresources.PageIndex = 0;
                if (rdbFromSupervisor.Checked == true)
                {
                    if (ddlUniformID.SelectedIndex > 0)
                    {
                        LoadItemsBySupervisorID();
                    }

                    // UpGv.Update();
                    // uppanel1.Update();

                }
            }
            else
            {
                ClearData();
            }

            // GetTotal();


        }

        public void LoadSupervisorIDs()
        {
            string qry = "Select (empid+' - '+empfname+' '+empmname+' '+emplname) as Name,empid from empdetails where  Employeetype='S'";
            DataTable dt = config.ExecuteReaderWithQueryAsync(qry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlSupervisorID.DataTextField = "Name";
                ddlSupervisorID.DataValueField = "empid";
                ddlSupervisorID.DataSource = dt;
                ddlSupervisorID.DataBind();
            }

            ddlSupervisorID.Items.Insert(0, "Select");


        }

        protected void ddlSupervisorID_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ddlUniformID.Items.Clear();

            if (ddlSupervisorID.SelectedIndex > 0)
            {
                string qry = "select distinct(UniformID) from empresourcedetails where empid='" + ddlSupervisorID.SelectedValue + "' and type='S' ";
                DataTable dt = config.ExecuteReaderWithQueryAsync(qry).Result;
                if (dt.Rows.Count > 0)
                {
                    ddlUniformID.DataTextField = "UniformID";
                    ddlUniformID.DataValueField = "UniformID";
                    ddlUniformID.DataSource = dt;
                    ddlUniformID.DataBind();
                    ddlUniformID.Items.Insert(0, "-Select-");
                }
                else
                {
                    ddlUniformID.Items.Insert(0, "-Select-");

                }
            }
        }

        protected void ddlUniformID_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            gvresources.DataSource = null;
            gvresources.DataBind();

            if (ddlUniformID.SelectedIndex > 0)
            {
                LoadItemsBySupervisorID();
            }
            else
            {
                GVUniformGrid.DataSource = null;
                GVUniformGrid.DataBind();
            }

            //uppanel1.Update();
            UpTotal.Update();
            UpGv.Update();
        }

        protected void ClearData()
        {
            txtnoofinstallments.Text = "1";
            txtPaidAmnt.Text = string.Empty;
            txttotal.Text = string.Empty;
            TxtReferedBy.Text = "";
            loanidauto();
            Uniformidauto();

            if (GVUniformGrid.Rows.Count > 0)
            {
                for (int i = 0; i < GVUniformGrid.Rows.Count; i++)
                {
                    CheckBox chkempid = GVUniformGrid.Rows[i].FindControl("CbChecked") as CheckBox;
                    chkempid.Checked = false;

                }

            }
            ChkGeneral.Checked = false;

            //if (rdbIndividualWise.Checked || rdbToSupervisor.Checked)
            //{
            //    LoadItemsbyUniformCategory();
            //}


        }

        protected void LoadResourcedetails()
        {
            string Sqlqry = "select R.ResourceID,SI.ItemName,R.Price,SI.ActualQuantity from Resources R inner join invStockItemList SI on R.ResourceID=SI.ItemId order by SI.ItemName";
            DataTable dt = config.ExecuteReaderWithQueryAsync(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                gvresources.DataSource = dt;
                gvresources.DataBind();
            }
            else
            {
                gvresources.DataSource = null;
                gvresources.DataBind();
            }
        }

        public void LoadItemsBySupervisorID()
        {
            string Sqlqry = "select R.ResourceID,SI.ItemName,SI.category,R.Price,1 as Qty,(r.qty) as TotalPrice,qty- " +
                            " (select ISNULL(sum(qty),0) from EmpResourceDetails erd where SupervisorID='" + ddlSupervisorID.SelectedValue + "' and SupervisorUniformID='" + ddlUniformID.SelectedValue + "' and TYPE='D'  " +
                            " and erd.ResourceId=R.ResourceId) as Balance  from EmpResourcedetails R inner join invStockItemList SI on R.ResourceID=SI.ItemId  where Empid='" + ddlSupervisorID.SelectedValue + "' and uniformid='" + ddlUniformID.SelectedValue + "'  and TYPE='S' group by R.ResourceID,SI.ItemName,R.Price,r.qty,SI.category order by si.category,SI.ItemName";
            DataTable dt = config.ExecuteReaderWithQueryAsync(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                GVUniformGrid.DataSource = dt;
                GVUniformGrid.DataBind();

                for (int i = 0; i < GVUniformGrid.Rows.Count; i++)
                {
                    GVUniformGrid.Columns[3].Visible = true;
                    //CheckBox chkempid = GVUniformGrid.Rows[i].FindControl("CbChecked") as CheckBox;
                    //chkempid.Checked = true;
                }


                GetTotal();
            }
            else
            {
                GVUniformGrid.DataSource = null;
                GVUniformGrid.DataBind();

            }


        }

        protected void LoadResourcedetailsBySearch()
        {

            var list = new List<string>();
            string ResourceIDs = "";

            ResourceIDs = string.Join(",", list.ToArray());

            DataTable dtpo = null;
            string Sqlqry = "";
            DataTable dt = null;

            var ResID = "";

            if (rdbIndividualWise.Checked == true)
            {
                Sqlqry = "select R.ResourceID,SI.ItemName,R.Price,SI.ActualQuantity,1 as Qty,'' as Balance from Resources R inner join invStockItemList SI on R.ResourceID=SI.ItemId where ResourceID in (" + ResourceIDs + ")  order by SI.ItemName";
            }
            else
            {
                Sqlqry = "select R.ResourceID,SI.ItemName,R.Price,SI.ActualQuantity,1 as Qty,'' as Balance from Resources R inner join invStockItemList SI on R.ResourceID=SI.ItemId where ResourceID in (" + ResourceIDs + ")  order by SI.ItemName";
            }

            dt = config.ExecuteReaderWithQueryAsync(Sqlqry).Result;


            if (dt.Rows.Count > 0)
            {

                gvresources.DataSource = dt;
                gvresources.DataBind();


                for (int i = 0; i < gvresources.Rows.Count; i++)
                {
                    CheckBox chkempid = gvresources.Rows[i].FindControl("CbChecked") as CheckBox;
                    chkempid.Checked = true;

                    if (rdbFromSupervisor.Checked == true)
                    {
                        gvresources.Columns[3].Visible = true;
                    }
                    else
                    {
                        gvresources.Columns[3].Visible = false;

                    }

                }

                gvresources.Visible = true;
                GetTotal();
            }
            else
            {
                gvresources.DataSource = null;
                gvresources.DataBind();

            }






        }

        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < gvresources.Rows.Count; i++)
                    {


                        CheckBox CbChecked = (CheckBox)gvresources.Rows[rowIndex].Cells[1].FindControl("CbChecked");
                        Label lblresourceid = (Label)gvresources.Rows[rowIndex].Cells[2].FindControl("lblresourceid");
                        Label lblresourcename = (Label)gvresources.Rows[rowIndex].Cells[3].FindControl("lblresourcename");
                        Label lblBalanceQty = (Label)gvresources.Rows[rowIndex].Cells[4].FindControl("lblBalanceQty");
                        TextBox txtQty = (TextBox)gvresources.Rows[rowIndex].Cells[5].FindControl("txtQty");
                        // TextBox txtresourceprice = (TextBox)gvresources.Rows[rowIndex].Cells[6].FindControl("txtresourceprice");


                        CbChecked.Checked = true;
                        lblresourceid.Text = dt.Rows[i]["ResourceID"].ToString();
                        lblresourcename.Text = dt.Rows[i]["ItemName"].ToString();
                        lblBalanceQty.Text = dt.Rows[i]["Balance"].ToString();
                        txtQty.Text = dt.Rows[i]["Qty"].ToString();



                        //txtresourceprice.Text = dt.Rows[i]["Price"].ToString();


                        rowIndex++;
                    }
                }
            }
        }

        public void GetTotal()
        {
            float Total = 0;
            float loanamt = 0;
            float paidamt = 0;


            for (int i = 0; i < GVUniformGrid.Rows.Count; i++)
            {
                CheckBox CbChecked = GVUniformGrid.Rows[i].FindControl("CbChecked") as CheckBox;
                TextBox txtQty = GVUniformGrid.Rows[i].FindControl("txtQty") as TextBox;
                TextBox txtresourceprice = GVUniformGrid.Rows[i].FindControl("txtresourceprice") as TextBox;

                if (CbChecked.Checked == true)
                {
                    Total += (Convert.ToSingle(txtQty.Text) * Convert.ToSingle(txtresourceprice.Text));
                }

            }

            loanamt = Total;

            if (txtPaidAmnt.Text == "")
            {
                paidamt = 0;
            }
            else
            {
                paidamt = float.Parse(txtPaidAmnt.Text);

            }

            if (paidamt > loanamt)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", "alert('Paid Amount should be equal or lower than the total loan amount. Please check and enter correct amount.');", true);
                txtPaidAmnt.Text = "0";
                txttotal.Text = Total.ToString();
                return;
            }
            else
            {
                txttotal.Text = (loanamt - paidamt).ToString();
            }


            UpTotal.Update();
            UpGv.Update();

        }

        private void Uniformidauto()
        {
            //getloandata();
            int UniformID;
            string selectqueryclientid = "select max(cast(UniformID as int )) as UniformID from EmpResourceDetails ";
            DataTable dt = config.ExecuteReaderWithQueryAsync(selectqueryclientid).Result;

            if (dt.Rows.Count > 0)
            {
                //  DtEmpId = dtempid.Rows[dtempid.Rows.Count - 1][0].ToString();
                if (String.IsNullOrEmpty(dt.Rows[0]["UniformID"].ToString()) == false)
                {
                    UniformID = Convert.ToInt32(dt.Rows[0]["UniformID"].ToString()) + 1;
                    txtuniformid.Text = UniformID.ToString();
                }
                else
                {
                    UniformID = int.Parse("1");
                    txtuniformid.Text = UniformID.ToString();
                }
            }
        }

        private void loanidauto()
        {
            //getloandata();
            int loanid;
            string selectqueryclientid = "select max(cast(LoanNo as int )) as Loanno from EmpLoanMaster ";
            DataTable dt = config.ExecuteReaderWithQueryAsync(selectqueryclientid).Result;

            if (dt.Rows.Count > 0)
            {
                //  DtEmpId = dtempid.Rows[dtempid.Rows.Count - 1][0].ToString();
                if (String.IsNullOrEmpty(dt.Rows[0]["LoanNo"].ToString()) == false)
                {
                    loanid = Convert.ToInt32(dt.Rows[0]["LoanNo"].ToString()) + 1;
                    txtloanid.Text = loanid.ToString();
                }
                else
                {
                    loanid = int.Parse("1");
                    txtloanid.Text = loanid.ToString("000001");
                }
            }
        }

        protected void gvresources_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvresources.PageIndex = e.NewPageIndex;
            LoadResourcedetails();
        }

        string Loanno = "";

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            string sqlqry = "select max(loanno) as loanno  from EmpResourceDetails where EmpID='" + txtEmpid.Text + "'";
            DataTable dtloan = config.ExecuteReaderWithQueryAsync(sqlqry).Result;
            if (dtloan.Rows.Count > 0)
            {
                Loanno = dtloan.Rows[0]["loanno"].ToString();
            }

            string UniformID = "";



            string qry = "select distinct erd.loanno,erd.price ,erd.qty,r.Price as itemrate,sil.itemname,elm.LoanAmount,elm.PaidAmnt,elm.NoInstalments,erd.uniformid,convert(varchar(10),elm.LoanIssuedDate,103) as LoanIssuedDate from EmpResourceDetails erd inner join invStockItemList sil on erd.ResourceId=sil.itemid inner join EmpLoanMaster elm on elm.empid=erd.empid and elm.LoanNo=erd.loanno inner join Resources R on erd.ResourceId=R.ResourceID where erd.empid='" + txtEmpid.Text + "' and erd.loanno='" + Loanno + "'";
            DataTable dt = config.ExecuteReaderWithQueryAsync(qry).Result;

            string LoanIssuedDate = "";


            if (dt.Rows.Count > 0)
            {

                MemoryStream ms = new MemoryStream();

                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                document.AddTitle("FaMS");
                document.AddAuthor("DIYOS");
                document.AddSubject("Wage Slips");
                document.AddKeywords("Keyword1, keyword2, …");//
                string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
                DataTable compInfo = config.ExecuteReaderWithQueryAsync(strQry).Result;
                string companyName = "Your Company Name";
                string companyAddress = "Your Company Address";
                if (compInfo.Rows.Count > 0)
                {
                    companyName = compInfo.Rows[0]["CompanyName"].ToString();
                    companyAddress = compInfo.Rows[0]["Address"].ToString();
                }

                PdfPTable Maintable = new PdfPTable(4);
                Maintable.TotalWidth = 500f;
                Maintable.LockedWidth = true;
                float[] width = new float[] { 1.5f, 2f, 2.5f, 1f };
                Maintable.SetWidths(width);
                uint FONT_SIZE = 10;
                #region  Table Headings


                LoanIssuedDate = dt.Rows[0]["LoanIssuedDate"].ToString();



                PdfPCell Heading = new PdfPCell(new Phrase("UNIFORM ISSUES", FontFactory.GetFont(Fontstyle, 10, Font.BOLD, BaseColor.BLACK)));
                Heading.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                Heading.Colspan = 4;
                Heading.Border = 0;// 15;
                Maintable.AddCell(Heading);

                PdfPCell CompanyName = new PdfPCell(new Phrase(companyName, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                CompanyName.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CompanyName.Border = 0;
                CompanyName.Colspan = 4;
                Maintable.AddCell(CompanyName);

                PdfPCell CompanyAddress = new PdfPCell(new Phrase(companyAddress, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                CompanyAddress.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CompanyAddress.Colspan = 4;
                CompanyAddress.Border = 0;
                Maintable.AddCell(CompanyAddress);

                PdfPCell employerAddress1 = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                employerAddress1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                employerAddress1.Border = 0;
                employerAddress1.Colspan = 4;
                Maintable.AddCell(employerAddress1);

                PdfPCell empcode = new PdfPCell(new Phrase("Emp Code            : ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                empcode.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                empcode.Colspan = 1;
                empcode.Border = 0;
                Maintable.AddCell(empcode);

                PdfPCell empcode1 = new PdfPCell(new Phrase(txtEmpid.Text, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                empcode1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                empcode1.Colspan = 1;
                empcode1.Border = 0;
                Maintable.AddCell(empcode1);

                PdfPCell IssueRefNo = new PdfPCell(new Phrase("   Issue RefNo  : ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                IssueRefNo.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                IssueRefNo.Colspan = 1;
                IssueRefNo.Border = 0;
                Maintable.AddCell(IssueRefNo);


                UniformID = dt.Rows[0]["uniformid"].ToString();

                PdfPCell IssueRefNo1 = new PdfPCell(new Phrase(UniformID, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                IssueRefNo1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                IssueRefNo1.Colspan = 1;
                IssueRefNo1.Border = 0;
                Maintable.AddCell(IssueRefNo1);

                PdfPCell EmployeeName = new PdfPCell(new Phrase("Employee Name  : ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                EmployeeName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                EmployeeName.Colspan = 1;
                EmployeeName.Border = 0;
                Maintable.AddCell(EmployeeName);

                PdfPCell EmployeeName1 = new PdfPCell(new Phrase(txtName.Text, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                EmployeeName1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                EmployeeName1.Colspan = 1;
                EmployeeName1.Border = 0;
                Maintable.AddCell(EmployeeName1);

                PdfPCell IssueDate = new PdfPCell(new Phrase("Issue Date  : ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                IssueDate.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right 
                IssueDate.Colspan = 1;
                IssueDate.Border = 0;
                Maintable.AddCell(IssueDate);

                PdfPCell IssueDate1 = new PdfPCell(new Phrase(LoanIssuedDate, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                IssueDate1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right 
                IssueDate1.Colspan = 1;
                IssueDate1.Border = 0;
                Maintable.AddCell(IssueDate1);


                PdfPCell cspace = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                cspace.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right 
                cspace.Colspan = 2;
                cspace.Border = 0;
                Maintable.AddCell(cspace);
                Maintable.AddCell(cspace);
                Maintable.AddCell(cspace);


                document.Add(Maintable);

                #endregion

                #region Table Data

                PdfPTable DetailsTable = new PdfPTable(5);
                DetailsTable.TotalWidth = 500f;
                DetailsTable.LockedWidth = true;
                float[] DetailsWidth = new float[] { 0.5f, 2f, 0.5f, 0.5f, 0.5f };
                DetailsTable.SetWidths(DetailsWidth);


                PdfPCell Series = new PdfPCell(new Phrase("S.No", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                Series.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                DetailsTable.AddCell(Series);


                PdfPCell ItemCode = new PdfPCell(new Phrase("Item Code ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                ItemCode.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                                  // DetailsTable.AddCell(ItemCode);


                PdfPCell ItemDesc = new PdfPCell(new Phrase("Item Desc ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                ItemDesc.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                DetailsTable.AddCell(ItemDesc);



                PdfPCell ItemRate = new PdfPCell(new Phrase("Item Rate ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                ItemRate.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                DetailsTable.AddCell(ItemRate);


                PdfPCell Quantity = new PdfPCell(new Phrase("Quantity", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                Quantity.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                DetailsTable.AddCell(Quantity);



                PdfPCell LineAmt = new PdfPCell(new Phrase("Line Amt", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                LineAmt.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                DetailsTable.AddCell(LineAmt);



                int j = 1;
                string ItemDescription = "";
                float Itemrate = 0;
                float quantity = 0;
                float Lineamt = 0;
                float TotalLineAmt = 0;

                float TotalAmountreceived = 0;
                float TotalAmountdue = 0;
                float NoOfinstalments = 0;

                float Amountreceived = 0;
                float Amountdue = 0;

                string InWords = "";


                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    ItemDescription = dt.Rows[i]["itemname"].ToString();
                    Itemrate = Convert.ToSingle(dt.Rows[i]["itemrate"].ToString());
                    quantity = Convert.ToSingle(dt.Rows[i]["qty"].ToString());
                    Lineamt = (Convert.ToSingle(dt.Rows[i]["price"].ToString()) * Convert.ToSingle(dt.Rows[i]["qty"].ToString()));


                    Amountreceived = Convert.ToSingle(dt.Rows[i]["LoanAmount"].ToString());
                    Amountdue = Convert.ToSingle(dt.Rows[i]["PaidAmnt"].ToString());
                    NoOfinstalments = Convert.ToSingle(dt.Rows[i]["NoInstalments"].ToString());


                    InWords = NumberToEnglish.Instance.changeNumericToWords(Amountreceived.ToString());

                    PdfPCell Series1 = new PdfPCell(new Phrase(j.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    Series1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    DetailsTable.AddCell(Series1);


                    PdfPCell ItemCode1 = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    ItemCode1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                       //DetailsTable.AddCell(ItemCode1);


                    PdfPCell ItemDesc1 = new PdfPCell(new Phrase(ItemDescription, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    ItemDesc1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    DetailsTable.AddCell(ItemDesc1);



                    PdfPCell ItemRate1 = new PdfPCell(new Phrase(Itemrate.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    ItemRate1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    DetailsTable.AddCell(ItemRate1);


                    PdfPCell Quantity1 = new PdfPCell(new Phrase(quantity.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    Quantity1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    DetailsTable.AddCell(Quantity1);



                    PdfPCell LineAmt1 = new PdfPCell(new Phrase(Lineamt.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    LineAmt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    DetailsTable.AddCell(LineAmt1);


                    TotalLineAmt += Lineamt;
                    // TotalAmountreceived += Amountreceived;
                    //TotalAmountdue += Amountdue;

                    j++;
                }


                #endregion

                document.Add(DetailsTable);


                PdfPTable DetailsTable2 = new PdfPTable(4);
                DetailsTable2.TotalWidth = 500f;
                DetailsTable2.LockedWidth = true;
                float[] DetailsWidth2 = new float[] { 2f, 2f, 3f, 1f };
                DetailsTable2.SetWidths(DetailsWidth2);


                PdfPCell Noofinstalments = new PdfPCell(new Phrase("No.of Instalments:   " + NoOfinstalments.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                Noofinstalments.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                Noofinstalments.Border = 0;
                Noofinstalments.Colspan = 4;
                // DetailsTable2.AddCell(Noofinstalments);


                //DetailsTable1.AddCell(cellemp);
                PdfPCell totalamt = new PdfPCell(new Phrase("Total Amount :", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                totalamt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                totalamt.Border = 0;
                totalamt.Colspan = 3;
                DetailsTable2.AddCell(totalamt);

                PdfPCell totalamts1 = new PdfPCell(new Phrase(TotalLineAmt.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                totalamts1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                totalamts1.Border = 0;
                totalamts1.Colspan = 1;
                DetailsTable2.AddCell(totalamts1);



                PdfPCell AmountReceived = new PdfPCell(new Phrase("Amount Received :", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                AmountReceived.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                AmountReceived.Border = 0;
                AmountReceived.Colspan = 3;
                // AmountReceived.PaddingLeft = 10;
                DetailsTable2.AddCell(AmountReceived);


                PdfPCell totalamt1 = new PdfPCell(new Phrase(Amountdue.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                totalamt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                totalamt1.Border = 0;
                totalamt1.Colspan = 1;
                DetailsTable2.AddCell(totalamt1);

                PdfPCell AmountDue = new PdfPCell(new Phrase("Amount Due :", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                AmountDue.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                AmountDue.Border = 0;
                AmountDue.Colspan = 3;
                // AmountDue.PaddingLeft = 10;
                DetailsTable2.AddCell(AmountDue);

                PdfPCell AmountReceived1 = new PdfPCell(new Phrase(Amountreceived.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                AmountReceived1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                AmountReceived1.Border = 0;
                AmountReceived1.Colspan = 1;
                DetailsTable2.AddCell(AmountReceived1);


                document.Add(DetailsTable2);

                PdfPTable DetailsTable1 = new PdfPTable(5);
                DetailsTable1.TotalWidth = 500f;
                DetailsTable1.LockedWidth = true;
                float[] DetailsWidth1 = new float[] { 1f, 1f, 1f, 1f, 1f };
                DetailsTable1.SetWidths(DetailsWidth1);


                PdfPCell AmountDue1 = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                AmountDue1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                AmountDue1.Border = 0;
                AmountDue1.Colspan = 5;
                DetailsTable1.AddCell(AmountDue1);

                PdfPCell Amountinwords = new PdfPCell(new Phrase("Amount in Words: " + InWords.Trim() + " Only \n\n\n", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                Amountinwords.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                Amountinwords.Border = 0;
                Amountinwords.Colspan = 6;
                DetailsTable1.AddCell(Amountinwords);


                PdfPCell PreparedBy = new PdfPCell(new Phrase("Prepared by\n\n\n\n\n", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                PreparedBy.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                PreparedBy.Border = 0;
                PreparedBy.Colspan = 2;
                DetailsTable1.AddCell(PreparedBy);


                PdfPCell IssuedBy = new PdfPCell(new Phrase("Issued by", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                IssuedBy.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                IssuedBy.Border = 0;
                IssuedBy.Colspan = 2;
                DetailsTable1.AddCell(IssuedBy);


                PdfPCell ReceivedBy = new PdfPCell(new Phrase("Received by", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                ReceivedBy.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                ReceivedBy.Border = 0;
                ReceivedBy.Colspan = 2;
                DetailsTable1.AddCell(ReceivedBy);


                PdfPCell SRActionedBy = new PdfPCell(new Phrase("S.R.Actioned by", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                SRActionedBy.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                SRActionedBy.Border = 0;
                SRActionedBy.Colspan = 3;
                DetailsTable1.AddCell(SRActionedBy);

                PdfPCell CAuthority = new PdfPCell(new Phrase("Recovery Actioned in Sys. by ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                CAuthority.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                CAuthority.Border = 0;
                CAuthority.Colspan = 3;
                DetailsTable1.AddCell(CAuthority);



                document.Add(DetailsTable1);




                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=UniformPDF.pdf");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();



            }
        }

        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            float Total = 0;
            for (int i = 0; i < gvresources.Rows.Count; i++)
            {
                CheckBox CbChecked = gvresources.Rows[i].FindControl("CbChecked") as CheckBox;
                TextBox txtQty = gvresources.Rows[i].FindControl("txtQty") as TextBox;
                TextBox txtresourceprice = gvresources.Rows[i].FindControl("txtresourceprice") as TextBox;

                if (CbChecked.Checked == true)
                {

                    Total += (Convert.ToSingle(txtQty.Text) * Convert.ToSingle(txtresourceprice.Text));
                }
                else
                {
                    txttotal.Text = "";
                }
            }
            txttotal.Text = Total.ToString();
        }

        public void LoadItemsbyUniformCategory()
        {
            GVUniformGrid.DataSource = null;
            GVUniformGrid.DataBind();

            string qry = "";

            //if (rdbIndividualWise.Checked)
            //{
            qry = "select R.ResourceID,SI.ItemName,R.Price,SI.ActualQuantity,1 as Qty,'' as Balance,category from Resources R inner join invStockItemList SI on R.ResourceID=SI.ItemId where ( category='Uniform' or category='General') order by category,SNo,SI.ItemName";
            // }
            // else if (rdbToSupervisor.Checked)
            //{
            //qry = "select R.ResourceID,SI.ItemName,R.Price,SI.ActualQuantity,1 as Qty,'' as Balance,category from Resources R inner join invStockItemList SI on R.ResourceID=SI.ItemId where ( category='Uniform') order by category,SNo,SI.ItemName";
            //}


            DataTable dt = config.ExecuteReaderWithQueryAsync(qry).Result;
            if (dt.Rows.Count > 0)
            {
                GVUniformGrid.DataSource = dt;
                GVUniformGrid.DataBind();

                for (int i = 0; i < GVUniformGrid.Rows.Count; i++)
                {

                    Label lblCategory = GVUniformGrid.Rows[i].FindControl("lblCategory") as Label;

                    if (lblCategory.Text == "General")
                    {
                        CheckBox chkempid = GVUniformGrid.Rows[i].FindControl("CbChecked") as CheckBox;
                        chkempid.Checked = true;


                    }

                    if (rdbFromSupervisor.Checked == true)
                    {
                        GVUniformGrid.Columns[3].Visible = true;
                    }
                    else
                    {
                        GVUniformGrid.Columns[3].Visible = false;

                    }


                }

                GetTotal();
            }

        }

        protected void ChkGeneral_CheckedChanged(object sender, EventArgs e)
        {

            if (GVUniformGrid.Rows.Count > 0)
            {
                for (int i = 0; i < GVUniformGrid.Rows.Count; i++)
                {
                    Label lblcategory = GVUniformGrid.Rows[i].FindControl("lblCategory") as Label;
                    if (lblcategory.Text == "General")
                    {
                        CheckBox chkempid = GVUniformGrid.Rows[i].FindControl("CbChecked") as CheckBox;

                        if (ChkGeneral.Checked)
                        {
                            chkempid.Checked = true;
                            GVUniformGrid.Rows[i].Visible = true;
                        }
                        else
                        {
                            chkempid.Checked = false;
                            GVUniformGrid.Rows[i].Visible = false;
                        }
                    }
                }
            }

            GetTotal();



        }

        protected void CbChecked_CheckedChanged(object sender, EventArgs e)
        {

            GetTotal();

        }

        protected void txtQty_TextChanged1(object sender, EventArgs e)
        {
            GetTotal();
        }

        protected void txtPaidAmnt_TextChanged(object sender, EventArgs e)
        {

            GetTotal();
        }

        protected void GVUniformGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((Label)e.Row.FindControl("lblCategory")).Text == "General")
                {
                    e.Row.Font.Italic = true;
                }




            }
        }
    }
}