using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using System.IO;
using System.Data.OleDb;
using SRF.P.DAL;

namespace SRF.P.Module_Clients
{
    public partial class AddClient : System.Web.UI.Page
    {
        DataTable dt = null;
        string CmpIDPrefix = "";
        AppConfiguration config = new AppConfiguration();

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
                if (!IsPostBack)
                {
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                    clientid();
                    LoadSegments();
                    LoadDesignations();
                    LoadClients();
                    LoadopmEmpsIDs();
                    LoadOurGSTNos();
                    LoadStatenames();
                    LoadAreas();
                    LoadZones();

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
                Response.Redirect("~/Login.aspx");
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

        protected void LoadDivisions()
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
            ddlShipToSate.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlShipToStateCode.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlPTState.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlStateCode.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlPOSStateCode.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlPTState.SelectedValue = "12";
            ddlLWFState.Items.Insert(0, new ListItem("-Select-", "0"));

        }

        protected void LoadBranches()
        {

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


        private void clientid()
        {
            txtCId.Text = GlobalData.Instance.LoadMaxClientid(CmpIDPrefix);
        }

        protected void btnaddclint_Click(object sender, EventArgs e)
        {
            try
            {

                #region  Begin  Check Validations as on  [19-09-2013]

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

                #region  Begin Check Area Selected or ?
                if (ddlArea.SelectedIndex == 0)
                {
                    lblMsg.Text = "Please Select Area";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select Designation ');", true);
                    return;
                }
                #endregion End Check Area Selected or ?
                #region  Begin Check Zone Selected or ?
                if (ddlZone.SelectedIndex == 0)
                {
                    lblMsg.Text = "Please Select Zone";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select Designation ');", true);
                    return;
                }
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
                var MainUnitId = "0";
                var MAinunitStatus = 0;
                var Invoice = 0;
                var Paysheet = 0;
                var ClientDesc = string.Empty;
                #endregion End Code Line Six To PaySheet



                var Area = string.Empty;
                var Zone = string.Empty;
                var LWFState = "0";

                int Category = 0;
                var State = "0";
                var StateCode = "0";
                var GSTIN = "";
                var OurGSTIN = "";

                var ShiptoLine1 = "";
                var ShiptoLine2 = "";
                var ShiptoLine3 = "";
                var ShiptoLine4 = "";
                var ShiptoLine5 = "";
                var ShiptoLine6 = "";
                var ShipToState = "0";
                var ShipToStateCode = "0";
                var ShipToGSTIN = "";
                var Locationid = "0";
                var Fieldofficer = "0";
                var Areamanager = "0";
                var Branch = "";


                #region   Begin Extra Varibles for This Event   As on [20-09-2013]
                var IRecordStatus = 0;
                #endregion End Extra Varibles for This Event   As on [20-09-2013]

                #endregion  End Declare Variables as on [19-09-2013]

                #region    Begin Code For Assign Values Into Declared Variables as on [19-09-2013]
                #region    Begin Code Client-id to  Contact Person
                ClientId = txtCId.Text;
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




                //if (ddlCategory.SelectedIndex == 0)
                //{
                //    lblMsg.Text = "Please Select Category";
                //    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select Category');", true);
                //    return;
                //}

                //if (ddlCategory.SelectedIndex > 0)
                //{
                //    Category = ddlCategory.SelectedIndex;

                //}

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


                if (ddlLWFState.SelectedIndex > 0)
                {
                    LWFState = ddlLWFState.SelectedValue;
                }

                var PTState = "0";

                if (ddlPTState.SelectedIndex > 0)
                {
                    PTState = ddlPTState.SelectedValue;
                }


                #endregion End Code Line Six To PaySheet



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
                    Fieldofficer = "0";
                }
                else
                {
                    Fieldofficer = dllfieldofficer.SelectedValue;
                }


                if (ddllocation.Text.Length > 0)
                {
                    Locationid = ddllocation.Text;
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


                #region    Begin Code For Stored Procedure Parameters as on [20-09-2013]
                Hashtable AddClientDetails = new Hashtable();
                string AddClientDetailsPName = "ADDClientDetails";

                #region     Begin Code Client-id to  Contact Person
                AddClientDetails.Add("@ClientId", ClientId);
                AddClientDetails.Add("@ClientName", ClientName);
                AddClientDetails.Add("@ClientShortName", ClientShortName);
                AddClientDetails.Add("@ClientSegment", ClientSegment);
                AddClientDetails.Add("@ClientContactPerson", ClientContactPerson);
                #endregion  End Code Client-id to Contact Person

                #region  Begin Code  Person-Designation To PIN-No

                AddClientDetails.Add("@ClientPersonDesgn", ClientPersonDesgn);
                AddClientDetails.Add("@ClientPhonenumbers", ClientPhonenumbers);
                AddClientDetails.Add("@ClientFax", ClientFax);
                AddClientDetails.Add("@ClientEmail", ClientEmail);
                AddClientDetails.Add("@ClientAddrPin", ClientAddrPin);
                AddClientDetails.Add("@EmailCC", EmailCC);

                #endregion  End Code  Person-Designation To PIN-No

                #region  Begin Code  Line-One To Line-Five

                AddClientDetails.Add("@ClientAddrHno", ClientAddrHno);
                AddClientDetails.Add("@ClientAddrStreet", ClientAddrStreet);
                AddClientDetails.Add("@ClientAddrArea", ClientAddrArea);
                AddClientDetails.Add("@ClientAddrCity", ClientAddrCity);
                AddClientDetails.Add("@ClientAddrColony", ClientAddrColony);
                AddClientDetails.Add("@Line7", Line7);
                AddClientDetails.Add("@Line8", Line8);

                #endregion  End Code Line-One To Line-Five

                #region Begin Code Line Six To PaySheet

                AddClientDetails.Add("@ClientAddrState", ClientAddrState);
                AddClientDetails.Add("@SubUnitStatus", SubUnitStatus);
                AddClientDetails.Add("@MainUnitId", MainUnitId);
                AddClientDetails.Add("@MAinunitStatus", MAinunitStatus);
                AddClientDetails.Add("@Invoice", Invoice);
                AddClientDetails.Add("@Paysheet", Paysheet);
                AddClientDetails.Add("@ClientDesc", ClientDesc);
                AddClientDetails.Add("@ClientPrefix", CmpIDPrefix);

                #endregion End Code Line Six To PaySheet
              
                AddClientDetails.Add("@Category", Category);
                AddClientDetails.Add("@state", State);
                AddClientDetails.Add("@StateCode", StateCode);
                AddClientDetails.Add("@GSTIN", GSTIN);
                AddClientDetails.Add("@OurGSTIN", OurGSTIN);
                AddClientDetails.Add("@ShiptoLine1", ShiptoLine1);
                AddClientDetails.Add("@ShiptoLine2", ShiptoLine2);
                AddClientDetails.Add("@ShiptoLine3", ShiptoLine3);
                AddClientDetails.Add("@ShiptoLine4", ShiptoLine4);
                AddClientDetails.Add("@ShiptoLine5", ShiptoLine5);
                AddClientDetails.Add("@ShiptoLine6", ShiptoLine6);
                AddClientDetails.Add("@ShipToState", ShipToState);
                AddClientDetails.Add("@ShipToStateCode", ShipToStateCode);
                AddClientDetails.Add("@ShipToGSTIN", ShipToGSTIN);
                AddClientDetails.Add("@PTState", PTState);
                AddClientDetails.Add("@BuyersOrderNo", BuyersOrderNo);
                AddClientDetails.Add("@Locationid", Locationid);
                AddClientDetails.Add("@Fieldofficer", Fieldofficer);
                AddClientDetails.Add("@Areamanager", Areamanager);

                AddClientDetails.Add("@BillToLegalName", BillToLegalName);
                AddClientDetails.Add("@BillToAddr1", BillToAddr1);
                AddClientDetails.Add("@BillToAddr2", BillToAddr2);
                AddClientDetails.Add("@BillToLocation", BillToLocation);
                AddClientDetails.Add("@BillToPIN", BillToPIN);
                AddClientDetails.Add("@BillToPOS", BillToPOS);
                AddClientDetails.Add("@ShipToLegalName", ShipToLegalName);
                AddClientDetails.Add("@ShipToAddr1", ShipToAddr1);
                AddClientDetails.Add("@ShipToAddr2", ShipToAddr2);
                AddClientDetails.Add("@ShipToLocation", ShipToLocation);
                AddClientDetails.Add("@ShipToPIN", ShipToPIN);
                AddClientDetails.Add("@SupplyType", SupplyType);

                AddClientDetails.Add("@LWFState", LWFState);
                AddClientDetails.Add("@Area", Area);
                AddClientDetails.Add("@Zone", Zone);

                #endregion End Code For Stored Procedure Parameters as on [20-09-2013]

                #region     Begin Code For Calling Stored Procedure as on [20-09-2013]
                IRecordStatus = SqlHelper.Instance.ExecuteQuery(AddClientDetailsPName, AddClientDetails);
                #endregion   End   Code For Calling Stored Procedure as on [20-09-2013]

                #region     Begin Code For Status/Resulted Message of the Inserted Record as on [20-09-2013]

                if (IRecordStatus > 0)
                {
                    lblMsg.Text = "";
                    lblSuc.Text = "Client Details Added Sucessfully  With  Client Id   :- " + txtCId.Text + " ";

                    //ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Client Details Added Sucessfully  With  Client Id   :- " + txtCId.Text + " -: ');", true);
                    clientid();
                    ClearClientsFieldsData();

                    return;
                }
                else
                {
                    lblMsg.Text = "Client Details Not  Added Sucessfully  With  Client Id   :- " + txtCId.Text + " -: ";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Client Details Not  Added Sucessfully  With  Client Id   :- " + txtCId.Text + " -: ');", true);
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
            txtstate.Text = txtdescription.Text = txtGSTUniqueID.Text = txtEmailCC.Text =
            txtBillToAddr1.Text = txtBillToAddr2.Text = txtBillToLglName.Text = txtBillToLocation.Text = txtBillToPIN.Text =
            txtShipToAddr1.Text = txtShipToAddr2.Text = txtShipToLglName.Text = txtShipToLocation.Text = txtShipToPIN.Text = string.Empty;

            ddlsegment.SelectedIndex = ddlArea.SelectedIndex = ddldesgn.SelectedIndex = ddlUnits.SelectedIndex = ddlZone.SelectedIndex = ddlstate.SelectedIndex =
            ddlStateCode.SelectedIndex = dllfieldofficer.SelectedIndex = ddlPOSStateCode.SelectedIndex = ddlLWFState.SelectedIndex = 0;
            ddlUnits.Visible = false;

            chkSubUnit.Checked = false;

            radioinvoiceyes.Checked = radioinvoiceno.Checked = radiopaysheetyes.Checked = radiopaysheetno.Checked = radioyesmu.Checked = radionomu.Checked = false;

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

        #region Begin New code for client imports from excel on 24/02/2014 by venkat



        #endregion

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

        protected void ddlBillToSate_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void fillfieldofficer()
        {
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlArea.SelectedIndex > 0)
            //{

            //    ddlZone.SelectedValue = ddlArea.SelectedValue;
            //}
            //else
            //{
            //    ddlZone.SelectedIndex = 0;
            //}

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