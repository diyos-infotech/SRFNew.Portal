<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="IDCardExpireDetails.aspx.cs" Inherits="SRF.P.Module_Reports.IDCardExpireDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

     <link href="css/global.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2 {
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
                        <li class="first"><a href="#" style="z-index: 8;"><span></span>Reports</a></li>
                       <%-- <li><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>--%>
                        <li class="active"><a href="#" style="z-index: 7;" class="active_bread">ID Card Report</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">ID Card Report
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                                    <div align="right">
                                                        <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>
                                                    </div>
        
                                    
                            
                                        <asp:GridView ID="dgLicExpire" runat="server" AllowPaging="True" 
                           EmptyDataRowStyle-BackColor="BlueViolet" PageSize="15"
                           EmptyDataRowStyle-BorderColor="Aquamarine" EmptyDataRowStyle-Font-Italic="true" 
                           EmptyDataText="No Records Found" GridLines="None" Height="97px" 
                           CellPadding="5" CellSpacing="3" Width="100%" 
                           ForeColor="#333333" 
                                        onpageindexchanging="dgLicExpire_PageIndexChanging">
                           <RowStyle HorizontalAlign="Center" BackColor="#EFF3FB" Height="30"/>
                           <EmptyDataRowStyle BackColor="SkyBlue" BorderColor="Aquamarine" 
                               Font-Italic="True" />

                           <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" HorizontalAlign="Center" 
                                    BorderWidth="1px" CssClass = "GridPager"/>
                           <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                           <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"   Height="30" />
                           <EditRowStyle CssClass="row" BackColor="#2461BF" />
                           <AlternatingRowStyle CssClass="altrow" BackColor="White" />
                       </asp:GridView>
                                    <asp:Label ID="LblResult" runat="server" Text="" style=" color:red"></asp:Label>
                                   

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
