<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="ReportforPBAttendance.aspx.cs" Inherits="SRF.P.Module_Reports.ReportforPBAttendance" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
  <link href="css/global.css" rel="stylesheet" type="text/css" />

     <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Art Comparison</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Art Comparison
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table width="100%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>
                                                Clientid :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlclientid" runat="server" class="sdrop" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Name :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlcname" runat="server" class="sdrop" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Month :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtmonth" runat="server" class="sinput"> </asp:TextBox><cc1:CalendarExtender
                                                    ID="txtFrom_CalendarExtender" runat="server" Enabled="true" TargetControlID="txtmonth"
                                                    Format="MM/dd/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="txtmonth"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnsearch" runat="server" Text="Search" class="btn save" OnClick="btnsearch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvpaysheet" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        CellSpacing="3" EmptyDataRowStyle-BackColor="BlueViolet" EmptyDataRowStyle-BorderColor="Aquamarine"
                                        EmptyDataRowStyle-Font-Italic="true" EmptyDataText="No Records Found" ForeColor="#333333"
                                        GridLines="None" Width="100%" OnRowDataBound="gvpaysheet_RowDataBound">
                                        <RowStyle BackColor="#EFF3FB" Height="30" />
                                        <EmptyDataRowStyle BackColor="SkyBlue" BorderColor="Aquamarine" Font-Italic="True" />
                                        <Columns>
                                            <asp:BoundField HeaderText="Clientid" DataField="Clientid" />
                                            <asp:BoundField HeaderText="Name" DataField="Clientname" />
                                            <asp:BoundField HeaderText="Paysheet Duties" DataField="PaysheetDuties" />
                                            <asp:BoundField HeaderText="Paysheet Ots" DataField="PaysheetOts" />
                                            <asp:BoundField HeaderText="Total Paysheet Duties" DataField="TotalPaysheetduties" />
                                            <asp:TemplateField HeaderText="Billing Duties">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBillingduties" runat="server"> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Billing ots">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBillingots" runat="server"> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Difference">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDifferenceduties" runat="server"> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </div>
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
