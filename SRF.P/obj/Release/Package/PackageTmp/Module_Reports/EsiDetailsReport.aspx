<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="EsiDetailsReport.aspx.cs" Inherits="SRF.P.Module_Reports.EsiDetailsReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

      <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Load.css" rel="stylesheet" type="text/css" />


        <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">ESI Upload</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                ESI Upload
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <div align="right">
                                        <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>
                                        <asp:LinkButton ID="lbtn_Export_PDF" runat="server" OnClick="lbtn_Export_PDF_Click">Export to PDF</asp:LinkButton>
                                   
                                        
                                         </div>
                                    <table width="50%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>
                                              <%--  Client Id :<span style="color: Red">*</span>--%>
                                              
                                            <%--  ESI Branch :<span style="color: Red">*</span>--%>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlclient" runat="server" AutoPostBack="true" class="sdrop"
                                                    OnSelectedIndexChanged="ddlclient_SelectedIndexChanged" Visible="false">
                                                </asp:DropDownList>
                                                
                                                <asp:DropDownList ID="ddlEsibranch" runat="server" AutoPostBack="true" class="sdrop" Visible="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                               <%-- Client Name :<span style="color: Red">*</span>--%>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlcname" runat="server" class="sdrop" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged" Visible="false">
                                                </asp:DropDownList>
                                                &nbsp;
                                            </td>
                                            <td>
                                                Month :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtmonth" runat="server" Text="" class="sinput"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                    TargetControlID="txtmonth" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="txtmonth"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btn_Submit_Click" />
                                            </td>
                                        </tr>
                                        <%--  <tr>
                          <td>--%>
                                    </table>
                                    </div>
                                    
                                    <asp:CheckBox ID="Chkmonth" runat="server" Text="&nbsp;From August" Checked="false" Visible="false" />
                                    <div class="rounded_corners">
                                        <div style="overflow: scroll;width: auto">
                                            <asp:GridView ID="GVListOfClients" runat="server" AutoGenerateColumns="False" Width="100%"
                                                CssClass="datagrid" CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>


                                                       <asp:TemplateField HeaderText="Emp ID">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblEmpid" Text="<%# Bind('empid') %> "></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    

                                                     <asp:TemplateField HeaderText="IP Number">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblIpNumber" Text="<%# Bind('EmpESINo') %> "></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    
                                                    <asp:TemplateField HeaderText="IP Name">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblIPName" Text="<%# Bind('Fullname') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                 
                                                   
                                                    <asp:TemplateField HeaderText="No of Days for which wages paid/payable during the month">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblNoofduties" Text="<%# Bind('NoOfDuties') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   
<%--                                                    <asp:TemplateField HeaderText="ESI Wages">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblEsiwages" Text="<%# Bind('ESIWAGES') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Empr Esi">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempresi" Text="<%# Bind('Esiempr') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Emp Esi">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempesi" Text="<%# Bind('esiemp') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>

                                                    <asp:TemplateField HeaderText="Total Monthly Wages">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbltotalmonthlyesi" Text="<%# Bind('ESIWAGES') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>



                                                    <asp:TemplateField HeaderText="Reason Code for Zero working days">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblReason" Text=" "></asp:Label></ItemTemplate>
                                                    </asp:TemplateField>

                                                    
                                                    <asp:TemplateField HeaderText="Last Working Day">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblLastWorkingDay" Text=" "></asp:Label></ItemTemplate>
                                                    </asp:TemplateField>


                                                    
                                                   <%-- <asp:BoundField DataField="EsiBranchname" HeaderText="ESI Cutting Branch" />--%>

                                                   <%-- <asp:TemplateField HeaderText="Extra1">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblPF1" Text=" "></asp:Label></ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Extra2">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblPFEmpr" Text=" "></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Extra3">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblPFEmpr1" Text=" "></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>

                                                  
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                            <asp:Label ID="LblResult" runat="server" Text="" Style="color: red"></asp:Label>
                                        </div>
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
