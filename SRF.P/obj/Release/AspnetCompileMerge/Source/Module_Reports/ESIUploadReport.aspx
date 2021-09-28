<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="ESIUploadReport.aspx.cs" Inherits="SRF.P.Module_Reports.ESIUploadReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Load.css" rel="stylesheet" type="text/css" />

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
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">ESI Upload</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">ESI Upload
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <div align="right">
                                        <asp:LinkButton ID="linkback" runat="server" OnClick="lbtn_Back_Click">Back</asp:LinkButton>
                                    </div>
                                    <div align="right">
                                        <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>
                                        <asp:LinkButton ID="lbtn_Export_PDF" runat="server" Visible="false" OnClick="lbtn_Export_PDF_Click">Export to PDF</asp:LinkButton>


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
                                            <td>Month :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtmonth" runat="server" Text="" class="sinput"></asp:TextBox>
                                                <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                    Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                </cc1:CalendarExtender>
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
                                    <div style="overflow: scroll; width: auto">
                                        <asp:GridView ID="GVListOfClients" runat="server" AutoGenerateColumns="False" Width="100%"
                                            CssClass="datagrid" CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None">
                                            <RowStyle BackColor="#EFF3FB" />
                                            <Columns>


                                                <asp:TemplateField HeaderText="EmpCode">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEmpid" Text="<%# Bind('empid') %> "></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="ESINo">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEsino" Text="<%# Bind('EmpESINo') %> "></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="EmpName">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEsino" Text="<%# Bind('Fullname') %> "></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="ESIWages">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblesiwages" Text="<%# Bind('ESIWAGES') %> "></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="ESIonOTWages">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblESIonOTWages" Text="<%# Bind('ESIonOTWages') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ESIConsWages">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblESIConsWages" Text="<%# Bind('EmpConsESIWages') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DutyDays">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblDutyDays" Text="<%# Bind('noofduties') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="OTDays">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblOTDays" Text="<%# Bind('ots') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reason">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblReason" Text="<%# Bind('empty') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="LastWorkingDay">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblLastWorkingDay" Text="<%# Bind('empty') %>"></asp:Label>
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
