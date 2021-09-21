<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="BillsVsActuals.aspx.cs" Inherits="SRF.P.Module_Reports.BillsVsActuals" %>
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
                        <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Bills Vs Actuals</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">Bills Vs Actuals
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">

                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>

                                    <div class="dashboard_firsthalf" style="width: 100%">

                                        <table width="100%" cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td>Client ID
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlclientid" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged" class="sdrop">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>Name
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged" class="sdrop">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>Month
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtmonth" runat="server" Text="" class="sinput"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                        Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                                </td>
                                                <td>
                                                    <div align="right">
                                                        <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="False">Export to Excel</asp:LinkButton>
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
                                    <div class="rounded_corners" style="overflow: scroll">
                                        <asp:GridView ID="GVBillsVsActuals" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                            CellSpacing="3" CellPadding="5" ForeColor="#333333" GridLines="None" ShowFooter="true" OnRowDataBound="GVBillsVsActuals_RowDataBound">
                                            <Columns>

                                                <%-- 1--%>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>



                                                <%-- 2--%>
                                                <asp:TemplateField HeaderText="Client ID" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblclientid" runat="server" Text='<%#Bind("clientid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 3--%>

                                                <asp:TemplateField HeaderText="Client Name" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblclientname" runat="server" Text='<%#Bind("clientname") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField HeaderText="Actual Billing Duties" DataField="ActualBillDuties" DataFormatString="{0:0.00}" />
                                                <asp:BoundField HeaderText="Billed Duties" DataField="BillDuties" DataFormatString="{0:0.00}" />
                                                <asp:BoundField HeaderText="Difference (Duties)" DataField="DiffDuties" DataFormatString="{0:0.00}" />



                                                <asp:BoundField HeaderText="Actual Billing Amount" DataField="ActualBillAmt" DataFormatString="{0:0.00}" />
                                                <asp:BoundField HeaderText="Billed Amount" DataField="BillAmt" DataFormatString="{0:0.00}" />
                                                <asp:BoundField HeaderText="Difference (Amount)" DataField="Difference" DataFormatString="{0:0.00}" />



                                            </Columns>

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
