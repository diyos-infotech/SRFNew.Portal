using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using KLTS.Data;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class BulkLeftEmployeeDetails : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";

        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil grvutil = new GridViewExportUtil();
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            GetWebConfigdata();
            //LblResult.Visible = true;
            //LblResult.Text = "";
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

            #region Validation

            if (txtmonth.Text.Trim().Length > 0)
            {

                try
                {

                    testDate = GlobalData.Instance.CheckEnteredDate(txtmonth.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return 0;
                    }
                    EnteredDate = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return 0;
                }
            }
            #endregion


            #region  Month Get Based on the Control Selection
            int month = 0;

            DateTime date = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            month = Timings.Instance.GetIdForEnteredMOnth(date);
            return month;



            #endregion
        }

        public string GetMonth()
        {
            string month = "";
            string year = "";
            string DateVal = "";
            DateTime date;


            if (txtmonth.Text != "")
            {

                month = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-GB")).Month.ToString();
                year = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-GB")).Year.ToString();

            }

            DateVal = month + year.Substring(2, 2);
            return DateVal;

        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {

            GetData();
        }

        protected void txtmonth_TextChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            GetData();
        }

        public void GetData()
        {
            string date = string.Empty;
            string ClientID = "";

            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select The Month');", true);
                return;
            }

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();
            btn_Submit.Visible = true;
            int B4Year = 0;
            int B4FMonth = 0;
            int B4SMonth = 0;
            int B4TMonth = 0;
            string B4FDate = "";
            string B4SDate = "";
            string B4TDate = "";
            if (month == "01" || month == "1")
            {
                B4FMonth = 12 - 1;
                B4SMonth = 12 - 2;
                B4TMonth = 12 - 3;
                B4Year = Convert.ToInt16(Year.Substring(2, 2)) - 1;
            }
            else
            {
                B4FMonth = Convert.ToInt16(month) - 1;
                B4SMonth = Convert.ToInt16(month) - 2;
                B4TMonth = Convert.ToInt16(month) - 3;
                B4Year = Convert.ToInt16(Year.Substring(2, 2));
            }
            B4FDate = B4FMonth.ToString() + B4Year.ToString();
            B4SDate = B4SMonth.ToString() + B4Year.ToString();
            B4TDate = B4TMonth.ToString() + B4Year.ToString();
            int option = 0;

            var SPName = "";
            Hashtable HTPaysheet = new Hashtable();
            SPName = "BulkLeftEmployeeDetails";

            HTPaysheet.Add("@B4FMonth", B4FDate);
            HTPaysheet.Add("@B4SMonth", B4SDate);
            HTPaysheet.Add("@B4TMonth", B4TDate);

            DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, HTPaysheet).Result;
            if (dt.Rows.Count > 0)
            {

                grvutil.NewExportExcel("ReportforLeftEmployees.xls", dt);
            }
            else
            {
                btn_Submit.Visible = false;
            }
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {

        }

        public string GetExcelSheetNames()
        {
            string ExcelSheetname = "";
            OleDbConnection con = null;
            DataTable dt = null;
            string filename = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(fileupload1.PostedFile.FileName));
            fileupload1.PostedFile.SaveAs(filename);
            string extn = Path.GetExtension(fileupload1.PostedFile.FileName);
            string conStr = string.Empty;
            if (extn.ToLower() == ".xls")
            {
                conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended properties=\"excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (extn.ToLower() == ".xlsx")
            {
                conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
            }

            con = new OleDbConnection(conStr);
            con.Open();
            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (dt == null)
            {
                return null;
            }
            ExcelSheetname = dt.Rows[0]["TABLE_NAME"].ToString();
            ////foreach (DataRow row in dt.Rows)
            ////{
            ////    ExcelSheetname = row["TABLE_NAME"].ToString();
            ////}

            return ExcelSheetname;
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {

                #region Begin Code for when select Full Attendance as on 31/07/2014 by Venkat

                //  if (ddlAttendanceMode.SelectedIndex == 0)
                {
                    string filename = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(fileupload1.PostedFile.FileName));
                    fileupload1.PostedFile.SaveAs(filename);
                    string extn = Path.GetExtension(fileupload1.PostedFile.FileName);
                    string constring = "";
                    if (extn.ToLower() == ".xls")
                    {
                        //constring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended properties=\"excel 8.0;HDR=Yes;IMEX=2\"";
                        constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (extn.ToLower() == ".xlsx")
                    {
                        constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
                    }

                    string Sheetname = string.Empty;

                    //code commented on 30-11-2015 by swathi 

                    //string qry = "select [Client Id],[Emp Id],[Designation],[Duties],[WOs],[NHS],[OTs],[OTs1],[Canteen Advance],[Penalty]," +
                    //" [Incentives],[NA],[AB] from  [" + GetExcelSheetNames() + "]" + "";


                    string qry = "select [EmpId],[EmpFName],[DOL] " +
                    "  from  [" + GetExcelSheetNames() + "]" + "";


                    OleDbConnection con = new OleDbConnection(constring);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();

                    }
                    OleDbCommand cmd = new OleDbCommand(qry, con);
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    da.Dispose();
                    con.Close();
                    con.Dispose();
                    GC.Collect();

                    string empid = string.Empty;
                    string DateOfLeaving = string.Empty;
                    int testDate = 0;
                    int status = 0;
                    //foreach (DataRow dr in ds.Tables[0].Rows)
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string Remark = string.Empty;
                        empid = ds.Tables[0].Rows[i]["EmpId"].ToString();
                        DateOfLeaving = ds.Tables[0].Rows[i]["DOL"].ToString();
                        string sqlchkempid = "select empid from empdetails where empid='" + empid + "'";
                        DataTable dtchkempid = config.ExecuteAdaptorAsyncWithQueryParams(sqlchkempid).Result;

                        if (dtchkempid.Rows.Count > 0)
                        {
                            empid = dtchkempid.Rows[0]["empid"].ToString();
                        }
                        else
                        {

                        }

                        if (DateOfLeaving.Length > 0)
                        {
                            testDate = GlobalData.Instance.CheckEnteredDate(DateOfLeaving);
                            if (testDate > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(),
                                    "show alert", "alert('You Are Entered Invalid Date Of Leaving.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990 for " + empid + "');", true);
                                return;
                            }

                            DateOfLeaving = DateTime.Parse(DateOfLeaving, CultureInfo.GetCultureInfo("en-gb")).ToString();
                        }
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Enter Loan issue date');", true);
                        //    return;
                        //}

                        //Create Procedure


                        if (empid.Length > 0)
                        {

                            #region Begin New code for Stored Procedure as on 29/04/2014 by venkat

                            string UpQry = "Update empdetails set Empstatus=0,EmpDtofLeaving='" + DateOfLeaving + "'  where EmpID='" + empid + "'";
                            status = config.ExecuteNonQueryWithQueryAsync(UpQry).Result;

                            #endregion
                        }
                    }
                }
            }


            #endregion
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please Upload Valid Data');", true);


            }

        }
    }
}