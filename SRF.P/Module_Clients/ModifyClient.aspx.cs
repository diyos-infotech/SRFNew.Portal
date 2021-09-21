using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using KLTS.Data;
using SRF.P.DAL;

namespace SRF.P.Module_Clients
{
    public partial class ModifyClient : System.Web.UI.Page
    {
        DataTable dt;
        string CmpIDPrefix = "";
        string EmpIDPrefix = "";

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

                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                    LoadSegments();
                    LoadDesignations();
                    LoadClients();
                    LoadopmEmpsIDs();
                    LoadClientids();
                    LoadOurGSTNos();
                    LoadStatenames();
                    LoadAreas();
                    LoadZones();
                    if (Request.QueryString["clientid"] != null)
                    {

                        string username = Request.QueryString["clientid"].ToString();
                        txtclientid.Text = username;
                        TxtClient_OnTextChanged(sender, e);

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void LoadOurGSTNos()
        {
            DataTable DtGSTNos = GlobalData.Instance.LoadGSTNumbers();
            if (DtGSTNos.Rows.Count > 0)
            {
                ddlOurGSTIN.DataValueField = "Id";
                ddlOurGSTIN.DataTextField = "GSTNo";
                ddlOurGSTIN.DataSource = DtGSTNos;
                ddlOurGSTIN.DataBind();
            }
        }

        protected void LoadStatenames()
        {

            DataTable DtStateNames = GlobalData.Instance.LoadStateNames();
            if (DtStateNames.Rows.Count > 0)
            {
                ddlstate.DataValueField = "StateID";
                ddlstate.DataTextField = "State";
                ddlstate.DataSource = DtStateNames;
                ddlstate.DataBind();


                ddlStateCode.DataValueField = "StateID";
                ddlStateCode.DataTextField = "GSTStateCode";
                ddlStateCode.DataSource = DtStateNames;
                ddlStateCode.DataBind();
            }
            ddlstate.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlStateCode.Items.Insert(0, new ListItem("-Select-", "0"));

        }

        protected void LoadClients()
        {
            DataTable DtCnames = GlobalData.Instance.LoadCNames(CmpIDPrefix);
            if (DtCnames.Rows.Count > 0)
            {
                ddlUnits.DataValueField = "clientid";
                ddlUnits.DataTextField = "clientname";
                ddlUnits.DataSource = DtCnames;
                ddlUnits.DataBind();

            }
            ddlUnits.Items.Insert(0, "-Select-");
        }

        protected void LoadClientids()
        {
            DataTable DtIds = GlobalData.Instance.LoadCIds(CmpIDPrefix);
            if (DtIds.Rows.Count > 0)
            {
                ddlcid.DataValueField = "clientid";
                ddlcid.DataTextField = "clientid";
                ddlcid.DataSource = DtIds;
                ddlcid.DataBind();
            }
            ddlcid.Items.Insert(0, "-Select-");
        }

        protected void LoadopmEmpsIDs()
        {
            DataTable DtopmEmpsIDs = GlobalData.Instance.LoadOpManagerIds();
            if (DtopmEmpsIDs.Rows.Count > 0)
            {
                ddlEmpId.DataValueField = "EmpId";
                ddlEmpId.DataTextField = "EmpId";
                ddlEmpId.DataSource = DtopmEmpsIDs;
                ddlEmpId.DataBind();
            }
            ddlEmpId.Items.Insert(0, "-Select-");
        }

        protected void LoadDesignations()
        {
            DataTable DtDesignations = GlobalData.Instance.LoadDesigns();
            if (DtDesignations.Rows.Count > 0)
            {
                ddldesgn.DataValueField = "Designid";
                ddldesgn.DataTextField = "Design";
                ddldesgn.DataSource = DtDesignations;
                ddldesgn.DataBind();
            }
            ddldesgn.Items.Insert(0, "-Select-");
        }

        protected void LoadSegments()
        {
            DataTable DtSegments = GlobalData.Instance.LoadSegments();
            if (DtSegments.Rows.Count > 0)
            {
                ddlsegment.DataValueField = "segid";
                ddlsegment.DataTextField = "segname";
                ddlsegment.DataSource = DtSegments;
                ddlsegment.DataBind();
            }
            ddlsegment.Items.Insert(0, "-Select-");
        }

        private void LoadAreas()
        {
            DataTable DtArea = GlobalData.Instance.LoadArea();
            if (DtArea.Rows.Count > 0)
            {
                ddlArea.DataValueField = "Areaid";
                ddlArea.DataTextField = "AreaName";
                ddlArea.DataSource = DtArea;
                ddlArea.DataBind();
            }
            ddlArea.Items.Insert(0, "-Select-");

        }

        private void LoadZones()
        {
            DataTable DtZone = GlobalData.Instance.LoadZone();
            if (DtZone.Rows.Count > 0)
            {
                ddlZone.DataValueField = "Zoneid";
                ddlZone.DataTextField = "ZoneName";
                ddlZone.DataSource = DtZone;
                ddlZone.DataBind();
            }
            ddlZone.Items.Insert(0, "-Select-");

        }


        protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qry = "select GSTstatecode,stateid from states where stateid='" + ddlstate.SelectedValue + "'";
            DataTable dt = config.ExecuteReaderWithQueryAsync(qry).Result;
            if (dt.Rows.Count > 0)
            {

                if (dt.Rows[0]["stateid"].ToString() != "0")
                {
                    ddlStateCode.SelectedValue = dt.Rows[0]["stateid"].ToString();
                }
                else
                {
                    ddlStateCode.SelectedIndex = 0;
                }

            }
            else
            {
                ddlStateCode.SelectedIndex = 0;

            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        protected void btnaddclint_Click(object sender, EventArgs e)
        {
            try
            {

                #region  Begin  Check Validations as on  [19-09-2013]

                #region     Begin Check Client id Selected or  ?
                //if (ddlcid.SelectedIndex == 0)
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select Client ID ');", true);
                //    return;
                //}


                if (txtclientid.Text.Trim().Length == 0)
                {
                    lblMsg.Text = "Please  Enter the Client ID";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please  Enter the Client ID ');", true);
                    return;
                }


                #endregion  End CCheck Client id Selected or  ?

                #region     Begin Check Client Name is  Empty or ?
                if (txtcname.Text.Trim().Length == 0)
                {
                    lblMsg.Text = "Please Enter The Client Name";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Enter The Client Name ');", true);
                    return;
                }
                #endregion  End Check Client Name is  Empty or ?

                #region   Begin Check   Contact Person   Name
                if (txtcontactperson.Text.Trim().Length == 0)
                {
                    lblMsg.Text = "Please fill Contact Person name";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please fill Contact Person name ');", true);
                    return;
                }
                #endregion  Begin Check   Contact Person   Name

                #region  Begin Check Designation Selected or ?
                if (ddldesgn.SelectedIndex == 0)
                {
                    lblMsg.Text = "Please Select Designation";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select Designation ');", true);
                    return;
                }
                #endregion End Check Designation Selected or ?

                #region Begin Check Phone Number Entered or ?
                if (txtphonenumbers.Text.Trim().Length == 0)
                {
                    lblMsg.Text = "Please Enter the Phone No.";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Enter the Phone No.');", true);
                    return;
                }
                if (txtphonenumbers.Text.Trim().Length < 8)
                {
                    lblMsg.Text = "Please enter a valid Phone No.";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please enter a valid Phone No.');", true);
                    return;
                }
                #endregion  End Check Phone Number Entered or ?

                #region  Begin Check if Sub unit Check then Should be Select MAin unit ID
                if (chkSubUnit.Checked)
                {
                    if (ddlUnits.SelectedIndex == 0)
                    {
                        lblMsg.Text = "Please select Main unit Id";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please select Main unit Id');", true);
                        return;
                    }
                }
                #endregion End Check if Sub unit Check then Should be Select MAin unit ID


                #region  Begin Check Invoice  Selected or ?
                if (radioinvoiceyes.Checked == false && radioinvoiceno.Checked == false)
                {
                    lblMsg.Text = "Please Select the Invoice Mode";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select the Invoice Mode ');", true);
                    return;
                }
                #endregion End Check Invoice Selected or ?

                #region Begin Check Paysheet Selected  or ?
                if (radiopaysheetyes.Checked == false && radiopaysheetno.Checked == false)
                {
                    lblMsg.Text = "Please Select the  Paysheet Mode";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select the  Paysheet Mode');", true);
                    return;
                }

                #endregion  End Check Paysheet Selected  Entered or ?

                #region Begin Check Main Unit  Selected  or ?
                if (radioyesmu.Checked == false && radionomu.Checked == false)
                {
                    lblMsg.Text = "Please Select the  Client Is Main Unit (YES/NO)";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select the  Client Is Main Unit (YES/NO)');", true);
                    return;
                }
                #endregion  End Check  Main Unit Selected  Entered or ?


                #endregion End   Check Validations as on  [19-09-2013]


                #region     Begin Declare Variables as on [19-09-2013]
                #region     Begin Code Client-id to  Contact Person
                var ClientId = string.Empty;
                var ClientName = string.Empty;
                var ClientShortName = string.Empty;
                var ClientSegment = string.Empty;
                var ClientContactPerson = string.Empty;
                #endregion  End Code Client-id to Contact Person

                #region  Begin Code  Person-Designation To PIN-No
                var ClientPersonDesgn = string.Empty;
                var ClientPhonenumbers = string.Empty;
                var ClientFax = string.Empty;
                var ClientEmail = string.Empty;
                var ClientAddrPin = string.Empty;
                var State = "0";
                var StateCode = "0";
                var GSTIN = "";
                var OurGSTIN = "";
                #endregion  End Code  Person-Designation To PIN-No

                #region  Begin Code  Line-One To Line-Five
                var ClientAddrHno = string.Empty;
                var ClientAddrStreet = string.Empty;
                var ClientAddrArea = string.Empty;
                var ClientAddrCity = string.Empty;
                var ClientAddrColony = string.Empty;
                #endregion  End Code Line-One To Line-Five

                #region Begin Code Line Six To PaySheet
                var ClientAddrState = string.Empty;
                var SubUnitStatus = string.Empty;
                var MainUnitId = string.Empty;
                var MAinunitStatus = 0;
                var Invoice = 0;
                var Paysheet = 0;
                var ClientDesc = string.Empty;
                var Area = string.Empty;
                var Zone = string.Empty;

                #endregion End Code Line Six To PaySheet

                #region   Begin Extra Varibles for This Event   As on [20-09-2013]
                var URecordStatus = 0;
                #endregion End Extra Varibles for This Event   As on [20-09-2013]

                var Location = "";

                #endregion  End Declare Variables as on [19-09-2013]


                #region    Begin Code For Assign Values Into Declared Variables as on [19-09-2013]
                #region    Begin Code Client-id to  Contact Person
                //ClientId =ddlcid.SelectedValue;
                ClientId = txtclientid.Text;
                ClientName = txtcname.Text;
                ClientShortName = txtshortname.Text;
                if (ddlsegment.SelectedIndex == 0)
                {
                    ClientSegment = "0";
                }
                else
                {
                    ClientSegment = ddlsegment.SelectedValue;
                }

                if (ddlArea.SelectedIndex == 0)
                {
                    Area = "0";
                }
                else
                {
                    Area = ddlArea.SelectedValue;
                }
                if (ddlZone.SelectedIndex == 0)
                {
                    Zone = "0";
                }
                else
                {
                    Zone = ddlZone.SelectedValue;
                }
                ClientContactPerson = txtcontactperson.Text;
                #endregion  End Code Client-id to Contact Person

                #region  Begin Code  Person-Designation To PIN-No
                ClientPersonDesgn = ddldesgn.SelectedValue;
                ClientPhonenumbers = txtphonenumbers.Text;
                ClientFax = txtfaxno.Text;
                ClientEmail = txtemailid.Text;
                ClientAddrPin = txtpin.Text;
                if (ddlstate.SelectedIndex > 0)
                {
                    State = ddlstate.SelectedValue;
                }

                if (ddlStateCode.SelectedIndex > 0)
                {
                    StateCode = ddlStateCode.SelectedValue;
                }


                GSTIN = txtGSTUniqueID.Text;
                OurGSTIN = ddlOurGSTIN.SelectedValue;
                #endregion  End Code  Person-Designation To PIN-No

                #region  Begin Code  Line-One To Line-Five
                ClientAddrHno = txtchno.Text;
                ClientAddrStreet = txtstreet.Text;
                ClientAddrArea = txtarea.Text;
                ClientAddrCity = TxtCity.Text;
                ClientAddrColony = txtcolony.Text;
                #endregion  End Code Line-One To Line-Five

                #region Begin Code Line Six To PaySheet
                ClientAddrState = txtstate.Text;
                if (chkSubUnit.Checked)
                {
                    SubUnitStatus = "1";
                    MainUnitId = ddlUnits.SelectedValue;
                }
                else
                {
                    MainUnitId = "0";
                }

                if (radioyesmu.Checked)
                {
                    MAinunitStatus = 1;
                }
                if (radioinvoiceyes.Checked)
                {
                    Invoice = 1;
                }
                if (radiopaysheetyes.Checked)
                {
                    Paysheet = 1;
                }
                ClientDesc = txtdescription.Text;
                #endregion End Code Line Six To PaySheet

                if (txtLocation.Text.Trim().Length > 0)
                {
                    Location = txtLocation.Text;
                }

                #endregion   End Code For Assign Values Into Declared Variables as on [19-09-2013]


                #region    Begin Code For Stored Procedure Parameters as on [20-09-2013]
                Hashtable ModifyClientDetails = new Hashtable();
                string ModifyClientDetailsPName = "ModifyClientDetails";

                #region     Begin Code Client-id to  Contact Person

                ModifyClientDetails.Add("@ClientId", ClientId);
                ModifyClientDetails.Add("@ClientName", ClientName);
                ModifyClientDetails.Add("@ClientShortName", ClientShortName);
                ModifyClientDetails.Add("@ClientSegment", ClientSegment);
                ModifyClientDetails.Add("@ClientContactPerson", ClientContactPerson);

                #endregion  End Code Client-id to Contact Person


                #region  Begin Code  Person-Designation To PIN-No

                ModifyClientDetails.Add("@ClientPersonDesgn", ClientPersonDesgn);
                ModifyClientDetails.Add("@ClientPhonenumbers", ClientPhonenumbers);
                ModifyClientDetails.Add("@ClientFax", ClientFax);
                ModifyClientDetails.Add("@ClientEmail", ClientEmail);
                ModifyClientDetails.Add("@ClientAddrPin", ClientAddrPin);
                ModifyClientDetails.Add("@state", State);
                ModifyClientDetails.Add("@StateCode", StateCode);
                ModifyClientDetails.Add("@GSTIN", GSTIN);
                ModifyClientDetails.Add("@OurGSTIN", OurGSTIN);
                #endregion  End Code  Person-Designation To PIN-No


                #region  Begin Code  Line-One To Line-Five

                ModifyClientDetails.Add("@ClientAddrHno", ClientAddrHno);
                ModifyClientDetails.Add("@ClientAddrStreet", ClientAddrStreet);
                ModifyClientDetails.Add("@ClientAddrArea", ClientAddrArea);
                ModifyClientDetails.Add("@ClientAddrCity", ClientAddrCity);
                ModifyClientDetails.Add("@ClientAddrColony", ClientAddrColony);

                #endregion  End Code Line-One To Line-Five

                #region Begin Code Line Six To PaySheet

                ModifyClientDetails.Add("@ClientAddrState", ClientAddrState);
                ModifyClientDetails.Add("@SubUnitStatus", SubUnitStatus);
                ModifyClientDetails.Add("@MainUnitId", MainUnitId);
                ModifyClientDetails.Add("@MAinunitStatus", MAinunitStatus);
                ModifyClientDetails.Add("@Invoice", Invoice);
                ModifyClientDetails.Add("@Paysheet", Paysheet);
                ModifyClientDetails.Add("@ClientDesc", ClientDesc);
                ModifyClientDetails.Add("@Area", Area);
                ModifyClientDetails.Add("@Zone", Zone);

                #endregion End Code Line Six To PaySheet

                ModifyClientDetails.Add("@Location", Location);

                #endregion End Code For Stored Procedure Parameters as on [20-09-2013]


                #region     Begin Code For Calling Stored Procedure as on [20-09-2013]
                URecordStatus = config.ExecuteNonQueryParamsAsync(ModifyClientDetailsPName, ModifyClientDetails).Result;
                #endregion   End   Code For Calling Stored Procedure as on [20-09-2013]


                #region     Begin Code For Status/Resulted Message of the Inserted Record as on [20-09-2013]

                if (URecordStatus > 0)
                {
                    lblSuc.Text = "Client Details Modified Sucessfully.  With  Client Id   :- " + ClientId + " ";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Client Details Modified Sucessfully.  With  Client Id   :- " + ClientId + " -: ');", true);
                    //ClearClientsFieldsData();
                    return;
                }
                else
                {
                    lblMsg.Text = "Client Details Not  Added Sucessfully  With  Client Id   :- " + ClientId + " ";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Client Details Not  Added Sucessfully  With  Client Id   :- " + ClientId + " -: ');", true);
                    return;
                }
                #endregion  End Code For Status/Resulted Message of the Inserted Record as on [20-09-2013]




            }

            catch (Exception ex)
            {

            }

        }

        private void ClearClientsFieldsData()
        {

            txtcname.Text = txtshortname.Text = txtcontactperson.Text = txtphonenumbers.Text = txtfaxno.Text = txtemailid.Text =
            txtpin.Text = txtchno.Text = txtstreet.Text = txtarea.Text = TxtCity.Text = txtcolony.Text =
            txtstate.Text = txtdescription.Text = txtclientid.Text = string.Empty;

            ddlsegment.SelectedIndex = ddldesgn.SelectedIndex = ddlUnits.SelectedIndex = ddlcid.SelectedIndex = ddlZone.SelectedIndex = ddlArea.SelectedIndex = 0;
            ddlUnits.Visible = false;

            chkSubUnit.Checked = false;

            radioinvoiceyes.Checked = radioinvoiceno.Checked = radiopaysheetyes.Checked = radiopaysheetno.Checked = radioyesmu.Checked = radionomu.Checked = false;
            txtLocation.Text = string.Empty;
        }

        protected void ddlcid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlcid.SelectedIndex > 0)
            {
                LoadClientDetails();
            }
            else
            {
                ClearClientsFieldsData();
            }

        }

        protected void LoadClientDetails()
        {

            try
            {
                #region    Begin Code For Declare/Assign Values Variables as on [20-09-2013]
                var Clientid = txtclientid.Text;
                var SPName = "GetCilientsInfo";
                var DdlValue = "0";
                #endregion  End Code For Declare/Assign  Variables as on [20-09-2013]

                #region   Begin Code for Calling Stored Procedure as on [20-09-2013]
                Hashtable HTSpParameters = new Hashtable();
                HTSpParameters.Add("@Clientid", Clientid);
                DataTable DtClientInfo = config.ExecuteAdaptorAsyncWithParams(SPName, HTSpParameters).Result;
                #endregion End Code for Calling Stored Procedure as on [20-09-2013]

                #region  Begin Code for Check Records Are available for the Entered text as  on [09-10-2013]
                if (DtClientInfo.Rows.Count == 0)
                {
                    lblMsg.Text = "Client Details Are Not Available For the Entered Client ID/NAME";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Client Details Are Not Available For the Entered Client ID/NAME');", true);
                    ClearClientsFieldsData();
                    return;
                }
                #endregion End Code for Check Records Are available for the Entered text as  on [09-10-2013]


                #region    Begin Code For Assign  Data Column Values to Controls as on [20-09-2013]
                #region    Begin Code Client-Name to  Contact Person
                txtcname.Text = DtClientInfo.Rows[0]["ClientName"].ToString();
                txtshortname.Text = DtClientInfo.Rows[0]["ClientShortName"].ToString();
                DdlValue = DtClientInfo.Rows[0]["ClientSegment"].ToString();
                if (DdlValue != "0")
                {
                    ddlsegment.SelectedValue = DdlValue;
                }
                else
                {
                    ddlsegment.SelectedIndex = 0;
                }
                DdlValue = DtClientInfo.Rows[0]["Area"].ToString();
                if (DdlValue != "0")
                {
                    ddlArea.SelectedValue = DdlValue;
                }
                else
                {
                    ddlArea.SelectedIndex = 0;
                }
                DdlValue = DtClientInfo.Rows[0]["Zone"].ToString();
                if (DdlValue != "0")
                {
                    ddlZone.SelectedValue = DdlValue;
                }
                else
                {
                    ddlZone.SelectedIndex = 0;
                }
                txtcontactperson.Text = DtClientInfo.Rows[0]["ClientContactPerson"].ToString();
                #endregion  End Code Client-Name  to Contact Person

                #region  Begin Code  Person-Designation To PIN-No
                DdlValue = DtClientInfo.Rows[0]["ClientPersonDesgn"].ToString();
                if (DdlValue != "0")
                {
                    ddldesgn.SelectedValue = DdlValue;
                }
                else
                {
                    ddldesgn.SelectedIndex = 0;
                }
                txtphonenumbers.Text = DtClientInfo.Rows[0]["ClientPhonenumbers"].ToString();
                txtfaxno.Text = DtClientInfo.Rows[0]["ClientFax"].ToString();
                txtemailid.Text = DtClientInfo.Rows[0]["ClientEmail"].ToString();
                txtpin.Text = DtClientInfo.Rows[0]["ClientAddrPin"].ToString();

                if (DtClientInfo.Rows[0]["state"].ToString() != "0")
                {
                    ddlstate.SelectedValue = DtClientInfo.Rows[0]["state"].ToString();
                }
                else
                {
                    ddlstate.SelectedIndex = 0;

                }
                if (DtClientInfo.Rows[0]["statecode"].ToString() != "0")
                {
                    ddlStateCode.SelectedValue = DtClientInfo.Rows[0]["statecode"].ToString();
                }
                else
                {
                    ddlStateCode.SelectedIndex = 0;
                }

                ddlOurGSTIN.SelectedValue = DtClientInfo.Rows[0]["OurGSTIN"].ToString();
                txtGSTUniqueID.Text = DtClientInfo.Rows[0]["GSTIN"].ToString();


                #endregion  End Code  Person-Designation To PIN-No

                #region  Begin Code  Line-One To Line-Five
                txtchno.Text = DtClientInfo.Rows[0]["ClientAddrHno"].ToString();
                txtstreet.Text = DtClientInfo.Rows[0]["ClientAddrStreet"].ToString();
                txtarea.Text = DtClientInfo.Rows[0]["ClientAddrArea"].ToString();
                TxtCity.Text = DtClientInfo.Rows[0]["ClientAddrCity"].ToString();
                txtcolony.Text = DtClientInfo.Rows[0]["ClientAddrColony"].ToString();
                #endregion  End Code Line-One To Line-Five

                #region Begin Code Line Six To PaySheet
                txtstate.Text = DtClientInfo.Rows[0]["ClientAddrState"].ToString();
                DdlValue = DtClientInfo.Rows[0]["SubUnitStatus"].ToString();
                if (bool.Parse(DdlValue) == true)
                {
                    chkSubUnit.Checked = true;
                    ddlUnits.SelectedValue = DtClientInfo.Rows[0]["MainUnitId"].ToString();
                    ddlUnits.Visible = true;
                }
                else
                {

                    chkSubUnit.Checked = false;
                    ddlUnits.Visible = false;
                    ddlUnits.SelectedIndex = 0;
                }

                DdlValue = DtClientInfo.Rows[0]["MainUnitId"].ToString();
                if (DdlValue != "0")
                {
                    ddlUnits.SelectedValue = DdlValue;
                }
                else
                {
                    ddlUnits.SelectedIndex = 0;
                }

                DdlValue = DtClientInfo.Rows[0]["MAinunitStatus"].ToString();
                if (bool.Parse(DdlValue) == true)
                {
                    radioyesmu.Checked = true;
                    radionomu.Checked = false;
                }
                else
                {
                    radioyesmu.Checked = false;
                    radionomu.Checked = true;
                }


                DdlValue = DtClientInfo.Rows[0]["Invoice"].ToString();
                if (bool.Parse(DdlValue) == true)
                {
                    radioinvoiceyes.Checked = true;
                    radioinvoiceno.Checked = false;
                }
                else
                {
                    radioinvoiceyes.Checked = false;
                    radioinvoiceno.Checked = true;
                }


                DdlValue = DtClientInfo.Rows[0]["Paysheet"].ToString();
                if (bool.Parse(DdlValue) == true)
                {
                    radiopaysheetyes.Checked = true;
                    radiopaysheetno.Checked = false;
                }
                else
                {
                    radiopaysheetyes.Checked = false;
                    radiopaysheetno.Checked = true;
                }

                txtdescription.Text = DtClientInfo.Rows[0]["ClientDesc"].ToString();
                #endregion End Code Line Six To PaySheet

                txtLocation.Text = DtClientInfo.Rows[0]["Location"].ToString();

                #endregion End   Code For Assign  Data Column Values to Controls as on [20-09-2013]

            }

            catch (Exception ex)
            {


            }

        }

        protected void btncancel_Click(object sender, EventArgs e)
        {

            ClearClientsFieldsData();

        }

        protected void chkSubUnit_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSubUnit.Checked)
            {
                ddlUnits.Visible = true;
            }
            else
            {
                ddlUnits.Visible = false;
            }
        }

        protected void TxtClient_OnTextChanged(object sender, EventArgs e)
        {
            if (txtclientid.Text.Trim().Length == 0)
            {
                lblMsg.Text = "Please Enter the Client ID";
                //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter the Client ID');", true);
            }
            LoadClientDetails();

        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlArea.SelectedIndex > 0)
            {
                string Qry = "select Z.Zoneid from Area A inner join Zone Z on Z.ZoneID=A.ZoneID where AreaID='" + ddlArea.SelectedValue + "'";
                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Qry).Result;
                ddlZone.SelectedValue = dt.Rows[0]["Zoneid"].ToString();
            }
            else
            {
                ddlZone.SelectedIndex = 0;
            }
        }


    }
}