<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="CreditNoteReport.aspx.cs" Inherits="SRF.P.Module_Reports.CreditNoteReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>

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
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Credit Note Report</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <asp:ScriptManager runat="server" ID="Scriptmanager1"></asp:ScriptManager>
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Credit Note Report
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <div align="right">
                                        <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="False" OnClientClick="AssignExportHTML()">Export to Excel</asp:LinkButton>
                                    </div>

                                    <table width="80%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>Client ID
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" class="sdrop" ID="ddlClientId" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlClientId_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="padding-left: 80px">Client Name
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="true" class="sdrop" Width="300px"
                                                    OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>GSTIN/UIN</td>
                                            <td>
                                                <asp:DropDownList ID="ddlOurGSTIN" runat="server" class="sdrop">
                                                </asp:DropDownList>
                                            </td>
                                            <td colspan="2"></td>
                                        </tr>
                                        <tr>
                                            <td>Bill Type
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlbilltype" runat="server" class="sdrop">
                                                    <asp:ListItem>All</asp:ListItem>
                                                    <asp:ListItem>Software</asp:ListItem>
                                                    <asp:ListItem>Manual</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="padding-left: 80px">Period  
                                                <%--Invoice Type:--%>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPeriod" runat="server" class="sdrop" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem>From-To</asp:ListItem>
                                                    <asp:ListItem>Month</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="visibility: hidden">
                                                <asp:DropDownList ID="ddlinvoicetype" runat="server" class="sdrop">
                                                    <asp:ListItem>All</asp:ListItem>
                                                    <asp:ListItem>With</asp:ListItem>
                                                    <asp:ListItem>With Out</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblfromdate" runat="server" Text="From Date :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txt_From_Date" runat="server" class="sinput"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CE_From_Date" runat="server" Enabled="True" TargetControlID="Txt_From_Date"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBE_From_Date" runat="server" Enabled="True" TargetControlID="Txt_From_Date"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>

                                            </td>
                                            <td style="padding-left: 80px">
                                                <asp:Label ID="lbltodate" runat="server" Text="To Date :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txt_To_Date" runat="server" class="sinput"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CE_To_Date" runat="server" Enabled="True" TargetControlID="Txt_To_Date"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBE_To_Date" runat="server" Enabled="True" TargetControlID="Txt_To_Date"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>

                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblmonth" runat="server" Text="Month" Visible="false"></asp:Label>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEndDate" runat="server" class="sinput" Visible="false"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="calendar1"
                                                    Enabled="true" Format="MMM-yyyy" TargetControlID="txtEndDate" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                </cc1:CalendarExtender>

                                            </td>
                                            <td colspan="2">
                                                <asp:Button runat="server" ID="Btn_Search_Invoice_Btn_Dates" Text="Submit" class="btn save" Style="margin-left: 80px"
                                                    OnClick="Btn_Search_Invoice_Btn_Dates_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <%-- <asp:HiddenField ID="hidGridView" runat="server" />--%>
                                <div id="forExport" style="overflow: scroll; width: 960px;">
                                    <asp:GridView ID="GVInvoiceBills" runat="server" AutoGenerateColumns="true" CssClass="table table-striped table-bordered table-condensed table-hover"
                                        CellPadding="4" ForeColor="#333333"
                                        Style="overflow: scroll; width: 950px">
                                        <Columns>
                                        </Columns>

                                    </asp:GridView>
                                    <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red">  </asp:Label>
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
