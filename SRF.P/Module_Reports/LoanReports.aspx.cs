using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Web.UI.HtmlControls;
using System.Globalization;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class LoanReports : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
        string BranchID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            lbltmt.Text = "";
            DataTable DtDesignation;
            if (!IsPostBack)
            {
                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {

                }
                else
                {
                    Response.Redirect("login.aspx");
                }

                if (this.Master != null)
                {
                    HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("sli1");
                    if (emplink != null)
                    {
                        emplink.Attributes["class"] = "current";
                    }
                }
                LoanDropdwn();
            }
        }

        public void LoanDropdwn()
        {
            DataTable DtloanTypes = GlobalData.Instance.LoadLoanTypes();
            if (DtloanTypes.Rows.Count > 0)
            {
                ddlloantypes.DataValueField = "Id";
                ddlloantypes.DataTextField = "Loantype";
                ddlloantypes.DataSource = DtloanTypes;
                ddlloantypes.DataBind();

            }
            //ddlLoanType.Items.Insert(0, "-Select-");
            ddlloantypes.Items.Insert(0, new ListItem("All", "-1"));
        }


        protected void ddlloanoperations_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlloanoperations.SelectedIndex == 1)
            {

            }


        }


        protected void ddlissuedloans_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlissuedloans.SelectedIndex == 0)
            {
                ddlloantypes.SelectedIndex = 0;
                txtfromdate.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txttodate.Visible = false;
                lblSelectmonth.Visible = true;
                txtloanissue.Text = "";
                txtloanissue.Visible = true;
                lblempid.Visible = false;
                lblEmployeeName.Visible = false;
                txtemplyid.Visible = false;
                txtFname.Visible = false;
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();
            }
            else if (ddlissuedloans.SelectedIndex == 1)
            {
                ddlloantypes.SelectedIndex = 0;
                txtfromdate.Visible = true;
                txtfromdate.Text = "";
                txttodate.Text = "";
                lblfrom.Visible = true;
                lblto.Visible = true;
                txttodate.Visible = true;
                lblSelectmonth.Visible = false;
                txtloanissue.Visible = false;
                lblempid.Visible = false;
                lblEmployeeName.Visible = false;
                txtemplyid.Visible = false;
                txtFname.Visible = false;
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();
            }
            else
            {
                ddlloantypes.SelectedIndex = 0;
                txtfromdate.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txttodate.Visible = false;
                lblSelectmonth.Visible = false;
                lblempid.Visible = true;
                lblEmployeeName.Visible = true;
                txtemplyid.Visible = true;
                txtFname.Visible = true;
                txtloanissue.Visible = false;
                txtloanissue.Visible = false;
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();
            }
        }
        protected void txtemplyid_TextChanged(object sender, EventArgs e)
        {

            GetEmpName();

        }

        protected void GetEmpName()
        {
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
            if (txtemplyid.Text.Length > 0)
            {
                string Sqlqry = "select (empfname+' '+empmname+' '+emplname) as empname from empdetails where empid='" + txtemplyid.Text + "' ";
                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        txtFname.Text = dt.Rows[0]["empname"].ToString();

                    }
                    catch (Exception ex)
                    {
                        // MessageLabel.Text = ex.Message;
                    }
                }
                else
                {
                    txtemplyid.Text = "";
                    txtFname.Text = "";
                }

            }
        }

        protected void Getempid()
        {
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();

            string Sqlqry = "select empid+' - '+Oldempid empid from empdetails where  empfname+' '+empmname+' '+emplname like '%" + txtFname.Text + "%' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtemplyid.Text = dt.Rows[0]["empid"].ToString();

                }
                catch (Exception ex)
                {
                    // MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                txtemplyid.Text = "";
                txtFname.Text = "";
            }

        }


        protected void txtFname_TextChanged(object sender, EventArgs e)
        {
            Getempid();

        }
        protected void Btn_Search_Loans_Click(object sender, EventArgs e)
        {

            #region Begin New Code for Variable Declaration as on [11-11-2013]
            var SqlQry = "";

            DataTable DtForLoans = null;
            Hashtable HtLoans = new Hashtable();
            HtLoans.Clear();
            var CaseNoofLoan = 0;
            string ProcedureName = "IMReportForEmployeeLoans";
            string TypeofLoan = ddlloantypes.SelectedValue;

            if (TypeofLoan == "-1")
            {
                TypeofLoan = "%";
            }
            #endregion End New Code For Variable Declaration as on [11-11-2013]

            #region  Begin code For Validations as on [09-11-2013]

            if (ddlloanoperations.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select The Loan Operations');", true);
                return;
            }

            if (ddlissuedloans.SelectedIndex == 0)
            {

                if (txtloanissue.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select The Month');", true);
                    return;
                }

            }
            if (ddlissuedloans.SelectedIndex == 1)
            {
                if (txtfromdate.Text.Trim().Length == 0 || txttodate.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select From Month and To Month');", true);
                    return;
                }
            }

            var testDate = 0;
            #region Begin  Code For Month check Valid or Invalid as on 09-10-2013
            if (ddlissuedloans.SelectedIndex == 0)
            {
                //if (txtloanissue.Text.Trim().Length > 0)
                //{

                //    //testDate = GlobalData.Instance.CheckEnteredDate(txtloanissue.Text);
                //    //if (testDate > 0)
                //    //{
                //    //    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid Month/Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                //    //    return;
                //    //}
                //}
                #endregion End   Code For Month check Valid or Invalid as on 09-10-2013
                #endregion end code For Validations as on [09-11-2013]

                string EntereMonth = DateTime.Parse(txtloanissue.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();

                #region Begin Code for  CASE ONE : Issued Loans &  MonthLy Wise & All Loans  as On [09-11-2013]

                //string Month = DateTime.Parse(txtloanissue.Text.Trim()).Month.ToString();
                //string year = DateTime.Parse(txtloanissue.Text.Trim()).Year.ToString();

                //string monthnew = Month + year.Substring(2, 2);

                string Month = DateTime.Parse(EntereMonth).Month.ToString();
                string year = DateTime.Parse(EntereMonth).Year.ToString();
                string monthnew = Month + year.Substring(2, 2);

              
                HtLoans.Add("@TypeofLoan", TypeofLoan);
                HtLoans.Add("@CaseNoofLoan", ddlloanoperations.SelectedIndex);
                HtLoans.Add("@selectedMonth", Month + year);
                HtLoans.Add("@loanCuttingMonth", monthnew);
                HtLoans.Add("@Option", ddlissuedloans.SelectedIndex);
              

                DtForLoans = config.ExecuteAdaptorAsyncWithParams(ProcedureName, HtLoans).Result;


                if (DtForLoans.Rows.Count > 0)
                {
                    GVListEmployees.DataSource = DtForLoans;
                    GVListEmployees.DataBind();
                }
                else
                {
                    GVListEmployees.DataSource = null;
                    GVListEmployees.DataBind();
                }
            }
            if (ddlissuedloans.SelectedIndex == 2)
            {

                var Empid = txtemplyid.Text;

                HtLoans.Add("@TypeofLoan", TypeofLoan);
                HtLoans.Add("@CaseNoofLoan", ddlloanoperations.SelectedIndex);
                HtLoans.Add("@Empid", Empid);
                HtLoans.Add("@Option", ddlissuedloans.SelectedIndex);

                DtForLoans = config.ExecuteAdaptorAsyncWithParams(ProcedureName, HtLoans).Result;


                if (DtForLoans.Rows.Count > 0)
                {
                    GVListEmployees.DataSource = DtForLoans;
                    GVListEmployees.DataBind();
                }
                else
                {
                    GVListEmployees.DataSource = null;
                    GVListEmployees.DataBind();
                }
            }
            else
            {
                if (ddlissuedloans.SelectedIndex == 1)
                {

                    if (txtfromdate.Text.Trim().Length > 0)
                    {
                        testDate = GlobalData.Instance.CheckEnteredDate(txtfromdate.Text);
                        if (testDate > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid Month/Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                            return;
                        }
                    }

                    if (txttodate.Text.Trim().Length > 0)
                    {
                        testDate = GlobalData.Instance.CheckEnteredDate(txttodate.Text);
                        if (testDate > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid Month/Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                            return;
                        }
                    }

                    string frommonth = DateTime.Parse(txtfromdate.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("MM/dd/yyyy");
                    string tomonth = DateTime.Parse(txttodate.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("MM/dd/yyyy");

                    HtLoans.Add("@TypeofLoan", TypeofLoan);
                    HtLoans.Add("@CaseNoofLoan", ddlloanoperations.SelectedIndex);
                    HtLoans.Add("@FromMonth", frommonth);
                    HtLoans.Add("@TOMonth", tomonth);
                    HtLoans.Add("@Option", ddlissuedloans.SelectedIndex);
                    // HtLoans.Add("@EmpidPrefix", EmpIDPrefix);

                    DtForLoans = config.ExecuteAdaptorAsyncWithParams(ProcedureName, HtLoans).Result;
                }

                if (DtForLoans.Rows.Count > 0)
                {
                    GVListEmployees.DataSource = DtForLoans;
                    GVListEmployees.DataBind();
                }
                else
                {
                    GVListEmployees.DataSource = null;
                    GVListEmployees.DataBind();
                }

                #endregion End  Code for  CASE ONE : Issued Loans &  MonthLy Wise & All Loans  as On [09-11-2013]




            }
        }


        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            gve.Export("AllLoansReport.xls", this.GVListEmployees);
        }

        decimal LoamAmt = 0, AmtTobededuct = 0, Amtdeduct = 0, DueAmt = 0;
        int NoOfinst = 0;
        protected void GVListEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[10].Attributes.Add("class", "text");
                e.Row.Cells[11].Attributes.Add("class", "text");
                LoamAmt += decimal.Parse(((Label)e.Row.FindControl("lblloanamount")).Text);
                NoOfinst += int.Parse(((Label)e.Row.FindControl("lblEmpLastName")).Text);
                // AmtTobededuct += decimal.Parse(((Label)e.Row.FindControl("lblamounttobededucted")).Text);
                Amtdeduct += decimal.Parse(((Label)e.Row.FindControl("lblamountdeducted")).Text);
                DueAmt += decimal.Parse(((Label)e.Row.FindControl("lbldueamount")).Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Total";
                e.Row.Cells[6].Text = LoamAmt.ToString("N", CultureInfo.InvariantCulture);
                e.Row.Cells[7].Text = NoOfinst.ToString("N", CultureInfo.InvariantCulture);
                // e.Row.Cells[8].Text = AmtTobededuct.ToString("N", CultureInfo.InvariantCulture);
                e.Row.Cells[8].Text = Amtdeduct.ToString("N", CultureInfo.InvariantCulture);
                e.Row.Cells[9].Text = DueAmt.ToString("N", CultureInfo.InvariantCulture);
            }
        }
    }
}