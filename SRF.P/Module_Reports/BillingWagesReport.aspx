<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="BillingWagesReport.aspx.cs" Inherits="SRF.P.Module_Reports.BillingWagesReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

      <link href="css/global.css" rel="stylesheet" type="text/css" />

       <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Billing Wages
                        Report</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Billing Wages Report
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div align="right">
                                    <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>
                                </div>
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table width="70%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>
                                                Client ID :
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" class="sdrop" ID="ddlClientId" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlClientId_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Client Name :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="true" class="sdrop"
                                                    OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                &nbsp;<asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                            </td>
                                        </tr>
                                        <tr style="width: 100%">
                                            <td colspan="6">
                                                <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"> </asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="rounded_corners">
                                    <div style="overflow: scroll;width: auto">
                                        <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="100%"
                                            CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None">
                                            <RowStyle BackColor="#EFF3FB" Height="30" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Client ID">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblclientid" Text="<%# Bind('ClientId') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Client Name">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblclientname" Text="<%# Bind('ClientName') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ContractStartDate" HeaderText="Contract Start Date" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField DataField="ContractEndDate" HeaderText="Contract End Date" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField DataField="PaymentType" HeaderText="Lumpsum" />
                                                <asp:BoundField DataField="ServiceTaxType" HeaderText="With/Without ST" />
                                                <asp:BoundField DataField="IncludeST" HeaderText="Include ST/Not" />
                                                <asp:BoundField DataField="ServiceTax75" HeaderText="ST 75%" />
                                                <asp:BoundField DataField="Design" HeaderText="Designation" />
                                                <asp:BoundField DataField="Quantity" HeaderText="Qty" />
                                                <asp:BoundField DataField="Amount" HeaderText="Pay Rate" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="PayType" HeaderText="Pay Type" />
                                                <asp:BoundField DataField="NoOfDays" HeaderText="No Of Days" />
                                                <asp:BoundField DataField="ServiceCharge" HeaderText="Service Charge" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="BillDates" HeaderText="Bill Period" />
                                                <asp:BoundField DataField="Nots" HeaderText="NOTs" />
                                                <asp:BoundField DataField="Basic" HeaderText="Basic" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="DA" HeaderText="DA" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="HRA" HeaderText="HRA" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="Conveyance" HeaderText="Conv." DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="CCA" HeaderText="CCA" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="LeaveAmount" HeaderText="L.A" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="Gratuity" HeaderText="Gratuity" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="Bonus" HeaderText="Bonus" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="WashAllownce" HeaderText="W.A" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="OtherAllowance" HeaderText="O.A" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="NFhs" HeaderText="Nfhs" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="RC" HeaderText="RC" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="CS" HeaderText="CS" DataFormatString="{0:0}" />
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
                                <div>
                                    <table width="100%">
                                        <tr style="width: 100%; font-weight: bold">
                                            <td style="width: 60%">
                                                <asp:Label ID="lbltamttext" runat="server" Visible="false" Text="Total Amount"></asp:Label>
                                            </td>
                                            <td style="width: 40%">
                                                <asp:Label ID="lbltmtemppf" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="lbltemprpf" runat="server" Text="" Style="margin-left: 30%"></asp:Label>
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
        </div>
       
    </div>

</asp:Content>
