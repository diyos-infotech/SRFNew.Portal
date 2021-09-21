using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Data;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class EmpSummary : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";

        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();


        protected void Page_Load(object sender, EventArgs e)
        {

            try
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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }

        }


        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }
        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("EmpSummary.xls", this.GVListEmployees);
        }



        protected void ClearData()
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
        }

        public void loadbalance()
        {

            string qry = "";
            DataTable dt = null;
            string balance = "";
            if (ddlloantype.SelectedIndex == 1)
            {
                qry = "select( (select isnull(SUM(isnull(LoanAmount,0)),0) from EmpLoanMaster where EmpId='" + txtEmpid.Text + "' and typeofloan=0)-(select isnull(SUM(isnull(recamt,0)),0) from EmpLoanDetails where Empid='" + txtEmpid.Text + "' and loantype=0) ) as balance ";
                dt = config.ExecuteReaderWithQueryAsync(qry).Result;
            }

            else if (ddlloantype.SelectedIndex == 2)
            {
                qry = "select( (select isnull(SUM(isnull(LoanAmount,0)),0) from EmpLoanMaster where EmpId='" + txtEmpid.Text + "' and typeofloan=1)-(select isnull(SUM(isnull(recamt,0)),0) from EmpLoanDetails where Empid='" + txtEmpid.Text + "' and loantype=1) ) as balance ";
                dt = config.ExecuteReaderWithQueryAsync(qry).Result;

            }
            else if (ddlloantype.SelectedIndex == 3)
            {
                qry = "select( (select isnull(SUM(isnull(LoanAmount,0)),0) from EmpLoanMaster where EmpId='" + txtEmpid.Text + "' and typeofloan=2)-(select isnull(SUM(isnull(recamt,0)),0) from EmpLoanDetails where Empid='" + txtEmpid.Text + "' and loantype=2) ) as balance ";
                dt = config.ExecuteReaderWithQueryAsync(qry).Result;

            }
            else if (ddlloantype.SelectedIndex == 4)
            {
                qry = "select( (select isnull(SUM(isnull(LoanAmount,0)),0) from EmpLoanMaster where EmpId='" + txtEmpid.Text + "' and typeofloan=3)-(select isnull(SUM(isnull(recamt,0)),0) from EmpLoanDetails where Empid='" + txtEmpid.Text + "' and loantype=3) ) as balance ";
                dt = config.ExecuteReaderWithQueryAsync(qry).Result;

            }
            else if (ddlloantype.SelectedIndex == 5)
            {
                qry = "select( (select isnull(SUM(isnull(LoanAmount,0)),0) from EmpLoanMaster where EmpId='" + txtEmpid.Text + "' and typeofloan=4)-(select isnull(SUM(isnull(recamt,0)),0) from EmpLoanDetails where Empid='" + txtEmpid.Text + "' and loantype=4) ) as balance ";
                dt = config.ExecuteReaderWithQueryAsync(qry).Result;

            }

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][0].ToString() == "" || dt.Rows[0][0].ToString() == "0")
                {
                    lblbalance.Visible = false;
                    txtbalance.Visible = false;
                    lblloanissued.Visible = false;
                    lblloandeducted.Visible = false;

                    ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('No loan details found');", true);

                }
                else
                {
                    lblloanissued.Visible = true;
                    lblloandeducted.Visible = true;
                    lblbalance.Visible = true;
                    txtbalance.Visible = true;
                    balance = dt.Rows[0]["balance"].ToString();
                    txtbalance.Text = balance;
                }


            }
            else
            {
                lblbalance.Visible = false;
                txtbalance.Visible = false;
                lblloanissued.Visible = false;
                lblloandeducted.Visible = false;

                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('No loan details found');", true);

            }
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            LblResult.Visible = true;
            ClearData();

            if (txtEmpid.Text.Trim() == "")
            {
                LblResult.Text = "Please Select Employee Id/Employee Name";
                return;
            }

            var SPNameIssued = "";
            Hashtable HTPaysheetissued = new Hashtable();

            var SPNameDeducted = "";
            Hashtable HTPaysheetDeducted = new Hashtable();

            string startdate = string.Empty;
            string enddate = string.Empty;
            string qry = string.Empty;
            DataTable dtloan = null;
            if (ddlloantype.SelectedIndex == 0)
            {
                lbtn_Export.Visible = true;
                pnlloans.Visible = false;
                SPNameIssued = "EmpSummary";
                HTPaysheetissued.Add("@Empid", txtEmpid.Text);
                HTPaysheetissued.Add("@Type", ddlloantype.SelectedIndex);
                DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPNameIssued, HTPaysheetissued).Result;

                if (dt.Rows.Count > 0)
                {
                    GVListEmployees.DataSource = dt;
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
                lbtn_Export.Visible = false;
                pnlloans.Visible = true;
                SPNameIssued = "EmpSummary";
                HTPaysheetissued.Add("@Empid", txtEmpid.Text);
                HTPaysheetissued.Add("@Type", ddlloantype.SelectedIndex);
                HTPaysheetissued.Add("@TypeOfLoan", ddlloantype.SelectedIndex - 1);
                DataTable dtloanIssued = config.ExecuteAdaptorAsyncWithParams(SPNameIssued, HTPaysheetissued).Result;

                if (dtloanIssued.Rows.Count > 0)
                {
                    GVLoanIssued.DataSource = dtloanIssued;
                    GVLoanIssued.DataBind();
                }
                else
                {
                    GVLoanIssued.DataSource = null;
                    GVLoanIssued.DataBind();
                }

                SPNameDeducted = "EmpSummaryforLoanDeducted";
                HTPaysheetDeducted.Add("@Empids", txtEmpid.Text);
                HTPaysheetDeducted.Add("@LoanType", ddlloantype.SelectedIndex - 1);
                DataTable dtloanDeducted = config.ExecuteAdaptorAsyncWithParams(SPNameDeducted, HTPaysheetDeducted).Result;

                if (dtloanDeducted.Rows.Count > 0)
                {
                    GVLoanDeducted.DataSource = dtloanDeducted;
                    GVLoanDeducted.DataBind();
                }
                else
                {
                    GVLoanDeducted.DataSource = null;
                    GVLoanDeducted.DataBind();
                }

                loadbalance();
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
            GetEmpid();
        }
        protected void txtEmpid_TextChanged(object sender, EventArgs e)
        {
            GetEmpName();
        }
        float totalDeductedAmount = 0;

        protected void GVLoanDeducted_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                // e.Row.Cells[1].Attributes.Add("class", "Text");

                Label month = (((Label)e.Row.FindControl("Lblmonth")));
                mvalue = month.Text;


                if (mvalue.Length == 3)
                {
                    monthval = mvalue.Substring(0, 1);

                    if (monthval == "1")
                    {
                        monthval = "Jan -";
                    }
                    if (monthval == "2")
                    {
                        monthval = "Feb -";
                    }
                    if (monthval == "3")
                    {
                        monthval = "Mar -";
                    }
                    if (monthval == "4")
                    {
                        monthval = "Apr -";
                    }
                    if (monthval == "5")
                    {
                        monthval = "May -";
                    }
                    if (monthval == "6")
                    {
                        monthval = "Jun -";
                    }
                    if (monthval == "7")
                    {
                        monthval = "Jul -";
                    }
                    if (monthval == "8")
                    {
                        monthval = "Aug -";
                    }
                    if (monthval == "9")
                    {
                        monthval = "Sep -";
                    }
                }
                else
                {
                    monthval = mvalue.Substring(0, 2);

                    if (monthval == "10")
                    {
                        monthval = "Oct -";
                    }

                    if (monthval == "11")
                    {
                        monthval = "Nov -";
                    }
                    if (monthval == "12")
                    {
                        monthval = "Dec -";
                    }
                }


                if (mvalue.Length == 3)
                {
                    yearvalue = mvalue.Substring(1, 2);
                }
                else
                {

                    yearvalue = mvalue.Substring(2, 2);
                }
                ((Label)e.Row.FindControl("Lblmonth")).Text = monthval + yearvalue;


                float DeductedAmount = float.Parse(((Label)e.Row.FindControl("LblDeductedAmount")).Text);
                totalDeductedAmount += DeductedAmount;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((Label)e.Row.FindControl("lblTotalDeductedAmount")).Text = totalDeductedAmount.ToString();

            }


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
        string mvalue = "";
        string monthval = "";
        string Yr = "";
        string yearvalue = "";
        //string Length = "";
        protected void GVListEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                // e.Row.Cells[1].Attributes.Add("class", "Text");

                Label month = (((Label)e.Row.FindControl("lblmonthn")));
                mvalue = month.Text.ToString();


                if (mvalue.Length == 3)
                {
                    monthval = mvalue.Substring(0, 1);

                    if (monthval == "1")
                    {
                        monthval = "Jan -";
                    }
                    if (monthval == "2")
                    {
                        monthval = "Feb -";
                    }
                    if (monthval == "3")
                    {
                        monthval = "Mar -";
                    }
                    if (monthval == "4")
                    {
                        monthval = "Apr -";
                    }
                    if (monthval == "5")
                    {
                        monthval = "May -";
                    }
                    if (monthval == "6")
                    {
                        monthval = "Jun -";
                    }
                    if (monthval == "7")
                    {
                        monthval = "Jul -";
                    }
                    if (monthval == "8")
                    {
                        monthval = "Aug -";
                    }
                    if (monthval == "9")
                    {
                        monthval = "Sep -";
                    }
                }
                else
                {
                    monthval = mvalue.Substring(0, 2);

                    if (monthval == "10")
                    {
                        monthval = "Oct -";
                    }

                    if (monthval == "11")
                    {
                        monthval = "Nov -";
                    }
                    if (monthval == "12")
                    {
                        monthval = "Dec -";
                    }
                }


                if (mvalue.Length == 3)
                {
                    yearvalue = mvalue.Substring(1, 2);
                }
                else
                {

                    yearvalue = mvalue.Substring(2, 2);
                }
                ((Label)e.Row.FindControl("lblmonth")).Text = monthval + "" + yearvalue;
                e.Row.Cells[3].Attributes.Add("class", "text");

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //    ((Label)e.Row.FindControl("lblTotalDeductedAmount")).Text = totalDeductedAmount.ToString();

            }


        }

        float totalissuedAmount = 0;
        protected void GVLoanIssued_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                float IssuedAmount = float.Parse(((Label)e.Row.FindControl("lblissuedAmount")).Text);
                totalissuedAmount += IssuedAmount;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((Label)e.Row.FindControl("lblTotalissuedAmount")).Text = totalissuedAmount.ToString();
            }

        }
    }
}