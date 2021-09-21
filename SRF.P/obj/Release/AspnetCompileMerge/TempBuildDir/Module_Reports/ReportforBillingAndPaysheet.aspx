<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="ReportforBillingAndPaysheet.aspx.cs" Inherits="SRF.P.Module_Reports.ReportforBillingAndPaysheet" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
<link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Load.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 47%;
        }
    </style>

     <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Billing and
                        Paysheet</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Billing and Paysheet
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <div align="right">
                                        <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>
                                    </div>
                                
                                <asp:UpdatePanel runat="server" ID="Updatepanel1">
                                    <ContentTemplate>
                                     
                                            <table width="52%">
                                                <tr>
                                                    <td>
                                                        Month :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtmonth" runat="server" Text="" class="sinput"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                            Format="MM/dd/yyyy" TargetControlID="txtmonth">
                                                        </cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="txtmonth"
                                                            ValidChars="/0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSearch" runat="server" class="btn save" Text="Search" OnClick="btnSearch_Click" />
                                                    </td>
                                                </tr>
                                                <tr style="width: 100%">
                                                    <td colspan="3" style="width: 30%">
                                                        <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"> </asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="100%"
                                                Height="50px" CssClass="datagrid" CellPadding="4" CellSpacing="3" ForeColor="#333333"
                                                GridLines="None">
                                                <RowStyle BackColor="#EFF3FB" Height="30" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Client ID">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblclientid" Text="<%#Bind('ClientId') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Client Name">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblclientname" Text="<%#Bind('clientname') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Billing Dts">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblBillingDts" Text="<%#Bind('Invoice') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Billing Generation">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblBillinggeneration" Text="<%#Bind('BillGenerate') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Payment Dts">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblPaymentDts" Text="<%#Bind('paysheet') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Payment Generation">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblPaymentgeneration" Text="<%#Bind('PaysheetGenerate') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                        </div>
                                        <div class="dashboard_firsthalf" style="width: 100%">
                                            <table width="70%">
                                                <tr>
                                                    <td>
                                                        Billing Duties :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBduties" runat="server" ReadOnly="True" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Pay Sheet Duties :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPduties" runat="server" ReadOnly="True" class="sinput"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="45%" cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td>
                                                        No of Clients for Billing : &nbsp;&nbsp;&nbsp;Yes
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBYes" runat="server" ReadOnly="True" class="sinput" Width="56px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBNo" runat="server" ReadOnly="True" class="sinput" Width="56px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        No of Clients for Paysheet: Yes
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPYes" runat="server" ReadOnly="True" class="sinput" Width="56px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPNo" runat="server" ReadOnly="True" class="sinput" Width="56px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                            <ProgressTemplate>
                                                <div id="Background">
                                                </div>
                                                <div id="Progress">
                                                </div>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <!-- DASHBOARD CONTENT END -->
       
    </div>

</asp:Content>
