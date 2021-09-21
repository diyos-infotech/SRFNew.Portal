<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="EmpWiseLoanDetails.aspx.cs" Inherits="SRF.P.Module_Reports.EmpWiseLoanDetails" %>
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
                        <li><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                        <li class="active"><a href="EmpBioData.aspx" style="z-index: 7;" class="active_bread">EMP Wise Loan Details</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">EMP Wise Loan Details
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                    <div>

                                        <div>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblempid" Text="Emp ID" Width="60px"></asp:Label></td>
                                                            <td>

                                                                <%--<asp:DropDownList ID="ddlEmpID" runat="server" Width="150px" OnSelectedIndexChanged="ddlEmpID_SelectedIndexChanged"
                                                            AutoPostBack="True" TabIndex="1" AutoCompleteMode="SuggestAppend" CssClass="sinput" Height="25px">
                                                        </asp:DropDownList>--%>
                                                                <asp:TextBox ID="txtEmpid" runat="server" CssClass="sinput" AutoPostBack="true" OnTextChanged="txtEmpid_TextChanged"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="EmpIdtoAutoCompleteExtender" runat="server"
                                                                    ServiceMethod="GetEmpID"
                                                                    ServicePath="AutoCompleteAA.asmx"
                                                                    MinimumPrefixLength="4"
                                                                    CompletionInterval="100"
                                                                    EnableCaching="true"
                                                                    TargetControlID="TxtEmpID"
                                                                    FirstRowSelected="false"
                                                                    CompletionListCssClass="completionList"
                                                                    CompletionListItemCssClass="listItem"
                                                                    CompletionListHighlightedItemCssClass="itemHighlighted">
                                                                </cc1:AutoCompleteExtender>

                                                            </td>
                                                            <td></td>

                                                            <td>
                                                                <asp:Label runat="server" ID="lblempname" Text="Name" Width="50px"></asp:Label></td>

                                                            <td>
                                                                <%--<asp:DropDownList ID="ddlempname" runat="server" Width="150px" OnSelectedIndexChanged="ddlempname_SelectedIndexChanged" AutoPostBack="True" TabIndex="2"
                                                            AutoCompleteMode="SuggestAppend" CssClass="sinput" Height="25px">
                                                        </asp:DropDownList>--%>

                                                                <asp:TextBox ID="txtName" runat="server" CssClass="sinput" AutoPostBack="true" OnTextChanged="txtName_TextChanged"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="EmpNameAutoCompleteExtender" runat="server"
                                                                    ServiceMethod="GetEmpName"
                                                                    ServicePath="AutoCompleteAA.asmx"
                                                                    MinimumPrefixLength="4"
                                                                    CompletionInterval="100"
                                                                    EnableCaching="true"
                                                                    TargetControlID="txtName"
                                                                    FirstRowSelected="false"
                                                                    CompletionListCssClass="completionList"
                                                                    CompletionListItemCssClass="listItem"
                                                                    CompletionListHighlightedItemCssClass="itemHighlighted">
                                                                </cc1:AutoCompleteExtender>

                                                            </td>
                                                            <td></td>
                                                            <td>Month</td>
                                                            <td>
                                                                <asp:TextBox ID="txtmonth" runat="server" Text="" class="sinput"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                                    Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnsudmit" runat="server" Text="Submit"
                                                                    Style="margin-left:30px" class="btn save" OnClick="btnsudmit_Click"/>
                                                            </td>

                                                        </tr>
                                                    </table>
                                                
                                                <div class="rounded_corners" style="overflow:auto">
                                                    <asp:GridView ID="gvresources" runat="server" Width="100%" CellPadding="4" AutoGenerateColumns="false"
                                                        ForeColor="#333333" GridLines="None" >
                                                        <RowStyle BackColor="#EFF3FB" />
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Emp Id">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblempid" Text='<%# Bind("EmpId") %> '></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Emp Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblempname" Text='<%# Bind("FullName") %> '></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Uniform Id">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lbluniformid" Text='<%# Bind("uniformid") %> '></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Loan Id">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblloanid" Text='<%# Bind("LoanNo") %> '></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>



                                                            <asp:TemplateField HeaderText="Item">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblitem" Text='<%# Bind("ItemName") %> '></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Price">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblprice" Text='<%# Bind("Price") %> '></asp:Label>
                                                                </ItemTemplate>
                                                              
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Quantity">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblqty" Text='<%# Bind("qty") %> '></asp:Label>
                                                                </ItemTemplate>
                                                               
                                                            </asp:TemplateField>

                                                          <%--  <asp:TemplateField HeaderText="Total Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblttlamt" Text='<%# Bind("LoanAmount") %> '></asp:Label>
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
                                                </div>

                                            

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