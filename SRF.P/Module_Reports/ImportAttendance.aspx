<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="ImportAttendance.aspx.cs" Inherits="SRF.P.Module_Reports.ImportAttendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">


    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Calendar.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #social div {
            display: block;
        }

        .HeaderStyle {
            text-align: Left;
        }

        .style1 {
            width: 106px;
        }
    </style>


    <script type="text/javascript">
        function AssignExportHTML() {

            document.getElementById('hidGridView').value = htmlEscape(forExport.innerHTML);
        }
        function htmlEscape(str) {
            return String(str)
                .replace(/&/g, '&amp;')
                .replace(/"/g, '&quot;')
                .replace(/'/g, '&#39;')
                .replace(/</g, '&lt;')
                .replace(/>/g, '&gt;');
        }

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
            <h1 class="dashboard_heading"></h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_full">
                    <div class="sidebox">
                        <div>
                            <h4 style="text-align: right">
                                <asp:LinkButton ID="lnkImportfromexcel" Text="Export Sample Excel" runat="server"
                                    OnClick="lnkImportfromexcel_Click"></asp:LinkButton>
                            </h4>
                        </div>
                        <div class="boxhead">
                            <h2 style="text-align: center">Employee Attendance&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </h2>
                        </div>

                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px; min-height: 600px">
                            <!--  Content to be add here> -->

                            <div>

                                <table>
                                    <tr>
                                        <td style="width: 60px">Option</td>

                                        <td style="width: 100px">
                                            <asp:DropDownList runat="server" ID="ddloption" Width="125px" OnSelectedIndexChanged="ddloption_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem>Month Wise</asp:ListItem>
                                                <asp:ListItem>Client Wise</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>

                                        <td style="width: 20px"></td>

                                        <td style="width: 60px">Attendance Mode</td>

                                        <td style="width: 150px">
                                            <asp:DropDownList runat="server" ID="ddlAttendanceMode" Width="125px">
                                                <asp:ListItem>Full Attendance</asp:ListItem>
                                                <asp:ListItem>Individual Attendance</asp:ListItem>
                                            </asp:DropDownList></td>

                                        <td style="width: 20px"></td>
                                        <td style="width: 60px"></td>

                                    </tr>
                                    <tr style="position:relative;top:10px">
                                        <td style="width: 60px">Month</td>
                                        <td>

                                            <asp:TextBox ID="txtmonth" runat="server" Text="" class="sinput" Width="100px" AutoPostBack="True" OnTextChanged="txtmonth_TextChanged"></asp:TextBox>
                                            <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                            </cc1:CalendarExtender>

                                        </td>
                                        <td style="width: 20px"></td>
                                        <td style="width: 60px">
                                            <asp:Label ID="lblclientid" runat="server" Text="Client ID" Visible="false"></asp:Label>

                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlClientID" runat="server" Width="125px" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlClientID_SelectedIndexChanged" Visible="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 20px"></td>
                                        <td style="width: 80px">
                                            <asp:Label ID="lblclientname" runat="server" Text="Client Name" Visible="false"></asp:Label>

                                        </td>
                                        <td style="width: 150px">
                                            <asp:DropDownList ID="ddlCName" runat="server" AutoPostBack="True"
                                                Width="125px" OnSelectedIndexChanged="ddlCName_SelectedIndexChanged" Visible="false">
                                            </asp:DropDownList>

                                        </td>
                                        <td style="width: 20px"></td>
                                        <td style="width: 60px">
                                            <asp:Button ID="btnClear" runat="server" Text="Clear" class=" btn save" Visible="false"
                                                OnClick="btnClear_Click" />
                                        </td>



                                    </tr>
                                    <tr style="position:relative;top:12px">

                                        <td style="width: 60px">Select File: </td>
                                        <td style="width: 100px">
                                            <asp:FileUpload ID="fileupload1" runat="server" />
                                        </td>
                                        <td style="width: 20px"></td>
                                        <td style="width: 60px">
                                            <asp:Button ID="btnImport" runat="server" Text="Import Data" class=" btn save" OnClick="btnImport_Click" />
                                        </td>
                                        <td style="width: 100px">
                                            <asp:Button ID="btnExport" runat="server" Text="Unsaved" Style="background-color: #d9534f; border-color: #d43f3a;"
                                                OnClick="btnExport_Click" Visible="false" Width="98px" />
                                        </td>
                                        <td style="width: 20px"></td>
                                        <td style="width: 80px"></td>
                                        <td style="width: 150px"></td>
                                        <td style="width: 20px"></td>
                                        <td style="width: 60px"></td>

                                    </tr>


                                </table>

                            </div>

                            <br />


                            <div>

                                <asp:GridView ID="SampleGrid" runat="server" Width="100%"
                                    AutoGenerateColumns="False" CellPadding="2" CellSpacing="2"
                                    ForeColor="#333333" BorderStyle="Solid"
                                    BorderColor="Black" BorderWidth="0" GridLines="None" Visible="false"
                                    HeaderStyle-CssClass="HeaderStyle">
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>

                                        <asp:TemplateField HeaderText="Client Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblsClientid" Width="200px" Text=" "></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Emp Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsEmpid" runat="server" Text=" " Style="text-align: center" Width="50px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblsDesg" Text=" " Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                Duties
                                           <%-- <br />
                                                OTs--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--************************************************************************************************************************************--%>

                                        <%--Duties --%>
                                        <asp:TemplateField HeaderText="1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday1" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday1ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday2" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%--<br />
                                      <asp:Label  ID="txtsday2ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday3" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday3ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday4" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%--<br />
                                      <asp:Label  ID="txtsday4ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="5" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday5" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday5ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="6" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday6" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%--<br />
                                      <asp:Label  ID="txtsday6ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="7" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday7" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday7ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="8" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday8" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday8ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="9" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday9" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday9ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="10" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday10" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday10ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="11" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday11" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%--<br />
                                      <asp:Label  ID="txtsday11ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="12" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday12" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday12ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="13" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday13" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday13ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday14" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday14ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="15" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday15" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday15ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="16" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday16" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday16ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="17" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday17" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday17ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="18" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday18" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday18ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="19" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday19" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%--<br />
                                      <asp:Label  ID="txtsday19ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="20" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday20" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday20ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="21" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday21" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%--<br />
                                      <asp:Label  ID="txtsday21ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="22" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday22" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday22ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="23" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday23" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday23ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="24" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday24" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%--<br />
                                      <asp:Label  ID="txtsday24ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="25" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday25" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday25ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="26" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday26" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday26ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="27" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday27" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday27ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="28" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday28" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%--<br />
                                      <asp:Label  ID="txtsday28ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="29" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday29" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday29ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="30" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday30" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday30ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="31" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday31" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday31ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="OTs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsOTs" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Leaves" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsLeaves" Style="text-align: center" Width="5px" Text=" <%#Bind('day6')%>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Canteen Advance" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsCanAdv" Style="text-align: center" Width="5px" Text=" <%#Bind('day5')%>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Incentives" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsIncentives" Style="text-align: center" Width="5px" Text=" <%#Bind('day6')%>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Rent Ded" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsRentDed" Style="text-align: center" Width="5px" Text=" <%#Bind('day6')%>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>

                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>

                                <asp:GridView ID="grvSample2" runat="server" Width="100%"
                                    AutoGenerateColumns="False" CellPadding="2" CellSpacing="2"
                                    ForeColor="#333333" BorderStyle="Solid"
                                    BorderColor="Black" BorderWidth="0" GridLines="None" Visible="false"
                                    HeaderStyle-CssClass="HeaderStyle">
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>

                                        <asp:TemplateField HeaderText="Employee Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsEmpname" runat="server" Text=" " Style="text-align: center" Width="50px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Client Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsClientId" runat="server" Text=" " Style="text-align: center" Width="50px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Emp Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsEmpid" runat="server" Text=" " Style="text-align: center" Width="50px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblsDesg" Text=" " Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Duties" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsDuties" Style="text-align: center" Width="5px" Text=" <%#Bind('day1')%>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="OTs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsOTs" Style="text-align: center" Width="5px" Text=" <%#Bind('day2')%>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="WOs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsWos" Style="text-align: center" Width="5px" Text=" <%#Bind('day3')%>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="NHs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsNhs" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Leaves" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsLeaves" Style="text-align: center" Width="5px" Text=" <%#Bind('day6')%>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Canteen Advance" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsCanAdv" Style="text-align: center" Width="5px" Text=" <%#Bind('day5')%>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Incentives" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsIncentives" Style="text-align: center" Width="5px" Text=" <%#Bind('day6')%>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Rent Ded" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsRentDed" Style="text-align: center" Width="5px" Text=" <%#Bind('day6')%>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>

                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>



                            </div>


                            <br />
                            <br />

                            <div class="panel panel-default" runat="server" id="pnlAttSummary" visible="false" style="font-size: 14px; font-family: Calibri; width: 900px; border-color: #1dacd6; border-width: 4px; margin: 0px auto; box-shadow: rgba(0, 0, 0, 0.25) 0px 14px 28px, rgba(0, 0, 0, 0.22) 0px 10px 10px;">
                                <div class="panel-heading" style="font-weight: bold; background-color: #1dacd6">Attendance Summary</div>
                                <div class="panel-body" >

                                    <asp:GridView ID="gvattsummarydata" runat="server" AutoGenerateColumns="true" Width="100%" CssClass="table table-striped table-bordered table-condensed table-hover">
                                        <Columns></Columns>
                                    </asp:GridView>

                                </div>

                            </div>

                            <div class="panel panel-default" runat="server" id="pnlnotinsertdata" visible="false" style="font-size: 14px; position: relative; top: 20px; font-family: Calibri; width: 900px; border-color: #ff4040; border-width: 4px; margin: 0px auto; box-shadow: rgba(0, 0, 0, 0.25) 0px 14px 28px, rgba(0, 0, 0, 0.22) 0px 10px 10px;">
                                <div class="panel-heading" style="font-weight: bold; background-color: #ff4040">Unsaved data</div>
                                <div class="panel-body">

                                    <asp:GridView ID="gvnotinsert" runat="server" AutoGenerateColumns="true" ShowHeader="True" Style="background-color: white" CssClass="table table-striped table-bordered table-condensed table-hover">
                                        <Columns></Columns>
                                    </asp:GridView>

                                </div>
                            </div>

                        </div>
                    </div>
                    <!-- DASHBOARD CONTENT END -->
                </div>
            </div>
            <!-- CONTENT AREA END -->

        </div>
    </div>
</asp:Content>
