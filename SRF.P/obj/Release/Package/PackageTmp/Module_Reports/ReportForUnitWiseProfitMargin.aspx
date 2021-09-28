<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="ReportForUnitWiseProfitMargin.aspx.cs" Inherits="SRF.P.Module_Reports.ReportForUnitWiseProfitMargin" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
     <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function onCalendarShown() {

            var cal = $find("calendar1");
            //Setting the default mode to month
            cal._switchMode("months", true);

            //Iterate every month Item and attach click event to it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }

        function onCalendarHidden() {
            var cal = $find("calendar1");
            //Iterate every month Item and remove click event from it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }

        }

        function call(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar1");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }

    </script>


      <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                        <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                        <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                        <li class="active"><a href="#" style="z-index: 7;" class="active_bread">UnitWise Profit Margin Report</a></li>
                    </ul>
                </div>
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">UnitWise Profit Margin Report
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                    <div class="dashboard_firsthalf" style="width: 100%">
                                        <div align="right">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>
                                                    </td>
                                                    

                                                </tr>
                                            </table>
                                        </div>
                                        
                                         <div align="right">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="lbtn_ExportNew" runat="server" OnClick="lbtn_ExportNew_Click">Export to Excel(New)</asp:LinkButton>
                                                    </td>

                                                </tr>
                                            </table>
                                        </div>

                                        <table width="100%" cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td>Clientid :</td>

                                                <td>
                                                    <asp:DropDownList ID="ddlclientid" runat="server" class="sdrop" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>Name :</td>

                                                <td>
                                                    <asp:DropDownList ID="ddlcname" runat="server" class="sdrop" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlclientname_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>Month :</td>

                                                <td>
                                                    <asp:TextBox ID="txtmonth" Width="100px" runat="server" class="sinput" AutoComplete="Off"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server"
                                                        Enabled="true" Format="dd/MM/yyyy" TargetControlID="txtmonth"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="Txt_Month_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" TargetControlID="txtmonth"
                                                        ValidChars="/0123456789"></cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnsearch" runat="server" Text="Search" class="btn save" OnClick="btnsearch_Click" />
                                                </td>

                                                <td colspan="3" style="width: 30%">
                                                    <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"> </asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="rounded_corners" style="overflow: scroll">
                                        <asp:GridView ID="GVListEmployees" runat="server" ShowFooter="true" AutoGenerateColumns="False" Width="100%"
                                            CellSpacing="2" CellPadding="5" ForeColor="#333333" GridLines="None"
                                            OnPageIndexChanging="GVListEmployees_PageIndexChanging"
                                            OnRowDataBound="GVListEmployees_RowDataBound">
                                            <RowStyle BackColor="#EFF3FB" Height="30" />
                                            <Columns>

                                                <%-- 0--%>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>

                                                </asp:TemplateField>

                                                <%-- 1--%>
                                                <asp:TemplateField HeaderText="Client ID" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblclientid" runat="server" Text='<%#Bind("clientid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 2--%>

                                                <asp:TemplateField HeaderText="Unit Name" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblclientname" runat="server" Text='<%#Bind("clientname") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 3--%>
                                                <asp:TemplateField HeaderText="Manual Billing" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBilling" runat="server" Text='<%#Bind("Billing","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalBilling"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 4--%>
                                                <asp:TemplateField HeaderText="Arrear Billing" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblArrearBilling" runat="server" Text='<%#Bind("ArrearBilling","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalArrearBilling"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 5--%>
                                                <asp:TemplateField HeaderText="Bonus Billing" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBonusBilling" runat="server" Text='<%#Bind("BonusBilling","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalBonusBilling"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 6--%>
                                                <asp:TemplateField HeaderText="Elnh Billing" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblElnhBilling" runat="server" Text='<%#Bind("ElnhBilling","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalElnhBilling"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 7--%>
                                                <asp:TemplateField HeaderText="Basic" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBasic" runat="server" Text='<%#Bind("Basic","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalBasic"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 8--%>
                                                <asp:TemplateField HeaderText="ESIGross" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblESIGross" runat="server" Text='<%#Bind("ESIWages","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalESIGross"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 9--%>
                                                <asp:TemplateField HeaderText="Gross" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGross" runat="server" Text='<%#Bind("Gross","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalGross"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 10--%>
                                                <asp:TemplateField HeaderText="LA" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNFH" runat="server" Text='<%#Bind("LA","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNFH"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 11--%>
                                                <asp:TemplateField HeaderText="Uniform" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUniform" runat="server" Text='<%#Bind("UniformDed","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalUniform"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 12--%>
                                                <asp:TemplateField HeaderText="Gratuity" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWC" runat="server" Text='<%#Bind("Gratuity","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalWC"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 13--%>
                                                <asp:TemplateField HeaderText="Bonus" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBonus" runat="server" Text='<%#Bind("Bonus","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalBonus"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 14--%>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPF" runat="server" Text='<%#Bind("PFEmpr","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalPF"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 15--%>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblESI" runat="server" Text='<%#Bind("ESIEmpr","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalESI"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 16--%>
                                                <asp:TemplateField HeaderText="Total Expences" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalExpences" runat="server" Text='<%#Bind("TotalDeductions","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalTotalExpences"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 17--%>
                                                <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%#Bind("ActualAmount","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalAmount"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 18--%>
                                                <asp:TemplateField HeaderText="Age" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAge" runat="server" Text='<%#Bind("Age","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>

                                                </asp:TemplateField>

                                                <%-- 19--%>
                                                <asp:TemplateField HeaderText="Service Charge %" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblservicepeer" runat="server" Text='<%#Bind("ServiceChargeper","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalserviceper"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 20--%>
                                                <asp:TemplateField HeaderText="Service Charge Amount" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblservicechargeamt" runat="server" Text='<%#Bind("ServiceCharge","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalservicechargeamt"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 21--%>
                                                <asp:TemplateField HeaderText="PreviousMonth" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPreviousMonth" runat="server" Text='<%#Bind("PreviousMonth","{0:0}") %>'> </asp:Label>
                                                    </ItemTemplate>

                                                </asp:TemplateField>

                                                <%-- 22--%>
                                                <asp:TemplateField HeaderText="Diff" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDiff" runat="server" Text='<%#Bind("Diff","{0:0}") %>'> </asp:Label>
                                                    </ItemTemplate>

                                                </asp:TemplateField>

                                                <%-- 23--%>
                                                <asp:TemplateField HeaderText="Remarks" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Bind("Remarks","{0:0}") %>'> </asp:Label>
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
