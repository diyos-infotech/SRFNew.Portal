using System;
using System.Collections;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using KLTS.Data;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using SRF.P.DAL;

namespace SRF.P.Module_Employees
{
    public partial class AndroidEmployee : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();


        DataTable dt = null;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        // New Page created done by Mahesh Goud on 2022-07-08 ticketno=009606
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {



                EmpIDPrefix = Session["EmpIDPrefix"].ToString();
                CmpIDPrefix = Session["CmpIDPrefix"].ToString();

                if (!IsPostBack)
                {
                    TabContainer1.ActiveTabIndex = 0;

                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }

                    //LoadClientids();

                    LoadDesignations();
                    employeeid();
                    LoadStatenames();
                    LoadOldEmployeeId();

                    LoadBanknames();
                    Session["CheckStatusRecordInserted"] = 0;
                    Session["ExactEmpid"] = 0;
                    SetInitialRow();
                    SetInitialRowEducation();
                    SetInitialRowPrevExp();
                    SetInitialRowEducation();
                    AndroidEmpid();
                    LoadBloodGroups();
                    LoadClientids();
                    LoadDivisions();
                    LoadDepartments();
                    LoadBranches();
                    LoadReportingManager();





                }
            }

            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
                Response.Redirect("~/login.aspx");
            }
        }


        #region New Code for Load Blood Groups dynamically from database as on 23/12/2013 by venkat

        protected void LoadBloodGroups()
        {
            DataTable dtBloodgroup = GlobalData.Instance.LoadBloodGroupNames();
            if (dtBloodgroup.Rows.Count > 0)
            {
                ddlBloodGroup.DataValueField = "BloodGroupId";
                ddlBloodGroup.DataTextField = "BloodGroupName";
                ddlBloodGroup.DataSource = dtBloodgroup;
                ddlBloodGroup.DataBind();
            }
            ddlBloodGroup.Items.Insert(0, "-Select-");
        }

        #endregion

        private void SetInitialRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("RName", typeof(string)));
            dt.Columns.Add(new DataColumn("DOfBirth", typeof(string)));
            dt.Columns.Add(new DataColumn("age", typeof(string)));
            dt.Columns.Add(new DataColumn("RType", typeof(string)));
            dt.Columns.Add(new DataColumn("ROccupation", typeof(string)));
            dt.Columns.Add(new DataColumn("RAAdharNo", typeof(string)));
            dt.Columns.Add(new DataColumn("PFNominee", typeof(string)));
            dt.Columns.Add(new DataColumn("ESINominee", typeof(string)));
            dt.Columns.Add(new DataColumn("RResidence", typeof(string)));
            dt.Columns.Add(new DataColumn("RPlace", typeof(string)));

            for (int i = 1; i < 12; i++)
            {

                dr = dt.NewRow();
                dr["RowNumber"] = 1;
                dr["RName"] = string.Empty;
                dr["DOfBirth"] = string.Empty;
                dr["age"] = string.Empty;
                dr["RType"] = string.Empty;
                dr["ROccupation"] = string.Empty;
                dr["RAAdharNo"] = string.Empty;
                dr["PFNominee"] = string.Empty;
                dr["ESINominee"] = string.Empty;
                dr["RResidence"] = string.Empty;
                dr["RPlace"] = string.Empty;

                dt.Rows.Add(dr);

            }

            //Store the DataTable in ViewState
            ViewState["CurrentTable"] = dt;

            gvFamilyDetails.DataSource = dt;
            gvFamilyDetails.DataBind();
        }

        private void SetInitialRowEducation()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("Qualification", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("NameOfSchoolClg", typeof(string)));
            dt.Columns.Add(new DataColumn("BoardorUniversity", typeof(string)));
            dt.Columns.Add(new DataColumn("YrOfStudy", typeof(string)));
            dt.Columns.Add(new DataColumn("PassOrFail", typeof(string)));
            dt.Columns.Add(new DataColumn("PercentageOfmarks", typeof(string)));

            for (int i = 1; i < 5; i++)
            {
                dr = dt.NewRow();
                dr["RowNumber"] = 1;
                dr["Qualification"] = string.Empty;
                dr["Description"] = string.Empty;
                dr["NameOfSchoolClg"] = string.Empty;
                dr["BoardorUniversity"] = string.Empty;
                dr["YrOfStudy"] = string.Empty;
                dr["PassOrFail"] = string.Empty;
                dr["PercentageOfmarks"] = string.Empty;
                dt.Rows.Add(dr);

            }


            //Store the DataTable in ViewState
            ViewState["EducationTable"] = dt;

            GvEducationDetails.DataSource = dt;
            GvEducationDetails.DataBind();
        }

        private void SetInitialRowPrevExp()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("RegionCode", typeof(string)));
            dt.Columns.Add(new DataColumn("EmployerCode", typeof(string)));
            dt.Columns.Add(new DataColumn("Extension", typeof(string)));
            dt.Columns.Add(new DataColumn("Designation", typeof(string)));
            dt.Columns.Add(new DataColumn("CompAddress", typeof(string)));
            dt.Columns.Add(new DataColumn("YrOfExp", typeof(string)));
            dt.Columns.Add(new DataColumn("PFNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ESINo", typeof(string)));
            dt.Columns.Add(new DataColumn("DateofResign", typeof(string)));

            for (int i = 1; i < 5; i++)
            {
                dr = dt.NewRow();
                dr["RowNumber"] = 1;
                dr["RegionCode"] = string.Empty;
                dr["EmployerCode"] = string.Empty;
                dr["Extension"] = string.Empty;
                dr["Designation"] = string.Empty;
                dr["CompAddress"] = string.Empty;
                dr["YrOfExp"] = string.Empty;
                dr["PFNo"] = string.Empty;
                dr["ESINo"] = string.Empty;
                dr["DateofResign"] = string.Empty;
                dt.Rows.Add(dr);

            }


            //Store the DataTable in ViewState
            ViewState["PrevExpTable"] = dt;

            GvPreviousExperience.DataSource = dt;
            GvPreviousExperience.DataBind();
        }

        protected void LoadBanknames()
        {

            DataTable DtBankNames = GlobalData.Instance.LoadBankNames();
            if (DtBankNames.Rows.Count > 0)
            {
                ddlbankname.DataValueField = "bankid";
                ddlbankname.DataTextField = "bankname";
                ddlbankname.DataSource = DtBankNames;
                ddlbankname.DataBind();
            }
            ddlbankname.Items.Insert(0, new ListItem("-Select-", "0"));
        }

        protected void LoadDivisions()
        {

            DataTable DtDivision = GlobalData.Instance.LoadDivision();
            if (DtDivision.Rows.Count > 0)
            {
                ddlDivision.DataValueField = "DivisionId";
                ddlDivision.DataTextField = "DivisionName";
                ddlDivision.DataSource = DtDivision;
                ddlDivision.DataBind();
                ddlDivision.SelectedValue = "1";

            }
            ddlDivision.Items.Insert(0, new ListItem("-Select-", "0"));
        }

        protected void LoadBranches()
        {

            DataTable DtBranches = GlobalData.Instance.LoadAllBranch();
            if (DtBranches.Rows.Count > 0)
            {
                ddlBranch.DataValueField = "BranchId";
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataSource = DtBranches;
                ddlBranch.DataBind();
                ddlBranch.SelectedValue = "1";

            }
            ddlBranch.Items.Insert(0, new ListItem("-Select-", "0"));
        }

        protected void LoadDepartments()
        {

            DataTable DtDepartments = GlobalData.Instance.LoadDepartments();
            if (DtDepartments.Rows.Count > 0)
            {
                ddldepartment.DataValueField = "DeptId";
                ddldepartment.DataTextField = "DeptName";
                ddldepartment.DataSource = DtDepartments;
                ddldepartment.DataBind();
                ddldepartment.SelectedValue = "3";

            }
            ddldepartment.Items.Insert(0, new ListItem("-Select-", "0"));
        }

        protected void LoadStatenames()
        {

            DataTable DtStateNames = GlobalData.Instance.LoadStateNames();
            if (DtStateNames.Rows.Count > 0)
            {
                ddlpreStates.DataValueField = "StateID";
                ddlpreStates.DataTextField = "State";
                ddlpreStates.DataSource = DtStateNames;
                ddlpreStates.DataBind();


                DdlStates.DataValueField = "StateID";
                DdlStates.DataTextField = "State";
                DdlStates.DataSource = DtStateNames;
                DdlStates.DataBind();

                ddlpvcstate.DataValueField = "StateID";
                ddlpvcstate.DataTextField = "State";
                ddlpvcstate.DataSource = DtStateNames;
                ddlpvcstate.DataBind();

                ////ddlbirthstate.DataValueField = "StateID";
                ////ddlbirthstate.DataTextField = "State";
                ////ddlbirthstate.DataSource = DtStateNames;
                ////ddlbirthstate.DataBind();
            }
            ddlpreStates.Items.Insert(0, new ListItem("-Select-", "0"));
            DdlStates.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlpvcstate.Items.Insert(0, new ListItem("-Select-", "0"));
            //ddlbirthstate.Items.Insert(0, new ListItem("-Select-", "0"));
        }

        protected void LoadDesignations()
        {

            DataTable DtDesignations = GlobalData.Instance.LoadDesigns();
            if (DtDesignations.Rows.Count > 0)
            {
                ddlDesignation.DataValueField = "Designid";
                ddlDesignation.DataTextField = "Design";
                ddlDesignation.DataSource = DtDesignations;
                ddlDesignation.DataBind();
            }
            ddlDesignation.Items.Insert(0, new ListItem("-Select-", "0"));
        }

        public void LoadOldEmployeeId()
        {

            string query = "select empid from EmpDetails where empstatus=0 and rejoinstatus=0";
            DataTable dtOldEmployeeIds = config.ExecuteReaderWithQueryAsync(query).Result;
            if (dtOldEmployeeIds.Rows.Count > 0)
            {
                ddloldempdrp.DataValueField = "Empid";
                ddloldempdrp.DataTextField = "Empid";
                ddloldempdrp.DataSource = dtOldEmployeeIds;
                ddloldempdrp.DataBind();
            }

            ddloldempdrp.Items.Insert(0, new ListItem("-Select-", "0"));

        }

        protected void LoadClientids()
        {
            #region New Code for Prefered Units as on 24/12/2013 by venkat

            DataTable dt = GlobalData.Instance.LoadCNames(CmpIDPrefix);
            if (dt.Rows.Count > 0)
            {
                DdlPreferedUnit.DataValueField = "clientid";
                DdlPreferedUnit.DataTextField = "clientname";
                DdlPreferedUnit.DataSource = dt;
                DdlPreferedUnit.DataBind();
            }
            DdlPreferedUnit.Items.Insert(0, new ListItem("-Select-", "0"));

            #endregion

        }

        protected void LoadReportingManager()
        {
            #region New Code for Prefered Units as on 24/12/2013 by venkat

            string Query = "Select Empid,(EmpFname+' '+EmpMname+' '+EmpLname) as Empname from Empdetails where EmployeeType='S' order by (EmpFname+' '+EmpMname+' '+EmpLname)";
            DataTable dt = config.ExecuteReaderWithQueryAsync(Query).Result;
            if (dt.Rows.Count > 0)
            {
                ddlReportingMgr.DataValueField = "Empid";
                ddlReportingMgr.DataTextField = "Empname";
                ddlReportingMgr.DataSource = dt;
                ddlReportingMgr.DataBind();
            }
            ddlReportingMgr.Items.Insert(0, new ListItem("-Select-", "0"));

            #endregion

        }

        DataTable dtempid;
        int empid;
        string DtEmpId;

        private void employeeid()
        {
            if (rdbGeneral.Checked == true)
            {
                string EmployeeType = "G";
                txtEmID.Text = GlobalData.Instance.LoadMaxEmpid(EmpIDPrefix, EmployeeType);
                txtmodifyempid.Text = " <i> Emp ID: <b>" + txtEmID.Text + "</b></i>";

            }
            else if (rdbStaff.Checked == true)
            {
                string EmployeeType = "S";
                txtEmID.Text = GlobalData.Instance.LoadMaxEmpid(EmpIDPrefix, EmployeeType);
                txtmodifyempid.Text = " <i> Emp ID: <b>" + txtEmID.Text + "</b></i>";

            }
            else
            {
                string EmployeeType = "G";
                txtEmID.Text = GlobalData.Instance.LoadMaxEmpid(EmpIDPrefix, EmployeeType);

            }
        }

        protected static void ClearControls(Control Parent)
        {
            if (Parent is TextBox)
            {
                (Parent as TextBox).Text = string.Empty;
            }

            else
            {
                foreach (Control c in Parent.Controls)
                    ClearControls(c);
            }

        }

        public void CheckEmpid()
        {
            string query = "select empid,(Empfname+' '+empmname+' '+emplname) as name from empdetails where empid='" + txtEmID.Text + "' ";
            DataTable dt = config.ExecuteReaderWithQueryAsync(query).Result;
            string name = "";

            if (dt.Rows.Count > 0)
            {
                name = dt.Rows[0]["name"].ToString();
                lblMsg.Visible = true;
                lblMsg.Text = "Employee ID already exists for Employee " + name;
                return;

            }

        }

        private void AndroidEmpid()
        {
            string query = "select empid from Empdetails_Android where isnull(EmpAndroidStatus,0)=0 and isnull(EmpRejectStatus,0) =0 order by empid Desc";
            DataTable dtemp = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dtemp.Rows.Count > 0)
            {
                ddlAndroidEmpid.DataValueField = "empid";
                ddlAndroidEmpid.DataValueField = "empid";
                ddlAndroidEmpid.DataTextField = "empid";
                ddlAndroidEmpid.DataSource = dtemp;
                ddlAndroidEmpid.DataBind();
            }

            ddlAndroidEmpid.Items.Insert(0, "--Select--");
        }

        protected void LoadPersonalInfo(string empid)
        {
            try
            {
                string EmpIDPrefix = GlobalData.Instance.GetEmployeeIDPrefix();
                #region Begin SP Parameters done by Mahesh Goud on 2022-07-08 ticketno=009606
                Hashtable HashtableIOM = new Hashtable();
                string SPNAME = "LoadPersonalInfoforAndroid";
                var EMPID = empid;
                HashtableIOM.Add("@EMPID", EMPID);
                DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPNAME, HashtableIOM).Result;
                #endregion SP Parameters end


                rdbactive.Checked = true;



                string Employeetype = dt.Rows[0]["Employeetype"].ToString();
                if (Employeetype == "G")
                {
                    rdbGeneral.Checked = true;
                }
                else
                {
                    rdbStaff.Checked = true;
                }
                ddlTitle.SelectedValue = dt.Rows[0]["Title"].ToString();
                txtEmpFName.Text = dt.Rows[0]["EmpFName"].ToString();
                txtEmpmiName.Text = dt.Rows[0]["EmpMName"].ToString();
                txtEmplname.Text = dt.Rows[0]["EmpLName"].ToString();
                string MaritalStatus = dt.Rows[0]["EmpMaritalStatus"].ToString();
                if (MaritalStatus == "M")
                {
                    rdbmarried.Checked = true;
                }
                else if (MaritalStatus == "S")
                {
                    rdbsingle.Checked = true;
                }
                else if (MaritalStatus == "W")
                {
                    rdbWidower.Checked = true;
                }
                else
                {
                    rdbdivorcee.Checked = true;
                }

                string EmpaddressStatus = dt.Rows[0]["EmpaddresStatus"].ToString();
                if (EmpaddressStatus == "NR")
                {
                    rdbnotrequired.Checked = true;
                }
                if (EmpaddressStatus == "PR")
                {
                    rdbpreaddress.Checked = true;
                }
                if (EmpaddressStatus == "PE")
                {
                    rdbperaddress.Checked = true;
                }

                txtQualification.Text = dt.Rows[0]["EmpQualification"].ToString();
                txtPhone.Text = dt.Rows[0]["EmpPhone"].ToString();
                txoldempid.Text = dt.Rows[0]["Oldempid"].ToString();
                txtmtongue.Text = dt.Rows[0]["MotherTongue"].ToString();
                txtnationality.Text = dt.Rows[0]["Nationality"].ToString();
                txtreligion.Text = dt.Rows[0]["Religion"].ToString();
                txtLangKnown.Text = dt.Rows[0]["EmpLanguagesKnown"].ToString();
                txtemail.Text = dt.Rows[0]["Emailid"].ToString();
                txtpsaraempcode.Text = dt.Rows[0]["psaraempcode"].ToString();
                txtSpousName.Text = dt.Rows[0]["EmpSpouseName"].ToString();
                txtMotherName.Text = dt.Rows[0]["EmpMotherName"].ToString();
                txtFatherName.Text = dt.Rows[0]["EmpFatherName"].ToString();
                txtfatheroccupation.Text = dt.Rows[0]["EmpFatherOccupation"].ToString();


                if (dt.Rows[0]["Division"].ToString() == "0")
                {
                    ddlDivision.SelectedIndex = 0;
                }
                else
                {
                    ddlDivision.SelectedValue = dt.Rows[0]["Division"].ToString();
                }

                if (dt.Rows[0]["Department"].ToString() == "0")
                {
                    ddldepartment.SelectedIndex = 0;
                }
                else
                {
                    ddldepartment.SelectedValue = dt.Rows[0]["Department"].ToString();
                }

                if (dt.Rows[0]["ReportingManager"].ToString() == "0")
                {
                    ddlReportingMgr.SelectedIndex = 0;
                }
                else
                {
                    ddlReportingMgr.SelectedValue = dt.Rows[0]["ReportingManager"].ToString();
                }

                if (dt.Rows[0]["Branch"].ToString() == "0")
                {
                    ddlBranch.SelectedIndex = 0;
                }
                else
                {
                    ddlBranch.SelectedValue = dt.Rows[0]["Branch"].ToString();
                }

                txtGrossSalary.Text = dt.Rows[0]["gross"].ToString();

                if (dt.Rows[0]["EmpDesgn"].ToString() == "0")
                {
                    ddlDesignation.SelectedIndex = 0;
                }
                else
                {
                    ddlDesignation.SelectedValue = dt.Rows[0]["EmpDesgn"].ToString();
                }

                txtQualification.Text = dt.Rows[0]["EmpQualification"].ToString();
                txtPreEmp.Text = dt.Rows[0]["EmpPreviousExp"].ToString();

                string Empsex = dt.Rows[0]["EmpSex"].ToString();
                if (Empsex == "M")
                {
                    rdbmale.Checked = true;
                }
                else if (Empsex == "F")
                {
                    rdbfemale.Checked = true;
                }
                else
                {
                    rdbTransgender.Checked = true;
                }

                bool ex = false;
                if (String.IsNullOrEmpty(dt.Rows[0]["EmpExservice"].ToString()) == false)
                {
                    ex = Convert.ToBoolean(dt.Rows[0]["EmpExservice"].ToString());
                }
                if (ex == true)
                    ChkExService.Checked = true;
                else
                    ChkExService.Checked = false;

                bool pf = false;
                if (String.IsNullOrEmpty(dt.Rows[0]["EmpPFDeduct"].ToString()) == false)
                {
                    pf = Convert.ToBoolean(dt.Rows[0]["EmpPFDeduct"].ToString());
                }
                if (pf == true)
                    ChkPFDed.Checked = true;
                else
                    ChkPFDed.Checked = false;

                bool ESI = false;
                if (String.IsNullOrEmpty(dt.Rows[0]["EmpESIDeduct"].ToString()) == false)
                {
                    ESI = Convert.ToBoolean(dt.Rows[0]["EmpESIDeduct"].ToString());
                }
                if (ESI == true)
                    ChkESIDed.Checked = true;
                else
                    ChkESIDed.Checked = false;


                bool PT = false;
                if (String.IsNullOrEmpty(dt.Rows[0]["EmpPTDeduct"].ToString()) == false)
                {
                    PT = Convert.ToBoolean(dt.Rows[0]["EmpPTDeduct"].ToString());
                }
                if (PT == true)
                    ChkPTDed.Checked = true;
                else
                    ChkPTDed.Checked = false;


                DdlPreferedUnit.SelectedValue = dt.Rows[0]["Unitid"].ToString();
                txtLangKnown.Text = dt.Rows[0]["EmpLanguagesKnown"].ToString();

                txtDofleaving.Text = "";




                if (String.IsNullOrEmpty(dt.Rows[0]["EmpDtofInterview"].ToString()) == false)
                {

                    txtEmpDtofInterview.Text = DateTime.Parse(dt.Rows[0]["EmpDtofInterview"].ToString()).ToString("dd/MM/yyyy");
                    if (txtEmpDtofInterview.Text == "01/01/1900")
                    {
                        txtEmpDtofInterview.Text = "";
                    }

                }
                else
                {
                    txtEmpDtofInterview.Text = "";

                }

                if (String.IsNullOrEmpty(dt.Rows[0]["EmpDtofJoining"].ToString()) == false)
                {
                    txtEmpDtofJoining.Text = DateTime.Parse(dt.Rows[0]["EmpDtofJoining"].ToString()).ToString("dd/MM/yyyy");
                    if (txtEmpDtofJoining.Text == "01/01/1900")
                    {
                        txtEmpDtofJoining.Text = "";
                    }
                }
                else
                {
                    //txtEmpDtofJoining.Text = "01/01/1900";
                    txtEmpDtofJoining.Text = "";
                }
                if (String.IsNullOrEmpty(dt.Rows[0]["EmpDtofBirth"].ToString()) == false)
                {

                    txtEmpDtofBirth.Text = DateTime.Parse(dt.Rows[0]["EmpDtofBirth"].ToString()).ToString("dd/MM/yyyy");
                    if (txtEmpDtofBirth.Text == "01/01/1900")
                    {
                        txtEmpDtofBirth.Text = "";
                    }

                }
                else
                {
                    txtEmpDtofBirth.Text = "";
                }

                if (String.IsNullOrEmpty(dt.Rows[0]["IDCardIssued"].ToString()) == false)
                {

                    TxtIDCardIssuedDt.Text = DateTime.Parse(dt.Rows[0]["IDCardIssued"].ToString()).ToString("dd/MM/yyyy");
                    if (TxtIDCardIssuedDt.Text == "01/01/1900")
                    {
                        TxtIDCardIssuedDt.Text = "";
                    }

                }
                else
                {
                    TxtIDCardIssuedDt.Text = "";

                }

                if (String.IsNullOrEmpty(dt.Rows[0]["IDCardValid"].ToString()) == false)
                {

                    TxtIdCardValid.Text = DateTime.Parse(dt.Rows[0]["IDCardValid"].ToString()).ToString("dd/MM/yyyy");
                    if (TxtIdCardValid.Text == "01/01/1900")
                    {
                        TxtIdCardValid.Text = "";
                    }

                }
                else
                {
                    TxtIdCardValid.Text = "";

                }


                if (dt.Rows[0]["Image"].ToString().Length > 0)
                {
                    using (var client = new System.Net.WebClient())
                    {
                        string Str = empid + "Photo.jpg";
                        string remoteUri = "http://gdxmobileapp.mydiyosfame.com/";

                        string fileName = "";

                        if (dt.Rows[0]["Image"].ToString().Length > 0)
                        {
                            fileName = dt.Rows[0]["Image"].ToString();
                        }

                        client.DownloadFile(remoteUri + fileName, Server.MapPath("~/assets/EmpPhotos/") + Str);

                        if (File.Exists(Server.MapPath("~/assets/EmpPhotos/") + Str))
                        {
                            Imageempphoto.ImageUrl = ("~/assets/EmpPhotos/") + Str;
                        }
                        else
                        {
                            Imageempphoto.ImageUrl = null;
                        }

                    }
                }
                using (var client = new System.Net.WebClient())
                {
                    string Str = empid + "Sign.jpg";
                    string remoteUri = "http://gdxmobileapp.mydiyosfame.com/";

                    string fileName = "";

                    if (dt.Rows[0]["EmpSign"].ToString().Length > 0)
                    {
                        fileName = dt.Rows[0]["EmpSign"].ToString();

                        client.DownloadFile(remoteUri + fileName, Server.MapPath("~/assets/Empsign/") + Str);

                        if (File.Exists(Server.MapPath("~/assets/Empsign/") + Str))
                        {
                            ImageSign.ImageUrl = ("~/assets/Empsign/") + Str;
                        }
                        else
                        {
                            ImageSign.ImageUrl = null;
                        }
                    }
                    else
                    {
                        ImageSign.ImageUrl = null;
                    }

                }


                //txtBirthDistrict.Text = dt.Rows[0]["BirthDistrict"].ToString();
                //txtBirthCountry.Text = dt.Rows[0]["BirthCountry"].ToString();
                //txtBirthVillage.Text = dt.Rows[0]["BirthVillage"].ToString();


                //if (dt.Rows[0]["BirthState"].ToString() == "0")
                //{
                //    ddlbirthstate.SelectedIndex = 0;
                //}
                //else
                //{
                //    ddlbirthstate.SelectedValue = dt.Rows[0]["BirthState"].ToString();
                //}

                //string Birthcity = "select CityID,City from cities where state='" + dt.Rows[0]["BirthState"].ToString() + "' order by City";
                //DataTable dtbirthcity = SqlHelper.Instance.GetTableByQuery(Birthcity);
                //if (dtbirthcity.Rows.Count > 0)
                //{
                //    ddlBirthDistrict.Enabled = true;
                //    ddlBirthDistrict.DataValueField = "CityID";
                //    ddlBirthDistrict.DataTextField = "City";
                //    ddlBirthDistrict.DataSource = dtbirthcity;
                //    ddlBirthDistrict.DataBind();
                //    ddlBirthDistrict.Items.Insert(0, new ListItem("--Select--", "0"));
                //}



                //if (dt.Rows[0]["BirthDistrict"].ToString() == "0" || dt.Rows[0]["BirthDistrict"].ToString() == "")
                //{
                //    ddlBirthDistrict.SelectedIndex = 0;

                //}
                //else
                //{
                //    ddlBirthDistrict.Enabled = true;
                //    ddlBirthDistrict.Items.FindByText(dt.Rows[0]["BirthDistrict"].ToString()).Selected = true;

                //}

                if (dt.Rows[0]["ApplicantCategory"].ToString() != null && dt.Rows[0]["ApplicantCategory"].ToString() != "")
                {

                    ddlAppCategory.SelectedValue = dt.Rows[0]["ApplicantCategory"].ToString();
                }

                bool a = false;
                if (String.IsNullOrEmpty(dt.Rows[0]["SpeciallyAbled"].ToString()) == false)
                {
                    a = Convert.ToBoolean(dt.Rows[0]["SpeciallyAbled"].ToString());
                }
                if (a == true)
                {
                    ChkSpeciallyAbled.Checked = true;

                }
                else
                {
                    ChkSpeciallyAbled.Checked = false;

                }
                txtEyeColour.Text = dt.Rows[0]["EmpEyesColor"].ToString();
                txthaircolour.Text = dt.Rows[0]["EmpHairColor"].ToString();

                if (dt.Rows[0]["EmpBloodGroup"].ToString() == "0" || dt.Rows[0]["EmpBloodGroup"].ToString() == "Choose The Blood Group")
                {
                    ddlBloodGroup.SelectedIndex = 0;
                }
                else
                {
                    ddlBloodGroup.SelectedValue = dt.Rows[0]["EmpBloodGroup"].ToString();
                }


                txtPhyRem.Text = dt.Rows[0]["EmpPhysicalRemarks"].ToString();

                txtImark1.Text = dt.Rows[0]["EmpIdMark1"].ToString();
                txtImark2.Text = dt.Rows[0]["EmpIdMark2"].ToString();
                // txtpermAddr.Text = dt.Rows[0]["EmpPermanentAddress"].ToString();
                //  txtPDist.Text = dt.Rows[0]["EmpPermanentDistrict"].ToString();
                //  txtprsntAddr.Text = dt.Rows[0]["EmpPresentAddress"].ToString();
                txtREfAddr1.Text = dt.Rows[0]["EmpRefAddr1"].ToString();
                txtREfAddr2.Text = dt.Rows[0]["EmpRefAddr2"].ToString();
                txtEmpRemarks.Text = dt.Rows[0]["EmpRemarks"].ToString();

                //txtPrdoor.Text = dt.Rows[0]["prDoorno"].ToString();

                //txtstreet.Text = dt.Rows[0]["prStreet"].ToString();
                //txtlmark.Text = dt.Rows[0]["prLmark"].ToString();
                //txtarea.Text = dt.Rows[0]["prArea"].ToString();

                //txtdistrictt.Text = dt.Rows[0]["prDistrict"].ToString();
                //txtpin.Text = dt.Rows[0]["prPincode"].ToString();
                //txtstate.Text = dt.Rows[0]["prState"].ToString();



                if (dt.Rows[0]["prState"].ToString() == "0")
                {
                    ddlpreStates.SelectedIndex = 0;
                }
                else
                {
                    ddlpreStates.SelectedValue = dt.Rows[0]["prState"].ToString();
                }

                string Cityquery = "select CityID,City from cities where State='" + dt.Rows[0]["prstate"].ToString() + "' order by City";
                DataTable CityDt = config.ExecuteAdaptorAsyncWithQueryParams(Cityquery).Result;
                if (CityDt.Rows.Count > 0)
                {
                    ddlpreCity.Enabled = true;
                    ddlpreCity.DataValueField = "CityID";
                    ddlpreCity.DataTextField = "City";
                    ddlpreCity.DataSource = CityDt;
                    ddlpreCity.DataBind();

                }

                ddlpreCity.Items.Insert(0, new ListItem("--Select--", "0"));


                if (dt.Rows[0]["prcity"].ToString() == "0" || dt.Rows[0]["prcity"].ToString() == "")
                {
                    ddlpreCity.SelectedIndex = 0;

                }
                else
                {
                    ddlpreCity.Enabled = true;
                    ddlpreCity.Items.FindByText(dt.Rows[0]["prcity"].ToString()).Selected = true;

                }



                txtmobile.Text = dt.Rows[0]["prphone"].ToString();
                txtheight.Text = dt.Rows[0]["EmpHeight"].ToString();
                txtweight.Text = dt.Rows[0]["EmpWeight"].ToString();
                txtcheunexpan.Text = dt.Rows[0]["EmpChestunex"].ToString();
                txtcheexpan.Text = dt.Rows[0]["EmpChestExp"].ToString();

                //txtdoor1.Text = dt.Rows[0]["pedoor"].ToString();
                //txtstreet2.Text = dt.Rows[0]["peStreet"].ToString();
                //txtlmark3.Text = dt.Rows[0]["pelmark"].ToString();
                // txtarea4.Text = dt.Rows[0]["peArea"].ToString();
                //txtcity5.Text = dt.Rows[0]["peCity"].ToString();
                //txtPDist.Text = dt.Rows[0]["peDistrict"].ToString();
                //txtpin7.Text = dt.Rows[0]["pePincode"].ToString();
                //txtstate8.Text = dt.Rows[0]["peState"].ToString();

                //txtPresentAddress.Text = dt.Rows[0]["EmpPresentAddress"].ToString();
                txtprPeriodofStay.Text = dt.Rows[0]["prperiodofstay"].ToString();
                txtPeriodofStay.Text = dt.Rows[0]["periodofstay"].ToString();
                txtprPoliceStation.Text = dt.Rows[0]["prPoliceStation"].ToString();
                txtprvillage.Text = dt.Rows[0]["prTown"].ToString();
                txtprtaluka.Text = dt.Rows[0]["prTaluka"].ToString();
                txtprPostOffice.Text = dt.Rows[0]["prPostoffice"].ToString();
                txtprLandmark.Text = dt.Rows[0]["prLmark"].ToString();
                txtprpin.Text = dt.Rows[0]["prPincode"].ToString();


                if (String.IsNullOrEmpty(dt.Rows[0]["ResidingDate"].ToString()) == false)
                {

                    txtResidingDate.Text = DateTime.Parse(dt.Rows[0]["ResidingDate"].ToString()).ToString("dd/MM/yyyy");
                    if (txtResidingDate.Text == "01/01/1900")
                    {
                        txtResidingDate.Text = "";
                    }

                }
                else
                {
                    txtResidingDate.Text = "";

                }


                if (String.IsNullOrEmpty(dt.Rows[0]["prResidingDate"].ToString()) == false)
                {

                    txtprResidingDate.Text = DateTime.Parse(dt.Rows[0]["prResidingDate"].ToString()).ToString("dd/MM/yyyy");
                    if (txtprResidingDate.Text == "01/01/1900")
                    {
                        txtprResidingDate.Text = "";
                    }

                }
                else
                {
                    txtprResidingDate.Text = "";

                }
                //txtPermanentAddress.Text = dt.Rows[0]["EmpPermanentAddress"].ToString();

                if (dt.Rows[0]["peState"].ToString() == "0")
                {
                    DdlStates.SelectedIndex = 0;
                }
                else
                {
                    DdlStates.SelectedValue = dt.Rows[0]["peState"].ToString();
                }

                string Pecity = "select CityID,City from cities where state='" + dt.Rows[0]["peState"].ToString() + "' order by City";
                DataTable dtpecity = config.ExecuteAdaptorAsyncWithQueryParams(Pecity).Result;
                if (dtpecity.Rows.Count > 0)
                {
                    ddlcity.Enabled = true;
                    ddlcity.DataValueField = "CityID";
                    ddlcity.DataTextField = "City";
                    ddlcity.DataSource = dtpecity;
                    ddlcity.DataBind();
                }

                ddlcity.Items.Insert(0, new ListItem("--Select--", "0"));


                if (dt.Rows[0]["peCity"].ToString() == "0" || dt.Rows[0]["peCity"].ToString() == "")
                {
                    ddlcity.SelectedIndex = 0;

                }
                else
                {
                    ddlcity.Enabled = true;
                    ddlcity.Items.FindByText(dt.Rows[0]["peCity"].ToString()).Selected = true;

                }


                txtmobile9.Text = dt.Rows[0]["pephone"].ToString();
                txtpePoliceStattion.Text = dt.Rows[0]["pePoliceStation"].ToString();
                txtpevillage.Text = dt.Rows[0]["peTown"].ToString();
                txtpeTaluka.Text = dt.Rows[0]["peTaluka"].ToString();
                txtpePostOffice.Text = dt.Rows[0]["pePostoffice"].ToString();
                txtpeLandmark.Text = dt.Rows[0]["peLmark"].ToString();
                txtpePin.Text = dt.Rows[0]["pepincode"].ToString();


                if (dt.Rows.Count > 0)
                {
                    //ddlbankname.SelectedValue = dt.Rows[0]["Empbankname"].ToString();

                    int value = 0;

                    ddlbankname.SelectedIndex = 0;
                    if (String.IsNullOrEmpty(dt.Rows[0]["Empbankname"].ToString()) == false)
                    {

                        value = int.Parse(dt.Rows[0]["Empbankname"].ToString());
                        if (value != 0)
                        {
                            ddlbankname.SelectedIndex = value;
                        }
                    }

                    txtbranchname.Text = dt.Rows[0]["Empbankbranchname"].ToString();
                    txtIFSCcode.Text = dt.Rows[0]["EmpIFSCcode"].ToString();
                    txtBankAppNum.Text = dt.Rows[0]["EmpBankAppNo"].ToString();
                    txtBankCardRef.Text = dt.Rows[0]["EmpBankCardRef"].ToString();
                    txtRegCode.Text = dt.Rows[0]["EmpRegionCode"].ToString();
                    txtBranchCode.Text = dt.Rows[0]["EmpBranchCode"].ToString();
                    txtInsDeb.Text = dt.Rows[0]["EmpInsDedAmt"].ToString();

                    txtBankAccNum.Text = dt.Rows[0]["EmpBankAcNo"].ToString();
                    txtBankCodenum.Text = dt.Rows[0]["EmpBankCode"].ToString();

                    txtEmpInsNominee.Text = dt.Rows[0]["EmpInsNominee"].ToString();
                    txtEmpNomRel.Text = dt.Rows[0]["EmpNomineeRel"].ToString();
                    txtaadhaar.Text = dt.Rows[0]["aadhaarid"].ToString();
                    txtInsCover.Text = dt.Rows[0]["EmpInsCover"].ToString();
                    txtSSNumber.Text = dt.Rows[0]["EmpUANNumber"].ToString();

                    if (String.IsNullOrEmpty(dt.Rows[0]["EmpNomineeDtofBirth"].ToString()) == false)
                    {

                        txtNomDoB.Text = DateTime.Parse(dt.Rows[0]["EmpNomineeDtofBirth"].ToString()).ToString("dd/MM/yyyy");
                        if (txtNomDoB.Text == "01/01/1900")
                        {
                            txtNomDoB.Text = "";
                        }

                    }
                    else
                    {
                        txtNomDoB.Text = "";
                    }


                }

                txtaddlamt.Text = dt.Rows[0]["AddlAmount"].ToString();
                txtfoodallowance.Text = dt.Rows[0]["FoodAllowance"].ToString();

                string selectquery5 = " select * from EMPEPFCodes where EmpID = '" + empid + "'";
                DataTable dt5 = config.ExecuteAdaptorAsyncWithQueryParams(selectquery5).Result;
                txtEmpPFNumber.Text = "0";

                txtEmpPFNumber.Text = "";

                if (dt5.Rows.Count > 0)
                {
                    //txtEmpPFNumber.Text = dt5.Rows[0]["EmpEpfNo"].ToString();
                    txtPFNominee.Text = dt5.Rows[0]["EmpNominee"].ToString();
                    txtPFNomineeRel.Text = dt5.Rows[0]["EmpRelation"].ToString();

                    if (String.IsNullOrEmpty(dt5.Rows[0]["EmpPFEnrolDt"].ToString()) == false)
                    {

                        txtPFEnrollDate.Text = DateTime.Parse(dt5.Rows[0]["EmpPFEnrolDt"].ToString()).ToString("dd/MM/yyyy");
                        if (txtPFEnrollDate.Text == "01/01/1900")
                        {
                            txtPFEnrollDate.Text = "";
                        }

                    }
                    else
                    {
                        txtPFEnrollDate.Text = "";
                    }
                }

                string selectquery6 = "select * from EMPESICodes where EmpID = '" + empid + "'";
                DataTable dt6 = config.ExecuteAdaptorAsyncWithQueryParams(selectquery6).Result;
                txtESINum.Text = txtESIDiSName.Text = txtESINominee.Text = txtESINomRel.Text = "0";
                if (dt6.Rows.Count > 0)
                {
                    txtESINum.Text = dt6.Rows[0]["EmpESINo"].ToString();
                    txtESIDiSName.Text = dt6.Rows[0]["EmpESIDispName"].ToString();
                    txtESINominee.Text = dt6.Rows[0]["EmpESINominee"].ToString();
                    txtESINomRel.Text = dt6.Rows[0]["EMPESIRelation"].ToString();
                }

                string selectquery7 = "select * from EmpExservice where EmpID = '" + empid + "'";
                DataTable dt7 = config.ExecuteAdaptorAsyncWithQueryParams(selectquery7).Result;

                txtServiceNum.Text = txtCrops.Text = txtMCategory.Text = txtConduct.Text = txtRank.Text =
                txtTrade.Text = TxtROfDischarge.Text = "0";
                if (dt7.Rows.Count > 0)
                {
                    txtServiceNum.Text = dt7.Rows[0]["ServiceNo"].ToString();

                    txtCrops.Text = dt7.Rows[0]["Crops"].ToString();
                    txtMCategory.Text = dt7.Rows[0]["MedcalCategoryBloodGroup"].ToString();
                    txtConduct.Text = dt7.Rows[0]["Conduct"].ToString();
                    txtRank.Text = dt7.Rows[0]["Rank"].ToString();

                    txtTrade.Text = dt7.Rows[0]["Trade"].ToString();
                    TxtROfDischarge.Text = dt7.Rows[0]["ReasonsofDischarge"].ToString();

                    if (String.IsNullOrEmpty(dt7.Rows[0]["DtofEnrolment"].ToString()) == false)
                    {

                        txtDOfEnroll.Text = DateTime.Parse(dt7.Rows[0]["DtofEnrolment"].ToString()).ToString("dd/MM/yyyy");
                        if (txtDOfEnroll.Text == "01/01/1900")
                        {
                            txtDOfEnroll.Text = "";
                        }

                    }
                    else
                    {
                        txtDOfEnroll.Text = "";
                    }


                    //if (String.IsNullOrEmpty(dt7.Rows[0]["DtofDischarge"].ToString()) == false)
                    //{

                    //    txtDOFDischarge.Text = DateTime.Parse(dt7.Rows[0]["DtofDischarge"].ToString()).ToString("dd/MM/yyyy");
                    //    if (txtDOFDischarge.Text == "01/01/1900")
                    //    {
                    //        txtDOFDischarge.Text = "";
                    //    }

                    //}
                    //else
                    //{
                    //    txtDOFDischarge.Text = "";
                    //}
                }
                string SqlPoliceRecord = "Select * from EmpPoliceRecord where empid='" + empid + "'";
                DataTable dtpr = config.ExecuteAdaptorAsyncWithQueryParams(SqlPoliceRecord).Result;

                var CriminalOffCheck = "N";
                var CriminalProCheck = "N";
                var CriminalArrestCheck = "N";
                var PoliceVerificationCheck = "N";

                if (dtpr.Rows.Count > 0)
                {

                    PoliceVerificationCheck = dtpr.Rows[0]["PoliceVerificationCheck"].ToString();
                    if (PoliceVerificationCheck == "Y")
                    {
                        rdbVerified.Checked = true;
                        txtPoliceVerificationNo.Text = dtpr.Rows[0]["PoliceVerificationNo"].ToString();
                        txtPoliceVerificationNo.Enabled = true;
                        rdbNotVerified.Checked = false;
                    }
                    else
                    {
                        rdbNotVerified.Checked = true;
                        rdbVerified.Checked = false;
                        txtPoliceVerificationNo.Enabled = false;
                    }

                    CriminalOffCheck = dtpr.Rows[0]["CriminalOffCheck"].ToString();
                    if (CriminalOffCheck == "Y")
                    {
                        ChkCriminalOff.Checked = true;
                        txtCriminalOffcaseNo.Enabled = true;
                        txtCriminalOffcaseNo.Text = dtpr.Rows[0]["CriminalOffcaseNo"].ToString();
                        txtCriminalOffCName.Enabled = true;
                        txtCriminalOffCName.Text = dtpr.Rows[0]["CriminalOffCName"].ToString();
                        txtCriminalOff.Enabled = true;
                        txtCriminalOff.Text = dtpr.Rows[0]["CriminalOff"].ToString();
                    }
                    else
                    {
                        ChkCriminalOff.Checked = false;
                        txtCriminalOffcaseNo.Enabled = false;
                        txtCriminalOffCName.Enabled = false;
                        txtCriminalOff.Enabled = false;
                    }

                    CriminalProCheck = dtpr.Rows[0]["CriminalProCheck"].ToString();
                    if (CriminalProCheck == "Y")
                    {
                        ChkCriminalProc.Checked = true;
                        txtCriminalProCaseNo.Enabled = true;
                        txtCriminalProCaseNo.Text = dtpr.Rows[0]["CriminalProCaseNo"].ToString();
                        txtCriminalProCName.Enabled = true;
                        txtCriminalProCName.Text = dtpr.Rows[0]["CriminalProCName"].ToString();
                        txtCriminalProOffence.Enabled = true;
                        txtCriminalProOffence.Text = dtpr.Rows[0]["CriminalProOffence"].ToString();

                    }
                    else
                    {
                        ChkCriminalProc.Checked = false;
                        txtCriminalProCaseNo.Enabled = false;
                        txtCriminalProCName.Enabled = false;
                        txtCriminalProOffence.Enabled = false;
                    }

                    CriminalArrestCheck = dtpr.Rows[0]["CriminalArrestCheck"].ToString();
                    if (CriminalArrestCheck == "Y")
                    {
                        ChkCrimalArrest.Checked = true;
                        txtCriminalArrestCaseNo.Enabled = true;
                        txtCriminalArrestCaseNo.Text = dtpr.Rows[0]["CriminalArrestCaseNo"].ToString();
                        txtCriminalArrestCName.Enabled = true;
                        txtCriminalArrestCName.Text = dtpr.Rows[0]["CriminalArrestCName"].ToString();
                        txtCriminalArrestOffence.Enabled = true;
                        txtCriminalArrestOffence.Text = dtpr.Rows[0]["CriminalArrestOffence"].ToString();

                    }
                    else
                    {
                        ChkCrimalArrest.Checked = false;
                        txtCriminalArrestCaseNo.Enabled = false;
                        txtCriminalArrestCName.Enabled = false;
                        txtCriminalArrestOffence.Enabled = false;
                    }

                    if (dtpr.Rows[0]["pvcState"].ToString() == "0")
                    {
                        ddlpvcstate.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlpvcstate.SelectedValue = dtpr.Rows[0]["pvcState"].ToString();
                    }

                    string Cityquery1 = "select CityID,City from cities where State='" + dtpr.Rows[0]["pvcState"].ToString() + "' order by City";
                    DataTable CityDt1 = config.ExecuteAdaptorAsyncWithQueryParams(Cityquery1).Result;
                    if (CityDt1.Rows.Count > 0)
                    {
                        ddlpvccity.Enabled = true;
                        ddlpvccity.DataValueField = "CityID";
                        ddlpvccity.DataTextField = "City";
                        ddlpvccity.DataSource = CityDt1;
                        ddlpvccity.DataBind();
                        ddlpvccity.Items.Insert(0, new ListItem("--Select--", "0"));

                    }

                    if (dtpr.Rows[0]["pvcCity"].ToString() == "0" || dtpr.Rows[0]["pvcCity"].ToString() == "")
                    {
                        ddlpvccity.SelectedIndex = 0;

                    }
                    else
                    {
                        ddlpvccity.Enabled = true;
                        ddlpvccity.Items.FindByValue(dtpr.Rows[0]["pvcCity"].ToString()).Selected = true;

                    }


                    txtpvcstay.Text = dtpr.Rows[0]["pvcperiodofstay"].ToString();

                    txtpvcpolicestation.Text = dtpr.Rows[0]["pvcPoliceStation"].ToString();
                    txtpvcvillage.Text = dtpr.Rows[0]["pvcTown"].ToString();
                    txtpvctaluka.Text = dtpr.Rows[0]["pvcTaluka"].ToString();
                    txtpvcpostofc.Text = dtpr.Rows[0]["pvcPostoffice"].ToString();
                    txtpvclandmark.Text = dtpr.Rows[0]["pvcLmark"].ToString();
                    txtpvcpin.Text = dtpr.Rows[0]["pvcPincode"].ToString();
                    txtpvcphone.Text = dtpr.Rows[0]["pvcphone"].ToString();


                    if (String.IsNullOrEmpty(dtpr.Rows[0]["pvcResidingDate"].ToString()) == false)
                    {

                        txtpvcresidedate.Text = DateTime.Parse(dtpr.Rows[0]["pvcResidingDate"].ToString()).ToString("dd/MM/yyyy");
                        if (txtpvcresidedate.Text == "01/01/1900")
                        {
                            txtpvcresidedate.Text = "";
                        }

                    }
                    else
                    {
                        txtpvcresidedate.Text = "";

                    }


                }
                string SqlProofDetails = "Select * from EmpProofDetails_Android where empid='" + empid + "'";
                DataTable dtpd = config.ExecuteAdaptorAsyncWithQueryParams(SqlProofDetails).Result;

                var AadharCard = "N";
                var drivingLicense = "N";
                var VoterID = "N";
                var ElectricityBill = "N";
                var BankPassbook = "N";
                var RationCard = "N";
                var PanCard = "N";
                var ESICCard = "N";
                var Other = "N";

                if (dtpd.Rows.Count > 0)
                {

                    AadharCard = dtpd.Rows[0]["AadharCard"].ToString();
                    txtAadharCard.Text = dtpd.Rows[0]["AadharCardNo"].ToString();
                    txtAadharName.Text = dtpd.Rows[0]["AadharCardName"].ToString();


                    drivingLicense = dtpd.Rows[0]["drivingLicense"].ToString();
                    txtDrivingLicense.Text = dtpd.Rows[0]["drivingLicenseNo"].ToString();
                    txtDrivingLicenseName.Text = dtpd.Rows[0]["DrivingLicenseName"].ToString();


                    VoterID = dtpd.Rows[0]["VoterID"].ToString();
                    txtVoterID.Text = dtpd.Rows[0]["VoterIDNo"].ToString();
                    txtVoterName.Text = dtpd.Rows[0]["VoterIDName"].ToString();


                    RationCard = dtpd.Rows[0]["RationCard"].ToString();
                    txtRationCard.Text = dtpd.Rows[0]["RationCardNo"].ToString();
                    txtRationCardName.Text = dtpd.Rows[0]["RationCardName"].ToString();


                    PanCard = dtpd.Rows[0]["PanCard"].ToString();
                    txtPanCard.Text = dtpd.Rows[0]["PanCardNo"].ToString();
                    txtPanCardName.Text = dtpd.Rows[0]["PanCardName"].ToString();


                    BankPassbook = dtpd.Rows[0]["Passbook"].ToString();
                    txtBankPassbook.Text = dtpd.Rows[0]["PassbookNo"].ToString();
                    txtBankPassBookName.Text = dtpd.Rows[0]["PassBookName"].ToString();


                    ElectricityBill = dtpd.Rows[0]["ElectricityBill"].ToString();
                    txtElectricityBill.Text = dtpd.Rows[0]["ElectricityBillNo"].ToString();
                    txtElecBillname.Text = dtpd.Rows[0]["ElectricityBillName"].ToString();

                    ESICCard = dtpd.Rows[0]["ESICCard"].ToString();
                    txtESICCardNo.Text = dtpd.Rows[0]["ESICCardNo"].ToString();
                    txtESICName.Text = dtpd.Rows[0]["ESICCardName"].ToString();


                    Other = dtpd.Rows[0]["Others"].ToString();
                    txtOther.Text = dtpd.Rows[0]["OtherType"].ToString();
                    txtOtherName.Text = dtpd.Rows[0]["OtherTypeName"].ToString();


                    #region Proof Iamges new code changes done by Mahesh Goud on 2022-06-18 TixketNo=009436

                    using (var client = new System.Net.WebClient())
                    {
                        string Str = empid + "AadharFrontPhoto.jpg";
                        string remoteUri = "http://gdxmobileapp.mydiyosfame.com/";

                        string fileName = "";

                        if (dtpd.Rows[0]["AadharCardImg"].ToString().Length > 0)
                        {
                            fileName = dtpd.Rows[0]["AadharCardImg"].ToString();


                            client.DownloadFile(remoteUri + fileName, Server.MapPath("~/assets/EmpPhotos/") + Str);

                            if (File.Exists(Server.MapPath("~/assets/EmpPhotos/") + Str))
                            {
                                AadharImg.ImageUrl = ("~/assets/EmpPhotos/") + Str;
                            }
                            else
                            {
                                AadharImg.ImageUrl = null;
                            }
                        }
                        else
                        {
                            AadharImg.ImageUrl = null;
                        }
                    }
                    using (var client = new System.Net.WebClient())
                    {
                        string Str = empid + "AadharBackPhoto.jpg";
                        string remoteUri = "http://gdxmobileapp.mydiyosfame.com/";

                        string fileName = "";

                        if (dtpd.Rows[0]["AadharCardImgBack"].ToString().Length > 0)
                        {
                            fileName = dtpd.Rows[0]["AadharCardImgBack"].ToString();


                            client.DownloadFile(remoteUri + fileName, Server.MapPath("~/assets/EmpPhotos/") + Str);

                            if (File.Exists(Server.MapPath("~/assets/EmpPhotos/") + Str))
                            {
                                AadharBackImg.ImageUrl = ("~/assets/EmpPhotos/") + Str;
                            }
                            else
                            {
                                AadharBackImg.ImageUrl = null;
                            }
                        }
                        else
                        {
                            AadharBackImg.ImageUrl = null;
                        }
                    }
                    using (var client = new System.Net.WebClient())
                    {
                        string Str = empid + "BankPassBookImg.jpg";
                        string remoteUri = "http://gdxmobileapp.mydiyosfame.com/";

                        string fileName = "";

                        if (dtpd.Rows[0]["OthersImg"].ToString().Length > 0)
                        {
                            fileName = dtpd.Rows[0]["OthersImg"].ToString();


                            client.DownloadFile(remoteUri + fileName, Server.MapPath("~/assets/EmpPhotos/") + Str);

                            if (File.Exists(Server.MapPath("~/assets/EmpPhotos/") + Str))
                            {
                                BankpassImageb.ImageUrl = ("~/assets/EmpPhotos/") + Str;
                            }
                            else
                            {
                                BankpassImageb.ImageUrl = null;
                            }
                        }
                        else
                        {
                            BankpassImageb.ImageUrl = null;
                        }
                    }
                    using (var client = new System.Net.WebClient())
                    {
                        string Str = empid + "PanCardImg.jpg";
                        string remoteUri = "http://gdxmobileapp.mydiyosfame.com/";

                        string fileName = "";

                        if (dtpd.Rows[0]["PanCardImg"].ToString().Length > 0)
                        {
                            fileName = dtpd.Rows[0]["PanCardImg"].ToString();


                            client.DownloadFile(remoteUri + fileName, Server.MapPath("~/assets/EmpPhotos/") + Str);

                            if (File.Exists(Server.MapPath("~/assets/EmpPhotos/") + Str))
                            {
                                pancardimageb.ImageUrl = ("~/assets/EmpPhotos/") + Str;
                            }
                            else
                            {
                                pancardimageb.ImageUrl = null;
                            }
                        }
                        else
                        {
                            pancardimageb.ImageUrl = null;
                        }
                    }
                    using (var client = new System.Net.WebClient())
                    {
                        string Str = empid + "PanCardImgBack.jpg";
                        string remoteUri = "http://gdxmobileapp.mydiyosfame.com/";

                        string fileName = "";

                        if (dtpd.Rows[0]["PanCardImgBack"].ToString().Length > 0)
                        {
                            fileName = dtpd.Rows[0]["PanCardImgBack"].ToString();


                            client.DownloadFile(remoteUri + fileName, Server.MapPath("~/assets/EmpPhotos/") + Str);

                            if (File.Exists(Server.MapPath("~/assets/EmpPhotos/") + Str))
                            {
                                pancardBackimageb.ImageUrl = ("~/assets/EmpPhotos/") + Str;
                            }
                            else
                            {
                                pancardBackimageb.ImageUrl = null;
                            }
                        }
                        else
                        {
                            pancardBackimageb.ImageUrl = null;
                        }
                    }
                    #endregion

                    if (AadharCard == "Y")
                    {
                        ChkAadharCard.Checked = true;
                        txtAadharCard.Enabled = true;
                        txtAadharName.Enabled = true;
                    }
                    else
                    {
                        ChkAadharCard.Checked = false;
                        txtAadharCard.Enabled = false;
                        txtAadharName.Enabled = false;

                    }

                    if (drivingLicense == "Y")
                    {
                        ChkdrivingLicense.Checked = true;
                        txtDrivingLicense.Enabled = true;
                        txtDrivingLicenseName.Enabled = true;
                    }
                    else
                    {
                        ChkdrivingLicense.Checked = false;
                        txtDrivingLicense.Enabled = false;
                        txtDrivingLicenseName.Enabled = false;

                    }

                    if (VoterID == "Y")
                    {
                        ChkVoterID.Checked = true;
                        txtVoterID.Enabled = true;
                        txtVoterName.Enabled = true;
                    }
                    else
                    {
                        ChkVoterID.Checked = false;
                        txtVoterID.Enabled = false;
                        txtVoterName.Enabled = false;

                    }

                    if (RationCard == "Y")
                    {
                        ChkRationCard.Checked = true;
                        txtRationCard.Enabled = true;
                        txtRationCardName.Enabled = true;
                    }
                    else
                    {
                        ChkRationCard.Checked = false;
                        txtRationCard.Enabled = false;
                        txtRationCardName.Enabled = false;
                    }

                    if (PanCard == "Y")
                    {
                        ChkPanCard.Checked = true;
                        txtPanCard.Enabled = true;
                        txtPanCardName.Enabled = true;
                    }
                    else
                    {
                        ChkPanCard.Checked = false;
                        txtPanCard.Enabled = false;
                        txtPanCardName.Enabled = false;
                    }


                    if (BankPassbook == "Y")
                    {
                        ChkBankPassbook.Checked = true;
                        txtBankPassbook.Enabled = true;
                        txtBankPassBookName.Enabled = true;
                    }
                    else
                    {
                        ChkBankPassbook.Checked = false;
                        txtBankPassbook.Enabled = false;
                        txtBankPassBookName.Enabled = false;

                    }

                    if (ElectricityBill == "Y")
                    {
                        ChkElectricityBill.Checked = true;
                        txtElectricityBill.Enabled = true;
                        txtElecBillname.Enabled = true;
                    }
                    else
                    {
                        ChkElectricityBill.Checked = false;
                        txtElectricityBill.Enabled = false;
                        txtElecBillname.Enabled = false;
                    }

                    if (ESICCard == "Y")
                    {
                        ChkESICCard.Checked = true;
                        txtESICCardNo.Enabled = true;
                        txtESICName.Enabled = true;
                    }
                    else
                    {
                        ChkESICCard.Checked = false;
                        txtESICCardNo.Enabled = false;
                        txtESICName.Enabled = false;
                    }

                    if (Other == "Y")
                    {
                        Chkother.Checked = true;
                        txtOther.Enabled = true;
                        txtOtherName.Enabled = true;
                    }
                    else
                    {
                        Chkother.Checked = false;
                        txtOther.Enabled = false;
                        txtOtherName.Enabled = false;

                    }


                }


                string sqlFamilyDetails = "select ER.RName,ER.RType,ER.EmpId,Convert(nvarchar(10),ER.DOfBirth,103) as DOfBirth,ER.pfnominee,ER.Esinominee,ER.age,ER.ROccupation,ER.RAAdharNo,ER.RResidence,ER.RPlace from EmpRelationships_Android as ER join EmpDetails_Android as ED on ER.EmpId=ED.EmpId where ED.EmpID = '" + empid + "' order by id ";
                DataTable dtfm = config.ExecuteAdaptorAsyncWithQueryParams(sqlFamilyDetails).Result;
                if (dtfm.Rows.Count > 0)
                {
                    gvFamilyDetails.DataSource = dtfm;
                    gvFamilyDetails.DataBind();

                    foreach (GridViewRow dr in gvFamilyDetails.Rows)
                    {
                        if (dtfm.Rows.Count == dr.RowIndex)
                        {
                            break;
                        }
                        TextBox txtEmpRName = dr.FindControl("txtEmpName") as TextBox;
                        DropDownList ddlRelationtype = dr.FindControl("ddlRelation") as DropDownList;
                        TextBox txtDOFBirth = dr.FindControl("txtRelDtofBirth") as TextBox;
                        TextBox txtAge = dr.FindControl("txtAge") as TextBox;
                        TextBox txtoccupation = dr.FindControl("txtReloccupation") as TextBox;
                        TextBox txtaadharno = dr.FindControl("txtaadharno") as TextBox;
                        DropDownList ddlrelresidence = dr.FindControl("ddlresidence") as DropDownList;
                        TextBox txtRelplace = dr.FindControl("txtplace") as TextBox;
                        CheckBox ChkPfNominee = dr.FindControl("ChkPFNominee") as CheckBox;
                        CheckBox ChkESINominee = dr.FindControl("ChkESINominee") as CheckBox;

                        txtEmpRName.Text = dtfm.Rows[dr.RowIndex]["RName"].ToString();
                        ddlRelationtype.SelectedValue = dtfm.Rows[dr.RowIndex]["RType"].ToString();
                        txtDOFBirth.Text = dtfm.Rows[dr.RowIndex]["DOfBirth"].ToString();

                        if (txtDOFBirth.Text == "01/01/1900")
                        {
                            txtDOFBirth.Text = "";
                        }
                        txtAge.Text = dtfm.Rows[dr.RowIndex]["Age"].ToString();
                        txtoccupation.Text = dtfm.Rows[dr.RowIndex]["ROccupation"].ToString();
                        txtaadharno.Text = dtfm.Rows[dr.RowIndex]["RAAdharNo"].ToString();
                        ddlrelresidence.SelectedValue = dtfm.Rows[dr.RowIndex]["RResidence"].ToString();
                        txtRelplace.Text = dtfm.Rows[dr.RowIndex]["RPlace"].ToString();
                        string PFNominee = dtfm.Rows[dr.RowIndex]["PfNominee"].ToString();
                        if (PFNominee == "Y")
                        {
                            ChkPfNominee.Checked = true;

                        }
                        else
                        {
                            ChkPfNominee.Checked = false;
                        }
                        string ESINominee = dtfm.Rows[dr.RowIndex]["EsiNominee"].ToString();
                        if (ESINominee == "Y")
                        {
                            ChkESINominee.Checked = true;

                        }
                        else
                        {
                            ChkESINominee.Checked = false;
                        }


                    }

                }
                else
                {
                    for (int i = 0; i < gvFamilyDetails.Rows.Count; i++)
                    {
                        TextBox txtEmpRName = gvFamilyDetails.Rows[i].FindControl("txtEmpName") as TextBox;
                        DropDownList ddlRelationtype = gvFamilyDetails.Rows[i].FindControl("ddlRelation") as DropDownList;
                        // TextBox txtDOFBirth = gvFamilyDetails.Rows[i].FindControl("txtRelDtofBirth") as TextBox;
                        TextBox txtAge = gvFamilyDetails.Rows[i].FindControl("txtAge") as TextBox;
                        TextBox txtoccupation = gvFamilyDetails.Rows[i].FindControl("txtReloccupation") as TextBox;
                        TextBox txtaadharno = gvFamilyDetails.Rows[i].FindControl("txtaadharno") as TextBox;
                        DropDownList ddlrelresidence = gvFamilyDetails.Rows[i].FindControl("ddlresidence") as DropDownList;
                        TextBox txtRelplace = gvFamilyDetails.Rows[i].FindControl("txtplace") as TextBox;
                        CheckBox ChkPfNominee = gvFamilyDetails.Rows[i].FindControl("ChkPFNominee") as CheckBox;
                        CheckBox ChkESINominee = gvFamilyDetails.Rows[i].FindControl("ChkESINominee") as CheckBox;



                        txtEmpRName.Text = "";
                        ddlRelationtype.SelectedIndex = 0;
                        //  txtDOFBirth.Text = "";
                        txtAge.Text = "";
                        txtoccupation.Text = "";
                        ddlrelresidence.SelectedIndex = 0;
                        txtRelplace.Text = "";
                        ChkPfNominee.Checked = false;
                        ChkESINominee.Checked = false;


                    }

                }




                string sqlEducationDetails = "select * from EmpEducationDetails where EmpID = '" + empid + "' order by id ";
                DataTable dted = config.ExecuteAdaptorAsyncWithQueryParams(sqlEducationDetails).Result;
                if (dted.Rows.Count > 0)
                {
                    GvEducationDetails.DataSource = dted;
                    GvEducationDetails.DataBind();

                    foreach (GridViewRow dr in GvEducationDetails.Rows)
                    {
                        if (dted.Rows.Count == dr.RowIndex)
                        {
                            break;
                        }

                        DropDownList ddlQualification = dr.FindControl("ddlQualification") as DropDownList;
                        TextBox txtEdLevel = dr.FindControl("txtEdLevel") as TextBox;
                        TextBox txtNameofSchoolColg = dr.FindControl("txtNameofSchoolColg") as TextBox;
                        TextBox txtBoard = dr.FindControl("txtBoard") as TextBox;
                        TextBox txtyear = dr.FindControl("txtyear") as TextBox;
                        TextBox txtPassFail = dr.FindControl("txtPassFail") as TextBox;
                        TextBox txtPercentage = dr.FindControl("txtPercentage") as TextBox;

                        ddlQualification.SelectedValue = dted.Rows[dr.RowIndex]["Qualification"].ToString();
                        txtEdLevel.Text = dted.Rows[dr.RowIndex]["Description"].ToString();
                        txtNameofSchoolColg.Text = dted.Rows[dr.RowIndex]["NameOfSchoolClg"].ToString();
                        txtBoard.Text = dted.Rows[dr.RowIndex]["BoardorUniversity"].ToString();
                        txtyear.Text = dted.Rows[dr.RowIndex]["YrOfStudy"].ToString();
                        txtPassFail.Text = dted.Rows[dr.RowIndex]["PassOrFail"].ToString();
                        txtPercentage.Text = dted.Rows[dr.RowIndex]["PercentageOfmarks"].ToString();

                    }

                }
                else
                {
                    for (int i = 0; i < GvEducationDetails.Rows.Count; i++)
                    {
                        DropDownList ddlQualification = GvEducationDetails.Rows[i].FindControl("ddlQualification") as DropDownList;
                        TextBox txtEdLevel = GvEducationDetails.Rows[i].FindControl("txtEdLevel") as TextBox;
                        TextBox txtNameofSchoolColg = GvEducationDetails.Rows[i].FindControl("txtNameofSchoolColg") as TextBox;
                        TextBox txtBoard = GvEducationDetails.Rows[i].FindControl("txtBoard") as TextBox;
                        TextBox txtyear = GvEducationDetails.Rows[i].FindControl("txtyear") as TextBox;
                        TextBox txtPassFail = GvEducationDetails.Rows[i].FindControl("txtPassFail") as TextBox;
                        TextBox txtPercentage = GvEducationDetails.Rows[i].FindControl("txtPercentage") as TextBox;

                        txtEdLevel.Text = "";
                        txtPassFail.Text = "";
                        txtPercentage.Text = "";
                        txtyear.Text = "";
                        txtNameofSchoolColg.Text = "";
                        ddlQualification.SelectedIndex = 0;


                    }

                }
                string sqlprevExpDetails = "select *,Convert(nvarchar(10),DateofResign,103) as DateofResign1 from EmpPrevExperience where EmpID = '" + empid + "' order by id ";
                DataTable dtped = config.ExecuteAdaptorAsyncWithQueryParams(sqlprevExpDetails).Result;
                if (dtped.Rows.Count > 0)
                {
                    GvPreviousExperience.DataSource = dtped;
                    GvPreviousExperience.DataBind();

                    foreach (GridViewRow dr in GvPreviousExperience.Rows)
                    {
                        if (dtped.Rows.Count == dr.RowIndex)
                        {
                            break;
                        }

                        TextBox txtregioncode = dr.FindControl("txtregioncode") as TextBox;
                        TextBox txtempcode = dr.FindControl("txtempcode") as TextBox;
                        TextBox txtExtension = dr.FindControl("txtExtension") as TextBox;
                        TextBox txtPrevDesignation = dr.FindControl("txtPrevDesignation") as TextBox;
                        TextBox txtCompAddress = dr.FindControl("txtCompAddress") as TextBox;
                        TextBox txtyearofexp = dr.FindControl("txtyearofexp") as TextBox;
                        TextBox txtPFNo = dr.FindControl("txtPFNo") as TextBox;
                        TextBox txtESINo = dr.FindControl("txtESINo") as TextBox;
                        TextBox txtDtofResigned = dr.FindControl("txtDtofResigned") as TextBox;


                        txtregioncode.Text = dtped.Rows[dr.RowIndex]["RegionCode"].ToString();
                        txtempcode.Text = dtped.Rows[dr.RowIndex]["EmployerCode"].ToString();
                        txtExtension.Text = dtped.Rows[dr.RowIndex]["Extension"].ToString();
                        txtPrevDesignation.Text = dtped.Rows[dr.RowIndex]["Designation"].ToString();
                        txtCompAddress.Text = dtped.Rows[dr.RowIndex]["CompAddress"].ToString();
                        txtyearofexp.Text = dtped.Rows[dr.RowIndex]["YrOfExp"].ToString();
                        txtPFNo.Text = dtped.Rows[dr.RowIndex]["PFNo"].ToString();
                        txtESINo.Text = dtped.Rows[dr.RowIndex]["ESINo"].ToString();
                        txtDtofResigned.Text = dtped.Rows[dr.RowIndex]["DateofResign1"].ToString();
                        if (txtDtofResigned.Text == "01/01/1900")
                        {
                            txtDtofResigned.Text = "";
                        }

                    }

                }
                else
                {
                    for (int i = 0; i < GvPreviousExperience.Rows.Count; i++)
                    {
                        TextBox txtregioncode = GvPreviousExperience.Rows[i].FindControl("txtregioncode") as TextBox;
                        TextBox txtempcode = GvPreviousExperience.Rows[i].FindControl("txtempcode") as TextBox;
                        TextBox txtExtension = GvPreviousExperience.Rows[i].FindControl("txtExtension") as TextBox;
                        TextBox txtPrevDesignation = GvPreviousExperience.Rows[i].FindControl("txtPrevDesignation") as TextBox;
                        TextBox txtCompAddress = GvPreviousExperience.Rows[i].FindControl("txtCompAddress") as TextBox;
                        TextBox txtyearofexp = GvPreviousExperience.Rows[i].FindControl("txtyearofexp") as TextBox;
                        TextBox txtPFNo = GvPreviousExperience.Rows[i].FindControl("txtPFNo") as TextBox;
                        TextBox txtESINo = GvPreviousExperience.Rows[i].FindControl("txtESINo") as TextBox;
                        TextBox txtDtofResigned = GvPreviousExperience.Rows[i].FindControl("txtDtofResigned") as TextBox;

                        txtregioncode.Text = "";
                        txtDtofResigned.Text = "";
                        txtESINo.Text = "";
                        txtPFNo.Text = "";
                        txtyearofexp.Text = "";
                        txtCompAddress.Text = "";
                        txtPrevDesignation.Text = "";
                        txtempcode.Text = "";
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        /*(1) Begin Code For the Employee Personal Information Tab (SAVE/CANCEL/CLEAR DATA METHOD)   As on [10-04-2013]*/

        protected void Btn_Save_Personal_Tab_Click(object sender, EventArgs e)//Personal Tab add/Insert  Button click
        {

            System.Threading.Thread.Sleep(500);
            lblSuc.Text = "";
            lblMsg.Text = "";
            try
            {


                CheckEmpid();


                #region  Begin  Check Validations as on  [18-09-2013]

                #region   /*Begin  Validation For Empid validation  */ as on 30/06/2014 by Venkat
                if (txtEmID.Text.Trim().Length == 0 || txtEmID.Text == "000000")
                {
                    lblMsg.Text = "Please Fill EmpId!";
                    return;

                }

                if (txtEmID.Text.Trim().Length < 6)
                {
                    lblMsg.Text = "Empid entered 6 digit Id!";
                    return;

                }




                string Checkempid = "select empid from empdetails where empid='" + txtEmID.Text + "'";
                DataTable dtempid = config.ExecuteReaderWithQueryAsync(Checkempid).Result;
                if (dtempid.Rows.Count > 0)
                {
                    if (rdbGeneral.Checked == true || rdbStaff.Checked == true)
                    {
                        employeeid();
                        //lblMsg.Text = "This Employee Id No: '" + txtEmID.Text + "' is already exist";
                        //return;
                    }
                }

                #endregion     /*End  Validation For Empid Validation */

                #region   /*Begin  Validation For First Name  */
                if (txtEmpFName.Text.Trim().Length == 0)
                {
                    lblMsg.Text = "Please Fill firstname!";
                    return;

                }
                #endregion     /*End  Validation For First Name  */

                #region /*Begin Validation For Middle Name  */
                //if (txtEmpmiName.Text.Trim().Length == 0)
                //    {
                //        lblMsg.Text = "Please fill Middle Name!";
                //        return;
                //    }
                #endregion  /*End  Validation For Middle Name  */

                #region /*Begin  Validation For Gender   */

                if (rdbmale.Checked == false && rdbfemale.Checked == false && rdbTransgender.Checked == false)
                {
                    lblMsg.Text = "Please Select The gender!";
                    return;
                }
                #endregion    /*End  Validation For Gender   */

                #region /*Begin  Validation For Marital Status   */
                if (rdbsingle.Checked == false && rdbmarried.Checked == false && rdbdivorcee.Checked == false && rdbWidower.Checked == false)
                {
                    lblMsg.Text = "Please Select The Marital Status";
                    return;
                }
                #endregion /*End  Validation For Marital Status  */

                #region Begin code For Check The Entered Dates are Valid Or Not as on [25-10-2013]
                var testDate = 0;
                #region Begin  Date Of Interivew

                if (txtEmpDtofInterview.Text == "0")
                {
                    lblMsg.Text = "You Are Entered Invalid Date Of Interview.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                    return;
                }

                if (txtEmpDtofInterview.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtEmpDtofInterview.Text);
                    if (testDate > 0)
                    {
                        lblMsg.Text = "You Are Entered Invalid Date Of Interview.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        return;
                    }
                }
                #endregion End   Date Of Interivew

                #region Begin  Date Of Joining

                if (txtEmpDtofJoining.Text == "0")
                {
                    lblMsg.Text = "You Are Entered Invalid Date Of Joining.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                    return;
                }

                if (txtEmpDtofJoining.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtEmpDtofJoining.Text);
                    if (testDate > 0)
                    {
                        lblMsg.Text = "You Are Entered Invalid Date Of Joining.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        return;
                    }
                }
                #endregion End   Date Of Joining


                #region Begin  Date Of Birth

                if (txtEmpDtofBirth.Text == "0")
                {
                    lblMsg.Text = "You Are Entered Invalid Date Of Birth.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                    return;
                }
                if (txtEmpDtofBirth.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtEmpDtofBirth.Text);
                    if (testDate > 0)
                    {
                        lblMsg.Text = "You Are Entered Invalid Date Of Birth.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        return;
                    }
                }
                #endregion End   Date Of Birth

                #region Begin  Date Of Leaving

                if (txtDofleaving.Text == "0")
                {
                    lblMsg.Text = "You Are Entered Invalid Date Of Leaving.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                    return;
                }

                if (txtDofleaving.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtDofleaving.Text);
                    if (testDate > 0)
                    {
                        lblMsg.Text = "You Are Entered Invalid Date Of Leaving.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        return;
                    }
                }
                #endregion End   Date Of Leaving


                #endregion End  code For Check The Entered Dates are Valid Or Not as on [25-10-2013]




                #region /*Begin  Validation For Date Of Joining */

                //if (txtEmpDtofJoining.Text.Trim().Length==0)
                //{
                //    lblMsg.Text = "Please fill Joining Date";
                //    return;
                //}


                #endregion   /*End  Validation For Date Of Joining  */

                #region /*Begin  Validation For Designation  */
                if (ddlDesignation.SelectedIndex == 0)
                {

                    lblMsg.Text = "Please Select the Designation";
                    return;
                }

                #endregion   /*End  Validation For Designation  */

                #region   /*  Begin check The Interview Date <  Joining Date  */
                if (txtEmpDtofInterview.Text.Trim().Length != 0 && txtEmpDtofJoining.Text.Trim().Length != 0)
                {
                    string strDate = txtEmpDtofInterview.Text;
                    string EndDate = txtEmpDtofJoining.Text;
                    DateTime dt1;
                    DateTime dt2;


                    dt1 = DateTime.Parse(strDate, CultureInfo.GetCultureInfo("en-gb"));
                    dt2 = DateTime.Parse(EndDate, CultureInfo.GetCultureInfo("en-gb"));


                    DateTime date1 = new DateTime(dt1.Year, dt1.Month, dt1.Day);
                    DateTime date2 = new DateTime(dt2.Year, dt2.Month, dt2.Day);

                    int result = DateTime.Compare(date1, date2);
                    if (result > 0)
                    {
                        lblMsg.Text = "Invalid Joining Date!";
                        return;
                    }
                }
                #endregion /*  End check The Interview Date <  Joining Date */

                #region  /* Begin Check The Dat Of Birth is More Than 18 Years   */
                if (txtEmpDtofBirth.Text.Trim().Length != 0)
                {
                    DateTime dayStart = DateTime.Parse(DateTime.Now.ToString());
                    DateTime dateEnd = DateTime.Parse(txtEmpDtofBirth.Text, CultureInfo.GetCultureInfo("en-gb"));
                    TimeSpan ts = dayStart - dateEnd;
                    //int result = DateTime.Compare(dayStart, dateEnd);
                    int years = ts.Days / 365;

                    if (years < 18)
                    {
                        lblMsg.Text = "Age Should be above 18 years";
                        txtEmpDtofBirth.Text = "";//; DateTime.Parse("01/01/0001"); 
                        return;
                    }

                }
                #endregion  /*End  Check The Dat Of Birth is More Than 18 Years   */

                #region  /*Begin Check the Phone Number Valid Or Not*/
                if (txtPhone.Text.Trim().Length == 0)
                {
                    lblMsg.Text = "Please fill Phone No.";
                    return;
                }
                if (txtPhone.Text.Trim().Length > 0)
                    if (txtPhone.Text.Trim().Length < 8)
                    {
                        lblMsg.Text = "Please enter a valid Phone Number!";
                        return;
                    }


                #endregion  /*End Check the Phone Number Valid Or Not*/

                #region Begin  Date Of Enrollment

                if (txtDOfEnroll.Text == "0")
                {
                    lblMsg.Text = "You Are Entered Invalid Date Of Enrollment .Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                    return;
                }

                if (txtDOfEnroll.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtDOfEnroll.Text);
                    if (testDate > 0)
                    {
                        lblMsg.Text = "You Are Entered Invalid Date Of Enrollment .Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid Date Of Enrollment .Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }
                }
                #endregion End   Date Of Enrollment


                #region Begin  Date Of Discharge

                if (txtDofDischarge.Text == "0")
                {
                    lblMsg.Text = "You Are Entered Invalid Date Of Discharge.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                    return;
                }

                if (txtDofDischarge.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtDofDischarge.Text);
                    if (testDate > 0)
                    {
                        lblMsg.Text = "You Are Entered Invalid Date Of Discharge.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        return;
                    }
                }
                #endregion End   Date Of Discharge


                if (rdbResigned.Checked == true)
                {
                    if (txtDofleaving.Text == " ")
                    {
                        lblMsg.Text = "Please fill a Date of Leaving";
                        return;
                    }

                }

                #endregion End   Check Validations as on  [18-09-2013]

                #region Begin Check Validaton as on [02-10-2013]


                #region Begin code For Check The Entered Dates are Valid Or Not as on [25-10-2013]

                #region Begin   PF Enrollment Date

                if (txtPFEnrollDate.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtPFEnrollDate.Text);
                    if (testDate > 0)
                    {
                        lblMsg.Text = "You Are Entered Invalid PF Enroll Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid PF Enroll Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }
                }

                if (txtNomDoB.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtNomDoB.Text);
                    if (testDate > 0)
                    {
                        lblMsg.Text = "You Are Entered Invalid Nominee  Date of Birth.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid Nominee  Date of Birth.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }
                }

                #endregion End   PF Enrollment Date

                #endregion End  code For Check The Entered Dates are Valid Or Not as on [25-10-2013]
                #endregion Begin Check Validaton as on [02-10-2013]

                string path = "";

                #region  Begin   Variable Declaration   as on  [18-09-2013]
                #region  Begin  1 to 5    Empid to Gender
                var Empstatus = 0;
                var Empid = string.Empty;
                var EmpFName = string.Empty;
                var EmpMName = string.Empty;
                var EmpLName = string.Empty;
                var EmpSex = "M";
                var Title = "";
                var Gross = "0";
                var EmpaddresStatus = "NR";

                #endregion  End  1 to 5    Empid to Gender

                #region  Begin  6 to 10    Marital Status  to Date Of Leaving
                var EmpMaritalStatus = "S";
                var EmpDtofInterview = string.Empty;
                var EmpDtofJoining = string.Empty;
                var EmpDtofBirth = string.Empty;
                var EmpDtofLeaving = string.Empty;
                #endregion  End  6 to 10     Marital Status  to Date Of Leaving

                #region  Begin  11 to 15     Qualification  to Father/Spouse Age
                var EmpQualification = string.Empty;
                var EmpDesgn = string.Empty;
                var EmpFatherName = string.Empty;
                var EmpFatherOccupation = string.Empty;
                var EmpSpouseName = string.Empty;
                //  var EmpOldEmployeeId = string.Empty;
                var MotherName = string.Empty;
                #endregion    Begin  11 to 15     Qualification  to Father/Spouse Age

                #region  Begin  16 to 20     Previous Employeer to  EmpPhone  
                var EmpPreviousExp = string.Empty;


                var MotherTongue = string.Empty;
                var EmpPhone = string.Empty;
                #endregion    Begin  16 to 21      Previous Employeer to  EmpPhone

                #region  Begin  21 to 25     Nationality/Religion  to PT-Deduct

                var Nationality = string.Empty;
                var Religion = string.Empty;
                var EmpESIDeduct = 0;
                var EmpPFDeduct = 0;
                var EmpExservice = 0;
                var EmpPTDeduct = 0;
                #endregion    Begin  21 to 25     Marital Status  to PT-Deduct


                #region  Begin  26 to 27    Languages  Known to Division
                var EmpLanguagesKnown = string.Empty;
                var community = 0;
                var PsaraEmpCode = "";
                var Email = "";
                var IDCardIssued = "";
                var IDCardValid = "";
                var Branch = "0";
                var ReportingManager = "0";
                var Department = "0";
                var Division = "0";
                #endregion    Begin  28 to 29    Languages  Known to Division

                #region   Begin Code For Extra Variables for This Page as [19-09-2013]
                var RecordStatus = 0;
                string[] Values = new string[2];

                #endregion  End Code For Extra Variables for This Page as [19-09-2013]

                #region Begin Code as on 24/12/2013 by venkat

                var PreferedUnit = string.Empty;

                #endregion

                #endregion  End Variable Declaration  as on  [18-09-2013]

                #region  Begin   Variable Declaration   as on  [01-10-2013]

                #region  Begin  1 to 7    RefName&Address1 to Identification Marks2
                var EmpRefAddr1 = string.Empty;
                var EmpRefAddr2 = string.Empty;
                var EmpBloodGroup = string.Empty;
                var EmpPhysicalRemarks = string.Empty;
                var EmpRemarks = string.Empty;
                var EmpIdMark1 = string.Empty;
                var EmpIdMark2 = string.Empty;
                #endregion  End  1 to 7    Ref Name&Address1 to Identification Marks2

                #region  Begin  8 to 11   Height to Eyescolor
                var EmpHeight = string.Empty;
                var EmpWeight = string.Empty;
                var EmpChestExp = string.Empty;
                var EmpChestunex = string.Empty;
                var Haircolor = "";
                var EyesColor = "";
                #endregion  End  8 to 11   Height to Eyescolor

                #region  Begin  12 to 21 Present  DoorNo to Family Details
                var prDoorno = string.Empty;
                var prStreet = string.Empty;
                var prLmark = string.Empty;
                var prArea = string.Empty;
                var prCity = "0";
                var prDistrict = string.Empty;
                var prPincode = string.Empty;
                var prState = "0";
                var prpolicestation = string.Empty;
                var prtaluka = string.Empty;
                var prtown = string.Empty;
                var prPostOffice = string.Empty;


                var prphone = string.Empty;
                var prperiodofstay = string.Empty;
                var prResidingDate = string.Empty;
                #endregion  End   12 to 21 Present  DoorNo to Family Details

                #region  Begin  22 to 30  Permanent DoorNo to PhoneNo
                var pedoor = string.Empty;
                var peStreet = string.Empty;
                var pelmark = string.Empty;
                var peArea = string.Empty;
                var peCity = "0";
                var peDistrict = string.Empty;
                var pePincode = string.Empty;
                var peState = "0";
                var pephone = string.Empty;
                var pepolicestation = string.Empty;
                var petaluka = string.Empty;
                var petown = string.Empty;
                var pePostOffice = string.Empty;
                var PermanentAddress = string.Empty;
                var PresentAddress = string.Empty;
                var periodofstay = string.Empty;
                var ResidingDate = string.Empty;
                #endregion  End  22 to 30  Permanent DoorNo to PhoneNo

                #region  Begin PVCAddress variables
                var pvcDoorno = string.Empty;
                var pvcStreet = string.Empty;
                var pvcLmark = string.Empty;
                var pvcArea = string.Empty;
                var pvcCity = "0";
                var pvcDistrict = string.Empty;
                var pvcPincode = string.Empty;
                var pvcState = "0";
                var pvcpolicestation = string.Empty;
                var pvctaluka = string.Empty;
                var pvctown = string.Empty;
                var pvcPostOffice = string.Empty;


                var pvcphone = string.Empty;
                var pvcperiodofstay = string.Empty;
                var pvcResidingDate = string.Empty;
                #endregion  End   12 to 21 Present  DoorNo to Family Details

                #region   Begin Extra Varibles for This Event   As on [01-10-2013]
                var IRecordStatus = 0;
                #endregion End Extra Varibles for This Event   As on [01-10-2013]

                #endregion  End Variable Declaration  as on  [01-10-2013]

                #region Begin Variable Declaration as on [03-10-2013]

                #region Begin [1-5] SSC(Name&address of School/clg to Percentagemarks)

                var sscschool = string.Empty;
                var sscbduniversity = string.Empty;
                var sscstdyear = string.Empty;
                var sscpassfail = string.Empty;
                var sscmarks = string.Empty;

                #endregion End [1-5] SSC(Name&address of School/clg to Percentagemarks)

                #region Begin [6-10] Inter(Name&address of School/clg to Percentagemarks)

                var imschool = string.Empty;
                var imbduniversity = string.Empty;
                var imstdyear = string.Empty;
                var impassfail = string.Empty;
                var immarks = string.Empty;

                #endregion End [6-10] Inter(Name&address of School/clg to Percentagemarks)

                #region Begin [11-15] Degree(Name&address of School/clg to Percentagemarks)

                var dgschool = string.Empty;
                var dgbduniversity = string.Empty;
                var dgstdyear = string.Empty;
                var dgmarks = string.Empty;
                var dgpassfail = string.Empty;

                #endregion End [16-20] P.G(Name&address of School/clg to Percentagemarks)

                #region Begin [16-20] P.G(Name&address of School/clg to Percentagemarks)
                var pgschool = string.Empty;
                var pgbduniversity = string.Empty;
                var pgstdyear = string.Empty;
                var pgpassfail = string.Empty;
                var pgmarks = string.Empty;

                #endregion End [16-20] P.G(Name&address of School/clg to Percentagemarks)

                #region   Begin Extra Varibles for This Event   As on [02-10-2013]

                #endregion End Extra Varibles for This Event   As on [02-10-2013]


                #endregion End Variable Declaration as on [03-10-2013]



                #region Begin Variable Declaration as on [02-10-2013]

                #region Begin 1-5 Bank Name to Branch Code

                var Empbankname = string.Empty;
                var EmpBankAcNo = string.Empty;
                var Empbankbranchname = string.Empty;
                var EmpIFSCcode = string.Empty;
                var EmpBranchCode = string.Empty;

                #endregion End 1-5 Bank Name to Branch Code

                #region Begin 6-10  Bank CodeNo to Branch Card Reference

                var EmpBankCode = string.Empty;
                var EmpBankAppNo = string.Empty;
                var EmpRegionCode = string.Empty;
                var EmpInsNominee = string.Empty;
                var EmpBankCardRef = string.Empty;

                #endregion End 6-10  Bank CodeNo to Branch Card Reference

                #region Begin 11-15  Nominee Date of Borth to SS No.

                var EmpNomineeDtofBirth = "01/01/1900";
                var EmpNomineeRel = string.Empty;
                var EmpInsCover = string.Empty;
                var EmpInsDedAmt = string.Empty;
                var EmpUANNumber = string.Empty;

                #endregion End 11-15  Nominee Date of Borth to SS No.

                #region Begin 16-20  EPFNo to EF Nominee Relation

                var EmpEpfNo = string.Empty;
                var EmpNominee = string.Empty;
                var EmpPFEnrolDt = "01/01/1900";
                var CmpShortName = string.Empty;
                var EmpRelation = string.Empty;

                #endregion end 16-20  EPFNo to EF Nominee Relation

                #region Begin 21-25  ESINo to ESI Nominee Relation

                var EmpESINo = string.Empty;
                var EmpESINominee = string.Empty;
                var EmpESIDispName = string.Empty;
                var aadhaarid = string.Empty;
                var EmpESIRelation = string.Empty;

                #endregion End 21-25  ESINo to ESI Nominee Relation

                #region   Begin salary

                var AddlAmount = string.Empty;
                var FoodAllowance = string.Empty;
                var RejoinEmpid = string.Empty;
                var RejoinStatus = 0;
                #endregion End salary

                #endregion End Variable Declaration as on [02-10-2013]


                #region  Begin  Assign Variables  as on  [18-09-2013]
                #region Begin  1 to 5    Empid to Gender
                if (rdbactive.Checked)
                    Empstatus = 1;
                else if (rdbResigned.Checked)
                    Empstatus = 0;
                else if (rdbAbsconded.Checked)
                    Empstatus = 2;

                Empid = txtEmID.Text;
                EmpFName = txtEmpFName.Text;
                EmpMName = txtEmpmiName.Text;
                EmpLName = txtEmplname.Text;
                if (rdbfemale.Checked)
                    EmpSex = "F";
                else if (rdbmale.Checked)
                    EmpSex = "M";
                else
                    EmpSex = "T";

                #endregion End  1 to 5    Empid to Gender

                #region Begin  6 to 10    Marital Status  to Date Of Leaving

                if (rdbmarried.Checked)
                    EmpMaritalStatus = "M";
                else if (rdbdivorcee.Checked)
                    EmpMaritalStatus = "D";
                else if (rdbWidower.Checked)
                    EmpMaritalStatus = "W";
                else
                    EmpMaritalStatus = "S";



                if (rdbpreaddress.Checked)
                    EmpaddresStatus = "PR";
                else if (rdbperaddress.Checked)
                    EmpaddresStatus = "PE";
                else
                    EmpaddresStatus = "NR";

                if (txtEmpDtofInterview.Text.Trim().Length != 0)
                {
                    EmpDtofInterview = Timings.Instance.CheckDateFormat(txtEmpDtofInterview.Text);
                }
                else
                {
                    EmpDtofInterview = "01/01/1900";
                }


                if (txtEmpDtofJoining.Text.Trim().Length != 0)
                {
                    EmpDtofJoining = Timings.Instance.CheckDateFormat(txtEmpDtofJoining.Text);

                }
                else
                {
                    EmpDtofJoining = "01/01/1900";
                }

                if (txtEmpDtofBirth.Text.Trim().Length != 0)
                {
                    //EmpDtofBirth = DateTime.Parse(txtEmpDtofBirth.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                    EmpDtofBirth = Timings.Instance.CheckDateFormat(txtEmpDtofBirth.Text);

                }
                else
                {
                    EmpDtofBirth = "01/01/1900";
                }

                if (txtDofleaving.Text.Trim().Length != 0)
                {
                    // EmpDtofLeaving = DateTime.Parse(txtDofleaving.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                    EmpDtofLeaving = Timings.Instance.CheckDateFormat(txtDofleaving.Text);

                }
                else
                {
                    EmpDtofLeaving = "01/01/1900";
                }

                Title = ddlTitle.SelectedValue;
                Gross = txtGrossSalary.Text;

                #endregion End  6 to 10     Marital Status  to Date Of Leaving

                #region  Begin  11 to 15     Qualification  to Father/Spouse Age



                EmpQualification = txtQualification.Text;
                EmpDesgn = ddlDesignation.SelectedValue;
                EmpFatherName = txtFatherName.Text;
                EmpFatherOccupation = txtfatheroccupation.Text;
                EmpSpouseName = txtSpousName.Text;
                //   EmpOldEmployeeId = txtoldemployeeid.Text;
                //EmpFatherOccupation = txtFaocccu.Text;
                //EmpFatherSpouseRelation = txtFaSpRelation.Text;
                //EmpFatherAge = txtFAge.Text;

                #endregion End  11 to 15     Qualification  to Father/Spouse Age

                #region  Begin  16 to 20     Previous Employeer to  EmpPhone
                EmpPreviousExp = txtPreEmp.Text;
                MotherName = txtMotherName.Text;
                //Moccupation = txtmoccupation.Text;
                MotherTongue = txtmtongue.Text; ;
                EmpPhone = txtPhone.Text.Trim();
                #endregion    Begin  16 to 20      Previous Employeer to EmpPhone

                #region for EmpadressStatus





                #endregion for EmpaddressStatus
                #region  Begin  21 to 25     Nationality/Religion  to PT-Deduct

                Nationality = txtnationality.Text;
                Religion = txtreligion.Text;
                if (ChkESIDed.Checked)
                    EmpESIDeduct = 1;
                if (ChkPFDed.Checked)
                    EmpPFDeduct = 1;
                if (ChkExService.Checked)
                    EmpExservice = 1;
                if (ChkPTDed.Checked)
                    EmpPTDeduct = 1;
                #endregion    Begin  21 to 25     Marital Status  to PT-Deduct

                #region  Begin  26 to 27    Languages  Known to Division
                EmpLanguagesKnown = txtLangKnown.Text;
                community = 0;
                if (rdsc.Checked)
                {
                    community = 0;
                }
                if (rdst.Checked)
                {
                    community = 1;
                }
                if (rdobc.Checked)
                {
                    community = 2;
                }
                if (rdur.Checked)
                {
                    community = 3;
                }

                PsaraEmpCode = txtpsaraempcode.Text;
                Email = txtemail.Text;
                if (TxtIDCardIssuedDt.Text.Trim().Length != 0)
                {
                    IDCardIssued = Timings.Instance.CheckDateFormat(TxtIDCardIssuedDt.Text);

                }
                else
                {
                    IDCardIssued = "01/01/1900";
                }
                if (TxtIdCardValid.Text.Trim().Length != 0)
                {
                    IDCardValid = Timings.Instance.CheckDateFormat(TxtIdCardValid.Text);

                }
                else
                {
                    IDCardValid = "01/01/1900";
                }
                Branch = ddlBranch.SelectedValue;
                Department = ddldepartment.SelectedValue;
                Division = ddlDivision.SelectedValue;
                ReportingManager = ddlReportingMgr.SelectedValue;

                #endregion    Begin  26 to 27    Languages  Known to Division

                #region Begin Code as on 24/12/2013 by venkat
                if (DdlPreferedUnit.SelectedIndex > 0)
                    PreferedUnit = DdlPreferedUnit.SelectedValue;
                else
                    PreferedUnit = "0";
                #endregion

                #endregion  End Assign Variables  as on  [18-09-2013]


                #region  Begin  Assign Variables  as on  [01-10-2013]

                #region  Begin  1 to 7    RefName&Address1 to Identification Marks2
                EmpRefAddr1 = txtREfAddr1.Text;
                EmpRefAddr2 = txtREfAddr2.Text;
                EmpPhysicalRemarks = txtPhyRem.Text;
                EmpRemarks = txtEmpRemarks.Text;
                EmpIdMark1 = txtImark1.Text;
                EmpIdMark2 = txtImark2.Text;

                if (ddlBloodGroup.SelectedIndex == 0)
                {
                    EmpBloodGroup = "0";
                }
                else
                {
                    EmpBloodGroup = ddlBloodGroup.SelectedValue;
                }
                #endregion  End  1 to 7    RefName&Address1 to Identification Marks2

                #region  Begin  8 to 11   Height to Expand
                EmpHeight = txtheight.Text;
                EmpWeight = txtweight.Text;
                EmpChestExp = txtcheexpan.Text;
                EmpChestunex = txtcheunexpan.Text;
                Haircolor = txthaircolour.Text;
                EyesColor = txtEyeColour.Text;

                #endregion  End  8 to 11   Height to Expand

                #region  Begin  12 to 21 Present  DoorNo to Family Details
                //prDoorno = txtPrdoor.Text;
                //prStreet = txtstreet.Text;
                //prLmark = txtlmark.Text;
                //prArea = txtarea.Text;
                //prDistrict = txtdistrictt.Text;
                //prPincode = txtpin.Text;
                //PresentAddress = txtPresentAddress.Text;
                prState = ddlpreStates.SelectedValue;

                if (ddlpreCity.SelectedIndex > 0)
                {
                    prCity = ddlpreCity.SelectedValue;
                }
                else
                {
                    prCity = "0";
                }
                prphone = txtmobile.Text;
                prperiodofstay = txtprPeriodofStay.Text;
                prpolicestation = txtprPoliceStation.Text;
                prtaluka = txtprtaluka.Text;
                prtown = txtprvillage.Text;
                prPincode = txtprpin.Text;
                prLmark = txtprLandmark.Text;
                prPostOffice = txtprPostOffice.Text;

                if (txtprResidingDate.Text.Trim().Length != 0)
                {
                    prResidingDate = Timings.Instance.CheckDateFormat(txtprResidingDate.Text);

                }


                //EmpFamilyDetails = txtFamDetails.Text;

                #endregion  End   12 to 21 Present  DoorNo to Family Details

                #region  Begin  22 to 30  Permanent DoorNo to PhoneNo
                //pedoor = txtdoor1.Text;
                //peStreet = txtstreet2.Text;
                //pelmark = txtlmark3.Text;
                //peArea = txtarea4.Text;
                //peDistrict = txtPDist.Text;
                //pePincode = txtpin7.Text;

                //PermanentAddress=txtPermanentAddress.Text;
                peState = DdlStates.SelectedValue;


                peCity = ddlcity.SelectedValue;


                pephone = txtmobile9.Text;
                periodofstay = txtPeriodofStay.Text;
                if (txtResidingDate.Text.Trim().Length != 0)
                {
                    ResidingDate = Timings.Instance.CheckDateFormat(txtResidingDate.Text);

                }

                petown = txtpevillage.Text;
                pepolicestation = txtpePoliceStattion.Text;
                petaluka = txtpeTaluka.Text;
                pelmark = txtpeLandmark.Text;
                pePostOffice = txtpePostOffice.Text;
                pePincode = txtpePin.Text;

                #endregion  End  22 to 30  Permanent DoorNo to PhoneNo

                #endregion  End  Assign Variables  as on  [01-10-2013]

                #region  Begin  Assign Variables  as on  [02-10-2013]

                #region Begin 1-5 Bank Name to Branch Code

                EmpBankAcNo = txtBankAccNum.Text;
                Empbankbranchname = txtbranchname.Text;
                EmpIFSCcode = txtIFSCcode.Text;
                EmpBranchCode = txtBranchCode.Text;
                if (ddlbankname.SelectedIndex == 0)
                {
                    Empbankname = "0";
                }
                else
                {
                    Empbankname = ddlbankname.SelectedValue;
                }

                #endregion End 1-5 Bank Name to Branch Code

                #region Begin 6-10  Bank CodeNo to Branch Card Reference

                EmpBankCode = txtBankCodenum.Text;
                EmpBankAppNo = txtBankAppNum.Text;
                EmpRegionCode = txtRegCode.Text;
                EmpInsNominee = txtEmpInsNominee.Text;
                EmpBankCardRef = txtBankCardRef.Text;


                #endregion End 6-10  Bank CodeNo to Branch Card Reference

                #region Begin 11-15  Nominee Date of Borth to SS No.

                if (txtNomDoB.Text.Trim().Length != 0)
                {
                    // EmpNomineeDtofBirth = DateTime.Parse(txtNomDoB.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                    EmpNomineeDtofBirth = Timings.Instance.CheckDateFormat(txtNomDoB.Text);

                }

                EmpNomineeRel = txtEmpNomRel.Text;
                EmpInsCover = txtInsCover.Text;
                EmpInsDedAmt = txtInsDeb.Text;
                EmpUANNumber = txtSSNumber.Text;

                #endregion End 11-15  Nominee Date of Borth to SS No.

                #region Begin 16-20  EPFNo to EF Nominee Relation

                EmpEpfNo = txtEmpPFNumber.Text;
                EmpNominee = txtPFNominee.Text;

                if (txtPFEnrollDate.Text.Trim().Length != 0)
                {
                    //EmpPFEnrolDt = DateTime.Parse(txtPFEnrollDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                    EmpPFEnrolDt = Timings.Instance.CheckDateFormat(txtPFEnrollDate.Text);
                }
                CmpShortName = txtCmpShortName.Text;
                EmpRelation = txtPFNomineeRel.Text;

                #endregion end 16-20  EPFNo to EF Nominee Relation

                #region Begin 21-25  ESINo to ESI Nominee Relation

                EmpESINo = txtESINum.Text;
                EmpESINominee = txtESINominee.Text;
                EmpESIDispName = txtESIDiSName.Text;
                aadhaarid = txtaadhaar.Text;
                EmpESIRelation = txtESINomRel.Text;

                #endregion End 21-25  ESINo to ESI Nominee Relation

                #endregion  End  Assign Variables  as on  [02-10-2013]


                #region Begin Asign variables as on [03-10-2013]

                //#region Begin [1-5] SSC(Name&address of School/clg to Percentagemarks)

                //sscschool = txtschool.Text;
                //sscbduniversity = txtbrd.Text;
                //sscstdyear = txtyear.Text;
                //sscpassfail = txtpsfi.Text;
                //sscmarks = txtpmarks.Text;

                //#endregion End [1-5] SSC(Name&address of School/clg to Percentagemarks)

                //#region Begin [6-10] Inter(Name&address of School/clg to Percentagemarks)

                //imschool = txtimschool.Text;
                //imbduniversity = txtimbrd.Text;
                //imstdyear = txtimyear.Text;
                //impassfail = txtimpsfi.Text;
                //immarks = txtimpmarks.Text;

                //#endregion End [6-10] Inter(Name&address of School/clg to Percentagemarks)

                //#region Begin [11-15] Degree(Name&address of School/clg to Percentagemarks)

                //dgschool = txtdgschool.Text;
                //dgbduniversity = txtdgbrd.Text;
                //dgstdyear = txtdgyear.Text;
                //dgpassfail = txtdgpsfi.Text;
                //dgmarks = txtdgpmarks.Text;

                //#endregion End [16-20] P.G(Name&address of School/clg to Percentagemarks)

                //#region Begin [16-20] P.G(Name&address of School/clg to Percentagemarks)

                //pgschool = txtpgschool.Text;
                //pgbduniversity = txtpgbrd.Text;
                //pgstdyear = txtpgyear.Text;
                //pgpassfail = txtpgpsfi.Text;
                //pgmarks = txtpgpmarks.Text;

                //#endregion End [16-20] P.G(Name&address of School/clg to Percentagemarks)

                #endregion End Asign variables as on [03-10-2013].

                #region Begin salary tab

                AddlAmount = txtaddlamt.Text;
                FoodAllowance = txtfoodallowance.Text;

                #endregion Begin salary tab

                #region  Begin PVC Address Details
                //prDoorno = txtPrdoor.Text;
                //prStreet = txtstreet.Text;
                //prLmark = txtlmark.Text;
                //prArea = txtarea.Text;
                //prDistrict = txtdistrictt.Text;
                //prPincode = txtpin.Text;
                //PresentAddress = txtPresentAddress.Text;
                pvcState = ddlpvcstate.SelectedValue;

                if (ddlpvccity.SelectedIndex > 0)
                {
                    pvcCity = ddlpvccity.SelectedValue;
                }
                else
                {
                    pvcCity = "0";
                }
                pvcphone = txtpvcphone.Text;
                pvcperiodofstay = txtpvcstay.Text;
                pvcpolicestation = txtpvcpolicestation.Text;
                pvctaluka = txtpvctaluka.Text;
                pvctown = txtpvcvillage.Text;
                pvcPincode = txtpvcpin.Text;
                pvcLmark = txtpvclandmark.Text;
                pvcPostOffice = txtpvcpostofc.Text;

                if (txtpvcresidedate.Text.Trim().Length != 0)
                {
                    pvcResidingDate = Timings.Instance.CheckDateFormat(txtpvcresidedate.Text);

                }

                //EmpFamilyDetails = txtFamDetails.Text;

                #endregion  End PVC Address Details
                RejoinEmpid = txtRejoinEmpid.Text;

                if (RejoinEmpid.Length > 0)
                {
                    string qry = "update empdetails set Rejoinstatus=1 where empid='" + RejoinEmpid + "'";
                    int updstatus = config.ExecuteNonQueryWithQueryAsync(qry).Result;
                }


                #region  Begin Stored Procedure Parameters  as  on [18-09-2013]

                #region  Begin Code Procedure -2 For Stored Procedure Parameters as on [18-09-2013]

                #region  Begin Code For Connection

                config.command.CommandType = System.Data.CommandType.StoredProcedure;
                config.command.CommandText = "AddEmpDetails";

                #endregion End Code For Connection

                #region Begin Parameters  From Empid To Gender

                config.command.Parameters.AddWithValue("@empstatus", Empstatus);
                config.command.Parameters.AddWithValue("@empid", Empid);
                config.command.Parameters.AddWithValue("@Title", Title);
                config.command.Parameters.AddWithValue("@EmpFName", EmpFName);
                config.command.Parameters.AddWithValue("@EmpMName", EmpMName);
                config.command.Parameters.AddWithValue("@EmpLName", EmpLName);
                config.command.Parameters.AddWithValue("@EmpSex", EmpSex);
                config.command.Parameters.AddWithValue("@Gross", Gross);

                #endregion End  Parameters  From Empid To Gender

                #region Begin Parameters  Marital Status To Date Of Leaving

                config.command.Parameters.AddWithValue("@EmpMaritalStatus", EmpMaritalStatus);
                config.command.Parameters.AddWithValue("@EmpaddresStatus", EmpaddresStatus);
                config.command.Parameters.AddWithValue("@EmpDtofBirth", EmpDtofBirth);
                config.command.Parameters.AddWithValue("@EmpQualification", EmpQualification);
                config.command.Parameters.AddWithValue("@EmpDesgn", EmpDesgn);
                config.command.Parameters.AddWithValue("@EmpFatherName", EmpFatherName);
                config.command.Parameters.AddWithValue("@EmpFatherOccupation", EmpFatherOccupation);
                config.command.Parameters.AddWithValue("@EmpSpouseName", EmpSpouseName);
                //config.command.Parameters.AddWithValue("@EmpOldEmployeeId", EmpOldEmployeeId);
                config.command.Parameters.AddWithValue("@EmpMotherName", MotherName);
                config.command.Parameters.AddWithValue("@EmpDtofInterview", EmpDtofInterview);
                config.command.Parameters.AddWithValue("@EmpDtofJoining", EmpDtofJoining);
                config.command.Parameters.AddWithValue("@EmpDtofLeaving", EmpDtofLeaving);
                config.command.Parameters.AddWithValue("@EmpPreviousExp", EmpPreviousExp);
                config.command.Parameters.AddWithValue("@EmpPhone", EmpPhone);


                #endregion End  Parameters  Marital Status To Date Of Leaving

                #region Begin Parameters 21 to 25   Nationality/Religion  to PT-Deduct

                config.command.Parameters.AddWithValue("@Nationality", Nationality);
                config.command.Parameters.AddWithValue("@Religion", Religion);
                config.command.Parameters.AddWithValue("@EmpESIDeduct", EmpESIDeduct);
                config.command.Parameters.AddWithValue("@EmpPFDeduct", EmpPFDeduct);
                config.command.Parameters.AddWithValue("@EmpExservice", EmpExservice);
                config.command.Parameters.AddWithValue("@EmpPTDeduct", EmpPTDeduct);

                #endregion End  Parameters 21 to 25   Nationality/Religion  to PT-Deduct

                #region  Begin  Parameters  26 to 28  Languages  Known to Division
                config.command.Parameters.AddWithValue("@EmpLanguagesKnown", EmpLanguagesKnown);
                config.command.Parameters.AddWithValue("@MotherTongue", MotherTongue);
                config.command.Parameters.AddWithValue("@community", community);
                config.command.Parameters.AddWithValue("@EMPidPrefix", EmpIDPrefix);
                config.command.Parameters.AddWithValue("@Preferred_UnitId", PreferedUnit);
                config.command.Parameters.AddWithValue("@PsaraEmpCode", PsaraEmpCode);
                config.command.Parameters.AddWithValue("@emailid", Email);
                config.command.Parameters.AddWithValue("@IDCardIssued", IDCardIssued);
                config.command.Parameters.AddWithValue("@IDCardValid", IDCardValid);
                config.command.Parameters.AddWithValue("@Division", Division);
                config.command.Parameters.AddWithValue("@Department", Department);
                config.command.Parameters.AddWithValue("@Branch", Branch);
                config.command.Parameters.AddWithValue("@ReportingManager", ReportingManager);


                #endregion Begin  Parameters  26 to 28  Languages  Known to Division



                #region  Begin  1 to 7    RefName&Address1 to Identification Marks2
                config.command.Parameters.AddWithValue("@emprefaddr1", EmpRefAddr1);
                config.command.Parameters.AddWithValue("@emprefaddr2", EmpRefAddr2);
                config.command.Parameters.AddWithValue("@empbloodgroup", EmpBloodGroup);
                config.command.Parameters.AddWithValue("@empphysicalremarks", EmpPhysicalRemarks);
                config.command.Parameters.AddWithValue("@empremarks", EmpRemarks);
                config.command.Parameters.AddWithValue("@empidmark1", EmpIdMark1);
                config.command.Parameters.AddWithValue("@empidmark2", EmpIdMark2);

                #endregion  End  1 to 7    RefName&Address1 to Identification Marks2

                #region  Begin  8 to 11   Height to Expand

                config.command.Parameters.AddWithValue("@empheight", EmpHeight);
                config.command.Parameters.AddWithValue("@empweight", EmpWeight);
                config.command.Parameters.AddWithValue("@empchestex", EmpChestExp);
                config.command.Parameters.AddWithValue("@empchestunex", EmpChestunex);
                config.command.Parameters.AddWithValue("@EmpHaircolor", Haircolor);
                config.command.Parameters.AddWithValue("@EmpEyecolor", EyesColor);

                #endregion End  8 to 11   Height to Expand

                #region  Begin  12 to 21 Present  DoorNo to Family Details

                config.command.Parameters.AddWithValue("@PresentAddress", PresentAddress);
                config.command.Parameters.AddWithValue("@prcity", prCity);
                config.command.Parameters.AddWithValue("@prstate", prState);
                config.command.Parameters.AddWithValue("@prphone", prphone);
                config.command.Parameters.AddWithValue("@prResidingDate", prResidingDate);
                config.command.Parameters.AddWithValue("@prperiodofstay", prperiodofstay);
                config.command.Parameters.AddWithValue("@prtaluka", prtaluka);
                config.command.Parameters.AddWithValue("@prtown", prtown);
                config.command.Parameters.AddWithValue("@prpolicestation", prpolicestation);
                config.command.Parameters.AddWithValue("@prLmark", prLmark);
                config.command.Parameters.AddWithValue("@prPostOffice", prPostOffice);
                config.command.Parameters.AddWithValue("@prPincode", prPincode);

                #endregion  End   12 to 21 Present  DoorNo to Family Details

                #region  Begin  22 to 31  Permanent DoorNo to ExactEmpId

                config.command.Parameters.AddWithValue("@PermanentAddress", PermanentAddress);
                config.command.Parameters.AddWithValue("@pecity", peCity);
                config.command.Parameters.AddWithValue("@pestate", peState);
                config.command.Parameters.AddWithValue("@pephone", pephone);
                config.command.Parameters.AddWithValue("@periodofstay", periodofstay);
                config.command.Parameters.AddWithValue("@ResidingDate", ResidingDate);
                config.command.Parameters.AddWithValue("@petaluka", petaluka);
                config.command.Parameters.AddWithValue("@petown", petown);
                config.command.Parameters.AddWithValue("@pepolicestation", pepolicestation);
                config.command.Parameters.AddWithValue("@pelmark", pelmark);
                config.command.Parameters.AddWithValue("@pePostOffice", pePostOffice);
                config.command.Parameters.AddWithValue("@pePincode", pePincode);


                #endregion  End  22 to 31  Permanent DoorNo to ExactEmpId

                #region End 1-5 Bank Name to Branch Code

                config.command.Parameters.AddWithValue("@empbankname", Empbankname);
                config.command.Parameters.AddWithValue("@empbankacno", EmpBankAcNo);
                config.command.Parameters.AddWithValue("@Empbankbranchname", Empbankbranchname); //dought
                config.command.Parameters.AddWithValue("@empifsccode", EmpIFSCcode);
                config.command.Parameters.AddWithValue("@empbranchcode", EmpBranchCode);

                #endregion End 1-5 Bank Name to Branch Code

                #region End 6-10  Bank CodeNo to Branch Card Reference
                config.command.Parameters.AddWithValue("@empbankcode", EmpBankCode);
                config.command.Parameters.AddWithValue("@empbankappno", EmpBankAppNo);
                config.command.Parameters.AddWithValue("@empregioncode", EmpRegionCode);
                config.command.Parameters.AddWithValue("@empinsnominee", EmpInsNominee);
                config.command.Parameters.AddWithValue("@empbankcardref", EmpBankCardRef);

                #endregion End 6-10  Bank CodeNo to Branch Card Reference

                #region Begin 11-15  Nominee Date of Borth to SS No.

                config.command.Parameters.AddWithValue("@empnomineedtofbirth", EmpNomineeDtofBirth);
                config.command.Parameters.AddWithValue("@empnomineerel", EmpNomineeRel);
                config.command.Parameters.AddWithValue("@empinscover", EmpInsCover);
                config.command.Parameters.AddWithValue("@empinsdedamt", EmpInsDedAmt);
                config.command.Parameters.AddWithValue("@empUANnumber", EmpUANNumber);

                #endregion Begin 11-15  Nominee Date of Borth to SS No.

                #region Begin 16-20  EPFNo to EF Nominee Relation

                config.command.Parameters.AddWithValue("@empepfno", EmpEpfNo);
                config.command.Parameters.AddWithValue("@empnominee", EmpNominee);
                config.command.Parameters.AddWithValue("@emppfenroldt", EmpPFEnrolDt);
                config.command.Parameters.AddWithValue("@cmpshortname", CmpShortName);
                config.command.Parameters.AddWithValue("@emprelation", EmpRelation);

                #endregion Begin 16-20  EPFNo to EF Nominee Relation

                #region Begin 21-26  ESINo to EmpId

                config.command.Parameters.AddWithValue("@empesino", EmpESINo);
                config.command.Parameters.AddWithValue("@empesinominee", EmpESINominee);
                config.command.Parameters.AddWithValue("@empesidispname", EmpESIDispName);
                config.command.Parameters.AddWithValue("@aadhaarid", aadhaarid);
                config.command.Parameters.AddWithValue("@empesirelation", EmpESIRelation);
                #endregion

                #region Begin [1-5] SSC(Name&address of School/clg to Percentagemarks)

                //config.command.Parameters.AddWithValue("@sscschool", sscschool);
                //config.command.Parameters.AddWithValue("@sscbduniversity", sscbduniversity);
                //config.command.Parameters.AddWithValue("@sscstdyear", sscstdyear);
                //config.command.Parameters.AddWithValue("@sscpassfail", sscpassfail);
                //config.command.Parameters.AddWithValue("@sscmarks", sscmarks);

                #endregion End [1-5] SSC(Name&address of School/clg to Percentagemarks)

                #region Begin [6-10] Inter(Name&address of School/clg to Percentagemarks)

                //config.command.Parameters.AddWithValue("@imschool", imschool);
                //config.command.Parameters.AddWithValue("@imbduniversity", imbduniversity);
                //config.command.Parameters.AddWithValue("@imstdyear", imstdyear);
                //config.command.Parameters.AddWithValue("@impassfail", impassfail);
                //config.command.Parameters.AddWithValue("@immarks", immarks);

                #endregion End [6-10] Inter(Name&address of School/clg to Percentagemarks)

                #region Begin [11-15] Degree(Name&address of School/clg to Percentagemarks)

                //config.command.Parameters.AddWithValue("@dgschool", dgschool);
                //config.command.Parameters.AddWithValue("@dgbduniversity", dgbduniversity);
                //config.command.Parameters.AddWithValue("@dgstdyear", dgstdyear);
                //config.command.Parameters.AddWithValue("@dgpassfail", dgpassfail);
                //config.command.Parameters.AddWithValue("@dgmarks", dgmarks);

                #endregion End [11-15] Degree(Name&address of School/clg to Percentagemarks)

                #region End [11-15] PG(Name&address of School/clg to EmpId)

                //config.command.Parameters.AddWithValue("@pgschool", pgschool);
                //config.command.Parameters.AddWithValue("@pgbduniversity", pgbduniversity);
                //config.command.Parameters.AddWithValue("@pgstdyear", pgstdyear);
                //config.command.Parameters.AddWithValue("@pgpassfail", pgpassfail);
                //config.command.Parameters.AddWithValue("@pgmarks", pgmarks);


                #endregion Begin [11-15] PG(Name&address of School/clg to EmpId)

                #region begin code for PVC Address               

                config.command.Parameters.AddWithValue("@pvcCity", pvcCity);
                config.command.Parameters.AddWithValue("@pvcState", pvcState);
                config.command.Parameters.AddWithValue("@pvcphone", pvcphone);
                config.command.Parameters.AddWithValue("@pvcResidingDate", pvcResidingDate);
                config.command.Parameters.AddWithValue("@pvcperiodofstay", pvcperiodofstay);
                config.command.Parameters.AddWithValue("@pvcTaluka", pvctaluka);
                config.command.Parameters.AddWithValue("@pvcTown", pvctown);
                config.command.Parameters.AddWithValue("@pvcPoliceStation", pvcpolicestation);
                config.command.Parameters.AddWithValue("@pvcLmark", pvcLmark);
                config.command.Parameters.AddWithValue("@pvcPostoffice", pvcPostOffice);
                config.command.Parameters.AddWithValue("@pvcPincode", pvcPincode);

                #endregion for PVC Address
                #endregion Begin 21-26  ESINo to EmpId
                #endregion  Begin Code Procedure -2 For Stored Procedure Parameters as on [18-09-2013]




                #region Begin Variable Declaration as on [03-10-2013]

                #region Begin 1-5 ServiceNo to Crops

                var ServiceNo = string.Empty;
                var Rank = string.Empty;
                var DtofEnrolment = string.Empty;
                var DtofDischarge = string.Empty;
                var Crops = string.Empty;

                #endregion End 1-5 ServiceNo to Crops

                #region Begin 6-9 Trade to Conduct
                var Trade = string.Empty;
                var MedicalCategoryBloodGroup = string.Empty;
                var ReasonsofDischarge = string.Empty;
                var Conduct = string.Empty;

                #endregion End 6-9 Trade to Conduct

                #region   Begin Extra Varibles for This Event   As on [03-10-2013]

                #endregion End Extra Varibles for This Event   As on [03-10-2013]

                #endregion End Variable Declaration as on  [03-10-2013]

                #region Begin Assign Variables  as on [03-10-2013]

                #region Begin 1-5 ServiceNo to Crops
                ServiceNo = txtServiceNum.Text;
                Rank = txtRank.Text;
                DtofDischarge = "01/01/1900";
                DtofEnrolment = "01/01/1900";
                if (txtDOfEnroll.Text.Trim().Length != 0)
                {
                    DtofEnrolment = DateTime.Parse(txtDOfEnroll.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                }

                if (txtDofDischarge.Text.Trim().Length != 0)
                {
                    DtofDischarge = DateTime.Parse(txtDofDischarge.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                }

                Crops = txtCrops.Text;

                #endregion End 1-5 ServiceNo to Crops

                #region Begin 6-9 Trade to Conduct

                Trade = txtTrade.Text;
                MedicalCategoryBloodGroup = txtMCategory.Text;
                ReasonsofDischarge = TxtROfDischarge.Text;
                Conduct = txtConduct.Text;

                #endregion End 6-9 Trade to Conduct

                #endregion End Assign Variables  as on [03-10-2013]

                #region  Begin Stored Procedure Parameters   as  on [02-10-2013]

                //Hashtable ExService = new Hashtable();
                //string ExServiceName = "EmployeeTabExService";

                #region Begin 1-5 ServiceNo to Crops

                config.command.Parameters.AddWithValue("@serviceno", ServiceNo);
                config.command.Parameters.AddWithValue("@rank", Rank);
                config.command.Parameters.AddWithValue("@dtofenroment", DtofEnrolment);
                config.command.Parameters.AddWithValue("@daofdischarge", DtofDischarge);
                config.command.Parameters.AddWithValue("@crops", Crops);

                #endregion End 1-5 ServiceNo to Crops

                #region Begin 6-10 Trade to empid

                config.command.Parameters.AddWithValue("@trade", Trade);
                config.command.Parameters.AddWithValue("@medicalcategorybloodgroup", MedicalCategoryBloodGroup);
                config.command.Parameters.AddWithValue("@reasonsofdischarge", ReasonsofDischarge);
                config.command.Parameters.AddWithValue("@conduct", Conduct);
                //  config.command.Parameters.AddWithValue("@empid", Session["ExactEmpid"]);
                config.command.Parameters.AddWithValue("@AddlAmount", AddlAmount);
                config.command.Parameters.AddWithValue("@FoodAllowance", FoodAllowance);


                #endregion End 6-10 Trade to empid


                #region for birth Details,other fields on 10-12-2015

                var BirthDistrict = "";
                var BirthCountry = "";
                var BirthVillage = "";
                var BirthState = "0";
                var ApplicantCategory = "";
                var SpeciallyAbled = 0;


                // BirthDistrict= ddlBirthDistrict.SelectedValue;
                // BirthCountry= txtBirthCountry.Text;
                // BirthVillage= txtBirthVillage.Text;
                // BirthState= ddlbirthstate.SelectedValue;
                ApplicantCategory = ddlAppCategory.SelectedValue;
                if (ChkSpeciallyAbled.Checked)
                    SpeciallyAbled = 1;


                #endregion for birth Details


                #region for Proof Details,other fields on 10-12-2015

                var AadharCard = "N";
                var AadharCardNo = "";
                var AadharCardName = "";
                var drivingLicense = "N";
                var drivingLicenseNo = "";
                var drivingLicenseName = "";
                var VoterID = "N";
                var VoterIDNo = "";
                var VoterIDName = "";
                var RationCard = "N";
                var RationCardNo = "";
                var RationCardName = "";
                var PanCard = "N";
                var PanCardNo = "";
                var PanCardName = "";
                var BankPassbook = "N";
                var BankPassbookNo = "";
                var BankPassbookName = "";
                var ElectricityBill = "N";
                var ElectricityBillNo = "";
                var ElectricityBillName = "";
                var Other = "N";
                var Othertext = "";
                var Othertextname = "";
                var ESICName = "";
                var ESICCardNo = "";
                var ESICCard = "N";

                if (ChkAadharCard.Checked)
                    AadharCard = "Y";
                AadharCardNo = txtAadharCard.Text;
                AadharCardName = txtAadharName.Text;

                if (ChkdrivingLicense.Checked)
                    drivingLicense = "Y";
                drivingLicenseNo = txtDrivingLicense.Text;
                drivingLicenseName = txtDrivingLicenseName.Text;

                if (ChkVoterID.Checked)
                    VoterID = "Y";
                VoterIDNo = txtVoterID.Text;
                VoterIDName = txtVoterName.Text;

                if (ChkRationCard.Checked)
                    RationCard = "Y";
                RationCardNo = txtRationCard.Text;
                RationCardName = txtRationCardName.Text;

                if (ChkPanCard.Checked)
                    PanCard = "Y";
                PanCardNo = txtPanCard.Text;
                PanCardName = txtPanCardName.Text;

                if (ChkBankPassbook.Checked)
                    BankPassbook = "Y";
                BankPassbookNo = txtBankPassbook.Text;
                BankPassbookName = txtBankPassBookName.Text;


                if (ChkElectricityBill.Checked)
                    ElectricityBill = "Y";
                ElectricityBillNo = txtElectricityBill.Text;
                ElectricityBillName = txtElecBillname.Text;

                if (ChkESICCard.Checked)
                    ESICCard = "Y";
                ESICCardNo = txtESICCardNo.Text;
                ESICName = txtESICName.Text;


                if (Chkother.Checked)
                    Other = "Y";
                Othertext = txtOther.Text;
                Othertextname = txtOtherName.Text;

                #endregion for Proof Details,other fields on 10-12-2015

                #region for policerecord on 11-12-2015

                var CriminalOffCName = "";
                var CriminalOffcaseNo = "";
                var CriminalOff = "";
                var CriminalProCName = "";
                var CriminalProCaseNo = "";
                var CriminalProOffence = "";
                var CriminalArrestCName = "";
                var CriminalArrestCaseNo = "";
                var CriminalArrestOffence = "";
                var PoliceVerCode = "";
                var PoliceverificationCheck = "N";
                var CriminalOffCheck = "N";
                var CriminalProCheck = "N";
                var CriminalArrestCheck = "N";
                var NearestPoliceStation = "";


                if (rdbVerified.Checked == true)
                {
                    PoliceverificationCheck = "Y";
                }

                if (ChkCrimalArrest.Checked == true)
                {
                    CriminalArrestCheck = "Y";
                }

                if (ChkCriminalOff.Checked == true)
                {
                    CriminalOffCheck = "Y";
                }

                if (ChkCriminalProc.Checked == true)
                {
                    CriminalProCheck = "Y";
                }
                CriminalOffCName = txtCriminalOffCName.Text;
                CriminalOffcaseNo = txtCriminalOffcaseNo.Text;
                CriminalOff = txtCriminalOff.Text;
                CriminalProCName = txtCriminalProCName.Text;
                CriminalProCaseNo = txtCriminalProCaseNo.Text;
                CriminalProOffence = txtCriminalProOffence.Text;
                CriminalArrestCName = txtCriminalArrestCName.Text;
                CriminalArrestCaseNo = txtCriminalArrestCaseNo.Text;
                CriminalArrestOffence = txtCriminalArrestOffence.Text;
                PoliceVerCode = txtPoliceVerificationNo.Text;
                NearestPoliceStation = txtPoliceStation.Text;
                #endregion for policerecord on 11-12-2015

                //string str1 = TextBox1.Text;

                string Str = "";


                if (FileUploadImage.HasFile)
                {
                    Str = txtEmID.Text + "Photo.jpg";
                    string filename = System.IO.Path.GetFileName(FileUploadImage.FileName);
                    path = Path.GetFileName(FileUploadImage.PostedFile.FileName);
                    FileUploadImage.PostedFile.SaveAs(Server.MapPath("~/assets/EmpPhotos/") + Str);

                }
                else
                {
                    Str = "";

                }



                string StrSign = "";
                if (FileUploadSign.HasFile)
                {
                    StrSign = txtEmID.Text + "Sign.jpg";
                    path = Path.GetFileName(FileUploadSign.PostedFile.FileName);
                    FileUploadSign.PostedFile.SaveAs(Server.MapPath("~/assets/EmpSign/") + StrSign);

                }
                else
                {
                    StrSign = "";

                }

                var EmployeeType = "G";

                if (rdbStaff.Checked == true)
                {
                    EmployeeType = "S";
                }



                config.command.Parameters.AddWithValue("@Image", Str);
                config.command.Parameters.AddWithValue("@EmpSign", StrSign);
                config.command.Parameters.AddWithValue("@BirthDistrict", BirthDistrict);
                config.command.Parameters.AddWithValue("@BirthCountry", BirthCountry);
                config.command.Parameters.AddWithValue("@BirthVillage", BirthVillage);
                config.command.Parameters.AddWithValue("@BirthState", BirthState);
                config.command.Parameters.AddWithValue("@ApplicantCategory", ApplicantCategory);
                config.command.Parameters.AddWithValue("@SpeciallyAbled", SpeciallyAbled);
                config.command.Parameters.AddWithValue("@AadharCard ", AadharCard);
                config.command.Parameters.AddWithValue("@AadharCardNo", AadharCardNo);
                config.command.Parameters.AddWithValue("@AadharCardName", AadharCardName);
                config.command.Parameters.AddWithValue("@DrivingLicense ", drivingLicense);
                config.command.Parameters.AddWithValue("@DrivingLicenseNo", drivingLicenseNo);
                config.command.Parameters.AddWithValue("@drivingLicenseName", drivingLicenseName);
                config.command.Parameters.AddWithValue("@VoterID ", VoterID);
                config.command.Parameters.AddWithValue("@VoterIDNo ", VoterIDNo);
                config.command.Parameters.AddWithValue("@VoterIDName ", VoterIDName);
                config.command.Parameters.AddWithValue("@RationCard ", RationCard);
                config.command.Parameters.AddWithValue("@RationCardNo ", RationCardNo);
                config.command.Parameters.AddWithValue("@RationCardName", RationCardName);
                config.command.Parameters.AddWithValue("@PanCard ", PanCard);
                config.command.Parameters.AddWithValue("@PanCardNo ", PanCardNo);
                config.command.Parameters.AddWithValue("@PanCardName ", PanCardName);
                config.command.Parameters.AddWithValue("@PassBook ", BankPassbook);
                config.command.Parameters.AddWithValue("@PassBookNo ", BankPassbookNo);
                config.command.Parameters.AddWithValue("@BankPassbookName ", BankPassbookName);
                config.command.Parameters.AddWithValue("@ElectricityBill ", ElectricityBill);
                config.command.Parameters.AddWithValue("@ElectricityBillNo ", ElectricityBillNo);
                config.command.Parameters.AddWithValue("@ElectricityBillName ", ElectricityBillName);
                config.command.Parameters.AddWithValue("@Others ", Other);
                config.command.Parameters.AddWithValue("@OtherType ", Othertext);
                config.command.Parameters.AddWithValue("@Othertextname ", Othertextname);
                config.command.Parameters.AddWithValue("@ESICCard ", ESICCard);
                config.command.Parameters.AddWithValue("@ESICCardNo ", ESICCardNo);
                config.command.Parameters.AddWithValue("@ESICName ", ESICName);
                config.command.Parameters.AddWithValue("@CriminalOffCName", CriminalOffCName);
                config.command.Parameters.AddWithValue("@CriminalOffcaseNo", CriminalOffcaseNo);
                config.command.Parameters.AddWithValue("@CriminalOff", CriminalOff);
                config.command.Parameters.AddWithValue("@CriminalProCName", CriminalProCName);
                config.command.Parameters.AddWithValue("@CriminalProCaseNo", CriminalProCaseNo);
                config.command.Parameters.AddWithValue("@CriminalProOffence", CriminalProOffence);
                config.command.Parameters.AddWithValue("@CriminalArrestCName", CriminalArrestCName);
                config.command.Parameters.AddWithValue("@CriminalArrestCaseNo", CriminalArrestCaseNo);
                config.command.Parameters.AddWithValue("@CriminalArrestOffence", CriminalArrestOffence);
                config.command.Parameters.AddWithValue("@PoliceVerificationNo", PoliceVerCode);
                config.command.Parameters.AddWithValue("@CriminalProCheck", CriminalProCheck);
                config.command.Parameters.AddWithValue("@CriminalArrestCheck", CriminalArrestCheck);
                config.command.Parameters.AddWithValue("@CriminalOffCheck", CriminalOffCheck);
                config.command.Parameters.AddWithValue("@PoliceverificationCheck", PoliceverificationCheck);
                config.command.Parameters.AddWithValue("@NearestPoliceStation", NearestPoliceStation);
                config.command.Parameters.AddWithValue("@EmployeeType", EmployeeType);
                config.command.Parameters.AddWithValue("@RejoinEmpid", RejoinEmpid);
                config.command.Parameters.AddWithValue("@RejoinStatus", RejoinStatus);


                #endregion  End Stored Procedure Parameters   as  on [02-10-2013]

                SqlParameter parm = new SqlParameter("@ExactEmpid", SqlDbType.NVarChar, 50);
                parm.Direction = ParameterDirection.Output; // This is important!
                config.command.Parameters.Add(parm);



                #region  Begin Code For Calling Stored Procedure [19-09-2013]

                int status = config.ExecuteNonQueryAsync().Result;
                string ExactEmpid = (string)config.command.Parameters["@ExactEmpid"].Value;

                #endregion End Code For Calling Stored Procedure [19-09-2013]


                #region   Begin Code For Resulted Messages as on [19-09-2013]
                if (status > 0)
                {
                    updateAndroidStatus();
                    addempfamilydetails();
                    addempeducationdetails();
                    addempPreviousExperience();
                    lblSuc.Text = "Employee Details Added Sucessfully With ID NO  :- " + ExactEmpid + "";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Employee Details Added Sucessfully With ID NO  :- " + ExactEmpid + "  -:');", true);
                    Session["CheckStatusRecordInserted"] = 1;
                    Session["ExactEmpid"] = ExactEmpid;
                    ClearDataFromPersonalInfoTabFields();

                    rdbGeneral.Checked = true;
                    rdbmanual.Checked = false;
                    rdbStaff.Checked = false;
                    employeeid();
                }
                else
                {
                    lblMsg.Text = "Employee Details Not Added Sucessfully With ID NO  :- " + ExactEmpid + "  , Please Check The Details.";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Employee Details Not Added Sucessfully With ID NO  :- " + ExactEmpid + "  -:  , Please Check The Details.');", true);
                    Session["CheckStatusRecordInserted"] = 0;
                    Session["ExactEmpid"] = "0";
                }
                #endregion  End Code For Resulted Messages as on [19-09-2013]


                FameService fs = new FameService();
                fs.UpdateEmpDataTable();




            }

            catch (Exception ex)
            {

            }




        }
        public void updateAndroidStatus()
        {
            var Empid = string.Empty;
            Empid = txtEmID.Text;
            string updatequery = "update Empdetails_Android set EmpAndroidStatus=1,OriginalEmpid='" + Empid + "' where empid='" + ddlAndroidEmpid.SelectedValue + "' ";
            int result = config.ExecuteNonQueryWithQueryAsync(updatequery).Result;

            updatequery = "update Empdetails set EmpAndroidStatus=1 where empid='" + txtEmID.Text + "' ";
            result = config.ExecuteNonQueryWithQueryAsync(updatequery).Result;
        }
        protected void Btn_Cancel_Personal_Tab_Click(object sender, EventArgs e) // Personal Tab Cancel Button Click
        {
            employeeid();
            ClearDataFromPersonalInfoTabFields();
            txtDOfEnroll.Text = txtDofDischarge.Text = "";
            txtServiceNum.Text = txtRank.Text = txtCrops.Text = txtTrade.Text = txtMCategory.Text = TxtROfDischarge.Text = txtConduct.Text = string.Empty;
        }

        public void ClearSection()
        {
            foreach (Control cntrl in PnlProofsSubmitted.Controls)
            {
                if (cntrl is TextBox)
                {
                    ((TextBox)cntrl).Text = "";
                }
                if (cntrl is CheckBox)
                {
                    ((CheckBox)cntrl).Checked = false;
                }

            }

        }

        protected void ClearDataFromProofTabfields()
        {
            //proofs


        }

        protected void ClearDataFromPersonalInfoTabFields()
        {

            //personal info
            //txtoldemployeeid.Text=
            rdbGeneral.Checked = true;
            txtEmID.ReadOnly = true;
            txtEmpFName.Text = txtEmpmiName.Text = txtEmplname.Text = txtEmpDtofBirth.Text = txtEmpDtofInterview.Text = txtEmpDtofJoining.Text = txtDofleaving.Text = txtMotherName.Text = txtFatherName.Text = txtfatheroccupation.Text = txtSpousName.Text = "";
            rdbactive.Checked = rdbmale.Checked = rdbmarried.Checked = ChkESIDed.Checked = ChkPFDed.Checked = ChkPTDed.Checked = rdur.Checked = rdbNotVerified.Checked = true;
            txtQualification.Text = txtPreEmp.Text = txtRejoinEmpid.Text =
            txtmtongue.Text = txtPhone.Text = txtnationality.Text = txtreligion.Text = txtGrossSalary.Text = txtLangKnown.Text = txtemail.Text = txtpsaraempcode.Text = TxtIDCardIssuedDt.Text = TxtIdCardValid.Text = txtGrossSalary.Text = string.Empty;
            rdbfemale.Checked = rdbTransgender.Checked = rdbsingle.Checked = rdbWidower.Checked = rdbdivorcee.Checked = false;
            ddlDesignation.SelectedIndex = DdlPreferedUnit.SelectedIndex = ddlReportingMgr.SelectedIndex = ddlTitle.SelectedIndex = 0;
            ChkExService.Checked = rdbStaff.Checked = rdbmanual.Checked = false;

            LoadStatenames();

            //references
            //txtBirthCountry.Text= txtBirthVillage.Text=
            txtREfAddr1.Text = txtREfAddr2.Text = txtPhyRem.Text = txtEmpRemarks.Text = txtImark1.Text = txtImark2.Text =
            txtheight.Text = txtweight.Text = txtcheexpan.Text = txtcheunexpan.Text = txtPhone.Text = txthaircolour.Text = txtEyeColour.Text =
            txtmobile.Text = txtmobile9.Text = string.Empty;
            txtprvillage.Text = txtprResidingDate.Text = txtprPoliceStation.Text = txtpePoliceStattion.Text = txtpeTaluka.Text = txtpevillage.Text = txtprtaluka.Text =
            txtprLandmark.Text = txtprPostOffice.Text = txtprpin.Text = txtpePostOffice.Text = txtpePin.Text = txtpeLandmark.Text = txtprPeriodofStay.Text = txtPeriodofStay.Text = txtResidingDate.Text = string.Empty;
            chkSame.Checked = ChkSpeciallyAbled.Checked = false;
            ddlBloodGroup.SelectedIndex = DdlStates.SelectedIndex = ddlpreStates.SelectedIndex = ddlAppCategory.SelectedIndex = 0;
            rdbnotrequired.Checked = true;
            rdbpreaddress.Checked = rdbperaddress.Checked = false;
            //ddlBirthDistrict.Items.Clear();
            ddlpreCity.Items.Clear();
            ddlcity.Items.Clear();

            //bank
            txtNomDoB.Text = txtPFEnrollDate.Text = "";
            txtBankAccNum.Text = txtbranchname.Text = txtIFSCcode.Text = txtBranchCode.Text = txtBankCodenum.Text = txtBankAppNum.Text = txtRegCode.Text =
            txtEmpInsNominee.Text = txtBankCardRef.Text = txtSSNumber.Text = txtInsDeb.Text = txtSSNumber.Text = txtEmpNomRel.Text =
            txtEmpPFNumber.Text = txtPFNominee.Text = txtCmpShortName.Text = txtPFNomineeRel.Text = txtESINum.Text = txtESINominee.Text =
            txtESIDiSName.Text = txtaadhaar.Text = txtESINomRel.Text = txtInsCover.Text = txtaddlamt.Text = txtfoodallowance.Text = string.Empty;
            ddlbankname.SelectedIndex = DdlStates.SelectedIndex = 0;


            Chkother.Checked = ChkAadharCard.Checked = ChkBankPassbook.Checked = ChkdrivingLicense.Checked = ChkPanCard.Checked = ChkVoterID.Checked = ChkElectricityBill.Checked = ChkESICCard.Checked = ChkRationCard.Checked = false;
            txtAadharCard.Text = txtPanCard.Text = txtPanCardName.Text = txtDrivingLicenseName.Text = txtDrivingLicense.Text = txtBankPassBookName.Text = txtBankPassbook.Text =
                txtVoterID.Text = txtVoterName.Text = txtElectricityBill.Text = txtRationCard.Text = txtRationCardName.Text = txtESICCardNo.Text = txtESICCardNo.Text = txtOther.Text =
                txtAadharName.Text = txtOtherName.Text = string.Empty;


            //service
            txtServiceNum.Text = txtRank.Text = txtCrops.Text = txtTrade.Text = txtMCategory.Text = TxtROfDischarge.Text = txtConduct.Text = string.Empty;
            txtDOfEnroll.Text = txtDofDischarge.Text = txthaircolour.Text = txtEyeColour.Text = txtpsaraempcode.Text = txtPoliceVerificationNo.Text = string.Empty;
            ChkCrimalArrest.Checked = ChkCriminalOff.Checked = ChkCrimalArrest.Checked = ChkPanCard.Checked = ChkRationCard.Checked =
            ChkExService.Checked = ChkCriminalProc.Checked = false;



            txtCriminalArrestCaseNo.Text = txtCriminalArrestCName.Text = txtCriminalArrestOffence.Text = txtCriminalOff.Text = txtCriminalOffcaseNo.Text = txtCriminalOffCName.Text =
            txtCriminalProCaseNo.Text = txtCriminalProCName.Text = txtCriminalProOffence.Text = txtPanCardName.Text = txtBankPassBookName.Text = txtElecBillname.Text = txtESICName.Text =
            txtRationCardName.Text = txtVoterName.Text = txtDrivingLicenseName.Text = txtAadharName.Text = string.Empty;
            txtpvclandmark.Text = txtpvcphone.Text = txtpvcpin.Text = txtpvcpolicestation.Text = txtpvcpostofc.Text = txtpvcresidedate.Text = txtpvcstay.Text = txtpvctaluka.Text =
                txtpvcvillage.Text = string.Empty;
            txtPoliceStation.Text = txtPoliceVerificationNo.Text = string.Empty;
            ddlpvcstate.SelectedIndex = 0; ddlpvccity.Items.Clear();
            ChkCrimalArrest.Checked = ChkCriminalOff.Checked = ChkCriminalProc.Checked = rdbVerified.Checked = false;

            disabledControls();

        }


        protected void ChkCriminalOff_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkCriminalOff.Checked == true)
            {
                txtCriminalOff.Enabled = true;
                txtCriminalOffcaseNo.Enabled = true;
                txtCriminalOffCName.Enabled = true;

            }
            else
            {
                txtCriminalOff.Enabled = false;
                txtCriminalOffcaseNo.Enabled = false;
                txtCriminalOffCName.Enabled = false;
            }
        }

        protected void ChkCrimalArrest_checkedChanged(object sender, EventArgs e)
        {
            if (ChkCrimalArrest.Checked == true)
            {
                txtCriminalArrestCaseNo.Enabled = true;
                txtCriminalArrestCName.Enabled = true;
                txtCriminalArrestOffence.Enabled = true;

            }
            else
            {
                txtCriminalArrestCaseNo.Enabled = false;
                txtCriminalArrestCName.Enabled = false;
                txtCriminalArrestOffence.Enabled = false;

            }
        }

        protected void ChkCriminalProc_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkCriminalProc.Checked == true)
            {
                txtCriminalProCaseNo.Enabled = true;
                txtCriminalProCName.Enabled = true;
                txtCriminalProOffence.Enabled = true;

            }
            else
            {
                txtCriminalProCaseNo.Enabled = false;
                txtCriminalProCName.Enabled = false;
                txtCriminalProOffence.Enabled = false;


            }
        }

        public void disabledControls()
        {
            txtESICCardNo.Enabled = false;
            txtOtherName.Enabled = false;
            txtAadharCard.Enabled = false;
            txtPanCard.Enabled = false;
            txtDrivingLicense.Enabled = false;
            txtBankPassbook.Enabled = false;
            txtVoterID.Enabled = false;
            txtElectricityBill.Enabled = false;
            txtRationCard.Enabled = false;
            txtOther.Enabled = false;
            txtCriminalArrestCaseNo.Enabled = false;
            txtCriminalArrestCName.Enabled = false;
            txtCriminalArrestOffence.Enabled = false;
            txtCriminalOff.Enabled = false;
            txtCriminalOffcaseNo.Enabled = false;
            txtCriminalOff.Enabled = false;
            txtCriminalProCaseNo.Enabled = false;
            txtCriminalProCName.Enabled = false;
            txtCriminalProOffence.Enabled = false;
            txtPanCardName.Enabled = false;
            txtBankPassBookName.Enabled = false;
            txtElecBillname.Enabled = false;
            txtESICName.Enabled = false;
            txtRationCardName.Enabled = false;
            txtVoterName.Enabled = false;
            txtDrivingLicenseName.Enabled = false;
            txtAadharName.Enabled = false;
        }
        /*(1) End Code For The Employee Personal Information  Tab  (SAVE/CANCEL/CLEAR DATA METHOD) As on [10-04-2014]*/


        /*(4)  Begin Code For the Employee IMAGES   Tab (SAVE/CANCEL/CLEAR DATA METHOD)   As on [10-04-2013]*/

        //protected void btnsaveimage_Click(object sender, EventArgs e)//images add save button
        //{
        //    lblimages.Visible = true;
        //    string selectquery = "select * from empdetails where empid = '" + txtEmID.Text + "'";
        //    DataTable dt = SqlHelper.Instance.GetTableByQuery(selectquery);
        //    if (int.Parse(Session["CheckStatusRecordInserted"].ToString()) != 0)
        //    {
        //        //Insert();
        //        chkSBond.Checked = ChkCertfSub.Checked = false;
        //        txtEmpCertfdeb.Text = string.Empty;
        //        imgESIPic.Src = imgRhImage.Src = imgLeft.Src = imglogo.Src = imgSignature.Src = "";
        //        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Record Added Successfully');", true);
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('First Fill The  Personfal Information Of the Employee !');", true);
        //    }
        //}

        //protected void Btn_Save_Image_Click(object sender, EventArgs e)//images add save button
        //{
        //    if (int.Parse(Session["CheckStatusRecordInserted"].ToString()) != 0)
        //    {
        //        #region  Begin Old code As on [16-10-2013]

        //        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ToString());

        //        //con.Open();

        //        //int CertfSubmit;
        //        //if (ChkCertfSub.Checked)
        //        //    CertfSubmit = 1;
        //        //else
        //        //    CertfSubmit = 0;
        //        //string certfDetails = txtEmpCertfdeb.Text;
        //        //int SuretyBond;
        //        //if (chkSBond.Checked)
        //        //    SuretyBond = 1;
        //        //else
        //        //    SuretyBond = 0;
        //        //int empid = int.Parse(txtEmID.Text.Trim());
        //        //string tempid = EmpIDPrefix + (empid - 1).ToString("000000");
        //        ////string SecurityDeposit = txtSecurityBond.Text;
        //        //SqlCommand cmd = new SqlCommand();

        //        //cmd.CommandText = "addEmpbiodata";
        //        //cmd.CommandType = CommandType.StoredProcedure;
        //        //cmd.Connection = con;

        //        //cmd.Parameters.Add("@pCertfSubmit", CertfSubmit);
        //        //cmd.Parameters.Add("@pcertfDetails", certfDetails);
        //        //cmd.Parameters.Add("@pSuretyBond", SuretyBond);

        //        //cmd.Parameters.Add("@plogo", imageBytes);
        //        //cmd.Parameters.Add("@pElogo", ESIimageBytes);
        //        //cmd.Parameters.Add("@pLlogo", LeftimageBytes);
        //        //cmd.Parameters.Add("@pRlogo", RightimageBytes);
        //        //cmd.Parameters.Add("@pSlogo", SignimageBytes);
        //        //cmd.Parameters.Add("@pEmpId",  tempid);
        //        //lblimages.Visible = true;
        //        //if (cmd.ExecuteNonQuery() != null)
        //        //{
        //        //    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Record Added Successfully');", true);
        //        //}
        //        //else
        //        //{
        //        //    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Record Not Added');", true);

        //        //}
        //        //con.Close();

        //        #endregion End  Old code As on [16-10-2013]

        //        #region Begin New Code as on [16-10-2013]

        //        #region  Begin Code Variable Declaration as on [16-10-2013]
        //        var CertfSubmit = 0;
        //        var certfDetails = "";
        //        var SuretyBond = 0;
        //        var SecurityDeposit = "";
        //        var StoredProcedureName = "";
        //        var IRecordStatus = 0;
        //        #endregion End  Code Variable Declaration as on [16-10-2013]

        //        #region  Begin Code Assign Values to  Variable   as on [16-10-2013]
        //        if (ChkCertfSub.Checked)
        //            CertfSubmit = 1;

        //        certfDetails = txtEmpCertfdeb.Text;

        //        if (chkSBond.Checked)
        //            SuretyBond = 1;

        //        StoredProcedureName = "addEmpbiodata";
        //        #endregion End  Code Assign Values to  Variable  as on [16-10-2013]

        //        #region  Begin Code For Stored Procedure Parameters   as on [16-10-2013]
        //        Hashtable HtEmployeeTabImages = new Hashtable();
        //        HtEmployeeTabImages.Add("@pCertfSubmit", CertfSubmit);
        //        HtEmployeeTabImages.Add("@pcertfDetails", certfDetails);
        //        HtEmployeeTabImages.Add("@pSuretyBond", SuretyBond);
        //        HtEmployeeTabImages.Add("@plogo", imageBytes);

        //        HtEmployeeTabImages.Add("@pElogo", ESIimageBytes);
        //        HtEmployeeTabImages.Add("@pLlogo", LeftimageBytes);
        //        HtEmployeeTabImages.Add("@pRlogo", RightimageBytes);
        //        HtEmployeeTabImages.Add("@pSlogo", SignimageBytes);
        //        HtEmployeeTabImages.Add("@pempid", Session["ExactEmpid"]);


        //        #endregion End  Code For  Stored Procedure Parameters  as on [16-10-2013]

        //        #region   Begin Code For Calling Stored Procedure as on [02-10-2013]
        //        IRecordStatus = SqlHelper.Instance.ExecuteQuery(StoredProcedureName, HtEmployeeTabImages);
        //        #endregion   End   Code For Calling Stored Procedure as on [02-10-2013]


        //        #region     Begin Code For Status/Resulted Message of the Inserted Record as on [02-10-2013]

        //        if (IRecordStatus > 0)
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Added Sucessfully   ');", true);
        //            ClearDataFromImagesTabFields();
        //            return;
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert(' Not  Added Sucessfully   ');", true);

        //            return;
        //        }
        //        #endregion  End Code For Status/Resulted Message of the Inserted Record as on [02-10-2013]


        //        #endregion Begin New Code as on [16-10-2013]

        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('First Fill The  Personfal Information Of the Employee !');", true);
        //    }
        //}

        //protected void Btn_Cancel_Image_Click(object sender, EventArgs e)//images add save button
        //{
        //    chkSBond.Checked = ChkCertfSub.Checked = false;
        //    txtEmpCertfdeb.Text = txtSecurityDeposit.Text = string.Empty;
        //    imgESIPic.Src = imgRhImage.Src = imgLeft.Src = imglogo.Src = imgSignature.Src = "";
        //}

        //protected void ClearDataFromImagesTabFields()
        //{
        //    chkSBond.Checked = ChkCertfSub.Checked = false;
        //    txtEmpCertfdeb.Text = txtSecurityDeposit.Text = string.Empty;
        //    imgESIPic.Src = imgRhImage.Src = imgLeft.Src = imglogo.Src = imgSignature.Src = "";
        //    imageBytes = ESIimageBytes = LeftimageBytes = RightimageBytes = SignimageBytes = null;

        //}

        /*(4) End Code For The Employee IMAGES  Tab  (SAVE/CANCEL/CLEAR DATA METHOD) As on [10-04-2014]*/

        //static string fileName;
        //static int fileLength;   //Emp
        //static byte[] imageBytes;
        //static string LeftfileName;  //Left
        //static int LeftfileLength;
        //static byte[] LeftimageBytes;
        //static string RightfileName; //Right
        //static int RightfileLength;
        //static byte[] RightimageBytes;
        //static string ESIfileName;
        //static int ESIfileLength;  //ESI
        //static byte[] ESIimageBytes;
        //static string SignfileName;
        //static int SignfileLength;  //Signature
        //static byte[] SignimageBytes;

        //public void SavePicture(String Fname, String img) //Emp Photo
        //{
        //    fileName = FcPicture1.PostedFile.FileName;
        //    fileLength = FcPicture1.PostedFile.ContentLength;
        //    imageBytes = new byte[fileLength];
        //    FcPicture1.PostedFile.InputStream.Read(imageBytes, 0, fileLength);
        //    if (img == "jpg" || img == "jpeg" || img == "png")
        //    {
        //        if (Fname != String.Empty)
        //        {
        //            string rootpath = Server.MapPath("Images");
        //            if (File.Exists(rootpath + "\\" + Fname))
        //            {
        //                File.Delete(rootpath + "\\" + Fname);
        //                FcPicture1.SaveAs(rootpath + "\\" + Fname);
        //                imglogo.Src = "Images/" + Fname;

        //            }
        //            else
        //            {
        //                FcPicture1.SaveAs(rootpath + "\\" + Fname);
        //                imglogo.Src = "Images/" + Fname;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        imglogo.Src = "";
        //        // lblError.Text = "Please Upload a Image in proper format";
        //    }
        //}

        //public void SaveLeftPicture(String Fname, String img)//Left
        //{
        //    LeftfileName = LeftFcPicture.PostedFile.FileName;
        //    LeftfileLength = LeftFcPicture.PostedFile.ContentLength;
        //    LeftimageBytes = new byte[LeftfileLength];
        //    LeftFcPicture.PostedFile.InputStream.Read(LeftimageBytes, 0, LeftfileLength);
        //    if (img == "jpg" || img == "jpeg" || img == "png")
        //    {
        //        if (Fname != String.Empty)
        //        {
        //            string rootpath = Server.MapPath("Images");
        //            if (File.Exists(rootpath + "\\" + Fname))
        //            {
        //                File.Delete(rootpath + "\\" + Fname);
        //                LeftFcPicture.SaveAs(rootpath + "\\" + Fname);
        //                imgLeft.Src = "Images/" + Fname;

        //            }
        //            else
        //            {
        //                LeftFcPicture.SaveAs(rootpath + "\\" + Fname);
        //                imgLeft.Src = "Images/" + Fname;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        imglogo.Src = "";
        //        // lblError.Text = "Please Upload a Image in proper format";
        //    }
        //}

        //public void SaveRightPicture(String Fname, String img) //Right
        //{
        //    RightfileName = RightFcPicture.PostedFile.FileName;
        //    RightfileLength = RightFcPicture.PostedFile.ContentLength;
        //    RightimageBytes = new byte[RightfileLength];
        //    RightFcPicture.PostedFile.InputStream.Read(RightimageBytes, 0, RightfileLength);
        //    if (img == "jpg" || img == "jpeg" || img == "png")
        //    {
        //        if (Fname != String.Empty)
        //        {
        //            string rootpath = Server.MapPath("Images");
        //            if (File.Exists(rootpath + "\\" + Fname))
        //            {
        //                File.Delete(rootpath + "\\" + Fname);
        //                RightFcPicture.SaveAs(rootpath + "\\" + Fname);
        //                imgRhImage.Src = "Images/" + Fname;
        //            }
        //            else
        //            {
        //                RightFcPicture.SaveAs(rootpath + "\\" + Fname);
        //                imgRhImage.Src = "Images/" + Fname;
        //            }
        //        }
        //    }

        //    else
        //    {
        //        imglogo.Src = "";
        //        // lblError.Text = "Please Upload a Image in proper format";
        //    }
        //}

        //public void SaveESIPicture(String Fname, String img)//ESI 
        //{
        //    ESIfileName = ESIPhotoPic.PostedFile.FileName;
        //    ESIfileLength = ESIPhotoPic.PostedFile.ContentLength;
        //    ESIimageBytes = new byte[ESIfileLength];
        //    ESIPhotoPic.PostedFile.InputStream.Read(ESIimageBytes, 0, ESIfileLength);
        //    if (img == "jpg" || img == "jpeg" || img == "png")
        //    {
        //        if (Fname != String.Empty)
        //        {
        //            string rootpath = Server.MapPath("Images");
        //            if (File.Exists(rootpath + "\\" + Fname))
        //            {
        //                File.Delete(rootpath + "\\" + Fname);
        //                ESIPhotoPic.SaveAs(rootpath + "\\" + Fname);
        //                imgESIPic.Src = "Images/" + Fname;
        //            }
        //            else
        //            {
        //                ESIPhotoPic.SaveAs(rootpath + "\\" + Fname);
        //                imgESIPic.Src = "Images/" + Fname;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        imglogo.Src = "";
        //        // lblError.Text = "Please Upload a Image in proper format";
        //    }
        //}

        //public void SaveSignaturePic(String Fname, String img) //Emp Photo
        //{
        //    SignfileName = SignaturePic.PostedFile.FileName;
        //    SignfileLength = SignaturePic.PostedFile.ContentLength;
        //    SignimageBytes = new byte[SignfileLength];
        //    SignaturePic.PostedFile.InputStream.Read(SignimageBytes, 0, SignfileLength);
        //    if (img == "jpg" || img == "jpeg" || img == "png")
        //    {
        //        if (Fname != String.Empty)
        //        {
        //            string rootpath = Server.MapPath("Images");
        //            if (File.Exists(rootpath + "\\" + Fname))
        //            {
        //                File.Delete(rootpath + "\\" + Fname);
        //                SignaturePic.SaveAs(rootpath + "\\" + Fname);
        //                imgSignature.Src = "Images/" + Fname;
        //            }
        //            else
        //            {
        //                SignaturePic.SaveAs(rootpath + "\\" + Fname);
        //                imgSignature.Src = "Images/" + Fname;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        imgSignature.Src = "";
        //        // lblError.Text = "Please Upload a Image in proper format";
        //    }
        //}


        //protected void btn_EmpPhoto_Click1(object sender, EventArgs e)//Emp Photo Click
        //{
        //    if (btn_EmpPhoto.Text == "Select Photo")
        //    {
        //        btn_EmpPhoto.Text = "Save";
        //        FcPicture1.Visible = true;
        //    }
        //    else
        //    {
        //        btn_EmpPhoto.Text = "Select Photo";
        //        string filename = FcPicture1.FileName;
        //        string ext = filename.Substring(filename.IndexOf('.') + 1);
        //        SavePicture(filename, ext.ToLower());
        //       FcPicture1.Visible = false;
        //    }
        //}

        //protected void btn_LeftHandImage_Click(object sender, EventArgs e)//Left Hand Image Click
        //{
        //    if (btn_LeftHandImage.Text == "Select Photo")
        //    {
        //        btn_LeftHandImage.Text = "Save";
        //        LeftFcPicture.Visible = true;
        //    }
        //    else
        //    {
        //        btn_LeftHandImage.Text = "Select Photo";
        //        string LeftfileName = LeftFcPicture.FileName;
        //        string ext = LeftfileName.Substring(LeftfileName.IndexOf('.') + 1);
        //        SaveLeftPicture(LeftfileName, ext.ToLower());
        //        LeftFcPicture.Visible = false;
        //    }
        //}

        //protected void btn_Signature_Click(object sender, EventArgs e)//Signature Button Click
        //{
        //    if (btn_Signature.Text == "Select Photo")
        //    {
        //        btn_Signature.Text = "Save";
        //        SignaturePic.Visible = true;
        //    }
        //    else
        //    {
        //        btn_Signature.Text = "Select Photo";
        //        string SignfileName = SignaturePic.FileName;
        //        string ext = SignfileName.Substring(SignfileName.IndexOf('.') + 1);
        //        SaveSignaturePic(SignfileName, ext.ToLower());
        //        SignaturePic.Visible = false;
        //    }
        //}

        //protected void btn_RightHandImage_Click(object sender, EventArgs e)//Right Hand Image Button Click 
        //{
        //    if (btn_RightHandImage.Text == "Select Photo")
        //    {
        //        btn_RightHandImage.Text = "Save";
        //        RightFcPicture.Visible = true;
        //    }
        //    else
        //    {
        //        btn_RightHandImage.Text = "Select Photo";
        //        string RightfileName = RightFcPicture.FileName;
        //        string ext = RightfileName.Substring(RightfileName.IndexOf('.') + 1);
        //        SaveRightPicture(RightfileName, ext.ToLower());
        //        RightFcPicture.Visible = false;
        //    }
        //}

        //protected void btn_ESIPhoto_Click(object sender, EventArgs e)//ESI Photo Button Cick
        //{
        //    if (btn_ESIPhoto.Text == "Select Photo")
        //    {
        //        btn_ESIPhoto.Text = "Save";
        //        ESIPhotoPic.Visible = true;
        //    }
        //    else
        //    {
        //        btn_ESIPhoto.Text = "Select Photo";
        //        string ESIfileName = ESIPhotoPic.FileName;
        //        string ext = ESIfileName.Substring(ESIfileName.IndexOf('.') + 1);
        //        SaveESIPicture(ESIfileName, ext.ToLower());
        //        ESIPhotoPic.Visible = false;
        //    }
        //}

        public void addempfamilydetails()
        {

            int Getbyresult = 0; string Rname = ""; string Age = ""; string Occupation = ""; string Raadharno = ""; string Relplace = ""; string pfnominee = "N"; string esinominee = "N"; string DOFBirth = "";

            for (int i = 0; i < gvFamilyDetails.Rows.Count; i++)
            {
                TextBox txtEmpRName = gvFamilyDetails.Rows[i].FindControl("txtEmpName") as TextBox;
                DropDownList ddlRelationtype = gvFamilyDetails.Rows[i].FindControl("ddlRelation") as DropDownList;
                TextBox txtDOFBirth = gvFamilyDetails.Rows[i].FindControl("txtRelDtofBirth") as TextBox;
                TextBox txtAge = gvFamilyDetails.Rows[i].FindControl("txtAge") as TextBox;
                TextBox txtoccupation = gvFamilyDetails.Rows[i].FindControl("txtReloccupation") as TextBox;
                TextBox txtRaadharno = gvFamilyDetails.Rows[i].FindControl("txtAAdharNo") as TextBox;
                DropDownList ddlrelresidence = gvFamilyDetails.Rows[i].FindControl("ddlresidence") as DropDownList;
                TextBox txtRelplace = gvFamilyDetails.Rows[i].FindControl("txtplace") as TextBox;
                CheckBox chkpfnominee = gvFamilyDetails.Rows[i].FindControl("ChkPFNominee") as CheckBox;
                CheckBox chkesinominee = gvFamilyDetails.Rows[i].FindControl("ChkESINominee") as CheckBox;


                if (txtEmpRName.Text != string.Empty || ddlRelationtype.SelectedIndex > 0 || txtoccupation.Text != string.Empty || txtRaadharno.Text != string.Empty || txtRelplace.Text != string.Empty || ddlrelresidence.SelectedIndex > 0)
                {

                    var testDate = 0;
                    #region Begin Validating Date Format

                    if (txtDOFBirth.Text.Trim().Length > 0)
                    {
                        testDate = GlobalData.Instance.CheckEnteredDate(txtDOFBirth.Text);
                        if (testDate > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid Date Of Birth Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                            return;
                        }
                    }
                    #endregion End Validating Date Format

                    if (txtDOFBirth.Text.Trim().Length != 0)
                    {
                        DOFBirth = Timings.Instance.CheckDateFormat(txtDOFBirth.Text);
                    }
                    else
                    {
                        DOFBirth = "01/01/1900";
                    }




                    #region Begin Getmax Id from DB

                    int RelationId = 0;

                    string selectquerycomppanyid = "select max(cast(Id as int )) as Id from EmpRelationships where EmpId='" + txtEmID.Text + "'";
                    DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectquerycomppanyid).Result;

                    if (dt.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(dt.Rows[0]["Id"].ToString()) == false)
                        {
                            RelationId = Convert.ToInt32(dt.Rows[0]["Id"].ToString()) + 1;
                            //txtCompid.Text = RelationId.ToString();
                        }
                        else
                        {
                            RelationId = int.Parse("1");
                            //txtCompid.Text = RelationId.ToString();
                        }
                    }
                    #endregion End Getmax Id from DB

                    if (txtEmpRName.Text.Length == 0)
                    {
                        Rname = "";
                    }
                    else
                    {
                        Rname = txtEmpRName.Text;
                    }
                    if (txtAge.Text.Trim().Length > 0)
                    {
                        Age = txtAge.Text;
                    }
                    else
                    {
                        Age = "0";
                    }
                    if (txtoccupation.Text.Length == 0)
                    {
                        Occupation = "";
                    }
                    else
                    {
                        Occupation = txtoccupation.Text;
                    }
                    if (txtRaadharno.Text.Length == 0)
                    {
                        Raadharno = "";
                    }
                    else
                    {
                        Raadharno = txtRaadharno.Text;
                    }
                    if (txtRelplace.Text.Length == 0)
                    {
                        Relplace = "";
                    }
                    else
                    {
                        Relplace = txtRelplace.Text;
                    }

                    string relationtype = "";
                    if (ddlRelationtype.SelectedIndex == 0)
                    {
                        relationtype = string.Empty;
                    }
                    if (ddlRelationtype.SelectedIndex > 0)
                    {
                        relationtype = ddlRelationtype.SelectedValue;
                    }

                    string relationresidence = "";
                    if (ddlrelresidence.SelectedIndex == 0)
                    {
                        relationresidence = string.Empty;
                    }
                    if (ddlrelresidence.SelectedIndex > 0)
                    {
                        relationresidence = ddlrelresidence.SelectedValue;
                    }

                    if (chkpfnominee.Checked)
                    {
                        pfnominee = "Y";
                    }
                    else
                    {
                        pfnominee = "N";
                    }

                    if (chkesinominee.Checked)
                    {
                        esinominee = "Y";
                    }
                    else
                    {
                        esinominee = "N";
                    }

                    string linksave = "insert into EmpRelationships values('" + txtEmID.Text + "','" + Rname + "','" + relationtype + "','" + DOFBirth + "','" + RelationId + "','" + Age + "','" + Occupation + "','" + relationresidence + "','" + Relplace + "','" + pfnominee + "','" + esinominee + "','" + Raadharno + "')";
                    Getbyresult = config.ExecuteNonQueryWithQueryAsync(linksave).Result;
                    txtEmpRName.Text = "";
                    ddlRelationtype.SelectedIndex = 0;
                    txtDOFBirth.Text = "";
                    txtAge.Text = "";
                    txtoccupation.Text = "";
                    txtRaadharno.Text = "";
                    chkpfnominee.Checked = false;
                    chkesinominee.Checked = false;
                    ddlrelresidence.SelectedIndex = 0;
                    txtRelplace.Text = "";

                }
                else
                {
                    txtEmpRName.Text = "";
                    ddlRelationtype.SelectedIndex = 0;
                    txtDOFBirth.Text = "";
                    txtAge.Text = "";
                    txtoccupation.Text = "";
                    txtRaadharno.Text = "";
                    chkpfnominee.Checked = false;
                    chkesinominee.Checked = false;
                    txtRelplace.Text = "";

                }
            }


        }
        public void addempeducationdetails()
        {

            int Getbyresult = 0; string Qualification = ""; string Description = ""; string NameOfschoolClg = ""; string Board = ""; string year = ""; string PassFail = " "; string Percentage = "";

            for (int i = 0; i < GvEducationDetails.Rows.Count; i++)
            {
                DropDownList ddlQualification = GvEducationDetails.Rows[i].FindControl("ddlQualification") as DropDownList;
                TextBox txtQualification = GvEducationDetails.Rows[i].FindControl("txtEdLevel") as TextBox;
                TextBox txtNameofSchoolColg = GvEducationDetails.Rows[i].FindControl("txtNameofSchoolColg") as TextBox;
                TextBox txtBoard = GvEducationDetails.Rows[i].FindControl("txtBoard") as TextBox;
                TextBox txtyear = GvEducationDetails.Rows[i].FindControl("txtyear") as TextBox;
                TextBox txtPassFail = GvEducationDetails.Rows[i].FindControl("txtPassFail") as TextBox;
                TextBox txtPercentage = GvEducationDetails.Rows[i].FindControl("txtPercentage") as TextBox;

                if (txtQualification.Text != string.Empty || txtNameofSchoolColg.Text != string.Empty || txtBoard.Text != string.Empty || txtyear.Text != string.Empty || txtPassFail.Text != string.Empty || txtPercentage.Text != string.Empty || ddlQualification.SelectedIndex > 0)
                {

                    #region Begin Getmax Id from DB
                    int EduId = 0;
                    string selectquery = "select max(cast(Id as int )) as Id from EmpEducationDetails where EmpId='" + txtEmID.Text + "'";
                    DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;


                    if (dt.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(dt.Rows[0]["Id"].ToString()) == false)
                        {
                            EduId = Convert.ToInt32(dt.Rows[0]["Id"].ToString()) + 1;
                        }
                        else
                        {
                            EduId = int.Parse("1");
                        }
                    }
                    #endregion End Getmax Id from DB

                    if (ddlQualification.SelectedIndex > 0)
                    {
                        Qualification = ddlQualification.SelectedValue;
                    }
                    else
                    {
                        Qualification = "0";
                    }

                    if (txtQualification.Text.Length == 0)
                    {
                        Description = "";
                    }
                    else
                    {
                        Description = txtQualification.Text;
                    }
                    if (txtNameofSchoolColg.Text.Trim().Length > 0)
                    {
                        NameOfschoolClg = txtNameofSchoolColg.Text;
                    }
                    else
                    {
                        NameOfschoolClg = " ";
                    }
                    if (txtBoard.Text.Length == 0)
                    {
                        Board = "";
                    }
                    else
                    {
                        Board = txtBoard.Text;
                    }
                    if (txtyear.Text.Length == 0)
                    {
                        year = "";
                    }
                    else
                    {
                        year = txtyear.Text;
                    }
                    if (txtPercentage.Text.Length == 0)
                    {
                        Percentage = "";
                    }
                    else
                    {
                        Percentage = txtPercentage.Text;
                    }
                    if (txtPassFail.Text.Length == 0)
                    {
                        PassFail = "";
                    }
                    else
                    {
                        PassFail = txtPassFail.Text;
                    }



                    string linksave = "insert into EmpEducationDetails (Empid,Qualification,Description,NameOfSchoolClg,BoardorUniversity,YrOfStudy,PassOrFail,PercentageOfmarks,id) values('" + txtEmID.Text + "','" + Qualification + "','" + Description + "','" + NameOfschoolClg + "','" + Board + "','" + year + "','" + PassFail + "','" + Percentage + "','" + EduId + "')";
                    Getbyresult = config.ExecuteNonQueryWithQueryAsync(linksave).Result;


                    txtQualification.Text = "";
                    txtPassFail.Text = "";
                    txtPercentage.Text = "";
                    txtyear.Text = "";
                    txtBoard.Text = "";
                    txtNameofSchoolColg.Text = "";
                    ddlQualification.SelectedIndex = 0;



                }
                else
                {
                    ddlQualification.SelectedIndex = 0;
                    txtQualification.Text = "";
                    txtPassFail.Text = "";
                    txtPercentage.Text = "";
                    txtyear.Text = "";
                    txtBoard.Text = "";
                    txtNameofSchoolColg.Text = "";

                }
            }

        }
        private void AddNewRowToGrid()
        {

            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                //DataRow drCurrentRow1 = null;


                if (dtCurrentTable.Rows.Count > 0)
                {


                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        //TextBox lblSno = (TextBox)gvFamilyDetails.Rows[rowIndex].Cells[0].FindControl("lblSno");
                        TextBox txtEmpName = (TextBox)gvFamilyDetails.Rows[rowIndex].Cells[1].FindControl("txtEmpName");
                        TextBox txtRelDtofBirth = (TextBox)gvFamilyDetails.Rows[rowIndex].Cells[2].FindControl("txtRelDtofBirth");
                        TextBox txtAge = (TextBox)gvFamilyDetails.Rows[rowIndex].Cells[3].FindControl("txtAge");
                        DropDownList ddlRelation = (DropDownList)gvFamilyDetails.Rows[rowIndex].Cells[4].FindControl("ddlRelation");
                        TextBox txtReloccupation = (TextBox)gvFamilyDetails.Rows[rowIndex].Cells[5].FindControl("txtReloccupation");
                        TextBox txtAAdharNo = (TextBox)gvFamilyDetails.Rows[rowIndex].Cells[6].FindControl("txtAAdharNo");
                        CheckBox ChkPFNominee = (CheckBox)gvFamilyDetails.Rows[rowIndex].Cells[7].FindControl("ChkPFNominee");
                        CheckBox ChkESINominee = (CheckBox)gvFamilyDetails.Rows[rowIndex].Cells[8].FindControl("ChkESINominee");
                        DropDownList ddlresidence = (DropDownList)gvFamilyDetails.Rows[rowIndex].Cells[9].FindControl("ddlresidence");
                        TextBox txtplace = (TextBox)gvFamilyDetails.Rows[rowIndex].Cells[10].FindControl("txtplace");


                        drCurrentRow = dtCurrentTable.NewRow();

                        var PFNominee = "N";
                        var ESINominee = "N";

                        //dtCurrentTable.Rows[i - 1]["Region"] = lblSno.Text;
                        dtCurrentTable.Rows[i - 1]["RName"] = txtEmpName.Text;
                        dtCurrentTable.Rows[i - 1]["DOfBirth"] = txtRelDtofBirth.Text;
                        dtCurrentTable.Rows[i - 1]["age"] = txtAge.Text;
                        dtCurrentTable.Rows[i - 1]["RType"] = ddlRelation.SelectedValue;
                        dtCurrentTable.Rows[i - 1]["ROccupation"] = txtReloccupation.Text;
                        dtCurrentTable.Rows[i - 1]["RAAdharNo"] = txtAAdharNo.Text;
                        dtCurrentTable.Rows[i - 1]["RResidence"] = ddlresidence.SelectedValue;
                        dtCurrentTable.Rows[i - 1]["RPlace"] = txtplace.Text;
                        if (ChkPFNominee.Checked == true)
                        {
                            PFNominee = "Y";
                        }
                        if (ChkESINominee.Checked == true)
                        {
                            ESINominee = "Y";
                        }

                        dtCurrentTable.Rows[i - 1]["PFNominee"] = PFNominee;
                        dtCurrentTable.Rows[i - 1]["ESINominee"] = ESINominee;


                        rowIndex++;

                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    gvFamilyDetails.DataSource = dtCurrentTable;
                    gvFamilyDetails.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks
            SetPreviousData();
        }
        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < gvFamilyDetails.Rows.Count; i++)
                    {

                        TextBox txtEmpName = (TextBox)gvFamilyDetails.Rows[rowIndex].Cells[1].FindControl("txtEmpName");
                        TextBox txtRelDtofBirth = (TextBox)gvFamilyDetails.Rows[rowIndex].Cells[2].FindControl("txtRelDtofBirth");
                        TextBox txtAge = (TextBox)gvFamilyDetails.Rows[rowIndex].Cells[3].FindControl("txtAge");
                        DropDownList ddlRelation = (DropDownList)gvFamilyDetails.Rows[rowIndex].Cells[4].FindControl("ddlRelation");
                        TextBox txtReloccupation = (TextBox)gvFamilyDetails.Rows[rowIndex].Cells[5].FindControl("txtReloccupation");
                        TextBox txtAAdharNo = (TextBox)gvFamilyDetails.Rows[rowIndex].Cells[6].FindControl("txtAAdharNo");
                        CheckBox ChkPFNominee = (CheckBox)gvFamilyDetails.Rows[rowIndex].Cells[7].FindControl("ChkPFNominee");
                        CheckBox ChkESINominee = (CheckBox)gvFamilyDetails.Rows[rowIndex].Cells[8].FindControl("ChkESINominee");
                        DropDownList ddlresidence = (DropDownList)gvFamilyDetails.Rows[rowIndex].Cells[9].FindControl("ddlresidence");
                        TextBox txtplace = (TextBox)gvFamilyDetails.Rows[rowIndex].Cells[10].FindControl("txtplace");

                        var PFNominee = "N";
                        PFNominee = dt.Rows[i]["PFNominee"].ToString();
                        if (PFNominee == "Y")
                        {
                            ChkPFNominee.Checked = true;
                        }
                        else
                        {
                            ChkPFNominee.Checked = false;
                        }

                        var ESINominee = "Y";
                        ESINominee = dt.Rows[i]["ESINominee"].ToString();
                        if (ESINominee == "Y")
                        {
                            ChkESINominee.Checked = true;
                        }
                        else
                        {
                            ChkESINominee.Checked = false;
                        }

                        txtEmpName.Text = dt.Rows[i]["RName"].ToString();
                        txtRelDtofBirth.Text = dt.Rows[i]["DOfBirth"].ToString();
                        txtAge.Text = dt.Rows[i]["age"].ToString();
                        ddlRelation.SelectedValue = dt.Rows[i]["RType"].ToString();
                        txtReloccupation.Text = dt.Rows[i]["ROccupation"].ToString();
                        txtAAdharNo.Text = dt.Rows[i]["RAAdharNo"].ToString();
                        ddlresidence.SelectedValue = dt.Rows[i]["RResidence"].ToString();
                        txtplace.Text = dt.Rows[i]["RPlace"].ToString();


                        rowIndex++;
                    }
                }
            }
        }
        private void AddEduNewRowToGrid()
        {

            int rowIndex = 0;

            if (ViewState["EducationTable"] != null)
            {
                DataTable dtEducationTable = (DataTable)ViewState["EducationTable"];
                DataRow drEducationRow = null;
                //DataRow drCurrentRow1 = null;


                if (dtEducationTable.Rows.Count > 0)
                {


                    for (int i = 1; i <= dtEducationTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        //TextBox lblSno = (TextBox)gvFamilyDetails.Rows[rowIndex].Cells[0].FindControl("lblSno");
                        DropDownList ddlQualification = (DropDownList)GvEducationDetails.Rows[rowIndex].Cells[1].FindControl("ddlQualification");
                        TextBox txtEdLevel = (TextBox)GvEducationDetails.Rows[rowIndex].Cells[2].FindControl("txtEdLevel");
                        TextBox txtNameofSchoolColg = (TextBox)GvEducationDetails.Rows[rowIndex].Cells[3].FindControl("txtNameofSchoolColg");
                        TextBox txtBoard = (TextBox)GvEducationDetails.Rows[rowIndex].Cells[4].FindControl("txtBoard");
                        TextBox txtyear = (TextBox)GvEducationDetails.Rows[rowIndex].Cells[5].FindControl("txtyear");
                        TextBox txtPassFail = (TextBox)GvEducationDetails.Rows[rowIndex].Cells[6].FindControl("txtPassFail");
                        TextBox txtPercentage = (TextBox)GvEducationDetails.Rows[rowIndex].Cells[7].FindControl("txtPercentage");


                        drEducationRow = dtEducationTable.NewRow();


                        //dtCurrentTable.Rows[i - 1]["Region"] = lblSno.Text;
                        dtEducationTable.Rows[i - 1]["Qualification"] = ddlQualification.SelectedValue;
                        dtEducationTable.Rows[i - 1]["Description"] = txtEdLevel.Text;
                        dtEducationTable.Rows[i - 1]["NameOfSchoolClg"] = txtNameofSchoolColg.Text;
                        dtEducationTable.Rows[i - 1]["BoardorUniversity"] = txtBoard.Text;
                        dtEducationTable.Rows[i - 1]["YrOfStudy"] = txtyear.Text;
                        dtEducationTable.Rows[i - 1]["PassOrFail"] = txtPassFail.Text;
                        dtEducationTable.Rows[i - 1]["PercentageOfmarks"] = txtPercentage.Text;
                        rowIndex++;


                    }
                    dtEducationTable.Rows.Add(drEducationRow);
                    ViewState["EducationTable"] = dtEducationTable;
                    GvEducationDetails.DataSource = dtEducationTable;
                    GvEducationDetails.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks
            SetEduPreviousData();
        }
        private void SetEduPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["EducationTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["EducationTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < GvEducationDetails.Rows.Count; i++)
                    {

                        DropDownList ddlQualification = (DropDownList)GvEducationDetails.Rows[rowIndex].Cells[1].FindControl("ddlQualification");
                        TextBox txtEdLevel = (TextBox)GvEducationDetails.Rows[rowIndex].Cells[2].FindControl("txtEdLevel");
                        TextBox txtNameofSchoolColg = (TextBox)GvEducationDetails.Rows[rowIndex].Cells[3].FindControl("txtNameofSchoolColg");
                        TextBox txtBoard = (TextBox)GvEducationDetails.Rows[rowIndex].Cells[4].FindControl("txtBoard");
                        TextBox txtyear = (TextBox)GvEducationDetails.Rows[rowIndex].Cells[5].FindControl("txtyear");
                        TextBox txtPassFail = (TextBox)GvEducationDetails.Rows[rowIndex].Cells[6].FindControl("txtPassFail");
                        TextBox txtPercentage = (TextBox)GvEducationDetails.Rows[rowIndex].Cells[7].FindControl("txtPercentage");


                        ddlQualification.SelectedValue = dt.Rows[i]["Qualification"].ToString();
                        txtEdLevel.Text = dt.Rows[i]["Description"].ToString();
                        txtNameofSchoolColg.Text = dt.Rows[i]["NameOfSchoolClg"].ToString();
                        txtBoard.Text = dt.Rows[i]["BoardorUniversity"].ToString();
                        txtyear.Text = dt.Rows[i]["YrOfStudy"].ToString();
                        txtPassFail.Text = dt.Rows[i]["PassOrFail"].ToString();
                        txtPercentage.Text = dt.Rows[i]["PercentageOfmarks"].ToString();


                        rowIndex++;
                    }
                }
            }
        }
        public void addempPreviousExperience()
        {

            int Getbyresult = 0; string RegionCode = ""; string EmpCode = ""; string Extension = ""; string DateofResign = ""; string Designation = ""; string CompAddress = ""; string yearofExp = ""; string PFNo = " "; string ESINo = "";

            for (int i = 0; i < GvPreviousExperience.Rows.Count; i++)
            {
                TextBox txtregioncode = GvPreviousExperience.Rows[i].FindControl("txtregioncode") as TextBox;
                TextBox txtempcode = GvPreviousExperience.Rows[i].FindControl("txtempcode") as TextBox;
                TextBox txtExtension = GvPreviousExperience.Rows[i].FindControl("txtExtension") as TextBox;
                TextBox txtPrevDesignation = GvPreviousExperience.Rows[i].FindControl("txtPrevDesignation") as TextBox;
                TextBox txtCompAddress = GvPreviousExperience.Rows[i].FindControl("txtCompAddress") as TextBox;
                TextBox txtyearofexp = GvPreviousExperience.Rows[i].FindControl("txtyearofexp") as TextBox;
                TextBox txtPFNo = GvPreviousExperience.Rows[i].FindControl("txtPFNo") as TextBox;
                TextBox txtESINo = GvPreviousExperience.Rows[i].FindControl("txtESINo") as TextBox;
                TextBox txtDtofResigned = GvPreviousExperience.Rows[i].FindControl("txtDtofResigned") as TextBox;


                if (txtregioncode.Text != string.Empty || txtDtofResigned.Text != string.Empty || txtempcode.Text != string.Empty || txtPrevDesignation.Text != string.Empty || txtCompAddress.Text != string.Empty || txtyearofexp.Text != string.Empty || txtPFNo.Text != string.Empty || txtESINo.Text != string.Empty)
                {
                    var testDate = 0;
                    #region Begin Validating Date Format

                    if (txtDtofResigned.Text.Trim().Length > 0)
                    {
                        testDate = GlobalData.Instance.CheckEnteredDate(txtDtofResigned.Text);
                        if (testDate > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid Date Of Birth Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                            return;
                        }
                    }
                    #endregion End Validating Date Format

                    if (txtDtofResigned.Text.Trim().Length != 0)
                    {
                        DateofResign = Timings.Instance.CheckDateFormat(txtDtofResigned.Text);
                    }
                    else
                    {
                        DateofResign = "01/01/1900";
                    }


                    #region Begin Getmax Id from DB
                    int PrevExpId = 0;
                    string selectquery = "select max(cast(Id as int )) as Id from EmpPrevExperience where EmpId='" + txtEmID.Text + "'";
                    DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

                    if (dt.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(dt.Rows[0]["Id"].ToString()) == false)
                        {
                            PrevExpId = Convert.ToInt32(dt.Rows[0]["Id"].ToString()) + 1;
                        }
                        else
                        {
                            PrevExpId = int.Parse("1");
                        }
                    }
                    #endregion End Getmax Id from DB

                    if (txtregioncode.Text.Length == 0)
                    {
                        RegionCode = "";
                    }
                    else
                    {
                        RegionCode = txtregioncode.Text;
                    }

                    if (txtempcode.Text.Length == 0)
                    {
                        EmpCode = "";
                    }
                    else
                    {
                        EmpCode = txtempcode.Text;
                    }

                    if (txtExtension.Text.Length == 0)
                    {
                        Extension = "";
                    }
                    else
                    {
                        Extension = txtExtension.Text;
                    }

                    if (txtPrevDesignation.Text.Trim().Length > 0)
                    {
                        Designation = txtPrevDesignation.Text;
                    }
                    else
                    {
                        Designation = " ";
                    }
                    if (txtCompAddress.Text.Length == 0)
                    {
                        CompAddress = "";
                    }
                    else
                    {
                        CompAddress = txtCompAddress.Text;
                    }
                    if (txtyearofexp.Text.Length == 0)
                    {
                        yearofExp = "";
                    }
                    else
                    {
                        yearofExp = txtyearofexp.Text;
                    }
                    if (txtPFNo.Text.Length == 0)
                    {
                        PFNo = "";
                    }
                    else
                    {
                        PFNo = txtPFNo.Text;
                    }
                    if (txtESINo.Text.Length == 0)
                    {
                        ESINo = "";
                    }
                    else
                    {
                        ESINo = txtESINo.Text;
                    }



                    string linksave = "insert into EmpPrevExperience (Empid,RegionCode,EmployerCode,Extension,Designation,CompAddress,YrOfExp,PFNo,ESINo,DateofResign,id) values('" + txtEmID.Text + "','" + RegionCode + "','" + EmpCode + "','" + Extension + "','" + Designation + "','" + CompAddress + "','" + yearofExp + "','" + PFNo + "','" + ESINo + "','" + DateofResign + "','" + PrevExpId + "')";
                    Getbyresult = config.ExecuteNonQueryWithQueryAsync(linksave).Result;
                    txtESINo.Text = "";
                    txtPFNo.Text = "";
                    txtyearofexp.Text = "";
                    txtCompAddress.Text = "";
                    txtPrevDesignation.Text = "";
                    txtregioncode.Text = "";
                    txtempcode.Text = "";
                    txtDtofResigned.Text = "";
                    txtExtension.Text = "";

                }
                else
                {
                    txtESINo.Text = "";
                    txtPFNo.Text = "";
                    txtyearofexp.Text = "";
                    txtCompAddress.Text = "";
                    txtPrevDesignation.Text = "";
                    txtregioncode.Text = "";
                    txtempcode.Text = "";
                    txtDtofResigned.Text = "";
                    txtExtension.Text = "";
                }
            }

        }
        private void AddPrevExpNewRowToGrid()
        {

            int rowIndex = 0;

            if (ViewState["PrevExpTable"] != null)
            {
                DataTable dtPrevExpTable = (DataTable)ViewState["PrevExpTable"];
                DataRow drPrevExpTableRow = null;
                //DataRow drCurrentRow1 = null;


                if (dtPrevExpTable.Rows.Count > 0)
                {


                    for (int i = 1; i <= dtPrevExpTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        //TextBox lblSno = (TextBox)gvFamilyDetails.Rows[rowIndex].Cells[0].FindControl("lblSno");
                        TextBox txtregioncode = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[1].FindControl("txtregioncode");
                        TextBox txtempcode = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[2].FindControl("txtempcode");
                        TextBox txtExtension = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[3].FindControl("txtExtension");
                        TextBox txtPrevDesignation = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[4].FindControl("txtPrevDesignation");
                        TextBox txtCompAddress = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[5].FindControl("txtCompAddress");
                        TextBox txtyearofexp = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[6].FindControl("txtyearofexp");
                        TextBox txtPFNo = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[7].FindControl("txtPFNo");
                        TextBox txtESINo = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[8].FindControl("txtESINo");
                        TextBox txtDtofResigned = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[9].FindControl("txtDtofResigned");


                        drPrevExpTableRow = dtPrevExpTable.NewRow();


                        //dtCurrentTable.Rows[i - 1]["Region"] = lblSno.Text;
                        dtPrevExpTable.Rows[i - 1]["RegionCode"] = txtregioncode.Text;
                        dtPrevExpTable.Rows[i - 1]["EmployerCode"] = txtempcode.Text;
                        dtPrevExpTable.Rows[i - 1]["Extension"] = txtExtension.Text;
                        dtPrevExpTable.Rows[i - 1]["Designation"] = txtPrevDesignation.Text;
                        dtPrevExpTable.Rows[i - 1]["CompAddress"] = txtCompAddress.Text;
                        dtPrevExpTable.Rows[i - 1]["YrOfExp"] = txtyearofexp.Text;
                        dtPrevExpTable.Rows[i - 1]["PFNo"] = txtPFNo.Text;
                        dtPrevExpTable.Rows[i - 1]["ESINo"] = txtESINo.Text;
                        dtPrevExpTable.Rows[i - 1]["DateofResign"] = txtDtofResigned.Text;

                        rowIndex++;



                    }
                    dtPrevExpTable.Rows.Add(drPrevExpTableRow);
                    ViewState["PrevExpTable"] = dtPrevExpTable;
                    GvPreviousExperience.DataSource = dtPrevExpTable;
                    GvPreviousExperience.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks
            SetPrevExpPreviousData();
        }
        private void SetPrevExpPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["PrevExpTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["PrevExpTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < GvPreviousExperience.Rows.Count; i++)
                    {


                        TextBox txtregioncode = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[1].FindControl("txtregioncode");
                        TextBox txtempcode = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[2].FindControl("txtempcode");
                        TextBox txtExtension = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[3].FindControl("txtExtension");
                        TextBox txtPrevDesignation = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[4].FindControl("txtPrevDesignation");
                        TextBox txtCompAddress = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[5].FindControl("txtCompAddress");
                        TextBox txtyearofexp = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[6].FindControl("txtyearofexp");
                        TextBox txtPFNo = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[7].FindControl("txtPFNo");
                        TextBox txtESINo = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[8].FindControl("txtESINo");
                        TextBox txtDtofResigned = (TextBox)GvPreviousExperience.Rows[rowIndex].Cells[9].FindControl("txtDtofResigned");


                        txtregioncode.Text = dt.Rows[i]["RegionCode"].ToString();
                        txtempcode.Text = dt.Rows[i]["EmployerCode"].ToString();
                        txtExtension.Text = dt.Rows[i]["Extension"].ToString();
                        txtPrevDesignation.Text = dt.Rows[i]["Designation"].ToString();
                        txtCompAddress.Text = dt.Rows[i]["CompAddress"].ToString();
                        txtyearofexp.Text = dt.Rows[i]["YrOfExp"].ToString();
                        txtPFNo.Text = dt.Rows[i]["PFNo"].ToString();
                        txtESINo.Text = dt.Rows[i]["ESINo"].ToString();
                        txtDtofResigned.Text = dt.Rows[i]["DateofResign"].ToString();


                        rowIndex++;
                    }
                }
            }
        }
        protected void ddlpreStates_SelectedIndexChanged1(object sender, EventArgs e)
        {
            string query = "select CityId,City from cities where state='" + ddlpreStates.SelectedValue + "' order by City";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dt.Rows.Count > 0)
            {
                ddlpreCity.Enabled = true;
                ddlpreCity.DataValueField = "CityId";
                ddlpreCity.DataTextField = "City";
                ddlpreCity.DataSource = dt;
                ddlpreCity.DataBind();
                ddlpreCity.Items.Insert(0, new ListItem("--Select--", "0"));

            }
            else
            {
                ddlpreCity.Items.Insert(0, new ListItem("--Select--", "0"));
            }

        }
        protected void chkSame_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSame.Checked == true)
            {
                //txtPermanentAddress.Text = txtPresentAddress.Text;
                txtmobile9.Text = txtmobile.Text;
                txtpeTaluka.Text = txtprtaluka.Text;
                txtpePoliceStattion.Text = txtprPoliceStation.Text;
                txtpevillage.Text = txtprvillage.Text;
                txtpeLandmark.Text = txtprLandmark.Text;
                txtpePostOffice.Text = txtprPostOffice.Text;
                txtpePin.Text = txtprpin.Text;
                txtPeriodofStay.Text = txtprPeriodofStay.Text;
                txtResidingDate.Text = txtprResidingDate.Text;

                string query = "select StateId,State from States where StateId='" + ddlpreStates.SelectedValue + "' order by State";
                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
                //write code here for fetching data
                if (dt.Rows.Count > 0)
                {
                    DdlStates.DataValueField = "StateId";
                    DdlStates.DataTextField = "State";
                    DdlStates.DataSource = dt;
                    DdlStates.DataBind();


                }


                string Cityquery = "select CityID,City from Cities where CityID='" + ddlpreCity.SelectedValue + "' order by City";
                DataTable dtCity = config.ExecuteAdaptorAsyncWithQueryParams(Cityquery).Result;
                //write code here for fetching data
                if (dtCity.Rows.Count > 0)
                {
                    ddlcity.DataValueField = "CityID";
                    ddlcity.DataTextField = "City";
                    ddlcity.DataSource = dtCity;
                    ddlcity.DataBind();
                }

            }
            else
            {
                //txtPermanentAddress.Text = "";
                txtmobile9.Text = "";
                txtpevillage.Text = "";
                txtpeTaluka.Text = "";
                txtpePoliceStattion.Text = "";
                txtpePostOffice.Text = "";
                txtpePin.Text = "";
                txtResidingDate.Text = "";
                txtPeriodofStay.Text = "";
                txtpeLandmark.Text = "";

                DataTable DtStateNames = GlobalData.Instance.LoadStateNames();
                if (DtStateNames.Rows.Count > 0)
                {
                    DdlStates.DataValueField = "StateId";
                    DdlStates.DataTextField = "State";
                    DdlStates.DataSource = DtStateNames;
                    DdlStates.DataBind();
                    DdlStates.Items.Insert(0, new ListItem("--Select--", "0"));
                }

                ddlcity.Items.Clear();


            }
        }
        protected void DdlStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "select CityId,City from cities where state='" + DdlStates.SelectedValue + "' order by City";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dt.Rows.Count > 0)
            {
                ddlcity.Enabled = true;
                ddlcity.DataValueField = "CityId";
                ddlcity.DataTextField = "City";
                ddlcity.DataSource = dt;
                ddlcity.DataBind();
                ddlcity.Items.Insert(0, new ListItem("--Select--", "0"));

            }
            else
            {
                ddlcity.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }
        protected void ChkAadharCard_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkAadharCard.Checked == true)
            {
                txtAadharCard.Enabled = true;
                txtAadharName.Enabled = true;

            }
            else
            {
                txtAadharCard.Enabled = false;
                txtAadharName.Enabled = false;
            }
        }
        protected void ChkPanCard_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkPanCard.Checked == true)
            {
                txtPanCard.Enabled = true;
                txtPanCardName.Enabled = true;
            }
            else
            {
                txtPanCard.Enabled = false;
                txtPanCardName.Enabled = false;

            }

        }
        protected void ChkdrivingLicense_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkdrivingLicense.Checked == true)
            {
                txtDrivingLicense.Enabled = true;
                txtDrivingLicenseName.Enabled = true;

            }
            else
            {
                txtDrivingLicense.Enabled = false;
                txtDrivingLicenseName.Enabled = false;

            }


        }
        protected void ChkBankPassbook_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkBankPassbook.Checked == true)
            {
                txtBankPassbook.Enabled = true;
                txtBankPassBookName.Enabled = true;

            }
            else
            {
                txtBankPassbook.Enabled = false;
                txtBankPassBookName.Enabled = false;

            }

        }
        protected void ChkVoterID_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkVoterID.Checked == true)
            {
                txtVoterID.Enabled = true;
                txtVoterName.Enabled = true;
            }
            else
            {
                txtVoterID.Enabled = false;
                txtVoterName.Enabled = false;

            }

        }
        protected void ChkElectricityBill_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkElectricityBill.Checked == true)
            {
                txtElectricityBill.Enabled = true;
                txtElecBillname.Enabled = true;

            }
            else
            {
                txtElectricityBill.Enabled = false;
                txtElecBillname.Enabled = false;

            }
        }
        protected void ChkRationCard_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkRationCard.Checked == true)
            {
                txtRationCard.Enabled = true;
                txtRationCardName.Enabled = true;

            }
            else
            {
                txtRationCard.Enabled = false;
                txtRationCardName.Enabled = false;

            }
        }
        protected void Chkother_CheckedChanged(object sender, EventArgs e)
        {
            if (Chkother.Checked == true)
            {
                txtOther.Enabled = true;
                txtOtherName.Enabled = true;
            }
            else
            {
                txtOther.Enabled = false;
                txtOtherName.Enabled = false;

            }

        }
        protected void ChkESICCard_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkESICCard.Checked == true)
            {
                txtESICCardNo.Enabled = true;
                txtESICName.Enabled = true;
            }
            else
            {
                txtESICCardNo.Enabled = false;
                txtESICName.Enabled = false;

            }
        }
        protected void grvMergeHeader_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(1, 1, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "";
                HeaderCell.ColumnSpan = 7;
                HeaderCell.CssClass = "HeaderStyle";
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Nominee";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 2;
                HeaderCell.CssClass = "HeaderStyle";
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "";
                // HeaderCell.HorizontalAlign = 1;
                HeaderCell.ColumnSpan = 2;
                HeaderCell.CssClass = "HeaderStyle";
                HeaderGridRow.Cells.Add(HeaderCell);



                gvFamilyDetails.Controls[0].Controls.AddAt(0, HeaderGridRow);

            }
        }
        protected void rdbNotVerified_CheckedChanged(object sender, EventArgs e)
        {
            txtPoliceVerificationNo.Enabled = true;
        }
        protected void rdbVerified_CheckedChanged(object sender, EventArgs e)
        {
            txtPoliceVerificationNo.Enabled = false;
        }
        protected void ddlTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTitle.SelectedIndex == 0)
            {
                rdbmale.Checked = true;
                rdbfemale.Checked = false;
            }
            else if (ddlTitle.SelectedIndex == 1)
            {
                rdbfemale.Checked = true;
                rdbmale.Checked = false;
            }
            else if (ddlTitle.SelectedIndex == 2)
            {
                rdbfemale.Checked = true;
                rdbmale.Checked = false;
            }
        }
        protected void rdbStaff_CheckedChanged(object sender, EventArgs e)
        {
            employeeid();
            txtEmID.ReadOnly = true;

        }
        protected void rdbGeneral_CheckedChanged(object sender, EventArgs e)
        {
            txtEmID.ReadOnly = true;
            employeeid();
        }

        //protected void ddlbirthstate_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string query = "select CityId,City from cities where state='" + ddlbirthstate.SelectedValue + "' order by City";
        //    DataTable dt = SqlHelper.Instance.GetTableByQuery(query);
        //    if (dt.Rows.Count > 0)
        //    {
        //        ddlBirthDistrict.Enabled = true;
        //        ddlBirthDistrict.Enabled = true;
        //        ddlBirthDistrict.DataValueField = "CityId";
        //        ddlBirthDistrict.DataTextField = "City";
        //        ddlBirthDistrict.DataSource = dt;
        //        ddlBirthDistrict.DataBind();
        //        ddlBirthDistrict.Items.Insert(0, new ListItem("--Select--", "0"));

        //    }
        //    else
        //    {
        //        ddlBirthDistrict.Items.Insert(0, new ListItem("--Select--", "0"));
        //    }
        //}

        protected void TxtIDCardIssuedDt_TextChanged(object sender, EventArgs e)
        {
            DateTime dt = DateTime.ParseExact(TxtIDCardIssuedDt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime end = dt.AddYears(1).AddDays(-1);
            TxtIdCardValid.Text = end.ToString("dd/MM/yyyy");
        }
        protected void btnPrevExpAdd_Click(object sender, EventArgs e)
        {
            AddPrevExpNewRowToGrid();
        }
        protected void btnEduAdd_Click(object sender, EventArgs e)
        {
            AddEduNewRowToGrid();
        }
        protected void btnFamilyDetailsAdd_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid();
        }

        protected void BtnOldEmpidDetails_Click(object sender, EventArgs e)
        {
            rdbResigned.Enabled = false;
            string ID = "";
            if (ddloldempdrp.SelectedIndex > 0)
            {
                ID = ddloldempdrp.SelectedValue;
                txtRejoinEmpid.Text = ddloldempdrp.SelectedValue;
                LoadPersonalInfo(ID);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Select Employee ID');", true);
                return;
            }
        }
        protected void ChkSpeciallyAbled_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSpeciallyAbled.Checked == true)
            {
                ddlAppCategory.Enabled = true;
            }
            else
            {
                ddlAppCategory.Enabled = false;
            }
        }
        protected void rdbmanual_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbmanual.Checked == true)
            {

                txtEmID.ReadOnly = false;
            }
            else if (rdbGeneral.Checked == true)
            {
                txtEmID.ReadOnly = true;
            }
            else if (rdbStaff.Checked == true)
            {
                txtEmID.ReadOnly = true;
            }
            else
            {
                txtEmID.ReadOnly = true;
            }


        }

        protected void ddlpvcstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "select CityId,City from cities where state='" + ddlpvcstate.SelectedValue + "' order by City";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dt.Rows.Count > 0)
            {
                ddlpvccity.Enabled = true;
                ddlpvccity.DataValueField = "CityId";
                ddlpvccity.DataTextField = "City";
                ddlpvccity.DataSource = dt;
                ddlpvccity.DataBind();
                ddlpvccity.Items.Insert(0, new ListItem("--Select--", "0"));
            }

            else
            {
                ddlpvccity.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }

        protected void rdbpreaddress_CheckedChanged(object sender, EventArgs e)
        {
            txtpvclandmark.Text = txtprLandmark.Text;
            txtpvcvillage.Text = txtprvillage.Text;
            txtpvcpostofc.Text = txtprPostOffice.Text;
            txtpvctaluka.Text = txtprtaluka.Text;
            txtpvcpolicestation.Text = txtprPoliceStation.Text;
            ddlpvcstate.SelectedIndex = ddlpreStates.SelectedIndex;
            loadcities();
            ddlpvccity.SelectedValue = ddlpreCity.SelectedValue;
            txtpvcpin.Text = txtprpin.Text;
            txtpvcresidedate.Text = txtprResidingDate.Text;
            txtpvcstay.Text = txtprPeriodofStay.Text;
            txtpvcphone.Text = txtmobile.Text;
        }

        protected void rdbperaddress_CheckedChanged(object sender, EventArgs e)
        {
            txtpvclandmark.Text = txtpeLandmark.Text;
            txtpvcvillage.Text = txtpevillage.Text;
            txtpvcpostofc.Text = txtpePostOffice.Text;
            txtpvctaluka.Text = txtpeTaluka.Text;
            txtpvcpolicestation.Text = txtpePoliceStattion.Text;
            ddlpvcstate.SelectedIndex = DdlStates.SelectedIndex;
            loadcities();
            ddlpvccity.SelectedIndex = ddlcity.SelectedIndex;
            txtpvcpin.Text = txtpePin.Text;
            txtpvcresidedate.Text = txtResidingDate.Text;
            txtpvcstay.Text = txtPeriodofStay.Text;
            txtpvcphone.Text = txtmobile9.Text;
        }

        protected void loadcities()
        {
            string query = "select CityId,City from cities where state='" + ddlpvcstate.SelectedValue + "' order by City";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dt.Rows.Count > 0)
            {
                ddlpvccity.Enabled = true;
                ddlpvccity.DataValueField = "CityId";
                ddlpvccity.DataTextField = "City";
                ddlpvccity.DataSource = dt;
                ddlpvccity.DataBind();
                ddlpvccity.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }

        private void ClearImages()
        {
            #region Images clear code done by Mahesh Goud on 2022-07-18

            Imageempphoto.ImageUrl = null;
            AadharImg.ImageUrl = null;
            AadharBackImg.ImageUrl = null;
            pancardimageb.ImageUrl = null;
            pancardBackimageb.ImageUrl = null;
            BankpassImageb.ImageUrl = null;
            #endregion

        }

        protected void ddlAndroidEmpid_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearImages();
            string AndroidEmpid = ddlAndroidEmpid.SelectedValue;
            LoadPersonalInfo(AndroidEmpid);
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            string updatequery = "update Empdetails_Android set EmpRejectStatus=1,Empstatus=2 where empid='" + ddlAndroidEmpid.SelectedValue + "' ";
            int result = config.ExecuteNonQueryWithQueryAsync(updatequery).Result;
            ClearDataFromPersonalInfoTabFields();
            AndroidEmpid();
        }
    }
}