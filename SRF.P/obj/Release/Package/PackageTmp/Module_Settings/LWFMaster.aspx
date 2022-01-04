<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="LWFMaster.aspx.cs" Inherits="SRF.P.LWFMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <link href="../css/boostrap/css/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.12.0/themes/base/jquery-ui.css" />
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script type="text/jscript" src="https://code.jquery.com/ui/1.12.0/jquery-ui.js"></script>
    <style>
        .custom-combobox {
            position: relative;
            display: inline-block;
        }

        .custom-combobox-toggle {
            position: absolute;
            top: 0;
            bottom: 0;
            margin-left: -1px;
            padding: 0;
        }

        .custom-combobox-input {
            margin: 0;
            padding: 5px 10px;
        }
    </style>
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

    </script>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">LWF Master</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">LWF Master
                            </h2>
                        </div>
                        <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                        </asp:ScriptManager>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <table width="100%" cellpadding="5" cellspacing="5">
                                    <tr style="height:40px">
                                        <td>State<span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlStates" runat="server" class="sdrop" AutoPostBack="true" OnSelectedIndexChanged="DdlStates_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                        <td>Deduct Type<span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDeductType" runat="server" class="sdrop" OnSelectedIndexChanged="ddlDeductType_SelectedIndexChanged1" AutoPostBack="true">
                                                <asp:ListItem>--Select--</asp:ListItem>
                                                <asp:ListItem>Monthly</asp:ListItem>
                                                <asp:ListItem>Quarterly</asp:ListItem>
                                                <asp:ListItem>Half Yearly</asp:ListItem>
                                                <asp:ListItem>Yearly</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>

                                        <td>
                                            <asp:Label ID="lblmonth" runat="server" Text="Month" Visible="false"></asp:Label><span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlMonth" runat="server" class="sdrop" Visible="false"></asp:DropDownList>
                                        </td>


                                    </tr>
                                    <tr style="height:40px">
                                        <td>Type
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddltype" runat="server" class="sdrop" OnSelectedIndexChanged="ddltype_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem>Amount</asp:ListItem>
                                                <asp:ListItem>Percentage</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>

                                        <td>
                                            <asp:Label ID="lblperon" runat="server" Text="Per On" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlperon" runat="server" Visible="false" class="sdrop">
                                                <asp:ListItem>Gross</asp:ListItem>
                                                <asp:ListItem>Gross+OT</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>


                                    </tr>
                                    <tr style="height:40px">
                                        <td>Employee Contribution
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmployeeContribution" Text="0" runat="server" class="sinput"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTBEDOB" runat="server" Enabled="True" TargetControlID="txtEmployeeContribution"
                                                ValidChars=".0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>From<span style="color: Red">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtFromDate" TabIndex="11" runat="server" class="sinput" MaxLength="10"
                                                onkeyup="dtval(this,event)"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CEFromDate" runat="server" Enabled="true" TargetControlID="txtFromDate"
                                                Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="FTBEFromDate" runat="server" Enabled="True" TargetControlID="txtFromDate"
                                                ValidChars="-/0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>

                                    <tr style="height:40px">
                                        <td>Employer Contribution
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmployeerContribution" Text="0" runat="server" class="sinput"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="txtEmployeerContribution"
                                                ValidChars=".0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>To<span style="color: Red">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtToDate" TabIndex="11" runat="server" class="sinput" MaxLength="10"
                                                onkeyup="dtval(this,event)"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CEToDate" runat="server" Enabled="true" TargetControlID="txtToDate"
                                                Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="FTBEToDate" runat="server" Enabled="True" TargetControlID="txtToDate"
                                                ValidChars="-/0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr style="height:40px">
                                        <td>Maximum
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMaximum" Text="0" runat="server" class="sinput"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="txtMaximum"
                                                ValidChars=".0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnsave" runat="server" Text="Save" class="btn save"
                                                OnClick="btnsave_Click" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>

                               

                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <!-- CONTENT AREA END -->
    </div>
</asp:Content>
