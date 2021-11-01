<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="EmpSummary.aspx.cs" Inherits="SRF.P.Module_Reports.EmpSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <style type="text/css">
        .style1 {
            width: 135px;
        }
    </style>
    <style type="text/css">
        .style1 {
            width: 135px;
        }
    </style>

    <script type="text/javascript">

        function dtval(d, e) {
            var pK = e ? e.which : window.event.keyCode;
            if (pK == 8) { d.value = substr(0, d.value.length - 1); return; }
            var dt = d.value;
            var da = dt.split('/');
            for (var a = 0; a < da.length; a++) { if (da[a] != +da[a]) da[a] = da[a].substr(0, da[a].length - 1); }
            if (da[0] > 31) { da[1] = da[0].substr(da[0].length - 1, 1); da[0] = '0' + da[0].substr(0, da[0].length - 1); }
            if (da[1] > 12) { da[2] = da[1].substr(da[1].length - 1, 1); da[1] = '0' + da[1].substr(0, da[1].length - 1); }
            if (da[2] > 9999) da[1] = da[2].substr(0, da[2].length - 1);
            dt = da.join('/');
            if (dt.length == 2 || dt.length == 5) dt += '/';
            d.value = dt;
        }

        function bindautofilldesgs() {
            $(".txtautofillempid").autocomplete({
                source: eval($("#hdempid").val()),
                minLength: 4
            });
        }

    </script>

    <script type="text/javascript">

        function GetEmpid() {

            $("#<%=txtEmpid.ClientID %>").autocomplete({
                source: function (request, response) {
                    var Url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                    var ajaxUrl = Url.substring(0, Url.lastIndexOf('/')) + "/Autocompletion.asmx/GetFormEmpIDs";
                    $.ajax({
                        url: ajaxUrl,
                        method: 'post',
                        contentType: 'application/json;charset=utf-8',

                        data: JSON.stringify({
                            term: request.term,
                        }),
                        datatype: 'json',
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (err) {
                            alert(err);
                        }
                    });
                },
                minLength: 4,
                select: function (event, ui) {

                    $("#<%=txtEmpid.ClientID %>").attr("data-Empid", ui.item.value); OnAutoCompletetxtEmpidchange(event, ui);
                }
            });
            }

            function GetEmpName() {

                $("#<%=txtName.ClientID %>").autocomplete({
                    source: function (request, response) {
                        var Url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                        var ajaxUrl = Url.substring(0, Url.lastIndexOf('/')) + "/Autocompletion.asmx/GetFormEmpNames";
                        $.ajax({

                            url: ajaxUrl,
                            method: 'post',
                            contentType: 'application/json;charset=utf-8',
                            data: JSON.stringify({
                                term: request.term
                            }),
                            datatype: 'json',
                            success: function (data) {
                                response(data.d);
                            },
                            error: function (err) {
                                alert(err);
                            }
                        });
                    },
                    minLength: 4,
                    select: function (event, ui) {
                        $("#<%=txtName.ClientID %>").attr("data-EmpName", ui.item.value); OnAutoCompletetxtEmpNamechange(event, ui);
                    }
                });

                }

                function OnAutoCompletetxtEmpidchange(event, ui) {
                    $("#<%=txtEmpid.ClientID %>").trigger('change');

                }
                function OnAutoCompletetxtEmpNamechange(event, ui) {
                    $("#<%=txtName.ClientID %>").trigger('change');

                    }

                    $(document).ready(function () {

                        GetEmpid();
                        GetEmpName();
                    });



    </script>



    <style type="text/css">
        .style1 {
            width: 135px;
        }

        .completionList {
            background: white;
            border: 1px solid #DDD;
            border-radius: 3px;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
            min-width: 165px;
            height: 120px;
            overflow: auto;
        }

        .listItem {
            display: block;
            padding: 5px 5px;
            border-bottom: 1px solid #DDD;
        }

        .itemHighlighted {
            color: black;
            background-color: rgba(0, 0, 0, 0.1);
            text-decoration: none;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
            border-bottom: 1px solid #DDD;
            display: block;
            padding: 5px 5px;
        }
    </style>


    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="EmployeeReports.aspx" style="z-index: 8;">Employee Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Emp Summary Report</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Emp Summary Report
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div align="right">
                                    <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="False">Export to Excel</asp:LinkButton>
                                </div>

                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table width="80%" cellpadding="5" cellspacing="5">


                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblempid" Text="Emp ID" Width="60px"></asp:Label></td>

                                            <td>
                                                <asp:TextBox ID="txtEmpid" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtEmpid_TextChanged"></asp:TextBox>

                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblempname" Text="Emp Name" Width="80px"></asp:Label>

                                            </td>
                                            <td>

                                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtName_TextChanged"></asp:TextBox>

                                            </td>
                                            <td>Type
                                            </td>
                                            <td>

                                                <asp:DropDownList ID="ddlloantype" runat="server" Width="125px" CssClass="form-control">
                                                    <asp:ListItem>All</asp:ListItem>
                                                    <asp:ListItem>Salary Advance</asp:ListItem>
                                                    <asp:ListItem>Uniform</asp:ListItem>
                                                    <asp:ListItem>Security Deposit</asp:ListItem>
                                                    <asp:ListItem>Other Deduction</asp:ListItem>
                                                    <asp:ListItem>Room Rent</asp:ListItem>

                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btn_Submit_Click" />
                                            </td>
                                        </tr>
                                        <tr style="visibility: hidden">
                                            <td>From Date<span style="color: Red">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStrtDate" runat="server" class="sinput"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                    TargetControlID="txtStrtDate" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEstartdate" runat="server" Enabled="True" TargetControlID="txtStrtDate"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>

                                            <td>To Date<span style="color: Red">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEndDate" runat="server" class="sinput"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtEndDate"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEEnddate" runat="server" Enabled="True" TargetControlID="txtEndDate"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>


                                        </tr>



                                    </table>

                                </div>
                                <div class="rounded_corners" style="overflow: scroll">
                                    <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="100%" OnRowDataBound="GVListEmployees_RowDataBound"
                                        Height="50px" CellPadding="5" CellSpacing="3" ForeColor="#333333" GridLines="None">
                                        <RowStyle BackColor="#EFF3FB" Height="30" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Unit Name">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblEmpMiddleName" Text='<%# Bind("clientid") %>'></asp:Label>
                                                    <asp:Label runat="server" ID="lblUnitName" Text='<%# Bind("clientname") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Monthn" Visible="false" HeaderStyle-CssClass="visibility">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblmonthn" Text='<%# Bind("month") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Month">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblmonth" Text=""></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Grade">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblDesignation" Text='<%# Bind("Design") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Duties">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblDuties" Text='<%# Bind("NoOfDuties") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="OTs">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblOts" Text='<%# Bind("ots") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="NHs">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblNFhs" Text='<%# Bind("nhs") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Gross">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblgross" Text='<%# Bind("gross") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="OT Amt">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblOTAmt" Text='<%# Bind("otamt") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="PF">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblPF" Text='<%# Bind("pf") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="ESI">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblESI" Text='<%# Bind("esi") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="PT">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblPT" Text='<%# Bind("proftax") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Sal Adv">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblSalAdv" Text='<%# Bind("saladvded") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Uniform Ded">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblUniformded" Text='<%# Bind("uniformded") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Room Rent Ded">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblRoomRentDed" Text='<%# Bind("roomrentded") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Other Ded">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblOtherDed" Text='<%# Bind("otherded") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Mess Ded">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCanteenAdv" Text='<%# Bind("CanteenAdv") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Security Dep Ded">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblSecDepDed" Text='<%# Bind("securitydepded") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="General Ded">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblGeneralDed" Text='<%# Bind("Generalded") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="LWF">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblLWF" Text='<%# Bind("owf") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Total Deductions">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblTotalDeductions" Text='<%# Bind("TotalDeductions") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Actual Amount">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblActualAmount" Text='<%# Bind("ActualAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--<asp:BoundField HeaderText="Oder Date " DataField="orderdt" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField HeaderText="Date Of Joining" DataField="joiningdt" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField HeaderText="Date Of Leaving" DataField="relievedt" DataFormatString="{0:dd/MM/yyyy}" />--%>
                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </div>

                                <asp:Panel ID="pnlloans" runat="server" Visible="false">
                                    <table width="100%">
                                        <tr>
                                            <td align="center" style="font-weight: bold">
                                                <asp:Label ID="lblloanissued" runat="server" Text="Loan Issued" Visible="false"></asp:Label></td>
                                            <td align="center" style="font-weight: bold">
                                                <asp:Label ID="lblloandeducted" runat="server" Text="Loan Deducted" Visible="false"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <div class="rounded_corners">

                                                    <asp:GridView ID="GVLoanIssued" runat="server" AutoGenerateColumns="False" Width="100%" ShowFooter="true"
                                                        Height="50px" CellPadding="5" CellSpacing="3" ForeColor="#333333" GridLines="None" OnRowDataBound="GVLoanIssued_RowDataBound">
                                                        <RowStyle BackColor="#EFF3FB" Height="30" />
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Loan No">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblloanno" Text="<%# Bind('LoanNo') %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Loan Type">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblLoanType" Text="<%# Bind('TypeOfLoan') %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Issued Amount" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblissuedAmount" Text="<%# Bind('LoanAmount') %>"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label runat="server" ID="lblTotalissuedAmount"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="No.of Instalments" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblnoofinstalments" Text="<%# Bind('NoInstalments') %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblStatus" Text="<%# Bind('LoanStatus') %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Issued Date">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblissueddate" Text="<%# Bind('LoanIssuedDate') %>"></asp:Label>
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
                                            </td>


                                            <td valign="top">
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="GVLoanDeducted" runat="server" AutoGenerateColumns="False" Width="100%" ShowFooter="true"
                                                        Height="50px" CellPadding="5" CellSpacing="3" ForeColor="#333333" GridLines="None" OnRowDataBound="GVLoanDeducted_RowDataBound">
                                                        <RowStyle BackColor="#EFF3FB" Height="30" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Loan No">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lbldedloanno" Text="<%# Bind('LoanNo') %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Loan Type">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="Lbldedloantype" Text="<%# Bind('LoanType') %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Deducted Amount" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="LblDeductedAmount" Text="<%# Bind('RecAmt') %>"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label runat="server" ID="lblTotalDeductedAmount"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Client ID/Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="LblClintidorname" Text="<%# Bind('ClientID') %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Month">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="Lblmonth" Text="<%# Bind('LoanCuttingMonth') %>" Width="50px"></asp:Label>
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
                                            </td>
                                        </tr>
                                    </table>

                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblbalance" runat="server" Text="Balance" Style="font: Bold" Enabled="false"> </asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="txtbalance" runat="server" Text="" class="sinput"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>

                                </asp:Panel>

                                <div style="margin-top: 20px; margin-left: 20px">
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
    </div>
</asp:Content>
