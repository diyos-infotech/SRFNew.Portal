﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Clients/Clients.master" AutoEventWireup="true" CodeBehind="AddClient.aspx.cs" Inherits="SRF.P.Module_Clients.AddClient" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Calendar.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style3 {
            height: 24px;
        }
    </style>
   

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
                                    <h3 class="panel-title">Add Client</h3>
                                    <!-- <div align="right"> <b>Import Data: </b> <asp:FileUpload  ID="fileupload1" runat="server" Width="50px"/> 
                 <asp:Button ID="btnImportData" runat="server" ValidationGroup="b"
                     Text="Import"  
                        class=" btn save" onclick="btnImportData_Click"  /></div> -->

                                </td>
                                <td align="right"><< <a href="Clients.aspx" style="color: #003366">Back</a>  </td>
                            </tr>
                        </table>


                    </div>
                    <div class="panel-body">
                        <asp:ScriptManager runat="server" ID="Scriptmanager1">
                        </asp:ScriptManager>



                        <div class="dashboard_firsthalf" style="width: 100%">
                            <table width="100%" cellpadding="5" cellspacing="5">
                                <tr>
                                    <td valign="top">

                                        <table width="100%" cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td>Client ID
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtCId" class="sinput" ReadOnly="true" TabIndex="1" MaxLength="10"></asp:TextBox>
                                                </td>
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
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                        ControlToValidate="txtphonenumbers" SetFocusOnError="true" Display="Dynamic" ValidationGroup="a" Text="*"></asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
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
                                                <td colspan="2">Billing Details
                                                </td>

                                            </tr>


                                            <tr>
                                                <td>Line One
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtchno" runat="server" class="sinput" TabIndex="11" MaxLength="300">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Line Three
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtarea" runat="server" class="sinput" TabIndex="13" MaxLength="300"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Line Five
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtcity" runat="server" TabIndex="15" class="sinput" MaxLength="200"></asp:TextBox>

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
                                                <td>Location&nbsp;</td>
                                                <td>
                                                    <asp:TextBox ID="txtLocation" runat="server" TabIndex="20" class="sinput"></asp:TextBox>
                                                    &nbsp;</td>
                                            </tr>

                                             <tr>
                                                <td>LWF State
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlLWFState" TabIndex="19" class="sdrop" >
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
                                                    <asp:DropDownList ID="ddlsegment" runat="server" ValidationGroup="a" TabIndex="4" class="sdrop">
                                                    </asp:DropDownList>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Person Designation<span style="color: Red">*</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddldesgn" runat="server" ValidationGroup="a" class="sdrop" TabIndex="6">
                                                    </asp:DropDownList>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Landline No.</td>
                                                <td class="style3">
                                                    <asp:TextBox ID="txtpin" runat="server" TabIndex="8" class="sinput" MaxLength="7"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                                        runat="server" Enabled="True" TargetControlID="txtpin"
                                                        ValidChars="0123456789">
                                                    </cc1:FilteredTextBoxExtender>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Fax No.</td>
                                                <td>
                                                    <asp:TextBox ID="txtfaxno" runat="server" TabIndex="10" class="sinput" MaxLength="30"></asp:TextBox>
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
                                                    <asp:DropDownList ID="ddlArea" runat="server" ValidationGroup="a" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged" AutoPostBack="true" class="sdrop" TabIndex="6">
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
                                                <td>Line Two
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtstreet" runat="server" TabIndex="12" class="sinput" MaxLength="300"></asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Line Four
                                                </td>
                                                <td class="style3">
                                                    <asp:TextBox ID="txtcolony" runat="server" TabIndex="14" class="sinput" MaxLength="300"> </asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Line Six
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtstate" runat="server" Text="" TabIndex="16" class="sinput" MaxLength="200"> </asp:TextBox>

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
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td style="padding-left: 30px">
                                                    <asp:Button ID="btnaddclint" runat="server" Text="SAVE" ToolTip="Add Client" class=" btn save" TabIndex="25"
                                                        ValidationGroup="a1" OnClick="btnaddclint_Click"
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
        <div class="clear">
        </div>
    </div>
    <!-- DASHBOARD CONTENT END -->


</asp:Content>
