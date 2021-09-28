<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="EmployeewiseESIPFPTReports.aspx.cs" Inherits="SRF.P.Module_Reports.EmployeewiseESIPFPTReports" %>

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

    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Employee wise ESI/PF/PT</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Employee wise ESI/PF/PT
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <div align="right">
                                        <asp:LinkButton ID="linkback" runat="server" OnClick=" lbtn_Back_Click">Back</asp:LinkButton>
                                    </div>
                                    <div align="right">
                                        <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="False">Export to Excel</asp:LinkButton>
                                    </div>
                                    <table width="80%">
                                        <td>Month</td>
                                        <td>
                                            <asp:TextBox ID="txtmonth" runat="server" Text="" class="sinput"></asp:TextBox>
                                            <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </table>

                                    <table width="100%">

                                        <tr style="width: 30%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Lblempid" runat="server" Text="Employee ID " Visible="false"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlEmployee" class="sdrop" Visible="false"
                                                        OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblempname" runat="server" Text=" Employee Name " Visible="false"></asp:Label></td>
                                                <td>
                                                    <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlempname" class="sdrop" Visible="false"
                                                        OnSelectedIndexChanged="ddlempname_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>

                                            </tr>


                                            <td>
                                                <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                            </td>
                                        </tr>
                                        <tr style="width: 100%">
                                            <td colspan="6">
                                                <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"> </asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>


                                <div class="rounded_corners">
                                    <div style="overflow: scroll; width: auto">
                                        <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="100%"
                                            Height="50px" CellPadding="5" CellSpacing="3" ForeColor="#333333" GridLines="None">
                                            <RowStyle BackColor="#EFF3FB" Height="30" />
                                            <Columns>
                                                <asp:BoundField DataField="EmpId" HeaderText="EmpCode" DataFormatString="{0:0.00}" />
                                                <asp:BoundField DataField="EmpEpfNo" HeaderText="PFNo" DataFormatString="{0:0.00}" />
                                                <asp:BoundField DataField="EmpEsiNo" HeaderText="ESINO" DataFormatString="{0:0.00}" />
                                                <asp:TemplateField HeaderText="DOB">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lbldob" Text="<%# Bind('EmpDtofBirth') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="EmpName">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEmpname" Text="<%# Bind('FullName') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="NoOfDuties" HeaderText="ShiftDays" DataFormatString="{0:0.00}" />
                                                <asp:BoundField DataField="ots" HeaderText="OTDays" DataFormatString="{0:0.00}" />
                                                <asp:BoundField DataField="PFWages" HeaderText="PFWages" DataFormatString="{0:0.00}" />
                                                <asp:BoundField DataField="pf" HeaderText="EmpPFAmt" DataFormatString="{0:0.00}" />
                                                <asp:BoundField DataField="PFEmpr" HeaderText="EmplrEPFAmt" DataFormatString="{0:0.00}" />
                                                <asp:BoundField DataField="EPSWAGES" HeaderText="EmplrEPSAmt" DataFormatString="{0:0.00}" />
                                                <asp:BoundField DataField="ESIWAGES" HeaderText="ESIWages" DataFormatString="{0:0.00}" />
                                                <asp:BoundField DataField="ESI" HeaderText="EmpESIAmt" DataFormatString="{0:0.00}" />
                                                <asp:BoundField DataField="ESIonOTWages" HeaderText="ESIonOTWages" DataFormatString="{0:0.00}" />
                                                <asp:BoundField DataField="ESIWAGES" HeaderText="OTEmpESIAmt" DataFormatString="{0:0.00}" />
                                                <asp:BoundField DataField="EmpConsESIWages" HeaderText="EmpConsESIWages" DataFormatString="{0:0.00}" />
                                                <asp:BoundField DataField="ESIonOTWages" HeaderText="EmpConsESIAmt" DataFormatString="{0:0.00}" />
                                                <asp:BoundField DataField="ProfTax" HeaderText="PTAmt" DataFormatString="{0:0.00}" />


                                                <%--         <asp:TemplateField HeaderText="UnitCode">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblclientid" Text="<%# Bind('clientid') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="UnitName">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblclientname" Text="<%# Bind('clientname') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                  
                                        <asp:TemplateField HeaderText="Employer Pf">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblerpf" Text="<%# Bind('emprpf') %>"></asp:Label>
                                            </ItemTemplate>
                                        
                                        </asp:TemplateField>--%>
                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                    </div>


                                    <div>
                                        <table width="100%">
                                            <tr style="width: 100%; font-weight: bold">
                                                <td style="width: 60%">
                                                    <asp:Label ID="lbltamttext" runat="server" Visible="false" Text="Total Amount"></asp:Label>
                                                </td>
                                                <td style="width: 40%">
                                                    <asp:Label ID="lbltmtemppf" runat="server" Text=""></asp:Label>
                                                    <asp:Label ID="lbltemprpf" runat="server" Text="" Style="margin-left: 30%"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
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
