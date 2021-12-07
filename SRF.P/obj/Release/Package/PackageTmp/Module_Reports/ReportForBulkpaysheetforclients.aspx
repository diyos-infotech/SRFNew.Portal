<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="ReportForBulkpaysheetforclients.aspx.cs" Inherits="SRF.P.Module_Reports.ReportForBulkpaysheetforclients" %>

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


        function AssignExportHTML() {
            document.getElementById("ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder3_hidGridView").value =
                htmlEscape(ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder3_forExport.innerHTML);
        }
        function htmlEscape(str) {
            return String(str)
                .replace(/&/g, '&amp;')
                .replace(/"/g, '&quot;')
                .replace(/</g, '&lt;')
                .replace(/>/g, '&gt;');
        }

        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            //Get the reference of GridView
            var GridView = row.parentNode;

            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];

                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;

        }
    </script>

    <script type="text/javascript">

        function checkAll(objRef) {

            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        inputList[i].checked = false;
                    }
                }
            }
        }

    </script>


    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Paysheet Report</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Salary Details
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">

                                    <table width="95%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>Type
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddltype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                                    <asp:ListItem>Month</asp:ListItem>
                                                    <asp:ListItem>From-To</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td id="lblmonth" runat="server">Month
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtmonth" AutoComplete="off" runat="server" Text="" class="sinput"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                    TargetControlID="txtmonth" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="txtmonth"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td></td>


                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblfrommonth" runat="server" Text="From Month" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtfrommonth" AutoComplete="off" runat="server" Text="" class="sinput" Visible="false"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true"
                                                    TargetControlID="txtfrommonth" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="txtfrommonth"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>

                                            <td>
                                                <asp:Label ID="lbltomonth" runat="server" Text="To Month" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txttomonth" AutoComplete="off" runat="server" Text="" class="sinput" Visible="false"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true"
                                                    TargetControlID="txttomonth" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="txttomonth"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>

                                            <td>
                                                <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbloptions" runat="server" Text="Print Options"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlOptions" runat="server">
                                                    <asp:ListItem Text="Excel"></asp:ListItem>
                                                    <asp:ListItem Text="PDF"></asp:ListItem>
                                                    <asp:ListItem Text="Excel New"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>

                                            <td>
                                                <asp:Button ID="btnDownload" runat="server" Text="Download" class="btn save"
                                                    OnClick="btnDownload_Click" />
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lbtn_Export" runat="server" Visible="false" OnClick="lbtn_Export_Click" OnClientClick="AssignExportHTML()">Export to Excel</asp:LinkButton>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="8">
                                                <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"> </asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="rounded_corners" style="overflow: scroll">
                                    <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="100%"
                                        CellSpacing="3" CellPadding="5" ForeColor="#333333" GridLines="None">
                                        <RowStyle BackColor="#EFF3FB" Height="30" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkindividual" runat="server" onclick="Check_Click(this)" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Client Id">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclientid" runat="server" Text='<%#Eval("clientid") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclientname" runat="server" Text='<%#Eval("clientname") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Monthname" HeaderText="Month" />
                                            <asp:TemplateField HeaderText="Month">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblmonth" runat="server" Text='<%#Eval("month") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="TotalGross" HeaderText="Gross" DataFormatString="{0:0.00}" />
                                            <asp:BoundField DataField="TotalOTamt" HeaderText="OT AMount" DataFormatString="{0:0.00}" />
                                            <asp:BoundField DataField="TotalDeductions" HeaderText="Total Deductions" DataFormatString="&nbsp; {0}" />
                                            <asp:BoundField DataField="TotalNetAmount" HeaderText="NET AMount" DataFormatString="{0:0.00}" />
                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </div>


                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                                    CellSpacing="3" CellPadding="5" ForeColor="#333333" GridLines="None" OnRowDataBound="GridView1_RowDataBound" Visible="false">
                                    <RowStyle BackColor="#EFF3FB" Height="30" />
                                    <Columns>
                                        <asp:BoundField DataField="clientid" HeaderText="Client Id" />
                                        <asp:BoundField DataField="clientname" HeaderText="Name" />
                                        <asp:BoundField DataField="empid" HeaderText="Emp Id" />
                                        <asp:BoundField DataField="empmname" HeaderText="Name" />
                                        <asp:BoundField DataField="design" HeaderText="Name" />
                                        <asp:TemplateField HeaderText="Month/Year">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmonth" runat="server" Text='<%# Bind("Monthname") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="NoOfDuties" HeaderText="No Of Duties" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="ot" HeaderText="No Of OT's" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="basic" HeaderText="Basic" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="da" HeaderText="DA" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="HRA" HeaderText="HRA" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="CCA" HeaderText="CCA" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="Conveyance" HeaderText="Conv." DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="WA" HeaderText="W.A" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="OA" HeaderText="O.A" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="LeaveEncashAmt" HeaderText="NHAmt" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="LeaveEncashAmt" HeaderText="L.A.Amt" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="OTAmt" HeaderText="OTAmt" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="clplamt" HeaderText="CLPLAmt" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="woamt" HeaderText="W.O.Amt" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="totalgross" HeaderText="Gross" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="pf" HeaderText="PF" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="esi" HeaderText="ESI" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="Proftax" HeaderText="PT Amt" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="owf" HeaderText="LWF" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="saladvded" HeaderText="SALADVDED" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="UniformDed" HeaderText="UniformDed" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="woamt" HeaderText="Reg Ded" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="woamt" HeaderText="Ins Ded" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="OtherDed" HeaderText="OtherDed" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="CanteenAdv" HeaderText="CanteenAdv" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="penalty" HeaderText="Penalty" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="TotalDeductions" HeaderText="Total Ded" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="actualamount" HeaderText="Net AMount" DataFormatString="{0:0.00}" />

                                        <asp:TemplateField HeaderText="Bank A/C No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbankno" runat="server" Text='<%# Eval("Empbankacno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>



                                        <asp:BoundField DataField="EmpBankCardRef" HeaderText="Reference No." DataFormatString="{0}&nbsp;" />
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>

                                <asp:HiddenField ID="hidGridView" runat="server" />
                                <div id="forExport" class="rounded_corners" runat="server" style="overflow: scroll">
                                    <asp:GridView ID="GVListEmployeesNew" runat="server" AutoGenerateColumns="False" CellPadding="10" Style="margin: 0px auto; margin-top: 10px;"
                                        OnRowDataBound="GVListEmployeesNew_RowDataBound">
                                        <Columns>
                                            <%-- 0--%>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>



                                            <%-- 3--%>
                                            <asp:TemplateField HeaderText="Emp Id" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblempid" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 4--%>
                                            <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text=" Name : " Style="font-weight: bold"></asp:Label>
                                                    <asp:Label ID="lblempname" runat="server" Text='<%#Bind("FullName") %>'></asp:Label><br />

                                                    <asp:Label ID="lblepfNo1" runat="server" Text=" EPF No : " Style="font-weight: bold"></asp:Label>
                                                    <asp:Label ID="lblepfNo1v" runat="server" Text='<%#Bind("EmpEpfNo") %>'></asp:Label><br />

                                                    <asp:Label ID="lblesiNo" runat="server" Text=" ESI No : " Style="font-weight: bold"></asp:Label>
                                                    <asp:Label ID="lblesiNov" runat="server" Text='<%#Bind("EmpEsiNo") %>'></asp:Label><br />

                                                    <asp:Label ID="Label2" runat="server" Text="UAN No : " Style="font-weight: bold"></asp:Label>
                                                    <asp:Label ID="Label3" runat="server" Text='<%#Bind("EmpUANNumber") %>'></asp:Label><br />


                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>

                                             <asp:TemplateField HeaderText="Client Id">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclientid" runat="server" Text='<%#Eval("clientid") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclientname" runat="server" Text='<%#Eval("clientname") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 5--%>
                                            <asp:TemplateField HeaderText="Desgn" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldesgn" runat="server" Text='<%#Bind("Desgn") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>



                                            <%-- 7--%>
                                            <asp:TemplateField HeaderText="Duties" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldutyhrs" runat="server" Text='<%#Bind("NoOfDuties") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalDuties" Text=""></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 8--%>
                                            <asp:TemplateField HeaderText="ED" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOts" runat="server" Text='<%#Bind("OTs") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalOTs"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 12--%>
                                            <asp:TemplateField HeaderText="Sal Rate" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltempgross" runat="server" Text='<%#Bind("TempGross") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotaltempgross"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>




                                            <%-- 13--%>

                                            <asp:TemplateField HeaderText="Basic" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%-- <asp:Label ID="lblbasic" runat="server" Text='<%#Bind("basic") %>'>--%>
                                                    <asp:Label ID="lblbasic" runat="server" Text='<%#Eval("basic", "{0:0}") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalBasic"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 14--%>
                                            <asp:TemplateField HeaderText="DA" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblda" runat="server" Text='<%#Eval("da","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalDA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 15--%>
                                            <asp:TemplateField HeaderText="HRA" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblhra" runat="server" Text='<%#Bind("hra","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalHRA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 16--%>
                                            <asp:TemplateField HeaderText="CCA" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcca" runat="server" Text='<%#Bind("CCa","{0:0}") %>'>  
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalCCA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 17--%>
                                            <asp:TemplateField HeaderText="Conv" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblConveyance" runat="server" Text='<%#Bind("conveyance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalConveyance"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 18--%>
                                            <asp:TemplateField HeaderText="W.A." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblwashallowance" runat="server" Text='<%#Bind("WashAllowance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalWA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 19--%>
                                            <asp:TemplateField HeaderText="O.A." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOtherallowance" runat="server" Text='<%#Bind("OtherAllowance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalOA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 20--%>
                                            <asp:TemplateField HeaderText="L.W" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLeaveEncashAmt" runat="server" Text='<%#Bind("LeaveEncashAmt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalLeaveEncashAmt"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 21--%>
                                            <asp:TemplateField HeaderText="Gratuity" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGratuity" runat="server" Text='<%#Bind("Gratuity","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalGratuity"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 22--%>
                                            <asp:TemplateField HeaderText="Bonus" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBonus" runat="server" Text='<%#Bind("Bonus","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalBonus"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 23--%>
                                            <asp:TemplateField HeaderText="Nfhs" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNfhs" runat="server" Text='<%#Bind("Nfhs","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNfhs"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 24--%>
                                            <asp:TemplateField HeaderText="RC" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblrc" runat="server" Text='<%#Bind("rc","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalrc"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 25--%>
                                            <asp:TemplateField HeaderText="CS" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcs" runat="server" Text='<%#Bind("cs","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcs"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 26--%>

                                            <asp:TemplateField HeaderText="Incentivs" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIncentivs" runat="server" Text='<%#Bind("Incentivs","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalIncentivs"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 27--%>

                                            <asp:TemplateField HeaderText="WO Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWoAmt" runat="server" Text='<%#Bind("WOAmt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalWOAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 28--%>

                                            <asp:TemplateField HeaderText="NHs Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNhsAmt" runat="server" Text='<%#Bind("Nhsamt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNhsAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 29--%>

                                            <asp:TemplateField HeaderText="NPOTs Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNpotsAmt" runat="server" Text='<%#Bind("Npotsamt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNpotsAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>



                                            <%-- 30--%>
                                            <asp:TemplateField HeaderText="Gross" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGross" runat="server" Text='<%#Bind("Gross","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalGross"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 31--%>

                                            <asp:TemplateField HeaderText="OT Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOTAmt" runat="server" Text='<%#Bind("OTAmt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalOTAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>



                                            <%-- 32--%>
                                            <asp:TemplateField HeaderText="PF" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPF" runat="server" Text='<%#Bind("PF","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalPF"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 33--%>
                                            <asp:TemplateField HeaderText="ESI" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblESI" runat="server" Text='<%#Bind("ESI","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalESI"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 34--%>
                                            <asp:TemplateField HeaderText="ProfTax" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProfTax" runat="server" Text='<%#Bind("ProfTax","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalProfTax"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 35--%>
                                            <asp:TemplateField HeaderText="Sal.Adv" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblsaladv" runat="server" Text='<%#Bind("SalAdvDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalsaladv"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 36--%>
                                            <asp:TemplateField HeaderText="Uniform" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbluniform" runat="server" Text='<%#Bind("UniformDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalUniformDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>



                                            <%-- 37--%>
                                            <asp:TemplateField HeaderText="Other Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOtherDed" runat="server" Text='<%#Bind("OtherDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalOtherDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 38--%>
                                            <asp:TemplateField HeaderText="Room Rent" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRoomRentDed" runat="server" Text='<%#Bind("RoomRentDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalRoomRentDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 39--%>
                                            <asp:TemplateField HeaderText="Mess Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcantadv" runat="server" Text='<%#Bind("CanteenAdv","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalcantadv"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 40--%>
                                            <asp:TemplateField HeaderText="Sec Dep" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSecDepDed" runat="server" Text='<%#Bind("SecurityDepDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalSecDepDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 41--%>
                                            <asp:TemplateField HeaderText="Gen Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGeneralDed" runat="server" Text='<%#Bind("GeneralDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalGeneralDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>



                                            <%-- 42--%>
                                            <asp:TemplateField HeaderText="LWF" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblowf" runat="server" Text='<%#Bind("OWF","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalowf"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 43--%>
                                            <asp:TemplateField HeaderText="Rent" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPenalty" runat="server" Text='<%#Bind("Penalty","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalPenalty"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>



                                            <%-- 44--%>
                                            <asp:TemplateField HeaderText="Total Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeductions" runat="server" Text='<%#Bind("TotalDeductions","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalDeductions"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 45--%>
                                            <asp:TemplateField HeaderText="Net Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblnetamount" runat="server" Text='<%#Bind("ActualAmount","{0:0}") %>'> </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNetAmount" Text=""></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 46--%>
                                            <asp:TemplateField HeaderText="Bank A/c No./ Signature" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                <HeaderStyle Width="15px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpBankAcNo" runat="server" Text='<%#Bind("EmpBankAcNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>



                                        </Columns>

                                    </asp:GridView>
                                </div>

                                <%--     <div>
                            
                            <table width="100%">
                            <tr style=" width:100%; font-weight:bold">
                            <td  style=" width:10%" >
                            <asp:Label ID="lbltamttext" runat="server" Visible="false" Text="Total Amount"></asp:Label>
                            </td>
                            
                            <td style=" width:70%" >
                          <asp:Label ID="lblstrength" runat="server" Text="" style=" margin-left:16%"></asp:Label>
                             <asp:Label ID="lblgross" runat="server" Text=""  style=" margin-left:13%"></asp:Label>
                              <asp:Label ID="lblbasicda" runat="server" Text="" style=" margin-left:5%"></asp:Label>
                             <asp:Label ID="lblpfemp" runat="server" Text=""  style=" margin-left:8%"></asp:Label>
                            <asp:Label ID="lblpfempr" runat="server" Text=""  style=" margin-left:8%"></asp:Label>
                              <asp:Label ID="lbltotal" runat="server" Text="" style=" margin-left:8%" ></asp:Label>
                             
                             </td>
                            </tr>
                            </table>
                            </div>--%>
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
