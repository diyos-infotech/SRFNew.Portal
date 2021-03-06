<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Clients/Clients.master" AutoEventWireup="true" CodeBehind="ModifyClient.aspx.cs" Inherits="SRF.P.Module_Clients.ModifyClient" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Calendar.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/jscript.js"> 
    </script>


    <div id="content-holder">
        <div class="content-holder">
            <div class="col-md-12" style="margin-top: 8px; margin-bottom: 8px">
                <div align="center">
                    <asp:Label ID="lblMsg" runat="server" Style="border-color: #f0c36d; background-color: #f9edbe; width: auto; font-weight: bold; color: #CC3300;"></asp:Label>
                </div>
                <div align="center">
                    <asp:Label ID="lblSuc" runat="server" Style="border-color: #f0c36d; background-color: #f9edbe; width: auto; font-weight: bold; color: #000;"></asp:Label>
                </div>
                <div class="panel panel-inverse">
                    <div class="panel-heading">
                        <table width="100%">
                            <tr>
                                <td>
                                    <h3 class="panel-title">Edit Client</h3>
                                </td>
                                <td align="right"><< <a href="Clients.aspx" style="color: #003366">Back</a>  </td>
                            </tr>
                        </table>


                    </div>
                    <div class="panel-body">
                        <!--  Content to be add here> -->
                        <asp:ScriptManager runat="server" ID="Scriptmanager1">
                        </asp:ScriptManager>

                        <div class="boxin" style="height: 450px; width: 950px; margin-left: 7px">
                            <div class="dashboard_firsthalf" style="width: 100%">
                                <table width="100%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td valign="top">
                                            <table width="100%" cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td>Client ID<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlcid" runat="server" class="sdrop" AutoPostBack="True" TabIndex="1"
                                                            OnSelectedIndexChanged="ddlcid_SelectedIndexChanged" Visible="false">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtclientid" class="sinput" AutoPostBack="true" MaxLength="10"
                                                            runat="server" TabIndex="1" OnTextChanged="TxtClient_OnTextChanged"></asp:TextBox>

                                                        <cc1:AutoCompleteExtender ID="ACECnlientIds" runat="server"
                                                            TargetControlID="txtclientid"
                                                            ServicePath="~/AutoCompleteAA.asmx"
                                                            ServiceMethod="GetClientids"
                                                            MinimumPrefixLength="1"
                                                            CompletionSetCount="10" EnableCaching="true"
                                                            CompletionInterval="1"
                                                            CompletionListCssClass="autocomplete_completionListElement"
                                                            CompletionListItemCssClass="autocomplete_listItem"
                                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            ShowOnlyCurrentWordInCompletionListItem="true" DelimiterCharacters=";,">
                                                        </cc1:AutoCompleteExtender>



                                                    </td>
                                                </tr>
                                    </tr>
                                    <tr>
                                        <td>Short Name
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtshortname" class="sinput" TabIndex="3" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Contact Person<%--<span style="color: Red">*</span>--%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtcontactperson" runat="server" TabIndex="5" class="sinput"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Phone No(s)<%--<span style="color: Red">*</span>--%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtphonenumbers" runat="server" TabIndex="7" class="sinput" MaxLength="50"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                                runat="server" Enabled="True" TargetControlID="txtphonenumbers"
                                                ValidChars="0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Email-ID
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtemailid" runat="server" class="sinput" TabIndex="9" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>



                                    <tr>
                                        <td><%--Zone--%><span style="color: Red"> <%--*--%> </span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlZones" runat="server" class="sdrop" TabIndex="9" Visible="false"></asp:DropDownList>
                                        </td>
                                    </tr>




                                    <tr>
                                        <td>Our GSTIN
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlOurGSTIN" runat="server" class="sdrop" TabIndex="9"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Type of Supply</td>
                                        <td>
                                            <asp:DropDownList ID="ddlSupplyType" runat="server" class="sdrop">
                                                <asp:ListItem>B2B</asp:ListItem>
                                                <asp:ListItem>B2C</asp:ListItem>
                                                <asp:ListItem>SEZWP</asp:ListItem>
												<asp:ListItem>SEZWOP</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>


                                    <tr style="font-weight: bold">
                                        <td colspan="2">Billing Details
                                        </td>

                                    </tr>


                                    <tr>
                                        <td style="font-weight: bold; text-decoration: underline">Bill To
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td>Legal Name
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillToLglName" runat="server" TabIndex="15" class="sinput" MaxLength="100"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Addr 1
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillToAddr1" runat="server" TabIndex="15" class="sinput" MaxLength="100"></asp:TextBox>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Addr 2
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillToAddr2" runat="server" TabIndex="15" class="sinput" MaxLength="100"></asp:TextBox>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Location
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillToLocation" runat="server" TabIndex="15" class="sinput" MaxLength="50"></asp:TextBox>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td>PIN
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillToPIN" runat="server" TabIndex="15" class="sinput" MaxLength="6"></asp:TextBox>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td>GSTIN/Unique ID </td>
                                        <td>
                                            <asp:TextBox ID="txtGSTUniqueID" runat="server" TextMode="MultiLine" TabIndex="21"
                                                class="sinput"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>State
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlstate" TabIndex="19" class="sdrop" AutoPostBack="true" OnSelectedIndexChanged="ddlstate_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>State Code</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlStateCode" TabIndex="21" class="sdrop">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>POS (State Code)</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlPOSStateCode" TabIndex="19" class="sdrop">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td style="padding-left: 40px">Line 1</td>
                                        <td>
                                            <asp:TextBox ID="txtchno" runat="server" class="sinput" TabIndex="12">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 40px">Line 2</td>
                                        <td>
                                            <asp:TextBox ID="txtstreet" runat="server" class="sinput" TabIndex="12">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 40px">Line 3</td>
                                        <td>
                                            <asp:TextBox ID="txtarea" runat="server" class="sinput" TabIndex="12">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 40px">Line 4</td>
                                        <td>
                                            <asp:TextBox ID="txtcolony" runat="server" class="sinput" TabIndex="12">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 40px">Line 5</td>
                                        <td>
                                            <asp:TextBox ID="txtcity" runat="server" class="sinput" TabIndex="12">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 40px">Line 6</td>
                                        <td>
                                            <asp:TextBox ID="txtstate" runat="server" class="sinput" TabIndex="12">
                                            </asp:TextBox>
                                        </td>
                                    </tr>




                                    <%--  <tr>
                                                    <td>PAN No </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCPanno" runat="server" TabIndex="21"
                                                            class="sinput"></asp:TextBox>
                                                    </td>
                                                </tr>--%>


                                    <tr>

                                        <td>Description(if any)
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdescription" runat="server" TabIndex="17" TextMode="multiline" MaxLength="500"
                                                class="sinput"></asp:TextBox>

                                        </td>
                                    </tr>


                                    <tr style="display: none">
                                        <td>Our Contact Person
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlEmpId" TabIndex="19" class="sdrop">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr runat="server" visible="false">

                                        <td>Buyer's Order No&nbsp;</td>
                                        <td>
                                            <asp:TextBox ID="txtBuyerOrderNo" runat="server" TabIndex="20" class="sinput"></asp:TextBox>
                                            &nbsp;</td>
                                    </tr>


                                    <tr runat="server" visible="false">

                                        <td>PT State
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlPTState" TabIndex="19" class="sdrop">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr runat="server" visible="false">

                                        <td>Field Officer
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="dllfieldofficer" TabIndex="21" runat="server"
                                                class="sdrop">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Location&nbsp;</td>
                                        <td>
                                            <asp:TextBox ID="ddllocation" TabIndex="18" runat="server"
                                                class="sinput">
                                            </asp:TextBox>
                                            &nbsp;</td>
                                    </tr>

                                    <tr>
                                        <td>LWF State
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlLWFState" TabIndex="19" class="sdrop">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr runat="server" visible="false">
                                        <td>Area Wise Manager
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAreamanager" TabIndex="23" runat="server"
                                                class="sdrop">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                </table>

                                </td>

                                        <td valign="top" align="right">
                                            <table width="100%" cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td>Name<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtCname" TabIndex="2" class="sinput" MaxLength="200"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Segment 
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlsegment" runat="server" ValidationGroup="a" TabIndex="6" class="sdrop">
                                                        </asp:DropDownList>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Person Designation<%--<span style="color: Red">*</span>--%>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddldesgn" runat="server" ValidationGroup="a" class="sdrop" TabIndex="8">
                                                        </asp:DropDownList>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Landline No.</td>
                                                    <td class="style3">
                                                        <asp:TextBox ID="txtpin" runat="server" TabIndex="10" class="sinput" MaxLength="7"> </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                                            runat="server" Enabled="True" TargetControlID="txtpin"
                                                            ValidChars="0123456789">
                                                        </cc1:FilteredTextBoxExtender>

                                                    </td>
                                                </tr>
                                                <tr runat="server" visible="false">
                                                    <td>CC</td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmailCC" runat="server" TabIndex="10" class="sinput" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Fax No.</td>
                                                    <td>
                                                        <asp:TextBox ID="txtfaxno" runat="server" TabIndex="11" class="sinput" MaxLength="30"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                            runat="server" Enabled="True" TargetControlID="txtfaxno"
                                                            ValidChars="0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Area<span style="color: Red"></span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlArea" runat="server" ValidationGroup="a" class="sdrop" TabIndex="6" AutoPostBack="true" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Zone<span style="color: Red"></span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlZone" runat="server" ValidationGroup="a" class="sdrop" TabIndex="6">
                                                        </asp:DropDownList>

                                                    </td>
                                                </tr>



                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>

                                                <tr>
                                                    <td style="font-weight: bold; text-decoration: underline; padding-top: 26px">Ship To
                                                    </td>
                                                    <td></td>
                                                </tr>

                                                <tr>
                                                    <td>Legal Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtShipToLglName" runat="server" TabIndex="15" class="sinput" MaxLength="100"></asp:TextBox>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Addr 1
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtShipToAddr1" runat="server" TabIndex="15" class="sinput" MaxLength="100"></asp:TextBox>

                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Addr 2
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtShipToAddr2" runat="server" TabIndex="15" class="sinput" MaxLength="100"></asp:TextBox>

                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Location</td>
                                                    <td>
                                                        <asp:TextBox ID="txtShipToLocation" runat="server" TabIndex="20" class="sinput"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>PIN
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtShipToPIN" runat="server" TabIndex="21" class="sinput" MaxLength="6"></asp:TextBox>

                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>GSTIN/Unique ID </td>
                                                    <td>
                                                        <asp:TextBox ID="txtShipToGSTIN" runat="server" TabIndex="21"
                                                            class="sinput"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>State
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlShipToSate" TabIndex="19" class="sdrop" AutoPostBack="true" OnSelectedIndexChanged="ddlShipToSate_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>State Code</td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlShipToStateCode" TabIndex="21" class="sdrop">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>



                                                <tr>
                                                    <td style="padding-left: 40px">Line 1</td>
                                                    <td>
                                                        <asp:TextBox ID="txtShipToLine1" runat="server" class="sinput" TabIndex="13">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 40px">Line 2</td>
                                                    <td>
                                                        <asp:TextBox ID="txtShipToLine2" runat="server" class="sinput" TabIndex="13">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 40px">Line 3</td>
                                                    <td>
                                                        <asp:TextBox ID="txtShipToLine3" runat="server" class="sinput" TabIndex="13">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 40px">Line 4</td>
                                                    <td>
                                                        <asp:TextBox ID="txtShipToLine4" runat="server" class="sinput" TabIndex="13">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 40px">Line 5</td>
                                                    <td>
                                                        <asp:TextBox ID="txtShipToLine5" runat="server" class="sinput" TabIndex="13">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 40px">Line 6</td>
                                                    <td>
                                                        <asp:TextBox ID="txtShipToLine6" runat="server" class="sinput" TabIndex="13">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>



                                                <tr>
                                                    <td>
                                                        <asp:CheckBox runat="server" ID="chkSubUnit" Text=" Sub Unit" TabIndex="18"
                                                            AutoPostBack="True" OnCheckedChanged="chkSubUnit_CheckedChanged" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlUnits" TabIndex="21" class="sdrop" Visible="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Main Unit<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="radioyesmu" runat="server" Text="YES" GroupName="mainunit" TabIndex="22" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="radionomu" runat="server" Text="NO" GroupName="mainunit" TabIndex="21" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Invoice<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="radioinvoiceyes" runat="server" Text="YES" GroupName="invoice" TabIndex="23" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="radioinvoiceno" runat="server" Text="NO" GroupName="invoice" TabIndex="22" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>PaySheet<span style="color: Red">*</span>
                                                    </td>

                                                    <td>
                                                        <asp:RadioButton ID="radiopaysheetyes" runat="server" Text="YES" GroupName="paysheet" TabIndex="24" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="radiopaysheetno" runat="server" Text="NO" GroupName="paysheet" TabIndex="23" />
                                                    </td>
                                                </tr>





                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td style="padding-left: 50px; padding-top: 20px">
                                                        <asp:Button ID="btnaddclint" runat="server" Text="Save" ToolTip="Add Client" class=" btn save" TabIndex="25"
                                                            ValidationGroup="a1" OnClick="btnaddclint_Click"
                                                            OnClientClick='return confirm(" Are you sure you want to add the client details ?");' />
                                                        <asp:Button ID="btncancel" runat="server" Text="Cancel" ToolTip="Cancel Client"
                                                            OnClientClick='return confirm(" Are you sure you want to cancel this entry ?");'
                                                            class=" btn save" OnClick="btncancel_Click" TabIndex="25" /></td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>

                                            </table>
                                        </td>
                                </tr>
                                </table>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
</asp:Content>
