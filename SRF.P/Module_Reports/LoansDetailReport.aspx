<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="LoansDetailReport.aspx.cs" Inherits="SRF.P.Module_Reports.LoansDetailReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <link href="../css/Load.css" rel="stylesheet" type="text/css" />

       <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                  <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                        <li><a href="EmployeeReports.aspx" style="z-index: 8;">Employee Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">LOAN DETAILS</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Loan Details
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <div align="right">
                                        <asp:LinkButton ID="lbtn_Export" runat="server" Visible="false" OnClick="Lnkbtnexcel_Click">Export to Excel</asp:LinkButton>
                                        
                                         </div>
                                    <table width="60%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            
                                            <td>
                                                Month :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtmonth" runat="server" Text="" class="sinput"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                    TargetControlID="txtmonth" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="txtmonth"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                Type
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddltype" runat="server" CssClass="sdrop">
                                                    <asp:ListItem>Loan Details</asp:ListItem>
                                                    <%--<asp:ListItem>Loans Deducted</asp:ListItem>--%>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btn_Submit_Click" />
                                            </td>
                                        </tr>
                                       
                                    </table>
                                    </div>
                                    
                                    
                                    <div class="rounded_corners">
                                        <div style="width: auto">
                                            <asp:GridView ID="GVListOfEmployees" runat="server" AutoGenerateColumns="False" Width="100%"
                                                CssClass="datagrid" CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None" ShowFooter="true" OnRowDataBound="GVListOfEmployees_RowDataBound">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                           
                                                     <asp:BoundField DataField="EmpId" HeaderText="Emp ID" />

                                                     <asp:BoundField DataField="EmpName" HeaderText="Emp Name" />

                                                     <asp:BoundField DataField="LoanNo" HeaderText="Loan No" nulldisplaytext="0" />
                                                     <asp:BoundField DataField="LoanDt" HeaderText="Loan Date" nulldisplaytext="0" />
                                                     <asp:BoundField DataField="LoanAmount" HeaderText="Loan Amt" nulldisplaytext="0" />
                                                     <asp:BoundField DataField="PaidAmnt" HeaderText="Paid Amt" nulldisplaytext="0" />
    
                                                  
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
