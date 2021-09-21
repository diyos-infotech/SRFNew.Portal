using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class QuarterlyandYearlyReport : System.Web.UI.Page
    {
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";

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

                    LoadClientList();
                    LoadClientNames();

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

        protected void LoadClientNames()
        {
            DataTable DtClientids = GlobalData.Instance.LoadCNames(CmpIDPrefix);
            if (DtClientids.Rows.Count > 0)
            {
                ddlcname.DataValueField = "Clientid";
                ddlcname.DataTextField = "clientname";
                ddlcname.DataSource = DtClientids;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "-Select-");
            ddlcname.Items.Insert(1, "ALL");

        }

        protected void LoadClientList()
        {
            DataTable DtClientNames = GlobalData.Instance.LoadCIds(CmpIDPrefix);
            if (DtClientNames.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "Clientid";
                ddlclientid.DataTextField = "Clientid";
                ddlclientid.DataSource = DtClientNames;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "-Select-");
            ddlclientid.Items.Insert(1, "ALL");
        }

        protected void ClearData()
        {
            //LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
            // lbtn_Export.Visible = false;
        }

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlcname.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlclientid.SelectedValue = ddlcname.SelectedValue;

            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }

            if (ddlclientid.SelectedIndex == 1)
            {
                lbtn_Export.Visible = true;

                btn_Submit.Visible = true;

            }

            else if (ddlclientid.SelectedIndex > 1)
            {

                lbtn_Export.Visible = false;
                btn_Submit.Visible = true;

            }
        }

        protected void ddlclientid_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();

            if (ddlclientid.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlcname.SelectedValue = ddlclientid.SelectedValue;
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }

            if (ddlclientid.SelectedIndex == 1)
            {
                lbtn_Export.Visible = true;

                btn_Submit.Visible = true;

            }

            else if (ddlclientid.SelectedIndex > 1)
            {
                //lbtn_Export_Ind.Visible = true;
                //lbtn_Export.Visible = false;
                //btn_Submit.Visible = true;

                lbtn_Export.Visible = true;

                btn_Submit.Visible = true;


            }
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            string filename = "Quarterly&YearlyReport.xls";
            GVUtil.Export(filename, GVListEmployees);
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string SPName = "";
            string date = string.Empty;

            if (ddlType.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select Type');", true);
                return;
            }

            if (ddlComponent.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select Component');", true);
                return;
            }


            if (ddlclientid.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select Clientid');", true);
                return;
            }


            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select Month');", true);
                return;
            }

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("MM/dd/yyyy");
            }
            string clientid = "";
            if (ddlclientid.SelectedIndex == 1)
            {
                clientid = "%";
            }
            else
            {
                clientid = ddlclientid.SelectedValue;
            }
            string TypeofBcomponent = "";
            string TypeofLAcomponent = "";
            string TypeofGcomponent = "";

            if (ddlComponent.SelectedIndex == 1)
            {
                TypeofLAcomponent = ddlType.SelectedValue.ToString();
                TypeofBcomponent = "-1";
                TypeofGcomponent = "-1";
            }

            if (ddlComponent.SelectedIndex == 2)
            {
                TypeofGcomponent = ddlType.SelectedValue.ToString();
                TypeofLAcomponent = "-1";
                TypeofBcomponent = "-1";
            }

            if (ddlComponent.SelectedIndex == 3)
            {
                TypeofBcomponent = ddlType.SelectedValue.ToString();
                TypeofLAcomponent = "-1";
                TypeofGcomponent = "-1";
            }
            int component = ddlComponent.SelectedIndex;

            Hashtable ht = new Hashtable();
            SPName = "QtrlyTestNew";
            ht.Add("@Month1", date);
            ht.Add("@clientval", clientid);
            ht.Add("@Type", ddlType.SelectedValue);
            ht.Add("@TypeofBcomponent", TypeofBcomponent);
            ht.Add("@TypeofLAcomponent", TypeofLAcomponent);
            ht.Add("@TypeofGcomponent", TypeofGcomponent);
            ht.Add("@component", component);

            DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            if (dt.Rows.Count > 1)
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

        protected void GVListEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string month = "";
            string monthnew = "";
            string Year = "";
            string Year2 = "";
            string date = "";
            string monthpre1 = "";
            string monthpre2 = "";
            string monthpre3 = "";
            string monthpre4 = "";
            string monthpre5 = "";
            string monthpre6 = "";
            string monthpre7 = "";
            string monthpre8 = "";
            string monthpre9 = "";
            string monthpre10 = "";
            string monthpre11 = "";
            string year1 = "";
            string year2 = "";
            string year3 = "";
            string year4 = "";
            string year5 = "";
            string year6 = "";
            string year7 = "";
            string year8 = "";
            string year9 = "";
            string year10 = "";
            string year11 = "";

            string typeRate = "";
            string typeforComponent = "";
            string totalNoofDuties = "";
            string totalCompAmount = "";



            if (ddlComponent.SelectedIndex == 1)
            {
                typeRate = "Leave Amount Rate";
                typeforComponent = "Leave Amount";
                totalNoofDuties = "Total NoofDuties";
                totalCompAmount = "TotalLeave Amount";
            }
            if (ddlComponent.SelectedIndex == 2)
            {
                typeRate = "Gratuity Rate";
                typeforComponent = "Gratuity";
                totalNoofDuties = "Total NoofDuties";
                totalCompAmount = "TotalGratuity";
            }
            if (ddlComponent.SelectedIndex == 3)
            {
                typeRate = "Bonus Rate";
                typeforComponent = "Bonus";
                totalNoofDuties = "Total NoofDuties";
                totalCompAmount = "TotalBonus";
            }

            if (ddlType.SelectedIndex == 1)
            {
                if (txtmonth.Text.Trim().Length > 0)
                {
                    date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                    year1 = DateTime.Parse(date).AddMonths(-1).ToString("yy");
                    year2 = DateTime.Parse(date).AddMonths(-2).ToString("yy");

                    month = DateTime.Parse(date).Month.ToString();
                    monthnew = DateTime.Parse(date).Month.ToString("00");
                    monthpre1 = DateTime.Parse(date).AddMonths(-1).Month.ToString("00");
                    monthpre2 = DateTime.Parse(date).AddMonths(-2).Month.ToString("00");
                    Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);
                }

                int monthprev1 = Convert.ToInt32(monthpre1);
                int monthprev2 = Convert.ToInt32(monthpre2);

                string monthname1 = string.Empty;
                string monthname2 = string.Empty;
                DateTimeFormatInfo mfi = new DateTimeFormatInfo();
                //DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                monthname1 = mfi.GetMonthName(monthprev1).ToString();
                monthname2 = mfi.GetMonthName(monthprev2).ToString();


                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.Cells[0].Text == "99999")
                    {
                        e.Row.Cells[0].Text = "";
                    }
                }



                e.Row.Cells[3].Attributes.Add("class", "text");

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[6].Text = typeRate;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[7].Text = GetMonthName() + "-" + Year;
                    e.Row.Cells[7].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[8].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[9].Text = totalNoofDuties;

                }


                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[10].Text = totalCompAmount;

                }

            }

            if (ddlType.SelectedIndex == 2)
            {
                if (txtmonth.Text.Trim().Length > 0)
                {
                    date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                    year1 = DateTime.Parse(date).AddMonths(-1).ToString("yy");
                    year2 = DateTime.Parse(date).AddMonths(-2).ToString("yy");

                    month = DateTime.Parse(date).Month.ToString();
                    monthnew = DateTime.Parse(date).Month.ToString("00");
                    monthpre1 = DateTime.Parse(date).AddMonths(-1).Month.ToString("00");
                    monthpre2 = DateTime.Parse(date).AddMonths(-2).Month.ToString("00");
                    Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);
                }

                int monthprev1 = Convert.ToInt32(monthpre1);
                int monthprev2 = Convert.ToInt32(monthpre2);

                string monthname1 = string.Empty;
                string monthname2 = string.Empty;
                DateTimeFormatInfo mfi = new DateTimeFormatInfo();
                //DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                monthname1 = mfi.GetMonthName(monthprev1).ToString();
                monthname2 = mfi.GetMonthName(monthprev2).ToString();

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.Cells[0].Text == "99999")
                    {
                        e.Row.Cells[0].Text = "";
                    }
                }

                e.Row.Cells[3].Attributes.Add("class", "text");

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[6].Text = typeRate;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[7].Text = GetMonthName() + "-" + Year;
                    e.Row.Cells[7].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[8].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[9].Text = monthname1 + "-" + year1;
                    e.Row.Cells[9].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[10].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[11].Text = monthname2 + "-" + year2;
                    e.Row.Cells[11].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[12].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[13].Text = totalNoofDuties;

                }


                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[14].Text = totalCompAmount;

                }
            }

            if (ddlType.SelectedIndex == 3)
            {
                if (txtmonth.Text.Trim().Length > 0)
                {
                    date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();

                    year1 = DateTime.Parse(date).AddMonths(-1).ToString("yy");
                    year2 = DateTime.Parse(date).AddMonths(-2).ToString("yy");
                    year3 = DateTime.Parse(date).AddMonths(-3).ToString("yy");
                    year4 = DateTime.Parse(date).AddMonths(-4).ToString("yy");
                    year5 = DateTime.Parse(date).AddMonths(-5).ToString("yy");

                    month = DateTime.Parse(date).Month.ToString();
                    monthnew = DateTime.Parse(date).Month.ToString("00");
                    monthpre1 = DateTime.Parse(date).AddMonths(-1).Month.ToString("00");
                    monthpre2 = DateTime.Parse(date).AddMonths(-2).Month.ToString("00");
                    monthpre3 = DateTime.Parse(date).AddMonths(-3).Month.ToString("00");
                    monthpre4 = DateTime.Parse(date).AddMonths(-4).Month.ToString("00");
                    monthpre5 = DateTime.Parse(date).AddMonths(-5).Month.ToString("00");
                    Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);
                }

                int monthprev1 = Convert.ToInt32(monthpre1);
                int monthprev2 = Convert.ToInt32(monthpre2);
                int monthprev3 = Convert.ToInt32(monthpre3);
                int monthprev4 = Convert.ToInt32(monthpre4);
                int monthprev5 = Convert.ToInt32(monthpre5);

                string monthname1 = string.Empty;
                string monthname2 = string.Empty;
                string monthname3 = string.Empty;
                string monthname4 = string.Empty;
                string monthname5 = string.Empty;
                DateTimeFormatInfo mfi = new DateTimeFormatInfo();
                //DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                monthname1 = mfi.GetMonthName(monthprev1).ToString();
                monthname2 = mfi.GetMonthName(monthprev2).ToString();
                monthname3 = mfi.GetMonthName(monthprev3).ToString();
                monthname4 = mfi.GetMonthName(monthprev4).ToString();
                monthname5 = mfi.GetMonthName(monthprev5).ToString();

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.Cells[0].Text == "99999")
                    {
                        e.Row.Cells[0].Text = "";
                    }
                }

                e.Row.Cells[3].Attributes.Add("class", "text");

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[6].Text = typeRate;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[7].Text = GetMonthName() + "-" + Year;
                    e.Row.Cells[7].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[8].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[9].Text = monthname1 + "-" + year1;
                    e.Row.Cells[9].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[10].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[11].Text = monthname2 + "-" + year2;
                    e.Row.Cells[11].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[12].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[13].Text = monthname3 + "-" + year3;
                    e.Row.Cells[13].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[14].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[15].Text = monthname4 + "-" + year4;
                    e.Row.Cells[15].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[16].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[17].Text = monthname5 + "-" + year5;
                    e.Row.Cells[17].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[18].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[19].Text = totalNoofDuties;


                }


                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[20].Text = totalCompAmount;

                }
            }
            if (ddlType.SelectedIndex == 4)
            {
                if (txtmonth.Text.Trim().Length > 0)
                {
                    date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                    month = DateTime.Parse(date).Month.ToString();
                    Year = DateTime.Parse(date).ToString("yy");
                    year1 = DateTime.Parse(date).AddMonths(-1).ToString("yy");
                    year2 = DateTime.Parse(date).AddMonths(-2).ToString("yy");
                    year3 = DateTime.Parse(date).AddMonths(-3).ToString("yy");
                    year4 = DateTime.Parse(date).AddMonths(-4).ToString("yy");
                    year5 = DateTime.Parse(date).AddMonths(-5).ToString("yy");
                    year6 = DateTime.Parse(date).AddMonths(-6).ToString("yy");
                    year7 = DateTime.Parse(date).AddMonths(-7).ToString("yy");
                    year8 = DateTime.Parse(date).AddMonths(-8).ToString("yy");
                    year9 = DateTime.Parse(date).AddMonths(-9).ToString("yy");
                    year10 = DateTime.Parse(date).AddMonths(-10).ToString("yy");
                    year11 = DateTime.Parse(date).AddMonths(-11).ToString("yy");

                    date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                    month = DateTime.Parse(date).Month.ToString();
                    monthnew = DateTime.Parse(date).Month.ToString();
                    monthpre1 = DateTime.Parse(date).AddMonths(-1).Month.ToString("00");
                    monthpre2 = DateTime.Parse(date).AddMonths(-2).Month.ToString("00");
                    monthpre3 = DateTime.Parse(date).AddMonths(-3).Month.ToString("00");
                    monthpre4 = DateTime.Parse(date).AddMonths(-4).Month.ToString("00");
                    monthpre5 = DateTime.Parse(date).AddMonths(-5).Month.ToString("00");
                    monthpre6 = DateTime.Parse(date).AddMonths(-6).Month.ToString("00");
                    monthpre7 = DateTime.Parse(date).AddMonths(-7).Month.ToString("00");
                    monthpre8 = DateTime.Parse(date).AddMonths(-8).Month.ToString("00");
                    monthpre9 = DateTime.Parse(date).AddMonths(-9).Month.ToString("00");
                    monthpre10 = DateTime.Parse(date).AddMonths(-10).Month.ToString("00");
                    monthpre11 = DateTime.Parse(date).AddMonths(-11).Month.ToString("00");
                    Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);
                    Year2 = DateTime.Parse(date).AddYears(-1).Year.ToString().Substring(2, 2);

                }

                int monthprev1 = Convert.ToInt32(monthpre1);
                int monthprev2 = Convert.ToInt32(monthpre2);
                int monthprev3 = Convert.ToInt32(monthpre3);
                int monthprev4 = Convert.ToInt32(monthpre4);
                int monthprev5 = Convert.ToInt32(monthpre5);
                int monthprev6 = Convert.ToInt32(monthpre6);
                int monthprev7 = Convert.ToInt32(monthpre7);
                int monthprev8 = Convert.ToInt32(monthpre8);
                int monthprev9 = Convert.ToInt32(monthpre9);
                int monthprev10 = Convert.ToInt32(monthpre10);
                int monthprev11 = Convert.ToInt32(monthpre11);

                string monthname1 = string.Empty;
                string monthname2 = string.Empty;
                string monthname3 = string.Empty;
                string monthname4 = string.Empty;
                string monthname5 = string.Empty;
                string monthname6 = string.Empty;
                string monthname7 = string.Empty;
                string monthname8 = string.Empty;
                string monthname9 = string.Empty;
                string monthname10 = string.Empty;
                string monthname11 = string.Empty;
                DateTimeFormatInfo mfi = new DateTimeFormatInfo();
                //DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                monthname1 = mfi.GetMonthName(monthprev1).ToString();
                monthname2 = mfi.GetMonthName(monthprev2).ToString();
                monthname3 = mfi.GetMonthName(monthprev3).ToString();
                monthname4 = mfi.GetMonthName(monthprev4).ToString();
                monthname5 = mfi.GetMonthName(monthprev5).ToString();
                monthname6 = mfi.GetMonthName(monthprev6).ToString();
                monthname7 = mfi.GetMonthName(monthprev7).ToString();
                monthname8 = mfi.GetMonthName(monthprev8).ToString();
                monthname9 = mfi.GetMonthName(monthprev9).ToString();
                monthname10 = mfi.GetMonthName(monthprev10).ToString();
                monthname11 = mfi.GetMonthName(monthprev11).ToString();

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.Cells[0].Text == "99999")
                    {
                        e.Row.Cells[0].Text = "";
                    }
                }

                e.Row.Cells[3].Attributes.Add("class", "text");

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[6].Text = typeRate;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[7].Text = GetMonthName() + "-" + Year;
                    e.Row.Cells[7].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[8].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[9].Text = monthname1 + "-" + year1;
                    e.Row.Cells[9].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[10].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[11].Text = monthname2 + "-" + year2;
                    e.Row.Cells[11].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[12].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[13].Text = monthname3 + "-" + year3;
                    e.Row.Cells[13].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[14].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[15].Text = monthname4 + "-" + year4;
                    e.Row.Cells[15].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[16].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[17].Text = monthname5 + "-" + year5;
                    e.Row.Cells[17].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[18].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[19].Text = monthname6 + "-" + year6;
                    e.Row.Cells[19].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[20].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[21].Text = monthname7 + "-" + year7;
                    e.Row.Cells[21].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[22].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[23].Text = monthname8 + "-" + year8;
                    e.Row.Cells[23].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[24].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[25].Text = monthname9 + "-" + year9;
                    e.Row.Cells[25].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[26].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[27].Text = monthname10 + "-" + year10;
                    e.Row.Cells[27].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[28].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[29].Text = monthname11 + "-" + year11;
                    e.Row.Cells[29].Attributes.Add("class", "text");
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[30].Text = typeforComponent;

                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[31].Text = totalNoofDuties;


                }


                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[32].Text = totalCompAmount;

                }
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
    }
}