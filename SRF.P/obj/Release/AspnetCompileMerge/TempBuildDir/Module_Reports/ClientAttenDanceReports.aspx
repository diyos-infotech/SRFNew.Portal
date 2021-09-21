<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="ClientAttenDanceReports.aspx.cs" Inherits="SRF.P.Module_Reports.ClientAttenDanceReports" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

     <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Load.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 135px;
        }
    </style>

     <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Attendance</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Attendance
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                        <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                        </asp:ScriptManager>
                        <%--   
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                     --%>
                        <div class="dashboard_firsthalf" style="width: 100%">
                        
                            <div align="right">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="lbtn_ExportPdf" runat="server" OnClick="lbtn_ExportPdf_Click">Export to PDF</asp:LinkButton><br />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            
                           
                                <table width="100%" cellpadding="5" cellspacing="5">
                                    <tr>
                                       <td>
                                            <asp:Label ID="lblClientId" runat="server" Text="Client ID "> </asp:Label><span style="color: Red">*</span></td>
                                           <td> <asp:DropDownList runat="server" AutoPostBack="true" class="sdrop" ID="ddlClientID" OnSelectedIndexChanged="ddlClientID_SelectedIndexChanged">
                                                <asp:ListItem>--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblclientname" runat="server" Text="Name"> </asp:Label><span style="color: Red">*</span></td>
                                          <td>  <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlclientname"  class="sdrop"
                                                OnSelectedIndexChanged="ddlclientname_OnSelectedIndexChanged">
                                                <asp:ListItem>--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        <asp:DropDownList runat="server" ID="ddlAttendanceType" class="sdrop" Width="100px">
                                            <asp:ListItem>Paysheet</asp:ListItem>
                                            <asp:ListItem>Billing</asp:ListItem>
                                        </asp:DropDownList>
                                        </td>
                                        <td>
                                        <asp:Label runat="server" Text="All" ID="lblAllattendance"></asp:Label>
                                        <asp:CheckBox runat="server" ID="ChkAllattendance" Height="20" Width="20"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMonth" runat="server" Text="Month"> </asp:Label></td>
                                           <td> <asp:TextBox ID="txtMonth" runat="server" class="sinput" Width="100px"></asp:TextBox><span style="color: Red">*</span>
                                            <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                TargetControlID="txtMonth" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="FTBEMonth" runat="server" Enabled="True" TargetControlID="txtMonth"
                                                ValidChars="/0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="btnSubmit" Text="Submit" class="btn save" OnClick="btnSubmit_Click" /><br />
                                            <asp:Label ID="LblResult" runat="server" Visible="false" Style="color: Red"> </asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="rounded_corners">
                                <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="100%"
                                   CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None" OnRowDataBound="GVListEmployees_RowDataBound"
                                    ShowFooter="true">
                                    <RowStyle BackColor="#EFF3FB" Height="30" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ClientId">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblClientId" Text="<%#Bind('Clientid') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ClientName">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblClientName" Text="<%#Bind('ClientName') %>">"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ContractId">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblContractId" Text="<%#Bind('ContractId') %>"></asp:Label>
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Emp Id">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpId" Text="<%# Bind('EmpID') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpFirstName" Text="<%# Bind('name') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Designation">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDesignation" Text="<%# Bind('Design') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="No. Of Duties">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblnoofduties" Text="<%# Bind('NoOfDuties') %>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalNoOfDutied" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OTs">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblots" Text="<%# Bind('ot') %>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalOTs" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nhs">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblNhs" Text="<%# Bind('NHS') %>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalNHS" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Npots">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblNpots" Text="<%# Bind('Npots') %>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalNpots" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TotalDuties">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTotalDuties" Text="<%#Bind('TotalDuties') %>"></asp:Label>
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30"/>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                                
                                 <asp:GridView ID="GVListClients" runat="server" AutoGenerateColumns="False" Width="100%" 
                                 OnRowDataBound="GVListClients_RowDataBound" ShowFooter="true"
                                   CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None" >
                                    <RowStyle BackColor="#EFF3FB" Height="30" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ClientId">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpId" Text="<%# Bind('ClientId') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ClientName">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpFirstName" Text="<%# Bind('ClientName') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Designation">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDesignation" Text="<%# Bind('Design') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="No. Of Duties">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblnoofduties" Text="<%# Bind('NoOfDuties') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FortheMonthOf">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblots" Text="<%# Bind('FortheMonthOf') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ContractId">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblContractId" Text="<%#Bind('ContractId') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30"/>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </div>
                      
                        <%--    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                        <ProgressTemplate>
                                            <div id="Background">
                                            </div>
                                            <div id="Progress">
                                                <img src="css/assets/loadimage.gif"  alt="" width="40" style="vertical-align:middle;" />
                                                Loading Please Wait...
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                         </ContentTemplate>
                    </asp:UpdatePanel>--%>
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
