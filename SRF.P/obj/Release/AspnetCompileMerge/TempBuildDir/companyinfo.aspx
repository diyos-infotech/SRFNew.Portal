<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="companyinfo.aspx.cs" Inherits="SRF.P.companyinfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tdsize {
            height: 15px;
        }

        .style8 {
            width: 335px;
            height: 29px;
        }
    </style>


    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <h1 class="dashboard_heading">CompanyInfo Dashboard</h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">ADD COMPANY INFORMATION
                            </h2>
                        </div>
                        <asp:ScriptManager runat="server" ID="Scriptmanager1">
                        </asp:ScriptManager>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin" style="min-height: 530px;">
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table width="100%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td valign="top">
                                                <table width="100%" cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>Company Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcname" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="a1"
                                                                ControlToValidate="txtcname" ErrorMessage="please enter the company name">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Company Short Name<span style="color: Red">*</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcsname" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="style8">
                                                        <td>Address
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtaddress" runat="server" TextMode="MultiLine" class="sinput" Height="100px" Enabled="False"></asp:TextBox>
                                                            &nbsp;
                                                            <asp:RequiredFieldValidator ID="rfvaddress" runat="server" ControlToValidate="txtaddress"
                                                                Text="*" SetFocusOnError="true" EnableClientScript="true" ErrorMessage="Please Enter The Address"
                                                                ValidationGroup="a1"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Phone No</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPhoneno" runat="server" class="sinput" MaxLength="11" Enabled="False"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilterExtenderPhone" runat="server" FilterMode="ValidChars" FilterType="Numbers"
                                                                ValidChars="0123456789" TargetControlID="txtPhoneno">
                                                            </cc1:FilteredTextBoxExtender>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Fax No</td>
                                                        <td>
                                                            <asp:TextBox ID="txtFaxno" runat="server" class="sinput" MaxLength="11" Enabled="False"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilterExtenderFaxno" runat="server" FilterMode="ValidChars" FilterType="Custom"
                                                                ValidChars=".0123456789" TargetControlID="txtFaxno">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Email</td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmail" runat="server" class="sinput" Enabled="False"></asp:TextBox>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Website</td>
                                                        <td>
                                                            <asp:TextBox ID="txtWebsite" runat="server" class="sinput" Enabled="False"></asp:TextBox>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Service Tax No
                                                            <%--Bill notes replace with service tax no --%>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtbnotes" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>PAN No
                                                            <%--Labour rule  replace with PAN no --%>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtlabour" runat="server" MaxLength="80" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>PF No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtpfno" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>ESI No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtesino" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>P Tax No
                                                            <%--Labour rule  replace with PAN no --%>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBANK" runat="server" MaxLength="200" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Corporate Identity No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcorporateIDNo" runat="server" class="sinput"
                                                                Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Reg.No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtregno" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>



                                                </table>
                                            </td>
                                            <td align="right">
                                                <table width="100%" cellpadding="5" cellspacing="5" style="position: relative; bottom: 4px;">
                                                    <tr>
                                                        <td>Billsq<span style="color: Red">*</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtbillsq" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>



                                                    <tr>
                                                        <td>Notes
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" class="sinput" Height="100px" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td>Bill Description
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtbilldesc" runat="server" TextMode="MultiLine" class="sinput"
                                                                Height="35px" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Company Info
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcinfo" runat="server" TextMode="MultiLine" class="sinput" Height="35px" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>ESIC No for Forms
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtESICNoForms" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Branch Office
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBranchOffice" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>ISO CERFT NO
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtISOCertNo" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>PSARA ACT REG NO
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPsaraAct" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>KSSA MEMBERSHIP NO
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtKSSAMemberShipNo" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Bank Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBankname" runat="server" TextMode="MultiLine" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Bank A/c No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBankAccNo" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="auto-style1">IFSC
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtifsccode" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td>
                                                            PREPARE
                                                             
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPREPARE" runat="server" MaxLength="300" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    
                                                    <tr>
                                                        <td>
                                                            ACCOUNT NO
                                                            
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAccountno" runat="server" MaxLength="50" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Address Line One
                                                            
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtaddresslineone" runat="server" MaxLength="100" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Address Line Two
                                                            
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtaddresslinetwo" runat="server" MaxLength="100" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            IFSC CODE
                                                           
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtifsccode" runat="server" MaxLength="100" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            SASTC CODE
                                                            
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtsastcc" runat="server" MaxLength="100" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <%--<td>
                                                            Company Logo
                                                        </td>--%>
                                                        <td>
                                                            <%--<img id="imglogo" runat="server" height="100" width="100" alt="Ther IS No Image" />
                                                            <div style="margin-top: 10px">
                                                                <asp:Button ID="btnphoto" runat="server" Text="Select Photo" class="btn save" OnClick="btnphoto_Click"
                                                                    OnClientClick="beforeadd()" style="width:100px" />
                                                                <asp:FileUpload ID="fcpicture" runat="server" Visible="true" OnDataBinding="btnphoto_Click" />--%>
                                                            <asp:Label ID="lblresult" runat="server" Style="color: Red" Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="float: right;">
                                    <asp:Button ID="btnEdit" runat="server" Text="EDIT" ToolTip="Add Client" class=" btn save"
                                        ValidationGroup="a1" OnClick="btnEdit_Click" />
                                    <asp:Button ID="btnaddclint" runat="server" Text="SAVE" ToolTip="Add Client" class=" btn save" Enabled="false"
                                        ValidationGroup="a1" OnClick="btnaddclint_Click" OnClientClick='return confirm(" Are you sure you  want to add this record ?");' />
                                    <asp:Button ID="btncancel" runat="server" Text="CANCEL" ToolTip="Cancel Client" OnClientClick='return confirm(" Are you sure  you  want to cancel  this record?");'
                                        class=" btn save" OnClick="btncancel_Click" Enabled="false" />
                                </div>

                            </div>
                        </div>

                    </div>
                    <!-- DASHBOARD CONTENT END -->
                </div>
                <div class="clear">
                </div>
            </div>
        </div>

    </div>
    <!-- CONTENT AREA END -->

</asp:Content>
