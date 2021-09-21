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
                    string ImagesFolderPath = Server.MapPath("ImportDocuments");
                    string[] filePaths = Directory.GetFiles(ImagesFolderPath);

                    foreach (string file in filePaths)
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
                Response.Redirect("~/Login.aspx");
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
                var Area = string.Empty;
                var Zone = string.Empty;
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
                var MainUnitId = "0";
                var MAinunitStatus = 0;
                var Invoice = 0;
                var Paysheet = 0;
                var ClientDesc = string.Empty;
                #endregion End Code Line Six To PaySheet


                var Location = "";

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

                if (txtLocation.Text.Trim().Length > 0)
                {
                    Location = txtLocation.Text;
                }

                #endregion   End Code For Assign Values Into Declared Variables as on [19-09-2013]

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
                AddClientDetails.Add("@state", State);
                AddClientDetails.Add("@StateCode", StateCode);
                AddClientDetails.Add("@GSTIN", GSTIN);
                AddClientDetails.Add("@OurGSTIN", OurGSTIN);
                #endregion  End Code  Person-Designation To PIN-No


                #region  Begin Code  Line-One To Line-Five

                AddClientDetails.Add("@ClientAddrHno", ClientAddrHno);
                AddClientDetails.Add("@ClientAddrStreet", ClientAddrStreet);
                AddClientDetails.Add("@ClientAddrArea", ClientAddrArea);
                AddClientDetails.Add("@ClientAddrCity", ClientAddrCity);
                AddClientDetails.Add("@ClientAddrColony", ClientAddrColony);

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
                AddClientDetails.Add("@Area", Area);
                AddClientDetails.Add("@Zone", Zone);
                #endregion End Code Line Six To PaySheet

                AddClientDetails.Add("@Location", Location);

                #endregion End Code For Stored Procedure Parameters as on [20-09-2013]

                #region     Begin Code For Calling Stored Procedure as on [20-09-2013]
                IRecordStatus = config.ExecuteNonQueryParamsAsync(AddClientDetailsPName, AddClientDetails).Result;
                #endregion   End   Code For Calling Stored Procedure as on [20-09-2013]

                #region     Begin Code For Status/Resulted Message of the Inserted Record as on [20-09-2013]

                if (IRecordStatus > 0)
                {
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

            txtCname.Text = txtshortname.Text = txtcontactperson.Text = txtphonenumbers.Text = txtfaxno.Text = txtemailid.Text =
            txtpin.Text = txtchno.Text = txtstreet.Text = txtarea.Text = txtcity.Text = txtcolony.Text =
            txtstate.Text = txtdescription.Text = txtGSTUniqueID.Text = string.Empty;

            ddlsegment.SelectedIndex = ddldesgn.SelectedIndex = ddlUnits.SelectedIndex = ddlstate.SelectedIndex = ddlStateCode.SelectedIndex = ddlZone.SelectedIndex = ddlArea.SelectedIndex = 0;
            ddlUnits.Visible = false;

            chkSubUnit.Checked = false;

            radioinvoiceyes.Checked = radioinvoiceno.Checked = radiopaysheetyes.Checked = radiopaysheetno.Checked = radioyesmu.Checked = radionomu.Checked = false;
            txtLocation.Text = string.Empty;
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

        protected void btnImportData_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = Path.Combine(Server.MapPath("ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(fileupload1.PostedFile.FileName));
                fileupload1.PostedFile.SaveAs(filename);
                string extn = Path.GetExtension(fileupload1.PostedFile.FileName);
                string constring = string.Empty;

                if (extn.ToLower() == ".xls")
                {
                    constring = "Provider=Microsoft.Jet.OLEDB.8.0;Data Source='" + filename + "';Extended Properties=\"excel 8.0;HDR=Yes;IMEX\"";
                }
                if (extn.ToLower() == ".xlsx")
                {
                    constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filename + "';Extended Properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
                }
                string qry = "select [Client Id],[Client Name],[Short Name],[Segment],[Contact Person],[Designation]," +
                        " [Phone],[Fax No],[Email Id],[Pin No],[Line One],[Line Two],[Line Three],[Line Four]," +
                        " [Line Five],[Line Six],[Description],[Sub Unit],[Main Unit],[Pay Sheet],[Invoice],[Area],[Zone] from [" + GetExcelSheetNames() + "]" + "";
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

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    var ClientId = string.Empty;
                    var Name = string.Empty;
                    var ShortName = string.Empty;
                    var Segment = string.Empty;
                    var Contactperson = string.Empty;
                    var Designation = string.Empty;
                    var Phoneno = string.Empty;
                    var Emailid = string.Empty;
                    var Faxno = string.Empty;
                    var Pinno = string.Empty;
                    var ClientAddrHno = string.Empty;
                    var ClientAddrStreet = string.Empty;
                    var ClientAddrArea = string.Empty;
                    var ClientAddrCity = string.Empty;
                    var ClientAddrColony = string.Empty;
                    var ClientAddrState = string.Empty;
                    var Description = string.Empty;

                    int Subunit = 0;
                    int Mainunit = 0;
                    int Paysheet = 0;
                    int Invoice = 0;
                    int MAinunitStatus = 0;

                    var IRecordStatus = 0;
                    var URecordStatus = 0;
                    var Area = string.Empty;
                    var Zone = string.Empty;

                    ClientId = dr["Client Id"].ToString();
                    Name = dr["Client Name"].ToString();
                    ShortName = dr["Short Name"].ToString();
                    Segment = dr["Segment"].ToString();
                    Contactperson = dr["Contact Person"].ToString();
                    Designation = dr["Designation"].ToString();
                    Phoneno = dr["Phone"].ToString();
                    Emailid = dr["Email Id"].ToString();
                    Faxno = dr["Fax No"].ToString();
                    Pinno = dr["Pin No"].ToString();
                    ClientAddrHno = dr["Line One"].ToString();
                    ClientAddrStreet = dr["Line Two"].ToString();
                    ClientAddrArea = dr["Line Three"].ToString();
                    ClientAddrCity = dr["Line Four"].ToString();
                    ClientAddrColony = dr["Line Five"].ToString();
                    ClientAddrState = dr["Line Six"].ToString();
                    Description = dr["Description"].ToString();
                    Area = dr["Area"].ToString();
                    Zone = dr["Zone"].ToString();

                    // MAinunitStatus = int.Parse(dr["Main Unit"].ToString().ToString());

                    if (String.IsNullOrEmpty(dr["Sub Unit"].ToString()) == false)
                    {
                        Subunit = int.Parse(dr["Sub Unit"].ToString());
                    }
                    if (String.IsNullOrEmpty(dr["Main Unit"].ToString()) == false)
                    {
                        Mainunit = int.Parse(dr["Main Unit"].ToString());
                    }

                    if (String.IsNullOrEmpty(dr["Pay Sheet"].ToString()) == false)
                    {
                        Paysheet = int.Parse(dr["Pay Sheet"].ToString());
                    }

                    if (String.IsNullOrEmpty(dr["Invoice"].ToString()) == false)
                    {
                        Invoice = int.Parse(dr["Invoice"].ToString());
                    }

                    if (ClientId.Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Client Id');", true);
                        return;
                    }
                    if (Name.Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Client Name');", true);
                        return;
                    }
                    if (Contactperson.Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Contact Person');", true);
                        return;
                    }
                    if (Designation.Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Designation');", true);
                        return;
                    }
                    if (Phoneno.Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Phone No');", true);
                        return;
                    }


                    string sqlclient = "select * from clients where clientid='" + ClientId + "'";
                    DataTable dtclient = config.ExecuteReaderWithQueryAsync(sqlclient).Result;
                    if (dtclient.Rows.Count > 0)
                    {
                        Hashtable ModifyClientDetails = new Hashtable();
                        string ModifyClientDetailsPName = "ModifyClientDetails";

                        #region     Begin Code Client-id to  Contact Person
                        ModifyClientDetails.Add("@ClientId", ClientId);
                        ModifyClientDetails.Add("@ClientName", Name);
                        ModifyClientDetails.Add("@ClientShortName", ShortName);
                        ModifyClientDetails.Add("@ClientSegment", Segment);
                        ModifyClientDetails.Add("@ClientContactPerson", Contactperson);
                        #endregion  End Code Client-id to Contact Person


                        #region  Begin Code  Person-Designation To PIN-No

                        ModifyClientDetails.Add("@ClientPersonDesgn", Designation);
                        ModifyClientDetails.Add("@ClientPhonenumbers", Phoneno);
                        ModifyClientDetails.Add("@ClientFax", Faxno);
                        ModifyClientDetails.Add("@ClientEmail", Emailid);
                        ModifyClientDetails.Add("@ClientAddrPin", Pinno);

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
                        ModifyClientDetails.Add("@SubUnitStatus", Subunit);
                        ModifyClientDetails.Add("@MainUnitId", Mainunit);
                        ModifyClientDetails.Add("@MAinunitStatus", MAinunitStatus);
                        ModifyClientDetails.Add("@Invoice", Invoice);
                        ModifyClientDetails.Add("@Paysheet", Paysheet);
                        ModifyClientDetails.Add("@ClientDesc", Description);
                        ModifyClientDetails.Add("@ClientPrefix", CmpIDPrefix);
                        ModifyClientDetails.Add("@Area", Area);
                        ModifyClientDetails.Add("@Zone", Zone);
                        #endregion End Code Line Six To PaySheet

                        URecordStatus = config.ExecuteNonQueryParamsAsync(ModifyClientDetailsPName, ModifyClientDetails).Result;

                        if (URecordStatus > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Client Details Modified Sucessfully.  With  Client Id   :- " + ClientId + " -: ');", true);

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Client Details Not  Added Sucessfully  With  Client Id   :- " + ClientId + " -: ');", true);
                            //return;
                        }
                    }
                    else
                    {


                        Hashtable AddClientDetails = new Hashtable();
                        string AddClientDetailsPName = "ADDClientDetails";

                        #region     Begin Code Client-id to  Contact Person
                        AddClientDetails.Add("@ClientId", ClientId);
                        AddClientDetails.Add("@ClientName", Name);
                        AddClientDetails.Add("@ClientShortName", ShortName);
                        AddClientDetails.Add("@ClientSegment", Segment);
                        AddClientDetails.Add("@ClientContactPerson", Contactperson);
                        #endregion  End Code Client-id to Contact Person


                        #region  Begin Code  Person-Designation To PIN-No

                        AddClientDetails.Add("@ClientPersonDesgn", Designation);
                        AddClientDetails.Add("@ClientPhonenumbers", Phoneno);
                        AddClientDetails.Add("@ClientFax", Faxno);
                        AddClientDetails.Add("@ClientEmail", Emailid);
                        AddClientDetails.Add("@ClientAddrPin", Pinno);

                        #endregion  End Code  Person-Designation To PIN-No


                        #region  Begin Code  Line-One To Line-Five

                        AddClientDetails.Add("@ClientAddrHno", ClientAddrHno);
                        AddClientDetails.Add("@ClientAddrStreet", ClientAddrStreet);
                        AddClientDetails.Add("@ClientAddrArea", ClientAddrArea);
                        AddClientDetails.Add("@ClientAddrCity", ClientAddrCity);
                        AddClientDetails.Add("@ClientAddrColony", ClientAddrColony);

                        #endregion  End Code Line-One To Line-Five

                        #region Begin Code Line Six To PaySheet

                        AddClientDetails.Add("@ClientAddrState", ClientAddrState);
                        AddClientDetails.Add("@SubUnitStatus", Subunit);
                        AddClientDetails.Add("@MainUnitId", Mainunit);
                        AddClientDetails.Add("@MAinunitStatus", MAinunitStatus);
                        AddClientDetails.Add("@Invoice", Invoice);
                        AddClientDetails.Add("@Paysheet", Paysheet);
                        AddClientDetails.Add("@ClientDesc", Description);
                        AddClientDetails.Add("@ClientPrefix", CmpIDPrefix);
                        AddClientDetails.Add("@Zone", Zone);
                        AddClientDetails.Add("@Area", Area);
                        #endregion End Code Line Six To PaySheet

                        IRecordStatus = config.ExecuteNonQueryParamsAsync(AddClientDetailsPName, AddClientDetails).Result;

                        if (IRecordStatus > 0)
                        {

                            ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Client Details Added Sucessfully  With  Client Id   :- " + txtCId.Text + " -: ');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Client Details Not  Added Sucessfully  With  Client Id   :- " + txtCId.Text + " -: ');", true);

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please upload valid data');", true);
            }

        }

        #endregion

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