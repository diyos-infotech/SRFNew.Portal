<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="WageSheetReports.aspx.cs" Inherits="SRF.P.Module_Reports.WageSheetReports" %>
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
                        <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                        <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                        <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Wage Sheet</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">Wage Sheet
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">

                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                    <div align="right">
                                        <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="false">Export to Excel</asp:LinkButton>
                                    </div>
                                    <div class="dashboard_firsthalf" style="width: 100%">

                                        <table width="100%" cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td>Type
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddltype" runat="server" AutoPostBack="true" CssClass="sdrop" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                                        <asp:ListItem>Month</asp:ListItem>
                                                        <asp:ListItem>From-To</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">Client Id
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlclientid" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged" class="sdrop">
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="5%"></td>
                                                <td>Name
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged" class="sdrop">
                                                    </asp:DropDownList>
                                                </td>
                                                <td runat="server" id="lblmonth">Month
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtmonth" runat="server" Text="" AutoComplete="off" class="sinput"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                        TargetControlID="txtmonth" Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="txtmonth"
                                                        ValidChars="/0123456789"></cc1:FilteredTextBoxExtender>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblfrom" runat="server" Text="From Date" Visible="false" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtfromdate" runat="server" Text="" AutoComplete="off" class="sinput" Visible="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true"
                                                        TargetControlID="txtfromdate" Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="txtfromdate"
                                                        ValidChars="/0123456789"></cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td></td>
                                                <td>
                                                    <asp:Label ID="lbltodate" runat="server" Text="To Date" Visible="false" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txttodate" runat="server" Text="" AutoComplete="off" class="sinput" Visible="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true"
                                                        TargetControlID="txttodate" Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="txttodate"
                                                        ValidChars="/0123456789"></cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="7" align="right">
                                                    <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="width: 30%">
                                                    <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"> </asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="rounded_corners" style="overflow: scroll">
                                        <asp:GridView ID="GVListEmployees" runat="server" ShowFooter="true" AutoGenerateColumns="False" Width="100%"
                                            CellSpacing="2" CellPadding="5" ForeColor="#333333" GridLines="None"
                                            OnRowDataBound="GVListEmployees_RowDataBound">
                                            <RowStyle BackColor="#EFF3FB" Height="30" />
                                            <Columns>
                                                <%-- 0--%>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 1--%>
                                                <asp:TemplateField HeaderText="Client ID" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblclientid" runat="server" Text='<%#Bind("clientid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 2--%>

                                                <asp:TemplateField HeaderText="Client Name" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblclientname" runat="server" Text='<%#Bind("clientname") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <%-- 3--%>
                                                <asp:TemplateField HeaderText="Emp Id" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblempid" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 4--%>
                                                <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblempname" runat="server" Text='<%#Bind("EmpMname") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <%-- 5--%>
                                                <asp:TemplateField HeaderText="Desgn" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldesgn" runat="server" Text='<%#Bind("Desgn") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 6--%>
                                                <asp:TemplateField HeaderText="Month-Year" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblmonth" runat="server" Text='<%#Bind("Monthname") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 7--%>
                                                <asp:TemplateField HeaderText="Duties" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldutyhrs" runat="server" Text='<%#Bind("NoOfDuties") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalDuties" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 8--%>
                                                <asp:TemplateField HeaderText="OTs" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOts" runat="server" Text='<%#Bind("OTs") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalOTs"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 9--%>
                                                <asp:TemplateField HeaderText="WO" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblwos" runat="server" Text='<%#Bind("WO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalwos"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 10--%>
                                                <asp:TemplateField HeaderText="Nhs" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNhs" runat="server" Text='<%#Bind("NHS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNhs"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 11--%>
                                                <asp:TemplateField HeaderText="Npots" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNpots" runat="server" Text='<%#Bind("npots") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNpots"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 12--%>
                                                <asp:TemplateField HeaderText="Sal Rate" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltempgross" runat="server" Text='<%#Bind("TempGross") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotaltempgross"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>




                                                <%-- 13--%>

                                                <asp:TemplateField HeaderText="Basic" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%-- <asp:Label ID="lblbasic" runat="server" Text='<%#Bind("basic") %>'>--%>
                                                        <asp:Label ID="lblbasic" runat="server" Text='<%#Eval("basic", "{0:0}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalBasic"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 14--%>
                                                <asp:TemplateField HeaderText="DA" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblda" runat="server" Text='<%#Eval("da","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalDA"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 15--%>
                                                <asp:TemplateField HeaderText="HRA" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblhra" runat="server" Text='<%#Bind("hra","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalHRA"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 16--%>
                                                <asp:TemplateField HeaderText="CCA" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcca" runat="server" Text='<%#Bind("CCa","{0:0}") %>'>  
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalCCA"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 17--%>
                                                <asp:TemplateField HeaderText="Conv" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblConveyance" runat="server" Text='<%#Bind("conveyance","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalConveyance"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 18--%>
                                                <asp:TemplateField HeaderText="W.A." ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblwashallowance" runat="server" Text='<%#Bind("WashAllowance","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalWA"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 19--%>
                                                <asp:TemplateField HeaderText="O.A." ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOtherallowance" runat="server" Text='<%#Bind("OtherAllowance","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalOA"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 20--%>
                                                <asp:TemplateField HeaderText="L.W" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLeaveEncashAmt" runat="server" Text='<%#Bind("LeaveEncashAmt","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalLeaveEncashAmt"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 21--%>
                                                <asp:TemplateField HeaderText="Gratuity" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGratuity" runat="server" Text='<%#Bind("Gratuity","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalGratuity"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 22--%>
                                                <asp:TemplateField HeaderText="Bonus" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBonus" runat="server" Text='<%#Bind("Bonus","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalBonus"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 23--%>
                                                <asp:TemplateField HeaderText="Nfhs" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNfhs" runat="server" Text='<%#Bind("Nfhs","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNfhs"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 24--%>
                                                <asp:TemplateField HeaderText="RC" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblrc" runat="server" Text='<%#Bind("rc","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalrc"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 25--%>
                                                <asp:TemplateField HeaderText="CS" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcs" runat="server" Text='<%#Bind("cs","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalcs"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                <%-- 26--%>

                                                <asp:TemplateField HeaderText="Incentivs" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIncentivs" runat="server" Text='<%#Bind("Incentivs","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalIncentivs"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 27--%>

                                                <asp:TemplateField HeaderText="WO Amt" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWoAmt" runat="server" Text='<%#Bind("WOAmt","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalWOAmount"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 28--%>

                                                <asp:TemplateField HeaderText="NHs Amt" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNhsAmt" runat="server" Text='<%#Bind("Nhsamt","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNhsAmount"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 29--%>

                                                <asp:TemplateField HeaderText="NPOTs Amt" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNpotsAmt" runat="server" Text='<%#Bind("Npotsamt","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNpotsAmount"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>



                                                <%-- 30--%>
                                                <asp:TemplateField HeaderText="Gross" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGross" runat="server" Text='<%#Bind("Gross","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalGross"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                <%-- 31--%>

                                                <asp:TemplateField HeaderText="OT Amt" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOTAmt" runat="server" Text='<%#Bind("OTAmt","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalOTAmount"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>



                                                <%-- 32--%>
                                                <asp:TemplateField HeaderText="PF" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPF" runat="server" Text='<%#Bind("PF","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalPF"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 33--%>
                                                <asp:TemplateField HeaderText="ESI" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblESI" runat="server" Text='<%#Bind("ESI","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalESI"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 34--%>
                                                <asp:TemplateField HeaderText="ProfTax" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProfTax" runat="server" Text='<%#Bind("ProfTax","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalProfTax"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 35--%>
                                                <asp:TemplateField HeaderText="Sal.Adv" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsaladv" runat="server" Text='<%#Bind("SalAdvDed","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalsaladv"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 36--%>
                                                <asp:TemplateField HeaderText="Uniform" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbluniform" runat="server" Text='<%#Bind("UniformDed","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: justify">
                                                            <asp:Label runat="server" ID="lblTotalUniformDed"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>



                                                <%-- 37--%>
                                                <asp:TemplateField HeaderText="Other Ded" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOtherDed" runat="server" Text='<%#Bind("OtherDed","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: justify">
                                                            <asp:Label runat="server" ID="lblTotalOtherDed"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 38--%>
                                                <asp:TemplateField HeaderText="Room Rent" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRoomRentDed" runat="server" Text='<%#Bind("RoomRentDed","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: justify">
                                                            <asp:Label runat="server" ID="lblTotalRoomRentDed"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 39--%>
                                                <asp:TemplateField HeaderText="Mess Ded" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcantadv" runat="server" Text='<%#Bind("CanteenAdv","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: justify">
                                                            <asp:Label runat="server" ID="lblTotalcantadv"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 40--%>
                                                <asp:TemplateField HeaderText="Sec Dep" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSecDepDed" runat="server" Text='<%#Bind("SecurityDepDed","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: justify">
                                                            <asp:Label runat="server" ID="lblTotalSecDepDed"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 41--%>
                                                <asp:TemplateField HeaderText="Gen Ded" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGeneralDed" runat="server" Text='<%#Bind("GeneralDed","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: justify">
                                                            <asp:Label runat="server" ID="lblTotalGeneralDed"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>



                                                <%-- 42--%>
                                                <asp:TemplateField HeaderText="LWF" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblowf" runat="server" Text='<%#Bind("OWF","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: justify">
                                                            <asp:Label runat="server" ID="lblTotalowf"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 43--%>
                                                <asp:TemplateField HeaderText="Rent" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPenalty" runat="server" Text='<%#Bind("Penalty","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: justify">
                                                            <asp:Label runat="server" ID="lblTotalPenalty"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>



                                                <%-- 44--%>
                                                <asp:TemplateField HeaderText="Total Ded" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDeductions" runat="server" Text='<%#Bind("TotalDeductions","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalDeductions"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 45--%>
                                                <asp:TemplateField HeaderText="Net Amt" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblnetamount" runat="server" Text='<%#Bind("ActualAmount","{0:0}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNetAmount" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                 <%-- 46--%>
                                                <asp:TemplateField HeaderText="Bank A/c No" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpBankAcNo" runat="server" Text='<%#Bind("EmpBankAcNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <%-- 47--%>
                                                <asp:TemplateField HeaderText="bank Name" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpbankname" runat="server" Text='<%#Bind("Empbankname") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <%-- 48--%>
                                                <asp:TemplateField HeaderText="IFSC Code" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpIFSCcode" runat="server" Text='<%#Bind("EmpIFSCcode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <%-- 49--%>
                                                <asp:TemplateField HeaderText="Reference" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReference" runat="server" Text='<%#Bind("Reference") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <%-- 50--%>
                                                <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLocation" runat="server" Text='<%#Bind("ClientLocation") %>'></asp:Label>
                                                    </ItemTemplate>
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

                                    <div class="rounded_corners" style="overflow: scroll">
                                        <asp:GridView ID="GVListEmployeesFromTo" runat="server"  AutoGenerateColumns="true" Width="100%"
                                            CellSpacing="2" CellPadding="5" ForeColor="#333333" GridLines="None"
                                            OnRowDataBound="GVListEmployeesFromTo_RowDataBound">
                                            <RowStyle BackColor="#EFF3FB" Height="30" />

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
