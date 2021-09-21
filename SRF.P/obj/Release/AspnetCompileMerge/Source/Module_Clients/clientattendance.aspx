<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Clients/Clients.master" AutoEventWireup="true" CodeBehind="clientattendance.aspx.cs" Inherits="SRF.P.Module_Clients.clientattendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />

    <script language="javascript" src="scripts\Calendar.js" type="text/javascript"></script>

    <link href="css/Calendar.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #social div {
            display: block;
        }

        .HeaderStyle {
            text-align: Left;
        }

        .style3 {
            height: 24px;
        }
    </style>
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            z-index: 10000;
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

    <style type="text/css">
        .slidingDiv {
            background-color: #99CCFF;
            padding: 10px;
            margin-top: 10px;
            border-bottom: 5px solid #3399FF;
        }

        .show_hide {
            display: none;
        }
    </style>

    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function () {


            $(".slidingDiv").hide();
            $(".show_hide").show();

            $('.show_hide').click(function () {
                $(".slidingDiv").slideToggle();
            })
            if (isPostBack) { $(".slidingDiv").show(); }

        });

    </script>


    <div id="content-holder">
        <div class="content-holder">
            <h1 class="dashboard_heading">Clients Dashboard</h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_full">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">CLIENT ATTENDANCE</h2>
                        </div>
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px; min-height: 200px; height: auto">
                            <!--  Content to be add here> -->
                            <div class="boxin">
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table width="95%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>
                                                <table width="100%" cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td width="110px">Client ID<span style="color: Red">*</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlClientID" runat="server" class="sdrop" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlClientID_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Phone N0(s)
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtphonenumbers" runat="server" class="sinput" Enabled="False" AutoCompleteType="Disabled"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Month
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlMonth" class="sdrop" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="Txt_Month" runat="server" AutoPostBack="true" class="sinput"
                                                                OnTextChanged="Txt_Month_OnTextChanged" Text="" Visible="false"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="txtMonth_CalendarExtender" runat="server"
                                                                Enabled="true" Format="dd/MM/yyyy" TargetControlID="Txt_Month">
                                                            </cc1:CalendarExtender>
                                                            <cc1:FilteredTextBoxExtender ID="txtMonth_FilteredTextBoxExtender"
                                                                runat="server" Enabled="True" TargetControlID="Txt_Month"
                                                                ValidChars="/0123456789">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:CheckBox ID="Chk_Month" runat="server" Text="Old" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table style="height: 100%" cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>Client Name
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCName" runat="server" AutoPostBack="True" class="sdrop"
                                                                Width="305px" OnSelectedIndexChanged="ddlCName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Our Contact Person
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtocp" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>OT in terms of
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlOTType" class="sdrop">
                                                                <asp:ListItem>Days</asp:ListItem>
                                                                <asp:ListItem>Hours</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <br />
                                    &nbsp;&nbsp;&nbsp;&nbsp;Attendance Mode :
                                    <asp:RadioButton ID="radioall" runat="server" Text="All" AutoPostBack="true" GroupName="a" Checked="true"
                                        OnCheckedChanged="radioindividual_CheckedChanged" />

                                    <asp:RadioButton ID="radioindividual" runat="server" Text="Individual" AutoPostBack="true"
                                        GroupName="a" OnCheckedChanged="radioindividual_CheckedChanged" Visible="false" />
                                    <%--  <asp:RadioButton ID="radioall" runat="server" Text="All" AutoPostBack="true" 
                                       GroupName="a" oncheckedchanged="radioindividual_CheckedChanged"/>
                                    --%>

                                    <asp:RadioButton ID="radiospecialdays" runat="server" Text="Special Days" Visible="false"
                                        OnCheckedChanged="radioindividual_CheckedChanged" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                          <asp:LinkButton ID="lnkClear" runat="server" Text="Clear" Visible="false"
                                              OnClientClick='return confirm("Are you sure you want to Clear The Attendance . ?");'
                                              OnClick="lnkClear_Click">
                                          </asp:LinkButton>
                                </div>

                                <br />
                                <br />
                                <cc1:ModalPopupExtender ID="modelLogindetails" runat="server" TargetControlID="Chk_Month" PopupControlID="pnllogin"
                                    BackgroundCssClass="modalBackground">
                                </cc1:ModalPopupExtender>

                                <asp:Panel ID="pnllogin" runat="server" Height="100px" Width="300px" Style="display: none; position: absolute; background-color: Silver;">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font: bold; font-size: medium">&nbsp;&nbsp;&nbsp;
                            Enter Password:
                                                    </td>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                    </td>
                                                </tr>

                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <br />
                                    <table style="background-position: center;">
                                        <tr>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                              <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" class="btn Save" />
                                            </td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" class="btn Save" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                                <div class="rounded_corners">
                                    <asp:GridView ID="gvfromcontracts" runat="server" Height="75px" Width="100%" Style="margin-left: 0px"
                                        AutoGenerateColumns="False" CellPadding="5" CellSpacing="3" ForeColor="#333333"
                                        BorderStyle="Solid" BorderColor="Black" BorderWidth="0px" GridLines="None" HeaderStyle-CssClass="HeaderStyle"
                                        HeaderStyle-Height="10px" RowStyle-Height="8px" ShowFooter="true"
                                        OnRowDataBound="gvfromcontracts_RowDataBound">
                                        <RowStyle BackColor="#EFF3FB" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50%">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbldesginid" Text="<%#Bind('Designid') %>" Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="lblDesignation" Text="<%#Bind('Design') %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="TxtQuantity" Text="<%#Bind('Quantity') %>" Style="text-align: center" Width="90%"></asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotalQty" runat="server"></asp:Label>
                                                </FooterTemplate>
                                                <ItemStyle></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Duties" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtDuties" Text="<%#Bind('duties') %>" Style="text-align: center" Width="90%"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                        TargetControlID="txtDuties" FilterMode="ValidChars" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>

                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotalDuties" runat="server"></asp:Label>
                                                </FooterTemplate>
                                                <ItemStyle></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Duties(In Hours)" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtDutiesInHours" Text="" AutoPostBack="true" Width="90%"
                                                        OnTextChanged="txtDutiesInHours_textChanged"
                                                        Style="text-align: center"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                        TargetControlID="txtDutiesInHours" FilterMode="ValidChars" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>

                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotalDutiesinhrs" runat="server"></asp:Label>
                                                </FooterTemplate>
                                                <ItemStyle></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OTs" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtOTs" Text="<%#Bind('OT') %>" Style="text-align: center" Width="90%"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                        TargetControlID="txtOTs" FilterMode="ValidChars" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>

                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotalOts" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </div>

                                <table width="100%">
                                    <tr>
                                        <td width="25%"></td>
                                        <td width="25%">
                                            <asp:Label ID="lblTotalDuties" runat="server" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td width="25%">
                                            <asp:Label ID="lblTotalOts" runat="server" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td width="25%"></td>
                                    </tr>
                                    <tr>
                                        <td width="25%"></td>
                                        <td width="25%"></td>
                                        <td width="25%"></td>
                                        <td colspan="4">
                                            <asp:Label ID="lbltotaldesignationlist" runat="server" Text="">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btn_Save_AttenDance" runat="server" Text="Save" class=" btn save"
                                                OnClick="btn_Save_AttenDanceClick" OnClientClick='return confirm(" Are you sure  you want to add this record ?");' />
                                            <asp:Button ID="Btn_Cancel_AttenDance" runat="server" class=" btn save" Text="Cancel"
                                                OnClientClick='return confirm(" Are you sure you  want to cancel this entry ?");'
                                                OnClick="Btn_Cancel_AttenDance_Click" />
                                            <br />
                                            <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
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
