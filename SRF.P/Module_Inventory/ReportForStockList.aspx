<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="ReportForStockList.aspx.cs" Inherits="SRF.P.Module_Inventory.ReportForStockList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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
                    <li class="first"><a href="ViewItems.aspx" style="z-index: 8;"><span></span>Reports</a></li>
                    <%-- <li><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>--%>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Stock List</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Stock List
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <div align="right">
                                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>


                                            </div>
                                        </td>
                                    </tr>
                                </table>


                                <%--  <asp:HiddenField ID="hidGridView" runat="server" />--%>
                                <asp:GridView ID="GVPODetails" runat="server" AutoGenerateColumns="False"
                                    EmptyDataText="No Records Found" Width="100%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                    CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None" OnRowDataBound="GVPODetails_RowDataBound" ShowFooter="true">


                                    <Columns>


                                        <%-- 0--%>
                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="5px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <%-- 1--%>
                                        <asp:TemplateField HeaderText="Item Id" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px">
                                            <HeaderStyle Width="15px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblitemid" runat="server" Text='<%#Bind("Itemid") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%-- 2 --%>

                                        <asp:TemplateField HeaderText="Item Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="180px">
                                            <HeaderStyle Width="15px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblitemname" runat="server" Text='<%#Bind("itemname") %>'> </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <%-- 3--%>
                                        <asp:TemplateField HeaderText="Stock In Hand" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="25px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblStock" runat="server" Text='<%#Bind("ActualQuantity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lbltotalStock" Style="font-weight: bold; text-align: right"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>


                                    </Columns>


                                </asp:GridView>


                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <!-- DASHBOARD CONTENT END -->

        <!-- CONTENT AREA END -->
    </div>


</asp:Content>
