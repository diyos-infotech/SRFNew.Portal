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

        protected void LoadShipStatenames()
        {

            DataTable DtStateNames = GlobalData.Instance.LoadStateNames();
            if (DtStateNames.Rows.Count > 0)
            {
                ddlShipToSate.DataValueField = "StateID";
                ddlShipToSate.DataTextField = "State";
                ddlShipToSate.DataSource = DtStateNames;
                ddlShipToSate.DataBind();


                ddlShipToStateCode.DataValueField = "StateID";
                ddlShipToStateCode.DataTextField = "GSTStateCode";
                ddlShipToStateCode.DataSource = DtStateNames;
                ddlShipToStateCode.DataBind();
            }
            ddlShipToSate.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlShipToStateCode.Items.Insert(0, new ListItem("-Select-", "0"));

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

        protected void LoadBranches()
        {

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


                ddlShipToSate.DataValueField = "StateID";
                ddlShipToSate.DataTextField = "State";
                ddlShipToSate.DataSource = DtStateNames;
                ddlShipToSate.DataBind();

                ddlShipToStateCode.DataValueField = "StateID";
                ddlShipToStateCode.DataTextField = "GSTStateCode";
                ddlShipToStateCode.DataSource = DtStateNames;
                ddlShipToStateCode.DataBind();


                ddlPTState.DataValueField = "StateID";
                ddlPTState.DataTextField = "State";
                ddlPTState.DataSource = DtStateNames;
                ddlPTState.DataBind();

                ddlPOSStateCode.DataValueField = "StateID";
                ddlPOSStateCode.DataTextField = "GSTStateCode";
                ddlPOSStateCode.DataSource = DtStateNames;
                ddlPOSStateCode.DataBind();

              

                ddlLWFState.DataValueField = "StateID";
                ddlLWFState.DataTextField = "State";
                ddlLWFState.DataSource = DtStateNames;
                ddlLWFState.DataBind();

            }
            ddlstate.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlStateCode.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlShipToSate.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlShipToStateCode.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlPTState.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlPOSStateCode.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlLWFState.Items.Insert(0, new ListItem("-Select-", "0"));

        }

        protected void LoadDivisions()
        {


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

        protected void fillfieldofficer()
        {


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
                if (txtCname.Text.Trim().Length == 0)
                {
                    lblMsg.Text = "Please Enter The Client Name";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Enter The Client Name ');", true);
                    return;
                }
                #endregion  End Check Client Name is  Empty or ?

                #region   Begin Check   Contact Person   Name
                //if (txtcontactperson.Text.Trim().Length == 0)
                //{
                //    lblMsg.Text = "Please fill Contact Person name";
                //    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please fill Contact Person name ');", true);
                //    return;
                //}
                #endregion  Begin Check   Contact Person   Name

                #region  Begin Check Designation Selected or ?
                //if (ddldesgn.SelectedIndex == 0)
                //{
                //    lblMsg.Text = "Please Select Designation";
                //    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select Designation ');", true);
                //    return;
                //}
                #endregion End Check Designation Selected or ?

                #region  Begin Check Zone Selected or ?
                //if (ddlZones.SelectedIndex == 0)
                //{
                //    lblMsg.Text = "Please Select Zone";
                //    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select Designation ');", true);
                //    return;
                //}
                #endregion End Check Zone Selected or ?

                #region Begin Check Phone Number Entered or ?
                //if (txtphonenumbers.Text.Trim().Length == 0)
                //{
                //    lblMsg.Text = "Please Enter the Phone No.";
                //    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Enter the Phone No.');", true);
                //    return;
                //}
                //if (txtphonenumbers.Text.Trim().Length < 8)
                //{
                //    lblMsg.Text = "Please enter a valid Phone No.";
                //    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please enter a valid Phone No.');", true);
                //    return;
                //}
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

                #region Begin Check Field Officer
                //if(dllfieldofficer.SelectedIndex==0)
                //{
                //    lblMsg.Text = "Please Select the  Field Officer";
                //    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select the  Field Officer');", true);
                //    return;
                //}
                #endregion End Check Field Officer


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
                var EmailCC = string.Empty;

                #endregion  End Code  Person-Designation To PIN-No

                #region  Begin Code  Line-One To Line-Five
                var ClientAddrHno = string.Empty;
                var ClientAddrStreet = string.Empty;
                var ClientAddrArea = string.Empty;
                var ClientAddrCity = string.Empty;
                var ClientAddrColony = string.Empty;
                var Line7 = "";
                var Line8 = "";
                #endregion  End Code Line-One To Line-Five

                #region Begin Code Line Six To PaySheet
                var ClientAddrState = string.Empty;
                var SubUnitStatus = string.Empty;
                var MainUnitId = string.Empty;
                var MAinunitStatus = 0;
                var Invoice = 0;
                var Paysheet = 0;
                var ClientDesc = string.Empty;
                #endregion End Code Line Six To PaySheet

                #region   Begin Extra Varibles for This Event   As on [20-09-2013]
                var URecordStatus = 0;
                #endregion End Extra Varibles for This Event   As on [20-09-2013]


                var Area = string.Empty;
                var Zone = string.Empty; var LWFState = "0";
                int Category = 0;
                var State = "0";
                var StateCode = "0";
                var GSTIN = "";
                var OurGSTIN = "";


                #endregion  End Declare Variables as on [19-09-2013]

                var ShiptoLine1 = "";
                var ShiptoLine2 = "";
                var ShiptoLine3 = "";
                var ShiptoLine4 = "";
                var ShiptoLine5 = "";
                var ShiptoLine6 = "";
                var ShipToState = "0";
                var ShipToStateCode = "0";
                var ShipToGSTIN = "";
                var Division = "0";
                var ESIBranch = "0";
                var Location = "";
                var FieldOfficer = "";
                var AreaManager = "0";
                var Branch = "";
                var BillSeq = string.Empty;
                var PONumber = "";
                var ClientSector = 0;


                #region    Begin Code For Assign Values Into Declared Variables as on [19-09-2013]
                #region    Begin Code Client-id to  Contact Person
                //ClientId =ddlcid.SelectedValue;
                ClientId = txtclientid.Text;
                ClientName = txtCname.Text;
                ClientShortName = txtshortname.Text;
                if (ddlsegment.SelectedIndex == 0)
                {
                    ClientSegment = "0";
                }
                else
                {
                    ClientSegment = ddlsegment.SelectedValue;
                }

                ClientContactPerson = txtcontactperson.Text;
                #endregion  End Code Client-id to Contact Person

                #region  Begin Code  Person-Designation To PIN-No
                ClientPersonDesgn = ddldesgn.SelectedValue;
                ClientPhonenumbers = txtphonenumbers.Text;
                ClientFax = txtfaxno.Text;
                ClientEmail = txtemailid.Text;
                ClientAddrPin = txtpin.Text;
                EmailCC = txtEmailCC.Text;

                #endregion  End Code  Person-Designation To PIN-No

                #region  Begin Code  Line-One To Line-Five
                ClientAddrHno = txtchno.Text;
                ClientAddrStreet = txtstreet.Text;
                ClientAddrArea = txtarea.Text;
                ClientAddrCity = txtcity.Text;
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



                if (ddlZones.SelectedIndex > 0)
                {
                    Zone = ddlZones.SelectedValue;
                }

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

                string BuyersOrderNo = "";
                BuyersOrderNo = txtBuyerOrderNo.Text;




                var PTState = "0";

                if (ddlPTState.SelectedIndex > 0)
                {
                    PTState = ddlPTState.SelectedValue;
                }



                if (txtShipToLine1.Text.Trim().Length > 0)
                {
                    ShiptoLine1 = txtShipToLine1.Text;
                }


                if (txtShipToLine2.Text.Trim().Length > 0)
                {
                    ShiptoLine2 = txtShipToLine2.Text;
                }

                if (txtShipToLine3.Text.Trim().Length > 0)
                {
                    ShiptoLine3 = txtShipToLine3.Text;
                }


                if (txtShipToLine4.Text.Trim().Length > 0)
                {
                    ShiptoLine4 = txtShipToLine4.Text;
                }

                if (txtShipToLine5.Text.Trim().Length > 0)
                {
                    ShiptoLine5 = txtShipToLine5.Text;
                }

                if (txtShipToLine6.Text.Trim().Length > 0)
                {
                    ShiptoLine6 = txtShipToLine6.Text;
                }

                if (txtShipToGSTIN.Text.Trim().Length > 0)
                {
                    ShipToGSTIN = txtShipToGSTIN.Text;
                }


                if (dllfieldofficer.SelectedIndex == 0)
                {
                    FieldOfficer = "0";
                }
                else
                {
                    FieldOfficer = dllfieldofficer.SelectedValue;
                }




                if (ddllocation.Text.Length > 0)
                {
                    Location = ddllocation.Text;
                }


                ShipToState = ddlShipToSate.SelectedValue;
                ShipToStateCode = ddlShipToStateCode.SelectedValue;

                var SupplyType = ddlSupplyType.SelectedValue;


                var BillToLegalName = "";
                var BillToAddr1 = "";
                var BillToAddr2 = "";
                var BillToLocation = "";
                var BillToPOS = "0";
                var BillToPIN = "0";


                if (txtBillToLglName.Text.Trim().Length > 0)
                {
                    BillToLegalName = txtBillToLglName.Text;
                }
                if (txtBillToAddr1.Text.Trim().Length > 0)
                {
                    BillToAddr1 = txtBillToAddr1.Text;
                }

                if (txtBillToAddr2.Text.Trim().Length > 0)
                {
                    BillToAddr2 = txtBillToAddr2.Text;
                }
                if (txtBillToLocation.Text.Trim().Length > 0)
                {
                    BillToLocation = txtBillToLocation.Text;
                }

                if (txtBillToPIN.Text.Trim().Length > 0)
                {
                    BillToPIN = txtBillToPIN.Text;
                }

                if (ddlPOSStateCode.SelectedIndex > 0)
                {
                    BillToPOS = ddlPOSStateCode.SelectedValue;
                }



                var ShipToLegalName = "";
                var ShipToAddr1 = "";
                var ShipToAddr2 = "";
                var ShipToLocation = "";
                var ShipToPIN = "0";

                if (txtShipToLglName.Text.Trim().Length > 0)
                {
                    ShipToLegalName = txtShipToLglName.Text;
                }
                if (txtShipToAddr1.Text.Trim().Length > 0)
                {
                    ShipToAddr1 = txtShipToAddr1.Text;
                }

                if (txtShipToAddr2.Text.Trim().Length > 0)
                {
                    ShipToAddr2 = txtShipToAddr2.Text;
                }

                if (txtShipToPIN.Text.Trim().Length > 0)
                {
                    ShipToPIN = txtShipToPIN.Text;
                }

                if (txtShipToLocation.Text.Trim().Length > 0)
                {
                    ShipToLocation = txtShipToLocation.Text;
                }

                #endregion   End Code For Assign Values Into Declared Variables as on [19-09-2013]

                if (ddlLWFState.SelectedIndex > 0)
                {
                    LWFState = ddlLWFState.SelectedValue;
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
                ModifyClientDetails.Add("@EmailCC", EmailCC);

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


                #endregion End Code Line Six To PaySheet


                ModifyClientDetails.Add("@Category", Category);
                ModifyClientDetails.Add("@state", State);
                ModifyClientDetails.Add("@StateCode", StateCode);
                ModifyClientDetails.Add("@GSTIN", GSTIN);
                ModifyClientDetails.Add("@OurGSTIN", OurGSTIN);
                ModifyClientDetails.Add("@Line7", Line7);
                ModifyClientDetails.Add("@Line8", Line8);
               

                ModifyClientDetails.Add("@ShiptoLine1", ShiptoLine1);
                ModifyClientDetails.Add("@ShiptoLine2", ShiptoLine2);
                ModifyClientDetails.Add("@ShiptoLine3", ShiptoLine3);
                ModifyClientDetails.Add("@ShiptoLine4", ShiptoLine4);
                ModifyClientDetails.Add("@ShiptoLine5", ShiptoLine5);
                ModifyClientDetails.Add("@ShiptoLine6", ShiptoLine6);
                ModifyClientDetails.Add("@ShipToState", ShipToState);
                ModifyClientDetails.Add("@ShipToStateCode", ShipToStateCode);
                ModifyClientDetails.Add("@ShipToGSTIN", ShipToGSTIN);
                ModifyClientDetails.Add("@Location", Location);
              

                ModifyClientDetails.Add("@BillToLegalName", BillToLegalName);
                ModifyClientDetails.Add("@BillToAddr1", BillToAddr1);
                ModifyClientDetails.Add("@BillToAddr2", BillToAddr2);
                ModifyClientDetails.Add("@BillToLocation", BillToLocation);
                ModifyClientDetails.Add("@BillToPIN", BillToPIN);
                ModifyClientDetails.Add("@BillToPOS", BillToPOS);
                ModifyClientDetails.Add("@ShipToLegalName", ShipToLegalName);
                ModifyClientDetails.Add("@ShipToAddr1", ShipToAddr1);
                ModifyClientDetails.Add("@ShipToAddr2", ShipToAddr2);
                ModifyClientDetails.Add("@ShipToLocation", ShipToLocation);
                ModifyClientDetails.Add("@ShipToPIN", ShipToPIN);
                ModifyClientDetails.Add("@SupplyType", SupplyType);
                ModifyClientDetails.Add("@LWFState", LWFState);
                ModifyClientDetails.Add("@Area", Area);
                ModifyClientDetails.Add("@Zone", Zone);

                #endregion End Code For Stored Procedure Parameters as on [20-09-2013]


                #region     Begin Code For Calling Stored Procedure as on [20-09-2013]
                URecordStatus = SqlHelper.Instance.ExecuteQuery(ModifyClientDetailsPName, ModifyClientDetails);
                #endregion   End   Code For Calling Stored Procedure as on [20-09-2013]


                #region     Begin Code For Status/Resulted Message of the Inserted Record as on [20-09-2013]

                if (URecordStatus > 0)
                {
                    lblMsg.Text = "";
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

            txtCname.Text = txtshortname.Text = txtcontactperson.Text = txtphonenumbers.Text = txtfaxno.Text = ddllocation.Text = txtemailid.Text =
            txtpin.Text = txtchno.Text = txtstreet.Text = txtarea.Text = txtcity.Text = txtcolony.Text =
            txtstate.Text = txtdescription.Text = txtclientid.Text = string.Empty;

            ddlsegment.SelectedIndex = ddldesgn.SelectedIndex = ddlUnits.SelectedIndex = ddlcid.SelectedIndex = ddlZones.SelectedIndex = ddlOurGSTIN.SelectedIndex = dllfieldofficer.SelectedIndex = ddlAreamanager.SelectedIndex = 0;
            ddlUnits.Visible = false;

            chkSubUnit.Checked = false;

            radioinvoiceyes.Checked = radioinvoiceno.Checked = radiopaysheetyes.Checked = radiopaysheetno.Checked = radioyesmu.Checked = radionomu.Checked = false;

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
                DataTable DtClientInfo = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPName, HTSpParameters);
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
                txtCname.Text = DtClientInfo.Rows[0]["ClientName"].ToString();
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
                txtEmailCC.Text = DtClientInfo.Rows[0]["EmailCC"].ToString();


                #endregion  End Code  Person-Designation To PIN-No

                #region  Begin Code  Line-One To Line-Five
                txtchno.Text = DtClientInfo.Rows[0]["ClientAddrHno"].ToString();
                txtstreet.Text = DtClientInfo.Rows[0]["ClientAddrStreet"].ToString();
                txtarea.Text = DtClientInfo.Rows[0]["ClientAddrArea"].ToString();
                txtcity.Text = DtClientInfo.Rows[0]["ClientAddrCity"].ToString();
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
                txtShipToLine1.Text = DtClientInfo.Rows[0]["ShiptoLine1"].ToString();
                txtShipToLine2.Text = DtClientInfo.Rows[0]["ShiptoLine2"].ToString();
                txtShipToLine3.Text = DtClientInfo.Rows[0]["ShiptoLine3"].ToString();
                txtShipToLine4.Text = DtClientInfo.Rows[0]["ShiptoLine4"].ToString();
                txtShipToLine5.Text = DtClientInfo.Rows[0]["ShiptoLine5"].ToString();
                txtShipToLine6.Text = DtClientInfo.Rows[0]["ShiptoLine6"].ToString();
                txtShipToGSTIN.Text = DtClientInfo.Rows[0]["ShipToGSTIN"].ToString();


                if (DtClientInfo.Rows[0]["ShipToState"].ToString() != "0")
                {
                    ddlShipToSate.SelectedValue = DtClientInfo.Rows[0]["ShipToState"].ToString();
                }
                else
                {
                    ddlShipToSate.SelectedIndex = 0;

                }
                if (DtClientInfo.Rows[0]["ShipToStateCode"].ToString() != "0")
                {
                    ddlShipToStateCode.SelectedValue = DtClientInfo.Rows[0]["ShipToStateCode"].ToString();
                }
                else
                {
                    ddlShipToStateCode.SelectedIndex = 0;
                }



                ddllocation.Text = DtClientInfo.Rows[0]["Location"].ToString();

                txtBillToLglName.Text = DtClientInfo.Rows[0]["BillToLegalName"].ToString();
                txtBillToAddr1.Text = DtClientInfo.Rows[0]["BillToAddr1"].ToString();
                txtBillToAddr2.Text = DtClientInfo.Rows[0]["BillToAddr2"].ToString();
                txtBillToLocation.Text = DtClientInfo.Rows[0]["BillToLocation"].ToString();
                txtBillToPIN.Text = DtClientInfo.Rows[0]["BillToPIN"].ToString();
                if (DtClientInfo.Rows[0]["BillToPOS"].ToString() != "0")
                {
                    ddlPOSStateCode.SelectedValue = DtClientInfo.Rows[0]["BillToPOS"].ToString();
                }
                else
                {
                    ddlPOSStateCode.SelectedIndex = 0;

                }

                txtShipToLglName.Text = DtClientInfo.Rows[0]["ShipToLegalName"].ToString();
                txtShipToAddr1.Text = DtClientInfo.Rows[0]["ShipToAddr1"].ToString();
                txtShipToAddr2.Text = DtClientInfo.Rows[0]["ShipToAddr2"].ToString();
                txtShipToLocation.Text = DtClientInfo.Rows[0]["ShipToLocation"].ToString();
                txtShipToPIN.Text = DtClientInfo.Rows[0]["ShipToPIN"].ToString();
                ddlSupplyType.SelectedValue = DtClientInfo.Rows[0]["SupplyType"].ToString();


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

                if (DtClientInfo.Rows[0]["LWFState"].ToString() != "0")
                {
                    ddlLWFState.SelectedValue = DtClientInfo.Rows[0]["LWFState"].ToString();
                }
                else
                {
                    ddlLWFState.SelectedIndex = 0;

                }




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
            }
            LoadClientDetails();

        }

        protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qry = "select GSTstatecode,stateid from states where stateid='" + ddlstate.SelectedValue + "'";
            DataTable dt = SqlHelper.Instance.GetTableByQuery(qry);
            if (dt.Rows.Count > 0)
            {

                if (dt.Rows[0]["stateid"].ToString() != "0")
                {
                    ddlStateCode.SelectedValue = dt.Rows[0]["stateid"].ToString();
                    ddlPOSStateCode.SelectedValue = dt.Rows[0]["stateid"].ToString();

                }
                else
                {
                    ddlStateCode.SelectedIndex = 0;
                    ddlPOSStateCode.SelectedIndex = 0;

                }

            }
            else
            {
                ddlStateCode.SelectedIndex = 0;
                ddlPOSStateCode.SelectedIndex = 0;


            }
        }

        protected void ddlShipToSate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qry = "select GSTstatecode,stateid from states where stateid='" + ddlShipToSate.SelectedValue + "'";
            DataTable dt = SqlHelper.Instance.GetTableByQuery(qry);
            if (dt.Rows.Count > 0)
            {

                if (dt.Rows[0]["stateid"].ToString() != "0")
                {
                    ddlShipToStateCode.SelectedValue = dt.Rows[0]["stateid"].ToString();
                }
                else
                {
                    ddlShipToStateCode.SelectedIndex = 0;
                }
            }
            else
            {
                ddlShipToStateCode.SelectedIndex = 0;

            }
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