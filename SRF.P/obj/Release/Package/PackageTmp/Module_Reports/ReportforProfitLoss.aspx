<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="ReportforProfitLoss.aspx.cs" Inherits="SRF.P.Module_Reports.ReportforProfitLoss" %>
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
                    <li><a href="ManagementReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Profit Margin</a></li>
                </ul>
            </div>
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Profit Margin
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
                                   
                                        <table width="100%" cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td>
                                                    Clientid :</td>
                                                    
                                                    <td><asp:DropDownList ID="ddlclientid" runat="server" class="sdrop" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Name :</td>
                                                    
                                                    <td><asp:DropDownList ID="ddlcname" runat="server" class="sdrop" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlclientname_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Month :</td>
                                                    
                                                    <td><asp:TextBox ID="txtmonth" runat="server" class="sinput" > </asp:TextBox>
                                                        <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server"  BehaviorID="calendar1"
                                                            Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden"  OnClientShown="onCalendarShown">
                                                        </cc1:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnsearch" runat="server" Text="Search" class="btn save" OnClick="btnsearch_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        </div>
                                  <div class="rounded_corners">
                                    <div style="overflow: scroll; width: 100%">
                                        <asp:GridView ID="gvreport" runat="server" AutoGenerateColumns="False" 
                                    CellSpacing="3" CellPadding="4" GridLines="None"
                                            EmptyDataRowStyle-BackColor="BlueViolet" EmptyDataRowStyle-BorderColor="Aquamarine"
                                            EmptyDataRowStyle-Font-Italic="true" EmptyDataText="No Records Found" ForeColor="#333333"
                                            Width="170%" Height="50%" OnPageIndexChanging="gvreport_PageIndexChanging">
                                            <RowStyle BackColor="#EFF3FB" Height="30" />
                                            <EmptyDataRowStyle BackColor="SkyBlue" BorderColor="Aquamarine" Font-Italic="True" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Client Id" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblclientid" runat="server" Text='<%#Bind("clientid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblclientname" runat="server" Text='<%#Bind("clientname") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Bill No" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <%--     <asp:Label ID="lblbillno" runat="server" Text="<%#Bind('billno') %>"></asp:Label>--%>
                                                        <asp:Label ID="lblbillno" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Invoice G.Total" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <%--     <asp:Label ID="lblinvoice" runat="server" Text="<%#Bind('Grandtotal','{0:0.00}') %>"></asp:Label>--%>
                                                        <asp:Label ID="lblinvoice" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="S.T Amount" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                                    <ItemTemplate>
                                                        <%--      <asp:Label ID="lblservicetaxamt" runat="server" Text="<%#Bind('servicetax','{0:0.00}') %>"></asp:Label>--%>
                                                        <asp:Label ID="lblservicetaxamt" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Net Total" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                                    <ItemTemplate>
                                                        <%--  <asp:Label ID="lblNet" runat="server" Text="<%#Bind('Nettotal','{0:0.00}') %>"></asp:Label>--%>
                                                        <asp:Label ID="lblNet" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Gross" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                                    <ItemTemplate>
                                                        <%--   <asp:Label ID="lblgross" runat="server" Text='<%#Eval("empgross","{0:0.00}") %>'></asp:Label>--%>
                                                        <asp:Label ID="lblgross" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--    <asp:TemplateField HeaderText="PF" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpf" runat="server" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="PF Empr" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                                    <ItemTemplate>
                                                        <%--  <asp:Label ID="lblpfemployer" runat="server" Text='<%#Bind("pfempr","{0:0.00}") %>'></asp:Label>--%>
                                                        <asp:Label ID="lblpfemployer" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--         <asp:TemplateField HeaderText="Total PF" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                  <asp:Label ID="lbltotalpf" runat="server" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        --%>
                                                <%--    <asp:TemplateField HeaderText="ESI" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblesi" runat="server" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="ESI Empr" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblesiemployer" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--  <asp:TemplateField HeaderText="Total ESI" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                 <asp:Label ID="lbltotalesi" runat="server" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                                <%--    <asp:TemplateField HeaderText="P.T" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpt" runat="server" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                                <%--<asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                 <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Total Expenses" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="4%">
                                                    <ItemTemplate>
                                                        <%-- <asp:Label ID="lblTotal" runat="server" Text='<%#Bind("Total","{0:0.00}") %>'></asp:Label>--%>
                                                        <asp:Label ID="lblTotalexpensives" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Profit/Loss" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="4%">
                                                    <ItemTemplate>
                                                        <%-- <asp:Label ID="lblTotal" runat="server" Text='<%#Bind("Total","{0:0.00}") %>'></asp:Label>--%>
                                                        <asp:Label ID="lblTotalprofitorloss" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText=" Total Expenses(%)" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="4%">
                                                    <ItemTemplate>
                                                        <%-- <asp:Label ID="lblTotal" runat="server" Text='<%#Bind("Total","{0:0.00}") %>'></asp:Label>--%>
                                                        <asp:Label ID="lblTotalexpesivesperc" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Profit(%)" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="4%">
                                                    <ItemTemplate>
                                                        <%-- <asp:Label ID="lblTotal" runat="server" Text='<%#Bind("Total","{0:0.00}") %>'></asp:Label>--%>
                                                        <asp:Label ID="lblTotalprofit" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--     <asp:TemplateField HeaderText="Perc" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPerc" runat="server" Text=""></asp:Label>
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
        <!-- DASHBOARD CONTENT END -->
 
    </div>

</asp:Content>