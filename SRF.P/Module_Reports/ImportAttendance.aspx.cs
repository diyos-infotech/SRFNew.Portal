using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.IO;
using System.Data.OleDb;
using KLTS.Data;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class ImportAttendance : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";

        GridViewExportUtil util = new GridViewExportUtil();
        AppConfiguration config = new AppConfiguration();

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




                string sqlemptydata = "select * from tblEmptyforExcel";
                DataTable dtempty = config.ExecuteAdaptorAsyncWithQueryParams(sqlemptydata).Result;
                if (dtempty.Rows.Count > 0)
                {
                    SampleGrid.DataSource = dtempty;
                    SampleGrid.DataBind();

                    grvSample2.DataSource = dtempty;
                    grvSample2.DataBind();
                }

                System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath("ImportDocuments"));

                foreach (FileInfo file in di.GetFiles())
                {

                    file.Delete();
                }
            }
        }

        public void getemployeeattendance()
        {

            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string Month = month + Year.Substring(2, 2);

            string qry = "select ea.EmpId,(ed.empfname+' '+ed.empmname+' '+ed.emplname) as fullname,Design,NoOfDuties from empattendance ea inner join empdetails ed on ea.empid=ed.empid where month='" + Month + "' and excelnumber=";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dt.Rows.Count > 0)
            {
                GVEmployeeList.DataSource = dt;
                GVEmployeeList.DataBind();
            }
            else
            {
                GVEmployeeList.DataSource = null;
                GVEmployeeList.DataBind();
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


        public void GoToLoginPage()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
            Response.Redirect("~/login.aspx");
        }

        protected void ddlClientID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridView2.DataSource = null;
                GridView2.DataBind();
                gvAttendancestatus.DataSource = null;
                gvAttendancestatus.DataBind();
                GridView1.DataSource = null;
                GridView1.DataBind();
                // ddlMonth.SelectedIndex = 0;
                txtmonth.Text = "";
                ddlContractId.Items.Clear();
                // ddlContractId.SelectedIndex = 0; ;
                lblMessage.Text = string.Empty;
                if (ddlClientID.SelectedIndex > 0)
                {

                    ddlCName.SelectedValue = ddlClientID.SelectedValue;
                    lblMessage.Text = string.Empty;
                    //displaydata();
                }
                else
                {
                    //ClearData();

                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlCName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridView2.DataSource = null;
                GridView2.DataBind();
                gvAttendancestatus.DataSource = null;
                gvAttendancestatus.DataBind();
                GridView1.DataSource = null;
                GridView1.DataBind();
                // ddlMonth.SelectedIndex = 0;
                txtmonth.Text = "";
                lblMessage.Text = string.Empty;
                ddlContractId.Items.Clear();
                //ddlContractId.SelectedIndex = 0;
                if (ddlCName.SelectedIndex > 0)
                {
                    ddlClientID.SelectedValue = ddlCName.SelectedValue;
                    lblMessage.Text = string.Empty;

                    //displaydataFormClientName();
                }
                else
                {
                    //ClearData();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void txtmonth_TextChanged(object sender, EventArgs e)
        {

            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView2.DataSource = null;
            GridView2.DataBind();
            gvAttendancestatus.DataSource = null;
            gvAttendancestatus.DataBind();
            lblTotalDuties.Text = string.Empty;
            lblTotalOts.Text = string.Empty;
            lblMessage.Text = string.Empty;
            ddlContractId.Items.Clear();
            // ddlContractId.SelectedIndex = 0;
            if (txtmonth.Text.Trim().Length != 0)
            {
                // displaydata();
                FillAttendanceGrid();
                DismatchDesignation();
                lblMessage.Text = string.Empty;
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                lblMessage.Text = string.Empty;
                GridView2.DataSource = null;
                GridView2.DataBind();
                gvAttendancestatus.DataSource = null;
                gvAttendancestatus.DataBind();

            }
        }

        protected void FillAttendanceGrid()
        {

            btnExportExcel.Visible = false;

            if (ddlClientID.SelectedIndex > 0)
            {
                try
                {
                    //int month = 0;

                    //if (ddlMonth.SelectedIndex == 1)
                    //{
                    //    month = GlobalData.Instance.GetIDForNextMonth();
                    //}
                    //if (ddlMonth.SelectedIndex == 2)
                    //{
                    //    month = GlobalData.Instance.GetIDForThisMonth();
                    //}
                    //if (ddlMonth.SelectedIndex == 3)
                    //{
                    //    month = GlobalData.Instance.GetIDForPrviousMonth();
                    //}


                    string date = string.Empty;

                    if (txtmonth.Text.Trim().Length > 0)
                    {
                        date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                    }

                    string month = DateTime.Parse(date).Month.ToString();
                    string Year = DateTime.Parse(date).Year.ToString();

                    string Month = month + Year.Substring(2, 2);
                    string Clientid = ddlClientID.SelectedValue;

                    DataTable data = new DataTable();
                    string SpName = "EmpattendanceMonthlywise";
                    Hashtable HtAttendance = new Hashtable();
                    HtAttendance.Add("@month", Month);



                    data = config.ExecuteAdaptorAsyncWithParams(SpName, HtAttendance).Result;

                    if (data.Rows.Count > 0)
                    {
                        //commented by swathi on 30-11-2015

                        // btnExportExcel.Visible = true;

                        GridView1.DataSource = data;
                        GridView1.DataBind();
                    }
                    else
                    {
                        GridView1.DataSource = null;
                        GridView1.DataBind();
                    }

                    string SpName1 = "TotalempAttendanceDeisgnandmonthlywise";
                    Hashtable HtTotaldata = new Hashtable();
                    HtTotaldata.Add("@month", Month);

                    DataTable dtTotaldata = config.ExecuteAdaptorAsyncWithParams(SpName1, HtTotaldata).Result;

                    if (dtTotaldata.Rows.Count > 0)
                    {
                        gvAttendancestatus.DataSource = dtTotaldata;
                        gvAttendancestatus.DataBind();
                    }
                    else
                    {
                        gvAttendancestatus.DataSource = null;
                        gvAttendancestatus.DataBind();
                    }

                }
                catch (Exception ex)
                {
                }
            }

        }


        #region Variables For Footer Total Values


        float Totalday1 = 0;
        float Totalday2 = 0;
        float Totalday3 = 0;
        float Totalday4 = 0;
        float Totalday5 = 0;
        float Totalday6 = 0;
        float Totalday7 = 0;
        float Totalday8 = 0;
        float Totalday9 = 0;
        float Totalday10 = 0;
        float Totalday11 = 0;
        float Totalday12 = 0;
        float Totalday13 = 0;
        float Totalday14 = 0;
        float Totalday15 = 0;
        float Totalday16 = 0;
        float Totalday17 = 0;
        float Totalday18 = 0;
        float Totalday19 = 0;
        float Totalday20 = 0;
        float Totalday21 = 0;
        float Totalday22 = 0;
        float Totalday23 = 0;
        float Totalday24 = 0;
        float Totalday25 = 0;
        float Totalday26 = 0;
        float Totalday27 = 0;
        float Totalday28 = 0;
        float Totalday29 = 0;
        float Totalday30 = 0;
        float Totalday31 = 0;


        float TotalDuties = 0;
        float TotalWos = 0;
        float TotalNHS = 0;
        float TotalOts = 0;
        float TotalCantAdv = 0;
        float TotalPenalty = 0;
        float TotalIncentives = 0;

        float GrandTotal = 0;

        float TotalNa = 0;
        float TotalAb = 0;

        #endregion

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // int month = 0;
            int days = 0;
            try
            {
                //if (ddlMonth.SelectedIndex == 1)
                //{
                //    month = GlobalData.Instance.GetIDForNextMonth();
                //    days = GlobalData.Instance.GetNoOfDaysForNextMonth();
                //}
                //if (ddlMonth.SelectedIndex == 2)
                //{
                //    month = GlobalData.Instance.GetIDForThisMonth();
                //    days = GlobalData.Instance.GetNoOfDaysForThisMonth();
                //}
                //if (ddlMonth.SelectedIndex == 3)
                //{
                //    month = GlobalData.Instance.GetIDForPrviousMonth();
                //    days = GlobalData.Instance.GetNoOfDaysForPrviousMonth();
                //}

                var ContractID = "";
                var bBillDates = 0;
                //var LastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlMonth.SelectedIndex);
                var LastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                string date = string.Empty;

                if (txtmonth.Text.Trim().Length > 0)
                {
                    date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                }

                string month = DateTime.Parse(date).Month.ToString();
                string Year = DateTime.Parse(date).Year.ToString();

                string Month = month + Year.Substring(2, 2);


                #region  Begin Get Contract Id Based on The Last Day


                Hashtable HtGetContractID = new Hashtable();
                var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                HtGetContractID.Add("@clientid", ddlClientID.SelectedValue);
                HtGetContractID.Add("@LastDay", Month);
                DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                if (DTContractID.Rows.Count > 0)
                {
                    ContractID = DTContractID.Rows[0]["contractid"].ToString();
                    bBillDates = int.Parse(DTContractID.Rows[0]["BillDates"].ToString());
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not  Avaialable For This Client.');", true);
                    return;
                }

                #endregion  End Get Contract Id Based on The Last Day

                if (e.Row.RowType == DataControlRowType.Header)
                {

                    #region If Bill Dates 26 to 25


                    if (bBillDates == 2)
                    {
                        e.Row.Cells[4].Text = "26";
                        e.Row.Cells[5].Text = "27";
                        e.Row.Cells[6].Text = "28";
                        e.Row.Cells[7].Text = "29";
                        e.Row.Cells[8].Text = "30";

                        e.Row.Cells[9].Text = "31";

                        e.Row.Cells[10].Text = "1";
                        e.Row.Cells[11].Text = "2";
                        e.Row.Cells[12].Text = "3";
                        e.Row.Cells[13].Text = "4";
                        e.Row.Cells[14].Text = "5";

                        e.Row.Cells[15].Text = "6";
                        e.Row.Cells[16].Text = "7";
                        e.Row.Cells[17].Text = "8";
                        e.Row.Cells[18].Text = "9";
                        e.Row.Cells[19].Text = "10";

                        e.Row.Cells[20].Text = "11";
                        e.Row.Cells[21].Text = "12";
                        e.Row.Cells[22].Text = "13";
                        e.Row.Cells[23].Text = "14";
                        e.Row.Cells[24].Text = "15";

                        e.Row.Cells[25].Text = "16";
                        e.Row.Cells[26].Text = "17";
                        e.Row.Cells[27].Text = "18";
                        e.Row.Cells[28].Text = "19";
                        e.Row.Cells[29].Text = "20";

                        e.Row.Cells[30].Text = "21";
                        e.Row.Cells[31].Text = "22";
                        e.Row.Cells[32].Text = "23";
                        e.Row.Cells[33].Text = "24";
                        e.Row.Cells[34].Text = "25";

                    }

                    #endregion

                    #region If Bill Dates 21 to 20


                    if (bBillDates == 3)
                    {
                        e.Row.Cells[4].Text = "21";
                        e.Row.Cells[5].Text = "22";
                        e.Row.Cells[6].Text = "23";
                        e.Row.Cells[7].Text = "24";
                        e.Row.Cells[8].Text = "25";

                        e.Row.Cells[9].Text = "26";
                        e.Row.Cells[10].Text = "27";
                        e.Row.Cells[11].Text = "28";
                        e.Row.Cells[12].Text = "29";
                        e.Row.Cells[13].Text = "30";

                        e.Row.Cells[14].Text = "31";

                        e.Row.Cells[15].Text = "1";
                        e.Row.Cells[16].Text = "2";
                        e.Row.Cells[17].Text = "3";
                        e.Row.Cells[18].Text = "4";
                        e.Row.Cells[19].Text = "5";

                        e.Row.Cells[20].Text = "6";
                        e.Row.Cells[21].Text = "7";
                        e.Row.Cells[22].Text = "8";
                        e.Row.Cells[23].Text = "9";
                        e.Row.Cells[24].Text = "10";

                        e.Row.Cells[25].Text = "11";
                        e.Row.Cells[26].Text = "12";
                        e.Row.Cells[27].Text = "13";
                        e.Row.Cells[28].Text = "14";
                        e.Row.Cells[29].Text = "15";

                        e.Row.Cells[30].Text = "16";
                        e.Row.Cells[31].Text = "17";
                        e.Row.Cells[32].Text = "18";
                        e.Row.Cells[33].Text = "19";
                        e.Row.Cells[34].Text = "20";

                    }

                    #endregion


                    if (days == 30)
                    {
                        e.Row.Cells[34].Visible = false;


                        #region If Bill Dates 26 to 25


                        if (bBillDates == 2)
                        {
                            e.Row.Cells[4].Text = "26";
                            e.Row.Cells[5].Text = "27";
                            e.Row.Cells[6].Text = "28";
                            e.Row.Cells[7].Text = "29";
                            e.Row.Cells[8].Text = "30";

                            e.Row.Cells[9].Text = "1";
                            e.Row.Cells[10].Text = "2";
                            e.Row.Cells[11].Text = "3";
                            e.Row.Cells[12].Text = "4";
                            e.Row.Cells[13].Text = "5";

                            e.Row.Cells[14].Text = "6";
                            e.Row.Cells[15].Text = "7";
                            e.Row.Cells[16].Text = "8";
                            e.Row.Cells[17].Text = "9";
                            e.Row.Cells[18].Text = "10";

                            e.Row.Cells[19].Text = "11";
                            e.Row.Cells[20].Text = "12";
                            e.Row.Cells[21].Text = "13";
                            e.Row.Cells[22].Text = "14";
                            e.Row.Cells[23].Text = "15";

                            e.Row.Cells[24].Text = "16";
                            e.Row.Cells[25].Text = "17";
                            e.Row.Cells[26].Text = "18";
                            e.Row.Cells[27].Text = "19";
                            e.Row.Cells[28].Text = "20";

                            e.Row.Cells[29].Text = "21";
                            e.Row.Cells[30].Text = "22";
                            e.Row.Cells[31].Text = "23";
                            e.Row.Cells[32].Text = "24";
                            e.Row.Cells[33].Text = "25";

                        }

                        #endregion

                        #region If Bill Dates 21 to 20


                        if (bBillDates == 3)
                        {
                            e.Row.Cells[4].Text = "21";
                            e.Row.Cells[5].Text = "22";
                            e.Row.Cells[6].Text = "23";
                            e.Row.Cells[7].Text = "24";
                            e.Row.Cells[8].Text = "25";

                            e.Row.Cells[9].Text = "26";
                            e.Row.Cells[10].Text = "27";
                            e.Row.Cells[11].Text = "28";
                            e.Row.Cells[12].Text = "29";
                            e.Row.Cells[13].Text = "30";

                            e.Row.Cells[14].Text = "1";
                            e.Row.Cells[15].Text = "2";
                            e.Row.Cells[16].Text = "3";
                            e.Row.Cells[17].Text = "4";
                            e.Row.Cells[18].Text = "5";

                            e.Row.Cells[19].Text = "6";
                            e.Row.Cells[20].Text = "7";
                            e.Row.Cells[21].Text = "8";
                            e.Row.Cells[22].Text = "9";
                            e.Row.Cells[23].Text = "10";

                            e.Row.Cells[24].Text = "11";
                            e.Row.Cells[25].Text = "12";
                            e.Row.Cells[26].Text = "13";
                            e.Row.Cells[27].Text = "14";
                            e.Row.Cells[28].Text = "15";

                            e.Row.Cells[29].Text = "16";
                            e.Row.Cells[30].Text = "17";
                            e.Row.Cells[31].Text = "18";
                            e.Row.Cells[32].Text = "19";
                            e.Row.Cells[33].Text = "20";

                        }

                        #endregion

                    }
                    if (days == 28)
                    {
                        e.Row.Cells[34].Visible = false;
                        e.Row.Cells[33].Visible = false;
                        e.Row.Cells[32].Visible = false;

                        #region If Bill Dates 26 to 25


                        if (bBillDates == 2)
                        {
                            e.Row.Cells[4].Text = "26";
                            e.Row.Cells[5].Text = "27";
                            e.Row.Cells[6].Text = "28";

                            e.Row.Cells[7].Text = "1";
                            e.Row.Cells[8].Text = "2";
                            e.Row.Cells[9].Text = "3";
                            e.Row.Cells[10].Text = "4";
                            e.Row.Cells[11].Text = "5";

                            e.Row.Cells[12].Text = "6";
                            e.Row.Cells[13].Text = "7";
                            e.Row.Cells[14].Text = "8";
                            e.Row.Cells[15].Text = "9";
                            e.Row.Cells[16].Text = "10";

                            e.Row.Cells[17].Text = "11";
                            e.Row.Cells[18].Text = "12";
                            e.Row.Cells[19].Text = "13";
                            e.Row.Cells[20].Text = "14";
                            e.Row.Cells[21].Text = "15";

                            e.Row.Cells[22].Text = "16";
                            e.Row.Cells[23].Text = "17";
                            e.Row.Cells[24].Text = "18";
                            e.Row.Cells[25].Text = "19";
                            e.Row.Cells[26].Text = "20";

                            e.Row.Cells[27].Text = "21";
                            e.Row.Cells[28].Text = "22";
                            e.Row.Cells[29].Text = "23";
                            e.Row.Cells[30].Text = "24";
                            e.Row.Cells[31].Text = "25";

                        }

                        #endregion

                        #region If Bill Dates 21 to 20


                        if (bBillDates == 3)
                        {
                            e.Row.Cells[4].Text = "21";
                            e.Row.Cells[5].Text = "22";
                            e.Row.Cells[6].Text = "23";
                            e.Row.Cells[7].Text = "24";
                            e.Row.Cells[8].Text = "25";

                            e.Row.Cells[9].Text = "26";
                            e.Row.Cells[10].Text = "27";
                            e.Row.Cells[11].Text = "28";

                            e.Row.Cells[12].Text = "1";
                            e.Row.Cells[13].Text = "2";
                            e.Row.Cells[14].Text = "3";
                            e.Row.Cells[15].Text = "4";
                            e.Row.Cells[16].Text = "5";

                            e.Row.Cells[17].Text = "6";
                            e.Row.Cells[18].Text = "7";
                            e.Row.Cells[19].Text = "8";
                            e.Row.Cells[20].Text = "9";
                            e.Row.Cells[21].Text = "10";

                            e.Row.Cells[22].Text = "11";
                            e.Row.Cells[23].Text = "12";
                            e.Row.Cells[24].Text = "13";
                            e.Row.Cells[25].Text = "14";
                            e.Row.Cells[26].Text = "15";

                            e.Row.Cells[27].Text = "16";
                            e.Row.Cells[28].Text = "17";
                            e.Row.Cells[29].Text = "18";
                            e.Row.Cells[30].Text = "19";
                            e.Row.Cells[31].Text = "20";

                        }

                        #endregion

                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (days == 30)
                    {
                        e.Row.Cells[34].Visible = false;
                    }
                    if (days == 28)
                    {
                        e.Row.Cells[34].Visible = false;
                        e.Row.Cells[33].Visible = false;
                        e.Row.Cells[32].Visible = false;
                    }


                    #region Days values for Footer Total

                    #region Day1

                    float day1 = 0;
                    float day1ot = 0;

                    Label txtday1 = e.Row.FindControl("txtday1") as Label;
                    Label txtday1ot = e.Row.FindControl("txtday1ot") as Label;

                    if (txtday1.Text == "P" || txtday1.Text == "W" || txtday1.Text == "p" || txtday1.Text == "w")
                    {
                        day1 = 1;
                    }
                    if (txtday1.Text == "H" || txtday1.Text == "h")
                    {
                        day1 = 0.5f;
                    }
                    if (txtday1.Text == "J" || txtday1.Text == "j")
                    {
                        day1 = 1.5f;
                    }

                    if (txtday1ot.Text == "P" || txtday1ot.Text == "W" || txtday1ot.Text == "p" || txtday1ot.Text == "w")
                    {
                        day1ot = 1;
                    }
                    if (txtday1ot.Text == "H" || txtday1ot.Text == "h")
                    {
                        day1ot = 0.5f;
                    }
                    if (txtday1ot.Text == "J" || txtday1ot.Text == "j")
                    {
                        day1ot = 1.5f;
                    }

                    Totalday1 += (day1 + day1ot);

                    #endregion


                    #region Day2

                    float day2 = 0;
                    float day2ot = 0;

                    Label txtday2 = e.Row.FindControl("txtday2") as Label;
                    Label txtday2ot = e.Row.FindControl("txtday2ot") as Label;
                    if (txtday2.Text == "P" || txtday2.Text == "W" || txtday2.Text == "p" || txtday2.Text == "w")
                    {
                        day2 = 1;
                    }
                    if (txtday2.Text == "H" || txtday2.Text == "h")
                    {
                        day2 = 0.5f;
                    }
                    if (txtday2.Text == "J" || txtday2.Text == "j")
                    {
                        day2 = 1.5f;
                    }

                    if (txtday2ot.Text == "P" || txtday2ot.Text == "W" || txtday2ot.Text == "p" || txtday2ot.Text == "w")
                    {
                        day2ot = 1;
                    }
                    if (txtday2ot.Text == "H" || txtday2ot.Text == "h")
                    {
                        day2ot = 0.5f;
                    }
                    if (txtday2ot.Text == "J" || txtday2ot.Text == "j")
                    {
                        day2ot = 1.5f;
                    }

                    Totalday2 += (day2 + day2ot);

                    #endregion

                    #region Day3

                    float day3 = 0;
                    float day3ot = 0;

                    Label txtday3 = e.Row.FindControl("txtday3") as Label;
                    Label txtday3ot = e.Row.FindControl("txtday3ot") as Label;
                    if (txtday3.Text == "P" || txtday3.Text == "W" || txtday3.Text == "p" || txtday3.Text == "w")
                    {
                        day3 = 1;
                    }
                    if (txtday3.Text == "H" || txtday3.Text == "h")
                    {
                        day3 = 0.5f;
                    }
                    if (txtday3.Text == "J" || txtday3.Text == "j")
                    {
                        day3 = 1.5f;
                    }

                    if (txtday3ot.Text == "P" || txtday3ot.Text == "W" || txtday3ot.Text == "p" || txtday3ot.Text == "w")
                    {
                        day3ot = 1;
                    }
                    if (txtday3ot.Text == "H" || txtday3ot.Text == "h")
                    {
                        day3ot = 0.5f;
                    }
                    if (txtday3ot.Text == "J" || txtday3ot.Text == "j")
                    {
                        day3ot = 1.5f;
                    }

                    Totalday3 += (day3 + day3ot);

                    #endregion

                    #region Day4

                    float day4 = 0;
                    float day4ot = 0;

                    Label txtday4 = e.Row.FindControl("txtday4") as Label;
                    Label txtday4ot = e.Row.FindControl("txtday4ot") as Label;
                    if (txtday4.Text == "P" || txtday4.Text == "W" || txtday4.Text == "p" || txtday4.Text == "w")
                    {
                        day4 = 1;
                    }
                    if (txtday4.Text == "H" || txtday4.Text == "h")
                    {
                        day4 = 0.5f;
                    }
                    if (txtday4.Text == "J" || txtday4.Text == "j")
                    {
                        day4 = 1.5f;
                    }

                    if (txtday4ot.Text == "P" || txtday4ot.Text == "W" || txtday4ot.Text == "p" || txtday4ot.Text == "w")
                    {
                        day4ot = 1;
                    }
                    if (txtday4ot.Text == "H" || txtday4ot.Text == "h")
                    {
                        day4ot = 0.5f;
                    }
                    if (txtday4ot.Text == "J" || txtday4ot.Text == "j")
                    {
                        day4ot = 1.5f;
                    }

                    Totalday4 += (day4 + day4ot);

                    #endregion

                    #region Day5

                    float day5 = 0;
                    float day5ot = 0;

                    Label txtday5 = e.Row.FindControl("txtday5") as Label;
                    Label txtday5ot = e.Row.FindControl("txtday5ot") as Label;
                    if (txtday5.Text == "P" || txtday5.Text == "W" || txtday5.Text == "p" || txtday5.Text == "w")
                    {
                        day5 = 1;
                    }
                    if (txtday5.Text == "H" || txtday5.Text == "h")
                    {
                        day5 = 0.5f;
                    }
                    if (txtday5.Text == "J" || txtday5.Text == "j")
                    {
                        day5 = 1.5f;
                    }

                    if (txtday5ot.Text == "P" || txtday5ot.Text == "W" || txtday5ot.Text == "p" || txtday5ot.Text == "w")
                    {
                        day5ot = 1;
                    }
                    if (txtday5ot.Text == "H" || txtday5ot.Text == "h")
                    {
                        day5ot = 0.5f;
                    }
                    if (txtday5ot.Text == "J" || txtday5ot.Text == "j")
                    {
                        day5ot = 1.5f;
                    }

                    Totalday5 += (day5 + day5ot);

                    #endregion

                    #region Day6

                    float day6 = 0;
                    float day6ot = 0;

                    Label txtday6 = e.Row.FindControl("txtday6") as Label;
                    Label txtday6ot = e.Row.FindControl("txtday6ot") as Label;
                    if (txtday6.Text == "P" || txtday6.Text == "W" || txtday6.Text == "p" || txtday6.Text == "w")
                    {
                        day6 = 1;
                    }
                    if (txtday6.Text == "H" || txtday6.Text == "h")
                    {
                        day6 = 0.5f;
                    }
                    if (txtday6.Text == "J" || txtday6.Text == "j")
                    {
                        day6 = 1.5f;
                    }

                    if (txtday6ot.Text == "P" || txtday6ot.Text == "W" || txtday6ot.Text == "p" || txtday6ot.Text == "w")
                    {
                        day6ot = 1;
                    }
                    if (txtday6ot.Text == "H" || txtday6ot.Text == "h")
                    {
                        day6ot = 0.5f;
                    }
                    if (txtday6ot.Text == "J" || txtday6ot.Text == "j")
                    {
                        day6ot = 1.5f;
                    }

                    Totalday6 += (day6 + day6ot);

                    #endregion

                    #region Day7

                    float day7 = 0;
                    float day7ot = 0;

                    Label txtday7 = e.Row.FindControl("txtday7") as Label;
                    Label txtday7ot = e.Row.FindControl("txtday7ot") as Label;
                    if (txtday7.Text == "P" || txtday7.Text == "W" || txtday7.Text == "p" || txtday7.Text == "w")
                    {
                        day7 = 1;
                    }
                    if (txtday7.Text == "H" || txtday7.Text == "h")
                    {
                        day7 = 0.5f;
                    }
                    if (txtday7.Text == "J" || txtday7.Text == "j")
                    {
                        day7 = 1.5f;
                    }

                    if (txtday7ot.Text == "P" || txtday7ot.Text == "W" || txtday7ot.Text == "p" || txtday7ot.Text == "w")
                    {
                        day7ot = 1;
                    }
                    if (txtday7ot.Text == "H" || txtday7ot.Text == "h")
                    {
                        day7ot = 0.5f;
                    }
                    if (txtday7ot.Text == "J" || txtday7ot.Text == "j")
                    {
                        day7ot = 1.5f;
                    }

                    Totalday7 += (day7 + day7ot);

                    #endregion

                    #region Day8

                    float day8 = 0;
                    float day8ot = 0;

                    Label txtday8 = e.Row.FindControl("txtday8") as Label;
                    Label txtday8ot = e.Row.FindControl("txtday8ot") as Label;
                    if (txtday8.Text == "P" || txtday8.Text == "W" || txtday8.Text == "p" || txtday8.Text == "w")
                    {
                        day8 = 1;
                    }
                    if (txtday8.Text == "H" || txtday8.Text == "h")
                    {
                        day8 = 0.5f;
                    }
                    if (txtday8.Text == "J" || txtday8.Text == "j")
                    {
                        day8 = 1.5f;
                    }

                    if (txtday8ot.Text == "P" || txtday8ot.Text == "W" || txtday8ot.Text == "p" || txtday8ot.Text == "w")
                    {
                        day8ot = 1;
                    }
                    if (txtday8ot.Text == "H" || txtday8ot.Text == "h")
                    {
                        day8ot = 0.5f;
                    }
                    if (txtday8ot.Text == "J" || txtday8ot.Text == "j")
                    {
                        day8ot = 1.5f;
                    }

                    Totalday8 += (day8 + day8ot);

                    #endregion

                    #region Day9

                    float day9 = 0;
                    float day9ot = 0;

                    Label txtday9 = e.Row.FindControl("txtday9") as Label;
                    Label txtday9ot = e.Row.FindControl("txtday9ot") as Label;
                    if (txtday9.Text == "P" || txtday9.Text == "W" || txtday9.Text == "p" || txtday9.Text == "w")
                    {
                        day9 = 1;
                    }
                    if (txtday9.Text == "H" || txtday9.Text == "h")
                    {
                        day9 = 0.5f;
                    }
                    if (txtday9.Text == "J" || txtday9.Text == "j")
                    {
                        day9 = 1.5f;
                    }

                    if (txtday9ot.Text == "P" || txtday9ot.Text == "W" || txtday9ot.Text == "p" || txtday9ot.Text == "w")
                    {
                        day9ot = 1;
                    }
                    if (txtday9ot.Text == "H" || txtday9ot.Text == "h")
                    {
                        day9ot = 0.5f;
                    }
                    if (txtday9ot.Text == "J" || txtday9ot.Text == "j")
                    {
                        day9ot = 1.5f;
                    }

                    Totalday9 += (day9 + day9ot);

                    #endregion

                    #region Day10

                    float day10 = 0;
                    float day10ot = 0;

                    Label txtday10 = e.Row.FindControl("txtday10") as Label;
                    Label txtday10ot = e.Row.FindControl("txtday10ot") as Label;
                    if (txtday10.Text == "P" || txtday10.Text == "W" || txtday10.Text == "p" || txtday10.Text == "w")
                    {
                        day10 = 1;
                    }
                    if (txtday10.Text == "H" || txtday10.Text == "h")
                    {
                        day10 = 0.5f;
                    }
                    if (txtday10.Text == "J" || txtday10.Text == "j")
                    {
                        day10 = 1.5f;
                    }

                    if (txtday10ot.Text == "P" || txtday10ot.Text == "W" || txtday10ot.Text == "p" || txtday10ot.Text == "w")
                    {
                        day10ot = 1;
                    }
                    if (txtday10ot.Text == "H" || txtday10ot.Text == "h")
                    {
                        day10ot = 0.5f;
                    }
                    if (txtday10ot.Text == "J" || txtday10ot.Text == "j")
                    {
                        day10ot = 1.5f;
                    }

                    Totalday10 += (day10 + day10ot);

                    #endregion

                    #region Day11

                    float day11 = 0;
                    float day11ot = 0;

                    Label txtday11 = e.Row.FindControl("txtday11") as Label;
                    Label txtday11ot = e.Row.FindControl("txtday11ot") as Label;
                    if (txtday11.Text == "P" || txtday11.Text == "W" || txtday11.Text == "p" || txtday11.Text == "w")
                    {
                        day11 = 1;
                    }
                    if (txtday11.Text == "H" || txtday11.Text == "h")
                    {
                        day11 = 0.5f;
                    }
                    if (txtday11.Text == "J" || txtday11.Text == "j")
                    {
                        day11 = 1.5f;
                    }

                    if (txtday11ot.Text == "P" || txtday11ot.Text == "W" || txtday11ot.Text == "p" || txtday11ot.Text == "w")
                    {
                        day11ot = 1;
                    }
                    if (txtday11ot.Text == "H" || txtday11ot.Text == "h")
                    {
                        day11ot = 0.5f;
                    }
                    if (txtday11ot.Text == "J" || txtday11ot.Text == "j")
                    {
                        day11ot = 1.5f;
                    }

                    Totalday11 += (day11 + day11ot);

                    #endregion

                    #region Day12

                    float day12 = 0;
                    float day12ot = 0;

                    Label txtday12 = e.Row.FindControl("txtday12") as Label;
                    Label txtday12ot = e.Row.FindControl("txtday12ot") as Label;
                    if (txtday12.Text == "P" || txtday12.Text == "W" || txtday12.Text == "p" || txtday12.Text == "w")
                    {
                        day12 = 1;
                    }
                    if (txtday12.Text == "H" || txtday12.Text == "h")
                    {
                        day12 = 0.5f;
                    }
                    if (txtday12.Text == "J" || txtday12.Text == "j")
                    {
                        day12 = 1.5f;
                    }

                    if (txtday12ot.Text == "P" || txtday12ot.Text == "W" || txtday12ot.Text == "p" || txtday12ot.Text == "w")
                    {
                        day12ot = 1;
                    }
                    if (txtday12ot.Text == "H" || txtday12ot.Text == "h")
                    {
                        day12ot = 0.5f;
                    }
                    if (txtday12ot.Text == "J" || txtday12ot.Text == "j")
                    {
                        day12ot = 1.5f;
                    }

                    Totalday12 += (day12 + day12ot);

                    #endregion

                    #region Day13

                    float day13 = 0;
                    float day13ot = 0;

                    Label txtday13 = e.Row.FindControl("txtday13") as Label;
                    Label txtday13ot = e.Row.FindControl("txtday13ot") as Label;
                    if (txtday13.Text == "P" || txtday13.Text == "W" || txtday13.Text == "p" || txtday13.Text == "w")
                    {
                        day13 = 1;
                    }
                    if (txtday13.Text == "H" || txtday13.Text == "h")
                    {
                        day13 = 0.5f;
                    }
                    if (txtday13.Text == "J" || txtday13.Text == "j")
                    {
                        day13 = 1.5f;
                    }

                    if (txtday13ot.Text == "P" || txtday13ot.Text == "W" || txtday13ot.Text == "p" || txtday13ot.Text == "w")
                    {
                        day13ot = 1;
                    }
                    if (txtday13ot.Text == "H" || txtday13ot.Text == "h")
                    {
                        day13ot = 0.5f;
                    }
                    if (txtday13ot.Text == "J" || txtday13ot.Text == "j")
                    {
                        day13ot = 1.5f;
                    }

                    Totalday13 += (day13 + day13ot);

                    #endregion

                    #region Day14

                    float day14 = 0;
                    float day14ot = 0;

                    Label txtday14 = e.Row.FindControl("txtday14") as Label;
                    Label txtday14ot = e.Row.FindControl("txtday14ot") as Label;
                    if (txtday14.Text == "P" || txtday14.Text == "W" || txtday14.Text == "p" || txtday14.Text == "w")
                    {
                        day14 = 1;
                    }
                    if (txtday14.Text == "H" || txtday14.Text == "h")
                    {
                        day14 = 0.5f;
                    }
                    if (txtday14.Text == "J" || txtday14.Text == "j")
                    {
                        day14 = 1.5f;
                    }

                    if (txtday14ot.Text == "P" || txtday14ot.Text == "W" || txtday14ot.Text == "p" || txtday14ot.Text == "w")
                    {
                        day14ot = 1;
                    }
                    if (txtday14ot.Text == "H" || txtday14ot.Text == "h")
                    {
                        day14ot = 0.5f;
                    }
                    if (txtday14ot.Text == "J" || txtday14ot.Text == "j")
                    {
                        day14ot = 1.5f;
                    }

                    Totalday14 += (day14 + day14ot);

                    #endregion

                    #region Day15

                    float day15 = 0;
                    float day15ot = 0;

                    Label txtday15 = e.Row.FindControl("txtday15") as Label;
                    Label txtday15ot = e.Row.FindControl("txtday15ot") as Label;
                    if (txtday15.Text == "P" || txtday15.Text == "W" || txtday15.Text == "p" || txtday15.Text == "w")
                    {
                        day15 = 1;
                    }
                    if (txtday15.Text == "H" || txtday15.Text == "h")
                    {
                        day15 = 0.5f;
                    }
                    if (txtday15.Text == "J" || txtday15.Text == "j")
                    {
                        day15 = 1.5f;
                    }

                    if (txtday15ot.Text == "P" || txtday15ot.Text == "W" || txtday15ot.Text == "p" || txtday15ot.Text == "w")
                    {
                        day15ot = 1;
                    }
                    if (txtday15ot.Text == "H" || txtday15ot.Text == "h")
                    {
                        day15ot = 0.5f;
                    }
                    if (txtday15ot.Text == "J" || txtday15ot.Text == "j")
                    {
                        day15ot = 1.5f;
                    }

                    Totalday15 += (day15 + day15ot);

                    #endregion

                    #region Day16

                    float day16 = 0;
                    float day16ot = 0;

                    Label txtday16 = e.Row.FindControl("txtday16") as Label;
                    Label txtday16ot = e.Row.FindControl("txtday16ot") as Label;
                    if (txtday16.Text == "P" || txtday16.Text == "W" || txtday16.Text == "p" || txtday16.Text == "w")
                    {
                        day16 = 1;
                    }
                    if (txtday16.Text == "H" || txtday16.Text == "h")
                    {
                        day16 = 0.5f;
                    }
                    if (txtday16.Text == "J" || txtday16.Text == "j")
                    {
                        day16 = 1.5f;
                    }

                    if (txtday16ot.Text == "P" || txtday16ot.Text == "W" || txtday16ot.Text == "p" || txtday16ot.Text == "w")
                    {
                        day16ot = 1;
                    }
                    if (txtday16ot.Text == "H" || txtday16ot.Text == "h")
                    {
                        day16ot = 0.5f;
                    }
                    if (txtday16ot.Text == "J" || txtday16ot.Text == "j")
                    {
                        day16ot = 1.5f;
                    }

                    Totalday16 += (day16 + day16ot);

                    #endregion

                    #region Day17

                    float day17 = 0;
                    float day17ot = 0;

                    Label txtday17 = e.Row.FindControl("txtday17") as Label;
                    Label txtday17ot = e.Row.FindControl("txtday17ot") as Label;
                    if (txtday17.Text == "P" || txtday17.Text == "W" || txtday17.Text == "p" || txtday17.Text == "w")
                    {
                        day17 = 1;
                    }
                    if (txtday17.Text == "H" || txtday17.Text == "h")
                    {
                        day17 = 0.5f;
                    }
                    if (txtday17.Text == "J" || txtday17.Text == "j")
                    {
                        day17 = 1.5f;
                    }

                    if (txtday17ot.Text == "P" || txtday17ot.Text == "W" || txtday17ot.Text == "p" || txtday17ot.Text == "w")
                    {
                        day17ot = 1;
                    }
                    if (txtday17ot.Text == "H" || txtday17ot.Text == "h")
                    {
                        day17ot = 0.5f;
                    }
                    if (txtday17ot.Text == "J" || txtday17ot.Text == "j")
                    {
                        day17ot = 1.5f;
                    }

                    Totalday17 += (day17 + day17ot);

                    #endregion

                    #region Day18

                    float day18 = 0;
                    float day18ot = 0;

                    Label txtday18 = e.Row.FindControl("txtday18") as Label;
                    Label txtday18ot = e.Row.FindControl("txtday18ot") as Label;
                    if (txtday18.Text == "P" || txtday18.Text == "W" || txtday18.Text == "p" || txtday18.Text == "w")
                    {
                        day18 = 1;
                    }
                    if (txtday18.Text == "H" || txtday18.Text == "h")
                    {
                        day18 = 0.5f;
                    }
                    if (txtday18.Text == "J" || txtday18.Text == "j")
                    {
                        day18 = 1.5f;
                    }

                    if (txtday18ot.Text == "P" || txtday18ot.Text == "W" || txtday18ot.Text == "p" || txtday18ot.Text == "w")
                    {
                        day18ot = 1;
                    }
                    if (txtday18ot.Text == "H" || txtday18ot.Text == "h")
                    {
                        day18ot = 0.5f;
                    }
                    if (txtday18ot.Text == "J" || txtday18ot.Text == "j")
                    {
                        day18ot = 1.5f;
                    }

                    Totalday18 += (day18 + day18ot);

                    #endregion

                    #region Day19

                    float day19 = 0;
                    float day19ot = 0;

                    Label txtday19 = e.Row.FindControl("txtday19") as Label;
                    Label txtday19ot = e.Row.FindControl("txtday19ot") as Label;
                    if (txtday19.Text == "P" || txtday19.Text == "W" || txtday19.Text == "p" || txtday19.Text == "w")
                    {
                        day19 = 1;
                    }
                    if (txtday19.Text == "H" || txtday19.Text == "h")
                    {
                        day19 = 0.5f;
                    }
                    if (txtday19.Text == "J" || txtday19.Text == "j")
                    {
                        day19 = 1.5f;
                    }

                    if (txtday19ot.Text == "P" || txtday19ot.Text == "W" || txtday19ot.Text == "p" || txtday19ot.Text == "w")
                    {
                        day19ot = 1;
                    }
                    if (txtday19ot.Text == "H" || txtday19ot.Text == "h")
                    {
                        day19ot = 0.5f;
                    }
                    if (txtday19ot.Text == "J" || txtday19ot.Text == "j")
                    {
                        day19ot = 1.5f;
                    }

                    Totalday19 += (day19 + day19ot);

                    #endregion

                    #region Day20

                    float day20 = 0;
                    float day20ot = 0;

                    Label txtday20 = e.Row.FindControl("txtday20") as Label;
                    Label txtday20ot = e.Row.FindControl("txtday20ot") as Label;
                    if (txtday20.Text == "P" || txtday20.Text == "W" || txtday20.Text == "p" || txtday20.Text == "w")
                    {
                        day20 = 1;
                    }
                    if (txtday20.Text == "H" || txtday20.Text == "h")
                    {
                        day20 = 0.5f;
                    }
                    if (txtday20.Text == "J" || txtday20.Text == "j")
                    {
                        day20 = 1.5f;
                    }

                    if (txtday20ot.Text == "P" || txtday20ot.Text == "W" || txtday20ot.Text == "p" || txtday20ot.Text == "w")
                    {
                        day20ot = 1;
                    }
                    if (txtday20ot.Text == "H" || txtday20ot.Text == "h")
                    {
                        day20ot = 0.5f;
                    }
                    if (txtday20ot.Text == "J" || txtday20ot.Text == "j")
                    {
                        day20ot = 1.5f;
                    }

                    Totalday20 += (day20 + day20ot);

                    #endregion

                    #region Day21

                    float day21 = 0;
                    float day21ot = 0;

                    Label txtday21 = e.Row.FindControl("txtday21") as Label;
                    Label txtday21ot = e.Row.FindControl("txtday21ot") as Label;
                    if (txtday21.Text == "P" || txtday21.Text == "W" || txtday21.Text == "p" || txtday21.Text == "w")
                    {
                        day21 = 1;
                    }
                    if (txtday21.Text == "H" || txtday21.Text == "h")
                    {
                        day21 = 0.5f;
                    }
                    if (txtday21.Text == "J" || txtday21.Text == "j")
                    {
                        day21 = 1.5f;
                    }

                    if (txtday21ot.Text == "P" || txtday21ot.Text == "W" || txtday21ot.Text == "p" || txtday21ot.Text == "w")
                    {
                        day21ot = 1;
                    }
                    if (txtday21ot.Text == "H" || txtday21ot.Text == "h")
                    {
                        day21ot = 0.5f;
                    }
                    if (txtday21ot.Text == "J" || txtday21ot.Text == "j")
                    {
                        day21ot = 1.5f;
                    }

                    Totalday21 += (day21 + day21ot);

                    #endregion

                    #region Day22

                    float day22 = 0;
                    float day22ot = 0;

                    Label txtday22 = e.Row.FindControl("txtday22") as Label;
                    Label txtday22ot = e.Row.FindControl("txtday22ot") as Label;
                    if (txtday22.Text == "P" || txtday22.Text == "W" || txtday22.Text == "p" || txtday22.Text == "w")
                    {
                        day22 = 1;
                    }
                    if (txtday22.Text == "H" || txtday22.Text == "h")
                    {
                        day22 = 0.5f;
                    }
                    if (txtday22.Text == "J" || txtday22.Text == "j")
                    {
                        day22 = 1.5f;
                    }

                    if (txtday22ot.Text == "P" || txtday22ot.Text == "W" || txtday22ot.Text == "p" || txtday22ot.Text == "w")
                    {
                        day22ot = 1;
                    }
                    if (txtday22ot.Text == "H" || txtday22ot.Text == "h")
                    {
                        day22ot = 0.5f;
                    }
                    if (txtday22ot.Text == "J" || txtday22ot.Text == "j")
                    {
                        day22ot = 1.5f;
                    }

                    Totalday22 += (day22 + day22ot);

                    #endregion

                    #region Day23

                    float day23 = 0;
                    float day23ot = 0;

                    Label txtday23 = e.Row.FindControl("txtday23") as Label;
                    Label txtday23ot = e.Row.FindControl("txtday23ot") as Label;
                    if (txtday23.Text == "P" || txtday23.Text == "W" || txtday23.Text == "p" || txtday23.Text == "w")
                    {
                        day23 = 1;
                    }
                    if (txtday23.Text == "H" || txtday23.Text == "h")
                    {
                        day23 = 0.5f;
                    }
                    if (txtday23.Text == "J" || txtday23.Text == "j")
                    {
                        day23 = 1.5f;
                    }

                    if (txtday23ot.Text == "P" || txtday23ot.Text == "W" || txtday23ot.Text == "p" || txtday23ot.Text == "w")
                    {
                        day23ot = 1;
                    }
                    if (txtday23ot.Text == "H" || txtday23ot.Text == "h")
                    {
                        day23ot = 0.5f;
                    }
                    if (txtday23ot.Text == "J" || txtday23ot.Text == "j")
                    {
                        day23ot = 1.5f;
                    }

                    Totalday23 += (day23 + day23ot);

                    #endregion

                    #region Day24

                    float day24 = 0;
                    float day24ot = 0;

                    Label txtday24 = e.Row.FindControl("txtday24") as Label;
                    Label txtday24ot = e.Row.FindControl("txtday24ot") as Label;
                    if (txtday24.Text == "P" || txtday24.Text == "W" || txtday24.Text == "p" || txtday24.Text == "w")
                    {
                        day24 = 1;
                    }
                    if (txtday24.Text == "H" || txtday24.Text == "h")
                    {
                        day24 = 0.5f;
                    }
                    if (txtday24.Text == "J" || txtday24.Text == "j")
                    {
                        day24 = 1.5f;
                    }

                    if (txtday24ot.Text == "P" || txtday24ot.Text == "W" || txtday24ot.Text == "p" || txtday24ot.Text == "w")
                    {
                        day24ot = 1;
                    }
                    if (txtday24ot.Text == "H" || txtday24ot.Text == "h")
                    {
                        day24ot = 0.5f;
                    }
                    if (txtday24ot.Text == "J" || txtday24ot.Text == "j")
                    {
                        day24ot = 1.5f;
                    }

                    Totalday24 += (day24 + day24ot);

                    #endregion

                    #region Day25

                    float day25 = 0;
                    float day25ot = 0;

                    Label txtday25 = e.Row.FindControl("txtday25") as Label;
                    Label txtday25ot = e.Row.FindControl("txtday25ot") as Label;
                    if (txtday25.Text == "P" || txtday25.Text == "W" || txtday25.Text == "p" || txtday25.Text == "w")
                    {
                        day25 = 1;
                    }
                    if (txtday25.Text == "H" || txtday25.Text == "h")
                    {
                        day25 = 0.5f;
                    }
                    if (txtday25.Text == "J" || txtday25.Text == "j")
                    {
                        day25 = 1.5f;
                    }

                    if (txtday25ot.Text == "P" || txtday25ot.Text == "W" || txtday25ot.Text == "p" || txtday25ot.Text == "w")
                    {
                        day25ot = 1;
                    }
                    if (txtday25ot.Text == "H" || txtday25ot.Text == "h")
                    {
                        day25ot = 0.5f;
                    }
                    if (txtday25ot.Text == "J" || txtday25ot.Text == "j")
                    {
                        day25ot = 1.5f;
                    }

                    Totalday25 += (day25 + day25ot);

                    #endregion

                    #region Day26

                    float day26 = 0;
                    float day26ot = 0;

                    Label txtday26 = e.Row.FindControl("txtday26") as Label;
                    Label txtday26ot = e.Row.FindControl("txtday26ot") as Label;
                    if (txtday26.Text == "P" || txtday26.Text == "W" || txtday26.Text == "p" || txtday26.Text == "w")
                    {
                        day26 = 1;
                    }
                    if (txtday26.Text == "H" || txtday26.Text == "h")
                    {
                        day26 = 0.5f;
                    }
                    if (txtday26.Text == "J" || txtday26.Text == "j")
                    {
                        day26 = 1.5f;
                    }

                    if (txtday26ot.Text == "P" || txtday26ot.Text == "W" || txtday26ot.Text == "p" || txtday26ot.Text == "w")
                    {
                        day26ot = 1;
                    }
                    if (txtday26ot.Text == "H" || txtday26ot.Text == "h")
                    {
                        day26ot = 0.5f;
                    }
                    if (txtday26ot.Text == "J" || txtday26ot.Text == "j")
                    {
                        day26ot = 1.5f;
                    }

                    Totalday26 += (day26 + day26ot);

                    #endregion

                    #region Day27

                    float day27 = 0;
                    float day27ot = 0;

                    Label txtday27 = e.Row.FindControl("txtday27") as Label;
                    Label txtday27ot = e.Row.FindControl("txtday27ot") as Label;
                    if (txtday27.Text == "P" || txtday27.Text == "W" || txtday27.Text == "p" || txtday27.Text == "w")
                    {
                        day27 = 1;
                    }
                    if (txtday27.Text == "H" || txtday27.Text == "h")
                    {
                        day27 = 0.5f;
                    }
                    if (txtday27.Text == "J" || txtday27.Text == "j")
                    {
                        day27 = 1.5f;
                    }

                    if (txtday27ot.Text == "P" || txtday27ot.Text == "W" || txtday27ot.Text == "p" || txtday27ot.Text == "w")
                    {
                        day27ot = 1;
                    }
                    if (txtday27ot.Text == "H" || txtday27ot.Text == "h")
                    {
                        day27ot = 0.5f;
                    }
                    if (txtday27ot.Text == "J" || txtday27ot.Text == "j")
                    {
                        day27ot = 1.5f;
                    }

                    Totalday27 += (day27 + day27ot);

                    #endregion

                    #region Day28

                    float day28 = 0;
                    float day28ot = 0;

                    Label txtday28 = e.Row.FindControl("txtday28") as Label;
                    Label txtday28ot = e.Row.FindControl("txtday28ot") as Label;
                    if (txtday28.Text == "P" || txtday28.Text == "W" || txtday28.Text == "p" || txtday28.Text == "w")
                    {
                        day28 = 1;
                    }
                    if (txtday28.Text == "H" || txtday28.Text == "h")
                    {
                        day28 = 0.5f;
                    }
                    if (txtday28.Text == "J" || txtday28.Text == "j")
                    {
                        day28 = 1.5f;
                    }

                    if (txtday28ot.Text == "P" || txtday28ot.Text == "W" || txtday28ot.Text == "p" || txtday28ot.Text == "w")
                    {
                        day28ot = 1;
                    }
                    if (txtday28ot.Text == "H" || txtday28ot.Text == "h")
                    {
                        day28ot = 0.5f;
                    }
                    if (txtday28ot.Text == "J" || txtday28ot.Text == "j")
                    {
                        day28ot = 1.5f;
                    }

                    Totalday28 += (day28 + day28ot);

                    #endregion

                    #region Day29

                    float day29 = 0;
                    float day29ot = 0;

                    Label txtday29 = e.Row.FindControl("txtday29") as Label;
                    Label txtday29ot = e.Row.FindControl("txtday29ot") as Label;
                    if (txtday29.Text == "P" || txtday29.Text == "W" || txtday29.Text == "p" || txtday29.Text == "w")
                    {
                        day29 = 1;
                    }
                    if (txtday29.Text == "H" || txtday29.Text == "h")
                    {
                        day29 = 0.5f;
                    }
                    if (txtday29.Text == "J" || txtday29.Text == "j")
                    {
                        day29 = 1.5f;
                    }

                    if (txtday29ot.Text == "P" || txtday29ot.Text == "W" || txtday29ot.Text == "p" || txtday29ot.Text == "w")
                    {
                        day29ot = 1;
                    }
                    if (txtday29ot.Text == "H" || txtday29ot.Text == "h")
                    {
                        day29ot = 0.5f;
                    }
                    if (txtday29ot.Text == "J" || txtday29ot.Text == "j")
                    {
                        day29ot = 1.5f;
                    }

                    Totalday29 += (day29 + day29ot);

                    #endregion

                    #region Day30

                    float day30 = 0;
                    float day30ot = 0;

                    Label txtday30 = e.Row.FindControl("txtday30") as Label;
                    Label txtday30ot = e.Row.FindControl("txtday30ot") as Label;
                    if (txtday30.Text == "P" || txtday30.Text == "W" || txtday30.Text == "p" || txtday30.Text == "w")
                    {
                        day30 = 1;
                    }
                    if (txtday30.Text == "H" || txtday30.Text == "h")
                    {
                        day30 = 0.5f;
                    }
                    if (txtday30.Text == "J" || txtday30.Text == "j")
                    {
                        day30 = 1.5f;
                    }

                    if (txtday30ot.Text == "P" || txtday30ot.Text == "W" || txtday30ot.Text == "p" || txtday30ot.Text == "w")
                    {
                        day30ot = 1;
                    }
                    if (txtday30ot.Text == "H" || txtday30ot.Text == "h")
                    {
                        day30ot = 0.5f;
                    }
                    if (txtday30ot.Text == "J" || txtday30ot.Text == "j")
                    {
                        day30ot = 1.5f;
                    }

                    Totalday30 += (day30 + day30ot);

                    #endregion

                    #region Day31

                    float day31 = 0;
                    float day31ot = 0;

                    Label txtday31 = e.Row.FindControl("txtday31") as Label;
                    Label txtday31ot = e.Row.FindControl("txtday31ot") as Label;
                    if (txtday31.Text == "P" || txtday31.Text == "W" || txtday31.Text == "p" || txtday31.Text == "w")
                    {
                        day31 = 1;
                    }
                    if (txtday31.Text == "H" || txtday31.Text == "h")
                    {
                        day31 = 0.5f;
                    }
                    if (txtday31.Text == "J" || txtday31.Text == "j")
                    {
                        day31 = 1.5f;
                    }

                    if (txtday31ot.Text == "P" || txtday31ot.Text == "W" || txtday31ot.Text == "p" || txtday31ot.Text == "w")
                    {
                        day31ot = 1;
                    }
                    if (txtday31ot.Text == "H" || txtday31ot.Text == "h")
                    {
                        day31ot = 0.5f;
                    }
                    if (txtday31ot.Text == "J" || txtday31ot.Text == "j")
                    {
                        day31ot = 1.5f;
                    }

                    Totalday31 += (day31 + day31ot);

                    #endregion

                    Label txtDuties = e.Row.FindControl("txtDuties") as Label;
                    TotalDuties += float.Parse(txtDuties.Text);

                    Label txtWos = e.Row.FindControl("txtWos") as Label;
                    TotalWos += float.Parse(txtWos.Text);

                    Label txtNHS = e.Row.FindControl("txtNHS") as Label;
                    TotalNHS += float.Parse(txtNHS.Text);

                    Label txtOTs = e.Row.FindControl("txtOTs") as Label;
                    TotalOts += float.Parse(txtOTs.Text);

                    Label txtCanAdv = e.Row.FindControl("txtCanAdv") as Label;
                    TotalCantAdv += float.Parse(txtCanAdv.Text);

                    Label txtPenalty = e.Row.FindControl("txtPenalty") as Label;
                    TotalPenalty += float.Parse(txtPenalty.Text);

                    Label txtIncentivs = e.Row.FindControl("txtIncentivs") as Label;
                    TotalIncentives += float.Parse(txtIncentivs.Text);

                    Label lblTotalDts = e.Row.FindControl("lblTotalDts") as Label;
                    Label lblTotalOts = e.Row.FindControl("lblTotalOts") as Label;
                    GrandTotal += (float.Parse(lblTotalDts.Text) + float.Parse(lblTotalOts.Text));

                    Label txtNa = e.Row.FindControl("txtNa") as Label;
                    TotalNa += float.Parse(txtNa.Text);

                    Label txtAb = e.Row.FindControl("txtAb") as Label;
                    TotalAb += float.Parse(txtAb.Text);


                    #endregion

                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (days == 30)
                    {
                        e.Row.Cells[34].Visible = false;
                    }
                    if (days == 28)
                    {
                        e.Row.Cells[34].Visible = false;
                        e.Row.Cells[33].Visible = false;
                        e.Row.Cells[32].Visible = false;
                    }

                    #region Assign Values to Footer Control

                    Label lblTotalday1 = e.Row.FindControl("lblTotalday1") as Label;
                    lblTotalday1.Text = Totalday1.ToString();

                    Label lblTotalday2 = e.Row.FindControl("lblTotalday2") as Label;
                    lblTotalday2.Text = Totalday2.ToString();

                    Label lblTotalday3 = e.Row.FindControl("lblTotalday3") as Label;
                    lblTotalday3.Text = Totalday3.ToString();

                    Label lblTotalday4 = e.Row.FindControl("lblTotalday4") as Label;
                    lblTotalday4.Text = Totalday4.ToString();

                    Label lblTotalday5 = e.Row.FindControl("lblTotalday5") as Label;
                    lblTotalday5.Text = Totalday5.ToString();

                    Label lblTotalday6 = e.Row.FindControl("lblTotalday6") as Label;
                    lblTotalday6.Text = Totalday6.ToString();

                    Label lblTotalday7 = e.Row.FindControl("lblTotalday7") as Label;
                    lblTotalday7.Text = Totalday7.ToString();

                    Label lblTotalday8 = e.Row.FindControl("lblTotalday8") as Label;
                    lblTotalday8.Text = Totalday8.ToString();

                    Label lblTotalday9 = e.Row.FindControl("lblTotalday9") as Label;
                    lblTotalday9.Text = Totalday9.ToString();

                    Label lblTotalday10 = e.Row.FindControl("lblTotalday10") as Label;
                    lblTotalday10.Text = Totalday10.ToString();

                    Label lblTotalday11 = e.Row.FindControl("lblTotalday11") as Label;
                    lblTotalday11.Text = Totalday11.ToString();

                    Label lblTotalday12 = e.Row.FindControl("lblTotalday12") as Label;
                    lblTotalday12.Text = Totalday12.ToString();

                    Label lblTotalday13 = e.Row.FindControl("lblTotalday13") as Label;
                    lblTotalday13.Text = Totalday13.ToString();

                    Label lblTotalday14 = e.Row.FindControl("lblTotalday14") as Label;
                    lblTotalday14.Text = Totalday14.ToString();

                    Label lblTotalday15 = e.Row.FindControl("lblTotalday15") as Label;
                    lblTotalday15.Text = Totalday15.ToString();

                    Label lblTotalday16 = e.Row.FindControl("lblTotalday16") as Label;
                    lblTotalday16.Text = Totalday16.ToString();

                    Label lblTotalday17 = e.Row.FindControl("lblTotalday17") as Label;
                    lblTotalday17.Text = Totalday17.ToString();

                    Label lblTotalday18 = e.Row.FindControl("lblTotalday18") as Label;
                    lblTotalday18.Text = Totalday18.ToString();

                    Label lblTotalday19 = e.Row.FindControl("lblTotalday19") as Label;
                    lblTotalday19.Text = Totalday19.ToString();

                    Label lblTotalday20 = e.Row.FindControl("lblTotalday20") as Label;
                    lblTotalday20.Text = Totalday20.ToString();

                    Label lblTotalday21 = e.Row.FindControl("lblTotalday21") as Label;
                    lblTotalday21.Text = Totalday21.ToString();

                    Label lblTotalday22 = e.Row.FindControl("lblTotalday22") as Label;
                    lblTotalday22.Text = Totalday22.ToString();

                    Label lblTotalday23 = e.Row.FindControl("lblTotalday23") as Label;
                    lblTotalday23.Text = Totalday23.ToString();

                    Label lblTotalday24 = e.Row.FindControl("lblTotalday24") as Label;
                    lblTotalday24.Text = Totalday24.ToString();

                    Label lblTotalday25 = e.Row.FindControl("lblTotalday25") as Label;
                    lblTotalday25.Text = Totalday25.ToString();

                    Label lblTotalday26 = e.Row.FindControl("lblTotalday26") as Label;
                    lblTotalday26.Text = Totalday26.ToString();

                    Label lblTotalday27 = e.Row.FindControl("lblTotalday27") as Label;
                    lblTotalday27.Text = Totalday27.ToString();

                    Label lblTotalday28 = e.Row.FindControl("lblTotalday28") as Label;
                    lblTotalday28.Text = Totalday28.ToString();

                    Label lblTotalday29 = e.Row.FindControl("lblTotalday29") as Label;
                    lblTotalday29.Text = Totalday29.ToString();

                    Label lblTotalday30 = e.Row.FindControl("lblTotalday30") as Label;
                    lblTotalday30.Text = Totalday30.ToString();

                    Label lblTotalday31 = e.Row.FindControl("lblTotalday31") as Label;
                    lblTotalday31.Text = Totalday31.ToString();


                    #endregion

                    Label lblTotalDuties = e.Row.FindControl("lblTotalDuties") as Label;
                    lblTotalDuties.Text = TotalDuties.ToString();

                    //Label lblTotalWOs = e.Row.FindControl("lblTotalWOs") as Label;
                    //lblTotalWOs.Text = TotalWos.ToString();

                    //Label lblTotalNHS = e.Row.FindControl("lblTotalNHS") as Label;
                    //lblTotalNHS.Text = TotalNHS.ToString();

                    Label lblTotalOTs = e.Row.FindControl("lblTotalOTs") as Label;
                    lblTotalOTs.Text = TotalOts.ToString();

                    Label lblGrandTotal = e.Row.FindControl("lblGrandTotal") as Label;
                    lblGrandTotal.Text = GrandTotal.ToString();

                    //Label lblTotalCanteenAdv = e.Row.FindControl("lblTotalCanteenAdv") as Label;
                    //lblTotalCanteenAdv.Text = TotalCantAdv.ToString();

                    //Label lblTotalPenalty = e.Row.FindControl("lblTotalPenalty") as Label;
                    //lblTotalPenalty.Text = TotalPenalty.ToString();

                    //Label lblTotalIncentives = e.Row.FindControl("lblTotalIncentives") as Label;
                    //lblTotalIncentives.Text = TotalIncentives.ToString();


                    //Label lblTotalNa = e.Row.FindControl("lblTotalNa") as Label;
                    //lblTotalNa.Text = TotalNa.ToString();

                    //Label lblTotalAb = e.Row.FindControl("lblTotalAb") as Label;
                    //lblTotalAb.Text = TotalAb.ToString();


                }
            }
            catch (Exception ex)
            {

            }
        }


        protected string GetEmpName(string empId)
        {
            string name = null;

            string sqlQry = "Select EmpFName,EmpMName from EmpDetails where EmpId='" + empId + "'";
            DataTable data = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;
            if (data.Rows.Count > 0)
            {
                name = data.Rows[0]["EmpFName"].ToString() + " " + data.Rows[0]["EmpMName"].ToString();
            }
            return name;
        }

        protected void DismatchDesignation()
        {


            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string Month = month + Year.Substring(2, 2);
            if (ddlClientID.SelectedIndex > 0)
            {
                string selqry = "select * from notinsertdata where  month='" + Month + "'";
                DataTable dtselect = config.ExecuteAdaptorAsyncWithQueryParams(selqry).Result;
                if (dtselect.Rows.Count > 0)
                {
                    GridView2.DataSource = dtselect;
                    GridView2.DataBind();
                    btnExport.Visible = true;
                }
                else
                {
                    GridView2.DataSource = null;
                    GridView2.DataBind();
                    btnExport.Visible = false;
                }
            }
            else
            {

            }
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

            return ExcelSheetname;
        }

        protected void btnGetdata_Click(object sender, EventArgs e)
        {
            getemployeeattendance();
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            // int month = 0;
            int days = 0;

            btnExport.Visible = false;
            GridView2.DataSource = null;
            GridView2.DataBind();



            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string Month = month + Year.Substring(2, 2);



            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please Select Month');", true);
                return;
            }

            try
            {
                #region Begin Getmax Id from DB
                int ExcelNo = 0;
                string selectquery = "select max(cast(ExcelNumber as int )) as Id from empattendance";
                DataTable dtExcelID = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

                if (dtExcelID.Rows.Count > 0)
                {
                    if (String.IsNullOrEmpty(dtExcelID.Rows[0]["Id"].ToString()) == false)
                    {
                        ExcelNo = Convert.ToInt32(dtExcelID.Rows[0]["Id"].ToString()) + 1;
                    }
                    else
                    {
                        ExcelNo = int.Parse("1");
                    }
                }
                #endregion End Getmax Id from DB

                #region Begin Code for when select Full Attendance as on 31/07/2014 by Venkat
                //
                if (ddlAttendanceMode.SelectedIndex == 0)
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


                    string qry = "select [Employee Name],[Client Id],[Emp Id],[Designation],[Duties],[OTs],[WOs],[NHS],[Leaves],[Canteen Advance],[Incentives],[Rent Ded] " +
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
                    string clientid = string.Empty;
                    string design = string.Empty;
                    int empstatus = 0;
                    string ContractID = "";

                    //foreach (DataRow dr in ds.Tables[0].Rows)
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string Remark = string.Empty;




                        #region Variables for Excel Values


                        //string Month = string.Empty;

                        float penalty = 0;
                        float incentives = 0;
                        float canteenadvance = 0;
                        float Leaves = 0;
                        float Wos = 0;
                        float NHS = 0;
                        float Npots = 0;
                        float Na = 0;
                        float Ab = 0;
                        float duties = 0;
                        float ots = 0;
                        float FoodDed = 0;
                        float RentDed = 0;
                        float SalAdv = 0;
                        float UniformDed = 0;
                        float OtherDed = 0;
                        float AdminCharges = 0;

                        float dayduties = 0;
                        float dayots = 0;
                        float daywos = 0;

                        #endregion

                        #region Variables for Posting order Table data and EmpAttendance(Default Values)

                        int orderid = 0;

                        string PrevUnitid = string.Empty;
                        string Dutyhrs = string.Empty;
                        DateTime Orderdate = DateTime.Now;
                        DateTime Joiningdate = DateTime.Now;
                        DateTime Releivingdate = DateTime.Now;
                        string IssuedAuthority = string.Empty;
                        string Remarks = string.Empty;
                        int TransferType = 1;

                        string AttString = string.Empty;
                        string HrsString = string.Empty;


                        #endregion

                        clientid = ds.Tables[0].Rows[i]["Client Id"].ToString();



                        DateTime DtLastDay = DateTime.Now;
                        DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));

                        Hashtable HtGetContractID = new Hashtable();
                        var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                        HtGetContractID.Add("@clientid", clientid);
                        HtGetContractID.Add("@LastDay", DtLastDay);
                        DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                        if (DTContractID.Rows.Count > 0)
                        {
                            ContractID = DTContractID.Rows[0]["contractid"].ToString();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not Avaialable For This Client.');", true);
                            return;
                        }

                        int SNo = 0;
                        selectquery = "select max(cast(sno as int )) as sno from empattendance where clientid='" + clientid + "' and month='" + Month + "' ";
                        DataTable dtSnoID = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

                        if (dtSnoID.Rows.Count > 0)
                        {
                            if (String.IsNullOrEmpty(dtSnoID.Rows[0]["sno"].ToString()) == false)
                            {
                                SNo = Convert.ToInt32(dtSnoID.Rows[0]["sno"].ToString()) + 1;
                            }
                            else
                            {
                                SNo = int.Parse("1");
                            }
                        }


                        // if (clientid == ddlClientID.SelectedValue)
                        {
                            empstatus = 0;

                            empid = ds.Tables[0].Rows[i]["Emp Id"].ToString();

                            string sqlchkempid = "select empid from empdetails where empid='" + empid + "' and empstatus=1";
                            DataTable dtchkempid = config.ExecuteAdaptorAsyncWithQueryParams(sqlchkempid).Result;

                            if (dtchkempid.Rows.Count > 0)
                            {
                                empstatus = 1;
                            }
                            else
                            {
                                empstatus = 0;
                            }



                            //Create Procedure


                            if (empid.Length > 0)
                            {

                                design = ds.Tables[0].Rows[i]["Designation"].ToString();

                                #region Begin New code on 28/04/2014 by venkat for Duties,ots,wos,penalty,Incentives,NHS,Ab and Na

                                duties = 0;
                                ots = 0;

                                if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Duties"].ToString()) == false)
                                {
                                    duties = float.Parse(ds.Tables[0].Rows[i]["Duties"].ToString());
                                }

                                if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Wos"].ToString()) == false)
                                {
                                    Wos = float.Parse(ds.Tables[0].Rows[i]["WOs"].ToString());
                                }

                                if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["NHS"].ToString()) == false)
                                {
                                    NHS = float.Parse(ds.Tables[0].Rows[i]["NHS"].ToString());
                                }


                                if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["OTs"].ToString()) == false)
                                {
                                    ots = float.Parse(ds.Tables[0].Rows[i]["OTs"].ToString());
                                }

                                if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Leaves"].ToString()) == false)
                                {
                                    Leaves = float.Parse(ds.Tables[0].Rows[i]["Leaves"].ToString());
                                }

                                if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Canteen Advance"].ToString()) == false)
                                {
                                    canteenadvance = float.Parse(ds.Tables[0].Rows[i]["Canteen Advance"].ToString());
                                }

                                if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Incentives"].ToString()) == false)
                                {
                                    incentives = float.Parse(ds.Tables[0].Rows[i]["Incentives"].ToString());
                                }

                                if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Rent Ded"].ToString()) == false)
                                {
                                    RentDed = float.Parse(ds.Tables[0].Rows[i]["Rent Ded"].ToString());
                                }


                                #endregion


                                #region Begin New code for Stored Procedure as on 29/04/2014 by venkat


                                #region Begin code for passing values to the Stored Procedure as 29/04/2014 by Venkat


                                Hashtable Httable = new Hashtable();

                                Httable.Add("@empidstatus", empstatus);
                                Httable.Add("@Clientid", clientid);
                                Httable.Add("@Month", Month);
                                Httable.Add("@Empid", empid);
                                Httable.Add("@ContractId", ContractID);
                                Httable.Add("@Design", design);
                                Httable.Add("@Duties", duties);
                                Httable.Add("@Ots", ots);
                                Httable.Add("@WOs", Wos);
                                Httable.Add("@CanteenAdv", canteenadvance);
                                Httable.Add("@Leaves", Leaves);
                                Httable.Add("@Incentivs", incentives);
                                Httable.Add("@RentDed", RentDed);
                                Httable.Add("@NHS", NHS);
                                Httable.Add("@Excel_Number", ExcelNo);
                                Httable.Add("@sno", SNo);

                                #endregion

                                string SPName = "ImportFullAttendanceFromExcel";
                                DataTable dtstatus = config.ExecuteAdaptorAsyncWithParams(SPName, Httable).Result;


                                #endregion



                            }

                            //End Create Procedure

                        }
                        // else
                        //{
                        //    ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please enter Clientid');", true);
                        //}
                    }
                    // getemployeeattendance();
                }


                #endregion

                #region Begin Code when select Individual attendance as on 31/07/2014 by Venkat
                //
                if (ddlAttendanceMode.SelectedIndex == 1)
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

                    //string qry = "select [Client Id],[Emp Id],[Designation],[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18]," +
                    //" [19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31],[Duties],[OTs],[WOs],[NHS],[Canteen Advance],[Penalty]," +
                    //" [Incentives],[NHS],[PF],[ESI],[PT],[NA],[AB] from  [" + GetExcelSheetNames() + "]" + "";


                    string qry = "select [Client Id],[Emp Id],[Designation],[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18]," +
                   " [19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31],[OTs],[Leaves],[Canteen Advance],[Incentives],[Rent Ded]" +
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


                    int k = 0;

                    string empid = string.Empty;
                    string clientid = string.Empty;
                    string design = string.Empty;
                    int empstatus = 0;

                    float duties = 0;
                    float ots = 0;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string Remark = string.Empty;


                        #region Variables for Excel Values


                        // string Month = string.Empty;

                        float penalty = 0;
                        float incentives = 0;
                        float canteenadvance = 0;
                        float Leaves = 0;
                        float Wos = 0;
                        float NHS = 0;
                        float Npots = 0;
                        float Na = 0;
                        float Ab = 0;
                        float FoodDed = 0;
                        float RentDed = 0;

                        float SalAdv = 0;
                        float UniformDed = 0;
                        float OtherDed = 0;
                        float AdminCharges = 0;


                        string day1 = string.Empty;
                        string day2 = string.Empty;
                        string day3 = string.Empty;
                        string day4 = string.Empty;
                        string day5 = string.Empty;
                        string day6 = string.Empty;
                        string day7 = string.Empty;
                        string day8 = string.Empty;
                        string day9 = string.Empty;
                        string day10 = string.Empty;
                        string day11 = string.Empty;
                        string day12 = string.Empty;
                        string day13 = string.Empty;
                        string day14 = string.Empty;
                        string day15 = string.Empty;
                        string day16 = string.Empty;
                        string day17 = string.Empty;
                        string day18 = string.Empty;
                        string day19 = string.Empty;
                        string day20 = string.Empty;
                        string day21 = string.Empty;
                        string day22 = string.Empty;
                        string day23 = string.Empty;
                        string day24 = string.Empty;
                        string day25 = string.Empty;
                        string day26 = string.Empty;
                        string day27 = string.Empty;
                        string day28 = string.Empty;
                        string day29 = string.Empty;
                        string day30 = string.Empty;
                        string day31 = string.Empty;

                        string day1ot = string.Empty;
                        string day2ot = string.Empty;
                        string day3ot = string.Empty;
                        string day4ot = string.Empty;
                        string day5ot = string.Empty;
                        string day6ot = string.Empty;
                        string day7ot = string.Empty;
                        string day8ot = string.Empty;
                        string day9ot = string.Empty;
                        string day10ot = string.Empty;
                        string day11ot = string.Empty;
                        string day12ot = string.Empty;
                        string day13ot = string.Empty;
                        string day14ot = string.Empty;
                        string day15ot = string.Empty;
                        string day16ot = string.Empty;
                        string day17ot = string.Empty;
                        string day18ot = string.Empty;
                        string day19ot = string.Empty;
                        string day20ot = string.Empty;
                        string day21ot = string.Empty;
                        string day22ot = string.Empty;
                        string day23ot = string.Empty;
                        string day24ot = string.Empty;
                        string day25ot = string.Empty;
                        string day26ot = string.Empty;
                        string day27ot = string.Empty;
                        string day28ot = string.Empty;
                        string day29ot = string.Empty;
                        string day30ot = string.Empty;
                        string day31ot = string.Empty;

                        float dayduties = 0;
                        float dayots = 0;
                        float daywos = 0;
                        float dayNHS = 0;

                        #endregion

                        #region Variables for Posting order Table data and EmpAttendance(Default Values)

                        int orderid = 0;

                        string PrevUnitid = string.Empty;
                        string Dutyhrs = string.Empty;
                        DateTime Orderdate = DateTime.Now;
                        DateTime Joiningdate = DateTime.Now;
                        DateTime Releivingdate = DateTime.Now;
                        string IssuedAuthority = string.Empty;
                        string Remarks = string.Empty;
                        int TransferType = 1;

                        string AttString = string.Empty;
                        string HrsString = string.Empty;
                        float TotalHours = 0;
                        float OTHours = 0;
                        float NHDays = 0;
                        float CL = 0;
                        float PL = 0;
                        float UL = 0;

                        int pf = 0;
                        int esi = 0;
                        int pt = 0;

                        string Contractid = string.Empty;

                        #endregion



                        if ((k % 2) == 0 || k == 0 || (k % 2) == 1)
                        {

                            clientid = ds.Tables[0].Rows[k]["Client Id"].ToString();
                            ViewState["clientid"] = clientid;
                            clientid = ViewState["clientid"].ToString();
                        }


                        int Sno = 0;
                        selectquery = "select max(cast(Sno as int )) as Sno from empattendance where clientid='" + clientid + "' and month='" + Month + "' ";
                        DataTable dtSno = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

                        if (dtSno.Rows.Count > 0)
                        {
                            if (String.IsNullOrEmpty(dtSno.Rows[0]["Sno"].ToString()) == false)
                            {
                                Sno = Convert.ToInt32(dtSno.Rows[0]["Sno"].ToString()) + 1;
                            }
                            else
                            {
                                Sno = int.Parse("1");
                            }
                        }

                        DateTime DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));

                        #region  Begin Get Contract Id Based on The Last Day


                        Hashtable HtGetContractID = new Hashtable();
                        var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                        HtGetContractID.Add("@clientid", clientid);
                        HtGetContractID.Add("@LastDay", DtLastDay);
                        DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                        if (DTContractID.Rows.Count > 0)
                        {
                            Contractid = DTContractID.Rows[0]["contractid"].ToString();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not  Avaialable For This Client.');", true);
                            return;
                        }

                        #endregion  End Get Contract Id Based on The Last Day

                        //if (clientid == ddlClientID.SelectedValue)
                        {
                            if ((k % 2) == 0 || k == 0 || (k % 2) == 1)
                            {
                                empstatus = 0;

                                empid = ds.Tables[0].Rows[k]["Emp Id"].ToString();
                                ViewState["empid"] = empid;
                                empid = ViewState["empid"].ToString();

                                string sqlchkempid = "select empid from empdetails where empid='" + empid + "' and empstatus=1";
                                DataTable dtchkempid = config.ExecuteAdaptorAsyncWithQueryParams(sqlchkempid).Result;

                                for (int i = 0; i < dtchkempid.Rows.Count; i++)
                                {
                                    string empid2 = "";
                                    empid2 = dtchkempid.Rows[i]["empid"].ToString();
                                    if (empid == empid2)
                                    {
                                        empstatus = 1;
                                    }
                                }
                            }


                            //Create Procedure


                            if (empid.Length > 0)
                            {

                                if ((k % 2) == 0 || k == 0 || (k % 2) == 1)
                                {
                                    design = ds.Tables[0].Rows[k]["Designation"].ToString();
                                    ViewState["design"] = design;
                                    design = ViewState["design"].ToString();

                                }

                                if ((k % 2) == 0 || k == 0 || (k % 2) == 1)
                                {
                                    float dt1, dt2, dt3, dt4, dt5, dt6, dt7, dt8, dt9, dt10, dt11, dt12, dt13, dt14, dt15, dt16, dt17, dt18, dt19, dt20, dt21,
                                        dt22, dt23, dt24, dt25, dt26, dt27, dt28, dt29, dt30, dt31;

                                    float Ots1, Ots2, Ots3, Ots4, Ots5, Ots6, Ots7, Ots8, Ots9, Ots10, Ots11, Ots12, Ots13, Ots14, Ots15, Ots16, Ots17, Ots18, Ots19, Ots20, Ots21,
                                       Ots22, Ots23, Ots24, Ots25, Ots26, Ots27, Ots28, Ots29, Ots30, Ots31;

                                    float wo1, wo2, wo3, wo4, wo5, wo6, wo7, wo8, wo9, wo10, wo11, wo12, wo13, wo14, wo15, wo16, wo17, wo18, wo19, wo20, wo21,
                                      wo22, wo23, wo24, wo25, wo26, wo27, wo28, wo29, wo30, wo31;


                                    float nhs1, nhs2, nhs3, nhs4, nhs5, nhs6, nhs7, nhs8, nhs9, nhs10, nhs11, nhs12, nhs13, nhs14, nhs15, nhs16, nhs17, nhs18, nhs19, nhs20, nhs21,
                                      nhs22, nhs23, nhs24, nhs25, nhs26, nhs27, nhs28, nhs29, nhs30, nhs31;

                                    dt1 = dt2 = dt3 = dt4 = dt5 = dt6 = dt7 = dt8 = dt9 = dt10 = dt11 = dt12 = dt13 = dt14 = dt15 = dt16 = dt17 = dt18 = dt19 = dt20 = dt21 =
                                        dt22 = dt23 = dt24 = dt25 = dt26 = dt27 = dt28 = dt29 = dt30 = dt31 = 0;

                                    Ots1 = Ots2 = Ots3 = Ots4 = Ots5 = Ots6 = Ots7 = Ots8 = Ots9 = Ots10 = Ots11 = Ots12 = Ots13 = Ots14 = Ots15 = Ots16 = Ots17 = Ots18 = Ots19 = Ots20 = Ots21 =
                                       Ots22 = Ots23 = Ots24 = Ots25 = Ots26 = Ots27 = Ots28 = Ots29 = Ots30 = Ots31 = 0;

                                    wo1 = wo2 = wo3 = wo4 = wo5 = wo6 = wo7 = wo8 = wo9 = wo10 = wo11 = wo12 = wo13 = wo14 = wo15 = wo16 = wo17 = wo18 = wo19 = wo20 = wo21 =
                                      wo22 = wo23 = wo24 = wo25 = wo26 = wo27 = wo28 = wo29 = wo30 = wo31 = 0;


                                    nhs1 = nhs2 = nhs3 = nhs4 = nhs5 = nhs6 = nhs7 = nhs8 = nhs9 = nhs10 = nhs11 = nhs12 = nhs13 = nhs14 = nhs15 = nhs16 = nhs17 = nhs18 = nhs19 = nhs20 = nhs21 =
                                      nhs22 = nhs23 = nhs24 = nhs25 = nhs26 = nhs27 = nhs28 = nhs29 = nhs30 = nhs31 = 0;

                                    #region Begin New code on 28/04/2014 by venkat for Duties,ots,wos,penalty,Incentives,NHS,Ab and Na

                                    duties = 0;
                                    ots = 0;

                                    //if (String.IsNullOrEmpty(dr["Duties"].ToString()) == false)
                                    //{
                                    //    duties = float.Parse(dr["Duties"].ToString());
                                    //}

                                    if (String.IsNullOrEmpty(dr["OTs"].ToString()) == false)
                                    {
                                        ots = float.Parse(dr["OTs"].ToString());
                                    }

                                    if (String.IsNullOrEmpty(dr["Canteen Advance"].ToString()) == false)
                                    {
                                        canteenadvance = float.Parse(dr["Canteen Advance"].ToString());
                                    }
                                    if (String.IsNullOrEmpty(dr["Leaves"].ToString()) == false)
                                    {
                                        Leaves = float.Parse(dr["Leaves"].ToString());
                                    }
                                    if (String.IsNullOrEmpty(dr["Incentives"].ToString()) == false)
                                    {
                                        incentives = float.Parse(dr["Incentives"].ToString());
                                    }


                                    if (String.IsNullOrEmpty(dr["Rent Ded"].ToString()) == false)
                                    {
                                        RentDed = float.Parse(dr["Rent Ded"].ToString());
                                    }

                                    #endregion

                                    #region Day wise Even data insert



                                    day1 = dr["1"].ToString();
                                    if (day1.Trim().Length == 0 || day1 == "0")
                                    { day1 = "A"; }

                                    day2 = dr["2"].ToString();
                                    if (day2.Length == 0 || day2 == "0")
                                    { day2 = "A"; }

                                    day3 = dr["3"].ToString();
                                    if (day3.Length == 0 || day3 == "0")
                                    { day3 = "A"; }

                                    day4 = dr["4"].ToString();
                                    if (day4.Length == 0 || day4 == "0")
                                    {
                                        day4 = "A";
                                    }

                                    day5 = dr["5"].ToString();
                                    if (day5.Length == 0 || day5 == "0")
                                    {
                                        day5 = "A";
                                    }

                                    day6 = dr["6"].ToString();
                                    if (day6.Length == 0 || day6 == "0")
                                    {
                                        day6 = "A";
                                    }

                                    day7 = dr["7"].ToString();
                                    if (day7.Length == 0 || day7 == "0")
                                    {
                                        day7 = "A";
                                    }

                                    day8 = dr["8"].ToString();
                                    if (day8.Length == 0 || day8 == "0")
                                    {
                                        day8 = "A";
                                    }

                                    day9 = dr["9"].ToString();
                                    if (day9.Length == 0 || day9 == "0")
                                    {
                                        day9 = "A";
                                    }

                                    day10 = dr["10"].ToString();
                                    if (day10.Length == 0 || day10 == "0")
                                    {
                                        day10 = "A";
                                    }

                                    day11 = dr["11"].ToString();
                                    if (day11.Length == 0 || day11 == "0")
                                    {
                                        day11 = "A";
                                    }

                                    day12 = dr["12"].ToString();
                                    if (day12.Length == 0 || day12 == "0")
                                    {
                                        day12 = "A";
                                    }

                                    day13 = dr["13"].ToString();
                                    if (day13.Length == 0 || day13 == "0")
                                    {
                                        day13 = "A";
                                    }

                                    day14 = dr["14"].ToString();
                                    if (day14.Length == 0 || day14 == "0")
                                    {
                                        day14 = "A";
                                    }

                                    day15 = dr["15"].ToString();
                                    if (day15.Length == 0 || day15 == "0")
                                    {
                                        day15 = "A";
                                    }

                                    day16 = dr["16"].ToString();
                                    if (day16.Length == 0 || day16 == "0")
                                    {
                                        day16 = "A";
                                    }

                                    day17 = dr["17"].ToString();
                                    if (day17.Length == 0 || day17 == "0")
                                    {
                                        day17 = "A";
                                    }

                                    day18 = dr["18"].ToString();
                                    if (day18.Length == 0 || day18 == "0")
                                    {
                                        day18 = "A";
                                    }

                                    day19 = dr["19"].ToString();
                                    if (day19.Length == 0 || day19 == "0")
                                    {
                                        day19 = "A";
                                    }

                                    day20 = dr["20"].ToString();
                                    if (day20.Length == 0 || day20 == "0")
                                    {
                                        day20 = "A";
                                    }

                                    day21 = dr["21"].ToString();
                                    if (day21.Length == 0 || day21 == "0")
                                    {
                                        day21 = "A";
                                    }

                                    day22 = dr["22"].ToString();
                                    if (day22.Length == 0 || day22 == "0")
                                    {
                                        day22 = "A";
                                    }

                                    day23 = dr["23"].ToString();
                                    if (day23.Length == 0 || day23 == "0")
                                    {
                                        day23 = "A";
                                    }

                                    day24 = dr["24"].ToString();
                                    if (day24.Length == 0 || day24 == "0")
                                    {
                                        day24 = "A";
                                    }

                                    day25 = dr["25"].ToString();
                                    if (day25.Length == 0 || day25 == "0")
                                    {
                                        day25 = "A";
                                    }

                                    day26 = dr["26"].ToString();
                                    if (day26.Length == 0 || day26 == "0")
                                    {
                                        day26 = "A";
                                    }

                                    day27 = dr["27"].ToString();
                                    if (day27.Length == 0 || day27 == "0")
                                    {
                                        day27 = "A";
                                    }

                                    day28 = dr["28"].ToString();
                                    if (day28.Length == 0 || day28 == "0")
                                    {
                                        day28 = "A";
                                    }

                                    day29 = dr["29"].ToString();
                                    if (day29.Length == 0 || day29 == "0")
                                    {
                                        day29 = "A";
                                    }

                                    day30 = dr["30"].ToString();
                                    if (day30.Length == 0 || day30 == "0")
                                    {
                                        day30 = "A";
                                    }

                                    day31 = dr["31"].ToString();
                                    if (day31.Length == 0 || day31 == "0")
                                    {
                                        day31 = "A";
                                    }

                                    #endregion


                                    #region Values for Duties

                                    //1

                                    if (day1.Trim() == "P" || day1.Trim() == "p")
                                    {
                                        dt1 = 1;
                                    }
                                    if (day1.Trim() == "P/P" || day1.Trim() == "p/p")
                                    {
                                        dt1 = 1;
                                        Ots1 = 1;
                                    }
                                    if (day1.Trim() == "J" || day1.Trim() == "j")
                                    {
                                        dt1 = 1.5f;
                                    }

                                    if (day1.Trim() == "W/O" || day1.Trim() == "w/o")
                                    {
                                        wo1 = 1;
                                    }

                                    if (day1.Trim() == "A" || day1.Trim() == "a")
                                    {
                                        dt1 = 0;
                                    }
                                    if (day1.Trim() == "N" || day1.Trim() == "n")
                                    {
                                        nhs1 = 1;
                                    }

                                    //2
                                    if (day2.Trim() == "P" || day2.Trim() == "p")
                                    {
                                        dt2 = 1;
                                    }
                                    if (day2.Trim() == "P/P" || day2.Trim() == "P/P")
                                    {
                                        dt2 = 1;
                                        Ots2 = 1;
                                    }
                                    if (day2.Trim() == "J" || day2.Trim() == "j")
                                    {
                                        dt2 = 1.5f;
                                    }
                                    if (day2.Trim() == "W/O" || day2.Trim() == "w/o")
                                    {
                                        wo2 = 1;
                                    }
                                    if (day2.Trim() == "A" || day2.Trim() == "a")
                                    {
                                        dt2 = 0;
                                    }
                                    if (day2.Trim() == "N" || day2.Trim() == "n")
                                    {
                                        nhs2 = 1;
                                    }



                                    //3
                                    if (day3.Trim() == "P" || day3.Trim() == "p")
                                    {
                                        dt3 = 1;
                                    }
                                    if (day3.Trim() == "P/P" || day3.Trim() == "p/p")
                                    {
                                        dt3 = 1; Ots3 = 1;
                                    }
                                    if (day3.Trim() == "J" || day3.Trim() == "j")
                                    {
                                        dt3 = 1.5f;
                                    }

                                    if (day3.Trim() == "W/O" || day3.Trim() == "w/o")
                                    {
                                        wo3 = 1;
                                    }
                                    if (day3.Trim() == "A" || day3.Trim() == "a")
                                    {
                                        dt3 = 0;
                                    }
                                    if (day3.Trim() == "N" || day3.Trim() == "n")
                                    {
                                        nhs3 = 1;
                                    }



                                    //4
                                    if (day4.Trim() == "P" || day4.Trim() == "p")
                                    {
                                        dt4 = 1;
                                    }
                                    if (day4.Trim() == "P/P" || day4.Trim() == "p/p")
                                    {
                                        dt4 = 1; Ots4 = 1;
                                    }
                                    if (day4.Trim() == "J" || day4.Trim() == "j")
                                    {
                                        dt4 = 1.5f;
                                    }

                                    if (day4.Trim() == "W/O" || day4.Trim() == "w/o")
                                    {
                                        wo4 = 1;
                                    }
                                    if (day4.Trim() == "A" || day4.Trim() == "a")
                                    {
                                        dt4 = 0;
                                    }
                                    if (day4.Trim() == "N" || day4.Trim() == "n")
                                    {
                                        nhs4 = 1;
                                    }

                                    //5
                                    if (day5.Trim() == "P" || day5.Trim() == "p")
                                    {
                                        dt5 = 1;
                                    }
                                    if (day5.Trim() == "P/P" || day5.Trim() == "p/p")
                                    {
                                        dt5 = 1; Ots5 = 1;
                                    }
                                    if (day5.Trim() == "J" || day5.Trim() == "j")
                                    {
                                        dt5 = 1.5f;
                                    }
                                    if (day5.Trim() == "W/O" || day5 == "w/o")
                                    {
                                        wo5 = 1;
                                    }
                                    if (day5.Trim() == "A" || day5.Trim() == "a")
                                    {
                                        dt5 = 0;
                                    }
                                    if (day5.Trim() == "N" || day5.Trim() == "n")
                                    {
                                        nhs5 = 1;
                                    }

                                    //6
                                    if (day6.Trim() == "P" || day6.Trim() == "p")
                                    {
                                        dt6 = 1;
                                    }
                                    if (day6.Trim() == "P/P" || day6.Trim() == "p/p")
                                    {
                                        dt6 = 1; Ots6 = 1;
                                    }
                                    if (day6.Trim() == "J" || day6.Trim() == "j")
                                    {
                                        dt6 = 1.5f;
                                    }

                                    if (day6.Trim() == "W/O" || day6.Trim() == "w/o")
                                    {
                                        wo6 = 1;
                                    }
                                    if (day6.Trim() == "A" || day6.Trim() == "a")
                                    {
                                        dt6 = 0;
                                    }
                                    if (day6.Trim() == "N" || day6.Trim() == "n")
                                    {
                                        nhs6 = 1;
                                    }

                                    //7
                                    if (day7.Trim() == "P" || day7.Trim() == "p")
                                    {
                                        dt7 = 1;
                                    }
                                    if (day7.Trim() == "P/P" || day7.Trim() == "p/p")
                                    {
                                        dt7 = 1; Ots7 = 1;
                                    }
                                    if (day7.Trim() == "J" || day7.Trim() == "j")
                                    {
                                        dt7 = 1.5f;
                                    }
                                    if (day7.Trim() == "W/O" || day7.Trim() == "w/o")
                                    {
                                        wo7 = 1;
                                    }
                                    if (day7.Trim() == "A" || day7.Trim() == "a")
                                    {
                                        dt7 = 0;
                                    }
                                    if (day7.Trim() == "N" || day7.Trim() == "n")
                                    {
                                        nhs7 = 1;
                                    }
                                    //8
                                    if (day8.Trim() == "P" || day8.Trim() == "p")
                                    {
                                        dt8 = 1;
                                    }
                                    if (day8.Trim() == "P/P" || day8.Trim() == "p/p")
                                    {
                                        dt8 = 1; Ots8 = 1;
                                    }
                                    if (day8.Trim() == "J" || day8.Trim() == "j")
                                    {
                                        dt8 = 1.5f;
                                    }
                                    if (day8.Trim() == "W/O" || day8.Trim() == "w/o")
                                    {
                                        wo8 = 1;
                                    }
                                    if (day8.Trim() == "A" || day8.Trim() == "a")
                                    {
                                        dt8 = 0;
                                    }
                                    if (day8.Trim() == "N" || day8.Trim() == "n")
                                    {
                                        nhs8 = 1;
                                    }

                                    //9
                                    if (day9.Trim() == "P" || day9.Trim() == "p")
                                    {
                                        dt9 = 1;
                                    }
                                    if (day9.Trim() == "P/P" || day9.Trim() == "p/p")
                                    {
                                        dt9 = 1; Ots9 = 1;
                                    }
                                    if (day9.Trim() == "J" || day9.Trim() == "j")
                                    {
                                        dt9 = 1.5f;
                                    }

                                    if (day9.Trim() == "W/O" || day9.Trim() == "w/o")
                                    {
                                        wo9 = 1;
                                    }
                                    if (day9.Trim() == "A" || day9.Trim() == "a")
                                    {
                                        dt9 = 0;
                                    }
                                    if (day9.Trim() == "N" || day9.Trim() == "n")
                                    {
                                        nhs9 = 1;
                                    }

                                    //10
                                    if (day10.Trim() == "P" || day10.Trim() == "p")
                                    {
                                        dt10 = 1;
                                    }
                                    if (day10.Trim() == "P/P" || day10.Trim() == "p/p")
                                    {
                                        dt10 = 1; Ots10 = 1;
                                    }
                                    if (day10.Trim() == "J" || day10.Trim() == "j")
                                    {
                                        dt10 = 1.5f;
                                    }
                                    if (day10.Trim() == "W/O" || day10.Trim() == "w/o")
                                    {
                                        wo10 = 1;
                                    }
                                    if (day10.Trim() == "A" || day10.Trim() == "a")
                                    {
                                        dt10 = 0;
                                    }
                                    if (day10.Trim() == "N" || day10.Trim() == "n")
                                    {
                                        nhs10 = 1;
                                    }
                                    //11

                                    if (day11.Trim() == "P" || day11.Trim() == "p")
                                    {
                                        dt11 = 1;
                                    }
                                    if (day11.Trim() == "P/P" || day11.Trim() == "p/p")
                                    {
                                        dt11 = 1; Ots11 = 1;
                                    }
                                    if (day11.Trim() == "J" || day11.Trim() == "j")
                                    {
                                        dt11 = 1.5f;
                                    }
                                    if (day11.Trim() == "W/O" || day11.Trim() == "w/o")
                                    {
                                        wo11 = 1;
                                    }
                                    if (day11.Trim() == "A" || day11.Trim() == "a")
                                    {
                                        dt11 = 0;
                                    }
                                    if (day11.Trim() == "N" || day11.Trim() == "n")
                                    {
                                        nhs11 = 1;
                                    }
                                    //12
                                    if (day12.Trim() == "P" || day12.Trim() == "p")
                                    {
                                        dt12 = 1;
                                    }
                                    if (day12.Trim() == "P/P" || day12.Trim() == "p/p")
                                    {
                                        dt12 = 1; Ots12 = 1;
                                    }
                                    if (day12.Trim() == "J" || day12.Trim() == "j")
                                    {
                                        dt12 = 1.5f;
                                    }
                                    if (day12.Trim() == "W/O" || day12.Trim() == "w/o")
                                    {
                                        wo12 = 1;
                                    }
                                    if (day12.Trim() == "A" || day12.Trim() == "a")
                                    {
                                        dt12 = 0;
                                    }
                                    if (day12.Trim() == "N" || day12.Trim() == "n")
                                    {
                                        nhs12 = 1;
                                    }
                                    //13
                                    if (day13.Trim() == "P" || day13.Trim() == "p")
                                    {
                                        dt13 = 1;
                                    }
                                    if (day13.Trim() == "P/P" || day13.Trim() == "p/p")
                                    {
                                        dt13 = 1; Ots13 = 1;
                                    }
                                    if (day13.Trim() == "J" || day13.Trim() == "j")
                                    {
                                        dt13 = 1.5f;
                                    }
                                    if (day13.Trim() == "W/O" || day13.Trim() == "w/o")
                                    {
                                        wo13 = 1;
                                    }
                                    if (day13.Trim() == "A" || day13.Trim() == "a")
                                    {
                                        dt13 = 0;
                                    }
                                    if (day13 == "N" || day13 == "n")
                                    {
                                        nhs13 = 1;
                                    }
                                    //14
                                    if (day14.Trim() == "P" || day14.Trim() == "p")
                                    {
                                        dt14 = 1;
                                    }
                                    if (day14.Trim() == "P/P" || day14.Trim() == "p/p")
                                    {
                                        dt14 = 1; Ots14 = 1;
                                    }
                                    if (day14.Trim() == "J" || day14.Trim() == "j")
                                    {
                                        dt14 = 1.5f;
                                    }
                                    if (day14.Trim() == "W/O" || day14.Trim() == "w/o")
                                    {
                                        wo14 = 1;
                                    }
                                    if (day14.Trim() == "A" || day14.Trim() == "a")
                                    {
                                        dt14 = 0;
                                    }
                                    if (day14.Trim() == "N" || day14.Trim() == "n")
                                    {
                                        nhs14 = 1;
                                    }
                                    //15
                                    if (day15.Trim() == "P" || day15.Trim() == "p")
                                    {
                                        dt15 = 1;
                                    }
                                    if (day15.Trim() == "P/P" || day15.Trim() == "p/p")
                                    {
                                        dt15 = 1; Ots15 = 1;
                                    }
                                    if (day15.Trim() == "J" || day15.Trim() == "j")
                                    {
                                        dt15 = 1.5f;
                                    }
                                    if (day15.Trim() == "W/O" || day15.Trim() == "w/o")
                                    {
                                        wo15 = 1;
                                    }
                                    if (day15.Trim() == "A" || day15.Trim() == "a")
                                    {
                                        dt15 = 0;
                                    }
                                    if (day15.Trim() == "N" || day15.Trim() == "n")
                                    {
                                        nhs15 = 1;
                                    }
                                    //16
                                    if (day16.Trim() == "P" || day16.Trim() == "p")
                                    {
                                        dt16 = 1;
                                    }
                                    if (day16.Trim() == "P/P" || day16.Trim() == "p/p")
                                    {
                                        dt16 = 1; Ots16 = 1;
                                    }
                                    if (day16.Trim() == "J" || day16.Trim() == "j")
                                    {
                                        dt16 = 1.5f;
                                    }
                                    if (day16.Trim() == "W/O" || day16.Trim() == "w/o")
                                    {
                                        wo16 = 1;
                                    }
                                    if (day16.Trim() == "A" || day16.Trim() == "a")
                                    {
                                        dt16 = 0;
                                    }
                                    if (day16.Trim() == "N" || day16.Trim() == "n")
                                    {
                                        nhs16 = 1;
                                    }
                                    //17
                                    if (day17.Trim() == "P" || day17.Trim() == "p")
                                    {
                                        dt17 = 1;
                                    }
                                    if (day17.Trim() == "P/P" || day17.Trim() == "p/p")
                                    {
                                        dt17 = 1; Ots17 = 1;
                                    }
                                    if (day17.Trim() == "J" || day17.Trim() == "j")
                                    {
                                        dt17 = 1.5f;
                                    }
                                    if (day17.Trim() == "W/O" || day17.Trim() == "w/o")
                                    {
                                        wo17 = 1;
                                    }
                                    if (day17.Trim() == "A" || day17.Trim() == "a")
                                    {
                                        dt17 = 0;
                                    }
                                    if (day17.Trim() == "N" || day17.Trim() == "n")
                                    {
                                        nhs17 = 1;
                                    }
                                    //18
                                    if (day18.Trim() == "P" || day18.Trim() == "p")
                                    {
                                        dt18 = 1;
                                    }
                                    if (day18.Trim() == "P/P" || day18.Trim() == "p/p")
                                    {
                                        dt18 = 1; Ots18 = 1;
                                    }
                                    if (day18.Trim() == "J" || day18.Trim() == "j")
                                    {
                                        dt18 = 1.5f;
                                    }
                                    if (day18.Trim() == "W/O" || day18.Trim() == "w/o")
                                    {
                                        wo18 = 1;
                                    }
                                    if (day18.Trim() == "A" || day18.Trim() == "a")
                                    {
                                        dt18 = 0;
                                    }
                                    if (day18.Trim() == "N" || day18.Trim() == "n")
                                    {
                                        nhs18 = 1;
                                    }
                                    //19
                                    if (day19.Trim() == "P" || day19.Trim() == "p")
                                    {
                                        dt19 = 1;
                                    }
                                    if (day19.Trim() == "P/P" || day19.Trim() == "p/p")
                                    {
                                        dt19 = 1; Ots19 = 1;
                                    }
                                    if (day19.Trim() == "J" || day19.Trim() == "j")
                                    {
                                        dt19 = 1.5f;
                                    }
                                    if (day19.Trim() == "W/O" || day19.Trim() == "w/o")
                                    {
                                        wo19 = 1;
                                    }
                                    if (day19.Trim() == "A" || day19.Trim() == "a")
                                    {
                                        dt19 = 0;
                                    }
                                    if (day19.Trim() == "N" || day19.Trim() == "n")
                                    {
                                        nhs19 = 1;
                                    }
                                    //20
                                    if (day20.Trim() == "P" || day20.Trim() == "p")
                                    {
                                        dt20 = 1;
                                    }
                                    if (day20.Trim() == "P/P" || day20.Trim() == "p/p")
                                    {
                                        dt20 = 1; Ots20 = 1;
                                    }
                                    if (day20.Trim() == "J" || day20.Trim() == "j")
                                    {
                                        dt20 = 1.5f;
                                    }
                                    if (day20.Trim() == "W/O" || day20.Trim() == "w/o")
                                    {
                                        wo20 = 1;
                                    }
                                    if (day20.Trim() == "A" || day20.Trim() == "a")
                                    {
                                        dt20 = 0;
                                    }
                                    if (day20.Trim() == "N" || day20.Trim() == "n")
                                    {
                                        nhs20 = 1;
                                    }
                                    //21
                                    if (day21.Trim() == "P" || day21.Trim() == "p")
                                    {
                                        dt21 = 1;
                                    }
                                    if (day21.Trim() == "P/P" || day21.Trim() == "p/p")
                                    {
                                        dt21 = 1; Ots21 = 1;
                                    }
                                    if (day21.Trim() == "J" || day21.Trim() == "j")
                                    {
                                        dt21 = 1.5f;
                                    }
                                    if (day21.Trim() == "W/O" || day21.Trim() == "w/o")
                                    {
                                        wo21 = 1;
                                    }
                                    if (day21.Trim() == "A" || day21.Trim() == "a")
                                    {
                                        dt21 = 0;
                                    }
                                    if (day21.Trim() == "N" || day21.Trim() == "n")
                                    {
                                        nhs21 = 1;
                                    }
                                    //22
                                    if (day22.Trim() == "P" || day22.Trim() == "p")
                                    {
                                        dt22 = 1;
                                    }
                                    if (day22.Trim() == "P/P" || day22.Trim() == "p/p")
                                    {
                                        dt22 = 1; Ots22 = 1;
                                    }
                                    if (day22.Trim() == "J" || day22.Trim() == "j")
                                    {
                                        dt22 = 1.5f;
                                    }
                                    if (day22.Trim() == "W/O" || day22.Trim() == "w/o")
                                    {
                                        wo22 = 1;
                                    }
                                    if (day22.Trim() == "A" || day22.Trim() == "a")
                                    {
                                        dt22 = 0;
                                    }
                                    if (day22.Trim() == "N" || day22.Trim() == "n")
                                    {
                                        nhs22 = 1;
                                    }
                                    //23
                                    if (day23.Trim() == "P" || day23.Trim() == "p")
                                    {
                                        dt23 = 1;
                                    }
                                    if (day23.Trim() == "P/P" || day23.Trim() == "p/p")
                                    {
                                        dt23 = 1; Ots23 = 1;
                                    }
                                    if (day23.Trim() == "J" || day23.Trim() == "j")
                                    {
                                        dt23 = 1.5f;
                                    }
                                    if (day23.Trim() == "W/O" || day23.Trim() == "w/o")
                                    {
                                        wo23 = 1;
                                    }
                                    if (day23.Trim() == "A" || day23.Trim() == "a")
                                    {
                                        dt23 = 0;
                                    }
                                    if (day23.Trim() == "N" || day23.Trim() == "n")
                                    {
                                        nhs23 = 1;
                                    }
                                    //24
                                    if (day24.Trim() == "P" || day24.Trim() == "p")
                                    {
                                        dt24 = 1;
                                    }
                                    if (day24.Trim() == "P/P" || day24.Trim() == "p/p")
                                    {
                                        dt24 = 1; Ots24 = 1;
                                    }
                                    if (day24.Trim() == "J" || day24.Trim() == "j")
                                    {
                                        dt24 = 1.5f;
                                    }
                                    if (day24.Trim() == "W/O" || day24.Trim() == "w/o")
                                    {
                                        wo24 = 1;
                                    }
                                    if (day24.Trim() == "A" || day24.Trim() == "a")
                                    {
                                        dt24 = 0;
                                    }
                                    if (day24.Trim() == "N" || day24.Trim() == "n")
                                    {
                                        nhs24 = 1;
                                    }
                                    //25
                                    if (day25.Trim() == "P" || day25.Trim() == "p")
                                    {
                                        dt25 = 1;
                                    }
                                    if (day25.Trim() == "P/P" || day25.Trim() == "p/p")
                                    {
                                        dt25 = 1; Ots25 = 1;
                                    }
                                    if (day25.Trim() == "J" || day25.Trim() == "j")
                                    {
                                        dt25 = 1.5f;
                                    }
                                    if (day25.Trim() == "W/O" || day25.Trim() == "w/o")
                                    {
                                        wo25 = 1;
                                    }
                                    if (day25.Trim() == "A" || day25.Trim() == "a")
                                    {
                                        dt25 = 0;
                                    }
                                    if (day25.Trim() == "N" || day25.Trim() == "n")
                                    {
                                        nhs25 = 1;
                                    }
                                    //26
                                    if (day26.Trim() == "P" || day26.Trim() == "p")
                                    {
                                        dt26 = 1;
                                    }
                                    if (day26.Trim() == "P/P" || day26.Trim() == "p/p")
                                    {
                                        dt26 = 1; Ots26 = 1;
                                    }
                                    if (day26.Trim() == "J" || day26.Trim() == "j")
                                    {
                                        dt26 = 1.5f;
                                    }
                                    if (day26.Trim() == "W/O" || day26.Trim() == "w/o")
                                    {
                                        wo26 = 1;
                                    }
                                    if (day26.Trim() == "A" || day26.Trim() == "a")
                                    {
                                        dt26 = 0;
                                    }
                                    if (day26.Trim() == "N" || day26.Trim() == "n")
                                    {
                                        nhs26 = 1;
                                    }
                                    //27
                                    if (day27.Trim() == "P" || day27.Trim() == "p")
                                    {
                                        dt27 = 1;
                                    }
                                    if (day27.Trim() == "P/P" || day27.Trim() == "p/p")
                                    {
                                        dt27 = 1; Ots27 = 1;
                                    }
                                    if (day27.Trim() == "J" || day27.Trim() == "j")
                                    {
                                        dt27 = 1.5f;
                                    }
                                    if (day27.Trim() == "W/O" || day27.Trim() == "w/o")
                                    {
                                        wo27 = 1;
                                    }
                                    if (day27.Trim() == "A" || day27.Trim() == "a")
                                    {
                                        dt27 = 0;
                                    }
                                    if (day27.Trim() == "N" || day27.Trim() == "n")
                                    {
                                        nhs27 = 1;
                                    }
                                    //28
                                    if (day28.Trim() == "P" || day28.Trim() == "p")
                                    {
                                        dt28 = 1;
                                    }
                                    if (day28.Trim() == "P/P" || day28.Trim() == "p/p")
                                    {
                                        dt28 = 1; Ots28 = 1;
                                    }
                                    if (day28.Trim() == "J" || day28.Trim() == "j")
                                    {
                                        dt28 = 1.5f;
                                    }
                                    if (day28.Trim() == "W/O" || day28.Trim() == "w/o")
                                    {
                                        wo28 = 1;
                                    }
                                    if (day28.Trim() == "A" || day28.Trim() == "a")
                                    {
                                        dt28 = 0;
                                    }
                                    if (day28.Trim() == "N" || day28.Trim() == "n")
                                    {
                                        nhs28 = 1;
                                    }
                                    //29
                                    if (day29.Trim() == "P" || day29.Trim() == "p")
                                    {
                                        dt29 = 1;
                                    }
                                    if (day29.Trim() == "P/P" || day29.Trim() == "p/p")
                                    {
                                        dt29 = 1; Ots29 = 1;
                                    }
                                    if (day29.Trim() == "J" || day29.Trim() == "j")
                                    {
                                        dt29 = 1.5f;
                                    }
                                    if (day29.Trim() == "W/O" || day29.Trim() == "w/o")
                                    {
                                        wo29 = 1;
                                    }
                                    if (day29.Trim() == "A" || day29.Trim() == "a")
                                    {
                                        dt29 = 0;
                                    }
                                    if (day29.Trim() == "N" || day29.Trim() == "n")
                                    {
                                        nhs29 = 1;
                                    }
                                    //30
                                    if (day30.Trim() == "P" || day30.Trim() == "p")
                                    {
                                        dt30 = 1;
                                    }
                                    if (day30.Trim() == "P/P" || day30.Trim() == "p/p")
                                    {
                                        dt30 = 1; Ots30 = 1;
                                    }
                                    if (day30.Trim() == "J" || day30.Trim() == "j")
                                    {
                                        dt30 = 1.5f;
                                    }
                                    if (day30.Trim() == "W/O" || day30.Trim() == "w/o")
                                    {
                                        wo30 = 1;
                                    }
                                    if (day30.Trim() == "A" || day30.Trim() == "a")
                                    {
                                        dt30 = 0;
                                    }
                                    if (day30.Trim() == "N" || day30.Trim() == "n")
                                    {
                                        nhs30 = 1;
                                    }
                                    //31
                                    if (day31.Trim() == "P" || day31.Trim() == "p")
                                    {
                                        dt31 = 1;
                                    }
                                    if (day31.Trim() == "P/P" || day31.Trim() == "p/p")
                                    {
                                        dt31 = 1; Ots31 = 1;
                                    }
                                    if (day31.Trim() == "J" || day31.Trim() == "j")
                                    {
                                        dt31 = 1.5f;
                                    }
                                    if (day31.Trim() == "W/O" || day31.Trim() == "w/o")
                                    {
                                        wo31 = 1;
                                    }
                                    if (day31.Trim() == "A" || day31.Trim() == "a")
                                    {
                                        dt31 = 0;
                                    }
                                    if (day31.Trim() == "N" || day31.Trim() == "n")
                                    {
                                        nhs31 = 1;
                                    }


                                    #endregion Values for Duties

                                    dayduties = dt1 + dt2 + dt3 + dt4 + dt5 + dt6 + dt7 + dt8 + dt9 + dt10 + dt11 + dt12 + dt13 + dt14 + dt15 + dt16 + dt17 + dt18 + dt19 + dt20 + dt21 +
                                 dt22 + dt23 + dt24 + dt25 + dt26 + dt27 + dt28 + dt29 + dt30 + dt31;

                                    dayots = Ots1 + Ots2 + Ots3 + Ots4 + Ots5 + Ots6 + Ots7 + Ots8 + Ots9 + Ots10 + Ots11 + Ots12 + Ots13 + Ots14 + Ots15 + Ots16 + Ots17 + Ots18 + Ots19 + Ots20 + Ots21 +
                               Ots22 + Ots23 + Ots24 + Ots25 + Ots26 + Ots27 + Ots28 + Ots29 + Ots30 + Ots31;

                                    daywos = wo1 + wo2 + wo3 + wo4 + wo5 + wo6 + wo7 + wo8 + wo9 + wo10 + wo11 + wo12 + wo13 + wo14 + wo15 + wo16 + wo17 + wo18 + wo19 + wo20 + wo21 +
                                 wo22 + wo23 + wo24 + wo25 + wo26 + wo27 + wo28 + wo29 + wo30 + wo31;

                                    dayNHS = nhs1 + nhs2 + nhs3 + nhs4 + nhs5 + nhs6 + nhs7 + nhs8 + nhs9 + nhs10 + nhs11 + nhs12 + nhs13 + nhs14 + nhs15 + nhs16 + nhs17 + nhs18 + nhs19 + nhs20 + nhs21 +
                                 nhs22 + nhs23 + nhs24 + nhs25 + nhs26 + nhs27 + nhs28 + nhs29 + nhs30 + nhs31;


                                    duties = dayduties + duties;
                                    ots = dayots + ots;
                                    Wos = daywos + Wos;

                                    NHS = dayNHS;

                                }


                                #region Begin New code for Stored Procedure as on 29/04/2014 by venkat


                                #region Begin code for passing values to the Stored Procedure as 29/04/2014 by Venkat

                                Hashtable Httable = new Hashtable();

                                Httable.Add("@k", k);
                                Httable.Add("@empidstatus", empstatus);


                                Httable.Add("@Clientid", clientid);
                                Httable.Add("@Month", Month);
                                Httable.Add("@Empid", empid);
                                Httable.Add("@ContractId", Contractid);
                                Httable.Add("@Design", design);

                                Httable.Add("@day1", day1);
                                Httable.Add("@day2", day2);
                                Httable.Add("@day3", day3);
                                Httable.Add("@day4", day4);
                                Httable.Add("@day5", day5);
                                Httable.Add("@day6", day6);
                                Httable.Add("@day7", day7);
                                Httable.Add("@day8", day8);
                                Httable.Add("@day9", day9);
                                Httable.Add("@day10", day10);
                                Httable.Add("@day11", day11);
                                Httable.Add("@day12", day12);
                                Httable.Add("@day13", day13);
                                Httable.Add("@day14", day14);
                                Httable.Add("@day15", day15);
                                Httable.Add("@day16", day16);
                                Httable.Add("@day17", day17);
                                Httable.Add("@day18", day18);
                                Httable.Add("@day19", day19);
                                Httable.Add("@day20", day20);
                                Httable.Add("@day21", day21);
                                Httable.Add("@day22", day22);
                                Httable.Add("@day23", day23);
                                Httable.Add("@day24", day24);
                                Httable.Add("@day25", day25);
                                Httable.Add("@day26", day26);
                                Httable.Add("@day27", day27);
                                Httable.Add("@day28", day28);
                                Httable.Add("@day29", day29);
                                Httable.Add("@day30", day30);
                                Httable.Add("@day31", day31);

                                Httable.Add("@day1ot", day1ot);
                                Httable.Add("@day2ot", day2ot);
                                Httable.Add("@day3ot", day3ot);
                                Httable.Add("@day4ot", day4ot);
                                Httable.Add("@day5ot", day5ot);
                                Httable.Add("@day6ot", day6ot);
                                Httable.Add("@day7ot", day7ot);
                                Httable.Add("@day8ot", day8ot);
                                Httable.Add("@day9ot", day9ot);
                                Httable.Add("@day10ot", day10ot);
                                Httable.Add("@day11ot", day11ot);
                                Httable.Add("@day12ot", day12ot);
                                Httable.Add("@day13ot", day13ot);
                                Httable.Add("@day14ot", day14ot);
                                Httable.Add("@day15ot", day15ot);
                                Httable.Add("@day16ot", day16ot);
                                Httable.Add("@day17ot", day17ot);
                                Httable.Add("@day18ot", day18ot);
                                Httable.Add("@day19ot", day19ot);
                                Httable.Add("@day20ot", day20ot);
                                Httable.Add("@day21ot", day21ot);
                                Httable.Add("@day22ot", day22ot);
                                Httable.Add("@day23ot", day23ot);
                                Httable.Add("@day24ot", day24ot);
                                Httable.Add("@day25ot", day25ot);
                                Httable.Add("@day26ot", day26ot);
                                Httable.Add("@day27ot", day27ot);
                                Httable.Add("@day28ot", day28ot);
                                Httable.Add("@day29ot", day29ot);
                                Httable.Add("@day30ot", day30ot);
                                Httable.Add("@day31ot", day31ot);

                                Httable.Add("@Duties", duties);
                                Httable.Add("@Ots", ots);
                                Httable.Add("@WOs", Wos);
                                Httable.Add("@NHs", NHS);
                                Httable.Add("@Sno", Sno);
                                Httable.Add("@Excel_Number", ExcelNo);

                                Httable.Add("@CanteenAdv", canteenadvance);
                                Httable.Add("@Leaves", Leaves);
                                Httable.Add("@Incentivs", incentives);
                                Httable.Add("@RentDed", RentDed);




                                #endregion

                                string SPName = "ImportAttendanceFromExcel";

                                DataTable dtstatus = config.ExecuteAdaptorAsyncWithParams(SPName, Httable).Result;


                                #endregion


                            }

                            //End Create Procedure

                        }

                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please enter Clientid');", true);
                        //}
                        k++;
                    }

                    // ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Attendance is Successfully Uploaded');", true);



                }

                string qry1 = "select ea.clientid,ea.EmpId,(ed.empfname+' '+ed.empmname+' '+ed.emplname) as fullname,d.Design,NoOfDuties,ot,nhs,wo from empattendance ea inner join empdetails ed on ea.empid=ed.empid inner join designations d on d.DesignId=ea.Design where month='" + Month + "' and excelnumber='" + ExcelNo + "'";
                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry1).Result;
                if (dt.Rows.Count > 0)
                {
                    GVEmployeeList.Visible = true;
                    GVEmployeeList.DataSource = dt;
                    GVEmployeeList.DataBind();

                }
                else
                {
                    GVEmployeeList.Visible = false;
                    GVEmployeeList.DataSource = null;
                    GVEmployeeList.DataBind();


                }


                #endregion
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please Upload Valid Data');", true);
                lblMessage.Visible = false;

            }


            FillAttendanceGrid();
            DismatchDesignation();
            ddlAttendanceMode.SelectedIndex = 0;
            //importunsaveddata();

        }

        protected void importunsaveddata()
        {

            //if (GridView2.Rows.Count > 0)
            //{
            //    GridViewExportUtil.Export("UnsavedData.xls", this.GridView2);
            //}
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (GridView2.Rows.Count > 0)
            {
                util.Export("Unsaveddata.xls", this.GridView2);
            }

        }

        float totalDuties = 0;
        float totalWos = 0;
        float totalNHS = 0;
        float totalOts = 0;

        float totalCanteenAdv = 0;
        float totalPenalty = 0;
        float totalIncentives = 0;
        float totalNa = 0;
        float totalAb = 0;
        float grandTotal = 0;

        protected void gvAttendancestatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblTotDuties = e.Row.FindControl("lblTotDuties") as Label;
                    totalDuties += float.Parse(lblTotDuties.Text);


                    Label lblTotOts = e.Row.FindControl("lblTotOts") as Label;
                    totalOts += float.Parse(lblTotOts.Text);


                    Label lblTotal = e.Row.FindControl("lblTotal") as Label;
                    grandTotal += float.Parse(lblTotal.Text);

                    #region code commented by swathi on 30-11-2015

                    //Label lblTotWos = e.Row.FindControl("lblTotWos") as Label;
                    //totalWos += float.Parse(lblTotWos.Text);

                    //Label lblTotNHS = e.Row.FindControl("lblTotNHS") as Label;
                    //totalNHS += float.Parse(lblTotNHS.Text);

                    //Label lblTotOts1 = e.Row.FindControl("lblTotOts1") as Label;
                    //totalOts1 += float.Parse(lblTotOts1.Text);

                    //Label lblTotCanteenadv = e.Row.FindControl("lblTotCanteenadv") as Label;
                    //totalCanteenAdv += float.Parse(lblTotCanteenadv.Text);

                    //Label lblTotPenalty = e.Row.FindControl("lblTotPenalty") as Label;
                    //totalPenalty += float.Parse(lblTotPenalty.Text);

                    //Label lblTotIncentives = e.Row.FindControl("lblTotIncentives") as Label;
                    //totalIncentives += float.Parse(lblTotIncentives.Text);

                    //Label lblTotNa = e.Row.FindControl("lblTotNa") as Label;
                    //totalNa += float.Parse(lblTotNa.Text);

                    //Label lblTotAb = e.Row.FindControl("lblTotAb") as Label;
                    //totalAb += float.Parse(lblTotAb.Text);

                    #endregion code commented by swathi on 30-11-2015

                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblGTotDuties = e.Row.FindControl("lblGTotDuties") as Label;
                    lblGTotDuties.Text = totalDuties.ToString();


                    Label lblGTotOts = e.Row.FindControl("lblGTotOts") as Label;
                    lblGTotOts.Text = totalOts.ToString();

                    #region code commented by swathi on 30-11-2015


                    //Label lblGTotWos = e.Row.FindControl("lblGTotWos") as Label;
                    //lblGTotWos.Text = totalWos.ToString();

                    //Label lblGTotNHS = e.Row.FindControl("lblGTotNHS") as Label;
                    //lblGTotNHS.Text = totalNHS.ToString();

                    //Label lblGTotOts1 = e.Row.FindControl("lblGTotOts1") as Label;
                    //lblGTotOts1.Text = totalOts1.ToString();

                    //Label lblGTotCanteenadv = e.Row.FindControl("lblGTotCanteenadv") as Label;
                    //lblGTotCanteenadv.Text = totalCanteenAdv.ToString();

                    //Label lblGTotPenalty = e.Row.FindControl("lblGTotPenalty") as Label;
                    //lblGTotPenalty.Text = totalPenalty.ToString();

                    //Label lblGTotIncentives = e.Row.FindControl("lblGTotIncentives") as Label;
                    //lblGTotIncentives.Text = totalIncentives.ToString();

                    //Label lblGTotNa = e.Row.FindControl("lblGTotNa") as Label;
                    //lblGTotNa.Text = totalNa.ToString();

                    //Label lblGTotAb = e.Row.FindControl("lblGTotAb") as Label;
                    //lblGTotAb.Text = totalAb.ToString();

                    Label lblGTotal = e.Row.FindControl("lblGTotal") as Label;
                    lblGTotal.Text = grandTotal.ToString();

                    #endregion code commented by swathi on 30-11-2015
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            //int month = 0;
            //if (ddlMonth.SelectedIndex == 1)
            //{
            //    month = GlobalData.Instance.GetIDForNextMonth();
            //}
            //else
            //{
            //    if (ddlMonth.SelectedIndex == 2)
            //    {
            //        month = GlobalData.Instance.GetIDForThisMonth();
            //    }
            //    if (ddlMonth.SelectedIndex == 3)
            //    {
            //        month = GlobalData.Instance.GetIDForPrviousMonth();
            //    }
            //}
            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string Month = month + Year.Substring(2, 2);

            if (ddlClientID.SelectedIndex > 0 && txtmonth.Text.Trim().Length != 0)
            {

                string sqldeleteempattendance = "delete empattendance where clientid='" + ddlClientID.SelectedValue + "' and month='" + Month + "'";
                int status = config.ExecuteNonQueryWithQueryAsync(sqldeleteempattendance).Result;
                if (status > 0)
                {
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    gvAttendancestatus.DataSource = null;
                    gvAttendancestatus.DataBind();
                    GridView2.DataSource = null;
                    GridView2.DataBind();
                    lblMessage.Text = string.Empty;
                }


            }
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            //int month = 0;
            //if (ddlMonth.SelectedIndex == 1)
            //{
            //    month = GlobalData.Instance.GetIDForNextMonth();
            //}
            //else
            //{
            //    if (ddlMonth.SelectedIndex == 2)
            //    {
            //        month = GlobalData.Instance.GetIDForThisMonth();
            //    }
            //    if (ddlMonth.SelectedIndex == 3)
            //    {
            //        month = GlobalData.Instance.GetIDForPrviousMonth();
            //    }
            //}

            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string Month = month + Year.Substring(2, 2);

            if (ddlClientID.SelectedIndex > 0 && txtmonth.Text.Trim().Length != 0)
            {

                string sqldeleteempattendance = "delete empattendance where clientid='" + ddlClientID.SelectedValue + "' and month='" + Month + "'";
                int status = config.ExecuteNonQueryWithQueryAsync(sqldeleteempattendance).Result;

                string sqldeletepostingorder = "delete EmpPostingOrder where Tounitid='" + ddlClientID.SelectedValue + "'";
                status = config.ExecuteNonQueryWithQueryAsync(sqldeletepostingorder).Result;
                if (status > 0)
                {
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    gvAttendancestatus.DataSource = null;
                    gvAttendancestatus.DataBind();
                    GridView2.DataSource = null;
                    GridView2.DataBind();
                    lblMessage.Text = string.Empty;

                }


            }
        }

        protected void lnkImportfromexcel_Click(object sender, EventArgs e)
        {//
            if (ddlAttendanceMode.SelectedIndex == 0)
            {
                util.Export("Employee Attendance.xls", this.grvSample2);
            }
            if (ddlAttendanceMode.SelectedIndex == 1)
            {
                util.Export("Employee Attendance.xls", this.SampleGrid);
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            util.Export("Employee Attendance.xls", this.GridView1);
        }
    }
}