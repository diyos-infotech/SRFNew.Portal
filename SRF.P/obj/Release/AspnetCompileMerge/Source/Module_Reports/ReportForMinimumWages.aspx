<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="ReportForMinimumWages.aspx.cs" Inherits="SRF.P.Module_Reports.ReportForMinimumWages" %>
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

     <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
              <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Minimum Wages Report</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
              <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                               List of Clients
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                        <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                        </asp:ScriptManager>
                        <div class="dashboard_firsthalf" style="width: 100%">
                        
                            <div align="right">
                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="False">Export to Excel</asp:LinkButton>
                            </div>
                            
                           
                                <table width="100%">
                                    <tr style="width: 30%;">
                                        <td >
                                            Purpose Name</td>
                                            <td  > 
                                        <asp:DropDownList ID="Ddl_Minimum_Wages_Categories" runat="server"  class="sdrop"  ></asp:DropDownList>
                                        
                                        </td>
                                        <td>
                                            Report Type</td>
                                        <td>    <asp:DropDownList ID="Ddl_Report_Type" runat="server"   class="sdrop"
                                                 >
                                                 <asp:ListItem>ALL</asp:ListItem>
                                                 <asp:ListItem>Above Minimum Wages</asp:ListItem>
                                                 <asp:ListItem>Below Minimum Wages</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td  style="display:none">
                                            Month</td>
                                           <td style="display:none"> <asp:TextBox ID="txtmonth" runat="server" Text="" class="sinput"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                TargetControlID="txtmonth" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="txtmonth"
                                                ValidChars="/0123456789">
                                            </cc1:FilteredTextBoxExtender></td>
                                            <td>
                                            <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
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
                                <asp:GridView ID="GV_ExpensesExpenditure" runat="server" AutoGenerateColumns="False" Width="100%"
                                    Height="50px" CellPadding="5" CellSpacing="3" ForeColor="#333333" GridLines="None">
                                    <RowStyle BackColor="#EFF3FB" Height="30" />
                                    <Columns>
                                    
                                              <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            
                                        <asp:TemplateField HeaderText="Client ID/Name">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="Lbl_Date" Text="<%# Bind('ClientName') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                           
                                        <asp:TemplateField HeaderText="PayRate">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="Lbl_PayRate" Text="<%# Bind('Amount') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                               
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="Lbl_Qty" Text="<%# Bind('Quantity') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                          <asp:TemplateField HeaderText="Designation">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="Lbl_Designation" Text="<%# Bind('Design') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                          <asp:TemplateField HeaderText="Category">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="Lbl_Category" Text="<%# Bind('name') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                         <asp:TemplateField HeaderText="WAGES">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="Lbl_Wages" Text="<%# Bind('TotalMinimumWages') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  Height="30" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
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
       
        <!-- CONTENT AREA END -->
    </div>

</asp:Content>
