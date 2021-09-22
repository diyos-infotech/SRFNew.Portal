<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="ImportPANNo.aspx.cs" Inherits="SRF.P.Module_Reports.ImportPANNo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link href="../css/global.css" rel="stylesheet" type="text/css" />
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
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                     <li><a href="Imports.aspx" style="z-index: 8;">Imports</a></li>
                    <li class="active"><a href="EmpDueAmount.aspx" style="z-index: 7;" class="active_bread">Import PanCard NOs</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Import PanCard NOs
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div id="right_content_area" style="text-align: left; font: Tahoma; font-size: small; font-weight: normal">
                                    <div align="right">
                                        <asp:LinkButton ID="lnkSample" runat="server" OnClick="lnkSample_Click">Export Sample Sheet</asp:LinkButton>
                                    </div>
                                    <div class="dashboard_firsthalf" style="width: 100%">
                                        <table width="60%" cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td style="width: 80px">Select File: </td>
                                                <td>
                                                    <asp:FileUpload ID="FlUploadAadhaarNo" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="BtnSave" runat="server" Text="Import Data" class="btn save" OnClick="BtnSave_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="BtnUnSave" runat="server" Text="Export UnSaved Data" Visible="false"
                                                        class="btn unsave" OnClick="BtnUnSave_Click" />
                                                </td>

                                            </tr>
                                        </table>

                                        <asp:GridView ID="GvNotInsertedlist" runat="server" AutoGenerateColumns="False" Width="90%" Visible="false"
                                            ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center; margin: 0px auto; margin-top: 10px;">
                                            <RowStyle BackColor="#EFF3FB" Height="30" />
                                            <Columns>

                                                <asp:TemplateField HeaderText="Emp ID">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempid" Text=' <%#Eval("EmpID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PanCard No">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPanCardNo" Text='<%#Eval("PanCardNo","{0:X}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblRemarks" Text='<%#Eval("Remark")%>'></asp:Label>
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

                                        <asp:GridView ID="gvlistofemp" runat="server" AutoGenerateColumns="False" Width="90%" Visible="false"
                                            ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center">
                                            <RowStyle BackColor="#EFF3FB" Height="30" />
                                            <Columns>

                                                <asp:TemplateField HeaderText="Emp ID">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempid" Text=' <%#Eval("EmpID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PanCard No">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPanCardNo" Text='<%#Eval("PanCardNo","{0:x4}")%>'></asp:Label>
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
                                    <br />

                                    <div style="margin-top: 20px">
                                        <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"></asp:Label>
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

            <!-- CONTENT AREA END -->
        </div>
    </div>
</asp:Content>
