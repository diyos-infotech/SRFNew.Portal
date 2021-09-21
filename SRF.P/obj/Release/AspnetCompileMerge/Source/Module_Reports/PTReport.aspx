<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="PTReport.aspx.cs" Inherits="SRF.P.Module_Reports.PTReport" %>
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
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">PT Report</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                             PT Report
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                            
                        <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                        </asp:ScriptManager>
                        
                        <div class="dashboard_firsthalf" style="width: 100%">
                            
                            <table width="60%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td>
                                            <%--Client Id--%>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlclientid" runat="server" AutoPostBack="True"
                                             OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged" class="sdrop" Visible="false" >
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                           <%-- Name--%>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged" class="sdrop"  Visible="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Month
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
                                            <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                        </td>
                                        <td>
                                            <div align="right">
                                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="False">Export to Excel</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="lbtn_Export_PDF" runat="server" OnClick="lbtn_Export_PDF_Click" Visible="False"><%--Export to PDF--%></asp:LinkButton>

                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 30%">
                                            <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"> </asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                               <div class="rounded_corners" style="  width:950px">
                                <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="100%" EmptyDataRowStyle-BackColor="BlueViolet"
                                    EmptyDataRowStyle-BorderColor="Aquamarine" EmptyDataText=""  CellPadding="4" ForeColor="#333333" CellSpacing="3" GridLines="None" ShowFooter="True"
                                    onrowdatabound="GVListEmployees_RowDataBound">
                                    <RowStyle BackColor="#EFF3FB"  Height="30"/>
                                        <EmptyDataRowStyle BackColor="LightSkyBlue" BorderColor="Aquamarine" Font-Italic="false"
                                            Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="SNo" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                             <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Emp Id" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblempid" runat="server" Text='<%#Bind("empid")%>'></asp:Label>
                                        </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Emp Name" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblempname" runat="server" Text='<%#Bind("Empname")%>'></asp:Label>
                                        </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="PT Gross" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                         
                                            <asp:Label ID="lblgross" runat="server" Text='<%#Bind("gross") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalgross" runat="server" ></asp:Label>
                                        </FooterTemplate>
                                        </asp:TemplateField>
                                        

                                    <asp:TemplateField HeaderText="PT Deducted" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPT" runat="server" Text='<%#Bind("ProfTax") %>'></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                            <asp:Label ID="lblTotalPT" runat="server" ></asp:Label>
                                        </FooterTemplate>
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
