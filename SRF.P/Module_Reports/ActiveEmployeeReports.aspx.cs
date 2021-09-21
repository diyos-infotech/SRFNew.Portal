using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using OfficeOpenXml;
using System.IO;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class ActiveEmployeeReports : System.Web.UI.Page
    {
        string CmpIDPrefix = "";
        string EmpIDPrefix = "";

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

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired . Please Login');", true);
                Response.Redirect("~/Login.aspx");
            }

        }


        protected void GVListEmployees_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVListEmployees.PageIndex = e.NewPageIndex;
        }


        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        protected void ClearData()
        {
            gvlistofemp.DataSource = null;
            gvlistofemp.DataBind();
        }

        protected void ddlActiveEmp_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cleardata();

            string SqlQueryForEmp = "";

            if (ddlActiveEmp.SelectedValue == "EmpId")
            {

                panelempid.Visible = true;
                // panelfromto.Visible = false;

            }
            else
            {
                panelempid.Visible = false;
                panelfromto.Visible = false;
                panelEmpIdFromTo.Visible = false;
                gvnomineelist.Visible = false;
                GVEmpDetails.Visible = false;
                gvlistofadhar.Visible = false;
                GVListEmployees.Visible = false;
            }

            if (ddlActiveEmp.SelectedValue == "EmpName")
            {

                panelemp.Visible = true;
                panelfromto.Visible = false;
                panelEmpIdFromTo.Visible = false;

            }
            else
            {
                panelemp.Visible = false;
                panelfromto.Visible = false;
                panelEmpIdFromTo.Visible = false;

                gvnomineelist.Visible = false;
                GVEmpDetails.Visible = false;
                gvlistofadhar.Visible = false;
                GVListEmployees.Visible = false;

            }

            if (ddlActiveEmp.SelectedValue == "Designation")
            {

                paneldesignation.Visible = true;
                FillDesgn();
                panelfromto.Visible = false;
                panelEmpIdFromTo.Visible = false;

            }
            else
            {
                paneldesignation.Visible = false;
                panelfromto.Visible = false;
                gvnomineelist.Visible = false;
                GVEmpDetails.Visible = false;
                gvlistofadhar.Visible = false;
                panelEmpIdFromTo.Visible = false;
                GVListEmployees.Visible = false;


            }

            if (ddlActiveEmp.SelectedValue == "JoiningDate")
            {

                panelJdate.Visible = true;
                panelfromto.Visible = false;
                panelEmpIdFromTo.Visible = false;


            }
            else
            {
                panelJdate.Visible = false;
                panelfromto.Visible = false;
                gvnomineelist.Visible = false;
                GVEmpDetails.Visible = false;
                gvlistofadhar.Visible = false;
                panelEmpIdFromTo.Visible = false;
                GVListEmployees.Visible = false;

            }


            if (ddlActiveEmp.SelectedValue == "LeavingDate")
            {

                panelLdate.Visible = true;
                panelfromto.Visible = false;
                panelEmpIdFromTo.Visible = false;


            }
            else
            {
                panelLdate.Visible = false;
                panelfromto.Visible = false;
                gvnomineelist.Visible = false;
                GVEmpDetails.Visible = false;
                gvlistofadhar.Visible = false;
                panelEmpIdFromTo.Visible = false;
                GVListEmployees.Visible = false;

            }
            if (ddlActiveEmp.SelectedValue == "NonAttendance")
            {

                panelNonAtten.Visible = true;
                panelfromto.Visible = false;
                panelEmpIdFromTo.Visible = false;


            }
            else
            {
                panelNonAtten.Visible = false;
                panelfromto.Visible = false;
                gvnomineelist.Visible = false;
                GVEmpDetails.Visible = false;
                gvlistofadhar.Visible = false;
                panelEmpIdFromTo.Visible = false;
                GVListEmployees.Visible = false;

            }

            if (ddlActiveEmp.SelectedValue == "EmployeeDetails")
            {

                panelfromto.Visible = true;
                GVEmpDetails.Visible = true;


            }
            else
            {
                panelNonAtten.Visible = false;
            }

            if (ddlActiveEmp.SelectedValue == "EmployeeAadharDetails")
            {

                panelfromto.Visible = true;


            }
            else
            {
                panelNonAtten.Visible = false;
                GVListEmployees.Visible = false;

            }

            if (ddlActiveEmp.SelectedValue == "EmployeeNomineeDetails")
            {

                panelfromto.Visible = true;
                GVListEmployees.Visible = false;


            }
            else
            {
                panelNonAtten.Visible = false;
                GVListEmployees.Visible = false;

            }
            if (ddlActiveEmp.SelectedValue == "PSTDataFormat")
            {
                GVListEmployees.Visible = true;
                LinkButton4.Visible = true;
                panelEmpIdFromTo.Visible = true;
            }
            else
            {
                panelEmpIdFromTo.Visible = false;
                GVListEmployees.Visible = false;
                LinkButton4.Visible = false;
            }
        }

        protected void Cleardata()
        {
            gvlistofemp.DataSource = null;
            gvlistofemp.DataBind();
        }


        protected void FillDesgn()
        {
            //string SqlQryDesgn = "Select Design from Designations";
            DataTable dtDesgn = GlobalData.Instance.LoadDesigns();
            if (dtDesgn.Rows.Count > 0)
            {
                ddldesgn.DataValueField = "Designid";
                ddldesgn.DataTextField = "Design";
                ddldesgn.DataSource = dtDesgn;
                ddldesgn.DataBind();
            }
            ddldesgn.Items.Insert(0, "-- Select --");

        }

        protected void Esearch_Click(object sender, EventArgs e)
        {
            try
            {
                Cleardata();

                string SqlQueryEmp = "";
                int status = ddlActiveEmp.SelectedIndex;
                Hashtable HTEmpList = new Hashtable();
                string spname = "IMReportForListOfEmployees";

                if (status == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('please  select search mode');", true);
                    return;
                }

                if (status == 1)
                {
                    HTEmpList.Add("@Status", 1);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }
                if (status == 2)
                {
                    HTEmpList.Add("@Status", 2);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }
                if (status == 3)
                {
                    HTEmpList.Add("@Status", 3);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }

                if (status == 4)
                {
                    HTEmpList.Add("@Status", 4);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }

                if (status == 5)
                {

                    if (TextEmpid.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The employee Id');", true);
                        return;
                    }
                    HTEmpList.Add("@Empid", TextEmpid.Text.Trim());
                    HTEmpList.Add("@Status", 5);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }
                if (status == 6)
                {

                    if (TxtEmpname.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The employee Name');", true);
                        return;
                    }
                    HTEmpList.Add("@EmpName", TxtEmpname.Text.Trim());
                    HTEmpList.Add("@Status", 6);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }
                if (status == 7)
                {
                    if (ddldesgn.SelectedIndex == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Select The Designation');", true);
                        return;
                    }
                    HTEmpList.Add("@Status", 7);
                    HTEmpList.Add("@Designation", ddldesgn.SelectedValue);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }
                if (status == 8)
                {
                    if (TxtJdateFrom.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The From Date');", true);
                        return;
                    }


                    if (TxtJdateTo.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The To Date');", true);
                        return;
                    }
                    HTEmpList.Add("@Status", 8);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                    HTEmpList.Add("@JdateFrom", TxtJdateFrom.Text.Trim());
                    HTEmpList.Add("@JdateTo", TxtJdateTo.Text.Trim());

                }

                if (status == 9)
                {

                    if (TxtLdateFrom.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The From Date');", true);
                        return;
                    }

                    if (TxtLdateTo.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The To Date');", true);
                        return;
                    }
                    string dateFrom = DateTime.Parse(TxtLdateFrom.Text.Trim(), CultureInfo.GetCultureInfo("en-GB")).ToString();
                    string dateTo = DateTime.Parse(TxtLdateTo.Text.Trim(), CultureInfo.GetCultureInfo("en-GB")).ToString();
                    dateFrom = GlobalData.Instance.CheckDateFormat(dateFrom);
                    dateTo = GlobalData.Instance.CheckDateFormat(dateTo);
                    HTEmpList.Add("@Status", 9);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                    HTEmpList.Add("@dateFrom", dateFrom);
                    HTEmpList.Add("@dateTo", dateTo);

                }
                #region  Begin Code developed by prasad [01-10-2013]

                if (status == 10)
                {
                    HTEmpList.Add("@Status", 10);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }

                if (status == 11)
                {
                    HTEmpList.Add("@Status", 11);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }
                if (status == 12)
                {
                    HTEmpList.Add("@Status", 12);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);


                }
                if (status == 13)
                {
                    HTEmpList.Add("@Status", 13);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }
                if (status == 14)
                {
                    HTEmpList.Add("@Status", 14);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }

                if (status == 15)
                {
                    HTEmpList.Add("@Status", 15);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }

                if (status == 16)
                {
                    HTEmpList.Add("@Status", 16);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }

                if (status == 17)
                {
                    HTEmpList.Add("@Status", 17);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }



                if (status == 18)
                {
                    HTEmpList.Add("@Status", 18);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                    HTEmpList.Add("@from", txtfrom.Text);
                    HTEmpList.Add("@to", txtto.Text);

                }

                if (status == 19)
                {
                    HTEmpList.Add("@Status", 19);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                    HTEmpList.Add("@from", txtfrom.Text);
                    HTEmpList.Add("@to", txtto.Text);

                }
                if (status == 20)
                {
                    HTEmpList.Add("@Status", 20);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                    HTEmpList.Add("@from", txtfrom.Text);
                    HTEmpList.Add("@to", txtto.Text);

                }
                if (status == 21)
                {
                    HTEmpList.Add("@Status", 21);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                    HTEmpList.Add("@from", txtFromEmpid.Text);
                    HTEmpList.Add("@to", txttoEmpid.Text);

                }



                #endregion   End  Code developed by prasad [01-10-2013]

                DataTable DtGetEmpList = config.ExecuteAdaptorAsyncWithParams(spname, HTEmpList).Result;


                if (DtGetEmpList.Rows.Count > 0)
                {


                    if (ddlActiveEmp.SelectedIndex == 1)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                    }

                    if (ddlActiveEmp.SelectedIndex == 2)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;

                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;

                    }

                    if (ddlActiveEmp.SelectedIndex == 3)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;


                    }

                    if (ddlActiveEmp.SelectedIndex == 4)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;


                    }

                    if (ddlActiveEmp.SelectedIndex == 5)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;


                    }

                    if (ddlActiveEmp.SelectedIndex == 6)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;

                    }


                    if (ddlActiveEmp.SelectedIndex == 7)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;

                    }

                    if (ddlActiveEmp.SelectedIndex == 8)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;

                    }

                    if (ddlActiveEmp.SelectedIndex == 9)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;

                    }

                    if (ddlActiveEmp.SelectedIndex == 10)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;

                    }

                    if (ddlActiveEmp.SelectedIndex == 11)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;

                    }

                    if (ddlActiveEmp.SelectedIndex == 12)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;

                    }

                    if (ddlActiveEmp.SelectedIndex == 13)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;

                    }

                    if (ddlActiveEmp.SelectedIndex == 14)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;

                    }

                    if (ddlActiveEmp.SelectedIndex == 15)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;

                    }

                    if (ddlActiveEmp.SelectedIndex == 16)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;

                    }

                    if (ddlActiveEmp.SelectedIndex == 17)
                    {
                        panelfromto.Visible = false;
                        gvlistofemp.Visible = true;
                        gvlistofemp.DataSource = DtGetEmpList;
                        gvlistofemp.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = true;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;

                    }


                    if (ddlActiveEmp.SelectedIndex == 18)
                    {
                        panelfromto.Visible = true;

                        GVEmpDetails.DataSource = DtGetEmpList;
                        GVEmpDetails.DataBind();
                        gvlistofemp.Visible = false;
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;

                        lbtn_Export1.Visible = true;
                        lbtn_Export.Visible = false;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;

                    }
                    else if (ddlActiveEmp.SelectedIndex == 19)
                    {
                        gvlistofadhar.Visible = true;

                        gvlistofadhar.DataSource = DtGetEmpList;
                        gvlistofadhar.DataBind();
                        gvlistofemp.Visible = false;
                        GVEmpDetails.Visible = false;
                        gvnomineelist.Visible = false;

                        LinkButton2.Visible = true;
                        lbtn_Export.Visible = false;
                        lbtn_Export1.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;
                        GVListEmployees.Visible = false;



                    }
                    else if (ddlActiveEmp.SelectedIndex == 20)
                    {
                        gvnomineelist.Visible = true;

                        gvnomineelist.DataSource = DtGetEmpList;
                        gvnomineelist.DataBind();
                        gvlistofemp.Visible = false;
                        gvlistofadhar.Visible = false;
                        GVEmpDetails.Visible = false;
                        GVListEmployees.Visible = false;


                        LinkButton3.Visible = true;
                        lbtn_Export.Visible = false;
                        lbtn_Export1.Visible = false;
                        LinkButton2.Visible = false;
                        panelEmpIdFromTo.Visible = false;
                        LinkButton4.Visible = false;


                    }
                    if (ddlActiveEmp.SelectedIndex == 21)
                    {
                        panelfromto.Visible = false;
                        GVListEmployees.Visible = true;
                        GVListEmployees.DataSource = DtGetEmpList;
                        GVListEmployees.DataBind();
                        gvlistofadhar.Visible = false;
                        gvnomineelist.Visible = false;
                        gvlistofemp.Visible = false;
                        lbtn_Export1.Visible = false;
                        lbtn_Export.Visible = false;
                        LinkButton2.Visible = false;
                        LinkButton3.Visible = false;
                        panelEmpIdFromTo.Visible = true;
                        LinkButton4.Visible = true;

                    }
                    //else 
                    //{
                    //    lbtn_Export.Visible = true;
                    //    gvlistofemp.Visible = true;
                    //    gvlistofemp.DataSource = DtGetEmpList;
                    //    gvlistofemp.DataBind();
                    //    GVEmpDetails.Visible = false;
                    //    gvlistofadhar.Visible = false;
                    //    gvlistofemp.Visible = false;

                    //    lbtn_Export1.Visible = false;
                    //    LinkButton2.Visible = false;
                    //    LinkButton3.Visible = false;
                    //}
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('The details Are Not Available What you are searching...');", true);
                }


            }
            catch (Exception ex)
            {

            }

        }

        private void BindData(string SqlQuery)
        {

            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQuery).Result;
            if (dt.Rows.Count > 0)
            {
                gvlistofemp.DataSource = dt;
                gvlistofemp.DataBind();

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('The details Are Not Available What you are searching...');", true);
            }

        }



        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("EmployeeList.xls", this.gvlistofemp);

        }

        protected void lbtn_Export1_Click(object sender, EventArgs e)
        {
            GVUtil.Export("EmployeeList.xls", this.GVEmpDetails);

        }

        protected void lbtn_Export2_Click(object sender, EventArgs e)
        {
            GVUtil.Export("EmployeeList.xls", this.gvlistofadhar);

        }

        protected void lbtn_Export3_Click(object sender, EventArgs e)
        {
            GVUtil.Export("EmployeeList.xls", this.gvnomineelist);

        }
        protected void lbtn_Export4_Click(object sender, EventArgs e)
        {
            GVUtil.Export("PST Report.xls", this.GVListEmployees);

        }
        protected void gvlistofemp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes.Add("class", "text");
            }
        }

        protected void GVEmpDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes.Add("class", "text");
            }
        }

        protected void GVListEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes.Add("class", "text");
                e.Row.Cells[59].Attributes.Add("class", "text");
                e.Row.Cells[58].Attributes.Add("class", "text");
                e.Row.Cells[7].Attributes.Add("class", "text");
            }
        }

        protected void gvnomineelist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes.Add("class", "text");
            }
        }

        protected void gvlistofadharr_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes.Add("class", "text");
            }
        }

        protected void lbtn_ExportNew_Click(object sender, EventArgs e)
        {
            try
            {
                //Cleardata();

                string SqlQueryEmp = "";
                int status = ddlActiveEmp.SelectedIndex;
                Hashtable HTEmpList = new Hashtable();
                string spname = "IMReportForListOfEmployeesNew";

                if (status == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('please  select search mode');", true);
                    return;
                }

                if (status == 1)
                {
                    HTEmpList.Add("@Status", 1);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }
                if (status == 2)
                {
                    HTEmpList.Add("@Status", 2);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }
                if (status == 3)
                {
                    HTEmpList.Add("@Status", 3);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }

                if (status == 4)
                {
                    HTEmpList.Add("@Status", 4);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }

                if (status == 5)
                {

                    if (TextEmpid.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The employee Id');", true);
                        return;
                    }
                    HTEmpList.Add("@Empid", TextEmpid.Text.Trim());
                    HTEmpList.Add("@Status", 5);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }
                if (status == 6)
                {

                    if (TxtEmpname.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The employee Name');", true);
                        return;
                    }
                    HTEmpList.Add("@EmpName", TxtEmpname.Text.Trim());
                    HTEmpList.Add("@Status", 6);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }
                if (status == 7)
                {
                    if (ddldesgn.SelectedIndex == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Select The Designation');", true);
                        return;
                    }
                    HTEmpList.Add("@Status", 7);
                    HTEmpList.Add("@Designation", ddldesgn.SelectedValue);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }
                if (status == 8)
                {
                    if (TxtJdateFrom.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The From Date');", true);
                        return;
                    }


                    if (TxtJdateTo.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The To Date');", true);
                        return;
                    }
                    HTEmpList.Add("@Status", 8);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                    HTEmpList.Add("@JdateFrom", TxtJdateFrom.Text.Trim());
                    HTEmpList.Add("@JdateTo", TxtJdateTo.Text.Trim());

                }

                if (status == 9)
                {

                    if (TxtLdateFrom.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The From Date');", true);
                        return;
                    }

                    if (TxtLdateTo.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The To Date');", true);
                        return;
                    }
                    string dateFrom = DateTime.Parse(TxtLdateFrom.Text.Trim(), CultureInfo.GetCultureInfo("en-GB")).ToString();
                    string dateTo = DateTime.Parse(TxtLdateTo.Text.Trim(), CultureInfo.GetCultureInfo("en-GB")).ToString();
                    dateFrom = GlobalData.Instance.CheckDateFormat(dateFrom);
                    dateTo = GlobalData.Instance.CheckDateFormat(dateTo);
                    HTEmpList.Add("@Status", 9);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                    HTEmpList.Add("@dateFrom", dateFrom);
                    HTEmpList.Add("@dateTo", dateTo);

                }
                #region  Begin Code developed by prasad [01-10-2013]

                if (status == 10)
                {
                    HTEmpList.Add("@Status", 10);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }

                if (status == 11)
                {
                    HTEmpList.Add("@Status", 11);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }
                if (status == 12)
                {
                    HTEmpList.Add("@Status", 12);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);


                }
                if (status == 13)
                {
                    HTEmpList.Add("@Status", 13);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }
                if (status == 14)
                {
                    HTEmpList.Add("@Status", 14);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }

                if (status == 15)
                {
                    HTEmpList.Add("@Status", 15);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }

                if (status == 16)
                {
                    HTEmpList.Add("@Status", 16);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }

                if (status == 17)
                {
                    HTEmpList.Add("@Status", 17);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                }



                if (status == 18)
                {
                    HTEmpList.Add("@Status", 18);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                    HTEmpList.Add("@from", txtfrom.Text);
                    HTEmpList.Add("@to", txtto.Text);

                }

                if (status == 19)
                {
                    HTEmpList.Add("@Status", 19);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                    HTEmpList.Add("@from", txtfrom.Text);
                    HTEmpList.Add("@to", txtto.Text);

                }
                if (status == 20)
                {
                    HTEmpList.Add("@Status", 20);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                    HTEmpList.Add("@from", txtfrom.Text);
                    HTEmpList.Add("@to", txtto.Text);

                }
                if (status == 21)
                {
                    HTEmpList.Add("@Status", 21);
                    HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                    HTEmpList.Add("@from", txtFromEmpid.Text);
                    HTEmpList.Add("@to", txttoEmpid.Text);

                }
                #endregion   End  Code developed by prasad [01-10-2013]

                DataTable DtGetEmpList = config.ExecuteAdaptorAsyncWithParams(spname, HTEmpList).Result;


                if (DtGetEmpList.Rows.Count > 0)
                {


                    string filename = "Listofemployess.xls";

                    var products = DtGetEmpList;
                    ExcelPackage excel = new ExcelPackage();
                    var workSheet = excel.Workbook.Worksheets.Add("Listofemployess");
                    var totalCols = products.Columns.Count;
                    var totalRows = products.Rows.Count;

                    for (var col = 1; col <= totalCols; col++)
                    {
                        workSheet.Cells[1, col].Value = products.Columns[col - 1].ColumnName;
                        workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    }
                    for (var row = 1; row <= totalRows; row++)
                    {
                        for (var col = 0; col < totalCols; col++)
                        {
                            workSheet.Cells[row + 1, col + 1].Value = products.Rows[row - 1][col];
                        }

                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment ;filename=\"" + filename + "\"");
                        excel.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('The details Are Not Available What you are searching...');", true);
                }


            }
            catch (Exception ex)
            {

            }
        }


    }
}