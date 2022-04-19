using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using SRF.P.DAL;

namespace SRF.P.Module_Clients
{
    public partial class AddContract : System.Web.UI.Page
    {
        //DataTable dt;
        DropDownList bind_dropdownlist;
        DropDownList bind_dropdownlistsw;
        DropDownList bind_dropdownlistHSN;

        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Zone = "";

        AppConfiguration config = new AppConfiguration();

        protected void Page_Load(object sender, EventArgs e)
        {

            int i = 0;
            try
            {
                //GlobalData.Instance.AppendLog("In Page_Load");
                GetWebConfigdata();
                if (!IsPostBack)
                {
                    //rowindexvisible = 1;
                    Session["ContractsAIndex"] = 0;
                    Session["ContractsAIndexsw"] = 0;
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                    LoadClientList();
                    LoadClientNames();
                    displaydata();
                    DisplayDefaultRow();
                    Displaydefaulrowsw();
                    Enable5Rows();
                    Enable5rowssw();
                    LoadEsibranches();
                    FillddlTakedata();
                    //LoadActiveClients();

                    if (Request.QueryString["clientid"] != null || Request.QueryString["ContractID"] != null)
                    {

                        string username = Request.QueryString["clientid"].ToString();
                        ddlclientid.SelectedValue = username;
                        ddlclientid_OnSelectedIndexChanged(sender, e);
                        string ContractID = Request.QueryString["ContractID"].ToString();
                        ddlContractids.SelectedValue = ContractID;
                        ddlContractids_OnSelectedIndexChanged(sender, e);
                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }


        protected void LoadActiveClients()
        {
            DataTable DtClientnames = null;
            DataTable dts = GlobalData.Instance.LoadZoneOnUserID(Zone);


            bool SelectAll = false;

            //if (ChkSelectAll.Checked)
            //    SelectAll = true;

            string order = "";

            order = "orderClientName";

            DtClientnames = GlobalData.Instance.LoadActiveClientnames(CmpIDPrefix, dts, SelectAll, order, "");

            if (DtClientnames.Rows.Count > 0)
            {
                ddlcname.DataValueField = "Clientid";
                ddlcname.DataTextField = "clientname";
                ddlcname.DataSource = DtClientnames;
                ddlcname.DataBind();
            }

            order = "orderClientID";

            DtClientnames = GlobalData.Instance.LoadActiveClientnames(CmpIDPrefix, dts, SelectAll, order, "");
            if (DtClientnames.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "Clientid";
                ddlclientid.DataTextField = "Clientid";
                ddlclientid.DataSource = DtClientnames;
                ddlclientid.DataBind();
            }

            ddlcname.Items.Insert(0, "-Select-");
            ddlclientid.Items.Insert(0, "-Select-");

        }



        protected void Fillcname()
        {
            if (ddlclientid.SelectedIndex > 0)
            {
                ddlcname.SelectedValue = ddlclientid.SelectedValue;
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void FillClientid()
        {
            if (ddlcname.SelectedIndex > 0)
            {
                ddlclientid.SelectedValue = ddlcname.SelectedValue;
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
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
        }

        protected void Enable5Rows()
        {
            btnadddesgn_Click1(this, null);
            btnadddesgn_Click1(this, null);
            btnadddesgn_Click1(this, null);
            btnadddesgn_Click1(this, null);
            btnadddesgn_Click1(this, null);

        }

        protected void Enable5rowssw()
        {
            if (RadioSpecial.Checked)
            {
                btnadddesgnsw_Click(this, null);
                //btnadddesgnsw_Click(this, null);
                //btnadddesgnsw_Click(this, null);
                //btnadddesgnsw_Click(this, null);
                //btnadddesgnsw_Click(this, null);
            }
            else
            {
                // gvSWDesignations.DataSource = null;
                //  gvSWDesignations.DataBind();
            }
        }

        protected void GetWebConfigdata()
        {

            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            //Zone = Session["Zone"].ToString();

        }

        protected void DisplayDefaultRow()
        {
            for (int i = 0; i < gvdesignation.Rows.Count; i++)
            {
                if (i < 1)
                {
                    Session["ContractsAIndex"] = Convert.ToInt16(Session["ContractsAIndex"]) + 1;
                    gvdesignation.Rows[i].Visible = true;
                    DefaultRowData(i);
                }
                else
                    gvdesignation.Rows[i].Visible = false;
            }
            Session["ContractsAIndex"] = 1;
            int check = int.Parse(Session["ContractsAIndex"].ToString());
        }

        protected void Displaydefaulrowsw()
        {
            for (int i = 0; i < gvSWDesignations.Rows.Count; i++)
            {
                if (i < 1)
                {
                    Session["ContractsAIndexsw"] = Convert.ToInt16(Session["ContractsAIndexsw"]) + 1;
                    int a = int.Parse(Session["ContractsAIndexsw"].ToString());
                    gvSWDesignations.Rows[i].Visible = true;
                    DefaultRowDatasw(i);
                }
                else
                    gvSWDesignations.Rows[i].Visible = false;
            }
            Session["ContractsAIndexsw"] = 1;
            int check = int.Parse(Session["ContractsAIndexsw"].ToString());
        }

        protected void DefaultRowDatasw(int row)
        {
            //Gvswdesignations Data 

            DropDownList ddlindexsw = gvSWDesignations.Rows[row].FindControl("DdlDesign") as DropDownList;
            ddlindexsw.SelectedIndex = 0;

            DropDownList ddlNoofDayssw = gvSWDesignations.Rows[row].FindControl("ddlNoOfDaysWages") as DropDownList;
            ddlNoofDayssw.SelectedIndex = 0;

            DropDownList ddlNoOfOtsPaysheet = gvSWDesignations.Rows[row].FindControl("ddlNoOfOtsPaysheet") as DropDownList;
            ddlNoOfOtsPaysheet.SelectedIndex = 0;

            DropDownList ddlLAtype = gvSWDesignations.Rows[row].FindControl("ddlLAtype") as DropDownList;
            ddlLAtype.SelectedIndex = 0;

            DropDownList ddlGratuitytype = gvSWDesignations.Rows[row].FindControl("ddlGratuitytype") as DropDownList;
            ddlGratuitytype.SelectedIndex = 0;

            DropDownList ddlbonustype = gvSWDesignations.Rows[row].FindControl("ddlbonustype") as DropDownList;
            ddlbonustype.SelectedIndex = 0;


            DropDownList ddlNoOfNhsPaysheet = gvSWDesignations.Rows[row].FindControl("ddlNoOfNhsPaysheet") as DropDownList;
            ddlNoOfNhsPaysheet.SelectedIndex = 0;

            TextBox Cbasicsw = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtBasic");
            Cbasicsw.Text = "";
            TextBox Cdasw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtda");
            Cdasw.Text = "";
            TextBox Chrasw = (TextBox)gvSWDesignations.Rows[row].FindControl("txthra");
            Chrasw.Text = "";
            TextBox CConveyancesw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtConveyance");
            CConveyancesw.Text = "";

            TextBox Cawsw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtoa");
            Cawsw.Text = "";
            TextBox Cwashallowancesw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtwa");
            Cwashallowancesw.Text = "";
            TextBox Ccasw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtcca");
            Ccasw.Text = "";
            TextBox CLeaveAmountsw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtleaveamount");
            CLeaveAmountsw.Text = "";
            TextBox CGratutysw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtgratuty");
            CGratutysw.Text = "";
            TextBox CBonussw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtbonus");
            CBonussw.Text = "";
            TextBox CWashallowancesw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtwa");
            CWashallowancesw.Text = "";
            TextBox COtherallowancesw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtoa");
            COtherallowancesw.Text = "";
            TextBox txtNfhs1 = (TextBox)gvSWDesignations.Rows[row].FindControl("txtNfhs1");
            txtNfhs1.Text = "";
            TextBox Txtrc = (TextBox)gvSWDesignations.Rows[row].FindControl("Txtrc");
            Txtrc.Text = "";
            TextBox TxtCs = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtCs");
            TxtCs.Text = "";
            TextBox TxtOTRate = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtOTRate");
            TxtOTRate.Text = "";

            TextBox TxtNhsRate = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtNhsRate");
            TxtNhsRate.Text = "";
            TextBox CTxtScPersw = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtScPer");
            CTxtScPersw.Text = "";
            TextBox Ctxtattbonussw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtattbonus");
            Ctxtattbonussw.Text = "";




        }

        protected void DefaultRowData(int row)
        {
            string Cddldesignation = ((DropDownList)gvdesignation.Rows[row].FindControl("DdlDesign")).Text;
            DropDownList ddlindex = gvdesignation.Rows[row].FindControl("DdlDesign") as DropDownList;
            ddlindex.SelectedIndex = 0;

            DropDownList ddlNoofDays = gvdesignation.Rows[row].FindControl("ddlNoOfDaysBilling") as DropDownList;
            ddlNoofDays.SelectedIndex = 0;

            DropDownList ddlNoOfOtsPaysheet = gvdesignation.Rows[row].FindControl("ddlNoOfOtsPaysheet") as DropDownList;
            ddlNoOfOtsPaysheet.SelectedIndex = 0;

            DropDownList CddlLAType = gvdesignation.Rows[row].FindControl("ddlLAtype") as DropDownList;
            CddlLAType.SelectedIndex = 0;

            DropDownList Cddlbonustype = gvdesignation.Rows[row].FindControl("ddlbonustype") as DropDownList;
            Cddlbonustype.SelectedIndex = 0;


            string Cddldutytype = ((DropDownList)gvdesignation.Rows[row].FindControl("ddldutytype")).Text;
            DropDownList ddldutyindex = gvdesignation.Rows[row].FindControl("ddldutytype") as DropDownList;
            ddldutyindex.SelectedIndex = 0;
            TextBox Cdutyhrs = (TextBox)gvdesignation.Rows[row].FindControl("txtdutyhrs");
            Cdutyhrs.Text = "";
            TextBox Cquantity = (TextBox)gvdesignation.Rows[row].FindControl("txtquantity");
            Cquantity.Text = "";
            TextBox Csummary = (TextBox)gvdesignation.Rows[row].FindControl("txtsummary");
            Csummary.Text = "";
            TextBox Cbasic = (TextBox)gvdesignation.Rows[row].FindControl("TxtBasic");
            Cbasic.Text = "";
            TextBox Cda = (TextBox)gvdesignation.Rows[row].FindControl("txtda");
            Cda.Text = "";
            TextBox Chra = (TextBox)gvdesignation.Rows[row].FindControl("txthra");
            Chra.Text = "";
            TextBox CConveyance = (TextBox)gvdesignation.Rows[row].FindControl("txtConveyance");
            CConveyance.Text = "";

            TextBox Caw = (TextBox)gvdesignation.Rows[row].FindControl("txtoa");
            Caw.Text = "";
            TextBox Cwashallowance = (TextBox)gvdesignation.Rows[row].FindControl("txtwa");
            Cwashallowance.Text = "";
            TextBox Cca = (TextBox)gvdesignation.Rows[row].FindControl("txtcca");
            Cca.Text = "";
            TextBox CLeaveAmount = (TextBox)gvdesignation.Rows[row].FindControl("txtleaveamount");
            CLeaveAmount.Text = "";
            TextBox CGratuty = (TextBox)gvdesignation.Rows[row].FindControl("txtgratuty");
            CGratuty.Text = "";
            TextBox CBonus = (TextBox)gvdesignation.Rows[row].FindControl("txtbonus");
            CBonus.Text = "";
            TextBox CPayRate = (TextBox)gvdesignation.Rows[row].FindControl("txtPayRate");
            CPayRate.Text = "";
            TextBox txtNfhs = (TextBox)gvdesignation.Rows[row].FindControl("txtNfhs");
            txtNfhs.Text = "";

            TextBox Txtrc = (TextBox)gvdesignation.Rows[row].FindControl("Txtrc");
            Txtrc.Text = "";

            TextBox TxtCs = (TextBox)gvdesignation.Rows[row].FindControl("TxtCs");
            TxtCs.Text = "";

            TextBox TxtOTRate = (TextBox)gvdesignation.Rows[row].FindControl("TxtOTRate");
            TxtOTRate.Text = "";

            TextBox CTxtScPer = (TextBox)gvdesignation.Rows[row].FindControl("TxtScPer");
            CTxtScPer.Text = "";
            TextBox Ctxtattbonus = (TextBox)gvdesignation.Rows[row].FindControl("txtattbonus");
            Ctxtattbonus.Text = "";

            TextBox Ctxtuniform = (TextBox)gvdesignation.Rows[row].FindControl("txtuniform");
            Ctxtuniform.Text = "";


        }

        //Display Data In GridView
        private void displaydata()
        {
            // string selectquery = "Select Design from designations ORDER BY Design";
            // DataTable DtDesignation = SqlHelper.Instance.GetTableByQuery(selectquery);
            DataTable DtDesignation = GlobalData.Instance.LoadDesigns();
            DataTable DtHSNNumber = GlobalData.Instance.LoadHSNNumbers();

            gvdesignation.DataSource = DtDesignation;
            gvdesignation.DataBind();
            gvSWDesignations.DataSource = DtDesignation;
            gvSWDesignations.DataBind();

            foreach (GridViewRow grdRow in gvdesignation.Rows)
            {
                bind_dropdownlist = (DropDownList)(gvdesignation.Rows[grdRow.RowIndex].Cells[0].FindControl("DdlDesign"));
                bind_dropdownlist.Items.Clear();
                //bind_dropdownlist.Items.Add("--Select--");
                //for (int i = 0; i < DtDesignation.Rows.Count; i++)
                //{
                //    bind_dropdownlist.Items.Add(DtDesignation.Rows[i][0].ToString());
                //}

                if (DtDesignation.Rows.Count > 0)
                {
                    bind_dropdownlist.DataValueField = "Designid";
                    bind_dropdownlist.DataTextField = "Design";
                    bind_dropdownlist.DataSource = DtDesignation;
                    bind_dropdownlist.DataBind();

                }
                bind_dropdownlist.Items.Insert(0, "--Select--");
                bind_dropdownlist.SelectedIndex = 0;

                break;
            }
            foreach (GridViewRow grdRow in gvdesignation.Rows)
            {
                bind_dropdownlistsw = (DropDownList)(gvSWDesignations.Rows[grdRow.RowIndex].Cells[0].FindControl("DdlDesign"));
                bind_dropdownlistsw.Items.Clear();
                //bind_dropdownlistsw.Items.Add("--Select--");
                //for (int i = 0; i < DtDesignation.Rows.Count; i++)
                //{
                //    bind_dropdownlistsw.Items.Add(DtDesignation.Rows[i][0].ToString());

                //}



                if (DtDesignation.Rows.Count > 0)
                {
                    bind_dropdownlistsw.DataValueField = "Designid";
                    bind_dropdownlistsw.DataTextField = "Design";
                    bind_dropdownlistsw.DataSource = DtDesignation;
                    bind_dropdownlistsw.DataBind();

                }
                bind_dropdownlistsw.Items.Insert(0, "--Select--");
                bind_dropdownlistsw.SelectedIndex = 0;


                bind_dropdownlistHSN = (DropDownList)(gvdesignation.Rows[grdRow.RowIndex].Cells[8].FindControl("ddlHSNNumber"));

                if (DtHSNNumber.Rows.Count > 0)
                {
                    bind_dropdownlistHSN.DataValueField = "Id";
                    bind_dropdownlistHSN.DataTextField = "HSNNo";
                    bind_dropdownlistHSN.DataSource = DtHSNNumber;
                    bind_dropdownlistHSN.DataBind();

                }

               
            }

        }

        protected void btnadddesgn_Click1(object sender, EventArgs e)
        {
            int designCount = Convert.ToInt16(Session["ContractsAIndex"]);
            if (designCount < gvdesignation.Rows.Count)
            {
                gvdesignation.Rows[designCount].Visible = true;
                DefaultRowData(designCount);

                //string selectquery = "Select Design from designations ORDER BY Design";
                //DataTable DtDesignation = SqlHelper.Instance.GetTableByQuery(selectquery);

                DataTable DtDesignation = GlobalData.Instance.LoadDesigns();
                DropDownList ddldrow = gvdesignation.Rows[designCount].FindControl("DdlDesign") as DropDownList;
                ddldrow.Items.Clear();
                //ddldrow.Items.Add("--Select--");
                //for (int i = 0; i < DtDesignation.Rows.Count; i++)
                //{
                //    ddldrow.Items.Add(DtDesignation.Rows[i][0].ToString());
                //}


                if (DtDesignation.Rows.Count > 0)
                {
                    ddldrow.DataValueField = "Designid";
                    ddldrow.DataTextField = "Design";
                    ddldrow.DataSource = DtDesignation;
                    ddldrow.DataBind();

                }
                ddldrow.Items.Insert(0, "--Select--");
                ddldrow.SelectedIndex = 0;

                designCount = designCount + 1;
                Session["ContractsAIndex"] = designCount;
            }
            else
            {
                lblmsgcontractdetails.Text = "Theres is No more Designations";
                //ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' There Is');", true);
            }
        }

        private void ClearDataFromThePage()
        {
            //ddlclientid.SelectedIndex = ddlcname.SelectedIndex =
            DdlPf.SelectedIndex = DdlEsi.SelectedIndex = DdlOt.SelectedIndex = ddlOAPer.SelectedIndex = 0;
            txtStartingDate.Text = txtEndingDate.Text = txtValidityDate.Text = "";
            txtBGAmount.Text = txtcontractid.Text = TxtContractDescription.Text = txtEMDValue.Text = txtSecurityDeposit.Text = txtTypeOfWork.Text = txtMaterial.Text =
            txtMachinary.Text = txtEMDValue.Text = txtlampsum.Text = txtPerGurante.Text = txtservicecharge.Text = txtOWF.Text =

            txtservicecharge.Text = txtWAWA.Text = txtotsalaryrate.Text = txttlamt.Text = "";
            TxtPf.Text = TxtEsi.Text = "100";
            RadioManPower.Checked = radiono.Checked = RadioPercent.Checked = Radio1to1.Checked = RadioClient.Checked =
            RadioWithST.Checked = radiootregular.Checked = chkCGST.Checked = RdbSGST.Checked = RdbIGST.Checked = true;
            RadioLumpsum.Checked = radioyes.Checked = RadioAmount.Checked = RadioStartDate.Checked = RadioCompany.Checked = Chkpdfs.Checked = ChkRoundOff.Checked =
            RadioWithoutST.Checked = radiootspecial.Checked = RadioSpecial.Checked = chkojt.Checked = chktl.Checked = RdbIGST.Checked = chkCess1.Checked = chkCess2.Checked = false;
            ddlnoofdays.SelectedIndex = ddlNoOfDaysWages.SelectedIndex = ddlbilldates.SelectedIndex = ddlPaySheetDates.SelectedIndex = ddlpfon.SelectedIndex = ddlesion.SelectedIndex = 0;
            chkotsalaryrate.Checked = checkPFonOT.Checked = checkESIonOT.Checked = chkProfTax.Checked = chkspt.Checked = CheckIncludeST.Checked = Check75ST.Checked = Chkpf.Checked = ChkEsi.Checked = chkrc.Checked = false;
            txtTds.Text = txtPono.Text = txtExpectdateofreceipt.Text = txtLumpsumtext.Text = string.Empty;
            chkStaxonservicecharge.Checked = ChkPFSpl.Checked = false;
            if (radiono.Checked == true)
            {
                RadioPercent.Visible = false;
                RadioAmount.Visible = false;
                txtservicecharge.Visible = false;
                chkStaxonservicecharge.Visible = false;
            }


            if (RadioManPower.Checked == true)
            {
                txtlampsum.Visible = false;
                txtLumpsumtext.Visible = false;
                lbllumpsumtext.Visible = false;
            }

        }

        protected void Btn_Save_Contracts_Click(object sender, EventArgs e)
        {
            try
            {
                var testDate = 0;

                #region  Begin Code For Validations As on [18-10-2013]

                #region     Begin Code For Check The Client Id/Name Selected Or Not   as on [18-10-2013]
                if (ddlclientid.SelectedIndex == 0)
                {
                    lblMsg.Text = "Please Select Clientid";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' please Select Clientid ');", true);
                    clearcontractdetails();
                    return;
                }
                #endregion  End Code For Check The Client Id/Name Selected Or Not  as on [18-10-2013]

                #region     Begin Code For Check The Contract Start Date Entered Or Not  as on [18-10-2013]
                if (txtStartingDate.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill start date.');", true);
                    return;
                }

                if (txtStartingDate.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtStartingDate.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "show alert", "alert('You Are Entered Invalid Contract Start Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }
                    else
                    {
                        string CheckSD = Timings.Instance.CheckDateFormat(txtStartingDate.Text);
                        //string CheckSD = DateTime.Parse(txtStartingDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();

                        string CheckStartDate = "";

                        if (ddlContractids.SelectedIndex == 0)
                        {
                            CheckStartDate = " select clientid from contracts  where ContractEndDate>='" +
                                CheckSD + "'  and Clientid='" + ddlclientid.SelectedValue + "'";

                            DataTable Dt = config.ExecuteReaderWithQueryAsync(CheckStartDate).Result;
                            if (Dt.Rows.Count > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(),
                                  "show alert", "alert('You Are Entered Invalid Contract Start Date.Start Date Should Not Be Interval of the Previous Contracts Start and End Dates  Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                                return;
                            }
                        }
                        else
                        {
                            if (ddlContractids.SelectedIndex > 1)
                            {
                                string CIDForCheck = (txtcontractid.Text).ToString().Substring((txtcontractid.Text).Length - 2);
                                CheckStartDate = " select clientid from contracts  where ContractEndDate>='" +
                                    CheckSD + "'  and Clientid='" + ddlclientid.SelectedValue + "'  and Right(contractid,2)<" + CIDForCheck;

                                DataTable Dt = config.ExecuteReaderWithQueryAsync(CheckStartDate).Result;
                                if (Dt.Rows.Count > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(),
                                      "show alert", "alert('You Are Entered Invalid Contract Start Date.Start Date Should Not Be Interval of the Previous Contracts Start and End Dates  Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                                    return;
                                }
                            }
                        }

                    }


                }


                #endregion  End Code For Check The Contract Start Date Entered Or Not  as on [18-10-2013]

                #region     Begin Code For Check The Contract End Date Enetered Or Not  as on [18-10-2013]
                if (txtEndingDate.Text.Trim().Length == 0)
                {
                    lblMsg.Text = "Please fill End date.";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill End date.');", true);
                    return;
                }

                if (txtEndingDate.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtEndingDate.Text);
                    if (testDate > 0)
                    {
                        lblMsg.Text = "You Are Entered Invalid Contract End Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        // ScriptManager.RegisterStartupScript(this, GetType(),
                        //"show alert", "alert('You Are Entered Invalid Contract End Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }

                }



                #endregion  End Code For Check The Contract End Date Enetered Or Not as on [18-10-2013]

                #region     Begin Code For Check The Selected Dates are Valid Or Not  as on [18-10-2013]
                if (txtStartingDate.Text.Trim().Length != 0 && txtEndingDate.Text.Trim().Length != 0)
                {
                    DateTime Dtstartdate = DateTime.Parse(txtStartingDate.Text, CultureInfo.GetCultureInfo("en-gb"));
                    DateTime DtEnddate = DateTime.Parse(txtEndingDate.Text, CultureInfo.GetCultureInfo("en-gb"));


                    if (Dtstartdate >= DtEnddate)
                    {
                        lblMsg.Text = "Invalid Contract End Date . Contract End Date Always Should Be Greater Than To Start Date.";
                        return;
                    }
                }
                #endregion  End Code For Check Selected Dates are Valid Or Not   as on [18-10-2013]

                #region     Begin Code For Check The Lampsum if Lampsum Selected  as on [18-10-2013]
                if (RadioLumpsum.Checked)
                {
                    if (txtlampsum.Text.Trim().Length == 0)
                    {
                        lblMsg.Text = "Please Enter The Lampsum Amount.";
                        return;
                    }
                }

                #endregion  End Code For Check The Lampsum if Lampsum Selected    as on [18-10-2013]

                #region     Begin Code For Check The Service Charge if Service Charge Yes Selected  as on [18-10-2013]
                if (radioyes.Checked)
                {
                    if (txtservicecharge.Text.Trim().Length == 0)
                    {
                        lblMsg.Text = "Please Enter The Service Charge.";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter The Service Charge.');", true);
                        return;
                    }
                }

                #endregion  End Code For Check The Service Charge if Service Charge Yes Selected    as on [18-10-2013]

                #endregion  Begin Code For Validations As on [18-10-2013]

                #region  Begin Code For Declaring Variables as on [18-10-2013]

                #region  Begin Code For Variable Declaration Which is Stored into Contracts Table as on [18-10-2013]

                #region  Begin Code For V-D   (1) to (5)  : Ref Clientid To ContractID

                var ClientId = "";
                var ContractStartDate = "01/01/1900";
                var ContractEndDate = "01/01/1900";
                var BGAmount = "0";
                var ContractId = "";

                #endregion  End Code For V-D   (1) to (5)  : Ref Clientid To ContractID

                #region  Begin Code For V-D   (6) to (10)  : Ref Security Deposit To Machinary Cost Per Month

                var SecurityDeposit = "0";
                var TypeOfWork = "";
                var MaterialCostPerMonth = "0";
                var ValidityDate = "01/01/1900";
                var MachinaryCostPerMonth = "0";

                #endregion  End Code For V-D   (6) to (10)  : Ref Security Deposit To Machinary Cost Per Month

                #region  Begin Code For V-D   (11) to (15)  : Ref EMD Value To PerFormance Guarantee

                var EMDValue = "0";
                var PaymentType = "0";
                var WageAct = "0";
                var PayLumpsum = "0";
                var PerformanceGuaranty = "0";

                #endregion  End Code For V-D   (11) to (15)  : Ref EMD Value To PerFormance Guarantee

                #region  Begin Code For V-D     : Ref PF To ESI On OT

                var PF = "0";
                var PFFrom = 0;
                var PFonOT = 0;
                var ESI = "0";
                var ESIFrom = 0;
                var ESIonOt = 0;

                var Pflimit = "0";
                var Esilimit = "0";
                var Bpf = 0;
                var Besi = 0;
                var RelChrg = 0;

                #endregion  End Code For V-D     : Ref PF To ESI On OT

                #region    Begin Code For V-D     : Ref    Servicharge  Yes To   Service Chagre Amount
                var ServiceChargeType = 0;
                var ServiceCharge = "0";
                var BillDates = 0;
                var PaySheetDates = 0;
                var WageType = 0;
                var ProfTax = 0;
                var SProfTax = 0;
                #endregion   End  Code For V-D     : Ref    Servicharge  Yes To   Service Chagre Amount

                #region    Begin Code For V-D     : Ref    ServiceTaxType To   TL No

                var ServiceTaxType = 0;
                var IncludeST = 0;
                var ServiceTax75 = 0;
                var OTPersent = 0;
                var OAPercent = 0;
                var OWF = "";
                var OTAmounttype = 0;
                var Description = "";
                var ContractDescription = "";
                var otsalaryratecheck = 0;
                var otsalaryrat = "0";
                var ojt = 0;
                var TL = 0;
                var TLNo = "0";
                var CGST = 0;
                var SGST = 0;
                var IGST = 0;
                var Cess1 = 0;
                var Cess2 = 0;
                var GSTLineItem = 0;
                #endregion   End  Code For V-D     : Ref       ServiceTaxType To   Description

                #region    Begin Code For V-D     : Ref    New Field add in Contract on 29/03/2014 by venkat

                var Tds = 0;
                var Pono = "";
                var ReceiptExpectedDate = 0;
                var Staxonservicecharge = 0;
                var Lumpsumtext = "";

                #endregion   End  Code For V-D     : Ref       New Field add in Contract on 29/03/2014 by venkat

                #region Begin Code for Esi branche adding as on 02/08/2014
                var Esibranch = "0";
                var pdfs = 0;
                var RoundOff = 0;
                var NoNhsWoDed = 0;
                var NoGeneralDed = 0;
                var NoUniformDed = 0;
                var NoSecDepDed = 0;
                var NoOtherDed = 0;
                var Grandtotwroff = 0;


                #endregion

                #region Begin code For Stored Procedure related Variables declaration as on [18-10-2013]
                Hashtable HtContracts = new Hashtable();
                string SPName = "";
                var IRecordStatus = 0;
                #endregion  End code For Stored Procedure related Variables declaration as on [18-10-2013]

                #endregion End  Code For Variable Declaration Which is Stored into Contracts Table as on [18-10-2013]

                #endregion End Code For Declaring Variables as on [18-10-2013]


                #region  Begin Code For Assign Values to The Variables as on [18-10-2013]

                #region Begin Code For A-V (1) to (5)  Ref : ClientId To ConractID
                ClientId = ddlclientid.SelectedValue;
                //ContractStartDate = DateTime.Parse(txtStartingDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                ContractStartDate = Timings.Instance.CheckDateFormat(txtStartingDate.Text);
                //ContractEndDate = DateTime.Parse(txtEndingDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                ContractEndDate = Timings.Instance.CheckDateFormat(txtEndingDate.Text);

                BGAmount = txtBGAmount.Text;
                ContractId = txtcontractid.Text;

                #endregion End Code For A-V (1) to (5)  Ref :ClientID To ContractID

                #region Begin Code For A-V (6) to (10)  Ref : Security Deposit To Machinary Cose Per Month
                SecurityDeposit = txtSecurityDeposit.Text;
                TypeOfWork = txtTypeOfWork.Text;
                MaterialCostPerMonth = txtMaterial.Text;
                if (txtValidityDate.Text.Trim().Length != 0)
                {
                    //ValidityDate = DateTime.Parse(txtValidityDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                    ValidityDate = Timings.Instance.CheckDateFormat(txtValidityDate.Text);

                }

                MachinaryCostPerMonth = txtMachinary.Text;

                #endregion End Code For A-V (6) to (10)  Ref : Security Deposit To Machinary Cose Per Month

                #region Begin Code For A-V (11) to (15)  Ref : EMD Value To PerFormance Guarantee

                EMDValue = txtEMDValue.Text;
                if (RadioLumpsum.Checked)
                {
                    PaymentType = "1";
                    if (txtLumpsumtext.Text.Trim().Length > 0)
                    {
                        Lumpsumtext = txtLumpsumtext.Text;
                    }
                }

                WageAct = txtWAWA.Text;
                if (RadioLumpsum.Checked)
                    PayLumpsum = txtlampsum.Text.Trim();
                PerformanceGuaranty = txtPerGurante.Text;

                #endregion End Code For A-V (11) to (15)  Ref : EMD Value To PerFormance Guarantee

                #region Begin Code For A-V   Ref : Ref PF To ESI On OT

                if (TxtPf.Text.Trim().Length > 0)
                {
                    PF = TxtPf.Text;
                }

                PFFrom = DdlPf.SelectedIndex;

                //if (checkPFonOT.Checked)
                //    PFonOT = 1;
                PFonOT = ddlpfon.SelectedIndex;
                if (TxtEsi.Text.Trim().Length > 0)
                {
                    ESI = TxtEsi.Text;
                }
                ESIFrom = DdlEsi.SelectedIndex;

                // if (checkESIonOT.Checked)
                //    ESIonOt = 1;
                ESIonOt = ddlesion.SelectedIndex;

                if (Chkpf.Checked)
                    Bpf = 1;

                if (ChkEsi.Checked)
                    Besi = 1;

                if (txtPfLimit.Text.Trim().Length > 0)
                    Pflimit = txtPfLimit.Text;
                if (txtEsiLimit.Text.Trim().Length > 0)
                    Esilimit = txtEsiLimit.Text;
                if (chkrc.Checked)
                    RelChrg = 1;


                #endregion End Code For A-V   Ref : Ref PF To ESI On OT

                #region Begin Code For A-V   Ref : Ref  ServiceChargeType  To SProfTax

                if (radioyes.Checked)
                {
                    if (RadioPercent.Checked)
                    {
                        ServiceCharge = txtservicecharge.Text.Trim();
                        ServiceChargeType = 0;
                    }
                    else
                    {
                        ServiceCharge = txtservicecharge.Text.Trim();
                        ServiceChargeType = 1;
                    }

                    if (chkStaxonservicecharge.Checked)
                    {
                        Staxonservicecharge = 1;
                    }

                }

                //if (RadioStartDate.Checked == true)
                //    BillDates = 1;
                BillDates = ddlbilldates.SelectedIndex;
                PaySheetDates = ddlPaySheetDates.SelectedIndex;

                if (RadioCompany.Checked)
                {
                    WageType = 0;
                }
                else if (RadioClient.Checked)
                {
                    WageType = 1;
                }
                else
                {
                    WageType = 2;
                }

                if (chkProfTax.Checked)
                    ProfTax = 1;

                if (chkspt.Checked)
                    SProfTax = 1;

                #endregion End Code For A-V   Ref : Ref  ServiceChargeType  To SProfTax

                #region Begin Code For A-V   Ref : Ref  ServiceTaxType  To TL No

                if (RadioWithoutST.Checked)
                {
                    ServiceTaxType = 1;
                }

                if (CheckIncludeST.Checked)
                {
                    IncludeST = 1;
                }

                if (Check75ST.Checked)
                {
                    ServiceTax75 = 1;
                }
                #region for GST 30-6-2017 by sharu

                if (chkCGST.Checked)
                {
                    CGST = 1;
                }

                if (RdbSGST.Checked)
                {
                    SGST = 1;
                }

                if (RdbIGST.Checked)
                {
                    IGST = 1;
                }

                if (chkCess1.Checked)
                {
                    Cess1 = 1;
                }

                if (chkCess2.Checked)
                {
                    Cess2 = 1;
                }

                if (chkCess2.Checked)
                {
                    Cess2 = 1;
                }

                if (chkGSTLineItem.Checked)
                {
                    GSTLineItem = 1;
                }

                #endregion for GST 30-6-2017 by sharu
                if (DdlOt.SelectedIndex == 0)
                    OTPersent = 100;
                else
                    OTPersent = 200;

                if (ddlOAPer.SelectedIndex == 0)
                {
                    OAPercent = 100;
                }
                else
                {
                    OAPercent = 200;
                }


                OWF = txtOWF.Text;
                if (radiootspecial.Checked)
                {
                    OTAmounttype = 1;
                }
                Description = txtdescription.Text.Trim();
                ContractDescription = TxtContractDescription.Text;

                if (chkotsalaryrate.Checked)
                {
                    otsalaryratecheck = 1;
                    otsalaryrat = txtotsalaryrate.Text;
                }
                if (chkojt.Checked)
                {
                    ojt = 1;
                }

                if (chktl.Checked)
                {
                    TL = 1;
                    TLNo = txttlamt.Text;
                }


                #endregion End Code For A-V   Ref : Ref  ServiceTaxType  To Description


                #region Begin Code For A-V   Ref : Ref  Tds  To Expect date on 29/03/2014 by venkat

                if (txtTds.Text.Trim().Length > 0)
                {
                    Tds = int.Parse(txtTds.Text);
                }

                if (txtPono.Text.Trim().Length > 0)
                {
                    Pono = txtPono.Text;
                }
                if (txtExpectdateofreceipt.Text.Trim().Length > 0)
                {
                    ReceiptExpectedDate = int.Parse(txtExpectdateofreceipt.Text);
                }

                #endregion End Code For A-V   Ref : Ref   Tds  To Expecte date

                #region Begin Code for Esi branche adding as on 02/08/2014

                if (ddlEsibranch.SelectedIndex > 0)
                {
                    Esibranch = ddlEsibranch.SelectedValue;
                }

                if (Chkpdfs.Checked)
                {

                    pdfs = 1;
                }

                if (ChkRoundOff.Checked)
                {
                    RoundOff = 1;
                }

                if (Chkgrandtotwroff.Checked)
                {

                    Grandtotwroff = 1;
                }

                if (chkNoNhsWoDed.Checked)
                {

                    NoNhsWoDed = 1;
                }
                if (chkUniformDed.Checked)
                {

                    NoUniformDed = 1;
                }
                if (chkSecDepDed.Checked)
                {

                    NoSecDepDed = 1;
                }
                if (chkOtherDed.Checked)
                {

                    NoOtherDed = 1;
                }
                if (chkGeneralDed.Checked)
                {

                    NoGeneralDed = 1;
                }


                #endregion

                var PaymentDates = "0";
                PaymentDates = ddlpaymentdates.SelectedValue;

                var PFSpl = 0;

                if (ChkPFSpl.Checked)
                {
                    PFSpl = 1;
                }

                #endregion   End  Code For Assign Values to The Variables as on [18-10-2013]


                #region Begin Code For Hash Table/Sp Parameters As on [18-10-2013]
                #region  Begin Code For H-S-Parameters   (1) to (5)  : Ref Clientid To ContractID

                HtContracts.Add("@ClientId", ClientId);
                HtContracts.Add("@ContractStartDate", ContractStartDate);
                HtContracts.Add("@ContractEndDate", ContractEndDate);
                HtContracts.Add("@BGAmount", BGAmount);
                HtContracts.Add("@ContractId", ContractId);

                #endregion  End Code For  H-S-Parameters   (1) to (5)  : Ref Clientid To ContractID

                #region  Begin Code For H-S-Parameters    (6) to (10)  : Ref Security Deposit To Machinary Cost Per Month

                HtContracts.Add("@SecurityDeposit", SecurityDeposit);
                HtContracts.Add("@TypeOfWork", TypeOfWork);
                HtContracts.Add("@MaterialCostPerMonth", MaterialCostPerMonth);
                HtContracts.Add("@ValidityDate", ValidityDate);
                HtContracts.Add("@MachinaryCostPerMonth", MachinaryCostPerMonth);

                #endregion  End Code For H-S-Parameters    (6) to (10)  : Ref Security Deposit To Machinary Cost Per Month

                #region  Begin Code For H-S-Parameters    (11) to (15)  : Ref EMD Value To PerFormance Guarantee

                HtContracts.Add("@EMDValue", EMDValue);
                HtContracts.Add("@PaymentType", PaymentType);
                HtContracts.Add("@WageAct", WageAct);
                HtContracts.Add("@PayLumpsum", PayLumpsum);
                HtContracts.Add("@PerformanceGuaranty", PerformanceGuaranty);

                #endregion  End Code For H-S-Parameters    (11) to (15)  : Ref EMD Value To PerFormance Guarantee

                #region  Begin Code For H-S-Parameters     : Ref PF To ESI On OT

                HtContracts.Add("@PF", PF);
                HtContracts.Add("@PFFrom", PFFrom);
                HtContracts.Add("@PFonOT", PFonOT);
                HtContracts.Add("@ESI", ESI);
                HtContracts.Add("@ESIFrom", ESIFrom);
                HtContracts.Add("@ESIonOt", ESIonOt);


                #endregion  End Code For H-S-Parameters      : Ref PF To ESI On OT


                #region    Begin Code For H-S-Parameters     : Ref    Servicharge  Yes To   Service Chagre Amount

                HtContracts.Add("@ServiceChargeType", ServiceChargeType);
                HtContracts.Add("@ServiceCharge", ServiceCharge);
                HtContracts.Add("@BillDates", BillDates);
                HtContracts.Add("@PaySheetDate", PaySheetDates);
                HtContracts.Add("@WageType", WageType);
                HtContracts.Add("@ProfTax", ProfTax);
                HtContracts.Add("@SProfTax", SProfTax);

                #endregion   End  Code For   H-S-Parameters    : Ref    Servicharge  Yes To   Service Chagre Amount


                #region    Begin Code For H-S-Parameters    : Ref    ServiceTaxType To   TL No

                HtContracts.Add("@ServiceTaxType", ServiceTaxType);
                HtContracts.Add("@IncludeST", IncludeST);
                HtContracts.Add("@ServiceTax75", ServiceTax75);
                HtContracts.Add("@OTPersent", OTPersent);
                HtContracts.Add("@OAPercent", OAPercent);
                HtContracts.Add("@OWF", OWF);
                HtContracts.Add("@OTAmounttype", OTAmounttype);
                HtContracts.Add("@Description", Description);
                HtContracts.Add("@ContractDescription", ContractDescription);
                HtContracts.Add("@otsalaryratecheck", otsalaryratecheck);
                HtContracts.Add("@otsalaryrat", otsalaryrat);
                HtContracts.Add("@ojt", ojt);
                HtContracts.Add("@tl", TL);
                HtContracts.Add("@tlno", TLNo);
                HtContracts.Add("@PFLimit", Pflimit);
                HtContracts.Add("@ESILimit", Esilimit);
                HtContracts.Add("@Bpf", Bpf);
                HtContracts.Add("@Besi", Besi);
                HtContracts.Add("@RelChrg", RelChrg);
                HtContracts.Add("@CGST", CGST);
                HtContracts.Add("@IGST", IGST);
                HtContracts.Add("@SGST", SGST);
                HtContracts.Add("@Cess1", Cess1);
                HtContracts.Add("@Cess2", Cess2);
                HtContracts.Add("@GSTLineItem", GSTLineItem);

                #endregion   End  Code For H-S-Parameters    : Ref       ServiceTaxType To   Description


                #region    Begin Code For H-S-Parameters    : Ref    Tds To   Lumpsumtext on 29/03/2014 by venkat

                HtContracts.Add("@Tds", Tds);
                HtContracts.Add("@Pono", Pono);
                HtContracts.Add("@ReceiptExpectedDate", ReceiptExpectedDate);
                HtContracts.Add("@Staxonservicecharge", Staxonservicecharge);
                HtContracts.Add("@Lumpsumtext", Lumpsumtext);

                #endregion   End  Code For H-S-Parameters    : Ref        Tds To   Lumpsumtext on 29/03/2014 by venkat


                HtContracts.Add("@Esibranch", Esibranch);
                HtContracts.Add("@pdfs", pdfs);
                HtContracts.Add("@RoundOff", RoundOff);
                HtContracts.Add("@NoNhsWoDed", NoNhsWoDed);
                HtContracts.Add("@NoGeneralDed", NoGeneralDed);
                HtContracts.Add("@NoUniformDed", NoUniformDed);
                HtContracts.Add("@NoSecDepDed", NoSecDepDed);
                HtContracts.Add("@NoOtherDed", NoOtherDed);
                HtContracts.Add("@PaymentDates", PaymentDates);
                HtContracts.Add("@PFSpl", PFSpl);
                HtContracts.Add("@Grandtotwroff", Grandtotwroff);

                #endregion  End  Code For Hash Table/Sp Parameters As on [18-10-2013]

                #region Begin Code For Calling Stored Procedure as on [18-10-2013]
                SPName = "AddorModifyContracts";
                IRecordStatus = config.ExecuteNonQueryParamsAsync(SPName, HtContracts).Result;
                #endregion End Code For Calling Stored Procedure as on [18-10-2013]




                if (IRecordStatus > 0)
                {
                    lblSuc.Text = "Contract Added Successfully";

                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Contract Added Successfully ');", true);
                    int j = 0;

                    DateTime today = DateTime.Now.Date;
                    int designCount = Convert.ToInt16(Session["ContractsAIndex"]);
                    //Store Data into Contract Details
                    int clientdesigncount = 0;
                    for (j = 0; j < designCount; j++)
                    {
                        if (j < gvdesignation.Rows.Count)
                        {

                            HtContracts.Clear();

                            #region Clientdesignation
                            string Cddldesignation = ((DropDownList)gvdesignation.Rows[j].FindControl("DdlDesign")).SelectedValue;
                            DropDownList ddlindex = gvdesignation.Rows[j].FindControl("DdlDesign") as DropDownList;
                            if (ddlindex.SelectedIndex == 0)
                            {
                                break;
                            }
                            #endregion


                            #region   Duty Hrs
                            string Cdutyhrs = ((TextBox)gvdesignation.Rows[j].FindControl("txtdutyhrs")).Text;
                            if (Cdutyhrs.Trim().Length == 0)
                            {
                                Cdutyhrs = "";
                            }
                            #endregion

                            #region   Quantity
                            string Cquantity = ((TextBox)gvdesignation.Rows[j].FindControl("txtquantity")).Text;
                            if (Cquantity.Trim().Length == 0)
                            {
                                //lblmsgcontractdetails.Text = "Please enter No. of employee needed";
                                // break;
                                //Cquantity = "0";
                            }
                            else
                            {
                                float tempQty = Convert.ToSingle(Cquantity);
                                if (tempQty < 0)
                                {
                                    lblMsg.Text = "No. of employee needed can't be  negative";
                                    break;
                                }
                            }
                            #endregion
                            //string Cddldutytype = ((DropDownList)gvdesignation.Rows[j].FindControl("ddldutytype")).Text;
                            #region PayType
                            DropDownList ddlPayType = gvdesignation.Rows[j].FindControl("ddldutytype") as DropDownList;
                            int PayType = ddlPayType.SelectedIndex;
                            #endregion

                            #region  No Of Days For Billing
                            DropDownList ddlNoOfDaysForBilling = gvdesignation.Rows[j].FindControl("ddlNoOfDaysBilling") as DropDownList;
                            var NoOfDaysForBilling = "0";

                            if (ddlNoOfDaysForBilling.SelectedIndex == 0)
                            {
                                NoOfDaysForBilling = "0";
                            }
                            if (ddlNoOfDaysForBilling.SelectedIndex == 1)
                            {
                                NoOfDaysForBilling = "1";
                            }


                            if (ddlNoOfDaysForBilling.SelectedIndex > 1)
                            {
                                NoOfDaysForBilling = ddlNoOfDaysForBilling.SelectedValue.ToString();
                            }
                            #endregion

                            #region  No Of Ots
                            string NoOfOts = "0";
                            DropDownList ddlNoOfOtsPaysheet = gvdesignation.Rows[j].FindControl("ddlNoOfOtsPaysheet") as DropDownList;

                            if (ddlNoOfOtsPaysheet.SelectedIndex == 0)
                            {
                                NoOfOts = "0";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 1)
                            {
                                NoOfOts = "1";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 2)
                            {
                                NoOfOts = "2";
                            }

                            if (ddlNoOfOtsPaysheet.SelectedIndex > 2)
                            {
                                NoOfOts = ddlNoOfOtsPaysheet.SelectedValue;
                            }
                            #endregion No Of Ots

                            var cdCGST = 0;

                            CheckBox chkcdCGST = gvdesignation.Rows[j].FindControl("chkcdCGST") as CheckBox;
                            if (chkcdCGST.Checked)
                            {
                                cdCGST = 1;
                            }


                            var cdSGST = 0;

                            CheckBox chkcdSGST = gvdesignation.Rows[j].FindControl("chkcdSGST") as CheckBox;
                            if (chkcdSGST.Checked)
                            {
                                cdSGST = 1;
                            }


                            var cdIGST = 0;

                            CheckBox chkcdIGST = gvdesignation.Rows[j].FindControl("chkcdIGST") as CheckBox;
                            if (chkcdIGST.Checked)
                            {
                                cdIGST = 1;
                            }

                            string HSNNumber = ((DropDownList)gvdesignation.Rows[j].FindControl("ddlHSNNumber")).SelectedValue;

                            #region Summary
                            string Csummary = ((TextBox)gvdesignation.Rows[j].FindControl("txtsummary")).Text;
                            //string Camount = ((TextBox)gvdesignation.Rows[j].FindControl("txtamount")).Text;
                            if (Csummary.Trim().Length == 0)
                            {
                                Csummary = "";
                            }
                            #endregion

                            #region  PayRate
                            string strPayRate = ((TextBox)gvdesignation.Rows[j].FindControl("txtPayRate")).Text;
                            if (RadioLumpsum.Checked == false)
                            {
                                if (strPayRate.Trim().Length == 0)
                                {
                                    lblMsg.Text = "Please enter Pay Rate for employee";
                                    break;
                                    //Cquantity = "0";
                                }
                                else
                                {
                                    float tempPay = Convert.ToSingle(strPayRate);
                                    if (tempPay <= 0)
                                    {
                                        lblMsg.Text = "Pay Rate of employee can't be 0 or negative";
                                        break;
                                    }
                                }
                            }
                            else
                                strPayRate = "0";

                            #endregion

                            #region  Basic
                            string Cbasic = ((TextBox)gvdesignation.Rows[j].FindControl("TxtBasic")).Text;
                            if (RadioLumpsum.Checked == false)
                            {
                                if (Cbasic.Trim().Length == 0)
                                {
                                    if (RadioClient.Checked)
                                    {
                                        lblMsg.Text = "Please enter basic pay for employee";
                                        break;
                                    }
                                    Cbasic = "0";
                                }
                                else
                                {
                                    if (RadioClient.Checked)
                                    {
                                        float tempBaic = Convert.ToSingle(Cbasic);
                                        if (tempBaic <= 0)
                                        {
                                            lblMsg.Text = "Basic pay can't be 0 or negative";
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                                Cbasic = "0";

                            #endregion

                            #region Da
                            string Cda = ((TextBox)gvdesignation.Rows[j].FindControl("txtda")).Text;
                            if (Cda.Trim().Length == 0)
                            {
                                Cda = "0";
                            }
                            #endregion

                            var LAType = 0;
                            DropDownList ddlLAtype = gvdesignation.Rows[j].FindControl("ddlLAtype") as DropDownList;
                            LAType = ddlLAtype.SelectedIndex;

                            var bonustype = 0;
                            DropDownList ddlbonustype = gvdesignation.Rows[j].FindControl("ddlbonustype") as DropDownList;
                            bonustype = ddlbonustype.SelectedIndex;

                            #region Hra
                            string Chra = ((TextBox)gvdesignation.Rows[j].FindControl("txthra")).Text;
                            if (Chra.Trim().Length == 0)
                            {
                                Chra = "0";
                            }
                            #endregion

                            #region Conveyance
                            string CConveyance = ((TextBox)gvdesignation.Rows[j].FindControl("txtConveyance")).Text;
                            if (CConveyance.Trim().Length == 0)
                            {
                                CConveyance = "0";
                            }
                            #endregion

                            #region Other Allowance
                            string Caw = ((TextBox)gvdesignation.Rows[j].FindControl("txtoa")).Text;
                            if (Caw.Trim().Length == 0)
                            {
                                Caw = "0";
                            }
                            #endregion

                            #region Wash Allowance
                            string Cwashallowance = ((TextBox)gvdesignation.Rows[j].FindControl("txtwa")).Text;
                            if (Cwashallowance.Trim().Length == 0)
                            {
                                Cwashallowance = "0";
                            }
                            #endregion

                            #region CCA
                            string Cca = ((TextBox)gvdesignation.Rows[j].FindControl("txtcca")).Text;
                            if (Cca.Trim().Length == 0)
                            {
                                Cca = "0";
                            }
                            #endregion

                            #region LeaveAmount

                            string LeaveAmount = ((TextBox)gvdesignation.Rows[j].FindControl("txtleaveamount")).Text;
                            if (LeaveAmount.Trim().Length == 0)
                            {
                                LeaveAmount = "0";
                            }
                            #endregion

                            #region Gratituty
                            string Gratuty = ((TextBox)gvdesignation.Rows[j].FindControl("txtgratuty")).Text;
                            if (Gratuty.Trim().Length == 0)
                            {
                                Gratuty = "0";
                            }
                            #endregion

                            #region Bonus
                            string Bonus = ((TextBox)gvdesignation.Rows[j].FindControl("txtbonus")).Text;
                            if (Bonus.Trim().Length == 0)
                            {
                                Bonus = "0";
                            }

                            string AttBonus = ((TextBox)gvdesignation.Rows[j].FindControl("txtattbonus")).Text;
                            if (AttBonus.Trim().Length == 0)
                            {
                                AttBonus = "0";
                            }
                            #endregion

                            #region for Uniform
                            string Uniform = ((TextBox)gvdesignation.Rows[j].FindControl("txtuniform")).Text;
                            if (Uniform.Trim().Length == 0)
                            {
                                Uniform = "0";
                            }
                            #endregion for Uniform

                            #region NFhs
                            string Nfhsw = ((TextBox)gvdesignation.Rows[j].FindControl("txtNfhs")).Text;
                            if (Nfhsw.Trim().Length == 0)
                            {
                                Nfhsw = "0";
                            }


                            #endregion


                            #region BEgin RC
                            string RC = ((TextBox)gvdesignation.Rows[j].FindControl("Txtrc")).Text;
                            if (RC.Trim().Length == 0)
                            {
                                RC = "0";
                            }

                            #endregion End Rc



                            #region Begin CS
                            string CS = ((TextBox)gvdesignation.Rows[j].FindControl("TxtCs")).Text;
                            if (CS.Trim().Length == 0)
                            {
                                CS = "0";
                            }

                            string CSper = ((TextBox)gvdesignation.Rows[j].FindControl("TxtScPer")).Text;
                            if (CSper.Trim().Length == 0)
                            {
                                CSper = "0";
                            }

                            #endregion End CS

                            #region Begin OT RATE
                            string OTRATE = ((TextBox)gvdesignation.Rows[j].FindControl("TxtOTRate")).Text;
                            if (OTRATE.Trim().Length == 0)
                            {
                                OTRATE = "0";
                            }
                            #endregion End OT RATE

                            double payRate = double.Parse(strPayRate);

                            #region  STored Procedure Parameters , connection Strings
                            SPName = "GetContractDetails";
                            #region Parameters 1 -8
                            HtContracts.Add("@clientid", ClientId);
                            HtContracts.Add("@Contractid", ContractId);
                            HtContracts.Add("@Designations", Cddldesignation);
                            HtContracts.Add("@DutyHrs", Cdutyhrs);
                            HtContracts.Add("@Quantity", Cquantity);
                            HtContracts.Add("@basic", Cbasic);
                            HtContracts.Add("@da", Cda);
                            HtContracts.Add("@hra", Chra);
                            HtContracts.Add("@conveyance", CConveyance);

                            #endregion

                            #region Parameters 8-16
                            HtContracts.Add("@washallownce", Cwashallowance);
                            HtContracts.Add("@OtherAllowance", Caw);
                            HtContracts.Add("@Summary", Csummary);
                            HtContracts.Add("@Amount", payRate);
                            #endregion

                            #region Parameters 16-24
                            HtContracts.Add("@cca", Cca);
                            HtContracts.Add("@leaveamount", LeaveAmount);
                            HtContracts.Add("@bonus", Bonus);
                            HtContracts.Add("@gratuity", Gratuty);
                            HtContracts.Add("@PayType", PayType);
                            HtContracts.Add("@NoOfDays", NoOfDaysForBilling);
                            HtContracts.Add("@NFhs", Nfhsw);
                            HtContracts.Add("@testrecord", clientdesigncount);

                            HtContracts.Add("@Nots", NoOfOts);
                            HtContracts.Add("@RC", RC);
                            HtContracts.Add("@CS", CS);
                            HtContracts.Add("@OTRATE", OTRATE);
                            HtContracts.Add("@attbonus", AttBonus);
                            HtContracts.Add("@servicechargeper", CSper);
                            HtContracts.Add("@Uniform", Uniform);
                            HtContracts.Add("@BonusType", bonustype);
                            HtContracts.Add("@LAType", LAType);

                            HtContracts.Add("@cdCGST", cdCGST);
                            HtContracts.Add("@cdSGST", cdSGST);
                            HtContracts.Add("@cdIGST", cdIGST);
                            HtContracts.Add("@HSNNumber", HSNNumber);
                            HtContracts.Add("@SNo", j + 1);

                            #endregion

                            IRecordStatus = config.ExecuteNonQueryParamsAsync(SPName, HtContracts).Result;
                            if (IRecordStatus != 0)
                            {
                                clientdesigncount++;
                            }
                            #endregion

                        }
                    }

                    #region   Contract Special Wise Designations 

                    int designCountsw = Convert.ToInt16(Session["ContractsAIndexsw"]);
                    int specialdesigncount = 0;
                    for (j = 0; j < designCountsw; j++)
                    {
                        if (j < gvSWDesignations.Rows.Count)
                        {
                            HtContracts.Clear();
                            #region  Client Designations
                            string Cddldesignationsw = ((DropDownList)gvSWDesignations.Rows[j].FindControl("DdlDesign")).SelectedValue;
                            DropDownList ddlindexsw = gvSWDesignations.Rows[j].FindControl("DdlDesign") as DropDownList;
                            if (ddlindexsw.SelectedIndex == 0)
                            {
                                break;
                            }
                            #endregion

                            #region  Begin  No Of Days For Wages
                            DropDownList ddlNoOfDaysForWages = gvSWDesignations.Rows[j].FindControl("ddlNoOfDaysWages") as DropDownList;
                            string NoOfDaysForWages = "0";

                            if (ddlNoOfDaysForWages.SelectedIndex == 0)
                            {
                                NoOfDaysForWages = "0";
                            }
                            if (ddlNoOfDaysForWages.SelectedIndex == 1)
                            {
                                NoOfDaysForWages = "1";
                            }
                            if (ddlNoOfDaysForWages.SelectedIndex == 2)
                            {
                                NoOfDaysForWages = "2";
                            }
                            if (ddlNoOfDaysForWages.SelectedIndex == 3)
                            {
                                NoOfDaysForWages = "3";
                            }
                            if (ddlNoOfDaysForWages.SelectedIndex == 4)
                            {
                                NoOfDaysForWages = "4";
                            }
                            if (ddlNoOfDaysForWages.SelectedIndex == 5)
                            {
                                NoOfDaysForWages = "5";
                            }
                            if (ddlNoOfDaysForWages.SelectedIndex == 6)
                            {
                                NoOfDaysForWages = "6";
                            }

                            if (ddlNoOfDaysForWages.SelectedIndex > 6)
                            {
                                NoOfDaysForWages = ddlNoOfDaysForWages.SelectedValue;
                            }
                            #endregion  //End  No Of Days For Wages

                            #region  No Of Ots
                            string NoOfOts = "0";
                            DropDownList ddlNoOfOtsPaysheet = gvSWDesignations.Rows[j].FindControl("ddlNoOfOtsPaysheet") as DropDownList;

                            if (ddlNoOfOtsPaysheet.SelectedIndex == 0)
                            {
                                NoOfOts = "0";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 1)
                            {
                                NoOfOts = "1";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 2)
                            {
                                NoOfOts = "2";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 3)
                            {
                                NoOfOts = "3";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 4)
                            {
                                NoOfOts = "4";
                            }

                            if (ddlNoOfOtsPaysheet.SelectedIndex == 5)
                            {
                                NoOfOts = "5";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex > 5)
                            {
                                NoOfOts = ddlNoOfOtsPaysheet.SelectedValue;
                            }
                            #endregion No Of Ots

                            #region  No Of NHS
                            string NoOfNHS = "0";
                            DropDownList ddlNoOfNhsPaysheet = gvSWDesignations.Rows[j].FindControl("ddlNoOfNhsPaysheet") as DropDownList;

                            if (ddlNoOfNhsPaysheet.SelectedIndex == 0)
                            {
                                NoOfNHS = "0";
                            }
                            if (ddlNoOfNhsPaysheet.SelectedIndex == 1)
                            {
                                NoOfNHS = "1";
                            }
                            if (ddlNoOfNhsPaysheet.SelectedIndex == 2)
                            {
                                NoOfNHS = "2";
                            }
                            if (ddlNoOfNhsPaysheet.SelectedIndex == 3)
                            {
                                NoOfNHS = "3";
                            }
                            if (ddlNoOfNhsPaysheet.SelectedIndex == 4)
                            {
                                NoOfNHS = "4";
                            }

                            if (ddlNoOfNhsPaysheet.SelectedIndex == 5)
                            {
                                NoOfNHS = "5";
                            }
                            if (ddlNoOfNhsPaysheet.SelectedIndex > 5)
                            {
                                NoOfNHS = ddlNoOfNhsPaysheet.SelectedValue;
                            }

                            string NHSRATE = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtNhsRate")).Text;
                            if (NHSRATE.Trim().Length == 0)
                            {
                                NHSRATE = "0";
                            }

                            #endregion No Of NHS

                            #region  Basic
                            string Cbasicsw = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtBasic")).Text;
                            if (Cbasicsw.Trim().Length == 0)
                            {
                                Cbasicsw = "0";
                            }

                            #endregion

                            #region DA
                            string Cdasw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtda")).Text;
                            if (Cdasw.Trim().Length == 0)
                            {
                                Cdasw = "0";
                            }
                            #endregion

                            #region  Hra
                            string Chrasw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txthra")).Text;
                            if (Chrasw.Trim().Length == 0)
                            {
                                Chrasw = "0";
                            }

                            #endregion

                            #region Conveyance
                            string CConveyancesw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtConveyance")).Text;
                            if (CConveyancesw.Trim().Length == 0)
                            {
                                CConveyancesw = "0";
                            }
                            #endregion

                            #region Other Allowance
                            string Cawsw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtoa")).Text;
                            if (Cawsw.Trim().Length == 0)
                            {
                                Cawsw = "0";
                            }
                            #endregion

                            #region Wash Allowance 

                            string Cwashallowancesw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtwa")).Text;
                            if (Cwashallowancesw.Trim().Length == 0)
                            {
                                Cwashallowancesw = "0";
                            }

                            #endregion

                            #region CCa
                            string Ccasw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtcca")).Text;
                            if (Ccasw.Trim().Length == 0)
                            {
                                Ccasw = "0";
                            }
                            #endregion

                            #region Leave Amount 
                            string LeaveAmountsw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtleaveamount")).Text;
                            if (LeaveAmountsw.Trim().Length == 0)
                            {
                                LeaveAmountsw = "0";
                            }
                            #endregion

                            var LAType = 0;
                            DropDownList ddlLAtype = gvSWDesignations.Rows[j].FindControl("ddlLAtype") as DropDownList;
                            LAType = ddlLAtype.SelectedIndex;

                            #region gratituty
                            string Gratutysw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtgratuty")).Text;
                            if (Gratutysw.Trim().Length == 0)
                            {
                                Gratutysw = "0";
                            }
                            #endregion

                            var Gratuitytype = 0;
                            DropDownList ddlGratuitytype = gvSWDesignations.Rows[j].FindControl("ddlGratuitytype") as DropDownList;
                            Gratuitytype = ddlGratuitytype.SelectedIndex;

                            #region Bonus 
                            string Bonussw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtbonus")).Text;
                            if (Bonussw.Trim().Length == 0)
                            {
                                Bonussw = "0";
                            }

                            string AttBonussw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtattbonus")).Text;
                            if (AttBonussw.Trim().Length == 0)
                            {
                                AttBonussw = "0";
                            }
                            #endregion

                            var bonustype = 0;
                            DropDownList ddlbonustype = gvSWDesignations.Rows[j].FindControl("ddlbonustype") as DropDownList;
                            bonustype = ddlbonustype.SelectedIndex;

                            #region NFHs
                            String Nfhssw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtNfhs1")).Text;
                            if (Nfhssw.Trim().Length == 0)
                            {
                                Nfhssw = "0";
                            }



                            #endregion

                            #region BEgin RC
                            string RC = ((TextBox)gvSWDesignations.Rows[j].FindControl("Txtrc")).Text;
                            if (RC.Trim().Length == 0)
                            {
                                RC = "0";
                            }

                            #endregion End Rc



                            #region Begin CS
                            string CS = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtCs")).Text;
                            if (CS.Trim().Length == 0)
                            {
                                CS = "0";
                            }

                            string CSpersw = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtScPer")).Text;
                            if (CSpersw.Trim().Length == 0)
                            {
                                CSpersw = "0";
                            }
                            #endregion End CS

                            #region Begin OT RATE
                            string OTRATE = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtOTRate")).Text;
                            if (OTRATE.Trim().Length == 0)
                            {
                                OTRATE = "0";
                            }



                            #endregion End OT RATE

                            #region  STored Procedure Parameters , connection Strings
                            SPName = "Insertcontractspecialdetails";

                            #region Parameters 1 -8
                            HtContracts.Add("@clientid", ClientId);
                            HtContracts.Add("@ContractId", ContractId);
                            // HtContracts.Add("@ClientID", ClientId);
                            HtContracts.Add("@Designations", Cddldesignationsw);
                            HtContracts.Add("@Basic", Cbasicsw);

                            HtContracts.Add("@DA", Cdasw);
                            HtContracts.Add("@HRA", Chrasw);
                            HtContracts.Add("@Conveyance", CConveyancesw);
                            HtContracts.Add("@WashAllownce", Cwashallowancesw);

                            #endregion

                            #region Parameters 8-16

                            HtContracts.Add("@OtherAllowance", Cawsw);
                            HtContracts.Add("@CCA", Ccasw);
                            HtContracts.Add("@LeaveAmount", LeaveAmountsw);
                            HtContracts.Add("@Bonus", Bonussw);
                            HtContracts.Add("@gratuity", Gratutysw);
                            #endregion

                            #region Parameters 16-22

                            HtContracts.Add("@NoOfDays", NoOfDaysForWages);
                            HtContracts.Add("@NFHs", Nfhssw);
                            HtContracts.Add("@testrecord", specialdesigncount);

                            HtContracts.Add("@Nots", NoOfOts);
                            HtContracts.Add("@RC", RC);
                            HtContracts.Add("@CS", CS);
                            HtContracts.Add("@OTRATE", OTRATE);
                            HtContracts.Add("@attbonus", AttBonussw);
                            HtContracts.Add("@servicechargeper", CSpersw);
                            HtContracts.Add("@LAType", LAType);
                            HtContracts.Add("@Gratuitytype", Gratuitytype);
                            HtContracts.Add("@bonustype", bonustype);
                            HtContracts.Add("@NoOfNHS", NoOfNHS);
                            HtContracts.Add("@NHSRATE", NHSRATE);
                            #endregion

                            IRecordStatus = config.ExecuteNonQueryParamsAsync(SPName, HtContracts).Result;
                            if (IRecordStatus != 0)
                            {
                                specialdesigncount++;
                            }
                            #endregion

                        }
                    }

                    #endregion
                }
                if (IRecordStatus != 0)
                {
                    contractidautogenrate();
                    clearcontractdetails();
                    Enable5Rows();
                    // lblreslt.Text = "Record Inserted Successfully";
                    Session["ContractsAIndex"] = 0;
                    Session["ContractsAIndexsw"] = 0;
                    ClearDataFromThePage();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }

            Session["ContractsAIndex"] = 0;
            Session["ContractsAIndexsw"] = 0;
        }

        private void contractidautogenrate()
        {
            txtcontractid.Text = GlobalData.Instance.LoadMaxContractId(ddlclientid.SelectedValue);
            DataTable DtContractIds = GlobalData.Instance.LoadAllContractIds(ddlclientid.SelectedValue);
            if (DtContractIds.Rows.Count > 0)
            {
                ddlContractids.DataValueField = "Contractid";
                ddlContractids.DataTextField = "Contractid";
                ddlContractids.DataSource = DtContractIds;
                ddlContractids.DataBind();
            }
            ddlContractids.Items.Insert(0, "-Select-");

        }

        //region for clone dropdown() 
        private void CloneContractidautogenrate()
        {
            txtclonecontractid.Text = GlobalData.Instance.LoadMaxContractId(ddlClientidNotincontract.SelectedValue);
            DataTable DtContractIds = GlobalData.Instance.LoadAllContractIds(ddlClientidNotincontract.SelectedValue);

            //if (DtContractIds.Rows.Count > 0)
            //{
            //    ddlContractids.DataValueField = "Contractid";
            //    ddlContractids.DataTextField = "Contractid";
            //    ddlContractids.DataSource = DtContractIds;
            //    ddlContractids.DataBind();
            //}
            //ddlContractids.Items.Insert(0, "-Select-");

        }

        //endregion for clone dropdown() 

        protected void gvdesignation_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
        }

        protected void RadioLumpsum_CheckedChanged1(object sender, EventArgs e)
        {
            if (RadioLumpsum.Checked == true)
            {
                txtlampsum.Visible = true;
                lbllampsum.Visible = true;
                txtLumpsumtext.Visible = true;
                lbllumpsumtext.Visible = true;

            }
            else
            {
                txtlampsum.Visible = false;
                txtLumpsumtext.Visible = false;
                lbllumpsumtext.Visible = false;
            }

        }

        protected void gvdesignation_RowCommand1(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvdesignation_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void radioyes_CheckedChanged(object sender, EventArgs e)
        {
            if (radioyes.Checked == true)
            {
                RadioPercent.Visible = true;
                RadioAmount.Visible = true;
                txtservicecharge.Visible = true;
                chkStaxonservicecharge.Visible = true;

            }
            else
            {
                RadioPercent.Visible = false;
                RadioAmount.Visible = false;

                txtservicecharge.Visible = false;
                chkStaxonservicecharge.Visible = false;
            }
        }

        protected void RdbSGST_CheckedChanged(object sender, EventArgs e)
        {
            if (RdbIGST.Checked)
            {
                chkCGST.Checked = false;
                chkCGST.Enabled = false;
            }
            else
            {
                chkCGST.Checked = true;
                chkCGST.Enabled = true;
            }
        }

        protected void chkcdIGST_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkcdIGST = sender as CheckBox;
            GridViewRow row = null;
            if (chkcdIGST == null)
                return;

            row = (GridViewRow)chkcdIGST.NamingContainer;
            if (row == null)
                return;


            CheckBox chkcdCGST = row.FindControl("chkcdCGST") as CheckBox;
            CheckBox chkcdSGST = row.FindControl("chkcdSGST") as CheckBox;


            if (chkcdIGST.Checked)
            {
                chkcdCGST.Checked = false;
                chkcdCGST.Enabled = false;

                chkcdSGST.Checked = false;
                chkcdSGST.Enabled = false;

            }
            else
            {
                chkcdCGST.Checked = true;
                chkcdCGST.Enabled = true;

                chkcdSGST.Checked = true;
                chkcdSGST.Enabled = true;

            }
        }
        protected void clearcontractdetails()
        {
            ddlclientid.SelectedIndex = 0;
            ddlcname.SelectedIndex = 0;
            checkPFonOT.Checked = false;
            checkESIonOT.Checked = false;
            TxtPf.Text = "100";
            TxtEsi.Text = "100";
            RadioCompany.Checked = true;
            RadioWithST.Checked = true;
            radiootregular.Checked = true;
            txtTypeOfWork.Text = "";
            radiono.Checked = true;
            Radio1to1.Checked = true;
            ddlEsibranch.SelectedIndex = 0;
            chkCGST.Checked = true;
            RdbSGST.Checked = true;
            RdbIGST.Checked = false;
            chkCess1.Checked = false;
            chkCess2.Checked = false;
            chkGSTLineItem.Checked = false;
        }

        protected void GetGridData()
        {
            //GlobalData.Instance.AppendLog("In GetGridDate");

            try
            {
                Session["DataContractsAIndex"] = 0;
                Session["DataContractsAIndexsw"] = 0;

                ClearDataFromThePage();

                DisplayDefaultRow();
                Displaydefaulrowsw();
                gvSWDesignations.Visible = true;
                gvdesignation.Visible = true;
                DateTime today = DateTime.Now.Date;

                #region Begin Old Code as on [19-10-2013]

                //string SqlQry = "Select * from contracts where Clientid='" + ddlclientid.SelectedValue + "' AND ContractStartDate <= '" +
                //    today.ToString("MM/dd/yyyy") + "' AND ContractEndDate>='" + today.ToString("MM/dd/yyyy") + "'";
                //DataTable DtContractsData = SqlHelper.Instance.GetTableByQuery(SqlQry);

                ////GlobalData.Instance.AppendLog("In GetGridDate- After quering for contracts");
                //if (DtContractsData.Rows.Count == 0)
                //{
                //    lblreslt.Text = "There is no valid Contract for the this Client";
                //    contractidautogenrate();
                //    Enable5Rows();
                //    return;
                //}
                #endregion End Old Code As on [19-10-2013]

                #region    Begin Code For Variable Declaration as on [19-10-2013]
                var SPName = "";
                var ClientID = "";
                Hashtable HtContracts = new Hashtable();
                #endregion  Begin Code For Variable Declaration as on [19-10-2013]

                #region    Begin Code For Assign Values To  Variable Declaration as on [19-10-2013]
                SPName = "GetContracts";
                ClientID = ddlclientid.SelectedValue; ;
                #endregion  Begin Code For Assign Values To  Variable Declaration as on [19-10-2013]

                #region Begin Code For Hash Table Parameters as on [19-10-2013]
                HtContracts.Add("@clientid", ClientID);
                HtContracts.Add("@Contractid", ddlContractids.SelectedValue);

                #endregion  End  Code For Hash Table Parameters as on [19-10-2013]

                #region  Begin Code For Calling Stored Procedure To Retrive Data from Contracts as on [19-10-2013]
                DataTable DtContractsData = config.ExecuteAdaptorAsyncWithParams(SPName, HtContracts).Result;
                #endregion End Code For Calling Stored Procedure To Retrive Data from Contracts as on [19-10-2013]

                #region  Begin Code Display the Resulted Messages As on [19-10-2013]
                if (DtContractsData.Rows.Count <= 0)
                {
                    lblMsg.Text = "There is no valid Contract for the this Client";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('There is no valid Contract for the this Client');", true);
                    contractidautogenrate();
                    Enable5Rows();
                    return;
                }

                #endregion  End  Code Display the Resulted Messages As on [19-10-2013]

                #region  Begin Code For Assign Values to The Controls as on [18-10-2013]

                #region Begin Code For V-C (1) to (5)  Ref : ClientId To ConractID
                // ClientId = ddlclientid.SelectedValue;
                txtStartingDate.Text = DateTime.Parse(DtContractsData.Rows[0]["ContractStartDate"].ToString()).ToString("dd/MM/yyyy");
                txtEndingDate.Text = DateTime.Parse(DtContractsData.Rows[0]["ContractEndDate"].ToString()).ToString("dd/MM/yyyy");
                txtBGAmount.Text = DtContractsData.Rows[0]["BGAmount"].ToString();
                txtcontractid.Text = DtContractsData.Rows[0]["ContractId"].ToString();

                #endregion End Code For V-C (1) to (5)  Ref :ClientID To ContractID

                #region Begin Code For V-C (6) to (10)  Ref : Security Deposit To Machinary Cose Per Month
                txtSecurityDeposit.Text = DtContractsData.Rows[0]["SecurityDeposit"].ToString();
                txtTypeOfWork.Text = DtContractsData.Rows[0]["TypeOfWork"].ToString();
                txtMaterial.Text = DtContractsData.Rows[0]["MaterialCostPerMonth"].ToString();
                txtValidityDate.Text = DateTime.Parse(DtContractsData.Rows[0]["ValidityDate"].ToString()).ToString("dd/MM/yyyy");
                if (txtValidityDate.Text == "01/01/1900")
                {
                    txtValidityDate.Text = "";
                }

                txtMachinary.Text = DtContractsData.Rows[0]["MachinaryCostPerMonth"].ToString();

                #endregion End Code For V-C (6) to (10)  Ref : Security Deposit To Machinary Cose Per Month

                #region Begin Code For V-C (11) to (15)  Ref : EMD Value To PerFormance Guarantee

                txtEMDValue.Text = DtContractsData.Rows[0]["EMDValue"].ToString();

                bool PaymentType = bool.Parse(DtContractsData.Rows[0]["PaymentType"].ToString());
                if (PaymentType == false)
                {
                    RadioLumpsum.Checked = false;
                    RadioManPower.Checked = true;
                    txtlampsum.Visible = false;
                    txtLumpsumtext.Visible = false;
                    lbllumpsumtext.Visible = false;
                }
                else
                {
                    RadioManPower.Checked = false;
                    RadioLumpsum.Checked = true;
                    txtlampsum.Visible = true;
                    txtlampsum.Text = DtContractsData.Rows[0]["PayLumpsum"].ToString();
                    txtLumpsumtext.Visible = true;
                    lbllumpsumtext.Visible = true;
                    txtLumpsumtext.Text = DtContractsData.Rows[0]["Lumpsumtext"].ToString();
                }

                txtWAWA.Text = DtContractsData.Rows[0]["WageAct"].ToString();
                txtPerGurante.Text = DtContractsData.Rows[0]["PerformanceGuaranty"].ToString();

                #endregion End Code For V-C(11) to (15)  Ref : EMD Value To PerFormance Guarantee

                #region Begin Code For V-C  Ref : Ref PF To ESI On OT

                TxtPf.Text = DtContractsData.Rows[0]["PF"].ToString();
                DdlPf.SelectedIndex = int.Parse(DtContractsData.Rows[0]["PFfrom"].ToString());
                //checkPFonOT.Checked = bool.Parse(DtContractsData.Rows[0]["PFonOT"].ToString());
                ddlpfon.SelectedIndex = int.Parse(DtContractsData.Rows[0]["PFonOT"].ToString());

                TxtEsi.Text = DtContractsData.Rows[0]["ESI"].ToString();
                DdlEsi.SelectedIndex = int.Parse(DtContractsData.Rows[0]["ESIfrom"].ToString());
                //checkESIonOT.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonOT"].ToString());
                ddlesion.SelectedIndex = int.Parse(DtContractsData.Rows[0]["ESIonOT"].ToString());
                txtPfLimit.Text = DtContractsData.Rows[0]["Pflimit"].ToString();
                txtEsiLimit.Text = DtContractsData.Rows[0]["Esilimit"].ToString();
                Chkpf.Checked = bool.Parse(DtContractsData.Rows[0]["Bpf"].ToString());
                ChkEsi.Checked = bool.Parse(DtContractsData.Rows[0]["Besi"].ToString());
                chkrc.Checked = bool.Parse(DtContractsData.Rows[0]["RelChrg"].ToString());
                #endregion End Code For V-c   Ref : Ref PF To ESI On OT

                #region Begin Code For V-C   Ref : Ref  ServiceChargeType  To SProfTax

                txtservicecharge.Text = DtContractsData.Rows[0]["ServiceCharge"].ToString();
                if (float.Parse(txtservicecharge.Text.Trim().ToString()) == 0)
                {
                    radiono.Checked = true;
                    RadioPercent.Visible = false;
                    RadioAmount.Visible = false;
                    txtservicecharge.Text = "0";
                    txtservicecharge.Visible = false;
                    // RadioPercent.Checked = true;
                }
                else
                {
                    radiono.Checked = false;
                    RadioPercent.Visible = true;
                    RadioAmount.Visible = true;
                    radioyes.Checked = true;
                    txtservicecharge.Visible = true;
                    bool ServiceChargeType = false;
                    if (String.IsNullOrEmpty(DtContractsData.Rows[0]["serviceChargeType"].ToString()) != false)
                    {
                        string tempData = DtContractsData.Rows[0]["serviceChargeType"].ToString();
                        if (tempData.Trim().Length > 0)
                        {
                            ServiceChargeType = bool.Parse(tempData);
                        }
                    }
                    else
                    {
                        ServiceChargeType = bool.Parse(DtContractsData.Rows[0]["serviceChargeType"].ToString());
                    }

                    if (ServiceChargeType == true)
                    {
                        RadioAmount.Checked = true;
                    }
                    else
                    {
                        RadioPercent.Checked = true;
                    }
                    chkStaxonservicecharge.Visible = true;

                    string strStaxonservicecharge = "";

                    if (String.IsNullOrEmpty(DtContractsData.Rows[0]["Staxonservicecharge"].ToString()) == false)
                    {
                        string tempData = DtContractsData.Rows[0]["Staxonservicecharge"].ToString();
                        if (tempData.Trim().Length > 0)
                        {
                            strStaxonservicecharge = tempData;
                        }
                    }
                    else
                    {
                        strStaxonservicecharge = DtContractsData.Rows[0]["serviceChargeType"].ToString();
                    }
                    if (strStaxonservicecharge == "True")
                    {
                        chkStaxonservicecharge.Checked = true;
                    }
                    if (strStaxonservicecharge == "False")
                    {
                        chkStaxonservicecharge.Checked = false;
                    }

                }

                var strBillDate = int.Parse(DtContractsData.Rows[0]["BillDates"].ToString());


                ddlbilldates.SelectedIndex = strBillDate;

                var strPaySheetDates = int.Parse(DtContractsData.Rows[0]["PaySheetDates"].ToString());

                ddlPaySheetDates.SelectedIndex = strPaySheetDates;

                var ojt = bool.Parse(DtContractsData.Rows[0]["ojt"].ToString());
                if (ojt == true)
                {
                    chkojt.Checked = true;
                }



                UInt16 WagesType = 0;
                if (String.IsNullOrEmpty(DtContractsData.Rows[0]["WageType"].ToString()) == false)
                {
                    string tempData = DtContractsData.Rows[0]["WageType"].ToString();
                    if (tempData.Trim().Length > 0)
                    {
                        //WagesType = bool.Parse(tempData);
                        WagesType = Convert.ToUInt16(tempData);
                    }
                }

                if (WagesType == 0)
                {
                    RadioSpecial.Checked = false;
                    RadioClient.Checked = false;
                    RadioCompany.Checked = true;
                    SpecialWagesPanel.Visible = false;
                }
                else if (WagesType == 1)
                {
                    RadioSpecial.Checked = false;
                    RadioCompany.Checked = false;
                    RadioClient.Checked = true;
                    SpecialWagesPanel.Visible = false;
                }
                else
                {
                    RadioSpecial.Checked = true;
                    RadioCompany.Checked = false;
                    RadioClient.Checked = false;
                    SpecialWagesPanel.Visible = true;
                }
                if (DtContractsData.Rows[0]["ProfTax"].ToString().Length > 0)
                {
                    bool PTaxStatus = Convert.ToBoolean(DtContractsData.Rows[0]["ProfTax"].ToString());
                    chkProfTax.Checked = PTaxStatus;
                }


                if (DtContractsData.Rows[0]["SProfTax"].ToString().Length > 0)
                {
                    bool SPTaxStatus = Convert.ToBoolean(DtContractsData.Rows[0]["SProfTax"].ToString());
                    chkspt.Checked = SPTaxStatus;
                }
                #endregion End Code For V-C   Ref : Ref  ServiceChargeType  To SProfTax

                #region Begin Code For V-C   Ref : Ref  ServiceTaxType  To TL No

                string stType = DtContractsData.Rows[0]["ServiceTaxType"].ToString();
                if (stType.Length > 0)
                {
                    bool bSTType = Convert.ToBoolean(stType);
                    if (bSTType == true)
                    {
                        RadioWithoutST.Checked = true;
                        RadioWithST.Checked = false;
                    }
                    else
                    {
                        RadioWithST.Checked = true;
                        RadioWithoutST.Checked = false;
                    }
                }





                string includeST = DtContractsData.Rows[0]["IncludeST"].ToString();
                CheckIncludeST.Checked = false;
                if (includeST.Length > 0)
                {
                    bool bIncludeST = Convert.ToBoolean(includeST);
                    if (bIncludeST == true)
                    {
                        CheckIncludeST.Checked = true;
                    }
                }
                string ST75 = DtContractsData.Rows[0]["ServiceTax75"].ToString();
                Check75ST.Checked = false;
                if (ST75.Length > 0)
                {
                    bool bST75 = Convert.ToBoolean(ST75);
                    if (bST75 == true)
                    {
                        Check75ST.Checked = true;
                    }
                }
                #region for GST on 30-6-2017 by sharada

                string CGST = DtContractsData.Rows[0]["CGST"].ToString();
                if (CGST == "False")
                {
                    chkCGST.Checked = false;
                }
                else
                {
                    chkCGST.Checked = true;
                }


                string SGST = DtContractsData.Rows[0]["SGST"].ToString();
                if (SGST == "False")
                {
                    RdbSGST.Checked = false;
                }
                else
                {
                    RdbSGST.Checked = true;
                }

                string IGST = DtContractsData.Rows[0]["IGST"].ToString();
                if (IGST == "False")
                {
                    RdbIGST.Checked = false;
                }
                else
                {
                    RdbIGST.Checked = true;
                }


                string Cess1 = DtContractsData.Rows[0]["Cess1"].ToString();
                if (Cess1 == "False")
                {
                    chkCess1.Checked = false;
                }
                else
                {
                    chkCess1.Checked = true;
                }


                string Cess2 = DtContractsData.Rows[0]["Cess2"].ToString();
                if (Cess2 == "False")
                {
                    chkCess2.Checked = false;
                }
                else
                {
                    chkCess2.Checked = true;
                }


                string GSTLineitem = DtContractsData.Rows[0]["GSTLineitem"].ToString();
                if (GSTLineitem == "False")
                {
                    chkGSTLineItem.Checked = false;
                }
                else
                {
                    chkGSTLineItem.Checked = true;
                }

                #endregion for GST on 30-6-2017 by sharada
                string OtPercent = DtContractsData.Rows[0]["OTPersent"].ToString();
                if (OtPercent == "100")
                {
                    DdlOt.SelectedIndex = 0;
                }
                else
                {
                    DdlOt.SelectedIndex = 1;
                }

                string OAPercent = DtContractsData.Rows[0]["OAPercent"].ToString();
                if (OAPercent == "100")
                {
                    ddlOAPer.SelectedIndex = 0;
                }
                else
                {
                    ddlOAPer.SelectedIndex = 1;
                }

                txtOWF.Text = DtContractsData.Rows[0]["OWF"].ToString();
                string OTAmounttype = DtContractsData.Rows[0]["OTAmounttype"].ToString();
                if (OTAmounttype.Length > 0)
                {
                    bool bSTType = Convert.ToBoolean(OTAmounttype);
                    if (bSTType == false)
                    {
                        radiootregular.Checked = true;
                    }
                    else
                    {
                        radiootspecial.Checked = true;
                    }
                }
                txtdescription.Text = DtContractsData.Rows[0]["Description"].ToString();
                TxtContractDescription.Text = DtContractsData.Rows[0]["ContractDescription"].ToString();

                chktl.Checked = bool.Parse(DtContractsData.Rows[0]["tl"].ToString());
                txttlamt.Text = DtContractsData.Rows[0]["tlno"].ToString();



                #endregion End Code For V-C  Ref : Ref  ServiceTaxType  To Description


                #region Begin Code For V-C   Ref : Ref  Tds  To Expecteddate as on 29/03/2014 by venkat


                txtTds.Text = DtContractsData.Rows[0]["Tds"].ToString();
                txtPono.Text = DtContractsData.Rows[0]["Pono"].ToString();
                txtExpectdateofreceipt.Text = DtContractsData.Rows[0]["ReceiptExpectedDate"].ToString();



                #endregion End Code For V-C  Ref : Ref  Tds  To Expecteddate as on 29/03/2014 by venkat

                #region Begin New code for Esibranche as on 02/08/2014 by venkat
                if (DtContractsData.Rows[0]["ESIBranch"].ToString() == "0")
                {
                    ddlEsibranch.SelectedIndex = 0;
                }
                else
                {
                    ddlEsibranch.SelectedValue = DtContractsData.Rows[0]["ESIBranch"].ToString();
                }

                Chkpdfs.Checked = bool.Parse(DtContractsData.Rows[0]["pdfs"].ToString());
                ChkRoundOff.Checked = bool.Parse(DtContractsData.Rows[0]["RoundOff"].ToString());
                chkNoNhsWoDed.Checked = bool.Parse(DtContractsData.Rows[0]["NoNhsWoDed"].ToString());
                chkGeneralDed.Checked = bool.Parse(DtContractsData.Rows[0]["NoGeneralDed"].ToString());
                chkUniformDed.Checked = bool.Parse(DtContractsData.Rows[0]["NoUniformDed"].ToString());
                chkSecDepDed.Checked = bool.Parse(DtContractsData.Rows[0]["NoSecDepDed"].ToString());
                chkOtherDed.Checked = bool.Parse(DtContractsData.Rows[0]["NoOtherDed"].ToString());
                ddlpaymentdates.SelectedValue = DtContractsData.Rows[0]["PaymentDate"].ToString();
                ChkPFSpl.Checked = bool.Parse(DtContractsData.Rows[0]["PFSpl"].ToString());

                Chkgrandtotwroff.Checked = bool.Parse(DtContractsData.Rows[0]["GrandTotalRoff"].ToString());


                #endregion

                #endregion   End  Code For Assign Values to The Controls as on [18-10-2013]

                #region  Begin Code For Client Contract Details Which are Used For The Billing As on [19-10-2013]
                HtContracts.Clear();
                #region    Begin Code For Assign Values To  Variable Declaration as on [19-10-2013]
                SPName = "GetContractDetailForInvoice";
                #endregion  Begin Code For Assign Values To  Variable Declaration as on [19-10-2013]

                #region Begin Code For Hash Table Parameters as on [19-10-2013]
                HtContracts.Add("@clientid", ClientID);
                HtContracts.Add("@Contractid", ddlContractids.SelectedValue);

                #endregion  End  Code For Hash Table Parameters as on [19-10-2013]

                #region  Begin Code For Calling Stored Procedure To Retrive Data from Contracts as on [19-10-2013]
                DataTable DtContractDetailsData = config.ExecuteAdaptorAsyncWithParams(SPName, HtContracts).Result;
                #endregion End Code For Calling Stored Procedure To Retrive Data from Contracts as on [19-10-2013]

                #endregion End  Code For Client Contract Details Which are Used For The Billing As on [19-10-2013]

                if (DtContractDetailsData.Rows.Count == 0)
                {
                    //lblmsgcontractdetails.Text = "There IS No Contract Details For This Client";
                    lblMsg.Text = "Contract Details Are Not Available . Which Are Generating The Invoice.";

                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Contract Details Are Not Available . Which Are Generating The Invoice.');", true);

                    if (RadioSpecial.Checked == false)
                    {
                        return;
                    }

                    else
                    {
                        Specialwagesdata();
                    }
                    // return;
                }
                else
                {
                    for (int i = 0; i < DtContractDetailsData.Rows.Count; i++)
                    {


                        gvdesignation.Rows[i].Visible = true;
                        //DefaultRowData(i);

                        DropDownList CDesgn = gvdesignation.Rows[i].FindControl("DdlDesign") as DropDownList;

                        if (String.IsNullOrEmpty(DtContractDetailsData.Rows[i]["Designations"].ToString()) != false)
                        {
                            CDesgn.SelectedIndex = 0;

                        }
                        else
                        {
                            if (int.Parse(DtContractDetailsData.Rows[i]["Designations"].ToString()) != 0)
                            {
                                CDesgn.SelectedValue = DtContractDetailsData.Rows[i]["Designations"].ToString();
                            }
                            else
                            {
                                CDesgn.SelectedIndex = 0;
                            }
                        }

                        //For Employee Duty Type

                        DropDownList CDutytype = gvdesignation.Rows[i].FindControl("ddldutytype") as DropDownList;

                        if (String.IsNullOrEmpty(DtContractDetailsData.Rows[i]["PayType"].ToString()) != false)
                        {
                            CDutytype.SelectedIndex = 0;
                        }
                        else
                        {
                            if (DtContractDetailsData.Rows[i]["PayType"].ToString().Trim().Length > 0)
                            {
                                CDutytype.SelectedIndex = Convert.ToInt32(DtContractDetailsData.Rows[i]["PayType"].ToString().Trim());
                            }
                        }

                        //No Of Days For Billing
                        DropDownList CNoOfDaysForBilling = gvdesignation.Rows[i].FindControl("ddlNoOfDaysBilling") as DropDownList;

                        if (CNoOfDaysForBilling != null)
                        {

                            if (String.IsNullOrEmpty(DtContractDetailsData.Rows[i]["NoofDays"].ToString()) != false)
                            {
                                CNoOfDaysForBilling.SelectedIndex = 0;
                            }
                            else
                            {
                                if (DtContractDetailsData.Rows[i]["NoofDays"].ToString().Trim().Length > 0)
                                {
                                    float noofdays = float.Parse(DtContractDetailsData.Rows[i]["NoofDays"].ToString());
                                    if (noofdays == 0)
                                        CNoOfDaysForBilling.SelectedIndex = 0;
                                    else
                                    if (noofdays == 1)
                                        CNoOfDaysForBilling.SelectedIndex = 1;
                                    else
                                        CNoOfDaysForBilling.SelectedValue = DtContractDetailsData.Rows[i]["NoofDays"].ToString();
                                }
                            }

                        }



                        DropDownList ddlNoOfOtsPaysheet = gvdesignation.Rows[i].FindControl("ddlNoOfOtsPaysheet") as DropDownList;

                        if (ddlNoOfOtsPaysheet != null)
                        {

                            if (String.IsNullOrEmpty(DtContractDetailsData.Rows[i]["Nots"].ToString()) != false)
                            {
                                ddlNoOfOtsPaysheet.SelectedIndex = 0;
                            }
                            else
                            {
                                if (DtContractDetailsData.Rows[i]["Nots"].ToString().Trim().Length > 0)
                                {
                                    int noofdays = int.Parse(DtContractDetailsData.Rows[i]["Nots"].ToString());
                                    if (noofdays == 0)
                                    {
                                        ddlNoOfOtsPaysheet.SelectedIndex = 0;
                                    }

                                    if (noofdays == 1)
                                    {
                                        ddlNoOfOtsPaysheet.SelectedIndex = 1;
                                    }
                                    if (noofdays == 2)
                                    {
                                        ddlNoOfOtsPaysheet.SelectedIndex = 2;
                                    }
                                }
                                else
                                {
                                    ddlNoOfOtsPaysheet.SelectedValue = DtContractDetailsData.Rows[i]["Nots"].ToString();
                                }

                            }

                        }

                        DropDownList ddlHSNNumber = gvdesignation.Rows[i].FindControl("ddlHSNNumber") as DropDownList;
                        ddlHSNNumber.SelectedValue = DtContractDetailsData.Rows[i]["HSNNumber"].ToString();

                        CheckBox chkcdCGST = gvdesignation.Rows[i].FindControl("chkcdCGST") as CheckBox;
                        chkcdCGST.Checked = bool.Parse(DtContractDetailsData.Rows[i]["cdCGST"].ToString());

                        CheckBox chkcdSGST = gvdesignation.Rows[i].FindControl("chkcdSGST") as CheckBox;
                        chkcdSGST.Checked = bool.Parse(DtContractDetailsData.Rows[i]["cdSGST"].ToString());

                        CheckBox chkcdIGST = gvdesignation.Rows[i].FindControl("chkcdIGST") as CheckBox;
                        chkcdIGST.Checked = bool.Parse(DtContractDetailsData.Rows[i]["cdIGST"].ToString());

                        DropDownList ddlbonustype = gvdesignation.Rows[i].FindControl("ddlbonustype") as DropDownList;
                        ddlbonustype.SelectedIndex = int.Parse(DtContractDetailsData.Rows[i]["BonusType"].ToString());

                        DropDownList ddlLAtype = gvdesignation.Rows[i].FindControl("ddlLAtype") as DropDownList;
                        ddlLAtype.SelectedIndex = int.Parse(DtContractDetailsData.Rows[i]["LAType"].ToString());

                        TextBox Cdutyhrs = (TextBox)gvdesignation.Rows[i].FindControl("txtdutyhrs");
                        Cdutyhrs.Text = DtContractDetailsData.Rows[i]["DutyHrs"].ToString();

                        TextBox CQuantity = (TextBox)gvdesignation.Rows[i].FindControl("txtquantity");
                        CQuantity.Text = DtContractDetailsData.Rows[i]["Quantity"].ToString();

                        TextBox Csummary = (TextBox)gvdesignation.Rows[i].FindControl("txtsummary");
                        Csummary.Text = DtContractDetailsData.Rows[i]["Summary"].ToString();

                        TextBox Cbasic = (TextBox)gvdesignation.Rows[i].FindControl("TxtBasic");
                        Cbasic.Text = DtContractDetailsData.Rows[i]["Basic"].ToString();

                        TextBox Cda = (TextBox)gvdesignation.Rows[i].FindControl("txtda");
                        Cda.Text = DtContractDetailsData.Rows[i]["da"].ToString();

                        TextBox Chra = (TextBox)gvdesignation.Rows[i].FindControl("txthra");
                        Chra.Text = DtContractDetailsData.Rows[i]["hra"].ToString();

                        TextBox CConveyance = (TextBox)gvdesignation.Rows[i].FindControl("txtConveyance");
                        CConveyance.Text = DtContractDetailsData.Rows[i]["Conveyance"].ToString();

                        TextBox Caw = (TextBox)gvdesignation.Rows[i].FindControl("txtoa");
                        Caw.Text = DtContractDetailsData.Rows[i]["OtherAllowance"].ToString();

                        TextBox Cwashallowance = (TextBox)gvdesignation.Rows[i].FindControl("txtwa");
                        Cwashallowance.Text = DtContractDetailsData.Rows[i]["washallownce"].ToString();

                        TextBox Cca = (TextBox)gvdesignation.Rows[i].FindControl("txtcca");
                        Cca.Text = DtContractDetailsData.Rows[i]["cca"].ToString();

                        TextBox LeaveAmount = (TextBox)gvdesignation.Rows[i].FindControl("txtleaveamount");
                        LeaveAmount.Text = DtContractDetailsData.Rows[i]["LeaveAmount"].ToString();

                        TextBox Gratuty = (TextBox)gvdesignation.Rows[i].FindControl("txtgratuty");
                        Gratuty.Text = DtContractDetailsData.Rows[i]["Gratuity"].ToString();

                        TextBox Bonus = (TextBox)gvdesignation.Rows[i].FindControl("txtbonus");
                        Bonus.Text = DtContractDetailsData.Rows[i]["Bonus"].ToString();

                        TextBox CAttBonus = (TextBox)gvdesignation.Rows[i].FindControl("txtattbonus");
                        CAttBonus.Text = DtContractDetailsData.Rows[i]["attbonus"].ToString();

                        TextBox CUniform = (TextBox)gvdesignation.Rows[i].FindControl("txtuniform");
                        CUniform.Text = DtContractDetailsData.Rows[i]["Uniform"].ToString();

                        TextBox PayRate = (TextBox)gvdesignation.Rows[i].FindControl("txtPayRate");
                        PayRate.Text = DtContractDetailsData.Rows[i]["Amount"].ToString();


                        if (i < DtContractDetailsData.Rows.Count)
                        {
                            Session["DataContractsAIndex"] = i + 1;
                            NewDataRow();
                        }
                        TextBox Nfhs = (TextBox)gvdesignation.Rows[i].FindControl("txtNfhs");
                        Nfhs.Text = DtContractDetailsData.Rows[i]["NFhs"].ToString();


                        TextBox Txtrc = (TextBox)gvdesignation.Rows[i].FindControl("Txtrc");
                        Txtrc.Text = DtContractDetailsData.Rows[i]["rc"].ToString();
                        TextBox CTxtCs = (TextBox)gvdesignation.Rows[i].FindControl("TxtCs");
                        CTxtCs.Text = DtContractDetailsData.Rows[i]["cs"].ToString();
                        TextBox CTxtOTRate = (TextBox)gvdesignation.Rows[i].FindControl("TxtOTRate");
                        CTxtOTRate.Text = DtContractDetailsData.Rows[i]["OTRATE"].ToString();

                        TextBox CTxtScPer = (TextBox)gvdesignation.Rows[i].FindControl("TxtScPer");
                        CTxtScPer.Text = DtContractDetailsData.Rows[i]["ServicechargePer"].ToString();



                    }
                    Session["ContractsAIndex"] = DtContractDetailsData.Rows.Count + 1;

                    //Data For The Second Gridview 


                    if (RadioSpecial.Checked == false)
                    {
                        return;
                    }

                    else
                    {
                        Specialwagesdata();
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void Specialwagesdata()
        {


            #region    Begin Code For Assign Values To  Variable Declaration as on [19-10-2013]
            var SPName = "GetContractDetailForPaysheet";
            Hashtable HtContractsForInvoice = new Hashtable();
            #endregion  Begin Code For Assign Values To  Variable Declaration as on [19-10-2013]

            #region Begin Code For Hash Table Parameters as on [19-10-2013]
            HtContractsForInvoice.Add("@clientid", ddlclientid.SelectedValue);
            HtContractsForInvoice.Add("@ContractId", ddlContractids.SelectedValue);

            #endregion  End  Code For Hash Table Parameters as on [19-10-2013]

            #region  Begin Code For Calling Stored Procedure To Retrive Data from Contracts as on [19-10-2013]
            DataTable dtspecialwage = config.ExecuteAdaptorAsyncWithParams(SPName, HtContractsForInvoice).Result;
            #endregion End Code For Calling Stored Procedure To Retrive Data from Contracts as on [19-10-2013]

            if (dtspecialwage.Rows.Count == 0)
            {
                lblMsg.Text = "There Is No Special Wage details for The Selected Client.";
                //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('There Is No Special Wage details for The Selected Client.');", true);
                return;
            }
            else
            {
                btnadddesgnsw.Visible = true;
                for (int i = 0; i < dtspecialwage.Rows.Count; i++)
                {
                    gvSWDesignations.Rows[i].Visible = true;
                    DropDownList CDesgnsw = gvSWDesignations.Rows[i].FindControl("DdlDesign") as DropDownList;
                    if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["Designations"].ToString()) != false)
                    {
                        CDesgnsw.SelectedIndex = 0;
                    }
                    else
                    {
                        if (int.Parse(dtspecialwage.Rows[i]["Designations"].ToString()) != 0)
                        {
                            CDesgnsw.SelectedValue = dtspecialwage.Rows[i]["Designations"].ToString();
                        }
                        else
                        {
                            CDesgnsw.SelectedIndex = 0;

                        }
                    }

                    //No Of Days For  wages
                    DropDownList CNoOfDaysFoWages = gvSWDesignations.Rows[i].FindControl("ddlNoOfDaysWages") as DropDownList;

                    if (CNoOfDaysFoWages != null)
                    {

                        if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["NoofDays"].ToString()) != false)
                        {
                            CNoOfDaysFoWages.SelectedIndex = 0;
                        }
                        else
                        {
                            if (dtspecialwage.Rows[i]["NoofDays"].ToString().Trim().Length > 0)
                            {

                                int noofdays = int.Parse(dtspecialwage.Rows[i]["NoofDays"].ToString());
                                if (noofdays == 0)
                                    CNoOfDaysFoWages.SelectedIndex = 0;
                                else
                                if (noofdays == 1)
                                    CNoOfDaysFoWages.SelectedIndex = 1;
                                else
                                    if (noofdays == 2)
                                    CNoOfDaysFoWages.SelectedIndex = 2;
                                else
                                    if (noofdays == 3)
                                    CNoOfDaysFoWages.SelectedIndex = 3;
                                else
                                    if (noofdays == 4)
                                    CNoOfDaysFoWages.SelectedIndex = 4;
                                else
                                    if (noofdays == 5)
                                    CNoOfDaysFoWages.SelectedIndex = 5;
                                else
                                    if (noofdays == 6)
                                    CNoOfDaysFoWages.SelectedIndex = 6;
                                else
                                    CNoOfDaysFoWages.SelectedValue = dtspecialwage.Rows[i]["NoofDays"].ToString();

                            }
                        }
                    }


                    DropDownList ddlNoOfOtsPaysheet = gvSWDesignations.Rows[i].FindControl("ddlNoOfOtsPaysheet") as DropDownList;

                    if (ddlNoOfOtsPaysheet != null)
                    {

                        if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["Nots"].ToString()) != false)
                        {
                            ddlNoOfOtsPaysheet.SelectedIndex = 0;
                        }
                        else
                        {
                            if (dtspecialwage.Rows[i]["Nots"].ToString().Trim().Length > 0)
                            {
                                int noofdays = int.Parse(dtspecialwage.Rows[i]["Nots"].ToString());
                                if (noofdays == 0)
                                    ddlNoOfOtsPaysheet.SelectedIndex = 0;
                                else
                                    if (noofdays == 1)
                                    ddlNoOfOtsPaysheet.SelectedIndex = 1;
                                else
                                        if (noofdays == 2)
                                    ddlNoOfOtsPaysheet.SelectedIndex = 2;
                                else
                                        if (noofdays == 3)
                                    ddlNoOfOtsPaysheet.SelectedIndex = 3;
                                else
                                        if (noofdays == 4)
                                    ddlNoOfOtsPaysheet.SelectedIndex = 4;
                                else
                                        if (noofdays == 5)
                                    ddlNoOfOtsPaysheet.SelectedIndex = 5;
                                else

                                    ddlNoOfOtsPaysheet.SelectedValue = dtspecialwage.Rows[i]["Nots"].ToString();
                            }
                        }

                    }


                    DropDownList ddlNoOfNhsPaysheet = gvSWDesignations.Rows[i].FindControl("ddlNoOfNhsPaysheet") as DropDownList;

                    if (ddlNoOfNhsPaysheet != null)
                    {

                        if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["NNhs"].ToString()) != false)
                        {
                            ddlNoOfNhsPaysheet.SelectedIndex = 0;
                        }
                        else
                        {
                            if (dtspecialwage.Rows[i]["NNhs"].ToString().Trim().Length > 0)
                            {
                                int noofdays = int.Parse(dtspecialwage.Rows[i]["NNhs"].ToString());
                                if (noofdays == 0)
                                    ddlNoOfNhsPaysheet.SelectedIndex = 0;
                                else
                                    if (noofdays == 1)
                                    ddlNoOfNhsPaysheet.SelectedIndex = 1;
                                else
                                        if (noofdays == 2)
                                    ddlNoOfNhsPaysheet.SelectedIndex = 2;
                                else
                                        if (noofdays == 3)
                                    ddlNoOfNhsPaysheet.SelectedIndex = 3;
                                else
                                        if (noofdays == 4)
                                    ddlNoOfNhsPaysheet.SelectedIndex = 4;
                                else
                                        if (noofdays == 5)
                                    ddlNoOfNhsPaysheet.SelectedIndex = 5;
                                else

                                    ddlNoOfNhsPaysheet.SelectedValue = dtspecialwage.Rows[i]["NNhs"].ToString();
                            }
                        }

                    }

                    DropDownList ddlLAtype = gvSWDesignations.Rows[i].FindControl("ddlLAtype") as DropDownList;
                    ddlLAtype.SelectedIndex = int.Parse(dtspecialwage.Rows[i]["LAType"].ToString());

                    DropDownList ddlGratuitytype = gvSWDesignations.Rows[i].FindControl("ddlGratuitytype") as DropDownList;
                    ddlGratuitytype.SelectedIndex = int.Parse(dtspecialwage.Rows[i]["Gratuitytype"].ToString());

                    DropDownList ddlbonustype = gvSWDesignations.Rows[i].FindControl("ddlbonustype") as DropDownList;
                    ddlbonustype.SelectedIndex = int.Parse(dtspecialwage.Rows[i]["BonusType"].ToString());


                    TextBox Cbasicsw = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtBasic");
                    Cbasicsw.Text = dtspecialwage.Rows[i]["Basic"].ToString();

                    TextBox Cdasw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtda");
                    Cdasw.Text = dtspecialwage.Rows[i]["da"].ToString();

                    TextBox Chrasw = (TextBox)gvSWDesignations.Rows[i].FindControl("txthra");
                    Chrasw.Text = dtspecialwage.Rows[i]["hra"].ToString();

                    TextBox CConveyancesw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtConveyance");
                    CConveyancesw.Text = dtspecialwage.Rows[i]["Conveyance"].ToString();

                    TextBox Cawsw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtoa");
                    Cawsw.Text = dtspecialwage.Rows[i]["OtherAllowance"].ToString();

                    TextBox Cwashallowancesw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtwa");
                    Cwashallowancesw.Text = dtspecialwage.Rows[i]["washallownce"].ToString();

                    TextBox Ccasw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtcca");
                    Ccasw.Text = dtspecialwage.Rows[i]["cca"].ToString();

                    TextBox LeaveAmountsw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtleaveamount");
                    LeaveAmountsw.Text = dtspecialwage.Rows[i]["LeaveAmount"].ToString();

                    TextBox Gratutysw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtgratuty");
                    Gratutysw.Text = dtspecialwage.Rows[i]["Gratuity"].ToString();

                    TextBox Bonussw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtbonus");
                    Bonussw.Text = dtspecialwage.Rows[i]["Bonus"].ToString();

                    TextBox AttBonussw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtattbonus");
                    AttBonussw.Text = dtspecialwage.Rows[i]["attbonus"].ToString();


                    if (i < dtspecialwage.Rows.Count)
                    {
                        Session["DataContractsAIndexsw"] = i + 1;
                        NewDataRowsw();
                    }
                    TextBox NFHsw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtNfhs1");
                    NFHsw.Text = dtspecialwage.Rows[i]["NFHs"].ToString();


                    TextBox Txtrc = (TextBox)gvSWDesignations.Rows[i].FindControl("Txtrc");
                    Txtrc.Text = dtspecialwage.Rows[i]["rc"].ToString();
                    TextBox TxtCs = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtCs");
                    TxtCs.Text = dtspecialwage.Rows[i]["cs"].ToString();
                    TextBox TxtOTRate = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtOTRate");
                    TxtOTRate.Text = dtspecialwage.Rows[i]["OTRate"].ToString();

                    TextBox CTxtNhsRate = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtNhsRate");
                    CTxtNhsRate.Text = dtspecialwage.Rows[i]["NHSRATE"].ToString();

                    TextBox TxtScPersw = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtScPer");
                    TxtScPersw.Text = dtspecialwage.Rows[i]["ServicechargePer"].ToString();

                }
            }
            Session["ContractsAIndexsw"] = dtspecialwage.Rows.Count + 1;
        }

        protected void NewDataRow()
        {
            int designcount = (int)Session["DataContractsAIndex"];

            if (designcount < gvdesignation.Rows.Count)
            {
                gvdesignation.Rows[designcount].Visible = true;
                DefaultRowData(designcount);

                //string selectquery = "Select Design from designations ORDER BY Design ";
                //DataTable DtDesignation = SqlHelper.Instance.GetTableByQuery(selectquery);

                DataTable DtDesignation = GlobalData.Instance.LoadDesigns();
                DropDownList ddldrow = gvdesignation.Rows[designcount].FindControl("DdlDesign") as DropDownList;
                ddldrow.Items.Clear();

                // ddldrow.Items.Add("--Select--");
                //for (int i = 0; i < DtDesignation.Rows.Count; i++)
                //{
                //    ddldrow.Items.Add(DtDesignation.Rows[i][0].ToString());
                //}


                if (DtDesignation.Rows.Count > 0)
                {
                    ddldrow.DataValueField = "Designid";
                    ddldrow.DataTextField = "Design";
                    ddldrow.DataSource = DtDesignation;
                    ddldrow.DataBind();

                }
                ddldrow.Items.Insert(0, "--Select--");
                ddldrow.SelectedIndex = 0;

            }
        }

        protected void NewDataRowsw()
        {
            int designcount = (int)Session["DataContractsAIndexsw"];
            if (designcount < gvSWDesignations.Rows.Count)
            {
                gvSWDesignations.Rows[designcount].Visible = true;
                DefaultRowDatasw(designcount);

                //string selectquery = "Select Design from designations ORDER BY Design";
                // DataTable DtDesignation = SqlHelper.Instance.GetTableByQuery(selectquery);

                DataTable DtDesignation = GlobalData.Instance.LoadDesigns();
                DropDownList ddldrowsw = gvSWDesignations.Rows[designcount].FindControl("DdlDesign") as DropDownList;
                ddldrowsw.Items.Clear();


                if (DtDesignation.Rows.Count > 0)
                {
                    ddldrowsw.DataValueField = "Designid";
                    ddldrowsw.DataTextField = "Design";
                    ddldrowsw.DataSource = DtDesignation;
                    ddldrowsw.DataBind();

                }
                ddldrowsw.Items.Insert(0, "--Select--");
                ddldrowsw.SelectedIndex = 0;


                //ddldrowsw.Items.Add("--Select--");
                //for (int i = 0; i < DtDesignation.Rows.Count; i++)
                //{
                //    ddldrowsw.Items.Add(DtDesignation.Rows[i][0].ToString());
                //}
            }
        }

        protected void RadioManPower_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioManPower.Checked == true)
            {
                lbllampsum.Visible = false;
                txtlampsum.Visible = false;
            }
        }

        protected void RadioSpecial_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioSpecial.Checked == true)
            {
                SpecialWagesPanel.Visible = true;
                btnadddesgnsw.Visible = true;
                Displaydefaulrowsw();
                Enable5rowssw();
                Specialwagesdata();
            }
            else
            {
                SpecialWagesPanel.Visible = false;
                btnadddesgnsw.Visible = false;
            }
        }

        protected void btnadddesgnsw_Click(object sender, EventArgs e)
        {
            int designCount = Convert.ToInt16(Session["ContractsAIndexsw"]);
            if (designCount < gvSWDesignations.Rows.Count)
            {
                gvSWDesignations.Rows[designCount].Visible = true;
                DefaultRowDatasw(designCount);

                // string selectquery = "Select Design from designations ORDER BY Design";
                // DataTable DtDesignation = SqlHelper.Instance.GetTableByQuery(selectquery);

                DataTable DtDesignation = GlobalData.Instance.LoadDesigns();
                DropDownList ddldrow = gvSWDesignations.Rows[designCount].FindControl("DdlDesign") as DropDownList;
                ddldrow.Items.Clear();
                //ddldrow.Items.Insert(0, "--Select--");
                if (DtDesignation.Rows.Count > 0)
                {
                    ddldrow.DataValueField = "Designid";
                    ddldrow.DataTextField = "Design";
                    ddldrow.DataSource = DtDesignation;
                    ddldrow.DataBind();

                }
                ddldrow.Items.Insert(0, "--Select--");
                ddldrow.SelectedIndex = 0;
                //for (int i = 0; i < DtDesignation.Rows.Count; i++)
                //{
                //    ddldrow.Items.Add(DtDesignation.Rows[i][0].ToString());
                //}
                designCount = designCount + 1;
                Session["ContractsAIndexsw"] = designCount;
                int check = int.Parse(Session["ContractsAIndexsw"].ToString());
            }
            else
            {
                lblmsgspecialwages.Text = "There is no more Designations";
            }
        }

        protected void Btn_Renewal_Click(object sender, EventArgs e)
        {
            if (ddlclientid.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select  Client ID/Name');", true);
                return;
            }
            else
            {
                txtcontractid.Text = GlobalData.Instance.LoadMaxContractId(ddlclientid.SelectedValue);
                ddlContractids.SelectedIndex = 0;
                txtStartingDate.Text = txtEndingDate.Text = "";
            }


        }

        protected void ddlContractids_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlContractids.SelectedIndex > 0)
            {
                GetGridData();
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
                Displaydefaulrowsw();
                ddlcname.SelectedIndex = 0;
                ClearDataFromThePage();
            }
        }

        protected void ddlclientid_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblreslt.Text = "";
            SpecialWagesPanel.Visible = false;
            DisplayDefaultRow();
            Displaydefaulrowsw();
            ClearDataFromThePage();
            if (ddlclientid.SelectedIndex > 0)
            {
                Fillcname();
                ddlContractids.Items.Clear();
                contractidautogenrate();
            }
            else
            {
                ddlcname.SelectedIndex = 0;

            }
        }

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SpecialWagesPanel.Visible = false;
            DisplayDefaultRow();
            Displaydefaulrowsw();
            ClearDataFromThePage();
            if (ddlcname.SelectedIndex > 0)
            {
                FillClientid();
                ddlContractids.Items.Clear();
                contractidautogenrate();
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        #region Begin New code for Esibranches as on 02/08/2014

        protected void LoadEsibranches()
        {
            DataTable dtEsibranches = GlobalData.Instance.LoadEsibranches();
            if (dtEsibranches.Rows.Count > 0)
            {
                ddlEsibranch.DataValueField = "EsiBranchid";
                ddlEsibranch.DataTextField = "EsiBranchname";
                ddlEsibranch.DataSource = dtEsibranches;
                ddlEsibranch.DataBind();
            }
            ddlEsibranch.Items.Insert(0, "-Select-");
        }

        #endregion


        protected void FillddlTakedata()
        {
            string SqlqryForClientIdAndName = string.Empty;
            DataTable dtForClientIdAndName = null;

            SqlqryForClientIdAndName = "select ClientName,Clientid from Clients where ClientId not in (select clientid from Contracts) order by ClientName";
            dtForClientIdAndName = config.ExecuteReaderWithQueryAsync(SqlqryForClientIdAndName).Result;

            if (dtForClientIdAndName.Rows.Count > 0)
            {
                ddlClientidNotincontract.DataTextField = "Clientname";
                ddlClientidNotincontract.DataValueField = "Clientid";
                ddlClientidNotincontract.DataSource = dtForClientIdAndName;
                ddlClientidNotincontract.DataBind();
            }
            ddlClientidNotincontract.Items.Insert(0, "-Select-");
        }

        protected void btnClone_Click(object sender, EventArgs e)
        {

            try
            {
                var testDate = 0;

                txtcontractid.Text = "";

                #region  Begin Code For Validations As on [18-10-2013]

                #region     Begin Code For Check The Client Id/Name Selected Or Not   as on [18-10-2013]
                if (ddlclientid.SelectedIndex == 0)
                {
                    lblMsg.Text = "Please Select Clientid";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' please Select Clientid ');", true);
                    clearcontractdetails();
                    return;
                }
                #endregion  End Code For Check The Client Id/Name Selected Or Not  as on [18-10-2013]

                #region     Begin Code For Check The Contract Start Date Entered Or Not  as on [18-10-2013]
                if (txtStartingDate.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill start date.');", true);
                    return;
                }

                if (txtStartingDate.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtStartingDate.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "show alert", "alert('You Are Entered Invalid Contract Start Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }
                    else
                    {
                        string CheckSD = Timings.Instance.CheckDateFormat(txtStartingDate.Text);
                        //string CheckSD = DateTime.Parse(txtStartingDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();

                        string CheckStartDate = "";

                        if (ddlContractids.SelectedIndex == 0)
                        {
                            CheckStartDate = " select clientid from contracts  where ContractEndDate>='" +
                                CheckSD + "'  and Clientid='" + ddlclientid.SelectedValue + "'";

                            DataTable Dt = config.ExecuteReaderWithQueryAsync(CheckStartDate).Result;
                            if (Dt.Rows.Count > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(),
                                  "show alert", "alert('You Are Entered Invalid Contract Start Date.Start Date Should Not Be Interval of the Previous Contracts Start and End Dates  Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                                return;
                            }
                        }
                        else
                        {
                            if (ddlContractids.SelectedIndex > 1)
                            {
                                string CIDForCheck = (txtcontractid.Text).ToString().Substring((txtcontractid.Text).Length - 2);
                                CheckStartDate = " select clientid from contracts  where ContractEndDate>='" +
                                    CheckSD + "'  and Clientid='" + ddlclientid.SelectedValue + "'  and Right(contractid,4)<" + CIDForCheck;

                                DataTable Dt = config.ExecuteReaderWithQueryAsync(CheckStartDate).Result;
                                if (Dt.Rows.Count > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(),
                                      "show alert", "alert('You Are Entered Invalid Contract Start Date.Start Date Should Not Be Interval of the Previous Contracts Start and End Dates  Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                                    return;
                                }
                            }
                        }

                    }


                }


                #endregion  End Code For Check The Contract Start Date Entered Or Not  as on [18-10-2013]

                #region     Begin Code For Check The Contract End Date Enetered Or Not  as on [18-10-2013]
                if (txtEndingDate.Text.Trim().Length == 0)
                {
                    lblMsg.Text = "Please fill End date.";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill End date.');", true);
                    return;
                }

                if (txtEndingDate.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtEndingDate.Text);
                    if (testDate > 0)
                    {
                        lblMsg.Text = "You Are Entered Invalid Contract End Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        // ScriptManager.RegisterStartupScript(this, GetType(),
                        //"show alert", "alert('You Are Entered Invalid Contract End Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }

                }



                #endregion  End Code For Check The Contract End Date Enetered Or Not as on [18-10-2013]

                #region     Begin Code For Check The Selected Dates are Valid Or Not  as on [18-10-2013]
                if (txtStartingDate.Text.Trim().Length != 0 && txtEndingDate.Text.Trim().Length != 0)
                {
                    DateTime Dtstartdate = DateTime.Parse(txtStartingDate.Text, CultureInfo.GetCultureInfo("en-gb"));
                    DateTime DtEnddate = DateTime.Parse(txtEndingDate.Text, CultureInfo.GetCultureInfo("en-gb"));


                    if (Dtstartdate >= DtEnddate)
                    {
                        lblMsg.Text = "Invalid Contract End Date . Contract End Date Always Should Be Greater Than To Start Date.";
                        return;
                    }
                }
                #endregion  End Code For Check Selected Dates are Valid Or Not   as on [18-10-2013]

                #region     Begin Code For Check The Lampsum if Lampsum Selected  as on [18-10-2013]
                if (RadioLumpsum.Checked)
                {
                    if (txtlampsum.Text.Trim().Length == 0)
                    {
                        lblMsg.Text = "Please Enter The Lampsum Amount.";
                        return;
                    }
                }

                #endregion  End Code For Check The Lampsum if Lampsum Selected    as on [18-10-2013]

                #region     Begin Code For Check The Service Charge if Service Charge Yes Selected  as on [18-10-2013]
                if (radioyes.Checked)
                {
                    if (txtservicecharge.Text.Trim().Length == 0)
                    {
                        lblMsg.Text = "Please Enter The Service Charge.";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter The Service Charge.');", true);
                        return;
                    }
                }

                #endregion  End Code For Check The Service Charge if Service Charge Yes Selected    as on [18-10-2013]

                #endregion  Begin Code For Validations As on [18-10-2013]

                #region  Begin Code For Declaring Variables as on [18-10-2013]

                #region  Begin Code For Variable Declaration Which is Stored into Contracts Table as on [18-10-2013]

                #region  Begin Code For V-D   (1) to (5)  : Ref Clientid To ContractID

                var ClientId = "";
                var ContractStartDate = "01/01/1900";
                var ContractEndDate = "01/01/1900";
                var BGAmount = "0";
                var ContractId = "";

                #endregion  End Code For V-D   (1) to (5)  : Ref Clientid To ContractID

                #region  Begin Code For V-D   (6) to (10)  : Ref Security Deposit To Machinary Cost Per Month

                var SecurityDeposit = "0";
                var TypeOfWork = "";
                var MaterialCostPerMonth = "0";
                var ValidityDate = "01/01/1900";
                var MachinaryCostPerMonth = "0";

                #endregion  End Code For V-D   (6) to (10)  : Ref Security Deposit To Machinary Cost Per Month

                #region  Begin Code For V-D   (11) to (15)  : Ref EMD Value To PerFormance Guarantee

                var EMDValue = "0";
                var PaymentType = "0";
                var WageAct = "0";
                var PayLumpsum = "0";
                var PerformanceGuaranty = "0";

                #endregion  End Code For V-D   (11) to (15)  : Ref EMD Value To PerFormance Guarantee

                #region  Begin Code For V-D     : Ref PF To ESI On OT

                var PF = "0";
                var PFFrom = 0;
                var PFonOT = 0;
                var ESI = "0";
                var ESIFrom = 0;
                var ESIonOt = 0;

                var Pflimit = "0";
                var Esilimit = "0";
                var Bpf = 0;
                var Besi = 0;
                var RelChrg = 0;

                #endregion  End Code For V-D     : Ref PF To ESI On OT

                #region    Begin Code For V-D     : Ref    Servicharge  Yes To   Service Chagre Amount
                var ServiceChargeType = 0;
                var ServiceCharge = "0";
                var BillDates = 0;
                var PaySheetDates = 0;
                var WageType = 0;
                var ProfTax = 0;
                var SProfTax = 0;
                #endregion   End  Code For V-D     : Ref    Servicharge  Yes To   Service Chagre Amount

                #region    Begin Code For V-D     : Ref    ServiceTaxType To   TL No

                var ServiceTaxType = 0;
                var IncludeST = 0;
                var ServiceTax75 = 0;
                var OTPersent = 0;
                var OAPercent = 0;
                var OWF = "";
                var OTAmounttype = 0;
                var Description = "";
                var ContractDescription = "";
                var otsalaryratecheck = 0;
                var otsalaryrat = "0";
                var ojt = 0;
                var TL = 0;
                var TLNo = "0";
                var CGST = 0;
                var SGST = 0;
                var IGST = 0;
                var Cess1 = 0;
                var Cess2 = 0;
                var GSTLineItem = 0;
                #endregion   End  Code For V-D     : Ref       ServiceTaxType To   Description

                #region    Begin Code For V-D     : Ref    New Field add in Contract on 29/03/2014 by venkat

                var Tds = 0;
                var Pono = "";
                var ReceiptExpectedDate = 0;
                var Staxonservicecharge = 0;
                var Lumpsumtext = "";

                #endregion   End  Code For V-D     : Ref       New Field add in Contract on 29/03/2014 by venkat

                #region Begin Code for Esi branche adding as on 02/08/2014
                var Esibranch = "0";
                var pdfs = 0;
                var RoundOff = 0;
                var NoNhsWoDed = 0;
                var NoGeneralDed = 0;
                var NoUniformDed = 0;
                var NoSecDepDed = 0;
                var NoOtherDed = 0;
                var Grandtotwroff = 0;
                #endregion

                #region Begin code For Stored Procedure related Variables declaration as on [18-10-2013]
                Hashtable HtContracts = new Hashtable();
                string SPName = "";
                var IRecordStatus = 0;
                #endregion  End code For Stored Procedure related Variables declaration as on [18-10-2013]

                #endregion End  Code For Variable Declaration Which is Stored into Contracts Table as on [18-10-2013]

                #endregion End Code For Declaring Variables as on [18-10-2013]


                #region  Begin Code For Assign Values to The Variables as on [18-10-2013]

                #region Begin Code For A-V (1) to (5)  Ref : ClientId To ConractID
                ClientId = ddlClientidNotincontract.SelectedValue;
                //ContractStartDate = DateTime.Parse(txtStartingDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                ContractStartDate = Timings.Instance.CheckDateFormat(txtStartingDate.Text);
                //ContractEndDate = DateTime.Parse(txtEndingDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                ContractEndDate = Timings.Instance.CheckDateFormat(txtEndingDate.Text);

                BGAmount = txtBGAmount.Text;

                if (ddlClientidNotincontract.SelectedIndex > 0)
                {
                    CloneContractidautogenrate();
                    ContractId = txtclonecontractid.Text;
                }

                #endregion End Code For A-V (1) to (5)  Ref :ClientID To ContractID

                #region Begin Code For A-V (6) to (10)  Ref : Security Deposit To Machinary Cose Per Month
                SecurityDeposit = txtSecurityDeposit.Text;
                TypeOfWork = txtTypeOfWork.Text;
                MaterialCostPerMonth = txtMaterial.Text;
                if (txtValidityDate.Text.Trim().Length != 0)
                {
                    //ValidityDate = DateTime.Parse(txtValidityDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                    ValidityDate = Timings.Instance.CheckDateFormat(txtValidityDate.Text);

                }

                MachinaryCostPerMonth = txtMachinary.Text;

                #endregion End Code For A-V (6) to (10)  Ref : Security Deposit To Machinary Cose Per Month

                #region Begin Code For A-V (11) to (15)  Ref : EMD Value To PerFormance Guarantee

                EMDValue = txtEMDValue.Text;
                if (RadioLumpsum.Checked)
                {
                    PaymentType = "1";
                    if (txtLumpsumtext.Text.Trim().Length > 0)
                    {
                        Lumpsumtext = txtLumpsumtext.Text;
                    }
                }

                WageAct = txtWAWA.Text;
                if (RadioLumpsum.Checked)
                    PayLumpsum = txtlampsum.Text.Trim();
                PerformanceGuaranty = txtPerGurante.Text;

                #endregion End Code For A-V (11) to (15)  Ref : EMD Value To PerFormance Guarantee

                #region Begin Code For A-V   Ref : Ref PF To ESI On OT

                if (TxtPf.Text.Trim().Length > 0)
                {
                    PF = TxtPf.Text;
                }

                PFFrom = DdlPf.SelectedIndex;

                //if (checkPFonOT.Checked)
                //    PFonOT = 1;
                PFonOT = ddlpfon.SelectedIndex;
                if (TxtEsi.Text.Trim().Length > 0)
                {
                    ESI = TxtEsi.Text;
                }
                ESIFrom = DdlEsi.SelectedIndex;

                // if (checkESIonOT.Checked)
                //    ESIonOt = 1;
                ESIonOt = ddlesion.SelectedIndex;

                if (Chkpf.Checked)
                    Bpf = 1;

                if (ChkEsi.Checked)
                    Besi = 1;

                if (txtPfLimit.Text.Trim().Length > 0)
                    Pflimit = txtPfLimit.Text;
                if (txtEsiLimit.Text.Trim().Length > 0)
                    Esilimit = txtEsiLimit.Text;
                if (chkrc.Checked)
                    RelChrg = 1;


                #endregion End Code For A-V   Ref : Ref PF To ESI On OT

                #region Begin Code For A-V   Ref : Ref  ServiceChargeType  To SProfTax

                if (radioyes.Checked)
                {
                    if (RadioPercent.Checked)
                    {
                        ServiceCharge = txtservicecharge.Text.Trim();
                        ServiceChargeType = 0;
                    }
                    else
                    {
                        ServiceCharge = txtservicecharge.Text.Trim();
                        ServiceChargeType = 1;
                    }

                    if (chkStaxonservicecharge.Checked)
                    {
                        Staxonservicecharge = 1;
                    }

                }

                //if (RadioStartDate.Checked == true)
                //    BillDates = 1;
                BillDates = ddlbilldates.SelectedIndex;
                PaySheetDates = ddlPaySheetDates.SelectedIndex;

                if (RadioCompany.Checked)
                {
                    WageType = 0;
                }
                else if (RadioClient.Checked)
                {
                    WageType = 1;
                }
                else
                {
                    WageType = 2;
                }

                if (chkProfTax.Checked)
                    ProfTax = 1;

                if (chkspt.Checked)
                    SProfTax = 1;

                #endregion End Code For A-V   Ref : Ref  ServiceChargeType  To SProfTax

                #region Begin Code For A-V   Ref : Ref  ServiceTaxType  To TL No

                if (RadioWithoutST.Checked)
                {
                    ServiceTaxType = 1;
                }

                if (CheckIncludeST.Checked)
                {
                    IncludeST = 1;
                }

                if (Check75ST.Checked)
                {
                    ServiceTax75 = 1;
                }
                #region for GST 30-6-2017 by sharu

                if (chkCGST.Checked)
                {
                    CGST = 1;
                }

                if (RdbSGST.Checked)
                {
                    SGST = 1;
                }

                if (RdbIGST.Checked)
                {
                    IGST = 1;
                }

                if (chkCess1.Checked)
                {
                    Cess1 = 1;
                }

                if (chkCess2.Checked)
                {
                    Cess2 = 1;
                }

                if (chkCess2.Checked)
                {
                    Cess2 = 1;
                }

                if (chkGSTLineItem.Checked)
                {
                    GSTLineItem = 1;
                }

                #endregion for GST 30-6-2017 by sharu
                if (DdlOt.SelectedIndex == 0)
                    OTPersent = 100;
                else
                    OTPersent = 200;

                if (ddlOAPer.SelectedIndex == 0)
                {
                    OAPercent = 100;
                }
                else
                {
                    OAPercent = 200;
                }


                OWF = txtOWF.Text;
                if (radiootspecial.Checked)
                {
                    OTAmounttype = 1;
                }
                Description = txtdescription.Text.Trim();
                ContractDescription = TxtContractDescription.Text;

                if (chkotsalaryrate.Checked)
                {
                    otsalaryratecheck = 1;
                    otsalaryrat = txtotsalaryrate.Text;
                }
                if (chkojt.Checked)
                {
                    ojt = 1;
                }

                if (chktl.Checked)
                {
                    TL = 1;
                    TLNo = txttlamt.Text;
                }


                #endregion End Code For A-V   Ref : Ref  ServiceTaxType  To Description


                #region Begin Code For A-V   Ref : Ref  Tds  To Expect date on 29/03/2014 by venkat

                if (txtTds.Text.Trim().Length > 0)
                {
                    Tds = int.Parse(txtTds.Text);
                }

                if (txtPono.Text.Trim().Length > 0)
                {
                    Pono = txtPono.Text;
                }
                if (txtExpectdateofreceipt.Text.Trim().Length > 0)
                {
                    ReceiptExpectedDate = int.Parse(txtExpectdateofreceipt.Text);
                }

                #endregion End Code For A-V   Ref : Ref   Tds  To Expecte date

                #region Begin Code for Esi branche adding as on 02/08/2014

                if (ddlEsibranch.SelectedIndex > 0)
                {
                    Esibranch = ddlEsibranch.SelectedValue;
                }

                if (Chkpdfs.Checked)
                {

                    pdfs = 1;
                }

                if (ChkRoundOff.Checked)
                {
                    RoundOff = 1;
                }

                if (Chkgrandtotwroff.Checked)
                {

                    Grandtotwroff = 1;
                }

                if (chkNoNhsWoDed.Checked)
                {

                    NoNhsWoDed = 1;
                }
                if (chkUniformDed.Checked)
                {

                    NoUniformDed = 1;
                }
                if (chkSecDepDed.Checked)
                {

                    NoSecDepDed = 1;
                }
                if (chkOtherDed.Checked)
                {

                    NoOtherDed = 1;
                }
                if (chkGeneralDed.Checked)
                {

                    NoGeneralDed = 1;
                }


                #endregion

                var PaymentDates = "0";
                PaymentDates = ddlpaymentdates.SelectedValue;

                var PFSpl = 0;

                if (ChkPFSpl.Checked)
                {
                    PFSpl = 1;
                }

                #endregion   End  Code For Assign Values to The Variables as on [18-10-2013]


                #region Begin Code For Hash Table/Sp Parameters As on [18-10-2013]
                #region  Begin Code For H-S-Parameters   (1) to (5)  : Ref Clientid To ContractID

                HtContracts.Add("@ClientId", ClientId);
                HtContracts.Add("@ContractStartDate", ContractStartDate);
                HtContracts.Add("@ContractEndDate", ContractEndDate);
                HtContracts.Add("@BGAmount", BGAmount);
                HtContracts.Add("@ContractId", ContractId);

                #endregion  End Code For  H-S-Parameters   (1) to (5)  : Ref Clientid To ContractID

                #region  Begin Code For H-S-Parameters    (6) to (10)  : Ref Security Deposit To Machinary Cost Per Month

                HtContracts.Add("@SecurityDeposit", SecurityDeposit);
                HtContracts.Add("@TypeOfWork", TypeOfWork);
                HtContracts.Add("@MaterialCostPerMonth", MaterialCostPerMonth);
                HtContracts.Add("@ValidityDate", ValidityDate);
                HtContracts.Add("@MachinaryCostPerMonth", MachinaryCostPerMonth);

                #endregion  End Code For H-S-Parameters    (6) to (10)  : Ref Security Deposit To Machinary Cost Per Month

                #region  Begin Code For H-S-Parameters    (11) to (15)  : Ref EMD Value To PerFormance Guarantee

                HtContracts.Add("@EMDValue", EMDValue);
                HtContracts.Add("@PaymentType", PaymentType);
                HtContracts.Add("@WageAct", WageAct);
                HtContracts.Add("@PayLumpsum", PayLumpsum);
                HtContracts.Add("@PerformanceGuaranty", PerformanceGuaranty);

                #endregion  End Code For H-S-Parameters    (11) to (15)  : Ref EMD Value To PerFormance Guarantee

                #region  Begin Code For H-S-Parameters     : Ref PF To ESI On OT

                HtContracts.Add("@PF", PF);
                HtContracts.Add("@PFFrom", PFFrom);
                HtContracts.Add("@PFonOT", PFonOT);
                HtContracts.Add("@ESI", ESI);
                HtContracts.Add("@ESIFrom", ESIFrom);
                HtContracts.Add("@ESIonOt", ESIonOt);


                #endregion  End Code For H-S-Parameters      : Ref PF To ESI On OT


                #region    Begin Code For H-S-Parameters     : Ref    Servicharge  Yes To   Service Chagre Amount

                HtContracts.Add("@ServiceChargeType", ServiceChargeType);
                HtContracts.Add("@ServiceCharge", ServiceCharge);
                HtContracts.Add("@BillDates", BillDates);
                HtContracts.Add("@PaySheetDate", PaySheetDates);
                HtContracts.Add("@WageType", WageType);
                HtContracts.Add("@ProfTax", ProfTax);
                HtContracts.Add("@SProfTax", SProfTax);

                #endregion   End  Code For   H-S-Parameters    : Ref    Servicharge  Yes To   Service Chagre Amount


                #region    Begin Code For H-S-Parameters    : Ref    ServiceTaxType To   TL No

                HtContracts.Add("@ServiceTaxType", ServiceTaxType);
                HtContracts.Add("@IncludeST", IncludeST);
                HtContracts.Add("@ServiceTax75", ServiceTax75);
                HtContracts.Add("@OTPersent", OTPersent);
                HtContracts.Add("@OAPercent", OAPercent);
                HtContracts.Add("@OWF", OWF);
                HtContracts.Add("@OTAmounttype", OTAmounttype);
                HtContracts.Add("@Description", Description);
                HtContracts.Add("@ContractDescription", ContractDescription);
                HtContracts.Add("@otsalaryratecheck", otsalaryratecheck);
                HtContracts.Add("@otsalaryrat", otsalaryrat);
                HtContracts.Add("@ojt", ojt);
                HtContracts.Add("@tl", TL);
                HtContracts.Add("@tlno", TLNo);
                HtContracts.Add("@PFLimit", Pflimit);
                HtContracts.Add("@ESILimit", Esilimit);
                HtContracts.Add("@Bpf", Bpf);
                HtContracts.Add("@Besi", Besi);
                HtContracts.Add("@RelChrg", RelChrg);
                HtContracts.Add("@CGST", CGST);
                HtContracts.Add("@IGST", IGST);
                HtContracts.Add("@SGST", SGST);
                HtContracts.Add("@Cess1", Cess1);
                HtContracts.Add("@Cess2", Cess2);
                HtContracts.Add("@GSTLineItem", GSTLineItem);

                #endregion   End  Code For H-S-Parameters    : Ref       ServiceTaxType To   Description


                #region    Begin Code For H-S-Parameters    : Ref    Tds To   Lumpsumtext on 29/03/2014 by venkat

                HtContracts.Add("@Tds", Tds);
                HtContracts.Add("@Pono", Pono);
                HtContracts.Add("@ReceiptExpectedDate", ReceiptExpectedDate);
                HtContracts.Add("@Staxonservicecharge", Staxonservicecharge);
                HtContracts.Add("@Lumpsumtext", Lumpsumtext);

                #endregion   End  Code For H-S-Parameters    : Ref        Tds To   Lumpsumtext on 29/03/2014 by venkat


                HtContracts.Add("@Esibranch", Esibranch);
                HtContracts.Add("@pdfs", pdfs);
                HtContracts.Add("@RoundOff", RoundOff);
                HtContracts.Add("@NoNhsWoDed", NoNhsWoDed);
                HtContracts.Add("@NoGeneralDed", NoGeneralDed);
                HtContracts.Add("@NoUniformDed", NoUniformDed);
                HtContracts.Add("@NoSecDepDed", NoSecDepDed);
                HtContracts.Add("@NoOtherDed", NoOtherDed);
                HtContracts.Add("@PaymentDates", PaymentDates);
                HtContracts.Add("@PFSpl", PFSpl);
                HtContracts.Add("@Grandtotwroff", Grandtotwroff);

                #endregion  End  Code For Hash Table/Sp Parameters As on [18-10-2013]

                #region Begin Code For Calling Stored Procedure as on [18-10-2013]
                SPName = "AddorModifyContracts";
                IRecordStatus = config.ExecuteNonQueryParamsAsync(SPName, HtContracts).Result;
                #endregion End Code For Calling Stored Procedure as on [18-10-2013]




                if (IRecordStatus > 0)
                {
                    lblSuc.Text = "Contract Added Successfully";

                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Contract Added Successfully ');", true);
                    int j = 0;

                    DateTime today = DateTime.Now.Date;
                    int designCount = Convert.ToInt16(Session["ContractsAIndex"]);
                    //Store Data into Contract Details
                    int clientdesigncount = 0;
                    for (j = 0; j < designCount; j++)
                    {
                        if (j < gvdesignation.Rows.Count)
                        {

                            HtContracts.Clear();

                            #region Clientdesignation
                            string Cddldesignation = ((DropDownList)gvdesignation.Rows[j].FindControl("DdlDesign")).SelectedValue;
                            DropDownList ddlindex = gvdesignation.Rows[j].FindControl("DdlDesign") as DropDownList;
                            if (ddlindex.SelectedIndex == 0)
                            {
                                break;
                            }
                            #endregion


                            #region   Duty Hrs
                            string Cdutyhrs = ((TextBox)gvdesignation.Rows[j].FindControl("txtdutyhrs")).Text;
                            if (Cdutyhrs.Trim().Length == 0)
                            {
                                Cdutyhrs = "";
                            }
                            #endregion

                            #region   Quantity
                            string Cquantity = ((TextBox)gvdesignation.Rows[j].FindControl("txtquantity")).Text;
                            if (Cquantity.Trim().Length == 0)
                            {
                                //lblmsgcontractdetails.Text = "Please enter No. of employee needed";
                                // break;
                                //Cquantity = "0";
                            }
                            else
                            {
                                float tempQty = Convert.ToSingle(Cquantity);
                                if (tempQty < 0)
                                {
                                    lblMsg.Text = "No. of employee needed can't be  negative";
                                    break;
                                }
                            }
                            #endregion
                            //string Cddldutytype = ((DropDownList)gvdesignation.Rows[j].FindControl("ddldutytype")).Text;
                            #region PayType
                            DropDownList ddlPayType = gvdesignation.Rows[j].FindControl("ddldutytype") as DropDownList;
                            int PayType = ddlPayType.SelectedIndex;
                            #endregion

                            #region  No Of Days For Billing
                            DropDownList ddlNoOfDaysForBilling = gvdesignation.Rows[j].FindControl("ddlNoOfDaysBilling") as DropDownList;
                            var NoOfDaysForBilling = "0";

                            if (ddlNoOfDaysForBilling.SelectedIndex == 0)
                            {
                                NoOfDaysForBilling = "0";
                            }
                            if (ddlNoOfDaysForBilling.SelectedIndex == 1)
                            {
                                NoOfDaysForBilling = "1";
                            }


                            if (ddlNoOfDaysForBilling.SelectedIndex > 1)
                            {
                                NoOfDaysForBilling = ddlNoOfDaysForBilling.SelectedValue.ToString();
                            }
                            #endregion

                            #region  No Of Ots
                            string NoOfOts = "0";
                            DropDownList ddlNoOfOtsPaysheet = gvdesignation.Rows[j].FindControl("ddlNoOfOtsPaysheet") as DropDownList;

                            if (ddlNoOfOtsPaysheet.SelectedIndex == 0)
                            {
                                NoOfOts = "0";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 1)
                            {
                                NoOfOts = "1";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 2)
                            {
                                NoOfOts = "2";
                            }

                            if (ddlNoOfOtsPaysheet.SelectedIndex > 2)
                            {
                                NoOfOts = ddlNoOfOtsPaysheet.SelectedValue;
                            }
                            #endregion No Of Ots


                            #region Summary
                            string Csummary = ((TextBox)gvdesignation.Rows[j].FindControl("txtsummary")).Text;
                            //string Camount = ((TextBox)gvdesignation.Rows[j].FindControl("txtamount")).Text;
                            if (Csummary.Trim().Length == 0)
                            {
                                Csummary = "";
                            }
                            #endregion

                            #region  PayRate
                            string strPayRate = ((TextBox)gvdesignation.Rows[j].FindControl("txtPayRate")).Text;
                            if (RadioLumpsum.Checked == false)
                            {
                                if (strPayRate.Trim().Length == 0)
                                {
                                    lblMsg.Text = "Please enter Pay Rate for employee";
                                    break;
                                    //Cquantity = "0";
                                }
                                else
                                {
                                    float tempPay = Convert.ToSingle(strPayRate);
                                    if (tempPay <= 0)
                                    {
                                        lblMsg.Text = "Pay Rate of employee can't be 0 or negative";
                                        break;
                                    }
                                }
                            }
                            else
                                strPayRate = "0";

                            #endregion

                            #region  Basic
                            string Cbasic = ((TextBox)gvdesignation.Rows[j].FindControl("TxtBasic")).Text;
                            if (RadioLumpsum.Checked == false)
                            {
                                if (Cbasic.Trim().Length == 0)
                                {
                                    if (RadioClient.Checked)
                                    {
                                        lblMsg.Text = "Please enter basic pay for employee";
                                        break;
                                    }
                                    Cbasic = "0";
                                }
                                else
                                {
                                    if (RadioClient.Checked)
                                    {
                                        float tempBaic = Convert.ToSingle(Cbasic);
                                        if (tempBaic <= 0)
                                        {
                                            lblMsg.Text = "Basic pay can't be 0 or negative";
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                                Cbasic = "0";

                            #endregion

                            #region Da
                            string Cda = ((TextBox)gvdesignation.Rows[j].FindControl("txtda")).Text;
                            if (Cda.Trim().Length == 0)
                            {
                                Cda = "0";
                            }
                            #endregion

                            var LAType = 0;
                            DropDownList ddlLAtype = gvdesignation.Rows[j].FindControl("ddlLAtype") as DropDownList;
                            LAType = ddlLAtype.SelectedIndex;

                            var bonustype = 0;
                            DropDownList ddlbonustype = gvdesignation.Rows[j].FindControl("ddlbonustype") as DropDownList;
                            bonustype = ddlbonustype.SelectedIndex;

                            #region Hra
                            string Chra = ((TextBox)gvdesignation.Rows[j].FindControl("txthra")).Text;
                            if (Chra.Trim().Length == 0)
                            {
                                Chra = "0";
                            }
                            #endregion

                            #region Conveyance
                            string CConveyance = ((TextBox)gvdesignation.Rows[j].FindControl("txtConveyance")).Text;
                            if (CConveyance.Trim().Length == 0)
                            {
                                CConveyance = "0";
                            }
                            #endregion

                            #region Other Allowance
                            string Caw = ((TextBox)gvdesignation.Rows[j].FindControl("txtoa")).Text;
                            if (Caw.Trim().Length == 0)
                            {
                                Caw = "0";
                            }
                            #endregion

                            #region Wash Allowance
                            string Cwashallowance = ((TextBox)gvdesignation.Rows[j].FindControl("txtwa")).Text;
                            if (Cwashallowance.Trim().Length == 0)
                            {
                                Cwashallowance = "0";
                            }
                            #endregion

                            #region CCA
                            string Cca = ((TextBox)gvdesignation.Rows[j].FindControl("txtcca")).Text;
                            if (Cca.Trim().Length == 0)
                            {
                                Cca = "0";
                            }
                            #endregion

                            #region LeaveAmount

                            string LeaveAmount = ((TextBox)gvdesignation.Rows[j].FindControl("txtleaveamount")).Text;
                            if (LeaveAmount.Trim().Length == 0)
                            {
                                LeaveAmount = "0";
                            }
                            #endregion

                            #region Gratituty
                            string Gratuty = ((TextBox)gvdesignation.Rows[j].FindControl("txtgratuty")).Text;
                            if (Gratuty.Trim().Length == 0)
                            {
                                Gratuty = "0";
                            }
                            #endregion

                            #region Bonus
                            string Bonus = ((TextBox)gvdesignation.Rows[j].FindControl("txtbonus")).Text;
                            if (Bonus.Trim().Length == 0)
                            {
                                Bonus = "0";
                            }

                            string AttBonus = ((TextBox)gvdesignation.Rows[j].FindControl("txtattbonus")).Text;
                            if (AttBonus.Trim().Length == 0)
                            {
                                AttBonus = "0";
                            }
                            #endregion

                            #region for Uniform
                            string Uniform = ((TextBox)gvdesignation.Rows[j].FindControl("txtuniform")).Text;
                            if (Uniform.Trim().Length == 0)
                            {
                                Uniform = "0";
                            }
                            #endregion for Uniform

                            #region NFhs
                            string Nfhsw = ((TextBox)gvdesignation.Rows[j].FindControl("txtNfhs")).Text;
                            if (Nfhsw.Trim().Length == 0)
                            {
                                Nfhsw = "0";
                            }


                            #endregion


                            #region BEgin RC
                            string RC = ((TextBox)gvdesignation.Rows[j].FindControl("Txtrc")).Text;
                            if (RC.Trim().Length == 0)
                            {
                                RC = "0";
                            }

                            #endregion End Rc



                            #region Begin CS
                            string CS = ((TextBox)gvdesignation.Rows[j].FindControl("TxtCs")).Text;
                            if (CS.Trim().Length == 0)
                            {
                                CS = "0";
                            }

                            string CSper = ((TextBox)gvdesignation.Rows[j].FindControl("TxtScPer")).Text;
                            if (CSper.Trim().Length == 0)
                            {
                                CSper = "0";
                            }

                            #endregion End CS

                            #region Begin OT RATE
                            string OTRATE = ((TextBox)gvdesignation.Rows[j].FindControl("TxtOTRate")).Text;
                            if (OTRATE.Trim().Length == 0)
                            {
                                OTRATE = "0";
                            }
                            #endregion End OT RATE

                            double payRate = double.Parse(strPayRate);

                            #region  STored Procedure Parameters , connection Strings
                            SPName = "GetContractDetails";
                            #region Parameters 1 -8
                            HtContracts.Add("@clientid", ClientId);
                            HtContracts.Add("@Contractid", ContractId);
                            HtContracts.Add("@Designations", Cddldesignation);
                            HtContracts.Add("@DutyHrs", Cdutyhrs);
                            HtContracts.Add("@Quantity", Cquantity);
                            HtContracts.Add("@basic", Cbasic);
                            HtContracts.Add("@da", Cda);
                            HtContracts.Add("@hra", Chra);
                            HtContracts.Add("@conveyance", CConveyance);

                            #endregion

                            #region Parameters 8-16
                            HtContracts.Add("@washallownce", Cwashallowance);
                            HtContracts.Add("@OtherAllowance", Caw);
                            HtContracts.Add("@Summary", Csummary);
                            HtContracts.Add("@Amount", payRate);
                            #endregion

                            #region Parameters 16-24
                            HtContracts.Add("@cca", Cca);
                            HtContracts.Add("@leaveamount", LeaveAmount);
                            HtContracts.Add("@bonus", Bonus);
                            HtContracts.Add("@gratuity", Gratuty);
                            HtContracts.Add("@PayType", PayType);
                            HtContracts.Add("@NoOfDays", NoOfDaysForBilling);
                            HtContracts.Add("@NFhs", Nfhsw);
                            HtContracts.Add("@testrecord", clientdesigncount);

                            HtContracts.Add("@Nots", NoOfOts);
                            HtContracts.Add("@RC", RC);
                            HtContracts.Add("@CS", CS);
                            HtContracts.Add("@OTRATE", OTRATE);
                            HtContracts.Add("@attbonus", AttBonus);
                            HtContracts.Add("@servicechargeper", CSper);
                            HtContracts.Add("@Uniform", Uniform);
                            HtContracts.Add("@BonusType", bonustype);
                            HtContracts.Add("@LAType", LAType);

                            #endregion

                            IRecordStatus = config.ExecuteNonQueryParamsAsync(SPName, HtContracts).Result;
                            if (IRecordStatus != 0)
                            {
                                clientdesigncount++;
                            }
                            #endregion

                        }
                    }

                    #region   Contract Special Wise Designations 

                    int designCountsw = Convert.ToInt16(Session["ContractsAIndexsw"]);
                    int specialdesigncount = 0;
                    for (j = 0; j < designCountsw; j++)
                    {
                        if (j < gvSWDesignations.Rows.Count)
                        {
                            HtContracts.Clear();
                            #region  Client Designations
                            string Cddldesignationsw = ((DropDownList)gvSWDesignations.Rows[j].FindControl("DdlDesign")).SelectedValue;
                            DropDownList ddlindexsw = gvSWDesignations.Rows[j].FindControl("DdlDesign") as DropDownList;
                            if (ddlindexsw.SelectedIndex == 0)
                            {
                                break;
                            }
                            #endregion

                            #region  Begin  No Of Days For Wages
                            DropDownList ddlNoOfDaysForWages = gvSWDesignations.Rows[j].FindControl("ddlNoOfDaysWages") as DropDownList;
                            string NoOfDaysForWages = "0";

                            if (ddlNoOfDaysForWages.SelectedIndex == 0)
                            {
                                NoOfDaysForWages = "0";
                            }
                            if (ddlNoOfDaysForWages.SelectedIndex == 1)
                            {
                                NoOfDaysForWages = "1";
                            }
                            if (ddlNoOfDaysForWages.SelectedIndex == 2)
                            {
                                NoOfDaysForWages = "2";
                            }
                            if (ddlNoOfDaysForWages.SelectedIndex == 3)
                            {
                                NoOfDaysForWages = "3";
                            }
                            if (ddlNoOfDaysForWages.SelectedIndex == 4)
                            {
                                NoOfDaysForWages = "4";
                            }
                            if (ddlNoOfDaysForWages.SelectedIndex == 5)
                            {
                                NoOfDaysForWages = "5";
                            }
                            if (ddlNoOfDaysForWages.SelectedIndex == 6)
                            {
                                NoOfDaysForWages = "6";
                            }

                            if (ddlNoOfDaysForWages.SelectedIndex > 6)
                            {
                                NoOfDaysForWages = ddlNoOfDaysForWages.SelectedValue;
                            }
                            #endregion  //End  No Of Days For Wages

                            #region  No Of Ots
                            string NoOfOts = "0";
                            DropDownList ddlNoOfOtsPaysheet = gvSWDesignations.Rows[j].FindControl("ddlNoOfOtsPaysheet") as DropDownList;

                            if (ddlNoOfOtsPaysheet.SelectedIndex == 0)
                            {
                                NoOfOts = "0";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 1)
                            {
                                NoOfOts = "1";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 2)
                            {
                                NoOfOts = "2";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 3)
                            {
                                NoOfOts = "3";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 4)
                            {
                                NoOfOts = "4";
                            }

                            if (ddlNoOfOtsPaysheet.SelectedIndex == 5)
                            {
                                NoOfOts = "5";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex > 5)
                            {
                                NoOfOts = ddlNoOfOtsPaysheet.SelectedValue;
                            }
                            #endregion No Of Ots

                            #region  No Of NHS
                            string NoOfNHS = "0";
                            DropDownList ddlNoOfNhsPaysheet = gvSWDesignations.Rows[j].FindControl("ddlNoOfNhsPaysheet") as DropDownList;

                            if (ddlNoOfNhsPaysheet.SelectedIndex == 0)
                            {
                                NoOfNHS = "0";
                            }
                            if (ddlNoOfNhsPaysheet.SelectedIndex == 1)
                            {
                                NoOfNHS = "1";
                            }
                            if (ddlNoOfNhsPaysheet.SelectedIndex == 2)
                            {
                                NoOfNHS = "2";
                            }
                            if (ddlNoOfNhsPaysheet.SelectedIndex == 3)
                            {
                                NoOfNHS = "3";
                            }
                            if (ddlNoOfNhsPaysheet.SelectedIndex == 4)
                            {
                                NoOfNHS = "4";
                            }

                            if (ddlNoOfNhsPaysheet.SelectedIndex == 5)
                            {
                                NoOfNHS = "5";
                            }
                            if (ddlNoOfNhsPaysheet.SelectedIndex > 5)
                            {
                                NoOfNHS = ddlNoOfNhsPaysheet.SelectedValue;
                            }

                            string NHSRATE = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtNhsRate")).Text;
                            if (NHSRATE.Trim().Length == 0)
                            {
                                NHSRATE = "0";
                            }

                            #endregion No Of NHS

                            #region  Basic
                            string Cbasicsw = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtBasic")).Text;
                            if (Cbasicsw.Trim().Length == 0)
                            {
                                Cbasicsw = "0";
                            }

                            #endregion

                            #region DA
                            string Cdasw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtda")).Text;
                            if (Cdasw.Trim().Length == 0)
                            {
                                Cdasw = "0";
                            }
                            #endregion

                            #region  Hra
                            string Chrasw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txthra")).Text;
                            if (Chrasw.Trim().Length == 0)
                            {
                                Chrasw = "0";
                            }

                            #endregion

                            #region Conveyance
                            string CConveyancesw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtConveyance")).Text;
                            if (CConveyancesw.Trim().Length == 0)
                            {
                                CConveyancesw = "0";
                            }
                            #endregion

                            #region Other Allowance
                            string Cawsw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtoa")).Text;
                            if (Cawsw.Trim().Length == 0)
                            {
                                Cawsw = "0";
                            }
                            #endregion

                            #region Wash Allowance 

                            string Cwashallowancesw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtwa")).Text;
                            if (Cwashallowancesw.Trim().Length == 0)
                            {
                                Cwashallowancesw = "0";
                            }

                            #endregion

                            #region CCa
                            string Ccasw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtcca")).Text;
                            if (Ccasw.Trim().Length == 0)
                            {
                                Ccasw = "0";
                            }
                            #endregion

                            #region Leave Amount 
                            string LeaveAmountsw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtleaveamount")).Text;
                            if (LeaveAmountsw.Trim().Length == 0)
                            {
                                LeaveAmountsw = "0";
                            }
                            #endregion

                            var LAType = 0;
                            DropDownList ddlLAtype = gvSWDesignations.Rows[j].FindControl("ddlLAtype") as DropDownList;
                            LAType = ddlLAtype.SelectedIndex;

                            #region gratituty
                            string Gratutysw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtgratuty")).Text;
                            if (Gratutysw.Trim().Length == 0)
                            {
                                Gratutysw = "0";
                            }
                            #endregion

                            var Gratuitytype = 0;
                            DropDownList ddlGratuitytype = gvSWDesignations.Rows[j].FindControl("ddlGratuitytype") as DropDownList;
                            Gratuitytype = ddlGratuitytype.SelectedIndex;

                            #region Bonus 
                            string Bonussw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtbonus")).Text;
                            if (Bonussw.Trim().Length == 0)
                            {
                                Bonussw = "0";
                            }

                            string AttBonussw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtattbonus")).Text;
                            if (AttBonussw.Trim().Length == 0)
                            {
                                AttBonussw = "0";
                            }
                            #endregion

                            var bonustype = 0;
                            DropDownList ddlbonustype = gvSWDesignations.Rows[j].FindControl("ddlbonustype") as DropDownList;
                            bonustype = ddlbonustype.SelectedIndex;

                            #region NFHs
                            String Nfhssw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtNfhs1")).Text;
                            if (Nfhssw.Trim().Length == 0)
                            {
                                Nfhssw = "0";
                            }



                            #endregion

                            #region BEgin RC
                            string RC = ((TextBox)gvSWDesignations.Rows[j].FindControl("Txtrc")).Text;
                            if (RC.Trim().Length == 0)
                            {
                                RC = "0";
                            }

                            #endregion End Rc



                            #region Begin CS
                            string CS = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtCs")).Text;
                            if (CS.Trim().Length == 0)
                            {
                                CS = "0";
                            }

                            string CSpersw = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtScPer")).Text;
                            if (CSpersw.Trim().Length == 0)
                            {
                                CSpersw = "0";
                            }
                            #endregion End CS

                            #region Begin OT RATE
                            string OTRATE = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtOTRate")).Text;
                            if (OTRATE.Trim().Length == 0)
                            {
                                OTRATE = "0";
                            }



                            #endregion End OT RATE

                            #region  STored Procedure Parameters , connection Strings
                            SPName = "Insertcontractspecialdetails";

                            #region Parameters 1 -8
                            HtContracts.Add("@clientid", ClientId);
                            HtContracts.Add("@ContractId", ContractId);
                            // HtContracts.Add("@ClientID", ClientId);
                            HtContracts.Add("@Designations", Cddldesignationsw);
                            HtContracts.Add("@Basic", Cbasicsw);

                            HtContracts.Add("@DA", Cdasw);
                            HtContracts.Add("@HRA", Chrasw);
                            HtContracts.Add("@Conveyance", CConveyancesw);
                            HtContracts.Add("@WashAllownce", Cwashallowancesw);

                            #endregion

                            #region Parameters 8-16

                            HtContracts.Add("@OtherAllowance", Cawsw);
                            HtContracts.Add("@CCA", Ccasw);
                            HtContracts.Add("@LeaveAmount", LeaveAmountsw);
                            HtContracts.Add("@Bonus", Bonussw);
                            HtContracts.Add("@gratuity", Gratutysw);
                            #endregion

                            #region Parameters 16-22

                            HtContracts.Add("@NoOfDays", NoOfDaysForWages);
                            HtContracts.Add("@NFHs", Nfhssw);
                            HtContracts.Add("@testrecord", specialdesigncount);

                            HtContracts.Add("@Nots", NoOfOts);
                            HtContracts.Add("@RC", RC);
                            HtContracts.Add("@CS", CS);
                            HtContracts.Add("@OTRATE", OTRATE);
                            HtContracts.Add("@attbonus", AttBonussw);
                            HtContracts.Add("@servicechargeper", CSpersw);
                            HtContracts.Add("@LAType", LAType);
                            HtContracts.Add("@Gratuitytype", Gratuitytype);
                            HtContracts.Add("@bonustype", bonustype);
                            HtContracts.Add("@NoOfNHS", NoOfNHS);
                            HtContracts.Add("@NHSRATE", NHSRATE);
                            #endregion

                            IRecordStatus = config.ExecuteNonQueryParamsAsync(SPName, HtContracts).Result;
                            if (IRecordStatus != 0)
                            {
                                specialdesigncount++;
                            }
                            #endregion

                        }
                    }

                    #endregion
                }
                if (IRecordStatus != 0)
                {
                    contractidautogenrate();
                    clearcontractdetails();
                    Enable5Rows();
                    // lblreslt.Text = "Record Inserted Successfully";
                    Session["ContractsAIndex"] = 0;
                    Session["ContractsAIndexsw"] = 0;
                    ClearDataFromThePage();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }

            Session["ContractsAIndex"] = 0;
            Session["ContractsAIndexsw"] = 0;

            FillddlTakedata();

        }
    }
}