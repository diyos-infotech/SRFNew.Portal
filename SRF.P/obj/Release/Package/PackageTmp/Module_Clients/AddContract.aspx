<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Clients/Clients.master" AutoEventWireup="true" CodeBehind="AddContract.aspx.cs" Inherits="SRF.P.Module_Clients.AddContract" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Calendar.css" rel="stylesheet" type="text/css" />

    <script src="script/jquery.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="script/jscript.js">
    </script>

    <script type="text/javascript">

        function dtval(d, e) {
            var pK = e ? e.which : window.event.keyCode;
            if (pK == 8) { d.value = substr(0, d.value.length - 1); return; }
            var dt = d.value;
            var da = dt.split('/');
            for (var a = 0; a < da.length; a++) { if (da[a] != +da[a]) da[a] = da[a].substr(0, da[a].length - 1); }
            if (da[0] > 31) { da[1] = da[0].substr(da[0].length - 1, 1); da[0] = '0' + da[0].substr(0, da[0].length - 1); }
            if (da[1] > 12) { da[2] = da[1].substr(da[1].length - 1, 1); da[1] = '0' + da[1].substr(0, da[1].length - 1); }
            if (da[2] > 9999) da[1] = da[2].substr(0, da[2].length - 1);
            dt = da.join('/');
            if (dt.length == 2 || dt.length == 5) dt += '/';
            d.value = dt;
        }
        function ChangePaySheetsDDL(ele) {
            $("#ddlPaySheetDates").val($(ele).val());
        }
    </script>

    <div id="content-holder">
        <div class="content-holder">
            <div class="col-md-12" style="margin-top: 8px; margin-bottom: 8px">

                 <asp:ScriptManager runat="server" ID="Scriptmanager1">
                </asp:ScriptManager>

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
                                    <h3 class="panel-title">Add Contract</h3>
                                </td>
                                <td align="right"><< <a href="contracts.aspx" style="color: #003366">Back</a>  </td>
                            </tr>
                        </table>

                    </div>
                    <div class="panel-body">
                        <table width="100%" cellpadding="5" cellspacing="5" style="margin-top: 20px">
                            <tr>
                                <td width="53%">
                                    <table width="100%">
                                        <tr>
                                            <td width="190px">Client Name
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlcname" runat="server" TabIndex="1" AutoPostBack="true" class="sdrop"
                                                    OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:Button ID="Btn_Renewal" runat="server" OnClick="Btn_Renewal_Click" Text="Renewal" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Contract Ids<span style="color: Red">*</span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlContractids" runat="server" class="sdrop"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlContractids_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Start Date<span style="color: Red">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStartingDate" TabIndex="3" runat="server" class="sinput"
                                                    MaxLength="10" onkeyup="dtval(this,event)"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CEStartingDate" runat="server" Enabled="true" TargetControlID="txtStartingDate"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEStartingDate" runat="server" Enabled="True"
                                                    TargetControlID="txtStartingDate" ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>BG Amount
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtBGAmount" TabIndex="5" class="sinput"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                    TargetControlID="txtBGAmount" FilterMode="ValidChars" FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Validity Date
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtValidityDate" TabIndex="14" runat="server" class="sinput" MaxLength="10"
                                                    onkeyup="dtval(this,event)"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CEValidityDate" runat="server" Enabled="true" TargetControlID="txtValidityDate"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEValidityDate" runat="server" Enabled="True"
                                                    TargetControlID="txtValidityDate" ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Payment
                                            </td>
                                            <td>
                                                <asp:RadioButton runat="server" GroupName="p1" ID="RadioLumpsum" TabIndex="7" Text="Lumpsum" AutoPostBack="true"
                                                    OnCheckedChanged="RadioLumpsum_CheckedChanged1" />
                                                <asp:RadioButton runat="server" GroupName="p1" AutoPostBack="true" TabIndex="8" ID="RadioManPower"
                                                    Text="Man Power" Checked="true" OnCheckedChanged="RadioManPower_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbllampsum" runat="server" Text="Lampsum Amount" Visible="false"> 
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtlampsum" runat="server" Visible="false" TabIndex="20" class="sinput"> </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbllumpsumtext" runat="server" Text="Lampsum Text" Visible="false"> 
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLumpsumtext" runat="server" Visible="false" TabIndex="20"
                                                    TextMode="MultiLine" class="sinput"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Wages
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RadioCompany" runat="server" Text="Company" TabIndex="10" GroupName="Wages"
                                                    AutoPostBack="true" OnCheckedChanged="RadioSpecial_CheckedChanged" Style="display: none" />
                                                <asp:RadioButton ID="RadioClient" runat="server" Text="Client" TabIndex="11" GroupName="Wages"
                                                    AutoPostBack="true" Checked="true" OnCheckedChanged="RadioSpecial_CheckedChanged" />
                                                <asp:RadioButton ID="RadioSpecial" runat="server" Text="Special" TabIndex="12" GroupName="Wages"
                                                    AutoPostBack="True" OnCheckedChanged="RadioSpecial_CheckedChanged" />
                                                <asp:CheckBox ID="chkProfTax" runat="server" Text="Prof. Tax" TabIndex="13" Checked="true" />
                                                <asp:CheckBox ID="chkspt" runat="server" TabIndex="14" Text="SP PT" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtclonecontractid" runat="server" class="sinput" Visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table width="100%">
                                        <tr>
                                            <td>Client Id<span style="color: Red">*</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlclientid" ValidationGroup="a" TabIndex="2" runat="server" class="sdrop"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlclientid_OnSelectedIndexChanged">
                                                </asp:DropDownList>

                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlClientidNotincontract" runat="server" class="sdrop">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnClone" runat="server" Text="Clone" class="btn save" OnClick="btnClone_Click" Style="margin-left: 5px;" />
                                                    </td>
                                                </tr>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Ending Date<span style="color: Red">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEndingDate" TabIndex="4" runat="server" class="sinput" MaxLength="10"
                                                    onkeyup="dtval(this,event)"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CEEndingDate" runat="server" Enabled="true" TargetControlID="txtEndingDate"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEEndingDate" runat="server" Enabled="True" TargetControlID="txtEndingDate"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--Type Of Work--%>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtTypeOfWork" class="sinput" Style="display: none"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Billing Dates
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="Radio1to1" runat="server" Text="1st to 1st" GroupName="BillDates"
                                                    Checked="true" Visible="false" />
                                                <asp:RadioButton ID="RadioStartDate" runat="server" Text="Start Date to One Month"
                                                    GroupName="BillDates" Checked="false" Visible="false" />
                                                <asp:DropDownList ID="ddlbilldates" TabIndex="6" runat="server" class="sdrop" onchange="ChangePaySheetsDDL(this);">
                                                    <asp:ListItem>1st To 1st</asp:ListItem>
                                                    <asp:ListItem>Start Date To One Month</asp:ListItem>
                                                    <asp:ListItem>26 To 25</asp:ListItem>
                                                    <asp:ListItem>21 To 20</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>PaySheet Dates
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPaySheetDates" TabIndex="6" runat="server" class="sdrop">
                                                    <asp:ListItem>1st To 1st</asp:ListItem>
                                                    <asp:ListItem>Start Date To One Month</asp:ListItem>
                                                    <asp:ListItem>26 To 25</asp:ListItem>
                                                    <asp:ListItem>21 To 20</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Contract Id
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtcontractid" runat="server" ReadOnly="true" TabIndex="9" class="sinput"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <%--<tr>
                                                <td>Service Tax
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="RadioWithST" runat="server" Text="With" GroupName="serviceTax" Checked="true" />
                                                    <asp:RadioButton ID="RadioWithoutST" runat="server" Text="Without" GroupName="serviceTax"/>
                                                </td>
                                              </tr>--%>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table width="100%" cellpadding="5" cellspacing="5">
                            <tr>
                                <td valign="top" width="53%">
                                    <h3 style="border: none; background: none; text-decoration: underline; color: Red">Billing</h2>
                                                <table width="100%">
                                                    <tr>
                                                        <td class="style1">
                                                            <%-- Security Deposit--%>
                                                        </td>
                                                        <td class="style1">
                                                            <asp:TextBox runat="server" ID="txtSecurityDeposit" class="sinput" Style="display: none"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                TargetControlID="txtSecurityDeposit" FilterMode="ValidChars" FilterType="Numbers">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Material Cost For Month
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtMaterial" TabIndex="15" class="sinput"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                TargetControlID="txtMaterial" FilterMode="ValidChars" FilterType="Numbers">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Machinery Cost For Month
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtMachinary" TabIndex="18" class="sinput"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                TargetControlID="txtMachinary" FilterMode="ValidChars" FilterType="Numbers">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Service Charge
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="radioyes" runat="server" AutoPostBack="true" TabIndex="21" GroupName="a1"
                                                                Text="Yes" OnCheckedChanged="radioyes_CheckedChanged" />
                                                            <asp:RadioButton ID="radiono" runat="server" GroupName="a1" Text="No" AutoPostBack="true"
                                                                OnCheckedChanged="radioyes_CheckedChanged" TabIndex="22" Checked="true" />
                                                            <asp:RadioButton ID="RadioPercent" runat="server" Text="Percent(%)" Visible="false"
                                                                GroupName="service" Checked="true" />
                                                            <asp:RadioButton ID="RadioAmount" runat="server" Text="Amount  " GroupName="service"
                                                                Visible="false" />
                                                            <asp:TextBox ID="txtservicecharge" runat="server" Visible="false" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                TargetControlID="txtservicecharge" ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            &nbsp;&nbsp;
                                                            <asp:CheckBox ID="chkStaxonservicecharge" runat="server" Visible="false" Checked="false" Text=" Service Tax" TextAlign="Right" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Contract Description
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtContractDescription" runat="server" TabIndex="25" TextMode="MultiLine" MaxLength="200"
                                                                class="sinput"> </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none">
                                                        <td>No Of Days for Billing :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlnoofdays" runat="server">
                                                                <asp:ListItem>Gen</asp:ListItem>
                                                                <asp:ListItem>22</asp:ListItem>
                                                                <asp:ListItem>23</asp:ListItem>
                                                                <asp:ListItem>24</asp:ListItem>
                                                                <asp:ListItem>25</asp:ListItem>
                                                                <asp:ListItem>26</asp:ListItem>
                                                                <asp:ListItem>27</asp:ListItem>
                                                                <asp:ListItem>30</asp:ListItem>
                                                                <asp:ListItem>31</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none">
                                                        <td>No of Days for Wages :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlNoOfDaysWages" runat="server">
                                                                <asp:ListItem>Gen</asp:ListItem>
                                                                <asp:ListItem>22</asp:ListItem>
                                                                <asp:ListItem>23</asp:ListItem>
                                                                <asp:ListItem>24</asp:ListItem>
                                                                <asp:ListItem>25</asp:ListItem>
                                                                <asp:ListItem>26</asp:ListItem>
                                                                <asp:ListItem>27</asp:ListItem>
                                                                <asp:ListItem>30</asp:ListItem>
                                                                <asp:ListItem>31</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">Service Tax
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="RadioWithST" runat="server" TabIndex="27" Text="With" GroupName="serviceTax"
                                                                Checked="true" />
                                                            <asp:RadioButton ID="RadioWithoutST" runat="server" TabIndex="28" Text="Without   " GroupName="serviceTax" />
                                                            <asp:CheckBox ID="CheckIncludeST" Text="  ST Exemption" TabIndex="29" Checked="false" runat="server" /><br />
                                                            <br />
                                                            <asp:CheckBox ID="Check75ST" Text="&nbsp;&nbsp;75% of ST by client" TabIndex="30" Checked="false" runat="server" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td valign="top">GST
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkCGST" Text="&nbsp;CGST" TabIndex="30" Checked="true" runat="server" />
                                                            <asp:RadioButton ID="RdbSGST" runat="server" TabIndex="27" Text="SGST" GroupName="GST"
                                                                Checked="true" OnCheckedChanged="RdbSGST_CheckedChanged" AutoPostBack="true" />
                                                            <asp:RadioButton ID="RdbIGST" runat="server" TabIndex="28" Text="IGST" GroupName="GST" OnCheckedChanged="RdbSGST_CheckedChanged" AutoPostBack="true" />

                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td>GST</td>
                                                        <td>
                                                            <asp:CheckBox ID="chkGSTLineItem" Text="&nbsp;Line Item" TabIndex="30" runat="server" />

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top"></td>
                                                        <td>
                                                            <asp:CheckBox ID="chkCess1" Text="&nbsp;CESS1" runat="server" Checked="false" Visible="false" />
                                                            <asp:CheckBox ID="chkCess2" Text="&nbsp;CESS2" runat="server" Checked="false" Visible="false" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblreslt" runat="server" Text="" Style="color: Red; font-weight: normal;"
                                                                Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <h3 style="border: none; background: none;">Invoice Description</h3>

                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtdescription" runat="server" MaxLength="200" TabIndex="35" Width="170px" Height="110px"
                                                                Text="We are presenting our bill for the Security Services provided at your establishment. Kindly release the payment at the earliest."
                                                                Style="font-variant: normal; padding: 10px" TextMode="MultiLine" class="sinput"></asp:TextBox>
                                                        </td>


                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkrc" runat="server" TabIndex="40" Text="&nbsp;&nbsp;1/6 Reliever Charges&nbsp;&nbsp;" />
                                                        </td>

                                                        <td>
                                                            <asp:CheckBox ID="Chkpdfs" runat="server" Text="  PDFs" Checked="true" />
                                                        </td>

                                                        <td>
                                                            <asp:CheckBox ID="ChkRoundOff" runat="server" Text=" Without Round Off" />
                                                        </td>
                                                    </tr>

                                                </table>
                                </td>
                                <td valign="top">
                                    <h3 style="border: none; background: none; text-decoration: underline; color: Red">Paysheet</h3>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <%--EMD Value--%>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtEMDValue" class="sinput" Style="display: none"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                    TargetControlID="txtEMDValue" FilterMode="ValidChars" FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--Wage According To Which Act--%>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtWAWA" class="sinput" Style="display: none"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--  Performance Guarentee--%>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPerGurante" class="sinput" Style="display: none"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                    TargetControlID="txtPerGurante" FilterMode="ValidChars" FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">PF
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="TxtPf" runat="server" Text="100" TabIndex="16" class="sinput" MaxLength="5" Style="width: 30px"> 
                                       
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>%
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="DdlPf" runat="server" class="sdrop" TabIndex="17" Style="width: 140px">
                                                                <asp:ListItem>Basic</asp:ListItem>
                                                                <asp:ListItem>Basic+DA</asp:ListItem>
                                                                <asp:ListItem>Basic+DA+WA</asp:ListItem>
                                                                <asp:ListItem>Basic+DA+OA</asp:ListItem>
                                                                <asp:ListItem>Basic+DA+EL</asp:ListItem>
                                                                <asp:ListItem>Basic+DA+WA+EL+OA</asp:ListItem>
                                                                <asp:ListItem>Basic+DA+EL+OA</asp:ListItem>
                                                                <asp:ListItem>Basic+DA+WA+OA</asp:ListItem>
                                                                <asp:ListItem>Basic+DA+WA+Conv</asp:ListItem>
                                                                <asp:ListItem>Basic+DA+Conv</asp:ListItem>
                                                                <asp:ListItem>Basic+DA+WA+EL+NFHs+OA</asp:ListItem>
                                                                <asp:ListItem>Basic+DA+NFH</asp:ListItem>
                                                                <asp:ListItem>BASIC + DA + NFH +EL</asp:ListItem>
                                                                <asp:ListItem>BASIC+DA+EL+NFH+BONUS</asp:ListItem>
                                                                <asp:ListItem>BASIC+DA+EL+NFH+BONUS+WA</asp:ListItem>
                                                                <asp:ListItem>BASIC+DA+WA+OA+NFH+EL+RC</asp:ListItem>
                                                                <asp:ListItem>BASIC+DA+WA+EL</asp:ListItem>
                                                                <asp:ListItem>BASIC+DA+WA+OA+BONUS+EL+NFH</asp:ListItem>
                                                                <asp:ListItem>BASIC+DA+OA+BONUS+EL+NFH</asp:ListItem>

                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <td>PF On :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlpfon" runat="server" TabIndex="19" class="sdrop" Style="width: 165px">
                                                                <asp:ListItem>None</asp:ListItem>
                                                                <asp:ListItem>OTs</asp:ListItem>
                                                                <asp:ListItem>WOs</asp:ListItem>
                                                                <asp:ListItem>NHs</asp:ListItem>
                                                                <asp:ListItem>NPOTs</asp:ListItem>
                                                                <asp:ListItem>OTs+WOs</asp:ListItem>
                                                                <asp:ListItem>OTs+NHs</asp:ListItem>
                                                                <asp:ListItem>NHs+NPOTs</asp:ListItem>
                                                                <asp:ListItem>OTs+WOs+NHs</asp:ListItem>
                                                                <asp:ListItem>OTs+NHs+NPOTs</asp:ListItem>
                                                                <asp:ListItem>WOs+NHs+NPOTs</asp:ListItem>
                                                                <asp:ListItem>OTs+WOs+NHs+NPOTs</asp:ListItem>
                                                                <asp:ListItem>OTs(Basic+DA)</asp:ListItem>

                                                            </asp:DropDownList>
                                                            <asp:CheckBox ID="checkPFonOT" Text="  PF on OTs" Checked="false" runat="server"
                                                                Visible="false" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">ESI
                                            </td>
                                            <td valign="top">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="TxtEsi" runat="server" Text="100" TabIndex="23" MaxLength="5" class="sinput" Style="width: 30px"> </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <asp:DropDownList ID="DdlEsi" runat="server" class="sdrop" TabIndex="24" Style="width: 140px">
                                                                <asp:ListItem>Gross-WA</asp:ListItem>
                                                                <asp:ListItem>Gross</asp:ListItem>
                                                                <asp:ListItem>Basic</asp:ListItem>
                                                                <asp:ListItem>Gross+Inc</asp:ListItem>
                                                                <asp:ListItem>Gross+WA+Bonus+LA+Grat</asp:ListItem>
                                                                <asp:ListItem>Gross+WA+Inc</asp:ListItem>
                                                                <asp:ListItem>Gross-WA+Bonus</asp:ListItem>
                                                                <asp:ListItem>Basic+DA</asp:ListItem>
                                                                <asp:ListItem>GROSS-WA-EL-NFH</asp:ListItem>
                                                                <asp:ListItem>GROSS-WA-EL</asp:ListItem>
                                                                <asp:ListItem>GROSS-WA-EL-NFH-BONUS</asp:ListItem>
                                                                <asp:ListItem>GROSS-WA-NFH</asp:ListItem>
                                                                <asp:ListItem>GROSS-WA-BONUS</asp:ListItem>
                                                                <asp:ListItem> GROSS-WA-NFH-BONUS</asp:ListItem>
                                                                <asp:ListItem>GROSS-WA-EL-BONUS</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <td>ESI On :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlesion" runat="server" TabIndex="26" class="sdrop" Style="width: 165px">
                                                                <asp:ListItem>None</asp:ListItem>
                                                                <asp:ListItem>OTs</asp:ListItem>
                                                                <asp:ListItem>WOs</asp:ListItem>
                                                                <asp:ListItem>NHs</asp:ListItem>
                                                                <asp:ListItem>NPOTs</asp:ListItem>
                                                                <asp:ListItem>OTs+WOs</asp:ListItem>
                                                                <asp:ListItem>OTs+NHs</asp:ListItem>
                                                                <asp:ListItem>NHs+NPOTs</asp:ListItem>
                                                                <asp:ListItem>OTs+WOs+NHs</asp:ListItem>
                                                                <asp:ListItem>OTs+NHs+NPOTs</asp:ListItem>
                                                                <asp:ListItem>WOs+NHs+NPOTs</asp:ListItem>
                                                                <asp:ListItem>OTs+WOs+NHs+NPOTs</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:CheckBox ID="checkESIonOT" Text="  ESI on OTs" Checked="false" Visible="false"
                                                                runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>PF Limit :
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtPfLimit" runat="server" TabIndex="31" class="sinput" Style="width: 30px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredPfLimit" runat="server" Enabled="True" TargetControlID="txtPfLimit"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Chkpf" runat="server" TabIndex="32" Text="&nbsp;&nbsp;PF" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Esi Limit :
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtEsiLimit" runat="server" TabIndex="33" class="sinput" Style="width: 30px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredEsiLimit" runat="server" Enabled="True"
                                                                TargetControlID="txtEsiLimit" ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="ChkEsi" runat="server" TabIndex="34" Text="&nbsp;&nbsp;ESI" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>OT
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DdlOt" runat="server" class="sdrop" TabIndex="36" Style="margin-left: 14px; width: 80px">
                                                    <asp:ListItem>100%</asp:ListItem>
                                                    <asp:ListItem>200%</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>OA
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlOAPer" runat="server" class="sdrop" TabIndex="36" Style="margin-left: 14px; width: 80px">
                                                    <asp:ListItem>100%</asp:ListItem>
                                                    <asp:ListItem>200%</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>OT Amount
                                            </td>
                                            <td height="30px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="radiootregular" TabIndex="37" runat="server"
                                                Text="Regular" GroupName="otregular" Checked="true" />
                                                <asp:RadioButton ID="radiootspecial" runat="server" TabIndex="38" Text="Special " GroupName="otregular" />
                                                <asp:RadioButton ID="Radiootspecialone" runat="server" TabIndex="39" Text="Special One" Visible="false"
                                                    GroupName="otregular" />
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td>
                                                <asp:CheckBox ID="chkotsalaryrate" runat="server" Text="OT Salary Rate" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtotsalaryrate" runat="server" class="sinput"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTBEotsalaryrate" runat="server" Enabled="True"
                                                    TargetControlID="txtotsalaryrate" ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkojt" runat="server" TabIndex="40" Text="&nbsp;&nbsp;OJT&nbsp;&nbsp;" />
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>

                                                        <td>
                                                            <asp:CheckBox ID="chktl" runat="server" TabIndex="41" Text="&nbsp;&nbsp;PL&nbsp;&nbsp;" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txttlamt" runat="server" TabIndex="42" class="sinput" Style="width: 30px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" Text="OWF " Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtOWF" Text="0" runat="server" TabIndex="43" class="sinput" Visible="false" Style="margin-left: 14px; width: 30px"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FTBEOWF" runat="server" Enabled="True" TargetControlID="txtOWF"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        &nbsp;&nbsp;
                                                        <td>
                                                            <asp:Label ID="lblTds" runat="server" Text="Tds "></asp:Label>
                                                        </td>

                                                        <td>
                                                            <asp:TextBox ID="txtTds" Text="0" runat="server" TabIndex="44" class="sinput" Style="margin-left: 14px; width: 30px"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxTds" runat="server" Enabled="True" TargetControlID="txtTds"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblpono" runat="server" Text="PO NO"></asp:Label>
                                                &nbsp;</td>
                                            <td>
                                                <asp:TextBox ID="txtPono" Text="" runat="server" TabIndex="45" class="sinput" Style="margin-left: 14px; width: 60px"> </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderpono" runat="server" Enabled="True" TargetControlID="txtPono"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                                &nbsp;&nbsp;&nbsp;
                                                            <asp:Label ID="lblExpecteddate" runat="server" Text="Expected date of Receipt"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:TextBox ID="txtExpectdateofreceipt" Text="" runat="server" TabIndex="46" class="sinput" Style="margin-left: 14px; width: 60px"> </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderExpectdate" runat="server" Enabled="True" TargetControlID="txtExpectdateofreceipt"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEsibranch" runat="server" Text="ESI Branch"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlEsibranch" runat="server" class="sdrop"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td colspan="2">
                                                <asp:CheckBox ID="chkNoNhsWoDed" runat="server" Text=" NO WOs NHs Calculation" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkGeneralDed" runat="server" Text=" No General Ded" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkUniformDed" runat="server" Text=" No Uniform Ded" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkSecDepDed" runat="server" Text=" No Sec Dep Ded" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkOtherDed" runat="server" Text=" No Other Ded" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Payment Date</td>
                                            <td>
                                                <asp:DropDownList ID="ddlpaymentdates" runat="server">
                                                    <asp:ListItem>5</asp:ListItem>
                                                    <asp:ListItem>7</asp:ListItem>
                                                    <asp:ListItem>9</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="ChkPFSpl" runat="server" Text=" PF @12%" />
                                            </td>

                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:UpdatePanel runat="server" ID="Contractdetails">
                        <ContentTemplate>
                            <div style="font-weight: bold; text-align: left; font-size: 13px; min-height: 20px; margin-left: 2px; height: auto">
                                <table cellpadding="5" cellspacing="5">


                                    <tr>
                                        <td>Human Resource Needs
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--class="dashboard_full"--%>
                            <div style="font-family: Arial; font-weight: normal; font-variant: normal; min-height: 100px; height: auto; font-size: 13px; overflow: auto"
                                class="rounded_corners">
                                <%--; overflow: scroll--%>
                                <asp:GridView ID="gvdesignation" runat="server" Width="99%" Height="50%" Style="margin-left: 5px"
                                    AutoGenerateColumns="False" CellPadding="5" CellSpacing="3" ForeColor="#333333" GridLines="None">
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle Height="3px" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <FooterStyle Height="3px" />
                                    <RowStyle Height="1px" BackColor="#EFF3FB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="DdlDesign" runat="server" Width="180px">
                                                    <%--DataValueField="<%# Bind('design') %>"--%>
                                                    <asp:ListItem Selected="True" Value="0">--Select Designation-- </asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle Height="3px" />
                                            <ItemStyle Width="10px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Duty Hrs" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle Height="3px" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtdutyhrs" runat="server" Width="100px" Style="text-align: center"></asp:TextBox>
                                                <%-- <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender100" runat="server" Enabled="True"
                                                        TargetControlID="txtdutyhrs" FilterMode="ValidChars" FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="3px" Height="3px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle Height="3px" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtquantity" runat="server" Width="30px" Style="text-align: center"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                    TargetControlID="txtquantity" FilterMode="ValidChars" FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                            <ItemStyle Width="3px" Height="3px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P.R">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPayRate" runat="server" Width="40px" Style="text-align: center"> </asp:TextBox>
                                                <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server"
                                                        Enabled="True" TargetControlID="txtPayRate" FilterMode="ValidChars" FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>--%>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                    TargetControlID="txtPayRate" ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="D.T">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddldutytype" runat="server">
                                                    <asp:ListItem Value="P.M" Selected="True">P.M</asp:ListItem>
                                                    <asp:ListItem Value="P.D">P.D</asp:ListItem>
                                                    <asp:ListItem Value="P.Hr">P.Hr</asp:ListItem>
                                                    <asp:ListItem Value="P.Sft">P.Sft</asp:ListItem>
                                                    <asp:ListItem Value="Fixed">Fixed</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="No of Days">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlNoOfDaysBilling" runat="server">
                                                    <asp:ListItem>Gen</asp:ListItem>
                                                    <asp:ListItem>G-S</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                    <asp:ListItem>24</asp:ListItem>
                                                    <asp:ListItem>25</asp:ListItem>
                                                    <asp:ListItem>26</asp:ListItem>
                                                    <asp:ListItem>27</asp:ListItem>
                                                    <asp:ListItem>28</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>31</asp:ListItem>
                                                    <asp:ListItem>30.45</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Summary" Visible="true">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtsummary" runat="server" Text="" TextMode="MultiLine" Height="20px"
                                                    Width="100px"></asp:TextBox><%-- Text="Summary"--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Nots">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlNoOfOtsPaysheet" runat="server">
                                                    <asp:ListItem>Gen</asp:ListItem>
                                                    <asp:ListItem>G-S</asp:ListItem>
                                                    <asp:ListItem>G-4</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                    <asp:ListItem>24</asp:ListItem>
                                                    <asp:ListItem>25</asp:ListItem>
                                                    <asp:ListItem>26</asp:ListItem>
                                                    <asp:ListItem>27</asp:ListItem>
                                                    <asp:ListItem>28</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>31</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>







                                        <asp:TemplateField HeaderText="BASIC" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtBasic" runat="server" Width="35px" Style="text-align: center"> </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                    TargetControlID="TxtBasic" ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DA">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtda" runat="server" Width="35px" Style="text-align: center"> </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                    TargetControlID="txtda" ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="HRA">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txthra" runat="server" Width="35px" Style="text-align: center"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="F1" runat="server" Enabled="True" TargetControlID="txthra"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Conv">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtConveyance" runat="server" Width="35px" Style="text-align: center"> </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="F2" runat="server" Enabled="True" TargetControlID="txtConveyance"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CCA">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtcca" runat="server" Width="35px" Style="text-align: center"> </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="F3" runat="server" Enabled="True" TargetControlID="txtcca"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="L A">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtleaveamount" runat="server" Width="35px" Style="text-align: center"> </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="F4" runat="server" Enabled="True" TargetControlID="txtleaveamount"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="LA Type">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlLAtype" runat="server">
                                                    <asp:ListItem>Claim</asp:ListItem>
                                                    <asp:ListItem>Pay</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Gratuity">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtgratuty" runat="server" Width="45px" Style="text-align: center"> </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="F5" runat="server" Enabled="True" TargetControlID="txtgratuty"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bonus">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtbonus" runat="server" Width="35px" Style="text-align: center">  </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="F6" runat="server" Enabled="True" TargetControlID="txtbonus"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Bonus Type">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlbonustype" runat="server">
                                                    <asp:ListItem>Claim</asp:ListItem>
                                                    <asp:ListItem>Pay</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Att Bonus">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtattbonus" runat="server" Width="35px" Style="text-align: center">  </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTBAttBonus" runat="server" Enabled="True" TargetControlID="txtattbonus"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Uniform">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtuniform" runat="server" Width="48px" Style="text-align: center">  </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTBUniform" runat="server" Enabled="True" TargetControlID="txtuniform"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="W A">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtwa" runat="server" Width="35px" Style="text-align: center"> </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="F7" runat="server" Enabled="True" TargetControlID="txtwa"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="O A">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtoa" runat="server" Width="35px" Style="text-align: center"> </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="F8" runat="server" Enabled="True" TargetControlID="txtoa"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NFHs">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNfhs" runat="server" Width="35px" Style="text-align: center"> </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Fnfhs" runat="server" Enabled="True" TargetControlID="txtNfhs"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="R.C">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txtrc" runat="server" Width="35px" Style="text-align: center"> </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Filterrc" runat="server" Enabled="True" TargetControlID="Txtrc"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Srv.Chrgs">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtCs" runat="server" Width="60px" Style="text-align: center"> </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Filtercs" runat="server" Enabled="True" TargetControlID="TxtCs"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Srv.Chrgs %">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtScPer" runat="server" Width="60px" Style="text-align: center"> </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilterSCPer" runat="server" Enabled="True" TargetControlID="TxtScPer"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="OT Rate">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtOTRate" runat="server" Width="35px" Style="text-align: center"> </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Filterotrate" runat="server" Enabled="True" TargetControlID="TxtOTRate"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>





                                    </Columns>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </div>
                            <div>
                                <br />
                                <div style="margin-right: 10px; float: right">
                                    <asp:Button ID="btnadddesgn" runat="server" class="btn save" Text="Add Designation"
                                        OnClick="btnadddesgn_Click1" Style="width: 125px" /><br />
                                    <asp:Label ID="lblmsgcontractdetails" runat="Server" Text="" Style="color: Red;"></asp:Label>
                                </div>
                                <br />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel runat="server" ID="ContractSpecialWages">
                        <ContentTemplate>
                            <div class="rounded_corners" style="overflow: auto">
                                <asp:Panel ID="SpecialWagesPanel" runat="server" Visible="false">
                                    <div style="font-weight: bold; text-align: left; font-size: 13px; min-height: 20px; margin-left: 2px; height: auto">
                                        <table cellpadding="5" cellspacing="5" border="0">


                                            <tr>
                                                <td style="border: none">Special Wages
                                                </td>
                                                <td style="border: none">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                    <asp:GridView ID="gvSWDesignations" runat="server" Width="98%" Height="50%" Style="margin-left: 5px"
                                        AutoGenerateColumns="False" CellPadding="5" CellSpacing="3" ForeColor="#333333" GridLines="None">
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle Height="3px" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <FooterStyle Height="3px" />
                                        <RowStyle Height="1px" BackColor="#EFF3FB" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Designation">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DdlDesign" runat="server" Width="180px">
                                                        <%--DataValueField="<%# Bind('design') %>"--%>
                                                        <asp:ListItem Selected="True" Value="0">--Select Designation-- </asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Height="3px" />
                                                <ItemStyle Width="10px"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No of Days">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlNoOfDaysWages" runat="server">
                                                        <asp:ListItem>Gen</asp:ListItem>
                                                        <asp:ListItem>G-S</asp:ListItem>
                                                        <asp:ListItem>G-4</asp:ListItem>
                                                        <asp:ListItem>P.Hr</asp:ListItem>
                                                        <asp:ListItem>P.Day</asp:ListItem>
                                                        <asp:ListItem>PM/PD(8Hrs)</asp:ListItem>
                                                        <asp:ListItem>22</asp:ListItem>
                                                        <asp:ListItem>23</asp:ListItem>
                                                        <asp:ListItem>24</asp:ListItem>
                                                        <asp:ListItem>25</asp:ListItem>
                                                        <asp:ListItem>26</asp:ListItem>
                                                        <asp:ListItem>27</asp:ListItem>
                                                        <asp:ListItem>28</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>31</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="BASIC">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtBasic" runat="server" Width="45px" Style="text-align: center"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                        TargetControlID="TxtBasic" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle Width="5px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DA">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtda" runat="server" Width="45px" Style="text-align: center"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                        TargetControlID="txtda" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle Width="5px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="HRA">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txthra" runat="server" Width="45px" Style="text-align: center"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="F1" runat="server" Enabled="True" TargetControlID="txthra"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle Width="5px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Conv">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtConveyance" runat="server" Width="45px" Style="text-align: center"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="F2" runat="server" Enabled="True" TargetControlID="txtConveyance"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle Width="9px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CCA">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtcca" runat="server" Width="45px" Style="text-align: center"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="F3" runat="server" Enabled="True" TargetControlID="txtcca"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle Width="9px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="L A">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtleaveamount" runat="server" Width="45px" Style="text-align: center"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="F4" runat="server" Enabled="True" TargetControlID="txtleaveamount"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle Width="9px" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="LA Type">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlLAtype" runat="server">
                                                        <asp:ListItem>Monthly</asp:ListItem>
                                                        <asp:ListItem>Yearly</asp:ListItem>
                                                        <asp:ListItem>Half Yearly</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Gratuity">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtgratuty" runat="server" Width="45px" Style="text-align: center"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="F5" runat="server" Enabled="True" TargetControlID="txtgratuty"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle Width="9px" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Gratuity Type">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlGratuitytype" runat="server">
                                                        <asp:ListItem>Monthly</asp:ListItem>
                                                        <asp:ListItem>Yearly</asp:ListItem>
                                                        <asp:ListItem>Half Yearly</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Bonus">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtbonus" runat="server" Width="45px" Style="text-align: center">  </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="F6" runat="server" Enabled="True" TargetControlID="txtbonus"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle Width="9px" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Bonus Type">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlbonustype" runat="server">
                                                        <asp:ListItem>Monthly</asp:ListItem>
                                                        <asp:ListItem>Yearly</asp:ListItem>
                                                        <asp:ListItem>Half Yearly</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Att Bonus">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtattbonus" runat="server" Width="35px" Style="text-align: center">  </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBAttBonus" runat="server" Enabled="True" TargetControlID="txtattbonus"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="W A">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtwa" runat="server" Width="45px" Style="text-align: center"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="F7" runat="server" Enabled="True" TargetControlID="txtwa"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle Width="9px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="O A">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtoa" runat="server" Width="45px" Style="text-align: center"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="F8" runat="server" Enabled="True" TargetControlID="txtoa"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle Width="15px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NFHs">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNfhs1" runat="server" Width="45px" Style="text-align: center"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="Fnhs" runat="server" Enabled="True" TargetControlID="txtNfhs1"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle Width="15px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="R.C">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txtrc" runat="server" Width="45px" Style="text-align: center"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="Filterrc" runat="server" Enabled="True" TargetControlID="Txtrc"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Srv.Chrgs">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtCs" runat="server" Width="60px" Style="text-align: center"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="Filtercs" runat="server" Enabled="True" TargetControlID="TxtCs"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Srv.Chrgs %">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtScPer" runat="server" Width="60px" Style="text-align: center"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilterSCPer" runat="server" Enabled="True" TargetControlID="TxtScPer"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Nots">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlNoOfOtsPaysheet" runat="server">
                                                        <asp:ListItem>Gen</asp:ListItem>
                                                        <asp:ListItem>G-S</asp:ListItem>
                                                        <asp:ListItem>G-4</asp:ListItem>
                                                        <asp:ListItem>P.Hr</asp:ListItem>
                                                        <asp:ListItem>P.Day</asp:ListItem>
                                                        <asp:ListItem>PM/PD(8Hrs)</asp:ListItem>
                                                        <asp:ListItem>22</asp:ListItem>
                                                        <asp:ListItem>23</asp:ListItem>
                                                        <asp:ListItem>24</asp:ListItem>
                                                        <asp:ListItem>25</asp:ListItem>
                                                        <asp:ListItem>26</asp:ListItem>
                                                        <asp:ListItem>27</asp:ListItem>
                                                        <asp:ListItem>28</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>31</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="OT Rate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtOTRate" runat="server" Width="35px" Style="text-align: center"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="Filterotrate" runat="server" Enabled="True" TargetControlID="TxtOTRate"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>



                                            <asp:TemplateField HeaderText="NNhs">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlNoOfNhsPaysheet" runat="server">
                                                        <asp:ListItem>Gen</asp:ListItem>
                                                        <asp:ListItem>G-S</asp:ListItem>
                                                        <asp:ListItem>G-4</asp:ListItem>
                                                        <asp:ListItem>P.Hr</asp:ListItem>
                                                        <asp:ListItem>P.Day</asp:ListItem>
                                                        <asp:ListItem>PM/PD(8Hrs)</asp:ListItem>
                                                        <asp:ListItem>22</asp:ListItem>
                                                        <asp:ListItem>23</asp:ListItem>
                                                        <asp:ListItem>24</asp:ListItem>
                                                        <asp:ListItem>25</asp:ListItem>
                                                        <asp:ListItem>26</asp:ListItem>
                                                        <asp:ListItem>27</asp:ListItem>
                                                        <asp:ListItem>28</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>31</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="NHS Rate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtNhsRate" runat="server" Width="35px" Style="text-align: center"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilterNHSrate" runat="server" Enabled="True" TargetControlID="TxtNhsRate"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <EditRowStyle BackColor="#2461BF" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>

                                </asp:Panel>
                            </div>
                            <br />


                            <div>
                                <asp:Button ID="btnadddesgnsw" runat="server" TabIndex="44" class="btn save" Text="Add Designation"
                                    OnClick="btnadddesgnsw_Click" Visible="False" Style="width: 125px" />
                                <asp:Label ID="lblmsgspecialwages" runat="Server" Text="" Style="color: Red; margin-left: 50%"></asp:Label>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />

                    <div style="margin-left: 810px; margin-top: 6px">
                        <asp:Button ID="Btn_Save_Contracts" runat="server" TabIndex="45" Text="Save" ValidationGroup="a"
                            class="btn save" Style="margin-bottom: 6px" OnClientClick='return confirm("Are you sure you want to add the  contract details ?"); '
                            OnClick="Btn_Save_Contracts_Click" />
                        <asp:Button ID="btncancel" runat="server" Text="Cancel" TabIndex="46" class="btn save" Style="margin-bottom: 6px"
                            OnClientClick='return confirm("Are you sure you want to cancel this entry?");' />
                    </div>

                </div>
                <div class="clear">
                </div>
                <!-- DASHBOARD CONTENT END -->
            </div>
        </div>

    </div>
    <!-- CONTENT AREA END -->

</asp:Content>
