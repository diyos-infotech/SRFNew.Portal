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
                                            <asp:TextBox ID="txtshortname" class="sinput" TabIndex="3" runat="server" MaxLength="200"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Contact Person<span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtcontactperson" runat="server" TabIndex="5" class="sinput" MaxLength="200"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Phone No(s)<span style="color: Red">*</span>
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
                                            <asp:TextBox ID="txtemailid" runat="server" class="sinput" TabIndex="9" MaxLength="200"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Our GSTIN
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlOurGSTIN" runat="server" class="sdrop" TabIndex="9"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="font-weight: bold">
                                        <td colspan="2">Billing Details</td>

                                    </tr>


                                    <tr>
                                        <td>Line One
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtchno" runat="server" class="sinput" TabIndex="11" ValidationGroup="a1" MaxLength="300"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Line Three
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtarea" class="sinput" TabIndex="13" runat="server" MaxLength="300"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Line Five
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtCity" runat="server" Text="" class="sinput" TabIndex="15" MaxLength="200"> </asp:TextBox>
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
                                        <td>GSTIN/Unique ID </td>
                                        <td>
                                            <asp:TextBox ID="txtGSTUniqueID" runat="server" TabIndex="21"
                                                class="sinput"></asp:TextBox>
                                        </td>
                                    </tr>

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


                                    <tr>
                                        <td>Location</td>
                                        <td>
                                            <asp:TextBox ID="txtLocation" runat="server" TabIndex="20" class="sinput"></asp:TextBox>
                                        </td>
                                    </tr>


                                </table>
                                </td>
                                        
                                        
                                        <td valign="top">
                                            <table cellpadding="5" cellspacing="5" width="100%">
                                                <tr>
                                                    <td>Name<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <%--                <asp:DropDownList ID="ddlcname" runat="server" Width="125px" AutoPostBack="true"
                               OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged"></asp:DropDownList>--%>
                                                        <asp:TextBox ID="txtcname" runat="server" TabIndex="2" class="sinput" MaxLength="200"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>

                                                    <td>Segment<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlsegment" runat="server" TabIndex="4" class="sdrop">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Person Designation<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddldesgn" runat="server" class="sdrop" TabIndex="6">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Landline No</td>
                                                    <td>
                                                        <asp:TextBox ID="txtpin" runat="server" TabIndex="8" class="sinput" MaxLength="7"> </asp:TextBox>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Fax No</td>

                                                    <td>
                                                        <asp:TextBox ID="txtfaxno" runat="server" TabIndex="10" class="sinput" MaxLength="30"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
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
                                                    <td>Line Two
                                                    </td>

                                                    <td>

                                                        <asp:TextBox ID="txtstreet" runat="server" TabIndex="12" class="sinput" MaxLength="300"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Line Four
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtcolony" runat="server" TabIndex="14" class="sinput" MaxLength="300"> </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Line Six
                                                    </td>
                                                    <td>

                                                        <asp:TextBox ID="txtstate" runat="server" class="sinput" TabIndex="16" MaxLength="200"></asp:TextBox>
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
                                                    <td>
                                                        <asp:CheckBox runat="server" ID="chkSubUnit" Text="Is this a Sub Unit"
                                                            TabIndex="18" AutoPostBack="True" OnCheckedChanged="chkSubUnit_CheckedChanged" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlUnits" TabIndex="20" class="sdrop" Visible="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Main Unit <span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="radioyesmu" runat="server" Text="YES" GroupName="mainunit" TabIndex="21" />
                                                        <asp:RadioButton ID="radionomu" runat="server" Text="NO" GroupName="mainunit" TabIndex="21" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Invoice<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="radioinvoiceyes" runat="server" Text="YES" GroupName="invoice" TabIndex="22" />
                                                        <asp:RadioButton ID="radioinvoiceno" runat="server" Text="NO" GroupName="invoice" TabIndex="22" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>PaySheet<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="radiopaysheetyes" runat="server" Text="YES" GroupName="paysheet" TabIndex="23" />
                                                        <asp:RadioButton ID="radiopaysheetno" runat="server" Text="NO" GroupName="paysheet" TabIndex="23" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td style="padding-left: 30px">
                                                        <asp:Label ID="lblresult" runat="server" Visible="false" Style="text-align: left; color: Red"></asp:Label>

                                                        <asp:Button ID="btnaddclint" runat="server" Text="SAVE" ToolTip="Add Client" class=" btn save"
                                                            ValidationGroup="a1" OnClick="btnaddclint_Click" TabIndex="24"
                                                            OnClientClick='return confirm(" Are you sure you  want to modify the client details ?");' />
                                                        <asp:Button ID="btncancel" runat="server" Text="CANCEL" ToolTip="Cancel Client"
                                                            OnClientClick='return confirm(" Are you  sure you  want to cancel this entry ?");'
                                                            class=" btn save" OnClick="btncancel_Click" TabIndex="25" /></td>
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
        </div>
        <div class="clear">
        </div>
    </div>
</asp:Content>
