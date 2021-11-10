<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="LoanReports.aspx.cs" Inherits="SRF.P.Module_Reports.LoanReports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />

    <link href="css/boostrap/css/bootstrap.css" rel="stylesheet" />
    <script src="script/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="script/jscript.js"> </script>

    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>




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
    </script>
    <script type="text/javascript">
        debugger
        function GetEmpid() {

            $("#<%=txtemplyid.ClientID %>").autocomplete({
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

                    $("#<%=txtemplyid.ClientID %>").attr("data-Empid", ui.item.value); OnAutoCompletetxtEmpidchange(event, ui);
                }
            });
            }

            function GetEmpName() {

                $("#<%=txtFname.ClientID %>").autocomplete({
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
                        $("#<%=txtFname.ClientID %>").attr("data-EmpName", ui.item.value); OnAutoCompletetxtEmpNamechange(event, ui);
                    }
                });

                }

                function OnAutoCompletetxtEmpidchange(event, ui) {
                    $("#<%=txtemplyid.ClientID %>").trigger('change');

                }
                function OnAutoCompletetxtEmpNamechange(event, ui) {
                    $("#<%=txtFname.ClientID %>").trigger('change');

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

        .auto-style1 {
            width: 125%;
        }
    </style>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <%--<li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                    <li class="active"><a href="LoanReports.aspx" style="z-index: 7;" class="active_bread">LOANS</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">LOANS
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div style="margin-left: 20px">


                                    <div style="float: right">
                                        <asp:LinkButton ID="lbtn_Export" runat="server" Text="Export to Excel" OnClick="lbtn_Export_Click"></asp:LinkButton>
                                    </div>
                                    <div class="dashboard_firsthalf" style="width: 750px; margin-right: 120px;">
                                        <table cellpadding="5" cellspacing="5" class="auto-style1">
                                            <tr>
                                                <td>
                                                    <div style="margin-bottom: 32px">
                                                        <asp:Label runat="server" ID="lblLoanOperation" Text="Loan Operations:" Width="125px"></asp:Label>

                                                        <asp:DropDownList ID="ddlloanoperations" runat="server" Width="175px" OnSelectedIndexChanged="ddlloanoperations_OnSelectedIndexChanged">
                                                            <asp:ListItem>--Select--</asp:ListItem>
                                                            <asp:ListItem>Issued Loans</asp:ListItem>
                                                            <asp:ListItem>Pending Loans</asp:ListItem>
                                                            <asp:ListItem>Deducted Loans</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="margin-bottom: 32px;">
                                                        <asp:Label runat="server" ID="lblissuedloans" Text="Options:" Width="125px"></asp:Label>
                                                        <asp:DropDownList ID="ddlissuedloans" runat="server" Width="175px" AutoPostBack="true" OnSelectedIndexChanged="ddlissuedloans_OnSelectedIndexChanged">
                                                            <asp:ListItem>Month Wise</asp:ListItem>
                                                            <asp:ListItem>From-To</asp:ListItem>
                                                            <asp:ListItem>Employee Wise</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="margin-bottom: 32px;">
                                                        <asp:Label runat="server" ID="lblLoantype" Text="Loan Type :" Width="125px"></asp:Label>
                                                        <asp:DropDownList ID="ddlloantypes" runat="server" Width="175px">
                                                        </asp:DropDownList>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td>
                                                    <asp:Label runat="server" ID="lblSelectmonth" Text="Select Month:" Width="125px"></asp:Label>
                                                    <asp:TextBox ID="txtloanissue" runat="server" class="form-control" Width="145px"> </asp:TextBox>

                                                    <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                        Enabled="true" Format="MMM-yyyy" TargetControlID="txtloanissue" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                    </cc1:CalendarExtender>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblfrom" Text="From Month:" Width="125px" Visible="false"></asp:Label>
                                                    <asp:TextBox ID="txtfromdate" AutoComplete="off" class="form-control" runat="server" Width="145px" Visible="false"> </asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true"
                                                        TargetControlID="txtfromdate" Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                        TargetControlID="txtfromdate" ValidChars="/0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>

                                                <td>
                                                    <asp:Label runat="server" ID="lblto" Text="To Month:" Width="125px" Visible="false"></asp:Label>
                                                    <asp:TextBox ID="txttodate" AutoComplete="off" runat="server" Width="145px" class="form-control" Visible="false"> </asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="true"
                                                        TargetControlID="txttodate" Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                        TargetControlID="txttodate" ValidChars="/0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblempid" Visible="false" runat="server" Text="Emp ID"></asp:Label>
                                                    <asp:TextBox runat="server" Visible="false" ID="txtemplyid" class="form-control" AutoPostBack="true" OnTextChanged="txtemplyid_TextChanged"></asp:TextBox>
                                                </td>

                                                <td>
                                                    <asp:Label ID="lblEmployeeName" Visible="false" runat="server" Text="Name"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtFname" Visible="false" class="form-control" AutoPostBack="true" OnTextChanged="txtFname_TextChanged"></asp:TextBox>
                                                </td>
                                                 <td>
                                                    <div style="margin-left: 190px">
                                                        <asp:Button ID="Btn_Search_Loans" runat="server" OnClick="Btn_Search_Loans_Click" Text="Search"
                                                            Style="float: right" class="btn save" />
                                                    </div>
                                                </td>
                                            </tr>
                                            
                                        </table>


                                    </div>
                                    <div class="rounded_corners" style="overflow: auto">
                                        <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="100%"
                                            CssClass="datagrid" CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDataBound="GVListEmployees_RowDataBound" ShowFooter="true">
                                            <RowStyle BackColor="#EFF3FB" />
                                            <Columns>


                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Loan Id">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblloanno" Text='<%# Bind("LoanNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Loan Type">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblloantype" Text='<%# Bind("TypeofLoan") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="empid">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempid" Text='<%# Bind("EmpId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempmname" Text='<%# Bind("Fullname") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Designation">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lbldesignation" Text='<%# Bind("Designation") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Loan Amount">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblloanamount" Text='<%# Bind("LoanAmount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="No Of Installments">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEmpLastName" Text='<%# Bind("NoofInstalments") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- <asp:TemplateField HeaderText="Amount To Be Deducted">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblamounttobededucted" Text="<%# Bind('AmountTobeDeducted') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                                <asp:TemplateField HeaderText="Amount Deducted">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblamountdeducted" Text='<%# Bind("AmountDeducted") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Due Amount">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lbldueamount" Text='<%# Bind("DueAmount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Loan Issue Date">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblloanissuedate"
                                                            Text='<%#Eval("LoanIssedDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Loan Cutting Month">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblloancuttingmonth"
                                                            Text='<%#Eval("LoanCuttingMonth", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Loan Status">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEmpDesignation" Text='<%# Bind("LoanStatus") %>'></asp:Label>
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
                                <div>
                                    <table width="100%" style="display:none">
                                        <tr style="width: 100%; font-weight: bold">
                                            <td style="width: 50%" align="right">
                                                <asp:Label ID="lbltamttext" runat="server" Text="&nbsp;&nbsp;Total Amount :"></asp:Label>
                                            </td>
                                            <td style="width: 62%">
                                                <asp:Label ID="lbltmt" runat="server" Text="" Style="margin-left: 2%"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
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
</asp:Content>
