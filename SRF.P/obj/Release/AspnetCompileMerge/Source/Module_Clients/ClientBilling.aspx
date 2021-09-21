<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Clients/Clients.master" AutoEventWireup="true" CodeBehind="ClientBilling.aspx.cs" Inherits="SRF.P.Module_Clients.ClientBilling" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />

    <script language="javascript" src="scripts\Calendar.js" type="text/javascript"></script>


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
    <link href="css/Calendar.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #social div {
            display: block;
        }

        .HeaderStyle {
            text-align: Left;
        }


        .modalBackground {
            background-color: Gray;
            z-index: 10000;
        }
    </style>

    <div id="content-holder">
        <div class="content-holder">
            <h1 class="dashboard_heading">Clients Dashboard</h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_full">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">CLIENT BILLING</h2>
                        </div>
                        <div style="text-align: center">
                            <asp:Label ID="lblResult" runat="server" Text="" Style="color: Red"></asp:Label>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <!--  Content to be add here> -->
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="Scriptmanager1">
                                </asp:ScriptManager>
                                <table width="95%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <!-- First Part -->
                                        <td>
                                            <table cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td>Client ID<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlclientid" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged"
                                                            class="sdrop" Width="120px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Month<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlmonth" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlmonth_SelectedIndexChanged"
                                                            class="sdrop" Width="120px">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtmonth" runat="server" AutoPostBack="true" Width="120px"
                                                            OnTextChanged="txtmonthOnTextChanged" Visible="false"></asp:TextBox>
                                                        &nbsp;&nbsp;
                                                            <asp:CheckBox ID="Chk_Month" runat="server"
                                                                Text="Old" />
                                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true"
                                                            Format="dd/MM/yyyy" TargetControlID="txtmonth">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <!-- First Part End-->
                                        <!-- Second Part -->
                                        <td>
                                            <table cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>Client Name<span style="color: Red">*</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlCname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCname_OnSelectedIndexChanged"
                                                                        class="sdrop" Width="355px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btngenratepayment"
                                                                    runat="server" class="btn save" Text="Genrate Bill" OnClick="Btn_Genrate_Invoice_Click"
                                                                    OnClientClick='return confirm(" Are you sure you  want to  generate bill ?");' />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>Year&nbsp;&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtyear" runat="server" Text="2013" Enabled="False" class="sinput"
                                                                        Width="50px"></asp:TextBox>
                                                                </td>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;From&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtfromdate" runat="server" Enabled="true" class="sinput" Width="80px"
                                                                        onkeyup="dtval(this,event)"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="txtfromdate_CalendarExtender" runat="server" Enabled="true"
                                                                        TargetControlID="txtfromdate" Format="dd/MM/yyyy">
                                                                    </cc1:CalendarExtender>
                                                                </td>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;To&nbsp;&nbsp;&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txttodate" runat="server" Enabled="true" class="sinput" Width="80px"
                                                                        onkeyup="dtval(this,event)"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="txttodate_Calender" runat="server" Enabled="true" TargetControlID="txttodate"
                                                                        Format="dd/MM/yyyy">
                                                                    </cc1:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table width="50%" cellpadding="5" cellspacing="5" style="margin-left: 17px">
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="linkmanualbilling" runat="server" Text="Manual Bills" PostBackUrl="~/Manual Billing.aspx" Visible="false"></asp:LinkButton>
                                        </td>

                                        <td>
                                            <a href="newmanualbill.aspx" class=" btn-link">New Manual Bill Model</a>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblbillnolatesttest" runat="server" Style="font-weight: bold;" Text="BillNo :"> </asp:Label>
                                            <asp:Label ID="lblbillnolatest" runat="server" Style="font-weight: bold;" Text=""> </asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table width="40%" cellpadding="5" cellspacing="5" style="margin-left: 17px">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblbilldate" runat="server" Text="Bill Date :" Style="font-weight: bold;"></asp:Label>
                                        </td>
                                        &nbsp; &nbsp; &nbsp;
                                        <td>
                                            <asp:TextBox ID="txtbilldate" runat="server" Text="" class="sinput" Width="80px" onkeyup="dtval(this,event)"> </asp:TextBox>
                                            <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                TargetControlID="txtbilldate" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="FTBEstartdate" runat="server" Enabled="True" TargetControlID="txtbilldate"
                                                ValidChars="/0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="Label1" runat="server" Text="Due Date :" Style="font-weight: bold;"></asp:Label>
                                        </td>
                                        &nbsp; &nbsp; &nbsp;
                                        <td>
                                            <asp:TextBox ID="txtduedate" runat="server" Text="" class="sinput" Width="80px" onkeyup="dtval(this,event)"> </asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true" TargetControlID="txtduedate"
                                                Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                TargetControlID="txtduedate" ValidChars="/0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                                &nbsp;
                                <asp:LinkButton ID="linkdelete" runat="server" Text="Delete Bills" Visible="false"></asp:LinkButton>
                                <div style="margin-left: 30px">
                                    <cc1:ModalPopupExtender ID="mpebilldelete" runat="server" TargetControlID="linkdelete"
                                        PopupControlID="pnlbilldeletedetails" CancelControlID="btncancel">
                                    </cc1:ModalPopupExtender>
                                    <asp:Panel ID="pnlbilldeletedetails" runat="server" Width="400px" Style="background-color: Silver"
                                        Visible="false">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>Enter Bill No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtbillno" runat="server" Width="240px" AutoPostBack="true" OnTextChanged="txtbillno_OnTextChanged"> 
                                   
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <br />
                                                    <tr>
                                                        <td>Client Id
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtclientid" runat="server" Width="240px"> 
                                   
                                                            </asp:TextBox>
                                                        </td>
                                                        <tr>
                                                            <td>Client Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtclientname" runat="server" Width="240px"> 
                                   
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btndelelte" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <table style="margin-left: 150px">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btndelelte" runat="server" Text="Delete" CssClass="btn save" OnClientClick='return confirm(" Are you sure you  want to  delete bill ?");'
                                                        OnClick="btndelelte_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btncancel" runat="server" Text="Cancel/Close" CssClass="btn save"
                                                        Width="95px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </div>


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

                                <div class="rounded_corners" style="overflow: auto; width: 99%; margin-left: 17px">
                                    <asp:GridView ID="gvClientBilling" runat="server" AutoGenerateColumns="False" EmptyDataRowStyle-BackColor="BlueViolet"
                                        EmptyDataRowStyle-BorderColor="Aquamarine" Width="95%" CellPadding="4" CellSpacing="3"
                                        ForeColor="#333333" GridLines="None">
                                        <RowStyle BackColor="#EFF3FB" Height="30" />
                                        <Columns>
                                            <%--<asp:TemplateField HeaderText="Unit Id" ItemStyle-Width="30px">
                        <ItemTemplate>
                        <asp:Label ID="lblunitid" runat="server" Text="<%#Bind('unitid')%>"> </asp:Label>
                        </ItemTemplate>
                       </asp:TemplateField> --%>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldesgn" runat="server" Text='<%# Bind("Designation") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="No. of Emps ">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblnoofemployees" runat="server" Text='<%#Bind("NoofEmps")%>'> </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No.of Dts/Hrs">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNoOfDuties" runat="server" Text='<%#Bind("DutyHours")%>'> </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pay Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpayrate" runat="server" Text='<%#Eval("payrate", "{0:0.##}")%>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblda" runat="server" Text='<%#Eval("BasicDa", "{0:0.##}")%>'> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Total">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmount" runat="server" Text=""></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OT Amount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOtAmount" runat="server" Text='<%#Eval("OTAmount", "{0:0.##}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>

                                        <EmptyDataRowStyle BackColor="BlueViolet" BorderColor="Aquamarine"></EmptyDataRowStyle>

                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </div>
                                <table width="100%" cellpadding="5" cellspacing="5" style="margin-left: 17px">
                                    <tr>
                                        <td valign="top" width="37%">
                                            <asp:CheckBox ID="checkExtraData" Text="&nbsp;&nbsp;Extra Data for Billing" runat="server"
                                                Checked="false" AutoPostBack="True" OnCheckedChanged="checkExtraData_CheckedChanged" />
                                            <asp:Panel ID="panelRemarks" runat="server" Visible="false">
                                                <table width="100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td>Sevice Tax
                                                        </td>
                                                        <td>Service Charge
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtmachinarycost" runat="server" Text="Machinery Cost :" class="sinput"
                                                                Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMachinery" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTYesMachinary" runat="server"
                                                                Checked="false" Text=" Yes" />
                                                        </td>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txtMachinery" ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <td>
                                                            <asp:CheckBox ID="chkSCYesMachinary" runat="server" Checked="false"
                                                                Text=" Yes" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtMaterialcost" runat="server" Text="Material Cost :" class="sinput"
                                                                Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMaterial" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTYesMaterial" runat="server"
                                                                Checked="false" Text=" Yes" />
                                                        </td>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                            TargetControlID="txtMaterial" ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <td>
                                                            <asp:CheckBox ID="chkSCYesMaterial" runat="server" Checked="false"
                                                                Text=" Yes" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtMaintanancecost" runat="server" Text="Maintenance Work :" class="sinput"
                                                                Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtElectical" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTYesElectrical" runat="server"
                                                                Checked="false" Text=" Yes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSCYesElectrical" runat="server" Checked="false"
                                                                Text=" Yes" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                TargetControlID="txtElectical" ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtextraonetitle" runat="server" Text="Extra Amount one :" class="sinput"
                                                                Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtextraonevalue" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTYesExtraone" runat="server"
                                                                Checked="false" Text=" Yes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSCYesExtraone" runat="server" Checked="false"
                                                                Text=" Yes" />
                                                        </td>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                            TargetControlID="txtextraonevalue" ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtextratwotitle" runat="server" Height="19px" Text="Extra Amount Two :"
                                                                class="sinput" Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtextratwovalue" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTYesExtratwo" runat="server"
                                                                Checked="false" Text=" Yes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSCYesExtratwo" runat="server" Checked="false"
                                                                Text=" Yes" />
                                                        </td>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                            TargetControlID="txtextratwovalue" ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtdiscount" runat="server" Text="Discounts :" class="sinput" Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDiscounts" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                TargetControlID="txtDiscounts" ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTDiscountone" runat="server" Checked="false" Text=" Before Service Tax" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtdiscounttwotitle" runat="server" Text="Discount Two:" class="sinput"
                                                                Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtdiscounttwovalue" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                                TargetControlID="txtdiscounttwovalue" ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTDiscounttwo" runat="server" Checked="false" Text=" Before Service Tax" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table>
                                                                <tr>
                                                                    <td>Remarks :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRemarks" runat="server" Text="" TextMode="MultiLine" class="sinput"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                        <td align="right" valign="top">
                                            <table width="60%" cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td width="80%" style="text-align: right">Total :
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblTotalResources" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblRelChrTitle" Visible="false" Text=" 1/6 Reliever Charges : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblRelChrgAmt" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblServiceChargeTitle" Visible="false" Text=" Service Charges : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblServiceCharges" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblStaxamtonServicechargetitle" Visible="false" Text=" Service Tax on Service Charges : " runat="server"></asp:Label></td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblStaxamtonServicecharge" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSChargeamtonMachinarytitle" Visible="false" Text=" Service Charge on Machinary Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSChargeamtonMachinary" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonMaintenancetitle" Visible="false" Text=" Service Charge on Maintenance Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonMaintenance" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonMaterialtitle" Visible="false" Text=" Service Charge on Material Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonMaterial" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonExtraonetitle" Visible="false" Text=" Service Charge on Extra amount one : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonExtraone" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonExtratwotitle" Visible="false" Text=" Service Charge on Extra amount two : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonExtratwo" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblMachineryTitlewithst" Visible="false" Text=" Machinery Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblMachinerywithst" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblMaterialTitlewithst" Visible="false" Text=" Material Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblMaterialwithst" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblElectricalTitlewithst" Visible="false" Text=" Maintenance Work : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblElectricalwithst" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblextraonetitlewithst" Visible="false" Text="Extra Amount One : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblextraonewithst" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblextratwotitlewithst" Visible="false" Text="Extra Amount Two : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblextratwowithst" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblDiscountTitlewithst" Visible="false" Text="Discount : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblDiscountwithst" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblDiscounttwotitlewithst" Visible="false" Text="Discount Two: " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblDiscounttwowithst" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblServiceTaxTitle" Visible="false" Text="Service Tax :" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblServiceTax" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSBCESSTitle" Visible="false" Text="SB CESS :" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSBCESS" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblKKCESSTitle" Visible="false" Text="KK CESS :" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblKKCESS" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <%-- region for GST as on 23-6-2017 by sharada--%>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblCGSTTitle" Visible="false" Text="CGST :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtCGSTPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblCGST" Text="" Visible="false" runat="server" Enabled="false"></asp:Label>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSGSTTitle" Visible="false" Text="SGST :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtSGSTPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSGST" Text="" Visible="false" runat="server" Enabled="false"></asp:Label>
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblIGSTTitle" Visible="false" Text="IGST :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtIGSTPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblIGST" Text="" Visible="false" runat="server" Enabled="false"></asp:Label>
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblCess1Title" Visible="false" Text="Cess1 :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtCess1Prc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblCess1" Text="" Visible="false" runat="server" Enabled="false"></asp:Label>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblCess2Title" Visible="false" Text="Cess2 :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtCess2Prc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblCess2" Text="" Visible="false" runat="server" Enabled="false"></asp:Label>
                                                    </td>
                                                </tr>

                                                <%-- endregion for GST as on 17-6-2017 by swathi--%>


                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblCESSTitle" Visible="false" Text="CESS :" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblCESS" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSheCESSTitle" Visible="false" Text="S&H Ed. CESS :" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSheCESS" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblST75Title" Visible="false" Text="Less 75% Service Tax :" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblST75" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblST25Title" Visible="false" Text="Service Tax Chargable @3.09%:" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblST25" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblMachineryTitle" Visible="false" Text=" Machinery Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblMachinery" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblMaterialTitle" Visible="false" Text=" Material Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblMaterial" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblElectricalTitle" Visible="false" Text=" Maintenance Work : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblElectrical" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblextraoneamttitle" Visible="false" Text="Extra Amount One : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblextraamt" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblextratwoamttitle" Visible="false" Text="Extra Amount Two : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblextratwoamt" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblDiscountTitle" Visible="false" Text="Discount : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblDiscount" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblDiscounttwoTitle" Visible="false" Text="Discount Two: " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblDiscounttwo" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right; font-weight: bold">Grand Total :
                                                    </td>
                                                    <td width="20%" style="text-align: right; font-weight: bold">
                                                        <asp:Label ID="lblGrandTotal" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <asp:Label ID="lblRemarks" Text="" runat="server" Visible="false"></asp:Label>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>


                                                    <td style="text-align: right; font-weight: bold">
                                                        <asp:Button ID="btninvoice" runat="server" Text="Invoice PDF" class="btn save" Visible="false" OnClick="btninvoice_Click" /><br />
                                                    </td>
                                                    <td style="text-align: right; font-weight: bold">
                                                        <asp:Button ID="btninvoice2" runat="server" Text="Invoice" class="btn save" Visible="false" OnClick="btninvoice2_Click" />
                                                    </td>

                                                    <td style="text-align: right; font-weight: bold">
                                                        <asp:Button ID="Button1" runat="server" Text="Invoice New" class="btn save" Visible="false" OnClick="btninvoicenew_Click" /><br />
                                                    </td>

                                                    <td style="text-align: right; font-weight: bold">
                                                        <asp:Button ID="Button2" runat="server" Text="Tax Invoice" class="btn save" OnClick="btninvoicenew1_Click" /><br />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btninvoice3" runat="server" Text="Invoice2" class="btn save" OnClick="btninvoice3_Click" Visible="false" />
                                                    </td>
                                                    <td style="text-align: right; font-weight: bold">
                                                        <asp:Button ID="btninvoiceletterhead" runat="server" Text="Letter Head" class="btn save" Visible="false"
                                                            OnClick="btninvoiceletterhead_Click" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>

                                                        <%--<asp:Label ID="Label2" runat="server" Text="Payment Options :"></asp:Label>&nbsp;--%>
                                                        <asp:DropDownList ID="ddlpaymenttype" runat="server" class="sdrop" Visible="false" Width="100px">


                                                            <%--<asp:ListItem>APMDC (OTHERS)</asp:ListItem>--%>
                                                            <asp:ListItem>mhb form t consol</asp:ListItem>
                                                            <%--<asp:ListItem>ICICI PRUDENTIAL LIFE</asp:ListItem>
                                                     <asp:ListItem>ICICI BANK</asp:ListItem>--%>
                                                        </asp:DropDownList>

                                                    </td>
                                                    <td>

                                                        <asp:Button ID="btndownloadpdffile" runat="server" Text="Download Bill" class="btn save" OnClick="btndownloadpdffile_Click" Visible="false" Style="width: 138px" />

                                                    </td>

                                                </tr>

                                                <div>
                                                    <asp:Label ID="lbltotalamount" runat="server"> </asp:Label>
                                                </div>
                                                <div style="width: 100%; font-weight: bold">
                                                    <asp:Label ID="lblamtinwords" Text="" runat="server" Visible="false"> </asp:Label>
                                                </div>
                                            </table>
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
        <!-- CONTENT AREA END -->
        <!-- FOOTER BEGIN -->


    </div>

</asp:Content>
