<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="PaySheetReport.aspx.cs" Inherits="SRF.P.Module_Reports.PaySheetReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
     <link href="css/global.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2
        {
            font-size: 10pt;
            font-weight: bold;
            color: #333333;
            background: #cccccc;
            padding: 5px 5px 2px 10px;
            border-bottom: 1px solid #999999;
            height: 26px;
        }
    </style>

       <div id="content-holder">
        <div class="content-holder">
           <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Paysheet Report</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
              
                   <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                               Paysheet Report
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin" style="height: 650px">
                            
                            
                        <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                        </asp:ScriptManager>
                        
                       
                            <div align="right">
                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>
                            </div>
                          
                           <div class="dashboard_firsthalf" style="width: 100%"> 
                                <table width="100%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td>
                                            Client ID : </td>
                                          <td>  <asp:DropDownList runat="server" class="sdrop" ID="ddlClientId" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlClientId_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Client Name :</td>
                                         <td>   <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="true" class="sdrop"
                                                OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Month :</td>
                                         <td>   <asp:TextBox ID="txtmonth" runat="server" Text="" class="sinput" ></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                TargetControlID="txtmonth" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="txtmonth"
                                                ValidChars="/0123456789">
                                            </cc1:FilteredTextBoxExtender></td>
                                         <td>   <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
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
                            <div style="overflow:scroll;height:700;width:auto">
                                <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="170%"
                                    Height="50px" CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None">
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>
                                    
                                             <asp:TemplateField HeaderText="S.No"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
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
                                        
                                        <asp:BoundField DataField="ContractStartDate" HeaderText="Contract Start Date" DataFormatString="{0:dd/MM/yyyy}"  />
                                        <asp:BoundField DataField="ContractEndDate" HeaderText="Contract End Date" DataFormatString="{0:dd/MM/yyyy}" />

                                        
                                         <asp:TemplateField HeaderText="Designation">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbldesgn" Text="<%# Bind('Design') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <asp:BoundField DataField="NoOfDuties" HeaderText="No of Duties" DataFormatString="{0:0}"  />
                                         <asp:BoundField DataField="basic" HeaderText="Basic" DataFormatString="{0:0}"  />
                                        <asp:BoundField DataField="da" HeaderText="DA" DataFormatString="{0:0}" />
                                        <asp:BoundField DataField="hra" HeaderText="HRA" DataFormatString="{0:0}" />
                                        <asp:BoundField DataField="cca" HeaderText="CCA" DataFormatString="{0:0}" />
                                        <asp:BoundField DataField="Conveyance" HeaderText="Conv." DataFormatString="{0:0}" />
                                         <asp:BoundField DataField="OTAmt" HeaderText="OT Amt" DataFormatString="{0:0}" />
                                        <asp:BoundField DataField="WOAmt" HeaderText="WO Amt" DataFormatString="{0:0}" />
                                        
                                         <asp:BoundField DataField="Nhsamt" HeaderText="Nhs Amt" DataFormatString="{0:0}" />
                                        <asp:BoundField DataField="Npotsamt" HeaderText="Npots Amt" DataFormatString="{0:0}" />
                                          <asp:BoundField DataField="LeaveEncashAmt" HeaderText="L.A" DataFormatString="{0:0}" />
                                        
                                         <asp:BoundField DataField="WashAllowance" HeaderText="W.A" DataFormatString="{0:0}"  />
                                        <asp:BoundField DataField="OtherAllowance" HeaderText="O.A" DataFormatString="{0:0}" />
                                      
                                         <asp:BoundField DataField="ActualAmount" HeaderText="Total Salary" DataFormatString="{0:0}" />
                                        
                                        <asp:BoundField DataField="PF" HeaderText="PF Percetange on" />
                                         <asp:BoundField DataField="pffrom" HeaderText="PF On" />
                                          <asp:BoundField DataField="Pflimit" HeaderText="PF Limit" />
                                        
                                        <asp:BoundField DataField="ESI" HeaderText="ESI" />
                                          <asp:BoundField DataField="Esifrom" HeaderText="ESI On" />
                                          <asp:BoundField DataField="Esilimit" HeaderText="ESI Limit" />
                                          <asp:BoundField DataField="ottype" HeaderText="OT Type" />
                                          <asp:BoundField DataField="OTPersent" HeaderText="OT 100/200%" />
                                      
                                      
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
        <!-- DASHBOARD CONTENT END -->

    </div>

</asp:Content>
